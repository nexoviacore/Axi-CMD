class HomePageCards {
    constructor() {
        this.html = {
            HomePageCards: `
			<div class="mt-3 widgetWrapper htmlDomWrapper col-lg-3 col-md-6 col-sm-12 col-xs-12" id="HP_cards_list" data-cardname="{{caption}}">
                    <div class="Home-cards-list" style="background:{{colorcode}}">
                        <div class="Hp-card-icon attendicon">
                            <img alt="" class="w-60px cardsIcons" src="../images/homepageicon/{{caption}}.png" onerror="this.onerror=null;this.src='../images/homepageicon/default.png';" onclick="navigateFromCard('{{caption}}', '{{stransid}}')">
                        </div>
                        <span id="attendancetittle" class="Hp-card-title text-truncate" style="cursor:pointer" title="{{caption}}" onclick="LoadIframe('{{target}}')">{{caption}}</span><p title="{{carddesc}}" class="min-h-45px mh-45px multiline-ellipsis">{{carddesc}}</p>
                        <!--<a href="javascript:void(0)" class="Hp-more-btn d-none1{{isMoreOption}}" data-fetching="rowcount-{{rowCount}}" id="Hp-more-btnid"
                            data-kt-menu-trigger="{default:'click', lg: 'click'}" data-kt-menu-overflow="true"
                            data-kt-menu-placement="bottom-end" title="{{caption}}" onclick="objHomePageCards.moreOptionsPopover('{{rowCount}}')">
                            <span class="material-icons material-icons-style">more_vert</span>
                        </a>-->
                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-title-gray-700 menu-icon-muted menu-active-bg menu-state-primary fw-bold py-4 fs-6 w-300px " id="rowcount-{{rowCount}}" data-cardname="{{caption}}" data-rowcount="{{id}}" 
                        data-kt-menu="true" data-icon="{{displayicon}}">
                        
                        </div>
                    </div>
            </div>
            `,
            HomePageMenuOptionCards: `
			<div class="mt-3 widgetWrapper htmlDomWrapper col-lg-3 col-md-6 col-sm-12 col-xs-12" id="HP_cards_list" data-cardname="{{caption}}">
                    <div class="Home-cards-list" style="background:{{colorcode}}">
                        <div class="Hp-card-icon attendicon">
                            <img alt="" class="w-60px cardsIcons" src="../images/homepageicon/{{caption}}.png" onerror="this.onerror=null;this.src='../images/homepageicon/default.png';" onclick="navigateFromCard('{{caption}}', '{{stransid}}')">
                        </div>
                        <span id="attendancetittle" class="Hp-card-title text-truncate" style="cursor:pointer" title="{{caption}}" onclick="navigateFromCard('{{caption}}','{{stransid}}')">{{caption}}</span><p title="{{carddesc}}" class="min-h-45px mh-45px multiline-ellipsis">{{carddesc}}</p>
                        <a href="javascript:void(0)" class="Hp-more-btn d-none1{{isMoreOption}}" data-fetching="rowcount-{{rowCount}}" id="Hp-more-btnid"
                            data-kt-menu-trigger="{default:'click', lg: 'click'}" data-kt-menu-overflow="true"
                            data-kt-menu-placement="bottom-end" title="{{caption}}" onclick="objHomePageCards.moreOptionsPopover('{{rowCount}}')">
                            <span class="material-icons material-icons-style">more_vert</span>
                        </a>
                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-title-gray-700 menu-icon-muted menu-active-bg menu-state-primary fw-bold py-4 fs-6 w-300px " id="rowcount-{{rowCount}}" data-cardname="{{caption}}" data-rowcount="{{id}}" 
                        data-kt-menu="true" data-icon="{{displayicon}}">
                        
                        </div>
                    </div>
            </div>
            `,
            HTMLPanel: `
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3 mt-3 widgetWrapper htmlDomWrapper">
                        <div class="card rounded-1 h-100 shadow-sm">
                            <div class="card-body p-0 h-300px heightControl pt-0---">
                                <bodyContent></bodyContent>
                            </div>
                        </div>
                    </div>
                    `
        };
        this.arrMoreOptions = [];
        this.arrDsNames = [];
        this.dataSources = {};
    }
    init() {
        this.getHomePageCards();
    }

    getHomePageCards() {
        let _this = this;
        let url = "../aspx/AxPEG.aspx/AxGetHomePageCards";
        let data = {};
        this.callAPI(url, data, false, result => {
            if (result.success) {
                let json = JSON.parse(result.response);
                if (json.d == 'Error in Home Configuration.') {
                    showAlertDialog("error", 'Error in Home Configuration.');
                    return;
                }
                if (typeof json.d != "undefined" && json.d.indexOf("The given key 'ConnectionString' was not present in the dictionary.") > -1) {
                    showAlertDialog("error", 'Error in ARM connection.');
                    return;
                }
                let dataResult = _this.dataConvert(json, "ARM");
                if (dataResult.length > 0) {
                    dataResult = _this.JSON_Lower_keys(dataResult);
                    dataResult = dataResult.sort((a, b) => {
                        const nameA = a.grppageid || '';
                        const nameB = b.grppageid || '';
                        return nameA.localeCompare(nameB);
                    });
                    _this.constructCards(dataResult);
                }
                else {
                    if (dataResult.d == 'Error in ARM connection.') {
                        showAlertDialog("error", 'Error in ARM connection.');
                    }
                    else {
                        const tabContainer = document.querySelector(`.HomePageMenuOptionCards`);
                        if (typeof tabContainer != "undefined" && tabContainer != null) {
                            tabContainer.innerHTML = '';
                            tabContainer.appendChild(`<h1>Home Page configuration is not available.</h1>`);
                        }
                    }
                }
            }
        });

    }

    JSON_Lower_keys(_thisJSON) {
        var _finalJson = [];
        _thisJSON.forEach(function (val) {
            var ret = {};
            $.map(val, function (value, key) {
                ret[key.toLowerCase()] = value;
            });
            _finalJson.push(ret);
        });
        return _finalJson;
    }

    constructCards(cardData) {
        let _this = this;
        _this.constructMenuFolderCards(cardData.filter(card => card.paneltype === "Menu folder"));
        _this.constructHTMLPanels(cardData.filter(card => card.paneltype === "HTML panel"));
        _this.constructMenuOptionCards(cardData.filter(card => card.paneltype === "Menu option"));

        try {
            AxAfterConstructCards();

        }
        catch { }
    }

    constructMenuFolderCards(cardData) {
        let _this = this;
        let generatedHTML = '';
        let generatedHTMLGroup = '';
        let curgrppageid = '';
        let listviewAsDefault = [];
        try {
            listviewAsDefault = document.querySelector("#listviewAsDefault").value;
            listviewAsDefault = JSON.parse(listviewAsDefault);
            if (listviewAsDefault.length > 0) {
                var _tempJson = [];
                try {
                    listviewAsDefault.forEach(function (val) {
                        var ret = {};
                        $.map(val, function (value, key) {
                            ret[key.toLowerCase()] = value;
                        });
                        _tempJson.push(ret);
                    });
                } catch (ex) { }

                listviewAsDefault = _tempJson;
            }
        }
        catch { }

        cardData.forEach((rowData, index) => {
            try {
                if (rowData.target && rowData.target.indexOf("tstruct.aspx") > -1) {

                    if (typeof axOldModelFlag != "undefined" && axOldModelFlag == "true") {
                        if (listviewAsDefault.length > 0) {
                            if (openListviewConf = listviewAsDefault.filter(list => list.structname == findGetParameter("transid", rowData.target))?.[0]) {
                            } else if (openListviewConf = listviewAsDefault.filter(list => list.structname == "ALL Forms")?.[0]) {
                            }

                            if (openListviewConf?.propsval == "false") {
                                rowData.target += `&openerIV=${findGetParameter("transid", rowData.target)}&isIV=false`;
                            } else {
                                rowData.target = rowData.target.replace("tstruct.aspx?transid=", "iview.aspx?ivname=") + "&tstcaption=" + rowData.caption;
                            }
                        } else {
                            rowData.target = rowData.target.replace("tstruct.aspx?transid=", "iview.aspx?ivname=") + "&tstcaption=" + rowData.caption;
                        }
                    }
                    else {
                        if (listviewAsDefault.length > 0) {
                            let openListviewConf = listviewAsDefault.filter(list => list.structname == findGetParameter("transid", rowData.target))?.[0];
                            if (!openListviewConf)
                                openListviewConf = listviewAsDefault.filter(list => list.structname == "ALL Forms")?.[0]

                        if (openListviewConf?.propsval == "false") {
                            rowData.target += `&openerIV=${findGetParameter("transid", rowData.target)}&isIV=false`;
                        } else {
                            rowData.target = rowData.target.replace("tstruct.aspx?transid=", "Entity.aspx?tstid=");
                        }
                    } else {
                        rowData.target = rowData.target.replace("tstruct.aspx?transid=", "Entity.aspx?tstid=");
}
                    }
                }
            } catch (ex) { }

            if (rowData.grppageid == '' || rowData.grppageid == null)
                generatedHTML += Handlebars.compile(this.html["HomePageCards"])(rowData);
            else {
                if (rowData.grppageid != curgrppageid) {
                    if (generatedHTMLGroup != '')
                        generatedHTMLGroup += `</div></div></div></div>`;
                    const divOpen = `<div class="card rounded-1 shadow-sm mt-3 col-md-3"><div class="menucard-wrapper"><div class="card-header min-h-40px px-6"><div class="card-title"><h4 class="fw-bolder">` + rowData.groupfolder + `</h4></div></div><div class="card-body px-6 py-2 mb-3"><div class="row d-flex">`;
                    generatedHTMLGroup += divOpen + Handlebars.compile(this.html["HomePageCards"])(rowData);
                } else
                    generatedHTMLGroup += Handlebars.compile(this.html["HomePageCards"])(rowData);
                curgrppageid = rowData.grppageid;
            }
        });
        if (generatedHTMLGroup != '')
            generatedHTMLGroup += `</div></div></div></div>`;
        if (generatedHTML != '') {
            let divOpen = `<div class="card rounded-1 shadow-sm mt-3"><div class="card-body px-6 py-2 mb-3"><div class="row d-flex">` + generatedHTML + `</div></div></div>`;
            divOpen += generatedHTMLGroup;
            const tabContainer = document.querySelector(`.HomePageCards`);
            tabContainer.innerHTML = divOpen;
        } else {
            const tabContainer = document.querySelector(`.HomePageCards`);
            tabContainer.innerHTML = generatedHTMLGroup;
        }
    }

    constructHTMLPanels(htmlPanels) {
     
        let _this = this;
        htmlPanels.forEach((htmlPanel, index) => {
            try {
                htmlPanel = AxBeforeHTMLPanelRender(htmlPanel);
            }
            catch { }

            if (htmlPanel?.cardhide != "T") {
                let panelElement = $(_this.html.HTMLPanel).attr("htmlpanel-index", index).data("htmlpanel-index", index);
                panelElement.attr("htmlpanel-name", htmlPanel["caption"]).data("htmlpanel-name", htmlPanel["caption"]);
                $(".HtmlPanelCards .row").append(panelElement);
                panelElement.find("bodyContent").replaceWith(`<div class="body-content"></div>`);
                var shadowDomEle = $(panelElement.find(".body-content"))[0];
                let panelHtml = htmlPanel["html_editor_card"];

                var shadowAtt = shadowDomEle.attachShadow({ mode: 'open' });
                shadowAtt.innerHTML = panelHtml;
            }

            try {
                htmlPanel.index = index;
                htmlPanel = AxAfterHTMLPanelRender(htmlPanel);
            }
            catch { }

        });

    }


    constructMenuOptionCards(cardData) {
        let generatedMenuOptionHTML = '';
        cardData.forEach((rowData, index) => {
            const moreOption = rowData.moreoption;
            var stransid = rowData.stransid;
            const rowCount = index;
            rowData.rowCount = rowCount;

            if (this.inValid(moreOption)) {
                rowData.isMoreOption = '';
                this.arrMoreOptions.push('');
            } else {
                rowData.isMoreOption = 'T';
                const replacedMoreOption = moreOption.replace(/{{stransid}}/g, `"${stransid}"`);
                this.arrMoreOptions.push(replacedMoreOption);
            }
            this.arrDsNames.push(rowData.datasource);
            generatedMenuOptionHTML += Handlebars.compile(this.html["HomePageMenuOptionCards"])(rowData);
        });

        if (generatedMenuOptionHTML != '') {
            let divOpen = `<div class="card rounded-1 shadow-sm mt-3"><div class="card-body px-6 py-2 mb-3"><div class="row d-flex">` + generatedMenuOptionHTML + `</div></div></div>`;
            const tabContainer = document.querySelector(`.HomePageMenuOptionCards`);
            tabContainer.innerHTML = divOpen;
        } else {
            const tabContainer = document.querySelector(`.HomePageMenuOptionCards`);
            tabContainer.classList.add("d-none")
        }
        try {

        }
        catch { }
    }

    moreOptionsPopover(count) {
        var popover = document.querySelector(`[data-fetching^="rowcount-${count}"]`);
        if (!popover.classList.contains("data-fetched")) {
            const attributeValue = popover.getAttribute("data-fetching");
            const indexCheck = "rowcount-";
            const dataIndexValue = attributeValue.substring(indexCheck.length);


            const createShowElement = document.createElement('div');
            const popoverShowing = document.querySelector(`[id^="rowcount-${count}"]`);
            const cardname = popoverShowing.getAttribute("data-cardname");
            var cardHeader = `<div class="card-header">
        <h3 class="card-title ">
            <img alt="" class="w-60px cardsIcons" src="../images/homepageicon/${cardname}.png" onerror="this.onerror=null;this.src='../images/homepageicon/default.png';">	${cardname}
        </h3>
    </div>`;

            if (this.arrDsNames[dataIndexValue] == null || this.arrDsNames[dataIndexValue] == "") {
                popover.classList.add("data-fetched");

                createShowElement.innerHTML = cardHeader;
                const popoverId = popoverShowing.getAttribute("id");
                const popoverContainer = document.getElementById(popoverId);
                if (popoverContainer) {
                    popoverContainer.appendChild(createShowElement);
                }
                var btnHtmlOutput = "";
                if (!this.inValid(this.arrMoreOptions[dataIndexValue])) {
                    var rowBtnTemplate = `<div class="Popover_Items Action_link ">
                                                <a href="javascript:void(0);" id="{{btnid}}" onclick="navigateFromCard('{{title}}','{{open}}')"> <div class="Action_link_name">{{title}}</div> </a>
                                                </div> `;


                    var rowJsBtnTemplate = `<div class="Popover_Items Action_link ">
                                                <a href="javascript:void(0);" id="{{btnid}}" onclick="{{exejs}}"> <div class="Action_link_name">{{title}}</div> </a>
                                                </div> `;


                    var btnStr = this.arrMoreOptions[dataIndexValue];
                    btnStr = btnStr.replace(/\}\s+\{/g, '}{');
                    var btnArr = this.getBtnArr(btnStr);
                    let _this = this;
                    btnArr.forEach(function (btnObj) {
                        if (!_this.inValid(btnObj.exejs)) {
                            btnHtmlOutput += Handlebars.compile(rowJsBtnTemplate)(btnObj);
                        }
                        else {
                            btnHtmlOutput += Handlebars.compile(rowBtnTemplate)(btnObj);
                        }
                    });
                }
                var htmlOutput = `<div class="card-body">
<div class="hover-scroll-overlay-y ">
<div class="d-flex flex-wrap" id="Popover_Details">${btnHtmlOutput}</div></div> </div>`;
                createShowElement.innerHTML = `<div class="card card-flush h-xl-100 Popover-Wrapper">` + cardHeader + htmlOutput + `</div>`;
                return;
            }

            var dsName = this.arrDsNames[dataIndexValue];
            var cardJson = [];
            if (this.inValid(this.dataSources[dsName])) {
                this.fetchData(dsName);
            }
            cardJson = this.dataSources[dsName];


            if (cardJson.result && cardJson.result[0] && cardJson.result[0].error) {
                var errorMsg = JSON.parse(cardJson.result[0].error.msg).error.msg;
                showAlertDialog("error", errorMsg);
                return;
            }

            var rowTemplateWithoutIcon = `<div class="Popover_Items ">
<a href="javascript:void(0);" onclick="navigateFromCard('{{caption}}','{{link}}')"> 
<span class="Pop_Item_label">{{caption}}</span>
<div class="Pop_items_value">{{text}}</div>
</a>
</div>`

            var htmlOutput = "";
            cardJson.forEach(function (card) {
                if (typeof card.cardname == "undefined" || card.cardname === cardname) {
                    htmlOutput += Handlebars.compile(rowTemplateWithoutIcon)(card);
                }
            });



            var btnHtmlOutput = "";
            if (!this.inValid(this.arrMoreOptions[dataIndexValue])) {
                var rowBtnTemplate = `<div class="Popover_Items Action_link ">
                                                <a href="javascript:void(0);" id="{{btnid}}" onclick="navigateFromCard('{{title}}','{{open}}')"> <div class="Action_link_name">{{title}}</div> </a>
                                                </div> `;


                var rowJsBtnTemplate = `<div class="Popover_Items Action_link ">
                                                <a href="javascript:void(0);" id="{{btnid}}" onclick="{{exejs}}"> <div class="Action_link_name">{{title}}</div> </a>
                                                </div> `;


                var btnStr = this.arrMoreOptions[dataIndexValue];
                btnStr = btnStr.replace(/\}\s+\{/g, '}{');
                var btnArr = this.getBtnArr(btnStr);
                let _this = this;
                btnArr.forEach(function (btnObj) {
                    if (!_this.inValid(btnObj.exejs)) {
                        btnHtmlOutput += Handlebars.compile(rowJsBtnTemplate)(btnObj);
                    }
                    else {
                        btnHtmlOutput += Handlebars.compile(rowBtnTemplate)(btnObj);
                    }
                });
            }

            htmlOutput = `<div class="card-body">
<div class="hover-scroll-overlay-y ">
<div class="d-flex flex-wrap" id="Popover_Details">${htmlOutput}${btnHtmlOutput}</div></div> </div>`;

            createShowElement.innerHTML = `<div class="card card-flush h-xl-100 Popover-Wrapper">` + cardHeader + htmlOutput + `</div>`;

            // Append createShowElement to popoverShowing element
            const popoverId = popoverShowing.getAttribute("id");
            const popoverContainer = document.getElementById(popoverId);
            if (popoverContainer) {
                popoverContainer.appendChild(createShowElement);
            }
        }
        popover.classList.add("data-fetched");
    }

    getBtnArr(str) {
        var btnObjArr = [];
        if (this.inValid(str))
            return btnObjArr;

        str = str.replace(/"(.*?)"/g, function (match, p1) {
            return match.replace(/ /g, '&nbsp;');
        });
        str = str.replaceAll("\"", "");

        str = str.slice(1, -1);

        var components = str.split('}{');
        var btnObjArr = [];
        function convertBtnStrToObj(component) {
            var obj = {};
            var parts = component.split(' ');

            // Extract btnid and type
            obj.btnid = parts[0];
            obj.type = parts[1];

            // Extract additional key-value pairs
            for (var i = 2; i < parts.length; i++) {
                obj[parts[i]] = parts[i + 1].replace(/&nbsp;/g, " ");
                i++;
            }
            btnObjArr.push(obj);
        }
        components.map(convertBtnStrToObj);
        return btnObjArr;

    }

    callAPI(url, data, async, callBack) {
        let _this = this;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, async);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        if (_this.isAxpertFlutter) {
            xhr.setRequestHeader('Authorization', `Bearer ${armToken}`);
            data["ARMSessionId"] = armSessionId;
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

    inValid(elem) {
        return elem == null || typeof elem == "undefined" || elem == "";
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

    fetchData(datasource) {
        let _this = this;
        var result = AxGetSqlData(datasource);
        result = _this.convertGetSQLDataJSON(result);
        result = result?.result[0]?.result?.row;
        _this.dataSources[datasource] = result;
    }

    convertGetSQLDataJSON(inputJson) {
        inputJson = JSON.parse(inputJson);
        const dynamicNodeName = Object.keys(inputJson)[0];
        const data = inputJson[dynamicNodeName].data;

        const outputJson = {
            result: [
                {
                    result: {
                        fields: inputJson[dynamicNodeName].sqlmetaData,
                        row: data.map(item => {
                            const rowObject = {};
                            inputJson[dynamicNodeName].sqlmetaData.forEach((metaData, index) => {
                                rowObject[metaData.name.toLowerCase()] = item[index];
                            });
                            return rowObject;
                        })
                    }
                }
            ]
        };

        return outputJson;
    }

}

function navigateFromCard(caption, id) {
    var form = id;
    document.querySelector('body').click();
    var customNavigate = false;
    try {
        customNavigate = AxCustomNavigateFromCard(caption, id);
    }
    catch (ex) { }

    if (customNavigate)
        return;

    if (form.startsWith('i')) {
        form = id.substring(1);
        LoadIframe(`iview.aspx?ivname=${form}`);
    } else if (form.startsWith('t')) {
        form = id.substring(1);

        let listviewAsDefault = [];
        try {
            listviewAsDefault = document.querySelector("#listviewAsDefault").value;
            listviewAsDefault = JSON.parse(listviewAsDefault);
            if (listviewAsDefault.length > 0) {
                var _tempJson = [];
                try {
                    listviewAsDefault.forEach(function (val) {
                        var ret = {};
                        $.map(val, function (value, key) {
                            ret[key.toLowerCase()] = value;
                        });
                        _tempJson.push(ret);
                    });
                } catch (ex) { }

                listviewAsDefault = _tempJson;
            }
        }
        catch { }


        if (typeof axOldModelFlag != "undefined" && axOldModelFlag == "true") {
            if (listviewAsDefault.length > 0) {
                let openListviewConf = listviewAsDefault.filter(list => list.structname == form)?.[0];
                if (!openListviewConf)
                    openListviewConf = listviewAsDefault.filter(list => list.structname == "ALL Forms")?.[0]

                if (openListviewConf?.propsval == "false") {
                    LoadIframe(`tstruct.aspx?transid=${form}`);
                } else {
                    LoadIframe(`iview.aspx?ivname=${form}&tstcaption=${caption}`);
                }
            } else {
                LoadIframe(`iview.aspx?ivname=${form}&tstcaption=${caption}`);
            }
        } else {
            if (listviewAsDefault.length > 0) {
                let openListviewConf = listviewAsDefault.filter(list => list.structname == form)?.[0];
                if (!openListviewConf)
                    openListviewConf = listviewAsDefault.filter(list => list.structname == "ALL Forms")?.[0]

                if (openListviewConf?.propsval == "false") {
                    LoadIframe(`tstruct.aspx?transid=${form}`);
                } else {
                    LoadIframe(`Entity.aspx?tstid=${form}`);
                }
            } else {
                LoadIframe(`Entity.aspx?tstid=${form}`);
            }
        }        
    } else if (form.startsWith('HP')) {
        form = id.substring(2);
        LoadIframe(`htmlPages.aspx?load=${form}`);
    } else if (form.startsWith('c')) {
        form = id.substring(1);
        LoadIframe(`htmlPages.aspx?loadcaption=${form}`);
    }
}

var objHomePageCards = new HomePageCards();

function AxBeforeHTMLPanelRender(htmlPanel) {
    return htmlPanel;
}

function AxAfterHTMLPanelRender(htmlPanel) {
    return htmlPanel;
}
