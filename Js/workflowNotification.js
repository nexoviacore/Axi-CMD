var isPostSSO = false;
var isAppRoveSuccess = false;
function createModal() {
    if ($('#workflowComments').length > 0) {
        $('#workflowComments').modal('show');
        $("#comment").val(wfDefaultComm);
    }
    isPostSSO = true;
}

function loginPanel(isAuthorized) {
    if (isAppRoveSuccess) {
        //alert("popup hide ");
        setTimeout(function () {
            $('.lpanel').empty();
            $('#workflowComments').modal('hide');
        }, 0);
    } else {
        if (isAuthorized == "T") {
            $('.lpanel').css("display", "block");
        }
        else if (isAuthorized == "F") {
            //alert("popup show ");
            $('.lpanel').empty();
            $('#workflowComments').modal('show');
            $("#comment").val(wfDefaultComm);
        }
        if (typeof isiOS != "undefined" && isiOS) {
            let applnUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            let _isAppRoveSuccess = localStorage["isAppRoveSuccess-" + applnUrl];
            if (typeof _isAppRoveSuccess != "undefined" && _isAppRoveSuccess == "true") {
                localStorage.removeItem("isAppRoveSuccess-" + applnUrl);
                $('.lpanel').empty();
                $('#workflowComments').modal('hide');
                let wfamsg = localStorage["wfamsgIos-" + applnUrl];
                localStorage.removeItem("wfamsgIos-" + applnUrl);
                let msgtype = wfamsg.split('~');
                showAlertDialog(msgtype[0], msgtype[1], undefined, undefined, undefined, "True");
            }
        }
    }
}

function ShowDimmer(status) {
    DimmerCalled = true;
    var dv = $("#waitDiv");

    if (dv.length > 0 && dv != undefined) {
        if (status == true) {

            var currentfr = $("#middle1", parent.document);
            if (currentfr) {
                dv.width(currentfr.width());
            }
            dv.show();
            document.onkeydown = function EatKeyPress() { return false; }
        }
        else {
            dv.hide();
            document.onkeydown = function EatKeyPress() { if (DimmerCalled == true) { return true; } }
        }
    }
    else {
        //TODO:Needs to be tested
        if (window.opener != undefined) {

            dv = $("#waitDiv", window.opener.document);
            if (dv.length > 0) {
                if (status == true)
                    dv.show();
                else
                    dv.hide();
            }
        }
    }
    DimmerCalled = false;
}


$(document).ready(function () {
    if (typeof saveBtnName != "" && saveBtnName != "") {
        $("#commentSave").val(saveBtnName).attr("title", saveBtnName);
    }
    if (typeof $("#axpertMob") != "undefined") {
        $("#axpertMob").addClass("d-none");
        $("#axpertMob").hide();
    }
    if (isPostSSO == false) {
        try {
            GetWFAErrorMsg();
        } catch (ex) { }

        if (typeof axOktaSessionInit != "undefined" && typeof axOktaSessionValidate != "undefined" && typeof axOktaLogin != "undefined" && typeof oktadomain != "undefined" && oktadomain != "") {
            try {
                axOktaSessionInit();
            } catch (ex) { }
        }

        if ((typeof isOfficeSSO != "undefined" && isOfficeSSO == "") && typeof axOktaSessionInit != "undefined" && typeof axOktaSessionValidate != "undefined" && typeof axOktaLogin != "undefined" && typeof oktadomain != "undefined" && oktadomain != "") {
            try {
                axOktaSessionValidate();
            } catch (ex) { }
        }

        loginPanel($("#isAuthorized").val())
    }
    else {
        createModal();
        isPostSSO = false;
    }

    $('#workflowComments').on('hide.bs.modal', function (e) {
        if (e.target === this) { // Prevent only if clicking outside
            e.preventDefault();
        }
    });
});

function office365SsoFinalise(userid, username, proj) {
    if (typeof proj != "undefined") {
        $("#hdnAxProjs").val(proj);
    }
    let staySignedIn = "";
    try {
        staySignedIn = localStorage.getItem("staySignedIn");
        localStorage.removeItem("staySignedIn");
    }
    catch (ex) { }
    if (window.location.href.indexOf("?") > -1)
        window.location.href = window.location.href + "&ssotype=office365&ismobile=" + (isMobileDevice() == true ? "True" : "False") + "&code=" + (userid + "*$*" + username) + "&ssin=" + staySignedIn;
    else
        window.location.href = window.location.href + "?ssotype=office365&ismobile=" + (isMobileDevice() == true ? "True" : "False") + "&code=" + (userid + "*$*" + username) + "&ssin=" + staySignedIn;
    return false;
}

function oktaSsoFinalise(userid, username, proj) {
    if (typeof proj != "undefined") {
        $("#hdnAxProjs").val(proj);
    }
    let staySignedIn = "";
    try {
        staySignedIn = localStorage.getItem("staySignedIn");
        localStorage.removeItem("staySignedIn");
    }
    catch (ex) { }
    if (window.location.href.indexOf("?") > -1)
        window.location.href = window.location.href + "&ssotype=okta&ismobile=" + (isMobileDevice() == true ? "True" : "False") + "&code=" + (userid + "*$*" + username) + "&ssin=" + staySignedIn;
    else
        window.location.href = window.location.href + "?ssotype=okta&ismobile=" + (isMobileDevice() == true ? "True" : "False") + "&code=" + (userid + "*$*" + username) + "&ssin=" + staySignedIn;
    return false;
}

function Office365Init() {
    if (typeof signedin != "undefined")
        localStorage.setItem("staySignedIn", signedin.checked);
    else
        localStorage.setItem("staySignedIn", "false");
    axOffice365SessionInit();
    ssoLogin();
}

function resetActions() {
    var queries = {};
    $.each(document.location.search.substr(1).split('&'), function (c, q) {
        var i = q.split('=');
        queries[i[0].toString()] = i[1].toString();
    });
    if (typeof axOktaSessionInit != "undefined" && typeof axOktaLogOut != "undefined") {
        try {
            axOktaLogOut();
        } catch (ex) { }
    }
    window.location.href = window.location.href.split('?')[0] + "?enc=" + queries.enc;
}

function SetWFAErrorMsg(msgtype, msg) {
    let applnUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
    localStorage["wfamsg-" + applnUrl] = msgtype + "~" + msg;
    resetActions();
}

function GetWFAErrorMsg() {
    let applnUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
    let wfamsg = localStorage["wfamsg-" + applnUrl];
    if (localStorage["wfamsg-" + applnUrl])
        localStorage.removeItem("wfamsg-" + applnUrl);
    if (typeof wfamsg != "undefined" && wfamsg != "") {
        let msgtype = wfamsg.split('~');
        if (msgtype[0] == 'success') {
            $("#commentSave").addClass("disabled").prop("disabled", true);
            isAppRoveSuccess = true;
            if (typeof isiOS != "undefined" && isiOS) {
                localStorage["isAppRoveSuccess-" + applnUrl] = "true";
                localStorage["wfamsgIos-" + applnUrl] = wfamsg;
            }
            showAlertDialog(msgtype[0], msgtype[1], undefined, undefined, undefined, "True");
        } else
            showAlertDialog(msgtype[0], msgtype[1]);
    }
}

function CommentsValidate() {
    if ($("#comment").val() == "") {
        showAlertDialog("error", "Please enter comments.");
        return false;
    }
    else {
        $("#axUserName").removeAttr("required");
        $("#axPassword").removeAttr("required");
        return true;
    }
}

function wfchkNextForm() {
    let userTxtVal = $j("#axUserName").val();
    let userTxt = $j("#axUserName");
    if (userTxtVal == "") {
        showAlertDialog("error", 1010, "client");
        userTxt.val("");
        userTxt.focus();
        return false;
    }
    else if (!alphanumeric(userTxtVal)) {
        showAlertDialog("error", 1013, "client");
        userTxt.val("");
        userTxt.focus();
        return false;
    }
    else if ($j("#axPassword").val() == "") {
        showAlertDialog("error", 1011, "client");
        $j("#axPassword").focus();
        return false;
    }
    else {
        return true;
    }
}
