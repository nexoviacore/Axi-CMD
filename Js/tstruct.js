//-------------------List of functions in this file----------------------------------
//OnTstructLoad() -Function to fill the fields with data which is called on load of the tstruct page. 
//MainFocus(fldObj, fldIndex) -Function which is executed on focussing a tstruct field.
//MainBlur(fldObj, fldIndex) -Function which is executed on blur of a field.
//ValidateField(fldObj, fldIndex, fldType, fName) -Function which calls the validation functions for the fields.
//ValidateReservedWord(obj, alertMessage) -Function which validates the field value with the reserved words used in the application.
//ValidateFieldPattern(fldObj, fName) -Function which validates the field value for the given pattern definition.
//ValidateExpression(fldObj, fldno) -Function which validates the field value with the result of the evaluated expression.
//EvaluatePattern(fld, pattrn, fldIndex)- Evaluates patters, if no error message or user continues returns true else False.       
//NumericFldOnfocus(fieldObj, fieldValue) -Function which removes comma and selects the value in a numeric field on focus.
//NumericFldOnBlur(newcurrFldValue, obj, fldIndex) -Function which adds comma and decimal point to the value of the numeric field.
//fixit(value, dec) -Function to add decimal point to the value based on the no of digits given in dec.
//ComboFillDependents(fieldID, value) -Function which fills the dependent combo values from the combo arrays.
//DoSetDependents() -Function which calls the GetDependents function based on the blur action.
//CheckDependency(fieldID) -Function which checks for the dependents and based on their type calls the respective function.
//GetDependentFieldID(depField, parentFieldID, parentRowNo) -Function to construct the field id of the dependent field.
//GetDependents(fieldID) -Function which calls the GetDependents webservice for the given field.
//SuccChoicesgetDep(result, eventArgs) -Callback function from the GetDependents webservice. 
//CheckGrdisplayHead(tabNo, fldObj, fieldName) -Function which displays the values of the grid cells above the grid.
//DisplayGfirstcol(obj, name) -Code to display first 2 columns in the top of Grid
//Tstructhyperlink(obj) -Function to open the page on click of the hyperlink label. 
//GetHyperLinkParamStr(indx) -Function to get the parameter string for the hyper link label to open the tstruct page. 
//TsructPopup(hl, hltype, isPop, params) -Function to open the relevant page onclick of the label link. 
//GetTabData(tabNo) -Function to get the currently selected tab HTML and data from the server
//SuccessGetTabData(result, eventArgs) -Callback function from the CallLoadDcData service.
//AddRow(dcNo, calledFrom) -Function to add a row in the grid dc.
//RowtoDOM(a_frano, rno) -For Firefox grid innerHTML updation
//UpdateDOM(inputFld) -Funtion called from RowtoDom.
//CallEvaluate(dcNo, rowNo, fields) -Function to call evaluate for the newly added row.
//FillNewRowComboValues(dcNo, rowNo, fields) -Function which fills the combo values in the new row if the master row is 1 for the field.
//IsInMasterRow(fieldName) -Function which returns true if the field is in master row array.
//DeleteRow(dcNo, rowFrmNo,elem) -Function which calls the Delete Grid row function.
//DeleteGridRow(dcNo, rowFrmNo) -Function which deletes the given row from the given dc no.
//ClearDeletedFields(dcNo, rowFrmNo, nextRow, prevRow) -Function to clear the values in the deleted row fields.
//RemoveDeletedFields(fieldName) -Function to remove the deleted row fields from the field arrays.  
//DeletePopRowsForDc(dcNo, rowNo) -Function which deletes the rows from the pop grids for the current parent row.
//AddDeletedRowsToArray(dcNo, rowNo) -Function to add the deleted row to the deleted rows array.
//ResetSerialNo(dcNo, rowFrmNo, rowCnt) -Function to reset the serial numbers in the rows below the deleted row.
//FormSubmit() -Function which calls the SaveData webservice.
//ValidateBeforeSubmit() -Function to validate all the fields which cannot be left empty before saving.
//EnableSaveBtn(enable) -Function to toggle save button disabling.
//UploadFiles() -Function to get the attached filenames. 
//CheckPattern() -Function to verify the pattern of email,url etc.. 
//SucceededCallback(result, eventArgs) -Callback function which returns either successfull or error message on save.  
//OpenSearch(tid) -Function which opens the search window. 
//NewTstruct() -Function which opens a new tstsruct.
//DeleteTstruct() -Function which deletes a tstruct record.
//SucceededCallbackDelTst(result, eventArgs) -Callback function for DeleteDataXML.
//OpenPrint(tid) -Function to print the transaction.
//CancelTstruct() -Function is used to cancel a workflow transaction.
//CancelData() -Function called for deleting the cancelled transaction.
//SucceededCallbackOnCancel(result, eventArgs) -Callback function for DeleteDataXML
//ShowDc(a) -Function to expand or collapse the dc div.
//ShowAttach() -Function to attach files to the transaction.
//ProcessRow() -Function which opens the pdf window on click of pdf toolbar button.
//CallListView(tid) -Function to open the list view on click of listview button in the toolbar.
//UploadImg(a) -Function to upload a image.
//ViewComments() -Function to open the comments page of the particular workflow. 
//ValidateComments(remarks, actname) -Function to validate if the comments text area is empty.
//FindPosX(obj) -Function to find the left position of the given obj on screen.
//FindPosY(obj) -Function to find the top position of the given object on screen.
//FindPos() -Function to get the left and top position of the task button image on screen.
//ShowTaskList() -Function to show the task list on click of the task button in the toolbar.
//OnBlrLstItem(lst) -Function which sets the css on blur of Lst item.
//OnLnkFcs(ankr) -Function which sets the css on focus of link.
//OnLnkBlr(ankr) -Function which sets the css on blur of link.
//HideTaskList() -Function to hide the task list.
//OpenHistory(tid) -Function to open view history page.
//AlertNoAction() -Function to display no task defined if there is no action defined for a task button.
//SearchOpen(txtobj) -Function to open the relevant page onclick of picklist.
//SetSelectedValue(listcontrol, listctrl) -Function to set the field in the parent page from picklist page.
//UpdateParentArray() -Function to update field array from the fill grid page.
//CheckFields(btnobj) -Function to check if any of the field is updated, on click of a workflow action button. 
//SucceededCallbackAction(result, eventArgs) -Callback function for workflow action.
//GetWFButStatus() -Function to get the workflow status.
//EnableWFBut() -Function to enable the workflow buttons if workflow status is not 0.
//DisableWFButtons() -Function to disable the workflow action buttons.          
//SafariTabDcHeight() - function to set tabbed dc caption container to height:100%
//------------------------------------------------------------------------------------
///<reference path="tstructvars.js" />

//Function to refresh the middle1 when there is axpframe update
function ReloadMiddleIframe() {
    var parFrm = $j("#middle1", parent.document);
    if (parFrm.length > 0) {
        SetSession({
            key: "IsFromChildWindow",
            val: "true"
        })

        var iframe = parFrm[0];
        var iframewindow = iframe.contentWindow ? iframe.contentWindow : iframe.contentDocument.defaultView;
        if (callParentNew("isDWB") && iframewindow.location.href.toLowerCase().indexOf("iview.aspx") > -1) {
            parFrm.attr("src", `ivtoivload.aspx?ivname=${findGetParameter("ivname", iframewindow.location.href)}&AxIvNav=true`);
        } else {
            var thisHref = iframewindow.location.href;
            if (thisHref.indexOf("&AxSplit=") == -1)
                thisHref += "&AxSplit=true";
            parFrm.attr("src", thisHref);
        }
    }
}

var DesigntstructMessages = {
    "noNode": eval(callParent('lcm[363]')),
    "designSaved": eval(callParent('lcm[364]')),
    "designSaveFailed": eval(callParent('lcm[365]')),
    "noModification": eval(callParent('lcm[366]')),
    "saveBeforePublish": eval(callParent('lcm[367]')),
    "designPublished": eval(callParent('lcm[368]')),
    "designPublisgFailed": eval(callParent('lcm[369]')),
    "resetSuccess": eval(callParent('lcm[370]')),
    "alredayreset": eval(callParent('lcm[371]')),
    "resetfailed": eval(callParent('lcm[372]')),
    "structureChanged": eval(callParent('lcm[374]'))
}

function setDesignedLayout(setDiv, SaveDesign) {
    //page handler
    if (theMode == "design") {
        $(".grid-stack").addClass("tstructDesignMode");
        $("div[id^=colScroll]").addClass("tstructDesignMode");
        $(".grid-stack").addClass("staticRunMode").removeClass("dynamicRunMode");
        $('#designModeToolbar').removeClass("d-none");
        $('.toolbar .toolbarRightMenu,.footer').addClass("d-none");
        $('.gridIconBtns').addClass("d-none");
    } else {
        //hide 
        $("#DivFrame1 .dcTitle").hide().nextAll("hr.hrline").hide();
        try {
            if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].selectedLayout != null && designObj[0].selectedLayout != "" && designObj[0].selectedLayout == "tile") {
                $("#DivFrame1 .dcTitle").show().nextAll("hr.hrline").show();
            }
        } catch (ex) { }
        $(".grid-stack").removeClass("tstructDesignMode");
        $("div[id^=colScroll]").removeClass("tstructDesignMode");
    }

    //grid-stack
    if (theMode == "design") {

        if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].tstUpdatedOn != null && designObj[0].tstUpdatedOn != "" && designObj[0].tstUpdatedOn != tstructUpdateOn) {
            showAlertDialog("info", DesigntstructMessages.structureChanged);
        }

        if (typeof AxpTstButtonStyle != "undefined" && AxpTstButtonStyle == "old") {
            $("#searchBar.toolbarOptionsParent").addClass("d-none");
        }

        $(".grdButtons,.newgridbtn [id^=gridAddBtn]:visible, .grdButtons .dropdown, .grdButtons li").hide();
        $(".newgridbtn li").hide();
        $(".tstructMainBottomFooter").hide();
        $(".rightToolbarMain").hide();
        if (!axInlineGridEdit) {
            $(".grid-icons [id^=wrapperForEditFields]").show().removeClass("hide").find(".editLayoutFooter").hide();
        }
        unBindEvents();
        if (!$(setDiv).find(".grid-stack").data('gridstack')) {
            //$(setDiv).find(".grid-stack").gridstack(designGridOptions);

            //clearHiddenGridstack();
            if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].dcLayout != null && designObj[0].dcLayout != "" && designObj[0].dcLayout != "default") {
                designGridOptions.disableDrag = true;
                designGridOptions.disableResize = true;
            }
            $(setDiv).find(".grid-stack").each(function (ind, gsElem) {
                clearHiddenGridstack(gsElem.id);
                $("#ckbStaticRunMode").prop('checked', staticRunMode);
                $(gsElem).gridstack(designGridOptions);
                //$(gsElem).data('gridstack').batchUpdate();
                //clearHiddenGridstack(gsElem.id);
                //$(gsElem).data('gridstack').commit();
            });
        } else {
            $(setDiv).find(".grid-stack").each(function (ind, gsElem) {
                $(gsElem).data('gridstack').batchUpdate();
                clearHiddenGridstack(gsElem.id);
                $(gsElem).data('gridstack').enableMove(true, true);
                $(gsElem).data('gridstack').enableResize(true, true);
                $(gsElem).data('gridstack').commit();
            });
        }
    } else {
        if (!$(setDiv).find(".grid-stack").data('gridstack')) {
            //$(setDiv).find(".grid-stack").gridstack(renderGridOptions);

            //clearHiddenGridstack();

            $(setDiv).find(".grid-stack").each(function (ind, gsElem) {
                clearHiddenGridstack(gsElem.id);
                if (staticRunMode) {
                    $(gsElem).addClass("staticRunMode").removeClass("dynamicRunMode");
                    $(gsElem).gridstack(renderGridOptions);
                    $(gsElem).data('gridstack').batchUpdate();
                } else {
                    $(gsElem).removeClass("staticRunMode").addClass("dynamicRunMode");
                }
                $("#ckbStaticRunMode").prop('checked', staticRunMode);
                //$(gsElem).data('gridstack').batchUpdate();
                //clearHiddenGridstack(gsElem.id);
                //$(gsElem).data('gridstack').commit();





                if (staticRunMode) {
                    //auto arrange code to move first and fill gap
                    if (autoArrangeFillGap) {
                        _.map($(gsElem).data('gridstack').grid.nodes, function (el) {
                            el = el.el;
                            var node = el.data('_gridstack_node');
                            node = generateRtlPos(node);
                            var newNode = {
                                ...el.data('_gridstack_node')
                            };
                            var x = 0;
                            var y = node.y;
                            //to move y
                            for (var i = 0; i <= node.y; i++) {
                                y = i;
                                //var finalY = -1;
                                if ($(gsElem).data('gridstack').isAreaEmpty(x, y, renderGridOptions.width, 1)) {
                                    $(gsElem).data('gridstack').grid.nodes.filter(function (ell) {
                                        ells = ell.el;
                                        //if(finalY)
                                        //debugger;
                                        //if(ells.y >= y || y <= ell.y){
                                        //    $(gsElem).data('gridstack').move(ells, newNode.x, ell.y);
                                        //  //  finalY = y;
                                        //}
                                        if (ell.y == y + 1) {
                                            $(gsElem).data('gridstack').move(ells, generateRtlPos(ell).x, y);
                                        }
                                    });
                                    //break;
                                }
                            }
                            //to move x
                            while (!$(gsElem).data('gridstack').isAreaEmpty(x, y, node.width, node.height)) {
                                if ((x == node.x && node.y == y)) {
                                    //if($(gsElem).data('gridstack').isAreaEmpty(0, y, node.width, node.height)){
                                    //    x = 0;
                                    //    //y = y;
                                    //}
                                    break;
                                }
                                x++;
                                if (x == renderGridOptions.width) {
                                    //x = 0;
                                    //y++;
                                    break;
                                }

                            }
                            newNode.x = x;
                            newNode.y = y;
                            $(gsElem).data('gridstack').move(el, newNode.x, newNode.y);
                        });
                    }
                    $(gsElem).data('gridstack').commit();
                }
            });
        } else {
            $(setDiv).find(".grid-stack").each(function (ind, gsElem) {
                if (staticRunMode) {
                    $(gsElem).data('gridstack').batchUpdate();
                }
                clearHiddenGridstack(gsElem.id);
                if (staticRunMode) {
                    $(gsElem).data('gridstack').enableMove(false, true);
                    $(gsElem).data('gridstack').enableResize(false, true);
                    $(gsElem).data('gridstack').commit();
                }
                //if(!axInlineGridEdit && gsElem.id.startsWith("sp"))
                //    $("#"+gsElem.id).removeAttr("style");
            });
        }

    }
    //grid table


    var tableIDs = [];
    //if (setDiv == "#wBdr") {
    //    $("#wBdr").find("table[id^=gridHd]").each(function () { tableIDs.push("#" + this.id); });
    //} else {
    //    tableIDs.push(setDiv);
    //}

    if ($(setDiv).find("table[id^=gridHd]").length > 0) {
        $(setDiv).find("table[id^=gridHd]").each(function () {
            tableIDs.push("#" + this.id);
        });
    } else {
        tableIDs.push(setDiv);
    }
    $(tableIDs).each(function () {
        if (theMode == "design") {
            var thHeight = $("table" + this + " th:first:visible, " + this + " table th:first:visible").height();
            clearHiddenGridstack(this, true);
            $("table" + this + " th, " + this + " table th").resizable({
                handles: "e",
                minHeight: thHeight,
                maxHeight: thHeight,
                minWidth: 40,
                resize: function (event, ui) {
                    $('.grid-stack').addClass('dirty');
                    var thisDC = "0";
                    try {
                        thisDC = $(ui)[0].element.parents("[id^=colScroll]").attr("id").substr(9);
                    } catch (ex) { }
                    if ($("#ckbGridStretch" + thisDC).prop("checked") == true) {
                        $("#ckbGridStretch" + thisDC).prop("checked", false);
                        toggleGridStretch(thisDC);
                    }
                    changeStatus("notSaved");
                    //var sizerID = "#" + $(event.target).attr("id") + "-sizer";
                    // $(sizerID).width(ui.size.width);
                    var localWidth = $(this).closest("table").css("width").replace("px", "") - $(this).outerWidth()
                    var tablewidth = localWidth + ui.size.width;
                    $(this).css({
                        "width": ui.size.width,
                        "min-width": ui.size.width
                    });
                    $(this).closest("table").attr("style", "width:" + tablewidth + "px");
                }
            });
        } else {
            clearHiddenGridstack(this, true);
            if ($("table" + this + " th, " + this + " table th").find(".ui-resizable-handle").length) {
                $("table" + this + " th, " + this + " table th").resizable("destroy");
            }
        }
    });
    designLayoutProperties();
    if (!isMobile) {
        CompressMode(compressedMode);
    }
    toggleWizardDCOption((typeof designObj != "undefined" && typeof designObj[0] != "undefined" && typeof designObj[0].wizardDC != "undefined") ? designObj[0].wizardDC : false);
    //toggleWizardDCOption(typeof designObj[0].wizardDC != "undefined" ? designObj[0].wizardDC : false);

    changeDesignLayout((typeof designObj != "undefined" && typeof designObj[0] != "undefined" && typeof designObj[0].selectedLayout != "undefined") ? designObj[0].selectedLayout : "default");
    /*designLayoutProperties();*/

    dynamicMobileResolution();
}

function unBindEvents() {
    $("#wBdr").find('input:not(.grdButtons .newgridbtn .tgl-ios),textarea').each(function () {
        $(this).prop('disabled', true);
    })
    $("#wBdr").find('.fa-paperclip').css("pointer-events", "none");
    $("#wBdr").find('.divImgBorder').css("pointer-events", "none");

    $("#wBdr").find('select').each(function () {
        $(this).parent().prepend('<div class="selectOverLay"></div>');
        $(this).prop('disabled', true);
    })
    if (!axInlineGridEdit) {
        $("[id^=wrapperForEditFields]").removeClass("hide");
    }
    $("button[id^=gridAddBtn]").removeAttr("onclick").addClass('disabled');
    $(".grdButtons,.newgridbtn button[id^=gridAddBtn]").removeAttr("onclick").addClass('disabled');
    //$(".grdButtons .dropdown button").removeAttr("data-toggle").addClass('disabled');
    $("li[onclick*='javascript:FillGrid']").removeAttr("onclick").addClass('disabled');
    $(".grdButtons,.newgridbtn li button").addClass('disabled').prop('disabled', true);
    $(".editLayoutFooter button").removeAttr("onclick").addClass('disabled').prop('disabled', true).attr('type', 'button');

    //For toolbardiv icons making them disable
    $('.toolbardiv').addClass('disableToolIcon');
    // $('#dvlayout [title]:not([class^="icon-basic-eye"])').removeAttr('title');
    $(".editLayoutFooter").hide();
    $("#wBdr [id^=divDc] tbody tr").hide();
}

function unionSets(...sets) {
    return sets.reduce((combined, list) => {
        return new Set([...combined, ...list]);
    }, new Set());
}

function clearHiddenGridstack(divId, isTable = false) {
    divId = divId.concat();
    var thisDC = divId.startsWith("divDc") ? parseInt(divId.substr(5), 10) : divId.startsWith("sp") ? parseInt(divId.substr(divId.lastIndexOf("F") + 1), 10) : divId.startsWith("#gridHd") ? parseInt(divId.substr(7), 10) : divId.startsWith("#tab-") ? parseInt(divId.substr(5), 10) : 0;
    divId = divId.startsWith("#") ? divId.substr(1) : divId;
    try {
        if (designObj[0].dcLayout == null || designObj[0].dcLayout == "default") {
            var currentDC = designObj[0].dcs.filter(function (ele) {
                return ele.dc_id == thisDC
            })[0];
        } else {
            var currentDC = designObj[0].newdcs.filter(function (ele) {
                return ele.dc_id == thisDC
            })[0];
        }
    } catch (ex) { }
    let _dcInx = $j.inArray(thisDC, DCFrameNo);
    if (DCIsGrid[_dcInx] == "True" && axInlineGridEdit && theMode == "design") {
        $("#wrapperForEditFields" + thisDC).hide();
    }

    //debugger;
    //if (jsonText != undefined && jsonText != "") {
    if ($.isEmptyObject(designObj)) {
        jsonText = "";
        return;
    }
    //$($(".grid-stack")[0]).data("gridstack").removeWidget($('#randomID_111'), false)
    //$('#randomID_111').hide().removeClass("grid-stack-item")
    /*var hiddenGS =*/
    try {
        // compressedMode = appGlobalVarsObject._CONSTANTS.compressedMode;//designObj[0].compressedMode;

        if (!isMobile) {
            staticRunMode = typeof designObj[0].staticRunMode != "undefined" ? designObj[0].staticRunMode : false;
        }
        var lastTableMod = "axp_recid" + thisDC;
        var groupFieldDesignArr = {};
        let addedRowsArray = new Set();
        if (!isTable) {
            var gsiY = "";
            var prevIndId = 0;
            let layoutOrder = [];
            let visibleElements = [];
            currentDC.fieldsDesign.filter(function (elm, ind) {
                elm = generateRtlPos(elm);
                //if (!designObj[0].newDesign && theMode == "render") {

                //}
                try {

                    var tempRowObj = currentDC.fieldsDesign.filter(function (elmns) {
                        return elmns.y >= elm.y && elmns.y <= (elm.y + elm.height - 1)
                    });
                    if (!addedRowsArray.has(elm.y) && tempRowObj[0].fld_id == elm.fld_id) {
                        addedRowsArray = unionSets(addedRowsArray, new Set(_.map(tempRowObj, 'y')));
                        groupFieldDesignArr = tempRowObj[0];
                    }
                } catch (ex) {}
                if (typeof elm.visibility == "undefined") {
                    elm.visibility = true;
                }
                if (elm.fld_id.startsWith("uniqueEditDeleteAct") || elm.fld_id.startsWith("uniqueThHead")) {
                    colID = "#" + elm.fld_id;
                } else {
                    colID = "#th-" + elm.fld_id;
                }
                if (currentDC.isGrid == "T") {
                    if ($(colID).length < 1) {
                        return false;
                    }
                    try {
                        var rowNoDcNo = "";
                        try {
                            var tempRowNo = $(colID).parents(".griddivColumn").find("tbody tr:eq(0)").attr("id");
                            rowNoDcNo = tempRowNo.substr(tempRowNo.lastIndexOf("F") - 3, 3) + "F" + thisDC;
                        } catch (ex) {}
                        //var indField = $(colID).parents(".griddivColumn").find("thead th").index($(colID));
                        var indField = $(colID).parents(".griddivColumn").find("tbody tr:eq(0) td").index($("[id^=" + elm.fld_id + rowNoDcNo + "]").parents("tr:eq(0) td"));
                        if ($(colID).endsWith("_image") && indField != -1 && prevIndId == -1)
                            indField = -1;
                        prevIndId = indField;
                        //var indLastField = $("#th-" + lastTableMod).parents(".griddivColumn").find("thead th").index($("#th-" + lastTableMod));
                        var indLastField = $("#th-" + lastTableMod).parents(".griddivColumn").find("tbody tr:eq(0) td").index($("[id^=" + lastTableMod + rowNoDcNo + "]").parents("tr:eq(0) td"));
                        //if (lastTableMod != "axp_recid" + thisDC) {
                        //    indLastField = indLastField + 1;//recid field will also be there in tbody but not in thead
                        //}
                        if (!axInlineGridEdit && (!axInlineGridEdit && AxpGridForm != "form") && currentDC.dc_id == thisDC && $(colID).parents("#divDc" + thisDC).length) {
                            //debugger;
                            $("#th-" + lastTableMod).after($(colID).detach());
                            //var detachedTDs = $("#th-" + lastTableMod).parents(".griddivColumn").find("tbody tr td:nth-child(" + (indField + 1) + ")").detach();
                            //$("#th-" + lastTableMod).parents(".griddivColumn").find("tbody tr td:nth-child(" + (indLastField + 1) + ")").after(detachedTDs);
                            $("#th-" + lastTableMod).parents(".griddivColumn").find("tbody tr").each(function () {
                                //$('td:first',this).remove().insertAfter($('td:last',this)); 
                                var detachedTD = $(this).find("td:nth-child(" + (indField + 1) + ")").detach();
                                $(this).find("td:nth-child(" + (indLastField + (indField < indLastField ? 0 : 1)) + ")").after(detachedTD);
                            });
                        }
                    } catch (ex) {}
                    lastTableMod = "" + elm.fld_id;
                }
                var gsiHeight = "height";
                var fullFieldHeight = "height";
                var compressNormalMode = compressedMode ? "compressedMode" : "normalMode";
                if (theMode == "render" && !staticRunMode) {
                    var gsiPX = gsiPixels(elm.height);
                    if(elm.height>1)
                        gsiHeight = { "height": gsiPX.toString() + "px" };
                    //if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].selectedFontSize != null && designObj[0].selectedFontSize != "") {
                    //    //gsiHeight = {
                    //    //    "height": (isMobile ? gsiPX + 10 : gsiPX).toString() + "px",
                    //    //    "font-size": designObj[0].selectedFontSize + "px"
                    //    //};
                    //} else {
                    //    //gsiHeight = {
                    //    //    "height": (isMobile ? gsiPX + 10 : gsiPX).toString() + "px"
                    //    //};
                    //}
                    fullFieldHeight = {
                        "height": (gsiPX + gsConf[compressNormalMode].labelHeight) + "px"
                    };
                    //debugger;
                    //(currentDC.fieldsDesign.filter(function(elm){return elm.y == 1}).reduce((max, p) => p.height > max ? p.height : max, elm.height) + elm.y);
                    if (gsiY !== elm.y) {
                        var oldGsiY = gsiY;
                        gsiY = elm.y;
                        try {
                            if (oldGsiY !== "") {
                                //if((_.max(currentDC.fieldsDesign.map(function(elmn){if(elmn.y == oldGsiY)return elmn.height})) + oldGsiY) <= gsiY + 1){
                                if (groupFieldDesignArr != {} && groupFieldDesignArr.fld_id == elm.fld_id) {
                                    $("div#" + divId).append("<div class=\"clearfix\"></div>");
                                    groupFieldDesignArr = {};
                                }
                                //}
                            }
                        } catch (ex) {}
                    }
                }
                //.css("height", gsiHeight.toString() + "px")
                if (!elm.visibility) {
                    if (theMode == "render") {
                        if (!$("#" + divId).data('gridstack')) {
                            if (currentDC.isGrid == "T") {
                                $("#dvGrid" + elm.fld_id).removeClass("grid-stack-item").hide().attr("data-pop-visible", false);
                            } else {
                                $("#dv" + elm.fld_id).parents(".grid-stack-item").removeClass("grid-stack-item").hide();
                            }
                        } else {
                            if (currentDC.isGrid == "T") {
                                $("#" + divId).data("gridstack").removeWidget($("#dvGrid" + elm.fld_id), false);
                                $("#dvGrid" + elm.fld_id).hide().removeClass("grid-stack-item").attr("data-pop-visible", false);
                            } else {
                                $("#" + divId).data("gridstack").removeWidget($("#dv" + elm.fld_id).parents(".grid-stack-item"), false);
                                $("#dv" + elm.fld_id).parents(".grid-stack-item").hide().removeClass("grid-stack-item");
                            }
                        }
                    } else {
                        if (currentDC.isGrid == "T") {
                            if (!$("#dvGrid" + elm.fld_id + ".grid-stack-item").length) {
                                //$("#dv" + elm.fld_id).parents("[id^=randomID]").addClass(".grid-stack-item");
                                var dvElem = $("#dvGrid" + elm.fld_id);
                                //$("#" + divId).data('gridstack').addWidget($("#dvGrid" + elm.fld_id).css("height", gsiHeight).show(), elm.x, elm.y, elm.width, elm.height);
                                $("#" + divId).data('gridstack').addWidget(dvElem.show(), elm.x, elm.y, elm.width, elm.height);
                                dvElem.find(".form-control").css(gsiHeight);
                                applyGridStackCalcHeight(dvElem.find("textarea.gridstackCalc, input[type=button].hotbtn.btn"), elm, gsiHeight, fullFieldHeight);
                            }
                            $("#dvGrid" + elm.fld_id + ".grid-stack-item").addClass("designHidden").find(".grid-stack-item-content").css("background-color", "rgba(101, 97, 97, 0.4)");
                        } else {
                            if (!$("#dv" + elm.fld_id).parents(".grid-stack-item").length) {
                                //$("#dv" + elm.fld_id).parents("[id^=randomID]").addClass(".grid-stack-item");
                                var dvElem = $("#dv" + elm.fld_id).parents("[id^=randomID]");
                                //$("#" + divId).data('gridstack').addWidget($("#dv" + elm.fld_id).parents("[id^=randomID]").css("height", gsiHeight).show(), elm.x, elm.y, elm.width, elm.height);
                                $("#" + divId).data('gridstack').addWidget(dvElem.show(), elm.x, elm.y, elm.width, elm.height);
                                dvElem.find(".form-control").css(gsiHeight);
                                applyGridStackCalcHeight(dvElem.find("textarea.gridstackCalc, input[type=button].hotbtn.btn"), elm, gsiHeight, fullFieldHeight);
                            }
                            $("#dv" + elm.fld_id).parents(".grid-stack-item").addClass("designHidden").find(".grid-stack-item-content").css("background-color", "rgba(101, 97, 97, 0.4)");
                        }
                    }
                } else {

                    if (!$("#" + divId).data('gridstack')) {
                        if (currentDC.isGrid == "T") {
                            if (!$("#" + divId).hasClass("inEditMode") && currentDC.dc_id == thisDC && $("#dvGrid" + elm.fld_id).parents("#divDc" + thisDC).length) {
                                if (!axInlineGridEdit && (!axInlineGridEdit && AxpGridForm != "form")) {
                                    $("#" + divId).append($("#dvGrid" + elm.fld_id).detach());
                                }
                            }
                            //$("#dvGrid" + elm.fld_id).css("height", gsiHeight).attr({ "data-gs-x": elm.x, "data-gs-y": elm.y, "data-gs-width": elm.width, "data-gs-height": elm.height });
                            var dvElem = $("#dvGrid" + elm.fld_id);
                            dvElem.attr({
                                "data-gs-x": elm.x,
                                "data-gs-y": elm.y,
                                "data-gs-width": elm.width,
                                "data-gs-height": elm.height
                            });
                            dvElem.find(".form-control").css(gsiHeight);
                            applyGridStackCalcHeight(dvElem.find("textarea.gridstackCalc, input[type=button].hotbtn.btn"), elm, gsiHeight, fullFieldHeight);

                        } else {
                             if (!$("#" + divId).hasClass("inEditMode") && currentDC.dc_id == thisDC && $("#dv" + elm.fld_id).parents("#divDc" + thisDC).length) {
                                 $("#" + divId).append($("#dv" + elm.fld_id).parents(".grid-stack-item").detach());
                             }
                            //$("#dv" + elm.fld_id).parents(".grid-stack-item").css("height", gsiHeight).attr({ "data-gs-x": elm.x, "data-gs-y": elm.y, "data-gs-width": elm.width, "data-gs-height": elm.height });
                            if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].dcLayout != null && designObj[0].dcLayout == "single")
                                elm.width = 36;
                            else if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].dcLayout != null && designObj[0].dcLayout == "double")
                                elm.width = 18;
                            else if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].dcLayout != null && designObj[0].dcLayout == "triple")
                                elm.width = 12;
                            var dvElem = $("#dv" + elm.fld_id).parents(".grid-stack-item");
                            dvElem.attr({
                                "data-gs-x": elm.x,
                                "data-gs-y": elm.y,
                                "data-gs-width": elm.width,
                                "data-gs-height": elm.height
                            });
                            dvElem.find(".form-control:not(.multiFldRdg),.divImgBorder>.dvImgClear").css(gsiHeight);
                            applyGridStackCalcHeight(dvElem.find("textarea.gridstackCalc, input[type=button].hotbtn.btn"), elm, gsiHeight, fullFieldHeight);
                        }
                    } else {
                        if (currentDC.isGrid == "T") {
                            var dvElem = $("#dvGrid" + elm.fld_id);
                            $("#" + divId).data('gridstack').move(dvElem, elm.x, elm.y);
                            //$("#" + divId).data('gridstack').resize($("#dvGrid" + elm.fld_id).css("height", gsiHeight), elm.width, elm.height);
                            $("#" + divId).data('gridstack').resize(dvElem, elm.width, elm.height);
                            dvElem.find(".form-control").css(gsiHeight);
                            applyGridStackCalcHeight(dvElem.find("textarea.gridstackCalc, input[type=button].hotbtn.btn"), elm, gsiHeight, fullFieldHeight);

                        } else {
                            var dvElem = $("#dv" + elm.fld_id).parents(".grid-stack-item");
                            $("#" + divId).data('gridstack').move(dvElem, elm.x, elm.y);
                            //$("#" + divId).data('gridstack').resize($("#dv" + elm.fld_id).parents(".grid-stack-item").css("height", gsiHeight), elm.width, elm.height);
                            $("#" + divId).data('gridstack').resize(dvElem, elm.width, elm.height);
                            dvElem.find(".form-control").css(gsiHeight);
                            applyGridStackCalcHeight(dvElem.find("textarea.gridstackCalc, input[type=button].hotbtn.btn"), elm, gsiHeight, fullFieldHeight);



                            layoutOrder.push({
                                fld_id: elm.fld_id,
                                width: elm.width,
                                height: elm.height,
                                x: elm.x,
                                y: elm.y
                            });

                            var dvElem = $("#dv" + elm.fld_id).parents(".grid-stack-item");
                            /*if (dvElem.is(":visible")) {*/
                            if (!dvElem.hasClass('d-none') && !dvElem.hasClass('hide')) {
                                visibleElements.push({
                                    elm: elm,
                                    dvElem: dvElem
                                });
                            }
                        }
                    }
                }
                return !elm.visibility;
            });

            $.each(visibleElements, function (i, item) {
                var gsiHeight = "height";
                var fullFieldHeight = "height";
                var $grid = $("#" + divId).data('gridstack');
                var layout = layoutOrder[i]; // Get the i-th layout slot
                var elm = item.elm;
                var dvElem = item.dvElem;

                elm.x = layout.x;
                elm.y = layout.y;
                elm.width = layout.width;
                elm.height = layout.height;

                $grid.move(dvElem, elm.x, elm.y);
                $grid.resize(dvElem, elm.width, elm.height);

                dvElem.find(".form-control").css(gsiHeight);
                applyGridStackCalcHeight(
                    dvElem.find("textarea.gridstackCalc, input[type=button].hotbtn.btn"),
                    elm,
                    gsiHeight,
                    fullFieldHeight
                );
            });
        } else {
            //currentDC.dc_id
            //var dcNo=currentDC.dc_id;
            if (theMode == "design" && $("#titleCkbGridStretch" + thisDC).length <= 0) {
                var gridStretchHTML = "";
                gridStretchHTML = "<div class=\"form-check form-switch form-check-custom\"><span class=\"setTitle\"><span id=\"titleCkbGridStretch" + thisDC + "\" for=\"usr\">Stretch Grid Columns &nbsp; <i tabindex=\"-1\" class=\"icon-arrows-question ui-draggable ui-draggable-handle\" id=\"ico_cl" + thisDC + "\" data-trigger=\"hover\" data-toggle=\"popover\" data-content=\"Stretch and Preview Grid Columns\" data-placement=\"right\" data-original-title=\"\" title=\"\"></i></span></span><input type=\"checkbox\" id=\"ckbGridStretch" + thisDC + "\" title=\"\" style=\"\" name=\"ckbGridStretch" + thisDC + "\" class=\"form-check-input tgl tgl-ios ckbGridStretchSwitch\"></div>";
                $("#divDc" + thisDC).find(".newgridbtn").parent().prepend(gridStretchHTML);
            }

            let totalWidth = 0;
            currentDC.tableDesign.filter(function (elm, ind) {
                if (typeof elm.visibility == "undefined") {
                    elm.visibility = true;
                }

                if (!elm.fld_id) {
                    return elm.visibility;
                }

                var colID = "";
                if (elm.fld_id.startsWith("uniqueEditDeleteAct") || elm.fld_id.startsWith("uniqueThHead")) {
                    colID = "#" + elm.fld_id;
                } else {
                    colID = "#th-" + elm.fld_id;
                }
                if ($(colID).parents("#divDc" + thisDC).length) {
                    if (!elm.visibility) {
                        if (theMode == "render") {
                            $(colID).parents(".griddivColumn").find("thead th" + colID).addClass("none").hide().attr("data-grid-visible", false);
                            var rowNoDcNo = "";
                            try {
                                var tempRowNo = $(colID).parents(".griddivColumn").find("tbody tr:eq(0)").attr("id");
                                rowNoDcNo = tempRowNo.substr(tempRowNo.lastIndexOf("F") - 3, 3) + "F" + thisDC;
                            } catch (ex) {}
                            if (colID.startsWith("#uniqueThHead"))
                                $(colID).parents(".griddivColumn").find("tbody tr td label[id^=lblSlNo]").parent("td").hide();
                            else if (colID.startsWith("#uniqueEditDeleteAct"))
                                $(colID).parents(".griddivColumn").find("tbody tr td .gridEditDeleteBtns").parent("td").hide();
                            else {
                                var indTR = $(colID).parents(".griddivColumn").find("tbody tr:eq(0) td").index($("[id^=" + elm.fld_id + rowNoDcNo + "]").parents("tr:eq(0) td")[0]);
                                $(colID).parents(".griddivColumn").find("tbody tr td:nth-child(" + (indTR + 1) + ")").hide();
                            }
                        }
                    } else {
                        if (colID.startsWith("#uniqueEditDeleteAct"))
                            $(colID).addClass("w-20px"); //css("width", "0px");
                        else if (colID.startsWith("#uniqueThHead"))
                            $(colID).css("width", "40px");
                        else
                            $(colID).css("width", elm.width);
                        totalWidth = totalWidth + parseInt(elm.width)
                    }
                }
                return !elm.visibility;
            });
            if (totalWidth !== 0)
                $(".wrapperForGridData" + thisDC + " table").css("width", totalWidth + "px");
            if (currentDC.gridStretch == true) {
                //toggleGridStretch(thisDC, true);
                toggleGridStretch(thisDC, true, "firstLoad");
                $("#ckbGridStretch" + thisDC).prop("checked", true);
            }
        }
    } catch (ex) {
        //debugger
    }
    //hiddenGS.forEach(function (elm, ind) {  });
    //}
}

function generateRtlPos(gsNode) {
    gsNode = {
        ...gsNode
    };
    gsNode.x = gllangType == "ar" ? ((renderGridOptions.width - 1) - gsNode.x - (gsNode.width - 1)) : gsNode.x;
    return gsNode;
}

function toggleStaticRunMode() {
    $('.grid-stack').addClass('dirty');
    changeStatus("notSaved");
}

function goToDesignMode() {
    if (AxIsTstructLocked)
        return;

    var tstCustHTML = "";

    if (AxpTstButtonStyle == "old") {
        tstCustHTML = $("#breadcrumb-panel").length != 0 ? $("#breadcrumb-panel").get(0).outerHTML : "*@*";

        tstCustHTML += $("#tstToolBarBtn").length != 0 ? "*@*" + $("#tstToolBarBtn").get(0).outerHTML : "*@*";

        if (isWizardTstruct) {
            tstCustHTML += $("#wbdrHtml").length != 0 ? "*@*" + $("#wbdrHtml").clone().append($("#wizardBodyContent").find(".row.dvdcframe")).get(0).outerHTML : "*@*";
        } else {
            tstCustHTML += $("#wbdrHtml").length != 0 ? "*@*" + $("#wbdrHtml").get(0).outerHTML : "*@*";
        }

        if (!isWizardTstruct)
            tstCustHTML += $(".BottomToolbarBar").length != 0 ? "*@*" + $(".BottomToolbarBar").get(0).outerHTML : "*@*";

        if ($("#dvFooter").length > 0)
            tstCustHTML += $("#dvFooter").length != 0 ? "*@*" + $("#dvFooter").get(0).outerHTML : "*@*";
    } else {
        $("#iconsNewOption").removeClass("open");
        tstCustHTML = $("#breadcrumb-panel").length != 0 ? $("#breadcrumb-panel").get(0).outerHTML : "*@*";

        tstCustHTML += $(".toolbarRightMenu").length != 0 ? "*@*" + $(".toolbarRightMenu").get(0).outerHTML : "*@*";

        if (isWizardTstruct) {
            tstCustHTML += $("#wbdrHtml").length != 0 ? "*@*" + $("#wbdrHtml").clone().append($("#wizardBodyContent").find(".row.dvdcframe")).get(0).outerHTML : "*@*";
        } else {
            tstCustHTML += $("#wbdrHtml").length != 0 ? "*@*" + $("#wbdrHtml").get(0).outerHTML : "*@*";
        }

        if (!isWizardTstruct)
            tstCustHTML += $(".BottomToolbarBar").length != 0 ? "*@*" + $(".BottomToolbarBar").get(0).outerHTML : "*@*";
    }

    callParentNew("tstructCustomHTML=", tstCustHTML);
    try {
        let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + transid + "-" + appSUrl + "♥)", "function");
        if (_thisKey.length > 0) {
            for (const val of _thisKey) {
                localStorage.removeItem(val);
            }
        }
    } catch (ex) { }
    let _ThisSrc = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
    _ThisSrc = GetUriFormCurrentUri(_ThisSrc, transid);
    window.location.href = _ThisSrc + "&theMode=design";
}

function DesignFormDcLayouts() {
    var dvHtml = `<div class="container-fluid designTstruct-layout"><div class="row">`;
    dvHtml += `<div class="column col-3 ${dcLayoutType == "default" ? "active" : ""}" onclick="setActiveDesignLayout(this);" data-layout="default"><div class="card shadow-sm"><div class="card-body px-8"><span class="material-icons material-icons-style active-icon float-end ${dcLayoutType == "default" ? "text-success" : ""}">check_circle</span><h4 class="layout-title py-1">Default</h4><div class="d-flex flex-column py-3"><span class="layout-info d-flex mb-3"><span class="material-icons material-icons-style layout-info-icon me-2">info</span>Drag&Drop and Resize</span><span class="material-icons material-icons-style layout-icon material-icons-5tx mx-auto">assignment</span></div></div></div></div>`;
    dvHtml += `<div class="column col-3 ${dcLayoutType == "single" ? "active" : ""}" onclick="setActiveDesignLayout(this);" data-layout="single"><div class="card shadow-sm"><div class="card-body px-8"><span class="material-icons material-icons-style active-icon float-end ${dcLayoutType == "single" ? "text-success" : ""}">check_circle</span><h4 class="layout-title py-1">Single Column</h4><div class="d-flex flex-column py-3"><span class="layout-info d-flex mb-3"><span class="material-icons material-icons-style layout-info-icon me-2">info</span>No Drag&Drop and Resize</span><span class="material-icons material-icons-style layout-icon material-icons-5tx mx-auto">table_rows</span></div></div></div></div>`;
    dvHtml += `<div class="column col-3 ${dcLayoutType == "double" ? "active" : ""}" onclick="setActiveDesignLayout(this);" data-layout="double"><div class="card shadow-sm"><div class="card-body px-8"><span class="material-icons material-icons-style active-icon float-end ${dcLayoutType == "double" ? "text-success" : ""}">check_circle</span><h4 class="layout-title py-1">Double Column</h4><div class="d-flex flex-column py-3"><span class="layout-info d-flex mb-3"><span class="material-icons material-icons-style layout-info-icon me-2">info</span>No Drag&Drop and Resize</span><span class="material-icons material-icons-style layout-icon material-icons-5tx mx-auto">window</span></div></div></div></div>`;
    dvHtml += `<div class="column col-3 ${dcLayoutType == "triple" ? "active" : ""}" onclick="setActiveDesignLayout(this);" data-layout="triple"><div class="card shadow-sm"><div class="card-body px-8"><span class="material-icons material-icons-style active-icon float-end ${dcLayoutType == "triple" ? "text-success" : ""}">check_circle</span><h4 class="layout-title py-1">Triple Column</h4><div class="d-flex flex-column py-3"><span class="layout-info d-flex mb-3"><span class="material-icons material-icons-style layout-info-icon me-2">info</span>No Drag&Drop and Resize</span><span class="material-icons material-icons-style layout-icon material-icons-5tx mx-auto">grid_on</span></div></div></div></div>`;
    dvHtml += `</div></div>`;
    /*displayBootstrapModalDialog("DC Layouts", "lg", "200px", "", dvHtml);*/
    let myModal = new BSModal("modalDCLayouts", "DC Layouts", dvHtml, () => {
        //shown callbackcard-body px-3
    }, () => {
        //hide callback
    });
    myModal.changeSize("xl");
    myModal.hideFooter();
}

function setActiveDesignLayout(elem) {
    $(".column").removeClass("active");
    $(elem).addClass("active");
    dcLayoutType = $(elem).attr("data-layout");
    isDcLayoutSelected = true;
    SavePublishDesignerJSONWeb(false);
}

function AddFormLabel(ele = "") {
    $("#designContextMenu").css({
        'display': 'none'
    });
    $("#designContextMenuLabelEdit").css({
        'display': 'none'
    });
    let editCaption = "";
    if (ele != "") {
        let flJson = JSON.parse(formLabelJSON);
        $.each(flJson, function (key, value) {
            if (flJson[key].id == ele) {
                editCaption = flJson[key].name;
                return;
            }
        });
    }
    var dvHtml = `<div class="container-fluid"><div class="row">`;
    dvHtml += `<div class="column col-sm-12 col-md-12 col-lg-12"><textarea id="txtlblcaption" class="form-control" rows="2" style="resize: none;">` + editCaption + `</textarea></div>`;
    dvHtml += `</div>`;
    dvHtml += `</div>`;
    let footerHTML = `<div class="modal-footer"><button id="btnlcOk" title="Ok" value="Ok" class="btn btn-primary shadow-sm" onclick="AddFormLabelCaption('label','` + ele + `');">Ok</button></div>`;
    
    let myModal = new BSModal("modalLabelCaption", "Caption", dvHtml, () => {
        $(".modal-content").append(footerHTML);
    }, () => {
        //hide callback
    });
    myModal.changeSize("lg");
    myModal.hideFooter();
}

function AddFormLabelCaption(eleType, ele) {
    //if(eleType=="label") 
    let lblcaption = $("#txtlblcaption").val();
    if (ele == "") {
        let aftFldName = $("#designContextMenu").attr("data-affld");
        let fldIndex = $j.inArray(aftFldName, FNames);
        let thisDc = 0;
        if (fldIndex > -1)
            thisDc = GetDcNo(aftFldName);
        else
            thisDc = aftFldName.slice(5);
        //JSON.parse(formLabelJSON)
        let lblCount = $(".gridstackCalc.formLabel").length + 1;

        if (formLabelJSON != "" && formLabelJSON != "[]")
            formLabelJSON = formLabelJSON.replace('}]', "},{\"ftype\":\"" + eleType + "\",\"id\":\"userFormLabel" + lblCount + "\",\"dc\":\"" + thisDc + "\",\"afterField\":\"" + aftFldName + "\",\"name\":\"" + lblcaption + "\",\"fontFamilly\":\"\",\"fontFamillyCode\":\"\",\"hyperlinkJson\":\"\"}]");
        else
            formLabelJSON = "[{\"ftype\":\"" + eleType + "\",\"id\":\"userFormLabel" + lblCount + "\",\"dc\":\"" + thisDc + "\",\"afterField\":\"" + aftFldName + "\",\"name\":\"" + lblcaption + "\",\"fontFamilly\":\"\",\"fontFamillyCode\":\"\",\"hyperlinkJson\":\"\"}]";
    } else {
        let flJson = JSON.parse(formLabelJSON);
        $.each(flJson, function (key, value) {
            if (flJson[key].id == ele) {
                flJson[key].name = lblcaption;
                return;
            }
        });
        formLabelJSON = JSON.stringify(flJson);
    }
    SavePublishDesignerJSONWeb(false);
}

function ChangeFontAndColor(ele, ctType) {
    let flJson = [];
    if (ctType == "label") {
        flJson = JSON.parse(formLabelJSON);
        jQuery.grep(flJson, function (obj) {
            if (obj.id === ele) {
                flJson = obj;
                return flJson;
            }
        });
    } else if (buttonFieldFontJSON.length > 0) {
        flJson = JSON.parse(buttonFieldFontJSON);
        jQuery.grep(flJson, function (obj) {
            if (obj.id === ele) {
                flJson = obj;
                return flJson;
            }
        });
    }

    $("#designContextMenu").css({
        'display': 'none'
    });
    $("#designContextMenuLabelEdit").css({
        'display': 'none'
    });
    loadAndCall({
        files: {
            css: ["/ThirdParty/seballot-spectrum/spectrum.css", "/Css/fontColorPicker.min.css?v=2"],
            js: ["/ThirdParty/seballot-spectrum/spectrum.js", "/Js/fontColorPicker.js?v=1"]
        },
        callBack() {
            $("#designlblFontPicker").remove();
            $("#designContextMenuLabelEdit").append(`<input type="text" id="designlblFontPicker" style="display:none;" data-value="` + flJson.fontFamillyCode + `" class="form-control" value="` + flJson.fontFamillyCode + `" />`);

            var iconBtnFldId = "#designlblFontPicker";
            var oldValue = $(iconBtnFldId).val();
            (new PropertySheet()).generateFontPicker($(iconBtnFldId));
            setTimeout(function () {
                $("#filterBtn #btnFilter").hide();
                $("#filterBtn").append(`<input type="button" name="btnFilterReturn" value="Ok" onclick="callBackFontPicker('` + ele + `','` + ctType + `')" id="btnFilterReturn" title="Ok" class="btn btn-primary shadow-sm ms-auto handCursor allow-enter-key" style="">`);
            }, 300);
        }
    });
}

function callBackFontPicker(ele, ctType) {
    let changeStyle = $("#fontPickerSampleText").attr("style");
    let changeStyleValue = `[${(new PropertySheet()).fontPickerObj.fontFamily},${(new PropertySheet()).fontPickerObj.Bold?.[0]?.toLowerCase() || "f"},${(new PropertySheet()).fontPickerObj.Italic?.[0]?.toLowerCase() || "f"},${(new PropertySheet()).fontPickerObj.Underline?.[0]?.toLowerCase() || "f"},${(new PropertySheet()).fontPickerObj.fontColor},${(new PropertySheet()).fontPickerObj.Strikeout?.[0]?.toLowerCase() || "f"},${(new PropertySheet()).fontPickerObj.fontSize}]`;

    this.fontPickerObj = {};
    new IviewBuilder().closeModalDialog();
    changeStyle = changeStyle.replace(/"/g, "");
    if (ctType == "label") {
        let flJson = JSON.parse(formLabelJSON);
        $.each(flJson, function (key, value) {
            if (flJson[key].id == ele) {
                flJson[key].fontFamilly = changeStyle;
                flJson[key].fontFamillyCode = changeStyleValue;
                return;
            }
        });
        formLabelJSON = JSON.stringify(flJson);
    } else {
        let isFontModify = false;
        if (buttonFieldFontJSON.length > 0) {
            let flJson = JSON.parse(buttonFieldFontJSON);
            $.each(flJson, function (key, value) {
                if (flJson[key].id == ele) {
                    isFontModify = true;
                    flJson[key].fontFamilly = changeStyle;
                    flJson[key].fontFamillyCode = changeStyleValue;
                    return;
                }
            });
            buttonFieldFontJSON = JSON.stringify(flJson);
        }
        if (isFontModify == false) {
            if (buttonFieldFontJSON != "" && buttonFieldFontJSON != "[]")
                buttonFieldFontJSON = buttonFieldFontJSON.replace('}]', "},{\"ftype\":\"" + ctType + "\",\"id\":\"" + ele + "\",\"fontFamilly\":\"" + changeStyle + "\",\"fontFamillyCode\":\"" + changeStyleValue + "\",\"hyperlinkJson\":\"\"}]");
            else
                buttonFieldFontJSON = "[{\"ftype\":\"" + ctType + "\",\"id\":\"" + ele + "\",\"fontFamilly\":\"" + changeStyle + "\",\"fontFamillyCode\":\"" + changeStyleValue + "\",\"hyperlinkJson\":\"\"}]";
        }
    }
    SavePublishDesignerJSONWeb(false);
}

function LabelHyperlink(ele, ctType) {
    let flJson = [];
    if (ctType == "label") {
        flJson = JSON.parse(formLabelJSON);
        jQuery.grep(flJson, function (obj) {
            if (obj.id === ele) {
                flJson = obj;
                return flJson;
            }
        });
    } else if (buttonFieldFontJSON.length > 0) {
        flJson = JSON.parse(buttonFieldFontJSON);
        jQuery.grep(flJson, function (obj) {
            if (obj.id === ele) {
                flJson = obj;
                return flJson;
            }
        });
    }
    $("#designContextMenu").css({
        'display': 'none'
    });
    $("#designContextMenuLabelEdit").css({
        'display': 'none'
    });
    let hylinkJson = "";
    if (typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "")
        hylinkJson = JSON.parse(flJson.hyperlinkJson);
    var dvHtml = `<div class="container-fluid"><div class="row">`;
    dvHtml += `<div class="col-md-12 d-flex pb-2"><div class="col-md-2"><span>Name</span></div><div class="col-md-4">`;
    if (typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "")
        dvHtml += `<input type="textbox" id="txtHlName" class="form-control" value="` + hylinkJson.name + `" readonly/>`;
    else
        dvHtml += `<input type="textbox" id="txtHlName" class="form-control"/>`;

    dvHtml += `</div></div>`;
    dvHtml += `<div class="col-md-12 d-flex pb-2"><div class="col-md-2"><span>Type</span></div><div class="col-md-4">`;
    dvHtml += `<select name="ddlHlType" id="ddlHlType" class="form-control form-select" onchange="labelHyperlinkType(this);" value="` + ((typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "") ? hylinkJson.type : "") + `">
  <option value=""></option>
        <option value="tstruct">tstruct</option>
  <option value="iview">iview</option>
  <option value="page">page</option>
</select></div><div class="col-md-2 ps-2"><span>Structure Name</span></div><div class="col-md-4"><select name="ddlStructName" id="ddlStructName" class="form-control form-select" onchange="labelHyperlinkStructName(this);" value="` + ((typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "") ? hylinkJson.structName : "") + `"></select></div></div>`;

    dvHtml += `<div class="col-md-12 d-flex pb-2 ps-2"><div class="col-md-2"></div><div class="col-md-4"><input type="checkbox" class="" id="chkpivtst" ` + ((typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "") ? (hylinkJson.ivtstpopup == "true" ? "checked" : "") : "") + `> Popup iview/tstruct</input></div><div class="col-md-4 ps-2"><input type="checkbox" class="" id="refreshparent" ` + ((typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "") ? (hylinkJson.refreshparent == "true" ? "checked" : "") : "") + `> Refresh parent on close</input></div></div>`;

    dvHtml += `<div class="col-md-12 d-flex pb-2 ps-2"><div class="col-md-2"></div><div class="col-md-4"><input type="radio" name="hyperlnikradio" value="load" ` + ((typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "") ? (hylinkJson.loadType == "load" ? "checked" : "") : "") + `> Load</input></div><div class="col-md-4 ps-2"><input type="radio" name="hyperlnikradio" value="open" ` + ((typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "") ? (hylinkJson.loadType == "open" ? "checked" : "") : "") + `> Open</input></div></div>`;
    dvHtml += `<div class="col-md-12 d-flex pb-2 ps-2"><table class="table table-bordered designTableHyperlink">
  <thead> 
    <tr>
        <th class="col-fit"></th>
        <th>Param Name</th>
        <th>Value</th>
    </tr>
  </thead>
  <tbody>`;
    if (typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "" && hylinkJson.paramNode != "") {
        let paramNodeList = hylinkJson.paramNode.split("^");
        $.each(paramNodeList, function (key, value) {
            let idInd = key + 1;
            let valList = value.split("~");
            dvHtml += `<tr><td><input type="checkbox" name="chkHlParam"></td><td><select name="ddlParamName` + idInd + `" id="ddlParamName` + idInd + `" class="form-control form-select" data-val="` + valList[0] + `"><option value="` + valList[0] + `" selected>` + valList[0] + `</option></select></td><td><input type="textbox" id="txtValue` + idInd + `" class="form-control" value="` + valList[1] + `"/></td></tr>`;
        });
    } else {
        dvHtml += `<tr>
        <td><input type="checkbox" name="chkHlParam"></td>
        <td><select name="ddlParamName1" id="ddlParamName1" class="form-control form-select"></select></td>
        <td><input type="textbox" id="txtValue1" class="form-control"/></td>
    </tr>`;
    }
    dvHtml += `</tbody></table></div>`;
    dvHtml += `<div class="col-md-12 d-flex pb-2 ps-2">
    <button id="btnHlAdd" class="btn btn-bg-light btn-color btn-active-light shadow-sm cardStyle designTableBtns" onclick="hlAddParam();"><span class="material-icons designTableIcons">add</span></button>
    <button id="btnHlDelete" class="btn btn-bg-light btn-color btn-active-light shadow-sm cardStyle designTableBtns" onclick="hlDeleteParam();"><span class="material-icons designTableIcons">delete</span></button>
    <button id="btnlcOk" title="Ok" value="Ok" class="btn btn-primary shadow-sm ms-auto" onclick="callBackHyperlink('` + ele + `','` + ctType + `')">Ok</button>
    </div>`;
    dvHtml += `</div></div>`;

    let myModal = new BSModal("modalHyperlink", "Hyperlink", dvHtml, () => {
        (typeof flJson.hyperlinkJson != "undefined" && flJson.hyperlinkJson != "") ? labelHyperlinkOnLoad() : "";
    }, () => {
        //hide callback
    });
    myModal.changeSize("xl");
    myModal.hideFooter();
}


function labelHyperlinkOnLoad() {
    setTimeout(function () {
        let hlpType = $("#ddlHlType").attr("value");
        if (typeof hlpType != "undefined" && hlpType != "") {
            $('#ddlHlType').val(hlpType);
            let hlpStructName = $("#ddlStructName").attr("value");
            labelHyperlinkType($('#ddlHlType'), hlpStructName);
        }
    }, 300);
}

function hlAddParam() {
    let tableRow = $(".designTableHyperlink tbody tr:last");
    let hltbltrcount = $(".designTableHyperlink tbody tr:last").find("select").attr("id"); //$(".designTableHyperlink tbody tr").length;
    hltbltrcount = hltbltrcount.replace("ddlParamName", "");
    hltbltrcount = parseInt(hltbltrcount) + 1;
    $(".designTableHyperlink tbody").append(`<tr>
        <td><input type="checkbox" name="chkHlParam"></td>
        <td><select name="ddlParamName` + hltbltrcount + `" id="ddlParamName` + hltbltrcount + `" class="form-control"></select></td>
        <td><input type="textbox" id="txtValue` + hltbltrcount + `" class="form-control"/></td>
    </tr>`);
    var options = $("#ddlParamName1 > option").clone();
    $('#ddlParamName' + hltbltrcount).append(options);;
}

function hlDeleteParam() {
    $(".designTableHyperlink tbody tr").filter(':has(:checkbox:checked)').remove()
    if ($(".designTableHyperlink tbody tr").length == 0)
        $(".designTableHyperlink tbody").append(`<tr><td><input type="checkbox" name="chkHlParam"></td><td><select name="ddlParamName1" id="ddlParamName1" class="form-control form-select"></select></td><td><input type="textbox" id="txtValue1" class="form-control"/></td></tr>`);
}

function callBackHyperlink(ele, ctType) {
    let txtHlName = $("#txtHlName").val();
    if (txtHlName == "") {
        showAlertDialog("warning", "Name should not be empty.");
        return;
    }
    let ddlHlType = $("#ddlHlType").val();
    if (ddlHlType == "") {
        showAlertDialog("warning", "Type should not be empty.");
        return;
    }
    let ddlStructName = $("#ddlStructName").val();
    if (ddlStructName == "") {
        showAlertDialog("warning", "Structure Name should not be empty.");
        return;
    }
    let chkpivtst = $("#chkpivtst").prop("checked");
    let refreshparent = $("#refreshparent").prop("checked");
    let radioType = typeof $('input[name="hyperlnikradio"]:checked').val() != "undefined" ? $('input[name="hyperlnikradio"]:checked').val() : "open";
    let tblParam = "";
    $(".designTableHyperlink tbody tr").each(function (ind, val) {
        if ($(val).find("select").val() != null && $(val).find("select").val() != "") {
            tblParam += $(val).find("select").val();
            tblParam += "~" + $(val).find("input[type=textbox]").val() + "^";
        }
    });
    if (tblParam != "")
        tblParam = tblParam.slice(0, -1);
    let hlNode = `{"name":"` + txtHlName + `","type":"` + ddlHlType + `","structName":"` + ddlStructName + `","ivtstpopup":"` + chkpivtst + `","refreshparent":"` + refreshparent + `","loadType":"` + radioType + `","paramNode":"` + tblParam + `"}`;
    if (ctType == "label") {
        let flJson = JSON.parse(formLabelJSON);
        $.each(flJson, function (key, value) {
            if (flJson[key].id == ele) {
                flJson[key].hyperlinkJson = hlNode;
                return;
            }
        });
        formLabelJSON = JSON.stringify(flJson);
    } else {
        let isFontModify = false;
        if (buttonFieldFontJSON != "" && buttonFieldFontJSON != "[]" && buttonFieldFontJSON.length > 0) {
            let flJson = JSON.parse(buttonFieldFontJSON);
            $.each(flJson, function (key, value) {
                if (flJson[key].id == ele) {
                    isFontModify = true;
                    flJson[key].hyperlinkJson = hlNode;
                    return;
                }
            });
            buttonFieldFontJSON = JSON.stringify(flJson);
        }
        if (isFontModify == false) {
            if (buttonFieldFontJSON != "" && buttonFieldFontJSON != "[]")
                buttonFieldFontJSON = buttonFieldFontJSON.replace('}]', "},{\"ftype\":\"" + ctType + "\",\"id\":\"" + ele + "\",\"fontFamilly\":\"\",\"fontFamillyCode\":\"\",\"hyperlinkJson\":\"\"}]");
            else
                buttonFieldFontJSON = "[{\"ftype\":\"" + ctType + "\",\"id\":\"" + ele + "\",\"fontFamilly\":\"\",\"fontFamillyCode\":\"\",\"hyperlinkJson\":\"\"}]";
            let flJson = JSON.parse(buttonFieldFontJSON);
            $.each(flJson, function (key, value) {
                if (flJson[key].id == ele) {
                    flJson[key].hyperlinkJson = hlNode;
                    return;
                }
            });
            buttonFieldFontJSON = JSON.stringify(flJson);
        }
    }
    SavePublishDesignerJSONWeb(false);
}


function labelHyperlinkStructName(ele, selectedValonLoad = "") {
    let StrType = $("#ddlHlType").val();
    let selectedStName = $(ele).val();
    if (selectedStName != "" && StrType != "page") {
        try {
            $.ajax({
                url: 'tstruct.aspx/GetLabelHlStrParams',
                type: 'POST',
                cache: false,
                async: true,
                data: JSON.stringify({
                    StrType: StrType,
                    StructName: selectedStName
                }),
                dataType: 'json',
                contentType: "application/json",
                success: function (data) {
                    if (data.d != "") {
                        var jVal = JSON.parse(data.d);
                        if (jVal != "" && jVal.result.row != "") {
                            var output = [];
                            output.push('<option value=""></option>');
                            $.each(jVal.result.row, function (ind, obj) {
                                if (obj.name != "")
                                    output.push('<option value="' + obj.name + '">' + obj.caption + '</option>');
                            });
                            $(".designTableHyperlink tbody tr").each(function (ind, val) {
                                $(val).find("select").empty().append(output.join(''));
                                if (selectedValonLoad != "") {
                                    let sVal = $(val).find("select").attr("data-val");
                                    if (typeof sVal != "undefined" && sVal != "")
                                        $(val).find("select").val(sVal);
                                }
                            });
                        }
                    } else {
                        jsonVal = "";
                    }
                },
                error: function (error) {

                }
            });
        } catch (exp) {}
    }
}

function labelHyperlinkType(ele, selectedValonLoad = "") {
    let jsonVal = "";
    let selectedValue = $(ele).val();
    if (selectedValue != "") {
        if (selectedValonLoad == "") {
            $(".designTableHyperlink tbody tr").remove();
            $(".designTableHyperlink tbody").append(`<tr><td><input type="checkbox" name="chkHlParam"></td><td><select name="ddlParamName1" id="ddlParamName1" class="form-control form-select"></select></td><td><input type="textbox" id="txtValue1" class="form-control"/></td></tr>`);
        }
        if (selectedValue == "page") {
            $("#chkpivtst,#refreshparent").prop("disabled", true);
            $('input[name="hyperlnikradio"]').prop("disabled", true);
            $('#ddlParamName1').prop("disabled", true);
            $('#txtValue1').prop("disabled", true);
            $("#btnHlAdd").removeAttr("onclick");
            $("#btnHlDelete").removeAttr("onclick");
        } else {
            $("#chkpivtst,#refreshparent").prop("disabled", false);
            $('input[name="hyperlnikradio"]').prop("disabled", false);
            $('#ddlParamName1').prop("disabled", false);
            $('#txtValue1').prop("disabled", false);
            if ($('#btnHlAdd').attr("onclick") == undefined) {
                $("#btnHlAdd").attr('onclick', 'hlAddParam();');
                $("#btnHlDelete").attr('onclick', 'hlDeleteParam();');
            }
        }
        try {
            $.ajax({
                url: 'tstruct.aspx/GetLabelHlStructures',
                type: 'POST',
                cache: false,
                async: true,
                data: JSON.stringify({
                    StrType: selectedValue
                }),
                dataType: 'json',
                contentType: "application/json",
                success: function (data) {
                    if (data.d != "") {
                        var jVal = JSON.parse(data.d);
                        if (jVal != "" && jVal.result.row != "") {
                            var output = [];
                            output.push('<option value=""></option>');
                            $.each(jVal.result.row, function (ind, obj) {
                                if (obj.name != "")
                                    output.push('<option value="' + obj.name + '">' + obj.caption + '</option>');
                            });
                            $('#ddlStructName').empty().html(output.join(''));
                            if (selectedValonLoad != "") {
                                $('#ddlStructName').val(selectedValonLoad);
                                if ($('#ddlHlType').val() != "page")
                                    labelHyperlinkStructName($("#ddlStructName"), selectedValonLoad);
                            }
                        }
                        jsonVal = "done";
                    } else {
                        jsonVal = "error";
                    }
                },
                error: function (error) {
                    jsonVal = "error";
                }
            });
        } catch (exp) {
            jsonVal = "error";
        }
    } else {
        var output = [];
        $('#ddlStructName').html(output.join(''));
        jsonVal = "done";
    }
    return jsonVal;
}

function RemoveLabelHyperlink(ele) {
    let flJson = JSON.parse(formLabelJSON);
    jQuery(flJson).each(function (index) {
        if (flJson[index].id == ele) {
            flJson.splice(index, 1);
            return false;
        }
    });
    formLabelJSON = JSON.stringify(flJson);
    SavePublishDesignerJSONWeb(false);
}


function customTstHtml() {
    // displayBootstrapModalDialog("Custom HTML", "lg", "370px", true, "./tstructhtml.aspx", true);
    ShowDimmer(true);
    let myModal = new BSModal("tstCustomHtml", "Custom HTML", `<iframe class="col-12 h-100" src="./tstructhtml.aspx?transid=${transid}"></iframe>`, () => {
        //shown callback
    }, () => {
        //hide callback
    });
    myModal.changeSize("fullscreen");
    myModal.hideFooter();
    myModal.hideHeader();
    myModal.modalBody.classList.add("overflow-hidden");
}


function goToRenderMode() {
    checkIfFormChanges();
    if (isFormChange) {
        confirmOnAction();
    } else {
        //window.location.href = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
        //callParentNew("lastLoadtstId=", _thisRui);

        //let _thisRui = 'tstruct.aspx?transid=' + transid + '&openerIV=ttotl&isIV=false';
        //callParentNew(`LoadIframe(${_thisRui})`, "function");

        let iframes = window.parent.document.getElementsByTagName('iframe');
        let currentIframe = null;
        for (let i = 0; i < iframes.length; i++) {
            if (iframes[i].contentWindow === window) {
                currentIframe = iframes[i];
                break;
            }
        }
        let _thisIframe = $(currentIframe).attr("id");
        if (typeof _thisIframe != "undefined" && _thisIframe == "middle1") {
            let _thisRui = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
            callParentNew(`LoadIframe(${_thisRui})`, "function");
        } else {
            window.location.href = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
        }
    }

}

function toggleDesignModeWBDR(SaveDesign) {
    if (typeof isWizardTstruct != "undefined" && isWizardTstruct && theMode == "render") {
        goToDesignMode();
        return;
    }
    theMode == "design" ? theMode = "render" : theMode = "design";
    setDesignedLayout("#wBdr", SaveDesign);
}


// TOGGLE WIZARD dc
function toggleWizardDCOption(type) {
    $('.grid-stack').addClass('dirty');
    // if ($("#ckbWizardDC").prop("checked") == true || (type != "undefined" && type == false)) {
    //     //checkbox is unchecked                 
    //     $("#ckbWizardDC").prop('checked', false);

    // } else {
    //     //checkbox is checked
    //     $("#ckbWizardDC").prop('checked', true);

    // }

}

function changeDesignLayout(value = "") {
    if (!value) {
        $('.grid-stack').addClass('dirty');
    } else {
        $("#designLayoutSelector").val(value);
        if (value == "tile") {
            if ($(".tileLayoutDiv").outerWidth(true) <= renderGridOptions.minWidth) {
                $(".grid-stack").addClass("grid-stack-one-column-mode");
            }
        }
    }
}

function designLayoutProperties() {
    if (typeof designObj != "undefined" && typeof designObj[0] != "undefined") {
        if (compressedMode) { //designObj[0].compressedMode
            $("#designControlHeightSelector").val(designObj[0].selectedControlHeight == null ? "24" : designObj[0].selectedControlHeight);
            $("#designControlHeightValue").val(designObj[0].selectedControlHeight == null ? "24" : designObj[0].selectedControlHeight);
        } else {
            $("#designControlHeightSelector").val(designObj[0].selectedControlHeight == null ? "28" : designObj[0].selectedControlHeight);
            $("#designControlHeightValue").val(designObj[0].selectedControlHeight == null ? "28" : designObj[0].selectedControlHeight);
        }
        $("#designForntSizeSelector").val(designObj[0].selectedFontSize == null ? "14" : designObj[0].selectedFontSize);
        $("#designForntSizeValue").val(designObj[0].selectedFontSize == null ? "14" : designObj[0].selectedFontSize);
        // $("#designFormWidthSelector").val(designObj[0].formWidth == null ? "100" : designObj[0].formWidth);
        // $("#designFormWidthValue").val(designObj[0].formWidth == null ? "100" : designObj[0].formWidth);
        // $("#designFormAlignmentSelector").val(designObj[0].formAlignment == null ? "default" : designObj[0].formAlignment);
        
        if (designObj[0].formWidth == null) {
            $("#designFormWidthSelector").val("100");
        } else {
            $("#designFormWidthSelector").val(designObj[0].formWidth);
            $("#" + designObj[0].transid).css("width", $("#designFormWidthSelector").val() + "%");
        }
        if (designObj[0].formWidth == null) {
            $("#designFormWidthValue").val("100");
        } else {
            //$("#dvlayout").removeClass("w-100");
            $("#designFormWidthValue").val(designObj[0].formWidth);
            $("#" + designObj[0].transid).css("width", $("#designFormWidthSelector").val() + "%");
        }
        if (designObj[0].formAlignment == null) {
            $("#designFormAlignmentSelector").val("default");
        } else {
            if (!isMobile)
                $("#dvlayout").removeClass("position-absolute");
            $("#designFormAlignmentSelector").val(designObj[0].formAlignment);
        }


        $("#designFieldCaptionWidthSelector").val(designObj[0].fieldCaptionWidth == null ? "30" : designObj[0].fieldCaptionWidth);
        $("#designFieldCaptionWidthValue").val(designObj[0].fieldCaptionWidth == null ? "30" : designObj[0].fieldCaptionWidth);
        dcLayoutType = designObj[0].dcLayout == null ? "default" : designObj[0].dcLayout;
        formLabelJSON = designObj[0].formLabel == null ? [] : JSON.stringify(designObj[0].formLabel);
        buttonFieldFontJSON = designObj[0].buttonFieldFont == null ? [] : JSON.stringify(designObj[0].buttonFieldFont);

        $("#ckbCompressedMode").prop('checked', compressedMode); // designObj[0].compressedMode);
        $("#ckbWizardDC").prop('checked', designObj[0].wizardDC);
        $("#ckbStaticRunMode").prop('checked', staticRunMode);
        $("#designLayoutSelector").val(designObj[0].selectedLayout);
        $("#designFormAlignmentSelector").val(designObj[0].formAlignment);

        setDesProp = {
            compressedMode: $("#ckbCompressedMode").is(":checked"),
            staticRunMode: $("#ckbStaticRunMode").prop('checked'),
            wizardDC: $("#ckbWizardDC").prop('checked'),
            selectedLayout: $("#designLayoutSelector").val(),
            selectedFontSize: $("#designForntSizeSelector").val(),
            selectedControlHeight: $("#designControlHeightSelector").val(),
            formWidth: $("#designFormWidthSelector").val(),
            formAlignment: $("#designFormAlignmentSelector").val(),
            fieldCaptionWidth: $("#designFieldCaptionWidthSelector").val()
        }
    }
}

function setDesignProperties() {
    setDesProp = {
        compressedMode: $("#ckbCompressedMode").is(":checked"),
        staticRunMode: $("#ckbStaticRunMode").prop('checked'),
        wizardDC: $("#ckbWizardDC").prop('checked'),
        selectedLayout: $("#designLayoutSelector").val(),
        selectedFontSize: $("#designForntSizeSelector").val(),
        selectedControlHeight: $("#designControlHeightSelector").val(),
        formWidth: $("#designFormWidthSelector").val(),
        formAlignment: $("#designFormAlignmentSelector").val(),
        fieldCaptionWidth: $("#designFieldCaptionWidthSelector").val()
    }
}

function applyGridStackCalcHeight(currentElm, elm, gsiHeight, fullFieldHeight) {
    if (theMode != "design") {
        var compressNormalMode = compressedMode ? "compressedMode" : "normalMode";
        var gsiPX = gsiPixels(elm.height);
        // gsiHeight = { "height": gsiPX.toString() + "px" };
        gsiHeight = {
            "height": (isMobile ? gsiPX + 10 : gsiPX).toString() + "px"
        };
        fullFieldHeight = {
            "height": (gsiPX + gsConf[compressNormalMode].labelHeight) + "px"
        };
        if (typeof CKEDITOR.instances[$(currentElm).attr("id")] != "undefined") {
            //var ckHeight = CKEDITOR.instances[$(currentElm).attr("id")].resize( '100%', gsiHeight.height, true );
            try {
                CKEDITOR.instances[$(currentElm).attr("id")].on("instanceReady", function () {
                    if(!staticRunMode) {
                        this.resize('100%', gsiPX);
                    } else {
                        this.resize('100%', ($(this.ui.contentsElement.$).parents(".grid-stack-item-content").height()-$(this.ui.contentsElement.$).parents(".grid-stack-item-content").children(".fld-wrap3").outerHeight(true)-2) -  $(this.ui.contentsElement.$).siblings().toArray().reduce((total, elm)=>{
                            return total = total + $(elm).outerHeight(true);            
                        }, 0), true );
                    }                
                });
            } catch (error) {}            
        } else {
            if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].dcLayout != null && designObj[0].dcLayout == "default") {
                if (currentElm.hasClass('memofam gridstackCalc') && staticRunMode) {
                    currentElm.css(gsiHeight);
                    currentElm.parent().css(gsiHeight);
                } else
                    currentElm.parent().css(fullFieldHeight);
            }
        }
    }
}

function gsiPixels(gsiHeight) {
    var compressNormalMode = compressedMode ? "compressedMode" : "normalMode";
    if (typeof designObj != "undefined" && designObj[0] != null && designObj[0].selectedControlHeight != null && designObj[0].selectedControlHeight != "" && gsiHeight == 1)
        return parseInt(designObj[0].selectedControlHeight, 10);
    else
        return parseInt((((gsiHeight * gsConf[compressNormalMode].cellHeight) + ((gsiHeight - 1) * gsConf[compressNormalMode].verticalMargin)) - gsConf[compressNormalMode].labelHeight), 10);
}

function getDesignerJSON() {
    //toggleDesignModeWBDR();
    var dsnJSON = "";
    var typeee_as_grid = '';

    this.form_serialized_data = _.map($('#wBdr'), function (el) {
        this.dc_serialized_data = _.map($('.grid-stack'), function (el) {
            el = $(el);
            dc_id = $(el).attr("id");
            typeee_as_grid = el.parents(".grid-icons").length > 0 ? "T" : "F";
            var irowfldX = 0;
            var irowfldY = 0;
            var irowfldH = 1;
            var isirowHChange = false;
            var dgoFldWidth = designGridOptions.width;
            this.serialized_data = _.map($('#' + dc_id + ' > .grid-stack-item'), function (el) {
                el = $(el);
                var node = el.data('_gridstack_node');
                if (dcLayoutType == "single" && typeee_as_grid == "F") {
                    node.width = dgoFldWidth;

                    let nhght = node.height;
                    if (nhght != irowfldH && irowfldH != 1)
                        irowfldY += irowfldH - 1;
                    irowfldH = nhght;
                    node.y = irowfldY;
                    irowfldY++;
                } else if (dcLayoutType == "double" && typeee_as_grid == "F") {
                    node.width = dgoFldWidth / 2;
                    if (irowfldX == 0) {
                        node.x = 0;
                        let nhght = node.height;
                        if (nhght != irowfldH && irowfldH != 1) {
                            irowfldY += irowfldH - 1;
                            isirowHChange = true;
                        } else {
                            irowfldH = nhght;
                            isirowHChange = false;
                        }
                        node.y = irowfldY;
                        irowfldX++;
                    } else {
                        node.x = dgoFldWidth / 2;
                        let nhght = node.height;
                        if (isirowHChange) {
                            irowfldH = nhght;
                            isirowHChange = false;
                        } else if (nhght > irowfldH)
                            irowfldH = nhght;
                        node.y = irowfldY;
                        irowfldY++;
                        irowfldX = 0;
                    }
                } else if (dcLayoutType == "triple" && typeee_as_grid == "F") {
                    node.width = dgoFldWidth / 3;
                    if (irowfldX == 0) {
                        node.x = 0;
                        let nhght = node.height;
                        if (nhght != irowfldH && irowfldH != 1) {
                            irowfldY += irowfldH - 1;
                            isirowHChange = true;
                        } else {
                            irowfldH = nhght;
                            isirowHChange = false;
                        }
                        node.y = irowfldY;
                        irowfldX++;
                    } else if (irowfldX == 1) {
                        node.x = dgoFldWidth / 3;
                        let nhght = node.height;
                        if (nhght > irowfldH)
                            irowfldH = nhght;
                        node.y = irowfldY;
                        irowfldX++;
                    } else {
                        node.x = (dgoFldWidth / 3) * 2;
                        let nhght = node.height;
                        if (isirowHChange) {
                            irowfldH = nhght;
                            isirowHChange = false;
                        } else if (nhght > irowfldH)
                            irowfldH = nhght;
                        node.y = irowfldY;
                        irowfldY++;
                        irowfldX = 0;
                    }
                }
                node = generateRtlPos(node);
                //debugger;
                return {
                    fld_id: typeee_as_grid == "F" ? $(el).find("div[id]").attr("id").substr(2) : $(el).attr("id").substr(6),
                    x: node.x,
                    y: node.y,
                    width: node.width,
                    height: node.height,
                    visibility: !el.hasClass("designHidden"),
                };
            }, this);

            //this.serialized_data_table = _.map($('#' + dc_id).parents(".grid-icons").next(".griddivColumn").children("table:eq(0)").find("th:visible"), function (el, index) {
            this.serialized_data_table = _.map($('#' + dc_id).parent().next(".griddivColumn").children("table:eq(0)").find("th:not(.hide)"), function (el, index) {
                el = $(el);
                var r__link = el.attr('r_link');
                //var indexxxx = el.attr('indexx');
                //var typeeeee = el.attr('typeee');
                //debugger;
                if (typeof $(el).attr("id") == "undefined") {
                    return;
                }
                return {
                    fld_id: $(el).attr("id").startsWith("th-") ? $(el).attr("id").substr(3) : $(el).attr("id"),
                    width: parseInt($(el).outerWidth(true), 10),
                    //visibility: !el.hasClass("designHidden"),
                    visibility: !(el.hasClass("designHidden") == true ? el.hasClass("designHidden") : el.css("display") == "none" ? true : false),
                };
            }, this);
            //var fieldsDesign = GridStackUI.Utils.sort(this.serialized_data);
            var fieldsDesign = this.serialized_data.length ? GridStackUI.Utils.sort(this.serialized_data) : null;
            var tableDesign = this.serialized_data_table.length ? this.serialized_data_table : null;
            dc_id = dc_id.startsWith("divDc") ? dc_id.substr(5) : dc_id.substr(dc_id.lastIndexOf("F") + 1);
            return {
                dc_id,
                isGrid: typeee_as_grid,
                gridStretch: $("#ckbGridStretch" + dc_id).is(":checked"), //To be changed with grid stretch
                fieldsDesign,
                tableDesign
            };
        }, this);
        var form_data = (typeof dcLayoutType == "undefined" || dcLayoutType == "" || dcLayoutType == "default") ? (isDcLayoutSelected == true ? designObj[0].dcs : (this.dc_serialized_data)) : designObj[0].dcs;
        var newform_data = (typeof dcLayoutType != "undefined" && dcLayoutType != "" && dcLayoutType != "default") ? (this.dc_serialized_data) : designObj[0].newdcs;
        return {
            transid,
            compressedMode: compressedMode, //setDesProp.compressedMode, //$("#ckbCompressedMode").is(":checked"),
            newDesign: true,
            staticRunMode: setDesProp.staticRunMode, // $("#ckbStaticRunMode").prop('checked'),
            wizardDC: setDesProp.wizardDC, //$("#ckbWizardDC").prop('checked'),
            selectedLayout: setDesProp.selectedLayout, //$("#designLayoutSelector").val(),
            selectedFontSize: setDesProp.selectedFontSize, // $("#designForntSizeSelector").val(),
            selectedControlHeight: setDesProp.selectedControlHeight, // $("#designControlHeightSelector").val(),
            tstUpdatedOn: typeof tstructUpdateOn != "undefined" ? tstructUpdateOn : "",
            dcLayout: typeof dcLayoutType != "undefined" && dcLayoutType != "" ? dcLayoutType : "default",
            formWidth: setDesProp.formWidth, // $("#designFormWidthSelector").val(),
            formAlignment: setDesProp.formAlignment, // $("#designFormAlignmentSelector").val(),
            fieldCaptionWidth: setDesProp.fieldCaptionWidth, // $("#designFieldCaptionWidthSelector").val(),
            formLabel: typeof formLabelJSON != "undefined" && formLabelJSON != "" ? JSON.parse(formLabelJSON) : [],
            buttonFieldFont: typeof buttonFieldFontJSON != "undefined" && buttonFieldFontJSON != "" ? JSON.parse(buttonFieldFontJSON) : [],
            dcs: form_data,
            newdcs: newform_data
        };
    }, this);

    if (dcLayoutType == "" || dcLayoutType == "default") {
        var newIndex = form_serialized_data[0].dcs.map(function (obj, index) {
            return obj.dc_id;
        });

        var oldUpdatedObject = [];
        try {
            if (designObj[0].dcs != null) {
                oldUpdatedObject = designObj[0].dcs.filter(function (obj, index) {
                    if (newIndex.indexOf(obj.dc_id) == -1) {
                        return true;
                    }
                });
            }
        } catch (ex) {

        }

        form_serialized_data[0].dcs = _.sortBy([...form_serialized_data[0].dcs, ...oldUpdatedObject], 'dc_id');
    } else {
        var newIndex = form_serialized_data[0].newdcs.map(function (obj, index) {
            return obj.dc_id;
        });

        var oldUpdatedObject = [];
        try {
            if (designObj[0].newdcs != null) {
                oldUpdatedObject = designObj[0].newdcs.filter(function (obj, index) {
                    if (newIndex.indexOf(obj.dc_id) == -1) {
                        return true;
                    }
                });
            }
        } catch (ex) {

        }

        form_serialized_data[0].newdcs = _.sortBy([...form_serialized_data[0].newdcs, ...oldUpdatedObject], 'dc_id');
    }
    dsnJSON = JSON.stringify(this.form_serialized_data, null, '');

    //console.log(dsnJSON.replace(/"/g, '\\"'));
    return dsnJSON;
}

function SaveDesignerJSON() {

    var dsnJSON = getDesignerJSON();
    var SavedId = $("#SaveID").val();
    var PublishId = $("#PublishID").val();
    checkIfFormChanges();
    if (isFormChange) {
        $("#designModeToolbar").css("pointer-events", "none");
        $(".grid-stack").removeClass('dirty')
        var settings = primaryApiSettings();
        settings.url = callParentNew("nodeApi") + "setLayoutSave";
        settings.data.transid = transid.toLowerCase();
        settings.data.module = "TSTRUCT";
        settings.data.content = dsnJSON;
        if (SavedId != undefined && SavedId.length > 0) {
            settings.data.design_id = SavedId
            settings.data.type = "UPDATE";
        } else
            settings.data.type = "SAVE";
        $.ajax(settings).done(function (response) {
            $("#designModeToolbar").css("pointer-events", "");
            if (response.status == true) {
                SavedId = response.data;
                $("#IsPublish").val("N");
                var ispublish = $("#IsPublish").val();
                //if (SavedId) {
                //$("#designModeToolbar").css("pointer-events", "none");
                ///ASB.WebService.SetSavedAndPublishedID(transid,dsnJSON ,SavedId, PublishId, ispublish, function (result, eventArgs) {
                // $("#designModeToolbar").css("pointer-events", "");
                //}, function (result) {
                //    $("#designModeToolbar").css("pointer-events", "");
                //});
                //}
                $("#SaveID").attr("value", SavedId);
                $("#PublishDesign").css("opacity", "1.0");
                changeStatus("save");

                showAlertDialog("success", DesigntstructMessages.designSaved);

            } else {
                showAlertDialog("error", response.errMsg);
                valSessByApi(response);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $("#designModeToolbar").css("pointer-events", "");
            showAlertDialog("error", DesigntstructMessages.designSaveFailed);
        });
    } else {
        //$("#designModeToolbar").css("pointer-events", "");
        showAlertDialog("warning", DesigntstructMessages.noModification);
        return;
    }
}


function SaveDesignerJSONWeb() {
    var dsnJSON = getDesignerJSON();
    var SavedId = $("#SaveID").val();
    var PublishId = $("#PublishID").val();
    var Savetype = "SAVE";
    checkIfFormChanges();
    if (isFormChange) {
        $("#designModeToolbar").css("pointer-events", "none");
        $(".grid-stack").removeClass('dirty');
        try {
            $.ajax({
                url: 'tstruct.aspx/SavePublishDesign',
                type: 'POST',
                cache: false,
                async: true,
                data: JSON.stringify({
                    Transid: transid,
                    DesignType: Savetype,
                    Content: dsnJSON,
                    SavedId: ''
                }),
                dataType: 'json',
                contentType: "application/json",
                success: function (data) {
                    $("#designModeToolbar").css("pointer-events", "");
                    if (data.d != "" && !data.d.startsWith("error:")) {
                        SavedId = data.d;
                        $("#IsPublish").val("N");
                        var ispublish = $("#IsPublish").val();
                        $("#SaveID").attr("value", SavedId);
                        $("#PublishDesign").css("opacity", "1.0");
                        changeStatus("save");
                        showAlertDialog("success", DesigntstructMessages.designSaved);
                    } else if (data.d.startsWith("error:")) {
                        showAlertDialog("error", data.d.replace(/^error:/, ''));
                    }
                    else {
                        showAlertDialog("error", "Error while save design, please check the trace file(using show logs) for more details.");
                    }
                },
                error: function (error) {
                    $("#designModeToolbar").css("pointer-events", "");
                    showAlertDialog("error", error);
                    //showAlertDialog("error", DesigntstructMessages.designSaveFailed);
                }
            });
        } catch (exp) {
            $("#designModeToolbar").css("pointer-events", ""); //ex.message
            showAlertDialog("error", exp.message);
            //showAlertDialog("error", DesigntstructMessages.designSaveFailed);
        }
    } else {
        showAlertDialog("warning", DesigntstructMessages.noModification);
        return;
    }
}

function PublishDesignerJSON() {

    checkIfFormChanges();
    if (isFormChange) {
        showAlertDialog("warning", DesigntstructMessages.saveBeforePublish);
        return;
    } else {
        if ($("#IsPublish").val() == 'Y') {
            showAlertDialog("warning", DesigntstructMessages.noModification);
            return;
        }
        var SavedId = $("#SaveID").val();
        var PublishId = $("#PublishID").val();
        $("#designModeToolbar").css("pointer-events", "none");
        var settings = primaryApiSettings();
        settings.url = callParentNew("nodeApi") + "setLayoutPublish";
        settings.data.transid = transid.toLowerCase();
        settings.data.design_id = SavedId;
        if (PublishId != undefined && PublishId.length > 0)
            settings.data.type = "UPDATE";
        $.ajax(settings).done(function (response) {
            $("#designModeToolbar").css("pointer-events", "");
            if (response.status == true) {

                showAlertDialog("success", DesigntstructMessages.designPublished);

                PublishId = response.data;
                $("#PublishID").val(PublishId);
                changeStatus("published");
                $("#IsPublish").val("Y");
                var ispublish = $("#IsPublish").val();
                //if (PublishId) {
                $("#designModeToolbar").css("pointer-events", "none");
                ASB.WebService.SetSavedAndPublishedID(transid, "", SavedId, PublishId, ispublish, function (result, eventArgs) {
                    $("#designModeToolbar").css("pointer-events", "");
                }, function (result) {
                    $("#designModeToolbar").css("pointer-events", "");
                });
                //}
            } else {
                showAlertDialog("error", response.errMsg);
                valSessByApi(response);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $("#designModeToolbar").css("pointer-events", "");
            showAlertDialog("error", DesigntstructMessages.designPublisgFailed);
        });

    }

}

function PublishDesignerJSONWeb() {
    checkIfFormChanges();
    if (isFormChange) {
        showAlertDialog("warning", DesigntstructMessages.saveBeforePublish);
        return;
    } else {
        if ($("#IsPublish").val() == 'Y') {
            showAlertDialog("warning", DesigntstructMessages.noModification);
            return;
        }
        var SavedId = $("#SaveID").val();
        var PublishId = $("#PublishID").val();
        var Savetype = "PUBLISH";
        $("#designModeToolbar").css("pointer-events", "none");
        $.ajax({
            url: 'tstruct.aspx/SavePublishDesign',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                Transid: transid,
                DesignType: Savetype,
                Content: '',
                SavedId: SavedId
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                $("#designModeToolbar").css("pointer-events", "");
                if (data.d != "" && !data.d.startsWith("error:")) {
                    showAlertDialog("success", DesigntstructMessages.designPublished);
                    PublishId = data.d;
                    $("#PublishID").val(PublishId);
                    changeStatus("published");
                    $("#IsPublish").val("Y");
                } else if (data.d.startsWith("error:")) {
                    showAlertDialog("error", data.d.replace(/^error:/, ''));
                }
                else {
                    showAlertDialog("error", "Saved data not found.");
                }
            },
            error: function (error) {
                $("#designModeToolbar").css("pointer-events", "");
                showAlertDialog("error", DesigntstructMessages.designPublisgFailed);
            }
        });
    }
}

function SavePublishDesignerJSON() {

    var dsnJSON = getDesignerJSON();
    var SavedId = $("#SaveID").val();
    var PublishId = $("#PublishID").val();
    checkIfFormChanges();
    if (isFormChange) {
        $("#designModeToolbar").css("pointer-events", "none");
        $(".grid-stack").removeClass('dirty')
        var settings = primaryApiSettings();
        settings.url = callParentNew("nodeApi") + "setLayoutSave";
        settings.data.transid = transid.toLowerCase();
        settings.data.module = "TSTRUCT";
        settings.data.content = dsnJSON;
        if (SavedId != undefined && SavedId.length > 0) {
            settings.data.design_id = SavedId
            settings.data.type = "UPDATE";
        } else
            settings.data.type = "SAVE";
        $.ajax(settings).done(function (response) {
            $("#designModeToolbar").css("pointer-events", "");
            if (response.status == true) {
                SavedId = response.data;
                $("#SaveID").attr("value", SavedId);
                var settings1 = primaryApiSettings();
                settings1.url = callParentNew("nodeApi") + "setLayoutPublish";
                settings1.data.transid = transid.toLowerCase();
                settings1.data.design_id = SavedId;
                if (PublishId != undefined && PublishId.length > 0)
                    settings1.data.type = "UPDATE";
                $.ajax(settings1).done(function (response) {
                    if (response.status == true) {
                        showAlertDialog("success", DesigntstructMessages.designSaved);

                        PublishId = response.data;
                        $("#PublishID").val(PublishId);
                        //if (PublishId) {
                        $("#designModeToolbar").css("pointer-events", "none");
                        $("#IsPublish").val("Y");
                        var ispublish = $("#IsPublish").val();
                        ASB.WebService.SetSavedAndPublishedID(transid, dsnJSON, SavedId, PublishId, ispublish, function (result, eventArgs) {
                            $("#designModeToolbar").css("pointer-events", "");
                        }, function (result) {
                            $("#designModeToolbar").css("pointer-events", "");
                        });
                        //}
                        window.location.href = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
                    } else {
                        showAlertDialog("error", response.errMsg);
                        valSessByApi(response);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    showAlertDialog("error", DesigntstructMessages.designPublisgFailed);
                });
            } else {
                showAlertDialog("error", response.errMsg);
                valSessByApi(response);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $("#designModeToolbar").css("pointer-events", "");
            showAlertDialog("error", DesigntstructMessages.designSaveFailed);
        });
    } else {
        //$("#designModeToolbar").css("pointer-events", "");
        showAlertDialog("warning", DesigntstructMessages.noModification);
        return;
    }
}

function SavePublishDesignerJSONWeb(isSaveNormal = true) {
    var dsnJSON = getDesignerJSON();
    var SavedId = $("#SaveID").val();
    var PublishId = $("#PublishID").val();
    var Savetype = "SAVEPUBLISH";
    checkIfFormChanges();
    if (isFormChange) {
        $("#designModeToolbar").css("pointer-events", "none");
        $(".grid-stack").removeClass('dirty')
        $.ajax({
            url: 'tstruct.aspx/SavePublishDesign',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                Transid: transid,
                DesignType: Savetype,
                Content: dsnJSON,
                SavedId: SavedId
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                $("#designModeToolbar").css("pointer-events", "");
                if (data.d != "" && !data.d.startsWith("error:")) {
                    SavedId = data.d.split('~')[0];
                    PublishId = data.d.split('~')[1];
                    $("#SaveID").attr("value", SavedId);
                    if (isSaveNormal)
                        showAlertDialog("success", DesigntstructMessages.designSaved);
                    $("#PublishID").val(PublishId);
                    $("#IsPublish").val("Y");
                    //window.location.href = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
                    // commenting the above line as redirection to the tstruct will not happen in def schema. so closing the popup after succesfull save of the design.
                    if (isSaveNormal)
                        callParentNew("closeRemodalPopup()", "function");
                    else {
                        let pUrl = window.location.href; //#tab-4
                        pUrl = pUrl.split("#")[0];
                        window.location.href = pUrl;
                    }
                } else if (data.d.startsWith("error:")) {
                    showAlertDialog("error", data.d.replace(/^error:/, ''));
                }
                else {
                    showAlertDialog("error", "Error while save & publish design.");
                }
            },
            error: function (error) {
                $("#designModeToolbar").css("pointer-events", "");
                showAlertDialog("error", error);
            }
        });
    } else {
        showAlertDialog("warning", DesigntstructMessages.noModification);
        return;
    }
}


function resetButtonClicked() {
    var SavedId = $("#SaveID").val();
    var PublishId = $("#PublishID").val();
    var ConfirmReset = $.confirm({
        theme: 'modern',
        title: eval(callParent('lcm[155]')),
        animation: 'scale',
        closeAnimation: 'scale',
        animateFromElement: false,
        onContentReady: function () {
            disableBackDrop('bind');
            //to display tooltips for Confirm & Cancel buttons
            $(".jconfirm-buttons button").each(function () {
                var txt = $(this).text();
                $(this).prop('title', txt.charAt(0).toUpperCase() + txt.slice(1))
            });
            $(".jconfirm-buttons .btn-primary").focus(); //to focus on Confirm button once dialog is opened
        },
        backgroundDismiss: 'false',
        escapeKey: 'cancel',
        content: eval(callParent('lcm[373]')),
        buttons: {
            confirm: {
                text: eval(callParent('lcm[164]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    ConfirmReset.close();
                    $("#designModeToolbar").css("pointer-events", "none");
                    var settings = primaryApiSettings();
                    settings.url = callParentNew("nodeApi") + "deleteLayout";
                    settings.data.transid = transid.toLowerCase();
                    settings.data.design_id = SavedId;
                    if (PublishId != undefined) {
                        settings.data.p_design_id = PublishId;
                    }
                    $.ajax(settings).done(function (response) {
                        $("#designModeToolbar").css("pointer-events", "");
                        if (response.status == true) {
                            ShowDimmer(true);
                            SavedId = "";
                            $("#SaveID").val(SavedId);
                            PublishId = "";
                            $("#PublishID").val(PublishId);
                            $("#designModeToolbar").css("pointer-events", "none");
                            $("#IsPublish").val("N");
                            var ispublish = $("#IsPublish").val();
                            ASB.WebService.SetSavedAndPublishedID(transid, "", SavedId, PublishId, ispublish, function (result, eventArgs) {
                                $("#designModeToolbar").css("pointer-events", "");
                            }, function (result) {
                                $("#designModeToolbar").css("pointer-events", "");
                            });
                            showAlertDialog("success", DesigntstructMessages.resetSuccess);

                            goToDesignMode();

                        } else {
                            if (response.statusCode === 500) {
                                showAlertDialog("warning", DesigntstructMessages.alredayreset);
                            } else {
                                showAlertDialog("error", response.errMsg);
                                valSessByApi(response);
                            }
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        showAlertDialog("error", DesigntstructMessages.resetfailed);
                    });



                }
            },
            cancel: {
                text: eval(callParent('lcm[192]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                    parent.actionsClicked = "";
                },
            }
        }
    });
}

function ResetButtonClickedWeb() {
    var SavedId = $("#SaveID").val();
    var PublishId = $("#PublishID").val();
    var Savetype = "RESET";
    var ConfirmReset = $.confirm({
        theme: 'modern',
        title: eval(callParent('lcm[155]')),
        animation: 'scale',
        closeAnimation: 'scale',
        animateFromElement: false,
        onContentReady: function () {
            disableBackDrop('bind');
            //to display tooltips for Confirm & Cancel buttons
            $(".jconfirm-buttons button").each(function () {
                var txt = $(this).text();
                $(this).prop('title', txt.charAt(0).toUpperCase() + txt.slice(1))
            });
            $(".jconfirm-buttons .btn-primary").focus(); //to focus on Confirm button once dialog is opened
        },
        backgroundDismiss: 'false',
        escapeKey: 'cancel',
        content: eval(callParent('lcm[373]')),
        buttons: {
            confirm: {
                text: eval(callParent('lcm[164]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    ConfirmReset.close();
                    $("#designModeToolbar").css("pointer-events", "none");
                    $.ajax({
                        url: 'tstruct.aspx/SavePublishDesign',
                        type: 'POST',
                        cache: false,
                        async: true,
                        data: JSON.stringify({
                            Transid: transid,
                            DesignType: Savetype,
                            Content: '',
                            SavedId: SavedId
                        }),
                        dataType: 'json',
                        contentType: "application/json",
                        success: function (data) {
                            $("#designModeToolbar").css("pointer-events", "");
                            if (data.d == "reseted" && !data.d.startsWith("error:")) {
                                ShowDimmer(true);
                                SavedId = "";
                                $("#SaveID").val(SavedId);
                                PublishId = "";
                                $("#PublishID").val(PublishId);
                                $("#IsPublish").val("N");
                                showAlertDialog("success", DesigntstructMessages.resetSuccess);
                                goToDesignMode();
                            } else if (data.d.startsWith("error:")) {
                                showAlertDialog("error", data.d.replace(/^error:/, ''));
                            }
                            else {
                                if (data.d === "500") {
                                    showAlertDialog("warning", DesigntstructMessages.alredayreset);
                                } else {
                                    showAlertDialog("error", "Error while reset design.");
                                }
                            }
                        },
                        error: function (error) {
                            showAlertDialog("error", DesigntstructMessages.resetfailed);
                        }
                    });
                }
            },
            cancel: {
                text: eval(callParent('lcm[192]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                    parent.actionsClicked = "";
                },
            }
        }
    });
}

function setSelectedDesignElement(elem) {
    selectedDesignObject.elem = elem;
    selectedDesignObject.parentElemId = $(selectedDesignObject.elem).parents(".tstructDesignMode").attr("id");
    selectedDesignObject.isColumnDesigner = $(selectedDesignObject.elem).parents(".tstructDesignMode").is($("div[id^=colScroll]"));
    selectedDesignObject.isGrid = (selectedDesignObject.parentElemId.startsWith("sp") || selectedDesignObject.isColumnDesigner) ? true : false;

    if (selectedDesignObject.isGrid) {
        $("#showTheField").css("opacity", "1");
        $("#hideTheField").css("opacity", "1");
    } else {
        if (!selectedDesignObject.isGrid && $(selectedDesignObject.elem).hasClass("designHidden")) {
            $("#showTheField").css("opacity", "1");
        } else {
            $("#showTheField").css("opacity", "0.5");
        }
        $("#hideTheField").css("opacity", "0.5");
    }

    $(".ui-resizable").find(".thhead").css({
        "border": "",
        "border-right": ""
    });
    $(".grid-stack-item").find(".grid-stack-item-content").css("border", "");
    //debugger;
    if (selectedDesignObject.isColumnDesigner) {
        //$($(selectedDesignObject.elem).css("border", "1px solid red").prevAll(".ui-resizable:visible")[0]).css("border-right", "1px solid red");
        $($(selectedDesignObject.elem).find(".thhead").css("border", "1px solid red"));
    } else {
        $(selectedDesignObject.elem).find(".grid-stack-item-content.ui-draggable-handle").css("border", "1px solid red");
    }
}

function showHideGridStackField(op) {
    if (selectedDesignObject.isGrid || $(selectedDesignObject.elem).hasClass("designHidden")) {
        $('.grid-stack').addClass('dirty');
        changeStatus("notSaved");
        if (op == "hide") {
            $(selectedDesignObject.elem).addClass("designHidden").find(".grid-stack-item-content").css("background-color", "rgba(101, 97, 97, 0.4)");
            $(selectedDesignObject.elem).is("th") && $(selectedDesignObject.elem).find(".designHiddenTR").length == 0 ? $(selectedDesignObject.elem).append("<div class=\"designHiddenTR\"></div>") : "";
        } else {
            if (!selectedDesignObject.isGrid && $(selectedDesignObject.elem).hasClass("designHidden")) {
                $("#showTheField").css("opacity", "0.5");
            }
            $(selectedDesignObject.elem).removeClass("designHidden").find(".grid-stack-item-content").css("background-color", "");
            $(selectedDesignObject.elem).is("th") ? $(selectedDesignObject.elem).find(".designHiddenTR").remove() : "";
        }
    }
}


//Function to fill the fields with data which is called on load of the tstruct page.
var multiSelectID = "";
var draftTimer = "";

function setGlobalParameters() {
    if (typeof tstDummyLoadParams != "undefined" && tstDummyLoadParams != "") {
        Parameters = [];
        var regex = /Parameters\[(\d+)\]\s*=\s*"([^"]*)"/g;
        var match;
        while ((match = regex.exec(tstDummyLoadParams)) !== null) {
            var index = parseInt(match[1]);
            var value = match[2];
            Parameters[index] = value;
        }
        tstDummyLoadParams = "";
    }
}
function OnTstructLoad() {
    //Temporary code
    StartTime = new Date().getTime();
    //End Temporary code
    var stTime = new Date();
    setGlobalParameters();
    if (isLoadDataCall == false)
        setListviewButtons();
    allRtfTextAreas();
    AxAllowCancel = true;

    SetMRForFromList(AxFromLstFlds);

    jsonText = $j('#designHidden').val();

    try {
        designObj = JSON.parse(jsonText);
        if ($.isEmptyObject(designObj)) {
            jsonText = "";
        }
    } catch (ex) {
        jsonText = "";
    }

    let isCopyTransLoad = false;

    var result = "";
    if ((typeof LoadResult) == "string") {

        result = LoadResult;
        result = result.replace(new RegExp("&quot;", "g"), "'");

        try {
            UpdateDefaultDcRows();
            //By default cancel transaction should be allowed
            AxAllowCancel = true;
            // To bring HTML for Tab DC's as well on form load.
            var dcTabHtml = $("#hdnTabHtml").val();
            $("#hdnTabHtml").val("");
            if (dcTabHtml != undefined && dcTabHtml != "") {
                AssignTabDcHtmlPerf(dcTabHtml);
                dcTabHtml = "";
                DCFrameNo.forEach(function (dcID) {
                    let _dcInx = $j.inArray(dcID, DCFrameNo);
                    if (DCIsGrid[_dcInx] == "True") {
                        //ClearFieldsInDC(dcID);
                        SetRows(dcID, "", "d*", "LoadData"); //GIS000209
                        navValidator = true;
                        ChangedFields = new Array();
                        ChangedFieldDbRowNo = new Array();
                        ChangedFieldValues = new Array();
                        DeletedDCRows = new Array();
                    }
                    SetFormDirty(false);
                });
            }           
            try {
                if (typeof isCopyTrans != "undefined" && isCopyTrans)
                    isCopyTrans = false;
                if (typeof TstCopyTransRes != "undefined" && TstCopyTransRes != "") {
                    isCopyTransLoad = true;
                    AssignLoadValues(TstCopyTransRes, "LoadData");
                    TstCopyTransRes = "";
                    isCopyTrans = false;
                    $j("#recordid000F0").val("0");
                    $j("#axp_recid1000F1").val("0");
                    recordid = "0";
                }
            } catch (ex) { }
            if (!isCopyTransLoad) {
                if (theMode != "design") {
                    if (typeof wsPerfFormLoadCall != "undefined" && wsPerfFormLoadCall) {
                        AssignLoadValues(result, "LoadData");
                        if (typeof AxMemVarClient != "undefined" && AxMemVarClient == true && recordid == '0')
                            AssignWsPerfExprValues("LoadData");
                    } else {
                        AssignWsPerfExprValues("LoadData");
                    }

                    if (evalExpOnSaveDraftLoad && typeof wsPerfEvalExpClient != "undefined" && wsPerfEvalExpClient != "") {
                        evalExpOnSaveDraftLoad = false;
                        AssignWsPerfExprValues("LoadData");
                    }

                    if (typeof callParentNew('AxNotifyMsgId') != "undefined" && callParentNew('AxNotifyMsgId') != "") {
                        GetCachedSaveNotification(callParentNew('AxNotifyMsgId'));
                        callParentNew('AxNotifyMsgId=', "");
                    }

                    if (!tstReadOnlyPeg)
                        GridDcAddEmptyRows();
                } else
                    AssignLoadValuesDesignMode(result, "LoadData");
            }
            //if (typeof TstCopyTransRes != "undefined" && TstCopyTransRes != "") {
            //    AssignLoadValues(TstCopyTransRes, "LoadData");
            //    TstCopyTransRes = "";
            //    isCopyTrans = false;
            //    $j("#recordid000F0").val("0");
            //    $j("#axp_recid1000F1").val("0");
            //    recordid = "0";
            //}

            DisplayImages();
            closeParentFrame();
        } catch (ex) {
            AxWaitCursor(false);
            $j('div#wBdr').show();
            ShowDimmer(false);
            //showAlertDialog("error","Invalid data from server."); 
            showAlertDialog("error", ex.message);
        }
        ShowHideNexPrev();

    }
    //if (IsObjCustomHtml == "False")
    setDesignedLayout("#wBdr");
    /*if (calledFrom == "LoadData" && isGridFileUploadOnLoad == true) {*/
    if (isGridFileUploadOnLoad == true) {
        isGridFileUploadOnLoad = false;
        for (var i = 0; i < DCName.length; i++) {
            let dvId = DCName[i];
            let dvDcNo = dvId.substr(2);
            if (IsDcGrid(dvDcNo)) {
                if (IsTabDc(dvDcNo)) {
                    let _dvTabInd = $j.inArray(dvDcNo, TabDCs);
                    if (TabDCStatus[_dvTabInd] == "1") {
                        DropzoneInit("#divDc" + dvDcNo);
                        DropzoneGridInit("#divDc" + dvDcNo);
                    }
                } else {
                    if (isMobile && !axInlineGridEdit && AxpGridForm == "form") {
                        DropzoneInitFormGridOnLoad("#divDc" + dvDcNo);
                        DropzoneGridInitFormGridOnLoad("#divDc" + dvDcNo);
                    } else {
                        DropzoneInit("#divDc" + dvDcNo);
                        DropzoneGridInit("#divDc" + dvDcNo);
                    }
                }
            }
        }
    }
    //to hide listview button if a TStruct is opened as popup from ListView and IView
    if (window.opener && (window.opener.document.title == "Listview" || window.opener.document.title == "Iview") && $j(".listview").length > 0) {
        $j(".listview").parent().hide();

        if ($j(".backbutton").length > 0)
            $j(".backbutton").children().hide();
    }

    SetBackForwardButtonProp(enableForwardButton, enableBackButton);

    try {
        if (typeof isTstPostBackVal != "undefined" && isTstPostBackVal != "") {
            if ($("#axp_recid1000F1").length > 0 && $("#axp_recid1000F1").val() != "0") {
                recordid = $("#axp_recid1000F1").val();
                $j("#recordid000F0").val($("#axp_recid1000F1").val());
                document.title = "Load Tstruct";                
            }
        }
    } catch (ex) { }

    LoadResult = "";
    result = "";
    //$j("#searchoverrelay").css("display", "none");
    //$j("#dvPDFDocList").css("display", "none");
    //for Approved/Rejected transaction, no need to call the Actions

    if ($j(".flddis").length > 0) {
        $j(".flddis").each(function () {
            //Below check is if there is a memo field do ot add disabled attribute as the scroll also getting disabled
            var inpType = $j(this);
            if (inpType[0].type != "textarea") {
                if (!$j(".flddis").prop("disabled"))
                    $j(".flddis").prop("disabled", true);
            }
        });
    }

    GetDcStateValue("", "");
    let isOverridFomrControl = true;
    if (AxRuesDefFormcontrol == "true") {
        isOverridFomrControl = AxRulesDefParser("formcontrol", "", "formcontrol");
    }
    if (appstatus != "Approved" && appstatus != "Rejected" && (!AxExecFormControl) && theMode != "design" && isOverridFomrControl) {
        DoFormControlOnload();
    }
    if (appstatus != "Approved" && appstatus != "Rejected" && (!AxExecFormControl) && theMode != "design" && isOverridFomrControl) {
        var rid = $j("#recordid000F0").val();
        if (rid != "0")
            DoScriptFormControl("", "On Data Load");
        else
            DoScriptFormControl("", "On Form Load");
    }
    FormControlSameFormLoad = false;
    AssignGridScript();
    //SetMRForFromList(AxFromLstFlds);

    if (AxRulesDefComputescript == "true") {
        var rid = $j("#recordid000F0").val();
        if (rid != "0")
            AxRulesDefParser("compute script ondataload", "", "computescript");
        else
            AxRulesDefParser("compute script onformload", "", "computescript");
    } 

    if (AxRulesDefScriptOnLoad == "true")
        AxRulesScriptsParser("scriptonload", "");

    if (AxRuesDefScriptFormcontrol == "true")
        AxRulesScriptsParser("formcontrol", "");

    try {
        if (callGetTab) {
            arrVisibleTabDcs.forEach(function (ele, ind) {
                if (ele != "") {
                    $("#myTab li a").removeClass('active');
                    $("#myTab li#li" + ele + " a").addClass('active');
                    $(".tab-content .tab-pane:not(.grid-icons)").removeClass('active');
                    $(".tab-content #tab-" + ele).addClass('active');
                    $("#myTab li#li" + ele + " a").click();
                }
            });
        }
    } catch (ex) { }

    GetDcStateOnLodaData(isCopyTransLoad);
    if (theModeDesign == 'false' && staticRunMode && typeof fldsHideOnPage != "undefined" && fldsHideOnPage == "true") {
        setDesignedLayout("#wBdr");
        fldsHideOnPage = "false";
    }

    if (window.parent.enableDraft)
        EnableDraftFeature();

    gridFreezeCols();
   
    if (AxLogTimeTaken == "true") {
        var edTime = new Date();
        var resTime = edTime.getTime() - stTime.getTime();
        AxStartTime = GetAxDate(AxStartTime);
        CreateTimeLog(AxStartTime, "0", resTime, ASBTotal, ASBDbTime, "TstructLoad");
    }
    DisableFileUplDSignFld();

    if ($j.inArray("axp_refreshDcs", FNames) > -1) {
        var fldValue = $j('[id*="axp_refreshDcs"]').val();
        if (fldValue != undefined && fldValue != "") {
            var tempArray = fldValue.split(';');
            for (var i = 0; i < tempArray.length; i++) {
                //dc2:dc3~cname,countryName~

                arrRefreshDcs.push(tempArray[i].substring(0, tempArray[i].indexOf('~')));
                arrRefreshFlds.push(tempArray[i].substring(tempArray[i].indexOf('~') + 1));
                arrRefreshFldDirty.push(false);
            }
        }
    }
    ParseJSON();
    if (recordid != "0") {
        applyFontListView = true;
        for (var i = 0; i < DCFrameNo.length; i++) {
            EvaluateSetFont(DCFrameNo[i]);
        }
        applyFontListView = false;
    }
    for (var i = 0; i < DCFrameNo.length; i++) {
        if (DCAllowChange[i].toLowerCase() == "false") {
            disableDC(DCFrameNo[i])
        }
    }

    SetDynamicDcCaptions();
    if (isTstPop == "True") {
        TstructTabEventsInPopUP("");
    }
    SetPositionfldDisplayTot();
    AxWaitCursor(false);

    if (isTstPop.toUpperCase() == "TRUE") {
        $('a#design').click(function () {
            openTstDesign();
        });
    }
    $('.lblgridactn').html(appGlobalVarsObject.lcm[301]);
    $('.lblcleargrid').attr("title", appGlobalVarsObject.lcm[302]);
    $('.gridAddRow').attr("title", appGlobalVarsObject.lcm[477]);
    $('.lblExportGrid').attr("title", appGlobalVarsObject.lcm[486]);

    //if (recordid != "0")
    var fldName = GetExactFieldName('axp_save_draft');
    var dElem = $("input[id^='" + fldName + "']:eq(0)");
    var isDraftFld = false;
    if (dElem.length > 0)
        isDraftFld = true;
    else
        isDraftFld = false;
    var isDraftEnabled = false;
    if (dElem.length > 0 && GetFieldValue(dElem.attr('id')).toLowerCase() == 't')
        isDraftEnabled = true;

    if ($("#IsPublish").val() == "Y") {
        $("#PublishDesign").css("opacity", "0.5");
        changeStatus("published");
    } else if ($("#IsPublish").val() == "N") {
        $("#PublishDesign").css("opacity", "1.0");
        changeStatus("save");
    }
    customAlignTstructFlds(VisibleDCs);
    if (typeof theModeDesign != "undefined" && theModeDesign == "false")
        $("#icons").css("display", "block");
    try {
        if (typeof theModeDesign != "undefined" && theModeDesign == "true") {
            for (var i = 0; i < DCName.length; i++) {
                if (GetDcState(DCName[i]) == "F") {
                    let dcNameId = DCName[i].substr(2);
                    $("#divDc" + dcNameId).removeClass("d-none");
                }
            }
            try {
                $(".ckbGridStretchSwitch").removeClass("disabled").attr("disabled", false);
            } catch (ex) { }
        } else {
            try {
                $(".ckbGridStretchSwitch").addClass("disabled").attr("disabled", true);
            } catch (ex) { }
        }
    }
    catch (ex) { }

    try {
        if (AxAmendmentReadOnly == "true" || AxPegFinalApproval == "true") {
            ReadonlyformPeg();
            AxFormContSetFldActGrid = new Array();
            AxFormContSetGridCell = new Array();
        }
    } catch (ex) { }

    //Temporary code
    var edTime = new Date().getTime();
    var diff = edTime - StartTime;
    console.log("Get Structure & Form load:" + formLogTime + " ms");
    console.log("Page Load Postback:" + pageLogTime + " ms");
    console.log("JQuery Load:" + diff + " ms");
    //End Temporary code
    FieldTypeOnLoad();
}

function GridDcAddEmptyRows() {
    if (axInlineGridEdit || (!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none")) {
        let dcgridCount = 0;
        DCFrameNo.forEach(function (dcID) {
            let _dcInx = $j.inArray(dcID, DCFrameNo);
            if (DCIsGrid[_dcInx] == "True") {
                var isAutoShow = false;
                if (typeof wsPerfFGDcName != "undefined") {
                    if (jQuery.inArray("dc" + dcID, wsPerfFGDcName) != -1) {
                        var dcClientRows = GetDcClientRows(dcID);
                        var lastRow = dcClientRows.getMaxVal();
                        if ((lastRow != 0 && lastRow != "001") || lastRow > 0) {
                            if (isMobile && $("#gridHd" + dcID + " tbody tr").length == 0)
                                isAutoShow = false;
                            else
                                isAutoShow = true;
                        }
                    }
                }
                if (isAutoShow == false) {
                    var dcClientRows = GetDcClientRows(dcID);
                    var lastRow = dcClientRows.getMaxVal();
                    if (lastRow == 0 || lastRow == "001") {
                        dcgridCount++;
                        gridDummyRows = true;
                        lastRow = "001";
                        if (typeof IsObjCustomHtml != "undefined" && IsObjCustomHtml == 'True') {
                            gridDummyRowVal.push(dcID + "~" + lastRow);
                            gridDummyRows = false;
                        } else {
                            if ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit)) {
                                if ($("#gridHd" + dcID + " tbody tr").length == 0) {
                                    editTheRow("", dcID, "", event);
                                    UpdateAxpRowVldInArray(dcID, "001", "1");
                                }
                            } else {
                                editTheRow("", dcID, lastRow, "event");
                                gridDummyRowVal.push(dcID + "~" + lastRow);
                                gridDummyRows = false;
                            }
                        }
                        try {
                            AxAfterAddRow(dcID, lastRow);
                        } catch (ex) { }
                        if (axInlineGridEdit) {
                            let tabDcSt = -1;
                            let tabInd = $.inArray(dcID, TabDCs);
                            if (tabInd > -1)
                                tabDcSt = TabDCStatus[tabInd];
                            if (dcgridCount == 1 && tabDcSt == -1 && (typeof isWizardTstruct == "undefined" || !isWizardTstruct) && $("[id^=dcBlean]").length == 0) {
                                setTimeout(function () {
                                    forceRowedit = true;
                                    gridRowEditOnLoad = true;
                                    if (typeof IsObjCustomHtml != "undefined" && IsObjCustomHtml == 'True') {
                                        var arrEditDiv = new Array();
                                        arrEditDiv.push("#sp" + dcID + "R" + lastRow + "F" + dcID);
                                        AssignJQueryEvents(arrEditDiv);
                                    }
                                    $("#gridHd" + dcID + " tr#sp" + dcID + "R" + lastRow + "F" + dcID + " td:eq(2)").click();
                                    swicthCompressMode(dcID);
                                }, 0);
                            }
                            else if (dcgridCount == 1 && tabDcSt == -1 && (typeof isWizardTstruct == "undefined" || !isWizardTstruct) && $("[id^=dcBlean]").length > 0 && $("#gridHd" + dcID + " tr#sp" + dcID + "R" + lastRow + "F" + dcID).find(".dropzone").length > 0) {
                                setTimeout(function () {
                                    forceRowedit = true;
                                    gridRowEditOnLoad = true;
                                    $("#gridHd" + dcID + " tr#sp" + dcID + "R" + lastRow + "F" + dcID + " td:eq(2)").click();
                                    swicthCompressMode(dcID);
                                }, 0);
                            }
                        }
                    }
                }
            }
        });
        if (gridDummyRowVal.length > 0) {
            let valuesBeforeTilde = gridDummyRowVal.map(function (item) {
                return item.split('~')[0];
            });
            $.each(valuesBeforeTilde, function (index, _tGdc) {
                if (IsDcGrid(_tGdc)) {
                    let tabDcSt = -1;
                    let tabInd = $.inArray(_tGdc, TabDCs);
                    if (tabInd > -1)
                        tabDcSt = TabDCStatus[tabInd];
                    if (tabDcSt == 1) {
                        GetTabData(_tGdc);
                        return false;
                    }
                }
            });
        }
    }
}

//if a tstruct field alignment is defined from Developer Options then align the textdirection after tstruct is loaded
function customAlignTstructFlds(visibleDCs, selectedDcNo, rowNo) {
    var hdnAlgnProp = $("#hdnFldAlgnProp").val();
    if (hdnAlgnProp != "") {
        try {
            var fldAlgnProps = JSON.parse(hdnAlgnProp);
            $.each(fldAlgnProps, function (ind, obj) {
                var dcNo = GetDcNo(obj.fldName);
                if (visibleDCs.indexOf(dcNo) != -1 || selectedDcNo != undefined) {
                    if (IsDcGrid(dcNo)) { //grid dc
                        var rowCnt = 0;
                        //if any grid row is selected(in edit mode) 
                        if (rowNo != undefined && selectedDcNo == dcNo) {
                            var fld = $j("#" + obj.fldName + GetClientRowNo(rowNo, dcNo) + "F" + dcNo);
                            if (fld.length > 0) {
                                fld.css("text-align", obj.alignProp).closest(".edit-mode-content, .grid-stack-item").addClass("btextDir-" + (obj.alignProp == "left" ? "ltr" : "rtl"));
                                if (fld.is("select"))
                                    fld.css("direction", (obj.alignProp == "left" ? "ltr" : "rtl"));
                            }
                        } else { //tabbed or fill gird dc
                            if (selectedDcNo != undefined || visibleDCs.indexOf(dcNo) != -1) {
                                rowCnt = GetDcRowCount(dcNo);
                                for (var i = 1; i <= rowCnt; i++) {
                                    var fld = $j("#" + obj.fldName + GetClientRowNo(i, dcNo) + "F" + dcNo);
                                    if (fld.length > 0) {
                                        fld.css("text-align", obj.alignProp).closest(".edit-mode-content, .grid-stack-item").addClass("btextDir-" + (obj.alignProp == "left" ? "ltr" : "rtl"));
                                        if (fld.is("select"))
                                            fld.css("direction", (obj.alignProp == "left" ? "ltr" : "rtl"));
                                    }
                                }
                            }
                        }
                    } else if (!IsDcGrid(dcNo)) { //non grid dc
                        if (visibleDCs.indexOf(dcNo) != -1 || selectedDcNo == dcNo) {
                            var fld = $("#" + GetComponentName(obj.fldName, dcNo));
                            if (fld.length > 0) {
                                fld.css("text-align", obj.alignProp).closest(".grid-stack-item").addClass("btextDir-" + (obj.alignProp == "left" ? "ltr" : "rtl"));
                                if (fld.is("select"))
                                    fld.css("direction", (obj.alignProp == "left" ? "ltr" : "rtl"));
                            }
                        }
                    }
                }
            });
        } catch (ex) {
            console.warn(ex.message);
        }
    }
}

//$j(document).on("click", "#design", function () {
//    window.location.href = "tstructdesign.aspx?transid=" + transid;
//});

function AssignGridScript() {

    for (var i = 0; i < VisibleDCs.length; i++) {
        var idx = $j.inArray(VisibleDCs[i], DCFrameNo);
        if (idx != -1) {
            if (DCIsGrid[idx].toLowerCase() == "true") {
                AssignGrdFreezeHdrScript(VisibleDCs[i]);
            }
        }
    }
}

//Add the fields which have from list to the MasterRow array.
function SetMRForFromList(flds) {
    if (flds && flds != "") {
        var strFrmLst = flds.split(",");
        for (var i = 0; i < strFrmLst.length; i++) {
            if (strFrmLst[i] != "")
                MasterRow.push(strFrmLst[i].toString());
        }
    }
}

function UpdateDefaultDcRows() {
    for (var i = 0; i < DCFrameNo.length; i++) {
        if (DCIsGrid[i].toLowerCase() == "true") {
            UpdateDcRowArrays(DCFrameNo[i], "001", "Add");
        }
    }
}


//function EvaluateFirstRowExpr(resultJson) {
//    var resval = resultJson.split("*$*");
//    for (var ind = 0; ind < resval.length; ind++) {
//        var strSingleLineText = resval[ind].toString().replace(new RegExp("\\n", "g"), "");
//        if (strSingleLineText == "")
//            return;
//        var tmpActiveRow = "";
//        var tmpActiveDc = "";
//        var myJSONObject = $j.parseJSON(strSingleLineText);

//        if (myJSONObject.data) {
//            var dataJsonObj = myJSONObject.data;

//            for (var i = 0; i < dataJsonObj.length; i++) {
//                var fldName = dataJsonObj[i].n;
//                var fldType = dataJsonObj[i].t;

//                if (dataJsonObj[i].cr)
//                    delRows = dataJsonObj[i].cr;

//                if (fldType == "dc") {
//                    dcNo = fldName.substring(2);
//                    var isDcActive = IsDcVisible(dcNo);
//                    if (IsDcGrid(dcNo) && isDcActive) {
//                        var fields = GetGridFields(dcNo);
//                        tmpActiveRow = AxActiveRowNo;
//                        tmpActiveDc = AxActiveDc;
//                        RegisterActiveRow("001", dcNo);
//                        AxActiveDc = dcNo;
//                        CallEvaluate(dcNo, "001", fields);
//                        AxActiveRowNo = tmpActiveRow;
//                        AxActiveDc = tmpActiveDc;

//                    }
//                }
//            }
//        }
//    }
//}


//Function to set image source to images from the arrays.
function DisplayImages() {
    for (var i = 0; i < imgNames.length; i++) {

        var imgFldId = GetComponentName(imgNames[i], "");
        var img = $j("#" + imgFldId);
        var srcStr = imgSrc[i].toString();
        srcStr = srcStr.replace(new RegExp(";bkslh", "g"), "/");
        var filename = srcStr.split("/");
        var hdnPath = $j("#hdnScriptsUrlpath");
        if (hdnPath.length > 0)
            srcStr = hdnPath.val() + "axpert/" + sid + "/" + imgNames[i].toString() + "/" + filename[filename.length - 1];
        if (img) {
            img.attr("src", srcStr);
            UpdateFieldArray(imgFldId, "0", filename[filename.length - 1], "parent", "LoadData");
            UpdateAllFieldValues(imgFldId, filename[filename.length - 1]);
            img.parents(".image-input").find(".delete-button").removeClass("d-none");
            img.parents(".image-input").find(".imageFileUpload").addClass("d-none");
            img.parents(".image-input").find(".profile-pic").removeClass("d-none");
            navValidator = true;
            SetFormDirty(false);
        }
    }
}

var fldObjHighlt = "";

//Function which is executed on focussing a tstruct field.
function MainFocus(fldObj) {

    //The below code is a hack for avoiding double time focus from advanced search to the picklist field in chrome browser.
    if ($j("#HdnAxAdvPickSearch").val() == "true" && window.chrome) {
        $j("#HdnAxAdvPickSearch").val("false");
        return;
    }
    // if (!fldObj.hasClass("fldmultiSelectInput"))
    //     $(".dropdown-mul").removeClass("active");
    var fldId = fldObj.attr("id");
    if (fldId != undefined) {
        AxFocusedFld=fldId;
        var fName = GetFieldsName(fldId);
        var fldIndex = $j.inArray(fName, FNames);
        var fieldDcNo = GetFieldsDcNo(fldId);
        var fieldRowNo = GetFieldsRowNo(fldId);
        fldObjHighlt = fldObj;
        if (IsDcGrid(fieldDcNo)) {

            // let prevMulTd = $("#" + fldId).parents("td").prevAll(":visible:first");
            // if ($j(prevMulTd).find("select").hasClass("fldmultiSelect") && $j(prevMulTd).find(".edit-mode-content").css("position") == "absolute") {
            //     $j(prevMulTd).find(".edit-mode-content").css("position", "");
            //     $j(prevMulTd).find(".edit-mode-content").css("width", "");
            //     $j(prevMulTd).find(".edit-mode-content").css("left", "");
            // }

            CheckGrdisplayHead(fieldDcNo, fName, fieldRowNo);
            //HighlightRow(fieldDcNo, fieldRowNo, "focus", fldObj.attr("type"));
            UpdateGridRowFlags(fldId, fieldDcNo, fieldRowNo);
        }

        if (isMobile) {
            var latlongName = fldId.substring(0, fldId.lastIndexOf("F") - 3);
            if (latlongName == axplatlongFldName)
                getlatlongitude();
        }

        var fldType = GetFieldType(fName, fldIndex);
        var fldValue = GetFieldValueNew(fldId);

        AxActiveField = fName;
        AxActiveFieldIndex = fldIndex;
        AxActiveRowNo = GetDbRowNo(fieldRowNo, fieldDcNo);
        AxActiveDc = fieldDcNo;
        if (TstructHasPop && IsDcParentGrid(fieldDcNo)) {
            AxActivePRow = AxActiveRowNo;
            AxActivePDc = AxActiveDc;
        }

        if (IsDcGrid(fieldDcNo))
            CheckGrdisplayHead(fieldDcNo, fName, fieldRowNo);

        if (fldType == "Numeric") {
            NumericFldOnfocus(fldObj, fldValue);
        } else if (fldType == "Date/Time" && fldObj.val() == dateString) {
            fldObj.val("");
        } else if (fldType == "Text" && IsDcGrid(fieldDcNo)) {
            //$j("#" + fldId).css("height", "80");
        }

        AxDoBlur = true;
        //if (!fldObj.hasClass("form-select"))
        ShowTooltip(fName, fldObj);
        FldOldValue = fldValue;

        try {
            if (fldId.toLowerCase().startsWith("axpfilepath_")) {
                axpfilepathold = GetFieldValue(fldId);
            }
        } catch (ex) {}

        try {
            var apifldInd = GetFieldIndex(fName);
            var acceptapi = FFieldAcceptApi[apifldInd];
            var acceptapiValue = GetFieldValue(fldId);
            var _autoIntelli = FFieldIntelli[apifldInd];
            if (typeof acceptapi != "undefined" && acceptapi != "" && acceptapiValue == "" && (_autoIntelli == "" || _autoIntelli=="F")) {
                try {
                    ShowDimmer(true);
                    ASB.WebService.GetAcceptDataFromAPI(tstDataId, fName, acceptapi, resTstHtmlLS, SuccessCallbackAcceptApi, OnExceptionAcceptApi);
                } catch (ex) {
                    ShowDimmer(false);
                }

                function SuccessCallbackAcceptApi(result, eventArgs) {
                    if (result.split("♠*♠").length > 1) {
                        tstDataId = result.split("♠*♠")[0];
                        result = result.split("♠*♠")[1];
                    }
                    ShowDimmer(false);
                    resTstHtmlLS = "";
                    var fieldRowNo = GetFieldsRowNo(fldId);
                    var fldDcNo = GetFieldsDcNo(fldId);
                    var fldDbRowNo = GetDbRowNo(fieldRowNo, fldDcNo);
                    SetFieldValue(fldId, result);
                    UpdateFieldArray(fldId, fldDbRowNo, result, "parent", "");
                    MainBlur(fldObj);
                }

                function OnExceptionAcceptApi(result) {
                    ShowDimmer(false);
                }
            }
        } catch (ex) {}


        if (AxFromAssociated == false) {
            if (fldObj.attr("type") == "radio")
                AxOldValue = "";
            else if (AxOldValueOnChange != "" && (AxOldValueOnChange == AxOldValue || AxOldValueOnChange == fldValue)) {
                AxOldValue = "";
                //AxOldValueOnChange="";
            } else
                AxOldValue = fldValue;
        } else {

            AxFromAssociated = false;
        }

        DoFormControl(fldId);
        DoScriptFormControl(fldId, "On Field Enter");

        //If the picklist field is a required field and on clearing the value, 
        //if the service returns the message that the 'field cannot be left empty',then do not show the picklist
        //Here if the field value is empty, do not show the picklist.
        //the same fix is there in ExecPickData also.  
        if (fldValue == "")
            HidePLDiv(false);

        if (isMobile) {
            if (fldId != "" && $("#" + fldId).hasClass("fldAutocomplete") && !$("#" + fldId).hasClass("custmAutoSelect")) {
                $("#" + fldId).addClass("custmAutoSelect");
                $("#" + fldId).parent().find(".virtualKeyboard").click();
            }
        }

        try {
            AxAfterMainFocus(fldId);
        } catch (e) {}
    }
    if (!isMobile)
        fldObj.select();
}


function BeforeBlurdateAutoGenerator(fldObj) {
    var fldId = fldObj.attr("id");
    var fName = GetFieldsName(fldId);
    var fldIndex = $j.inArray(fName, FNames);
    var fldType = GetFieldType(fName, fldIndex);
    if (fldType == "Date/Time") {
        if (fldObj.hasClass("dtRoundLast")) {
            dateAutoGenerator(fldObj, "true");
        } else if (fldObj.hasClass("dtRoundStart")) {
            dateAutoGenerator(fldObj, "false");
        }
    }
}

//Function which is executed on blur of a field.
function MainBlur(fldObj) {
    //if (fldObj.hasClass("multiFldChk") && fldObj.attr("data-separator"))
    //    return;

    if ($(fldObj).data("preventBlur")) {
        $(fldObj).data("preventBlur", false);
        return;
    }
    AxFldBlurFromSelect = "";
    BeforeBlurdateAutoGenerator(fldObj);

    try {
        AxBeforeBlur(fldObj);
    } catch (ex) {

    }

    if (fldObj.hasClass("token-input")) {
        fldObj = fldObj.siblings("input[data-type=checkbox]");
    }

    var fldId = fldObj.attr("id");
    var newFldValue = "";
    var fName = GetFieldsName(fldId);
    var fldIndex = $j.inArray(fName, FNames);

    if (fldIndex == -1)
        return;
    if (typeof $("#" + fldId).attr('disabled') != "undefined")
        return;
    if (FFieldReadOnly[fldIndex] == "True")
        return;
    if (FFieldHidden[fldIndex] == "True")
        return;

    var fldType = GetFieldType(fName, fldIndex);
    var fieldID = fldId;
    lastChangedField = fldId;
    var fieldRowNo = GetFieldsRowNo(fieldID);
    var fldDcNo = GetFieldsDcNo(fieldID);
    var fldDbRowNo = GetDbRowNo(fieldRowNo, fldDcNo);

    
    if (AxDoBlur == true) {

        newFldValue = GetFieldValueNew(fldId);// GetFieldValue(fldId);

        if (fldType == "Numeric") {
            let maskType = "";
            try {
                if (typeof FldMaskType != "undefined" || typeof ScriptMaskFields != "undefined") {                   
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
                        if (AxOldValue == newFldValue && newFldMaskValue != "")
                            newFldValue = newFldMaskValue;
                    }
                }
            } catch (ex) { }
            newFldValue = NumericFldOnBlur(newFldValue, fldIndex);
            var oldValue = removeCommas(AxOldValue);
            var newValue = removeCommas(newFldValue);
            if (oldValue == newValue) {
                if (typeof FldMaskType != "undefined" || typeof ScriptMaskFields != "undefined") {
                    if (maskType != "" && newFldValue != "") {
                        SetFieldValue(fldId, newFldValue);
                    }
                }
                return;
            }
            SetFieldValue(fldId, newFldValue);
        } 

        if (AxOldValue != "" && newFldValue != "") { //do nothing
        } else
            UpdateAssignedFld(fldId);

        if (fldObj.hasClass("initCapField")) {
            var capitalizedString = fldObj.val().toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });
            fldObj.val(capitalizedString);
            newFldValue = capitalizedString;
        }

        if (fldObj.attr("type") == "select-one" || fldObj.prop("type") == "select-one") {
            ComboFillDependents(fieldID, newFldValue);
        }

        if (fldObj.attr("type") == "checkbox" || fldObj.data("type") == "checkbox") {
            AxActiveRowNo = parseInt(fieldRowNo, 10);
            if (TstructHasPop && IsDcParentGrid(fldDcNo)) {
                AxActivePRow = AxActiveRowNo;
                AxActivePDc = AxActiveDc;
            }
            if (IsDcGrid(fldDcNo))
                AxOldValue = GetChkValue(fldId, fldObj.prop("checked"));
            else
                AxOldValue = $("#" + fldId).val();
        }
        if (AxOldValue == newFldValue && typeof fldObj.attr("type") !="undefined" && (fldObj.attr("type") != "checkbox" && fldObj.data("type") != "checkbox")) { //&& !fldId.toLowerCase().startsWith("axptm_")
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
                if (maskType != "" && newFldValue != "") {
                    SetFieldValue(fldId, newFldValue, true);
                }
            }
            return;
        }
        SetFieldValue(fldId, newFldValue, true);
        AxFldBlured = fldId;
        UpdateGridRowFlags(fldId, fldDcNo, fieldRowNo);
        AxGlobalChange = true;

        var dcIndx = GetFormatGridIndex(fldDcNo);
        if (dcIndx != -1)
            UpdateFormatGridTotal(fName, fldDcNo, fieldRowNo, dcIndx);

        if (!errorFlag) {

            if (!ValidateField(fldObj, fldDbRowNo, fldIndex, fldType, AxActiveField)) {
                setTimeout(function () {
                    fldObj.focus();
                }, 0);
                errorFlag = true;
                errorField = fldId;
                //return;
            } else {
                errorFlag = false;
            }
        }
        if (TstructHasPop) {
            if (IsParentField(AxActiveField, AxActiveDc)) {
                RegisterActivePRow(fieldRowNo, AxActiveDc);
                UpdatePopUpParents(fieldID);
            }
        }

        if (IsPickListField(fieldID) == true) {
            AxBlurAction = "ForceWebService";
            if ($j("#" + fldId).hasClass("fldAutocomplete"))
                AxBlurAction = "Default"; //eventhough it is picklist and if it is auto complete
            var pickIdFld = $j("#pickIdVal_" + fieldID);
            var pickVal = newFldValue;
            if (pickIdFld.val() != "") {
                pickVal = pickIdFld.val() + "¿" + newFldValue;
            }
            if (blurNextPreventId.substring(0, 7) == "plClear") {
                $j("#" + blurNextPreventId.substring(7)).val("");
                blurNextPreventId = "";
                return;
            }
            UpdateFieldArray(fieldID, fldDbRowNo, pickVal, "parent");
        } else
            AxBlurAction = "Default";

        if (errorFlag == true) {
            errorFlag = false;
            return;
        }
        if (AxRulesDefValidation == "true") {
            if (!errorFlag) {
                if (!AxRulesDefParser(fldId, "field", "validate")) {
                    setTimeout(function () {
                        fldObj.focus();
                    }, 0);
                    errorFlag = true;
                    errorField = fldId;
                } else {
                    errorFlag = false;
                }
            }
            if (errorFlag == true) {
                errorFlag = false;
                return;
            }
        }

        if (arrRefreshDcs.length > 0) {
            for (var i = 0; i < arrRefreshDcs.length; i++) {
                var arrDcNos = arrRefreshDcs[i].split(':');
                if (arrDcNos[1] == "dc" + fldDcNo && arrRefreshFlds[i].indexOf(fName) > -1) {
                    arrRefreshFldDirty[i] = true;
                    break;
                }
            }
        }

        AxExecFormControl = true;
        //if(developer option is exists then avoaid call below funtion)
        let isNotFillDep = false;
        if (typeof AxpNotFillDepFields != "undefined" && AxpNotFillDepFields != "") {
            var AxpNotFillDepField = AxpNotFillDepFields.split(",");
            if (AxpNotFillDepField.indexOf(fName) > -1) {
                isNotFillDep = true;
            }
        }

        if (AxRulesDefComputescript == "true")
            isOverridExpression = AxRulesDefParser(fieldID, "field", "computescript");

        if (isNotFillDep == false)
            DoSetDependents(fieldID);
        else {
            var rowNo = GetFieldsRowNo(fieldID);
            var fldDbRowNo = GetDbRowNo(rowNo, fldDcNo);
            var fldValue = GetFieldValue(fieldID);
            UpdateFieldArray(fieldID, fldDbRowNo, fldValue, "parent");
        }
        //let isOverridFomrControl=true;
        //if(AxRuesDefFormcontrol=="true")
        //   isOverridFomrControl= AxRulesDefParser(fldId, "field", "formcontrol");
        if (AxExecFormControl && AxFldFormControl == "") {
            DoFormControl(fldId);
            DoScriptFormControl(fldId, "On Field Exit");
        }

        if (AxRuesDefScriptFCP == "true")
            AxRulesScriptsParser("formcontrol", fName);

        if (fldType == "Numeric") {
            var isPositive = GetFieldProp(fldIndex, "onlyPositive");
            if (isPositive.toUpperCase() == "T" && parseInt(newFldValue) < 0) {
                showAlertDialog("warning", 2052, "client", FCaption[fldIndex]);
                fldObj.select();
            }
        }
        if (newFldValue != "" && fldType.toLowerCase() == "date/time") {
            var glCulture = eval(callParent('glCulture'));
            if (glCulture != undefined && glCulture == "en-us") {
                newFldValue = GetDateStr(newFldValue, "mm/dd/yyyy", "dd/mm/yyyy");
                UpdateFieldArray(fieldID, fldDbRowNo, newFldValue, "parent");
            }
        }
    }

    if ((!axInlineGridEdit && AxpGridForm == "form") || (isMobile && mobileCardLayout != "none" && !axInlineGridEdit))
        formGridRowBlur(fldObj);

    AxProcessObjRefresh(fldObj);
    try {
        if (transid == "ad_pa")
            publishAPIInputJSON(fieldID);
        AxAfterBlur(fldObj);
    } catch (ex) {}

    try {
        if (transid == 'axurg' && fieldID == 'pusername000F1') {
            UserActivateBtns(fieldID);
        }

        if (transid == "b_sql" && fieldID == "cachedata000F1") {
            switchOffCacheData('true');
        }

        /*if (transid == "apidg" && (fieldID == "APItype000F1" || fieldID == "config_url_name000F1")) {*/
        if (transid == "apidg" && fieldID == "config_url_name000F1") {
            OnChangeApiType(fieldID);
        }
    } catch (ex) { }
}

function blurThis(blurField) {
    $(blurField).data("preventBlur", true).blur();
}


function UpdateGridRowFlags(fldId, fldDcNo, fldRowNo) {
    if (gridRowEditOnLoad) {
        if (IsDcGrid(fldDcNo)) {
            let _isExitDummy = false;
            gridDummyRowVal.map(function (v) {
                if (v.split("~")[0] == fldDcNo)
                    _isExitDummy = true;
            });
            if (_isExitDummy) {
                gridDummyRowVal.splice($.inArray(fldDcNo.toString() + "~" + fldRowNo, gridDummyRowVal), 1);
                gridDummyRows = false;
                gridRowEditOnLoad = false;
                AxActiveDc = fldDcNo;
                AxActiveRowNo = "1";
                var fields = GetGridFields(fldDcNo);
                CallEvaluateOnAddPerf(fldDcNo, fldRowNo, fields, "AddRow");
            }
        }
    }
}

//Function which calls the validation functions for the fields.
function ValidateField(fldObj, fldDbRowNo, fldIndex, fldType, fName) {

    if (fldType == "Date/Time") {
        if (typeof FFieldType[fldIndex] != "undefined" && FFieldType[fldIndex] == 'Timestamp') {
            return true;
        }
        var dtTitle = "";
        if (fldObj.attr("title") != undefined && fldObj.attr("title") != null)
            dtTitle = fldObj.attr("title");

        if (dtTitle == dateString && fldObj.val() == "") fldObj.val(dateString);
        var isproperdate = ValidateDate(fldObj);
        if (isproperdate) {
            UpdateFieldArray(fldObj.attr("id"), fldDbRowNo, fldObj.val(), "parent");
            if (!ValidateReservedWord(fldObj, "value.")) {
                return false;
            }
            // Validate the pattern
            if (!ValidateFieldPattern(fldObj, fName, fldIndex)) {
                return false;
            }

            // Validate Expression
            if (!ValidateExpression(fldObj, fldIndex)) {
                return false;
            }
            // Validate Field Type Expression
            if (!ValidateFieldTypeExpression(fldObj, fldIndex)) {
                return false;
            }
            return true;
        } else {
            UpdateFieldArray(fldObj.attr("id"), fldDbRowNo, "", "parent");
            return false;
        }

    } else {

        if (!ValidateReservedWord(fldObj, "value.")) {
            return false;
        }
        // Validate the pattern
        if (!ValidateFieldPattern(fldObj, fName, fldIndex)) {
            return false;
        }

        // Validate Expression
        if (!ValidateExpression(fldObj, fldIndex)) {
            return false;
        }
        // Validate Field Type Expression
        if (!ValidateFieldTypeExpression(fldObj, fldIndex)) {
            return false;
        }

        // Validate Formula
        if (!ValidateFldValFormula(fldObj, fldIndex, fName)) {
            return false;
        }
        return true;
    }
}

//Function which validates the field value with the reserved words used in the application.
function ValidateReservedWord(obj, alertMessage) {

    var checkRW = "";
    var checkObjVal = GetFieldValue(obj.attr("id"));// obj.val();

    if (obj.prop("disabled") != true && checkObjVal != null) {

        if (checkObjVal.indexOf("~") != -1) { //checkRW = "~";
        } else if (checkObjVal.indexOf("***") != -1) {
            checkRW = "***";
        } else if (checkObjVal.indexOf("**") != -1) {
            checkRW = "**";
        } else if (checkObjVal.indexOf("###") != -1) {
            checkRW = "###";
        } else if (checkObjVal.indexOf("`") != -1) {
            checkRW = "`";
        } else if (checkObjVal.indexOf("*$*") != -1) {
            checkRW = "*$*";
        }
        if (checkRW != "") {
            /*showAlertDialog("error", 2053, "client", alertMessage + "^♠^" + checkRW);*/
            showAlertDialog("error", 2053, "client", "^♠^" + checkRW);
            let _fValue = obj.val();
            _fValue = _fValue.replace(/\*\*\*/g, " ");
            _fValue = _fValue.replace(/\*\*/g, " ");
            _fValue = _fValue.replace(/###/g, " ");
            _fValue = _fValue.replace(/`/g, " ");
            _fValue = _fValue.replace(/\*\$\*/g, " ");
            obj.val(_fValue);
            UpdateAllFieldValues(obj.attr("id"), _fValue);
            //return false;
        }
    }
    return true;
}

//Function which validates the field value for the given pattern definition.
function ValidateFieldPattern(fldObj, fName, fldIndex) {

    var fldId = fldObj.attr("id");
    var indx = fldId.lastIndexOf("F");
    var fname = "";
    if (indx != -1) {
        fname = fldId.substring(0, parseInt(indx, 10) - 3);
    }
    for (var j = 0; j < PatternNames.length; j++) {

        if (fname == PatternNames[j].substring(3)) {
            if (!EvaluatePattern(fldId, Patterns[j], fldIndex))
                return false;
            break;
        }
    }

    return true;
}

//Function which validates the field value with the result of the evaluated expression.
function ValidateExpression(fldObj, fldno) {

    var flname = fldObj.attr("id");
    var flval = GetFieldValue(flname);
    var setOldVal = false;
    var indx = parseInt(fldno, 10);
    var vexpr = "";
    if (FldValidateExpr[indx] != undefined)
        vexpr = FldValidateExpr[indx].toString();
    if (vexpr != undefined && vexpr != "" && (flval && (flval != "" || flval != dateString))) {
        var _fldCap = FCaption[indx];
        if (_fldCap == '&nbsp')
            _fldCap = "";
        if (_fldCap != '')
            _fldCap = _fldCap + " ";

        let _fldName = FNames[indx];
        if (_fldName != "" && AxSetFldCaption.length > 0) {
            let _thisEle = AxSetFldCaption.filter(x => x.indexOf('setfieldcaption~' + _fldName + '^') > -1);
            if (_thisEle.length > 0) {
                _fldCap = _thisEle[0].split('^')[1];
                if (_fldCap != '')
                    _fldCap = _fldCap + " ";
            }
        }

        var fResult = Evaluate(flname, flval, vexpr, "vexpr");

        if (fResult != 'T' && fResult != 't' && fResult != true) {
            var cutMsg = eval(callParent('lcm[52]'));
            var firstChar = fResult.substring(0, 1);
            var alertMsg = fResult.substring(1);
            if (firstChar == "_") {
                showAlertDialog("error", _fldCap + alertMsg);
            } else if (firstChar == "?") {
                showAlertDialog("error", _fldCap + alertMsg);
                fldVal = GetFieldValueFromArray(flname);
                SetFieldValue(flname, fldVal);
                var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
                UpdateFieldArray(flname, dRowNo, fldVal, "parent", "");
                return false;
            } else if (firstChar == "*") {
                showAlertDialog("error", _fldCap + alertMsg);
                return false;
            } else if (firstChar == "#") {
                let AxErrCode = "";
                try {
                    if (AxvalErrorCodes != "") {
                        var Xmlparser = new DOMParser();
                        let xmlDoc = Xmlparser.parseFromString(AxvalErrorCodes, "text/xml");
                        AxErrCode = xmlDoc.getElementsByTagName("E" + alertMsg)[0].childNodes[0].nodeValue;
                    } else
                        AxErrCode = fResult;
                } catch (ex) {
                    AxErrCode = fResult;
                }
                if (getDevOptionValue() == "true") {
                    alert(_fldCap + AxErrCode + ". " + cutMsg)
                    SetFieldValue(flname, FldOldValue);
                    var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
                    UpdateFieldArray(flname, dRowNo, FldOldValue, "parent", "");
                    return false;
                } else {
                    if (confirm(_fldCap + AxErrCode + ". " + cutMsg)) {
                        SetFieldValue(flname, FldOldValue);
                        var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
                        UpdateFieldArray(flname, dRowNo, FldOldValue, "parent", "");
                        return false;
                    } else {
                        return true;
                    }
                }
            } else {
                if (fResult == "MessageSetAxFont") {
                    return true;
                } else {
                    if (getDevOptionValue() == "true") {
                        alert(_fldCap + fResult + ". " + cutMsg)
                        SetFieldValue(flname, FldOldValue);
                        var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
                        UpdateFieldArray(flname, dRowNo, FldOldValue, "parent", "");
                        return false;
                    } else {
                        if (confirm(_fldCap + fResult + ". " + cutMsg)) {
                            //SetFieldValue(flname, "");
                            //var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
                            //UpdateFieldArray(flname, dRowNo, "", "parent", "");
                            //Refer Bug: HEA000082, Old Value assigning back to field if new value is not valid..  
                            SetFieldValue(flname, FldOldValue);
                            var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
                            UpdateFieldArray(flname, dRowNo, FldOldValue, "parent", "");

                            return false;
                        } else {
                            return true;
                        }
                    }
                }
            }
        }
    }
    return true;
}

//Function which validates the field value if given as formula of axp_formula field
function ValidateFldValFormula(fldObj, fldno, fldName) {
    if (fldName != "" && fldName.toLowerCase() == "axp_formula") {
        var flname = fldObj.attr("id");
        try {
            let _thisFldDcno = GetFieldsDcNo(flname);
            if (typeof AxpListofRagVarflds != "undefined" && typeof AxpListofRagVarflds[0] != "undefined" && AxpListofRagVarflds[0] != "") {
                let _rgrdVar = "";
                let _dupAxActiveRowNo = AxActiveRowNo;
                AxpListofRagVarflds[0].split(',').forEach(function (ele, ind) {
                    if (ele != "") {
                        let _thisFld = ele;
                        let _rFldDcno = GetDcNo(_thisFld);
                        if (_thisFldDcno == _rFldDcno && IsDcGrid(_rFldDcno)) {
                            var curRowNum = 1;
                            var rowCnt = 0;
                            rowCnt = parseInt(GetDcRowCount(_rFldDcno), 10);
                            let _fldIndx = GetFldNamesIndx(_thisFld);
                            var expression = Expressions[_fldIndx].toString();
                            for (var i = curRowNum; i <= rowCnt; i++) {
                                var rowNo = GetClientRowNo(i, _rFldDcno);
                                AxActiveRowNo = GetDbRowNo(rowNo, _rFldDcno);
                                var tmpFldId = _thisFld + rowNo + "F" + _rFldDcno;
                                Evaluate(tmpFldId, rowNo, expression, "expr");
                            }
                            if (_rgrdVar == "")
                                _rgrdVar += _thisFld;
                            else
                                _rgrdVar += "," + _thisFld;
                        }
                    }
                });
                if (_rgrdVar != "")
                    AxpListofRagVarflds[0] = AxpListofRagVarflds[0].replace(_rgrdVar, "");
                AxActiveRowNo = _dupAxActiveRowNo;
            }
        } catch (ex) { }

        var flval = GetFieldValue(flname);
        var indx = parseInt(fldno, 10);
        if (flval && (flval != "" || flval != dateString)) {
            var fResult = EvaluateFormula(flname, flval, flval, "vexpr");

            if (fResult != 'T' && fResult != 't' && fResult != true) {
                showAlertDialog("error", fResult);
                SetFieldValue(flname, "");
                var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
                UpdateFieldArray(flname, dRowNo, "", "parent", "");
            }
        }
    }
    return true;
}

function GetRandnumber(length) {
    return Math.floor(Math.pow(10, length - 1) + Math.random() * (Math.pow(10, length) - Math.pow(10, length - 1) - 1));
}

function FieldTypeOnLoad() {
    //$(".randomnum").each(function () {
    $("[id^='divDc']:not(:has(.editWrapTr)) .randomnum").each(function () {
        ValidateFieldTypeExpression($(this));
    });

}

//Function which validates the DWB field value with the result of the evaluated expression.
function ValidateFieldTypeExpression(fldObj, fldno) {

    var flname = fldObj.attr("id");
    var rid = $j("#recordid000F0").val();
    var flval = GetFieldValue(flname);
    var fldtype = GetDWBFieldType(GetFieldsName(flname));
    var setOldVal = false;
    if (flval != undefined && flval != "" && (fldtype != "" && fldtype != "Random Number")) {
        var fResult = validatedwbfieldtype(flval, fldtype, flname); // Evaluate(flname, flval, vexpr, "vexpr");        
        if (fResult != 'T' && fResult != 't' && fResult != true) {
            SetFieldValue(flname, FldOldValue);
            var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
            UpdateFieldArray(flname, dRowNo, FldOldValue, "parent", "");
            return false;
        } else {
            return true;
        }
    } else {
        if (fldtype == "Random Number" && rid == "0") {
            var rno = GetRandnumber(fldObj.attr("maxlength") != undefined ? fldObj.attr("maxlength") : "0");
            SetFieldValue(flname, rno);
            var dRowNo = GetDbRowNo(GetFieldsRowNo(flname), GetFieldsDcNo(flname));
            UpdateFieldArray(flname, dRowNo, rno, "parent", "");
        }


        return true;
    }
    return true;
}

function validatedwbfieldtype(value, dwbfldtype, flname) {
    var vregex = "";
    var patternErrMsg = "";
    var pwdpatternErrMsg = "";
    if (dwbfldtype != "") {
        var tmpFldCaption = "";
        tmpFldCaption = GetFieldCaption(flname);
        if (value != "") {
            switch (dwbfldtype) {
                case 'Email':
                    //vregex = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
                    vregex = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)+$";
                    if (!CheckIsvalidfield(value, vregex)) {
                        var cutMsg = eval(callParent('lcm[53]'));
                        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                    }
                    break;
                case 'URL':
                    vregex = /(http(s)?:\\)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?/;
                    if (!CheckIsvalidfield(value, vregex)) {
                        var cutMsg = eval(callParent('lcm[502]'));
                        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                    }
                    break;
                case 'IP Address':
                    vregex = "^[0-9]\+\.[0-9]\+\.[0-9]\+\.[0-9]\+$";
                    if (!CheckIsvalidfield(value, vregex)) {
                        var cutMsg = eval(callParent('lcm[493]'));
                        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                    }
                    break;
                case 'Password':
                    let pwdopts = $("#" + flname).attr("data-pwdopts").split("~");
                    if (pwdopts[0] == "True") {
                        vregex = "^[a-zA-Z0-9~!@#$%^&*()_+]+$";
                        if (pwdopts[1] != "" && value.length < pwdopts[1]) {
                            var cutMsg = eval(callParent('lcm[513]'));
                            cutMsg = cutMsg.replace("_", pwdopts[1]);
                            pwdpatternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                        } else if (!CheckIsvalidfield(value, vregex)) {
                            var cutMsg = eval(callParent('lcm[450]'));
                            pwdpatternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                        }
                    } else if (pwdopts[0] == "False") {
                        vregex = "^[0-9]+$";
                        if (pwdopts[1] != "" && value.length < pwdopts[1]) {
                            var cutMsg = eval(callParent('lcm[513]'));
                            cutMsg = cutMsg.replace("_", pwdopts[1]);
                            pwdpatternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                        } else if (!CheckIsvalidfield(value, vregex)) {
                            var cutMsg = eval(callParent('lcm[514]'));
                            pwdpatternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                        }
                    }
                    break;
                case 'Web site':
                    vregex = "^((https?|ftp|smtp):\/\/)?(www.)?[a-z0-9]+\.[a-z]+(\/[a-zA-Z0-9#]+\/?)*$";
                    if (!CheckIsvalidfield(value, vregex)) {
                        var cutMsg = eval(callParent('lcm[494]'));
                        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                    }
                    break;
                //case 'Mobile Number': //This need to match with all the countries mobile number formats.
                //    vregex = "^([+][9][1]|[9][1]|[0]){0,1}([6-9]{1})([0-9]{9})$";
                //    if (!CheckIsvalidfield(value, vregex)) {
                //        var cutMsg = eval(callParent('lcm[495]'));
                //        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                //    }
                //    break;
                case 'Phone Number':
                    vregex = "^[0-9]{5,10}$";
                    if (!CheckIsvalidfield(value, vregex)) {
                        var cutMsg = eval(callParent('lcm[496]'));
                        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                    }
                    break;
                case 'Pin Code':
                    vregex = "^[1-9][0-9]{5}$";
                    if (!CheckIsvalidfield(value, vregex)) {
                        var cutMsg = eval(callParent('lcm[497]'));
                        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                    }
                    break;
                case 'Zip Code':
                    vregex = "^[0-9]{5}(?:-[0-9]{4})?$";
                    if (!CheckIsvalidfield(value, vregex)) {
                        var cutMsg = eval(callParent('lcm[498]'));
                        patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                    }
                    break;
                case 'Random Number':
                    return true;
                    break;
                default:
                    return true;
            }
        }
    }
    if (pwdpatternErrMsg != "") {
        showAlertDialog("warning", pwdpatternErrMsg);
        return false;
    } else if (patternErrMsg != "") {
        // var cutMsg = eval(callParent('lcm[58]'));
        // if (!confirm(patternErrMsg + cutMsg))
        showAlertDialog("error", patternErrMsg);
        return false;
    } else {
        return true;
    }
}

// Evaluates patters, if no error message or user continues returns true else False.         
function EvaluatePattern(fld, pattrn, fldIndex) {

    var value = ""
    if ($j("#" + fld))
        value = GetFieldValue(fld);// $j("#" + fld).val();

    if (value == undefined || value == null || value == "")
        return true;

    if (value != "") {

        var patternErrMsg = "";
        var tmpFldCaption = "";
        tmpFldCaption = GetFieldCaption(fld, fldIndex);
        switch (pattrn) {
            case 'isemail':
                if (!CheckAxpEmail(value)) {
                    var cutMsg = eval(callParent('lcm[53]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
            case 'isalpha':
                if (!CheckAlpha(value)) {
                    var cutMsg = eval(callParent('lcm[54]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
            case 'isnumeric':
                if (!CheckIsNumeric(value)) {
                    var cutMsg = eval(callParent('lcm[55]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
            case 'isalphanumeric':
                if (!CheckAlphaNumeric(value)) {
                    var cutMsg = eval(callParent('lcm[56]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
            case 'isphone':
                if (!CheckIsPhone(value)) {
                    var cutMsg = eval(callParent('lcm[57]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
                case 'Initcaps':
                if (!CheckIsInit(value)) {
                    var cutMsg = eval(callParent('lcm[525]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
                   case 'Caps':
                if (!CheckIsCaps(value)) {
                    var cutMsg = eval(callParent('lcm[526]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
                   case 'small':
                if (!CheckIsSmall(value)) {
                    var cutMsg = eval(callParent('lcm[527]'));
                    patternErrMsg = (tmpFldCaption == "" ? tmpFldCaption : tmpFldCaption + ": ") + cutMsg;
                }
                break;
            default:
                return true;
        }
    }
    if (patternErrMsg != "") {
        var cutMsg = eval(callParent('lcm[58]'));
        if (!confirm(patternErrMsg + cutMsg))
            return false;
    } else {
        return true;
    }
}



//Function to check for Phone.
function CheckIsPhone(phone) {
    var re = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
    return re.test(phone);
}

function CheckIsInit(str) {
    var strsplit =str.split(' ')
    var checkInit ="";
    strsplit.forEach(myFunction);
    function myFunction(item) {
     checkInit += isUppercase(item).toString();
    }
    if(checkInit.indexOf("false") != -1)
        return false;
     else
        return true;
        }
function isUppercase(word){
	 return /^\p{Lu}/u.test( word );
    }
function CheckIsCaps(str) {
    //var strsplit =str.split(' ')
    //var CheckIsCaps ="";
    //strsplit.forEach(checkCaps);
    //function checkCaps(item) {
    // CheckIsCaps += /^[A-Z]+$/.test(item);
    //}
   var CheckIsCaps = /^[A-Z ]+$/.test(str);
   //if(CheckIsCaps.indexOf("false") != -1)
   if(!CheckIsCaps)
        return false;
    else
        return true;
    }

function CheckIsSmall(str) {
    //var strsplit =str.split(' ')
    //var CheckIsSmall ="";
    //strsplit.forEach(checkSmall);
    //function checkSmall(item) {
     var CheckIsSmall = /^[a-z ]+$/.test(str);;
    //}
    // if(CheckIsSmall.indexOf("false") != -1)
    if(!CheckIsSmall)
        return false;
    else
        return true;
        }
//Function to check for custom fields regexp.
function CheckIsvalidfield(fieldv, regexv) {

    var regex = new RegExp(regexv);
    return regex.test(fieldv);
}

//Function which removes comma and selects the value in a numeric field on focus.
function NumericFldOnfocus(fieldObj, fieldValue) {

    if (fieldValue != "") {
        if ((fieldValue.length > 3) && (fieldValue.indexOf(',') != -1))
            fieldValue = removeCommas(fieldValue);
    }

    var fld = document.getElementById(fieldObj.attr("id"));
    fld.select();

    AxOldValue = fieldValue;
    //if (fieldObj.data("attr") != undefined)
    //    CallSetFieldValue(fieldObj.attr("id"), fieldObj.data("attr"), "fromNumFocus");
    var fld = document.getElementById(fieldObj.attr("id"));
    fld.select();
    try {
        //In chrome and firefox the selected text is getting unselected.The onmouseup event that is causing the selection to get unselected text.
        if (!$j.browser.msie) {

            var tempSouper = fieldObj;
            tempSouper.select();
            //window.setTimeout(function () {
            //    tempSouper.select();
            //}, 10);

            fieldObj.mouseup(function () {
                // Prevent further mouseup intervention
                fieldObj.unbind("mouseup");
                return false;
            });
        }
    } catch (ex) {}

}

//Function which adds comma and decimal point to the value of the numeric field.
function NumericFldOnBlur(newcurrFldValue, fldIndex) {
    var numWithDecimals = 0;
    if (FCustDecimal[fldIndex] == "True" && typeof gloAxDecimal != "undefined" && gloAxDecimal > -1)
        numWithDecimals = fixit(newcurrFldValue, gloAxDecimal);
    else
        numWithDecimals = fixit(newcurrFldValue, FDecimal[fldIndex]);
    var applyComma = GetFieldProp(fldIndex, "applyComma");
    if (applyComma == "T" || (typeof isexpApplyComma != "undefined" && isexpApplyComma == true)) {
        if (Math.abs(numWithDecimals) > 999) {
            numWithDecimals = CommaFormatted(numWithDecimals).toString();
        }
    }
    if (typeof isexpApplyComma != "undefined")
        isexpApplyComma = false;

    return numWithDecimals;
}

//Function to add decimal point to the value based on the no of digits given in dec.
function fixit(value, dec) {

    var val = value.toString();

    if (val == "")
        return 0;
    else if (val == "0")
        return parseFloat(val).toFixed(dec);

    var dc = parseInt(dec, 10);
    var x = val.replace(/,/g, "");
    if (isNaN(x)) return 0;
    if (dc == 0) {
        return Math.floor(x);
    } else {
        x = RoundOff(x, dc);
        var x1 = parseFloat(x).toFixed(dc);
        return x1;
    }
}

//Function which fills the dependent combo values from the combo arrays.
function ComboFillDependents(fieldID, value) {

    var fName = GetFieldsName(fieldID);
    var rowFrameNo = GetFieldsRowFrameNo(fieldID);
    var dcNo = GetFieldsDcNo(fieldID);
    var rowNo = GetFieldsRowNo(fieldID);
    var dbRowNo = GetDbRowNo(rowNo, dcNo);

    for (var i = 0; i < ComboParentField.length; i++) {
        //var parFldName = GetFieldsName(ComboParentField[i]);
        if (ComboParentField[i] == fName && ComboParentValue[i] == value) {
            var depFldName = ComboDepField[i];
            var depDc = GetDcNo(depFldName);
            var depFldValue = ComboDepValue[i].toString();
            if (dcNo == depDc) {
                var depFldId = ComboDepField[i] + rowFrameNo;
                var depFld = $j("#" + depFldId);
                if (depFld.length > 0) {
                    CallSetFieldValue(depFldId, depFldValue);
                    UpdateFieldArray(depFldId, dbRowNo, depFldValue, "parent", "");
                }
            } else {
                //if the parent is in NonGrid and dep is in grid
                var rCnt = GetDcRowCount(depDc);
                for (var j = 1; j <= rCnt; j++) {
                    var rowNo = GetClientRowNo(j, depDc);
                    var depFldId = depFldName + rowNo + "F" + depDc;
                    var depFld = $j("#" + depFldId);
                    if (depFld.length > 0) {
                        CallSetFieldValue(depFldId, depFldValue);
                        UpdateFieldArray(depFldId, j, depFldValue, "parent", "");
                    }
                }
            }
        }
    }
}

//Function which calls the GetDependents function based on the blur action.
function DoSetDependents(fieldID) {
    if (isAutocomBlur == false)
        AdditionalRunTimeMsg("Initiating GetDependency ");
    else
        GetCurrentTime("Tstruct Get dependency (ws call)");
    isAutocomBlur = true;
    var isDefaultDeps = false;
    if (isRapidLoad == "True" && defaultDepFlds != undefined && defaultDepFlds != "") {
        if (defaultDepFlds.trim() == "*")
            isDefaultDeps = true;
        else {
            var fldName = GetFieldsName(fieldID);
            var arrFlds = defaultDepFlds.split(",");
            if (arrFlds.indexOf(fldName) > -1) {
                isDefaultDeps = true;
            }
        }
    }

    if (AxBlurAction == "ForceWebService") {
        if (isRapidLoad == "True" && !isDefaultDeps)
            CheckRapidDependency(fieldID);
        else
            GetDependents(fieldID);
    } else if (AxBlurAction == "Default") {
        if (isRapidLoad == "True" && !isDefaultDeps)
            CheckRapidDependency(fieldID);
        else {
            if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
                CheckDependencyPerf(fieldID);
            else
                CheckDependency(fieldID);
        }
    }

    if (fieldID.toLowerCase().startsWith("axpfilepath_")) {
        AxpFilePathChange(fieldID);
    }
}

//Function to get the non grid expression dependents for a grid field.
//function GetExpDependents(fieldID) {
//    var fieldName = GetFieldsName(fieldID);
//    var fldInd = GetFieldIndex(fieldName);
//    var dcNo = GetFieldsDcNo(fieldID);
//    var expDepArray = new Array();
//    var depArray;
//    if (fldInd != -1) {
//        depArray = FldDependents[fldInd].toString().split(",");
//        for (var i = 0; i < depArray.length; i++) {
//            var dField = depArray[i].toString();
//            var depFirstChar = dField.substring(0, 1);
//            var depfName = dField.substring(1);
//            var depFldDc = GetDcNo(depfName);

//            if (depFirstChar == "e" && dcNo > depFldDc && !IsDcGrid(depFldDc)) {
//                expDepArray.push(depfName);
//            }
//        }
//    }

//    return expDepArray;
//}

function GetFastDataFlds(depFlds) {
    var arrDepFlds = depFlds.split(",");
    var fDataFlds = rSFlds.split(",");
    var tempArray = new Array();
    //rSFlds
    var strFlds = "";
    for (var i = 0; i < arrDepFlds.length; i++) {
        var newFldName = arrDepFlds[i].toString().substring(1);
        if ($j.inArray(newFldName, fDataFlds) != -1) {
            if ($j.inArray(newFldName, tempArray) == -1) {
                if (strFlds == "")
                    strFlds = newFldName;
                else
                    strFlds += "," + newFldName;
                tempArray.push(newFldName);
            }
        }
    }

    return strFlds;
}

//Function checks if the dependency chain conatins any sql field which is not in fastdata, if yes then returns fastdataflds as empty string else returns same string
function CheckFastOrDB(depArray, fastDataFlds) {
    var arrFastFlds = fastDataFlds.split(",");
    if (depArray != undefined) {
        for (var di = 0; di < depArray.length; di++) {
            var dField = depArray[di].toString();
            var depFirstChar = dField.substring(0, 1);
            var depfName = dField.substring(1);
            if (depFirstChar == 's' || depFirstChar == "f") {
                if ($j.inArray(depfName, arrFastFlds) == -1)
                    return "";
            }
        }
    }
    return fastDataFlds;
}

function CheckRapidDependency(fieldID, calledFrom) {
    AxWaitCursor(true);
    var isServiceCalled = false;
    var fieldName = GetFieldsName(fieldID);
    var dcNo = GetFieldsDcNo(fieldID);
    var rowNo = GetFieldsRowNo(fieldID);
    var fldDbRowNo = GetDbRowNo(rowNo, dcNo);
    var fldValue = GetFieldValue(fieldID);

    var fldInd = GetFieldIndex(fieldName);
    //For numeric fld on blur, the comma has to be removed before updating in the field array.
    if (fldInd != -1) {
        var fldType = GetFieldType(fieldName, fldInd);
        if (fldType == "Numeric")
            fldValue = removeCommas(fldValue);
    }

    UpdateFieldArray(fieldID, fldDbRowNo, fldValue, "parent");

    var depArray;
    var depStr = "";
    var depType = "";
    if (fldInd != -1) {
        depStr = FldRapidDeps[fldInd].toString();
        depType = FldRapidDepType[fldInd].toString();
    }
    if (depStr != "")
        depArray = depStr.split(",");
    var fDataFlds = "";
    if (rSFlds != "") {
        fDataFlds = GetFastDataFlds(depStr);
        fDataFlds = CheckFastOrDB(depArray, fDataFlds);
    }

    if (depType == "c") {
        //Evaluate expression for all expression dependents
        var depExprFlds = FldRapidExpDeps[fldInd].toString().split(',');
        var eIdx = 0;
        var depExpLen = depExprFlds.length;
        for (eIdx = 0; eIdx < depExpLen; eIdx++) {
            EvaluateAxFunction(depExprFlds[eIdx].substring(1), fieldID);
        }
    } else if (depType == "w") {
        var dField;
        var depFirstChar;
        var depfName;
        var dfldInd;
        var depFldDc;
        var di = 0;
        var depLength = depArray.length;
        var callService = false;
        var isGridDc;
        var depDcRCnt = 0;
        for (di = 0; di < depLength; di++) {

            dField = depArray[di].toString();
            depFirstChar = dField.substring(0, 1);
            depfName = dField.substring(1);
            dfldInd = GetFieldIndex(depfName);
            depFldDc = GetDcNo(depfName);

            //if (AxEditActiveDcNo != parseInt(GetFieldsDcNo(fieldID)) && AxEditActiveDcNo != "")
            //    return;

            //If the Accept with expression field has itself in the dep array then skipping the same
            if (FMoe[fldInd].toString().toLowerCase() == "accept" && depfName == fieldName && depFirstChar == "e")
                continue;

            if (depFirstChar == 's' || depFirstChar == "f") {
                //If autocomplete field is enabled then only in case of accept and sql we need to make getdepenednts call
                //if ($j("#hdnAxIsPerfCode").val() == "true" && FMoe[dfldInd].toString().toLowerCase() != "accept") {
                if ($j("#hdnAxIsPerfCode").val() == "true" && (FMoe[dfldInd].toString().toLowerCase() != "accept" && (FldAutoSelect[dfldInd] != "True" && FMoe[dfldInd].toString().toLowerCase() != "select")) && depAddCheck != true) {
                    //This is to clear the depenednt fill fields on clearing the autocomplete field

                    if (!IsGridField(depfName)) {
                        if (GetFieldValue(fieldID) == "")
                            SetFieldValue(depfName + "000F" + depFldDc, "");
                    } else {
                        if (dcNo == depFldDc) {
                            if (GetFieldValue(fieldID) == "")
                                SetFieldValue(depfName + rowNo + "F" + depFldDc, "");
                        }
                    }
                    continue;
                }

                if ($j("#hdnAxIsPerfCode").val() == "true" && FMoe[dfldInd].toString().toLowerCase() != "accept") {

                    var depFldId = "";
                    if (!IsGridField(depfName))
                        depFldId = depfName + "000F" + depFldDc;
                    else
                        depFldId = depfName + rowNo + "F" + depFldDc;
                    var depFldType = $j("#" + depFldId).attr("type");
                    if (depFldType != "checkbox" && depFldType != "radio")
                        continue;
                }

                callService = false;
                isGridDc = IsGridField(depfName);
                if (isGridDc == false) {
                    //Parents bound check is not done in rapid call since all fields do not come in formload
                    callService = true;
                } else {
                    depDcRCnt = 0;
                    //The below code loops through all the rows for the depfield and checks for the parents are bound.
                    depDcRCnt = GetDcRowCount(depFldDc);
                    for (var i = 1; i <= depDcRCnt; i++) {
                        var curRowNo = GetRowNoHelper(GetClientRowNo(i, depFldDc));
                        callService = IsRapidParentBound(depfName, curRowNo);
                        if (callService)
                            break;
                    }
                }

                if (callService) {
                    //TODO:Rapid Call rapid getdependents
                    isServiceCalled = true;
                    AxExecFormControl = true;
                    var actualFieldID = GetDependentFieldID(fieldName, fieldID, rowNo);
                    GetRapidDependents(actualFieldID, fDataFlds);
                    break;
                }
            } else if (depFirstChar == 'e') {
                EvaluateAxFunction(depfName, fieldID);
            }

            if (!AxExecFormControl) {
                DoFormControl(fieldID);
            }
            if (!isServiceCalled) {
                AxWaitCursor(false);
                ShowDimmer(false);
            }
        }
    } else if (depType == "g") {

        //Function call to check for fill grid params
        var CallService = IsFGParamChanged(fieldName);
        if (CallService) {

            isServiceCalled = true;
            AxExecFormControl = true;
            var actualFieldID = GetDependentFieldID(fieldName, fieldID, rowNo);
            if (calledFrom == "AddRow")
                return true;
            GetDependents(actualFieldID);
        }
    } else if (depType == '$') {
        EvaluateDcCaption(depfName, fieldID);
    }
}

function IsRapidParentBound(fldName, rowNo) {
    var AssignedArr = new Array();
    var returnflag = true;
    var pobj = "";
    var depDcNo = GetDcNo(fldName);
    var fldInd = GetFieldIndex(fldName);
    if (fldInd != -1) {
        var parentStr = FldRapidParents[fldInd].toString().split(",");
        var parDcNo;
        var newObjName;
        var tmpFldId = "";
        var isValBound;
        var rows;
        var isDcPopUp;
        var isBound;
        if (parentStr != "") {
            for (var i = 0; i < parentStr.length; i++) {
                parDcNo = GetDcNo(parentStr[i]);
                IsParDcgrid = IsGridField(parentStr[i]);
                newObjName = GetExactFieldName(parentStr[i]);
                isFldOldField = false;
                //Check for the field name, if the first 3 chars of the fieldname are "old"                 
                if (newObjName.substring(0, 3) == "old") {
                    isFldOldField = true;
                }

                //If the parent field is in the same dc or any other non grid dc.
                if (parDcNo == depDcNo || IsParDcgrid == false) {
                    if (IsParDcgrid == false)
                        pobj = newObjName + "000F" + parDcNo;
                    else
                        pobj = newObjName + rowNo + "F" + parDcNo;

                    //If field prefix is old and the rest of the fieldname is a valid field.
                    //Consider it to be bound.
                    if (isFldOldField) {
                        tmpFldId = "#" + pobj.substring(3);
                        if ($j(tmpFldId).length > 0) {
                            UpdateAssignedFld(pobj);
                            isFldOldField = false;
                        }
                    }

                    isValBound = CheckEmptyValue(pobj);
                    if (!isValBound) {
                        returnflag = false;
                        break;
                    }
                } else {
                    //If the parent field is in any other grid dc
                    rows = GetDcClientRows(parDcNo);
                    //If the depdc is a subgrid then check only for subgrids parent row.
                    isDcPopUp = IsDcPopGrid(depDcNo);
                    for (var j = 0; j < rows.length; j++) {
                        pobj = newObjName + rows[j] + "F" + parDcNo;

                        if (isDcPopUp && parseInt(rows[j], 10) != AxActivePRow)
                            continue;
                        //If field prefix is old and the rest of the fieldname is a valid field.
                        //Consider it to be bound.
                        if (isFldOldField) {
                            tmpFldId = "#" + pobj.substring(3);
                            if ($j(tmpFldId).length > 0) {
                                UpdateAssignedFld(pobj);
                                isFldOldField = false;
                            }
                        }
                        isBound = false;
                        isBound = CheckEmptyValue(pobj);
                        if (!isBound) {
                            returnflag = false;
                            break;
                        }
                    }
                }
                if (!returnflag)
                    break;
            }
        }
    }
    return returnflag;
}



//Function which checks for the dependents and based on their type, calls the respective function.
function CheckDependency(fieldID, calledFrom) {
    AxWaitCursor(true);
    var isServiceCalled = false;
    var fieldName = GetFieldsName(fieldID);
    var dcNo = GetFieldsDcNo(fieldID);
    var rowNo = GetFieldsRowNo(fieldID);
    var fldDbRowNo = GetDbRowNo(rowNo, dcNo);
    var fldValue = GetFieldValue(fieldID);

    var fldInd = GetFieldIndex(fieldName);
    //For numeric fld on blur, the comma has to be removed before updating in the field array.
    if (fldInd != -1) {
        var fldType = GetFieldType(fieldName, fldInd);
        if (fldType == "Numeric")
            fldValue = removeCommas(fldValue);
    }

    UpdateFieldArray(fieldID, fldDbRowNo, fldValue, "parent");

    var depArray;
    var depStr = "";
    if (fldInd != -1) {
        depStr = FldDependents[fldInd].toString();
    }
    if (depStr != "")
        depArray = depStr.split(",");
    var fDataFlds = "";
    if (rSFlds != "") {
        fDataFlds = GetFastDataFlds(depStr);
        fDataFlds = CheckFastOrDB(depArray, fDataFlds);
    }

    if (depArray != undefined) {
        for (var di = 0; di < depArray.length; di++) {

            var dField = depArray[di].toString();
            var depFirstChar = dField.substring(0, 1);
            var depfName = dField.substring(1);
            var dfldInd = GetFieldIndex(depfName);
            var depFldDc = GetDcNo(depfName);

            // this is failling on this scenario add rows in grid dc and and try to validate dependency field from other DC by Malakonda 
            //if (AxEditActiveDcNo != parseInt(GetFieldsDcNo(fieldID)) && AxEditActiveDcNo != "")
            //    return;

            //If the Accept with expression field has itself in the dep array then skipping the same
            if (FMoe[fldInd].toString().toLowerCase() == "accept" && depfName == fieldName && depFirstChar == "e")
                continue;

            if (depFirstChar == 's' || depFirstChar == "f") {

                //If autocomplete field is enabled then only in case of accept and sql we need to make getdepenednts call
                //if ($j("#hdnAxIsPerfCode").val() == "true" && FMoe[dfldInd].toString().toLowerCase() != "accept") {
                if ($j("#hdnAxIsPerfCode").val() == "true" && (FMoe[dfldInd].toString().toLowerCase() != "accept" && (FldAutoSelect[dfldInd] != "True" && FMoe[dfldInd].toString().toLowerCase() != "select")) && depAddCheck != true) {
                    //This is to clear the depenednt fill fields on clearing the autocomplete field
                    // if (depFirstChar == "f") {
                    if (!IsGridField(depfName)) {
                        if (GetFieldValue(fieldID) == "")
                            SetFieldValue(depfName + "000F" + depFldDc, "");
                    } else {
                        if (dcNo == depFldDc) {
                            if (GetFieldValue(fieldID) == "")
                                SetFieldValue(depfName + rowNo + "F" + depFldDc, "");
                        }
                    }
                    //}
                    continue;
                }

                var callService = false;
                var isGridDc = IsGridField(depfName);
                if (isGridDc == false) {
                    if ($j("#hdnAxIsPerfCode").val() == "true")
                        callService = true;
                    else
                        callService = ISBound(depfName, "000");
                } else {
                    var depDcRCnt = 0;
                    //The below code loops through all the rows for the depfield and checks for the parents are bound.
                    depDcRCnt = GetDcRowCount(depFldDc);
                    for (var i = 1; i <= depDcRCnt; i++) {
                        var curRowNo = GetRowNoHelper(GetClientRowNo(i, depFldDc));
                        callService = ISBound(depfName, curRowNo);
                        if (callService)
                            break;
                    }
                }

                if (callService) {

                    if (calledFrom == "AddRow")
                        return true;
                    isServiceCalled = true;
                    AxExecFormControl = true;
                    var actualFieldID = GetDependentFieldID(fieldName, fieldID, rowNo);
                    if (depAddCheck == true) // resetting add master data check (+ Add Button)
                        depAddCheck = false;
                    GetDependents(actualFieldID, fDataFlds);

                    break;
                } else {
                    if (calledFrom == "AddRow") {
                        AxWaitCursor(false);
                        //ShowDimmer(false);
                        return false;
                    }
                }
            } else if (depFirstChar == '$') {
                EvaluateDcCaption(depfName, fieldID);
            } else if (depFirstChar == 'e') {
                EvaluateAxFunction(depfName, fieldID);
            } else if (depFirstChar == 'r') {

                var rcount = parseInt(GetDcRowCount(dcNo), 10);
                for (var cnt = parseInt(1, 10); cnt < rcount; cnt++) {
                    var expObjName = depfName + GetRowNoHelper(cnt) + "F" + dcNo;
                    EvaluateAxFunction(depfName, expObjName);
                }
            } else if (depFirstChar == 'd' || depFirstChar == "a") {

                isServiceCalled = true;
                AxExecFormControl = true;
                if (calledFrom == "AddRow")
                    return true;
                GetDependents(fieldID, fDataFlds);
                break;
            } else if (depFirstChar == "g") {

                //Function call to check for fill grid params
                var CallService = IsFGParamChanged(fieldName);
                if (CallService) {

                    isServiceCalled = true;
                    AxExecFormControl = true;
                    var actualFieldID = GetDependentFieldID(fieldName, fieldID, rowNo);
                    if (calledFrom == "AddRow")
                        return true;
                    GetDependents(actualFieldID, fDataFlds);
                    break;
                }
            }
        }
    }
    if (!AxExecFormControl) {
        DoFormControl(fieldID);
    }
    if (!isServiceCalled) {
        if (calledFrom != "AddRow")
            ShowDimmer(false);

        AxWaitCursor(false);
    }
}

function CheckDependencyPerf(fieldID, calledFrom) {
    AxWaitCursor(true);
    var isServiceCalled = false;
    var fieldName = GetFieldsName(fieldID);
    var dcNo = GetFieldsDcNo(fieldID);
    var rowNo = GetFieldsRowNo(fieldID);
    var fldDbRowNo = GetDbRowNo(rowNo, dcNo);
    var fldValue = GetFieldValue(fieldID);

    var fldInd = GetFieldIndex(fieldName);
    //For numeric fld on blur, the comma has to be removed before updating in the field array.
    if (fldInd != -1) {
        var fldType = GetFieldType(fieldName, fldInd);
        if (fldType == "Numeric")
            fldValue = removeCommas(fldValue);
        if (fldType.toLowerCase() == "date/time" && fldValue != "") {
            var glCulture = eval(callParent('glCulture'));
            if (glCulture != undefined && glCulture == "en-us") {
                fldValue = GetDateStr(fldValue, "mm/dd/yyyy", "dd/mm/yyyy");
            }
        }
    }

    UpdateFieldArray(fieldID, fldDbRowNo, fldValue, "parent");

    var depArray;
    var depStr = "";
    if (fldInd != -1) {
        depStr = FldDependents[fldInd].toString();
    }
    if (depStr != "")
        depArray = depStr.split(",");
    var isSkipSqlFillFields = false;
    if (depArray != undefined) {
        for (var di = 0; di < depArray.length; di++) {

            var dField = depArray[di].toString();
            var depFirstChar = dField.substring(0, 1);
            var depfName = dField.substring(1);
            var dfldInd = GetFieldIndex(depfName);
            var depFldDc = GetDcNo(depfName);

            //If the Accept with expression field has itself in the dep array then skipping the same
            if (FMoe[fldInd].toString().toLowerCase() == "accept" && depfName == fieldName && depFirstChar == "e")
                continue;

            if ((depFirstChar == 's' || depFirstChar == "f") && isSkipSqlFillFields == false) {
                if (fldValue == "") //TO clear fill & select fields if field value is empty.
                {
                    ClearFldValues(fieldID, rowNo, dcNo, depFldDc, depfName);
                    continue;
                }
                var callService = false;
                if (wsPerfFields != "") {
                    if (wsPerfFields.indexOf(depfName) != -1 && calledFrom != "AddRow")
                        callService = ISBoundPerf(fieldName, rowNo);
                    else if (wsPerfFields.indexOf(depfName) == -1 && depFirstChar == 's')
                        isSkipSqlFillFields = true;
                }
                if (callService == false) {
                    if (typeof AxpFillDepFields != "undefined" && AxpFillDepFields != "") {
                        var AxpFillDepField = AxpFillDepFields.split(",");
                        if (AxpFillDepField.indexOf(fieldName) > -1)
                            callService = true;
                    }
                    if (callService == false && typeof AxpFillDepFieldsClient != "undefined" && AxpFillDepFieldsClient != "") {
                        var AxpFillDepFieldCl = AxpFillDepFieldsClient.split(",");
                        if (AxpFillDepFieldCl.indexOf(fieldName) > -1)
                            callService = true;
                    }
                }

                if (callService) {
                    if (calledFrom == "AddRow")
                        return true;
                    AcceptExpressionFlds = new Array();
                    for (var aedi = 0; aedi < depArray.length; aedi++) {
                        var aedField = depArray[aedi].toString();
                        var aedepFirstChar = aedField.substring(0, 1);
                        var aedepfName = aedField.substring(1);
                        var aedfldInd = GetFieldIndex(aedepfName);
                        if (aedfldInd > -1 && FMoe[aedfldInd].toString().toLowerCase() == "accept" && FldIsSql[aedfldInd].toString().toLowerCase() != "true" && aedepFirstChar == "e" && fieldName != aedepfName)
                            AcceptExpressionFlds.push(aedepfName);
                    }
                    isServiceCalled = true;
                    AxExecFormControl = true;
                    AxFldFormControl = fieldID;
                    var actualFieldID = GetDependentFieldID(fieldName, fieldID, rowNo);
                    if (depAddCheck == true) // resetting add master data check (+ Add Button)
                        depAddCheck = false;
                    GetDependentsPerf(actualFieldID, "", depfName);
                    break;
                } else {
                    if (depNotBoundFld != "") {
                        AdditionalRunTimeMsg(depNotBoundFld + " parent not bound for the field " + fieldName);
                        depNotBoundFld = "";
                    }
                    if (calledFrom == "AddRow") {
                        AxWaitCursor(false);
                        return false;
                    }
                }
            } else if (depFirstChar == '$') {
                EvaluateDcCaption(depfName, fieldID);
            } else if (depFirstChar == 'e') {
                //let isOverridExpression=true;
                //if(AxRulesDefComputescript=="true")
                //    isOverridExpression= AxRulesDefParser(depfName, "field", "computescript",fieldID);
                //if(isOverridExpression)
                EvaluateAxFunction(depfName, fieldID);
            } else if (depFirstChar == 'r') {

                var rcount = parseInt(GetDcRowCount(dcNo), 10);
                for (var cnt = parseInt(1, 10); cnt < rcount; cnt++) {
                    var expObjName = depfName + GetRowNoHelper(cnt) + "F" + dcNo;
                    EvaluateAxFunction(depfName, expObjName);
                }
            } else if (depFirstChar == "a") {
                if (calledFrom == "AddRow")
                    return true;
                if (wsPerfFields != "" && wsPerfFields.indexOf(depfName) != -1) {
                    isServiceCalled = true;
                    AxExecFormControl = true;
                    AxFldFormControl = fieldID;
                    GetDependentsPerf(fieldID, "");
                    break;
                }
            } else if (depFirstChar == 'd') {

                isServiceCalled = true;
                AxExecFormControl = true;
                AxFldFormControl = fieldID;
                if (calledFrom == "AddRow")
                    return true;
                GetDependentsPerf(fieldID, "");
                break;
            } else if (depFirstChar == "g") {

                //Function call to check for fill grid params
                var CallService = IsFGParamChangedPerf(fieldName, depfName, fieldID);
                if (CallService) {

                    isServiceCalled = true;
                    AxExecFormControl = true;
                    AxFldFormControl = fieldID;
                    var actualFieldID = GetDependentFieldID(fieldName, fieldID, rowNo);
                    if (calledFrom == "AddRow")
                        return true;
                    //GetDependentsPerf(actualFieldID, "");
                    GetDependentsPerf(actualFieldID, "", depfName);
                    break;
                }
            }
        }
    }
    if (!AxExecFormControl) {
        DoFormControl(fieldID);
        DoScriptFormControl(fieldID, "On Field Exit");
    }
    if (!isServiceCalled) {
        if (calledFrom != "AddRow") {
            ShowDimmer(false);

            GetProcessTime();
            GetTotalElapsTime();
        }
        AxWaitCursor(false);
    }
}

//TO clear Fill & Select fields if field value is empty.
function ClearFldValues(fieldID, rowNo, dcNo, depFldDc, depfName) {
    if (!IsGridField(depfName)) {
        if (GetFieldValue(fieldID) == "") {
            try {
                if (depfName.toLowerCase().startsWith("axpfilepath_")) {
                    axpfilepathold = GetFieldValue(depfName + "000F" + depFldDc);
                }
            } catch (ex) { }
            try {
                if ($j("#" + depfName + "000F" + depFldDc).prop("type") === "select-one") {
                    $j("#" + depfName + "000F" + depFldDc).val("");
                }
            } catch (ex) { }
            UpdateFieldArray(depfName + "000F" + depFldDc, "000", "", "parent", "AutoComplete");
            SetFieldValue(depfName + "000F" + depFldDc, "");
        }
    } else {
        if (dcNo == depFldDc) {
            if (GetFieldValue(fieldID) == "") {
                try {
                    if (depfName.toLowerCase().startsWith("axpfilepath_")) {
                        axpfilepathold = GetFieldValue(depfName + rowNo + "F" + depFldDc);
                    }
                } catch (ex) { }
                try {
                    if ($j("#" + depfName + rowNo + "F" + depFldDc).prop("type") === "select-one") {
                        $j("#" + depfName + rowNo + "F" + depFldDc).val("");
                    }
                } catch (ex) { }
                UpdateFieldArray(depfName + rowNo + "F" + depFldDc, rowNo, "", "parent", "AutoComplete");
                SetFieldValue(depfName + rowNo + "F" + depFldDc, "");
            }
        } else if (dcNo != depFldDc) {
            if (GetFieldValue(fieldID) == "") {
                var rcount = parseInt(GetDcRowCount(depFldDc), 10);
                for (var i = 1; i <= rcount; i++) {
                    var newRowNo = GetClientRowNo(i, depFldDc);
                    try {
                        if ($j("#" + depfName + newRowNo + "F" + depFldDc).prop("type") === "select-one") {
                            $j("#" + depfName + newRowNo + "F" + depFldDc).val("");
                        }
                    } catch (ex) { }
                    UpdateFieldArray(depfName + newRowNo + "F" + depFldDc, newRowNo, "", "parent", "AutoComplete");
                    SetFieldValue(depfName + newRowNo + "F" + depFldDc, "");
                }
            }
        }
    }
}

//Function to construct the field id of the dependent field.
function GetDependentFieldID(depField, parentFieldID, parentRowNo) {

    var depFieldID = "";
    var IsGrid = IsGridField(depField);
    var frameNo = GetDcNo(depField);
    if (IsGrid == false) {
        depFieldID = depField + "000F" + frameNo;
    } else {
        depFieldID = depField + parentRowNo + "F" + frameNo;
    }
    return depFieldID;
}

//Function to return comma seperated subgrids dc no for a given parent dc.
function GetSubGridsForParentDc(parDcNo) {
    var strSubGrids = "";
    for (var i = 0; i < PopParentDCs.length; i++) {
        if (PopParentDCs[i] == parDcNo) {
            if (strSubGrids == "")
                strSubGrids = PopGridDCs[i].toString();
            else
                strSubGrids += "," + PopGridDCs[i].toString();
        }
    }

    return strSubGrids;
}

//Function to return the subgrids and their rows for the given parent row.
//Format of string should be - as subgridDcNo~1,2,3¿subgridDcNo~3,4,5
function GetSubGridInfoForParent(parDcNo, parentRowNo) {
    var strSubGridDetails = "";

    var strSubGrids = GetSubGridsForParentDc(parDcNo);
    var subGridDcs = strSubGrids.split(",");
    for (var i = 0; i < subGridDcs.length; i++) {
        var strGridRows = GetPopRows(parDcNo, parentRowNo, subGridDcs[i]);
        strGridRows = GetDbPopRows(strGridRows, subGridDcs[i]);

        if (strGridRows != "") {
            if (strSubGridDetails == "")
                strSubGridDetails = subGridDcs[i] + "~" + strGridRows;
            else
                strSubGridDetails = "¿" + subGridDcs[i] + "~" + strGridRows;
        }
    }

    return strSubGridDetails;
}

function GetRapidDependents(fieldID, fastDataFlds) {
    AxStartTime = new Date();
    AxStartTime = GetAxDate(AxStartTime);
    var stTime = new Date();
    ShowDimmer(true);
    IsService = true;
    var inputXml = "";
    var IsServiceCalled = false;
    var rid = $j("#recordid000F0").val();
    ArrActionLog = "GetDependents-FieldId-" + fieldID + "-Recordid-" + rid;
    var trace = traceSplitStr + "GetRapidDep-" + transid + traceSplitChar;
    var fldName = GetFieldsName(fieldID);
    var fldRowNo = GetFieldsRowNo(fieldID);
    var fldDcNo = GetFieldsDcNo(fieldID);
    var activeRow = AxActiveRowNo;
    if (activeRow == "") activeRow = 0;
    var visDcname = "";
    var oldValue = CheckSpecialCharsInStr(AxOldValue);
    AxActDepFld = fieldID;
    AxActDepFldVal = GetFieldValue(fieldID);
    visDcname = GetOpenTabDcs();

    //if the picklist is open while calling the get dep, then it should be hidden
    if ($j("#dvPickList").is(':visible')) $j("#dvPickList").hide();

    var subStr = "";
    //For every parent row making a getdep call, the respective sub grid rows should be sent.
    if (IsParentDc(fldDcNo)) {
        //for each subgrid, get the sub grid rows for the given parent row and send this info
        //The format is popdc1~row1,row2,row3¿popdc2~row1,row2
        subStr = GetSubGridInfoForParent(fldDcNo, fldRowNo);
        AxSubGridRows = subStr;
    }

    var parStr = "";
    if (AxActivePRow != "" && AxActivePDc != "") {
        parStr = AxActivePDc + "~" + AxActivePRow;
        AxParStrFromDep = parStr;
    }

    inputXml = '<sqlresultset activerow="' + activeRow + '" prow="' + AxActivePRow + '" pdc="' + AxActivePDc + '" axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" field="' + fldName + '" oldvalue="' + oldValue + '" pageno="1" pagesize="10" frameno = "' + fldDcNo + '" rowno ="' + fldRowNo + '" dcname="' + visDcname + '"  sessionid="' + sid + '" trace="' + trace + '" >';

    try {
        var isPerfCode = $j("#hdnAxIsPerfCode").val();
        if (fastDataFlds != "" && isPerfCode == "true") {
            //ASB.WebService.GetFastDepFlds(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, visDcname, inputXml, transid, tstDataId, fldDcNo, GetRowNoHelper(activeRow), fldName, subStr, parStr, fastDataFlds, SuccChoicesFastDatatDep, OnException);

            AxWaitCursor(false);
            ShowDimmer(false);
            console.log("This is fast data field and dependency's are there in fast data" + fieldID);

        } else {
            ASB.WebService.GetRapidDepFlds(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, visDcname, inputXml, transid, tstDataId, fldDcNo, GetRowNoHelper(activeRow), fldName, subStr, parStr, resTstHtmlLS, SuccChoicesgetDep, OnException);
        }
    } catch (exp) {
        isDepForceCallOnExp = "false";
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
    var edTime = new Date();
    AxTimeBefSerCall = edTime - stTime;
}

//Function which calls the GetDependents webservice for the given field.
function GetDependents(fieldID, fastDataFlds) {
    AxStartTime = new Date();
    AxStartTime = GetAxDate(AxStartTime);
    var stTime = new Date();
    ShowDimmer(true);
    IsService = true;
    var inputXml = "";
    var IsServiceCalled = false;
    var rid = $j("#recordid000F0").val();
    ArrActionLog = "GetDependents-FieldId-" + fieldID + "-Recordid-" + rid;
    var trace = traceSplitStr + "GetDependents-" + transid + traceSplitChar;
    var fldName = GetFieldsName(fieldID);
    var fldRowNo = GetFieldsRowNo(fieldID);
    var fldDcNo = GetFieldsDcNo(fieldID);

    var activeRow = AxActiveRowNo;
    if (activeRow == "") activeRow = 0;
    AxActDepFld = fieldID;
    var visDcname = "";
    var oldValue = CheckSpecialCharsInStr(AxOldValue);
    AxActDepFldVal = GetFieldValue(fieldID);
    visDcname = GetOpenTabDcs();

    //if the picklist is open while calling the get dep, then it should be hidden
    if ($j("#dvPickList").is(':visible')) $j("#dvPickList").hide();

    var subStr = "";
    //For every parent row making a getdep call, the respective sub grid rows should be sent.
    if (IsParentDc(fldDcNo)) {
        //for each subgrid, get the sub grid rows for the given parent row and send this info
        //The format is popdc1~row1,row2,row3¿popdc2~row1,row2
        subStr = GetSubGridInfoForParent(fldDcNo, fldRowNo);
        AxSubGridRows = subStr;
    }

    var parStr = "";
    if (AxActivePRow != "" && AxActivePDc != "") {
        parStr = AxActivePDc + "~" + AxActivePRow;
        AxParStrFromDep = parStr;
    }

    inputXml = '<sqlresultset activerow="' + activeRow + '" prow="' + AxActivePRow + '" pdc="' + AxActivePDc + '" axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" field="' + fldName + '" oldvalue="' + oldValue + '" pageno="1" pagesize="10" frameno = "' + fldDcNo + '" rowno ="' + fldRowNo + '" dcname="' + visDcname + '"  sessionid="' + sid + '" trace="' + trace + '" >';

    try {
        var isPerfCode = $j("#hdnAxIsPerfCode").val();
        if (fastDataFlds != "" && isPerfCode == "true") {
            //ASB.WebService.GetFastDepFlds(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, visDcname, inputXml, transid, tstDataId, fldDcNo, GetRowNoHelper(activeRow), fldName, subStr, parStr, fastDataFlds, SuccChoicesFastDatatDep, OnException);

            AxWaitCursor(false);
            ShowDimmer(false);
            console.log("This is fast data field and dependency's are there in fast data" + fieldID);

        } else {
            callBackFunDtls = "GetDependents♠" + fieldID + "♠" + fastDataFlds;
            ASB.WebService.GetDepFlds(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, visDcname, inputXml, transid, tstDataId, fldDcNo, GetRowNoHelper(activeRow), fldName, subStr, parStr, resTstHtmlLS, SuccChoicesgetDep, OnException);
        }
    } catch (exp) {
        isDepForceCallOnExp = "false";
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
    var edTime = new Date();
    AxTimeBefSerCall = edTime - stTime;
}

function GetDependentsPerf(fieldID, fastDataFlds = "", dfldName = "") {
    //Temporary code
    StartTime = new Date().getTime();
    //End Temporary code
    AxStartTime = new Date();
    AxStartTime = GetAxDate(AxStartTime);
    var stTime = new Date();
    ShowDimmer(true);
    IsService = true;
    var inputXml = "";
    var IsServiceCalled = false;
    var rid = $j("#recordid000F0").val();
    ArrActionLog = "GetDependents-FieldId-" + fieldID + "-Recordid-" + rid;
    var trace = traceSplitStr + "GetDependents-" + transid + traceSplitChar;
    var fldName = GetFieldsName(fieldID);
    var fldRowNo = GetFieldsRowNo(fieldID);
    var fldDcNo = GetFieldsDcNo(fieldID);

    var activeRow = AxActiveRowNo;
    if (activeRow == "") activeRow = 0;
    AxActDepFld = fieldID;
    var visDcname = "";
    var oldValue = CheckSpecialCharsInStr(AxOldValue);
    AxActDepFldVal = GetFieldValue(fieldID);
    visDcname = GetOpenTabDcs();

    //if the picklist is open while calling the get dep, then it should be hidden
    if ($j("#dvPickList").is(':visible')) $j("#dvPickList").hide();

    var subStr = "";
    //For every parent row making a getdep call, the respective sub grid rows should be sent.
    if (IsParentDc(fldDcNo)) {
        //for each subgrid, get the sub grid rows for the given parent row and send this info
        //The format is popdc1~row1,row2,row3¿popdc2~row1,row2
        subStr = GetSubGridInfoForParent(fldDcNo, fldRowNo);
        AxSubGridRows = subStr;
    }

    var parStr = "";
    if (AxActivePRow != "" && AxActivePDc != "") {
        parStr = AxActivePDc + "~" + AxActivePRow;
        AxParStrFromDep = parStr;
    }

    if (IsDcGrid(fldDcNo) && isGrdEditDirty && !axInlineGridEdit) //Refer Bug:AGI004107
        UpdateFieldArray(axpIsRowValid + fldDcNo + fldRowNo + "F" + fldDcNo, GetDbRowNo(fldRowNo, fldDcNo), "", "parent", "AddRow");

    inputXml = '<sqlresultset activerow="' + activeRow + '" prow="' + AxActivePRow + '" pdc="' + AxActivePDc + '" axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" field="' + fldName + '" oldvalue="' + oldValue + '" pageno="1" pagesize="10" frameno = "' + fldDcNo + '" rowno ="' + fldRowNo + '" dcname="' + visDcname + '"  sessionid="' + sid + '" trace="' + trace + '" >';

    if (typeof dfldName != "undefined" && dfldName != "") {
        if ($j("[id^=" + dfldName + "]").hasClass("fldmultiSelect"))
            fldmultiSelectdep = true;
    }

    try {
        callBackFunDtls = "GetDependents♠" + fieldID;
        GetProcessTime();
        ASB.WebService.GetDepFldsPerf(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, RegVarFldList, ArrActionLog, visDcname, inputXml, transid, tstDataId, fldDcNo, GetRowNoHelper(activeRow), dfldName, fldName, subStr, parStr, AcceptExpressionFlds, resTstHtmlLS, SuccChoicesgetDep, OnException);

    } catch (exp) {
        isDepForceCallOnExp = "false";
        fldmultiSelectdep = false;
        AxFldFormControl = "";
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        UpdateExceptionMessageInET("GetDepFldsPerfWS exception : " + exp.message);

        GetProcessTime();
        GetTotalElapsTime();
    }
    var edTime = new Date();
    AxTimeBefSerCall = edTime - stTime;
}

function OnException(result) {
    isExcelImpDelWS = "false";
    AxFldFormControl = "";
    fldmultiSelectdep = false;
    if (result._message.toLowerCase().indexOf("access violation") === -1) {
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
            "pointer-events": "auto"
        });
        AxWaitCursor(false);
        ShowDimmer(false);
        if (FromSave) {
            FromSave = false;
            EnableSaveBtn(true);
        }
        if ($j("#icons").length > 0 && $j("#icons").prop("disabled"))
            $j("#icons").removeProp("disabled");

        window.clearInterval(draftTimer);
        showAlertDialog("error", result._message);
        UpdateExceptionMessageInET("OnException : " + result._message);
    } else {
        try {
            UpdateExceptionMessageInET("OnException : " + result._message);
        } catch (ex) {}
        AxWaitCursor(false);
        ShowDimmer(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
}

function SuccChoicesFastDatatDep(result, eventArgs) {
    if (CheckSessionTimeout(result))
        return;
    if ((IsFunction != "addrow") && (IsFunction != "delrow")) {
        ChangedFields = new Array();
        ChangedFieldDbRowNo = new Array();
        ChangedFieldValues = new Array();
        DeletedDCRows = new Array();
    }
    var strResult = result.split("*$*");
    if (strResult.length > 1 && result != "*$*") {
        var strFlds = strResult[0].split(",");
        for (var i = 0; i < strFlds.length; i++) {
            if (!$j("#" + strFlds[i]).attr("list") && $j(strResult[1])[i].options.length == 1)
                SetFieldValue(strFlds[i], $j(strResult[1])[i].options[0].value);
            else
                SetFieldValue(strFlds[i], "");
            $j("#axlist-" + strFlds[i]).remove();
        }
        $j("#wBdr").append(strResult[1]);
        //alert(strResult[1]);
    }
    AxWaitCursor(false);
    ShowDimmer(false);
}

//Callback function from the GetDependents webservice.
function SuccChoicesgetDep(result, eventArgs) {
    if (result != "") {
        if (result.split("♠*♠").length > 1) {
            tstDataId = result.split("♠*♠")[0];
            result = result.split("♠*♠")[1];
        }
        if (result.split("*♠*").length > 1) {
            var serverprocesstime = result.split("*♠*")[0];
            var requestProcess_logtime = result.split("*♠*")[1];
            result = result.split("*♠*")[2];
            WireElapsTime(serverprocesstime, requestProcess_logtime, true);
        } else {
            UpdateExceptionMessageInET("Error : " + result);
        }
    }
    if (result.toLowerCase().indexOf("access violation") === -1) {
        if (isDepForceCallOnExp == "true" && result.indexOf('{"msg":"No Dependant field to refresh..."}') > -1) {
            result = "";
            isDepForceCallOnExp = "false";
        }
        var stTime = new Date();
        ArrActionLog = "";
        if (CheckSessionTimeout(result))
            return;
        resTstHtmlLS = "";
        if ((IsFunction != "addrow") && (IsFunction != "delrow")) {
            ChangedFields = new Array();
            ChangedFieldDbRowNo = new Array();
            ChangedFieldValues = new Array();
            DeletedDCRows = new Array();
            AcceptExpressionFlds= new Array();
        }
        fldmultiSelectdep = false;

        AxActiveDc = GetFieldsDcNo(AxActDepFld);

        ParseServiceResult(result, "GetDep");
        //for Approved/Rejected transaction, no need to call the Actions
        if (appstatus != "Approved" && appstatus != "Rejected" && (!AxExecFormControl)) {
            DoFormControlOnload();

            var rid = $j("#recordid000F0").val();
            if (rid != "" && rid != "0")
                DoScriptFormControl("", "On Data Load");
            else
                DoScriptFormControl("", "On Form Load");
        } else if (AxExecFormControl && AxFldFormControl != "") {
            DoFormControl(AxFldFormControl);
            DoScriptFormControl(AxFldFormControl, "On Field Exit");
            AxFldFormControl = "";
        }
        //Refer Bug: BIZ000245, Stopped Evaluation after getdependency.
        //var fldName = GetFieldsName(AxActDepFld);
        //var isGrid = IsGridField(fldName);
        //if (isGrid) {
        //    EvaluateExpressions(AxActDepFld);
        //}


        if (axInlineGridEdit) {
            var fieldName = GetFieldsName(AxActDepFld);
            if (IsGridField(fieldName)) {
                //var rowIndex = $("#" + AxActDepFld).closest("tr").index();
                //var columnIndex = $("#" + AxActDepFld).parent().index();
                //inlineShiftKeyPressed = false;
                //focusOnDeleteButton = false;
                //inlineGridEdit(AxActiveDc, $("#" + AxActDepFld).closest("td"), columnIndex, rowIndex);
                var dataindex = $("#" + AxActDepFld).closest("td").attr("data-focus-index");
                var lastFocusIndex = $("#" + AxActDepFld).closest("tr").attr("last-focus-index");
                if (lastFocusIndex != dataindex)
                    dataindex = parseInt(dataindex) + 1;
                setTimeout(function () {
                    $("#" + AxActDepFld).parents('input:eq(0),body').find("td[data-focus-index='" + dataindex + "']").find("input").focus();
                }, 30);
            }
        }

        EvaluateSetFontOnGetDep(AxActDepFld);

        try {
            AxAfterGetDependents();
        } catch (ex) {

        }
        AxParStrFromDep = "";
        AxDepRows = new Array();

        //Commented the below code as in a scenario where a field is changed in 
        //second row and the first row field is focussed the getdep was called multiple times.
        //if (IsPickListField(AxActDepFld) != true)
        //   $j("#" + AxActDepFld).focusNextInputField();

        if (AxLogTimeTaken == "true") {
            var edTime = new Date();
            var diff = edTime.getTime() - stTime.getTime();
            CreateTimeLog(AxStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "GetDependents");
        }
        AxWaitCursor(false);
        ShowDimmer(false);

        try {
            if (callBackFunDtls == "") {
                if (isPKItemSelected == true || $j("#" + AxActDepFld).hasClass('fldFromSelect') || $j("#" + AxActDepFld).hasClass('fldmultiSelect') || (blurNextPreventId != "" && $("#" + blurNextPreventId).is(".editLayoutFooter button") || (!$.isEmptyObject(blurNextPreventElement) && $(blurNextPreventElement).is(".editLayoutFooter button")))) {
                    isPKItemSelected = false;
                    CallNextActionPList();
                }
            } else if ((!$.isEmptyObject(blurNextPreventElement) && (($(blurNextPreventElement).is("button, a") && !$(blurNextPreventElement).is("a.ui-state-default")) || $(blurNextPreventElement).attr("type") == "button" || $(blurNextPreventElement).attr("type") == "submit")) || (blurNextPreventId != "" && $("#" + blurNextPreventId).is("button, a"))) {
                //for handling button/hyperlink click events after getdependency
                CallNextActionPList();
                callBackFunDtls = "";
            } else if (!$.isEmptyObject(blurNextPreventElement) && $(blurNextPreventElement).hasClass('material-icons') && $(blurNextPreventElement).parent().is("button, a")) {
                CallNextActionPList();
                callBackFunDtls = "";
            }
            else if (!$.isEmptyObject(blurNextPreventElement) && $(blurNextPreventElement).hasClass('material-icons') && $(blurNextPreventElement).parents('.input-group').length > 0 && $(blurNextPreventElement).parents('.input-group').find('.flatpickr-input').length > 0) {
                CallNextActionPList();
                callBackFunDtls = "";
            }
            else {
                callBackFunDtls = "";
            }
        } catch (ex) {}
    } else {
        AxWaitCursor(false);
        ShowDimmer(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
    //Temporary code
    var edTime = new Date().getTime();
    var diff = edTime - StartTime;
    console.log("GetDependency: " + diff + " ms");
    //End Temporary code
    GetProcessTime();
    GetTotalElapsTime();
}


function AxAfterGetDependents() {
    // if (IsPickListField(AxActDepFld) == true) {
    try {
        AxAfterGetDependent();
    } catch (ex) {}
    var $focused = document.activeElement;
    $focused.select();
    //  }
}




function CallNextActionPList() {
    if (blurNextPreventId != "") {
        //  if (blurNextPreventId != "" && !$j("#" + blurNextPreventId).is("input,img,select") && blurNextPreventId.indexOf("addrow") == -1) {
        if (blurNextPreventId.indexOf("addrow") == -1 && blurNextPreventId.indexOf("advancebtn") == -1 && blurNextPreventId.indexOf("gridAddBtn") == -1 && blurNextPreventId.indexOf("newRecordBtn") == -1) {
            //$j("#" + blurNextPreventId).click();
            $j("[id=" + blurNextPreventId + "]:visible").click();
        }
    } else {
        blurNextPreventElement.click();
    }
    blurNextPreventId = "";
    blurNextPreventElement = new Object();
}

function EvaluateExpressions(fieldID) {
    var fieldName = GetFieldsName(fieldID);
    var dcNo = GetFieldsDcNo(fieldID);
    var rowNo = GetFieldsRowNo(fieldID);
    var fldDbRowNo = GetDbRowNo(rowNo, dcNo);
    var fldValue = GetFieldValue(fieldID);

    var fldInd = GetFieldIndex(fieldName);
    //For numeric fld on blur, the comma has to be removed before updating in the field array.
    if (fldInd != -1) {
        var fldType = GetFieldType(fieldName, fldInd);
        if (fldType == "Numeric")
            fldValue = removeCommas(fldValue);
    }

    var depArray;
    var depStr = "";
    if (fldInd != -1) {
        depStr = FldDependents[fldInd].toString();
    }
    if (depStr != "")
        depArray = depStr.split(",");


    if (depArray != undefined) {
        for (var di = 0; di < depArray.length; di++) {

            var dField = depArray[di].toString();
            var depFirstChar = dField.substring(0, 1);
            var depfName = dField.substring(1);
            var depFldDc = GetDcNo(depfName);
            if (depFirstChar == 'e') {
                if (!IsDcGrid(depFldDc)) {
                    EvaluateAxFunction(depfName, fieldID);
                } else {
                    var depIndx = GetFldNamesIndx(depfName);
                    var depExpr = Expressions[depIndx].toString();
                    if (depExpr.indexOf("sumtill") != -1) {
                        EvaluateAxFunction(depfName, fieldID);
                    }
                }
            }
        }
    }

    //If the get dependents is called on a grid field, should be evaluate the non grid expression dependent field which returned by the service.
    if (NonGridExpDepFlds.length > 0) {
        for (var di = 0; di < NonGridExpDepFlds.length; di++) {
            var exField = NonGridExpDepFlds[di].toString();
            var exfldInd = GetFieldIndex(exField);
            var exdepStr = FldDependents[exfldInd].toString();
            var exdepArray;
            if (exdepStr != "")
                exdepArray = exdepStr.split(",");
            if (exdepArray != undefined) {
                for (var edi = 0; edi < exdepArray.length; edi++) {
                    var eddField = exdepArray[edi].toString();
                    var depFirstChar = eddField.substring(0, 1);
                    var depfName = eddField.substring(1);
                    var depFldDc = GetDcNo(depfName);
                    if (depFirstChar == 'e') {
                        if (!IsDcGrid(depFldDc)) {
                            EvaluateAxFunction(depfName, exField);
                        } else {
                            var depIndx = GetFldNamesIndx(depfName);
                            var depExpr = Expressions[depIndx].toString();
                            if (depExpr.indexOf("sumtill") != -1) {
                                EvaluateAxFunction(depfName, exField);
                            }
                        }
                    }
                }
            }
        }
        NonGridExpDepFlds = new Array();
    }
}


//Function which displays the values of the grid cells above the grid.
function CheckGrdisplayHead(tabNo, fieldName, frRowno, status) {

    var disField = "displayhead" + tabNo + "000F1";
    var dispCaption = new Array();
    var dispFldName = new Array();
    var dfld = $j("#" + disField);
    if (dfld && dfld.val() && dfld.val() != "") {
        let _thisVal = GetFieldValue(disField)
        var disHeadings = _thisVal.split("~");//dfld.val().split("~");
        for (var di = 0; di < disHeadings.length; di++) {
            var dcapFld = disHeadings[di].split(":");
            dispCaption.push(dcapFld[0]);
            dispFldName.push(dcapFld[1]);
        }
    }

    if (dispCaption.length == 0)
        return;

    var dispVal = new Array();
    for (var dic = 0; dic < dispFldName.length; dic++) {

        var curCol = dispFldName[dic].toString() + frRowno + "F" + tabNo;
        var curColFld = $j("#" + curCol);
        if (curColFld) {
            if (status == "Clear") dispVal.push("");
            else {
                var tmp = GetFieldValue(curCol);
                if (tmp == undefined) tmp = "";
                dispVal.push(tmp);
            }
        }
    }

    // build html
    if (dispCaption.length == dispVal.length) {
        var ntxt = "<table><tr>";

        for (var nht = 0; nht < dispCaption.length; nht++)
            ntxt += "<td class=labelcap>" + dispCaption[nht].toString() + " : </td><td class=grdispcol>" + dispVal[nht].toString() + "&nbsp;&nbsp;&nbsp;&nbsp;</td>";

        ntxt += "</tr></table>";

        var disHeaddiv = "disgridhead" + tabNo;
        if ($j("#" + disHeaddiv))
            $j("#" + disHeaddiv).css({
                "position": "relative"
            }).html(ntxt);

        var sLeft = $j("#divDc" + tabNo + "").scrollLeft();
        if (sLeft > 0) {
            var lVal = $j("#" + disHeaddiv).css("left");
            $j("#" + disHeaddiv).css("left", screen.width / sLeft);
        } else {
            $j("#" + disHeaddiv).css("left", 0);
        }
    }
}



//Function to open the page on click of the hyperlink label. 
function Tstructhyperlink(obj) {

    var hl = "";
    var hltype = "";
    var na = "";
    var isPop = "";
    var params = "";

    for (var m = 0; m < HLinkSource.length; m++) {

        if (obj.id == HLinkSource[m].toString()) {

            hl = HLinkName[m].toString();
            hltype = "&hltype=" + HLinkLoad[m].toString();
            isPop = HLinkPop[m].toString();
            params = GetHyperLinkParamStr(m);
            TsructPopup(hl, hltype, isPop, params);
            break;
        }
    }
}

function TstructLabelhyperlink(obj) {
    if (typeof theMode != "undefined" && theMode == "design")
        return;
    var params = "";
    let hlParamData = JSON.parse($(obj).attr("data-param"));
    if (hlParamData.paramNode != "" && hlParamData.paramNode != "~") {
        let prNode = hlParamData.paramNode.split("^");
        $.each(prNode, function (indx, pval) {
            let pvals = pval.split("~");
            let pName = pvals[0];
            let pValue = "";
            if (pvals[1] != "" && pvals[1].substring(0, 1).toString() == ":") {
                pValue = pvals[1];
                var parDcNo = GetDcNo(pValue.toString().substring(1));
                var fldIndex = $j.inArray(pValue.toString().substring(1), FNames);
                var fldType = GetFieldType(pValue.toString().substring(1), fldIndex);
                var parafield = pValue.toString().substring(1) + "000F" + parDcNo;
                pValue = GetFieldValue(parafield);
                if (fldType == "Numeric") {
                    if ((pValue.length > 3) && (pValue.indexOf(',') != -1))
                        pValue = removeCommas(pValue);
                }
            } else
                pValue = pvals[1];
            pValue = CheckUrlSplChars(pValue);
            pValue = pValue.replace(/&/g, "--.--");
            params += "&" + pName + "=" + pValue;
        });
        if(params=="" && hlParamData.paramNode!="")
            params = "&"+hlParamData.paramNode;
    }
    if (hlParamData.type == "tstruct") {
        var loadpopup = 'tstruct.aspx?act=' + hlParamData.loadType + '&transid=' + hlParamData.structName + params + `&isDupTab=${callParentNew('isDuplicateTab')}`;
        if (hlParamData.ivtstpopup == "true")
            createPopup(loadpopup);
        else
            window.document.location.href = loadpopup;
    } else if (hlParamData.type == "iview") {
        var popname = "ivewPop" + hlParamData.structName;
        if (hlParamData.ivtstpopup == "true") {
            params = params + "&AxIsPop=true";
            var loadpopup = 'ivtoivload.aspx?ivname=' + hlParamData.structName + params + `&isDupTab=${callParentNew('isDuplicateTab')}`,
                popname;
            createPopup(loadpopup);
        } else {
            params = params + "&AxIsPop=false";
            let srcUrl = 'ivtoivload.aspx?ivname=' + hlParamData.structName + params + `&isDupTab=${callParentNew('isDuplicateTab')}`;
            window.document.location.href = srcUrl;
        }
    } else {
        let srcUrl = "htmlPages.aspx?load=" + hlParamData.structName;
        window.document.location.href = srcUrl;
    }
}

//Function to get the parameter string to open the tstruct page.
function GetHyperLinkParamStr(indx) {

    var tparams = "";
    if (HLinkParamName[indx]) {

        var params = HLinkParamName[indx];
        var paramval = HLinkParamValue[indx];
        if (params == "-" && paramval == "-")
            return "";

        if (params != "")
            params = params.split("~");

        if (paramval != "")
            paramval = paramval.split("~");

        for (var k = 0; k < params.length; k++) {

            var paraName = params[k];
            var paraVal = paramval[k];
            if (paraVal != undefined) {
                //TODO: Below replacing :: with : since this is a bug in axpert.
                paraVal = paraVal.replace("::", ":");
                if (paraVal.substring(0, 1).toString() == ":") {
                    var parDcNo = GetDcNo(paraVal.toString().substring(1));
                    var fldIndex = $j.inArray(paraVal.toString().substring(1), FNames);
                    var fldType = GetFieldType(paraVal.toString().substring(1), fldIndex);
                    var parafield = paraVal.toString().substring(1) + "000F" + parDcNo;
                    paraVal = GetFieldValue(parafield);
                    if (fldType == "Numeric") {
                        if ((paraVal.length > 3) && (paraVal.indexOf(',') != -1))
                            paraVal = removeCommas(paraVal);
                    }

                }
                var fres = paraName + "=" + CheckUrlSplChars(paraVal);
                fres = fres.replace(/&/g, "--.--");
                tparams = tparams + "&" + fres;
            }
        }
    }
    return tparams;
}

function CheckUrlSplChars(value) {
    value = value.replace(/&amp;/g, "&");
    value = value.replace(/%/g, "%25");
    value = value.replace(/&/g, "%26");
    value = value.replace(/'/g, "%27");
    value = value.replace(/"/g, "%22");
    value = value.replace(/#/g, "%23");
    //value = encodeURIComponent(value);
    return value;
}

//Function to open the relevant page onclick of the label link. 
function TsructPopup(hl, hltype, isPop, params) {

    var torecid = "&torecid=false";
    var poppath = "";
    if (hl.toString().substring(0, 1) == "p") {
        poppath = "Actionpage.aspx?name=" + hl.toString().substring(1) + params + hltype;
    } else if (hl.toString().substring(0, 1) == "t") {
        poppath = "tstruct.aspx?transid=" + hl.toString().substring(1) + params + hltype + torecid + `&isDupTab=${callParentNew('isDuplicateTab')}`;
    } else if (hl.toString().substring(0, 1) == "i") {
        poppath = "ivtoivload.aspx?ivname=" + hl.toString().substring(1) + params + hltype + `&isDupTab=${callParentNew('isDuplicateTab')}`;
    } else if (hl.toString().substring(0, 1) == ":") {
        var parDcNo = GetDcNo(hl.toString().substring(1));
        var parafield = hl.toString().substring(1) + "000F" + parDcNo;
        var srcfieldvalue = GetFieldValue(parafield);
        if (srcfieldvalue != "") {
            TsructPopup(srcfieldvalue, hltype, isPop, params);
            return;
        }
    }

    if (isPop.toString().toLowerCase() != "false") {
        var loadPop;
        try {
            loadPop = window.open(poppath, "LoadPop", "width=800,height=500,resizable=1,scrollbars=yes");
        } catch (ex) {
            showAlertDialog("warning", eval(callParent('lcm[356]')));
        }
    } else
        window.location.href = poppath;
}

//Function to refresh all the fields in the given dc.
function RefreshDc(dcNo, parentDcs) {
    ShowDimmer(true);
    var recId = $j("#recordid000F0").val();
    try {
        callBackFunDtls = "RefreshDc♠" + dcNo + "♠" + parentDcs;
        ASB.WebService.RefreshDc(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, dcNo, tstDataId, recId, parentDcs, resTstHtmlLS, SuccessRefreshDc, OnException);
    } catch (exp) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
}

function SuccessRefreshDc(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (result.toLowerCase().indexOf("access violation") === -1) {
        if (CheckSessionTimeout(result))
            return;
        resTstHtmlLS = "";
        ParseServiceResult(result, "RefreshDc");
        AxWaitCursor(false);
        ShowDimmer(false);
    } else {
        AxWaitCursor(false);
        ShowDimmer(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
}


///Function to get the currently selected tab HTML and data from the server
function GetTabData(tabNo) {

    if ($("#myTab li#li" + tabNo).hasClass('active'))
        return false;

    $("#myTab li").removeClass("bg-white");
    $("#myTab li").find(".form-check").addClass("d-none");
    $("#myTab li#li" + tabNo).addClass("bg-white"); 
    $("#myTab li").addClass("bg-light");
    $("#myTab li#li" + tabNo).removeClass("bg-light");
    $("#myTab li#li" + tabNo).find(".form-check").removeClass("d-none");
    $("#myTab li span[id^=tabCollapseButton]").addClass("d-none");
    $("#myTab li span#tabCollapseButton" + tabNo).removeClass("d-none");

    //$("#tabCollapseButton" + tabNo).attr("title", "Hide Dc").text('expand_less');   
    $("[id^='tabsCont']").show();
    AxTabStartTime = new Date();
    AxTabStartTime = GetAxDate(AxTabStartTime);
    var stTime = new Date();
    ShowDimmer(true);
    AxWaitCursor(true);
    var IsFillGrid = false;

    trace = traceSplitStr + "GetTabData-" + tabNo.toString() + traceSplitChar;
    var rid = 0;

    if ($j("#recordid000F0"))
        rid = $j("#recordid000F0").val();
    ArrActionLog = "LoadTab-" + tabNo + "-Recordid-" + rid;
    var noFill = false;

    if (noFill) {
        updtxt = '<sqlresultset axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" dcname="dc' + tabNo + '"  sessionid="' + sid + '" trace="' + trace + '" nofill="true" >';
    } else {
        updtxt = '<sqlresultset axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" dcname="dc' + tabNo + '"  sessionid="' + sid + '" trace="' + trace + '" >';
    }
    CurrTabNo = tabNo;

    //update the VisibleDCs array with the current tab

    var j = 0;
    for (j = 0; j < PagePositions.length; j++) {

        if (PagePositions[j].toString().indexOf(",") != -1) {

            var strTabs = PagePositions[j].toString().split(',');
            var cnt = 0;
            for (cnt = 0; cnt < strTabs.length; cnt++) {
                if (CurrTabNo == strTabs[cnt]) {
                    VisibleDCs[j] = CurrTabNo.toString();
                }
            }
        }
    }

    var i = 0;
    var status = 0;
    var indx = 0;
    for (i = 0; i < TabDCs.length; i++) {

        if (TabDCs[i] == tabNo) {
            status = TabDCStatus[i];
            indx = i;
            break;
        }
    }

    var dvId = "tab-" + tabNo;
    var dvTab = $j("#" + dvId);
    if (dvTab.prop("disabled") == true) {
        IsTabDisabled = "true";
    }
    var isTabHtml = false;
    TabDcActive = false;
    if (status == 0 && $j("#tab-" + tabNo).html() != "") {
        isTabHtml = true;
        status = 1;
        TabDcActive = true;
        TabDCStatus[indx] = "1";
        //AssignGrdFreezeHdrScript(tabNo);
    }

    if (arrRefreshDcs.length > 0) {
        for (var i = 0; i < arrRefreshDcs.length; i++) {
            var arrDcNos = arrRefreshDcs[i].split(':');
            if (arrDcNos[0] == "dc" + CurrTabNo && arrRefreshFldDirty[i] == true) {
                RefreshDc(arrDcNos[0].replace("dc", ""), arrDcNos[1].replace("dc", "") + ',' + arrDcNos[0].replace("dc", ""));
                arrRefreshFldDirty[i] = false;
                break;
            }
        }
    }
    try {
        FldListParents = new Array();
        FldListData = new Array();
    } catch (ex) { }

    if (status == 0) {
        TabDcActive = true;
        TabDCStatus[indx] = "1";
        isFill = 1;

        try {
            callBackFunDtls = "GetTabData♠" + tabNo;
            ASB.WebService.CallLoadDcData(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, updtxt, tabNo, IsFillGrid, IsTabDisabled, tstDataId, resTstHtmlLS, SuccessGetTabData, OnException);
        } catch (exp) {
            AxWaitCursor(false);
            ShowDimmer(false);
            showAlertDialog("error", ServerErrMsg);
        }
    } else {
        gridFreezeCols(CurrTabNo);
        isFill = 0;
        try {
            //Hook to fire a event on every time tab is clicked.
            //AxLoadTab();
            AxAfterLoadTab(CurrTabNo);
            //makeFieldInitCap(tabNo)
        } catch (ex) {}
        SetDynamicDcCaptions(CurrTabNo);
        if (isTstPop == "True") {
            TstructTabEventsInPopUP(CurrTabNo);
        }

        if (!$("#divDc" + CurrTabNo).is(":visible") && typeof TabDCAlignmentStatus[CurrTabNo - 1] == "undefined") {
            $(document).off('shown.bs.tab').on('shown.bs.tab', 'div[role="tabpanel"] .nav-tabs a[data-toggle="tab"]', function (e) {
                if (typeof TabDCAlignmentStatus[CurrTabNo - 1] == "undefined") {
                    AlignDcElements("divDc" + CurrTabNo);
                }
            });
        }        

        if (($("#divDc" + CurrTabNo).find("div[class*=cke_editor_]").length > 0 || $("#divDc" + CurrTabNo).find(".memofam.gridstackCalc").length>0) && isTabHtml) {
            try {
                let _thisEls = $("#divDc" + CurrTabNo).find("div[class*=cke_editor_]").length == 0 ? $("#divDc" + CurrTabNo).find(".memofam.gridstackCalc") : $("#divDc" + CurrTabNo).find("div[class*=cke_editor_]");
                _thisEls.each(function () {
                    var _thisID = typeof $(this).siblings("textarea").attr("id") != "undefined" ? $(this).siblings("textarea").attr("id") : $(this).attr("id");
                    if (typeof CKEDITOR.instances[_thisID] != "undefined") {
                        try {
                            var _this = CKEDITOR.instances[_thisID];
                            _this.destroy();
                        } catch (error) { }
                    }
                });
                allRtfTextAreasTab(CurrTabNo);
                CKEDITOR.on('instanceReady', function (event) {
                    event.editor.on('change', function () {
                        currentCK = this.name;
                    });
                });
                clearHiddenGridstack('divDc' + CurrTabNo, true);
            } catch (ex) { }
        }

        AxWaitCursor(false);
        ShowDimmer(false);
        //adjustwin("10", window);
        focusOnFirstInputOnTabClick(CurrTabNo, true)
    }
    var edTime = new Date();
    AxTimeBefSerCall = edTime - stTime;
}

//if dc layout type is Header-Tabbed then focus on the first input element when user clicks on a tab
function focusOnFirstInputOnTabClick(tabNo, reffreshEditor) {
    setTimeout(function () {
        var elemntsToCheck = 'button[tabindex!="-1"],a[tabindex!="-1"],input[tabindex!="-1"],textarea[tabindex!="-1"],select[tabindex!="-1"],table tbody tr[tabindex!="-1"]';
        var inputs = $('#divDc' + tabNo).find(elemntsToCheck).filter(':visible').not(':disabled');
        $(inputs).first().focus();
        if (reffreshEditor === true)
            refreshEditor(tabNo);

        let isExitDummy = false;
        if (gridDummyRowVal.length > 0) {
            gridDummyRowVal.map(function (v) {
                if (v.split("~")[0] == tabNo && v.split("~")[1] == "001") isExitDummy = true;
            });
        }
        if(isExitDummy){
            forceRowedit = true;
            gridRowEditOnLoad = true;
            $("#gridHd" + tabNo + " tr#sp" + tabNo + "R001F" + tabNo + " td:eq(2)").click();
        }
        SetPositionfldDisplayTot();  
        if (typeof theModeDesign != "undefined" && theModeDesign == "true") {
            const _obj = $(".tstformbutton");
            _obj.prop("disabled", true);
            _obj.addClass('disabled');
            _obj.addClass('btndis');
            _obj.css("pointer-events", 'none');
            _obj.css("cursor", 'default');
            _obj.attr('tabindex', -1);
        }
        try {
            if (typeof TabDcActive != "undefined" && TabDcActive && !isExitDummy && recordid != "0") {
                DropzoneInit("#divDc" + tabNo);
                DropzoneGridInit("#divDc" + tabNo);
            }
        } catch (ex) { }
    }, 200)
}

//Callback function from the CallLoadDcData service.
function SuccessGetTabData(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (result.toLowerCase().indexOf("access violation") === -1) {
        var stTime = new Date();
        ArrActionLog = "";
        if (CheckSessionTimeout(result))
            return;
        resTstHtmlLS = "";
        //the result format -> AxMasterRowFlds*♠*jsonsResult*♠*dcno♣rowCnt*?*dchtml
        //First split the json and html, call assignloadvalues for json 
        //second parse the html
        //if the dc contains subgrids then -> dcno♣rowCnt*?*dchtml(*Tab*subGridNo♣rowCnt*?*subGridHtml)this is repeated for every subgrid

        var strResult = result.split("*♠*"); {
            var strComDep = strResult[0].split("*♦*");

            if (strComDep[0] != "") {
                $j("#DivFrame1").append("<script type='text/javascript'>" + strComDep[0] + "</script>");
            }
            if (strComDep[1] != "Ax-NA")
                SetMRForFromList(strComDep[1]);
        }

        result = result.replace(strResult[0] + "*♠*", "");
        ParseServiceResult(result, "GetTabData");

        UpdateFldArrayInFillgrid(CurrTabNo);


        UpdatePopGridInfo();
        CheckFormControlPriv(CurrTabNo);
        CheckDisabledDcs(CurrTabNo);
        CheckShowHideFlds();
        CheckEnableDiableFlds();
        try {
            $('.griddivColumn ').addClass('gridFixedHeader').css({ "overflow": "auto" });
            if (typeof gridFixedHeader == "undefined" || gridFixedHeader == "true") {
                (isMobile && AxpGridForm == "form") ? "" : $('.griddivColumn').addClass('gridFixedHeader').css({ "max-height": "calc(100vh - 130px)" });
                $(".gridFixedHeader table thead tr th").css({ "background": "#fff", "position": "sticky", "top": "0" });
            }
        } catch (ex) { }
        if (appstatus == "Approved" || appstatus == "Rejected") {
            ShowingDc(CurrTabNo, "Disable");
        }

        if (gridDummyRowVal.length > 0) {
            gridDummyRowVal.map(function (v) {
                if (v.split("~")[0] == CurrTabNo)
                    gridDummyRowVal.splice($.inArray(v, gridDummyRowVal), 1);
            });
        }

        GridDcAddEmptyRows();

        FocusOnFirstField(CurrTabNo);
        DeletedDCRows = new Array();
        if (isTstPop == "True") {
            TstructTabEventsInPopUP(CurrTabNo);
        }
        // for adding RichTextBox in tabDC's
        allRtfTextAreasTab(CurrTabNo);

        //enable/disable/show/hide grid DC's in tab
        SetGridBtnAccess();
        //check for column merge value
        var dcColumnValue = $j('#axp_colmerge_' + CurrTabNo + '000F1');
        dcColumnValue = dcColumnValue.val();
        if (dcColumnValue != null && dcColumnValue != '' && dcColumnValue != undefined) {
            GetGridDcTable(dcColumnValue, CurrTabNo);
        }
        try {
            AxAfterLoadTab(CurrTabNo);
        } catch (ex) {}
        if (AxLogTimeTaken == "true") {
            var edTime = new Date();
            var diff = edTime.getTime() - stTime.getTime();
            CreateTimeLog(AxTabStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "LoadDcCombos");
        }

        if (arrRefreshDcs.length > 0) {
            for (var i = 0; i < arrRefreshDcs.length; i++) {
                var arrDcNos = arrRefreshDcs[i].split(':');
                if (arrDcNos[0] == "dc" + CurrTabNo && arrRefreshFldDirty[i] == false) {
                    RefreshDc(arrDcNos[0].replace("dc", ""), arrDcNos[1].replace("dc", ""));
                    arrRefreshFldDirty[i] = false;
                    break;
                }
            }
        }
        if (theMode == "design") {
            //temp BindFunctionsOnLoad();
            //$("#wrapperForEditFields" + CurrTabNo).removeClass("hide");
        }

        setDesignedLayout("#tab-" + CurrTabNo);
        //applyDesignJsonAgain(CurrTabNo);

        var dcIdx = $j.inArray(CurrTabNo.toString(), DCFrameNo);
        //Code for changeEditLayoutIds from listview was removed because it's not applying changeEditLayoutIds if record is coming from tab data based on dependency
        //if ($j("#recordid000F0").val().toString() != "0" && IsDcGrid(CurrTabNo) && DCHasDataRows[dcIdx] == "True") {
        //Refer bugs-s-AGI003587,AGI003578,AGI003573,AGI003589,AGI003588 - If no rows come from server then no need to change wrapper ids
        var curDcRCnt = $("#gridHd" + CurrTabNo + " tbody tr").length;
        if (IsDcGrid(CurrTabNo) && DCHasDataRows[dcIdx] == "True" && curDcRCnt > 0) {
            changeEditLayoutIds('', CurrTabNo, 'tabListView');
        }
        makeFieldInitCap(CurrTabNo);
        SetDynamicDcCaptions(CurrTabNo);
        $(".gridFocusable").removeClass("gridFocusable").off("keydown.tabRot");
        $("#myTab .active").addClass("gridFocusable");
        $(".gridFocusable").on('keydown.tabRot', function (e) {
            if ((e.which === 9 && !e.shiftKey)) {
                e.preventDefault();
                $('#gridAddBtn' + CurrTabNo).focus();
                $(".gridFocusable").removeClass("gridFocusable").off("keydown.tabRot");
            }
        });

        if (appstatus != "Approved" && appstatus != "Rejected" && (!AxExecFormControl) && theMode != "design") {
            var rid = $j("#recordid000F0").val();
            if (rid != "0")
                DoScriptFormControl("", "On Data Load");
            else
                DoScriptFormControl("", "On Form Load");
        }

        CheckShowHideFldsGrid(CurrTabNo.toString());
        focusOnFirstInputOnTabClick(CurrTabNo)
        customAlignTstructFlds([], CurrTabNo);
        ShowDimmer(false);
    } else {
        AxWaitCursor(false);
        ShowDimmer(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
}


function AssignTabDcHtmlPerf(result) {
    if (CheckSessionTimeout(result))
        return;
    var tabDchtmlArray = result.split("*♠♠*");
    for (var i = 0; i < tabDchtmlArray.length; i++) {
        var strResult = tabDchtmlArray[i].split("*♠*"); {
            var strComDep = strResult[0].split("*♦*");

            if (strComDep[0] != "") {
                $j("#DivFrame1").append("<script type='text/javascript'>" + strComDep[0] + "</script>");
            }
            if (strComDep[1] != "Ax-NA")
                SetMRForFromList(strComDep[1]);
        }
        var tabDcNo = strResult[2].split("♣")[0];
        result = tabDchtmlArray[i];
        result = result.replace(strResult[0] + "*♠*", "");
        ParseServiceResult(result, "GetTabData");
        UpdatePopGridInfo();
        CheckFormControlPriv(tabDcNo);
        CheckDisabledDcs(tabDcNo);
        CheckShowHideFlds();
        CheckEnableDiableFlds();
        if (appstatus == "Approved" || appstatus == "Rejected") {
            ShowingDc(tabDcNo, "Disable");
        }
        FocusOnFirstField(tabDcNo);
        DeletedDCRows = new Array();
        if (isTstPop == "True") {
            TstructTabEventsInPopUP(tabDcNo);
        }
        allRtfTextAreasTab(tabDcNo);
        SetGridBtnAccess();
        var dcColumnValue = $j('#axp_colmerge_' + tabDcNo + '000F1');
        dcColumnValue = dcColumnValue.val();
        if (dcColumnValue != null && dcColumnValue != '' && dcColumnValue != undefined) {
            GetGridDcTable(dcColumnValue, tabDcNo);
        }
        try {
            AxAfterLoadTab(tabDcNo);
        } catch (ex) {}
        if (AxLogTimeTaken == "true") {
            var edTime = new Date();
            var diff = edTime.getTime() - stTime.getTime();
            CreateTimeLog(AxTabStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "LoadDcCombos");
        }

        if (arrRefreshDcs.length > 0) {
            for (var i = 0; i < arrRefreshDcs.length; i++) {
                var arrDcNos = arrRefreshDcs[i].split(':');
                if (arrDcNos[0] == "dc" + tabDcNo && arrRefreshFldDirty[i] == false) {
                    RefreshDc(arrDcNos[0].replace("dc", ""), arrDcNos[1].replace("dc", ""));
                    arrRefreshFldDirty[i] = false;
                    break;
                }
            }
        }
        //setDesignedLayout("#tab-" + tabDcNo);
        var dcIdx = $j.inArray(tabDcNo.toString(), DCFrameNo);
        var curDcRCnt = $("#gridHd" + tabDcNo + " tbody tr").length;
        if (IsDcGrid(tabDcNo) && DCHasDataRows[dcIdx] == "True" && curDcRCnt > 0) {
            changeEditLayoutIds('', tabDcNo, 'tabListView');
        }
        makeFieldInitCap(tabDcNo);
        SetDynamicDcCaptions(tabDcNo);
    }
}

function applyDesignJsonAgain(designDcNo) {
    var wrapperForEditFieldsIsHidden = false;
    var wraperDivId;
    try {
        wraperDivId = $j("#divDc" + designDcNo + " [id^=wrapperForEditFields]")[0].id;
        if ($j("#" + wraperDivId).hasClass("hide")) {
            wrapperForEditFieldsIsHidden = true;
            $j("#" + wraperDivId).removeClass("hide");
        }
    } catch (ex) {}
    AlignDcElements("divDc" + designDcNo);
    try {
        if (wrapperForEditFieldsIsHidden == true) {
            $j("#" + wraperDivId).addClass("hide");
            wrapperForEditFieldsIsHidden = false;
        }
    } catch (ex) {}
}

//Function to check if any field was disabled on data load, and disable the field on open of tab.
function CheckEnableDiableFlds() {
    for (var i = 0; i < AxFormContDisableFlds.length; i++) {
        if (AxFormContDisableFlds[i] != "") {
            var strFld = AxFormContDisableFlds[i].split("~");
            EnableDisableField(strFld[0], strFld[1], "1");
        }
    }
}

//Function to check if any tab was disabled on data load, and disable the tab on open of tab.
function CheckDisabledDcs(CurrTabNo) {
    for (var i = 0; i < DisabledDcs.length; i++)
        if (DisabledDcs[i] != "") {
            var strFld = DisabledDcs[i].split("~");
            if (CurrTabNo == strFld[0]) {

                ShowingDc(strFld[0], strFld[1]);
            }
        }
}

//Function to check if the fields are in the AxFormContHiddenFlds then apply the formcontrol action.
function CheckShowHideFlds() {
    for (var i = 0; i < AxFormContHiddenFlds.length; i++) {
        if (AxFormContHiddenFlds[i] != "") {
            var strFld = AxFormContHiddenFlds[i].split("~");
            HideShowField(strFld[0], strFld[1]);
        }
    }
    for (var j = 0; j < AxFormContSetCapFlds.length; j++) {
        if (AxFormContSetCapFlds[j] != "") {
            var strFld = AxFormContSetCapFlds[j].split("~");
            SetCaptionFormControl(strFld[0], strFld[1]);
        }
    }
}

function CheckShowHideFldsGrid(gfDcNo, _thisRowNo = "") {
    for (var j = 0; j < AxFormContSetCapFldsGrid.length; j++) {
        if (AxFormContSetCapFldsGrid[j] != "") {
            var strFld = AxFormContSetCapFldsGrid[j].split("~");
            if (IsDcGrid(gfDcNo)) {
                if (strFld[0] == gfDcNo)
                    SetCaptionFormControlOnGrid(strFld[1], strFld[2], gfDcNo);
            } else {
                if (strFld[0] == gfDcNo)
                    SetCaptionFormControl(strFld[1] + "000F" + strFld[0], strFld[2]);
            }
        }
    }

    SetFormContFldActGrid(gfDcNo);

    SetFormContFldGridCell(gfDcNo, _thisRowNo);
}

function SetFormContFldGridCell(gfDcNo, _thisRowNo) {
    if (_thisRowNo != "")
        _thisRowNo = _thisRowNo.substring(_thisRowNo.lastIndexOf("F"), _thisRowNo.lastIndexOf("F") - 3);
    else
        return;
    for (var j = 0; j < AxFormContSetGridCell.length; j++) {
        if (AxFormContSetGridCell[j] != "") {
            var strFld = AxFormContSetGridCell[j].split("~");
            var dcNo = strFld[0];
            let _tRowNo = strFld[2];

            if (gfDcNo != "" && dcNo == gfDcNo && _tRowNo == _thisRowNo) {
                let fieldId = strFld[1] + _tRowNo + "F" + dcNo;
                if (strFld[3] == "disable")
                    EnableDisableField(fieldId, "2", "1");
                else if (strFld[3] == "enable")
                    EnableDisableField(fieldId, "1", "1");
                else if (strFld[3] == "setfont") {
                    document.getElementById(fieldId).setAttribute("style", strFld[4]);
                }
            } else if (gfDcNo != "" && dcNo == gfDcNo && _tRowNo == "*") {
                let fieldId = strFld[1] + _thisRowNo + "F" + dcNo;
                if (strFld[3] == "setfont") {
                    document.getElementById(fieldId).setAttribute("style", strFld[4]);
                }
            }
        }
    }
}

function SetFormContFldActGrid(gfDcNo) {
    for (var j = 0; j < AxFormContSetFldActGrid.length; j++) {
        if (AxFormContSetFldActGrid[j] != "") {
            var strFld = AxFormContSetFldActGrid[j].split("~");
            var dcNo = strFld[0];

            if (gfDcNo == "" || dcNo == gfDcNo) {
                if (strFld[2] == "hide")
                    HideShowField(strFld[1], "hide");
                else
                    HideShowField(strFld[1], "show");

                var fieldId = "";
                if (IsDcGrid(dcNo)) {
                    var rowCnt = 0;
                    rowCnt = GetDcRowCount(dcNo);
                    for (var i = 1; i <= rowCnt; i++) {
                        fieldId = strFld[1] + GetClientRowNo(i, dcNo) + "F" + dcNo;
                        if (strFld[2] == "disable")
                            EnableDisableField(fieldId, "2", "1");
                        else
                            EnableDisableField(fieldId, "1", "1");
                    }
                } else {
                    fieldId = strFld[1] + "000F" + dcNo;
                    if (strFld[2] == "disable")
                        EnableDisableField(fieldId, "2", "1");
                    else
                        EnableDisableField(fieldId, "1", "1");
                }
            }
        }
    }
}

function SetFocusFormControl() {
    for (var j = 0; j < AxFormContFldSetFocus.length; j++) {
        if (AxFormContFldSetFocus[j] != "") {
            var strFld = AxFormContFldSetFocus[j].split("~");
            var dcNo = strFld[0];
            if (strFld[2] == "setfocus") {
                if (!IsDcGrid(dcNo)) {
                    var fieldId = strFld[1] + "000F" + dcNo;
                    if (isMobile && $("#" + fieldId).hasClass("fldFromSelect"))
                        continue;
                    else {
                        //if (callParentNew("originaltrIds").length > 0 && callParentNew("originaltrIds").filter(x => x == transid).length > 0) {
                        //} else
                        //    $j("#" + fieldId).focus();
                        try {
                            let appSessUrl = top.window.location.href.toLowerCase().substring(0, top.window.location.href.indexOf("/aspx/"));
                            let storedKey = 'originaltrIds-' + appSessUrl;
                            let transidArray = JSON.parse(localStorage.getItem(storedKey) || '[]');
                            if (transidArray.length > 0 && transidArray.includes(transid)) {
                                // Do nothing
                            } else {
                                $j("#" + fieldId).focus();
                            }
                        } catch (ex) {
                            $j("#" + fieldId).focus();
                        }
                    }
                }
                return;
            }
        }
    }
}

function SetCaptionFormControlOnGrid(fldName, setCap, gfDcNo) {
    if (IsDcGrid(gfDcNo)) {
        $("#gridHd" + gfDcNo + " th[id='th-" + fldName + "']").text(setCap);
        $("#wrapperForEditFields" + gfDcNo + " label[for='" + fldName + "']").text(setCap);
        $("#wrapperForEditFields" + gfDcNo).find("#dvGrid" + fldName + " label").text(setCap);
    }
}

function SetCaptionFormControl(fldName, setCap) {
    var setFldDc = GetFieldsDcNo(fldName);
    if (!IsDcGrid(setFldDc)) {
        //if (rFrameNo == fldName)
        //    fldName = fldName + "000F" + setFldDc;
        if ($j("#" + fldName).attr("type") == "checkbox")
            $("label[for='" + fldName + "'] span").text(setCap);
        else
            $("label[for='" + fldName + "']").text(setCap);
    } else {
        var fName = GetFieldsName(fldName);
        $("#gridHd" + setFldDc + " th[id='th-" + fName + "']").text(setCap);
        $("#wrapperForEditFields" + setFldDc + " label[for='" + fName + "001F" + setFldDc + "']").text(setCap);
    }
}

//Function to check for formcontrol privilege for tab dcs, as tab json will not be sent to the client.
function CheckFormControlPriv(CurrTabNo) {
    var fields = GetGridFields(CurrTabNo);
    var rowCount = GetDcRowCount(CurrTabNo);
    var isFound = false;
    for (var i = fields.length - 1; i >= 0; i--) {
        if (fields[i].substring(0, 8) == "axpvalid") {
            for (var j = 1; j <= rowCount; j++) {
                var fldId = fields[i] + GetRowNoHelper(j) + "F" + CurrTabNo;
                var fldVal = $j("#" + fldId).val();
                DoFormControlPrivilege(fldId, fldVal);
                isFound = true;
            }
        }
        if (isFound)
            break;
    }
}

//NOTE:Any modification in the below function should be checked with the LoadEvents function in main-tstruct.js
function AssignJQueryEvents(dcArray,_calledFrom="") {
    CKEDITOR.on('instanceReady', function (event) {
        event.editor.on('change', function () {
            currentCK = this.name;
        });
    });

    // createEditors();

    $('[data-toggle="popover"]').popover({
        container: 'body'
    });
    for (var i = 0; i < dcArray.length; i++) {

        var dvId = dcArray[i];
        if ($j(dvId).length > 0) {

            //setDesignedLayout(dvId);

            // createAutoComplete(dvId + " .fldAutocomplete");
            // $j(dvId).find(".fldAutocomplete").autocomplete();

            $j(dvId).find(".fldFromSelect").select2();
            createFormSelect(dvId + " .fldFromSelect");

            setTimeout(function(){
                $j(dvId).find(".fldmultiSelect").select2();
                createFormMultiSelect(dvId + " .fldmultiSelect");
            },0);            

            try {
                if (_calledFrom != "FillGrid") {
                    DropzoneInit(dvId);
                    DropzoneGridInit(dvId);
                }
            } catch (ex) { }

            KTApp.initBootstrapPopovers();

            $j(dvId).find("input,select,textarea:not(#txtCommentWF)").unbind("focus");
            $j(dvId).find("input:not([class=AxAddRows],[class=AxSearchField],.gridRowChk,.gridHdrChk,.flatpickr-input),select:not(#selectbox),textarea:not(#txtCommentWF):not(.labelInp)").focus(function () {
                MainFocus($j(this));
            });
            let _tbDcNo = "";
            if (typeof dvId != "undefined" && dvId != "")
                _tbDcNo = dvId.replace('#tab-', '');
            if (IsTabDc(_tbDcNo) && !IsDcGrid(_tbDcNo)) {
                //Tabbed non grid dc not required register event 
            } else {
                $j(dvId + " .date").removeClass('hasDatepicker');
                $j(dvId + " .tstOnlyTime").removeClass('hasDatepicker');
                $j(dvId + " .tstOnlyTime24hours").removeClass('hasDatepicker');
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
                $j(dvId + " .flatpickr-input:not(.tstOnlyTime,.tstOnlyTime24hours,.fldtimestamp)").each(function () {
                    let fp = flatpickr(this, {
                        dateFormat: dtFormat,
                        allowInput: true,
                        disableMobile: true,
                        onOpen: function (selectedDates, dateStr, instance) {
                            if (dateStr != null && dateStr != "")
                                instance.setDate(dateStr);
                            MainFocus($(instance.element));
                        },
                        onClose: function (selectedDates, dateStr, instance) {
                            MainBlur($(instance.element));
                        }
                    });

                    $j(this)
                        .closest(".input-group")
                        .find(".input-group-text")
                        .on("click", function () {
                            fp.open();
                        });
                });

                $j(dvId + " .flatpickr-input.fldtimestamp").each(function () {
                    let fp = flatpickr(this, {
                        dateFormat: tsFormat,
                        enableTime: true,
                        time_24hr: true,
                        enableSeconds: true,
                        allowInput: false,
                        disableMobile: true,
                        defaultHour: new Date().getHours(),
                        defaultMinute: new Date().getMinutes(),
                        defaultSeconds: new Date().getSeconds(),
                        onPreCalendarPosition: function (selectedDates, dateStr, instance) {
                            let $input = $(instance.element);
                            let inputVal = $input.val();
                            if (inputVal && inputVal.trim() !== "") {
                                instance.setDate(inputVal, false, tsFormat);
                            } else {
                                instance.clear();
                            }
                        },
                        onOpen: function (selectedDates, dateStr, instance) {
                            if (dateStr != null && dateStr != "")
                                instance.setDate(dateStr);
                            MainFocus($(instance.element));
                        },
                        onClose: function (selectedDates, dateStr, instance) {
                            MainBlur($(instance.element));
                        }
                    });

                    // Attach icon click handler
                    $j(this)
                        .closest(".input-group")
                        .find(".spanDateTime")
                        .on("click", function () {
                            fp.open();
                        });
                });
                $j(dvId + " .tstOnlyTime").each(function () {
                    let fp = flatpickr(this, {
                        enableTime: true,
                        //allowInput: true,
                        noCalendar: true,
                        dateFormat: "h:i K",
                        disableMobile: true,
                        onPreCalendarPosition: function (selectedDates, dateStr, instance) {
                            let _thisfpVal = $(instance.element).val();
                            if (_thisfpVal != "")
                                instance.setDate(_thisfpVal);
                        },
                        onOpen: function (selectedDates, dateStr, instance) {
                            MainFocus($(instance.element));
                        },
                        onClose: function (selectedDates, dateStr, instance) {
                            MainBlur($(instance.element));
                        }
                    });

                    $j(this)
                        .closest(".input-group")
                        .find(".input-group-text")
                        .on("click", function () {
                            fp.open();
                        });
                });
                $j(dvId + " .tstOnlyTime24hours").each(function () {
                    let fp = flatpickr(this, {
                        enableTime: true,
                        noCalendar: true,
                        //allowInput: true,
                        dateFormat: "H:i",
                        time_24hr: true,
                        disableMobile: true,
                        onPreCalendarPosition: function (selectedDates, dateStr, instance) {
                            let _thisfpVal = $(instance.element).val();
                            if (_thisfpVal != "")
                                instance.setDate(_thisfpVal);
                        },
                        onOpen: function (selectedDates, dateStr, instance) {
                            MainFocus($(instance.element));
                        },
                        onClose: function (selectedDates, dateStr, instance) {
                            MainBlur($(instance.element));
                        }
                    });

                    $j(this)
                        .closest(".input-group")
                        .find(".input-group-text")
                        .on("click", function () {
                            fp.open();
                        });
                });
            }

            // TimePickerEvent(dvId, dtpkrRTL);

            $j(dvId).find("textarea:not(#txtCommentWF),:text,:password").unbind("blur");

            //function call on blur event of textarea, textbox.
            $j(dvId).find("textarea:not(#txtCommentWF):not(.labelInp,.select2-search__field),[id]:text:not([class=AxAddRows],[class=AxSearchField],.gridRowChk,.gridHdrChk,.dvgrdchkboxnonedit,.tstOnlyTime,.tstOnlyTime24hours,.flatpickr-input),:password").blur(function () {
                if (AxFldBlurFromSelect != "♠♠") {
                    MainBlur($j(this));
                    if (axInlineGridEdit) {
                        var fldObj = $j(this);
                        var fldId = fldObj.attr("id");
                        var fName = GetFieldsName(fldId);
                        var fldIndex = $j.inArray(fName, FNames);
                        var allowEmpty = GetFieldProp(fldIndex, "allowEmpty");
                        var fldValue = GetFieldValue(fldId);// fldObj.val();
                        if (allowEmpty === "F") {
                            var fldType = FDataType[fldIndex];
                            if ((fldType == "Numeric" && parseFloat(fldValue) == 0 || fldValue === "")) {
                                fldObj.css("border", "1px solid red");
                                fldObj.closest("td").attr("data-fld-mandatory", '')
                            } else {
                                fldObj.css("border", "1px solid rgb(204, 204, 204)");
                                fldObj.closest("td").removeAttr("data-fld-mandatory");
                            }
                        }
                    }
                } else {
                    AxFldBlurFromSelect = "";
                }
            });

            $j(dvId).find("select:not(#selectbox),:checkbox,:radio").unbind("change");
            //function call on change event of dropdown.
            $j(dvId + " select:not(.fldFromSelect,.multiFldChklist,.multiFldChk,.fldmultiSelect)").change(function () {
                if ($j(this).find(':selected').text().indexOf("+ Add") == -1) {
                    MainBlur($j(this));
                    $j(this).blur();
                    $j(this).focus();
                }
            });

            //function call on blur event of checkbox, checklist & radiogroup.
            $j(dvId).find(":checkbox:not([class=chkAllList],.gridRowChk,.gridHdrChk,.dvgrdchkboxnonedit):not(.tokenSelectAll),:radio").not(".chkShwSel").change(function () {
                MainBlur($j(this));
            });

            $j(dvId + " textarea").unbind("keydown");
            //function call on keydown event in a textarea.
            $j(dvId + " textarea:not(.labelInp)").keydown(function (event) {
                LimitText($j(this));
            });

            $j(dvId + " textarea").unbind("keyup");
            //function call on keyup event in a textarea.
            $j(dvId + " textarea:not(.labelInp)").keyup(function () {
                LimitText($j(this));
            });

            $j(dvId + " .number").unbind("keypress");
            //function call on keypress event in a numeric field.
            $j(dvId + " .number").keypress(function (event) {
                return CheckNumeric(event, $j(this).val());
            });

            // function on keypress of picklist input '#tblPickData tr td'
            $j(dvId + " .inputClass2").keydown(function (event) {
                if (event.keyCode == 13 && pickStatus == true) {
                    DisplayPickList($j(this));
                }
                pickStatus = true;
            });

            //function on click of image field
            $j(dvId + " .axpImg").click(function () {
                var imgFld = $j(this);
                var fldName = imgFld[0].id;
                UploadImg(fldName);
            });

            $j(dvId + " .signaturePad").click(function (e) {
                if ($(e.target).attr("onclick") == "ClearImageSrc(this);") {
                    return;
                } else {
                    var imgFld = $j(this).find(".signatureInput");
                    var fldName = imgFld[0].id;
                    openSignaturePad(fldName);
                }
            });

            $j(dvId + " .divBarQrScan").click(function () {
                var scanFld = $j(this);
                var fldName = scanFld.parent().find("input")[0].id;
                openBarQrScanner(fldName);
            });

            $j(dvId + " .randomnum").each(function () {
                ValidateFieldTypeExpression($(this));
            });

            //$j(dvId + " .axpBtn").click(function () {
            //    var obj = $j(this);
            //    CallAction(obj[0].id);
            //});

            //function on click of action buttons
            $(dvId + " .axpBtn").off("click.axpBtn").on("click.axpBtn", function () {
                var obj = $j(this);
                CallAction(obj[0].id);
            });

            $j(dvId + " .rowdelete").click(function () {
                DeleteCurrentRow($j(this));
            });

            $j(dvId + " .subGrid").click(function () {

                ShowPopUp($j(this));
            });


            $j(".rowdelete img").hover(function () {
                $j(this).attr("src", "../axpimages/icons/16x16/delete.png");
            }, function () {
                $j(this).attr("src", "../axpimages/icons/16x16/delete-fade.png");
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

            //function call on focusin event for masked field value to actual value.
            $j(dvId + " input.form-control,textarea.tem.form-control").on('focusin', function (e) {
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

            $j(dvId + " input.number").on('input', function () {
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
        }

        // $j(dvId).find('.fileuploadmore').off('click.fileuploadmorea');
        // $j(dvId).find('.fileuploadmore').on("click.fileuploadmorea", function () {
        //     if ($(this).parents(".dropzone").find(".dropzone-items").hasClass("d-none"))
        //         $(this).parents(".dropzone").find(".dropzone-items").removeClass("d-none");
        //     else
        //         $(this).parents(".dropzone").find(".dropzone-items").addClass("d-none");
        // });

        $j(dvId).find(".fldCustTableIcon").off('click.fldCustTableIcona');
        $j(dvId).find(".fldCustTableIcon").on('click.fldCustTableIcona', function (event) {
            FieldTypeTable(event, this);
        });

        $j(dvId).find(".fldCustTable ").off('keydown.fldtblA');
        $j(dvId).find(".fldCustTable ").on("keydown.fldtblA", function (event) {
            if (event.keyCode == 13) {
                FieldTypeTable(event, this);
            }
        });

        swicthCompressMode(dvId);
    }
    // dcArray = new Array();
    if (parent.MainNewEdit == true) {
        $j('html').addClass("makeFullHeight");
    }
    try {
        //if (parent.parent.isTstructPopup == undefined || !parent.parent.isTstructPopup) {
        if ($j("#middle1", parent.document).attr("src") == undefined || $j("#middle1", parent.document).attr("src").indexOf("ParamsTstruct.aspx") === -1) {
            $j(document).off("keydown").on("keydown", function (e) {
                if (e.which == 13 && $j('#dvPickList').is(':visible') && $j('#tblPickData tr.active').length > 0) {
                    if (initialSrchVal == $j("#" + $j("#tblPickData").attr("data-pick")).val()) {
                        e.stopPropagation();
                        var paramString = $j("#tblPickData tr.active td").attr("onclick");
                        paramString = paramString.substring(23, paramString.length - 2);
                        SetPickVal(paramString);
                        pickStatus = false;
                        selectedRow = 0;
                        e.which = -1;
                        //clear the value
                        initialSrchVal = "";
                    } else {
                        DisplayPickList($j("#" + $j("#tblPickData").attr("data-pick")));
                    }
                }
                if (e.which == 40 && $j('#dvPickList').is(':visible') && $j('#tblPickData tr.active').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    pickDownn();
                    if ($("#dvPickHead .active").offset().top - $("#dvPickHead").offset().top + $("#dvPickHead .active").outerHeight() >= $("#dvPickHead").height() || $("#dvPickHead .active").offset().top - $("#dvPickHead").offset().top < 0) {
                        $("#dvPickHead").animate({
                            scrollTop: ($("#dvPickHead .active").offset().top - $("#dvPickHead").offset().top) + $("#dvPickHead .active").height()
                        }, 100);
                    }
                    return false;
                }
                if (e.which == 38 && $j('#dvPickList').is(':visible') && $j('#tblPickData tr.active').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    pickUpp();
                    //   if ($("#dvPickHead .active").offset().top - $("#dvPickHead").offset().top < $("#dvPickHead").height()) {
                    $("#dvPickHead").animate({
                        scrollTop: ($("#dvPickHead .active").offset().top - $("#dvPickHead").offset().top) - $("#dvPickHead .active").height()
                    }, 100);
                    //    }
                    return false;
                }
            });
        }
        //This event is already available in main-tstruct.js in LoadEvents() so removing from here. 

        //if ($j("#popupIframeRemodal", parent.document).attr("src") != undefined) {
        //    $j(document).keyup(function (e) {
        //        if (e.keyCode === 27) {
        //            $j('.remodal-close', parent.document).click();
        //        }
        //    });
        //}
        typeof commonReadyTasks == 'function' ? commonReadyTasks() : "";
        //}
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

}

function ConvertFieldID(obj, newRowNo) {
    if (obj) {

        if (obj.prop("tagName") == "I") {
            var attVal = "data-clk";
            id = obj.attr(attVal);
            if (id == undefined) {
                attVal = "data-fldname";
                id = obj.attr(attVal);
            }
            if (id == undefined) {
                attVal = "data-refresh";
                id = obj.attr(attVal);
            }
            if (id == undefined) {
                attVal = "data-ids";
                id = obj.attr(attVal);
            }
            if (id != undefined) {
                var fName = GetFieldsName(id);
                var dcNo = GetFieldsDcNo(id);
                var newFldName = fName + newRowNo + "F" + dcNo;
                obj.attr(attVal, newFldName);
            }
        }

        var id = obj.attr("id");
        if (id != undefined) {
            var fName = GetFieldsName(id);
            var dcNo = GetFieldsDcNo(id);
            var newFldName = fName + newRowNo + "F" + dcNo;
            obj.attr("id", newFldName);
            //Changing the id of grid reference's delete function arguments(DeletefromFile)
            if (newFldName.indexOf("grdRefLink_hlnk__") > -1) {
                if (obj.attr("onclick") != undefined) {
                    obj = UpdateGridReferFunc(obj, newFldName, newRowNo, dcNo);
                }
            }
            if (obj.attr("tagname") != "IMG")
                obj.attr("name", newFldName);
        }
    }
}

//Updating the onClick event of the grid reference 
function UpdateGridReferFunc(obj, newFldName, newRowNo, dcNo) {

    var objOnClick = obj.attr("onclick");
    obj.removeAttr("onclick");
    var lastCommaIndx = objOnClick.lastIndexOf(",");
    var closeIndx = objOnClick.indexOf(")");
    var openIndx = objOnClick.indexOf("(");
    //Getting the field
    var Fld = objOnClick.substr(openIndx + 2, (objOnClick.indexOf(",") - openIndx) - 1);
    var oldFldName = GetFieldsName(Fld);
    //Giving the newrow number to Id
    var newDelFldId = oldFldName + newRowNo + "F" + dcNo;
    //Getting the fldvalue(filename to be deleted)
    var fileToDelete = objOnClick.substr(lastCommaIndx + 2, (closeIndx - lastCommaIndx) - 3);
    obj.attr("onclick", "DeleteFileFromRow('" + newDelFldId + "', '" + newRowNo + "F" + dcNo + "', '" + fileToDelete + "')");

    return obj;
}
//function AddNewRow(cmd, obj, id) {
//    var dcNo = id.substring(6);
//    AddRow(dcNo);
//}

function SetSerialNo(dcNo, rowNo) {
    var slCnt = $j("#hdnSlNoCnt" + dcNo);
    var newSlNo = parseInt(slCnt.val(), 10) + 1;
    slCnt.val(newSlNo);
    var lblSl = $j("#lblSlNo" + rowNo + "F" + dcNo);
    lblSl.text(newSlNo);
}

function UpdateFieldsInNewRow(dcNo, rowNo, newRow) {

    var dbRowNo = GetDbRowNo(rowNo, dcNo);
    var flag = false;
    if (rowNo != "000") {
        var fields = GetGridFields(dcNo);
        for (var i = 0; i < fields.length; i++) {
            var fldName = fields[i] + rowNo + "F" + dcNo;
            UpdateFieldArray(fldName, dbRowNo, "", "parent", "");
        }

        //AGC-TODO: Add row for new design needs to be handled- here the hyperlink for attachment is being removed
        //The grid attachment field will be cleared
        var rNo = "00" + (rowNo - 1);
        //newRow.find('div[id^=Link_' + rNo + ']').each(function () {
        //    $j(this).remove();
        //    flag = true;
        //});

    }
}

function AddInlineRow(dcNo, calledFrom, keyColValue, newCalledFrom) {

    var srcCalledFrom;
    if (calledFrom == undefined && $j("#addrow" + dcNo).length > 0 && $j("#addrow" + dcNo).attr("class").indexOf("disableadd") != -1)
        return;

    if (IsDcPopGrid(dcNo)) {
        AddSubGridRow(dcNo, calledFrom);
        return;
    }

    ShowDimmer(true);
    IsAddRowCalled = false;
    var tableName = "gridDc" + dcNo;
    var newRow = "";
    var sTRowIndx = -1;
    var hdnRowCount = $j("#hdnRCntDc" + dcNo);
    if (hdnRowCount.length > 0) {

        var dcClientRows = GetDcClientRows(dcNo);
        var lastRow = dcClientRows.getMaxVal();
        if (lastRow == 0) lastRow = 1;
        if (lastRow == 1 && typeof IsObjCustomHtml != "undefined" && IsObjCustomHtml == 'True') {
            lastRow = 2;
        }
        var newCnt = parseInt(lastRow, 10);
        var newRowNo = GetRowNoHelper(newCnt);
        srcCalledFrom = calledFrom;
        var rowCount = $j("#hdnRCntDc" + dcNo).val();
        calledFrom = "AddRow";

        RegisterActiveRow(newRowNo, dcNo);
        AxActiveDc = dcNo;

        UpdateDcRowArrays(dcNo, newRowNo, "Add");
        //SetRowCount(dcNo, newCnt, "", calledFrom);

        //On clearing the rows, the row will be added to the DeletedDCRows,
        //if you add the same row agian then the row should be removed from the DeletedDCRows
        var rowFrameNo = newRowNo + "F" + dcNo;
        var ind = $j.inArray(rowFrameNo, DeletedDCRows);
        if (ind != -1)
            DeletedDCRows.splice(ind, 1);
        CheckScroll(dcNo);
        var fields = GetGridFields(dcNo);
        FillNewRowComboValues(dcNo, newRowNo, fields, keyColValue);

        //To update the parent rows in the pop up arrays
        if (TstructHasPop) {
            var IsParentGrid = IsDcParentGrid(dcNo);
            if (IsParentGrid)
                UpdatePopUpArrays(dcNo, newRowNo, false, "Add");
        }
        if (calledFrom == undefined || calledFrom == "newRow" || calledFrom == "AddRow") {
            if (newCalledFrom != "GetDep" && newCalledFrom != "Action") { //if (newCalledFrom != "GetDep") {//Addrow service calling after import row to grid action call, so it has avoided with this condition 
                if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
                    CallEvaluateOnAddPerf(dcNo, newRowNo, fields, calledFrom);
                else
                    CallEvaluateOnAdd(dcNo, newRowNo, fields, calledFrom);
            }
        }

        if (!IsAddRowCalled && (calledFrom == undefined || calledFrom == "newRow"))
            UpdateRowInDataObj();

        if (calledFrom == undefined || calledFrom == "newRow") {
            FocusOnLastField(dcNo, newRowNo);
        }
        try {

            AxAfterAddRow(dcNo, newRowNo);
        } catch (ex) {}
        SetDynamicDcCaptions(dcNo);

        axpBtnClickEvent("sp" + dcNo + "R" + rowFrameNo);

        // adjustwin("10", window);
    }
    //Below code should not call for inlinegrid edit becuase this, if the tstruct load in popup getting tab focus to header dc while add new row.
    //if (isTstPop == "True") {
    //    TstructTabEventsInPopUP("");
    //}
    return false;
}



function AddRow(dcNo, calledFrom, keyColValue) {

    //UnHighlightRow();
    var srcCalledFrom;
    if (calledFrom == undefined && $j("#addrow" + dcNo).length > 0 && $j("#addrow" + dcNo).attr("class").indexOf("disableadd") != -1)
        return;

    if (IsDcPopGrid(dcNo)) {
        AddSubGridRow(dcNo, calledFrom);
        return;
    }

    ShowDimmer(true);
    IsAddRowCalled = false;
    var tableName = "gridDc" + dcNo;
    var newRow = "";
    var sTRowIndx = -1;
    //If there is a default group in the format grid with empty row, on add row simply return.
    //Hack: If keycolumn value is empty, the if condition{if (keyColValue)} fails so added as part of the or condition.    
    //if (keyColValue || keyColValue == "") {
    //    if (keyColValue == "") return;
    //    sTRowIndx = GetFormatRowIndex(tableName, dcNo, keyColValue, "strow");
    //    newRow = $j('#' + tableName + ' tbody>tr:eq(' + (sTRowIndx - 1) + ')').clone(true);
    //}
    //else
    //    newRow = $j('#' + tableName + ' tbody>tr:last').clone(true);
    //var oldRowNo = newRow.attr("id");

    var hdnRowCount = $j("#hdnRCntDc" + dcNo);
    if (hdnRowCount.length > 0) {

        var dcClientRows = GetDcClientRows(dcNo);
        var lastRow = dcClientRows.getMaxVal();
        if (lastRow == 0) lastRow = 1;
        var rowCount = $j("#hdnRCntDc" + dcNo).val();
        var newCnt = parseInt(lastRow, 10) + 1;
        var newRowNo = GetRowNoHelper(newCnt);
        srcCalledFrom = calledFrom;
        if (calledFrom == "AddInlineRow" || isMobileGridRowEdit) {
            isMobileGridRowEdit = false;
            newCnt = newCnt - 1;
            newRowNo = GetRowNoHelper(newCnt);
            calledFrom = "AddRow";
        }

        //newRow.attr("id", "sp" + dcNo + "R" + newRowNo + "F" + dcNo);
        //newRow.find(':input:not(.multiFldChk)').val('');

        ////Converting the id of the dc_referimages div
        //newRow.find("div[id^='ReferLink']").each(function () {
        //    var oldReferId = $j(this).attr("id");
        //    var oldID = oldReferId.split('_');
        //    if (oldID[1] != undefined && oldID[2] != undefined ) {
        //        $j(this).attr("id",'ReferLink_' + newRowNo + "_" + oldID[2]);
        //    }

        //});
        //***********Note need to hadle coloring techinques in new grid
        //newRow.find('select,input,label,img,.Grdlnk,textarea,.grdAttach,.pickimg,.grdRefer,a, div[id^=GridAttach]').each(function () {
        //    ConvertFieldID($j(this), newRowNo);
        //    $j(this).css("background-color", "");
        //    $j(this).css("font-family", "");
        //    $j(this).css('font-weight', "");
        //    $j(this).css('font-style', "");
        //    $j(this).css("text-decoration", "");
        //    $j(this).css("color", "");
        //    $j(this).css("background", "");
        //});

        ////Code to clear the checkbox selection in the new row
        //if (newRow.find(':checkbox').length > 0)
        //    newRow.find(':checkbox').attr('checked', false);

        var rid = $j("#recordid000F0").val();
        ArrActionLog = "AddRow-DcNo-" + dcNo + "-rowno-" + newRowNo + "-Recordid-" + rid;

        RegisterActiveRow(newRowNo, dcNo);
        AxActiveDc = dcNo;
        if (AxpGridForm != "form") {
            //wrapperForEditFields2
            $j("#wrapperForEditFields" + dcNo + " .date").datepicker("destroy");
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
            $j("#wrapperForEditFields" + dcNo + " .date").datepicker({
                isRTL: dtpkrRTL,
                dateFormat: dtFormat,
                //showOn: "button",
                //buttonImage: "../AxpImages/icons/16x16/calendar.png",
                //buttonImageOnly: true,
                //buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                yearRange: "-100:+100"

            });

            TimePickerEvent("#wrapperForEditFields" + dcNo, dtpkrRTL);
        }

        //delImg = newRow.find('.rowdelete');
        //if (delImg.attr("id") != undefined)
        //    delImg.attr("id", ("del" + newRowNo + "F" + dcNo));
        //if (keyColValue && sTRowIndx != -1) {
        //    //:eq('+index+')
        //    newRow.insertAfter('#' + tableName + ' tbody>tr:eq(' + (sTRowIndx - 1) + ')');
        //}
        //else {
        //    newRow.insertAfter('#' + tableName + ' tbody>tr:last');
        //}
        //AssignJQueryEvents(["#sp" + dcNo + "R" + newRowNo + "F" + dcNo]);

        axpBtnClickEvent("sp" + dcNo + "R" + rowFrameNo);

        UpdateDcRowArrays(dcNo, newRowNo, "Add");
        if (calledFrom == undefined || calledFrom == "newRow")
            SetRowCount(dcNo, (parseInt(rowCount, 10) + 1), "i" + (parseInt(rowCount, 10) + 1));
        else {
            if (calledFrom == "FillGrid" && axInlineGridEdit)
                SetRowCount(dcNo, rowCount, "", calledFrom);
            else
                SetRowCount(dcNo, newCnt, "", calledFrom);
        }

        var rowFrameNo = newRowNo + "F" + dcNo;
        //On clearing the rows, the row will be added to the DeletedDCRows,
        //if you add the same row agian then the row should be removed from the DeletedDCRows

        var ind = $j.inArray(rowFrameNo, DeletedDCRows);
        if (ind != -1)
            DeletedDCRows.splice(ind, 1);

        CheckScroll(dcNo);

        if (calledFrom == undefined)
            UpdateFieldsInNewRow(dcNo, newRowNo, newRow);

        if (keyColValue)
            SetFGSerialNo(dcNo, newRowNo, oldRowNo);
        else
            SetSerialNo(dcNo, newRowNo);
        var fields = GetGridFields(dcNo);
        FillNewRowComboValues(dcNo, newRowNo, fields, keyColValue);
        if (calledFrom == undefined)
            FillAcceptSqlFlds(dcNo, newRowNo, keyColValue);
        //To update the parent rows in the pop up arrays
        if (TstructHasPop) {
            var IsParentGrid = IsDcParentGrid(dcNo);
            if (IsParentGrid)
                UpdatePopUpArrays(dcNo, newRowNo, false, "Add");
        }
        if (calledFrom == undefined || calledFrom == "newRow" || calledFrom == "AddRow") {
            if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
                CallEvaluateOnAddPerf(dcNo, newRowNo, fields, calledFrom);
            else
                CallEvaluateOnAdd(dcNo, newRowNo, fields, calledFrom);
        }
        if (keyColValue) {
            SetKeyColValue(dcNo, newRowNo, keyColValue);
            newCnt = parseInt(newRowNo, 10) + 1;
            newRowNo = GetRowNoHelper(newCnt);
            ResetRowStyle(newRowNo, dcNo);
        } else
            ResetRowStyle(newRowNo, dcNo);
        IsFunction = "";
        if (calledFrom == undefined) {
            CheckAxpvalid(dcNo, newRowNo, fields);
        }

        if (!IsAddRowCalled && (calledFrom == undefined || calledFrom == "newRow")) {
            UpdateRowInDataObj();

        }
        if (!IsUpdateRowcalled && !IsAddRowCalled) {
            ShowDimmer(false);
        }

        if (calledFrom == undefined || calledFrom == "newRow") {
            FocusOnLastField(dcNo, newRowNo);
        }
        try {

            AxAfterAddRow(dcNo, newRowNo);
        } catch (ex) {}
        SetDynamicDcCaptions(dcNo);

        // adjustwin("10", window);
    }
    if (isTstPop == "True") {
        TstructTabEventsInPopUP("");
    }
    return false;
}

//Function to update the newly added rows fields in the tstruct data object array.
function UpdateRowInDataObj() {
    IsUpdateRowcalled = true;
    try {
        ASB.WebService.AppendRowToDataObj(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ArrActionLog, tstDataId, resTstHtmlLS, SuccessUpdateRow);
    } catch (exp) {
        AxWaitCursor(false);
        showAlertDialog("error", ServerErrMsg);
    }
    ChangedFields = new Array();
    ChangedFieldDbRowNo = new Array();
    ChangedFieldValues = new Array();
    DeletedDCRows = new Array();
}

function SuccessUpdateRow(result) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    ArrActionLog = "";
    ChangedFields = new Array();
    ChangedFieldDbRowNo = new Array();
    ChangedFieldValues = new Array();
    DeletedDCRows = new Array();
    IsUpdateRowcalled = false;
    ShowDimmer(false);
}

function CallAddRowWS(dcStr, rowNo) {
    AxWaitCursor(true);
    ShowDimmer(true);
    var visDcname = GetOpenTabDcs();
    var dbRowNo = GetDbRowNo(rowNo, dcStr);
    var rid = $j("#recordid000F0").val();
    var filename = traceSplitStr + "AddRow-" + transid + traceSplitChar;
    IsAddRowCalled = true;
    var ixml = '<sqlresultset axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" dcname="' + dcStr + '" rowno="' + dbRowNo + '" dcnames="' + visDcname + '" sessionid="' + sid + '" trace="' + filename + '" >';
    try {
        ASB.WebService.AddRowWS(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ixml, dcStr, tstDataId, resTstHtmlLS, SuccessCalAddData, ErrorAddRowWS);
    } catch (exp) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
    //  adjustwin("10", window);
    AxWaitCursor(false);
}

function CallAddRowPerfWS(dcStr, rowNo) {
    //Temporary code
    StartTime = new Date().getTime();
    //End Temporary code
    if (typeof axpScriptaddrowres != "undefined" && axpScriptaddrowres != "") {
        IsAddRowCalled = true;
        SuccessCalAddData(axpScriptaddrowres, "");
        return;
    }
    AxWaitCursor(true);
    ShowDimmer(true);
    var visDcname = GetOpenTabDcs();
    var dbRowNo = GetDbRowNo(rowNo, dcStr);
    var rid = $j("#recordid000F0").val();
    var filename = traceSplitStr + "AddRow-" + transid + traceSplitChar;
    IsAddRowCalled = true;
    var ixml = '<sqlresultset axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" dcname="' + dcStr + '" rowno="' + dbRowNo + '" dcnames="' + visDcname + '" sessionid="' + sid + '" trace="' + filename + '" >';
    try {
        isAddRowWsCalled = "true";
        GetProcessTime();
        ASB.WebService.AddRowPerfWS(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, ixml, dcStr, tstDataId, false, resTstHtmlLS, SuccessCalAddData, ErrorAddRowWS);
    } catch (exp) {
        isAddRowWsCalled = "false";
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        UpdateExceptionMessageInET("AddRowPerfWS exception : " + exp.message);

        GetProcessTime();
        GetTotalElapsTime();
    }
    //  adjustwin("10", window);
    AxWaitCursor(false);
}

function SuccessCalAddData(result, eventArgs) {
    if (result != "") {
        if (result.split("♠*♠").length > 1) {
            tstDataId = result.split("♠*♠")[0];
            result = result.split("♠*♠")[1];
        }
        if (result.split("*♠*").length > 1) {
            var serverprocesstime = result.split("*♠*")[0];
            var requestProcess_logtime = result.split("*♠*")[1];
            result = result.split("*♠*")[2];
            WireElapsTime(serverprocesstime, requestProcess_logtime, true);
        } else {
            UpdateExceptionMessageInET("Error : " + result);
        }
    }

    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    ArrActionLog = "";
    ChangedFields = new Array();
    ChangedFieldDbRowNo = new Array();
    ChangedFieldValues = new Array();
    DeletedDCRows = new Array();
    AssignLoadValues(result, "CallAdd");
    let resAddrow = result;
    if (resAddrow != "")
        resAddrow = resAddrow.replace(/\\/g, ";bkslh"); //Replace(@"\", "\\"); 
    var parsedAddData = JSON.parse(resAddrow).data;
    if ((parsedAddData[0].n == "DC" + AxActiveDc && parsedAddData[0].t == "dc") || parsedAddData.filter(x => x.n == "DC" + AxActiveDc && x.t == "dc").length > 0) {
        let _reVals = parsedAddData.filter(x => x.n == "DC" + AxActiveDc && x.t == "dc");
        if (axInlineGridEdit || (isMobile && (isMobile && !axInlineGridEdit)))
            UpdateFieldArray(ConstructFieldName(axpIsRowValid + AxActiveDc, AxActiveDc, _reVals[0].v), _reVals[0].v, "", "parent", "AddRow");
        else
            UpdateFieldArray(ConstructFieldName(axpIsRowValid + AxActiveDc, AxActiveDc, _reVals[0].v), _reVals[0].v, "false", "parent", "AddRow");
    }
    ShowDimmer(false);
    if (axpScanBarFldFocus != "") {
        updateInlineGridRowValues();
        axpScanBarFldFocus = "";
    }
    //isAddRowWsCalled = "false";
    if (isAddRowWsCalled != "false" && isAddRowWsCalled != "true") {
        let _isAddRowWsCalled = isAddRowWsCalled;
        isAddRowWsCalled = "false";
        setTimeout(function () {
            setTimeout(function () {
                if (recordid == "0") {
                    let _thisId = $("#" + _isAddRowWsCalled).parent().find("select.form-select").attr("id");
                    if (typeof _thisId != "undefined" && _thisId != "") {
                        let _fldName = GetFieldsName(_thisId);
                        let _fldInd = GetFieldIndex(_fldName);
                        let _fldAEmpty = GetFieldProp(_fldInd, 'allowEmpty');
                        let _thidVal = GetFieldValueNew(_thisId);
                        if (_thidVal == "") {
                            if (_fldAEmpty == "F")
                                $("#" + _isAddRowWsCalled).select2('open');
                            else {
                                select2IsOpened = true;
                                select2IsFocused = true;
                                select2EventType = "click";
                                //$("#"+_isAddRowWsCalled).focus();
                                $("#" + _isAddRowWsCalled).select2('open');
                            }
                        }
                    }
                }
            }, 0);
        }, 100);
    }
    isAddRowWsCalled = "false";
    //Temporary code
    var edTime = new Date().getTime();
    var diff = edTime - StartTime;
    console.log("Addrow :" + diff + " ms");
    //End Temporary code
    GetProcessTime();
    GetTotalElapsTime();
}

//Function to call evaluate for the newly added row.
function CallEvaluate(dcNo, rowNo, fields) {

    for (var i = 0; i < fields.length; i++) {

        var fldName = fields[i] + rowNo + "F" + dcNo;
        var oldValue = GetFieldValue(fldName);

        var isBound = false;
        isBound = ISBound(fields[i], rowNo);
        if (isBound) {
            EvaluateAxFunction(fields[i], fldName, rowNo + "F" + dcNo);
            var newValue = GetFieldValue(fldName);
            var fldDbRowNo = GetDbRowNo(rowNo, dcNo);
            //UpdateFieldArray(fldName, fldDbRowNo, newValue, "parent");
            IsService = false;
            if (oldValue == parseInt(newValue, 10))
                continue;
        }
    }
}

//Function to call evaluate for the newly added row.
function CallEvaluateOnAdd(dcNo, rowNo, fields, calldepField) {
    var isGrid = false;
    isGrid = IsDcGrid(dcNo);

    var callService = false;
    for (var i = 0; i < fields.length; i++) {
        var idx = GetFieldIndex(fields[i]);
        var fldIdnName = fields[i];
        var fldName = fields[i] + rowNo + "F" + dcNo;
        var type = $j("#" + fldName).attr("type");
        var oldValue = GetFieldValue(fldName);

        var isBound = false;
        isBound = ISBound(fields[i], rowNo);
        if (isBound) {
            if (calldepField == "newRow" || calldepField == "AddRow") {
                EvaluateAxFunction(fields[i], fldName, rowNo + "F" + dcNo);
                EvaluateExpressions(fldName);
            } else
                EvaluateAxFunction(fields[i], fldName, rowNo + "F" + dcNo);
            var newValue = GetFieldValue(fldName);
            var fldDbRowNo = GetDbRowNo(rowNo, dcNo);
            UpdateFieldArray(fldName, fldDbRowNo, newValue, "parent", "AddRow");
            IsService = false;
            if (oldValue == parseInt(newValue, 10))
                continue;

            if (oldValue != newValue) {
                if (CheckDependency(fldName, "AddRow") == true && callService == false && calldepField != "Delete") {
                    callService = true;
                }
                //ShowDimmer(true);
            }

            //On grid row add should call addrow service if the any field is Accept+SQL for first row else bind previous row field value. 11/07/2018
            if (callService == false && (calldepField == "newRow" || calldepField == "AddRow" || calldepField == "ToCheckAddRow")) {
                var fldIndx = GetFieldIndex(fldIdnName);
                if (fldIndx != -1 && FMoe[fldIndx].toString().toLowerCase() == "accept" && FldIsSql[fldIndx].toString().toLowerCase() == "true") {
                    callService = true;
                    $("#" + fldName).addClass("addServiceCallMade");
                }
            }
            //ShowDimmer(true);

            //if the field is an input and has its parents in non grid and no parents in the current grid 
            //If the field has a Fill or sql, and if it is in a grid, then the value should be copied from master row.
            //commented as logic doesnt suit new grid (24/04/2017)

            //if (isGrid && type == "text") {

            //    if (FMoe[idx] == "Fill") {
            //        if (ISBound(fields[i], rowNo)) {
            //            CopyFromMasterRow(fldName, dcNo, rowNo, "AddRow", "", false);
            //        }
            //    }
            //}
        }
    }

    UpdateAxpRowVldInArray(dcNo, rowNo, fldDbRowNo);

    if (callService) {
        callService = false;
        ShowDimmer(true);
        //Below service call is commented assuming that the row is not ediatble and does not require new values, later if there is any issue the code needs to be uncommented
        CallAddRowWS(dcNo, GetRowNoHelper(parseInt(rowNo, 10)));
    } else {
        if (calldepField != "FillGrid") //!axInlineGridEdit
        {
            UpdateFieldArray(ConstructFieldName(axpIsRowValid + dcNo, dcNo, parseInt(rowNo, 10)), rowNo, "", "parent", "AddRow");
        }
        //ShowDimmer(false);
    }
}

function CallEvaluateOnAddPerf(dcNo, rowNo, fields, calldepField) {
    var isGrid = false;
    isGrid = IsDcGrid(dcNo);
    var fldAcceptSql = new Array();
    $j("#chkallgridrow"+dcNo).prop("disabled", false);
    $j("#grdchkItemTd"+rowNo + "F" + dcNo).prop("disabled", false);
    
    var callService = false;
    for (var i = 0; i < fields.length; i++) {
        var idx = GetFieldIndex(fields[i]);
        var fldIdnName = fields[i];
        var fldName = fields[i] + rowNo + "F" + dcNo;
        var type = $j("#" + fldName).attr("type");
        var oldValue = GetFieldValue(fldName);

        if (calldepField == "newRow" || calldepField == "AddRow" || calldepField == "AllRowClear") {
            EvaluateAxFunction(fields[i], fldName, rowNo + "F" + dcNo, calldepField);
            EvaluateExpressions(fldName);
        } else
            EvaluateAxFunction(fields[i], fldName, rowNo + "F" + dcNo, calldepField);
        var newValue = GetFieldValue(fldName);
        var fldDbRowNo = GetDbRowNo(rowNo, dcNo);
        UpdateFieldArray(fldName, fldDbRowNo, newValue, "parent", "AddRow");
        IsService = false;
        if (oldValue == parseInt(newValue, 10))
            continue;

        if (oldValue != newValue) {
            if (CheckDependencyPerf(fldName, "AddRow") == true && callService == false && calldepField != "Delete" && calldepField != "AllRowClear" && calldepField != "FillGrid") {
                callService = true;
            }
        }

        //On grid row add should call addrow service if the any field is Accept+SQL for first row else bind previous row field value. 11/07/2018
        if (callService == false && (calldepField == "newRow" || calldepField == "AddRow" || calldepField == "ToCheckAddRow")) {
            var fldIndx = GetFieldIndex(fldIdnName);
            if (fldIndx != -1 && FMoe[fldIndx].toString().toLowerCase() == "accept" && FldIsSql[fldIndx].toString().toLowerCase() == "true") {
                var isBound = false;
                isBound = ISBoundPerf(fields[i], rowNo);
                depNotBoundFld = ""
                if (isBound) {
                    callService = true;
                    $("#" + fldName).addClass("addServiceCallMade");
                }
            } else if (fldIndx != -1 && FMoe[fldIndx].toString().toLowerCase() == "select" && FldAutoSelect[fldIndx].toString().toLowerCase() == "true" && typeof AxFillAutoSelectFlds != "undefined" && AxFillAutoSelectFlds == "true") {
                var isBound = false;
                isBound = ISBoundPerf(fields[i], rowNo);
                depNotBoundFld = ""
                if (isBound) {
                    callService = true;
                    $("#" + fldName).addClass("addServiceCallMade");
                }
            }
        }

        if (callService == false && (calldepField == "newRow" || calldepField == "AddRow" || calldepField == "ToCheckAddRow")) {
            var fldIndx = GetFieldIndex(fldIdnName);
            if (fldIndx != -1 && FMoe[fldIndx].toString().toLowerCase() == "select" && GetFldNamesIndx(fldIdnName) > -1) {
                callService = true;
            }
        }

        if (callService == false && (calldepField == "newRow" || calldepField == "AddRow" || calldepField == "ToCheckAddRow")) {
            if (typeof AxFldExpOnAddRow != "undefined" && AxFldExpOnAddRow != "") {
                var AxExpDepField = AxFldExpOnAddRow.split(",");
                if (AxExpDepField.indexOf(fldIdnName) > -1)
                    callService = true;
            }
        }
    }
    if ((!isMobile || (isMobile && axInlineGridEdit)))
        UpdateAxpRowVldInArray(dcNo, rowNo, fldDbRowNo);

    if (callService) {
        callService = false;
        ShowDimmer(true);
        //Below service call is commented assuming that the row is not ediatble and does not require new values, later if there is any issue the code needs to be uncommented
        CallAddRowPerfWS(dcNo, GetRowNoHelper(parseInt(rowNo, 10)));
    } else {
        if (calldepField != "FillGrid" && (!isMobile || (isMobile && axInlineGridEdit)))
        {
            if (!gridRowEditOnLoad)
                UpdateFieldArray(ConstructFieldName(axpIsRowValid + dcNo, dcNo, parseInt(rowNo, 10)), rowNo, "", "parent", "AddRow");
        }
    }
}

function FillAcceptSqlFlds(dcNo, rowNo, keyColValue) {
    var idx = $j.inArray(dcNo, DCFrameNo);
    var isMasterFld = true;
    if (idx != -1) {
        var dcAccFlds = DcAcceptMRFlds[idx].toString().split(",");
        //for each accept field with sql, check if any of the parents are in current dc.
        //if true then do nothing else copy from master row.
        for (var i = 0; i < dcAccFlds.length; i++) {
            var fldInd = GetFieldIndex(dcAccFlds[i]);
            var fldId = dcAccFlds[i] + rowNo + "F" + dcNo;
            if (fldInd != -1) {
                var parentStr = FldParents[fldInd].toString().split(",");
                if (parentStr != "") {
                    for (var j = 0; j < parentStr.length; j++) {

                        var parDcNo = GetDcNo(parentStr[j]);
                        if (parDcNo == dcNo) {
                            isMasterFld = false;
                            break;
                        }
                    }
                }
                if (isMasterFld)
                    CopyFromMasterRow(fldId, dcNo, "", "AddRow", keyColValue);
            }
        }
    }
}

//Function which fills the combo values in the new row if the master row is 1 for the field.
function FillNewRowComboValues(dcNo, rowNo, fields, keyColValue) {

    for (var i = 0; i < fields.length; i++) {
        var IsMasterRow = IsInMasterRow(fields[i]);

        var fldName = "#" + fields[i] + rowNo + "F" + dcNo;
        var fldId = fields[i] + rowNo + "F" + dcNo;
        var fld = $j(fldName);
        if (fld.length > 0) {
            var fldType = fld.prop("type");
            //if (fldType == "select-one") {
            if (IsMasterRow == true) {
                // CopyFromMasterRow(fldId, dcNo, "", "AddRow", keyColValue);
                if (fldType == "select-one") {
                    $j(fldName + " option:selected").removeAttr("selected");
                    $j(fldName).val("");

                } else if (fldType == "checkbox")
                    $j(fldName).removeAttr("checked").prop("checked", false);

                //Fix for autocomplete field not getting cleared if it is masterrow true and autocomplte
                if (fld.hasClass("fldFromSelect") || fld.hasClass("fldmultiSelect")) {
                    $j(fldName).val("");
                }
            } else {
                if (fldType == "select-one")
                    $j(fldName + " option:gt(0)").remove();
                else if (fldType == "checkbox")
                    $j(fldName).removeAttr("checked").prop("checked", false);
                else {
                    $j(fldName).val("");
                    if (fld[0])
                        fld[0].defaultValue = "";
                }
            }
            // }
        }
    }
}

//Function which returns true if the field is in master row array.
function IsInMasterRow(fieldName) {

    for (var i = 0; i < MasterRow.length; i++) {
        if (fieldName == MasterRow[i])
            return true;
    }
    return false;
}

function DeleteCurrentRow(imgObj) {
    //row id format - sp + dcno + "R" + rowno + "F" + dcno
    var rowId = imgObj.parent().parent().attr("id");
    var dcNo = rowId.substring(2, rowId.indexOf("R"));
    var rowFrmNo = rowId.split("R")[1];
    DeleteRow(dcNo, rowFrmNo);
}

//Function which calls the Delete Grid row function.
function DeleteRow(dcNo, rowFrmNo, elem) {
    //returning back if its having disable class
    if (elem != undefined) {
        if ($(elem).hasClass('disabled'))
            return false;
    }
    //if (!checkEditMode())
    //    return;

    //HighlightRow(dcNo, rowFrmNo.substring(0, rowFrmNo.indexOf("F")), "focus", "Img");

    //for inline grid edit - if any row is in edit mode
    if (axInlineGridEdit && $(".inline-edit").length) {
        //clear row inline mode if any other row delete button clicks
        if ($(".inline-edit").attr("id") != $(elem).closest("tr").attr("id"))
            updateInlineGridRowValues();
    }
    if ((axInlineGridEdit && AxpGridForm != "form") || (!isMobile && mobileCardLayout == "none")) {
        if ($j("#del" + rowFrmNo).length > 0 && $j("#del" + rowFrmNo).attr("class").indexOf("disabledelete") != -1)
            return;
    }
    var glType = eval(callParent('gllangType'));
    var isRTL = false;
    if (glType == "ar")
        isRTL = true;
    else
        isRTL = false;
    var cutMsg = eval(callParent('lcm[5]'));
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
                        if (axInlineGridEdit)
                            updateInlineGridRowValues(); // clears if any grid row is in edit mode & updates the field id's
                        DeleteGridRow(dcNo, rowFrmNo, undefined);
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


    if (arrRefreshDcs.length > 0) {
        for (var i = 0; i < arrRefreshDcs.length; i++) {
            var arrDcNos = arrRefreshDcs[i].split(':');
            if (arrDcNos[1] == "dc" + dcNo) {
                arrRefreshFldDirty[i] = true;
                break;
            }
        }
    }
    OnRowChangeSetHeight(dcNo);
    //Refer bug-AGI003560 - commented the adjustEditLayoutId as it should not change the id of wrapper on delete- added code in deletegridrow
    //adjustEditLayoutId(dcNo);
    //var wrapperForEditFields = "wrapperForEditFields" + dcNo;
    //var idOfPresentEdit = $j("#" + wrapperForEditFields + " .editWrapTr").attr('id'); //sp2R002F2
    //idOfPresentEdit = idOfPresentEdit.substr(0, idOfPresentEdit.indexOf('F')); //sp2R002
    //idOfPresentEdit = idOfPresentEdit.substr(idOfPresentEdit.length - 3); //2

    //Below function call is commented assuming that the on row delete no need to Evaluate Ax Function.
    //var fields = GetGridFields(dcNo);
    // CallEvaluateOnAdd(dcNo, idOfPresentEdit, fields, "Delete");
    checkTableBodyWidths(dcNo);
    if (isTstPop == "True") {
        TstructTabEventsInPopUP("");
    }

}

function SetFormDirty(status) {
    IsFormDirty = status;
    window.parent.globalChange = status;
}

//Function which deletes the given row from the given dc no.
function DeleteGridRow(dcNo, rowFrmNo, webService) {

    ShowDimmer(true);

    if (!IsFormDirty)
        SetFormDirty(true);
    AxActiveDc = dcNo;
    IsFunction = "";
    IsService = false;
    var IsParentGrid = false;
    var delDcStr = "";
    var rowFNo = rowFrmNo.split("F");
    var rowNo = rowFNo[0];
    var rowCnt = $j("#hdnRCntDc" + dcNo).val();
    rowCnt = parseInt(rowCnt, 10);
    var dbRowNo = GetDbRowNo(rowNo, dcNo);
    delDcStr = "dc" + dcNo;
    var dcIndx = GetFormatGridIndex(dcNo);  

    var rid = $j("#recordid000F0").val();
    ArrActionLog = "DeleteRow-DcNo-" + dcNo + "-rowno-" + rowFrmNo + "-Recordid-" + rid;
    if (dcIndx != -1) {
        if (IsOnlyFormatGridRow(dcNo, rowNo, dcIndx)) {
            UpdateSubTotalOnDelRow(dcIndx, dcNo, rowNo);
            var keyColumn = DcKeyColumns[dcIndx].toString();
            ClearDeletedFields(dcNo, rowNo, keyColumn, true);
            AddDeletedRowsToArray(dcNo, rowNo);
            var fields = GetGridFields(dcNo);
            CallEvaluate(dcNo, rowNo, fields);
            SetRowCount(dcNo, rowCnt - 1, "d");
            var kColVal = GetFieldValue(keyColumn + rowNo + "F" + dcNo);
            if (webService == undefined && GetGrpCntValue(dcNo, kColVal) != "cleared") {
                var keyColValue = GetFieldValue(keyColumn + rowNo + "F" + dcNo);
                SetGrpCntValue(dcNo, keyColValue);
                if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
                    CallDeletePerfWS(delDcStr);
                else
                    CallDeleteWS(delDcStr);
            } else {
                if (webService != "group")
                    DeletedDCRows = new Array();
                ShowDimmer(false);
                AxWaitCursor(false);
            }
            $("#chkallgridrow" + dcNo).prop("checked", false);
            $("#clearThisDC" + dcNo).addClass("disabled");            
            return;
        }
        //update the subtotal
        UpdateSubTotalOnDelRow(dcIndx, dcNo, rowNo);
    }

    var onlyRow = IsSingleRow(dcNo);
    if (TstructHasPop) {

        IsParentGrid = IsDcParentGrid(dcNo);
        if (IsParentGrid) {
            delDcStr += "," + GetPopGridDcNo(dcNo);

            DeletePopRowsForDc(dcNo, rowNo);
        }
    }
    var IsPopGrid = false;
    if (TstructHasPop) {
        IsPopGrid = IsDcPopGrid(dcNo);
    }
    if (!onlyRow) {

        var slNo = GetSerialNoCnt(dcNo);
        slNo = parseInt(slNo, 10) - 1;

        if (dcIndx != -1) {
            ResetFormatGridSlNo(dcNo, rowNo, dcIndx);
        }

        //This updates the rowcount in the array.
        if (!axInlineGridEdit)
            SetRowCount(dcNo, rowCnt - 1, "d");

        SetSerialNoCnt(dcNo, slNo);
        AddDeletedRowsToArray(dcNo, rowNo)

        UpdateFldArrayInDeleteRow(dcNo, rowNo);//Remove this row field values from ALLField array 

        var a = "sp" + dcNo + "R" + rowNo + "F" + dcNo;
        if (!$j("#" + a).hasClass("editWrapTr"))
            $j("#" + a).remove();


        //This updates the rowcount in the array.
        if (axInlineGridEdit)
            SetRowCount(dcNo, rowCnt - 1, "d");

        if (dcIndx == -1) {
            for (var j = 1; j <= rowCnt; j++) {
                var rowNu = GetRowNoHelper(j);
                ResetRowStyle(rowNu, dcNo);
            }
        }
        CheckScroll(dcNo);
        if (IsPopGrid)
            UpdatePopUpArrays(dcNo, rowNo, IsPopGrid, "Delete");


        UpdateDcRowArrays(dcNo, rowNo, "Delete", dcIndx);

    } else {
        UpdateFldArrayInDeleteRow(dcNo, rowNo);//Remove this row field values from ALLField array 

        AddDeletedRowsToArray(dcNo, rowNo);
        ClearDeletedFields(dcNo, rowNo);
        var fields = GetGridFields(dcNo);
        CallEvaluate(dcNo, rowNo, fields);
        //Refer bug-AGI003560 - Only if the record is new wrapper div id will be reset to 1, since existing row will be sent to service in deleted array
        if (rid != "0")
            changeEditLayoutIds(0, dcNo);

    }

    if (webService == undefined) {
        if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
            CallDeletePerfWS(delDcStr);
        else
            CallDeleteWS(delDcStr);
    }
    $("#chkallgridrow" + dcNo).prop("checked", false);
    $("#clearThisDC" + dcNo).addClass("disabled");    
    navValidator = false;
}

function CallDeleteWS(dcStr) {
    AxWaitCursor(true);
    var visDcname = GetOpenTabDcs();
    var rid = $j("#recordid000F0").val();
    var filename = traceSplitStr + "DeleteRow-" + transid + traceSplitChar;
    var tmpDelArr = new Array();
    tmpDelArr = DeletedDCRows.slice();
    DeletedDCRows = new Array();
    var dcNo = dcStr.toString().substring(2);

    var pRow = "";
    var pDc = "";
    if (IsDcPopGrid(dcNo)) {
        pRow = AxActivePRow;
        pDc = AxActivePDc;
    }

    var ixml = '<sqlresultset axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" dcname="' + dcStr + '"  dcnames="' + visDcname + '" sessionid="' + sid + '" trace="' + filename + '" prow="' + pRow + '" pdc="' + pDc + '">';
    try {
        ASB.WebService.DeleteRowWS(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, tmpDelArr, ArrActionLog, ixml, tstDataId, dcNo, resTstHtmlLS, SuccessDelData, OnException);
    } catch (exp) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
    //  adjustwin("10", window);
    AxWaitCursor(false);
}

function CallDeletePerfWS(dcStr) {
    AxWaitCursor(true);
    var visDcname = GetOpenTabDcs();
    var rid = $j("#recordid000F0").val();
    var filename = traceSplitStr + "DeleteRow-" + transid + traceSplitChar;
    var tmpDelArr = new Array();
    tmpDelArr = DeletedDCRows.slice();
    DeletedDCRows = new Array();
    var dcNo = dcStr.toString().substring(2);

    var pRow = "";
    var pDc = "";
    if (IsDcPopGrid(dcNo)) {
        pRow = AxActivePRow;
        pDc = AxActivePDc;
    }
    isExcelImpDelWS ="true";
    var ixml = '<sqlresultset axpapp="' + proj + '" transid="' + transid + '" recordid="' + rid + '" dcname="' + dcStr + '"  dcnames="' + visDcname + '" sessionid="' + sid + '" trace="' + filename + '" prow="' + pRow + '" pdc="' + pDc + '">';
    try {
        ASB.WebService.DeleteRowPerfWS(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, tmpDelArr, ArrActionLog, ixml, tstDataId, dcNo, DeletedRowNos, resTstHtmlLS, SuccessDelData, OnException);
    } catch (exp) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        isExcelImpDelWS = "false";
    }
    AxWaitCursor(false);
}

function SuccessDelData(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    ArrActionLog = "";
    ChangedFields = new Array();
    ChangedFieldDbRowNo = new Array();
    ChangedFieldValues = new Array();
    DeletedDCRows = new Array();
    DeletedRowNos = new Array();
    AssignLoadValues(result, "Delete");
    if (result == "") {
        try {
            if ($("#gridHd" + AxActiveDc + " tbody tr").length == 0) {
                var dcIdx = $j.inArray(AxActiveDc.toString(), DCFrameNo);
                DCHasDataRows[dcIdx] = "False";
            }
        } catch (ex) {}
    }
    UpdateFldArrayInFillgrid(AxActiveDc);
    if (IsDcPopGridCleared) {
        IsDcPopGridCleared = false;
        if (!axInlineGridEdit) {
            UpdateDcRowArrays(AxActiveDc, "001", "Add");
        }
    }
    isExcelImpDelWS = "false";
    ShowDimmer(false);
}

function ErrorAddRowWS() {
    isAddRowWsCalled = "false";
    ArrActionLog = "";
    ChangedFields = new Array();
    ChangedFieldDbRowNo = new Array();
    ChangedFieldValues = new Array();
    DeletedDCRows = new Array();
    AxWaitCursor(false);
    ShowDimmer(false);
}

function IsSingleRow(dcNo) {
    var isSingleRow = true;
    //var cnt = 0;
    //for (var i = 0; i < RowDcNo.length; i++) {
    //    if (dcNo == RowDcNo[i]) {
    //        cnt++;
    //        if (cnt > 1) {
    //            isSingleRow = false;
    //            break;
    //        }
    //    }
    //}

    $("#gridHd" + dcNo + " tbody tr").length === 0 ? isSingleRow = true : isSingleRow = false

    return isSingleRow;
}

//Function to clear the values in the deleted row fields.
function ClearDeletedFields(dcNo, rowNo, keyColumn, isOnlyRow) {

    var flds = GetGridFields(dcNo);
    var fldDbRowNo = GetDbRowNo(rowNo, dcNo);

    for (var i = 0; i < flds.length; i++) {

        var fName = flds[i] + rowNo + "F" + dcNo;
        var fld = document.getElementById(fName);

        if (keyColumn && keyColumn == flds[i])
            continue;

        if (flds[i] == "axp_gridattach_" + dcNo) {
            var lnkId = "#grdAtt_hlnk_" + rowNo + "F" + dcNo;
            $j(lnkId).text("");
        }

        if (fld) {

            if (fName.toString().toLowerCase().startsWith("sub" + dcNo + "_"))
                continue;
            if (fld.type == "select-one") {
                if (!$(fld).hasClass("form-select"))
                    fld.options[0].selected = true;
            } else {
                if (fld.type == "checkbox")
                    fld.checked = false;
                else
                    fld.value = "";
            }
        }
    }

    //Clear The Display grid head labels above the grid.
    CheckGrdisplayHead(dcNo, "", rowNo, "Clear");

    var slNoLbl = "#lblSlNo" + rowNo + "F" + dcNo;
    $j(slNoLbl).text("1");
    SetSerialNoCnt(dcNo, "1");
}

function GetGrpCntValue(dcNo, keyColValue) {
    var hdnId = "hdn" + dcNo + keyColValue;
    var idx = GetFormatRowIndex("gridDc" + dcNo, dcNo, keyColValue, "strow");
    var row = $j('#gridDc' + dcNo + ' tbody>tr:eq(' + idx + ')');
    var hdnValue = "";
    row.find("input").each(function () {
        if ($j(this).attr("id") == hdnId) {
            hdnValue = $j(this).val();
            return false;
        }
    });

    return hdnValue;
}

function SetGrpCntValue(dcNo, keyColValue) {
    var hdnId = "hdn" + dcNo + keyColValue;
    var idx = GetFormatRowIndex("gridDc" + dcNo, dcNo, keyColValue, "strow");
    var row = $j('#gridDc' + dcNo + ' tbody>tr:eq(' + idx + ')');
    var hdnValue = "";
    row.find("input").each(function () {
        if ($j(this).attr("id") == hdnId) {
            $j(this).val("cleared");
            return false;
        }
    });
}

//Function to remove the deleted row fields from the field arrays.
function RemoveDeletedFields(fieldName) {

    for (var i = 0; i < ChangedFields.length; i++) {

        if (ChangedFields[i] == fieldName) {
            ChangedFields.splice(i, 1);
            ChangedFieldDbRowNo.splice(i, 1);
            ChangedFieldValues.splice(i, 1);
        }
    }
}

//Function which deletes the rows from the pop grids for the current parent row.
function DeletePopRowsForDc(dcNo, rowNo) {

    var popGridStr = GetPopGrids(dcNo);
    var popGrids = popGridStr.split(",");

    var parentFlds;
    for (var i = 0; i < popGrids.length; i++) {

        parentFlds = GetParentFields(popGrids[i]);
        if (parentFlds != undefined) {

            var popRowStr = GetPopRows(dcNo, rowNo, popGrids[i]);
            var popRows = popRowStr.split(",");
            popRows.sort();
            for (var j = popRows.length - 1; j >= 0; j--) {
                if (popRows[j] != "") {
                    AddDeletedRowsToArray(popGrids[i], popRows[j]);
                    UpdateDcRowArrays(popGrids[i], popRows[j], "Delete", -1);
                }
            }
            ClearPopRows(dcNo, rowNo, popGrids[i]);
        }
    }
}

//Function to add the deleted row to the deleted rows array.
function AddDeletedRowsToArray(dcNo, rowNo) {

    var axpRecIdFld = "axp_recid" + dcNo + rowNo + "F" + dcNo;
    if ((!axInlineGridEdit && AxpGridForm == "form"))
        axpRecIdFld = "gr" + axpRecIdFld;
    var axpRecIdFld = GetFieldValue(axpRecIdFld);

    if (DeletedRowNos.length > 0) {
        let dcIdx = $j.inArray(parseInt(rowNo), DeletedRowNos);
        if (dcIdx != -1)
            DeletedRowNos[dcIdx] = parseInt(rowNo);
        else
            DeletedRowNos.push(parseInt(rowNo));
    } else
        DeletedRowNos.push(parseInt(rowNo));

    rowNo = GetDbRowNo(rowNo, dcNo);
    var delRowStr = "dc" + dcNo;
    if (axpRecIdFld != "0") {
        if (DeletedDCs.length > 0) {

            var dcIdx = $j.inArray(delRowStr, DeletedDCs);
            if (dcIdx != -1) {
                var newVal = DeletedRows[dcIdx].toString() + "," + axpRecIdFld;
                DeletedRows[dcIdx] = newVal;
            } else {
                DeletedDCs.push(delRowStr);
                DeletedRows.push(axpRecIdFld);
            }
        } else {
            DeletedDCs.push(delRowStr);
            DeletedRows.push(axpRecIdFld);
        }
    }
    var rowFrmNo = rowNo + "F" + dcNo;
    DeletedDCRows.push(rowFrmNo); 
}

//Function to reset the serial numbers in the rows below the deleted row.
function ResetSerialNo(dcNo, rowFrmNo, rowCnt, fromdb) {

    var currentRowNo = rowFrmNo.substring(0, rowFrmNo.indexOf("F"));
    var curSlNoFld = "#lblSlNo" + rowFrmNo;
    var curSlNo = 0;
    var newSlNo = 0;

    if ($j(curSlNoFld).length > 0) {

        curSlNo = $j(curSlNoFld).text();
        newSlNo = parseInt(curSlNo, 10);
    }
    curSlNo = parseInt(curSlNo, 10);
    if (curSlNo == 0) curSlNo = 1;

    currentRowNo = parseInt(currentRowNo, 10);

    for (var i = currentRowNo + 1; i <= rowCnt; i++) {

        var rowNo = GetRowNoHelper(i);
        if (fromdb)
            rowNo = GetClientRowNo(i, dcNo);
        var slNo = "#lblSlNo" + rowNo + "F" + dcNo;
        slNo = $j(slNo);

        if (slNo.length > 0) {
            slNo.text(curSlNo);
            curSlNo += 1;
        }
    }
}

function ResetGridRowsStyle(dcNo) {

    $j("#gridDc" + dcNo + " tr").each(function () {

        if ($j(this).attr("rowindex") % 2 == 0) {
            $j(this).addClass("grey1");
        } else {
            $j(this).removeClass("grey1");
        }
    });
}

function ResetRowStyle(rowNo, dcNo) {

    var slNo = "#lblSlNo" + rowNo + "F" + dcNo;
    var slNoVal = $j(slNo).text();
    slNoVal = parseInt(slNoVal, 10);
    var a = "#sp" + dcNo + "R" + rowNo + "F" + dcNo;

    if ($j(a).attr("rowindex") % 2 == 0) {
        $j(a).addClass("grey1");
    } else {
        $j(a).removeClass("grey1");
    }
}

//Function which calls the SaveData webservice.
function FormSubmit() {
    GetCurrentTime("Tstruct load on Save button click(ws call)");
    if (actionCallFlag == actionCallbackFlag) {
        actionCallFlag = Math.random();
        $("#icons,#btnSaveTst,.BottomToolbarBar a").css({
            "pointer-events": "auto"
        });
    } else {
        //showAlertDialog("error", 2000, "client");
        $("#icons,#btnSaveTst,.BottomToolbarBar a").css({
            "pointer-events": "none"
        });
        return;
    }
    if (currentCK != "") {
        ShowdivContentCK($("#" + currentCK), false);
    }
    var stTime = new Date();
    AxStartTime = GetAxDate(stTime);
    var saveBtn = $j("#imgSaveTst");

    if (saveBtn.prop("disabled") == true || AxIsTstructLocked) {
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
            "pointer-events": "auto"
        });
        return;
    }
    saveBtn.prop("disabled", true);

    EnableSaveBtn(false);

    var doSave = true;

    try {
        doSave = AxBeforeSave();
    } catch (ex) {
        doSave = true;
    }

    if (doSave == undefined)
        doSave = true;

    if (typeof tstructCachedsave != 'undefined' && tstructCachedsave == "T" && (recordid == "" || recordid == "0")) {
        if (typeof callParentNew('signalRNotifications') != 'undefined' && callParentNew('signalRNotifications') == 'true') {
            SaveTransactionJSON();
            return;
        } else {
            showAlertDialog("error", 'ARM is not setup for cached save.');
            return;
        }
    }

    if (doSave) {
        if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
            if (!AxRulesBtnConMsgParser(event.currentTarget)) {
                ShowDimmer(false);
                AxWaitCursor(false);
                actionCallbackFlag = actionCallFlag;
                $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
                    "pointer-events": "auto"
                });
                EnableSaveBtn(true);
                GetProcessTime();
                GetTotalElapsTime();
                return;
            }
        }
        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            if (AxRulesDefComputescript == "true")
                AxRulesDefParser("compute script onsave", "", "computescript");
            if (AxRulesDefValidation == "true") {
                if (!AxRulesDefParser("validate onsave", "", "validate")) {
                    ShowDimmer(false);
                    AxWaitCursor(false);
                    actionCallbackFlag = actionCallFlag;
                    $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
                        "pointer-events": "auto"
                    });
                    EnableSaveBtn(true);
                    GetProcessTime();
                    GetTotalElapsTime();
                    return;
                }
            }
            if (AxRulesDefAllowdup == "true")
                AxRulesDefParser("allowduplicate", "", "allowduplicate");
        } catch (ex) { }
        if (!ValidateBeforeSubmit()) {
            ShowDimmer(false);
            AxWaitCursor(false);
            actionCallbackFlag = actionCallFlag;
            $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
                "pointer-events": "auto"
            });
            EnableSaveBtn(true);
            GetProcessTime();
            GetTotalElapsTime();
            return;
        }
        if (typeof AxpForceValidation != "undefined" && AxpForceValidation != "") {
            if (!ForceFieldValidation()) {
                ShowDimmer(false);
                AxWaitCursor(false);
                actionCallbackFlag = actionCallFlag;
                $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
                    "pointer-events": "auto"
                });
                EnableSaveBtn(true);
                GetProcessTime();
                GetTotalElapsTime();
                return;
            }
        }

        var strpop = parent.window.name;
        FromSave = true;

        var rid = 0;
        if ($j("#recordid000F0").length > 0)
            rid = $j("#recordid000F0").val();

        var files = UploadFiles();
        var delRows = GetDeletedRows();
        var chngRows = GetChangedRows();
        try {
            if (CheckPattern()) {
                var sumDisplay = false;
                try {
                    sumDisplay = AxSummaryDisplay();
                } catch (ex) {

                }
                if (!sumDisplay && AxRulesDefOnSubmit == "true") {
                    CallActionExt("axonsubmit_script", "", "", "", true, true);
                    return;
                }
                GetProcessTime();
                if (recordid != "0" && lastChangedField != "") {
                    focusAfterSaveOnLoad = lastChangedField;
                    lastChangedField = "";
                }
                let axrulesFlds = "";
                if (AxRulesFlds.length > 0) {
                    axrulesFlds = AxRulesFlds.join("♥");
                }
                let AxPegApprovalSave = "";
                if (AxDoPegApprovalSave != "") {
                    AxPegApprovalSave = AxDoPegApprovalSave;
                    AxDoPegApprovalSave = "";
                }
                let _isLoadFromDraft = "false";
                if (typeof isLoadFromDraft != "undefined")
                    _isLoadFromDraft = isLoadFromDraft;
                if (!sumDisplay)
                    ASB.WebService.SaveDataXML(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, DeletedFieldValue, files, rid, delRows, chngRows, tstDataId, axrulesFlds, AxPegApprovalSave, resTstHtmlLS, _isLoadFromDraft, SucceededCallback, OnException);
            } else {
                actionCallbackFlag = actionCallFlag;
                $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
                    "pointer-events": "auto"
                });
                EnableSaveBtn(true);
                AxWaitCursor(false);
                ShowDimmer(false);
                UpdateExceptionMessageInET("SaveData CheckPattern function");
                GetProcessTime();
                GetTotalElapsTime();
            }
        } catch (exp) {
            actionCallbackFlag = actionCallFlag;
            $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
                "pointer-events": "auto"
            });
            ShowDimmer(false);
            AxWaitCursor(false);
            EnableSaveBtn(true);
            var execMess = exp.name + "^♠^" + exp.message;
            showAlertDialog("error", 2030, "client", execMess);
            UpdateExceptionMessageInET("SaveDataWS exception : " + exp.message);
            GetProcessTime();
            GetTotalElapsTime();
        }
    } else {
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper").css({
            "pointer-events": "auto"
        });
        EnableSaveBtn(true);
        AxWaitCursor(false);
        ShowDimmer(false);
        GetProcessTime();
        GetTotalElapsTime();
    }
    var endDate = new Date();
    AxTimeBefSerCall = endDate - stTime;
    // refreshiframe();
}

// Function to create Autorefresh to other iframe
function refreshiframe() {
    let middleframe = callParentNew("middle1", "id")
    let axpframe = callParentNew("axpiframe", "id")
    if (middleframe.src.indexOf("tstruct.aspx") != -1 && axpframe != undefined) {
        axpframe.src = axpframe.src;
    } else if (axpframe.src.indexOf("tstruct.aspx") != -1 && middleframe != undefined) {
        middleframe = middleframe.src;
    }

}


//Function to validate all the fields which cannot be left empty before saving.
function ValidateBeforeSubmit(valDcNo) {

    valDcNo = valDcNo || "";

    for (var i = 0; i < FNames.length; i++) {
        var allowEmpty = GetFieldProp(i, "allowEmpty");
        var visible = GetFieldProp(i, "visible");
        var isPositive = GetFieldProp(i, "onlyPositive");

        dcNo = GetDcNo(FNames[i]);
        if (valDcNo != "" && valDcNo != dcNo) {
            continue;
        }
        if (FNames[i].toString().toLowerCase().indexOf('axpcurrencydec') && currdecVal != "") {
            showAlertDialog("warning", 2031, "client", currdecVal);
            return false;
        }
        if (isMobile) {
            if (FNames[i] == axplatlongFldName) {
                var latLongId = axplatlongFldName + "000F" + GetDcNo(axplatlongFldName);
                if ($("#" + latLongId).val() == "")
                    getlatlongitude();
            }
        }

        let isRuleEnabled = true;
        let rlAllow = AxAllowEmptyCheck(FNames[i]);
        if (rlAllow === 1) {
            isRuleEnabled = true;
        } else if (rlAllow === -1) {
            isRuleEnabled = false;
        } else {
            isRuleEnabled = true;
            return false;
        }

        if (AxRulesDefAllowEmpty == "true") {
            let rlAllow = AxRulesDefParser(FNames[i], "field", "allowempty");
            if (rlAllow === true) {
                isRuleEnabled = false;
            } else if (rlAllow === 1) {
                isRuleEnabled = true;
            } else {
                isRuleEnabled = true;
                return false;
            }
        }

        if ((allowEmpty.toLowerCase() == "f" || isPositive.toLowerCase() == "t") && visible.toLowerCase() == "f" && !isRuleEnabled) {

            var frmno = 0;
            var isGrid = IsGridField(FNames[i]);
            dcNo = GetDcNo(FNames[i]);

            if (isGrid == true) {

                var isPopGrid = false;
                isPopGrid = IsDcPopGrid(dcNo);
                var dcIdx = $j.inArray(dcNo, DCFrameNo);
                //TODO: The allow empty check for sub grid is not being handled. Needs a new feature for handling the same.
                if (!isPopGrid) {
                    if (DCHasDataRows[dcIdx] == "False") {
                        if (DCAllowEmpty[dcIdx] == "False") {
                            var dcCaption = GetDcCaption(dcNo);
                            showAlertDialog("warning", 2054, "client", dcCaption);
                            return false;
                        }
                        continue;
                    }
                }

                var frowno = "row" + dcNo + "temF0";
                var rCount = 0;
                rCount = GetDcRowCount(dcNo);
                var parentFlds = "";
                if (isPopGrid) {
                    parentFlds = GetParentFields(dcNo);
                    if (parentFlds == undefined)
                        parentFlds = new Array();
                }

                //if(!axInlineGridEdit && (AxpGridForm != "form" && !axInlineGridEdit) && rCount == 1){
                // if (!axInlineGridEdit && rCount == 1 && $("#gridHd" + dcNo + " tbody tr").length == 0) {
                //     var dcCaption = GetDcCaption(dcNo);
                //     showAlertDialog("warning", 2054, "client", dcCaption);
                //     return false;
                // }

                for (var k = 1; k < (axInlineGridEdit ? rCount + 1 : rCount + 1); k++) {

                    k = GetRowNoHelper(k);
                    //If the field is in popgrid, check if the sub_parent fields are not empty, if the sub_parent fields are empty dont validate that row values.
                    var isRowEmpty = false;
                    if (isPopGrid) {
                        frmno = dcNo;
                        for (var cnt = 0; cnt < parentFlds.length; cnt++) {

                            var subColumn = GetSubFieldId("sub" + frmno + "_" + parentFlds[cnt], k, frmno);

                            if ($j("#" + subColumn).length > 0) {
                                if (GetFieldValue(subColumn) == "" || GetFieldValue(subColumn) == "***") {
                                    isRowEmpty = true;
                                    break;
                                }
                            }
                        }
                    }

                    //Refer bug -AGI003509
                    if (isRowEmpty || (axInlineGridEdit && $j("#sp" + dcNo + "R" + k + "F" + dcNo).hasClass("editWrapTr")))
                        continue;


                    if (gridDummyRowVal.length > 0) {
                        var isExitDummy = false;
                        gridDummyRowVal.map(function (v) {
                            if (v.split("~")[0] == dcNo && v.split("~")[1] == k)
                                isExitDummy = true;
                        });
                        if (isExitDummy)
                            return isExitDummy;
                    }
                    if (IsAxFldEmpty(i, k, dcNo, allowEmpty, isPositive) == false) {
                        return false;
                    }
                }
            } else {
                if (IsAxFldEmpty(i, "000", dcNo, allowEmpty, isPositive) == false)
                    return false;
            }
        }

        if (FNames[i].toString().toLowerCase().indexOf('validate_') > -1 && FDataType[i].toString().toLowerCase() == "image") {

            if (FNames[i].indexOf('validate_gridattach_') > -1) {
                var controlName = $j("#" + FNames[i] + "001F" + dcNo).val();
                if (controlName.toString().toLowerCase() == "true") {
                    for (var j = 1; j < GetDcRowCount(dcNo) + 1; j++) {
                        var controlToValidate = $j("#axp" + FNames[i].toString().replace('validate', '') + "00" + j + "F" + dcNo);
                        if (controlToValidate == null || controlToValidate == undefined || controlToValidate.val() == "") {
                            showAlertDialog("warning", 2055, "client", FCaption[FNames.indexOf("axp" + FNames[i].toString().replace('validate', ''))]);
                            var frmNo = FldFrameNo[i];
                            SetFocusOnField(dcNo, FNames[i] + "000F" + dcNo);
                            return false;
                        }
                    }
                }
            } else {
                var controlName = $j("#" + FNames[i].toString().replace('validate_', '') + "000F" + dcNo);
                if (controlName[0].src == '' || controlName[0].src == null || (controlName[0].src.indexOf('/AxpImages/upload.png') > -1 || controlName[0].src.indexOf('/AxpImages/signature.png') > -1)) {
                    showAlertDialog("warning", 2055, "client", FCaption[FNames.indexOf(FNames[i].toString().replace('validate_', ''))]);
                    var frmNo = FldFrameNo[i];
                    SetFocusOnField(dcNo, FNames[i] + "000F" + dcNo);
                    return false;
                }
            }

        }
    }
    return true;
}

function ClearFieldsInDC(dcNo) {
    var isGrid = IsDcGrid(dcNo);
    if (isGrid)
        ClearRowsInGrid(dcNo);
    else {
        var fields = GetGridFields(dcNo);
        for (var i = 0; i < fields.length; i++) {
            if (isGrid)
                fld = fields[i] + "001F" + dcNo;
            else
                fld = fields[i] + "000F" + dcNo;
            CallSetFieldValue(fld, "");
            UpdateFieldArray(fld, 1, "", "parent");
        }
    }
}

function IsAxFldEmpty(idx, rowNo, dcNo, allowEmpty, isPositive, calledFrom) {

    var isFldEmpty = false;
    var clRowNo = GetClientRowNo(rowNo, dcNo);
    var fld = $j("#" + FNames[idx] + clRowNo + "F" + dcNo);
    if (fld.length > 0) {
        var fldType = FDataType[idx];
        var fldValue = GetFieldValue(FNames[idx] + clRowNo + "F" + dcNo);

        if (fldType == "Image") {
            if (fldValue == "" && fld.attr("src") == "")
                isFldEmpty = true;
        }
        else if ((fldType == "Numeric" && parseFloat(fldValue) == 0) || fldValue == "") {
            isFldEmpty = true;
        }

        if ($("#" + FNames[idx] + clRowNo + "F" + dcNo).hasClass('autogen') && $("#" + FNames[idx] + clRowNo + "F" + dcNo).val() == "Auto") {
            return true;
        }

        if (!isFldEmpty && fldType == "Numeric" && isPositive.toLowerCase() == "t" && parseInt(fldValue) < 0) {
            showAlertDialog("warning", 2052, "client", FCaption[idx]);
            return false;
        }
        if (allowEmpty.toLowerCase() != "f")
            return true;
        if (isFldEmpty) {
            if (calledFrom == "Edit") {
                rowNo = GetDbRowNo(rowNo, dcNo);
                rowNo = GetRowNoHelper(rowNo);
            }
            //need to call a function which will create a inline error message
            //createInlineErrorInGrid(fld, rowNo, dcNo, idx);
            if (IsDcGrid(dcNo)) {

                try {
                    //$("textarea[id*=2001F2]:not([id^=axp_recid])")
                    if ($("textarea[id*=" + dcNo + clRowNo + "F" + dcNo + "]:not([id^=axp_recid])").length > 0) {
                        let isVRow = false;
                        $("textarea[id*=" + dcNo + clRowNo + "F" + dcNo + "]:not([id^=axp_recid])").each(function () {
                            let vrId = $(this).attr("id");
                            if (vrId.toLowerCase() == "validrow" + dcNo + clRowNo + "f" + dcNo) {
                                let vrVal = $("#" + vrId).val();
                                if (vrVal != "" && vrVal.toLowerCase() == "f") {
                                    isVRow = true;
                                    return false;
                                }
                            }
                        });
                        if (isVRow)
                            return true;
                    }
                } catch (ex) { }

                if (axInlineGridEdit) { //if grid inline edit mode is enabled then make that particular grid row into editmode & focus on the input field
                    var showActiveTabTime = 50;
                    $("[inline-grid-warning]").removeAttr("inline-grid-warning");
                    showAlertDialog("warning", 2056, "client", FCaption[idx] + "^♠^" + parseInt(rowNo, 10));
                    if (!$("#li" + dcNo).hasClass("active")) { //if invalid fld grid is not an active tab then make that tab active & focus on the input field
                        showActiveTabTime = 300;
                        $("#ank" + dcNo).click();
                    }
                    setTimeout(function () {
                        $("#" + FNames[idx] + clRowNo + "F" + dcNo).closest("td").attr("inline-grid-warning", '').click();
                    }, showActiveTabTime);
                } else {
                    if (isMobile && AxpGridForm == "form" && $("#gridHd" + dcNo + " tbody tr").length == 0) {
                        //donothing
                    } else
                        showAlertDialog("warning", 2056, "client", FCaption[idx] + "^♠^" + parseInt(rowNo, 10));
                }
            } else
                showAlertDialog("warning", 2055, "client", FCaption[idx]);
            SetFocusOnField(dcNo, FNames[idx] + rowNo + "F" + dcNo);
            return false;
        }
    }

    return true;
}


function createInlineErrorInGrid(fld, rowNo, dcNo, idx) {
    //GetRowNoHelper
    var cutMsg = eval(callParent('lcm[60]'));
    var errorMessage = FCaption[idx] + " " + cutMsg;
    var errorMessageHtml = '<p id="FEE' + FNames[idx] + GetRowNoHelper(rowNo) + 'F' + dcNo + '" class="IlError text-danger">' + errorMessage + '</p>';
    $("#wrapperForEditFields" + dcNo + " .IlError").remove();
    fld.parents('div.gridElement ').append(errorMessageHtml);
}

//Function to toggle save button disabling.
function EnableSaveBtn(enable) {

    var saveObj = $j("#imgSaveTst,#ftbtn_iSave");
    EnableDisableBtns(saveObj, enable);

    if (enable)
        saveObj.removeProp("disabled");

    var draftsSaveObj = $j("#imgDraftSaveTst");
    EnableDisableBtns(draftsSaveObj, enable);
    var statusBarSave = $j("#dvsubmitCancelBtns #btnSaveTst");
    EnableDisableBtns(statusBarSave, enable);
    var statusBarNew = $j("#dvsubmitCancelBtns #New");
    EnableDisableBtns(statusBarNew, enable);
    var btnDraftSave = $j("#dvsubmitCancelBtns #btnDraftSave");
    EnableDisableBtns(btnDraftSave, enable);
}

//TODO: need to review
//Function to get the attached filenames. 
function UploadFiles() {

    var fup = "";
    for (var k = 0; k < filenamearray.length; k++) {
        if (fup == "")
            fup += filenamearray[k];
        else
            fup += "," + filenamearray[k];
    }
    return fup;
}


//Function to verify the pattern of email,url etc..
function CheckPattern() {

    var fldName = '';
    var patType = '';

    for (var j = 0; j < Patterns.length; j++) {

        patType = Patterns[j];
        // Fields in the non grids are handled here
        var gorng = PatternNames[j].substring(0, 3);
        fldName = PatternNames[j].substring(3);
        var dcNo = GetDcNo(fldName);

        if (gorng == "ng_") {

            fldName = fldName + "000F" + dcNo;

            if (!EvaluatePattern(fldName, patType))
                return false;
        }

        // Fields in the grids are handled here
        else if (gorng == "gr_") {

            var rCount = parseInt(GetDcRowCount(dcNo), 10);
            for (k = 1; k <= rCount; k++) {
                var rno = k + "";
                var rnolength = rno.length;

                if (rnolength == 2)

                    nrno = "0" + rno;

                else if (rnolength == 1)

                    nrno = "00" + rno;

                else if (rnolength == 3)

                    nrno = rno;

                fldName = fldName + nrno + "F" + dcNo;
                if ($j("#" + fldName).length > 0) {
                    if (!EvaluatePattern(fldName, patType))
                        return false;
                }
            }
        }
    }

    try {
        return AxValidSave();
    } catch (ex) {

    }

    return true;
}

var recordidax = '';
//Callback function which returns either successfull or error message on save.
function SucceededCallback(resultJson, eventArgs) {
    if (resultJson != "") {
        if (resultJson.split("♠*♠").length > 1) {
            tstDataId = resultJson.split("♠*♠")[0];
            resultJson = resultJson.split("♠*♠")[1];
        }
        if (resultJson.split("*♠*").length > 1) {
            var serverprocesstime = resultJson.split("*♠*")[0];
            var requestProcess_logtime = resultJson.split("*♠*")[1];
            resultJson = resultJson.split("*♠*")[2];
            WireElapsTime(serverprocesstime, requestProcess_logtime, true);
        } else {
            UpdateExceptionMessageInET("Error : " + resultJson);
        }
    }

    if (typeof resultJson != "undefined" && resultJson == "<error>The operation has timed out</error>") {
        SetFormDirty(false);
        callParentNew("isSessionCleared=", true);
        AxWaitCursor(true);
        ShowDimmer(true);
        if (window.opener)
            GetFormLoadData("AxIsPop=true");
        else
            GetFormLoadData("");
        ShowDialog('error', "Transaction save taking long time than expected.");
        return;
    }

    var stTime = new Date();
    var grdMsg = "";
    AxActiveAction = "Save";
    if (CheckSessionTimeout(resultJson))
        return;
    resTstHtmlLS = "";
    GetFldSetCarryValue();
    resultJson = resultJson.replace(new RegExp("\\n", "g"), "");
    var clrCacheKeys = resultJson.split("*#*")[1];
    if (typeof clrCacheKeys != "undefined" && clrCacheKeys != "") {
        console.log("Clearing cache in save");
        ClearRedisKeys(clrCacheKeys);
    }
    if (resultJson.indexOf("*#*") > -1)
        resultJson = resultJson.substring(0, resultJson.indexOf("*#*"));
    var strResult1 = resultJson.split("♣");
    var hold = false;
    //Checks for axp_savedirective Field for value--> display~fieldName
    fldMsg = GetSaveDirectiveValue();

    if (typeof transid != "" && transid == "ad_af") {
        addfieldSaveSuccess(strResult1);
        return;
    }

    for (var i = 0; i < strResult1.length; i++) {
        strResult = strResult1[i].split("*$*");
        for (var j = 0; j < strResult.length; j++) {
            try {
                var resJson = $j.parseJSON(strResult[j]);
            } catch (ex) {
                ShowDimmer(false);
            }
            var recIdDetails = "";
            var restxt = "";
            var result = "";
            if (resJson != undefined) {
                if (resJson.message != undefined) {
                    for (var k = 0; k < resJson.message.length; k++) {
                        result = resJson.message[0].msg;
                        if (result.indexOf(",") != -1) {
                            var strMsg = result.split(",");
                            //result = strMsg[0];
                            //recIdDetails = strMsg[1];
                            recIdDetails = strMsg.pop(strMsg.length);
                            result = strMsg.join(",");
                        }
                    }
                } else if (resJson.error != undefined) {
                    restxt = ErrStr;
                    result = resJson.error[0].msg;
                    UpdateExceptionMessageInET("SaveData Error : " + result);
                    GetProcessTime();
                    GetTotalElapsTime();
                } else if (resJson.timetaken != undefined) {
                    ExecTimetaken(resJson.timetaken);
                } else if (resJson.GridAttachments != undefined) {
                    grdMsg = resJson.GridAttachments[0].Message;

                } else if (resJson.command != undefined) {
                    ExecCommand(resJson.command, 'autoprints', '');
                }
                if (result == "Dataloss") {
                    ShowDialog('error', 2001, "client");
                    EnableSaveBtn(true);
                    AxWaitCursor(false);
                    ShowDimmer(false);
                    return;
                }

                if (result.indexOf(";bkslh") != -1) {
                    result = result.replace(new RegExp(";bkslh", "g"), "\\");
                }
                if (result != "") {
                    if (grdMsg != "") {
                        result = result + " :" + grdMsg;
                    }
                    //Appends display~fieldName - value to the save success message
                    if (fldMsg != undefined && fldMsg != "") {
                        if (result.indexOf("(") > -1) { //Have autogenerate Field
                            result = result.substring(0, result.indexOf("("));
                        }
                        result = result + " , (" + fldMsg + ") ";
                    }
                    ChangedFields = new Array();
                    ChangedFieldDbRowNo = new Array();
                    ChangedFieldValues = new Array();
                    DeletedDCRows = new Array();
                    DeletedFieldValue = new Array();                    


                    if (restxt != ErrStr) {
                        window.status = eval(callParent('lcm[76]'));
                        AxRulesFlds = new Array();
                        for (var n = 0; n < filenamearray.length; n++) {
                            fileonloadarray.push(filenamearray[n]);
                            RemoveArrVal(filenamearray[n], filenamearray);
                        }
                        ConstructAttachments();
                        recordidax = recIdDetails.substring(recIdDetails.indexOf("=") + 1);
                        axSaveRecId = recIdDetails.substring(recIdDetails.indexOf("=") + 1);

                        var recidIdx = result.toString().lastIndexOf("~");
                        if (recidIdx != -1) {
                            //if its opening in dynamic widget in pagebuilder then it need to reload the widget from where it called 
                            const presentFrame = $(window.frameElement);
                            if (presentFrame.attr("id") === "homePageDynamicFrame") {
                                if (parent.reloadTheWidget)
                                    parent.reloadTheWidget({
                                        widgetTarget: presentFrame.data("calledElemntId")
                                    });
                            } else if (presentFrame.attr("id") === "axpiframe") {
                                presentFrame.removeClass("frameSplited");
                                ReloadMiddleIframe();
                            }

                            //TODO: assuming that the status message will be parsed here
                            ShowDialog('success', result.toString().substring(0, recidIdx), undefined, undefined, undefined, undefined, undefined, 'true');
                        } else {
                            AxProcessObjSaveRedirect(strResult);
                            try {
                                AxCustomSaveRedirect();
                            } catch (ex) {
                                const presentFrame = $(window.frameElement);
                                if (presentFrame.attr("id") === "homePageDynamicFrame") {
                                    if (parent.reloadTheWidget)
                                        parent.reloadTheWidget({
                                            widgetTarget: presentFrame.data("calledElemntId")
                                        });
                                } else if (presentFrame.attr("id") === "axpiframe") {
                                    presentFrame.removeClass("frameSplited");
                                    ReloadMiddleIframe();
                                }
                                //Refer Bug: AXP000134
                                var src = $(eval(callParent("loadPopUpPage"))).attr("src");
                                if (src != undefined && src.indexOf("AxPop=true") != -1)
                                    eval(callParent('isRefreshParentOnClose') + "= true");
                                //End AXP000134
                                ShowDialog('success', result, undefined, undefined, undefined, undefined, undefined, 'true');
                                eval(callParent('isSuccessAlertInPopUp') + "= true");
                                SetFormDirty(false);
                            }

                        }

                        if (AxIsTstructCached && recIdDetails != "") {
                            var recId = recIdDetails.substring(recIdDetails.indexOf("=") + 1);
                            SaveInCache(recId, "Save");
                        }

                        if (IsDraftLoad) {
                            if (window.parent.draftsWin != false && window.parent.draftsWin != undefined) {
                                try {
                                    window.parent.draftsWin.$j("#btnGetDrafts").click();
                                } catch (ex) {}
                            }
                        }

                    } else {
                        if (restxt == ErrStr) {
                            var errFld = "";
                            var errMsg = result;
                            var index = errMsg.indexOf("^^dq");
                            while (index != -1) {
                                errMsg = errMsg.replace("^^dq", '"');
                                index = errMsg.indexOf("^^dq");
                            }

                            if (errMsg != null && errMsg != undefined && errMsg != "") {
                                try {
                                    if (errMsg.indexOf("errfld") > -1) {
                                        let errfldInfo = errMsg.substring(errMsg.lastIndexOf("errfld"));
                                        errfldInfo = errfldInfo.split(":")[1];
                                        errFld = errfldInfo.replace("\"", "").trim();
                                        errMsg = errMsg.substring(0, errMsg.lastIndexOf("errfld") - 2);
                                        errMsg = errMsg.replace("\",", "").replace("\" ,", "");

                                        if (errFld != "") {
                                            let _fldDetails = errFld.toString().split(",");
                                            let _focusFldId = _fldDetails[0];
                                            let _indx = $j.inArray(_focusFldId, FCaption);
                                            if (typeof _indx != "undefined" && _indx > -1) {
                                                let _fldCap = FCaption[_indx];
                                                let _fscCap = "";
                                                let _fldName = FNames[_indx];
                                                if (_fldName != "" && AxSetFldCaption.length > 0) {
                                                    let _thisEle = AxSetFldCaption.filter(x => x.indexOf('setfieldcaption~' + _fldName + '^') > -1);
                                                    if (_thisEle.length > 0) {
                                                        _fscCap = _thisEle[0].split('^')[1];
                                                    }
                                                }
                                                if (_fscCap != "" && errMsg.startsWith(_fldCap)) {
                                                    errMsg = errMsg.replace(_fldCap, _fscCap);
                                                }
                                            }
                                        }
                                    }
                                    ShowDialog('error', errMsg);

                                    var focusFldId = "";
                                    if (errFld != "") {
                                        var fldDetails = errFld.toString().split(",");
                                        focusFldId = fldDetails[0];
                                        let focusFldRowNo = fldDetails[1];
                                        var dcNo = GetDcNo(focusFldId)
                                        var rowNo = "000";
                                        if (IsGridField(focusFldId)) {
                                            if (focusFldRowNo.length == 3)
                                                rowNo = focusFldRowNo;
                                            else if (focusFldRowNo.length == 2)
                                                rowNo = "0" + focusFldRowNo;
                                            else if (focusFldRowNo.length == 1)
                                                rowNo = "00" + focusFldRowNo;
                                            var fldName = focusFldId + rowNo + "F" + dcNo;
                                            var focusFld = $j("#" + fldName);
                                            if (focusFld.length > 0) {
                                                if(IsTabDc(dcNo))
                                                {
                                                    if(!$("#ank"+dcNo).hasClass('active'))
                                                    {
                                                       let ankId= $("#ank"+dcNo).parents("ul#myTab").find("a.active").attr("id");
                                                       $("#"+ankId).removeClass("active");
                                                       let ackTabId=ankId.substr(3);
                                                       $("#tab-"+ackTabId).removeClass('show active');
                                                       $("#tab-"+dcNo).addClass('show active');
                                                       $("#ank"+dcNo).addClass('active');
                                                    }
                                                }
                                                let focusFldIndex = focusFld.parents("td").index();
                                                if ($("#gridHd" + dcNo + " tbody tr#sp" + dcNo + "R" + rowNo + "F" + dcNo).hasClass('inline-edit'))
                                                    focusFld.focus();
                                                else
                                                    $("#gridHd" + dcNo + " tbody tr#sp" + dcNo + "R" + rowNo + "F" + dcNo + " td:eq(" + focusFldIndex + ")").click();
                                            }
                                        } else {
                                            var fldName = focusFldId + rowNo + "F" + dcNo;
                                            var focusFld = $j("#" + fldName);
                                            if (focusFld.length > 0) {
                                                focusFld.focus();
                                            }
                                        }
                                    }
                                } catch (ex) { }
                            } else {
                                ShowDialog('error', errMsg);
                            }

                            EnableSaveBtn(true);
                            AxWaitCursor(false);
                            ShowDimmer(false);

                            try {
                                AxAfterSaveOnError();
                            } catch (ex) {}
                            return;
                        }
                    }
                }
            }
        }
    }
    if (AxLogTimeTaken == "true") {
        var edTime = new Date();
        var diff = edTime.getTime() - stTime.getTime();
        CreateTimeLog(AxStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "SaveData");
    }
    ShowDimmer(false);
    AxWaitCursor(false);
}

function addfieldSaveSuccess(strResult1) {
    try {
        var result = "";
        for (var i = 0; i < strResult1.length; i++) {
            strResult = strResult1[i].split("*$*");
            for (var j = 0; j < strResult.length; j++) {
                try {
                    var resJson = $j.parseJSON(strResult[j]);
                } catch (ex) {
                    ShowDimmer(false);
                }
                var recIdDetails = "";
                var restxt = "";
                if (resJson != undefined) {
                    if (resJson.error != undefined) {
                        result = resJson.error[0].msg;
                    }
                }
            }
        }
        if (typeof addFieldReloaduri != "undefined" && addFieldReloaduri != "") {
            let _adfhref = addFieldReloaduri;
            addFieldReloaduri = "";
            _adfhref = _adfhref.replace("&newform=T", "&newform=F");
            window.location.href = _adfhref;
        } else {
            let _adfhref = window.location.href;
            _adfhref = _adfhref.replace("&newform=T", "&newform=F");
            window.location.href = _adfhref;
        }
        if (result == "") {
            if (typeof recordid != "undefined" && recordid != "" && recordid != "0")
                showAlertDialog("success", "Field Updated successfully.");
            else
                showAlertDialog("success", "Field Created successfully.");
        }
        else
            showAlertDialog("error", result);
    } catch (ex) { }
}

function GetSaveDirectiveValue() {
    var fldMsg = "";
    var dispfld, fldName, saveDirFld, fld, fldVal;
    saveDirFld = $j("#axp_savedirective000F1").val();
    if (saveDirFld != undefined && saveDirFld.indexOf("display") > -1) {
        dispfld = saveDirFld.split("~");
        if (dispfld.length > 1) {
            fldName = dispfld[1];
            fld = GetActualFieldName(fldName);
            fldVal = GetFieldValue(fld);
            fldMsg = fldName + " : " + fldVal;
        }
    }
    return fldMsg;
}
//Toolbar button functions
//-------------------------------------------------------------------------------------------

//Function which opens the pdf window.
function OpenPdfDocList() {
    recID = parseInt(recordid);
    if (recID != "0") {
        if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
            if (!AxRulesBtnConMsgParser(event.currentTarget)) {
                return;
            }
        } 
        var lengthofDDL = $j('#pdfFName').children('option').length;
        if (lengthofDDL == 0)
            showAlertDialog("error", 2003, "client");
        else if (lengthofDDL > 1)
            displaypdfdiv("dvPDFDocList", "PDF");
        else
            CallPDFws();
    } else {
        showAlertDialog("error", 2002, "client");
    }
    return;
}

//Function which opens the search window.
function OpenSearch(tid) {
    if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
        if (!AxRulesBtnConMsgParser(event.currentTarget)) {
            return;
        }
    }
    $j("#searchoverrelay").parent().removeClass("d-none");
    return;

    var left = (screen.width / 2) - (810 / 2);
    var top = (screen.height / 2) - (400 / 2);

    if (newWin != null) {
        if (!newWin.closed) {
            newWin.focus();
        } else {
            try {
                newWin = window.open('tstsearch_gv.aspx?transid=' + tid, 'MyPopUp' + tid, 'width=810,height=400,resizable=yes,top=' + top + ',left=' + left + '');
                newWin.focus();
            } catch (ex) {
                showAlertDialog("warning", eval(callParent('lcm[356]')));
            }
        }
    }

    if (newWin == null) {
        if (parent.parent.document.clwindow != null) {

            if (parent.parent.document.clwindow.clwin.value == "") {
                parent.parent.document.clwindow.clwin.value = 'MyPopUp' + tid;
            } else {
                parent.parent.document.clwindow.clwin.value = parent.parent.document.clwindow.clwin.value + ',' + 'MyPopUp' + tid;
            }
        }
        try {
            newWin = window.open('tstsearch_gv.aspx?transid=' + tid, 'MyPopUp' + tid, 'width=810,height=400,resizable=yes,top=' + top + ',left=' + left + '');
            newWin.focus();
        } catch (ex) {
            showAlertDialog("warning", eval(callParent('lcm[356]')));
        }
    }
}


//Function which opens a new tstsruct.
function NewTstruct() {
    //For new tstruct,home and navigation buttons will not be available
    if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
        if (!AxRulesBtnConMsgParser(event.currentTarget)) {
            return;
        }
    }
    GetCurrentTime("Tstruct load on New button click(ajax call)");
    if (!window.opener)
        window.parent.disableNavigation = true;
    else
        window.opener.parent.disableNavigation = true;
    $(".lnkPrev").addClass("hide");
    $(".lnkNext").addClass("hide");
    if (window.parent.globalChange) {
        var cutMsg = eval(callParent('lcm[31]'));
        var glType = eval(callParent('gllangType'));
        var isRTL = false;
        if (glType == "ar")
            isRTL = true;
        else
            isRTL = false;

        var NewTstructCB = $.confirm({
            theme: 'modern',
            closeIcon: false,
            rtl: isRTL,
            title: eval(callParent('lcm[155]')),
            onContentReady: function () {
                disableBackDrop('bind');
            },
            backgroundDismiss: true,
            escapeKey: 'buttonB',
            content: cutMsg,
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[164]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        GetCurrentTime("Tstruct load on New button click(ajax call)");
                        NewTstructCB.close();
                        if (transid == "axpwf")
                            window.location.href = window.location.href;
                        else
                            callParentNew("loadFrame()", "function");
                        var rid = $j("#recordid000F0").val();
                        if (rid != "0" && AxIsTstructLocked && callParentNew("isLockOnRead")) {
                            if (window.opener)
                                window.location.href = "tstruct.aspx?transid=" + tst + "&AxIsPop=true";
                            else
                                window.location.href = "tstruct.aspx?transid=" + tst;
                        } else {
                            SetFormDirty(false);
                            callParentNew("isSessionCleared=", true);
                            AxWaitCursor(true);
                            ShowDimmer(true);
                            if (window.opener)
                                GetFormLoadData("AxIsPop=true");
                            else
                                GetFormLoadData("");
                        }
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[192]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        ShowDimmer(false);
                        closeParentFrame();
                        disableBackDrop('destroy');
                        callParentNew("isSessionCleared=", true);
                    }
                }
            }
        });
    } else {

        var rid = $j("#recordid000F0").val();
        if (rid != "0" && AxIsTstructLocked && callParentNew("isLockOnRead")) {
            if (window.opener)
                window.location.href = "tstruct.aspx?transid=" + tst + "&AxIsPop=true";
            else
                window.location.href = "tstruct.aspx?transid=" + tst;
        } else {
            if (transid == "axpwf")
                window.location.href = window.location.href;
            else
                callParentNew("loadFrame()", "function");
            callParentNew("isSessionCleared=", true);
            SetFormDirty(false);
            AxWaitCursor(true);
            ShowDimmer(true);
            if (window.opener)
                GetFormLoadData("AxIsPop=true");
            else
                GetFormLoadData("");
        }
    }
}

//TO Get DoFormLoad data from client side with the result & tstruct HTML.
function GetFormLoadData(tstQureystr, isDraft, forceRefresh = "false",isdirectCall="false") {
    if (typeof isDraft == "undefined")
        isDraft = "false";
    checkIsdraft = isDraft.startsWith('true♦') ? 'true' : isDraft;
    try {
        AxWaitCursor(true);
        ShowDimmer(true);
        let _isDupTab = callParentNew('isDuplicateTab');
        if ((window.location.href).indexOf("AxSplit") != -1) {
            tstQureystr += tstQureystr.indexOf("AxSplit") == -1 ? `${tstQureystr.endsWith("♠") ? "": "♠"}AxSplit=true` : "";
        }
        if (typeof isLoadFromDraft != "undefined")
            isLoadFromDraft = "false";
        if (isdirectCall == "false" && forceRefresh == "false") {
            if (typeof wsPerfFormLoadCall != "undefined" && wsPerfFormLoadCall) {
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
                let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "formload" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid);
                if (_formLoadFlag.length > 0)
                    isdirectCall = "true";
                else if (callParentNew('axTraceFlag') == true) {
                    isdirectCall = "true";
                }
            } else if (callParentNew('axTraceFlag') == true) {
                isdirectCall = "true";
            }
        }
        if (isdirectCall == "false" && forceRefresh == "false") {
            if ($("#hdnisHtmlLsLoad").val() == "*loaddata*") {
                //window.location.href = "tstruct.aspx?transid=" + transid + "&dummyload=false♠forcecall";
                window.location.href = "tstruct.aspx?transid=" + transid + "&dummyload=false♠";
                return;
            }
            var _TstlocalStorage = "";
            try {
                if (typeof (Storage) !== "undefined") {
                    let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                    //_TstlocalStorage = localStorage["tstHtml♠" + transid + "-" + appSUrl];
                    let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + transid + "-" + appSUrl + "♥)", "function");
                    _TstlocalStorage = localStorage[_thisKey[0]];
                    if (typeof _TstlocalStorage == "undefined") {
                        _TstlocalStorage = "";
                    }
                }
            } catch (e) {
            }

            if (_TstlocalStorage != "") {
                let _thisHtmlls = _TstlocalStorage.split('♠♠♠')[1];
                GetTstHtmlLS(_thisHtmlls);
                return;
            }
        }
       let isFromDummytrue = isLocalStorageHtml();
        GetProcessTime();
        $.ajax({
            url: 'tstruct.aspx/GetFormLoadValues',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                key: tstDataId,
                tstQureystr: tstQureystr,
                isDraft: isDraft,
                forceRefresh: forceRefresh,
                isDupTab: _isDupTab,
                isTstHtmlLs: resTstHtmlLS,
                isFromDummytrue: isFromDummytrue
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                AxWaitCursor(true);
                ShowDimmer(true);
                resTstHtmlLS = '';
                var result = data.d;
                if (result != "") {
                    if (result.split("*♠♦*").length > 1) {
                        serverprocesstime = result.split("*♠♦*")[1];
                        requestProcess_logtime = result.split("*♠♦*")[2];
                        result = result.split("*♠♦*")[0];
                        WireElapsTime(serverprocesstime, requestProcess_logtime, true);
                    } else {
                        UpdateExceptionMessageInET("Error : " + result);
                    }
                }
                Closediv();
                if (result.toLowerCase().indexOf("access violation") === -1) {
                    ArrActionLog = "";
                    if (CheckSessionTimeout(result))
                        return;
                    actionCallbackFlag = actionCallFlag;
                    $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                        "pointer-events": "auto"
                    });
                    if (FromSave) {
                        FromSave = false;
                    }
                    EnableSaveBtn(true);
                    isReadyCK = false;
                    DCFrameNo.forEach(function (dcID) {
                        ClearFieldsInDC(dcID);
                        try {
                            let _isGrid = IsDcGrid(dcID);
                            if (_isGrid)
                                ClearRowsInGrid(dcID, "htmlload");
                        } catch (ex) { }
                    });
                    ExprPosArray.forEach(function (vals, ind) {
                        if (vals != "")
                            ExprPosArray[ind] = "";
                    });
                    tstReadOnlyPeg = false;
                    navValidator = true;
                    SetFormDirty(false);
                    appstatus = "";
                    blurNextPreventId = "";
                    document.title = "Tstruct";
                    recordid = "0";
                    $j("#recordid000F0").val("0");
                    AxFormControlList = new Array();
                    AxSetFldCaption = new Array();
                    RegVarFldList = new Array();
                    ChangedFields = new Array();
                    ChangedFieldDbRowNo = new Array();
                    ChangedFieldValues = new Array();
                    DeletedDCRows = new Array();
                    ChangedDcRows = new Array();
                    ChangedDcs = new Array();
                    AllFieldNames = new Array();
                    AllFieldValues = new Array();
                    ScriptMaskFields = new Array();
                    AxExecFormControl = false;
                    DisabledDcs = new Array();
                    AxFormContHiddenFlds = new Array();
                    AxFormContSetCapFlds = new Array();
                    AxFormContSetCapFldsGrid = new Array();
                    AxFormContSetFldActGrid = new Array();
                    AxFormContSetGridCell = new Array();
                    AxFormContFldSetFocus = new Array();
                    multiSelectflds = new Array();
                    multiSelFldParents = new Array();
                    multiSelFldResult = new Array();
                    multiSelectLoadVals = new Array();
                    FldListParents = new Array();
                    FldListData = new Array();
                    FldDListPFNames = new Array();
                    FldDListPFNameVals = new Array();
                    AxRulesFlds = new Array();
                    imgNames = new Array();
                    imgSrc = new Array();
                    changeFillGridDc = 0;
                    if (isMobile)
                        OnMobileNewTst();

                    if (result != "") {
                        var resval = result.split("*$*");
                        if (resval[0] == "") {
                            resval = GetTstHtmlNodeLs(resval);
                            if (resval[0] == "") {
                                showAlertDialog("error", "Clear In-Memory and retry again.");
                                return;
                            }
                            //window.location.reload();
                            //return;
                        }
                        $("#tblWrk").html("");
                        $("#collapseOneTable").html("");
                        $j(".workflow").remove();
                        //$j("#selectbox").empty();
                        //$j("#selectbox").removeAttr("disabled");
                        $j(".wrkflwinline").remove();
                        $j(".workflowMsg").remove();
                        $j(".downArr").remove();
                        $j(".Selectboxlist").remove();
                        //$("#selectbox").remove();
                        $j("#workflowoverlay").addClass("d-none");
                        $j("#dvMessage").html("");
                        $j("#dvMessage").removeClass("AXinfo").addClass("success d-none");
                        $j("#file").addClass("d-none");
                        $j("#attachment-overlay").html("");
                        $j("#attachfname").val("");
                        attachments = "";
                        filenamearray = new Array();
                        fileonloadarray = new Array();
                        $("#hdnTabHtml").val(resval[3]);
                        var tstPerfVars = resval[4];
                        if (tstPerfVars != "") {
                            wsPerfFormLoadCall = tstPerfVars.split(";")[0] == "true" ? true : false;
                            wsPerfEvalExpClient = tstPerfVars.split(";")[1].split(",");
                            headerAttachDir = tstPerfVars.split(";")[2];
                            try {
                                if (tstPerfVars.split(";").length > 3) {
                                    formParamFlds = tstPerfVars.split(";")[3].split(",");
                                    formParamVals = tstPerfVars.split(";")[4].split(",");
                                } else {
                                    formParamFlds = new Array();
                                    formParamVals = new Array();
                                }
                            } catch (ex) { }
                        }
                        var ImgVals = resval[5];
                        if (ImgVals != "") {
                            ImgVals = ImgVals.split("♠");
                            for (i = 0; i < ImgVals.length; i++) {
                                imgNames[i] = ImgVals[i].split("♦")[0];
                                imgSrc[i] = ImgVals[i].split("♦")[1];
                                if (imgSrc[i] != "")
                                    imgSrc[i] = imgSrc[i].replace("%20", " ");
                            }
                        }

                        if (resval[6] != "") {
                            var dcStatus = resval[6].split(',');
                            dcStatus.forEach(function (dcStat, indx) {
                                TabDCStatus[indx] = dcStat;
                            })
                        }
                        displayAutoGenVal = resval[7];
                        $j("#hdnShowAutoGenFldValue").val(displayAutoGenVal);
                        tstructCancelled = resval[8];
                        tstDataId = resval[11];

                        try {
                            FFieldHidden = new Array();
                            FFieldReadOnly = new Array();

                            if (resval[9] != "") {
                                var _thisFFH = resval[9].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldHidden.push(_val);
                                })
                            }
                            if (resval[10] != "") {
                                var _thisFFH = resval[10].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldReadOnly.push(_val);
                                })
                            }
                        } catch (ex) { }

                        $j("#hdnDataObjId").val(tstDataId);                      
                        LoadResult = result;
                        isLoadDataCall = false;
                        if (typeof isDraft != "undefined" && isDraft.startsWith('true♦')) {
                            let _thisDraftkey = isDraft.split('♦')[1];
                            _thisDraftkey = _thisDraftkey.substring(_thisDraftkey.indexOf("-") + 1);
                            isLoadFromDraft = _thisDraftkey;
                        } else
                            isLoadFromDraft = "false";
                        ReloadJqueryReference();
                        //if (typeof isWizardTstruct == "undefined" || isWizardTstruct == false) {
                        isTstPostBackVal = resval[0] + "*$*" + resval[1] + "*$*" + resval[2];
                        //}
                    }
                } else {
                    blurNextPreventId = "";
                    actionCallbackFlag = actionCallFlag;
                    $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                        "pointer-events": "auto"
                    });
                    if (FromSave) {
                        FromSave = false;
                    }
                    EnableSaveBtn(true);
                    AxWaitCursor(false);
                    ShowDimmer(false);
                    SetFormDirty(false);
                    $("#reloaddiv").show();
                    $("#dvlayout").hide();
                }
            },
            error: function (error) {
                Closediv();
                blurNextPreventId = "";
                actionCallbackFlag = actionCallFlag;
                $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                    "pointer-events": "auto"
                });
                if (FromSave) {
                    FromSave = false;
                }
                EnableSaveBtn(true);
                AxWaitCursor(false);
                ShowDimmer(false);
                SetFormDirty(false);
                $("#reloaddiv").show();
                $("#dvlayout").hide();
            }
        });
    } catch (exp) {
        Closediv();
        blurNextPreventId = "";
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
            "pointer-events": "auto"
        });
        if (FromSave) {
            FromSave = false;
        }
        EnableSaveBtn(true);
        AxWaitCursor(false);
        ShowDimmer(false);
        SetFormDirty(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
}

function LoadTstHtmlLS(_thisTransid, _src) {
    var _TstlocalStorage = "";
    try {
        /*window.frameElement.contentWindow.jQuery('#form1').attr('action', './tstruct.aspx?' + window.location.href.split('?')[1]);*/
        window.frameElement.contentWindow.jQuery('#form1').attr('action', _src);
        if (typeof (Storage) !== "undefined") {
            let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + _thisTransid + "-" + appSUrl + "♥)", "function");
            //_TstlocalStorage = localStorage["tstHtml♠" + _thisTransid + "-" + appSUrl];
            _TstlocalStorage = localStorage[_thisKey[0]];
            if (typeof _TstlocalStorage == "undefined") {
                _TstlocalStorage = "";
            }
        }
    } catch (e) { }
    if (_TstlocalStorage != "") {
        /*$('.menu.menu-sub.menu-sub-dropdown').removeClass('initialized');*/
        $('#dvlayout .menu.menu-sub.menu-sub-dropdown').removeClass('initialized');
        KTMenu?.init();
        let _thisHtmlls = _TstlocalStorage.split('♠♠♠')[1];
        var resval = _thisHtmlls.split("♦♠♣♥");
        if (typeof resval[1] != "undefined" && resval[1] != "") {
            $('.subres').html('');
            $('.subres').html(resval[1]);
        }
        if (typeof resTstLoadDummy != "undefined" && resTstLoadDummy != "")
            GetTstHtmlLS(resval[0]);
        else {
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
            if (typeof isHtmlLsLoadData != "undefined" && isHtmlLsLoadData != "") {
                $("#hdnisHtmlLsLoad").val(isHtmlLsLoadData);
                isHtmlLsLoadData = "";
            }
            let _formLoadFlag = _finalJson.filter(item => item.props.toLowerCase() === "formload" && item.stype.toLowerCase() == 'tstruct' && item.structname == transid);
            if (_formLoadFlag.length > 0 || $("#hdnisHtmlLsLoad").val() == "*loaddata*") {
                resTstHtmlLS = transid;
                GetFormLoadData("");
            }
            else
                GetTstHtmlLS(resval[0]);
        }
    }
    //ShowDimmer(false);
}

function GetTstHtmlLS(_TstlocalStorage) {
    //Closediv();
    pageLogTime = '';
    formLogTime = '';
    resTstHtmlLS = "";
    if (typeof resTstLoadDummy != "undefined" && resTstLoadDummy != "") {
        var resResult = resTstLoadDummy;
        resTstLoadDummy = '';
        if (resResult != "") {
            if (resResult.split("*♠♦*").length > 1) {
                serverprocesstime = resResult.split("*♠♦*")[1];
                requestProcess_logtime = resResult.split("*♠♦*")[2];
                resResult = resResult.split("*♠♦*")[0];
                WireElapsTime(serverprocesstime, requestProcess_logtime, true);
            } else {
                UpdateExceptionMessageInET("Error : " + resResult);
            }
        }
        Closediv();
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
            "pointer-events": "auto"
        });
        if (FromSave) {
            FromSave = false;
        }
        EnableSaveBtn(true);
        isReadyCK = false;
        DCFrameNo.forEach(function (dcID) {
            //ClearFieldsInDC(dcID);
            let _isGrid = IsDcGrid(dcID);
            if (_isGrid)
                ClearRowsInGrid(dcID, "htmlload");
            else
                ClearFieldsInDC(dcID);
        });
        ExprPosArray.forEach(function (vals, ind) {
            if (vals != "")
                ExprPosArray[ind] = "";
        });
        tstReadOnlyPeg = false;
        navValidator = true;
        SetFormDirty(false);
        blurNextPreventId = "";
        AxFormControlList = new Array();
        AxSetFldCaption = new Array();
        RegVarFldList = new Array();
        ChangedFields = new Array();
        ChangedFieldDbRowNo = new Array();
        ChangedFieldValues = new Array();
        ChangedDcRows = new Array();
        ChangedDcs = new Array();
        DeletedDCRows = new Array();
        AllFieldNames = new Array();
        AllFieldValues = new Array();
        ScriptMaskFields = new Array();
        AxExecFormControl = false;
        DisabledDcs = new Array();
        AxFormContHiddenFlds = new Array();
        AxFormContSetCapFlds = new Array();
        AxFormContSetCapFldsGrid = new Array();
        AxFormContSetFldActGrid = new Array();
        AxFormContSetGridCell = new Array();
        AxFormContFldSetFocus = new Array();
        multiSelectflds = new Array();
        multiSelFldParents = new Array();
        multiSelFldResult = new Array();
        multiSelectLoadVals = new Array();
        AxRulesFlds = new Array();
        FldListParents = new Array();
        FldListData = new Array();
        FldDListPFNames = new Array();
        FldDListPFNameVals = new Array();
        changeFillGridDc = 0;
        imgNames = new Array();
        imgSrc = new Array();

        if (isMobile)
            OnMobileNewTst();

        document.title = "Tstruct";
        var resval = resResult.split("*$*");
        if (resval[0] == "") {
            resval = GetTstHtmlNodeLs(resval);
            if (resval[0] == "") {
                showAlertDialog("error", "Clear In-Memory and retry again.");
                return;
            }

            //if (_TstlocalStorage != "" && _TstlocalStorage.toLowerCase().indexOf("access violation") === -1)
            //    resval = _TstlocalStorage.split("*$*");
            //else
            //    return;
        }
        $j(".workflow").remove();
        $(".wfselectbox").html("<div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"bottom-end\" id=\"selectbox\"></div>");
        $j(".wrkflwinline").remove();
        $j(".workflowMsg").remove();
        $j(".downArr").remove();
        $j(".Selectboxlist").remove();
        $j("#workflowoverlay").addClass("d-none");
        $j("#dvMessage").html("");
        $j("#dvMessage").removeClass("AXinfo").addClass("success d-none");
        $j("#file").addClass("d-none");
        $j("#attachment-overlay").html("");
        $j("#attachfname").val("");
        $j("#PEGDiv").addClass("d-none");
        $j("#AxAmendDiv").addClass("d-none");
        attachments = "";
        filenamearray = new Array();
        fileonloadarray = new Array();
        $("#hdnTabHtml").val(resval[3]);
        var tstPerfVars = resval[4];
        if (tstPerfVars != "") {
            wsPerfFormLoadCall = tstPerfVars.split(";")[0] == "true" ? true : false;
            wsPerfEvalExpClient = tstPerfVars.split(";")[1].split(",");
            headerAttachDir = tstPerfVars.split(";")[2];
            try {
                if (tstPerfVars.split(";").length > 3) {
                    formParamFlds = tstPerfVars.split(";")[3].split(",");
                    formParamVals = tstPerfVars.split(";")[4].split(",");
                } else {
                    formParamFlds = new Array();
                    formParamVals = new Array();
                }
            } catch (ex) { }
        }
        var ImgVals = resval[5];
        if (ImgVals != "") {
            ImgVals = ImgVals.split("♠");
            for (i = 0; i < ImgVals.length; i++) {
                imgNames[i] = ImgVals[i].split("♦")[0];
                imgSrc[i] = ImgVals[i].split("♦")[1];
                if (imgSrc[i] != "")
                    imgSrc[i] = imgSrc[i].replace("%20", " ");
            }
        }
        if (resval[6] != "") {
            var dcStatus = resval[6].split(',');
            dcStatus.forEach(function (dcStat, indx) {
                TabDCStatus[indx] = dcStat;
            })
        }
        tstructCancelled = resval[7];
        if (typeof AxGridAttNotExistList != "undefined" && resval[8] != "") {
            var agattnotfiles = resval[8].split(',');
            AxGridAttNotExistList = agattnotfiles;
        } else if (resval[8] != "") {
            var agattnotfiles = resval[8].split(',');
            AxGridAttNotExistList = agattnotfiles;
        } else
            AxGridAttNotExistList = "";
        tstDataId = resval[11];
        displayAutoGenVal = "true";
        $j("#hdnShowAutoGenFldValue").val(displayAutoGenVal);
        try {
            FFieldHidden = new Array();
            FFieldReadOnly = new Array();

            if (resval[9] != "") {
                var _thisFFH = resval[9].split(',');
                _thisFFH.forEach(function (_val) {
                    if (_val != "")
                        FFieldHidden.push(_val);
                })
            }
            if (resval[10] != "") {
                var _thisFFH = resval[10].split(',');
                _thisFFH.forEach(function (_val) {
                    if (_val != "")
                        FFieldReadOnly.push(_val);
                })
            }
        } catch (ex) { }

        $j("#hdnDataObjId").val(tstDataId);
        LoadResult = resResult;
        Closediv();
        isLoadDataCall = true;
        ReloadJqueryReference();
        isTstPostBackVal = resval[0] + "*$*" + resval[1] + "*$*" + resval[2];

        for (var ind = 0; ind < resval.length; ind++) {
            //TODO:We need to Review the below line.
            var strSingleLineText = resval[ind].toString().replace(new RegExp("\\n", "g"), "");
            strSingleLineText = strSingleLineText.replace(new RegExp("\\t", "g"), "&#9;");
            strSingleLineText = strSingleLineText.replace(/\\/g, ";bkslh");
            strSingleLineText = strSingleLineText.replace(new RegExp("&lt", "g"), "<");
            strSingleLineText = strSingleLineText.replace(new RegExp("&gt", "g"), ">");
            if (strSingleLineText == "")
                continue;
            try {
                var _myJSONObject = $j.parseJSON(strSingleLineText);
            }
            catch (ex) {
                continue;
            }

            //if (typeof _myJSONObject.data != "undefined") {
            //    resTstHtmlLS = resval[ind] + '♠*$' + transid;
            //} 
        }
        //if (resTstHtmlLS == "")
        //    resTstHtmlLS = transid;
    }
    else if (_TstlocalStorage.toLowerCase().indexOf("access violation") === -1) {
        isServerSide = 'false';
        ArrActionLog = "";
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
            "pointer-events": "auto"
        });
        if (FromSave) {
            FromSave = false;
        }
        EnableSaveBtn(true);
        isReadyCK = false;
        DCFrameNo.forEach(function (dcID) {
            //ClearFieldsInDC(dcID);
            let _isGrid = IsDcGrid(dcID);
            if (_isGrid)
                ClearRowsInGrid(dcID, "htmlload");
            else
                ClearFieldsInDC(dcID);
        });
        ExprPosArray.forEach(function (vals, ind) {
            if (vals != "")
                ExprPosArray[ind] = "";
        });
        tstReadOnlyPeg = false;
        navValidator = true;
        SetFormDirty(false);
        blurNextPreventId = "";
        document.title = "Tstruct";
        recordid = "0";
        $j("#recordid000F0").val("0");
        AxFormControlList = new Array();
        AxSetFldCaption = new Array();
        RegVarFldList = new Array();
        ChangedFields = new Array();
        ChangedFieldDbRowNo = new Array();
        ChangedFieldValues = new Array();
        DeletedDCRows = new Array();
        ChangedDcRows = new Array();
        ChangedDcs = new Array();
        AllFieldNames = new Array();
        AllFieldValues = new Array();
        ScriptMaskFields = new Array();
        AxExecFormControl = false;
        DisabledDcs = new Array();
        AxFormContHiddenFlds = new Array();
        AxFormContSetCapFlds = new Array();
        AxFormContSetCapFldsGrid = new Array();
        AxFormContSetFldActGrid = new Array();
        AxFormContSetGridCell = new Array();
        AxFormContFldSetFocus = new Array();
        multiSelectflds = new Array();
        multiSelFldParents = new Array();
        multiSelFldResult = new Array();
        multiSelectLoadVals = new Array();
        FldListParents = new Array();
        FldListData = new Array();
        FldDListPFNames = new Array();
        FldDListPFNameVals = new Array();
        AxRulesFlds = new Array();
        imgNames = new Array();
        imgSrc = new Array();
        changeFillGridDc = 0;
        if (isMobile)
            OnMobileNewTst();

        if (_TstlocalStorage != "") {
            var resval = _TstlocalStorage.split("*$*");
            if (resval[0] == "") {
                showAlertDialog("error", "Clear In-Memory and retry again.");
                return;
            }
            $("#tblWrk").html("");
            $("#collapseOneTable").html("");
            $j(".workflow").remove();
            $j(".wrkflwinline").remove();
            $j(".workflowMsg").remove();
            $j(".downArr").remove();
            $j(".Selectboxlist").remove();
            $j("#workflowoverlay").addClass("d-none");
            $j("#dvMessage").html("");
            $j("#dvMessage").removeClass("AXinfo").addClass("success d-none");
            $j("#file").addClass("d-none");
            $j("#attachment-overlay").html("");
            $j("#attachfname").val("");
            $j("#PEGDiv").addClass("d-none");
            $j("#AxAmendDiv").addClass("d-none");
            attachments = "";
            filenamearray = new Array();
            fileonloadarray = new Array();
            $("#hdnTabHtml").val(resval[3]);
            var tstPerfVars = resval[4];
            if (tstPerfVars != "") {
                wsPerfFormLoadCall = tstPerfVars.split(";")[0] == "true" ? true : false;
                wsPerfEvalExpClient = tstPerfVars.split(";")[1].split(",");
                headerAttachDir = tstPerfVars.split(";")[2];
                try {
                    if (tstPerfVars.split(";").length > 3) {
                        formParamFlds = tstPerfVars.split(";")[3].split(",");
                        formParamVals = tstPerfVars.split(";")[4].split(",");
                    } else {
                        formParamFlds = new Array();
                        formParamVals = new Array();
                    }
                } catch (ex) { }
            }
            var ImgVals = resval[5];
            if (ImgVals != "") {
                ImgVals = ImgVals.split("♠");
                for (i = 0; i < ImgVals.length; i++) {
                    imgNames[i] = ImgVals[i].split("♦")[0];
                    imgSrc[i] = ImgVals[i].split("♦")[1];
                    if (imgSrc[i] != "")
                        imgSrc[i] = imgSrc[i].replace("%20", " ");
                }
            }

            if (resval[6] != "") {
                var dcStatus = resval[6].split(',');
                dcStatus.forEach(function (dcStat, indx) {
                    TabDCStatus[indx] = dcStat;
                })
            }
            displayAutoGenVal = resval[7];
            $j("#hdnShowAutoGenFldValue").val(displayAutoGenVal);
            tstructCancelled = resval[8];
            tstDataId = resval[11];

            try {
                FFieldHidden = new Array();
                FFieldReadOnly = new Array();

                if (resval[9] != "") {
                    var _thisFFH = resval[9].split(',');
                    _thisFFH.forEach(function (_val) {
                        if (_val != "")
                            FFieldHidden.push(_val);
                    })
                }
                if (resval[10] != "") {
                    var _thisFFH = resval[10].split(',');
                    _thisFFH.forEach(function (_val) {
                        if (_val != "")
                            FFieldReadOnly.push(_val);
                    })
                }
            } catch (ex) { }

            $j("#hdnDataObjId").val(tstDataId);
            LoadResult = _TstlocalStorage;
            isLoadDataCall = false;
            ReloadJqueryReference();
            isTstPostBackVal = resval[0] + "*$*" + resval[1] + "*$*" + resval[2];
            //resTstHtmlLS = resval[13];
            for (var ind = 0; ind < resval.length; ind++) {
                //TODO:We need to Review the below line.
                var strSingleLineText = resval[ind].toString().replace(new RegExp("\\n", "g"), "");
                strSingleLineText = strSingleLineText.replace(new RegExp("\\t", "g"), "&#9;");
                strSingleLineText = strSingleLineText.replace(/\\/g, ";bkslh");
                strSingleLineText = strSingleLineText.replace(new RegExp("&lt", "g"), "<");
                strSingleLineText = strSingleLineText.replace(new RegExp("&gt", "g"), ">");
                if (strSingleLineText == "")
                    continue;
                try {
                    var _myJSONObject = $j.parseJSON(strSingleLineText);
                }
                catch (ex) {
                    continue;
                }
                if (typeof _myJSONObject.data != "undefined") {
                    resTstHtmlLS = resval[ind] + '♠*$' + transid;
                }
            }
            if (resTstHtmlLS == "")
                resTstHtmlLS = transid;
        }
    }
}

function isLocalStorageHtml() {
    var _TstlocalStorage = "";
    try {
        if (typeof (Storage) !== "undefined") {
            let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + transid + "-" + appSUrl + "♥)", "function");
            _TstlocalStorage = localStorage[_thisKey[0]];
            if (typeof _TstlocalStorage == "undefined") {
                _TstlocalStorage = "";
            }
        }
    } catch (e) { }
    if (_TstlocalStorage != "")
        return "true";
    else
        return "false";
}

function GetTstHtmlNodeLs(resVal) {
    var _thisArra = resVal;
    try {
        let _fndIndx = resVal.findIndex(element => element.startsWith(transid + "_"));
        if (_fndIndx == 11)
            return _thisArra
        else {
            var _TstlocalStorage = "";
            try {
                if (typeof (Storage) !== "undefined") {
                    let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                    let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + transid + "-" + appSUrl + "♥)", "function");
                    _TstlocalStorage = localStorage[_thisKey[0]];
                    if (typeof _TstlocalStorage == "undefined") {
                        _TstlocalStorage = "";
                    }
                }
            } catch (e) { }
            if (_TstlocalStorage != "") {
                let _thisHtmlls = _TstlocalStorage.split('♠♠♠')[1];
                let _resval = _thisHtmlls.split("♦♠♣♥");
                _resval = _resval[0].split("*$*");
                try {
                    //if (_resval[2] != "" && (_resval[2].indexOf("ftbtn_iSave~,") == -1 || _resval[2].indexOf("ftbtn_iNew~,") == -1)) {
                    //    let _fbSave = "♦";
                    //    if (_TstlocalStorage.split('♠♠♠')[0].indexOf("ftbtn_iNew~,") > -1)
                    //        _fbSave += "ftbtn_iNew~,";
                    //    if (_TstlocalStorage.split('♠♠♠')[0].indexOf("ftbtn_iSave~,") > -1)
                    //        _fbSave += "ftbtn_iSave~,";
                    //    if (_fbSave != "♦")
                    //        _fbSave += "♦";
                    //    _resval[2] = _fbSave;
                    //}
                    let _fbSave = "♦";
                    if (typeof _resval[2] != "undefined" && _resval[2].indexOf("ftbtn_iSave~") == -1) {
                        if (_TstlocalStorage.split('♠♠♠')[0].indexOf("ftbtn_iSave~") > -1)
                            _fbSave += "ftbtn_iSave~,";
                    }
                    if (typeof _resval[2] != "undefined" && _resval[2].indexOf("ftbtn_iNew~") == -1) {
                        if (_TstlocalStorage.split('♠♠♠')[0].indexOf("ftbtn_iNew~") > -1)
                            _fbSave += "ftbtn_iNew~,";
                    }
                    if (_fbSave != "♦") {
                        _fbSave += "♦";
                        _resval[2] += _fbSave;
                    }
                } catch (ex) { }
                _thisArra = new Array();
                _thisArra.push(_resval[0]);
                _thisArra.push(_resval[1]);
                _thisArra.push(_resval[2]);
                resVal.forEach(function (val, ind) {
                    if (ind != 0)
                        _thisArra.push(val);
                });
            } else {
                return _thisArra
            }
        }

    } catch (ex) { }
    return _thisArra
}
//TO Get LoadData from client side with the result & tstruct HTML.
function GetLoadData(recid, tstQureystr) {
    try {
        AxWaitCursor(true);
        ShowDimmer(true);
        GetProcessTime();
        if (typeof isLoadFromDraft != "undefined")
            isLoadFromDraft = "false";
        let _isDupTab = callParentNew('isDuplicateTab');
        if ((window.location.href).indexOf("AxSplit") != -1) {            
            tstQureystr += tstQureystr.indexOf("AxSplit") == -1 ? `${tstQureystr.endsWith("♠") ? "": "♠"}AxSplit=true` : "";
        }
        let isFromDummytrue = isLocalStorageHtml();
        $.ajax({
            url: 'tstruct.aspx/GetLoadDataValues',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                key: tstDataId,
                recordid: recid,
                tstQureystr: tstQureystr,
                isDupTab: _isDupTab,
                isTstHtmlLs: resTstHtmlLS,
                isFromDummytrue: isFromDummytrue
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                AxWaitCursor(true);
                ShowDimmer(true);
                resTstHtmlLS = '';
                var result = data.d;
                if (result != "") {
                    if (result.split("*♠♦*").length > 1) {
                        serverprocesstime = result.split("*♠♦*")[1];
                        requestProcess_logtime = result.split("*♠♦*")[2];
                        result = result.split("*♠♦*")[0];
                        WireElapsTime(serverprocesstime, requestProcess_logtime, true);
                    } else {
                        UpdateExceptionMessageInET("Error : " + result);
                    }
                }
                Closediv();
                if (result.toLowerCase().indexOf("access violation") === -1) {
                    ArrActionLog = "";
                    if (CheckSessionTimeout(result))
                        return;
                    if (result != "") {
                        actionCallbackFlag = actionCallFlag;
                        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                            "pointer-events": "auto"
                        });
                        if (FromSave) {
                            FromSave = false;
                        }
                        EnableSaveBtn(true);
                        isReadyCK = false
                        DCFrameNo.forEach(function (dcID) {
                            ClearFieldsInDC(dcID);
                            try {
                                let _isGrid = IsDcGrid(dcID);
                                if (_isGrid)
                                    ClearRowsInGrid(dcID, "htmlload");
                            } catch (ex) { }
                        });
                        ExprPosArray.forEach(function (vals, ind) {
                            if (vals != "")
                                ExprPosArray[ind] = "";
                        });
                        tstReadOnlyPeg = false;
                        navValidator = true;
                        SetFormDirty(false);
                        appstatus = "";
                        blurNextPreventId = "";
                        AxFormControlList = new Array();
                        AxSetFldCaption = new Array();
                        RegVarFldList = new Array();
                        ChangedFields = new Array();
                        ChangedFieldDbRowNo = new Array();
                        ChangedFieldValues = new Array();
                        ChangedDcRows = new Array();
                        ChangedDcs = new Array();
                        DeletedDCRows = new Array();
                        AllFieldNames = new Array();
                        AllFieldValues = new Array();
                        ScriptMaskFields = new Array();
                        AxExecFormControl = false;
                        DisabledDcs = new Array();
                        AxFormContHiddenFlds = new Array();
                        AxFormContSetCapFlds = new Array();
                        AxFormContSetCapFldsGrid = new Array();
                        AxFormContSetFldActGrid = new Array();
                        AxFormContSetGridCell = new Array();
                        AxFormContFldSetFocus = new Array();
                        multiSelectflds = new Array();
                        multiSelFldParents = new Array();
                        multiSelFldResult = new Array();
                        multiSelectLoadVals = new Array();
                        AxRulesFlds = new Array();
                        FldListParents = new Array();
                        FldListData = new Array();
                        FldDListPFNames = new Array();
                        FldDListPFNameVals = new Array();
                        changeFillGridDc = 0;
                        imgNames = new Array();
                        imgSrc = new Array();

                        if (isMobile)
                            OnMobileNewTst();

                        document.title = "Load Tstruct";
                        recordid = recid;
                        $j("#recordid000F0").val(recid);
                        var resval = result.split("*$*");
                        //let _loadFromls = false;
                        if (resval[0] == "") {
                            resval = GetTstHtmlNodeLs(resval);
                            if (resval[0] == "") {
                                //window.location.reload();
                                showAlertDialog("error", "Clear In-Memory and retry again.");
                                return;
                            }
                        }
                        //$("#tblWrk").html("");
                        //$("#collapseOneTable").html("");
                        $j(".workflow").remove();
                        $(".wfselectbox").html("<div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"bottom-end\" id=\"selectbox\"></div>");
                        $j(".wrkflwinline").remove();
                        $j(".workflowMsg").remove();
                        $j(".downArr").remove();
                        $j(".Selectboxlist").remove();
                        //$("#selectbox").remove();
                        $j("#workflowoverlay").addClass("d-none");
                        $j("#dvMessage").html("");
                        $j("#dvMessage").removeClass("AXinfo").addClass("success d-none");
                        $j("#file").addClass("d-none");
                        $j("#attachment-overlay").html("");
                        $j("#attachfname").val("");
                        attachments = "";
                        filenamearray = new Array();
                        fileonloadarray = new Array();
                        $("#hdnTabHtml").val(resval[3]);
                        var tstPerfVars = resval[4];
                        if (tstPerfVars != "") {
                            wsPerfFormLoadCall = tstPerfVars.split(";")[0] == "true" ? true : false;
                            wsPerfEvalExpClient = tstPerfVars.split(";")[1].split(",");
                            headerAttachDir = tstPerfVars.split(";")[2];
                            try {
                                if (tstPerfVars.split(";").length > 3) {
                                    formParamFlds = tstPerfVars.split(";")[3].split(",");
                                    formParamVals = tstPerfVars.split(";")[4].split(",");
                                } else {
                                    formParamFlds = new Array();
                                    formParamVals = new Array();
                                }
                            } catch (ex) { }
                        }
                        var ImgVals = resval[5];
                        if (ImgVals != "") {
                            ImgVals = ImgVals.split("♠");
                            for (i = 0; i < ImgVals.length; i++) {
                                imgNames[i] = ImgVals[i].split("♦")[0];
                                imgSrc[i] = ImgVals[i].split("♦")[1];
                                if (imgSrc[i] != "")
                                    imgSrc[i] = imgSrc[i].replace("%20", " ");
                            }
                        }
                        if (resval[6] != "") {
                            var dcStatus = resval[6].split(',');
                            dcStatus.forEach(function (dcStat, indx) {
                                TabDCStatus[indx] = dcStat;
                            })
                        }
                        tstructCancelled = resval[7];
                        if (typeof AxGridAttNotExistList != "undefined" && resval[8] != "") {
                            var agattnotfiles = resval[8].split(',');
                            AxGridAttNotExistList = agattnotfiles;
                        } else if (resval[8] != "") {
                            var agattnotfiles = resval[8].split(',');
                            AxGridAttNotExistList = agattnotfiles;
                        } else
                            AxGridAttNotExistList = "";
                            tstDataId = resval[11];

                        try {
                            FFieldHidden = new Array();
                            FFieldReadOnly = new Array();

                            if (resval[9] != "") {
                                var _thisFFH = resval[9].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldHidden.push(_val);
                                })
                            }
                            if (resval[10] != "") {
                                var _thisFFH = resval[10].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldReadOnly.push(_val);
                                })
                            }
                        } catch (ex) { }

                        $j("#hdnDataObjId").val(tstDataId);
                        LoadResult = result;
                        Closediv();
                        isLoadDataCall = true;
                        ReloadJqueryReference();
                        //if (typeof isWizardTstruct == "undefined" || isWizardTstruct == false) {
                        isTstPostBackVal = resval[0] + "*$*" + resval[1] + "*$*" + resval[2];
                        //}
                    }
                } else {
                    blurNextPreventId = "";
                    actionCallbackFlag = actionCallFlag;
                    $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                        "pointer-events": "auto"
                    });
                    if (FromSave) {
                        FromSave = false;
                    }
                    EnableSaveBtn(true);
                    AxWaitCursor(false);
                    ShowDimmer(false);
                    SetFormDirty(false);
                    $("#reloaddiv").show();
                    $("#dvlayout").hide();
                }
            },
            error: function (error) {
                Closediv();
                blurNextPreventId = "";
                actionCallbackFlag = actionCallFlag;
                $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                    "pointer-events": "auto"
                });
                if (FromSave) {
                    FromSave = false;
                }
                EnableSaveBtn(true);
                AxWaitCursor(false);
                ShowDimmer(false);
                SetFormDirty(false);
                $("#reloaddiv").show();
                $("#dvlayout").hide();
            }
        });
    } catch (exp) {
        Closediv();
        blurNextPreventId = "";
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
            "pointer-events": "auto"
        });
        if (FromSave) {
            FromSave = false;
        }
        EnableSaveBtn(true);
        AxWaitCursor(false);
        ShowDimmer(false);
        SetFormDirty(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
}

//Function which deletes a tstruct record.
function DeleteTstruct() {
    if (AxIsTstructLocked) {
        showAlertDialog("warning", 2008, "client");
        return;
    }

    AxStartTime = new Date();
    AxStartTime = GetAxDate(AxStartTime);
    var stTime = new Date();
    var rid = "0";
    if ($j("#recordid000F0").val() != "0") {
        rid = $j("#recordid000F0").val();
    } else {
        rid = $j("#recordid000F0").val();
    }

    trace = traceSplitStr + "DeleteData-" + tst + traceSplitChar;
    if (rid == "0") {
        showAlertDialog("warning", 2009, "client");
    } else if (tstructCancelled.toLowerCase() == "cancelled") {
        showAlertDialog("warning", 2010, "client");
        return;
    } else {
        if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
            if (!AxRulesBtnConMsgParser(event.currentTarget)) {
                return;
            }
        }
        //Mid of workflow record should not be deleted
        //commented becoz, only after final approval record should not be deleted this is handled in application
        //if ($j("#txtCommentWF").length > 0 && $j("#txtCommentWF").prop("disabled")) {
        //    showAlertDialog("warning", "This transaction cannot be deleted.");
        //    return;
        //}

        try {
            if (AxRulesDefComputescript == "true")
                AxRulesDefParser("compute script ondelete", "", "computescript");
            if (AxRulesDefValidation == "true") {
                if (!AxRulesDefParser("validate ondelete", "", "validate")) {
                    return;
                }
            }
        } catch (ex) { }

        var cutMsg = eval(callParent('lcm[5]'));
        var glType = eval(callParent('gllangType'));
        var isRTL = false;
        if (glType == "ar")
            isRTL = true;
        else
            isRTL = false;
        var DeleteTstructCB = $.confirm({
            theme: 'modern',
            closeIcon: false,
            title: eval(callParent('lcm[155]')),
            onContentReady: function () {
                disableBackDrop('bind');
            },
            backgroundDismiss: true,
            escapeKey: 'buttonB',
            rtl: isRTL,
            content: cutMsg,
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[164]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        DeleteTstructCB.close();
                        var txt = '';

                        if (rid == "0") {
                            showAlertDialog("warning", 2011, "client");
                        } else {
                            txt = txt + '<Transaction axpapp="' + proj + '" transid="' + tst + '" recordid="' + rid + '" action="delete" trace="' + trace + '" sessionid="' + sid + '" allowCancel="' + AxAllowCancel + '"> ';
                            //txt = txt + '</Transaction>';
                            try {
                                ASB.WebService.DeleteDataXML(rid, txt, tstDataId, AxAllowCancel, resTstHtmlLS, SucceededCallbackDelTst);
                            } catch (exp) {
                                AxWaitCursor(false);
                                var execMess = exp.name + "^♠^" + exp.message;
                                showAlertDialog("error", 2030, "client", execMess);
                            }
                        }
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[192]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        disableBackDrop('destroy');
                    }
                },
            }
        });

    }

    var edTime = new Date();
    AxTimeBefSerCall = edTime - stTime;
}

//Callback function for DeleteDataXML.
function SucceededCallbackDelTst(result, eventArgs) {
    var stTime = new Date();
    var isLogCalled = false;
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;

    var clrCacheKeys = result.split("*#*")[1];
    if (typeof clrCacheKeys != "undefined" && clrCacheKeys != "") {
        console.log("Clearing cache in delete");
        ClearRedisKeys(clrCacheKeys);
    }
    if (result.indexOf("*#*") > -1)
        result = result.substring(0, result.indexOf("*#*"));

    var resval = result.split("*$*");
    for (var ind = 0; ind < resval.length; ind++) {
        var strSingleLineText = resval[ind].toString().replace(new RegExp("\\n", "g"), "");
        if (strSingleLineText == "")
            return;

        resTstHtmlLS = "";
        var myJSONObject = $j.parseJSON(strSingleLineText);
        if (myJSONObject.error) {
            ExecErrorMsg(myJSONObject.error, "Delete");
        } else if (myJSONObject.timetaken) {
            ExecTimetaken(myJSONObject.timetaken);
        } else {        
            if (typeof transid != "" && transid == "ad_af") {
                eval(callParent('isRefreshParentOnClose') + "= true");
            }
            if (AxLogTimeTaken == "true") {
                var edTime = new Date();
                var diff = edTime.getTime() - stTime.getTime();
                CreateTimeLog(AxStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "DeleteData");
                isLogCalled = true;
            }
            showAlertDialog("success", 2012, "client");
            {
                // delete appLinkHistory[curPageIndex];
                delete callParentNew("appLinkHistory")[callParentNew("curPageIndex")];
                callParentNew("curPageIndex=", callParentNew("curPageIndex") - 1);
            }
            if (window.frameElement && window.frameElement.id && (window.frameElement.id === "loadPopUpPage" || window.frameElement.id === "iFrameCalendarEvents")) {
                eval(callParent('isSuccessAlertInPopUp') + "= true");
                SetFormDirty(false);
                if (transid == "axcal") {
                    callParentNew("closeModalDialog()", "function");
                } else {
                    $j("#axpertPopupWrapper .remodal-close", window.parent.document).click();
                    try {
                        callParentNew("loadPopUpPage", "id").dispatchEvent(new CustomEvent("close"));
                    } catch (ex) { }
                }
            } else {
                if (typeof transid!="undefined" && transid == "sect") {
                    try {
                        htmlCustomDeleteRedirect();
                    } catch (ex) { }
                } else
                    window.location.href = "tstruct.aspx?transid=" + tst + `&isDupTab=${callParentNew('isDuplicateTab')}`;
            }
            window.parent.isSessionCleared = true;
            ResetNavGlobalVariables();
        }
    }

    if (AxLogTimeTaken == "true" && isLogCalled == false) {
        var edTime = new Date();
        var diff = edTime.getTime() - stTime.getTime();
        CreateTimeLog(AxStartTime, AxTimeBefSerCall, diff, ASBTotal, ASBDbTime, "DeleteData");
    }
    const presentFrame = $(window.frameElement);
    if (recordid != "0" && presentFrame.attr("id") === "axpiframe") {
        presentFrame.removeClass("frameSplited");
        ReloadMiddleIframe();
    }
}

var printWindow;
//Function to print the transaction.
function OpenPrint(tid) {
    if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
        if (!AxRulesBtnConMsgParser(event.currentTarget)) {
            return;
        }
    }
    try {
        ASB.WebService.GetPrintDocs(tst, SuccessGetDocs);
    } catch (exp) {
        AxWaitCursor(false);
        var execMess = exp.name + "^♠^" + exp.message;
        showAlertDialog("error", 2030, "client", execMess);
    }
}

function SuccessGetDocs(result, eventArgs) {
    var dvHtml = "<div><label id=\"lblSelect\">Select Print Form</label><select id=\"ddlPrintForms\" class=\"combotem Family  form-control \" >";
    if (result.length > 0) {
        var i = 0;
        for (i = 0; i < result.length; i++) {
            var res = result[i].split("♣");
            dvHtml += "<option value=" + res[0] + ">" + res[1] + "</option>";
        }
    }
    dvHtml += "</select><br /><div class=\"pull-right\"><input type=\"button\" class=\"hotbtn btn\" value=\"Open\" onclick=\"javascript:AddPrintDoc();\" /></div></div>";
    // createBootstrapModal("dvPrintDoc", "Print Docs", dvHtml, "", "200px", "400px");
    displayBootstrapModalDialog("Print Docs", "", "200px", "", dvHtml)
}

function AddPrintDoc() {
    var rid = $j("#recordid000F0").val();
    var selectedDoc = $j("#ddlPrintForms").find("option:selected").text();
    var docType = $j("#ddlPrintForms").val();
    if (selectedDoc == "-- Select --") {
        showAlertDialog("warning", 2013, "client");
        return;
    }

    try {
        if (docType == "doc")
            ASB.WebService.AddPrintDoc(tst, selectedDoc, rid, tstDataId, docType, resTstHtmlLS, SuccessAddPrintDoc);
        else
            ASB.WebService.CreateFastReportDoc(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, tst, selectedDoc, rid, tstDataId, resTstHtmlLS, SuccessFastReportDoc);
    } catch (ex) {
        var execMess = ex.name + "^♠^" + ex.message;
        showAlertDialog("error", 2030, "client", execMess);
    }
}

function SuccessFastReportDoc(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    AssignLoadValues(result, "");
}

function SuccessAddPrintDoc(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    try {
        $j("#dvPrintDoc").dialog("close");;
    } catch (err) {

    }
    parent.RefreshPrintDocs();
}

//Function is used to cancel a workflow transaction.
function CancelTstruct() {

    if (tstructCancelled == "Cancelled") {
        return;
    }
    var rid = "0";
    if ($j("#recordid000F0").val() != "0") {
        rid = $j("#recordid000F0").val();
    }

    if (AxAllowCancel == false)
        return showAlertDialog("warning", 2014, "client");
    trace = traceSplitStr + "CancelData-" + tst + traceSplitChar;
    if (rid == "0") {
        showAlertDialog("warning", 2009, "client");
    } else {
        var cutMsg = eval(callParent('lcm[61]'));
        var glType = eval(callParent('gllangType'));
        var isRTL = false;
        if (glType == "ar")
            isRTL = true;
        else
            isRTL = false;
        var CancelTstructCB = $.confirm({
            theme: 'modern',
            closeIcon: false,
            rtl: isRTL,
            title: eval(callParent('lcm[155]')),
            onContentReady: function () {
                disableBackDrop('bind');
            },
            backgroundDismiss: true,
            escapeKey: 'buttonB',
            content: cutMsg,
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[164]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        CancelTstructCB.close();
                        var txt = '';

                        if (rid == "0") {
                            showAlertDialog("warning", 2015, "client");
                        } else {
                            var left = (screen.width / 2) - (350 / 2);
                            var top = (screen.height / 2) - (150 / 2);
                            var newWindow;
                            try {
                                newWindow = window.open('cancel.aspx?transid=' + tst + '&rid=' + rid, 'MyPopUp', 'width=550,height=170,scrollbars=no,resizable=yes,top=' + top + ',left=' + left + '');
                            } catch (ex) {
                                showAlertDialog("warning", eval(callParent('lcm[356]')));
                            }
                        }
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[192]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        disableBackDrop('destroy');
                    }
                }
            }
        });

    }
}

//Function called for deleting the cancelled transaction.
function CancelData() {

    var remarks = $j("#comments").val();
    if (remarks == "" || remarks.length == 0) {
        showAlertDialog("warning", 2016, "client");
    } else {

        trace = traceSplitStr + "CancelData-" + tst + traceSplitChar;
        var txt = '';
        txt = txt + '<Transaction axpapp="' + proj + '" transid="' + tst + '" recordid="' + recordid + '" action="cancel" trace="' + trace + '" sessionid="' + sid + '" allowCancel="' + AxAllowCancel + '"> ';
        txt = txt + '<comments>' + document.getElementById("comments").value + '</comments>';
        //txt = txt + '</Transaction>';
        var key = window.opener.tstDataId;
        try {
            ASB.WebService.DeleteDataXML(recordid, txt, key, AxAllowCancel, resTstHtmlLS, SucceededCallbackOnCancel);
        } catch (exp) {
            AxWaitCursor(false);
            var execMess = exp.name + "^♠^" + exp.message;
            showAlertDialog("error", 2030, "client", execMess);
        }
    }
}

//Callback function for DeleteDataXML
function SucceededCallbackOnCancel(result, eventArgs) {
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    if (result.substring(0, 7) == ErrStr) {
        var nres = result.substring(7, result.length - 8);
        showAlertDialog("error", nres);
    } else {
        result = result.replace(new RegExp("\\n", "g"), "");
        var myJSONObject = $j.parseJSON(result);
        if (myJSONObject.error) {
            ExecErrorMsg(myJSONObject.error, "CancelTrans");
            window.close();
        } else {
            showAlertDialog("info", 2017, "client");
            window.close();
            window.opener.location.href = "tstruct.aspx?transid=" + tst + `&isDupTab=${callParentNew('isDuplicateTab')}`;
        }
    }
}

//Function to expand or collapse the dc div.
function ShowDc(a) {
    var dcBtn = $("#dcButspan" + a);
    var tabBd = document.getElementById("divDc" + a);
    if (dcBtn) {
        var x = dcBtn.data("type");
        if (x == "hide") {
            dcBtn.data("type", "show");
            dcBtn.attr("title", "Show Dc");
            dcBtn.removeClass(' glyphicon-chevron-down icon-arrows-up').addClass(" glyphicon-chevron-down icon-arrows-down");
            if (tabBd) {
                tabBd.style.display = 'none';
            }
        } else if (x == "show") {
            dcBtn.data("type", "hide");
            dcBtn.attr("title", "Hide Dc");
            dcBtn.removeClass(' glyphicon-chevron-down icon-arrows-down').addClass(" glyphicon-chevron-down icon-arrows-up");
            if (tabBd) {
                tabBd.style.display = 'block';
            }
        }
    }
    // adjustwin("10", window);
}

//function to toggle the tabbed DC
function showHideTabbedDc(elem) {
    if ($(elem).attr("title") == "Hide Dc") {
        $(elem).attr("title", "Show Dc").text('expand_more');
        let _thisEleId = $(elem).attr("id");
        _thisEleId = _thisEleId.replace('tabCollapseButton', '');
        //$("[id^='tabsCont']").hide();
        if (IsDcGrid(_thisEleId)) {
            $("#divDc" + _thisEleId + " .griddivColumn").addClass("d-none");
            $("#divDc" + _thisEleId + " .gridIconBtns").addClass("d-none");
        } else {
            $("#divDc" + _thisEleId).addClass("d-none");
        }
    } else {
        $(elem).attr("title", "Hide Dc").text('expand_less');
        let _thisEleId = $(elem).attr("id");
        _thisEleId = _thisEleId.replace('tabCollapseButton', '');
        //$("[id^='tabsCont']").show();
        if (IsDcGrid(_thisEleId)) {
            $("#divDc" + _thisEleId + " .griddivColumn").removeClass("d-none");
            $("#divDc" + _thisEleId + " .gridIconBtns").removeClass("d-none");
        } else {
            $("#divDc" + _thisEleId).removeClass("d-none");
        }
    }
}

//Function to attach files to the transaction.
//function ShowAttach() {

//    var saMenu = document.getElementById("saMenu");
//    var saMenuAtt = document.getElementById("saMenuAttach");
//    var tstBut = document.getElementById("TstButArr");

//    if (saMenuAtt.style.visibility == "visible") {
//        saMenuAtt.style.visibility = "hidden";
//    }
//    else if (saMenu.style.visibility == "visible") {

//        saMenu.style.visibility = "hidden";
//        tstBut.alt = "hide";
//        tstBut.src = "../AxpImages/icons/16x16/expandwt.png";
//        saMenuAtt.style.visibility = "visible";
//    }
//    else if (saMenu.style.visibility == "hidden") {
//        saMenuAtt.style.visibility = "visible";
//    }
//    else {
//        saMenuAtt.style.visibility = "hidden";
//    }
//}

//Function which opens the pdf window on click of pdf toolbar button.
function ProcessRow() {

    var recid = "";
    recid = $j("#recordid000F0").val()

    if (recid == "0") {
        showAlertDialog("warning", 2009, "client");
    } else {
        var left = (screen.width / 2) - (350 / 2);
        var top = (screen.height / 2) - (150 / 2);
        var newWindow;
        try {
            newWindow = window.open('pdfparams.aspx?sname=' + tst + '&recid=' + recid + '&stype=tstructs', '_self', 'width=350,height=150,resizable=yes,top=' + top + ',left=' + left + '');
        } catch (ex) {
            showAlertDialog("warning", eval(callParent('lcm[356]')));
        }
    }
}

//Function to open the list view on click of listview button in the toolbar.
function CallListView(tid) {
    if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
        if (!AxRulesBtnConMsgParser(event.currentTarget)) {
            return;
        }
    } 
    // Pass Tstruct's Developer Option "FetchSize" value as Querrystring in Listview URL
    let _lvFetchSizeQs = "";
    try {
        // Filter the array to include only objects with "props" equal to "FetchSize" 
        let _fetchSizeObj = tstConfigurations.config.filter(item => item.props.toLowerCase() === "fetchsize");
        // Extract the "propsval" values from the filtered objects
        let _fetchSizeVal = "";
        if (_fetchSizeObj.length > 1) {
           _fetchSizeVal = _fetchSizeObj.filter(item => item.applyon.toLowerCase() === "single").map(item => item.propsval).toString();
        } else {
            _fetchSizeVal = _fetchSizeObj.map(item => item.propsval).toString();
        }        
        _lvFetchSizeQs = (_fetchSizeVal != "") ? `&lvFetchSize=${_fetchSizeVal}` : "";
    } catch (error) {
        _lvFetchSizeQs = "";
    }
    GetCurrentTime("Iview load on ListView button click(post back)");
    if (window.parent.globalChange) {
        var cutMsg = eval(callParent('lcm[31]'));
        var glType = eval(callParent('gllangType'));
        var isRTL = false;
        if (glType == "ar")
            isRTL = true;
        else
            isRTL = false;
        var CallListViewCB = $.confirm({
            theme: 'modern',
            closeIcon: false,
            title: eval(callParent('lcm[155]')),
            rtl: isRTL,
            onContentReady: function () {
                disableBackDrop('bind');
            },
            backgroundDismiss: true,
            escapeKey: 'buttonB',
            content: cutMsg,
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[164]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        CallListViewCB.close();
                        ShowDimmer(true);
                        window.parent.listViewPage = "0";
                        //window.document.location.href = "./listIview.aspx?tid=" + tid;
                        GetProcessTime();
                        if(frameElement.name == "axpiframe" && $(frameElement).hasClass("frameSplited")){
                            callParentNew(`OpenOnPropertyBase(iview.aspx?ivname=${tid}&tstcaption=${tstructCaption}&isDupTab=${callParentNew('isDuplicateTab')}${_lvFetchSizeQs})`, 'function');
                        }else{
                            window.document.location.href = `./iview.aspx?ivname=${tid}&tstcaption=${tstructCaption}&isDupTab=${callParentNew('isDuplicateTab')}${_lvFetchSizeQs}`;
                        }
                        SetFormDirty(false);
                        window.parent.isSessionCleared = true;
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[192]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        ShowDimmer(false);
                        disableBackDrop('destroy');
                        closeParentFrame();
                        window.parent.isSessionCleared = true;
                    }
                }

            }
        });

    } else {
        window.parent.listViewPage = "0";
        ShowDimmer(true);
        //window.document.location.href = "./listIview.aspx?tid=" + tid;
        GetProcessTime();
        if(frameElement.name == "axpiframe" && $(frameElement).hasClass("frameSplited")){
            callParentNew(`OpenOnPropertyBase(iview.aspx?ivname=${tid}&tstcaption=${tstructCaption}&isDupTab=${callParentNew('isDuplicateTab')}${_lvFetchSizeQs})`, 'function');
        }else{
            window.document.location.href = `./iview.aspx?ivname=${tid}&tstcaption=${tstructCaption}&isDupTab=${callParentNew('isDuplicateTab')}${_lvFetchSizeQs}`;
        }
        window.parent.isSessionCleared = true;
    }
    // let middleframe = callParentNew("middle1", "id");
    // $(middleframe).attr("src", "listiview.aspx?tid=" + tid);
}

//Function to upload a image.
function UploadImg(a) {
    if (!IsFormDirty)
        SetFormDirty(true);


    //var na = "./imageUpload.aspx?tid=" + tst + "&recordid=0&fldname=" + a;
    var isAxpImagePath = $("#isAxpImagePathHidden").val();
    if (isAxpImagePath == "true")
        displayBootstrapModalDialog("Image Upload", "xs", "150px", true, "./imageUpload.aspx?tid=" + tst + "&recordid=0&fldname=" + a + "&isAxpImagePath=" + isAxpImagePath + "&AxpCameraOption=" + AxpCameraOption)
    else
        displayBootstrapModalDialog("Image Upload", "xs", "150px", true, "./imageUpload.aspx?tid=" + tst + "&recordid=0&fldname=" + a + "&AxpCameraOption=" + AxpCameraOption)


}

function UploadCaptureImage(elem) {
    if (!IsFormDirty)
        SetFormDirty(true);

    // displayBootstrapModalDialog("Image Upload", "xs", "150px", true, "./imageUpload.aspx?tid=" + tst + "&recordid=0&fldname=" + a + "&AxpCameraOption=" + AxpCameraOption)

    var na = "./imageUpload.aspx?tid=" + tst + "&recordid=0&fldname=" + elem + "&AxpCameraOption=" + AxpCameraOption;
    // createPopup(na, "73vw");
    let myModal = new BSModal("modalIdCaptureImage", "", "<iframe class='col-12 h-100' src='" + na + "'></iframe>", () => {
        //shown callback
    }, () => {
        //hide callback
    });
    myModal.changeSize("fullscreen");
    myModal.hideFooter();
    myModal.hideHeader();
    myModal.showFloatingClose();
}

//Function to open the comments page of the particular workflow. 
function ViewComments() {

    var recid = $j("#recordid000F0").val();
    var na = "./Comments.aspx?transid=" + tst + "&recordid=" + recid;
    window.open(na, "SaveWindow", "width=840,height=600,scrollbars=yes,resizable=yes");
}

//Function to validate if the comments text area is empty.
function ValidateComments(remarks, actname) {

    var output = false;
    if (remarks.toString().trim() == "" && actname.toLowerCase() != "approve") {
        $("#lblreject").text("Please enter the comments.");
        $("#comment").focus();
        showAlertDialog("error", eval(callParent('lcm[523]')));
        return false;
    } else {
        output = true;
    }

    return output;
}

//Function which sets the css on blur of Lst item.
//function OnBlrLstItem(lst) {

//    lst.className = "popDivBg";
//}

//Function which sets the css on focus of link.
//function OnLnkFcs(ankr) {

//    ankr.style.color = "White";
//}

//Function which sets the css on blur of link.
//function OnLnkBlr(ankr) {

//    ankr.style.color = "Black";
//}

//Function to hide the task list.
function HideTaskList(btnClicked) {

    var dvtaskList = $j("#taskListPopUp");

    if (btnClicked == "true") {
        dvtaskList.hide();
    }

    if (IsTaskBtnCliked == true) {
        IsTaskBtnCliked = false;

        dvtaskList.hide();
        //         if (dvtaskList != null && dvtaskList.offsetWidth != undefined && dvtaskList.style.display == "block") {
        //             dvtaskList.style.display = "none";
        //         }
    } else {
        // dvtaskList.show();
        //         if (dvtaskList != null && dvtaskList.offsetWidth != undefined && dvtaskList.offsetHeight != undefined) {
        //             dvtaskList.style.display = "none";
        //         }
    }
}

//Function to open view history page.
function OpenHistory(tid) {
    if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
        if (!AxRulesBtnConMsgParser(event.currentTarget)) {
            return;
        }
    } 
    var rid = $j("#recordid000F0").val();
    //displayBootstrapModalDialog("View History", "xs", "180px", true, "./ViewHistory.aspx?tid=" + tid + "&rid=" + rid, false, () => {
    //    $(callParentNew('middle1', 'id').contentWindow.document.getElementsByTagName('body')).css("overflow", "");
    //}, () => {
    //});
    ShowDimmer(true);
    let _src = "./ViewHistory.aspx?tid=" + tid + "&rid=" + rid;
    let myModal = new BSModal("modalIdViewHistory", "View History", "<iframe class='w-100 h-400px' src='" + _src + "'></iframe>", () => {
        // CallbackFunctionBootstrap();
    }, () => {
        //hide callback
    });
    myModal.scrollableDialog();
    myModal.modalBody.classList.add('overflow-hidden');
    myModal.modalHeader.classList.add('py-4');
    myModal.changeSize("xl");
    myModal.hideFooter();
}

//Function to display no task defined if there is no action defined for a task button.
function AlertNoAction() {

    showAlertDialog("warning", 2018, "client");
}

//function GetInputFldXml(parentFld, fldId) {
//    var inputXml = "";
//    if ($j("#" + fldId).length > 0) {
//        var val = GetFieldValue(fldId);
//        inputXml = "~" + parentFld + "=" + val;
//    }
//    return inputXml;
//}

//Function to open the relevant page onclick of picklist.
function SearchOpen(txtobj) {
    //Not to enter in mainfocus twice in chrome
    if (window.chrome)
        $j("#HdnAxAdvPickSearch").val("true");
    var id = txtobj.attr("id");
    var fname1 = "";
    var fname = id.split('~');
    var pobj = "";
    var pobjval = "";
    fname1 = fname[1].toString();

    fldPk = $j("#" + fname1);
    if (fldPk.length > 0) {
        AxOldValue = fldPk.val();
    }

    if (fldPk.prop("disabled") == true) {
        return;
    }
    var fldFrameNo = GetFieldsDcNo(fname1);
    var fldName = GetFieldsName(fname1);
    var fldRowNo = GetFieldsRowNo(fname1);

    var parStr = "";
    if (AxActivePRow != "" && AxActivePDc != "")
        parStr = AxActivePDc + "~" + AxActivePRow;

    var subStr = "";
    if (IsParentField(fldName, fldFrameNo)) {
        //for each subgrid, get the sub grid rows for the given parent row and send this info
        subStr = GetSubGridInfoForParent(fldFrameNo, fldRowNo);
    }

    var x = "";
    x = $j("#" + fname1).val();
    if (x.indexOf("#") != -1) {
        x = x.replace(/#/, "hash;");
    }
    AxActiveRowNo = GetDbRowNo(fldRowNo, fldFrameNo);

    var objKey = tstDataId;
    // fldXml = fldXml.replace(/&/g, "%26");

    if ($j("#" + fname1).hasClass("fldFromSelect")) {
        var na = "./AutoComplete.aspx?search=" + x + "&fldname=" + fname1 + "&transid=" + tst + "&activeRow=" + AxActiveRowNo + "&frameno=" + fldFrameNo + "&key=" + objKey + "&parStr=" + parStr + "&subStr=" + subStr + "&AxPop=true";
        createPopup(na);
    } else {
        var na = "./srchComponent.aspx?search=" + x + "&fldname=" + fname1 + "&transid=" + tst + "&activeRow=" + AxActiveRowNo + "&frameno=" + fldFrameNo + "&key=" + objKey + "&parStr=" + parStr + "&subStr=" + subStr + "&AxPop=true";
        window.open(na, "SaveWindow", "width=800,height=530,scrollbars=no,resizable=yes").focus();
    }
}

function SearchOpenNew(fldAdvSrcId) {
    //Not to enter in mainfocus twice in chrome
    if (window.chrome)
        $j("#HdnAxAdvPickSearch").val("true");
    var id = fldAdvSrcId;
    var fname = fldAdvSrcId;

    fldPk = $j("#" + fname);
    if (!fldPk.hasClass("multiFldChk")) {
        if (fldPk.length > 0) {
            AxOldValue = fldPk.val();
        }
    } else {
        if (typeof fldPk.data("separator") != "undefined") {
            let fldAcValue = fldPk.val();
            let separator = fldPk.data("separator");
            AxOldValue = fldAcValue.join(separator);
        }
    }

    if (fldPk.prop("disabled") == true) {
        return;
    }
    var fldFrameNo = GetFieldsDcNo(fname);
    var fldName = GetFieldsName(fname);
    var fldRowNo = GetFieldsRowNo(fname);

    var parStr = "";
    if (AxActivePRow != "" && AxActivePDc != "")
        parStr = AxActivePDc + "~" + AxActivePRow;

    var subStr = "";
    if (IsParentField(fldName, fldFrameNo)) {
        //for each subgrid, get the sub grid rows for the given parent row and send this info
        subStr = GetSubGridInfoForParent(fldFrameNo, fldRowNo);
    }

    var x = "";
    x = $j("#" + fname).val();
    if (x != null) {
        if (x.indexOf("#") != -1)
            x = x.replace(/#/, "hash;");
    } else
        x = "";
    AxActiveRowNo = GetDbRowNo(fldRowNo, fldFrameNo);
    let _isDropfld = fldPk.hasClass("multiFldChk") ? "true" : (typeof fldPk.attr("data-refresh") == "undefined" ? "" : fldPk.attr("data-refresh"));
    let _isMultSel = fldPk.hasClass("multiFldChk") || fldPk.hasClass("fldmultiSelect") ? "true" : "false";
    let _thisFldVal = "";
    if (fldPk.hasClass("multiFldChk")) {
        if (fldPk.attr("data-selected") != "") {
            let _chkSep = fldPk.attr("data-separator");
            _thisFldVal = fldPk.val();
            _thisFldVal = _thisFldVal.join(_chkSep);
            _thisFldVal = _thisFldVal.replace(new RegExp(_chkSep, "g"), '♠');
        }
    } else if (fldPk.hasClass("fldmultiSelect")) {
        if (fldPk.attr("data-sep") != "") {
            let _chkSep = fldPk.attr("data-sep");
            _thisFldVal = fldPk.val();
            _thisFldVal = _thisFldVal.join(_chkSep);
            _thisFldVal = _thisFldVal.replace(new RegExp(_chkSep, "g"), '♠');
        }
    }
    else {
        _thisFldVal = fldPk.val();
    }
    var objKey = tstDataId;
    var na = "./AutoComplete.aspx?search=" + x + "&fldname=" + fname + "&transid=" + tst + "&activeRow=" + AxActiveRowNo + "&frameno=" + fldFrameNo + "&key=" + objKey + "&parStr=" + parStr + "&subStr=" + subStr + "&isFldddl=" + _isDropfld + "&selFldVal=" + _thisFldVal + "&isMultSel=" + _isMultSel + "&AxPop=true";
    // createPopup(na, "73vw");
    let myModal = new BSModal("loadPopUpPage", "", "<iframe id='loadPopUpPage' name='loadPopUpPage' class='col-12 h-100' src='" + na + "'></iframe>", () => {
        //shown callback
    }, (closing, modal) => {
        //hide callback
        try {
            if (closingModal = $((function getClosingModal(frame) {
                if (innerFrame = frame.contentDocument?.getElementById(myModal.elementId)?.querySelectorAll(`iframe[name=${myModal.elementId}]`)?.[0]) {
                    return getClosingModal(innerFrame);
                }
                else {
                    return frame;
                }
            })(myModal.modal._element.querySelectorAll(`iframe[name=${myModal.elementId}]`)?.[0])).parents(".modal")) {
                if (!$((myModal.modal._element.querySelectorAll(`iframe[name=${myModal.elementId}]`)?.[0])).parents(".modal").is(closingModal)) {
                    closingModal?.[0].dispatchEvent(new CustomEvent("close"));
                    closing.preventDefault();
                    return;
                }
            }
        } catch (ex) { }
    });
    myModal.changeSize("fullscreen");
    myModal.modalBody.classList.add('overflow-hidden');
    //if (_isMultSel != "true") {
    //    myModal.hideFooter();
    //} else {
    myModal.modalFooter.classList.add("py-3");
    myModal.okBtn.classList.add("btn-sm");
    myModal.cancelBtn.classList.add("btn-sm");
    myModal.okBtn.addEventListener("click", (e) => {
        document.getElementById('loadPopUpPage').querySelector("iframe").contentWindow.AddValuesToField();
    });
    //}    
    myModal.hideHeader();
    myModal.showFloatingClose();
}


function GetDcNoInAdvSearch(fieldName) {
    var fldDcNo = "0";
    if (fieldName != "" && fieldName != undefined)
        for (var i = 0; i < parent.FNames.length; i++) {

            if (parent.FNames[i].toLowerCase() == fieldName.toString().toLowerCase()) {
                fldDcNo = parent.FldFrameNo[i];
                break;
            }
        }
    return fldDcNo;
}

function IsDcGridInAdvSearch(dcNo) {
    var isGrid = false;
    for (var i = 0; i < parent.DCFrameNo.length; i++) {
        if (parent.DCFrameNo[i] == dcNo) {
            if (parent.DCIsGrid[i] == "False")
                isGrid = false;
            else
                isGrid = true;
            break;
        }
    }
    return isGrid;
}

//Function to set the field in the parent page from picklist page.
function SetSelectedValue(listcontrol, listctrl, _ValNotSel) {

    AxWaitCursor(true);
    ShowDimmer(true);

    if (_ValNotSel == false && listcontrol[listcontrol.selectedIndex].value != -1) {

        var fname = document.form1.fname.value;
        if (window.opener == null) {
            var parentFillField = $j("#" + fname, parent.window.document);
            var parentWinPickfld = $j('#pickfld000F0', parent.window.document);
        } else {
            var parentFillField = $j("#" + fname, window.opener.document);
            var parentWinPickfld = $j('#pickfld000F0', window.opener.document);
        }
        var nlen = fname.lastIndexOf("F");
        var cbname = fname.substring(0, nlen - 3);
        var rowNo = GetFieldsRowNo(fname);
        var dcNo = GetFieldsDcNo(fname);
        var fldDbRowNo = parent.GetDbRowNo(rowNo, dcNo);
        var cbnameExt = fname.substring(nlen - 3, fname.length);
        var result = listcontrol[listcontrol.selectedIndex].value;
        if (result == "*") {
            result = "";
        }
        var pickIdVal = "";
        if (result.indexOf("¿") != -1) {
            pickIdVal = result.substring(0, result.indexOf("¿"));
            result = result.substring(result.indexOf("¿") + 1);
        }

        if ($j("#" + fname, parent.window.document).hasClass('multiFldChk') && $j("#" + fname, parent.window.document).data("selected") != "") {
            let _selValues = $j("#" + fname, parent.window.document).data("selected");
            let _chkSep = $j("#" + fname, parent.window.document).data('separator');
            let _isValueSelected = _selValues.split(_chkSep).includes(result);
            if (_isValueSelected) {
                if (window.opener == null)
                    callParentNew("loadPopUpPage", "id").dispatchEvent(new CustomEvent("close"));
                else
                    window.close();
                callParentNew(`showAlertDialog(warning,This value already selected.)`, "function");
                AxWaitCursor(false);
                ShowDimmer(false);
                return;
            }
        }
        // parentFillField.val(result);
        window.parent.$("#" + $(parentFillField).attr("id")).append('<option value="' + result.replace(/"/g, '&quot;') + '" selected="selected">' + result + '</option>');
        if (parentWinPickfld)
            parentWinPickfld.val(fname);

        try {
            let _fName = GetFieldsName(fname);
            let _fInd = $j.inArray(_fName, parent.FNames);
            parent.checkNextSelExpFld(_fName, _fInd);
        } catch (ex) { }

        UpdateFieldArray(fname, fldDbRowNo, result, "popup");
        //UpdateAllFieldValues(fname, result);
        callParentNew("UpdateAllFieldValues(" + fname + "," + result + ")", "function");
        if (window.opener == null) {
            frSearchFld = $j("#pickIdVal_" + fname, parent.window.document);
            if (frSearchFld.length > 0) {
                frSearchFld.val(pickIdVal);
                parent.window.document.AxFromAssociated = true;
            }
        } else {
            frSearchFld = $j("#pickIdVal_" + fname, window.opener.document);
            if (frSearchFld.length > 0) {
                frSearchFld.val(pickIdVal);
                window.opener.AxFromAssociated = true;
            }
        }


        var lstFld = document.getElementById(listctrl);
        var len = lstFld.options.length;
        if (len > 0) {
            if (lstFld[listcontrol.selectedIndex].value != -1) {
                var fldarr = lstFld[listcontrol.selectedIndex].value;
                fldarr = fldarr.split('~');
                for (var fill = 0; fill < fldarr.length; fill++) {
                    if (fldarr[fill] != "") {
                        var fieldnm = fldarr[fill].toString();
                        fieldnm = fieldnm.split('***');
                        var fillfieldId = fieldnm[0] + cbnameExt;
                        if (window.opener == null) {
                            fillfield = parent.window.document.getElementById(fillfieldId);
                            if (typeof fillfield != "undefined" && fillfield == null) {
                                if (GetFieldsDcNo(fillfieldId) != GetDcNoInAdvSearch(fieldnm[0]) && !IsDcGridInAdvSearch(GetDcNoInAdvSearch(fieldnm[0]))) {
                                    let _thisfillfld = fieldnm[0] + "000F" + GetDcNoInAdvSearch(fieldnm[0]);
                                    fillfield = parent.window.document.getElementById(_thisfillfld);
                                }
                            }
                        }
                        else
                            fillfield = window.opener.document.getElementById(fillfieldId);
                        if (fillfield) {
                            if (fillfield.type != "radio") {
                                if (fieldnm[1] == "*") fieldnm[1] = "";
                                UpdateFieldArray(fillfieldId, fldDbRowNo, fieldnm[1], "popup");
                                //UpdateAllFieldValues(fillfieldId, fieldnm[1]);
                                callParentNew("UpdateAllFieldValues(" + fillfieldId + "," + fieldnm[1] + ")", "function");
                                fillfield.value = fieldnm[1];
                            }
                        }
                    }
                }
            }
        }
        UpdateParentArray();
        if (window.opener == null)
            callParentNew("loadPopUpPage", "id").dispatchEvent(new CustomEvent("close"));
        else
            window.close();
    } else if (_ValNotSel == true) {
        var fname = document.form1.fname.value;
        if (window.opener == null) {
            var parentFillField = $j("#" + fname, parent.window.document);
            var parentWinPickfld = $j('#pickfld000F0', parent.window.document);
        } else {
            var parentFillField = $j("#" + fname, window.opener.document);
            var parentWinPickfld = $j('#pickfld000F0', window.opener.document);
        }
        var nlen = fname.lastIndexOf("F");
        var cbname = fname.substring(0, nlen - 3);
        var rowNo = GetFieldsRowNo(fname);
        var dcNo = GetFieldsDcNo(fname);
        var fldDbRowNo = parent.GetDbRowNo(rowNo, dcNo);
        var cbnameExt = fname.substring(nlen - 3, fname.length);
        var result = "";
        if (result == "*") {
            result = "";
        }
        var pickIdVal = "";
        if (result.indexOf("¿") != -1) {
            pickIdVal = result.substring(0, result.indexOf("¿"));
            result = result.substring(result.indexOf("¿") + 1);
        }

        let _selValues = $j("#" + fname, parent.window.document).data("selected");
        window.parent.$("#" + $(parentFillField).attr("id")).find("option[value='" + _selValues + "']").remove();
        window.parent.$("#" + $(parentFillField).attr("id")).val("");
        if (parentWinPickfld)
            parentWinPickfld.val(fname);
        AxOldValue = " ";
        callParentNew("UpdateFieldArray(" + fname + "," + fldDbRowNo + "," + result + ",'parent')", "function");
        callParentNew("UpdateAllFieldValues(" + fname + "," + result + ")", "function");

        try {
            let _fName = GetFieldsName(fname);
            let _fInd = $j.inArray(_fName, parent.FNames);
            parent.checkNextSelExpFld(_fName, _fInd);
        } catch (ex) { }


        if (window.opener == null) {
            frSearchFld = $j("#pickIdVal_" + fname, parent.window.document);
            if (frSearchFld.length > 0) {
                frSearchFld.val(pickIdVal);
                parent.window.document.AxFromAssociated = true;
            }
        } else {
            frSearchFld = $j("#pickIdVal_" + fname, window.opener.document);
            if (frSearchFld.length > 0) {
                frSearchFld.val(pickIdVal);
                window.opener.AxFromAssociated = true;
            }
        }


        var lstFld = document.getElementById(listctrl);
        var len = lstFld.options.length;
        if (len > 0) {
            if (lstFld[0].value != -1) {
                var fldarr = lstFld[0].value;
                fldarr = fldarr.split('~');
                for (var fill = 0; fill < fldarr.length; fill++) {
                    if (fldarr[fill] != "") {
                        var fieldnm = fldarr[fill].toString();
                        fieldnm = fieldnm.split('***');
                        var fillfieldId = fieldnm[0] + cbnameExt;
                        if (window.opener == null) {
                            fillfield = parent.window.document.getElementById(fillfieldId);
                            if (typeof fillfield != "undefined" && fillfield == null) {
                                if (GetFieldsDcNo(fillfieldId) != GetDcNoInAdvSearch(fieldnm[0]) && !IsDcGridInAdvSearch(GetDcNoInAdvSearch(fieldnm[0]))) {
                                    let _thisfillfld = fieldnm[0] + "000F" + GetDcNoInAdvSearch(fieldnm[0]);
                                    fillfield = parent.window.document.getElementById(_thisfillfld);
                                }
                            }
                        }
                        else
                            fillfield = window.opener.document.getElementById(fillfieldId);
                        if (fillfield) {
                            if (fillfield.type != "radio") {
                                if (fieldnm[1] == "*") fieldnm[1] = "";
                                callParentNew("UpdateFieldArray(" + fillfieldId + "," + fldDbRowNo + ",'','parent')", "function");
                                callParentNew("UpdateAllFieldValues(" + fillfieldId + ",'')", "function");
                                fillfield.value = "";
                            }
                        }
                    }
                }
            }
        }
        UpdateParentArray();
        if (window.opener == null)
            callParentNew("loadPopUpPage", "id").dispatchEvent(new CustomEvent("close"));
        else
            window.close();
    }
    AxWaitCursor(false);
    ShowDimmer(false);
}

function SetSelectedValueMul(selList, unSelList, listctrl) {

    AxWaitCursor(true);
    ShowDimmer(true);

    if (selList.length > 0 || unSelList.length > 0) {

        var fname = document.form1.fname.value;
        if (window.opener == null) {
            var parentFillField = $j("#" + fname, parent.window.document);
            var parentWinPickfld = $j('#pickfld000F0', parent.window.document);
        } else {
            var parentFillField = $j("#" + fname, window.opener.document);
            var parentWinPickfld = $j('#pickfld000F0', window.opener.document);
        }
        var nlen = fname.lastIndexOf("F");
        var rowNo = GetFieldsRowNo(fname);
        var dcNo = GetFieldsDcNo(fname);
        var fldDbRowNo = parent.GetDbRowNo(rowNo, dcNo);
        var cbnameExt = fname.substring(nlen - 3, fname.length);
        var result = "";

        let _chkSep = typeof $j("#" + fname, parent.window.document).data('separator') != "undefined" ? $j("#" + fname, parent.window.document).data('separator') : $j("#" + fname, parent.window.document).data('sep');

        let _selValues = $j("#" + fname, parent.window.document).data("selected");
        unSelList.forEach(function (ele) {
            let _thisVal = ele;
            let _isValueSelected = _selValues.split(_chkSep).includes(_thisVal);
            if (_isValueSelected) {
                if (_selValues.indexOf(_chkSep) > -1) {
                    _selValues = _selValues.split(_chkSep).filter(val => val !== _thisVal).join(_chkSep);
                }
               else
                    _selValues = _selValues.replace(_thisVal, "");
                window.parent.$("#" + $(parentFillField).attr("id")).find("option[value='" + _thisVal + "']").remove();
            }
        });

        //_selValues = $j("#" + fname, parent.window.document).data("selected");
        selList.forEach(function (ele) {
            let _thisVal = ele;
            let _isValueSelected = _selValues.split(_chkSep).includes(_thisVal);
            if (!_isValueSelected) {
                if (_selValues == "")
                    _selValues += _thisVal;
                else
                    _selValues += _chkSep + _thisVal;
                window.parent.$("#" + $(parentFillField).attr("id")).append('<option value="' + _thisVal.replace(/"/g, '&quot;') + '" selected="selected">' + _thisVal + '</option>');
            }
        });

        if (parentWinPickfld)
            parentWinPickfld.val(fname);
        result = _selValues;
        UpdateFieldArray(fname, fldDbRowNo, result, "popup");
        callParentNew("UpdateAllFieldValues(" + fname + "," + result + ")", "function");

        var lstFld = document.getElementById(listctrl);
        var len = lstFld.options.length;
        if (len > 0) {
            if (lstFld[0].value != -1) {
                var fldarr = lstFld[0].value;
                fldarr = fldarr.split('~');
                for (var fill = 0; fill < fldarr.length; fill++) {
                    if (fldarr[fill] != "") {
                        var fieldnm = fldarr[fill].toString();
                        fieldnm = fieldnm.split('***');
                        var fillfieldId = fieldnm[0] + cbnameExt;
                        if (window.opener == null)
                            fillfield = parent.window.document.getElementById(fillfieldId);
                        else
                            fillfield = window.opener.document.getElementById(fillfieldId);
                        if (fillfield) {
                            if (fillfield.type != "radio") {
                                if (fieldnm[1] == "*") fieldnm[1] = "";
                                UpdateFieldArray(fillfieldId, fldDbRowNo, fieldnm[1], "popup");
                                callParentNew("UpdateAllFieldValues(" + fillfieldId + "," + fieldnm[1] + ")", "function");
                                fillfield.value = fieldnm[1];
                            }
                        }
                    }
                }
            }
        }
        UpdateParentArray();
        if (window.opener == null)
            callParentNew("loadPopUpPage", "id").dispatchEvent(new CustomEvent("close"));
        else
            window.close();
    }
    AxWaitCursor(false);
    ShowDimmer(false);
}

//Function to update field array from the fill grid page. 
function UpdateParentArray() {

    var isAlreadyFound = false;
    if (fldNewNameArr.length > 0) {

        for (var n = 0; n < fldNewNameArr.length; n++) {
            if (window.opener == null) {
                parent.window.ChangedFields.push(fldNewNameArr[n].toString());
                parent.window.ChangedFieldDbRowNo.push(fldNewDbRowNo[n].toString());
                parent.window.ChangedFieldValues.push(fldNewValueArr[n].toString());
                parent.window.ChangedFieldOldValues.push("");
            } else {
                window.opener.ChangedFields.push(fldNewNameArr[n].toString());
                window.opener.ChangedFieldDbRowNo.push(fldNewDbRowNo[n].toString());
                window.opener.ChangedFieldValues.push(fldNewValueArr[n].toString());
                window.opener.ChangedFieldOldValues.push("");
            }
        }
    }
    if (window.opener == null)
        AxBlurAction = "Default";
    else
        window.opener.AxBlurAction = "Default";

}

//Function to validate '&' and other special characters
function CheckSpecialCharsInXml(str) {
    var str = str;
    str = str.replace(/&/g, "&amp;");
    str = str.replace(/</g, "&lt;");
    str = str.replace(/>/g, "&gt;");
    str = str.replace(/'/g, "&apos;");
    str = str.replace(/"/g, '&quot;');
    return str;
}

//----------------------------------- Workflow functions --------------------------
var actname = "";
var wfButStatus;


//function CheckFieldsInArray() {
//    var fieldsChanged = false;
//    for (var i = 0; i < ChangedFields.length; i++) {
//        if (ChangedFields[i].substring(0, 2) == "dc" && ChangedFieldDbRowNo[i] == "-1")
//            continue;

//        fieldsChanged = true;
//    }
//    return fieldsChanged;
//}

//Function to check if any of the field is updated, on click of a workflow action button. 
function CheckFields(btnobj) {

    try {
        AxBeforeWFAction();
    } catch (ex) {}
    ShowDimmer(true);
    var inxml = "";
    var tid = transid;
    var txt = '';
    var tem = 1;
    //var b =document.form1.elements.length;
    var blen = FNames.length;
    var rid = $j("#recordid000F0").val();
    var remarks = $j("#comment").val();
    var trace = traceSplitStr + "Workflow-" + tid + traceSplitChar;
    actname = btnobj.value;
    let _mwfComments = "true";
    if (typeof AxMandatoryWfComments != "undefined" && AxMandatoryWfComments != "")
        _mwfComments = AxMandatoryWfComments;
    var returnres = _mwfComments == "true" ? ValidateComments(remarks, actname) : true;
    wfButStatus = GetWFButStatus();
    DisableWFButtons();

    if (returnres) {

        if ((btnobj.id == "btntabreject" && AxOnRejectSave == false) || (btnobj.id == "btntabreturn" && AxOnReturnSave == false)) {
            AxGlobalChange = false;
        }
    }

    var lno = "";
    //Get the workflow Level no
    if ($j("#hdnWfLno").length > 0) {
        lno = $j("#hdnWfLno").val();
    }
    var elno = "";
    //Get the workflow Level no
    if ($j("#hdnWfELno").length > 0) {
        elno = $j("#hdnWfELno").val();
    }

    if (actname == "Approve & Forward") actname = "Review";
    remarks = CheckSpecialCharsInXml(remarks);
    remarks = remarks.replace(/%20/g, " ");
    if (returnres) {

        var files = UploadFiles();
        inxml = '<root axpapp="' + proj + '" trace="' + trace + '"  sessionid="' + sid + '" transid="' + tid + '"   recordid="' + rid + '" actname="' + actname + '" comments="' + remarks + '" changed="' + AxGlobalChange + '" lno="' + lno + '"  elno="' + elno + '">';

        try {
            if (AxGlobalChange) {
                ASB.WebService.ProcessWFActionChanges(ChangedFields, ChangedFieldDbRowNo, ChangedFieldValues, DeletedDCRows, files, rid, inxml, tstDataId, resTstHtmlLS, SuccessWFCAction);
            } else {
                ASB.WebService.ProcessWFAction(inxml, SuccessWFAction);
            }
            AxGlobalChange = false;
        } catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
            var execMess = exp.name + "^♠^" + exp.message;
            showAlertDialog("error", 2030, "client", execMess);
            EnableWFBut();
        }
    } else {
        EnableWFBut();
        ShowDimmer(false);
    }
}

function SuccessWFAction(result, eventArgs) {
    SucceededCallbackAction(result);
    ShowDimmer(false);
}

function SuccessWFCAction(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    SucceededCallbackAction(result, "WithChanges");
    ShowDimmer(false);
}

//Callback function for workflow action.
function SucceededCallbackAction(result, calledFrom) {

    if (CheckSessionTimeout(result))
        return;
    var proceed = true;
    try {
        proceed = AxAfterWFAction(result);
    } catch (ex) {
        proceed = true;
    }
    if (typeof proceed == "undefined" || proceed) {
        AxGlobalChange = false;
        if (actname == "Approve") {
            actname = actname + 'd';
        }
        if (actname == "Reject") {
            actname = actname + 'ed';
        }
        if (actname == "Review") {
            actname = 'Forwarded';
        }
        if (actname == "Return") {
            actname = actname + 'ed';
        }
        if (result == "done") {

            if (actname == "Approved") {
                Readonlyform();
            }
            if (findGetParameter("AxPop") != null) {
                callParentNew('isSuccessAlertInPopUp=', true);
            }
            showAlertDialog("info", 2051, "client", actname);
            window.location.href = window.location.href;
        } else {
            // Page element to display feedback.
            var resSplit1 = result.split(",");
            var reslen = result.length;
            var restxt = result.substring(reslen - 20, reslen);

            if (restxt == "Transaction commited") {

                window.status = eval(callParent('lcm[76]'));
                if (findGetParameter("AxPop") != null) {
                    callParentNew('isSuccessAlertInPopUp=', true);
                }
                showAlertDialog("info", 2051, "client", actname);
                DisableWFButtons();

                if (tst == "pwda") {
                    window.document.location.href = "./mid1.aspx";
                } else {
                    if (tstType == "loadqs") {
                        window.location.href = window.location.href;
                    } else {
                        window.document.location.href = "./tstruct.aspx?transid=" + tst + `&openerIV=${typeof isListView != "undefined" ? iName : tst}&isIV=${typeof isListView != "undefined" ? !isListView : "false"}&isDupTab=${callParentNew('isDuplicateTab')}`;
                    }
                }
            } else {

                var restxt = result.substring(0, 7);

                if (restxt == ErrStr) {
                    if (calledFrom != undefined && calledFrom == "WithChanges")
                        AxGlobalChange = true;
                    var nres = result.substring(7, result.length - 8);
                    showAlertDialog("error", nres);
                    EnableWFBut();
                    return;
                }
                var reslen = result.length;
                var endpt = reslen - 8;
                var finalerr = result.substring(7, endpt);
                // Err Focusing
                var stPos = finalerr.indexOf("[");
                var endPos = finalerr.indexOf("]");
                var tempStr = "";
                var tempStr1 = "";
                var errFld = "";
                if ((stPos < 0) && (endPos < 0)) { } else {

                    tempStr = finalerr.substring(0, stPos);
                    tempStr1 = finalerr.substring(endPos + 2);
                    errFld = finalerr.substring(stPos + 1, endPos);

                    if (errFld != "") {
                        var nerr = finalerr.substring(stPos, endPos + 2);
                        finalerr = finalerr.replace(nerr, "");
                    }

                    if (tempStr1.indexOf(tempStr) > -1) {
                        finalerr = tempStr1;
                    } else {
                        finalerr = tempStr + tempStr1;
                    }
                }
                showAlertDialog("error", finalerr);
                EnableWFBut();
                var errDcNo = GetDcNo(errFld);
                errFld = errFld + "000F" + errDcNo;
                focusFld = $j("#" + errFld);
                if (focusFld.length > 0) {
                    try {
                        focusFld.focus();
                    } catch (ex) { }
                }
            }
        }
    }
}

//Function to get the workflow status.
function GetWFButStatus() {

    var x = 0;
    if ($j("#btntabapprove").prop("disabled") == false)
        x += 1;

    if ($j("#btntabreject").prop("disabled") == false)
        x += 2;

    if ($j("#btntabreview").prop("disabled") == false)
        x += 4;

    if ($j("#btntabreturn").prop("disabled") == false)
        x += 8;

    return x;
}

//Function to enable the workflow buttons if workflow status is not 0.
function EnableWFBut() {

    if (wfButStatus == 0) {
        $j("#txtCommentWF").prop("disabled", true);
        return;
    } else {
        $j("#txtCommentWF").prop("disabled", false);
    }
    $("#slectbox").attr('disabled', false);
    $j("#btntabapprove").prop("disabled", (wfButStatus & 1) == 1 ? false : true);
    $j("#btntabreject").prop("disabled", (wfButStatus & 2) == 2 ? false : true);
    $j("#btntabreview").prop("disabled", (wfButStatus & 4) == 4 ? false : true);
    $j("#btntabreturn").prop("disabled", (wfButStatus & 8) == 8 ? false : true);
}


//Function to disable the workflow action buttons. 
function DisableWFButtons() {
    $j("#btntabapprove").prop("disabled", true);
    $j("#btntabreject").prop("disabled", true);
    $j("#btntabreview").prop("disabled", true);
    $j("#btntabreturn").prop("disabled", true);
    if ($j("#txtCommentWF"))
        $j("#txtCommentWF").prop("disabled", true);
    $("#slectbox").attr("disabled", true);
}

//function CalculateRowsSubTotal(formatGridIdx, fRNo, dcNo) {
//    var subTotCols = DcSubTotCols[formatGridIdx].toString().split(",");
//    var keyColVal = GetFieldValue(DcKeyColumns[formatGridIdx] + fRNo + "F" + dcNo);
//    for (var colIdx = 0; colIdx < subTotCols.length; colIdx++) {
//        UpdateSubTotal(subTotCols[colIdx], fRNo, dcNo, DcKeyColumns[formatGridIdx]);
//    }
//}

function AssignHTML(result, calledFrom, source) {

    var resArr = result.split("*Tab*");
    var dcNo = "";
    var mainTabDc = "";
    var rCnt = 0;
    var dcArray = new Array();
    var isDcPop = false;
    var pDcNo = "";
    var pRCnt = "0";
    for (var i = 0; i < resArr.length; i++) {

        var dcData = resArr[i].split("*?*");
        var dcStr = dcData[0].toString().split("♣");
        dcNo = dcStr[0];
        rCnt = dcStr[1];
        if (!isNaN(rCnt)) rCnt = parseInt(rCnt, 10);

        if (dcNo != "") {
            if (!isDcPop)
                isDcPop = IsDcPopGrid(dcNo);

            if (!isDcPop) {
                pDcNo = dcNo;
                pRCnt = rCnt;
            }

            if (i == 0) {

                var dvId = "tab-" + dcNo;
                mainTabDc = dvId;
                var dvTab = $j("#" + dvId);
                //if (dvTab.length > 0 && !isWizardTstruct)
                if (dvTab.length > 0) {
                    if (dvTab.find("#" + "DivFrame" + dcNo).length == 0 && $("#DivFrame" + dcNo).length) {
                        $("#" + "DivFrame" + dcNo).replaceWith($(dcData[1]).find("#" + "DivFrame" + dcNo));
                    } else {
                        dvTab.html(dcData[1]);
                    }
                } else {
                    dvId = "DivFrame" + dcNo;
                    var dvTab = $j("#" + dvId);
                    //if(!isWizardTstruct)
                    dvTab.html(dcData[1]);
                    //else
                    //    dvTab.replaceWith($(dcData[1]).find("#"+dvId));
                }
            } else {
                dvId = "DivFrame" + dcNo;
                var dvTab = $j("#" + dvId);
                dvTab.html(dcData[1]);

            }
            if (IsDcGrid(dcNo)) {

                var tmpRecId = "";
                tmpRecId = $j("#recordid000F0").val();
                if (tmpRecId != "0" && calledFrom == "FillGrid") {
                    for (var dIdx = 1; dIdx <= parseInt(pRCnt, 10); dIdx++) {
                        var rowNo = GetRowNoHelper(dIdx);
                        UpdateChangedRows(rowNo + "F" + pDcNo, dIdx);
                    }

                }

                ClearDcRowArrays(dcNo);
                UpdateDcArrays(dcNo, rCnt);
                ResetGridRowsStyle(dcNo, rCnt);
                CheckScroll(dcNo);
                AssignGrdFreezeHdrScript(dcNo);
                var dcIdx = $j.inArray(dcNo, DCFrameNo);
                if (DCHasDataRows[dcIdx] == "True")
                    SetHdnRowCount(dcNo, source);
            }
            dcArray.push("#" + dvId);
        }
        if (source == "GetTabData" && isWizardTstruct) {
            WizardTabDcHtml(dcNo);
        }
        //gridScrollBar developer option withdran as per UI.
        //if (typeof gridScrollBar != "undefined" && gridScrollBar == "true") {
        //    $('.griddivColumn').parents().find('.dcTitle').css({
        //        'display': 'block'
        //    });
        //    if ($('body').hasClass('btextDir-rtl')) {
        //        $('.griddivColumn').parents().find('.newgridbtn').css({
        //            'float': 'right'
        //        });
        //    } else {
        //        $('.griddivColumn').parents().find('.newgridbtn').css({
        //            'float': 'left'
        //        });
        //    }
        //}
    }
    $j(".axpvalid").each(function () {
        DoFormControlPrivilege($j(this).attr("id"), $j(this).val());
    });
    if (calledFrom != "LoadData") {
        if (isDcPop)
            UpdatePopGridInfo();

        LoadGridScript();
        AssignJQueryEvents(dcArray, calledFrom);
    }
    if (calledFrom == "FillGrid") {
        FillGridFillRows = pRCnt;
        FillGridCurrentDC = pDcNo;
    }
}



function AssignGrdFreezeHdrScript(dcNo) {
    //  SetGridHeaderWidth(dcNo);


    var content = $j("#contentScroll" + dcNo);
    var headers = $j("#colScroll" + dcNo);
    //gridDc-gridHd
    content.scroll(function () {
        headers.scrollLeft(content.scrollLeft());
    });
}


function CheckAxpvalid(dcNo, rowNo, fields) {
    AxWaitCursor(true);
    var axpValidval = "";
    for (var i = 0; i < fields.length; i++) {

        var fldName = fields[i] + rowNo + "F" + dcNo;
        if (fldName.indexOf("axpvalid") != -1) {
            axpValidval = GetFieldValue(fldName);
            break;
        }
    }
    if (axpValidval != "")
        GridRowBehave(axpValidval, rowNo, dcNo, fields);
    AxWaitCursor(false);
}

function GridRowBehave(axpValidval, rowNo, dcNo, fields) {

    var disableDel = true;
    var disableField = true;
    if (axpValidval.toString() == "" || axpValidval.toString().toUpperCase() == "A") {
        disableDel = false;
        disableField = false;
    } else if (axpValidval.toString().toUpperCase() == "B") {
        disableDel = true;
        disableField = false;
    } else if (axpValidval.toString().toUpperCase() == "C") {
        disableDel = true;
        disableField = true;
    }
    if (disableDel)
        $("#sp" + dcNo + "R" + rowNo + "F" + dcNo + " td:first").find('.glyphicon.glyphicon-trash').addClass('disabled').prop("disabled", true);
    if (disableField)
        $("#sp" + dcNo + "R" + rowNo + "F" + dcNo + " td:first").find('.glyphicon.glyphicon-pencil.icon-software-pencil').addClass('disabled').prop("disabled", true);

    delImg = $j("#" + "del" + rowNo + "F" + dcNo);

    if (delImg.length > 0) {
        $j(this).prop("disabled", disableDel);
        delImg.css('cursor', 'hand');
        if (!disableDel) {
            delImg.removeClass("disabledelete");
            delImg.unbind('click');
            $j("#" + "del" + rowNo + "F" + dcNo).bind('click', function () {
                DeleteCurrentRow($j(this));
            });
        }
    }
    for (var j = 0; j < fields.length; j++) {
        var flName = fields[j] + rowNo + "F" + dcNo;

        var fld = $j("#" + flName);
        // If the same element of previous row is disabled then disabled true
        var prevRowNo = parseInt(rowNo) - 1;
        var prevCompleteRowNo = GetRowNoHelper(prevRowNo);
        var prevFld = $j("#" + fields[j] + prevCompleteRowNo + "F" + dcNo);
        //if (fld.length > 0 && axpValidval.toString().toUpperCase() != "C" && prevFld.length > 0 && prevFld.hasClass("flddis"))
        //removing prevFld.length > 0 && prevFld.hasClass("flddis") from the above statement as there will be no previous row in new grid
        if (fld.length > 0 && axpValidval.toString().toUpperCase() == "C")
            fld.prop("disabled", true);
        else {
            if (!fld.hasClass('flddis'))
                fld.prop("disabled", disableField);
        }

        $j("#sp" + dcNo + "R" + rowNo + "F" + dcNo).find(".pickimg").each(function () {
            $j(this).prop("disabled", disableField);
            $j(this).css('cursor', 'hand');
        });
    }
}

function EnableDisableBtns(obj, enable) {
    //This function will be called on toolbar buttons.
    //Toolbar buttons have a anchor tag and an image inside.
    //Here if the parameter obj is image then we are disabling the parent of img i.e. the anchor tag,
    //since the onclick event is on anchor tag.

    var isObjFound = false;
    if (obj.length > 0) {

        if (obj[0].tagName.toLowerCase() == "a") {
            var childObj = obj.children()[0];

            if (childObj != undefined) {
                var childId = childObj.id;
                isObjFound = true;

                if (enable == false) {
                    if (childObj.className == "handCur")
                        childObj.className = "handCurDis";

                    childObj.disabled = true;

                    //if (!obj.hasClass("action"))
                    //    $j('#' + childId).toggle(false);
                    if ($j(childObj).is("img")) {
                        $j(childObj).css("opacity", "0.5");
                        $j(childObj).css("cursor", "no-drop");
                    }
                } else {
                    childObj.disabled = false;
                    if (childObj.className == "handCurDis")
                        childObj.className = "handCur";

                    //if (!obj.hasClass("action"))
                    //    $j('#' + childId).toggle(true);
                    if ($j(childObj).is("img")) {
                        $j(childObj).css("opacity", "1")
                        $j(childObj).css("cursor", "pointer");
                    }
                }
            }
        }

        if (enable == false) {
            obj.prop("disabled", true);
            obj.addClass('disabled');
            obj.addClass('btndis');
            obj.css("pointer-events", 'none');
            obj.css("cursor", 'default');  
            obj.attr('tabindex', -1);
            if (obj.hasClass('menu-link'))
                obj.addClass('btn');
        } else {
            obj.prop("disabled", false);
            obj.removeClass('disabled');
            obj.removeClass('btndis');
            obj.css("pointer-events", 'auto');
            obj.css("cursor", '');
            obj.css("cursor", 'hand');
            obj.removeAttr('tabindex');
            if (obj.hasClass('menu-link'))
                obj.removeClass('btn');
        }

        var isAxpBtn = false;
        if (obj.attr("id") && obj.attr("id").toLowerCase().startsWith("axptstbtn_"))
            isAxpBtn = true;

        //if (!isObjFound && !obj.hasClass("action") && !isAxpBtn) {
        //    if (enable == false)
        //        obj.toggle(false);
        //    else
        //        obj.toggle(true);
        //}
    }
}




function SaveInCache(rid, calledfrom) {

    //TODO: If the tstruct contains tab, then focus the first tab.
    var dcNo = "";
    try {
        dcNo = TabDCs[0];
        if (dcNo != "") {
            $j("#ank" + dcNo).click();
            if (calledfrom == "Save") {
                var dvObj = $j("#dvMessage");
                dvObj.show();
            }
        }
    } catch (ex) {

    }

    var tstHtml = $j("#wBdr").html();

    var jsArrayStr = "<script type='text/javascript'>" + GetJsArrays() + "</script>";
    try {
        if (calledfrom == "SaveAction")
            ASB.WebService.AddToDataCache(tstHtml, tstDataId, jsArrayStr, rid, resTstHtmlLS, SuccessSaveActionCache);
        else
            //ASB.WebService.AddToDataCache(tstHtml, tstDataId, jsArrayStr, rid, SuccessSaveCache);//if required save we can add later
            ASB.WebService.AddToDataCache(tstHtml, tstDataId, jsArrayStr, rid, resTstHtmlLS);
    } catch (exp) {
        AxWaitCursor(false);
        showAlertDialog("error", ServerErrMsg);
    }
}

function SuccessSaveActionCache(result, eventArgs) {
    if (result != "" && result.split("♠*♠").length > 1) {
        tstDataId = result.split("♠*♠")[0];
        result = result.split("♠*♠")[1];
    }
    if (CheckSessionTimeout(result))
        return;
    resTstHtmlLS = "";
    ShowDimmer(true);
    AxWaitCursor(true);
    if (result.indexOf("error:") == -1)
        window.location.href = "tstruct.aspx?act=load&transid=" + tst + "&recordid=" + result + `&isDupTab=${callParentNew('isDuplicateTab')}`;
}

//function SuccessSaveCache(result, eventArgs) {
//    //ShowDimmer(true);
//    //AxWaitCursor(true);
//    //window.location.href = "tstruct.aspx?act=load&transid=" + tst + "&recordid=" + rid;
//}

function GetJsArrays() {
    var strJs = "";

    for (var i = 0; i < RowDcNo.length; i++) {
        strJs += "RowDcNo[" + i + "]='" + RowDcNo[i] + "';DbRowNo[" + i + "]='" + DbRowNo[i] + "';ClientRowNo[" + i + "]='" + ClientRowNo[i] + "';";
    }

    for (var j = 0; j < MasterRow.length; j++) {
        strJs += "MasterRow[" + j + "]='" + MasterRow[j] + "';";
    }

    for (var k = 0; k < ComboParentField.length; k++) {
        strJs += "ComboParentField[" + k + "]='" + ComboParentField[k] + "';ComboParentValue[" + k + "]='" + ComboParentValue[k] + "';";
        strJs += "ComboDepField[" + k + "]='" + ComboDepField[k] + "';ComboDepValue[" + k + "]='" + ComboDepValue[k] + "';";
    }
    return strJs;
}

//Added for navigation of TStruct from ListView
function GetRecord(nextPrev) {
    AxWaitCursor(true);
    ShowDimmer(true);
    var columnName = "";
    var dataRowIndex = -1;
    var isParentIview = false;
    if (window.opener && !window.opener.closed) {
        if (window.opener.parent.isParentIview)
            isParentIview = window.opener.parent.isParentIview;
        if (window.opener.parent.clickedColumn)
            columnName = window.opener.parent.clickedColumn;
        if (window.opener.parent.dataRowIndex)
            dataRowIndex = window.opener.parent.dataRowIndex;
    } else {
        if (window.parent.isParentIview)
            isParentIview = window.parent.isParentIview;
        if (window.parent.clickedColumn)
            columnName = window.parent.clickedColumn;
        if (window.parent.dataRowIndex)
            dataRowIndex = window.parent.dataRowIndex;
    }
    if (IfUnSavedChanges())
        return;
    var recordId = $j("#recordid000F0").val();
    try {
        ASB.WebService.GetRecord(nextPrev, recordId, tst, columnName, dataRowIndex, isParentIview, NextPrevTstructResult, OnException);
    } catch (ex) {}
}

function NextPrevTstructResult(result, eventArgs) {
    if (result.indexOf("This") != -1) {
        AxWaitCursor(false);
        ShowDimmer(false);
        return showAlertDialog("info", result);
    } else if (result == $j("#recordid000F0").val() || result == "") {
        AxWaitCursor(false);
        ShowDimmer(false);
        return showAlertDialog("error", 2019, "client");
    }
    if (CheckSessionTimeout(result)) {
        AxWaitCursor(false);
        ShowDimmer(false);
        return;
    } else if (result.indexOf("ivtstload.aspx") != -1) {
        var href = result.split('*¿*')[0];
        if (window.opener && !window.opener.closed)
            window.opener.parent.dataRowIndex = result.split('*¿*')[1];
        else
            window.parent.dataRowIndex = result.split('*¿*')[1];
        return window.location.href = href;
    } else if (result.indexOf("transid=") != -1) {
        var href = "tstruct.aspx?" + result.split('*¿*')[0];
        if (window.opener && !window.opener.closed)
            window.opener.parent.dataRowIndex = result.split('*¿*')[1];
        else
            window.parent.dataRowIndex = result.split('*¿*')[1];
        return window.location.href = href;
    } else {
        var href = "tstruct.aspx?transid=" + tst + "&recordid=" + result + `&isDupTab=${callParentNew('isDuplicateTab')}`;
        return window.location.href = href;
    }
    if (window.opener && window.opener.parent.tstructPop == false)
        window.opener.parent.tstructPop = true;
}

//Show and hide navigation buttons in TStruct
function ShowHideNexPrev() {
    var columnName = "";
    var dataRowIndex = -1;
    var isParentIview = false;
    if (window.parent.disableNavigation || (window.opener && !window.opener.closed && window.opener.parent.disableNavigation) || (window.opener && !window.opener.closed && document.referrer.indexOf("iview.aspx?") > -1)) {
        ShowHideNexPrevResult(false);
        if (!window.opener)
            window.parent.disableNavigation = false;
        else
            window.opener.parent.disableNavigation = false;
        return;
    } else if (!window.opener && document.referrer.indexOf("iview.aspx?") > -1) {
        if (document.referrer.indexOf("&homeicon=true") > -1)
            window.parent.homeHref = document.referrer;
        //else
        // window.parent.homeHref = document.referrer + "&homeicon=true";
        //} else if (!window.opener && document.referrer.indexOf("listIview.aspx?") > -1) {
    } else if (!window.opener && document.referrer.indexOf("iview.aspx?") > -1 && document.referrer.indexOf("&tstcaption") > -1) {
        if (document.referrer.indexOf("&homeicon=true") > -1)
            window.parent.homeHref = document.referrer;
        else
            window.parent.homeHref = document.referrer + "&homeicon=true";
    }
    if (window.opener && !window.opener.closed) {
        if (window.opener.parent.isParentIview)
            isParentIview = window.opener.parent.isParentIview;
        if (window.opener.parent.clickedColumn)
            columnName = window.opener.parent.clickedColumn;
        if (window.opener.parent.dataRowIndex)
            dataRowIndex = window.opener.parent.dataRowIndex;
    } else {
        if (window.parent.isParentIview)
            isParentIview = window.parent.isParentIview;
        if (window.parent.clickedColumn)
            columnName = window.parent.clickedColumn;
        if (window.parent.dataRowIndex)
            dataRowIndex = window.parent.dataRowIndex;
    }
    //try {
    //    ASB.WebService.SessionHasValue(tst, dataRowIndex, columnName, isParentIview, ShowHideNexPrevResult, OnException);
    //} catch (ex) {}
}

function ShowHideNexPrevResult(result, eventArgs) {
    if ($j("#goprior").length > 0 && $j("#gonext").length > 0) {
        if (!result) {
            $j("#goprior").parent().hide();
            $j("#gonext").parent().hide();
            $j("#backforwrdbuttons").show();
            $j("#backtohm").css('display', 'none');
            if (window.opener == null || !document.parent) {
                //                if (document.referrer.indexOf("iview.aspx?") > -1) {
                //                    $j("#backforwrdbuttons").hide();
                //                }
                //                else
                //                    $j("#backforwrdbuttons").show();
            }
        } else {
            $j("#goprior").parent().show();
            $j("#gonext").parent().show();
            $j("#nextprevicons").css('display', 'block');
            $j("#backforwrdbuttons").hide();
            if (!window.opener) {
                if (document.referrer.indexOf("iview.aspx?") > -1) {
                    $j("#nextprevicons").css('display', 'none');
                    $j("#backforwrdbuttons").show();
                    $j("#goprior").parent().hide();
                    $j("#gonext").parent().hide();
                } else {
                    $j("#backtohm").css({
                        'display': 'block',
                        'margin-left': '5px'
                    });
                    $j("#homeico").bind('click', function () {
                        document.location.href = window.parent.homeHref
                    });
                }
            }
        }
    }
}

function EnableDraftFeature() {
    var result = false;
    var dTstructsArr = new Array();
    if (window.parent.draftEnabledTstructs) {
        dTstructsArr = window.parent.draftEnabledTstructs.toLowerCase().split(',');
    } else {
        result = true;
    }
    var transid = tst.toLowerCase();
    if (dTstructsArr.length > 0) {
        $j.each(dTstructsArr, function (index, value) {
            if (result == false && value == transid) {
                result = true;
                return;
            }
        });
    }
    if (result)
        draftTimer = window.setInterval(SaveAsDraft, parseInt(window.parent.draftSaveTime, 10) * 1000);
}

//Add multiple rows in grid dc
function AddRows(dcNo) {
    var noOfRows = $j("#txtRowsToAdd" + dcNo).val();
    if (noOfRows && !isNaN(noOfRows)) {
        var rno = parseInt(noOfRows, 10);
        while (rno > 0) {
            AddRow(dcNo);
            rno--;
        }
    } else {
        AddRow(dcNo);
    }
}

//for Add rows validation 
function ValidateAddRows(value, min, max) {
    if (parseInt(value, 10) < min || isNaN(value))
        return min;
    else if (parseInt(value, 10) > max)
        return max;
    else return value;
}

//Region Axpert Web Search feature
function OpenSearchPop(e) {
    if ((e.ctrlKey || e.metaKey) && e.keyCode === 70) {
        if ($j("#searchFields").is(':visible') || (!$j('.ui-dialog').is(':visible') && !$j("#searchFields").is(':visible'))) {
            e.preventDefault();
            $j("#srchDynamicText").val('');
            $j("#searchFields").show();
            $j("#searchFields").animate({
                top: "0px"
            });
        }
    } else if (e.keyCode == 27) {
        //CloseSearchPop();

        $j("#searchFields").animate({
            top: "-50px"
        }, 500);
        setTimeout(function () {
            $j("#searchFields").hide();
        }, 500);
    }
}
var AxSearchArray = new Array();
var idxSearchFoc = 0;

function FindTstructString(searchTerm) {
    AxSearchArray = new Array();
    UnSelectSearchItem();
    if (searchTerm == "") {
        $j("#searchResCount").text("0 of 0");
        return;
    }
    $j('#pagebdy').find(':input, textarea, select').not('[id=srchDynamicText],[id=ddlSearch],[type=button],input[type=hidden],[class^=frmAtt],[class="ui-tabs-hide"]').each(function () {
        if ($j(this).attr('type') == "hidden")
            return;

        var id = $j(this).attr('id');
        if (id) {
            var dcNo = GetFieldsDcNo(id);
            if (IsDcPopGrid(dcNo))
                return;
        }

        var txtn = $j(this).val().toString().trim().toLowerCase();
        if ($j(this).is('select'))
            txtn = $j("#" + id + " :selected").text().trim().toLowerCase();

        if (txtn.indexOf(searchTerm) > -1 && !$j(this).is(":focus"))
            AxSearchArray.push($j(this));
        if (AxSearchArray.length > 0 && !AxSearchArray[0].parent().hasClass('picklist'))
            $j(AxSearchArray[0]).addClass('srchselectedfield');
        else if (AxSearchArray.length > 0 && AxSearchArray[0].parent().hasClass('picklist'))
            AxSearchArray[0].parent().addClass('srchselectedfield');
        idxSearchFoc = 0;
        if (AxSearchArray.length > 0)
            $j("#searchResCount").text("1 of " + AxSearchArray.length);
        else
            $j("#searchResCount").text("0 of 0");
    });
}

function UnSelectSearchItem(operation) {
    typeof operation == 'undefined' ? operation = "" : operation = operation;
    $j('input:not([id=srchDynamicText],[type=button]), select, textarea').each(function () {
        if ($j(this).hasClass('srchselectedfield'))
            $j(this).removeClass('srchselectedfield');
        else if ($j(this).parent().hasClass('picklist', 'srchselectedfield'))
            $j(this).parent().removeClass('srchselectedfield');
    });
    if (operation != "") {
        $j("#searchFields").animate({
            top: "-50px"
        }, 500);
        setTimeout(function () {
            $j("#searchFields").hide();
        }, 500);
    }
}

function MoveNext() {
    if (idxSearchFoc < AxSearchArray.length - 1) {
        idxSearchFoc++;
        var dcNo = 0;
        if ($j(AxSearchArray[idxSearchFoc]).attr('id'))
            dcNo = GetFieldsDcNo($j(AxSearchArray[idxSearchFoc]).attr('id'));
        var isThisFldPicklist = $j(AxSearchArray[idxSearchFoc]).parent().hasClass('picklist');
        var isPrevFldPicklist = $j(AxSearchArray[idxSearchFoc - 1]).parent().hasClass('picklist');
        if (isThisFldPicklist && !isPrevFldPicklist) {
            $j(AxSearchArray[idxSearchFoc]).parent().addClass('srchselectedfield');
            AxSearchArray[idxSearchFoc - 1].removeClass('srchselectedfield');
            // if (TabDCs.indexOf(dcNo) > -1)
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
        } else if (isThisFldPicklist && isPrevFldPicklist) {
            $j(AxSearchArray[idxSearchFoc]).parent().addClass('srchselectedfield');
            $j(AxSearchArray[idxSearchFoc - 1]).parent().removeClass('srchselectedfield');
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
        } else if (!isThisFldPicklist && isPrevFldPicklist) {
            AxSearchArray[idxSearchFoc].addClass('srchselectedfield');
            $j(AxSearchArray[idxSearchFoc - 1]).parent().removeClass('srchselectedfield');
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
        } else {
            AxSearchArray[idxSearchFoc].addClass('srchselectedfield');
            AxSearchArray[idxSearchFoc - 1].removeClass('srchselectedfield');
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
        }
        $j("#searchResCount").text(idxSearchFoc + 1 + " of " + AxSearchArray.length);
        AxSearchArray[idxSearchFoc].focus();
    } else if (idxSearchFoc == AxSearchArray.length - 1) {
        AxSearchArray[idxSearchFoc].focus();
    }
}

function MovePrev() {
    var visDcname = GetOpenTabDcs();
    if (idxSearchFoc > 0 && AxSearchArray.length >= idxSearchFoc) {
        idxSearchFoc--;
        var dcNo = 0;
        if ($j(AxSearchArray[idxSearchFoc]).attr('id'))
            dcNo = GetFieldsDcNo($j(AxSearchArray[idxSearchFoc]).attr('id'));
        var isThisFldPicklist = $j(AxSearchArray[idxSearchFoc]).parent().hasClass('picklist');
        var isNxtFldPicklist = $j(AxSearchArray[idxSearchFoc + 1]).parent().hasClass('picklist');
        if (isThisFldPicklist && !isNxtFldPicklist) {
            $j(AxSearchArray[idxSearchFoc]).parent().addClass('srchselectedfield');
            AxSearchArray[idxSearchFoc + 1].removeClass('srchselectedfield');
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
            AxSearchArray[idxSearchFoc].focus();
        } else if (isThisFldPicklist && isNxtFldPicklist) {
            $j(AxSearchArray[idxSearchFoc]).parent().addClass('srchselectedfield');
            $j(AxSearchArray[idxSearchFoc + 1]).parent().removeClass('srchselectedfield');
            AxSearchArray[idxSearchFoc].focus();
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
        } else if (!isThisFldPicklist && isNxtFldPicklist) {
            AxSearchArray[idxSearchFoc].addClass('srchselectedfield');
            $j(AxSearchArray[idxSearchFoc + 1]).parent().removeClass('srchselectedfield');
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
            AxSearchArray[idxSearchFoc].focus();
        } else {
            AxSearchArray[idxSearchFoc].addClass('srchselectedfield');
            AxSearchArray[idxSearchFoc + 1].removeClass('srchselectedfield');
            if ($j.inArray(dcNo, TabDCs) > -1)
                $j("#ank" + dcNo).click();
            AxSearchArray[idxSearchFoc].focus();
        }
        $j("#searchResCount").text(idxSearchFoc + 1 + " of " + AxSearchArray.length);
    } else if (idxSearchFoc == AxSearchArray.length - 1) {
        AxSearchArray[idxSearchFoc].focus();
    }
}

function CloseSearchPop() {
    var isSearchDlg = $j("#searchFields").is(':visible');
    var isTabbPressed = false;
    //Closing search dialog on tab dc open
    if (isSearchDlg && TabDCs.length > 0 && $j(AxSearchArray[idxSearchFoc]).attr('id') && TabDCs.indexOf(GetFieldsDcNo($j(AxSearchArray[idxSearchFoc]).attr('id'))) == -1)
        isTabbPressed = true;

    if ((isSearchDlg && TabDCs.length == 0 && (!event || event.type != "blur")) || isTabbPressed)
        $j("#searchFields").dialog('close');
}

//Function for CkRichTextEditor
function ShowdivContentCK(objId, isBlur) {
    //var dvObj = $j("#" + obj.id);
    if (currentCK != "") {
        if (isBlur) {
            objId = objId.name;
        } else {
            objId = objId.attr("name");
        }
        var dvHtml = CKEDITOR.instances[objId].getData();
        /*dvHtml = dvHtml.replace(/<img/g, '<gmi');*/
        dvHtml = TagReplaceCharForRTB(dvHtml);
        try {
            if (dvHtml != "") {
                let checkRW = "";
                if (dvHtml.indexOf("***") != -1) {
                    checkRW = "***";
                } else if (dvHtml.indexOf("**") != -1) {
                    checkRW = "**";
                } else if (dvHtml.indexOf("###") != -1) {
                    checkRW = "###";
                } else if (dvHtml.indexOf("`") != -1) {
                    checkRW = "`";
                } else if (dvHtml.indexOf("*$*") != -1) {
                    checkRW = "*$*";
                }
                if (checkRW != "") {
                    showAlertDialog("error", 2053, "client", "^♠^" + checkRW);
                    dvHtml = dvHtml.replace(/\*\*\*/g, " ");
                    dvHtml = dvHtml.replace(/\*\*/g, " ");
                    dvHtml = dvHtml.replace(/###/g, " ");
                    dvHtml = dvHtml.replace(/`/g, " ");
                    dvHtml = dvHtml.replace(/\*\$\*/g, " ");
                }
            }
        } catch (ex) { }

        $j("#" + objId).val(dvHtml);
        MainBlur($j("#" + objId));
        currentCK = "";
    }
}



//Grid Attachment Functions

var AxGridAttField = "";

function ShowGridAttachPopUp(obj) {
    if ($(obj).hasClass("disabledClickEvent")) {
        return false;
    }
    $(obj).addClass("disabledClickEvent");
    if ($(obj).parent().find(".attach-files").children().length > 0 && AxpFileUploadlmt != "0") {
        //var upFileCount = $(obj).parent().find(".attachment-count").text();

        var upFileCount = $(obj).parent().find(".attach-files").children().length;
        if (upFileCount >= AxpFileUploadlmt) {
            showAlertDialog("warning", "Uploaded file count reached to limit of file upload.");
            CallbackFunctionBootstrap();
            return;
        }
    }
    GetCurrentTime("Tstruct grid file upload button click");
    AxGridAttField = obj.id;
    var rFNo = GetFieldsRowFrameNo(AxGridAttField);
    var gridHl = "grdAtt_hlnk_" + rFNo;
    var hlinkidNongrid = obj.id;
    let attFldId = $("#" + AxGridAttField).parent("div").siblings("input.grdAttach").attr("id");
    let attFldName =GetFieldsName(attFldId);
    var fNo = GetFieldsDcNo(AxGridAttField);
    var atFname = GetFieldValue("axpattach_filename" + fNo + rFNo);
    var src = "";
    if (atFname != "")
        src = "./gridfileupload.aspx?attFld=" + gridHl + "&attTransId=" + transid + "&attFldName=" + attFldName + "&atFname=" + atFname + "&dcNo=" + fNo;
    else if (IsGridField(attFldName) == false)
        src = "./gridfileupload.aspx?attFld=" + hlinkidNongrid + "&attTransId=" + transid + "&attFldName=" + attFldName + "&dcNo=" + fNo;
    else
        src = "./gridfileupload.aspx?attFld=" + gridHl + "&attTransId=" + transid + "&attFldName=" + attFldName + "&dcNo=" + fNo;
    displayBootstrapModalDialog("Grid Fileupload", "xs", "156px", true, src, "", "", CallbackFunctionBootstrap)

    function CallbackFunctionBootstrap() {
        $(obj).removeClass('disabledClickEvent');
        $("#" + obj.id).focus(); //to focus on the upload icon field once dialog closes
        try {
            if (axGridAttSavedPath != "") {
                ASB.WebService.IsGridAttFileExists(axGridAttSavedPath, CallBackOnGridAttFile);

                function CallBackOnGridAttFile(result, eventArgs) {
                    if (result == "false") {
                        showAlertDialog("error", eval(callParent('lcm[515]')));
                    }
                }
                UpdateExceptionMessageInET("Final file upload path at client side : "+axGridAttSavedPath);
                GetTotalElapsTime();
                axGridAttSavedPath = "";
            }
            else{
                UpdateExceptionMessageInET("Final file upload is empty in CallbackFunctionBootstrap");
                GetTotalElapsTime();
            }
        } catch (ex) {
            UpdateExceptionMessageInET("File upload in CallbackFunctionBootstrap: "+ex.message);
            GetTotalElapsTime();
            axGridAttSavedPath = "";
        }
    }
    //displayIFrameModalDialog("Grid Fileupload", src, "xs", "");

}

function ShowAxpFileAttachPopUp(obj) {
    if ($(obj).hasClass("disabledClickEvent")) {
        return false;
    }
    $(obj).addClass("disabledClickEvent");
    if ($(obj).parent().find(".attach-files").children().length > 0 && AxpFileUploadlmt != "0") {
        var upFileCount = $(obj).parent().find(".attach-files").children().length;
        if (upFileCount >= AxpFileUploadlmt) {
            showAlertDialog("warning", "Uploaded file count reached to limit of file upload.");
            CallbackFunctionBootstrap();
            return;
        }
    }
    AxGridAttField = obj.id;
    let attFldId = $("#" + AxGridAttField).parents("div").siblings("input.grdAttach").attr("id");
    let attFldName = attFldId.substring(0, attFldId.lastIndexOf("F") - 3);
    let fNo = GetFieldsDcNo(AxGridAttField);
    if (IsGridField(attFldName)) {
        if (axInlineGridEdit && !$(obj).parents("tr").hasClass("inline-edit")) {
            $(obj).removeClass('disabledClickEvent');
            return;
        }
    }
    var src = "./axpfileupload.aspx?attFld=" + AxGridAttField + "&attFldName=" + attFldName + "&dcNo=" + fNo;
    displayBootstrapModalDialog("Fileupload", "lg", "300px", true, src, "", "", CallbackFunctionBootstrapAxpFile);

    function CallbackFunctionBootstrapAxpFile() {
        $(obj).removeClass('disabledClickEvent');
        $("#" + obj.id).focus(); //to focus on the upload icon field once dialog closes
        try {
            if (axpFileSavedPath != "") {
                ASB.WebService.IsAxpFileExists(axpFileSavedPath, CallBackOnAxpFile);

                function CallBackOnAxpFile(result, eventArgs) {
                    if (result == "false") {
                        showAlertDialog("error", eval(callParent('lcm[515]')));
                    }
                }
                axpFileSavedPath = "";
            }
        } catch (ex) {
            axpFileSavedPath = "";
        }
        try {
            axAfterFileupload();
        } catch (ex) {

        }
    }
}

function ShowGridAttLink(src) {
    src = src.replace(/♠/g, "\'");
    src = unescape(src); //to show decode special characters
    var idx = src.lastIndexOf("/");
    if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
    let openFileName = src.split("/").pop();
    let isFileMissed = false;
    if (typeof AxGridAttNotExistList != "undefined" && openFileName != "") {
        if (jQuery.inArray(openFileName, AxGridAttNotExistList) != -1)
            isFileMissed = true;
    }
    if (isFileMissed)
        showAlertDialog("error", eval(callParent('lcm[516]')));
    else {
        if (src != "" && src.endsWith('.zip')) {
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
            if (protectFile == "true") {
                const _zipfileName = src.substring(src.lastIndexOf('/') + 1);
                ASB.WebService.LoadAxpFileToScript("", _zipfileName, CallBackOnAxpFile);
                function CallBackOnAxpFile(furesult, eventArgs) {
                    if (typeof furesult != "undefined" && furesult != "" && furesult.indexOf("true:") > -1) {
                        let newFileName = furesult.startsWith("true:") ? furesult.substring(5) : furesult;
                        let lastSlashIndex = src.lastIndexOf("/");
                        let updatedSrc = src.substring(0, lastSlashIndex + 1) + newFileName;
                        src = unescape(updatedSrc);
                        var idx = src.lastIndexOf("/");
                        if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
                        if (isMobile) {
                            let _fileName = src.match(/\/([^\/?#]+)$/)[1];
                            OpenPdfFile(_fileName, "", "", "", false);
                        } else {
                            window.open(src, "GridDcUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
                        }
                    }
                }
            } else
                window.open(src, "GridDcUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
        } else
            window.open(src, "GridDcUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
    }
}

function ShowAxpFileuploadLink(fldName, pathSrc, src, event) {
    event.stopPropagation();
    pathSrc = pathSrc.replace(/♠/g, "\'");
    src = src.replace(/♠/g, "\'");
    let axpFName = fldName.substr(7);
    var axpPath = "";
    if (typeof $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0] != "undefined")
        axpPath = $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value;
    else {
        if ($("input[id*=" + axpFName + "]").hasClass("axpFilePathFld")) {
            $("input[id*=" + axpFName + "]").each(function () {
                if ($(this).hasClass("axpFilePathFld"))
                    axpPath = $(this).val();
            });
        }
    }

    ASB.WebService.LoadAxpFileToScript(axpPath, pathSrc, CallBackOnAxpFile);

    function CallBackOnAxpFile(furesult, eventArgs) {
        if (typeof furesult != "undefined" && furesult != "" && furesult.indexOf("true:") > -1) {
            let newFileName = furesult.startsWith("true:") ? furesult.substring(5) : furesult;
            let lastSlashIndex = src.lastIndexOf("/");
            let updatedSrc = src.substring(0, lastSlashIndex + 1) + newFileName;
            src = unescape(updatedSrc);
            var idx = src.lastIndexOf("/");
            if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
            if (isMobile) {
                let _fileName = src.match(/\/([^\/?#]+)$/)[1];
                OpenPdfFile(_fileName, "", "", "", false);
            } else {
                window.open(src, "GridUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
            }

        } else {
            src = unescape(src); //to show decode special characters
            var idx = src.lastIndexOf("/");
            if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
            if (isMobile) {
                let _fileName = src.match(/\/([^\/?#]+)$/)[1];
                OpenPdfFile(_fileName, "", "", "", false);
            } else {
                window.open(src, "GridUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
            }
        }
    }
}

var fileLinks = "";

function SetGridAttValue(fldName, fldValue) {

    var rowFrmNo = GetFieldsRowFrameNo(fldName);
    var fNo = GetFieldsDcNo(fldName);
    var rid = $j("#recordid000F0").val();
    var hlObj = $j("#grdAtt_hlnk_" + rowFrmNo);
    fileLinks = "";
    if (fldValue != "") {
        if (fldValue.indexOf("@") != -1) {
            fldValue = fldValue.substr(fldValue.lastIndexOf(")") + 2);
            $("input#" + fldName).attr("value", fldValue).text(fldValue).val(fldValue);
            isGridFileUploadOnLoad = true;
        }else{
            $("input#" + fldName).attr("value", fldValue).text(fldValue).val(fldValue);
            isGridFileUploadOnLoad = true;
        }
    }
}

//Constructing Html for GridAttachments
function ConstructGridAttachHTML(fldName, fNo, fldValue, hdnValue, rowFrmNo) {
    var filePath = "",
        linkHtml = "";
    var rowFrmNo = GetFieldsRowFrameNo(fldName);
    filePath = hdnValue.val() + "axpert/" + sid + "/";
    var arrFileNames = fldValue.split(',');
    for (var i = 0; i < arrFileNames.length; i++)
        if (arrFileNames[i] != undefined || arrFileNames[i] != "") {
            var finalFileName = arrFileNames[i].replace(/\s/g, '♠').replace(/\(/g, '♦').replace(/\)/g, '♣');
            let fileOpenLink = arrFileNames[i];
            fileOpenLink = fileOpenLink.replace(/'/g, '♠');
            fileLinks += "<div id=\"Link_" + rowFrmNo + "_" + finalFileName + "\"><a onclick=\"DeleteFileFromRow('" + fldName + "','" + rowFrmNo + "','" + fileOpenLink + "')\"><i class=\"glyphicon glyphicon-remove close icon-arrows-remove attachmentcrossicon\"></i></a><a href=\"javascript:void(0)\"  tabindex=\"-1\" class='grdAttach handCur' onclick='ShowGridAttLink(\"" + filePath + fileOpenLink + "\")'>" + arrFileNames[i] + "</a></div>";
        }

    fileLinks = "<div id='grdAtt_hlnk_" + rowFrmNo + "attach' class='attach-files' style='display:none'>" + fileLinks + "</div>"
    if (!isMobile)
        fileLinks += "<a class='pull-" + (parent.gllangType === "ar" ? "left" : "right") + " attachment-count' href='javascript:void(0)' class=''><span data-target='grdAtt_hlnk_" + rowFrmNo + "attach' class=' attach-popover' data-container='#bootstrapModal' data-placement='bottom'>" + arrFileNames.length + "+</span></a>";
    else
        fileLinks += "<a class='pull-" + (parent.gllangType === "ar" ? "left" : "right") + " attachment-count' href='javascript:void(0)' class=''><span data-target='grdAtt_hlnk_" + rowFrmNo + "attach' class=' attach-popover' data-container='.mobilewrapperForEditFields' data-placement='bottom'>" + arrFileNames.length + "+</span></a>";

    return fileLinks;

}

function ConstructAxpFileAttachHTML(fldName, fNo, fldValue, hdnValue, rowFrmNo) {
    var filePath = "",
        linkHtml = "";
    var rowFrmNo = GetFieldsRowFrameNo(fldName);

    let axpFName = fldName.substr(7);
    var axpPath = "";
    if (typeof $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0] != "undefined")
        axpPath = $("textarea[id*=" + axpFName + "]").not(".axpAttach")[0].value;
    else
        axpPath = $("input[id*=" + axpFName + "]").not(".axpAttach")[0].value;

    filePath = hdnValue.val() + "axpert/" + sid + "/";
    var arrFileNames = fldValue.split(',');
    for (var i = 0; i < arrFileNames.length; i++) {
        if (arrFileNames[i] != "") {
            var showFldValue = arrFileNames[i];
            if (axpPath != "" && axpPath.endsWith("\\*"))
                showFldValue = showFldValue.substr(20);

            var finalFileName = arrFileNames[i].replace(/\s/g, '♠').replace(/\(/g, '♦').replace(/\)/g, '♣');
            let fileOpenLink = arrFileNames[i];
            fileOpenLink = fileOpenLink.replace(/'/g, '♠');
            fileLinks += "<div id=\"Link_" + rowFrmNo + "_" + finalFileName + "\"><a onclick=\"DeleteFileFromRow('" + fldName + "','" + rowFrmNo + "','" + fileOpenLink + "')\"><i class=\"glyphicon glyphicon-remove close icon-arrows-remove attachmentcrossicon\"></i></a><a href=\"javascript:void(0)\"  tabindex=\"-1\" class='grdAttach handCur' onclick='ShowAxpFileuploadLink(\"" + fldName + "\",\"" + arrFileNames[i] + "\",\"" + filePath + fileOpenLink + "\")'>" + showFldValue + "</a></div>";
        }
    }
    fileLinks = "<div id='axpFileAtt_hlnk_" + rowFrmNo + "attach' class='attach-files' style='display:none'>" + fileLinks + "</div>"
    if (!isMobile)
        fileLinks += "<a class='pull-" + (parent.gllangType === "ar" ? "left" : "right") + " attachment-count' href='javascript:void(0)' class=''><span data-target='axpFileAtt_hlnk_" + rowFrmNo + "attach' class=' attach-popover' data-container='#bootstrapModal' data-placement='bottom'>" + arrFileNames.length + "+</span></a>";
    else
        fileLinks += "<a class='pull-" + (parent.gllangType === "ar" ? "left" : "right") + " attachment-count' href='javascript:void(0)' class=''><span data-target='axpFileAtt_hlnk_" + rowFrmNo + "attach' class=' attach-popover' data-container='.mobilewrapperForEditFields' data-placement='bottom'>" + arrFileNames.length + "+</span></a>";
    return fileLinks;
}

//Constructing Html for Attachments 
function ConstructAttachHTML(fldValue, fldName, calledfrom) {

    var FileLink = "";
    var ParentDiv = "";
    var arrFileNames = fldValue.split(',');

    var rowFrmNo = GetFieldsRowFrameNo(fldName);
    var rowNo = GetFieldsRowNo(fldName);

    var dcNo = fldName.substring(fldName.lastIndexOf("F") + 1, fldName.length);

    var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
    filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
    $("input#" + fldName).attr("value", fldValue).text(fldValue).val(fldValue);
}

//Constructing Html for Attachments (fieldname starts with "axpfile_")
function ConstructAxpAttachHTML(fldValue, fldName, calledfrom) {

    var FileLink = "";
    var ParentDiv = "";
    var arrFileNames = fldValue.split(',');

    var rowFrmNo = GetFieldsRowFrameNo(fldName);
    var rowNo = GetFieldsRowNo(fldName);

    var dcNo = fldName.substring(fldName.lastIndexOf("F") + 1, fldName.length);

    var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
    filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
    $("input#" + fldName).attr("value", fldValue).text(fldValue).val(fldValue);
    isGridFileUploadOnLoad = true;
}

function GetAttachControlHeight(ftsize) {
    let minHght = 20;
    if (typeof ftsize != "undefined") {
        ftsize = parseInt(ftsize.slice(0, -2));
        if (ftsize <= 14)
            minHght = 20;
        else if (ftsize == 15 || ftsize == 16) {
            minHght = ftsize + 6;
        } else if (ftsize > 16) {
            let flotval = Math.floor(((ftsize - 14) / 2) + 0.5);
            minHght = ftsize + 5 + flotval;
        }
    }
    return minHght;
}


//Delete attachments
function DeleteFileFromRow(fldName, fNo, fldValue) {
    fldValue = fldValue.replace(/♠/g, "\'");
    var hasFolderReference = "false";
    var hasReferImages = "false";
    var referFieldValue = "";
    var cutMsg = eval(callParent('lcm[62]'));
    var glType = eval(callParent('gllangType'));
    var isRTL = false;
    if (glType == "ar")
        isRTL = true;
    else
        isRTL = false;
    var DeleteFileFromRowCB = $.confirm({
        theme: 'modern',
        closeIcon: false,
        rtl: isRTL,
        title: eval(callParent('lcm[155]')),
        onContentReady: function () {
            disableBackDrop('bind');
        },
        backgroundDismiss: true,
        escapeKey: 'buttonB',
        content: cutMsg,
        buttons: {
            buttonA: {
                text: eval(callParent('lcm[164]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    DeleteFileFromRowCB.close();
                    var currentRowValue = $j('input#' + fldName).val(); //$j('#' + fldName).val();
                    var rowNo = GetFieldsRowNo(fldName);
                    var rowFrmNo = GetFieldsRowFrameNo(fldName);

                    if (fldName.indexOf("_referimages") != -1) {

                        if (currentRowValue.indexOf("(") != -1) {
                            hasReferImages = "true";
                            var nFileSIndx = currentRowValue.indexOf("(");
                            var nFileEIndx = currentRowValue.indexOf(")");
                            referFieldValue = currentRowValue.substr(0, nFileSIndx + 1);
                            currentRowValue = currentRowValue.substr(nFileSIndx + 1, (nFileEIndx - (nFileSIndx + 1)));
                        } else {
                            hasFolderReference = "true";
                            referFieldValue = currentRowValue.substr(0, currentRowValue.lastIndexOf("\\") + 1); //Getting folderpath
                            currentRowValue = currentRowValue.substr(currentRowValue.lastIndexOf("\\") + 1); //getting filenames
                        }
                    }
                    fldValue = unescape(fldValue);
                    currentRowValue = DeleteAttachFiles(currentRowValue, hasFolderReference, hasReferImages, referFieldValue, fldValue)
                    UpdateFieldArray(fldName, rowNo, currentRowValue, "gridattachment", "parent");
                    UpdateAllFieldValues(fldName, currentRowValue);
                    var FullfileName = fldValue.replace(/\s/g, '♠').replace(/\(/g, '♦').replace(/\)/g, '♣');
                    if (rowNo == "000")
                        if (fldName.toLowerCase().startsWith("axpfile_"))
                            $("input#" + fldName).siblings(".divNonGridAxpFile").find($j('div[id$="' + rowFrmNo + "_" + FullfileName + '"]')).remove();
                        else
                            $("input#" + fldName).siblings(".divnonattach").find($j('div[id$="' + rowFrmNo + "_" + FullfileName + '"]')).remove();
                    else
                        $("input#" + fldName).siblings(".grdattch").find($j('div[id$="' + rowFrmNo + "_" + FullfileName + '"]')).remove();
                    if (fldName.toLowerCase().startsWith("axpfile_")) {
                        DeletedFieldValue.push(fldName + "~" + fldValue);
                        ManageAxpDeleteFiles(fldName, fldValue);
                    } else
                        ManageLblOfDltFiles(fldName, fldValue);
                    $j('#' + fldName).val(currentRowValue);
                    //$j('#attachWrapper' + fldValue.replace(".", "")).remove();

                    //if (axInlineGridEdit) //to update the attachment label count after a file deleting from the inline grid popover
                    //  updateAttachmentLabelCount(rowFrmNo);
                }
            },
            buttonB: {
                text: eval(callParent('lcm[192]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                }
            },

        }
    });

}

function DeleteAttachFiles(currentRowValue, hasFolderReference, hasReferImages, referFieldValue, fldValue) {

    var arrFiles = currentRowValue.split(',');
    if (arrFiles != undefined) {
        arrFiles = $j.grep(arrFiles, function (value) {
            return value != fldValue;
        });
    }

    if (hasFolderReference == "true") { //specified imagepath with reference
        if (arrFiles.length > 0) {
            currentRowValue = referFieldValue + arrFiles.join(",");
        } else
            currentRowValue = "";
    } else if (hasReferImages == "true") { //Reference without imagepath
        currentRowValue = referFieldValue + arrFiles.join(",") + ")";
    } else
        currentRowValue = arrFiles.join(","); //Without reference

    return currentRowValue;
}

//For fill the label after file is deleted
function ManageLblOfDltFiles(fldName, fldValue) {

    var rowFrmNo = GetFieldsRowFrameNo(fldName);
    var rowNo = GetFieldsRowNo(fldName);
    var grdlistnames = new Array();
    var ParentDivVal = $("input#" + fldName).val();
    //Oldfiles is just for retrieving all the files name before deleting
    var Oldfiles = $("input#" + fldName).val();
    var dcNo = fldName.substring(fldName.lastIndexOf("F") + 1, fldName.length);

    ParentDivVal = ParentDivVal.replace(fldValue + ",", "").replace("," + fldValue, "").replace(fldValue, "");
    $("input#" + fldName).removeAttr("value").attr("value", ParentDivVal).val(ParentDivVal).text(ParentDivVal);

    if (rowNo == "000") {
        var NonGridAttr = $("#" + fldName).siblings(".divnonattach").attr("data-morefiles"); //$(".divnonattach").attr("data-morefiles");
        Oldfiles.split(',').forEach(function (val) {
            NonGridAttr.split(',').forEach(function (attr) {
                var attrname = attr.substring(("Link_" + rowFrmNo + "_").length, attr.length);
                if (val != attrname) {
                    if (val == fldValue) {
                        for (i = 0; i < $("#" + fldName).siblings(".divnonattach").find(".attach-files").children().length; i++) {

                            if ($("#" + fldName).siblings(".divnonattach").find(".attach-files").children()[i].id == attr) {
                                $($("#" + fldName).siblings(".divnonattach").find(".attach-files").children()[i]).removeClass('revrseInlinediv');

                                if ($("#" + fldName).siblings(".divnonattach").find(".attach-files").height() > 22) {
                                    $($("#" + fldName).siblings(".divnonattach").find(".attach-files").children()[i]).addClass('revrseInlinediv');
                                } else {

                                    var UpdateAttr = $("#" + fldName).siblings(".divnonattach").attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                                    $("#" + fldName).siblings(".divnonattach").attr("data-morefiles", UpdateAttr);
                                    if ($("#" + fldName).siblings(".divnonattach").attr("data-morefiles") == "") {
                                        $("#" + fldName).siblings(".divnonattach").find(".attachment-count").addClass("revrseInlinediv");
                                    }
                                }
                            }
                        }
                    }
                    if (fldValue == attrname.replace(/♠/g, ' ').replace(/♦/g, '(').replace(/♣/g, ')')) {
                        var UpdateAttr2 = $("#" + fldName).siblings(".divnonattach").attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                        $("#" + fldName).siblings(".divnonattach").attr("data-morefiles", UpdateAttr2);
                        if ($("#" + fldName).siblings(".divnonattach").attr("data-morefiles") == "") {
                            $("#" + fldName).siblings(".divnonattach").find(".attachment-count").addClass("revrseInlinediv");
                            $(".popover").css("display", "none");
                        }
                    }
                }
            });
            if (val == fldValue) {
                var attr1 = "Link_" + rowFrmNo + "_" + fldValue;
                attr1 = attr1.replace(/\s/g, '♠').replace(/\(/g, '♦').replace(/\)/g, '♣');
                var UpdateAttr3 = $("#" + fldName).siblings(".divnonattach").attr("data-morefiles").replace(attr1 + ",", "").replace("," + attr1, "").replace(attr1, "");
                $("#" + fldName).siblings(".divnonattach").attr("data-morefiles", UpdateAttr3);
                if ($("#" + fldName).siblings(".divnonattach").attr("data-morefiles") == "") {
                    $("#" + fldName).siblings(".divnonattach").find(".attachment-count").addClass("revrseInlinediv");
                    $(".popover").css("display", "none");
                }
            }

        });
        var datafiles = $("#" + fldName).siblings(".divnonattach").attr("data-morefiles");
        $("#" + fldName).siblings(".divnonattach").find(".attachment-count span").text(datafiles.split(',').length + "+");
    } else {
        if (AxpGridForm == "popup" && isMobile) {
            $("#GridAttach" + rowFrmNo).attr("data-morefiles", $("#GridAttach" + rowFrmNo).siblings("input").val());
        }
        var GridAttr = $("#GridAttach" + rowFrmNo).attr("data-morefiles");
        Oldfiles && Oldfiles.split(',').forEach(function (val) {
            GridAttr && GridAttr.split(',').forEach(function (attr) {
                var grdattrname = attr.substring(("Link_" + rowFrmNo + "_").length, attr.length);
                if (val != grdattrname) {
                    if (val == fldValue) {
                        for (i = 0; i < $(".attach-files").parent("#GridAttach" + rowFrmNo).find(".attach-files").children().length; i++) {

                            if ($(".attach-files").parent("#GridAttach" + rowFrmNo).find(".attach-files").children()[i].id == attr) {
                                $($(".attach-files").parent("#GridAttach" + rowFrmNo).find(".attach-files").children()[i]).removeClass('revrseInlinediv');
                                let ftsize = $(".attach-files").parent("#GridAttach" + rowFrmNo).find(".attach-files").css('font-size');
                                let minHght = GetAttachControlHeight(ftsize);
                                if ($(".attach-files").parent("#GridAttach" + rowFrmNo).find(".attach-files")[0].offsetHeight > minHght) {
                                    $($(".attach-files").parent("#GridAttach" + rowFrmNo).find(".attach-files").children()[i]).addClass('revrseInlinediv');
                                } else {

                                    var UpdateGrdAttr = $("#GridAttach" + rowFrmNo).attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                                    $("#GridAttach" + rowFrmNo).attr("data-morefiles", UpdateGrdAttr);
                                    if ($("#GridAttach" + rowFrmNo).attr("data-morefiles") == "") {
                                        $("#GridAttach" + rowFrmNo).find(".attachment-count").addClass("revrseInlinediv");
                                    }

                                }
                            }
                        }
                    }
                    if (fldValue == grdattrname.replace(/♠/g, ' ').replace(/♦/g, '(').replace(/♣/g, ')')) {
                        var UpdateGrdAttr2 = $("#GridAttach" + rowFrmNo).attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                        $("#GridAttach" + rowFrmNo).attr("data-morefiles", UpdateGrdAttr2);
                        if ($("#GridAttach" + rowFrmNo).attr("data-morefiles") == "") {
                            $("#GridAttach" + rowFrmNo).find(".attachment-count").addClass("revrseInlinediv");
                            $(".popover").css("display", "none");
                        }
                    }

                }
            });

            if (val == fldValue) {
                $("textarea#" + fldName).text(ParentDivVal).val(ParentDivVal);
                //$("#divDc" + dcNo).find("textarea[data-type=gridattach]").siblings("#" + fldName).attr("value", ParentDivVal);
            }
        });
        var grddatafiles = $("#GridAttach" + rowFrmNo).attr("data-morefiles");
        grddatafiles && $("#GridAttach" + rowFrmNo).find(".attachment-count span").text(grddatafiles.split(',').length + "+");
    }
}

//For fill the label after file is deleted (fieldname starts with "AxpFile_")
function ManageAxpDeleteFiles(fldName, fldValue) {
    var rowFrmNo = GetFieldsRowFrameNo(fldName);
    var rowNo = GetFieldsRowNo(fldName);
    var grdlistnames = new Array();
    var ParentDivVal = $("input#" + fldName).val();
    //Oldfiles is just for retrieving all the files name before deleting
    var Oldfiles = $("input#" + fldName).val();
    var dcNo = fldName.substring(fldName.lastIndexOf("F") + 1, fldName.length);

    ParentDivVal = ParentDivVal.replace(fldValue + ",", "").replace("," + fldValue, "").replace(fldValue, "");
    $("input#" + fldName).removeAttr("value").attr("value", ParentDivVal).val(ParentDivVal).text(ParentDivVal);

    if (rowNo == "000") {
        var NonGridAttr = $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles"); //$(".divNonGridAxpFile").attr("data-morefiles");
        Oldfiles && Oldfiles.split(',').forEach(function (val) {
            NonGridAttr && NonGridAttr.split(',').forEach(function (attr) {
                var attrname = attr.substring(("Link_" + rowFrmNo + "_").length, attr.length);
                if (val != attrname) {
                    if (val == fldValue) {
                        for (i = 0; i < $("#" + fldName).siblings(".divNonGridAxpFile").find(".attach-files").children().length; i++) {

                            if ($("#" + fldName).siblings(".divNonGridAxpFile").find(".attach-files").children()[i].id == attr) {
                                $($("#" + fldName).siblings(".divNonGridAxpFile").find(".attach-files").children()[i]).removeClass('revrseInlinediv');

                                if ($("#" + fldName).siblings(".divNonGridAxpFile").find(".attach-files").height() > 22) {
                                    $($("#" + fldName).siblings(".divNonGridAxpFile").find(".attach-files").children()[i]).addClass('revrseInlinediv');
                                } else {

                                    var UpdateAttr = $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                                    $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles", UpdateAttr);
                                    if ($("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles") == "") {
                                        $("#" + fldName).siblings(".divNonGridAxpFile").find(".attachment-count").addClass("revrseInlinediv");
                                    }
                                }
                            }
                        }
                    }
                    if (fldValue == attrname.replace(/♠/g, ' ').replace(/♦/g, '(').replace(/♣/g, ')')) {
                        var UpdateAttr2 = $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                        $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles", UpdateAttr2);
                        if ($("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles") == "") {
                            $("#" + fldName).siblings(".divNonGridAxpFile").find(".attachment-count").addClass("revrseInlinediv");
                            $(".popover").css("display", "none");
                        }
                    }
                }
            });
            if (val == fldValue) {
                var attr1 = "Link_" + rowFrmNo + "_" + fldValue;
                attr1 = attr1.replace(/\s/g, '♠').replace(/\(/g, '♦').replace(/\)/g, '♣');
                var UpdateAttr3 = $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles").replace(attr1 + ",", "").replace("," + attr1, "").replace(attr1, "");
                $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles", UpdateAttr3);
                if ($("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles") == "") {
                    $("#" + fldName).siblings(".divNonGridAxpFile").find(".attachment-count").addClass("revrseInlinediv");
                    $(".popover").css("display", "none");
                }
            }
        });
        var datafiles = $("#" + fldName).siblings(".divNonGridAxpFile").attr("data-morefiles");
        $("#" + fldName).siblings(".divNonGridAxpFile").find(".attachment-count span").text(datafiles.split(',').length + "+");
    } else {
        if (AxpGridForm == "popup" && isMobile) {
            $("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles", $("#GridAxpFile" + fldName).siblings("input").val());
        }
        var GridAttr = $("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles");
        Oldfiles && Oldfiles.split(',').forEach(function (val) {
            GridAttr && GridAttr.split(',').forEach(function (attr) {
                var grdattrname = attr.substring(("Link_" + rowFrmNo + "_").length, attr.length);
                if (val != grdattrname) {
                    if (val == fldValue) {
                        for (i = 0; i < $(".attach-files").parent("#GridAxpFile" + fldName).find(".attach-files").children().length; i++) {

                            if ($(".attach-files").parent("#GridAxpFile" + fldName).find(".attach-files").children()[i].id == attr) {
                                $($(".attach-files").parent("#GridAxpFile" + fldName).find(".attach-files").children()[i]).removeClass('revrseInlinediv');
                                let ftsize = $(".attach-files").parent("#GridAxpFile" + fldName).find(".attach-files").css('font-size');
                                let minHght = GetAttachControlHeight(ftsize);
                                if ($(".attach-files").parent("#GridAxpFile" + fldName).find(".attach-files")[0].offsetHeight > minHght) {
                                    $($(".attach-files").parent("#GridAxpFile" + fldName).find(".attach-files").children()[i]).addClass('revrseInlinediv');
                                } else {

                                    var UpdateGrdAttr = $("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                                    $("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles", UpdateGrdAttr);
                                    if ($("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles") == "") {
                                        $("input#" + fldName).siblings("#GridAxpFile" + fldName).find(".attachment-count").addClass("revrseInlinediv");
                                    }

                                }
                            }
                        }
                    }
                    if (fldValue == grdattrname.replace(/♠/g, ' ').replace(/♦/g, '(').replace(/♣/g, ')')) {
                        var UpdateGrdAttr2 = $("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles").replace(attr + ",", "").replace("," + attr, "").replace(attr, "");
                        $("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles", UpdateGrdAttr2);
                        if ($("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles") == "") {
                            $("input#" + fldName).siblings("#GridAxpFile" + fldName).find(".attachment-count").addClass("revrseInlinediv");
                            $(".popover").css("display", "none");
                        }
                    }

                }
            });

            if (val == fldValue) {
                $("textarea#" + fldName).text(ParentDivVal).val(ParentDivVal);
                //$("#divDc" + dcNo).find("textarea[data-type=gridattach]").siblings("#" + fldName).attr("value", ParentDivVal);
            }
        });
        var grddatafiles = $("input#" + fldName).siblings("#GridAxpFile" + fldName).attr("data-morefiles");
        grddatafiles && $("input#" + fldName).siblings("#GridAxpFile" + fldName).find(".attachment-count span").text(grddatafiles.split(',').length + "+");
    }
}

// Referring with dc+dcNo_image
var referRecId = "";
var refFiles = Array();
var fldExp = "";

function SetGridRefWithdcImage(fldName, fldValue) {
    //Using AxpImagePath
    var rowFrmNo = GetFieldsRowFrameNo(fldName);
    var fNo = GetFieldsDcNo(fldName);
    var rid = $j("#recordid000F0").val();
    var arrTempFileNames = Array();
    var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
    fldExp = fldValue;
    var filePath = "";
    filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";

    var hlObj = $j("#grdAtt_hlnk_" + rowFrmNo);
    var TempfldValues = fldValue;

    //  Eg: {@MyDocs\transid\fieldname,recordid(filename1,f2,f3)}
    if (fldValue.indexOf(',') > -1) {

        var referFiles = GetReferedFiles(fldValue);
        var arrFileNames = referFiles.split(',');

        for (var i = 0; i < arrFileNames.length; i++) {
            let fileOpenLink = arrFileNames[i];
            fileOpenLink = fileOpenLink.replace(/'/g, '♠');
            fileLinks += "<div id=\"Link" + arrFileNames[i] + "\"><a href='javascript:void(0)' id='grdAtt_hlnk_001F2' tabindex='-1' class='grdAttach handCur' onclick='ShowGridAttLink(\"" + filePath + referRecId + "-" + fileOpenLink + "\")'>" + arrFileNames[i] + "</a></div>";
            refFiles.push(arrFileNames[i]);
        }
        if (window.localStorage) {
            localStorage.refFiles = JSON.stringify(arrFileNames);
            localStorage.referRecId = JSON.stringify(referRecId);
            localStorage.fldExp = JSON.stringify(fldExp);
        }
    }
    return fileLinks;
}

// Get Referred Filenames
function GetReferedFiles(fldValue) {

    var TempfldValues = fldValue;
    var OIndx = TempfldValues.indexOf('(');
    var CommaIndx = TempfldValues.indexOf(',');
    var fileNameSIndx = OIndx + 1;
    var fileNameEIndx = TempfldValues.indexOf(")") - 1;
    referRecId = TempfldValues.substring(CommaIndx + 1, fileNameSIndx - 1);
    var referFiles = TempfldValues.substr(fileNameSIndx, fileNameEIndx - fileNameSIndx + 1);
    return referFiles;
}

//dc+dcNo_referImages
function SetGridRefValue(fldName, fldValue) {
    var fileLinks = "";
    var fValues = fldValue;
    var FileNames;
    var sFilsOnebyOne;
    var arrTempFileNames = Array();
    var hasImagePath = "false";
    var arrFileNames = new Array();
    var referFiles
    if (fldValue != "") {
        FileNames = fValues.split("@");
        if (FileNames.length >= 1) {
            var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
            var filePath = "";
            filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
            var rowFrmNo = GetFieldsRowFrameNo(fldName);
            //var hlObj = $j("#grdRef_hlnk_" + rowFrmNo);
            var hlObj = $j("#grdRef_img_" + rowFrmNo);
            var fDCNo = GetFieldsDcNo(fldName);
            var fNo = GetFieldsRowNo(fldName);
            var rid = $j("#recordid000F0").val();
            for (var j = 0; j < FileNames.length; j++) {
                sFilsOnebyOne = FileNames[j];
                if (sFilsOnebyOne != "") {
                    sFilsOnebyOne = sFilsOnebyOne.substring(sFilsOnebyOne.lastIndexOf("\\") + 1);
                    var OIndx = sFilsOnebyOne.indexOf('(');
                    var CommaIndx = sFilsOnebyOne.indexOf(',');

                    if (CommaIndx < OIndx) {
                        var fileNameSIndx = OIndx + 1;
                        var fileNameEIndx = sFilsOnebyOne.indexOf(")") - 1;
                        referRecId = sFilsOnebyOne.substring(CommaIndx + 1, fileNameSIndx - 1);
                        referFiles = sFilsOnebyOne.substr(fileNameSIndx, fileNameEIndx - fileNameSIndx + 1);
                    } else {
                        hasImagePath = "true";
                        referFiles = sFilsOnebyOne;
                    }

                    if (referFiles.length > 0)
                        arrFileNames = referFiles.split(',');

                    if (arrFileNames.length) {
                        var destFName;
                        for (var i = 0; i < arrFileNames.length; i++) {
                            if (arrFileNames[i] != undefined || arrFileNames[i] != "") {
                                if (hasImagePath == "false")
                                    destFName = referRecId + "-" + arrFileNames[i];
                                else
                                    destFName = arrFileNames[i];
                                destFName = destFName.replace(/'/g, '♠');
                                fileLinks += "<div id=\"ReferLink_" + fNo + "_" + arrFileNames[i] + "\"><a id=\"grdRefLink_hlnk__" + fNo + "F" + fDCNo + "\" onclick=\"DeleteFileFromRow('" + fldName + "','" + rowFrmNo + "','" + arrFileNames[i] + "')\"><img src=\"../AxpImages/icons/16x16/delete.png\" /></a><a id='grdRef_hlnk_" + fNo + "F" + fDCNo + "' class='grdRefer handCur' onclick='ShowGridAttLink(\"" + filePath + destFName + "\")'>" + arrFileNames[i] + "</a></div>";
                            }
                        }
                    }
                }
            }
            hlObj[0].outerHTML = fileLinks;

        }
    }
}

//End 
var hlightRow = ""; //#ededed
//function HighlightRow(dcNo, rowNo, ref, ftype) {

//    var rowId = "sp" + dcNo + "R" + rowNo + "F" + dcNo;
//    if (document.getElementById(rowId)) {
//        if (ref == "focus") {
//            if (hlightRow != "")
//                if (document.getElementById(hlightRow) != null) {
//                    document.getElementById(hlightRow).style.backgroundColor = "#FFFFFF";
//                    document.getElementById(rowId).style.backgroundColor = "#FFF200";
//                }
//            hlightRow = rowId;
//        }
//        else {
//            if (document.getElementById(hlightRow) != null) {
//                if ((hlightRow == rowId) && (ftype == "checkbox"))
//                    document.getElementById(rowId).style.backgroundColor = "#FFF200";
//                else
//                    document.getElementById(rowId).style.backgroundColor = "#FFFFFF";
//            }
//        }
//        if (hlightRow != "")
//            if (document.getElementById(hlightRow) != null) {
//                document.getElementById(hlightRow).style.backgroundColor = "#FFFFFF";
//                document.getElementById(rowId).style.backgroundColor = "#FFF200";
//            }
//        hlightRow = rowId;
//    }
//}

//function UnHighlightRow() {
//    if (hlightRow != "") {
//        if (document.getElementById(hlightRow) != null)
//            document.getElementById(hlightRow).style.backgroundColor = "#FFFFFF";
//        hlightRow = "";
//    }
//}

// handling onPaste for RichTextBox for mozilla browser
//function handlePaste(elem, e) {
//    e.preventDefault();
//    var text;
//    var clp = (e.originalEvent || e).clipboardData;
//    text = clp.getData('text/plain') || "";
//    if (text !== "") {
//        document.execCommand('insertText', false, text);
//    }
//}
/*----------enable/disable add/delete/fillGrid buttons dynamically on dataload-------------*/
// function to get the conditions
function SetGridBtnAccess() {
    if ($j("#recordid000F0").val() != "0") {
        // checks if condition has set for dc or not  
        if ($j("#axp_btnaccess000F1").length > 0) {
            var cDcValue = $j("#axp_btnaccess000F1").val().toLowerCase();
            if (cDcValue != "") {
                // get all the conditions
                ////the expression format :  {Dc2~A0~D0~Fillname^Dc3~d1}
                var cofigDcsConds;
                if (cDcValue.indexOf("{") != "-1") {
                    cofigDcsConds = cDcValue.substring(1, cDcValue.length - 1).split("^");
                } else {
                    cofigDcsConds = cDcValue.split("^");
                }
                for (var i = 0; i < cofigDcsConds.length; i++) {
                    var conDcsValue = cofigDcsConds[i].split("~");
                    var dcNo = conDcsValue[0].substring(2);
                    for (var j = 1; j < conDcsValue.length; j++) {
                        if (conDcsValue[j].toString().startsWith('a'))
                            SetAddBtnAccess(dcNo, conDcsValue[j]);
                        else if (conDcsValue[j].toString().startsWith('d'))
                            SetDeleteBtnAccess(dcNo, conDcsValue[j]);
                        else
                            SetFillGridBtnAccess(dcNo, conDcsValue[j]);
                    }
                }

            }

        }
    }
}

// function to set the property of Add, Delete, FillGrid
function SetAddBtnAccess(dcNo, props) {
    // if condt checks whether following dc is present in tstruct or not
    if ($j("#divDc" + dcNo).length > 0) {
        // checks the condition of add row ( add row will be dissabled or not)
        if ($j("#addrow" + dcNo).length > 0) {
            if (props.toString() == 'a0')
                $j("#addrow" + dcNo).removeClass("disableadd");
            else if (props.toString() == 'a1')
                $j("#addrow" + dcNo).addClass("disableadd");
            else if (props.toString() == 'a2')
                $j("#addrow" + dcNo).show();
            else if (props.toString() == 'a3')
                $j("#addrow" + dcNo).hide();
        }
    }
}

function SetDeleteBtnAccess(dcNo, props) {
    // counts all the rows that hvae delete button
    var rowCount = parseInt(GetDcRowCount(dcNo), 10);
    for (var i = 1; i <= rowCount; i++) {
        var rowNo = GetRowNoHelper(GetClientRowNo(i, dcNo));
        var delRowId = delRowId = "del" + rowNo + "F" + dcNo;
        if (props.toString().toLowerCase() == 'd0') {
            if ($j("#" + delRowId).length > 0)
                $j("#" + delRowId).removeClass('disabledelete')
        } else if (props.toString().toLowerCase() == 'd1') {
            if ($j("#" + delRowId).length > 0)
                $j("#" + delRowId).addClass("disabledelete");
        }
    }
}

function SetFillGridBtnAccess(dcNo, props) {
    if ($j("#fillgrid" + dcNo).length > 0) {
        var fgname;
        var arrFillGrid = document.getElementsByClassName("handCur fillcls");
        for (var j = 0; j < arrFillGrid.length; j++) {
            fgname = arrFillGrid[j].name;
            // checks the condition of fill ie ( fill button  will be dissabled or not)
            if (props.toString().startsWith("f*")) {
                if (props.toString() == 'f*0')
                    $j("[name=" + fgname + "]").removeClass("disablefill");
                else if (props.toString() == 'f*1')
                    $j("[name=" + fgname + "]").addClass("disablefill");
                else if (props.toString() == 'f*2') {
                    $j("[name=" + fgname + "]").show();
                    $j("[name = lbl" + fgname + "]").show();
                } else if (props.toString() == 'f*3') {
                    $j("[name=" + fgname + "]").hide();
                    $j("[name = lbl" + fgname + "]").hide();

                }
            } else {
                if (props.toString() == fgname + 0)
                    $j("[name=" + fgname + "]").removeClass("disablefill");
                else if (props.toString() == fgname + 1)
                    $j("[name=" + fgname + "]").addClass("disablefill");
                else if (props.toString() == fgname + 2) {
                    $j("[name=" + fgname + "]").show();
                    $j("[name = lbl" + fgname + "]").show(); // for showing fill grid label
                } else if (props.toString() == fgname + 3) {
                    $j("[name=" + fgname + "]").hide();
                    $j("[name = lbl" + fgname + "]").hide(); // for hiding fill grid label

                }
            }

        }
    }
}
//End of Region Tstruct Search

function SafariTabDcHeight() {
    $j(document).ready(function () {
        if (navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1) {
            var j = 0;
            for (j = 0; j < PagePositions.length; j++) {
                if (PagePositions[j].toString().indexOf(",") != -1) {
                    var strTabs = PagePositions[j].toString().split(',');
                    var ulId = "tabs" + strTabs[0];
                    $j("#" + ulId + " ul").css("height", "100%");
                }
            }
        }
        //Added to hide Message if anywhere click on body 
        $('body').click(function (e) {
            if ($('.agc-alert-success').length != 0) {
                HideDialog();
            }
        });
        $(document).keypress(function (event) {
            if ($('.agc-alert-success').length != 0) {
                HideDialog();
            }
        });
    });
    checkSuccessAxpertMsg();
}

function CheckCustFooter() {
    var custToolFld = $j("#ax_customtoolbar000F1");
    if (custToolFld != null && custToolFld.length > 0) {
        $j("#dvCustToolbar").css("display", "block");
        $j("#dvsubmitCancelBtns").css("display", "none");
        //n=sa,c=Save,a=axsave~n=sc,c=Save and Continue,a=axsavecontinue,d=ipcort~n=ss,c=Save and Send,a=axsavesend,d=fname:value
        //sa,Save,axsave~sc,Save and continue,axsavecntinue,ipcort~ss,Save and Send,axsavesend,fname=value

        var strBtns = custToolFld.val().split("~");
        var strBtnHtml = "";
        for (var i = 0; i < strBtns.length; i++) {
            var btnProps = strBtns[i].split(",");
            if (btnProps.length > 3)
                strBtnHtml += CreateCustFooter(btnProps[0], btnProps[1], btnProps[2], btnProps[3]);
            else
                strBtnHtml += CreateCustFooter(btnProps[0], btnProps[1], btnProps[2]);
        }
        //$j("#dvCustToolbar").html("<table style=\"width:100%;\"><tbody><tr><td align=\"center\" valign=\"middle\"><center>" + strBtnHtml + "</center></td></tr></tbody></table>");
        $j("#dvCustToolbar").html("<table><tbody><tr><td align=\"center\" valign=\"middle\"><center>" + strBtnHtml + "</center></td></tr></tbody></table>");

    }


}

//TODO: details to  be saved
function CreateCustFooter(name, caption, btnAction, details) {
    var btnHtml = "";
    if (btnAction == "ax_save")
        btnHtml = "<span id=\"dvSaveNew\"><input id=\"btnSaveNew\" name=\"axFooBtn_" + name + "\" type=\"button\" onclick=\"javascript: FormSaveNew();\" value=\"" + caption + "\" class=\"button\"/>&nbsp;&nbsp;</span>";
    else if (btnAction == "ax_savereset")
        btnHtml = "<span id=\"dvSaveReset\"><input id=\"btnSaveReset\" name=\"axFooBtn_" + name + "\" type=\"button\" onclick=\"javascript: FormSaveReset();\" value=\"" + caption + "\" class=\"button\"/>&nbsp;&nbsp;</span>";
    else if (btnAction == "ax_savecontinue") {
        btnHtml = "<span id=\"dvSaveCont\"><input id=\"btnSaveCont\" name=\"axFooBtn_" + name + "\" type=\"button\" onclick=\"javascript: FormSaveContinue();\" value=\"" + caption + "\" class=\"button\"/>&nbsp;&nbsp;</span>";
        btnSaveContVal = details;
    } else if (btnAction == "ax_savesend") {
        btnHtml = "<span id=\"dvSaveSend\"><input id=\"btnSaveSend\" name=\"axFooBtn_" + name + "\" type=\"button\" onclick=\"javascript: FormSaveSend('" + details + "');\" value=\"" + caption + "\" class=\"button\"/>&nbsp;&nbsp;</span>";
        btnSaveSendVal = details;
    } else if (btnAction == "ax_reset")
        btnHtml = "<span id=\"dvReset\"><input id=\"btnFooReset\" name=\"axFooBtn_" + name + "\" type=\"button\" onclick=\"javascript: FormReset();\" value=\"" + caption + "\" class=\"button\"/>&nbsp;&nbsp;</span>";

    return btnHtml;
}

var axFooAction = "";

function FormSaveNew() {
    axFooAction = "ax_savenew";
    FormSubmit();
}

function FormSaveReset() {
    axFooAction = "ax_save";
    FormSubmit();
}

function FormSaveContinue() {
    axFooAction = "ax_savecontinue";
    FormSubmit();
}

function FormSaveSend(details) {
    axFooAction = "ax_savesend";

    var strDet = details.split(":");
    if (strDet.length > 1) {
        var fName = "#" + strDet[0] + "000F1";
        var value = strDet[1];


        if ($j(fName).length > 0) {
            $j(fName).val(value);
            UpdateFieldArray(fName.substring(1), "0", value, "parent");
            UpdateAllFieldValues(fName.substring(1), value);
        }
    }
    if (!FormSubmit()) {
        if ($j(fName).length > 0) {
            $j(fName).val(value);
            UpdateFieldArray(fName.substring(1), "0", value, "parent");
            UpdateAllFieldValues(fName.substring(1), value);
        }
    }
}

function FormReset() {
    axFooAction = "ax_reset";
    NewTstruct();
}
//// Adjusting tstruct height in mozilla firefox
//function AdjustWinff() {
//    //if ($j.browser.mozilla)
//    //    adjustwin($j(window.document.body).height(), window);
//}

function ShowTstHelp(transId) {

    var loadPop;
    try {
        loadPop = window.open("Formhelp.aspx?transid=" + tst + "&key=" + tstDataId, "LoadPop", "width=800,height=500,resizable=1,scrollbars=yes");
    } catch (ex) {
        showAlertDialog("warning", eval(callParent('lcm[356]')));
    }
}

//function OnSucessShowHint(result) {

//    var hintText = result;
//    if (hintText == "" || hintText == "Error") {
//        hintText = " Hint is not Available";
//    }
//    $j.msgBox({
//        title: "",
//        content: hintText,
//        type: "info"
//    }, 50, "70%", "60%");

//}

//function to show popup window when user upload pdf files fro signing
function ShowFileUplDSignPopUp(obj) {
    var AxAttField = obj.id;
    AxAttField = AxAttField.replace("img_", "");
    var hdnDocName = document.getElementById("hdn_" + AxAttField + "").value;
    if (hdnDocName == "") {
        showAlertDialog("warning", 2020, "client");
    } else {
        var na = "../dsign/DsignUploadFile.aspx?pdocname=" + hdnDocName + "&attachtxtid=" + AxAttField + "";
        window.open(na, "Fileupload", "width=500,height=350,scrollbars=no,resizable=yes");
    }
}

function DisableFileUplDSignFld() {
    if ($j(".disupldsign").length > 0) {
        $j(".disupldsign").each(function () {
            $j(".disupldsign").prop("disabled", true);
            var inpType = $j(this);
            if (inpType[0].type != "text") {
                var rid = $j("#recordid000F0").val();
                if (rid > 0)
                    inpType.show();
                else
                    inpType.hide();
            } else {
                var rid = $j("#recordid000F0").val();
                if (rid <= 0)
                    $j(".disupldsign").val('');
            }
        });
    }
}
//End of Region Tstruct Search

/* Region Start MultiSelectSearch*/
function OpenMulSelSrchPop(ele) {
    var imgId = ele.id.split(/_(.*)/);
    multiSelectID = imgId[1];
    var yPosition = $j("#" + multiSelectID).offset().top;
    $j("#srchMulSelResCnt").text("0 of 0");
    if ($j('.ui-dialog').is(':visible') && $j("#srchMulSelFlds").is(':visible') || (!$j('.ui-dialog').is(':visible') && !$j("#srchMulSelFlds").is(':visible'))) {
        //e.preventDefault();
        $j("#srchMulSelDynTxt").val('');
        srchDlg = $j("#srchMulSelFlds").dialog({
            resizable: false,
            autoOpen: true,
            height: 100,
            modal: false,
            title: "Search",
            position: ['top', yPosition - 40],
            buttons: {
                "OK": function () {
                    focusSelected();
                },
                "Next": function () {
                    MovMulSelNxt();
                },
                "Prev": function () {
                    MovMulSelPrv();
                },
                "Close": function () {
                    UnSelMuSelSrchItm();
                    $j(this).dialog("close");
                }
            },
            close: function (ev, ui) {
                UnSelMuSelSrchItm();
            }
        }).prev(".ui-dialog-titlebar").css("color", "white");
    } else
        UnSelMuSelSrchItm();

}

function FndMulSelStr(searchTerm) {
    AxSearchArray = new Array();
    UnSelectSearchItem();
    if (searchTerm == "") {
        $j("#srchMulSelResCnt").text("0 Found");
        return;
    }
    $j("span [name =" + multiSelectID + "]").find(':checkbox').each(function () {
        if ($j(this).attr('type') == "hidden")
            return;

        var id = $j(this).attr('id');
        if (id) {
            var dcNo = GetFieldsDcNo(id);
            if (IsDcPopGrid(dcNo))
                return;
        }

        var txtn = $j(this).val().toString().trim().toLowerCase();

        if (txtn.indexOf(searchTerm.toString().toLowerCase()) > -1 && !$j(this).is(":focus"))
            AxSearchArray.push($j(this));
        idxSearchFoc = 0;
        if (AxSearchArray.length > 0)
            $j("#srchMulSelResCnt").text(AxSearchArray.length + " Found");
        else
            $j("#srchMulSelResCnt").text("0 Found");
    });
}

function UnSelMuSelSrchItm() {
    $j('input:not([id=srchMulSelDynTxt]').each(function () {
        if ($j(this).hasClass('srchselectedfield'))
            $j(this).removeClass('srchselectedfield');
    });
}

function focusSelected() {
    $j(AxSearchArray[0]).focus();
}

function MovMulSelNxt() {
    if (idxSearchFoc < AxSearchArray.length - 1) {
        idxSearchFoc++;
        var dcNo = 0;
        if ($j(AxSearchArray[idxSearchFoc]).attr('id'))
            dcNo = GetFieldsDcNo($j(AxSearchArray[idxSearchFoc]).attr('id'));
        AxSearchArray[idxSearchFoc].addClass('srchselectedfield');
        AxSearchArray[idxSearchFoc - 1].removeClass('srchselectedfield');
        if ($j.inArray(dcNo, TabDCs) > -1)
            $j("#ank" + dcNo).click();
        $j("#srchMulSelResCnt").text(idxSearchFoc + 1 + " of " + AxSearchArray.length);
        AxSearchArray[idxSearchFoc].focus();
    } else if (idxSearchFoc == AxSearchArray.length - 1) {
        AxSearchArray[idxSearchFoc].focus();
    }
}

function MovMulSelPrv() {
    var visDcname = GetOpenTabDcs();
    if (idxSearchFoc > 0 && AxSearchArray.length >= idxSearchFoc) {
        idxSearchFoc--;
        var dcNo = 0;
        if ($j(AxSearchArray[idxSearchFoc]).attr('id'))
            dcNo = GetFieldsDcNo($j(AxSearchArray[idxSearchFoc]).attr('id'));
        AxSearchArray[idxSearchFoc].addClass('srchselectedfield');
        AxSearchArray[idxSearchFoc + 1].removeClass('srchselectedfield');
        if ($j.inArray(dcNo, TabDCs) > -1)
            $j("#ank" + dcNo).click();
        AxSearchArray[idxSearchFoc].focus();
        $j("#srchMulSelResCnt").text(idxSearchFoc + 1 + " of " + AxSearchArray.length);
    } else if (idxSearchFoc == AxSearchArray.length - 1) {
        AxSearchArray[idxSearchFoc].focus();
    }
}
/*Region End MultiSelect Search*/

function ParseJSON() {

    var visibleDcs = $j('[id*="divDc"]');
    for (var i = 0; i < visibleDcs.length; i++) {
        var wrapperForEditFieldsIsHidden = false;
        var wraperDivId;
        try {
            wraperDivId = $j("#" + visibleDcs[i].id + " [id^=wrapperForEditFields]")[0].id
            if ($j("#" + wraperDivId).hasClass("hide")) {
                wrapperForEditFieldsIsHidden = true;
                $j("#" + wraperDivId).removeClass("hide");
            }
        } catch (ex) {}
        AlignDcElements(visibleDcs[i].id, "firstLoad");
        try {
            if (wrapperForEditFieldsIsHidden == true) {
                $j("#" + wraperDivId).addClass("hide");
                wrapperForEditFieldsIsHidden = false;
            }
        } catch (ex) {}
        AssignGrdFreezeHdrScript(i + 1)
        OnRowChangeSetHeight(i + 1);
    }
    $j('div#wBdr').show();
    ShowDimmer(false);
}


//To open tstructdesign onclick ifpopup
function openTstDesign() {
    //window.location.href = 'tstructdesign.aspx?transid=' + transid + '' + "&AxPop=true";
}


function AlignDcElements(dcNo, calledFrom) {
    calledFrom = typeof calledFrom != "undefined" ? calledFrom : "default";
    if (dcNo.indexOf("divDc") > -1 && $("#" + dcNo).is(":visible")) {
        TabDCAlignmentStatus[dcNo.substr(dcNo.indexOf("divDc") + 5) - 1] = "aligned";
    }
    //var jsonText = $j('#designHidden').val();
    //var jsonText = "";
    if (jsonText != undefined && jsonText != "") {

        //var obj = JSON.parse(jsonText);



        //var obj = JSON.parse(jsonText);
        //for (var i = 0; i < obj.dcs.length; i++) {

        //    if (obj.dcs[i].id == dcNo) {
        //        for (var j = 0; j < obj.dcs[i].elements[0].length; j++) {
        //            if (obj.dcs[i].elements[0][j].classes == "row" || obj.dcs[i].elements[0][j].classes == "rowdesign") {

        //                var currentElement = $j('#' + obj.dcs[i].elements[0][j].id);
        //                currentElement.attr('class', '');
        //                currentElement.attr('class', obj.dcs[i].elements[0][j].classes);

        //                //elements are detached from the current place and then added according to the designer using the JSON.
        //                for (var e = 0; e < obj.dcs[i].elements[0][j].elements[0].length; e++) {
        //                    var currentElement = $j('#' + obj.dcs[i].elements[0][j].elements[0][e].id);
        //                    var curElemClasses = currentElement.attr("class");
        //                    if (currentElement.length === 0)
        //                        continue;
        //                    var dataindex = curElemClasses.substr(curElemClasses.indexOf("dataindex") + 9);
        //                    currentElement.data("elempos", dataindex);
        //                    currentElement.attr('class', '');
        //                    currentElement.attr('class', obj.dcs[i].elements[0][j].elements[0][e].classes);
        //                    if (obj.dcs[i].elements[0][j].elements[0][e].rows > 0 && currentElement.children().last()[0] != undefined)
        //                        currentElement.children().last().attr("rows", obj.dcs[i].elements[0][j].elements[0][e].rows);
        //                    else if (obj.dcs[i].elements[0][j].elements[0][e].height != undefined)
        //                        currentElement.find(".divImgBorder .axpImg").css("height", parseInt(obj.dcs[i].elements[0][j].elements[0][e].height.replace("px", "")));

        //                }
        //            }
        //            else if (obj.dcs[i].elements[0][j].id == "containerDc") {
        //                var tabDCAlignStatus = typeof TabDCAlignmentStatus[obj.dcs[i].id.substr(obj.dcs[i].id.indexOf("divDc") + 5) - 1];
        //                var containerElement = obj.dcs[i].elements[0][j];
        //                var ContCurrentElement = $j('#' + containerElement.id);
        //                if ($j("#myTab").length == 0) {
        //                    //$j('#' + containerElement.id).css("padding", "0px 15px");
        //                    $j('[id^=' + containerElement.id + ']').css("padding", "0px 15px");
        //                }

        //                for (var k = 0; k < containerElement.elements[0].length; k++) {
        //                    var visibilityEyesCreated = false;//for design mode

        //                    if (containerElement.elements[0][k].classes == "divGridContent") {

        //                        //todo for the body hwight fix in grid

        //                    }
        //                    else if (containerElement.elements[0][k].classes == "grid-icons") {
        //                        //for edit layout of grid
        //                        var editElements = containerElement.elements[0][k].elements;
        //                        for (l = 0; l < editElements.length ; l++) {
        //                            var idOfElement = editElements[l].id;
        //                            if (tabDCAlignStatus != "undefined") {
        //                                $j("#" + idOfElement).is(":visible") ? idOfElement = idOfElement : idOfElement = "";
        //                            }
        //                            if (idOfElement != "") {
        //                                var isFieldVisible = editElements[l].isVisible;
        //                                isFieldVisible === undefined ? isFieldVisible = "true" : "";
        //                                var element = $j("#" + idOfElement)
        //                                element.attr({
        //                                    'class': editElements[l].classes,
        //                                    'data-isVisible': isFieldVisible
        //                                });
        //                                element.data("isVisible", isFieldVisible);
        //                                if (theMode == "design") {
        //                                    //isFieldVisible == "true" ? element.addClass('customDesignHide') : element.addClass('customDesignHide');
        //                                } else {
        //                                    isFieldVisible == "true" ? element.show() : element.addClass('customHide').hide();
        //                                }

        //                            }
        //                        }

        //    }

        //                    else if (containerElement.elements[0][k].classes.indexOf('wrapperForGridData') != -1) {
        //                        //for table of grid
        //                        var tableElements = containerElement.elements[0][k].elements;
        //                        for (m = 0; m < tableElements.length ; m++) {
        //                            var idOfElement = tableElements[m].id;
        //                            if (tabDCAlignStatus != "undefined") {
        //                                $j("#" + idOfElement).is(":visible") ? idOfElement = idOfElement : idOfElement = "";
        //                            }
        //                            var gridVisibility = tableElements[m].visibility;
        //                            var eyeIcon = "";
        //                            var eyeTitle = "";
        //                            gridVisibility == undefined ? gridVisibility = 'table-cell' : "";
        //                            gridVisibility == 'none' ? eyeIcon = 'icon-basic-eye-closed' : eyeIcon = 'icon-basic-eye';
        //                            //.attr('title','Un-hide Column')
        //                            eyeIcon == 'icon-basic-eye' ? eyeTitle = 'Hide Column' : eyeTitle = 'Show Column';
        //                            if (idOfElement != "") {
        //                                if (theMode == "design" && tabDCAlignStatus != "undefined") {
        //                                    visibilityEyesCreated = true;
        //                                    $j("#" + idOfElement).append('<button type="button" class="eyeBtn" onclick="toggleEyeClass(this,event);"><i title="' + eyeTitle + '" class="' + eyeIcon + '"></i></button>');
        //                                    gridVisibility = 'table-cell';
        //                                }
        //                                $j("#" + idOfElement).addClass(gridVisibility).css({ "width": tableElements[m].width, "min-width": tableElements[m].width, "display": gridVisibility });
        //                                var pDcNo = dcNo.substr(5);
        //                                if ($j("#gridHd" + pDcNo + " tbody tr").length > 0) {
        //                                    var index = $j("#" + idOfElement).index();
        //                                    $j("#gridHd" + pDcNo + " tbody tr td:nth-child(" + (index + 1) + ")").addClass(gridVisibility).css('display', gridVisibility);
        //                                }
        //                            }

        //                        }
        //                        if (theMode == "design") {
        //                            $j("#" + containerElement.elements[0][k].tableid).addClass('fixHeaderTable').attr('style', containerElement.elements[0][k].tablecss).find('tbody').css("min-height", containerElement.elements[0][k].tbodyHeight);

        //                        } else {
        //                            var maxHeight = "300px";
        //                            containerElement.elements[0][k].tbodyHeight == "0px" ? maxHeight = "300px" : maxHeight = containerElement.elements[0][k].tbodyHeight;
        //                            $j("#" + containerElement.elements[0][k].tableid).addClass('fixHeaderTable').attr('style', containerElement.elements[0][k].tablecss).find('tbody').css("max-height", maxHeight);
        //                            if ($(".wrapperForGridData" + dcNo + "").outerWidth(true) < $(".wrapperForGridData" + dcNo + " table").outerWidth(true)) {
        //                                $j("#" + containerElement.elements[0][k].tableid).css("display", "block");
        //                            } else {
        //                                $j("#" + containerElement.elements[0][k].tableid).css("display", "");
        //                            }
        //                        }
        //                        if (containerElement.elements[0][k].gridStretch) {
        //                            $("#ckbGridStretch" + pDcNo).prop("checked", true);
        //                            toggleGridStretch(pDcNo, true, calledFrom);
        //                            //$(".wrapperForGridData" + containerElement.elements[0][k].classes.substring(32) + " table tr th").each(function (index, elem) {
        //                            //    if ($(elem).find(".icon-basic-eye-closed").length > 0) {

        //                            //        $j("#gridHd" + pDcNo + " tbody tr td:nth-child(" + (index + 1) + "),#gridHd" + pDcNo + " thead tr th:nth-child(" + (index + 1) + ")").addClass("hiddenTableColumn");
        //                            //    }
        //                            //});
        //    }
        //                    }

        //                    else if (containerElement.elements[0][k].classes == "griddivColumn") {
        //                        //var masterEle = containerElement.elements[0][k];
        //                        //var currentElement = $j('#' + masterEle.id);
        //                        //if (containerElement.elements[0][k].tableid != undefined)
        //                        //    $j("#" + containerElement.elements[0][k].tableid).attr("style", containerElement.elements[0][k].tablecss);
        //                        //for (var e = 0; e < masterEle.elements.length; e++) {

        //                        //    var currentElement = $j('#' + masterEle.elements[e].id);
        //                        //    currentElement.css('width', masterEle.elements[e].width);

        //                        //    var rowCount = $j("#gridDc" + (i + 1) + " tr").length;
        //                        //    for (var row = 0; row < rowCount; row++) {
        //                        //        var column = masterEle.elements[e].id.replace("th-", "") + "00" + (row + 1) + "F" + (i + 1);
        //                        //        currentElement = $j("#" + column).closest("td");

        //                        //        currentElement.css("width", masterEle.elements[e].width);
        //                        //    }
        //                        //}
        //                    }
        //                }
        //                if (theMode == 'design' && typeof visibilityEyesCreated && visibilityEyesCreated == false && tabDCAlignStatus != "undefined") {
        //                    var pDcNo = dcNo.substr(5)
        //                    //.append('<button type="button" class="eyeBtn" onclick="toggleEyeClass(this,event);"><i class="' + eyeIcon + '"></i></button>')
        //                    $j("#gridHd" + pDcNo + " thead th").each(function () {
        //                        var elem = $(this);
        //                        if (elem.is(':visible') && elem.attr('id') != undefined && elem.attr('id') != "") {
        //                            elem.append('<button type="button" class="eyeBtn" onclick="toggleEyeClass(this,event);"><i class="icon-basic-eye" title="Hide Column"></i></button>');
        //                        }
        //                    })
        //                }

        //            }
        //        }
        //    }
        //}

        ////checking for comprossed mode of tstruct
        //if (obj.cpMode == true) {
        //    //load the compressed css
        //    if ($("link[href='../Css/compressedTstruct.min.css?v=39']").length != 1)
        //        $('head').append($('<link rel="stylesheet" type="text/css" />').attr('href', '../Css/compressedTstruct.min.css?v=39'));
        //    if (theMode == 'design') {
        //        $("#ckbCompressedMode").prop('checked', true);
        //    }
        //    if ($j("#hdnCompMode").length)
        //        $j("#hdnCompMode").val("true");
        //} else {
        //    //remove the compresssed css
        //    if ($("link[href='../Css/compressedTstruct.min.css?v=39']").length > 0)
        //        $('link[rel=stylesheet][href~="../Css/compressedTstruct.min.css?v=39"]').remove();
        //    if (theMode == 'design') {
        //        $("#ckbCompressedMode").prop('checked', false);
        //    }
        //    if ($j("#hdnCompMode").length)
        //        $j("#hdnCompMode").val("false");
        //}
        //SetGridElementsHeight();

    } else {
        var dcNum = parseInt(dcNo.substring(5));
        if ($j("#contentScroll" + dcNum).length > 0) {
            if ($j("#myTab").length == 0) {
                //$j('#containerDc').css("padding", "0px 15px");
                $j('[id^=containerDc]').css("padding", "0px 15px");
            }
            var theadwidth = parseInt($j("#gridHd" + dcNum).css("min-width").replace("px", ""));
            $j("#gridHd" + dcNum).css("width", theadwidth + "px");
            if (theMode == 'design') {
                $j("#gridHd" + dcNum + " thead th").each(function () {
                    var elem = $(this);
                    if (elem.is(':visible') && elem.attr('id') != undefined && elem.attr('id') != "") {
                        elem.append('<button type="button" class="eyeBtn" onclick="toggleEyeClass(this,event);"><i class="icon-basic-eye" title="Hide Column"></i></button>');
                    }
                })
            }
            if (theadwidth < $j("#wBdr").width()) {
                $j("#contentScroll" + dcNum).css("width", parseInt(theadwidth));

            } else {
                $j("#contentScroll" + dcNum).css("width", (parseInt($j("#wBdr").width()) - 30));
            }
        }
    }
    var pDcNo = dcNo.substr(5); //DivDc2=>2
    checkTableBodyWidths(pDcNo);
}


function OnRowChangeSetHeight(dcNumber) {
    var headtable = $j("#gridHd" + dcNumber);
    var ele = $j("#contentScroll" + dcNumber);
    if (headtable.length > 0 && ele.length > 0) {
        var Height = parseInt($j("#gridDc" + dcNumber + " tbody").css("height").replace("px", ""));
        if ((parseInt(headtable.css("min-width").replace("px", "")) < parseInt($j("#wBdr").css("width").replace("px", ""))) ||
            (ele[0].scrollWidth > ele[0].clientWidth)) {
            if ((Height + 20) > parseInt(ele.css("height").replace("px", ""))) {

                if (!ele.hasClass("hasScroll")) {
                    ele.css("width", (parseInt(ele.css("width").replace("px", "")) + 18));
                    ele.addClass("hasScroll");
                }
            } else if (ele.hasClass("hasScroll")) {
                ele.css("width", (parseInt(ele.css("width").replace("px", "")) - 18));
                ele.removeClass("hasScroll");
            }
        }
    }
}

function FocusOnLastField(dcNumber, rownum) {

    var ele = $j("#contentScroll" + dcNumber);
    OnRowChangeSetHeight(dcNumber);
    if (ele != undefined) {
        ele.scrollTop(ele[0].scrollHeight);
        if ($j("#gridDc" + dcNumber + " tbody tr:last").find("td:visible:not('.gridtdclass')").length > 0)
            var lasttd = $j("#gridDc" + dcNumber + " tbody tr:last").find("td:visible:not('.gridtdclass')")[0];
        $j(lasttd).find("select,input").focus();
    }
}

function FocusOnFirstGridField(dcNumber) {

    var ele = $j("#contentScroll" + dcNumber);
    if (ele.length > 0) {
        OnRowChangeSetHeight(dcNumber);
        if (ele != undefined) {
            ele.scrollTop(ele[0].scrollHeight);
            if ($j("#gridDc" + dcNumber + " tbody tr:first").find("td:visible:not('.gridtdclass')").length > 0)
                var lasttd = $j("#gridDc" + dcNumber + " tbody tr:first").find("td:visible:not('.gridtdclass')")[0];
            $j(lasttd).find("select,input").focus();
        }
    }
}

function OnMenuPanelChange(mpanel) {
    var tables = $j("[id^='gridHd']");

    if (tables.length > 0) {
        for (i = 0; i < tables.length; i++) {
            var dcNumb = tables[i].id.substring(6);
            var ele = $j("#contentScroll" + dcNumb);
            var Height = parseInt($j("#gridDc" + dcNumb + " tbody").css("height").replace("px", ""));
            if (dcNumb != undefined && !(isNaN(dcNumb))) {
                if ($j("#myTab").length == 0) {
                    //$j('#containerDc').css("padding", "0px 15px");
                    $j('[id^=containerDc]').css("padding", "0px 15px");
                }
                var minwidth = parseInt($j(tables[i]).css("min-width").replace("px", ""));
                if ((minwidth) > (parseInt($j("#wBdr").css("width").replace("px", "")) + 30)) {
                    $j("#contentScroll" + dcNumb).css("width", (parseInt($j("#wBdr").css("width").replace("px", "")) - 30));
                    if (ele.hasClass("hasScroll")) {
                        ele.css("width", (parseInt(ele.css("width").replace("px", "")) + 18));
                    }
                } else {
                    $j("#contentScroll" + dcNumb).css("width", (minwidth));
                    if (ele.hasClass("hasScroll")) {
                        ele.css("width", (parseInt(ele.css("width").replace("px", "")) + 18));
                    }
                }
                OnRowChangeSetHeight(dcNumb);
            }
        }
    }
}





//$j(document).off("click", "#pickDown").on("click", "#pickDown", function (e) {
//    e.stopPropagation();
//    pickDownn();

//});



//$j(document).off("click", "#pickUp").on("click", "#pickUp", function (e) {
//    e.stopPropagation();
//    pickUpp();
//});


function pickDownn() {
    var pickLength = $j("#tblPickData tr").length;
    var activatedClass = "";
    activatedClass = $j('#tblPickData tr.active').index() + 1;

    if (activatedClass == "-1") {
        activatedClass = 0;
    }

    $j("#tblPickData tr").removeClass("active");
    if (activatedClass < (pickLength)) {

        $j("#tblPickData tr:nth-child(" + (activatedClass + 1) + ")").addClass('active');
        //}
    } else {
        $j("#tblPickData tr:nth-child(1)").addClass('active');
    }
}

function pickUpp() {
    var pickLength = $j("#tblPickData tr").length;
    var activatedClass = "";
    activatedClass = $j('#tblPickData tr.active').index() + 1;
    if (activatedClass == "0") {
        activatedClass = 1;
    }

    $j("#tblPickData tr").removeClass("active");
    if (activatedClass == 1) {
        $j("#tblPickData tr:nth-last-child(1)").addClass('active');
        //}
    } else {
        $j("#tblPickData tr:nth-child(" + (activatedClass - 1) + ")").addClass('active');

    }
}


function AxCustomExportToXml() {
    var branchVal = "";
    var companyVal = "";
    var dateVal = "";
    var url = "";
    branchVal = GetFieldValue("branch000F1").replace(/\&/g, '¿');
    companyVal = GetFieldValue("company000F1").replace(/\&/g, '¿');
    dateVal = GetFieldValue("docdate000F1");
    url = "ExportasXML.aspx?tid=" + transid + "&branch=" + branchVal + "&comp=" + companyVal + "&vcd=" + dateVal;
    window.open(url, "_self");
}



function ChangeDir(dir) {
    $j("#form1").attr("dir", dir);

}

//function AdjustIframeHeight() {
//    //var heightOfContainer = $j("#wBdr").css("height");
//    //// $j("#middle1").css("height", heightOfContainer);
//    //window.parent.document.getElementById("middle1").height
//}




function LoadPdfDDL(ddlStr) {
    document.getElementById("dvPdfDDl").innerHTML = ddlStr;
}



(function ($) {

    'use strict';

    $(document).on('show.bs.tab', '.nav-tabs-responsive [data-toggle="tab"]', function (e) {
        //if (!checkEditMode()) {
        //    e.preventDefalut();
        //    e.stopPropagation();
        //    return;
        //}

        var $target = $(e.target);
        var $tabs = $target.closest('.nav-tabs-responsive');
        var $current = $target.closest('li');
        var $parent = $current.closest('li.dropdown');
        $current = $parent.length > 0 ? $parent : $current;
        var $next = $current.next();
        var $prev = $current.prev();
        var updateDropdownMenu = function ($el, position) {
            $el
                .find('.dropdown-menu')
                .removeClass('pull-xs-left pull-xs-center pull-xs-right')
                .addClass('pull-xs-' + position);
        };

        $tabs.find('>li').removeClass('next prev');
        $prev.addClass('prev');
        $next.addClass('next');

        updateDropdownMenu($prev, 'left');
        updateDropdownMenu($current, 'center');
        updateDropdownMenu($next, 'right');
    });


})(jQuery);

$j(document).on('click', '.printhtmltopdf', function () {
    //$j(".printhtmltopdf").on('click', function () {
    eval(callParent('isPrintPDFClick') + "= false");
    if (currentCK != "")
        ShowdivContentCK($j("#" + currentCK), false);

    if (eval(callParent('globalChange'))) {
        var cutMsg = eval(callParent('lcm[31]'));
        if (confirm(cutMsg)) {
            eval(callParent('isPrintPDFClick') + "= true");
            SetFormDirty(false);
            FormSubmit();
            return;
        }
    }
    ShowDimmer(true);
    var formId = $j(this).attr("id");
    var rid = $j("#recordid000F0").val();
    if (rid == "0") {
        ShowDimmer(false);
        ShowDialog("error", 2021, "client");
        return;
    } else {
        $j.ajax({
            url: 'htmltopdf.aspx/Getpdftohtml',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                key: tstDataId,
                fname: formId
            }),
            dataType: 'json',
            contentType: "application/json",
            beforeSend: function () {
                ShowDimmer(true);
            },
            success: function (data) {
                if (data.d != null || data.d != "") {
                    if (data.d.split('~')[0] != "error" && data.d.split('~')[0] != "session") {
                        var waitTime = 1000;
                        if (jQBrowser.chrome || jQBrowser.opera || jQBrowser.safari) {
                            try {
                                printJS(data.d);
                            } catch (ex) {
                                var URL = data.d;
                                var W;
                                try {
                                    W = window.open(URL, "", "width=600,height=500,scrollbars=yes,resizable=yes");
                                    W.window.focus();
                                } catch (ex) {
                                    showAlertDialog("warning", eval(callParent('lcm[356]')));
                                }
                            }
                        } else if (jQBrowser.mozilla) {
                            var URL = data.d;
                            var W;
                            try {
                                W = window.open(URL, "", "width=600,height=500,scrollbars=yes,resizable=yes");
                                W.window.focus();
                            } catch (ex) {
                                showAlertDialog("warning", eval(callParent('lcm[356]')));
                            }

                            //W.window.print();
                        } else if (jQBrowser.msie) {
                            var URL = data.d;
                            var W;
                            try {
                                W = window.open(URL);
                                W.window.focus();
                            } catch (ex) {
                                showAlertDialog("warning", eval(callParent('lcm[356]')));
                            }
                        } else if (jQBrowser.msedge) {
                            var fileUrl = data.d;
                            var win;
                            try {
                                win = window.open("", "htmltopdf", "width=600,height=400,menubar=yes,toolbar=yes,location=yes,status=yes,scrollbars=auto,resizable=yes");
                                win.location.href = fileUrl;
                                win.focus();
                                setTimeout(function () {
                                    win.print();
                                }, 5000);
                            } catch (ex) {
                                showAlertDialog("warning", eval(callParent('lcm[356]')));
                            }
                            waitTime = 1000;
                        } else {
                            var URL = data.d;
                            var W;
                            try {
                                W = window.open(URL);
                                W.window.focus();
                            } catch (ex) {
                                showAlertDialog("warning", eval(callParent('lcm[356]')));
                            }
                        }
                        setTimeout(function () {
                            deletepdffile(data.d);
                        }, waitTime);
                    } else if (data.d.split('~')[0] == "session") {
                        ShowDimmer(false);
                        parent.parent.location.href = data.d.split('~')[1];
                    } else {
                        ShowDimmer(false);
                        ShowDialog("warning", data.d.split('~')[1]);
                    }
                } else {
                    ShowDimmer(false);
                    ShowDialog("warning", 2022, "client");
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                ShowDimmer(false);
            }
        });
    }
});

function deletepdffile(filePath) {
    $j.ajax({
        url: 'htmltopdf.aspx/DeletePrintPDF',
        type: 'POST',
        cache: false,
        data: JSON.stringify({
            pdfPath: filePath
        }),
        async: false,
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            ShowDimmer(false);
        },
        error: function (xhr, textStatus, errorThrown) {
            ShowDimmer(false);
        }
    });
}

function resetActions() {
    $("#comment").val("");
    $("#message").text("");
}

function clickOnWrkBtn() {

    var optionSel = $("#selectbox").find('div.selected').text();

    //if($("#comment").val() != ""){
    //var optionSel = $("#selectbox option:selected").text();

    switch (optionSel) {
        case "Approve":
            $("#btntabapprove").click();
            break;
        case "Reject":
            $("#btntabreject").click();
            break;
        case "Review & Forward":
            $("#btntabreview").click();
            break;
        case "Return":
            $("#btntabreturn").click();
            break;
        default:
            break;
    }
    //}
    //else{
    //    showAlertDialog("error", eval(callParent('lcm[475]')));
    //}
}

function SlectBoxWrf(wftype) {
    if (isWizardTstruct) {
        $("body").append($("#consumergoods2").detach());
    }
    $("#accordion").addClass("d-none");
    let proceed = true;
    try {
        proceed = AxBeforewfonBtnClick(wftype);
    } catch (error) {
        proceed = true;
    }
    if (typeof proceed == "undefined" || proceed) {
        let _mwfComments = "true";
        if (typeof AxMandatoryWfComments != "undefined" && AxMandatoryWfComments != "")
            _mwfComments = AxMandatoryWfComments;
        $("#selectbox div.selected").removeClass("selected");
        $("#" + wftype).addClass("selected");
        if (typeof tstWFpdcomments != "undefined" && tstWFpdcomments == "true" && typeof tstWorkFlowId != "undefined" && tstWorkFlowId != "") {
            GetWFPdComments(wftype, tstWorkFlowId);
        } else {
            let _wfpopComments = 'Comments';
            if (_mwfComments == "true") {
                $("#comment").val(wftype);
                _wfpopComments = 'Comments *';
            }
            $("#consumergoods2").removeClass("d-none");
            let myModal = new BSModal("modalIdWfComments", _wfpopComments, $("#consumergoods2").html(), () => {
                if (_mwfComments == "true")
                    $("#comment").val(wftype);
                $("#consumergoods2").addClass("d-none");
            }, () => {
                // resetActions();
            });
            myModal.scrollableDialog();
            myModal.okBtn.innerText = "Ok";
            myModal.okBtn.setAttribute("onClick", "clickOnWrkBtn()");
            myModal.cancelBtn.innerText = "Cancel";
            myModal.cancelBtn.setAttribute("onClick", "resetActions()");
        }
    }
}

function GetWFPdComments(wfpType, workFlowId) {
    $.ajax({
        url: 'tstruct.aspx/GetWfPdComments',
        type: 'POST',
        cache: false,
        async: false,
        data: JSON.stringify({
            wftype: wfpType,
            wfid: workFlowId,
            strTransid: transid
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (msg) {
            if (msg.d == "SESSION_TIMEOUT") {
                parent.window.location.href = "../aspx/sess.aspx";
            } else if (msg.d != "") {
                dataJSON = JSON.parse(msg.d);
                if (typeof dataJSON.error == "undefined") {
                    var tdRow = "";
                    if (dataJSON.result.row.length > 0) {
                        for (var i = 0; i < dataJSON.result.row.length; i++) {
                            tdRow += "<tr style=\"cursor: pointer;\"><td style=\"width: 1px;\">";
                            tdRow += "<input type=\"checkbox\" class=\"fgChk\" name=\"chkItem\" onclick=\"javascript:wfaCheckbox(this);\"></td>";
                            //tdRow+="<td>"+ dataJSON.result.row[i].WFCOMMENTS+"</td></tr>";
                            tdRow += "<td>" + getCaseInSensitiveJsonProperty(dataJSON.result.row[i], "wfcomments") + "</td></tr>";
                        }
                        $("#btnwfapprove").show();
                    } else {
                        tdRow = "No predefined comments found.";
                        $("#btnwfapprove").hide();
                    }
                    var tblData = "<table id=\"tblwfpdcomments\" class=\"table\">";
                    tblData += "<tbody>";
                    tblData += tdRow;
                    tblData += "</tbody>";
                    tblData += "</table>";
                    $("#wfpdcomments").html(tblData);
                    // $('#wfpdcommentmodal').modal('show');
                    $("#wfpdcommentmodal").removeClass("d-none");
                    let myModal = new BSModal("modalIdWfPdcComm", "Choose Comments", $("#wfpdcomments").html(), () => {
                        $("#wfpdcommentmodal").addClass("d-none");
                    }, () => {
                        //hide callback
                    });
                    myModal.scrollableDialog();
                    myModal.okBtn.innerText = "Ok";
                    myModal.okBtn.setAttribute("onClick", "clickOnWrkBtnNew()");
                    myModal.cancelBtn.innerText = "Cancel";
                    myModal.cancelBtn.setAttribute("onClick", "resetActions()");
                } else {
                    ShowDialog("error", dataJSON.error.msg);
                }
            }
        },
        error: function () {
            ShowDialog("error", "Error while getting workflow predefined comments.");
        }
    });
}

function wfaCheckbox(obj) {
    if ($j(".fgChk:checked").length > 0) {
        $j(".fgChk").prop("checked", false);
        $j(obj).prop("checked", true);
    } else
        $j(obj).prop("checked", true);
}

function clickOnWrkBtnNew() {
    if ($j(".fgChk:checked").length > 0) {
        var selectedTxt = $j(".fgChk:checked").closest('td').next('td')[0].innerText;
        $("#comment").val(selectedTxt);
        clickOnWrkBtn();
    } else
        ShowDialog("warning", "No comments selected.");
}

function TstructTabEventsInPopUP(gridDcNo) {
    $(".firstFocusable").removeClass("firstFocusable").off("keydown.tabRot")
    $(".lastFocusable").removeClass("lastFocusable").off("keydown.tabRot")
    //var ele = $j("#contentScroll" + dcNumber);
    var elemntsToCheck = 'button[tabindex!="-1"],a[tabindex!="-1"],input[tabindex!="-1"],textarea[tabindex!="-1"],select[tabindex!="-1"],table tbody tr[tabindex!="-1"]';
    var inputs = $('#dvlayout').find(elemntsToCheck).filter(':visible').not(':disabled');
    var gridLastInput = "";
    if ($j(".wrapperForGridData" + gridDcNo + " tbody tr").length > 0) {
        var Gridinputs = $j(".wrapperForGridData" + gridDcNo + " tbody tr").find(elemntsToCheck).filter(':visible').not(':disabled');
        gridLastInput = Gridinputs.last();
    }
    //var inputs = $('#dvlayout :focusable');
    firstInput = inputs.first();
    lastInput = inputs.last();
    if ($('#imgSaveTst').length != 0)
        $('#imgSaveTst').addClass("firstFocusable");
    else
        firstInput.addClass("firstFocusable");
    lastInput.addClass("lastFocusable");
    $(".firstFocusable").focus();
    if (gridLastInput != "") {
        lastInput.removeClass("lastFocusable");
        gridLastInput.addClass("lastFocusable");
    }

    $(".lastFocusable").on('keydown.tabRot', function (e) {
        //for inline grid, in tstruct listview popup record, if any grid row is in editable mode then prevent focusing on firstFocusable element when user press tabkey on delete icon
        if (axInlineGridEdit && $(".inline-edit").length) {
            return;
        }

        if ((e.which === 9 && !e.shiftKey)) {
            e.preventDefault();
            $(".firstFocusable").focus();
        }
    });
    $(".firstFocusable").on('keydown.tabRot', function (e) {
        if ((e.which === 9 && e.shiftKey)) {
            e.preventDefault();
            $(".lastFocusable").focus();
        }
    });

}

function SetGridElementsHeight() {
    //$j("#containerDc [id^=wrapperForEditFields] input[type=checkbox], #bootstrapModalData [id^=wrapperForEditFields] input[type=checkbox]").each(function () {
    $j("[id^=containerDc] [id^=wrapperForEditFields] input[type=checkbox], #bootstrapModalData [id^=wrapperForEditFields] input[type=checkbox]").each(function () {
        var isCompMode = false;
        if (($("#ckbCompressedMode").length && $("#ckbCompressedMode").prop('checked') == true) || ($j("#hdnCompMode").length && $j("#hdnCompMode").val().toLowerCase() == "true")) isCompMode = true;

        if ($(this).parent().is("div")) {
            if (isCompMode) {
                $(this).parent().css("min-height", "23px");
                $(this).parent().css("height", "23px");
            } else {
                $(this).parent().css("min-height", "24px");
                $(this).parent().css("height", "24px");
            }
        }
    });
}

function lnkNextClick() {
    if ($("#lnkNext").length > 0 && $("#lnkNext").prop("disabled") == true) {
        return false;
    }
    //$("#lnkPrev").addClass("disabled").prop("disabled", true);
    $("#lnkNext").addClass("disabled").prop("disabled", true);
    lvNavDetails = "";
    if (navValidator) {
        eval(callParent("loadFrame()", "function"));
        ASB.WebService.GetListViewNext(findGetParameter("openerIV"), tstDataId, resTstHtmlLS, SuccGetLVDetails, OnExceptionGetLVDetails);
    } else {
        var cutMsg = eval(callParent('lcm[284]'));
        var lvConfirm = $.confirm({
            theme: 'modern',
            title: eval(callParent('lcm[155]')),
            content: cutMsg,
            onContentReady: function () {
                disableBackDrop('bind');
            },
            escapeKey: 'buttonB',
            //rtl: isRTL,
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[279]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        lvNavDetails = "next";
                        FormSubmit();
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[280]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        eval(callParent("loadFrame()", "function"));
                        disableBackDrop('destroy');
                        ASB.WebService.GetListViewNext(findGetParameter("openerIV"), tstDataId, resTstHtmlLS, SuccGetLVDetails, OnExceptionGetLVDetails);
                    }
                }
            }
        });
    }
}

function lnkPrevClick() {
    if ($("#lnkPrev").length > 0 && $("#lnkPrev").prop("disabled") == true) {
        return false;
    }
    $("#lnkPrev").addClass("disabled").prop("disabled", true);
    //$("#lnkNext").addClass("disabled").prop("disabled", true);
    lvNavDetails = "";
    if (navValidator) {
        eval(callParent("loadFrame()", "function"));
        ASB.WebService.GetListViewPrev(findGetParameter("openerIV"), tstDataId, resTstHtmlLS, SuccGetLVDetails, OnExceptionGetLVDetails);
    } else {
        var cutMsg = eval(callParent('lcm[284]'));
        var lvConfirm = $.confirm({
            theme: 'modern',
            title: eval(callParent('lcm[155]')),
            content: cutMsg,
            onContentReady: function () {
                disableBackDrop('bind');
            },
            escapeKey: 'buttonB',
            //rtl: true,
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[279]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        lvNavDetails = "prev";
                        FormSubmit();
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[280]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        eval(callParent("loadFrame()", "function"));
                        disableBackDrop('destroy');
                        ASB.WebService.GetListViewPrev(findGetParameter("openerIV"), tstDataId, resTstHtmlLS, SuccGetLVDetails, OnExceptionGetLVDetails);
                    }
                }
            }
        });

    }
}

// function SuccGetLVPrvDetails(result, eventArgs) {
//     if (result != "") {
//         if (CheckSessionTimeout(result))
//             return;
//         if (result.indexOf("sess.aspx") == 0 || result.indexOf("err.aspx") == 0) {
//             top.location.href = result;
//         } else {
//             var redirLocation = result;
//             var axPopData = findGetParameter("AxPop");
//             if (axPopData != null) {
//                 redirLocation = redirLocation + "&AxPop=" + axPopData;
//             }
//             if (redirLocation.toLowerCase().indexOf("tstruct.aspx?") > -1)
//                 AvoidPostBackAfterSave(redirLocation);
//             else
//                 window.location.href = redirLocation;
//             $("#lnkPrev").removeClass("disabled").prop("disabled", false);
//             $("#lnkNext").removeClass("disabled").prop("disabled", false);
//         }
//     } else {
//         eval(callParent("closeFrame()", "function"));
//     }
// }

function SuccGetLVDetails(result, eventArgs) {
    if (result != "") {
        if (result.split("♠*♠").length > 1) {
            tstDataId = result.split("♠*♠")[0];
            result = result.split("♠*♠")[1];
        }
        if (CheckSessionTimeout(result))
            return;
        resTstHtmlLS = "";
        if (result.indexOf("sess.aspx") == 0 || result.indexOf("err.aspx") == 0) {
            top.location.href = result;
        } else {
            var redirLocation = result;
            var axPopData = findGetParameter("AxPop");
            if (axPopData != null) {
                redirLocation = redirLocation + "&AxPop=" + axPopData;
            }

            let dynNavTst = findGetParameter("dynNavTst", redirLocation)

            if (redirLocation.toLowerCase().indexOf("tstruct.aspx?") > -1 && (!dynNavTst || dynNavTst == transid)) {
                var _TstlocalStorage = "";
                try {
                    if (typeof (Storage) !== "undefined") {
                        let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                        let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + transid + "-" + appSUrl + "♥)", "function");
                        _TstlocalStorage = localStorage[_thisKey[0]];
                        if (typeof _TstlocalStorage == "undefined") {
                            _TstlocalStorage = "";
                        }
                    }
                } catch (e) {
                }
                if (_TstlocalStorage != "") {
                    redirLocation += "&dummyload=true♠" + tstDataId;
                } else {
                    redirLocation += "&dummyload=false♠";
                }
                callParentNew("lastLoadtstId=", redirLocation);
                resTstHtmlLS = transid;
                AvoidPostBackAfterSave(redirLocation, 'true');
            }
            else {
                if (redirLocation.toLowerCase().indexOf("tstruct.aspx?") > -1 && findGetParameter("hltype", redirLocation) == "load") {
                    redirLocation = redirLocation.replace("tstruct.aspx?", "ivtstload.aspx?");
                }
                if (redirLocation != '')
                    window.location.href = redirLocation;
                else {
                    eval(callParent("closeFrame()", "function"));
                }
            }
            if (redirLocation != '') {
                $("#lnkNext").removeClass("disabled").prop("disabled", false);
                $("#lnkPrev").removeClass("disabled").prop("disabled", false);
            }
        }
    } else {
        eval(callParent("closeFrame()", "function"));
    }

    try {
        AxAfterSuccGetLVDetails();
    } catch (ex) {
    }
}

function OnExceptionGetLVDetails(result) {
    eval(callParent("closeFrame()", "function"));
    console.log("ListView Navigation Exception");
}

function setListviewButtons() {
    if (recordid != "0") {
        var pageType = findGetParameter("pageType");
        if (pageType == "single") {
            $("#lnkPrev").addClass("disabled").prop("disabled", true);
            $("#lnkNext").addClass("disabled").prop("disabled", true);
        } else if (pageType == "first") {
            $("#lnkPrev").addClass("disabled").prop("disabled", true);
        } else if (pageType == "last") {
            $("#lnkNext").addClass("disabled").prop("disabled", true);
        }
    }
}

function toggleGridStretch(dcNo, forceStretch, calledFrom) {
    forceStretch = forceStretch || false;
    calledFrom = typeof calledFrom != "undefined" ? calledFrom : "default";
    if (calledFrom != "firstLoad") {
        designChanged = true;
        $('.grid-stack').addClass('dirty');
        changeStatus("notSaved");
    }
    if (!forceStretch && ($("#ckbGridStretch" + dcNo).prop("checked") == true)) {
        $(".wrapperForGridData" + dcNo + " table").css("max-width", "").css("min-width", "");
        if ($(".wrapperForGridData" + dcNo + " table").css("display") == "block") {
            fixGridStretch(dcNo, "reset");
        } else {
            $(".wrapperForGridData" + dcNo + " table").css("display", "block");
        }
    } else {
        $(".wrapperForGridData" + dcNo + " table").css("max-width", "100%").css("min-width", "100%");
        if ($(".wrapperForGridData" + dcNo + "").outerWidth(true) < $(".wrapperForGridData" + dcNo + " table").outerWidth(true)) {
            fixGridStretch(dcNo, "stretch");
        } else {
            $(".wrapperForGridData" + dcNo + " table").css("display", "");
        }
    }
}

function fixGridStretch(dcNo, type) {
    $(".wrapperForGridData" + dcNo + " table").css("max-width", "100%").css("min-width", "100%");
    if (type == "stretch") {
        $(".wrapperForGridData" + dcNo + " table").css("display", "block");
    } else if (type == "reset") {
        $(".wrapperForGridData" + dcNo + " table").css("display", "");
    }
    $(".wrapperForGridData" + dcNo + " table thead tr th").each(function (i) {
        if (type == "stretch") {
            $(this).data("min-width", $(this).css("min-width")).css("min-width", "");
        } else if (type == "reset") {
            $(this).css("min-width", $(this).data("min-width")).data("min-width", "");
        }
    });
}

function gridFreezeCols(thisDcno = "") {
    try {
        let _freezCols = gridFreezeColName;
        if (_freezCols != null && _freezCols != "") {
            _freezCols.split(',').forEach(function (ele, ind) {
                let _grflds = ele.split('~');
                if (theModeDesign == "true") {
                    $("#ckbFreezeColumns" + _grflds[1]).prop("checked", true);
                } else {
                    if (thisDcno == "" || thisDcno == _grflds[1]) {
                        var _thisGrd = "gridHd" + _grflds[1];
                        var _thIndex = $("#th-" + _grflds[0]).index();
                        for (var i = 0; i <= _thIndex; i++) {
                            var offsetth = parseInt($("#" + _thisGrd + " thead th:eq(" + i + ")").offset().left - 10, 10);
                            var offsettd = parseInt($("#" + _thisGrd + " tbody td:eq(" + i + ")").offset().left - 10, 10);

                            if (offsettd == -10) {
                                offsettd = 0;
                                offsetth = 0;
                            }
                            $("#" + _thisGrd + " thead th:eq(" + i + ")").css({ "position": "sticky", "background-color": "rgb(251 218 141)", "left": offsetth + "px", "z-index": "3" });
                            $("#" + _thisGrd + " tbody tr").each(function () {
                                $(this).find("td:eq(" + i + ")").css({ "position": "sticky", "background-color": "rgb(245 235 214)", "z-index": "2", "left": offsettd + "px", "box-shadow": "none" });
                            });
                            if ($("#dvlayout .footer").length > 0) {
                                $("#dvlayout .footer").css({ "z-index": "4" });
                            }
                        }
                    }
                }
            });
        }
    } catch (ex) { }
}

function FreezeGridCols(dcNo) {
    let _thisDcFlds = GetGridFields(dcNo);
    let _sldFld = gridFreezeColName;
    var tblData = `<div class="row my-3">`;
    tblData += `<label class="form-label col-form-label">Select Freeze Column</label>`;
    tblData += `<div class="input-group">`;
    tblData += `<select class="form-control form-select" id="dllFreezeSel">`;
    tblData += `<option value="">--No Freeze--</option>`;
    _thisDcFlds.forEach(function (ele) {
        if (ele.indexOf('axp_recid') == -1) {
            let _fldInd = GetFieldIndex(ele);
            let _fldCapt = GetFieldCaption(ele, _fldInd);
            if (_sldFld != null && _sldFld != "" && _sldFld.indexOf(ele + "~") > -1)
                tblData += `<option value=` + ele + ` selected>` + _fldCapt + " (" + ele + ")" + `</option>`;
            else
                tblData += `<option value=` + ele + `>` + _fldCapt + " (" + ele + ")" + `</option>`;
        }
    });
    tblData += `</select>`;
    tblData += `</div>`;
    tblData += `</div>`;

    let myModal = new BSModal("SelectFreezeCol", "Select Freeze Column", tblData, () => {
    }, () => {
    });
    myModal.scrollableDialog();
    myModal.okBtn.innerText = "Ok";
    myModal.okBtn.setAttribute("onClick", "GridFreezeColumn(" + dcNo + ")");
}

function GridFreezeColumn(dcNo) {
    if ($("#dllFreezeSel").val() != "") {
        if (gridFreezeColName != "") {
            let _thisSelelct = $("#dllFreezeSel").val() + "~" + dcNo;
            let _thisSelectfreez = gridFreezeColName;
            gridFreezeColName = "";
            _thisSelectfreez.split(',').forEach(function (ele, ind) {
                let _grflds = ele.split('~');
                if (_grflds[1] == dcNo) {
                    if (gridFreezeColName == "")
                        gridFreezeColName = _thisSelelct;
                    else
                        gridFreezeColName += "," + _thisSelelct;
                } else {
                    if (gridFreezeColName == "")
                        gridFreezeColName = ele;
                    else
                        gridFreezeColName += "," + ele;
                }
            });
            if (gridFreezeColName != null && gridFreezeColName != "" && gridFreezeColName.indexOf(_thisSelelct) == -1) {
                gridFreezeColName += "," + _thisSelelct;
            }
        }
        else
            gridFreezeColName = $("#dllFreezeSel").val() + "~" + dcNo;
    } else {
        if (gridFreezeColName != "") {
            let _thisSelectfreez = gridFreezeColName;
            gridFreezeColName = "";
            _thisSelectfreez.split(',').forEach(function (ele, ind) {
                let _grflds = ele.split('~');
                if (_grflds[1] != dcNo) {
                    if (gridFreezeColName == "")
                        gridFreezeColName = ele;
                    else
                        gridFreezeColName += "," + ele;
                }
            });
        }
    }

    try {
        $.ajax({
            url: 'tstruct.aspx/SaveFreezeCols',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                transid: transid,
                freezcols: gridFreezeColName
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                //window.location.href = window.location.href;
                try {
                    let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                    let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + transid + "-" + appSUrl + "♥)", "function");
                    if (_thisKey.length > 0) {
                        for (const val of _thisKey) {
                            localStorage.removeItem(val);
                        }
                    }
                } catch (ex) { }
                let _ThisSrc = window.location.href;
                _ThisSrc = _ThisSrc.replace('&dummyload=true', '&dummyload=false');
                _ThisSrc = GetUriFormCurrentUri(_ThisSrc, transid);
                window.location.href = _ThisSrc
            },
            error: function (error) {
            }
        });
    } catch (exp) {
    }
}

function disableBtnBeforeReload(flag) {
    flag = typeof flag == "undefined" ? true : flag;
    flag ? $("#icons").find("a").css("pointer-events", "none").prop("disabled", true).attr("disabled", "disabled") : $("#icons").find("a").css("pointer-events").prop("disabled", false).removeAttr("disabled");
    flag ? $("#btnSaveTst,.BottomToolbarBar a").css("pointer-events", "none").prop("disabled", true).attr("disabled", "disabled") : $("#btnSaveTst,.BottomToolbarBar a").css("pointer-events").prop("disabled", false).removeAttr("disabled");
}

function CheckIsAccessVoilation() {

    $("#reloaddiv").show();
    $("#dvlayout").hide();
    //$('body').append('<span id="reloaddiv">Server is unable to process your request. <a onclick="ReloadOnAccessVoilation()">Click here to retry</a></span>');

}

function ResetFormLoadCache() {
    try {
        GetCurrentTime("Tstruct load on Refresh button click(ajax call)");
        AxWaitCursor(true);
        ShowDimmer(true);
        var tstQureystr = '';
        var qrString = window.location.href;
        qrString.split('&').forEach(function (paramType) {
            tstQureystr += paramType + "♠";
        });
        GetFormLoadData(tstQureystr, "", "true")

        //ASB.WebService.ResetFormLoadCache(transid, SuccResetFormLoadCache, OnExceptionFormLoad);
        //window.location.href = window.location.href+"&forceRefresh=true";
    } catch (exp) {
        AxWaitCursor(false);
    }
}

function OnExceptionFormLoad(result) {
    AxWaitCursor(false);
    ShowDimmer(false);
}

function SuccResetFormLoadCache(result, eventArgs) {
    if (CheckSessionTimeout(result)) {
        AxWaitCursor(false);
        ShowDimmer(false);
        return;
    }
    if (result == "done")
        window.location.href = window.location.href;
    else {
        AxWaitCursor(false);
        ShowDimmer(false);
    }
}

function ReloadOnAccessVoilation() {
    $("#reloaddiv").hide();
    $("#dvlayout").show();
    AxWaitCursor(true);
    ShowDimmer(true);
    var funName = callBackFunDtls.split("♠")[0];
    if (funName == "GetDependents") {
        GetDependents(callBackFunDtls.split("♠")[1], callBackFunDtls.split("♠")[2]);
    } else if (funName == "createAutoComplete") {
        createAutoComplete(callBackFunDtls.split("♠")[1]);
        callBackFunDtls = "";
    } else if (funName == "GetTabData") {
        GetTabData(callBackFunDtls.split("♠")[1]);
        callBackFunDtls = "";
    } else if (funName == "CallActionExt") {
        CallActionExt(callBackFunDtls.split("♠")[1], callBackFunDtls.split("♠")[2], callBackFunDtls.split("♠")[3], callBackFunDtls.split("♠")[4]);
        callBackFunDtls = "";
    } else if (funName == "FillGridAfterConfirm") {
        FillGridAfterConfirm(callBackFunDtls.split("♠")[1]);
        callBackFunDtls = "";
    } else if (funName == "ProcessFillGrid") {
        ProcessFillGrid(callBackFunDtls.split("♠")[1], callBackFunDtls.split("♠")[2]);
        callBackFunDtls = "";
    } else if (funName == "RefreshDc") {
        ProcessFillGrid(callBackFunDtls.split("♠")[1], callBackFunDtls.split("♠")[2]);
        callBackFunDtls = "";
    }
}

function switchTstructMode() {
    if (typeof theModeDesign != "undefined" && theModeDesign == "true") {
        theMode = "design";
        isWizardTstruct = false;
        if (theMode == "design")
            $(callParentNew("splitIcon", "id")).addClass("hide");
        const _obj = $(".tstformbutton");
        _obj.prop("disabled", true);
        _obj.addClass('disabled');
        _obj.addClass('btndis');
        _obj.css("pointer-events", 'none');
        _obj.css("cursor", 'default');
        _obj.attr('tabindex', -1);
    }
    if (typeof isWizardTstruct != "undefined" && isWizardTstruct) {
        createTstructWizardNew();
    }
}

function createTstructWizardNew() {
    $(".tstructMainBottomFooter").addClass("d-none");
    $("#wbdrHtml").addClass("stepper stepper-pills card bg-transparent border-0 min-h-100");
    $("ul.nav-tabs,.dcTitle,.dcTitle+.hrline").addClass("d-none");
    let wizardTab = `<div class="card-header d-block px-0 py-5 bg-transparent border-0" id="wizardHeader"><div class="stepper-nav bg-white rounded-2 flex-center flex-wrap">`;

    if (AxpTstButtonStyle == "old") {
        $("#tstIcons #imgSaveTst,#tstIcons [title='Save' i]").addClass("d-none");

        wizardSaveActionBtn = $("#tstIcons #imgSaveTst").length == 1 ? wizardSaveActionBtn :
            $("#tstIcons [onclick*='CallAction'][title='Save' i]").length == 1 ? $("#tstIcons [onclick*='CallAction'][title='Save' i]").attr('onclick') :
            $("#tstIcons [onclick*='CallAction'][data-hint='Save' i]").length == 1 ? $("#tstIcons [onclick*='CallAction'][data-hint='Save' i]").attr('onclick') : wizardSaveActionBtn;
    } else {
        $("#iconsUl [title='Save' i]").addClass("d-none");

        wizardSaveActionBtn = $("#iconsUl [onclick*='CallAction'][title='Save' i]").length == 1 ? $("#iconsUl [onclick*='CallAction'][title='Save' i]").attr('onclick') : wizardSaveActionBtn;
    }


    for (var i = 0; i < DCName.length; i++) {
        wizardTab += `<div class="stepper-item mx-2 my-4 ` + (i == 0 ? "current" : "") + `"
        data-kt-stepper-element="nav" data-kt-stepper-action="step">
        <div class="stepper-line w-40px"></div>
        <div class="stepper-icon w-40px h-40px rounded-circle">
            <span class="stepper-check material-icons material-icons-style material-icons-2">done</span>
            <span id="wdc${(i + 1)}" class="stepper-number material-icons material-icons-style material-icons-1">web_asset</span>
        </div>
        <div class="stepper-label">
            <h3 class="stepper-title">
            ` + DCCaption[i] + `
            </h3>
        </div>
    </div>`;
    }

    wizardTab += `</div></div>`;
    $("#DivFrame1").before(wizardTab);

    let wizardBody = `<div class="card card-body">
    <div class="form" novalidate="novalidate"
        id="kt_stepper_example_basic_form">
        <div class="mb-5" id="dcContent">
        </div>
    </div>
</div>`;

    let wizardFooter = `<div class="card-footer bg-transparent border-0 px-0 py-2">
<div class="d-flex d-flex justify-content-end">
    <div class="me-2">
        <button type="button"
            class="btn btn-white btn-color-gray-700 btn-active-primary shadow-sm"
            data-kt-stepper-action="previous">
            Back
        </button>
    </div>
    <div>
        <button type="button" class="btn btn-primary shadow-sm"
            data-kt-stepper-action="submit" onclick="` + wizardSaveActionBtn + `">
            <span class="indicator-label">
                Submit
            </span>
            <span class="indicator-progress">
                Please wait... <span
                    class="spinner-border spinner-border-sm align-middle ms-2"></span>
            </span>
        </button>

        <button type="button" class="btn btn-primary shadow-sm"
            data-kt-stepper-action="next">
            Continue
        </button>
    </div>
</div>
</div>`;

    $("#wizardHeader").after(wizardBody + wizardFooter);
    $(".divGridContent").addClass("d-none");
    for (var i = 0; i < DCName.length; i++) {
        let tblStyle = "";
        let DivId = "#DivFrame" + (i + 1);
        if (DCIsGrid[i] == "True") {
            tblStyle = `overflow-auto w-` + $("#gridHd" + (i + 1)).width() + `px`;
            $(DivId).find(".card-body").removeClass().addClass("me-9");
        }
        if (i == 0) {
            $("#dcContent").append(`<div class="flex-column ` + tblStyle + ` current" data-kt-stepper-element="content" id="wizardDc1"></div>`);
            $("#dcContent").find(".flex-column:last").append($(DivId).detach());
        } else {
            $("#dcContent").append(`<div class="flex-column ` + tblStyle + ` " data-kt-stepper-element="content" id="wizardDc` + (i + 1) + `"></div>`);
            $("#dcContent").find(".flex-column:last").append($(DivId).detach());
        }
        $(DivId).find(".card-xl-stretch").removeClass();
    }
}

function WizardTabDcHtml(wdDcNo) {
    $("#wizardDc" + wdDcNo).append($("#DivFrame" + wdDcNo).detach());
    $("#DivFrame" + wdDcNo).find(".card-xl-stretch").removeClass();
    $(".divGridContent").addClass("d-none");
    // let IsGriddc = DCIsGrid[i];
    // if (IsGriddc) {
    //     tblStyle = `overflow-auto w-` + $("#gridHd" + (i + 1)).width() + `px`;
    //     $(DivId).find(".card-body").removeClass().addClass("me-9");
    // }
}

function ToggleWizardDc(dcNo, action) {
    if (action == "hide") {
        $("#wdc"+dcNo).parents(".stepper-item").addClass("d-none");
        $("#DivFrame" + dcNo).addClass("d-none");
        if (wizardHidenDcNos.length > 0) {
            if (!wizardHidenDcNos.includes(dcNo))
                wizardHidenDcNos.push(dcNo);
        } else
            wizardHidenDcNos.push(dcNo);
    } else if (action == "show") {
        $("#wdc"+dcNo).parents(".stepper-item").removeClass("d-none");
        $("#DivFrame" + dcNo).removeClass("d-none");
        if (wizardHidenDcNos.length > 0)
            wizardHidenDcNos.splice($.inArray(dcNo, wizardHidenDcNos), 1);
    }
}

function CompressMode(isCompress) {
    if (isCompress === "true" || isCompress === true) {
        $("#wBdr").find(".grid-stack").each(function (ind, gsElem) {
            if ($(gsElem).data('gridstack')) {
                if (typeof dcLayoutType == "undefined" || dcLayoutType == "" || dcLayoutType == "default") {
                    $(gsElem).data('gridstack').cellHeight(gsConf.compressedMode.cellHeight, false);
                    $(gsElem).data('gridstack').verticalMargin(gsConf.compressedMode.verticalMargin, false);
                } else {
                    $(gsElem).data('gridstack').cellHeight(gsConf.compressedMode.cellHeight - gsConf.compressedMode.labelHeight, false);
                    $(gsElem).data('gridstack').verticalMargin(gsConf.compressedMode.verticalMargin + 10, false);
                }
            }
            else{
                injectGridStackDynamicMargin(`.grid-stack-item{margin-bottom: ${gsConf.compressedMode.verticalMargin}px !important;}`);
            }
        });

        if (theMode == "design") {
            $("#ckbCompressedMode").prop('checked', true);
        }
    } else {
        $("#wBdr").find(".grid-stack").each(function (ind, gsElem) {
            if ($(gsElem).data('gridstack')) {
                if (typeof dcLayoutType == "undefined" || dcLayoutType == "" || dcLayoutType == "default") {
                    $(gsElem).data('gridstack').cellHeight(gsConf.normalMode.cellHeight, false);
                    $(gsElem).data('gridstack').verticalMargin(gsConf.normalMode.verticalMargin, false);
                }
                else {
                    $(gsElem).data('gridstack').cellHeight(gsConf.normalMode.cellHeight - gsConf.normalMode.labelHeight, false);
                    $(gsElem).data('gridstack').verticalMargin(gsConf.normalMode.verticalMargin + 10, false);
                }
            }
            else{
                injectGridStackDynamicMargin(`.grid-stack-item{margin-bottom: ${gsConf.normalMode.verticalMargin}px !important;}`);
            }
        });
        if (theMode == "design") {
            $("#ckbCompressedMode").prop('checked', false);
        }
    }

    function injectGridStackDynamicMargin(css){
        var style = document.createElement("style");

        var head = document.head || document.querySelectorAll("head")[0] || document.documentElement;

        // style.innerText = "body{background:red!important;}";
        style.innerText = css;

        // head.insertBefore( style, head.firstChild );
        head.appendChild(style);
    }
}

var primaryApiSettings = function () {
    return {
        "async": true,
        "crossDomain": true,
        "method": "POST",
        "headers": {
            "content-type": "application/x-www-form-urlencoded"
        },
        "data": {
            "session_id": callParentNew("thmSid"),
            "utl": callParentNew("utl"),
            "username": callParentNew("thmUser"),
            "authorization": callParentNew("nodeAccessToken"),
            "appSKey": appsessionKey
        }
    }
}

function getDraftsList(elem) {
    try {
        ASB.WebService.GetDraftsFromRedis(tst, tstructCaption, SuccessCallbackDraftKeys, FailureCallBackDraftKeys);
    } catch (exp) {
        AxWaitCursor(false);
    }
}

function SuccessCallbackDraftKeys(result, eventArgs) {
    if (CheckSessionTimeout(result))
        return;
}

function getDraftsListNew(elem) {
    try {
        ASB.WebService.GetDraftsFromRedisNew(tst, tstructCaption, SuccessCallbackDraftKeysNew, FailureCallBackDraftKeys);
    } catch (exp) {
        AxWaitCursor(false);
    }
}

function SuccessCallbackDraftKeysNew(result, eventArgs) {
    if (CheckSessionTimeout(result))
        return;
    if (result.indexOf('No Drafts') > -1) {
        showAlertDialog("error", "No drafts exist in this form.");
    } else {
        let myModal = new BSModal("modalIdSaveDraft", "Load Saved Drafts", "<div>" + result + "</div>", () => {
        }, () => {
        });
        myModal.okBtn.innerText = "Delete";
        let newOkBtn = myModal.okBtn.cloneNode(true);
        myModal.okBtn.parentNode.replaceChild(newOkBtn, myModal.okBtn);
        myModal.okBtn = newOkBtn;
        myModal.okBtn.removeAttribute("data-bs-dismiss");
        myModal.okBtn.addEventListener("click", function (e) {
            e.preventDefault();
            const proceed = clearDraftrediskeysNew();
            if (!proceed)
                return;
            myModal.hide();
        });
        myModal.cancelBtn.classList.add('d-none');
    }
}

function clearDraftrediskeysNew() {
    try {
        const selected = document.querySelectorAll(".draft-delete-checkbox:checked");
        if (selected.length === 0) {
            showAlertDialog("error", "Please select at least one draft to delete.");
            return false; 
        }
        const keyIds = Array.from(selected).map(chk => chk.getAttribute("data-keyid"));
        const keyIdString = keyIds.join("♥");
        $.ajax({
            url: 'tstruct.aspx/DeletedraftRediskeysNew',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                key: transid,
                keyNames: keyIdString
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                callParentNew("modalIdSaveDraft", "id").dispatchEvent(new CustomEvent("close"));
                getDraftsListNew(this);
            },
            error: function (error) {
                showAlertDialog("error", "Failed to delete selected drafts.");
            }
        });
    } catch (exp) {
        showAlertDialog("error", "Unexpected error occurred while deleting drafts.");
        return false;
    }
}

function FailureCallBackDraftKeys(result, eventArgs) {
    //Need to implement
}


function loadSavedDraft(CreatedOn) {

    var cutMsg = eval(callParent('lcm[31]'));
    var cutMsgTime = CreatedOn;
    var glType = eval(callParent('gllangType'));
    cutMsg = cutMsg.split(".")[0] + " " + "created on " + cutMsgTime + "." + "Do you want to retrieve and edit this data?";
    var isRTL = false;
    if (glType == "ar")
        isRTL = true;
    else
        isRTL = false;
    draftSetTimeoutObj = setInterval(SaveAsDraft, parseInt(AutosaveDraftTime))
    clearInterval(draftSetTimeoutObj);
    var CallListViewCB = $.confirm({
        theme: 'modern',
        closeIcon: false,
        title: eval(callParent('lcm[155]')),
        rtl: isRTL,
        onContentReady: function () {
            disableBackDrop('bind');
        },
        backgroundDismiss: true,
        escapeKey: 'buttonB',
        content: cutMsg,
        buttons: {
            buttonA: {
                text: eval(callParent('lcm[279]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    GetCurrentTime("Tstruct load on SaveDraft button click(ajax call)");
                    CallListViewCB.close();
                    ShowDimmer(true);
                    evalExpOnSaveDraftLoad = true;
                    GetFormLoadData("", "true", "false", "true");
                    //loadSavedDraftExt(draftName);
                    window.parent.isSessionCleared = true;
                    if (AutosaveDraft == "true" & AutosaveDraftTime != "") {
                        draftSetTimeoutObj = setInterval(SaveAsDraft, parseInt(AutosaveDraftTime));
                    }
                }
            },
            buttonB: {
                text: eval(callParent('lcm[280]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    ShowDimmer(false);
                    evalExpOnSaveDraftLoad = false;
                    disableBackDrop('destroy');
                    clearDraftrediskeys("true");
                    //GetFormLoadData("","false");
                    closeParentFrame();
                    window.parent.isSessionCleared = true;
                    if (AutosaveDraft == "true" & AutosaveDraftTime != "") {
                        draftSetTimeoutObj = setInterval(SaveAsDraft, parseInt(AutosaveDraftTime));
                    }
                }
            }

        }
    });
}

function loadSavedDraftNew(CreatedOn) {

    var cutMsg = eval(callParent('lcm[31]'));
    const cutMsgTime = CreatedOn;
    var glType = eval(callParent('gllangType'));
    cutMsg = cutMsg.split(".")[0] + " " + "created on " + cutMsgTime + "." + "Do you want to retrieve and edit this data?";
    var isRTL = false;
    if (glType == "ar")
        isRTL = true;
    else
        isRTL = false;
    var CallListViewCB = $.confirm({
        theme: 'modern',
        closeIcon: false,
        title: eval(callParent('lcm[155]')),
        rtl: isRTL,
        onContentReady: function () {
            disableBackDrop('bind');
        },
        backgroundDismiss: true,
        escapeKey: 'buttonB',
        content: cutMsg,
        buttons: {
            buttonA: {
                text: eval(callParent('lcm[279]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    GetCurrentTime("Tstruct load on SaveDraft button click(ajax call)");
                    CallListViewCB.close();
                    ShowDimmer(true);
                    evalExpOnSaveDraftLoad = true;
                    callParentNew("modalIdSaveDraft", "id").dispatchEvent(new CustomEvent("close"));
                    closeParentFrame();
                    GetFormLoadData("", "true♦" + cutMsgTime, "false", "true");
                    window.parent.isSessionCleared = true;
                }
            },
            buttonB: {
                text: eval(callParent('lcm[280]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    ShowDimmer(false);
                    evalExpOnSaveDraftLoad = false;
                    disableBackDrop('destroy');
                    //clearDraftrediskeys("true");
                    closeParentFrame();
                    window.parent.isSessionCleared = true;
                }
            }

        }
    });
}

function toggleAllDrafts(source) {
    const checkboxes = document.querySelectorAll('.draft-delete-checkbox');
    checkboxes.forEach(cb => {
        cb.checked = source.checked;
    });
}

$j(document).off("change", ".draft-delete-checkbox").on("change", ".draft-delete-checkbox", function () {
    const total = $j(".draft-delete-checkbox").length;
    const checked = $j(".draft-delete-checkbox:checked").length;
    $j("#selectAllDrafts").prop("checked", total === checked);
});

function clearDraftrediskeys(Isclear) {
    try {
        $.ajax({
            url: 'tstruct.aspx/DeletedraftRediskeys',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                key: transid,
                Isclear: Isclear
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                var result = data.d;
            },
            error: function (error) {

            }
        });
    } catch (exp) {

    }
}

function loadSavedDraftExt(draftName) {
    document.getElementById('hdnDraftName').value = draftName;
    $('#hdnDraftName').attr('value', draftName);
    SetSession({
        key: "DraftName",
        val: draftName,
        async: false
    });
    window.location.href = window.location.href.replace('#', '');
}

function changeStatus(status) {
    //var elem = $("#designStatus");
    var statusIndicatorIcon = "";

    var toolTip = "";
    var statusIndicator = ""
    if (status) {
        switch (status) {
            case "save":
                statusIndicatorIcon = "icon-basic-elaboration-cloud-check";
                toolTip = "Design Saved. Not yet published";
                break;
            case "notSaved":
                statusIndicatorIcon = "icon-basic-elaboration-cloud-remove";
                toolTip = "Design not yet saved.";
                break;
            case "published":
                statusIndicatorIcon = "icon-basic-elaboration-cloud-upload";
                toolTip = "Design published.";
                break;
        }
    }

    statusIndicator = '<a title="' + toolTip + '" class="toolbarIcons widgetStatus-' + status + '"><i class="icon ' + statusIndicatorIcon + ' "></i></a>';
    $('#designStatus').html(statusIndicator);

}


function confirmOnAction(fromPropsheet) {
    var ConfirmLeave = $.confirm({
        theme: 'modern',
        title: eval(callParent('lcm[155]')),
        animation: 'scale',
        closeAnimation: 'scale',
        animateFromElement: false,
        onContentReady: function () {
            disableBackDrop('bind');
            //to display tooltips for Confirm & Cancel buttons
            $(".jconfirm-buttons button").each(function () {
                var txt = $(this).text();
                $(this).prop('title', txt.charAt(0).toUpperCase() + txt.slice(1))
            });
            $(".jconfirm-buttons .btn-primary").focus(); //to focus on Confirm button once dialog is opened
        },
        backgroundDismiss: 'false',
        escapeKey: 'cancel',
        content: eval(callParent('lcm[121]')),
        buttons: {
            confirm: {
                text: eval(callParent('lcm[164]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    ConfirmLeave.close();
                    if (!fromPropsheet) {
                        //window.location.href = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
                        //callParentNew("lastLoadtstId=", _thisRui);

                        let iframes = window.parent.document.getElementsByTagName('iframe');
                        let currentIframe = null;
                        for (let i = 0; i < iframes.length; i++) {
                            if (iframes[i].contentWindow === window) {
                                currentIframe = iframes[i];
                                break;
                            }
                        }
                        let _thisIframe = $(currentIframe).attr("id");
                        if (typeof _thisIframe != "undefined" && _thisIframe == "middle1") {
                            let _thisRui = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
                            callParentNew(`LoadIframe(${_thisRui})`, "function");
                        } else {
                            window.location.href = "tstruct.aspx" + window.location.search.replace(/&theMode=design/g, "");
                        }
                    }
                    else
                        openProprtySht('addFieldPS');
                }
            },
            cancel: {
                text: eval(callParent('lcm[192]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                    parent.actionsClicked = "";
                },
            }
        }
    });
}


function refreshEditor(tabNo) {
    $("#divDc" + tabNo).find("textarea[id*='sql_editor'],textarea[id*='SQL_EDITOR'],textarea[id*='exp_editor'],textarea[id*='EXP_EDITOR']").each(function () {
        let myeditor = $(this).data("myeditor");
        if (myeditor) {
            //refreshing the text editor
            myeditor.refresh();
        }
    });
}

function TimePickerChecker(axptmExpr) {
    //For time picker expression to enable seconds should be given in format as {HH:mm:ss} all numeric equivalent to {00:00:00} or to disable seconds it should be given as {HH:mm} all numeric equivalent to {00:00}
    var tstTimePicker = false;
    if (typeof (axptmExpr) == "string" && axptmExpr != "" && axptmExpr.charAt(0) == "{" && axptmExpr.charAt(axptmExpr.length - 1) == "}" && (axptmExpr).indexOf(":") > -1) {
        var expTime = (axptmExpr).slice(1, -1).split(':');
        expTime.forEach(function (value, i) {
            if (!isNaN(value)) {
                if (i == (expTime.length - 1) && expTime.length == 3) {
                    tstTimePicker = true;
                    return tstTimePicker;
                }
            }
        });
    }
    return tstTimePicker;
}

//To Register timepicker event
//If user wants seconds also needs to be define expression as {00:00:00} and time format will be {HH:MM:SS} else time format will be HH:MM
function TimePickerEvent(dvId, dtpkrRTL) {
    var objTimeFlds = "";
    if (dvId == undefined || dvId == "")
        objTimeFlds = $j(".tstOnlyTime");
    else
        objTimeFlds = $j(dvId + " .tstOnlyTime");
    [...objTimeFlds].forEach(function (cntrl, icnt) {
        var fldId = $(cntrl).attr("id");
        var fieldName = GetFieldsName(fldId);
        var axptmFldIndx = GetFldNamesIndx(fieldName);
        var axptmExpr = "";
        if (axptmFldIndx != -1)
            axptmExpr = Expressions[axptmFldIndx].toString();
        var tstTimePicker = TimePickerChecker(axptmExpr);
    });
}

//For getting server time through ajax call in TimePickerEvent (Only if the field name starts with axpdbtm_)
function ShowCurrentTime(setTimeFormat) {
    var temp = "";
    try {
        $.ajax({
            url: 'tstruct.aspx/GetCurrentTime',
            type: 'POST',
            cache: false,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({
                setTimeFormat: setTimeFormat,
            }),
            success: function (data) {
                if (data.d.status == 'success')
                    temp = data.d.result;
                else
                    showAlertDialog("error", "Error while getting server time.");
            },
            error: function (error) {
                showAlertDialog("error", "Error while getting server time.");
            }
        });
    } catch (exp) {
        showAlertDialog("error", "Error while getting server time.");
    }
    return temp;
}



//to display Confirm dialog before closing the form only if any changes are there in the form
function ConfirmLeave() {
    return false;
    if ($(".jconfirm").length > 0) {
        $(".jconfirm").remove();
    } else {
        var glType = eval(callParent('gllangType'));
        var isRTL = false;
        if (glType == "ar")
            isRTL = true;
        else
            isRTL = false;
        var ConfirmSaveCB = $.confirm({
            theme: 'modern',
            title: eval(callParent('lcm[155]')),
            onContentReady: function () {
                disableBackDrop('bind');
                //to display tooltips for Confirm & Cancel buttons
                $(".jconfirm-buttons button").each(function () {
                    var txt = $(this).text();
                    $(this).prop('title', txt.charAt(0).toUpperCase() + txt.slice(1))
                });
                $(".jconfirm-buttons .btn-primary").focus(); //to focus on Confirm button once dialog is opened
            },
            backgroundDismiss: 'false',
            escapeKey: 'buttonB',
            rtl: isRTL,
            content: eval(callParent('lcm[319]')),
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[164]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        ConfirmSaveCB.close();
                        parent.closeModalDialog();
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[192]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        disableBackDrop('destroy');
                        parent.actionsClicked = "";
                    },
                }
            }
        });
    }
}

function MultiGroupSelectClk(event, el) {
    var className = event.target.className;
    if (className == "dropdown-main") // className == "dropdown-option dropdown-chose" || className == "dropdown-option" || className == "dropdown-chose-list" || className == "dropdown-group")
        return;
    else { // if (className == "dropdown-display-label") {
        var fldNameMSId = $(el).find("select").attr('id');
        var parentFldVal = "";
        var fieldNameMs = fldNameMSId.substring(0, fldNameMSId.lastIndexOf("F") - 3);
        $('body #dv' + fieldNameMs).css('overflow-x', 'visible');
        $('body #dv' + fieldNameMs).css('overflow-y', 'visible');
        $('body #dv' + fieldNameMs).css('position', 'static');

        if (axInlineGridEdit == false) {
            $('#' + fldNameMSId).parents(".grid-stack-item-content").css('overflow-x', 'visible');
            $('#' + fldNameMSId).parents(".grid-stack-item-content").css('overflow-y', 'visible');
            $('#' + fldNameMSId).parents(".grid-stack-item-content").css('position', 'static');
        }
        if ($("#" + fldNameMSId).parents(".edit-mode-content").length > 0) {
            $("#" + fldNameMSId).parents(".edit-mode-content").css({
                "position": "absolute",
                "width": $("#" + fldNameMSId).parents("td").outerWidth(),
                "left": $("#" + fldNameMSId).parents("td").offset().left - 22
            });
            
            if ($("#" + fldNameMSId).parents(".gridFixedHeader").scrollTop() > 0){
                $("#" + fldNameMSId).parents(".edit-mode-content").css({
                    "position": "static"
                });
            }
        }
        var selectedList = $("#" + fldNameMSId).attr("data-selected");;
        var selSep = $("#" + fldNameMSId).attr("data-sep");
        if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
            parentFldVal = ISBoundAutoCom(fieldNameMs, fldNameMSId);
        else
            parentFldVal = ISBoundNew(fieldNameMs, fldNameMSId);
        if (jQuery.inArray(fldNameMSId, multiSelectflds) > -1) {
            if (multiSelFldParents[multiSelectflds.indexOf(fldNameMSId)] != parentFldVal) {
                multiSelFldParents[multiSelectflds.indexOf(fldNameMSId)] = parentFldVal;
                var options = GetMultiSelectValues(el, parentFldVal);
                multiSelFldResult[multiSelectflds.indexOf(fldNameMSId)] = options;
                if (selectedList != "") {
                    options = filterSelectedData(options, selectedList, selSep);
                    $("#" + fldNameMSId).parent(".dropdown-mul").find(".dropdown-selected").remove();
                }
                $(el).data('dropdown').destroy();
                $(el).dropdown({
                    data: options,
                    limitCount: 40,
                    multipleMode: 'label',
                    choice: function () {}
                });
                MultiGroupSelectChoice($(el).data('dropdown'));
            } else if (multiSelFldParents[multiSelectflds.indexOf(fldNameMSId)] == parentFldVal && IsGridField(fieldNameMs) && $(el).data('dropdown').config.data.length == 0) {
                var options = multiSelFldResult[multiSelectflds.indexOf(fldNameMSId)];
                if (selectedList != "") {
                    $("#" + fldNameMSId).parent(".dropdown-mul").find(".dropdown-selected").remove();
                    options = filterSelectedData(options, selectedList, selSep);
                    $(el).data('dropdown').update(options, true);
                } else
                    $(el).data('dropdown').update(options, false);
                MultiGroupSelectChoice($(el).data('dropdown'));
            }
        } else {
            multiSelectflds.push(fldNameMSId);
            multiSelFldParents.push(parentFldVal);
            jsonData = GetMultiSelectValues(el, parentFldVal);
            var options = jsonData;
            multiSelFldResult.push(options);
            //$(".dropdown-mul").data('dropdown').reset();
            if (selectedList != "") {
                $("#" + fldNameMSId).parent(".dropdown-mul").find(".dropdown-selected").remove();
                options = filterSelectedData(options, selectedList, selSep);
                $(el).data('dropdown').update(options, true);
            } else
                $(el).data('dropdown').update(options, false);
            MultiGroupSelectChoice($(el).data('dropdown'));
        }
    }
}

function filterSelectedData(options, selectedList, selSep) {
    var selList = selectedList.split(selSep);
    var filteredData = $(options).each(function () {
        if ($.inArray(this.mslist, selList) != -1)
            this.selected = "true";
    });
    return filteredData;
}

function MultiGroupSelectChoice(el) {
    var fldNameMs = $(el)[0].$select.attr("id");
    let fldMsValue = "";
    if (typeof $(el)[0].name != "undefined" && $(el)[0].name.length > 0) {
        let fldMsSp = $(el)[0].$select.attr("data-sep");
        jQuery.each($(el)[0].name, function (idx, elem) {
            if (fldMsValue == "")
                fldMsValue += $(elem).text();
            else
                fldMsValue += fldMsSp + $(elem).text();
        });
    } else if (typeof $(el)[0].name == "undefined" && $(el)[0].$choseList[0].children.length > 0) {
        let fldMsSp = $(el)[0].$select.attr("data-sep");
        jQuery.each($(el)[0].$choseList[0].children, function (idx, elem) {
            if ($(elem).hasClass("dropdown-selected")) {
                if (fldMsValue == "")
                    fldMsValue += $(elem)[0].innerText;
                else
                    fldMsValue += fldMsSp + $(elem)[0].innerText;
            }
        });
    }
    $(el)[0].$select.attr("data-selected", fldMsValue);
    var rcID = GetFieldsRowFrameNo(fldNameMs);
    var acFrNo = GetFieldsDcNo(fldNameMs);
    var rowNum = GetDbRowNo(GetFieldsRowNo(fldNameMs), acFrNo);
    var fldRowNo = GetFieldsRowNo(fldNameMs)
    if (IsDcGrid(acFrNo) && isGrdEditDirty)
        UpdateFieldArray(axpIsRowValid + acFrNo + fldRowNo + "F" + acFrNo, GetDbRowNo(fldRowNo, acFrNo), "", "parent", "AddRow");
    UpdateFieldArray(fldNameMs, rowNum, fldMsValue, "parent", "AutoComplete");
    UpdateAllFieldValues(fldNameMs, fldMsValue);
    MainBlur($("#" + fldNameMs));
}

function AxGetFieldData(ParentFieldId) {
    if ($("#" + ParentFieldId).prop("type") == "select-one") {
        var optionValue = [];
        var optionText = [];
        $('#' + ParentFieldId + ' option').each(function () {
            optionText.push($(this).text());
            optionValue.push($(this).val());
        });
        return [optionValue, optionText];
    }
    //else if(($("#"+ParentFieldId).prop("type")=="text" || $("#"+ParentFieldId).prop("type")=="textarea") && $("#"+ParentFieldId).hasClass("fldAutocomplete")){
    else if (($("#" + ParentFieldId).prop("type") == "text" && $("#" + ParentFieldId).hasClass("fldFromSelect")) || ($("#" + ParentFieldId).prop("type") == "textarea" && $("#" + ParentFieldId).attr("data-type").startsWith("fromselect-") == true)) {
        var custfldData = [];
        var custfldId = [];
        var custfldDep = [];
        var custData = AxGetCustSelectFldData(ParentFieldId);
        if (custData != "") {
            var JsonData = JSON.parse(custData);
            var fldData = JsonData.pickdata[3].data;
            var custfldData = [];
            $(fldData).each(function (iIndex, sElement) {
                sElement.i = sElement.i.replace(/\^\^dq/g, '"');
                custfldData.push(sElement.i);
                custfldId.push(sElement.v);
                custfldDep.push(sElement.d);
            });
        }
        return [custfldData, custfldId, custfldDep];
    }
}

function GetFieldId(AxFldName, AxRowNo, AxDcNo) {
    let AxFldId = "";
    AxFldId = AxFldName + AxRowNo + "F" + AxDcNo;
    return AxFldId;
}

function FieldTypeTable(event, el) {
    let thisFldId = typeof $(el).attr("id") == "undefined" ? $(el).parent().parent().find("input").attr("id") : $(el).attr("id");
    src = "./tsttable.aspx?fldId=" + thisFldId;
    var tableHeader = GetFieldCaption(thisFldId).length != -1 ? (GetFieldCaption(thisFldId) == "&nbsp" ? "" : GetFieldCaption(thisFldId)) : "";
    // displayBootstrapModalDialog(tableHeader, "", "330px", true, src, "", "", CallbackFunctionBootstrap);

    let myModal = new BSModal("modalIdTableField", tableHeader, "<iframe class='w-100 h-400px' src='" + src + "'></iframe>", () => {
        if (typeof isTstPop != "undefined" && isTstPop == 'True')
            $(".modal-content").css({ "height": "100%" });
        // CallbackFunctionBootstrap();
    }, () => {
        //hide callback
    });
    myModal.scrollableDialog();
    myModal.modalBody.classList.add('overflow-hidden'); 
    if (typeof transid != "undefined" && transid == "b_sql")
        myModal.changeSize("xl");
    else if (typeof isTstPop != "undefined" && isTstPop == 'True') {
        myModal.changeSize("xl");
    }
    else
        myModal.changeSize("fullscreen");
    myModal.okBtn.innerText = "Ok";
    myModal.okBtn.addEventListener("click", (e) => {
        document.getElementById('modalIdTableField').querySelector("iframe").contentWindow.AddTableData();
    });
    myModal.cancelBtn.innerText = "Clear";
    myModal.cancelBtn.addEventListener("click", (e) => {
        document.getElementById('modalIdTableField').querySelector("iframe").contentWindow.ClearTableData('fldTable');
    });
    myModal.cancelBtn.removeAttribute("data-bs-dismiss");
    myModal.okBtn.removeAttribute("data-bs-dismiss");
    myModal.modalFooter.classList.add("p-2");
}

function CallbackFunctionBootstrap(thisFldId) {
    ChangedTblFields = new Array();
    ChangedTblFieldVals = new Array();
    MainBlur($j($("#" + thisFldId)));
    $("#" + thisFldId).blur();
    $("#" + thisFldId).focus();
}


function AxpFilePathChange(fieldID) {
    var afFldName = fieldID.substr(("axpfilepath_").length);
    var axpNwePathValue = GetFieldValue(fieldID);
    if ($("#axpfile_" + afFldName).val() != "" && axpfilepathold != axpNwePathValue) {
        var attachedfileNames = "";
        if ($("#axpfile_" + afFldName).length > 0)
            attachedfileNames = $("input#axpfile_" + afFldName).val();
        else {
            if ($("input[id*='" + afFldName + "']").length > 0) {
                $("input[id*='" + afFldName + "']").each(function () {
                    if ($(this).attr("id").toLowerCase() == "axpfile_" + afFldName.toLowerCase()) {
                        attachedfileNames = $("input#" + $(this).attr("id")).val();
                        return false;
                    }
                });
            }
        }
        if (attachedfileNames != "")
            UpdateAxpFileLocation(axpfilepathold, axpNwePathValue, attachedfileNames);
    }
}

function UpdateAxpFileLocation(axpOldPath, axpNewPath, axpFileList) {
    ASB.WebService.AxpFileMoveToNewFolder(axpOldPath, axpNewPath, axpFileList, CallBackOnAxpFile);

    function CallBackOnAxpFile() {}
}

function ReloadJqueryReference() {
    if ($('script[src*="/Js/formInit?v="]').length) {
        $('script[src*="/Js/formInit?v="]').attr("id", "removeInit");

        var formInitUrl = $("#removeInit")[0].src;

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", formInitUrl);
        if (typeof fileref != "undefined") {
            $("#removeInit").remove();
            document.getElementsByTagName("head")[0].appendChild(fileref);
        }
    } else {
        $('head script[src*="/process.min.js"]').remove();
        $('head script[src*="/tstruct.min.js"]').remove();
        $('head script[src*="/helper.min.js"]').remove();
        $('head script[src*="/jsclient.min.js"]').remove();
        $('head script[src*="/main-tstruct.min.js"]').remove();
        $('head script[src*="/newGridJS.min.js"]').remove();
        $('head script[src*="/select2.min.js"]').remove();
        $('head script[src*="/multigroupselect.min.js"]').remove();
        $('head script[src*="/AxpertFormsLib.min.js"]').remove();

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/process.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/tstruct.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/helper.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/jsclient.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/main-tstruct.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/newGridJS.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/select2.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/multigroupselect.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);

        var fileref = document.createElement('script');
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", "../Js/AxpertFormsLib.min.js");
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);
    }
    try {
        AxAfterReloadJsRef();
    } catch (ex) { }
}


/**
 * @description: Export Form Grid to excel using javascript object
 * @author Prashik
 * @date 2020-07-08
 * @param {*} exportDcNo: Grid Data Container number
 */
function exportGridToExcel(exportDcNo) {
    var gridExport = {
        processGridExcelTable(containsHeader = true) {
            $("body").append($('#gridHd' + exportDcNo).clone().attr("id", "gridToExport").css({
                "position": "fixed",
                "left": ($(window).width() + 1000) + "px"
            }));

            $("#gridToExport").find("td").each((ind, elm) => {
                if ($(elm).find("textarea").length > 0) {
                    if ($(elm).find("textarea").attr("id").startsWith("EDIT~")) {
                        if ($(elm).find("select").length > 0) {
                            if ($(elm).find(".select2-selection__rendered").text().toLowerCase() == "select an option") {
                                $(elm).text("");
                            }
                            else {
                                $(elm).text($(elm).find(".select2-selection__rendered").text());
                            }
                        }
                        else {
                            $(elm).text($(elm).find("input").val());
                        }
                    }
                    else if ($(elm).find("textarea").attr("id").startsWith("txtA~")) {
                        if (($(elm).find("select").length == 1)) {
                            $(elm).text($(elm).find("select").val());
                        }
                        else {
                            var selectId = $(elm).find("textarea").attr("id").replace(/^(txtA\~)/, "");
                            $(elm).text($(elm).find("#" + selectId).val());
                        }

                    }
                    else {
                        $(elm).text($(elm).find("textarea").val());
                    }
                }
            });

            /*var editDeleteIndex = $("#gridToExport thead th").index($("#gridToExport thead th[id=uniqueEditDeleteAct" + exportDcNo + "]"));*/
            var editDeleteIndex = $("#gridToExport thead th[id=uniqueEditDeleteAct" + exportDcNo + "]").index();
            if (editDeleteIndex > -1) {
                $("#gridToExport thead th[id=uniqueEditDeleteAct" + exportDcNo + "]").addClass("d-none");
                $("#gridToExport tbody tr").each((ind, elm) => {
                    $(elm).find("td:eq(" + editDeleteIndex + ")").addClass("d-none");
                });
            }

            /*var rowNumber = $("#gridToExport thead th").index($("#gridToExport thead th[id=uniqueThHead" + exportDcNo + "]"));*/
            var rowNumber = $("#gridToExport thead th[id=uniqueThHead" + exportDcNo + "]").index();
            if (rowNumber > -1) {
                $("#gridToExport thead th[id=uniqueThHead" + exportDcNo + "]").addClass("d-none");
                $("#gridToExport tbody tr").each((ind, elm) => {
                    $(elm).find("td:eq(" + rowNumber + ")").addClass("d-none");
                });
            }
            var indexes = $("#gridToExport thead th").map(function (index) {
                var id = $(this).attr('id');
                if (/^th-btn\d+$/.test(id) || /^th-axpbutton_/.test(id)) {
                    return index;
                }
            }).get();

            if (typeof indexes != "undefined" && indexes.length > 0) {
                indexes.forEach(function (_ind) {
                    $("#gridToExport thead th:eq(" + _ind + ")").addClass("d-none");
                    $("#gridToExport tbody tr").each((ind, elm) => {
                        $(elm).find("td:eq(" + _ind + ")").addClass("d-none");
                    });
                });
            }

            $("#gridToExport thead th").each((ind, elm) => {
                if ($(elm).attr("id") && $(elm).attr("id").indexOf("th-") == 0) {
                    let _fldName = $(elm).attr("id").substr(3);
                    let _fieldIndex = FNames.indexOf(_fldName);
                    let _fldCap = FCaption[_fieldIndex];
                    $(elm).text(_fldCap + '(' + _fldName + ')');
                }
            });

            _this.header = $("#gridToExport thead").clone();

            if (!containsHeader) {
                $("#gridToExport thead").remove();
            }
        },
        generateExcel(containsHeader = true) {
            _this.processGridExcelTable(containsHeader);
            try {
                let table = $('#gridToExport')[0];
                if (table.tFoot) {
                    table.removeChild(table.tFoot); // ensures tfoot doesn't cause layout/row issues
                }
                var workbook = XLSX.utils.table_to_book($('#gridToExport')[0], {
                    sheet: DCName[parseInt(exportDcNo) - 1],
                    display: true,
                    raw: true
                });
                if (workbook) {
                    let idS = _this.header.find("th:not(.d-none):not(.none)").toArray().map(th => $(th).attr("id").substring(3));

                    let objectMaxWidth = [];

                    var sheet = workbook.Sheets[workbook.SheetNames[0]];

                    let headerCount = idS.length;

                    let rows = Math.floor(Object.keys(sheet).filter(cell => cell != "!ref").length / headerCount);

                    Object.keys(sheet).forEach(function (s, ind) {
                        var sheetColumn = s.match(/[a-zA-Z]/g) ? s.match(/[a-zA-Z]/g).join("") : "";
                        var sheetRow = s.match(/[0-9]/g) ? s.match(/[0-9]/g).join("") : "";

                        if (!sheetColumn || !sheetRow) {
                            return;
                        }

                        // var rowIndex = ind % rows;
                        var rowIndex = parseInt(sheetRow) - 1;

                        // var columnIndex = Math.floor(ind / rows);
                        var columnIndex = sheetColumn.split("").reduce((final, c, i, arr) => {
                            let thisVal = c.charCodeAt(0) - 64;
                            if (arr[i + 1]) {
                                thisVal = 26 * thisVal;
                            }
                            return thisVal + final;
                        }, 0) - 1;

                        var fieldName = idS[columnIndex];

                        var fieldIndex = FNames.indexOf(fieldName);

                        var fieldDataType = FDataType[fieldIndex];

                        var fieldApplyComma = FProps[fieldIndex][0] != "F";

                        var fieldDecimal = 0;
                        if (FCustDecimal[fieldIndex] == "True" && typeof gloAxDecimal != "undefined" && gloAxDecimal > -1)
                            fieldDecimal = gloAxDecimal;
                        else
                            fieldDecimal = FDecimal[fieldIndex];

                        let decimals = "";

                        if (fieldDataType == "Numeric" && (!containsHeader || rowIndex > 0)) {
                            sheet[s].t = "n";
                            if (sheet[s].w) {
                                delete sheet[s].w;
                                // sheet[s].z = '0';
                            }
                            sheet[s].v = sheet[s].v.replace(/,/g, "")



                            if (fieldDecimal !== "" && fieldDecimal != "0") {
                                decimals = "." + "0".repeat(fieldDecimal)
                            }

                            if (sheet[s].v && sheet[s].v != 0) {
                                if (fieldApplyComma) {
                                    sheet[s].z = '#,###' + decimals;
                                } else {
                                    sheet[s].z = '#' + decimals;
                                }
                            } else {
                                sheet[s].z = '0' + decimals;
                            }
                            // sheet[s].z = '0';
                        }

                        var valLen = (sheet[s].v || "").length;
                        var decimalLen = decimals.length;
                        var commaLen = Math.ceil(valLen / 3);

                        objectMaxWidth[columnIndex] = objectMaxWidth[columnIndex] >= (valLen + decimalLen + commaLen) ?
                            objectMaxWidth[columnIndex] :
                            (valLen + decimalLen + commaLen);
                    });

                    sheet["!cols"] = objectMaxWidth.map(w => {
                        return {
                            width: w
                        }
                    })

                    XLSX.writeFile(workbook, tstructName + "-" + DCName[parseInt(exportDcNo) - 1] + '.xlsx');
                }
            } catch (ex) {}
            $("#gridToExport").remove();
        },
        confirm() {
            _this.confirmObj = $.confirm({
                theme: 'modern',
                title: appGlobalVarsObject.lcm[155],
                animation: 'scale',
                closeAnimation: 'scale',
                animateFromElement: false,
                onContentReady: function () {
                    disableBackDrop('bind');
                    //to display tooltips for Confirm & Cancel buttons
                    $(".jconfirm-buttons button").each(function () {
                        var txt = $(this).text();
                        $(this).prop('title', txt.charAt(0).toUpperCase() + txt.slice(1))
                    });
                    $(".jconfirm-buttons .hotbtn").focus(); //to focus on Confirm button once dialog is opened
                },
                // closeIcon: true,
                backgroundDismiss: 'false',
                escapeKey: 'escape',
                content: appGlobalVarsObject.lcm[485],
                buttons: {
                    confirm: {
                        text: appGlobalVarsObject.lcm[279],
                        btnClass: 'btn btn-primary',
                        action: function () {
                            _this.confirmObj.close();
                            _this.generateExcel();
                        }
                    },
                    cancel: {
                        text: appGlobalVarsObject.lcm[280],
                        btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                        action: function () {
                            _this.confirmObj.close();
                            _this.generateExcel(false);
                        },
                    },
                    escape: {
                        isHidden: true,
                        action: function () {
                            disableBackDrop('destroy');
                            parent.actionsClicked = "";
                        },
                    }
                }
            })
        },
        process() {
            _this = this;

            if (typeof XLSX == "undefined") {
                ShowDimmer(true);
                loadAndCall({
                    files: {
                        css: [],
                        js: [
                            "/Js/shim.min.js",
                            "/Js/xlsx.full.min.js"
                        ]
                    },
                    callBack() {
                        ShowDimmer(false);
                        _this.confirm();
                    }
                });
            } else {
                _this.confirm();
            }
        }
    }.process();
}


function importExceltoGrid(importDcNo) {
    //var dcCaption = GetDcCaption(importDcNo);
    //displayBootstrapModalDialog("Import Data from Excel to " + dcCaption + " ", "xs", "147px", true, "./excelimportgrid.aspx?dcNo=" + importDcNo + "");

    var dcCaption = GetDcCaption(importDcNo);
    displayBootstrapModalDialog("Import Data from Excel to " + dcCaption + " ", "xs", "388px", true, "./excelimportgrid.aspx?dcNo=" + importDcNo + "", false, () => {
        $(callParentNew('middle1', 'id').contentWindow.document.getElementsByTagName('body')).css("overflow", "");
    }, () => { });

}

function openSignaturePad(signFld) {

    let signCanvas = `
    <div id="modal_${signFld}" class="signature-pad h-400px">
        <div class="signature-pad--body cursor-pointer">
            <canvas></canvas>
        </div>
        <div class="signature-pad--footer">
            <div class="description d-none">Sign Above</div>
            <div class="signature-pad--actions">
                <div class="signatureImage d-none">
                    <button type="button" class="button save" data-action="save-png" title="Download PNG">PNG</button>
                    <button type="button" class="button save" data-action="save-jpg" title="Download JPG">JPG</button>
                    <button type="button" class="button save" data-action="save-svg" title="Download SVG">SVG</button>                    
                </div>
                <div class="signatureFooterButton d-flex gap-3 ms-auto">  
                    <input class="color-picker" class="d-none" value="black" />              
                    <button type="button" class="btn btn-icon btn-light-primary" data-action="change-color" title="Change Color">
                        <span class="material-icons material-icons-style">palette</span>
                    </button>                    
                    <button type="button" class="btn btn-icon btn-light-primary clear" data-action="clear" title="Clear">
                    <span class="material-icons material-icons-style">clear_all</span>
                    </button>
                    <button type="button" class="btn btn-icon btn-light-primary" data-action="undo" title="Undo">
                        <span class="material-icons material-icons-style">undo</span>
                    </button>
                    <button type="button" class="btn btn-icon btn-light-primary save" data-action="save-sign" title="Save Signature">
                        <span class="material-icons material-icons-style">done</span>
                    </button>
                    <button type="button" class="btn btn-icon btn-light-primary" data-action="close-sign" title="Close">
                        <span class="material-icons material-icons-style">close</span>
                    </button>                    
                </div>
            </div>
        </div>
    </div>`;

    // var modalHeight = isMobile ? "calc(100vh - 138px)" : "350px";

    loadAndCall({
        files: {
            css: ["/ThirdParty/signature_pad-master/css/signature-pad.css",
                "/ThirdParty/seballot-spectrum/spectrum.css"
            ],
            js: ["/ThirdParty/signature_pad-master/js/signature_pad.umd.js",
                "/Js/signature.js",
                "/ThirdParty/seballot-spectrum/spectrum.js"
            ]
        },
        callBack() {

            let myModal = new BSModal(`sp_${signFld}`, "Signature Pad", signCanvas, () => {
                signatuePadCallBack(signFld);
            }, () => {
            });
            myModal.changeSize("lg");
            myModal.scrollableDialog();
            myModal.verticallyCentered();
            myModal.hideHeader();
            myModal.hideFooter();
        }
    });

}

function openBarQrScanner(scanEle) {
    ShowDimmer(true);
    let scanReaderId = "reader_" + scanEle;
    let scannerTemplate = `<div class="container">
        <div class="row">
            <div class="col-md-12 text-center">
                <div id="${scanReaderId}" class="d-inline-block position-releative w-100 h-100">
                    <input type="file" id="qr-input-file" class="form-control d-none" accept="image/*" capture>
                </div>
            </div>
        </div>
    </div>`;

    loadAndCall({
        files: {
            css: ["/ThirdParty/html5-qrcode-master/dist/highlight.min.css"],
            js: ["/ThirdParty/html5-qrcode-master/dist/highlight.min.js",
                "/ThirdParty/html5-qrcode-master/dist/html5-qrcode.min.js"
            ]
        },
        callBack() {
            let myModal = new BSModal(`modal_${scanEle}`, "Scanner", scannerTemplate, () => {
                callBackBarQrScanner(scanReaderId);
            }, () => {
                $("#html5-qrcode-button-camera-stop").trigger("click");
            });
            myModal.changeSize("lg");
            myModal.scrollableDialog();
            myModal.verticallyCentered();
            myModal.hideHeader();
            myModal.hideFooter();
            myModal.showFloatingClose();
        }
    });
}

function callBackBarQrScanner(scanReaderId) {

    docReady(function () {
        hljs.initHighlightingOnLoad();

        var html5QrcodeScanner = new Html5QrcodeScanner(
            scanReaderId, {
                fps: 10,
                qrbox: 250,
                aspectRatio: 1.777778,
                experimentalFeatures: {
                    useBarCodeDetectorIfSupported: true
                },
                rememberLastUsedCamera: true
            }, false
        );

        html5QrcodeScanner.render(onScanSuccess);
        ShowDimmer(false);            

        function onScanSuccess(decodedText, decodedResult) {
            if (decodedText != "") {
                let eleId = html5QrcodeScanner.elementId.substring(7);
                var scanFldName = GetFieldsName(eleId);
                var isGridDc = IsGridField(scanFldName);
                if (isGridDc == false) {
                    SetFieldValue(eleId, decodedText);
                    UpdateFieldArray(eleId, GetFieldsRowNo(eleId), decodedText, "parent", "");

                    html5QrcodeScanner.clear();
                    html5QrcodeScanner.render("stop");
                    callParentNew(`modal_${eleId}`, "id").dispatchEvent(new CustomEvent("close"));
                    MainBlur($("#" + eleId));
                } else {
                    var isBrExist = false;
                    var scanRow = 0; // GetFieldsRowNo(eleId);
                    var scanfDcNo = GetFieldsDcNo(eleId);
                    $("#gridHd" + scanfDcNo + " tbody tr").each(function (index, el) {
                        if (!isMobile && eleId != $j(this).find('textarea[id^=' + scanFldName + ']').attr("id") && decodedText == $j(this).find('textarea[id^=' + scanFldName + ']').val()) {
                            scanRow = GetFieldsRowNo($j(this).find('textarea[id^=' + scanFldName + ']').attr("id"));
                            //alert("Same value exist in row:"+scanRow+", do you want edit or continue?");
                            isBrExist = true;
                            return false;
                        } else if (isMobile && eleId != $j(this).find('textarea[id^=gr' + scanFldName + ']').attr("id") && decodedText == $j(this).find('textarea[id^=gr' + scanFldName + ']').val()) {
                            scanRow = GetFieldsRowNo($j(this).find('textarea[id^=gr' + scanFldName + ']').attr("id"));
                            //alert("Same value exist in row:"+scanRow+", do you want edit or continue?");
                            isBrExist = true;
                            return false;
                        }
                    });
                    if (isBrExist) {
                        html5QrcodeScanner.clear();
                        html5QrcodeScanner.render("stop");
                        callParentNew(`modal_${eleId}`, "id").dispatchEvent(new CustomEvent("close"));
                        $.confirm({
                            theme: 'modern',
                            closeIcon: false,
                            title: eval(callParent('lcm[155]')),
                            content: "Same scanned value exists in row:" + parseInt(scanRow) + ", do you want edit?",
                            escapeKey: 'buttonB',
                            onContentReady: function () {
                                disableBackDrop('bind');
                            },
                            buttons: {
                                buttonA: {
                                    text: eval(callParent('lcm[279]')),
                                    btnClass: 'btn btn-primary',
                                    action: function () {
                                        if (!isMobile) {
                                            $("#gridHd" + scanfDcNo + " tbody tr[id=sp" + scanfDcNo + "R" + scanRow + "F" + scanfDcNo + "]").find(".glyphicon-pencil").parent().click();
                                        } else {
                                            $("#" + scanFldName + scanRow + "F" + scanfDcNo).focus();
                                        }
                                    }
                                },
                                buttonB: {
                                    text: eval(callParent('lcm[280]')),
                                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                                    action: function () {
                                        SetFieldValue(eleId, decodedText);
                                        UpdateFieldArray(eleId, GetFieldsRowNo(eleId), decodedText, "parent", "");

                                        html5QrcodeScanner.clear();
                                        html5QrcodeScanner.render("stop");
                                        callParentNew(`modal_${eleId}`, "id").dispatchEvent(new CustomEvent("close"));
                                        MainBlur($("#" + eleId));
                                    },
                                }
                            }
                        });

                    } else {
                        SetFieldValue(eleId, decodedText);
                        UpdateFieldArray(eleId, GetFieldsRowNo(eleId), decodedText, "parent", "");

                        html5QrcodeScanner.clear();
                        html5QrcodeScanner.render("stop");
                        callParentNew(`modal_${eleId}`, "id").dispatchEvent(new CustomEvent("close"));
                        MainBlur($("#" + eleId));
                    }
                }
            } else {
                showAlertDialog("error", "Error occurred while scanning!..");
                callParentNew(`modal_${eleId}`, "id").dispatchEvent(new CustomEvent("close"));
            }
        }
    });

    function docReady(fn) {
        if (document.readyState === "complete" || document.readyState === "interactive") {
            setTimeout(fn, 1);
        } else {
            document.addEventListener("DOMContentLoaded", fn);
        }
    }
}

var isViewColUpdate=false;
function SearchViewColumns() {
    try {
        $.ajax({
            url: 'tstruct.aspx/GetSavedViewColumns',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                TransId: transid
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                if (data.d != "" && !data.d.startsWith('error:')) {
                    isViewColUpdate = true;
                    let vcValues = data.d;
                    ViewColumnsPopup(vcValues);
                } else if (data.d.startsWith('error:')) {
                    isViewColUpdate = false;
                    ViewColumnsPopup("");
                    showAlertDialog("error", data.d.replace(/^error:/, ''));
                }
                else {
                    isViewColUpdate = false;
                    ViewColumnsPopup("");
                }
            },
            error: function (error) {
                isViewColUpdate = false;
            }
        });
    } catch (exp) { }
}

function ViewColumnsPopup(vcValues)
{
    let lstValue=[];
    if(vcValues!="")
        lstValue=vcValues.replace(/\r\n/g,"~").split("~");
    var tblData = "<table id=\"SVCTable\" class=\"table table-bordered table-sm\">";
    tblData += "<thead>";
    tblData += "<tr><th><input type=\"checkbox\" name=\"chkHdrItem\" class=\"form-check-input fgHdrChk\" onclick=\"javascript:ChecksvcAll(this);\"></div></th><th class=\"d-none\">Fields</th><th>Fields</th>";
    tblData += "</thead>";
    tblData += "<tbody>";
    $.each(FNames,function(ind,val){
        let ftype=FFieldType[ind];
        let fhidden=FFieldHidden[ind];
        let excludeTypes=['Rich Text','Large Text','Image','Table'];
        if (ftype != "" && excludeTypes.indexOf(ftype) == -1 && fhidden.toLowerCase() == "false" && FCaption[ind] != "" && FCaption[ind] != "&nbsp")
        {
          if(lstValue.indexOf(val)==-1)
              tblData += "<tr><td><input type=\"checkbox\" name=\"chkItem\" class=\"form-check-input fgChk\" onclick=\"javascript:ChkHdrsvcCheckbox(this);\"></td>";
          else
              tblData += "<tr><td><input type=\"checkbox\" name=\"chkItem\" class=\"form-check-input fgChk\" onclick=\"javascript:ChkHdrsvcCheckbox(this);\" checked></td>";
          tblData += "<td class=\"d-none\">"+val+"</td>";
            tblData += "<td>" + FCaption[ind] + "</td></tr>";
        }
    });
    tblData += "</tbody>";
    tblData += "</table>";

    let myModal = new BSModal("SelectViewColumns", "Select View Columns", tblData, () => {
    }, () => {
    });
    myModal.scrollableDialog();
    myModal.okBtn.innerText = "Ok";
    myModal.okBtn.setAttribute("onClick", "SaveViewColumns()");
}

function ChecksvcAll(obj) {
    $j("input[name=chkItem]:checkbox").each(function () {
        $j(this).prop("checked", obj.checked);
    });
}
function ChkHdrsvcCheckbox(obj, exprResult) {
    if ($j(".fgChk:visible").length == $j(".fgChk:checked").length)
        $j(".fgHdrChk").prop("checked", true);
    else
        $j(".fgHdrChk").prop("checked", false);
}

function SaveViewColumns() {
    let selectedFlds = "";
    $j(".fgChk:checked").each(function (ind, val) {
        selectedFlds += $(this).parents("tr").find("td:eq(1)").text() + "~100~";
    });
    if (selectedFlds != "") {
        selectedFlds = "189,388,350,590,1~~~" + selectedFlds;
        selectedFlds = selectedFlds.slice(0, -1);
        try {
            $.ajax({
                url: 'tstruct.aspx/SaveViewColumns',
                type: 'POST',
                cache: false,
                async: true,
                data: JSON.stringify({
                    TransId: transid,
                    selectedFlds: selectedFlds,
                    isViewColUpdate: isViewColUpdate
                }),
                dataType: 'json',
                contentType: "application/json",
                success: function (data) {
                    if (data.d != "" && data.d.startsWith("error:")) {
                        showAlertDialog("error", data.d.replace(/^error:/, ''));
                    }
                    else if (data.d == "" || data.d != "success")
                        showAlertDialog("warning", 1031, "client");
                },
                error: function (error) {
                    showAlertDialog("warning", 1031, "client");
                }
            });
        } catch (exp) { }
    }
}


function AxRulesDefParser(thisEleName,thisEleType,thisEvent,thisExpDep="")
{
    var flname="";
    if(FNames.indexOf(thisEleName)>-1)
        flname=thisEleName;
    else
        flname=GetFieldsName(thisEleName);
    if(flname=="")
        flname = thisEleName;
    var loopInd;
    if (flname != "") {
        loopInd = AxRDCompName.reduce(function (a, e, i) {
            if (e === flname || e.indexOf(flname + ",") > -1 || e.indexOf("," + flname) > -1) {
                let flg = false;
                $.each(e.split(","), function (index, el) {
                    if (el === flname) {
                        flg = true;
                        return false;
                    }
                })
                if (flg)
                    a.push(i);
            }
            return a;
        }, []);
    }

    if (loopInd.length > 0 || thisEleName == 'validate onsave' || thisEleName == 'validate ondelete' || thisEleName == 'validate oncancel' || thisEleName == 'compute script onsave' || thisEleName == 'compute script ondelete' || thisEleName == 'compute script oncancel' || thisEleName == 'allowduplicate' || thisEleName == "formcontrol" || thisEleName == "compute script ondataload" || thisEleName == "compute script onformload") {
        switch (thisEvent) {
            case 'validate':
                var loopInd = AxRDCompName.reduce(function (a, e, i) {
                    if (e === flname)
                        a.push(i);
                    return a;
                }, []);
                if (loopInd.length == 0 && (thisEleName == 'validate onsave' || thisEleName == 'validate ondelete' || thisEleName == 'validate oncancel')) {
                    loopInd = AxRDRuleType.reduce(function (a, e, i) {
                        if (e.toLowerCase() === thisEleName)
                            a.push(i);
                        return a;
                    }, []);
                }
                var thisRes = true;
                $.each(loopInd, function (ind, thisInd) {
                    var flval = "";
                    if (flname != "")
                        flval = GetFieldValue(thisEleName);
                    let vexpr = AxRDValidation[thisInd];
                    if (vexpr != "" && AxRuleIsApplicable(flname, thisInd)) {
                        if ((AxRDRuleType[thisInd].toLowerCase() == 'validate onsave' || AxRDRuleType[thisInd].toLowerCase() == 'validate ondelete' || AxRDRuleType[thisInd].toLowerCase() == 'validate oncancel') && thisEleName != 'validate onsave' && thisEleName != 'validate ondelete' && thisEleName != 'validate oncancel')
                            return;
                        AxRulesFldsArray('validate', thisEleName, vexpr, AxRDCompName[thisInd]);
                        var fResult = Evaluate(flname, flval, vexpr, "vexpr");
                        if (fResult != 'T' && fResult != 't' && fResult != true) {
                            var cutMsg = eval(callParent('lcm[52]'));
                            var firstChar = fResult.substring(0, 1);
                            var alertMsg = fResult.substring(1);
                            if (firstChar == "_") {
                                showAlertDialog("error", alertMsg);
                            } else if (firstChar == "?") {
                                showAlertDialog("error", alertMsg);
                                if (flname != "" && flname != "validate onsave" && flname != "validate ondelete" && flname != "validate oncancel") {
                                    fldVal = GetFieldValueFromArray(thisEleName);
                                    SetFieldValue(thisEleName, fldVal);
                                    UpdateFieldArray(thisEleName, dRowNo, fldVal, "parent", "");
                                }
                                thisRes = false;
                            } else if (firstChar == "*") {
                                showAlertDialog("error", alertMsg);
                                thisRes = false;
                            } else {
                                if (fResult == "MessageSetAxFont") {
                                    thisRes = true;
                                } else if (confirm(fResult + ". " + cutMsg)) {
                                    if (flname != "" && flname != "validate onsave" && flname != "validate ondelete" && flname != "validate oncancel") {
                                        SetFieldValue(thisEleName, FldOldValue);
                                        var dRowNo = GetDbRowNo(GetFieldsRowNo(thisEleName), GetFieldsDcNo(thisEleName));
                                        UpdateFieldArray(thisEleName, dRowNo, FldOldValue, "parent", "");
                                    }
                                    thisRes = false;
                                } else {
                                    thisRes = true;
                                }
                            }
                        } else
                            thisRes = true;
                    }
                    else
                        thisRes = true;
                    if (!thisRes)
                        return false;
                });
                if (thisRes)
                    return true;
                else
                    return false;
                break;
            case 'filter':
                var loopInd = AxRDCompName.reduce(function (a, e, i) {
                    if (e === flname)
                        a.push(i);
                    return a;
                }, []);
                var axrFulFilter = "";
                $.each(loopInd, function (ind, thisInd) {
                    let axrFilter = AxRDFilter[thisInd];
                    if (axrFilter != "" && AxRuleIsApplicable(flname, thisInd))
                        axrFulFilter += axrFilter + "♠";
                });
                if (axrFulFilter != "") {
                    axrFulFilter = axrFulFilter.slice(0, -1);
                    return axrFulFilter;
                }
                else
                    return "";
                break;
            case 'formcontrol':
                var axrfc = true;
                flname = "";
                $.each(AxRDFormControl, function (ind, thisInd) {
                    if (thisInd != "" && AxRuleIsApplicable(flname, ind)) {
                        EvaluateScriptFormControl(thisInd, flname);
                        axrfc = false;
                    }
                });
                if (axrfc)
                    return true;
                else
                    return false;
                break;
            case 'computescript':
                var loopInd = AxRDCompName.reduce(function (a, e, i) {
                    if (e === flname || e.indexOf(flname + ",") > -1 || e.indexOf("," + flname) > -1) {
                        let flg = false;
                        $.each(e.split(","), function (index, el) {
                            if (el === flname) {
                                flg = true;
                                return false;
                            }
                        })
                        if (flg)
                            a.push(i);
                    }
                    return a;
                }, []);
                if (loopInd.length == 0 && (thisEleName == 'compute script onsave' || thisEleName == 'compute script ondelete' || thisEleName == 'compute script oncancel' || thisEleName == 'compute script ondataload' || thisEleName == 'compute script onformload')) {
                    loopInd = AxRDRuleType.reduce(function (a, e, i) {
                        if (e.toLowerCase() === thisEleName)
                            a.push(i);
                        return a;
                    }, []);
                }
                var axrsc = true;
                var axisBound = true;
                $.each(loopInd, function (ind, thisInd) {
                    let axrExp = AxRDComputeScript[thisInd];
                    if ((AxRDRuleType[thisInd].toLowerCase() == 'compute script onsave' || AxRDRuleType[thisInd].toLowerCase() == 'compute script ondelete' || AxRDRuleType[thisInd].toLowerCase() == 'compute script oncancel') && thisEleName != 'compute script onsave' && thisEleName != 'compute script ondelete' && thisEleName != 'compute script oncancel')
                        return;
                    if ((AxRDRuleType[thisInd].toLowerCase() == 'compute script ondataload' || AxRDRuleType[thisInd].toLowerCase() == 'compute script onformload') && (thisEleName == 'compute script ondataload' || thisEleName == 'compute script onformload'))
                        axisBound = true;
                    else {
                        let axfldList = AxRDCompName[thisInd];
                        axfldList = axfldList.split(',');
                        $.each(axfldList, function (inz, fld) {
                            let thisDcNo = GetDcNo(fld);
                            if (IsGridField(fld)) {
                                let thisEleRowNo = thisEleName.substring(thisEleName.lastIndexOf("F"), thisEleName.lastIndexOf("F") - 3);
                                let fldobj = fld + thisEleRowNo + "F" + thisDcNo;
                                let isValBound = CheckEmptyValuePerf(fldobj);
                                if (!isValBound) {
                                    axisBound = false;
                                    return;
                                }
                            } else {
                                let fldobj = fld + "000F" + thisDcNo;
                                let isValBound = CheckEmptyValuePerf(fldobj);
                                if (!isValBound) {
                                    axisBound = false;
                                    return;
                                }
                            }
                        });
                    }
                    if (axrExp != "" && AxRuleIsApplicable(flname, thisInd) && axisBound) {
                        var arrsfcExp = axrExp.split("♥");
                        //var strefVal = EvalExprSet(arrsfcExp);
                        //if (strefVal != "" && strefVal.split("~").length >= 2)
                        //    ProcessScriptFormControl(strefVal.split("~")[1], strefVal.split("~")[0], flname);

                        AxFormControlList = new Array();
                        var strefVal = EvalExprSet(arrsfcExp);
                        if (AxFormControlList.length > 0)
                            ProcessScriptFormControlOnList(flname);
                        else if (strefVal != "" && strefVal.split("~").length >= 2)
                            ProcessScriptFormControl(strefVal.split("~")[1], strefVal.split("~")[0], flname);

                        axrsc = false;
                    }
                });
                if (axrsc)
                    return true;
                else
                    return false;
                break;
            case 'allowduplicate':
                $.each(AxRDAllowDuplicate, function (ind, thisInd) {
                    if (thisInd != "") {
                        let axfldList = AxRDCompName[ind];
                        axfldList = axfldList.split(',');
                        $.each(axfldList, function (inz, fld) {
                            if (AxRuleIsApplicable(fld, ind))
                                AxRulesFldsArray('allowduplicate', fld, thisInd);
                        });
                    }
                });
                return true;
                break;
            case 'allowempty':
                var loopInd = AxRDCompName.reduce(function (a, e, i) {
                    if (e === flname || e.indexOf(flname + ",") > -1 || e.indexOf("," + flname) > -1) {
                        let flg = false;
                        $.each(e.split(","), function (index, el) {
                            if (el === flname) {
                                flg = true;
                                return false;
                            }
                        })
                        if (flg)
                            a.push(i);
                    }
                    return a;
                }, []);
                var axraw = 1;
                $.each(loopInd, function (ind, thisInd) {
                    let axralEmpty = AxRDAllowEmpty[thisInd];
                    if (axralEmpty != "")
                        AxRulesFldsArray('allowempty', thisEleName, axralEmpty);
                    if (axralEmpty != "" && axralEmpty.toLowerCase() == "f" && AxRuleIsApplicable(flname, thisInd)) {
                        let idx = FNames.indexOf(flname);
                        let thisDcNo = GetDcNo(flname);
                        var fldType = FDataType[idx];
                        if (IsGridField(flname)) {

                            var rCount = 0;
                            rCount = GetDcRowCount(thisDcNo);

                            if (!axInlineGridEdit && rCount == 1 && $("#gridHd" + thisDcNo + " tbody tr").length == 0) {
                                var dcCaption = GetDcCaption(thisDcNo);
                                showAlertDialog("warning", 2054, "client", dcCaption);
                                axraw = 0;
                            }

                            if (gridDummyRowVal.length > 0) {
                                var isExitDummy = false;
                                gridDummyRowVal.map(function (v) {
                                    if (v.split("~")[0] == thisDcNo && v.split("~")[1] == '001')
                                        isExitDummy = true;
                                });
                                if (isExitDummy)
                                    axraw = 1;
                            }

                            for (var k = 1; k < (axInlineGridEdit ? rCount + 1 : rCount); k++) {
                                var clRowNo = GetClientRowNo(k, thisDcNo);
                                var fldValue = GetFieldValue(flname + clRowNo + "F" + thisDcNo);
                                if ((fldType == "Numeric" && parseFloat(fldValue) == 0) || fldValue == "") {
                                    showAlertDialog("warning", 2055, "client", FCaption[idx]);
                                    axraw = 0;
                                }
                                else
                                    axraw = 1;
                            }
                        } else {
                            var fldValue = GetFieldValue(flname + "000F" + thisDcNo);
                            if ((fldType == "Numeric" && parseFloat(fldValue) == 0) || fldValue == "") {
                                showAlertDialog("warning", 2055, "client", FCaption[idx]);
                                axraw = 0;
                            } else
                                axraw = 1;
                        }
                    }
                    if (!axraw)
                        return 0;
                });
                if (axraw)
                    return 1;
                else
                    return 0;
                break;
            default:
                return true;
        }
    } else
        return true;
}

function AxRuleIsApplicable(thisfldName,axrdInd)
{
    if(AxRulesDefIsAppli=="true"){
        let isapplExp=AxRDIsApplicable[axrdInd];
        if(isapplExp!="" && isapplExp!="T"){
            if(isapplExp=="true")
                return true;
            else if(isapplExp=="false")
                return false;
            else{
                let flval=GetFieldValue(thisfldName);
                let fResult = Evaluate(thisfldName, flval, isapplExp, "vexpr");
                if (fResult == 'T' || fResult == 't' || fResult == true) {
                    AxRDIsApplicable[axrdInd]="true";
                    return true;
                }else{
                    AxRDIsApplicable[axrdInd]="false";
                    return false;
                }
            }
        }else
            return true;
    }else
        return true;
}

function AxFilterDropDownResult(thisFld,result)
{
    var resultSet=result;
    try{
        let axrFilter = AxRulesDefParser(thisFld, "field", "filter");
        if (axrFilter != "" && axrFilter!=true) {    
            var axrFilterArray=axrFilter.split('♠');
            $.each(axrFilterArray, function (ind, thisFilter) { 
                if(thisFilter.toLowerCase().startsWith("not in(") || thisFilter.toLowerCase().startsWith("not in ("))
                {
                    let fliterVals = thisFilter.replace("not in(", "").replace("not in (", "").replace("Not in(", "").replace("Not in (", "");
                    fliterVals=fliterVals.substr(0,fliterVals.length-1);
                    let flVals = GetFilterVarValues(fliterVals, thisFld);
                    resultSet = resultSet.filter(x=>!flVals.includes(x.id));
                }else if(thisFilter.toLowerCase().startsWith("in(") || thisFilter.toLowerCase().startsWith("in ("))
                {
                    let fliterVals = thisFilter.replace("in(", "").replace("in (", "").replace("In(", "").replace("In (", "");
                    fliterVals=fliterVals.substr(0,fliterVals.length-1);
                    let flVals = GetFilterVarValues(fliterVals, thisFld);
                    resultSet = resultSet.filter(x=>flVals.includes(x.id));
                }                
            });
        }
        return resultSet;
    }catch(ex){
        return resultSet;
    }    
}
    
function GetFilterVarValues(filterVars,selectFld) {
    var repFilterVal=[];
    if (filterVars != "") {
        var paramsList = filterVars.split(',');
        paramsList.forEach(item => {
            item=item.trim();
            if (item.startsWith(":")) {
                let thisVarName= item.substring(1);
                var fldInd = GetFieldIndex(thisVarName);
                if (fldInd > -1) {
                    var thisDc = GetDcNo(thisVarName);
                    if (IsDcGrid(thisDc)){
                        //Get Active row number 
                        let thisRow = selectFld.substring(selectFld.lastIndexOf("F"), selectFld.lastIndexOf("F") - 3);
                        let thisFldVal = GetFieldValue(thisVarName + thisRow + "F" + thisDc);
                        repFilterVal.push(...thisFldVal.split(','));
                    }
                    else{
                        let thisFldVal = GetFieldValue(thisVarName + "000F" + thisDc);
                        repFilterVal.push(...thisFldVal.split(','));
                    }
                } else {
                    let glbParamVal = Parameters.filter(word => word.startsWith(thisVarName + "~"))[0];
                    if (typeof glbParamVal != "undefined"){
                        let pval=glbParamVal.split("~")[1];
                        if(pval!="")
                            pval=pval.split(",");
                        repFilterVal.push(...pval);
                    }
                }
            } else {
                repFilterVal.push(item);
            }
        });
    }
    return repFilterVal;
}

function AxRulesFldsArray(type,fldname,ruleVal,vonsave="") {
    if (fldname != "") {
        let fName = "";
        if (FNames.indexOf(fldname) > -1)
            fName = fldname;
        else
            fName = GetFieldsName(fldname);
        if (fName == "" && fldname == "validate onsave")
            fName = "validate_onsave";
        if (fName != "" && fName != "validate_onsave" && AxRulesFlds.length > 0) {
            let flValue = AxRulesFlds.filter(x => x.startsWith(type + "♦" + fName + "♦"))[0];
            if (typeof flValue != "undefined" && flValue != "") {
                let flInx = AxRulesFlds.indexOf(flValue);
                AxRulesFlds[flInx] = type + "♦" + fName + "♦" + ruleVal;
            } else
                AxRulesFlds.push(type + "♦" + fName + "♦" + ruleVal);
            /* } else if (fName == "validate_onsave" && AxRulesFlds.length > 0) {*/
        } else if (fName == "validate_onsave") {
            type = 'validate_onsave';
            let flValue = AxRulesFlds.filter(x => x.startsWith(type + "♦" + vonsave + "♦"))[0];
            if (typeof flValue != "undefined" && flValue != "") {
                let flInx = AxRulesFlds.indexOf(flValue);
                AxRulesFlds[flInx] = type + "♦" + vonsave + "♦" + ruleVal;
            } else
                AxRulesFlds.push(type + "♦" + vonsave + "♦" + ruleVal);
        } else if (fName != "") {
            AxRulesFlds.push(type + "♦" + fName + "♦" + ruleVal);
        }
    }
}

//Function to check either time field have 24 hours pattern or not.
function TimeFieldPattern(fldId) {
    var fname = "";
    var indx = fldId.lastIndexOf("F");
    if (fldId != "") {
        fname = fldId.substring(0, parseInt(indx, 10) - 3);
    }
    let isTimeFormat = false;
    for (var j = 0; j < PatternNames.length; j++) {
        if (fname == PatternNames[j].substring(3)) {
            if (Patterns[j] == "24H Format")
                isTimeFormat = true;
            break;
        }
    }
    if (isTimeFormat)
        return true;
    else
        return false;
}

function AxAllowEmptyCheck(_thisFldName) {
    var axraw = -1;
    if (AxAllowEmptyFlds.length > 0) {
        let _IndVal = AxAllowEmptyFlds.filter(vals => vals.startsWith(_thisFldName + "~"));
        if (_IndVal.length > 0) {
            let axralEmpty = _IndVal[0].split('~')[1];
            if (axralEmpty != "")
                AxRulesFldsArray('allowempty', _thisFldName, axralEmpty);
            if (axralEmpty != "" && axralEmpty.toLowerCase() == "f") {
                let idx = FNames.indexOf(_thisFldName);
                let thisDcNo = GetDcNo(_thisFldName);
                var fldType = FDataType[idx];
                if (IsGridField(_thisFldName)) {

                    var rCount = 0;
                    rCount = GetDcRowCount(thisDcNo);

                    if (!axInlineGridEdit && rCount == 1 && $("#gridHd" + thisDcNo + " tbody tr").length == 0) {
                        var dcCaption = GetDcCaption(thisDcNo);
                        showAlertDialog("warning", 2054, "client", dcCaption);
                        axraw = 0;
                    }

                    if (gridDummyRowVal.length > 0) {
                        var isExitDummy = false;
                        gridDummyRowVal.map(function (v) {
                            if (v.split("~")[0] == thisDcNo && v.split("~")[1] == '001')
                                isExitDummy = true;
                        });
                        if (isExitDummy)
                            axraw = 1;
                    }

                    for (var k = 1; k < (axInlineGridEdit ? rCount + 1 : rCount); k++) {
                        var clRowNo = GetClientRowNo(k, thisDcNo);
                        var fldValue = GetFieldValue(_thisFldName + clRowNo + "F" + thisDcNo);
                        if ((fldType == "Numeric" && parseFloat(fldValue) == 0) || fldValue == "") {
                            let _fldCap = FCaption[idx];
                            if (_fldCap == '&nbsp')
                                _fldCap = "";
                            if (_fldCap != '')
                                _fldCap = _fldCap + " ";
                            let _fldName = FNames[idx];
                            if (_fldName != "" && AxSetFldCaption.length > 0) {
                                let _thisEle = AxSetFldCaption.filter(x => x.indexOf('setfieldcaption~' + _fldName + '^') > -1);
                                if (_thisEle.length > 0) {
                                    _fldCap = _thisEle[0].split('^')[1];
                                    if (_fldCap != '')
                                        _fldCap = _fldCap + " ";
                                }
                            }

                            showAlertDialog("warning", 2055, "client", _fldCap);
                            axraw = 0;
                        }
                        else
                            axraw = 1;
                    }
                } else {
                    var fldValue = GetFieldValue(_thisFldName + "000F" + thisDcNo);
                    if ((fldType == "Numeric" && parseFloat(fldValue) == 0) || fldValue == "") {
                        let _fldCap = FCaption[idx];
                        if (_fldCap == '&nbsp')
                            _fldCap = "";
                        if (_fldCap != '')
                            _fldCap = _fldCap + " ";
                        let _fldName = FNames[idx];
                        if (_fldName != "" && AxSetFldCaption.length > 0) {
                            let _thisEle = AxSetFldCaption.filter(x => x.indexOf('setfieldcaption~' + _fldName + '^') > -1);
                            if (_thisEle.length > 0) {
                                _fldCap = _thisEle[0].split('^')[1];
                                if (_fldCap != '')
                                    _fldCap = _fldCap + " ";
                            }
                        }
                        showAlertDialog("warning", 2055, "client", _fldCap);
                        axraw = 0;
                    } else
                        axraw = 1;
                }
            } else if (axralEmpty != "" && axralEmpty.toLowerCase() != "f") {
                axraw = 1;
            }
        }
    }
    return axraw;
}

function AxRulesScriptsParser(thisEvent, thisEleName = "") {
    switch (thisEvent) {
        case 'formcontrol':
            var flname = "";
            $.each(AxRDFormControl, function (ind, thisScript) {
                if (thisScript != "") {
                    let _isappli = true;
                    if (AxRDIsApplicable[ind] != "" && AxRDIsApplicable[ind] != "T") {
                        let fResult = Evaluate('', '', AxRDIsApplicable[ind], "vexpr");
                        if (fResult != 'T' && fResult != 't' && fResult != true)
                            _isappli = false;
                    }
                    if (_isappli) {
                        let _isScriptExc = true;
                        let _isPegRule = AxRDPegRule[ind];
                        if (_isPegRule != "" && _isPegRule == "T") {
                            let _pegRuleLevelNo = AxRDLevelNo[ind];
                            if (AxPegLevelNo != _pegRuleLevelNo) {
                                _isScriptExc = false;
                            }
                        }
                        if (_isScriptExc) {
                            if (thisEleName != "") {
                                var axfldList = AxRDFormControlParent[ind];
                                if (axfldList != "") {
                                    axfldList = axfldList.split(',');
                                    if ($.inArray(thisEleName, axfldList) > -1) {
                                        isScriptFormLoad = "false";
                                        EvaluateScriptFormControl(thisScript, thisEleName);
                                    }
                                }
                            } else {
                                isScriptFormLoad = "true";
                                EvaluateScriptFormControl(thisScript, flname);
                            }
                        }
                    }
                }
            });
            break;
        case 'scriptonload':
            var flname = "";
            $.each(AxRDScriptOnLoad, function (ind, thisScript) {
                if (thisScript) {
                    let _isappli = true;
                    if (AxRDIsApplicable[ind] != "" && AxRDIsApplicable[ind] != "T") {
                        let fResult = Evaluate('', '', AxRDIsApplicable[ind], "vexpr");
                        if (fResult != 'T' && fResult != 't' && fResult != true)
                            _isappli = false;
                    }
                    if (_isappli) {
                        let _isScriptExc = true;
                        let _isPegRule = AxRDPegRule[ind];
                        if (_isPegRule != "" && _isPegRule == "T") {
                            let _pegRuleLevelNo = AxRDLevelNo[ind];
                            if (AxPegLevelNo != _pegRuleLevelNo) {
                                _isScriptExc = false;
                            }
                        }
                        if (_isScriptExc) {
                            var arrsfcExp = thisScript.split("♥");
                            AxFormControlList = new Array();
                            EvalExprSet(arrsfcExp);
                            isScriptFormLoad = "true";
                            if (AxFormControlList.length > 0)
                                ProcessScriptFormControlOnList(flname);
                        }
                    }
                }
            });
            break;
        default:
            return true;
    }
}

function AxRulesBtnConMsgParser(thisElement) {
    try {
        if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
            let thisEventId = $(thisElement).attr("data-id");
            if (typeof thisEventId == "undefined")
                thisEventId = $(thisElement).attr("id");
            try {
                if (typeof thisEventId == "undefined" && blurNextPreventId != "") {
                    thisEventId = typeof $("#" + blurNextPreventId).attr("data-id") != "undefined" ? $("#" + blurNextPreventId).attr("data-id") : (typeof $("#" + blurNextPreventId).attr("id") != "undefined" ? $("#" + blurNextPreventId).attr("id") : thisEventId);
                }
            } catch (ex) { }
            if (typeof thisEventId != "undefined" && thisEventId != "") {
                if (thisEventId.startsWith('actbtn_'))
                    thisEventId = thisEventId.replace('actbtn_', '');
                let _thisInd = -1;
                $.each(AxRDBtnMsgName, function (ind, thisBtnId) {
                    if (thisBtnId != "" && thisBtnId == thisEventId) {
                        _thisInd = ind;
                        return;
                    }
                });
                if (_thisInd > -1) {
                    let axRdBtnCon = AxRDBtnMsgCond[_thisInd];
                    let axRdBtnConMsg = AxRDBtnMsgConMsg[_thisInd];

                    var glType = eval(callParent('gllangType'));
                    var isRTL = false;
                    if (glType == "ar")
                        isRTL = true;
                    else
                        isRTL = false;

                    if (axRdBtnCon != "") {
                        let _fResult = Evaluate('', '', axRdBtnCon, "vexpr")
                        if ((_fResult == 'T' || _fResult == 't' || _fResult == true) && axRdBtnConMsg != "") {
                            if (confirm(axRdBtnConMsg))
                                return true;
                            else
                                return false;
                        } else
                            return true;
                    } else if (axRdBtnConMsg != "") {
                        if (confirm(axRdBtnConMsg))
                            return true;
                        else
                            return false;
                    }
                } else
                    return true;
            } else
                return true;
        } else
            return true;
    } catch (ex) {
        return true;
    }
}

//TO Get DoFormLoad data from client side with the result & tstruct HTML.
function GetCloneFormLoadData(tstQureystr) {
    try {
        GetProcessTime();
        $.ajax({
            url: 'tstruct.aspx/GetCloneFormLoadValues',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                key: tstDataId,
                tstQureystr: tstQureystr,
                isTstHtmlLs: resTstHtmlLS
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                resTstHtmlLS = "";
                var result = data.d;
                if (result != "") {
                    if (result.split("*♠♦*").length > 1) {
                        serverprocesstime = result.split("*♠♦*")[1];
                        requestProcess_logtime = result.split("*♠♦*")[2];
                        result = result.split("*♠♦*")[0];
                        WireElapsTime(serverprocesstime, requestProcess_logtime, true);
                    } else {
                        UpdateExceptionMessageInET("Error : " + result);
                    }
                }
                Closediv();
                if (result.toLowerCase().indexOf("access violation") === -1) {
                    ArrActionLog = "";
                    if (CheckSessionTimeout(result))
                        return;
                    if (result != "") {
                        actionCallbackFlag = actionCallFlag;
                        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                            "pointer-events": "auto"
                        });
                        if (FromSave) {
                            FromSave = false;
                        }
                        EnableSaveBtn(true);
                        isReadyCK = false
                        DCFrameNo.forEach(function (dcID) {
                            ClearFieldsInDC(dcID);
                            try {
                                let _isGrid = IsDcGrid(dcID);
                                if (_isGrid)
                                    ClearRowsInGrid(dcID, "htmlload");
                            } catch (ex) { }
                        });
                        ExprPosArray.forEach(function (vals, ind) {
                            if (vals != "")
                                ExprPosArray[ind] = "";
                        });
                        tstReadOnlyPeg = false;
                        navValidator = true;
                        SetFormDirty(false);
                        appstatus = "";
                        blurNextPreventId = "";
                        AxFormControlList = new Array();
                        AxSetFldCaption = new Array();
                        RegVarFldList = new Array();
                        ChangedFields = new Array();
                        ChangedFieldDbRowNo = new Array();
                        ChangedFieldValues = new Array();
                        DeletedDCRows = new Array();
                        ChangedDcRows = new Array();
                        ChangedDcs = new Array();
                        AllFieldNames = new Array();
                        AllFieldValues = new Array();
                        ScriptMaskFields = new Array();
                        AxExecFormControl = false;
                        DisabledDcs = new Array();
                        AxFormContHiddenFlds = new Array();
                        AxFormContSetCapFlds = new Array();
                        AxFormContSetCapFldsGrid = new Array();
                        AxFormContSetFldActGrid = new Array();
                        AxFormContSetGridCell = new Array();
                        AxFormContFldSetFocus = new Array();
                        multiSelectflds = new Array();
                        multiSelFldParents = new Array();
                        multiSelFldResult = new Array();
                        multiSelectLoadVals = new Array();
                        AxRulesFlds = new Array();
                        FldListParents = new Array();
                        FldListData = new Array();
                        FldDListPFNames = new Array();
                        FldDListPFNameVals = new Array();
                        changeFillGridDc = 0;
                        imgNames = new Array();
                        imgSrc = new Array();

                        if (isMobile)
                            OnMobileNewTst();

                        document.title = "Tstruct";
                        recordid = "0";
                        $j("#recordid000F0").val("0");
                        var resval = result.split("*$*");
                        if (resval[0] == "") {
                            window.location.reload();
                            return;
                        }
                        $j("#PEGDiv").addClass("d-none");
                        $j("#AxAmendDiv").addClass("d-none");
                        try {
                            $(".tstructMainBottomFooter").removeClass("d-none");
                            $("#btnAppsHeader").removeClass('disabled');
                            $("#btnAppsHeader").attr('disabled', false);
                            $(".BottomToolbarBar").find('a').removeClass('disabled');
                            $(".BottomToolbarBar").find('a').attr('disabled', false);
                        } catch (ex) { }
                        $j(".workflow").remove();
                        $(".wfselectbox").html("<div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"bottom-end\" id=\"selectbox\"></div>");
                        $j(".wrkflwinline").remove();
                        $j(".workflowMsg").remove();
                        $j(".downArr").remove();
                        $j(".Selectboxlist").remove();
                        $j("#workflowoverlay").addClass("d-none");
                        $j("#dvMessage").html("");
                        $j("#dvMessage").removeClass("AXinfo").addClass("success d-none");
                        $j("#file").addClass("d-none");
                        $j("#attachment-overlay").html("");
                        $j("#attachfname").val("");
                        attachments = "";
                        filenamearray = new Array();
                        fileonloadarray = new Array();
                        $("#hdnTabHtml").val(resval[3]);
                        var tstPerfVars = resval[4];
                        if (tstPerfVars != "") {
                            wsPerfFormLoadCall = tstPerfVars.split(";")[0] == "true" ? true : false;
                            wsPerfEvalExpClient = tstPerfVars.split(";")[1].split(",");
                            headerAttachDir = tstPerfVars.split(";")[2];
                            try {
                                if (tstPerfVars.split(";").length > 3) {
                                    formParamFlds = tstPerfVars.split(";")[3].split(",");
                                    formParamVals = tstPerfVars.split(";")[4].split(",");
                                } else {
                                    formParamFlds = new Array();
                                    formParamVals = new Array();
                                }
                            } catch (ex) { }
                        }
                        var ImgVals = resval[5];
                        if (ImgVals != "") {
                            ImgVals = ImgVals.split("♠");
                            for (i = 0; i < ImgVals.length; i++) {
                                imgNames[i] = ImgVals[i].split("♦")[0];
                                imgSrc[i] = ImgVals[i].split("♦")[1];
                                if (imgSrc[i] != "")
                                    imgSrc[i] = imgSrc[i].replace("%20", " ");
                            }
                        }
                        if (resval[6] != "") {
                            var dcStatus = resval[6].split(',');
                            dcStatus.forEach(function (dcStat, indx) {
                                TabDCStatus[indx] = dcStat;
                            })
                        }
                        tstructCancelled = resval[7];
                        if (typeof AxGridAttNotExistList != "undefined" && resval[8] != "") {
                            var agattnotfiles = resval[8].split(',');
                            AxGridAttNotExistList = agattnotfiles;
                        } else if (resval[8] != "") {
                            var agattnotfiles = resval[8].split(',');
                            AxGridAttNotExistList = agattnotfiles;
                        } else
                            AxGridAttNotExistList = "";
                        tstDataId = resval[11];

                        try {
                            DCHasDataRows.forEach(function (_val, _ind) {
                                if (_val == "True")
                                    DCHasDataRows[_ind] = 'False';
                            })

                            FFieldHidden = new Array();
                            FFieldReadOnly = new Array();

                            if (resval[9] != "") {
                                var _thisFFH = resval[9].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldHidden.push(_val);
                                })
                            }
                            if (resval[10] != "") {
                                var _thisFFH = resval[10].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldReadOnly.push(_val);
                                })
                            }
                        } catch (ex) { }
                        displayAutoGenVal = "false";
                        $j("#hdnShowAutoGenFldValue").val(displayAutoGenVal);
                        $j("#hdnDataObjId").val(tstDataId);
                        LoadResult = result;
                        Closediv();
                        isLoadDataCall = false;
                        ReloadJqueryReference();
                        isTstPostBackVal = resval[0] + "*$*" + resval[1] + "*$*" + resval[2];
                    }
                } else {
                    blurNextPreventId = "";
                    actionCallbackFlag = actionCallFlag;
                    $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                        "pointer-events": "auto"
                    });
                    if (FromSave) {
                        FromSave = false;
                    }
                    EnableSaveBtn(true);
                    AxWaitCursor(false);
                    ShowDimmer(false);
                    SetFormDirty(false);
                    $("#reloaddiv").show();
                    $("#dvlayout").hide();
                }
            },
            error: function (error) {
                Closediv();
                blurNextPreventId = "";
                actionCallbackFlag = actionCallFlag;
                $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                    "pointer-events": "auto"
                });
                if (FromSave) {
                    FromSave = false;
                }
                EnableSaveBtn(true);
                AxWaitCursor(false);
                ShowDimmer(false);
                SetFormDirty(false);
                $("#reloaddiv").show();
                $("#dvlayout").hide();
            }
        });
    } catch (exp) {
        Closediv();
        blurNextPreventId = "";
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
            "pointer-events": "auto"
        });
        if (FromSave) {
            FromSave = false;
        }
        EnableSaveBtn(true);
        AxWaitCursor(false);
        ShowDimmer(false);
        SetFormDirty(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
}

function SaveTransactionJSON() {
    if (!ValidateBeforeSubmit()) {
        ShowDimmer(false);
        AxWaitCursor(false);
        return;
    }
    let fldJSON = "\"recdata\":[";
    let changedDcs = "";
    DCName.forEach(function (ele) {
        let _thisDcNo = ele.substring(2);
        let _thisDcFld = GetGridFields(_thisDcNo);
        if (!IsDcGrid(_thisDcNo)) {
            let _RecVal = $("#axp_recid" + _thisDcNo + "000F" + _thisDcNo).val();
            fldJSON += "{\"axp_recid" + _thisDcNo + "\":[{\"rowno\":\"001\",\"text\":\"" + _RecVal + "\",\"columns\":{";
            let fldValJson = "";
            _thisDcFld.forEach(function (flele) {
                if (flele.startsWith('axp_recid'))
                    return;
                let _thisFld = flele + "000F" + _thisDcNo;
                let _thisVal = GetFieldValue(_thisFld);
                _thisVal = _thisVal.replace(/\\/g, '\\\\');
                fldValJson += "\"" + flele + "\":\"" + _thisVal + "\",";
            });
            if (fldValJson != "")
                fldJSON += fldValJson.substr(0, fldValJson.length - 1);
            fldJSON += "}}]},";
        } else {
            var isExitDummy = false;
            if (gridDummyRowVal.length > 0) {
                gridDummyRowVal.map(function (v) {
                    if (v.split("~")[0] == _thisDcNo) isExitDummy = true;
                });
            }
            if (!isExitDummy) {
                changedDcs += "\"dc" + _thisDcNo + "\":\"*\",";
                fldJSON += "{\"axp_recid" + _thisDcNo + "\":[";
                let _gridRows = false;
                $("#gridHd" + _thisDcNo + " tbody tr").each(function (rind, rele) {
                    _gridRows = true;
                    let _thisRNo = $(rele).attr("id");
                    _thisRNo = _thisRNo.substring(_thisRNo.indexOf('F'), _thisRNo.indexOf('F') - 3);
                    let _thisDbrow = GetDbRowNo(_thisRNo, _thisDcNo);
                    if (_thisDbrow.length == 1)
                        _thisDbrow = "00" + _thisDbrow;
                    else if (_thisDbrow.length == 2)
                        _thisDbrow = "0" + _thisDbrow;
                    let _RecVal = $("#axp_recid" + _thisDcNo + _thisRNo + "F" + _thisDcNo).val();
                    fldJSON += "{\"rowno\":\"" + _thisDbrow + "\",\"text\":\"" + _RecVal + "\",\"columns\":{";
                    let fldValJson = "";
                    _thisDcFld.forEach(function (flele) {
                        if (flele.startsWith('axp_recid'))
                            return;
                        let _thisFld = flele + _thisRNo + "F" + _thisDcNo;
                        let _thisVal = GetFieldValue(_thisFld);
                        _thisVal = _thisVal.replace(/\\/g, '\\\\');
                        fldValJson += "\"" + flele + "\":\"" + _thisVal + "\",";
                    });
                    if (fldValJson != "")
                        fldJSON += fldValJson.substr(0, fldValJson.length - 1);
                    fldJSON += "}},";
                });
                if (_gridRows == true)
                    fldJSON = fldJSON.substr(0, fldJSON.length - 1);
                fldJSON += "]},";
            }
        }
    });
    fldJSON = fldJSON.substr(0, fldJSON.length - 1);
    fldJSON += "]";

    //alert(fldJSON);

    //var finalJson = "{\"_parameters\":[{\"savedata\":{\"axpapp\":\"" + proj + "\",\"transid\":\"" + transid + "\",\"s\":\"" + sid + "\",\"changedrows\":{\"dc2\":\"*\"},\"trace\":\"true\",\"recordid\":\"" + recordid +"\",";
    //finalJson += fldJSON;
    //finalJson += "}}]}";

    ShowDimmer(true);
    AxWaitCursor(true);
    try {
        let _hfiles = UploadFiles();
        let _delRows = GetDeletedRows();
        ASB.WebService.SaveTransactionRestAPI(fldJSON, transid, recordid, changedDcs, _delRows, _hfiles, tstDataId, DeletedFieldValue, resTstHtmlLS, SuccessCallbackRestSave, OnExceptionRestSave);
    }
    catch (exp) {
        ShowDimmer(false);
        AxWaitCursor(false);
    }

    function SuccessCallbackRestSave(result, eventArgs) {
        try {
            if (result != "" && result.split("♠*♠").length > 1) {
                tstDataId = result.split("♠*♠")[0];
                result = result.split("♠*♠")[1];
            }
            if (result.startsWith("RMQError:")) {
                ShowDialog('error', result.replace("RMQError:", ""));
                ShowDimmer(false);
                AxWaitCursor(false);
            }
            else {
                resTstHtmlLS = "";
                try {
                    var resJson = $j.parseJSON(result);
                } catch (ex) {
                    ShowDimmer(false);
                }
                ShowDimmer(false);
                AxWaitCursor(false);
                if (resJson != undefined) {
                    if (resJson.result.success == true) {
                        try {
                            callParentNew("removeSavedtranNotify(" + transid + "," + recordid + ")", "function");
                        } catch (ex) { }

                        var src = $(eval(callParent("loadPopUpPage"))).attr("src");
                        if (src != undefined && src.indexOf("AxPop=true") != -1)
                            eval(callParent('isRefreshParentOnClose') + "= true");
                        ShowDialog('success', "Data submitted to Queue successfully.");
                        eval(callParent('isSuccessAlertInPopUp') + "= true");
                        SetFormDirty(false);
                    } else {
                        ShowDialog('error', "There was an error please try again");
                    }
                } else {
                    ShowDialog('error', "There was an error please try again");
                }
            }
        } catch (ex) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }
    }
    function OnExceptionRestSave(result) {
        ShowDimmer(false);
        AxWaitCursor(false);
    }
}

function ForceFieldValidation() {
    try {
        let _isValidate = "";
        AxpForceValidation.split(',').forEach(function (eleName) {
            if (eleName != "") {
                let _fName = eleName;
                let _fldIndex = $j.inArray(_fName, FNames);
                if (_fldIndex > -1) {
                    let _dcNo = GetDcNo(_fName);
                    let _fldType = GetFieldType(_fName, _fldIndex);
                    if (!IsGridField(_fName)) {
                        let _fldObj = $("#" + _fName + "000F" + _dcNo);
                        if (!ValidateField(_fldObj, "000", _fldIndex, _fldType, _fName)) {
                            _isValidate = _fldObj;
                            return false;
                        } else {
                            let _fldValue = GetFieldValue(_fName + "000F" + _dcNo);
                            UpdateFieldArray(_fName + "000F" + _dcNo, "000", _fldValue, "parent", "");
                        }
                    } else {
                        $("#gridHd" + _dcNo + " tbody tr").each(function () {
                            let _trId = $(this).attr("id");
                            _trId = _trId.substr(_trId.lastIndexOf("F") - 3, 3);
                            let _fldObj = $("#" + _fName + _trId + "F" + _dcNo);
                            let _fieldRowNo = GetFieldsRowNo(_fName + _trId + "F" + _dcNo);
                            let _fldDbRowNo = GetDbRowNo(_fieldRowNo, _dcNo);
                            if (!ValidateField(_fldObj, _fldDbRowNo, _fldIndex, _fldType, _fName)) {
                                _isValidate = _fldObj;
                                return false;
                            } else {
                                let _fldValue = GetFieldValue(_fName + _trId + "F" + _dcNo);
                                UpdateFieldArray(_fName + _trId + "F" + _dcNo, _fldDbRowNo, _fldValue, "parent", "");
                            }
                        });
                    }
                }
            }
            if (_isValidate != "") {
                return false;
            }
        });

        if (_isValidate != "") {
            setTimeout(function () {
                let _fldId = _isValidate.attr("id");
                let _dcNo = GetFieldsDcNo(_fldId);
                if (IsDcGrid(_dcNo)) {
                    let _rowNo = _fldId.substring(_fldId.lastIndexOf("F"), _fldId.lastIndexOf("F") - 3);
                    let focusFldIndex = _isValidate.parents("td").index();
                    if ($("#gridHd" + _dcNo + " tbody tr#sp" + _dcNo + "R" + _rowNo + "F" + _dcNo).hasClass('inline-edit'))
                        _isValidate.focus();
                    else
                        $("#gridHd" + _dcNo + " tbody tr#sp" + _dcNo + "R" + _rowNo + "F" + _dcNo + " td:eq(" + focusFldIndex + ")").click();
                } else {
                    _isValidate.focus();
                }
            }, 0);
            return false;
        } else
            return true;
    } catch (ex) {
        return true;
    }
}

$j(document).off("click", ".amendmentOptions").on("click", ".amendmentOptions", function (e) {
    let _axamdJson = AxAmendmentJSON;
    if (typeof _axamdJson != "undefined" && _axamdJson != "") {
        let _displaytitle = _axamdJson[0].data.displaytitle;
        let _html = "<label class='d-flex form-label col-form-label fw-boldest'>" + _axamdJson[0].displaymsg + "</label>";
        let _htmlDel = "";
        var _delDcRNo = new Array();
        $.each(_axamdJson[0].data.amenddata, function (iNode, ele) {
            if (iNode == "axdeleterows") {

            } else {
                let _ilength = iNode.lastIndexOf('_');
                _ilength = iNode.length - _ilength;
                let _eleName = iNode.substring(iNode.length - _ilength);
                if (_eleName.indexOf('-') > -1) {
                    if (_delDcRNo.indexOf(ele.dcname + ele.row) == -1) {
                        _delDcRNo.push(ele.dcname + ele.row);
                        _htmlDel += "<label class='d-flex form-label '>Deleted row no. " + ele.row + " in " + ele.dccaption + "(" + ele.dcname + ")</label>";
                    }
                } else {
                    let _oldVal = ele.oldvalue == "" ? "Nil" : ele.oldvalue;
                    let _newVal = ele.newvalue == "" ? "Nil" : ele.newvalue
                    let _dcNo = ele.dcname;
                    _dcNo = _dcNo.substr(2);
                    if (IsDcGrid(_dcNo)) {
                        _html += "<label class='d-flex form-label'>" + ele.fieldcaption + " at row no. " + ele.row + " in " + ele.dccaption + "(" + ele.dcname + ") changed from " + _oldVal + " to " + _newVal + "</label>";
                    } else
                        _html += "<label class='d-flex form-label'>" + ele.fieldcaption + " changed from " + _oldVal + " to " + _newVal + "</label>";
                }
            }
        });
        _html += _htmlDel;
        let buttonApplyHTML = "";
        if (typeof _axamdJson[0].enableactions != "undefined" && _axamdJson[0].enableactions == "true") {

            buttonApplyHTML = `<div class="d-flex align-items-center task-more-btns gap-3">`;
            buttonApplyHTML += `<div class="task-actions-sets d-flex"><a href="javascript:void(0)" title="Approve" class="btn btn-primary d-inline-flex align-items-center btn-sm" onclick="axAmdApprove();"><span class="material-icons material-icons-style material-icons-2" style="color: #47BE7D;">check_circle</span> Approve</a>`;
            buttonApplyHTML += `<a id="btnAppMore" href="javascript:void(0)" title="Approve Reasons/Comments" data-popover="AmdApprove" class="btn btn-white btn-color-gray-700 align-items-center btn-sm more-comments p-0 show menu-dropdown" data-kt-menu-trigger="click" data-kt-menu-placement="top" data-kt-menu-flip="top"><span class="comments-icons material-icons material-icons-style material-icons-2">more_vert</span></a>`;
            buttonApplyHTML += `<div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover shadow-sm" data-kt-menu="true" style="z-index: 105; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(0px, 166px);" data-popper-placement="top"><div class="card"><div class="card-body py-5z">`;
            if (typeof _axamdJson[0].data.approvereasons != "undefined" && _axamdJson[0].data.approvereasons != "") {
                buttonApplyHTML += `<div class="row my-3">`;
                buttonApplyHTML += `<div class="approval-controls" data-tasktype="Approve">`;
                buttonApplyHTML += `<label class="form-label col-form-label">Approve reasons</label>`;
                buttonApplyHTML += `<div class="input-group">`;
                buttonApplyHTML += `<select class="form-control form-select" id="dllAppReason">`;
                _axamdJson[0].data.approvereasons.split(',').forEach(function (ele) {
                    buttonApplyHTML += `<option value=` + ele + `>` + ele + `</option>`;
                });
                buttonApplyHTML += `</select>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
            }
            if (typeof _axamdJson[0].data.approvalcomments != "undefined" && _axamdJson[0].data.approvalcomments == "T") {
                buttonApplyHTML += `<div class="row my-3 aaa">`;
                buttonApplyHTML += `<div class="approval-controls" data-tasktype="Approve">`;
                buttonApplyHTML += `<label class="form-label col-form-label required">Comments</label>`;
                buttonApplyHTML += `<div class="input-group">`;
                buttonApplyHTML += `<textarea id="txtAmdComments" title="Approve comments" name="txtAmdComments" class="tem Family memofam form-control">` + _axamdJson[0].data.defapptext + `</textarea>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
            }
            buttonApplyHTML += `<div class="text-center Approval-OK">`;
            buttonApplyHTML += `<button class="btn btn-sm btn-primary shadow-sm" onclick="return axAmdApprove();">Approve</button>`;
            buttonApplyHTML += `</div></div></div></div></div>`;

            buttonApplyHTML += `<div class="task-actions-sets d-flex"><a href="javascript:void(0)" title="Reject" class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center btn-sm" onclick="axAmdReject();"><span class="material-icons material-icons-style material-icons-2" style="color: red;">cancel</span>Reject</a>`;
            buttonApplyHTML += `<a id="btnRejectMore" href="javascript:void(0)" title="Reject Reasons/Comments" data-popover="AmdReject" class="btn btn-white btn-color-gray-700 align-items-center btn-sm more-comments p-0 show menu-dropdown" data-kt-menu-trigger="click" data-kt-menu-placement="top" data-kt-menu-flip="top"><span class="comments-icons material-icons material-icons-style material-icons-2">more_vert</span></a>`;
            buttonApplyHTML += `<div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover shadow-sm" data-kt-menu="true" style="z-index: 105; position: fixed; inset: 0px auto auto 0px; margin: 0px; transform: translate(0px, 166px);" data-popper-placement="top"><div class="card"><div class="card-body py-5z">`;
            if (typeof _axamdJson[0].data.rejectreasons != "undefined" && _axamdJson[0].data.rejectreasons != "") {
                buttonApplyHTML += `<div class="row my-3">`;
                buttonApplyHTML += `<div class="approval-controls" data-tasktype="Approve">`;
                buttonApplyHTML += `<label class="form-label col-form-label">Reject reasons</label>`;
                buttonApplyHTML += `<div class="input-group">`;
                buttonApplyHTML += `<select class="form-control form-select" id="dllRejectReason">`;
                _axamdJson[0].data.rejectreasons.split(',').forEach(function (ele) {
                    buttonApplyHTML += `<option value=` + ele + `>` + ele + `</option>`;
                });
                buttonApplyHTML += `</select>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
            }
            if (typeof _axamdJson[0].data.rejectcomments != "undefined" && _axamdJson[0].data.rejectcomments == "T") {
                buttonApplyHTML += `<div class="row my-3">`;
                buttonApplyHTML += `<div class="approval-controls" data-tasktype="Approve">`;
                buttonApplyHTML += `<label class="form-label col-form-label required">Comments</label>`;
                buttonApplyHTML += `<div class="input-group">`;
                buttonApplyHTML += `<textarea id="txtAmdRejectComments" title="Reject comments" name="txtAmdRejectComments" class="tem Family memofam form-control">` + _axamdJson[0].data.defregtext + `</textarea>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
                buttonApplyHTML += `</div>`;
            }
            buttonApplyHTML += `<div class="text-center Approval-OK">`;
            buttonApplyHTML += `<button class="btn btn-sm btn-primary shadow-sm" onclick="return axAmdReject();">Reject</button>`;
            buttonApplyHTML += `</div></div></div></div></div>`;
            buttonApplyHTML += `</div>`;

            buttonApplyHTML += `</div>`;
        } else if (typeof _axamdJson[0].withdraw != "undefined" && _axamdJson[0].withdraw == "true") {
            /*buttonApplyHTML += `<div class="task-actions-sets d-flex"><a href="javascript:void(0)" title="Withdraw" class="btn btn-primary d-inline-flex align-items-center btn-sm" onclick="axAmdReject('discard');"><span class="material-icons material-icons-style material-icons-2">undo</span>Withdraw</a></div>`;*/
            buttonApplyHTML += `<div class="task-actions-sets d-flex"><a href="javascript:void(0)" title="Withdraw" class="btn btn-primary d-inline-flex align-items-center btn-sm" onclick="axAmdWithdrw();"><span class="material-icons material-icons-style material-icons-2">undo</span>Withdraw</a></div>`;
        }

        if (typeof _axamdJson[0].recall != "undefined" && _axamdJson[0].recall == "true") {
            buttonApplyHTML += `<div class="task-actions-sets d-flex"><a href="javascript:void(0)" title="ReCall" class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center btn-sm" onclick="axAmdReCall();"><span class="material-icons material-icons-style material-icons-2">repeat</span>ReCall</a></div>`;
        }
        let myModal = new BSModal("modalIdAmendment", "Amendment - " + _displaytitle, _html, () => {
            KTMenu?.init();
        }, () => {
        });
        myModal.scrollableDialog();
        myModal.modalBody.classList.add(..."overflow-auto pt-2 pb-1 min-h-250px".split(" "));

        myModal.modalHeader.firstElementChild.classList.add(..."text-dark fw-boldest fs-2".split(" "));
        myModal.modalHeader.classList.add(..."pt-3 pb-3".split(" "));
        myModal.modalFooter.classList.add(..."pt-1 pb-1".split(" "));
        myModal.changeSize("lg");
        myModal.okBtn.classList.add('d-none');
        myModal.cancelBtn.classList.add('d-none');

        /* Section :: footerExtras object */
        var footerExtras = document.createDocumentFragment();
        myModal.footerExtras = footerExtras;

        /* Section :: footerExtras => apply filter, sort... option */
        var buttonApply = document.createElement("div");
        buttonApply.classList.add(..."d-flex gap-2".split(" "));
        myModal.modalFooter.prepend(buttonApply);
        myModal.footerExtras.buttonApply = buttonApply;
        myModal.footerExtras.buttonApply.innerHTML = buttonApplyHTML;        
    }
});

function axAmdApprove() {
    try {
        let _axamdJson = AxAmendmentJSON;
        let _keyFldInfo = "";
        if (typeof _axamdJson != "undefined" && _axamdJson != "") {
            _keyFldInfo = _axamdJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axamdJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axamdJson[0].data.processname;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskid;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskname;
            _keyFldInfo += "♦" + _axamdJson[0].data.transid;
            _keyFldInfo += "♦" + recordid;
        }
        let _amdReasons = "";
        let _amdComment = "";
        _amdComment = $("#txtAmdComments").length > 0 ? $("#txtAmdComments").val() : "";
        if (typeof _axamdJson[0].data.approvalcomments != "undefined" && _axamdJson[0].data.approvalcomments == "T" && _amdComment == "") {
            setTimeout(function () {
                document.querySelector(`a[data-popover="AmdApprove"]`).click();
                document.querySelector(`textarea[id="txtAmdComments"]`).focus();
            }, 10);
            showAlertDialog("error", "Approval comments are mandatory.");
            return;
        }
        _amdReasons = $("#dllAppReason").length > 0 ? $("#dllAppReason").val() : "";

        let confirmmsg = "";
        if (typeof AxAmendmentJSON[0].confirmmsg != "undefined" && AxAmendmentJSON[0].confirmmsg != "") {
            confirmmsg = AxAmendmentJSON[0].confirmmsg;
        }
        let _AxTstIsPEG = "false";
        if (typeof AxTstIsPEG != "undefined" && AxTstIsPEG == "true")
            _AxTstIsPEG = "true";
        if (confirmmsg != "") {
            if (confirm(confirmmsg)) {
                ShowDimmer(true);
                AxWaitCursor(true);
                try {
                    ASB.WebService.AmdApproveWS(_keyFldInfo, _amdComment, _amdReasons, "true", _AxTstIsPEG, SuccessCallbackAmdApprove, OnExceptionAmdApprove);
                }
                catch (exp) {
                    ShowDimmer(false);
                    AxWaitCursor(false);
                }

                function SuccessCallbackAmdApprove(result, eventArgs) {
                    try {
                        var resJson = $j.parseJSON(result);
                    } catch (ex) {
                        ShowDimmer(false);
                    }
                    if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                        showAlertDialog("success", resJson.result[0].msg);
                        window.location.href = window.location.href;
                    } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                        showAlertDialog("error", resJson.result[0].error.msg);
                        ShowDimmer(false);
                        AxWaitCursor(false);
                    }
                }
                function OnExceptionAmdApprove(result) {
                    ShowDimmer(false);
                    AxWaitCursor(false);
                }
                callParentNew("modalIdAmendment", "id").dispatchEvent(new CustomEvent("close"));
            } else {
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        } else {
            ShowDimmer(true);
            AxWaitCursor(true);
            try {
                ASB.WebService.AmdApproveWS(_keyFldInfo, _amdComment, _amdReasons, "true", _AxTstIsPEG, SuccessCallbackAmdApprove, OnExceptionAmdApprove);
            }
            catch (exp) {
                ShowDimmer(false);
                AxWaitCursor(false);
            }

            function SuccessCallbackAmdApprove(result, eventArgs) {
                try {
                    var resJson = $j.parseJSON(result);
                } catch (ex) {
                    ShowDimmer(false);
                }
                if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                    showAlertDialog("success", resJson.result[0].msg);
                    window.location.href = window.location.href;
                } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                    showAlertDialog("error", resJson.result[0].error.msg);
                    ShowDimmer(false);
                    AxWaitCursor(false);
                }
            }
            function OnExceptionAmdApprove(result) {
                ShowDimmer(false);
                AxWaitCursor(false);
            }
            callParentNew("modalIdAmendment", "id").dispatchEvent(new CustomEvent("close"));
        }
    } catch (ex) { }
}

function axAmdReturn() {
    let _amdComment = $("#txtAmdComments").val();
    if (_amdComment != "") {
        try {
            let _axamdJson = AxAmendmentJSON;
            let _keyFldInfo = "";
            if (typeof _axamdJson != "undefined" && _axamdJson != "") {
                _keyFldInfo = _axamdJson[0].data.keyfield;
                _keyFldInfo += "♦" + _axamdJson[0].data.keyvalue;
                _keyFldInfo += "♦" + _axamdJson[0].data.processname;
                _keyFldInfo += "♦" + _axamdJson[0].data.taskid;
                _keyFldInfo += "♦" + _axamdJson[0].data.taskname;
                _keyFldInfo += "♦" + _axamdJson[0].data.transid;
                _keyFldInfo += "♦" + recordid;
            }

            ShowDimmer(true);
            AxWaitCursor(true);
            try {
                ASB.WebService.AmdReturnWS(_keyFldInfo, _amdComment, "true", SuccessCallbackAmdReturn, OnExceptionAmdReturn);
            }
            catch (exp) {
                ShowDimmer(false);
                AxWaitCursor(false);
            }

            function SuccessCallbackAmdReturn(result, eventArgs) {
                try {
                    var resJson = $j.parseJSON(result);
                } catch (ex) {
                    ShowDimmer(false);
                }
                if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                    showAlertDialog("success", resJson.result[0].msg);
                    window.location.href = window.location.href;
                } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                    showAlertDialog("error", resJson.result[0].error.msg);
                    ShowDimmer(false);
                    AxWaitCursor(false);
                }
            }
            function OnExceptionAmdReturn(result) {
                ShowDimmer(false);
                AxWaitCursor(false);
            }

            callParentNew("modalIdAmendment", "id").dispatchEvent(new CustomEvent("close"));
        } catch (ex) { }
    }
    else {
        showAlertDialog("error", "Comments is mandatory.");
    }
}

function axAmdReject(isDiscard = "") {
    try {
        let _axamdJson = AxAmendmentJSON;
        let _keyFldInfo = "";
        if (typeof _axamdJson != "undefined" && _axamdJson != "") {
            _keyFldInfo = _axamdJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axamdJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axamdJson[0].data.processname;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskid;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskname;
            _keyFldInfo += "♦" + _axamdJson[0].data.transid;
            _keyFldInfo += "♦" + recordid;
        }

        let _amdReasons = "";
        let _amdComment = "";
        _amdComment = $("#txtAmdRejectComments").length > 0 ? $("#txtAmdRejectComments").val() : "";
        if (typeof _axamdJson[0].data.rejectcomments != "undefined" && _axamdJson[0].data.rejectcomments == "T" && _amdComment == "" && isDiscard == "") {
            setTimeout(function () {
                document.querySelector(`a[data-popover="AmdReject"]`).click();
                document.querySelector(`textarea[id="txtAmdRejectComments"]`).focus();
            }, 10);
            showAlertDialog("error", "Reject comments are mandatory.");
            return;
        }
        _amdReasons = $("#dllRejectReason").length > 0 ? $("#dllRejectReason").val() : "";

        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.AmdRejectWS(_keyFldInfo, _amdComment, _amdReasons, "true", SuccessCallbackAmdReject, OnExceptionAmdReject);
        }
        catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackAmdReject(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                showAlertDialog("success", resJson.result[0].msg);
                window.location.href = window.location.href;
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionAmdReject(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdAmendment", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}

function axAmdWithdrw() {
    try {
        let _axamdJson = AxAmendmentJSON;
        let _keyFldInfo = "";
        if (typeof _axamdJson != "undefined" && _axamdJson != "") {
            _keyFldInfo = _axamdJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axamdJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axamdJson[0].data.processname;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskid;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskname;
            _keyFldInfo += "♦" + _axamdJson[0].data.transid;
            _keyFldInfo += "♦" + recordid;
        }

        let _amdReasons = "";
        let _amdComment = "";

        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.PegWithdrawWS(_keyFldInfo, _amdComment, _amdReasons, "true", SuccessCallbackAmdWithdrw, OnExceptionAmdWithdrw);
        }
        catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackAmdWithdrw(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                showAlertDialog("success", resJson.result[0].msg);
                window.location.href = window.location.href;
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionAmdWithdrw(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdAmendment", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}

function axAmdReCall() {
    try {
        let _axamdJson = AxAmendmentJSON;
        let _keyFldInfo = "";
        if (typeof _axamdJson != "undefined" && _axamdJson != "") {
            _keyFldInfo = _axamdJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axamdJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axamdJson[0].data.processname;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskid;
            _keyFldInfo += "♦" + _axamdJson[0].data.taskname;
            _keyFldInfo += "♦" + _axamdJson[0].data.transid;
            _keyFldInfo += "♦" + recordid;
        }

        let _amdReasons = "";
        let _amdComment = "";

        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.PegReCallWS(_keyFldInfo, _amdComment, _amdReasons, "true", SuccessCallbackAmdRecall, OnExceptionAmdRecall);
        }
        catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackAmdRecall(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                showAlertDialog("success", resJson.result[0].msg);
                window.location.href = window.location.href;
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionAmdRecall(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdAmendment", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}

$j(document).off("click", ".axPegOptions").on("click", ".axPegOptions", function (e) {
    let _axPegJson = AxPegJSON;
    if (typeof _axPegJson != "undefined" && _axPegJson != "") {
        let _withdraw = _axPegJson[0].data.withdraw;
        let _enableactions = _axPegJson[0].enableactions;

        let _html = "<div><div class=\"required\"><label class=\"form-label col-form-label pb-1 fw-boldest\" style=\"cursor: pointer;\" for=\"txtPegComments\">Comments</label></div><div class=\"input-group input-group-sm\" ><textarea id=\"txtPegComments\" title=\"\" name=\"txtPegComments\" class=\"tem Family memofam form-control\"></textarea></div></div>";

        let buttonApplyHTML = "";
        if (_withdraw == "true")
            buttonApplyHTML += `<button type="button" class="btn btn-primary shadow-sm" onclick="axPegWithdrw();">Withdraw</button>`;
        if (_enableactions == "true")
            buttonApplyHTML += `<button type="button" class="btn btn-primary shadow-sm" onclick="axPegApprove();">Approve</button><button type="button" class="btn btn-light-primary btn-active-primary shadow-sm" onclick="axPegReturn();">Return</button><button type="button" class="btn btn-light-primary btn-active-primary shadow-sm" onclick="axPegReject();">Reject</button>`;

        let myModal = new BSModal("modalIdPeg", "", _html, () => {
        }, () => {
        });
        myModal.scrollableDialog();
        myModal.modalBody.classList.add('overflow-auto');
        myModal.changeSize("lg");
        myModal.okBtn.classList.add('d-none');

        /* Section :: footerExtras object */
        var footerExtras = document.createDocumentFragment();
        myModal.footerExtras = footerExtras;

        /* Section :: footerExtras => apply filter, sort... option */
        var buttonApply = document.createElement("div");
        buttonApply.classList.add(..."d-flex gap-2".split(" "));
        myModal.modalFooter.prepend(buttonApply);
        myModal.footerExtras.buttonApply = buttonApply;
        myModal.footerExtras.buttonApply.innerHTML = buttonApplyHTML;
    }
});

function axPegCheck() {
    try {
        let _axPegJson = AxPegJSON;
        let _keyFldInfo = "";
        if (typeof _axPegJson != "undefined" && _axPegJson != "") {
            _keyFldInfo = _axPegJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axPegJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axPegJson[0].data.processname;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskid;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskname;
            _keyFldInfo += "♦" + _axPegJson[0].data.transid;
            _keyFldInfo += "♦" + recordid;
        }

        let _pegReasons = "";
        let _pegComment = "";
        //_pegComment = $("#txtPegComments").length > 0 ? $("#txtPegComments").val() : "";
        //if (typeof _axPegJson[0].data.approvalcomments != "undefined" && _axPegJson[0].data.approvalcomments == "T" && _pegComment == "") {
        //    setTimeout(function () {
        //        document.querySelector(`a[data-popover="PegApprove"]`).click();
        //        document.querySelector(`textarea[id="txtPegComments"]`).focus();
        //    }, 10);
        //    showAlertDialog("error", "Approval comments are mandatory.");
        //    return;
        //}
        //_pegReasons = $("#dllAppPegReason").length > 0 ? $("#dllAppPegReason").val() : "";

        //if (typeof _axPegJson[0].data.cmsg_appcheck != "undefined" && _axPegJson[0].data.cmsg_appcheck != "") {
        //    if (!axPegConfirmation("Approve", _axPegJson[0].data.cmsg_appcheck)) {
        //        return;
        //    }
        //}
        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.PegCheckWS(_keyFldInfo, _pegComment, _pegReasons, SuccessCallbackPegCheck, OnExceptionPegCheck);
        }
        catch (exp) {
            AxDoPegApprovalSave = "";
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackPegCheck(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                showAlertDialog("success", resJson.result[0].msg);
                window.location.href = window.location.href;
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionPegCheck(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdPeg", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}


function axPegApprove(isCommentApprove = "") {
    try {
        if (isCommentApprove == "")
            isPegApproveConfirm = "axapprove";
        else
            isPegApproveConfirm = "";
        let _axPegJson = AxPegJSON;
        let _keyFldInfo = "";
        if (typeof _axPegJson != "undefined" && _axPegJson != "") {
            _keyFldInfo = _axPegJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axPegJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axPegJson[0].data.processname;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskid;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskname;
            _keyFldInfo += "♦" + _axPegJson[0].data.transid;
            _keyFldInfo += "♦" + recordid;
        }

        if (typeof _axPegJson[0].data.cmsg_appcheck != "undefined" && _axPegJson[0].data.cmsg_appcheck != "" && isCommentApprove == "") {
            if (!axPegConfirmation("Approve", _axPegJson[0].data.cmsg_appcheck)) {
                return;
            }
        }

        let _pegReasons = "";
        let _pegComment = "";
        _pegComment = $("#txtPegComments").length > 0 ? $("#txtPegComments").val() : "";
        if (typeof _axPegJson[0].data.approvalcomments != "undefined" && _axPegJson[0].data.approvalcomments == "T" && _pegComment == "") {
            setTimeout(function () {
                document.querySelector(`a[data-popover="PegApprove"]`).click();
                document.querySelector(`textarea[id="txtPegComments"]`).focus();
                setTimeout(function () {
                    $("#btnAppPegMore").next('.Active_list_popover').removeAttr('style');
                    $("#btnAppPegMore").next('.Active_list_popover').css({ "z-index": "105", "position": "absolute", "inset": "30px 10px auto auto" });
                }, 0);
            }, 10);
            showAlertDialog("error", "Approval comments are mandatory.");
            return;
        }
        _pegReasons = $("#dllAppPegReason").length > 0 ? $("#dllAppPegReason").val() : "";

        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            if (window.parent.globalChange) {
                AxDoPegApprovalSave = "t♦" + _axPegJson[0].data.taskid;
                FormSubmit();
            } else {
                AxDoPegApprovalSave = "";
                ASB.WebService.AmdApproveWS(_keyFldInfo, _pegComment, _pegReasons, "false", "", SuccessCallbackPegApprove, OnExceptionPegApprove);
            }
        }
        catch (exp) {
            AxDoPegApprovalSave = "";
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackPegApprove(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                let _tiframes = parent.document.getElementsByTagName("iframe");
                if (typeof _tiframes != "undefined" && $(_tiframes).attr("id").startsWith("ldb")) {
                    parent.parent.window.location.reload();
                    showAlertDialog("success", resJson.result[0].msg);
                }
                else {
                    showAlertDialog("success", resJson.result[0].msg);
                    window.location.href = window.location.href;
                }
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionPegApprove(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdPeg", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}

function axPegReturn(isCommentReturn = "") {
    try {
        if (isCommentReturn == "")
            isPegApproveConfirm = "axreturn";
        else
            isPegApproveConfirm = "";
        let _axPegJson = AxPegJSON;
        let _keyFldInfo = "";
        if (typeof _axPegJson != "undefined" && _axPegJson != "") {
            _keyFldInfo = _axPegJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axPegJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axPegJson[0].data.processname;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskid;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskname;
            _keyFldInfo += "♦" + _axPegJson[0].data.transid;
        }

        if (typeof _axPegJson[0].data.cmsg_return != "undefined" && _axPegJson[0].data.cmsg_return != "" && isCommentReturn == "") {
            if (!axPegConfirmation("Return", _axPegJson[0].data.cmsg_return)) {
                return;
            }
        }

        let _pegReasons = "";
        let _pegRetLevel = "";
        let _pegComment = "";
        _pegComment = $("#txtPegReturnComments").length > 0 ? $("#txtPegReturnComments").val() : "";
        if (typeof _axPegJson[0].data.returncomments != "undefined" && _axPegJson[0].data.returncomments == "T" && _pegComment == "") {
            setTimeout(function () {
                document.querySelector(`a[data-popover="PegReturn"]`).click();
                document.querySelector(`textarea[id="txtPegReturnComments"]`).focus();
                setTimeout(function () {
                    $("#btnReturnPegMore").next('.Active_list_popover').removeAttr('style');
                    $("#btnReturnPegMore").next('.Active_list_popover').css({ "z-index": "105", "position": "absolute", "inset": "30px 10px auto auto" });
                }, 0);
            }, 10);
            showAlertDialog("error", "Return comments are mandatory.");
            return;
        }
        _pegReasons = $("#dllReturnPegReason").length > 0 ? $("#dllReturnPegReason").val() : "";

        _pegRetLevel = $("#dllReturnPegLevel").length > 0 ? $("#dllReturnPegLevel").val() : "0";


        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.AmdReturnWS(_keyFldInfo, _pegComment, _pegReasons, _pegRetLevel, "false", SuccessCallbackPegReturn, OnExceptionPegReturn);
        }
        catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackPegReturn(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                let _tiframes = parent.document.getElementsByTagName("iframe");
                if (typeof _tiframes != "undefined" && $(_tiframes).attr("id").startsWith("ldb")) {
                    parent.parent.window.location.reload();
                    showAlertDialog("success", resJson.result[0].msg);
                }
                else {
                    showAlertDialog("success", resJson.result[0].msg);
                    window.location.href = window.location.href;
                }
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionPegReturn(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdPeg", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}

function axPegReject(isCommentReject = "") {
    try {
        if (isCommentReject == "")
            isPegApproveConfirm = "axreject";
        else
            isPegApproveConfirm = "";
        let _axPegJson = AxPegJSON;
        let _keyFldInfo = "";
        if (typeof _axPegJson != "undefined" && _axPegJson != "") {
            _keyFldInfo = _axPegJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axPegJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axPegJson[0].data.processname;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskid;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskname;
            _keyFldInfo += "♦" + _axPegJson[0].data.transid;
        }
        if (typeof _axPegJson[0].data.cmsg_reject != "undefined" && _axPegJson[0].data.cmsg_reject != "" && isCommentReject == "") {
            if (!axPegConfirmation("Reject", _axPegJson[0].data.cmsg_reject)) {
                return;
            }
        }

        let _pegReasons = "";
        let _pegComment = "";
        _pegComment = $("#txtPegRejectComments").length > 0 ? $("#txtPegRejectComments").val() : "";
        if (typeof _axPegJson[0].data.rejectcomments != "undefined" && _axPegJson[0].data.rejectcomments == "T" && _pegComment == "") {
            setTimeout(function () {
                document.querySelector(`a[data-popover="PegReject"]`).click();
                document.querySelector(`textarea[id="txtPegRejectComments"]`).focus();
                setTimeout(function () {
                    $("#btnRejectPegMore").next('.Active_list_popover').removeAttr('style');
                    $("#btnRejectPegMore").next('.Active_list_popover').css({ "z-index": "105", "position": "absolute", "inset": "30px 10px auto auto" });
                }, 0);
            }, 10);
            showAlertDialog("error", "Reject comments are mandatory.");

            return;
        }
        _pegReasons = $("#dllRejectPegReason").length > 0 ? $("#dllRejectPegReason").val() : "";


        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.AmdRejectWS(_keyFldInfo, _pegComment, _pegReasons, "false", SuccessCallbackPegReject, OnExceptionPegReject);
        }
        catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackPegReject(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                let _tiframes = parent.document.getElementsByTagName("iframe");
                if (typeof _tiframes != "undefined" && $(_tiframes).attr("id").startsWith("ldb")) {
                    parent.parent.window.location.reload();
                    showAlertDialog("success", resJson.result[0].msg);
                }
                else {
                    showAlertDialog("success", resJson.result[0].msg);
                    window.location.href = window.location.href;
                }
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionPegReject(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdPeg", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}

function axPegWithdrw() {
    try {
        let _axPegJson = AxPegJSON;
        let _keyFldInfo = "";
        if (typeof _axPegJson != "undefined" && _axPegJson != "") {
            _keyFldInfo = _axPegJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axPegJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axPegJson[0].data.processname;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskid;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskname;
            _keyFldInfo += "♦" + _axPegJson[0].data.transid;
        }
        if (typeof _axPegJson[0].data.cmsg_withdraw != "undefined" && _axPegJson[0].data.cmsg_withdraw != "") {
            if (!axPegConfirmation("Withdraw", _axPegJson[0].data.cmsg_withdraw)) {
                return;
            }
        }
        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.PegWithdrawWS(_keyFldInfo, "", "", "false", SuccessCallbackPegWithdrw, OnExceptionPegWithdrw);
        }
        catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackPegWithdrw(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                showAlertDialog("success", resJson.result[0].msg);
                window.location.href = window.location.href;
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionPegWithdrw(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdPeg", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}

function axPegReCall() {
    try {
        let _axPegJson = AxPegJSON;
        let _keyFldInfo = "";
        if (typeof _axPegJson != "undefined" && _axPegJson != "") {
            _keyFldInfo = _axPegJson[0].data.keyfield;
            _keyFldInfo += "♦" + _axPegJson[0].data.keyvalue;
            _keyFldInfo += "♦" + _axPegJson[0].data.processname;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskid;
            _keyFldInfo += "♦" + _axPegJson[0].data.taskname;
            _keyFldInfo += "♦" + _axPegJson[0].data.transid;
        }
        if (typeof _axPegJson[0].data.cmsg_recall != "undefined" && _axPegJson[0].data.cmsg_recall != "") {
            if (!axPegConfirmation("Recall", _axPegJson[0].data.cmsg_recall)) {
                return;
            }
        }
        ShowDimmer(true);
        AxWaitCursor(true);
        try {
            ASB.WebService.PegReCallWS(_keyFldInfo, "", "", "false", SuccessCallbackPegReCall, OnExceptionPegReCall);
        }
        catch (exp) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        function SuccessCallbackPegReCall(result, eventArgs) {
            try {
                var resJson = $j.parseJSON(result);
            } catch (ex) {
                ShowDimmer(false);
            }
            if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].status != "undefined" && resJson.result[0].status == "success") {
                showAlertDialog("success", resJson.result[0].msg);
                window.location.href = window.location.href;
            } else if (resJson != undefined && typeof resJson.result != "undefined" && typeof resJson.result[0].error != "undefined" && resJson.result[0].error.status.toLowerCase() == "failed") {
                showAlertDialog("error", resJson.result[0].error.msg);
                ShowDimmer(false);
                AxWaitCursor(false);
            }
        }
        function OnExceptionPegReCall(result) {
            ShowDimmer(false);
            AxWaitCursor(false);
        }

        callParentNew("modalIdPeg", "id").dispatchEvent(new CustomEvent("close"));
    } catch (ex) { }
}
function axPegConfirmation(pegType, confMsg) {
    if (confirm(confMsg)) {
        return true;
    } else {
        return false;
    }
}

function AxProcessObjRefresh(fldObj) {
    try {
        if (typeof parent?.axProcessObj != "undefined" && typeof parent?.axProcessObj?.cardParams != "undefined") {
            var fldId = fldObj.attr("id");
            var _fName = GetFieldsName(fldId);
            if (typeof parent?.axProcessObj?.cardParams?.[_fName.toLowerCase()] != "undefined") {
                let cardIds = parent.axProcessObj.cardParams[_fName.toLowerCase()];
                let cardDepFlds = [];
                let cardParamsValues = {};
                cardIds.forEach((cardId) => {
                    cardDepFlds = [...new Set([...cardDepFlds, ...parent.axProcessObj.cardFlds[cardId]])];
                });
                cardDepFlds.forEach((cardDepFld) => {
                    let dcNo = GetDcNo(cardDepFld);
                    let fldIdx = AllFieldNames.indexOf(`${cardDepFld}000F${dcNo}`);
                    cardParamsValues[cardDepFld] = AllFieldValues[fldIdx];
                })
                parent.axProcessObj.refreshCards(cardIds, cardParamsValues);
            }
        }
    } catch (ex) { }
}

function AxProcessObjSaveRedirect(strResult) {
    try {
        if (typeof parent?.axProcessObj != "undefined" && typeof parent?.axProcessBuilderObj != "undefined") {
            let isSuccessFullSave = typeof strResult != "undefined" && ((typeof strResult[2] != "undefined" && strResult[2].indexOf('{\"result\":[{\"save\": \"success\"}]}') > -1) || (typeof strResult[3] != "undefined" && strResult[3].indexOf('{\"result\":[{\"save\": \"success\"}]}') > -1));
            if (window.frameElement.id == "rightIframe" && typeof parent.axProcessObj != "undefined" && isSuccessFullSave) {
                ShowDialog('success', 'Saved successfully.');
                if (parent.axProcessObj.keyValue == "NA") {
                    parent.axProcessObj.reloadProcess(axSaveRecId);
                }
                else {
                    parent.axProcessObj.taskCompleted = true;
                    parent.axProcessObj.refreshProcess(parent.axProcessObj.keyValue);
                }
            }
            else if (window.frameElement.id == "rightIframe" && typeof parent.axProcessBuilderObj != "undefined" && isSuccessFullSave) {
                ShowDialog('success', 'Saved successfully.');
                parent.axProcessBuilderObj.fetchProcessDefinition("Process");
            }
        }
    } catch (ex) { }
}

function AxProcessObjSaveRedirectAction(strResult) {
    try {
        let isSuccessFullSave = (typeof strResult != "undefined" && strResult.indexOf('{\"result\":[{\"save\": \"success\"}]}') > -1);
        let isSuccessFullRemove = (typeof strResult != "undefined" && strResult.indexOf(`{"message":[{"msg":"Transaction has been deleted"}]}`) > -1);
        if (window.frameElement.id == "rightIframe" && typeof parent.axProcessBuilderObj != "undefined" && (isSuccessFullSave || isSuccessFullRemove)) {
            //isSuccessFullSave ? ShowDialog('success', 'Saved successfully.') : ShowDialog('success', 'Task is removed.')
            parent.axProcessBuilderObj.fetchProcessDefinition("Process");
        }
    } catch (ex) { }
}


//TO Get LoadData for Discard from client side with the result & tstruct HTML.
function GetLoadDataForDiscard(recid, tstQureystr) {
    try {
        GetProcessTime();
        let _isDupTab = callParentNew('isDuplicateTab');
        if ((window.location.href).indexOf("AxSplit") != -1) {
            tstQureystr += tstQureystr.indexOf("AxSplit") == -1 ? `${tstQureystr.endsWith("♠") ? "": "♠"}AxSplit=true` : "";
        }
        $.ajax({
            url: 'tstruct.aspx/GetLoadDataForDiscard',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                key: tstDataId,
                recordid: recid,
                tstQureystr: tstQureystr,
                isDupTab: _isDupTab,
                isTstHtmlLs: resTstHtmlLS
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                resTstHtmlLS = "";
                var result = data.d;
                if (result != "") {
                    if (result.split("*♠♦*").length > 1) {
                        serverprocesstime = result.split("*♠♦*")[1];
                        requestProcess_logtime = result.split("*♠♦*")[2];
                        result = result.split("*♠♦*")[0];
                        WireElapsTime(serverprocesstime, requestProcess_logtime, true);
                    } else {
                        UpdateExceptionMessageInET("Error : " + result);
                    }
                }
                Closediv();
                if (result.toLowerCase().indexOf("access violation") === -1) {
                    ArrActionLog = "";
                    if (CheckSessionTimeout(result))
                        return;
                    if (result != "") {
                        actionCallbackFlag = actionCallFlag;
                        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                            "pointer-events": "auto"
                        });
                        if (FromSave) {
                            FromSave = false;
                        }
                        EnableSaveBtn(true);
                        isReadyCK = false
                        DCFrameNo.forEach(function (dcID) {
                            ClearFieldsInDC(dcID);
                            try {
                                let _isGrid = IsDcGrid(dcID);
                                if (_isGrid)
                                    ClearRowsInGrid(dcID, "htmlload");
                            } catch (ex) { }
                        });
                        ExprPosArray.forEach(function (vals, ind) {
                            if (vals != "")
                                ExprPosArray[ind] = "";
                        });
                        tstReadOnlyPeg = false;
                        navValidator = true;
                        SetFormDirty(false);
                        appstatus = "";
                        blurNextPreventId = "";
                        AxFormControlList = new Array();
                        AxSetFldCaption = new Array();
                        RegVarFldList = new Array();
                        ChangedFields = new Array();
                        ChangedFieldDbRowNo = new Array();
                        ChangedFieldValues = new Array();
                        DeletedDCRows = new Array();
                        ChangedDcRows = new Array();
                        ChangedDcs = new Array();
                        AllFieldNames = new Array();
                        AllFieldValues = new Array();
                        ScriptMaskFields = new Array();
                        AxExecFormControl = false;
                        DisabledDcs = new Array();
                        AxFormContHiddenFlds = new Array();
                        AxFormContSetCapFlds = new Array();
                        AxFormContSetCapFldsGrid = new Array();
                        AxFormContSetFldActGrid = new Array();
                        AxFormContSetGridCell = new Array();
                        AxFormContFldSetFocus = new Array();
                        multiSelectflds = new Array();
                        multiSelFldParents = new Array();
                        multiSelFldResult = new Array();
                        multiSelectLoadVals = new Array();
                        AxRulesFlds = new Array();
                        FldListParents = new Array();
                        FldListData = new Array();
                        FldDListPFNames = new Array();
                        FldDListPFNameVals = new Array();
                        changeFillGridDc = 0;
                        imgNames = new Array();
                        imgSrc = new Array();

                        if (isMobile)
                            OnMobileNewTst();

                        document.title = "Load Tstruct";
                        recordid = recid;
                        $j("#recordid000F0").val(recid);
                        var resval = result.split("*$*");
                        if (resval[0] == "") {
                            window.location.reload();
                            return;
                        }
                        //$("#tblWrk").html("");
                        //$("#collapseOneTable").html("");
                        $j(".workflow").remove();
                        $(".wfselectbox").html("<div class=\"menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-bold w-200px py-3\" data-kt-menu=\"true\" data-popper-placement=\"bottom-end\" id=\"selectbox\"></div>");
                        $j(".wrkflwinline").remove();
                        $j(".workflowMsg").remove();
                        $j(".downArr").remove();
                        $j(".Selectboxlist").remove();
                        //$("#selectbox").remove();
                        $j("#workflowoverlay").addClass("d-none");
                        $j("#dvMessage").html("");
                        $j("#dvMessage").removeClass("AXinfo").addClass("success d-none");
                        $j("#file").addClass("d-none");
                        $j("#attachment-overlay").html("");
                        $j("#attachfname").val("");
                        attachments = "";
                        filenamearray = new Array();
                        fileonloadarray = new Array();
                        $("#hdnTabHtml").val(resval[3]);
                        var tstPerfVars = resval[4];
                        if (tstPerfVars != "") {
                            wsPerfFormLoadCall = tstPerfVars.split(";")[0] == "true" ? true : false;
                            wsPerfEvalExpClient = tstPerfVars.split(";")[1].split(",");
                            headerAttachDir = tstPerfVars.split(";")[2];
                            try {
                                if (tstPerfVars.split(";").length > 3) {
                                    formParamFlds = tstPerfVars.split(";")[3].split(",");
                                    formParamVals = tstPerfVars.split(";")[4].split(",");
                                } else {
                                    formParamFlds = new Array();
                                    formParamVals = new Array();
                                }
                            } catch (ex) { }
                        }
                        var ImgVals = resval[5];
                        if (ImgVals != "") {
                            ImgVals = ImgVals.split("♠");
                            for (i = 0; i < ImgVals.length; i++) {
                                imgNames[i] = ImgVals[i].split("♦")[0];
                                imgSrc[i] = ImgVals[i].split("♦")[1];
                                if (imgSrc[i] != "")
                                    imgSrc[i] = imgSrc[i].replace("%20", " ");
                            }
                        }
                        if (resval[6] != "") {
                            var dcStatus = resval[6].split(',');
                            dcStatus.forEach(function (dcStat, indx) {
                                TabDCStatus[indx] = dcStat;
                            })
                        }
                        tstructCancelled = resval[7];
                        if (typeof AxGridAttNotExistList != "undefined" && resval[8] != "") {
                            var agattnotfiles = resval[8].split(',');
                            AxGridAttNotExistList = agattnotfiles;
                        } else if (resval[8] != "") {
                            var agattnotfiles = resval[8].split(',');
                            AxGridAttNotExistList = agattnotfiles;
                        } else
                            AxGridAttNotExistList = "";
                        tstDataId = resval[11];

                        try {
                            FFieldHidden = new Array();
                            FFieldReadOnly = new Array();

                            if (resval[9] != "") {
                                var _thisFFH = resval[9].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldHidden.push(_val);
                                })
                            }
                            if (resval[10] != "") {
                                var _thisFFH = resval[10].split(',');
                                _thisFFH.forEach(function (_val) {
                                    if (_val != "")
                                        FFieldReadOnly.push(_val);
                                })
                            }
                        } catch (ex) { }

                        $j("#hdnDataObjId").val(tstDataId);
                        LoadResult = result;
                        Closediv();
                        isLoadDataCall = true;
                        ReloadJqueryReference();
                        //if (typeof isWizardTstruct == "undefined" || isWizardTstruct == false) {
                        isTstPostBackVal = resval[0] + "*$*" + resval[1] + "*$*" + resval[2];
                        //}
                    }
                } else {
                    blurNextPreventId = "";
                    actionCallbackFlag = actionCallFlag;
                    $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                        "pointer-events": "auto"
                    });
                    if (FromSave) {
                        FromSave = false;
                    }
                    EnableSaveBtn(true);
                    AxWaitCursor(false);
                    ShowDimmer(false);
                    SetFormDirty(false);
                    $("#reloaddiv").show();
                    $("#dvlayout").hide();
                }
            },
            error: function (error) {
                Closediv();
                blurNextPreventId = "";
                actionCallbackFlag = actionCallFlag;
                $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
                    "pointer-events": "auto"
                });
                if (FromSave) {
                    FromSave = false;
                }
                EnableSaveBtn(true);
                AxWaitCursor(false);
                ShowDimmer(false);
                SetFormDirty(false);
                $("#reloaddiv").show();
                $("#dvlayout").hide();
            }
        });
    } catch (exp) {
        Closediv();
        blurNextPreventId = "";
        actionCallbackFlag = actionCallFlag;
        $("#icons,#btnSaveTst,.BottomToolbarBar a,.wizardNextPrevWrapper,.toolbarRightMenu a,.toolbarRightMenu button").css({
            "pointer-events": "auto"
        });
        if (FromSave) {
            FromSave = false;
        }
        EnableSaveBtn(true);
        AxWaitCursor(false);
        ShowDimmer(false);
        SetFormDirty(false);
        $("#reloaddiv").show();
        $("#dvlayout").hide();
    }
}


function GetCachedSaveNotification(axmsgid) {
    try {
        $.ajax({
            url: 'tstruct.aspx/GetCachedSaveNotificationJSON',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                transid: transid,
                axmsgid: axmsgid
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                if (data.d != "" && data.d != "error") {
                    bindNotificationFldData(data.d);
                }
            },
            error: function (error) {
            }
        });
    } catch (ex) {
    }
}

function bindNotificationFldData(notifyData) {
    try {
        notifyData = notifyData.replace(/\\/g, '\\\\');
        var _json = JSON.parse(notifyData);
        _json = _json.savedata.recdata;
        var _dataJson = `{"data":[`;
        for (var i = 0; i < _json.length; i++) {
            let _ic = i + 1;
            if (IsDcGrid(_ic)) {
                let _crnode = "";
                for (var j = 0; j < _json[i]["axp_recid" + _ic].length; j++) {
                    _crnode += `i` + (j + 1) + `,`;
                }
                if (_crnode != "")
                    _crnode = _crnode.slice(0, -1);
                _dataJson += `{"n":"DC` + _ic + `","v":"` + _ic + `","cr":"` + _crnode + `","hasdatarows":"` + (_crnode == "" ? "no" : "yes") + `","t":"dc"},`;
            } else
                _dataJson += `{"n":"DC` + _ic + `","v":"` + _ic + `","t":"dc"},`;
            for (var j = 0; j < _json[i]["axp_recid" + _ic].length; j++) {
                let _rowN = _json[i]["axp_recid" + _ic].length;// == 1 ? 0 : j + 1;
                if (IsDcGrid(_ic))
                    _rowN = j + 1;
                _dataJson += `{"n":"axp_recid` + _ic + `","v":"0","r":"` + _rowN + `","t":"s"},`;
                for (_key in _json[i]["axp_recid" + _ic][j].columns) {
                    let _val = _json[i]["axp_recid" + _ic][j].columns[_key];
                    _dataJson += `{"n":"` + _key + `","v":"` + _val + `","r":"` + _rowN + `","t":"s"},`;
                }
            }
        }
        _dataJson = _dataJson.slice(0, -1);
        _dataJson += `]}`;
        if (_dataJson != "") {
            try {
                let myJSONObject = $j.parseJSON(_dataJson);
                ExecData(myJSONObject.data, "LoadData", true);
            }
            catch (ex) {
            }
        }
    } catch (ex) {

    }
}

function publishAPIInputJSON(thisFldid) {
    try {
        if (thisFldid == "apitype000F1" || thisFldid == "structcapui000F1" || thisFldid == "unameui000F1" || thisFldid == "objdatasrc000F1" || thisFldid =="scriptcapui000F1") {
            if (thisFldid == "apitype000F1") {
                setTimeout(function () {
                    $("#structcapui000F1").val('').trigger('change');
                }, 0);

                UpdateFieldArray("structcapui000F1", "0", "", "parent", "AutoComplete");
                UpdateAllFieldValues("structcapui000F1", "");
                $("#apirequeststring000F1").val('');
                UpdateFieldArray("apirequeststring000F1", "0", "", "parent", "AutoComplete");
                UpdateAllFieldValues("apirequeststring000F1", "");
                $("#apisuccess000F1").val('');
                UpdateFieldArray("apisuccess000F1", "0", "", "parent", "AutoComplete");
                UpdateAllFieldValues("apisuccess000F1", "");
                $("#apierror000F1").val('');
                UpdateFieldArray("apierror000F1", "0", "", "parent", "AutoComplete");
                UpdateAllFieldValues("apierror000F1", "");               
            }

            if (thisFldid == "structcapui000F1") {
                setTimeout(function () {
                    $("#scriptcapui000F1").val('').trigger('change');
                }, 0);
                UpdateFieldArray("scriptcapui000F1", "0", "", "parent", "AutoComplete");
                UpdateAllFieldValues("scriptcapui000F1", "");
            }
            if ($("#apitype000F1").val() != null && $("#apitype000F1").val() == "Submit Data" && $("#structcapui000F1").val() != null && $("#structcapui000F1").val() != "" && $("#unameui000F1").val() != null && $("#unameui000F1").val() != "") {
                let strtrId = $("#objname000F1").val();
                let formName = $("#structcap000F1").val();
                formName = formName.substr(0, formName.indexOf('('));
                let _uname = $("#uname000F1").val();
                $.ajax({
                    type: "POST",
                    url: "tstruct.aspx/PublishAPISubmitDataJSON",
                    data: JSON.stringify({
                        strTrId: strtrId
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d != "error") {
                            let resp = response.d;
                            var tempfldJSON = JSON.parse(resp.split('¿')[0]);
                            var tstDCJSON = JSON.parse(resp.split('¿')[1]);
                            var _strFldDetails = "";
                            $.each(tstDCJSON["result"]["row"], function (i, e) {
                                var tempjson = {};
                                var dcName = (e.dname || e.DNAME);
                                _strFldDetails += ',"' + dcName + '":{\"row1\":{';
                                var fldCnt = 1;
                                $.each(tempfldJSON["result"]["row"], function (ind, ele) {
                                    if ((ele.dcname || ele.DCNAME) === dcName) {
                                        if (fldCnt == 1)
                                            _strFldDetails += '"' + (ele.fname || ele.FNAME) + '":"FIELD_VALUE"';
                                        else
                                            _strFldDetails += ',"' + (ele.fname || ele.FNAME) + '":"FIELD_VALUE"';
                                        fldCnt++;
                                    }
                                });
                                _strFldDetails += '}}';
                            });

                            $("#apirequeststring000F1").val('{"SecretKey":"","publickey":"","project":"' + proj + '","submitdata":{"username":"' + _uname + '","trace":"false","keyfield":"","dataarray":{"data":{"mode":"new","keyvalue": "","recordid": "0" ' + _strFldDetails + '}}}}');

                            UpdateFieldArray("apirequeststring000F1", "0", $("#apirequeststring000F1").val(), "parent", "AutoComplete");
                            UpdateAllFieldValues("apirequeststring000F1", $("#apirequeststring000F1").val());

                            $("#apisuccess000F1").val('{"success":true,"result":"' + formName + ' Saved,recordid=1720660000001"}');
                            UpdateFieldArray("apisuccess000F1", "0", $("#apisuccess000F1").val(), "parent", "AutoComplete");
                            UpdateAllFieldValues("apisuccess000F1", $("#apisuccess000F1").val());

                            $("#apierror000F1").val('{"result": [{"error": {"status": "Failed","msg": "Sessionid not specified in call to submitdata" } }] }');
                            UpdateFieldArray("apierror000F1", "0", $("#apierror000F1").val(), "parent", "AutoComplete");
                            UpdateAllFieldValues("apierror000F1", $("#apierror000F1").val());
                        }
                        else {
                            ShowDialog("error", "Error while Getting Input JSON!");
                        }
                    },
                    error: function (error) {
                        ShowDialog("error", "Error while Getting Input JSON!");
                    }
                });
            }
            else if ($("#apitype000F1").val() != null && $("#apitype000F1").val() == "Get Iview Parameters" && $("#structcapui000F1").val() != null && $("#structcapui000F1").val() != "" && $("#unameui000F1").val() != null && $("#unameui000F1").val() != "") {
                let strtrId = $("#objname000F1").val();
                let formName = $("#structcap000F1").val();
                formName = formName.substr(0, formName.indexOf('('));
                let _uname = $("#uname000F1").val();

                $("#apirequeststring000F1").val('{"SecretKey":"","publickey":"","Project":"' + proj + '","getreportparams":{"username":"' + _uname + '","trace": "false"}}');

                $("#apisuccess000F1").val('{"success":true,"result":{"pform":"yes","name":"' + strtrId + '","caption":"' + formName + '","updatedon":"21/08/2023 18:28:21","updatedby":"' + _uname + '","showparams":"false","params":{"pname":{"name":"pname","caption":"pname","order":"1","datatype":"Character","multiselect":"false","expression":null,"validateexpression":null,"hidden":"false","decimals":"0","savevalue":"true","dynamicparam":"false","defaultvalue":null,"modeofentry":"Select","sql":"","response":[{"name":"ALL"},{"name":"syamala"},{"name":"11"}]}}}}');

                $("#apierror000F1").val('{​​​​​​​​"result":[{​​​​​​​​"error":"Sessionid not specified in call to Get Iview Parameters"}​​​​​​​​]}');
            }
            else if ($("#apitype000F1").val() != null && $("#apitype000F1").val() == "Get Iview" && $("#structcapui000F1").val() != null && $("#structcapui000F1").val() != "" && $("#unameui000F1").val() != null && $("#unameui000F1").val() != "") {
                let strtrId = $("#objname000F1").val();
                let formName = $("#structcap000F1").val();
                formName = formName.substr(0, formName.indexOf('('));
                let _uname = $("#uname000F1").val();

                $("#apirequeststring000F1").val('{"SecretKey":"","publickey": "","Project":"' + proj + '","getreport":{"metadata":"false","pageno":"1","pagesize":"100","sqlpagination":"false","params":{"param1":"value1"//Add the respective params of the iview}}}');

                $("#apisuccess000F1").val('{"success":true,"result":{"row":[{"rowno":"1","emp":"emp0002","city":"","age":"0"}]}}');

                $("#apierror000F1").val('{​​​​​​​​"result":[{​​​​​​​​"error":"Sessionid not specified in call to Get Iview"}​​​​​​​​]}');
            }
            else if ($("#apitype000F1").val() != null && $("#apitype000F1").val() == "Get Print Form" && $("#structcapui000F1").val() != null && $("#structcapui000F1").val() != "" && $("#unameui000F1").val() != null && $("#unameui000F1").val() != "") {
                let strtrId = $("#printform000F1").val();
                let _uname = $("#uname000F1").val();

                $("#apirequeststring000F1").val('{"secretkey":"","publickey":"","Project":"' + proj + '","getprintform":{"username":"' + _uname + '","recordid": "0","printform":"' + strtrId + '","trace":"false"}}');

                $("#apisuccess000F1").val('{"success":true,"result":"JVBERi0xLjcNCiXi48/TDQo2IDAgb2JqDQo8PC9MZW5n……"}');

                $("#apierror000F1").val('{​​​​​​​​"result":[{​​​​​​​​"error":"Sessionid not specified in call to Get Print Form"}​​​​​​​​]}');
            }
            else if ($("#apitype000F1").val() != null && $("#apitype000F1").val() == "Get From Custom Data source" && $("#objdatasrc000F1").val() != null && $("#objdatasrc000F1").val() != "" && $("#unameui000F1").val() != null && $("#unameui000F1").val() != "") {
                let strtrId = $("#objname000F1").val();
                let _uname = $("#uname000F1").val();
                let _paramJson = "";
                $.ajax({
                    type: "POST",
                    url: "tstruct.aspx/CreateSqlRequestJSON",
                    data: JSON.stringify({
                        custSqlName: strtrId
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d != "error") {
                            _paramJson = response.d;
                            $("#apirequeststring000F1").val('{"SecretKey":"","publickey":"","Project":"' + proj + '","getsqldata":{"trace":"true"}' + _paramJson + '}');

                            $("#apisuccess000F1").val('{"success":true,"' + strtrId + '":[{"username":"user1","a":"abc","b":"bca"},{"username":"user2","a":"xyz","b":"xyz"}]}');

                            $("#apierror000F1").val('{​​​​​​​​"result":[{​​​​​​​​"error":"Sessionid not specified in call to Get From Custom Data Source"}​​​​​​​​]}');
                        }
                        else {
                            _paramJson = "";
                            $("#apirequeststring000F1").val('{"SecretKey":"","publickey":"","Project":"' + proj + '","getsqldata":{"trace":"true"}' + _paramJson + '}');

                            $("#apisuccess000F1").val('{"success":true,"' + strtrId + '":[{"username":"user1","a":"abc","b":"bca"},{"username":"user2","a":"xyz","b":"xyz"}]}');

                            $("#apierror000F1").val('{​​​​​​​​"result":[{​​​​​​​​"error":"Sessionid not specified in call to Get From Custom Data Source"}​​​​​​​​]}');
                        }
                    },
                    error: function (error) {
                        _paramJson = "";
                    }
                });
            } else if ($("#apitype000F1").val() != null && $("#apitype000F1").val() == "Execute script" && $("#structcapui000F1").val() != null && $("#structcapui000F1").val() != "" && $("#unameui000F1").val() != null && $("#unameui000F1").val() != "" && $("#scriptcap000F1").val() != null && $("#scriptcap000F1").val() != "") {
                let strtrId = $("#structcapuiname000F1").val();
                let _scriptcap = $("#scriptcap000F1").val();

                $("#apirequeststring000F1").val('{"secretkey":"","publickey":"","Project":"' + proj + '","executescript":{"script":"' + _scriptcap + '","type": "form","name":"' + strtrId + '","recordid":"0","trace":"false"}}');

                $("#apisuccess000F1").val('{"status":"success","msg":"Script executed successfully"}');

                $("#apierror000F1").val('{"status":"failed","result":"Script \"' + _scriptcap + '\" execution failed, Recordid not defined to create PDF file"}');
            }
        }
    } catch (ex) { }
}

function saveFillgridColResize(_thisDcno, _thisFillgridCaption, _thisFillgridName) {
    var headerCellWidths = [];
    $('#dvFillGrid .dataTables_scrollHeadInner .dataTable thead th').each(function (index) {
        var cellWidth = this.offsetWidth;
        headerCellWidths.push(cellWidth);
    });
    let _strWidth = headerCellWidths.join(",");
    ShowDimmer(true);
    try {
        ASB.WebService.SaveFillgridResize(transid, _thisFillgridCaption, _strWidth, SuccessCallbackFGResize, OnExceptionFGResize);
    }
    catch (exp) {
        ShowDimmer(false);
    }

    function SuccessCallbackFGResize(result, eventArgs) {
        try {
            ShowDimmer(false);
            if (result != undefined && result == "Saved successfully.") {
                showAlertDialog("success", "Resize successfully saved.");
                $("#dvFillGrid .btn-close").click();
                setTimeout(function () {
                    FillGrid(_thisDcno, '', _thisFillgridName);
                }, 100);
            } else if (result != undefined && result.startsWith("error:")) {
                showAlertDialog('error', result.replace(/^error:/, ''));
            }
            else {
                showAlertDialog('error', "There was an error please try again");
            }
        } catch (ex) {
            ShowDimmer(false);
        }
    }
    function OnExceptionFGResize(result) {
        ShowDimmer(false);
    }
}
function clearCacheReloadForm(_thisTrId) {
    let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
    try {
        ShowDimmer(true);
        let _thisKey = callParentNew("getKeysWithPrefix(tstHtml♠" + _thisTrId + "-" + appSUrl + "♥)", "function");
        if (_thisKey.length > 0) {
            for (const val of _thisKey) {
                localStorage.removeItem(val);
            }
        }
        //if (callParentNew("originaltrIds").filter(x => x == _thisTrId).length == 0) {
        //    let _thisInd = callParentNew("originaltrIds").indexOf(_thisTrId);
        //    callParentNew("originaltrIds").splice(_thisInd, 1);
        //}
        try {
            let appSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            let storedKey = 'originaltrIds-' + appSessUrl;
            let transidArray = JSON.parse(localStorage.getItem(storedKey) || '[]');
            let index = transidArray.indexOf(_thisTrId);
            if (index > -1) {
                transidArray.splice(index, 1);
                localStorage.setItem(storedKey, JSON.stringify(transidArray));
            }
        } catch (ex) { }
        
        $.ajax({
            url: 'tstruct.aspx/ClearCacheTstKeys',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                Transid: _thisTrId
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                callParentNew('globalChange=', false);
                let _thisUri = 'tstruct.aspx?transid=' + _thisTrId + '&openerIV=tldt&isIV=false';
                callParentNew("LoadIframe(" + _thisUri + ")", "function");
            }, error: function (error) {
                ShowDimmer(false);
            }
        });
    } catch (ex) {
    }
}

function BindAxpFilesonDep(_thisFldName, _thisfldValue) {
    const id = $("#" + _thisFldName).parent().find(".dropzone").attr("id");
    const dropzone = document.querySelector("#" + id);
    var _myDropzone = Dropzone.forElement(dropzone);
    var files = _myDropzone.files;
    let _isfiledel = false;
    for (var i = 0; i < files.length; i++) {
        _myDropzone.removeFile(files[i]);
        _isfiledel = true;
    }
    if (_isfiledel) {
        DeletedFieldValue.length = 0;
        $("#" + _thisFldName).val(_thisfldValue);
    }   
    let fuInpId = $("#" + id).attr("id").substr(9);
    let funame = fuInpId.substring(0, fuInpId.lastIndexOf("F") - 3);
    let isthisReadOnly = false;
    var _ffuIndex = $j.inArray(funame, FNames);
    if (FFieldReadOnly[_ffuIndex] == "True") {
        isthisReadOnly = true;
    }
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
            _myDropzone.emit("addedfile", file);
            _myDropzone.emit("complete", file);
            _myDropzone.files.push(file);

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
            adjustFileNameWidths($(file.previewElement), _myDropzone);
            $(file.previewElement).find(".dropzone-toolbar").css({ "margin-left": "auto" });


            var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
            let filePath = hdnScriptsUrlPath.val() + "axpert/" + sid + "/";
            filePath += _fullval;

            $(file.previewElement).find(".dropzone-filename span").text(val);
            $(file.previewElement).find(".dropzone-filename span").attr("data-fullfile", _fullval);
            $(file.previewElement).find(".dropzone-filename").attr("onclick", "ShowAxpFileuploadLink('" + $(_myDropzone.files[0].previewElement).parents(".dropzone").attr("id").substr(9) + "','" + _fullval + "','" + filePath + "',event)");
            if (isthisReadOnly)
                $(file.previewElement).find(".dropzoneItemDelete").addClass('d-none');
        });
        //$("#" + id).find(".fileuploadmore").removeClass("d-none");
        //$("#" + id).find("#gridAttachCounter").removeClass("d-none");
        //$("#" + id).find("span.spanAttCount").text(_myDropzone.files.length);
    }
}


//Function to open the list view on click of listview button in the toolbar.
function CallEntityList(tid) {
    if (typeof AxRulesBtnConMsg != "undefined" && AxRulesBtnConMsg == "true") {
        if (!AxRulesBtnConMsgParser(event.currentTarget)) {
            return;
        }
    }
    if (window.parent.globalChange) {
        var cutMsg = eval(callParent('lcm[31]'));
        var glType = eval(callParent('gllangType'));
        var isRTL = false;
        if (glType == "ar")
            isRTL = true;
        else
            isRTL = false;
        var CallListViewCB = $.confirm({
            theme: 'modern',
            closeIcon: false,
            title: eval(callParent('lcm[155]')),
            rtl: isRTL,
            onContentReady: function () {
                disableBackDrop('bind');
            },
            backgroundDismiss: true,
            escapeKey: 'buttonB',
            content: cutMsg,
            buttons: {
                buttonA: {
                    text: eval(callParent('lcm[164]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        CallListViewCB.close();
                        ShowDimmer(true);
                        window.parent.listViewPage = "0";
                        if (frameElement.name == "axpiframe" && $(frameElement).hasClass("frameSplited")) {
                            callParentNew(`OpenOnPropertyBase(Entity.aspx?tstid=${tid}&isDupTab=${callParentNew('isDuplicateTab')})`, 'function');
                        } else {
                            window.document.location.href = `./Entity.aspx?tstid=${tid}&isDupTab=${callParentNew('isDuplicateTab')}`;
                        }
                        SetFormDirty(false);
                        window.parent.isSessionCleared = true;
                    }
                },
                buttonB: {
                    text: eval(callParent('lcm[192]')),
                    btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                    action: function () {
                        ShowDimmer(false);
                        disableBackDrop('destroy');
                        closeParentFrame();
                        window.parent.isSessionCleared = true;
                    }
                }

            }
        });

    } else {
        window.parent.listViewPage = "0";
        ShowDimmer(true);
        if (frameElement.name == "axpiframe" && $(frameElement).hasClass("frameSplited")) {
            callParentNew(`OpenOnPropertyBase(Entity.aspx?tstid=${tid}&isDupTab=${callParentNew('isDuplicateTab')})`, 'function');
        } else {
            window.document.location.href = `./Entity.aspx?tstid=${tid}&isDupTab=${callParentNew('isDuplicateTab')}`;
        }
        window.parent.isSessionCleared = true;
    }
}

function CheckTransReadOnly() {
    try {
        const currentUrl = window.location.href;
        const urlParams = new URL(currentUrl);
        const trromode = urlParams.searchParams.get("trromode");
        if (trromode != null && (trromode == "t" || trromode == "true")) {
            $("[id^=DivFrame").find('input,textarea, img, select, a').attr('disabled', true);
            $("[id^=DivFrame").find('.gridIconBtns a').addClass('disabled');
            $("[id^=DivFrame").find('.gridIconBtns a').attr('disabled', true);
            $("[id^=DivFrame").find('.gridRowChk,.gridHdrChk').attr('disabled', true);
            $(".BottomToolbarBar").find('a').addClass('disabled');
            $(".BottomToolbarBar").find('a').attr('disabled', true);
            $(".BottomToolbarBar").find('a').attr('tabindex', -1);
            $(".toolbarRightMenu").find('a').addClass('disabled');
            $(".toolbarRightMenu").find('a').attr('disabled', true);
            $(".toolbarRightMenu").find('button').attr('disabled', true);
            $(".toolbarRightMenu").find('button').addClass('disabled');
            $(".toolbarRightMenu").find('button').css({ "pointer-events": "none" });
            $(".toolbarRightMenu").find('a').attr('tabindex', -1);
            $(".toolbarRightMenu").find('button').attr('tabindex', -1);
            $(".toolbarRightMenu").find('a').css({ "pointer-events": "none" });
            $(".dz-hidden-input").prop("disabled", true);
            $(".fldImageCamera").addClass('disabled');
            $(".fileuploadmore").prop("disabled", true);
            $("#icons,#btnSaveTst,.BottomToolbarBar a,a.tstformbutton,a.axpBtnCustom,.formLabelParent a").css({
                "pointer-events": "none"
            });

            $("a.tstformbutton,a.axpBtnCustom,.formLabelParent a").addClass('disabled');
            $("a.tstformbutton,a.axpBtnCustom,.formLabelParent a").attr('disabled', true);
            $("a.tstformbutton,a.axpBtnCustom,.formLabelParent a").attr('tabindex', -1);
            $("a.tstformbutton,a.axpBtnCustom,.formLabelParent a").removeAttr("onclick");
        }
    } catch (ex) {
    }
}

var userVType = "";
var userTypePrev = "";
function UserActivateBtns(_fieldID) {
    if (transid == 'axurg' && _fieldID == 'pusername000F1') {
        $("#btn17").removeAttr("onclick");
        $("#btn17").attr("onclick", "UserActivation();");
        $("#btn18").removeAttr("onclick");
        $("#btn18").attr("onclick", "UserDeactivate();");
        $("#btn19").removeAttr("onclick");
        $("#btn19").attr("onclick", "UserUpdate();");
        userVType = "";
        userTypePrev = "";
        if ($("#authkey000F1").val() != "" && $("#userkey000F1").val()!="") {
            GetALCUserInfo();
        }
        else {
            if ($("#authkey000F1").val() == "" && $("#userkey000F1").val() == "" && $("#IsActive000F1").val() == "T") {
                $("#IsActive000F1").val('F');
                $("#IsActive000F1").prop("checked", false);
            }
            if ($("#IsActive000F1").val() == "T" && $("#authkey000F1").val() != "") {
                if ($("#usertype000F1").val() == "")
                    $("#usertype000F1").val("N");
                $("#btn18").parent("div").removeClass("d-none");
                $("#btn19").parent("div").addClass("d-none");
                $("#btn17").parent("div").addClass("d-none");
            } else if ($("#IsActive000F1").val() != "" && ($("#IsActive000F1").val() == "F" || $("#authkey000F1").val() == "")) {
                if ($("#usertype000F1").val() == "")
                    $("#usertype000F1").val("N");
                $("#btn17").parent("div").removeClass("d-none");
                $("#btn19").parent("div").addClass("d-none");
                $("#btn18").parent("div").addClass("d-none");
            } else {
                $("#btn19").parent("div").addClass("d-none");
                $("#btn18").parent("div").addClass("d-none");
                $("#btn17").parent("div").addClass("d-none");
            }
        }
    }
}

function GetALCUserInfo() {
    try {
        AxWaitCursor(true);
        ShowDimmer(true);
        const _pUser = GetFieldValue("pusername000F1");
        const _puserkey = GetFieldValue("userkey000F1");
        const _pauthkey = GetFieldValue("authkey000F1");
        ASB.WebService.GetUserKeyInfo(_pUser, _puserkey, _pauthkey, SuccGetALCUserInfo, OnException);
    } catch (ex) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
}
function SuccGetALCUserInfo(result, eventArgs) {
    if (result == "") {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        return;
    }
    if (CheckSessionTimeout(result))
        return;

    AxWaitCursor(false);
    ShowDimmer(false);
    if (result.startsWith("error:")) {
        userVType = "";
        userTypePrev = "";
        if ($("#IsActive000F1").val() == "T" && $("#authkey000F1").val() != "") {
            if ($("#usertype000F1").val() == "")
                $("#usertype000F1").val("N");
            $("#btn18").parent("div").removeClass("d-none");
            $("#btn19").parent("div").addClass("d-none");
            $("#btn17").parent("div").addClass("d-none");
        } else if ($("#IsActive000F1").val() != "" && ($("#IsActive000F1").val() == "F" || $("#authkey000F1").val() == "")) {
            if ($("#usertype000F1").val() == "")
                $("#usertype000F1").val("N");
            $("#btn17").parent("div").removeClass("d-none");
            $("#btn19").parent("div").addClass("d-none");
            $("#btn18").parent("div").addClass("d-none");
        } else {
            $("#btn19").parent("div").addClass("d-none");
            $("#btn18").parent("div").addClass("d-none");
            $("#btn17").parent("div").addClass("d-none");
        }
    } else {
        const userValidType = result.split("#")[0];
        const userTypeKey = result.split("#")[1];
        userVType = userValidType;
        userTypePrev = userTypeKey;
        if (userValidType == "false" && $("#authkey000F1").val() != "") {
            $("#btn19").parent("div").removeClass("d-none");
            $("#btn19").attr("title", "Validity is over and needs to be updated for the user.");
            $("#btn18").parent("div").addClass("d-none");
            $("#btn17").parent("div").addClass("d-none");
            if ($("#usertype000F1").val() == "")
                $("#usertype000F1").val("N");
        } else if ($("#authkey000F1").val() != "" && $("#usertype000F1").val() != userTypeKey) {
            if ($("#usertype000F1").val() == "")
                $("#usertype000F1").val("N");
            $("#btn19").parent("div").removeClass("d-none");
            if ($("#usertype000F1").val() != "" && $("#usertype000F1").val()=="p")
                $("#btn19").attr("title", "User type has changed from Noraml user to Portal user.");
            else
                $("#btn19").attr("title", "User type has changed from Portal user to Normal user.");
            $("#btn18").parent("div").addClass("d-none");
            $("#btn17").parent("div").addClass("d-none");
        } else if (userValidType == "true" && $("#IsActive000F1").val() == "T" && $("#authkey000F1").val() != "") {
            if ($("#usertype000F1").val() == "")
                $("#usertype000F1").val("N");
            $("#btn18").parent("div").removeClass("d-none");
            $("#btn19").parent("div").addClass("d-none");
            $("#btn17").parent("div").addClass("d-none");
        } else if (userValidType == "true" && $("#IsActive000F1").val() == "F" && $("#authkey000F1").val() != "") {
            if ($("#usertype000F1").val() == "")
                $("#usertype000F1").val("N");
            $("#btn17").parent("div").removeClass("d-none");
            $("#btn19").parent("div").addClass("d-none");
            $("#btn18").parent("div").addClass("d-none");
        } else {
            $("#btn19").parent("div").addClass("d-none");
            $("#btn18").parent("div").addClass("d-none");
            $("#btn17").parent("div").addClass("d-none");
        }
    }
}

function UserActivation() {
    try {
        AxWaitCursor(true);
        ShowDimmer(true);
        const _pUser = GetFieldValue("pusername000F1");
        let _pUserType = GetFieldValue("usertype000F1");
        if (_pUserType != "" && _pUserType=="N") {
            _pUserType = "";
        }
        const _pUserActive = GetFieldValue("IsActive000F1");
        ASB.WebService.UserActivation(_pUser, _pUserType, _pUserActive, SuccUserActivation, OnException);
    } catch (ex) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
}
function SuccUserActivation(result, eventArgs) {
    if (result == "") {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        return;
    }
    if (CheckSessionTimeout(result))
        return;

    AxWaitCursor(true);
    ShowDimmer(true);
    if (window.opener)
        GetFormLoadData("AxIsPop=true", "", "true");
    else
        GetFormLoadData("", "", "true");
    if (result.startsWith("Error:")) {
        let errorMsg = result.replace("Error:", "");
        ShowDialog('error', errorMsg);
    }
    else
        showAlertDialog('success', result);
}
function UserDeactivate() {
    try {
        AxWaitCursor(true);
        ShowDimmer(true);
        const _pUser = GetFieldValue("pusername000F1");
        /*const _pUserType = GetFieldValue("usertype000F1");*/
        let _pUserType = GetFieldValue("usertype000F1");
        if (_pUserType != "" && _pUserType == "N") {
            _pUserType = "";
        }
        const _pUserActive = GetFieldValue("IsActive000F1");
        ASB.WebService.UserDeactivate(_pUser, _pUserType, _pUserActive, SuccUserDeactivate, OnException);
    } catch (ex) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
}
function SuccUserDeactivate(result, eventArgs) {
    if (result == "") {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        return;
    }
    if (CheckSessionTimeout(result))
        return;

    AxWaitCursor(true);
    ShowDimmer(true);
    if (window.opener)
        GetFormLoadData("AxIsPop=true", "", "true");
    else
        GetFormLoadData("", "", "true");
    if (result.startsWith("Error:")) {
        let errorMsg = result.replace("Error:", "");
        ShowDialog('error', errorMsg);
    }
    else
        showAlertDialog('success', result);
}

function UserUpdate() {
    try {
        AxWaitCursor(true);
        ShowDimmer(true);
        const _pUser = GetFieldValue("pusername000F1");
        /* const _pUserType = GetFieldValue("usertype000F1");*/
        let _pUserType = GetFieldValue("usertype000F1");
        if (_pUserType != "" && _pUserType == "N") {
            _pUserType = "";
        }
        const _pUserActive = GetFieldValue("IsActive000F1");
        ASB.WebService.UserUpdate(_pUser, _pUserType, _pUserActive, userTypePrev, userVType, SuccUserUpdate, OnException);
    } catch (ex) {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
    }
}
function SuccUserUpdate(result, eventArgs) {
    if (result == "") {
        AxWaitCursor(false);
        ShowDimmer(false);
        showAlertDialog("error", ServerErrMsg);
        return;
    }
    if (CheckSessionTimeout(result))
        return;

    AxWaitCursor(true);
    ShowDimmer(true);
    if (window.opener)
        GetFormLoadData("AxIsPop=true", "", "true");
    else
        GetFormLoadData("", "", "true");
    if (result.startsWith("Error:")) {
        let errorMsg = result.replace("Error:", "");
        ShowDialog('error', errorMsg);
    }
    else
        showAlertDialog('success', result);
}

function axDeveloperStudioTstToolbar() {
    try {
        if (typeof axpertDevTstList != "undefined" && axpertDevTstList.indexOf(transid) > -1) {
            $(".toolbarRightMenu a[title='List View'],.toolbarRightMenu a#tasks,.toolbarRightMenu a#design,.toolbarRightMenu a[title='print'],.toolbarRightMenu a[title='Print'],.toolbarRightMenu a[title='attach'],.toolbarRightMenu a[title='preview'],.toolbarRightMenu a[title='Preview']").addClass("d-none");
            $(".toolbarRightMenu a[onclick='javascript:OpenPdfDocList();']").addClass("d-none");
            $("#btnAppsHeader").parent().addClass('d-none');
            if ($("#btnAppsHeader").parent().find(".menu a[onclick^=\"javascript:CallAction('iSave',\"]").length > 0) {
                let _isaveclick = $("#btnAppsHeader").parent().find(".menu a[onclick^=\"javascript:CallAction('iSave',\"]").attr("onclick");
                $("#ftbtn_iSave").removeAttr("onclick").attr("onclick", _isaveclick);
                $("#ftbtn_iSave").removeClass("d-none");
            }
            if ($("#btnAppsHeader").parent().find(".menu .menu-sub-dropdown a[onclick^=\"javascript:CallAction('iRemove',\"]").length > 0) {
                let _iremoveclick = $("#btnAppsHeader").parent().find(".menu .menu-sub-dropdown a[onclick^=\"javascript:CallAction('iRemove',\"]").attr("onclick");
                $(".toolbarRightMenu").find("[onclick='javascript:DeleteTstruct();']").removeAttr("onclick").attr("onclick", _iremoveclick);
            }
            if ((transid == "axvar" || transid == "ad_db") && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=axvars");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
                if (transid == "ad_db") {
                    $("#btn18").attr("onclick", "javascript:GetAppDbVars();");
                    $("#btn18").attr("title", "Click here to get defined DB level variables.");
                }
            } else if (transid == "ctype" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=cdlist");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "axpub" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=publist");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "ad_lg" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=axlangs");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "job_s" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=jobtsk");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "axstc" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=dop_list");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "b_sql" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=csqlist");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "sect" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=hplist");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "apidg" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=exapidef");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "axeml" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=emaildef");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "a__td" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=ad___tbd");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "astcp" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=dop_list");callParentNew("closeFrame();","function");' title="Developer Options List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
                $(".toolbarRightMenu").append(`<a id="axdevListConfig" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=ivconfdt");callParentNew("closeFrame();","function");' title="List" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">book</span></a>`);
            }
            else if (transid == "ad__q" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                let _iframes = parent.document.getElementsByTagName("iframe");
                if (typeof _iframes != "undefined" && _iframes[0].id == "axpiframeac")
                    $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=ad__qls");callParentNew("closeFrame();","function");' title="Queue listing" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
                else
                    $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.parent.LoadIframeac("ivtoivload.aspx?ivname=ad__qls");callParentNew("closeFrame();","function");' title="Queue listing" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            }
            else if (transid == "a__qm" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                let _iframes = parent.document.getElementsByTagName("iframe");
                if (typeof _iframes != "undefined" && _iframes[0].id == "axpiframeac")
                    $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=ad__qls");callParentNew("closeFrame();","function");' title="Queue listing" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
                else
                    $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.parent.LoadIframeac("ivtoivload.aspx?ivname=ad__qls");callParentNew("closeFrame();","function");' title="Queue listing" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            } else if (transid == "a__iq" && $(".toolbarRightMenu").find("a#axdevList").length == 0) {
                let _iframes = parent.document.getElementsByTagName("iframe");
                if (typeof _iframes != "undefined" && _iframes[0].id == "axpiframeac")
                    $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.LoadIframeac("ivtoivload.aspx?ivname=ad__qls");callParentNew("closeFrame();","function");' title="Queue listing" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
                else
                    $(".toolbarRightMenu").append(`<a id="axdevList" onclick='callParentNew("loadFrame();","function");parent.parent.LoadIframeac("ivtoivload.aspx?ivname=ad__qls");callParentNew("closeFrame();","function");' title="Queue listing" href="javascript:void(0)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 btn-sm"><span class="material-icons material-icons-style material-icons-2">view_list</span></a>`);
            }

            $(".toolbarRightMenu").css({ "padding-right": "45px" });

            AxDevStudioHelper(transid);
        }

        if (typeof transid != "undefined" && transid == "ad_af") {
            $("#ftbtn_iSave").removeClass("d-none");
            $('.toolbarRightMenu a[onclick^="javascript:CallAction(\'usave\'"]').addClass("d-none");
            //if (typeof recordid != "undefined" && (recordid == "" || recordid == "0"))
            $('.toolbarRightMenu').addClass("d-none");
            $('.toolbarRightMenu #design').addClass("d-none");
            $('.toolbarRightMenu').addClass("me-10");
            $("#ftbtn_iNew").addClass("d-none");
            $("#ftbtn_iDiscard").addClass("d-none");
            $("#ftbtn_iSave").removeAttr("onclick");
            $("#ftbtn_iSave").attr("onclick", "saveAddFieldRuntime()");
            if (typeof recordid != "undefined" && recordid != "" && recordid != "0") {
                $("#ftbtn_iRemove").removeClass("d-none");
                $("#ftbtn_iRemove").removeAttr("onclick");
                $("#ftbtn_iRemove").attr("onclick", "removeAddFieldRuntime()");
            }
        }

        if (typeof transid != "undefined" && transid == "ad_pr") {
            $("#btn18").removeAttr("onclick");
            $("#btn18").attr("onclick", "AlertNoAction();");
        }
    } catch (ex) { }
}

function axdevSturioCustomValidationFn(textarea) {
    var jsonData = "";
    var webServiceName = "";
    var textSqlandExpression = textarea.value;
    if (textSqlandExpression.length <= 0) {
        return;
    }
    if (textarea.id.toLocaleLowerCase().startsWith("sql_editor") || textarea.id.toLocaleLowerCase().startsWith("exp_editor") || GetDWBFieldType(GetFieldsName(textarea.id)) == "SQL Editor" || GetDWBFieldType(GetFieldsName(textarea.id)) == "Expression Editor") {
        var sourcefield = "", sourcetable = "", Axp_Web_SqlExp_Val_Def_RestRad = "";
        var calfrom = "", fgname = "";
        var fieldname = "", fldordno = "", datatype = "", fieldType = "", sfrom = "", expression = "", validateExpression = "", transid = "", savenorm = "", refresh = "", autoselect = "", combobox = "", sql = "", iviewparams = "", pmetadata = "", sourcefieldid = "", sourcetableid = "";
        var dcNo = "";

        fieldType = GetFieldValue(GetComponentName("type", GetDcNo("type"))) || "";
        fieldname = GetFieldValue(GetComponentName("name", GetDcNo("name"))) || "";
        fldordno = GetFieldValue(GetComponentName("fldordno", GetDcNo("fldordno"))) || "";
        datatype = GetFieldValue(GetComponentName("datatype", GetDcNo("datatype"))) || "";

        if (fieldType.toLowerCase() === "field") {
            sfrom = GetFieldValue(GetComponentName("modeofentry", GetDcNo("modeofentry"))) || "";
        } else if (fieldType.toLowerCase() === "fill grid") {
            sfrom = "fillgrid";
        }
        transid = GetFieldValue(GetComponentName("stransid", GetDcNo("stransid"))) || callParentNew("transid");
        expression = GetFieldValue(GetComponentName("exp_editor_expression", GetDcNo("exp_editor_expression"))) || "";
        validateExpression = GetFieldValue(GetComponentName("exp_editor_validateexpression", GetDcNo("exp_editor_validateexpression"))) || "";


        sfrom = sfrom.toLowerCase();
        if (sfrom === "accept") {
            sql = GetFieldValue(GetComponentName("sql_editor_detail", GetDcNo("sql_editor_detail"))) || "";
        }
        else if (sql == "") {
            sql = textSqlandExpression;
        }
        if (transid == "axvar" || transid == "axeml")
            Axp_Web_SqlExp_Val_Def_RestRad = "online";
        else
            Axp_Web_SqlExp_Val_Def_RestRad = GetFieldValue(GetComponentName("Axp_Web_SqlExp_Val_Def_RestRad", GetDcNo("Axp_Web_SqlExp_Val_Def_RestRad"))) || "";

        let paramsForSQL = "";

        if (textarea.id.toLocaleLowerCase().startsWith("sql_editor") || GetDWBFieldType(GetFieldsName(textarea.id)) == "SQL Editor") {
            if (transid == "b_sql") {
                sfrom = "iview";

                if (transid == "b_sql") {
                    paramsForSQL = $("#sqlparams000F1").val() || "";
                } else {
                    paramsForSQL = $("#Def_Table_params000F1").val() || $("#Def_Table_paramsmain000F1").val() || "";// "puser~Character~admin,abcd~Character~";
                }

                if (paramsForSQL) {
                    paramsForSQL = `{${paramsForSQL.split(',').map(par => par.split('~').map((pars, ind, allPars) => {
                        if (ind == 0) {
                            return allPars.length == 1 ? `"${pars}":""` : `"${pars}":`
                        }
                        else if (ind == 1) {
                            return `"${(pc = pars[0]?.toLowerCase()) ? `${pc}~` : ""}`
                        }
                        else if (ind == 2) {
                            return `${pars}"`;
                        }
                    }).join("")).join(",")}}`;
                }
            } 

            jsonData = {
                "_parameters": [{
                    "tstructs": {
                        "axpapp": callParentNew('mainProject'),
                        "sessionid": callParentNew('mainSessionId'),
                        "transid": transid,
                        "fieldname": fieldname,
                        "iviewparams": iviewparams,
                        "fldordno": fldordno,
                        "datatype": datatype,
                        "sfrom": sfrom,
                        "expression": "",
                        "validateexpression": "",
                        "savenorm": savenorm,
                        "refresh": refresh,
                        "autoselect": autoselect,
                        "combobox": combobox,
                        "sql": sql,
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
        }
        ShowDimmer(true);

        if (actionCallFlag == actionCallbackFlag) {
            actionCallFlag = Math.random();
            $("#icons").css({ "pointer-events": "auto" });
        } else {
            $("#icons").css({ "pointer-events": "none" });
            return;
        }
        try {
            if ((typeof $(blurNextPreventElement).attr("id") != "undefined" && $(blurNextPreventElement).attr("id") == "tstlinkSqlConsole" && $(blurNextPreventElement).parent().hasClass("tstbtnSqlConsole")) || (typeof blurNextPreventId != "undefined" && blurNextPreventId == "tstlinkSqlConsole"))
                callParentNew("beforeRestCallId=", "tstbtnSqlConsole");
            callParentNew("sqlRestCallInitiated=", true);
            ASB.WebService.CallRestWS(`${JSON.stringify(jsonData)}${paramsForSQL ? `*$*"params":${paramsForSQL}` : ""}`, webServiceName, SuccessCallbackAction, OnException);
        }
        catch (exp) {
            actionCallbackFlag = actionCallFlag;
            $("#icons,#btnSaveTst,.wizardNextPrevWrapper").css({ "pointer-events": "auto" });
        }


        function SuccessCallbackAction(result, eventArgs) {
            callParentNew("sqlRestCallInitiated=", false);
            actionCallbackFlag = actionCallFlag;
            $("#icons,#btnSaveTst,.wizardNextPrevWrapper").css({ "pointer-events": "auto" });
            ShowDimmer(false);
            if (transid == "b_sql")
                switchOffCacheData('');
            try {
                var json = $.parseJSON(result);
                var msg = json["result"][0].msg;
                var autofillsql = "", fldid = "";
                var status = json["result"][0].status;

                if (status.toLowerCase() == "failed") {
                    callParentNew("beforeRestCallId=", "");
                    msg = msg.replace('<error>', '').replace('</error>', '');
                    showAlertDialog("warning", msg);
                }
            } catch (ex) { }

        }

        function OnException(result) {
            actionCallbackFlag = actionCallFlag;
            $("#icons,#btnSaveTst,.wizardNextPrevWrapper").css({ "pointer-events": "auto" });
            ShowDimmer(false)
        }
    }
}

function switchOffCacheData(isShowMessage) {
    try {
        let _sqlText = $("#sqltext000F1").val();
        if (typeof _sqlText != "undefined" && _sqlText != "" && _sqlText.toLowerCase().indexOf("--ax_") > -1 && $("#cachedata000F1").prop('checked')) {
            $("#cachedata000F1").prop('checked', false);
            $("#cacheinterval000F1").parents('.grid-stack-item').addClass('d-none')
            SetFieldValue("cachedata000F1", "F");
            UpdateFieldArray("cachedata000F1", "000", "F", "parent", "");
            if (isShowMessage == "true") {
                showAlertDialog("info", "Data can not be cache for dynamic SQL's");
            }
        }
    } catch (ex) { }
}

function AxDevStudioHelper(_transid) {
    try {
        if (_transid == "sect") {
            $("#wBdr").addClass("htmlPagesCls");
            $("#wBdr").css({ "height": "85vh", "overflow": "auto" });
            ToggleWizardDc("3", "hide");
            if ($(".tstructMainBottomFooter .BottomToolbarBar").find("[onclick='previewHtmlPage();']").length == 0) {
                var customButtons = `
        <button onclick="previewHtmlPage();" class="btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm" type="button" id="previewHtmlPageId" title="Preview"><span class="material-icons">preview</span><span class="hpCustBtnsText">Preview</span></button>
        
        <button onclick="addCssJs(this.id);" class="btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm" type="button" id="addCssRow" title="Add Css"><span class="material-icons">code</span><span class="hpCustBtnsText">Add Css</span></button>

        <button onclick="addCssJs(this.id);" class="btn btn-white btn-color-gray-700 btn-active-primary btn-sm d-inline-flex align-items-center shadow-sm me-2 dwbIvBtnbtm" type="button" id="addJsRow" title="Add Js"><span class="material-icons">code</span><span class="hpCustBtnsText">Add Js</span></button>
        `;
                $(".tstructMainBottomFooter .BottomToolbarBar").prepend(customButtons);
            }
            let HPImagePath = $("#hdnAxpertWebDirPath").val() + proj + "\\HTMLPages\\images\\*";
            SetFieldValue("AxpFilePath_hpImages000F1", HPImagePath);
            UpdateFieldArray("AxpFilePath_hpImages000F1", "000", HPImagePath, "parent", "");

            if (recordid != "0") {
                pageNo = GetFieldValue("pageno000F1");
                loadcontentsFromFile();
                $(`#${htmlObj.fields.templateBtn}`).prop("disabled", true);
            }
            else if (recordid == "0") {
                pageNo = Date.now().toString();
                SetFieldValue("pageno000F1", pageNo);
                UpdateFieldArray("pageno000F1", GetFieldsRowNo("pageno000F1"), pageNo, "parent");
                $(`#${htmlObj.fields.templateBtn}`).removeAttr("onclick").attr("onclick", "selectTemplate()");                
            }
            htmlObj.template.name = GetFieldValue(`${htmlObj.fields.template}`);
            if (htmlObj.template.name != "" && !+recordid) {
                htmlObj.template.flag = true;
                SetFieldValue(`${htmlObj.fields.template}`, htmlObj.template.name);
                UpdateFieldArray(`${htmlObj.fields.template}`, GetFieldsRowNo(`${htmlObj.fields.template}`), htmlObj.template.name, "parent");
                /*$("#DivFrame3").hide();*/
                $("#DivFrame3").show();
                $("#DivFrame3").removeClass('d-none');
                $("#DivFrame3 .gridIconBtns").addClass('d-none');
                loadcontentsFromFile();
                htmlCodeMirror.getDoc().setValue($(`#${htmlObj.fields.htmlCm}`).val());
                htmlObj.template.files.html = htmlObj.template.files.html == "" ? $(`#${htmlObj.fields.htmlCm}`).val() : htmlObj.template.files.html;
            } else if (recordid == "0" || (recordid != "0" && DCHasDataRows[2].toLowerCase() == "false")) {
                $("#DivFrame3").hide();
                $("#DivFrame3").addClass('d-none');
            }
            else {
                $("#DivFrame3").show();
                $("#DivFrame3").removeClass('d-none');
                $("#DivFrame3 .gridIconBtns").addClass('d-none');
            }
        }


        if (_transid == 'b_sql') {
            $("#sqlparams000F1").addClass("fldCustTable");
            var dfName = GetFieldsName("sqlparams000F1");
            var dfldIndex = $j.inArray(dfName, FNames);
            var tjson = "{\"props\":{\"type\":\"table\",\"colcount\":\"3\",\"rowcount\":\"1\",\"addrow\":\"f\",\"deleterow\":\"f\",\"valueseparator\":\"~\",\"rowseparator\":\",\"},\"columns\":{\"1\":{\"caption\":\"Parameter Name\",\"name\":\"paraname\",\"value\":\"\",\"source\":\"\",\"exp\":\"\",\"vexp\":\"\"},\"2\":{\"caption\":\"Data Type\",\"name\":\"datatypefld\",\"value\":\"\",\"source\":\"ddldatatype\",\"exp\":\"\",\"vexp\":\"\"},\"3\":{\"caption\":\"Value\",\"name\":\"valuefld\",\"value\":\"\",\"source\":\"\",\"exp\":\"\",\"vexp\":\"\"}}}";
            FTableTypeVal[dfldIndex] = tjson;
            $("#sqltext000F1").on("focusout", function (event) {
                var sqlParStr = "";
                try {
                    if ($(this).val() != "" && ($(this).val().replace(/'(.*?)'/g, "''").match(/(:)+[a-zA-Z0-9_]{1,}/g) || []).filter(v => v.indexOf("::") != 0).length > 0) {
                        sqlParStr = _.uniqBy($(this).val().replace(/'(.*?)'/g, "''").match(/(:)+[a-zA-Z0-9_]{1,}/g).filter(v => v.indexOf("::") != 0).map((val) => val.replace(/[:\s]/g, ""))).join(",");
                    }
                    if ($(this).val() != "" && ($(this).val().replace(/'(.*?)'/g, "''").match(/(\{(?:\[??[^\[]*?\}))*/g) || []).filter(v => v).length > 0) {
                        sqlParStr = (sqlParStr ? (sqlParStr + ",") : "") + _.uniqBy($(this).val().replace(/'(.*?)'/g, "''").match(/(\{(?:\[??[^\[]*?\}))*/g).map((val) => val.replace(/[{\s]/g, "").replace(/[}\s]/g, ""))).filter(v => v).join(",");
                    }
                    sqlParStr = sqlParStr.split(",").map(par => par && `${par}`).join(",");
                } catch (error) {

                }

                if (sqlParStr == "") {
                    SetFieldValue("sqlparams000F1", sqlParStr);
                    UpdateFieldArray("sqlparams000F1", GetFieldsRowNo("sqlparams000F1"), sqlParStr, "parent");
                }
                if ($("#paramcal000F1").val() != sqlParStr) {
                    SetFieldValue("paramcal000F1", sqlParStr);
                    UpdateFieldArray("paramcal000F1", GetFieldsRowNo("paramcal000F1"), sqlParStr, "parent");
                    SetFieldValue("sqlparams000F1", sqlParStr);
                    UpdateFieldArray("sqlparams000F1", GetFieldsRowNo("sqlparams000F1"), sqlParStr, "parent");

                    if (sqlParStr != "") {
                        if (parent.$("#actbtn_nextclk.dwbIvBtnbtm:visible").length > 0 || parent.$("#btn_nxclkDuplicate.dwbIvBtnbtm:visible").length > 0) {
                            parent.$("#actbtn_nextclk.dwbIvBtnbtm").hide();
                            $(callParentNew("btn_nxclkDuplicate")).show();
                        }

                        FieldTypeTable(event, $("#sqlparams000F1"));
                    } else {
                        customValidationFn($("#sqltext000F1")[0]);
                    }
                } else {
                    customValidationFn($("#sqltext000F1")[0]);
                }
            });
        }

        if (_transid == "ad_lg") {
            $("#btn24").addClass("d-none");
            $("#btn25").addClass("d-none");
            //$("#btn22").removeAttr('onclick').attr("onclick", "btn22langDownload()");
            $("#btn23").removeAttr('onclick').attr("onclick", "btn23langUpload()");
            $('[onclick="FormSubmit()"]').addClass("d-none");
        }

        if (_transid == "apidg") {
            $("#btn17").attr("onclick", "javascript:ExecuteOnClick()");
            $("#APIAxpertSample000F1").val("{\"data\":[{\"i\":\"admin\",\"v\":\"\"},{\"i\":\"User1\",\"v\":\"\"},{\"i\":\"User2\",\"v\":\"\"}]}");
            $("#APIAxpertSample000F1").attr('readonly', true);
            GetInterExterResources();
        }
    } catch (ex) { }
}

/* save redirection for htmlpages and hyperlinks in iview builder */
function htmlCustomSaveRedirect() {
    if (transid == "sect") {
        var htmlContent = htmlCodeMirror.getValue();
        htmlContent = htmlContent.replace(/&/g, '&amp;');
        var domParser = new DOMParser();
        var dom = "";
        if (htmlContent.trim() != "") {
            dom = domParser.parseFromString(htmlContent, 'text/html');
        }
        var cssContents = [], jsContents = [], isNewPage = true;
        var headTag = dom.getElementsByTagName('head')[0];
        var bodyTag = dom.getElementsByTagName('body')[0];
        let HtmlDirPath = "../../" + callParentNew("mainProject") + "/HTMLPages/";
        $(".formGridRow").each(function () {
            var fileExt = $(this).find("[id^='filetype']").val().toLowerCase();
            var fileName = $(this).find("[id^='filename']").val().replace(/ /g, "_") + "_" + pageNo + "." + fileExt;
            var content = $(this).find("textarea[id^='css_js_src']").val();
            if (fileExt == "css") {
                cssFileName.push(fileName);
                cssContents.push(content);

                $(headTag).find("link[href*='/" + fileName + "']").remove();

                var linkTag = dom.createElement("link");
                linkTag.setAttribute("type", "text/css");
                linkTag.setAttribute("rel", "stylesheet");
                linkTag.setAttribute("href", HtmlDirPath + "Css/" + fileName + "?v=" + (new Date()).getTime());
                headTag.append("\t");
                headTag.appendChild(linkTag);
                headTag.append("\n");
            }
            else if (fileExt == "js") {
                jsFileName.push(fileName);
                jsContents.push(content);

                $(bodyTag).find("script[src*='/" + fileName + "']").remove();

                var scriptTag = dom.createElement("script");
                scriptTag.setAttribute("type", "text/javascript");
                scriptTag.setAttribute("src", HtmlDirPath + "Js/" + fileName + "?v=" + (new Date()).getTime());
                bodyTag.append("\t");
                bodyTag.appendChild(scriptTag);
                bodyTag.append("\n");
            }
        });

        htmlContent = dom.documentElement.outerHTML;
        pageCaption = GetFieldValue("caption000F1");
        var addToMenu = GetFieldValue("isacoretrans000F1") == "Yes" ? true : false;
        if (recordid != "0") {
            isNewPage = false;
        }
        try {
            $.ajax({
                type: "POST",
                url: "tstruct.aspx/htmlPagePublish",
                data: JSON.stringify({
                    htmlContent, cssFileName, cssContents, jsFileName, jsContents, pageCaption, addToMenu, pageNo, isNewPage
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "done") {
                        cssFileName = [], cssContents = [], jsFileName = [], jsContents = [];
                        SetFormDirty(false);
                    }
                    else {
                        ShowDialog("error", response.d);
                        SetFormDirty(false);
                    }
                },
                error: function (error) {
                    ShowDialog("error", "Error occurred while saving..!");
                }
            });
        }
        catch (e) {
            ShowDialog("error", "Exception occurred while saving");
        }
    }
}

/* delete redirection for htmlpages and hyperlinks in iview builder */
function htmlCustomDeleteRedirect() {
    if (transid == "sect") {
        if (pageNo != "") {
            try {
                $.ajax({
                    type: "POST",
                    url: "tstruct.aspx/removeFromMenuAndFolder",
                    data: JSON.stringify({
                        pageNo
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "done") {
                            window.location.href = "ivtoivload.aspx?ivname=hplist";
                        }
                        else {
                            ShowDialog("error", response.d);
                        }
                    },
                    error: function (error) {
                        ShowDialog("error", "Error while removing the file..!");
                    }
                });
            }
            catch (ex) {
                ShowDialog("error", "Error while removing the file..! " + ex);
            }
        }
        else {
            ShowDialog("error", "page number not found");
        }
    }
}

function btn22langDownload() {
    if (transid == "ad_lg") {
        if ($("#exportto000F1").val() != "" && $("#language000F1").val() != "") {
            let srptPath = $("#exportto000F1").val();
            let axLang = $("#language000F1").val();
            srptPath += "\\" + axLang;

            try {
                var curWin = window.open('OpenFiles.aspx?alPath=' + srptPath, '_self', 'width=400,height=300,scrollbars=yes,resizable=yes');
            } catch (ex) {
                showAlertDialog("warning", 'This file does not exist.');
            }
        }
    }
}
var isAxLangUploaded = "false";
function btn23langUpload() {
    if (transid == "ad_lg") {
        if ($("#exportto000F1").val() != "" && $("#language000F1").val() != "" && $("#uploadfilename000F3").val() != "") {
            let srptPath = $("#exportto000F1").val();
            let axLang = $("#language000F1").val();
            srptPath += "\\" + axLang;

            let upFileType = $("#uploadfilename000F3").val();
            isAxLangUploaded = "false";
            var src = "./axlangupload.aspx?fpath=" + srptPath + "&upfiletype=" + upFileType;
            displayBootstrapModalDialog("Axpert Language", "lg", "200px", true, src, "", "", CallbackImportFunction);
        }
    }

    function CallbackImportFunction() {
        try {
            if (isAxLangUploaded == "true") {
                let upFileType = $("#uploadfilename000F3").val();
                if (upFileType != "") {
                    upFileType = upFileType + ".xlsx";
                    //upFileType = upFileType + ".xls";
                    $("#uploadedfile000F3").val(upFileType);
                    UpdateFieldArray("uploadedfile000F3", GetFieldsRowNo("uploadedfile000F3"), upFileType, "parent");

                    $("#btn25").click();
                }
            }

        } catch (ex) { }
    }
}

function ExecuteOnClick() {
    let apimethod = $("#ExecAPIMethod000F1").val();
    if (apimethod == "") {
        ShowDialog("error", "Select API Method.");
        return;
    }
    let apiresp = $("#APIResponseFormat000F1").val();
    let apivalue = $("#ExecAPIURL000F1").val();
    if (apivalue == "") {
        ShowDialog("error", "API URL should not empty.");
        return;
    }

    ASB.WebService.GetDataFromExternalAPI(apivalue, apimethod, apiresp, SuccessGetExternalApi, OnException);

    //APIResponseString000F1
}

function SuccessGetExternalApi(result, eventArgs) {
    if (result != "") {
        $("#APIResponseString000F1").val(result);
        $("#APIResponseString000F1").attr('readonly', true);
    }
}

function saveAddFieldRuntime() {
    if (!ValidateBeforeSubmit()) {
        return;
    } else {
        const currentUrl = window.location.href;
        const urlParams = new URL(currentUrl);
        const _newform = urlParams.searchParams.get("newform");
        const _formtype = urlParams.searchParams.get("formtype");
        if (typeof addFieldReloaduri != "undefined")
            addFieldReloaduri = "";
        let _afdcName = '';
        let _afTransid = '';
        let _afFormCap = '';
        let _fieldJson = '{';
        let _addfieldExcludeList = ["axp_recid1", "disableform", "formcaption", "stransid", "formtype", "newform", "flddupchk", "fldctl", "rangeperiod", "autogenprefix", "autogenprefixfld", "autogensno", "autogenno", "acceptfld", "selectfld", "hidden", "masterdl", "sourcedl", "fType", "DSFormCaptionui", "DSFieldui", "defSrcFieldui"];
        try {
            AllFieldNames.forEach(function (_ele, _ind) {
                let _thisFName = GetFieldsName(_ele);
                if (typeof _thisFName != "undefined" && _thisFName != "" && typeof AllFieldValues[_ind] != "undefined" && AllFieldValues[_ind] != "") {
                    if (_addfieldExcludeList.indexOf(_thisFName) == -1) {
                        let _thisFVal = AllFieldValues[_ind];
                        if (_thisFName == "exp_editor_formula")
                            _thisFName = "expression";
                        if (_thisFName == "exp_editor_valexpr")
                            _thisFName = "validateExp";
                        if (_thisFName == "flddecimal")
                            _thisFName = "decimal";
                        if (_thisFName == "defTable" && _thisFVal != "") {
                            _fieldJson += "\"" + _thisFName + "\":" + JSON.stringify(_thisFVal) + ",";
                        } else
                            _fieldJson += "\"" + _thisFName + "\":\"" + _thisFVal + "\",";
                        if (_thisFName == "dcName") {
                            _afdcName = _thisFVal;
                        }
                    } else if (_thisFName == "stransid") {
                        _afTransid = AllFieldValues[_ind];
                    } else if (_thisFName == "formcaption") {
                        _afFormCap = AllFieldValues[_ind];
                        _afFormCap = _afFormCap.split('(')[0];
                    }
                }
            });
            if (_fieldJson.endsWith(',')) {
                let _allowEmpty = GetFieldValueNew("allowEmpty000F1");
                _fieldJson += "\"allowEmpty\":\"" + _allowEmpty + "\",";

                let _allowDuplicate = GetFieldValueNew("allowDuplicate000F1")
                _fieldJson += "\"allowDuplicate\":\"" + _allowDuplicate + "\",";

                let _fldPosition = GetFieldValueNew("fldPosition000F1")
                _fieldJson += "\"fldPosition\":\"" + _fldPosition + "\",";

                _fieldJson = _fieldJson.slice(0, -1);
            }
        } catch (ex) { }
        _fieldJson += "}";
        if (_fieldJson == "{}") {
            showAlertDialog("error", "Field JSON is not correct");
            return;
        }
        let _formInfo = '';
        if (_newform == "T")
            _formInfo = "addform~" + _afTransid + "~" + _afFormCap + "~" + _formtype;
        let _fldMode = "new";
        if (typeof recordid != "undefined" && recordid != "" && recordid != "0")
            _fldMode = "edit";

        if (callParentNew("addFormRuntimeDcFlag") != "" && _afdcName != "dc1") {
            alert("Header(dc1) should have at least one field to save the Form.");
            return;
        }
        try {
            $.ajax({
                type: "POST",
                url: "../WebService.asmx/AddFieldRuntimeWS",
                data: JSON.stringify({ transid: _afTransid, formCap: _afFormCap, fieldJson: _fieldJson, fldMode: _fldMode, newform: _newform, formInfo: _formInfo }),
                cache: false,
                async: false,
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        if (data.d.startsWith("addform:")) {
                            data.d = data.d.replace("addform:", "");
                            let addFormJson = JSON.parse(data.d);
                            callParentNew("addFormRuntimeDcFlag=", '');
                            showAlertDialog("error", "Duplicate Form name. Cannot be saved.");
                            getAxpertStudioAddFormData(_formInfo);

                        } else {
                            let addFieldJson = JSON.parse(data.d);
                            if (addFieldJson.success) {
                                addFieldReloaduri = callParentNew("addFormRuntimeDcFlag");
                                if (_afdcName == "dc1")
                                    callParentNew("addFormRuntimeDcFlag=", '');
                                clearLShtmlkeyAddField(_afTransid);

                                FormSubmit();                                
                            } else {
                                showAlertDialog("error", addFieldJson.message);
                            }
                        }
                    } else {
                        showAlertDialog("error", "Error while creating field, please try again.");
                    }
                },
                error: function (error) {
                    showAlertDialog("error", "Error while creating field, please try again.");
                }
            });
        } catch (ex) { }
    }
}

function clearLShtmlkeyAddField(addFldTid) {
    try {
        let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        let _thisKey = getKeysWithPrefixTst("tstHtml♠" + addFldTid + "-" + appSUrl + "♥");
        _TstlocalStorage = localStorage[_thisKey[0]];
        if (typeof _TstlocalStorage == "undefined") {
            _TstlocalStorage = "";
        }

        if (_TstlocalStorage != '') {
            for (const val of _thisKey) {
                localStorage.removeItem(val);
            }
        }
    } catch (ex) { }
}

function getKeysWithPrefixTst(prefix) {
    var allKeys = Object.keys(localStorage);
    var matchingKeys = allKeys.filter(function (key) {
        return key.startsWith(prefix);
    });
    return matchingKeys;
}

function removeAddFieldRuntime() {
    try {
        var rid = "0";
        if ($j("#recordid000F0").val() != "0") {
            rid = $j("#recordid000F0").val();
        } else {
            rid = $j("#recordid000F0").val();
        }
        if (rid == "0") {
            showAlertDialog("warning", 2009, "client");
        } else {

            var cutMsg = eval(callParent('lcm[5]'));
            var glType = eval(callParent('gllangType'));
            var isRTL = false;
            if (glType == "ar")
                isRTL = true;
            else
                isRTL = false;
            var DeleteTstructCB = $.confirm({
                theme: 'modern',
                closeIcon: false,
                title: eval(callParent('lcm[155]')),
                onContentReady: function () {
                    disableBackDrop('bind');
                },
                backgroundDismiss: true,
                escapeKey: 'buttonB',
                rtl: isRTL,
                content: cutMsg,
                buttons: {
                    buttonA: {
                        text: eval(callParent('lcm[164]')),
                        btnClass: 'btn btn-primary',
                        action: function () {
                            DeleteTstructCB.close();

                            let _stransid = GetFieldValueNew("stransid000F1");
                            let _fldName = GetFieldValueNew("name000F1");
                            let _dcname = GetFieldValueNew("dcName000F1");
                            $.ajax({
                                type: "POST",
                                url: "../WebService.asmx/DeleteFieldRuntimeWS",
                                data: JSON.stringify({ transid: _stransid, fieldname: _fldName, dcname: _dcname }),
                                cache: false,
                                async: false,
                                contentType: "application/json;charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    if (data.d != "") {
                                        let delFieldJson = JSON.parse(data.d);
                                        if (delFieldJson.success) {
                                            var txt = '';
                                            trace = traceSplitStr + "DeleteData-" + _stransid + traceSplitChar;
                                            txt = txt + '<Transaction axpapp="' + proj + '" transid="ad_af" recordid="' + rid + '" action="delete" trace="' + trace + '" sessionid="' + sid + '" allowCancel="' + AxAllowCancel + '"> ';
                                            try {
                                                ASB.WebService.DeleteDataXML(rid, txt, tstDataId, AxAllowCancel, resTstHtmlLS, SucceededCallbackDelTst);
                                            } catch (exp) {
                                                AxWaitCursor(false);
                                                var execMess = exp.name + "^♠^" + exp.message;
                                                showAlertDialog("error", 2030, "client", execMess);
                                            }
                                        }
                                        else
                                            showAlertDialog("error", delFieldJson.message);
                                    } else
                                        showAlertDialog("error", "Error while deleting field, please try again.");
                                }, error: function (error) {
                                    showAlertDialog("error", "Error while deleting field, please try again.");
                                }
                            });
                        }
                    },
                    buttonB: {
                        text: eval(callParent('lcm[192]')),
                        btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                        action: function () {
                            disableBackDrop('destroy');
                        }
                    },
                }
            });
        }
    } catch (ex) {

    }
}


function GetAppDbVars() {
    try {
        AxWaitCursor(true);
        ShowDimmer(true);
        var fname = $("#db_function000F1").val();
        var fparams = $("#db_function_params000F1").val();
        if (fname == "") {
            showAlertDialog("error", "Please enter DB function name.");
            AxWaitCursor(false);
            ShowDimmer(false);
            return;
        }
        $.ajax({
            url: 'tstruct.aspx/GetFromDBJSON',
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                funName: fname, funParams: fparams
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                if (data.d != "") {
                    var resJson = data.d;
                    if (!resJson.startsWith("error:")) {
                        var paramJson = JSON.parse(resJson.replace(/\\r\\n/g, ""));
                        //var paramJson = JSON.parse(resJson);
                        paramJson = paramJson[0];
                        var indNo = 1
                        try {
                            $.each(paramJson, function (key, value) {
                                var dataType = key.charAt(key.length - 1);
                                var pname = key.substr(0, key.length - 1);
                                var pvalue = value == null ? "" : value;
                                if (gridDummyRowVal.length > 0) {
                                    gridDummyRowVal.map(function (v) {
                                        if (v.split("~")[0] == 2)
                                            gridDummyRowVal.splice($.inArray(v, gridDummyRowVal), 1);
                                    });
                                }
                                let rowNo = "";
                                if (indNo.toString().length == 1)
                                    rowNo = "00" + indNo;
                                else if (indNo.toString().length == 2)
                                    rowNo = "0" + indNo;
                                else
                                    rowNo = indNo;
                                if (recordid != "0" && indNo == 1) {
                                    var fDcRowCount = GetDcRowCount("2");
                                    if (fDcRowCount > 0)
                                        DeleteAllRows("2", fDcRowCount);
                                    editTheRow('', 2, '', event);
                                }
                                if (indNo != 1)
                                    editTheRow('', 2, '', event);

                                $("#db_varname" + rowNo + "F2").val(pname);
                                UpdateFieldArray("db_varname" + rowNo + "F2", rowNo, pname, "parent");
                                $("#db_varval" + rowNo + "F2").val(pvalue);
                                UpdateFieldArray("db_varval" + rowNo + "F2", rowNo, pvalue, "parent");
                                $("#db_vartype" + rowNo + "F2").val(dataType);
                                UpdateFieldArray("db_vartype" + rowNo + "F2", rowNo, dataType, "parent");
                                UpdateFieldArray(axpIsRowValid + "2" + rowNo + "F2", GetDbRowNo(rowNo, "2"), "", "parent", "AddRow");

                                indNo++;
                            });
                        } catch (ex) { }
                    } else {
                        showAlertDialog("error", resJson.replace("error:", ""));
                    }
                } else {
                    showAlertDialog('error', 1031, "client");
                }
                AxWaitCursor(false);
                ShowDimmer(false);
            },
            error: function (error) {
                showAlertDialog('error', 1031, "client");
                AxWaitCursor(false);
                ShowDimmer(false);
            }
        });
    } catch (exp) {
        AxWaitCursor(false);
        ShowDimmer(false);
    }
}

function SaveMultiFrameDataJSON() {
    let resSuccess = false;
    if (!ValidateBeforeSubmit()) {
        ShowDimmer(false);
        AxWaitCursor(false);
        return false;
    }
    let fldJSON = "\"recdata\":[";
    let changedDcs = "";
    DCName.forEach(function (ele) {
        let _thisDcNo = ele.substring(2);
        let _thisDcFld = GetGridFields(_thisDcNo);
        if (!IsDcGrid(_thisDcNo)) {
            let _RecVal = $("#axp_recid" + _thisDcNo + "000F" + _thisDcNo).val();
            fldJSON += "{\"axp_recid" + _thisDcNo + "\":[{\"rowno\":\"001\",\"text\":\"" + _RecVal + "\",\"columns\":{";
            let fldValJson = "";
            _thisDcFld.forEach(function (flele) {
                if (flele.startsWith('axp_recid'))
                    return;
                let _thisFld = flele + "000F" + _thisDcNo;
                let _thisVal = GetFieldValue(_thisFld);
                _thisVal = _thisVal.replace(/\\/g, '\\\\');
                fldValJson += "\"" + flele + "\":\"" + _thisVal + "\",";
            });
            if (fldValJson != "")
                fldJSON += fldValJson.substr(0, fldValJson.length - 1);
            fldJSON += "}}]},";
        } else {
            var isExitDummy = false;
            if (gridDummyRowVal.length > 0) {
                gridDummyRowVal.map(function (v) {
                    if (v.split("~")[0] == _thisDcNo) isExitDummy = true;
                });
            }
            if (!isExitDummy) {
                changedDcs += "\"dc" + _thisDcNo + "\":\"*\",";
                fldJSON += "{\"axp_recid" + _thisDcNo + "\":[";
                let _gridRows = false;
                $("#gridHd" + _thisDcNo + " tbody tr").each(function (rind, rele) {
                    _gridRows = true;
                    let _thisRNo = $(rele).attr("id");
                    _thisRNo = _thisRNo.substring(_thisRNo.indexOf('F'), _thisRNo.indexOf('F') - 3);
                    let _thisDbrow = GetDbRowNo(_thisRNo, _thisDcNo);
                    if (_thisDbrow.length == 1)
                        _thisDbrow = "00" + _thisDbrow;
                    else if (_thisDbrow.length == 2)
                        _thisDbrow = "0" + _thisDbrow;
                    let _RecVal = $("#axp_recid" + _thisDcNo + _thisRNo + "F" + _thisDcNo).val();
                    fldJSON += "{\"rowno\":\"" + _thisDbrow + "\",\"text\":\"" + _RecVal + "\",\"columns\":{";
                    let fldValJson = "";
                    _thisDcFld.forEach(function (flele) {
                        if (flele.startsWith('axp_recid'))
                            return;
                        let _thisFld = flele + _thisRNo + "F" + _thisDcNo;
                        let _thisVal = GetFieldValue(_thisFld);
                        _thisVal = _thisVal.replace(/\\/g, '\\\\');
                        fldValJson += "\"" + flele + "\":\"" + _thisVal + "\",";
                    });
                    if (fldValJson != "")
                        fldJSON += fldValJson.substr(0, fldValJson.length - 1);
                    fldJSON += "}},";
                });
                if (_gridRows == true)
                    fldJSON = fldJSON.substr(0, fldJSON.length - 1);
                fldJSON += "]},";
            }
        }
    });    

    let rid = 0;
    if ($j("#recordid000F0").length > 0)
        rid = $j("#recordid000F0").val();

    let files = UploadFiles();
    let delRows = GetDeletedRows();
    let chngRows = GetChangedRows();
    $.ajax({
        type: "POST",
        url: "../WebService.asmx/MultiFrameUpdateDataXML",
        cache: false,
        async: false,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({
            fldArray: ChangedFields, fldDbRowNo: ChangedFieldDbRowNo, fldValueArray: ChangedFieldValues,
            fldDeletedArray: DeletedDCRows, deletedFldArrayValues: DeletedFieldValue, files: files, rid: rid, delRows: delRows,
            changedRows: chngRows, key: tstDataId, resTstHtmlLS: resTstHtmlLS
        }),
        dataType: "json",
        success: function (data) {
            let resultJson = data.d;
            if (resultJson != "") {
                if (resultJson.includes("♠*♠")) {
                    tstDataId = resultJson.split("♠*♠")[0];
                    resultJson = resultJson.split("♠*♠")[1];
                }
            }
            if (resultJson === "true")
                resSuccess= true;
            else
                resSuccess= false;
        },
        error: function (error) {
            resSuccess= false;
        }
    });

    if (changedDcs.endsWith(',')) {
        changedDcs = changedDcs.slice(0, -1);
    }
    fldJSON = fldJSON.substr(0, fldJSON.length - 1);
    fldJSON += "],\"recordid\":\"" + rid + "\",\"changedDcs\":{" + changedDcs + "},\"delRows\":\"" + delRows + "\",\"files\":\"" + files + "\",\"tstDataId\":\"" + tstDataId + "\"";

    let formJson;
    try {
        formJson = JSON.parse("{" + fldJSON + "}");
    } catch (e) {
        return false;
    }
    let multiFormsJsonRaw = callParentNew("multiFormsJson");
    let multiFormsJson = {};
    if (multiFormsJsonRaw) {
        if (typeof multiFormsJsonRaw === "string") {
            try {
                multiFormsJson = JSON.parse(multiFormsJsonRaw);
            } catch (e) {
                multiFormsJson = {};
            }
        } else if (typeof multiFormsJsonRaw === "object") {
            multiFormsJson = multiFormsJsonRaw;
        }
    }
    multiFormsJson[transid] = formJson;
    callParentNew("multiFormsJson=", JSON.stringify(multiFormsJson));

    return resSuccess;
}
function startBulkActivation() {
    $.ajax({
        url: "../WebService.asmx/BulkUserActivation",
        method: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (typeof response.d != "undefined" && typeof response.d != "") {
                if (response.d.startsWith('Error:')) {
                    ShowDimmer(false);
                    let errorMsg = response.d.replace("Error:", "");
                    showAlertDialog('error', errorMsg);
                } else {
                    var jobId = response.d.replace('JobQueued:', '');
                    ShowDimmer(true);
                    checkJobStatus(jobId);
                }
            }
        }
    });
}
function checkJobStatus(jobId) {
    $.ajax({
        url: "../WebService.asmx/GetBulkActivationStatus",
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ jobId: jobId }),
        success: function (response) {
            var status = response.d;
            if (status.startsWith('Completed. ')) {
                ShowDimmer(false);
                let compMsg = status.replace("Completed. ", "");
                showAlertDialog('success', compMsg);
                setTimeout(function () {
                    parent.LoadIframeac("tstruct.aspx?transid=axurg&act=open");
                }, 300);
            } else if (status.startsWith('Error:')) {
                ShowDimmer(false);
                let errorMsg = status.replace("Error:", "");
                showAlertDialog('error', errorMsg);
            } else if (status == "Pending") {
                ShowDimmer(false);
                showAlertDialog('error', "There is an error, please check logs and try again.");
            }
            else {
                if (status.endsWith(' users activated.')) {
                    showAlertDialog('success', status);
                }
                setTimeout(() => checkJobStatus(jobId), 5000);
            }
        }
    });
}
$j(document).off("click", "#axurg .toolbarRightMenu .menu a[id='btn20']").on("click", "#axurg .toolbarRightMenu .menu a[id='btn20']", function (e) {
    if (typeof transid != "undefined" && transid == "axurg") {
        $.ajax({
            url: "../WebService.asmx/GetInActiveUserCount",
            method: 'POST',
            contentType: 'application/json',
            success: function (response) {
                if (response.d != "" && !response.d.startsWith('Error: ')) {
                    var count = parseInt(response.d, 10);
                    if (count > 0) {
                        let _mgsUser = "There are " + count + " inactive users. Do you want to activate?";
                        if (count == 1)
                            _mgsUser = "There is " + count + " inactive user. Do you want to activate?";
                        if (confirm(_mgsUser)) {
                            startBulkActivation();
                        } else {
                            //Donothing
                        }
                    } else {
                        showAlertDialog('info', "No inactive users found.");
                    }
                } else if (response.d.startsWith('Error: ')) {
                    let errorMsg = response.d.replace("Error: ", "");
                    showAlertDialog('error', errorMsg);
                }
            },
            error: function (xhr, status, error) {
            }
        });
    }
});

function GetInterExterResources() {
    try {
        $.ajax({
            url: 'tstruct.aspx/GetInterExterResources',
            type: 'POST',
            cache: false,
            async: true,
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                if (data != null && data.d != null) {
                    interExterResourcesData = JSON.parse(data.d);
                    let _existingUrl = $("#ExecAPIURL000F1").val();
                    let objextres = interExterResourcesData.resources.extres;
                    let objintres = interExterResourcesData.resources.intres;
                    let mergedData = Object.assign({}, objintres, objextres);
                    var _fld = $j("#config_url_name000F1");
                    _fld.empty();
                    _fld.append('<option value=""></option>');
                    Object.keys(mergedData).forEach(function (key) {
                        var value = mergedData[key];
                        if (_existingUrl != "" && _existingUrl == value)
                            _fld.append('<option value="' + value + '" selected="selected">' + key + '</option>');
                        else
                            _fld.append('<option value="' + value + '">' + key + '</option>');
                    });
                    _fld.select2();

                    //let apitype = $("#APItype000F1").val();
                    //if (apitype != null && (apitype == "General" || apitype == "Bind API to Dropdown field from Axpert")) {
                    //    var dataObj = interExterResourcesData.resources.intres;
                    //    var _fld = $j("#config_url_name000F1");
                    //    _fld.empty();
                    //    _fld.append('<option value=""></option>');
                    //    Object.keys(dataObj).forEach(function (key) {
                    //        var value = dataObj[key];
                    //        if (_existingUrl != "" && _existingUrl == value)
                    //            _fld.append('<option value="' + value + '" selected="selected">' + key + '</option>');
                    //        else
                    //            _fld.append('<option value="' + value + '">' + key + '</option>');
                    //    });
                    //    _fld.select2();

                    //} else if (apitype != null && apitype == "Bind API to Dropdown field from External") {
                    //    var dataObj = interExterResourcesData.resources.extres;
                    //    var _fld = $j("#config_url_name000F1");
                    //    _fld.empty();
                    //    _fld.append('<option value=""></option>');
                    //    Object.keys(dataObj).forEach(function (key) {
                    //        var value = dataObj[key];
                    //        if (_existingUrl != "" && _existingUrl == value)
                    //            _fld.append('<option value="' + value + '" selected="selected">' + key + '</option>');
                    //        else
                    //            _fld.append('<option value="' + value + '">' + key + '</option>');
                    //    });
                    //    _fld.select2();
                    //}
                }
            }, error: function (error) {

            }
        });

    } catch (ex) {
    }
}

function OnChangeApiType(_changeFldName) {
    if (_changeFldName == "config_url_name000F1") {
        let config_url_name = $("#config_url_name000F1").val();
        $("#ExecAPIURL000F1").val(config_url_name);
        SetFieldValue("ExecAPIURL000F1", config_url_name);
        UpdateFieldArray("ExecAPIURL000F1", "0", config_url_name, "parent", "");
    }

    //if (_changeFldName == "APItype000F1") {
    //    let apitype = $("#APItype000F1").val();
    //    if (apitype != null && (apitype == "General" || apitype == "Bind API to Dropdown field from Axpert")) {
    //        var dataObj = interExterResourcesData.resources.intres;
    //        var _fld = $j("#config_url_name000F1");
    //        _fld.empty();
    //        _fld.append('<option value=""></option>');
    //        Object.keys(dataObj).forEach(function (key) {
    //            var value = dataObj[key];
    //            _fld.append('<option value="' + value + '">' + key + '</option>');
    //        });
    //        _fld.select2();

    //    } else if (apitype != null && apitype == "Bind API to Dropdown field from External") {
    //        var dataObj = interExterResourcesData.resources.extres;
    //        var _fld = $j("#config_url_name000F1");
    //        _fld.empty();
    //        _fld.append('<option value=""></option>');
    //        Object.keys(dataObj).forEach(function (key) {
    //            var value = dataObj[key];
    //            _fld.append('<option value="' + value + '">' + key + '</option>');
    //        });
    //        _fld.select2();
    //    }
    //    $("#ExecAPIURL000F1").val("");
    //    SetFieldValue("ExecAPIURL000F1", "");
    //    UpdateFieldArray("ExecAPIURL000F1", "0", "", "parent", "");
    //} else if (_changeFldName == "config_url_name000F1") {
    //    let config_url_name = $("#config_url_name000F1").val();
    //    $("#ExecAPIURL000F1").val(config_url_name);
    //    SetFieldValue("ExecAPIURL000F1", config_url_name);
    //    UpdateFieldArray("ExecAPIURL000F1", "0", config_url_name, "parent", "");
    //}
}

function getDevOptionValue() {
    var _hideCancelButton = "false";
    try {
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
            const hideCancelButton = _finalJson.find(item => item.props?.trim().toLowerCase() === "hide cancel button in validation expression messages")?.propsval;
            if (typeof hideCancelButton != "undefined" && hideCancelButton == "true") {
                _hideCancelButton = "true";
            }
        }
    } catch (ex) {
        _hideCancelButton = "false";
    }
    return _hideCancelButton;
}