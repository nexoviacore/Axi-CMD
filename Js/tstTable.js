var fldAutoData = [];
var fldAutoList = "";
var fldExpData = [];
var colNames = [];
var colVExp = [];
var colfExp = [];
var aportionWhat = [];
var aportionAgainst = [];
var aportionWhatVal = [];
var isQuery = "";
var QueryData = "";
var colSource = [];
var thisFldId = "";
var valuesep = "";
var rowsep = "";
var rowObj = $();
var isfromSqlExec = false;
var tblFldFTot = [];

$j(document).ready(function () {

    GetTableHtml();

    $j(":text").blur(function (event) {
        MainBlur($j(this), "input");
    });

    $j("select").change(function () {
        MainBlur($j(this), "select");
    });

    LoadTableEvents("#divTable");

});

function LoadTableEvents(dvId) {

    var autoDiv = dvId == undefined ? "#divTable" : dvId;

    $j(dvId).find(".tblFromSelect:not(span)").select2();
    createFormSelect(dvId + " .tblFromSelect:not(span)");

    $j(dvId + " .number").unbind("keypress");
    $j(".number").keypress(function (event) {
        return CheckNumeric(event, $j(this).val());
    });

    $j("input.number").on('input', function () {
        var fldVal = $j(this).val();
        var fldId = $j(this).attr("id");
        var fldIndex = $j(this).attr("data-fInd");
        if (fldId != undefined) {
            var maxFldLength = callParentNew("FMaxLength")[fldIndex];
            var decimalLength = callParentNew("FDecimal")[fldIndex];
            if (decimalLength != undefined && decimalLength > 0) {
                var intPartMaxLimit = maxFldLength - decimalLength - 1;
                if (fldVal.indexOf('.') == -1) {
                    if (fldVal.length > intPartMaxLimit) $j(this).val(fldVal.slice(0, intPartMaxLimit));
                }
                else if (fldVal.indexOf('.') != -1) {
                    var intPart = fldVal.substring(0, fldVal.indexOf('.'));
                    var decPart = fldVal.substring(fldVal.indexOf('.') + 1, fldVal.length);
                    if (intPart.length > intPartMaxLimit) intPart = intPart.slice(0, intPartMaxLimit);
                    if (decPart.length > decimalLength) decPart = decPart.slice(0, decimalLength);
                    $j(this).val(intPart + "." + decPart);
                }
            }
        }
    });

    $j(":text").unbind("blur");
    $j(":text").blur(function (event) {
        MainBlur($j(this), "input");
    });

    $j("select").unbind("change");
    $j("select").change(function () {
        MainBlur($j(this), "select");
    });

    $(dvId).find(".selection .select2-selection").addClass("form-select-sm");
    $(dvId).find(".select2-hidden-accessible").addClass("form-select-sm");
    $(dvId).find(".fldmultiSelect").addClass("py-0");

    var glType = eval(callParent('gllangType'));
    var dtpkrRTL = false;
    if (glType == "ar")
        dtpkrRTL = true;
    else
        dtpkrRTL = false;
    var glCulture = eval(callParent('glCulture'));
    var dtFormat = "d/m/Y";
    if (glCulture == "en-us")
        dtFormat = "m/d/Y";
    $j(dvId + " .flatpickr-input").flatpickr({
        dateFormat: dtFormat,
        disableMobile: "true",
        allowInput: true,
        onOpen: function (selectedDates, dateStr, instance) {
        },
        onChange: function (selectedDates, dateStr, instance) {
        },
        onClose: function (selectedDates, dateStr, instance) {
            MainBlur($(instance.element), "input");
        }
    });
    $j(dvId + " .input-group-text").on("click", function () {
        $j(dvId + " .flatpickr-input")[0]._flatpickr.open();
    });
}

var dtAssoc = [];
var searchResult = [];
var AutPageNo = 1, AutPageSize = 50, rcount = 0, fetchRCount = callParentNew("FetchPickListRows") || 1000; PageCount = 0;
var CangefldName = '', isNavigation = false, refreshAC = false, pickarrow = false;
function createFormSelect(fld) {
    const formSelect = $(fld);
    var fldNameAc = "",
        fieldName = "",
        fastdll = "",
        termVal = "",
        depFldName = "";
    formSelect.select2({
        ajax: {
            url: 'tstruct.aspx/GetAutoCompleteData',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                termVal = params.term == undefined ? "" : params.term;
                let fldId = $(this).attr("data-id");
                fastdll = $(callParentNew(fldId, "id")).hasClass('fastdll');// parent.$("#" + fldId).hasClass('fastdll');
                var isrefreshsave = $(callParentNew(fldId, "id")).hasClass('isrefreshsave');// parent.$("#" + fldId).hasClass('isrefreshsave');
                var tblSourceParams = GetSourceParamValue($(this));
                var pageData = GetAutoCompData(fldNameAc, termVal, AutPageNo, AutPageSize);
                var fieldName = fldNameAc.substring(0, fldNameAc.lastIndexOf("F") - 3);
                var parentFldVal = "";
                if (typeof callParentNew('wsPerfEnabled') != "undefined" && callParentNew('wsPerfEnabled'))
                    parentFldVal = callParentNew("ISBoundAutoCom(" + fieldName + "," + fldNameAc + ")", "function");
                else
                    parentFldVal = callParentNew("ISBoundNew(" + fieldName + "," + fldNameAc + ")", "function");
                let fldApiInd = callParentNew("GetFieldIndex(" + fieldName + ")", "function");
                let isApifld = callParentNew("FldIsAPI")[fldApiInd];
                let dllSqlParamsFlag = callParentNew("FldDSqlParams")[fldApiInd];
                return JSON.stringify({
                    tstDataId: callParentNew("tstDataId"),
                    FldName: fieldName,
                    FltValue: termVal,
                    ChangedFields: parent.ChangedFields,
                    ChangedFieldDbRowNo: parent.ChangedFieldDbRowNo,
                    ChangedFieldValues: parent.ChangedFieldValues,
                    DeletedDCRows: parent.DeletedDCRows,
                    pageData: pageData,
                    fastdll: fastdll,
                    fldNameAc: fldNameAc,
                    refreshAC: refreshAC,
                    pickArrow: pickarrow,
                    parentsFlds: parentFldVal,
                    rfSave: isrefreshsave,
                    IsApiFld: isApifld,
                    tblSourceParams: tblSourceParams,
                    isTstHtmlLs: callParentNew("resTstHtmlLS"),
                    ddlFldSqlParams: dllSqlParamsFlag,
                    ddlSqlPNames: ""
                });
            },
            processResults: function (data) {
                refreshAC = false;
                callParentNew("resTstHtmlLS=", "");
                try {
                    var result = data.d.toString().replace(new RegExp("\\n", "g"), "");
                    if (result != "") {
                        if (result.split("♠*♠").length > 1) {
                            tstDataId = result.split("♠*♠")[0];
                            result = result.split("♠*♠")[1];
                        }
                        if (result.split("*♠*").length > 1) {
                            //serverprocesstime = result.split("*♠*")[1];
                            //requestProcess_logtime = result.split("*♠*")[2];
                            result = result.split("*♠*")[0];
                            // WireElapsTime(serverprocesstime, requestProcess_logtime, true);
                        }
                    }
                    if (callParentNew("CheckSessionTimeout(" + result + ")", "function"))
                        return;
                    result = result.toString().replace(new RegExp("\\t", "g"), "&#9;");
                    ChangedFields = new Array();
                    ChangedFieldDbRowNo = new Array();
                    ChangedFieldValues = new Array();
                    DeletedDCRows = new Array();
                    if (!(result.toLowerCase().includes("access violation") && result.toLowerCase().includes("asbtstruct.dll"))) {
                        if (result.indexOf('♥') > -1) {
                            result = result.split('♥')[0];
                        }
                        var serResult = $.parseJSON(result);
                        datasss = serResult;
                        if (serResult.error) {
                            ExecErrorMsg(serResult.error, "autocomplete");
                            return;
                        }

                        dtAssoc = serResult.pickdata[3].data;
                        if (dtAssoc != undefined && dtAssoc.length != 0) {
                            if (fastdll) {
                                var aSearch = [];
                                $(dtAssoc).each(function (iIndex, sElement) {
                                    sElement.i = sElement.i.replace(/\^\^dq/g, '"');
                                    if (sElement.i.toLowerCase().indexOf(termVal.toLowerCase()) >= 0) {
                                        aSearch.push(sElement);
                                    }
                                });
                                $("#" + fldNameAc).data("rowcount", aSearch.length);
                                if (aSearch.length != 0) {
                                    var result = ($.map(aSearch, function (item) {
                                        item.i = item.i.replace(/\^\^dq/g, '"');
                                        return {
                                            //id: item.v == "" ? item.i : item.v,
                                            //text: item.i,
                                            id: item.i,
                                            text: item.i,
                                            dep: item.d
                                        }
                                    }))
                                    searchResult = result;
                                    return {
                                        results: result
                                    };
                                } else {
                                    var cutMsg = eval(callParent('lcm[0]'));
                                    return { results: "", noResults: cutMsg };
                                }
                            } else {
                                var result = ($.map(dtAssoc, function (item) {
                                    item.i = item.i.replace(/\^\^dq/g, '"');
                                    return {
                                        //id: item.v == "" ? item.i : item.v,
                                        //text: item.i,
                                        id: item.i,
                                        text: item.i,
                                        dep: item.d
                                    }
                                }))
                                searchResult = result;
                                return {
                                    results: result
                                };
                            }
                        } else {
                            var cutMsg = eval(callParent('lcm[0]'));
                            return { results: "", noResults: cutMsg };
                        }
                    } else {
                        AxWaitCursor(false);
                        ShowDimmer(false);
                        $("#reloaddiv").show();
                        $("#dvlayout").hide();
                    }
                } catch (exception) {
                    if (exception.message.toLowerCase().indexOf("access violation") != -1) {
                        AxWaitCursor(false);
                        ShowDimmer(false);
                        $("#reloaddiv").show();
                        $("#dvlayout").hide();
                    }
                }
            }
        },
        placeholder: 'Search for a repository',
        minimumInputLength: 0
    }).on('select2:open', function (e) {
        let fldId = $(this).attr("data-id");
        fldNameAc = fldId;
    });
}

function GetTableHtml() {
    let tblData = "";
    let tblMetaDataJSON = "";
    let rowDataExist = false;
    try {
        parent.ChangedTblFields = new Array();
        parent.ChangedTblFieldVals = new Array();
        thisFldId = $("#hdnfieldId").val();

        let thisFldVal = "";
        let thisFldDisabled = false;
        let thisFldRowNo = "";
        let json = "";
        if (typeof thisFldId != "undefined" && thisFldId == "fromsqlexec") {
            isfromSqlExec = true;
            thisFldVal = callParentNew('_sqlParStr');
            thisFldDisabled = false;
            thisFldRowNo = "";
            json = callParentNew('_sqlTblJson');
        } else {
            thisFldVal = $(callParentNew(thisFldId, "id")).val();
            thisFldDisabled = $(callParentNew(thisFldId, "id")).prop("disabled");
            thisFldRowNo = callParentNew("GetFieldsRowNo(" + thisFldId + ")", "function");
            let thisFieldName = thisFldId.substring(0, thisFldId.lastIndexOf("F") - 3);
            let thisFldInd = callParentNew("GetFieldIndex(" + thisFieldName + ")", "function");
            json = callParentNew("FTableTypeVal")[thisFldInd];
        }
        if (json != "") {
            json = RepSpecialCharsInHTML(json);
            if (!json.startsWith("{") && !json.startsWith("db@")) {
                let dsfName = json.split("~")[1];
                let dsfDc = callParentNew("GetDcNo(" + dsfName + ")", "function");
                let _dcInx = $j.inArray(dsfDc, parent.DCFrameNo);
                let isGirdDc = parent.DCIsGrid[_dcInx];
                let dsfValue = "";
                if (isGirdDc == "True")
                    dsfValue = $(callParentNew(dsfName + thisFldRowNo + "F" + dsfDc, "id")).val();
                else
                    dsfValue = $(callParentNew(dsfName + "000F" + dsfDc, "id")).val();
                json = GetstrTableJson(dsfValue);
                tblMetaDataJSON = json;
            } else if (json.startsWith("db@")) {
                json = GetDBProcTableJson(json);
                tblMetaDataJSON = json;
            }
            var tableJson = JSON.parse(json);
            if (typeof tableJson != "undefined") {
                var tblType = tableJson.props.type;
                let colCount = tableJson.props.colcount;
                let rowCount = tableJson.props.rowcount;
                let delRow = tableJson.props.deleterow;
                let addRow = tableJson.props.addrow;
                rowsep = tableJson.props.rowseparator;
                valuesep = tableJson.props.valueseparator;
                if (typeof tableJson.props.sql != "undefined") {
                    isQuery = tableJson.props.sql;
                }
                let isDisabled = thisFldDisabled == true ? "disabled" : "";
                if (typeof parent.transid != "undefined" && parent.transid == "b_sql")
                    isDisabled = "";
                if (tblType == "table") {
                    let rowVal = "";
                    if (thisFldVal != "") {
                        rowVal = thisFldVal.split(rowsep);
                        if (rowVal.length > rowCount)
                            rowCount = rowVal.length;
                    }

                    if (rowVal == "" && isQuery == "true") {
                        SqlFillData();
                    }

                    let thRow = "";
                    let tdRow = "";

                    var exprVal = [];
                    thRow += "<tr class=\"fw-bold fs-6 text-gray-800 border-bottom-2 border-gray-200\">";
                    if (delRow.toLowerCase() == "t" || delRow.toLowerCase() == "true")
                        //thRow += "<th class=\"w-10px\"></th>";
                        thRow += "<th class=\"w-10px\"><div class=\"form-check form-check-sm form-check-custom p-2\"><input type=\"checkbox\" class=\"form-check-input fgHdrChk\" name=\"chkItem\" onclick=\"javascript:CheckAll(this);\"></div></th>";
                    for (var i = 0; i < colCount; i++) {
                        let colCaption = tableJson.columns[i + 1].caption;
                        let tfhide = "";
                        if (typeof tableJson.columns[i + 1].hide != "undefined" && tableJson.columns[i + 1].hide != "")
                            tfhide = tableJson.columns[i + 1].hide.toLowerCase() == "t" ? " d-none " : "";

                        colNames.push(tableJson.columns[i + 1].name);

                        if (typeof tableJson.columns[i + 1].ftot != "undefined")
                            tblFldFTot.push(tableJson.columns[i + 1].ftot);
                        else
                            tblFldFTot.push('');

                        parent.ChangedTblFields.push(tableJson.columns[i + 1].name);
                        parent.ChangedTblFieldVals.push('');
                        if (tfhide != "")
                            thRow += "<th class=\"" + tfhide + "\">" + colCaption + "</th>";
                        else
                            thRow += "<th>" + colCaption + "</th>";
                        let colExp = tableJson.columns[i + 1].exp;
                        colfExp.push(colExp);
                        var result = "";
                        if (colExp != "") {
                            colExp = colExp.replace(/,/g, "♦");
                            result = callParentNew("Evaluate(" + tableJson.columns[i + 1].name + ",000," + colExp + ",expr,table)", "function");
                        }
                        fldExpData.push(result);
                        colVExp.push(tableJson.columns[i + 1].vexp);
                        if (typeof tableJson.columns[i + 1].aportionwhat != "undefined" && tableJson.columns[i + 1].aportionwhat != "") {
                            aportionWhat.push(tableJson.columns[i + 1].aportionwhat);
                            let fldaportionwhat = tableJson.columns[i + 1].aportionwhat;
                            var fldaportionwhatId = $j($j("[id*='" + fldaportionwhat + "']", doParentOpInIframe("document", "rtn", "[id*='" + fldaportionwhat + "']")).find("input")).attr("id");
                            aportionWhatVal.push($(callParentNew(fldaportionwhatId, "id")).val());
                        }
                        else {
                            aportionWhat.push("");
                            aportionWhatVal.push("");
                        }
                        if (typeof tableJson.columns[i + 1].aportionagainst != "undefined")
                            aportionAgainst.push(tableJson.columns[i + 1].aportionagainst);
                        else
                            aportionAgainst.push("");
                    }
                    thRow += "</tr>";

                    for (var j = 0; j < rowCount; j++) {
                        let colValues = "";
                        if ((typeof rowVal[j] != "undefined" && rowVal[j] != "") && rowVal != "") {
                            colValues = rowVal[j];
                            colValues = colValues.split(valuesep);
                        }
                        tdRow += "<tr id=" + j + ">";
                        if (delRow.toLowerCase() == "t" || delRow.toLowerCase() == "true")
                            tdRow += "<td class=\"input-group-sm\"><div class=\"form-check form-check-sm form-check-custom p-2\"><input type=\"checkbox\" class=\"form-check-input fgChk\" name=\"chkItem\" onclick=\"javascript:ChkHdrCheckbox(this);\" " + isDisabled + "></div></td>";

                        for (var i = 0; i < colCount; i++) {
                            let colValue = "";

                            if ((typeof colValues[i] != "undefined" && colValues[i] != "") && colValues != "")
                                colValue = colValues[i];
                            else if (typeof fldExpData[i] != "undefined" && fldExpData[i] != "")
                                colValue = fldExpData[i];
                            else if (typeof aportionWhatVal[i] != "undefined" && aportionWhatVal[i] != "" && j == 0)
                                colValue = aportionWhatVal[i];
                            else
                                colValue = tableJson.columns[i + 1].value;
                            let sourceFld = tableJson.columns[i + 1].source;

                            let allowEmpty = "";
                            if (typeof tableJson.columns[i + 1].allowempty != "undefined" && tableJson.columns[i + 1].allowempty != "")
                                allowEmpty = tableJson.columns[i + 1].allowempty.toLowerCase() == "f" ? " data-ae='false' " : "";
                            let allowDuplicate = "";
                            if (typeof tableJson.columns[i + 1].allowduplicate != "undefined" && tableJson.columns[i + 1].allowduplicate != "")
                                allowDuplicate = tableJson.columns[i + 1].allowduplicate.toLowerCase() == "f" ? " data-ad='false' " : "";
                            let tfreadyOnly = "";
                            if (typeof tableJson.columns[i + 1].readyonly != "undefined" && tableJson.columns[i + 1].readyonly != "")
                                tfreadyOnly = tableJson.columns[i + 1].readyonly.toLowerCase() == "t" ? " disabled " : "";
                            let tfhide = "";
                            if (typeof tableJson.columns[i + 1].hide != "undefined" && tableJson.columns[i + 1].hide != "")
                                tfhide = tableJson.columns[i + 1].hide.toLowerCase() == "t" ? " d-none " : "";

                            colSource.push(sourceFld);
                            let sourFldInd = callParentNew("GetFieldIndex(" + sourceFld + ")", "function");
                            let fldType = callParentNew("GetDWBFieldType(''," + sourFldInd + ")", "function");
                            let sourceDc = callParentNew("GetDcNo(" + sourceFld + ")", "function");
                            let _thisthisFldRowNo = thisFldRowNo;
                            if (!callParentNew("IsDcGrid(" + sourceDc + ")", "function")) {
                                _thisthisFldRowNo = "000";
                            }

                            let sourceFldId = sourceFld + _thisthisFldRowNo + "F" + sourceDc;
                            if (typeof callParentNew("FMoe") != "undefined" && callParentNew("FMoe")[sourFldInd] == "Select") {
                                if (callParentNew("FldIsSql")[sourFldInd] == "True") {
                                    tdRow += "<td class=\"input-group-sm " + tfhide + "\"><select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"combotem Family form-control form-select tblFromSelect fldtableinput\" data-control=\"select2\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + colValue + "\" data-id=\"" + sourceFldId + "\" data-placeholder=\"Select an option\" data-allow-clear=\"true\">";
                                    if (colValue != "")
                                        tdRow += '<option value="' + colValue + '" selected="selected">' + colValue + '</option>';
                                    tdRow += "</select></td>";
                                }
                                else {
                                    var options = "";
                                    $(callParentNew(sourceFldId)).find("option").each(function () {
                                        if (colValue != "" && $(this)[0].value == colValue)
                                            options += $(this)[0].outerHTML.replace("<option ", "<option selected ");
                                        else
                                            options += $(this)[0].outerHTML.replace("selected=\"selected\"", "");
                                    });

                                    if (options == "") {
                                        let ddList = $(callParentNew(sourceFldId)).data('val').split(',');
                                        let ddlSelVal = ($(callParentNew(sourceFldId)).val() == "" ? colValue : $(callParentNew(sourceFldId)).val());
                                        for (var z = 0; z < ddList.length; z++) {
                                            if (ddlSelVal != "" && ddlSelVal == ddList[z])
                                                options += "<option selected=\"selected\" value=\"" + ddList[z] + "\">" + (ddList[z] == "" ? "-- Select --" : ddList[z]) + "</option>";
                                            else
                                                options += "<option value=\"" + ddList[z] + "\">" + (ddList[z] == "" ? "-- Select --" : ddList[z]) + "</option>";
                                        }
                                        tdRow += "<td class=\"input-group-sm " + tfhide + "\"><select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"tem Family form-control\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + ddlSelVal + "\">" + options + "</select></td>";
                                    }
                                    else
                                        tdRow += "<td class=\"input-group-sm " + tfhide + "\"><select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"tem Family form-control\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + colValue + "\">" + options + "</select></td>";
                                }
                            }
                            else if (typeof callParentNew("FMoe") != "undefined" && callParentNew("FMoe")[sourFldInd] == "Accept" && callParentNew("FldIsSql")[sourFldInd] == "True") {
                                if (sourceFld != "" && colValue == "")
                                    colValue = $(callParentNew(sourceFldId)).val();
                                if (typeof colValue == "undefined")
                                    colValue = "";
                                tdRow += "<td class=\"input-group-sm " + tfhide + "\"><input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control fldtableinput\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + colValue + "\"></td>";
                            }
                            else if (typeof aportionWhat[i] != "undefined" && aportionWhat[i] != "") {
                                tdRow += "<td class=\"input-group-sm " + tfhide + "\"><input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control fldtableinput\" readonly name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + colValue + "\"></td>";
                            }
                            else {
                                if (typeof callParentNew("FDataType") != "undefined" && callParentNew("FDataType")[sourFldInd] == "Numeric") {
                                    if (sourceFld != "" && colValue == "")
                                        colValue = $(callParentNew(sourceFldId)).val();
                                    tdRow += "<td class=\"input-group-sm " + tfhide + "\"><input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" style=\"text-align:right;\" class=\"tem Family form-control number fldtableinput\" data-fInd=\"" + sourFldInd + "\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + colValue + "\"></td>";
                                }
                                else if (typeof callParentNew("FDataType") != "undefined" && callParentNew("FDataType")[sourFldInd] == "Date/Time") {
                                    if (sourceFld != "" && colValue == "")
                                        colValue = $(callParentNew(sourceFldId)).val();
                                    if (typeof colValue == "undefined")
                                        colValue = "";
                                    tdRow += "<td class=\"input-group input-group-sm " + tfhide + "\"><input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control flatpickr-input fldtableinput\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + colValue + "\">";
                                    //tdRow += "<span class=\"input-group-addon spandate \"><i class=\"glyphicon glyphicon-calendar icon-basic-calendar\" title=" + (colNames[i] + "R" + j) + "></i></span></td>";
                                    tdRow += "<span class=\"input-group-text\" id=\"basic-addon2\" data-toggle><span class=\"material-icons material-icons-style cursor-pointer fs-4\">calendar_today</span></span>";
                                }
                                else {
                                    if (sourceFld != "" && callParentNew("FMoe")[sourFldInd] == "Accept" && $(callParentNew(sourceFldId)).val() != "" && $(callParentNew(sourceFldId)).val().indexOf(',') > 0) {
                                        let ddList = $(callParentNew(sourceFldId)).val().split(',');
                                        let ddlSelVal = colValue;
                                        var options = "";
                                        options += "<option value=\"\">-- Select --</option>";
                                        for (var z = 0; z < ddList.length; z++) {
                                            if (ddlSelVal != "" && ddlSelVal == ddList[z])
                                                options += "<option selected=\"selected\" value=\"" + ddList[z] + "\">" + ddList[z] + "</option>";
                                            else
                                                options += "<option value=\"" + ddList[z] + "\">" + ddList[z] + "</option>";
                                        }
                                        tdRow += "<td class=\"input-group-sm " + tfhide + "\"><select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"tem Family form-control\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + ddlSelVal + "\">" + options + "</select></td>";
                                    }
                                    else {
                                        if (isfromSqlExec && colNames[i] == "datatypefld") {
                                            var options = "";
                                            options += "<option selected=\"selected\" value=\"c\">" + colValue + "</option>";
                                            options += "<option value=\"n\">Numeric</option>";
                                            options += "<option value=\"d\">Date/Time</option>";
                                            tdRow += "<td class=\"input-group-sm " + tfhide + "\"><select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"tem Family form-control\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"c\">" + options + "</select></td>";
                                        }
                                        else {
                                            if (sourceFld != "" && colValue == "")
                                                colValue = $(callParentNew(sourceFldId)).val();
                                            if (typeof colValue == "undefined")
                                                colValue = "";
                                            tdRow += "<td class=\"input-group-sm " + tfhide + "\"><input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control fldtableinput\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + j) + "\" value=\"" + colValue + "\"></td>";
                                        }
                                    }
                                }
                            }
                            if (j == 0)
                                parent.ChangedTblFieldVals[i] = colValue;
                            else {
                                parent.ChangedTblFieldVals[i] = parent.ChangedTblFieldVals[i] + "~" + colValue;
                            }
                        }
                        tdRow += "</tr>";
                    }
                    tblData = "<div class=\"\">";
                    tblData += "<div class=\"pb-5\">";
                    if (addRow.toLowerCase() == "t" || addRow.toLowerCase() == "true")
                        tblData += "<div class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm pull-left " + isDisabled + "\"><span class=\"material-icons material-icons-style material-icons-3\" title=\"Add\" onclick=\"AddTableRows('fldTable','addrowclk');\">add</span></div>";
                    if (delRow.toLowerCase() == "t" || delRow.toLowerCase() == "true")
                        tblData += "<div class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm pull-left " + isDisabled + "\"><span class=\"material-icons material-icons-style material-icons-3\" title=\"Delete\" onclick=\"DeleteTableRows('fldTable');\">delete_outline</span></div>";
                    if (isQuery != "")
                        tblData += "<div class=\"btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary me-2 shadow-sm pull-left " + isDisabled + "\"><span class=\"material-icons material-icons-style material-icons-3\" title=\"Fill Data\" onclick=\"SqlFillData();\">view_list</span></div>";
                    tblData += "<table id=\"fldTable\" class=\"table table-bordered table-sm mt-10\">";
                    tblData += "<thead>";
                    tblData += thRow;
                    tblData += "</thead>";
                    tblData += "<tbody>";
                    tblData += tdRow;
                    tblData += "</tbody>";
                    tblData += "</table>";
                    tblData += "</div>";
                    tblData += "<div></div>";
                    if (isfromSqlExec)
                        tblData += "<div class=\"float-end\"><a href=\"javascript:void(0)\" onclick=\"ClearTableData('fldTable');\" class=\"btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 " + isDisabled + "\"><span class=\"material-icons\">clear</span>Clear</a><a href=\"javascript:void(0);\" title=\"Ok\" onclick=\"AddTableData('" + thisFldId + "','" + valuesep + "','" + rowsep + "');\" class=\"btn btn-primary d-inline-flex align-items-center shadow-sm me-2 " + isDisabled + "\"><span class=\"material-icons\">save</span>Ok</a></div>";
                    else
                        tblData += "<div class=\"float-end d-none\"><a href=\"javascript:void(0)\" onclick=\"ClearTableData('fldTable');\" class=\"btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 " + isDisabled + "\"><span class=\"material-icons\">clear</span>Clear</a><a href=\"javascript:void(0);\" title=\"Ok\" onclick=\"AddTableData('" + thisFldId + "','" + valuesep + "','" + rowsep + "');\" class=\"btn btn-primary d-inline-flex align-items-center shadow-sm me-2 " + isDisabled + "\"><span class=\"material-icons\">save</span>Ok</a></div>";

                    tblData += "</div>";

                    if (rowVal == "" && isQuery == "sqlDescr") {
                        setTimeout(function () {
                            SqlFillData();
                        }, 0);
                    }
                    if (rowVal != "") {
                        rowDataExist = true;
                    }
                }
                else {
                    var exprVal = [];
                    let colValues = "";
                    if (thisFldVal != "") {
                        colValues = thisFldVal;
                        colValues = colValues.split(valuesep);
                    }
                    var formDiv = "<div id=\"divtblForm\" >";
                    for (var i = 0; i < colCount; i++) {
                        let tfhide = "";
                        if (typeof tableJson.columns[i + 1].hide != "undefined" && tableJson.columns[i + 1].hide != "")
                            tfhide = tableJson.columns[i + 1].hide.toLowerCase() == "t" ? " d-none " : "";

                        let colCaption = tableJson.columns[i + 1].caption;
                        formDiv += "<div class=\"col-lg-12 col-sm-3 col-md-3 col-xs-12 " + tfhide + "\"><label>" + colCaption + "</label></div>";
                        formDiv += "<div class=\"col-lg-12 col-sm-9 col-md-9 col-xs-12 " + tfhide + "\">";
                        colNames.push(tableJson.columns[i + 1].name);
                        parent.ChangedTblFields.push(tableJson.columns[i + 1].name);
                        parent.ChangedTblFieldVals.push('');
                        let colExp = tableJson.columns[i + 1].exp;
                        colfExp.push(colExp);
                        var result = "";
                        if (colExp != "") {
                            colExp = colExp.replace(/,/g, "♦");
                            result = callParentNew("Evaluate(" + tableJson.columns[i + 1].name + ",000," + colExp + ",expr,table)", "function");
                        }
                        fldExpData.push(result);
                        colVExp.push(tableJson.columns[i + 1].vexp);
                        if (typeof tableJson.columns[i + 1].aportionwhat != "undefined" && tableJson.columns[i + 1].aportionwhat != "") {
                            aportionWhat.push(tableJson.columns[i + 1].aportionwhat);
                            let fldaportionwhat = tableJson.columns[i + 1].aportionwhat;
                            var fldaportionwhatId = $j($j("[id*='" + fldaportionwhat + "']", doParentOpInIframe("document", "rtn", "[id*='" + fldaportionwhat + "']")).find("input")).attr("id");
                            aportionWhatVal.push($(callParentNew(fldaportionwhatId, "id")).val());
                        }
                        else {
                            aportionWhat.push("");
                            aportionWhatVal.push("");
                        }
                        if (typeof tableJson.columns[i + 1].aportionagainst != "undefined")
                            aportionAgainst.push(tableJson.columns[i + 1].aportionagainst);
                        else
                            aportionAgainst.push("");


                        let colValue = "";

                        if ((typeof colValues[i] != "undefined" && colValues[i] != "") && colValues != "")
                            colValue = colValues[i];
                        else if (typeof fldExpData[i] != "undefined" && fldExpData[i] != "")
                            colValue = fldExpData[i];
                        else if (typeof aportionWhatVal[i] != "undefined" && aportionWhatVal[i] != "")
                            colValue = aportionWhatVal[i];
                        else
                            colValue = tableJson.columns[i + 1].value;
                        let sourceFld = tableJson.columns[i + 1].source;

                        let allowEmpty = "";
                        if (typeof tableJson.columns[i + 1].allowempty != "undefined" && tableJson.columns[i + 1].allowempty != "")
                            allowEmpty = tableJson.columns[i + 1].allowempty.toLowerCase() == "f" ? " data-ae='false' " : "";
                        let allowDuplicate = "";
                        if (typeof tableJson.columns[i + 1].allowduplicate != "undefined" && tableJson.columns[i + 1].allowduplicate != "")
                            allowDuplicate = tableJson.columns[i + 1].allowduplicate.toLowerCase() == "f" ? " data-ad='false' " : "";
                        let tfreadyOnly = "";
                        if (typeof tableJson.columns[i + 1].readyonly != "undefined" && tableJson.columns[i + 1].readyonly != "")
                            tfreadyOnly = tableJson.columns[i + 1].readyonly.toLowerCase() == "t" ? " disabled " : "";

                        colSource.push(sourceFld);
                        let sourFldInd = callParentNew("GetFieldIndex(" + sourceFld + ")", "function");
                        let fldType = callParentNew("GetDWBFieldType(''," + sourFldInd + ")", "function");
                        let sourceDc = callParentNew("GetDcNo(" + sourceFld + ")", "function");
                        let _thisthisFldRowNo = thisFldRowNo;
                        if (!callParentNew("IsDcGrid(" + sourceDc + ")", "function")) {
                            _thisthisFldRowNo = "000";
                        }

                        let sourceFldId = sourceFld + _thisthisFldRowNo + "F" + sourceDc;
                        if (callParentNew("FMoe")[sourFldInd] == "Select") {
                            if (callParentNew("FldIsSql")[sourFldInd] == "True") {
                                formDiv += "<select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"combotem Family form-control form-select tblFromSelect fldtableinput\" data-control=\"select2\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + colValue + "\" data-id=\"" + sourceFldId + "\" data-placeholder=\"Select an option\" data-allow-clear=\"true\">";
                                if (colValue != "")
                                    formDiv += '<option value="' + colValue + '" selected="selected">' + colValue + '</option>';
                                formDiv += "</select>";
                            }
                            else {
                                var options = "";
                                $(callParentNew(sourceFldId)).find("option").each(function () {
                                    if (colValue != "" && $(this)[0].value == colValue)
                                        options += $(this)[0].outerHTML.replace("<option ", "<option selected ");
                                    else
                                        options += $(this)[0].outerHTML.replace("selected=\"selected\"", "");
                                });

                                if (options == "") {
                                    let ddList = $(callParentNew(sourceFldId)).data('val').split(',');
                                    let ddlSelVal = ($(callParentNew(sourceFldId)).val() == "" ? colValue : $(callParentNew(sourceFldId)).val());
                                    for (var z = 0; z < ddList.length; z++) {
                                        if (ddlSelVal != "" && ddlSelVal == ddList[z])
                                            options += "<option selected=\"selected\" value=\"" + ddList[z] + "\">" + (ddList[z] == "" ? "-- Select --" : ddList[z]) + "</option>";
                                        else
                                            options += "<option value=\"" + ddList[z] + "\">" + (ddList[z] == "" ? "-- Select --" : ddList[z]) + "</option>";
                                    }
                                    formDiv += "<select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"tem Family form-control\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + ddlSelVal + "\">" + options + "</select>";
                                }
                                else {
                                    formDiv += "<select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"tem Family form-control\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + colValue + "\">" + options + "</select>";
                                }
                            }
                        }
                        else if (callParentNew("FMoe")[sourFldInd] == "Accept" && callParentNew("FldIsSql")[sourFldInd] == "True") {
                            if (sourceFld != "" && colValue == "")
                                colValue = $(callParentNew(sourceFldId)).val();
                            if (typeof colValue == "undefined")
                                colValue = "";
                            formDiv += "<input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control fldtableinput\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + colValue + "\">";
                        }
                        else if (typeof aportionWhat[i] != "undefined" && aportionWhat[i] != "") {
                            formDiv += "<input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control fldtableinput\" readonly name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + colValue + "\">";
                        }
                        else {
                            if (callParentNew("FDataType")[sourFldInd] == "Numeric") {
                                if (sourceFld != "" && colValue == "")
                                    colValue = $(callParentNew(sourceFldId)).val();
                                if (typeof colValue == "undefined")
                                    colValue = "";
                                formDiv += "<input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" style=\"text-align:right;\" class=\"tem Family form-control number fldtableinput\" data-fInd=\"" + sourFldInd + "\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + colValue + "\">";
                            }
                            else if (callParentNew("FDataType")[sourFldInd] == "Date/Time") {
                                if (sourceFld != "" && colValue == "")
                                    colValue = $(callParentNew(sourceFldId)).val();
                                if (typeof colValue == "undefined")
                                    colValue = "";
                                formDiv += "<div class=\"input-group input-group-sm\"><input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control flatpickr-input fldtableinput\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + colValue + "\">";
                                //formDiv += "<span class=\"input-group-addon spandate \"><i class=\"glyphicon glyphicon-calendar icon-basic-calendar\" title=\"" + colNames[i] + "\"></i></span></div>";
                                formDiv += "<span class=\"input-group-text\" id=\"basic-addon2\" data-toggle><span class=\"material-icons material-icons-style cursor-pointer fs-4\">calendar_today</span></span>";
                            }
                            else {
                                if (sourceFld != "" && callParentNew("FMoe")[sourFldInd] == "Accept" && $(callParentNew(sourceFldId)).val() != "" && $(callParentNew(sourceFldId)).val().indexOf(',') > 0) {
                                    let ddList = $(callParentNew(sourceFldId)).val().split(',');
                                    let ddlSelVal = colValue;
                                    var options = "";
                                    options += "<option value=\"\">-- Select --</option>";
                                    for (var z = 0; z < ddList.length; z++) {
                                        if (ddlSelVal != "" && ddlSelVal == ddList[z])
                                            options += "<option selected=\"selected\" value=\"" + ddList[z] + "\">" + ddList[z] + "</option>";
                                        else
                                            options += "<option value=\"" + ddList[z] + "\">" + ddList[z] + "</option>";
                                    }
                                    formDiv += "<select " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " class=\"tem Family form-control\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + ddlSelVal + "\">" + options + "</select>";
                                }
                                else {
                                    if (sourceFld != "" && colValue == "")
                                        colValue = $(callParentNew(sourceFldId)).val();
                                    if (typeof colValue == "undefined")
                                        colValue = "";
                                    formDiv += "<input " + isDisabled + tfreadyOnly + allowEmpty + allowDuplicate + " type=\"text\" class=\"tem Family form-control fldtableinput\" name=\"" + colNames[i] + "\" id=\"" + (colNames[i] + "R" + 0) + "\" value=\"" + colValue + "\">";
                                }
                            }
                        }
                        if (i == 0)
                            parent.ChangedTblFieldVals[i] = colValue;
                        else {
                            parent.ChangedTblFieldVals[i] = parent.ChangedTblFieldVals[i] + "~" + colValue;
                        }
                        formDiv += "</div><div class=\"clearfix\"></div>";
                    }
                    formDiv += "</div>";

                    tblData = "<div>";
                    tblData += formDiv;
                    tblData += "</div><div class=\"clearfix\"></div>";
                    tblData += "<div class=\"d-none\"><div class=\"col-12 float-end\"><a href=\"javascript:void(0)\" onclick=\"ClearTableData('fldTable');\" title=\"Clear\" class=\"btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center shadow-sm me-2 " + isDisabled + "\"><span class=\"material-icons\">clear</span>Clear</a><a href=\"javascript:void(0);\" title=\"Ok\" onclick=\"AddTableData('" + thisFldId + "','" + valuesep + "','" + rowsep + "');\" class=\"btn btn-primary d-inline-flex align-items-center shadow-sm me-2 " + isDisabled + "\"><span class=\"material-icons\">save</span>Ok</a></div></div>";
                }
            }
        }
    } catch (ex) { }
    $("#divTable").html(tblData);

    if (rowDataExist) {
        setTimeout(function () {
            totalOnLoadtstTable();
        }, 0);
    }
    try {
        parent.AxAfterTableLoad(tblMetaDataJSON);
    } catch (ex) { }
}

function AddTableData() {
    var fldTblVal = "";
    let isErrorflag = true;
    let strAllowDuplicate = "";
    if ($("#fldTable tbody tr").length > 0) {
        $("#fldTable tbody tr").each(function () {
            var fldTblTdVal = "";
            $(this).find("td").each(function () {
                if ($(this).find("select").length > 0 && typeof $(this).find("select").attr("value") != "undefined") {
                    if (typeof $(this).find("select").data("ae") != "undefined" && $(this).find("select").data("ae") == false && $(this).find("select").attr("value") == "") {
                        callParentNew('showAlertDialog("error",' + $(this).find("select").attr('id') + ' cannot be left empty.)', 'function');
                        $(this).find("select").focus();
                        isErrorflag = false;
                        return false;
                    }
                    if (typeof $(this).find("select").data("ad") != "undefined" && $(this).find("select").data("ad") == false && $(this).find("select").attr("value") != "") {
                        let _thisAd = $(this).find("select").attr('name') + "~" + $(this).find("select").attr("value") + "♠";
                        if (strAllowDuplicate.indexOf(_thisAd) > -1) {
                            callParentNew('showAlertDialog("error",' + $(this).find("select").attr('id') + ' duplicate value not allowed.)', 'function');
                            $(this).find("select").focus();
                            isErrorflag = false;
                            return false;
                        } else {
                            strAllowDuplicate += $(this).find("select").attr('name') + "~" + $(this).find("select").attr("value") + "♠";
                        }
                    }
                    fldTblTdVal += $(this).find("select").attr("value") + valuesep;
                }
                else if ($(this).find("input:text").length > 0) {
                    if (typeof $(this).find("input").data("ae") != "undefined" && $(this).find("input").data("ae") == false && $(this).find("input").attr("value") == "") {
                        callParentNew('showAlertDialog("error",' + $(this).find("input").attr('id') + ' cannot be left empty.)', 'function');
                        $(this).find("input").focus();
                        isErrorflag = false;
                        return false;
                    }
                    if (typeof $(this).find("input").data("ad") != "undefined" && $(this).find("input").data("ad") == false && $(this).find("input").attr("value") != "") {
                        let _thisAd = $(this).find("input").attr('name') + "~" + $(this).find("input").attr("value") + "♠";
                        if (strAllowDuplicate.indexOf(_thisAd) > -1) {
                            callParentNew('showAlertDialog("error",' + $(this).find("input").attr('id') + ' duplicate value not allowed.)', 'function');
                            $(this).find("select").focus();
                            isErrorflag = false;
                            return false;
                        } else {
                            strAllowDuplicate += $(this).find("input").attr('name') + "~" + $(this).find("input").attr("value") + "♠";
                        }
                    }
                    fldTblTdVal += $(this).find("input").attr("value") + valuesep;
                }
            });
            if (fldTblTdVal != "")
                fldTblTdVal = fldTblTdVal.substring(0, fldTblTdVal.length - 1);
            fldTblVal += fldTblTdVal + rowsep;
        });
    }
    else {
        var fldTblTdVal = "";
        $("#divTable").find("input,select").each(function () {
            if ($(this).is("select") && typeof $(this).attr("value") != "undefined") {
                if (typeof $(this).data("ae") != "undefined" && $(this).data("ae") == false && $(this).attr("value") == "") {
                    callParentNew('showAlertDialog("error",' + $(this).attr('name') + ' cannot be left empty.)', 'function');
                    $(this).find("select").select2('open');
                    isErrorflag = false;
                    return false;
                }
                fldTblTdVal += $(this).attr("value") + valuesep;
            }
            else if ($(this).is("input") && $(this).attr("type") == "text") {
                if (typeof $(this).data("ae") != "undefined" && $(this).data("ae") == false && $(this).attr("value") == "") {
                    callParentNew('showAlertDialog("error",' + $(this).attr('name') + ' cannot be left empty.)', 'function');
                    $(this).find("input").focus();
                    isErrorflag = false;
                    return false;
                }
                fldTblTdVal += $(this).attr("value") + valuesep;
            }
        });
        if (fldTblTdVal != "")
            fldTblTdVal = fldTblTdVal.substring(0, fldTblTdVal.length - 1);
        fldTblVal += fldTblTdVal + rowsep;
    }
    if (isErrorflag) {
        if (isfromSqlExec) {
            fldTblVal = fldTblVal.substring(0, fldTblVal.length - 1);
            parent._sqlTblValues = fldTblVal;
            parent.closeModalDialog();
        } else {
            fldTblVal = fldTblVal.substring(0, fldTblVal.length - 1);
            $j("#" + thisFldId, doParentOpInIframe("document", "rtn", "#" + thisFldId)).val(fldTblVal)
            UpdateArray(thisFldId, fldTblVal);
            parent.CallbackFunctionBootstrap(thisFldId);
        }
        try {
            parent.AxAfterAddTableData();
        } catch (ex) { }
        callParentNew("modalIdTableField", "id").dispatchEvent(new CustomEvent("close"));
    }
}

function ClearTableData(thisTblId) {
    $("#" + thisTblId + " tbody tr td").each(function () {
        $(this).find('input:text').val('');
        $(this).find('input:text').attr("value", '');
        $(this).find('select').val('');
        $(this).find('select').prop("selected", false);
        if ($(this).find('select').hasClass("tblFromSelect"))
            $(this).find('select').find('option').remove();
    });
    setTimeout(function () {
        totalOnLoadtstTable();
    }, 0);
}

function DeleteTableRows(thisTblId) {
    $("#" + thisTblId + " tbody tr").each(function () {
        if (typeof $(this).find(".fgChk:checked").prop("checked") != "undefined" && $(this).find(".fgChk:checked").prop("checked")) {
            if ($("#" + thisTblId + " tbody tr").length == 1) {
                $j(".fgHdrChk").prop("checked", false);
                rowObj = $(this).clone();
            }
            $(this).remove();
        }
    });
    setTimeout(function () {
        totalOnLoadtstTable();
    }, 0);
}

function AddTableRows(thisTblId, isAddRowClk = '') {
    var thisRow = $("#" + thisTblId + " tbody tr:last");
    var rowNo = '-1';
    if (thisRow.length == 0) {
        rowObj.find('input:text').val('');
        $("#" + thisTblId + " tbody").append(rowObj);
    }
    else {
        rowNo = $("#" + thisTblId + " tbody tr:last").attr("id");
        $(thisRow).clone().insertAfter(thisRow).find('input:text').val('');
    }
    $("#" + thisTblId + " tbody tr:last").find(".fgChk").prop("checked", false);
    $j(".fgHdrChk").prop("checked", false);
    $("#" + thisTblId + " tbody tr:last").find('select,input,div.autoclear i,div.edit i,div[id^=autoadv],div[id^=autoadv] i').each(function (index, el) {
        var currentElement = $j(this);
        if (currentElement.hasClass("tblFromSelect")) {
            let isdisabled = currentElement.attr("disabled");
            let _thisTd = currentElement.parent("td");
            let fName = currentElement.attr("name");
            let newFldName = fName + "R" + (parseInt(rowNo) + 1);
            let dtId = currentElement.attr("data-id");
            let _thisclasses = currentElement.attr("class");
            _thisclasses = _thisclasses.replace(" select2-hidden-accessible", " ");
            currentElement.siblings("span.select2").remove();
            currentElement.remove();
            let _thisae = typeof currentElement.data("ae") != "undefined" ? (currentElement.data("ae") == false ? " data-ae='false' " : "") : "";
            let _thisad = typeof currentElement.data("ad") != "undefined" ? (currentElement.data("ad") == false ? " data-ad='false' " : "") : "";

            _thisTd.html(`<select ` + isdisabled + _thisae + _thisad + ` class="` + _thisclasses + `" data-control="select2" name=` + fName + ` id=` + newFldName + ` value="" data-id=` + dtId + ` data-placeholder="Select an option" data-allow-clear="true"></select>`);
        } else {
            var attVal = "data-clk";
            id = currentElement.attr(attVal);
            if (id != undefined)
                currentElement.attr(attVal, $j(this).parents(".autoinput-parent").find("input").attr("id"));
            else {
                var fName = currentElement.attr("name");
                var prevFldName = fName + "R" + rowNo;
                var newFldName = fName + "R" + (parseInt(rowNo) + 1);
                currentElement.attr("id", newFldName);
                currentElement.removeAttr('value').attr('value', '');
            }
        }
    });
    $("#" + thisTblId + " tbody tr:last").attr("id", (parseInt(rowNo) + 1).toString());
    if (isAddRowClk != '') {
        let _thisRNo = (parseInt(rowNo) + 1).toString();
        let _thisFldId = $("#hdnfieldId").val();
        let _thisFldRowNo = callParentNew("GetFieldsRowNo(" + _thisFldId + ")", "function");
        $(colNames).each(function (iIndex, sElement) {
            if (colSource[iIndex] != "") {
                let _thisEleName = sElement + "R" + _thisRNo;
                let _sourceFld = colSource[iIndex];
                let _sourFldInd = callParentNew("GetFieldIndex(" + _sourceFld + ")", "function");
                let _sourceDc = callParentNew("GetDcNo(" + _sourceFld + ")", "function");
                let _thisthisFldRowNo = _thisFldRowNo;
                if (!callParentNew("IsDcGrid(" + _sourceDc + ")", "function")) {
                    _thisthisFldRowNo = "000";
                }
                let sourceFldId = _sourceFld + _thisthisFldRowNo + "F" + _sourceDc;
                let _colValue = "";
                if (_sourceFld != "")
                    _colValue = $(callParentNew(sourceFldId)).val();
                if (typeof _colValue != "undefined" && _colValue != "" && $("#" + _thisEleName).length > 0) {
                    $("#" + _thisEleName).val(_colValue);
                }
            }
        });
    }
    LoadTableEvents("#divTable");

    setTimeout(function () {
        totalOnLoadtstTable();
    }, 0);
}

function MainBlur(obj, type) {
    let fldInd = colNames.indexOf(obj.attr("name"));
    if (typeof colVExp[fldInd] != "undefined" && colVExp[fldInd] != "") {
        var vExp = colVExp[fldInd].replace(/,/g, "♦");
        var vRowNo = obj.attr("id").replace(obj.attr("name") + "R", '');
        var indx = $j.inArray(obj.attr("name"), parent.ChangedTblFields);
        /*var cellVal = parent.ChangedTblFieldVals[indx].split('~');*/
        var cellVal = String(parent.ChangedTblFieldVals[indx]).split('~');       
        cellVal[vRowNo] = $(obj).val();
        parent.ChangedTblFieldVals[indx] = cellVal.join("~");

        var fResult = callParentNew("Evaluate(" + obj.attr("name") + "," + vRowNo + "," + vExp + ",vexpr,table)", "function");
        if (fResult != 'T' && fResult != 't' && fResult != true) {
            var cutMsg = eval(callParent('lcm[52]'));
            var firstChar = fResult.substring(0, 1);
            var alertMsg = fResult.substring(1);
            if (firstChar == "_") {
                showAlertDialog("error", alertMsg);
            } else if (firstChar == "?") {
                showAlertDialog("error", alertMsg);
                if (type == "input") {
                    $(obj).val($(obj).val());
                    $(obj).attr("value", $(obj).val());
                }
                else {
                    let ddlValue = $j(obj).val();
                    if ($(obj).hasClass("tblFromSelect")) {
                        ddlValue = ddlValue == null ? "" : ddlValue;
                        $(obj).append('<option value="' + ddlValue + '" selected="selected">' + ddlValue + '</option>');
                        $(obj).attr("value", ddlValue);
                    } else {
                        $j(obj).attr("selected", "selected");
                        $(obj).attr("value", ddlValue);
                    }
                }
                return false;
            } else if (firstChar == "*") {
                showAlertDialog("error", alertMsg);
                return false;
            } else {
                if (fResult == "MessageSetAxFont") {
                    return true;
                } else if (confirm(fResult + ". " + cutMsg)) {
                    if (type == "input") {
                        $(obj).val($(obj).val());
                        $(obj).attr("value", $(obj).val());
                    }
                    else {
                        let ddlValue = $j(obj).val();
                        if ($(obj).hasClass("tblFromSelect")) {
                            ddlValue = ddlValue == null ? "" : ddlValue;
                            $(obj).append('<option value="' + ddlValue + '" selected="selected">' + ddlValue + '</option>');
                            $(obj).attr("value", ddlValue);
                        } else {
                            $j(obj).attr("selected", "selected");
                            $(obj).attr("value", ddlValue);
                        }
                    }

                    return false;
                } else {
                    return true;
                }
            }
        }

        TableEvalonBlur(indx, vRowNo);
    }
    else {
        if (type == "input") {
            $(obj).val($(obj).val());
            $(obj).attr("value", $(obj).val());
        }
        else {
            let ddlValue = $j(obj).val();
            if ($(obj).hasClass("tblFromSelect")) {
                ddlValue = ddlValue == null ? "" : ddlValue;
                $(obj).append('<option value="' + ddlValue + '" selected="selected">' + ddlValue + '</option>');
                $(obj).attr("value", ddlValue);
            } else {
                $j(obj).attr("selected", "selected");
                $(obj).attr("value", ddlValue);
            }
        }

        var vRowNo = obj.attr("id").replace(obj.attr("name") + "R", '');
        var indx = $j.inArray(obj.attr("name"), parent.ChangedTblFields);
        //var cellVal = parent.ChangedTblFieldVals[indx].split('~');
        var cellVal = String(parent.ChangedTblFieldVals[indx]).split('~');       
        cellVal[vRowNo] = $(obj).val();
        parent.ChangedTblFieldVals[indx] = cellVal.join("~");

        TableEvalonBlur(indx, vRowNo);
    }

    Aportion(obj);
    setTimeout(function () {
        totalOnLoadtstTable();
    }, 0);
}

function TableEvalonBlur(fldInd, fRowNo) {
    if (typeof colfExp[fldInd] != "undefined" && colfExp[fldInd] == "") {
        for (var i = fldInd; i < colfExp.length; i++) {
            if (typeof colfExp[i] != "undefined" && colfExp[i] != "") {
                var fExp = colfExp[i].replace(/,/g, "♦");
                var obj = $("#" + colNames[i] + "R" + fRowNo);
                //var fRowNo = obj.attr("id").replace(obj.attr("name"), '');
                var indx = $j.inArray(obj.attr("name"), parent.ChangedTblFields);
                /* var cellVal = parent.ChangedTblFieldVals[indx].split('~');*/
                var cellVal = String(parent.ChangedTblFieldVals[indx]).split('~');       
                cellVal[fRowNo] = $(obj).val();
                parent.ChangedTblFieldVals[indx] = cellVal.join("~");

                var fResult = callParentNew("Evaluate(" + obj.attr("name") + "," + fRowNo + "," + fExp + ",expr,table)", "function");
                if (fResult != '') {
                    let type = $(obj).attr("type");
                    if (type == "text") {
                        $(obj).val(fResult);
                        $(obj).attr("value", fResult);
                    }
                    else {
                        let ddlValue = fResult;
                        $j(obj).attr("selected", "selected");
                        $(obj).attr("value", ddlValue);
                    }
                    cellVal[fRowNo] = fResult;
                    parent.ChangedTblFieldVals[indx] = cellVal.join("~");
                }
            }
        }
    }
}

function UpdateArray(fldName, fldValue) {

    var isAlreadyFound = false;
    for (var x = 0; x < doParentOpInIframe("ChangedFields", "rtn").length; x++) {

        var fName = doParentOpInIframe("ChangedFields[" + x + "]", "rtn").toString();
        if (fName == fldName) {
            if (fldValue == "***") {
                doParentOpInIframe("ChangedFields", "rtn").splice(x, 1);
                doParentOpInIframe("ChangedFieldDbRowNo", "rtn").splice(x, 1);
                doParentOpInIframe("ChangedFieldValues", "rtn").splice(x, 1);
                doParentOpInIframe().ChangedFieldOldValues.splice(x, 1);
            }
            else {
                doParentOpInIframe("ChangedFieldOldValues[" + x + "]", "var", doParentOpInIframe("ChangedFieldValues[" + x + "]", "rtn").toString());
                doParentOpInIframe("ChangedFieldDbRowNo[" + x + "]", "var", doParentOpInIframe("ChangedFieldDbRowNo[" + x + "]", "rtn"));
                doParentOpInIframe("ChangedFieldValues[" + x + "]", "var", fldValue);
            }
            isAlreadyFound = true; // the field name is already found and updated.
            break;
        }
    }

    if ((!isAlreadyFound) && (fldValue != "***")) {
        var fIndx = fldName.lastIndexOf("F");
        var rowNo = fldName.substring(fIndx - 3, fIndx);
        var dcNo = fldName.substring(fIndx + 1);
        var dbRowNo = callParentNew("GetDbRowNo(" + rowNo + "," + dcNo + ")", "function");
        parent.ChangedFields.push(fldName);
        parent.ChangedFieldDbRowNo.push(dbRowNo);
        parent.ChangedFieldValues.push(fldValue);
        parent.ChangedFieldOldValues.push("");
    }
}

function CheckSpecialCharsInStr(str) {

    var str = str;
    if (str != undefined) {
        str = str.replace(/&/g, "&amp;");
        str = str.replace(/</g, "&lt;");
        str = str.replace(/>/g, "&gt;");
        str = str.replace(/'/g, "&apos;");
        str = str.replace(/"/g, '&quot;');
    }
    else {
        str = "";
    }
    return str;
}

function RepSpecialCharsInHTML(str) {
    str = str.replace(/&amp;/g, "&");
    str = str.replace(/&lt;/g, "<");
    str = str.replace(/&gt;/g, ">");
    str = str.replace(/&apos;/g, "'");
    str = str.replace(/&quot;/g, '"');
    return str;
}

function CheckAll(obj) {
    $j("input[name=chkItem]:checkbox").each(function () {
        $j(this).prop("checked", obj.checked);
    });
}

function ChkHdrCheckbox(obj, exprResult) {
    if ($j(".fgChk:visible").length == $j(".fgChk:checked").length)
        $j(".fgHdrChk").prop("checked", true);
    else
        $j(".fgHdrChk").prop("checked", false);
}

function GetTblFldAutoData(thisId, thisfldId) {
    fldAutoData = [];
    var custData = callParentNew("AxGetCustSelectFldData(" + thisfldId + ")", "function");
    if (custData != "") {
        fldAutoList = thisfldId;
        var JsonData = JSON.parse(custData);
        var fldData = JsonData.pickdata[3].data;
        var aSearch = [];
        $(fldData).each(function (iIndex, sElement) {
            sElement.i = sElement.i.replace(/\^\^dq/g, '"');
            aSearch.push(sElement);
        });
        if (aSearch.length != 0) {
            var result = ($.map(aSearch, function (item) {
                item.i = item.i.replace(/\^\^dq/g, '"');
                return {
                    //id: item.v == "" ? item.i : item.v,
                    //text: item.i
                    id: item.i,
                    text: item.i,
                }
            }))
            fldAutoData = result;
        } else {
            var cutMsg = eval(callParent('lcm[0]'));
            fldAutoData = { results: "", noResults: cutMsg };
        }
    }
}

function Aportion(thisChangedFld) {
    let fldInd = colNames.indexOf(thisChangedFld.attr("name"));
    let fldName = thisChangedFld.attr("name");
    let apAgainst = aportionAgainst.indexOf(fldName);
    if (apAgainst > -1) {
        let fldApName = colNames[apAgainst];
        var fldApVa = 0;
        $('[name="' + fldApName + '"]').each(function () {
            fldApVa = parseInt(fldApVa) + parseInt($(this).val() == "" ? 0 : $(this).val());
        });
        var thisVal = $(thisChangedFld).val();
        if (thisVal != "") {
            var adjVal = $(thisChangedFld).parents("tr").find("[name='" + fldApName + "']").val();
            var adjId = $(thisChangedFld).parents("tr").find("[name='" + fldApName + "']").attr("id");
            if (parseInt(thisVal) <= parseInt(adjVal)) {
                $($("#" + adjId)).val(thisVal);
                $($("#" + adjId)).attr("value", thisVal);
                var nxtAdjVal = $(thisChangedFld).parents("tr").next("tr").find("[name='" + fldApName + "']").val();
                var nxtAdjId = $(thisChangedFld).parents("tr").next("tr").find("[name='" + fldApName + "']").attr("id");

                var fldAfterApVa = 0;
                $('[name="' + fldApName + '"]').each(function () {
                    fldAfterApVa = parseInt(fldAfterApVa) + parseInt($(this).val() == "" ? 0 : $(this).val());
                });

                if (nxtAdjVal == "" && parseInt(aportionWhatVal[apAgainst]) > parseInt(thisVal)) {
                    let nxtVal = 0;
                    if (aportionWhatVal[apAgainst] == fldApVa)
                        nxtVal = aportionWhatVal[apAgainst] - parseInt(fldAfterApVa);
                    else
                        nxtVal = aportionWhatVal[apAgainst] - (fldApVa + parseInt(thisVal));
                    $($("#" + nxtAdjId)).val(nxtVal);
                    $($("#" + nxtAdjId)).attr("value", nxtVal);
                }
                else if (nxtAdjVal != "" && parseInt(aportionWhatVal[apAgainst]) > parseInt(fldApVa)) {
                    var ss = nxtAdjVal;
                }
            }
            else if ((adjVal == "" || adjVal == "0") && parseInt(aportionWhatVal[apAgainst]) != parseInt(fldApVa)) {
                $($("#" + adjId)).val(thisVal);
                $($("#" + adjId)).attr("value", thisVal);
            }
            else if (adjVal != "" && aportionWhatVal[apAgainst] == "") {
                $($("#" + adjId)).val(thisVal);
                $($("#" + adjId)).attr("value", thisVal);
            }
            else if (parseInt(aportionWhatVal[apAgainst]) != parseInt(fldApVa)) {
                let balVal = parseInt(aportionWhatVal[apAgainst]) - parseInt(fldApVa);
                var thisAdjVal = $(thisChangedFld).parents("tr").find("[name='" + fldApName + "']").val();
                balVal = balVal + parseInt(thisAdjVal);
                if (thisVal >= balVal) {
                    $($("#" + adjId)).val(balVal);
                    $($("#" + adjId)).attr("value", balVal);
                }
                else {
                    $($("#" + adjId)).val(thisVal);
                    $($("#" + adjId)).attr("value", thisVal);
                }
            }
        }
    }
}

function SqlFillData() {
    if (isQuery == "true") {
        var jsonVal = "";
        try {
            $.ajax({
                url: 'tsttable.aspx/GetSqlFillData',
                type: 'POST',
                cache: false,
                async: true,
                data: JSON.stringify({
                    tstDId: callParentNew("tstDataId"), fldName: $("#hdnfieldId").val()
                }),
                dataType: 'json',
                contentType: "application/json",
                success: function (data) {
                    if (data.d != "") {
                        jsonVal = JSON.parse(data.d);
                        for (var l = 0; l < jsonVal.length; l++) {
                            var varData = jsonVal[l];
                            for (var keyData in varData) {
                                for (var key in varData[keyData]) {
                                    let KeyName = key;

                                    var obj = $($("#" + KeyName + "R" + l));
                                    if (obj.length == 0) {
                                        AddTableRows('fldTable');
                                        obj = $($("#" + KeyName + "R" + l));
                                    }
                                    let KeyValue = varData[keyData][key];
                                    let type = $(obj).attr("type");
                                    if (type == "text") {
                                        $(obj).val(KeyValue);
                                        $(obj).attr("value", KeyValue);
                                    }
                                    else {
                                        let ddlValue = KeyValue;
                                        if (obj.hasClass("tblFromSelect")) {
                                            ddlValue = ddlValue == null ? "" : ddlValue;
                                            $(obj).append('<option value="' + ddlValue + '" selected="selected">' + ddlValue + '</option>');
                                            $(obj).attr("value", ddlValue);
                                        }
                                        else {
                                            $(obj).attr("selected", "selected");
                                            $(obj).attr("value", ddlValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else {
                        jsonVal = "";
                    }
                },
                error: function (error) {
                    jsonVal = "";
                }
            });
        }
        catch (ex) { }
    }
    else if (isQuery == "sqlDescr" && QueryData != "") {
        jsonVal = JSON.parse(QueryData);
        for (var l = 0; l < jsonVal.length; l++) {
            var varData = jsonVal[l];
            for (var keyData in varData) {
                for (var key in varData[keyData]) {
                    let KeyName = key;

                    var obj = $($("#" + KeyName + "R" + l));
                    if (obj.length == 0) {
                        AddTableRows('fldTable');
                        obj = $($("#" + KeyName + "R" + l));
                    }
                    let KeyValue = varData[keyData][key];
                    let type = $(obj).attr("type");
                    if (type == "text") {
                        $(obj).val(KeyValue);
                        $(obj).attr("value", KeyValue);
                    }
                    else {
                        let ddlValue = KeyValue;
                        if (obj.hasClass("tblFromSelect")) {
                            ddlValue = ddlValue == null ? "" : ddlValue;
                            $(obj).append('<option value="' + ddlValue + '" selected="selected">' + ddlValue + '</option>');
                            $(obj).attr("value", ddlValue);
                        }
                        else {
                            $(obj).attr("selected", "selected");
                            $(obj).attr("value", ddlValue);
                        }
                    }
                }
            }
        }
    }
}

function GetstrTableJson(descrName) {
    var jsonVal = "";
    try {
        $.ajax({
            url: 'tsttable.aspx/GetstrTableJson',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                descrName: descrName, tstDId: callParentNew("tstDataId"), fldName: $("#hdnfieldId").val()
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                if (data.d != "") {
                    let fullData = data.d;
                    QueryData = fullData.split('♥')[1];
                    fullData = fullData.split('♥')[0];
                    if (fullData != "")
                        jsonVal = fullData;
                }
                else {
                    jsonVal = "";
                }
            },
            error: function (error) {
                jsonVal = "";
            }
        });
    }
    catch (ex) { }
    return jsonVal;
}

function GetAutoCompData(fldNameAc, value, curPageNo, AutPageSize) {
    var includeDcs = "";
    if (arrRefreshDcs.length > 0) {
        for (var i = 0; i < arrRefreshDcs.length; i++) {
            var arrDcNos = arrRefreshDcs[i].split(':');
            includeDcs = arrDcNos[1].replace("dc", "") + ',' + arrDcNos[0].replace("dc", "");
        }
    }
    value = CheckSpecialCharsInStr(value);
    var fldDcNo = callParentNew("GetFieldsDcNo(" + fldNameAc + ")", "function");

    AxActiveRowNo = parseInt(callParentNew("GetFieldsRowNo(" + fldNameAc + ")", "function"), 10);
    AxActiveRowNo = callParentNew("GetDbRowNo(" + AxActiveRowNo + "," + fldDcNo + ")", "function");
    var activeRow = AxActiveRowNo;

    var parStr = "";
    if (AxActivePRow != "" && AxActivePDc != "")
        parStr = AxActivePDc + "♠" + AxActivePRow;

    var subStr = "";
    return curPageNo.toString() + "~" + AutPageSize.toString() + "~" + fldDcNo + "~" + activeRow + "~" + parStr + "~" + subStr + "~" + includeDcs;
}

function GetDBProcTableJson(procName) {
    var jsonVal = "";
    try {
        let _fldName = $("#hdnfieldId").val().substring(0, $("#hdnfieldId").val().lastIndexOf("F") - 3);
        let _fldRowNo = callParentNew("GetFieldsRowNo(" + $("#hdnfieldId").val() + ")", "function");
        let _dsfDc = callParentNew("GetDcNo(" + _fldName + ")", "function");
        let _fldCltRowNo = callParentNew("GetDbRowNo(" + _fldRowNo + "," + _dsfDc + ")", "function");
        $.ajax({
            url: 'tsttable.aspx/GetDBProcTableJson',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                procName: procName, tstDId: callParentNew("tstDataId"), fldName: $("#hdnfieldId").val(), ChangedFields: parent.ChangedFields, ChangedFieldDbRowNo: parent.ChangedFieldDbRowNo, ChangedFieldValues: parent.ChangedFieldValues, DeletedDCRows: parent.DeletedDCRows, tfldDc: _dsfDc, tfldRowNo: _fldCltRowNo
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                if (data.d != "") {
                    let fullData = data.d;
                    QueryData = fullData.split('♥')[1];
                    fullData = fullData.split('♥')[0];
                    if (fullData != "")
                        jsonVal = fullData;
                }
                else {
                    jsonVal = "";
                }
            },
            error: function (error) {
                jsonVal = "";
            }
        });
    }
    catch (ex) { }
    return jsonVal;
}

function GetSourceParamValue(_thisFld) {
    var _thisFinalVal = "";
    let _thisRowNo = typeof _thisFld.parents("tr").attr("id") == "undefined" ? "0" : _thisFld.parents("tr").attr("id");
    let _thisName = _thisFld.attr("name");
    $(colNames).each(function (iIndex, sElement) {
        if (colSource[iIndex] != "" && _thisName != sElement) {
            let _thisEleName = sElement + "R" + _thisRowNo;
            if ($("#" + _thisEleName).is("select") && typeof $("#" + _thisEleName).attr("value") != "undefined" && $("#" + _thisEleName).attr("value") != "")
                _thisFinalVal += _thisFinalVal == "" ? colSource[iIndex] + ":" + $("#" + _thisEleName).attr("value") : "♠" + colSource[iIndex] + ":" + $("#" + _thisEleName).attr("value");
            else if ($("#" + _thisEleName).is("input") && $("#" + _thisEleName).attr("type") == "text" && $("#" + _thisEleName).attr("value") != "")
                _thisFinalVal += _thisFinalVal == "" ? colSource[iIndex] + ":" + $("#" + _thisEleName).attr("value") : "♠" + colSource[iIndex] + ":" + $("#" + _thisEleName).attr("value");
        }
    });
    return _thisFinalVal;
}

function isEmptyOrBlankArray(arr) {
    return arr.length === 0 || arr.every(item => item.trim() === '');
}

function totalOnLoadtstTable() {
    if (!isEmptyOrBlankArray(tblFldFTot)) {
        colNames.forEach(thisColName => {
            const _obj = $("#fldTable").find(`[name='${thisColName}']`);
            totalFooterInTable(_obj)
        });
    }
}

function totalFooterInTable(_obj) {
    try {
        let _eleInd = colNames.indexOf(_obj.attr("name"));
        if (typeof tblFldFTot != "undefined" && _eleInd > -1 && tblFldFTot[_eleInd] != "") {
            let _inputString = tblFldFTot[_eleInd];

            const regex = /:(\w+)/g;
            let match;
            let _matchinflds = [];
            while ((match = regex.exec(_inputString)) !== null) {
                _matchinflds.push(match[1]);
            }

            let matchingThIndexes = [];
            _matchinflds.forEach(field => {
                let matched = false;
                $(_obj).parents("table").find("thead tr th").each(function (index) {
                    if (index != 0 && colNames[index - 1] === field.toLowerCase()) {
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
            $(_obj).parents("table").find("tbody tr").each(function () {
                matchingThIndexes.forEach((index, i) => {
                    if (index != -1) {
                        let tdValue = $(this).find(`td:eq(${index}) input,td:eq(${index}) select`).val().trim();
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
                const _index = colNames.findIndex(col => col.toLowerCase() === val.toLowerCase());
                _inputString = _inputString.replace(":" + val, _forTotVal);
                if (parent.$("#modalIdTableField .modal-footer .dvfTotal").length === 0) {
                    parent.$("#modalIdTableField .modal-footer").prepend(`<span class="dvfTotal ms-0 me-auto"></span>`);
                }
                const $totalContainer = parent.$("#modalIdTableField .modal-footer .dvfTotal");
                const $existing = $totalContainer.find(`#dvfgTotal${_index}`);
                if ($existing.length > 0) {
                    $existing.text(_inputString);
                } else {
                    $totalContainer.append(`<span id="dvfgTotal${_index}" style="font-weight:600;display:inline-block;margin-right:10px;">${_inputString}</span>`);
                }
                const spans = $totalContainer.find("span[id^='dvfgTotal']").get();
                spans.sort((a, b) => {
                    const aIndex = parseInt(a.id.replace("dvfgTotal", ""), 10);
                    const bIndex = parseInt(b.id.replace("dvfgTotal", ""), 10);
                    return aIndex - bIndex;
                });
                $totalContainer.append(spans);
            });
        } else if (_eleInd == -1 && parent.$("#modalIdTableField .modal-footer .dvfTotal").length > 0) {
            parent.$("#modalIdTableField .modal-footer .dvfTotal").remove();
        }
    } catch (ex) { }
}