var isFormDirty = false;
var butres = "";
var totelements = "";
$(document).ready(function () {    
    $("#save").prop({ 'value': eval(callParent('lcm[442]')), 'title': eval(callParent('lcm[442]')) });
    $("#btnClose").prop({ 'value': eval(callParent('lcm[192]')), 'title': eval(callParent('lcm[192]')) });

    //menu header text
    $("#aButtons").text(eval(callParent('lcm[333]')));

    //table header text for DCs, field name, buttons, listview buttons
    $("#thButtonName").text(eval(callParent('lcm[355]')));
    $(".view").text(eval(callParent('lcm[335]')));
    $(".enable").text(eval(callParent('lcm[336]')));

    // ChangeTheme(window);
    checkSuccessAxpertMsg();
    $('#ViewControl input[type="checkbox"]').each((index, element) => {
        $(element).on("change", function () {
            $(element).addClass('dirty');
        });
    });
    totelements = document.form1.elements.length;
    enableDisableChkboxes();//to enable/disable associated checkboxes buttons for DC's,Buttons,Fields,Listview buttons    
});

function setAccessValues() {
    for (i = 0; i < totelements; i++) {
        if (document.form1.elements[i].type == "checkbox") {
            var id = document.form1.elements[i].id;
            var vie = id.substring(0, id.length - 3);
            if (vie == "butview") {
                var viewsts = document.form1.elements[i].checked;
                var butname = document.form1.elements[i].value;
                var butval = document.form1.elements[i + 1].value;
                var enasts = document.form1.elements[i + 1].checked;
                butres = butres + '<' + butname + ' cap="' + butval + '" view="' + viewsts + '" enable="' + enasts + '" ></' + butname + '>';
                i = i + 1;
            }
        }
    }
    if (butres != "") {
        //update default values(xml tags) in the hidden fields dc_xml, fld_xml, but_xml, lv_xml
        var butxml = document.getElementById("but_xml");
        butxml.value = butres;
    }
}

//to check if any checkbox has changed
function checkIfFormChanges() {
    $('#ViewControl input[type="checkbox"]').each((index, element) => {
        if (!isFormDirty && $(element).hasClass('dirty')) {
            isFormDirty = true;
        }
    });
    return isFormDirty;
}

//to display confirm dialog while unloading the form only if form inputs changes
function ConfirmLeave() {
    if ($(".jconfirm").length > 0) {
        $(".jconfirm").remove();
    }
    else {
        var ConfirmSaveCB = $.confirm({
            theme: 'modern',
            title: eval(callParent('lcm[155]')),
            rtl: callParentNew("gllangType") == "ar",
            onContentReady: function () {
                disableBackDrop('bind');
                //to display tooltips for Confirm & Cancel buttons
                $(".jconfirm-buttons button").each(function () {
                    var txt = $(this).text();
                    $(this).prop('title', txt.charAt(0).toUpperCase() + txt.slice(1))
                });
                $(".jconfirm-buttons .hotbtn").focus(); //to focus on Confirm button once dialog is opened
            },
            backgroundDismiss: 'false',
            escapeKey: 'cancel',
            content: eval(callParent('lcm[319]')),
            buttons: {
                confirm: {
                    text: eval(callParent('lcm[164]')),
                    btnClass: 'btn btn-primary',
                    action: function () {
                        ConfirmSaveCB.close();
                        parent.closeModalDialog();
                        parent.checkIfAnyActionPerformed();
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
}

//to enable/disable associated checkboxes for dc's, fields, buttons, listview buttons
function enableDisableChkboxes() {
    //checkbox change event for all View options
    $("[data-btn-view]").change(function () {
        var chkId = $(this).attr("data-btn-view");
        var status = $(this).is(":checked");
        if (!status)//if any view option is unselected then unselect the associated enable option
            $("[data-btn-enable='" + chkId + "']").prop("checked", false);
    });

    //checkbox change event for all Enable options
    $("[data-btn-enable]").change(function () {
        var chkId = $(this).attr("data-btn-enable");
        var status = $(this).is(":checked");
        if (status)//if any enable option is selected then select the associated view option
            $("[data-btn-view='" + chkId + "']").prop("checked", true);
    });
}

//check if any form elements has been changed, if yes display confirm dialog else close User Access Iview popup
function closeWindow() {
    if (checkIfFormChanges())
        ConfirmLeave();
    else
        parent.closeModalDialog();
}