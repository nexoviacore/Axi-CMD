var armToken = "", axProcessObj;

var LoadIframe = callParentNew("LoadIframe");
var cardsData = {}, cardsDesign = {}, xmlMenuData = "", menuJson = "";

let cardsDashboardObj = {
    dirLeft: true,
    enableMasonry: false,
    homePageType: "cards",
    isCardsDashboard: true,
    isMobile: isMobileDevice(),
};
var files = {
    css: [],
    js: []
};

const queryString = window.location.search;
//const urlParams = new URLSearchParams(queryString);
var taskcards = "";

class AxProcessFlow {
    constructor() {
        this.isAxpertFlutter = !this.isNullOrEmpty(armToken);
        this.cardParams = {};
        this.cardFlds = {};
        this.processName = '';
        this.keyField = '';
        this.keyValue = '';
        this.taskCompleted = false;
        this.taskId = '';
        this.userType = 'GUEST';
        this.stepHtml = `
            <div class="step">
                <div>
                    <div class="circle ">
                        <i class="fa fa-check"></i>
                        <span class="Emp-steps-counts">{{sno}}</span>
                    </div>
                    <div class="line"></div>
                </div>
                <div class="Task-process-wrapper">
                    {{groupNameHtml}}
                    {{taskCaptionHtml}}
                </div>
            </div>`;
        this.groupNameHtml = `
            <div class="title">
                <a href="#">{{taskgroupname}}</a>
                    <span data-groupname="{{taskgroupname}}" class="Process-flow accordion-icon rotate">
                    <span  class="material-icons material-icons-style material-icons-2">chevron_right</span>
                </span>
            </div>`
        this.taskCaptionHtml = `<div class="process-sub-flow" data-groupname="{{taskgroupname}}">
            <div class="Task-process-list status-{{taskstatus}}" onclick="axProcessObj.openTask(this, '{{taskname}}', '{{tasktype}}', '{{transid}}', '{{keyfield}}', '{{keyvalue}}', '{{recordid}}', '{{taskid}}');" data-taskid="{{taskid}}" data-tasktype="{{tasktype}}" data-transid="{{transid}}">
                <a href="#">{{taskname}}</a>
            </div>
        </div>`;


        this.horizontalStepHtml = `<li class="{{taskstatus}}">
                                <a href="javascript:void(0)" onclick="axProcessObj.openTask(this, '{{taskname}}', '{{tasktype}}', '{{transid}}', '{{keyfield}}', '{{keyvalue}}', '{{recordid}}', '{{taskid}}');" data-taskid="{{taskid}}" data-tasktype="{{tasktype}}" data-transid="{{transid}}" data-taskname="{{taskname}}" class="horizontal-steps {{taskstatus}}">
                                    <span class="circle">{{sno}}</span>
                                    <span class="label">{{taskname}}</span>
                                </a>
                            </li>`;

        this.dataSources = [];
        this.processFlowObj = {};
        this.processProgressObj = {};
        this.getUrlParams();
        this.horizontalFlow = true;

        this.processVars = {
            plistTbIcons: ["add_task", "receipt_long", "post_add", "library_books", "free_cancellation", "published_with_changes"],
            plistCols: {
                taskName: {
                    caption: "Task Name",
                    hidden: false,
                },
                //doneBy: {
                //    caption: "Done By",
                //    hidden: false,
                //},
                //doa: {
                //    caption: "Date of Activity",
                //    hidden: false,
                //},
                //status: {
                //    caption: "Status",
                //    hidden: false,
                //},
                //nextStep: {
                //    caption: "Next Step",
                //    hidden: true,
                //}
            },
            pStatus: {
                approve: {
                    badgeColor: "badge-light-success",
                    bgColor: "bg-light-success",
                    borderColor: "border-light-success",
                    color: "bg-success",
                    iconColor: "text-success",
                },
                approved: {
                    badgeColor: "badge-light-success",
                    bgColor: "bg-light-success",
                    borderColor: "border-light-success",
                    color: "bg-success",
                    iconColor: "text-success",
                },
                check: {
                    badgeColor: "badge-light-custom-checked",
                    bgColor: "bg-light-custom-checked",
                    borderColor: "border-light-custom-checked",
                    color: "bg-custom-checked",
                    iconColor: "text-custom-checked",
                },
                checked: {
                    badgeColor: "badge-light-custom-checked",
                    bgColor: "bg-light-custom-checked",
                    borderColor: "border-light-custom-checked",
                    color: "bg-custom-checked",
                    iconColor: "text-custom-checked",
                },
                made: {
                    badgeColor: "badge-light-primary",
                    bgColor: "bg-light-primary",
                    borderColor: "border-light-primary",
                    color: "bg-primary",

                },
                make: {
                    badgeColor: "badge-light-primary",
                    bgColor: "bg-light-primary",
                    borderColor: "border-light-primary",
                    color: "bg-primary",

                },
                rejected: {
                    badgeColor: "badge-light-danger",
                    bgColor: "bg-light-danger",
                    borderColor: "border-light-danger",
                    color: "bg-danger",

                },
                returned: {
                    badgeColor: "badge-light-warning",
                    bgColor: "bg-light-warning",
                    borderColor: "border-light-warning",
                    color: "bg-warning",

                },
            }
        };

        this.toolbarDrawerHTML = `
        <button id="kt_drawer_bulkApprove_button"
            class="btn btn-icon btn-sm btn-white btn-color-gray-600 btn-active-light-primary shadow-sm btn-custom-border-radius d-none" title="Bulk Approve" onclick="axTasksObj.openBulkApprove();">
            <span class="material-icons material-icons-style">checklist</span>
        </button>
        <button id="kt_drawer_proFlw_button"
            class="btn btn-icon btn-sm btn-white btn-color-gray-600 btn-active-light-primary shadow-sm btn-custom-border-radius" title="Refresh Page" onclick="axProcessObj.refreshPage();">
            <span class="material-icons material-icons-style">refresh</span>
        </button>
        
        <div>
            <button id="pd_timeline" class="btn btn-icon btn-sm btn-white btn-color-gray-600 btn-active-light-primary shadow-sm btn-custom-border-radius" data-kt-menu-trigger="click" data-kt-menu-placement="left" data-kt-menu-flip="top-end" data-kt-menu-attach="parent" title="Timeline">            
                <span class="material-icons material-icons-style">history_toggle_off</span>            
            </button>

            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light fw-bolder w-400px py-3"
                data-kt-menu="true" data-id="pd_timeline">
                <div id="TimeLine_overall" class="content">
                    <div class="card" id="Timeline-wrap">
                        <h1 class="Timeline-heading">
                            Timeline                            
                        </h1>
                        <ul class="Timel-sessions full-session d-none">
                        </ul>
                        <span id="nodata" class="p-5">No Timeline data available.</span>
                    </div>
                </div>
            </div>
        </div>
        <button id="pd_ann" class="btn btn-icon btn-sm btn-success btn-color-white shadow-sm d-none" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end" data-kt-menu-attach="parent" title="Make tasks">            
            <span class="material-icons material-icons-style">add</span>            
        </button>

        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light fw-bolder w-250px py-3"
            data-kt-menu="true" data-id="pd_ann">
            <annContent></annContent>
        </div>

        <button id="pd_moreOpt" class="btn btn-icon btn-sm d-none">
            <span class="material-icons material-icons-style">more_horiz</span>
        </button>    

        <div id="kt_drawer_proFlw" class="bg-white rounded-2 mt-15" data-kt-drawer="true" data-kt-drawer-activate="true"
            data-kt-drawer-toggle="#kt_drawer_proFlw_button" data-kt-drawer-close="#kt_drawer_proFlw_close"
            data-kt-drawer-name="docs" data-kt-drawer-overlay="true"
            data-kt-drawer-width="{default:'100%', 'sm': '100%', 'md': '75%', 'lg': '75%', 'xl': '75%'}">
            
        </div>`;

        this.annHTML = `<div class="menu-item px-3">
            <a class="menu-link px-3 border-bottom" href="javascript:void(0);" data-newtstruct>
                <span class="symbol symbol-30px symbol-circle me-5">
                    <span class="symbol-label bg-primary text-white fw-normal fs-3 material-icons"></annIcon></span>
                </span>
                <span class="fw-normal addCaption"></annCaption></span>
            </a>
        </div>`;

        this.allTskHTML = `<div class="custom-menu-item px-3">
            <span class="custom-menu-link px-3 border-bottom">
                <span class="form-check form-check-custom form-check-solid me-2">
                    <input class="form-check-input h-25px w-25px menu-task" type="checkbox" checked="checked"/>
                </span>
                <span class="symbol symbol-30px symbol-circle me-2">
                    <span class="symbol-label bg-light-primary text-primary fw-normal fs-3 material-icons border border-light-primary"></annIcon></span>
                </span>
                <span class="fw-normal menu-task-text"></annCaption></span>
            </span>
        </div>`;

        this.getProcessUserType();
    }

    getProcessUserType() {
        let _this = this, data = {}, url = "";
        url = "../../aspx/AxPEG.aspx/AxGetProcessUserType";
        data = { processName: this.processName };

        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                dataResult.every((rowData) => {
                    if (rowData.tasktype?.toUpperCase() == "APPROVE") {
                        this.userType = "APPROVER";                        
                        return false;
                    }

                    if (rowData.tasktype?.toUpperCase() == "MAKE") {
                        this.userType = "MAKER";
                        return false;
                    }

                    return true;
                })
            }
        });
    }

    getNextTaskInProcess() {
        let _this = this, data = {}, url = "";
        url = "../../aspx/AxPEG.aspx/AxGetNextTaskInProcess";
        data = { processName: this.processName, keyValue: this.keyValue };
        var nextTask = false;
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                if (dataResult.length > 0) {
                    let rowData = dataResult[0];
                    if ('URLSearchParams' in window) {
                        nextTask = true;
                        var searchParams = new URLSearchParams(window.location.search);
                        searchParams.set("keyfield", rowData.keyfield);
                        searchParams.set("keyvalue", rowData.keyvalue);
                        searchParams.set("taskid", rowData.taskid);
                        window.location.search = searchParams.toString();
                    }                    
                }
                else {
                    //Load homepage
                    callParentNew('LoadIframe(loadhomepage)', 'function');
                    nextTask = true;
                }
            }            
        });
        return nextTask;
    }


    showProcessTree() {
        if (this.isNullOrEmpty(axProcessTreeObj) || this.isUndefined(axProcessTreeObj)) {
            axProcessTreeObj = new AxProcessTree(this.processName);
        }
        axProcessTreeObj.showProcessTree();
    }

    reloadProcess(recordId) {
        this.taskCompleted = true;
        ShowDimmer(true);
        let _this = this;
        let url = "../../aspx/AxPEG.aspx/AxGetKeyValue";
        let data = { processName: this.processName, recordId: recordId };
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.refreshProcess(dataResult[0].keyvalue);
            }
        });
    };

    refreshPage() {
        window.location.href = window.location.href;
    }

    refreshProcess(keyValue) {
        ShowDimmer(true);
        const params = new URLSearchParams(location.search);
        params.set('keyvalue', keyValue);
        window.history.replaceState({}, '', `${location.pathname}?${params.toString()}`);
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.setAttribute("src", "");
        $rightIframe.classList.remove("d-none");

        //window.location.href = window.location.href;
        //this.dataSources = [];
        //this.processFlowObj = {};
        this.keyValue = keyValue;
        //this.init();
        this.showProgress();
        this.fetchProcessList("ProcessList", "TaskCompletion")
        //this.showProcessFlow();
        //this.fetchProcessKeyValues("ProcessKeyValues");
    };

    hideDefaultCenterPanel() {
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
    }

    fetchProcessList(name, from) {
        let _this = this, data = {}, url = "";

        if (name == "ProcessList") {
            url = "../../aspx/AxPEG.aspx/AxGetProcessList";
            if (_this.isAxpertFlutter) {
                url = "../../ARM_APIs/api/v1/ARMGetProcessList";
            }
        }

        data = { processName: this.processName, keyField: this.keyField, keyValue: this.keyValue || "NA" };

        this.callAPI(url, data, true, result => {
            if (result.success) {

                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.dataSources[name] = dataResult;
                _this.showProcessList(from);
                if (from != "TaskCompletion")
                    _this.processNewNode();
            }
        });
    };

    fetchProcessKeyValues(name) {
        let _this = this;
        let url = "../../aspx/AxPEG.aspx/AxGetProcessKeyValues";
        let data = { processName: this.processName };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                let dataArray = [];
                dataResult.forEach((item) => {
                    dataArray.push({ id: item.keyvalue, text: item.keyvalue })
                })
                $("select#keyvalues-select").select2({
                    placeHolder: "Search...",
                    data: dataArray
                }).on('select2:select', function (e) {
                    let keyValue = e.params.data.text;
                    _this.refreshProcess(keyValue);
                }).on('select2:open', () => {
                    $(this).find('.select2-search__field').focus();
                }).on('select2:close', () => {
                    $('.searchBoxChildContainer.search').addClass('d-none');
                });
            }
        });
    };

    callAPI(url, data, async, callBack) {
        let _this = this;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, async);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        if (_this.isAxpertFlutter) {
            xhr.setRequestHeader('Authorization', `Bearer ${armToken}`);
            data["armSessionId"] = armSessionId;
        }
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

    catchError(error) {
        showAlertDialog("error", error);
    };

    showSuccess(message) {
        showAlertDialog("success", message);
    };

    dataConvert(data, type) {
        if (type == "AXPERT") {
            try {
                data = JSON.parse(data.d);
                if (typeof data.result[0].result.row != "undefined") {
                    return data.result[0].result.row;
                }

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
                if (!this.isAxpertFlutter)
                    data = JSON.parse(data.d);
                if (data.result && data.result.success) {
                    if (!this.isUndefined(data.result.data)) {
                        return data.result.data;
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
    };

    generateFldId() {
        return `fid${Date.now()}${Math.floor(Math.random() * 90000) + 10000}`;
    };

    isEmpty(elem) {
        return elem == "";
    };

    isNull(elem) {
        return elem == null || elem == 'null';
    };

    isNullOrEmpty(elem) {
        return elem == null || elem == 'null' || elem == "";
    };

    isUndefined(elem) {
        return typeof elem == "undefined";
    };

    inValid(elem) {
        return elem == null || typeof elem == "undefined" || elem == "";
    };

    showProcessFlow() {
        if (this.horizontalFlow == true) {
            this.showHorizontalProcessFlow();
            return;
        }

        this.processProgressObj = {};
        this.dataSources["Process"].forEach((rowData, idx) => {
            let tempTaskType = rowData.tasktype.toUpperCase();
            if (["IF", "ELSE", "ELSE IF", "END"].indexOf(tempTaskType) == -1) {
                if (this.isUndefined(this.processProgressObj[rowData.taskgroupname])) {
                    this.processProgressObj[rowData.taskgroupname] = {};
                    this.processProgressObj[rowData.taskgroupname].group_name_html = '';
                    this.processProgressObj[rowData.taskgroupname].task_caption_html = '';
                }

                if (this.isNullOrEmpty(rowData.taskstatus) && rowData.indexno > 1) {
                    rowData.taskstatus = "disabled";
                }

                if (this.isNullOrEmpty(rowData.recordid)) {
                    rowData.recordid = "0";
                }

                let taskGroup = this.processProgressObj[rowData.taskgroupname];
                taskGroup.indexno = rowData.indexno;
                taskGroup.group_name_html = Handlebars.compile(this.groupNameHtml)(rowData);
                taskGroup.task_caption_html += Handlebars.compile(this.taskCaptionHtml)(rowData);
            }
        });

        document.querySelector('#procflow-steps').innerHTML = "";
        //if (this.isNullOrEmpty(this.keyValue))
        //    document.querySelector('#process-ref').innerText = '';
        //else
        //    document.querySelector('#process-ref').innerText = `Identifier : ${this.keyValue}`.toUpperCase();

        let sno = 1;
        for (let [key, value] of Object.entries(this.processProgressObj)) {
            document.querySelector('#procflow-steps').insertAdjacentHTML("beforeend", ` ${this.stepHtml.replace("{{sno}}", sno).replace("{{groupNameHtml}}", value.group_name_html).replace("{{taskCaptionHtml}}", value.task_caption_html)} `);
            sno++;
        }
        ShowDimmer(false);

        //let activeTask = document.querySelector('.status-active');
        //if (!this.isUndefined(this.taskId) && !this.isNullOrEmpty(this.taskId)) {
        //    ShowDimmer(true);
        //    document.querySelector(`.Task-process-list[data-taskid="${this.taskId}"]`).click();
        //}
        //else if (!this.isUndefined(this.transId) && !this.isNullOrEmpty(this.transId)) {
        //    ShowDimmer(true);
        //    document.querySelector(`.Task-process-list[data-transid="${this.transid}"][data-tasktype="Make"]`).click();
        //}
        //else if (this.isNull(activeTask)) {
        //    ShowDimmer(true);
        //    document.querySelector('.Task-process-list').click();
        //}
        //else {
        //    ShowDimmer(true);
        //    activeTask.click();
        //}

        $(".accordion-icon").click(function () {
            let groupname = $(this).attr('data-groupname');
            $(this).toggleClass('rotate');
            $(`.process-sub-flow[data-groupname="${groupname}"]`).toggle();
        });

        //this.loadDefaultProcessPage();
    };

    showHorizontalProcessFlow() {
        let sno = 1;
        document.querySelector('#horizontal-processbar').innerHTML = "";
        this.dataSources["Process"].forEach((rowData, idx) => {
            let tempTaskType = rowData.tasktype.toUpperCase();
            if (["IF", "ELSE", "ELSE IF", "END"].indexOf(tempTaskType) == -1) {
                if (this.isNullOrEmpty(rowData.taskstatus) && rowData.indexno > 1) {
                    rowData.taskstatus = "disabled";
                }
                else if (["APPROVED", "REJECTED", "RETURNED", "CHECKED", "MADE"].indexOf(rowData.taskstatus?.toUpperCase())) {
                    rowData.taskstatus = "completed";
                }

                if (this.taskId == rowData.taskid) {
                    rowData.taskstatus = "active";
                }

                if (this.isNullOrEmpty(rowData.recordid)) {
                    rowData.recordid = "0";
                }

                rowData.sno = sno;
                sno++;

                document.querySelector('#horizontal-processbar').insertAdjacentHTML("beforeend", ` ${Handlebars.compile(this.horizontalStepHtml)(rowData)} `);

            }
        });



        //for (let [key, value] of Object.entries(this.processProgressObj)) {
        //    document.querySelector('#procflow-steps').insertAdjacentHTML("beforeend", ` ${this.horizontalStepHtml} `);

        //}
        ShowDimmer(false);
    };

    loadDefaultProcessPage() {
        document.querySelector("#rightIframe").setAttribute("src", `../../aspx/htmlPages.aspx?loadcaption=Process landing page`);
    }

    openTask(elem, taskName, taskType, transId, keyField, keyValue, recordId, taskId, taskdefid) {
        //var tasks = document.querySelectorAll(".Task-process-list.Active");

        //[].forEach.call(tasks, function (el) {
        //    el.classList.remove("Active");
        //});

        //elem.classList.add("Active");

        //var steps = document.querySelectorAll(".step.step-active");

        //[].forEach.call(steps, function (el) {
        //    el.classList.remove("step-active");
        //});

        //elem.closest(".step").classList.add("step-active");
        this.keyField = keyField;
        this.keyValue = keyValue || this.keyValue;

        switch (taskType.toUpperCase()) {
            case "MAKE":
                this.openTstruct(transId, keyField, keyValue, recordId);
                break;
            case "CHECK":
                this.openProcessTask(taskName, taskType, taskId);
                break;
            case "APPROVE":
                this.openProcessTask(taskName, taskType, taskId);
                break;
        }

        this.taskId = taskId;
        this.showProgress(elem);
        this.showTimeLine();
        this.openRightSideCards(taskName, keyValue);
    };

    setActiveInList(elemTaskId) {
        document.querySelectorAll(`[data-taskid="${elemTaskId}"]`).forEach((elem) => {
            if (elem.classList.contains("Procurement-list")) {
                document.querySelector(".Procurement-list-wrap.active")?.classList.remove("active");
                elem.closest(`.Procurement-list-wrap`)?.classList.add("active");
                elem.scrollIntoView();
            }

            if (elem.classList.contains("horizontal-steps")) {
                document.querySelectorAll(".horizontal-steps").forEach((step) => {
                    step.closest("li")?.classList.remove("Active");
                });

                elem.closest(`li`)?.classList.add("Active");
                elem.scrollIntoView();
            }
        });
    }


    openRightSideCards(taskname, keyvalue) {
        ShowDimmer(true);
        //let processParams = `&process=${this.processName}&taskname=${taskname}&keyvalue=${keyvalue}`;
        //document.querySelector("#rideSideCards").setAttribute("src", `../../aspx/htmlPages.aspx?loadcaption=Custom Dashboard${processParams}`);

        /* Variables from mainpage */


        files.js.push("/../ThirdParty/lodash.min.js");
        files.js.push("/../ThirdParty/deepdash.min.js");

        files.js.push("/../Js/handlebars.min.js?v=1");
        files.js.push("/../Js/handleBarsHelpers.min.js");

        files.js.push("/../ThirdParty/Highcharts/highcharts-3d.js");
        files.js.push("/../ThirdParty/Highcharts/highcharts-more.js");
        files.js.push("/../ThirdParty/Highcharts/highcharts.js");
        files.js.push("/../ThirdParty/Highcharts/highcharts-exporting.js");
        files.js.push("/../Js/high-charts-functions.min.js?v=20");
        files.js.push("/../Js/AxInterface.min.js?v=10");

        files.js.push("/../ThirdParty/DataTables-1.10.13/media/js/jquery.dataTables.js");
        files.js.push("/../ThirdParty/DataTables-1.10.13/media/js/dataTables.bootstrap.js");

        files.js.push("/../ThirdParty/DataTables-1.10.13/extensions/Extras/moment.min.js");
        files.css.push("/../ThirdParty/fullcalendar/lib/main.min.css");
        files.js.push("/../ThirdParty/fullcalendar/lib/main.min.js");

        if (cardsDashboardObj.isMobile) {
            files.js.push("/../ThirdParty/jquery-ui-touch-punch-master/jquery.ui.touch-punch.min.js");
        }

        if (cardsDashboardObj.enableMasonry) {
            files.js.push("/../ThirdParty/masonry/masonry.pkgd.min.js");
        }

        files.js.push(`/HTMLPages/js/axpertFlutterCustomDashboard.js?v=2`);

        if (document.getElementsByTagName("body")[0].classList.contains("btextDir-rtl")) {
            cardsDashboardObj.dirLeft = false;
        }



        loadAndCall({
            files: files,
            callBack: () => {

                $(function () {
                    deepdash(_);

                    $.ajax({
                        url: "../../aspx/AxPEG.aspx/AxGetCardsData",
                        type: 'POST',
                        cache: false,
                        async: true,
                        data: JSON.stringify({
                            processName: axProcessObj.processName || '', taskName: taskname || '', keyValue: keyvalue || ''
                        }),
                        dataType: 'json',
                        contentType: "application/json",
                        success: (data) => {
                            if (data.d && data.d != "") {
                                let result = JSON.parse(data.d);
                                if (result?.result?.success) {
                                    document.querySelector("#PROFLOW_Right_Last").classList.remove("d-none");
                                }
                                else {
                                    document.querySelector("#PROFLOW_Right_Last").classList.add("d-none");
                                    return;
                                }
                                cardsData.value = JSON.stringify(result.result.cards);
                                cardsDesign.value = "";
                                //xmlMenuData = result.menu;
                                //taskcards = JSON.parse(result.taskcards).result[0].result.row;
                                //if (taskcards != '') {
                                //    taskcards = taskcards.map(item => item.cardname);
                                //}
                            } else {
                                showAlertDialog("error", "Error while loading cards dashboard..!!");
                                return;
                            }

                            if (xmlMenuData != "") {
                                xmlMenuData = xmlMenuData.replace(/&apos;/g, "'");
                                var xml = parseXml(xmlMenuData)
                                var xmltojson = xml2json(xml, "");
                                menuJson = JSON.parse(xmltojson);
                            }
                            appGlobalVarsObject._CONSTANTS.menuConfiguration = $.extend(true, {},
                                appGlobalVarsObject._CONSTANTS.menuConfiguration, {
                                menuJson: menuJson,
                            });
                            // try {
                            appGlobalVarsObject._CONSTANTS.cardsPage = $.extend(true, {},
                                appGlobalVarsObject._CONSTANTS.cardsPage, {
                                setCards: true,
                                cards: (JSON.parse(cardsData.value !== "" ? ReverseCheckSpecialChars(cardsData.value) : "[]",
                                    function (k, v) {
                                        try {
                                            return (typeof v === "object" || isNaN(v) || v.toString().trim() === "") ? v : (typeof v == "string" && (v.startsWith("0") || v.startsWith("-")) ? parseFloat(v, 10) : JSON.parse(v));
                                        } catch (ex) {
                                            return v;
                                        }
                                        //0 & - starting with does not gets parsed in json.parse
                                        //json.parse is used because it porcess int, float and boolean together
                                    }
                                ) || []).map(arr => _.mapKeys(arr, (value, key) => key.toString().toLowerCase())),
                                design: (JSON.parse(cardsDesign.value !== "" ? cardsDesign.value : "[]",
                                    function (k, v) {
                                        try {
                                            return (typeof v === "object" || isNaN(v) || v.toString().trim() === "") ? v : (typeof v == "string" && (v.startsWith("0") || v.startsWith("-")) ? parseFloat(v, 10) : JSON.parse(v));
                                        } catch (ex) {
                                            return v;
                                        }
                                    }
                                ) || []).map(arr => _.mapKeys(arr, (value, key) => key.toString().toLowerCase())),
                                enableMasonry: cardsDashboardObj.enableMasonry,
                                staging: {
                                    iframes: ".splitter-wrapper",
                                    cardsFrame: {
                                        div: ".cardsPageWrapper",
                                        cardsDiv: ".cardsPlot",
                                        cardsDesigner: ".cardsDesigner",
                                        cardsDesignerToolbar: ".designer",
                                        editSaveButton: ".editSaveCardDesign",
                                        icon: "span.material-icons",
                                        divControl: "#arrangeCards"
                                    },
                                }
                            });

                            var lcm = appGlobalVarsObject.lcm;
                            var tempaxpertUIObj = $.axpertUI
                                .init({
                                    isHybrid: appGlobalVarsObject._CONSTANTS.isHybrid,
                                    isMobile: cardsDashboardObj.isMobile,
                                    compressedMode: appGlobalVarsObject._CONSTANTS.compressedMode,
                                    dirLeft: cardsDashboardObj.dirLeft,
                                    axpertUserSettings: {
                                        settings: appGlobalVarsObject._CONSTANTS.axpertUserSettings
                                    },
                                    cardsPage: appGlobalVarsObject._CONSTANTS.cardsPage
                                });

                            appGlobalVarsObject._CONSTANTS.cardsPage = tempaxpertUIObj.cardsPage;
                            ShowDimmer(false);
                        },
                        error: (error) => {
                            ShowDimmer(false);
                            showAlertDialog("error", "Error while loading cards dashboard..!!");
                            return;
                        },
                        failure: (error) => {
                            ShowDimmer(false);
                            showAlertDialog("error", "Error while loading cards dashboard..!!");
                            return;
                        },
                    });

                    //start cards dasboard Init



                    // } catch (ex) {
                    //     showAlertDialog("error", ex.Message);
                    // }
                });

                //axTimeLineObj = new ProcessTimeLine();
                //axTimeLineObj.keyvalue = urlParams.get('keyvalue');
                //axTimeLineObj.getTimeLineData();
            }
        });
        //End cards dashboard Code
    }

    openTstruct(transId, keyField, keyValue, recordId) {
        ShowDimmer(true);
        let isProcess = (this.userType == "APPROVER" ? "fromprocess=true&" : "");
        let url = `../../aspx/tstruct.aspx?${isProcess}transid=${transId}`;
        if (this.isNullOrEmpty(recordId))
            recordId = "0";
        if (recordId != "0")
            url += `&act=load&recordid=${recordId}`;
        else {
            if (keyValue != "" && keyValue != '{{keyvalue}}') {
                url += `&act=open&${keyField}=${keyValue}`
            }
            else {
                url += `&act=open&${this.keyField}=${this.keyValue}`
            }
        }
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.classList.remove("d-none");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
        ShowDimmer(false);
    };

    newTstruct(transId, taskName) {
        ShowDimmer(true);
        //callParentNew(`LoadIframe(htmlPages.aspx?loadcaption=AxProcessFlow&processname=${this.processName}&transid=${transId})`, "function");

        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
        let isProcess = (this.userType == "APPROVER" ? "fromprocess=true&" : "");
        let url = `../../aspx/tstruct.aspx?${isProcess}transid=${transId}&act=open`;
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.classList.remove("d-none");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
        this.keyValue = "NA";
        this.showProgress();
        axProcessObj.openRightSideCards(taskName, "NA");
        ShowDimmer(false);
    };

    getUrlParams() {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        this.keyField = urlParams.get('keyfield');
        this.keyValue = urlParams.get('keyvalue') || 'NA';
        this.processName = urlParams.get('processname') || _processName;
        this.taskId = urlParams.get('taskid') || '';
        this.transId = urlParams.get('transid') || '';
        this.target = urlParams.get('target') || '';

        this.add = urlParams.get('add') || 'T';
        this.leftPanel = urlParams.get('left') || 'T';
        this.rightPanel = urlParams.get('right') || 'T';
        this.showTree = urlParams.get('tree') || 'T';
        this.bulkApproval = urlParams.get('bulk') || 'T';

        document.querySelector('#process-name span').innerText = this.processName;
    };

    openProcessTask(taskName, taskType, taskid) {
        ShowDimmer(true);
        let url = `../../aspx/htmlPages.aspx?loadcaption=Active Lists&processname=${this.processName}&keyfield=${this.keyField}&keyvalue=${this.keyValue}&taskname=${taskName}&tasktype=${taskType}&taskid=${taskid}`;
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.classList.remove("d-none");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
        ShowDimmer(false);
    };

    openSearch() {
        if ($('.searchBoxChildContainer.search').hasClass('d-none')) {
            $('.searchBoxChildContainer.search').removeClass('d-none');
            $('select#keyvalues-select').select2('open');
        }
        else {
            $('.searchBoxChildContainer.search').addClass('d-none');
            $('select#keyvalues-select').select2('close');
        }
    };

    init() {
        try {
            ShowDimmer(true);
            var headerExtras = document.createElement("div");
            headerExtras.classList.add(..."d-flex gap-4".split(" "));
            document.getElementsByTagName("toolbarDrawer")[0].replaceWith(headerExtras);
            headerExtras.innerHTML = this.toolbarDrawerHTML;

            if (this.userType == "APPROVER")
                document.querySelector("#kt_drawer_bulkApprove_button")?.classList.remove("d-none");

            //KTDrawer.init();
            KTMenu.init();

            if (this.add == "F") {
                document.querySelector("#pd_ann").classList.add("d-none")
            }

            if (this.leftPanel == "F") {
                document.querySelector("#PROFLOW_Left").classList.add("d-none")
                document.querySelector("#PROFLOW_Right")?.classList.add("right-only")
            }

            if (this.rightPanel == "F") {
                document.querySelector("#PROFLOW_Right_Last")?.classList.add("d-none")
            }

            if (this.bulkApproval == "F") {
                document.querySelector("#kt_drawer_bulkApprove_button")?.classList.add("d-none")
            }

            if (this.showTree == "F") {
                document.querySelector("#proFlw_Tree_button")?.classList.add("d-none")
            }

            this.fetchProcessList("ProcessList");
            if (!this.inValid(this.target)) {
                ShowDimmer(true);
                this.showProgress();
            }

            //this.showTimeLine();
        } catch (error) {
            this.catchError(error.message);
        }
    };

    processNewNode(drawer = false) {
        let annContent = ``;
        if (axProcessObj.dataSources["ProcessList"].addnewnode && axProcessObj.dataSources["ProcessList"].addnewnode.length > 0) {
            annContent = Object.values(axProcessObj.dataSources["ProcessList"].addnewnode).map((annode) => {
                let ann = axProcessObj.annHTML;
                !this.isUndefined(annode.transid) ? ann = ann.replace("data-newtstruct", `onclick="axProcessObj.newTstruct('${annode.transid}','${annode.taskname}')"`) : "";
                ann = ann.replace("</annIcon>", (annode.taskicon));
                ann = ann.replace("</annCaption>", (annode.taskname || ""));
                return ann;
            }).join("");
        } else {
            let ann = axProcessObj.annHTML;
            ann = ann.replace("</annIcon>", "playlist_remove");
            ann = ann.replace("</annCaption>", "No data");
            annContent = ann;
        }

        if (drawer) {
            return annContent;
        }

        if (!this.isUndefined(document.getElementsByTagName("anncontent")[0])) {
            document.getElementsByTagName("anncontent")[0].outerHTML = annContent
        }
        // !this.isUndefined(document.getElementsByTagName("pd-annContent")[0]) && (document.getElementsByTagName("pd-annContent")[0].outerHTML = annContent);
    };

    showProcessList() {
        ShowDimmer(true);
        try {
            let allTasks = ``;

            let tblContentPending = document.getElementById("plistContentPending");
            let tblContentCompleted = document.getElementById("plistContentCompleted");

            if (axProcessObj.dataSources["ProcessList"].list && axProcessObj.dataSources["ProcessList"].list.length > 0) {

                document.querySelector("#PROFLOW_Left").classList.remove("d-none");
                document.querySelector("#PROFLOW_Right").classList.remove("right-only");

                let allTaskCaps = [...new Set(axProcessObj.dataSources["ProcessList"].list.map((plst) => plst.taskname))];
                let allTaskIcons = [...new Set(axProcessObj.dataSources["ProcessList"].list.map((plst) => plst.displayicon))].splice(0, allTaskCaps.length);
                allTasks = `<div class="custom-menu-item px-3">
                    <span class="custom-menu-link px-3 border-bottom">
                        <span class="form-check form-check-custom form-check-solid me-4">
                            <input class="form-check-input h-25px w-25px all-task" type="checkbox" checked="checked" />
                        </span>                        
                        <span class="fw-normal all-task-text">Unselect All</span>
                        <button class="btn btn-sm btn-light-primary ms-auto btn-custom-border-radius" onclick="axProcessObj.taskFilter('task')">Apply</button>
                    </span>
                </div>`;
                allTaskCaps.forEach((cap, indx) => {
                    let ann = axProcessObj.allTskHTML;
                    ann = ann.replace("</annCaption>", cap);
                    ann = ann.replace("</annIcon>", allTaskIcons[indx]);
                    allTasks += ann;
                });

                !this.isUndefined(document.getElementsByTagName("pd-allTasks")[0]) && (document.getElementsByTagName("pd-allTasks")[0].outerHTML = allTasks);

                let tblCompleted = document.createElement("table");
                tblCompleted.classList.add(..."table table-sm table-row-bordered table-responsive overflow-auto".split(" "));
                tblCompleted.setAttribute("id", "completedList");

                let tblPending = document.createElement("table");
                tblPending.classList.add(..."table table-sm table-row-bordered table-responsive overflow-auto".split(" "));
                tblPending.setAttribute("id", "pendingList");

                let plistThead = Object.keys(axProcessObj.processVars.plistCols).map(key => axProcessObj.processVars.plistCols[key]).filter(obj => obj.hidden == false);

                let tbodyCompleted = document.createElement("tbody");
                let tbodyPending = document.createElement("tbody");
                let pendingCount = 0;
                let completedCount = 0;

                Object.values(axProcessObj.dataSources["ProcessList"].list).map((plst) => {
                    let newTr = document.createElement("tr");
                    plistThead.map((col) => {
                        let trTd = document.createElement("td");
                        trTd.classList.add("align-middle");
                        trTd.classList.add("Procurement-list-wrap");
                        let tdData = axProcessObj.processListData(plst, col);
                        trTd.innerHTML = tdData;
                        trTd.setAttribute("data-fromuser", plst.fromuser);
                        trTd.setAttribute("data-tasktime", plst.tasktime);
                        newTr.appendChild(trTd);
                    });

                    if (plst.completionstatus == "pending") {
                        tbodyPending.appendChild(newTr);
                        pendingCount++;
                    }
                    else {
                        tbodyCompleted.appendChild(newTr);
                        completedCount++;
                    }

                }).join("");

                tblCompleted.appendChild(tbodyCompleted);
                tblPending.appendChild(tbodyPending);

                if (tblContentCompleted && completedCount) {
                    tblContentCompleted.innerHTML = "";
                    tblContentCompleted.appendChild(tblCompleted);
                }
                else {
                    let plistNoDataAlert = `<label class="form-label fs-4">No data available...!!!</label>`;
                    tblContentCompleted.innerHTML = plistNoDataAlert;
                }

                if (tblContentPending && pendingCount) {
                    tblContentPending.innerHTML = "";
                    tblContentPending.appendChild(tblPending);
                }
                else {
                    let plistNoDataAlert = `<label class="form-label fs-4">No data available...!!!</label>`;
                    tblContentPending.innerHTML = plistNoDataAlert;
                }

                $("#pdFilFrom, #pdFilTo").flatpickr({
                    dateFormat: "d-m-Y", //"d-m-Y H:i:S",
                    enableTime: false,
                });

                KTMenu.init();

                $(document).on("click", ".all-task", (event) => {
                    let menuPr = document.querySelector("#pd_all_tasks");
                    let menuDd = KTMenu.getInstance(menuPr);
                    menuDd.element.children.forEach((ch, inx) => {
                        if (inx != 0) {
                            ch.getElementsByClassName("menu-task")[0].checked = event.currentTarget.checked;
                        }
                    });
                    event.currentTarget.checked ? $(".all-task-text").text("Unselect All") : $(".all-task-text").text("Select All");
                });


                $(document).on("click", ".menu-task", (event) => {
                    let totalCheckBoxes = document.querySelectorAll(".menu-task");
                    let checkedCheckBoxes = document.querySelectorAll(".menu-task:checked");
                    if (totalCheckBoxes.length > 0 && totalCheckBoxes.length == checkedCheckBoxes.length) {
                        document.querySelector(".all-task").checked = true;
                        $(".all-task-text").text("Unselect All")
                    }
                    else {
                        document.querySelector(".all-task").checked = false;
                        $(".all-task-text").text("Select All")
                    }
                });

            } else {
                document.querySelector("#PROFLOW_Left").classList.add("d-none");
                document.querySelector("#PROFLOW_Right").classList.add("right-only");
                //document.getElementsByClassName("card-header")[0].classList.add("d-none");
                //document.getElementsByClassName("card-footer")[0].classList.add("d-none");
                //let plistNoDataAlert = `<label class="form-label fs-4">No data available...!!!</label>`;
                //tblContentCompleted.innerHTML = plistNoDataAlert;
                //tblContentPending.innerHTML = plistNoDataAlert;
            }

            if (!this.isUndefined(this.taskId) && !this.isNullOrEmpty(this.taskId)) {
                let elem = document.querySelector(`.Procurement-list[data-taskid="${this.taskId}"]`);
                elem?.click();
                elem?.scrollIntoView();
            }

        } catch (error) {
            this.catchError(error.message);
        }

        document.querySelectorAll('.accordion-button').forEach((btn) => {
            btn.addEventListener("click", function (e) {
                let elem = this;
                let target = elem.getAttribute('data-target');
                elem.classList.toggle('collapsed');
                document.querySelector(target).classList.toggle('show');
            });
        });
        ShowDimmer(false);
    };

    processListData(plst, col) {
        try {
            let returnData = ``;
            switch (col.caption) {
                case "Task Name":
                    returnData = `${axProcessObj.getProStatusDetails(col, plst)} 
                    <a href="javascript:void(0);" class="text-dark cursor-pointer text-hover-primary fs-6 Procurement-list" onclick="axProcessObj.openTask(this, '${plst.taskname}','${plst.tasktype}','${plst.transid}','${plst.keyfield}','${plst.keyvalue}','${plst.recordid}','${plst.taskid}');" data-caption="${plst.taskname}" data-keyvalue="${plst.keyvalue}" data-taskid="${plst.taskid}">${plst.displaytitle}</a>`;
                    break;
                //case "Done By":
                //    returnData = `<div class="d-flex">
                //            <span class="material-icons material-icons-style me-2">admin_panel_settings</span>
                //            <span class="fs-6 my-auto user-name">${plst.taskfromuser}</span>
                //        </div>`;
                //    break;
                //case "Date of Activity":
                //    returnData = `<div class="d-flex">
                //            <span class="material-icons material-icons-style me-2">calendar_month</span>
                //            <span class="fs-6 my-auto date-time">${plst.tasktime || "NA"}</span>
                //        </div >`;
                //    break;
                //case "Status":
                //    returnData = plst.taskstatus != "" ? `<span class="badge ${this.processVars.pStatus[plst.taskstatus.toLowerCase()].badgeColor} rounded-1">${plst.taskstatus}</span>` : "";
                //    break;
                //case "Next Step":
                //    returnData = `${axProcessObj.getProStatusDetails(col, plst)}`;
                //    break;
                default: returnData = ``;
                    break;
            }
            return returnData;
        } catch (error) {
            this.catchError(error.message);
        }
    };

    getProStatusDetails(col, plst) {
        let _this = {
            counter: 0,
            returnProStatus: ``,
        };

        if (col.caption == "Task Name") {
            _this.counter = 1;
            _this.icon = plst.displayicon;
            _this.taskType = (this.processVars.pStatus[plst.tasktype.toLowerCase()] || "make");
            _this.title = "";
        } else if (col.caption == "Next Step") {
            _this.counter = plst.nexttask != "" ? plst.nexttask.split(",").length : 0;
            _this.nextTask = plst.nexttask.split(",");
        }

        let i = 0;
        while (i < _this.counter) {
            if (col.caption == "Next Step") {
                _this.curTask = _this.nextTask[i].split("~");
                _this.icon = _this.curTask[0];
                _this.taskType = (this.processVars.pStatus[_this.curTask[1].toLowerCase()] || "make");
                _this.title = _this.curTask[2];
            }
            _this.returnProStatus += `<button class="btn btn-icon btn-sm ${_this.taskType.bgColor} border ${_this.taskType.borderColor} shadow-sm flex-column me-2" ${_this.title != "" ? `title="${_this.title}"` : ""}>
                <span class="blinker bullet bullet-dot h-8px w-8px position-relative bottom-25 start-25 animation-blinkz ${_this.taskType.color}"></span>
                <span class="material-icons material-icons-style material-icons-3 mt-n2 ${_this.taskType.iconColor}">${_this.icon}</span>
            </button>`;

            i++;
        }

        return _this.returnProStatus;

    };

    taskFilter(type) {
        let tryFilter = {
            user: document.getElementById("pdFilUser").value || "",
            from: document.getElementById("pdFilFrom").value?.replaceAll("-", "/") || "",
            to: document.getElementById("pdFilTo").value?.replaceAll("-", "/") || "",
        };

        //tryFilter.tbl = document.getElementById("completedList");
        //tryFilter.tbdy = tryFilter.tbl.getElementsByTagName("tbody")[0];
        tryFilter.allTrs = document.querySelectorAll('#pendingList tr, #completedList tr');

        if (type == "reset") {
            Object.values(tryFilter.allTrs).filter((elm) => {
                elm.style.display = "";
            });

            document.getElementById("pdFilUser").value = "";
            document.getElementById("pdFilFrom").value = "";
            document.getElementById("pdFilTo").value = "";
        } else if (type == "filter" && (tryFilter && (tryFilter.user || tryFilter.from || tryFilter.to)) == "") {
            this.catchError("Filter parameters cannot be left empty..!!");
        } else {
            if (type == "task") {
                $(".menu-each-task").removeClass("btn-primary");
                document.getElementById("pd_all_tasks").classList.add("btn-primary");
                let menuPr = document.querySelector("#pd_all_tasks");
                let menuDd = KTMenu.getInstance(menuPr);
                let menuItems = [], selectedItems = [];
                let selectAll = menuDd.element.children[0].getElementsByClassName("all-task")[0];
                let selectAllText = menuDd.element.children[0].getElementsByClassName("all-task-text")[0];

                menuDd.element.children.forEach((ch, inx) => {
                    if (inx != 0) {
                        menuItems.push(ch);
                        if (ch.getElementsByClassName("menu-task")[0].checked) {
                            selectedItems.push(ch);
                        }
                    }
                });

                if (selectedItems.length == menuItems.length) {
                    selectAll.checked = true;
                    selectAllText.innerText = "Unselect All";
                } else {
                    selectAll.checked = false;
                    selectAllText.innerText = "Select All";
                }

                Object.values(tryFilter.allTrs).filter((elm) => {
                    elm.style.display = "none";
                });

                if (selectedItems.length > 0) {
                    Object.values(tryFilter.allTrs).filter((elm) => {
                        selectedItems.forEach(aa => {
                            tryFilter.cap = aa.getElementsByClassName("menu-task-text")[0].innerText;
                            if (tryFilter.cap != "") {
                                let td = elm.getElementsByTagName("td")[0];
                                if (td) {
                                    let txtValue = td.getElementsByTagName("a")[0].dataset['caption'];
                                    if (txtValue.trim().indexOf(tryFilter.cap) > -1) {
                                        elm.style.display = "";
                                    }
                                }
                            }
                        })
                    });
                } else {
                    this.catchError("Please select a task to show the process list..!!");
                }

                menuDd.hide(menuPr);

            } else if (type == "eachTask") {


                $(document).on("click", ".menu-each-task", (event) => {
                    $(".menu-each-task, #pd_all_tasks").removeClass("btn-primary");
                    event.currentTarget.classList.add("btn-primary");
                    tryFilter.cap = event.currentTarget.dataset['caption'];
                    Object.values(tryFilter.allTrs).filter((elm) => {
                        if (tryFilter.cap != "") {
                            let td = elm.getElementsByTagName("td")[0];
                            if (td) {
                                let txtValue = td.getElementsByTagName("a")[0].dataset['caption'];
                                if (txtValue.trim().indexOf(tryFilter.cap) > -1) {
                                    elm.style.display = "";
                                } else {
                                    elm.style.display = "none";
                                }
                            }
                        }
                    });

                    let menuPr = document.querySelector("#pd_all_tasks");
                    let menuDd = KTMenu.getInstance(menuPr);
                    let menuItems = [], selectedItems = [];
                    let selectAll = menuDd.element.children[0].getElementsByClassName("all-task")[0];
                    let selectAllText = menuDd.element.children[0].getElementsByClassName("all-task-text")[0];

                    menuDd.element.children.forEach((ch, inx) => {
                        if (inx != 0) {
                            menuItems.push(ch);
                            if (ch.getElementsByClassName("menu-task-text")[0].innerText == tryFilter.cap) {
                                ch.getElementsByClassName("menu-task")[0].checked = true;
                                selectedItems.push(ch);
                            } else {
                                ch.getElementsByClassName("menu-task")[0].checked = false;
                            }
                        }
                    });

                    if (selectedItems.length == menuItems.length) {
                        selectAll.checked = true;
                        selectAllText.innerText = "Unselect All";
                    } else {
                        selectAll.checked = false;
                        selectAllText.innerText = "Select All";
                    }
                });
            } else {
                Object.values(tryFilter.allTrs).filter((elm) => {
                    if (tryFilter.user != "") {
                        let td = elm.getElementsByTagName("td")[0];
                        if (td) {
                            let txtValue = td.getAttribute("data-fromuser");

                            if (txtValue.trim().indexOf(tryFilter.user) > -1) {
                                elm.style.display = "";
                            } else {
                                elm.style.display = "none";
                            }
                        }
                    } else if (tryFilter.from != "" && tryFilter.to != "") {
                        let td = elm.getElementsByTagName("td")[0];
                        if (td) {
                            let txtValue = td.getAttribute("data-tasktime");
                            txtValue = txtValue.split(" ")[0];
                            let dateVal = moment(txtValue, "DD/MM/YYYY");
                            let visible = "";
                            if (tryFilter.from === tryFilter.to) {
                                visible = (dateVal._i === tryFilter.from ? "" : "none");
                            } else {
                                visible = (dateVal.isBetween(moment(tryFilter.from, "DD/MM/YYYY"), moment(tryFilter.to, "DD/MM/YYYY"), null, [])) ? "" : "none";
                            }
                            elm.style.display = visible;
                        }
                    }
                });
            }
        }
    };

    openCurrentTask(keyValue, taskId) {
        ShowDimmer(true);
        callParentNew(`LoadIframe(htmlPages.aspx?loadcaption=AxProcessFlow&processname=${this.processName}&keyvalue=${keyValue}&taskid=${taskId})`, "function");
    }

    showProgress(elem) {
        setTimeout(function () {
            axProcessObj.beforeLoad();
        }, 100)
        let _this = this, data = {}, url = "";
        url = "../../aspx/AxPEG.aspx/AxGetProcess";
        data = { processName: this.processName, keyField: this.keyField, keyValue: this.keyValue };
        let elemTaskId = elem?.dataset?.taskid;
        this.callAPI(url, data, true, result => {
            if (result.success) {
                _this.hideDefaultCenterPanel();
                ShowDimmer(false);
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");

                let sno = 1;
                document.querySelector('#horizontal-processbar').innerHTML = "";
                dataResult.forEach((rowData, idx) => {
                    let tempTaskType = rowData.tasktype.toUpperCase();
                    if (["IF", "ELSE", "ELSE IF", "END"].indexOf(tempTaskType) == -1) {
                        if (rowData.indexno == "1" && this.inValid(rowData.taskstatus)) {
                            rowData.taskstatus = "pending";
                        }
                        else if (rowData.taskstatus == "Active") {
                            rowData.taskstatus = "pending";
                        }

                        if (this.isNullOrEmpty(rowData.taskstatus) && rowData.indexno > 1) {
                            rowData.taskstatus = "disabled";
                        }
                        else if (["APPROVED", "REJECTED", "RETURNED", "CHECKED", "MADE"].indexOf(rowData.taskstatus?.toUpperCase()) > -1) {
                            rowData.taskstatus = "completed";
                        }

                        //if (this.taskId == rowData.taskid) {
                        //    rowData.taskstatus = "active";
                        //}

                        if (this.isNullOrEmpty(rowData.recordid)) {
                            rowData.recordid = "0";
                        }

                        rowData.sno = sno;
                        sno++;

                        document.querySelector('#horizontal-processbar').insertAdjacentHTML("beforeend", ` ${Handlebars.compile(this.horizontalStepHtml)(rowData)} `);

                    }
                });

                if (this.taskCompleted) {
                    this.taskCompleted = false;
                    let nextTask = this.getNextTaskInProcess();
                    if (!nextTask) {
                        if (!this.inValid(document.querySelector(".horizontal-steps.pending"))) {
                            document.querySelector(".horizontal-steps.pending")?.click();
                            document.querySelector(".horizontal-steps.pending")?.scrollIntoView();
                        }
                        else if (!this.inValid(document.querySelector(".horizontal-steps"))) {
                            document.querySelector(".horizontal-steps")?.click();
                            document.querySelector(".horizontal-steps")?.scrollIntoView();
                        }
                    }
                }
                else if (!this.inValid(this.target)) {
                    let pendingSelector = `.horizontal-steps.pending[data-taskname="${this.target}"]`;
                    let selector = `.horizontal-steps[data-taskname="${this.target}"]`;
                    if (!this.inValid(document.querySelector(pendingSelector))) {
                        document.querySelector(selector)?.click();
                        document.querySelector(pendingSelector)?.scrollIntoView();
                    }
                    else if (!this.inValid(document.querySelector(selector))) {
                        document.querySelector(selector)?.click();
                        document.querySelector(selector)?.scrollIntoView();
                    }
                    this.target = null;
                }

                if (!_this.inValid(elemTaskId)) {
                    _this.setActiveInList(elemTaskId);
                }

                setTimeout(function () {
                    axProcessObj.afterLoad();
                }, 100)
            }
            else {
                ShowDimmer(false);
            }
        });

    }

    beforeLoad() { }
    afterLoad() { }

    showTimeLine() {
        axTimeLineObj = new ProcessTimeLine();
        axTimeLineObj.keyvalue = this.keyValue;
        axTimeLineObj.processName = this.processName;

        if (!this.inValid(axTimeLineObj.keyvalue) && axTimeLineObj.keyvalue != "NA")
            axTimeLineObj.getTimeLineData();
    }

    showList() {
        document.querySelectorAll("#showList,#PROFLOW-profile-container").forEach((item) => {
            item.classList.add("d-none");
        });
        document.querySelectorAll("#plistAccordion,#showProgress").forEach((item) => {
            item.classList.remove("d-none");
        });
    }

    showDefaultList() {
        document.querySelectorAll("#showList,#PROFLOW-profile-container,#showProgress").forEach((item) => {
            item.classList.add("d-none");
        });
        document.querySelectorAll("#plistAccordion").forEach((item) => {
            item.classList.remove("d-none");
        });
    }

    refreshCards(cardIds, cardParamsValues) {
        if (this.inValid(cardIds))
            return;
        $.ajax({
            url: "../../aspx/AxPEG.aspx/AxRefreshCardsData",
            type: 'POST',
            cache: false,
            async: true,
            data: JSON.stringify({
                cardsIds: cardIds.join(","), cardsParams: cardParamsValues
            }),
            dataType: 'json',
            contentType: "application/json",
            success: (data) => {
                if (data.d && data.d != "") {
                    let result = JSON.parse(data.d);
                    cardsData.value = JSON.parse(cardsData.value);
                    var mergedCardsData = cardsData.value.concat(result.result.cards); // Merge the two arrays
                    /*var uniqueCardsData = mergedCardsData.reduce((acc, current) => {
                        const x = acc.find(item => item.axp_cardsid === current.axp_cardsid);
                        if (!x) {
                            return acc.concat([current]);
                        } else {
                            return acc;
                        }
                    }, []);*/

                    var uniqueCardsData = mergedCardsData.reduce((acc, current) => {
                        const x = acc.find(item => item.axp_cardsid === current.axp_cardsid);
                        if (!x) {
                            return acc.concat([current]);
                        } else {
                            x.cardsql = current.cardsql; // Update the "cardsql" property value of the matching element
                            return acc;
                        }
                    }, []);

                    cardsData.value = JSON.stringify(uniqueCardsData);
                    cardsDesign.value = "";
                    //xmlMenuData = result.menu;
                    //taskcards = JSON.parse(result.taskcards).result[0].result.row;
                    //if (taskcards != '') {
                    //    taskcards = taskcards.map(item => item.cardname);
                    //}
                } else {
                    showAlertDialog("error", "Error while loading cards dashboard..!!");
                    return;
                }

                if (xmlMenuData != "") {
                    xmlMenuData = xmlMenuData.replace(/&apos;/g, "'");
                    var xml = parseXml(xmlMenuData)
                    var xmltojson = xml2json(xml, "");
                    menuJson = JSON.parse(xmltojson);
                }
                appGlobalVarsObject._CONSTANTS.menuConfiguration = $.extend(true, {},
                    appGlobalVarsObject._CONSTANTS.menuConfiguration, {
                    menuJson: menuJson,
                });
                // try {
                appGlobalVarsObject._CONSTANTS.cardsPage = $.extend(true, {},
                    appGlobalVarsObject._CONSTANTS.cardsPage, {
                    setCards: true,
                    cards: (JSON.parse(cardsData.value !== "" ? ReverseCheckSpecialChars(cardsData.value) : "[]",
                        function (k, v) {
                            try {
                                return (typeof v === "object" || isNaN(v) || v.toString().trim() === "") ? v : (typeof v == "string" && (v.startsWith("0") || v.startsWith("-")) ? parseFloat(v, 10) : JSON.parse(v));
                            } catch (ex) {
                                return v;
                            }
                            //0 & - starting with does not gets parsed in json.parse
                            //json.parse is used because it porcess int, float and boolean together
                        }
                    ) || []).map(arr => _.mapKeys(arr, (value, key) => key.toString().toLowerCase())),
                    design: (JSON.parse(cardsDesign.value !== "" ? cardsDesign.value : "[]",
                        function (k, v) {
                            try {
                                return (typeof v === "object" || isNaN(v) || v.toString().trim() === "") ? v : (typeof v == "string" && (v.startsWith("0") || v.startsWith("-")) ? parseFloat(v, 10) : JSON.parse(v));
                            } catch (ex) {
                                return v;
                            }
                        }
                    ) || []).map(arr => _.mapKeys(arr, (value, key) => key.toString().toLowerCase())),
                    enableMasonry: cardsDashboardObj.enableMasonry,
                    staging: {
                        iframes: ".splitter-wrapper",
                        cardsFrame: {
                            div: ".cardsPageWrapper",
                            cardsDiv: ".cardsPlot",
                            cardsDesigner: ".cardsDesigner",
                            cardsDesignerToolbar: ".designer",
                            editSaveButton: ".editSaveCardDesign",
                            icon: "span.material-icons",
                            divControl: "#arrangeCards"
                        },
                    }
                });

                var lcm = appGlobalVarsObject.lcm;
                var tempaxpertUIObj = $.axpertUI
                    .init({
                        isHybrid: appGlobalVarsObject._CONSTANTS.isHybrid,
                        isMobile: cardsDashboardObj.isMobile,
                        compressedMode: appGlobalVarsObject._CONSTANTS.compressedMode,
                        dirLeft: cardsDashboardObj.dirLeft,
                        axpertUserSettings: {
                            settings: appGlobalVarsObject._CONSTANTS.axpertUserSettings
                        },
                        cardsPage: appGlobalVarsObject._CONSTANTS.cardsPage
                    });

                appGlobalVarsObject._CONSTANTS.cardsPage = tempaxpertUIObj.cardsPage;
            },
            error: (error) => {
                showAlertDialog("error", "Error while loading cards dashboard..!!");
                return;
            },
            failure: (error) => {
                showAlertDialog("error", "Error while loading cards dashboard..!!");
                return;
            },
        });
    }
}

var axProcessTreeObj;
class AxProcessTree {
    constructor(processName) {
        this.definitionFetched = false;
        this.dataSources = [];

        this.processName = processName;
        this.stepHtml = `
            <div class="step">
                <div>
                    <div class="circle ">
                        <i class="fa fa-check"></i>
                        <span class="Emp-steps-counts">{{sno}}</span>
                    </div>
                    <div class="line"></div>
                </div>
                <div class="Task-process-wrapper">
                    {{groupNameHtml}}
                    {{taskCaptionHtml}}
                </div>
            </div>`;
        this.groupNameHtml = `
            <div class="title">
                <a href="javascript:void(0)">{{taskgroup}}</a>
                <span data-groupname="{{taskgroup}}" class="Process-flow accordion-icon rotate">
                    <span  class="material-icons material-icons-style material-icons-2">chevron_right</span>                    
                </span>
            </div>`
        this.taskCaptionHtml = `<div class="process-sub-flow" data-groupname="{{taskgroup}}" data-tasktype="{{tasktype}}">`;
        this.taskCaptionHtml += `<div class="positionRel Task-process-list">`;
        this.taskCaptionHtml += `<a href="javascript:void(0)" onclick="return false;">{{taskname}}</a>`;
        this.taskCaptionHtml += `</div></div>`;
    }

    fetchProcessDefinition(name) {
        let _this = this;
        let url = "../../aspx/AxPEG.aspx/AxGetProcessDefinition";
        let data = { processName: this.processName };
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.dataSources[name] = dataResult;
                _this.definitionFetched = true;
            }
        });
    }

    showProcessTree() {
        try {
            let myModal = new BSModal(`modal_ProcessTree`, "", `<div class="PROFLOW-Info-Steps ax-data accordion arrows" id="procflow-steps"></div>`,
                (opening) => {
                    ShowDimmer(true);
                    if (!this.definitionFetched) {
                        this.fetchProcessDefinition("Process");
                    }
                    this.showProcessDefinition();
                    ShowDimmer(false);

                }, (closing) => {
                    //document.querySelector('#procflow-steps').innerHTML = "";
                }
            );

            myModal.changeSize("md");
            myModal.hideHeader();
            myModal.hideFooter();
            myModal.showFloatingClose();
        } catch (error) {
            showAlertDialog("error", error.message);
        }
    }

    showProcessDefinition() {
        if (document.querySelector('#procflow-steps').innerHTML != "")
            return;
        this.processFlowObj = {};
        this.dataSources["Process"].forEach((rowData, idx) => {
            if (this.isUndefined(this.processFlowObj[rowData.taskgroup])) {
                this.processFlowObj[rowData.taskgroup] = {};
                this.processFlowObj[rowData.taskgroup].group_name_html = '';
                this.processFlowObj[rowData.taskgroup].task_caption_html = '';

            }

            if (this.isNullOrEmpty(rowData.taskstatus) && rowData.indexno > 1) {
                rowData.taskstatus = "disabled";
            }

            if (this.isNullOrEmpty(rowData.recordid)) {
                rowData.recordid = "0";
            }

            let taskGroup = this.processFlowObj[rowData.taskgroup];
            taskGroup.indexno = rowData.indexno;
            taskGroup.group_name_html = Handlebars.compile(this.groupNameHtml)(rowData);
            taskGroup.task_caption_html += Handlebars.compile(this.taskCaptionHtml)(rowData);

        });

        document.querySelector('#procflow-steps').innerHTML = "";


        let sno = 1;
        for (let [key, value] of Object.entries(this.processFlowObj)) {
            document.querySelector('#procflow-steps').insertAdjacentHTML("beforeend", ` ${this.stepHtml.replace("{{sno}}", sno).replace("{{groupNameHtml}}", value.group_name_html).replace("{{taskCaptionHtml}}", value.task_caption_html)} `);
            sno++;
        }
        ShowDimmer(false);


        $(".accordion-icon").click(function () {
            let groupname = $(this).attr('data-groupname');
            $(this).toggleClass('rotate');
            $(`.process-sub-flow[data-groupname="${groupname}"]`).toggle();
        });

    }

    callAPI(url, data, async, callBack) {
        let _this = this;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, async);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        if (_this.isAxpertFlutter) {
            xhr.setRequestHeader('Authorization', `Bearer ${armToken}`);
            data["armSessionId"] = armSessionId;
        }
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

    catchError(error) {
        showAlertDialog("error", error);
    };

    showSuccess(message) {
        showAlertDialog("success", message);
    };

    dataConvert(data, type) {
        if (type == "AXPERT") {
            try {
                data = JSON.parse(data.d);
                if (typeof data.result[0].result.row != "undefined") {
                    return data.result[0].result.row;
                }

                if (typeof data.result[0].result != "undefined") {
                    return data.result[0].result;
                }
            }
            catch (error) {
                ShowDimmer(false);
                this.catchError(error.message);
            };
        }
        else if (type == "ARM") {
            try {
                if (!this.isAxpertFlutter)
                    data = JSON.parse(data.d);
                if (data.result && data.result.success) {
                    if (!this.isUndefined(data.result.data)) {
                        return data.result.data;
                    }
                }
                else {
                    if (!this.isUndefined(data.result.message)) {
                        this.catchError(data.result.message);
                    }
                }
            }
            catch (error) {
                ShowDimmer(false);
                this.catchError(error.message);
            };
        }

        return data;
    };

    generateFldId() {
        return `fid${Date.now()}${Math.floor(Math.random() * 90000) + 10000}`;
    };

    isEmpty(elem) {
        return elem == "";
    };

    isNull(elem) {
        return elem == null;
    };

    isNullOrEmpty(elem) {
        return elem == null || elem == "";
    };

    inValid(elem) {
        return elem == null || typeof elem == "undefined" || elem == "";
    };

    isUndefined(elem) {
        return typeof elem == "undefined";
    };

}

var axTimeLineObj;
class ProcessTimeLine {
    constructor() {
        this.keyvalue = "";
        this.processName = "";
        this.make = `<li class="make">                                   
                    <p class="T-Desc">{{taskname}}#{{keyvalue}}</p>
 		    <p class="T-Heading">{{taskfromuser}}</p>
 		    <div class="time">{{tasktime}}</div>
                </li>`;
        this.check = `<li class="check">
                    <p class="T-Desc">{{taskname}}#{{keyvalue}} - {{taskstatus}}</p>
                    <p class="T-Heading">{{taskfromuser}}</p>                    
		    <div class="time">{{tasktime}}</div>
                </li>`;
        this.approve = `<li class="approve">
                   <p class="T-Desc">{{taskname}}#{{keyvalue}} - {{taskstatus}}</p>
                    <p class="T-Heading">{{taskfromuser}}</p>                    
		    <div class="time">{{tasktime}}</div>
                </li>`;
    }

    getTimeLineData() {
        ShowDimmer(true);
        let _this = this;
        let url = "../../aspx/AxPEG.aspx/AxGetTimelineData";
        let data = { keyValue: _this.keyvalue, processName: _this.processName };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.constructTimeline(dataResult);
            }
        });
    };

    constructTimeline(data) {
        if (this.isUndefined(data) || data.length == 0) {
            document.querySelector(".Timel-sessions").classList.add("d-none");
            document.querySelector("#nodata").classList.remove("d-none");
        }
        else {
            document.querySelector("#nodata").classList.add("d-none");
            document.querySelector(".Timel-sessions").classList.remove("d-none");
            var timelineData = "";
            document.querySelector('.Timel-sessions').innerHTML = '';
            data.forEach((rowData) => {
                timelineData += Handlebars.compile(this[rowData.tasktype.toLowerCase()])(rowData);
            })
            document.querySelector('.Timel-sessions').insertAdjacentHTML("beforeend", timelineData);
        }
    }

    callAPI(url, data, async, callBack) {
        let _this = this;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, async);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        if (_this.isAxpertFlutter) {
            xhr.setRequestHeader('Authorization', `Bearer ${armToken}`);
            data["armSessionId"] = armSessionId;
        }
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

    catchError(error) {
        showAlertDialog("error", error);
    };

    showSuccess(message) {
        showAlertDialog("success", message);
    };

    dataConvert(data, type) {
        if (type == "AXPERT") {
            try {
                data = JSON.parse(data.d);
                if (typeof data.result[0].result.row != "undefined") {
                    return data.result[0].result.row;
                }

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
                if (!this.isAxpertFlutter)
                    data = JSON.parse(data.d);
                if (data.result && data.result.success) {
                    if (!this.isUndefined(data.result.data)) {
                        return data.result.data;
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
    };

    generateFldId() {
        return `fid${Date.now()}${Math.floor(Math.random() * 90000) + 10000}`;
    };

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

    inValid(elem) {
        return elem == null || typeof elem == "undefined" || elem == "";
    };
}

$(document).ready(function () {
    ShowDimmer(true);
    axProcessObj = new AxProcessFlow();
    axProcessObj.init();
});
