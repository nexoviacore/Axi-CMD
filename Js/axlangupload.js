

$j(document).ready(function () {
    modalHeader = eval(callParent("divModalHeader", "id") + ".getElementById('divModalHeader')");
    modalHeader.innerText = eval(callParent('lcm[489]'));
    $("#cmdSend").prop("title", eval(callParent('lcm[490]'))).val(eval(callParent('lcm[490]')));
    $("#close").prop("title", eval(callParent('lcm[249]'))).val(eval(callParent('lcm[249]')));
    $('#filMyFile').bind({
        change: function () {
            var filename = $("#filMyFile").val();
            if (filename.indexOf("+") > -1) {
                $("#fileuploadsts").text("[FileName should not contain '+' character.]");
            }
            if (/^\s*$/.test(filename)) {
                $(".file-upload").removeClass('active');
                var cutMsg = eval(callParent('lcm[66]'));
                $("#noFile").text(cutMsg);
            }
            else {
                $(".file-upload").addClass('active');
                $("#noFile").text(filename.replace("C:\\fakepath\\", ""));
            }
            var uploadControl = $('#filMyFile')[0].files;
            if (uploadControl.length > 0)
                $("#lblnofilename").text(uploadControl[0].name);
            var fileExtension = ['xlsx'];
            var ext = filename.split('.').pop().toLowerCase();
            if ($.inArray(ext, fileExtension) > -1) {
                $('#fileuploadsts').val();
                $('#cmdSend').prop('disabled', false);
                $(".file-upload").addClass('active');
                $("#noFile").text(filename.replace("C:\\fakepath\\", ""));
            }
            else {
                $("#fileuploadsts").text("[" + eval(callParent('lcm[305]')) + "]");
                $('#cmdSend').prop('disabled', true);
                ResetFileUploadProperties();
            }
        }
    });

});


function DoClientFunction() {
    var upl = $j("#upsts");
    var succ = upl.val();
    if (succ == "Uploaded Successfully") {   
        parent.isAxLangUploaded = "true";
        setTimeout(function () { closeUploadDialog(); }, 300);
    }
    else {
        closeUploadDialog();
    }
}

$(document).keydown(function (e) {
    if (e.which == 27) {
        if ($(parent.$('.modal')) != undefined && $(parent.$('.modal')).length > 0) {
            $(parent).focus();
            if ($(parent.$('.modal .close')) != undefined && $(parent.$('.modal .close')).length > 0) {
                $(parent.$('.modal .close'))[$(parent.$('.modal .close')).length - 1].click();
            }
            setTimeout(function () {
                $("#btnModalClose").click();
                $(parent.$('.modal'))[$(parent.$('.modal')).length - 1].remove();
            }, 300);
        }
    }
})

function closeUploadDialog() {
    if ($(parent.$('.custom-dialog')) != undefined && $(parent.$('.custom-dialog')).length > 0) {
        $(parent).focus();
        if ($(parent.$('.custom-dialog .close')) != undefined && $(parent.$('.custom-dialog .close')).length > 0) {
            $(parent.$('.custom-dialog  .close'))[$(parent.$('.custom-dialog .close')).length - 1].click();
        }
        setTimeout(function () {
            //$(parent.$('.custom-dialog'))[$(parent.$('.custom-dialog')).length - 1].remove();
            parent.closeModalDialog();
        }, 300);
    }
}

function AllowAttachements() {
    var myFile = $('#filMyFile').val();
    if (myFile != null && myFile != "")
        return true;
    else
        return false;
}