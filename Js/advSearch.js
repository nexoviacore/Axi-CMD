$(document).ready(function () {
    if (typeof _isPageInfo != "undefined" && _isPageInfo != "") {
        let _pageNo = _isPageInfo.split('♠')[2];
        let _pageTotRows = _isPageInfo.split('♠')[1];
        let _pageSize = _isPageInfo.split('♠')[0];
        if (_pageNo == 1 && _pageTotRows <= 100) {
            $("#divFilter *").attr("disabled", "disabled").off('click');
            $("#btnFilterClear").addClass('noHover');
            $("#btnFilterSearch").removeClass('btn-primary').addClass('btn-white btn-color-gray-600 btn-active-primary noHover');
        } else if (_pageNo == 1 && _pageSize > 100) {
            $("#divFilter *").attr("disabled", "disabled").off('click');
            $("#btnFilterClear").addClass('noHover');
            $("#btnFilterSearch").removeClass('btn-primary').addClass('btn-white btn-color-gray-600 btn-active-primary noHover');
        } else if (_pageNo == 1 && _pageSize == 0) {
            $("#divFilter *").attr("disabled", "disabled").off('click');
            $("#btnFilterClear").addClass('noHover');
            $("#btnFilterSearch").removeClass('btn-primary').addClass('btn-white btn-color-gray-600 btn-active-primary noHover');
        }
    }

    var jTbl = $j('#GridView1');
    if (jTbl.find("tbody>tr>th").length > 0) {
        jTbl.find("tbody").before("<thead><tr></tr></thead>");
        jTbl.find("thead:first tr").append(jTbl.find("th"));
        jTbl.find("tbody tr:first").remove();
    }
    // applyCurrentTheme();
    var heightOfDT = $(callParentNew("axpertPopupWrapper")).height() - ($(".adv-dv1").outerHeight() + $(".adv-dv2").outerHeight() + $("#divTitle").outerHeight() + 100);

    //if (typeof _isMulSel != "undefined" && _isMulSel == "true") {
    //    oTable = jTbl.DataTable({
    //        "order": [],
    //        "scrollX": true,
    //        "scrollY": heightOfDT + "px",
    //        "dom": '<"float-end"l>',
    //        "columnDefs": [{
    //            "oLanguage": {
    //                "Search": '<i class="icon-search"></i>',
    //                "sEmptyTable": "My Custom Message On Empty Table"
    //            },
    //            "targets": 0,
    //            "searchable": false,
    //            "orderable": false,
    //            "bJQueryUI": true,
    //            "bFilter": false,
    //            "bInfo": false,
    //            //"className": 'select-checkbox',
    //            "render": function (data, type, full, meta) {
    //                return '<div class="d-flex form-check form-check-custom form-check-sm"><input type="checkbox" name="advsearchChk" class="cursor-pointer mx-auto form-check-input border-gray-500" onclick="javascript:CheckboxAdvSearch(this);"></div>';
    //            }
    //        }],
    //        //"select": {
    //        //    "style": 'os',
    //        //    "selector": 'td:first-child'
    //        //},
    //        "bLengthChange": false,
    //        "bPaginate": false,
    //        "tabIndex": -1,
    //        "info": false,
    //    });
    //    $j('#txtSrchText').keyup(function () {
    //        oTable.search($j(this).val()).draw();
    //    });
    //} else {
    //    oTable = jTbl.DataTable({
    //        "order": [],
    //        "scrollX": true,
    //        "scrollY": heightOfDT + "px",
    //        "dom": '<"float-end"l>',
    //        "columnDefs": [{
    //            "oLanguage": {
    //                "Search": '<i class="icon-search"></i>',
    //                "sEmptyTable": "My Custom Message On Empty Table"
    //            },
    //            "targets": 'no-sort',
    //            "orderable": false,
    //            "bJQueryUI": true,
    //            "bFilter": false,
    //            "bInfo": false
    //        }],
    //        "bLengthChange": false,
    //        "bPaginate": false,
    //        "tabIndex": -1,
    //        "info": false,
    //    });
    //    $j('#txtSrchText').keyup(function () {
    //        oTable.search($j(this).val()).draw();
    //    });
    //}

    oTable = jTbl.DataTable({
        "order": [],
        "scrollX": true,
        "scrollY": heightOfDT + "px",
        "dom": '<"float-end"l>',
        "columnDefs": [{
            "oLanguage": {
                "Search": '<i class="icon-search"></i>',
                "sEmptyTable": "My Custom Message On Empty Table"
            },
            "targets": 0,
            "searchable": false,
            "orderable": false,
            "bJQueryUI": true,
            "bFilter": false,
            "bInfo": false,
            "render": function (data, type, full, meta) {
                if (typeof _isMulSel != "undefined" && _isMulSel == "true")
                    return '<div class="d-flex form-check form-check-custom form-check-sm"><input type="checkbox" name="advsearchChk" class="cursor-pointer mx-auto form-check-input border-gray-500" onclick="javascript:CheckboxAdvSearchMul(this);"></div>';
                else
                    return '<div class="d-flex form-check form-check-custom form-check-sm"><input type="checkbox" name="advsearchChk" class="cursor-pointer mx-auto form-check-input border-gray-500" onclick="javascript:CheckboxAdvSearch(this);"></div>';
            }
        }],
        "bLengthChange": false,
        "bPaginate": false,
        "tabIndex": -1,
        "info": false,
    });

    $j('#txtSrchText').keyup(function () {
        oTable.search($j(this).val()).draw();
    });

    $j("[data-toggle=tooltip]").tooltip();

    $j('[data-toggle="popover"],[data-original-title]').popover();

    $j(document).on('click', function (e) {
        $j('[data-toggle="popover"],[data-original-title]').each(function () {
            if (!$j(this).is(e.target) && $j(this).has(e.target).length === 0 && $j('.popover').has(e.target).length === 0) {
                $j(this).popover('hide').data('bs.popover').inState.click = false // fix for BS 3.3.6
            }

        });
    });

    //var Idnx = $("select[name='ddlSearchFld'] option:selected").index();
    //var dtTpe = $('#ddlDataType option').eq(Idnx).val();
    //enableCondition(dtTpe);


    $("#ddlCondition").change(function () {
        var dllval = this.value;
        if (dllval == "between")
            $("#txtfltbetween").show();
        else
            $("#txtfltbetween").hide();
    });

    if ($("#ddlCondition").val() == "between")
        $("#txtfltbetween").show();
    else
        $("#txtfltbetween").hide();

    $(".table>tbody>tr").keyup(function (event) {
        if (event.keyCode == 13) {
            $(this).click();
        }
    });

    setTimeout(setTabloop, 10);

    //if (typeof _isMulSel != "undefined" && _isMulSel == "true") {
    //    setTimeout(SetCheckedOnload, 0);
    //}
    setTimeout(SetCheckedOnload, 0);
});


//$(document).on("change", "#ddlCondition", function () {
//    var Idnx = $(this + " option:selected").index();
//    var dtTpe = $('#ddlDataType option').eq(Idnx).val();
//    enableCondition(dtTpe);
//});

function enableCondition(type) {
    alert(type);

}

$(document).on("click", ".clearico", function () {
    var contID = $(this).closest('.profile-pic').children('.form-control').attr("id");
    $("#" + contID).val("");
});

function callserverbtn(btntype) {
    $("#" + btntype).click();
}

$(document).on("keydown.AxpCls", function (e) {
    if (e.keyCode == 27) {
        $(document).off("keydown.AxpCls")
        parent.closeRemodalPopup();
    }
});


function setTabloop() {
    var elemntsToCheck = 'button[tabindex!="-1"],a[tabindex!="-1"],input[tabindex!="-1"],select[tabindex!="-1"],textarea[tabindex!="-1"],table tbody tr[tabindex!="-1"]';
    var inputs = $('#main').find(elemntsToCheck).filter(':visible').not(':disabled');
    var firstInput = inputs.first();
    var lastInput = inputs.last();
    console.log(firstInput);
    console.log(lastInput);
    /*redirect last tab to first input*/
    lastInput.on('keydown', function (e) {
        if ((e.which === 9 && !e.shiftKey)) {
            e.preventDefault();
            firstInput.focus();
        }
    });
    firstInput.on('keydown', function (e) {
        if ((e.which === 9 && e.shiftKey)) {
            e.preventDefault();
            lastInput.focus();
        }
    });
}

function applyCurrentTheme() {
    var theme = eval(callParent('currentThemeColor'));
    // $j("#themecss").attr('href', "../App_Themes/" + theme + "/Stylesheet.min.css?v=23");
    if (theme == "" || theme == undefined) {
        if (window.parent.document)
            theme = $j("#themecss", window.parent.document).attr("href");
        else
            theme = $j("#themecss", window.opener.document).attr("href");
        $j("#themecss").attr("href", theme);
    }
}

function SetCheckedOnload() {
    if (typeof _isMulSel != "undefined" && _isMulSel == "true")
        $(".gridData thead tr th:eq(0)").append("<div class=\"d-flex form-check form-check-custom form-check-sm\"><input type=\"checkbox\" name=\"select_all\" id=\"advSearchChkAll\" onclick=\"javascript:AdvSearchChkAll(this);\" class=\"cursor-pointer mx-auto form-check-input border-gray-500\"></div>");
    else
        $(".gridData thead tr th:eq(0)").append("<div class=\"d-flex form-check form-check-custom form-check-sm\"><input type=\"checkbox\" name=\"select_all\" id=\"advSearchChkAll\" disabled=\"disabled\" class=\"cursor-pointer mx-auto form-check-input border-gray-500\"></div>");
    $(".gridData tbody tr.tableStripedFix").find("td").find("input").prop("checked", true);
    if (typeof _isMulSel != "undefined" && _isMulSel == "true") {
        if ($(".gridData tbody tr.tableStripedFix").length == $(".gridData tbody tr").length) {
            $(".gridData thead tr th:eq(0)").find("input").prop("checked", true);
        }
    }
}

var isChboxclickedMul = false;
function CheckboxAdvSearchMul(ele) {
    isChboxclickedMul = true;
    if ($(ele).is(":checked")) {
        $(ele).parents("tr").addClass("tableStripedFix bg-secondary");
        selectedValList.push($(ele).parents("td").next("td").text());
        let _arInd = unSelectedValList.indexOf($(ele).parents("td").next("td").text());
        if (_arInd > -1)
            unSelectedValList.splice(_arInd);
    }
    else {
        $(ele).parents("tr").removeClass("tableStripedFix bg-secondary");
        unSelectedValList.push($(ele).parents("td").next("td").text());
        let _arInd = selectedValList.indexOf($(ele).parents("td").next("td").text());
        if (_arInd > -1)
            selectedValList.splice(_arInd);
    }

    if ($(".gridData tbody tr.tableStripedFix").length == $(".gridData tbody tr").length) {
        $(".gridData thead tr th:eq(0)").find("input").prop("checked", true);
    } else
        $(".gridData thead tr th:eq(0)").find("input").prop("checked", false);
}

function SetchkAdsValMul(ele) {
    if (isChboxclickedMul == false) {
        //var _trInd = $(".gridData tbody tr:eq(" + ele + ")");
        var _trInd = $(".gridData tbody tr[onclick=\"SetchkAdsValMul('" + ele + "');\"]");

        if (_trInd.find("td").find("input").is(":checked"))
            _trInd.find("td").find("input").prop("checked", false);
        else
            _trInd.find("td").find("input").prop("checked", true);
        CheckboxAdvSearchMul(_trInd.find("td").find("input"));
        isChboxclickedMul = false;
    } else
        isChboxclickedMul = false;
}

function AdvSearchChkAll(ele) {
    if ($(ele).is(":checked")) {
        $(".gridData tbody tr").each(function (ind, ele) {
            var _trInd = $(".gridData tbody tr:eq(" + ind + ")");
            _trInd.find("td").find("input").prop("checked", true);
            _trInd.addClass("tableStripedFix bg-secondary");
            selectedValList.push(_trInd.find("td:eq(1)").text());
            let _arInd = unSelectedValList.indexOf(_trInd.find("td:eq(1)").text());
            if (_arInd > -1)
                unSelectedValList.splice(_arInd);
        });
    } else {
        $(".gridData tbody tr").each(function (ind, ele) {
            var _trInd = $(".gridData tbody tr:eq(" + ind + ")");
            _trInd.find("td").find("input").prop("checked", false);
            _trInd.removeClass("tableStripedFix bg-secondary");
            unSelectedValList.push(_trInd.find("td:eq(1)").text());
            let _arInd = selectedValList.indexOf(_trInd.find("td:eq(1)").text());
            if (_arInd > -1)
                selectedValList.splice(_arInd);
        });
    }
}
var _selTrIndex = "NoChange";
function SetchkAdsVal(ele) {
    if (isChboxclicked == false) {
        /*var _trInd = $(".gridData tbody tr:eq(" + ele + ")");*/
        var _trInd = $(".gridData tbody tr[onclick=\"SetchkAdsVal('" + ele + "');\"]");
        let _checked = _trInd.find("td").find("input").is(":checked");
        $(".gridData tbody tr").find("td").find("input").prop("checked", false);
        if (_checked)
            _trInd.find("td").find("input").prop("checked", false);
        else
            _trInd.find("td").find("input").prop("checked", true);
        CheckboxAdvSearch(_trInd.find("td").find("input"));
        isChboxclicked = false;
    } else
        isChboxclicked = false;
}

var isChboxclicked = false;
function CheckboxAdvSearch(ele) {
    isChboxclicked = true;
    if ($(ele).is(":checked")) {
        $(ele).parents("tbody").find("tr").removeClass("tableStripedFix bg-secondary")
        $(ele).parents("tr").addClass("tableStripedFix bg-secondary");
        _selTrIndex = $(ele).parents("tr").attr("onclick").replace("SetchkAdsVal('", "").replace("');", "");
        let _SerselTrIndex = $(ele).parents("tr").index();
        $(".gridData tbody tr").find("td").find("input").prop("checked", false);
        $(".gridData tbody tr:eq(" + _SerselTrIndex + ")").find("td").find("input").prop("checked", true);
    }
    else {
        $(ele).parents("tbody").find("tr").removeClass("tableStripedFix bg-secondary");
        $(".gridData tbody tr").find("td").find("input").prop("checked", false);
        _selTrIndex = -1;
    }
}
