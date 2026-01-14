; $(function () {
    try {
        ib = new IviewBuilder();
    } catch (ex) {
        console.error(ex);
        if (showAlertDialog) { 
            showAlertDialog("error", ex.message);
        } else {
            alert(ex.message)
        }
     }
});

class IviewBuilder {
    constructor(obj, isMobile) {
        if (IviewBuilder.exists) {
            return IviewBuilder.instance;
        }
        IviewBuilder.instance = this;
        IviewBuilder.exists = true;
        this.shadowStructure = { ...obj };
        this.stage = new Stages();
        return this;
    }
    _getStages() {
        return this.stage;
    }
    
    _preInit() {
        let _this = this;
        _this.isMobile && $(_this.stage.body).addClass("isMobile");
        
        switch (_this.stage.propertySheet.type) {
            case "div":
                $(_this.stage.body).addClass("isNavPropDiv");
                break;
            case "popup":
                $(_this.stage.body).addClass("isPropPopUp");
                break;
        }
    }
    
    _generateData() {
        let _this = this;
        return [_this.columns.FieldName.map((f, i) => {
            switch (_this.stage.propertySheet.type) { 
                case "div":
                    return `
                    <a class="btn btn-default dropdown-toggle fa fa-ellipsis-h col-xs-12 col-sm-12 col-md-12 col-lg-12" data-toggle="dropdown" href="#" title=" Open ${_this.columns.HeaderText[i]} Properties" data-toggle="tooltip"data-field="${f}" onclick="${this.stage.propertySheet.call[this.stage.propertySheet.type].create}">
                    </a>
                    `;
                case "popup":
                    return `
                    <div class="dropdown">
                        <span class="btn btn-default dropdown-toggle fa fa-ellipsis-v" data-toggle="dropdown">
                        </span>
                        <ul class="dropdown-menu" role="menu" aria-labelledby="menu1">
                            ${
                                (f != "rowno")
                                ?
                                    _this.stage.propertySheet.sheets.filter((o) => o.propertyIs == "column").map((o, oi) => {
                                        return `
                                        <li role="presentation">
                                            <a role="menuitem" class="${o.icon}" tabindex="-1" href="#" title="${o.title}" data-toggle="tooltip" data-type="${o.type}" data-field="${f}" data-property-is="${o.propertyIs}" onclick="${o.function}">
                                                ${o.title}
                                            </a>
                                        </li>
                                        <!--<a href="#" class="btn btn-default ${o.icon}" aria-label="Left Align" title="${o.title}" data-toggle="tooltip" data-type="${o.type}" data-field="${f}" data-property-is="${o.propertyIs}" onclick="${o.function}">
                                        </a>
                                        <br />
                                        <span>${o.title}</span>-->
                                    `;
                                    }).join("")
                                :
                                    ""
                            }
                        </ul>
                    </div>
                    `;
            }
        })];
    }
    AxClColors = AxClColors;
    showAlertDialog = showAlertDialog;
    callParentNew = callParentNew;
    displayBootstrapModalDialog = displayBootstrapModalDialog;
    closeModalDialog = closeModalDialog;
    createTheEditor = createTheEditor;
    lcm = this.callParentNew("lcm");
}

class Stages {
    constructor() {
        let _this = this;
        
        this.body = "body";

        this.AxClColors = new IviewBuilder().AxClColors;
        
        this.colorPicker = {
            icon: "fa fa-ellipsis-h",
            title: "Color Picker",
            containerClass: "colorPickerInput",
            call: {
                // toggle: "new PropertySheet().toggleColorPicker($(this));"
            }
        };
        this.fontPicker = {
            icon: "fa fa-ellipsis-h",
            title: "Font",
            modalTitle: "Font Picker",
            stage: "body",
            containerId: "fontPickerContainer",
            fontPickerSampleId: "fontPickerSampleText",
            call: {
                generate: "new PropertySheet().generateFontPicker($(this));",
                generatePreview: "new PropertySheet()._generateFontPickerPreview($(this));",
                save: "new PropertySheet().saveFontPicker($(this));"
            },
            objects: {
                fontFamily: "fontFamily",
                fontSize: "fontSize",
                fontEffects: "fontEffects",
                fontColor: "fontColor",
                fontScript: "fontScript"
            },
            fontFamily: [
                { value: "Arial", caption: "Arial", cssProp: "font-family", cssValue: "Arial,Helvetica,sans-serif" },
                { value: "Arial Black", caption: "Arial Black", cssProp: "font-family", cssValue: "Arial Black,Gadget,sans-serif" },
                { value: "Comic Sans MS", caption: "Comic Sans MS", cssProp: "font-family", cssValue: "Comic Sans MS,cursive" },
                { value: "Courier New", caption: "Courier New", cssProp: "font-family", cssValue: "Courier New,Courier,monospace" },
                { value: "Georgia", caption: "Georgia", cssProp: "font-family", cssValue: "Georgia,serif" },
                { value: "Impact", caption: "Impact", cssProp: "font-family", cssValue: "Impact,Charcoal,sans-serif" },
                { value: "Lucida Console", caption: "Lucida Console", cssProp: "font-family", cssValue: "Lucida Console,Monaco,monospace" },
                { value: "Lucida Sans Unicode", caption: "Lucida Sans Unicode", cssProp: "font-family", cssValue: "Lucida Sans Unicode,Lucida Grande,sans-serif" },
                { value: "Palatino Linotype", caption: "Palatino Linotype", cssProp: "font-family", cssValue: "Palatino Linotype,Book Antiqua,Palatino,serif" },
                { value: "Tahoma", caption: "Tahoma", cssProp: "font-family", cssValue: "Tahoma,Geneva,sans-serif" },
                { value: "Times New Roman", caption: "Times New Roman", cssProp: "font-family", cssValue: "Times New Roman,Times,serif" },
                { value: "Trebuchet MS", caption: "Trebuchet MS", cssProp: "font-family", cssValue: "Trebuchet MS,Helvetica,sans-serif" },
                { value: "Verdana", caption: "Verdana", cssProp: "font-family", cssValue: "Verdana,Geneva,sans-serif" },
                { value: "Gill Sans", caption: "Gill Sans", cssProp: "font-family", cssValue: "Gill Sans,Geneva,sans-serif" }
            ],
            fontSize: [
                { value: "8", caption: "8pt", cssProp: "font-size", cssValue: "10.67px" },
                { value: "9", caption: "9pt", cssProp: "font-size", cssValue: "12px" },
                { value: "10", caption: "10pt", cssProp: "font-size", cssValue: "13.34px" },
                { value: "11", caption: "11pt", cssProp: "font-size", cssValue: "14.66px" },
                { value: "12", caption: "12pt", cssProp: "font-size", cssValue: "16px" },
                { value: "14", caption: "14pt", cssProp: "font-size", cssValue: "18.66px" },
                { value: "16", caption: "16pt", cssProp: "font-size", cssValue: "21.32px" },
                { value: "18", caption: "18pt", cssProp: "font-size", cssValue: "24px" },
                { value: "20", caption: "20pt", cssProp: "font-size", cssValue: "26.68px" },
                { value: "22", caption: "22pt", cssProp: "font-size", cssValue: "29.32px" },
                { value: "24", caption: "24pt", cssProp: "font-size", cssValue: "32px" },
                { value: "26", caption: "26pt", cssProp: "font-size", cssValue: "34.64px" },
                { value: "28", caption: "28pt", cssProp: "font-size", cssValue: "37.36px" },
                { value: "36", caption: "36pt", cssProp: "font-size", cssValue: "48px" },
                { value: "48", caption: "48pt", cssProp: "font-size", cssValue: "64px" },
                { value: "72", caption: "72pt", cssProp: "font-size", cssValue: "96px" },
            ],
            fontEffects: [
                { caption: "Bold", cssProp: "font-weight", cssValue: "bold", selectedValue: { checked: "True", unchecked: "False" } },
                { caption: "Italic", cssProp: "font-style", cssValue: "italic", selectedValue: { checked: "True", unchecked: "False" } },
                { caption: "Underline", cssProp: "text-decoration", cssValue: "underline", selectedValue: { checked: "True", unchecked: "False" } },
                { caption: "Strikeout", cssProp: "text-decoration", cssValue: "line-through", selectedValue: { checked: "True", unchecked: "False" } },
            ],
            fontColor: [
                { value: "clBlack", caption: "Black", cssProp: "color", cssValue: this.AxClColors["clBlack"] },
                { value: "clMaroon", caption: "Maroon", cssProp: "color", cssValue: this.AxClColors["clMaroon"] },
                { value: "clGreen", caption: "Green", cssProp: "color", cssValue: this.AxClColors["clGreen"] },
                { value: "clOlive", caption: "Olive", cssProp: "color", cssValue: this.AxClColors["clOlive"] },
                { value: "clNavy", caption: "Navy", cssProp: "color", cssValue: this.AxClColors["clNavy"] },
                { value: "clPurple", caption: "Purple", cssProp: "color", cssValue: this.AxClColors["clPurple"] },
                { value: "clTeal", caption: "Teal", cssProp: "color", cssValue: this.AxClColors["clTeal"] },
                { value: "clGray", caption: "Gray", cssProp: "color", cssValue: this.AxClColors["clGray"] },
                { value: "clSilver", caption: "Silver", cssProp: "color", cssValue: this.AxClColors["clSilver"] },
                { value: "clRed", caption: "Red", cssProp: "color", cssValue: this.AxClColors["clRed"] },
                { value: "clLime", caption: "Lime", cssProp: "color", cssValue: this.AxClColors["clLime"] },
                { value: "clYellow", caption: "Yellow", cssProp: "color", cssValue: this.AxClColors["clYellow"] },
                { value: "clBlue", caption: "Blue", cssProp: "color", cssValue: this.AxClColors["clBlue"] },
                { value: "clFuchsia", caption: "Fuchsia", cssProp: "color", cssValue: this.AxClColors["clFuchsia"] },
                { value: "clAqua", caption: "Aqua", cssProp: "color", cssValue: this.AxClColors["clAqua"] },
                { value: "clWhite", caption: "White", cssProp: "color", cssValue: this.AxClColors["clWhite"] },
                { value: "clWindowText", caption: "Custom", cssProp: "color", cssValue: this.AxClColors["clWindowText"] }
            ],
            fontScript: [

            ]
        }
        this.expressionBuilder = {
            icon: "fa fa-ellipsis-h",
            title: "Expression",
            modalTitle: "Expression Builder",
            stage: "body",
            containerId: "expressionBuilderContainer",
            call: {
                generate: "new PropertySheet().generateExpressionBuilder($(this));",
                save: "new PropertySheet().saveExpressionBuilder($(this));",
            },
        }
        this.propertySheet = {
            parent: "#form1",
            containerId: "Wrapperpropsheet",
            propTableContentId: "propTableContent",
            type: "div",//popup/div
            call: {
                div: {
                    create: "new PropertySheet()._generatePropSheetAsNavDiv($(this));",
                },
                popup: {
                    save: "new PropertySheet().save($(this));",
                    close: "new PropertySheet().close($(this));"
                }
            },
            allSheetInit() {

                $("#propertySearchFld").on('keyup', function (e) {
                    var elem = $(this);
                    var enteredVal = elem.val().toLowerCase();
                    var nodata = '<tr class="noDatFoundTr"><td colspan="2" class="center">No data found</td></tr>';
                    $("#propTableContent table tbody .noDatFoundTr").remove();
                    $("#propTableContent table tr:not('.notSearchable')").each(function (index, el) {
                        var presTr = $(this);
                        var childTd = presTr.find('td:first');
                        if (enteredVal != "" && childTd.hasClass('subHeading')) {
                            presTr.hide();
                            return;
                        }
                        childTd.text().toLowerCase().indexOf(enteredVal) === -1 ? presTr.hide() : presTr.show();
                    });
                    if (elem.val() != "" && $("#propTableContent table tr:visible").length == 0) {

                        $("#propTableContent table tbody").append(nodata);
                    }
                });
                $("#propTableContent .propShtDataToggleIcon").on('click', function (e) {
                    var elem = $(this);
                    var target = elem.data('target');
                    $("#propTableContent table tr[data-group='" + target + "']:not('.notSearchable')").toggle();
                    if (elem.hasClass('icon-arrows-up')) {
                        elem.removeClass('icon-arrows-up').addClass('icon-arrows-down');
                    }
                    else if (elem.hasClass('icon-arrows-down')) {
                        elem.removeClass('icon-arrows-down').addClass('icon-arrows-up');
                    }
                    /* Act on the event */
                });
            }
        }
    }
}

class DataTableObj {
    constructor() {
        let _this = this;

        // let columns = new IviewBuilder()._getColumn();
        let stage = new IviewBuilder()._getStages();

        this.data = new IviewBuilder()._generateData();

        
        this.initComplete = function (settings, json) {
           
        };
        
    }
    
}

class PropertySheet {
    constructor() {
        if (PropertySheet.exists) {
            return PropertySheet.instance;
        }

        PropertySheet.instance = this;
        PropertySheet.exists = true;

        this.propSheet = "";//type of property sheet
        this.propColumn = "";//column name of opening property sheet

        // this.columns = new IviewBuilder()._getColumn();
        this.stage = new IviewBuilder()._getStages();

        this.fontPickerObj = {};

        this._initPropSheetContainer();

        return this;
    }
    _initPropSheetContainer() {
        let _this = this;
        switch (this.stage.propertySheet.type) {
            // case "div":
            //     _this._initPropSheetAsNavDiv();
            //     break;
            case "popup":
                _this._initPropSheetAsPopUp();
                break;
        }
    }
    _initPropSheetAsPopUp() {
        // $(this.body).addClass("isPropPopUp");
        $(this.stage.propertySheet.parent).append(`
        <div id="${this.stage.propertySheet.containerId}" style="display: block;">
            <div class='col s3 card hoverable scale-transition scale-out' id='propertySheet'>
                <div class='hpbHeaderTitle'>
                    <span class='icon-paint-roller'></span>
                    <span class='title'>Property Sheet</span>
                    <button title='Close' type='button' id='propertySrchCls' onclick='${this.stage.propertySheet.call[this.stage.propertySheet.type].close}' class='btn-flat waves-effect btn-floating pull-right'><i class='icon-arrows-remove'></i></button>
                    <button title='Save' type='button' onclick='${this.stage.propertySheet.call[this.stage.propertySheet.type].save}' id='updateWidget' class='btn-flat waves-effect btn-floating pull-right '><span class='icon-arrows-check'></span></button>
                    <div class='clear'></div>
                </div>
                <div id='propertySheetDataWrapper'>
                    <div class='clear'></div>
                    <div id='propertySearch'>
                        <input placeholder='Search...' type='text' id='propertySearchFld' class='normalFld searchFld'>
                        <span class='srchIcn icon-magnifier'></span>
                    </div>
                    <div class='posr' id='${this.stage.propertySheet.propTableContentId}'>
                        <!--My Name is Prashik-->
                    </div>
                </div>
            </div>
        </div>
        `);
    }
    _generatePropertySheet() {

        let propMainTitle = "";

        try {
            propMainTitle = this.stage.propertySheet.sheets.filter((opt) => opt.type == this.propSheet)[0].title;
        } catch (ex) { }

        if (!propMainTitle) {
            propMainTitle = this.propSheet;
        }

        let columnnCaption = "";

        if (this.propColumn) {
            try {
                columnnCaption = this.columns.HeaderText.filter((f, i) => { if (this.columns.FieldName[i] == this.propColumn) { return true } })[0];
            } catch (ex) { }

            if (!columnnCaption) {
                columnnCaption = this.propColumn;
            }
        }

        $("#Wrapperpropsheet #propertySheet .hpbHeaderTitle span.title").text(`${propMainTitle}${this.propColumn ? ` - ${columnnCaption}` : ``}`);


        let propColumnBuildObj = this.stage.propertySheet.sheets.filter((opt) => opt.type == this.propSheet && opt.propertyIs == "column");

        if (propColumnBuildObj && propColumnBuildObj.length > 0) {
            $(`#${this.stage.propertySheet.propTableContentId}`).html(`
            <table id="${propColumnBuildObj[0].name}" class='bordered' data-parent="addedPsWrapper" data-title="Add Property Sheet" data-prop-column="${this.propColumn}" data-prop-sheet="${this.propSheet}" style="min-width: 350px;">
                ${propColumnBuildObj[0].sheetMap.map((sheet) => {
                return `
                    <tr data-group='general'>
                    ${
                    sheet.type != "table"
                    ?
                    `
                        <td>${sheet.caption}${sheet.mandatory ? `<sup>*</sup>` : ``}</td>
                        <td>
                            ${this._generatePropertyInput(sheet, this.propColumn, propColumnBuildObj[0])}
                        </td>
                    `
                    :
                    `
                        <td colspan="2">
                            ${this._generatePropertyInput(sheet, this.propColumn, propColumnBuildObj[0])}
                        </td>
                    `
                    }
                    </tr>
                    `;
            }).join("")}
            </table>
            `);

            propColumnBuildObj[0].sheetInit();
        }

        this.stage.propertySheet.allSheetInit();
        
    }
    _generatePropertyInput(sheet, propColumn, propColumnBuildObj) {
        //text/number/checkbox/dropdown/expression/fontpicker/colorpicker/fieldmapper/addformat/deleteformat/deletehyperlink

        // debugger;

        let val = typeof sheet.value == "function" ? sheet.value(sheet, propColumn, propColumnBuildObj) : sheet.value;

        typeof val == "undefined" ? val = `` : ``;

        switch (sheet.type) {            
            case "expression":
                // border: 0px solid transparent;
                // box-shadow: none;
                // padding-left: 3px;
                let { expressionBuilder } = this.stage;
                return `
                <div class="${this.stage.propertySheet.type != "popup" ? `form-control` : ``}" style="${this.stage.propertySheet.type == "popup" ? `float: right;` : `border: 1px solid transparent; box-shadow: none; padding-left: 3px;`}">
                    <a href="#" class="btn btn-default ${expressionBuilder.icon}" aria-label="Right Align" title="${expressionBuilder.title}" data-toggle="tooltip" data-expression-builder-type="${sheet.expressionBuilderType}" data-value="${val}" onclick="${expressionBuilder.call.generate}">
                    </a>
                </div>
                `;
            case "fontpicker":
                let { fontPicker } = this.stage;
                return `
                <div class="${this.stage.propertySheet.type != "popup" ? `form-control` : ``}" style="${this.stage.propertySheet.type == "popup" ? `float: right;` : `border: 1px solid transparent; box-shadow: none; padding-left: 3px;`}">
                    <a href="#" class="btn btn-default ${fontPicker.icon}" aria-label="Right Align" title="${fontPicker.title}" data-toggle="tooltip" data-font-picker-type="${sheet.fontPickerType}" data-value="${val}" onclick="${fontPicker.call.generate}">
                    </a>
                </div>
                `;
            case "colorpicker":
                let { colorPicker } = this.stage;
                let colorValue = "";
                // colorValue = "#01579b";

                colorValue = this._parseHexAndDelphiColors(val, true);

                return `
                <div class="" style="${this.stage.propertySheet.type == "popup" ? `float: right;` : ``}">
                    <!--<a href="#" class="btn btn-default ${colorPicker.icon}" aria-label="Right Align" title="${colorPicker.title}" data-toggle="tooltip" data-id="${sheet.name}" data-font-picker-type="${sheet.colorPickerType}" onclick="${colorPicker.call.toggle}">
                    </a>
                    <div class="hide">-->
                        <input type="text" id="${sheet.name}" class="${colorPicker.containerClass} 
                        ${this.stage.propertySheet.type != "popup" ? `form-control` : ``}" value="${colorValue}" style="cursor: pointer;" ${
                            sheet.events && sheet.events.length > 0
                            ?
                            sheet.events.map(e => { 
                                return `${e.eventAttribute}="${typeof e.eventFunction == "function" ? e.eventFunction(sheet, propColumn, propColumnBuildObj) : e.eventFunction}"`;
                            }).join(" ")
                            :
                            ``
                        } />
                    <!--</div>-->
                </div>
                `;           
            case "addformat":
                return ``;
            case "deleteformat":
                return ``;           
                      
        }
    }
    _clearPropertyInput(sheet, propColumn, propColumnBuildObj) {
        //text/number/checkbox/dropdown/expression/fontpicker/colorpicker/fieldmapper/addformat/deleteformat/deletehyperlink
        
        switch (sheet.type) {            
            case "expression":
                $(`#${sheet.name}`).val(``);
                break;
            case "fontpicker":
                $(`#${sheet.name}`).val(``);
                break;
            case "colorpicker":
                break;            
            case "addformat":
                break;
            case "deleteformat":
                break;
        }
    }
    generateFontPicker(elem) {
        let _this = this;
        let { fontPicker } = this.stage;

        let previousValue = ``;
        let elemID = elem.data("id");
        // previousValue = `[Comic Sans MS,24,clNavy,,True,True,True,False]`;

        previousValue = elem.data("value").toString();

        // if(previousValue && previousValue.indexOf("[") == "0" && previousValue.indexOf("]") == (previousValue.length - 1)) {
        //     previousValue = previousValue.substring(1, previousValue.length - 1);
        // }

        previousValue = previousValue.replace(/[~\[\]]/g, "");

        let previosValueArray = [];
        if (elem.data("fontPickerType") == "properties") {
            previousValue.split(",").length == 8 ? (previosValueArray = previousValue.split(",")) : "";

            _this.fontPickerObj = {
                fontFamily: previosValueArray[0] || fontPicker[fontPicker.objects.fontFamily][0].value,
                fontSize: previosValueArray[1] || fontPicker[fontPicker.objects.fontSize][1].value,
                fontColor: previosValueArray[2] || fontPicker[fontPicker.objects.fontColor][0].value,
                unknown: previosValueArray[3] || "",
                Bold: previosValueArray[4] || fontPicker[fontPicker.objects.fontEffects][0].selectedValue.unchecked,
                Italic: previosValueArray[5] || fontPicker[fontPicker.objects.fontEffects][1].selectedValue.unchecked,
                Underline: previosValueArray[6] || fontPicker[fontPicker.objects.fontEffects][2].selectedValue.unchecked,
                Strikeout: previosValueArray[7] || fontPicker[fontPicker.objects.fontEffects][3].selectedValue.unchecked,
            };
        } else {
            previousValue.split(",").length == 7 ? (previosValueArray = previousValue.split(",")) : "";

            _this.fontPickerObj = {
                fontFamily: previosValueArray[0] || fontPicker[fontPicker.objects.fontFamily][0].value,
                fontSize: previosValueArray[6] || fontPicker[fontPicker.objects.fontSize][1].value,
                fontColor: previosValueArray[4] || fontPicker[fontPicker.objects.fontColor][0].value,
                unknown: previosValueArray[7] || "",
                Bold: previosValueArray[1] == "t" ? fontPicker[fontPicker.objects.fontEffects][0].selectedValue.checked : fontPicker[fontPicker.objects.fontEffects][0].selectedValue.unchecked,
                Italic: previosValueArray[2] == "t" ? fontPicker[fontPicker.objects.fontEffects][1].selectedValue.checked : fontPicker[fontPicker.objects.fontEffects][1].selectedValue.unchecked,
                Underline: previosValueArray[3] == "t" ? fontPicker[fontPicker.objects.fontEffects][2].selectedValue.checked : fontPicker[fontPicker.objects.fontEffects][2].selectedValue.unchecked,
                Strikeout: previosValueArray[5] == "t" ? fontPicker[fontPicker.objects.fontEffects][3].selectedValue.checked : fontPicker[fontPicker.objects.fontEffects][3].selectedValue.unchecked,
            };
        }

        


        let fontPickerHTML = `
        <div class="bodyAndFooter-cont">
            <div class="body-cont" data-pillindex="XXXXXXXXXXXXX">
                <div id="${fontPicker.containerId}" class="" data-elemid="${elemID}">
                    <div class="row rowNoMargin fontPickerSeperation">
                        <select class="col-md-6 col-sm-6 col-xs-6 windowsUI" data-font-picker="${fontPicker.objects.fontFamily}" onchange="${fontPicker.call.generatePreview}">
                            ${
                                //{ value: "Arial", caption: "Arial", cssProp: "font-family", cssValue: "Arial,Helvetica,sans-serif" },
                                fontPicker[fontPicker.objects.fontFamily].map(ff => {
                                    return `
                                        <option value="${ff.value}" style="${ff.cssProp}:${ff.cssValue};" ${_this.fontPickerObj[fontPicker.objects.fontFamily] == ff.value ? `selected="selected"` : ``}>${ff.caption}</option>
                                    `;
                                }).join("")
                            }
                        </select>
                        <select class="col-md-3 col-sm-3 col-xs-3 windowsUI" data-font-picker="${fontPicker.objects.fontSize}" onchange="${fontPicker.call.generatePreview}">
                            ${
                                //{ value: "8", caption: "8pt", cssProp: "font-size", cssValue: "11px" },
                                fontPicker[fontPicker.objects.fontSize].map(fs => {
                                    return `
                                        <option value="${fs.value}" data-style="${fs.cssProp}:${fs.cssValue};" ${_this.fontPickerObj[fontPicker.objects.fontSize] == fs.value ? `selected="selected"` : ``}>${fs.caption}</option>
                                    `;
                                }).join("")
                            }
                        </select>
                        <select class="col-md-3 col-sm-3 col-xs-3 windowsUI" data-font-picker="${fontPicker.objects.fontColor}" onchange="${fontPicker.call.generatePreview}">
                            ${
                                //{ value: "clBlack", caption: "Black", cssProp: "color", cssValue: this.AxClColors["clBlack"] },
                                fontPicker[fontPicker.objects.fontColor].map(fc => {
                                    return `
                                        <option value="${fc.value}" data-style="${fc.cssProp}:${fc.cssValue};" ${_this.fontPickerObj[fontPicker.objects.fontColor] == fc.value ? `selected="selected"` : ``}>${fc.caption}</option>
                                    `;
                                }).join("")
                            }
                        </select>
                    </div>
                    
                    <div id="fontEffectsDiv" class="row rowNoMargin windowsUI fontEffectsDiv fontPickerSeperation">
                        ${
                            //{ caption: "Bold", cssProp: "font-weight", cssValue: "bold", selectedValue: { checked: "True", unchecked: "False" } },
                            fontPicker[fontPicker.objects.fontEffects].map(fe => {
                                
                                return `
                                    <div class="fontEffectsDivSingle col-md-3 col-sm-3 col-xs-3 rowNoMargin">
                                        <div class="switch" onclick="return true;">
                                            <a href="javascript:void(0)" class="swtchDummyAnchr">
                                                <input class="tgl tgl-ios" name="fe${fe.caption}" id="fe${fe.caption}" type="checkbox"  data-font-picker="${fe.caption}" data-font-picker-parent="${fontPicker.objects.fontEffects}" onchange="${fontPicker.call.generatePreview}"  ${_this.fontPickerObj[fe.caption] == fe.selectedValue.checked ? `checked="checked"` : ``}>
                                                <label class="tgl-btn togglecustom toggle_btn" for="fe${fe.caption}" id="lbl${fe.caption}"></label>
                                            </a>
                                        </div>
                                        <label for="fe${fe.caption}">${fe.caption}</label>
                                        <!--<input id="fe${fe.caption}" type="checkbox" name="fe${fe.caption}" value="${fe.cssValue}" />
                                        <label for="fe${fe.caption}">${fe.caption}</label>-->
                                    </div>
                                `;
                            }).join("")
                        }
                    </div>
                    <!--<div class="row rowNoMargin fontPickerSeperation">-->
                        <!--<div class="col-md-2 col-sm-2 col-xs-2 windowsUI fontColorDiv">-->
                            <!--Color-->
                        <!--</div>-->
                        
                        <!--<select class="col-md-5 col-sm-5 col-xs-5 windowsUI">
                            <option value="Western">Western</option>
                        </select>-->
                    <!--</div>-->
                    <fieldset class="fontPickerSample col-md-12 col-sm-12 col-xs-12" style="border-radius: 4px;
                    border: 1px solid #cecece;">
                        <legend class="fontPickerSampleLegend windowsUI">Sample</legend>
                        <div id="${fontPicker.fontPickerSampleId}" class="${fontPicker.fontPickerSampleId}">Axpert Web</div>
                    </fieldset>
                </div>
            </div>
            <div class="footer-cont">
                <div class="pull-right d-flex" id="filterBtn">
                    <!--<input type="button" name="btnFilterApply" value="Cancel" onclick="" id="btnFilterApply" title="Cancel}" class="normalbtn btn handCursor allow-enter-key"/>-->
                    <input type="button" name="btnFilter" value="Ok" data-font-picker-type="${elem.data("fontPickerType")}" onclick="${fontPicker.call.save}" id="btnFilter" title="Ok" class="btn btn-primary shadow-sm ms-auto handCursor allow-enter-key" />
                </div>
            </div>
        </div>
        `;

        new PropertySheet().close();
        new IviewBuilder().displayBootstrapModalDialog(fontPicker.modalTitle, "lg", "330px", false, fontPickerHTML, "",
            function () {
                $(".bodyAndFooter-cont .body-cont").css({ "height": "calc(100% - 30px)" });
                
                $("input:checkbox, select", ".bodyAndFooter-cont .body-cont").trigger("change");

                // $(".modal-backdrop.in").css({"opacity": "0"});

                if(elem.data("fontPickerType") == "properties"){
                    $("[id=htmlContentOriFrameUrl]:visible").css({
                        "overflow-y": "auto",
                        "overflow-x": "hidden",
                        "max-height": `calc(100vh - ${$(".modal-header:visible").outerHeight() * 3}px)`
                    });
                }
            },
            function () { 
                _this.fontPickerObj = {};
                _this.unhide();
            }
        );
    }
    _generateFontPickerPreview(elem) {
        let _this = this;
        let { fontPicker } = this.stage;
        // debugger;
        switch (elem.data("fontPicker")) {
            case "fontFamily": {
                let _thisObj = fontPicker[elem.data("fontPicker")].filter((effect) => effect.value == elem.val())[0];

                // elem.css(_thisObj.cssProp, _thisObj.cssValue);

                $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp, _thisObj.cssValue);

                _this.fontPickerObj[elem.data("fontPicker")] = _thisObj.value;
            }
                break;
            case "fontSize": {
                let _thisObj = fontPicker[elem.data("fontPicker")].filter((effect) => effect.value == elem.val())[0];

                $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp, _thisObj.cssValue);

                _this.fontPickerObj[elem.data("fontPicker")] = _thisObj.value;
            }
                break;
            case "fontColor": {
                let _thisObj = fontPicker[elem.data("fontPicker")].filter((effect) => effect.value == elem.val())[0];

                $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp, _thisObj.cssValue);

                _this.fontPickerObj[elem.data("fontPicker")] = _thisObj.value;
            }
                break;
            case "unknown":
                break;
            case "Bold": {
                let _thisObj = fontPicker[elem.data("fontPickerParent")].filter((effect) => effect.caption == elem.data("fontPicker"))[0];
                
                $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp, (elem.prop("checked") ? _thisObj.cssValue : ""));
                
                _this.fontPickerObj[elem.data("fontPicker")] = _thisObj.selectedValue[elem.prop("checked") ? "checked" : "unchecked"];
            }
                break;
            case "Italic": {
                let _thisObj = fontPicker[elem.data("fontPickerParent")].filter((effect) => effect.caption == elem.data("fontPicker"))[0];

                $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp, (elem.prop("checked") ? _thisObj.cssValue : ""));

                _this.fontPickerObj[elem.data("fontPicker")] = _thisObj.selectedValue[elem.prop("checked") ? "checked" : "unchecked"];
            }
                break;
            case "Underline": {
                let _thisObj = fontPicker[elem.data("fontPickerParent")].filter((effect) => effect.caption == elem.data("fontPicker"))[0];

                let existingCssPropVal = $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp).split(" ").filter(prop => (prop == "line-through" || prop == " underline")).join(" ");

                let newCssPropVal = (elem.prop("checked") ? [...new Set([_thisObj.cssValue, ...existingCssPropVal.split(" ")])].join(" ") : existingCssPropVal.split(" ").filter(prop => prop != _thisObj.cssValue).join(" "));

                $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp, newCssPropVal);

                _this.fontPickerObj[elem.data("fontPicker")] = _thisObj.selectedValue[elem.prop("checked") ? "checked" : "unchecked"];
            }
                break;
            case "Strikeout": {
                let _thisObj = fontPicker[elem.data("fontPickerParent")].filter((effect) => effect.caption == elem.data("fontPicker"))[0];

                let existingCssPropVal = $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp).split(" ").filter(prop => (prop == "line-through" || prop == "underline")).join(" ");

                let newCssPropVal = (elem.prop("checked") ? [...new Set([_thisObj.cssValue, ...existingCssPropVal.split(" ")])].join(" ") : existingCssPropVal.split(" ").filter(prop => prop != _thisObj.cssValue).join(" "));

                $("#" + fontPicker.fontPickerSampleId).css(_thisObj.cssProp, newCssPropVal);

                _this.fontPickerObj[elem.data("fontPicker")] = _thisObj.selectedValue[elem.prop("checked") ? "checked" : "unchecked"];
            }
                break;
        }
    }
    saveFontPicker(elem) {
        let { fontPickerObj } = this;

        let fpContainerID = this.stage.fontPicker.containerId ;
        let fpInput = $($("#" + fpContainerID).data("elemid"));

        let finalValue = ``;
        
        
        if (elem.data("fontPickerType") == "properties") {
            finalValue = `[${fontPickerObj.fontFamily},${fontPickerObj.fontSize},${fontPickerObj.fontColor},${fontPickerObj.unknown},${fontPickerObj.Bold},${fontPickerObj.Italic},${fontPickerObj.Underline},${fontPickerObj.Strikeout}]`;
        } else {
            finalValue = `[${fontPickerObj.fontFamily},${fontPickerObj.Bold?.[0]?.toLowerCase() || "f"},${fontPickerObj.Italic?.[0]?.toLowerCase() || "f"},${fontPickerObj.Underline?.[0]?.toLowerCase() || "f"},${fontPickerObj.fontColor},${fontPickerObj.Strikeout?.[0]?.toLowerCase() || "f"},${fontPickerObj.fontSize}]`;
        }
        fpInput.val(finalValue);

        fpInput.next(".changeFontSettings").data("value", finalValue).attr("data-value", finalValue);      

        SetFieldValue(fpInput[0].id, finalValue);
        UpdateFieldArray(fpInput[0].id, GetFieldsRowNo(fpInput[0].id), finalValue, "parent", "AddRow");


        this.fontPickerObj = {};
        new IviewBuilder().closeModalDialog();
    }
    initializeColorPicker(elem){
        let _this = this;

        elem.spectrum({
            showInput: true,
            className: "full-spectrum",
            showInitial: true,
            showPalette: true,
            showSelectionPalette: true,
            maxSelectionSize: 10,
            preferredFormat: "hex",
            localStorageKey: "spectrum.agile",
            chooseText: "Apply",
            cancelText: "Close",
            clickoutFiresChange: false,

            type: "color",//_this.propertySheet.type == "div" ? "text" : "color",
            showAlpha: false,
            togglePaletteOnly: true,
            hideAfterPaletteSelect: true,
            allowEmpty: true,
            move: function (color) {
                if (!color) {
                    $(this).spectrum("set", _this._parseHexAndDelphiColors("", true));
                }
            },
            show: function (color) {
                let finalValue = _this._parseHexAndDelphiColors(color.toHexString(), false);
                SetFieldValue($(elem)[0].id, finalValue);
            },
            beforeShow: function (color) {
            
            },
            hide: function (color) {
            },
            change: function (color) {
                if (color) {

                    let finalValue = _this._parseHexAndDelphiColors(color.toHexString(), false)

                    SetFieldValue($(elem)[0].id, finalValue);
                    UpdateFieldArray($(elem)[0].id, GetFieldsRowNo($(elem)[0].id), finalValue, "parent", "");
                }
            },
            palette: [
                ["#000","#444","#666","#999","#ccc","#eee","#f3f3f3","#fff"],
                ["#f00","#f90","#ff0","#0f0","#0ff","#00f","#90f","#f0f"],
                ["#f4cccc","#fce5cd","#fff2cc","#d9ead3","#d0e0e3","#cfe2f3","#d9d2e9","#ead1dc"],
                ["#ea9999","#f9cb9c","#ffe599","#b6d7a8","#a2c4c9","#9fc5e8","#b4a7d6","#d5a6bd"],
                ["#e06666","#f6b26b","#ffd966","#93c47d","#76a5af","#6fa8dc","#8e7cc3","#c27ba0"],
                ["#c00","#e69138","#f1c232","#6aa84f","#45818e","#3d85c6","#674ea7","#a64d79"],
                ["#900","#b45f06","#bf9000","#38761d","#134f5c","#0b5394","#351c75","#741b47"],
                ["#600","#783f04","#7f6000","#274e13","#0c343d","#073763","#20124d","#4c1130"]
            ]
        });
    }
    _parseHexAndDelphiColors(color = "", delphitoHex = false) {
        let _this = this;
        if (delphitoHex) {
            if (!color) {
                color = "$00FFFFFF"
            }
            color = color.replace(/~/g, "");
            if (_this.stage.AxClColors[color]) {
                return _this.stage.AxClColors[color];
            } else if (color.indexOf("$") == 0 && color.length == 9) {
                return "#" + [color[7], color[8], color[5], color[6], color[3], color[4]].join("").toUpperCase();
            } else {
                return color;
            }
        } else {
            if (!color) {
                color = "#FFFFFF"
            }

            if(_this.tempDelphiAxColor = Object.keys(_this.stage.AxClColors).filter((clr)=>_this.stage.AxClColors[clr] == color.toUpperCase())?.[0]){
                return _this.tempDelphiAxColor;
            }
            if (color.indexOf("#") == 0 && color.length == 7) {
                return "$" + ["0", "0", color[5], color[6], color[3], color[4], color[1], color[2]].join("").toUpperCase();
            } else {
                return color;
            }
        }
        
    }
    _getPropertyInputValues(sheet, propColumn, propColumnBuildObj) {
        
        switch (propColumnBuildObj.type) {
            
            case "condFormat":
                break;
        }
        

        switch (sheet.type) {
            case "text":
                return ``;
            default:
                return ``;
            case "number":
                return ``;            
            case "expression":
                let { expressionBuilder } = this.stage;
                return `
                `;
            case "fontpicker":
                let { fontPicker } = this.stage;
                return `
                `;
            case "colorpicker":
                let { colorPicker } = this.stage;
                let colorValue = "";
                return `
                `;
            case "addformat":
                return ``;
            case "deleteformat":
                return ``;
        }
    }
    _generateSelectOptions(obj, mandatory, val) {
        if (!mandatory) {
            obj = [{ value: "", caption: new IviewBuilder().lcm[441] }, ...obj];
        }
        return obj.map((option) => {
            return `
            <option value="${option.value}" ${val == option.value ? ` selected="selected" ` : ``}>${option.caption || option.value}</option>
            `;
        }).join("");
    }
    
    _addCondFormat(elem) {
        
    }
    _deleteCondFormat(elem) {
        
    }
    _showPropertySheet() {
        $("#Wrapperpropsheet").css("display", "block");
        $("#updateWidget").prop('title', 'Save');
        $("#propertySrchCls").prop('title', 'Cancel');
        

        setTimeout(function () {
            $("#propertySheet").removeClass('scale-out').addClass('scale-in');
            $("#propertySheet").draggable({
                containment: "body"
            });
        }, 50);
    }
    show(elem) {
        let tempPropSheet = elem.data("type");
        let tempPropColumn = elem.data("field");

        if (this.propSheet == tempPropSheet && this.propColumn == tempPropColumn) {
            // show existing propertysheet and return
            // return;

        } else {
            this.propSheet = tempPropSheet;

            this.propColumn = tempPropColumn;

        }
        this._generatePropertySheet();
        this._showPropertySheet();
        // debugger;
    }
    _update() {
        // _this = this;
    }
    save() {
        this._update();
        this.close();
    }
    close() {
        if ($("#propertySheet").hasClass('scale-out'))
            return true;

        // var target = $("#propertySheet").data('target');

        $("#propertySheet").data('target', "").removeClass('scale-in').addClass('scale-out');
    }
    unhide() {
        $("#propertySheet").addClass('scale-in').removeClass('scale-out');
    }
}
