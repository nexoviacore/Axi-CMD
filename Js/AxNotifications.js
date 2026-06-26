var notifications = [];
//var clientId = null;
var loginToken = null;
var notifyCount;
var newNotifications;
var reversedNotifications;
var apiUrl;
var apiSessionId;
var apiToken;
var signalrServiceinit;
var notificationtrigger;
var pendingCount = 0;
var _thisClickCount = 0;
var readNotifications = [];
var signalRNotificationIds = [];
$(document).ready(function () {
    if (typeof signalRNotifications != "undefined" && signalRNotifications == "true") {
        $.ajax({
            type: "POST",
            url: "../aspx/Axpeg.aspx/AxGetARMNotificationDetails",
            cache: false,
            async: true,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (data) {
                try {
                    var dataParse = data.d;
                    try {
                        if (dataParse.startsWith("Error")) {
                            showAlertDialog("error", dataParse);
                            ShowDimmer(false);
                            return;
                        }
                    }
                    catch { }

                    apiUrl = dataParse.url;
                    apiSessionId = dataParse.sessionId;
                    apiToken = dataParse.token;

                    //clientId = mainUserName;
                    //clientId = "ARM-" + mainProject + "-" + mainUserName;
                    loginToken = apiToken
                    signalrServiceinit = new SignalrService();
                    signalrServiceinit.startConnection();
                    if (typeof callParentNew("pwdExpDaysAlert") != "undefined" && callParentNew("pwdExpDaysAlert") > 0) {
                        const connectionCheckInterval = setInterval(function () {
                            if (typeof signalrServiceinit !== 'undefined' && signalrServiceinit.connectionId) {
                                clearInterval(connectionCheckInterval);
                                setTimeout(function () {
                                    let messageType = 'Password Expiry';
                                    let messageText = 'Your login password will expire within ' + callParentNew("pwdExpDaysAlert") + ' days';
                                    ASB.WebService.GenerateSignalRNotification(messageType, messageText);
                                }, 0);
                            }
                        }, 100);
                    }
                }
                catch { }
            },
            error: function (er) {
                //showAlertDialog("error", "Error occurred in ARM/Notification Hub connection.");
                showAlertDialog("error", appGlobalVarsObject.lcm[572]);            
                ShowDimmer(false);
            }
        });


    }
});

class SignalrService {
    constructor() {
        this.data = null;
        this.connectionId = null;
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(apiUrl + "/notificationHub?userId=" + mainUserName + "&project=" + mainProject, { accessTokenFactory: () => loginToken })
            .withAutomaticReconnect()
            .build();

        this.hubConnection.on("ReceiveNotification", (message) => {
            this.constructNotification(message);
        });
        this.hubConnection.onclose(err => console.log("Disconnected", err));
        this.hubConnection.onreconnecting(() => console.log("Reconnecting..."));
        this.hubConnection.onreconnected(id => console.log("Reconnected with ID:", id));
    }

    constructNotification(message) {

        try {
            var notificationId = message.split("♠*♠")[0];

            if (signalRNotificationIds.indexOf(notificationId) > -1)
                return;

            signalRNotificationIds.push(notificationId);
            message = message.split("♠*♠")[1];
            newNotifications = JSON.parse(message);
            newNotifications[0].notificationId = notificationId;
            var incommingCount = newNotifications.length;
            pendingCount = incommingCount;
            var incomingNotificationCount = newNotifications.length;
            notifications = notifications.concat(newNotifications);
            if (notifications.length > 0)
                pendingCount = notifications.length;

            AxNotifyHistory = "";//JSON.stringify(notifications);

            // alert(message);
            notificationtrigger = document.querySelector('#notificationtrigger');
            notificationtrigger.classList.add("blink");
            notifyCount = document.querySelector('#notifyCountForBlink');
            notifyCount.classList.remove('d-none')
            notifyCount.classList.add("blink");

            var existingNotificationFloat = document.querySelector('.floatnotification');

            if (existingNotificationFloat) {
                existingNotificationFloat.remove();
            }

            var notificationfloat = document.createElement('div');
            notificationfloat.classList.add('floatnotification', 'd-flex', 'gap-1');
            var notificationfloatSpan = document.createElement('span');
            notificationfloatSpan.classList.add('floatnotificationSpan', 'text-truncate');
            var notificationClose = document.createElement('button');
            notificationClose.classList.add('notificationClose');
            notificationClose.innerHTML = '&times'
            var flotnotificationCount = document.createElement('span');
            flotnotificationCount.classList.add('flotnotificationCount');
            //  pendingCount = notifications.length;
            if (pendingCount <= 1) {
                flotnotificationCount.innerHTML = "";// pendingCount;
            } else {
                pendingCount = pendingCount - 1;
                flotnotificationCount.innerHTML = '+' + pendingCount + 'Notifications';
            }

            if (pendingCount >= 0) {
                var lastNotification = notifications[pendingCount];
                if (lastNotification !== undefined) {
                    notificationfloatSpan.textContent = lastNotification.title;
                    notificationfloatSpan.title = lastNotification.title;
                }
                else {
                    notificationfloatSpan.textContent = notifications[0].title;
                    notificationfloatSpan.title = notifications[0].title;
                }
            }
            notificationfloat.appendChild(notificationfloatSpan);
            notificationfloat.appendChild(notificationClose);
            notificationfloat.appendChild(flotnotificationCount);

            var bodyAppend = document.querySelector('body');

            bodyAppend.appendChild(notificationfloat);
            setTimeout(function () {
                var notificationfloat = document.querySelector('.floatnotification');
                notificationfloat.remove();
            }, 5000);

            var flotnotificationCount = document.querySelector('.flotnotificationCount');

            flotnotificationCount.addEventListener('click', function () {
                var removeElement = document.querySelector('.floatnotification');
                var notificationtrigger = document.querySelector('#notificationtrigger');
                if (removeElement) {
                    removeElement.remove();
                }
                signalrServiceinit.onclick();
                var menuElement = document.querySelector("#notificationtrigger");
                var menu = KTMenu.getInstance(menuElement);
                var item = document.querySelector("#notificationWindowMain");
                menu.show(item);
                item.style.transform = "translate(370%, 45px)";
                signalrServiceinit.notificationEvent();

                notificationtrigger.classList.remove("blink");
                notifyCount.classList.remove("blink");
                notifyCount.classList.add('d-none');
            });
            var bodyclick = document.querySelector('.cardsPageWrapper');
            bodyclick.addEventListener('click', function () {
                var menuElement = document.querySelector("#notificationtrigger");
                var menu = KTMenu.getInstance(menuElement);
                var item = document.querySelector("#notificationWindowMain");
                menu.hide(item);
            })
            var notificationCloseEvent = document.querySelector('.notificationClose');
            notificationCloseEvent.addEventListener('click', function () {
                var notificationfloat = document.querySelector('.floatnotification');
                notificationfloat.remove();
            });
        }
        catch (ex) {
            console.log("Exception occurred in ReceiveNotification: " + ex);
        }
    }

    startConnection() {
        this.hubConnection
            .start()
            //.then(() => this.getConnectionId())
            .then(() => console.log("Connected with ID:", this.hubConnection.connectionId))
            .catch(err => console.log('Error while starting connection: ' + err))
    }
    //getConnectionId() {
    //    this.hubConnection.invoke('GetConnectionId')
    //        .then((data) => {
    //            this.connectionId = data;
    //            //this.passConnectionId(clientId, data);
    //            //this.getMessage();
    //        })
    //        .then(() => console.log("GetConnectionId completed - " + this.connectionId));
    //}
    //passConnectionId(userId, connectionId) {
    //    this.hubConnection.invoke("PassConnectionId", userId, connectionId)
    //        .catch(err => console.error(err));
    //}
    //getMessage() {

    //}
    init() {
        this.onclick();
        this.notificationEvent();
    }
    onclick() {
        if (_thisClickCount == 0) {
            //GetHistoryFromCache();
        }
        _thisClickCount++;
        AxNotifyHistory = "";//JSON.stringify(notifications);
        var notificationWindow = document.querySelector('#notificationWindow');
        if (notifications.length === 0) {
            notificationWindow.innerHTML = '';
            var nodata = document.createElement('span');
            notificationWindow.classList.add('d-flex');
            nodata.innerHTML = 'No notification available';
            notificationWindow.appendChild(nodata);
            notifyCount = document.querySelector('#notifyCountForBlink');
            notifyCount.classList.add("d-none");
        } else {
            //pendingCount = 0;
            notificationWindow.innerHTML = '';
            notificationWindow.classList.remove('d-flex');
            notifyCount = document.querySelector('#notifyCountForBlink');
            notifyCount.classList.remove('d-none');
            notifyCount.classList.add("blink");
            var index;
            for (var i = notifications.length - 1; i >= 0; i--) {
                index = i;
                var notification = notifications[i];
                let _isReadNoti = false;
                if (readNotifications.indexOf(notification.dt) > -1) {
                    _isReadNoti = true;
                }
                var notificationDiv = document.createElement("div");
                if (_isReadNoti)
                    notificationDiv.classList.add('d-flex', 'align-items-center', 'notificationList', 'col-12', 'px-4', 'py-2', 'border-bottom-dashed', 'border-1', 'border-gray-300', 'min-h-70px', 'mh-150px', 'bg-light');
                else
                    notificationDiv.classList.add('d-flex', 'align-items-center', 'notificationList', 'col-12', 'px-4', 'py-2', 'border-bottom-dashed', 'border-1', 'border-gray-300', 'min-h-70px', 'mh-150px');
                notificationDiv.setAttribute('data-index', index);

                var closebtnAppend = document.createElement("div");
                closebtnAppend.classList.add('d-flex', 'justify-content-end', 'closebtnAppend', 'col-1');
                var notificationcloseBtn = document.createElement('button');
                notificationcloseBtn.innerHTML = `<span class='material-icons material-icons-style fs-3' onclick='deleteNotification("${notification.notificationId}")'>close</span>`;
                notificationcloseBtn.setAttribute('data-index', index);
                notificationcloseBtn.classList.add('btn', 'btn-sm', 'btn-icon', 'btn-white', 'closeNotification');
                notificationcloseBtn.title = 'Clear';
                //notificationcloseBtn.onclick = function () {
                //    clearOneNotification();
                //}
                closebtnAppend.appendChild(notificationcloseBtn)

                var symbolDiv = document.createElement("div");
                symbolDiv.classList.add("symbol", "symbol-50px", "col-2");

                if (notification.type.toLowerCase() == 'tstruct') {
                    if (notification.icon == '')
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/NotificationIcons/TstructSave.png" class="w-30px"></span>`;
                    else
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/` + notification.icon + `" class="w-30px"></span>`;
                } else if (notification.type.toLowerCase() == 'iview') {
                    if (notification.icon == '')
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/NotificationIcons/Defalut.png" class="w-30px"></span>`;
                    else
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/` + notification.icon + `" class="w-30px"></span>`;
                } else if (notification.type.toLowerCase() == 'page') {
                    if (notification.icon == '')
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/NotificationIcons/Defalut.png" class="w-30px"></span>`;
                    else
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/` + notification.icon + `" class="w-30px"></span>`;
                } else if (notification.type.toLowerCase() == 'export') {
                    if (notification.icon == '')
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/NotificationIcons/Defalut.png" class="w-30px"></span>`;
                    else
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/` + notification.icon + `" class="w-30px"></span>`;
                } else if (notification.type.toLowerCase() == 'password expiry') {
                    if (notification.icon == '')
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/NotificationIcons/Notification.png" class="w-30px"></span>`;
                    else
                        symbolDiv.innerHTML = `<span class="symbol-label"><img src="../images/` + notification.icon + `" class="w-30px"></span>`;
                }

                var textDiv = document.createElement("div");
                textDiv.classList.add("d-flex", "notificationText", "flex-column", "col-9");

                var titleH5 = document.createElement("a");
                titleH5.classList.add("notificationText-h5", "title");
                titleH5.title = notification.title;
                titleH5.textContent = notification.title;
                titleH5.setAttribute('data-notificationid', notification.notificationId);
                var notificationLink = notification.link;
                if (notificationLink !== '') {
                    if (_isReadNoti)
                        titleH5.classList.add('notificationLinkClick', "h5", "text-truncate", "mb-0", "fw-normal", "cursor-pointer");
                    else
                        titleH5.classList.add('notificationLinkClick', "h5", "text-truncate", "mb-0", "fw-boldest", "cursor-pointer");
                    if (notificationLink.t == 't') {
                        let _linkparam = 't' + notificationLink.name + '&act=' + notificationLink.act;
                        if (notificationLink.p != '')
                            _linkparam += '&' + notificationLink.p;
                        titleH5.setAttribute('data-link', _linkparam);
                        titleH5.setAttribute('data-axmsgid', notificationLink.axmsgid);
                        titleH5.setAttribute('data-tstname', notificationLink.name);
                    } else if (notificationLink.t == 'i') {
                        let _linkparam = 'i' + notificationLink.name;
                        if (notificationLink.p != '')
                            _linkparam += '&' + notificationLink.p;
                        titleH5.setAttribute('data-link', _linkparam);
                    }
                    else if (notificationLink.t == 'p') {
                        let _linkparam = 'HP' + notificationLink.name;
                        if (notificationLink.p != '')
                            _linkparam += '&' + notificationLink.p;
                        titleH5.setAttribute('data-link', _linkparam);
                    } else if (notificationLink.t == 'e') {
                        let _linkparam = 'e' + notificationLink.name;
                        titleH5.setAttribute('data-link', _linkparam);
                        titleH5.setAttribute('data-axmsgid', notificationLink.axmsgid);
                    }
                    index++;
                }
                var messageA = document.createElement("span");
                messageA.classList.add("notificationText-sapn", "message", "text-truncate");
                messageA.textContent = notification.message;
                messageA.title = notification.message;

                var messageB = document.createElement("span");
                messageB.classList.add("notificationText-dt", "message");
                messageB.textContent = notification.dt;

                textDiv.appendChild(titleH5);
                textDiv.appendChild(messageA);
                textDiv.appendChild(messageB);

                notificationDiv.appendChild(symbolDiv);
                notificationDiv.appendChild(textDiv);
                notificationDiv.appendChild(closebtnAppend);
                notificationWindow.appendChild(notificationDiv);
            }
        }
    }

    notificationEvent() {
        var notificationLinkClick = document.querySelectorAll('.notificationLinkClick');
        var menuElement = document.querySelector("#notificationtrigger");
        var menu = KTMenu.getInstance(menuElement);
        var item = document.querySelector("#notificationWindowMain");
        notificationLinkClick.forEach((ele) => {
            ele.addEventListener('click', function () {
                AxNotifyMsgId = "";
                var clickLink = ele.getAttribute('data-link');
                $(ele).parents('.notificationList').addClass('bg-light');
                $(ele).removeClass('fw-boldest').addClass('fw-normal');
                deleteNotification(ele.getAttribute('data-notificationid'));
                readNotifications.push($(ele).parent().find('.notificationText-dt').text());
                if (clickLink.startsWith('i')) {
                    clickLink = clickLink.substring(1);
                    LoadIframe(`iview.aspx?ivname=${clickLink}`);
                    menu.hide(item);
                } else if (clickLink.startsWith('t')) {
                    clickLink = clickLink.substring(1);
                    AxNotifyMsgId = ele.getAttribute('data-axmsgid');
                    callParentNew("isdummyLoad=", "false");
                    clickLink += "&dummyload=false♠";
                    let _thisappSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                    let _thisstoredKey = 'originaltrIds-' + _thisappSessUrl;
                    let _transidArray = JSON.parse(localStorage.getItem(_thisstoredKey) || '[]');
                    let _thisTrId = clickLink.split('&')[0];
                    if (_transidArray.includes(_thisTrId)) {
                        _transidArray = _transidArray.filter(x => x.toLowerCase() !== _thisTrId.toLowerCase());
                        localStorage.setItem(_thisstoredKey, JSON.stringify(_transidArray));
                    }
                    LoadIframe(`tstruct.aspx?transid=${clickLink}`);
                    menu.hide(item);
                } else if (clickLink.startsWith('HP')) {
                    clickLink = clickLink.substring(2);
                    LoadIframe(`htmlPages.aspx?load=${clickLink}`);
                    menu.hide(item);
                } else if (clickLink.startsWith('e')) {
                    clickLink = clickLink.substring(1);
                    if (clickLink == "import") {
                        const failedMatches = [...$(".notificationText").find("span")[0].title.matchAll(/Failed:(\d+)/g)];
                        const failedValues = failedMatches.map(match => parseInt(match[1]));
                        if ((failedValues.length === 1 && failedValues[0] !== 0) || (failedValues.length > 1 && failedValues.some(val => val !== 0))) {
                            AxNotifyMsgId = ele.getAttribute('data-axmsgid');
                            let myImportModal = new BSModal("ImportData", "Import Data", "<iframe src='../aspx/ImportAll.aspx?axmsgid=" + AxNotifyMsgId + "' class='vw-100 vh-100'></iframe>", () => {
                                //shown callback
                                $(".btn-close").focus();
                            }, () => {
                                //hide callback
                            });

                            myImportModal.changeSize("fullscreen");
                            myImportModal.hideFooter();
                            myImportModal.modalBody.classList.add("p-0");
                            myImportModal.close();
                            menu.hide(item);
                        }
                    }
                    else {
                        if (clickLink != "")
                            DownloadExportFile(clickLink);
                        menu.hide(item);
                    }
                }
            })
        });
        var notificationtrigger = document.querySelector("#notificationtrigger");

        notificationtrigger.addEventListener('mouseover', function () {
            notificationtrigger.classList.remove("blink");
            notifyCount.classList.remove("blink");
            notifyCount.classList.add('d-none');
        });

        notificationtrigger.addEventListener('click', function () {
            notificationtrigger.classList.remove("blink");
            notifyCount.classList.remove("blink");
            notifyCount.classList.add('d-none');
        });


        const closeButtons = document.querySelectorAll('.closeNotification');

        closeButtons.forEach((closeButton) => {
            closeButton.addEventListener('click', (event) => {
                // event.stopPropagation();
                const notificationDiv = closeButton.closest('.notificationList');
                if (notificationDiv) {
                    let data_index = notificationDiv.getAttribute('data-index');
                    data_index = parseInt(data_index, 10);
                    if (!isNaN(data_index) && data_index >= 0 && data_index < notifications.length) {
                        notifications.splice(data_index, 1);
                    }
                    notificationDiv.classList.add('d-none');
                }
            });
        });
    }
    stopConnection() {
        this.hubConnection.stop()
            .then(() => {

                console.log('Connection stopped');
                this.hubConnection = null;
            })
            .catch(err => console.log('Error while stopping connection: ' + err));
    }

}

function DownloadExportFile(eLink) {
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
        document.body.removeChild(anchor);
    } else {
        let filePath = eLink.substring(0, eLink.lastIndexOf('\\') + 1);
        let fileName = eLink.substring(eLink.lastIndexOf('\\') + 1);
        try {
            ASB.WebService.LoadExportFileToScript(filePath, fileName, successfileMoved);
        }
        catch (ex) { }
    }
}

function successfileMoved(result, eventArgs) {
    if (CheckSessionTimeout(result))
        return;
    var fileUrl = result;
    var fileName = result.substring(result.lastIndexOf('/') + 1);
    var anchor = document.createElement('a');
    anchor.href = fileUrl;
    anchor.download = fileName;
    document.body.appendChild(anchor);
    anchor.click();
    document.body.removeChild(anchor);
}
function clearAll() {
    deleteAllNotifications()
    var notificationWindow = document.querySelector('#notificationWindow');
    notificationWindow.innerHTML = '';
    var nodata = document.createElement('span');
    notificationWindow.classList.add('d-flex');
    nodata.innerHTML = 'No notification available';
    notificationWindow.appendChild(nodata);
    notifications = [];
    var menuElement = document.querySelector("#notificationtrigger");
    menuElement.click();
}

function GetHistoryFromCache() {
    $.ajax({
        type: "POST",
        url: "../aspx/Axpeg.aspx/AxGetNotifyHistory",
        cache: false,
        async: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != "") {
                let _newNotifications = JSON.parse(data.d);
                for (var i = 0; i < _newNotifications.length; i++) {
                    readNotifications.push(_newNotifications[i].dt);
                }
                if (notifications.length > 0) {
                    let thisnotifications = notifications;
                    notifications = [];
                    notifications = notifications.concat(_newNotifications);
                    notifications = notifications.concat(thisnotifications);
                } else
                    notifications = notifications.concat(_newNotifications);
            }
        }
    });
}

function removeSavedtranNotify(_transid, _recId) {
    try {
        if (_recId == "" || _recId == "0") {
            for (var i = notifications.length - 1; i >= 0; i--) {
                var notification = notifications[i];
                if (notification.notifytype == "cachedsave" && notification.link.name == _transid && notification.link.p == "") {
                    notifications.splice(i, 1);
                }
            }
        } else {
            for (var i = notifications.length - 1; i >= 0; i--) {
                var notification = notifications[i];
                if (notification.notifytype == "cachedsave" && notification.link.name == _transid && notification.link.p.indexOf(_recId) > -1) {
                    notifications.splice(i, 1);
                }
            }
        }
    } catch (ex) { }
}

function deleteNotification(notificationId) {
    var url = apiUrl + "/api/v1/DeleteNotification";
    var input = { userId: mainUserName, project: mainProject, notificationId: notificationId };
    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        async: true,
        data: JSON.stringify(input),
        contentType: "application/json;charset=utf-8",
        dataType: "json"
    });
}

function deleteAllNotifications() {
    var url = apiUrl + "/api/v1/DeleteAllNotifications";
    var input = { userId: mainUserName, project: mainProject };
    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        async: false,
        data: JSON.stringify(input),
        contentType: "application/json;charset=utf-8",
        dataType: "json"
    });
}
