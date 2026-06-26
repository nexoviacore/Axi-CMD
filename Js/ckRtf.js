//<Module>  CK Editor on TextAreas  </Module>
//<Author>  Prashik  </Author>
//<Description> New CK Editor module with Normal(rtf_) and Minimal(rtfm_) Rich Text Box input support </Description>
function allRtfTextAreas() {
    var A = document.getElementsByTagName('textarea');
    var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));
    for (var B = 0; B < A.length; B++) {
        let config = null;
        function addCollapseFeature(editor) {
            var toolbar = editor.container.findOne('.cke_top');
            function applyCollapse() {
                var toolbox = toolbar.findOne('.cke_toolbox');
                if (!toolbox) return;
                toolbar.removeStyle('height');
                toolbar.removeStyle('overflow');
                var oldBtn = toolbar.findOne('.cke_collapse_btn');
                if (oldBtn) oldBtn.remove();
                var rows = toolbox.find('.cke_toolbar');
                if (!rows || rows.count() === 0) return;
                var isWrapped = false;
                var firstTop = rows.getItem(0).$.offsetTop;
                for (var i = 1; i < rows.count(); i++) {
                    if (rows.getItem(i).$.offsetTop > firstTop) {
                        isWrapped = true;
                        break;
                    }
                }
                if (!isWrapped) {
                    return;
                }
                var firstRowHeight = rows.getItem(0).$.offsetHeight;
                toolbar.setStyle('position', 'relative');
                toolbar.setStyle('height', firstRowHeight + 'px');
                toolbar.setStyle('overflow', 'hidden');
                toolbar.setStyle('padding-right', '30px');
                var toggleBtn = CKEDITOR.dom.element.createFromHtml(
                    '<span class="cke_collapse_btn" style="position:absolute;right:8px;top:6px;cursor:pointer;color:#007bff;font-weight:bold;">[+]</span>'                   
                );
                var expanded = false;
                toggleBtn.on('click', function () {
                    if (expanded) {
                        toolbar.setStyle('height', firstRowHeight + 'px');
                        toolbar.setStyle('overflow', 'hidden');
                        toggleBtn.setText('[+]');
                    } else {
                        toolbar.removeStyle('height');
                        toolbar.removeStyle('overflow');
                        toggleBtn.setText('[-]');
                    }
                    expanded = !expanded;
                });
                toolbar.append(toggleBtn);
            }
            setTimeout(applyCollapse, 100);
            window.addEventListener('resize', function () {
                applyCollapse();
            });
        }
        if (A[B].id.startsWith("rtf_") || GetDWBFieldType(GetFieldsName(A[B].id)) == "Rich Text") {
            config = {
                toolbar: [
                    { name: 'document', items: ['Source'] },
                    { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
                    { name: 'editing', items: ['SpellChecker', 'Scayt'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
                    { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                    { name: 'insert', items: ['Table', 'HorizontalRule', 'SpecialChar'] },
                    { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                    { name: 'colors', items: ['TextColor'] },
                    { name: 'tools', items: ['Maximize', 'Image'] }
                ],
                removeDialogTabs: 'image:advanced;link:advanced',
                filebrowserUploadUrl: url + '/ckImgUpload.ashx',
                filebrowserImageUploadUrl: url + '/ckImgUpload.ashx?transId=' + transid + '&fldName=' + A[B].id,
                filebrowserFlashUploadUrl: url + '/ckImgUpload.ashx',
                filebrowserUploadMethod: 'form',
                enterMode: CKEDITOR.ENTER_BR,
                image_previewText: ' ',
                tabSpaces: 0,
                width: '100%',
                on: {
                    instanceReady: function (evt) {
                        addCollapseFeature(evt.editor);
                    }
                }
            };
        }
        else if (A[B].id.startsWith("rtfm_")) {
            config = {
                toolbar: [
                    { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] },
                    ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
                    { name: 'basicstyles', items: ['Bold', 'Italic'] }
                ],
                enterMode: CKEDITOR.ENTER_BR,
                tabSpaces: 0,
                width: '100%'
            };
        }
        else if (A[B].id.startsWith("fr_rtf_")) {
            config = {
                toolbar: [
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript'] },
                    { name: 'colors', items: ['TextColor'] },
                    ['Cut', 'Copy', 'Paste', '-', 'Undo', 'Redo'],
                    { name: 'document', items: ['NewPage', 'Preview', '-', 'Source'] }
                ],
                enterMode: CKEDITOR.ENTER_BR,
                coreStyles_bold: { element: 'b', overrides: 'strong' },
                coreStyles_strike: { element: 'strike', overrides: 's' },
                coreStyles_italic: { element: 'i', overrides: 'em' },
                colorButton_foreStyle: { element: 'font', attributes: { 'color': '#(color)' } },
                autoParagraph: false,
                entities: false,
                basicEntities: false,
                tabSpaces: 0,
                width: '100%'
            };
        }
        if (config) {
            CKEDITOR.replace(A[B], config).on("blur", function () { ShowdivContentCK(this, true); });
        }
    }
}

// Same function but it will called only for tabbed DC's
function allRtfTextAreasTab(tabNo) {
    var divID = "divDc" + tabNo;
    if (document.getElementById(divID) != null) {
        var url = location.origin + location.pathname.substr(0, location.pathname.indexOf('aspx'));
        var A = document.getElementById(divID).getElementsByTagName('textarea');
        function addCollapseFeature(editor) {
            var toolbar = editor.container.findOne('.cke_top');
            function applyCollapse() {
                var toolbox = toolbar.findOne('.cke_toolbox');
                if (!toolbox) return;
                toolbar.removeStyle('height');
                toolbar.removeStyle('overflow');
                var oldBtn = toolbar.findOne('.cke_collapse_btn');
                if (oldBtn) oldBtn.remove();
                var rows = toolbox.find('.cke_toolbar');
                if (!rows || rows.count() === 0) return;
                var isWrapped = false;
                var firstTop = rows.getItem(0).$.offsetTop;
                for (var i = 1; i < rows.count(); i++) {
                    if (rows.getItem(i).$.offsetTop > firstTop) {
                        isWrapped = true;
                        break;
                    }
                }
                if (!isWrapped) {
                    return;
                }
                var firstRowHeight = rows.getItem(0).$.offsetHeight;
                toolbar.setStyle('position', 'relative');
                toolbar.setStyle('height', firstRowHeight + 'px');
                toolbar.setStyle('overflow', 'hidden');
                toolbar.setStyle('padding-right', '30px');
                var toggleBtn = CKEDITOR.dom.element.createFromHtml(
                    '<span class="cke_collapse_btn" style="position:absolute;right:8px;top:6px;cursor:pointer;color:#007bff;font-weight:bold;">[+]</span>'
                );
                var expanded = false;
                toggleBtn.on('click', function () {
                    if (expanded) {
                        toolbar.setStyle('height', firstRowHeight + 'px');
                        toolbar.setStyle('overflow', 'hidden');
                        toggleBtn.setText('[+]');
                    } else {
                        toolbar.removeStyle('height');
                        toolbar.removeStyle('overflow');
                        toggleBtn.setText('[-]');
                    }
                    expanded = !expanded;
                });
                toolbar.append(toggleBtn);
            }
            setTimeout(applyCollapse, 100);
            window.addEventListener('resize', function () {
                applyCollapse();
            });
        }
        for (var B = 0; B < A.length; B++) {
            let config = null;
            if (A[B].id.startsWith("rtf_") || GetDWBFieldType(GetFieldsName(A[B].id)) == "Rich Text") {
                config = {
                    toolbar: [
                        { name: 'document', items: ['Source'] },
                        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
                        { name: 'editing', items: ['SpellChecker', 'Scayt'] },
                        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
                        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                        { name: 'insert', items: ['Table', 'HorizontalRule', 'SpecialChar'] },
                        { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                        { name: 'colors', items: ['TextColor'] },
                        { name: 'tools', items: ['Maximize', 'Image'] }
                    ],
                    removeDialogTabs: 'image:advanced;link:advanced',
                    filebrowserUploadUrl: url + '/ckImgUpload.ashx',
                    filebrowserImageUploadUrl: url + '/ckImgUpload.ashx?transId=' + transid + '&fldName=' + A[B].id,
                    filebrowserFlashUploadUrl: url + '/ckImgUpload.ashx',
                    filebrowserUploadMethod: 'form',
                    enterMode: CKEDITOR.ENTER_BR,
                    image_previewText: ' ',
                    tabSpaces: 0,
                    width: '100%',
                    on: {
                        instanceReady: function (evt) {
                            addCollapseFeature(evt.editor);
                        }
                    }
                };
            }
            else if (A[B].id.startsWith("rtfm_")) {
                config = {
                    toolbar: [
                        { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] },
                        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
                        { name: 'basicstyles', items: ['Bold', 'Italic'] }
                    ],
                    enterMode: CKEDITOR.ENTER_BR,
                    tabSpaces: 0,
                    width: '100%'
                };
            }
            else if (A[B].id.startsWith("fr_rtf_")) {
                config = {
                    toolbar: [
                        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript'] },
                        { name: 'colors', items: ['TextColor'] },
                        ['Cut', 'Copy', 'Paste', '-', 'Undo', 'Redo'],
                        { name: 'document', items: ['NewPage', 'Preview', '-', 'Source'] }
                    ],
                    enterMode: CKEDITOR.ENTER_BR,
                    coreStyles_bold: { element: 'b', overrides: 'strong' },
                    coreStyles_strike: { element: 'strike', overrides: 's' },
                    coreStyles_italic: { element: 'i', overrides: 'em' },
                    colorButton_foreStyle: { element: 'font', attributes: { 'color': '#(color)' } },
                    autoParagraph: false,
                    entities: false,
                    basicEntities: false,
                    tabSpaces: 0,
                    width: '100%'
                };
            }
            if (config) {
                CKEDITOR.replace(A[B], config).on("blur", function () { ShowdivContentCK(this, true); });
            }
        }
    }
}