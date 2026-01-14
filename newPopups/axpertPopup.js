
var iframeHtmlSrc = "";
var isBackBtnHidden = false;
var isRefreshParentAction = "false";

var onLoadEvent = "";
var unLoadEvent = "";

function createPopup(iframeSource, isRefreshSelect, width, height, onLoadEventArg = "", unLoadEventArg = "") {
    iframeHtmlSrc = iframeSource
    width = width || "100vw";
    //  height = height || "40vh";

    onLoadEvent = onLoadEventArg;
    unLoadEvent = unLoadEventArg;

    if (isRefreshSelect != undefined) {
        try {
            eval(callParent('isTstructPopup') + " = true");
        }
        catch (ex) {
            console.log(ex.message);
        }
    }
    htmlContent = createIframeMarkup(iframeSource, width, height);
    $("head").append(htmlContent);
    if (iframeSource.indexOf("tstruct.aspx") !== -1 || iframeSource.indexOf("ivtstload.aspx") !== -1) {
        var options = { "closeOnOutsideClick": true, "hashTracking": false, "closeOnEscape": false };
    } else {
        var options = { "closeOnOutsideClick": true, "hashTracking": false, "closeOnEscape": true };
    }
    var inst = $('[data-remodal-id=axpertPopupModal]:not(.remodal-is-initialized):not(.remodal-is-closed):eq(0)').remodal(options);
    if (inst && inst.state != "opened")
        inst.open();


    return inst;
}

function createIframeMarkup(iframeSource, width, height) {
    var popUpsCount = checkForAxpPopUpExists();
    var sizeCss = "";
    if (popUpsCount > 0) {
        popUpsCount++;
        sizeCss = "height:" + (100 - popUpsCount * 10) + "vh;width:" + (100 - popUpsCount * 10) + "vw";
    }
    if (width != undefined) {
        sizeCss = "width:" + width + ";";
    }
    if (height != undefined) {
        sizeCss += "height:" + height + ";";
    }

    isRefreshParentAction = "false";
    if (iframeHtmlSrc.indexOf("AxPop=true") > -1 && (window.document.title == "Iview" || window.document.title == "Listview")){
        try {
            var refreshParentActionName = "";
            
            refreshParentActionName = $("#hdnAct").val().substr(4);

            if (ivActions && ivActions[refreshParentActionName]) {
                isRefreshParentAction = ivActions[refreshParentActionName].r1.param1.Refresh.toLowerCase();
            }else if (ivScripts && ivScripts[refreshParentActionName]) {
                isRefreshParentAction = ivScripts[refreshParentActionName].r1.param1.Refresh.toLowerCase();
            }
        } catch (ex) { }
    }

    var $markup = '<div id="axpertPopupWrapper" style="' + sizeCss + '" class="remodal" data-remodal-id="axpertPopupModal">';
    $markup += '<button data-remodal-action="close" class="remodal-close remodalCloseBtn cardStyle material-icons" title="Close">close</button>';
    //$markup += '<iframe src="" id="popupIframeRemodal" width="100%" height="100%" style="border:0px; "></iframe>';
    $markup += "<div style='height:100%;' id='iframeMarkUp'></div>"
    $markup += '</div>';
    return $markup;
}
$(document).on('opening', '#axpertPopupWrapper', function () {
    $("#wrapperForMainNewData", eval(callParent('wrapperForMainNewData', 'id'))).hide();
    ShowDimmer(true);


});

function checkForAxpPopUpExists() {
    if (eval(callParent('axpertPopupWrapper', 'id'))) {
        //
        return $("#axpertPopupWrapper", eval(callParent('axpertPopupWrapper', 'id'))).length;
    }
    else
        return false;
}

$(document).on('closing', '#axpertPopupWrapper', function () {
    if (!checkForAxpPopUpExists())
        $("#wrapperForMainNewData", eval(callParent('wrapperForMainNewData', 'id'))).show();

    if(isBackBtnHidden){
        $(callParentNew("appBackBtn","class")).show();
        isBackBtnHidden = false;
    }
});
$(document).on('opened', '#axpertPopupWrapper', function () {
    if(onLoadEvent){
        onLoadEvent();
        onLoadEvent = "";
    }
    eval(callParent("addOverlayToBody()"));
    try {
        iframeHtmlSrc = ax_loadCustomPopIframe(iframeHtmlSrc, window) || iframeHtmlSrc;
    } catch (ex) { }

    $("#axpertPopupWrapper #iframeMarkUp").html('<iframe src="' + iframeHtmlSrc + '" id="popupIframeRemodal" width="100%" height="100%" style="border:0px; " data-refresh="'+ isRefreshParentAction +'"></iframe>');

    if (window.leftMenuWrapper === undefined) {
        //else it is the main frame should not go beyond this to avoid Cross Frame Origin

        if (window.parent.document) {
            $(window.parent.document).contents().find(".remodalCloseBtn").hide();
        }

    }


    $("#popupIframeRemodal").on("load", function () {
        iframePopupLoadOpts($("#popupIframeRemodal"));
        try {
            if (eval(callParent('isTstructPopup'))) {
                $("#popupIframeRemodal").contents().find("head")
                    .append($("<style>#icons li a:not([title=Save]){display: none !important;}</style>"));
            }
            if (eval(callParent('isDWB')) && $("#popupIframeRemodal").attr('src') == "AxDBScript.aspx" ) {
                $("#popupIframeRemodal").contents().find("head")
                    .append($("<style>#schemaBrwsr #dataView .tab-content #QryEditor #ScriptToolBar ul{ margin-right: 48px;} #dvObjList{margin-top: 42px;}#axpertPopupWrapper button{    background: #807e7e;color: #fff}</style>"));
            }
            
        }
        catch (ex) {
            console.log(ex.message);
        }

        try{
            if($(callParentNew("appBackBtn","class")).is(":visible") && (findGetParameter("axispop", iframeHtmlSrc) || findGetParameter("axpop", iframeHtmlSrc) || eval(callParent('isTstructPopup')))){
                $(callParentNew("appBackBtn","class")).hide();
                isBackBtnHidden = true;
            }
        }catch(ex){}

        try {
            ax_loadCustomPopPage(iframeHtmlSrc, window);
        } catch (ex) { }

        //end
        ShowDimmer(false);
    });

    $("#dvSelectedGlobalVar,#ExportImportCogIcon", eval(callParent('ExportImportCogIcon', 'id'))).hide();
    $("#popupIframeRemodal").contents().find('body :focusable').first().focus();

    MainNewEdit = true;
});

$(document).on('closed', '#axpertPopupWrapper', function () {
    if (window.leftMenuWrapper === undefined) {
        //else it is the main frame should not go beyond this to avoid Cross Frame Origin

        if (window.parent.document) {
            $(window.parent.document).contents().find(".remodalCloseBtn").show();
        }

    }
    var isAxPop = $("#axpertPopupWrapper").find("#popupIframeRemodal").attr("src").indexOf("AxPop=true") > -1;

    var isRefresh = $("#axpertPopupWrapper").find("#popupIframeRemodal").data("refresh") || false;

    var inst = $('[data-remodal-id=axpertPopupModal]:eq(0)').remodal();
    try {
        inst.destroy();
    } catch (ex) { }
    if (!checkForAxpPopUpExists())
        $("#ExportImportCogIcon", eval(callParent('ExportImportCogIcon', 'id'))).show();

    if (!checkForAxpPopUpExists())
        $("#dvSelectedGlobalVar", eval(callParent('dvSelectedGlobalVar', 'id'))).show();
    if (!checkForAxpPopUpExists())
        $("#wrapperForMainNewData", eval(callParent('wrapperForMainNewData', 'id'))).show();
    MainNewEdit = false;
    if (isAxPop && (window.document.title == "Iview" || window.document.title == "Listview") && eval(callParent('isSuccessAlertInPopUp'))) {
        eval(callParent('isSuccessAlertInPopUp') + "= false");
        pushValToSession('IsFromChildWindow', 'true');
        if (eval(callParent('isRefreshParentOnClose'))) {
            eval(callParent('isRefreshParentOnClose') + "= false");
            window.location.href = window.location.href;
        }else if(isRefresh){
            window.location.href = window.location.href;
        }
    } else if (isAxPop && (window.document.title == "Load TStruct with QS" || window.document.title == "Tstruct" || window.document.title == "Load Tstruct") && eval(callParent('isSuccessAlertInPopUp'))) {
        eval(callParent('isSuccessAlertInPopUp') + "= false");
        let ReloadParent = true;
        try {
            var CallActionVar = eval(callParent('callBackFunDtls'));
            if (typeof CallActionVar != "undefined" && CallActionVar.indexOf("♠") > -1) {//On Action call 
                let CallActionName = CallActionVar.split("♠").length == 5 ? CallActionVar.split("♠")[1] : "";
                if (CallActionName != "") {
                    var actIndex = $j.inArray(CallActionName, tstActionName);
                    if (actIndex == -1)
                        actIndex = $j.inArray(CallActionName.toLowerCase(), tstActionName);
                    ReloadParent = actParRefresh[actIndex].toLowerCase() == "true" ? true : false;
                }
            }
        }
        catch (ex) { }
        if (ReloadParent)
            redirectOnSaveAction();
    }
    parent.isTstructPopup = false;
    eval(callParent("removeOverlayFromBody()"));    
    if(unLoadEvent){
        unLoadEvent();
        unLoadEvent = "";
    }
    ShowDimmer(false);
});

function closeRemodalPopup() {
    var inst = $('[data-remodal-id=axpertPopupModal]:not(.remodal-is-closed):eq(0)').remodal();
    try {
        inst.close();
    } catch (ex) { }
}


function pushValToSession(key, val) {
    $.ajax({
        type: "POST",
        url: "../WebService.asmx/AddSessionPair",
        cache: false,
        async: false,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({ key: key, val: val }),
        dataType: "json",
        success: function (data) {
        },
    });
}
$(document).on('click', '#homeIcon,#dashBoardIcon,.leftPartAC', function () {
    if (axMenuStyle === "classic") {
        if (!checkForAxpPopUpExists())
            $("#dvSelectedGlobalVar", eval(callParent('dvSelectedGlobalVar', 'id'))).show();
        $("#wrapperForMainNewData", eval(callParent('wrapperForMainNewData', 'id'))).show();

        //hide utilities menu if user don't have access to any menu (Responsibilities, Import data, Export data, Import history, In-memory DB, Config app, Widget builder)
        if (visibleAppSettings > 0)
            $("#ExportImportCogIcon", eval(callParent('ExportImportCogIcon', 'id'))).show();
    }
});

//$(document).on('click', '#dashBoardIcon', function () {
//    if (!checkForAxpPopUpExists())
//        $("#dvSelectedGlobalVar", eval(callParent('dvSelectedGlobalVar', 'id'))).show();
//    $("#ExportImportCogIcon", eval(callParent('ExportImportCogIcon', 'id'))).show();
//    $("#wrapperForMainNewData", eval(callParent('wrapperForMainNewData', 'id'))).show();

//});

//$(document).on('click', '.leftPartAC', function () {
//    if (!checkForAxpPopUpExists())
//        $("#dvSelectedGlobalVar", eval(callParent('dvSelectedGlobalVar', 'id'))).show();
//    $("#ExportImportCogIcon", eval(callParent('ExportImportCogIcon', 'id'))).show();
//    $("#wrapperForMainNewData", eval(callParent('wrapperForMainNewData', 'id'))).show();

//});

//Devendra+Rakesh Popup SQl Editor 

$(document).on('closed', '#axpertPopupWrapperDWB', function () {
    var parentdiv = '#dv' + $("#axpertPopupWrapperDWB").attr('data-pushtxt-id').split('0')[0];

    var parentEditor = $(parentdiv + ' .CodeMirror')[0].CodeMirror;
    parentEditor.getDoc().setValue($('#QryEditor .CodeMirror')[0].CodeMirror.getValue());
    var inst = $('[data-remodal-id=axpertPopupModalDWB]:eq(0)').remodal();
    try {
        inst.destroy();
    } catch (ex) { }

});
$(document).on('opened', '#axpertPopupWrapperDWB', function () {
    $('table').on('scroll resize', function () {
        $(this).children().width($(this).width() + $(this).scrollLeft())
    });
});
$(document).on('click', '#exeQuery', function () {
    var mainSqlCM = $('#QryEditor .CodeMirror')[0].CodeMirror;
    if (mainSqlCM.getSelection() != "") {
        var query = mainSqlCM.getSelection();
    }
    else {
        var query = mainSqlCM.getValue();
    }

    $.ajax({
        url: 'tstruct.aspx/callExecuteSQL',
        type: 'POST',
        cache: false,
        async: false,
        data: JSON.stringify({ queryString: query }),
        dataType: 'json',
        contentType: "application/json",
        success: function (msg) {
            if (msg.d == "SESSION_TIMEOUT") {
                parent.window.location.href = "../aspx/sess.aspx";
            }
            else if (msg.d != "") {
                dataJSON = JSON.parse(msg.d);
                if (typeof dataJSON.error != "undefined") {
                    $('#txtOutput').text(dataJSON.error["msg"]);
                    $('#txtOutput').show();
                    $('#tblOutput').hide();
                    $('#spnRowCnt').hide();
                }
                else {
                    if (typeof dataJSON["result"] != "object") {
                        $('#txtOutput').text(dataJSON.result);
                        $('#txtOutput').show();
                        $('#tblOutput').hide();
                        $('#spnRowCnt').hide()
                    }
                    else {
                        createDatatableFromJson(dataJSON, $('#tblOutput'));
                        $('#txtOutput').hide();
                        $('#tblOutput').show();
                        $('#spnRowCnt').show();
                    }
                }
            }
        },
        error: function () {
            $('#txtOutput').text("Error while executing the query.")
            // showAlertDialog("warning", "Error while executing the query.");
        }
    });
    return false;
});
function createDatatableFromJson(dataJSON,table){
    if ($.fn.DataTable.isDataTable(table)) {
        table.DataTable().destroy();
        table.empty();
    }


    if (typeof dataJSON["result"]["row"] != "undefined" && dataJSON["result"]["row"].length != 0) {
        table.DataTable({
            data: dataJSON["result"]["row"],
            columns: Object.keys(dataJSON["result"]["row"][0]).map(function (item) {
                return { data: item, title: item }
            }),
            "autoWidth": false,
            "paging": false,
            "filter": false,
            "bInfo": false
        });
        if (table == $('#tblOutput')) {
            var rowCnt = $('#tblOutput').DataTable().data().count();
            $('#spnRowCnt').text("No. of rows : " + rowCnt);
        }
    }
}

function processQueryResult(dataXML) {

    // if ($(dataXML).find('response row').length > 0) {
    //     var table = $('#tblOutput').empty();
    //     table[0].border = "1";
    //     //Add the header row.
    //     var HeaderRow = $(table[0].createTHead().insertRow());
    //     $(dataXML).find('row:first').children().each(function () {
    //         var headerCell = $("<th/>");
    //         headerCell.html($(this).attr('name'));
    //         HeaderRow.append(headerCell);
    //     });

    //     //Add the data rows.
    //     var tbody = table[0].createTBody();
    //     $(dataXML).find('row').each(function () {
    //         var dataRow = $(tbody.insertRow());
    //         $(this).children().each(function () {
    //             var cell = $("<td />");
    //             cell.html($(this).text());
    //             dataRow.append(cell);
    //         });
    //     });

    // }

}
