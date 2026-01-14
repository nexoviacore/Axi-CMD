var armToken = "", axProcessObj;

var LoadIframe = callParentNew("LoadIframe");
var cardsData = {}, cardsDesign = {}, xmlMenuData = "", menuJson = "";
var _horizontalFlow = true;
var _autoLoadNextTask = true;
var dtCulture = eval(callParent('glCulture'));
var processflowJson = {}, taskdetailsJson = {}
var TaskCount = 0;
callParentNew("AxNotifyMsgId=", '');
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

const myDiv = document.getElementById('body_Container');

class AxProcessFlow {
    constructor() {
        this._entity = {
            inValid: function (elem) {
                return elem == null || typeof elem == "undefined" || elem == "";
            },
            "metaData": [{
                    "fldname": "DropDown",
                    "fldcap": "Task Status",
                    "fdatatype": "c",
                    "cdatatype": "DropDown",
                    "hide": "F"
                },
                {
                    "fldname": "DropDown",
                    "fldcap": "Task Type",
                    "fdatatype": "c",
                    "cdatatype": "DropDown",
                    "hide": "F"
                },
                {
                    "fldname": "DropDown",
                    "fldcap": "Message Type",
                    "fdatatype": "c",
                    "cdatatype": "DropDown",
                    "hide": "F"
                },
                {
                    "fldname": "DropDown",
                    "fldcap": "Process Name",
                    "fdatatype": "c",
                    "cdatatype": "DropDown",
                    "hide": "F"
                },
                {
                    "fldname": "DropDown",
                    "fldcap": "User Name",
                    "fdatatype": "c",
                    "cdatatype": "DropDown",
                    "hide": "F"
                },
                {
                    "fldname": "Date",
                    "fldcap": "Date",
                    "fdatatype": "d",
                    "cdatatype": "Date",
                    "hide": "F"
                },
                {
                    "fldname": "Numeric",
                    "fldcap": "Numeric",
                    "fdatatype": "n",
                    "cdatatype": "Numeric",
                    "hide": "F"
                },
                {
                    "fldname": "Text",
                    "fldcap": "Text",
                    "fdatatype": "t",
                    "cdatatype": "Text",
                    "hide": "F"
                }
            ]
        }
        this.tasksJson = {}
        this.filterChanged = false
        this.pageNo = 1
        this.pageSize = 20
        this.count = 0
        this.isFetching = false
        this.lastScrollLeft = 0
        this.lastScrollTop = 0
        this.isDropdownClick = false
        this.filterkey = "All"
        this.isInitialized = false;
        this.isAxpertFlutter = !this.isNullOrEmpty(armToken);
        this.cardParams = {};
        this.cardFlds = {};
        this.processName = '';
        this.keyField = '';
        this.keyValue = '';
        this.taskCompleted = false;
        this.taskId = '';
        this.userType = 'GUEST';
        this.isTaskEditable = false;
        this.currentElem = null;
        this.taskStatus = '';
        this.currentIndex = 1;
        this.stepHtml = `
            <div class="step">
                <div>
                    <div class="circle d-none">
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
            <div class="Task-process-list vertical-steps status-{{taskstatus}}" data-indexno='{{indexno}}' onclick="axProcessObj.openTask(this, '{{taskname}}', '{{tasktype}}', '{{transid}}', '{{keyfield}}', '{{keyvalue}}', '{{recordid}}', '{{taskid}}', '{{indexno}}','{{hlink_transid}}','{{hlink_params}}','{{processname}}');" data-taskid="{{taskid}}" data-tasktype="{{tasktype}}" data-transid="{{transid}}" data-recordid="{{recordid}}">
                <a href="#">{{taskname}}</a>
            </div>
        </div>`;
        this.bulkTaskRowHtml = `
        <div class="table align-middle  fs-6 gy-5 mb-0 dataTable no-footer task-listing-card">
            
               
                        <div class="d-flex flex-column task-name">
                        <div class="d-flex">
                            <input type="checkbox" value="" class="form-check-input task-list-checkbox my-auto" data-taskid="{{taskid}}" >
                            <a href="javascript:void(0)"  class="text-gray-800 fw-bolder fs-6 task-title pt-0 pb-2 px-0" title="{{displaytitle}}"><span href="javascript:void(0)"><span class="material-icons material-icons-style material-icons-1 display-icon task-listing-icons">{{displayicon}}</span>{{displaytitle}}</span></a>
                            </div><div class="task-subtitle">{{displaycontent}}</div>
                        </div>
                  

                        <div class="d-flex flex-row task-process gap-5">
                            <a href="javascript:void(0)" class="d-flex text-gray-800 task-assignedBy my-auto mb-1 gap-2" title="Assigned By"><span class="material-icons material-icons-style p-0">person</span><span class="p-0"> {{fromuser}}</span></a>
                            <a href="javascript:void(0)" title="Assigned On" class="d-flex text-gray-800 mb-1 task-date my-auto gap-2"><span class="material-icons material-icons-style p-0">today</span><span class="p-0"> {{eventdatetime}}</span></a>
                        </div>
                                      
           
        </div>`;

        this.horizontalStepHtml = `<li class="{{taskstatus}}">
                                <a href="javascript:void(0)" onclick="axProcessObj.openTask(this, '{{taskname}}', '{{tasktype}}', '{{transid}}', '{{keyfield}}', '{{keyvalue}}', '{{recordid}}', '{{taskid}}', '{{indexno}}','{{hlink_transid}}','{{hlink_params}}','{{processname}}');" data-taskid="{{taskid}}" data-tasktype="{{tasktype}}" data-transid="{{transid}}" data-recordid="{{recordid}}" data-taskname="{{taskname}}" data-indexno='{{indexno}}' class="horizontal-steps {{taskstatus}}">
                                    <span class="circle">{{sno}}</span>
                                    <span class="label">{{taskname}}</span>
                                </a>
                            </li>`;

        this.dataSources = [];
        this.processFlowObj = {};
        this.processProgressObj = {};
        this.getUrlParams();
        this.horizontalFlow = _horizontalFlow;
        this.autoLoadNextTask = _autoLoadNextTask;
        this.isScrollAtBottomWithinDiv = this.isScrollAtBottomWithinDiv.bind(this);
        this.fetchProcessList = this.fetchProcessList.bind(this);

        this.processVars = {
            plistTbIcons: ["add_task", "receipt_long", "post_add", "library_books", "free_cancellation", "published_with_changes"],
            plistCols: {
                taskName: {
                    caption: "Task Name",
                    hidden: false,
                },
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
        this.toolbarDrawerHTML = `<div class="Tkts-toolbar-Right">
                            <button id="" type="submit" class="btn btn-sm btn-icon btn-primary btn-active-primary btn-custom-border-radius d-none">
                                <span class="material-icons material-icons-style material-icons-2">add_task</span>
                            </button>
                            <button type="submit" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" onclick="axProcessObj.taskFilter('reset')" id="reset">
                                <span class="material-icons material-icons-style material-icons-2">refresh</span>
                            </button>


                            <button type="submit" id="" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm  tb-btn btn-sm">
                                <span class="material-icons material-icons-style material-icons-2">history_toggle_off</span>
                            </button>
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
        this.calledFrom = "";
        this.getProcessUserType();
    }

    getProcessUserType() {
        let _this = this, data = {}, url = "";
        url = "../aspx/AxPEG.aspx/AxGetProcessUserType";
        data = { processName: this.processName };

        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                if (json.d == 'Error in ARM connection.') {
                    showAlertDialog("error", 'Error in ARM connection.');
                    return;
                }
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
        var nextTask = false;
        if (!this.autoLoadNextTask)
            return nextTask;

        let _this = this, data = {}, url = "";
        url = "../aspx/AxPEG.aspx/AxGetNextTaskInProcess";
        data = { processName: this.processName, keyValue: this.keyValue };
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                if (dataResult.length > 0) {
                    let rowData = dataResult[0];
                    if (this.horizontalFlow && rowData.nexttasktype != "Current Process")
                        nextTask = false;

                    if ('URLSearchParams' in window) {
                        nextTask = true;
                        var searchParams = new URLSearchParams(window.location.search);
                        searchParams.set("keyfield", rowData.keyfield);
                        searchParams.set("keyvalue", rowData.keyvalue);
                        searchParams.set("taskid", rowData.taskid);
                        searchParams.set("target", "");
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
        let url = "../aspx/AxPEG.aspx/AxGetKeyValue";
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
        this.refreshPage();
        return;
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
        if (this.horizontalFlow)
            this.showProgress();
        else
            this.showVerticalProgress();
        this.fetchProcessList("ProcessList", "TaskCompletion")
        //this.showVerticalProgress();
        //this.fetchProcessKeyValues("ProcessKeyValues");
    };

    hideDefaultCenterPanel() {
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
    }

    fetchProcessList() {
        const url = "../aspx/AxPEG.aspx/AxGetAllActiveTasks";
        const data = {
            pageNo: this.pageNo,
            pageSize: this.pageSize,
            filter: this.filterkey.toLocaleLowerCase()
        };

        this.callAPI(url, data, true, (result) => {
            if (result.success) {
                const json = JSON.parse(result.response);

                if (json.d === "Error in ARM connection.") {
                    ShowDimmer(false);
                    showAlertDialog("error", "Error in ARM connection.");
                    return;
                }

                const dataResult = this.dataConvert(json, "ARM");
                this.tasksJson = dataResult.result.tasks || []; // Safeguard for empty tasks
                if (this.tasksJson.length > 0) {
                    this.showProcessList(); // Append new data
                } else {
                    this.showNoMoreRecords(); // Display "No more records" message
                }
                ShowDimmer(false);
            } else {
                ShowDimmer(false);
                showAlertDialog("error", "Failed to fetch tasks.");
            }
        });
    }



    filterProcessList(reset) {
        if (reset)
            document.querySelector("#advTextSearch").value = "";

        var searchTerm = document.querySelector("#advTextSearch").value;
        let _this = this;
        if (!_this.inValid(searchTerm)) {
            searchTerm = searchTerm.toLowerCase();
            var filteredResults = axProcessObj.dataSources["ProcessList"].list.filter(function (item) {
                return item.displaytitle?.toLowerCase().includes(searchTerm);
            });

            document.querySelectorAll(`#plistAccordion tr`).forEach((listItems) => {
                listItems.classList.add("d-none");
                listItems.classList.add("filterApplied");
            });

            if (filteredResults.length > 0) {
                filteredResults.forEach((filteredRow) => {
                    document.querySelectorAll(`.filterApplied [data-taskid="${filteredRow.taskid}"]`).forEach((i) => {
                        i?.closest('tr')?.classList.remove("d-none");
                        i?.closest('tr')?.classList.remove("filterApplied");
                    })
                })
            }
        }
        else {
            document.querySelectorAll(`.filterApplied`).forEach((filteredRow) => {
                filteredRow.classList.remove("d-none");
                filteredRow.classList.remove("filterApplied");
            });
        }
    }

    fetchProcessKeyValues(name) {
        let _this = this;
        let url = "../aspx/AxPEG.aspx/AxGetProcessKeyValues";
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

    showVerticalProgress(elem) {
        let _this = this, data = {}, url = "";
        url = "../aspx/AxPEG.aspx/AxGetProcess";
        data = { processName: this.processName, keyField: this.keyField, keyValue: this.keyValue };
        let elemTaskId = elem?.dataset?.taskid || this.taskId;
        this.callAPI(url, data, true, result => {
            if (result.success) {
                _this.hideDefaultCenterPanel();
                ShowDimmer(false);
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                this.processProgressObj = {};
                dataResult.forEach((rowData, idx) => {
                    let tempTaskType = rowData.tasktype.toUpperCase();
                    if (["IF", "ELSE", "ELSE IF", "END"].indexOf(tempTaskType) == -1) {
                        if (this.isNullOrEmpty(rowData.taskstatus) && rowData.indexno > 1) {
                            rowData.taskstatus = "disabled";
                            return;
                        }

                        if (this.isUndefined(this.processProgressObj[rowData.taskname])) {
                            this.processProgressObj[rowData.taskname] = {};
                            this.processProgressObj[rowData.taskname].group_name_html = '';
                            this.processProgressObj[rowData.taskname].task_caption_html = '';
                        }

                        if (this.isNullOrEmpty(rowData.recordid)) {
                            rowData.recordid = "0";
                        }

                        let taskGroup = this.processProgressObj[rowData.taskname];
                        taskGroup.indexno = rowData.indexno;
                        //taskGroup.group_name_html = Handlebars.compile(this.groupNameHtml)(rowData);
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

                if (this.taskCompleted) {
                    this.taskCompleted = false;
                    let nextTask = this.getNextTaskInProcess();
                    if (!nextTask) {
                        if (!this.inValid(document.querySelector(".vertical-steps.status-Active"))) {
                            this.calledFrom = "ProgressBar";
                            document.querySelector(".vertical-steps.status-Active")?.click();
                            document.querySelector(".vertical-steps.status-Active")?.scrollIntoView();
                            elemTaskId = document.querySelector(".vertical-steps.status-Active").dataset?.taskid;
                        }
                        else if (!this.inValid(document.querySelector(".vertical-steps"))) {
                            this.calledFrom = "ProgressBar";
                            document.querySelector(".vertical-steps")?.click();
                            document.querySelector(".vertical-steps")?.scrollIntoView();
                            elemTaskId = document.querySelector(".vertical-steps").dataset?.taskid;
                        }
                    }
                }
                else if (!this.inValid(this.taskId)) {
                    this.calledFrom = "ProgressBar";
                    let selector = `.vertical-steps[data-taskid="${this.taskId}"]`;
                    if (!this.inValid(document.querySelector(selector))) {
                        document.querySelector(selector)?.click();
                        document.querySelector(selector)?.scrollIntoView();
                        if (this.inValid(elemTaskId))
                            elemTaskId = document.querySelector(selector).dataset?.taskid;
                    }
                    this.target = null;
                }
                else if (!this.inValid(this.target)) {
                    this.calledFrom = "ProgressBar";
                    let selector = `.vertical-steps[data-taskname="${this.target}"]`;
                    if (!this.inValid(document.querySelector(selector))) {
                        document.querySelector(selector)?.click();
                        document.querySelector(selector)?.scrollIntoView();
                        if (this.inValid(elemTaskId))
                            elemTaskId = document.querySelector(selector).dataset?.taskid;
                    }
                    else if (!this.inValid(document.querySelector(".vertical-steps"))) {
                        this.calledFrom = "ProgressBar";
                        document.querySelector(".vertical-steps")?.click();
                        document.querySelector(".vertical-steps")?.scrollIntoView();
                        if (this.inValid(elemTaskId))
                            elemTaskId = document.querySelector(".vertical-steps").dataset?.taskid;
                    }
                    this.target = null;
                }

                if (!_this.inValid(elemTaskId)) {
                    _this.setActiveInList(elemTaskId);
                }

                $(".accordion-icon").click(function () {
                    let groupname = $(this).attr('data-groupname');
                    $(this).toggleClass('rotate');
                    $(`.process-sub-flow[data-groupname="${groupname}"]`).toggle();
                });

            }
        });
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

    openTask(elem, taskName, taskType, transId, keyField, keyValue, recordId, taskId, indexNo, hLinkPage, hLinkParam, processName, messageType, calledFrom) {
        document.getElementById('horizontal-processbar').innerHTML = ""
        ShowDimmer(true);
        this.currentIndex = parseInt(indexNo);
        this.keyField = keyField;
        this.keyValue = keyValue || this.keyValue;
        this.taskId = taskId;
        this.processName = processName;
        this.taskType = taskType;
        if (!this.inValid(calledFrom))
            this.calledFrom = calledFrom;
        callParentNew("AxNotifyMsgId=", '');
        if (!this.inValid(messageType) && (messageType.toLowerCase() == "message" || messageType.toLowerCase() == "form notification" || messageType.toLowerCase() == "periodic notification"))
            taskType = messageType;
        switch (taskType.toUpperCase()) {
            case "MAKE":
                document.querySelector("#pd_timeline").classList.remove("d-none");
                this.showProgressNew();
                this.openTstruct(taskName, transId, keyField, keyValue, recordId);
                break;
            case "CHECK":
                document.querySelector("#pd_timeline").classList.remove("d-none");
                this.showProgressNew();
                this.openProcessTask(taskName, taskType, taskId);
                break;
            case "APPROVE":
                document.querySelector("#pd_timeline").classList.remove("d-none");
                this.showProgressNew();
                this.openProcessTask(taskName, taskType, taskId);
                break;
            case "FORM NOTIFICATION":
            case "PERIODIC NOTIFICATION":
            case "MESSAGE":
                document.querySelector("#pd_timeline").classList.add("d-none");
                this.openMessageLink(hLinkPage, hLinkParam);
                break;
            case "CACHED SAVE":
                document.querySelector("#pd_timeline").classList.add("d-none");
                this.openMessageLinkCachedSave(hLinkPage, hLinkParam, taskId);
                break;
            case "EXPORT":
                this.DownloadExportFile(hLinkParam);
                break;
            default:
                document.querySelector("#pd_timeline").classList.add("d-none");
                let _errorTitle = $(elem).text().trim();
                let _errorMessage = $(elem).closest('.row').find('.taskcontent').text();
                this.showErrorModal(_errorTitle, _errorMessage);
                ShowDimmer(false);
                break;
        }
    };

     showErrorModal(title, message) {
         $("#dynamicErrorModal").remove();
    let modalHtml = `
        <div class="modal fade" id="dynamicErrorModal" tabindex="-1">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header bg-danger-- text-white--">
                <h5 class="modal-title">${title}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
              </div>
              <div class="modal-body">${message}</div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>`;
         $("body").append(modalHtml);
         let modal = new bootstrap.Modal(document.getElementById('dynamicErrorModal'));
         modal.show();
    };

    
    DownloadExportFile(eLink) {
        if (eLink != "" && (eLink.startsWith('http:') || eLink.startsWith('https:'))) {
            let fileUrl = eLink;
            let fileName = eLink.substring(eLink.lastIndexOf('/') + 1);

            const lastUnderscoreIndex = fileName.lastIndexOf('_');
            const filenameWithoutExtension = fileName.substring(0, lastUnderscoreIndex);
            const fileExtension = fileName.substring(fileName.lastIndexOf('.'));
            const finalFilename = filenameWithoutExtension + fileExtension;
            var anchor = document.createElement('a');
            anchor.href = fileUrl;
            anchor.download = finalFilename;
            document.body.appendChild(anchor);
            anchor.click();
            setTimeout(function () {
                setTimeout(() => {
                    document.body.removeChild(anchor);
                }, 0);
            }, 500);
            ShowDimmer(false);
        } else {
            let filePath = eLink.substring(0, eLink.lastIndexOf('\\') + 1);
            let fileName = eLink.substring(eLink.lastIndexOf('\\') + 1);

            $.ajax({
                type: "POST",
                url: "../WebService.asmx/LoadExportFileToScript",
                cache: false,
                async: false,
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({
                    filePath: filePath, fileName: fileName
                }),
                dataType: "json",
                success: (data) => {
                    if (data.d && data.d != "") {
                        var fileUrl = data.d;
                        var fileName = data.d.substring(data.d.lastIndexOf('/') + 1);
                        var anchor = document.createElement('a');
                        anchor.href = fileUrl;
                        anchor.download = fileName;
                        document.body.appendChild(anchor);
                        anchor.click();
                        /* document.body.removeChild(anchor);*/
                        setTimeout(function () {
                            setTimeout(() => {
                                document.body.removeChild(anchor);
                            }, 0);
                        }, 500);
                        ShowDimmer(false);
                    } else
                        ShowDimmer(false);
                }, error: (error) => {
                    ShowDimmer(false);
                },
                failure: (error) => {
                    ShowDimmer(false);
                },
            });
        }
    };

    setActiveInList(elemTaskId) {
        document.querySelectorAll(`[data-taskid="${elemTaskId}"]`).forEach((elem) => {
            if (elem.classList.contains("Procurement-list")) {
                document.querySelector(".Procurement-list-wrap.active")?.classList.remove("active");
                elem.closest(`.Procurement-list-wrap`)?.classList.add("active");
            }

            if (elem.classList.contains("horizontal-steps")) {
                document.querySelectorAll(".horizontal-steps").forEach((step) => {
                    step.closest("li")?.classList.remove("Active");
                });

                elem.closest(`li`)?.classList.add("Active");
                //elem.scrollIntoView();
            }
        });
    }


    openRightSideCards(taskname, keyvalue) {
        ShowDimmer(true);
        /* Variables from mainpage */


        files.js.push("/../ThirdParty/lodash.min.js?v=1");
        files.js.push("/../ThirdParty/deepdash.min.js");

        files.js.push("/../Js/handlebars.min.js?v=2");
        files.js.push("/../Js/handleBarsHelpers.min.js");

        files.js.push("/../ThirdParty/Highcharts/highcharts-3d.js");
        files.js.push("/../ThirdParty/Highcharts/highcharts-more.js");
        files.js.push("/../ThirdParty/Highcharts/highcharts.js");
        files.js.push("/../ThirdParty/Highcharts/highcharts-exporting.js");
        files.js.push("/../Js/high-charts-functions.min.js?v=20");
        files.js.push("/../Js/AxInterface.min.js?v=16");

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

        //files.js.push(`/HTMLPages/js/axpertFlutterCustomDashboard.js?v=2`);

        if (document.getElementsByTagName("body")[0].classList.contains("btextDir-rtl")) {
            cardsDashboardObj.dirLeft = false;
        }



        loadAndCall({
            files: files,
            callBack: () => {

                $(function () {
                    //deepdash(_);

                    $.ajax({
                        url: "../aspx/AxPEG.aspx/AxGetCardsData",
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
                                    var rightclassList = document.querySelector("#PROFLOW_Right").classList;
                                    rightclassList.remove('col-md-9', 'col-xl-9');
                                    rightclassList.add('col-md-7', 'col-xl-7');
                                }
                                else {
                                    document.querySelector("#PROFLOW_Right_Last").classList.add("d-none");
                                    var rightclassList = document.querySelector("#PROFLOW_Right").classList;
                                    rightclassList.add('col-md-9', 'col-xl-9');
                                    rightclassList.remove('col-md-7', 'col-xl-7');
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

    isEditableTask(taskName, keyValue) {
        var isEditable = false;
        let _this = this;
        let url = "../aspx/AxPEG.aspx/AxGetEditableTask";
        let data = { processName: this.processName, taskName: taskName, keyValue: keyValue, indexNo: this.currentIndex };
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                if (dataResult?.length > 0) {
                    let rowData = dataResult[0];
                    if (rowData?.editable == 'T')
                        isEditable = true
                }
            }
        });
        return isEditable;
    }

    openTstruct(taskName, transId, keyField, keyValue, recordId) {
        ShowDimmer(true);
        let isProcess = (this.userType == "APPROVER" ? "&fromprocess=true" : "");
        let isPegEdit = '';
        if (!this.inValid(this.keyValue) && this.keyValue != "NA" && (document.querySelector(`[data-tasktype="Make"][data-transid="${transId}"][data-recordid="0"]`) == null || this.currentElem?.classList.contains('completed'))) {
            var isEditable = this.isEditableTask(taskName, this.keyValue);
            isPegEdit = `&ispegedit=${isEditable.toString()}`;

            if (isEditable === false) {
                this.isTaskEditable = false;
            }
            else {
                this.isTaskEditable = true;
            }
        }

        let url = `../aspx/tstruct.aspx?transid=${transId}${isProcess}${isPegEdit}`;
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
        //ShowDimmer(false);
    };

    openMessageLink(pageType, pageParams) {
        ShowDimmer(true);
        if (pageType == '') {
            showAlertDialog("error", 'Page name should not be empty.');
            return false;
        }
        let url = '';
        pageParams = pageParams.replace("^", "&");
        if (pageType.startsWith('i')) {
            pageType = pageType.substring(1);
            url = `../aspx/ivtoivload.aspx?ivname=${pageType}&${pageParams}`;
        } else if (pageType.startsWith('t')) {
            pageType = pageType.substring(1);
            url = `../aspx/tstruct.aspx?transid=${pageType}&${pageParams}`;
        } else if (pageType.startsWith('c')) {
            //pageType = pageType.substring(2);
            url = `../aspx/${pageParams}`;
        }
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.add("d-none");
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.classList.remove("d-none");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
        if (pageType.startsWith('c'))
            ShowDimmer(false);
    };

    openMessageLinkCachedSave(pageType, pageParams, taskId) {
        ShowDimmer(true);
        if (pageType == '') {
            showAlertDialog("error", 'Page name should not be empty.');
            return false;
        }
        let url = '';
        pageParams = pageParams.replace("^", "&");
        if (pageType.startsWith('i')) {
            pageType = pageType.substring(1);
            url = `../aspx/iview.aspx?ivname=${pageType}&${pageParams}`;
        } else if (pageType.startsWith('t')) {
            pageType = pageType.substring(1);
            callParentNew("AxNotifyMsgId=", taskId);
            url = `../aspx/tstruct.aspx?transid=${pageType}&${pageParams}`;
        } else if (pageType.startsWith('c')) {
            //pageType = pageType.substring(2);
            url = `../aspx/${pageParams}`;
        }
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.add("d-none");
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.classList.remove("d-none");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
        if (pageType.startsWith('c'))
            ShowDimmer(false);
    };

    newTstruct(transId, taskName) {
        ShowDimmer(true);

        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
        let isProcess = (this.userType == "APPROVER" ? "fromprocess=true&" : "");
        let url = `../aspx/tstruct.aspx?${isProcess}transid=${transId}&act=open`;
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.classList.remove("d-none");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
        this.keyValue = "NA";
        if (this.horizontalFlow)
            this.showProgress();
        else
            this.showVerticalProgress();
        axProcessObj.openRightSideCards(taskName, "NA");
        ShowDimmer(false);
    };

    openBulkApprove() {
        let _this = this;
        try {
            let _this = this, data = {}, url = "";
            url = "../aspx/AxPEG.aspx/AxGetBulkApprovalCount";
            data = {};
            this.callAPI(url, data, true, result => {
                if (result.success) {
                    let json = JSON.parse(result.response);
                    json = JSON.parse(json.d);
                    let allTasks = '';
                    if (json.result.data.length > 0) {
                        let allTasks = '';
                        json = json.result.data;
                        json.forEach((item) => {
                            allTasks += `<div class="d-flex custom-menu-item px-4 py-2 border-1 border-bottom">
<div class="symbol symbol-45px">
                        <span class="symbol-label">
                            <img alt="{{processname}}" class=" w-30px" src="../images/homepageicon/{{processname}}.png"
                                onerror="this.onerror=null;this.src='../images/homepageicon/default.png';">

                        </span></div>
            <div class="d-flex align-items-center " onclick="axProcessObj.openBulkApprovePopup('${item.processname}')">
                        <span class="custom-menu-link px-3">
                            <span class="fw-normal fs-5 menu-task-text">${item.processname} (${item.pendingapprovals})</span>
                        </span></div>
                        </div>`;
                        });

                        if (document.getElementsByTagName("pd-allTasksNew") && document.getElementsByTagName("pd-allTasksNew").length > 0)
                            document.getElementsByTagName("pd-allTasksNew")[0].outerHTML = allTasks;
                    }
                    else {
                        showAlertDialog("warning", "No records available for approvals.");
                        var menuElement = document.querySelector('[data-kt-menu="true"][data-id="pd_all_tasksNew"]');
                        if (menuElement) {
                            menuElement.classList.remove("show");
                        }

                    }
                }
            });


        } catch (error) {
            showAlertDialog("error", error.message);
        }
    }

    openBulkApprovePopup(processName) {
        let _this = this;
        document.querySelector("body").click();
        this.processName = processName;
        let modalObj = {
            "id": `ldbApprove`
        };
        modalObj.iFrameModalBody = `
            <div class="card-body d-flex  row">
                <div class="w-100" id="Bulk-SelectALL-wrap">
                    <div class="form-check" style="position:absolute">
                      <input class="form-check-input task-list-checkbox" type="checkbox" id="select-all-checkbox">
                      <label class="form-check-label" for="select-all-checkbox">
                        Select All
                      </label>
                    </div>
                    <h4 style="margin-left: 295px; font-weight:bold">Bulk Approvals</h4>
                </div>
                <div class="w-100" id="BulkActiveList_Container">
                </div>
                <div class="w-100 mt-3">
                    <div class="approval-controls" data-tasktype="Approve">
                        <label class="form-label col-form-label mandatory">Bulk Approval Comments</label>
                        <div class="input-group">
                            <textarea data-tasktype="Approve" class="form-control"
                                data-tasktype="Approve" id="BULKAPPROVEComments"></textarea>
                        </div>
                    </div>
                </div>
                <div>
                    <button  style="float:right " class="btn btn-primary btn-sm mt-2" type="button" onclick="axProcessObj.doBulkAction('BULKAPPROVE','APPROVE');">Bulk Approve</button>
                </div>
            </div>`;
        modalObj.size = "lg";
        modalObj.opening = _this.bulkInit;

        try {
            let myModal = new BSModal(`modal_${modalObj.id}`, "", modalObj.iFrameModalBody,
                (opening) => { _this.bulkInit(); ShowDimmer(false); }, (closing) => { }
            );

            myModal.changeSize(modalObj.size || "fullscreen");
            myModal.hideHeader();
            myModal.hideFooter();
            myModal.showFloatingClose();
        } catch (error) {
            showAlertDialog("error", error.message);
        }
    }

    fetchBulkTasks(name) {
        let _this = this;
        let url = "../aspx/AxPEG.aspx/AxGetBulkActiveTasks";
        let data = { processName: _this.processName, taskType: "Approve" };

        this.callAPI(url, data, true, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.dataSources[name] = dataResult;
                this.showBulkTasks();
                ShowDimmer(false);
            } else {
                ShowDimmer(false);
            }
        });
    }

    bulkInit() {
        this.fetchBulkTasks("BulkTasks");
        const selectAllCheckbox = document.getElementById('select-all-checkbox');
        selectAllCheckbox?.addEventListener('change', function () {
            const checkboxes = document.querySelectorAll('.task-list-checkbox');
            checkboxes.forEach(function (checkbox) {
                checkbox.checked = selectAllCheckbox.checked;
            });
        });
    }

    showBulkTasks() {
        document.querySelector(`#BulkActiveList_Container`).innerHTML = "";

        if (this.dataSources["BulkTasks"]?.length == undefined || this.dataSources["BulkTasks"].length == 0) {
            if (this.isUndefined(parent.axProcessObj)) {
                document.querySelector(`#BulkActiveList_Container`).insertAdjacentHTML("beforeend", ` ${this.noRecordsRowHtml} `);
            }
            else {
                document.querySelector(`#BulkActiveList_Container`).insertAdjacentHTML("beforeend", ` ${this.processedRowHtml} `);
            }
        }
        else {
            this.dataSources["BulkTasks"].forEach((rowData, idx) => {
                var htmlText = Handlebars.compile(this.bulkTaskRowHtml)(rowData);
                document.querySelector(`#BulkActiveList_Container`).insertAdjacentHTML("beforeend", ` ${htmlText} `);
            });
        }
    }

    doBulkAction(action, taskType) {

        let _this = this;
        ShowDimmer(true);
        let url = "../aspx/AxPEG.aspx/AxDoBulkAction";

        let taskReason = "";
        let taskText = document.querySelector(`#${action}Comments`).value;

        var checkboxes = document.querySelectorAll('.task-list-checkbox:checked');
        if (checkboxes.length == 0) {
            showAlertDialog("Error", "Select atleast one task for approval.");
            ShowDimmer(false);
            return false;
        }

        if (taskText == '') {
            showAlertDialog("Error", "Bulk Approval Comments cannot be left empty.");
            ShowDimmer(false);
            return false;
        }

        let taskIds = [];
        checkboxes.forEach((item) => {
            taskIds.push(item.dataset.taskid);
        })
        taskIds = taskIds.join(",");

        let data = { action: action, taskId: taskIds, taskType: taskType, statusText: taskText, processName: this.processName };
        this.callAPI(url, data, true, result => {
            ShowDimmer(false);
            if (result.success) {
                let json = JSON.parse(result.response);
                if (!_this.isAxpertFlutter) {
                    json = JSON.parse(json.d);
                }
                if (json.result.success) {
                    _this.showSuccess(json.result.message);
                    if (!_this.isUndefined(parent.axProcessObj))
                        parent.window.location.reload();
                    else {
                        setTimeout(function () {

                            axProcessObj.refreshPage();
                        }, 1000)
                    }
                }
                else {
                    _this.catchError(json.result.message);
                }
            }
        });
    }


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
        this.horizontal = urlParams.get('horizontal') || 'T';

        //document.querySelector('#process-name span').innerText = this.processName;
        _autoLoadNextTask = (urlParams.get('autoloadnexttask') == 'F' ? false : _autoLoadNextTask);
    };

    openProcessTask(taskName, taskType, taskid) {
        ShowDimmer(true);
        //let url = `../aspx/htmlPages.aspx?loadcaption=Active Lists&processname=${this.processName}&keyfield=${this.keyField}&keyvalue=${this.keyValue}&taskname=${taskName}&tasktype=${taskType}&taskid=${taskid}`;
        let url = `../aspx/processflow.aspx?loadcaption=Active Lists&processname=${this.processName}&keyfield=${this.keyField}&keyvalue=${this.keyValue}&taskname=${taskName}&tasktype=${taskType}&taskid=${taskid}`;
        document.querySelector("#process_centerpanel").classList.add("d-none");
        document.querySelector("#horizontal-processbar").classList.remove("d-none");
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.classList.remove("d-none");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
        //ShowDimmer(false);
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
            //document.getElementsByTagName("toolbarDrawer")[0].replaceWith(headerExtras);
            headerExtras.innerHTML = this.toolbarDrawerHTML;

            if (this.userType == "APPROVER")
                document.querySelector("#kt_drawer_bulkApprove_button")?.classList.remove("d-none");

            //KTDrawer.init();
            KTMenu.init();

            if (this.horizontal == "F" || (typeof _horizontalFlow != "undefined" && !_horizontalFlow)) {
                this.horizontalFlow = false;
            }

            if (this.add == "F") {
                document.querySelector("#pd_ann").classList.add("d-none")
            }

            if (this.leftPanel == "F") {
                document.querySelector("#PROFLOW_Left").classList.add("d-none")
                document.querySelector("#PROFLOW_Right")?.classList.add("right-only")
            }

            if (this.horizontalFlow) {
                if (this.leftPanel != "F")
                    this.fetchProcessList("ProcessList");
                //this.showProgress();
            }
            else {
                document.querySelector("#PROFLOW_Left").classList.remove("d-none");
                document.querySelector("#PROFLOW_Left .card-header").classList.add("d-none");
                document.querySelector("#PROFLOW_Left #PROFLOW-profile-container").classList.remove("d-none");
                document.querySelector("#PROFLOW_Left #plistAccordion").classList.add("d-none");
                document.querySelector("#PROFLOW_Right")?.classList.remove("right-only")
                this.showVerticalProgress();
            }

            if (this.rightPanel == "F") {
                document.querySelector("#PROFLOW_Right_Last")?.classList.add("d-none");
                var rightclassList = document.querySelector("#PROFLOW_Right").classList;
                rightclassList.remove('col-md-7', 'col-xl-7');
                rightclassList.add('col-md-9', 'col-xl-9');
            }

            if (this.bulkApproval == "F") {
                document.querySelector("#kt_drawer_bulkApprove_button")?.classList.add("d-none")
            }

            if (this.showTree == "F") {
                document.querySelector("#proFlw_Tree_button")?.classList.add("d-none")
            }

            document.querySelector("#pd_all_tasksNew").addEventListener('click', () => {
                if (document.getElementsByTagName("pd-allTasksNew") && document.getElementsByTagName("pd-allTasksNew").length > 0)
                    axProcessObj.openBulkApprove();
            });

            document.querySelectorAll("#pd_timeline").forEach((item) => {
                KTMenu.getInstance(item).on("kt.menu.dropdown.show", function (item) {
                    axProcessObj.showTimeLine();
                });
            });

            //if (!this.inValid(this.target)) {
            //    ShowDimmer(true);
            //    this.showProgress();
            //}

            // changes related to activelist
            myDiv.addEventListener('scroll', this.scrollListener);
            this.toggleList()
            this.toggleCheckbox()
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

    // changes related to activelist
    formatEventDate(eventDateTime) {
        const eventDate = new Date(eventDateTime.replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3"));
        const today = new Date();
        const todayDateOnly = new Date(today.getFullYear(), today.getMonth(), today.getDate());

        const eventDateOnly = new Date(eventDate.getFullYear(), eventDate.getMonth(), eventDate.getDate());

        const dayDifference = Math.floor((todayDateOnly - eventDateOnly) / (1000 * 60 * 60 * 24));

        const startOfThisWeek = new Date(todayDateOnly);
        startOfThisWeek.setDate(todayDateOnly.getDate() - todayDateOnly.getDay()); // Sunday of this week

        const startOfLastWeek = new Date(startOfThisWeek);
        startOfLastWeek.setDate(startOfThisWeek.getDate() - 7); // Sunday of last week

        const timeOnly = eventDate.toLocaleTimeString([], {
            hour: '2-digit',
            minute: '2-digit'
        });
        const dayName = eventDate.toLocaleDateString('en-US', {
            weekday: 'short'
        });
        const fullDate = eventDate.toLocaleDateString('en-US', {
            day: 'numeric',
            month: 'short',
            year: 'numeric'
        });
        const monthYear = `${eventDate.getMonth()}-${eventDate.getFullYear()}`;
        const currentMonthYear = `${today.getMonth()}-${today.getFullYear()}`;

        if (dayDifference === 0) {
            return timeOnly;
        } else if (dayDifference === 1) {
            return `${dayName} ${timeOnly}`;
        } else if (eventDateOnly >= startOfThisWeek && dayDifference <= 7) {
            return `${dayName} ${timeOnly}`;
        } else if (eventDateOnly >= startOfLastWeek && eventDateOnly < startOfThisWeek) {
            return `${dayName} ${timeOnly}`;
        } else if (monthYear === currentMonthYear) {
            return `${eventDate.toLocaleDateString('en-US', { day: 'numeric', month: 'short', year: 'numeric' })}`;
        } else if (
            eventDate.getMonth() === (today.getMonth() - 1 + 12) % 12 && // Handles year boundary
            eventDate.getFullYear() === (today.getMonth() === 0 ? today.getFullYear() - 1 : today.getFullYear())
        ) {
            return `${eventDate.toLocaleDateString('en-US', { day: 'numeric', month: 'short' , year: 'numeric'})}`;
        } else {
            return fullDate;
        }
    }


    getTimeframe(dateString) {
        const today = new Date();

        const [day, month, year] = dateString.split('/'); // Extract the day, month, and year
        const targetDate = new Date(year, month - 1, day); // Use month - 1 as months are zero-indexed

        today.setHours(0, 0, 0, 0);
        targetDate.setHours(0, 0, 0, 0);

        const diffTime = today.getTime() - targetDate.getTime();
        const diffDays = diffTime / (1000 * 60 * 60 * 24);

        if (diffDays === 0) return "Today";
        if (diffDays === 1) return "Yesterday";

        const dayOfWeek = today.getDay();
        const startOfThisWeek = new Date(today);
        startOfThisWeek.setDate(today.getDate() - dayOfWeek);

        const startOfLastWeek = new Date(startOfThisWeek);
        startOfLastWeek.setDate(startOfLastWeek.getDate() - 7);

        const endOfLastWeek = new Date(startOfLastWeek);
        endOfLastWeek.setDate(endOfLastWeek.getDate() + 6);

        if (targetDate >= startOfThisWeek) return "This Week";
        if (targetDate >= startOfLastWeek && targetDate <= endOfLastWeek) return "Last Week";

        const thisMonth = today.getMonth();
        const thisYear = today.getFullYear();
        const startOfThisMonth = new Date(thisYear, thisMonth, 1);
        const startOfLastMonth = new Date(thisYear, thisMonth - 1, 1);
        const endOfLastMonth = new Date(thisYear, thisMonth, 0);

        if (targetDate >= startOfThisMonth) return "This Month";
        if (targetDate >= startOfLastMonth && targetDate <= endOfLastMonth) return "Last Month";

        return "Previous records";
    }


    scrollListener(event) {
        const myDiv = event.target;
        const currentScrollTop = myDiv.scrollTop;
        const currentScrollLeft = myDiv.scrollLeft;

        if (this.lastScrollTop !== currentScrollTop) {
            this.lastScrollTop = currentScrollTop;

            if (!this.isFetching && axProcessObj.isScrollAtBottomWithinDiv(myDiv)) {
                this.isFetching = true;
                axProcessObj.pageNo++;

                axProcessObj.fetchProcessList("task", "scroll");

                setTimeout(() => {
                    this.isFetching = false;
                }, 500);
            }
        }

        this.lastScrollLeft = currentScrollLeft;


    }


    isScrollAtBottomWithinDiv(divElement) {
        const distanceToBottom = divElement.scrollHeight - (divElement.scrollTop + divElement.clientHeight);
        return distanceToBottom <= 1;
    }

    showProcessList() {
        const dataSource = axProcessObj.tasksJson

        try {

            if (dataSource.length > 0) {
                const groupedData = axProcessObj.tasksJson.reduce((acc, item) => {
                    const timeframe = axProcessObj.getTimeframe(item.eventdatetime.split(" ")[0]);
                    if (!acc[timeframe]) {
                        acc[timeframe] = [];
                    }
                    acc[timeframe].push(item);
                    return acc;
                }, {});

                if (this.isDropdownClick) {
                    const container = document.getElementById('plistContent')
                    container.innerHTML = ""
                }
                Object.keys(groupedData).forEach((timeframe) => {
                    const existingAccordion = document.getElementById(`section-${timeframe.replace(/\s+/g, '')}`);
                    if (existingAccordion) {
                        const accordionBody = existingAccordion.querySelector(".accordion-body");
                        accordionBody.insertAdjacentHTML(
                            "beforeend",
                            groupedData[timeframe].map((item) => this.generateTaskHTML(item)).join("")
                        );

                    } else {
                        this.createAccordionForTimeframe(timeframe, groupedData[timeframe], existingAccordion);

                    }
                });


                document.querySelectorAll('.tasktitle', '#nextrecord', '#previousrecord').forEach(item => {
                    item.addEventListener('click', function () {
                        document.querySelectorAll('.tasktitle').forEach(row => {
                            row.classList.remove('taskRowClicked')
                            // const rowElement = row.closest('.listrow');
                            // if (rowElement) {
                            //     rowElement.style.backgroundColor = ''; // Reset background color
                            // }
                        });

                        this.classList.add('taskRowClicked');

                        // const listRowElement = this.closest('.listrow');
                        // if (listRowElement) {
                        //     listRowElement.style.backgroundColor = 'rgb(239 239 236)';
                        // }

                    })
                });
                if (!this.isInitialized) {
                    //  const listRow = document.querySelector('.listrow[data-index="1"]');
                    //  if (listRow) {
                    //      const taskTitle = listRow.querySelector('.tasktitle');
                    //      if (taskTitle) {
                    //          taskTitle.click();
                    //      }
                    //     }

                    document.getElementById("nextrecord").addEventListener("click", () => {
                        this.navigateToRecord("next");

                    });

                    document.getElementById("previousrecord").addEventListener("click", () => {
                        this.navigateToRecord("previous");
                    });

                    document.querySelectorAll('#alllist, #activelist, #completedlist').forEach((button) => {
                        button.addEventListener('click', function () {
                            

                            // Remove the 'active' class from all buttons
                            document.querySelectorAll('#alllist, #activelist, #completedlist').forEach((btn) => {
                                btn.classList.remove('active');
                            });

                            // Add the 'active' class to the clicked button
                            this.classList.add('active');

                            // Update `axProcessObj` properties based on the clicked button
                            axProcessObj.isDropdownClick = true;
                            axProcessObj.filterkey = this.getAttribute("data-id"); // Extract 'all', 'active', 'completed'
                            axProcessObj.pageNo = 1;
                            axProcessObj.fetchProcessList();
                        });
                    });


                    document.querySelector('.selectbtn[data-kt-menu-attach="parent"]').addEventListener('click', function (e) {
                        e.preventDefault();
                        
                        const button = e.currentTarget;
                        button.classList.toggle('toggleclicked');

                        // Get the checkboxes with class names maincheckbox and task-checkbox
                        const mainCheckbox = document.querySelector('.maincheckbox');
                        const taskCheckboxes = document.querySelectorAll('.task-checkbox');


                        document.querySelectorAll('.checkbox-wrapper').forEach(wrapper => {
                            const userElement = wrapper.querySelector('.user');
                            const checkbox = wrapper.querySelector('.task-checkbox');
                            checkbox.classList.toggle('toggleclicked');

                            if (button.classList.contains('toggleclicked')) {
                                userElement.style.display = 'none';

                                checkbox.style.visibility = 'visible';
                                mainCheckbox.style.display = 'block';

                            } else {
                                userElement.style.display = 'flex';
                                checkbox.style.visibility = 'hidden';
                                mainCheckbox.style.display = 'none';
                            }

                        });
                    });

                    // Add keyup and keydown event listeners
                    document.addEventListener("keydown", (e) => {
                        
                        if (e.key === "ArrowDown") {
                            axProcessObj.navigateToRecordWithKeyboard("next");
                        } else if (e.key === "ArrowUp") {
                            axProcessObj.navigateToRecordWithKeyboard("previous");
                        }
                    });

                    document.querySelectorAll(".listrow").forEach((row) => {
                        row.addEventListener("click", function () {
                            
                            // Remove 'selected' class from all list rows
                            document.querySelectorAll(".listrow").forEach((r) => {
                                r.style.outline = "none"
                            });


                        });
                    });

                    document.querySelector('.accordion-button').addEventListener('click', (event) => {
                        const accordionButton = event.target;

                        // Check if the button is not collapsed
                        if (accordionButton.classList.contains('collapsed')) {
                            const bodyContainer = document.getElementById('body_Container');
                            const plistContainer = document.getElementById('plistContent');

                            if (bodyContainer && plistContainer) {
                                const bodyHeight = bodyContainer.offsetHeight;
                                const plistHeight = plistContainer.offsetHeight;
                                const lastChild = plistContainer.children[plistContainer.children.length - 1];

                                if (bodyHeight > plistHeight && lastChild && !lastChild.classList.contains('no-more-records')) {
                                    // Enable scrollbar by adjusting the height
                                    plistContainer.style.height = `${bodyHeight}px`;
                                    bodyContainer.style.overflowY = 'auto'; // Ensure vertical scrollbar is enabled
                                } else {
                                    // Reset overflow if the condition is not met
                                    //bodyContainer.style.overflowY = 'hidden';
                                }
                            }
                        }
                    });



                    this.initializeTooltips();
                    this.initializeDatePickers();
                    this.setupSelectAllTasks();
                    this.isInitialized = true;

                }
            } else {
                document.querySelector("#PROFLOW_Left").classList.add("d-none");
                document.querySelector("#PROFLOW_Right").classList.add("right-only");
            }

            if (!this.inValid(this.taskId) && !this.taskCompleted) {
                this.setActiveInList(this.taskId);
            }

        } catch (error) {
            this.catchError(error.message);
        }
        this.isDropdownClick = false
    }

    openFilters() {
        
        $('#filterModal').modal('show');
        document.getElementById('filterGroupName').disabled = true;

        if ($('#dvModalFilter').html() === "") {
            axProcessObj.createFilterLayout();
        }
    }
    createFilterLayout() {
        $('#dvModalFilter').html("");


        $.each(axProcessObj._entity.metaData, function (index, field) {
            if (field.hide === 'T') {
                return true;
            }

            axProcessObj.generateFilterHTML(field);

            if (axProcessObj._entity.filterObj[field.fldname]) {
                axProcessObj.updateFilterLayout(field.fldname, axProcessObj._entity.filterObj[field.fldname]);
            }
        });

        // Initialize dropdown fields
        document.querySelectorAll("#dvModalFilter .filter-fld[data-type=DropDown]").forEach(fld => {
            let fldId = fld.id;
            let dataArray = [...new Set(axProcessObj._entity.fldData[fldId])];

            // Add a default option
            $(fld).append($("<option></option>").val('0').html('--Select--'));

            // Add unique values to the dropdown
            dataArray.forEach(item => {
                if (!axProcessObj._entity.inValid(item))
                    fld.insertAdjacentHTML("beforeend", `<option value="${item}">${item}</option>`);
            });

            // Initialize Select2 for the dropdown
            $(fld).select2({
                multiple: true
            }).on("select2:unselect select2:select", function (e) {
                let fldNamesf = $(this).attr("id");
                let fldAcValue = $(this).val();
                fldAcValue = fldAcValue.filter(num => num !== '0');
                $(this).val(fldAcValue);
                $(this).trigger("change");
            });
        });

        // Initialize date pickers
        var glCulture = eval(callParent('glCulture'));
        var dtFormat = "d/m/Y";
        if (glCulture == "en-us")
            dtFormat = "m/d/Y";
        $(".flatpickr-input").flatpickr({
            dateFormat: dtFormat
        });
    }
    convertDateFormat(dateStr) {
        const parts = dateStr.split('/'); // Split the date by '/'
        return `${parts[2]}-${parts[1]}-${parts[0]}`; // Return in yyyy-MM-dd format
    }

    filterModelClose() {
        $('#filterGroupName').val('')
        $('#filterGroupModalWrapper').modal('hide');

        document.getElementById('filterGroupCheckbox').checked = false
        document.getElementById('filterGroupName').disabled = true

        $('#dvModalFilter').html("");
        $('#filterModal').modal('hide');

    }

    generateFilterHTML(field) {
        var fldtype = axProcessObj.getFieldDataType(field);
        var fldcap = field.fldcap || '';
        var fldname = field.fldname;
        let filterHTML = '';
        if (fldtype.toUpperCase() == "BUTTON" || fldtype.toUpperCase() == "ATTACHMENTS")
            return;

        if (field.fdatatype == "n")
            fldtype = "Numeric";

        switch (fldtype) {
            case 'DropDown':
                filterHTML = `<div class="row" data-type="${fldtype}"><div class="col-md-3 fldCaption"><p class="form-group ">${fldcap}</p> </div>
                              <div class="col-md-9 fldCaption">
                                <select class="form-control filter-fld"  data-type="${fldtype}" id="${fldname}" name="${fldtype}">
                                <option value="All">All</option>
                                </select>
                              </div>`;

                break;
            case 'Numeric':
                filterHTML = `<div class="row filter-fld" data-type="${fldtype}" id="${fldname}" data-type="${fldtype}">
                <div class="col-md-3 fldCaption">
                <p class="form-group ">${fldcap}</p>
                </div> 
                <div class="col-md-9">
                <div class="form-group form-row fldCaption">
                <div class="col-md-6 col">
                <label>From</label>           
                <input type="number" id="${fldname}_from" class="form-control" data-type="${fldtype}"/>
                </div>
                <div class="col-md-6 col">
                <label>To</label>           
                <input type="number" id="${fldname}_to" class="form-control" data-type="${fldtype}"/>
                </div></div></div>`;
                break;
            case 'Date':
                var dateOptions = ["Custom", "Today", "Yesterday", "Tomorrow", "This week", "Last week", "Next week", "This month", "Last month", "Next month", "This quarter", "Last quarter", "Next quarter", "This year", "Last year", "Next year"];
                var dateOptionsId = ["customOption", "todayOption", "yesterdayOption", "tomorrowOption", "this_weekOption", "last_weekOption", "next_weekOption", "this_monthOption", "last_monthOption", "next_monthOption", "this_quarterOption", "last_quarterOption", "next_quarterOption", "this_yearOption", "last_yearOption", "next_yearOption"];

                filterHTML += `<div class="row filter-fld" data-type="${fldtype}" id="${fldname}"><div class="col-md-3 fldCaption"><p class="form-group ">${fldcap}</p></div>
                                <div class="col-md-4 fldCaption">
                                <select class="form-select dateFilter" type="text" id="${fldname}_dateoption" name="${fldname}" data-field="${fldname}" onchange="axProcessObj.generateAdvFilterDates('${fldname}');">`;
                for (var i = 0; i < dateOptions.length; i++) {
                    filterHTML += `<option value=${dateOptionsId[i]}>${dateOptions[i]}</option>`;
                }
                filterHTML += `</select> 
                                </div>
                                <div class="col-md-5">
    <div class="form-group form-row fldCaption">
    <div class="col-md-6 col">
    <label>From</label>           
     <input id="${fldname}_from" name="${fldname}_from" value="" maxlength="10" type="date" class="form-control flatpickr-input" data-input="" onchange="axProcessObj.validateDateRange('${fldname}');">
    </div>
    <div class="col-md-6 col">
    <label>To</label>           
    <input id="${fldname}_to" name="${fldname}_to" value="" maxlength="10" type="date" class="form-control flatpickr-input" data-input="" onchange="axProcessObj.validateDateRange('${fldname}');">
    </div></div></div>   
                                </div>`;
                break;
            default:
                filterHTML = `<div class="row" data-type="${fldtype}">
                <div class="col-md-3 fldCaption">
                <p class="form-group ">${fldcap}</p> 
                </div>
                <div class="col-md-4 fldValue"> 
                <select class="form-select" type="text" id="${fldname}_searchoption" class="form-control">
                <option value="STARTSWITH">Starts with</option>
                <option value="CONTAINS">Contains</option>
                <option value="ENDSWITH">Ends with</option>
                </select></div>
                <div class="col-md-5 fldValue"> <input type="text" id="${fldname}" class="form-control filter-fld" data-type="${fldtype}"/></div>
                </div>`;
                break;
        }

        $('#dvModalFilter').append(filterHTML);
    }

    getDatesBasedonSelection(selectionvalue) {

        var fromToObj = {
            from: "",
            to: ""
        };
        var advFilterDtCulture = dtCulture == "en-us" ? "MM/DD/YYYY" : "DD/MM/YYYY";
        switch (selectionvalue) {
            case "customOption":
                break;
            case "todayOption":
                var dateObj = new Date();
                fromToObj.from = fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "yesterdayOption":
                var dateObj = new Date();
                dateObj.setDate(dateObj.getDate() - 1);
                fromToObj.from = fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "tomorrowOption":
                var dateObj = new Date();
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.from = fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_weekOption":
                var dateObj = getFirstDayOfWeek(new Date());
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 6);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_weekOption":
                var dateObj = getFirstDayOfWeek(new Date());
                dateObj.setDate(dateObj.getDate() - 7)
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 6);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_weekOption":
                var dateObj = getFirstDayOfWeek(new Date());
                dateObj.setDate(dateObj.getDate() + 7)
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 6);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_monthOption":
                var dateObj = getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_monthOption":
                var dateObj = getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                dateObj.setMonth(dateObj.getMonth() - 1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_monthOption":
                var dateObj = getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                dateObj.setMonth(dateObj.getMonth() + 1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_quarterOption":
                var dateObj = new Date();
                var thisQuarter = Math.floor(((dateObj).getMonth() + 3) / 3);
                dateObj.setDate(1);
                dateObj.setMonth((thisQuarter * 3) - 3);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 3);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_quarterOption":
                var dateObj = new Date();
                var thisQuarter = Math.floor(((dateObj).getMonth() + 3) / 3) - 1;
                if (thisQuarter == 0) {
                    thisQuarter = 4;
                    dateObj.setFullYear(dateObj.getFullYear() - 1);
                }
                dateObj.setDate(1);
                dateObj.setMonth((thisQuarter * 3) - 3);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 3);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_quarterOption":
                var dateObj = new Date();
                var thisQuarter = Math.floor(((dateObj).getMonth() + 3) / 3) + 1;
                if (thisQuarter == 5) {
                    thisQuarter = 1;
                    dateObj.setFullYear(dateObj.getFullYear() + 1);
                }
                dateObj.setDate(1);
                dateObj.setMonth((thisQuarter * 3) - 3);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 3);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_yearOption":
                var dateObj = new Date();
                dateObj.setDate(1);
                dateObj.setMonth(0);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setFullYear(dateObj.getFullYear() + 1);
                dateObj.setMonth(0);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_yearOption":
                var dateObj = new Date();
                dateObj.setFullYear(dateObj.getFullYear() - 1);
                dateObj.setDate(1);
                dateObj.setMonth(0);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setFullYear(dateObj.getFullYear() + 1);
                dateObj.setMonth(0);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_yearOption":
                var dateObj = new Date();
                dateObj.setFullYear(dateObj.getFullYear() + 1);
                dateObj.setDate(1);
                dateObj.setMonth(0);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setFullYear(dateObj.getFullYear() + 1);
                dateObj.setMonth(0);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
        }

        return fromToObj;
    }

    generateAdvFilterDates(dateFld) {

        var selectionvalue = document.querySelector(`#${dateFld}_dateoption`).value;
        var currentDate = new Date();
        var fromDate = document.querySelector(`#${dateFld}_from`);
        var toDate = document.querySelector(`#${dateFld}_to`);

        var fromToObj = axProcessObj.getDatesBasedonSelection(selectionvalue);

        fromDate.value = axProcessObj.convertDateFormat(fromToObj.from);
        toDate.value = axProcessObj.convertDateFormat(fromToObj.to);

        if (selectionvalue == "customOption") {
            fromDate.disabled = false;
            toDate.disabled = false;
            fromDate.classList.add('disabledDate');
            toDate.classList.add('disabledDate');
        } else {
            fromDate.disabled = true;
            toDate.disabled = true;
            fromDate.classList.remove('disabledDate');
            toDate.classList.remove('disabledDate');
        }

    }
    validateDateRange(fieldId) {
        var fromDateElement = document.getElementById(fieldId + '_from');
        var toDateElement = document.getElementById(fieldId + '_to');
        var fromDate = fromDateElement.value;
        var toDate = toDateElement.value;

        if (fromDate && toDate) {
            var fromDateObj = parseDate(fromDate);
            var toDateObj = parseDate(toDate);

            if (!fromDateObj || !toDateObj) {
                alert('Invalid date format.');
                return;
            }

            // Check if the "To" date is earlier than the "From" date
            if (toDateObj < fromDateObj) {
                alert('The "To" date cannot be earlier than the "From" date.');
                // Clear the "To" date
                toDateElement.value = '';
            }
        }
    }
    getFieldDataType(fldProps) {
        // Corrected: Calling _entity's inValid method properly
        if (axProcessObj._entity.inValid(fldProps.cdatatype)) {
            if (fldProps.fdatatype == "n") return "Number";
            else if (fldProps.fdatatype == "d") return "Date";
            else if (fldProps.fdatatype == "c") return "Text";
            else if (fldProps.fdatatype == "i") return "Image";
            else if (fldProps.fdatatype == "t") return "Large Text";
            else return "Text";
        } else {
            return fldProps.cdatatype;
        }
    }

    updateFilterLayout(fieldId, filterDetails) {
        filterChanged = true;

        $('#dvModalFilter').children().each(function () {
            let divDataType = $(this).attr('data-type');
            let selectElement = $(this).find('div select');
            let divEleId = selectElement.attr('id');
            let divDataField = selectElement.attr('data-field');

            let inputElement = $(this).find('div input');
            let inputEleId = inputElement.attr('id');

            // let selectedOptionValue = selectElement.val(); 

            // let inputValue = inputElement.val();

            for (const [key, value] of Object.entries(fieldId)) {
                if (divEleId === key || divDataField === key || inputEleId === key) {
                    const secondValue = value.split(',')[1];
                    const dropdownElement = $(`#${divEleId}`);

                    switch (divDataType) {
                        case "DropDown":
                            if (dropdownElement.prop('multiple')) {
                                const values = secondValue.split(';');
                                dropdownElement.val(values).trigger('change');
                            } else {
                                dropdownElement.val(secondValue).trigger('change');
                            }
                            break;

                        case "Simple Text":
                            inputElement.val(secondValue);
                            break;

                        case "Date":
                            if (divEleId === "modifiedon_dateoption") {
                                const fromDate = $('#modifiedon_from');
                                const toDate = $('#modifiedon_to');
                                const fromToObj = getDatesBasedonSelection(secondValue);

                                fromDate.val(fromToObj.from);
                                toDate.val(fromToObj.to);

                                dropdownElement.val(secondValue).trigger('change');

                                // Enable or disable date fields based on the selected option
                                if (secondValue === "customOption") {
                                    fromDate.prop('disabled', false).addClass('disabledDate');
                                    toDate.prop('disabled', false).addClass('disabledDate');
                                } else {
                                    fromDate.prop('disabled', true).removeClass('disabledDate');
                                    toDate.prop('disabled', true).removeClass('disabledDate');
                                }
                            }
                            break;

                        case "Numeric":
                        case "Auto Generate":
                            inputElement.val(secondValue);
                            break;

                        default:
                            break;
                    }
                    break;
                }
            }
        });

        // Handle pillText if present
        if (fieldId.pillText) {
            $('#filterGroupName').val(fieldId.pillText);
        }
    }


    navigateToRecordWithKeyboard(direction) {
        const taskTitles = Array.from(document.querySelectorAll(".listrow .tasktitle"));
        const listContainer = document.querySelector(".list-container");

        let activeIndex = taskTitles.findIndex((item) =>
            item.classList.contains("taskRowClicked")
        );

        if (activeIndex === -1 && taskTitles.length > 0) {
            // If no row is active, select the first row
            activeIndex = 0;
            taskTitles[activeIndex].classList.add("taskRowClicked");
            taskTitles[activeIndex].click();
            taskTitles[activeIndex].closest('.listrow').style.outline = "1px solid black"; // Add outline border
            // Scroll into view and adjust for header height
            setTimeout(() => {
                taskTitles[activeIndex].closest('.listrow').scrollIntoView({
                    behavior: "smooth",
                    block: "start"
                });
                const header = document.querySelector(".Page-Title-Bar");
                if (header) {
                    const headerHeight = header.offsetHeight || 0;
                    window.scrollBy(0, -headerHeight);
                }
            }, 0);
            return;
        }

        let targetIndex = direction === "next" ? activeIndex + 1 : activeIndex - 1;

        if (targetIndex >= 0 && targetIndex < taskTitles.length) {
            // Remove outline and class from the current active row
            taskTitles[activeIndex].classList.remove("taskRowClicked");
            taskTitles[activeIndex].closest('.listrow').style.outline = "none";

            // Add outline and class to the new target row
            taskTitles[targetIndex].classList.add("taskRowClicked");
            taskTitles[targetIndex].click();
            taskTitles[targetIndex].closest('.listrow').style.outline = "1px solid black";
            setTimeout(() => {
                taskTitles[targetIndex].closest('.listrow').scrollIntoView({
                    behavior: "smooth",
                    block: "start"
                });
                const header = document.querySelector(".Page-Title-Bar");
                if (header) {
                    const headerHeight = header.offsetHeight || 0;
                    window.scrollBy(0, -headerHeight);
                }
            }, 0);

            s
        } else {
            const message = direction === "next" ? "No more Next records." : "No more Previous records.";
            showAlertDialog("info", message);
        }
    }


    getRowIcon(rowdata) {
        let iconHtml = '';
        let tooltip = '';

        if (rowdata.rectype === 'PEG') {
            if (rowdata.taskstatus) {
                switch (rowdata.taskstatus.toUpperCase()) {
                    case 'RETURNED':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.taskstatus.charAt(0).toUpperCase() + rowdata.taskstatus.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" style="color:blueviolet" >reply</span>`;
                        break;
                    case 'WITHDRAWN':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.taskstatus.charAt(0).toUpperCase() + rowdata.taskstatus.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" style="color:red" >cancel</span>`;
                        break;
                    case 'APPROVED':
                        tooltip = `<span class="badge  badge-light-success fw-bold ">${rowdata.taskstatus.charAt(0).toUpperCase() + rowdata.taskstatus.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" style="color:#50cd89" >check_circle</span>`;
                        break;
                    case 'REJECTED':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.taskstatus.charAt(0).toUpperCase() + rowdata.taskstatus.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" style="color:red" >cancel</span>`;
                        break;
                    case 'CHECKED':
                        tooltip = `<span class="badge  badge-light-success fw-bold ">${rowdata.taskstatus.charAt(0).toUpperCase() + rowdata.taskstatus.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" style="color:#50cd89" >check_box</span>`;
                        break;
                    case 'MADE':
                        tooltip = '';
                        iconHtml = `<span class="material-icons" >done</span>`;
                        break;
                    case 'SKIPPED':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.taskstatus.charAt(0).toUpperCase() + rowdata.taskstatus.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons text-primary" >skip_next</span>`;
                        break;
                    case 'SENT':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.taskstatus.charAt(0).toUpperCase() + rowdata.taskstatus.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" >skip_next</span>`;
                        break;
                    default:
                        tooltip = '';
                        iconHtml = `<span class="material-icons" title="PEG">assignment</span>`;
                }
            }
             else if (rowdata.tasktype) {
                 switch (rowdata.tasktype.toUpperCase()) {
                     case 'APPROVE':
                         tooltip = ``;
                         iconHtml = `<span class="material-icons" >task</span>`;
                         break;
                     case 'CACHED SAVE':
                         tooltip = ``;
                         iconHtml = `<span class="material-icons" >save</span>`;
                         break;
                     case 'CHECK':
                         tooltip = ``;
                         iconHtml = `<span class="material-icons" >check</span>`;
                         break;
                     case 'MAKE':
                         tooltip = ``;
                         iconHtml = `<span class="material-icons" >construction</span>`;
                         break;
                     case 'EXPORT':
                         tooltip = ``;
                         iconHtml = `<span class="material-icons" >import_export</span>`;
                         break;
                     case 'REMINDERS':
                         tooltip = ``;
                         iconHtml = `<span class="material-icons" >notifications</span>`;
                         break;
                     default:
                         tooltip = '';
                         iconHtml = `<span class="material-icons" title="PEG">assignment</span>`;
                 }
             }
        } else if (rowdata.rectype === 'MSG') {
            if (rowdata.msgtype) {
                switch (rowdata.msgtype.toUpperCase()) {
                    case 'EXPORT EXCEL1':
                        tooltip = `<span class="badge  badge-light-primary fw-bold ">Export Excel</span>`;
                        iconHtml = `<span class="material-icons" >file_copy</span>`;
                        break;
                    case 'EXPORT PDF':
                        tooltip = `<span class="badge  badge-light-primary fw-bold ">${rowdata.msgtype.charAt(0).toUpperCase() + rowdata.msgtype.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" >picture_as_pdf</span>`;
                        break;
                    case 'EXPORT WORD':
                        tooltip = `<span class="badge  badge-light-primary fw-bold ">${rowdata.msgtype.charAt(0).toUpperCase() + rowdata.msgtype.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" >article</span>`;
                        break;
                    case 'PERIODIC NOTIFICATION':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.msgtype.charAt(0).toUpperCase() + rowdata.msgtype.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" >alarm</span>`;
                        break;
                    case 'FORM NOTIFICATION':
                        tooltip = '';
                        iconHtml = `<span class="material-icons" >notifications</span>`;
                        break;
                    case 'CCHED SAVE':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.msgtype.charAt(0).toUpperCase() + rowdata.msgtype.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" >save</span>`;
                        break;
                    case 'MESSAGE':
                        tooltip = `<span class="badge  badge-light-success fw-bold ">${rowdata.msgtype.charAt(0).toUpperCase() + rowdata.msgtype.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" >message</span>`;
                        break;
                    case 'REMINDERS':
                        tooltip = `<span class="badge  badge-light-danger fw-bold ">${rowdata.msgtype.charAt(0).toUpperCase() + rowdata.msgtype.slice(1)}</span>`;
                        iconHtml = `<span class="material-icons" >notifications</span>`;
                        break;
                    default:
                        tooltip = '';
                        iconHtml = `<span class="material-icons" title="Message">message</span>`;
                }
            }
        }

        return {
            iconHtml,
            tooltip
        };
    }




  
    toggleRowBackground(element) {
        const allRows = document.querySelectorAll('.listrow');

        const parentRow = element.closest('.listrow');

        allRows.forEach(row => row.classList.remove('bg-changed'));

        if (parentRow) {
            parentRow.classList.add('bg-changed');
        }
    }

    navigateToRecord(direction) {
        
        const taskTitles = Array.from(document.querySelectorAll(".listrow .tasktitle"));
        const activeIndex = taskTitles.findIndex((item) =>
            item.classList.contains("taskRowClicked")
        );

        if (activeIndex !== -1) {
            let targetIndex = direction === "next" ? activeIndex + 1 : activeIndex - 1;

            if (targetIndex >= 0 && targetIndex < taskTitles.length) {

                taskTitles[targetIndex].click();
            } else {
                const myDiv = document.getElementById("body_Container");

                if (direction === "next" && axProcessObj.isScrollAtBottomWithinDiv(myDiv)) {
                    if (!this.isFetching) {
                        this.isFetching = true;
                        axProcessObj.pageNo++;
                        axProcessObj.fetchProcessList("task", "scroll");

                        setTimeout(() => {
                            this.isFetching = false;
                            const updatedTaskTitles = Array.from(document.querySelectorAll(".listrow .tasktitle"));
                            if (updatedTaskTitles.length > taskTitles.length) {
                                const newTargetIndex = updatedTaskTitles.findIndex((_, index) => index === targetIndex);
                                if (newTargetIndex !== -1) {

                                    updatedTaskTitles[newTargetIndex].click();
                                } else {
                                    showAlertDialog("info", "No more Next records.");
                                }
                            } else {
                                showAlertDialog("info", "No more Next records.");
                            }
                        }, 500);
                    }
                } else {
                    const message = direction === "next" ? "No more Next records." : "No more Previous records.";
                    showAlertDialog("info", message);
                }
            }
        } else {
            showAlertDialog("info", "No currently selected task to navigate from.");
        }
    }


    toggleCheckbox() {
        document.querySelector('.maincheckbox').addEventListener('click', (e) => {
            const button = e.currentTarget;
            button.classList.toggle('selectclicked');


            document.querySelectorAll('.checkbox-wrapper').forEach(wrapper => {
                const userElement = wrapper.querySelector('.user');
                const checkbox = wrapper.querySelector('.task-checkbox');
                checkbox.classList.toggle('selectclicked');

                if (button.classList.contains('selectclicked')) {
                    checkbox.setAttribute('checked', true);
                    userElement.style.display = 'none';
                    checkbox.style.visibility = 'visible';
                    button.style.display = 'blockwindow.top';

                } else {
                    checkbox.removeAttribute('checked');
                    userElement.style.display = 'flex';
                    checkbox.style.visibility = 'hidden';
                    button.style.display = 'none';
                    document.querySelector('.selectbtn[data-kt-menu-attach="parent"]').classList.remove('toggleclicked');
                }
            });
        });




    }

    toggleList() {
        document.getElementById("collapseicon").addEventListener("click", function () {
            
            const leftDiv = document.getElementById("PROFLOW_Left");
            const rightDiv = document.getElementById("PROFLOW_Right");
            const nextRec = document.getElementById('nextrecord');
            const prevRec = document.getElementById('previousrecord')
            leftDiv.classList.toggle("collapsed");
            const mainCheckbox = document.querySelector('.maincheckbox');
            mainCheckbox.style.display = 'none';

            let filteredRows;
            const listRows = Array.from(document.querySelectorAll('.listrow'));
            // Filter rows where taskTitle exists and does not have the 'taskRowClicked' class
            filteredRows = listRows.filter((listRow) => {
                const taskTitle = listRow.querySelector('.tasktitle');
                return taskTitle && taskTitle.classList.contains('taskRowClicked');
            });
            if (leftDiv.classList.contains("collapsed")) {
                

                let row = null;


                // Find the row with data-index="1" in the filtered rows
                const targetRow = listRows.find((listRow) => listRow.getAttribute('data-index') === "1");

                if (targetRow && filteredRows.length == 0) {
                    row = targetRow.querySelector('.tasktitle');
                    if (row) {
                        console.log(`Triggering click for taskTitle inside listRow with data-index: 1`);
                        row.click();

                    }
                }

                leftDiv.classList.add("d-none")
                rightDiv.classList.remove("col-xl-8");
                rightDiv.classList.add("col-xl-12");

                this.setAttribute("data-bs-title", "Show record List");
                this.setAttribute("data-bs-toggle", "tooltip");
                this.setAttribute("data-bs-placement", "bottom");

                const tooltipInstance = bootstrap.Tooltip.getInstance(this);
                if (tooltipInstance) {
                    tooltipInstance.dispose();
                }
                new bootstrap.Tooltip(this); // Initialize the new tooltip instance

                nextRec.classList.remove("d-none")
                prevRec.classList.remove("d-none")
            } else {

                if (filteredRows.length == 1) {
                    const activeRow = filteredRows[0].querySelector('.tasktitle');
                    if (activeRow) {
                        console.log(`Scrolling to the taskTitle in the filtered row`);
                        setTimeout(() => {
                            activeRow.scrollIntoView({
                                behavior: "smooth",
                                block: "start"
                            });
                            const headerHeight = document.getElementsByClassName("Page-Title-Bar").offsetHeight;
                            window.scrollBy(10, -headerHeight); // Adjust to avoid overlap
                        }, 0);
                    }
                }

                leftDiv.classList.remove("d-none");
                rightDiv.classList.remove("col-xl-12");
                rightDiv.classList.add("col-xl-8");

                this.setAttribute("data-bs-title", "Hide record List");
                this.setAttribute("data-bs-toggle", "tooltip");
                this.setAttribute("data-bs-placement", "bottom");

                const tooltipInstance = bootstrap.Tooltip.getInstance(this);
                if (tooltipInstance) {
                    tooltipInstance.dispose();
                }
                new bootstrap.Tooltip(this);
                nextRec.classList.add("d-none")
                prevRec.classList.add("d-none")
            }



        });
    }


    initializeTooltips() {
     const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');

    tooltipTriggerList.forEach(tooltipTriggerEl => {
        if (!tooltipTriggerEl.getAttribute('data-bs-title')) {
            tooltipTriggerEl.removeAttribute('data-bs-title'); // Remove empty tooltips
        }

        let tooltipInstance = bootstrap.Tooltip.getInstance(tooltipTriggerEl);
        if (tooltipInstance) {
            tooltipInstance.dispose(); // Destroy old tooltip
        }

        new bootstrap.Tooltip(tooltipTriggerEl);
    });
	}


    initializeDatePickers() {
        $("#pdFilFrom, #pdFilTo").flatpickr({
            dateFormat: "d-M-Y",
            enableTime: false
        });
    }

    setupSelectAllTasks() {
        $(document).on("click", ".all-task", (event) => {
            const isChecked = event.currentTarget.checked;


            const menuPr = document.querySelector("#pd_all_tasks");
            const menuDd = KTMenu.getInstance(menuPr);


            menuDd.element.children.forEach((child, index) => {
                const menuTask = child.querySelector(".menu-task");
                if (menuTask) {
                    menuTask.checked = isChecked;
                }
            });


            $(".all-task-text").text(isChecked ? "Unselect All" : "Select All");
        });


        $(document).on("click", ".menu-task", () => {
            const totalCheckBoxes = document.querySelectorAll(".menu-task");
            const checkedCheckBoxes = document.querySelectorAll(".menu-task:checked");

            const allTasksCheckbox = document.querySelector(".all-task");
            const actionTextElement = $(".all-task-text");


            if (totalCheckBoxes.length === checkedCheckBoxes.length) {
                allTasksCheckbox.checked = true;
                actionTextElement.text("Unselect All");
            } else {
                allTasksCheckbox.checked = false;
                actionTextElement.text("Select All");
            }
        });
    }


    generateTaskHTML(item) {
        this.count++
        const eventTime = axProcessObj.formatEventDate(item.eventdatetime);
        const displayTitleInitial = item.displaytitle ? item.displaytitle.charAt(0).toUpperCase() : "N/A";
        const isActive = item.cstatus ?.toLowerCase() === "active" ? "active-row" : "completed-row";
        var userName = item.fromuser ?.toLowerCase() === window.top.mainUserName ? " &nbsp; " : item.fromuser
        var selectedclicked = document.querySelector('.selectbtn[data-kt-menu-attach="parent"]').classList.contains('selectclicked')
        var statusIcon = this.getRowIcon(item)

        if (selectedclicked) {
            return `
            <div class="container listrow ${isActive}" data-status="${item.cstatus}" data-index=${this.count}>
            <div class="row">
    
                <div class="col-2 checkbox-wrapper">
                <div>  <input type="checkbox" class="task-checkbox selectclicked" id="checkbox-${item.taskid}" checked
                            style="visibility:visible"></div>
                        <div class="user" style="display:none;">${statusIcon.iconHtml}</div>
                </div>
                <div class="col-7 col-content" style="${item.displaycontent ? '' : 'display: flex; align-items: center; justify-content: center;'}">
                    <div>
                        <a href="javascript:void(0)" class="tasktitle ProcessFlow_New-List-Title Procurement-list"
                            onclick="axProcessObj.openTask(this, '${item.taskname}','${item.tasktype}','${item.transid}','${item.keyfield}','${item.keyvalue}','${item.recordid}','${item.taskid}', '${item.indexno}','${item.hlink_transid}','${item.hlink_params}','${item.processname}','${item.msgtype}', 'LeftPanel');axProcessObj.toggleRowBackground(this) "
                            data-caption="${item.taskname}" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-title="${item.displaytitle ? item.displaytitle : ''}">
                            ${item.displaytitle || item.msgtype || "Untitled Task"}
                        </a>
                        </div>
                        <div class="taskcontent">${item.displaycontent || ""}</div>
                       <div class="rowicons">${statusIcon.tooltip}</div>
                   
                </div>
                    <div class="col-3 timespace">
                    <div class="nametime">${userName}</div>
                        <div class="nametime" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-title="${eventTime}">${eventTime}
                        </div>
                    </div>
                </div>
            </div>`;
        }
        return `
        <div class="container listrow ${isActive}"data-status="${item.cstatus}" data-index=${this.count}>
        <div class="row">
            <div class="col-2 checkbox-wrapper">
                <div><input type="checkbox" class="task-checkbox" id="checkbox-${item.taskid}"></div>
                <div class="user">${statusIcon.iconHtml}</div>

            </div>
            <div class="col-7" style="${item.displaycontent ? '' : 'display: flex; align-items: center; justify-content: center;'}">
                <div>
                    <a href="javascript:void(0)" class="tasktitle ProcessFlow_New-List-Title Procurement-list" 
                    onclick="axProcessObj.openTask(this, '${item.taskname}','${item.tasktype}','${item.transid}','${item.keyfield}','${item.keyvalue}','${item.recordid}','${item.taskid}', '${item.indexno}','${item.hlink_transid}','${item.hlink_params}','${item.processname}','${item.msgtype}', 'LeftPanel');axProcessObj.toggleRowBackground(this) "
                    data-caption="${item.taskname}" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-title="${item.displaytitle ? item.displaytitle : ''}">
                        ${item.displaytitle || item.msgtype || "Untitled Task"}
                    </a>
                    </div>
                    <div class="taskcontent">${item.displaycontent || ""}</div>
                    <div class="rowicons">${statusIcon.tooltip}</div>
           
            </div>
            <div class="timespace col-3">
                <div class="nametime">${userName}</div>
                <div class="nametime" data-bs-toggle="tooltip" data-bs-placement="bottom" data-bs-title="${eventTime}">${eventTime}</div>

            </div>
        
        </div>
    </div>`;

    }

    createAccordionForTimeframe(timeframe, tasks, existingAccordion) {
        const isToday = timeframe === 'Today' ? 'style="display: none;"' : '';
        const accHTML = `
            <div class="accordion accordion-flush"  id="section-${timeframe.replace(/\s+/g, '')}">
                <div class="accordion-item">
                    <h2 class="accordion-header" id="heading-${timeframe}" ${isToday}>
                        <button class="accordion-button" type="button"  
                            data-bs-target="#collapse-${timeframe.replace(/\s+/g, '')}" aria-expanded="false" 
                            aria-controls="collapse-${timeframe}" >
                            ${timeframe}
                        </button>
                    </h2>
                    <div id="collapse-${timeframe.replace(/\s+/g, '')}" class="accordion-collapse collapse show" 
                        aria-labelledby="heading-${timeframe}" data-bs-parent="#section-${timeframe.replace(/\s+/g, '')}">
                        <div class="accordion-body">
                            ${tasks.map((task) => this.generateTaskHTML(task)).join("")}
                        </div>
                    </div>
                </div>
            </div>`;
        document.getElementById("plistContent").insertAdjacentHTML("beforeend", accHTML);


        const currentSection = document.getElementById(`section-${timeframe.replace(/\s+/g, '')}`);
        const previousSection = currentSection ?.previousElementSibling;
        const previousSectioncollapseButton = previousSection ?.querySelector('.accordion-button');



        if (previousSection && previousSectioncollapseButton != null) {
            if (previousSectioncollapseButton) {
                previousSectioncollapseButton.classList.add('show-collapse-button')
                previousSectioncollapseButton.setAttribute('data-bs-toggle', 'collapse')
            } else {

                previousSectioncollapseButton.classList.remove('show-collapse-button');
                previousSectioncollapseButton.removeAttribute('data-bs-toggle', 'collapse')
            }

        }

    }

    showNoMoreRecords() {
        const container = document.getElementById("plistContent");
        const existingMessage = container.querySelector(".no-more-records");
        if (!existingMessage) {
            const messageDiv = document.createElement("div");
            messageDiv.textContent = "No more records to display.";
            messageDiv.className = "no-more-records";
            messageDiv.style.color = "red";
            messageDiv.style.margin = "10px";
            messageDiv.style.padding = "10px";
            messageDiv.style.textAlign = "center";
            container.appendChild(messageDiv);
        }

        var currentSection = document.getElementById(`section-Previousrecords`);
        currentSection ?.querySelector('.accordion-button').classList.add('show-collapse-button')
        currentSection ?.querySelector('.accordion-button').setAttribute('data-bs-toggle', 'collapse')
    }

    processListData(plst, col) {
        try {
            let returnData = '';
            // Group items by date
            const eventDate = plst.eventdatetime.split(' ')[0]; // '12/11/2024'
            const groupedData = plst.reduce((acc, item) => {
                const timeframe = axProcessObj.getTimeframe(eventDate);
                if (!acc[timeframe]) {
                    acc[timeframe] = [];
                }
                acc[timeframe].push(item);
                return acc;
            }, {});

            Object.keys(groupedData).forEach((timeframe, index) => {
                const sectionId = `section-${index}`;
                const collapseId = `collapse-${index}`;

                returnData += `
                      <div class="accordion accordion-flush" id="${sectionId}">
                        <div class="accordion-item">
                          <h2 class="accordion-header" id="heading-${index}">
                            <button class="accordion-button" type="button" data-bs-target="#${collapseId}" aria-expanded="false" aria-controls="${collapseId}">
                              ${timeframe} 
                            </button>
                          </h2>
                          <div id="${collapseId}" class="accordion-collapse collapse" aria-labelledby="heading-${index}" data-bs-parent="#${sectionId}">
                            <div class="accordion-body">
                              ${groupedData[timeframe].map(item => `<p>Content for ${item.date}</p>`).join('')}
                            </div>
                          </div>
                        </div>
                      </div>
                    `;
            });

            return returnData;
        } catch (error) {
            console.error('Error processing task data:', error.message);
            return '<p>Error processing task data.</p>';
        }
    }

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
        ShowDimmer(true);
        let tryFilter = {
            user: document.getElementById("pdFilUser").value || "",
            from: document.getElementById("pdFilFrom").value ?.replaceAll("-", "/") || "",
            to: document.getElementById("pdFilTo").value ?.replaceAll("-", "/") || "",
        };

        tryFilter.allTrs = document.querySelectorAll('#pendingList tr, #completedList tr');

        if (type == "reset") {
            document.getElementById("pdFilUser").value = "";
            document.getElementById("pdFilFrom").value = "";
            document.getElementById("pdFilTo").value = "";
            let _leftType = $("#PROFLOW_Left li a.active").attr("id");
            if (_leftType == 'pending')
                this.fetchProcessList("ProcessList");
            else
                this.fetchProcessList("ProcessList", "TaskCompletion");
        } else if (type == "filter" && (tryFilter && (tryFilter.user || tryFilter.from || tryFilter.to)) == "") {
            ShowDimmer(false);
            this.catchError("Filter parameters cannot be left empty..!!");
        } else {
            let _leftType = $("#PROFLOW_Left li a.active").attr("id");

            let _this = this,
                data = {},
                url = "";
            url = "../aspx/AxPEG.aspx/AxGetFilteredActiveTasks";
            data = { filterType: _leftType, pageNo: _pageno, pageSize: _pagesize, fromuser: tryFilter.user, processname: '', fromdate: tryFilter.from, todate: tryFilter.to, searchtext: '' };

            this.callAPI(url, data, true, result => {
                if (result.success) {
                    let json = JSON.parse(result.response);
                    let dataResult = _this.dataConvert(json, "ARM");
                    TaskCount = dataResult.result.count;
                    if (typeof _leftType != 'undefined' && _leftType == 'completed') {
                        if (typeof dataResult.result.completedtasks != "undefined" && dataResult.result.completedtasks.length > 0) {
                            completedtasksJson = dataResult.result.completedtasks;
                            _this.showProcessList('completed');
                        } else {
                            showAlertDialog("warning", "No task available.");
                        }
                    } else {
                        if (typeof dataResult.result.pendingtasks != "undefined" && dataResult.result.pendingtasks.length > 0) {
                            pendingtasksJson = dataResult.result.pendingtasks;
                            _this.showProcessList('pending');
                        } else {
                            showAlertDialog("warning", "No task available.");
                        }
                    }
                    document.getElementById("pdFilUser").value = "";
                    document.getElementById("pdFilFrom").value = "";
                    document.getElementById("pdFilTo").value = "";
                    ShowDimmer(false);
                } else {
                    ShowDimmer(false);
                }
            });
        }
    };

    taskSearch(type) {
        ShowDimmer(true);
        let trySearch = {
            searchVal: document.getElementById("advTextSearch").value || ""
        };

        //trySearch.allTrs = document.querySelectorAll('#pendingList tr, #completedList tr');

        if (type == "reset") {
            document.getElementById("advTextSearch").value = "";
            let _leftType = $("#PROFLOW_Left li a.active").attr("id");
            if (_leftType == 'pending')
                this.fetchProcessList("ProcessList");
            else
                this.fetchProcessList("ProcessList", "TaskCompletion");
        } else if (type == "search" && (trySearch && trySearch.searchVal) == "") {
            ShowDimmer(false);
            this.catchError("Search cannot be left empty..!!");
        } else {
            let _leftType = $("#PROFLOW_Left li a.active").attr("id");

            let _this = this, data = {}, url = "";
            url = "../aspx/AxPEG.aspx/AxGetFilteredActiveTasks";
            data = { filterType: _leftType, pageNo: _pageno, pageSize: _pagesize, fromuser: '', processname: '', fromdate: '', todate: '', searchtext: trySearch.searchVal };

            this.callAPI(url, data, true, result => {
                if (result.success) {
                    let json = JSON.parse(result.response);
                    let dataResult = _this.dataConvert(json, "ARM");
                    TaskCount = dataResult.result.count;
                    if (typeof _leftType != 'undefined' && _leftType == 'completed') {
                        if (typeof dataResult.result.completedtasks != "undefined" && dataResult.result.completedtasks.length > 0) {
                            completedtasksJson = dataResult.result.completedtasks;
                            _this.showProcessList('completed');
                        } else {
                            showAlertDialog("warning", "No task available.");
                        }
                    }
                    else {
                        if (typeof dataResult.result.pendingtasks != "undefined" && dataResult.result.pendingtasks.length > 0) {
                            pendingtasksJson = dataResult.result.pendingtasks;
                            _this.showProcessList('pending');
                        } else {
                            showAlertDialog("warning", "No task available.");
                        }
                    }

                    document.getElementById("advTextSearch").value = "";
                    ShowDimmer(false);
                } else {
                    ShowDimmer(false);
                }
            });
        }
    };

    showProgress(elem) {
        setTimeout(function () {
            axProcessObj.beforeLoad();
        }, 100)
        let _this = this, data = {}, url = "";
        url = "../aspx/AxPEG.aspx/AxGetProcess";
        let elemTaskId = elem?.dataset?.taskid || this.taskId;
        if (typeof elem != "undefined")
            data = { processName: this.processName, keyField: this.keyField, keyValue: this.keyValue };
        else {
            let _thisEle = $($('.Procurement-list')[0]);
            data = { processName: _thisEle.data('processname'), keyField: _thisEle.data('keyfield'), keyValue: _thisEle.data('keyvalue') };
            elemTaskId = _thisEle.data('taskid');
            this.processName = _thisEle.data('processname');
        }
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
                            this.calledFrom = "ProgressBar";
                            document.querySelector(".horizontal-steps.pending")?.click();
                            document.querySelector(".horizontal-steps.pending")?.scrollIntoView();
                            elemTaskId = document.querySelector(".horizontal-steps.pending").dataset?.taskid;
                        }
                        else if (!this.inValid(document.querySelector(".horizontal-steps"))) {
                            this.calledFrom = "ProgressBar";
                            document.querySelector(".horizontal-steps")?.click();
                            document.querySelector(".horizontal-steps")?.scrollIntoView();
                            elemTaskId = document.querySelector(".horizontal-steps.pending").dataset?.taskid;
                        }
                    }
                }
                else if (!this.inValid(this.taskId)) {
                    this.calledFrom = "ProgressBar";
                    let selector = `.horizontal-steps[data-taskid="${this.taskId}"]`;
                    if (!this.inValid(document.querySelector(selector))) {
                        document.querySelector(selector)?.click();
                        document.querySelector(selector)?.scrollIntoView();
                        if (this.inValid(elemTaskId))
                            elemTaskId = document.querySelector(selector).dataset?.taskid;
                    }
                    this.target = null;
                }
                else if (!this.inValid(this.target)) {
                    this.calledFrom = "ProgressBar";
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
                    if (this.inValid(elemTaskId))
                        elemTaskId = document.querySelector(selector).dataset?.taskid;
                    this.target = null;
                }
                else {
                    this.calledFrom = "ProgressBar";
                    let pendingSelector = `.horizontal-steps.pending`;
                    let selector = `.horizontal-steps`;
                    if (!this.inValid(document.querySelector(pendingSelector))) {
                        document.querySelector(selector)?.click();
                        document.querySelector(pendingSelector)?.scrollIntoView();
                    }
                    else if (!this.inValid(document.querySelector(selector))) {
                        document.querySelector(selector)?.click();
                        document.querySelector(selector)?.scrollIntoView();
                    }
                    if (this.inValid(elemTaskId))
                        elemTaskId = document.querySelector(selector).dataset?.taskid;
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


    showProgressNew() {
        setTimeout(function () {
            axProcessObj.beforeLoad();
        }, 100);

        let _this = this, data = {}, url = "";
        url = "../aspx/AxPEG.aspx/AxPEGGetTaskDetails";

        data = { processName: this.processName, taskType: this.taskType, taskId: this.taskId, keyValue: this.keyValue };

        this.callAPI(url, data, true, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                processflowJson = dataResult.result.processflow;

                if (processflowJson.length > 0) {
                    //ShowDimmer(false);
                    let dataResult = processflowJson;

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

                    setTimeout(function () {
                        axProcessObj.afterLoad();
                    }, 100)
                }
                else {
                    ShowDimmer(false);
                }
            } else {
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
            url: "../aspx/AxPEG.aspx/AxRefreshCardsData",
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
                    var mergedCardsData = cardsData.value.concat(result.result.cards);
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
        let url = "../aspx/AxPEG.aspx/AxGetProcessDefinition";
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
        let url = "../aspx/AxPEG.aspx/AxGetTimelineData";
        let data = { keyValue: _this.keyvalue, processName: _this.processName };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.constructTimeline(dataResult);
                ShowDimmer(false);
            } else {
                ShowDimmer(false);
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
