class AxProcessBuilder {
    constructor() {
        this.processName = '';
        this.keyField = '';
        this.keyValue = '';
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
        // this.taskCaptionHtml = `<div class="process-sub-flow" data-groupname="{{taskgroup}}"><div class="Task-process-list status-{{taskstatus}}" onclick="axProcessBuilderObj.openTask(this, '{{taskname}}', '{{tasktype}}', '{{transid}}', '{{keyfield}}', '{{keyvalue}}', '{{recordid}}');"><a href="javascript:void(0)">{{taskname}}</a></div></div>`;
        this.taskCaptionHtml = `<div class="process-sub-flow" data-groupname="{{taskgroup}}" data-tasktype="{{tasktype}}">`;
        this.taskCaptionHtml += `<div class="positionRel Task-process-list">`;
        this.taskCaptionHtml += `<a href="javascript:void(0)" onclick="axProcessBuilderObj.openTask(this, '{{tasktype}}', '{{recordid}}');">{{taskname}}</a>`;
        this.taskCaptionHtml += `<div class="plusactioncontainer">`;
        this.taskCaptionHtml += `<div class="addTopBtnlink" ><span class="material-icons material-icons-style material-icons-2 addTopBtn">add</span>`;
        this.taskCaptionHtml += `<div class="addProcessHilight">`;
        this.taskCaptionHtml += `<a href="javascript:void(0)" class="make" onclick="axProcessBuilderObj.openNewTask('Make','{{indexno}}', '{{processname}}','{{taskgroup}}'); return false;">Make</a><a href="javascript:void(0)" class="approve"  onclick="axProcessBuilderObj.openNewTask('Approve','{{indexno}}', '{{processname}}','{{taskgroup}}'); return false;">Approve<a><a href="javascript:void(0)" class="check" onclick="axProcessBuilderObj.openNewTask('Check','{{indexno}}', '{{processname}}','{{taskgroup}}'); return false;">Check</a>`;
        this.taskCaptionHtml += `<a href="javascript:void(0)" class="if" onclick="axProcessBuilderObj.openNewTask('If','{{indexno}}', '{{processname}}','{{taskgroup}}'); return false;">If</a><a href="javascript:void(0)" class="else" onclick="axProcessBuilderObj.openNewTask('Else','{{indexno}}', '{{processname}}','{{taskgroup}}'); return false;">Else</a><a href="javascript:void(0)" class="IfElse" onclick="axProcessBuilderObj.openNewTask('Else if','{{indexno}}', '{{processname}}','{{taskgroup}}'); return false;">Else if</a><a href="javascript:void(0)" class="end" onclick="axProcessBuilderObj.openNewTask('End','{{indexno}}', '{{processname}}','{{taskgroup}}'); return false;">End</a>`;
        this.taskCaptionHtml += `</div></div>`;
        this.taskCaptionHtml += `<div class="addTopBtnHLine">&nbsp;</div>`;
        this.taskCaptionHtml += `<div class="addTopBtnVLine">&nbsp;</div>`;
        this.taskCaptionHtml += `<div class="addBotBtnlink"><span class="material-icons material-icons-style material-icons-2 addBotBtn">add</span>`;
        this.taskCaptionHtml += `<div class="addSubProcessHilight">`;
        this.taskCaptionHtml += `<a href="javascript:void(0)" class="make" onclick="axProcessBuilderObj.openNewTask('Make','{{nextindexno}}', '{{processname}}','{{taskgroup}}'); return false;">Make</a><a href="javascript:void(0)" class="approve"  onclick="axProcessBuilderObj.openNewTask('Approve','{{nextindexno}}', '{{processname}}','{{taskgroup}}'); return false;">Approve<a><a href="javascript:void(0)" class="check" onclick="axProcessBuilderObj.openNewTask('Check','{{nextindexno}}', '{{processname}}','{{taskgroup}}'); return false;">Check</a>`;
        this.taskCaptionHtml += `<a href="javascript:void(0)" class="if" onclick="axProcessBuilderObj.openNewTask('If','{{nextindexno}}', '{{processname}}','{{taskgroup}}'); return false;">If</a><a href="javascript:void(0)" class="else" onclick="axProcessBuilderObj.openNewTask('Else','{{nextindexno}}', '{{processname}}','{{taskgroup}}'); return false;">Else</a><a href="javascript:void(0)" class="IfElse" onclick="axProcessBuilderObj.openNewTask('Else if','{{nextindexno}}', '{{processname}}','{{taskgroup}}'); return false;">Else if</a><a href="javascript:void(0)" class="end" onclick="axProcessBuilderObj.openNewTask('End','{{nextindexno}}', '{{processname}}','{{taskgroup}}'); return false;">End</a>`;
        this.taskCaptionHtml += `</div></div></div></div>`;
        this.taskCaptionHtml += `</div>`;
        this.dataSources = [];
        this.processFlowObj = {};
        this.getUrlParams();
    }

    fetchProcessDefinition(name) {
        let _this = this;
        let url = "../aspx/AxPEG.aspx/AxGetProcessDefinition";
        let data = { processName: this.processName };
        this.callAPI(url, data, true, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                if (json.d.startsWith('<!DOCTYPE HTML PUBLIC ')) {
                    showAlertDialog('error', appGlobalVarsObject.lcm[572]);
                    return;
                }
                let dataResult = _this.dataConvert(json, "ARM");
                _this.dataSources[name] = dataResult;
                _this.showProcessDefinition();
            }
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
    }

    catchError(error) {
        showAlertDialog("error", error);
    };

    showSuccess(message) {
        showAlertDialog("success", message);
    };

    isEmpty(elem) {
        return elem == "";
    };

    isUndefined(elem) {
        return typeof elem == "undefined";
    };

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
                data = JSON.parse(data.d);
                if (data.result.success) {
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
    }

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

    showProcessDefinition() {
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
        if (document.querySelectorAll(".Task-process-list").length == 0) {
            this.openNewTask("Make", "1", this.processName, '');
            document.querySelector("#PROFLOW-profile-container arrows").classList.toggle("d-none");
            document.querySelector("#NoRecords").classList.toggle("d-none");
        }
        else
            document.querySelector(".Task-process-list a").click();

        $(".accordion-icon").click(function () {
            let groupname = $(this).attr('data-groupname');
            $(this).toggleClass('rotate');
            $(`.process-sub-flow[data-groupname="${groupname}"]`).toggle();
        });
    }

    openTask(elem, taskType, recordId) {
        var tasks = document.querySelectorAll(".Task-process-list.Active");

        [].forEach.call(tasks, function (el) {
            el.classList.remove("Active");
        });

        elem?.closest('.Task-process-list').classList.add("Active");

        var steps = document.querySelectorAll(".step.step-active");

        [].forEach.call(steps, function (el) {
            el.classList.remove("step-active");
        });

        elem?.closest(".step").classList.add("step-active");

        let targetTransId = "";
        taskType = taskType.toUpperCase();
        if (taskType == "MAKE")
            targetTransId = "pgv2m";
        else if (taskType == "APPROVE" || taskType == "CHECK")
            targetTransId = "pgv2a";
        else if (taskType == "IF" || taskType == "ELSE" || taskType == "ELSE IF" || taskType == "END")
            targetTransId = "pgv2c";

        this.loadTstruct(targetTransId, recordId);
    }

    openNewTask(taskType, indexNo, processName ,taskgGroup) {        
        let targetTransId = "";
        let tmpTaskType = taskType.toUpperCase();
        if (tmpTaskType == "MAKE")
            targetTransId = "pgv2m";
        else if (tmpTaskType == "APPROVE" || tmpTaskType == "CHECK")
            targetTransId = "pgv2a";
        else if (tmpTaskType == "IF" || tmpTaskType == "ELSE" || tmpTaskType == "ELSE IF" || tmpTaskType == "END")
            targetTransId = "pgv2c";
        this.openTstruct(targetTransId, `&processname=${processName}&indexNo=${indexNo}&taskType=${taskType}&taskgroupname=${taskgGroup}`)
    }

    openTstruct(transId, params) {
        //ShowDimmer(true);
        let url = `../aspx/tstruct.aspx?transid=${transId}&act=open&recordid=0${params}`;
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
    }

    loadTstruct(transId, recordId) {
        //ShowDimmer(true);
        let url = `../aspx/tstruct.aspx?transid=${transId}&act=load&recordid=${recordId}`;
        let $rightIframe = document.querySelector("#rightIframe");
        $rightIframe.setAttribute("src", "");
        $rightIframe.setAttribute("src", url);
    }

    getUrlParams() {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        this.keyField = urlParams.get('keyfield');
        this.keyValue = urlParams.get('keyvalue') || '';
        this.processName = urlParams.get('processname') || _processName;
        if (curMode == 'new') {
            document.querySelector('#process-name').innerText = "Enter Process Flow";
        }
        else {
            document.querySelector('#process-name').innerText = this.processName;
        }
    }
   
}

var axProcessBuilderObj;
$(document).ready(function () {
    ShowDimmer(true);
    axProcessBuilderObj = new AxProcessBuilder();
    axProcessBuilderObj.fetchProcessDefinition("Process");
});
