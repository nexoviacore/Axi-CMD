
$(document).ready(function () {
    $(".toggle-password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $($(this).attr("toggle"));
        if (input.attr("type") == "password") {
            input.attr("type", "text");
            $(this).text("visibility");
            $(this).css("color", "#999");
        } else {
            input.attr("type", "password");
            $(this).text("visibility_off");
            $(this).css("color", "#999");
        }
    });

    if (typeof hdnencryptkey.value != "undefined" && hdnencryptkey.value == "true") {
        if ($("#Label2").length > 0)
            $("#Label2").append("<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer\" title=\"Password should be Alphanumeric. Must contains one Upper Character, one Lower Character/Number with at least one special character.\">info</span>");
        if ($("#lblnewpwd").length > 0)
            $("#lblnewpwd").append("<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer\" title=\"Password should be Alphanumeric. Must contains one Upper Character, one Lower Character/Number with at least one special character.\">info</span>");
    } else if (typeof hdnalphanumeric.value != "undefined" && hdnalphanumeric.value == "True") {
        if ($("#Label2").length > 0)
            $("#Label2").append("<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer\" title=\"Password should be Alphanumeric. Must contains one Upper Character, one Lower Character/Number with at least one special character.\">info</span>");
        if ($("#lblnewpwd").length > 0)
            $("#lblnewpwd").append("<span class=\"material-icons material-icons-style align-middle material-icons-3 cursor-pointer\" title=\"Password should be Alphanumeric. Must contains one Upper Character, one Lower Character/Number with at least one special character.\">info</span>");
    }
    try {
        GetOtpErrorMsg();
    } catch (ex) { }
    checkSuccessAxpertMsg();
    setTimeout(setFocusThickboxIframe, 200);
    function setFocusThickboxIframe() {
        $("#existingPwd").focus();
    }
    $("#btnModalClose").hide();
    var chpdRemark = (new URL(location.href)).searchParams.get('remark');
    setTimeout(function () {
        if (cpwdRemark == "chpwd") {
            //to set modal dialog header dynamically based on language selection
            modalHeader = eval(callParent("divModalHeader", "id") + ".getElementById('divModalHeader')");
            modalHeader.innerText = eval(callParent('lcm[252]'));
            $("#btnClose").prop("title", eval(callParent('lcm[192]'))).text(eval(callParent('lcm[192]')));
            $("#main_body").css("background", "");
            $("#btnModalClose").addClass("d-none")
            //$("#iFrameChangePassword").height("366px");
            tabFocusEvent();//for focusing tab within the page
        }
        else {
            $("#spnCpwdHeading").text(eval(callParent('lcm[252]')));
            //$("#existingPwd").attr("placeholder", eval(callParent('lcm[295]')));
            //$("#newPwd").attr("placeholder", eval(callParent('lcm[296]')));
            //$("#confirmPwd").attr("placeholder", eval(callParent('lcm[297]')));
        }
        if (typeof hdnpwdPolicy.value != "undefined" && hdnpwdPolicy.value != "")
            $("#btnSumit").prop({ "title": eval(callParent('lcm[299]')), "value": eval(callParent('lcm[299]')) });
        else
            $("#btnSumit").prop({ "title": eval(callParent('lcm[200]')), "value": eval(callParent('lcm[200]')) });
    }, 100)

    $j("#breadcrumb-panel").css("display", "none");
    b();
    // ChangeTheme(window);
    // DefaultTheme();

    $("#confirmPwd").keypress(function (e) {
        if (e.which == 13) {
            $("#npwdHidden").val(this.value);
            $("#swc000F0").val(md5authNew(this.value));

        }
    });



    if ($j("#axpertVer").length > 0) {
        AddVerion();
    }

    setTimeout(function () {
        $("#existingPwd").focus();
    }, 500)
    $(".field-wrapper .field-placeholder").on("click", function () {
        $(this).closest(".field-wrapper").find("input").focus();
    });
    $(".field-wrapper input").on("click", function () {
        $(this).closest(".field-wrapper").addClass('hasValue')
    });

    $("#confirmPwd, #newPwd, #existingPwd").on('keyup', function () {
        var value = $.trim($(this).val());
        if (value) {
            $(this).closest(".field-wrapper").addClass("hasValue");
        } else {
            $(this).closest(".field-wrapper").removeClass("hasValue");
        }
        if ($j('#axSelectProj').is(':visible') && $j('#axSelectProj').val() != "")
            $j('#axSelectProj').closest(".field-wrapper").addClass("hasValue");
        if ($j('#language').is(':visible') && $j('#language').val() != "")
            $j('#language').closest(".field-wrapper").addClass("hasValue");
    });

    $(".field-wrapper input").each(function () {
        var value = $.trim($(this).val());
        if (value) {
            $(this).closest(".field-wrapper").addClass("hasValue");
        } else {
            $(this).closest(".field-wrapper").removeClass("hasValue");
        }
    });
    setProjectImages(proj);
});

function LoadParentHome() {
    url = window.parent.location;
    if (url.toString().toLowerCase().indexOf("cpwd.aspx") == -1)
        window.parent.loadHome();
}

//once password is changed successfully, close the dialog & display success msg, signout of the application after 3 sec
function closeDialog(result, url) {
    parent.$("#divModalChangePassword, #divModalChangePassword+.modal-backdrop").hide(); //hide modal dialog
    parent.showAlertDialog('success', result);
    let applnUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
    setTimeout(function () {
        setTimeout(function () {
            parent.window.location.href = applnUrl + url; //signout & redirect page to signin page
        }, 0);
    }, 200);
}

var myVersionJSON;
var versionInfoFile = "../versionInfo.json";
function AddVerion() {


    //Caching in local storage.
    try {
        if (typeof (Storage) !== "undefined") {
            if (localStorage["versionInfo-" + appUrl]) {
                var data = JSON.parse(localStorage["versionInfo-" + appUrl])
                if (Date.parse(data.expiry) > new Date()) {
                    myVersionJSON = data.value;
                } else {
                    getVersionDetails();
                    location.reload(true);
                }
            } else {
                getVersionDetails();
            }
        } else {
            getVersionDetails();
        }
    } catch (e) {
        getVersionDetails();
    }


    if (typeof myVersionJSON != "undefined") {
        setVersionInfo();
    }
}

function getVersionDetails() {
    $.ajax({
        url: versionInfoFile,
        type: "GET",
        statusCode: {
            404: function () {
                $.getJSON(versionInfoFile, function (json) {
                    //If File Dont Exist
                });
            }
        },
        success: function (json) {
            var expiry = addDays(new Date(), 0.3);
            var data = { value: json, expiry: expiry };
            try {
                if (typeof (Storage) !== "undefined") {
                    localStorage["versionInfo-" + appUrl] = JSON.stringify(data);
                }
            } catch (e) {
                //console.log("LocalStorage not Suported");
            }
            myVersionJSON = data.value;
            setVersionInfo();
        }
    });
}

function addDays(theDate, days) {
    return new Date(theDate.getTime() + days * 24 * 60 * 60 * 1000);
}

function setVersionInfo() {
    var currentVerion = "10.0.0.0";
    var subVersion = "";
    currentVerion = myVersionJSON.version;
    if (myVersionJSON.subVersion) {
        subVersion = "_" + myVersionJSON.subVersion.toString();
    }
    var finalVersion = "";
    finalVersion = "Version " + currentVerion + subVersion;
    $j("#axpertVer").text(finalVersion);
}

//for focusing tab within the page including Modal close icon
function tabFocusEvent() {
    try {
        modalButton = eval(callParent("btnModalClose", "id") + ".getElementById('btnModalClose')");
        if (modalButton.className.indexOf("firstFocusable") == -1)
            modalButton.className += " firstFocusable";
        $("#btnClose").addClass("lastFocusable").focus();
        $(".lastFocusable").on('keydown.tabRot', function (e) {
            if ((e.which === 9 && !e.shiftKey)) {
                e.preventDefault();
                modalButton.focus();
            }
        });
        modalButton.addEventListener('keydown', function (e) {
            if ((e.which === 9 && e.shiftKey)) {
                e.preventDefault();
                $(".lastFocusable").focus();
            }
        });
    } catch (ex) { }
}

function displayAlertMsgOnParent() {
    $(".shortMessageWrapper").appendTo(parent.$("#divModalChangePassword .modal-body"));
    setTimeout(function () {
        parent.hideAlertDialog("");
    }, 3000)
    if (typeof isMobile != "undefined" && isMobile) {
        setTimeout(function () {
            hideKeyboard();
        }, 0);
    }
}

function hideKeyboard() {
    document.activeElement.blur();
}

function setProjectImages(proj) {
    let logoImage = `../images/loginlogo.png`;
    let webBgImage = `../AxpImages/login-img.png`;
    let mobBgImage = `../AxpImages/login-img.png`;

    let logoImageDiv = $('.form-title img');
    let webBgImageDiv = $("body");
    let mobBgImageDiv = $("body");

    if (typeof cpwdRemark != "undefined" && cpwdRemark != "chpwd") {
        if (proj) {
            getProjectAppLogo(proj, async = true,
                (success) => {
                    if (success?.d) {
                        let { logo, webbg, mobbg } = JSON.parse(success.d);

                        if (webbg && !mobbg) {
                            mobbg = webbg
                        }

                        logoImageDiv.prop("src", logo ? `${logo}?v=${(new Date().getTime())}` : logoImage);
                        if (!isMobile) {
                            webBgImageDiv.css("background", `url(${webbg ? (`${webbg}?v=${(new Date().getTime())}`) : webBgImage}) ${webbg ? `no-repeat center center fixed` : `no-repeat fixed bottom`}`).css("background-size", "cover");
                        } else {
                            mobBgImageDiv.css("background", `url(${mobbg ? (`${mobbg}?v=${(new Date().getTime())}`) : mobBgImage}?v=${(new Date().getTime())}) ${mobbg ? `no-repeat center center fixed` : `no-repeat fixed bottom`}`).css("background-size", "cover");
                        }
                    } else {
                        logoImageDiv.prop("src", logoImage);
                        if (!isMobile) {
                            webBgImageDiv.css("background", `url(${webBgImage}) no-repeat fixed bottom`).css("background-size", "cover");
                        } else {
                            mobBgImageDiv.css("background", `url(${mobBgImage}?v=${(new Date().getTime())}) no-repeat fixed bottom `).css("background-size", "cover");
                        }
                    }
                },
                (error) => {
                    logoImageDiv.prop("src", logoImage);
                    if (!isMobile) {
                        webBgImageDiv.css("background", `url(${webBgImage}) no-repeat fixed bottom`).css("background-size", "cover");
                    } else {
                        mobBgImageDiv.css("background", `url(${mobBgImage}?v=${(new Date().getTime())}) no-repeat fixed bottom`).css("background-size", "cover");
                    }
                }
            );
        } else {
            logoImageDiv.prop("src", logoImage);
            if (!isMobile) {
                webBgImageDiv.css("background", `url(${webBgImage}) no-repeat fixed bottom`).css("background-size", "cover");
            } else {
                mobBgImageDiv.css("background", `url(${mobBgImage}?v=${(new Date().getTime())}) no-repeat fixed bottom`).css("background-size", "cover");
            }
        }
    }
}

function chngPwdOtpExpires(_exptime, _otpsuccmsg) {
    $("#dvChngPwd").addClass("d-none");
    _isOtpAuth = true;
    function displayExpiryTime(minutes) {
        const expiryTime = new Date(Date.now() + minutes * 60000); // Calculate the expiry time
        const expiryInputElement = document.getElementById("lblotpexpiry");
        function updateRemainingTime() {
            const currentTime = new Date();
            const remainingTimeInSeconds = Math.floor((expiryTime - currentTime) / 1000); // Calculate remaining time in seconds

            if (remainingTimeInSeconds > 0) {
                const remainingMinutes = Math.floor(remainingTimeInSeconds / 60); // Calculate remaining minutes
                let min = remainingMinutes < 10 ? '0' + remainingMinutes : remainingMinutes;
                let remsec = remainingTimeInSeconds - (remainingMinutes * 60);
                let sec = remsec < 10 ? '0' + remsec : remsec;
                if (_otpsuccmsg != '')
                    expiryInputElement.textContent = `OTP will expire in ${min}:${sec} min ( ${_otpsuccmsg})`; // Update the input field with remaining minutes
                else
                    expiryInputElement.textContent = `OTP will expire in ${min}:${sec} min (Sent to your registered email)`; // Update the input field with remaining minutes
            } else {
                clearInterval(intervalID); // Clear the interval when the OTP expires
                if (_otpsuccmsg != '')
                    expiryInputElement.textContent = `OTP has expired ( ${_otpsuccmsg})`; // Display "Expired" after the expiry time
                else
                    expiryInputElement.textContent = 'OTP has expired (Sent to your registered email)'; // Display "Expired" after the expiry time
                $("#btnResendotp").removeClass('d-none');
            }
        }
        updateRemainingTime();
        const intervalID = setInterval(updateRemainingTime, 1000);
    }
    const otpExpiryMinutes = _exptime; // Change this to your desired OTP expiry duration in minutes
    displayExpiryTime(otpExpiryMinutes);
}

function backToPwdDiv() {
    window.document.location.href = window.document.location.href;
}

function chnPwdoptauth() {
    if ($j("#axOTPpwd").val() == "") {
        showAlertDialog("error", callParentNew('lcm')[530]);
        $j("#axOTPpwd").focus();
        return false;
    } else {
        $.ajax({
            type: "POST",
            url: "cpwd.aspx/chkOtppwd",
            data: '{stsotpauth: "' + $j("#axOTPpwd").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d == "success")
                    $("#btnOTPpwdChange").click();
                else if (response.d == "InvalidOTP")
                    showAlertDialog("error", "Invalid OTP");
                else if (response.d == "OTPexpired")
                    showAlertDialog("error", "OTP expired");
                else if (response.d == "PleaseenterOTP")
                    showAlertDialog("error", "Please enter OTP");
                else if (response.d == "InvalidUser")
                    showAlertDialog("error", "Invalid User");
            },
            failure: function (response) {
                showAlertDialog("error", response.d);
            }
        });
    }
}

function btnResendOTP() {
    try {
        $("#axOTPpwd").attr("required", false);
        $("#btnResendOtp").click();
    } catch (ex) { }
}

function SetOtpErrorMsg(msg) {
    let applnUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
    localStorage["lnmsgpwdchng-" + applnUrl] = msg;
    if (typeof cpwdRemark != "undefined" && cpwdRemark == "chpwd")
        window.location.href = window.location.href;
    else
        top.window.location.href = top.window.location.href;
}

function GetOtpErrorMsg() {
    let applnUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
    let lnmsg = localStorage["lnmsgpwdchng-" + applnUrl];
    if (localStorage["lnmsgpwdchng-" + applnUrl])
        localStorage.removeItem("lnmsgpwdchng-" + applnUrl);
    if (typeof lnmsg != "undefined" && lnmsg != "") {        
        setTimeout(function () {
            if (typeof cpwdRemark != "undefined" && cpwdRemark != "chpwd")
                $("#toast-container").remove();
            showAlertDialog('error', lnmsg);
        }, 100);
    }
}