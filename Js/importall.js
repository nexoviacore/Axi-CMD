var placement;
var ignoredColCount = 0;
var ignoredColumns = [];
var oldSelectedValues = 0;
var isFormDirty = false;
var leftSelectedClickedOnce = false;
var impTempObj = {};
var filenameuploaded = "";
var selectedValuesheets;
var arraysByDC;
var filenameXl;
var sheetHeaders = {};
var oldValue;
var fileUrl;
var impXlimportCol;

Dropzone.autoDiscover = false;

function validateDataSearchWiz() {
    var ddl = 0;
    var multiselect = 0;
    if ($('#ddlImpTbl').val() != 'NA') {
        ddl = 1;
        if ($('#mSelectRight option').val() != undefined && $('#mSelectRight option').val().length > 0) {
            var mandatoryField = false;
            $("#mSelectLeft option").each(function () {
                if ($(this).val().indexOf("*") >= 0) {
                    mandatoryField = true;
                    return;
                }
            });
            if (mandatoryField) {
                showAlertDialog("warning", eval(callParent('lcm[156]')));
                $('#mSelectLeft').focus();
            } else
                multiselect = 1
        } else {
            if ($("#dataupdatecheck").is(":checked")) {
                // Check the state of the checkbox
                showAlertDialog("warning", eval(callParent('lcm[531]')));
                $('#primaryfld').focus();
            }
            else {
                showAlertDialog("warning", eval(callParent('lcm[108]')));
                $('#mSelectLeft').focus();
            }
        }
    } else {
        showAlertDialog("warning", eval(callParent('lcm[106]')));
        $('#ddlImpTbl').data('selectpicker').$button.focus();
    }

    if (ddl == 1 && multiselect == 1) {
        var tempColNames = "", tempColValues = "";
        $("#mSelectRight option").each(function () {
            tempColNames += $(this).text() + ", ";
            tempColValues += $(this).val() + ", ";
        });
        tempColNames = tempColNames.substring(0, tempColNames.length - 2);
        tempColValues = tempColValues.substring(0, tempColValues.length - 2);

        selectedValues = $("#mSelectRight option").length;

        if (oldSelectedValues != 0 && selectedValues != oldSelectedValues) {
            $("#noFile").text(eval(callParent('lcm[66]')));
            $('#fileToUpload').val("").prop("title", eval(callParent('lcm[66]')));
            $("#IsFileUploaded").val("");
            $("#divProgress").hide();
            oldSelectedValues = 0;
        } else {
            $('#hdnSelectedColumnCount').val(selectedValues);
        }

        $('#hdnColNames').val(tempColNames);
        $('#hdnColValues').val(tempColValues);
        tempFileName = $('#ddlImpTbl :selected').text();
        hdnTemplateName = $("#hdnTemplateName").val();

        $("#excel1").click(function () {
            //$("#excel1").attr("href", "openfile.aspx?fpath=" + hdnTemplateName + ".xlsx&Imp=t")
            //$("#hdnTemplateName").val() = hdnTemplateName + ".xls";
            if (hdnTemplateName == "") {
                hdnTemplateName = $("#ddlImpTbl :selected").text();
            }
            $("#hdnTemplateName").val(hdnTemplateName + ".xlsx");
            $("#btnCreateTemplate").click();
        });

        $("#CSV1").click(function () {
            // $("#CSV1").attr("href", "openfile.aspx?fpath=" + hdnTemplateName + ".csv&Imp=t")
            if (hdnTemplateName == "") {
                hdnTemplateName = $("#ddlImpTbl :selected").text();
            }
            $("#hdnTemplateName").val(hdnTemplateName + ".csv");
            $("#btnCreateTemplate").click();
        });

        // $("#lnkExpTemp").attr("href", "openfile.aspx?fpath=" + hdnTemplateName + ".csv&Imp=t")
        // $("#lnkExpTemp").attr("href", "openfile.aspx?fpath=" + hdnTemplateName + ".xls&Imp=t")
        // $("#btnCreateTemplate").click();

        $("#ddlGroupBy").empty().append("<option value='NA'>-- Select --</option");
        colValues = $("#hdnGroupByColVal").val().split(', ');
        colNames = $("#hdnGroupByColName").val().split(', ');
        for (var i = 0; i < colNames.length; i++) {
            $("#ddlGroupBy").append("<option value='" + colValues[i] + "'>" + colNames[i] + "</option");
        }
        $("#ddlGroupBy").change(function () {
            $("#hdnGroupBy").val($("#ddlGroupBy").val());
        });
        return true;
    } else {
        return false;
    }
}

function FileuplaodValidation() {
    try {
        $.ajax({
            url: 'importnew.aspx/FileuplaodValidation',
            type: 'POST',
            cache: false,
            async: true,
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                var result = data.d;
                // let appSUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                // if (typeof localStorage["ExecutionFullLog-" + appSUrl] != "undefined")
                //    ExecutionLogText += localStorage["ExecutionFullLog-" + appSUrl];
                // $("#myDiv").html('');
                // $("#myDiv").html(ExecutionLogText.replace(/♦/g, '<br/>').replace(/\r\n/g, '<br/>'));
            },
            error: function (error) {
            }
        });
    }
    catch (exp) { }
}

// Function to generate the HTML content dynamically
function generateHTML(sheetNames, columns) {
    var container = document.getElementById('xlnum');
    impXlimportCol = columns;
    $('#gridImpData th select').each(function (index, selectElement) {
        // Set the default value for each select2 dropdown
        $(selectElement).val(columns[0][index]).trigger('change');
    });
    // Split the sheet names string into an array
    var sheets = sheetNames.split(',');

    var output = '';

    // Iterate through the sheet names and columns arrays
    sheets.forEach(function (sheet, index) {
        // For each sheet, create the string format sheetX.colY=columnName
        columns[index].forEach(function (col, colIndex) {
            output += sheet + '.col' + (colIndex + 1) + '=' + col;

            // Add a comma except for the last column in each sheet
            if (colIndex < columns[index].length - 1) {
                output += ', ';
            }
        });

        // After each sheet, add a newline (line break) if it's not the last sheet
        if (index < sheets.length - 1) {
            output += ' <br>';
        }
    });

    // Set the output to the div with id 'xlnum'
    container.innerHTML = output;
    $("#xlnum").addClass("form-control");
    if ($('#mulForm').is(':checked')) {
        // Code to execute when the checkbox is checked
        $("#xlnum").addClass("d-none");
    }

    //$("#btngridxl").removeClass("d-none");
}

// Call the function to generate the HTML content dynamically
//generateHTML(sheetNames, columns);

function addSheetRow1() {
    // Create a new row container
    var rowContainer = $("<div>").addClass("sheetRow");
    var section = $("<section>").addClass("form-group col-md-12 mb-4").text("xlfileafterupload");
    rowContainer.append(section);
    var formArr = $("#hdnformQueue").val().split(',');
    var label = $('<label>').attr('for', 'selectform').text('Map Form:').addClass('form-label col-form-label pb-1 fw-boldest mb-4');

    function createSelect(label, options, id) {
        var formArrVal = $("#hdnformQueueVal").val().split(',');
        var select = $("<select>").addClass("sheetselect form-select forValidation selectpicker mb-4").attr("id", id);;
        select.append($("<option>").text(label).attr("disabled", true).attr("selected", true));
        options.forEach(function (option, index) {
            select.append($("<option>").text(option).attr("value", formArrVal[index]));
        });
        //var dcTooltip ='<i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select an Existing ImportName from which the data needs to be imported." data-bs-placement="right">help_outline</i>';
        if (label === "Choose Group By") {
            select.prop("disabled", true);
        }
        if (label === "Choose DC") {
            //  select.append(dcTooltip);
        }

        return select;
    }
    function createSelectMap(label, options, id) {
        var formArrVal = $("#hdnformQueueVal").val().split(',');
        var select = $("<select>").addClass("multiselect form-select forValidation selectpicker mb-4").attr("id", id).attr("multiple", "multiple").attr("disabled", true);
        select.append($("<option>").text(label).attr("disabled", true).attr("selected", true));
        options.forEach(function (option, index) {
            select.append($("<option>").text(option).attr("value", formArrVal[index]));
        });
        return select;
    }
    // Create and append dropdowns for choose form, choose dc, choose primary key, and choose group by
    var selectForm = createSelect("Choose Form", formArr, "selectform");
    // var selectDC = createSelect("Choose DC", ["DC 1", "DC 2", "DC 3"], "selectDC");
    // var dcTooltip ='<i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select an Existing ImportName from which the data needs to be imported." data-bs-placement="right">help_outline</i>';
    var selectPrimaryKey = createSelect("Choose Primary Key", [], "selectPrimaryKey");
    //var selectGroupBy = createSelect("Choose Group By", ["Group 1", "Group 2", "Group 3"], "selectGroupBy" + i);
    $('#btnSaveTemplateXl').after(selectPrimaryKey).after(selectForm);//.after(label);
    var $container = $('<div>').addClass('d-flex align-items-center mb-3');

    // Move selectform and btnSaveTemplateXl into the container
    $('#selectform').appendTo($container);
    $('#btnSaveTemplateXl').appendTo($container);

    //$('#btnSaveTemplateXl').addClass('col-md-3 ms-3 mt-n4');

    // Insert the container into the controls div
    $container.prependTo('.controls');
    $(".controls").prepend($("#axstaysigninforms"));
    var newDiv = $('<div>').append(label); // Add a class for styling (optional)
    $('#myButton').next().before(newDiv);
    // $('#wizardBodyContent').prepend(selectForm,selectPrimaryKey).prepend(label);
    // $('#dropdownSheetContainer').append(label).append(selectForm, selectDC, selectPrimaryKey);
    //var selectElement = createSelectMap("Choose Map Fields", [], "selectMapFileds" + i);
    //rowContainer.append(selectElement);
    // Append the row container to the main container
    $("#dropdownsContainer").append(rowContainer);
    $('#selectform').on('select2:select', function (e) {
        var selectedValue = $(this).val();
        var selectedValueselFormid = $(this)[0].id;
        var lastCharac = selectedValueselFormid[selectedValueselFormid.length - 1];
        var sheetSelect = lastCharac - 1;
        var sheetnum = sheetSelect.toString();
        $(this).select2('close');
        // Example AJAX call
        //  debugger;
        $.ajax({
            url: 'ImportAll.aspx/GetFields',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                transid: selectedValue,
            }),
            dataType: 'json',
            contentType: "application/json",
            success: (data) => {
                var datas = JSON.parse(data.d);
                $('#hdnFname').val($('#select2-selectform-container').text() + "(" + $('#selectform').val() + ")");
                var visibleTstFlds = datas.VisibleTstFlds.m_StringValue;
                const items = datas.visibleTstFldsGridDc.m_StringValue.slice(0, -1).split(',');

                // Step 2: Create an object to group values by DC number
                const groupedDC = {};

                // Step 3: Iterate through the items and group them by their DC number
                items.forEach(item => {
                    const [field, dc] = item.split('♠'); // Split the field and the DC number
                    if (!groupedDC[dc]) {
                        groupedDC[dc] = []; // Initialize an array if the DC number is new
                    }
                    groupedDC[dc].push(field); // Add the field to the appropriate group
                });

                // Step 4: Convert the grouped object into an array of arrays dynamically
                const result = Object.entries(groupedDC).map(([dc, fields]) => ({
                    //dc: dc,
                    fields: fields
                }));

                const itemsnongrid = datas.visibleTstFldsNonGriddc.m_StringValue.slice(0, -1).split(',');

                // Step 2: Create an object to group values by DC number
                const groupedDCnongrid = {};

                // Step 3: Iterate through the items and group them by their DC number
                itemsnongrid.forEach(item => {
                    const [field, dc] = item.split('♠'); // Split the field and the DC number
                    if (!groupedDCnongrid[dc]) {
                        groupedDCnongrid[dc] = []; // Initialize an array if the DC number is new
                    }
                    groupedDCnongrid[dc].push(field); // Add the field to the appropriate group
                });

                // Step 4: Convert the grouped object into an array of arrays dynamically
                const resultnongrid = Object.entries(groupedDCnongrid).map(([dc, fields]) => ({
                    //dc: dc,
                    fields: fields
                }));

                const combined = [...resultnongrid, ...result];

                // Sort combined data by DC number (numerically)
                combined.sort((a, b) => parseInt(a.dc) - parseInt(b.dc));

                // Create arrays for each DC number dynamically
                arraysByDC = combined.map(entry => entry.fields);

                // Example to access arrays by DC numbering
                arraysByDC.forEach((fields, index) => {
                    console.log(`Array for DC ${combined[index].dc}:`, fields);
                });
                var visibleTstFlds = datas.VisibleTstFlds.m_StringValue;
                var separator = ',♠';
                var separatorIndex = visibleTstFlds.indexOf(separator); // Find the index of the separator
                var newStr = "";
                var trueValue = "";
                if (separatorIndex !== -1) {
                    newStr = visibleTstFlds.substring(0, separatorIndex); // Extract substring before the separator
                    trueValue = visibleTstFlds.substring(separatorIndex + separator.length); // Extract "true" after the separator
                }
                var PkeyArray = newStr.split(",");
                $("#hdnFieldnamesExcelnew").val(newStr);
                $(this).select2('close');
                var selectedValueseMapid = "#selectPrimaryKey";
                $(selectedValueseMapid).empty();
                // Append options to the select element from the parray
                PkeyArray.forEach(function (option) {
                    $(selectedValueseMapid).append($('<option></option>').attr('value', option).text(option));
                });
                $(selectedValueseMapid).select2();
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error("AJAX error:", error);
                $(this).select2('close');
            }
        });
    });

}

function getHeadersForSheet() {
    // Fixed table ID
    var table = document.getElementById("gridImpData");

    // Initialize an empty array to store headers
    var headers = [];

    if (table) {
        var headerRow = table.querySelector("tr"); // Get the header row

        if (headerRow) {
            var thElements = headerRow.querySelectorAll("th");

            thElements.forEach(function (th) {
                // Find the selected option value inside each <select> element
                var selectElement = th.querySelector("select");
                if (selectElement && selectElement.options[selectElement.selectedIndex]) {
                    // var selectedOption = selectElement.options[selectElement.selectedIndex];
                    headers.push(selectElement.options[selectElement.selectedIndex].text); // Store the selected value
                }
            });
        }
    }
    sheetHeaders[parseInt($('#selectsheet').val()) + 1] = headers;
    var sheetHeadersJSON = JSON.stringify(sheetHeaders);

    // Set the value of the hidden input field to the JSON string
    document.getElementById('hdndheetheaders').value = sheetHeadersJSON;
    return headers; // Return the collected headers
}

function countSheets(numberSheets, fileName) {
    if ($('#mulForm').is(':checked')) {
        // Code to execute when the checkbox is checked
        return;
    }
    $(".sheetselect").select2();
    $(".importrecord").removeClass("d-none");
    //$("#txtgrey").removeClass("d-none");
    filenameuploaded = fileName + "-" + filename;
    // Variable to hold the number of sheets
    var numberOfSheets = parseInt(numberSheets); // Change this to the actual number of sheets
    var label = $('<label>').attr('for', 'selectsheet').text('Select Sheet:').addClass('form-label col-form-label pb-1 fw-boldest mb-4');
    var selectSheet = $('<select>').attr('id', 'selectsheet').addClass('sheetname form-select forValidation selectpicker mb-4');
    var outlineHelp = '<span tabindex="0" class="material-icons material-icons-style material-icons-3 icon-arrows-question col-info" data-bs-toggle="tooltip" id="headertip" data-bs-original-title="Please Map all the coloumns of every sheet if you are uploading file without headers" data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span>';
    //var label = $('<label>').attr('for', 'selectform').text('Select Ttsruct:').addClass('form-label col-form-label pb-1 fw-boldest mb-4');
    //var selectForm = $('<select>').attr('id', 'selectform').addClass('selectform form-select forValidation selectpicker mb-4');
    //var formArr = $("#hdnformQueue").val().split(',');

    // var formArr = $("#hdnformQueue").val().split(',');

    // Create and append dropdowns for choose form, choose dc, choose primary key, and choose group by
    //var selectForm = createSelect("Choose Form", formArr,"selectform" + i);
    //var selectDC = createSelect("Choose DC", ["DC 1", "DC 2", "DC 3"], "selectDC" + i);
    //var selectPrimaryKey = createSelect("Choose Primary Key", ["Key 1", "Key 2", "Key 3"], "selectPrimaryKey" + i);
    //var selectGroupBy = createSelect("Choose Group By", ["Group 1", "Group 2", "Group 3"], "selectGroupBy" + i);

    // rowContainer.append(selectForm, selectDC, selectPrimaryKey, selectGroupBy);
    // Append options to the select element
    for (var i = 1; i <= numberOfSheets; i++) {
        var option = $('<option>').val(i - 1).text('Sheet ' + i);
        selectSheet.append(option);
    }

    // Append the select element to the container


    // Initialize Select2
    $('#selectsheet').select2();
    //var numberOfSheets = 2;
    // Loop to add rows for each sheet
    //for (var i = 1; i <= numberOfSheets; i++) {
    //    addSheetRow("Sheet " + i);
    //}
    //addSheetRow();
    //$('#checksheets').append(label).append(selectSheet);
    if ($('#checksheets #selectsheet').length > 0) {
        $('#checksheets #selectsheet').replaceWith(selectSheet);
        $('#checksheets label[for="selectsheet"]').replaceWith(label);
    } else {
        $('#checksheets').append(label).append(selectSheet);
    }

    $('#selectsheet').select2();
    var btngridxl = $('#btngridxl');
    $('#btngridxl').removeClass("d-none");
    $('#checksheets').append(btngridxl);//.append(outlineHelp);
    if ($('#checksheets #headertip').length > 0) {
        $('#checksheets #headertip').remove();
    }
    $('#checksheets').append(outlineHelp);
    $('#selectsheet').on('select2:opening', function () {
        oldValue = $(this).val(); // Store the current value as old value
        //console.log("Old Value captured:", oldValue);
    });
    //$('#selectsheet').on('focus', function () {
    //    oldValue = $(this).val();
    //});
    $.ajax({
        url: 'ImportAll.aspx/GetSheets',
        type: 'POST',
        cache: false,
        async: false,
        data: JSON.stringify({
            sheetname: "0",
            filename: filenameuploaded,
            xlhasheader: $('#hdnxlHasHeader').val(),
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (response) {
            if (response.d) {
                var responseObject = JSON.parse(response.d);
                var headersString = responseObject.Headers;
                var jsonData = JSON.parse(responseObject.Data);

                var primKey = $('#selectPrimaryKey').val();
                if (primKey != "" && primKey.includes("("))
                    primKey = primKey;
                else
                    primKey = $('#hdnFnameKey').val();

                arraysByDC.forEach(arr => {
                    // Check if the first element is not 'x', then add it
                    if (arr[0] !== primKey) {
                        arr.unshift(primKey);  // Add 'x' to the beginning of the array
                    }
                });
                getHeadersForSheet();
                var chosenSheetKey = parseInt(oldValue) + 1;
                sheetHeaders[chosenSheetKey] = getHeadersForSheet();
                var sheetHeadersJSON = JSON.stringify(sheetHeaders);

                // Set the value of the hidden input field to the JSON string
                document.getElementById('hdndheetheaders').value = sheetHeadersJSON;
                // Use headersString as needed
                let parts = headersString.split(/[,()]/);

                // Step 2: Process the parts to get the desired strings
                let headersStrings = parts.filter((part, index) => index % 2 === 0).join(',');
                var headersStringsarr = headersString.split(",");
                let valuesStrings = parts.filter((part, index) => index % 2 !== 0).join(',');
                var selectedMapfieldId = "#mapfile" + (parseInt(selectedValuesheets) + 1).toString();
                var selectedMapfldwthHeaders = "#selectMapFileds" + (parseInt(selectedValuesheets) + 1).toString();
                var gridData = "<table class='table w-100' id='gridImpData'><thead><tr>";

                if ($('#hdnxlHasHeader').val() != "" && $('#hdnxlHasHeader').val() == "false") {
                    for (let i = 0; i < Object.keys(jsonData[0]).length; i++) {
                        gridData += "<th>" + arraysByDC[0][i] + "</th>";//($('#hdnFieldnamesExcelnew').val().split(",")[i]);
                    }
                }
                else {
                    // Generate header row with dropdowns
                    for (var key in jsonData[0]) {
                        gridData += "<th>" + key + "</th>";
                    }
                }
                gridData += "</tr></thead><tbody>";

                // Generate data rows
                jsonData.forEach(function (row) {
                    gridData += "<tr>";
                    for (var key in row) {
                        gridData += "<td>" + row[key] + "</td>";
                    }
                    gridData += "</tr>";
                });
                gridData += "</tbody></table>";
                $('#UpdateGrid').html(gridData);
                if ($('#hdnxlHasHeader').val() != "" && $('#hdnxlHasHeader').val() == "false") {
                    $('#gridImpData thead th').each(function (index) {
                        // var colName = headersStrings.split(',').map(x => x.trim()).filter(x => x);
                        let valuesStringshdr = arraysByDC[0].toString().split(/[,()]/).filter((part, index) => index % 2 !== 0).join(',');
                        var colValue = valuesStringshdr.split(',').map(x => x.trim()).filter(x => x);

                        var ddlColumnName = $('<select>').attr('id', 'ddlColumnName' + index).addClass('form-control');
                        for (var j = 0; j < colValue.length; j++) {
                            //ddlColumnName.append($('<option>').val(colValue[j]).text(colName[j]));
                            ddlColumnName.append($('<option>').val(colValue[j]).text(arraysByDC[0][j]))
                        }
                        var headerText = $(this).text();
                        var headerList = colValue;
                        if (ChkColNameInfile.checked == false) { // Assuming you have a way to set this
                            if (headerList[index]) {
                                ddlColumnName.val(headerList[index]);
                            }
                        } else {
                            ddlColumnName.val(headerText);
                        }

                        $(this).html(ddlColumnName); // Replace the header text with the dropdown

                    });
                }
                else {
                    // Add dropdowns to the header cells
                    $('#gridImpData thead th').each(function (index) {
                        var colName = headersStrings.split(',').map(x => x.trim()).filter(x => x);
                        var colValue = valuesStrings.split(',').map(x => x.trim()).filter(x => x);

                        var ddlColumnName = $('<select>').attr('id', 'ddlColumnName' + index).addClass('form-control');
                        for (var j = 0; j < colName.length; j++) {
                            //ddlColumnName.append($('<option>').val(colValue[j]).text(colName[j]));
                            ddlColumnName.append($('<option>').val(colValue[j]).text(headersStringsarr[j]))
                        }

                        // If needed, set the selected value based on some logic
                        var headerText = $(this).text();
                        var headerList = colValue;
                        if (ChkColNameInfile.checked == false) { // Assuming you have a way to set this
                            if (headerList[index]) {
                                ddlColumnName.val(headerList[index]);
                            }
                        } else {
                            ddlColumnName.val(headerText);
                        }

                        $(this).html(ddlColumnName); // Replace the header text with the dropdown

                    });
                }
                addChkbxsToGrdColumns();
            }

            // Handle success response
            //if (response.d) {
            //    // Bind the data to the GridView using partial postback
            //    var jsonData = JSON.parse(response.d);
            //    var gridData = "<table class='table w-100'><tr>";
            //    for (var key in jsonData[0]) {
            //        gridData += "<th>" + key + "</th>";
            //    }
            //    gridData += "</tr>";
            //    jsonData.forEach(function(row) {
            //        gridData += "<tr>";
            //        for (var key in row) {
            //            gridData += "<td>" + row[key] + "</td>";
            //        }
            //        gridData += "</tr>";
            //    });
            //    gridData += "</table>";
            //    $('#UpdateGrid').html(gridData);
            //}
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error("AJAX error:", error);
        }
    });
    $('#selectsheet').on('change', function () {
        selectedValuesheets = $(this).val();

        // Call the AJAX function
        $.ajax({
            url: 'ImportAll.aspx/GetSheets',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                sheetname: selectedValuesheets,
                filename: filenameuploaded,
                xlhasheader: $('#hdnxlHasHeader').val(),
            }),
            dataType: 'json',
            contentType: "application/json",
            success: function (response) {
                if (response.d) {
                    var responseObject = JSON.parse(response.d);
                    var headersString = responseObject.Headers;
                    var jsonData = JSON.parse(responseObject.Data);

                    var primKey = $('#selectPrimaryKey').val();
                    if (primKey != "" && primKey.includes("("))
                        primKey = primKey;
                    else
                        primKey = $('#hdnFnameKey').val();

                    arraysByDC.forEach(arr => {
                        // Check if the first element is not 'x', then add it
                        if (arr[0] !== primKey) {
                            arr.unshift(primKey);  // Add 'x' to the beginning of the array
                        }
                    });
                    function getHeadersForSheet() {
                        // Fixed table ID
                        var table = document.getElementById("gridImpData");

                        // Initialize an empty array to store headers
                        var headers = [];

                        if (table) {
                            var headerRow = table.querySelector("tr"); // Get the header row

                            if (headerRow) {
                                var thElements = headerRow.querySelectorAll("th");

                                thElements.forEach(function (th) {
                                    // Find the selected option value inside each <select> element
                                    var selectElement = th.querySelector("select");
                                    if (selectElement) {
                                        var selectedOption = selectElement.options[selectElement.selectedIndex];
                                        headers.push(selectedOption.text); // Store the selected value
                                    }
                                });
                            }
                        }

                        return headers; // Return the collected headers
                    }


                    //function getHeadersForSheet() {
                    //    var table = document.getElementById("gridImpData");
                    //    var headers = [];

                    //    // Check if the table exists
                    //    if (table) {
                    //        var headerRow = table.querySelector("tr"); // Get the first row

                    //        if (headerRow) {
                    //            // Loop through all the <th> elements in the header row
                    //            var thElements = headerRow.querySelectorAll("th");

                    //            thElements.forEach(function (th) {
                    //                // Find the selected option value inside each <select> element
                    //                var selectElement = th.querySelector("select");
                    //                if (selectElement) {
                    //                    var selectedOption = selectElement.options[selectElement.selectedIndex];
                    //                    headers.push(selectedOption.value); // Push the value of the selected option
                    //                }
                    //            });
                    //        }
                    //    }
                    //    return headers;
                    //}
                    //var chosenSheetKey = parseInt(selectedValuesheets) - 1; // Example: sheet2
                    var chosenSheetKey = parseInt(oldValue) + 1;
                    sheetHeaders[chosenSheetKey] = getHeadersForSheet();
                    var sheetHeadersJSON = JSON.stringify(sheetHeaders);

                    // Set the value of the hidden input field to the JSON string
                    document.getElementById('hdndheetheaders').value = sheetHeadersJSON;
                    //var labelHtml = '<div class="importrecord">' +
                    //'<asp:Label ID="lblrecords" CssClass="form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" runat="server" meta:resourcekey="lblrecords">' +
                    //'Top 5 records' +
                    //'</asp:Label>' +
                    //'</div>';
                    //$('#UpdateGrid').before(labelHtml);
                    // Use headersString as needed
                    let parts = headersString.split(/[,()]/);

                    // Step 2: Process the parts to get the desired strings
                    let headersStrings = parts.filter((part, index) => index % 2 === 0).join(',');
                    var headersStringsarr = headersString.split(",");
                    let valuesStrings = parts.filter((part, index) => index % 2 !== 0).join(',');
                    var selectedMapfieldId = "#mapfile" + (parseInt(selectedValuesheets) + 1).toString();
                    var selectedMapfldwthHeaders = "#selectMapFileds" + (parseInt(selectedValuesheets) + 1).toString();
                    //if(! $(selectedMapfieldId).is(":checked")){
                    //    headersStrings =$(selectedMapfldwthHeaders).val().toString();
                    //    valuesStrings =$(selectedMapfldwthHeaders).val().toString();
                    //}
                    // var headerstring=$(selectedMapfieldId).val().toString();
                    //let parts = headersString.split(/,|[(](.*?)[)]/);

                    // Remove empty strings and trim spaces
                    //parts = parts.filter(part => part.trim() !== "");

                    //// Extract headers and values separately
                    //let headers = [];
                    //let values = [];
                    //for (let i = 0; i < parts.length; i += 2) {
                    //    headers.push(parts[i].trim());
                    //    values.push(parts[i + 1].trim());
                    //}

                    //// Join the headers and values arrays into strings
                    //let headersStrings = headers.join(",");
                    //let headersStrings = values.join(",");

                    // Bind the data to the GridView using partial postback
                    var gridData = "<table class='table w-100' id='gridImpData'><thead><tr>";

                    if ($('#hdnxlHasHeader').val() != "" && $('#hdnxlHasHeader').val() == "false") {
                        for (let i = 0; i < arraysByDC[selectedValuesheets].length; i++) {
                            gridData += "<th>" + arraysByDC[selectedValuesheets][i] + "</th>";
                        }
                    } else {
                        // Fallback when jsonData is empty
                        if (jsonData.length === 0) {
                            let fallbackHeaders = headersString.split(',').map(x => x.trim()).filter(x => x);
                            for (let i = 0; i < fallbackHeaders.length; i++) {
                                gridData += "<th>" + fallbackHeaders[i] + "</th>";
                            }
                        } else {
                            for (var key in jsonData[0]) {
                                gridData += "<th>" + key + "</th>";
                            }
                        }
                    }
                    gridData += "</tr></thead><tbody>";

                    // Generate data rows
                    jsonData.forEach(function (row) {
                        gridData += "<tr>";
                        for (var key in row) {
                            gridData += "<td>" + row[key] + "</td>";
                        }
                        gridData += "</tr>";
                    });
                    gridData += "</tbody></table>";
                    $('#UpdateGrid').html(gridData);
                    if ($('#hdnxlHasHeader').val() != "" && $('#hdnxlHasHeader').val() == "false") {
                        $('#gridImpData thead th').each(function (index) {
                            // var colName = headersStrings.split(',').map(x => x.trim()).filter(x => x);
                            let valuesStringshdr = arraysByDC[selectedValuesheets].toString().split(/[,()]/).filter((part, index) => index % 2 !== 0).join(',');
                            var colValue = valuesStringshdr.split(',').map(x => x.trim()).filter(x => x);

                            var ddlColumnName = $('<select>').attr('id', 'ddlColumnName' + index).addClass('form-control');
                            for (var j = 0; j < colValue.length; j++) {
                                //ddlColumnName.append($('<option>').val(colValue[j]).text(colName[j]));
                                ddlColumnName.append($('<option>').val(colValue[j]).text(arraysByDC[selectedValuesheets][j]))
                            }
                            var headerText = $(this).text();
                            var headerList = colValue;
                            if (ChkColNameInfile.checked == false) { // Assuming you have a way to set this
                                if (headerList[index]) {
                                    ddlColumnName.val(headerList[index]);
                                }
                            } else {
                                ddlColumnName.val(headerText);
                            }

                            $(this).html(ddlColumnName); // Replace the header text with the dropdown

                        });
                    }
                    else {
                        // Add dropdowns to the header cells
                        $('#gridImpData thead th').each(function (index) {
                            var colName = headersStrings.split(',').map(x => x.trim()).filter(x => x);
                            var colValue = valuesStrings.split(',').map(x => x.trim()).filter(x => x);

                            var ddlColumnName = $('<select>').attr('id', 'ddlColumnName' + index).addClass('form-control');
                            for (var j = 0; j < colName.length; j++) {
                                //ddlColumnName.append($('<option>').val(colValue[j]).text(colName[j]));
                                ddlColumnName.append($('<option>').val(colValue[j]).text(headersStringsarr[j]))
                            }

                            // If needed, set the selected value based on some logic
                            var headerText = $(this).text();
                            var headerList = colValue;
                            if (ChkColNameInfile.checked == false) { // Assuming you have a way to set this
                                if (headerList[index]) {
                                    ddlColumnName.val(headerList[index]);
                                }
                            } else {
                                ddlColumnName.val(headerText);
                            }

                            $(this).html(ddlColumnName); // Replace the header text with the dropdown

                        });
                    }
                    addChkbxsToGrdColumns();
                }

                // Handle success response
                //if (response.d) {
                //    // Bind the data to the GridView using partial postback
                //    var jsonData = JSON.parse(response.d);
                //    var gridData = "<table class='table w-100'><tr>";
                //    for (var key in jsonData[0]) {
                //        gridData += "<th>" + key + "</th>";
                //    }
                //    gridData += "</tr>";
                //    jsonData.forEach(function(row) {
                //        gridData += "<tr>";
                //        for (var key in row) {
                //            gridData += "<td>" + row[key] + "</td>";
                //        }
                //        gridData += "</tr>";
                //    });
                //    gridData += "</table>";
                //    $('#UpdateGrid').html(gridData);
                //}
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error("AJAX error:", error);
            }
        });
    });

    function addSheetRow() {
        // Create a new row container
        var rowContainer = $("<div>").addClass("sheetRow");
        var section = $("<section>").addClass("form-group col-md-12 mb-4").text("xlfileafterupload");
        rowContainer.append(section);
        var formArr = $("#hdnformQueue").val().split(',');
        var label = $('<label>').attr('for', 'selectform').text('Map Form:').addClass('form-label col-form-label pb-1 fw-boldest mb-4');
        // Create and append dropdowns for choose form, choose dc, choose primary key, and choose group by
        var selectForm = createSelect("Choose Form", formArr, "selectform");
        var selectDC = createSelect("Choose DC", ["DC 1", "DC 2", "DC 3"], "selectDC");
        // var dcTooltip ='<i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select an Existing ImportName from which the data needs to be imported." data-bs-placement="right">help_outline</i>';
        var selectPrimaryKey = createSelect("Choose Primary Key", ["Key 1", "Key 2", "Key 3"], "selectPrimaryKey");
        //var selectGroupBy = createSelect("Choose Group By", ["Group 1", "Group 2", "Group 3"], "selectGroupBy" + i);
        $('#wizardBodyContent').append(label).append(selectForm, selectPrimaryKey);
        // $('#dropdownSheetContainer').append(label).append(selectForm, selectDC, selectPrimaryKey);
        // Create and append normal button
        //var normalButton = $("<button>").addClass("btn btn-primary").text("Map");
        //rowContainer.append(normalButton);
        var selectElement = createSelectMap("Choose Map Fields", [], "selectMapFileds" + i);
        rowContainer.append(selectElement);
        // Create and append toggle button attr("type", "checkbox").
        //var toggleCheckboxDiv = $("<div>").addClass("form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4");
        //var toggleButton = $("<input>").attr({"type": "checkbox","checked": "checked"}).addClass("m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2").attr("id", "mapfile" + i).text("Map in File");
        //var togglespan = $("<span>").addClass("form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0").text("Map in File");
        //toggleButton.change(function() {
        //    if (!$(this).is(":checked")) {
        //       // $(this).removeClass("d-none");
        //        var selectedMapfieldId = "#selectMapFileds" +$(this)[0].id[$(this)[0].id.length - 1];
        //        $(selectedMapfieldId).attr("disabled",false);
        //        // Code to execute when checkbox is unchecked
        //        //console.log("Checkbox is unchecked!");
        //        // Add your code here
        //    }
        //});
        //toggleCheckboxDiv.append(toggleButton);
        //toggleCheckboxDiv.append(togglespan);
        //rowContainer.append(toggleCheckboxDiv);
        //if (i !== 1) {
        //    rowContainer = $("<div>").addClass("collapse sheetCollapse").attr("id", "sheetCollapse" + i).append(rowContainer);
        //}

        // Append the row container to the main container
        $("#dropdownsContainer").append(rowContainer);


        // Add collapse button
        //var collapseButton = $("<button>")
        //  .addClass("btn btn-primary mb-4")
        //  .attr("type", "button")
        //  .attr("data-toggle", "collapse")
        //  .attr("data-target", ".collapse")
        //  .text("Show/Hide Additional Sheets");

        //$("#dropdownsContainer").append(collapseButton);
        // Append the row container to the main container
        // $("#dropdownsContainer").append(rowContainer);
        // selectForm.on("change", function() {
        selectForm.on('select2:select', function (e) {
            setTimeout(function () {
                // Close the "Choose Group By" Select2 dropdown after selecting an option
                $(this).select2('close');
                ShowDimmer(true);
            }, 0);

            var selectedValue = $(this).val();
            var selectedValueselFormid = $(this)[0].id;
            var lastCharac = selectedValueselFormid[selectedValueselFormid.length - 1];
            var sheetSelect = lastCharac - 1;
            var sheetnum = sheetSelect.toString();
            $(this).select2('close');
            // Example AJAX call
            $.ajax({
                url: 'ImportAll.aspx/GetDcs',
                type: 'POST',
                cache: false,
                async: false,
                data: JSON.stringify({
                    transid: selectedValue,
                }),
                dataType: 'json',
                contentType: "application/json",
                success: (data) => {
                    var dccount = parseInt(data.d).toString();
                    // ShowDimmer(false);
                    var dcArray = [];
                    for (var i = 1; i <= data.d; i++) {
                        dcArray.push("DC" + i);
                    }
                    selectedValueselFormid = "#selectDC";
                    $(selectedValueselFormid).empty();
                    //  $(selectedValueselFormid).

                    // Add the new options to the Select2 dropdown
                    dcArray.forEach(function (option) {
                        $(selectedValueselFormid).append($("<option>").text(option));
                    });
                    $(selectedValueselFormid).attr("multiple", "multiple");
                    $(selectedValueselFormid).select2({
                        //placeholder: "Select options", // Placeholder text
                        allowClear: true,              // Clear button for selected options
                        tags: true,                    // Enable tagging mode (pills for selections)
                        //tokenSeparators: [',', ' ']    // Allow selection by typing values separated by commas or spaces
                    });
                    // if (data.d == "done") {
                    // success: function(response) {
                    // Handle success response
                    //}
                    const selectAllOption = new Option("Select All", "selectAll", false, false);
                    $(selectedValueselFormid).prepend(selectAllOption); // Add at the beginning
                    $(selectedValueselFormid).trigger("change"); // Update Select2 UI

                    // Handle "Select All" functionality
                    $(selectedValueselFormid).on("select2:select", function (e) {
                        const selectedValue = e.params.data.id;

                        if (selectedValue === "selectAll") {
                            // Select all other options
                            $("#selectDC > option").each(function () {
                                if ($(this).val() !== "selectAll") {
                                    $(this).prop("selected", true);
                                }
                            });
                            $(selectedValueselFormid).trigger("change"); // Update Select2 UI
                        }
                    });

                    // Handle deselecting "Select All"
                    $("#selectDC").on("select2:unselect", function (e) {
                        const unselectedValue = e.params.data.id;

                        if (unselectedValue === "selectAll") {
                            // Deselect all options
                            $("#selectDC > option").prop("selected", false);
                            $("#selectDC").trigger("change"); // Update Select2 UI
                        }
                    });
                    $(this).select2('close');
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.error("AJAX error:", error);
                    $(this).select2('close');
                }
            });
            //$.ajax({
            //    url: 'ImportAll.aspx/GetSheets',
            //    type: 'POST',
            //    cache: false,
            //    async: false,
            //    data: JSON.stringify({
            //        sheetname: sheetnum,
            //        filename: filenameuploaded,
            //    }),
            //    dataType: 'json',
            //    contentType: "application/json",
            //    success: function(response) {
            //        if (response.d) {
            //            var responseObject = JSON.parse(response.d);
            //            var headersString = responseObject.Headers;
            //            //var jsonData = JSON.parse(responseObject.Data);
            //            //var labelHtml = '<div class="importrecord">' +
            //            //'<asp:Label ID="lblrecords" CssClass="form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0" runat="server" meta:resourcekey="lblrecords">' +
            //            //'Top 5 records' +
            //            //'</asp:Label>' +
            //            //'</div>';
            //            //$('#UpdateGrid').before(labelHtml);
            //            // Use headersString as needed
            //            //let parts = headersString.split(/[,()]/);

            //            //// Step 2: Process the parts to get the desired strings
            //            //let headersStrings = parts.filter((part, index) => index % 2 === 0).join(',');
            //            //let valuesStrings = parts.filter((part, index) => index % 2 !== 0).join(',');
            //            var PkeyArray = headersString.split(",");
            //            selectedValueselFormid ="#selectPrimaryKey" +lastCharac.toString();
            //    $(selectedValueselFormid).empty();
            //$(selectedValueselFormid).append($("<option>").text("Choose primary key").attr("disabled", true).attr("selected", true).attr("hidden", true));
            //    PkeyArray.forEach(function(option) {
            //        $(selectedValueselFormid).append($("<option>").text(option));
            //    });
            //}
            //else{
            //    //selectedValueselFormid ="#selectGroupBy" +$(this)[0].id[$(this)[0].id.length - 1];
            //   // $(selectedValueselFormid).attr("disabled",true);
            //}
            //$(this).select2('close');
            //    ////////selectedValueselFormid ="#selectPrimaryKey" +$(this)[0].id[$(this)[0].id.length - 1];
            //    ////////$(selectedValueselFormid).empty();

            //    ////////// Add the new options to the Select2 dropdown
            //    ////////PkeyArray.forEach(function(option) {
            //    ////////    $(selectedValueselFormid).append($("<option>").text(option));
            //    ////////});

            //    // if (data.d == "done") {
            //    // success: function(response) {
            //    // Handle success response
            //    //}
            //},
            //error: function(xhr, status, error) {
            //    // Handle error
            //    console.error("AJAX error:", error);
            //    $(this).select2('close');
            //}
            //});

            $.ajax({
                url: 'ImportAll.aspx/GetFields',
                type: 'POST',
                cache: false,
                async: false,
                data: JSON.stringify({
                    transid: selectedValue,
                }),
                dataType: 'json',
                contentType: "application/json",
                success: (data) => {
                    var datas = JSON.parse(data.d);
                    var visibleTstFlds = datas.VisibleTstFlds.m_StringValue;
                    // var dccount = parseInt(data.d).toString()
                    //ShowDimmer(false);
                    var separator = ',♠';
                    var separatorIndex = visibleTstFlds.indexOf(separator); // Find the index of the separator
                    var newStr = "";
                    var trueValue = "";
                    if (separatorIndex !== -1) {
                        newStr = visibleTstFlds.substring(0, separatorIndex); // Extract substring before the separator
                        trueValue = visibleTstFlds.substring(separatorIndex + separator.length); // Extract "true" after the separator
                    }
                    var PkeyArray = newStr.split(",");
                    //for (var i = 1; i <= data.d; i++) {
                    //    dcArray.push("DC" + i);
                    //}
                    //if(trueValue =="true"){

                    //    selectedValueselFormid ="#selectGroupBy" +$(this)[0].id[$(this)[0].id.length - 1];
                    //    $(selectedValueselFormid).attr("disabled",false);
                    //    $(selectedValueselFormid).empty();
                    //    $(selectedValueselFormid).append($("<option>").text("Choose GroupBy").attr("disabled", true).attr("selected", true).attr("hidden", true));
                    //    PkeyArray.forEach(function(option) {
                    //        $(selectedValueselFormid).append($("<option>").text(option));
                    //    });
                    //}
                    //else{
                    //    selectedValueselFormid ="#selectGroupBy" +$(this)[0].id[$(this)[0].id.length - 1];
                    //    $(selectedValueselFormid).empty();
                    //    $(selectedValueselFormid).append($("<option>").text("Choose GroupBy").attr("disabled", true).attr("selected", true).attr("hidden", true));
                    //    $(selectedValueselFormid).attr("disabled",true);
                    //}
                    $(this).select2('close');
                    //function createMultiselect() {
                    // Create a select element
                    //var selectElement = $('<select multiple="multiple" id="multiselect"></select>');
                    var selectedValueseMapid = "#selectPrimaryKey";
                    $(selectedValueseMapid).empty();
                    // Append options to the select element from the parray
                    PkeyArray.forEach(function (option) {
                        $(selectedValueseMapid).append($('<option></option>').attr('value', option).text(option));
                    });

                    // Append the select element to the container div
                    // $('#multiselectContainer').append(selectElement);

                    // Initialize Select2 on the select element
                    //var selectedValueseMapid ="#selectMapFileds" +$(this)[0].id[$(this)[0].id.length - 1];
                    //$(selectedValueseMapid).empty();
                    $(selectedValueseMapid).select2();
                    //$(selectedValueseMapid).attr("disabled",false);
                    //}
                    ////////selectedValueselFormid ="#selectPrimaryKey" +$(this)[0].id[$(this)[0].id.length - 1];
                    ////////$(selectedValueselFormid).empty();

                    ////////// Add the new options to the Select2 dropdown
                    ////////PkeyArray.forEach(function(option) {
                    ////////    $(selectedValueselFormid).append($("<option>").text(option));
                    ////////});

                    // if (data.d == "done") {
                    // success: function(response) {
                    // Handle success response
                    //}

                    //var multiselect = datas.Multiselect;
                    //var selectElement = $('#mSelectLeft');
                    //var repeaterContainer = $('#<%= rptSelectFields.ClientID %>');

                    //selectElement.empty(); // Clear existing options
                    //repeaterContainer.empty(); // Clear existing repeater items

                    //$.each(multiselect, function(key, value) {
                    //    // Add options to the select element
                    //    var parts = key.split('&&');
                    //    var option = $('<option>').val(parts[0]).text(parts[1]);
                    //    selectElement.append(option);

                    //    // Add items to the repeater
                    //    //var repeaterItem = `<option value="${parts[0]}">${parts[1]}</option>`;
                    //    var repeaterItem = '<option value="' + parts[0] + '">' + parts[1] + '</option>';
                    //    repeaterContainer.append(repeaterItem);
                    //});

                    //    // Initialize Select2
                    //selectElement.select2();

                    // Use the visibleTstFlds string as needed
                    //console.log('Visible Test Fields:', visibleTstFlds);
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.error("AJAX error:", error);
                    $(this).select2('close');
                }
            });
            //setTimeout(function() {
            //    // Close the "Choose Group By" Select2 dropdown after selecting an option
            //    $(this).select2('close');
            //    ShowDimmer(false);
            //}, 0);
            //selectForm.select2('close');
            //ShowDimmer(false);
        });

    }
    function createSelect(label, options, id) {
        var formArrVal = $("#hdnformQueueVal").val().split(',');
        var select = $("<select>").addClass("sheetselect form-select forValidation selectpicker mb-4").attr("id", id);;
        select.append($("<option>").text(label).attr("disabled", true).attr("selected", true));
        options.forEach(function (option, index) {
            select.append($("<option>").text(option).attr("value", formArrVal[index]));
        });
        //var dcTooltip ='<i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select an Existing ImportName from which the data needs to be imported." data-bs-placement="right">help_outline</i>';
        if (label === "Choose Group By") {
            select.prop("disabled", true);
        }
        if (label === "Choose DC") {
            //  select.append(dcTooltip);
        }

        return select;
    }
    function createSelectMap(label, options, id) {
        var formArrVal = $("#hdnformQueueVal").val().split(',');
        var select = $("<select>").addClass("multiselect form-select forValidation selectpicker mb-4").attr("id", id).attr("multiple", "multiple").attr("disabled", true);
        select.append($("<option>").text(label).attr("disabled", true).attr("selected", true));
        options.forEach(function (option, index) {
            select.append($("<option>").text(option).attr("value", formArrVal[index]));
        });
        return select;
    }
    //var selectElement = $('<select multiple="multiple" id="multiselect" class="select"></select>');
    $(".sheetselect").select2();
    $(".multiselect").select2();
}

function validateDataUploadWiz() {
    var uploadMsg = $("#hdnUploadFileWarnings").val();
    if ($("#IsFileUploaded").val() == "1") {
        ChkAllowUpdate();
        return true;
    } else {
        if (uploadMsg == "Empty") {
            showAlertDialog('warning', 4036, 'client');
            return false;
        } else if (uploadMsg == "NotEqualColumns") {
            showAlertDialog('warning', 4039, 'client');
            return false;
        } else if (uploadMsg == "DuplicateColumns") {
            showAlertDialog('warning', eval(callParent('lcm[307]')));
            return false;
        } else if (uploadMsg == "InvalidFileFormat") {
            showAlertDialog('warning', eval(callParent('lcm[310]')));
            return false;
        } else {
            fileupload = 1;
            var errorMesg = $(".fileUploadErrorMessage").text();
            if (errorMesg.indexOf('csv') < 0)
                showAlertDialog("warning", eval(callParent('lcm[113]')));
            $("#fileuploadsts").text("");
            return false;
        }
    }
}

function GetSelColData() {
    var colselected = [];
    var colselectedName = [];

    $("#gridImpData tr >th select:not([disabled])").each(function () {
        var col = $(this).find(":selected").val();
        var txt = $(this).find(":selected").text();
        if (col == "None") {
            colselected.push("None");
        } else {
            colselected.push(col);
            colselectedName.push(txt)
        }
    });

    var sorted_arr = colselected.slice().sort();
    var results = [];
    for (var i = 0; i < colselected.length - 1; i++) {
        if (sorted_arr[i + 1] == sorted_arr[i]) {
            results.push(sorted_arr[i]);
        }
    }
    if (results.length > 0) {
        showAlertDialog('warning', 1030, "client");
        focusSelectInGrid(colselected, results);
        return false;
    } else {
        var tempColNames = "", tempColValues = "";
        $("#gridImpData tr th").find("select:not([disabled])").each(function () {
            tempColNames += $(this).find("option[selected]").text() + ", ";
            tempColValues += $(this).val() + ", ";
        });
        tempColNames = tempColNames.substring(0, tempColNames.length - 2);
        tempColValues = tempColValues.substring(0, tempColValues.length - 2);

        $('#hdnColNames').val(tempColNames);
        $('#hdnColValues').val(tempColValues);

        $("#colheader").val(colselected.join(','));
        $("#colHeaderNames").val(colselectedName.join(','));
        unselectIgnoredColumns();

        var allowUpdate = $('#chkForAllowUpdate').prop("checked");
        if (allowUpdate) {
            primaryKeyCol = $("#ddlPrimaryKey").val();
            if (primaryKeyCol == "NA") {
                showAlertDialog("warning", "Please select Primary Key column");
                $("#ddlPrimaryKey").focus();
                return false;
            } else {
                $("#hdnPrimaryKey").val(primaryKeyCol);
                return true;
            }
        } else {
            return true;
        }
    }
}

//once tstuct select is selected - move all manadatory fields to right selection
function updateMandatoryFieldsToSelection() {
    $("#mSelectLeft option").each(function () {
        if ($(this).text().indexOf("*") >= 0) {
            $('#mSelectRight').append($('<option>', {
                value: $(this).val(), // .replace("*",""),
                text: $(this).text(), // .replace("*", ""),
                mandatory: true
            }));
            $(this).remove();
        } else {
            $(this).attr("mandatory", false);
        }
    });
}

/* Once tstuct's template is selected => Update all manadatory fields in right selection to "true", rest fields in left and right selection as "false" */
function updateMandatoryFieldsInTemplate() {
    $("#mSelectLeft option, #mSelectRight option").each((ind, item) => {
        if ($(item).text().indexOf("*") >= 0) {
            $(item).attr("mandatory", true);
        } else {
            $(item).attr("mandatory", false);
        }
    });
}

function ChkAllowUpdate() {
    if ($('#chkForAllowUpdate').prop("checked")) {
        $('#lblprimarycolmn,#ddlPrimaryKey').parent("div").removeClass("d-none");
        $("#ddlPrimaryKey").empty();
        $("#ddlPrimaryKey").append("<option value='NA'>-- Select --</option");
        colValues = $("#hdnColValues").val().split(', ');
        colNames = $("#hdnColNames").val().split(', ');
        for (var i = 0; i < colNames.length; i++) {
            $("#ddlPrimaryKey").append("<option value='" + colValues[i] + "'>" + colNames[i] + "</option");
        }
        disabledIgnoredColumns(ignoredColCount);
    } else {
        $('#lblprimarycolmn,#ddlPrimaryKey').parent("div").addClass("d-none");
    }
}

function ColNameInfileChanged() {
    // $("#ColHeaderClick").click();
}

function focusSelectInGrid(SelectedColValues, RepeatedColNames) {
    var result = [];

    for (i = 0; i < RepeatedColNames.length; i++) {
        for (j = 0; j < SelectedColValues.length; j++) {
            if (RepeatedColNames[i] == SelectedColValues[j]) {
                result.push(j);
            }
        }
    }
    for (i = 0; i < result.length; i++) {
        if (i == 1) {
            $("#gridImpData tbody tr th").eq(result[i]).find('select').focus();
        }
    }
}

function uploadFileChangeEvent() {
    $('#fileToUpload').change(function (e) {
        var uploadControl = $('#fileToUpload')[0].files[0];
        var regex = /^.*\.(CSV|csv|txt|TXT|XLS|xls|XLSX|xlsx)$/;
        if (uploadControl != undefined) {
            var Filename = uploadControl.name;
            if (Filename != "") {
                var fileSize = uploadControl.size / 1024 / 1024; // in MB
                if (regex.test(Filename)) {
                    if (fileSize > 1) {
                        showAlertDialog("warning", eval(callParent('lcm[156]')));
                        $("#noFile").text(eval(callParent('lcm[66]')));
                        $('#fileToUpload').val("");
                        $("#IsFileUploaded").val("");
                    } else {
                        $("#noFile").text(Filename);
                        $('#chkForAllowUpdate').attr("checked", false);
                        $("#hdnPrimaryKey").val("");
                        ChkAllowUpdate();
                        $("#btnFileUpload").click();
                    }
                } else {
                    showAlertDialog("warning", eval(callParent('lcm[157]')));
                    uploadControl.value = '';
                    $("#noFile").text(eval(callParent('lcm[66]')));
                    $('#fileToUpload').val("");
                    $("#IsFileUploaded").val("");
                }
            }
        } else {
            $("#noFile").text(eval(callParent('lcm[66]')));
            $("#IsFileUploaded").val("");
        }
    });
}

function uploadFileClickEvent() {
    $('#btnFileUpload').click(function () {
        var fileUpload = $("#fileToUpload").get(0);
        var files = fileUpload.files;
        var frm = new FormData();
        if (files.length == 0) {
            showAlertDialog("warning", eval(callParent('lcm[158]')));
        } else {
            for (var i = 0; i < files.length; i++) {
                frm.append(files[i].name, files[i]);
            }
            $(".progress").show();
            //upload file using Generic handler ashx file, once successfully uploaded display top 5 records in Grid
            var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx')); //to get base url of the website
            $.ajax({
                url: url + "FileUploadHandler.ashx",
                type: "POST",
                contentType: false,
                processData: false,
                data: frm,
                success: function (result) {
                    if (result.indexOf("File Uploaded successfully") == 0) {
                        var filename = result.substr(result.indexOf("&&") + 2);
                        $("#divProgressBar").removeClass("progress-bar-danger");
                        setTimeout(function () {
                            //to reset progress bar
                            $("#divProgressBar").removeClass("progress-bar progress-bar-striped active").addClass("progress-bar-success").text("100%").css("width", "100%").attr('aria-valuenow', "100");
                        }, 500);
                        // filename=$("#noFile").text()
                        $("#upFileName").val(filename);
                        $("#uploadFileName").val(files[0].name);
                        $("#UploadButton").click();
                    } else
                        showAlertDialog("warning", result);
                },
                error: function (err) {
                    showAlertDialog("warning", eval(callParent('lcm[159]')));
                    $("#divProgressBar").removeClass("progress-bar-striped active").addClass("progress-bar-danger").text("0%").css("width", "100%").attr('aria-valuenow', "100");
                },
                xhr: function () {
                    //upload Progress
                    var xhr = $.ajaxSettings.xhr();
                    xhr.upload.onprogress = function (event) {
                        var percent = 0;
                        var position = event.loaded || event.position;
                        var total = event.total;
                        if (event.lengthComputable) {
                            percent = Math.ceil(position / total * 100);
                        }
                        //update progressbar
                        $("#divProgressBar").addClass("progress-bar-striped").text(percent + "%").css("width", "100%").attr('aria-valuenow', percent);
                    };
                    return xhr;
                },
            });
        }
    });
}

function openTableInBSModal() {
    let modalElement = document.getElementById('tableModal');

    // Check if the modal already exists
    if (!modalElement) {
        // Create the modal dynamically if it doesn't exist
        const modalHTML = `
            <div class="modal fade" id="tableModal" tabindex="-1" aria-labelledby="tableModalLabel" aria-hidden="true">
                <div class="modal-dialog" style="max-width: 90vw; max-height: 90vh;">
                    <div class="modal-content">
                        <div class="modal-header" style="max-height: 40px;margin-top : 10px">
                            <h5 class="modal-title" id="tableModalLabel">Map Columns</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body" style="max-height: 62vh; overflow-y: auto;">
                            <!-- Table will be placed directly here -->
                        </div>
                        <div class="modal-footer" style="max-height: 60px;">
                            <button type="button" class="btn btn-primary" data-bs-dismiss="modal" style="position:relative;top:-13px;">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        // Append the modal to the body
        document.body.insertAdjacentHTML('beforeend', modalHTML);

        // Get the newly created modal element
        modalElement = document.getElementById('tableModal');
    }

    // Reference the modal body directly
    const modalBody = modalElement.querySelector('.modal-body');

    // Clear the modal body to avoid duplicates
    //modalBody.innerHTML = '';

    // Add the table (or any content you want to show)
    const table = document.getElementById('UpdateGrid'); // Replace with your table ID
    if (table) {
        //const clonedTable = table.cloneNode(true); // Clone the table to keep it intact
        //clonedTable.id = ''; // Remove ID to avoid duplication issues
        modalBody.appendChild(table);
    }

    // Show the modal using Bootstrap's Modal API
    const modalInstance = new bootstrap.Modal(modalElement);
    modalInstance.show();
}

// Example trigger
//document.getElementById('openTableButton').addEventListener('click', openTableInBSModal);

function fileUploadSuccess() {
    showAlertDialog('success', 4037, 'client');
    var uploadedFileName = $("#uploadFileName").val();
    $(".dropzone-select").text(uploadedFileName).addClass("col-10");
    //  $(".importFileDelete").removeClass("d-none")
    uploadFileClickEvent();
    uploadFileChangeEvent();

    addChkbxsToGrdColumns();
    ignoredColCount = parseInt($("#hdnIgnoredColCount").val());
    disabledIgnoredColumns(ignoredColCount);
    unselectIgnoredColumns();
    showPopover();//to show popover tooltip for hint
    filename = $("#uploadFileName").val();
    $("#noFile").text(filename);
    $("#fileToUpload").attr("title", filename);
}

//multi select control creation
function createMultiselectControl() {
    $('.multiselect').multiselect({
        beforeMoveToLeft: function ($left, $right, $options) {
            return false; //prevent by default left click functionality
        },
        sort: true,
        moveToLeft: function (Multiselect, $options, event, silent, skipStack) {
            targetId = $(event.target).attr("id");
            var selectionOptionCount = 0;
            if ($options.length == 1) { //if double clicks on right select option 
                if ($("#dataupdatecheck").is(":checked")) {
                    if ($("#tstFlds").val() != $options.text()) {
                        $('#mSelectLeft').append($('<option>', {
                            value: $options.val(),
                            text: $options.text(),
                            mandatory: false
                        }));

                        $options.remove();
                        return true;
                    }

                }
                else if ($options.attr("mandatory") == "false") {
                    $('#mSelectLeft').append($('<option>', {
                        value: $options.val(),
                        text: $options.text(),
                        mandatory: false
                    }));
                    $options.remove();
                    return true;
                }
                else {
                    showAlertDialog('warning', eval(callParent('lcm[160]')));
                }

            } else if (targetId == "left_Selected_1") { //if clicks on left select button to unselect selected options
                $("#mSelectRight :selected").map(function (i, el) {
                    if ($(el).attr("mandatory") == "true")
                        selectionOptionCount++;
                });
                if (selectionOptionCount == $options.length) {
                    showAlertDialog('warning', eval(callParent('lcm[161]')));
                }
            }

            var mandatoryCount = 0;
            $("#mSelectRight option").map(function (i, el) {
                var li = $(el).attr("mandatory");
                if (li == "true") {
                    mandatoryCount++;
                }
            });
            if ($("#dataupdatecheck").is(":checked")) {
                var selectOrAll = $(event.currentTarget).attr("id") == "left_All_1" ? "option" : ":selected";
                $("#mSelectRight " + selectOrAll).map(function (i, el) {
                    if ($("#tstFlds").val() != $(this).text()) {
                        $('#mSelectLeft').append($('<option>', {
                            value: $(this).val(),
                            text: $(this).text(),
                            mandatory: false
                        }));
                        $(this).remove();
                    }
                });
            }
            else {
                if (mandatoryCount > 1 && mandatoryCount == $options.length) {//if all right select fields are mandatory fields
                    showAlertDialog('warning', eval(callParent('lcm[161]')));
                } else {
                    var selectOrAll = $(event.currentTarget).attr("id") == "left_All_1" ? "option" : ":selected";
                    $("#mSelectRight " + selectOrAll).map(function (i, el) {
                        var li = $(el).attr("mandatory");
                        if (li == "false") {
                            leftSelectedClickedOnce = true;
                            $('#mSelectLeft').append($('<option>', {
                                value: $(this).val(),
                                text: $(this).text(),
                                mandatory: false
                            }));
                            $(this).remove();
                        }
                    });
                }
            }
        }
    });
}

//to check if the form is dirty before closing
function checkIfFormChanges() {
    var tstructForm = $("#ddlImpTbl").val();
    if (tstructForm != "NA")
        isFormDirty = true;
    else
        isFormDirty = false;
    return isFormDirty;
}

//text for Failed Summary report table headings
function setFailedSummaryColumnHeadings() {
    $("#thSummFileName").text(eval(callParent('lcm[263]')));
    $("#thSummRecords").text(eval(callParent('lcm[264]')));
    $("#thSummAdded").text(eval(callParent('lcm[357]')));
    $("#thSummUpdated").text(eval(callParent('lcm[358]')));
    $("#thSummFailed").text(eval(callParent('lcm[266]')));
    $("#hdnIgnoredColumns").val("");
    ignoredColumns = [];
}

//Edit wizard grid - add a checkbox option to select/unselect column after file uploaded successfully
function addChkbxsToGrdColumns() {
    var grd = $("#gridImpData");
    var selectExist = grd.data("column-select");
    // $('#gridImpData th select')
    $("#gridImpData > tbody > tr > th").each(function () {
        $(this).find('option').each(function () {
            var optionText = $(this).text().trim();  // Get the option's display text
            var optionValue = $(this).val();        // Get the option's value

            // Update text: "Employee ID (empid)" format
            if (!optionText.includes("(")) { // Avoid duplicating if already modified
                $(this).text(optionText + " (" + optionValue + ")");
            }
            $(this).val(optionValue).trigger("change");
        });
    });

    //$('#gridImpData th select').each(function () {
    //    $(this).find('option').each(function () {
    //        // Get the option text and value
    //        var optionText = $(this).text();
    //        var optionValue = $(this).val();

    //        // Set the new text as "OptionText (OptionValue)"
    //        $(this).text(optionText + " (" + optionValue + ")");
    //    });

    //    // Find the currently selected option and update its text
    //    var selectedOption = $(this).find('option:selected');
    //    var selectedText = selectedOption.text();
    //    var selectedValue = selectedOption.val();
    //    selectedOption.text(selectedText.split(' ')[0] + " (" + selectedValue + ")");
    //});
    if (selectExist == undefined) {
        $("#gridImpData th").each(function () {//> tbody > tr >
            $(this).css({
                "min-width": "160px",
            });
            $(this).find("select").addClass("form-select");
            // $(this).find("select").attr("data-control","select2");
            $(this).find("select").select2();
            $(this).find("select").css("width", "auto");
            selectId = $(this).find("select").attr("id");
            $(this).append("<label class='checkbox-inline'><br><input type='checkbox' class='grd-column-select me-2 w-20px h-20px' checked id='chk" + selectId + "'/><span>Select Column</span></label><i tabindex='0' data-trigger='focus' class='icon-arrows-question col-info' data-toggle='popover' data-content='Uncheck to ignore this column' data-placement='right' style='cursor: pointer;'  title=''></i>");
            //var thisText = $(this).text();
            //$(this).text("");   
            //$(this).append(`<div class='d-flex gap-2 form-check form-check-sm form-check-custom form-check-solid checkbox-inline'><input type='checkbox' class='form-check-input grd-column-select' checked id='chk" + selectId + "'/><label class="form-label fw-boldest">${thisText}</label><span tabindex='0' data-trigger='focus' class='material-icons material-icons-style material-icons-3 icon-arrows-question col-info' data-bs-toggle='tooltip' data-bs-original-title='Uncheck to ignore this column' data-bs-placement="bottom" data-bs-dismiss="click">help_outline</span></div>`);

            //select column change event 
            //$(this).find("select").change(function () {
            //    $("option[value=" + this.value + "]", this).attr("selected", true).siblings().removeAttr("selected");
            //    selectVal = $(this).val();
            //    selectId = $(this).attr("id");
            //    mandatory = $("#mSelectRight option[value='" + selectVal + "']").attr("mandatory");
            //    mandatory = mandatory != undefined ? JSON.parse(mandatory) : false;
            //    colIndex = $(this).closest("th").index();
            //    if (mandatory)
            //        $("#chk" + selectId).attr("disabled", "disabled");
            //    else
            //        $("#chk" + selectId).removeAttr("disabled");
            //});
            curselectVal = $(this).find("select").val();
            mandatory = $("#mSelectRight option[value='" + curselectVal + "']").attr("mandatory");
            mandatory = mandatory != undefined ? JSON.parse(mandatory) : false;
            mandatory ? $("#chk" + selectId).attr("disabled", "disabled") : "";
        });

        //grid column checkbox change event 
        $(".grd-column-select").change(function () {
            var selected = $(this).is(":checked");
            var colIndex = $(this).closest("th").index() + 1;
            if (selected) {
                $(this).closest("th").find("select").removeAttr("disabled");
                $("#gridImpData tr td:nth-child(" + colIndex + "), #gridImpData tr th:nth-child(" + colIndex + ")").removeClass("column-disabled");
                updateIgnoredColumns(colIndex, "pop");
            } else {
                $(this).closest("th").find("select").attr("disabled", "disabled");
                $("#gridImpData tr td:nth-child(" + colIndex + "), #gridImpData tr th:nth-child(" + colIndex + ")").addClass("column-disabled");
                updateIgnoredColumns(colIndex, "push");
            }

            var selDisabled = $(this).closest("th").find("select").is(":disabled");
            var selVal = $(this).closest("th").find("select").val();
            if (selDisabled) {
                $("#ddlPrimaryKey option[value='" + selVal + "']").attr("disabled", "disabled");
            } else {
                $("#ddlPrimaryKey option[value='" + selVal + "']").removeAttr("disabled");
            }
        });
        $("#gridImpData").attr("data-column-select", true);
    }

    try {
        KTApp?.initBootstrapTooltips();
    } catch (ex) { }
}

//once file uploaded successfully, if any column is ignored then disable those columns
function disabledIgnoredColumns(index) {
    if (index > 0) {
        --index;
        $("#gridImpData tr").each(function () {
            $(this).find("td:gt(" + index + "), th:gt(" + index + ")").addClass("column-disabled")
        });
        $("#gridImpData tr th:gt(" + index + ")").find("select").attr("disabled", "disabled");
        $("#gridImpData tr th").find("select[disabled]").each(function () {
            var val = $(this).val();
            $("#ddlPrimaryKey option[value='" + val + "']").attr("disabled", "disabled");
            updateIgnoredColumns($(this).parent().index() + 1, "push")
        });
        $("#gridImpData tr th:gt(" + index + ")").find(".checkbox-inline, .col-info").remove();

    }
}

function filepaths(filepathjs) {
    filepath = filepathjs
}
//if any column is ignored then unselect those field from the Data search wizard 
function unselectIgnoredColumns() {
    $("#gridImpData tr th").find("select[disabled]").each(function () {
        var val = $(this).val();
        var txt = $(this).find("option[selected]").text();
        if ($('#mSelectLeft option[value="' + val + '"]').length == 0) {
            $('#mSelectLeft').append($('<option>', {
                value: val,
                text: txt,
                mandatory: false
            }));
            $("#mSelectRight option[value='" + val + "']").remove();
        }
    });
    oldSelectedValues = $("#mSelectRight option").length;
}

//add ignored column index in hdnIgnoredColumns fld seperated by ','
function updateIgnoredColumns(val, type) {
    var ind = ignoredColumns.indexOf(val);
    if (type == "push") {
        if (ind === -1)
            ignoredColumns.push(val);
    } else {
        if (ind !== -1)
            ignoredColumns.splice(ind, 1);
    }
    $("#hdnIgnoredColumns").val(ignoredColumns.sort(((a, b) => a - b)));
}

window.addEventListener('error', function (e) {
    var error = e.error;
    console.log(error);
});

function GetImpData(redisip, redisport, redispwd, proj, token, seed, authkey, trace) {
    var result = false;
    var impfile = $("#upFileName").val();
    var fileNametoShow = $("#uploadFileName").val();
    var transid = $("#hdnFname").val();
    var primarykey = $('#select2-selectPrimaryKey-container').text();//$('#selectPrimaryKey').val();//.split("(")[1].slice(0,-1);
    var rowCount = (0.015 * parseInt($('#hdnRowCount').val())).toFixed(2);
    var queueSats = "In Progress";
    var updatedValue = $('#xlnum').text().replace(/(sheet\d+)/g, (match, p1, offset) => {
        return (offset > 0 && $('#xlnum').text()[offset - 1] !== ",") ? `, ${p1}` : p1;
    });
    if ($("#hdnmulForm").val() == "true" && $("#hdnuniqueNamesStringSheet").val() != "")
        transid = $("#hdnuniqueNamesStringSheet").val();
    if ($("#hdnmulForm").val() == "true" && $("#hdnuniqueNamesStringheaders").val() != "")
        primarykey = $("#hdnuniqueNamesStringheaders").val();
    updatedValue = updatedValue.replaceAll(', ,', ',').replaceAll(' ', '');
    if ($('#hdnUpdateMapcolumns').val() == "true") {
        var mapcols = $('#hdndheetheaders').val();
        const jsonObject = JSON.parse(mapcols);
        //const jsonObject = JSON.parse(jsonString);

        // Initialize an array to store the resulting strings
        let result = [];

        // Loop through the object and create the formatted string
        Object.keys(jsonObject).forEach(sheetKey => {
            const sheetValues = jsonObject[sheetKey];
            sheetValues.forEach((colValue, index) => {
                result.push(`sheet${sheetKey}.col${index + 1}=${colValue}`);
            });
        });
        updatedValue = "";
        // Join the result array into a single string
        updatedValue = result.join(", ");
        updatedValue = updatedValue.replaceAll(", ", ",");
    }
    var hasHeader = $('#hdnxlHasHeader').val();
    if (transid.indexOf("$$") != -1)
        hasHeader = "true";
    var sheetnames = $('#hdnsheetSequence').val();
    sheetnames = sheetnames.replaceAll(' ', '');
    ShowDimmer(true);
    var json = {
        "convertxltojson": {
            "redisserver": redisip,
            "redisport": redisport,
            "redispwd": redispwd,
            "project": proj,
            "s": "$SESSIONID$",
            "username": "$USERNAME$",
            "signalrurl": "$SIGNALRURL$",
            "armscripturl": "$ARMSCRIPTURL$",
            "transid": transid,
            "keyfield": primarykey,
            "importfile": impfile,
            "sheetnames": sheetnames,
            "trace": trace,
            "mapcolumns": updatedValue,
            "hasheader": hasHeader
        }
    };

    var settings = {
        url: '../aspx/ImportAll.aspx/PushToImportQueue',
        type: 'POST',
        cache: false,
        async: false,
        dataType: 'json',
        data: JSON.stringify({ queueData: JSON.stringify(json), transid: transid }),
        contentType: "application/json"
    }
    $.ajax(settings).done(function (response) {
        if (response.d != "" && response.d == "keyexist") {
            result = "error";
            ShowDimmer(false);
            showAlertDialog('warning', "Previous request still in process. Please wait till you get the notification");
        } else if (response.d && !response.d.startsWith("RMQError:") && JSON.parse(response.d) && JSON.parse(response.d)?.result?.success == true) {
            result = true;
        } else if (response.d.startsWith("RMQError:")) {
            result = "error";
            ShowDimmer(false);
            showAlertDialog("error", response.d.replace("RMQError:", ""));
        } else if (response.d.startsWith("Error:")) {
            result = false;
        }
    });

    var messageDiv = document.getElementById("imWizardEdit");

    if (result.toString() != "error") {
        // Update the content and apply styling classes
        if (result) {
            ShowDimmer(false);
            callParentNew("btn-close", "class")[0].click();
            showAlertDialog("success", "Your Excel file has been successfully uploaded and queued for processing. The file will be processed shortly, and you will be notified upon completion.You can continue with other tasks while the import is being handled.");
            //  messageDiv.innerHTML = `<h3>File Queued for Processing</h3><p>Your Excel file has been successfully uploaded and queued for processing. The file will be processed shortly, and you will be notified upon completion.You can continue with other tasks while the import is being handled.</p><div><h4>Additional Details:</h4><ul><li><strong>File Name:</strong> ${fileNametoShow}</li><li><strong>Queue Status:</strong> ${queueSats}</li><li><strong>Expected Processing Time:</strong> ~${rowCount} minutes</li></ul></div>`; 
            // messageDiv.className = "form-label col-form-label pb-1 fw-boldest"; // Add the styling classes
            // messageDiv.style.display = "block"; // Ensure the div is visible
        } else {
            ShowDimmer(false);
            callParentNew("btn-close", "class")[0].click();
            showAlertDialog("error", "An error occurred while processing the file. Please ensure that the file is correct and the environment is properly set up. Refer to the logs for more details and try again.");
            // messageDiv.innerHTML = "An error occurred while processing the file. Please ensure that the file is correct and the environment is properly set up. Refer to the logs for more details and try again.";
            // messageDiv.className = "form-label col-form-label pb-1 fw-boldest";
        }
    }
    return result;
}

function SuccessCallbackRestSave(result, eventArgs) {
    try {
        var resJson = $j.parseJSON(result);
    } catch (ex) {
        ShowDimmer(false);
    }
    if (resJson != undefined && resJson.status == "success") {
        callParentNew("btn-close", "class")[0].click();
        showAlertDialog("success", "Your Excel file has been successfully uploaded and queued for processing. The file will be processed shortly, and you will be notified upon completion.You can continue with other tasks while the import is being handled.");
        //
    }
    else {
        callParentNew("btn-close", "class")[0].click();
        showAlertDialog("error", "An error occurred while processing the file. Please ensure that the file is correct and the environment is properly set up. Refer to the logs for more details and try again.");
    }
}

function OnExceptionRestSave(result, eventArgs) {
    //
}

function showPopover() {
    $('[data-bs-toggle="popover"]').popover({
        placement: placement
    });
}


function DropzoneInitImport() {
    var id = "#dropzone_AxpFileImport",
        dropzone = document.querySelector(id),
        previewNode = dropzone.querySelector(".dropzone-item");
    
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    var url = location.origin + location.pathname.substr(0, location.pathname.indexOf("aspx"));
    
    var myDropzone = new Dropzone(id, {
        url: url + "FileUploadHandler.ashx",
        previewTemplate: previewTemplate,
        addRemoveLinks: true,
        maxFilesize: 10, // Increased to 50MB
        previewsContainer: id + " .dropzone-items",
        clickable: id + " .dropzone-select",
        // timeout: 600000 // 10 minutes timeout
    });

    myDropzone.on("addedfile", function(file) {
        dropzone.querySelectorAll(".dropzone-item").forEach(function(id) {
            id.style.display = "";
        });
    });

    myDropzone.on("totaluploadprogress", function(progress) {
        dropzone.querySelectorAll(".progress-bar").forEach(function(bar) {
            bar.style.width = progress + "%";
        });
    });

    myDropzone.on("error", function(file, errorMessage) {
        alert("Upload failed: " + errorMessage);
    });

    myDropzone.on("complete", function (file) {
        if ($("#selectform").length > 0 && $("#selectform").val() == null) {
            ShowImportError("Please choose form before upload the file.");
            return;
        }
        var responseText = file.xhr.responseText;
        if (!responseText.includes("successfully")) {
            alert("Upload Error: " + responseText);
        } else {
            var fileNameModified = responseText.split("&&")[1];
            $("#upFileName").val(fileNameModified);
            $("#uploadFileName").val(file.name);
            $("#hdnOriginalfileName").val($("#uploadFileName").val());
            $("#UploadButton").click();
        }

        setTimeout(function() {
            dropzone.querySelectorAll(".dz-complete").forEach(function(id) {
                id.querySelector(".progress-bar").style.opacity = "0";
                id.querySelector(".progress").style.opacity = "0";
            });
        }, 300);
    });
}


//function DropzoneInitImport() {
//    const id = "#dropzone_AxpFileImport";
//    const dropzone = document.querySelector(id);

//    //get the preview element template
//    var previewNode = dropzone.querySelector(".dropzone-item");
//    previewNode.id = "";
//    var previewTemplate = previewNode.parentNode.innerHTML;
//    previewNode.parentNode.removeChild(previewNode);
//    var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));
//    var myDropzone = new Dropzone(id, { // Make the whole body a dropzone
//        url: url + "FileUploadHandler.ashx", // Set the url for your upload script location
//        // parallelUploads: 20,
//        previewTemplate: previewTemplate,
//        addRemoveLinks: true,
//        maxFilesize: 1, // Max filesize in MB
//        // autoQueue: false, // Make sure the files aren't queued until manually added
//        previewsContainer: id + " .dropzone-items", // Define the container to display the previews
//        clickable: id + " .dropzone-select" // Define the element that should be used as click trigger to select files.
//    });
//    //.addRemoveLinks = true
//    myDropzone.on("addedfile", function (file) {
//        // Hookup the start button
//        const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
//        dropzoneItems.forEach(dropzoneItem => {
//            dropzoneItem.style.display = '';
//        });
//    });

//    // Update the total progress bar
//    myDropzone.on("totaluploadprogress", function (progress) {
//        const progressBars = dropzone.querySelectorAll('.progress-bar');
//        progressBars.forEach(progressBar => {
//            progressBar.style.width = progress + "%";
//        });
//    });
//    myDropzone.on("removedfile", function (file) {
//        // debugger;
//    });

//    // Hide the total progress bar when nothing's uploading anymore
//    myDropzone.on("complete", function (progress) {
//        const progressBars = dropzone.querySelectorAll('.dz-complete');
//        var responseText = progress.xhr.responseText;
//        var fileNameModified = progress.xhr.responseText.substr(progress.xhr.responseText.indexOf("&&") + 2);

//        $("#upFileName").val(fileNameModified);
//        $("#uploadFileName").val(progress.name);
//        $("#hdnOriginalfileName").val($("#uploadFileName").val());
//        //FileuplaodValidation();
//        $("#UploadButton").click();
//        // $("#upFileName").val(progress.name); 
//        setTimeout(function () {
//            progressBars.forEach(progressBar => {
//                progressBar.querySelector('.progress-bar').style.opacity = "0";
//                progressBar.querySelector('.progress').style.opacity = "0";
//                // progressBar.querySelector('.dropzone-start').style.opacity = "0";
//            });
//        }, 300);
//    });
//}

//// Generate the table based on selected DC
//function generateTable(dcKey) {
//    var jsonData = [];

//    // Prepare data for rows dynamically
//    for (var key in dataarray) {
//        if (dataarray[key][dcKey] && dataarray[key][dcKey].row1) {
//            jsonData.push(dataarray[key][dcKey].row1);
//        }
//    }

//    var gridData = "<table class='table w-100' id='gridImpData'><thead><tr>";

//    // Generate header row with Select2 dropdowns
//    if (jsonData.length > 0) {
//        for (var key in jsonData[0]) {
//            gridData += "<th><select class='select2Header' style='width: 100px;'><option value='" + key + "'>" + key + "</option></select></th>";
//        }
//    }
//    gridData += "</tr></thead><tbody>";

//    // Generate data rows
//    jsonData.forEach(function(row) {
//        gridData += "<tr>";
//        for (var key in row) {
//            gridData += "<td>" + row[key] + "</td>";
//        }
//        gridData += "</tr>";
//    });

//    gridData += "</tbody></table>";
//    $('#UpdateGrid').html(gridData);

//    // Initialize Select2 on header dropdowns
//    $('.select2Header').select2();
//}

//// Event listener for DC selection
//$('#dcSelect').on('change', function () {
//    var selectedDC = $(this).val();
//    generateTable(selectedDC);
//});

//// Initialize the table with the first DC by default
//$(document).ready(function () {
//    generateTable('dc1');
//});
$(document).ready(function () {
    // $("#tstFlds").append("<option value='NA'>-- Select --</option");

    //$('#select2Dropdown1, #select2Dropdown2').select2();
    addSheetRow1();
    $("#selectform").select2();
    $('#selectPrimaryKey').select2();
    //getHeadersForSheet();
    $('#mulForm').on('change', function () {
        if ($(this).is(':checked')) {
            // Code to execute when the checkbox is checked
            $('#selectform, #selectPrimaryKey').prop('disabled', true);
            $('#hdnmulForm').val("true");
            $("#xlnum").addClass("d-none");

        } else {
            // Code to execute when the checkbox is unchecked
            $('#selectform, #selectPrimaryKey').prop('disabled', false);
            $('#hdnmulForm').val("false");
            $("#xlnum").removeClass("d-none");
            if ($("#hdnaxmsgid").val() != "") {
                $('#selectform').attr("disabled", true);
                $('#selectPrimaryKey').attr("disabled", true);

            }

        }
    });
    $("#selectform").select2();
    $('#selectPrimaryKey').select2();
    if ($("#hdnaxmsgid").val() != "") {
        ShowDimmer(true);

        $.ajax({
            url: 'ImportAll.aspx/GETtabledata',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                tempname: $("#hdnaxmsgid").val(),
            }),
            dataType: 'json',
            contentType: "application/json",
            success: (data) => {
                let _impData = data.d.replace(/[\r\n]+/g, "\\n");
                var datas = JSON.parse(_impData);
                let transid = datas.rapidsave.transid;
                //if (transid.includes("$$")){
                //    $('#selectform, #selectPrimaryKey').prop('disabled', true);
                //    $('#hdnmulForm').val("true");
                //    $('#btnDownloadxl').insertBefore('#selectform');
                //    $("#btnDownloadxl").removeClass("d-none");

                //}
                //else{
                $('#hdnFname').val(datas.rapidsave.transid);
                if (transid.includes("("))
                    transid = transid.split('(')[1].split(')')[0];
                let keyfield = datas.rapidsave.keyfield;
                $('#hdnFnameKey').val(datas.rapidsave.keyfield);
                keyfield = keyfield.split('(')[1].split(')')[0];
                let impfilewithsummary = datas.rapidsave.impfilewithsummary;
                fileUrl = impfilewithsummary;
                $('#hdnExcelfilepath').val(fileUrl);
                var filenameExcel = fileUrl.substring(fileUrl.lastIndexOf("\\") + 1);
                filenameExcel = decodeURIComponent(filenameExcel);
                $('#hdnExcelfileName').val(filenameExcel);
                let mapcoloumns = datas.rapidsave.mapcolumns;//"sheet1.col1=Employee ID(empid), sheet1.col2=Employee Name(empname), sheet1.col3=Employee DOB(empdob) sheet2.col1=Employee ID(empid), sheet2.col2=Month(month), sheet2.col3=Salary(salary) sheet3.col1=Employee ID(empid), sheet3.col2=Month(lMonth), sheet3.col3=Leave Days(leavedays)";//datas.rapidsave.mapcolumns;
                $('#selectform').val(transid).trigger('change');
                $('#selectform').attr("disabled", true);
                $.ajax({
                    url: 'ImportAll.aspx/GetFields',
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: JSON.stringify({
                        transid: transid,
                    }),
                    dataType: 'json',
                    contentType: "application/json",
                    success: (data) => {
                        var datas = JSON.parse(data.d);
                        var visibleTstFlds = datas.VisibleTstFlds.m_StringValue;
                        const items = datas.visibleTstFldsGridDc.m_StringValue.slice(0, -1).split(',');

                        // Step 2: Create an object to group values by DC number
                        const groupedDC = {};

                        // Step 3: Iterate through the items and group them by their DC number
                        items.forEach(item => {
                            const [field, dc] = item.split('♠'); // Split the field and the DC number
                            if (!groupedDC[dc]) {
                                groupedDC[dc] = []; // Initialize an array if the DC number is new
                            }
                            groupedDC[dc].push(field); // Add the field to the appropriate group
                        });

                        // Step 4: Convert the grouped object into an array of arrays dynamically
                        const result = Object.entries(groupedDC).map(([dc, fields]) => ({
                            //dc: dc,
                            fields: fields
                        }));

                        const itemsnongrid = datas.visibleTstFldsNonGriddc.m_StringValue.slice(0, -1).split(',');

                        // Step 2: Create an object to group values by DC number
                        const groupedDCnongrid = {};

                        // Step 3: Iterate through the items and group them by their DC number
                        itemsnongrid.forEach(item => {
                            const [field, dc] = item.split('♠'); // Split the field and the DC number
                            if (!groupedDCnongrid[dc]) {
                                groupedDCnongrid[dc] = []; // Initialize an array if the DC number is new
                            }
                            groupedDCnongrid[dc].push(field); // Add the field to the appropriate group
                        });

                        // Step 4: Convert the grouped object into an array of arrays dynamically
                        const resultnongrid = Object.entries(groupedDCnongrid).map(([dc, fields]) => ({
                            //dc: dc,
                            fields: fields
                        }));

                        const combined = [...resultnongrid, ...result];

                        // Sort combined data by DC number (numerically)
                        combined.sort((a, b) => parseInt(a.dc) - parseInt(b.dc));

                        // Create arrays for each DC number dynamically
                        arraysByDC = combined.map(entry => entry.fields);

                        // Example to access arrays by DC numbering
                        arraysByDC.forEach((fields, index) => {
                            console.log(`Array for DC ${combined[index].dc}:`, fields);
                        });


                        // var dccount = parseInt(data.d).toString()
                        //ShowDimmer(false);
                        var separator = ',♠';
                        var separatorIndex = visibleTstFlds.indexOf(separator); // Find the index of the separator
                        var newStr = "";
                        var trueValue = "";
                        if (separatorIndex !== -1) {
                            newStr = visibleTstFlds.substring(0, separatorIndex); // Extract substring before the separator
                            trueValue = visibleTstFlds.substring(separatorIndex + separator.length); // Extract "true" after the separator
                        }
                        $("#hdnFieldnamesExcelnew").val(newStr);
                        var PkeyArray = newStr.split(",");
                        var Primarykey = "#selectPrimaryKey";
                        $(Primarykey).empty();
                        var newArray = PkeyArray.map(function (item) {
                            var match = item.match(/\((.*?)\)/); // Regular expression to extract text inside ()
                            return match ? match[1] : ''; // Return the matched value, or an empty string if not found
                        });
                        // Extracted values inside ()

                        // Step 3: Append options to the Select2 dropdown
                        PkeyArray.forEach(function (option, index) {
                            $(Primarykey).append(
                                $('<option></option>')
                                    .attr('value', newArray[index]) // Set value from the new array
                                    .text(option)                  // Set text from the original array
                            );
                        });
                        $('#selectPrimaryKey').val(keyfield).trigger('change');
                        $('#selectPrimaryKey').attr("disabled", true);
                        $('#btnSaveTemplateXl').addClass("d-none");
                        $("#xlnum").addClass("form-control");
                        $('#xlnum').text(mapcoloumns);
                        //var downloadButton = document.createElement("button");
                        //downloadButton.innerText = "Download File"; // Button text
                        $('#btnDownloadxl').insertBefore('#selectform');
                        $("#btnDownloadxl").removeClass("d-none");
                        ShowDimmer(false);
                    }
                    // }
                })
                //}
            }
        })
    }

    $("#btnDownloadxl").click(function () {

        // window.location.href ="C:\\\\importexcel\\Download\\empdtimportdata.xlsx";// "\\\\localhost\\importexcel\\Download\\empdtimportdata.xlsx";//fileUrl;
        //const fileUrl ="";
        //const anchor = document.createElement('a');
        //anchor.href = fileUrl;
        //anchor.download = 'empdtimportdata.xlsx'; // Specify the filename
        //document.body.appendChild(anchor);
        //anchor.click();
        //document.body.removeChild(anchor);
        var filepathxl = fileUrl;//"\\\\localhost\\importexcel\\Download\\empdtimportdata.xlsx";
        // var filenameExcel =filepathxl.substring(fileUrl.lastIndexOf("\\") + 1);
        // filenameExcel = decodeURIComponent(filenameExcel);

        $.ajax({
            url: 'ImportAll.aspx/DownloadExcelFile',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                filepathxl: filepathxl,
            }),
            dataType: 'json',
            contentType: "application/json",
            success: (data) => {



                // const link = document.createElement('a');
                // link.href = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + response.d;
                // link.download =filenameExcel ;
                // link.click();

            }
        })

    })
    // $("#btnDownloadxl").click(function () {
    //     // Create an invisible anchor element
    //     var anchor = document.createElement("a");
    //     anchor.href = "\\\\localhost\\importexcel\\Download\\empdt importdata.xlsx"//fileUrl;
    //     let fileName = fileUrl.substring(fileUrl.lastIndexOf("\\") + 1); // Extracts "empdt%20importdata.xlsx"
    //     fileName = decodeURIComponent(fileName);
    //     // Set the download attribute using the extracted file name
    //     anchor.download = fileName;

    //     // Append the anchor to the body and trigger the click event
    //     document.body.appendChild(anchor);
    //     anchor.click();

    // Clean up: Remove the anchor element
    //     document.body.removeChild(anchor);
    // });
    var tstFldsArr = $("#hdnTstructflds").val().slice(0, -1).split(",");
    var collapseButton = $("<button>")
        .addClass("btn btn-primary mb-4")
        .attr("type", "button")
        .attr("data-toggle", "collapse")
        .attr("data-target", ".sheetCollapse")
        .text("Show/Hide Additional Sheets");

    $("#dropdownsContainer").prepend(collapseButton);
    $(document).on('shown.bs.collapse', function () {
        $(".sheetselect").select2();
    });
    //$(document).on('shown.bs.collapse', function () {
    //    $(".sheetselect").select2();
    //});
    $("#DropDownList1").select2();
    $("#DropDownList2").select2({
        allowClear: false,
        // data: [{ id: '', text: '--Select--' }, ...tstFldsArr],
        // data: tstFldsArr,
    }).on('select2:select', function () {
        var val = $(this).val();
        $.ajax({
            url: 'ImportAll.aspx/GETdata',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                tempname: val,
            }),
            dataType: 'json',
            contentType: "application/json",
            success: (data) => {
                //        var newOption = new Option(data.d.split("~")[0], data.d.split("~")[0], true, true);
                //$('#selectform').append(newOption).trigger('change');
                //var valuesArray = data.d.split("~")[1].split(',');

                //// Add any missing options dynamically
                //valuesArray.forEach(function(value) {
                //    if ($('#selectDC').find("option[value='" + value + "']").length === 0) {
                //        var newOption = new Option(value, value, false, false);
                //        $('#selectDC').append(newOption);
                //    }
                //});

                //// Set the selected values
                //$('#selectDC').val(valuesArray).trigger('change');
                ////var newOption1 = new Option(data.d.split("~")[0], data.d.split("~")[0], true, true);
                ////$('#selectDC').append(newOption1).trigger('change');
                //var newOption2 = new Option(data.d.split("~")[2], data.d.split("~")[2], true, true);
                //$('#selectPrimaryKey').append(newOption2).trigger('change');

            }
        })
    })
    $("#DropDownList1").addClass("d-none");
    $("#DropDownList1").removeClass("select2-hidden-accessible");
    // Function to add a row for each sheet
    //function addSheetRow(sheetName) {
    //    // Create a new row container
    //    var rowContainer = $("<div>").addClass("sheetRow");
    //    var section = $("<section>").addClass("form-group col-md-12 mb-4").text(sheetName);
    //    rowContainer.append(section);
    //    var formArr = $("#hdnformQueue").val().split(',');
    //    // Create and append dropdowns for choose form, choose dc, choose primary key, and choose group by
    //    var selectForm = createSelect("Choose Form", formArr);
    //    var selectDC = createSelect("Choose DC", ["DC 1", "DC 2", "DC 3"]);
    //    var selectPrimaryKey = createSelect("Choose Primary Key", ["Key 1", "Key 2", "Key 3"]);
    //    var selectGroupBy = createSelect("Choose Group By", ["Group 1", "Group 2", "Group 3"]);

    //    rowContainer.append(selectForm, selectDC, selectPrimaryKey, selectGroupBy);
    //    // Create and append normal button
    //    var normalButton = $("<button>").addClass("btn btn-primary").text("Map");
    //    rowContainer.append(normalButton);

    //    // Create and append toggle button
    //    //var toggleCheckboxDiv = $("<div>").addClass("form-check form-switch form-check-custom form-check-solid px-1 align-self-end mb-4");
    //    //var toggleButton = $("<input>").attr("type", "checkbox").addClass("m-wrap placeholder-no-fix form-check-input h-25px w-45px my-2").text("Map in File");
    //    //var togglespan = $("<span>").addClass("form-check-label form-label col-form-label pb-1 fw-boldest text-dark fs-6 mb-0").text("Map in File");
    //    //toggleCheckboxDiv.append(toggleButton);
    //    //toggleCheckboxDiv.append(togglespan);
    //    //rowContainer.append(toggleCheckboxDiv);

    //    // Append the row container to the main container
    //    $("#dropdownsContainer").append(rowContainer);
    //}
    function createSelect(label, options) {
        var select = $("<select>").addClass("select2 form-select forValidation selectpicker mb-4");
        select.append($("<option>").text(label).attr("disabled", true).attr("selected", true));
        options.forEach(function (option) {
            select.append($("<option>").text(option));
        });
        return select;
    }
    $("#tstFlds").select2({
        allowClear: false,
        data: [{ id: '', text: '--Select--' }, ...tstFldsArr],
        // data: tstFldsArr,
    }).on('select2:select', function (selectEv) {
        try {
            let _thisEle = $(selectEv.currentTarget).data("select2")?.$element[0]?.id;
            let _thisSelval = $(`#${_thisEle}`).val();
            var tstfld = { fields: [] };
            var tstfldRight = { field: [] };

            if (_thisSelval != "" && _thisSelval.indexOf("--Select--") == -1) {
                var partsright = _thisSelval.split('(');
                var valueright = partsright[1].replace(')', '').trim();
                tstfldRight.field.push({
                    val: valueright,
                    text: _thisSelval
                });
                $("#mSelectLeft  option").remove();
                $("#mSelectRight  option").remove();
                $.each(tstfldRight.field, (ind, item) => {
                    $("#mSelectRight").append($('<option>', {
                        value: item.val,
                        text: item.text,
                    }));
                });
            }
            else {
                $("#mSelectLeft  option").remove();
                $("#mSelectRight  option").remove();
            }
            tstFldsArr.forEach(function (field) {
                // Split the field into name and value
                var parts = field.split('(');
                var name = parts[0].trim();
                var value = parts[1].replace(')', '').trim();

                // Create an object for each field and push it to the fields array
                tstfld.fields.push({
                    val: value,
                    text: field
                });
            });
            var filteredTstfld = {
                fields: tstfld.fields.filter(function (field) {
                    return !((field.text === tstfldRight.field[0].text && field.val === tstfldRight.field[0].val));
                })
            };
            $.each(filteredTstfld.fields, (ind, item) => {
                $("#mSelectLeft").append($('<option>', {
                    value: item.val,
                    text: item.text,
                }));
            });
            updateMandatoryFieldsInTemplate();
        }

        catch (error) { }
    });
    $("#dataupdatecheck").change(function () {
        // Check the state of the checkbox
        if ($(this).is(":checked")) {
            // If checked, show the div
            $("#dataupdate").removeClass("d-none");
            $("#mSelectLeft  option").remove();
            $("#mSelectRight  option").remove();
            var tstfld = { fields: [] };
            var tstfldRight = { field: [] };
            if ($("#tstFlds").val() != "" && $("#tstFlds").val().indexOf("--Select--") == -1) {
                var partsright = $("#tstFlds").val().split('(');
                var valueright = partsright[1].replace(')', '').trim();
                tstfldRight.field.push({
                    val: valueright,
                    text: $("#tstFlds").val()
                });
                tstFldsArr.forEach(function (field) {
                    // Split the field into name and value
                    var parts = field.split('(');
                    var name = parts[0].trim();
                    var value = parts[1].replace(')', '').trim();

                    // Create an object for each field and push it to the fields array
                    tstfld.fields.push({
                        val: value,
                        text: field
                    });
                });
                var filteredTstfld = {
                    fields: tstfld.fields.filter(function (field) {
                        return !(field.text === tstfldRight.field[0].text && field.val === tstfldRight.field[0].val);
                    })
                };
                $.each(filteredTstfld.fields, (ind, item) => {
                    $("#mSelectLeft").append($('<option>', {
                        value: item.val,
                        text: item.text,
                    }));
                });
                $.each(tstfldRight.field, (ind, item) => {
                    $("#mSelectRight").append($('<option>', {
                        value: item.val,
                        text: item.text,
                    }));
                });
            }
            updateMandatoryFieldsInTemplate();
        } else {
            $("#mSelectLeft  option").remove();
            $("#mSelectRight  option").remove();
            if ($("#hdnMandatoryFields").val() != "") {
                var hdnMandatoryFieldsLines = $("#hdnMandatoryFields").val().split('\n').filter(line => line.trim() !== '');
                var hdnMandatoryFieldsExtractedValues = hdnMandatoryFieldsLines.map(line => {
                    var startIndex = line.indexOf('(');
                    var endIndex = line.lastIndexOf(')');

                    if (startIndex !== -1 && endIndex !== -1 && endIndex > startIndex) {
                        return line.substring(0, startIndex) + line.substring(startIndex, endIndex + 1);
                    }

                    return '';
                }).join(',');
                var hdnMandatoryFieldsParts = hdnMandatoryFieldsExtractedValues.split(',');
                var hdnMandatoryFieldsTrimmedString = hdnMandatoryFieldsParts.map(part => part.trim()).join(',');
                var hdnMandatoryFieldsPartsNxt = hdnMandatoryFieldsTrimmedString.split(',');
                var hdnMandatoryFieldsAct = hdnMandatoryFieldsPartsNxt.filter(part => part.trim() !== '').map(part => part.trim());
                var resultArrayLft = $("#hdnLeftFlds").val().split(",").filter(element => !hdnMandatoryFieldsAct.includes(element));
                var tstfldRight = { field: [] };
                var tstfld = { field: [] };
                //tstfldRight.field.push({
                //    val: valueright,
                //    text: hdnMandatoryFieldsAct
                //});
                resultArrayLft.forEach(function (field) {
                    // Split the field into name and value
                    var parts = field.split('(');
                    var name = parts[0].trim();
                    var value = parts[1].replace(')', '').trim();

                    // Create an object for each field and push it to the fields array
                    tstfld.field.push({
                        val: value,
                        text: field
                    });
                });
                $.each(tstfld.field, (ind, item) => {
                    $("#mSelectLeft").append($('<option>', {
                        value: item.val,
                        text: item.text,
                    }));
                });
                hdnMandatoryFieldsAct.forEach(function (field) {
                    // Split the field into name and value
                    var parts = field.split('(');
                    var name = parts[0].trim();
                    var value = parts[1].replace(')', '').trim();

                    // Create an object for each field and push it to the fields array
                    tstfldRight.field.push({
                        val: value,
                        text: field
                    });
                });
                $.each(tstfldRight.field, (ind, item) => {
                    $("#mSelectRight").append($('<option>', {
                        value: item.val,
                        text: item.text,
                    }));
                });
                updateMandatoryFieldsInTemplate();
            }
            else {
                var tstfldLft = { field: [] };
                var leftFlds = $("#hdnLeftFlds").val().split(",")
                leftFlds.forEach(function (field) {
                    // Split the field into name and value
                    var parts = field.split('(');
                    var name = parts[0].trim();
                    var value = parts[1].replace(')', '').trim();

                    // Create an object for each field and push it to the fields array
                    tstfldLft.field.push({
                        val: value,
                        text: field
                    });
                });
                $.each(tstfldLft.field, (ind, item) => {
                    $("#mSelectLeft").append($('<option>', {
                        value: item.val,
                        text: item.text,
                    }));
                });
                updateMandatoryFieldsInTemplate();
            }
            // If unchecked, hide the div
            $("#dataupdate").addClass("d-none");;
        }
    });
    var element = document.querySelector("#kt_stepper_example_clickable");

    // Initialize Stepper
    var stepper = new KTStepper(element);

    // Handle navigation click
    stepper.on("kt.stepper.click", function (stepper) {
        if (!stepper.stepped) {
            return;
        }
        stepper.goTo(stepper.getClickedStepIndex()); // go to clicked step
    });

    // Handle next step
    stepper.on("kt.stepper.next", function (stepper) {

        // stepper.goNext(); // go next step
        if (stepper.getCurrentStepIndex() == 1) {
            if ($("#uploadFileName").val() != "") {
                $("#hdnSheetnumber").val($('#selectsheet option').length.toString());
                $("#btnImport").click();
                // stepper.goNext();

                // Function to add a row for each sheet

                //function createSelect(label, options, id) {
                //    var select = $("<select>").addClass("select2").attr("id", id);
                //    select.append($("<option>").text(label).attr("disabled", true).attr("selected", true));
                //    options.forEach(function(option, index) {
                //        select.append($("<option>").text(option).attr("value", formArrVal[index]));
                //    });
                //    return select;
                //}
                //    validateDataSearchWiz();
                //    if (validateDataSearchWiz() == true) {
                //        stepper.goNext();
                //        $("#hdnupdateField").val( $("#tstFlds").val());
                //        $("#btnSaveTemplate").addClass("d-none");
                return;
            }

            else {
                showAlertDialog("warning", appGlobalVarsObject.lcm[113]);
                return;
            }
            //    } else {
            //        return;
            //    }
        }
        if (stepper.getCurrentStepIndex() == 2) {

            $("#hdnsltForm").val($('#selectform').val());
            $("#hdnsltDC").val($('#selectDC').val().toString().toLowerCase());
            $("#hdnsltTemp").val($('#projectName').val());
            $("#hdnsltFormFull").val($('#select2-selectform-container').text());
            $("#hdnsltPkey").val($('#selectPrimaryKey').val());
            // $("#btnImport").click();

            // $("#btnxlrest").click();
            stepper.goNext();
            //validateDataUploadWiz();
            //if (validateDataUploadWiz() == true) {
            //    $("#ColHeaderClick").click();
            //    stepper.goNext();
            //    $("#btnSaveTemplate").addClass("d-none");
            return;
            //} else {
            //    return;
            //}
        }
        //if (stepper.getCurrentStepIndex() == 3) {
        //    //if (GetSelColData()) {
        //    //    $(".card-footer").addClass("d-none");

        //    //    ShowDimmer(true);
        //        stepper.goNext();
        //        //$("#btnSaveTemplate").addClass("d-none");
        //        return;
        //    //}
        //    //return;
        //}
        //if (stepper.getCurrentStepIndex() == 4) {
        //    $("#btnSaveTemplate").addClass("d-none");
        //    $("#btnImport").click();
        //    if ($("#fileUploadComplete").val() == "1")
        //        return true;
        //    return false;
        //}
    });

    // Handle previous step
    stepper.on("kt.stepper.previous", function (stepper) {
        stepper.goPrevious(); // go previous step

        if (stepper.getCurrentStepIndex() == 1) {
            $("#btnSaveTemplate").removeClass("d-none");
        }
    });
    $("#ddlImpTbl, #ddlSeparator, #ddlPrimaryKey").select2();
    if ($("#ddlGroupBy").prop("disabled") != true)
        $("#ddlGroupBy").select2();

    try {
        if ($("#hdnTransid").val() != "") {
            GetTemplates();
        }

        $("#ddlImpTemplate").select2({
            allowClear: false,
            data: impTempObj.ddList,
            placeholder: appGlobalVarsObject.lcm[441],
        }).on('select2:select', function (selectEv) {
            try {
                let _thisEle = $(selectEv.currentTarget).data("select2")?.$element[0]?.id;
                let _thisSelval = $(`#${_thisEle}`).val() || selectEv.params.data.id;

                if (typeof _thisSelval != "undefined" && _thisSelval != "" && _thisSelval != "NA") {
                    impTempObj.selected = impTempObj.sqlRes.filter((itm) => {
                        if (itm.templatename == _thisSelval) {
                            return itm;
                        }
                    });

                    $("#mSelectLeft, #mSelectRight").empty();

                    let _allFlds = $("#hdnLeftFlds").val().split(",");
                    let _rSelect = (impTempObj.selected[0].impfields).split(",");
                    let _lSelect = _allFlds.filter(x => !_rSelect.includes(x));

                    _lSelect.length > 0 && _lSelect.forEach((itm, ind) => {
                        let ival = itm.substring(itm.indexOf("(") + 1, itm.length - 1);
                        $("#mSelectLeft").append(`<option value="${ival}">${itm}</option>`);
                    });
                    _rSelect.length > 0 && _rSelect.forEach((itm, ind) => {
                        let ival = itm.substring(itm.indexOf("(") + 1, itm.length - 1);
                        $("#mSelectRight").append(`<option value="${ival}">${itm}</option>`);
                    });

                    if (impTempObj.selected[0].dataupd != "" && impTempObj.selected[0].dataupd == "T") {
                        $("#dataupdatecheck").prop('checked', true);
                        $("#dataupdate").removeClass("d-none");
                        //var tstFldsArr = $("#hdnTstructflds").val().slice(0, -1).split(",");
                        $("#tstFlds").select2({
                            allowClear: false,
                            data: [{ id: '', text: '--Select--' }, ...tstFldsArr],
                            // data: tstFldsArr,
                        })
                        var primarykey = impTempObj.selected[0].fldpkey;
                        $('#tstFlds').val(primarykey).trigger('change');

                    }
                    else {
                        $("#dataupdatecheck").prop('checked', false);
                        $("#dataupdate").addClass("d-none");
                    }

                    updateMandatoryFieldsInTemplate();
                } else { // _thisSelval == "NA"
                    impTempObj.selected = [];
                    $("#mSelectLeft, #mSelectRight").empty();
                    $("#dataupdatecheck").prop('checked', false);
                    $("#dataupdate").addClass("d-none");
                    let _allFlds = $("#hdnLeftFlds").val().split(",");
                    _allFlds.forEach((itm, ind) => {
                        $("#mSelectLeft").append(`<option value="${itm}">${itm}</option>`);
                    });
                    updateMandatoryFieldsToSelection();
                }
            } catch (error) {
                impTempObj.selected = [];
                $("#mSelectLeft, #mSelectRight").empty();
                $("#dataupdatecheck").prop('checked', false);
                $("#dataupdate").addClass("d-none");
                showAlertDialog("error", error.message);
            }
        });
    } catch (error) {
        showAlertDialog("error", error.message);
    }

    DropzoneInitImport();
    $("#ChkColNameInfile").attr("checked", false);
    //updating popup over content dynamically based on language selection
    $("#icocl1").attr("data-bs-content", eval(callParent('lcm[179]')));
    $("#icocl2").attr("data-bs-content", eval(callParent('lcm[181]')));
    $("#icocl3").attr("data-bs-content", eval(callParent('lcm[176]')));
    $("#icocl4").attr("data-bs-content", eval(callParent('lcm[177]')));
    $("#icocl5").attr("data-bs-content", eval(callParent('lcm[307]')));
    $("#icocl6").attr("data-bs-content", eval(callParent('lcm[308]')));
    $("#icocl7").attr("data-bs-content", eval(callParent('lcm[309]')));
    parent.gllangType === "ar" ? (placement = "left") : (placement = "right");

    $("#lblimgroupby,#lblseparator, #icocl6, #icocl7").css("float", parent.gllangType === "ar" ? "right" : "left");
    showPopover();

    //updating >, <, >>, << button title content & icons alignment dynamically based on language selection
    $("#right_All_1").prop("title", eval(callParent('lcm[171]'))).addClass(parent.gllangType === "ar" ? "fa-angle-double-left" : "fa-angle-double-right");
    $("#right_Selected_1").prop("title", eval(callParent('lcm[172]'))).addClass(parent.gllangType === "ar" ? "fa-angle-left" : "fa-angle-right");
    $("#left_Selected_1").prop("title", eval(callParent('lcm[173]'))).addClass(parent.gllangType === "ar" ? "fa-angle-right" : "fa-angle-left");
    $("#left_All_1").prop("title", eval(callParent('lcm[174]'))).addClass(parent.gllangType === "ar" ? "fa-angle-double-right" : "fa-angle-double-left");

    $("#btnFileUpload").attr({ 'value': (eval(callParent('lcm[167]'))), 'title': (eval(callParent('lcm[167]'))) });

    callParentNew("closeFrame()", "function");

    uploadFileChangeEvent();
    uploadFileClickEvent();
    // commonReadyTasks();

    $(document).on("keydown", "input[type='text'],input[type='radio'],input[type='checkbox']", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
        }
    });

    // createMultiselectControl();

    //multiselect left button clicks - allow user to move selected options from right to left(only non mandatory fields)
    $("#left_Selected_1").click(function () {
        $("#mSelectRight :selected").map(function (i, el) {
            var li = Boolean($(el).attr("mandatory"));
            if (!li) {
                $('#mSelectLeft').append($('<option>', {
                    value: $(this).val(),
                    text: $(this).text(),
                }));
                $(this).remove();
            }
        });
    });

    //multiselect left all button clicks - allow user to move all options from right to left(only non mandatory fields)
    $("#right_All_1").click(function () {
        $("#mSelectLeft option").map(function (i, el) {
            var li = Boolean($(el).attr("mandatory"));
            if (!li) {
                $('#mSelectRight').append($('<option>', {
                    value: $(this).val(),
                    text: $(this).text(),
                }));
                $(this).remove();
            }
        });
    });

    //multiselect left all button clicks - allow user to move all options from right to left(only non mandatory fields)
    $("#left_All_1").click(function () {
        $("#mSelectRight option").map(function (i, el) {
            var li = Boolean($(el).attr("mandatory"));
            if (!li) {
                $('#mSelectLeft').append($('<option>', {
                    value: $(this).val(),
                    text: $(this).text(),
                }));
                $(this).remove();
            }
        });
    });

    $("#reorder").on("click", (event) => {
        try {
            impFldsReOrder();
        } catch (error) {
            showAlertDialog("error", error.message);
        }
    });

    $('body').on('click', function (e) {
        $('[data-bs-toggle=popover]').each(function () {
            // hide any open popovers when the anywhere else in the body is clicked
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    }).on('hidden.bs.popover', function (e) {
        $(e.target).popover('hide');
    });

    $("#ddlImpTbl").change(function () {
        ShowDimmer(true);
    });

    $("#spnFileSelect").keypress(function (e) {
        if (e.keyCode == 13)
            $("#fileToUpload").click();
    });

    $("#fileToUpload").attr("title", eval(callParent('lcm[66]')));
    $("#ddlSeparator").change(function () {
        $("#hdnUploadFileWarnings, #IsFileUploaded").val("");
        $("#noFile").text(eval(callParent('lcm[66]')));
        $("#fileToUpload").attr("title", eval(callParent('lcm[66]')));
        $("#divProgress").hide();

        $("[data-target='imWizardEdit']").removeClass("in-progress complete");
        $("[data-target='imWizardSummary']").removeClass("in-progress complete");
        $("[data-target='imWizardUpload']").addClass("in-progress").removeClass("complete");
    })

    mandatoryColCount = $("#mSelectRight option[mandatory='true']").length;
    $("#hdnMandatoryColCount").val(mandatoryColCount);

    var mandatoryFldVal = "", mandatoryFldCap = "";
    $("#mSelectRight option[mandatory='true']").each(function () {
        mandatoryFldVal += $(this).val() + ",";
        mandatoryFldCap += $(this).text() + ",";
    });
    if (mandatoryFldVal != "" && mandatoryFldCap != "") {
        mandatoryFldVal = mandatoryFldVal.substr(0, mandatoryFldVal.length - 1);
        mandatoryFldCap = mandatoryFldCap.substr(0, mandatoryFldCap.length - 1);
    }
    $("#hdnMandatoryFields").val(mandatoryFldVal + '#' + mandatoryFldCap);

    //to display tooltips for Wizard tabs
    $("#wizardWrappper .stepName").each(function () {
        $(this).prop("title", $(this).text());
        $(this).next().prop("title", $(this).text());
    });

    try {
        let headerFooter = $(callParentNew("modal-header", "class")).outerHeight(true) + $(".stepper>.card-header").outerHeight(true) + $(".stepper>.card-footer").outerHeight(true);
        $(".stepper>.card-body").css("height", `calc(100vh - ${headerFooter}px)`);

        KTApp?.initBootstrapTooltips();
    } catch (ex) { }
});

function saveTemplateXl() {
    try {
        if (true) {
            /* Import Data Template Save Object */
            impTempObj = {
                ...impTempObj,
                tid: "ad_it", // Import Data Templates' structure transid
                // recordid: "0",
                regex: /^[0-9a-zA-Z\_]+$/,
                modalHTML: `<div id="select2Container" style="margin-top:20px;">
                                    <div class="form-group col-md-12 mb-4">
    <label class="form-label col-form-label pb-1 fw-boldest" for="selectFormTemp">Select Form</label>
    <select id="selectForm" class="form-control">
    </select>
</div>
                <div class="form-group col-md-12 mb-4">
                    <label class="form-label col-form-label pb-1 fw-boldest" for="selectDc">Select DC</label>
                    <select id="selectDc" class="form-control">
                    </select>
                </div>
                <div class="form-group col-md-12 mb-4">
                    <label class="form-label col-form-label pb-1 fw-boldest" for="selectFields">Select Fields</label>
                     <div class="overflow-auto" style="max-height: 90px;">
                    <select id="selectFields" class="form-control"  multiple="multiple">
                    </select>
					</div>
                </div>
                                <div class="form-group col-md-12 mb-4">
                    <label class="form-label col-form-label pb-1 fw-boldest" for="selectPrimaryKey">Select Primary Key</label>
                    <select id="selectPrimaryKey" class="form-control" >
                    </select>
                </div>
            </div>`,
            };

            let myModal = new BSModal("ImportTemplate", "Import Template", impTempObj.modalHTML, (Opening) => {
                // Opening callback

                setTimeout(() => {
                    const dialog = document.querySelector('#ImportTemplate .modal-dialog');

            if (dialog) {
                // Remove vertical centering class
                dialog.classList.remove('modal-dialog-centered');

                // Add custom top margin
                dialog.style.marginTop = '10px';
            }
        }, 100);
                var formArrName = $("#hdnformQueue").val().split(',');
                var formArrVal = $("#hdnformQueueVal").val().split(',');
                var dataxl = formArrName.map((text, index) => {
                    return { id: formArrVal[index], text: text };
                });
                //var formArr = $("#hdnformQueue").val().split(',');
                //function createSelect(label, options, id) {
                //    var formArrVal = $("#hdnformQueueVal").val().split(',');
                //    var select = $("<select>").addClass("select2 sheetselect form-select forValidation selectpicker mb-4").attr("id", id);;
                //    select.append($("<option>").text(label).attr("disabled", true).attr("selected", true));
                //    options.forEach(function(option, index) {
                //        select.append($("<option>").text(option).attr("value", formArrVal[index]));
                //    });
                //    //var dcTooltip ='<i tabindex="0" data-bs-trigger="focus" class="material-icons material-icons-style material-icons-2 align-middle ms-2" data-bs-toggle="popover" id="icocl1" data-bs-content="Select an Existing ImportName from which the data needs to be imported." data-bs-placement="right">help_outline</i>';
                //    if (label === "Choose Group By") {
                //        select.prop("disabled", true);
                //    }
                //    if (label === "Choose DC") {
                //        //  select.append(dcTooltip);
                //    }

                //    return select;
                //}

                //var selectForm = createSelect("Choose Form", formArr,"selectFormTemp");
                //$('#select2Container').prepend(selectForm);
                // $('#ImportTemplate').on('shown.bs.modal', function () {
                //$('#selectFormTemp').select2({
                //    placeholder: "Choose Form",
                //    allowClear: true,
                //    minimumResultsForSearch: 0 // Always show the search bar
                //});
                //});
                //$("#selectForm").select2({
                //   // placeholder: "Choose Form",
                //    //allowClear: true,
                //    minimumResultsForSearch: 0 // Always show the search bar
                //});
                //$('#selectFormTemp').select2();
                $('#selectForm').select2({
                    placeholder: 'Select Form',
                    allowClear: true,
                    data: dataxl,
                    minimumResultsForSearch: 0,
                    dropdownParent: $('#select2Container')
                });
                var selectForm = $('#selectForm');
                //selectForm.empty(); // Clear any existing options

                // Append data to the select dropdown
                //dataxl.forEach(function (item) {
                //    selectForm.append(new Option(item.text, item.id, false, false));
                //});

                // Initialize Select2 after appending the options
                //    selectForm.select2({
                //        placeholder: 'Select Form',
                //        allowClear: true
                //    }).on('select2:open', function () {
                //        // Enable the search field and focus on it
                //        setTimeout(() => {
                //            const searchField = $('.select2-container--open .select2-search__field');
                //        if (searchField.length && searchField.prop('disabled')) {
                //            searchField.prop('disabled', false); // Enable the search field
                //        }
                //        searchField.focus(); // Focus on the search field
                //    }, 100);
                //});

                // Initialize Select2 for the other dropdowns
                // $('#selectForm').select2();
                $('#selectDc').select2({
                    placeholder: 'Select DC',
                    multiple: true,
                    allowClear: true
                });

                $('#selectFields').select2({
                    placeholder: 'Select Fields',
                    allowClear: true
                });
                $('#selectPrimaryKey').select2({
                    placeholder: 'Select Primary Key',
                    allowClear: true
                });

                selectForm.on('select2:select', function (e) {
                    setTimeout(function () {
                        // Close the "Choose Group By" Select2 dropdown after selecting an option
                        // $(this).select2('close');
                        //ShowDimmer(true);
                    }, 0);

                    var selectedValue = $(this).val();
                    var selectedValueselFormid = $(this)[0].id;
                    // var lastCharac = selectedValueselFormid[selectedValueselFormid.length - 1];
                    // var sheetSelect = lastCharac - 1;
                    // var sheetnum = sheetSelect.toString();
                    $(this).select2('close');
                    // Example AJAX call
                    $.ajax({
                        url: 'ImportAll.aspx/GetDcs',
                        type: 'POST',
                        cache: false,
                        async: false,
                        data: JSON.stringify({
                            transid: selectedValue,
                        }),
                        dataType: 'json',
                        contentType: "application/json",
                        success: (data) => {
                            var dccount = parseInt(data.d).toString();
                            // ShowDimmer(false);
                            var dcArray = [];
                            for (var i = 1; i <= data.d; i++) {
                                dcArray.push("DC" + i);
                            }
                            // selectedValueselFormid ="#selectDC" +$(this)[0].id[$(this)[0].id.length - 1];
                            // $(selectedValueselFormid).empty();
                            selectedValueselFormid = "#selectDc";
                            $(selectedValueselFormid).empty();

                            // Add the new options to the Select2 dropdown
                            dcArray.forEach(function (option) {
                                $(selectedValueselFormid).append($("<option>").text(option));
                            });
                            const selectAllOption = new Option("Select All", "selectAll", false, false);
                            $("#selectDc").prepend(selectAllOption); // Add at the beginning
                            $("#selectDc").trigger("change"); // Update Select2 UI


                            $("#selectDc").on("select2:select", function (e) {
                                const selectedDcValue = e.params.data.id;

                                if (selectedDcValue === "selectAll") {
                                    // Select all other options
                                    $("#selectDc > option").each(function () {
                                        if ($(this).val() !== "selectAll") {
                                            $(this).prop("selected", true);
                                        }
                                    });
                                    $("#selectDc").trigger("change"); // Update Select2 UI                                   
                                }
                                // Remove "Select All" pill manually
                                const container = $(".select2-selection__rendered");
                                container.find("li.select2-selection__choice").each(function () {
                                    if ($(this).attr("title") == "Select All") {
                                        $(this).remove();
                                    }
                                });

                                BindSelectDcFields(selectedValue);
                            });

                            // Handle deselecting "Select All"
                            $("#selectDc").on("select2:unselect", function (e) {
                                const unselectedValue = e.params.data.id;

                                if (unselectedValue === "selectAll") {
                                    // Deselect all options
                                    $("#selectDc > option").prop("selected", false);
                                    $("#selectDc").trigger("change"); // Update Select2 UI
                                }
                                const container = $(".select2-selection__rendered");
                                container.find("li.select2-selection__choice").each(function () {
                                    if ($(this).attr("title") == "Select All") {
                                        $(this).remove();
                                    }
                                });
                                BindSelectDcFields(selectedValue);
                            });

                            //// Handle "Select All" functionality
                            //$("#selectDc").on("select2:select", function (e) {
                            //    const selectedValue = e.params.data.id;

                            //    if (selectedValue === "selectAll") {
                            //        // Select all other options
                            //        $("#selectDc > option").each(function () {
                            //            if ($(this).val() !== "selectAll") {
                            //                $(this).prop("selected", true);
                            //            }
                            //        });
                            //        $("#selectDc").trigger("change"); // Update Select2 UI
                            //    }
                            //});

                            //// Handle deselecting "Select All"
                            //$("#selectDc").on("select2:unselect", function (e) {
                            //    const unselectedValue = e.params.data.id;

                            //    if (unselectedValue === "selectAll") {
                            //        // Deselect all options
                            //        $("#selectDc > option").prop("selected", false);
                            //        $("#selectDc").trigger("change"); // Update Select2 UI
                            //    }
                            //});
                            //});
                            // if (data.d == "done") {
                            // success: function(response) {
                            // Handle success response
                            //}
                            $(this).select2('close');
                        },
                        error: function (xhr, status, error) {
                            // Handle error
                            console.error("AJAX error:", error);
                            $(this).select2('close');
                        }
                    });
                    //}, (closing) => {
                    //    // Closing callback
                    //});

                    $.ajax({
                        url: 'ImportAll.aspx/GetFields',
                        type: 'POST',
                        cache: false,
                        async: false,
                        data: JSON.stringify({
                            transid: selectedValue,
                        }),
                        dataType: 'json',
                        contentType: "application/json",
                        success: (data) => {
                            //var datas = JSON.parse(data.d);
                            //var visibleTstFlds = datas.VisibleTstFlds.m_StringValue;
                            //const items = datas.visibleTstFldsGridDc.m_StringValue.slice(0, -1).split(',');

                            //// Step 2: Create an object to group values by DC number
                            //const groupedDC = {};

                            //// Step 3: Iterate through the items and group them by their DC number
                            //items.forEach(item => {
                            //    const [field, dc] = item.split('♠'); // Split the field and the DC number
                            //    if (!groupedDC[dc]) {
                            //        groupedDC[dc] = []; // Initialize an array if the DC number is new
                            //    }
                            //    groupedDC[dc].push(field); // Add the field to the appropriate group
                            //});

                            //// Step 4: Convert the grouped object into an array of arrays dynamically
                            //const result = Object.entries(groupedDC).map(([dc, fields]) => ({
                            //    //dc: dc,
                            //    fields: fields
                            //}));

                            //const itemsnongrid = datas.visibleTstFldsNonGriddc.m_StringValue.slice(0, -1).split(',');

                            //// Step 2: Create an object to group values by DC number
                            //const groupedDCnongrid = {};

                            //// Step 3: Iterate through the items and group them by their DC number
                            //itemsnongrid.forEach(item => {
                            //    const [field, dc] = item.split('♠'); // Split the field and the DC number
                            //    if (!groupedDCnongrid[dc]) {
                            //        groupedDCnongrid[dc] = []; // Initialize an array if the DC number is new
                            //    }
                            //    groupedDCnongrid[dc].push(field); // Add the field to the appropriate group
                            //});

                            //// Step 4: Convert the grouped object into an array of arrays dynamically
                            //const resultnongrid = Object.entries(groupedDCnongrid).map(([dc, fields]) => ({
                            //    //dc: dc,
                            //    fields: fields
                            //}));

                            //const combined = [...resultnongrid, ...result];

                            //// Sort combined data by DC number (numerically)
                            //combined.sort((a, b) => parseInt(a.dc) - parseInt(b.dc));

                            //// Create arrays for each DC number dynamically
                            //arraysByDC = combined.map(entry => entry.fields);

                            //// Example to access arrays by DC numbering
                            //arraysByDC.forEach((fields, index) => {
                            //    console.log(`Array for DC ${combined[index].dc}:`, fields);
                            //});

                            var datas = JSON.parse(data.d);
                            var visibleTstFlds = datas.VisibleTstFlds.m_StringValue;
                            const items = datas.visibleTstFldsGridDc.m_StringValue.slice(0, -1).split(',');
                            const groupedDC = {};

                            items.forEach(item => {
                                const [field, dc] = item.split('♠');
                                if (!groupedDC[dc]) groupedDC[dc] = [];
                                groupedDC[dc].push(field);
                            });

                            const result = Object.entries(groupedDC).map(([dc, fields]) => ({
                                dc: parseInt(dc),
                                fields: fields
                            }));
                            const itemsnongrid = datas.visibleTstFldsNonGriddc.m_StringValue.slice(0, -1).split(',');
                            const groupedDCnongrid = {};

                            itemsnongrid.forEach(item => {
                                const [field, dc] = item.split('♠');
                                if (!groupedDCnongrid[dc]) groupedDCnongrid[dc] = [];
                                groupedDCnongrid[dc].push(field);
                            });

                            const resultnongrid = Object.entries(groupedDCnongrid).map(([dc, fields]) => ({
                                dc: parseInt(dc),
                                fields: fields
                            }));
                            const combined = [...resultnongrid, ...result]
                                .sort((a, b) => a.dc - b.dc);

                            arraysByDC = combined.map(x => x.fields);


                            // var dccount = parseInt(data.d).toString()
                            //ShowDimmer(false);
                            var separator = ',♠';
                            var separatorIndex = visibleTstFlds.indexOf(separator); // Find the index of the separator
                            var newStr = "";
                            var trueValue = "";
                            if (separatorIndex !== -1) {
                                newStr = visibleTstFlds.substring(0, separatorIndex); // Extract substring before the separator
                                trueValue = visibleTstFlds.substring(separatorIndex + separator.length); // Extract "true" after the separator
                            }
                            var PkeyArray = newStr.split(",");
                            //for (var i = 1; i <= data.d; i++) {
                            //    dcArray.push("DC" + i);
                            //}
                            $(this).select2('close');

                            //
                            //        arraysByDC = arraysByDC.map(array => 
                            //    array.filter(item => PkeyArray.some(check => check.trim() === item.trim()))
                            //);
                            // arraysByDC = arraysByDC.map(array => {
                            //    console.log("Processing array:", array); // Debug: Current array
                            //const filteredArray = array.filter(item => {
                            //    const isInCheck = PkeyArray.some(check => check.trim() === item.trim());
                            //console.log(`Checking if '${item}' exists in arraycheck: ${isInCheck}`); // Debug: Comparison result
                            //return isInCheck;
                            //});
                            //console.log("Filtered array:", filteredArray); // Debug: Filtered result
                            //return filteredArray;
                            //});
                            //function createMultiselect() {
                            // Create a select element
                            //var selectElement = $('<select multiple="multiple" id="multiselect"></select>');
                            var selectedValueseMapid = "#selectFields";
                            $(selectedValueseMapid).empty();
                            var Primarykey = $("#selectPrimaryKey");
                            $(Primarykey).empty();
                            // Append options to the select element from the parray
                            PkeyArray.forEach(function (option) {
                                $(selectedValueseMapid).append($('<option></option>').attr('value', option).text(option));
                            });
                            const selectAllOption = new Option("Select All", "selectAll", false, false);
                            $("#selectFields").prepend(selectAllOption); // Add at the beginning
                            $("#selectFields").trigger("change"); // Update Select2 UI

                            // Handle "Select All" functionality
                            $("#selectFields").on("select2:select", function (e) {
                                const selectedValue = e.params.data.id;

                                if (selectedValue === "selectAll") {
                                    // Select all other options
                                    $("#selectFields > option").each(function () {
                                        if ($(this).val() !== "selectAll") {
                                            $(this).prop("selected", true);
                                        }
                                    });
                                    $("#selectFields").trigger("change"); // Update Select2 UI
                                }

                                // Remove "Select All" pill manually
                                const container = $(".select2-selection__rendered");
                                container.find("li.select2-selection__choice").each(function () {
                                    if ($(this).attr("title") == "Select All") {
                                        $(this).remove();
                                    }
                                });
                            });

                            // Handle deselecting "Select All"
                            $("#selectFields").on("select2:unselect", function (e) {
                                const unselectedValue = e.params.data.id;

                                if (unselectedValue === "selectAll") {
                                    // Deselect all options
                                    $("#selectFields > option").prop("selected", false);
                                    $("#selectFields").trigger("change"); // Update Select2 UI
                                }
                            });
                            PkeyArray.forEach(function (option) {
                                $(Primarykey).append($('<option></option>').attr('value', option).text(option));
                            });
                            if (Primarykey.hasClass('select2-hidden-accessible'))
                                Primarykey.select2('destroy');
                            Primarykey.select2({
                                width: '100%',
                                placeholder: 'Select Primary Key',
                                allowClear: true,
                                dropdownParent: $('#ImportTemplate').length ? $('#ImportTemplate') : $(document.body),
                                minimumResultsForSearch: 0
                            });
                            //for (let i = 0; i < arraysByDC.length; i++) {
                            //    if (!arraysByDC[i].includes($(Primarykey).val())) { // Prevent duplicate if `pkey` already exists
                            //        arraysByDC[i].unshift($(Primarykey).val());
                            //    }
                            //}
                        },
                        error: function (xhr, status, error) {
                            // Handle error
                            console.error("AJAX error:", error);
                            $(this).select2('close');
                        }
                    });
                });
                myModal.changeSize("lg");
                myModal.verticallyCentered();
                myModal.hideHeader();
                myModal.showFloatingClose();

                myModal.cancelBtn.classList.add("btn-sm");
                myModal.okBtn.classList.add("btn-sm");

                myModal.okBtn.addEventListener("click", (event) => {
                    var filenameXl = $('#select2-selectForm-container').text() + "(" + $('#selectForm').val() + ")" + ".xlsx";


                    for (let i = 0; i < arraysByDC.length; i++) {
                        arraysByDC[i] = arraysByDC[i].filter(value => $('#selectFields').val().includes(value));
                    }
                    arraysByDC = arraysByDC.filter(arr => arr.length > 0 && arr.some(item => item.trim() !== ''));
                    for (let i = 0; i < arraysByDC.length; i++) {
                        if (!arraysByDC[i].includes($("#selectPrimaryKey").val())) { // Prevent duplicate if `pkey` already exists
                            arraysByDC[i].unshift($("#selectPrimaryKey").val());
                        }
                    }
                    // Handle OK button click
                    $.ajax({
                        type: "POST",
                        url: "ImportAll.aspx/GenerateExcel", // Replace "YourPage" with your actual page name
                        data: JSON.stringify({
                            transid: $('#selectForm').val(),
                            arraysByDC: arraysByDC,
                            //filename:$('#select2-selectForm-container').text(),
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            // var filename = $('#selectForm').val() +"xlsx";
                            const link = document.createElement('a');
                            link.href = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + response.d;
                            link.download = filenameXl;
                            link.click();
                            //alert(response.d); // Handle the response (e.g., success message)
                        },
                        error: function (xhr, status, error) {
                            console.error("Error: " + error);
                        }
                    });
                });
            });
        } else {
            //  showAlertDialog("warning", appGlobalVarsObject.lcm[108]);
            // $('#mSelectLeft').trigger("focus");
            return;
        }
    } catch (error) {
        //  showAlertDialog("warning", appGlobalVarsObject.lcm[108]);
        // $('#mSelectLeft').trigger("focus");
        return;
    }
}

function BindSelectDcFields(selectedValue) {
    var dcArray = $("#selectDc").val() || [];
    var dcString = dcArray.filter(x => x.toLowerCase() !== "selectall").map(x => x.replace(/^DC/i, '').trim()).join(',');

    $.ajax({
        url: 'ImportAll.aspx/GetFieldsNew',
        type: 'POST',
        cache: false,
        async: false,
        data: JSON.stringify({
            transid: selectedValue,
            _selectedDcs: dcString
        }),
        dataType: 'json',
        contentType: "application/json",
        success: (data) => {
            var datas = JSON.parse(data.d);
            var visibleTstFlds = datas.VisibleTstFlds.m_StringValue;
            const items = datas.visibleTstFldsGridDc.m_StringValue.slice(0, -1).split(',');
            const groupedDC = {};

            items.forEach(item => {
                const [field, dc] = item.split('♠');
                if (!groupedDC[dc]) groupedDC[dc] = [];
                groupedDC[dc].push(field);
            });

            const result = Object.entries(groupedDC).map(([dc, fields]) => ({
                dc: parseInt(dc),
                fields: fields
            }));
            const itemsnongrid = datas.visibleTstFldsNonGriddc.m_StringValue.slice(0, -1).split(',');
            const groupedDCnongrid = {};

            itemsnongrid.forEach(item => {
                const [field, dc] = item.split('♠');
                if (!groupedDCnongrid[dc]) groupedDCnongrid[dc] = [];
                groupedDCnongrid[dc].push(field);
            });

            const resultnongrid = Object.entries(groupedDCnongrid).map(([dc, fields]) => ({
                dc: parseInt(dc),
                fields: fields
            }));
            const combined = [...resultnongrid, ...result]
                .sort((a, b) => a.dc - b.dc);
            arraysByDC = combined.map(x => x.fields);
            var separator = ',♠';
            var separatorIndex = visibleTstFlds.indexOf(separator); 
            var newStr = "";
            var trueValue = "";
            if (separatorIndex !== -1) {
                newStr = visibleTstFlds.substring(0, separatorIndex); // Extract substring before the separator
                trueValue = visibleTstFlds.substring(separatorIndex + separator.length); // Extract "true" after the separator
            }
            var PkeyArray = newStr.split(",");

            var selectedValueseMapid = "#selectFields";
            $(selectedValueseMapid).empty();
            var Primarykey = $("#selectPrimaryKey");
            Primarykey.empty();
            PkeyArray.forEach(function (option) {
                $(selectedValueseMapid).append($('<option></option>').attr('value', option).text(option));
            });
            const selectAllOption = new Option("Select All", "selectAll", false, false);
            $("#selectFields").prepend(selectAllOption); // Add at the beginning
            $("#selectFields").trigger("change"); // Update Select2 UI

            // Handle "Select All" functionality
            $("#selectFields").on("select2:select", function (e) {
                const selectedValue = e.params.data.id;

                if (selectedValue === "selectAll") {
                    // Select all other options
                    $("#selectFields > option").each(function () {
                        if ($(this).val() !== "selectAll") {
                            $(this).prop("selected", true);
                        }
                    });
                    $("#selectFields").trigger("change"); // Update Select2 UI
                }

                // Remove "Select All" pill manually
                const container = $(".select2-selection__rendered");
                container.find("li.select2-selection__choice").each(function () {
                    if ($(this).attr("title") == "Select All") {
                        $(this).remove();
                    }
                });
            });

            // Handle deselecting "Select All"
            $("#selectFields").on("select2:unselect", function (e) {
                const unselectedValue = e.params.data.id;

                if (unselectedValue === "selectAll") {
                    // Deselect all options
                    $("#selectFields > option").prop("selected", false);
                    $("#selectFields").trigger("change"); // Update Select2 UI
                }
            });
            PkeyArray.forEach(function (option) {
                $(Primarykey).append($('<option></option>').attr('value', option).text(option));
            });
            if (Primarykey.hasClass('select2-hidden-accessible'))
                Primarykey.select2('destroy');
            Primarykey.select2({
                width: '100%',
                placeholder: 'Select Primary Key',
                allowClear: true,
                dropdownParent: $('#ImportTemplate').length ? $('#ImportTemplate') : $(document.body),
                minimumResultsForSearch: 0
            });
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error("AJAX error:", error);
            $(this).select2('close');
        }
    });
}

function saveTemplate() {
    try {
        if (validateDataSearchWiz() == true) {
            /* Import Data Template Save Object */
            impTempObj = {
                ...impTempObj,
                tid: "ad_it", // Import Data Templates' structure transid
                // recordid: "0",
                regex: /^[0-9a-zA-Z\_]+$/,
                modalHTML: `<div class="mb-3">
                    <label for="impTemplateName" class="form-label col-form-label required">Template Name</label>
                    <input type="text" class="form-control" id="impTemplateName" placeholder="Template Name">
                </div>
                <div class="mb-3">
                    <label for="impTemplateCaption" class="form-label col-form-label required">Template Caption</label>
                    <input type="text" class="form-control" id="impTemplateCaption" placeholder="Template Caption">
                </div>`,
            };
            let myModal = new BSModal("ImportTemplate", "Import Template", impTempObj.modalHTML, (Opening) => {
                //Opening callback        
                if ($("#ddlImpTemplate").val() != "" && $("#ddlImpTemplate").val() != "NA") {
                    let tempCaption = $("#ddlImpTemplate").select2("data")[0].text;
                    let tempVal = $("#ddlImpTemplate").select2("data")[0].id;
                    $("#impTemplateCaption").val(tempCaption).trigger("focus");
                    $("#impTemplateName").val(tempVal).prop("disabled", true);
                } else {
                    $("#impTemplateName").trigger("focus");
                }
            }, (closing) => {
                //closing callback
            });
            myModal.changeSize("sm");
            myModal.verticallyCentered();
            myModal.hideHeader();
            myModal.showFloatingClose();

            myModal.cancelBtn.classList.add("btn-sm");
            myModal.okBtn.classList.add("btn-sm");

            myModal.okBtn.addEventListener("click", (event) => {
                try {
                    impTempObj.flds = {
                        cols: ["stransid", "templatename", "templatecap", "impfields", "recordid", "dataupd", "fldpkey"],
                        vals: []
                    };

                    impTempObj.flds.vals.push($("#hdnTransid").val() || "");
                    impTempObj.flds.vals.push($("#impTemplateName").val() || "");
                    impTempObj.flds.vals.push($("#impTemplateCaption").val() || "");

                    let rightSelectedFlds = $("#hdnColNames").val() || "";
                    if (rightSelectedFlds != "") {
                        rightSelectedFlds = rightSelectedFlds.replace(/^\s+|\s+$/gm, '').replaceAll("\n", "").replace(/,\s+/g, ',');
                    }
                    impTempObj.flds.vals.push(rightSelectedFlds || "");

                    impTempObj.flds.vals[1] == "" ? showAlertDialog("warning", "Template Name cannot be empty!") : "";
                    impTempObj.flds.vals[2] == "" ? showAlertDialog("warning", "Template Caption cannot be empty!") : "";

                    if ($("#dataupdatecheck").is(":checked")) {
                        impTempObj.flds.vals[5] = "T";
                        impTempObj.flds.vals[6] = $("#tstFlds").val();
                    }
                    else {
                        impTempObj.flds.vals[5] = "F";
                        impTempObj.flds.vals[6] = "";
                    }

                    if (impTempObj.flds.vals[1] != "" && impTempObj.flds.vals[2] != "") {
                        if (!impTempObj.regex.test(impTempObj.flds.vals[1])) {
                            showAlertDialog("error", "Invalid Template Name!");
                        } else {
                            $.each(impTempObj.flds.cols, (ind, col) => {
                                if (col != "recordid")
                                    AxSetValue(impTempObj.tid, col, 1, 0, impTempObj.flds.vals[ind]);
                            });

                            let recordid = (typeof impTempObj.selected != "undefined" ? impTempObj?.selected[0]?.axpdef_impdata_templatesid : "0") || "0";
                            let saveMsg = AxSubmitData(impTempObj.tid, recordid);
                            impTempObj.flds.vals.push(AssignLoadValues(saveMsg));

                            if (impTempObj.flds.vals[4] != "") {
                                let ddOpts = $("#ddlImpTemplate option:not(:first)");
                                if (ddOpts.length != 0) {
                                    $.each(ddOpts, (ind, item) => {
                                        if (item.value == impTempObj.flds.vals[1])
                                            $(`#ddlImpTemplate option[value='${item.value}']`).remove();
                                    });

                                    impTempObj.sqlRes = impTempObj.sqlRes.filter((obj) => obj.templatename !== impTempObj.flds.vals[1]);
                                }

                                impTempObj.selected = [{ axpdef_impdata_templatesid: impTempObj.flds.vals[4], templatename: impTempObj.flds.vals[1], templatecap: impTempObj.flds.vals[2], impfields: impTempObj.flds.vals[3] }];

                                impTempObj.sqlRes.push({ axpdef_impdata_templatesid: impTempObj.flds.vals[4], templatename: impTempObj.flds.vals[1], templatecap: impTempObj.flds.vals[2], impfields: impTempObj.flds.vals[3] });

                                $("#ddlImpTemplate").append(new Option(impTempObj.flds.vals[2], impTempObj.flds.vals[1], true, true)).trigger("change");
                                $("#btnContinue").trigger("click");

                            }
                        }
                    }
                } catch (error) {
                    showAlertDialog("error", error.message);
                }
            });
        } else {
            showAlertDialog("warning", appGlobalVarsObject.lcm[108]);
            $('#mSelectLeft').trigger("focus");
            return;
        }
    } catch (error) {
        showAlertDialog("warning", appGlobalVarsObject.lcm[108]);
        $('#mSelectLeft').trigger("focus");
        return;
    }
}

function importHistory() {
    ShowDimmer(true);
    let impHistoryObj = {
        modalHTML: `<iframe id="impHistoryFrame" src="../aspx/ivtoivload.aspx?ivname=ad_implg&ptransid=${$("#hdnTransid").val()}" scrolling="no" class="w-100 h-100 h-md-400px"></iframe>`,
    }
    let myModal = new BSModal("ImportHistory", "Import History", impHistoryObj.modalHTML, (Opening) => {
        //Opening callback        
        try {
            var counter = 0;
            var intervalID = setInterval(() => {
                counter++;
                if (counter == 10) {
                    clearInterval(intervalID);
                    ShowDimmer(false);
                }

                let impHistFrame = document.getElementById("impHistoryFrame")?.contentWindow?.document?.getElementsByClassName("iframeScrollFix")[0]?.getElementsByTagName("body");
                if (typeof impHistFrame != "undefined" && impHistFrame.length > 0) {
                    clearInterval(intervalID);
                    ShowDimmer(false);
                }
            }, 1000);
        } catch (error) {
            clearInterval(intervalID);
            ShowDimmer(false);
        }

    }, (closing) => {
        //closing callback
    });

    myModal.changeSize("xl");
    myModal.hideHeader();
    myModal.hideFooter();
    myModal.showFloatingClose();

    myModal.okBtn.classList.add("btn-sm");
    myModal.cancelBtn.classList.add("btn-sm");
}

function AssignLoadValues(result, calledFrom = "") {
    var returnVal = "";
    var resval = result.split("*$*");
    for (var ind = 0; ind < resval.length; ind++) {
        var strSingleLineText = resval[ind].toString().replace(new RegExp("\\n", "g"), "");
        try {
            var myJSONObject = $j.parseJSON(strSingleLineText);
        } catch (ex) {
            continue;
        }

        if (myJSONObject.error) {
            ExecErrorMsg(myJSONObject.error, calledFrom);
        } else if (myJSONObject.message) {
            returnVal = ExecMessage(myJSONObject.message, calledFrom);
        }
    }
    return returnVal;
}

function ExecMessage(messageJsonObj, calledFrom = "") {
    var alertType = "";
    var recID = "0";
    for (var i = 0; i < messageJsonObj.length; i++) {
        var msgs = messageJsonObj[i].msg;
        if (msgs.indexOf("recordid") > -1) {
            var msgsArray = msgs.split(',');
            msgs = "";
            for (var x = 0; x < msgsArray.length; x++) {
                if (msgsArray[x].indexOf("recordid=") == -1) {
                    if (x == msgsArray.length - 1)
                        msgs += msgsArray[x];
                    else
                        msgs += msgsArray[x] + ",";
                } else if (msgsArray[x].indexOf("recordid=") != -1) {
                    recID = msgsArray[x].split("=")[1];
                }
            }
        }
        if (msgs.indexOf("errfld") > -1)
            msgs = msgs.substring(0, msgs.lastIndexOf("errfld") - 2);
        msgs = msgs.split(",");

        var responsemsgFlds = msgs.length;
        var msg = "";
        for (var mm = 0; mm < responsemsgFlds; mm++) {
            msg += msgs[mm] + ",";
            var stPos = msg.indexOf("[");
            var endPos = msg.indexOf("]");
            var errFld = msg.substring(stPos + 1, endPos);
            if (errFld != "") {
                var nerr = msg.substring(stPos, endPos + 2);
                msg = msg.replace(nerr, "");
                alertType = "error";
            }
            var index = msg.indexOf("^^dq");
            while (index != -1) {
                msg = msg.replace("^^dq", '"');
                index = msg.indexOf("^^dq");
            }
        }

        if (msg != "") {
            if (calledFrom == "Action" && msg.indexOf("recordid") > 0)
                msg = msg.substr(0, msg.indexOf("recordid"));

            if ((calledFrom == "Action" || calledFrom == "Iview") && msg.indexOf("`") != -1) {
                var alType = msg.split("`")[0].toLowerCase();
                if (alType.indexOf(",") > -1) {
                    alType = alType.split(",")[1];
                }

                if (alType == "simple" || alType == "")
                    alertType = "info";
                else if (alType == "warning")
                    alertType = "warning";
                else if (alType == "confirmation")
                    alertType = "success";
                else if (alType == "exceptions")
                    alertType = "error";
                msg = msg.split("`")[1].slice(0, -1);
            } else if (msg.toLowerCase().indexOf("invalid") != -1 || msg.indexOf("cannot be left empty") != -1) {
                alertType = "error";
            } else if (msg.toLowerCase().indexOf("is completed") != -1 || msg.toLowerCase().indexOf("saved successfully") != -1) {
                alertType = "success";
            } else {
                alertType = "info";
            }

            if (msg.indexOf("cannot be left empty") != -1) {
                msg = msg.replace('cannot be left empty', appGlobalVarsObject.lcm[45]);
            }
            showAlertDialog(alertType, msg);
        }
    }

    return recID;
}

function ExecErrorMsg(ErroMsgJsonObj, calledFrom = "") {
    for (var i = 0; i < ErroMsgJsonObj.length; i++) {
        var errMsg = ErroMsgJsonObj[i].msg;
        var errFld;

        if (ErroMsgJsonObj[i].errfld)
            errFld = ErroMsgJsonObj[i].errfld;

        var index = errMsg.indexOf("^^dq");
        while (index != -1) {
            errMsg = errMsg.replace("^^dq", '"');
            index = errMsg.indexOf("^^dq");
        }
        if (errMsg != null && errMsg != undefined && errMsg != "") {
            if (errMsg.indexOf("errfld") > -1) {
                errMsg = errMsg.substring(0, errMsg.lastIndexOf("errfld") - 2);
                errMsg = errMsg.replace("\",", "").replace("\" ,", "");
            }
            showAlertDialog("error", errMsg);
        }
    }
}

function deleteTemplate() {
    try {
        let toDelTemp = $("#ddlImpTemplate").val();
        if (toDelTemp != null && toDelTemp != "" && toDelTemp != "NA") {
            var isRTL = (callParentNew('gllangType') == "ar") ? true : false;
            var deleteTemplateCB = $.confirm({
                theme: 'modern',
                title: appGlobalVarsObject.lcm[155],
                onContentReady: () => {
                    disableBackDrop('bind');
                },
                backgroundDismiss: 'false',
                escapeKey: 'buttonB',
                rtl: isRTL,
                content: `Do you want to delete ${$("#ddlImpTemplate").select2("data")[0].text} Template?`,
                buttons: {
                    buttonA: {
                        text: appGlobalVarsObject.lcm[164],
                        btnClass: 'btn btn-primary',
                        action: () => {
                            delConfirmCB(toDelTemp);
                            deleteTemplateCB.close();
                            disableBackDrop('destroy');
                        }
                    },
                    buttonB: {
                        text: appGlobalVarsObject.lcm[192],
                        btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                        action: () => {
                            deleteTemplateCB.close();
                            disableBackDrop('destroy');
                        }
                    }
                }
            });
        } else {
            showAlertDialog("warning", "Please select a template to delete");
        }
    } catch (error) {
        showAlertDialog("error", error.message);
    }
}

//    function dcCount(formname) {
//        try {
//            $.ajax({
//                url: 'Importnew.aspx/deleteTemplate',
//                type: 'POST',
//                cache: false,
//                async: false,
//                data: JSON.stringify({
//                    transid: $("#hdnTransid").val(),
//                    tempname: toDelTemp
//                }),
//                dataType: 'json',
//                contentType: "application/json",
//                success: (data) => {
//                    if (data.d == "done") {
//                        impTempObj.sqlRes = impTempObj.sqlRes.filter((obj) => obj.templatename !== toDelTemp);
//            $(`#ddlImpTemplate option[value='${toDelTemp}']`).remove();
//            $("#ddlImpTemplate").val("NA").trigger("select2:select");
//            showAlertDialog("success", "Template deleted successfully");
//        } else {
//                        showAlertDialog("error", "Error occurred while deleting the template");
//    }
//    },
//    error: (error) => {
//        showAlertDialog("error", "Error occurred while deleting the template");
//    }
//    });
//    } catch (error) {
//        showAlertDialog("error", error.message);
//    }
//}

function delConfirmCB(toDelTemp) {
    try {
        $.ajax({
            url: 'Importnew.aspx/deleteTemplate',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                transid: $("#hdnTransid").val(),
                tempname: toDelTemp
            }),
            dataType: 'json',
            contentType: "application/json",
            success: (data) => {
                if (data.d == "done") {
                    impTempObj.sqlRes = impTempObj.sqlRes.filter((obj) => obj.templatename !== toDelTemp);
                    $(`#ddlImpTemplate option[value='${toDelTemp}']`).remove();
                    $("#ddlImpTemplate").val("NA").trigger("select2:select");
                    showAlertDialog("success", "Template deleted successfully");
                } else {
                    showAlertDialog("error", "Error occurred while deleting the template");
                }
            },
            error: (error) => {
                showAlertDialog("error", "Error occurred while deleting the template");
            }
        });
    } catch (error) {
        showAlertDialog("error", error.message);
    }
}

function impFldsReOrder() {
    try {
        let reOrdObj = {
            preOrdOpts: $("#mSelectRight option"),
        };

        if (reOrdObj.preOrdOpts.length > 1) {
            reOrdObj.isOrdered = false;
            reOrdObj.li = ``;
            $.each(reOrdObj.preOrdOpts, (ind, opt) => {
                reOrdObj.li += `<li class="d-flex list-group-item ui-state-default ui-sortable-handle" data-optval="${opt.attributes.value.value}" data-optmand="${opt.attributes.mandatory.value}" data-opttext="${opt.text}">
                    <span class="material-icons dragIcon cursor-drag my-auto">drag_indicator</span>                
                    <label class="form-label fw-boldest my-2">
                        <span class="dragName">${opt.text}</span>
                    </label>
                </li> `
            });
            reOrdObj.Html = `<div class="impFldsReOrdWrapper h-450px min-h-sm-100px">
                <ul class="card list-group ui-sortable">
                    ${reOrdObj.li}
                </ul>
            </div>`;

            let myModal = new BSModal("ImportReOrder", "Import Re-Order", reOrdObj.Html, (Opening) => {
                //Opening callback                  
                $(".impFldsReOrdWrapper > .list-group").sortable({
                    cursor: "move",
                    update: function (event, ui) {
                        reOrdObj.isOrdered = true;
                    },
                });
            }, (closing) => {
                //closing callback
            });

            myModal.hideHeader();
            myModal.scrollableDialog();

            myModal.modalFooter.classList.add("py-2");
            myModal.cancelBtn.classList.add("btn-sm");
            myModal.okBtn.classList.add("btn-sm");
            myModal.okBtn.innerText = "Re-Order";
            myModal.okBtn.removeAttribute("data-bs-dismiss");

            myModal.okBtn.addEventListener("click", (event) => {
                try {
                    if (reOrdObj.isOrdered) {
                        reOrdObj.postOrdOpts = $(".impFldsReOrdWrapper > .list-group > .list-group-item").map((index, elem) => {
                            return { ordno: index + 1, liOpt: { val: $(elem).data("optval"), mandatory: $(elem).data("optmand"), text: $(elem).data("opttext") } }
                        }).toArray();

                        $("#mSelectRight option").remove();

                        $.each(reOrdObj.postOrdOpts, (ind, item) => {
                            $("#mSelectRight").append($('<option>', {
                                value: item.liOpt.val,
                                text: item.liOpt.text,
                                mandatory: item.liOpt.mandatory
                            }));
                        });

                        showAlertDialog("success", "Reorder successful");
                        callParentNew("ImportReOrder", "id").dispatchEvent(new CustomEvent("close"));
                    } else {
                        showAlertDialog("warning", "Please reorder the fields");
                    }
                } catch (error) {
                    showAlertDialog("error", error.message);
                }
            });

        } else if (reOrdObj.preOrdOpts.length == 1) {
            showAlertDialog("warning", "Not enough options to reorder.");
        } else {
            showAlertDialog("warning", "No options to reorder.");
        }
    } catch (error) {
        showAlertDialog("error", error.message);
    }
}

function GetTemplates() {
    try {
        impTempObj.sqlRes = [];

        $.ajax({
            url: 'Importnew.aspx/GetTemplates',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({
                transid: $("#hdnTransid").val(),
            }),
            dataType: 'json',
            contentType: "application/json",
            success: (data) => {
                if (data && data.d != "") {
                    data = JSON.parse(data.d);
                    if (data.result && data.result.row) {
                        // impTempObj.sqlRes = data.result.row;                        
                        data.result.row.forEach(function (val) {
                            var ret = {};
                            $.map(val, function (value, key) {
                                ret[key.toLowerCase()] = value;
                            });
                            impTempObj.sqlRes.push(ret);
                        });
                        if (data.result.row.length > 0) {
                            impTempObj.ddList = Array.from(new Set([...[{ id: "NA", text: appGlobalVarsObject.lcm[441] }],
                            ...$.map(impTempObj.sqlRes, (val, index) => {
                                return {
                                    id: val.templatename,
                                    text: val.templatecap
                                }
                            })
                            ]));
                        } else {
                            impTempObj.ddList = [{ id: "NA", text: appGlobalVarsObject.lcm[441] }];
                        }
                    }
                } else {
                    impTempObj.ddList = [{ id: "NA", text: appGlobalVarsObject.lcm[441] }];
                    showAlertDialog("error", 3028, "client");
                }
            },
            error: (error) => {
                impTempObj.ddList = [{ id: "NA", text: appGlobalVarsObject.lcm[441] }];
                showAlertDialog("error", 3028, "client");;
            }
        });
    } catch (error) {
        impTempObj.ddList = [{ id: "NA", text: appGlobalVarsObject.lcm[441] }];
        showAlertDialog("error", error.message);
    }
}

/* begins:: ImportData with AxRule Engine */
var AxRulesDefScriptOnLoad = "false";
var AxRuesDefScriptFormcontrol = "false";
var AxRuesDefScriptFCP = "false";
var AxAllowEmptyFlds = new Array;
var transid = "";
var Parameters = new Array();
var AxMemParameters = new Array();
var AxRDFormControl = new Array();
var AxRDScriptOnLoad = new Array();
var FNames = new Array();
var DCFrameNo = new Array();
var TstructHasPop = false;
var AxActiveDc = "";
var AxFormControlList = new Array();
var AxOldValueOnChange = "";
function CheckAxRulesForImport() {
    let AxRulesEngine = $("#axRuleDetails").val();
    if (typeof AxRulesEngine != "undefined" && AxRulesEngine != "") {
        let axruleList = AxRulesEngine.split('~');
        AxRulesDefScriptOnLoad = axruleList[0];
        AxRuesDefScriptFormcontrol = axruleList[2];

        let AxRDFormControls = $("#axRuleScript").val();
        if (AxRDFormControls != "")
            AxRDFormControl = AxRDFormControls.split('♠');
        let AxRDScriptOnLoads = $("#axRuleOnSubmit").val();
        if (AxRDScriptOnLoads != "")
            AxRDScriptOnLoad = AxRDScriptOnLoads.split('♠');

        if (AxRulesDefScriptOnLoad == "true")
            AxRulesScriptsParser("scriptonload");

        if (AxRuesDefScriptFormcontrol == "true")
            AxRulesScriptsParser("formcontrol");

        if (AxAllowEmptyFlds.length > 0) {
            AxRuleSetMandatory();
        }
    }
}

function AxRulesScriptsParser(thisEvent) {
    switch (thisEvent) {
        case 'formcontrol':
            var flname = "";
            $.each(AxRDFormControl, function (ind, thisScript) {
                if (thisScript != "") {
                    var arrsfcExp = thisScript.split("♥");
                    EvalExprSet(arrsfcExp);
                }
            });
            break;
        case 'scriptonload':
            var flname = "";
            $.each(AxRDScriptOnLoad, function (ind, thisScript) {
                if (thisScript) {
                    var arrsfcExp = thisScript.split("♥");
                    EvalExprSet(arrsfcExp);
                }
            });
            break;
        default:
            return true;
    }
}

function AxRuleSetMandatory() {
    try {
        $.ajax({
            url: 'importnew.aspx/AxRuleSetMandatory',
            type: 'POST',
            cache: false,
            async: false,
            data: JSON.stringify({ AxAllowEmpty: AxAllowEmptyFlds }),
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {
                if (data.d == "done") {
                    $("#mSelectLeft").empty();
                    $("#mSelectRight").empty();
                    let _fldModName = "";
                    let allFlds = $("#hdnLeftFlds").val();
                    if (allFlds != "") {
                        allFlds.split(',').forEach(function (ele) {
                            let _fldName = ele.split('(')[1].split(')')[0];
                            let _fldCaption = ele.split('(')[0];
                            _fldCaption = _fldCaption.replace('*', "");
                            let _MatchFld = AxAllowEmptyFlds.filter((x) => x.startsWith(_fldName + "~"));
                            let _eleNameCap = ele;
                            if (_MatchFld.length > 0 && _MatchFld[0].split('~')[1] == "F")
                                _eleNameCap = _fldCaption + "*(" + _fldName + ")";
                            else if (_MatchFld.length > 0 && _MatchFld[0].split('~')[1] == "T")
                                _eleNameCap = _fldCaption + "(" + _fldName + ")";
                            $("#mSelectLeft").append('<option value="' + _fldName + '">' + _eleNameCap + '</option>');
                            if (_fldModName == "")
                                _fldModName = _eleNameCap;
                            else
                                _fldModName += "," + _eleNameCap;
                        });
                        setTimeout(function () {
                            updateMandatoryFieldsToSelection();
                        }, 0);
                    }
                    if (_fldModName != "")
                        $("#hdnLeftFlds").val(_fldModName);
                }
            },
            error: function (error) {
            }
        });

    } catch (ex) { }
}

/* ends:: ImportData with AxRule Engine */

function ShowImportError(errorMsg) {
    ShowDimmer(false);
    showAlertDialog("error", errorMsg);
}