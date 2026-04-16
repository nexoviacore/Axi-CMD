var dtCulture = eval(callParent('glCulture'));
let _entityFilter;
var _analyticsCharts;
var _entityCommon;
var _entityfilter;

var selectedItemsArray = [];
class AnalyticsCharts {
    constructor() {
        this.entityName = '';
        this.entityTransId = '';
        this.metaData = {};
        this.listJson = {};
        this.maxPageNumber = 1;
        this.pageSize = 50;
        this.kpiJson = [];
        this.chartsJson = [];
        this.chartsMetaData = {};
        this.selectedCharts = "";
        this.selectedChartsObj = {};
        this.fldData = {};
        this.emptyRowsHtml = `No data found`;
        this.filter = [];
        // this.fields = "All";
        this.xAxisFields = "";
        this.yAxisFields = "";
        this.selectedKeyField = '';
        this.entityList = [];
        this.allEntitiesList = [];
        this.selectedEntitiesList = [];
        this.filterObj = {};
        this.globalOrder = [];
        this.selectedChartType = "line"
        this.globalYOrder = [];
        this.urlParams = {};
        this.pendingGroupBy = "";
        this.selectedDateRange = "this_month";
        this.activeDateField = "";
        this.enableCardGridLayout = true;
        this.isGridChartLibLoaded = false;
        this.gridChartLibLoading = false;
        this.gridChartLibCallbacks = [];
        this.gridMenuOutsideClickBound = false;
        this.gridCardsPerRow = 4;
        this.minGridCardsPerRow = 2;
        this.maxGridCardsPerRow = 4;
        this.gridTotalTracks = 12;
        this.gridResizeHandlerBound = false;
        this.isHyperlinkPanelVisible = false;
        this.activeResizeRafId = 0;
        this.isFieldPopupShownOnLoad = false;
        this.hasStoredFieldSelection = false;
        this.fieldPopupShownEntities = {};
        this.entityListDataCache = {};
        this.entityListDataRequests = {};
        this.analyticsEntityCache = {};
        this.analyticsPrefsCache = {};
        this.availableAggFields = null;
        this.globalAggField = "count";
        this.isAggFieldAvailabilitySyncing = false;
        this.isApplyingGridCardState = false;
        this.gridBatchChartsByKey = {};
        this.gridBatchRequests = {};
        this.globalDecimalPlaces = 2;
        this.globalDecimalPlacesResolved = false;
        this.gridSelectionFilter = {
            groupField: "",
            values: [],
            filters: {}
        };
        // Filtering is now driven by the toolbar filter popup (EntityFilter pills),
        // not by clicking chart points.
        this.enableGridPointFilterSelection = false;
        this.enableFilterDebugLogs = true;
        this.analyticsIndexedDb = null;
        this.analyticsIndexedDbOpenPromise = null;
        this.dataSourceType = "tstruct";
        this.dataSourceName = "";
        this.adsMetaDataCache = {};
        this.gridCardStateCache = {};
        this.gridCardStateIndexedDbRequests = {};


    }
    init() {
        parent.ShowDimmer(false);
        if (typeof window !== "undefined" && typeof window.ANALYTICS_FILTER_DEBUG === "undefined") {
            window.ANALYTICS_FILTER_DEBUG = true;
        }

        const pageLoadData = JSON.parse(document.querySelector("#hdnAnalyticsPageLoadData").value);
        this.urlParams = this.getUrlParams();
        const urlSource = this.getUrlSource();
        this.dataSourceType = urlSource.type;
        this.dataSourceName = urlSource.name;
        this.updatePageHeaderTitle(this.dataSourceName || this.entityTransId || "");
        this.pendingGroupBy = this.isAdsMode() ? "" : (this.urlParams.groupby || "");
        this.applyUrlEntityContextUi();
        this.ensureLastRefreshLabel();

        const shouldForceUrlSourceLoad = !!urlSource.name && (
            urlSource.type === "ads" ||
            this.normalizeUrlParam(pageLoadData?.result?.data?.TransId || "") !== this.normalizeUrlParam(urlSource.name)
        );
        if (shouldForceUrlSourceLoad && this.tryAutoLoadEntityFromUrl()) {
            return;
        }

        if (_entityCommon.inValid(pageLoadData.result.data.SelectedEntities)) {
            document.querySelector("#Entity_summary_Left").classList.add("d-none");
            document.querySelector("#Entity_summary_Right").classList.add("d-none");
            document.querySelector("#Analytics_Grid_Container")?.classList.add("d-none");
            if (this.tryAutoLoadEntityFromUrl()) {
                return;
            }
            this.finalizeAnalyticsLoad(false);
        } else {
            document.querySelector("#Entity_summary_Left").classList.remove("d-none");
            document.querySelector("#Entity_summary_Right").classList.remove("d-none");
            document.querySelector("#Analytics_Grid_Container")?.classList.remove("d-none");
            let { "XAXISFIELDS": xAxisFields, "YAXISFIELDS": yAxisFields } = pageLoadData.result.data.Properties;

            this.xAxisFields = xAxisFields || "All";
            this.yAxisFields = yAxisFields || "All";

            this.selectedChartType = "line";

            const transidFieldsRaw = pageLoadData?.result?.data?.Fields || pageLoadData?.result?.data?.fields || "";
            this.metaData = this.mergeTransidFieldSourceIntoMeta(pageLoadData.result.data.MetaData, transidFieldsRaw);
            this.selectedEntitiesList = pageLoadData.result.data.SelectedEntitiesList;
            this.entityTransId = pageLoadData.result.data.TransId;
            this.entityName = this.getEntityCaption(this.entityTransId);
            this.updatePageHeaderTitle(this.entityName || this.entityTransId || this.dataSourceName || "");
            this.writeAnalyticsEntityCache(this.entityTransId, pageLoadData);

            this.loadStoredAnalyticsPreferences(this.entityTransId, () => {
                this.constructXandYAxis(this.metaData, "Page Load");
                this.constructSelectedEntityHeader();
                this.finalizeAnalyticsLoad(true);
            });
            return;
        }

        // this.checkAppmanagerAccess();
    }

    tryAutoLoadEntityFromUrl() {
        const source = this.getUrlSource();
        if (!source.name) {
            return false;
        }

        this.dataSourceType = source.type;
        this.dataSourceName = source.name;

        document.querySelector("#Entity_summary_Left")?.classList.remove("d-none");
        document.querySelector("#Entity_summary_Right")?.classList.remove("d-none");
        document.querySelector("#Analytics_Grid_Container")?.classList.remove("d-none");

        this.selectedEntitiesList = [{
            name: source.name,
            caption: source.name
        }];
        this.entityTransId = source.name;
        this.entityName = source.name;
        this.updatePageHeaderTitle(source.name);

        this.logFilterDebug("init:autoLoadEntityFromUrl", {
            sourceName: source.name,
            sourceType: source.type
        });

        if (source.type === "ads") {
            this.fetchAdsData(source.name, () => {
                try {
                    this.constructSelectedEntityHeader();
                } catch (error) { }
                this.finalizeAnalyticsLoad(true);
            });
        } else {
            fetchEntityData(source.name, () => {
                try {
                    this.constructSelectedEntityHeader();
                } catch (error) { }
                this.finalizeAnalyticsLoad(true);
            });
        }

        return true;
    }

    finalizeAnalyticsLoad(showFieldSelectionPopup) {
        this.ensureChartControls();
        this.initializeGridColumnControls();
        this.initializeHyperlinkPanelToggle();
        this.updateChartButton(this.selectedChartType);
        this.ensureGlobalGridAggControl();
        this.applyUrlParams();

        if (showFieldSelectionPopup) {
            this.openFieldSelectionOnPageLoad();
        }
    }

    getAnalyticsPropertyValue(properties, propertyName) {
        if (!properties || typeof properties !== "object") {
            return "";
        }

        const targetKey = `${propertyName || ""}`.toLowerCase();
        const matchKey = Object.keys(properties).find(key => `${key}`.toLowerCase() === targetKey);
        return matchKey ? properties[matchKey] : "";
    }

    extractStoredAnalyticsProperties(response) {
        let parsed = response;

        if (typeof parsed === "string") {
            try {
                parsed = JSON.parse(parsed);
            } catch (error) {
                return {};
            }
        }

        const candidates = [
            parsed?.result?.data?.Properties,
            parsed?.result?.data?.properties,
            parsed?.result?.data,
            parsed?.result?.Properties,
            parsed?.result?.properties,
            parsed?.data?.Properties,
            parsed?.data?.properties,
            parsed?.properties,
            parsed
        ];

        for (let i = 0; i < candidates.length; i++) {
            const current = candidates[i];
            if (current && typeof current === "object" && !Array.isArray(current)) {
                if (this.getAnalyticsPropertyValue(current, "XAXISFIELDS") ||
                    this.getAnalyticsPropertyValue(current, "YAXISFIELDS") ||
                    this.getAnalyticsPropertyValue(current, "CHARTTYPE") ||
                    this.getAnalyticsPropertyValue(current, "GLOBALAGGFIELD") ||
                    this.getAnalyticsPropertyValue(current, "GRIDCARDSTATE")) {
                    return current;
                }
            }
        }

        return {};
    }

    hasConfiguredAnalyticsFields(value) {
        const normalizedValue = `${value || ""}`.trim().toLowerCase();
        return normalizedValue !== "" && normalizedValue !== "all";
    }

    loadStoredAnalyticsPreferences(transId, callback, options = {}) {
        const done = () => {
            if (typeof callback === "function") {
                callback();
            }
        };
        const forceRefresh = !!options.forceRefresh;

        this.hasStoredFieldSelection =
            this.hasConfiguredAnalyticsFields(this.xAxisFields) ||
            this.hasConfiguredAnalyticsFields(this.yAxisFields);

        if (!forceRefresh) {
            const cachedPrefs = this.readAnalyticsPrefsCache(transId);
            if (cachedPrefs && typeof cachedPrefs === "object") {
                const cachedXAxis = this.getAnalyticsPropertyValue(cachedPrefs, "XAXISFIELDS");
                const cachedYAxis = this.getAnalyticsPropertyValue(cachedPrefs, "YAXISFIELDS");
                this.hasStoredFieldSelection =
                    this.hasConfiguredAnalyticsFields(cachedXAxis) ||
                    this.hasConfiguredAnalyticsFields(cachedYAxis);

                if (`${cachedXAxis || ""}`.trim() !== "") {
                    this.xAxisFields = cachedXAxis;
                }
                if (`${cachedYAxis || ""}`.trim() !== "") {
                    this.yAxisFields = cachedYAxis;
                }
                const cachedChartType = this.getAnalyticsPropertyValue(cachedPrefs, "CHARTTYPE");
                const cachedGlobalAgg = this.getAnalyticsPropertyValue(cachedPrefs, "GLOBALAGGFIELD");
                this.selectedChartType = cachedChartType || this.selectedChartType || "line";
                this.globalAggField = cachedGlobalAgg || this.globalAggField || "count";
                this.logFilterDebug("loadStoredAnalyticsPreferences:cacheHit", {
                    transId: transId,
                    xAxisFields: this.xAxisFields,
                    yAxisFields: this.yAxisFields,
                    chartType: this.selectedChartType,
                    globalAggField: this.globalAggField
                });
            }
        }

        if (!_entityCommon || typeof _entityCommon.getAnalyticsDataWS !== "function" || _entityCommon.inValid(transId)) {
            done();
            return;
        }

        const requestPayload = {
            page: "Analytics",
            transId: transId,
            propertiesList: ["XAXISFIELDS", "YAXISFIELDS", "CHARTTYPE", "GLOBALAGGFIELD", "GRIDCARDSTATE"]
        };

        _entityCommon.getAnalyticsDataWS(requestPayload, (response) => {
            try {
                const properties = this.extractStoredAnalyticsProperties(response);
                const storedXAxis = this.getAnalyticsPropertyValue(properties, "XAXISFIELDS");
                const storedYAxis = this.getAnalyticsPropertyValue(properties, "YAXISFIELDS");
                const storedChartType = this.getAnalyticsPropertyValue(properties, "CHARTTYPE");
                const storedGlobalAgg = this.getAnalyticsPropertyValue(properties, "GLOBALAGGFIELD");
                const storedGridState = this.getAnalyticsPropertyValue(properties, "GRIDCARDSTATE");
                this.hasStoredFieldSelection =
                    this.hasConfiguredAnalyticsFields(storedXAxis) ||
                    this.hasConfiguredAnalyticsFields(storedYAxis);

                if (`${storedXAxis || ""}`.trim() !== "") {
                    this.xAxisFields = storedXAxis;
                }
                if (`${storedYAxis || ""}`.trim() !== "") {
                    this.yAxisFields = storedYAxis;
                }
                this.selectedChartType = storedChartType || this.selectedChartType || "line";
                this.globalAggField = storedGlobalAgg || this.globalAggField || "count";

                if (storedGridState) {
                    try {
                        const parsedGridState = (typeof storedGridState === "string")
                            ? JSON.parse(storedGridState)
                            : storedGridState;
                        if (parsedGridState && typeof parsedGridState === "object") {
                            const normalizedGridState = this.normalizeGridCardStatePayload(parsedGridState, {
                                storedOn: parsedGridState.storedOn || new Date().toISOString()
                            });
                            this.writeGridCardState(normalizedGridState, transId);
                        }
                    } catch (error) {
                        console.warn("Unable to parse stored GRIDCARDSTATE:", error);
                    }
                }

                this.writeAnalyticsPrefsCache(transId, {
                    XAXISFIELDS: this.xAxisFields || "All",
                    YAXISFIELDS: this.yAxisFields || "All",
                    CHARTTYPE: this.selectedChartType || "line",
                    GLOBALAGGFIELD: this.globalAggField || "count"
                });
            } catch (error) {
                console.error("Error while reading analytics preferences:", error);
            } finally {
                done();
            }
        }, (error) => {
            console.error("Error while loading analytics preferences:", error);
            done();
        });
    }

    persistStoredAnalyticsPreferences(transId) {
        if (!_entityCommon || typeof _entityCommon.setAnalyticsDataWS !== "function" || _entityCommon.inValid(transId)) {
            return;
        }

        const payload = {
            page: "Analytics",
            transId: transId,
            properties: {
                "XAXISFIELDS": this.xAxisFields || "All",
                "YAXISFIELDS": this.yAxisFields || "All",
                "CHARTTYPE": this.selectedChartType || "line",
                "GLOBALAGGFIELD": this.globalAggField || "count"
            },
            allUsers: false
        };

        _entityCommon.setAnalyticsDataWS(payload, () => { }, (error) => {
            console.error("Error while saving analytics preferences:", error);
        });

        this.mergeAnalyticsPrefsCache(transId, {
            XAXISFIELDS: this.xAxisFields || "All",
            YAXISFIELDS: this.yAxisFields || "All",
            CHARTTYPE: this.selectedChartType || "line",
            GLOBALAGGFIELD: this.globalAggField || "count"
        });
    }

    openFieldSelectionOnPageLoad() {
        const entityKey = this.normalizeUrlParam(this.entityTransId);
        if (!entityKey || this.hasStoredFieldSelection || !Array.isArray(this.metaData) || this.metaData.length === 0) {
            return;
        }

        const cachedPrefs = this.readAnalyticsPrefsCache(entityKey);
        const cachedHasSelection = cachedPrefs && (
            this.hasConfiguredAnalyticsFields(this.getAnalyticsPropertyValue(cachedPrefs, "XAXISFIELDS")) ||
            this.hasConfiguredAnalyticsFields(this.getAnalyticsPropertyValue(cachedPrefs, "YAXISFIELDS"))
        );
        if (cachedHasSelection) {
            this.hasStoredFieldSelection = true;
            return;
        }

        if (this.fieldPopupShownEntities[entityKey]) {
            return;
        }

        this.fieldPopupShownEntities[entityKey] = true;
        this.isFieldPopupShownOnLoad = true;
        setTimeout(() => {
            openFieldSelection();
        }, 250);
    }

    getSupportedChartTypes() {
        return [
            { type: "pie", name: "Pie", icon: "pie_chart" },
            { type: "donut", name: "Donut", icon: "donut_large" },
            { type: "semi-donut", name: "Semi Donut", icon: "donut_small" },
            { type: "column", name: "Column", icon: "bar_chart" },
            { type: "bar", name: "Bar", icon: "bar_chart", iconStyle: "writing-mode: vertical-rl;" },
            { type: "line", name: "Line", icon: "show_chart" },
            { type: "area", name: "Area", icon: "show_chart" },
            { type: "stacked-column", name: "Stacked Column", icon: "view_column" },
            { type: "stacked-bar", name: "Stacked Bar", icon: "view_stream" },
            { type: "stacked-percentage-column", name: "100% Stacked", icon: "table_chart" },
            { type: "funnel", name: "Funnel", icon: "filter_alt" }
        ];
    }

    getChartTypeConfig(chartType) {
        const types = this.getSupportedChartTypes();
        const normalizedType = this.normalizeUrlParam(chartType) || "line";
        return types.find(item => item.type === normalizedType) || types[0];
    }

    getAutoDefaultGridChartType(cardIndex) {
        const supportedTypes = this.getSupportedChartTypes().map(item => item.type);
        const preferredOrder = [
            "bar",
            "line",
            "column",
            "area",
            "pie",
            "donut",
            "funnel"
        ].filter(type => supportedTypes.indexOf(type) > -1);

        const fallbackOrder = supportedTypes.length > 0 ? supportedTypes : ["line"];
        const typeOrder = preferredOrder.length > 0 ? preferredOrder : fallbackOrder;
        const safeIndex = Number.isFinite(cardIndex) ? Math.max(0, cardIndex) : 0;
        return typeOrder[safeIndex % typeOrder.length] || "line";
    }

    getSupportedColorPalettes() {
        return [
            { value: "newPalette", label: "Default" },
            { value: "pallet1", label: "Palette 1" },
            { value: "pallet2", label: "Palette 2" },
            { value: "pallet3", label: "Palette 3" },
            { value: "pallet4", label: "Palette 4" },
            { value: "avacado", label: "Avacado" },
            { value: "darkGreen", label: "Dark Green" },
            { value: "grid", label: "Grid" },
            { value: "sand", label: "Sand" },
            { value: "skies", label: "Skies" },
            { value: "sunset", label: "Sunset" }
        ];
    }

    getGridDefaultPaletteColors() {
        return ['#f4bc01', '#3d5996', '#e80502', '#539cfe', '#3ddab4', '#f14f5a'];
    }

    normalizeColorHex(colorValue, fallbackValue) {
        const fallback = `${fallbackValue || "#4f8df8"}`.trim().toLowerCase();
        const rawValue = `${colorValue || ""}`.trim().toLowerCase();
        if (/^#[0-9a-f]{6}$/i.test(rawValue)) {
            return rawValue;
        }
        if (/^#[0-9a-f]{3}$/i.test(rawValue)) {
            return `#${rawValue[1]}${rawValue[1]}${rawValue[2]}${rawValue[2]}${rawValue[3]}${rawValue[3]}`;
        }
        return fallback;
    }

    resolvePaletteColorsByKey(paletteKey) {
        const defaultColors = this.getGridDefaultPaletteColors();
        const normalizedKey = `${paletteKey || ""}`.trim();
        if (!normalizedKey) {
            return defaultColors.slice();
        }

        const paletteSource = (typeof customChartColors === "object" && customChartColors)
            ? customChartColors[normalizedKey]
            : null;
        if (!Array.isArray(paletteSource) || !paletteSource.length) {
            return defaultColors.slice();
        }

        const sanitized = [];
        for (let index = 0; index < defaultColors.length; index++) {
            const sourceColor = paletteSource[index] || paletteSource[index % paletteSource.length];
            sanitized.push(this.normalizeColorHex(sourceColor, defaultColors[index]));
        }
        return sanitized;
    }

    applyGridCardColorInputs(cardElement, colors) {
        if (!cardElement) {
            return;
        }

        const colorInputs = Array.from(cardElement.querySelectorAll(".analytics-grid-color-input"));
        if (!colorInputs.length) {
            return;
        }

        const fallbackColors = this.getGridDefaultPaletteColors();
        const sourceColors = Array.isArray(colors) && colors.length ? colors : fallbackColors;
        colorInputs.forEach((input, index) => {
            const fallbackColor = fallbackColors[index % fallbackColors.length];
            const sourceColor = sourceColors[index] || sourceColors[index % sourceColors.length];
            input.value = this.normalizeColorHex(sourceColor, fallbackColor);
        });
    }

    getGridCardCustomColors(cardElement) {
        const defaultColors = this.getGridDefaultPaletteColors();
        const colorInputs = cardElement ? Array.from(cardElement.querySelectorAll(".analytics-grid-color-input")) : [];
        if (!colorInputs.length) {
            return defaultColors.slice();
        }

        return colorInputs.map((input, index) => {
            const fallback = defaultColors[index % defaultColors.length];
            return this.normalizeColorHex(input.value, fallback);
        });
    }

    resolveGridPaletteConfig(paletteValue) {
        let resolvedColors = this.getGridDefaultPaletteColors();
        let paletteKey = "custom";

        if (paletteValue && typeof paletteValue === "object") {
            const customColors = Array.isArray(paletteValue.customColors)
                ? paletteValue.customColors
                : (Array.isArray(paletteValue.colors) ? paletteValue.colors : []);
            if (customColors.length > 0) {
                resolvedColors = customColors;
            } else if (`${paletteValue.paletteKey || ""}`.trim() !== "") {
                resolvedColors = this.resolvePaletteColorsByKey(paletteValue.paletteKey);
                paletteKey = `${paletteValue.paletteKey}`.trim();
            }
        } else if (typeof paletteValue === "string" && `${paletteValue}`.trim() !== "") {
            resolvedColors = this.resolvePaletteColorsByKey(paletteValue);
            paletteKey = `${paletteValue}`.trim();
        }

        const defaultColors = this.getGridDefaultPaletteColors();
        const normalizedColors = defaultColors.map((fallbackColor, index) => {
            const source = resolvedColors[index] || resolvedColors[index % Math.max(resolvedColors.length, 1)];
            return this.normalizeColorHex(source, fallbackColor);
        });

        return {
            paletteKey: paletteKey || "custom",
            customColors: normalizedColors,
            cck: "custom",
            cccv: normalizedColors.join(",")
        };
    }

    getGridCardPalette(cardElement) {
        return {
            paletteKey: "custom",
            customColors: this.getGridCardCustomColors(cardElement)
        };
    }

    getSelectedGlobalAggField() {
        const select = document.getElementById("analyticsGridGlobalAggSelect");
        if (select && select.value) {
            return select.value;
        }
        return this.globalAggField || "count";
    }

    getSelectedGlobalAggLabel() {
        const select = document.getElementById("analyticsGridGlobalAggSelect");
        if (select && select.options && select.selectedIndex > -1) {
            return select.options[select.selectedIndex]?.text || "Count";
        }
        const options = this.getGridAggFields();
        const selectedAgg = this.getSelectedGlobalAggField();
        const option = options.find(item => this.normalizeUrlParam(item.value) === this.normalizeUrlParam(selectedAgg));
        return option ? option.label : "Count";
    }

    setSelectedGlobalAggField(value) {
        const targetValue = `${value || "count"}`.trim() || "count";
        this.globalAggField = targetValue;
        const select = document.getElementById("analyticsGridGlobalAggSelect");
        if (!select) {
            return;
        }
        const hasOption = Array.from(select.options).some(option =>
            this.normalizeUrlParam(option.value) === this.normalizeUrlParam(targetValue)
        );
        if (hasOption) {
            select.value = targetValue;
        } else {
            select.value = "count";
            this.globalAggField = "count";
        }
    }

    ensureGlobalGridAggControl() {
        const pageHeader = document.querySelector(".card-header.Page-title");
        if (!pageHeader) {
            return;
        }

        let host = document.getElementById("analyticsGridAggHost");
        if (!host) {
            host = document.createElement("div");
            host.id = "analyticsGridAggHost";
            host.className = "analytics-grid-global-agg-host";
            host.innerHTML = `
                <label for="analyticsGridGlobalAggSelect" class="analytics-grid-global-agg-label">Aggregate</label>
                <select id="analyticsGridGlobalAggSelect" class="form-select form-select-sm analytics-grid-global-agg-select" title="Aggregate Field"></select>
            `;
            const titleElement = pageHeader.querySelector("h3.card-title");
            if (titleElement && titleElement.nextSibling) {
                pageHeader.insertBefore(host, titleElement.nextSibling);
            } else {
                pageHeader.appendChild(host);
            }
        }

        const select = host.querySelector("#analyticsGridGlobalAggSelect");
        if (!select) {
            return;
        }

        const options = this.getGridAggFields();
        const currentValue = this.globalAggField || select.value || "count";
        select.innerHTML = options.map(option =>
            `<option value="${this.escapeHtml(option.value)}">${this.escapeHtml(option.label)}</option>`
        ).join("");

        const targetValue = options.some(option =>
            this.normalizeUrlParam(option.value) === this.normalizeUrlParam(currentValue)
        ) ? currentValue : "count";
        select.value = targetValue;
        this.globalAggField = targetValue;

        if (!select.dataset.boundChange) {
            select.addEventListener("change", () => {
                this.globalAggField = select.value || "count";
                this.saveGridCardState();
                const container = document.querySelector("#analytics-yaxis-grid");
                const cards = container ? Array.from(container.querySelectorAll(".analytics-grid-card")) : [];
                if (cards.length > 0) {
                    this.loadGridCardsBatch(cards, { forceRefresh: false });
                }
            });
            select.dataset.boundChange = "T";
        }
    }

    promptAndSavePreferences() {
        if (!_entityCommon || typeof _entityCommon.setAnalyticsDataWS !== "function" || _entityCommon.inValid(this.entityTransId)) {
            showAlertDialog("warning", "No active SmartList is selected to save.");
            return;
        }

        const shouldSave = window.confirm("Save current SmartList changes?");
        if (!shouldSave) {
            return;
        }

        const applyAllUsers = window.confirm("Apply these changes for all users?\nClick OK for All Users, or Cancel for Myself only.");
        this.saveGridCardState();

        const statePayload = this.readGridCardState(this.entityTransId);
        const normalizedState = this.normalizeGridCardStatePayload(statePayload || {}, {
            storedOn: new Date().toISOString()
        });

        const payload = {
            page: "Analytics",
            transId: this.entityTransId,
            properties: {
                "XAXISFIELDS": this.xAxisFields || "All",
                "YAXISFIELDS": this.yAxisFields || "All",
                "CHARTTYPE": this.selectedChartType || "line",
                "GLOBALAGGFIELD": this.getSelectedGlobalAggField() || "count",
                "GRIDCARDSTATE": JSON.stringify(normalizedState)
            },
            confirmNeeded: false,
            allUsers: !!applyAllUsers
        };

        _entityCommon.setAnalyticsDataWS(payload, () => {
            this.mergeAnalyticsPrefsCache(this.entityTransId, {
                XAXISFIELDS: this.xAxisFields || "All",
                YAXISFIELDS: this.yAxisFields || "All",
                CHARTTYPE: this.selectedChartType || "line",
                GLOBALAGGFIELD: this.getSelectedGlobalAggField() || "count"
            });
            showAlertDialog("success", applyAllUsers
                ? "Changes saved for all users."
                : "Changes saved for your user.");
        }, (error) => {
            showAlertDialog("error", error?.status
                ? `${error.status} ${error.statusText || ""}`.trim()
                : "Unable to save changes.");
        });
    }

    ensureAnalyticsSaveButton() {
        const toolbar = document.querySelector(".Tkts-toolbar-Right");
        if (!toolbar) {
            return;
        }

        let saveButton = toolbar.querySelector("#btn_AnalyticsSaveScope");
        if (!saveButton) {
            const settingsButton = toolbar.querySelector("#btn_selectfields");
            const buttonHtml = `<button id="btn_AnalyticsSaveScope" type="button" title="Save Preferences" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary btn-custom-border-radius" aria-label="Save Preferences">
                                    <span class="material-icons material-icons-style material-icons-2">save</span>
                                </button>`;
            if (settingsButton) {
                settingsButton.insertAdjacentHTML("beforebegin", buttonHtml);
            } else {
                toolbar.insertAdjacentHTML("afterbegin", buttonHtml);
            }
            saveButton = toolbar.querySelector("#btn_AnalyticsSaveScope");
        }

        if (!saveButton || saveButton.dataset.analyticsSaveBound === "T") {
            return;
        }

        saveButton.addEventListener("click", (event) => {
            event.preventDefault();
            this.promptAndSavePreferences();
        });
        saveButton.dataset.analyticsSaveBound = "T";
    }

    ensureChartControls() {
        this.initializeChartTypeDropdown();
        this.ensureDateRangeDropdown();
        this.ensureAnalyticsFilterButton();
        this.ensureAnalyticsSaveButton();
        this.ensureEntityFilterBridge();
    }

    ensureAnalyticsFilterButton() {
        const toolbar = document.querySelector(".Tkts-toolbar-Right");
        if (!toolbar) {
            return;
        }

        let filterButton = toolbar.querySelector("#btn_AnalyticsFilter")
            || toolbar.querySelector("button[onclick*='openFilters']");

        if (!filterButton) {
            const refreshButton = toolbar.querySelector("#btn_AnalyticsRefresh");
            const buttonHtml = `<button id="btn_AnalyticsFilter" type="button" title="Apply Filter(s)" class="btn btn-sm btn-icon btn-white btn-color-gray-600 btn-active-primary btn-custom-border-radius" aria-label="Apply Filter(s)">
                                    <span class="material-icons material-icons-style material-icons-2">tune</span>
                                </button>`;
            if (refreshButton) {
                refreshButton.insertAdjacentHTML("afterend", buttonHtml);
            } else {
                toolbar.insertAdjacentHTML("afterbegin", buttonHtml);
            }
            filterButton = toolbar.querySelector("#btn_AnalyticsFilter");
        }

        if (!filterButton) {
            return;
        }

        filterButton.id = "btn_AnalyticsFilter";
        filterButton.classList.remove("d-none");
        filterButton.setAttribute("title", "Apply Filter(s)");
        filterButton.setAttribute("aria-label", "Apply Filter(s)");

        const hasInlineOpenFilters = `${filterButton.getAttribute("onclick") || ""}`.toLowerCase().indexOf("openfilters") > -1;
        if (!hasInlineOpenFilters && !filterButton.dataset.analyticsFilterBound) {
            filterButton.addEventListener("click", (event) => {
                event.preventDefault();
                openFilters();
            });
            filterButton.dataset.analyticsFilterBound = "T";
        }
    }

    ensureEntityFilterPillsContainer() {
        let container = document.querySelector(".filterPills");
        if (!container) {
            container = document.querySelector("#analytics-grid-filter-summary");
        }

        if (!container) {
            const pageHeader = document.querySelector(".card-header.Page-title");
            if (!pageHeader) {
                return null;
            }
            pageHeader.insertAdjacentHTML("afterend", `<div id="analytics-grid-filter-summary"></div>`);
            container = document.querySelector("#analytics-grid-filter-summary");
        }

        if (!container) {
            return null;
        }

        if (container.querySelector(".analytics-grid-filter-summary-inner")) {
            container.innerHTML = "";
        }
        container.classList.add("filterPills", "flex-row", "py-2", "px-2", "gap-3", "col-12");
        return container;
    }

    refreshEntityFilterPills() {
        if (typeof _entityFilter === "undefined" || !_entityFilter || typeof _entityFilter.createFilterPills !== "function") {
            return;
        }

        this.ensureEntityFilterPillsContainer();
        try {
            _entityFilter.createFilterPills();
        } catch (error) {
            console.error("analytics createFilterPills failed:", error);
        }
    }

    ensureEntityFilterBridge() {
        if (typeof _entityFilter === "undefined" || !_entityFilter) {
            return;
        }

        _entityFilter.metaData = Array.isArray(this.metaData) ? this.metaData : [];
        _entityFilter.pageName = "Analytics";
        _entityFilter.entityTransId = this.entityTransId || this.dataSourceName || "";
        _entityFilter.filterObj = (_entityFilter.filterObj && typeof _entityFilter.filterObj === "object")
            ? _entityFilter.filterObj
            : (this.filterObj && typeof this.filterObj === "object" ? this.filterObj : {});
        this.filterObj = _entityFilter.filterObj;
        this.ensureEntityFilterPillsContainer();

        if (!_entityFilter._analyticsCreatePillsPatched && typeof _entityFilter.createFilterPills === "function") {
            const originalCreateFilterPills = _entityFilter.createFilterPills.bind(_entityFilter);
            _entityFilter.createFilterPills = function () {
                const response = originalCreateFilterPills();
                try {
                    const hasAny = this.filterObj && Object.keys(this.filterObj).length > 0;
                    const pillsContainer = document.querySelector(".filterPills");
                    if (!pillsContainer) {
                        return response;
                    }

                    if (hasAny) {
                        pillsContainer.classList.remove("d-none");
                        pillsContainer.style.display = "flex";
                    } else {
                        pillsContainer.classList.add("d-none");
                        pillsContainer.style.display = "none";
                    }
                } catch (error) { }
                return response;
            };
            _entityFilter._analyticsCreatePillsPatched = true;
        }

        if (!_entityFilter._analyticsEditPillPatched && typeof _entityFilter.editPill === "function") {
            const originalEditPill = _entityFilter.editPill.bind(_entityFilter);
            _entityFilter.editPill = function (key) {
                try {
                    const dv = document.getElementById("dvModalFilter");
                    if (dv) {
                        dv.innerHTML = "";
                    }
                } catch (error) { }
                return originalEditPill(key);
            };
            _entityFilter._analyticsEditPillPatched = true;
        }

        if (!_entityFilter._analyticsRemoveFilterPatched && typeof _entityFilter.removeFilter === "function") {
            _entityFilter.removeFilter = function (key) {
                try {
                    if (!key) {
                        return;
                    }

                    this.filterObj = this.filterObj || {};
                    if (this.filterObj[key]) {
                        delete this.filterObj[key];
                    }

                    try {
                        if (typeof this.createFilterPills === "function") {
                            this.createFilterPills();
                        }
                    } catch (error) { }

                    const remainingKeys = Object.keys(this.filterObj || {});
                    if (remainingKeys.length > 0) {
                        const nextKey = remainingKeys[0];
                        const nextFilter = this.filterObj[nextKey] || {};
                        this.activeFilterId = nextKey;
                        this.activeFilterName = nextFilter.caption || "";
                        this.activeFilterArray = Array.isArray(nextFilter.filter) ? nextFilter.filter : [];
                    } else {
                        this.activeFilterId = "";
                        this.activeFilterName = "";
                        this.activeFilterArray = [];
                    }

                    if (typeof this.applyFilters === "function") {
                        this.applyFilters();
                    }

                    try {
                        const savedObj = {};
                        remainingKeys.forEach(currentKey => {
                            const item = this.filterObj[currentKey];
                            if (item && item.save === true) {
                                savedObj[currentKey] = item;
                            }
                        });

                        if (typeof _entityCommon !== "undefined" && _entityCommon && typeof _entityCommon.setAnalyticsDataWS === "function") {
                            _entityCommon.setAnalyticsDataWS({
                                page: this.pageName,
                                transId: this.entityTransId,
                                properties: { FILTERS: JSON.stringify(savedObj) },
                                allUsers: false
                            }, () => { }, () => { });
                        }
                    } catch (error) { }
                } catch (error) {
                    console.error("analytics patched removeFilter failed:", error);
                }
            };
            _entityFilter._analyticsRemoveFilterPatched = true;
        }

        if (!_entityFilter._analyticsDropdownPatched && typeof _entityFilter.initializeDropdowns === "function") {
            _entityFilter.initializeDropdowns = function () {
                try {
                    window._analyticsFilterDropdownCache = window._analyticsFilterDropdownCache || {};
                    const pageSize = 500;
                    const jq = window.jQuery || window.$;
                    const hasSelect2 = !!(jq && jq.fn && typeof jq.fn.select2 === "function");
                    const metaData = Array.isArray(this.metaData) ? this.metaData : [];

                    const normalizeTF = (value, fallback = "F") => {
                        const normalizedValue = `${value || ""}`.trim().toUpperCase();
                        if (normalizedValue === "T" || normalizedValue === "TRUE" || normalizedValue === "1" || normalizedValue === "Y" || normalizedValue === "YES") {
                            return "T";
                        }
                        if (normalizedValue === "F" || normalizedValue === "FALSE" || normalizedValue === "0" || normalizedValue === "N" || normalizedValue === "NO") {
                            return "F";
                        }
                        return fallback;
                    };

                    const parseAxListResponse = (response) => {
                        let parsed = response;
                        try {
                            if (typeof parsed === "string") {
                                parsed = JSON.parse(parsed);
                            }
                            if (parsed && typeof parsed.d === "string") {
                                parsed = JSON.parse(parsed.d);
                            } else if (parsed && parsed.d && typeof parsed.d === "object" && !parsed.result) {
                                parsed = parsed.d;
                            }
                        } catch (error) {
                            console.error("analytics ds_smartlist_filters parse error:", error);
                        }

                        if (typeof safeParseAxResponse === "function") {
                            try {
                                parsed = safeParseAxResponse(parsed);
                            } catch (error) {
                                // ignore safe parser failures and continue with current parsed value
                            }
                        } else if (typeof _analyticsCharts !== "undefined"
                            && _analyticsCharts
                            && typeof _analyticsCharts.parseAxListResponsePayload === "function") {
                            try {
                                parsed = _analyticsCharts.parseAxListResponsePayload(parsed);
                            } catch (error) {
                                // ignore fallback parser failures and continue
                            }
                        }

                        const dsBlock = parsed?.result?.data?.[0] || parsed?.data?.[0] || parsed?.result || parsed;
                        let rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
                        if (rows && rows.length && rows[0] && Array.isArray(rows[0].data)) {
                            rows = rows[0].data;
                        }
                        return Array.isArray(rows) ? rows : [];
                    };

                    const mapRowToOption = (row) => {
                        if (row === null || row === undefined) {
                            return null;
                        }
                        if (typeof row === "string" || typeof row === "number") {
                            const value = String(row).trim();
                            return value ? { id: value, text: value } : null;
                        }
                        if (typeof row !== "object") return null;

                        const id = row.datavalue ?? row.value ?? row.id ?? row.code ?? row.name ?? row.text;
                        const text = row.datacaption ?? row.caption ?? row.text ?? row.value ?? row.name ?? row.code ?? row.id;

                        const idStr = (id === null || id === undefined) ? "" : String(id).trim();
                        const textStr = (text === null || text === undefined) ? "" : String(text).trim();
                        const cleanId = (idStr && idStr.toLowerCase() !== "null") ? idStr : "";
                        const cleanText = (textStr && textStr.toLowerCase() !== "null") ? textStr : cleanId;

                        if (!cleanId) {
                            for (const key in row) {
                                const value = row[key];
                                if (value === null || value === undefined) continue;
                                const candidate = String(value).trim();
                                if (!candidate || candidate.toLowerCase() === "null") continue;
                                return { id: candidate, text: candidate };
                            }
                            return null;
                        }

                        return {
                            id: cleanId,
                            text: cleanText || cleanId
                        };
                    };

                    const getFieldMeta = (fieldId) => {
                        const normalizedField = `${fieldId || ""}`.trim().toLowerCase();
                        return metaData.find(item => `${item?.fldname || ""}`.trim().toLowerCase() === normalizedField) || null;
                    };

                    const buildPsrcTxt = (fieldMeta, fieldId) => {
                        const direct = `${fieldMeta?.psrctxt || fieldMeta?.psrcTxt || fieldMeta?.psrctext || ""}`.trim();
                        if (direct) {
                            return direct;
                        }

                        const fldName = `${fieldMeta?.fldname || fieldId || ""}`.trim();
                        const normalizedToken = normalizeTF(
                            fieldMeta?.normalized ?? fieldMeta?.isnormalized ?? fieldMeta?.isNormalized ?? fieldMeta?.is_normalized,
                            "F"
                        );
                        let srcTable = `${fieldMeta?.srctable || fieldMeta?.src_table || fieldMeta?.sourcetable || fieldMeta?.srctbl || ""}`.trim();
                        let srcField = `${fieldMeta?.srcfld || fieldMeta?.src_fld || fieldMeta?.sourcefld || fieldMeta?.sourcefield || fieldMeta?.srcfield || ""}`.trim();

                        if (!fldName) {
                            return "";
                        }

                        if (!srcTable || !srcField) {
                            const fallbackEntity = `${fieldMeta?.ftransid || ((typeof _analyticsCharts !== "undefined" && _analyticsCharts) ? (_analyticsCharts.entityTransId || _analyticsCharts.dataSourceName || "") : "")}`.trim();
                            if (fallbackEntity) {
                                srcTable = srcTable || fallbackEntity;
                                srcField = srcField || fldName;
                            }
                        }

                        if (!srcTable || !srcField) {
                            return "";
                        }

                        return `${fldName}~${normalizedToken}~${srcTable}~${srcField}`;
                    };

                    const caller = (typeof parent !== "undefined" && typeof parent.GetDataFromAxList === "function")
                        ? parent
                        : (typeof window !== "undefined" && typeof window.GetDataFromAxList === "function" ? window : null);
                    if (!caller || typeof caller.GetDataFromAxList !== "function") {
                        console.error("GetDataFromAxList not available for analytics filter dropdowns");
                        return;
                    }

                    let dropdownFields = Array.from(document.querySelectorAll("#dvModalFilter .filter-fld[data-type=DropDown]"));
                    if (dropdownFields.length === 0) {
                        dropdownFields = Array.from(document.querySelectorAll("#dvModalFilter .filter-fld"))
                            .filter(item => `${item?.dataset?.type || ""}`.trim().toUpperCase() === "DROPDOWN");
                    }

                    const fetchDropdownOptions = (psrctxt, term, onSuccess, onError) => {
                        try {
                            const normalizedTerm = `${term || ""}`.trim().toLowerCase();
                            const cacheKey = psrctxt;
                            const cachedOptions = window._analyticsFilterDropdownCache[cacheKey];

                            const applySearch = (options) => {
                                const safeOptions = Array.isArray(options) ? options : [];
                                const filtered = normalizedTerm
                                    ? safeOptions.filter(item => `${item?.text || ""}`.toLowerCase().includes(normalizedTerm) || `${item?.id || ""}`.toLowerCase().includes(normalizedTerm))
                                    : safeOptions;
                                onSuccess(filtered.slice(0, pageSize));
                            };

                            if (Array.isArray(cachedOptions) && cachedOptions.length > 0) {
                                applySearch(cachedOptions);
                                return;
                            }

                            const requestPayload = {
                                adsNames: ["ds_smartlist_filters"],
                                refreshCache: false,
                                sqlParams: { psrctxt: `${psrctxt}` },
                                props: {
                                    ADS: true,
                                    CachePermissions: true,
                                    getallrecordscount: false,
                                    pageno: 1,
                                    pagesize: 0
                                }
                            };

                            caller.GetDataFromAxList(requestPayload, (response) => {
                                try {
                                    const rows = parseAxListResponse(response);
                                    const seenIds = new Set();
                                    const options = [];
                                    rows.forEach(row => {
                                        const option = mapRowToOption(row);
                                        if (!option || !option.id) {
                                            return;
                                        }
                                        const normalizedId = option.id.toLowerCase();
                                        if (seenIds.has(normalizedId)) {
                                            return;
                                        }
                                        seenIds.add(normalizedId);
                                        options.push(option);
                                    });
                                    window._analyticsFilterDropdownCache[cacheKey] = options;
                                    applySearch(options);
                                } catch (parseError) {
                                    console.error("analytics ds_smartlist_filters parse error:", parseError);
                                    onSuccess([]);
                                }
                            }, (error) => {
                                console.error("analytics ds_smartlist_filters call failed:", error);
                                onSuccess([]);
                            });
                        } catch (error) {
                            if (typeof onError === "function") {
                                onError(error);
                            }
                        }
                    };

                    dropdownFields.forEach((element) => {
                        try {
                            const fieldId = `${element.id || ""}`.replace("filter_", "");
                            const fieldMeta = getFieldMeta(fieldId);
                            const psrctxt = buildPsrcTxt(fieldMeta, fieldId);
                            if (!psrctxt) {
                                console.warn("analytics filter dropdown skipped (psrctxt missing):", fieldId, fieldMeta);
                                return;
                            }

                            if (hasSelect2 && `${element.tagName || ""}`.toUpperCase() === "SELECT") {
                                const $element = jq(element);
                                let $wrapper = $element.closest(".dropdown-wrapper");
                                if ($wrapper.length === 0) {
                                    $wrapper = jq('<div class="dropdown-wrapper" style="position:relative;"></div>');
                                    $element.wrap($wrapper);
                                    $wrapper = $element.closest(".dropdown-wrapper");
                                }

                                if ($wrapper.find(".dropdown-toggle-btn").length === 0) {
                                    $wrapper.append(`<button type="button" class="dropdown-toggle-btn btn btn-sm" aria-label="Open" title="Open"
                                        style="position:absolute; right:36px; top:6px; z-index:1100; height:30px; padding:0 6px; line-height:1;">&#9662;</button>`);
                                }
                                if ($wrapper.find(".clear-all-btn").length === 0) {
                                    $wrapper.append(`<button type="button" class="clear-all-btn btn btn-sm" aria-label="Clear" title="Clear"
                                        style="position:absolute; right:4px; top:6px; z-index:1100; height:30px; padding:0 6px; line-height:1; display:none;">&#10005;</button>`);
                                }

                                const $openButton = $wrapper.find(".dropdown-toggle-btn");
                                const $clearButton = $wrapper.find(".clear-all-btn");

                                if (!$element.hasClass("select2-hidden-accessible")) {
                                    $element.select2({
                                        multiple: true,
                                        width: "100%",
                                        minimumInputLength: 0,
                                        ajax: {
                                            delay: 150,
                                            transport: (params, success, failure) => {
                                                const term = `${params?.data?.term || ""}`.trim();
                                                fetchDropdownOptions(psrctxt, term, (options) => {
                                                    success({ results: options });
                                                }, failure);
                                            },
                                            processResults: (data) => data,
                                            cache: true
                                        },
                                        escapeMarkup: (markup) => markup
                                    });
                                }

                                $openButton.off("click.analyticsDropOpen").on("click.analyticsDropOpen", (event) => {
                                    event.preventDefault();
                                    try { $element.select2("open"); } catch (error) { }
                                });

                                $clearButton.off("click.analyticsDropClear").on("click.analyticsDropClear", (event) => {
                                    event.preventDefault();
                                    $element.val(null).trigger("change");
                                    try { $element.select2("close"); } catch (error) { }
                                });

                                const updateClearVisibility = () => {
                                    const selectedValues = $element.val() || [];
                                    if (selectedValues.length > 0) {
                                        $clearButton.show();
                                    } else {
                                        $clearButton.hide();
                                    }
                                };

                                $element.off(".analyticsDropEvents")
                                    .on("change.analyticsDropEvents", updateClearVisibility)
                                    .on("select2:select.analyticsDropEvents", updateClearVisibility)
                                    .on("select2:unselect.analyticsDropEvents", updateClearVisibility);
                                updateClearVisibility();
                            } else {
                                if (element.dataset.analyticsDsBound === "T") {
                                    return;
                                }

                                const bindLoad = () => {
                                    fetchDropdownOptions(psrctxt, "", (options) => {
                                        const selectedValues = Array.isArray(element.value) ? element.value : Array.from(element.selectedOptions || []).map(item => item.value);
                                        element.innerHTML = "";
                                        options.forEach(option => {
                                            const optionElement = document.createElement("option");
                                            optionElement.value = option.id;
                                            optionElement.textContent = option.text || option.id;
                                            if (selectedValues.indexOf(option.id) > -1) {
                                                optionElement.selected = true;
                                            }
                                            element.appendChild(optionElement);
                                        });
                                    });
                                };

                                element.addEventListener("focus", bindLoad);
                                element.addEventListener("mousedown", bindLoad);
                                element.addEventListener("click", bindLoad);
                                element.dataset.analyticsDsBound = "T";
                            }
                        } catch (dropdownError) {
                            console.error("analytics dropdown initialization error:", dropdownError);
                        }
                    });
                } catch (error) {
                    console.error("analytics initializeDropdowns patch failed:", error);
                }
            };

            _entityFilter._analyticsDropdownPatched = true;
        }

        if (_entityFilter._analyticsFilterBridgeApplied === true) {
            return;
        }

        _entityFilter.applyFilters = () => {
            const activeFilters = Array.isArray(_entityFilter.activeFilterArray)
                ? JSON.parse(JSON.stringify(_entityFilter.activeFilterArray))
                : [];

            this.filter = activeFilters;
            this.filterObj = (_entityFilter.filterObj && typeof _entityFilter.filterObj === "object")
                ? _entityFilter.filterObj
                : {};
            this.gridSelectionFilter = {
                groupField: "",
                values: [],
                filters: {}
            };

            this.gridBatchChartsByKey = {};
            this.gridBatchRequests = {};

            this.logFilterDebug("entityFilter:applyFilters", {
                transId: this.entityTransId || this.dataSourceName || "",
                filtersCount: activeFilters.length,
                filters: activeFilters
            });

            this.refreshEntityFilterPills();

            const cards = Array.from(document.querySelectorAll("#analytics-yaxis-grid .analytics-grid-card"));
            if (cards.length > 0) {
                this.loadGridCardsBatch(cards, { forceRefresh: false });
            } else {
                this.refreshCurrentChart();
            }

            try {
                $('#filterModal').modal('hide');
            } catch (error) {
                // ignore modal errors in non-bootstrap context
            }
        };

        _entityFilter._analyticsFilterBridgeApplied = true;
    }

    initializeChartTypeDropdown() {
        const menu = document.querySelector(".chart-selections .dropdown-menu");
        if (!menu) {
            return;
        }

        const chartTypes = this.getSupportedChartTypes();
        menu.innerHTML = chartTypes.map(item => {
            const iconStyleAttr = item.iconStyle ? ` style="${item.iconStyle}"` : "";
            return `<li>
                        <a href="javascript:void(0);" onclick="chartSelectionClick(event, this)" class="dropdown-item menu-link" chart_type="${item.type}">
                            <span class="material-icons material-icons-style material-icons-2"${iconStyleAttr}>${item.icon}</span>
                            <span class="chart-name">${item.name}</span>
                        </a>
                    </li>`;
        }).join("");
    }

    ensureDateRangeDropdown() {
        const chartHeader = document.querySelector("#analytics-chart-title");
        if (!chartHeader || document.querySelector("#analytics-date-filter-wrap")) {
            return;
        }

        const filterHtml = `<div id="analytics-date-filter-wrap" class="d-inline ms-3 d-none">
                                <select id="analytics-date-filter" class="form-select form-select-sm">
                                    <option value="this_week">This week</option>
                                    <option value="this_month" selected>This month</option>
                                    <option value="this_year">This year</option>
                                    <option value="days">Days (Last 30)</option>
                                </select>
                            </div>`;
        const chartSelections = chartHeader.querySelector(".chart-selections");
        if (chartSelections) {
            chartSelections.insertAdjacentHTML("afterend", filterHtml);
        } else {
            chartHeader.insertAdjacentHTML("beforeend", filterHtml);
        }

        const filterElement = document.querySelector("#analytics-date-filter");
        if (filterElement) {
            filterElement.value = this.selectedDateRange;
            filterElement.addEventListener("change", () => {
                this.selectedDateRange = filterElement.value || "this_month";
                this.refreshCurrentChart();
            });
        }
    }

    refreshCurrentChart() {
        const selectedAnchor = document.querySelector(".Aggregation-item.selected");
        if (selectedAnchor) {
            selectAggField(selectedAnchor, this.selectedChartType);
        }
    }
    // Update the chart button based on the selected chart type
    updateChartButton(chartType) {
        const config = this.getChartTypeConfig(chartType);
        this.selectedChartType = config.type;
        var button = document.querySelector('.btn.dropdown-toggle');
        if (button) {
            button.querySelector('.chart-name').textContent = `${config.name} Chart`;
            button.querySelector('.material-icons').textContent = config.icon;
        }

        document.querySelectorAll(".chart-selections .dropdown-item").forEach(link => {
            const isSelected = this.normalizeUrlParam(link.getAttribute("chart_type") || link.dataset.chartType || "") === config.type;
            link.classList.toggle("bg-success", isSelected);
        });
    }

    getAnalyticsChartsDataWS(input) {
        let _this = this;

        this.fetchAnalyticsRawData(input).then(chartsData => {
            try {
                this.updateLastRefreshDateTime();
                if (!Array.isArray(chartsData) || chartsData.length === 0) {
                    handleNoChartData();
                    document.querySelector('#KPI-2 .row').innerHTML = "";
                    document.querySelector("#Homepage_CardsList_Wrapper").innerHTML = "";
                    document.querySelector('#analytics-chart-title #chart-title').innerHTML = `${_analyticsCharts.entityName}(s)`;
                    return;
                }

                handleValidChartData();
                document.querySelector("#Homepage_CardsList_Wrapper").innerHTML = "";

                if (input.aggField === "count" && input.groupField === "all") {
                    var kpiJson = chartsData[0];
                    var html = _analyticsCharts.generateGeneralKPIHTML(kpiJson);
                    document.querySelector('#KPI-2 .row').innerHTML = html;
                    document.querySelector("#Homepage_CardsList_Wrapper").innerHTML = "";
                    document.querySelector('#analytics-chart-title #chart-title').innerHTML = `${_analyticsCharts.entityName}(s)`;

                    let map = {
                        "totrec": "Total records",
                        "cyear": "This year",
                        "cmonth": "This month",
                        "cweek": "This week",
                        "cyesterday": "Yesterday",
                        "ctoday": "Today"
                    };

                    let chartJson = Object.keys(kpiJson)
                        .filter(item => item !== "cnd" && item !== "creiteria" && item != null && kpiJson[item] != null && kpiJson[item] !== 0)
                        .map(key => ({ data_label: map[key], value: kpiJson[key] }));

                    var finalDataObj = [];
                    var chartTypeToUse = _this.selectedChartType || document.querySelector(".chart-selections a.bg-success")?.getAttribute("chart_type");
                    finalDataObj.push({
                        "chartsid": "General",
                        "charttype": "chart",
                        "chartjson": JSON.stringify(chartJson),
                        "chartname": `criteria`,
                        "chart": chartTypeToUse
                    });

                    _analyticsCharts.chartsJson = finalDataObj;
                } else {
                    let kpiJson = chartsData;
                    let criteria = kpiJson[0].criteria || "";
                    let caption = "Empty data";
                    const groupField = (criteria.split("~")[2] || "").trim();

                    if (!groupField) {
                        caption = kpiJson[0].keyname || _analyticsCharts.getChartCaptions(criteria) || caption;
                    }

                    document.querySelector('#analytics-chart-title #chart-title').innerHTML = _analyticsCharts.getChartCaptions(criteria);

                    var chartJson = kpiJson.map(item => ({
                        data_label: item.keyname || caption,
                        value: item.keyvalue || 0
                    }));

                    document.querySelector("#Homepage_CardsList_Wrapper").innerHTML = "";
                    html = _analyticsCharts.generateCustomKPIHTML(kpiJson, caption);
                    document.querySelector('#KPI-2 .row').innerHTML = html;
                    chartTypeToUse = _this.selectedChartType || document.querySelector(".chart-selections a.bg-success")?.getAttribute("chart_type");

                    finalDataObj = [];
                    finalDataObj.push({
                        "chartsid": "General",
                        "charttype": "chart",
                        "chartjson": JSON.stringify(chartJson),
                        "chartname": `criteria`,
                        "chart": chartTypeToUse
                    });

                    _analyticsCharts.chartsJson = finalDataObj;
                    $("#Homepage_CardsList_Wrapper").show();
                }

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

                const assetBaseUrl = _this.getAssetBaseUrl();
                const toAssetUrl = path => _this.toAssetUrl(path, assetBaseUrl);

                files.js.push(toAssetUrl("/ThirdParty/lodash.min.js"));
                files.js.push(toAssetUrl("/ThirdParty/deepdash.min.js"));
                files.js.push(toAssetUrl("/Js/handlebars.min.js?v=1"));
                files.js.push(toAssetUrl("/Js/handleBarsHelpers.min.js"));
                files.js.push(toAssetUrl("/ThirdParty/Highcharts/highcharts.js"));
                files.js.push(toAssetUrl("/ThirdParty/Highcharts/highcharts-3d.js"));
                files.js.push(toAssetUrl("/ThirdParty/Highcharts/highcharts-more.js"));
                files.js.push(toAssetUrl("/ThirdParty/Highcharts/highcharts-exporting.js"));
                files.js.push(toAssetUrl("/AxpertPlugins/Axi/HTMLPages/js/analytics-charts-functions.js"));
                files.js.push(toAssetUrl("/Js/AxInterface.js?v=10"));
                files.js.push(toAssetUrl("/ThirdParty/DataTables-1.10.13/media/js/jquery.dataTables.js"));
                files.js.push(toAssetUrl("/ThirdParty/DataTables-1.10.13/media/js/dataTables.bootstrap.js"));
                files.js.push(toAssetUrl("/ThirdParty/DataTables-1.10.13/extensions/Extras/moment.min.js"));
                files.css.push(toAssetUrl("/ThirdParty/fullcalendar/lib/main.min.css"));
                files.js.push(toAssetUrl("/ThirdParty/fullcalendar/lib/main.min.js"));

                if (cardsDashboardObj.isMobile) {
                    files.js.push(toAssetUrl("/ThirdParty/jquery-ui-touch-punch-master/jquery.ui.touch-punch.min.js"));
                }

                if (cardsDashboardObj.enableMasonry) {
                    files.js.push(toAssetUrl("/ThirdParty/masonry/masonry.pkgd.min.js"));
                }

                files.js.push(toAssetUrl(`/js/entity-charts.min.js?v=2`));

                if (document.getElementsByTagName("body")[0].classList.contains("btextDir-rtl")) {
                    cardsDashboardObj.dirLeft = false;
                }

                loadAndCall({
                    files: files,
                    callBack: () => {
                        $(function () {
                            ShowDimmer(true);
                            deepdash(_);
                            var cardVisibleArray = [];

                            cardsData.value = JSON.stringify(_analyticsCharts.chartsJson);
                            cardsDesign.value = JSON.stringify(cardVisibleArray);
                            xmlMenuData = "";

                            if (xmlMenuData !== "") {
                                xmlMenuData = xmlMenuData.replace(/'/g, "'");
                                var xml = parseXml(xmlMenuData);
                                var xmltojson = xml2json(xml, "");
                                menuJson = JSON.parse(xmltojson);
                            }

                            appGlobalVarsObject._CONSTANTS.menuConfiguration = $.extend(true, {},
                                appGlobalVarsObject._CONSTANTS.menuConfiguration, {
                                menuJson: menuJson,
                            });

                            appGlobalVarsObject._CONSTANTS.cardsPage = {
                                setCards: false,
                                cards: []
                            };

                            $.axpertUI.options.cardsPage.cards = [];
                            appGlobalVarsObject._CONSTANTS.cardsPage = $.extend(true, {},
                                appGlobalVarsObject._CONSTANTS.cardsPage, {
                                setCards: true,
                                cards: (JSON.parse(cardsData.value !== "" ? ReverseCheckSpecialChars(cardsData.value) : "[]",
                                    function (k, v) {
                                        try {
                                            return (typeof v === "object" || isNaN(v) || v.toString().trim() === "") ? v : (typeof v === "string" && (v.startsWith("0") || v.startsWith("-")) ? parseFloat(v, 10) : JSON.parse(v));
                                        } catch (ex) {
                                            return v;
                                        }
                                    }
                                ) || []).map(arr => _.mapKeys(arr, (value, key) => key.toString().toLowerCase())),
                                design: (JSON.parse(cardsDesign.value !== "" ? cardsDesign.value : "[]",
                                    function (k, v) {
                                        try {
                                            return (typeof v === "object" || isNaN(v) || v.toString().trim() === "") ? v : (typeof v === "string" && (v.startsWith("0") || v.startsWith("-")) ? parseFloat(v, 10) : JSON.parse(v));
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
                                        cardsDiv: "#Homepage_CardsList_Wrapper",
                                        cardsDesigner: ".cardsDesigner",
                                        cardsDesignerToolbar: ".designer",
                                        editSaveButton: ".editSaveCardDesign",
                                        icon: "span.material-icons",
                                        divControl: "#arrangeCards"
                                    },
                                }
                            });

                            var tempaxpertUIObj = $.axpertUI.init({
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
                        });

                        $.axpertUI.cardsPage._addCardHeader = function (cardElement) {
                            cardElement.find(".card-header").addClass("d-none");
                        };
                    }
                });
            } catch (e) {
                console.error("Error processing summary data:", e);
            }
        }).catch(error => {
            console.error("Error loading analytics data:", error);
            handleNoChartData();
        });
    }

    getEntityCaption(transId) {
        let _this = this;
        let data = _this.selectedEntitiesList;
        for (let i = 0; i < data.length; i++) {
            if (data[i].name === transId) {
                return data[i].caption;
            }
        }
        return '';
    }

    getAllEntities() {
        if (Array.isArray(this.allEntitiesList) && this.allEntitiesList.length > 0) {
            this.constructEntityPopup(this.allEntitiesList);
            return;
        }

        $.ajax({
            url: `${this.getAssetBaseUrl()}/aspx/Analytics.aspx/GetEntityListWS`,
            type: 'POST',
            cache: false,
            async: true,
            dataType: 'json',
            data: "{}",
            contentType: "application/json",
            success: (data) => {
                try {
                    const parsedResponse = typeof data?.d === "string" ? JSON.parse(data.d) : (data?.d || data);
                    const entities = Array.isArray(parsedResponse?.result?.list) ? parsedResponse.result.list : [];

                    if (!entities.length) {
                        showAlertDialog("warning", "No entities found.");
                        return;
                    }

                    this.allEntitiesList = entities;
                    this.constructEntityPopup(this.allEntitiesList);
                } catch (error) {
                    console.error("Error parsing entity list response:", error);
                    showAlertDialog("error", "Unable to load entity list.");
                }
            },
            error: (error) => {
                console.error("Error fetching entity list:", error);
                showAlertDialog("error", "Unable to load entity list.");
            }
        });
    }

    constructSelectedEntityHeader() {
        var selectedHtml = `<label id="selectedLabel" class="selected-name">Selected Entities</label>`;
        var entityHtml = ""; // Variable for all items HTML

        let _this = this;

        // Loop through the selected entities to create HTML
        _this.selectedEntitiesList.forEach(function (item, index) {
            var initials = _entityCommon.getInitials(item.caption);

            selectedHtml += ` <div data-cardname="${item.caption}" onclick="toggleSelection(${index})" class="EntityData_Select-lists col-lg-FV col-md-3 col-sm-12 col-xs-3">
                                    <div class="EntityData_Select-Items selected">

                                        <h6 class="Entity-title" title="${item.caption}" value="${item.name}">
                                            ${item.caption}(${item.name})
                                        </h6>
                                    </div>

                                </div>`;
        });

        // Append selected items HTML above the existing list
        $("#selectedEntitiesContainer").html(selectedHtml);

        // Loop through the selected entities again to create HTML for all items
        $.each(_this.selectedEntitiesList, function (index, item) {
            var selectedClass = index === 0 ? "active" : "";
            var initials = _entityCommon.getInitials(item.caption);
            entityHtml += `<li class="nav-item" onclick="selectEntity(this, '${item.name}')" data-cardname="${item.caption}" dt_transid="${item.name}">                                
                                <a class="nav-link ${selectedClass}" href="#">${item.caption}</a>
                            </li>`;
        });

        // Update the main container with all items
        $("#dv_EntityContainer").html(entityHtml);

        // Additional logic to handle other elements
        var firstAnchorGroup = $('#Data-Group-container .Data-Group_Items:first a');
        var parentElement = $(firstAnchorGroup).parent();
        $(".Data-Group_Items").removeClass("selected");
        $(parentElement).addClass("selected");

        var firstAnchorAgg = $('.Aggregation-item:first');
        $(".Aggregation-item").removeClass("selected");
        $(firstAnchorAgg).addClass("selected");
        $('#entityModal').modal('hide');
    }

    constructEntityPopup(entityList) {
        var entityHtml = "";

        $.each(entityList, function (index, item) {
            // Check if the item is not already selected
            if (!selectedItemsArray.map(e => e.toLowerCase()).includes(item.name.toLowerCase())) {
                var initials = _entityCommon.getInitials(item.caption);

                entityHtml += `<div data-cardname="${item.caption}" onclick="selectEntityToAdd('${item.caption}', '${item.name}', this)" 
                                class="EntityData_Select-lists col-lg-3 col-md-3 col-sm-12 col-xs-3">
                                    <div class="EntityData_Select-Items">                                        
                                        <h6 class="Entity-title" title="${item.caption}" value="${item.name}">
                                            ${item.caption}(${item.name})
                                        </h6>
                                    </div>
                                    
                            </div>`;
            }
        });

        $("#entityDataContainer").html(entityHtml);

        $('#entityModal').modal('show');

        $('#entityDataContainer .EntityData_Select-lists').each(function () {
            var entityData = $(this).find('.Entity-title').attr('value').toLowerCase();
            if (selectedItemsArray.map(e => e.toLowerCase()).includes(entityData)) {
                $(this).hide();
            }
        });
    }


    constructXandYAxis(metaData, calledFrom) {
        let _this = this;
        $("#Data-Group-container").html("");
        $("#Aggregation_Wrapper").html("");
        let groupHtml = "";
        let aggHtml = "";

        // Default X axis (Aggregation)
        aggHtml += `<div class="col-lg-3 col-sm-4 Aggregation-Item-wrap agg-count" data-dcname="Count">
                        <a href="#" class="Aggregation-item" onclick="selectAggField(this)">
                            <div class="Aggregation-icon">
                                <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                            </div>
                            <div class="Aggregation-content">
                                <h6 class="subtitle"  data-fldname="Count">Count</h6>
                            </div>
                        </a>
                    </div>`;

        // Default Y axis (Grouping)
        groupHtml += `<div class="Data-Group_Items group-all" data-dcname="All">
                        <a href="#" class="group-item" onclick="selectGroupField(this)">
                            <div class="d-flex">
                                <div class="symbol symbol-40px symbol-circle me-5" style="margin-left:5px !important;">
                                    <div class="symbol-label bgs1">
                                        <span class="material-icons material-icons-style material-icons-2">date_range</span>
                                    </div>
                                </div>
                                <div class="d-flex flex-column y-axis-caption">
                                    <span class="Data-Group-name" nonmandatory="F" normalized="F" data-fldname="All" srctbl="All" srcfld="All">All</span>
                                </div>
                            </div>
                        </a>
                      </div>`;

        const xAxisFieldsArr = this.xAxisFields && this.xAxisFields.trim() !== "" && this.xAxisFields.trim() !== "All" ? this.xAxisFields.split(",") : [];
        const yAxisFieldsArr = this.yAxisFields && this.yAxisFields.trim() !== "" && this.yAxisFields.trim() !== "All" ? this.yAxisFields.split(",") : [];

        function getSortOrder(fieldName) {
            if (xAxisFieldsArr.includes(fieldName)) {
                return xAxisFieldsArr.indexOf(fieldName);
            } else if (yAxisFieldsArr.includes(fieldName)) {
                return yAxisFieldsArr.indexOf(fieldName) + xAxisFieldsArr.length;
            }
            return -1;
        }

        if (xAxisFieldsArr.length > 0) {
            let filteredMetaData = metaData.filter(item => xAxisFieldsArr.includes(item.fldname));
            const sortedMetaData = filteredMetaData.sort((a, b) => getSortOrder(a.fldname) - getSortOrder(b.fldname));
            $.each(sortedMetaData, function (index, item) {
                if (item.aggfield === "T" || item.fdatatype === "d") {
                    aggHtml += _this.getAggFldHtml(item);
                }
            });
        }
        else {
            $.each(metaData, function (index, item) {
                if (item.aggfield === "T" && item.hide === "F") {
                    aggHtml += _this.getAggFldHtml(item);
                }
            });
        }

        if (yAxisFieldsArr.length > 0) {
            let filteredMetaData = metaData.filter(item => yAxisFieldsArr.includes(item.fldname));
            const sortedMetaData = filteredMetaData.sort((a, b) => getSortOrder(a.fldname) - getSortOrder(b.fldname));
            $.each(sortedMetaData, function (index, item) {
                if ((item.grpfield === "T" || item.fdatatype === "d") && (item.cdatatype === "DropDown" || item.fdatatype === "c" || item.fdatatype === "d")) {
                    groupHtml += _this.getGroupFldHtml(item);
                }
            });
        }
        else {
            $.each(metaData, function (index, item) {
                if (item.grpfield === "T" && item.hide === "F" && (item.cdatatype === "DropDown" || item.fdatatype === "c" || item.fdatatype === "d")) {
                    groupHtml += _this.getGroupFldHtml(item);
                }
            });
        }

        $("#Data-Group-container").html(groupHtml);
        $("#Aggregation_Wrapper").html(aggHtml);

        if (calledFrom === "Entity Selection" || calledFrom === "Page Load") {
            const ftransidItem = metaData.find(item => item.ftransid);
            let ftransid = ftransidItem ? ftransidItem.ftransid : "default";

            const firstAnchorGroup = $('#Data-Group-container .Data-Group_Items:first a');
            const parentElement = $(firstAnchorGroup).parent();
            $(".Data-Group_Items").removeClass("selected");
            $(parentElement).addClass("selected");

            const firstAnchorAgg = $('.Aggregation-item:first');
            $(".Aggregation-item").removeClass("selected");
            $(firstAnchorAgg).addClass("selected");
            this.toggleDateRangeFilter("all");

            if (!this.enableCardGridLayout) {
                const deferDefaultLoad = this.shouldDeferDefaultLoad();
                if (!deferDefaultLoad) {
                    const groupByApplied = this.applyPendingGroupBy();
                    if (!groupByApplied) {
                        _analyticsCharts.getAnalyticsChartsDataWS({
                            page: "Analytics",
                            transId: ftransid,
                            aggField: "count",
                            aggTransId: ftransid,
                            groupField: "all",
                            groupTransId: ftransid,
                            aggFunc: "count"
                        });
                    }
                }
            }
        }

        if (this.enableCardGridLayout) {
            this.renderAnalyticsGridCards();
        }

        if (calledFrom === "Entity Selection") {
            $('#entityModal').modal('hide');
        }
    }

    getGroupFldHtml(item) {
        return `
        <div class="Data-Group_Items" data-fldname="${item.fldname}" data-dcname="${item.dcname}" data-griddc="${item.griddc}">
            <a href="#" class="group-item" onclick="selectGroupField(this)" >
                <div class="d-flex ">
                    <div class="symbol symbol-40px symbol-circle me-5" style="margin-left:5px !important;">
                        <div class="symbol-label bgs1">
                            <span class="material-icons material-icons-style material-icons-2">date_range</span>
                        </div>
                    </div>
                    <div class="d-flex flex-column y-axis-caption">
                        <span class="Data-Group-name" data-fldname="${item.fldname}">${item.fldcap || item.fldname}</span>
                    </div>
                </div>
            </a>
        </div>`;
    }

    getAggFldHtml(item) {
        return `
        <div class="col-lg-3 col-sm-4 Aggregation-Item-wrap" data-fldname="${item.fldname}" data-dcname="${item.dcname}" data-griddc="${item.griddc}">
            <a href="#" class="Aggregation-item" onclick="selectAggField(this)">
                <div class="Aggregation-icon">
                    <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                </div>
                <div class="Aggregation-content">
                    <h6 class="subtitle"  data-fldname="${item.fldname}" >${item.fldcap || item.fldname}</h6>
                </div>
                <div class="Aggregation-icon2">
                    <button type="button" onclick="handleFuncSelection(event,this)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm bg-success" agg_function="sum" title="Sum">
                        <span class="material-icons material-icons-style material-icons-2">functions</span>
                    </button>
                    <button type="button" onclick="handleFuncSelection(event,this)" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm" agg_function="avg" title="Average">
                        <span class="material-icons material-icons-style material-icons-2">percent</span>
                    </button>
                </div>
            </a>
        </div>`;
    }

    isDateField(fieldName) {
        const normalizedFieldName = this.normalizeUrlParam(fieldName);
        if (!normalizedFieldName || normalizedFieldName === "all" || !Array.isArray(this.metaData)) {
            return false;
        }

        const fieldMeta = this.metaData.find(item => (item.fldname || "").toLowerCase() === normalizedFieldName);
        return fieldMeta ? fieldMeta.fdatatype === "d" : false;
    }

    toggleDateRangeFilter(groupFieldName) {
        const filterWrap = document.querySelector("#analytics-date-filter-wrap");
        if (!filterWrap) {
            return;
        }

        if (this.isDateField(groupFieldName)) {
            this.activeDateField = this.normalizeUrlParam(groupFieldName);
            filterWrap.classList.remove("d-none");
        } else {
            this.activeDateField = "";
            filterWrap.classList.add("d-none");
        }
    }

    applyDateRangeFilter(chartsData, input) {
        if (!Array.isArray(chartsData) || chartsData.length === 0 || !input) {
            return chartsData;
        }

        const groupField = this.normalizeUrlParam(input.groupField || "");
        if (!groupField || groupField === "all" || !this.isDateField(groupField)) {
            return chartsData;
        }

        const dateRange = this.getDateRangeBySelection();
        if (!dateRange) {
            return chartsData;
        }

        return chartsData.filter(item => {
            const keyValue = item ? (item.keyname || item.data_label || item.name) : "";
            const chartDate = this.parseChartDateValue(keyValue);
            if (!chartDate) {
                return false;
            }
            return chartDate >= dateRange.from && chartDate <= dateRange.to;
        });
    }

    getDateRangeBySelection() {
        const now = new Date();
        const toDate = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 23, 59, 59, 999);
        let fromDate = new Date(toDate);
        fromDate.setHours(0, 0, 0, 0);

        switch (this.selectedDateRange) {
            case "this_week":
                const currentDay = now.getDay();
                const diff = now.getDate() - currentDay + (currentDay === 0 ? -6 : 1);
                fromDate = new Date(now.getFullYear(), now.getMonth(), diff, 0, 0, 0, 0);
                break;
            case "this_month":
                fromDate = new Date(now.getFullYear(), now.getMonth(), 1, 0, 0, 0, 0);
                break;
            case "this_year":
                fromDate = new Date(now.getFullYear(), 0, 1, 0, 0, 0, 0);
                break;
            case "days":
                fromDate = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0);
                fromDate.setDate(fromDate.getDate() - 29);
                break;
            default:
                return null;
        }

        return { from: fromDate, to: toDate };
    }

    parseChartDateValue(value) {
        if (_entityCommon.inValid(value)) {
            return null;
        }

        const dateString = `${value}`.trim();
        if (dateString === "") {
            return null;
        }

        const formats = [];
        if (typeof advFilterDtCulture === "string" && advFilterDtCulture.trim() !== "") {
            formats.push(advFilterDtCulture);
        }
        formats.push("DD-MMM-YYYY", "DD-MMM-YY", "DD/MM/YYYY", "D/M/YYYY", "MM/DD/YYYY", "M/D/YYYY", "YYYY-MM-DD", "YYYY/MM/DD", "DD-MM-YYYY", "D-M-YYYY");

        if (typeof moment === "function") {
            for (let i = 0; i < formats.length; i++) {
                const parsedDate = moment(dateString, formats[i], true);
                if (parsedDate.isValid()) {
                    return parsedDate.toDate();
                }
            }

            const nonStrictDate = moment(dateString);
            if (nonStrictDate.isValid()) {
                return nonStrictDate.toDate();
            }
        }

        const nativeDate = new Date(dateString);
        return isNaN(nativeDate.getTime()) ? null : nativeDate;
    }

    normalizeUrlParam(value) {
        if (value === null || value === undefined) {
            return "";
        }
        return `${value}`.trim().toLowerCase();
    }

    isFilterDebugEnabled() {
        if (typeof window !== "undefined" && window.ANALYTICS_FILTER_DEBUG === false) {
            return false;
        }
        return !!this.enableFilterDebugLogs;
    }

    logFilterDebug(label, payload) {
        if (!this.isFilterDebugEnabled()) {
            return;
        }

        try {
            if (payload === undefined) {
                console.log(`[AnalyticsFilter] ${label}`);
            } else {
                console.log(`[AnalyticsFilter] ${label}`, payload);
            }
        } catch (error) {
            // Ignore console serialization errors in old browsers.
        }
    }

    getUrlParams() {
        const queryString = window.location.search || "";
        const urlParams = new URLSearchParams(queryString);
        return {
            name: this.normalizeUrlParam(urlParams.get('name')),
            type: this.normalizeUrlParam(urlParams.get('type')),
            entity: this.normalizeUrlParam(urlParams.get('entity')),
            groupby: this.normalizeUrlParam(urlParams.get('groupby'))
        };
    }

    getUrlSource() {
        const legacyEntity = this.urlParams ? this.normalizeUrlParam(this.urlParams.entity) : "";
        const sourceName = this.urlParams ? this.normalizeUrlParam(this.urlParams.name || legacyEntity) : legacyEntity;
        const sourceType = this.urlParams ? this.normalizeUrlParam(this.urlParams.type || "tstruct") : "tstruct";
        return {
            name: sourceName,
            type: sourceType === "ads" ? "ads" : "tstruct"
        };
    }

    formatHeaderSourceName(sourceName) {
        const value = `${sourceName || ""}`.trim();
        if (!value) {
            return "";
        }

        return value
            .split(/\s+/)
            .filter(Boolean)
            .map(part => `${part.charAt(0).toUpperCase()}${part.slice(1)}`)
            .join(" ");
    }

    updatePageHeaderTitle(sourceName = "") {
        const titleElement = document.querySelector(".card-header.Page-title .card-label");
        if (!titleElement) {
            return;
        }

        const displayName = this.formatHeaderSourceName(
            sourceName || this.dataSourceName || this.entityTransId || ""
        );
        titleElement.textContent = displayName ? `Analytics - ${displayName}` : "Analytics";
    }

    isAdsMode() {
        const currentType = this.normalizeUrlParam(this.dataSourceType || "");
        if (currentType) {
            return currentType === "ads";
        }
        return this.getUrlSource().type === "ads";
    }

    applyUrlEntityContextUi() {
        const source = this.getUrlSource();
        const hasEntityInUrl = !!source.name;
        document.querySelectorAll(".scroll-container").forEach(element => {
            element.classList.toggle("d-none", hasEntityInUrl);
        });

        const addEntityButton = document.querySelector("#btn_Add_Entity");
        if (addEntityButton) {
            addEntityButton.classList.toggle("d-none", hasEntityInUrl);
        }
    }

    ensureLastRefreshLabel() {
        const toolbar = document.querySelector(".Tkts-toolbar-Right");
        if (!toolbar) {
            return null;
        }

        let refreshLabel = toolbar.querySelector("#analytics-last-refresh");
        if (!refreshLabel) {
            const html = `<span id="analytics-last-refresh" class="analytics-last-refresh" title="Last refresh">Last refresh: -</span>`;
            const refreshButton = toolbar.querySelector("#btn_AnalyticsRefresh");
            if (refreshButton) {
                refreshButton.insertAdjacentHTML("beforebegin", html);
            } else {
                toolbar.insertAdjacentHTML("afterbegin", html);
            }
            refreshLabel = toolbar.querySelector("#analytics-last-refresh");
        }

        return refreshLabel;
    }

    getGlobalDateFormatPattern(includeTime = false) {
        const basePattern = `${dtCulture || ""}`.toLowerCase() === "en-us" ? "MM/DD/YYYY" : "DD/MM/YYYY";
        return includeTime ? `${basePattern} HH:mm:ss` : basePattern;
    }

    formatDateByGlobalSetting(dateValue, includeTime = false) {
        const dateObj = dateValue instanceof Date ? dateValue : new Date(dateValue);
        if (!(dateObj instanceof Date) || isNaN(dateObj.getTime())) {
            return "";
        }

        if (typeof moment === "function") {
            return moment(dateObj).format(this.getGlobalDateFormatPattern(includeTime));
        }

        const day = `${dateObj.getDate()}`.padStart(2, "0");
        const month = `${dateObj.getMonth() + 1}`.padStart(2, "0");
        const year = dateObj.getFullYear();
        const datePart = `${dtCulture || ""}`.toLowerCase() === "en-us"
            ? `${month}/${day}/${year}`
            : `${day}/${month}/${year}`;

        if (!includeTime) {
            return datePart;
        }

        const hours = `${dateObj.getHours()}`.padStart(2, "0");
        const minutes = `${dateObj.getMinutes()}`.padStart(2, "0");
        const seconds = `${dateObj.getSeconds()}`.padStart(2, "0");
        return `${datePart} ${hours}:${minutes}:${seconds}`;
    }

    getGlobalNumberLocale() {
        return `${dtCulture || ""}`.toLowerCase() === "en-us" ? "en-US" : "en-IN";
    }

    resolveGlobalDecimalPlaces() {
        if (this.globalDecimalPlacesResolved) {
            return this.globalDecimalPlaces;
        }

        let resolvedDigits = 2;
        try {
            const globalObj = typeof AxGetGlobalVar === "function" ? AxGetGlobalVar() : null;
            const globalVars = globalObj && typeof globalObj === "object" ? (globalObj.globalVars || {}) : {};
            const digitCandidates = [];
            const validKeys = new Set(["decimalplaces", "decimals", "decimaldigits", "noofdecimals", "numdecimals", "amountdecimals", "decimallength", "numdigits"]);

            Object.keys(globalVars || {}).forEach(key => {
                const row = globalVars[key];
                if (!row || typeof row !== "object") {
                    return;
                }

                Object.keys(row).forEach(propKey => {
                    const normalizedKey = `${propKey || ""}`.toLowerCase().replace(/[^a-z]/g, "");
                    if (!validKeys.has(normalizedKey)) {
                        return;
                    }

                    const digits = parseInt(row[propKey], 10);
                    if (!isNaN(digits) && digits >= 0 && digits <= 10) {
                        digitCandidates.push(digits);
                    }
                });
            });

            if (digitCandidates.length > 0) {
                resolvedDigits = digitCandidates[0];
            }
        } catch (error) { }

        this.globalDecimalPlaces = resolvedDigits;
        this.globalDecimalPlacesResolved = true;
        return this.globalDecimalPlaces;
    }

    getDecimalPlacesFromValue(value) {
        const textValue = `${value ?? ""}`;
        if (!textValue) {
            return this.resolveGlobalDecimalPlaces();
        }

        const decimalMatch = textValue.replace(/,/g, "").match(/\.(\d+)/);
        if (!decimalMatch || !decimalMatch[1]) {
            return 0;
        }

        return Math.min(decimalMatch[1].length, this.resolveGlobalDecimalPlaces());
    }

    formatNumberByGlobalSetting(value, options = {}) {
        const numericValue = Number(`${value ?? ""}`.replace(/,/g, ""));
        if (isNaN(numericValue)) {
            return _entityCommon.inValid(value) ? "0" : `${value}`;
        }

        const decimalDigits = options.fixedDecimals === true
            ? this.resolveGlobalDecimalPlaces()
            : this.getDecimalPlacesFromValue(value);

        try {
            return numericValue.toLocaleString(this.getGlobalNumberLocale(), {
                minimumFractionDigits: decimalDigits,
                maximumFractionDigits: decimalDigits
            });
        } catch (error) {
            return numericValue.toLocaleString();
        }
    }

    getChartCompactUnit(value) {
        const numericValue = Number(`${value ?? ""}`.replace(/,/g, ""));
        if (!isFinite(numericValue)) {
            return { divisor: 1, suffix: "" };
        }

        const absValue = Math.abs(numericValue);
        const isUsLocale = this.getGlobalNumberLocale() === "en-US";

        if (isUsLocale) {
            if (absValue >= 1e9) return { divisor: 1e9, suffix: "B" };
            if (absValue >= 1e6) return { divisor: 1e6, suffix: "M" };
            if (absValue >= 1e3) return { divisor: 1e3, suffix: "K" };
            return { divisor: 1, suffix: "" };
        }

        if (absValue >= 1e7) return { divisor: 1e7, suffix: "Cr" };
        if (absValue >= 1e5) return { divisor: 1e5, suffix: "L" };
        if (absValue >= 1e3) return { divisor: 1e3, suffix: "K" };
        return { divisor: 1, suffix: "" };
    }

    formatChartCompactValue(value, decimals = 2) {
        const numericValue = Number(`${value ?? ""}`.replace(/,/g, ""));
        if (!isFinite(numericValue)) {
            return _entityCommon.inValid(value) ? "0" : `${value}`;
        }

        const compact = this.getChartCompactUnit(numericValue);
        const scaledValue = compact.divisor > 0 ? (numericValue / compact.divisor) : numericValue;
        const maxDigits = Math.max(0, Math.min(10, Number(decimals) || 2));

        try {
            const formatted = scaledValue.toLocaleString(this.getGlobalNumberLocale(), {
                minimumFractionDigits: 0,
                maximumFractionDigits: maxDigits
            });
            return `${formatted}${compact.suffix ? ` ${compact.suffix}` : ""}`;
        } catch (error) {
            return `${scaledValue}${compact.suffix ? ` ${compact.suffix}` : ""}`;
        }
    }

    formatChartValue(value, decimals = 2) {
        const numericValue = Number(`${value ?? ""}`.replace(/,/g, ""));
        if (!isFinite(numericValue)) {
            return _entityCommon.inValid(value) ? "0" : `${value}`;
        }

        const maxDigits = Math.max(0, Math.min(10, Number(decimals) || 2));
        try {
            return numericValue.toLocaleString(this.getGlobalNumberLocale(), {
                minimumFractionDigits: 0,
                maximumFractionDigits: maxDigits
            });
        } catch (error) {
            return `${numericValue}`;
        }
    }

    updateLastRefreshDateTime(dateValue) {
        const label = this.ensureLastRefreshLabel();
        if (!label) {
            return;
        }

        const dateObj = dateValue instanceof Date ? dateValue : new Date();
        const formatted = this.formatDateByGlobalSetting(dateObj, true);
        label.textContent = `Last refresh: ${formatted}`;
    }

    shouldDeferDefaultLoad() {
        const entityParam = this.urlParams ? this.urlParams.entity : "";
        if (!entityParam || !Array.isArray(this.selectedEntitiesList) || this.selectedEntitiesList.length === 0) {
            return false;
        }

        const entityExists = this.selectedEntitiesList.some(item => (item.name || "").toLowerCase() === entityParam);
        if (!entityExists) {
            return false;
        }

        const currentEntity = this.normalizeUrlParam(this.entityTransId);
        return currentEntity !== entityParam;
    }

    applyUrlParams() {
        const entityParam = this.urlParams ? this.urlParams.entity : "";
        if (!entityParam) {
            return;
        }
        this.applyEntityParam(entityParam);
    }

    applyEntityParam(entityParam) {
        if (!Array.isArray(this.selectedEntitiesList) || this.selectedEntitiesList.length === 0) {
            return;
        }

        const selected = this.selectedEntitiesList.find(item => (item.name || "").toLowerCase() === entityParam);
        if (!selected) {
            console.warn(`Analytics URL parameter entity not found: ${entityParam}`);
            return;
        }

        this.limitEntityTabs(selected.name);

        const currentEntity = this.normalizeUrlParam(this.entityTransId);
        if (currentEntity === entityParam) {
            if (this.enableCardGridLayout) {
                this.pendingGroupBy = "";
                this.renderAnalyticsGridCards();
            } else {
                const groupByApplied = this.applyPendingGroupBy();
                if (!groupByApplied && this.urlParams && this.urlParams.groupby) {
                    this.applyGroupByParam(this.urlParams.groupby);
                }
            }
            return;
        }

        const navItem = document.querySelector(`#dv_EntityContainer .nav-item[dt_transid="${selected.name}"]`);
        if (navItem) {
            selectEntity(navItem, selected.name);
        } else {
            this.entityTransId = selected.name;
            this.entityName = this.getEntityCaption(selected.name);
            fetchEntityData(selected.name);
        }
    }

    limitEntityTabs(transId) {
        const targetId = (transId || "").toLowerCase();
        document.querySelectorAll("#dv_EntityContainer .nav-item").forEach(item => {
            const itemId = (item.getAttribute("dt_transid") || "").toLowerCase();
            if (itemId && itemId !== targetId) {
                item.classList.add("d-none");
            } else {
                item.classList.remove("d-none");
            }
        });
    }

    canApplyPendingGroupBy() {
        if (!this.pendingGroupBy) {
            return false;
        }

        const entityParam = this.urlParams ? this.urlParams.entity : "";
        if (!entityParam) {
            return true;
        }

        if (!Array.isArray(this.selectedEntitiesList) || this.selectedEntitiesList.length === 0) {
            return true;
        }

        const entityExists = this.selectedEntitiesList.some(item => (item.name || "").toLowerCase() === entityParam);
        if (!entityExists) {
            return true;
        }

        const currentEntity = this.normalizeUrlParam(this.entityTransId);
        return currentEntity === entityParam;
    }

    applyPendingGroupBy() {
        if (this.enableCardGridLayout) {
            this.pendingGroupBy = "";
            return false;
        }

        if (!this.pendingGroupBy) {
            return false;
        }

        if (!this.canApplyPendingGroupBy()) {
            return false;
        }

        const applied = this.applyGroupByParam(this.pendingGroupBy);
        this.pendingGroupBy = "";
        return applied;
    }

    applyGroupByParam(groupByParam) {
        const groupBy = this.normalizeUrlParam(groupByParam);
        if (!groupBy) {
            return false;
        }

        const groupItems = document.querySelectorAll("#Data-Group-container .Data-Group_Items");
        let targetItem = null;

        groupItems.forEach(item => {
            if (targetItem) {
                return;
            }
            const fldName = (item.getAttribute("data-fldname") || "").toLowerCase();
            if (fldName === groupBy) {
                targetItem = item;
            }
        });

        if (!targetItem) {
            console.warn(`Analytics URL parameter groupby not found: ${groupBy}`);
            return false;
        }

        const anchor = targetItem.querySelector("a");
        if (anchor) {
            selectGroupField(anchor);
            return true;
        }

        return false;
    }

    escapeHtml(value) {
        return `${value || ""}`
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#39;");
    }

    getGridAggFields() {
        const options = this.getGridAggFieldOptionsRaw();
        const hasSelectedAggFields = this.xAxisFields && this.xAxisFields.trim() !== "" && this.xAxisFields.trim() !== "All";
        if (hasSelectedAggFields) {
            return options;
        }

        if (!this.availableAggFields || !(this.availableAggFields instanceof Set)) {
            return options;
        }

        return options.filter(option => {
            if (this.normalizeUrlParam(option.value) === "count") {
                return true;
            }
            return this.availableAggFields.has(this.normalizeUrlParam(option.value));
        });
    }

    getGridAggFieldOptionsRaw() {
        if (!Array.isArray(this.metaData)) {
            return [{ value: "count", label: "Count" }];
        }

        const options = [{ value: "count", label: "Count" }];
        const selectedFields = this.xAxisFields && this.xAxisFields.trim() !== "" && this.xAxisFields.trim() !== "All"
            ? this.xAxisFields.split(",").map(item => item.trim()).filter(item => item !== "")
            : [];

        let sourceFields = [];
        if (selectedFields.length > 0) {
            sourceFields = selectedFields.map(fieldName => {
                const normalizedField = this.normalizeUrlParam(fieldName);
                const compactField = this.getNormalizedFieldKey(fieldName);
                return this.metaData.find(item =>
                    this.normalizeUrlParam(item?.fldname || "") === normalizedField ||
                    this.getNormalizedFieldKey(item?.fldname || "") === compactField
                );
            }).filter(Boolean);
        } else {
            sourceFields = this.metaData.filter(item => (item.aggfield === "T" || item.fdatatype === "d") && item.hide === "F");
        }

        sourceFields.forEach(item => {
            const value = item.fldname || "";
            if (!value) {
                return;
            }
            const exists = options.some(option => this.normalizeUrlParam(option.value) === this.normalizeUrlParam(value));
            if (!exists) {
                options.push({
                    value: value,
                    label: item.fldcap || value
                });
            }
        });

        if (selectedFields.length > 0) {
            selectedFields.forEach(fieldName => {
                const exists = options.some(option => this.normalizeUrlParam(option.value) === this.normalizeUrlParam(fieldName));
                if (!exists) {
                    options.push({
                        value: fieldName,
                        label: fieldName
                    });
                }
            });
        }

        return options;
    }

    updateAvailableAggFieldsFromRows(rows) {
        if (!Array.isArray(rows)) {
            this.availableAggFields = null;
            return;
        }

        const options = this.getGridAggFieldOptionsRaw();
        const available = new Set(["count"]);

        if (rows.length > 0) {
            options.forEach(option => {
                const optionValue = option?.value || "";
                if (!optionValue || this.normalizeUrlParam(optionValue) === "count") {
                    return;
                }
                const hasMetricValue = rows.some(row =>
                    this.parseMetricValue(this.getRowFieldValue(row, optionValue)) !== null
                );
                if (hasMetricValue) {
                    available.add(this.normalizeUrlParam(optionValue));
                }
            });
        }

        this.availableAggFields = available;
        this.logFilterDebug("availableAggFields:update", {
            totalRows: rows.length,
            availableAggFields: Array.from(available)
        });

        this.applyAggFieldAvailabilityToGrid();
    }

    applyAggFieldAvailabilityToGrid() {
        if (!this.availableAggFields || !(this.availableAggFields instanceof Set)) {
            return;
        }

        this.isAggFieldAvailabilitySyncing = true;
        const currentValue = this.getSelectedGlobalAggField();
        this.ensureGlobalGridAggControl();
        this.setSelectedGlobalAggField(currentValue);
        this.isAggFieldAvailabilitySyncing = false;
        this.saveGridCardState();

        const normalizedBefore = this.normalizeUrlParam(currentValue || "count");
        const normalizedAfter = this.normalizeUrlParam(this.getSelectedGlobalAggField() || "count");
        if (normalizedBefore !== normalizedAfter) {
            const container = document.querySelector("#analytics-yaxis-grid");
            const cards = container ? Array.from(container.querySelectorAll(".analytics-grid-card")) : [];
            if (cards.length > 0) {
                this.loadGridCardsBatch(cards, { forceRefresh: false });
            }
        }
    }
    getGridGroupFields() {
        const options = [];
        if (!Array.isArray(this.metaData)) {
            return options;
        }

        const selectedFields = this.yAxisFields && this.yAxisFields.trim() !== "" && this.yAxisFields.trim() !== "All"
            ? this.yAxisFields.split(",").map(item => item.trim()).filter(item => item !== "")
            : [];

        let sourceFields = [];
        if (selectedFields.length > 0) {
            sourceFields = selectedFields.map(fieldName =>
                this.metaData.find(item => (item.fldname || "").toLowerCase() === this.normalizeUrlParam(fieldName))
            ).filter(Boolean);
        } else {
            sourceFields = this.metaData.filter(item =>
                item.grpfield === "T" &&
                item.hide === "F" &&
                (item.cdatatype === "DropDown" || item.fdatatype === "c" || item.fdatatype === "d")
            );
        }

        sourceFields.forEach(item => {
            const value = item.fldname || "";
            if (!value) {
                return;
            }
            const exists = options.some(option => this.normalizeUrlParam(option.value) === this.normalizeUrlParam(value));
            if (!exists) {
                options.push({
                    value: value,
                    label: item.fldcap || value
                });
            }
        });

        return options;
    }

    getGridCardHtml(groupOption, defaultChartType, index) {
        const cardId = `analytics-grid-card-${this.normalizeUrlParam(groupOption.value || "all").replace(/[^a-z0-9]+/g, "-")}-${index}`;
        const preferredGroup = this.normalizeUrlParam(this.urlParams?.groupby || "");
        const isPreferredGroup = preferredGroup && preferredGroup === this.normalizeUrlParam(groupOption.value || "");
        const chartTypes = this.getSupportedChartTypes();
        const defaultColors = this.getGridDefaultPaletteColors();

        const chartOptionsHtml = chartTypes.map(option => {
            const selectedAttr = this.normalizeUrlParam(option.type) === this.normalizeUrlParam(defaultChartType) ? " selected" : "";
            return `<option value="${this.escapeHtml(option.type)}"${selectedAttr}>${this.escapeHtml(option.name)}</option>`;
        }).join("");

        const paletteColorInputsHtml = defaultColors.map((color, index) =>
            `<input type="color" class="analytics-grid-color-input" data-color-index="${index}" value="${this.escapeHtml(color)}" title="Color ${index + 1}" aria-label="Color ${index + 1}" />`
        ).join("");

        return `<div class="analytics-grid-card${isPreferredGroup ? " selected" : ""}" data-default-selected="${isPreferredGroup ? "T" : "F"}" data-group-field="${this.escapeHtml(groupOption.value)}" data-group-caption="${this.escapeHtml(groupOption.label)}">
                    <div class="analytics-grid-card-header">
                        <h5 class="analytics-grid-card-title" title="${this.escapeHtml(groupOption.label)}">${this.escapeHtml(groupOption.label)}</h5>
                        <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm analytics-grid-resize-btn" title="Resize card" aria-label="Resize card">
                            <span class="material-icons material-icons-style material-icons-2">drag_indicator</span>
                        </button>
                        <div class="analytics-grid-menu-wrap">
                            <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm analytics-grid-popup-btn" title="Open in popup" aria-label="Open in popup">
                                <span class="material-icons material-icons-style material-icons-2">open_in_full</span>
                            </button>
                            <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm analytics-grid-menu-btn" title="Card options" aria-label="Card options">
                                <span class="material-icons material-icons-style material-icons-2">menu</span>
                            </button>
                            <div class="analytics-grid-menu-panel">
                                <div class="analytics-grid-menu-item">
                                    <label class="analytics-grid-menu-label">Chart type</label>
                                    <select class="form-select form-select-sm analytics-grid-chart-select" title="Chart type">
                                        ${chartOptionsHtml}
                                    </select>
                                </div>
                                <div class="analytics-grid-menu-item">
                                    <label class="analytics-grid-menu-label">Custom colors</label>
                                    <div class="analytics-grid-color-picker-wrap">
                                        <div class="analytics-grid-color-picker-list">
                                            ${paletteColorInputsHtml}
                                        </div>
                                        <button type="button" class="btn btn-link btn-sm analytics-grid-color-reset-btn">Reset</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="analytics-grid-card-body">
                        <div id="${cardId}" class="analytics-grid-chart">
                            <div class="analytics-grid-message">Loading...</div>
                        </div>
                    </div>
                </div>`;
    }

    closeGridCardMenus(exceptCard) {
        const cards = document.querySelectorAll("#analytics-yaxis-grid .analytics-grid-card.menu-open");
        cards.forEach(card => {
            if (exceptCard && card === exceptCard) {
                return;
            }
            card.classList.remove("menu-open");
            if (!card.classList.contains("selected")) {
                card.classList.remove("active");
            }
        });
    }

    setActiveGridCard(cardElement, options = {}) {
        if (!cardElement) {
            return;
        }
        const renderPanel = options.renderPanel !== false;

        document.querySelectorAll("#analytics-yaxis-grid .analytics-grid-card.active").forEach(card => {
            if (card === cardElement || card.classList.contains("menu-open")) {
                return;
            }
            card.classList.remove("active");
        });
        cardElement.classList.add("active");
        if (renderPanel) {
            this.renderHyperlinkPanelForCard(cardElement);
        }
    }

    handleGridCardClickSelection(cardElement) {
        if (!cardElement) {
            return;
        }

        const isAlreadyActive = cardElement.classList.contains("active");
        if (isAlreadyActive && !cardElement.classList.contains("menu-open")) {
            cardElement.classList.remove("active");
            if (this.isHyperlinkPanelVisible) {
                this.renderHyperlinkPanelForCard(null);
            }
            return;
        }

        this.setActiveGridCard(cardElement, { renderPanel: this.isHyperlinkPanelVisible });
    }

    getGridSelectionFiltersMap() {
        if (!this.gridSelectionFilter || typeof this.gridSelectionFilter !== "object") {
            this.gridSelectionFilter = { groupField: "", values: [], filters: {} };
        }
        if (!this.gridSelectionFilter.filters || typeof this.gridSelectionFilter.filters !== "object") {
            this.gridSelectionFilter.filters = {};
        }
        return this.gridSelectionFilter.filters;
    }

    getGridSelectionFilterEntries(excludeGroupField = "") {
        const normalizedExcludeGroup = this.normalizeUrlParam(excludeGroupField || "");
        return Object.entries(this.getGridSelectionFiltersMap()).filter(([groupField, values]) => {
            if (!groupField || !Array.isArray(values) || values.length === 0) {
                return false;
            }
            if (normalizedExcludeGroup && this.normalizeUrlParam(groupField) === normalizedExcludeGroup) {
                return false;
            }
            return true;
        });
    }

    refreshGridSelectionPrimaryState() {
        const firstEntry = this.getGridSelectionFilterEntries()[0];
        if (!firstEntry) {
            this.gridSelectionFilter.groupField = "";
            this.gridSelectionFilter.values = [];
            return;
        }
        this.gridSelectionFilter.groupField = firstEntry[0];
        this.gridSelectionFilter.values = firstEntry[1].slice();
    }

    hasGridSelectionFilter() {
        return this.getGridSelectionFilterEntries().length > 0;
    }

    isGridSelectionFilterActiveForGroup(groupField) {
        const normalizedGroup = this.normalizeUrlParam(groupField || "");
        if (!normalizedGroup) {
            return false;
        }

        const filtersMap = this.getGridSelectionFiltersMap();
        return Array.isArray(filtersMap[normalizedGroup]) && filtersMap[normalizedGroup].length > 0;
    }

    isGridSelectionValueSelected(groupField, value) {
        const normalizedGroup = this.normalizeUrlParam(groupField || "");
        const normalizedValue = this.normalizeUrlParam(value || "");
        if (!normalizedGroup || !normalizedValue) {
            return false;
        }

        const filtersMap = this.getGridSelectionFiltersMap();
        const selectedValues = Array.isArray(filtersMap[normalizedGroup]) ? filtersMap[normalizedGroup] : [];
        return selectedValues.some(item => this.normalizeUrlParam(item || "") === normalizedValue);
    }

    toggleGridSelectionValue(groupField, value) {
        const normalizedGroup = this.normalizeUrlParam(groupField || "");
        const normalizedValue = this.normalizeUrlParam(value || "");
        if (!normalizedGroup || !normalizedValue) {
            return false;
        }

        const filtersMap = this.getGridSelectionFiltersMap();
        const selectedValues = Array.isArray(filtersMap[normalizedGroup]) ? filtersMap[normalizedGroup].slice() : [];
        const existingIndex = selectedValues.findIndex(item => this.normalizeUrlParam(item || "") === normalizedValue);
        if (existingIndex > -1) {
            selectedValues.splice(existingIndex, 1);
        } else {
            selectedValues.push(value);
        }

        if (selectedValues.length > 0) {
            filtersMap[normalizedGroup] = selectedValues;
            this.gridSelectionFilter.groupField = normalizedGroup;
            this.gridSelectionFilter.values = selectedValues.slice();
        } else {
            delete filtersMap[normalizedGroup];
            this.refreshGridSelectionPrimaryState();
        }

        return true;
    }

    clearGridSelectionFilter(groupField = "", value = "") {
        const filtersMap = this.getGridSelectionFiltersMap();
        const normalizedGroup = this.normalizeUrlParam(groupField || "");
        const normalizedValue = this.normalizeUrlParam(value || "");

        if (!normalizedGroup) {
            this.gridSelectionFilter = {
                groupField: "",
                values: [],
                filters: {}
            };
        } else if (!normalizedValue) {
            delete filtersMap[normalizedGroup];
            this.refreshGridSelectionPrimaryState();
        } else {
            const selectedValues = Array.isArray(filtersMap[normalizedGroup]) ? filtersMap[normalizedGroup].slice() : [];
            const nextValues = selectedValues.filter(item => this.normalizeUrlParam(item || "") !== normalizedValue);
            if (nextValues.length > 0) {
                filtersMap[normalizedGroup] = nextValues;
                if (this.gridSelectionFilter.groupField === normalizedGroup) {
                    this.gridSelectionFilter.values = nextValues.slice();
                }
            } else {
                delete filtersMap[normalizedGroup];
                this.refreshGridSelectionPrimaryState();
            }
        }

        this.syncGridSelectedCardState();
        this.renderGridSelectionSummary();
    }

    getGridGroupCaption(groupField) {
        const normalizedGroup = this.normalizeUrlParam(groupField || "");
        if (!normalizedGroup) {
            return groupField || "";
        }

        const card = this.getGridCards().find(item => this.normalizeUrlParam(item.getAttribute("data-group-field") || "") === normalizedGroup);
        if (card) {
            const caption = card.getAttribute("data-group-caption") || "";
            if (caption) {
                return caption;
            }
        }

        if (Array.isArray(this.metaData)) {
            const metaField = this.metaData.find(item => this.normalizeUrlParam(item?.fldname || "") === normalizedGroup);
            if (metaField) {
                return metaField.fldcap || metaField.fldname || groupField;
            }
        }

        return groupField;
    }

    ensureGridSelectionSummaryContainer() {
        if (!this.enableGridPointFilterSelection) {
            return null;
        }
        let container = document.querySelector("#analytics-grid-filter-summary");
        if (container) {
            return container;
        }

        const pageHeader = document.querySelector(".card-header.Page-title");
        if (!pageHeader) {
            return null;
        }

        pageHeader.insertAdjacentHTML("afterend", `<div id="analytics-grid-filter-summary" class="analytics-grid-filter-summary d-none"></div>`);
        container = document.querySelector("#analytics-grid-filter-summary");
        return container;
    }

    renderGridSelectionSummary() {
        if (!this.enableGridPointFilterSelection) {
            return;
        }
        const container = this.ensureGridSelectionSummaryContainer();
        if (!container) {
            return;
        }

        const filterEntries = this.getGridSelectionFilterEntries();
        if (!filterEntries.length) {
            container.innerHTML = "";
            container.classList.add("d-none");
            return;
        }

        const chips = [];
        filterEntries.forEach(([groupField, values]) => {
            const caption = this.getGridGroupCaption(groupField);
            values.forEach(value => {
                chips.push(`<button type="button" class="analytics-grid-filter-chip analytics-grid-filter-chip-btn" data-group-field="${this.escapeHtml(groupField)}" data-filter-value="${this.escapeHtml(value)}">${this.escapeHtml(caption)}: ${this.escapeHtml(value)} <span class="analytics-grid-filter-chip-remove">&times;</span></button>`);
            });
        });

        container.innerHTML = `
            <div class="analytics-grid-filter-summary-inner">
                <span class="analytics-grid-filter-summary-label">Active filters:</span>
                <div class="analytics-grid-filter-summary-list">${chips.join("")}</div>
                <button type="button" class="btn btn-light btn-sm analytics-grid-clear-all-btn">Clear all</button>
            </div>
        `;
        container.classList.remove("d-none");

        if (!container.dataset.bound) {
            container.addEventListener("click", (event) => {
                const clearAllButton = event.target.closest(".analytics-grid-clear-all-btn");
                if (clearAllButton) {
                    event.preventDefault();
                    this.handleGridFilterSelection({ action: "clear" });
                    return;
                }

                const chipButton = event.target.closest(".analytics-grid-filter-chip-btn");
                if (!chipButton) {
                    return;
                }

                event.preventDefault();
                const groupField = chipButton.getAttribute("data-group-field") || "";
                const value = chipButton.getAttribute("data-filter-value") || "";
                this.handleGridFilterSelection({
                    action: "clear",
                    groupField: groupField,
                    value: value
                });
            });
            container.dataset.bound = "true";
        }
    }

    syncGridSelectedCardState() {
        const cards = this.getGridCards();
        if (!cards.length) {
            this.renderGridSelectionSummary();
            return;
        }

        if (this.hasGridSelectionFilter()) {
            cards.forEach(card => {
                const cardGroupField = this.normalizeUrlParam(card.getAttribute("data-group-field") || "");
                card.classList.toggle("selected", this.isGridSelectionFilterActiveForGroup(cardGroupField));
            });
            this.renderGridSelectionSummary();
            return;
        }

        let hasDefaultSelected = false;
        cards.forEach(card => {
            const isDefaultSelected = card.getAttribute("data-default-selected") === "T";
            card.classList.toggle("selected", isDefaultSelected);
            hasDefaultSelected = hasDefaultSelected || isDefaultSelected;
        });

        if (!hasDefaultSelected && cards[0]) {
            cards[0].classList.add("selected");
        }

        this.renderGridSelectionSummary();
    }

    updateGridCardFilterAction(cardElement, input) {
        if (!cardElement) {
            return;
        }

        const filterAction = cardElement.querySelector(".analytics-grid-filter-action");
        const groupField = input?.groupField || cardElement.getAttribute("data-group-field") || "";
        cardElement.classList.toggle("has-filter-selection", this.isGridSelectionFilterActiveForGroup(groupField));

        if (filterAction) {
            filterAction.classList.add("d-none");
        }
    }

    getGridCards() {
        const container = document.querySelector("#analytics-yaxis-grid");
        return container ? Array.from(container.querySelectorAll(".analytics-grid-card")) : [];
    }

    reloadGridCardsFromSelection(forceRefresh = false) {
        const cards = this.getGridCards();
        if (!cards.length) {
            return;
        }
        this.loadGridCardsBatch(cards, { forceRefresh: !!forceRefresh });
    }

    getGridCardChartInstance(cardElement) {
        if (!cardElement || typeof Highcharts === "undefined" || !Array.isArray(Highcharts.charts)) {
            return null;
        }

        return Highcharts.charts.find(chart => {
            if (!chart || !chart.renderTo) {
                return false;
            }
            return chart.renderTo.closest(".analytics-grid-card") === cardElement;
        }) || null;
    }

    clearGridCardSelectionHighlight(chart) {
        if (!chart) {
            return;
        }

        if (Array.isArray(chart.xAxis) && chart.xAxis[0]) {
            const axis = chart.xAxis[0];
            const categories = Array.isArray(axis.categories) ? axis.categories : [];
            const bandCount = Math.max(categories.length, 50);
            for (let index = 0; index < bandCount; index++) {
                axis.removePlotBand(`analytics-grid-sel-band-${index}`);
            }
        }

        if (Array.isArray(chart.series)) {
            chart.series.forEach(series => {
                if (!series || !Array.isArray(series.points)) {
                    return;
                }
                series.points.forEach(point => {
                    if (point && typeof point.select === "function") {
                        point.select(false, true);
                    }
                });
            });
        }
    }

    applyGridCardSelectionHighlight(cardElement) {
        if (!cardElement) {
            return;
        }

        const chart = this.getGridCardChartInstance(cardElement);
        if (!chart) {
            return;
        }

        this.clearGridCardSelectionHighlight(chart);
        const groupField = cardElement.getAttribute("data-group-field") || "";
        if (!this.isGridSelectionFilterActiveForGroup(groupField)) {
            return;
        }

        const selectedValues = new Set(((this.getGridSelectionFiltersMap()[this.normalizeUrlParam(groupField || "")] || []) || []).map(item => this.normalizeUrlParam(item || "")));
        if (selectedValues.size === 0) {
            return;
        }

        const selectedPointIndexes = new Set();

        if (Array.isArray(chart.series)) {
            chart.series.forEach(series => {
                if (!series || !Array.isArray(series.points)) {
                    return;
                }
                series.points.forEach(point => {
                    const linkPayload = this.parseGridPointLink(point?.options?.link || point?.link);
                    const pointValue = linkPayload?.type === "grid-filter-toggle"
                        ? (linkPayload?.value || "")
                        : (point?.name || point?.category || point?.options?.name || "");
                    const shouldSelect = selectedValues.has(this.normalizeUrlParam(pointValue || ""));
                    if (typeof point.select === "function") {
                        point.select(shouldSelect, true);
                    }
                    if (shouldSelect && typeof point?.x === "number") {
                        selectedPointIndexes.add(point.x);
                    }
                });
            });
        }

        this.logFilterDebug("applyGridCardSelectionHighlight", {
            cardGroupField: groupField,
            selectedValues: Array.from(selectedValues),
            selectedPointIndexes: Array.from(selectedPointIndexes)
        });

        if (Array.isArray(chart.xAxis) && chart.xAxis[0] && selectedPointIndexes.size > 0) {
            const axis = chart.xAxis[0];
            selectedPointIndexes.forEach(index => {
                axis.addPlotBand({
                    id: `analytics-grid-sel-band-${index}`,
                    from: index - 0.5,
                    to: index + 0.5,
                    color: "rgba(43, 95, 217, 0.22)"
                });
            });
        }

        if (typeof chart.redraw === "function") {
            chart.redraw();
        }
    }

    handleGridFilterSelection(payload) {
        if (!this.enableGridPointFilterSelection) {
            return;
        }
        if (!payload || typeof payload !== "object") {
            return;
        }

        this.logFilterDebug("handleGridFilterSelection:payload", payload);

        if (payload.action === "clear") {
            if (!this.hasGridSelectionFilter()) {
                return;
            }
            this.clearGridSelectionFilter(payload.groupField || "", payload.value || "");
            this.saveGridCardState();
            this.logFilterDebug("handleGridFilterSelection:clear", this.getGridSelectionFiltersMap());
            this.reloadGridCardsFromSelection(false);
            return;
        }

        const groupField = payload.groupField || "";
        const value = payload.value || payload.keyname || "";
        if (!_entityCommon.inValid(groupField) && !_entityCommon.inValid(value)) {
            const normalizedGroup = this.normalizeUrlParam(groupField || "");
            const targetCard = this.getGridCards().find(card => this.normalizeUrlParam(card.getAttribute("data-group-field") || "") === normalizedGroup);
            if (targetCard) {
                this.setActiveGridCard(targetCard);
            }

            const toggled = this.toggleGridSelectionValue(groupField, value);
            if (toggled) {
                this.syncGridSelectedCardState();
                this.saveGridCardState();
                this.logFilterDebug("handleGridFilterSelection:state", {
                    filters: this.getGridSelectionFiltersMap()
                });
                this.reloadGridCardsFromSelection(false);
            }
            return;
        }

        this.logFilterDebug("handleGridFilterSelection:ignoredPayload", {
            groupField: groupField,
            value: value
        });
    }

    applyGridSelectionFilterToRows(rows, options = {}) {
        const safeRows = Array.isArray(rows) ? rows : [];
        if (!this.enableGridPointFilterSelection) {
            return safeRows;
        }
        const filterEntries = this.getGridSelectionFilterEntries(options.excludeGroupField || "");
        if (!filterEntries.length) {
            return safeRows;
        }

        const filteredRows = safeRows.filter(row => {
            return filterEntries.every(([groupField, selectedValues]) => {
                const selectedSet = new Set((selectedValues || []).map(item => this.normalizeUrlParam(item || "")));
                if (!selectedSet.size) {
                    return true;
                }
                const rowValue = this.getRowFieldValue(row, groupField);
                return selectedSet.has(this.normalizeUrlParam(rowValue || ""));
            });
        });
        this.logFilterDebug("applyGridSelectionFilterToRows", {
            filters: filterEntries.map(item => ({ groupField: item[0], values: item[1] })),
            totalRows: safeRows.length,
            filteredRows: filteredRows.length
        });
        return filteredRows;
    }

    canBuildAnalyticsDataFromRows(rows, input) {
        const safeRows = Array.isArray(rows) ? rows : [];
        if (!safeRows.length) {
            return true;
        }

        const groupField = this.normalizeUrlParam(input?.groupField || "");
        if (groupField && groupField !== "all") {
            const hasGroupValue = safeRows.some(row => !_entityCommon.inValid(this.getRowFieldValue(row, input.groupField)));
            if (!hasGroupValue) {
                return false;
            }
        }

        const aggFunc = this.normalizeUrlParam(input?.aggFunc || "count") || "count";
        if (aggFunc !== "count") {
            const hasMetricValue = safeRows.some(row => this.parseMetricValue(this.getRowFieldValue(row, input?.aggField)) !== null);
            if (!hasMetricValue) {
                return false;
            }
        }

        return true;
    }

    getGridSelectionAwareChartData(chartsData, input, sourceRows = null) {
        const safeChartsData = Array.isArray(chartsData) ? chartsData : [];
        if (!this.enableGridPointFilterSelection) {
            return Promise.resolve(safeChartsData);
        }
        if (!this.hasGridSelectionFilter()) {
            return Promise.resolve(safeChartsData);
        }

        const groupField = this.normalizeUrlParam(input?.groupField || "");

        const buildFromRows = (rows) => {
            const filteredRows = this.applyGridSelectionFilterToRows(rows, { excludeGroupField: groupField });
            if (!filteredRows.length || !this.canBuildAnalyticsDataFromRows(filteredRows, input)) {
                return [];
            }

            const filteredChartsData = this.buildAnalyticsDataFromRows(filteredRows, input);
            return Array.isArray(filteredChartsData) ? filteredChartsData : [];
        };

        if (Array.isArray(sourceRows)) {
            const derivedCharts = buildFromRows(sourceRows);
            if (derivedCharts.length > 0 || this.getGridSelectionFilterEntries(groupField).length > 0) {
                return Promise.resolve(derivedCharts);
            }
            return Promise.resolve(safeChartsData);
        }

        return this.fetchEntityListRows(this.entityTransId).then(rows => {
            this.logFilterDebug("getGridSelectionAwareChartData:rowsLoaded", {
                totalRows: Array.isArray(rows) ? rows.length : 0,
                requestGroupField: groupField,
                filters: this.getGridSelectionFiltersMap()
            });
            const filteredData = buildFromRows(rows);
            if (Array.isArray(filteredData) && filteredData.length > 0) {
                this.logFilterDebug("getGridSelectionAwareChartData:filteredData", {
                    resultCount: filteredData.length,
                    sample: filteredData.slice(0, 5)
                });
                return filteredData;
            }

            return this.fetchEntityListRows(this.entityTransId, { forceRefresh: true }).then(retryRows => {
                this.logFilterDebug("getGridSelectionAwareChartData:rowsReloaded", {
                    totalRows: Array.isArray(retryRows) ? retryRows.length : 0
                });
                const retryData = buildFromRows(retryRows);
                if (Array.isArray(retryData) && retryData.length > 0) {
                    this.logFilterDebug("getGridSelectionAwareChartData:retryFilteredData", {
                        resultCount: retryData.length,
                        sample: retryData.slice(0, 5)
                    });
                    return retryData;
                }

                this.logFilterDebug("getGridSelectionAwareChartData:fallbackToChartsData", {
                    fallbackCount: safeChartsData.length
                });
                return this.getGridSelectionFilterEntries(groupField).length > 0 ? [] : safeChartsData;
            }).catch(() => safeChartsData);
        }).catch(error => {
            console.error("Error applying analytics grid selection filter:", error);
            return safeChartsData;
        });
    }

    toggleGridCardMenu(cardElement) {
        if (!cardElement) {
            return;
        }

        const isOpen = cardElement.classList.contains("menu-open");
        if (isOpen) {
            cardElement.classList.remove("menu-open");
            if (!cardElement.classList.contains("selected")) {
                cardElement.classList.remove("active");
            }
            return;
        }

        this.closeGridCardMenus(cardElement);
        this.setActiveGridCard(cardElement);
        cardElement.classList.add("menu-open");
    }

    initializeGridColumnControls() {
        const plusButton = document.querySelector("#btn_AnalyticsGridPlus");
        const minusButton = document.querySelector("#btn_AnalyticsGridMinus");

        if (plusButton) {
            plusButton.classList.add("d-none");
        }

        if (minusButton) {
            minusButton.classList.add("d-none");
        }

        if (plusButton && !plusButton.dataset.bound) {
            plusButton.addEventListener("click", (event) => {
                event.preventDefault();
                this.changeGridCardsPerRow(1);
            });
            plusButton.dataset.bound = "true";
        }

        if (minusButton && !minusButton.dataset.bound) {
            minusButton.addEventListener("click", (event) => {
                event.preventDefault();
                this.changeGridCardsPerRow(-1);
            });
            minusButton.dataset.bound = "true";
        }

        if (!this.gridResizeHandlerBound) {
            window.addEventListener("resize", () => this.applyGridColumnsLayout());
            this.gridResizeHandlerBound = true;
        }

        this.applyGridColumnsLayout();
    }

    changeGridCardsPerRow(step) {
        const nextValue = this.gridCardsPerRow + step;
        this.gridCardsPerRow = Math.min(this.maxGridCardsPerRow, Math.max(this.minGridCardsPerRow, nextValue));
        this.applyGridColumnsLayout();
        this.reflowGridCharts();
    }

    applyGridColumnsLayout() {
        const grid = document.querySelector("#analytics-yaxis-grid");
        const plusButton = document.querySelector("#btn_AnalyticsGridPlus");
        const minusButton = document.querySelector("#btn_AnalyticsGridMinus");
        const maxColumnsByPanel = this.isHyperlinkPanelVisible ? 3 : this.maxGridCardsPerRow;
        const targetColumns = Math.min(this.gridCardsPerRow, maxColumnsByPanel);
        const effectiveColumns = window.innerWidth <= 991 ? 1 : targetColumns;
        const totalTracks = this.gridTotalTracks || 12;
        const defaultSpan = Math.max(1, Math.floor(totalTracks / Math.max(1, effectiveColumns)));

        if (grid) {
            grid.style.gridTemplateColumns = `repeat(${totalTracks}, minmax(0, 1fr))`;
            grid.setAttribute("data-cards-per-row", `${targetColumns}`);
            grid.setAttribute("data-effective-columns", `${effectiveColumns}`);

            grid.querySelectorAll(".analytics-grid-card").forEach(card => {
                let span = Number(card.getAttribute("data-card-span"));
                const isCustom = card.getAttribute("data-custom-span") === "true";
                if (!isCustom || !span) {
                    span = defaultSpan;
                    card.setAttribute("data-card-span", `${span}`);
                }
                this.applyGridCardSpan(card, span);
            });
        }

        if (plusButton) {
            const isDisabled = this.gridCardsPerRow >= maxColumnsByPanel;
            plusButton.disabled = isDisabled;
            plusButton.classList.toggle("disabled", isDisabled);
        }

        if (minusButton) {
            const isDisabled = this.gridCardsPerRow <= this.minGridCardsPerRow;
            minusButton.disabled = isDisabled;
            minusButton.classList.toggle("disabled", isDisabled);
        }
    }

    initializeHyperlinkPanelToggle() {
        const toggleButton = document.querySelector("#btn_ToggleHyperlinksPanel");
        const gridContainer = document.querySelector("#Analytics_Grid_Container");
        const panel = document.querySelector("#Analytics_Hyperlink_Panel");

        if (!toggleButton || !panel || !gridContainer) {
            return;
        }

        if (!this.enableCardGridLayout) {
            toggleButton.classList.add("d-none");
            gridContainer.classList.remove("col-xl-8", "col-lg-8", "col-md-12", "col-12");
            gridContainer.classList.add("col-12");
            panel.classList.add("d-none");
            return;
        }

        panel.classList.add("d-none");
        this.isHyperlinkPanelVisible = false;
        toggleButton.classList.remove("btn-primary");
        if (!toggleButton.classList.contains("btn-white")) {
            toggleButton.classList.add("btn-white");
        }
        this.applyHyperlinkPanelLayout();
    }

    applyHyperlinkPanelLayout() {
        const gridContainer = document.querySelector("#Analytics_Grid_Container");
        const panel = document.querySelector("#Analytics_Hyperlink_Panel");
        if (!gridContainer || !panel) {
            return;
        }

        gridContainer.classList.remove("col-xl-8", "col-lg-8", "col-md-12", "col-12");
        panel.classList.remove("col-xl-4", "col-lg-4", "col-md-12", "col-12");

        if (this.isHyperlinkPanelVisible) {
            gridContainer.classList.add("col-xl-8", "col-lg-8", "col-md-12", "col-12");
            panel.classList.add("col-xl-4", "col-lg-4", "col-md-12", "col-12");
        } else {
            gridContainer.classList.add("col-12");
            panel.classList.add("col-12");
        }

        this.applyGridColumnsLayout();
        setTimeout(() => this.reflowGridCharts(), 120);
    }

    applyGridCardSpan(cardElement, spanValue) {
        if (!cardElement) {
            return;
        }

        const totalTracks = this.gridTotalTracks || 12;
        const safeSpan = Math.max(1, Math.min(totalTracks, Number(spanValue) || 1));
        cardElement.style.gridColumn = `span ${safeSpan}`;
        cardElement.setAttribute("data-card-span", `${safeSpan}`);
    }

    bindGridCardResize(cardElement, handleElement) {
        if (!cardElement || !handleElement || handleElement.dataset.bound === "true") {
            return;
        }

        handleElement.addEventListener("pointerdown", (event) => {
            event.preventDefault();
            event.stopPropagation();

            const grid = document.querySelector("#analytics-yaxis-grid");
            if (!grid) {
                return;
            }

            const gridRect = grid.getBoundingClientRect();
            const cardRect = cardElement.getBoundingClientRect();
            const totalTracks = this.gridTotalTracks || 12;
            const gapValue = window.getComputedStyle(grid).columnGap || window.getComputedStyle(grid).gap || "14";
            const gap = parseFloat(gapValue) || 14;
            const usableWidth = Math.max(0, gridRect.width - (gap * (totalTracks - 1)));
            const trackWidth = usableWidth > 0 ? (usableWidth / totalTracks) : 0;
            if (trackWidth <= 0) {
                return;
            }

            const minSpan = Math.max(2, Math.floor(totalTracks / this.maxGridCardsPerRow));
            const maxSpan = totalTracks;
            const startX = event.clientX;
            const startWidth = cardRect.width;

            cardElement.classList.add("resizing");

            const onPointerMove = (moveEvent) => {
                const deltaX = moveEvent.clientX - startX;
                const nextWidth = Math.max(startWidth + deltaX, (trackWidth * minSpan) + (gap * (minSpan - 1)));
                const rawSpan = (nextWidth + gap) / (trackWidth + gap);
                const nextSpan = Math.max(minSpan, Math.min(maxSpan, Math.round(rawSpan)));

                cardElement.setAttribute("data-custom-span", "true");
                this.applyGridCardSpan(cardElement, nextSpan);

                if (this.activeResizeRafId) {
                    cancelAnimationFrame(this.activeResizeRafId);
                }
                this.activeResizeRafId = requestAnimationFrame(() => {
                    this.reflowGridCardChart(cardElement);
                    this.activeResizeRafId = 0;
                });
            };

            const stopResize = () => {
                window.removeEventListener("pointermove", onPointerMove);
                window.removeEventListener("pointerup", stopResize);
                window.removeEventListener("pointercancel", stopResize);
                cardElement.classList.remove("resizing");
                if (this.activeResizeRafId) {
                    cancelAnimationFrame(this.activeResizeRafId);
                    this.activeResizeRafId = 0;
                }
                this.reflowGridCardChart(cardElement);
                this.reflowGridCharts();
            };

            window.addEventListener("pointermove", onPointerMove);
            window.addEventListener("pointerup", stopResize);
            window.addEventListener("pointercancel", stopResize);
        });

        handleElement.dataset.bound = "true";
    }

    toggleHyperlinkPanel() {
        if (!this.enableCardGridLayout) {
            return;
        }

        const toggleButton = document.querySelector("#btn_ToggleHyperlinksPanel");
        const panel = document.querySelector("#Analytics_Hyperlink_Panel");
        if (!toggleButton || !panel) {
            return;
        }

        this.isHyperlinkPanelVisible = !this.isHyperlinkPanelVisible;
        panel.classList.toggle("d-none", !this.isHyperlinkPanelVisible);
        toggleButton.classList.toggle("btn-primary", this.isHyperlinkPanelVisible);
        toggleButton.classList.toggle("btn-white", !this.isHyperlinkPanelVisible);
        this.applyHyperlinkPanelLayout();

        if (!this.isHyperlinkPanelVisible) {
            return;
        }

        const activeCard = document.querySelector("#analytics-yaxis-grid .analytics-grid-card.active")
            || document.querySelector("#analytics-yaxis-grid .analytics-grid-card.selected")
            || document.querySelector("#analytics-yaxis-grid .analytics-grid-card");
        this.renderHyperlinkPanelForCard(activeCard);
    }

    formatHyperlinkPanelValue(value) {
        const numericValue = Number(value);
        if (!isNaN(numericValue)) {
            return this.formatNumberByGlobalSetting(value);
        }
        if (value === null || value === undefined) {
            return "0";
        }
        return `${value}`;
    }

    getHyperlinkPanelDisplayLabel(label) {
        const normalized = this.normalizeUrlParam(label || "");
        if (normalized === "total records") {
            return "Total";
        }
        return label || "";
    }

    getVisibleHyperlinkPoints(payload) {
        if (!payload || !Array.isArray(payload.points)) {
            return [];
        }

        const periodLabels = new Set(["this year", "this month", "this week", "yesterday", "today"]);
        return payload.points.filter(point => {
            const label = this.normalizeUrlParam(point?.label || "");
            if (label === "total records") {
                return true;
            }
            if (periodLabels.has(label)) {
                return this.normalizeNumericValue(point?.value) !== 0;
            }
            return true;
        });
    }

    buildGridHyperlinkHtml(points) {
        return points.map(item => {
            const navigationLink = item.navigateLink || item.link || "";
            const encodedLink = encodeURIComponent(navigationLink);
            const hasLink = !!navigationLink;
            return `<div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
                        <a href="#" data-link="${encodedLink}" class="Invoice-item analytics-hyperlink-item${hasLink ? "" : " no-link"}">
                            <div class="Invoice-icon">
                                <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                            </div>
                            <div class="Invoice-content">
                                <h6 class="subtitle">${this.escapeHtml(this.getHyperlinkPanelDisplayLabel(item.label || ""))}</h6>
                                <h3 class="title">${this.escapeHtml(this.formatHyperlinkPanelValue(item.value))}</h3>
                            </div>
                            <div class="Invoice-icon2"></div>
                        </a>
                    </div>`;
        }).join("");
    }

    bindGridHyperlinkNavigation(listContainer) {
        if (!listContainer) {
            return;
        }

        listContainer.querySelectorAll(".analytics-hyperlink-item").forEach(anchor => {
            anchor.addEventListener("click", (event) => {
                event.preventDefault();
                const linkData = decodeURIComponent(anchor.getAttribute("data-link") || "");
                if (anchor.classList.contains("no-link")) {
                    return;
                }
                if (linkData && typeof handleAnalyticsGridPointNavigation === "function") {
                    handleAnalyticsGridPointNavigation(linkData);
                }
            });
        });
    }

    renderGridHyperlinkList(listContainer, cardElement) {
        if (!listContainer) {
            return;
        }

        if (!cardElement) {
            listContainer.innerHTML = `<div class="analytics-grid-message">No card selected</div>`;
            return;
        }

        const payload = cardElement.__gridChartPayload;
        if (!payload || !Array.isArray(payload.points)) {
            listContainer.innerHTML = `<div class="analytics-grid-message">Loading...</div>`;
            return;
        }

        const visiblePoints = this.getVisibleHyperlinkPoints(payload);
        if (visiblePoints.length === 0) {
            listContainer.innerHTML = `<div class="analytics-grid-message">No data found</div>`;
            return;
        }

        listContainer.innerHTML = this.buildGridHyperlinkHtml(visiblePoints);
        this.bindGridHyperlinkNavigation(listContainer);
    }

    renderHyperlinkPanelForCard(cardElement) {
        if (!this.isHyperlinkPanelVisible) {
            return;
        }

        const listContainer = document.querySelector("#Analytics_Hyperlink_List");
        const titleElement = document.querySelector("#Analytics_Hyperlink_Title");
        if (!listContainer) {
            return;
        }

        const caption = cardElement ? (cardElement.getAttribute("data-group-caption") || "Details") : "Details";
        if (titleElement) {
            titleElement.textContent = caption;
        }

        if (!cardElement) {
            listContainer.innerHTML = `<div class="analytics-grid-message">No card selected</div>`;
            return;
        }

        this.renderGridHyperlinkList(listContainer, cardElement);
    }

    reflowGridCharts() {
        if (typeof Highcharts === "undefined" || !Array.isArray(Highcharts.charts)) {
            return;
        }

        setTimeout(() => {
            Highcharts.charts.forEach(chart => {
                if (!chart || !chart.renderTo) {
                    return;
                }
                if (chart.renderTo.closest("#analytics-yaxis-grid") || chart.renderTo.closest(".analytics-grid-popup-chart")) {
                    try {
                        const parentWidth = chart.renderTo.parentElement
                            ? (chart.renderTo.parentElement.clientWidth || chart.renderTo.parentElement.getBoundingClientRect().width || 0)
                            : 0;
                        const ownWidth = chart.renderTo.clientWidth || chart.renderTo.getBoundingClientRect().width || 0;
                        const targetWidth = Math.round(Math.max(parentWidth, ownWidth));
                        if (targetWidth > 0 && typeof chart.setSize === "function") {
                            chart.setSize(targetWidth, null, false);
                        } else {
                            chart.reflow();
                        }
                    } catch (error) {
                        console.error("Error while reflowing chart:", error);
                    }
                }
            });
        }, 80);
    }

    reflowGridCardChart(cardElement) {
        if (!cardElement || typeof Highcharts === "undefined" || !Array.isArray(Highcharts.charts)) {
            return;
        }

        Highcharts.charts.forEach(chart => {
            if (!chart || !chart.renderTo) {
                return;
            }

            if (chart.renderTo.closest(".analytics-grid-card") === cardElement) {
                try {
                    const parentWidth = chart.renderTo.parentElement
                        ? (chart.renderTo.parentElement.clientWidth || chart.renderTo.parentElement.getBoundingClientRect().width || 0)
                        : 0;
                    const ownWidth = chart.renderTo.clientWidth || chart.renderTo.getBoundingClientRect().width || 0;
                    const targetWidth = Math.round(Math.max(parentWidth, ownWidth));
                    if (targetWidth > 0 && typeof chart.setSize === "function") {
                        chart.setSize(targetWidth, null, false);
                    } else {
                        chart.reflow();
                    }
                } catch (error) {
                    console.error("Error while reflowing card chart:", error);
                }
            }
        });
    }

    renderAnalyticsGridCards(forceRefresh = false) {
        if (!this.enableCardGridLayout) {
            return;
        }

        const container = document.querySelector("#analytics-yaxis-grid");
        if (!container) {
            return;
        }

        const groupOptions = this.getGridGroupFields();
        if (!groupOptions.length) {
            container.innerHTML = `<div class="analytics-grid-message w-100">No group fields configured.</div>`;
            return;
        }

        container.innerHTML = groupOptions.map((groupOption, index) =>
            this.getGridCardHtml(groupOption, this.getAutoDefaultGridChartType(index), index)
        ).join("");
        this.applyStoredGridCardState(container);
        this.ensureGlobalGridAggControl();
        this.applyGridColumnsLayout();

        const cards = container.querySelectorAll(".analytics-grid-card");
        cards.forEach(card => {
            const popupButton = card.querySelector(".analytics-grid-popup-btn");
            const menuButton = card.querySelector(".analytics-grid-menu-btn");
            const menuPanel = card.querySelector(".analytics-grid-menu-panel");
            const chartSelect = card.querySelector(".analytics-grid-chart-select");
            const colorInputs = Array.from(card.querySelectorAll(".analytics-grid-color-input"));
            const paletteResetButton = card.querySelector(".analytics-grid-color-reset-btn");
            const resizeButton = card.querySelector(".analytics-grid-resize-btn");
            const clearFilterButton = card.querySelector(".analytics-grid-clear-filter-btn");

            if (popupButton) {
                popupButton.addEventListener("click", (event) => {
                    event.preventDefault();
                    event.stopPropagation();
                    this.closeGridCardMenus();
                    this.setActiveGridCard(card);
                    this.openGridCardPopup(card);
                });
            }

            if (menuButton) {
                menuButton.addEventListener("click", (event) => {
                    event.preventDefault();
                    event.stopPropagation();
                    this.toggleGridCardMenu(card);
                });
            }

            if (menuPanel) {
                menuPanel.addEventListener("click", (event) => event.stopPropagation());
            }

            if (resizeButton) {
                this.bindGridCardResize(card, resizeButton);
            }

            card.addEventListener("click", (event) => {
                if (event.target.closest(".analytics-grid-menu-wrap")
                    || event.target.closest(".analytics-grid-filter-action")
                    || event.target.closest(".analytics-grid-popup-btn")
                    || event.target.closest(".analytics-grid-resize-btn")) {
                    return;
                }
                this.handleGridCardClickSelection(card);
            });

            if (chartSelect) {
                chartSelect.addEventListener("change", () => {
                    this.saveGridCardState(container);
                    this.loadGridCard(card);
                });
            }
            if (colorInputs.length) {
                colorInputs.forEach(input => {
                    input.addEventListener("input", () => {
                        this.saveGridCardState(container);
                        this.applyGridCardPalette(card);
                    });
                });
            }

            if (paletteResetButton) {
                paletteResetButton.addEventListener("click", (event) => {
                    event.preventDefault();
                    event.stopPropagation();
                    this.applyGridCardColorInputs(card, this.getGridDefaultPaletteColors());
                    this.saveGridCardState(container);
                    this.applyGridCardPalette(card);
                });
            }

            if (clearFilterButton) {
                clearFilterButton.addEventListener("click", (event) => {
                    event.preventDefault();
                    event.stopPropagation();
                    this.handleGridFilterSelection({ action: "clear" });
                });
            }
        });

        const defaultActiveCard = container.querySelector(".analytics-grid-card.selected")
            || container.querySelector(".analytics-grid-card");
        if (defaultActiveCard) {
            this.setActiveGridCard(defaultActiveCard);
        }

        if (!this.gridMenuOutsideClickBound) {
            document.addEventListener("click", (event) => {
                if (!event.target.closest(".analytics-grid-menu-wrap")) {
                    this.closeGridCardMenus();
                }
            });
            this.gridMenuOutsideClickBound = true;
        }

        this.ensureGridChartLibraries(() => {
            this.loadGridCardsBatch(Array.from(cards), { forceRefresh: !!forceRefresh });
        });
    }

    ensureGridChartLibraries(callback) {
        if (typeof callback !== "function") {
            return;
        }

        if (typeof createAgileChart === "function") {
            this.isGridChartLibLoaded = true;
            callback();
            return;
        }

        this.gridChartLibCallbacks.push(callback);
        if (this.gridChartLibLoading) {
            return;
        }

        this.gridChartLibLoading = true;

        if (typeof loadAndCall !== "function") {
            this.gridChartLibLoading = false;
            const pendingCallbacks = this.gridChartLibCallbacks.splice(0);
            pendingCallbacks.forEach(cb => cb());
            return;
        }

        const assetBaseUrl = this.getAssetBaseUrl();
        const files = {
            css: [],
            js: [
                this.toAssetUrl("/ThirdParty/Highcharts/highcharts.js", assetBaseUrl),
                this.toAssetUrl("/ThirdParty/Highcharts/highcharts-3d.js", assetBaseUrl),
                this.toAssetUrl("/ThirdParty/Highcharts/highcharts-more.js", assetBaseUrl),
                this.toAssetUrl("/ThirdParty/Highcharts/highcharts-exporting.js", assetBaseUrl),
                this.toAssetUrl("/AxpertPlugins/Axi/HTMLPages/js/analytics-charts-functions.js", assetBaseUrl)
            ]
        };

        loadAndCall({
            files: files,
            callBack: () => {
                this.gridChartLibLoading = false;
                this.isGridChartLibLoaded = typeof createAgileChart === "function";
                const pendingCallbacks = this.gridChartLibCallbacks.splice(0);
                pendingCallbacks.forEach(cb => {
                    try {
                        cb();
                    } catch (error) {
                        console.error("Error rendering analytics grid card:", error);
                    }
                });
            }
        });
    }

    getAssetBaseUrl() {
        try {
            const topHref = `${window?.top?.location?.href || ""}`;
            const topHrefLower = topHref.toLowerCase();
            const topAspxIndex = topHrefLower.indexOf("/aspx/");
            if (topAspxIndex > -1) {
                return topHref.substring(0, topAspxIndex).replace(/\/+$/, "");
            }
        } catch (error) { }

        const currentHref = `${window.location.href || ""}`;
        const currentHrefLower = currentHref.toLowerCase();
        const markers = ["/htmlpages/"];
        let markerIndex = -1;

        markers.forEach(marker => {
            const currentIndex = currentHrefLower.indexOf(marker);
            if (currentIndex > -1 && (markerIndex === -1 || currentIndex < markerIndex)) {
                markerIndex = currentIndex;
            }
        });

        if (markerIndex > -1) {
            const currentPathName = `${window.location.pathname || ""}`;
            const pathParts = currentPathName.split("/").filter(Boolean);
            if (pathParts.length > 0) {
                return `${window.location.origin}/${pathParts[0]}`.replace(/\/+$/, "");
            }
            return `${window.location.origin || ""}`.replace(/\/+$/, "");
        }

        const aspxIndex = currentHrefLower.indexOf("/aspx/");
        if (aspxIndex > -1) {
            return currentHref.substring(0, aspxIndex).replace(/\/+$/, "");
        }

        return `${window.location.origin || ""}`.replace(/\/+$/, "");
    }

    toAssetUrl(path, cachedBaseUrl = "") {
        const rawPath = `${path || ""}`.trim();
        if (!rawPath) {
            return rawPath;
        }

        if (rawPath.indexOf("://") > -1 || rawPath.startsWith("data:")) {
            return rawPath;
        }

        const baseUrl = `${cachedBaseUrl || this.getAssetBaseUrl() || ""}`.replace(/\/+$/, "");
        const normalizedPath = rawPath.startsWith("/") ? rawPath : `/${rawPath}`;
        return baseUrl ? `${baseUrl}${normalizedPath}` : normalizedPath;
    }

    buildAnalyticsInput(groupFieldName, aggFieldName) {
        const normalizedGroup = this.normalizeUrlParam(groupFieldName) || "all";
        const normalizedAgg = this.normalizeUrlParam(aggFieldName) || "count";

        const groupFieldMeta = this.metaData.find(item => this.normalizeUrlParam(item.fldname) === normalizedGroup);
        const aggFieldMeta = this.metaData.find(item => this.normalizeUrlParam(item.fldname) === normalizedAgg);

        let transId = this.entityTransId;
        let aggField = "count";
        let aggFunc = "count";

        if (normalizedGroup !== "all" && groupFieldMeta && groupFieldMeta.ftransid) {
            transId = groupFieldMeta.ftransid;
        }

        if (normalizedAgg !== "count" && aggFieldMeta) {
            aggField = aggFieldMeta.fldname;
            aggFunc = "sum";
            if (aggFieldMeta.ftransid) {
                transId = aggFieldMeta.ftransid;
            }
        }

        const input = {
            page: "Analytics",
            transId: transId,
            aggField: aggField,
            aggTransId: transId,
            groupField: normalizedGroup === "all" ? "all" : (groupFieldMeta ? groupFieldMeta.fldname : normalizedGroup),
            groupTransId: transId,
            aggFunc: aggFunc
        };

        return input;
    }

    getCurrentUserName() {
        let userName = "";
        try {
            userName = window?.top?.mainUserName || "";
        } catch (error) { }

        if (!userName && typeof callParentNew === "function") {
            try {
                userName = callParentNew("mainUserName") || "";
            } catch (error) { }
        }

        return `${userName || "anonymous"}`.trim();
    }

    getCurrentProjectName() {
        let projectName = "";
        try {
            projectName = window?.top?.mainProject || "";
        } catch (error) { }

        if (!projectName && typeof callParentNew === "function") {
            try {
                projectName = callParentNew("mainProject") || "";
            } catch (error) { }
        }

        return `${projectName || "project"}`.trim();
    }

    getAnalyticsEntityStorageKey(transId, useLegacy = false) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const entityId = this.normalizeUrlParam(transId || this.entityTransId || "");
        const namespace = useLegacy ? "analytics:entityWS:v1" : "analyticsinfo:entityWS:v1";
        return `${namespace}:${projectName}:${userName}:${entityId}`;
    }

    getAnalyticsEntityStorageKeyProjInfo(transId) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const entityId = this.normalizeUrlParam(transId || this.entityTransId || "");
        return `projInfo-analytics:entityWS:v1:${projectName}:${userName}:${entityId}`;
    }

    readAnalyticsEntityCache(transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId)) {
            return null;
        }

        if (this.analyticsEntityCache[entityId]) {
            return this.analyticsEntityCache[entityId];
        }

        try {
            if (typeof localStorage === "undefined") {
                return null;
            }

            const primaryKey = this.getAnalyticsEntityStorageKey(entityId);
            const projInfoKey = this.getAnalyticsEntityStorageKeyProjInfo(entityId);
            const legacyKey = this.getAnalyticsEntityStorageKey(entityId, true);
            let rawValue = localStorage.getItem(primaryKey);
            if (!rawValue) {
                rawValue = localStorage.getItem(projInfoKey) || localStorage.getItem(legacyKey);
                if (rawValue) {
                    localStorage.setItem(primaryKey, rawValue);
                }
            }
            if (!rawValue) {
                return null;
            }

            const parsedValue = JSON.parse(rawValue);
            if (!parsedValue || !parsedValue.data) {
                return null;
            }

            this.analyticsEntityCache[entityId] = parsedValue.data;
            return parsedValue.data;
        } catch (error) {
            return null;
        }
    }

    writeAnalyticsEntityCache(transId, pageLoadData) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId) || !pageLoadData) {
            return;
        }

        this.analyticsEntityCache[entityId] = pageLoadData;
        try {
            if (typeof localStorage === "undefined") {
                return;
            }

            localStorage.setItem(this.getAnalyticsEntityStorageKey(entityId), JSON.stringify({
                data: pageLoadData,
                storedOn: new Date().toISOString()
            }));
        } catch (error) {
            console.warn("Unable to write analytics entity cache:", error);
        }
    }

    clearAnalyticsEntityCache(transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId)) {
            return;
        }

        delete this.analyticsEntityCache[entityId];
        try {
            if (typeof localStorage !== "undefined") {
                localStorage.removeItem(this.getAnalyticsEntityStorageKey(entityId));
                localStorage.removeItem(this.getAnalyticsEntityStorageKeyProjInfo(entityId));
                localStorage.removeItem(this.getAnalyticsEntityStorageKey(entityId, true));
            }
        } catch (error) {
            console.warn("Unable to clear analytics entity cache:", error);
        }
    }

    getAnalyticsPrefsStorageKey(transId, useLegacy = false) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const entityId = this.normalizeUrlParam(transId || this.entityTransId || "");
        const namespace = useLegacy ? "analytics:prefs:v1" : "analyticsinfo:prefs:v1";
        return `${namespace}:${projectName}:${userName}:${entityId}`;
    }

    getAnalyticsPrefsStorageKeyProjInfo(transId) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const entityId = this.normalizeUrlParam(transId || this.entityTransId || "");
        return `projInfo-analytics:prefs:v1:${projectName}:${userName}:${entityId}`;
    }

    readAnalyticsPrefsCache(transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId)) {
            return null;
        }

        if (this.analyticsPrefsCache[entityId]) {
            return this.analyticsPrefsCache[entityId];
        }

        try {
            if (typeof localStorage === "undefined") {
                return null;
            }

            const primaryKey = this.getAnalyticsPrefsStorageKey(entityId);
            const projInfoKey = this.getAnalyticsPrefsStorageKeyProjInfo(entityId);
            const legacyKey = this.getAnalyticsPrefsStorageKey(entityId, true);
            let rawValue = localStorage.getItem(primaryKey);
            if (!rawValue) {
                rawValue = localStorage.getItem(projInfoKey) || localStorage.getItem(legacyKey);
                if (rawValue) {
                    localStorage.setItem(primaryKey, rawValue);
                }
            }
            if (!rawValue) {
                return null;
            }

            const parsedValue = JSON.parse(rawValue);
            if (!parsedValue || !parsedValue.data || typeof parsedValue.data !== "object") {
                return null;
            }

            this.analyticsPrefsCache[entityId] = parsedValue.data;
            return parsedValue.data;
        } catch (error) {
            return null;
        }
    }

    writeAnalyticsPrefsCache(transId, preferences) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId) || !preferences || typeof preferences !== "object") {
            return;
        }

        this.analyticsPrefsCache[entityId] = preferences;
        try {
            if (typeof localStorage === "undefined") {
                return;
            }

            localStorage.setItem(this.getAnalyticsPrefsStorageKey(entityId), JSON.stringify({
                data: preferences,
                storedOn: new Date().toISOString()
            }));
        } catch (error) {
            console.warn("Unable to write analytics preferences cache:", error);
        }
    }

    mergeAnalyticsPrefsCache(transId, partialPreferences) {
        if (!partialPreferences || typeof partialPreferences !== "object") {
            return;
        }

        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId)) {
            return;
        }

        const current = this.readAnalyticsPrefsCache(entityId) || {};
        const merged = Object.assign({}, current, partialPreferences);
        this.writeAnalyticsPrefsCache(entityId, merged);
    }

    clearAnalyticsPrefsCache(transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId)) {
            return;
        }

        delete this.analyticsPrefsCache[entityId];
        try {
            if (typeof localStorage !== "undefined") {
                localStorage.removeItem(this.getAnalyticsPrefsStorageKey(entityId));
                localStorage.removeItem(this.getAnalyticsPrefsStorageKeyProjInfo(entityId));
                localStorage.removeItem(this.getAnalyticsPrefsStorageKey(entityId, true));
            }
        } catch (error) {
            console.warn("Unable to clear analytics preferences cache:", error);
        }
    }

    getGridCardStateStorageKey(transId, useLegacy = false) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const entityId = this.normalizeUrlParam(transId || this.entityTransId || "");
        const namespace = useLegacy ? "analytics:gridCardState:v1" : "analyticsinfo:gridCardState:v1";
        return `${namespace}:${projectName}:${userName}:${entityId}`;
    }

    getGridCardStateStorageKeyProjInfo(transId) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const entityId = this.normalizeUrlParam(transId || this.entityTransId || "");
        return `projInfo-analytics:gridCardState:v1:${projectName}:${userName}:${entityId}`;
    }

    normalizeGridCardStatePayload(state, options = {}) {
        const sourceState = state && typeof state === "object" ? state : {};
        const sourceSelectionFilter = sourceState.selectionFilter && typeof sourceState.selectionFilter === "object"
            ? sourceState.selectionFilter
            : {};
        const sourceFilters = sourceSelectionFilter.filters && typeof sourceSelectionFilter.filters === "object"
            ? sourceSelectionFilter.filters
            : {};

        const normalizedFilters = {};
        Object.keys(sourceFilters).forEach(groupField => {
            const values = sourceFilters[groupField];
            if (Array.isArray(values) && values.length > 0) {
                normalizedFilters[groupField] = values.slice();
            }
        });

        return {
            cards: sourceState.cards && typeof sourceState.cards === "object" ? sourceState.cards : {},
            globalAggField: `${sourceState.globalAggField || "count"}`.trim() || "count",
            selectionFilter: {
                groupField: `${sourceSelectionFilter.groupField || ""}`.trim(),
                values: Array.isArray(sourceSelectionFilter.values) ? sourceSelectionFilter.values.slice() : [],
                filters: normalizedFilters
            },
            storedOn: options.storedOn || sourceState.storedOn || new Date().toISOString()
        };
    }

    readGridCardStateFromLocalStorage(transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId) || typeof localStorage === "undefined") {
            return null;
        }

        try {
            const primaryKey = this.getGridCardStateStorageKey(entityId);
            const projInfoKey = this.getGridCardStateStorageKeyProjInfo(entityId);
            const legacyKey = this.getGridCardStateStorageKey(entityId, true);
            let rawValue = localStorage.getItem(primaryKey);
            if (!rawValue) {
                rawValue = localStorage.getItem(projInfoKey) || localStorage.getItem(legacyKey);
                if (rawValue) {
                    localStorage.setItem(primaryKey, rawValue);
                }
            }
            if (!rawValue) {
                return null;
            }

            const parsedValue = JSON.parse(rawValue);
            if (!parsedValue || typeof parsedValue !== "object") {
                return null;
            }

            return this.normalizeGridCardStatePayload(parsedValue, { storedOn: parsedValue.storedOn });
        } catch (error) {
            return null;
        }
    }

    writeGridCardStateToLocalStorage(state, transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId) || typeof localStorage === "undefined") {
            return;
        }

        try {
            const payload = this.normalizeGridCardStatePayload(state);
            localStorage.setItem(this.getGridCardStateStorageKey(entityId), JSON.stringify(payload));
        } catch (error) {
            console.warn("Unable to write analytics grid card state:", error);
        }
    }

    readGridCardState(transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId)) {
            return null;
        }

        const memoryCacheState = this.gridCardStateCache[entityId];
        if (memoryCacheState && typeof memoryCacheState === "object") {
            return memoryCacheState;
        }

        const localState = this.readGridCardStateFromLocalStorage(entityId);
        if (localState) {
            this.gridCardStateCache[entityId] = localState;
            return localState;
        }

        this.ensureGridCardStateFromIndexedDb(entityId);
        return null;
    }

    writeGridCardState(state, transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId) || !state || typeof state !== "object") {
            return;
        }

        const payload = this.normalizeGridCardStatePayload(state);
        this.gridCardStateCache[entityId] = payload;
        this.writeGridCardStateToLocalStorage(payload, entityId);
        this.writeGridCardStateIndexedDb(payload, entityId);
    }

    ensureGridCardStateFromIndexedDb(transId) {
        const entityId = transId || this.entityTransId;
        if (_entityCommon.inValid(entityId)) {
            return Promise.resolve(null);
        }

        if (this.gridCardStateCache[entityId]) {
            return Promise.resolve(this.gridCardStateCache[entityId]);
        }

        if (this.gridCardStateIndexedDbRequests[entityId]) {
            return this.gridCardStateIndexedDbRequests[entityId];
        }

        const request = this.readGridCardStateIndexedDb(entityId).then(state => {
            if (!state) {
                return null;
            }

            const normalizedState = this.normalizeGridCardStatePayload(state, { storedOn: state.storedOn });
            this.gridCardStateCache[entityId] = normalizedState;
            this.writeGridCardStateToLocalStorage(normalizedState, entityId);

            const currentEntityId = this.normalizeUrlParam(this.entityTransId || "");
            if (currentEntityId === this.normalizeUrlParam(entityId)) {
                const container = document.querySelector("#analytics-yaxis-grid");
                const cards = container ? Array.from(container.querySelectorAll(".analytics-grid-card")) : [];
                if (cards.length > 0) {
                    this.applyStoredGridCardState(container);
                    this.loadGridCardsBatch(cards, { forceRefresh: false });
                }
            }

            return normalizedState;
        }).finally(() => {
            delete this.gridCardStateIndexedDbRequests[entityId];
        });

        this.gridCardStateIndexedDbRequests[entityId] = request;
        return request;
    }

    saveGridCardState(containerElement) {
        const container = containerElement || document.querySelector("#analytics-yaxis-grid");
        if (!container || this.isApplyingGridCardState) {
            return;
        }

        const cards = Array.from(container.querySelectorAll(".analytics-grid-card"));
        if (!cards.length) {
            return;
        }

        const cardState = {};
        const globalAggField = this.getSelectedGlobalAggField() || "count";
        cards.forEach(card => {
            const groupField = this.normalizeUrlParam(card.getAttribute("data-group-field") || "");
            if (!groupField) {
                return;
            }

            const chartSelect = card.querySelector(".analytics-grid-chart-select");
            const paletteConfig = this.getGridCardPalette(card);
            cardState[groupField] = {
                aggField: globalAggField,
                chartType: chartSelect ? (chartSelect.value || this.selectedChartType || "line") : (this.selectedChartType || "line"),
                paletteKey: paletteConfig?.paletteKey || "custom",
                customColors: Array.isArray(paletteConfig?.customColors) ? paletteConfig.customColors.slice() : this.getGridDefaultPaletteColors()
            };
        });

        this.writeGridCardState({
            cards: cardState,
            globalAggField: globalAggField,
            selectionFilter: {
                groupField: this.gridSelectionFilter.groupField || "",
                values: Array.isArray(this.gridSelectionFilter.values) ? this.gridSelectionFilter.values.slice() : [],
                filters: Object.keys(this.getGridSelectionFiltersMap()).reduce((acc, groupField) => {
                    const values = this.getGridSelectionFiltersMap()[groupField];
                    if (Array.isArray(values) && values.length > 0) {
                        acc[groupField] = values.slice();
                    }
                    return acc;
                }, {})
            }
        }, this.entityTransId);
    }

    applyStoredGridCardState(containerElement) {
        const container = containerElement || document.querySelector("#analytics-yaxis-grid");
        if (!container) {
            return;
        }

        const storedState = this.readGridCardState(this.entityTransId);
        if (!storedState || typeof storedState !== "object") {
            this.clearGridSelectionFilter();
            return;
        }

        this.isApplyingGridCardState = true;
        try {
            const storedCards = storedState.cards && typeof storedState.cards === "object" ? storedState.cards : {};
            const storedGlobalAgg = `${storedState.globalAggField || ""}`.trim();
            if (storedGlobalAgg) {
                this.setSelectedGlobalAggField(storedGlobalAgg);
            } else {
                const firstCardWithAgg = Object.keys(storedCards).find(key => !!storedCards[key]?.aggField);
                if (firstCardWithAgg) {
                    this.setSelectedGlobalAggField(storedCards[firstCardWithAgg].aggField);
                } else {
                    this.setSelectedGlobalAggField(this.globalAggField || "count");
                }
            }

            Array.from(container.querySelectorAll(".analytics-grid-card")).forEach(card => {
                const groupField = this.normalizeUrlParam(card.getAttribute("data-group-field") || "");
                const cardConfig = storedCards[groupField];
                if (!cardConfig) {
                    return;
                }

                const chartSelect = card.querySelector(".analytics-grid-chart-select");

                if (chartSelect && cardConfig.chartType) {
                    const targetChart = `${cardConfig.chartType}`;
                    const hasChart = Array.from(chartSelect.options).some(option => this.normalizeUrlParam(option.value) === this.normalizeUrlParam(targetChart));
                    if (hasChart) {
                        chartSelect.value = targetChart;
                    }
                }

                const restoredColors = Array.isArray(cardConfig.customColors) && cardConfig.customColors.length > 0
                    ? cardConfig.customColors
                    : this.resolvePaletteColorsByKey(cardConfig.paletteKey || "newPalette");
                this.applyGridCardColorInputs(card, restoredColors);
            });

            const savedFilter = storedState.selectionFilter;
            if (savedFilter && typeof savedFilter === "object") {
                const savedFiltersMap = savedFilter.filters && typeof savedFilter.filters === "object" ? savedFilter.filters : {};
                const normalizedFilters = {};

                Object.keys(savedFiltersMap).forEach(groupField => {
                    const normalizedGroupField = this.normalizeUrlParam(groupField || "");
                    const values = savedFiltersMap[groupField];
                    if (!normalizedGroupField || !Array.isArray(values) || values.length === 0) {
                        return;
                    }
                    normalizedFilters[normalizedGroupField] = values.slice();
                });

                if ((!Object.keys(normalizedFilters).length) && savedFilter.groupField && Array.isArray(savedFilter.values) && savedFilter.values.length > 0) {
                    normalizedFilters[this.normalizeUrlParam(savedFilter.groupField)] = savedFilter.values.slice();
                }

                this.gridSelectionFilter = {
                    groupField: "",
                    values: [],
                    filters: normalizedFilters
                };
                this.refreshGridSelectionPrimaryState();
            } else {
                this.clearGridSelectionFilter();
            }
        } finally {
            this.isApplyingGridCardState = false;
        }

        this.syncGridSelectedCardState();
    }

    getGridBatchStorageKey(useLegacy = false) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const transId = this.normalizeUrlParam(this.entityTransId || "");
        const namespace = useLegacy ? "analytics:multiCharts" : "analyticsinfo:multiCharts";
        return `${namespace}:${projectName}:${userName}:${transId}`;
    }

    getGridBatchStorageKeyProjInfo() {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const transId = this.normalizeUrlParam(this.entityTransId || "");
        return `projInfo-analytics:multiCharts:${projectName}:${userName}:${transId}`;
    }

    buildChartMetaFromInput(input) {
        return {
            aggField: input?.aggField || "count",
            aggFunc: input?.aggFunc || "count",
            aggTransId: input?.aggTransId || input?.transId || this.entityTransId || "",
            groupField: input?.groupField || "all",
            groupTransId: input?.groupTransId || input?.transId || this.entityTransId || ""
        };
    }

    getChartMetaKey(chartMeta) {
        const parts = [
            this.normalizeUrlParam(chartMeta?.aggField || ""),
            this.normalizeUrlParam(chartMeta?.aggFunc || ""),
            this.normalizeUrlParam(chartMeta?.aggTransId || ""),
            this.normalizeUrlParam(chartMeta?.groupField || ""),
            this.normalizeUrlParam(chartMeta?.groupTransId || "")
        ];
        return parts.join("~");
    }

    getGridBatchRequestSignature(chartMetaList) {
        try {
            return JSON.stringify((chartMetaList || []).map(item => ({
                aggField: this.normalizeUrlParam(item.aggField || ""),
                aggFunc: this.normalizeUrlParam(item.aggFunc || ""),
                aggTransId: this.normalizeUrlParam(item.aggTransId || ""),
                groupField: this.normalizeUrlParam(item.groupField || ""),
                groupTransId: this.normalizeUrlParam(item.groupTransId || "")
            })));
        } catch (error) {
            return "[]";
        }
    }

    parseChartsDataPayload(chartEntry) {
        if (!chartEntry) {
            return [];
        }

        let rawData = chartEntry.data_json;
        if (typeof rawData === "string") {
            try {
                rawData = JSON.parse(rawData);
            } catch (error) {
                rawData = [];
            }
        }

        return Array.isArray(rawData) ? rawData : [];
    }

    hydrateGridBatchMemory(chartMetaList, charts) {
        if (!Array.isArray(chartMetaList) || !Array.isArray(charts)) {
            return;
        }

        chartMetaList.forEach((chartMeta, index) => {
            const chartKey = this.getChartMetaKey(chartMeta);
            this.gridBatchChartsByKey[chartKey] = this.parseChartsDataPayload(charts[index]);
        });
    }

    readGridBatchCache(signature) {
        try {
            if (typeof localStorage === "undefined") {
                return null;
            }

            const primaryKey = this.getGridBatchStorageKey();
            const projInfoKey = this.getGridBatchStorageKeyProjInfo();
            const legacyKey = this.getGridBatchStorageKey(true);
            let rawValue = localStorage.getItem(primaryKey);
            if (!rawValue) {
                rawValue = localStorage.getItem(projInfoKey) || localStorage.getItem(legacyKey);
                if (rawValue) {
                    localStorage.setItem(primaryKey, rawValue);
                }
            }
            if (!rawValue) {
                return null;
            }

            const parsedValue = JSON.parse(rawValue);
            if (!parsedValue || parsedValue.signature !== signature || !Array.isArray(parsedValue.charts)) {
                return null;
            }

            return parsedValue.charts;
        } catch (error) {
            return null;
        }
    }

    writeGridBatchCache(signature, charts) {
        try {
            if (typeof localStorage === "undefined") {
                return;
            }

            const payload = {
                signature: signature,
                charts: Array.isArray(charts) ? charts : [],
                storedOn: new Date().toISOString()
            };
            localStorage.setItem(this.getGridBatchStorageKey(), JSON.stringify(payload));
        } catch (error) {
            console.warn("Unable to write analytics chart cache:", error);
        }
    }

    clearGridBatchCache() {
        this.gridBatchChartsByKey = {};
        this.gridBatchRequests = {};
        this.clearGridSelectionFilter();
        this.clearEntityListRowsCache();

        try {
            if (typeof localStorage !== "undefined") {
                localStorage.removeItem(this.getGridBatchStorageKey());
                localStorage.removeItem(this.getGridBatchStorageKeyProjInfo());
                localStorage.removeItem(this.getGridBatchStorageKey(true));
            }
        } catch (error) {
            console.warn("Unable to clear analytics chart cache:", error);
        }
    }

    fetchAnalyticsMultipleChartsData(chartMetaList, options = {}) {
        const safeChartMetaList = Array.isArray(chartMetaList) ? chartMetaList : [];
        if (!safeChartMetaList.length || _entityCommon.inValid(this.entityTransId)) {
            return Promise.resolve([]);
        }

        const forceRefresh = !!options.forceRefresh;
        const signature = this.getGridBatchRequestSignature(safeChartMetaList);
        const requestKey = `${this.normalizeUrlParam(this.entityTransId)}::${signature}`;

        if (!forceRefresh) {
            const cachedCharts = this.readGridBatchCache(signature);
            if (Array.isArray(cachedCharts)) {
                this.hydrateGridBatchMemory(safeChartMetaList, cachedCharts);
                this.logFilterDebug("fetchAnalyticsMultipleChartsData:localStorageCacheHit", {
                    transId: this.entityTransId,
                    requestedCharts: safeChartMetaList.length,
                    cachedCharts: cachedCharts.length,
                    signature: signature
                });
                return Promise.resolve(cachedCharts);
            }
        }

        if (!forceRefresh && this.gridBatchRequests[requestKey]) {
            return this.gridBatchRequests[requestKey];
        }

        const requestPromise = new Promise((resolve, reject) => {
            this.logFilterDebug("fetchAnalyticsMultipleChartsData:serverRequest", {
                transId: this.entityTransId,
                forceRefresh: forceRefresh,
                requestedCharts: safeChartMetaList.length,
                chartMetaList: safeChartMetaList
            });
            $.ajax({
                url: `${this.getAssetBaseUrl()}/aspx/Analytics.aspx/GetAnalyticsMultipleChartsDataWS`,
                type: 'POST',
                cache: false,
                async: true,
                dataType: 'json',
                data: JSON.stringify({
                    page: "Entity",
                    transId: this.entityTransId,
                    charts: safeChartMetaList
                }),
                contentType: "application/json",
                success: (data) => {
                    try {
                        const parsedResponse = typeof data?.d === "string" ? JSON.parse(data.d) : (data?.d || data);
                        if (!(parsedResponse?.result?.success)) {
                            resolve([]);
                            return;
                        }

                        const charts = Array.isArray(parsedResponse?.result?.charts) ? parsedResponse.result.charts : [];
                        this.logFilterDebug("fetchAnalyticsMultipleChartsData:serverResponse", {
                            transId: this.entityTransId,
                            requestedCharts: safeChartMetaList.length,
                            receivedCharts: charts.length,
                            sample: charts.slice(0, 3).map((entry, index) => {
                                const parsedRows = this.parseChartsDataPayload(entry);
                                return {
                                    index: index,
                                    rows: Array.isArray(parsedRows) ? parsedRows.length : 0,
                                    firstRow: Array.isArray(parsedRows) && parsedRows.length > 0 ? parsedRows[0] : null
                                };
                            })
                        });
                        this.hydrateGridBatchMemory(safeChartMetaList, charts);
                        this.writeGridBatchCache(signature, charts);
                        resolve(charts);
                    } catch (error) {
                        reject(error);
                    } finally {
                        delete this.gridBatchRequests[requestKey];
                    }
                },
                error: (error) => {
                    delete this.gridBatchRequests[requestKey];
                    reject(error);
                }
            });
        });

        this.gridBatchRequests[requestKey] = requestPromise;
        return requestPromise;
    }

    getCachedGridChartData(chartMetaKey, input) {
        const chartsData = this.gridBatchChartsByKey[chartMetaKey];
        if (!Array.isArray(chartsData)) {
            return null;
        }

        return this.applyDateRangeFilter(chartsData, input);
    }

    getEntityListCacheKey(transId, requestedFields) {
        const effectiveTransId = this.normalizeUrlParam(transId || this.entityTransId || "");
        const fieldKey = requestedFields || this.getEntityListRequestFieldsString();
        const filterKey = this.getEntityListRequestFilterString();
        return `${effectiveTransId}::${fieldKey}::${filterKey}`;
    }

    getEntityListFilterPayload() {
        let filterPayload = Array.isArray(this.filter) ? this.filter : [];

        if (!filterPayload.length && typeof _entityFilter !== "undefined" && _entityFilter && Array.isArray(_entityFilter.activeFilterArray)) {
            filterPayload = _entityFilter.activeFilterArray;
        }

        try {
            return JSON.parse(JSON.stringify(filterPayload || []));
        } catch (error) {
            return Array.isArray(filterPayload) ? filterPayload.slice() : [];
        }
    }

    getEntityListRequestFilterString() {
        try {
            return JSON.stringify(this.getEntityListFilterPayload());
        } catch (error) {
            return "[]";
        }
    }

    getEntityListRequestFieldsList() {
        const requestedFields = [];
        const seenFields = new Set();
        const metaData = Array.isArray(this.metaData) ? this.metaData : [];
        const hasMetaData = metaData.length > 0;
        const metaFieldSet = new Set(metaData.map(item => this.normalizeUrlParam(item?.fldname || "")).filter(Boolean));

        const addField = (fieldName) => {
            if (_entityCommon.inValid(fieldName)) {
                return;
            }
            const normalized = this.normalizeUrlParam(fieldName);
            if (!normalized || normalized === "all" || seenFields.has(normalized)) {
                return;
            }

            const metaField = metaData.find(item => this.normalizeUrlParam(item?.fldname || "") === normalized);
            if (hasMetaData && !metaField) {
                return;
            }
            const resolvedFieldName = (metaField?.fldname || `${fieldName}`).trim();
            if (_entityCommon.inValid(resolvedFieldName)) {
                return;
            }

            seenFields.add(normalized);
            requestedFields.push(resolvedFieldName);
        };

        const addFieldsFromCsv = (fieldCsv) => {
            if (_entityCommon.inValid(fieldCsv)) {
                return;
            }
            `${fieldCsv}`.split(",").forEach(item => addField(item.trim()));
        };

        const hasSelectedXAxis = this.xAxisFields && this.xAxisFields.trim() !== "" && this.xAxisFields.trim() !== "All";
        const hasSelectedYAxis = this.yAxisFields && this.yAxisFields.trim() !== "" && this.yAxisFields.trim() !== "All";

        if (hasSelectedXAxis) {
            addFieldsFromCsv(this.xAxisFields);
        } else {
            this.getGridAggFieldOptionsRaw().forEach(option => {
                if (this.normalizeUrlParam(option?.value || "") !== "count") {
                    addField(option.value);
                }
            });
        }

        if (hasSelectedYAxis) {
            addFieldsFromCsv(this.yAxisFields);
        } else {
            this.getGridGroupFields().forEach(option => addField(option.value));
        }

        const preferredDateField = this.getPreferredAnalyticsDateField();
        if (!hasMetaData || metaFieldSet.has(this.normalizeUrlParam(preferredDateField))) {
            addField(preferredDateField);
        }

        if (!hasMetaData || metaFieldSet.has("modifiedon")) {
            addField("modifiedon");
        }
        if (!hasMetaData || metaFieldSet.has("createdon")) {
            addField("createdon");
        }

        return requestedFields;
    }

    getEntityListRequestFieldsString() {
        const requestedFields = this.getEntityListRequestFieldsList();
        return requestedFields.length > 0 ? requestedFields.join(",") : "All";
    }

    getEntityListStorageKey(transId, useLegacy = false) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const effectiveTransId = this.normalizeUrlParam(transId || this.entityTransId || "");
        const namespace = useLegacy ? "analytics:entityRows:v2" : "analyticsinfo:entityRows:v2";
        return `${namespace}:${projectName}:${userName}:${effectiveTransId}`;
    }

    getEntityListStorageKeyProjInfo(transId) {
        const projectName = this.normalizeUrlParam(this.getCurrentProjectName() || "project");
        const userName = this.normalizeUrlParam(this.getCurrentUserName() || "anonymous");
        const effectiveTransId = this.normalizeUrlParam(transId || this.entityTransId || "");
        return `projInfo-analytics:entityRows:v2:${projectName}:${userName}:${effectiveTransId}`;
    }

    readEntityListRowsCache(transId, signature) {
        try {
            if (typeof localStorage === "undefined") {
                return null;
            }

            const primaryKey = this.getEntityListStorageKey(transId);
            const projInfoKey = this.getEntityListStorageKeyProjInfo(transId);
            const legacyKey = this.getEntityListStorageKey(transId, true);
            let rawValue = localStorage.getItem(primaryKey);
            if (!rawValue) {
                rawValue = localStorage.getItem(projInfoKey) || localStorage.getItem(legacyKey);
                if (rawValue) {
                    localStorage.setItem(primaryKey, rawValue);
                }
            }
            if (!rawValue) {
                return null;
            }

            const parsedValue = JSON.parse(rawValue);
            if (!parsedValue || parsedValue.signature !== signature || !Array.isArray(parsedValue.rows)) {
                return null;
            }

            return parsedValue.rows;
        } catch (error) {
            return null;
        }
    }

    writeEntityListRowsCache(transId, signature, rows) {
        try {
            if (typeof localStorage === "undefined") {
                return;
            }

            localStorage.setItem(this.getEntityListStorageKey(transId), JSON.stringify({
                signature: signature,
                rows: Array.isArray(rows) ? rows : [],
                storedOn: new Date().toISOString()
            }));
        } catch (error) {
            console.warn("Unable to write analytics row cache:", error);
        }
    }

    openAnalyticsIndexedDb() {
        if (this.analyticsIndexedDb) {
            return Promise.resolve(this.analyticsIndexedDb);
        }

        if (this.analyticsIndexedDbOpenPromise) {
            return this.analyticsIndexedDbOpenPromise;
        }

        this.analyticsIndexedDbOpenPromise = new Promise((resolve) => {
            if (typeof indexedDB === "undefined") {
                resolve(null);
                return;
            }

            try {
                const request = indexedDB.open("analyticsinfo-cache", 2);

                request.onupgradeneeded = (event) => {
                    const db = event?.target?.result;
                    if (!db) {
                        return;
                    }

                    if (!db.objectStoreNames.contains("entityRows")) {
                        db.createObjectStore("entityRows");
                    }

                    if (!db.objectStoreNames.contains("gridCardState")) {
                        db.createObjectStore("gridCardState");
                    }
                };

                request.onsuccess = () => {
                    this.analyticsIndexedDb = request.result || null;
                    resolve(this.analyticsIndexedDb);
                };

                request.onerror = () => {
                    resolve(null);
                };
            } catch (error) {
                resolve(null);
            }
        });

        return this.analyticsIndexedDbOpenPromise;
    }

    readEntityListRowsCacheIndexedDb(transId, signature) {
        return this.openAnalyticsIndexedDb().then(db => {
            if (!db) {
                return null;
            }

            return new Promise((resolve) => {
                try {
                    const request = db
                        .transaction("entityRows", "readonly")
                        .objectStore("entityRows")
                        .get(this.getEntityListStorageKey(transId));

                    request.onsuccess = () => {
                        const payload = request.result;
                        if (!payload || payload.signature !== signature || !Array.isArray(payload.rows)) {
                            resolve(null);
                            return;
                        }
                        resolve(payload.rows);
                    };

                    request.onerror = () => resolve(null);
                } catch (error) {
                    resolve(null);
                }
            });
        });
    }

    writeEntityListRowsCacheIndexedDb(transId, signature, rows) {
        return this.openAnalyticsIndexedDb().then(db => {
            if (!db) {
                return;
            }

            return new Promise((resolve) => {
                try {
                    const payload = {
                        signature: signature,
                        rows: Array.isArray(rows) ? rows : [],
                        storedOn: new Date().toISOString()
                    };

                    const request = db
                        .transaction("entityRows", "readwrite")
                        .objectStore("entityRows")
                        .put(payload, this.getEntityListStorageKey(transId));

                    request.onsuccess = () => resolve();
                    request.onerror = () => resolve();
                } catch (error) {
                    resolve();
                }
            });
        });
    }

    readGridCardStateIndexedDb(transId) {
        return this.openAnalyticsIndexedDb().then(db => {
            if (!db) {
                return null;
            }

            return new Promise((resolve) => {
                try {
                    const storageKey = this.getGridCardStateStorageKey(transId);
                    const request = db
                        .transaction("gridCardState", "readonly")
                        .objectStore("gridCardState")
                        .get(storageKey);

                    request.onsuccess = () => {
                        const payload = request.result;
                        if (!payload || typeof payload !== "object") {
                            resolve(null);
                            return;
                        }
                        resolve(this.normalizeGridCardStatePayload(payload, { storedOn: payload.storedOn }));
                    };

                    request.onerror = () => resolve(null);
                } catch (error) {
                    resolve(null);
                }
            });
        });
    }

    writeGridCardStateIndexedDb(state, transId) {
        return this.openAnalyticsIndexedDb().then(db => {
            if (!db) {
                return;
            }

            return new Promise((resolve) => {
                try {
                    const storageKey = this.getGridCardStateStorageKey(transId);
                    const payload = this.normalizeGridCardStatePayload(state);
                    const request = db
                        .transaction("gridCardState", "readwrite")
                        .objectStore("gridCardState")
                        .put(payload, storageKey);

                    request.onsuccess = () => resolve();
                    request.onerror = () => resolve();
                } catch (error) {
                    resolve();
                }
            });
        });
    }

    clearEntityListRowsCacheIndexedDb(transId) {
        return this.openAnalyticsIndexedDb().then(db => {
            if (!db) {
                return;
            }

            return new Promise((resolve) => {
                try {
                    const store = db.transaction("entityRows", "readwrite").objectStore("entityRows");
                    const request = _entityCommon.inValid(transId)
                        ? store.clear()
                        : store.delete(this.getEntityListStorageKey(transId));

                    request.onsuccess = () => resolve();
                    request.onerror = () => resolve();
                } catch (error) {
                    resolve();
                }
            });
        });
    }

    clearEntityListRowsCache(transId) {
        const effectiveTransId = transId || this.entityTransId;
        if (_entityCommon.inValid(effectiveTransId)) {
            this.entityListDataCache = {};
            this.entityListDataRequests = {};
            this.clearEntityListRowsCacheIndexedDb();
            return;
        }

        const cachePrefix = `${this.normalizeUrlParam(effectiveTransId)}::`;
        Object.keys(this.entityListDataCache || {}).forEach(key => {
            if (key.indexOf(cachePrefix) === 0) {
                delete this.entityListDataCache[key];
            }
        });

        Object.keys(this.entityListDataRequests || {}).forEach(key => {
            if (key.indexOf(cachePrefix) === 0) {
                delete this.entityListDataRequests[key];
            }
        });

        try {
            if (typeof localStorage !== "undefined") {
                localStorage.removeItem(this.getEntityListStorageKey(effectiveTransId));
                localStorage.removeItem(this.getEntityListStorageKeyProjInfo(effectiveTransId));
                localStorage.removeItem(this.getEntityListStorageKey(effectiveTransId, true));
            }
        } catch (error) {
            console.warn("Unable to clear analytics row cache:", error);
        }
        this.clearEntityListRowsCacheIndexedDb(effectiveTransId);
    }

    getAxListCaller() {
        if (typeof parent !== "undefined" && parent && typeof parent.GetDataFromAxList === "function") {
            return parent;
        }
        if (typeof window !== "undefined" && window && typeof window.GetDataFromAxList === "function") {
            return window;
        }
        return null;
    }

    parseAxListResponsePayload(response) {
        let parsed = response;
        try {
            if (typeof parsed === "string") {
                parsed = JSON.parse(parsed);
            }
            if (parsed && typeof parsed.d === "string") {
                parsed = JSON.parse(parsed.d);
            } else if (parsed && parsed.d && typeof parsed.d === "object" && !parsed.result) {
                parsed = parsed.d;
            }
        } catch (error) {
            this.logFilterDebug("parseAxListResponsePayload:parseError", {
                message: error?.message || "Unknown parse error"
            });
        }
        return parsed || {};
    }

    extractAdsRowsFromAxResponse(parsedResponse) {
        const dataList = Array.isArray(parsedResponse?.result?.data)
            ? parsedResponse.result.data
            : (Array.isArray(parsedResponse?.data) ? parsedResponse.data : []);

        let dsBlock = null;
        if (Array.isArray(dataList) && dataList.length > 0) {
            dsBlock = dataList.find(item => item && typeof item === "object") || dataList[0];
        }

        if (!dsBlock && parsedResponse && typeof parsedResponse === "object") {
            dsBlock = parsedResponse?.result || parsedResponse;
        }

        let rows = [];
        if (Array.isArray(dsBlock?.data)) {
            rows = dsBlock.data;
        } else if (Array.isArray(parsedResponse?.data) && (!parsedResponse?.data[0] || !Array.isArray(parsedResponse?.data[0]?.data))) {
            rows = parsedResponse.data;
        }

        const columns = Array.isArray(dsBlock?.columns)
            ? dsBlock.columns
            : (Array.isArray(parsedResponse?.columns) ? parsedResponse.columns : []);

        return {
            dsBlock: dsBlock || {},
            rows: Array.isArray(rows) ? rows.filter(item => item && typeof item === "object") : [],
            columns: Array.isArray(columns) ? columns : []
        };
    }

    getAdsRowValue(row, fieldName) {
        if (!row || typeof row !== "object" || _entityCommon.inValid(fieldName)) {
            return null;
        }
        if (Object.prototype.hasOwnProperty.call(row, fieldName)) {
            return row[fieldName];
        }

        const normalizedField = this.getNormalizedFieldKey(fieldName);
        const matchedKey = Object.keys(row).find(key => this.getNormalizedFieldKey(key) === normalizedField);
        return matchedKey ? row[matchedKey] : null;
    }

    inferAdsFieldType(fieldName, sampleValue, columnDef = {}) {
        const dataTypeToken = this.normalizeUrlParam(
            columnDef?.datatype ||
            columnDef?.fdatatype ||
            columnDef?.type ||
            columnDef?.cdatatype ||
            ""
        );

        const numericTokens = ["n", "num", "number", "numeric", "decimal", "currency", "amount", "double", "float", "int", "integer", "long", "short"];
        const dateTokens = ["d", "date", "datetime", "timestamp", "time"];

        if (numericTokens.some(token => dataTypeToken === token || dataTypeToken.indexOf(token) > -1)) {
            return "n";
        }
        if (dateTokens.some(token => dataTypeToken === token || dataTypeToken.indexOf(token) > -1)) {
            return "d";
        }

        if (typeof sampleValue === "number" && !isNaN(sampleValue)) {
            return "n";
        }

        if (typeof sampleValue === "string") {
            const trimmedValue = sampleValue.trim();
            if (!trimmedValue) {
                return "c";
            }

            if (this.parseDateString(trimmedValue)) {
                return "d";
            }

            if (/^[+-]?\d[\d,]*(\.\d+)?$/.test(trimmedValue)) {
                return "n";
            }
        }

        const normalizedField = this.normalizeUrlParam(fieldName || "");
        if (normalizedField.includes("date") || normalizedField.endsWith("on")) {
            return "d";
        }
        if (normalizedField.includes("amount") || normalizedField.includes("value") || normalizedField.includes("total") || normalizedField.includes("tax") || normalizedField.includes("qty")) {
            return "n";
        }

        return "c";
    }

    formatAnalyticsFieldCaption(fieldName) {
        const source = `${fieldName || ""}`.trim();
        if (!source) {
            return "";
        }

        return source
            .replace(/[_\-]+/g, " ")
            .replace(/([a-z])([A-Z])/g, "$1 $2")
            .replace(/\s+/g, " ")
            .trim()
            .replace(/\b\w/g, char => char.toUpperCase());
    }

    sanitizeAxisFieldsAgainstMeta(metaData) {
        const safeMetaData = Array.isArray(metaData) ? metaData : [];
        if (!safeMetaData.length) {
            this.xAxisFields = "All";
            this.yAxisFields = "All";
            return;
        }

        const availableFields = new Set(
            safeMetaData
                .map(item => this.normalizeUrlParam(item?.fldname || ""))
                .filter(Boolean)
        );

        const sanitizeCsv = (fieldCsv) => {
            const rawValue = `${fieldCsv || ""}`.trim();
            if (!rawValue || rawValue.toLowerCase() === "all") {
                return "All";
            }

            const validFields = rawValue
                .split(",")
                .map(item => item.trim())
                .filter(item => {
                    const normalized = this.normalizeUrlParam(item);
                    return normalized && availableFields.has(normalized);
                });

            return validFields.length > 0 ? validFields.join(",") : "All";
        };

        this.xAxisFields = sanitizeCsv(this.xAxisFields);
        this.yAxisFields = sanitizeCsv(this.yAxisFields);
    }

    buildAdsMetaData(adsName, columns, rows) {
        const sourceRows = Array.isArray(rows) ? rows : [];
        const metaMap = new Map();

        const addMetaField = (rawFieldName, columnDef = {}) => {
            const fieldName = `${rawFieldName || ""}`.trim();
            if (!fieldName) {
                return;
            }

            const normalizedField = this.normalizeUrlParam(fieldName);
            if (!normalizedField || metaMap.has(normalizedField)) {
                return;
            }

            const sampleRow = sourceRows.find(row => this.getAdsRowValue(row, fieldName) !== null && this.getAdsRowValue(row, fieldName) !== undefined);
            const sampleValue = sampleRow ? this.getAdsRowValue(sampleRow, fieldName) : null;
            const fdatatype = this.inferAdsFieldType(fieldName, sampleValue, columnDef);

            const normalizedRaw = (columnDef?.normalized ?? columnDef?.isnormalized ?? columnDef?.isNormalized ?? columnDef?.is_normalized ?? "");
            const isNormalized = normalizedRaw === true || `${normalizedRaw || ""}`.toUpperCase() === "T";

            let cdatatype = "DropDown";
            if (fdatatype === "n") {
                cdatatype = "Numeric";
            } else if (fdatatype === "d") {
                cdatatype = "Date";
            }

            if (isNormalized) {
                cdatatype = "DropDown";
            }

            const fldcap = `${columnDef?.caption || columnDef?.fldcap || this.formatAnalyticsFieldCaption(fieldName)}`.trim();
            const isNumeric = fdatatype === "n";
            const isDate = fdatatype === "d";

            metaMap.set(normalizedField, {
                fldname: fieldName,
                fldcap: fldcap || fieldName,
                fdatatype: fdatatype,
                cdatatype: cdatatype,
                aggfield: (isNumeric || isDate) ? "T" : "F",
                grpfield: (fdatatype === "c" || isDate || isNormalized) ? "T" : "F",
                hide: "F",
                listingfld: "T",
                filters: "T",
                normalized: isNormalized ? "T" : "F",
                ftransid: adsName,
                dcname: columnDef?.dcname || "",
                griddc: columnDef?.griddc || ""
            });
        };

        if (Array.isArray(columns) && columns.length > 0) {
            columns.forEach(column => {
                const fieldName = column?.key || column?.name || column?.fldname || column?.fieldname || column?.column || column?.colname || "";
                addMetaField(fieldName, column || {});
            });
        }

        if (metaMap.size === 0 && sourceRows.length > 0) {
            Object.keys(sourceRows[0] || {}).forEach(fieldName => addMetaField(fieldName, {}));
        } else if (sourceRows.length > 0) {
            Object.keys(sourceRows[0] || {}).forEach(fieldName => addMetaField(fieldName, {}));
        }

        return Array.from(metaMap.values());
    }

    normalizeFlagToTF(value, defaultValue = "F") {
        if (value === true) {
            return "T";
        }
        if (value === false) {
            return "F";
        }
        const normalizedValue = `${value || ""}`.trim().toUpperCase();
        if (normalizedValue === "T" || normalizedValue === "TRUE" || normalizedValue === "1" || normalizedValue === "Y" || normalizedValue === "YES") {
            return "T";
        }
        if (normalizedValue === "F" || normalizedValue === "FALSE" || normalizedValue === "0" || normalizedValue === "N" || normalizedValue === "NO") {
            return "F";
        }
        return defaultValue;
    }

    normalizeAdsMetaRows(metaRows, adsName) {
        if (!Array.isArray(metaRows) || metaRows.length === 0) {
            return [];
        }

        const fieldMetaMap = new Map();
        const knownRow = metaRows.find(item => item && typeof item === "object") || {};

        metaRows.forEach((row) => {
            if (!row || typeof row !== "object") {
                return;
            }

            const fieldName = `${row.fldname || row.fieldname || row.name || row.column || row.colname || row.srcfield || ""}`.trim();
            if (!fieldName) {
                return;
            }

            const normalizedField = this.normalizeUrlParam(fieldName);
            if (!normalizedField || fieldMetaMap.has(normalizedField)) {
                return;
            }

            const fdatatype = this.inferAdsFieldType(
                fieldName,
                this.getAdsRowValue(knownRow, fieldName),
                row
            );

            const normalizedToken = this.normalizeFlagToTF(
                row.normalized ?? row.isnormalized ?? row.isNormalized ?? row.is_normalized,
                "F"
            );
            const isNormalized = normalizedToken === "T";

            const cdatatypeRaw = `${row.cdatatype || row.column_type || row.columntype || row.controltype || ""}`.trim();
            let cdatatype = cdatatypeRaw || (fdatatype === "n" ? "Numeric" : (fdatatype === "d" ? "Date" : "DropDown"));
            if (/^dropdown$/i.test(cdatatype) || /^drop[\s_-]*down$/i.test(cdatatype) || /^select$/i.test(cdatatype)) {
                cdatatype = "DropDown";
            }
            if (isNormalized) {
                cdatatype = "DropDown";
            }

            const defaultAggField = (fdatatype === "n" || fdatatype === "d") ? "T" : "F";
            const defaultGrpField = (isNormalized || fdatatype === "c" || fdatatype === "d") ? "T" : "F";

            fieldMetaMap.set(normalizedField, {
                fldname: fieldName,
                fldcap: `${row.fldcaption || row.fldcap || row.caption || row.displayname || this.formatAnalyticsFieldCaption(fieldName)}`.trim() || fieldName,
                fdatatype: fdatatype,
                cdatatype: cdatatype,
                aggfield: this.normalizeFlagToTF(row.aggfield ?? row.agg_field ?? row.isaggfield, defaultAggField),
                grpfield: this.normalizeFlagToTF(row.grpfield ?? row.groupfield ?? row.grp_field ?? row.isgrpfield, defaultGrpField),
                hide: this.normalizeFlagToTF(row.hide ?? row.ishidden ?? row.hidden, "F"),
                listingfld: this.normalizeFlagToTF(row.listingfld ?? row.islistingfield ?? row.listingfield, "T"),
                filters: this.normalizeFlagToTF(row.filters ?? row.isfilter ?? row.filterable, "T"),
                normalized: normalizedToken,
                dcname: row.dcname || row.dc || "",
                griddc: row.griddc || row.dcname || "",
                ftransid: `${row.ftransid || row.transid || adsName || ""}`.trim(),
                srctable: `${row.srctable || row.src_table || row.sourcetable || ""}`.trim(),
                srcfld: `${row.srcfld || row.src_fld || row.srcfield || row.sourcefield || ""}`.trim(),
                psrctxt: `${row.psrctxt || row.psrctext || ""}`.trim()
            });
        });

        return Array.from(fieldMetaMap.values());
    }

    parseTransidFieldsSourceMap(rawFields) {
        const sourceMap = {};
        const rawValue = `${rawFields || ""}`.trim();
        if (!rawValue) {
            return sourceMap;
        }

        const segments = rawValue.split("|").map(item => `${item || ""}`.trim()).filter(Boolean);
        let lastKnownTable = "";

        segments.forEach((segment) => {
            let fieldBlock = segment;
            let tablePrefix = "";

            const equalIndex = segment.indexOf("=");
            if (equalIndex > -1) {
                tablePrefix = segment.substring(0, equalIndex).trim();
                fieldBlock = segment.substring(equalIndex + 1).trim();
            }

            const tokens = fieldBlock.split("~").map(item => `${item || ""}`.trim());
            const fieldName = tokens[0] || "";
            if (!fieldName) {
                return;
            }

            const normalizedField = this.normalizeUrlParam(fieldName);
            if (!normalizedField) {
                return;
            }

            const normalizedToken = this.normalizeFlagToTF(tokens[1] || "F", "F");
            let srcTable = tokens[2] || "";
            let srcField = tokens[3] || "";

            if (tablePrefix) {
                lastKnownTable = tablePrefix;
            }

            if (!srcTable && tablePrefix) {
                srcTable = tablePrefix;
            }
            if (!srcTable && lastKnownTable) {
                srcTable = lastKnownTable;
            }
            if (!srcField && srcTable) {
                srcField = fieldName;
            }

            const psrctxt = (srcTable && srcField)
                ? `${fieldName}~${normalizedToken}~${srcTable}~${srcField}`
                : "";

            sourceMap[normalizedField] = {
                fldname: fieldName,
                normalized: normalizedToken,
                srctable: srcTable,
                srcfld: srcField,
                psrctxt: psrctxt
            };
        });

        return sourceMap;
    }

    mergeTransidFieldSourceIntoMeta(metaData, rawFields) {
        const safeMetaData = Array.isArray(metaData) ? metaData : [];
        if (safeMetaData.length === 0) {
            return [];
        }

        const sourceMap = this.parseTransidFieldsSourceMap(rawFields);
        const sourceKeys = Object.keys(sourceMap);
        if (sourceKeys.length === 0) {
            return safeMetaData;
        }

        return safeMetaData.map((item) => {
            const normalizedField = this.normalizeUrlParam(item?.fldname || "");
            const sourceInfo = sourceMap[normalizedField];
            if (!sourceInfo) {
                return item;
            }

            const next = { ...item };
            next.normalized = `${next.normalized || sourceInfo.normalized || "F"}`.trim().toUpperCase() || "F";
            if (!next.srctable && sourceInfo.srctable) {
                next.srctable = sourceInfo.srctable;
            }
            if (!next.srcfld && sourceInfo.srcfld) {
                next.srcfld = sourceInfo.srcfld;
            }
            if (!next.psrctxt && sourceInfo.psrctxt) {
                next.psrctxt = sourceInfo.psrctxt;
            }

            return next;
        });
    }

    mergeMetaDataWithSupplemental(supplementalMeta) {
        const baseMeta = Array.isArray(this.metaData) ? this.metaData : [];
        const extraMeta = Array.isArray(supplementalMeta) ? supplementalMeta : [];
        if (baseMeta.length === 0 || extraMeta.length === 0) {
            return baseMeta;
        }

        const extraByField = new Map();
        extraMeta.forEach((item) => {
            const normalizedField = this.normalizeUrlParam(item?.fldname || "");
            if (!normalizedField || extraByField.has(normalizedField)) {
                return;
            }
            extraByField.set(normalizedField, item);
        });

        this.metaData = baseMeta.map((item) => {
            const normalizedField = this.normalizeUrlParam(item?.fldname || "");
            const extra = extraByField.get(normalizedField);
            if (!extra) {
                return item;
            }

            const next = { ...item };
            const nextNormalized = this.normalizeFlagToTF(next.normalized ?? extra.normalized, "F");
            next.normalized = nextNormalized;

            if (!`${next.srctable || ""}`.trim() && `${extra.srctable || ""}`.trim()) {
                next.srctable = `${extra.srctable}`.trim();
            }
            if (!`${next.srcfld || ""}`.trim() && `${extra.srcfld || ""}`.trim()) {
                next.srcfld = `${extra.srcfld}`.trim();
            }
            if (!`${next.psrctxt || ""}`.trim() && `${extra.psrctxt || ""}`.trim()) {
                next.psrctxt = `${extra.psrctxt}`.trim();
            }
            if (!`${next.ftransid || ""}`.trim() && `${extra.ftransid || ""}`.trim()) {
                next.ftransid = `${extra.ftransid}`.trim();
            }
            if (!`${next.cdatatype || ""}`.trim() && `${extra.cdatatype || ""}`.trim()) {
                next.cdatatype = `${extra.cdatatype}`.trim();
            }

            if (!`${next.psrctxt || ""}`.trim() && `${next.fldname || ""}`.trim() && `${next.srctable || ""}`.trim() && `${next.srcfld || ""}`.trim()) {
                next.psrctxt = `${next.fldname}~${nextNormalized}~${next.srctable}~${next.srcfld}`;
            }

            return next;
        });

        return this.metaData;
    }

    hasDropdownSourceGap(metaData = this.metaData) {
        const safeMetaData = Array.isArray(metaData) ? metaData : [];
        return safeMetaData.some((item) => {
            if (!item || typeof item !== "object") {
                return false;
            }

            const cdatatype = `${item.cdatatype || ""}`.trim().toLowerCase();
            const isDropDown = cdatatype === "dropdown" || cdatatype === "drop down" || cdatatype === "drop_down" || cdatatype === "select";
            const isNormalized = this.normalizeFlagToTF(item.normalized, "F") === "T";
            if (!isDropDown && !isNormalized) {
                return false;
            }

            const isFilterEnabled = this.normalizeFlagToTF(item.filters, "T") === "T";
            if (!isFilterEnabled) {
                return false;
            }

            const hasPsrcTxt = `${item.psrctxt || item.psrcTxt || item.psrctext || ""}`.trim() !== "";
            const hasSourceFields = `${item.srctable || ""}`.trim() !== "" && `${item.srcfld || ""}`.trim() !== "";
            return !hasPsrcTxt && !hasSourceFields;
        });
    }

    fetchEntityFieldSourceMeta(transId, options = {}) {
        const effectiveTransId = `${transId || this.entityTransId || ""}`.trim();
        if (!effectiveTransId) {
            return Promise.resolve([]);
        }

        const forceRefresh = !!options.forceRefresh;
        const jobs = [];

        jobs.push(
            this.fetchAdsMetadataFromAxList(effectiveTransId, { forceRefresh: forceRefresh })
                .then((metaFromAds) => {
                    if (Array.isArray(metaFromAds) && metaFromAds.length > 0) {
                        this.mergeMetaDataWithSupplemental(metaFromAds);
                    }
                    return metaFromAds;
                })
                .catch(() => [])
        );

        if (!this.isAdsMode()) {
            jobs.push(new Promise((resolve) => {
                $.ajax({
                    url: `${this.getAssetBaseUrl()}/aspx/Entity.aspx/GetEntityListDataWS`,
                    type: "POST",
                    cache: false,
                    async: true,
                    dataType: "json",
                    data: JSON.stringify({
                        transId: effectiveTransId,
                        fields: this.getEntityListRequestFieldsString(),
                        pageNo: 1,
                        pageSize: 1,
                        filter: this.getEntityListFilterPayload()
                    }),
                    contentType: "application/json",
                    success: (data) => {
                        try {
                            const parsedResponse = typeof data?.d === "string" ? JSON.parse(data.d) : (data?.d || data);
                            const fieldsRaw = parsedResponse?.result?.data?.Fields || parsedResponse?.result?.data?.fields || "";
                            if (fieldsRaw) {
                                this.metaData = this.mergeTransidFieldSourceIntoMeta(this.metaData, fieldsRaw);
                            }
                        } catch (error) {
                            this.logFilterDebug("fetchEntityFieldSourceMeta:tstructParseError", {
                                transId: effectiveTransId,
                                message: error?.message || "Unable to parse Fields"
                            });
                        }
                        resolve(true);
                    },
                    error: () => resolve(false)
                });
            }));
        }

        return Promise.all(jobs).then(() => {
            this.logFilterDebug("fetchEntityFieldSourceMeta:done", {
                transId: effectiveTransId,
                hasGapAfter: this.hasDropdownSourceGap(this.metaData)
            });
            return this.metaData;
        });
    }

    fetchAdsMetadataFromAxList(adsName, options = {}) {
        const caller = this.getAxListCaller();
        if (!caller || typeof caller.GetDataFromAxList !== "function") {
            return Promise.reject(new Error("GetDataFromAxList is not available."));
        }

        const effectiveAdsName = `${adsName || ""}`.trim();
        const adsCacheKey = this.normalizeUrlParam(effectiveAdsName);
        if (!options.forceRefresh && Array.isArray(this.adsMetaDataCache[adsCacheKey]) && this.adsMetaDataCache[adsCacheKey].length > 0) {
            return Promise.resolve(this.adsMetaDataCache[adsCacheKey]);
        }

        const requestParams = {
            adsNames: ["ds_smartlist_ads_metadata"],
            refreshCache: !!options.forceRefresh,
            sqlParams: { adsname: effectiveAdsName },
            props: {
                ADS: false,
                CachePermissions: true,
                getallrecordscount: false,
                pageno: 1,
                pagesize: 500,
                sorting: [],
                filters: []
            }
        };

        return new Promise((resolve, reject) => {
            const onSuccess = (response) => {
                try {
                    const parsedResponse = this.parseAxListResponsePayload(response);
                    const extracted = this.extractAdsRowsFromAxResponse(parsedResponse);
                    let metadata = this.normalizeAdsMetaRows(extracted.rows, effectiveAdsName);

                    if ((!Array.isArray(metadata) || metadata.length === 0) && Array.isArray(extracted.columns) && extracted.columns.length > 0) {
                        metadata = this.buildAdsMetaData(effectiveAdsName, extracted.columns, []);
                    }

                    if (Array.isArray(metadata) && metadata.length > 0) {
                        this.adsMetaDataCache[adsCacheKey] = metadata;
                    }

                    this.logFilterDebug("fetchAdsMetadata:serverResponse", {
                        adsName: effectiveAdsName,
                        metaRows: extracted.rows?.length || 0,
                        metaDataCount: Array.isArray(metadata) ? metadata.length : 0,
                        sample: Array.isArray(metadata) ? metadata.slice(0, 2) : []
                    });

                    resolve(Array.isArray(metadata) ? metadata : []);
                } catch (error) {
                    reject(error);
                }
            };

            const onError = (error) => reject(error || new Error("Unknown error while loading ADS metadata."));

            try {
                caller.GetDataFromAxList(requestParams, onSuccess, onError);
            } catch (error) {
                reject(error);
            }
        });
    }

    fetchAdsRowsFromAxList(adsName, options = {}) {
        const caller = this.getAxListCaller();
        if (!caller || typeof caller.GetDataFromAxList !== "function") {
            return Promise.reject(new Error("GetDataFromAxList is not available."));
        }

        const effectiveAdsName = `${adsName || ""}`.trim();
        const adsCacheKey = this.normalizeUrlParam(effectiveAdsName);
        const filterPayload = Array.isArray(options.filterPayload) ? options.filterPayload : [];
        const requestedPageSize = Number(options.pageSize);
        const pageSize = Number.isFinite(requestedPageSize) && requestedPageSize > 0 ? requestedPageSize : 5000;
        const requestParams = {
            adsNames: [effectiveAdsName],
            refreshCache: !!options.forceRefresh,
            sqlParams: options.sqlParams || {},
            props: {
                ADS: false,
                CachePermissions: true,
                getallrecordscount: false,
                pageno: 1,
                pagesize: pageSize,
                keyfield: "",
                keyvalue: "",
                sorting: [],
                filters: filterPayload
            }
        };

        return new Promise((resolve, reject) => {
            const onSuccess = (response) => {
                try {
                    const parsedResponse = this.parseAxListResponsePayload(response);
                    const extracted = this.extractAdsRowsFromAxResponse(parsedResponse);
                    let adsMetaData = Array.isArray(this.adsMetaDataCache[adsCacheKey]) ? this.adsMetaDataCache[adsCacheKey] : [];
                    if (!adsMetaData.length) {
                        adsMetaData = this.buildAdsMetaData(effectiveAdsName, extracted.columns, extracted.rows);
                    }
                    if (adsMetaData.length > 0) {
                        this.adsMetaDataCache[adsCacheKey] = adsMetaData;
                    }
                    resolve({
                        rows: extracted.rows,
                        metaData: adsMetaData,
                        raw: parsedResponse
                    });
                } catch (error) {
                    reject(error);
                }
            };

            const onError = (error) => reject(error || new Error("Unknown error while loading ADS data."));

            try {
                caller.GetDataFromAxList(requestParams, onSuccess, onError);
            } catch (error) {
                reject(error);
            }
        });
    }

    buildAdsEntityCachePayload(adsName, metaData) {
        return {
            result: {
                message: "success",
                data: {
                    TransId: adsName,
                    EntityName: adsName,
                    MetaData: Array.isArray(metaData) ? metaData : [],
                    SelectedEntities: adsName,
                    SelectedEntitiesList: [{ name: adsName, caption: adsName }],
                    AllEntitiesList: [],
                    Properties: {
                        XAXISFIELDS: this.xAxisFields || "All",
                        YAXISFIELDS: this.yAxisFields || "All",
                        CHARTTYPE: this.selectedChartType || "line",
                        GLOBALAGGFIELD: this.globalAggField || "count"
                    }
                }
            }
        };
    }

    fetchAdsData(adsName, callback, options = {}) {
        const effectiveAdsName = `${adsName || ""}`.trim();
        if (!effectiveAdsName) {
            if (typeof callback === "function") {
                callback();
            }
            return;
        }

        const forceRefresh = !!options.forceRefresh;
        const applyMetaAndContinue = (metaData) => {
            this.metaData = Array.isArray(metaData) ? metaData : [];
            this.entityTransId = effectiveAdsName;
            this.entityName = effectiveAdsName;
            this.selectedChartType = this.selectedChartType || "line";
            this.sanitizeAxisFieldsAgainstMeta(this.metaData);

            this.writeAnalyticsEntityCache(effectiveAdsName, this.buildAdsEntityCachePayload(effectiveAdsName, this.metaData));

            this.loadStoredAnalyticsPreferences(effectiveAdsName, () => {
                this.constructXandYAxis(this.metaData, "Page Load");
                this.constructSelectedEntityHeader();
                this.updateChartButton(this.selectedChartType);
                this.openFieldSelectionOnPageLoad();
                if (typeof callback === "function") {
                    callback();
                }
            }, { forceRefresh: forceRefresh });
        };

        if (!forceRefresh) {
            const cachedEntity = this.readAnalyticsEntityCache(effectiveAdsName);
            const cachedMeta = cachedEntity?.result?.data?.MetaData;
            if (Array.isArray(cachedMeta) && cachedMeta.length > 0) {
                applyMetaAndContinue(cachedMeta);
                return;
            }
        }

        Promise.all([
            this.fetchAdsMetadataFromAxList(effectiveAdsName, { forceRefresh: forceRefresh }).catch(() => []),
            this.fetchAdsRowsFromAxList(effectiveAdsName, {
                forceRefresh: forceRefresh,
                pageSize: 1,
                filterPayload: this.getEntityListFilterPayload()
            }).catch(() => ({ rows: [], metaData: [] }))
        ]).then(([metaFromMetaAds, rowResponse]) => {
            const rows = Array.isArray(rowResponse?.rows) ? rowResponse.rows : [];
            const rowDerivedMeta = Array.isArray(rowResponse?.metaData) ? rowResponse.metaData : [];
            const resolvedMetaData = Array.isArray(metaFromMetaAds) && metaFromMetaAds.length > 0
                ? metaFromMetaAds
                : (rowDerivedMeta.length > 0 ? rowDerivedMeta : this.buildAdsMetaData(effectiveAdsName, [], rows));

            this.logFilterDebug("fetchAdsData:resolvedMetaData", {
                adsName: effectiveAdsName,
                metaFromAdsMetaCount: Array.isArray(metaFromMetaAds) ? metaFromMetaAds.length : 0,
                rowDerivedMetaCount: rowDerivedMeta.length,
                resolvedMetaCount: resolvedMetaData.length
            });
            applyMetaAndContinue(resolvedMetaData);
        }).catch(error => {
            console.error("Error loading ADS data for analytics:", error);
            applyMetaAndContinue([]);
        });
    }

    fetchEntityListRows(transId, options = {}) {
        const effectiveTransId = transId || this.entityTransId;
        if (_entityCommon.inValid(effectiveTransId)) {
            return Promise.resolve([]);
        }

        const forceRefresh = !!options.forceRefresh;
        const requestedFields = this.getEntityListRequestFieldsString();
        const cacheKey = this.getEntityListCacheKey(effectiveTransId, requestedFields);
        if (!forceRefresh && Object.prototype.hasOwnProperty.call(this.entityListDataCache, cacheKey)) {
            this.logFilterDebug("fetchEntityListRows:memoryCacheHit", {
                transId: effectiveTransId,
                requestedFields: requestedFields,
                rows: Array.isArray(this.entityListDataCache[cacheKey]) ? this.entityListDataCache[cacheKey].length : 0
            });
            return Promise.resolve(this.entityListDataCache[cacheKey]);
        }

        if (!forceRefresh) {
            const cachedRows = this.readEntityListRowsCache(effectiveTransId, cacheKey);
            if (Array.isArray(cachedRows)) {
                this.entityListDataCache[cacheKey] = cachedRows;
                this.logFilterDebug("fetchEntityListRows:localStorageCacheHit", {
                    transId: effectiveTransId,
                    requestedFields: requestedFields,
                    rows: cachedRows.length
                });
                return Promise.resolve(cachedRows);
            }
        }

        if (!forceRefresh && this.entityListDataRequests[cacheKey]) {
            return this.entityListDataRequests[cacheKey];
        }

        const requestPromise = (async () => {
            if (!forceRefresh) {
                const indexedDbRows = await this.readEntityListRowsCacheIndexedDb(effectiveTransId, cacheKey);
                if (Array.isArray(indexedDbRows)) {
                    this.entityListDataCache[cacheKey] = indexedDbRows;
                    this.logFilterDebug("fetchEntityListRows:indexedDbCacheHit", {
                        transId: effectiveTransId,
                        requestedFields: requestedFields,
                        rows: indexedDbRows.length
                    });
                    return indexedDbRows;
                }
            }

            this.logFilterDebug("fetchEntityListRows:serverRequest", {
                transId: effectiveTransId,
                forceRefresh: forceRefresh,
                requestedFields: requestedFields,
                filterPayload: this.getEntityListFilterPayload()
            });

            if (this.isAdsMode()) {
                const requestedPageSize = Number(options.pageSize);
                const adsPageSize = Number.isFinite(requestedPageSize) && requestedPageSize > 0 ? requestedPageSize : 5000;
                const adsMetaCacheKey = this.normalizeUrlParam(effectiveTransId);
                const hasCachedAdsMeta = Array.isArray(this.adsMetaDataCache[adsMetaCacheKey]) && this.adsMetaDataCache[adsMetaCacheKey].length > 0;
                if (forceRefresh || !hasCachedAdsMeta) {
                    try {
                        await this.fetchAdsMetadataFromAxList(effectiveTransId, { forceRefresh: forceRefresh });
                    } catch (metaError) {
                        this.logFilterDebug("fetchEntityListRows:adsMetadataError", {
                            transId: effectiveTransId,
                            message: metaError?.message || "Unable to load ADS metadata"
                        });
                    }
                }

                const adsResponse = await this.fetchAdsRowsFromAxList(effectiveTransId, {
                    forceRefresh: forceRefresh,
                    pageSize: adsPageSize,
                    filterPayload: this.getEntityListFilterPayload()
                });

                const adsRows = Array.isArray(adsResponse?.rows) ? adsResponse.rows : [];
                const adsMetaData = Array.isArray(adsResponse?.metaData) ? adsResponse.metaData : [];
                if (adsMetaData.length > 0) {
                    this.metaData = adsMetaData;
                }

                this.entityListDataCache[cacheKey] = adsRows;
                this.writeEntityListRowsCache(effectiveTransId, cacheKey, this.entityListDataCache[cacheKey]);
                this.writeEntityListRowsCacheIndexedDb(effectiveTransId, cacheKey, this.entityListDataCache[cacheKey]);
                this.logFilterDebug("fetchEntityListRows:serverResponse", {
                    transId: effectiveTransId,
                    sourceType: "ads",
                    requestedFields: requestedFields,
                    pageSize: adsPageSize,
                    rows: this.entityListDataCache[cacheKey].length,
                    sample: this.entityListDataCache[cacheKey].slice(0, 2)
                });
                return this.entityListDataCache[cacheKey];
            }

            return await new Promise((resolve, reject) => {
                $.ajax({
                    url: `${this.getAssetBaseUrl()}/aspx/Entity.aspx/GetEntityListDataWS`,
                    type: 'POST',
                    cache: false,
                    async: true,
                    dataType: 'json',
                    data: JSON.stringify({
                        transId: effectiveTransId,
                        fields: requestedFields,
                        pageNo: 1,
                        pageSize: 0,
                        filter: this.getEntityListFilterPayload()
                    }),
                    contentType: "application/json",
                    success: (data) => {
                        try {
                            const parsedResponse = typeof data?.d === "string" ? JSON.parse(data.d) : (data?.d || data);
                            if (!this.isEntityListResponseSuccess(parsedResponse)) {
                                this.entityListDataCache[cacheKey] = [];
                                resolve([]);
                                return;
                            }

                            const rows = this.extractEntityListRows(parsedResponse);

                            this.entityListDataCache[cacheKey] = Array.isArray(rows) ? rows : [];
                            this.writeEntityListRowsCache(effectiveTransId, cacheKey, this.entityListDataCache[cacheKey]);
                            this.writeEntityListRowsCacheIndexedDb(effectiveTransId, cacheKey, this.entityListDataCache[cacheKey]);
                            this.logFilterDebug("fetchEntityListRows:serverResponse", {
                                transId: effectiveTransId,
                                sourceType: "tstruct",
                                requestedFields: requestedFields,
                                rows: this.entityListDataCache[cacheKey].length,
                                sample: this.entityListDataCache[cacheKey].slice(0, 2)
                            });
                            resolve(this.entityListDataCache[cacheKey]);
                        } catch (error) {
                            reject(error);
                        }
                    },
                    error: (error) => {
                        reject(error);
                    }
                });
            });
        })();

        this.entityListDataRequests[cacheKey] = requestPromise;
        return requestPromise.finally(() => {
            delete this.entityListDataRequests[cacheKey];
        });
    }

    isEntityListResponseSuccess(parsedResponse) {
        if (parsedResponse?.result?.success === true || parsedResponse?.success === true) {
            return true;
        }

        const message = `${parsedResponse?.result?.message || parsedResponse?.message || ""}`.trim().toLowerCase();
        return message === "success";
    }

    extractEntityListRows(parsedResponse) {
        const resultNode = parsedResponse?.result || parsedResponse || {};
        const dataNode = resultNode?.data || {};
        const candidates = [
            dataNode?.ListData,
            dataNode?.listData,
            resultNode?.list,
            resultNode?.ListData,
            dataNode?.rows,
            resultNode?.rows,
            dataNode?.data_json,
            resultNode?.data_json
        ];

        let rawPayload = null;
        for (let index = 0; index < candidates.length; index++) {
            const candidate = candidates[index];
            if (_entityCommon.inValid(candidate)) {
                continue;
            }

            if (Array.isArray(candidate)) {
                if (!candidate.length) {
                    rawPayload = [];
                    break;
                }

                const firstItem = candidate[0];
                if (firstItem && typeof firstItem === "object" && Object.prototype.hasOwnProperty.call(firstItem, "data_json")) {
                    rawPayload = firstItem.data_json;
                } else {
                    rawPayload = candidate;
                }
                break;
            }

            rawPayload = candidate;
            break;
        }

        return this.normalizeEntityRowsPayload(rawPayload);
    }

    normalizeEntityRowsPayload(rawPayload) {
        let payload = rawPayload;

        for (let parseCount = 0; parseCount < 4; parseCount++) {
            if (typeof payload !== "string") {
                break;
            }

            const trimmed = payload.trim();
            if (!trimmed) {
                return [];
            }

            try {
                payload = JSON.parse(trimmed);
            } catch (error) {
                break;
            }
        }

        if (Array.isArray(payload)) {
            return payload.filter(item => item && typeof item === "object");
        }

        if (!payload || typeof payload !== "object") {
            return [];
        }

        if (Object.prototype.hasOwnProperty.call(payload, "data_json")) {
            return this.normalizeEntityRowsPayload(payload.data_json);
        }

        if (Array.isArray(payload.rows)) {
            return payload.rows.filter(item => item && typeof item === "object");
        }

        if (Array.isArray(payload.list)) {
            return payload.list.filter(item => item && typeof item === "object");
        }

        const objectValues = Object.values(payload).filter(item => item && typeof item === "object");
        if (objectValues.length) {
            return objectValues;
        }

        return [];
    }

    getNormalizedFieldKey(fieldName) {
        return `${fieldName || ""}`.replace(/[^a-z0-9]/gi, "").toLowerCase();
    }

    getPrimitiveFieldValue(value) {
        if (value === null || value === undefined) {
            return value;
        }

        if (typeof value !== "object" || value instanceof Date) {
            return value;
        }

        if (Object.prototype.hasOwnProperty.call(value, "value")) {
            return value.value;
        }

        if (Object.prototype.hasOwnProperty.call(value, "text")) {
            return value.text;
        }

        if (Object.prototype.hasOwnProperty.call(value, "label")) {
            return value.label;
        }

        return value;
    }

    getRowFieldValue(row, fieldName) {
        if (!row || typeof row !== "object" || _entityCommon.inValid(fieldName)) {
            return null;
        }

        const normalizedFieldName = this.normalizeUrlParam(fieldName);
        const fieldCandidates = [fieldName];
        const fieldMeta = Array.isArray(this.metaData)
            ? this.metaData.find(item => this.normalizeUrlParam(item?.fldname || "") === normalizedFieldName)
            : null;
        const aliasMap = {
            "username": ["modifiedby"],
            "modifiedby": ["username"]
        };

        if (fieldMeta) {
            [fieldMeta.fldname, fieldMeta.normalized, fieldMeta.srcfield].forEach(candidate => {
                if (!_entityCommon.inValid(candidate)) {
                    fieldCandidates.push(candidate);
                }
            });
        }

        if (normalizedFieldName && !normalizedFieldName.endsWith("name")) {
            fieldCandidates.push(`${fieldName}name`);
            fieldCandidates.push(`${fieldName}_name`);
        }

        if (normalizedFieldName.endsWith("id")) {
            const withoutId = fieldName.substring(0, Math.max(fieldName.length - 2, 0));
            if (!_entityCommon.inValid(withoutId)) {
                fieldCandidates.push(`${withoutId}name`);
                fieldCandidates.push(`${withoutId}_name`);
            }
        }

        (aliasMap[normalizedFieldName] || []).forEach(alias => fieldCandidates.push(alias));

        for (let index = 0; index < fieldCandidates.length; index++) {
            const currentField = fieldCandidates[index];
            if (Object.prototype.hasOwnProperty.call(row, currentField)) {
                return this.getPrimitiveFieldValue(row[currentField]);
            }

            const exactKey = Object.keys(row).find(key => this.normalizeUrlParam(key) === this.normalizeUrlParam(currentField));
            if (exactKey) {
                return this.getPrimitiveFieldValue(row[exactKey]);
            }

            const normalizedKey = Object.keys(row).find(key => this.getNormalizedFieldKey(key) === this.getNormalizedFieldKey(currentField));
            if (normalizedKey) {
                return this.getPrimitiveFieldValue(row[normalizedKey]);
            }

            const normalizedField = this.getNormalizedFieldKey(currentField);
            if (normalizedField) {
                const suffixMatchKey = Object.keys(row).find(key => {
                    const normalizedRowKey = this.getNormalizedFieldKey(key);
                    return normalizedRowKey.endsWith(normalizedField);
                });
                if (suffixMatchKey) {
                    return this.getPrimitiveFieldValue(row[suffixMatchKey]);
                }
            }
        }

        return null;
    }

    parseMetricValue(value) {
        if (value === null || value === undefined) {
            return null;
        }

        if (typeof value === "number") {
            return isNaN(value) ? null : value;
        }

        if (typeof value === "string") {
            const cleanedValue = value.replace(/,/g, "").trim();
            if (cleanedValue === "") {
                return null;
            }

            const parsedValue = Number(cleanedValue);
            return isNaN(parsedValue) ? null : parsedValue;
        }

        const parsedValue = Number(value);
        return isNaN(parsedValue) ? null : parsedValue;
    }

    getAnalyticsCriteriaString(input) {
        const aggFunc = this.normalizeUrlParam(input?.aggFunc || "count") || "count";
        const groupField = this.normalizeUrlParam(input?.groupField || "") === "all" ? "" : (input?.groupField || "");
        const aggField = aggFunc === "count" ? "count" : (input?.aggField || "");
        const transId = this.entityTransId || input?.transId || "";

        return [aggFunc, transId, groupField, "", "", "", "", "", "", aggField].join("~");
    }

    formatAnalyticsGroupValue(value, fieldName) {
        if (value === null || value === undefined) {
            return "";
        }

        if (this.isDateField(fieldName)) {
            const parsedDate = value instanceof Date ? value : this.parseChartDateValue(value);
            if (parsedDate) {
                return this.formatDateByGlobalSetting(parsedDate, false);
            }
        }

        return `${value}`.trim();
    }

    getPreferredAnalyticsDateField() {
        if (this.activeDateField) {
            return this.activeDateField;
        }

        if (Array.isArray(this.metaData)) {
            const priorityFields = ["modifiedon", "createdon"];
            for (let index = 0; index < priorityFields.length; index++) {
                const matchedField = this.metaData.find(item => this.normalizeUrlParam(item.fldname) === priorityFields[index]);
                if (matchedField) {
                    return matchedField.fldname;
                }
            }

            const dateField = this.metaData.find(item => item.fdatatype === "d" && item.hide !== "T");
            if (dateField) {
                return dateField.fldname;
            }
        }

        return "modifiedon";
    }

    getRowDateValue(row, fieldName) {
        const dateCandidates = [];
        const normalizedFieldName = this.normalizeUrlParam(fieldName || "");

        if (normalizedFieldName) {
            dateCandidates.push(fieldName);
        }

        const preferredDateField = this.getPreferredAnalyticsDateField();
        if (preferredDateField && !dateCandidates.some(item => this.normalizeUrlParam(item) === this.normalizeUrlParam(preferredDateField))) {
            dateCandidates.push(preferredDateField);
        }

        ["modifiedon", "createdon"].forEach(candidate => {
            if (!dateCandidates.some(item => this.normalizeUrlParam(item) === candidate)) {
                dateCandidates.push(candidate);
            }
        });

        for (let index = 0; index < dateCandidates.length; index++) {
            const rawValue = this.getRowFieldValue(row, dateCandidates[index]);
            const parsedDate = this.parseChartDateValue(rawValue);
            if (parsedDate) {
                return parsedDate;
            }
        }

        return null;
    }

    applyDateRangeFilterToRows(rows, input) {
        const safeRows = Array.isArray(rows) ? rows : [];
        if (!safeRows.length || !input) {
            return safeRows;
        }

        const groupField = this.normalizeUrlParam(input.groupField || "");
        if (!groupField || groupField === "all" || !this.isDateField(groupField)) {
            return safeRows;
        }

        const dateRange = this.getDateRangeBySelection();
        if (!dateRange) {
            return safeRows;
        }

        return safeRows.filter(row => {
            const rowDate = this.getRowDateValue(row, input.groupField);
            return rowDate && rowDate >= dateRange.from && rowDate <= dateRange.to;
        });
    }

    buildGeneralAnalyticsData(rows) {
        const safeRows = Array.isArray(rows) ? rows : [];
        const now = new Date();
        const startOfToday = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0);
        const startOfTomorrow = new Date(startOfToday);
        startOfTomorrow.setDate(startOfTomorrow.getDate() + 1);
        const startOfYesterday = new Date(startOfToday);
        startOfYesterday.setDate(startOfYesterday.getDate() - 1);
        const startOfWeek = new Date(startOfToday);
        const dayOfWeek = startOfWeek.getDay();
        const weekOffset = dayOfWeek === 0 ? -6 : 1 - dayOfWeek;
        startOfWeek.setDate(startOfWeek.getDate() + weekOffset);
        const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1, 0, 0, 0, 0);
        const startOfYear = new Date(now.getFullYear(), 0, 1, 0, 0, 0, 0);
        const dateField = this.getPreferredAnalyticsDateField();

        const summary = {
            totrec: safeRows.length,
            cyear: 0,
            cmonth: 0,
            cweek: 0,
            cyesterday: 0,
            ctoday: 0
        };

        safeRows.forEach(row => {
            const rowDate = this.getRowDateValue(row, dateField);
            if (!rowDate) {
                return;
            }

            if (rowDate >= startOfYear && rowDate < startOfTomorrow) {
                summary.cyear += 1;
            }
            if (rowDate >= startOfMonth && rowDate < startOfTomorrow) {
                summary.cmonth += 1;
            }
            if (rowDate >= startOfWeek && rowDate < startOfTomorrow) {
                summary.cweek += 1;
            }
            if (rowDate >= startOfYesterday && rowDate < startOfToday) {
                summary.cyesterday += 1;
            }
            if (rowDate >= startOfToday && rowDate < startOfTomorrow) {
                summary.ctoday += 1;
            }
        });

        return [summary];
    }

    buildGroupedAnalyticsData(rows, input) {
        const safeRows = Array.isArray(rows) ? rows : [];
        const groupField = input?.groupField || "";
        const aggFunc = this.normalizeUrlParam(input?.aggFunc || "count") || "count";
        const isDateGroup = this.isDateField(groupField);
        const criteria = this.getAnalyticsCriteriaString(input);
        const groupedData = new Map();

        safeRows.forEach(row => {
            const rowDate = isDateGroup ? this.getRowDateValue(row, groupField) : null;
            if (isDateGroup && !rowDate) {
                return;
            }

            const displayValueRaw = isDateGroup
                ? this.formatAnalyticsGroupValue(rowDate, groupField)
                : this.formatAnalyticsGroupValue(this.getRowFieldValue(row, groupField), groupField);
            const displayValue = displayValueRaw === "" ? "NA" : displayValueRaw;

            if (!groupedData.has(displayValue)) {
                groupedData.set(displayValue, {
                    keyname: displayValue,
                    keyvalue: 0,
                    criteria: criteria,
                    _sum: 0,
                    _count: 0,
                    _hasMetricValue: aggFunc === "count",
                    _sortValue: rowDate ? new Date(rowDate.getFullYear(), rowDate.getMonth(), rowDate.getDate()).getTime() : null
                });
            }

            const currentItem = groupedData.get(displayValue);
            if (aggFunc === "count") {
                currentItem.keyvalue += 1;
                currentItem._count += 1;
                return;
            }

            const metricValue = this.parseMetricValue(this.getRowFieldValue(row, input.aggField));
            if (metricValue === null) {
                return;
            }

            currentItem._sum += metricValue;
            currentItem._count += 1;
            currentItem._hasMetricValue = true;
        });

        let results = Array.from(groupedData.values()).filter(item => aggFunc === "count" || item._hasMetricValue);
        if (isDateGroup) {
            results = results.sort((left, right) => (left._sortValue || 0) - (right._sortValue || 0));
        }

        return results.map(item => {
            const keyvalue = aggFunc === "avg"
                ? (item._count > 0 ? Number((item._sum / item._count).toFixed(this.resolveGlobalDecimalPlaces())) : 0)
                : (aggFunc === "sum" ? item._sum : item.keyvalue);

            return {
                keyname: item.keyname,
                keyvalue: keyvalue,
                criteria: item.criteria
            };
        });
    }

    buildUngroupedAnalyticsData(rows, input) {
        const safeRows = Array.isArray(rows) ? rows : [];
        const aggFunc = this.normalizeUrlParam(input?.aggFunc || "count") || "count";

        if (aggFunc === "count") {
            return this.buildGeneralAnalyticsData(safeRows);
        }

        let sum = 0;
        let count = 0;

        safeRows.forEach(row => {
            const metricValue = this.parseMetricValue(this.getRowFieldValue(row, input?.aggField));
            if (metricValue === null) {
                return;
            }

            sum += metricValue;
            count += 1;
        });

        if (count === 0) {
            return [];
        }

        return [{
            keyname: "",
            keyvalue: aggFunc === "avg" ? Number((sum / count).toFixed(this.resolveGlobalDecimalPlaces())) : sum,
            criteria: this.getAnalyticsCriteriaString(input)
        }];
    }

    buildAnalyticsDataFromRows(rows, input) {
        const safeRows = Array.isArray(rows) ? rows : [];
        const groupField = this.normalizeUrlParam(input?.groupField || "");
        const aggFunc = this.normalizeUrlParam(input?.aggFunc || "count") || "count";
        const aggField = this.normalizeUrlParam(input?.aggField || "count") || "count";

        if ((groupField === "" || groupField === "all") && aggFunc === "count" && aggField === "count") {
            return this.buildGeneralAnalyticsData(safeRows);
        }

        const filteredRows = this.applyDateRangeFilterToRows(safeRows, input);
        if (groupField === "" || groupField === "all") {
            return this.buildUngroupedAnalyticsData(filteredRows, input);
        }

        return this.buildGroupedAnalyticsData(filteredRows, input);
    }

    fetchAnalyticsRawData(input) {
        const safeInput = input || {};
        return this.fetchEntityListRows(this.entityTransId).then(rows => {
            const safeRows = Array.isArray(rows) ? rows : [];
            const chartsData = this.buildAnalyticsDataFromRows(safeRows, safeInput);
            this.logFilterDebug("fetchAnalyticsRawData:rowsToCharts", {
                groupField: safeInput.groupField,
                aggField: safeInput.aggField,
                aggFunc: safeInput.aggFunc,
                totalRows: safeRows.length,
                chartRows: Array.isArray(chartsData) ? chartsData.length : 0
            });
            return this.getGridSelectionAwareChartData(Array.isArray(chartsData) ? chartsData : [], safeInput);
        }).catch(error => {
            console.error("Error building analytics data from rows:", error);
            return [];
        });
    }

    normalizeNumericValue(value) {
        if (typeof value === "number") {
            return value;
        }

        if (typeof value === "string") {
            const cleanedValue = value.replace(/,/g, "").trim();
            const parsedValue = Number(cleanedValue);
            return isNaN(parsedValue) ? 0 : parsedValue;
        }

        const parsedValue = Number(value);
        return isNaN(parsedValue) ? 0 : parsedValue;
    }

    buildGridPointLink(payload) {
        try {
            return `__analytics_grid_link__:${encodeURIComponent(JSON.stringify(payload || {}))}`;
        } catch (error) {
            return "";
        }
    }

    parseGridPointLink(linkData) {
        const linkPrefix = "__analytics_grid_link__:";
        if (_entityCommon.inValid(linkData) || typeof linkData !== "string" || !linkData.startsWith(linkPrefix)) {
            return null;
        }

        try {
            return JSON.parse(decodeURIComponent(linkData.substring(linkPrefix.length)));
        } catch (error) {
            return null;
        }
    }

    buildGridChartPoints(chartsData, input, groupCaption) {
        if (!Array.isArray(chartsData) || chartsData.length === 0) {
            return [];
        }

        if (this.normalizeUrlParam(input.aggField) === "count" && this.normalizeUrlParam(input.groupField) === "all") {
            const kpiJson = chartsData[0] || {};
            const map = {
                "totrec": "Total records",
                "cyear": "This year",
                "cmonth": "This month",
                "cweek": "This week",
                "cyesterday": "Yesterday",
                "ctoday": "Today"
            };
            const periodMap = {
                "totrec": "total",
                "cyear": "this_yearOption",
                "cmonth": "this_monthOption",
                "cweek": "this_weekOption",
                "cyesterday": "yesterdayOption",
                "ctoday": "todayOption"
            };

            return Object.keys(map).filter(key => kpiJson[key] !== null && kpiJson[key] !== undefined).map(key => {
                const periodLink = this.buildGridPointLink({
                    type: "period",
                    period: periodMap[key] || "total"
                });
                return {
                    label: map[key],
                    value: this.normalizeNumericValue(kpiJson[key]),
                    link: periodLink,
                    navigateLink: periodLink
                };
            });
        }

        return chartsData.map(item => {
            const criteriaItems = `${item.criteria || ""}`.split("~");
            const transid = (criteriaItems[1] || input.transId || this.entityTransId || "").trim();
            const fldname = (criteriaItems[2] || input.groupField || "").trim();
            const keyname = (item.keyname || groupCaption || "Empty data").toString().trim();

            const navLink = this.buildGridPointLink({
                type: "list",
                transid: transid,
                fldname: fldname,
                keyname: keyname
            });

            return {
                label: keyname,
                value: this.normalizeNumericValue(item.keyvalue),
                link: navLink,
                navigateLink: navLink
            };
        }).filter(item => item.label !== "");
    }

    showGridCardMessage(cardElement, message, cssClass) {
        if (!cardElement) {
            return;
        }
        const chartDiv = cardElement.querySelector(".analytics-grid-chart");
        if (!chartDiv) {
            return;
        }
        chartDiv.innerHTML = `<div class="analytics-grid-message ${cssClass || ""}">${this.escapeHtml(message)}</div>`;
    }

    getGridChartRenderPayload(chartType, aggLabel, points) {
        const normalizedType = this.getChartTypeConfig(chartType).type;
        let data = [];
        let metaData = [];

        if (normalizedType === "pie" || normalizedType === "donut" || normalizedType === "semi-donut") {
            data = points.map(item => [item.label, item.value, item.link || ""]);
            metaData = [{ name: "data_label" }, { name: "value" }, { name: "link" }];
        } else if (normalizedType === "funnel") {
            data = points.map(item => [item.label, item.value, item.link || ""]);
            metaData = [{ name: "data_label" }, { name: "value" }, { name: "link" }];
        } else {
            const seriesName = aggLabel || "Value";
            data = points.map(item => [seriesName, item.label, item.value, item.link || ""]);
            metaData = [{ name: "data_label" }, { name: "x_axis" }, { name: "value" }, { name: "link" }];
        }

        return {
            type: normalizedType,
            data: data,
            metaData: metaData
        };
    }

    renderChartInGridCard(cardElement, chartType, aggLabel, points, paletteConfig) {
        const chartDiv = cardElement.querySelector(".analytics-grid-chart");
        if (!chartDiv) {
            return;
        }

        if (!chartDiv.id) {
            chartDiv.id = `analytics-grid-chart-${Date.now()}`;
        }

        const target = `#${chartDiv.id}`;
        const renderPayload = this.getGridChartRenderPayload(chartType, aggLabel, points);
        const resolvedPalette = this.resolveGridPaletteConfig(paletteConfig);
        const chartAttr = {
            cck: resolvedPalette.cck,
            showBarDataLabels: false
        };
        if (resolvedPalette.cccv) {
            chartAttr.cccv = resolvedPalette.cccv;
        }

        chartDiv.innerHTML = "";
        createAgileChart({
            target: target,
            type: renderPayload.type,
            data: renderPayload.data,
            metaData: renderPayload.metaData,
            height: 220,
            disableExporting: true,
            attr: chartAttr
        });
        this.applyChartNumericFormattingWithRetry(chartDiv.id, chartType);

        setTimeout(() => {
            this.forceHideGridBarDataLabels(cardElement, chartType);
            this.applyGridCardSelectionHighlight(cardElement);
        }, 80);
    }

    shouldHideDataLabelsForChartType(chartType) {
        const normalizedType = this.getChartTypeConfig(chartType).type;
        const barLikeTypes = new Set(["bar", "column", "stacked-bar", "stacked-column", "stacked-percentage-column"]);
        return barLikeTypes.has(normalizedType);
    }

    forceHideChartDataLabels(chart, errorLabel) {
        if (!chart) {
            return;
        }

        try {
            chart.update({
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: false
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: false
                        }
                    },
                    column: {
                        dataLabels: {
                            enabled: false
                        }
                    }
                }
            }, false);

            if (Array.isArray(chart.series)) {
                chart.series.forEach(series => {
                    if (!series || typeof series.update !== "function") {
                        return;
                    }
                    series.update({
                        dataLabels: {
                            enabled: false
                        }
                    }, false);
                });
            }

            if (typeof chart.redraw === "function") {
                chart.redraw();
            }
        } catch (error) {
            console.error(errorLabel, error);
        }
    }

    getChartInstanceByRenderToId(containerId) {
        if (!containerId || typeof Highcharts === "undefined" || !Array.isArray(Highcharts.charts)) {
            return null;
        }

        return Highcharts.charts.find(chart => chart && chart.renderTo && chart.renderTo.id === containerId) || null;
    }

    applyChartNumericFormatting(chart, chartType) {
        if (!chart || typeof chart.update !== "function") {
            return;
        }

        const hideBarLabels = this.shouldHideDataLabelsForChartType(chartType);
        const self = this;

        try {
            chart.update({
                xAxis: {
                    labels: {
                        formatter: function () {
                            const value = this && this.value;
                            if (typeof value === "number") {
                                return self.formatChartCompactValue(value, 2);
                            }
                            return value;
                        }
                    }
                },
                yAxis: {
                    labels: {
                        formatter: function () {
                            const value = this && this.value;
                            if (typeof value === "number") {
                                return self.formatChartCompactValue(value, 2);
                            }
                            return value;
                        }
                    }
                },
                tooltip: {
                    pointFormatter: function () {
                        const pointValue = typeof this.y === "number"
                            ? this.y
                            : (typeof this.value === "number" ? this.value : 0);
                        const label = this.name || this.key || this.category || this.series?.name || "Value";
                        return `<span style="color:${this.color}">\u25CF</span> ${label}: <b>${self.formatChartValue(pointValue, 2)}</b><br/>`;
                    }
                },
                plotOptions: hideBarLabels ? {
                    series: {
                        dataLabels: {
                            enabled: false
                        }
                    },
                    bar: {
                        dataLabels: {
                            enabled: false
                        }
                    },
                    column: {
                        dataLabels: {
                            enabled: false
                        }
                    }
                } : {
                    series: {
                        dataLabels: {
                            formatter: function () {
                                const pointValue = typeof this.y === "number"
                                    ? this.y
                                    : (this.point && typeof this.point.y === "number" ? this.point.y : null);
                                if (pointValue === null) {
                                    return null;
                                }
                                return self.formatChartCompactValue(pointValue, 2);
                            },
                            allowOverlap: false,
                            crop: true
                        }
                    },
                    pie: {
                        dataLabels: {
                            formatter: function () {
                                const pointValue = typeof this.y === "number" ? this.y : 0;
                                const pointName = this.point?.name || "";
                                return `${pointName}: ${self.formatChartCompactValue(pointValue, 2)}`;
                            }
                        }
                    }
                }
            }, false);

            if (typeof chart.redraw === "function") {
                chart.redraw();
            }
        } catch (error) {
            console.error("Unable to apply chart numeric formatting:", error);
        }
    }

    applyChartNumericFormattingWithRetry(chartContainerId, chartType, retries = 8) {
        const tryApply = (remaining) => {
            const chart = this.getChartInstanceByRenderToId(chartContainerId);
            if (chart) {
                this.applyChartNumericFormatting(chart, chartType);
                return;
            }

            if (remaining > 0) {
                setTimeout(() => tryApply(remaining - 1), 140);
            }
        };

        tryApply(retries);
    }

    forceHideGridBarDataLabels(cardElement, chartType) {
        if (!this.shouldHideDataLabelsForChartType(chartType)) {
            return;
        }

        const chart = this.getGridCardChartInstance(cardElement);
        this.forceHideChartDataLabels(chart, "Unable to disable bar data labels for analytics grid card:");
    }

    forceHidePopupBarDataLabels(chartContainerId, chartType) {
        if (!this.shouldHideDataLabelsForChartType(chartType)) {
            return;
        }

        const chart = this.getChartInstanceByRenderToId(chartContainerId);
        this.forceHideChartDataLabels(chart, "Unable to disable bar data labels for analytics popup chart:");
    }

    hidePopupBarDataLabelsWithRetry(chartContainerId, chartType, retries = 8) {
        if (!this.shouldHideDataLabelsForChartType(chartType)) {
            return;
        }

        const tryHide = (remaining) => {
            const chart = this.getChartInstanceByRenderToId(chartContainerId);
            if (chart) {
                this.forceHideChartDataLabels(chart, "Unable to disable bar data labels for analytics popup chart:");
                return;
            }

            if (remaining > 0) {
                setTimeout(() => tryHide(remaining - 1), 140);
            }
        };

        tryHide(retries);
    }

    applyGridCardPalette(cardElement) {
        if (!cardElement || !cardElement.__gridChartPayload) {
            return;
        }

        const payload = cardElement.__gridChartPayload;
        const paletteConfig = this.getGridCardPalette(cardElement);
        payload.paletteConfig = paletteConfig;
        this.renderChartInGridCard(cardElement, payload.chartType, payload.aggLabel, payload.points, paletteConfig);
    }

    openGridCardPopup(cardElement) {
        if (!cardElement) {
            return;
        }

        const payload = cardElement.__gridChartPayload;
        if (!payload || !Array.isArray(payload.points) || payload.points.length === 0) {
            if (typeof showAlertDialog === "function") {
                showAlertDialog("warning", "No chart data available to open.");
            }
            return;
        }

        if (typeof createPopup !== "function") {
            if (typeof showAlertDialog === "function") {
                showAlertDialog("warning", "Popup is not available.");
            }
            return;
        }

        const popupChartId = `analytics-grid-popup-chart-${Date.now()}-${Math.floor(Math.random() * 1000)}`;
        const popupListId = `analytics-grid-popup-list-${Date.now()}-${Math.floor(Math.random() * 1000)}`;
        const chartTypeName = this.getChartTypeConfig(payload.chartType).name;
        const shouldHidePopupLabels = this.shouldHideDataLabelsForChartType(payload.chartType);
        const popupTitle = `${payload.groupCaption || "Chart"} - ${payload.aggLabel || "Count"} (${chartTypeName})`;
        const popupModal = createPopup("about:blank", false);
        if (!popupModal || !popupModal.modalBody) {
            if (typeof showAlertDialog === "function") {
                showAlertDialog("warning", "Unable to open popup.");
            }
            return;
        }

        const modalBody = popupModal.modalBody;
        const iframe = modalBody.querySelector("#loadPopUpPage");
        if (iframe) {
            iframe.style.display = "none";
            iframe.setAttribute("aria-hidden", "true");
            iframe.src = "about:blank";
        }

        modalBody.classList.remove("overflow-hidden");
        modalBody.classList.add("overflow-auto");
        modalBody.querySelectorAll(".analytics-grid-popup-content").forEach(item => item.remove());
        modalBody.insertAdjacentHTML("beforeend", `<div class="analytics-grid-popup-content">
                <div class="analytics-grid-popup-title">${this.escapeHtml(popupTitle)}</div>
                <div class="analytics-grid-popup-layout">
                    <div class="analytics-grid-popup-chart-wrap">
                        <div id="${popupChartId}" class="analytics-grid-popup-chart${shouldHidePopupLabels ? " analytics-grid-popup-hide-data-labels" : ""}"></div>
                    </div>
                    <div class="analytics-grid-popup-panel">
                        <div class="analytics-grid-popup-panel-header">
                            <div class="analytics-grid-popup-panel-title">${this.escapeHtml(payload.groupCaption || "Details")}</div>
                        </div>
                        <div id="${popupListId}" class="row analytics-grid-popup-panel-list"></div>
                    </div>
                </div>
            </div>`);
        this.renderGridHyperlinkList(modalBody.querySelector(`#${popupListId}`), cardElement);

        const renderPayload = this.getGridChartRenderPayload(payload.chartType, payload.aggLabel, payload.points);
        const renderPopupChart = () => {
            try {
                createAgileChart({
                    target: `#${popupChartId}`,
                    type: renderPayload.type,
                    data: renderPayload.data,
                    metaData: renderPayload.metaData,
                    height: Math.max(window.innerHeight - 220, 360),
                    disableExporting: true,
                    attr: (() => {
                        const popupPalette = this.resolveGridPaletteConfig(payload.paletteConfig || payload.palette || "newPalette");
                        const popupAttr = {
                            cck: popupPalette.cck,
                            showBarDataLabels: false
                        };
                        if (popupPalette.cccv) {
                            popupAttr.cccv = popupPalette.cccv;
                        }
                        return popupAttr;
                    })()
                });
                this.applyChartNumericFormattingWithRetry(popupChartId, payload.chartType);
                this.hidePopupBarDataLabelsWithRetry(popupChartId, payload.chartType);
                this.reflowGridCharts();
            } catch (error) {
                console.error("Error rendering popup chart:", error);
                const chartContainer = modalBody.querySelector(`#${popupChartId}`);
                if (chartContainer) {
                    chartContainer.innerHTML = `<div class="analytics-grid-message">Unable to render chart</div>`;
                }
            }
        };

        if (popupModal.modalElement && !popupModal.modalElement.classList.contains("show")) {
            popupModal.modalElement.addEventListener("shown.bs.modal", renderPopupChart, { once: true });
        } else {
            setTimeout(renderPopupChart, 80);
        }

        if (popupModal.modalElement) {
            popupModal.modalElement.addEventListener("hide.bs.modal", () => {
                modalBody.querySelectorAll(".analytics-grid-popup-content").forEach(item => item.remove());
                modalBody.classList.remove("overflow-auto");
                modalBody.classList.add("overflow-hidden");
                if (iframe) {
                    iframe.style.display = "";
                    iframe.removeAttribute("aria-hidden");
                }
            }, { once: true });
        }
    }

    buildGridCardRequest(cardElement) {
        if (!cardElement) {
            return null;
        }

        const groupField = cardElement.getAttribute("data-group-field") || "all";
        const groupCaption = cardElement.getAttribute("data-group-caption") || "All";
        const chartSelect = cardElement.querySelector(".analytics-grid-chart-select");
        const aggField = this.getSelectedGlobalAggField();
        const chartType = chartSelect ? chartSelect.value : this.selectedChartType;
        const paletteConfig = this.getGridCardPalette(cardElement);
        const aggLabel = this.getSelectedGlobalAggLabel();
        const input = this.buildAnalyticsInput(groupField, aggField);
        const chartMeta = this.buildChartMetaFromInput(input);
        const chartMetaKey = this.getChartMetaKey(chartMeta);

        return {
            cardElement: cardElement,
            groupCaption: groupCaption,
            chartType: chartType,
            paletteConfig: paletteConfig,
            aggLabel: aggLabel,
            input: input,
            chartMeta: chartMeta,
            chartMetaKey: chartMetaKey
        };
    }

    renderGridCardFromRequest(request, chartsData, requestToken) {
        if (!request || !request.cardElement) {
            return;
        }

        const { cardElement, chartType, aggLabel, groupCaption, paletteConfig, input } = request;
        if (cardElement.getAttribute("data-request-token") !== requestToken) {
            return;
        }

        const points = this.buildGridChartPoints(chartsData, input, groupCaption);
        if (!points.length) {
            this.logFilterDebug("renderGridCardFromRequest:noPoints", {
                cardGroupField: input?.groupField || cardElement.getAttribute("data-group-field"),
                aggField: input?.aggField,
                aggFunc: input?.aggFunc,
                chartsDataLength: Array.isArray(chartsData) ? chartsData.length : 0,
                chartsDataSample: Array.isArray(chartsData) ? chartsData.slice(0, 5) : []
            });
            this.updateGridCardFilterAction(cardElement, input);
            this.showGridCardMessage(cardElement, "No data found");
            return;
        }

        cardElement.__gridChartPayload = {
            chartType: this.getChartTypeConfig(chartType).type,
            aggLabel: aggLabel,
            groupCaption: groupCaption,
            points: points.map(item => ({ ...item })),
            paletteConfig: paletteConfig
        };
        this.logFilterDebug("renderGridCardFromRequest:points", {
            cardGroupField: input?.groupField || cardElement.getAttribute("data-group-field"),
            aggField: input?.aggField,
            aggFunc: input?.aggFunc,
            pointCount: points.length,
            sample: points.slice(0, 5)
        });
        this.updateGridCardFilterAction(cardElement, input);
        this.renderChartInGridCard(cardElement, chartType, aggLabel, points, paletteConfig);
        if (this.isHyperlinkPanelVisible && cardElement.classList.contains("active")) {
            this.renderHyperlinkPanelForCard(cardElement);
        }
    }

    loadGridCard(cardElement) {
        const request = this.buildGridCardRequest(cardElement);
        if (!request) {
            return;
        }

        const { cardElement: currentCard, input, chartType, aggLabel, groupCaption, paletteConfig, chartMetaKey } = request;
        this.selectedChartType = this.getChartTypeConfig(chartType).type;
        currentCard.__gridChartPayload = null;

        const requestToken = `${Date.now()}_${Math.random()}`;
        currentCard.setAttribute("data-request-token", requestToken);
        currentCard.classList.add("loading");
        this.showGridCardMessage(currentCard, "Loading...");

        const cachedCharts = this.getCachedGridChartData(chartMetaKey, input);
        if (Array.isArray(cachedCharts)) {
            this.getGridSelectionAwareChartData(cachedCharts, input).then(chartsData => {
                this.renderGridCardFromRequest(request, chartsData, requestToken);
            }).catch(error => {
                console.error("Error loading cached analytics card data:", error);
                if (currentCard.getAttribute("data-request-token") === requestToken) {
                    this.showGridCardMessage(currentCard, "Unable to load chart");
                }
            }).finally(() => {
                if (currentCard.getAttribute("data-request-token") === requestToken) {
                    currentCard.classList.remove("loading");
                }
            });
            return;
        }

        this.fetchAnalyticsRawData(input).then(chartsData => {
            this.renderGridCardFromRequest({
                cardElement: currentCard,
                chartType: chartType,
                aggLabel: aggLabel,
                groupCaption: groupCaption,
                paletteConfig: paletteConfig,
                input: input
            }, chartsData, requestToken);
        }).catch(error => {
            if (currentCard.getAttribute("data-request-token") !== requestToken) {
                return;
            }
            console.error("Error loading analytics card data:", error);
            this.showGridCardMessage(currentCard, "Unable to load chart");
        }).finally(() => {
            if (currentCard.getAttribute("data-request-token") === requestToken) {
                currentCard.classList.remove("loading");
            }
        });
    }

    loadGridCardsBatch(cardElements, options = {}) {
        const cards = Array.isArray(cardElements) ? cardElements.filter(Boolean) : [];
        if (!cards.length) {
            return Promise.resolve([]);
        }

        const requestToken = `${Date.now()}_${Math.random()}`;

        cards.forEach(card => {
            card.__gridChartPayload = null;
            card.setAttribute("data-request-token", requestToken);
            card.classList.add("loading");
            this.showGridCardMessage(card, "Loading...");
        });

        const forceRefresh = !!options.forceRefresh;
        return this.fetchEntityListRows(this.entityTransId, { forceRefresh: forceRefresh }).then(rows => {
            const safeRows = Array.isArray(rows) ? rows : [];
            this.updateAvailableAggFieldsFromRows(safeRows);

            const requests = cards.map(card => this.buildGridCardRequest(card)).filter(Boolean);
            if (!requests.length) {
                return [];
            }

            this.logFilterDebug("loadGridCardsBatch:start", {
                requestToken: requestToken,
                forceRefresh: !!options.forceRefresh,
                activeFilter: this.gridSelectionFilter,
                cardRequests: requests.map(request => ({
                    groupField: request?.input?.groupField || "",
                    aggField: request?.input?.aggField || "",
                    aggFunc: request?.input?.aggFunc || "",
                    chartType: request?.chartType || ""
                }))
            });

            return Promise.all(requests.map((request) => {
                if (request.cardElement.getAttribute("data-request-token") !== requestToken) {
                    return Promise.resolve();
                }

                let chartsData = this.buildAnalyticsDataFromRows(safeRows, request.input);
                return this.getGridSelectionAwareChartData(Array.isArray(chartsData) ? chartsData : [], request.input, safeRows).then(finalCharts => {
                    this.logFilterDebug("loadGridCardsBatch:cardData", {
                        requestToken: requestToken,
                        cardGroupField: request?.input?.groupField || "",
                        aggField: request?.input?.aggField || "",
                        aggFunc: request?.input?.aggFunc || "",
                        baseRows: safeRows.length,
                        responseRows: Array.isArray(finalCharts) ? finalCharts.length : 0,
                        responseSample: Array.isArray(finalCharts) ? finalCharts.slice(0, 5) : []
                    });
                    this.renderGridCardFromRequest(request, finalCharts, requestToken);
                });
            })).then(() => {
                this.updateLastRefreshDateTime();
                this.saveGridCardState();
                return safeRows;
            });
        }).catch(error => {
            cards.forEach(card => {
                if (card.getAttribute("data-request-token") === requestToken) {
                    this.showGridCardMessage(card, "Unable to load chart");
                }
            });
            console.error("Error loading analytics cards data:", error);
            throw error;
        }).finally(() => {
            cards.forEach(card => {
                if (card.getAttribute("data-request-token") === requestToken) {
                    card.classList.remove("loading");
                }
            });
        });
    }

    generateCustomKPIHTML(jsonData, caption) {
        let html = '';

        let isGroupAll = document.querySelector('.Data-Group_Items.group-all.selected') !== null;

        jsonData.forEach((obj) => {
            let criterianodeResponse = obj.criteria || '';
            let criteriaItems = criterianodeResponse.split('~');
            let transid = criteriaItems[1] ? criteriaItems[1].trim() : '';
            let fldname = criteriaItems[2] ? criteriaItems[2].trim() : '';

            let subtitle = (obj.keyname || caption || '').trim();
            let keyvalue = obj.keyvalue;

            if (isGroupAll && (subtitle === '' || subtitle.toLowerCase() === 'empty data')) {
                let altSubtitle = _analyticsCharts.getChartCaptions(criterianodeResponse) || '';
                if (altSubtitle.trim() !== '') {
                    subtitle = altSubtitle.trim();
                }
            }

            if (
                (subtitle === '' || subtitle.toLowerCase() === 'empty data') &&
                (keyvalue === '' || keyvalue === null || keyvalue === undefined || keyvalue === 0)
            ) {
                return;
            }

            const subtitleHtml = transid && fldname
                ? `<h6 class="subtitle" data-keyname="${subtitle}" onclick="navigateToListPage('${transid}', '${fldname}', this)">${subtitle}</h6>`
                : `<h6 class="subtitle" data-keyname="${subtitle}">${subtitle}</h6>`;

            html += `
                <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
                    <a href="#" class="Invoice-item">
                        <div class="Invoice-icon">
                            <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                        </div>
                        <div class="Invoice-content">
                            ${subtitleHtml}
                            <h3 class="title">${_analyticsCharts.formatNumberByGlobalSetting(obj.keyvalue)}</h3>
                        </div>
                        <div class="Invoice-icon2"></div>
                    </a>
                </div>
            `;
        });

        return html;
    }


    generateGeneralKPIHTML(data) {
        let html = '';

        html += `
        <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
            <a href="#" class="Invoice-item">
                <div class="Invoice-icon">
                    <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                </div>
                <div class="Invoice-content">
                    <h6 class="subtitle" onclick="navigateToPeriod('total')">Total</h6>
                    <h3 class="title">${data.totrec || 0}</h3>
                </div>
                <div class="Invoice-icon2"></div>
            </a>
        </div>
    `;

        if (data.cyear !== 0) {
            html += `
            <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
                <a href="#" class="Invoice-item">
                    <div class="Invoice-icon">
                        <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                    </div>
                    <div class="Invoice-content">
                        <h6 class="subtitle" onclick="navigateToPeriod('this_yearOption')">This year</h6>
                        <h3 class="title">${data.cyear || 0}</h3>
                    </div>
                    <div class="Invoice-icon2"></div>
                </a>
            </div>
        `;
        }

        if (data.cmonth !== 0) {
            html += `
            <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
                <a href="#" class="Invoice-item">
                    <div class="Invoice-icon">
                        <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                    </div>
                    <div class="Invoice-content">
                        <h6 class="subtitle" onclick="navigateToPeriod('this_monthOption')">This month</h6>
                        <h3 class="title">${data.cmonth || 0}</h3>
                    </div>
                    <div class="Invoice-icon2"></div>
                </a>
            </div>
        `;
        }

        if (data.cweek !== 0) {
            html += `
            <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
                <a href="#" class="Invoice-item">
                    <div class="Invoice-icon">
                        <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                    </div>
                    <div class="Invoice-content">
                        <h6 class="subtitle" onclick="navigateToPeriod('this_weekOption')">This week</h6>
                        <h3 class="title">${data.cweek || 0}</h3>
                    </div>
                    <div class="Invoice-icon2"></div>
                </a>
            </div>
        `;
        }

        if (data.cyesterday !== 0) {
            html += `
            <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
                <a href="#" class="Invoice-item">
                    <div class="Invoice-icon">
                        <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                    </div>
                    <div class="Invoice-content">
                        <h6 class="subtitle" onclick="navigateToPeriod('yesterdayOption')">Yesterday</h6>
                        <h3 class="title">${data.cyesterday || 0}</h3>
                    </div>
                    <div class="Invoice-icon2"></div>
                </a>
            </div>
        `;
        }

        if (data.ctoday !== 0) {
            html += `
            <div class="col-xl-12 col-lg-12 col-sm-12 Invoice-content-wrap">
                <a href="#" class="Invoice-item">
                    <div class="Invoice-icon">
                        <span class="material-icons material-icons-style material-icons-2">bar_chart</span>
                    </div>
                    <div class="Invoice-content">
                        <h6 class="subtitle" onclick="navigateToPeriod('todayOption')">Today</h6>
                        <h3 class="title">${data.ctoday || 0}</h3>
                    </div>
                    <div class="Invoice-icon2"></div>
                </a>
            </div>
        `;
        }

        return html;
    }



    filterByFldname(metaData, fldname) {
        return metaData.filter(obj => obj.fldname === fldname);
    }
    getChartCriteria(criteria) {
        let parts = criteria.split("~~");
        let result = parts.slice(0, 4).join("~~");
        return result;
    }

    getChartCaptions(chart) {
        let _this = this;
        if (!_this.selectedChartsObj[chart]) {
            var chartItems = chart.split("~");
            const groupfld = this.filterByFldname(this.metaData, chartItems[2]);
            const aggFld = this.filterByFldname(this.metaData, chartItems[9]);
            const aggCondVal = chartItems[0];

            var chartStr = "";

            if (groupfld.length === 0 || chartItems[2] == "") {
                if (aggCondVal === "count") {
                    chartStr = `${this.entityName} (Count)`;
                } else {
                    chartStr = `${aggFld[0].fldcap || ''} (${aggCondVal})`;
                }
            } else {
                if (aggCondVal === "count") {
                    chartStr = `${groupfld[0].fldcap || ''} wise ${aggCondVal}`;
                } else {
                    chartStr = `${groupfld[0].fldcap || ''} wise ${aggFld[0].fldcap || ''} (${aggCondVal})`;
                }
            }

            _this.selectedChartsObj[chart] = chartStr;
        }
        return _this.selectedChartsObj[chart];
    }

    checkAppmanagerAccess() {
        if (!_entityCommon.isAppManager()) {
            document.querySelector("#Entity_Summary_Wrapper .card-header .Tkts-toolbar-Right").innerHTML = "";

        }
    }
}

$(document).ready(function () {
    _entityFilter = new EntityFilter();
    _entityCommon = new EntityCommon();
    _analyticsCharts = new AnalyticsCharts();
    _analyticsCharts.init();

    $('#searchEntity').keyup(function () {
        applyEntitySearch();
    }).change(function () {
        applyEntitySearch();
    });

    $(document).keydown(function (e) {
        if (_analyticsCharts.enableCardGridLayout) {
            return;
        }
        switch (e.which) {
            case 37: // left
                moveselection('left');
                break;
            case 38: // up
                moveselection('up');
                break;
            case 39: // right
                moveselection('right');
                break;
            case 40: // down
                moveselection('down');
                break;
            default: return;
        }
        e.preventDefault();
    });
});

function applyEntitySearch() {
    var txt = $('#searchEntity').val().trim().toLowerCase();

    if (txt === "") {
        $("#entityDataContainer .EntityData_Select-lists, #selectedEntitiesContainer .EntityData_Select-lists").removeClass("d-none");
    } else {
        // Filter in entityDataContainer
        $("#entityDataContainer .EntityData_Select-lists").addClass("d-none");
        var filteredEntityDataDivs = $("#entityDataContainer").find('.EntityData_Select-lists').filter(function () {
            return $(this).text().toLowerCase().indexOf(txt) > -1;
        });
        filteredEntityDataDivs.removeClass("d-none");

        // Filter in selectedEntitiesContainer
        $("#selectedEntitiesContainer .EntityData_Select-lists").addClass("d-none");
        var filteredSelectedEntitiesDivs = $("#selectedEntitiesContainer").find('.EntityData_Select-lists').filter(function () {
            return $(this).text().toLowerCase().indexOf(txt) > -1;
        });
        filteredSelectedEntitiesDivs.removeClass("d-none");
    }
}


function selectGroupField(anchor) {
    var parentElement = $(anchor).parent();
    $(".Data-Group_Items").removeClass("selected");
    $(parentElement).addClass("selected");

    var groupFieldName = $(anchor).find(".Data-Group-name").attr("data-fldname").toLowerCase();

    var aggFieldName = $(".Aggregation-item.selected").find(".subtitle").attr("data-fldname").toLowerCase();

    var selectedItem = _analyticsCharts.metaData.find(item => item.fldname === groupFieldName);

    var groupField = groupFieldName === "all" ? "all" : groupFieldName;
    var aggField = aggFieldName || "count";

    var aggFunc = $(".Aggregation-item.selected").find("button.bg-success").attr("agg_function") || "count";

    var transId = groupFieldName === "all" ? _analyticsCharts.entityTransId : selectedItem ? selectedItem.ftransid : null;
    _analyticsCharts.toggleDateRangeFilter(groupField);

    if (!groupField || !aggField || !transId) {
        console.error("One or more required fields are missing from the selected item.");
        return;
    }

    if (groupField === "all" || (selectedItem && selectedItem.griddc === "F")) {
        showAllXAxis();
    } else {
        constructXandYAxisForSelectedGroup(groupField);
    }

    _analyticsCharts.getAnalyticsChartsDataWS({
        page: "Analytics",
        transId: transId,
        aggField: aggField,
        aggTransId: transId,
        groupField: groupField,
        groupTransId: transId,
        aggFunc: aggFunc
    });
}




function constructXandYAxisForSelectedGroup(selectedDc) {


    document.querySelectorAll(".Aggregation-Item-wrap").forEach(aggFld => {
        aggFld.classList.add("d-none");

        let fldDcName = aggFld.dataset.dcname;

        if (fldDcName == "Count" || fldDcName == selectedDc) {
            aggFld.classList.remove("d-none");
        }
    });


    let selectedAggregation = document.querySelector(".Aggregation-item.selected");
    if (selectedAggregation.closest('.Aggregation-Item-wrap').classList.contains("d-none")) {
        selectedAggregation.classList.remove("selected");
        document.querySelector(".agg-count .Aggregation-item").classList.add("selected");
    }
}

function showAllXAxis() {
    document.querySelectorAll(".Aggregation-Item-wrap.d-none").forEach(aggFld => {
        aggFld.classList.remove("d-none");
    });

}

function selectEntityToAdd(ename, transid, imgtag) {
    var entityElem = $(imgtag).find(".EntityData_Select-Items");

    // Toggle selection state
    if (entityElem.hasClass('selected')) {
        entityElem.removeClass("selected");
        if (selectedItemsArray.includes(transid)) {
            selectedItemsArray = selectedItemsArray.filter(id => id !== transid);
        }
    } else {
        entityElem.addClass("selected");
        if (!selectedItemsArray.includes(transid)) {
            selectedItemsArray.push(transid);
        }
    }
}


function selectAggField(anchor, chartType) {
    $(".Aggregation-item").removeClass("selected");
    $(anchor).addClass("selected");

    // Get the selected aggregation field name (x-axis)
    var selectedFldName = $(anchor).find(".subtitle").attr("data-fldname");

    // Get the selected group field name (y-axis)
    var groupFieldName = $(".Data-Group_Items.selected").attr("data-fldname");

    var selectedItem = _analyticsCharts.metaData.find(item => item.fldname === selectedFldName);

    var groupField = "all";
    var aggField = "count";
    var aggFunc = "count";
    var transId = null;

    if (selectedFldName === "Count") {
        groupField = groupFieldName || "all";
        aggField = "count";
        aggFunc = "count";
        transId = _analyticsCharts.entityTransId;
    } else if (selectedItem) {
        aggFunc = $(anchor).find("button.bg-success").attr("agg_function");
        groupField = groupFieldName || "all";
        aggField = selectedFldName;
        transId = selectedItem.ftransid;
    }

    if (!groupField || !aggField || !aggFunc || (selectedFldName !== "count" && !transId)) {
        console.error("One or more required fields are missing from the selected item.");
        return;
    }

    _analyticsCharts.getAnalyticsChartsDataWS({
        page: "Analytics",
        transId: transId,
        aggField: aggField,
        aggTransId: transId,
        groupField: groupField,
        groupTransId: transId,
        aggFunc: aggFunc,
        chartType: chartType
    });
}




function handleFuncSelection(event, button) {
    event.stopPropagation();
    var parentDiv = button.closest('.Aggregation-icon2');
    parentDiv.querySelectorAll('button').forEach(function (btn) {
        btn.classList.remove('bg-success');
    });
    button.classList.add('bg-success');
    button.closest('.Aggregation-item').click();
}

function chartSelectionClick(event, anchor) {
    event.stopPropagation();

    var parentDiv = anchor.closest('.chart-selections');
    parentDiv.querySelectorAll('a').forEach(function (link) {
        link.classList.remove('bg-success');
    });

    anchor.classList.add('bg-success');

    var chartType = anchor.getAttribute('chart_type');
    var selectedAnchor = $(".Aggregation-item.selected").get(0);
    const chartConfig = _analyticsCharts.getChartTypeConfig(chartType);

    var button = parentDiv.querySelector('.btn.dropdown-toggle');
    if (button) {
        button.querySelector('.chart-name').textContent = chartConfig.name;
        button.querySelector('.material-icons').textContent = chartConfig.icon;
    }

    if (selectedAnchor) {
        selectAggField(selectedAnchor, chartConfig.type);
    } else {
        console.error("No aggregation field is selected.");
    }

    _analyticsCharts.selectedChartType = chartConfig.type;

    storeChartType(chartConfig.type);

}


function storeChartType(chartType) {
    let _this = this;
    var data = {
        page: "Analytics",
        transId: _analyticsCharts.entityTransId,
        properties: { "CHARTTYPE": chartType },
        confirmNeeded: false,
        allUsers: false
    };

    _entityCommon.setAnalyticsDataWS(data, () => {


    }, (error) => {
        showAlertDialog("error", error.status + " " + error.statusText);
    });

    if (_analyticsCharts && typeof _analyticsCharts.mergeAnalyticsPrefsCache === "function") {
        _analyticsCharts.mergeAnalyticsPrefsCache(_analyticsCharts.entityTransId, {
            CHARTTYPE: chartType || "line"
        });
    }
}





function entityModelClose() {

    //$('#dvModalFilter').html("");
    $('#entityModal').modal('hide');
}
function resetSelection() {
    $('#entityDataContainer .selected').removeClass('selected');

    var data = {
        page: "Analytics",
        transId: "",
        properties: { "ENTITIES": "" },
        confirmNeeded: true,
        allUsers: false
    }

    _entityCommon.setAnalyticsDataWS(data, () => {
        window.location.reload();
    }, (error) => {
        showAlertDialog("error", error.status + " " + error.statusText);
    })
}




function saveSelectedEntities() {
    var entitiesToKeep = _analyticsCharts.selectedEntitiesList.filter(item => !item.toBeRemoved);
    _analyticsCharts.selectedEntitiesList.forEach(item => {
        item.toBeRemoved = false;
    });

    var updatedSelectedEntities = entitiesToKeep.map(item => item.name).concat(selectedItemsArray);

    if (updatedSelectedEntities.length > 0) {
        selectedItemsArray = updatedSelectedEntities;

        $('#entityDataContainer .EntityData_Select-lists').each(function () {
            var entityData = $(this).find('.Entity-title').attr('value').toLowerCase();
            if (updatedSelectedEntities.map(e => e.toLowerCase()).includes(entityData)) {
                $(this).hide();
            }
        });

        var data = {
            page: "Analytics",
            transId: "",
            properties: { "ENTITIES": updatedSelectedEntities.join(",") },
            confirmNeeded: true,
            allUsers: false
        }

        _entityCommon.setAnalyticsDataWS(data, () => {
            window.location.reload();
        }, (error) => {
            showAlertDialog("error", error.status + " " + error.statusText);
        })

    } else {

        showAlertDialog("error", "Please select at least one entity to proceed.");
        return;
    }
}


function selectEntity(elem, transId, options = {}) {
    document.querySelectorAll("#dv_EntityContainer .nav-link.active").forEach(nav => nav.classList.remove('active'));
    elem.querySelector(".nav-link").classList.add('active');

    _analyticsCharts.entityTransId = transId;
    _analyticsCharts.entityName = _analyticsCharts.getEntityCaption(transId);

    fetchEntityData(transId, null, options);
}


function moveselection(direction) {
    var currentItem = $(".Aggregation-item.selected").closest(".Aggregation-Item-wrap");
    var targetItem;

    switch (direction) {
        case 'left':
            targetItem = currentItem.prev(".Aggregation-Item-wrap");

            // Loop to the last item if no previous element
            if (targetItem.length === 0) {
                targetItem = $("#Aggregation_Wrapper .Aggregation-Item-wrap").last();
            }
            break;

        case 'right':
            targetItem = currentItem.next(".Aggregation-Item-wrap");

            // Loop to the first item if no next element
            if (targetItem.length === 0) {
                targetItem = $("#Aggregation_Wrapper .Aggregation-Item-wrap").first();
            }
            break;
    }

    if (targetItem && targetItem.length > 0) {
        // Deselect the current item and select the target item
        $(".Aggregation-item").removeClass("selected");
        targetItem.find(".Aggregation-item").addClass("selected");
        targetItem.find(".Aggregation-item").click();
    }
}


function reloadPage() {
    if (typeof _analyticsCharts !== "undefined" && _analyticsCharts) {
        if (typeof _analyticsCharts.clearGridBatchCache === "function") {
            _analyticsCharts.clearGridBatchCache();
        }

        if (_analyticsCharts.enableCardGridLayout) {
            _analyticsCharts.renderAnalyticsGridCards(true);
            return;
        }

        const selectedAnchor = document.querySelector(".Aggregation-item.selected");
        if (selectedAnchor) {
            selectAggField(selectedAnchor, _analyticsCharts.selectedChartType);
            return;
        }
    }

    window.location.reload();
}
function ShowDimmer(status) {

    DimmerCalled = true;
    var dv = $("#waitDiv");

    if (dv.length > 0 && dv != undefined) {
        if (status == true) {

            var currentfr = $("#middle1", parent.document);
            if (currentfr) {
                dv.width(currentfr.width());
            }
            dv.show();
            document.onkeydown = function EatKeyPress() {
                return false;
            }
        } else {
            dv.hide();
            document.onkeydown = function EatKeyPress() {
                if (DimmerCalled == true) {
                    return true;
                }
            }
        }
    } else {
        //TODO:Needs to be tested
        if (window.opener != undefined) {

            dv = $("#waitDiv", window.opener.document);
            if (dv.length > 0) {
                if (status == true)
                    dv.show();
                else
                    dv.hide();
            }
        }
    }
    DimmerCalled = false;
}
function openEntitySelection() {
    // Entity selection popup is intentionally disabled in custom HTML analytics page.
    showAlertDialog("warning", "Entity selection is disabled in this custom Analytics page.");
}


function openFieldSelection() {

    $('#fieldsModal').modal('show');
    // if ($('#fields-selection').html() == "")
    createFieldsLayout();
}

function createFieldsLayout() {
    const fieldsContainer = document.getElementById("fields-selection");

    const xAxisFields = _analyticsCharts.xAxisFields || "All";
    const yAxisFields = _analyticsCharts.yAxisFields || "All";

    const skipFields = ["transid"];

    // Split xAxisFields and yAxisFields into arrays, if not 'All'
    const xAxisArray = xAxisFields === "All" ? [] : xAxisFields.split(",");
    const yAxisArray = yAxisFields === "All" ? [] : yAxisFields.split(",");

    var grpFields = _analyticsCharts.metaData.filter(item => {
        const fdt = `${item?.fdatatype || ""}`.trim().toLowerCase();
        const cdt = `${item?.cdatatype || ""}`.trim().toLowerCase();
        return (item.grpfield === "T") &&
            (cdt === "dropdown" || fdt === "c") &&
            fdt !== "d" &&
            skipFields.indexOf(item.fldname) == -1;
    });
    var aggFields = _analyticsCharts.metaData.filter(item => {
        const fdt = `${item?.fdatatype || ""}`.trim().toLowerCase();
        return (item.aggfield === "T") &&
            fdt !== "d" &&
            skipFields.indexOf(item.fldname) == -1;
    });

    function reorderFields(fields, selectedFields) {
        return fields.sort((a, b) => {
            const aSelected = selectedFields.includes(a.fldname);
            const bSelected = selectedFields.includes(b.fldname);

            if (aSelected && !bSelected) return -1;
            if (!aSelected && bSelected) return 1;

            const aIndex = selectedFields.indexOf(a.fldname);
            const bIndex = selectedFields.indexOf(b.fldname);
            return aIndex - bIndex;
        });
    }

    var orderedAggFields = reorderFields(aggFields, xAxisArray);
    var orderedGrpFields = reorderFields(grpFields, yAxisArray);

    var html = '';
    if (orderedAggFields.length) {
        html += `
        <div class="card KC_Items">
            <div class="card-header collapsible cursor-pointer rotate" data-bs-toggle="collapse" aria-expanded="true" data-bs-target="#fields-aggFields">
                <h3 class="card-title">Aggregate Fields (X-axis)</h3>
                <div class="card-toolbar rotate-180">
                    <span class="material-icons material-icons-style material-icons-2">expand_circle_down</span>
                </div>
            </div>
            <div class="KC_Items_Content collapse show heightControl pt-0---" id="fields-aggFields">
            <table class="table table-hover">
                <tbody id="fields-table-body">`;
        orderedAggFields.forEach(fld => {
            let hidden = `${fld.hide == "T" ? "(Hidden)" : ""}`;
            let hiddenClass = hidden == "(Hidden)" ? "chk-Hiddenfld" : "";
            let fldname = `${fld.fldcap || ''}  (${fld.fldname})  (${fld.dcname}) ${hidden}`;
            html += `<tr><td><input type="checkbox" id="chk_${fld.fldname}" class="chk-fields chk-relateddataflds ${hiddenClass} chk-aggfld" value="${fld.fldname}" data-fldcap="${fld.fldcap || ''}" data-dcno="${fld.dcname}"></td>
            <td><label for="chk_${fld.fldname}">${fldname}</label></td></tr>`;
        });
        html += `</tbody></table></div></div>`;
    }

    if (orderedGrpFields.length) {
        html += `
        <div class="card KC_Items">
            <div class="card-header collapsible cursor-pointer rotate" data-bs-toggle="collapse" aria-expanded="true" data-bs-target="#fields-grpFields">
                <h3 class="card-title">Group Fields (Y-axis)</h3>
                <div class="card-toolbar rotate-180">
                    <span class="material-icons material-icons-style material-icons-2">expand_circle_down</span>
                </div>
            </div>
            <div class="KC_Items_Content collapse show heightControl pt-0---" id="fields-grpFields">
            <table class="table table-hover">
                <tbody id="fields-table-body">`;
        orderedGrpFields.forEach(fld => {
            let hidden = `${fld.hide == "T" ? "(Hidden)" : ""}`;
            let hiddenClass = hidden == "(Hidden)" ? "chk-Hiddenfld" : "";
            let fldname = `${fld.fldcap || ''}  (${fld.fldname})  (${fld.dcname}) ${hidden}`;
            html += `<tr><td><input type="checkbox" id="chk_${fld.fldname}" class="chk-fields chk-relateddataflds ${hiddenClass} chk-grpfld" value="${fld.fldname}" data-fldcap="${fld.fldcap || ''}" data-dcno="${fld.dcname}"></td>
            <td><label for="chk_${fld.fldname}">${fldname}</label></td></tr>`;
        });
        html += `</tbody></table></div></div>`;
    }

    fieldsContainer.innerHTML = html;

    // If "All" is selected for the X-axis, check all Aggregate Fields checkboxes
    if (xAxisFields === "All") {
        document.querySelectorAll(".chk-aggfld:not(.chk-Hiddenfld)").forEach(checkbox => {
            checkbox.checked = true;
        });
    }

    // If "All" is selected for the Y-axis, check all Group Fields checkboxes
    if (yAxisFields === "All") {
        document.querySelectorAll(".chk-grpfld:not(.chk-Hiddenfld)").forEach(checkbox => {
            checkbox.checked = true;
        });
    }

    // Handle manual checkbox selection
    xAxisArray.forEach(fld => {
        if (fld !== "All") {
            const checkbox = document.querySelector(`#chk_${fld}`);
            if (checkbox) {
                checkbox.checked = true;
            } else {
                console.warn(`Checkbox with ID #chk_${fld} not found.`);
            }
        }
    });

    yAxisArray.forEach(fld => {
        if (fld !== "All") {
            const checkbox = document.querySelector(`#chk_${fld}`);
            if (checkbox) {
                checkbox.checked = true;
            } else {
                console.warn(`Checkbox with ID #chk_${fld} not found.`);
            }
        }
    });

    const checkFields = document.querySelectorAll(".chk-fields");
    checkFields.forEach((checkbox) => {
        checkbox.addEventListener("change", function () {
            const allChecked = checkFields.length === document.querySelectorAll(".chk-fields:checked").length;
            document.querySelector("#check-all").checked = allChecked;
        });
    });

    const checkAllCheckbox = document.getElementById("check-all");
    checkAllCheckbox.addEventListener("change", function () {
        const isChecked = checkAllCheckbox.checked;
        checkFields.forEach((checkbox) => {
            checkbox.checked = isChecked;
        });
    });
}






function applyFields() {
    const selectedFields = document.querySelectorAll(".chk-fields:checked");

    if (selectedFields.length === 0) {
        showAlertDialog("error", "Error: No fields are selected.");
        return;
    }

    // Get the child divs by their IDs
    const aggFieldsDiv = document.getElementById("fields-aggFields");
    const grpFieldsDiv = document.getElementById("fields-grpFields");

    let xAxisFields = [];
    let yAxisFields = [];

    selectedFields.forEach((field) => {
        const fieldName = field.value;
        const parentDiv = field.closest(".card");

        if (parentDiv) {
            if (aggFieldsDiv && aggFieldsDiv.contains(field.closest("div"))) {
                xAxisFields.push(fieldName);
            } else if (grpFieldsDiv && grpFieldsDiv.contains(field.closest("div"))) {
                yAxisFields.push(fieldName);
            }
        }
    });

    xAxisFields = xAxisFields.join(",");
    yAxisFields = yAxisFields.join(",");

    // Store the selected fields
    //_entityCommon.storeSelectedFields(xAxisFields, yAxisFields);

    const dataPayload = {
        page: "Analytics",
        transId: _analyticsCharts.entityTransId,
        properties: {
            "XAXISFIELDS": xAxisFields,
            "YAXISFIELDS": yAxisFields,
        },
        confirmNeeded: true,
        allUsers: false
    };
    _entityCommon.setAnalyticsDataWS(dataPayload, () => {
        const transId = _analyticsCharts.entityTransId;
        if (_analyticsCharts && typeof _analyticsCharts.mergeAnalyticsPrefsCache === "function") {
            _analyticsCharts.mergeAnalyticsPrefsCache(transId, {
                "XAXISFIELDS": xAxisFields || "All",
                "YAXISFIELDS": yAxisFields || "All"
            });
        }

        const dataTransIdElement = document.querySelector(`[dt_transid="${transId}"]`);

        if (dataTransIdElement) {
            fieldsModelClose();
            selectEntity(dataTransIdElement, transId);
        } else {
            console.error(`Element with dt_transid="${transId}" not found`);
        }
    }, (error) => {
        showAlertDialog("error", error.status + " " + error.statusText);
    });

}


function resetFields() {
    const checkFields = document.querySelectorAll(".chk-fields , #check-all");
    checkFields.forEach((checkbox) => {
        checkbox.checked = true;
    })

    //_analyticsCharts.fields = "All";
    //_analyticsCharts.setSelectedFields();

    const dataPayload = {
        page: "Analytics",
        transId: _analyticsCharts.entityTransId,
        properties: {
            "XAXISFIELDS": "All",
            "YAXISFIELDS": "All",
        },
        confirmNeeded: true,
        allUsers: false
    };
    _entityCommon.setAnalyticsDataWS(dataPayload, () => {
        const transId = _analyticsCharts.entityTransId;
        if (_analyticsCharts && typeof _analyticsCharts.mergeAnalyticsPrefsCache === "function") {
            _analyticsCharts.mergeAnalyticsPrefsCache(transId, {
                "XAXISFIELDS": "All",
                "YAXISFIELDS": "All"
            });
        }

        const dataTransIdElement = document.querySelector(`[dt_transid="${transId}"]`);

        if (dataTransIdElement) {
            fieldsModelClose();
            selectEntity(dataTransIdElement, transId);
        } else {
            console.error(`Element with dt_transid="${transId}" not found`);
        }
    }, (error) => {
        showAlertDialog("error", error.status + " " + error.statusText);
    });

}

function fieldsModelClose() {
    $('#fieldsModal').modal('hide');
}


document.addEventListener('DOMContentLoaded', function () {
    const container = document.getElementById('dv_EntityContainer');
    let dragSrcEl = "";
    let dragImage = "";

    function handleDragStart(e) {
        dragSrcEl = this;

        // Create a clone of the element being dragged
        dragImage = dragSrcEl.cloneNode(true);
        dragImage.classList.add('drag-image');
        document.body.appendChild(dragImage);

        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setDragImage(dragImage, 0, 0);
        e.dataTransfer.setData('text/html', this.innerHTML);

        this.classList.add('dragging');
    }

    function handleDragOver(e) {
        if (e.preventDefault) {
            e.preventDefault(); // Allow drop
        }
        e.dataTransfer.dropEffect = 'move';
        return false;
    }

    function handleDragEnter(e) {
        if (this !== dragSrcEl) {
            this.classList.add('over');
        }
    }

    function handleDragLeave(e) {
        this.classList.remove('over');
    }

    function handleDrop(e) {
        if (e.stopPropagation) {
            e.stopPropagation(); // Prevent default drop behavior
        }

        if (dragSrcEl !== this) {
            const items = Array.from(container.querySelectorAll('.nav-item'));
            const srcIndex = items.indexOf(dragSrcEl);
            const targetIndex = items.indexOf(this);

            if (srcIndex < targetIndex) {
                this.after(dragSrcEl);
            } else {
                this.before(dragSrcEl);
            }
        }

        // Remove the custom drag image
        if (dragImage && dragImage.parentNode) {
            dragImage.parentNode.removeChild(dragImage);
        }

        const newOrder = Array.from(container.querySelectorAll('.nav-item'))
            .map(item => item.getAttribute('dt_transid')).join(',');

        var data = {
            page: "Analytics",
            transId: "",
            properties: { "ENTITIES": newOrder },
            confirmNeeded: true,
            allUsers: false
        }

        _entityCommon.setAnalyticsDataWS(data, () => { }, (error) => {
            showAlertDialog("error", error.status + " " + error.statusText);
        })

        return false;
    }


    function handleDragEnd(e) {
        this.classList.remove('dragging');
        items.forEach(function (item) {
            item.classList.remove('over');
        });

        // Remove the custom drag image if not already removed
        if (dragImage && dragImage.parentNode) {
            dragImage.parentNode.removeChild(dragImage);
        }
    }

    function addDragAndDropHandlers(item) {
        item.addEventListener('dragstart', handleDragStart, false);
        item.addEventListener('dragenter', handleDragEnter, false);
        item.addEventListener('dragover', handleDragOver, false);
        item.addEventListener('dragleave', handleDragLeave, false);
        item.addEventListener('drop', handleDrop, false);
        item.addEventListener('dragend', handleDragEnd, false);
    }

    let items = container.querySelectorAll('.nav-item');
    items.forEach(function (item) {
        item.setAttribute('draggable', 'true');
        addDragAndDropHandlers(item);
    });

    const observer = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
            if (mutation.addedNodes.length) {
                mutation.addedNodes.forEach(node => {
                    if (node.nodeType === 1 && node.classList.contains('nav-item')) {
                        node.setAttribute('draggable', 'true');
                        addDragAndDropHandlers(node);
                    }
                });
            }
        });
    });

    observer.observe(container, {
        childList: true,
    });
});



// X-axis draggable code

document.addEventListener('DOMContentLoaded', function () {
    const axcontainer = document.getElementById('Data-Group-container');
    let dragSrcEl = '';
    let dragClone = null;

    function handleDragStart(e) {
        dragSrcEl = this;
        e.dataTransfer.effectAllowed = 'move';

        dragClone = this.cloneNode(true);
        dragClone.style.position = 'absolute';
        dragClone.style.top = '-1000px';
        document.body.appendChild(dragClone);

        e.dataTransfer.setDragImage(dragClone, e.offsetX, e.offsetY);

        this.classList.add('dragging');
    }

    function handleDragOver(e) {
        if (e.preventDefault) {
            e.preventDefault();
        }
        e.dataTransfer.dropEffect = 'move';
        return false;
    }

    function handleDragEnter(e) {
        if (this !== dragSrcEl) {
            this.classList.add('over');
        }
    }

    function handleDragLeave(e) {
        this.classList.remove('over');
    }

    function handleDrop(e) {
        if (e.stopPropagation) {
            e.stopPropagation();
        }

        if (dragSrcEl !== this) {
            const items = Array.from(axcontainer.querySelectorAll('.Data-Group_Items:not(.first-item):not(.selected)'));
            const srcIndex = items.indexOf(dragSrcEl);
            const targetIndex = items.indexOf(this);

            if (srcIndex < targetIndex) {
                this.after(dragSrcEl);
            } else {
                this.before(dragSrcEl);
            }

            items.forEach(item => {
                const icon = item.querySelector('.material-icons');
                if (icon) {
                    icon.style.color = item.dataset.initialColor;
                }
            });

            const updatedItems = Array.from(axcontainer.querySelectorAll('.Data-Group_Items:not(.first-item):not(.selected)'));
            const yOrder = updatedItems.map(item => item.getAttribute('data-fldname'));
            console.log("Updated yOrder:", yOrder);

            const selectedFldName = dragSrcEl.getAttribute('data-fldname');
            console.log("Selected fldname:", selectedFldName);

            const metadataItem = _analyticsCharts.metaData.find(meta => meta["fldname"] === selectedFldName);

            if (metadataItem) {
                const transId = metadataItem.ftransid;
                console.log("Found ftransid:", transId);

                const dataPayload = {
                    page: "Analytics",
                    transId: transId,
                    properties: {
                        "YAXISFIELDS": yOrder.join(','),
                    },
                    allUsers: false
                };

                _entityCommon.setAnalyticsDataWS(dataPayload, () => {
                    if (_analyticsCharts && typeof _analyticsCharts.mergeAnalyticsPrefsCache === "function") {
                        _analyticsCharts.mergeAnalyticsPrefsCache(_analyticsCharts.entityTransId, {
                            "YAXISFIELDS": yOrder.join(',')
                        });
                    }
                }, (error) => {
                    showAlertDialog("error", error.status + " " + error.statusText);
                })
            } else {
                console.warn("Metadata item not found for selected fldname:", selectedFldName);
            }
        }

        return false;
    }



    function handleDragEnd(e) {
        this.classList.remove('dragging');
        const items = Array.from(axcontainer.querySelectorAll('.Data-Group_Items:not(.first-item):not(.selected)'));
        items.forEach(function (item) {
            item.classList.remove('over');
        });

        if (dragClone) {
            document.body.removeChild(dragClone);
            dragClone = null;
        }
    }

    function addDragAndDropHandlers(item) {
        item.addEventListener('dragstart', handleDragStart, false);
        item.addEventListener('dragenter', handleDragEnter, false);
        item.addEventListener('dragover', handleDragOver, false);
        item.addEventListener('dragleave', handleDragLeave, false);
        item.addEventListener('drop', handleDrop, false);
        item.addEventListener('dragend', handleDragEnd, false);
    }

    const firstItem = axcontainer.querySelector('.Data-Group_Items:first-child');
    if (firstItem) {
        firstItem.classList.add('first-item');
    }

    let items = axcontainer.querySelectorAll('.Data-Group_Items:not(.first-item):not(.selected)');
    items.forEach(function (item) {
        item.setAttribute('draggable', 'true');

        const icon = item.querySelector('.material-icons');
        if (icon) {
            item.dataset.initialColor = window.getComputedStyle(icon).color;
        }
        addDragAndDropHandlers(item);
    });

    const observer = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
            if (mutation.addedNodes.length) {
                mutation.addedNodes.forEach(node => {
                    if (node.classList && node.classList.contains('Data-Group_Items') && !node.classList.contains('selected') && !node.classList.contains('first-item')) {
                        node.setAttribute('draggable', 'true');
                        // Store the initial color of the icon
                        const icon = node.querySelector('.material-icons');
                        if (icon) {
                            node.dataset.initialColor = window.getComputedStyle(icon).color;
                        }
                        addDragAndDropHandlers(node);
                    }
                });
            }
        });
    });

    observer.observe(axcontainer, {
        childList: true,
    });
});

// y-axis dragable code


document.addEventListener('DOMContentLoaded', function () {
    const wrapper = document.getElementById('Aggregation_Wrapper');
    let dragSrcEl = '';
    let dragClone = null;

    function handleDragStart(e) {
        if (this.getAttribute('draggable') === 'false') {
            return;
        }

        dragSrcEl = this;
        e.dataTransfer.effectAllowed = 'move';

        dragClone = this.cloneNode(true);
        dragClone.style.position = 'absolute';
        dragClone.style.top = '-1000px';
        document.body.appendChild(dragClone);

        e.dataTransfer.setDragImage(dragClone, e.offsetX, e.offsetY);

        this.classList.add('dragging');
    }

    function handleDragOver(e) {
        if (e.preventDefault) {
            e.preventDefault();
        }
        e.dataTransfer.dropEffect = 'move';
        return false;
    }

    function handleDragEnter(e) {
        if (this !== dragSrcEl) {
            this.classList.add('over');
        }
    }

    function handleDragLeave(e) {
        this.classList.remove('over');
    }

    function handleDrop(e) {
        if (e.stopPropagation) {
            e.stopPropagation();
        }

        if (dragSrcEl !== this) {
            const items = Array.from(wrapper.querySelectorAll('.Aggregation-Item-wrap'));
            const srcIndex = items.indexOf(dragSrcEl);
            const targetIndex = items.indexOf(this);

            if (srcIndex < targetIndex) {
                this.after(dragSrcEl);
            } else {
                this.before(dragSrcEl);
            }

            const updatedItems = Array.from(wrapper.querySelectorAll('.Aggregation-Item-wrap'));
            const xOrder = updatedItems.map(item => item.getAttribute('data-fldname'));
            console.log("Updated xOrder:", xOrder);

            const selectedFldName = dragSrcEl.getAttribute('data-fldname');
            console.log("Selected fldname:", selectedFldName);

            const metadataItem = _analyticsCharts.metaData.find(meta => meta["fldname"] === selectedFldName);

            if (metadataItem) {
                const transId = metadataItem.ftransid;
                console.log("Found ftransid:", transId);

                const dataPayload = {
                    page: "Analytics",
                    transId: transId,
                    properties: {
                        "XAXISFIELDS": xOrder.join(','),
                    },
                    allUsers: false
                };

                _entityCommon.setAnalyticsDataWS(dataPayload, () => {
                    if (_analyticsCharts && typeof _analyticsCharts.mergeAnalyticsPrefsCache === "function") {
                        _analyticsCharts.mergeAnalyticsPrefsCache(_analyticsCharts.entityTransId, {
                            "XAXISFIELDS": xOrder.join(',')
                        });
                    }
                }, (error) => {
                    showAlertDialog("error", error.status + " " + error.statusText);
                })
            } else {
                console.warn("Metadata item not found for selected fldname:", selectedFldName);
            }
        }

        return false;
    }


    function handleDragEnd(e) {
        this.classList.remove('dragging');
        const items = Array.from(wrapper.querySelectorAll('.Aggregation-Item-wrap:not(.selected)'));
        items.forEach(function (item) {
            item.classList.remove('over');
        });

        if (dragClone) {
            document.body.removeChild(dragClone);
            dragClone = null;
        }
    }

    function addDragAndDropHandlers(item) {
        item.addEventListener('dragstart', handleDragStart, false);
        item.addEventListener('dragenter', handleDragEnter, false);
        item.addEventListener('dragover', handleDragOver, false);
        item.addEventListener('dragleave', handleDragLeave, false);
        item.addEventListener('drop', handleDrop, false);
        item.addEventListener('dragend', handleDragEnd, false);
    }

    let items = wrapper.querySelectorAll('.Aggregation-Item-wrap');
    items.forEach(function (item, index) {
        if (index !== 0) {
            item.setAttribute('draggable', 'true');
            addDragAndDropHandlers(item);
        } else {
            item.setAttribute('draggable', 'false');
        }
    });

    const observer = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
            if (mutation.addedNodes.length) {
                mutation.addedNodes.forEach(node => {
                    if (node.classList && node.classList.contains('Aggregation-Item-wrap')) {
                        const index = Array.from(wrapper.children).indexOf(node);
                        if (index !== 0) {
                            node.setAttribute('draggable', 'true');
                            addDragAndDropHandlers(node);
                        }
                    }
                });
            }
        });
    });

    observer.observe(wrapper, {
        childList: true,
    });
});

function toggleSelection(entityIndex) {
    var selectedItem = _analyticsCharts.selectedEntitiesList[entityIndex];

    selectedItem.toBeRemoved = !selectedItem.toBeRemoved;

    var selectedEntity = $(`#selectedEntitiesContainer .EntityData_Select-lists:eq(${entityIndex})`);
    selectedEntity.find('.EntityData_Select-Items').toggleClass('selected');

    var anySelected = _analyticsCharts.selectedEntitiesList.some(item => !item.toBeRemoved);
    var selectedLabel = $(".selcted");
    selectedLabel.toggle(anySelected);

    if (!selectedItem.toBeRemoved) {
        $('#entityDataContainer .EntityData_Select-lists').each(function () {
            var entityData = $(this).find('.EntityData_Select-Items').text().trim().toLowerCase();
            console.log(`Showing entity: ${entityData}`);
            if (entityData === selectedItem.name.toLowerCase()) {
                $(this).show();
            }
        });
    }
}


function navigateToListPage(transid, fldname, element) {
    let keyname = "";
    if (element && typeof element.getAttribute === "function") {
        keyname = element.getAttribute('data-keyname') || "";
    }
    navigateToListPageByValue(transid, fldname, keyname);
}

function navigateToListPageByValue(transid, fldname, keyname) {
    if (_entityCommon.inValid(fldname)) {
        let selectedEntity = _analyticsCharts.selectedEntitiesList.find(entity => entity.name === transid);
        let ename = selectedEntity.caption;
        let url = `../aspx/Entity.aspx?tstid=${transid}`;

        loadEntityPage(url);
    }
    else {
        if (_analyticsCharts.selectedEntitiesList && fldname) {
            let selectedEntity = _analyticsCharts.selectedEntitiesList.find(entity => entity.name === transid);

            if (selectedEntity) {
                let ename = selectedEntity.caption;
                let url = `../aspx/Entity.aspx?tstid=${transid}&filterval=${keyname}&filterfld=${fldname}&applyfilter=true`;

                loadEntityPage(url);
            } else {
                console.error(`Entity not found for transid: ${transid}`);
            }
        } else {
            console.error(`selectedEntitiesList or fldname is undefined`);
        }
    }
}

function handleAnalyticsGridPointNavigation(linkData) {
    const linkPrefix = "__analytics_grid_link__:";
    if (_entityCommon.inValid(linkData) || typeof linkData !== "string" || !linkData.startsWith(linkPrefix)) {
        return;
    }

    let payload = null;
    try {
        payload = JSON.parse(decodeURIComponent(linkData.substring(linkPrefix.length)));
    } catch (error) {
        console.error("Invalid analytics chart navigation payload.", error);
        return;
    }

    try {
        if (typeof window !== "undefined" && window.ANALYTICS_FILTER_DEBUG !== false) {
            console.log("[AnalyticsFilter] pointClickPayload", payload);
        }
    } catch (error) { }

    if (!payload || typeof payload !== "object") {
        return;
    }

    if (payload.type === "period") {
        navigateToPeriod(payload.period || "total");
        return;
    }

    if (payload.type === "grid-filter-toggle") {
        // Deprecated flow: keep backward compatibility for old cached links by
        // treating point click as list navigation.
        navigateToListPageByValue(
            payload.transid || _analyticsCharts?.entityTransId || "",
            payload.groupField || payload.fldname || "",
            payload.value || payload.keyname || ""
        );
        return;
    }

    if (payload.type === "list") {
        navigateToListPageByValue(payload.transid || _analyticsCharts.entityTransId, payload.fldname || "", payload.keyname || "");
    }
}

function navigateToPeriod(period) {
    let transid = _analyticsCharts.entityTransId;

    if (_analyticsCharts.selectedEntitiesList) {
        let selectedEntity = _analyticsCharts.selectedEntitiesList.find(entity => entity.name === transid);

        if (selectedEntity) {
            let ename = selectedEntity.caption;
            let url = `../aspx/Entity.aspx?tstid=${transid}`;

            if (period.trim().toLowerCase() !== 'total') {
                let filterDates = _entityCommon.getDatesBasedonSelectionForBetweenFilter(period);
                filterDates.from = moment(filterDates.from, advFilterDtCulture).format("DD-MMM-YYYY");
                filterDates.to = moment(filterDates.to, advFilterDtCulture).format("DD-MMM-YYYY");

                url += `&filterfrom=${filterDates.from}&filterto=${filterDates.to}&filterfld=modifiedon&filtertype=date&applyfilter=true`;
            }

            loadEntityPage(url);
        } else {
            console.error(`Entity not found for transid: ${transid}`);
        }
    } else {
        console.error(`selectedEntitiesList is undefined`);
    }
}




function loadEntityPage(url) {
    parent.ShowDimmer(true);
    _entityCommon.loadHyperLink(url);
}

function handleNoChartData() {
    document.querySelectorAll(".analytics-container").forEach(item => {
        item.classList.add("d-none");
    })
    document.querySelector(".nodata").classList.remove("d-none");
}

function handleValidChartData() {
    document.querySelectorAll(".analytics-container").forEach(item => {
        item.classList.remove("d-none");
    });

    const noDataElement = document.querySelector(".nodata");
    if (noDataElement) {
        noDataElement.classList.add("d-none");
    }
}




function fetchEntityData(transId, callback, options = {}) {
    const forceRefresh = !!options.forceRefresh;

    const applyEntityResponse = (pageLoadData) => {
        if (!pageLoadData || !pageLoadData.result || !pageLoadData.result.data) {
            return;
        }

        const responseData = pageLoadData.result.data || {};
        let { "XAXISFIELDS": xAxisFields, "YAXISFIELDS": yAxisFields } = pageLoadData.result.data.Properties || {};

        if (Array.isArray(responseData.AllEntitiesList)) {
            _analyticsCharts.allEntitiesList = responseData.AllEntitiesList;
        }

        if (Array.isArray(responseData.SelectedEntitiesList) && responseData.SelectedEntitiesList.length > 0) {
            _analyticsCharts.selectedEntitiesList = responseData.SelectedEntitiesList;
        } else if (_entityCommon.inValid(_analyticsCharts.selectedEntitiesList)) {
            _analyticsCharts.selectedEntitiesList = [{
                name: transId,
                caption: transId
            }];
        }

        document.querySelector("#Entity_summary_Left")?.classList.remove("d-none");
        document.querySelector("#Entity_summary_Right")?.classList.remove("d-none");
        document.querySelector("#Analytics_Grid_Container")?.classList.remove("d-none");

        _analyticsCharts.xAxisFields = xAxisFields || "All";
        _analyticsCharts.yAxisFields = yAxisFields || "All";
        _analyticsCharts.selectedChartType = "line";

        const transidFieldsRaw = responseData.Fields || responseData.fields || "";
        _analyticsCharts.metaData = _analyticsCharts.mergeTransidFieldSourceIntoMeta(pageLoadData.result.data.MetaData, transidFieldsRaw);
        _analyticsCharts.entityTransId = pageLoadData.result.data.TransId;
        _analyticsCharts.entityName = _analyticsCharts.getEntityCaption(_analyticsCharts.entityTransId);

        _analyticsCharts.loadStoredAnalyticsPreferences(_analyticsCharts.entityTransId, () => {
            _analyticsCharts.constructXandYAxis(_analyticsCharts.metaData, "Page Load");
            _analyticsCharts.constructSelectedEntityHeader();
            _analyticsCharts.updateChartButton(_analyticsCharts.selectedChartType);
            _analyticsCharts.openFieldSelectionOnPageLoad();
            if (typeof callback === "function") {
                callback();
            }
        }, { forceRefresh: forceRefresh });
    };

    if (!forceRefresh && _analyticsCharts && typeof _analyticsCharts.readAnalyticsEntityCache === "function") {
        const cachedResponse = _analyticsCharts.readAnalyticsEntityCache(transId);
        if (cachedResponse) {
            _analyticsCharts.logFilterDebug("fetchEntityData:entityCacheHit", { transId: transId });
            applyEntityResponse(cachedResponse);
            return;
        }
    }

    $.ajax({
        url: `${_analyticsCharts.getAssetBaseUrl()}/aspx/Analytics.aspx/GetAnalyticsEntityWS`,
        type: 'POST',
        cache: false,
        async: true,
        dataType: 'json',
        data: JSON.stringify({ page: "Analytics", transId: transId }),
        contentType: "application/json",
        success: function (data) {
            if (data && data.d) {
                let pageLoadData = JSON.parse(data.d);
                if (_analyticsCharts && typeof _analyticsCharts.writeAnalyticsEntityCache === "function") {
                    _analyticsCharts.writeAnalyticsEntityCache(transId, pageLoadData);
                }
                applyEntityResponse(pageLoadData);
            }
        },
        error: function (error) {
            console.error("Error fetching entity data:", error);
        }
    });
}

function openFilters() {
    if (!_analyticsCharts || !_entityFilter) {
        return;
    }

    if (typeof _analyticsCharts.ensureEntityFilterBridge === "function") {
        _analyticsCharts.ensureEntityFilterBridge();
    }

    const showFilterPopup = () => {
        let metadata = _analyticsCharts.metaData;
        let filterObj = _analyticsCharts.filterObj;
        let containerId = 'dvModalFilter';

        _analyticsCharts.ensureEntityFilterPillsContainer();
        _entityFilter.metaData = Array.isArray(metadata) ? metadata : [];
        _entityFilter.metadata = _entityFilter.metaData;
        _entityFilter.containerId = containerId;
        _entityFilter.filterObj = filterObj;
        _entityFilter.pageName = "Analytics";
        _entityFilter.entityTransId = _analyticsCharts.entityTransId || _analyticsCharts.dataSourceName || "";
        _entityFilter.activeFilterArray = [];
        _entityFilter.activeFilterId = "";
        _entityFilter.activeFilterName = "";

        _entityFilter.createFilterLayout();
        $('#applyFilterButton').off('click.analyticsFilters').on('click.analyticsFilters', function () {
            _entityFilter.handleApply();
        });

        $('#filterModal').modal('show');
    };

    const needsSourceMeta = typeof _analyticsCharts.hasDropdownSourceGap === "function"
        ? _analyticsCharts.hasDropdownSourceGap(_analyticsCharts.metaData)
        : false;

    if (needsSourceMeta && typeof _analyticsCharts.fetchEntityFieldSourceMeta === "function") {
        _analyticsCharts.fetchEntityFieldSourceMeta(_analyticsCharts.entityTransId || _analyticsCharts.dataSourceName, { forceRefresh: true })
            .finally(() => showFilterPopup());
        return;
    }

    showFilterPopup();
}


function filterModelClose() {
    $('#filterGroupName').val('')
    $('#filterGroupModalWrapper').modal('hide');



    $('#dvModalFilter').html("");
    $('#filterModal').modal('hide');

}

function scrollLeft() {
    document.querySelector('.scrollable-menu').scrollBy({
        left: -200,
        behavior: 'smooth'
    });
}

function scrollRight() {
    document.querySelector('.scrollable-menu').scrollBy({
        left: 200,
        behavior: 'smooth'
    });
}

function increaseGridCardsPerRow() {
    if (_analyticsCharts) {
        _analyticsCharts.changeGridCardsPerRow(1);
    }
}

function decreaseGridCardsPerRow() {
    if (_analyticsCharts) {
        _analyticsCharts.changeGridCardsPerRow(-1);
    }
}

function toggleAnalyticsHyperlinksPanel() {
    if (_analyticsCharts) {
        _analyticsCharts.toggleHyperlinkPanel();
    }
}




