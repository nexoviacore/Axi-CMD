var xlresult;
var xlGridfieldNames;
var xlDcNo
var xlRowCount;
var IsgridummyRow =false;

function ImportGridData(isDataExist) {
    var crvalue = "";
       
    var arrImportcrValue = [];
    var arrImportrecID = [];
    var arrImportgridValues = [];
    var keyimport = "";
    var valueimport = "";
    var arrImportFldNames = [];

    if (callParentNew("gridDummyRowVal").length > 0) {
        callParentNew("gridDummyRowVal").map(function (v) {
            if (v.split("~")[0] == xlDcNo)
                IsgridummyRow = true
        });
    }
    if (typeof xlresult == "undefined") {
        showAlertDialog("error", "File does not have the data");
        return;
    }

    let xmlOutput = ExcelImpJsonToXML(xlresult);
    callParentNew("closeModalDialog()", "function");
    parent.ExcelImportGridCall(xmlOutput, xlDcNo, isDataExist);
    return;

    for (var l = 1; l < xlresult.length; l++) {
        if (xlRowCount > 0 && !IsgridummyRow) {
            var excelRowCountcheck = xlRowCount + l;
            crvalue += "i" + excelRowCountcheck + ",";
            arrImportrecID.push({ n: xlGridfieldNames[0], v: "0", r: excelRowCountcheck.toString(), t: "s" });
        }
        else {
            crvalue += "i" + l + ",";
            arrImportrecID.push({ n: xlGridfieldNames[0], v: "0", r: l.toString(), t: "s" });
        }

        for (var k = 0; k < xlresult[l].length; k++) {
            valueimport = typeof xlresult[l][k] != "undefined" ? xlresult[l][k] : "";
            keyimport = xlresult[0][k];
            if (keyimport.indexOf('(') > -1 && keyimport.endsWith(')')) {
                keyimport = keyimport.split('(')[1].split(')')[0];
            }

            if (valueimport != "")
                valueimport = valueimport.toString();
            if (xlRowCount > 0 && !IsgridummyRow) {
                var excelRowCount = xlRowCount + l;
                arrImportgridValues.push({ n: keyimport, v: valueimport, r: excelRowCount.toString(), t: "s" });
            }
            else
                arrImportgridValues.push({ n: keyimport, v: valueimport, r: l.toString(), t: "s" });
        }
    }
    for (var d = 0; d < xlresult[0].length; d++) {
        let _keyimport = xlresult[0][d];
        if (_keyimport.indexOf('(') > -1 && _keyimport.endsWith(')')) {
            _keyimport = _keyimport.split('(')[1].split(')')[0];
        }
        arrImportFldNames.push(_keyimport);
    }
    crvalue = crvalue.slice(0, -1);
    arrImportcrValue.push({ n: "DC" + xlDcNo, v: xlDcNo, cr: crvalue, t: "dc", hasdatarows: "yes" });
    var xlFinalimport = [];
    xlFinalimport = xlFinalimport.concat(arrImportcrValue, arrImportrecID, arrImportgridValues);
    callParentNew("AxActiveDc=", xlDcNo);
    try {
        if (callParentNew("gridDummyRowVal").length > 0) {
            callParentNew("gridDummyRowVal").map(function (v) {
                if (v.split("~")[0] == xlDcNo)
                    callParentNew("gridDummyRowVal").splice($.inArray(v, callParentNew("gridDummyRowVal")), 1);
            });
        }
    }
    catch (ex) { }
    // }
    parent.ExecData(xlFinalimport, "LoadData", true);
    callParentNew("closeModalDialog()", "function");
    var rowCount = parent.$("#gridHd" + xlDcNo + " tbody tr").length;// xlresult.length - 1;
    var rowno = "";
    let _AxpFormula = false;

    var _fldArrayList = new Array;
    for (var k = 1; k <= rowCount; k++) {
        callParentNew("AxActiveRowNo=", k);
        if (k <= 9) {
            rowno = "00" + k;
        }
        else if (k > 9 && k <= 99) {
            rowno = "0" + k;
        }
        else {
            rowno = k;
        }
        for (var i = 0; i < xlGridfieldNames.length; i++) {
            if (xlGridfieldNames[i].toLowerCase() == "axp_formula")
                _AxpFormula = true;

            if (k == 1) {
                let _thisEle = xlFinalimport.filter(item => item.n === xlGridfieldNames[i] && (item.v == "0" || item.v == ""));
                if (_thisEle.length > 0 && !xlGridfieldNames[i].startsWith('axp_recid')) {
                    _fldArrayList.push(xlGridfieldNames[i]);
                } else {
                    _thisEle = xlFinalimport.filter(item => item.n === xlGridfieldNames[i]);
                    if (_thisEle.length == 0 && !xlGridfieldNames[i].startsWith('axp_recid')) {
                        _fldArrayList.push(xlGridfieldNames[i]);
                    }
                }
            }
            var fldName = xlGridfieldNames[i] + rowno + "F" + xlDcNo;
            var fldIndex = $j.inArray(xlGridfieldNames[i], parent.FNames);
            if (parent.FFieldHidden[fldIndex] == "True" || _fldArrayList.indexOf(xlGridfieldNames[i]) > -1) {
                parent.EvaluateAxFunction(xlGridfieldNames[i], fldName, rowno + "F" + xlDcNo, "ImportFromExcel");
                parent.EvaluateExpressions(fldName);
                let _thisFName = xlGridfieldNames[i];
                let _thisFInd = parent.GetFieldIndex(xlGridfieldNames[i]);
                if (typeof _thisFInd != "undefined" && _thisFInd != -1) {
                    let _thisFdepStr = parent.FldDependents[_thisFInd].toString();
                    if (_thisFdepStr != "")
                        _thisFdepStr = _thisFdepStr.split(",");
                    if (_thisFdepStr != undefined) {
                        for (var di = 0; di < _thisFdepStr.length; di++) {
                            var dField = _thisFdepStr[di].toString();
                            var depFirstChar = dField.substring(0, 1);
                            var depfName = dField.substring(1);
                            if (parent.FMoe[_thisFInd].toString().toLowerCase() == "accept" && depfName == _thisFName && depFirstChar == "e")
                                continue;
                            if (depFirstChar == 'e' && $j.inArray(depfName, arrImportFldNames) == -1) {
                                parent.EvaluateAxFunction(depfName, fldName, "", "ImportFromExcel");
                            }
                        }
                    }
                }
            } else {
                let _thisFName = xlGridfieldNames[i];
                let _thisFInd = parent.GetFieldIndex(xlGridfieldNames[i]);
                if (typeof _thisFInd != "undefined" && _thisFInd != -1) {
                    let _thisFdepStr = parent.FldDependents[_thisFInd].toString();
                    if (_thisFdepStr != "")
                        _thisFdepStr = _thisFdepStr.split(",");
                    if (_thisFdepStr != undefined) {
                        for (var di = 0; di < _thisFdepStr.length; di++) {
                            var dField = _thisFdepStr[di].toString();
                            var depFirstChar = dField.substring(0, 1);
                            var depfName = dField.substring(1);
                            if (parent.FMoe[_thisFInd].toString().toLowerCase() == "accept" && depfName == _thisFName && depFirstChar == "e")
                                continue;
                            if (depFirstChar == 'e' && $j.inArray(depfName, arrImportFldNames) == -1) {
                                parent.EvaluateAxFunction(depfName, fldName, "", "ImportFromExcel");
                            }
                        }
                    }
                }
            }
        }
    }
    if (_AxpFormula) {
        rowno = "";
        let _fldIndex = parent.GetFieldIndex('axp_formula');
        let _fldName = parent.FNames[_fldIndex];
        for (var l = 1; l <= rowCount; l++) {
            if (l <= 9) {
                rowno = "00" + l;
            }
            else if (l > 9 && l <= 99) {
                rowno = "0" + l;
            }
            else {
                rowno = l;
            }
            let _fldObj = _fldName + rowno + "F" + xlDcNo;
            parent.ValidateFldValFormula(parent.$("#" + _fldObj), _fldIndex, _fldName);
        }
    }
}

function ExcelImpJsonToXML(jsonData) {
    let xml = '';
    const headers = jsonData[0].map(header => {
        const match = header.match(/\(([^)]+)\)(?!.*\()/);
        return match ? match[1] : header.replace(/\W/g, '_');
    });
    for (let i = 1; i < jsonData.length; i++) {
        xml += '<row>';
        const row = jsonData[i];
        for (let j = 0; j < headers.length; j++) {
            let fieldName = headers[j];
            let value = row[j];
            let selColData = parent.CheckSpecialCharsInStr(value);
            xml += `<${fieldName}>${selColData}</${fieldName}>`;
        }
        xml += '</row>';
    }
    return xml;
}

$(() => {
    $("#excelimport").click(function () {
        if (callParentNew("gridDummyRowVal").length > 0) {
            callParentNew("gridDummyRowVal").map(function (v) {
                if (v.split("~")[0] == xlDcNo)
                    IsgridummyRow =true
            });
        }
        if (xlRowCount > 0 && !(IsgridummyRow)) {
            var ConfirmDeleteCB = $.confirm({
                theme: 'modern',
                title: appGlobalVarsObject.lcm["155"],
                closeIcon: true,
                onContentReady: function () {
                    disableBackDrop('bind');
                },
                backgroundDismiss: 'false',
                escapeKey: 'buttonB',
                content: "Do You Want to Append?<br> Yes will Append, No will clear the existing data and import again!",
                buttons: {
                    buttonA: {
                        text: "Yes",
                        btnClass: 'btn btn-primary',
                        action: function () {
                            ImportGridData('t');
                            ConfirmDeleteCB.close();
                        }
                    },
                    buttonB: {
                        text: "No",
                        btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                        action: function () {
                            IsgridummyRow = false;
                            parent.$j("#chkallgridrow" + xlDcNo).prop("checked", "checked");
                            parent.clearDataGrid(xlDcNo, "excelimport");
                            xlRowCount = 0;
                            var myInterval = setInterval(() => {
                                if (callParentNew('isExcelImpDelWS') == 'false') {
                                    clearInterval(myInterval);
                                    ImportGridData('f');
                                }
                            }, 2000);
                            disableBackDrop('destroy');
                        }
                    }
                },
            });
        }
        else {
            if (IsgridummyRow) {

                try {
                    if (callParentNew("gridDummyRowVal").length > 0) {
                        callParentNew("gridDummyRowVal").map(function (v) {
                            if (v.split("~")[0] == xlDcNo)
                                callParentNew("gridDummyRowVal").splice($.inArray(v, callParentNew("gridDummyRowVal")), 1);
                        });
                    }
                }
                catch (ex) { }

                IsgridummyRow = false;
                parent.$j("#chkallgridrow" + xlDcNo).prop("checked", "checked");
                parent.clearDataGrid(xlDcNo, "excelimport");
                xlRowCount = 0;
                var myInterval = setInterval(() => {
                    if (callParentNew('isExcelImpDelWS') == 'false') {
                        clearInterval(myInterval);
                        parent.$j("#chkallgridrow" + xlDcNo).prop("checked", "");
                        ImportGridData('f');
                    }
                }, 2000);

            } else {
                ImportGridData('f');
            }
        }
    });
})


$j(document).ready(function () {
    $('#excelfile').bind({
        change: function (e){
            var filename = $("#excelfile").val();
            var fName = filename.toString().split("\\");
            filename = fName[fName.length - 1]
            $("#noFile").text(filename);
            var file = e.target.files[0];
            var files = e.target.files, f = files[0];
            var FR = new FileReader();
            FR.onload = function (e) {
                var data = new Uint8Array(e.target.result);
                var workbook = XLSX.read(data, { type: 'array',cellDates: true, dateNF: 'dd/mm/yyyy;@' });
                var firstSheet = workbook.Sheets[workbook.SheetNames[0]];//
                var result = XLSX.utils.sheet_to_json(firstSheet, { header: 1, raw: false });
                // var result = XLSX.utils.sheet_to_json(firstSheet, { header: 1, blankrows: false });
                if (result.length != 0)
                    result = result.filter(row => row.some(cell => cell !== "" && cell !== null));
                if (result.length == 0) {
                    showAlertDialog("error", eval(callParent('lcm[509]')));
                    return;
                }
                var dcNo = window.location.href.split('=')[1];
                var gridFieldNames = callParentNew('GetGridFields(' + dcNo + ')', 'function');
                var gridRowCount = callParentNew('GetDcRowCount(' + dcNo + ')', 'function');
                var gridFieldAlongwithCap = [];
                gridFieldNames.forEach(function (ele, ind) {
                    let _fieldIndex = parent.FNames.indexOf(ele);
                    let _fldCap = parent.FCaption[_fieldIndex];
                    gridFieldAlongwithCap.push(_fldCap + '(' + ele + ')');
                });
                var check = gridFieldNames.some(r => result[0].includes(r));
                var checkwithCap = gridFieldAlongwithCap.some(r => result[0].includes(r));
                if (!check && !checkwithCap) {
                    showAlertDialog("error", eval(callParent('lcm[510]')));
                    return;
                }
                else {
                    if (result[1].length == 0) {
                        showAlertDialog("error", eval(callParent('lcm[511]')));
                        return;
                    }
                    else {
                        xlresult = result;
                        xlGridfieldNames = gridFieldNames;
                        xlRowCount = gridRowCount;
                        xlDcNo = dcNo;
                    }
                }
            };
        
            FR.readAsArrayBuffer(file);
        }
    })
})
