var dtCulture = eval(callParent('glCulture'));
var advFilterDtCulture = dtCulture == "en-us" ? "MM/DD/YYYY" : "DD/MM/YYYY";

var dateOptions = ["Custom", "Today", "Yesterday", "Tomorrow", "This week", "Last week", "Next week", "This month", "Last month", "Next month", "This quarter", "Last quarter", "Next quarter", "This year", "Last year", "Next year"];
var dateOptionsId = ["customOption", "todayOption", "yesterdayOption", "tomorrowOption", "this_weekOption", "last_weekOption", "next_weekOption", "this_monthOption", "last_monthOption", "next_monthOption", "this_quarterOption", "last_quarterOption", "next_quarterOption", "this_yearOption", "last_yearOption", "next_yearOption"];

class EntityFilter {
    constructor() {
        this.metaData = {};
        this.filterObj = [];
        this.pageName = '';
        this.filterObj = {};
        this.activeFilterArray = [];
        this.activeFilterName = '';
        this.activeFilterId = '';
        this.entityTransId = '';
    }

    static inValid(value) {
        return value === null || value === undefined || value === '';
    }

    isUndefined(elem) {
        return typeof elem == "undefined";
    };

    init() {
        let _this = this;
        _this.activeFilterArray = [];
        _this.activeFilterName = '';
        _this.activeFilterId = '';
        _this.createFilterLayout();
        $('#applyFilterButton').on('click', function () {
            _this.handleApply();
        });

        $('#filterModal').modal('show');
    }

    checkUrlFilters() {
        let _this = this;
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        if (urlParams.get('applyfilter') == 'true' && urlParams.get('filterfld')) {            
            var filterFld = urlParams.get('filterfld');
            
            var fldData = _this.metaData.find(x => x.fldname == filterFld);
            if (fldData) {
                let fldType = _this.getFieldDataType(fldData);
                fldType = fldType.toUpperCase();
                if (!(fldType == "NUMERIC" || fldType == "DATE" || fldType == "TEXT" || fldType == "DROPDOWN"))
                    fldType = "TEXT";

                _this.activeFilterArray = [];
                let tempObj = {};
                switch (fldType) {
                    case "DROPDOWN":
                        var val = urlParams.get('filterval');
                        if (_entityCommon.isValid(val)) {
                            tempObj = { ftransid: _entityFilter.entityTransId, fldname: filterFld, datatype: fldType, value: [val] }
                        }

                        break;
                    case "DATE":
                        var fromDate = urlParams.get('filterfrom');
                        var toDate = urlParams.get('filterto');

                        toDate = moment(toDate, "DD-MMM-YYYY").subtract(1, 'days').format("DD-MMM-YYYY");

                        tempObj = { ftransid: _entityFilter.entityTransId, fldname: filterFld, datatype: fldType, from: "", to: "" };

                        if (_entityCommon.isValid(fromDate)) {
                            tempObj["from"] = fromDate;
                        }
                        if (_entityCommon.isValid(toDate)) {
                            tempObj["to"] = toDate;
                        }

                        tempObj["condition"] = "customOption";
                        break;

                    case "TEXT":
                        var val = urlParams.get('filterval');
                        if (_entityCommon.isValid(val)) {
                            tempObj = { ftransid: _entityFilter.entityTransId, fldname: filterFld, datatype: fldType, value: val }
                        }

                        tempObj["condition"] = "EQUALS";
                        break;
                    default:
                        return;
                        break;
                }

                _this.activeFilterArray.push(tempObj);

                let filterCaption = _this.constructFilterCaption(_this.activeFilterArray);
                _this.activeFilterId = _this.getCurrentTimestamp();
                _this.activeFilterName = filterCaption;

                _this.filterObj[_this.activeFilterId] = { caption: _this.activeFilterName, filter: _this.activeFilterArray, save: false }
                _this.createFilterPills();

            }
        }
    }

    readFilterInput() {
        let _this = this;
        
        var filterArray = [];
        document.querySelectorAll(`#dvModalFilter .filter-fld`).forEach(fld => {
            let fldId = fld.id.replace("filter_", "");
            let filterval = fld.value;
            let fldType = fld.dataset.type;
            let fldTransId = fld.dataset.transid;

            fldType = fldType?.toUpperCase() || "";
            let tempObj = {};
            switch (fldType) {
                case "DROPDOWN":
                    if (EntityFilter.inValid(filterval) || filterval == 0)
                        return;

                    filterval = $(fld).val();
                    tempObj = { ftransid: fldTransId, fldname: fldId, datatype: fldType, value: filterval }
                    filterArray.push(tempObj);
                    break;

                case "DATE":
                    var dates = fld.querySelectorAll("input");
                    var fromDate = dates[0];
                    var toDate = dates[1];

                    if (fromDate.value == "" && toDate.value == "") return;

                    tempObj = { ftransid: fldTransId, fldname: fldId, datatype: fldType, from: "", to : "" };

                    if (fromDate.value != "") {
                        filterval = moment(fromDate.value, advFilterDtCulture).format("DD-MMM-YYYY");
                        tempObj["from"] = filterval;
                    }
                    if (toDate.value != "") {
                        filterval = moment(toDate.value, advFilterDtCulture).format("DD-MMM-YYYY");
                        tempObj["to"] = filterval;
                    }

                    tempObj["condition"] = $(`#${fld.id}_dateoption option:selected`).val();
                    filterArray.push(tempObj);
                    break;

                case "NUMERIC":
                    var nums = fld.querySelectorAll("input");
                    var fromNum = nums[0];
                    var toNum = nums[1];
                    if (fromNum.value == "" && toNum.value == "") return;

                    tempObj = { ftransid: fldTransId, fldname: fldId, datatype: fldType, from: "", to: "" };

                    if (fromNum.value != "") {
                        filterval = fromNum.value;
                        tempObj["from"] = filterval;
                    }
                    if (toNum.value != "") {
                        filterval = toNum.value;
                        tempObj["to"] = filterval;
                    }

                    filterArray.push(tempObj);
                    break;
                case "TEXT":
                    var fldVal = $(fld).val();
                    if (_entityCommon.inValid(fldVal)) return;

                    tempObj = { ftransid: fldTransId, fldname: fldId, datatype: fldType };

                    tempObj["value"] = filterval;
                    tempObj["condition"] = document.querySelector(`#${fld.id}_searchoption`).value || "CONTAINS";

                    filterArray.push(tempObj);
                    break;
                default:
                    break;
            }
        });

        _this.activeFilterArray = filterArray;
    }

    handleApply() {
        let _this = this;
        _this.activeFilterArray = [];
        _this.readFilterInput();
        let filterCaption = _this.constructFilterCaption(_this.activeFilterArray);
        _this.activeFilterId = _this.activeFilterId || _this.getCurrentTimestamp();
        _this.activeFilterName = filterCaption;
        var saveFilter = false;
        if (_this.activeFilterArray.length == 0) {
            showAlertDialog("error", "Please select atleast one filter.");
            return;
        }

        if ($('#filterGroupCheckbox').is(":checked")) {
            $('#filterGroupModalWrapper').modal('show');

            let filterText = $('#filterGroupName').val();
            if (!_entityCommon.inValid(filterText)) {                
                _this.activeFilterName = filterText
            }
            saveFilter = true;
        } else {
            $('#filterGroupModalWrapper').modal('hide');
            saveFilter = false;                       
        }

        _this.filterObj[_this.activeFilterId] = { caption: _this.activeFilterName, filter: _this.activeFilterArray, save: saveFilter}
        _this.createFilterPills();
        if (saveFilter)
            _this.saveFilters();
        _this.applyFilters();
    }

    saveFilters() {
        let _this = this;
        let tempObj = {};
        for (let key in this.filterObj) {
            if (this.filterObj[key]?.save == true) {
                tempObj[key] = this.filterObj[key];
            }
        }

        var data = {
            page: _this.pageName,
            transId: _this.entityTransId,
            properties: { "FILTERS": JSON.stringify(tempObj) },
            allUsers: false
        }

        _entityCommon.setAnalyticsDataWS(data, () => {
            //Do nothing
        }, (error) => {
            showAlertDialog("error", error.status + " " + error.statusText);
        })
    }

    removeFilter(key) {
        $(`div.${key}`).remove();
    
        let _this = this;
        let tempObj = {};
        delete _this.filterObj[key];
    
        for (let key in _this.filterObj) {
            if (_this.filterObj[key]?.save === true) {
                tempObj[key] = _this.filterObj[key];
            }
        }
    
        if (Object.keys(tempObj).length === 0) {
            $(".filterPills").html("");
            $(".filterPills").addClass("d-none");
    
            // Trigger to show all data when no filters remain
            handlePillClick('All');
        }
    
        var data = {
            page: _this.pageName,
            transId: _this.entityTransId,
            properties: { "FILTERS": JSON.stringify(tempObj) },
            allUsers: false
        };
    
        _entityCommon.setAnalyticsDataWS(data, () => {
            // Do nothing
        }, (error) => {
            showAlertDialog("error", error.status + " " + error.statusText);
        });
    
        $(`.filterPills .${key}`).remove();
    }


    constructFilterCaption(data) {
        return data.map(item => {
            switch (item.datatype) {
                case 'TEXT':
                    return item.value;
                    break;
                case 'DATE':
                    let str = '';
                    if (item.condition == "customOption") {
                        if (item.from)
                            str += `from ${item.from} `
                        if (item.to)
                            str += `till ${item.to}`
                    }
                    else {
                        var idx = dateOptionsId.indexOf(item.condition);
                        str += dateOptions[idx];
                    }
                    return str;
                case 'NUMERIC':
                    return `${item.from || "-"} to ${item.to || "-"}`;
                case 'DROPDOWN':
                    return item.value.join(', ');
                default:
                    return '';
            }
        }).join(', ');
    }


    applyFilters() {
        //Write filter apply logic here.
    }



    generateFilterString(fldType, filterval, rangeType) {
        return rangeType ? `${fldType}:${rangeType}:${filterval}` : `${fldType}:${filterval}`;
    }


    getCurrentTimestamp() {
        const now = new Date();
        return "Filter-" + now.toISOString().replace(/[-:TZ.]/g, '');
    }

    createFilterLayout(isEditMode = false) {
        let container = $(`#dvModalFilter`);
        container.html("");
    
        if (!isEditMode) {
            $('#filterGroupName').val("");
        }
    
        this.metaData.forEach((field) => {
            if (field.hide === 'T') return;
            this.generateFilterHTML(field);            
        });
    
        this.initializeDropdowns();
        this.initializeDatePickers();
    }

    generateFilterHTML(field) {
        let fldtype = this.getFieldDataType(field);
        let fldcap = field.fldcap || '';
        let fldId = `filter_${field.fldname}`;
        let fldname = field.fldname;
        let fldTransId = field.ftransid;
        let filterHTML = '';
        let skipFields = ["transid", "axpeg_nextlevel"];

        if (fldtype?.toUpperCase() === "BUTTON" || fldtype?.toUpperCase() === "ATTACHMENTS" || fldtype?.toUpperCase() === "IMAGE" || (skipFields.indexOf(fldname.toLowerCase()) > -1)) return;

        if (field.fdatatype === "n") fldtype = "Numeric";

        switch (fldtype) {
            case 'DropDown':
                filterHTML = `
                <div class="row" data-type="${fldtype}">
                    <div class="col-md-3 fldCaption"><p class="form-group">${fldcap}</p></div>
                    <div class="col-md-9 fldCaption">
                        <select class="form-control filter-fld" data-type="${fldtype}" data-transid="${fldTransId}" id="${fldId}" name="${fldtype}" multiple>
                        </select>
                    </div>
                </div>`;
                break;

            case 'Numeric':
                filterHTML = `
                <div class="row filter-fld" data-type="${fldtype}" id="${fldId}" data-type="${fldtype}" data-transid="${fldTransId}">
                    <div class="col-md-3 fldCaption"><p class="form-group">${fldcap}</p></div>
                    <div class="col-md-9">
                        <div class="form-group form-row fldCaption">
                            <div class="col-md-6 col"><label>From</label><input type="number" id="${fldId}_from" class="form-control" /></div>
                            <div class="col-md-6 col"><label>To</label><input type="number" id="${fldId}_to" class="form-control" /></div>
                        </div>
                    </div>
                </div>`;
                break;

            case 'Date':

                filterHTML += `<div class="row filter-fld" data-type="${fldtype}" id="${fldId}" data-transid="${fldTransId}"><div class="col-md-3 fldCaption"><p class="form-group ">${fldcap}</p></div>
                                    <div class="col-md-4 fldCaption">
                                    <select class="form-select dateFilter" type="text" id="${fldId}_dateoption" name="${fldId}" data-field="${fldname}" onchange="generateAdvFilterDates('${fldId}');">`;
                for (var i = 0; i < dateOptions.length; i++) {
                    filterHTML += `<option value=${dateOptionsId[i]}>${dateOptions[i]}</option>`;
                }
                filterHTML += `</select> 
                                    </div>
                                    <div class="col-md-5">
                                        <div class="form-group form-row fldCaption">
                                        <div class="col-md-6 col">
                                        <label>From</label>           
                                         <input id="${fldId}_from" name="${fldId}_from" value="" maxlength="10" type="date" class="form-control flatpickr-input" data-input="" onchange="validateDateRange('${fldId}');">
                                        </div>
                                        <div class="col-md-6 col">
                                        <label>To</label>           
                                        <input id="${fldId}_to" name="${fldId}_to" value="" maxlength="10" type="date" class="form-control flatpickr-input" data-input="" onchange="validateDateRange('${fldId}');">
                                        </div></div></div>   
                                    </div>`;
                break;

            default:
                filterHTML = `
                <div class="row" data-type="${fldtype}">
                    <div class="col-md-3 fldCaption"><p class="form-group">${fldcap}</p></div>
                    <div class="col-md-4 fldValue">
                        <select class="form-select" id="${fldId}_searchoption">
                            <option value="STARTSWITH">Starts with</option>
                            <option value="EQUALS">Equals</option>
                            <option value="CONTAINS">Contains</option>
                            <option value="ENDSWITH">Ends with</option>
                        </select>
                    </div>
                    <div class="col-md-5 fldValue"><input type="text" id="${fldId}" data-type="TEXT" class="form-control filter-fld" data-transid="${fldTransId}"/></div>
                </div>`;
                break;
        }

        $(`#dvModalFilter`).append(filterHTML);
    }

    updateFilterLayout(activeFilter) {        
        activeFilter.filter.forEach(fldFilter => {
            var fldType = fldFilter.datatype.toUpperCase();
            var fldId = fldFilter.fldname;
            var condition = fldFilter.condition;
            var value = fldFilter.value;
            switch (fldType) {
                case "TEXT":
                    document.getElementById(`filter_${fldId}`).value = value;
                    document.getElementById(`filter_${fldId}_searchoption`).value = condition;
                    break;
                case "DROPDOWN":
                    value.forEach(item => {
                        const option = new Option(item, item,);
                        $(`#filter_${fldId}`).append(option);
                    });
                    $(`#filter_${fldId}`).val(value).trigger('change');
                    break;
                case "DATE":                   
                    document.getElementById(`filter_${fldId}_dateoption`).value = condition;
                    $(`#filter_${fldId}_dateoption`).trigger('change');
                    var fromDate = document.getElementById(`filter_${fldId}_from`);
                    var toDate = document.getElementById(`filter_${fldId}_to`);

                    if (condition === "customOption") {                        
                        fromDate.value = moment(fldFilter.from, "DD-MMM-YYYY").format(advFilterDtCulture) || "";
                        toDate.value = moment(fldFilter.to, "DD-MMM-YYYY").format(advFilterDtCulture) || "";
                        $(fromDate).prop('disabled', false).addClass('disabledDate');
                        $(toDate).prop('disabled', false).addClass('disabledDate');
                    } else {
                        $(fromDate).prop('disabled', true).removeClass('disabledDate');
                        $(toDate).prop('disabled', true).removeClass('disabledDate');
                    }
                    break;

                case "NUMERIC":
                    var fromNum = document.getElementById(`filter_${fldId}_from`);
                    var toNum = document.getElementById(`filter_${fldId}_to`);
                    fromNum.value = fldFilter.from || "";
                    toNum.value = fldFilter.to || "";
                    break;
                default:
                    break;
            }

        });      
    }

    getFieldDataType(fldProps) {
        if (EntityFilter.inValid(fldProps.cdatatype)) {
            switch (fldProps.fdatatype) {
                case "n": return "Numeric";
                case "d": return "Date";
                case "c": return "Text";
                case "i": return "Image";
                case "t": return "Text";
                default: return "Text";
            }
        } else {
            return fldProps.cdatatype;
        }
    }



    initializeDropdowns() {
        document.querySelectorAll(`#dvModalFilter .filter-fld[data-type=DropDown]`).forEach(fld => {        
            let fldId = fld.id.replace("filter_", "");
            let transId = fld.dataset.transid;
           
            $(fld).select2({
                multiple: true,  // Enable multi-selection
                minimumInputLength: 0,
                ajax: {
                    url: top.window.location.href.toLowerCase().substring(0, top.window.location.href.toLowerCase().indexOf("/aspx/")) + '/aspx/Analytics.aspx/GetEntityDropDownDataWS',
                    type: 'POST',
                    dataType: 'json',
                    contentType: "application/json",
                    delay: 250,
                    data: function (params) {
                        let data = {
                            fldId: fldId,
                            transId: transId,
                            searchText: params.term ?? ""
                        };
                        return JSON.stringify(data);
                    },
                    processResults: function (data) {
                        try {
                            const parsedData = JSON.parse(data.d);
                            const resultData = parsedData.result;

                            if (resultData && resultData.data?.data && Array.isArray(resultData.data.data)) {
                                const resultsArray = resultData.data.data.map(item => ({
                                    id: item.datavalue,
                                    text: item.datavalue
                                })).filter(item => item.id !== null && item.id?.toLowerCase() !== "null");

                                return { results: resultsArray };
                            } else {
                                console.warn("data is not in the expected format.");
                                return { results: [] };
                            }
                        } catch (error) {
                            console.error("Error processing results: ", error);
                            return { results: [] };
                        }
                    },
                    cache: true
                }
            })
                //// Handle select/unselect events properly
                //.on("select2:select", function (e) {
                //    let selectedValues = $(this).val();
                //    console.log("Selected values for field:", fldId, selectedValues);
                //})
                //.on("select2:unselect", function (e) {
                //    let selectedValues = $(this).val();
                //    console.log("Unselected values for field:", fldId, selectedValues);
                //});
        });
    }


    initializeDatePickers() {
        let glCulture = eval(callParent('glCulture'));
        let dtFormat = (glCulture === "en-us") ? "m/d/Y" : "d/m/Y";
        $(".flatpickr-input").flatpickr({
            dateFormat: dtFormat,
            disableMobile: true
        });
    }


    clearFilters() {
        //parent.ShowDimmer(true);
        document.querySelectorAll(`#dvModalFilter .filter-fld`).forEach(fld => {
            let fldType = fld.dataset.type;

            if (_entityCommon.inValid(fldType))
                return;

            fldType = fldType.toUpperCase();

            switch (fldType) {
                case "DROPDOWN":
                    $(fld).val("");
                    $(fld).trigger("change");
                    break;
                case "NUMERIC":
                    var nums = fld.querySelectorAll("input");
                    var fromNum = nums[0];
                    var toNum = nums[1];
                    fromNum.value = "";
                    toNum.value = "";
                    break;
                case "DATE":
                    var dates = fld.querySelectorAll("input");
                    var fromDate = dates[0];
                    var toDate = dates[1];
                    fromDate.value = "";
                    toDate.value = "";
                    break;
                default:
                    fld.value = "";
                    break;
            }

        });

    }

    filterModelClose() {
        $('#filterGroupName').val('')
        $('#filterGroupModalWrapper').modal('hide');

        document.getElementById('filterGroupCheckbox').checked = false
        document.getElementById('filterGroupName').disabled = true

        $('#dvModalFilter').html("");
        $('#filterModal').modal('hide');

    }

    createFilterPills() {
        if (Object.keys(this.filterObj).length === 0) {
            $('.filterPills').css('display', 'none');
        } else {
            $('.filterPills').css('display', 'flex');
        }        

        $(".filterPills").html("");
        let filterGroupPill = "";
        for (let key in this.filterObj) {
            filterGroupPill += `<div class="${key} filterGroupBadge badge rounded-pill bg-primary d-flex align-items-center gap-2 py-2 px-6" role="button" style="max-width: fit-content;" data-toggle="tooltip" data-placement="top" data-html="true" title="${this.filterObj[key].caption}" onclick="handlePillClick('${key}')">
                ${this.filterObj[key].caption}
                <span title='edit' class="pillEditButton material-icons btn btn-primary px-2 py-1" onclick="editPill('${key}'); event.stopPropagation();">edit</span> 
                <span title="remove" class="pillRemoveButton material-icons btn btn-primary px-2 py-1" onclick="removePill('${key}'); event.stopPropagation();" >close</span> 
                </div>`;
        }

        if (filterGroupPill) {
            $('.filterPills').append(`<button  class="filterGroupBadge badge rounded-pill bg-primary d-flex align-items-center gap-2 py-2 px-6 border-0" style="max-width: fit-content;" data-toggle="tooltip" data-placement="top" data-html="true" onclick="handlePillClick('All'); return false;">All</button>` + filterGroupPill);
        }

    }

    editPill(key) {
        let _this = this;
        _this.activeFilterId = key;
        _this.activeFilterName = _this.filterObj[key].caption;
    
        document.getElementById('filterGroupName').value = _this.activeFilterName;
    
        $('#filterModal').modal('show');        
    
        if (_this.filterObj[key].save) {
            document.getElementById('filterGroupCheckbox').checked = true;
            document.getElementById('filterGroupName').disabled = false;
        } else {
            document.getElementById('filterGroupCheckbox').checked = false;
            document.getElementById('filterGroupName').disabled = true;
        }
    
        if ($('#dvModalFilter').html() === "") {
            // Pass true to indicate edit mode
            _this.createFilterLayout(true);
        }
    
        if (_entityCommon.isValid(_this.filterObj[key])) {
            _this.updateFilterLayout(_this.filterObj[key]);
        }
    }
}

function openFilters() {
    _entityFilter.init();
}

function resetFilters() {
    _entityFilter.clearFilters();
    _entityFilter.filterModelClose();
}

function filterModelClose() {
    _entityFilter.filterModelClose();
}

function removePill(key) {
    // Remove the filter pill
    _entityFilter.removeFilter(key);

    // Check if there are any remaining filters
    if (Object.keys(_entityFilter.filterObj).length === 0) {
        // Trigger to show all data when no filters remain
        handlePillClick('All');
    }
}


function editPill(key) {
    _entityFilter.editPill(key);
}

function handleApply() {
    _entityFilter.handleApply();
}

function handlePillClick(key) {
    if (key == 'All') {
        _entityFilter.activeFilterArray = [];
    } else {
        _entityFilter.activeFilterArray = _entityFilter.filterObj[key].filter;
        _entityFilter.activeFilterArray.forEach(filter => {
            if (filter.datatype.toUpperCase() === "DATE") {
                if (filter.condition != "customOption") {
                    let filterDates = _entityCommon.getDatesBasedonSelectionForBetweenFilter(filter.condition);
                    filter.from = filterDates.from;
                    filter.to = filterDates.to;
                }
            }
        });
    }
    // Apply the filters to update the data view
    _entityFilter.applyFilters();
}


function validateDateRange(fieldId) {
    var fromDateElement = document.getElementById(fieldId + '_from');
    var toDateElement = document.getElementById(fieldId + '_to');
    var fromDate = fromDateElement.value;
    var toDate = toDateElement.value;

    if (fromDate && toDate) {
        var fromDateObj = parseDate(fromDate);
        var toDateObj = parseDate(toDate);

        if (!fromDateObj || !toDateObj) {
            alert('Invalid date format.');
            return;
        }

        // Check if the "To" date is earlier than the "From" date
        if (toDateObj < fromDateObj) {
            alert('The "To" date cannot be earlier than the "From" date.');
            // Clear the "To" date
            toDateElement.value = '';
        }
    }
}


function generateAdvFilterDates(dateFld) {

    var selectionvalue = document.querySelector(`#${dateFld}_dateoption`).value;
    var currentDate = new Date();
    var fromDate = document.querySelector(`#${dateFld}_from`);
    var toDate = document.querySelector(`#${dateFld}_to`);

    var fromToObj = _entityCommon.getDatesBasedonSelection(selectionvalue);

    fromDate.value = fromToObj.from;
    toDate.value = fromToObj.to;

    if (selectionvalue == "customOption") {
        fromDate.disabled = false;
        toDate.disabled = false;
        fromDate.classList.add('disabledDate');
        toDate.classList.add('disabledDate');
    }
    else {
        fromDate.disabled = true;
        toDate.disabled = true;
        fromDate.classList.remove('disabledDate');
        toDate.classList.remove('disabledDate');
    }

}
