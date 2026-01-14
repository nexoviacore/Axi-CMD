let dataFldAttrHTMLConst = `data-gcode="" data-attributes fld-attributes `;
var chartProps = {}
let fieldType, date_format, time_format, timeFormat;

let formCompJSON = {
    "components": {
        "advanced": {
            "image": {
                "html": `<img class="img-fluid ax-field ax-img $fld-ClassAttr$" alt="Image" src="../images/loginlogo.png" ${dataFldAttrHTMLConst} />`
            },
            "chart": {
                "html": `<div id=chart_id style="width:100%; height:400px;" class="charts ax-chart" ${dataFldAttrHTMLConst}></div>`,
                "properties": ['type', 'title', 'datasource']
            },
            "table": {
                "html": `<div><h3 class="mb-5, mx-2">table-title</h3> table-html </div>`,
                "properties": ['datasource', 'params', 'class']
            }
        },
        "basic": {
            "boolean": {
                "html": `<div class="form-check form-check-custom me-5">
                            <input class="form-check-input ax-field ax-radio $fld-ClassAttr$" value="YES" data-type="boolean" type="radio" ${dataFldAttrHTMLConst} $checkbox-default$ />
                            <label class="form-check-label $fld-ClassAttr$">
                                Yes
                            </label>
                        </div>
                        <div class="form-check form-check-custom">
                            <input class="form-check-input ax-field ax-radio $fld-ClassAttr$" value="NO" data-type="boolean" type="radio" ${dataFldAttrHTMLConst} $checkbox-default$/>
                            <label class="form-check-label $fld-ClassAttr$">
                                No
                            </label>
                        </div>`,
                "properties": ['default', 'disabled', 'class']
            },
            "checkbox": {
                "html": `<div class="form-check form-check-custom">
                            <input class="form-check-input ax-field ax-checkbox $fld-ClassAttr$" type="checkbox" ${dataFldAttrHTMLConst} $checkbox-default$/>
                            <label class="form-check-label $fld-ClassAttr$">
                                <!--label-->
                            </label>
                        </div>`,
                "properties": ['datasource', 'params', 'class', 'default', 'disabled']
            },
            "date": {
                "html": `<div class="input-group ">
                            <input type="date" class="form-control form-control-sm ax-field ax-date $fld-ClassAttr$" format="$dateformat$" min=$min$ max=$max$ data-input ${dataFldAttrHTMLConst}  placeholder="dd-mm-yyyy" />
                            <span class="input-group-text" data-toggle>
                                <span class="material-icons material-icons-style cursor-pointer fs-4">
                                    calendar_today
                                </span>
                            </span>
                        </div>`,
                "properties": ['default', 'disabled', 'class', 'format', 'min', 'max']
            },
            "number": {
                "html": `<input type="number" class="ax-number form-control form-control-sm w-auto text-end $fld-ClassAttr$" value="$checkbox-default$" ${dataFldAttrHTMLConst}  />`,
                "properties": ['datasource', 'params', 'class', 'range', 'decimal', 'default', 'disabled', 'length']
            },
            "radio": {
                "html": `<div class="form-check form-check-custom">
                            <input class="form-check-input ax-field  ax-radio $fld-ClassAttr$" type="radio" ${dataFldAttrHTMLConst}  />
                            <label class="form-check-label">
                                <!--label-->
                            </label>
                        </div>`,
                "properties": ['datasource', 'params', 'class', 'default', 'disabled']
            },
            "select": {
                "html": `<select class="form-select form-select-sm w-auto al-field select ax-select $fld-ClassAttr$" ${dataFldAttrHTMLConst} >
                <!-- <option value="0"  >Choose...</option> -->
                        </select>`,
                "properties": ['default', 'disabled', 'class', 'datasource']
            },
            "text": {
                "html": `<input type="text" class="ax-field ax-text form-control form-control-sm w-25 $fld-ClassAttr$" value="$checkbox-default$"  ${dataFldAttrHTMLConst} />`,
                "properties": ['datasource', 'params', 'class', 'default', 'disabled', 'length']
            },
            "textarea": {
                "html": `<textarea rows="5" cols="50" class="ax-field ax-textarea form-control w-50 $fld-ClassAttr$" ${dataFldAttrHTMLConst}></textarea>`,
                "properties": ['default', 'disabled', 'length', 'class']
            },
            "html": {
                "html": `$htmltags$`
            },

            "time": {
                "html": `<div class="input-group">
                            <input type="text" class="form-control form-control-sm ax-field ax-time $fld-ClassAttr$" format="$timeformat$" data-input ${dataFldAttrHTMLConst} />
                            <span class="input-group-text" data-toggle>
                                <span class="material-icons material-icons-style cursor-pointer fs-4">
                                    schedule
                                </span>
                            </span>
                        </div>`,
                "properties": ['default', 'disabled', 'length', 'class', 'format']
            },
            "label": {
                "html": `<label class="ax-field">$fld-value$</label>`,
                "properties": ['datasource', 'params', 'class', 'default', 'disabled']
                }
        },
        "extra": {
            "noPreview": {
                "html": `<label class="form-label col-form-label text-danger fst-italic">NO PREVIEW...!!</label>`
            },
            "variation": {
                "html": `<div title="Select variations" class="ax-variation ax-field btn btn-icon btn-sm btn-secondary shadow-sm my-auto rounded-circle" ${dataFldAttrHTMLConst}>
                            <span class="material-icons material-icons-style material-icons-1">more_horiz</span>
                        </div>`
            },
        },

    }
};

let formFieldsJSON = {
    "rule": "",
    "fields": {},
};

/* On Page Load */
//document.onreadystatechange = function () {
//    if (document.readyState === "interactive") {
//        try {

//            this = new AxHTML();
//            this.clearRule();
//            if (this.varObjs.issetData) {
//                this.loadForm();
//            }

//        } catch (error) { }
//    }
//};

/* Config Params Class => Objects*/
let fldTypes = ["boolean", "checkbox", "checklist", "date", "number", "radio", "radiogroup", "select", "text", "textarea", "time", "html", "chart", "table", "label"];
let htmlText = "";
let component = "";
let arr_length;
let arr = [];
let data4 = "";

class AxHTML {
    addComponent(item) {
        if (item.hasOwnProperty('copyOf')) {
            this.fldTypes.push(item.type)
            this[item.type] = {}
            this[item.type].title = item.type
            this[item.type].copyOf = item.copyOf
            this[item.type].obj = { html: `${item.html}`, properties: this[this[item.type].copyOf].obj.properties }
            this[item.type].afterLoad = item.afterLoad
            this.components.basic[item.type] = { html: `${item.html}` }
        }
        else {
            this.catchError("copyOf does not exist");
        }
    }
    constructor() {
        this.fldTypes = fldTypes
        this.varObjs = {
            formFieldsObj: formFieldsJSON,
            dataSources: {}
        }
        this.variationPopup = {};
        this.variationPopupHtmls = {
            selectHtml: `<select class="variation-values"><option value="-1">Select...</option></select>`,
            addRowHtml: `<tr class="ax-variation-row" data-rowid="$rowid$">
                    <td class="variation-values-select ax-variations-cells w-25">  $variation-value-select$</td>
                    <td class="ax-variations-cells"> $variation-entry-html$  </td>    
                    <td class="variation-row-delete ax-variations-cells">  <span class="material-icons material-icons-style material-			icons-1">delete</span></td>  
            </tr> `,
            bodyHtml: `
            <table class="w-100 container" >
            <tbody class="fld-variation  variation-grid" data-varName="thisFieldName">
            <tr class="variation-header">
                            <td class="variation-values-select"> Select variation parameter:</td>
                            <td> <select id="selectVariation"><option value="-1">Select...</option></select>  </td>  
            <td class="add-variation"> <a class="btn btn-primary  Add-new" type="button">
                                        <span class="material-icons material-icons-style material-icons-1">add</span> Add </a>  </td>    
            </tr>
            <tr class="">
                        <td class="grid-header">  Value</td>
                            <td class="grid-header"> Enter variations  </td>    
                            <td class="grid-header">  </td>  
                            </tr>
                         <tbody>
                    </tbody>
                </table>`
        };

        this.container = 'body';
        this.components = this.getSelector(formCompJSON, "components")?.[0] || {};
        this.advanced = this.getSelector(this.components, "advanced")?.[0] || "";
        this.basic = this.getSelector(this.components, "basic")?.[0] || "";
        this.extra = this.getSelector(this.components, "extra")?.[0] || "";
        /* basic components */
        this.boolean = {
            obj: this.getSelector(this.basic, "boolean", "properties")?.[0] || {},
            title: "boolean"
        };
        this.checkbox = {
            obj: this.getSelector(this.basic, "checkbox", "properties")?.[0] || {},
            title: "checkbox"
        };
        this.date = {
            obj: this.getSelector(this.basic, "date", "properties")?.[0] || {},
            title: "date"
        };
        this.number = {
            obj: this.getSelector(this.basic, "number", "properties")?.[0] || {},
            title: "number",

        };
        this.radio = {
            obj: this.getSelector(this.basic, "radio", "properties")?.[0] || {},
            title: "radio"
        };
        this.select = {
            obj: this.getSelector(this.basic, "select", "properties")?.[0] || {},
            title: "select"
        };
        this.text = {
            obj: this.getSelector(this.basic, "text", "properties")?.[0] || {},
            title: "text"
        };
        this.textarea = {
            obj: this.getSelector(this.basic, "textarea", "properties")?.[0] || {},
            title: "textarea"
        };
        this.html = {
            obj: this.getSelector(this.basic, "html")?.[0] || {},
            title: "html"
        };
        this.time = {
            obj: this.getSelector(this.basic, "time", "properties")?.[0] || {},
            title: "time"
        };
        this.label = {
            obj: this.getSelector(this.basic, "label", "properties")?.[0] || {},
            title: "label"
        };
        /* advanced component */
        this.image = {
            obj: this.getSelector(this.advanced, "image")?.[0] || {},
            title: "image"
        }
        this.chart = {
            obj: this.getSelector(this.advanced, "chart")?.[0] || {},
            title: "chart"
        }
        this.table = {
            obj: this.getSelector(this.advanced, "table")?.[0] || {},
            title: "table"
        }
        /* extra components */
        this.variation = {
            obj: this.getSelector(this.extra, "variation")?.[0] || {},
            title: "variation"
        }
        this.noPreview = {
            obj: this.getSelector(this.extra, "noPreview")?.[0] || {},
            title: "noPreview"
        }
        this.layoutBuilder = {};
        this.layoutBuilder.rowHtml = `
        <div class="row m-2 layout-builder-row">
            <div class="col-4">
                <textarea class="form-control w-100 builder-text"></textarea>
            </div>
            <div class="col-4">
                <textarea class="form-control w-100 builder-text"></textarea>
            </div>
            <div class="col-4">
                <span class="material-icons material-icons-style material-icons-2 add-row-builder" onclick="axHtmlObj.addRowInBuilder(this)">add</span>
                <span class="material-icons material-icons-style material-icons-2 remove-row-builder" onclick="axHtmlObj.removeRowInBuilder(this)">close</span>
            </div>
        </div>`;
        this.layoutView = {};
        this.layoutView.rowHtml = `
        <div class="row m-2">
            <div class="col-4">
                <div class="w-100">
            </div>
            <div class="col-4">
                <div class="w-100">
            </div>
            <div class="col-4">                
            </div>
        </div>`;
        this.fromLayourBuilder = false;
        this.pageVariables = {};
    }

    getSelector(from, ...selectors) {
        return [...selectors].map(selector =>
            selector
                .replace(/\[([^\[\]]*)\]/g, '.$1.')
                .split('.')
                .filter(t => t !== '')
                .reduce((prev, cur) => prev && prev[cur], from)
        )
    };
    clear(options) {
        this.varObjs.formFieldsObj.rule = "";
        this.varObjs.formFieldsObj.fields = {};
    };

    parse(options) {
        try {
            this.varObjs.formFieldsObj.rule = "";
            /*  Parse input */
            if (options.name == "" && typeof options.name != "undefined") {
                this.catchError("Name cannot be left empty.");
                document.getElementById("form-preview").innerHTML = this.accessExtra("noPreview");
                return;
            } else {
                this.varObjs.formFieldsObj.rule = options.name;
            }

            if (!this.isNullOrEmpty(options.pageVariables)) {
                this.pageVariables = options.pageVariables
            }

            if (options.input != "" && typeof options.input != "undefined") {
                options.input = this.replaceSeparators(options.input);

                let allLines = options.input.split(/\r?\n|\r|\n/g); //To split multi-line string into array of strings.
                let container = options.container || this.container;
                let htmlText = "";
                allLines.forEach((line) => {
                    let words = line.split(" ");
                    let configText = "";
                    let isVar = false;

                    let inDataSet = false;
                    let varText = "";
                    let dsVarText = "";
                    let isFromDs = false;
                    words.forEach((word) => {
                        if (word == "{") { //start of a var
                            isVar = true;
                            varText = "";
                        } else if (word == "}") { //end of a var
                            isVar = false;
                            if (inDataSet) {
                                dsVarText += this.getHTML(varText);
                            } else {
                                configText += this.getHTML(varText);
                            }
                        } else if (word == "[") { //start of a dataset command
                            inDataSet = true;
                            dsVarText = "";
                        } else if (word == "]") { //end of dataset command
                            inDataSet = false;
                            isFromDs = true;
                            configText += this.getDSHTML(dsVarText);
                        }
                        else {
                            if (isVar) {
                                varText += " " + word;

                            } else if (inDataSet) {
                                dsVarText += " " + word;
                            } else {
                                configText += " " + word;

                            }
                        }
                    });

                    if (isFromDs)
                        configText = `<div class="flex-column ax-row mb-3">${configText}</div>`;

                    else {
                        if (this.fromLayoutBuilder)
                            configText = `<div class="align-items-center ax-row mb-3">${configText}</div>`;
                        else
                            configText = `<div class="d-flex align-items-center mb-3 ax-row">${configText}</div>`;

                    }
                    htmlText = configText;
                    if (fieldType == "chart") {
                        htmlText = `<div class="ax-inline-form mb-3" data-axrule="${options.name}" >${htmlText}</div>`;
                        document.querySelector(container).insertAdjacentHTML("beforeend", `${htmlText}`);
                        this.chartData(chartProps)
                        htmlText = ""
                    }
                    else if (fieldType !== "chart") {
                        htmlText = `<div class="ax-inline-form mb-3" data-axrule="${options.name}" >${htmlText}</div>`;
                        document.querySelector(container).insertAdjacentHTML("beforeend", ` ${htmlText} `);

                        this.bindEvents(options);
                    }
                });
                if (arr.length >= 0) {
                    for (let i = 0; i < arr.length; i++) {
                        if (typeof this[arr[i]].afterLoad != "undefined") {
                            this[arr[i]].afterLoad()
                        }
                    }
                }
                //  this.setData(options);
                //document.getElementById("ruleSave").classList.remove("disabled");
            } else {
                this.catchError("Expression cannot be left empty.");
            }
        } catch (error) {
            this.catchError(error.message);
        }
        if (typeof this.afterParse != "undefined") {
            this.afterParse()
        }

    }

    chartData(chartProps) {
        $(`#${chartProps.varName}`).highcharts({
            chart: {
                type: chartProps.chartType
            },
            title: {
                text: chartProps.chartTitle
            },
            xAxis: {
                categories: chartProps.xAxis,
                title:
                {
                    text: chartProps.titleX
                }
            },
            yAxis: {
                categories: chartProps.yAxis,
                title:
                {
                    text: chartProps.titleY
                }
            },
            series: chartProps.series
        });

    }
    dynamicTbl = (arr) => {
        let Table = [];
        let top_row = [];
        let rows = [];

        for (let i = 0; i < arr.length; i++) {
            let cells = [];
            for (let tHeader in arr[i]) {
                if (top_row.length < Object.keys(arr[i]).length) {
                    top_row.push(`<th scope="col">${tHeader.toUpperCase()}</th>`);
                }
                if (arr[i][tHeader] === null) {
                    cells.push(`<td>${null}</td>`);
                } else {
                    cells.push(`<td>${arr[i][tHeader]}</td>`);
                }
            }
            rows.push(`<tr>${cells.join("")}</tr>`);
        }

        Table.push(`<table class="table card-table table-bordered">`);
        Table.push(`<thead class="table-light">${top_row.join("")}</thead>`);
        Table.push(`<tbody>${rows.join("")}<tbody>`);
        Table.push("</table>");
        return Table.join("")
    }

    getHTML(fldExp) {
        // Construct HTML
        // return `<input type="number" class="form-control" max="200" min="0" step=".0" value="97.4">`;
        /* Example for number type */
        /* Input Expression value = "{clMax number range 0.0..200.0 decimal 1 default 97.4 required true}" */
        let _this = this;
        try {
            let fldExpArr = [];
            let fldProps = {};

            if (fldExp == "") {
                this.catchError("Please enter the field expression");
                return this.accessExtra("noPreview");
            } else {
                fldExp = fldExp.trim().replace(/  /g, ' ')
                fldExpArr = fldExp.split(" ");

                if (fldExpArr.length > 1 && fldExpArr[0] != '"' && !this.checkEmpty(fldExpArr[0]) && !this.checkTypeOf(fldExpArr[0]) && !this.checkEmpty(fldExpArr[1]) && !this.checkTypeOf(fldExpArr[1])) {
                    if (fldExpArr[0] != '"' && fldExpArr[0] == "html") {

                        var element_type = fldExp.substring(0, fldExp.indexOf(' '));
                        var html_element = fldExp.substring(fldExp.indexOf(' ') + 1);
                        fldProps.type = element_type;
                        fldProps.html = html_element;
                    }

                    else {
                        if (fldExpArr[0].toLowerCase() == "variation") {
                            fldProps.type = fldExpArr[0].toLowerCase();
                            fldProps.dtsrc = fldExpArr[1];
                        } else {
                            fldProps.varName = fldExpArr[0];

                            if (fldTypes.indexOf(fldExpArr[1]) != -1) {
                                fldProps.type = fldExpArr[1].toLowerCase();
                            } else {
                                this.catchError("Please enter the valid field type");

                                return this.accessExtra("noPreview");
                            }
                            fieldType = fldProps.type


                            for (var i = 2; i <= fldExpArr.length - 1; i = i + 2) {
                                arr.push(fldProps.type)
                                if ((this[fldProps.type].obj.properties.indexOf(fldExpArr[i]) != -1)) {
                                    switch (fldExpArr[i]) {
                                        case "type":
                                            if (typeof fldExpArr[i + 1] != "undefined") {
                                                fldProps.chart_type = fldExpArr[i + 1]
                                            }
                                            break;
                                        case "title":
                                            if (typeof fldExpArr[i + 1] != "undefined") {
                                                fldProps.title = fldExpArr[i + 1]
                                            }
                                            break;
                                        case "datasource":
                                            let isSelectData = false;
                                            let selectData = "";
                                            if (fldExpArr[i + 1].startsWith("(") && fldExpArr[i + 1].indexOf("(") != -1) {
                                                fldExpArr.forEach((word, wordIdx) => {
                                                    if (word.startsWith("(") && (word.indexOf("(") != -1)) {
                                                        isSelectData = true;
                                                        selectData = word;
                                                    } else if (word.endsWith(")")) {
                                                        isSelectData = false;
                                                        selectData = selectData + " " + word;
                                                    } else {
                                                        if (isSelectData) {
                                                            selectData = selectData + " " + word;
                                                        }
                                                    }
                                                });
                                            }
                                            if (!isSelectData && selectData != "") {
                                                fldProps.dtsrc = selectData;
                                            } else {
                                                fldProps.dtsrc = fldExpArr[i + 1];
                                            }
                                            if (fldProps.type == "chart") {
                                                let url = "http://127.0.0.1:5500/chartsJson/" + fldExpArr[i + 1] + ".json"
                                                let method = "GET"
                                                let data = {}
                                                this.callAPI(method, url, data, false, result => {
                                                    if (result.success) {
                                                        var chartJson = JSON.parse(result.response);
                                                        console.log(chartJson)

                                                        if (typeof chartJson != "undefined") {
                                                            if (chartJson.chart_data.hasOwnProperty(fldExpArr[i + 1])) {

                                                                chartProps.varName = fldProps.varName,
                                                                    chartProps.chartType = fldProps.chart_type,
                                                                    chartProps.chartTitle = fldProps.title,
                                                                    chartProps.xAxis = chartJson.chart_data[fldExpArr[i + 1]].xAxis,
                                                                    chartProps.titleX = chartJson.chart_data[fldExpArr[i + 1]].title_x,
                                                                    chartProps.yAxis = chartJson.chart_data[fldExpArr[i + 1]].yAxis,
                                                                    chartProps.titleY = chartJson.chart_data[fldExpArr[i + 1]].title_y,
                                                                    chartProps.series = chartJson.chart_data[fldExpArr[i + 1]].series
                                                            }
                                                        }

                                                    };


                                                })
                                            }                                            
                                            else {
                                                if (typeof fldExpArr[i + 1] != "undefined") {
                                                    fldProps[fldExpArr[i]] = fldExpArr[i + 1]
                                                }
                                            }
                                            break;

                                        case "decimal":
                                            fldProps.decimal = fldExpArr[i + 1];
                                            break;
                                        case "default":
                                            if (fldExpArr[i + 1] == '"' && typeof fldExpArr[i + 1] != "undefined") {
                                                let tempArr = fldExpArr.slice();
                                                tempArr = tempArr.splice(i + 2);
                                                tempArr = tempArr.slice(0, tempArr.indexOf('"'));
                                                // fldProps.value = tempArr.join(" ");
                                                fldProps.default = tempArr.join(" ");
                                                i = i + tempArr.length + 1; //set index to next keyword after double quote & default value
                                            }
                                            else {
                                                fldProps.value = fldExpArr[i + 1];
                                                fldProps.default = "";
                                            }
                                            break;
                                        case "disabled":
                                            fldProps.disabled = fldExpArr[i + 1];
                                            break;
                                        case "format":
                                            if (fldProps.type == "time") {
                                                if (fldExpArr[i + 1] == "12hours") {
                                                    fldProps.format = "h:i K"
                                                } else if (fldExpArr[i + 1] == "24hours") {
                                                    fldProps.format = "H:i K"
                                                }
                                            } else {
                                                fldProps.format = fldExpArr[i + 1];
                                            }

                                            break;

                                        case "length":
                                            fldProps.length = fldExpArr[i + 1];
                                            break;
                                        case "range":
                                            let minMax = fldExpArr[i + 1].split("..");
                                            if (typeof minMax != "undefined" && minMax != "") {
                                                fldProps.min = minMax[0];
                                                fldProps.max = minMax[1];
                                            }
                                            break;
                                        case "required":
                                            fldProps.required = fldExpArr[i + 1];
                                            break;
                                        case "searchcount":
                                            fldProps.searchcount = fldExpArr[i + 1];
                                            break;
                                        case "class":
                                            let flddata = fldExpArr[i + 1];
                                            if (typeof flddata != "undefined" && flddata != "") {
                                                fldProps.class = fldExpArr[i + 1];
                                                fldProps.class = fldProps.class.replace(",", " ");
                                            } else {
                                                fldProps.class = "";
                                            }
                                            break;
                                        case "html":
                                            fldProps.html = fldExpArr[i + 1]

                                        case "min":
                                            if (typeof fldExpArr[i + 1] != "undefined") {
                                                fldProps.min = fldExpArr[i + 1]
                                            }
                                            break;
                                        case "max":
                                            if (typeof fldExpArr[i + 1] != "undefined") {
                                                fldProps.max = fldExpArr[i + 1]
                                            }
                                            break;
                                        default:
                                            if (typeof fldExpArr[i + 1] != "undefined") {
                                                fldProps[fldExpArr[i]] = fldExpArr[i + 1]
                                            }
                                            break;
                                    }
                                } else {
                                    this.catchError(`${fldExpArr[i] + " is not a valid prop"}`);
                                    return this.accessExtra("noPreview");
                                }
                            }
                        }
                    }

                    if (["chart"].indexOf(fldProps.type) == -1 && typeof fldProps.datasource != "undefined") {
                        if (typeof this.varObjs.dataSources[fldProps.datasource] == "undefined") {
                            let url = "../../aspx/AxPEG.aspx/AxGetDataSourcesData"
                            let method = "POST"
                            let data = {};// JSON.parse(JSON.stringify(this.pageVariables));
                            data.dataSource = fldProps.datasource;
                            data.dataSourceParams = {};
                            if (typeof fldProps.params != "undefined") {
                                data.dataSourceParams = this.parseParams(fldProps.params);
                            }
                            Object.assign(data.dataSourceParams, this.pageVariables);

                            this.callAPI(method, url, data, false, result => {
                                if (result.success) {
                                    let json = JSON.parse(result.response);
                                    let dataResult = _this.dataConvert(json, "ARM");
                                    this.varObjs.dataSources[fldProps.datasource] = dataResult[fldProps.datasource] || "";
                                }
                            });
                        }

                        if (fldProps.type != "table") {
                            fldProps["value"] = this.varObjs.dataSources[fldProps.datasource]?.[0]?.[fldProps.varName];
                        }
                        else {
                            fldProps["table-html"] = this.dynamicTbl(this.varObjs.dataSources[fldProps.datasource]);
                        }


                        
                    }

                    /* field-object json begin */
                    let fldId = this.generateFldId();
                    this.varObjs.formFieldsObj.fields[fldId] = fldProps;

                    /* field-object json end */
                    if (fldProps.type == "boolean" || fldProps.type == "radio") {
                        fldProps.name = `radio_${fldProps.varName}${fldId}`;
                    }

                    let dataAttributes = ``;
                    dataAttributes += `data-fldId="${fldId}" `;
                    Object.keys(fldProps).map(atr => {
                        dataAttributes += `data-${atr}="${fldProps[atr]}" `;

                    });

                    if (typeof fldProps.format != "undefined") {
                        time_format = fldProps.format
                        date_format = fldProps.format
                    } else {
                        time_format = "H:i K"
                        date_format = "d/m/Y"
                    }

                    let fldAttributes = ``;
                    let fld_htmlAttributes = ``;
                    let fld_tableAttributes = ``;
                    Object.keys(fldProps).filter(atr => atr != "type" && atr != "varName" && atr != "required")
                        .map(atr => {
                            fldAttributes += `${atr}="${fldProps[atr]}" `;
                            fld_htmlAttributes += `${fldProps[atr]} `;
                            fld_tableAttributes += fldProps["table-html"]

                        });
                    var generateForm = ``;

                    Object.keys(this.basic)

                        .filter(obj => !this.checkTypeOf(this?.[obj]?.obj?.html))
                        .map(obj => {

                            obj = this[obj];
                            if (!this.checkTypeOf(fldProps.dtsrc) && !this.checkEmpty(fldProps.dtsrc) && (fldProps.type == "radio" || fldProps.type == "checkbox" || fldProps.type == "checkbox-grp" || fldProps.type == "number-inc-dec" || fldProps.type == "radiobtn-grp") && obj.title == fldProps.type) {
                                let thisFldOpt = fldProps.dtsrc.trim() || "";//"(Male~Female)";

                                if (thisFldOpt && thisFldOpt.startsWith("(") && thisFldOpt.endsWith(")")) {
                                    let processedOptions = thisFldOpt.trim().slice(1, thisFldOpt.length - 1).split("~");
                                    processedOptions.forEach((_opt) => {
                                        let objhtml = obj.obj.html;
                                        objhtml = objhtml.replaceAll("$id$", _opt);
                                        objhtml = objhtml.replaceAll("$fld-ClassAttr$", fldProps.class);
                                        objhtml = objhtml.replaceAll("$checkbox-default$", fldProps.default);
                                        objhtml = objhtml.replace("data-attributes", dataAttributes);
                                        objhtml = objhtml.replace("fld-attributes", fldAttributes);
                                        objhtml = objhtml.replace("<!--label-->", _opt);
                                        generateForm += `${objhtml}`;
                                    });
                                }
                            } else if (obj.title == fldProps.type) {
                                let objhtml = obj.obj.html;
                                objhtml = this.replaceGlobally(objhtml, "data-attributes", dataAttributes);
                                objhtml = this.replaceGlobally(objhtml, "fld-attributes", fldAttributes);
                                if (typeof fldProps.default == "undefined") {
                                    fldProps.default = ""
                                }
                                objhtml = objhtml.replaceAll("$checkbox-default$", fldProps.default);
                                if (typeof fldProps.class == "undefined") {
                                    fldProps.class = ""
                                }
                                objhtml = objhtml.replaceAll("$fld-ClassAttr$", fldProps.class);
                                objhtml = objhtml.replaceAll("$htmltags$", fld_htmlAttributes);
                                objhtml = objhtml.replace("$timeformat$", time_format);
                                objhtml = objhtml.replace("$dateformat$", date_format);
                                objhtml = objhtml.replace("$min$", fldProps.min);
                                objhtml = objhtml.replace("$max$", fldProps.max);
                                objhtml = objhtml.replace("$fld-value$", fldProps.value);
                                generateForm += `${objhtml}`;

                            }

                        });

                    Object.keys(this.extra)
                        .filter(obj => !this.checkTypeOf(this?.[obj]?.obj?.html))
                        .map(obj => {
                            obj = this[obj];

                            if (obj.title == fldProps.type) {
                                let objhtml = obj.obj.html;
                                objhtml = objhtml.replace("data-attributes", dataAttributes);
                                objhtml = objhtml.replace("fld-attributes", fldAttributes);
                                generateForm += `${objhtml}`;
                            }
                        });
                    Object.keys(this.advanced)
                        .filter(obj => !this.checkTypeOf(this?.[obj]?.obj?.html))
                        .map(obj => {

                            obj = this[obj];

                            if (obj.title == fldProps.type) {
                                let objhtml = obj.obj.html;
                                objhtml = objhtml.replace("chart_id", fldProps.varName);
                                //  objhtml = objhtml.replaceAll("chart_id", datavarname1);
                                objhtml = objhtml.replace("data-attributes", dataAttributes);
                                objhtml = objhtml.replace("fld-attributes", fldAttributes);
                                objhtml = objhtml.replace("table-html", fldProps["table-html"]);
                                objhtml = objhtml.replace("table-title", fldProps.tableTitle || "");
                                generateForm += `${objhtml}`;
                                var data = document.getElementById(fldId)
                            }
                        });
                    generateForm = `<div class="input-group w-auto field-wrapper">${generateForm}</div>`;
                    return ` ${generateForm} `;
                } else if (fldExpArr.length == 1 && fldExpArr[0] != "" && typeof fldExpArr[0] != "undefined") {
                    return `<label name="${fldExpArr[0]}">${fldExpArr[0]}</label>`;
                } else {
                    this.catchError("Please enter a valid field expression");
                    return this.accessExtra("noPreview");
                }
            }
        } catch (error) {
            this.catchError(error.message);
            return this.accessExtra("noPreview");
        }


    };

    parseParams(inputString) {
        const fieldValues = inputString.split('|');
        const parsedObject = {};
        fieldValues.forEach(fieldValue => {
            const [field, value] = fieldValue.split('~');
            parsedObject[field] = value;
        });

        return parsedObject;
    }

    callAPI(method, url, data, async, callBack) {
        let _this = this;
        var xhr = new XMLHttpRequest();
        xhr.open(method, url, async);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

        xhr.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 200) {
                    callBack({ success: true, response: this.responseText });
                }
                else {
                    try {
                        var message = JSON.parse(this.responseText)?.result?.message;
                        _this.catchError(message);
                    }
                    catch {
                        _this.catchError(this.responseText);
                    }
                    callBack({ success: false, response: this.responseText });
                }
            }
        }
        xhr.send(JSON.stringify(data));
    }
    getDSHTML(dsVarText) {
        /*  Bind data to string
            Example1: list
                Original Input = [LeaveTypes list Enter max allowed for :LeaveName {maxleaves number range 1..100}]            
                dsVarText = LeaveTypes list Enter max allowed for :LeaveName <input...>

            Example 2: checklist / radiogroup
                Original Input = [LeaveTypes checklist {IsApplicable} :LeaveName]
                dsvarText = LeaveTypes checklist {IsApplicable} :LeaveName

            while parsing...
            words = dsVarText.trim & split(" ")
            0 = DataSource name
            1 = type //list/checkList/radiogroup
            2 = variableName (optional)

            when processed..
            Enter max allowed for :LeaveName <input...>
        */
        try {
            if (dsVarText == "") {
                this.catchError("Please enter a field expression");
                return this.accessExtra("noPreview");
            } else {
                let words = dsVarText.trim().split(" ");
                let dsStr;

                if (words.length > 1 && words[0] != "" && typeof words[0] != "undefined" && words[1] != "" && typeof words[1] != "undefined") {

                    let dataSource = words[0];
                    let dsType = words[1].toLowerCase();
                    let dsDerivedType = "";
                    let varName = "";
                    let curFldId = "";

                    if (dsType == "list") {
                        dsStr = words.slice(2).join(' ');
                    } else if (dsType == "checklist" || dsType == "radiogroup") {
                        if (words[2].startsWith("{") && words[2].endsWith("}")) {
                            varName = words[2].slice(1, words[2].length - 1);
                        }
                        dsStr = words.slice(3).join(' ');
                        dsDerivedType = dsType == "radiogroup" ? "radio" : "checkbox";
                    } else {
                        this.catchError("Please enter a valid field type");
                        return this.accessExtra("noPreview");
                    }

                    if (dsDerivedType != "list") {
                        let dataAttributes = ``;
                        let fldAttributes = ``;
                        let fldProps = {};
                        let fldId = this.generateFldId();

                        if (varName != "") {
                            dataAttributes = `data-varName="${varName}" data-fldId="${fldId}"`;
                            fldAttributes = `name="${varName}" `;
                            fldProps = {
                                "varName": varName,
                                "type": dsDerivedType
                            };
                        }

                        this.varObjs.formFieldsObj.fields[fldId] = fldProps;

                        var generateForm = ``;
                        Object.keys(this.basic)
                            .filter(obj => !this.checkTypeOf(this?.[obj]?.obj?.html))
                            .map(obj => {
                                obj = this[obj];
                                if (obj.title == dsDerivedType) {
                                    let objhtml = obj.obj.html;
                                    objhtml = objhtml.replace("data-attributes", dataAttributes);
                                    objhtml = objhtml.replace("fld-attributes", fldAttributes);
                                    generateForm += `${objhtml}`
                                }
                            });

                        dsStr = `${generateForm} ${dsStr}`;
                        curFldId = dsStr.slice(dsStr.search('data-fldId="') + 12, dsStr.search("data-fldId=") + 33);
                    } else {
                        curFldId = dsVarText.slice(dsVarText.search('data-fldId="') + 12, dsVarText.search("data-fldId=") + 33);
                    }

                    this.varObjs.formFieldsObj.fields[curFldId].querySet = {
                        "dsName": dataSource,
                        "dsType": dsType,
                        "gName": varName
                    };

                    let result = "";

                    if (typeof this.varObjs.dataSources[dataSource] == "undefined") {
                        this.fetchData(dataSource);
                    }
                    //Bind Data & create field list
                    this.varObjs.dataSources[dataSource].forEach((rowData, rowIdx) => {
                        let tempDsStr = dsStr;

                        for (const key of Object.keys(rowData)) {

                            if (key == "code") {
                                tempDsStr = this.replaceGlobally(tempDsStr, `data-gcode=""`, `data-gcode="${rowData[key]}"`);
                                tempDsStr = this.replaceGlobally(tempDsStr, ` name="radio_`, ` name="radio_${rowData[key]}_`);
                            }
                            tempDsStr = this.replaceGlobally(tempDsStr, `:${key}`, rowData[key]);
                        }

                        result += `<div class="d-flex gap-2 align-items-center dataset-row-wrapper">${tempDsStr}</div>`;
                    });

                    return result;
                } else {
                    this.catchError("Please enter a valid field expression");
                    return this.accessExtra("noPreview");
                }
            }
        } catch (error) {
            this.catchError(error.message);
            return this.accessExtra("noPreview");
        }
    };
    setData(options) {
        try {
            let loadDataJson = options.data;
            if (typeof loadDataJson != "undefiend" && loadDataJson) {
                let $formContainer;
                if (typeof options.name != "undefined") {
                    $formContainer = document.querySelector(`[data-axrule='${options.name}']`);
                }
                else {
                    $formContainer = document.querySelector(`[data-rowid='${options.rowid}']`);
                }

                $formContainer.querySelectorAll(".ax-field").forEach((curFld) => {
                    let curFldName = curFld?.dataset?.varname;
                    let curFldGcode = curFld?.dataset?.gcode;

                    if (typeof curFldName != "undefined") {
                        let curFldVal = "";

                        if (curFldGcode != "" && typeof loadDataJson[curFldGcode] != "undefined") {
                            curFldVal = loadDataJson[curFldGcode][curFldName];
                        } else if (typeof loadDataJson[curFldName] != "undefined") {
                            curFldVal = loadDataJson[curFldName];
                        }
                        //else {
                        //    return this.catchError("Error in setData..");
                        //}

                        //if (this.checkTypeOf(curFldVal))
                        //    return;

                        this.setValue(curFld, curFldVal);
                    }
                    else {
                        if (curFld.classList.contains("ax-variation")) {

                            let variationText = "";
                            let variationNode;
                            if (curFldGcode != "" && typeof loadDataJson[curFldGcode] != "undefined") {
                                variationText = loadDataJson[curFldGcode]["variations"];
                                variationNode = loadDataJson[curFldGcode];
                            } else {
                                variationText = loadDataJson["variations"];
                                variationNode = loadDataJson;
                            }

                            if (typeof variationText != "undefined" && variationText != "") {
                                let variationName = variationText.split('~')[1];
                                let variationValue = variationNode[variationName];

                                if (typeof variationValue != "undefined") {
                                    let tempVal = {};
                                    tempVal[variationName] = variationValue;
                                    tempVal["variations"] = variationText;
                                    curFld.setAttribute("data-value", JSON.stringify(tempVal));
                                    curFld.parentElement.classList.add("ax-variation-available");
                                }
                            }

                        }
                    }
                });
            }
        } catch (error) {
            this.catchError(error.message);
        }
    };
    bindEvents(options) {
        try {
            let $container;
            if (typeof options.name != "undefined") {
                $container = document.querySelector(`[data-axrule='${options.name}']`);
            }
            else {
                $container = document.querySelector(`[data-rowid='${options.rowid}']`);
            }
            document.querySelectorAll("select.ax-select").forEach((thisFld) => {

                try {
                    let thisFldOpt = thisFld?.dataset?.dtsrc?.trim() || "";
                    /* process static / dynamic data source */
                    let processedOptions = "";
                    if (thisFldOpt.startsWith("(") && thisFldOpt.endsWith(")")) {
                        thisFldOpt = thisFldOpt.trim().slice(1, thisFldOpt.length - 1).split("~");
                        processedOptions = $.map(thisFldOpt, (val, index) => {
                            return {
                                id: val, // index,
                                text: val
                            }
                        });
                    } else {
                        processedOptions = $.map(this.varObjs.selectOptsObj[thisFldOpt], (val, index) => {
                            return {
                                id: val['col1'], // index,
                                text: val['col2'] || val['col1']
                            }
                        });
                    }
                    let showSearch = thisFld?.dataset?.searchcount > 0 ? thisFld?.dataset?.searchcount : Infinity;
                    $(thisFld).select2({
                        allowClear: true,
                        data: processedOptions,
                        // dropdownParent: this.getId("myModalId"),
                        minimumResultsForSearch: showSearch,
                        placeholder: "Select...", // appGlobalVarsObject.lcm[441],
                        // width: 'resolve'
                    });
                } catch (error) {
                    this.catchError(error.message);
                }
            });
            let dtFormats = ["d/m/Y", "m/d/Y"];
            let curDtFormat;
            const allChart = $container.querySelectorAll(".ax-chart")
            const allDate = document.querySelectorAll(".ax-date")
            allDate.forEach((thisFld) => {
                // if (thisFld?.dataset?.dateFormat && (dtFormats.indexOf(thisFld?.dataset?.dateFormat) != -1)) {
                //     curDtFormat = thisFld?.dataset?.dateFormat;
                // }
                curDtFormat = thisFld.getAttribute("format")
                let min = thisFld.getAttribute("min")
                let max = thisFld.getAttribute("max")
                thisFld.parentElement.flatpickr({
                    minDate: min,
                    maxDate: max,
                    dateFormat: curDtFormat,
                    disableMobile: "true",
                    allowInput: true,
                    wrap: true
                });
            });
            const allTime = document.querySelectorAll(".ax-time")
            allTime.forEach((thisFld) => {
                timeFormat = thisFld.getAttribute("format")
                thisFld.parentElement.flatpickr({
                    enableTime: true,
                    noCalendar: true,
                    dateFormat: timeFormat,
                    disableMobile: "true",
                    wrap: true
                });


            });
            /* Form components init ends */
            const allNumber = $container.querySelectorAll(".ax-number");
            allNumber.forEach((thisFld) => {
                if (thisFld?.dataset?.disabled?.toLowerCase == "true") {
                    thisFld.setAttribute("disabled");
                }
                thisFld.addEventListener("change", (event) => {
                    try {
                        let thisVal = event.target.value;

                        if ((thisFld?.dataset?.required?.toLowerCase() == "true") && this.checkEmpty(thisVal)) {
                            thisFld.focus();
                            this.catchError("Field cannot be left empty");
                            return;
                        }
                        thisVal = parseFloat(thisVal);
                        if (thisVal > parseFloat(thisFld?.dataset?.max)) {
                            thisFld.value = thisFld?.dataset?.max;
                            this.catchError(`Number is above maximum limit : ${thisFld?.dataset?.max}`);
                            return;
                        }

                        if (thisVal < parseFloat(thisFld?.dataset?.min)) {
                            thisFld.value = thisFld?.dataset?.min;
                            this.catchError(`Number is below minimum limit : ${thisFld?.dataset?.min}`);
                            return;
                        }

                        if (thisFld?.dataset?.decimal) {
                            let validDecimal = this.countDecimals(thisVal);

                            if (validDecimal > thisFld?.dataset?.decimal) {
                                this.catchError("Invalid decimal");
                                return;
                            }

                            if (validDecimal == 0) {
                                thisFld.value = thisVal.toFixed(thisFld?.dataset?.decimal);
                            }
                        }
                    } catch (error) {
                        this.catchError(error.message);
                    }
                });
            });

            const allTexts = $container.querySelectorAll(".ax-text, ax-textarea");
            allTexts.forEach((thisFld) => {
                if (thisFld?.dataset?.disabled?.toLowerCase == "true") {
                    thisFld.setAttribute("disabled");
                }
                if (thisFld?.dataset?.value) {
                    thisFld.value = thisFld?.dataset?.value;
                }
                thisFld.addEventListener("change", (event) => {
                    try {
                        let thisVal = event.target.value;

                        if ((thisFld?.dataset?.required?.toLowerCase() == "true") && this.checkEmpty(thisVal)) {
                            thisFld.focus();
                            this.catchError("Field cannot be left empty");
                            return;
                        }

                        if (thisVal.length > thisFld?.dataset?.length) {
                            this.catchError("Exceeds maximum length.");
                            return;
                        }
                    } catch (error) {
                        this.catchError(error.message);
                    }
                });
            });

            const allCheckRadio = $container.querySelectorAll(".ax-checkbox, .ax-radio");
            allCheckRadio.forEach((thisFld) => {
                if (thisFld?.dataset?.disabled?.toLowerCase == "true") {
                    thisFld.disabled = !thisFld.disabled;
                }

                if (thisFld.closest(".dataset-row-wrapper") != null) {
                    this.enableDisable(thisFld, "load");
                    thisFld.addEventListener("click", (event) => {
                        this.enableDisable(thisFld, "click");
                    });
                }
            });

            const allVariations = $container.querySelectorAll(".ax-variation");
            allVariations.forEach((thisFld) => {
                thisFld.addEventListener("click", (event) => {
                    this.openVariation(event.target);
                });
            });
        } catch (error) {
        }
    };

    getData(options) {
        try {
            let $forms = document.querySelectorAll('.ax-inline-form');
            let dataObj = {};
            $forms.forEach(($form) => {
                let ruleName = $form?.dataset?.axrule;
                dataObj[ruleName] = {};

                $form.querySelectorAll(".ax-field").forEach((curFld) => {
                    let curFldName = curFld?.dataset?.varname;
                    let curFldGcode = curFld?.dataset?.gcode;
                    let curFldVal = this.getValue(curFld);
                    if (typeof curFldName != "undefined") {
                        if (curFldGcode != "") {
                            if (typeof dataObj[ruleName][curFldGcode] == "undefined") {
                                dataObj[ruleName][curFldGcode] = { [curFldName]: curFldVal };
                            } else {
                                dataObj[ruleName][curFldGcode][curFldName] = curFldVal;
                            }
                        } else {
                            dataObj[ruleName][curFldName] = curFldVal;
                        }
                    }
                    else {
                        if (curFld.classList.contains('ax-variation') && typeof curFld.dataset?.value != "undefined") {
                            if (curFldGcode != "") {
                                if (typeof dataObj[ruleName][curFldGcode] == "undefined") {
                                    dataObj[ruleName][curFldGcode] = {};
                                }
                                Object.assign(dataObj[ruleName][curFldGcode], JSON.parse(curFld.dataset.value));
                            } else {
                                Object.assign(dataObj[ruleName], JSON.parse(curFld.dataset.value));
                            }
                        }
                    }
                });
            })

            return dataObj;
        } catch (error) {
            this.catchError(error.message);
        }
    };

    getValue($fld) {
        //((curFld?.type == "checkbox") ? curFld?.checked : (curFld?.type == "radio") ? curFld?.checked : curFld?.value || "")
        let val = "";
        if ($fld?.type == "checkbox") {
            val = $fld?.checked ? "YES" : "NO";
        }
        else if ($fld?.dataset?.type == "boolean") {
            val = document.querySelector(`input[name="${$fld.name}"]:checked`).value;
        }
        else if ($fld?.type == "radio") {
            val = $fld?.checked ? "YES" : "NO";
        }
        else {
            val = $fld?.value || ""
        }

        return val;
    }

    setValue($fld, value) {
        if ($fld?.type == "checkbox") {
            $fld.checked = (value == "YES") ? true : false;
        } else if ($fld?.dataset?.type == "boolean") {
            if (value == "YES") {
                $fld.checked = ($fld.value == "YES") ? true : false;
            }
            else if (value == "NO") {
                $fld.checked = ($fld.value == "NO") ? true : false;
            }
            else {
                if ($fld.value == $fld.getAttribute('default'))
                    $fld.checked = true;
            }
        } else if ($fld?.type == "radio") {
            $fld.checked = (value == "YES") ? true : false;
        } else if ($fld?.type == "select-one") {
            // $(curFld).val(curFldVal).trigger("change");
            $fld.value = value;
            $fld.dispatchEvent(new Event('change'));
        } else {
            $fld.value = value;
        }
    }

    updateRule() {
        /* Update Rule */
    }

    /* Helper functions */
    replaceSeparators(input) {
        return input.replace(/{/g, ' { ')
            .replace(/}/g, ' } ')
            .replace(/\[/g, ' [ ')
            .replace(/]/g, ' ] ')
            .replace(/"/g, ' " ');
    };

    replaceGlobally(original, searchTxt, replaceTxt) {
        const regex = new RegExp(searchTxt, 'gi');
        return original.replace(regex, replaceTxt);
    };

    accessExtra(thisEtc) {
        var resultEtc = ``;
        Object.keys(this.extra)
            .filter(obj => !this.checkTypeOf(this?.[obj]?.obj?.html))
            .map(obj => {
                obj = this[obj];
                if (obj.title == thisEtc) {
                    resultEtc += obj.obj.html;
                }
            });
        return resultEtc;
    };

    countDecimals(thisVal) {
        if (Math.floor(thisVal) === thisVal) return 0;
        return thisVal.toString().split(".")[1].length || 0;
    };

    generateFldId() {
        return `fid${Date.now()}${Math.floor(Math.random() * 90000) + 10000}`;
    };

    getId(idX) {
        return document.getElementById(idX);
    };

    getClass(classX) {
        return document.getElementsByClassName(classX)[0];
    };

    catchError(error) {
        showAlertDialog("error", error);
    };

    checkEmpty(elem) {
        return elem == "";
    };

    checkTypeOf(elem) {
        return typeof elem == "undefined";
    };

    enableDisable(thisFld, calledFrom) {
        let enableDisable = thisFld.checked;
        thisFld.closest(".dataset-row-wrapper").children.forEach((prEle) => {
            if (prEle.classList.contains("field-wrapper")) {
                prEle.children.forEach((chEle) => {
                    if (chEle.classList.contains("ax-field")) {
                        chEle.disabled = !enableDisable;
                    }
                });
            }
        });
    };

    fetchData(datasource) {
        let _this = this;
        let url = "../CustomPages/aspx/TreeConfig_v2.aspx/GetSqlData";
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, false);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let json = JSON.parse(this.responseText);
                let result = _this.dataConvert(json);
                _this.varObjs.dataSources[datasource] = result;
            }
            else {
                _this.catchError(this.responseText);
            }
        }
        xhr.send(JSON.stringify({ sqlName: datasource, sqlParams: "" }));
    }

    dataConvert(data, type) {
        if (type == "AXPERT") {
            try {
                data = JSON.parse(data.d);
                if (typeof data.result[0].result.row != "undefined") {
                    return data.result[0].result.row;
                }
            }
            catch (error) {
                this.catchError(error.message);
            };

            try {
                if (typeof data.result[0].result != "undefined") {
                    return data.result[0].result;
                }
            }
            catch (error) {
                this.catchError(error.message);
            };
        }
        else if (type == "ARM") {
            try {
                //if (!this.isAxpertFlutter)
                data = JSON.parse(data.d);
                if (data.result.success) {
                    if (!this.isUndefined(data.result.data)) {
                        return JSON.parse(data.result.data);
                    }
                }
                else {
                    if (!this.isUndefined(data.result.message)) {
                        this.catchError(data.result.message);
                    }
                }
            }
            catch (error) {
                this.catchError(error.message);
            };
        }

        return data;
    }

    openVariation($elem) {
        this.variationPopup = {};
        let entryHtml = $elem.closest('.dataset-row-wrapper')?.outerHTML || $elem.closest('.ax-row')?.outerHTML;
        this.variationPopupHtmls.entryHtml = entryHtml;
        this.variationPopup.elem = $elem.closest('.ax-variation');
        this.variationPopup.variationSource = this.variationPopup.elem.dataset.dtsrc;
        this.variationPopup.currentData = this.variationPopup.elem.dataset.value;

        if (typeof this.varObjs.dataSources[this.variationPopup.variationSource] == "undefined") {
            this.fetchData(this.variationPopup.variationSource);
        }

        let myModal = new BSModal("variationsPopup", "Get Variation", this.variationPopupHtmls.bodyHtml, this.variationPopupOpen(), this.variationPopupClose());

        myModal.changeSize("lg");
        myModal.scrollableDialog();
        myModal.staticBackdrop();
        myModal.verticallyCentered();

    };

    variationPopupOpen() {

        setTimeout(() => {
            let tempVariationOptions = $.map(this.varObjs.dataSources[this.variationPopup.variationSource], (val, index) => {
                return {
                    id: val['name'], // index,
                    text: val['caption']
                }
            });

            const variationSelect = document.getElementById("selectVariation")
            let variationOptions = [];
            tempVariationOptions.filter(function (item) {
                var i = variationOptions.findIndex(x => (x.id == item.id));
                if (i <= -1) {
                    variationOptions.push(item);
                    let option = document.createElement('option');
                    option.value = item.id;
                    option.text = item.text;
                    variationSelect.add(option);
                }
                return null;
            })

            if (typeof this.variationPopup.currentData == "undefined") {
                this.variationPopup.selectedVariation = '';
            }
            else {
                this.variationPopup.currentData = JSON.parse(this.variationPopup.currentData);
                this.variationPopup.selectedVariation = Object.keys(this.variationPopup.currentData)[0].replace("arv_", "");
                let variationData = Object.values(this.variationPopup.currentData)[0];
                document.getElementById("selectVariation").value = axHtmlObj.variationPopup.selectedVariation;

                Object.keys(variationData).forEach((item) => {
                    this.addVariationRow({ variation: item, data: variationData[item] });
                });
            }
            this.bindVariationPopupEvents();
        }, 10);

    }

    bindVariationPopupEvents() {
        const selectVariation = document.getElementById("variationsPopup").querySelector("#selectVariation");
        selectVariation.addEventListener("change", (event) => {
            try {
                let thisVal = event.target.value;
                this.variationPopup.selectedVariation = thisVal;

                const varRows = document.querySelector(".variation-grid").querySelectorAll(".ax-variation-row");
                varRows.forEach((row) => {
                    row.remove();
                });

                this.populateVariationValues();

            } catch (error) {
                this.catchError(error.message);
            }
        });

        const addVariationRow = document.getElementById("variationsPopup").querySelector(".add-variation");
        addVariationRow.addEventListener("click", (event) => {
            let options = { variation: "-1", data: {} };
            this.addVariationRow(options);
        });

        //modal-ok
        const okBtn = document.getElementById("variationsPopup").querySelector(".modal-ok");
        okBtn.removeAttribute("data-bs-dismiss");
        okBtn.addEventListener("click", (event) => {
            let varObj = this.getVariationData();
            this.variationPopup.elem.setAttribute("data-value", JSON.stringify(varObj));
            this.variationPopup = {};
            document.getElementById("variationsPopup").querySelector(".btn-close").click();
        })
    }

    populateVariationValues() {
        let tempVariationOptions = $.map(this.varObjs.dataSources[this.variationPopup.variationSource], (val, index) => {
            return {
                id: val['name'], // index,
                text: val['value']
            }
        });

        let variationOptions = [];
        tempVariationOptions.filter((item) => {
            var i = variationOptions.findIndex(x => (x.id == item.id));
            if (this.variationPopup.selectedVariation == item.id && i <= -1) {
                variationOptions.push({ id: item.text, text: item.text });
            }
            return null;
        });

        this.variationPopup.selectData = variationOptions;
    }

    addVariationRow(options) {
        try {
            if (this.variationPopup?.selectedVariation == "") {
                this.catchError("Select valid variation");
                document.getElementById("variationsPopup").querySelector("#selectVariation").focus();
                return;
            }

            let rowid = this.generateFldId();
            let rowHtml = this.getSelector(this.variationPopupHtmls, "addRowHtml")?.[0] || '';
            let selectHtml = this.getSelector(this.variationPopupHtmls, "selectHtml")?.[0] || '';
            let entryHtml = this.getSelector(this.variationPopupHtmls, "entryHtml")?.[0] || '';
            rowHtml = rowHtml.replace(`$variation-value-select$`, selectHtml);
            rowHtml = rowHtml.replace(`$variation-entry-html$`, entryHtml);
            rowHtml = rowHtml.replace(`$rowid$`, rowid);
            rowHtml = this.replaceGlobally(rowHtml, ` name="radio_`, ` name="radio_${rowid}_`);
            rowHtml = this.replaceGlobally(rowHtml, `ax-variation-available`, '');
            document.querySelector(".variation-grid").insertAdjacentHTML("beforeend", ` ${rowHtml} `);
            document.querySelector(`[data-rowid="${rowid}"]`).querySelector(".ax-variation").remove();

            this.initVariationRow(rowid);
            this.bindVariationRowEvents(rowid);

            if (typeof options?.data != "undefined") {
                const variationSelect = document.querySelector(`[data-rowid="${rowid}"]`).querySelector(".variation-values-select select");
                variationSelect.value = options.variation;
                options.rowid = rowid;
                this.bindVariationRowData(options);
            }
        } catch (error) {
            this.catchError(error.message);
        }
    }

    bindVariationRowData(options) {
        this.bindEvents(options);
        this.setData(options);
    }

    initVariationRow(rowid) {
        const variationSelect = document.querySelector(`[data-rowid="${rowid}"]`).querySelector(".variation-values-select select");

        if (typeof this.variationPopup.selectData == "undefined") {
            this.populateVariationValues();
        }

        this.variationPopup.selectData.forEach((item) => {
            let option = document.createElement('option');
            option.value = item.id;
            option.text = item.text;
            variationSelect.add(option);
        })
    }

    bindVariationRowEvents(rowid) {
        const deleteRow = document.querySelector(`[data-rowid="${rowid}"]`).querySelector(".variation-row-delete");
        deleteRow.addEventListener("click", (event) => {
            deleteRow.closest('.ax-variation-row').remove();
        })
    }

    getVariationData() {
        if (this.variationPopup?.selectedVariation == "") {
            this.catchError("Select valid variation");
            document.getElementById("variationsPopup").querySelector("#selectVariation").focus();
            return;
        }

        let variationRows = document.querySelectorAll(".ax-variation-row");
        let variationDataObj = {};
        let variationName = `arv_${this.variationPopup?.selectedVariation}`;
        variationDataObj[variationName] = {};
        variationDataObj['variations'] = (this.variationPopup?.selectedVariation == "employee_grouping" ? `employee_grouping~${variationName}` : `attribute~${variationName}`);
        variationRows.forEach(($row) => {
            let selectVal = $row.querySelector(".variation-values-select select").value;
            variationDataObj[variationName][selectVal] = {};

            $row.querySelectorAll(".ax-field").forEach((curFld) => {
                let curFldName = curFld?.dataset?.varname;
                //let curFldGcode = curFld?.dataset?.gcode;
                let curFldVal = this.getValue(curFld);
                if (typeof curFldName != "undefined") {
                    variationDataObj[variationName][selectVal][curFldName] = curFldVal;
                }
            });
        })
        return variationDataObj;
    }

    variationPopupClose() {
        //this.variationPopup = {};
    }

    addRowInBuilder($elem) {
        $elem.closest('.row').insertAdjacentHTML("afterEnd", ` ${this.layoutBuilder.rowHtml} `);
    }

    removeRowInBuilder($elem) {
        $elem.closest('.row').remove();
    }

    getLayoutBuilderText() {
        let rows = document.querySelectorAll(".layout-builder-row");
        let htmlText = '';
        rows.forEach((row) => {
            let texts = row.querySelectorAll(".builder-text");
            htmlText += `
            <div class="row m-2">`;
            texts.forEach((text) => {
                htmlText += `<div class="col-4"><div class="w-100">${text.value}</div></div>`
            })
            htmlText += `</div>`;
        })
        return htmlText;
    }

    isEmpty(elem) {
        return elem == "";
    };

    isNull(elem) {
        return elem == null;
    };

    isNullOrEmpty(elem) {
        return elem == null || elem == "";
    };

    isUndefined(elem) {
        return typeof elem == "undefined";
    };
}
