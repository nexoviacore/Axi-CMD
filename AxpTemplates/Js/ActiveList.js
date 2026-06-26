var armToken = "";
class AxTasks {
    constructor() {
        this.isAxpertFlutter = !this.isNullOrEmpty(armToken);
        this.taskCardHtml = `
        <div class="row">
            <div class="col-md-12 pb-8 task-listing-card" data-process="{{processname}}" data-type="{{tasktype}}" data-taskid="{{taskid}}" data-transid="{{transid}}" data-keyvalue="{{keyvalue}}" >
                <h2 class="task-heading">
                    <input type="checkbox" value="" class="form-check-input task-list-checkbox">
                    <a href="javascript:void(0)" onclick="axTasksObj.openProcess('{{processname}}','{{keyfield}}','{{keyvalue}}','{{taskid}}')" title="{{displaytitle}}">
                        <span class="material-icons material-icons-style material-icons-1 display-icon task-listing-icons">{{displayicon}}</span>
                        {{displaytitle}}
                    </a>
                    
                </h2>
                <div class="task-subtitle">{{displaysubtitle}}</div>
                <div class="mobile-more-btns">
                    <button class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn btn-sm" id="Mobile_more-btns">
                        <span class="material-icons material-icons-style material-icons-2">more_horiz</span>


                    </button><ul class="total-activity">
                        <li>
                            <a href="javascript:void(0)">
                                <span class="material-icons material-icons-style material-icons-2">
                                    <span class="material-symbols-outlined">open_in_new</span>
                                </span><span class="TA-name">Open </span>
                            </a>

                            <a href="javascript:void(0)">
                            <span class="material-icons material-icons-style material-icons-2">
                                <span class="material-symbols-outlined">more_vert</span>
                            </span>
                        </a>

                        </li>
                        <li>
                            <a href="javascript:void(0)">
                                <span class="material-icons material-icons-style material-icons-2" style="color: green;">
                                    <span class="material-symbols-outlined">
                                        check_circle
                                    </span>
                                </span><span class="TA-name">Approve</span>
                            </a>
                            <a href="javascript:void(0)">
                            <span class="material-icons material-icons-style material-icons-2">
                                <span class="material-symbols-outlined">more_vert</span>
                            </span>
                        </a>
                        </li>
                        <li><a href="javascript:void(0)">   <span class="material-icons material-icons-style material-icons-2" style="color: red;">cancel</span><span class="TA-name">Reject</span></a>
                        <a href="javascript:void(0)">
                        <span class="material-icons material-icons-style material-icons-2">
                            <span class="material-symbols-outlined">more_vert</span>
                        </span>
                    </a></li>

                        <li><a href="javascript:void(0)">   <span class="material-icons material-icons-style material-icons-2" style="color: blueviolet;">reply</span><span class="TA-name">Return</span></a>
                        <a href="javascript:void(0)">
                        <span class="material-icons material-icons-style material-icons-2">
                            <span class="material-symbols-outlined">more_vert</span>
                        </span>
                    </a></li>

                        <li>
                            <a href="javascript:void(0)">
                                <span class="material-icons material-icons-style material-icons-2" style="color: brown;">
                                    <span class="material-symbols-outlined">
                                        fast_forward
                                    </span>
                                </span><span class="TA-name">Forward</span>
                            </a>
                            <a href="javascript:void(0)">
                            <span class="material-icons material-icons-style material-icons-2">
                                <span class="material-symbols-outlined">more_vert</span>
                            </span>
                        </a>
                        </li>
                    </ul>
                </div>

                <div class="d-flex flex-row task-date  ">
                    <span title="Assigned From" class="task-assignedBy"> {{fromuser}}</span>
                    <span title="Assigned On" class="start-date"> {{eventdatetime}}</span>
                </div>

                <div class="d-flex flex-row Task-content">
                    <div class="fs-6 fw-semibold  py-2 col-md-12">
                        {{displaycontent}}
                    </div>
                </div>

                
                <div class="text-end task-more-btns">
                    <div class="d-flex align-items-center  task-more-btns">
                        {{task_buttons_html}}
                    </div>
                </div>
            </div>
        </div>`;
        this.taskListHtml = `
        <table class="table align-middle  fs-6 gy-5 mb-0 dataTable no-footer task-listing-card" data-process="{{processname}}" data-type="{{tasktype}}" data-taskid="{{taskid}}"  data-transid="{{transid}}" data-keyvalue="{{keyvalue}}">
            <tbody>
                <tr class="d-flex ">

                    <td class="d-flex align-items-center task-name">
                        <div class="d-flex flex-column">
                            <span class="List_view accordion-icon arrow{{taskid}}" onclick="axTasksObj.toggleTask('{{taskid}}')">
                                <span class="material-icons material-icons-style material-icons-2">chevron_right</span>
                            </span>
                            <input type="checkbox" value="" class="form-check-input task-list-checkbox">
                            <a href="javascript:void(0)" class="text-gray-800 fw-bold fs-6  mb-1 task-title" title="{{displaytitle}}"><span href="javascript:void(0)" onclick="axTasksObj.openProcess('{{processname}}','{{keyfield}}','{{keyvalue}}','{{taskid}}')"><span class="material-icons material-icons-style material-icons-1 display-icon task-listing-icons">{{displayicon}}</span>{{displaytitle}}</span></a>
                            <span class="task-subtitle">{{displaysubtitle}}</span>
                        </div>
                    </td>

                    <td class="d-flex align-items-center MObile-task-more-btns">
                        <div class="mobile-more-btns">
                            <button class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm me-2 tb-btn btn-sm" id="Mobile_more-btns">
                                <span class="material-icons material-icons-style material-icons-2">more_horiz</span>


                            </button>
                                            
                            <ul class="total-activity">
                                {{mobile_task_buttons_html}}
                            </ul>
                        </div>
                    </td>

                    <td class="task-right">

                        <div class="d-flex flex-row task-process">
                            <a href="javascript:void(0)" class="text-gray-800 fs-6 mb-1 task-assignedBy" title="Assigned By"> {{fromuser}}
                            </a>
                            <a href="javascript:void(0)" title="Assigned On" class="text-gray-800 fs-6 mb-1 task-date"> {{eventdatetime}}</a>
                        </div>
                    </td>

                   



                </tr>
                <tr class="taskSubrow{{taskid}} d-none" data-displaycontent="{{displaycontent}}">
                    <td class="task-description">
                        <span class="more">
                            
                        </span>
                    </td>
                </tr>  
                
                <tr>
                    <td class="text-end task-more-btns">
                        <div class="d-flex align-items-center task-more-btns">{{task_buttons_html}}
                        </div>
                    </td>
                </tr>

            </tbody>
        </table>`

        this.taskNoRecordsCardHtml = `
        <table class="table align-middle  fs-6 gy-5 mb-0 dataTable no-footer task-listing-card">
            <tbody>
                <tr class="d-flex header">                                           
                    <td class="d-flex align-items-center task-name">
                        No active tasks available.
                    </td>
                </tr>
            </tbody>
        </table>`;
        this.taskNotFoundHtml = `
        <table class="table align-middle  fs-6 gy-5 mb-0 dataTable no-footer task-listing-card">
            <tbody>
                <tr class="d-flex header">                                           
                    <td class="d-flex align-items-center task-name">
                        Task details not found.
                    </td>
                </tr>
            </tbody>
        </table>`;
        this.view = "List";
        this.filters = {};
        this.filters.allTasksHtml = `<a href="javascript:void(0)" class="task_activity-counts task-process-selection active" >
                                    <span class="task_activity-name  d-block ">All Tasks</span>
                                    <div class="symbol-label" id="All-tasks-count">...</div>
                                </a>`;
        this.filters.processFilterCardHtml = ` 
        <div class="accordion accordion-icon-toggle task-process-selected" id="" data-process="{{processname}}">
            <div class="accordion-header py-3 d-flex collapsed sub-task-lists-wrap"
                    data-bs-toggle="collapse" data-bs-target="#Task_list-{{processid}}"
                    aria-expanded="false">
                <span class="accordion-icon">
                    <span class="material-icons material-icons-style material-icons-2">chevron_right</span>
                </span>
                <div class="  task_activity_content">
                    <div class="d-flex justify-content-center  " onclick="axTasksObj.filterTasks('process','{{processname}}',this)">
                        <div class="symbol symbol-40px symbol-circle me-5">
                            <div class="symbol-label bgs{{sno}}">
                                <span class="material-icons material-icons-style material-icons-2">account_tree</span>
                            </div>
                        </div>
                        <div class="task-process-selection">
                            <span class="task_activity-name  d-block ">{{processname}}</span>
                            <a href="javascript:void(0)" class="task_activity-counts">
                                <div class="symbol-label">{{processcount}}</div>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Task_list-{{processid}}" class="fs-6 ps-10 accordion-body collapse" data-process="{{processname}}">                
            </div>
        </div>`
        this.filters.typeFilterCardHtml = `
        <div class="step sub-task-lists " onclick="axTasksObj.filterTasks('type','{{processname}}~~{{type}}',this)">
            <div> <span
                    class="material-icons material-icons-style material-icons-2">{{icon}}</span>
            </div>
            <div class="title">{{type}} <span class="task-count-sub">{{count}}</span></div>
        </div>`
        this.filters.viewAllBtnHtml = `<!--<button class="task-activity-all btn btn-white" id="viewAllFilters" onclick="axTasksObj.viewAllFilters();" title="View All">View All</button>-->`;

        this.taskBtnsHtml = {};
        this.taskBtnsHtml.make = `
            <a href="javascript:void(0)" title="Make" class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  btn-sm  me-2" onclick="axTasksObj.openProcess('{{processname}}','{{keyfield}}','{{keyvalue}}','{{taskid}}')""><span class="material-icons material-icons-style material-icons-2" style="color: orange;">assignment</span></a>`;
        this.taskBtnsHtml.view = `
            <a href="javascript:void(0)" title="View form" class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  btn-sm  me-2 " onclick="axTasksObj.openTstruct('{{transid}}','{{recordid}}')""><span class="material-icons material-icons-style material-icons-2" style="color: darkmagenta;">visibility</span> View form
          
            
            </a>`;
        this.taskBtnsHtml.history = `
            <a href="javascript:void(0)" title="View History" class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  btn-sm  me-2 " onclick="axTasksObj.openHistory('{{processname}}','{{keyfield}}','{{keyvalue}}')""><span class="material-icons material-icons-style material-icons-2" style="color: blue;">history</span>View History
         
            </a>`;

        this.taskBtnsHtml.approve = `
        <div class="task-actions-sets d-flex">

            <a href="javascript:void(0)" title="Approve"
                class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  btn-sm  me-2 "
                onclick="axTasksObj.doApprove('{{taskid}}', this);"><span class="material-icons material-icons-style material-icons-2"
                    style="color: #47BE7D;">check_circle</span> Approve
            </a>

            <a href="javascript:void(0)" title="Approve Reasons/Comments" data-popover="APPROVE{{taskid}}"
                class="btn btn-white btn-color-gray-700d-inline-flex align-items-center  btn-sm  more-comments"
                data-kt-menu-trigger="click" data-kt-menu-placement="bottom" data-kt-menu-flip="top">
                <span class="comments-icons material-icons material-icons-style material-icons-2">more_vert</span>
            </a>
            <div  class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover"
                data-kt-menu="true">
                <div class="card">
                    <div class="card-body py-5z">
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Approve">
                                <label class="form-label col-form-label">Approve reasons</label>
                                <div class="input-group">
                                    <select data-tasktype="Approve" data-taskid="APPROVE{{taskid}}" class="form-select "
                                        data-tasktype="Approve">
                                        <option value="">NA</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Approve">
                                <label class="form-label col-form-label  {{isApprovalComments}}">Comments</label>
                                <div class="input-group">
                                    <textarea data-tasktype="Approve" data-taskid="APPROVE{{taskid}}" class="form-control"
                                        data-tasktype="Approve" {{isApprovalComments}}></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="text-center Approval-OK">
                            <button class="btn btn-sm btn-primary shadow-sm" onclick="return axTasksObj.doApprove('{{taskid}}', this);">Approve</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
`;
        this.taskBtnsHtml.reject = `
        <div class="task-actions-sets d-flex">
            <a href="javascript:void(0)" title="Reject"
                class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  me-2  btn-sm"
                onclick="axTasksObj.doReject('{{taskid}}', this)"><span class="material-icons material-icons-style material-icons-2"
                    style="color: red;">cancel</span>Reject</a>
            <a href="javascript:void(0)" title="Reject Reasons/Comments" data-popover="REJECT{{taskid}}"
                class="btn btn-white btn-color-gray-700d-inline-flex align-items-center  btn-sm  more-comments"
                data-kt-menu-trigger="click" data-kt-menu-placement="bottom" data-kt-menu-flip="top">
                <span class="comments-icons material-icons material-icons-style material-icons-2">more_vert</span>
            </a>
            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover"
                data-kt-menu="true">
                <div class="card">
                    <div class="card-body py-5z">
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Reject">
                                <label class="form-label col-form-label">Reject reasons</label>
                                <div class="input-group">
                                    <select data-tasktype="Reject" data-taskid="REJECT{{taskid}}" class="form-select "
                                        data-tasktype="Reject">
                                        <option value="">NA</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Reject">
                                <label class="form-label col-form-label  {{isRejectComments}}">Comments</label>
                                <div class="input-group">
                                    <textarea data-tasktype="Reject" data-taskid="REJECT{{taskid}}" class="form-control"
                                        data-tasktype="Reject" {{isRejectComments}}></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="text-center Approval-OK">
                            <button class="btn btn-sm btn-primary shadow-sm" onclick="return axTasksObj.doReject('{{taskid}}', this)">Reject</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
`;
        this.taskBtnsHtml.return = `
        <div class="task-actions-sets d-flex">
            <a href="javascript:void(0)" title="Return"
                class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  me-2  btn-sm"
                onclick="axTasksObj.doReturn('{{taskid}}', this, '{{tasktype}}')"><span class="material-icons material-icons-style material-icons-2"
                    style="color: blueviolet;">reply</span>Return</a>
            <a href="javascript:void(0)" title="Return Reasons/Comments" data-popover="RETURN{{taskid}}"
                class="btn btn-white btn-color-gray-700d-inline-flex align-items-center  btn-sm  more-comments"
                data-kt-menu-trigger="click" data-kt-menu-placement="bottom" data-kt-menu-flip="top">
                <span class="comments-icons material-icons material-icons-style material-icons-2">more_vert</span>
            </a>
            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover"
                data-kt-menu="true">
                <div class="card">
                    <div class="card-body py-5z">
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Return">
                                <label class="form-label col-form-label">Return To</label>
                                <div class="input-group">
                                    <select data-tasktype="Return" data-taskid="returnto_{{taskid}}" class="form-select " data-tasktype="Return">
                                        <option value="0">Initiator</option>
                                        <option value="1">Previous Level</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Return">
                                <label class="form-label col-form-label">Return reasons</label>
                                <div class="input-group">
                                    <select data-tasktype="Return" data-taskid="RETURN{{taskid}}" class="form-select "
                                        data-tasktype="Return">
                                        <option value="">NA</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Return">
                                <label class="form-label col-form-label  {{isApprovalComments}}">Comments</label>
                                <div class="input-group">
                                    <textarea data-tasktype="Return" data-taskid="RETURN{{taskid}}" class="form-control"
                                        data-tasktype="Return" {{isReturnComments}}></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="text-center Approval-OK">
                            <button class="btn btn-sm btn-primary shadow-sm" onclick="return axTasksObj.doReturn('{{taskid}}', this, '{{tasktype}}')">Return</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;

        this.taskBtnsHtml.check = `
             <a href="javascript:void(0)" title="Check" class="btn btn-white btn-color-gray-700 btn-active-primary d-inline-flex align-items-center  me-2  btn-sm" onclick="axTasksObj.doCheck('{{taskid}}')">
			 <span class="material-icons material-icons-style material-icons-2" style="color: #47BE7D;">done_all</span>Check
             </a>            
        `;
        this.taskBtnsHtml.more = `
            <button href="javascript:void(0)" class="btn btn-icon btn-active-light-primary w-30px h-30px"
                data-kt-menu-trigger="{default:'click', lg: 'hover'}" data-kt-menu-attach="parent"
                data-kt-menu-placement="bottom-end">
                <span class="material-icons material-icons-style material-icons-1">
                    more_horiz
                </span>
            </button>
            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-title-gray-700 menu-icon-muted menu-active-bg menu-state-primary fw-bold py-4 fs-6 w-150px Active_list_popover"
                data-kt-menu="true" style="">

                <div class="menu-item px-3">
                    <a href="javascript:void(0)" class="menu-link d-flex px-5 ">
                        <span class="symbol symbol-20px me-4">
                            <span class="material-icons material-icons-style material-icons-1">
                                ios_share
                            </span>
                        </span>Open</a>
                </div>

                <div class="menu-item px-3">
                    <a href="javascript:void(0)" class="menu-link d-flex px-5">
                        <span class="symbol symbol-20px me-4">
                            <span class="material-icons material-icons-style material-icons-1">
                                check_circle
                            </span>
                        </span>Approve</a>
                </div>

                <div class="menu-item px-3">
                    <a href="javascript:void(0)" class="menu-link d-flex px-5">
                        <span class="symbol symbol-20px me-4">
                            <span class="material-icons material-icons-style material-icons-1">
                                cancel
                            </span>
                        </span>Reject </a>
                </div>

                <div class="menu-item px-3">
                    <a href="javascript:void(0)" class="menu-link d-flex px-5">
                        <span class="symbol symbol-20px me-4">
                            <span class="material-icons material-icons-style material-icons-1">
                                reply
                            </span>
                        </span>Return </a>
                </div>
            </div>`;
        this.mobileTaskButtons = {};
        this.mobileTaskButtons.approve = `
        <li>
            <a href="javascript:void(0)" href="javascript:void(0)" title="Approve" onclick="axTasksObj.doApprove('{{taskid}}', this);" class="mobile">
                <span class="material-icons material-icons-style material-icons-2" style="color: #47BE7D;">
                    <span class="material-symbols-outlined">
                        check_circle
                    </span>
                </span><span class="TA-name">Approve</span>
            </a>
            <a href="javascript:void(0)" title="Approve Reasons/Comments" data-popover="APPROVE{{taskid}}" data-kt-menu-trigger="click"
                data-kt-menu-placement="bottom" data-kt-menu-flip="top" class="mobile">
                <span class="material-icons material-icons-style material-icons-2">
                    <span class="material-symbols-outlined">more_vert</span>
                </span>
            </a>
            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover"
                data-kt-menu="true">
                <div class="card">
                    <div class="card-body py-5z">
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Approve">
                                <label class="form-label col-form-label">Approve reasons</label>
                                <div class="input-group">
                                    <select data-tasktype="Approve" data-taskid="APPROVE{{taskid}}" class="form-select mobile"
                                        data-tasktype="Approve">
                                        <option value="">NA</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Approve">
                                <label class="form-label col-form-label  {{isApprovalComments}}">Comments</label>
                                <div class="input-group">
                                    <textarea data-tasktype="Approve" data-taskid="APPROVE{{taskid}}" class="form-control mobile"
                                        data-tasktype="Approve" {{isApprovalComments}}></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="text-center Approval-OK">
                            <button class="btn btn-sm btn-primary shadow-sm mobile"
                                onclick="return axTasksObj.doApprove('{{taskid}}', this);">Approve</button>
                        </div>
                    </div>
                </div>
            </div>
        </li>`;
        this.mobileTaskButtons.reject = `
        <li>
            <a href="javascript:void(0)" href="javascript:void(0)" title="Reject" onclick="axTasksObj.doReject('{{taskid}}', this);" class="mobile">
                <span class="material-icons material-icons-style material-icons-2" style="color: red;">
                    <span class="material-symbols-outlined">
                        cancel
                    </span>
                </span><span class="TA-name">Reject</span>
            </a>
            <a href="javascript:void(0)" title="Reject Reasons/Comments" data-popover="REJECT{{taskid}}" data-kt-menu-trigger="click"
                data-kt-menu-placement="bottom" data-kt-menu-flip="top" class="mobile">
                <span class="material-icons material-icons-style material-icons-2">
                    <span class="material-symbols-outlined">more_vert</span>
                </span>
            </a>
            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover"
                data-kt-menu="true">
                <div class="card">
                    <div class="card-body py-5z">
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Reject">
                                <label class="form-label col-form-label">Reject reasons</label>
                                <div class="input-group">
                                    <select data-tasktype="Reject" data-taskid="REJECT{{taskid}}" class="form-select mobile"
                                        data-tasktype="Reject">
                                        <option value="">NA</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Reject">
                                <label class="form-label col-form-label  {{isRejectComments}}">Comments</label>
                                <div class="input-group">
                                    <textarea data-tasktype="Reject" data-taskid="REJECT{{taskid}}" class="form-control mobile"
                                        data-tasktype="Reject" {{isRejectComments}}></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="text-center Reject-OK">
                            <button class="btn btn-sm btn-primary shadow-sm mobile"
                                onclick="return axTasksObj.doReject('{{taskid}}', this);">Reject</button>
                        </div>
                    </div>
                </div>
            </div>
        </li>`;
        this.mobileTaskButtons.return = `
        <li>
            <a href="javascript:void(0)" href="javascript:void(0)" title="Return" onclick="axTasksObj.doReturn('{{taskid}}', this, '{{tasktype}}');" class="mobile">
                <span class="material-icons material-icons-style material-icons-2" style="color: blueviolet;">
                    <span class="material-symbols-outlined">
                        reply
                    </span>
                </span><span class="TA-name">Return</span>
            </a>
            <a href="javascript:void(0)" title="Return Reasons/Comments" data-popover="RETURN{{taskid}}" data-kt-menu-trigger="click"
                data-kt-menu-placement="bottom" data-kt-menu-flip="top" class="mobile">
                <span class="material-icons material-icons-style material-icons-2">
                    <span class="material-symbols-outlined">more_vert</span>
                </span>
            </a>
            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-400px Active_list_popover"
                data-kt-menu="true">
                <div class="card">
                    <div class="card-body py-5z">
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Return">
                                <label class="form-label col-form-label">Return To</label>
                                <div class="input-group">
                                    <select data-tasktype="Return" data-taskid="returnto_{{taskid}}" class="form-select mobile" data-tasktype="Return">
                                        <option value="0">Initiator</option>
                                        <option value="1">Previous Level</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Return">
                                <label class="form-label col-form-label">Return reasons</label>
                                <div class="input-group">
                                    <select data-tasktype="Return" data-taskid="RETURN{{taskid}}" class="form-select mobile"
                                        data-tasktype="Return">
                                        <option value="">NA</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row my-3">
                            <div class="approval-controls" data-tasktype="Return">
                                <label class="form-label col-form-label  {{isReturnComments}}">Comments</label>
                                <div class="input-group">
                                    <textarea data-tasktype="Return" data-taskid="RETURN{{taskid}}" class="form-control mobile"
                                        data-tasktype="Return" {{isReturnComments}}></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="text-center Return-OK">
                            <button class="btn btn-sm btn-primary shadow-sm mobile"
                                onclick="return axTasksObj.doReturn('{{taskid}}', this, '{{tasktype}}');">Return</button>
                        </div>
                    </div>
                </div>
            </div>
        </li>`;
        this.mobileTaskButtons.make = `
        <li>
            <a href="javascript:void(0)" title="Make" onclick="axTasksObj.openProcess('{{processname}}','{{keyfield}}','{{keyvalue}}','{{taskid}}')">
                <span class="material-icons material-icons-style material-icons-2">
                    <span class="material-symbols-outlined">assignment</span>
                </span><span class="TA-name">Make </span>
            </a>

        </li>`
        this.mobileTaskButtons.view = `
        <li>
            <a href="javascript:void(0)" title="View Form" onclick="axTasksObj.openTstruct('{{transid}}','{{recordid}}')">

                <span class="material-icons material-icons-style material-icons-2">
                    <span class="material-symbols-outlined" style="color: darkmagenta;">visibility</span>
                </span><span class="TA-name">View Form </span>
            </a>

        </li>`
        this.mobileTaskButtons.history = `
        <li>
            <a href="javascript:void(0)" title="View History" onclick="axTasksObj.openHistory('{{processname}}','{{keyfield}}','{{keyvalue}}')"  style="color: blue;">
                <span class="material-icons material-icons-style material-icons-2">
                    <span class="material-symbols-outlined" style="color: blue;">history</span>
                </span><span class="TA-name">View History </span>
            </a>

        </li>`
        this.mobileTaskButtons.check = `
        <li>
            <a href="javascript:void(0)" title="Check" onclick="axTasksObj.doCheck('{{taskid}}')"  style="color: blue;">
                <span class="material-icons material-icons-style material-icons-2">
                    <span class="material-symbols-outlined" style="color: #47BE7D;">done_all</span>
                </span><span class="TA-name">Check </span>
            </a>

        </li>`
        this.dataSources = [];
        this.processName = '';
        this.task = {};
        this.task.keyField = '';
        this.task.keyValue = '';
        this.task.taskName = '';
        this.task.taskType = '';
        this.task.taskId = '';
    }

    fetchActiveTasks(name) {
        let _this = this;
        let url = "../../aspx/AxPEG.aspx/GetActiveTasks";
        let data = {};
        if (_this.isAxpertFlutter) {
            url = "../../ARM_APIs/api/v1/ARMGetActiveTasks";
        }
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.dataSources[name] = dataResult;
            }
        });
    }

    connectToAxpert() {
        let _this = this;
        let url = "../../AxAuth/api/v1/ARMConnectToAxpert";
        let data = {};
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                if (dataResult?.result?.success == true) {
                    sessionStorage["connectedToAxpert"] = true;
                }
                else {
                    showAlertDialog("Error", "Error in connecting to Axpert.");
                }
            }
        });
    }

    fetchProcessTask(name) {
        let _this = this;
        let url = "../../aspx/AxPEG.aspx/AxGetProcessTask";
        if (_this.isAxpertFlutter) {
            url = "../../ARM_APIs/api/v1/ARMGetProcessTask";
        }
        let _task = this.task;

        let data = { taskId: _task.taskId };
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                let dataResult = _this.dataConvert(json, "ARM");
                _this.dataSources[name] = dataResult;
            }
        });
    }

    checkComments(type, taskId, elem) {
        let isMobile = (elem?.classList?.contains("mobile") ? ".mobile" : ":not(.mobile)");
        let tempType = type.toLowerCase();
        tempType = (tempType == "approve") ? "approval" : tempType;
        let taskProps = axTasksObj.dataSources.TaskReasons[taskId];
        if (taskProps[`${tempType}comments`] == "T" && document.querySelector(`textarea[data-taskid="${type}${taskId}"]${isMobile}`).value == "") {
            if (!document.querySelector(`a[data-popover="${type}${taskId}"]${isMobile}`)?.classList.contains("show")) {
                setTimeout(function () {
                    document.querySelector(`a[data-popover="${type}${taskId}"]${isMobile}`).click();
                }, 100);
            }

            document.querySelector(`textarea[data-taskid="${type}${taskId}"]${isMobile}`).focus();
            showAlertDialog("Error", "Comments are mandatory.");
            return false;
        }
        return true;
    }
    doApprove(taskId, elem) {
        let isValidComments = this.checkComments('APPROVE', taskId, elem);
        if (!isValidComments)
            return;

        this.doTaskAction('APPROVE', taskId, 'APPROVE', elem);
        return true;
    }

    doReject(taskId, elem) {
        let isValidComments = this.checkComments('REJECT', taskId, elem);
        if (!isValidComments)
            return;

        this.doTaskAction('REJECT', taskId, 'APPROVE', elem);
    }

    doReturn(taskId, elem, taskType) {
        let isValidComments = this.checkComments('RETURN', taskId, elem);
        if (!isValidComments)
            return;

        this.doTaskAction('RETURN', taskId, taskType.toUpperCase(), elem);
    }

    doCheck(taskId) {
        this.doTaskAction('CHECK', taskId, 'CHECK');
    }

    doTaskAction(action, taskId, taskType, elem) {
        let isMobile = (elem?.classList?.contains("mobile") ? ".mobile" : ":not(.mobile)");
        let _this = this;
        ShowDimmer(true);
        let url = "../../aspx/AxPEG.aspx/AxDoTaskAction";
        if (_this.isAxpertFlutter) {
            url = "../../ARM_APIs/api/v1/ARMDoTaskAction";
        }

        let taskReason = "";
        let taskText = "";
        if (taskType == "APPROVE") {
            taskReason = document.querySelector(`select[data-taskid="${action}${taskId}"]${isMobile}`)?.value || '';
            taskText = document.querySelector(`textarea[data-taskid="${action}${taskId}"]${isMobile}`)?.value || '';
        }
        let toUser = "";
        if (action == "RETURN") {
            toUser = document.querySelector(`select[name="returnto_${taskId}"]${isMobile}`)?.value || '';
        }
        let data = { action: action, taskId: taskId, taskType: taskType, statusReason: taskReason, statusText: taskText, toUser: toUser };
        this.callAPI(url, data, true, result => {
            ShowDimmer(false);
            if (result.success) {
                let json = JSON.parse(result.response);
                if (!_this.isAxpertFlutter) {
                    json = JSON.parse(json.d);
                }
                if (json.result.success) {
                    this.showSuccess(json.result.message);
                    if (!this.isUndefined(parent.axProcessObj)) {
                        parent.axProcessObj.taskCompleted = true;
                        parent.axProcessObj.refreshProcess(parent.axProcessObj.keyValue);
                    }
                    else {
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000)
                    }
                }
                else {
                    this.catchError(json.result.message);
                }
            }
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

    catchError(error) {
        showAlertDialog("error", error);
    };

    showSuccess(message) {
        showAlertDialog("success", message);
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
                if (!this.isAxpertFlutter)
                    data = JSON.parse(data.d);
                if (data.result.success) {
                    if (!this.isUndefined(data.result.data)) {
                        try {
                            return JSON.parse(data.result.data);
                        } catch (e) {
                            return data.result.data;
                        }
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

    showTasks() {
        if (!this.dataSources.hasOwnProperty('TaskReasons')) {
            this.dataSources["TaskReasons"] = {};
        }

        if (this.dataSources["Tasks"]?.length == undefined || this.dataSources["Tasks"].length == 0) {
            if (this.isUndefined(parent.axProcessObj)) {
                document.querySelector(`#${this.view}_view`).innerHTML = "";
                document.querySelector(`#${this.view}_view`).insertAdjacentHTML("beforeend", ` ${this.taskNoRecordsCardHtml} `);
            }
            else {
                document.querySelector(`#${this.view}_view`).innerHTML = "";
                document.querySelector(`#${this.view}_view`).insertAdjacentHTML("beforeend", ` ${this.taskNotFoundHtml} `);
            }
        }
        else {
            document.querySelector(`#${this.view}_view`).innerHTML = "";
            this.dataSources["Tasks"].forEach((rowData, idx) => {
                if (!this.isNullOrEmpty(rowData.taskstatus)) {
                    let htmlText = `<span>This task has been <b>{{taskstatus}}</b> by <b>{{username}}</b> on <b>{{eventdatetime}}.</b></span>`;
                    if (!this.isNullOrEmpty(rowData.statusreason)) {
                        htmlText += `<span>Reason: <b>{{statusreason}}</b></span>`;
                    }
                    if (!this.isNullOrEmpty(rowData.statustext)) {
                        htmlText += `<span>Comments: <b>{{statustext}}</b></span>`;
                    }

                    htmlText = Handlebars.compile(htmlText)(rowData);
                    document.querySelector(`#${this.view}_view`).insertAdjacentHTML("beforeend", ` ${htmlText} `);
                    return;
                }
                else if (!this.isNullOrEmpty(rowData.ispending)) {
                    let htmlText = `<span>This task has been pending with user(s):  <b>{{touser}}</b> from <b>{{eventdatetime}}.</b></span>`;
                    if (!this.isNullOrEmpty(rowData.statusreason)) {
                        htmlText += `<span>Reason: <b>{{statusreason}}</b></span>`;
                    }
                    if (!this.isNullOrEmpty(rowData.statustext)) {
                        htmlText += `<span>Comments: <b>{{statustext}}</b></span>`;
                    }

                    htmlText = Handlebars.compile(htmlText)(rowData);
                    document.querySelector(`#${this.view}_view`).insertAdjacentHTML("beforeend", ` ${htmlText} `);
                    return;
                }

                switch (rowData.tasktype.toUpperCase()) {
                    case "MAKE":
                        rowData.task_buttons_html = this.taskBtnsHtml.make + this.taskBtnsHtml.history;
                        rowData.mobile_task_buttons_html = this.mobileTaskButtons.make + this.mobileTaskButtons.history;
                        break;
                    case "CHECK":
                        rowData.task_buttons_html = this.taskBtnsHtml.check + this.taskBtnsHtml.return + this.taskBtnsHtml.view + this.taskBtnsHtml.history;
                        rowData.mobile_task_buttons_html = this.mobileTaskButtons.check + this.mobileTaskButtons.return + this.mobileTaskButtons.view + this.mobileTaskButtons.history;

                        break;
                    case "APPROVE":
                        rowData.task_buttons_html = this.taskBtnsHtml.approve + this.taskBtnsHtml.reject + (rowData?.returnable == 'T' ? this.taskBtnsHtml.return : "") + this.taskBtnsHtml.view + this.taskBtnsHtml.history;
                        rowData.mobile_task_buttons_html = this.mobileTaskButtons.approve + this.mobileTaskButtons.reject + (rowData?.returnable == 'T' ? this.mobileTaskButtons.return : "")  + this.mobileTaskButtons.view + this.mobileTaskButtons.history;
                        break;
                }

                this.dataSources.TaskReasons[rowData.taskid] = { approvalcomments: rowData.approvalcomments, rejectcomments: rowData.rejectcomments, returncomments: rowData.returncomments };

                //rowData.task_buttons_html += this.taskBtnsHtml.view + this.taskBtnsHtml.history;
                //rowData.mobile_task_buttons_html += this.mobileTaskButtons.view + this.mobileTaskButtons.history;

                //rowData.task_buttons_html = `<div class="d-flex align-items-center task-more-btns">${rowData.task_buttons_html}</div>`;
                let htmlText = "";

                if (rowData.tasktype.toUpperCase() == "APPROVE") {
                    //document.querySelector(`[data-taskid="${rowData.taskid}"] .Task-content`).classList.remove('d-none');
                    rowData.isApprovalComments = (rowData.approvalcomments == "T" ? `required` : "");
                    rowData.isRejectComments = (rowData.rejectcomments == "T" ? `required` : "");
                    rowData.isReturnComments = (rowData.returncomments == "T" ? `required` : "");
                }
                else if (rowData.tasktype.toUpperCase() == "CHECK") {
                    rowData.isReturnComments = (rowData.returncomments == "T" ? `required` : "");
                }

                if (this.view == "List") {
                    htmlText = Handlebars.compile(this.taskListHtml.replace("{{task_buttons_html}}", rowData.task_buttons_html).replace("{{mobile_task_buttons_html}}", rowData.mobile_task_buttons_html))(rowData);
                }
                else {
                    htmlText = Handlebars.compile(this.taskCardHtml.replace("{{task_buttons_html}}", rowData.task_buttons_html).replace("{{mobile_task_buttons_html}}", rowData.mobile_task_buttons_html))(rowData);
                }
                document.querySelector(`#${this.view}_view`).insertAdjacentHTML("beforeend", ` ${htmlText} `);

                if (rowData.tasktype.toUpperCase() == "APPROVE") {
                    if (!this.isNullOrEmpty(rowData.approvereasons)) {
                        var approvereasons = rowData.approvereasons.split(',');
                        document.querySelectorAll(`select[data-taskid="APPROVE${rowData.taskid}"]`).forEach((select) => {
                            while (select.options.length > 0) {
                                select.remove(0);
                            }
                            for (var i = 0; i < approvereasons.length; i++) {
                                var opt = document.createElement('option');
                                opt.value = approvereasons[i];
                                opt.innerHTML = approvereasons[i];
                                select.appendChild(opt);
                            }
                        })


                    }
                    else {
                        document.querySelectorAll(`select[data-taskid="APPROVE${rowData.taskid}"]`).forEach((select) => {
                            select.closest('.row').classList.add("d-none");
                        })
                    }

                    if (!this.isNullOrEmpty(rowData.defapptext)) {
                        document.querySelectorAll(`textarea[data-taskid="APPROVE${rowData.taskid}"]`).forEach((textarea) => {
                            textarea.value = rowData.defapptext;
                        });
                    }

                    if (!this.isNullOrEmpty(rowData.rejectreasons)) {
                        var rejectreasons = rowData.rejectreasons.split(',');
                        document.querySelectorAll(`select[data-taskid="REJECT${rowData.taskid}"]`).forEach((select) => {
                            while (select.options.length > 0) {
                                select.remove(0);
                            }
                            for (var i = 0; i < rejectreasons.length; i++) {
                                var opt = document.createElement('option');
                                opt.value = rejectreasons[i];
                                opt.innerHTML = rejectreasons[i];
                                select.appendChild(opt);
                            }
                        });
                    } else {
                        document.querySelectorAll(`select[data-taskid="REJECT${rowData.taskid}"]`).forEach((select) => {
                            select.closest('.row').classList.add("d-none");
                        });
                    }

                    if (!this.isNullOrEmpty(rowData.defregtext)) {
                        document.querySelectorAll(`textarea[data-taskid="REJECT${rowData.taskid}"]`).forEach((textarea) => {
                            textarea.value = rowData.defregtext;
                        })
                    }
                }
                if (rowData.tasktype.toUpperCase() == "APPROVE" || rowData.tasktype.toUpperCase() == "CHECK") {
                    if (!this.isNullOrEmpty(rowData.returnreasons)) {
                        var returnreasons = rowData.returnreasons.split(',');
                        document.querySelectorAll(`select[data-taskid="RETURN${rowData.taskid}"]`).forEach((select) => {
                            while (select.options.length > 0) {
                                select.remove(0);
                            }
                            for (var i = 0; i < returnreasons.length; i++) {
                                var opt = document.createElement('option');
                                opt.value = returnreasons[i];
                                opt.innerHTML = returnreasons[i];
                                select.appendChild(opt);
                            }
                        })
                    } else {
                        document.querySelectorAll(`select[data-taskid="RETURN${rowData.taskid}"]`).forEach((select) => {
                            select.closest('.row').classList.add("d-none");
                        })
                    }

                    if (!this.isNullOrEmpty(rowData.defrettext)) {
                        document.querySelectorAll(`textarea[data-taskid="RETURN${rowData.taskid}"]`).forEach((textarea) => {
                            textarea.value = rowData.defrettext;
                        })
                    }
                }
            });

            if (!this.isUndefined(parent.axProcessObj)) {
                document.querySelectorAll(".List_view").forEach((item) => {
                    item.click();
                });
            }

            this.constructFilters();
            this.setApprovalReasons();
        }

        /*var showChar = 20;  // How many characters are shown by default
        var ellipsesText = "...";
        var moreText = "Show more >";
        var lessText = "Show less";

        document.querySelectorAll('.more').forEach((taskContent) => {
            taskContent.classList.add('hideContent');
            taskContent.insertAdjacentHTML("afterend", ` <span class="moreellipses">${ellipsesText}&nbsp;</span><span class="morecontent">&nbsp;&nbsp;<a href="javascript:void(0)" class="morelink show-more">${moreText}</a></span>`);
        });

        $(".show-more").on("click", function () {
            var $this = $(this);
            var $content = $this.closest('.task-description').find(".more");
            var linkText = $this.text().toUpperCase();

            if (linkText === moreText.toUpperCase()) {
                linkText = lessText;
                $content.toggleClass("hideContent", "showContent", 400);
            } else {
                linkText = moreText;
                $content.toggleClass("showContent", "hideContent", 400);
            };

            $this.text(linkText);
        });*/

        //$('.more').each(function () {
        //    var content = $(this).html();

        //    if (content.length > showChar) {

        //        var c = content.substr(0, showChar);
        //        var h = content.substr(showChar, content.length - showChar);

        //        var html = c + '<span class="moreellipses">' + ellipsesText + '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="javascript:void(0)" class="morelink">' + moreText + '</a></span>';

        //        $(this).html(html);
        //    }

        //});

        //$(".morelink").click(function () {
        //    if ($(this).hasClass("less")) {
        //        $(this).removeClass("less");
        //        $(this).html(moreText);
        //    } else {
        //        $(this).addClass("less");
        //        $(this).html(lessText);
        //    }
        //    $(this).parent().prev().toggle();
        //    $(this).prev().toggle();
        //    return false;
        //});        
    }

    constructFilters() {
        document.querySelector(".all-tasks-container").innerHTML = this.filters.allTasksHtml;
        let btnAll = document.querySelector("#All-tasks-count");
        btnAll.closest('.task-process-selected').classList.add("active")
        if (!this.isUndefined(btnAll) && !this.isNull(btnAll)) {
            btnAll.innerHTML = this.dataSources["Tasks"].length.toString();
        }
        if (this.dataSources["Tasks"].length > 0) {
            var processWiseCounts = axTasksObj.dataSources["Tasks"].reduce((p, c) => {
                var name = c.processname;
                if (!p.hasOwnProperty(name)) {
                    p[name] = 0;
                }
                p[name]++;
                return p;
            }, {});

            document.querySelector("#Active-List-container").innerHTML = "";

            let tempSno = 1;
            for (const [key, value] of Object.entries(processWiseCounts)) {
                let rowData = { processname: key, processid: this.sanitizeElementId(key), processcount: value, sno: tempSno };
                var filterHtml = Handlebars.compile(this.filters.processFilterCardHtml)(rowData);
                document.querySelector("#Active-List-container").insertAdjacentHTML("beforeend", ` ${filterHtml} `);
                if (tempSno == 6)
                    tempSno = 1;
                else
                    tempSno++;
            }

            var processWiseCounts = {}
            axTasksObj.dataSources["Tasks"].forEach((row) => {

                if (!processWiseCounts.hasOwnProperty(row.processname)) {
                    processWiseCounts[row.processname] = {};
                }

                if (!processWiseCounts[row.processname].hasOwnProperty(row.tasktype)) {
                    processWiseCounts[row.processname][row.tasktype] = 0;
                }

                processWiseCounts[row.processname][row.tasktype]++;
            });

            for (const [processname, types] of Object.entries(processWiseCounts)) {
                for (const [type, count] of Object.entries(types)) {
                    let rowData = { type: type, count: count, processname: processname, icon: this.getTaskTypeIcon(type) };
                    var filterHtml = Handlebars.compile(this.filters.typeFilterCardHtml)(rowData);
                    document.querySelector(`.accordion-body[data-process="${processname}"]`).insertAdjacentHTML("beforeend", ` ${filterHtml} `);
                }
            }

            KTMenu.createInstances('[data-kt-menu="true"]')
        }
    }

    sanitizeElementId(id) {
        return id.replace(/[^a-zA-Z0-9-_]/g, '');
    }

    getTaskTypeIcon(taskType) {
        let result = "assignment";
        switch (taskType.toUpperCase()) {
            case "MAKE":
                result = "assignment";
                break;
            case "CHECK":
                result = "done_all";
                break;
            case "APPROVE":
                result = "check_circle";
                break;
            case "REJECT":
                result = "cancel";
                break;
            case "RETURN":
                result = "reply";
                break;
            default:
                result = "assignment";
                break;
        }
        return result;
    }

    openTstruct(transId, recordId) {
        let url = "";
        if (this.isAxpertFlutter) {
            this.checkAxpertConnection();
            url = `${axpertUrl}/aspx/AxMain.aspx?authKey=AXPERT-${armSessionId}&pname=t${transId}`;
            if (!this.isNullOrEmpty(recordId) && recordId != "0")
                url += `&params=~act=load~recordid=${recordId}`;
            else {
                showAlertDialog("error", "Form details not available.");
                return;
            }
        }
        else {
            url = `../../aspx/tstruct.aspx?transid=${transId}`;
            if (!this.isNullOrEmpty(recordId) && recordId != "0")
                url += `&act=load&recordid=${recordId}`;
            else {
                showAlertDialog("error", "Form details not available.");
                return;
            }
        }

        try {
            let modalObj = {
                "id": `ldb${transId}${recordId}`,
                "url": url
            };
            this.openModal(modalObj);
        } catch (error) {
            showAlertDialog("error", error.message);
        }
    }

    openHistory(processName, keyField, keyValue) {
        let url = "";
        if (this.isAxpertFlutter) {
            this.checkAxpertConnection();
            url = `${axpertUrl}/aspx/AxMain.aspx?authKey=AXPERT-${armSessionId}&pname=iad___pth&params=~pkeyvalue=${keyValue}~pprocess=${processName}`;
        }
        else {
            url = `../../aspx/ivtoivload.aspx?ivname=ad___pth&pkeyvalue=${keyValue}&pprocess=${processName}`;
        }
        try {
            let modalObj = {
                "id": `ldb${keyValue}`,
                "url": url
            };
            this.openModal(modalObj);
        } catch (error) {
            showAlertDialog("error", error.message);
        }
    }

    openModal(modalObj) {
        try {
            modalObj.iFrameModalBody = `<iframe id="${modalObj.id}" name="${modalObj.id}" class="col-12 flex-column-fluid w-100 h-100" src="${modalObj.url}" frameborder="0" allowtransparency="True"></iframe>`

            let myModal = new BSModal(`modal_${modalObj.id}`, "", modalObj.iFrameModalBody,
                (opening) => { ShowDimmer(false); }, (closing) => { }
            );

            myModal.changeSize("fullscreen");
            myModal.hideHeader();
            myModal.hideFooter();
            myModal.showFloatingClose();
        } catch (error) {
            showAlertDialog("error", error.message);
        }
    }

    getUrlParams() {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        this.task.keyField = urlParams.get('keyfield');
        this.task.keyValue = urlParams.get('keyvalue');
        this.processName = urlParams.get('processname');
        this.task.taskType = urlParams.get('tasktype');
        this.task.taskId = urlParams.get('taskid');
        this.task.taskName = urlParams.get('taskname');
    }

    openProcess(processName, keyField, keyValue, taskId) {
        ShowDimmer(true);
        if (this.isAxpertFlutter) {
            this.checkAxpertConnection();
            window.location.href = `${axpertUrl}/aspx/AxMain.aspx?authKey=AXPERT-${armSessionId}&pname=hAxProcessFlow&params=~processname=${processName}~keyfield=${keyField}~keyvalue=${keyValue}`;
        }
        else {
            callParentNew(`LoadIframe(htmlPages.aspx?loadcaption=AxProcessFlow&processname=${processName}&keyfield=${keyField}&keyvalue=${keyValue}&taskid=${taskId})`, "function");
        }
    }

    checkAxpertConnection() {
        if (typeof sessionStorage["connectedToAxpert"] == "undefined") {
            this.connectToAxpert();
        }
    }

    filterTasks(type, value, elem) {
        if (type == "process") {
            document.querySelectorAll(".task-listing-card").forEach((task) => {
                if (task.dataset.process == value) {
                    task.classList.remove('d-none');
                }
                else {
                    task.classList.remove('d-none');
                    task.classList.add('d-none');
                }
            })

            document.querySelectorAll(".task-process-selection.active,.sub-task-lists.active,.task-process-selected.active").forEach((filter) => {
                filter.classList.remove('active');
            })

            //document.querySelectorAll(".sub-task-lists.active").forEach((filter) => {
            //    filter.classList.remove('active');
            //})

            elem.querySelectorAll(".task-process-selection").forEach((title) => {
                title.classList.add('active');
            })

            elem.closest(".task-process-selected").classList.add('active')

        }
        else if (type == "type") {
            let tempProcess = value.split("~~")[0];
            let tempType = value.split("~~")[1];

            document.querySelectorAll(".task-listing-card").forEach((task) => {
                if (task.dataset.type == tempType && task.dataset.process == tempProcess) {
                    task.classList.remove('d-none');
                }
                else {
                    task.classList.remove('d-none');
                    task.classList.add('d-none');
                }
            })
            document.querySelectorAll(".sub-task-lists.active").forEach((filter) => {
                filter.classList.remove('active');
            })
            elem.classList.add('active');
        }
        else if (type == "all") {
            document.querySelectorAll(".task-listing-card").forEach((task) => {
                task.classList.remove('d-none');
                task.classList.remove('active');
            })

            document.querySelectorAll(".task-process-selection.active,.sub-task-lists.active,.task-process-selected.active").forEach((filter) => {
                filter.classList.remove('active');
            })

            //document.querySelectorAll(".sub-task-lists.active").forEach((filter) => {
            //    filter.classList.remove('active');
            //})

            elem.querySelectorAll(".task-process-selection").forEach((title) => {
                title.classList.add('active');
            })

            elem.closest(".task-process-selected").classList.add('active');

            //document.querySelectorAll('.accordion-header:not(.collapsed)').forEach((item) => {
            //    item.classList.add('collapsed');
            //})
        }

    }

    viewAllFilters() {
        let expanded = event.target.classList.contains('expanded');
        if (!expanded) {
            document.querySelector('#Active-List-container').classList.remove('horizontal');
            event.target.setAttribute('title', 'Collapse');
            event.target.innerHTML = 'Collapse';
            event.target.classList.add('expanded');
        }
        else {
            document.querySelector('#Active-List-container').classList.add('horizontal');
            event.target.setAttribute('title', 'View All');
            event.target.innerHTML = 'View All';
            event.target.classList.remove('expanded');
        }
    }

    setApprovalReasons() {
        var radios = document.querySelectorAll(".approval-radio");
        for (var i = 0; i < radios.length; i++) {
            radios[i].addEventListener('change', function () {
                var name = this.getAttribute('name');
                var id = this.getAttribute('id');
                document.querySelectorAll(`.${name}`).forEach((tr) => {
                    //tr.classList.remove('d-none');
                    tr.classList.remove('active');
                    //tr.classList.add('d-none');
                })

                this.closest('.approval-radio-group')?.querySelectorAll(`.btn-toggle`)?.forEach((item) => {
                    item.classList.remove('active');
                })

                if (this.checked) {
                    //document.querySelector(`.${name}[data-tasktype="${this.dataset.tasktype}"]`)?.classList?.add('active');
                    document.querySelectorAll(`.${name} .approval-controls`).forEach((controls) => {
                        if (!controls.classList.contains("d-none")) {
                            controls.classList.add("d-none")
                        }
                    });

                    document.querySelectorAll(`.${name} .approval-controls[data-tasktype="${this.dataset.tasktype}"]`).forEach((controls) => {
                        controls.classList.remove("d-none")
                    });

                    document.querySelector(`[for="${id}"]`).classList.add("active");
                }
            });
        }
    }

    toggleFilters() {
        let filter = document.querySelector(".filter-wrapper");
        if (filter.classList.contains("d-none")) {
            filter.classList.remove("d-none");
        }
        else {
            filter.classList.add("d-none");
        }
    }

    toggleViews(elem) {
        let currentView = localStorage["ActiveListView"];
        if (this.isUndefined(currentView)) {
            currentView = "List";
        }

        if (elem.getAttribute("value") == currentView) {
            return;
        }

        if (currentView == "Card") {
            this.showListView();
        }
        else {
            this.showCardView();
        }
    }

    showViews() {
        let currentView = localStorage["ActiveListView"];

        if (this.isUndefined(currentView) || currentView == "List") {
            this.showListView();
        }
        else {
            this.showCardView();
        }
    }

    showListView() {
        localStorage["ActiveListView"] = "List";
        this.view = "List";
        document.querySelectorAll(".task-view").forEach((view) => {
            view.classList.remove('active');
        });
        document.querySelector("#list-view")?.classList?.add("active");

        if (!document.querySelector("#Card_view").classList.contains('d-none')) {
            document.querySelector("#Card_view").classList.add('d-none');
            document.querySelector("#Card_view").innerHTML = "";
        }
        document.querySelector("#List_view").classList.remove('d-none');

        this.showTasks();
    }

    showCardView() {
        localStorage["ActiveListView"] = "Card";
        this.view = "Card";
        document.querySelectorAll(".task-view").forEach((view) => {
            view.classList.remove('active');
        });
        document.querySelector("#card-view")?.classList?.add("active");

        if (!document.querySelector("#List_view").classList.contains('d-none')) {
            document.querySelector("#List_view").classList.add('d-none');
            document.querySelector("#List_view").innerHTML = "";
        }
        document.querySelector("#Card_view").classList.remove('d-none');
        this.showTasks();
    }

    toggleTask(taskId) {
        document.querySelector(`.arrow${taskId}`).classList.toggle('rotate');
        document.querySelectorAll(`.taskSubrow${taskId}`).forEach((view) => {
            view.classList.toggle('d-none');
            if (!view.classList.contains("data-fetched")) {
                let contentHtml = view.dataset["displaycontent"];
                if (contentHtml.indexOf('{') > -1 || contentHtml.indexOf('[') > -1) {
                    let taskContainer = view.closest("table");
                    let axHtmlObj = new AxHTML();
                    axHtmlObj.parse({ name: taskId, input: view.dataset["displaycontent"], data: {}, pageVariables: { "taskid": taskId, "tasktype": taskContainer.getAttribute("data-type"), "transid": taskContainer.getAttribute("data-transid"), "keyvalue": taskContainer.getAttribute("data-keyvalue") }, container: `.taskSubrow${taskId} span` });
                }
                else {
                    document.querySelector(`.taskSubrow${taskId} span`).insertAdjacentHTML("beforeend", ` ${contentHtml} `)
                }
                view.classList.add('data-fetched');
            }
        });
    }
}

var axTasksObj;
$(document).ready(function () {
    //ShowDimmer(true);
    axTasksObj = new AxTasks();
    axTasksObj.getUrlParams();
    let _tasks = axTasksObj.task;
    if (axTasksObj.isNullOrEmpty(_tasks.taskName) || axTasksObj.isUndefined(_tasks.taskName)) {
        axTasksObj.fetchActiveTasks("Tasks");
    }
    else {
        document.querySelector("#ActiveTask_Left").classList.remove('d-flex');
        document.querySelector("#ActiveTask_Left").style.display = "none";
        axTasksObj.fetchProcessTask("Tasks");
        if (!axTasksObj.isUndefined(axTasksObj.dataSources?.["Tasks"]?.[0]?.["displaytitle"])) {
            document.querySelector("#task-page-title").innerHTML = axTasksObj.dataSources["Tasks"][0]["displaytitle"];
        }
    }
    axTasksObj.showViews();
    ShowDimmer(false);
    if (typeof parent.axProcessObj != "undefined") {
        parent.ShowDimmer(false);
        document.querySelectorAll(".task-title > span").forEach((item) => {
            item.onclick = "#";
        })
    }

    $("#AllTasks").click(function () {
        var filterBox = document.querySelector(".task-activity-container");
        if (filterBox.classList.contains('d-none')) {
            filterBox.classList.remove('d-none');
        }
        else {
            filterBox.classList.add('d-none');
        }

        document.getElementById('task-page-title').click();
    });
});
