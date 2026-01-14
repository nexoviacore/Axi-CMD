var iframeindex = -1;
var _entityForm;
var _entityCommon;

//++ Variables from main-tstruct.js
var isMozilla;
var objDiv = null;
var originalDivHTML = "";
var DivID = "";
var over = false;
var addHeader = "";
var fillGridDatatbl = "";
var shouldFaceUser = false;
var flipCamera = false;
var gridDummyRows = false;
var gridDummyRowVal = new Array;
var AxpGridForm = "popup";
var fldmultiSelectdep = false;
var depNotBoundFld = "";
var axplatlongFldName = "";
var axpfilepathold = "";
var axpScanBarFldFocus = "";
var axpScriptaddrowres = "";
var axpScriptIsAddrow = true;
var dcLayoutType = "default";
var isDcLayoutSelected = false;
var formLabelJSON = [];
var buttonFieldFontJSON = [];
var regVarFldExp = new Array();
var select2EventType = "";
var select2IsOpened = false;
var select2IsFocused = false;
var setDesProp = {};
var forceRowedit = false;
var isGridFileUploadOnLoad = false;
var wizardHidenDcNos = [];
var gridRowEditOnLoad = false;
var AxRulesDefValidation = "false";
var AxRulesDefFilter = "false";
var AxRuesDefFormcontrol = "false";
var AxRulesDefComputescript = "false";
var AxRulesDefAllowdup = "false";
var AxRulesDefAllowEmpty = "false";
var AxRulesDefIsAppli = "false";

var AxRulesDefScriptOnLoad = "false";
var AxRulesDefOnSubmit = "false";
var AxRuesDefScriptFormcontrol = "false";
var AxRuesDefScriptFCP = "false";

var AxpDcstateVal = "";
var AxAllowEmptyFlds = new Array;
var isScriptFormLoad = "false";
var callGetTab = false;
var AxFldExpOnAddRow = "";
var AxpFillDepFieldsClient = "";
var AxFldBlured = "";
var AxFldBlurFromSelect = "";
var AxDoPegApprovalSave = "";
var AxPegFinalApproval = "false";
var AxAmendmentReadOnly = "false";
var AxPegAmdreadOnly = "false";
var isAddRowWsCalled = "false";
var isExcelImpDelWS = "false";
var isPegApproveConfirm = "";
var AxPegLevelNo = "";
var AxSetFldCaption = new Array();
var AxDiscardNxtPrevFc = new Array();
var MultiFillgridSldIndex = new Array();
var addFieldReloaduri = "";
var fldsHideOnPage = "false";
var iststddlcheck = false;
var interExterResourcesData = "";
//-- Variables from main-tstruct.js

class EntityForm {
    constructor() {
        this.components = {
            "N": {
                "html": `<input type="number" class="inputcontent1" value="{{value}}" readonly id="{{fieldid}}">`,
            },
            "ST": {
                "html": `<div class="inputcontent"  readonly id="{{fieldid}}">{{value}}</div>`,
            },
            "S": {
                "html": `<div class="inputcontent"  readonly id="{{fieldid}}">{{v}}</div>`,
            },
            "M": {
                "html": `<div class="inputcontent"  readonly>{{v}}</div>`,
            },
            "C": {
                "html": `<div class="inputcontent"  readonly id="{{fieldid}}">{{v}}</div>`,
            },
            "RT": {
                "html": `<p class="add-read-more show-less-content" id="{{fieldid}}">{{value}}</p><br>`,
            },
            "LT": {
                "html": `<p class="add-read-more show-less-content" id="{{fieldid}}">{{value}}</p><br>`,
            },
            "D": {
                "html": `<input type="date" class="form-control form-control-sm w-auto" value="{{value}}" readonly id="{{fieldid}}"/>`,
            },
            "ATT": {
                "html": ` <div id="{{fieldid}}" custom-id="{{customid}}" class="Files-Attached ATT float-start" onclick="_entityForm.downloadFileAttachment('{{v}}','{{filepath}}')" data-filename="{{v}}" data-filepath="{{filepath}}" role="button">
                                  {{componentimg}}
                               <div class="mx-3 fw-semibold attachmenttxt">
                               <a class="attached-filename" data-bs-toggle="tooltip" data-bs-placement="bottom" title="{{v}}">{{v}}</a>
                            </div>
                       </div>`
            },
            "AXPFILE_ATT": {
                "html": ` <div id="{{fieldid}}" custom-id="{{customid}}" class="Files-Attached AXPFILE_ATT float-start" onclick="_entityForm.downloadAxpFileAttachment('{{fieldid}}','{{v}}','{{scriptspath}}/axpert/{{sid}}/{{v}}',event, this)" data-filename="{{v}}" data-filepath="{{filepath}}" role="button">
                                  {{componentimg}}
                               <div class="mx-3 fw-semibold attachmenttxt">
                               <a class="attached-filename" data-bs-toggle="tooltip" data-bs-placement="bottom" title="{{v}}">{{v}}</a>
                            </div>
                       </div>`
            },
            "GRID_ATT": {
                "html": ` <div id="{{fieldid}}" custom-id="{{customid}}" class="Files-Attached GRID_ATT float-start" onclick="_entityForm.showGridAttLink('{{v}}')" data-filename="{{v}}" role="button">
                                  {{componentimg}}
                               <div class="mx-3 fw-semibold attachmenttxt">
                               <a class="attached-filename" data-bs-toggle="tooltip" data-bs-placement="bottom" title="{{v}}">{{v}}</a>
                            </div>
                       </div>`
            },

            "Image": {
                "html": `<img src="{{src}}" style="width:150px;" height="100" onerror="this.onerror=null;this.src='../images/no image.png';" id="{{fieldid}}">`
            },
            "togglebutton": {
                "html": `<div class="form-check form-switch" id="{{fieldid}}">
                  <input class="form-check-input" type="checkbox" role="switch" {{checked}} onclick="return false;" />
                </div>`
            }
        };
        this.fldtype = ["N", "ST", "D", "LT", "RT", "ATT", "Image", "table", "togglebutton"];
        this.flds = {};
        this.entityFormJson = {};
        this.pageSize = 5;
        this.emptyRowsHtml = `No data found`;
        this.hideControls = [];
        this.connectedData = {};
        this.connectedData.accordion =
            `<div class="Project_items">
                <div class="card  ">
                    <div class="col-xl-12 col-md-12 d-flex flex-column flex-column-fluid   ">
                        <div class="page-Header">
                            <div class="card-title collapsible cursor-pointer rotate" data-bs-toggle="collapse"
                                aria-expanded="true" data-bs-target="#Sub-Entity_wrapper">
                                <div class="symbol symbol-25px symbol-circle initialized"
                                    onclick="_entityForm.openEntityPage('{{transid}}')" data-toggle="tooltip" >
                                    <span class="symbol-label bg-warning text-inverse-warning fw-bold"
                                        style="height:30px !important; width:30px !important;">{{initial}}</span>
                                </div>
                                <span class="Project_title"
                                    onclick="_entityForm.fetchConnectedDataRecords('{{caption}}', '{{transid}}')">{{caption}}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=" row">
                    <div class="col">Count :</div>
                    <div class="col fw-bold" >{{totrec}}</div>
                </div>
            </div>`;

        this.connectedData.accordionWrapper =
            `<div class="row connecteddata-accordions" style="position: relative; right: 3px;" data-transid="{{transid}}" data-name="{{transid}}">
                <div class="dc-heading card-title cursor-pointer collapsible  rotate  collapsed " data-bs-toggle="collapse"
                    aria-expanded="true" data-bs-target="#sub-entitycontainer-{{transid}}" data-transid="{{transid}}">
                    <span class="material-icons material-icons-style material-icons-2 rotate-180">expand_circle_down
                    </span>
                    <span class="material-icons material-icons-style material-icons-2">account_tree</span>
                    <label class="dccaption">{{caption}}</label>
                    <span class="badge badge-dark mx-5">{{totrec}}</span>
                    <div class="card-toolbar">
                        <div class="Tkts-toolbar-Right">

                            <button type="button" data-transid="{{transid}}" data-caption="{{caption}}" id="SubEntity-add-btn-{{transid}}" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm  tb-btn btn-sm subentity-add" data-toggle="tooltip" title="New">
                                <span class="material-icons material-icons-style material-icons-2">add
                                </span>
                            </button>

                            <button type="button" data-transid="{{transid}}" data-caption="{{caption}}"  id="SubEntity-list-btn-{{transid}}" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm  tb-btn btn-sm  subentity-list" data-toggle="tooltip" title="List View">
                                <span class="material-icons material-icons-style material-icons-2">format_list_bulleted
                                </span>
                            </button>

                        </div>
                    </div>
                    <!--end::Toolbar-->

                </div>

                <div id="sub-entitycontainer-{{transid}}" class="row sub-entity-row collapse">
                    <div class="Entity_Form_SubEntity body_Container">
                    </div>
                </div>

            </div>`;

        this.connectedData.tabMenu =
            `<li class="nav-item w-100 me-0 mb-md-2" role="presentation">
                <a class="nav-link w-100 btn btn-flex btn-active-light-primary" data-bs-toggle="tab"
                    href="#kt_vtab_pane_{{transid}}" aria-selected="true" role="tab">
                    <i class="ki-duotone ki-icons/duotune/general/gen001.svg fs-2 text-primary me-3"></i> <span
                        class="d-flex flex-column align-items-start">
                        <span class="fs-4 fw-bold">{{caption}} ({{transid}})</span>
                    </span>
                </a>
            </li>`;

        this.connectedData.tabBody =
            `<div class="tab-pane fade" id="kt_vtab_pane_{{transid}}" role="tabpanel">
                <div class="table-responsive" id="connecteddata-fieldslist-{{transid}}">                                        
                </div>
            </div>`;

        this.entityName = '';
        this.entityTransId = '';
        this.recordId = '';
        this.keyValue = '';
        this.metaData = {};
        this.entityWiseMetaData = {};
        this.entityWiseFields = {};
        this.subEntityMapping = {};
        this.listJson = {};
        this.maxPageNumber = 1;
        this.pageSize = 100;
        this.chartsMetaData = [];
        this.subEntityList = {};
        this.selectedCharts = "";
        this.selectedSubEntities = "";
        this.subEntityFilter == "";
        this.selectedChartsObj = {};
        this.activitiesExpanded = false;
        this.isSubEntitiesAvailable = true;
        this.emptyRowsHtml = `No data found`;
        this.editable = true;
        this.keyField = "";
        this.connectedDataFields = {};
        this.customHyperLinks = [];
        this.customDataSource = false;
        this.relatedDataFields = "";
        this.relatedDataDisplayFields = "";
        this.relatedDataDisplayGridFields = "";
        this.formControlActions = {
            "expandcollapse": {}
        };
    }

    init() {
        let _this = this;
        try {
            
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            this.entityTransId = urlParams.get('tstid');
            this.recordId = urlParams.get('recid') || urlParams.get('recordid') || "0";

            var entityFormMetaData = JSON.parse(document.querySelector("#hdnEntityFormPageMetaData").value);
            var dataStr = document.querySelector("#hdnEntityFormPageData").value.split("*$*")[1].replaceAll("<br/>", "").replaceAll("<br />", "").replaceAll("<br>", "");
            var entityFormData = JSON.parse(dataStr);
            _this.hideControls = entityFormData.data.find(i => i.n?.toLowerCase() == 'axro_hidecontrols')?.v?.toLowerCase().split(',') || [];
            _this.entityFormJson.data = entityFormData.data;
            _this.entityFormJson.metadata = entityFormMetaData.result.data.FormMetaData;
            _this.entityFormJson.error = entityFormData.error;
            _this.metaData = entityFormMetaData.result.data.SubEntityMetaData;
            _this.properties = entityFormMetaData.result.data.Properties;
            _this.entityName = entityFormMetaData.result.data.EntityName;
            _this.keyField = entityFormMetaData.result.data.KeyField;
            _this.customDataSource = entityFormMetaData.result.data.DataSource;
            _this.entityTransId = entityFormMetaData.result.data.TransId;
            
            if (_entityCommon.isValid(_this.properties.CONNECTEDDATA_CONFIG)) {
                let configData = JSON.parse(_this.properties.CONNECTEDDATA_CONFIG);

                _this.connectedDataFields = configData.CONNECTEDDATA_METRICS || {};
                _this.customHyperLinks = configData.HYPERLINKS || [];
                _this.relatedDataFields = configData.RELATEDDATAFIELDS || "";
                _this.relatedDataDisplayFields = configData.DISPLAYFIELDS || "";
                _this.relatedDataDisplayGridFields = configData.DISPLAYGRIDFIELDS || "";
            }

            if (_this.customDataSource || !_entityCommon.inValid(_this.properties.CONNECTEDDATA_CONFIG)) {

                var validRightPanelData = Object.values(JSON.parse(_this.properties.CONNECTEDDATA_CONFIG)).some(value => {
                    if (Array.isArray(value)) return value.length > 0;
                    if (typeof value === 'object' && value !== null) return Object.keys(value).length > 0;
                    return !!value; // handles strings, numbers, booleans
                });

                if (validRightPanelData) {

                    $("#Entity_management_Left").toggleClass("col-xl-8 col-md-7 col-xl-12 col-md-12");
                    $("#Entity_management_Right").removeClass("d-none");
                }
            }

            if (_entityCommon.isValid(_this.connectedDataFields) && Object.keys(_this.connectedDataFields).length  > 0) {
                document.querySelector('#KPI-2').classList.remove("d-none");
                document.querySelector('#NO-KPI-Items').classList.add("d-none");
                document.querySelector('.right-connectedpagesdata').classList.remove("d-none");
            }

            _this.recordId = _this.entityFormJson.data.find(i => i.n == "axp_recid1").v;

            if (_entityCommon.inValid(_this.keyField)) {
                _this.keyField = _this.getKeyField(_this.entityTransId)?.fldname || _this.entityFormJson.data[2]?.n || "axp_recid1";
            }

            _this.keyValue = _this.entityFormJson.data.find(i => i.n == _this.keyField).v;
            _this.keyValue = _entityCommon.replaceSpecialChars(_this.keyValue)

            try {
                AxBeforeDataPageLoad(_this);
            }
            catch {
            }

            _this.constructEntityFormHTML();

            document.querySelector("#EntityTitle").innerHTML = _this.entityName + (_this.keyValue ? " - " + _this.keyValue : "");

            if (axDisallowCreate) {
                var transids = axDisallowCreate.split(",");
                if (transids.indexOf(this.entityTransId) > -1) {
                    this.allowCreate = false;
                    document.querySelector("#add-entity").remove();
                }
            }

            _this.parseSubEntityMetaDataJSON();          
            _this.constructRightPanel();
            _this.bindEvents();
            _this.applyTheme();

            DoScriptFormControl("", "On Data Load");
            try {
                AxAfterDataPageLoad(_this);
            }
            catch {
            }
        }
        catch (ex) {
            showAlertDialog("error", "Error occurred. Please check with administrator.");
            console.log(ex);
        }
        ShowHideDimmer(false);
    }

    constructRightPanel() {
        let _this = this;

        if (_this.customDataSource)
            _this.constructCustomDataSourceAccordion();

        if (_this.customHyperLinks.length > 0)
            _this.constructCustomHyperLinkAccordions();

        _this.getConnectedData();
        //_this.constructFormRelatedDataAccordions();
    }

    openNewTstruct() {

        let inputJson = { page: "Entity", transId: this.entityTransId, recordId: '0', action: "Create" };
        var allowCreate = _entityCommon.getEntityEditableFlag(inputJson);
        if (allowCreate) {
            _entityCommon.loadHyperLink(`../aspx/tstruct.aspx?transid=${this.entityTransId}&dummyload=false`)
            return true;
        }
        else {
            showAlertDialog("error", "User does not have 'Create' access for this screen. Please check with administrator.");
            return false;
        }

        
    }

    openSubEntityTstruct(transId) {
        let _this = this;
        let params = "";
        if (_this.subEntityMapping[transId]?.["mapfield"] && _this.subEntityMapping[transId]?.["mapsrcdata"]) {
            params = `act=open&${_this.subEntityMapping[transId]?.["mapfield"]}=${_this.subEntityMapping[transId]?.["mapsrcdata"]}`;
        }       

        let inputJson = { page: "Entity", transId: transId, recordId: '0', action: "Create" };
        var allowCreate = _entityCommon.getEntityEditableFlag(inputJson);
        if (allowCreate) {
            _entityCommon.loadHyperLink(`../aspx/tstruct.aspx?transid=${transId}&${params}&dummyload=false`)
            return true;
        }
        else {
            showAlertDialog("error", "User does not have 'Create' access for this screen. Please check with administrator.");
            return false;
        }

    }

    editEntity() {
        var _this = this;
        let inputJson = { page: "EntityForm", transId: _this.entityTransId, recordId: this.recordId,  action : "Edit" };
        var editable = _entityCommon.getEntityEditableFlag(inputJson);

        if (editable) {
            _entityCommon.loadHyperLink(`../aspx/tstruct.aspx?transid=${this.entityTransId}&act=load&recordid=${this.recordId}&dummyload=false`)
            return true;
        }
        else {
            showAlertDialog("error", "User does not have 'Edit' access for this record. Please check with administrator.");
            return false;
        }

    }

    editSubEntity(entityTransId, recordId) {
        let inputJson = { page: "EntityForm", transId: entityTransId, recordId: recordId, action: "Edit" };
        var editable = _entityCommon.getEntityEditableFlag(inputJson);

        if (editable) {
            _entityCommon.loadHyperLink(`../aspx/tstruct.aspx?transid=${entityTransId}&act=load&recordid=${recordId}&dummyload=false`);
            return true;
        }
        else {
            showAlertDialog("error", "User does not have 'Edit' access for this record. Please check with administrator.");
            return false;
        }
    }

    reloadEntityPage() {

        _entityCommon.loadHyperLink(`../aspx/EntityForm.aspx?tstid=${this.entityTransId}&recid=${this.recordId}`)
    }

    openEntityForm(entityName, transId, recordId, keyValue, rowNo) {
        //if (rowNo) {
        //    let _this = this;
        //    _this.updateRelatedData(transId, rowNo)
        //}

        var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;
        _entityCommon.loadHyperLink(url);
    }

    updateRelatedData(transId, rowNo) {
        let _this = this;
        if (!window.top.entityNavList)
            window.top.entityNavList = {};

        let tmpArr = _this.getNearestRecords(rowNo);
        tmpArr.forEach(rowData => {
            if (_entityCommon.inValid(rowData.entityname)) {
                rowData.entityname = _this.subEntityList[rowData.transid];
            }
        });
        window.top.entityNavList[transId] = _this.getNearestRecords(rowNo);
        window.top.entityNavList.lastTransId = transId;
        window.top.entityNavList.lastCaption = `${_this.keyValue}`;
    }

    openEntityPage(entityTransId) {
        let _this = this;
        if (_entityCommon.inValid(entityTransId))
            entityTransId = _this.entityTransId;

        _entityCommon.loadHyperLink(`../aspx/Entity.aspx?tstid=${entityTransId}`)
    }

    parseSubEntityMetaDataJSON() {
        let _this = this;
        let subEntityList = {};
        let entityWiseMetaData = {};
        let subEntityMapping = {};

        const subEntities = _this.metaData.filter(item => item.subentity === 'T');

        subEntities.forEach(item => {
            if (!subEntityList[item.ftransid])
                subEntityList[item.ftransid] = item.fcaption || item.ftransid;

            if (!entityWiseMetaData[item.ftransid])
                entityWiseMetaData[item.ftransid] = {};

            if (!_entityCommon.inValid(item.entityrelfld)) {
                if (!subEntityMapping[item.ftransid])
                    subEntityMapping[item.ftransid] = {};

                subEntityMapping[item.ftransid]["mapfieldjson"] = item;
                subEntityMapping[item.ftransid]["mapfield"] = item.fldname;
                subEntityMapping[item.ftransid]["mapdatafield"] = item.entityrelfld;
                subEntityMapping[item.ftransid]["mapsrcfield"] = item.srcfield;

                let mapVal = _this.keyValue;
                let mapSrc = _this.keyValue;
                if (item.entityrelfld == "recordid" && item.srcfield) {
                    mapVal = _this.recordId.toString();
                    let mapDataRow = _entityForm.entityFormJson.data.filter(i => i.n === item.srcfield);
                    if (mapDataRow.length)
                        mapSrc = ReverseCheckSpecialChars(mapDataRow[0].v);
                }
                if (item.entityrelfld == "recordid" && !item.srcfield) { //dcflds
                    mapVal = _this.recordId.toString();
                    mapSrc = _this.recordId.toString();
                }
                else if (item.srcfield) {
                    let mapDataRow = _entityForm.entityFormJson.data.filter(i => i.n === item.entityrelfld);
                    if (mapDataRow.length)
                        mapVal = ReverseCheckSpecialChars(mapDataRow[0].v);

                    let mapSrcRow = _entityForm.entityFormJson.data.filter(i => i.n === item.srcfield);
                    if (mapSrcRow.length)
                        mapSrc = ReverseCheckSpecialChars(mapSrcRow[0].v);
                }
                else if (item.entityrelfld) {
                    let mapDataRow = _entityForm.entityFormJson.data.filter(i => i.n === item.entityrelfld);
                    if (mapDataRow.length)
                        mapVal = ReverseCheckSpecialChars(mapDataRow[0].v);

                    let mapSrcRow = _entityForm.entityFormJson.data.filter(i => i.n === item.entityrelfld);
                    if (mapSrcRow.length)
                        mapSrc = ReverseCheckSpecialChars(mapSrcRow[0].v);
                }
                subEntityMapping[item.ftransid]["mapsrcdata"] = mapSrc;
                subEntityMapping[item.ftransid]["mapdatavalue"] = mapVal;

            }


            entityWiseMetaData[item.ftransid][item.fldname] = item;

        });

        _this.subEntityMapping = subEntityMapping;
        _this.entityWiseMetaData = entityWiseMetaData;
        _this.subEntityList = subEntityList;
        //_this.getActivitiesBtnsHTML();
        _this.getSubEntityKeyFields();
    }


    getSubEntityKeyFields() {
        let _this = this;
        let subEntityList = Object.keys(_this.subEntityList).join(",");
        let url = "../aspx/EntityForm.aspx/GetSubEntityKeyFieldsWS";
        let data = { subEntityList: subEntityList };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                let keyfields = JSON.parse(result.response).d;
                _this.subEntityKeyFields = JSON.parse(keyfields);
                ShowHideDimmer(false);
            } else {
                ShowHideDimmer(false);
            }
        });
    }

    populateEntityWiseFlds(index, field) {
        if (!_entityForm.entityWiseFields[field.ftransid]) {
            _entityForm.entityWiseFields[field.ftransid] = {};
            _entityForm.entityWiseFields[field.ftransid].largeTextElements = [];
            _entityForm.entityWiseFields[field.ftransid].attachmentElements = [];
            _entityForm.entityWiseFields[field.ftransid].otherElements = [];
            _entityForm.entityWiseFields[field.ftransid].buttonElements = [];
        }

        var fldType = getFieldDataType(field).toUpperCase();
        if (fldType === "BUTTON") {
            _entityForm.entityWiseFields[field.ftransid].buttonElements.push(field);
        }
        else if (fldType === "ATTACHMENTS") {
            _entityForm.entityWiseFields[field.ftransid].attachmentElements.push(field);
        }
        else if (fldType === "LARGE TEXT") {
            _entityForm.entityWiseFields[field.ftransid].largeTextElements.push(field);
        }
        else {
            _entityForm.entityWiseFields[field.ftransid].otherElements.push(field);
        }
    }

    openEntityForm(entityName, transId, recordId, keyValue) {
        var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;
        _entityCommon.loadHyperLink(url);
    }

    constructEntityFormHTML() {
        let _this = this;
        let jsonData = _this.entityFormJson
        // Get the container where HTML will be appended
        const container = document.getElementById('entityform-container');

        if (_entityCommon.inValid(jsonData.error)) {
            // Group the data by 'dc'
            const groupedData = this.groupByDC(jsonData.data);
            this.parseFormMetaData(jsonData.metadata);
            this.constructScriptButtons(jsonData.metadata);
            // Generate HTML for each group and append it to the container
            container.innerHTML = this.generateHTML(groupedData, jsonData.metadata);

            //const dcTabsList = document.querySelectorAll("#dcTabs li a");

            //dcTabsList.forEach((tab) => {
            //    tab.addEventListener("click", function (event) {
            //        const allTabs = document.querySelectorAll("#dcTabs li");
            //        allTabs.forEach(tab => tab.classList.remove("active"));
            //        this.closest('li').classList.add("active");

            //        document.querySelector(`${this.getAttribute("href")} .dc-heading.collapsed`)?.click();
            //    });
            //})
        }
        else {
            container.innerHTML = jsonData.error[0].msg;
        }

    }

    parseFormMetaData(metaData) {
        metaData.forEach((fld) => {
            this.flds[fld.fname] = fld;
        })
    }

    constructScriptButtons(metaData) {

        let html = '';
        metaData.filter(item => item.customdatatype == "Button").forEach((btn) => {
            html += `<div class="menu-item px-3 my-0" onclick="_entityForm.callScriptAction('${btn.fname}')">
                        <a href="javascript:void(0)" class="menu-link px-3 py-2" data-kt-element="mode" data-target="lightTheme" data-kt-value="light">
                            <span class="material-icons material-icons-style material-icons-2">keyboard_double_arrow_right</span>
                            <span class="menu-title">${btn.caption}
                            </span>
                        </a>
                    </div>`;
        });

        if (html) {
            document.querySelector("#scriptBtns-btn").classList.remove("d-none");
            document.querySelector("#scriptBtns-container").innerHTML = html;
        }

    }

    callScriptAction(script) {
        ShowHideDimmer(true);
        let _this = this;
        let url = "../aspx/EntityForm.aspx/CallScriptsWS";
        let data = { transId: _this.entityTransId, script: script, recordId: _this.recordId };
        this.callAPI(url, data, true, result => {
            ShowHideDimmer(false);
            if (result.success) {
                try {
                    result = JSON.parse(JSON.parse(result.response).d).result;
                    AssignLoadValues(result, "", script, "")
                    try {
                        AxAfterCallAction();
                    }
                    catch (ex) {

                    }
                }
                catch (e) {

                }
            }
            else {
                showAlertDialog("Error", result.response);
            }
        });
    }


    bindEvents() {
        let _this = this;
        const subEntityDiv = document.getElementById('Project_Entity_form'); // Replace 'yourDivId' with the ID of your div
        subEntityDiv.addEventListener('scroll', function () {
            if (_this.isSubEntitiesAvailable && _this.activitiesExpanded && isScrollAtBottomWithinDiv(subEntityDiv)) {
                var newPageNo = _this.maxPageNumber + 1;
                _this.getSubEntityListData(newPageNo);
            }
        });

        _this.bindThemeEvent();
    }

    bindAccordionExpandEvent() {
        let _this = this;
        document.querySelectorAll('.connecteddata-accordions').forEach(item => {
            $(item).on('show.bs.collapse', function () {
                var _accordion = this;
                setTimeout(function () {
                    if (_accordion.classList.contains('data-fetched'))
                        return;

                    let transId = _accordion.dataset.transid;
                    _accordion.classList.add('data-fetched')
                    _this.fetchConnectedDataRecords(transId);

                }, 1);

            })
        });

        document.querySelectorAll('.right-connectedpagesdata').forEach(item => {
            $(item).on('show.bs.collapse', function () {
                var _accordion = this;
                if (_accordion.classList.contains('data-fetched'))
                    return;

                _accordion.classList.add('data-fetched')
                _this.fetchAllConnectedDataMetrics();

            })
        });

        document.querySelectorAll('.subentity-add').forEach(item => {
            item.addEventListener('click', function () {
                let transId = this.dataset.transid;
                _this.openSubEntityTstruct(transId);
                return false;
            })
        });

        document.querySelectorAll('.subentity-list').forEach(item => {
            item.addEventListener('click', function () {
                let transId = this.dataset.transid;
                let caption = this.dataset.caption;
                _this.openEntityPage(transId);
                return false;
            })
        });
    }

    getSubEntityConnectedDataFilter() {
        let _this = this;
        let strFilter = "";
        Object.entries(_this.subEntityMapping).forEach(([key, value]) => {
            let mapVal = value["mapdatavalue"];
            let mapFld = value["mapfield"];
            strFilter += `${key}~~~~${value.mapfieldjson.normalized || "F"}~${value.mapfieldjson.srctable || ""}~${value.mapfieldjson.srcfield || ""}~${value.mapfieldjson.allowempty || "F"}~${value.mapfieldjson.tablename || ""}~${mapFld}~'${mapVal}'^`;
        });

        if (strFilter.endsWith("^"))
            return strFilter.substr(0, strFilter.length - 1);
        else
            return strFilter
    }

    getConnectedDataMetricsCriteria(transId) {
        let _this = this;
        let strCriteria = transId + "~";
        let connectedDataFields = _this.connectedDataFields[transId];
        if (_entityCommon.isValid(connectedDataFields)) {
            connectedDataFields.forEach(fld => {                
                strCriteria += `${fld.condition}(${fld.fieldname}) ${fld.condition}_${fld.fieldname},`                
            });

            if (strCriteria.endsWith(","))
                strCriteria = strCriteria.substr(0, strCriteria.length - 1);

            var value = _this.subEntityMapping[transId];
            let mapVal = value["mapdatavalue"];
            let mapFld = value["mapfield"];
            strCriteria += `~${value.mapfieldjson.normalized || "F"}~${value.mapfieldjson.srctable || ""}~${value.mapfieldjson.srcfield || ""}~${value.mapfieldjson.allowempty || "F"}~${value.mapfieldjson.tablename || ""}~${mapFld}~'${mapVal}'^`;

        }
        

        if (strCriteria.endsWith("^"))
            return strCriteria.substr(0, strCriteria.length - 1);
        else
            return strCriteria;
    }

    getAllConnectedDataMetricsCriteria() {
        let _this = this;
        let strCriteria = "";
        Object.keys(_this.connectedDataFields).forEach(transId => {
            strCriteria += transId + "~";
            let connectedDataFields = _this.connectedDataFields[transId];
            if (_entityCommon.isValid(connectedDataFields)) {
                connectedDataFields.forEach(fld => {
                    strCriteria += `${fld.condition}(${fld.fieldname}) ${fld.condition}_${fld.fieldname},`
                });

                if (strCriteria.endsWith(","))
                    strCriteria = strCriteria.substr(0, strCriteria.length - 1);

                var value = _this.subEntityMapping[transId];
                let mapVal = value["mapdatavalue"];
                let mapFld = value["mapfield"];
                strCriteria += `~${value.mapfieldjson.normalized || "F"}~${value.mapfieldjson.srctable || ""}~${value.mapfieldjson.srcfield || ""}~${value.mapfieldjson.allowempty || "F"}~${value.mapfieldjson.tablename || ""}~${mapFld}~'${mapVal}'^`;
            }
        });
        


        if (strCriteria.endsWith("^"))
            return strCriteria.substr(0, strCriteria.length - 1);
        else
            return strCriteria;
    }


    getConnectedData() {
        let _this = this;
        let url = "../aspx/EntityForm.aspx/GetSubEntityChartsDataWS";
        var criteria = _this.getSubEntityConnectedDataFilter();

        let data = { entityName: _this.entityName, transId: _this.entityTransId, condition: "General", criteria: criteria, recordId: _this.recordId, keyValue: _this.keyValue };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                _this.connectedDataJson = JSON.parse(JSON.parse(result.response).d);
                var connectedDataJson = _this.connectedDataJson.result.charts;
                if (connectedDataJson.length) {
                    _this.isSubEntitiesAvailable = true;

                    _this.isSubEntitiesAvailable = true;
                    _this.constructConnectedDataAccordions(connectedDataJson);
                }
                else {
                    _this.isSubEntitiesAvailable = false;                    
                }                
            }            
        });
    }

    constructFormRelatedDataAccordions() {
        let _this = this;
        var relatedFields = [];

        if (_this.relatedDataFields == "") {
            return;
        }
        else {

            var relatedFldsArr = [];
            _this.relatedDataFields.split(",").forEach(fld => {
                relatedFldsArr.push(fld.split('~')[1]);
            })

            relatedFields = _this.metaData.filter(function (item) {
                return item.subentity === "F" && item.hide === "F" && relatedFldsArr.includes(item.fldname);
            });
        }

        let html = '';
        relatedFields.forEach(fld => {
            let fldVal = fld.fldcap || '';
            let mapDataRow = _this.entityFormJson.data.filter(item => item.n === fld.fldname);
            if (mapDataRow.length) {
                let rowNo = 1;
                mapDataRow.forEach(row => {
                    fldVal = ReverseCheckSpecialChars(row.v);
                    let dcNameWithRowNo = `${fld.dcname}${rowNo}`;
                    html += 
                        `<div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap rightpanel-relatedddata-accordion" data-fldid="${fld.fldname}" data-dcname="${fld.dcname}" data-rowno="${rowNo}"  data-fldval="${fldVal}" data-griddc="${fld.griddc}">
                            <a href="#" class="Invoice-item accordion-header accordion-button fs-4 fw-semibold collapsed" data-bs-toggle="collapse" data-bs-target="#kt_accordion_connecteddata_${dcNameWithRowNo}${fld.fldname}" aria-expanded="false" aria-controls="kt_accordion_connecteddata_${dcNameWithRowNo}${fld.fldname}">
                                <div class="Invoice-icon">
                                    <span class="material-icons material-icons-style material-icons-2">account_tree</span>
                                </div>
                                <div class="Invoice-content">
                                    <h6 class="subtitle">${fld.fldcap || ''} - ${fldVal}</h6>
                                </div>
                            </a>
                            <div id="kt_accordion_connecteddata_${dcNameWithRowNo}${fld.fldname}" class="accordion-collapse collapse" aria-labelledby="kt_accordion_connecteddata_${dcNameWithRowNo}${fld.fldname}">
                                <div class="cc-metrics-panel">
                                    <div class="cc-metrics-list">               
                                    </div>
                                </div>
                            </div>
                        </div>`

                    rowNo++;
                });
            }


        })        


        if (html) {
            document.querySelector('#KPI-2 .row').insertAdjacentHTML("afterbegin", html);
            document.querySelector('#KPI-2').classList.remove("d-none");
            document.querySelector('#NO-KPI-Items').classList.add("d-none");
        }

        document.querySelectorAll('.rightpanel-relatedddata-accordion').forEach(item => {
            $(item).on('show.bs.collapse', function () {
                if (this.classList.contains('data-fetched'))
                    return;

                let fldId = this.dataset.fldid;
                let dcName = this.dataset.dcname;
                let gridDc = this.dataset.griddc;
                let rowNo = this.dataset.rowno;
                let fldVal = this.dataset.fldval;


                _this.fetchFilteredData(fldId, fldVal, dcName, gridDc, rowNo.toString());
            })
        });

    }

    getEntityListDataForOtherData(pageNo, fldId, gridDc) {
        ShowHideDimmer(true);
        let _this = this;
        let url = "../aspx/Entity.aspx/GetEntityListWithGridDataWS";
        let data = { transId: _this.entityTransId, fields: _this.fields, pageNo: pageNo, pageSize: _this.pageSize, filter: _this.filter, gridDc: gridDc };
        this.callAPI(url, data, false, result => {
            if (result.success) {
                ShowHideDimmer(false);
                let listJson = JSON.parse(JSON.parse(result.response).d);
                listJson = listJson.result.list?.[0]?.data_json ?? "[]";
                try {
                    if (listJson.constructor != Array)
                        listJson = JSON.parse(listJson);
                }
                catch {
                    showAlertDialog("error", "Error occurred in data fetch.");
                    return;
                }

                if ((listJson.length || 0) == 0) {
                    if (pageNo == 1) {
                        _this.listJson = [];
                        document.querySelector(`kt_accordion_connecteddata_${fldId}`).innerHTML = _this.emptyRowsHtml;
                        return;
                    }
                }
                else {

                    _this.constructFormRelatedDataList(listJson, fldId);

                }
            }
            else {
                ShowHideDimmer(false);
            }
        });
    }

    constructFormRelatedDataList(listArr, fldId) {
        let _this = this;
        let html = '';

        if (!_this.entityWiseFields[_this.entityTransId]) {
            $.each(_this.metaData.filter(i => i.ftransid === _this.entityTransId), function (index, field) {
                _this.populateEntityWiseFlds(index, field);
            })
        }

        for (var rowData of listArr) {
            if (rowData.recordid == _this.recordId)
                continue;

            let keyCol = _this.entityKeyField;

            if (!rowData.hasOwnProperty(keyCol)) {
                keyCol = _this.getKeyField(rowData.transid).fldname;
                if (_entityCommon.inValid(rowData[keyCol]))
                    keyCol = Object.keys(rowData)[4];
            }

            rowData.keycol = keyCol;


            if (!rowData.keycol || _entityCommon.inValid(rowData[rowData.keycol])) {
                const keys = Object.keys(rowData);
                let notNullNode = null;

                for (let i = 4; i < keys.length; i++) {
                    const key = keys[i];
                    if (rowData[key] !== null && rowData[key] !== undefined && rowData[key] !== "") {
                        notNullNode = key;
                        break;
                    }
                }

                rowData.keycol = notNullNode;
                keyCol = notNullNode;
            }

            const axpdef_keycol = rowData[keyCol]?.toString().replace("T00:00:00", "");

            let subEntityCaption = rowData.entityname || _this.entityName;

            html +=
                `<div class="cc-metrics-panel">
                    <div class="cc-metrics-list">
                        <div class="cc-metrics-item">
                            <div class="cc-metrics-label">
                                <a ref="javascript:void(0)" class="Project_title" onclick="_entityForm.openEntityForm('${subEntityCaption}', '${rowData.transid}', '${rowData.recordid}','${axpdef_keycol}')">${axpdef_keycol}</a>                        
                                ${ generateRowElements(rowData, rowData.transid, "Other Data") }
                            </div>
                        </div>
                    </div>
                 </div>`;           
        }
        //$('#body_Container').html('');
        if (html)
            $(`#kt_accordion_connecteddata_${fldId}`).html(html);
        else {
            html = 
                `<div class="cc-metrics-panel">
                    <div class="cc-metrics-list">
                        <div class="cc-metrics-item">
                            <div class="cc-metrics-label">                        
                                No related data found.
                            </div>
                        </div>
                    </div>
                </div>`
            $(`#kt_accordion_connecteddata_${fldId}`).html(html);
        }
        $(`#related-records-container_${fldId}`).addClass("data-fetched");
        $('[data-toggle="tooltip"]').tooltip();
        initReadMore();
        KTMenu.init();
    }


    constructConnectedDataListHTML(dataJson) {
        let _this = this;
        let html = '';
        dataJson.forEach(data => {
            let item = JSON.parse(data.data_json)[0];
            item.caption = _this.subEntityList[item.transid];
            item.initial = _this.getSubEntityInitials(item.caption);
            if (item.totrec != 0) {
                html += Handlebars.compile(_this.connectedData.accordion)(item);
            }
        });
        return html;
    }

    getSubEntityFilter(subEntityTransId) {
        let _this = this;
        let strFilter = "";
        let selectedEntities = [];

        let subEntity = _this.subEntityMapping[subEntityTransId];
        let subEntityMapFldJson = subEntity.mapfieldjson;
        let mapVal = subEntity["mapdatavalue"];
        let mapFld = subEntity["mapfield"];
        strFilter += `${subEntityTransId}=All=${subEntityMapFldJson.tablename}.${mapFld}~'${mapVal}'~${subEntityMapFldJson.tablename}~${subEntityMapFldJson.normalized}~${subEntityMapFldJson.srctable}~${subEntityMapFldJson.srcfield}~${subEntityMapFldJson.allowempty}++`;
        selectedEntities.push(subEntityTransId);

        if (strFilter.endsWith("++"))
            strFilter = strFilter.substr(0, strFilter.length - 2);

        _this.subEntityFilter = strFilter;
        _this.selectedSubEntities = selectedEntities.join(',');
        return true;
    }

    fetchConnectedDataRecords(transId) {
        let _this = this;
        if (!_this.getSubEntityFilter(transId))
            return;

        ShowHideDimmer(true);
        let url = "../aspx/EntityForm.aspx/GetSubEntityListDataWS";
        let data = { entityName: _this.entityName, transId: _this.entityTransId, recordId: _this.recordId, fields: _this.subEntityFilter, pageNo: 1, pageSize: 10000, metaData: false, subEntityList: _this.selectedSubEntities };
        _this.callAPI(url, data, false, result => {
            ShowHideDimmer(false);
            if (result.success) {
                let subEntityListDataJson = JSON.parse(JSON.parse(result.response).d);
                subEntityListDataJson = subEntityListDataJson.result.list ?? [];
                try {
                    if (subEntityListDataJson.constructor != Array)
                        subEntityListDataJson = JSON.parse(subEntityListDataJson);
                }
                catch {
                    showAlertDialog("error", "Error occurred in data fetch.");
                    return;
                }

                if ((subEntityListDataJson.length || 0) == 0) {
                    _this.listJson = [];
                    showAlertDialog("warning", "No records available.");
                    return;

                }
                else {
                    try {
                        _this.listJson = [];
                        _this.entityWiseFields = {}

                        var newListJson = [];
                        subEntityListDataJson.forEach(item => {
                            newListJson.push(item.data_json);
                        })
                        _this.listJson.push(...newListJson);
                        _this.maxPageNumber = getMaxPageNumber(_this.listJson);
                        noMoreRecordsMessage.style.display = 'none';
                        _this.renderConnectedData(_this.listJson, transId);

                    }
                    catch {

                        $(`#sub-entitycontainer-${transId} .body_Container`).append(JSON.parse(result.response).d);
                    }
                }
            }
        });
    }

    fetchConnectedDataMetrics(transId) {
        let _this = this;
        let criteria = _this.getConnectedDataMetricsCriteria(transId);

        ShowHideDimmer(true);
        let url = "../aspx/EntityForm.aspx/GetEntityFormConnectedDataMetricsWS";
        let data = { transId: transId, recordId: _this.recordId, criteria: criteria };
        _this.callAPI(url, data, false, result => {
            ShowHideDimmer(false);
         
            if (result.success) {
                let connectedDataJson = JSON.parse(JSON.parse(result.response).d);
                connectedDataJson = connectedDataJson.result.data ?? [];

                try {
                    if (connectedDataJson.constructor != Array)
                        connectedDataJson = JSON.parse(connectedDataJson);
                }
                catch {
                    showAlertDialog("error", "Error occurred in connected data fetch.");
                    return;
                }

                if ((connectedDataJson.length || 0) == 0) {
                    $(`#kt_accordion_connecteddata_${transId} .cc-metrics-list`).html(
                        `<div class="cc-metrics-item">
                            <div class="cc-metrics-label">                        
                                No data found
                            </div>
                        </div>`);
                    return;

                }
                else {
                    try {                        
                        var dataJson = JSON.parse(connectedDataJson[0].data_json);
                        var html = "";
                        if (dataJson.length > 0) {
                            Object.keys(dataJson[0]).forEach(key => {
                                var skipFlds = ["cnd", "condition", "criteria", "transid"];
                                if (skipFlds.indexOf(key) == -1) {
                                    var transId = dataJson[0]["transid"];
                                    var fieldName = key.substr(4);
                                    var fieldCaption = _this.metaData.find(x => x.ftransid == transId && x.fldname == fieldName)?.fldcap || fieldName;
                                    var condition = key.substr(0, 3).replace("sum", "Sum").replace("avg", "Avg");
                                    
                                    html += 
                                        `<div class="cc-metrics-item">
                                            <div class="cc-metrics-label">                        
                                                ${fieldCaption} (${condition})
                                            </div>
                                            <div class="cc-metrics-value">${dataJson[0][key]}</div>
                                        </div>`;
                                }
                            });

                            $(`#kt_accordion_connecteddata_${transId} .cc-metrics-list`).html(html);
                        }
                        
                    }
                    catch (e) {
                        console.log("Error" + e);
                    }
                }
            }
        });
    }

    fetchAllConnectedDataMetrics() {
        let _this = this;
        let criteria = _this.getAllConnectedDataMetricsCriteria();
        ShowHideDimmer(true);
        let url = "../aspx/EntityForm.aspx/GetEntityFormConnectedDataMetricsWS";
        let data = { transId: _this.entityTransId, recordId: _this.recordId, criteria: criteria };
        _this.callAPI(url, data, false, result => {
            ShowHideDimmer(false);

            if (result.success) {
                let connectedDataJson = JSON.parse(JSON.parse(result.response).d);
                connectedDataJson = connectedDataJson.result.data ?? [];
                try {
                    if (connectedDataJson.constructor != Array)
                        connectedDataJson = JSON.parse(connectedDataJson);
                }
                catch {
                    showAlertDialog("error", "Error occurred in connected data fetch.");
                    return;
                }

                if ((connectedDataJson.length || 0) == 0) {
                    $(`#kt_accordion_connecteddata_${transId} .cc-metrics-list`).html(
                        `<div class="cc-metrics-item">
                            <div class="cc-metrics-label">                        
                                No data found
                            </div>
                        </div>`);
                    return;

                }
                else {
                    try {
                        var html = "";
                        connectedDataJson.forEach(connectedData => {
                            var dataJson = JSON.parse(connectedData.data_json);                            
                            if (dataJson.length > 0) {

                                html +=
                                    `<div class="cc-metrics-item cc-metrics-caption">
                                                <div class="cc-metrics-label">
                                                    ${_this.subEntityList[dataJson[0]["transid"]]}
                                                </div>
                                            </div>`;
                                Object.keys(dataJson[0]).forEach(key => {
                                    var skipFlds = ["cnd", "condition", "criteria", "transid"];
                                    if (skipFlds.indexOf(key) == -1) {
                                        var transId = dataJson[0]["transid"];
                                        var fieldName = key.substr(4);
                                        var fieldCaption = _this.metaData.find(x => x.ftransid == transId && x.fldname == fieldName)?.fldcap || fieldName;
                                        var condition = key.substr(0, 3).replace("sum", "Sum").replace("avg", "Avg");

                                        html +=
                                            `<div class="cc-metrics-item">
                                                <div class="cc-metrics-label">                        
                                                    ${fieldCaption} (${condition})
                                                </div>
                                                <div class="cc-metrics-value">${dataJson[0][key]}</div>
                                            </div>`;
                                    }
                                });
                            }
                        });
                        

                        $(`#kt_accordion_connectedpagesdata .cc-metrics-list`).html(html);

                    }
                    catch (e) {
                        console.log("Error" + e);
                    }
                }
            }
        });
    }

    bindThemeEvent() {
        var _this = this;
        var menuItems = document.querySelectorAll("#selectThemes .menu-link");

        menuItems.forEach(function (item) {
            item.addEventListener("click", function (event) {
                event.preventDefault();
                var target = this.getAttribute("data-target");
                // _this.updateTheme(target);
                _this.updateActiveClass(item);
                
                var data = {
                    page: "ENTITY_FORM",  
                    transId: _this.entityTransId,  
                    properties: {
                        "THEME": target
                    },
                    confirmNeeded: true,  
                    allUsers: true  
                };
                
    
                _entityCommon.setAnalyticsDataWS(data, function () {
                    _this.updateTheme(target);

                    
                }, function (error) {
                    console.error('Error in SetAnalyticsDataWS:', error);
                    ShowHideDimmer(false);
                });
            });
        });
    }

    applyTheme() {
        var _this = this;
        
    
        var storedTheme = this.properties.THEME|| "gradTheme";
    
        _this.updateTheme(storedTheme);
    
        var activeMenuItem = document.querySelector(`#selectThemes .menu-link[data-target="${storedTheme}"]`);
        if (activeMenuItem) {
            _this.updateActiveClass(activeMenuItem);
        }
    }
    

    updateActiveClass(clickedItem) {
        var menuItems = document.querySelectorAll("#selectThemes .menu-link");
        menuItems.forEach(function (item) {
            item.classList.remove("active");
        });

        clickedItem.classList.add("active");
    }

    updateTheme(target) {
        var body = document.body;

        // Remove existing theme-related classes
        body.classList.remove("lightTheme", "blackTheme", "gradTheme", "compactTheme");

        // Add the new theme class based on the data-target attribute
        if (target === "lightTheme" || target === "light") {
            body.classList.add("lightTheme");
        } else if (target === "blackTheme" || target === "dark") {
            body.classList.add("blackTheme");
        } else if (target === "gradTheme" || target === "gradient" || target === "system") {
            body.classList.add("gradTheme");
        } else if (target === "compactTheme" || target === "pattern") {
            body.classList.add("compactTheme");
        }
    }


    createHTML(data, metadata) {
        
        let html = '';
        let dcName = data[0].n.toLowerCase();
        let dcNo = dcName.replace("dc", "");
        if (data[0].hasdatarows && data[0].hasdatarows === 'yes') {
            // Initialize an object to store rows grouped by the 'r' value
            
            const rows = {};
            data.forEach(item => {
                if (item.t !== 'dc') {
                    if (!rows[item.r]) {
                        rows[item.r] = {};
                    }
                    rows[item.r][item.n] = item.v;
                }
            });
    
            html += `<div class="row" style="position: relative; right: 3px;" id="${dcName}" data-name="${dcName}">
                        <div class="dc-heading card-title cursor-pointer collapsible rotate" data-bs-toggle="collapse"
                            aria-expanded="true" data-bs-target="#${dcName}container" >
                            <span class="material-icons material-icons-style material-icons-2 rotate-180">expand_circle_down</span>
                            <label class="dccaption">${getCaption(dcName)}</label>
                        </div>
        
                        <div id="${dcName}container" class="row sub-entity-row collapse show">
                            <div class="table-responsive">
                                <table class="table table-light table-striped table-bordered tabularDC">`;
        
            // Create table header
            html += `<thead><tr>`;
            let dcMetaData = metadata.filter(i => i.datatype != 'dc' && i.dcname == dcName && (i.hidden != null && i.hidden != "T"));
            dcMetaData.forEach(field => {
                html += `<th id="th${field.fname}" data-name="${field.fname}"><label>${field.caption}</label></th>`;
            });
            html += `</tr></thead>`;
        
            // Create table body
            html += `<tbody>`;
            Object.keys(rows).forEach(rowKey => {
                html += `<tr>`;
                dcMetaData.forEach((field, index) => {
                    let fieldValue = rows[rowKey][field.fname] || '';
                    let fieldId = getFieldId(field.fname, dcNo, true, rowKey);
                    UpdateAllFieldValues(fieldId, fieldValue);
                    let cellContent = '';
        
                    if (field.customdatatype === 'Currency') {
                        cellContent = _entityCommon.inValid(fieldValue) ? "--" : formmatingtoillions(fieldValue);
                    } else if (field.customdatatype === 'Numeric') {
                        // For numeric values, you can add formatting if needed
                        cellContent = _entityCommon.inValid(fieldValue) ? "--" : fieldValue;
                    } else if (field.fname.startsWith("axpfile_")) {
                        const axpfilepathUploadObject = _entityForm.entityFormJson.data.find(i => i.n === field.fname.replace("axpfile_", "axpfilepath_"));
                        var pathVal = axpfilepathUploadObject ? axpfilepathUploadObject.v : "";
                        if (pathVal.indexOf(";bkslh") != -1) {
                            pathVal = pathVal.replace(new RegExp(";bkslh", "g"), "\\");
                        }

                        if (pathVal.endsWith("\\"))
                            pathVal = pathVal.substr(0, pathVal.length - 1);
        
                        var item = {};
                        item["v"] = ReverseCheckSpecialChars(fieldValue);
                        item["filepath"] = pathVal;
                        cellContent = this.createAxpFileAttachmentHTML(item, index, field.fname);
                    }
                    else if (field.fname.startsWith("dc") && field.fname.endsWith("_image")) {
                        var item = {};
                        item["v"] = ReverseCheckSpecialChars(fieldValue);
                        cellContent = this.createDCAttachmentHTML(item, index, "");
                    } else {
                        cellContent = ReverseCheckSpecialChars(fieldValue);
                    }
        
                    // Determine if the cell should be right aligned
                    const isRightAligned = (field.customdatatype === 'Currency' || field.customdatatype === 'Numeric');
                    html += `<td${isRightAligned ? ' class="align-right"' : ''} id="${fieldId}" data-name="${field.fname}">${cellContent}</td>`;
                });
                html += `</tr>`;
            });
            html += `</tbody>`;
            html += `</table></div></div></div>`;
        } else {
            var i = 1;
            var visibleItems = 0;
            var dcHTML = `<div class="row" style="position: relative; right: 3px;" id="${dcName}" data-name="${dcName}">
                            <div class="dc-heading card-title cursor-pointer collapsible rotate" data-bs-toggle="collapse"
                                aria-expanded="true" data-bs-target="#${dcName}container" >
                                <span class="material-icons material-icons-style material-icons-2 rotate-180">expand_circle_down</span>
                                <label class="dccaption">${getCaption(dcName)}</label>
                            </div>
                            <div id="${dcName}container" class="row sub-entity-row collapse show">`;
        
            data.forEach(item => {
                if (item.n) {
                    var isHidden = getHiddenFlag(item.n);
                    if (isHidden != null && isHidden != "T") {
                        visibleItems++;
                        dcHTML += this.nongridDcHTML(item, i, dcNo);
                    }
                }
                i++;
            });
            dcHTML += `</div></div>`;
        
            if (visibleItems > 0) {
                html += dcHTML;
            }
        }
        
        return html;
    }
    
    
    
    groupByDC(data) {
        const groups = [];
        let currentGroup = [];
        let hideDc = false;
        data.forEach(item => {
            if (item.t === 'dc') {
                hideDc = false;

                if (item.n && this.hideControls.indexOf(item.n?.toLowerCase()) > -1)
                    hideDc = true;

                if (currentGroup.length > 0) {
                    groups.push(currentGroup);
                }
                currentGroup = [];
            }

            if (!hideDc && item.n && !(this.hideControls.indexOf(item.n?.toLowerCase()) > -1)) {
                currentGroup.push(item);
            }

        });
        if (currentGroup.length > 0) {
            groups.push(currentGroup);
        }
        return groups;
    }
    generateHTML(groups, metadata) {
        let html = '';
        groups.forEach(group => {
            html += this.createHTML(group, metadata);
        });
        return html;
    }
    nongridDcHTML(fldjson, index, dcNo) {
        
        var fldName = fldjson.n;
        var fldVal = ReverseCheckSpecialChars(fldjson.v);
        var fldCaption = getCaption(fldjson.n);
        var comphtml = "";
        var finalhtml;
        let fieldType = fldjson.t.toUpperCase();
        let fieldId = getFieldId(fldName, dcNo, false, fldjson.r)
        UpdateAllFieldValues(fieldId, fldVal);
        // Determine align-right class if customdatatype is Numeric or Currency
        let alignClass = "";
        if (this.flds && this.flds[fldName] && 
            (this.flds[fldName].customdatatype === "Currency" || this.flds[fldName].customdatatype === "Numeric")) {
            alignClass = " align-right";
        }
    
        if (fieldType != 'DC') {
            let componentHtml = this.components[fieldType]?.html || `<div class="inputcontent" readonly id="{{fieldid}}">{{v}}</div>`;
    
            if (fldVal != "") {
                var colclassname = this.getclassname(fldVal, '');
                if (!colclassname.startsWith("col")) {
                    var stylewidth = colclassname;
                    colclassname = "";
                } else {
                    stylewidth = "";
                }
            }
    
            if (fieldType == "RT" || fieldType == "LT") {
                comphtml = this.createRichTextComponent(fldjson, index);
            } else if (fieldType == "ATT" || fldName.startsWith("axpfile_")) {
                const axpfilepathUploadObject = _entityForm.entityFormJson.data.find(item => item.n === fldName.replace("axpfile_", "axpfilepath_"));
                var pathVal = axpfilepathUploadObject ? axpfilepathUploadObject.v : "";
    
                if (pathVal?.indexOf(";bkslh") != -1) {
                    pathVal = pathVal.replace(new RegExp(";bkslh", "g"), "\\");
                }
    
                if (pathVal.endsWith("\\")) {
                    pathVal = pathVal.substr(0, pathVal.length - 1);
                }
    
                pathVal = pathVal.replace(new RegExp("\\\\", "g"), "\\\\");
                fldjson.filepath = pathVal;
                comphtml = `<div class="${colclassname} col-sm-12${alignClass}" data-type="${fieldType}" data-name="${fldName}" data-ctype="${this.flds[fldName].customdatatype}" id="{{fieldid}}">
                                <label>${fldCaption}</label>
                                ${this.createAttachmentHTML(fldjson, index, colclassname)}
                            </div>`;
            } else if (fldName.startsWith("dc") && fldName.endsWith("_image")) {
                comphtml = `<div class="${colclassname} col-sm-12${alignClass}" data-type="${fieldType}" data-name="${fldName}" data-ctype="${this.flds[fldName].customdatatype}">
                                <label>${fldCaption}</label>
                                ${this.createDCAttachmentHTML(fldjson, index, colclassname)}
                            </div>`;
            } else if (this.flds[fldName].customdatatype == "Image") {
                if (_entityCommon.isValid(fldjson.v)) {
                    comphtml = `<div class="${colclassname} col-sm-12${alignClass}" data-type="${fieldType}" data-name="${fldName}" data-ctype="${this.flds[fldName].customdatatype}" id="{{fieldid}}">
                                <label>${fldCaption}</label>
                                ${this.getTstructImage(fldjson, index, colclassname)}
                            </div>`;
                }
            } else if (this.flds[fldName].customdatatype == "Check box") {
                var checked = fldVal === "T" ? "checked" : "";
                componentHtml = this.components["togglebutton"].html.replace("{{checked}}", checked);
                comphtml = `<div class="col-xl-3 col-lg-4 Eform_Display_Items col-sm-12" data-type="${fieldType}" data-name="${fldName}" data-ctype="${this.flds[fldName].customdatatype}" id="{{fieldid}}">
                                <label>${fldCaption}</label>
                                ${componentHtml}
                            </div>`;
            } else {
                if (this.flds[fldName].customdatatype === "Currency") {
                    fldVal = _entityCommon.inValid(fldVal) ? "--" : formmatingtoillions(fldVal);                    
                }
                componentHtml = `<div class="inputcontent" readonly>${fldVal}</div>`;
                comphtml = `<div class="${colclassname} col-sm-12${alignClass}" data-type="${fieldType}" data-name="${fldName}" data-ctype="${this.flds[fldName].customdatatype}" id="{{fieldid}}"> 
                                <label>${fldCaption}</label>
                                ${componentHtml}
                            </div>`;
            }
        }
    
        if (fldjson.v)
            fldjson.v = _entityCommon.replaceSpecialChars(fldjson.v);

        fldjson.fieldid = fieldId;
        finalhtml = Handlebars.compile(comphtml)(fldjson);
        return finalhtml;
    }
    
    

    ConstructEntityForm(targetSelector, jsonData) {
        var formContainer = document.querySelector(targetSelector);
        formContainer.className = "container";
        var isgrid = false;
        var currentDC = null;
        var rowdiv = document.createElement("div");
        rowdiv.classList.add("row");

        var gridChildren = [];

        for (var i = 0; i < jsonData.length; i++) {

            if (jsonData[i].props.split("~")[0] == "DC") {
                var dcNumber = jsonData[i].dcno;

                if (currentDC !== dcNumber) {
                    isgrid = false;
                    if (rowdiv.innerHTML !== "") {
                        formContainer.appendChild(rowdiv.cloneNode(true));
                        rowdiv.innerHTML = "";
                    }

                    var rowdiv = document.createElement("div");
                    rowdiv.classList.add("row");
                    rowdiv.style.position = "relative"
                    rowdiv.style.right = "3px"

                    var dcHeadingDiv = document.createElement("div");
                    dcHeadingDiv.className = "dc-heading";
                    dcHeadingDiv.innerHTML = "<h3>" + jsonData[i].caption + "</h3>";

                    rowdiv.appendChild(dcHeadingDiv);
                    formContainer.appendChild(rowdiv);

                    currentDC = dcNumber;
                    if (this.isGridDC(jsonData[i])) {
                        isgrid = true;
                        gridChildren = [];
                    }
                }

            }

            if (isgrid) {
                if (jsonData[i].props.split("~")[0] !== "DC" && this.fldtype.includes(jsonData[i].props
                    .split("~")[0]) && jsonData[i].props.split("~")[2] !== "F") {

                    gridChildren.push(jsonData[i]);


                    if ((i + 1 === jsonData.length || jsonData[i + 1].props.split("~")[0] === "DC") &&
                        gridChildren.length > 0) {

                        var tableHtml = this.gridDc(gridChildren, i);
                        rowdiv.innerHTML += tableHtml;
                        gridChildren = [];
                    }
                }
            } else {
                if (jsonData[i].props.split("~")[0] !== "DC" && this.fldtype.includes(jsonData[i].props
                    .split("~")[0]) && jsonData[i].props.split("~")[2] !== "F") {

                    var c = this.nongridDc(jsonData[i], i);
                    rowdiv.innerHTML += c;
                }
            }
        }



        if (rowdiv.innerHTML !== "") {
            formContainer.appendChild(rowdiv);
        }
    }

    isGridDC(dcData) {
        return dcData.props.split("~")[1] == "T";
    }

    gridDc(jsonData, index) {
        var columnValues = {};
        jsonData.forEach(function (data) {
            var columnName = data.fieldid;
            var columnValue = data.value;


            if (!columnValues[columnName]) {
                columnValues[columnName] = [];
            }

            var rowIndex = data.row - 1;
            if (!columnValues[columnName][rowIndex]) {
                columnValues[columnName][rowIndex] = '';
            }
            var fldtype = data.props.split("~")[0];

            var componentHtml = _entityForm.components[data.props.split("~")[0]]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
            if (fldtype == "ATT" || fldName.startsWith("axpfile_") || (fldName.startsWith("dc") && fldName.endsWith("_image"))) {
                var colclassname = _entityForm.getclassname(data.value, data.props.split("~")[3]);
                //componentHtml = _entityForm.createAttachmentHTML(data, index, colclassname);
                const axpfilepathUploadObject = _entityForm.entityFormJson.data.find(item => item.n === fldName.replace("axpfile_", "axpfilepath_"));
                var pathVal = axpfilepathUploadObject ? axpfilepathUploadObject.v : "";
                if (pathVal.indexOf(";bkslh") != -1) {
                    pathVal = pathVal.replace(new RegExp(";bkslh", "g"), "\\");
                }

                if (pathVal.endsWith("\\"))
                    pathVal = pathVal.substr(0, pathVal.length - 1);

                pathVal = pathVal.replace(new RegExp("\\\\", "g"), "\\\\");

                fldjson.filepath = pathVal;
                componentHtml =
                    `<div class="${colclassname} col-sm-12 " data-type="${fieldType}" data-name="${fldName}" data-ctype="${this.flds[fldName].customdatatype}"><label>${fldCaption}</label>${this.createAttachmentHTML(fldjson, index, colclassname)}</div>`;

                columnValues[columnName][rowIndex] += componentHtml +
                    '<br>';
            } else if (fldtype == "togglebutton" && _entityForm.fldtype.includes(data.props.split("~")[
                0])) {
                var togglehtml = _entityForm.components[fldtype]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
                var checked = data.value === "T" ? "checked" : "";

                togglehtml = togglehtml.replace("{{checked}}",
                    checked);
                componentHtml =
                    `<div class="${colclassname} col-sm-12" style="display:inline-grid;" ><div><label style="font-weight:bold;">${data.caption}</label></div><div>${togglehtml}</div></div>`;
                columnValues[columnName][rowIndex] += componentHtml + '<br>';
            } else {
                columnValues[columnName][rowIndex] += columnValue +
                    '<br>';
            }


        });


        var tableHtml = '<table class="table table-light table-striped table-bordered">';
        tableHtml += '<thead><tr>';


        Object.keys(columnValues).forEach(function (columnName) {
            tableHtml += '<th>' + columnName + '</th>';
        });

        tableHtml += '</tr></thead><tbody>';


        var maxRows = Math.max(...Object.values(columnValues).map(arr => arr.length));
        for (var i = 0; i < maxRows; i++) {
            tableHtml += '<tr>';
            Object.keys(columnValues).forEach(function (columnName) {
                var value = columnValues[columnName][i] ||
                    '';
                tableHtml += '<td>' + value + '</td>';
            });
            tableHtml += '</tr>';
        }


        tableHtml += '</tbody></table>';

        return tableHtml;
    }

    nongridDc(fldjson, index) {

        var comphtml = "";
        var finalhtml;
        let fieldType = fldjson.props.split("~")[0];
        let componentHtml = this.components[fieldType]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
        if (fldjson.value != "") {
            var colclassname = this.getclassname(fldjson.value, fldjson.props.split("~")[3]);
            if (!colclassname.startsWith("col")) {
                var stylewidth = colclassname;
                colclassname = "";
            } else {
                stylewidth = ""
            }
        }

        if (fieldType == "RT" || fieldType == "LT") {
            comphtml = this.createRichTextComponent(fldjson, index);
        } else if (fieldType == "ATT") {
            comphtml = this.createAttachmentHTML(fldjson, index, colclassname)
        } else if (fieldType == "Img") {
            comphtml = this.getimage(fldjson, index, colclassname)
        } else if (fieldType == "togglebutton") {
            var checked = fldjson.value === "T" ? "checked" : "";
            componentHtml = componentHtml.replace("{{checked}}", checked);
            comphtml =
                `<div class="${colclassname} col-sm-12 " style="display:inline-grid;" ><div><label style="font-weight:bold;">${fldjson.caption}</label></div><div>${componentHtml}</div></div>`;

        } else {
            comphtml =
                `<div class="${colclassname} col-sm-12 " style="width:${stylewidth}px;"><div><label style="font-weight:bold;">{{caption}}</label></div><div>${componentHtml}</div></div>`;
        }

        if (fldjson.v)
            fldjson.v = _entityCommon.replaceSpecialChars(fldjson.v);

        finalhtml = Handlebars.compile(comphtml)(fldjson);
        return finalhtml;
    }
    getimage(fldjson, index, colclassname) {
        var imgsrc = this.downloadFileAttachment(fldjson.caption, "")
        let fieldType = fldjson.props.split("~")[0];
        let componentHtml = this.components[fieldType]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
        componentHtml = componentHtml.replace("{{colclassname}}", `${colclassname}`);
        if (imgsrc != undefined) {
            componentHtml = componentHtml.replace("{{src}}", `${imgsrc}`);
        } else {
            componentHtml = componentHtml.replace("{{src}}", `../images/logo.png`);
        }
        var comphtml =
            `<div class="${colclassname} col-sm-12 ">${componentHtml}</div>`
        return comphtml;
    }

    getTstructImage(fldjson, index, colclassname) {
        var imgsrc = "";

        var fileName = ReverseCheckSpecialChars(fldjson.v);
        var fldValuePath = "";
        if (fileName.length > 1) {
            fldValuePath = this.getScriptsPath() + "/" + fileName;
        }

        GetDateTime();
        imgsrc = fldValuePath + "?" + imageSuffix;
        let componentHtml = this.components["Image"]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
        componentHtml = componentHtml.replace("{{colclassname}}", `${colclassname}`);
        if (imgsrc != undefined) {
            componentHtml = componentHtml.replace("{{src}}", `${imgsrc}`);
        }
        return componentHtml;
    }

    getScriptsPath() {
        var path = "";
        var hdnScriptsUrlPath = $j("#hdnScriptsUrlpath");
        if (hdnScriptsUrlPath[0] != undefined)
            path = hdnScriptsUrlPath.val() + "axpert/" + callParentNew("mainSessionId");
        return path;
    }

    createRichTextComponent(fldjson, index) {
        var comphtml = `<div class="col-lg-12 col-sm-12 ">
                        <div class="mb-3">
                            <label style="font-weight:bold;">{{caption}}</label>
                        </div>
                        <div id='textcontent${index}' class="mb-5">${fldjson.value}</div>
                    </div>`;

        if (fldjson.v)
            fldjson.v = _entityCommon.replaceSpecialChars(fldjson.v);

        comphtml = Handlebars.compile(comphtml)(fldjson);
        return comphtml;
    }
    //createAttachmentHTML(fldjson, index, colclassname) {
    //    if (fldjson.v) {
    //        let componentHtml = this.components["ATT"]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
    //        componentHtml = componentHtml.replaceAll("{{customid}}", `fileattached${index}`);
    //        var extension = ReverseCheckSpecialChars(fldjson.v).split('.').pop().toLowerCase();
    //        var fileimg = this.getFileImgHTML(extension)
    //        componentHtml = componentHtml.replace("{{componentimg}}", `${fileimg}`);

    //        if (fldjson.v)
    //            fldjson.v = _entityCommon.replaceSpecialChars(fldjson.v);

    //        var comphtml = Handlebars.compile(componentHtml)(fldjson);
    //        return comphtml;
    //    }
    //    return "";
    //}
    createAttachmentHTML(fldjson, index, colclassname) {
        if (fldjson.v) {
            // Prepare base template
            let componentHtml = this.components["ATT"]?.html || `<div class="inputcontent" readonly>{{v}}</div>`;

            // Split multiple filenames
            let filenames = fldjson.v.split(',').map(f => f.trim()).filter(f => f);
            let finalHtml = '';

            filenames.forEach((filename, idx) => {
                let extension = ReverseCheckSpecialChars(filename).split('.').pop().toLowerCase();
                let fileimg = this.getFileImgHTML(extension);

                // Replace template placeholders
                let fileHtml = componentHtml
                    .replaceAll("{{customid}}", `fileattached${index}_${idx}`)
                    .replace("{{componentimg}}", fileimg);

                let fileData = {
                    ...fldjson,
                    v: _entityCommon.replaceSpecialChars(filename)
                };

                // Compile using Handlebars
                finalHtml += Handlebars.compile(fileHtml)(fileData);
            });

            return finalHtml;
        }
        return "";
    }
    //createAxpFileAttachmentHTML(fldjson, index, fldId) {
    //    if (fldjson.v) {
    //        var fileName = GetFieldsName(fldId);
    //        let componentHtml = this.components["AXPFILE_ATT"]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
    //        componentHtml = componentHtml.replaceAll("{{customid}}", `fileattached${index}`);
    //        var extension = ReverseCheckSpecialChars(fldjson.v).split('.').pop().toLowerCase();
    //        var fileimg = this.getFileImgHTML(extension)
    //        componentHtml = componentHtml.replace("{{componentimg}}", `${fileimg}`);

    //        if (fldjson.v)
    //            fldjson.v = _entityCommon.replaceSpecialChars(fldjson.v);

    //        fldjson.scriptspath = window.parent.mainRestDllPath;
    //        fldjson.sid = window.parent.mainSessionId;
    //        fldjson.fieldid = fldId;


    //        var comphtml = Handlebars.compile(componentHtml)(fldjson);
    //        return comphtml;
    //    }
    //    return "";
    //}

    createAxpFileAttachmentHTML(fldjson, index, fldId) {
        if (fldjson.v) {
            var fileName = GetFieldsName(fldId);
            let componentHtml = this.components["AXPFILE_ATT"]?.html || `<div class="inputcontent" readonly>{{v}}</div>`;

            // Split multiple file names
            let fileNames = fldjson.v.split(',').map(f => f.trim()).filter(f => f);
            let finalHtml = '';

            fileNames.forEach((file, idx) => {
                let extension = ReverseCheckSpecialChars(file).split('.').pop().toLowerCase();
                let fileimg = this.getFileImgHTML(extension);

                // Replace placeholders in HTML template
                let fileHtml = componentHtml
                    .replaceAll("{{customid}}", `fileattached${index}_${idx}`)
                    .replace("{{componentimg}}", fileimg);

                // Prepare data for Handlebars
                let fileData = {
                    ...fldjson,
                    v: _entityCommon.replaceSpecialChars(file),
                    scriptspath: window.parent.mainRestDllPath,
                    sid: window.parent.mainSessionId,
                    fieldid: fldId
                };

                finalHtml += Handlebars.compile(fileHtml)(fileData);
            });

            return finalHtml;
        }
        return "";
    }

    //createDCAttachmentHTML(fldjson, index, colclassname) {
    //    if (fldjson.v) {
    //        let componentHtml = this.components["GRID_ATT"]?.html || `<div class="inputcontent"  readonly>{{v}}</div>`;
    //        componentHtml = componentHtml.replaceAll("{{customid}}", `fileattached${index}`);
    //        var extension = ReverseCheckSpecialChars(fldjson.v).split('.').pop().toLowerCase();
    //        var fileimg = this.getFileImgHTML(extension)
    //        componentHtml = componentHtml.replace("{{componentimg}}", `${fileimg}`);

    //        if (fldjson.v)
    //            fldjson.v = _entityCommon.replaceSpecialChars(fldjson.v);

    //        var comphtml = Handlebars.compile(componentHtml)(fldjson);
    //        return comphtml;
    //    }
    //    return "";

    //}

    createDCAttachmentHTML(fldjson, index, colclassname) {
        if (fldjson.v) {
            let componentHtml = this.components["GRID_ATT"]?.html || `<div class="inputcontent" readonly>{{v}}</div>`;

            // Split the filenames by comma, trim each entry
            let fileNames = fldjson.v.split(',').map(f => f.trim()).filter(f => f);
            let finalHtml = '';

            fileNames.forEach((file, idx) => {
                let extension = ReverseCheckSpecialChars(file).split('.').pop().toLowerCase();
                let fileimg = this.getFileImgHTML(extension);

                // Create HTML with unique custom ID
                let fileHtml = componentHtml
                    .replaceAll("{{customid}}", `fileattached${index}_${idx}`)
                    .replace("{{componentimg}}", fileimg);

                let fileData = {
                    ...fldjson,
                    v: _entityCommon.replaceSpecialChars(file)
                };

                finalHtml += Handlebars.compile(fileHtml)(fileData);
            });

            return finalHtml;
        }
        return "";
    }
    getclassname(comphtml, compfldwidth) {
        var classval = "";
        if (compfldwidth == "" || compfldwidth == undefined || compfldwidth == null) {
            if (comphtml.length > 50) {
                classval = "col-xl-12 col-lg-12 Eform_Display_Items";
            } else if (comphtml.length > 30) {
                classval = "col-xl-6 col-lg-8 Eform_Display_Items";
            } else {
                classval = "col-xl-3 col-lg-4 Eform_Display_Items";
            }
            return classval;
        } else {
            return compfldwidth;
        }
    }
    downloadFileAttachment(filename, filepath) {
        var src = this.getScriptsPath() + "/" + filename;
        ASB.WebService.LoadAxpFileToScript(filepath, filename, CallBackOnAxpFile);

        function CallBackOnAxpFile(furesult, eventArgs) {
            if (typeof furesult != "undefined" && furesult != "" && furesult.indexOf("true:") > -1) {
                let newFileName = furesult.startsWith("true:") ? furesult.substring(5) : furesult;
                let lastSlashIndex = src.lastIndexOf("/");
                let updatedSrc = src.substring(0, lastSlashIndex + 1) + newFileName;
                src = unescape(updatedSrc);
                var idx = src.lastIndexOf("/");
                if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
                if (isMobile) {
                    let _fileName = src.match(/\/([^\/?#]+)$/)[1];
                    OpenPdfFile(_fileName, "", "", "", false);
                } else {
                    window.open(src, "GridUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
                }
            } else {
                src = unescape(src); //to show decode special characters
                var idx = src.lastIndexOf("/");
                if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
                if (isMobile) {
                    let _fileName = src.match(/\/([^\/?#]+)$/)[1];
                    OpenPdfFile(_fileName, "", "", "", false);
                } else {
                    window.open(src, "GridUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
                }
            }
        }

    }

    downloadAxpFileAttachment(fldName, pathSrc, src, event, elem) {
        event.stopPropagation();
        pathSrc = pathSrc.replace(/♠/g, "\'");
        src = src.replace(/♠/g, "\'");
        let axpFName = fldName.substr(7);
        var axpPath = $(elem).data("filepath");
        
        ASB.WebService.LoadAxpFileToScript(axpPath, pathSrc, CallBackOnAxpFile);

        function CallBackOnAxpFile(furesult, eventArgs) {
            if (typeof furesult != "undefined" && furesult != "" && furesult.indexOf("true:") > -1) {
                let newFileName = furesult.startsWith("true:") ? furesult.substring(5) : furesult;
                let lastSlashIndex = src.lastIndexOf("/");
                let updatedSrc = src.substring(0, lastSlashIndex + 1) + newFileName;
                src = unescape(updatedSrc);
                var idx = src.lastIndexOf("/");
                if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
                if (isMobile) {
                    let _fileName = src.match(/\/([^\/?#]+)$/)[1];
                    OpenPdfFile(_fileName, "", "", "", false);
                } else {
                    window.open(src, "GridUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
                }
            } else {
                src = unescape(src); //to show decode special characters
                var idx = src.lastIndexOf("/");
                if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));
                if (isMobile) {
                    let _fileName = src.match(/\/([^\/?#]+)$/)[1];
                    OpenPdfFile(_fileName, "", "", "", false);
                } else {
                    window.open(src, "GridUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");
                }
            }
        }

    }

    showGridAttLink(filename) {
        var src = this.getScriptsPath() + "/" + filename;

        src = src.replace(/♠/g, "\'");
        src = unescape(src); //to show decode special characters
        var idx = src.lastIndexOf("/");
        if (idx > -1) src = src.substring(0, idx + 1) + encodeURIComponent(src.substring(idx + 1, src.length));

        window.open(src, "GridUploadFile", "width=500,height=350,scrollbars=no,resizable=yes");

    }

    downloadAttachment(fileUrl, filename, extension) {
        var link = document.createElement('a');
        link.style.display = 'none';
        document.body.appendChild(link);
        link.href = fileUrl;
        link.download = filename;
        link.click();
        document.body.removeChild(link);
    }
    getFileImgHTML(fileType) {
        switch (fileType) {
            case 'pdf':
                return '<img src="../Images/filetypes/pdf.svg" class="file-img"/>';
            case 'ppt':
            case 'pptx':
                return '<img src="../Images/filetypes/ppt.svg" class="file-img"/>';
            case 'jpeg':
            case 'jpg':
            case 'png':
                return '<img src="../Images/filetypes/images.svg" class="file-img" />';
            case 'doc':
            case 'docx':
                return '<img src="../Images/filetypes/word.svg" class="file-img" />';
            case 'txt':
                return '<img src="../Images/filetypes/text.svg" class="file-img" />';
            case 'xls':
            case 'xlsx':
            case 'csv':
                return '<img src="../Images/filetypes/csv.svg" class="file-img"/>';
            default:
                return '<img src="../Images/filetypes/default.svg" class="file-img" />';
        }
    }
    addmore() {
        document.querySelectorAll(`[id^=${'textcontent'}]`).forEach(element => {

            var maxLength = 350;
            var showText = "..Read More";
            var hideText = "Show Less";
            var content = $(element).html();
            if (content.length > maxLength) {
                var truncatedContent = content.substr(0, maxLength); //
                var hiddenContent = content.substr(maxLength);
                $(element).html(truncatedContent);
                $(element).append('<span class="more-text" style="display:none;">' +
                    hiddenContent +
                    '</span>');
                $(element).append('<a href="#" class="read-more">' + showText +
                    '</a>');
            }

            $(element).on("click", ".read-more", function (e) {
                e.preventDefault();
                var moreText = $(this).text();
                if (moreText === showText) {
                    $(this).text(hideText);
                    $(this).prev(".more-text").show();
                } else {
                    $(this).text(showText);
                    $(this).prev(".more-text").hide();
                }
            });

        });

    }
    readmore() {
        document.querySelectorAll(`[id^=${'textcontent'}]`).forEach(element => {

            var showText = "..Read More";
            var hideText = "Read Less";
            var lines = element.innerHTML.trim().split("\n");
            var firstTwoLines = lines.slice(0, 2);
            var truncatedContent = firstTwoLines.join('\n');
            var tempElement = document.createElement('div');
            tempElement.innerHTML = truncatedContent;
            document.body.appendChild(tempElement);
            var height = tempElement.clientHeight;
            tempElement.parentNode.removeChild(tempElement);


            var hiddenContent = lines.slice(2).join('\n');



            if (firstTwoLines.length >= 2) {
                element.innerHTML = truncatedContent;
                element.insertAdjacentHTML('beforeend',
                    '<span class="more-text" style="display:none;">' + hiddenContent + '</span>'
                );
                element.insertAdjacentHTML('beforeend',
                    '<a href="#" class="read-more" style="float:right;">' + showText + '</a>');
            } else {
                element.innerHTML = truncatedContent;
                element.insertAdjacentHTML('beforeend',
                    '<span class="more-text" style="display:none;">' + hiddenContent + '</span>'
                );
                element.insertAdjacentHTML('beforeend',
                    '<a href="#" class="read-more" style="float:right;display:none;">' +
                    showText + '</a>');

            }
            element.addEventListener("click", function (e) {
                if (!e.target.matches('.read-more')) return;
                e.preventDefault();
                var moreText = e.target.textContent;
                if (moreText === showText) {
                    e.target.textContent = hideText;
                    e.target.previousElementSibling.style.display =
                        'inline';
                } else {
                    e.target.textContent = showText;
                    e.target.previousElementSibling.style.display =
                        'none';
                }
            });
        });
    }

    callAPI(url, data, async, callBack) {
        let _this = this;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, async);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

        xhr.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 200) {
                    callBack({ success: true, response: this.responseText });
                }
                else {
                    _this.catchError(this.responseText);
                    callBack({ success: false, response: this.responseText });
                }
            }
        }
        xhr.send(JSON.stringify(data));
    };


    setSelectedRelatedFields() {
        let _this = this;
        let url = "../aspx/EntityForm.aspx/SetSelectedRelatedDataFieldsWS";
        let data = { transId: _this.entityTransId, fields: _this.relatedDataFields };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                _entityForm.reloadEntityPage();
                ShowHideDimmer(false);
            } else {

                ShowHideDimmer(false);
            }
        });
    }


    setSelectedDisplayFields() {
        let _this = this;
        let url = "../aspx/EntityForm.aspx/SetSelectedDisplayFieldsWS";
        let data = { transId: _this.entityTransId, fields: _this.relatedDataDisplayFields, gridFields: _this.relatedDataDisplayGridFields };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                _entityForm.reloadEntityPage();
                ShowHideDimmer(false);
            } else {

                ShowHideDimmer(false);
            }
        });
    }

    getSubEntityInitials(caption) {
        let initials = '';
        const words = caption.split(' ');

        if (words.length === 1) {
            if (words[0].length == 1)
                initials = words[0].charAt(0);
            else
                initials = words[0].charAt(0) + words[0].charAt(1);
        } else if (words.length === 2) {
            initials = words[0].charAt(0) + words[1].charAt(0);
        } else if (words.length >= 3) {
            initials = words[0].charAt(0) + words[1].charAt(0);
        }

        return initials.toUpperCase();
    }

    renderConnectedData(listJson, transId) {
        let _this = this;
        let html = '';
    
        const filteredRows = listJson;
        Object.keys(_this.subEntityMapping).forEach(subEnt => {
            if (!_this.entityWiseFields[subEnt]) {
                $.each(_this.metaData.filter(i => i.ftransid === subEnt), function (index, field) {
                    _this.populateEntityWiseFlds(index, field);
                });
            }
        });
    
        let customRender = false;
        try {
            customRender = AxRenderConnectedDataCustom(_this, listJson, transId);
        } catch {
            customRender = false;
        }
    
        if (customRender) return;
    
        if (filteredRows.length === 0) {
            html += `<p>No records found.</p>`;
            $(`#sub-entitycontainer-${transId} .body_Container`).append(html);
            return;
        }
    
        html += `<div style="overflow-x: auto; max-width: 100%; white-space: nowrap;">`;
        html += `<table id="subEntityTable_${transId}" class="table table-striped table-bordered">`;
    
        let firstRow = filteredRows[0];
        let validFields = Object.keys(firstRow).filter(field => ![
            "projectid", "axpeg_processname", "axpeg_keyvalue", "axpeg_status",
            "axpeg_statustext", "rno", "pageno", "transid", "recordid",
            "modifiedby", "modifiedon", "createdon", "createdby"
        ].includes(field));
    
        html += `<thead><tr>`;
        html += `<th>Actions</th>`;

        var excludedFields = [];
        validFields.forEach(field => {
            if (typeof _this.entityWiseMetaData[transId]?.[field] == "undefined")
                excludedFields.push(field);
            else {
                let fieldCaption = _this.entityWiseMetaData[transId]?.[field]?.fldcap || field;
                html += `<th>${fieldCaption}</th>`;
            }
        });
        html += `</tr></thead>`;
    
        html += `<tbody>`;
    
        for (let rowData of filteredRows) {
            let keyCol = _this.subEntityMapping[rowData.transid].keycol;
    
            if (!rowData.hasOwnProperty(keyCol)) {
                keyCol = _this.getKeyField(rowData.transid).fldname;
                if (_entityCommon.inValid(rowData[keyCol]))
                    keyCol = Object.keys(rowData)[4];
    
                _this.subEntityMapping[rowData.transid].keycol = keyCol;
            }
    
            rowData.keycol = keyCol;
    
            const axpdef_keycol = rowData[keyCol];
            const subEntityCaption = _this.subEntityList[rowData.transid];
            const subEntityVal = _this.getSubEntityInitials(subEntityCaption);
    
            html += `<tr>`;
            html += `<td style="display: flex; justify-content: center; gap: 10px;">`;
    
            html += `<button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" 
                        data-bs-toggle="tooltip" title="View" 
                        onclick="_entityForm.openEntityForm('${_this.subEntityList[rowData.transid]}', '${rowData.transid}', '${rowData.recordid}', '${axpdef_keycol}', ${rowData.rno})">
                        <span class="material-icons" style="color:red;">visibility</span>
                     </button>`;
    
            html += `<button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" 
                        data-bs-toggle="tooltip" title="Edit" 
                        onclick="_entityForm.editSubEntity('${rowData.transid}','${rowData.recordid}'); return false;">
                        <span class="material-icons material-icons-style material-icons-2" style="color:red;">edit_note</span>
                     </button>`;
    
            html += `</td>`;
    
            validFields.forEach(field => {
                if (excludedFields.indexOf(field) == -1) {
                    let cellValue = rowData[field] != null ? rowData[field] : '';
                    let fieldMeta = _this.entityWiseMetaData[transId]?.[field];

                    // Check if the field's customdatatype is Currency or numeric
                    const isNumeric = !isNaN(parseFloat(cellValue)) && isFinite(cellValue);
                    const isCurrency = fieldMeta?.cdatatype === "Currency";
                    const isDate = fieldMeta?.cdatatype === "Date";

                    // Apply formatting based on customdatatype
                    if (isCurrency) {
                        cellValue = _entityCommon.inValid(cellValue) ? "--" : formmatingtoillions(cellValue);
                    } else if (isDate) {
                        cellValue = formatDateString(cellValue);
                    }

                    // Add a class for right alignment if the field is numeric or currency
                    const alignClass = (isNumeric || isCurrency) ? 'align-right' : '';

                    html += `<td class="${alignClass}">${cellValue}</td>`;
                }
            });
    
            html += `</tr>`;
        }
    
        html += `</tbody></table>`;
        html += `</div>`;
    
        $(`#sub-entitycontainer-${transId} .body_Container`).append(html);
    
        setTimeout(() => {
            $(`#subEntityTable_${transId}`).DataTable({
                paging: true,
                searching: true,
                info: false,
                scrollX: true,
                columnDefs: [{ targets: '_all', className: 'dt-center' }]
            });
        }, 0);
    
        initReadMore();
        KTMenu.init();
        $('[data-toggle="tooltip"]').tooltip();
    }

    constructConnectedDataAccordions(dataJson) {
        let _this = this;
        let accordionHtml = '';
        dataJson.forEach(data => {
            let item = JSON.parse(data.data_json)[0];
            if (item.totrec != 0) {
                //html +=
                //    `<div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap rightpanel-connecteddata-accordion" data-transid="${item.transid}">
                //        <a href="#" class="Invoice-item accordion-header accordion-button fs-4 fw-semibold collapsed" data-bs-toggle="collapse" data-bs-target="#kt_accordion_connecteddata_${item.transid}" aria-expanded="false" aria-controls="kt_accordion_connecteddata_${item.transid}">
                //            <div class="Invoice-icon">
                //                <span class="material-icons material-icons-style material-icons-2">account_tree</span>
                //            </div>
                //            <div class="Invoice-content">
                //                <h6 class="subtitle">${_this.subEntityList[item.transid]}</h6>
                //                <h3 class="title">${item.totrec}</h3>
                //            </div>
                //        </a>
                //        <div id="kt_accordion_connecteddata_${item.transid}" class="accordion-collapse collapse" aria-labelledby="kt_accordion_connecteddata_${item.transid}">
                //            <div class="cc-metrics-panel">
                //                <div class="cc-metrics-list">               
                //                </div>
                //            </div>
                //        </div>
                //    </div> `;

                const subEntityCaption = _this.subEntityList[item.transid];
                accordionHtml += Handlebars.compile(_this.connectedData.accordionWrapper)({ caption: subEntityCaption, transid: item.transid, totrec: item.totrec })
            }
        });


        //if (html) {
        //    document.querySelector('#KPI-2 .row').insertAdjacentHTML("afterbegin", html);
        //    document.querySelector('#KPI-2').classList.remove("d-none");
        //    document.querySelector('#NO-KPI-Items').classList.add("d-none");
        //}

        if (accordionHtml) {
            document.querySelector('#KPI-2').classList.remove("d-none");
            document.querySelector('#NO-KPI-Items').classList.add("d-none");
            document.querySelector('#entityform-container').insertAdjacentHTML("beforeend", accordionHtml);            
        }

        _this.bindAccordionExpandEvent();
    }

    constructCustomHyperLinkAccordions() {       
        let _this = this;        
        let html = ``;       

        document.querySelector('#KPI-2').classList.remove("d-none");
        document.querySelector('#NO-KPI-Items').classList.add("d-none");
        document.querySelector('.right-customhyperlinks').classList.remove("d-none");

        _this.customHyperLinks.forEach(link => {
            html +=
                `<div class="cc-metrics-item">
                    <div class="cc-metrics-label">                        
                        <a href="javascript:void(0)" onclick="_entityCommon.navigateToUrl('${link.link}')">${link.hyperlinkcaption}</a>
                    </div>
                </div>`;
        })

        $("#kt_accordion_connecteddata_hyperlinks .cc-metrics-list").append(html);
        
    }

    constructCustomDataSourceAccordion() {
        let _this = this;
        var input = {
            adsNames: ["cdp_" + _this.entityTransId],
            sqlParams: {}
        };
        
        _entityCommon.getDataFromAxList(input, (result) => {
            //Success callback
            var data = JSON.parse(result).result.data[0].data;

            if (data.length > 0) {
                let html = ``;
                document.querySelector('#KPI-2').classList.remove("d-none");
                document.querySelector('#NO-KPI-Items').classList.add("d-none");
                document.querySelector('.right-customdatasource').classList.remove("d-none");

                data.forEach(item => {
                    html +=
                        `<div class="cc-metrics-item">
                        <div class="cc-metrics-label">
                            ${item.name}
                        </div>
                        <div class="cc-metrics-value">
                            <a href="javascript:void(0)" onclick="_entityForm.navigateToUrl('${item.link}')">${item.value}</a>
                        </div>
                     </div>`;
                })

                $("#kt_accordion_connecteddata_datasource .cc-metrics-list").append(html);
            }            
        }, () => {
            //Error callback
        })

            

    }

    openConnectedDataConfiguration() {
        let _this = this;
        $('#connectedDataConfigModal').modal('show');
        if (!$('#connectedDataConfigModal').hasClass("content-loaded"))
            _this.createConnectedDataConfigLayout();
    }

    closeConnectedDataConfiguration() {
        $('#connectedDataConfigModal').modal('hide');        
    }

    createConnectedDataConfigLayout() {
        let _this = this;
        let html = ``;

        //_this.createOtherDataFieldsLayout()

        Object.keys(_this.subEntityList).forEach(transId => {
            var tab = Handlebars.compile(_this.connectedData.tabMenu)({ transid : transId, caption: _this.subEntityList[transId] });
            $('#connectedDataTabMenu').append(tab);

            var tabBody = Handlebars.compile(_this.connectedData.tabBody)({ transid: transId, caption: _this.subEntityList[transId] });
            $('#connectedDataTabBody').append(tabBody);

            _this.constructedConnectedDataFieldsList(transId);
        })

        _this.constructCustomHyperlinksContainer();

        Object.keys(_this.connectedDataFields).forEach(transId => {
            const fields = _this.connectedDataFields[transId];

            fields.forEach(item => {
                const { fieldname, condition } = item;
                const checkboxId = `chk_${condition}_${fieldname}`;
                const checkbox = document.getElementById(checkboxId);
                if (checkbox && checkbox.getAttribute('data-transid') === transId) {
                    checkbox.checked = true;
                }
            });
        });

        $('#connectedDataConfigModal').addClass("content-loaded");
        $("#connectedDataConfigModal #connectedDataTabMenu .nav-link")?.[0]?.click();
    }

    constructedConnectedDataFieldsList(transId) {
        let _this = this;
        var fields = _this.metaData.filter(item => item.ftransid === transId && item.aggfield === "T" && item.hide === "F" && item.griddc == "F");
        const groupedFields = {};
        fields.forEach(field => {
            if (!groupedFields[field.dcname]) {
                groupedFields[field.dcname] = [];
            }
            groupedFields[field.dcname].push(field);
        });

        var html = '';
        Object.entries(groupedFields).forEach(([dc, dcFields]) => {
            let collapsed = false;

            let dcName = _entityForm.entityFormJson.metadata.find(item => item.fname == dc).caption || dc;
            html += `
        <div class="card KC_Items">
            <div class="card-header collapsible cursor-pointer rotate  ${collapsed ? "collapsed" : ""}" data-bs-toggle="collapse" aria-expanded="true" data-bs-target="#conn-fields-${dc}">
                <h3 class="card-title">${dcName} (${dc})</h3>
                <div class="card-toolbar rotate-180">
                    <span class="material-icons material-icons-style material-icons-2">
                        expand_circle_down
                    </span>
                </div>
            </div>
            <div class="KC_Items_Content collapse ${collapsed ? "" : "show"} heightControl pt-0---" id="conn-fields-${dc}">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>Field Name</th>
                        <th>Sum</th>
                        <th>Avg</th>
                    </tr>
                </thead>
                <tbody>`;

            dcFields.forEach(fld => {
                html +=
                    `<tr>
                        <td><label for="chk_${fld.fldname}">${fld.fldcap || ''} (${fld.fldname})</label></td>
                        <td><input type="checkbox" id="chk_sum_${fld.fldname}" class="chk-sum" value="sum_${fld.fldname}" data-dcname="${dc}" data-transid="${fld.ftransid}" data-fieldname="${fld.fldname}" data-condition="sum"></td>
                        <td><input type="checkbox" id="chk_avg_${fld.fldname}" class="chk-avg" value="avg_${fld.fldname}" data-dcname="${dc}" data-transid="${fld.ftransid}" data-fieldname="${fld.fldname}" data-condition="avg"></td>
                    </tr>`;

            })
            html += `</tbody></table></div></div>`;
        })

        if(html)
            $(`#connecteddata-fieldslist-${transId}`).html(html);
        else
            $(`#connecteddata-fieldslist-${transId}`).html(`No fields availabe for configuration.`);       
    }

    constructCustomHyperlinksContainer() {
        let _this = this;
        var hyperlinkGridBody = $('#hyperlinkGridBody');
        var addRowBtn = $('#addRowBtn');
    
        function addRow(hyperlink = {}) {
            var newRow = `
                <tr>
                    <td class="serial-number">${hyperlinkGridBody.find('tr').length + 1}</td>
                    <td>
                        <input type="text" class="form-control hyperlink-name" 
                               placeholder="Hyperlink Name" 
                               value="${hyperlink.hyperlinkcaption || ''}" required>
                    </td>
                    <td>
                        <input type="text" class="form-control link-url" 
                               placeholder="Enter Link" 
                               value="${hyperlink.link || ''}" required>
                    </td>
                    <td class="text-center">
                        <button class="btn btn-sm delete-row" type="button">
                            <i class="material-icons">delete</i>
                        </button>
                    </td>
                </tr>
            `;
            hyperlinkGridBody.append(newRow);
            updateDeleteButtonState();
        }
    
        addRow();
    
        if (_this.customHyperLinks?.length > 0) {
            try {
                hyperlinkGridBody.empty();
                _this.customHyperLinks.forEach(function (hyperlink) {
                    addRow(hyperlink);
                });
            } catch (error) {
                alert('Invalid JSON: ' + error.message);
            }
        }
    
        addRowBtn.on('click', function () {
            addRow();
        });
    
        hyperlinkGridBody.on('click', '.delete-row', function () {
            if (hyperlinkGridBody.find('tr').length > 1) {
                $(this).closest('tr').remove();
                reorderSerialNumbers();
                updateDeleteButtonState();
            }
        });
    
        function reorderSerialNumbers() {
            hyperlinkGridBody.find('tr').each(function (index) {
                $(this).find('.serial-number').text(index + 1);
            });
        }
    
        function updateDeleteButtonState() {
            var deleteButtons = hyperlinkGridBody.find('.delete-row');
            deleteButtons.prop('disabled', hyperlinkGridBody.find('tr').length <= 1);
        }
    }
    

    getCustomHyperLinksJSON() {
        var hyperlinkGridBody = $('#hyperlinkGridBody');
        var hyperlinks = [];

        // Validate inputs
        hyperlinkGridBody.find('tr').each(function () {
            var hyperlinkName = $(this).find('.hyperlink-name').val().trim();
            var linkUrl = $(this).find('.link-url').val().trim();

            if (hyperlinkName && linkUrl) {
                hyperlinks.push({
                    "hyperlinkcaption": hyperlinkName,
                    "link": linkUrl
                });
            }           
        });

        return hyperlinks;

    }

    getConnectedDataSelectionJSON() {
        const result = {};
        
        const checkboxes = document.querySelectorAll('.chk-sum:checked, .chk-avg:checked');

        checkboxes.forEach(checkbox => {
            const transId = checkbox.getAttribute('data-transid');
            const fieldName = checkbox.getAttribute('data-fieldname');
            const condition = checkbox.getAttribute('data-condition');

            if (!result[transId]) {
                result[transId] = [];
            }

            result[transId].push({
                fieldname: fieldName,
                condition: condition
            });
        });

        return result;
    }

    getSelectedRelatedDataFieldsSelection() {
        let _this = this;
        let selectedFields = document.querySelectorAll(".chk-relateddataflds:checked");

        // Check if no checkboxes are selected
        if (selectedFields.length === 0) {
            showAlertDialog("error", "Error: No fields are selected.");
            return; // Exit the function
        }

        let fields = "";
        let isDc1FldSelected = false;
        selectedFields.forEach((field) => {
            const fieldName = field.value;
            const dcName = field.dataset.dcname;
            if (!isDc1FldSelected)
                isDc1FldSelected = (field.dataset.dcname === "dc1");

            fields += `${dcName}~${fieldName},`;
        });

        if (!isDc1FldSelected) {
            showAlertDialog("error", "Error: No DC1 fields are selected. Please select atleast one DC1 field.");
            return;
        }

        if (fields.endsWith(","))
            fields = fields.substr(0, fields.length - 1);

        _this.relatedDataFields = fields;

        selectedFields = document.querySelectorAll(".chk-displayflds:checked");

        fields = "";
        selectedFields.forEach((field) => {
            const fieldName = field.value;
            const dcName = field.dataset.dcname;
            fields += `${dcName}~${fieldName},`;
        });

        if (fields.endsWith(","))
            fields = fields.substr(0, fields.length - 1);

        let fldsStr = "";
        let gridFldsStr = "";
        let groupedSelectedFlds = {}
        selectedFields.forEach((field) => {
            let dcName = `${field.dataset.dcname}~${field.dataset.griddc}`;
            if (!groupedSelectedFlds[dcName])
                groupedSelectedFlds[dcName] = [];

            const fieldName = field.value;
            groupedSelectedFlds[dcName].push(fieldName);
        });

        Object.entries(groupedSelectedFlds).forEach(([dcName, fields]) => {
            let isGridDc = dcName.split("~")[1] == "T";
            const tempFldData = _entityForm.metaData.find(item => item.fldname === fields[0] && item.ftransid == _entityForm.entityTransId);
            if (!isGridDc) {
                fldsStr += `${tempFldData.tablename}=`;

                fields.forEach(fld => {
                    const fieldData = _entityForm.metaData.find(item => item.fldname === fld && item.ftransid == _entityForm.entityTransId);
                    fldsStr += `${fieldData.fldname}~${fieldData.normalized}~${fieldData.srctable || ""}~${fieldData.srcfield || ""}~${fieldData.allowempty}|`;
                });
                if (fldsStr.endsWith("|"))
                    fldsStr = fldsStr.substr(0, fldsStr.length - 1);

                fldsStr += `^`;
            }
            else {
                gridFldsStr += `${tempFldData.tablename}=`;

                fields.forEach(fld => {
                    const fieldData = _entityForm.metaData.find(item => item.fldname === fld);
                    gridFldsStr += `${fieldData.fldname}~${fieldData.normalized}~${fieldData.srctable || ""}~${fieldData.srcfield || ""}~${fieldData.allowempty}|`;
                });
                if (gridFldsStr.endsWith("|"))
                    gridFldsStr = gridFldsStr.substr(0, gridFldsStr.length - 1);

                gridFldsStr += `^`;
            }
        });

        if (fldsStr.endsWith("^"))
            fldsStr = fldsStr.substr(0, fldsStr.length - 1);

        if (gridFldsStr.endsWith("^"))
            gridFldsStr = gridFldsStr.substr(0, gridFldsStr.length - 1);


        _this.relatedDataDisplayFields = fldsStr;
        _this.relatedDataDisplayGridFields = gridFldsStr;
    }

    resetConnectedDataConfig() {
        if (!confirm("Reset the selections?"))
            return;

        let _this = this;
        var data = {
            page: "ENTITY_FORM",
            transId: _this.entityTransId,
            properties: {
                "CONNECTEDDATA_CONFIG": JSON.stringify({
                    "HYPERLINKS": [],
                    "CONNECTEDDATA_METRICS": {},
                    "RELATEDDATAFIELDS": "",
                    "DISPLAYFIELDS": "",
                    "DISPLAYGRIDFIELDS": ""
                })
                
            },
            confirmNeeded: true,
            allUsers: true
        };

        _entityCommon.setAnalyticsDataWS(data, () => {
            showAlertDialog("success", "Changes applied successfully");
            _this.reloadEntityPage();
        }, (error) => {
            showAlertDialog("error", error.status + " " + error.statusText);
        });

    }

    applyConnectedDataConfig() {
        let _this = this;
        //_this.getSelectedRelatedDataFieldsSelection();
        var data = {
            page: "ENTITY_FORM",
            transId: _this.entityTransId,
            properties: {
                "CONNECTEDDATA_CONFIG": JSON.stringify({
                    "HYPERLINKS": _this.getCustomHyperLinksJSON() || [],
                    "CONNECTEDDATA_METRICS": _this.getConnectedDataSelectionJSON() || {},
                    "RELATEDDATAFIELDS": "", //_this.relatedDataFields,
                    "DISPLAYFIELDS": "", // _this.relatedDataDisplayFields,
                    "DISPLAYGRIDFIELDS": "", //_this.relatedDataDisplayGridFields
                })
            },
            confirmNeeded: true,
            allUsers: true
        };

        _entityCommon.setAnalyticsDataWS(data, () => {
            showAlertDialog("success", "Changes applied successfully");
            _this.reloadEntityPage();
        }, (error) => {
            showAlertDialog("error", error.status + " " + error.statusText);
        });
    }

    createOtherDataFieldsLayout() {
        let _this = this;
        const fieldsContainer = document.getElementById("fields-selection");
        var fields = _entityForm.metaData.filter(item => item.ftransid === _entityForm.entityTransId && item.hide === "F" && (item.cdatatype === "DropDown" || item.fdatatype == "c" || item.fdatatype == "d"));
        const groupedFields = {};
        fields.forEach(field => {
            if (!groupedFields[field.dcname]) {
                groupedFields[field.dcname] = [];
            }
            groupedFields[field.dcname].push(field);
        });

        var html = '';
        Object.entries(groupedFields).forEach(([dc, dcFields]) => {
            let collapsed = false;

            let dcName = _entityForm.entityFormJson.metadata.find(item => item.fname == dc).caption || dc;
            html += 
                `<div class="card KC_Items">
                    <div class="card-header collapsible cursor-pointer rotate  ${collapsed ? "collapsed" : ""}" data-bs-toggle="collapse" aria-expanded="true" data-bs-target="#rel-fields-${dc}">
                        <h3 class="card-title">${dcName} (${dc})</h3>
                        <div class="card-toolbar rotate-180">
                            <span class="material-icons material-icons-style material-icons-2">
                                expand_circle_down
                            </span>
                        </div>
                    </div>
                    <div class="KC_Items_Content collapse ${collapsed ? "" : "show"} heightControl pt-0---" id="rel-fields-${dc}">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th>Field Name</th>
                                <th>Show in right data panel</th>
                                <th>Show in data panel listing </th>
                            </tr>
                        </thead>
                        <tbody id="fields-table-body">`;

            dcFields.forEach(fld => {
                html +=
                    `<tr>                        
                        <td><label>${fld.fldcap || ''} (${fld.fldname})</label></td>
                        <td><input type="checkbox" id="chk_${fld.fldname}" class="chk-fields chk-relateddataflds" value="${fld.fldname}" data-dcname="${dc}"></td>
                        <td><input type="checkbox" id="chkdisp_${fld.fldname}" class="chk-fields chk-displayflds" value="${fld.fldname}" data-dcname="${dc}"></td>
                    </tr>`;

            })
            html += `</tbody></table></div></div>`;

        })

        fieldsContainer.innerHTML = html;

        if (_this.relatedDataFields !== "") {
            _this.relatedDataFields.split(",").forEach(fld => {
                var fldName = fld.split("~")[1];
                document.querySelector(`#chk_${fldName}`).checked = true;
            });
        }

        if (_this.relatedDataDisplayFields !== "") {
            _this.relatedDataDisplayFields.split("^").forEach(dc => {
                dc.split("|").forEach(fld => {
                    var fldName = (fld.indexOf("=") > -1) ? fld.split("~")[0].split('=')[1] : fld.split("~")[0];
                    document.querySelector(`#chkdisp_${fldName}`).checked = true;
                });
            });
        }

    }

    fetchFilteredData(fldId, fldVal, dcName, gridDc, rowNo) {
        let _this = this;
        var tmpFld = _this.metaData.find(item => item.fldname === fldId && item.ftransid === _this.entityTransId)
        let isListingFld = "F";
        if (_this.relatedDataDisplayFields.indexOf(`=${fldId}~`) > -1 || _this.relatedDataDisplayFields.indexOf(`|${fldId}~`) > -1 || (dcName == "dc1" && (_this.relatedDataDisplayFields == "All" || _this.relatedDataDisplayFields == "")))
            isListingFld = "T";
        var filterStr = `${fldId}~${tmpFld.normalized}~${tmpFld.srctable || ""}~${tmpFld.srcfield || ""}~${tmpFld.fdatatype || ""}~${isListingFld || "F"}~${tmpFld.tablename || ""}| = '${fldVal.toLowerCase()}'`;

        _this.filter = filterStr;
        _this.fields = _this.relatedDataDisplayFields || "All";
        _this.getEntityListDataForOtherData(1, `${dcName}${rowNo}${fldId}`, gridDc);
    }

    getKeyField(transId) {
        let _this = this;
        if (_this.subEntityKeyFields && _this.subEntityKeyFields[transId]) {
            return { "fldname": _this.subEntityKeyFields[transId] };
        }

        let metaData = _this.metaData.filter(item => item.ftransid === transId && item.hide === "F");

        // Filter objects with "cdatatype": "Autogenerate"
        const autoGenerateField = metaData.filter(item => item.cdatatype === "Auto Generate").find(item => typeof _this.entityFormJson.data.find(i => i.n == item.fldname)?.v != "undefined");
        if (autoGenerateField) {
            return autoGenerateField;
        }

        // Filter objects with hide = F and mandatory/allow empty = F and allowduplicate = F
        const mandatoryUniqueFld = metaData.filter(item => item.allowempty === "F" && item.allowduplicate === "F").find(item => typeof _this.entityFormJson.data.find(i => i.n == item.fldname)?.v != "undefined");
        if (mandatoryUniqueFld) {
            return mandatoryUniqueFld;
        }

        // Filter objects with hide = F and mandatory/allow empty = F and allowduplicate = F
        const uniqueFld = metaData.filter(item => item.allowduplicate === "F").find(item => typeof _this.entityFormJson.data.find(i => i.n == item.fldname)?.v != "undefined");
        if (uniqueFld) {
            return uniqueFld;
        }

        // Filter objects with hide = F and mandatory/allow empty = F
        const mandatoryFld = metaData.filter(item => item.allowempty === "F").find(item => typeof _this.entityFormJson.data.find(i => i.n == item.fldname)?.v != "undefined");
        if (mandatoryFld) {
            return mandatoryFld;
        }

        var firstValueField = metaData.find(item => typeof _this.entityFormJson.data.find(i => i.n == item.fldname)?.v != "undefined")
        if (firstValueField) {
            return firstValueField;
        }
        return metaData[0];
    }
}

function getCaption(fname) {
    var metadata = _entityForm.entityFormJson.metadata;
    for (var i = 0; i < metadata.length; i++) {
        if (metadata[i].fname === fname) {
            return metadata[i].caption;
        }
    }
    return null;
}
function getHiddenFlag(fname) {
    var metadata = _entityForm.entityFormJson.metadata;
    for (var i = 0; i < metadata.length; i++) {
        if (metadata[i].fname === fname) {
            return metadata[i].hidden;
        }
    }
    return null;
}

/* Function Set to generate html */
function generateRowElements(rowData, subEntity, calledFrom) {
    var RowElements = '<div class="Data-fields-row">';
    if (calledFrom && calledFrom == "Other Data") {
        RowElements = '<div class="Data-fields-row relateddata-row workflow-items">';
        for (const fld of _entityForm.entityWiseFields[subEntity].otherElements) {
            RowElements += generateHTMLBasedForOtherData(fld, rowData);
        }

        if (RowElements.endsWith(',&nbsp;'))
            RowElements = RowElements.substr(0, RowElements.length - 7);
    }
    else {
        for (const fld of _entityForm.entityWiseFields[subEntity].otherElements) {
            RowElements += generateHTMLBasedOnDataType(fld, rowData);
        }
        for (const fld of _entityForm.entityWiseFields[subEntity].largeTextElements) {
            RowElements += generateHTMLBasedOnDataType(fld, rowData);
        }
        for (const fld of _entityForm.entityWiseFields[subEntity].attachmentElements) {
            RowElements += generateHTMLBasedOnDataType(fld, rowData);
        }
    }
    RowElements += '</div>';

    if (calledFrom && calledFrom == "Other Data") {
        RowElements = RowElements.replaceAll("txt-bold", " ");
    }
    return RowElements;
}
function generateRowButtonHTML(rowData, subEntity) {
    var rowButtonHTML = "";
    for (const fld of _entityForm.entityWiseFields[subEntity].buttonElements) {
        rowButtonHTML += `<div class="menu-item px-3 my-0">`;
        rowButtonHTML += generateHTMLBasedOnDataType(fld, rowData);
        rowButtonHTML += `</div>`;
    }
    return rowButtonHTML;
}

function getFieldDataType(fldProps) {
    if (_entityCommon.inValid(fldProps.cdatatype)) {
        if (fldProps.fdatatype == "n")
            return "Number";
        else if (fldProps.fdatatype == "d")
            return "Date";
        else if (fldProps.fdatatype == "c")
            return "Text";
        else if (fldProps.fdatatype == "i")
            return "Image";
        else if (fldProps.fdatatype == "t")
            return "Large Text";
        else
            return "Text";
    }
    else
        return fldProps.cdatatype;
}


function generateHTMLBasedOnDataType(fld, rowData) {
    var fldName = fld.fldname;
    if ((fldName == _entityForm.keyField && rowData.transid == _entityForm.entityTransId) || (fldName == rowData.keycol && rowData.transid == fld.ftransid))
        return '';
    var fldtype = getFieldDataType(fld);
    var fCaption = fld.fldcap || '';
    var fProps = fld.props;
    var fldValue = rowData[fldName];
    if ((_entityCommon.inValid(fldValue) && fldtype.toUpperCase() != "BUTTON") || fld.fldname == "transid")
        return '';

    if (fldtype.toUpperCase() != "BUTTON")
        fldValue = fldValue?.toString().replace("T00:00:00", "");

    let html = '';
    switch (fldtype) {
        case 'Large Text':
            html = `<div class=" Data-fields-items Department-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                               <p class="task-description moretext" style="margin-bottom:0px !important;">
															${fldValue}
														 </p>
														 <a class="moreless-button" href="#">Read more</a>
                                            </div>`;
            break;
        case 'Short Text':
            html = `<div class=" Data-fields-items Department-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">description</span><span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
            break;
        case 'Currency':
            html = `<div class=" Data-fields-items Department-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">payments</span><span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
            break;
        case 'Date':
            html = `<div class=" Data-fields-items Date-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">today</span><span class="txt-bold Data-field-value"> Dec 14 2023 </span>
                                            </div>`;
            break;
        case 'Time':
            html = ` <div class=" Data-fields-items Time-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">schedule</span><span class="txt-bold Data-field-value">11:30 PM</span>
                                            </div>`;
            break;
        case 'Link':
            html = `<div class=" Data-fields-items link-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">link</span><span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
            break;
        case 'Mobile':
            html = `<div class=" Data-fields-items Email-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">phone</span><span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
            break;
        case 'Phone':
            html = `<div class=" Data-fields-items Email-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">phone</span><span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
            break;
        case 'Pincode':
            html = `<div class=" Data-fields-items Email-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">location_on</span><span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
            break;
        case 'Zipcode':
            html = `<div class=" Data-fields-items Email-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">location_on</span><span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
            break;
        case 'Email':
            html = `<div class=" Data-fields-items Email-field truncate" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="material-icons material-icons-style material-icons-2 ">mail</span><span class="txt-bold Data-field-value" data-text="${fldValue}" onclick="showPopup(this)">${fldValue}</span>
                                            </div>`;
            break;
        case 'Bool':
            if (fldValue == "T")
                html = `<div class=" Data-fields-items Email-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                    <div class="form-check form-switch">
                          <input class="form-check-input" type="checkbox" role="switch" checked disabled />
                          <span class="txt-bold Data-field-value">${fCaption}</span>
                        </div></div>`;
            else
                html = `<div class=" Data-fields-items Email-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                    <div class="form-check form-switch" style="padding-top: 5px;">
                          <input class="form-check-input" type="checkbox" role="switch" disabled />
                          <span class="txt-bold Data-field-value">${fCaption}</span>
                        </div></div>`;
            break;
        case 'button':
            var propsVal = fProps.split("|");
            var iconVal = propsVal[0].split("~")[1];
            html = `<a href="javascript:void(0)" title="View form"
                                                   class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  btn-sm  me-2 ">
                                                    <span class="material-icons material-icons-style material-icons-2"
                                                          style="color: darkmagenta;">${iconVal}</span>${fCaption}
                                                </a>`;
            break;
        case 'Attachments':
            var fileType = getFileType(fldValue);
            var iconClass = getIconClass(fileType);
            var attachmentURL = "";
            html = `<div class="Files-Attached " data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
						   ${iconClass}	
						   <div class="ms-1 fw-semibold">							  
							  <a class="attached-filename" onclick="downloadFileFromPath('${attachmentURL}','${fldValue}')">${fldValue}</a>						  							  
						   </div>						  						   
						</div>`;
            break;
        default:
            html = `<div class=" Data-fields-items Department-field" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                                                <span class="txt-bold Data-field-value">${fldValue}</span>
                                            </div>`;
    }

    return html;
}

function generateHTMLBasedForOtherData(fld, rowData) {
    var fldName = fld.fldname;
    if ((fldName == _entityForm.keyField && rowData.transid == _entityForm.entityTransId) || (fldName == rowData.keycol && rowData.transid == fld.ftransid))
        return '';
    var fldtype = getFieldDataType(fld);
    var fCaption = fld.fldcap || '';
    var fProps = fld.props;
    var fldValue = rowData[fldName];
    if (_entityCommon.inValid(fldValue) || fld.fldname == "transid")
        return '';

    fldValue = fldValue?.toString().replace("T00:00:00", "");
    let html = `<div class="" data-toggle="tooltip" data-placement="top" title="${fCaption}" data-name="${fCaption.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                <span class="txt-bold Data-field-value">${fldValue}</span>
            </div>,&nbsp;`;

    return html;
}
// Event listener for scroll events
// Add a scroll event listener to your div
function isScrollAtBottomWithinDiv(divElement) {
    // Calculate the distance between the top of the div and the bottom of the viewport
    const distanceToBottom = divElement.scrollHeight - (divElement.scrollTop + divElement.clientHeight);

    // Check if the distance to the bottom is within a small threshold to account for rounding errors
    return distanceToBottom <= 1;
}
function isScrollAtTopWithinDiv(divElement) {
    // Calculate the distance between the top of the div and the top of the viewport
    const distanceToTop = divElement.scrollTop;

    // Check if the distance to the top is within a small threshold to account for rounding errors
    return distanceToTop <= 1;
}
function filterRowsByPage(rows, pageNumber) {
    return rows.filter(row => row.pageno === pageNumber);
}
function getMaxPageNumber(data) {
    let maxPage = -Infinity; // Initialize with negative infinity
    //data = JSON.parse(data);
    // Iterate over each row and update maxPage if needed
    data.forEach(row => {
        if (row.pageno > maxPage) {
            maxPage = row.pageno;
        }
    });

    return maxPage;
}
function initReadMore() {
    // Get all elements with the "moretext" class
    var moreTextElements = document.querySelectorAll('.moretext');

    moreTextElements.forEach(function (moreText, index) {
        // Save the initial height when the page loads for each element
        var initialHeight = moreText.clientHeight;

        // Get the corresponding "Read more" button
        var moreLessButton = moreText.nextElementSibling;

        // Initial check and setup
        if (moreText.scrollHeight > moreText.clientHeight) {
            moreLessButton.style.display = 'inline-block';
            moreText.style.maxHeight = initialHeight + 'px'; // Set initial max height
        } else {
            moreLessButton.style.display = 'none'; // Hide button if content is not overflowing
        }

        // Toggle between two lines and full height and adjust max-height accordingly
        moreLessButton.addEventListener('click', function () {
            // Get the computed style for the moreText element
            var computedStyle = window.getComputedStyle(moreText);

            if (computedStyle.maxHeight === 'none' || computedStyle.maxHeight === initialHeight + 'px') {
                moreText.style.maxHeight = moreText.scrollHeight + 'px'; // Expand to full height
                moreLessButton.textContent = 'Read less';
            } else {
                moreText.style.maxHeight = initialHeight + 'px'; // Set back to initial height
                moreLessButton.textContent = 'Read more';
            }
        });
    });
}
function getFilesHtml(rowJson) {
    var returnHtml = "";
    var filesArray = rowJson["axpfile_file"].split(',');
    $.each(filesArray, function (index, value) {
        var fileType = getFileType(value);
        var iconClass = getIconClass(fileType);
        var attachmentURL = rowJson["axpfilepath_file"] + value;
        attachmentURL = attachmentURL.replace(/\\/g, "\\\\");
        if (value != "") {
            returnHtml += `<div class="Files-Attached ">
						   ${iconClass}						   
						   <div class="ms-1 fw-semibold">						  
							  <a class="attached-filename" onclick="downloadFileFromPath('${attachmentURL}','${value}')">${value}</a>                    
							  <div class="attached-filedetails">
								 <span class="doctype">${fileType}</span>
							  </div>
						   </div>
						</div>`;
        }

    });
    return returnHtml;
}
function getIconClass(fileType) {
    switch (fileType) {
        case 'pdf':
            return '<img src="../images/pdf.svg" class="file-img" />';
        case 'jpeg':
            return '<img src="../images/images.svg" class="file-img" />';
        case 'jpg':
            return '<img src="../images/images.svg" class="file-img" />';
        case 'png':
            return '<img src="../images/images.svg" class="file-img" />';
        case 'doc':
        case 'docx':
            return '<img src="../images/word.svg" class="file-img" />';
        case 'txt':
            return '<i style="margin:0px !important; font-size: 40px !important; " class="material-icons">description</i>';
        case 'xls':
        case 'xlsx':
            return '<img src="../images/xl.svg" class="file-img"/>';
        case 'ppt':
        case 'pptx':
            return '<img src="../images/ppt.svg" class="file-img"/>';
        case 'zip':
            return '<i style="margin:0px !important; font-size: 40px !important; " class="material-icons">archive</i>';
        case 'mp3':
            return '<i style="margin:0px !important; font-size: 40px !important; " class="material-icons">audiotrack</i>';
        case 'mp4':
            return '<i style="margin:0px !important; font-size: 40px !important; " class="material-icons">videocam</i>';
        default:
            return '<i style="margin:0px !important; font-size: 40px !important; " class="material-icons">insert_drive_file</i>'; // Default icon for unknown file types
    }
}
function getFileType(fileName) {
    // Split the file name into an array based on the dot separator
    var parts = fileName.split('.');

    // Get the last part of the array, which is the file extension
    var fileExtension = parts[parts.length - 1];

    // Convert the file extension to lowercase (optional, for consistency)
    return fileExtension.toLowerCase();
}
function scrollToTopOfDiv(divElement) {
    divElement.scrollTop = 0; // Set scrollTop to 0 to scroll to the top
}
/* End Function Set to generate html */
$(document).ready(function () {
    _entityForm = new EntityForm();
    _entityCommon = new EntityCommon();

    _entityForm.init();
});

function showPopup(element) {
    var text = element.getAttribute('data-text');
    document.getElementById('popupText').textContent = text;
    $('#myModal').modal('show');
}



function openFieldSelection() {
    $('#fieldsModal').modal('show');
    if ($('#fields-selection').html() == "")
        createOtherDataFieldsLayout();
}

function openDisplayFieldSelection() {
    $('#displayFieldsModal').modal('show');
    if ($('#displayfields-selection').html() == "")
        createDisplayFieldsLayout();
}

function createDisplayFieldsLayout() {
    const fieldsContainer = document.getElementById("displayfields-selection");

    const dcs = new Set();
    relatedDataFields.split(',').forEach(pair => {
        const key = pair.split('~')[0];
        dcs.add(key);
    });

    const dcArr = Array.from(dcs);

    var fields = _entityForm.metaData.filter(item => item.ftransid === _entityForm.entityTransId && item.hide === "F" && dcArr.includes(item.dcname));

    const groupedFields = {};
    fields.forEach(field => {
        if (!groupedFields[field.dcname]) {
            groupedFields[field.dcname] = [];
        }
        groupedFields[field.dcname].push(field);
    });


    var html = '';
    Object.entries(groupedFields).forEach(([dc, dcFields]) => {
        let collapsed = true;
        if (dc == "dc1")
            collapsed = false;

        let dcRow = _entityForm.entityFormJson.metadata.find(item => item.fname == dc);
        let dcName = dcRow.caption || dc;
        html += `
        <div class="card KC_Items">
            <div class="card-header collapsible cursor-pointer rotate  ${collapsed ? "collapsed" : ""}" data-bs-toggle="collapse" aria-expanded="true" data-bs-target="#fields-${dc}">
                <h3 class="card-title">${dcName} (${dc})</h3>
                <div class="card-toolbar rotate-180">
                    <span class="material-icons material-icons-style material-icons-2">
                        expand_circle_down
                    </span>
                </div>
            </div>
            <div class="KC_Items_Content collapse ${collapsed ? "" : "show"} heightControl pt-0---" id="fields-${dc}">
            <table class="table table-hover">
                <tbody id="fields-table-body">`;

        dcFields.forEach(fld => {
            html += `<tr><td><input type="checkbox" id="chkdisp_${fld.fldname}" class="chk-fields chk-displayflds" value="${fld.fldname}" data-fldcap="${fld.fldcap || ''}" data-dcname="${dc}" data-griddc="${dcRow.customdatatype || "T"}"></td>
          <td><label for="chkdisp_${fld.fldname}">${fld.fldcap || ''} (${fld.fldname})</label></td></tr>`;

        })
        html += `</tbody></table></div></div>`;
    })

    fieldsContainer.innerHTML = html;

    if (relatedDataDisplayFields !== "") {
        relatedDataDisplayFields.split("^").forEach(dc => {
            dc.split("|").forEach(fld => {
                var fldName = (fld.indexOf("=") > -1) ? fld.split("~")[0].split('=')[1] : fld.split("~")[0];
                document.querySelector(`#chkdisp_${fldName}`).checked = true;
            });
        });
    }



    const checkKeyFld = document.querySelector(`#chkdisp_${_entityForm.keyField}`);
    if (checkKeyFld) {
        checkKeyFld.checked = true;
        checkKeyFld.disabled = true;
    }
}


function fieldsModelClose() {
    $('#fieldsModal').modal('hide');
}

function displayFieldsModelClose() {
    $('#displayFieldsModal').modal('hide');
}

function applyRelatedDataFields() {
    const selectedFields = document.querySelectorAll(".chk-relateddataflds:checked");

    // Check if no checkboxes are selected
    if (selectedFields.length === 0) {
        showAlertDialog("error", "Error: No fields are selected.");
        return; // Exit the function
    }

    let fields = "";
    let isDc1FldSelected = false;
    selectedFields.forEach((field) => {
        const fieldName = field.value;
        const dcName = field.dataset.dcname;
        if (!isDc1FldSelected)
            isDc1FldSelected = (field.dataset.dcname === "dc1");

        fields += `${dcName}~${fieldName},`;
    });

    if (!isDc1FldSelected) {
        showAlertDialog("error", "Error: No DC1 fields are selected. Please select atleast one DC1 field.");
        return;
    }

    if (fields.endsWith(","))
        fields = fields.substr(0, fields.length - 1);

    _entityForm.relatedDataFields = fields;
    _entityForm.setSelectedRelatedFields();
}

function resetRelatedDataFields() {
    _entityForm.relatedDataFields = "";
    _entityForm.setSelectedRelatedFields();
}

function applyDisplayFields() {
    const selectedFields = document.querySelectorAll(".chk-displayflds:checked");

    // Check if no checkboxes are selected
    if (selectedFields.length === 0) {
        showAlertDialog("error", "Error: No fields are selected.");
        return; // Exit the function
    }

    let fields = "";
    selectedFields.forEach((field) => {
        const fieldName = field.value;
        const dcName = field.dataset.dcname;
        fields += `${dcName}~${fieldName},`;
    });

    if (fields.endsWith(","))
        fields = fields.substr(0, fields.length - 1);

    let fldsStr = "";
    let gridFldsStr = "";
    let groupedSelectedFlds = {}
    selectedFields.forEach((field) => {
        let dcName = `${field.dataset.dcname}~${field.dataset.griddc}`;
        if (!groupedSelectedFlds[dcName])
            groupedSelectedFlds[dcName] = [];

        const fieldName = field.value;
        groupedSelectedFlds[dcName].push(fieldName);
    });

    Object.entries(groupedSelectedFlds).forEach(([dcName, fields]) => {
        let isGridDc = dcName.split("~")[1] == "T";
        const tempFldData = _entityForm.metaData.find(item => item.fldname === fields[0] && item.ftransid == _entityForm.entityTransId);
        if (!isGridDc) {
            fldsStr += `${tempFldData.tablename}=`;

            fields.forEach(fld => {
                const fieldData = _entityForm.metaData.find(item => item.fldname === fld && item.ftransid == _entityForm.entityTransId);
                fldsStr += `${fieldData.fldname}~${fieldData.normalized}~${fieldData.srctable || ""}~${fieldData.srcfield || ""}~${fieldData.allowempty}|`;
            });
            if (fldsStr.endsWith("|"))
                fldsStr = fldsStr.substr(0, fldsStr.length - 1);

            fldsStr += `^`;
        }
        else {
            gridFldsStr += `${tempFldData.tablename}=`;

            fields.forEach(fld => {
                const fieldData = _entityForm.metaData.find(item => item.fldname === fld);
                gridFldsStr += `${fieldData.fldname}~${fieldData.normalized}~${fieldData.srctable || ""}~${fieldData.srcfield || ""}~${fieldData.allowempty}|`;
            });
            if (gridFldsStr.endsWith("|"))
                gridFldsStr = gridFldsStr.substr(0, gridFldsStr.length - 1);

            gridFldsStr += `^`;
        }
    });

    if (fldsStr.endsWith("^"))
        fldsStr = fldsStr.substr(0, fldsStr.length - 1);

    if (gridFldsStr.endsWith("^"))
        gridFldsStr = gridFldsStr.substr(0, gridFldsStr.length - 1);


    _entityForm.relatedDataDisplayFields = fldsStr;
    _entityForm.relatedDataDisplayGridFields = gridFldsStr;
    _entityForm.setSelectedDisplayFields();
}

function resetDisplayFields() {
    _entityForm.relatedDataDisplayFields = "All";
    _entityForm.relatedDataDisplayGridFields = "";
    _entityForm.setSelectedDisplayFields();
}

function searchFields() {
    let _searchInput = document.getElementById('searchBar').value.toLowerCase();
    let _tables = document.querySelectorAll('[id^="fields-dc"]');

    _tables.forEach(table => {
        let _fields = table.getElementsByTagName('tr');
        for (let i = 0; i < _fields.length; i++) {
            let _label = _fields[i].getElementsByTagName('label')[0];
            if (_label.innerText.toLowerCase().includes(_searchInput)) {
                _fields[i].style.display = '';
            } else {
                _fields[i].style.display = 'none';
            }
        }
    });
}


function formmatingtoillions(amount) {
    let isMillions = ((Object.values(window.top.allGloblVars.globalVars)
        .map((gv) => gv)
        .filter((gv) => gv["millions"])).length !== 0 &&
        (Object.values(window.top.allGloblVars.globalVars)
        .map((gv) => gv)
        .filter((gv) => gv["millions"])[0].millions === "T")) || false;

    if (isMillions) {
        return InsertCommas(amount); 
    } else {
        return formatIndianNumber(amount); 
    }
}

function InsertCommas(amount) {
    var i = parseFloat(amount);
    if (isNaN(i)) { i = 0.00; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    i = parseInt((i + .005) * 100);
    i = i / 100;
    s = new String(i);
    if (s.indexOf('.') < 0) { s += '.00'; }
    if (s.indexOf('.') === (s.length - 2)) { s += '0'; }
    s = minus + s;
    var formattedamt = CommaFormatted(s);
    return formattedamt;
}

function CommaFormatted(amount) {
    var delimiter = ","; 
    var a = "";
    if (amount.toString().indexOf(".") === -1)
        amount = amount + ".";

    a = amount.split('.');
    var d = "";
    if (a.length > 1)
        d = a[1].toString();
    var i = parseInt(a[0]);
    if (isNaN(i)) { return ''; }
    var minus = '';
    if (amount < 0) { minus = '-'; }
    i = Math.abs(i);
    var n = new String(i);
    var a = [];
    try {
        if (Parameters.length > 0 && Parameters.indexOf("millions~T") > -1) {
            while (n.length > 3) {
                var nn = n.substr(n.length - 3);
                a.unshift(nn);
                n = n.substr(0, n.length - 3);
            }
        } else {
            if (n.length > 3) {
                var nn = n.substr(n.length - 3);
                a.unshift(nn);
                n = n.substr(0, n.length - 3);
            }
            while (n.length > 2) {
                var nn = n.substr(n.length - 2);
                a.unshift(nn);
                n = n.substr(0, n.length - 2);
            }
        }
    } catch (ex) {
        while (n.length > 3) {
            var nn = n.substr(n.length - 3);
            a.unshift(nn);
            n = n.substr(0, n.length - 3);
        }
    }
    if (n.length > 0) { a.unshift(n); }
    n = a.join(delimiter);
    if (d.length < 1) { amount = n; }
    else { amount = n + '.' + d; }
    amount = minus + amount;
    return amount;
}

function formatIndianNumber(amount) {
    if (isNaN(amount)) return '';

    let num = Math.abs(parseInt(amount, 10)).toString();
    let result = '';

    if (num.length > 3) {
        result = ',' + num.slice(-3);
        num = num.slice(0, -3);
    }

    while (num.length > 2) {
        result = ',' + num.slice(-2) + result;
        num = num.slice(0, -2);
    }

    if (num.length > 0) {
        result = num + result;
    }

    return (amount < 0 ? '-' : '') + result;
}


function formatDateString(dateString) {
    if (!dateString) return '';

    const dateTimeParts = dateString.split('T');
    const dateParts = dateTimeParts[0].split('-');
    const timeParts = (dateTimeParts[1] || "00:00:00").split(':');

    const year = dateParts[0];
    const month = dateParts[1];
    const day = dateParts[2];

    const hours = timeParts[0];
    const minutes = timeParts[1];
    const seconds = timeParts[2];

    const formattedDate = dtCulture === "en-us"
        ? `${month}/${day}/${year}`
        : `${day}/${month}/${year}`;

    const timePart = (hours !== "00" || minutes !== "00" || seconds !== "00")
        ? `${hours}:${minutes}:${seconds}`
        : '';

    return timePart ? `${formattedDate} ${timePart}` : formattedDate;
}

function ShowHideDimmer(show) {
    try {
        if (typeof parent.ShowDimmer != "undefined") parent.ShowDimmer(show);
        if (typeof ShowDimmer != "undefined") ShowDimmer(show);
        if (typeof window.top.ShowDimmer != "undefined") window.top.ShowDimmer(show);
    }
    catch (ex) { }
}

//++ Start - FormControl changes

function getFieldId(fieldName, dcNo, isGridDc, rowNo) {
    if (isGridDc)
        return `${fieldName}${GetRowNoHelper(rowNo)}F${dcNo}`;
    else
        return `${fieldName}000F${dcNo}`;
}

function DoScriptFormControl(componentName, eventType) {    
    if (SFormControls.length > 0) {
        if (eventType == "On Form Load" || eventType == "On Data Load") {
            var rid = $j("#recordid000F0").val();
            $.each(SFCApply, function (idx, elem) {
                if (elem == "On Form Load" && rid == "0") {
                    isScriptFormLoad = "true";
                    var sfcExp = SFormControls[idx];
                    if (typeof SFCIsActive != "undefined" && SFCIsActive[idx] == "true")
                        EvaluateScriptFormControl(sfcExp);
                } else if (elem == "On Data Load" && rid != "0") {
                    isScriptFormLoad = "true";
                    var sfcExp = SFormControls[idx];
                    if (typeof SFCIsActive != "undefined" && SFCIsActive[idx] == "true")
                        EvaluateScriptFormControl(sfcExp);
                }
            });
        } else if (componentName != "") {
            var sfcfName = GetFieldsName(componentName);
            $.each(SFCFldNames, function (idx, elem) {
                if (SFCApply[idx] == "On Click" && eventType == "On Field Enter")
                    eventType = "On Click";
                if (elem == sfcfName && SFCApply[idx] == eventType) {
                    isScriptFormLoad = "false";
                    var sfcExp = SFormControls[idx];
                    if (typeof SFCIsActive != "undefined" && SFCIsActive[idx] == "true")
                        EvaluateScriptFormControl(sfcExp, componentName);
                }
            });
        }
    }
}

function EvaluateScriptFormControl(sfcExp, sfcfName = "") {
    if (sfcExp != "") {
        var flval = "";
        if (sfcfName != "")
            flval = GetFieldValue(sfcfName);
        //var strefVal= Evaluate(sfcfName, flval, sfcExp, "vexpr");
        var sfName = sfcfName == "" ? "" : sfcfName;
        var arrsfcExp = sfcExp.split("♥");
        AxFormControlList = new Array();
        
        var strefVal = EvalExprSet(arrsfcExp);
        if (AxFormControlList.length > 0)
            ProcessScriptFormControlOnList(sfName);
        //else if (strefVal != "" && strefVal.split("~").length >= 2)
        //    ProcessScriptFormControl(strefVal.split("~")[1], strefVal.split("~")[0], sfName);
        //else if (strefVal != "" && strefVal.split("♠").length > 0 && FormControlSameFormLoad == false)
        //    ProcessScriptFormControlEvents(strefVal, sfName);
    }
}

function ProcessScriptFormControlOnList(sfName) {
    $.each(AxFormControlList, function (i, val) {
        if (val != "")
            ProcessScriptFormControl(val.split("~")[1], val.split("~")[0], sfName);
    });
}

function ProcessScriptFormControl(listControls, actionStr, sfName) {
    var sfFldVal = "";
    var sfRno = 0;
    if (listControls == "")
        return;
    if (listControls.indexOf("♦") > 0) {
        sfRno = listControls.split("♦")[2];
        sfFldVal = listControls.split("♦")[1];
        listControls = listControls.split("♦")[0];
    }
    listControls.split(',').forEach(function (contName) {

        let setfldCap = "";
        if (contName.indexOf("^") > 0) {
            setfldCap = contName.split("^")[1];
            contName = contName.split("^")[0];
        }

        var fldname = GetExactFieldName(contName);
        var conFldDcNo = GetDcNo(fldname);
        contName = fldname;
        
        var destfld = $(`[data-name="${fldname}"]`);
        
        if (destfld.length > 0) {
            switch (actionStr) {
                case ("hide"):
                    if (/^dc\d+$/.test(fldname.toLowerCase())) {
                        ShowingDc(fldname, "hide");
                    } else {
                        HideShowField(fldname, "hide");
                    }
                    break;
                case ("show"):
                    if (/^dc\d+$/.test(fldname.toLowerCase())) {
                        ShowingDc(fldname, "show");
                    } else {
                        HideShowField(fldname, "show");
                    }
                    break;
                case ("setfieldcaption"):                    
                    $(`[data-name="${fldname}"] label`).text(setfldCap);
                    break;
                case ("setdccaption"):
                    $(`[data-name="${fldname}"] label.dccaption`).text(setfldCap);
                    break;
                case ("collapse"):                   
                    _entityForm.formControlActions["expandcollapse"][fldname] = "collapse";
                    setTimeout(function () { ExpandCollapseDc() }, 1);
                    break;
                case ("expand"):
                    _entityForm.formControlActions["expandcollapse"][fldname] = "expand";                    
                    setTimeout(function () { ExpandCollapseDc() }, 1);
                    break;
                default:
                    return;
            }
        }
    });
}

function HideShowField(fldName, action) {
    if (action == "hide") {
        $(`[data-name="${fldName}"]`).hide();
        $(`[data-name="${fldName}"]`).addClass('d-none');
    }
    else {
        $(`[data-name="${fldName}"]`).show();
        $(`[data-name="${fldName}"]`).removeClass('d-none');
    }
}

function ShowingDc(dcName, action) {
    _entityForm.formControlActions["expandcollapse"]
    if (action == "hide") {        
        $(`[data-name="${dcName}"]`).hide();
        $(`[data-name="${dcName}"]`).addClass('d-none');
    }
    else {
        $(`[data-name="${dcName}"]`).show();
        $(`[data-name="${dcName}"]`).removeClass('d-none');
    }
}

function ExpandCollapseDc() {
    Object.keys(_entityForm.formControlActions["expandcollapse"]).forEach(dcName => {
        var action = _entityForm.formControlActions["expandcollapse"][dcName];
        delete _entityForm.formControlActions["expandcollapse"][dcName];
        if (action == "expand") {
            $(`#${dcName}container`).collapse('show');
        }
        else {
            $(`#${dcName}container`).collapse('hide');
        }
    });
}
//-- End - FormControl changes
