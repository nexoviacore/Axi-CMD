var applstJson = "";
let handlerType = "";
var redislstJson = "";
var isFile = "false";
var isArm = "false";
$j(document).ready(function () {
    $("#lstconnection").select2();
    $("#lstRconnection").select2();
    $("#ddldbtype").select2();
    $("#selLicDomain").select2();
    // $(".axSelectProj").select2();
    $("#ddldbversion").select2();
    $("#lstStudioRconnection").select2();
    $('#axpDevOptions').select2();
    /*    $("#lstconnection").val(selProj).trigger('change');*/

    $("#btnaddcon").click(function () {
        $("#lstconnection").val("").trigger('change');
        $("#ddldbtype").val("").trigger('change');
        $("#ddldbversion").val("").trigger('change');

    })
    //$("#axSelectProj").select2().select2('val',localStorageProj);

    if (IsLicExist == "false") {
        //$("#btnRefresh").attr('disabled', 'disabled').addClass("disabledButton");
        //$("#btnActivate").removeAttr("disabled").removeClass("disabledButton");
        //$("#btnTrial").removeAttr("disabled").removeClass("disabledButton");
        $("#dvlicmessage").show();
        $("#btnRefresh").hide();
        $("#btnActivate").show();
        $("#btnTrial").show();
        $(".licActOptions").show();
        $("#dvdbConnection").addClass("disabledDiv");
    }
    else {
        $("#dvlicmessage").hide();
        $(".licActOptions").hide();
        $("#btnRefresh").show();
        $("#btnActivate").hide();
        $("#btnTrial").hide();
        $("#dvdbConnection").removeClass("disabledDiv");
    }

    let selLicDomain = $("#selLicDomain option:first").val();
    if (typeof selLicDomain != "undefined") {
        if (appSettingList && typeof appSettingList === "string") {
            appSettingList = JSON.parse(appSettingList);
        }
        $("#hdnLicDomainProj").val(selLicDomain);
        if (appSettingList && appSettingList.appsettings) {
            if (appSettingList.appsettings.hasOwnProperty(selLicDomain) && typeof appSettingList.appsettings[selLicDomain].licdomain != "undefined") {
                let projData = appSettingList.appsettings[selLicDomain];
                document.getElementById("txtLicensedDomain").value = projData.licdomain.domainname || "";
                $('#selLicDomain').val(selLicDomain);
                $('#selLicDomain').trigger('change');
            }
        }
    }

    let fstRValue = $("#lstRconnection option:first").val();
    if (typeof fstRValue != "undefined") {
        if (appSettingList && typeof appSettingList === "string") {
            appSettingList = JSON.parse(appSettingList);
        }
        if (appSettingList && appSettingList.appsettings) {
            if (appSettingList.appsettings.hasOwnProperty(fstRValue) && typeof appSettingList.appsettings[fstRValue].Redis != "undefined") {
                let projData = appSettingList.appsettings[fstRValue];
                document.getElementById("txtrhotname").value = projData.Redis.host || "";
                document.getElementById("txtrport").value = projData.Redis.port || "";
                document.getElementById("txtrpwd").value = "";
                $('#lstRconnection').val(fstRValue);
                $('#lstRconnection').trigger('change');
            }
        }
    }


    let axStudioRValue = $("#lstStudioRconnection option:first").val();
    if (typeof axStudioRValue != "undefined") {
        if (appSettingList && typeof appSettingList === "string") {
            appSettingList = JSON.parse(appSettingList);
        }
        if (appSettingList && appSettingList.appsettings) {
            if (appSettingList.appsettings.hasOwnProperty(axStudioRValue) && typeof appSettingList.appsettings[axStudioRValue].AxStudioRedis != "undefined") {
                let projData = appSettingList.appsettings[axStudioRValue];
                document.getElementById("txtStudioUrl").value = projData.AxStudioRedis.studiourl || "";
                document.getElementById("txtrhotnameStudio").value = projData.AxStudioRedis.host || "";
                document.getElementById("txtrportStudio").value = projData.AxStudioRedis.port || "";
                document.getElementById("txtrpwdStudio").value = "";
                $('#lstStudioRconnection').val(axStudioRValue);
                $('#lstStudioRconnection').trigger('change');
            }
        }
    }

    // $("#btndelete").hide(); // hide class is added in asp button    
    $("#ddldbversion").val(version).prop("disabled", true);
    applist = ReverseCheckSpecialChars(applist);
    if (applist != "") {
        var xml = parseXml(applist)
        applstJson = JSON.parse(xml2json(xml, ""));
        if (selProj != "" && applstJson != "") {
            $("#lstconnection").val(selProj).focus();
            // setProjectImages(selProj);
            var db = applstJson.connections[selProj].db;
            var dbcon = applstJson.connections[selProj].dbcon;
            var dbuser = applstJson.connections[selProj].dbuser;
            var version = applstJson.connections[selProj].version == null ? "" : applstJson.connections[selProj].version;
            var driver = applstJson.connections[selProj].driver;

            $("#lblconname").text(selProj);
            $("#ddldbtype").val(db).trigger('change');
            if (db.toString() != "MS SQL")
                $(".databasever").addClass("d-none");
            // $("#ddldbtype").val(db);
            if (version != "")
                $("#ddldbversion").val(version).prop("disabled", false);
            else
                $("#ddldbversion").val(version).prop("disabled", true);
            if ((db.toString().toLowerCase() == "postgre" || db.toString().toLowerCase() == "postgresql") && driver.toLowerCase() == "ado") {
                $("#txtpostgresodbc").prop("checked", true);
                $("#dbtxtusername").addClass("d-none");
                $("#dbtxtPassword").addClass("d-none");
                $("#lblccname").addClass("d-none");
                $("#lblccnameodbc").removeClass("d-none");
            }
            $("#txtccname").val(dbcon);
            $("#txtusername").val(dbuser);
            $("#ddldriver").val(driver);
            if (driver == "dbx")
                $("#ddldriver").prop('selectedIndex', 0);
            else
                $("#ddldriver").prop('selectedIndex', 1);
        }
        else if (applstJson != "") {
            var fstValue = $("#lstconnection option:first").val();
            if (typeof fstValue != "undefined") {
                //$("#lstconnection").val(fstValue).focus();
                // setProjectImages(fstValue);
                var db = applstJson.connections[fstValue].db;
                var dbcon = applstJson.connections[fstValue].dbcon;
                var dbuser = applstJson.connections[fstValue].dbuser;
                var version = applstJson.connections[fstValue].version == null ? "" : applstJson.connections[fstValue].version;
                var driver = applstJson.connections[fstValue].driver;

                $("#lblconname").text(fstValue);
                $("#ddldbtype").val(db).trigger('change');
                // $("#ddldbtype").val(db);
                if (version != "")
                    $("#ddldbversion").val(version).prop("disabled", false);
                else
                    $("#ddldbversion").val(version).prop("disabled", true);
                if ((db.toString().toLowerCase() == "postgre" || db.toString().toLowerCase() == "postgresql") && driver.toLowerCase() == "ado") {
                    $("#txtpostgresodbc").prop("checked", true);
                    $("#dbtxtusername").addClass("d-none");
                    $("#dbtxtPassword").addClass("d-none");
                    $("#lblccname").addClass("d-none");
                    $("#lblccnameodbc").removeClass("d-none");
                }
                $("#txtccname").val(dbcon);
                $("#txtusername").val(dbuser);
                $("#ddldriver").val(driver);
                if (driver == "dbx")
                    $("#ddldriver").prop('selectedIndex', 0);
                else
                    $("#ddldriver").prop('selectedIndex', 1);
                $('#lstconnection').val(fstValue);
                $('#lstconnection').trigger('change');

            }
        }
    }
    if (applstJson != "") {
        var fstValuearm = $("#armproj option:first").val();
        if (typeof fstValuearm != "undefined") {
            $("#armproj").val(fstValuearm);//.focus();
            $('#armproj').trigger('change');
            //var dbuserarm = applstJson.connections[fstValuearm].dbuser;
            //var proj = dbuserarm;
            var proj = fstValuearm;
            if (proj.indexOf("\\") != -1)
                proj = proj.split("\\")[0];
            $("#hdnprojarm").val(proj);
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            if (appSettingList && appSettingList.appsettings) {
                if (appSettingList.appsettings.hasOwnProperty(projfile) && typeof appSettingList.appsettings[projfile].InternalResources != "undefined") {
                    let projData = appSettingList.appsettings[projfile];
                    document.getElementById("txtRMQAPIURL").value = projData.InternalResources.AxRMQAPIURL || "";
                    document.getElementById("txtSignalRapiURL").value = projData.InternalResources.AxSignalRapiURL || "";
                    document.getElementById("txtFCMSendMsgURL").value = projData.InternalResources.AxFCMSendMsgURL || "";
                    document.getElementById("txtRapidSaveURL").value = projData.InternalResources.AxRapidSaveURL || "";
                    document.getElementById("txtpegemailactionurl").value = projData.InternalResources.axpegemailactionurl || "";
                    document.getElementById("txtScriptsAPIURL").value = projData.InternalResources.AxScriptsAPIURL || "";
                }

                if (appSettingList.appsettings.hasOwnProperty(projfile) && typeof appSettingList.appsettings[projfile].ARM != "undefined") {
                    let projData = appSettingList.appsettings[projfile];
                    document.getElementById("txtarmkey").value = projData.ARM.ARM_PrivateKey || "";
                    document.getElementById("txtarmurl").value = projData.ARM.ARM_URL || "";
                    document.getElementById("txtscripturl").value = projData.ARM.ARM_Scripts_URL || "";
                    document.getElementById("txtpeg").value = projData.ARM.PEG || "";

                    document.getElementById("txtArmExpiryMinutes").value = projData.ARM.ExpiryMinutes || "";

                    document.getElementById("txtNotificationURL").value = projData.ARM.ARM_Notification_URL || "";
                    document.getElementById("txtNotificationExpiryHours").value = projData.ARM.ARM_Notification_ExpiryHours || "12";
                    document.getElementById("txtNotificationMaxPerUser").value = projData.ARM.ARM_Notification_MaxPerUser || "10";

                    $('#txtpeg').trigger('change');

                    $('#txtNotificationExpiryHours').trigger('change');
                    $('#txtNotificationMaxPerUser').trigger('change');
                }
            }
        }
    }

    if (applstJson != "") {
        if (appSettingList && typeof appSettingList === "string") {
            appSettingList = JSON.parse(appSettingList);
        }
        if (appSettingList && appSettingList.appsettings) {
            document.getElementById("txtRmQueueHost").value = appSettingList.rmqsettings.RMQueueHost || "";
            document.getElementById("txtRmQueuePort").value = appSettingList.rmqsettings.RMQueuePort || "";
            document.getElementById("txtrmqueueuser").value = appSettingList.rmqsettings.RMQueueUser || "";
            document.getElementById("txtrmqueuepwd").value = appSettingList.rmqsettings.RMQueuePassword || "";
        }
    }



    if (applstJson != "") {
        var extResValuearm = $("#armExtResource option:first").val();
        if (typeof extResValuearm != "undefined") {
            $("#armExtResource").val(extResValuearm);//.focus();
            $('#armExtResource').trigger('change');
            var proj = extResValuearm;
            if (proj.indexOf("\\") != -1)
                proj = proj.split("\\")[0];
            $("#hdnarmExtResource").val(proj);
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            if (appSettingList && appSettingList.appsettings) {
                if (appSettingList.appsettings.hasOwnProperty(projfile) && appSettingList.appsettings[projfile].ExternalResources) {
                    let ext = appSettingList.appsettings[projfile].ExternalResources;
                    if (ext.NamedURLS && Object.keys(ext.NamedURLS).length > 0) {
                        renderNamedItems("#namedUrlContainer", ext.NamedURLS, "URLS");
                    } else {
                        $j("#namedUrlContainer").empty();
                    }
                    if (ext.NamedSFTP && Object.keys(ext.NamedSFTP).length > 0) {
                        renderNamedItems("#namedSFTPContainer", ext.NamedSFTP, "SFTP");
                    } else {
                        $j("#namedSFTPContainer").empty();
                    }
                    if (ext.NamedFileServer && Object.keys(ext.NamedFileServer).length > 0) {
                        renderNamedItems("#namedFileServerContainer", ext.NamedFileServer, "FileServer");
                    } else {
                        $j("#namedFileServerContainer").empty();
                    }

                    $('#armExtResource').trigger('change');
                }
                else {
                    $j("#namedUrlContainer").empty();
                    $j("#namedSFTPContainer").empty();
                    $j("#namedFileServerContainer").empty();
                }
            }
        }
    }

    if (applstJson != "") {
        var axpDevOptions = $("#axpDevOptions option:first").val();
        if (typeof axpDevOptions != "undefined") {
            $("#axpDevOptions").val(axpDevOptions);//.focus();
            $('#axpDevOptions').trigger('change');
            let _proj = extResValuearm;
            if (_proj.indexOf("\\") != -1)
                _proj = _proj.split("\\")[0];
            $("#hdnaxpDevOptions").val(_proj);
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            if (appSettingList && appSettingList.appsettings) {
                if (appSettingList.appsettings.hasOwnProperty(_proj) && appSettingList.appsettings[_proj].AxpDevOptsMenu) {
                    let savedValue = appSettingList.appsettings[_proj].AxpDevOptsMenu.Options || "";
                    var $ddl = $('#ddlAxpertdevOptions');
                    if (savedValue === "all") {
                        var allVals = [];
                        $ddl.find('option').each(function () {
                            if ($(this).val() !== "all")
                                allVals.push($(this).val());
                        });
                        $ddl.val(allVals).trigger('change');
                    }
                    else if (savedValue !== "") {
                        $ddl.val(savedValue.split(',')).trigger('change');
                    }
                    else {
                        $ddl.val(null).trigger('change');
                    }
                    $('#axpDevOptions').trigger('change');
                }
            }
        }
    }

    if (applstJson != "") {
        var fstValuefile = $("#fileproj option:first").val();
        if (typeof fstValuefile != "undefined") {
            $("#fileproj").val(fstValuefile);//.focus();
            $('#fileproj').trigger('change');
            var projfile = fstValuefile;
            if (projfile.indexOf("\\") != -1)
                projfile = projfile.split("\\")[0];
            $("#hdnfileproj").val(projfile);
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            //if (appSettingList != "") {
            //    if (appSettingList.hasOwnProperty(projfile) && typeof appSettingList[projfile].FileConfig != "undefined") {
            if (appSettingList && appSettingList.appsettings) {
                if (appSettingList.appsettings.hasOwnProperty(projfile) && typeof appSettingList.appsettings[projfile].FileConfig != "undefined") {
                    let projData = appSettingList.appsettings[projfile];
                    document.getElementById("txtfileupload").value = projData.FileConfig.FileUploadPath || "";
                    document.getElementById("txtfiledownload").value = projData.FileConfig.FileDownloadPath || "";
                    document.getElementById("txtfileMapUsername").value = projData.FileConfig.FileServerMapUsername || "";
                    document.getElementById("tstfileMapPwd").value = projData.FileConfig.FileServerMapPwd || "";
                    document.getElementById("ddlAttachmentSize").value = projData.FileConfig.AxAttachmentSize || "1";

                    $('#ddlAttachmentSize').trigger('change');
                }
            }
        }
    }

    $(document).on('click', ".upload-button", function (e) {
        try {
            if ($("#axSelectProj").val() == "") {
                showAlertDialog("warning", "Please Select UI Configuration Project");
                $("#axSelectProj").focus();
                e.preventDefault();
                e.stopPropagation();
                return false;
            }

            handlerType = "upload";
            $(e.currentTarget).nextAll(".file-upload").trigger("click");
        } catch (ex) { }
    });

    $(document).on('click', ".delete-button", function (e) {
        try {
            if ($("#axSelectProj").val() == "") {
                showAlertDialog("warning", "Please Select UI Configuration Project");
                $("#axSelectProj").focus();
                e.preventDefault();
                e.stopPropagation();
                return false;
            }

            handlerType = "delete";
            $(e.currentTarget).parent().find(".file-upload").trigger("change");
        } catch (ex) { }
    });

    $(document).on('change', ".file-upload", function (e) {
        uploadLogo(e);
    });

    $("#ddlIsNewConnection").val('new');
    $("#txtNewConName").val('');

    $("#ddlIsRedisNewConnection").val('new');
    $("#txtRedisNewConn").val('');

    $("#demo").on("hide.bs.collapse", function () {
        $(".licInfoAccordion").html('<span class="material-icons">expand_more</span>');
    });
    $("#demo").on("show.bs.collapse", function () {
        $(".licInfoAccordion").html('<span class="material-icons">expand_less</span>');
    });
    if (IsLicExist == "false") {
        $("#demo").addClass("in");
        $(".licInfoAccordion").html('<span class="material-icons">expand_less</span>');
    }


    $('#filMyFile').bind({
        change: function () {
            var filename = $("#filMyFile").val();
            if (/^\s*$/.test(filename)) {
                $(".file-upload").removeClass('active');
                $("#lblfuerror").text("No file chosen...");
            }
            else
                $(".file-upload").addClass('active');
            var fileExtension = ['lic'];
            var ext = filename.split('.').pop().toLowerCase();
            if ($.inArray(ext, fileExtension) == -1) {
                $('#btnFileUpload').prop('disabled', true);
                $("#lblfuerror").text("Invalid file format.");
            }
            else {
                $('#btnFileUpload').prop('disabled', false);
                $("#lblfuerror").text("");
            }
            var uploadControl = $('#filMyFile')[0].files;
            if (uploadControl.length > 0)
                $("#lblnofilename").text(uploadControl[0].name);
        }
    });

    if (authPopup == "true")
        $("#myModalAuth").show();

    initUiPanel();

    if (selProj != '') {
        try {
            $('#lstconnection').val(selProj);
            $("#lstconnection").trigger('change');
        } catch (ex) { }

        //try {
        //    $('#selLicDomain').val(selProj);
        //    $('#selLicDomain').trigger('change');
        //} catch (ex) { }

        //try {
        //    $('#lstRconnection').val(selProj);
        //    $('#lstRconnection').trigger('change');
        //} catch (ex) { }

        //try {
        //    $('#lstStudioRconnection').val(selProj);
        //    $('#lstStudioRconnection').trigger('change');
        //} catch (ex) { }

        //try {
        //    $('#armproj').val(selProj);
        //    $('#armproj').trigger('change');
        //} catch (ex) { }

        //try {
        //    $('#fileproj').val(selProj);
        //    $('#fileproj').trigger('change');
        //} catch (ex) { }
    }

    if (typeof strudioScriptUrlMesg != "undefined" && strudioScriptUrlMesg != "") {
        SuccStudioRedisConnection("info", strudioScriptUrlMesg);
        strudioScriptUrlMesg = "";
    }
});

$j(document).on("click", "#lstconnection option", function (e) {
    var lvalue = $(this).val();
    if (applstJson != "") {
        var db = applstJson.connections[lvalue].db;
        var dbcon = applstJson.connections[lvalue].dbcon;
        var dbuser = applstJson.connections[lvalue].dbuser;
        var version = applstJson.connections[lvalue].version == null ? "" : applstJson.connections[lvalue].version;
        var driver = applstJson.connections[lvalue].driver;

        $("#lblconname").text(lvalue);
        $("#ddldbtype").val(db);
        if (db != "")
            $("#ddldbtype").val(db).trigger('change');

        if ((db.toString().toLowerCase() == "postgre" || db.toString().toLowerCase() == "postgresql") && driver.toLowerCase() == "ado") {
            $("#txtpostgresodbc").prop("checked", true);
            $("#dbtxtusername").addClass("d-none");
            $("#dbtxtPassword").addClass("d-none");
            $("#lblccname").addClass("d-none");
            $("#lblccnameodbc").removeClass("d-none");
        }

        if (version != "") {
            $("#ddldbversion").val(version).prop("disabled", false);
            $("#ddldbversion").val(version).trigger('change');
        }
        else
            $("#ddldbversion").val(version).prop("disabled", true);
        $("#txtccname").val(dbcon);
        $("#txtusername").val(dbuser);
        $("#ddldriver").val(driver);
        if (driver == "dbx")
            $("#ddldriver").prop('selectedIndex', 0);
        else
            $("#ddldriver").prop('selectedIndex', 1);
    }
});

$j(document).on("change", "#lstconnection", function (e) {
    var lvalue = $(this).val();
    if (applstJson != "" && lvalue != null) {
        var db = applstJson.connections[lvalue].db;
        var dbcon = applstJson.connections[lvalue].dbcon;
        var dbuser = applstJson.connections[lvalue].dbuser;
        var version = applstJson.connections[lvalue].version == null ? "" : applstJson.connections[lvalue].version;
        var driver = applstJson.connections[lvalue].driver;

        $("#lblconname").text(lvalue);
        $("#ddldbtype").val(db);
        $('#ddldbtype').trigger('change');
        if (db != "")
            $("#ddldbtype").val(db).trigger('change');
        if (db.toString().toLowerCase() != "ms sql") {
            $("#ddldbversion").val("").trigger('change');
            $(".databasever").addClass("d-none");
        }//'MS SQL'
        else
            $(".databasever").removeClass("d-none");
        if (version != "") {
            $("#ddldbversion").val(version).prop("disabled", false);
            $("#ddldbversion").val(version).trigger('change');
        }
        else {
            $("#ddldbversion").val(version).prop("disabled", true);
        }
        if ((db.toString().toLowerCase() == "postgre" || db.toString().toLowerCase() == "postgresql") && driver.toLowerCase() == "ado") {
            $(".postgresodbc").removeClass('d-none');
            $("#txtpostgresodbc").prop("checked", true);
            $("#dbtxtusername").addClass("d-none");
            $("#dbtxtPassword").addClass("d-none");
            $("#lblccname").addClass("d-none");
            $("#lblccnameodbc").removeClass("d-none");
        } else if (db.toString().toLowerCase() == "postgre" || db.toString().toLowerCase() == "postgresql") {
            $(".postgresodbc").removeClass('d-none');
            $("#txtpostgresodbc").prop("checked", false);
        } else {
            $(".postgresodbc").addClass('d-none');
        }
        $("#txtccname").val(dbcon);
        $("#txtusername").val(dbuser);
        $("#ddldriver").val(driver);
        if (driver == "dbx")
            $("#ddldriver").prop('selectedIndex', 0);
        else
            $("#ddldriver").prop('selectedIndex', 1);
        // setProjectImages(lvalue);

        $('#selLicDomain').val(lvalue).trigger('change');

        $('#lstRconnection').val(lvalue).trigger('change');

        $('#lstStudioRconnection').val(lvalue).trigger('change');

        $('#armproj').val(lvalue).trigger('change');

        //$('#RMQueueproj').val(lvalue).trigger('change');

        $('#fileproj').val(lvalue).trigger('change');

        $('#armExtResource').val(lvalue).trigger('change');

        $('#axpDevOptions').val(lvalue).trigger('change');        
    }
});

$j(document).on("change", "#fileproj", function (e) {
    var lvalue = $(this).val();
    if (applstJson != "") {
        var proj = lvalue;
        if (proj.indexOf("\\") != -1)
            proj = proj.split("\\")[0];
        $("#hdnfileproj").val(proj);
        try {
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            if (appSettingList && appSettingList.appsettings) {
                isFile = "true"
                if (appSettingList.appsettings.hasOwnProperty(proj) && typeof appSettingList.appsettings[proj].FileConfig != "undefined") {
                    let projData = appSettingList.appsettings[proj];
                    document.getElementById("txtfileupload").value = projData.FileConfig.FileUploadPath || "";
                    document.getElementById("txtfiledownload").value = projData.FileConfig.FileDownloadPath || "";
                    document.getElementById("txtfileMapUsername").value = projData.FileConfig.FileServerMapUsername || "";
                    document.getElementById("tstfileMapPwd").value = projData.FileConfig.FileServerMapPwd || "";
                    document.getElementById("ddlAttachmentSize").value = projData.FileConfig.AxAttachmentSize || "1";
                    $('#ddlAttachmentSize').trigger('change');
                }
                else {
                    document.getElementById("txtfileupload").value = "";
                    document.getElementById("txtfiledownload").value = "";
                    document.getElementById("txtfileMapUsername").value = "";
                    document.getElementById("tstfileMapPwd").value = "";
                    document.getElementById("ddlAttachmentSize").value = "1";
                    $('#ddlAttachmentSize').trigger('change');
                }
            }
        }
        catch (ex) { }
    }
});

$j(document).on("change", "#armproj", function (e) {
    var lvalue = $(this).val();
    if (applstJson != "") {
        //var dbuser = applstJson.connections[lvalue].dbuser;
        //var proj = dbuser;
        var proj = lvalue;
        if (proj.indexOf("\\") != -1)
            proj = proj.split("\\")[0];
        $("#hdnprojarm").val(proj);
        try {
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            if (appSettingList && appSettingList.appsettings) {
                isArm = "true"
                if (appSettingList.appsettings.hasOwnProperty(proj) && typeof appSettingList.appsettings[proj].ARM != "undefined") {
                    let projData = appSettingList.appsettings[proj];
                    document.getElementById("txtarmkey").value = projData.ARM.ARM_PrivateKey || "";
                    document.getElementById("txtarmurl").value = projData.ARM.ARM_URL || "";
                    document.getElementById("txtscripturl").value = projData.ARM.ARM_Scripts_URL || "";
                    document.getElementById("txtpeg").value = projData.ARM.PEG || "";

                    document.getElementById("txtArmExpiryMinutes").value = projData.ARM.ExpiryMinutes || "";

                    document.getElementById("txtNotificationURL").value = projData.ARM.ARM_Notification_URL || "";
                    document.getElementById("txtNotificationExpiryHours").value = projData.ARM.ARM_Notification_ExpiryHours || "12";
                    document.getElementById("txtNotificationMaxPerUser").value = projData.ARM.ARM_Notification_MaxPerUser || "10";

                    if (appSettingList.appsettings.hasOwnProperty(proj) && typeof appSettingList.appsettings[proj].InternalResources != "undefined") {
                        document.getElementById("txtRMQAPIURL").value = projData.InternalResources.AxRMQAPIURL || "";
                        document.getElementById("txtSignalRapiURL").value = projData.InternalResources.AxSignalRapiURL || "";
                        document.getElementById("txtFCMSendMsgURL").value = projData.InternalResources.AxFCMSendMsgURL || "";
                        document.getElementById("txtRapidSaveURL").value = projData.InternalResources.AxRapidSaveURL || "";
                        document.getElementById("txtpegemailactionurl").value = projData.InternalResources.axpegemailactionurl || "";
                        document.getElementById("txtScriptsAPIURL").value = projData.InternalResources.AxScriptsAPIURL || "";
                    } else {
                        document.getElementById("txtRMQAPIURL").value = "";
                        document.getElementById("txtSignalRapiURL").value = "";
                        document.getElementById("txtFCMSendMsgURL").value = "";
                        document.getElementById("txtRapidSaveURL").value = "";
                        document.getElementById("txtpegemailactionurl").value = "";
                        document.getElementById("txtScriptsAPIURL").value = "";
                    }

                    $('#txtpeg').trigger('change');

                    $('#txtNotificationExpiryHours').trigger('change');
                    $('#txtNotificationMaxPerUser').trigger('change');
                }
                else {
                    document.getElementById("txtarmkey").value = "";
                    document.getElementById("txtarmurl").value = "";
                    document.getElementById("txtscripturl").value = "";
                    document.getElementById("txtpeg").value = "";

                    document.getElementById("txtArmExpiryMinutes").value = "";

                    document.getElementById("txtNotificationURL").value = "";
                    document.getElementById("txtNotificationExpiryHours").value = "12";
                    document.getElementById("txtNotificationMaxPerUser").value = "10";

                    document.getElementById("txtRMQAPIURL").value = "";
                    document.getElementById("txtSignalRapiURL").value = "";
                    document.getElementById("txtFCMSendMsgURL").value = "";
                    document.getElementById("txtRapidSaveURL").value = "";
                    document.getElementById("txtpegemailactionurl").value = "";
                    document.getElementById("txtScriptsAPIURL").value = "";

                    $('#txtpeg').trigger('change');

                    $('#txtNotificationExpiryHours').trigger('change');
                    $('#txtNotificationMaxPerUser').trigger('change');
                }
            }
        }
        catch (ex) { }
    }
});

$j(document).on("change", "#armExtResource", function (e) {
    var lvalue = $(this).val();
    if (applstJson != "") {
        var proj = lvalue;
        if (proj.indexOf("\\") != -1)
            proj = proj.split("\\")[0];
        $("#hdnarmExtResource").val(proj);
        try {
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            if (appSettingList && appSettingList.appsettings) {
                if (appSettingList.appsettings.hasOwnProperty(proj) && appSettingList.appsettings[proj].ExternalResources) {
                    let ext = appSettingList.appsettings[proj].ExternalResources;
                    if (ext.NamedURLS && Object.keys(ext.NamedURLS).length > 0) {
                        renderNamedItems("#namedUrlContainer", ext.NamedURLS, "URLS");
                    } else {
                        $j("#namedUrlContainer").empty();
                    }
                    if (ext.NamedSFTP && Object.keys(ext.NamedSFTP).length > 0) {
                        renderNamedItems("#namedSFTPContainer", ext.NamedSFTP, "SFTP");
                    } else {
                        $j("#namedSFTPContainer").empty();
                    }
                    if (ext.NamedFileServer && Object.keys(ext.NamedFileServer).length > 0) {
                        renderNamedItems("#namedFileServerContainer", ext.NamedFileServer, "FileServer");
                    } else {
                        $j("#namedFileServerContainer").empty();
                    }
                }
                else {
                    $j("#namedUrlContainer").empty();
                    $j("#namedSFTPContainer").empty();
                    $j("#namedFileServerContainer").empty();
                }
            }
        }
        catch (ex) { }
    }
});

$j(document).on("click", "#btnApply", function (e) {
    if ($("#lstconnection").val() == null || $("#lstconnection").val() == "") {
        $("#ddlIsNewConnection").val('new');
        $('#myModal').modal("show");
    }
    else {
        $("#ddlIsNewConnection").val('old');
        $("#txtNewConName").val($("#lstconnection").val());
        $("#btnok").click();
    }
});

$j(document).on("click", "#btnChangePassword", function (e) {
    $('#myModaldbpassword').modal("show");
});

$j(document).on("click", "#btnCancel", function (e) {
    window.document.location.href = "signin.aspx";
});

$j(document).on("click", "#btnaddcon", function (e) {
    $("#ddldbtype").prop('selectedIndex', 0);
    $("#ddldbversion").prop('selectedIndex', 0).prop("disabled", true);
    $(".databasever").removeClass("d-none");
    $("#ddldriver").prop('selectedIndex', 0);
    $("#lblconname").text("");
    $("#txtccname").val("");
    $("#txtusername").val("");
    $("#lstconnection").val("");
    $("#txtPassword").val("");
    $("#txtNewPassword").val("");
    $("#txtConfirmPassword").val("");
    $("#txtpostgresodbc").prop("checked", false);
});

$j(document).on("click", "#btncdelete", function (e) {
    var conDelete = $.confirm({
        closeIcon: false,
        title: 'Confirm',
        escapeKey: 'buttonB',
        theme: 'modern',
        onContentReady: function () {
            disableBackDrop('bind');
        },
        content: eval(callParent('lcm[512]')),
        buttons: {
            buttonA: {
                text: eval(callParent('lcm[279]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    $j("#btndelete").click();
                }
            },
            buttonB: {
                text: eval(callParent('lcm[280]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                }
            }
        }
    });
})

$j(document).on("change", "#ddldbtype", function (e) {
    var dbtype = $(this).val();
    if (dbtype == "MS SQL") {
        $("#ddldbversion").prop("disabled", false);
        $("#ddldriver").val("ado");
        $("#ddldriver").prop('selectedIndex', 1);
    }
    else {
        $("#ddldbversion").val("").prop("disabled", true);
        $("#ddldriver").val("dbx");
        $("#ddldriver").prop('selectedIndex', 0);
    }

    if ((dbtype.toLowerCase() == "postgre" || dbtype.toLowerCase() == "postgresql") && $("#ddldriver").val() == "ado") {
        $(".postgresodbc").removeClass('d-none');
        $("#txtpostgresodbc").prop("checked", true);
        $("#dbtxtusername").addClass("d-none");
        $("#dbtxtPassword").addClass("d-none");
        $("#lblccname").addClass('d-none');
        $("#lblccnameodbc").removeClass('d-none');
    } else if (dbtype.toLowerCase() == "postgre" || dbtype.toLowerCase() == "postgresql") {
        $(".postgresodbc").removeClass('d-none');
        $("#txtpostgresodbc").prop("checked", false);
        $("#dbtxtusername").removeClass("d-none");
        $("#dbtxtPassword").removeClass("d-none");
        $("#lblccname").removeClass('d-none');
        $("#lblccnameodbc").addClass('d-none');
    } else {
        $(".postgresodbc").addClass('d-none');
        $("#dbtxtusername").removeClass("d-none");
        $("#dbtxtPassword").removeClass("d-none");
        $("#lblccname").removeClass('d-none');
        $("#lblccnameodbc").addClass('d-none');
    }
});

$j(document).on("change", "#txtpostgresodbc", function (e) {
    if ($(this).is(':checked')) {
        $("#dbtxtusername").addClass("d-none");
        $("#dbtxtPassword").addClass("d-none");
        $("#lblccname").addClass("d-none");
        $("#lblccnameodbc").removeClass("d-none");
    } else {
        $("#dbtxtusername").removeClass("d-none");
        $("#dbtxtPassword").removeClass("d-none");
        $("#lblccname").removeClass("d-none");
        $("#lblccnameodbc").addClass("d-none");
    }
    $("#btnApply").attr("disabled", true).addClass("btndisable");
    $("#btnChangePassword").attr("disabled", true).addClass("btndisable");
});

function OpenSignIn() {
    window.document.location.href = "Signin.aspx";
}

$j(document).on("click", "#btnlicactivation", function (e) {

    //displayBootstrapModalDialog("License Activation", "xs", "156px", true, "./licactivation.aspx", "", "", CallbackFunctionBootstrap)

    //function CallbackFunctionBootstrap() {

    //}
    window.document.location.href = "licactivation.aspx";
});

function TestConnection() {
    let dbtype = $("#ddldbtype").val();
    if (dbtype == "") {
        $("#ddldbtype").focus();
        showAlertDialog("error", "Database name should not be left empty.");
        return false;
    }
    let dbverno = $("#ddldbversion").val();
    if (dbtype.toLowerCase() == "ms sql" && dbverno == "") {
        $("#ddldbversion").focus();
        showAlertDialog("error", "Database verion should not be left empty.");
        return false;
    }
    let isPostgreOdbc = false;
    if (dbtype.toLowerCase() == "postgre" || dbtype.toLowerCase() == "postgresql") {
        if ($("#txtpostgresodbc").is(':checked')) {
            isPostgreOdbc = true;
            $("#ddldriver").val("ado");
            $("#ddldriver").prop('selectedIndex', 1);
        } else {
            isPostgreOdbc = false;
            $("#ddldriver").val("dbx");
            $("#ddldriver").prop('selectedIndex', 0);
        }
    }

    let txtccname = $("#txtccname").val();
    if (txtccname == "") {
        $("#txtccname").focus();
        showAlertDialog("error", "Connection name should not be left empty.");
        return false;
    }

    if (!isPostgreOdbc) {
        let txtusername = $("#txtusername").val();
        if (txtusername == "") {
            $("#txtusername").focus();
            showAlertDialog("error", "User name should not be left empty.");
            return false;
        }

        let txtPassword = $("#txtPassword").val();
        if (txtPassword == "") {
            $("#txtPassword").focus();
            showAlertDialog("error", "Password should not be left empty.");
            return false;
        }
    }
    TestConnectionWs();
    // return true;
}

function TestConnectionWs() {
    let _txtpostgresodbc = "false";
    if ($("#txtpostgresodbc").is(':checked'))
        _txtpostgresodbc = "true";
    $.ajax({
        url: 'AxpertAdmin.aspx/AppTestConnection',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            ddldbtype: $("#ddldbtype").val(), ddldbversion: $("#ddldbversion").val(), ddldriver: $("#ddldriver").val(), txtccname: $("#txtccname").val(), txtusername: $("#txtusername").val(), txtPassword: $("#txtPassword").val(), txtpostgresodbc: _txtpostgresodbc,
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                var response = $.parseJSON(data.d)
                if (response.result[0].result == "true") {
                    showAlertDialog("success", "Test Connection was Successful.");
                    EnableApplyConnection();
                }
                else
                    showAlertDialog("error", response.result[0].error.msg);
            }
            else
                showAlertDialog("error", data.d);
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function EnableApplyConnection() {
    $("#btnApply").removeAttr("disabled").removeClass("btndisable");
    $("#btnChangePassword").removeAttr("disabled").removeClass("btndisable");
}


function applyconnection() {
    let txtNewConName = $("#txtNewConName").val();
    if (txtNewConName == "") {
        $("#txtNewConName").focus();
        showAlertDialog("error", "Connection name should not be left empty.");
        return false;
    }
    var regex = new RegExp("^[a-zA-Z0-9_]+$");
    if (!regex.test(txtNewConName)) {
        $("#txtNewConName").focus();
        showAlertDialog("error", "Invalid Name - Special characters not allowed.");
        return false;
    }
    return true;
}

function applydbpwdconnection() {
    let txtNewConName = $("#txtNewPassword").val();
    if (txtNewConName == "") {
        $("#txtNewPassword").focus();
        showAlertDialog("error", "Password can not be empty.");
        return false;
    }
    let txtConfirmPassword = $("#txtConfirmPassword").val();
    if (txtConfirmPassword == "") {
        $("#txtConfirmPassword").focus();
        showAlertDialog("error", "Confirm Password can not be empty.");
        return false;
    }

    if (txtNewConName != txtConfirmPassword) {
        $("#txtNewPassword").focus();
        showAlertDialog("error", "The password you entered did not match.");
        return false;
    }
    return true;
}

$j(document).on("click", "#btnnewcancel,#myModalclose", function (e) {
    $('#myModal').hide();
    $("#btnApply").attr("disabled", true).addClass("btndisable");
    $("#btnChangePassword").attr("disabled", true).addClass("btndisable");
});

$j(document).on("click", "#btndbpwdcancel,#myModaldbpasswordclose", function (e) {
    $('#myModaldbpassword').hide();
    $("#btnApply").attr("disabled", true).addClass("btndisable");
    $("#btnChangePassword").attr("disabled", true).addClass("btndisable");
});

function SuccApplyConnection(isSucc, newConnection) {
    if (isSucc == "success") {
        showAlertDialog("success", "Connection is created successful.");
    }
    else {
        showAlertDialog("error", "Connection Name already exists.");
    }
}

function SuccPasswordChange(isSucc, succMsg) {
    if (isSucc == "success") {
        showAlertDialog("success", succMsg);
    }
    else {
        showAlertDialog("error", succMsg);
    }
}

function setProjectImages(proj) {
    let logoImage = `../images/loginlogo.png`;
    let webBgImage = `../AxpImages/login-img.png`;
    let mobBgImage = `../AxpImages/login-img.png`;

    let logoImageDiv = $(".file-upload[data-type=logo]").parent().parent().find(".profile-pic");
    let webBgImageDiv = $(".file-upload[data-type=webbg]").parent().parent().find(".profile-pic");
    let mobBgImageDiv = $(".file-upload[data-type=mobbg]").parent().parent().find(".profile-pic")//$(".file-upload[data-type=mobbg]").prevAll(".profile-pic");

    if (proj) {
        getProjectAppLogo(proj, async = true,
            (success) => {
                if (success?.d) {
                    let { logo, webbg, mobbg } = JSON.parse(success.d);

                    let updateMobileBG = false;

                    if (webbg && !mobbg) {
                        mobbg = webbg;
                        updateMobileBG = true;
                    }

                    logoImageDiv.prop("src", logo ? `${logo}?v=${(new Date().getTime())}` : logoImage);
                    webBgImageDiv.prop("src", webbg ? webbg : webBgImage);
                    mobBgImageDiv.prop("src", mobbg ? mobbg : mobBgImage);

                    if (logo) {
                        logoImageDiv.parent().find(".delete-button").removeClass("d-none");
                    } else {
                        logoImageDiv.parent().find(".delete-button").addClass("d-none");
                    }

                    if (webbg) {
                        webBgImageDiv.parent().find(".delete-button").removeClass("d-none");
                    } else {
                        webBgImageDiv.parent().find(".delete-button").addClass("d-none");
                    }

                    if (mobbg && !updateMobileBG) {
                        mobBgImageDiv.parent().find(".delete-button").removeClass("d-none");
                    } else {
                        mobBgImageDiv.parent().find(".delete-button").addClass("d-none");
                    }
                } else {
                    logoImageDiv.prop("src", logoImage);
                    webBgImageDiv.prop("src", webBgImage);
                    mobBgImageDiv.prop("src", mobBgImage);

                    logoImageDiv.parent().find(".delete-button").addClass("d-none");
                    webBgImageDiv.parent().find(".delete-button").addClass("d-none");
                    mobBgImageDiv.parent().find(".delete-button").addClass("d-none");
                }
            },
            (error) => {
                logoImageDiv.prop("src", logoImage);
                webBgImageDiv.prop("src", webBgImage);
                mobBgImageDiv.prop("src", mobBgImage);

                logoImageDiv.parent().find(".delete-button").addClass("d-none");
                webBgImageDiv.parent().find(".delete-button").addClass("d-none");
                mobBgImageDiv.parent().find(".delete-button").addClass("d-none");
            }
        );
    } else {
        logoImageDiv.prop("src", logoImage);
        webBgImageDiv.prop("src", webBgImage);
        mobBgImageDiv.prop("src", mobBgImage);

        logoImageDiv.nextAll(".delete-button").addClass("hide");
        webBgImageDiv.nextAll(".delete-button").addClass("hide");
        mobBgImageDiv.nextAll(".delete-button").addClass("hide");
    }
}

function closeUploadDialog() {
    $('#btnFileUpload').prop('disabled', false);
    $("#filMyFile").val('');
    $("#lblnofilename").text('');
}

$j(document).on("change", "#rbllictype input", function (e) {
    switch ($(this).val()) {
        case 'offline':
            $("#dvOffline").show();
            $("#dvOnline").hide();
            break;
        default:
            $("#dvOffline").hide();
            $("#dvOnline").show();
            break;
    }
});

function uploadLogo(e) {

    // var fileUpload = $("#UploadAppLogoImg").get(0);
    var fileUpload = e.target;
    var files = fileUpload.files;

    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }

    var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));

    if ($("#axSelectProj").val()) {
        $.ajax({
            url: url + `ProjectImageUploadHandler.ashx?proj=${$("#axSelectProj").val()}&type=${fileUpload?.dataset?.type || ""}&handlertype=${handlerType}`,
            type: "POST",
            data: data,
            contentType: false,
            processData: false,
            success(result) {
                handlerType = "upload";
                if (result.indexOf("success") > -1) {
                    setProjectImages($("#axSelectProj").val());
                    showAlertDialog("success", result);
                } else if (result.indexOf("size") > -1 || result.indexOf("select")) {
                    showAlertDialog("warning", result);
                } else if (result.indexOf("error") > -1) {
                    showAlertDialog("error", result);
                } else {
                    showAlertDialog("error", result);
                }
                $(fileUpload).val("");
            },
            error(err) {
                handlerType = "upload";
                showAlertDialog("error", err.statusText);
                $(fileUpload).val("");
            }
        });
    } else {
        handlerType = "upload";
        showAlertDialog("error", "Project Name should be selected");
    }
}

function AuthenticateUser() {
    let txtAuthUsername = $("#txtAuthUsername").val();
    if (txtAuthUsername == "") {
        $("#txtAuthUsername").focus();
        showAlertDialog("error", "User name should not be left empty.");
        return false;
    }

    let txtAuthPwd = $("#txtAuthPwd").val();
    if (txtAuthPwd == "") {
        $("#txtAuthPwd").focus();
        showAlertDialog("error", "Password should not be left empty.");
        return false;
    }

    $.ajax({
        url: 'AxpertAdmin.aspx/UserAuthentication',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            AuthUsername: $("#txtAuthUsername").val(), AuthPwd: $("#txtAuthPwd").val(),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            $('#configbody').removeClass('m-auto');

            $('#main_body').removeAttr("style")
            if (data.d != "" && !data.d.startsWith("error:")) {
                var qstr = window.document.location.href;
                if (qstr.indexOf("?") > -1) {
                    var qsAuth = qstr.split("?")[1];
                    if (qsAuth.indexOf("auth=") > -1) {
                        var varAuth = "";
                        qsAuth.split("&").forEach(function (elem) {
                            if (elem.indexOf("auth=") == -1)
                                varAuth += elem + "&";
                        });
                        window.document.location.href = "AxpertAdmin.aspx?" + varAuth + "auth=" + data.d;
                    }
                    else
                        window.document.location.href = qstr + "&auth=" + data.d;
                }
                else
                    window.document.location.href = "AxpertAdmin.aspx?auth=" + data.d;
            }
            else {
                if (data.d != "")
                    showAlertDialog("error", data.d.replace("error:", ""));
                else
                    showAlertDialog("error", "Incorrect credentials. please try again!");
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

$j(document).on("click", "#btnAuthCancel,#myModalAuthClose", function (e) {
    window.document.location.href = "signin.aspx";
});

function initUiPanel() {
    // if(selProj){
    $("#axSelectProj").val(selProj);

    setProjectImages(selProj);
    // }
    var newArr = $("#lstconnection option").toArray().map((opt) => {
        return { label: opt.value, value: opt.value, link: "" };
    });
    $("#axSelectProj").select2({
        data: applstJson
    }).on('select2:select', function (event) {
        (appGlobalVarsObject?._CONSTANTS?.window || window).toastr.clear();

        setProjectImages($(this).val());
    });
    //let dbconnVal = '';
    //let params = new URLSearchParams(window.location.search);
    //if (params.has("dbconn"))
    //    dbconnVal = params.get("dbconn");
    //if (dbconnVal != '') {
    //    setTimeout(function () {
    //        $('#lstconnection').val(dbconnVal).trigger('change');

    //        $('#selLicDomain').val(dbconnVal).trigger('change');

    //        $('#lstRconnection').val(dbconnVal).trigger('change');

    //        $('#lstStudioRconnection').val(dbconnVal).trigger('change');

    //        $('#armproj').val(dbconnVal).trigger('change');

    //        $('#fileproj').val(dbconnVal).trigger('change');
    //    });
    //}
}

function applyRedisconnection() {
    let txtrhotname = $("#txtrhotname").val();
    if (txtrhotname == "") {
        $("#txtrhotname").focus();
        showAlertDialog("error", "Host name should not be left empty.");
        return false;
    }

    let txtrport = $("#txtrport").val();
    if (txtrport == "") {
        $("#txtrport").focus();
        showAlertDialog("error", "Port should not be left empty.");
        return false;
    }

    //let txtrpwd = $("#txtrpwd").val();
    //if (txtrpwd == "") {
    //    $("#txtrpwd").focus();
    //    showAlertDialog("error", "Password should not be left empty.");
    //    return false;
    //}
    TestRedisConnectionWs();
    //$("#btnRedisTestConnection").click();
}

$j(document).on("click", "#btnRedisApply", function (e) {
    if ($("#lstRconnection").val() == null || $("#lstRconnection").val() == "") {
        $("#ddlIsRedisNewConnection").val('new');
        $('#myModalRedis').modal("show");
    }
    else {
        $("#ddlIsRedisNewConnection").val('old');
        $("#txtRedisNewConn").val($("#lstRconnection").val());
        $("#btnRedisOk").click();
    }
});

function EnableRedisApplyConnection() {
    $("#btnRedisApply").removeAttr("disabled").removeClass("btndisable");
}

function SuccRedisConnection(msgType) {
    if (msgType == "success") {
        showAlertDialog("success", "Connection is created successful.");
    }
    else {
        showAlertDialog("error", "Connection Name already exists.");
    }
}

function SuccStudioRedisConnection(msgType, strudioUrl) {
    if (msgType == "success") {
        showAlertDialog("success", "Connection is created successful.");
    } else if (msgType == "info") {
        setTimeout(function () {
            showAlertDialog("info", "Connection is created successful. Copy AppSettings.ini and AppSettings.json files to " + strudioUrl + " of physical path and needs to Recycle AxpertStudio Application Pool in IIS");
        }, 0);
    }
    else {
        showAlertDialog("error", "Connection Name already exists.");
    }
}

function TestRedisConnectionWs() {
    $.ajax({
        url: 'AxpertAdmin.aspx/RedisTestConnection',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            rHost: $("#txtrhotname").val(), rPort: $("#txtrport").val(), rPwd: $("#txtrpwd").val(), axwConn: $("#lstRconnection").val(),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                if (data.d == "true:yes") {
                    $("#hdnRPwd").val("true");
                    showAlertDialog("success", "Test Connection was Successful.");
                    EnableRedisApplyConnection();
                } else if (data.d == "true:no") {
                    $("#hdnRPwd").val("false");
                    showAlertDialog("success", "Test Connection was Successful.");
                    EnableRedisApplyConnection();
                }
                else if (data.d.indexOf("false:") > -1) {
                    let lclHost = data.d;
                    $("#hdnRPwd").val("true");
                    showAlertDialog("error", lclHost.split(':')[1]);
                    $("#btnRedisApply").attr("disabled", "true").addClass("btndisable");
                }
                else {
                    $("#hdnRPwd").val("true");
                    let txtrpwd = $("#txtrpwd").val();
                    if (txtrpwd == "")
                        showAlertDialog("error", "Test Connection failed. Please enter password and try again.");
                    else
                        showAlertDialog("error", "Test Connection failed.");
                    $("#btnRedisApply").attr("disabled", "true").addClass("btndisable");
                }
            }
            else {
                $("#hdnRPwd").val("true");
                showAlertDialog("error", "Test Connection failed.");
                $("#btnRedisApply").attr("disabled", "true").addClass("btndisable");
            }
        }, error: function (error) {
            $("#hdnRPwd").val("true");
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function ValidateARMSettings() {
    var proj = $("#hdnprojarm").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    if ($.trim(proj) === '')
        return showAlertDialog("error", "Project cannot be left empty");
    if ($.trim($("#txtarmurl").val()) === '')
        return showAlertDialog("error", "ARM URL cannot be left empty");
    if ($.trim($("#txtarmkey").val()) === '')
        return showAlertDialog("error", "ARMPrivate Key cannot be left empty");
    if ($.trim($("#txtscripturl").val()) === '')
        return showAlertDialog("error", "ARM Scripts URL cannot be left empty");
    if ($.trim($("#txtpeg").val()) === '')
        return showAlertDialog("error", "PEG cannot be left empty");

    if ($.trim($("#txtArmExpiryMinutes").val()) === '')
        return showAlertDialog("error", "ARM Expiry Minutes cannot be left empty");

    if ($.trim($("#txtNotificationURL").val()) === '')
        return showAlertDialog("error", "Notification URL cannot be left empty");
    if ($.trim($("#txtNotificationExpiryHours").val()) === '')
        return showAlertDialog("error", "Notification Expiry Hours cannot be left empty");
    if ($.trim($("#txtNotificationMaxPerUser").val()) === '')
        return showAlertDialog("error", "Notification Max Per User cannot be left empty");

    $.ajax({
        url: 'AxpertAdmin.aspx/VerifyARMSettings',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            proj: proj,
            aUrl: $("#txtarmurl").val(),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "" && !data.d.startsWith("Error:")) {
                let _settings = JSON.parse(data.d);
                if (typeof _settings.result.errors != "undefined") {
                    let errorsArr = JSON.parse(data.d).result.errors;
                    let errorMsg = errorsArr.join("<br/>");
                    showAlertDialog("warning", errorMsg);
                } else if (_settings.result.success) {
                    showAlertDialog("success", "ARM is running successfully.");
                }
            } else if (data.d != "" && data.d.startsWith("Error:")) {
                let _error = data.d;
                showAlertDialog("error", _error.replace("Error: ", ""));
            } else if (data.d == "") {
                showAlertDialog("error", "ARM is not running ");
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function TestARMConnectionWs() {
    var proj = $("#hdnprojarm").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    if ($.trim(proj) === '')
        return showAlertDialog("error", "Project cannot be left empty");
    if ($.trim($("#txtarmkey").val()) === '')
        return showAlertDialog("error", "ARMPrivate Key cannot be left empty");
    if ($.trim($("#txtarmurl").val()) === '')
        return showAlertDialog("error", "ARM URL cannot be left empty");
    if ($.trim($("#txtscripturl").val()) === '')
        return showAlertDialog("error", "ARM Scripts URL cannot be left empty");
    if ($.trim($("#txtpeg").val()) === '')
        return showAlertDialog("error", "PEG cannot be left empty");

    if ($.trim($("#txtArmExpiryMinutes").val()) === '')
        return showAlertDialog("error", "ARM Expiry Minutes cannot be left empty");

    if ($.trim($("#txtNotificationURL").val()) === '')
        return showAlertDialog("error", "Notification URL cannot be left empty");
    if ($.trim($("#txtNotificationExpiryHours").val()) === '')
        return showAlertDialog("error", "Notification Expiry Hours cannot be left empty");
    if ($.trim($("#txtNotificationMaxPerUser").val()) === '')
        return showAlertDialog("error", "Notification Max Per User cannot be left empty");

    $.ajax({
        url: 'AxpertAdmin.aspx/ARMConnection',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            aKey: $("#txtarmkey").val(), aUrl: $("#txtarmurl").val(), aScriptsUrl: $("#txtscripturl").val(), aPeg: $("#txtpeg").val(), proj: proj,
            aExpiryMinutes: $("#txtArmExpiryMinutes").val(),
            aNotificationURL: $("#txtNotificationURL").val(), aNotificationExpiryHours: $("#txtNotificationExpiryHours").val(),
            aNotificationMaxPerUser: $("#txtNotificationMaxPerUser").val(),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                /*appSettingList = data.d;*/
                appSettingList = JSON.parse(data.d)
                /*showAlertDialog("success", "ARM Connection is saved successfully");*/
                let _armPath = $("#txtarmurl").val();
                showAlertDialog("info", "ARM Connection is saved successfully. Copy AppSettings.ini file to " + _armPath + " of physical path and needs to Recycle ARM Application Pool in IIS.");
                document.getElementById("txtarmkey").value = "";
                document.getElementById("txtarmurl").value = "";
                document.getElementById("txtscripturl").value = "";
                document.getElementById("txtpeg").value = "";

                document.getElementById("txtArmExpiryMinutes").value = "";

                document.getElementById("txtNotificationURL").value = "";
                document.getElementById("txtNotificationExpiryHours").value = "12";
                document.getElementById("txtNotificationMaxPerUser").value = "10";

                document.getElementById("txtRMQAPIURL").value = "";
                document.getElementById("txtSignalRapiURL").value = "";
                document.getElementById("txtFCMSendMsgURL").value = "";
                document.getElementById("txtRapidSaveURL").value = "";
                document.getElementById("txtpegemailactionurl").value = "";
                document.getElementById("txtScriptsAPIURL").value = "";

                $('#txtpeg').trigger('change');

                $('#txtNotificationExpiryHours').trigger('change');
                $('#txtNotificationMaxPerUser').trigger('change');

                $('#armproj').val(null).trigger('change');
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function DelARMConnectionWs() {
    var proj = $("#hdnprojarm").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    $.ajax({
        url: 'AxpertAdmin.aspx/DelARMConnectionWs',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            proj: proj,
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                /*appSettingList = data.d;*/
                appSettingList = JSON.parse(data.d)
                showAlertDialog("success", "ARM Connection is deleted successfully");
                document.getElementById("txtarmkey").value = "";
                document.getElementById("txtarmurl").value = "";
                document.getElementById("txtscripturl").value = "";
                document.getElementById("txtpeg").value = "";

                document.getElementById("txtArmExpiryMinutes").value = "";

                document.getElementById("txtNotificationURL").value = "";
                document.getElementById("txtNotificationExpiryHours").value = "12";
                document.getElementById("txtNotificationMaxPerUser").value = "10";

                document.getElementById("txtRMQAPIURL").value = "";
                document.getElementById("txtSignalRapiURL").value = "";
                document.getElementById("txtFCMSendMsgURL").value = "";
                document.getElementById("txtRapidSaveURL").value = "";
                document.getElementById("txtpegemailactionurl").value = "";
                document.getElementById("txtScriptsAPIURL").value = "";

                $('#txtpeg').trigger('change');

                $('#txtNotificationExpiryHours').trigger('change');
                $('#txtNotificationMaxPerUser').trigger('change');
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}


function RMQueuesaveWs() {
    if ($.trim($("#txtRmQueueHost").val()) === '')
        return showAlertDialog("error", "RMQueue Host cannot be left empty");
    if ($.trim($("#txtRmQueuePort").val()) === '')
        return showAlertDialog("error", "RMQueue Port cannot be left empty");
    if ($.trim($("#txtrmqueueuser").val()) === '')
        return showAlertDialog("error", "RMQueue User cannot be left empty");
    if ($.trim($("#txtrmqueuepwd").val()) === '')
        return showAlertDialog("error", "RMQueue Password cannot be left empty");
    $.ajax({
        url: 'AxpertAdmin.aspx/RMQueueConnection',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            aRMQHost: $("#txtRmQueueHost").val(), aRMQPort: $("#txtRmQueuePort").val(), aRMQUser: $("#txtrmqueueuser").val(), aRMQPwd: $("#txtrmqueuepwd").val(),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                appSettingList = JSON.parse(data.d);
                showAlertDialog("success", "RM Queue Connection is saved successfully");
                //document.getElementById("txtRmQueueHost").value = "";
                //document.getElementById("txtRmQueuePort").value = "";
                //document.getElementById("txtrmqueueuser").value = "";
                //document.getElementById("txtrmqueuepwd").value = "";

                //$('#RMQueueproj').val(null).trigger('change');
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function RMQueueCancelWs() {
    $.ajax({
        url: 'AxpertAdmin.aspx/DelRMQueueConnectionWs',
        type: 'POST',
        cache: false,
        async: true,
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                appSettingList = JSON.parse(data.d)
                showAlertDialog("success", "RM Queue Connection is deleted successfully");

                document.getElementById("txtRmQueueHost").value = "";
                document.getElementById("txtRmQueuePort").value = "";
                document.getElementById("txtrmqueueuser").value = "";
                document.getElementById("txtrmqueuepwd").value = "";
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}


function TestFileConnectionWs() {
    var proj = $("#hdnfileproj").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    if ($.trim(proj) === '')
        return showAlertDialog("error", "Project cannot be left empty");
    if ($.trim($("#txtfileupload").val()) === '')
        return showAlertDialog("error", "File Upload Path cannot be left empty");
    if ($.trim($("#txtfiledownload").val()) === '')
        return showAlertDialog("error", "File Download Path cannot be left empty");

    $.ajax({
        url: 'AxpertAdmin.aspx/FileConnection',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            fUpload: $("#txtfileupload").val(), fDownload: $("#txtfiledownload").val(), proj: proj, fMapUsername: $("#txtfileMapUsername").val(), fMapPwd: $("#tstfileMapPwd").val(), AxAttachSize: $('#ddlAttachmentSize').val(),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {

                showAlertDialog("success", "File Connection is saved successfully");
                /*appSettingList = data.d;*/
                appSettingList = JSON.parse(data.d)
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function DelFileConnectionWs() {
    var proj = $("#hdnfileproj").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    $.ajax({
        url: 'AxpertAdmin.aspx/DelFileConnectionWs',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            proj: proj,
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                showAlertDialog("success", "File Connection is deleted successfully");
                document.getElementById("txtfileupload").value = "";
                document.getElementById("txtfiledownload").value = "";
                document.getElementById("txtfileMapUsername").value = "";
                document.getElementById("tstfileMapPwd").value = "";
                document.getElementById("ddlAttachmentSize").value = "1";
                $('#ddlAttachmentSize').trigger('change');

                appSettingList = JSON.parse(data.d)
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}


function CreateRedisConnection() {
    let txtNewConName = $("#txtRedisNewConn").val();
    if (txtNewConName == "") {
        $("#txtRedisNewConn").focus();
        showAlertDialog("error", "Connection name should not be left empty.");
        return false;
    }
    var regex = new RegExp("^[a-zA-Z0-9_]+$");
    if (!regex.test(txtNewConName)) {
        $("#txtRedisNewConn").focus();
        showAlertDialog("error", "Invalid Name - Special characters not allowed.");
        return false;
    }
}

$j(document).on("click", "#lstRconnection option", function (e) {
    var rlvalue = $(this).val();
    //if (redislstJson != "") {
    //    var rhost = redislstJson.axp_rconn[rlvalue].host;
    //    var rport = redislstJson.axp_rconn[rlvalue].port;
    //    var rpwd = redislstJson.axp_rconn[rlvalue].pwd;
    //    $("#txtrhotname").val(rhost);
    //    $("#txtrport").val(rport);
    //    $("#txtrpwd").val("");
    //}
    if (appSettingList && typeof appSettingList === "string") {
        appSettingList = JSON.parse(appSettingList);
    }
    if (appSettingList && appSettingList.appsettings && rlvalue != null) {
        if (appSettingList.appsettings.hasOwnProperty(rlvalue) && typeof appSettingList.appsettings[rlvalue].Redis != "undefined") {
            let projData = appSettingList.appsettings[rlvalue];
            document.getElementById("txtrhotname").value = projData.Redis.host || "";
            document.getElementById("txtrport").value = projData.Redis.port || "";
            document.getElementById("txtrpwd").value = "";
        } else {
            $("#txtrhotname").val("");
            $("#txtrport").val("");
            $("#txtrpwd").val("");
        }
    } else {
        $("#txtrhotname").val("");
        $("#txtrport").val("");
        $("#txtrpwd").val("");
    }
});

$j(document).on("change", "#lstRconnection", function (e) {
    var rlvalue = $(this).val();
    //if (redislstJson != "" && rlvalue != null) {
    //    var rhost = redislstJson.axp_rconn[rlvalue].host;
    //    var rport = redislstJson.axp_rconn[rlvalue].port;
    //    var rpwd = redislstJson.axp_rconn[rlvalue].pwd;
    //    $("#txtrhotname").val(rhost);
    //    $("#txtrport").val(rport);
    //    $("#txtrpwd").val("");
    //}
    if (appSettingList && typeof appSettingList === "string") {
        appSettingList = JSON.parse(appSettingList);
    }
    if (appSettingList && appSettingList.appsettings && rlvalue != null) {
        if (appSettingList.appsettings.hasOwnProperty(rlvalue) && typeof appSettingList.appsettings[rlvalue].Redis != "undefined") {
            let projData = appSettingList.appsettings[rlvalue];
            document.getElementById("txtrhotname").value = projData.Redis.host || "";
            document.getElementById("txtrport").value = projData.Redis.port || "";
            document.getElementById("txtrpwd").value = "";
        } else {
            $("#txtrhotname").val("");
            $("#txtrport").val("");
            $("#txtrpwd").val("");
        }
    } else {
        $("#txtrhotname").val("");
        $("#txtrport").val("");
        $("#txtrpwd").val("");
    }
});

$j(document).on("click", "#btnRedisAdd", function (e) {
    $("#txtrhotname").val("");
    $("#txtrport").val("");
    $("#txtrpwd").val("");
    $("#lstRconnection").val("").trigger('change');
});

$j(document).on("click", "#btnRcdelete", function (e) {
    var conDelete = $.confirm({
        closeIcon: false,
        title: 'Confirm',
        escapeKey: 'buttonB',
        theme: 'modern',
        onContentReady: function () {
            disableBackDrop('bind');
        },
        content: eval(callParent('lcm[522]')),
        buttons: {
            buttonA: {
                text: eval(callParent('lcm[279]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    $j("#btnRedisdelete").click();
                }
            },
            buttonB: {
                text: eval(callParent('lcm[280]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                }
            }
        }
    });
});

$j(document).on("click", "#btnrnewcancel,#myModalRedisclose", function (e) {
    $('#myModalRedis').modal("hide");
});
function SaveLicDomain() {
    var proj = $("#hdnLicDomainProj").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    if ($.trim(proj) === '')
        return showAlertDialog("error", "Project cannot be left empty");
    if ($.trim($("#txtLicensedDomain").val()) === '')
        return showAlertDialog("error", "Licensed Domain URL cannot be left empty");

    $.ajax({
        url: 'AxpertAdmin.aspx/LicDomainConnection',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            domainName: $("#txtLicensedDomain").val(), proj: proj,
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                appSettingList = JSON.parse(data.d)
                showAlertDialog("success", "Licensed Domain Connection is saved successfully");
                document.getElementById("txtLicensedDomain").value = "";
                $('#selLicDomain').val(null).trigger('change');
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function DelLicDomain() {
    var proj = $("#hdnLicDomainProj").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    $.ajax({
        url: 'AxpertAdmin.aspx/DelLicDomainConnection',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            proj: proj,
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                showAlertDialog("success", "Licensed Domain Connection is deleted successfully");
                document.getElementById("txtLicensedDomain").value = "";
                appSettingList = JSON.parse(data.d)
                $('#selLicDomain').val(null).trigger('change');
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

$j(document).on("change", "#selLicDomain", function (e) {
    var lvalue = $(this).val();
    if (applstJson != "") {
        var proj = lvalue;
        if (proj.indexOf("\\") != -1)
            proj = proj.split("\\")[0];
        $("#hdnLicDomainProj").val(proj);
        try {
            if (appSettingList && typeof appSettingList === "string") {
                appSettingList = JSON.parse(appSettingList);
            }
            if (appSettingList && appSettingList.appsettings) {
                if (appSettingList.appsettings.hasOwnProperty(proj) && typeof appSettingList.appsettings[proj].licdomain != "undefined") {
                    let projData = appSettingList.appsettings[proj];
                    document.getElementById("txtLicensedDomain").value = projData.licdomain.domainname || "";
                }
                else {
                    document.getElementById("txtLicensedDomain").value = "";
                }
            }
        }
        catch (ex) { }
    }
});

$j(document).on("click", "#btnRedisAddStudio", function (e) {
    $("#txtStudioUrl").val("");
    $("#txtStudioScriptPath").val("");
    $("#txtStudioURLPath").val("");
    $("#txtrhotnameStudio").val("");
    $("#txtrportStudio").val("");
    $("#txtrpwdStudio").val("");
    $("#lstStudioRconnection").val("").trigger('change');
});

$j(document).on("click", "#btnRcdeleteStudio", function (e) {
    var conDelete = $.confirm({
        closeIcon: false,
        title: 'Confirm',
        escapeKey: 'buttonB',
        theme: 'modern',
        onContentReady: function () {
            disableBackDrop('bind');
        },
        content: eval(callParent('lcm[522]')),
        buttons: {
            buttonA: {
                text: eval(callParent('lcm[279]')),
                btnClass: 'btn btn-primary',
                action: function () {
                    $j("#btnRedisdeleteStudio").click();
                }
            },
            buttonB: {
                text: eval(callParent('lcm[280]')),
                btnClass: 'btn btn-bg-light btn-color-danger btn-active-light-danger',
                action: function () {
                    disableBackDrop('destroy');
                }
            }
        }
    });
});

$j(document).on("click", "#btnRedisApplyStudio", function (e) {
    if ($("#lstStudioRconnection").val() == null || $("#lstStudioRconnection").val() == "") {
        $("#ddlIsRedisNewConnectionStudio").val('new');
        $('#myModalRedisStudio').modal("show");
    }
    else {
        $("#ddlIsRedisNewConnectionStudio").val('old');
        $("#txtRedisNewConnStudio").val($("#lstStudioRconnection").val());
        $("#btnRedisOkStudio").click();
    }
});

function CreateRedisConnectionStudio() {
    let txtNewConName = $("#txtRedisNewConnStudio").val();
    if (txtNewConName == "") {
        $("#txtRedisNewConnStudio").focus();
        showAlertDialog("error", "Connection name should not be left empty.");
        return false;
    }
    var regex = new RegExp("^[a-zA-Z0-9_]+$");
    if (!regex.test(txtNewConName)) {
        $("#txtRedisNewConnStudio").focus();
        showAlertDialog("error", "Invalid Name - Special characters not allowed.");
        return false;
    }
}

$j(document).on("click", "#lstStudioRconnection option", function (e) {
    var rlvalue = $(this).val();
    if (appSettingList && typeof appSettingList === "string") {
        appSettingList = JSON.parse(appSettingList);
    }
    if (appSettingList && appSettingList.appsettings && rlvalue != null) {
        if (appSettingList.appsettings.hasOwnProperty(rlvalue) && typeof appSettingList.appsettings[rlvalue].AxStudioRedis != "undefined") {
            let projData = appSettingList.appsettings[rlvalue];
            document.getElementById("txtStudioUrl").value = projData.AxStudioRedis.studiourl || "";
            document.getElementById("txtrhotnameStudio").value = projData.AxStudioRedis.host || "";
            document.getElementById("txtrportStudio").value = projData.AxStudioRedis.port || "";
            document.getElementById("txtrpwdStudio").value = "";

            document.getElementById("txtStudioScriptPath").value = projData.AxStudioRedis.studioScriptPath || "";
            document.getElementById("txtStudioURLPath").value = projData.AxStudioRedis.studioURLPath || "";


        } else {
            $("#txtStudioUrl").val("");
            $("#txtrhotnameStudio").val("");
            $("#txtrportStudio").val("");
            $("#txtrpwdStudio").val("");
            $("#txtStudioScriptPath").val("");
            $("#txtStudioURLPath").val("");
        }
    } else {
        $("#txtStudioUrl").val("");
        $("#txtrhotnameStudio").val("");
        $("#txtrportStudio").val("");
        $("#txtrpwdStudio").val("");
        $("#txtStudioScriptPath").val("");
        $("#txtStudioURLPath").val("");
    }
});

$j(document).on("change", "#lstStudioRconnection", function (e) {
    var rlvalue = $(this).val();
    if (appSettingList && typeof appSettingList === "string") {
        appSettingList = JSON.parse(appSettingList);
    }
    if (appSettingList && appSettingList.appsettings && rlvalue != null) {
        if (appSettingList.appsettings.hasOwnProperty(rlvalue) && typeof appSettingList.appsettings[rlvalue].AxStudioRedis != "undefined") {
            let projData = appSettingList.appsettings[rlvalue];
            document.getElementById("txtStudioUrl").value = projData.AxStudioRedis.studiourl || "";
            document.getElementById("txtrhotnameStudio").value = projData.AxStudioRedis.host || "";
            document.getElementById("txtrportStudio").value = projData.AxStudioRedis.port || "";
            document.getElementById("txtrpwdStudio").value = "";

            document.getElementById("txtStudioScriptPath").value = projData.AxStudioRedis.studioScriptPath || "";
            document.getElementById("txtStudioURLPath").value = projData.AxStudioRedis.studioURLPath || "";
        } else {
            $("#txtStudioUrl").val("");
            $("#txtrhotnameStudio").val("");
            $("#txtrportStudio").val("");
            $("#txtrpwdStudio").val("");
            $("#txtStudioScriptPath").val("");
            $("#txtStudioURLPath").val("");
        }
    } else {
        $("#txtStudioUrl").val("");
        $("#txtrhotnameStudio").val("");
        $("#txtrportStudio").val("");
        $("#txtrpwdStudio").val("");
        $("#txtStudioScriptPath").val("");
        $("#txtStudioURLPath").val("");
    }
});

function applyRedisconnStudio() {
    let txtStudioUrl = $("#txtStudioUrl").val();
    if (txtStudioUrl == "") {
        $("#txtStudioUrl").focus();
        showAlertDialog("error", "Studio URL should not be left empty.");
        return false;
    }

    let txtrhotname = $("#txtrhotnameStudio").val();
    if (txtrhotname == "") {
        $("#txtrhotnameStudio").focus();
        showAlertDialog("error", "Host name should not be left empty.");
        return false;
    }

    let txtrport = $("#txtrportStudio").val();
    if (txtrport == "") {
        $("#txtrportStudio").focus();
        showAlertDialog("error", "Port should not be left empty.");
        return false;
    }

    let txtStudioScriptPath = $("#txtStudioScriptPath").val();
    if (txtStudioScriptPath == "") {
        $("#txtStudioScriptPath").focus();
        showAlertDialog("error", "Studio Script path should not be left empty.");
        return false;
    }

    let txtStudioURLPath = $("#txtStudioURLPath").val();
    if (txtStudioURLPath == "") {
        $("#txtStudioURLPath").focus();
        showAlertDialog("error", "Studio Script URL should not be left empty.");
        return false;
    }

    TestRedisConnectionStudioWs();
}

function TestRedisConnectionStudioWs() {
    $.ajax({
        url: 'AxpertAdmin.aspx/RedisTestConnectionStudio',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            rHost: $("#txtrhotnameStudio").val(), rPort: $("#txtrportStudio").val(), rPwd: $("#txtrpwdStudio").val(), axsConn: $("#lstStudioRconnection").val(), studioUrl: $("#txtStudioUrl").val(),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                if (data.d == "true:yes") {
                    showAlertDialog("success", "Test Connection was Successful.");
                    EnableRedisApplyConnectionStudio();
                } else if (data.d == "true:no") {
                    showAlertDialog("success", "Test Connection was Successful.");
                    EnableRedisApplyConnectionStudio();
                }
                else if (data.d.indexOf("false:") > -1) {
                    let lclHost = data.d;
                    showAlertDialog("error", lclHost.split(':')[1]);
                    $("#btnRedisApplyStudio").attr("disabled", "true").addClass("btndisable");
                }
                else {
                    let txtrpwd = $("#txtrpwd").val();
                    if (txtrpwd == "")
                        showAlertDialog("error", "Test Connection failed. Please enter password and try again.");
                    else
                        showAlertDialog("error", "Test Connection failed.");
                    $("#btnRedisApplyStudio").attr("disabled", "true").addClass("btndisable");
                }
            }
            else {
                showAlertDialog("error", "Test Connection failed.");
                $("#btnRedisApplyStudio").attr("disabled", "true").addClass("btndisable");
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}
function EnableRedisApplyConnectionStudio() {
    $("#btnRedisApplyStudio").removeAttr("disabled").removeClass("btndisable");
}
$j(document).on("click", "#btnNamedUrls", function (e) {
    let nextIndex = $j("#namedUrlContainer .named-url-row").length + 1;
    let row = `
        <div class="row g-3 align-items-center mb-2 named-url-row">
            <div class="col-md-4 col-sm-12">
                <input type="text" class="m-wrap placeholder-no-fix form-control"
                       placeholder="Name" name="seq_URLS_${nextIndex}" />
            </div>
            <div class="col-md-7 col-sm-12">
                <input type="text" class="m-wrap placeholder-no-fix form-control"
                       placeholder="Named URL" name="url_URLS_${nextIndex}" />
            </div>
            <div class="col-md-1 col-sm-12">
                <button type="button" class="btn btn-sm btn-light-danger deleteNamedURLS">
                    <span class="material-icons">delete</span>
                </button>
            </div>
        </div>
    `;
    $j("#namedUrlContainer").append(row);
});

$(document).on("click", ".deleteNamedURLS", function () {
    $(this).closest(".named-url-row").remove();
});
$j(document).on("click", "#btnNamedSFTP", function (e) {
    let nextIndex = $j("#namedSFTPContainer .named-url-row").length + 1;
    let row = `
        <div class="row g-3 align-items-center mb-2 named-url-row">
            <div class="col-md-4 col-sm-12">
                <input type="text" class="m-wrap placeholder-no-fix form-control"
                       placeholder="Name" name="seq_SFTP_${nextIndex}" />
            </div>
            <div class="col-md-7 col-sm-12">
                <input type="text" class="m-wrap placeholder-no-fix form-control"
                       placeholder="Named SFTP" name="url_SFTP_${nextIndex}" />
            </div>
            <div class="col-md-1 col-sm-12">
                <button type="button" class="btn btn-sm btn-light-danger deleteNamedSFTP">
                    <span class="material-icons">delete</span>
                </button>
            </div>
        </div>
    `;
    $j("#namedSFTPContainer").append(row);
});

$(document).on("click", ".deleteNamedSFTP", function () {
    $(this).closest(".named-url-row").remove();
});
$j(document).on("click", "#btnNamedFileServer", function (e) {
    let nextIndex = $j("#namedFileServerContainer .named-url-row").length + 1;
    let row = `
        <div class="row g-3 align-items-center mb-2 named-url-row">
            <div class="col-md-4 col-sm-12">
                <input type="text" class="m-wrap placeholder-no-fix form-control"
                       placeholder="Name" name="seq_FileServer_${nextIndex}" />
            </div>
            <div class="col-md-7 col-sm-12">
                <input type="text" class="m-wrap placeholder-no-fix form-control"
                       placeholder="Named File Server" name="url_FileServer_${nextIndex}" />
            </div>
            <div class="col-md-1 col-sm-12">
                <button type="button" class="btn btn-sm btn-light-danger deleteNamedFileServer">
                    <span class="material-icons">delete</span>
                </button>
            </div>
        </div>
    `;
    $j("#namedFileServerContainer").append(row);
});

$(document).on("click", ".deleteNamedFileServer", function () {
    $(this).closest(".named-url-row").remove();
});


function SaveExternalResWs() {

    var proj = $("#hdnarmExtResource").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    if ($.trim(proj) === '')
        return showAlertDialog("error", "Project cannot be left empty");
    let namedUrls = [];
    $("#namedUrlContainer .named-url-row").each(function () {
        let name = $(this).find("input[name^='seq_URLS_']").val();
        let url = $(this).find("input[name^='url_URLS_']").val();

        if ($.trim(name) !== "" || $.trim(url) !== "") {
            namedUrls.push({ Name: name, Url: url });
        }
    });
    let namedSftp = [];
    $("#namedSFTPContainer .named-url-row").each(function () {
        let name = $(this).find("input[name^='seq_SFTP_']").val();
        let sftp = $(this).find("input[name^='url_SFTP_']").val();

        if ($.trim(name) !== "" || $.trim(sftp) !== "") {
            namedSftp.push({ Name: name, SFTP: sftp });
        }
    });
    let namedFileServers = [];
    $("#namedFileServerContainer .named-url-row").each(function () {
        let name = $(this).find("input[name^='seq_FileServer_']").val();
        let fileServer = $(this).find("input[name^='url_FileServer_']").val();

        if ($.trim(name) !== "" || $.trim(fileServer) !== "") {
            namedFileServers.push({ Name: name, FileServer: fileServer });
        }
    });
    $.ajax({
        url: 'AxpertAdmin.aspx/SaveExternalResWs',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            proj: proj,
            NamedUrls: JSON.stringify(namedUrls),
            NamedSftp: JSON.stringify(namedSftp),
            NamedFileServers: JSON.stringify(namedFileServers),
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {

                showAlertDialog("success", "External resources is saved successfully");

                appSettingList = JSON.parse(data.d)
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function DelExternalResWs() {
    var proj = $("#hdnarmExtResource").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    $.ajax({
        url: 'AxpertAdmin.aspx/DelExternalResWs',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            proj: proj,
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                appSettingList = JSON.parse(data.d)
                showAlertDialog("success", "External resources is deleted successfully");



                $('#armExtResource').trigger('change');
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}

function renderNamedItems(container, dataObject, type) {
    let index = 1;
    for (let key in dataObject) {
        let value = dataObject[key];
        let row = `
            <div class="row g-3 align-items-center mb-2 named-url-row">
                <div class="col-md-4 col-sm-12">
                    <input type="text" class="m-wrap placeholder-no-fix form-control"
                           name="seq_${type}_${index}" value="${key}" />
                </div>
                <div class="col-md-7 col-sm-12">
                    <input type="text" class="m-wrap placeholder-no-fix form-control"
                           name="url_${type}_${index}" value="${value}" />
                </div>
                <div class="col-md-1 col-sm-12">
                    <button type="button" class="btn btn-sm btn-light-danger deleteNamed${type}">
                        <span class="material-icons">delete</span>
                    </button>
                </div>
            </div>
        `;
        $j(container).append(row);
        index++;
    }
}

$j(document).on("change", "#axpDevOptions", function () {
    var lvalue = $(this).val();
    if (applstJson != "") {
        var projDevOpt = lvalue;
        if (projDevOpt.indexOf("\\") != -1)
            projDevOpt = projDevOpt.split("\\")[0];
        $("#hdnaxpDevOptions").val(projDevOpt);
        try {
            if (appSettingList && typeof appSettingList === "string")
                appSettingList = JSON.parse(appSettingList);
            if (appSettingList && appSettingList.appsettings && projDevOpt != null) {
                if (appSettingList.appsettings.hasOwnProperty(projDevOpt) && typeof appSettingList.appsettings[projDevOpt].AxpDevOptsMenu != "undefined") {
                    let savedValue = appSettingList.appsettings[projDevOpt].AxpDevOptsMenu.Options || "";
                    var $ddl = $('#ddlAxpertdevOptions');
                    if (savedValue === "all") {
                        var allVals = [];
                        $ddl.find('option').each(function () {
                            if ($(this).val() !== "all")
                                allVals.push($(this).val());
                        });
                        $ddl.val(allVals).trigger('change');
                    }
                    else if (savedValue !== "") {
                        $ddl.val(savedValue.split(',')).trigger('change');
                    }
                    else {
                        $ddl.val(null).trigger('change');
                    }
                } else {
                    $('#ddlAxpertdevOptions').val(null).trigger('change');
                }
            } else {
                $('#ddlAxpertdevOptions').val(null).trigger('change');
            }
        }
        catch (ex) {
        }
    }
});

$j(document).on("change", "#ddlAxpertdevOptions", function () {
    var values = $(this).val();
    if (values && values.includes("all")) {
        var allVals = [];
        $(this).find('option').each(function () {
            if ($(this).val() !== "all")
                allVals.push($(this).val());
        });
        $(this).val(allVals).trigger('change.select2');
    }
});

function SaveAxpertDevOptions() {
    var proj = $("#hdnaxpDevOptions").val();
    if (proj.indexOf("\\") != -1)
        proj = proj.split("\\")[0];
    if ($.trim(proj) === '')
        return showAlertDialog("error", "Project cannot be left empty");
    var selectedVals = $("#ddlAxpertdevOptions").val();
    if (!selectedVals || selectedVals.length === 0) {
      /*  return showAlertDialog("error", "Options cannot be empty");*/
        showAlertDialog("warning", "No Axpert Developer Options selected. All Axpert Developer Options will be hidden in runtime.");
    }
    var devOpt = "";
    if (selectedVals.includes("all"))
        devOpt = "all";
    else
        devOpt = selectedVals.join(",");
    $.ajax({
        url: 'AxpertAdmin.aspx/SaveAxpertDevOptionsWs',
        type: 'POST',
        cache: false,
        async: true,
        data: JSON.stringify({
            proj: proj,
            devOpt: devOpt,
            csrfToken: $("#_antiforgery").val()
        }),
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                showAlertDialog("success", "Axpert Developer Options Menu saved successfully");
                appSettingList = JSON.parse(data.d)
            }
        }, error: function (error) {
            if (typeof error?.responseJSON?.Message != "undefined" && error?.responseJSON?.Message != "")
                showAlertDialog("error", error?.responseJSON?.Message);
            else
                showAlertDialog("error", error);
        }
    });
}