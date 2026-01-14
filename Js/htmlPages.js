var pageCaption = "", cssFileName = [], jsFileName = [], pageNo, validateHtmlPage = true;

var cssCodeMirror = "";
var jsCodeMirror = "";

let htmlObj = {
    "fields": {
        "pageno": "pageno000F1",
        "caption": "caption000F1",
        "template": "template000F1",
        "templateBtn": "btn26",
        "image": "AxpFilePath_hpImages000F1",
        "htmlCm": "html_editor_htmlsrc000F2",
    },
    "template": {
        "name": "",
        "flag": false,
        "files": {
            "html": "",
            "css": "",
            "js": ""
        }
    }
};

function applyCodeMirror(editorId, type) {

    loadAndCall({
        files: {
            css: ["/Css/codemirror.css",
                "/ThirdParty/codemirror/addon/hint/show-hint.css",
                "/ThirdParty/codemirror/addon/fold/foldgutter.css",
                "/ThirdParty/codemirror/addon/dialog/dialog.css",
                "/ThirdParty/codemirror/addon/search/matchesonscrollbar.css"
            ],
            js: [
                "/Js/codemirror/mode/htmlmixed.js",
                "/Js/codemirror/mode/css.js",
                "/Js/codemirror/mode/javascript.js",
                "/Js/codemirror/mode/xml.js",
                "/ThirdParty/codemirror/addon/edit/matchbrackets.js",
                "/ThirdParty/codemirror/addon/edit/closebrackets.js",
                "/ThirdParty/codemirror/addon/hint/show-hint.js",
                "/ThirdParty/codemirror/addon/hint/html-hint.js",
                "/ThirdParty/codemirror/addon/hint/css-hint.js",
                "/ThirdParty/codemirror/addon/hint/javascript-hint.js",
                "/ThirdParty/codemirror/addon/hint/xml-hint.js",
                "/ThirdParty/codemirror/addon/fold/foldgutter.js",
                "/ThirdParty/codemirror/addon/fold/foldcode.js",
                "/ThirdParty/codemirror/addon/display/autorefresh.js",
                "/ThirdParty/codemirror/addon/dialog/dialog.js",
                "/ThirdParty/codemirror/addon/search/searchcursor.js",
                "/ThirdParty/codemirror/addon/search/search.js",
                "/ThirdParty/codemirror/addon/search/matchesonscrollbar.js",
                "/ThirdParty/codemirror/addon/search/match-highlighter.js",
                "/ThirdParty/codemirror/addon/search/jump-to-line.js",
            ]
        },
        callBack() {

            if (type.toLowerCase() == "html") {
                htmlCodeMirror = CodeMirror.fromTextArea(document.getElementById(editorId), {
                    mode: 'htmlmixed',
                    smartIndent: true,
                    lineNumbers: true,
                    lineWrapping: true,
                    matchBrackets: true,
                    autoCloseBrackets: true,
                    autoRefresh: true,
                    foldGutter: true,
                    gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                    extraKeys: { "Alt-F": "findPersistent" }
                });
                htmlCodeMirror.on("blur", function () {
                    var htmlContent = htmlCodeMirror.getValue();
                    htmlContent = htmlContent.replace(/&/g, '&amp;');
                    var domParser = new DOMParser();
                    var dom = "";
                    if(htmlContent.trim() != ""){
                        dom = domParser.parseFromString(htmlContent, 'text/html');
    
                        if (dom && dom.getElementsByTagName("parsererror").length > 0) {
                            var errorMessage = checkErrorXML(dom.getElementsByTagName("parsererror")[0]);
                            console.log(dom);
                            showAlertDialog("error", errorMessage);
                        }
                        else if (dom && dom.getElementsByTagName("head").length == 0) {
                            console.log(dom);
                            showAlertDialog("error", "Head tag doesn't exists.");
                        }
                        else if (dom && dom.getElementsByTagName("body").length == 0) {
                            console.log(dom);
                            showAlertDialog("error", "Body tag doesn't exists.");
                        }
                        else {
                            SetFieldValue(editorId, htmlContent);
                            UpdateFieldArray(editorId, GetFieldsRowNo(editorId), htmlContent, "parent", "");
                        }
                    }
                    else {
                        showAlertDialog("error", "HTML is empty..!");  
                    }
                });

                htmlCodeMirror.on("keyup", function (editor, event) {
                    if (
                        !(event.ctrlKey) &&
                        (event.keyCode >= 65 && event.keyCode <= 90) ||
                        (event.keyCode >= 97 && event.keyCode <= 122) ||
                        (event.keyCode >= 46 && event.keyCode <= 57)
                    ) {
                        CodeMirror.commands.autocomplete(editor, null, { completeSingle: false });
                    }
                });

                var xt = "", h3OK = 1;
                function checkErrorXML(x) {
                    xt = "";
                    h3OK = 1;
                    checkXML(x);
                    return xt;
                }

                function checkXML(n) {
                    var l, i, nam;
                    nam = n.nodeName;
                    if (nam == "h3") {
                        if (h3OK == 0) {
                            return;
                        }
                        h3OK = 0;
                    }
                    if (nam == "#text") {
                        xt = xt + n.nodeValue + "\n";
                    }
                    l = n.childNodes.length;
                    for (i = 0; i < l; i++) {
                        checkXML(n.childNodes[i]);
                    }
                }
            }

            if (type.toLowerCase() == "css") {
                cssCodeMirror = CodeMirror.fromTextArea(document.getElementById(editorId), {
                    mode: 'text/css',
                    smartIndent: true,
                    lineNumbers: true,
                    lineWrapping: true,
                    matchBrackets: true,
                    autoCloseBrackets: true,
                    autoRefresh: true,
                    foldGutter: true,
                    gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                    extraKeys: { "Alt-F": "findPersistent" }
                });
                cssCodeMirror.on("blur", function () {
                    SetFieldValue(editorId, cssCodeMirror.getValue());
                    UpdateFieldArray(editorId, GetFieldsRowNo(editorId), $("#" + editorId).val(), "parent", "");
                });
                cssCodeMirror.on("keyup", function (editor, event) {
                    if (
                        !(event.ctrlKey) &&
                        (event.keyCode >= 65 && event.keyCode <= 90) ||
                        (event.keyCode >= 97 && event.keyCode <= 122) ||
                        (event.keyCode >= 46 && event.keyCode <= 57)
                    ) {
                        CodeMirror.commands.autocomplete(editor, null, { completeSingle: false });
                    }
                });
            }

            if (type.toLowerCase() == "js") {
                jsCodeMirror = CodeMirror.fromTextArea(document.getElementById(editorId), {
                    mode: 'text/javascript',
                    smartIndent: true,
                    lineNumbers: true,
                    lineWrapping: true,
                    matchBrackets: true,
                    autoCloseBrackets: true,
                    autoRefresh: true,
                    foldGutter: true,
                    gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                    extraKeys: { "Alt-F": "findPersistent" }
                });
                jsCodeMirror.on("blur", function () {
                    SetFieldValue(editorId, jsCodeMirror.getValue());
                    UpdateFieldArray(editorId, GetFieldsRowNo(editorId), $("#" + editorId).val(), "parent", "");
                });
                jsCodeMirror.on("keyup", function (editor, event) {
                    if (
                        !(event.ctrlKey) &&
                        (event.keyCode >= 65 && event.keyCode <= 90) ||
                        (event.keyCode >= 97 && event.keyCode <= 122) ||
                        (event.keyCode >= 46 && event.keyCode <= 57)
                    ) {
                        CodeMirror.commands.autocomplete(editor, null, { completeSingle: false });
                    }
                });
            }

            $("#" + editorId).addClass("CodeMirrorApplied");
        }   
    });
}

function addCSSnJSEditorWRTdata() {
    $.each($(".formGridRow"), function () {
        var type = $(this).find("[id^='filetype']").val().toLowerCase();
        var textAreaObj = $(this).find('textarea[id^="css_js"]');
        var editorId = textAreaObj.attr('id');

        if (type == "css") {
            $(this).find('label[for^="css_js"]').text("Css");
            cssCodeMirror = CodeMirror.fromTextArea(textAreaObj[0], {
                mode: 'text/css',
                smartIndent: true,
                lineNumbers: true,
                lineWrapping: true,
                matchBrackets: true,
                autoCloseBrackets: true,
                autoRefresh: true,
                foldGutter: true,
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                extraKeys: { "Alt-F": "findPersistent" }
            });
            cssCodeMirror.on("blur", function () {
                SetFieldValue(editorId, cssCodeMirror.getValue());
                UpdateFieldArray(editorId, GetFieldsRowNo(editorId), $("#" + editorId).val(), "parent");
            });
            cssCodeMirror.on("keyup", function (editor, event) {
                if (
                    !(event.ctrlKey) &&
                    (event.keyCode >= 65 && event.keyCode <= 90) ||
                    (event.keyCode >= 97 && event.keyCode <= 122) ||
                    (event.keyCode >= 46 && event.keyCode <= 57)
                ) {
                    CodeMirror.commands.autocomplete(editor, null, { completeSingle: false });
                }
            });
            cssCodeMirror.getDoc().setValue(textAreaObj.val());
        }

        if (type == "js") {
            $(this).find('label[for^="css_js"]').text("Js");
            jsCodeMirror = CodeMirror.fromTextArea(textAreaObj[0], {
                mode: 'text/javascript',
                smartIndent: true,
                lineNumbers: true,
                lineWrapping: true,
                matchBrackets: true,
                autoCloseBrackets: true,
                autoRefresh: true,
                foldGutter: true,
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                extraKeys: { "Alt-F": "findPersistent" }
            });
            jsCodeMirror.on("blur", function () {
                SetFieldValue(editorId, jsCodeMirror.getValue());
                UpdateFieldArray(editorId, GetFieldsRowNo(editorId), $("#" + editorId).val(), "parent");
            });
            jsCodeMirror.on("keyup", function (editor, event) {
                if (
                    !(event.ctrlKey) &&
                    (event.keyCode >= 65 && event.keyCode <= 90) ||
                    (event.keyCode >= 97 && event.keyCode <= 122) ||
                    (event.keyCode >= 46 && event.keyCode <= 57)
                ) {
                    CodeMirror.commands.autocomplete(editor, null, { completeSingle: false });
                }
            });
            jsCodeMirror.getDoc().setValue(textAreaObj.val());
        }
    });
}

function loadcontentsFromFile() {
    let htmlFileName = "";
    if (htmlObj.template.flag && !+recordid) {
        htmlFileName = `${htmlObj.template.name}.html`;
        cssFileName.push(`${htmlObj.template.name}.css`);
        jsFileName.push(`${htmlObj.template.name}.js`);
    } else {
        htmlFileName = `${GetFieldValue("caption000F1").replace(/ /g, "_")}_${GetFieldValue("pageno000F1")}.html`;

        $('.formGridRow').each(function () {
            var fileExt = $(this).find("[id^='filetype']").val().toLowerCase();
            var fileName = `${$(this).find("[id^='filename']").val().replace(/ /g, "_")}_${GetFieldValue("pageno000F1")}.${fileExt}`;

            if (fileExt == "css") {
                cssFileName.push(fileName);
            }
            else if (fileExt == "js") {
                jsFileName.push(fileName);
            }
        });
    }
    try {
        $.ajax({
            url: "tstruct.aspx/renderHtmlPagesFiles",
            type: "POST",
            cache: false,
            async: false,            
            data: JSON.stringify({
                htmlFileName, cssFileName, jsFileName, getTemplate: htmlObj.template.flag
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //console.log("success", data);
                onSuccessFileLoad(data.d);
                cssFileName = [], jsFileName = [];
            },
            error: function (error) {
                ShowDialog("error", error);
            }
        });
    }
    catch (e) {
        ShowDialog("error", "");
    }
}

function onSuccessFileLoad(data) {
    // if ($("#html_editor_htmlsrc000F2").val(data.html).next(".CodeMirror").length > 0) {
    //     $('#html_editor_htmlsrc000F2').next('.CodeMirror')[0].CodeMirror.getDoc().setValue(data.html);
    // }
    $("#html_editor_htmlsrc000F2").val(data.html);
    if (htmlObj.template.flag && !+recordid) {
        htmlObj.template.files.html = htmlObj.template.files.html == "" ? data.html : htmlObj.template.files.html;

        /* @Description: Loading template's style or/and script file(s) available. 
            Every Template may have one script and one stylesheet only.
        */
        try {
            if (typeof data.css[cssFileName[0]] != "undefined" && data.css[cssFileName[0]] != "") {
                addCssJs("addCssRow", true);
                if (typeof htmlObj.template.grId != "undefined" && htmlObj.template.grId != "") {
                    $(`#${htmlObj.template.grId}`).find('label[for^="css_js"]').text("Css");
                    $(`#${htmlObj.template.grId}`).find("[id^='filetype']").val("Css");
                    var typeID = $(`#${htmlObj.template.grId}`).find("input[id^='filetype']").attr("id");
                    SetFieldValue(typeID, "Css");
                    UpdateFieldArray(typeID, GetFieldsRowNo(typeID), "Css", "parent");
                    formGridRowBlur($("#" + typeID));

                    $(`#${htmlObj.template.grId}`).find("[id^='filename']").val(htmlObj.template.name);
                    let _filenameId = $(`#${htmlObj.template.grId}`).find("[id^='filename']").attr("id");
                    SetFieldValue(_filenameId, htmlObj.template.name);
                    UpdateFieldArray(_filenameId, GetFieldsRowNo(_filenameId), htmlObj.template.name, "parent");
                    formGridRowBlur($("#" + _filenameId));

                    let editorid = $(`#${htmlObj.template.grId}`).find("textarea[id^='css_js_src']").attr('id');
                    $(`#${editorid}`).val(data.css[cssFileName[0]]);
                    if ($(`#${editorid}`).next(".CodeMirror").length > 0)
                        $(`#${editorid}`).next(".CodeMirror")[0].CodeMirror.setValue(data.css[cssFileName[0]]);                   
                    formGridRowBlur($("#" + editorid));
                }
            }
            if (typeof data.js[jsFileName[0]] != "undefined" && data.js[jsFileName[0]] != "") {
                addCssJs("addJsRow", true);
                if (typeof htmlObj.template.grId != "undefined" && htmlObj.template.grId != "") {
                    $(`#${htmlObj.template.grId}`).find('label[for^="css_js"]').text("Js");
                    $(`#${htmlObj.template.grId}`).find("[id^=filetype]").val("Js");
                    var typeID = $(`#${htmlObj.template.grId}`).find("input[id^='filetype']").attr("id");
                    SetFieldValue(typeID, "Js");
                    UpdateFieldArray(typeID, GetFieldsRowNo(typeID), "Js", "parent");
                    formGridRowBlur($("#" + typeID));

                    $(`#${htmlObj.template.grId}`).find("[id^='filename']").val(htmlObj.template.name);
                    let _filenameId = $(`#${htmlObj.template.grId}`).find("[id^='filename']").attr("id");
                    SetFieldValue(_filenameId, htmlObj.template.name);
                    UpdateFieldArray(_filenameId, GetFieldsRowNo(_filenameId), htmlObj.template.name, "parent");
                    formGridRowBlur($("#" + _filenameId));

                    let editorid = $(`#${htmlObj.template.grId}`).find("textarea[id^='css_js_src']").attr('id');
                    $(`#${editorid}`).val(data.js[jsFileName[0]]);
                    if ($(`#${editorid}`).next(".CodeMirror").length > 0)
                        $(`#${editorid}`).next(".CodeMirror")[0].CodeMirror.setValue(data.js[jsFileName[0]]);
                    formGridRowBlur($("#" + editorid));

                    if (!isLoadDataRow) {
                        let fields = GetGridFields("3");
                        if (typeof wsPerfEnabled != "undefined" && wsPerfEnabled)
                            CallEvaluateOnAddPerf("3", "002", fields, "ToCheckAddRow");
                        else
                            CallEvaluateOnAdd("3", "002", fields, "ToCheckAddRow");
                    }
                }
            }
        } catch (error) {
            showAlertDialog("error", error.Message);
        }
    }
    else {
        $('.formGridRow').each(function () {
            var fileExt = $(this).find("[id^='filetype']").val().toLowerCase();
            var fileName = `${$(this).find("[id^='filename']").val().replace(/ /g, "_")}_${GetFieldValue("pageno000F1")}.${fileExt}`;
            var editorid = $(this).find("textarea[id^='css_js_src']").attr('id');
            if (fileExt == "css") {
                if (typeof data.css[fileName] != "undefined") {
                    $("#" + editorid).val(data.css[fileName]);
                    if ($("#" + editorid).next(".CodeMirror").length > 0)
                        $("#" + editorid).next(".CodeMirror")[0].CodeMirror.setValue(data.css[fileName]);
                }
            }
            else if (fileExt == "js") {
                if (typeof data.js[fileName] != "undefined") {
                    $("#" + editorid).val(data.js[fileName]);
                    if ($("#" + editorid).next(".CodeMirror").length > 0)
                        $("#" + editorid).next(".CodeMirror")[0].CodeMirror.setValue(data.js[fileName]);
                }
            }
        });
    }
}

function createHtmlEditor(editorId) {
    var opts = {
        mode: 'htmlmixed',
        htmlMode: true,
        smartIndent: true,
        lineNumbers: true,
        matchBrackets: true,
        autoCloseBrackets: true,
        lineWrapping: true,
        autoRefresh: true,
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        extraKeys: { "Alt-F": "findPersistent" }
    };

    loadAndCall({
        files: {
            css: ["/Css/codemirror.css",
                "/ThirdParty/codemirror/addon/hint/show-hint.css",
                "/ThirdParty/codemirror/addon/fold/foldgutter.css",
                "/ThirdParty/codemirror/addon/dialog/dialog.css",
                "/ThirdParty/codemirror/addon/search/matchesonscrollbar.css"
            ],
            js: [
                "/Js/codemirror/mode/htmlmixed.js",
                "/Js/codemirror/mode/css.js",
                "/Js/codemirror/mode/javascript.js",
                "/Js/codemirror/mode/xml.js",
                "/ThirdParty/codemirror/addon/edit/matchbrackets.js",
                "/ThirdParty/codemirror/addon/edit/closebrackets.js",
                "/ThirdParty/codemirror/addon/hint/show-hint.js",
                "/ThirdParty/codemirror/addon/hint/html-hint.js",
                "/ThirdParty/codemirror/addon/hint/css-hint.js",
                "/ThirdParty/codemirror/addon/hint/javascript-hint.js",
                "/ThirdParty/codemirror/addon/hint/xml-hint.js",
                "/ThirdParty/codemirror/addon/fold/foldgutter.js",
                "/ThirdParty/codemirror/addon/fold/foldcode.js",
                "/ThirdParty/codemirror/addon/display/autorefresh.js",
                "/ThirdParty/codemirror/addon/dialog/dialog.js",
                "/ThirdParty/codemirror/addon/search/searchcursor.js",
                "/ThirdParty/codemirror/addon/search/search.js",
                "/ThirdParty/codemirror/addon/search/matchesonscrollbar.js",
                "/ThirdParty/codemirror/addon/search/match-highlighter.js",
                "/ThirdParty/codemirror/addon/search/jump-to-line.js",
            ]
        },
        callBack() {
            htmlCodeMirror = CodeMirror.fromTextArea(document.getElementById(editorId), opts);
            htmlCodeMirror.on("blur", function () {
                var htmlContent = htmlCodeMirror.getValue();
                htmlContent = htmlContent.replace(/&/g, '&amp;');
                var domParser = new DOMParser();
                var dom = "";
                if(htmlContent.trim() != ""){
                    dom = domParser.parseFromString(htmlContent, 'text/html');

                    if (dom && dom.getElementsByTagName("parsererror").length > 0) {
                        var errorMessage = checkErrorXML(dom.getElementsByTagName("parsererror")[0]);
                        console.log(dom);
                        showAlertDialog("error", errorMessage);
                        $("#wizardNextbtn").attr("onclick", "showAlertDialog(\"error\", \"Error in HTML..," + errorMessage + "\");");
                    }
                    else if (dom && dom.getElementsByTagName("head").length == 0) {
                        console.log(dom);
                        showAlertDialog("error", "Head tag doesn't exists.");
                        $("#wizardNextbtn").attr("onclick", "showAlertDialog(\"error\", \"Error in HTML..!, Head tag doesn't exists.\");");
                    }
                    else if (dom && dom.getElementsByTagName("body").length == 0) {
                        console.log(dom);
                        showAlertDialog("error", "Body tag doesn't exists.");
                        $("#wizardNextbtn").attr("onclick", "showAlertDialog(\"error\", \"Error in HTML..!, Body tag doesn't exists.\")");
                    }
                    else {
                        // $("#wizardNextbtn").attr("onclick", "FormSubmit()");
                        $("#wizardNextbtn").attr("onclick", wizardSaveActionBtn);
                        SetFieldValue(editorId, htmlContent);
                        UpdateFieldArray(editorId, "000", htmlContent, "parent");
                    }
                }
                else {
                    $("#wizardNextbtn").attr("onclick", "showAlertDialog(\"error\", \"HTML is empty..!\")");  
                }
            });

            htmlCodeMirror.on('keyup', function (editor, event) {
                if (
                    !(event.ctrlKey) &&
                    (event.keyCode >= 65 && event.keyCode <= 90) ||
                    (event.keyCode >= 97 && event.keyCode <= 122) ||
                    (event.keyCode >= 46 && event.keyCode <= 57)
                ) {
                    CodeMirror.commands.autocomplete(editor, null, { completeSingle: false });
                }
            });
            htmlCodeMirror.getDoc().setValue($("#" + editorId).val());
            try {
                setTimeout(function () {
                    setTimeout(function () {
                        $(".CodeMirror-hscrollbar").css({ "right": "0px", "left": "30px" });
                        $(".CodeMirror-sizer").css({ "border-right-width": "40px" });
                        $(".CodeMirror-gutters").css({ "left": "1.9e-06px" });
                        $(".CodeMirror-line span").css({ "padding-right": "1px" });
                        $(".CodeMirror.cm-s-default.CodeMirror-wrap").addClass('w-100');
                        document.querySelectorAll('.CodeMirror-line [cm-text]').forEach(element => {
                            element.parentNode.removeChild(element);
                        });
                    }, 0);
                }, 200);
            } catch (ex) { }

            if (recordid != "0") {
                addCSSnJSEditorWRTdata();
            }

            var xt = "", h3OK = 1
            function checkErrorXML(x) {
                xt = "";
                h3OK = 1;
                checkXML(x);
                return xt;
            }

            function checkXML(n) {
                var l, i, nam;
                nam = n.nodeName;
                if (nam == "h3") {
                    if (h3OK == 0) {
                        return;
                    }
                    h3OK = 0;
                }
                if (nam == "#text") {
                    xt = xt + n.nodeValue + "\n";
                }
                l = n.childNodes.length;
                for (i = 0; i < l; i++) {
                    checkXML(n.childNodes[i]);
                }
            }
        }
    });

}

function addCssJs(type, isfromfile = false) {
    var textAreaObj, formRowId;
    if (!$("#DivFrame3").is(":visible")) {
        if (recordid != "0" && DCHasDataRows[2].toLowerCase() == "false") {
            $("[id^=gridAddBtn]").trigger("click");
        }
        $("[id^=gridAddBtnAdd]").addClass('d-none');
        $("#DivFrame3").show();
        $("#DivFrame3").removeClass('d-none');
        formRowId = "gridrowWrap3-001";
    }
    else {
        $("[id^=gridAddBtnAdd]").trigger("click");
        var curDcNo = $("[id^=gridAddBtnAdd]").attr("onclick").split(",")[1];
        var nxtRowNo = $("[id^=gridAddBtnAdd]").attr("onclick").split(",")[2];
        nxtRowNo = nxtRowNo.replace(/'/g, '').trim();
        formRowId = "gridrowWrap" + curDcNo + "-" + nxtRowNo;
        $("[id^=gridAddBtnAdd]").addClass('d-none');
    }

    textAreaObj = $("#" + formRowId).find('textarea[id^="css_js"]');
    if (!$(textAreaObj).hasClass("CodeMirrorApplied")) {
        if (type == "addCssRow") {
            $("#" + formRowId).find('label[for^="css_js"]').text("Css");
            $("#" + formRowId).find("[id^='filetype']").val("Css");
            var typeID = $("#" + formRowId).find("input[id^='filetype']").attr("id");
            SetFieldValue(typeID, "Css");
            UpdateFieldArray(typeID, GetFieldsRowNo(typeID), "Css", "parent");
            applyCodeMirror(textAreaObj.attr("id"), "css");
            if (isfromfile == false) {
                setTimeout(function () {
                    formGridRowBlur($("#" + typeID));
                }, 0)
            }
        }
        else if (type == "addJsRow") {
            $("#" + formRowId).find('label[for^="css_js"]').text("Js");
            $("#" + formRowId).find("[id^=filetype]").val("Js");
            var typeID = $("#" + formRowId).find("input[id^='filetype']").attr("id");
            SetFieldValue(typeID, "Js");
            UpdateFieldArray(typeID, GetFieldsRowNo(typeID), "Js", "parent");
            applyCodeMirror(textAreaObj.attr("id"), "js");
            if (isfromfile == false) {
                setTimeout(function () {
                    formGridRowBlur($("#" + typeID));
                }, 0)
            }
        }
    }

    $("html").on("blur", "[id^='filename']", function () {
        var name = $(this).val();
        var regex = /^[0-9a-zA-Z\_ .]+$/; 
        if (!regex.test(name)) {
            if(name == ""){
                ShowDialog("error", "FileName is empty");                
            }
            else {
                ShowDialog("error", "FileName is invalid");
            }
            validateHtmlPage = false;
        }
        else
            validateHtmlPage = true;
    });
    
    //$("#wizardBodyFooterWrapper .wizardBodyContent").scrollTop($("#wizardBodyFooterWrapper .wizardBodyContent")[0].scrollHeight);

    if (htmlObj.template.flag) {
        htmlObj.template.grId = formRowId;
    }
}

function previewHtmlPage() {
    if (!+recordid) {
        showAlertDialog("warning", "Please save this HTML Page");
        return;
    }  
    createPopup(`htmlPages.aspx?load=${GetFieldValue("pageno000F1") || ""}`, undefined, undefined, undefined, () => {
        setTimeout(() => {
            $("#popupIframeRemodal:visible").css("pointer-events", "none");
        }, 0);
    });
}

function selectTemplate() {
    try {
        let templateObj = {
            "options" : ``,
        };
        ASB.WebService.ListTemplates(".html",
            (success) => {
                if (success != null && success != "" && typeof success != "undefined") {
                    let templatesList = Object.values(success).map(tName => { return tName.slice(0, tName.length - 5) });
                    templateObj.label = `Select a sample template`;
                    $.each(templatesList, function (ind, cap) {
                        templateObj.options += `<a href="#" class="list-group-item list-group-item-action tempSelected">${cap}</a>`;
                    });
                } else {
                    templateObj.label = `No templates available...!`;
                }
            }, (error) => { }
        );

        templateObj.modalBody = `
        <div class="axpTempaltes">
            <label class="form-label template-label">
                <!-- ${templateObj.label} -->
            </label>
            <div class="list-group template-list">    
                <!-- ${templateObj.options} -->
            </div>
        </div>`;

        displayBootstrapModalDialog("Upload Template", "sm", "300px", "", templateObj.modalBody, false, () => {
            $("#divModalUploadTemplate.custom-dialog.modal .modal-dialog").css({ "width": "450px" }).find("#htmlContentOriFrameUrl").css({ "overflow-y": "auto" });
            $(".template-label").append(templateObj.label);
            $(".template-list").append(templateObj.options);
            $(document).off("click").on("click", ".tempSelected", (ele) => {
                try {
                    SetFieldValue(`${htmlObj.fields.template}`, ele.currentTarget.innerHTML);
                    UpdateFieldArray(`${htmlObj.fields.template}`, GetFieldsRowNo(`${htmlObj.fields.template}`), ele.currentTarget.innerHTML, "parent");
                    AxDevStudioHelper(transid);
                    closeModalDialog();
                } catch (ex) { }
            });
        }, () => { });
    } catch (error) { }
}
