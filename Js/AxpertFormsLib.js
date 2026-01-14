
//Custom tstruct functions to call from custom/external javascript files

//Returns the Field Value. If RowNo is 0 and Field is a grid Field, returns a an array of values.
function AxpGetFieldValue(fieldName, rowNo) {
    if (fieldName == "")
        return "Field Name should not empty."
    else {
        let fldIndex = $j.inArray(fieldName, FNames);
        if (fldIndex > -1) {
            let isGrid = IsGridField(fieldName);
            let dcNo = GetDcNo(fieldName);
            if (isGrid == true) {
                if (rowNo != "0") {
                    let _thisRClient = GetClientRowNo(rowNo, dcNo);
                    let _thisFld = fieldName + _thisRClient + "F" + dcNo;
                    return GetFieldValue(_thisFld);
                } else {
                    if ($("#gridHd" + dcNo + " tbody tr").length > 0) {
                        var _allRowFldVal = new Array;
                        $("#gridHd" + dcNo + " tbody tr").each(function () {
                            let _trId = $(this).attr("id");
                            _trId = _trId.substr(_trId.lastIndexOf("F") - 3, 3);
                            let _thisFld = fieldName + _trId + "F" + dcNo;
                            _allRowFldVal.push(GetFieldValue(_thisFld));
                        });
                        return _allRowFldVal;
                    }
                }
            } else {
                let _thisFld = fieldName + "000F" + dcNo;
                return GetFieldValue(_thisFld);
            }
        }
    }
}

//It sets the fieldvalue and updates dependent fields
function AxpSetFieldValue(fieldName, rowNo, value, updateDependents) {
    if (fieldName == "")
        return "Field Name should not empty."
    else {
        let fldIndex = $j.inArray(fieldName, FNames);
        if (fldIndex > -1) {
            let isGrid = IsGridField(fieldName);
            let dcNo = GetDcNo(fieldName);
            if (isGrid == true) {
                let _thisRClient = GetClientRowNo(rowNo, dcNo);
                let _thisFld = fieldName + _thisRClient + "F" + dcNo;
                SetFieldValue(_thisFld, value);
                if (IsDcGrid(dcNo) && isGrdEditDirty)
                    UpdateFieldArray(axpIsRowValid + dcNo + _thisRClient + "F" + dcNo, GetDbRowNo(_thisRClient, dcNo), "", "parent", "AddRow");
                UpdateFieldArray(_thisFld, _thisRClient, value, "parent");
                if (updateDependents == "true") {
                    CheckDependencyPerf(_thisFld);
                }
            } else {
                let _thisFld = fieldName + "000F" + dcNo;
                SetFieldValue(_thisFld, value);
                UpdateFieldArray(_thisFld, "000", value, "parent");
                if (updateDependents == "true") {
                    CheckDependencyPerf(_thisFld);
                }
            }
        }
    }
}

//Returns the number of rows in DC.
function AxpGetRowCount(dcName) {
    if (dcName == "")
        return "DC Name should not empty."
    else {
        let _thisDcNo = dcName.substr(2, dcName.length);
        if (_thisDcNo != "") {
            return GetDcRowCount(_thisDcNo);
        }
    }
}

//Add row to the DC.
function AxpAddRow(dcName) {
    if (dcName == "")
        return "DC Name should not empty."
    else {
        let _thisDcNo = dcName.substr(2, dcName.length);
        if (_thisDcNo != "") {
            $("#gridAddBtn" + _thisDcNo).click();
        }
    }
}

//Delete row from Grid Dc. If RowNo is 0, delete all rows.
function AxpDeleteRow(dcName, rowNo) {
    if (dcName == "")
        return "DC Name should not empty."
    else {
        let _thisDcNo = dcName.substr(2, dcName.length);
        if (_thisDcNo != "") {
            if (axInlineGridEdit)
                updateInlineGridRowValues();
            if (rowNo != "0") {
                if ($("#gridHd" + _thisDcNo + " tbody tr").length >= rowNo) {
                    let _rowFrmNo = GetClientRowNo(rowNo, _thisDcNo)
                    DeleteGridRow(_thisDcNo, _rowFrmNo, undefined);
                }
            }
            else {
                let _RowCount = GetDcRowCount(_thisDcNo);
                DeleteAllRows(_thisDcNo, _RowCount);
            }
        }
    }
}

//Returns a JSON to call SaveData REST service.
function AxpGetDataJSON() {
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
                let _thisVal = GetFieldValueNew(_thisFld);
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
                $("#gridHd" + _thisDcNo + " tbody tr").each(function (rind, rele) {
                    let _thisRNo = $(rele).attr("id");
                    _thisRNo = _thisRNo.substring(_thisRNo.indexOf('F'), _thisRNo.indexOf('F') - 3);
                    let _RecVal = $("#axp_recid" + _thisDcNo + _thisRNo + "F" + _thisDcNo).val();
                    fldJSON += "{\"rowno\":\"" + _thisRNo + "\",\"text\":\"" + _RecVal + "\",\"columns\":{";
                    let fldValJson = "";
                    _thisDcFld.forEach(function (flele) {
                        if (flele.startsWith('axp_recid'))
                            return;
                        let _thisFld = flele + _thisRNo + "F" + _thisDcNo;
                        let _thisVal = GetFieldValueNew(_thisFld);
                        fldValJson += "\"" + flele + "\":\"" + _thisVal + "\",";
                    });
                    if (fldValJson != "")
                        fldJSON += fldValJson.substr(0, fldValJson.length - 1);
                    fldJSON += "}},";
                });
                fldJSON = fldJSON.substr(0, fldJSON.length - 1);
                fldJSON += "]},";
            }
        }
    });
    fldJSON = fldJSON.substr(0, fldJSON.length - 1);
    fldJSON += "]";

    return fldJSON + "&*&" + changedDcs + "&*&" + recordid;
}

//Gets drop down values for Field and row no. 
function AxpGetDropDown(fieldName, rowNo) {
    if (fieldName == "")
        return "Field Name should not empty."
    else {
        let fldIndex = $j.inArray(fieldName, FNames);
        if (fldIndex > -1) {
            let isGrid = IsGridField(fieldName);
            let dcNo = GetDcNo(fieldName);
            if (isGrid == true) {
                let _thisRClient = GetClientRowNo(rowNo, dcNo);
                let _thisFld = fieldName + _thisRClient + "F" + dcNo;
                return AxGetCustSelectFldData(_thisFld)
            } else {
                let _thisFld = fieldName + "000F" + dcNo;
                return AxGetCustSelectFldData(_thisFld)
            }
        }
    }
}

//Clears Value from all controls like new button click.
function AxpClearform() {
    NewTstruct();
}

//Returns an array of fieldname, datatype, width, component type, hidden, enabled, caption. If DCNo is 0, all fields are returned.
function AxpGetFieldList(dcName = "") {
    //FNames    //FDataType    //FMaxLength    //FFieldType    //FFieldHidden    //FFieldReadOnly    //FCaption
    var fldDetails = new Array();
    if (dcName == "") {
        FNames.forEach(function (_thisFld, fldIndex) {
            let _fldDts = FNames[fldIndex] + "~" + FDataType[fldIndex] + "~" + FMaxLength[fldIndex] + "~" + FFieldType[fldIndex] + "~" + FFieldHidden[fldIndex] + "~" + FFieldReadOnly[fldIndex] + "~" + FCaption[fldIndex];
            fldDetails.push(_fldDts);

        });
    }
    else {
        let _thisDcNo = dcName.substr(2, dcName.length);
        let _thisDcFlds = GetGridFields(_thisDcNo);
        _thisDcFlds.forEach(function (_thisFld) {
            let fldIndex = $j.inArray(_thisFld, FNames);
            let _fldDts = FNames[fldIndex] + "~" + FDataType[fldIndex] + "~" + FMaxLength[fldIndex] + "~" + FFieldType[fldIndex] + "~" + FFieldHidden[fldIndex] + "~" + FFieldReadOnly[fldIndex] + "~" + FCaption[fldIndex];
            fldDetails.push(_fldDts);

        });
    }
    return fldDetails;
}

//Calculates all formula in given DC and updates in field controls.
function AxpRefreshCalcualtions(dcName, rowNo) {
    if (dcName == "")
        return "DC Name should not empty."
    else {
        let _thisDcNo = dcName.substr(2, dcName.length);
        if (_thisDcNo != "") {
            if (IsDcGrid(_thisDcNo)) {
                if (rowNo != "0") {
                    let _thisRClient = GetClientRowNo(rowNo, _thisDcNo);
                    let _thisDcFlds = GetGridFields(_thisDcNo);
                    _thisDcFlds.forEach(function (_thisFld) {
                        let _thisFldId = _thisFld + _thisRClient + "F" + _thisDcNo;
                        EvaluateAxFunction(_thisFld, _thisFldId, _thisRClient + "F" + _thisDcNo);
                        EvaluateExpressions(_thisFldId);
                    });
                } else {
                    let _thisDcFlds = GetGridFields(_thisDcNo);
                    if ($("#gridHd" + _thisDcNo + " tbody tr").length > 0) {
                        $("#gridHd" + _thisDcNo + " tbody tr").each(function () {
                            let _trId = $(this).attr("id");
                            _trId = _trId.substr(_trId.lastIndexOf("F") - 3, 3);
                            _thisDcFlds.forEach(function (_thisFld) {
                                let _thisFldId = _thisFld + _trId + "F" + _thisDcNo;
                                EvaluateAxFunction(_thisFld, _thisFldId, _trId + "F" + _thisDcNo);
                                EvaluateExpressions(_thisFldId);
                            });
                        });
                    }
                }
            } else {
                let _thisDcFlds = GetGridFields(_thisDcNo);
                _thisDcFlds.forEach(function (_thisFld) {
                    let _thisFldId = _thisFld + "000F" + _thisDcNo;
                    EvaluateAxFunction(_thisFld, _thisFldId, "000F" + _thisDcNo);
                    EvaluateExpressions(_thisFldId);
                });
            }
        }
    }
}


//Returns prop value. Only frontend props are accessible.
function AxpGetFieldProperty(fieldName, propertyName) {

}

