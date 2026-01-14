var placement;
var ignoredColCount = 0;
var ignoredColumns = [];
var oldSelectedValues = 0;
var isFormDirty = false;
var leftSelectedClickedOnce = false;
var impTempObj = {};

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
                if (!$("#ChkColNameInfile").is(":checked")) {
                    showAlertDialog("warning", eval(callParent('lcm[108]')));
                    $('#mSelectLeft').focus();
                } else {
                    multiselect = 1;
                }
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

        //$("#excel1").click(function () {
        //    //$("#excel1").attr("href", "openfile.aspx?fpath=" + hdnTemplateName + ".xlsx&Imp=t")
        //    //$("#hdnTemplateName").val() = hdnTemplateName + ".xls";
        //    if (hdnTemplateName == "") {
        //        hdnTemplateName = $("#ddlImpTbl :selected").text();
        //    }
        //    $("#hdnTemplateName").val(hdnTemplateName + ".xlsx");
        //    $("#btnCreateTemplate").click();
        //});

        //$("#CSV1").click(function () {
        //    // $("#CSV1").attr("href", "openfile.aspx?fpath=" + hdnTemplateName + ".csv&Imp=t")
        //    if (hdnTemplateName == "") {
        //        hdnTemplateName = $("#ddlImpTbl :selected").text();
        //    }
        //    $("#hdnTemplateName").val(hdnTemplateName + ".csv");
        //    $("#btnCreateTemplate").click();
        //});

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
                if($("#dataupdatecheck").is(":checked")) {
                    if($("#tstFlds").val() != $options.text())
                    {
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
            if($("#dataupdatecheck").is(":checked")) 
            {
                var selectOrAll = $(event.currentTarget).attr("id") == "left_All_1" ? "option" : ":selected";
                $("#mSelectRight " + selectOrAll).map(function (i, el) {
                    if($("#tstFlds").val() != $(this).text())
                    {
                        $('#mSelectLeft').append($('<option>', {
                            value: $(this).val(),
                            text: $(this).text(),
                            mandatory: false
                        }));
                        $(this).remove();
                    }
                });
            }
            else
            {
                if (mandatoryCount > 1 && mandatoryCount == $options.length ) {//if all right select fields are mandatory fields
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
    if (selectExist == undefined) {
        $("#gridImpData th").each(function () {
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
            $(this).find("select").change(function () {
                $("option[value=" + this.value + "]", this).attr("selected", true).siblings().removeAttr("selected");
                selectVal = $(this).val();
                selectId = $(this).attr("id");
                mandatory = $("#mSelectRight option[value='" + selectVal + "']").attr("mandatory");
                mandatory = mandatory != undefined ? JSON.parse(mandatory) : false;
                colIndex = $(this).closest("th").index();
                if (mandatory)
                    $("#chk" + selectId).attr("disabled", "disabled");
                else
                    $("#chk" + selectId).removeAttr("disabled");
            });
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

function showPopover() {
    $('[data-bs-toggle="popover"]').popover({
        placement: placement
    });
}

function DropzoneInitImport() {
    const id = "#dropzone_AxpFileImport";
    const dropzone = document.querySelector(id);

    //get the preview element template
    var previewNode = dropzone.querySelector(".dropzone-item");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);
    var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));
    var myDropzone = new Dropzone(id, { // Make the whole body a dropzone
        url: url + "FileUploadHandler.ashx", // Set the url for your upload script location
        // parallelUploads: 20,
        previewTemplate: previewTemplate,
        addRemoveLinks: true,
        maxFilesize: 1, // Max filesize in MB
        // autoQueue: false, // Make sure the files aren't queued until manually added
        previewsContainer: id + " .dropzone-items", // Define the container to display the previews
        clickable: id + " .dropzone-select" // Define the element that should be used as click trigger to select files.
    });
    //.addRemoveLinks = true
    myDropzone.on("addedfile", function (file) {
        // Hookup the start button
        const dropzoneItems = dropzone.querySelectorAll('.dropzone-item');
        dropzoneItems.forEach(dropzoneItem => {
            dropzoneItem.style.display = '';
        });
    });

    // Update the total progress bar
    myDropzone.on("totaluploadprogress", function (progress) {
        const progressBars = dropzone.querySelectorAll('.progress-bar');
        progressBars.forEach(progressBar => {
            progressBar.style.width = progress + "%";
        });
    });
    myDropzone.on("removedfile", function (file) {
        // debugger;
    });

    // Hide the total progress bar when nothing's uploading anymore
    myDropzone.on("complete", function (progress) {
        const progressBars = dropzone.querySelectorAll('.dz-complete');
        var responseText = progress.xhr.responseText;
        var fileNameModified = progress.xhr.responseText.substr(progress.xhr.responseText.indexOf("&&") + 2);

        $("#upFileName").val(fileNameModified);
        $("#uploadFileName").val(progress.name);
        $("#hdnOriginalfileName").val($("#uploadFileName").val());
        //FileuplaodValidation();
        $("#UploadButton").click();
        // $("#upFileName").val(progress.name); 
        setTimeout(function () {
            progressBars.forEach(progressBar => {
                progressBar.querySelector('.progress-bar').style.opacity = "0";
                progressBar.querySelector('.progress').style.opacity = "0";
                // progressBar.querySelector('.dropzone-start').style.opacity = "0";
            });
        }, 300);
    });
}

function toggleReadonly(_isDisabled) {
    // Get the select elements
    const selectLeft = document.getElementById('mSelectLeft');
    const selectRight = document.getElementById('mSelectRight');
    // Toggle the disabled attribute
    selectLeft.disabled = !_isDisabled;
    selectRight.disabled = !_isDisabled;

    // Disable/Enable buttons
    const buttons = document.querySelectorAll('a.btn');
    buttons.forEach(button => {
        if (_isDisabled) {
            // Enable the buttons by removing the class and adding pointer events
            button.classList.remove('disabled-link');
            button.style.pointerEvents = 'auto';
        } else {
            // Disable the buttons by adding the disabled class and disabling pointer events
            button.classList.add('disabled-link');
            button.style.pointerEvents = 'none';
        }
    });

    if (_isDisabled) {
        document.getElementById('lnkExpTemp').disabled = false;
        document.getElementById('excel1').style.pointerEvents = 'auto';
        document.getElementById('CSV1').style.pointerEvents = 'auto';
        document.getElementById('datetip').setAttribute('data-bs-toggle', 'tooltip');
        document.getElementById('icocl3').setAttribute('data-bs-toggle', 'popover');
        document.querySelector('.upload-section').style.pointerEvents = 'auto';
    } else {
        document.getElementById('lnkExpTemp').disabled = true;
        document.getElementById('excel1').style.pointerEvents = 'none';
        document.getElementById('CSV1').style.pointerEvents = 'none';
        document.getElementById('datetip').removeAttribute('data-bs-toggle');
        document.getElementById('icocl3').removeAttribute('data-bs-toggle');
        document.querySelector('.upload-section').style.pointerEvents = 'none';
    }
}

$(document).ready(function () {
    // $("#tstFlds").append("<option value='NA'>-- Select --</option");
    var tstFldsArr = $("#hdnTstructflds").val().slice(0, -1).split(",");
    $("#tstFlds").select2({
        allowClear: false,
        data: [{ id: '', text: '--Select--' }, ...tstFldsArr],
        // data: tstFldsArr,
    }).on('select2:select', function (selectEv) {
        try {
            let _thisEle = $(selectEv.currentTarget).data("select2")?.$element[0]?.id;
            let _thisSelval = $(`#${_thisEle}`).val();
        var tstfld = { fields: [] };
        var tstfldRight ={field: []};
        
        if(_thisSelval != "" && _thisSelval.indexOf("--Select--") == -1)
        {
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
    else{
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
    fields: tstfld.fields.filter(function(field) {
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

        catch (error) {}
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

    $("#ChkColNameInfile").change(function () {
        if ($(this).is(":checked")) {
            $("#mSelectLeft option").map(function (i, el) {
                $('#mSelectRight').append($('<option>', {
                    value: $(this).val(),
                    text: $(this).text(),
                }));
                $(this).remove();
            });
            $("#mSelectRight option").css({ 'color': '#eff2f5;' });
            toggleReadonly(false);
        } else {
            $("#mSelectRight option").map(function (i, el) {
                $('#mSelectLeft').append($('<option>', {
                    value: $(this).val(),
                    text: $(this).text(),
                }));
                $(this).remove();
            });
            toggleReadonly(true);
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
        validateDataSearchWiz();
        if (validateDataSearchWiz() == true) {
            stepper.goNext();
            $("#hdnupdateField").val( $("#tstFlds").val());
            $("#btnSaveTemplate").addClass("d-none");
            return;
        } else {
            return;
        }
    }
    if (stepper.getCurrentStepIndex() == 2) {
        validateDataUploadWiz();
        if (validateDataUploadWiz() == true) {
            $("#ColHeaderClick").click();
            stepper.goNext();
            $("#btnSaveTemplate").addClass("d-none");
            return;
        } else {
            return;
        }
    }
    if (stepper.getCurrentStepIndex() == 3) {
        if (GetSelColData()) {
            $(".card-footer").addClass("d-none");
            $("#btnImport").click();
            ShowDimmer(true);
            stepper.goNext();
            $("#btnSaveTemplate").addClass("d-none");
            return;
        }
        return;
    }
    if (stepper.getCurrentStepIndex() == 4) {
        $("#btnSaveTemplate").addClass("d-none");
        if ($("#fileUploadComplete").val() == "1")
            return true;
        return false;
    }
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

                        if(impTempObj.selected[0].dataupd != "" && impTempObj.selected[0].dataupd == "T"){
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
                        else
                        {
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
    commonReadyTasks();

    $(document).on("keydown", "input[type='text'],input[type='radio'],input[type='checkbox']", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
        }
    });

    createMultiselectControl();

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

    $(document).on("click", "#excel1", function () {
        if (validateDataSearchWiz() == true) {
            hdnTemplateName = $("#hdnTemplateName").val();
            if (hdnTemplateName == "") {
                hdnTemplateName = $("#ddlImpTbl :selected").text();
            }
            $("#hdnTemplateName").val(hdnTemplateName + ".xlsx");
            $("#btnCreateTemplate").click();
        }
    });
    $(document).on("click", "#CSV1", function () {
        if (validateDataSearchWiz() == true) {
            hdnTemplateName = $("#hdnTemplateName").val();
            if (hdnTemplateName == "") {
                hdnTemplateName = $("#ddlImpTbl :selected").text();
            }
            $("#hdnTemplateName").val(hdnTemplateName + ".csv");
            $("#btnCreateTemplate").click();
        }
    });
});

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
                        cols: ["stransid", "templatename", "templatecap", "impfields", "recordid","dataupd","fldpkey"],
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

                    if($("#dataupdatecheck").is(":checked")){
                        impTempObj.flds.vals[5] ="T";
                        impTempObj.flds.vals[6] =$("#tstFlds").val();
                    }
                    else
                    {
                        impTempObj.flds.vals[5] ="F";
                        impTempObj.flds.vals[6] ="";
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
