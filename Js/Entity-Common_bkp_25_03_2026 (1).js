var dtCulture = eval(callParent('glCulture'));
var advFilterDtCulture = dtCulture == "en-us" ? "MM/DD/YYYY" : "DD/MM/YYYY";

class EntityCommon {
    constructor() { }

    isAppManager() {
        if (callParentNew("isMobile"))
            return true;
        else
            return window.top.$.axpertUI?.options?.axpertUserSettings?.settings?.axUserOptions?.configurationStudio?.display == 'block';
    }

    getInitials(str) {

        let initials = '';

        // Split the string into words
        const words = str.split(' ');

        // Extract initials based on number of words
        if (words.length === 1) {
            if (words[0].length == 1)
                initials = words[0].charAt(0);
            else
                initials = words[0].charAt(0) + words[0].charAt(1);
        } else if (words.length === 2) {
            // If there are two words, get the first character of each word
            initials = words[0].charAt(0) + words[1].charAt(0);
        } else if (words.length >= 3) {
            // If there are three or more words, get the first character of the first two words
            initials = words[0].charAt(0) + words[1].charAt(0);
        }

        return initials.toUpperCase();
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

    inValid(elem) {
        return elem == null || typeof elem == "undefined" || elem == "";
    };

    isValid(elem) {
        return !this.inValid(elem);
    };

    isUndefined(elem) {
        return typeof elem == "undefined";
    };

    setAnalyticsDataWS(data, successCB = () => { }, errorCB = () => { }) {
        const ajaxCall = (data) => {
    
            $.ajax({
                url: top.window.location.href.toLowerCase().substring(0, top.window.location.href.toLowerCase().indexOf("/aspx/")) + '/aspx/Analytics.aspx/SetAnalyticsDataWS',
                type: 'POST',
                cache: false,
                async: true,
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json",
                success: function (response) {
                    if (successCB && typeof successCB === "function") {
                        successCB(response.d || response);
                    }
                },
                error: function (error) {
                    console.error('Error in SetAnalyticsDataWS:', error);
                    if (errorCB && typeof errorCB === "function") {
                        errorCB(error);
                    }
                }
            });
        };

        if (this.isAppManager() && data.confirmNeeded) {    
            $('#confirmationModal').appendTo('body').css('z-index', 1100).modal('show');
       
            $('#btnAllUsers').off('click').on('click', function () {
                data.allUsers = true;
                ajaxCall(data);
                $('#confirmationModal').modal('hide');
            });
    
            $('#btnJustMyself').off('click').on('click', function () {
                data.allUsers = false;
                ajaxCall(data);
                $('#confirmationModal').modal('hide');
            });
        } else {
            ajaxCall(data);
        }
    }
    
    getAnalyticsDataWS(data, successCB = () => { }, errorCB = () => { }) {
        $.ajax({
            url: top.window.location.href.toLowerCase().substring(0, top.window.location.href.toLowerCase().indexOf("/aspx/")) + '/aspx/Analytics.aspx/GetAnalyticsDataWS',
            type: 'POST',
            cache: false,
            async: true,
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (response) {
                if (successCB && typeof successCB == "function") {
                    successCB(response.d || response);
                }
            },
            error: function (error) {
                if (errorCB && typeof errorCB == "function") {
                    errorCB(error);
                }
            }
        });
    }

    setAnalyticsDataSyncWS(data, successCB = () => { }, errorCB = () => { }) {
        const ajaxCall = (data) => {

            $.ajax({
                url: top.window.location.href.toLowerCase().substring(0, top.window.location.href.toLowerCase().indexOf("/aspx/")) + '/aspx/Analytics.aspx/SetAnalyticsDataWS',
                type: 'POST',
                cache: false,
                async: false,
                dataType: 'json',
                data: JSON.stringify(data),
                contentType: "application/json",
                success: function (response) {
                    if (successCB && typeof successCB === "function") {
                        successCB(response.d || response);
                    }
                },
                error: function (error) {
                    console.error('Error in SetAnalyticsDataWS:', error);
                    if (errorCB && typeof errorCB === "function") {
                        errorCB(error);
                    }
                }
            });
        };

        if (this.isAppManager() && data.confirmNeeded) {
           

            $('#confirmationModal').appendTo('body').css('z-index', 1100).modal('show');

            $('#btnAllUsers').on('click', function () {
                data.allUsers = true;
                ajaxCall(data);
                $('#confirmationModal').modal('hide');
            });

            $('#btnJustMyself').on('click', function () {
                data.allUsers = false;
                ajaxCall(data);
                $('#confirmationModal').modal('hide');
            });
        } else {
            ajaxCall(data);
        }
    }

    getAnalyticsDataSyncWS(data, successCB = () => { }, errorCB = () => { }) {
        $.ajax({
            url: top.window.location.href.toLowerCase().substring(0, top.window.location.href.toLowerCase().indexOf("/aspx/")) + '/aspx/Analytics.aspx/GetAnalyticsDataWS',
            type: 'POST',
            cache: false,
            async: false,
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (response) {
                if (successCB && typeof successCB == "function") {
                    successCB(response.d || response);
                }
            },
            error: function (error) {
                if (errorCB && typeof errorCB == "function") {
                    errorCB(error);
                }
            }
        });
    }

    getEntityEditableFlag(inputJson) {
        var editable = false;

        let _this = this;
        let url = "../aspx/Entity.aspx/GetEntityEditableFlagWS";
        this.callAPI(url, inputJson, false, result => {
            if (result.success) {
                if (result.response.indexOf("SessionId is not valid") > -1) {
                    parent.window.location = "../aspx/sess.aspx";
                    editable = true;
                    return;
                }
                if (result.response.indexOf(`:\\"T\\"`) > -1) //To show that user has edit access.
                    editable = true;
            } else {
                editable = false;
            }
        });

        return editable;
    }

    getDataFromDataSource(data, successCB = () => { }, errorCB = () => { }) {

        alert("EntityCommon.getDataFromDataSource method is deprecated. Use EntityCommon.getDataFromAxList method or AxInterface.GetDataFromAxList instead.")
        return;

        if (typeof data.refreshCache == "undefined")
            data.refreshCache = false;
        
        $.ajax({
            url: top.window.location.href.toLowerCase().substring(0, top.window.location.href.toLowerCase().indexOf("/aspx/")) + '/aspx/AxPEG.aspx/GetDataFromDataSource',
            type: 'POST',
            cache: false,
            async: true,
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (response) {
                if (successCB && typeof successCB == "function") {
                    successCB(response.d || response);
                }
            },
            error: function (error) {
                if (errorCB && typeof errorCB == "function") {
                    errorCB(error);
                }
            }
        });
    }

    getDataFromAxList(data, successCB = () => { }, errorCB = () => { }) {
        if (!data) {
            throw new Error("Parameter 'data' is required.");
        }

        // adsNames must exist and be an array
        if (!Array.isArray(data.adsNames)) {
            throw new Error("'adsNames' is required and must be an array.");
        }

        // set defaults if missing
        if (typeof data.refreshCache === "undefined") data.refreshCache = false;
        if (typeof data.keyField === "undefined") data.keyField = "";
        if (typeof data.keyValue === "undefined") data.keyValue = "";
        if (typeof data.sqlParams === "undefined") data.sqlParams = {};
        if (typeof data.props === "undefined") data.props = {};        

        $.ajax({
            url: top.window.location.href.toLowerCase().substring(0, top.window.location.href.toLowerCase().indexOf("/aspx/")) + '/aspx/AxPEG.aspx/GetDataFromAxList',
            type: 'POST',
            cache: false,
            async: true,
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (response) {
                if (successCB && typeof successCB == "function") {
                    successCB(response.d || response);
                }
            },
            error: function (error) {
                if (errorCB && typeof errorCB == "function") {
                    errorCB(error);
                }
            }
        });
    }


    getDatesBasedonSelection(selectionvalue) {
        var fromToObj = { from: "", to: "" };
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
                var dateObj = this.getFirstDayOfWeek(new Date());
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 6);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_weekOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(dateObj.getDate() - 7)
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 6);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_weekOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(dateObj.getDate() + 7)
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 6);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_monthOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_monthOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                dateObj.setMonth(dateObj.getMonth() - 1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(0);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_monthOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
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

    getDatesBasedonSelectionForBetweenFilter(selectionvalue) {
        var fromToObj = { from: "", to: "" };
        var advFilterDtCulture = "DD-MMM-YYYY";
        switch (selectionvalue) {
            case "customOption":
                break;
            case "todayOption":
                var dateObj = new Date();
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "yesterdayOption":
                var dateObj = new Date();
                dateObj.setDate(dateObj.getDate() - 1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "tomorrowOption":
                var dateObj = new Date();
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_weekOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 7);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_weekOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(dateObj.getDate() - 7)
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 7);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_weekOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(dateObj.getDate() + 7)
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setDate(dateObj.getDate() + 7);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_monthOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "last_monthOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                dateObj.setMonth(dateObj.getMonth() - 1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "next_monthOption":
                var dateObj = this.getFirstDayOfWeek(new Date());
                dateObj.setDate(1);
                dateObj.setMonth(dateObj.getMonth() + 1);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 1);
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_quarterOption":
                var dateObj = new Date();
                var thisQuarter = Math.floor(((dateObj).getMonth() + 3) / 3);
                dateObj.setDate(1);
                dateObj.setMonth((thisQuarter * 3) - 3);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setMonth(dateObj.getMonth() + 3);
                dateObj.setDate(dateObj.getDate() + 1);
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
                dateObj.setDate(dateObj.getDate() + 1);
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
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
            case "this_yearOption":
                var dateObj = new Date();
                dateObj.setDate(1);
                dateObj.setMonth(0);
                fromToObj.from = moment(dateObj).format(advFilterDtCulture);
                dateObj.setFullYear(dateObj.getFullYear() + 1);
                dateObj.setMonth(0);
                dateObj.setDate(dateObj.getDate() + 1);
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
                dateObj.setDate(dateObj.getDate() + 1);
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
                dateObj.setDate(dateObj.getDate() + 1);
                fromToObj.to = moment(dateObj).format(advFilterDtCulture);
                break;
        }

        return fromToObj;
    }


    getFirstDayOfWeek(currentDate) {
        currentDate = new Date(currentDate);
        var day = currentDate.getDay();
        var diff = currentDate.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
        return new Date(currentDate.setDate(diff));
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

    loadHyperLink(link,isHyperLink) {
        if (typeof isHyperLink != "undefined" && isHyperLink == "true")
            parent.ShowDimmer(true);
        window.top.LoadIframe(link);
    }

    navigateToUrl(link) {
        window.top.navigateToUrl(link);
    }

    replaceSpecialChars(value) {
        if (value.indexOf(";bkslh") != -1) {
            value = value.replace(new RegExp(";bkslh", "g"), "\\");
        }

        value = value.replace("T00:00:00", "");
        return value;
    }

    getCardData(cardName) {
        if (window.location.href.toLowerCase().indexOf("dashboardtemplate.html") > -1) {
            return $.axpertUI_dashboard?.dashboardcardsPage?.cardsData?.[cardName] || [];
        }
        else {
            return $.axpertUI?.cardsPage?.cardsData?.[cardName] || [];
        }
    }
}

function AxGetGlobalVar() {
    var jsonVal = "";
    $.ajax({
        url: top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/") + 6) + 'axinterface.aspx/GetGlobalVar',
        type: 'POST',
        cache: false,
        async: false,
        dataType: 'json',
        contentType: "application/json",
        success: function (data) {
            if (data.d != "") {
                jsonVal = JSON.parse(data.d);
            }
            else {
                jsonVal = "";
            }
        },
        error: function (error) {
            jsonVal = error;
        }
    });
    return jsonVal;
}
