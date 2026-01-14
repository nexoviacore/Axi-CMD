/**
 * Material HomePage and Cards
 * @author Prashik
 * @Date   2019-08-04T13:21:51+0530
 */
; (function ($, win) {
    $_this = this;
    function axpertUI() {
        options = {
            leftSideBar: {
                menuConfiguration: {
                    menuJson: {},
                    listviewAsDefault: [],
                }
            },
            loader: {
                div: '.page-loader',
                subDiv: '.loader-box-wrapper',
                textDiv: '.page-loader-text',
                parent: '.page-loading',
                stayClass: 'stay-page-loading',
                inAnimationClass: 'animation animation-fade-in',
                outAnimationClass: 'animation animation-fade-out',
                radialGradientClass: 'bg-radial-gradient',
                loaderWrapClass: 'bg-white p-20 shadow rounded'
            },
            dirLeft: true,
            axpertUserSettings: {},
            cardsPage: {
                setCards: false,
                cards: [],
                totalColumns: 12,
                designChanged: false,
                design: [],
                enableMasonry: false,
                html: {
                    wrapper: `
                    <div class="widget-main">
                        <div class="container-fluid">
                            <div class="kpi-main">
                                <div class="row innerCardsPageWrapper">
                                </div>
                            </div>
                        </div>
                    </div>
                    `,
                    loader: `
                    <div class="demo-preloader">
                        <div class="preloader pl-size-sm">
                            <div class="spinner-layer">
                                <div class="circle-clipper left">
                                    <div class="circle"></div>
                                </div>
                                <div class="circle-clipper right">
                                    <div class="circle"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    `,
                    kpi: `
                    <div class="col-xs-12 col-sm-12 col-md-3 col-lg-3 mt-3 widgetWrapper kpiWrapper">
                        <div class="card shadow-sm h-100">
                            <!--begin::Card header-->
                            <div class="card-header border-0">
                                <!--begin::Card title-->
                                <div class="card-title">
                                    <div class="symbol symbol-35px me-2 mainIcon">
                                        <div class="symbol-label">
                                            <span class="material-icons material-icons-style">
                                                <iconContent></iconContent>
                                            </span>
                                        </div>
                                    </div>
                                    <h4 class="fw-bolder mainHeading">
                                        <headerContent></headerContent>
                                    </h4>
                                </div>
                                <!--end::Card title-->
                                <!--begin::Card toolbar-->
                                <div class="card-toolbar d-none">
                                    <a href="javascript:void(0);" class="btn btn-sm btn-flex btn-light-primary">
                                        <statusContent></statusContent>
                                    </a>
                                </div>
                                <!--end::Card toolbar-->
                            </div>
                            <!--end::Card header-->
                            <!--begin::Card body-->
                            <div class="card-body heightControl pt-0">
                                <div class="fw-bolder fs-2">
                                    <div class="">
                                        <bodyContent>No Data Found</bodyContent>
                                    </div>
                                </div>
                            </div>
                            <!--end::Card body-->
                            <div class="card-footer border-0 d-none">
                                <div class="fs-7 fw-normal text-muted">
                                    <footerContent></footerContent>
                                </div>
                            </div>
                        </div>
                    </div>
                    `,
                    chart: `
                    <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4 mt-3 widgetWrapper chartWrapper">
                        <div class="card h-100 shadow-sm">
                            <div class="card-header border-0---">
                                <div class="card-title">
                                    <div class="symbol symbol-35px me-2 mainIcon">
                                        <div class="symbol-label">
                                            <span class="material-icons material-icons-style">
                                                <iconContent></iconContent>
                                            </span>
                                        </div>
                                    </div>
                                    <h4 class="fw-bolder mainHeading">
                                        <headerContent></headerContent>
                                    </h4>
                                </div>
                                <div class="card-toolbar">
                                    <a href="javascript:void(0);" class="d-none btn btn-sm btn-icon btn-icon-primary btn-active-light-primary me-n3" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                        <span class="material-icons material-icons-style">
                                            more_vert
                                        </span>
                                    </a>
                                    <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-200px" data-kt-menu="true" style="">
                                        <div class="menu-item px-3">
                                            <div class="menu-content fs-6 text-dark fw-boldest px-3 py-4">Quick Actions</div>
                                        </div>
                                        <div class="separator mb-3 opacity-75"></div>
                                        <div class="menu-item px-3">
                                            <a href="javascript:void(0);" class="menu-link px-3">New Ticket</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body min-h-300px heightControl pt-0---">
                                <bodyContent>No Data Found</bodyContent>
                            </div>
                        </div>
                    </div>
                    `,
                    list: `
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 mt-3 widgetWrapper listWrapper">
                        <div class="card h-100 shadow-sm">
                            <div class="card-header border-0---">
                                <div class="card-title">
                                    <div class="symbol symbol-35px me-2 mainIcon">
                                        <div class="symbol-label">
                                            <span class="material-icons material-icons-style">
                                                <iconContent></iconContent>
                                            </span>
                                        </div>
                                    </div>
                                    <h4 class="fw-bolder mainHeading">
                                        <headerContent></headerContent>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-body min-h-300px h-300px heightControl pt-0---">
                                <bodyContent>No Data Found</bodyContent>
                            </div>
                        </div>
                    </div>
                    `,
                    menu: `
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 mt-3 widgetWrapper menuWrapper">
                        <div class="card h-100 shadow-sm">
                            <div class="card-header border-0---">
                                <div class="card-title">
                                    <div class="symbol symbol-35px me-2 mainIcon">
                                        <div class="symbol-label">
                                            <span class="material-icons material-icons-style">
                                                <iconContent></iconContent>
                                            </span>
                                        </div>
                                    </div>
                                    <h4 class="fw-bolder mainHeading">
                                        <headerContent></headerContent>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-body heightControl pt-0---">
                                <descriptionContent>
                                    <div class="description pb-5" title="menu">
                                        No Description
                                    </div>
                                </descriptionContent>
                                <bodyContent>No Data Found</bodyContent>
                            </div>
                        </div>
                    </div>
                    `,
                    "modern menu": `
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 widgetWrapper modernMenuWrapper">
                        <modernMenuContent></modernMenuContent>
                    </div>
                    `,
                    modernMenuContent: `
                    <div class="col-lg-2 col-md-2 col-sm-12 col-xs-12 mt-3">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body cursor-pointer">
                                <div class="text text-center header---">
                                    <iconContent></iconContent>
                                    <span class="fw-bolder fs-5 modernMenuName">
                                        <bodyContent></bodyContent>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    `,
                    "image card": `
                    <div class="col-xs-12 col-sm-12 col-md-4 col-lg-4 mt-3 widgetWrapper imageWrapper">
                        <div class="card h-100 shadow-sm">
                            <div class="h-400px heightControl">
                                <imageContent></imageContent>
                            </div>

                            <div class="card-img-overlay p-0 h-100" style="background: rgba(0,0,0,.4);">
                                <div class="card bg-transparent h-100">
                                    <div class="card-header bg-transparent border-0">
                                        <div class="card-title d-flex">
                                            <div class="symbol symbol-35px me-2 mainIcon">
                                                <div class="symbol-label">
                                                    <span class="material-icons material-icons-style">
                                                        <iconContent></iconContent>
                                                    </span>
                                                </div>
                                            </div>
                                            <h4 class="mt-1 fs-1 fw-bolder text-white mainHeading">
                                                <headerContent></headerContent>
                                            </h4>
                                        </div>
                                    </div>

                                    <div class="card-body pt-0">
                                        <div class="fs-3">
                                            <div class="text-white">
                                                <bodyContent>No Data Found</bodyContent>
                                            </div>
                                            
                                        </div>
                                    </div>

                                    <div class="card-footer bg-transparent border-0 d-none">
                                        <div class="fs-7 fw-normal text-white text-muted">
                                            <footerContent></footerContent>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </div>
                    </div>
                    `,
                    calendar: `
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 mt-3 widgetWrapper calendarWrapper">
                        <div class="card h-100 shadow-sm">
                            <div class="card-header border-0---">
                                <div class="card-title">
                                    <div class="symbol symbol-35px me-2 mainIcon">
                                        <div class="symbol-label">
                                            <span class="material-icons material-icons-style">
                                                <iconContent></iconContent>
                                            </span>
                                        </div>
                                    </div>
                                    <h4 class="fw-bolder mainHeading">
                                        <headerContent></headerContent>
                                    </h4>
                                </div>
                                <div class="d-none card-toolbar">
                                    <a href="javascript:void(0);" class="btn btn-sm btn-icon btn-icon-primary btn-active-light-primary me-n3" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                                        <span class="material-icons material-icons-style">
                                            more_vert
                                        </span>
                                    </a>
                                    <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bolder w-200px" data-kt-menu="true" style="">
                                        <div class="menu-item px-3">
                                            <div class="menu-content fs-6 text-dark fw-boldest px-3 py-4">Quick Actions</div>
                                        </div>
                                        <div class="separator mb-3 opacity-75"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body min-h-600px h-600px heightControl pt-0---">
                                <bodyContent>No Data Found</bodyContent>
                            </div>
                            <div class="card-footer py-2 border-0--- d-flex justify-content-evenly d-none">
                                <div class="fs-7 fw-normal text-white text-muted">
                                    <footerContent></footerContent>
                                </div>
                            </div>
                        </div>
                    </div>
                    `,
                    "html": `
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6 mt-3 widgetWrapper htmlDomWrapper">
                        <div class="card h-100 shadow-sm">
                            <div class="d-none card-header border-0---">
                                <div class="card-title">
                                    <div class="symbol symbol-35px me-2 mainIcon">
                                        <div class="symbol-label">
                                            <span class="material-icons material-icons-style">
                                                <iconContent></iconContent>
                                            </span>
                                        </div>
                                    </div>
                                    <h4 class="fw-bolder mainHeading">
                                        <headerContent></headerContent>
                                    </h4>
                                </div>
                            </div>
                            <div class="card-body p-0 min-h-300px heightControl pt-0---">
                                <bodyContent>No Data Found</bodyContent>
                            </div>
                        </div>
                    </div>
                    `,
                    masonryWidth: `
                    <div class="grid-sizer col-lg-1 col-md-1 col-sm-12 col-xs-12"></div><div class="clearfix"></div>
                    `,
                    break: `
                    <div class="clearfix"></div>
                    `
                }
            },
            navigation: {
                backButton: {
                    div: ".appBackBtn"
                }
            },
            isHybrid: false,
            isMobile: false,
        }

        /* cardsPage - Function ================================================================================================
        *  You can manage the cards page here
        *
        */
        cardsPage = {
            activate(options) {
                let _this = this;
                if (options == "loadhomepage") {
                    _this.options.cards = _this._refreshData() || _this.options.cards;
                    _this.options = _this.options;
                } else if (options) {
                    _this.options = options;
                } else {
                    _this.options = _this.options;
                }
                _this.currentColumnWidth = 0;
                _this.firstTime = true;
                _this._checkSetCardsAndInit();

                _this._showCardsFrame(_this.options.setCards);

                if (_this.options.setCards) {
                    _this._sortCardsObject();
                    _this._drawCardsLayout();
                }
                delete _this.currentColumnWidth;

                _this.firstTime = false;
            },
            _refreshData(axp_cardsid, singleLoad = false) {
                let _this = this;
                let result = "";
                $.ajax({
                    url: 'mainnew.aspx/refreshCards',
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: JSON.stringify({
                        json: (axp_cardsid || _this.options.cards.map(card => card["axp_cardsid"]).join(",") || ""),
                        isJSON: false,
                        singleLoad
                    }),
                    dataType: 'json',
                    contentType: "application/json",
                    success: function (data) {
                        if (data && data.d) {
                            try {
                                result = (JSON.parse(data.d !== "" ? data.d : "[]",
                                    function (k, v) {
                                        try {
                                            return (typeof v === "object" || isNaN(v) || v.toString().trim() === "") ? v : (typeof v == "string" && (v.startsWith("0") || v.startsWith("-")) ? parseFloat(v, 10) : JSON.parse(v));
                                        } catch (ex) {
                                            return v;
                                        }
                                    }
                                ) || []).map(arr => _.mapKeys(arr, (value, key) => key.toString().toLowerCase()));
                            } catch (ex) { }
                        }
                    },
                    error: function (error) {

                    }
                });
                return result;
            },
            _checkSetCardsAndInit() {
                let _this = this;
                let showEditButton = _this.options.cards.length > 0;
                if (!showEditButton) {
                    $(_this.options.staging.cardsFrame.editSaveButton).addClass("d-none");
                }
            },
            _showCardsFrame(showCards) {
                let _this = this;
                setTimeout(() => {
                    if (showCards && _this.options.setCards) {
                        $(_this.options.staging.iframes).addClass("d-none");
                        $(_this.options.staging.cardsFrame.div).removeClass("d-none");

                        _this.editSaveCardDesignToggle(true);
                    } else {
                        $(_this.options.staging.iframes).removeClass("d-none");
                        $(_this.options.staging.cardsFrame.div).addClass("d-none");
                    }
                }, 0);

                if (!_this.options.setCards) {
                    $(_this.options.staging.cardsFrame.divControl).addClass("d-none");
                }
            },
            editSaveCardDesignToggle(reset = false) {
                let _this = this;
                let isEdit = $(_this.options.staging.cardsFrame.cardsDesigner).hasClass("d-none");

                if ($(isEdit && _this.options.staging.cardsFrame.div).hasClass("d-none")) {
                    $(_this.options.staging.iframes).addClass("d-none");
                    $(_this.options.staging.cardsFrame.div).removeClass("d-none");
                }

                if (!reset && isEdit) {
                    $(_this.options.staging.cardsFrame.cardsDiv).addClass("d-none");
                    $(_this.options.staging.cardsFrame.cardsDesigner).removeClass("d-none");
                    $(_this.options.staging.cardsFrame.cardsDesignerToolbar).removeClass("d-none");
                    $(_this.options.staging.cardsFrame.editSaveButton).removeClass("btn-default").addClass("btn-primary");
                    $(_this.options.staging.cardsFrame.editSaveButton + " " + _this.options.staging.cardsFrame.icon).text("save");
                } else {
                    $(_this.options.staging.cardsFrame.cardsDiv).removeClass("d-none");
                    $(_this.options.staging.cardsFrame.cardsDesigner).addClass("d-none");
                    $(_this.options.staging.cardsFrame.cardsDesignerToolbar).addClass("d-none");
                    $(_this.options.staging.cardsFrame.editSaveButton).addClass("btn-default").removeClass("btn-primary");
                    $(_this.options.staging.cardsFrame.editSaveButton + " " + _this.options.staging.cardsFrame.icon).text("edit");

                    if (!reset && _this.options.designChanged) {
                        _this.options.design = _this._generateCardsDesign();
                        if ((saveMsg = _this._saveCardDesign()) == "true") {
                            _this.options.designChanged = false;
                            showAlertDialog("success", "Cards Design Saved Successfully");
                            _this.activate();
                        } else {
                            try {
                                saveMsg = JSON.parse(saveMsg)["error"]["msg"];
                            } catch (ex) { }
                            showAlertDialog("error", saveMsg);
                            if (saveMsg == "Session Authentication failed...") {
                                setTimeout(() => {
                                    parent.window.location.href = "../aspx/sess.aspx";
                                }, 1000);
                            }
                        }
                    } else if (!_this.options.designChanged) {
                    }
                }
            },
            _generateCardsDesign() {
                let _this = this;
                return $(`${_this.options.staging.cardsFrame.cardsDesigner} li input:checkbox`).map((index, elem) => {
                    return { axp_cardsid: $(elem).data("cardId"), visible: $(elem).prop("checked"), orderno: index + 1 }
                }).toArray();
            },
            _saveCardDesign() {
                let _this = this;
                let result = "";
                $.ajax({
                    url: 'mainnew.aspx/saveCardsDesign',
                    type: 'POST',
                    cache: false,
                    async: false,
                    data: JSON.stringify({
                        design: JSON.stringify(_this.options.design)
                    }),
                    dataType: 'json',
                    contentType: "application/json",
                    success: function (data) {
                        if (data && data.d) {
                            result = data.d;
                        }
                    },
                    error: function (error) {

                    }
                });
                return result;
            },
            _sortCardsObject(cards) {
                let _this = this;

                if (_this.options.cards.length > 0) {
                    _this.options.cards = _.values(_.merge(_.keyBy(_this.options.cards, 'axp_cardsid'), _.keyBy(_this.options.design, 'axp_cardsid'))).map((card => {
                        if (card["cardtype"]) {
                            typeof card["visible"] != "undefined" ? delete card.visible : "";
                            return card;
                        } else {
                            return false;
                        }
                    })).filter((card => card));
                }

                _this.options.cards = _.sortBy(_this.options.cards, (card) => parseInt(card.orderno))
            },
            _drawCardsLayout() {
                let _this = this;
                $(_this.options.staging.cardsFrame.div + " " + _this.options.staging.cardsFrame.cardsDiv).html(``);
                $(_this.options.staging.cardsFrame.div + " " + _this.options.staging.cardsFrame.cardsDesigner).html(``);

                if (_this.options.enableMasonry) {
                    $(_this.options.staging.cardsFrame.div + " " + _this.options.staging.cardsFrame.cardsDiv).html(_this.options.html.masonryWidth);
                }

                try {
                    callParentNew("updateAppLinkObj")?.("loadhomepage");
                } catch (ex) {

                }

                _this.options.cards.forEach((card, index) => {
                    let cardType = card["cardtype"];
                    if (cardType) {
                        if (["chart", "kpi", "list", "menu", "modern menu", "image card", "calendar", "html"].indexOf(cardType) == -1) {
                            return;
                        }

                        let cardData = _this._getCardData(card);

                        let cardId = card["axp_cardsid"];

                        let design = _this.options.design.filter((dsign) => dsign.axp_cardsid == cardId)[0];

                        let cardVisible = design && typeof design["visible"] != "undefined" ? JSON.parse(design["visible"]) : true;

                        $(_this.options.staging.cardsFrame.cardsDesigner).append(`
                        <li class="d-flex list-group-item ui-state-default">
                            <span class="material-icons dragIcon cursor-pointer my-auto">drag_indicator</span>

                            <div class="agform form-check form-check-custom form-check-solid px-1 align-self-end">
                                <input id="chkBox${cardId}" class="showHideChkBox form-check-input h-20px w-20px" type="checkbox" ${cardVisible ? `checked="checked"` : ``} data-card-id="${cardId}" />
                                <label class="form-check-label form-label fw-boldest my-2" for="chkBox${cardId}" >
                                    <span class="dragName">${card["cardname"]} (${cardType})</span>
                                </label>
                            </div>
                        </li>
                        `);

                        if (!cardVisible) {
                            return cardVisible;
                        }

                        let cardFormattedData = [];

                        cardFormattedData = _this._formatOutput(cardData);

                        let cardElement = $(_this.options.html[cardType]).attr("card-index", index).data("card-index", index);

                        $(_this.options.staging.cardsFrame.cardsDiv).append(cardElement);

                        switch (cardType) {
                            case "kpi":
                                _this._addCardHeight(cardElement, card, "");
                                _this._addCardWidth(cardElement, card, "3");

                                _this._addCardIcon(cardElement, card);
                                _this._addCardHeader(cardElement, card);
                                if (typeof cardData != "string" && cardData.length > 0) {
                                    try {
                                        _this._setKpiInfo(cardElement, cardData, cardFormattedData);
                                    } catch (ex) { }
                                }

                                _this._addCardBackground(cardElement, card);
                                break;
                            case "image card":
                                _this._addCardHeight(cardElement, card, "");
                                _this._addCardWidth(cardElement, card, "4");

                                _this._addCardIcon(cardElement, card);
                                _this._addCardHeader(cardElement, card);

                                try {
                                    _this._setImageCardInfo(cardElement, cardData, card);
                                } catch (ex) { }

                                _this._addCardBackground(cardElement, card);
                                break;
                            case "chart":
                                _this._addCardHeight(cardElement, card, "");
                                _this._addCardWidth(cardElement, card, "4");

                                _this._addCardIcon(cardElement, card);
                                _this._addCardHeader(cardElement, card);

                                if (typeof cardData != "string" && cardData.length > 0) {
                                    setTimeout(() => {
                                        let body = cardElement.find("bodyContent").parent();
                                        try {
                                            AxPlotHighChartWidgets(card["charttype"], body, cardFormattedData.rows, cardFormattedData.metaData, (_this._getChartJsonProps(card, "attributes") || {}), (_this._getChartJsonProps(card, "attributes.enableSlick") || false));
                                            setTimeout(() => {
                                                body.find(".highcharts-button.highcharts-contextbutton, .highcharts-button.highcharts-contextbutton *").hide();
                                            }, 0);
                                        } catch (ex) { }
                                    }, 0);
                                }
                                break;
                            case "list":
                                _this._addCardHeight(cardElement, card, "");
                                _this._addCardWidth(cardElement, card, "6");

                                _this._addCardIcon(cardElement, card);
                                _this._addCardHeader(cardElement, card);
                                if (typeof cardData != "string" && cardData.length > 0) {
                                    setTimeout(() => {
                                        try {
                                            _this._generateDatatable(cardElement.find("bodyContent").parent(), card, cardData, cardFormattedData);

                                            _this._addCardBackground(cardElement, card);
                                        } catch (ex) { }
                                    }, 0);
                                }
                                break;
                            case "menu":
                                _this._addCardHeight(cardElement, card, "");
                                _this._addCardWidth(cardElement, card, "6");

                                _this._addCardIcon(cardElement, card);
                                _this._addCardHeader(cardElement, card);

                                try {
                                    _this._setMenuInfo(cardElement, cardData, card);
                                } catch (ex) { }

                                _this._addCardBackground(cardElement, card);
                                break;
                            case "modern menu":
                                _this._addCardWidth(cardElement, { ...card, width: "col-md-12" }, "12");

                                try {
                                    _this._setModernMenuInfo(cardElement, cardData, card);
                                } catch (ex) { }
                                break;
                            case "calendar":
                                _this._addCardWidth(cardElement, card, "12");
                                _this._addCardHeight(cardElement, card, "");

                                _this._addCardIcon(cardElement, card);
                                _this._addCardHeader(cardElement, card);

                                try {
                                    _this._setCalendar(cardElement, cardData, card);
                                } catch (ex) { }
                                break;
                            case "html":
                                _this._addCardHeight(cardElement, card, "");
                                _this._addCardWidth(cardElement, card, "4");

                                _this._addCardIcon(cardElement, card);
                                _this._addCardHeader(cardElement, card);

                                try {
                                    _this._setHtmlData(cardElement, card);
                                } catch (ex) { }

                                _this._addCardBackground(cardElement, card);
                                break;
                            case "break":
                                break;
                        }
                    }
                });
                _this._cardsWidthPostProcessor($(_this.options.staging.cardsFrame.cardsDiv));
                _this._registerPlugins();
            },
            _getCardData(card) {
                let cardData = card.cardsql.row || card.cardsql["@value"] || "";
                if (typeof cardData.length == "undefined") {
                    cardData = [cardData];
                }

                return cardData;
            },
            _addCardIcon(cardElement, card) {
                let iconDiv = cardElement.find("iconContent").parents(".mainIcon");

                let icon = card["cardicon"] ? (card["cardicon"] != "*" ? card["cardicon"].toString() : "") : "";

                cardElement.find("iconContent").replaceWith(icon);

                if (icon) {
                    iconDiv.addClass("iconAdded");
                } else {
                    iconDiv.addClass("d-none");
                }
            },
            _addCardHeader(cardElement, card) {
                cardElement.find(".mainHeading").attr("title", (card["cardname"] || "")).find("headerContent").replaceWith(card["cardname"] || "");
            },
            _addCardBackground(cardElement, card) {
                cardElement.find(".card-body a:not(.dropdown a)").addClass("border-bottom");

                if (card["axpfile_imgcard"] && card["cardtype"] == "image card") {
                    cardElement.find("imageContent").replaceWith($(`<img class="w-100 h-100 card-img img-fluid" style="object-fit: cover;" />`).attr("src", `/${callParentNew("mainProject")}/ImageCard/${card["axpfile_imgcard"].split(",")[0]}`));
                }

                if (card["cardbgclr"]) {
                    cardElement.find(".card, table, a:not(.dropdown a)").addClass("cardbg-" + card["cardbgclr"]);

                    cardElement.find(".symbol .symbol-label, button").addClass("cardbg-inverse-" + card["cardbgclr"]);
                }
            },
            _addCardWidth(cardElement, card, defaultWidth) {
                let _this = this;
                if (_this._getSetCardWidth(cardElement, card, defaultWidth)) {
                    if (_this.options.enableMasonry) {
                        $(_this._getClearFix()).insertBefore(cardElement);
                    } else {
                        $(cardElement.prevAll(":not(.row)").toArray().reverse()).wrapAll(`<div class="row" />`);
                    }
                }
            },
            _addCardHeight(cardElement, card, defaultHeight) {
                let _this = this;
                if (cardElement.find(".heightControl").length > 0) {
                    _this._getSetCardHeight(cardElement.find(".heightControl"), card, defaultHeight);
                }
            },
            _formatOutput(result) {
                let mt = [], row = [], rowset = [];
                for (i = 0; i < result.length; i++) {
                    row = [];
                    for (attr in result[i]) {
                        if (i == 0) {
                            mt.push({ 'name': attr });
                        }
                        row.push(result[i][attr]);
                    }
                    rowset[i] = row;
                }
                return { "metaData": mt, "rows": rowset };
            },
            _generateDatatable(elem, card, cardData, cardFormattedData) {
                let _this = this;
                let hiddenColumns = [];

                var parsedHyperLink = parseHyperLink(cardFormattedData.metaData, cardFormattedData.rows[0]);

                if (cardData.length > 0) {
                    let tableHTML = `
                    <table class="table" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                ${Object.keys(cardData[0]).map((col, ind) => {
                        if (col.toLowerCase() == "link" || col.toLowerCase().endsWith("_ui") || col.toLowerCase().startsWith("axphide_")) {
                            hiddenColumns.push(ind);
                        }
                        if (col.toLowerCase().startsWith("axphideh_")) {
                            return `<th></th>`;
                        } else {
                            return `<th>${col}</th>`;
                        }
                    }).join('')}
                            </tr>
                        </thead>
                    </table>
                    `;
                    elem.html(tableHTML);
                    elem.find("table").DataTable({
                        data: cardData,
                        columns: Object.keys(cardData[0]).map((col) => {
                            return { data: col }
                        }),
                        dom: "t",
                        // order: [],
                        columnDefs: [
                            {
                                targets: hiddenColumns,
                                visible: false
                            },
                            {
                                targets: "_all",
                                createdCell(elem, cellData, rowData, row, col) {
                                    if (Object.keys(rowData)[col].toLowerCase().startsWith("axphidec_")) {
                                        $(elem).html("");
                                        return;
                                    }
                                    var hyperLinkCol = parsedHyperLink ? parsedHyperLink.i : false;

                                    if (hyperLinkCol !== false) {
                                        cellData = getTheHyperLink(cardFormattedData.rows[row][hyperLinkCol], parsedHyperLink, customizeData(cardFormattedData.rows[row][col]), col);

                                        $(elem).html(cellData);
                                    }

                                    if (styleString = rowData[`${Object.keys(rowData)[col]}_ui`]) {
                                        $(elem).html(_this._SetAxFontCondition($(`<div><div>${cellData}</div></div>`).find("div"), styleString).css({ "border-radius": "10px" }).parent().html());
                                    }
                                }
                            }
                        ],
                        "bLengthChange": false,
                        "bPaginate": false,
                        "pageLength": 20,
                        initComplete(settings, json) {
                            $(this).css({ "height": (elem.height()) + "px", "overflow": "auto", "display": "block" });
                        }
                    });
                }
            },
            _setKpiInfo(cardElement, cardData, cardFormattedData) {
                if (cardData && cardData[0] && cardData[0]["status"]) {
                    let statusContent = cardElement.find("statusContent");

                    statusContent.parents("card-toolbar").removeClass("d-none");

                    statusContent.replaceWith($("<div>" + cardData[0]["status"] + "</div>").attr("title", `${(cardData[0]["status"]) || ""}`) || "");
                }

                if (cardData && cardData[0] && cardData[0]["footer"]) {
                    cardElement.find("footerContent").parents(".card-footer").removeClass("d-none");
                    cardElement.find("footerContent").replaceWith($("<div>" + cardData[0]["footer"] + "</div>").attr("title", `${(cardData[0]["footer"]) || ""}`) || "");
                } else {
                    cardElement.find("footerContent").parents(".card-footer").addClass("d-none");
                }

                var plotName = ((thisdata = cardData[0][Object.keys(cardData[0])[0]]) || (typeof thisdata != "undefined" ? thisdata : ""));

                var data = "";
                try {
                    var parsedHyperLink = parseHyperLink(cardFormattedData.metaData, cardFormattedData.rows[0]);

                    var hyperLinkCol = parsedHyperLink ? parsedHyperLink.i : false;

                    if (hyperLinkCol !== false) {
                        data = getTheHyperLink(cardFormattedData.rows[0][hyperLinkCol], parsedHyperLink, customizeData(plotName), parseInt(Object.keys(parsedHyperLink.data)[0].replace("col", "")) - 1);
                    } else {
                        data = customizeData(plotName);
                    }
                } catch (ex) {
                    data = customizeData(plotName);
                }

                cardElement.find("bodyContent").replaceWith($(`<div class="fs-2qx---">${data}</div>`).attr("title", `${(cardData[0][Object.keys(cardData[0])[0]]) || ""}`) || "");
            },
            _setImageCardInfo(cardElement, cardData, card) {
                var data = "";
                data = card.pagedesc || "";

                if (data) {
                    cardElement.find("bodyContent").replaceWith($(`<div>${data}</div>`).attr("title", `${(data) || ""}`) || "");
                }

                if (card.htransid && card.htype) {
                    cardElement.find("footerContent").parents(".card-footer").removeClass("d-none");
                    let linkStr = "";
                    if (card["htype"].toLowerCase() === "iview") {
                        linkStr = "ivtoivload.aspx?ivname=" + card.htransid;
                    } else if (card["htype"].toLowerCase() === "tstruct") {
                        linkStr = "tstruct.aspx?transid=" + card.htransid + `&openerIV=${card.htransid}&isIV=false`;
                    }

                    let footerHyperLink = `AxLoadUrl('${linkStr}');`;

                    cardElement.find("footerContent").replaceWith($("<a class='imgCardHyperlink' href='javascript:void(0);'>" + (card.hcaption || "Link") + "</a>").attr("onclick", `${footerHyperLink || ""}`) || "");
                } else {
                    cardElement.find("footerContent").parents(".card-footer").addClass("d-none");
                }
            },
            _setMenuInfo(cardElement, cardData, card) {
                var data = "";
                data = card["pagedesc"] || card["cardname"] || "";

                cardElement.find("descriptionContent").replaceWith($(`<div class="description pb-5">${data}</div>`).attr("title", `${data}`) || "");

                var pageName = "";
                pageName = card["pagename"] || "";

                let menuArray = [];
                if (pageName) {
                    var pageObj = _.findDeep(
                        menuJson,
                        (value, key, parent) => {
                            if (key == 'oname' && value == pageName) return true;
                        }
                    )?.parent;
                    if (pageObj) {
                        if (!pageObj.target) {
                            menuArray = pageObj.child || [];
                        } else {
                            menuArray = pageObj;
                        }
                        if (typeof menuArray.length == "undefined") {
                            menuArray = [menuArray];
                        }
                    }
                }

                if (menuArray.length > 0) {
                    let menuOptions = menuArray.map(menu => {

                        if (!menu.target) {
                            return ``;
                        }

                        let iconString = ``;

                        if (typeof menu.icon == "object") {
                            iconString = `<span class="menu-icon material-icons material-icons-style menuIconFont">${menu.icon.text}</span>`;
                        }
                        else if (menu.icon && menu.icon.indexOf("/") != -1) {
                            iconString = `<img src="${menu.icon}" class="menu-icon menuIcon" />`;
                        }
                        else if (menu.target != undefined && menu.target != "") {
                            if (menu.target.indexOf("tstruct.aspx") > -1) {
                                iconString = `<span class="menu-icon material-icons material-icons-style material-icons-2 menuIconFont">assignment</span>`;
                            } else if (menu.target.indexOf("iview.aspx") > -1 || menu.target.indexOf("iviewInteractive.aspx") > -1) {
                                iconString = `<span class="menu-icon material-icons material-icons-style material-icons-2 menuIconFont">view_list</span>`;
                            }
                            else {
                                iconString = `<span class="menu-icon material-icons material-icons-style material-icons-2 menuIconFont">insert_drive_file</span>`;
                            }
                        }
                        else if (menu.target == "" && menu.oname.indexOf("Head") > -1) {
                            iconString = `<span class="menu-icon material-icons material-icons-style material-icons-2 menuIconFont">folder</span>`;
                        }
                        else {
                            iconString = `<span class="menu-icon material-icons material-icons-style material-icons-2 menuIconFont">insert_drive_file</span>`;
                        }

                        return `
                        <li class="menu-item">
                            <a href="javascript:void(0);" class="dropdown-item menu-link" title="${menu.name}" onclick="AxLoadUrl('${menu.target}')">
                                ${iconString}
                                <span>${menu.name}</span>
                            </a>
                        </li>
                        `;
                    }).join("");

                    let menuHTML = `
                    <div class="dropdown" id="dropdownMenuButton1">
                        <button type="button" class="btn btn-primary dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                            Menu
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                            ${menuOptions}
                        </ul>
                    </div>
                    `;

                    if (menuHTML && menuOptions) {
                        cardElement.find("bodyContent").replaceWith($(`<div class="">${menuHTML}</div>`) || "");
                    }
                }
            },
            _setModernMenuInfo(cardElement, cardData, card) {
                let _this = this;

                var pageName = "";
                pageName = card["pagename"] || "";

                let menuArray = [];
                if (pageName) {
                    var pageObj = _.findDeep(
                        menuJson,
                        (value, key, parent) => {
                            if (key == 'oname' && value == pageName) return true;
                        }
                    )?.parent;
                    if (pageObj) {
                        if (!pageObj.target) {
                            menuArray = pageObj.child || [];
                        } else {
                            menuArray = pageObj;
                        }
                        if (typeof menuArray.length == "undefined") {
                            menuArray = [menuArray];
                        }
                    }
                }

                if (menuArray.length > 0) {
                    let menuOptions = menuArray.map(menu => {

                        if (!menu.target) {
                            return ``;
                        }

                        let iconString = ``;

                        if (typeof menu.icon == "object") {
                            iconString = `<span class="material-icons material-icons-style material-icons-3x text-primary menuIconFont">${menu.icon.text}</span>`;
                        }
                        else if (menu.icon && menu.icon.indexOf("/") != -1) {
                            iconString = `<img src="${menu.icon}" class="menuIcon" />`;
                        }
                        else if (menu.target != undefined && menu.target != "") {
                            if (menu.target.indexOf("tstruct.aspx") > -1) {
                                iconString = `<span class="material-icons material-icons-style material-icons-3x text-primary menuIconFont">assignment</span>`;
                            } else if (menu.target.indexOf("iview.aspx") > -1 || menu.target.indexOf("iviewInteractive.aspx") > -1) {
                                iconString = `<span class="material-icons material-icons-style material-icons-3x text-primary menuIconFont">view_list</span>`;
                            }
                            else {
                                iconString = `<span class="material-icons material-icons-style material-icons-3x text-primary menuIconFont">insert_drive_file</span>`;
                            }
                        }
                        else if (menu.target == "" && menu.oname.indexOf("Head") > -1) {
                            iconString = `<span class="material-icons material-icons-style material-icons-3x text-primary menuIconFont">folder</span>`;
                        }
                        else {
                            iconString = `<span class="material-icons material-icons-style material-icons-3x text-primary menuIconFont">insert_drive_file</span>`;
                        }

                        let tempMenu = $(_this.options.html["modernMenuContent"]);

                        tempMenu.find("bodyContent").replaceWith($(`<div class="pt-2 modernMenuNameText" title="${menu.name}">${menu.name}</div>`) || "");

                        tempMenu.find("iconContent").replaceWith($(`<div class="">${iconString}</div>`) || "");

                        tempMenu.attr("onclick", `AxLoadUrl('${menu.target}')`);

                        return tempMenu.get(0).outerHTML;
                    }).join("");

                    if (menuOptions) {
                        cardElement.find("modernMenuContent").replaceWith($(`<div class="row">${menuOptions}</div>`) || "");
                    }
                }
            },
            _setCalendar(cardElement, cardData, card) {
                let _this = this;

                let body = cardElement.find("bodyContent").parent();

                body.find("bodyContent").hide();

                if (!body.data("height")) {
                    body.data("height", body.css("height"));
                }

                body.css("height", body.data("height"));

                let bodyHeight = $(body).height();

                try {
                    if (card["obj"]) {

                        let ind = cardElement.data("cardIndex");

                        _this.options.cards[ind] = card;

                        cardData = _this._getCardData(card);

                        card["obj"].destroy();
                    }
                } catch (ex) { }

                let headerOptions = [
                    {
                        icon: "calendar_view_month",
                        href: "javascript:void(0);",
                        caption: "Month View",
                        onclick: `$.axpertUI.cardsPage.options.cards[${cardElement.data("cardIndex")}].obj.changeView('dayGridMonth');`
                    },
                    {
                        icon: "calendar_view_week",
                        href: "javascript:void(0);",
                        caption: "Week View",
                        onclick: `$.axpertUI.cardsPage.options.cards[${cardElement.data("cardIndex")}].obj.changeView('dayGridWeek');`
                    },
                    {
                        icon: "calendar_view_day",
                        href: "javascript:void(0);",
                        caption: "Day View",
                        onclick: `$.axpertUI.cardsPage.options.cards[${cardElement.data("cardIndex")}].obj.changeView('timeGridDay');`
                    },
                    {
                        icon: "add",
                        href: "javascript:void(0);",
                        caption: "Add Event",
                        onclick: `$.axpertUI.cardsPage._calendarEventClick('', ${cardElement.data("cardIndex")});`
                    }
                ];

                cardElement.find(".card-toolbar").removeClass("d-none");
                cardElement.find(".card-toolbar .menu.menu-sub.menu-sub-dropdown .separator + .menu-item").remove();
                cardElement.find(".card-toolbar .menu.menu-sub.menu-sub-dropdown .menu-item.dynamic-item").remove();
                headerOptions.forEach((opt) => {
                    cardElement.find(".card-toolbar .menu.menu-sub.menu-sub-dropdown").append(`
                    <div class="menu-item px-3 dynamic-item">
                        <a href="${opt.href}" onclick="${opt.onclick}" class="menu-link px-3">
                            <i class="menu-icon material-icons material-icons-style material-icons-2">${opt.icon}</i>
                            <span>${opt.caption}</span>
                        </a>
                    </div>
                    `);
                });

                var calendar = new FullCalendar.Calendar($(body)[0], {
                    initialView: 'dayGridMonth',
                    dayMaxEventRows: true,
                    views: {
                        timeGrid: {
                            dayMaxEventRows: true
                        }
                    },
                    height: bodyHeight,
                    headerToolbar: {
                        start: 'title',
                        center: '',
                        end: 'addEvent today,prev,next'
                    },
                    buttonText: {
                        today: "today",
                        dayGridMonth: "calendar_view_month",
                        timeGridDay: "calendar_view_day",
                        prev: "navigate_before",
                        next: "navigate_next"
                    },
                    customButtons: {
                        addEvent: {
                            text: 'add',
                            click(e) {
                                _this._calendarEventClick(e);
                            }
                        }
                    },
                    events: _this._calendarEventData(cardElement, cardData, card),
                    eventClick(info) {

                        $(".popover.in, .popover.show, .fc-popover").remove();

                        let { recordid } = info?.event?._def?.extendedProps;
                        _this._calendarEventClick(recordid, cardElement.data("cardIndex"));
                    },
                    dateClick: function (info) {
                        $(".popover.in, .popover.show, .fc-popover").remove();

                        _this._calendarEventClick(``, cardElement.data("cardIndex"), (info.dateStr ? `${moment(info.dateStr).format("DD/MM/YYYY")}` : ``));
                    },
                    eventDidMount(info) {
                        $(info.el).find(".fc-event-title").addClass("fw-boldest");

                        let eventColor;
                        if (info.event && (eventColor = info?.event?._def?.extendedProps?.statusColor)) {
                            $(info.el).find(".fc-event-main-frame").prepend(`<span style="background-color:${eventColor}" class="bullet border mx-1 mt-2 w-7px h-7px"></span>`);
                        }

                        $(info.el).popover({
                            get title() {
                                let { title } = info?.event?._def;

                                let { type } = info?.event?._def?.extendedProps;

                                return `${title ? (`${title} (${type || ""})`) : ""}`
                            },
                            get content() {
                                let finalContent = "";

                                let { description, status, location } = info?.event?._def?.extendedProps;

                                typeof description != "undefined" && description != "" && (finalContent += ((finalContent ? "<br />" : "") + `<div class="menu-item"><div class="menu-link px-0 py-1"><span class="menu-icon material-icons material-icons-style material-icons-1 calendarPopoverSpan">description</span><span class="position-relative" style="top: -5px;">${description}</span></div></div>`));

                                typeof status != "undefined" && status != "" && (finalContent += ((finalContent ? "<!--<br />-->" : "") + `<div class="menu-item"><div class="menu-link px-0 py-1"><span class="menu-icon material-icons material-icons-style material-icons-1 calendarPopoverSpan">analytics</span><span class="position-relative" style="top: -5px;">${status}</span></div></div>`));

                                typeof location != "undefined" && location != "" && (finalContent += ((finalContent ? "<!--<br />-->" : "") + `<div class="menu-item"><div class="menu-link px-0 py-1"><span class="menu-icon material-icons material-icons-style material-icons-1 calendarPopoverSpan">location_on</span><span class="position-relative" style="top: -5px;">${location}</span></div></div>`));

                                return finalContent || "";
                            },
                            trigger: 'hover',
                            placement: 'top',
                            container: 'body',
                            html: true,
                            animation: false,
                            delay: { show: 0, hide: 0 }
                        }).on("shown.bs.popover", function () {
                            setTimeout(() => {
                                $(".popover.show").css("z-index", 9999);
                            }, 0);
                        });
                    }
                });

                setTimeout(() => {
                    let _this = $.axpertUI.cardsPage;

                    calendar.render();
                    card["obj"] = calendar;

                    _this._generateCalendarLegend(body, card);
                }, 0);
            },
            _generateCalendarLegend(body, card) {
                let _this = $.axpertUI.cardsPage;

                let footer = body.next();
                let legendObj = [...new Set(card.cardsql.row.map((row) => { return JSON.stringify({ type: row.eventtype, color: row.eventcolor }) }))].map(legend => JSON.parse(legend)) || [];

                if (legendObj.length > 0) {
                    footer.removeClass("d-none");
                    footer.find(".dynamic-item").remove();
                    footer.append($(legendObj.map(leg => `
                    <span class="d-flex dynamic-item">
                        <span style="background-color:${leg.color}" class="bullet border mx-1 w-20px h-20px">
                        </span>
                        <span class="fw-boldest">
                            ${leg.type}
                        </span>
                    </span>
                    `).join("") || `<span></span>`));
                } else {
                    footer.addClass("d-none");
                }
            },
            _calendarEventClick(recordObj = "", ind = "", date = "") {
                let _this = $.axpertUI.cardsPage;

                let recordid = "";

                let cardElement;
                let card;
                let cardData;

                let transid;

                if (typeof recordObj == "object") {
                    cardElement = $(recordObj.currentTarget).parents(".widgetWrapper");

                    ind = cardElement.data("cardIndex");

                    card = _this.options.cards[ind];

                    cardData = _this._getCardData(card);
                } else {
                    cardElement = $(`.widgetWrapper[card-index=${ind}]`);

                    card = _this.options.cards[ind]

                    cardData = _this._getCardData(card);

                    recordid = recordObj;
                }

                transid = card.calendarstransid || "axcal";

                let popTitle = "Calendar Events";

                try {
                    var modalId = "loadPopUpPage";
                    var modalBodyLink = "";

                    modalBodyLink = `tstruct.aspx?transid=${transid}${typeof recordid != "undefined" && recordid != "" ? `&recordid=${recordid}` : ``}${typeof date == "string" && date != "" ? `&startdate=${date}` : ``}&AxPop=true&act=open`

                    var iFrameModalBody = `<iframe id="${modalId}" name="${modalId}" class="col-12 flex-column-fluid w-100 h-100 p-0 my-n1" src="${""}" frameborder="0" allowtransparency="True"></iframe>`;

                    let myModal = new BSModal(modalId, "", iFrameModalBody,
                        (opening, modal) => {
                            try {
                                modal.url = modalBodyLink;
                                myModal.modalBody.querySelector(`#${modalId}`).contentWindow.location.href = modalBodyLink;
                            } catch (ex) { }
                        },
                        (closing, modal) => {
                            var isAxPop = modalBodyLink.indexOf("AxPop=true") > -1;

                            if (isAxPop && eval(callParent('isSuccessAlertInPopUp'))) {
                                eval(callParent('isSuccessAlertInPopUp') + "= false");
                                try {
                                    let obj = card["obj"];

                                    card = _this._refreshData(card["axp_cardsid"], true).filter(c => c["axp_cardsid"] == card["axp_cardsid"])[0];

                                    cardData = _this._getCardData(card);

                                    card["obj"] = obj;

                                    _this._setCalendar(cardElement, cardData, card);
                                } catch (ex) { }
                            }
                        }
                    );

                    myModal.changeSize("lg");
                    myModal.scrollableDialog();
                    myModal.hideHeader();
                    myModal.hideFooter();
                    myModal.showFloatingClose();
                    myModal.modalBody.classList.add(...["bg-light", "overflow-hidden"]);
                    myModal.modalContent.classList.add("h-100");
                } catch (error) {
                }
            },
            _calendarEventData(cardElement, cardData, card) {
                let _this = this;

                let returnData = [];

                cardData = cardData || [];

                try {
                    if (cardData) {
                        returnData = cardData.map(d => {
                            return {
                                id: d.axcalendarid || "",
                                title: d.eventname || "",
                                get start() {
                                    let returnString = "";

                                    if (d.startdate) {
                                        let { date, format } = _this._getDateAndFormatBasedOnCulture(d.startdate);

                                        if (date && format) {
                                            d.axptm_starttime = d.axptm_starttime || "00:00";

                                            date = `${date} ${d.axptm_starttime}`;
                                            format = `${format} hh:mm`;

                                            try {
                                                returnString = moment(date, format).toISOString();
                                            } catch (ex) { }
                                        }
                                    }

                                    return returnString;
                                },
                                get end() {
                                    let returnString = "";

                                    d.enddate = d.enddate || d.startdate;

                                    if (d.enddate) {
                                        let { date, format } = _this._getDateAndFormatBasedOnCulture(d.enddate);

                                        if (date && format) {
                                            d.axptm_endtime = d.axptm_endtime || "23:59";

                                            date = `${date} ${d.axptm_endtime}`;
                                            format = `${format} hh:mm`;

                                            try {
                                                returnString = moment(date, format).toISOString();
                                            } catch (ex) { }
                                        }
                                    }

                                    return returnString;
                                },
                                get allDay() {
                                    return (d.startdate == d.enddate && d.axptm_starttime == "00:00" && d.axptm_endtime == "23:59") || (moment(this.end).diff(moment(this.start), 'days') == 1 && d.axptm_starttime == "00:00" && d.axptm_endtime == "00:00");
                                },
                                type: d.eventtype || "",
                                description: d.messagetext || "",
                                get status() {
                                    if (d.eventstatus) {
                                        return d.eventstatus;
                                    } else {
                                        switch (d.status) {
                                            case 0:
                                                return "New event";
                                            case 1:
                                                return "Accepted";
                                            case 2:
                                                return "Rejected";
                                            case 3:
                                                return "Reschedule request";
                                            case 4:
                                                return "Cancel";
                                            default:
                                                return "";
                                        }
                                    }
                                },
                                get statusColor() {
                                    if (d.eventstatcolor) {
                                        return rgbToHex(getCssByAttr("style", `color: ${d.eventstatcolor}`, "color"))
                                    } else {
                                        return "";
                                    }
                                },
                                location: d.location || "",
                                get classNames() {
                                    switch (this.type.toLowerCase()) {
                                        default:
                                            return "";
                                    }
                                },
                                get color() {
                                    switch (this.type.toLowerCase()) {
                                        default:
                                            {
                                                if (d.eventcolor) {
                                                    return rgbToHex(getCssByAttr("style", `color: ${d.eventcolor}`, "color"));
                                                } else {
                                                    return ""
                                                }
                                            }
                                    }
                                },
                                get textColor() {
                                    switch (this.type.toLowerCase()) {
                                        default:
                                            {
                                                if (d.eventcolor) {
                                                    return invert(this.color, { ...appGlobalVarsObject._CONSTANTS.colors, threshold: 0.2 });
                                                } else {
                                                    return ""
                                                }
                                            }
                                    }
                                },
                                get borderColor() {
                                    return this.textColor;
                                },

                                get recordid() {
                                    if (card.calendarstransid) {
                                        return d.recordid;
                                    } else {
                                        return d.axcalendarid;
                                    }
                                },
                                display: "block",
                                url: `javascript:void(0);`
                            };
                        }) || [];
                    }
                } catch (ex) { }

                return returnData;
            },
            _setHtmlData(cardElement, card) {

                cardElement.find("bodyContent").replaceWith(`<div class="body-content"></div>`);
                var shadowDomEle = $(cardElement.find(".body-content"))[0];
                var shadowAtt = shadowDomEle.attachShadow({ mode: 'open' });
                card.shadow = shadowAtt;
                if (card["html_editor_card"]) {
                    let cardHTML = card["html_editor_card"];


                    let objCardHTML = $(cardHTML);

                    if (objCardHTML.data("theme")) {
                        cardHTML = bundleCss.map(file => `<link rel="stylesheet" href="${file}" />`).join("") + cardHTML;
                    }

                    if (objCardHTML.data("handlebars")) {
                        let sourceApiName = objCardHTML.data("sourceApiName");
                        let sourceApiType = objCardHTML.data("sourceApiType");

                        if (sourceApiName && sourceApiType == "menu") {
                            let menuData = AxGetMenus(sourceApiName);
                            menuData = Object.keys(menuData).length == 1 ? menuData[Object.keys(menuData)] : menuData;

                            card.axRenderProject = callParentNew("mainProject");
                            card.apiData = menuData;

                            try {
                                cardHTML = Handlebars.compile(cardHTML)(card);
                            } catch (error) {
                                console.error("Card HTML Templete", "menu", error);
                            }

                            shadowAtt.innerHTML = cardHTML;
                        } else if (sourceApiName && (sourceApiType == "sql" || sourceApiType == "axpert")) {
                            AxAsyncGetApiData(sourceApiName, sourceApiType,
                                [{
                                    "id": card.axp_cardsid,
                                    "cachedata": card.cachedata,
                                    "cachedTime": String(card.cachedTime).padStart(12, '0'),
                                    "autorefresh": card.autorefresh
                                }],
                                (succ) => {
                                    var apiData = JSON.parse(succ);
                                    apiData = Object.keys(apiData).length == 1 ? apiData[Object.keys(apiData)] : apiData;

                                    card.axRenderProject = callParentNew("mainProject");
                                    card.apiData = apiData;

                                    try {
                                        cardHTML = Handlebars.compile(cardHTML)(card);
                                    } catch (error) {
                                        console.error("Card HTML Templete", sourceApiType, error);
                                    }

                                    shadowAtt.innerHTML = cardHTML;
                                },
                                (err) => {
                                }
                            );
                        }
                    }
                    else {
                        shadowAtt.innerHTML = cardHTML;
                    }
                }
            },
            _getDateAndFormatBasedOnCulture(dateStr) {
                if (dateStr != "") {
                    dateStr = dateStr.split(" ")[0];
                    if (callParentNew("glCulture") == "en-us") {
                        var splittedDate = dateStr.split("/");
                        if (splittedDate.length > 2) {
                            dateStr = splittedDate[1] + "/" + splittedDate[0] + "/" + splittedDate[2];
                        }
                    }
                }
                return { date: dateStr, format: (callParentNew("glCulture") == "en-us" ? "MM/DD/YYYY" : "DD/MM/YYYY") };
            },
            _SetAxFontCondition(thisFld, applyStyle) {
                let _this = this;
                if (applyStyle != "") {
                    _this._ResetAxFont(thisFld);
                    applyStyle = applyStyle.split(',');
                    for (var y = 0; y < applyStyle.length; y++) {
                        var currentStyle = applyStyle[y].split('=');
                        switch (currentStyle[0].toLowerCase()) {
                            case "fontname":
                                thisFld.css("font-family", currentStyle[1]);
                                break;
                            case "fontsize":
                                thisFld.css("font-size", currentStyle[1] + "pt");
                                break;
                            case "fontstyle":
                                for (var z = 0; z < currentStyle[1].toLowerCase().length; z++) {
                                    switch (currentStyle[1].toLowerCase()[z]) {
                                        case "b":
                                            thisFld.css('font-weight', 'bold');
                                            break;
                                        case "i":
                                            thisFld.css('font-style', 'italic');
                                            break;
                                        case "u":
                                            if (thisFld.css("text-decoration") == "line-through") {
                                                thisFld.css("text-decoration", "line-through underline");
                                            } else {
                                                thisFld.css("text-decoration", "underline");
                                            }
                                            break;
                                        case "s":
                                            if (thisFld.css("text-decoration") == "underline") {
                                                thisFld.css("text-decoration", "underline line-through");
                                            } else {
                                                thisFld.css("text-decoration", "line-through");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "fontcolor":
                                if (AxClColors[currentStyle[1]]) {
                                    thisFld.css("color", AxClColors[currentStyle[1]]);
                                } else if (currentStyle[1].indexOf("#") == 0) {
                                    thisFld.css("color", currentStyle[1]);
                                } else {
                                    thisFld.css("color", currentStyle[1]);
                                }
                                break;
                            case "backcolor":
                                if (AxClColors[currentStyle[1]]) {
                                    thisFld.css("background", AxClColors[currentStyle[1]]);
                                } else if (currentStyle[1].indexOf("#") == 0) {
                                    thisFld.css("background", currentStyle[1]);
                                } else {
                                    thisFld.css("background", currentStyle[1]);
                                }
                                break;
                            default:
                                thisFld.css(currentStyle[0], currentStyle[1]);
                                break;
                        }
                    }
                } else {
                    _this._ResetAxFont(thisFld);
                }
                return thisFld;
            },
            _ResetAxFont(thisFld) {
                let _this = this;
                thisFld.css("font-family", "");
                thisFld.css('font-weight', "");
                thisFld.css('font-style', "");
                thisFld.css("text-decoration", "");
                thisFld.css("color", "");
                thisFld.css("background", "");
            },
            _getSetCardWidth(elem, card, defaultVal, returnValue = false) {
                let _this = this;
                let cardWidth = "";

                cardWidth = card["width"] || $(elem)[0].className;
                if (!cardWidth) {
                    if (defaultVal) {
                        cardWidth = defaultVal.toString();
                    }
                }
                if (cardWidth) {
                    cardWidth = cardWidth.split(" ").filter(col => col.indexOf("col-") == 0).reverse().join(" ").toString();
                    if (cardWidth.indexOf("col-") == 0 || !isNaN(+cardWidth)) {
                        if (cardWidth.indexOf("col-") == 0) {
                            cardWidth = cardWidth.split("-")[cardWidth.split("-").length - 1];
                        }

                        if (!cardWidth) {
                            if (defaultVal) {
                                cardWidth = defaultVal.toString();
                            }
                        }

                        if (cardWidth) {
                            if (returnValue) {
                                return +cardWidth;
                            }

                            elem.removeClass(function (index, className) {
                                return (className.match(/(^|\s)col-\S+/g) || []).join(' ');
                            });

                            elem.addClass(`col-lg-${cardWidth} col-md-${cardWidth} col-sm-12 col-xs-12`);

                            if (cardWidth == "12" && (heightControl = elem.find(".heightControl")) && heightControl.length > 0 && (setHeight = heightControl.data("height"))) {
                                heightControl.css({
                                    "height": `${setHeight}`
                                });

                                heightControl.removeClass(heightControl.attr("class").split(" ").filter(className => className.startsWith("h-") || className.startsWith("mh-") || className.startsWith("min-h-")).join(" "));


                            }
                        } else if (returnValue) {
                            return +cardWidth;
                        }
                    } else {
                        cardWidth = "";

                        if (returnValue) {
                            return +cardWidth;
                        }
                    }
                } else if (returnValue) {
                    return +cardWidth;
                }

                if (cardWidth !== "") {
                    cardWidth = +cardWidth;
                    if (_this.currentColumnWidth + cardWidth > _this.options.totalColumns || cardWidth >= _this.options.totalColumns) {
                        if (_this.currentColumnWidth + cardWidth > _this.options.totalColumns) {
                            _this.currentColumnWidth = cardWidth;
                        }
                        else {
                            _this.currentColumnWidth = 0;
                        }
                        return true;
                    }
                    else {
                        _this.currentColumnWidth += cardWidth;
                        return false;
                    }
                }
            },
            _getSetCardHeight(elem, card, defaultVal) {
                let _this = this;
                let cardHeight = "";
                cardHeight = card["height"];
                if (!cardHeight) {
                    if (defaultVal) {
                        cardHeight = defaultVal;
                    }
                }
                if (cardHeight) {
                    cardHeight = cardHeight.toString();
                    if (cardHeight.indexOf("px") == cardHeight.length - 2) {
                        let cardHeightRaw = cardHeight.substring(0, cardHeight.length - 2);

                        cardHeight = cardHeightRaw + "px";

                        if (cardHeight != "0px" && +cardHeightRaw > -1) {
                            elem.data("height", cardHeight);
                        }
                    } else {
                        cardHeight = "";
                    }
                } else {
                    cardHeight = "";
                }
            },
            _getChartJsonProps(card, prop) {
                if (card["chartjson"] && card["chartjson"] != "*") {
                    let chartJsonProps = {};
                    if (typeof card["chartjson"] == "string") {
                        try {
                            chartJsonProps = JSON.parse(card["chartjson"]);
                        } catch (ex) { }
                    } else {
                        chartJsonProps = card["chartjson"];
                    }

                    if (typeof prop != "undefined") {
                        if (typeof _.get(chartJsonProps, prop) != "undefined") {
                            return _.get(chartJsonProps, prop);
                        } else {
                            return "";
                        }
                    } else {
                        return chartJsonProps;
                    }

                } else {
                    return "";
                }
            },
            _cardsWidthPostProcessor(cardsStage) {
                let _this = this;
                if (!_this.options.enableMasonry) {
                    cardsStage.children(":not(.row)").wrapAll(`<div class="row" />`);

                    cardsStage.children().toArray().forEach(row => {
                        const totalChildWidth = $(row).children().toArray().reduce((totalWidth, card) => {
                            return _this._getSetCardWidth($(card), "", "", true) + totalWidth;
                        }, 0) || 0;

                        if (totalChildWidth < 12) {
                            const lastCard = $(row).children(":last")[0];

                            const newLastColumnWidth = _this._getSetCardWidth($(lastCard), "", "", true) + (12 - totalChildWidth);

                            _this._getSetCardWidth($(lastCard), { "width": `col-${newLastColumnWidth}` });
                        }
                    });
                }
            },
            _registerPlugins() {
                let _this = this;

                KTMenu && KTMenu.init();

                // $(_this.options.staging.cardsFrame.cardsDesigner).sortable({
                //     cursor: "move",
                //     update: function (event, ui) {
                //         _this.options.designChanged = true;
                //     },
                // });
                // $(`${_this.options.staging.cardsFrame.cardsDesigner} li input:checkbox`).off("change.design").on("change.design", () => {
                //     _this.options.designChanged = true;
                // });

                $(window).off("resize.cards").on("resize.cards", (e) => {
                    $(".widgetWrapper.chartWrapper .highcharts-container").each((ind, elem) => {
                        try {
                            let chartObj = Highcharts.charts[$(elem).parent(".card-body").data("highchartsChart")];
                            setTimeout(() => {
                                chartObj.reflow();
                            }, 1000);
                        } catch (ex) { }
                    });
                    $(".widgetWrapper.listWrapper table.dataTable").each((ind, elem) => {
                        try {
                            setTimeout(() => {
                                $(elem).css("display", "");
                                $(elem).css({ "height": ($(elem).parents(".card-body").height()) + "px", "overflow": "auto", "display": "block" });
                            }, 0);
                        } catch (ex) { }
                    });
                    $(".widgetWrapper.calendarWrapper").each((ind, elem) => {
                        try {
                            let calendarObj = _this.options.cards[$(elem).data("cardIndex")].obj;
                            setTimeout(() => {
                                calendarObj.render();
                            }, 1000);
                        } catch (ex) { }
                    });
                });

                if (_this.options.enableMasonry) {
                    setTimeout(() => {
                        if ($(_this.options.staging.cardsFrame.cardsDiv).data("masonry")) {
                            $(_this.options.staging.cardsFrame.cardsDiv).masonry('destroy');
                        }
                        $(_this.options.staging.cardsFrame.div + " " + _this.options.staging.cardsFrame.cardsDiv)
                            .masonry({
                                itemSelector: '.widgetWrapper',
                                columnWidth: '.grid-sizer',
                                percentPosition: true
                            });
                    }, 0);
                }
            },
            _getClearFix() {
                return `<div class="clearfix"></div>`;
            }
        }

        //==========================================================================================================================        

        setUserOptions = {
            activate: function (options, compressedMode) {
                var _this = this;
                _this.options = options;
                _this.compressedMode = compressedMode;
            },
        }

        init = function (initOptions) {
            this.options = $.extend(true, {}, this.options, initOptions);

            cardsPage.activate(this.options.cardsPage);
            $(callParentNew(this.options.navigation.backButton.div.substring(1), "class")).hide();

            setTimeout(() => {

                $(this.options.loader.div).addClass(this.options.loader.outAnimationClass);
                setTimeout(() => {
                    $(this.options.loader.textDiv).addClass("d-none");
                    $(this.options.loader.parent).removeClass(this.options.loader.parent.substr(1)).removeClass(this.options.loader.stayClass);
                    $(this.options.loader.div).removeClass(this.options.loader.outAnimationClass).addClass(this.options.loader.radialGradientClass);
                    $(this.options.loader.subDiv).addClass(this.options.loader.loaderWrapClass);
                }, 0);

            }, 50);

            return this.options;
        }
        //==========================================================================================================================        
        return { options, cardsPage, init }
    }
    $.axpertUI = new axpertUI();
})(jQuery, window);
