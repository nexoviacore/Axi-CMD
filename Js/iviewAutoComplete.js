$(document).ready(function () {
    try {
        $("#ddlSearchFld, #ddlCondition").select2({
            minimumResultsForSearch: Infinity,
        });
        var jTbl = $j('#GridView1');
        if (jTbl.find("tbody>tr>th").length > 0) {
            jTbl.find("tbody").before("<thead><tr></tr></thead>");
            jTbl.find("thead:first tr").append(jTbl.find("th"));
            jTbl.find("tbody tr:first").remove();
        }

        oTable = jTbl.DataTable({
            "order": [],
            "scrollX": true,
            "scrollY": "",
            "dom": '<"float-end"l>',
            "columnDefs": [{
                "oLanguage": {
                    "Search": '<i class="icon-search"></i>',
                    "sEmptyTable": "My Custom Message On Empty Table"
                },
                "targets": 'no-sort',
                "orderable": false,
                "bJQueryUI": true,
                "bFilter": false,
                "bInfo": false
            }],
            "bLengthChange": false,
            "bPaginate": false,
            "tabIndex": -1,
            "info": false,
        });
        $j('#txtSrchText').keyup(function (event) {
            oTable.search($j(event.currentTarget).val()).draw();
            var rowCount = oTable.rows({ search: 'applied' }).count();
            $("#dvFooter #lblend").text(rowCount);
        });

        $j("[data-bs-toggle=tooltip]").tooltip();

        $j('[data-bs-toggle="popover"],[data-bs-original-title]').popover();

        $j(document).on('click', function (e) {
            $j('[data-bs-toggle="popover"],[data-bs-original-title]').each(function () {
                if (!$j(this).is(e.target) && $j(this).has(e.target).length === 0 && $j('.popover').has(e.target).length === 0) {
                    $j(this).popover('hide').data('bs.popover').inState.click = false // fix for BS 3.3.6
                }

            });
        });
        $(".table>tbody>tr").keyup(function (event) {
            if (event.keyCode == 13) {
                $(this).click();
            }
        });

        // setTimeout(setTabloop, 10);
        parent.ShowDimmer(false);
    } catch (error) {
        parent.ShowDimmer(false);
        showAlertDialog("error", error.message);
    }
});

$(document).on("click", ".clearico", function (event) {
    var contID = $(event.currentTarget).closest('.profile-pic').children('.form-control').attr("id");
    $(`#${contID}`).val("");
    oTable.search("").draw();
});

function callserverbtn(btntype) {
    if (btntype == "btnsearch") {
        var filterValue = $("#txtfilter").val();
        if (filterValue == "") {
            showAlertDialog("warning", "Filter value cannot be left empty.");
            return false;
        } else if (filterValue.length <= 2) {
            showAlertDialog("warning", "Enter at least 3 minimum characters to search.");
            return false;
        }
    }

    $(`#${btntype}`).click();
}

// function setTabloop() {
//     var elemntsToCheck = 'button[tabindex!="-1"],a[tabindex!="-1"],input[tabindex!="-1"],select[tabindex!="-1"],textarea[tabindex!="-1"],table tbody tr[tabindex!="-1"]';
//     var inputs = $('#main').find(elemntsToCheck).filter(':visible').not(':disabled');
//     var firstInput = inputs.first();
//     var lastInput = inputs.last();
//     console.log(firstInput);
//     console.log(lastInput);
//     /*redirect last tab to first input*/
//     lastInput.on('keydown', function (e) {
//         if ((e.which === 9 && !e.shiftKey)) {
//             e.preventDefault();
//             firstInput.focus();
//         }
//     });
//     firstInput.on('keydown', function (e) {
//         if ((e.which === 9 && e.shiftKey)) {
//             e.preventDefault();
//             lastInput.focus();
//         }
//     });
// }

function loadParent(a) {
    try {
        var gov = document.getElementById("searchlist");
        var grviewpno = document.getElementById("pgno");
        var pno = parseInt(grviewpno.value)
        gov.selectedIndex = (pno * 10) + parseInt(a);

        var thisVal = gov[gov.selectedIndex].value;
        callParentNew("loadPopUpPage", "id").dispatchEvent(new CustomEvent("close"));
        var thisSelect = parent.$(callParentNew(`${fldname}`, "id"));
        var optionExists = false;
        try {
            // Use the index and option parameters in filter function
            optionExists = thisSelect.find("option").filter((index, opt) => {
                // Check if the option is not undefined or null
                if (typeof opt !== "undefined" && opt !== null) {
                    // Compare the value of the option with thisVal
                    return $(opt).val() === thisVal;
                }
            }).length > 0;
        } catch (error) {
            console.log(error.message);
        }

        if (!optionExists) {
            var newOption = new Option(thisVal, thisVal, false, false);
            thisSelect.append(newOption).val(thisVal).trigger('change');
        } else {
            thisSelect.val(thisVal).trigger("change");
        }
        thisSelect.data("select2").dropdown.$search.val("");

    } catch (ex) { }
}