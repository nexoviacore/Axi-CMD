(() => {
    // 20-02-2026: Pre release version

    // /plugins/Axi/HTMLPages/js/axi-autocomplete.js
    
    let apiMetadataUrl = "";
    let apiMetadataConfigPromise = null;
    let apiMetadataConfigError = "";

    const goOption = {
        displaydata: "Go [Ctrl + Enter]",
        name: "GO_ACTION",
        isExecutable: true
    };

    const saveOption = {
        displaydata: "Save [Ctrl + S]",
        name: "Save_ACTION",
        isExecutable: true
    };



    const VIEW_HANDLERS = {
        tstruct: ({ transId, fieldName, fieldValue }) =>
            redirectToEntity(transId, fieldName, fieldValue),

        iview: ({ transId }) =>
            redirectToIView(transId),

        page: ({ transId, fieldName, fieldValue }) =>
            redirectToEntity(transId, fieldName, fieldValue),

        ads: ({ transId, fieldName, fieldValue }) => redirectToEntity(transId, fieldName, fieldValue)



    };






    const COMMAND_HANDLERS = {
        show: {
            toast: () => showToast(input.value)

        },
        edit: {
            default: handleEditData,
            data: handleEditData,
            // user: handleEditUser



        },
        create: {
            default: handleCreate,




        },
        view: {
            default: handleViewCommand,



            inbox: handleViewInbox,



        },
        configure: {
            peg: handleConfigurePeg,

            api: handleConfigureApi,

            properties: handleConfigureProperties,
            job: handleConfigureJob,
            rule: handleConfigureRule,
            server: handleConfigureServer,
            formnotification: handleConfigureFormNotification,
            pegformnotification: handleCofigurePegFormNotification,
            permission: handleConfigurePermissions,
            access: handleConfigureAccess,
            schdulednotification: handleConfigureSchduledNotification,
            keyfield: handleKeyfield
        },
        open: {
            default: handleOpenSource,
            ads: handleOpenAds,
            card: handleOpenCard,
            page: handleOpenPage,
            appvar: handleOpenAppVar,
            devoption: handleOpenDevOptions,
            dbconsole: handleOpenDbConsole,


        },
        upload: {
            default: handleUpload
        },
        download: {
            default: handleDownload
        },
        set: {
            default: () => console.log("set command!")
        },
        run: {
            default: handleRunCommand,
        },
        analyse: {
            default: handleAnalyse
        },
        ai: {
            start: handleAiStart,

        },
        connect: { default: () => handleAiButtons("openConnect"), },
        ask: { default: handleAiAsk, },
        end: { default: handleAiEnd, },
        editprompt: { default: () => handleAiButtons("openSystemPrompt") },
        analyze: { default: () => handleAiButtons("axiLoad"), },
        upload: { default: () => handleAiButtons("openUpload") }
    };



    let SET_COMMAND_STATE = {
        isNextField: false,  
        currentField: null,
        currentFieldType : null,
        isFirst:true,
        transid: null,
        currentFieldValue: null,
        isDropDown : false
    };

    let input;

    let hintDiv;
    let list;
    let btnRefresh;
    let runBtn;
    let axiClearBtn;
    let axiLogo;
    let searchWrapper;
    let setCommandTransid = null;

    //let cachedSessionId;


    let topToolbarButtons = null;
    let bottomToolbarButtons = null;
    let entityToolbarButtons = null;
    let designModeToolbarButtons = null;
    let pfToolbarButtons = null;
    let buttonsList = null;
    const OPERATORS_LIST = [">=", "<=", "!=", "=", ">", "<"];
    const OPERATORS_SET = new Set(OPERATORS_LIST);


    const OPERATOR_REGEX_PART = OPERATORS_LIST.join("|");

    // STATE
    let commands = null;
    let items = [];
    let activeIndex = -1;
    let resolvedParams = {};
    let lastTypedTokens = [];

    // DATA CACHES
    let axDatasourceObj = {};
    let activeFetches = new Set();
    let filteredObjects = [];
    let adsfieldvalueanddt = {};
    let createfieldnamevaluesList = [];
    let mode = "";
    const aiModeCommands = {
        "connect": { "cmdToken": 11, "command": "", "commandGroup": "connect", "prompts": [] },
        "ask": { "cmdToken": 11, "command": "", "commandGroup": "ask", "prompts": [{"cmdToken":11,"wordPos":2,"prompt":"Chat","promptSource":"","promptParams":"","promptValues":"","extraParams":""}] },
        "end": { "cmdToken": 11, "command": "", "commandGroup": "end", "prompts": [] },
        "editprompt": { "cmdToken": 11, "command": "", "commandGroup": "editprompt", "prompts": [] },
        "analyze": { "cmdToken": 11, "command": "", "commandGroup": "analyze", "prompts": [] },
        "upload": { "cmdToken": 11, "command": "", "commandGroup": "upload", "prompts": [] }
    }


    function init() {

        input = document.getElementById("Axi-Searchinp");


        if (!input) {
            console.log("Axi Input not ready yet... waiting.");
            setTimeout(init, 200);
            return;
        }

        console.log("Axi Input Found!", input);

        axiLogo = document.getElementById("axiLogo");



        if (!axiLogo) {
            console.log("Axi Logo not ready yet... waiting.");
            setTimeout(init, 200);
            return;

        }

        searchWrapper = document.querySelector(".searchwrap-AXI");

        if (!searchWrapper) {
            console.log("Axi search wrapper not ready yet... waiting.");
            setTimeout(init, 200);
            return;

        }

        hintDiv = document.getElementById("axiHint");
        if (!hintDiv) {
            console.log("Axi HintDiv not ready yet... waiting.");
            setTimeout(init, 200);
            return;
        }

        console.log("Axi HintDiv Found!", hintDiv);

        list = document.getElementById("axiSuggestions");
        if (!list) {
            console.log("Axi AxiSuggestion not ready yet... waiting.");
            setTimeout(init, 200);
            return;
        }

        console.log("Axi AxiSuggestionList found Found!", list);

        btnRefresh = document.getElementById("btnRefresh");
        if (!btnRefresh) {
            console.log("Axi btnRefresh not ready yet... waiting.");
            setTimeout(init, 200);
            return;
        }

        console.log("Axi btnRefresh Found!", btnRefresh);

        runBtn = document.getElementById("runBtn");
        if (!runBtn) {
            console.log("Axi Run btn not ready yet... waiting.");
            setTimeout(init, 200);
            return;
        }

        console.log("Axi Runbtn Found!", runBtn);

        axiClearBtn = document.getElementById("axiClearBtn");

        if (!axiClearBtn) {
            console.log("Axi Clear button not ready yet... waiting.");
            setTimeout(init, 200);
            return;
        }





        console.log("Axi Clear button found!", axiClearBtn);



        setupEventListeners();



        initCommands(false);
    }

    init();

    /* ===============================
        INITIALIZATION
    =============================== */
    async function initCommands(isForced = false) {

        let appSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        console.log("Origin: " + appSessUrl);
        const projInfoKey = `projInfo-${appSessUrl}`;

        const appname = localStorage.getItem(projInfoKey);
        console.log(appname);

        await ensureApiMetadataConfigLoaded();
        if (!apiMetadataUrl) {
            console.error("Metadata API URL is not configured.", apiMetadataConfigError);
            showToast("Metadata API URL is not configured.");
            return;
        }


        const cached = localStorage.getItem("axi_commands_v1");
        if (cached && !isForced) {
            commands = JSON.parse(cached);
            console.log(JSON.stringify(commands));

        } else {
            try {
                const res = await fetch(`${apiMetadataUrl}?view=metadata&forceRefresh=${isForced}&appname=${appname}`);
                if (!res.ok) throw new Error("Metadata fetch failed");
                const data = await res.json();
                commands = data.commands;

                console.log(JSON.stringify(commands));
                localStorage.setItem("axi_commands_v1", JSON.stringify(commands));
            } catch (err) {
                console.error("Critical: Could not load commands", err);
            }
        }
    }

    function getAppBaseUrl() {
        const href = top.window.location.href;
        const aspxIndex = href.toLowerCase().indexOf("/aspx/");

        if (aspxIndex === -1) {
            throw new Error(`Cannot resolve app base URL. '/aspx/' not found in: ${href}`);
        }

        return href.substring(0, aspxIndex);
    }

    async function loadApiMetadataConfig() {
        let configUrl = "";
        try {
            configUrl = `${getAppBaseUrl()}/plugins/Axi/axiConfig.json`;
            const res = await fetch(configUrl, { cache: "no-store" });
            if (!res.ok) {
                throw new Error(`Failed to load ${configUrl}. Status: ${res.status}`);
            }

            const settings = await res.json();
            const configuredApiMetadata = typeof settings?.API_METADATA === "string" ? settings.API_METADATA.trim() : "";

            if (!configuredApiMetadata) {
                throw new Error(`API_METADATA is missing or empty in ${configUrl}`);
            }

            apiMetadataUrl = configuredApiMetadata;
            apiMetadataConfigError = "";
        } catch (error) {
            apiMetadataUrl = "";
            apiMetadataConfigError = (error && error.message) ? error.message : String(error);
            showToast("Failed to load API metadata configuration.");
            console.error(`Failed to resolve API_METADATA from ${configUrl || "app base URL"}`, error);
        }
    }

    function ensureApiMetadataConfigLoaded() {
        if (!apiMetadataConfigPromise) {
            apiMetadataConfigPromise = loadApiMetadataConfig();
        }

        return apiMetadataConfigPromise;
    }

    function getActivePromptInfo(commandConfig, tokens, targetIndex) {
        // targetIndex is 0-based. WordPos is 1-based.
        // Since the user DOES NOT type the extraParam, the mapping is direct.
        const currentWordPos = targetIndex + 1;

        const sortedPrompts = commandConfig.prompts.sort((a, b) => a.wordPos - b.wordPos);
        const prompt = sortedPrompts.find(p => p.wordPos === currentWordPos);

        if (!prompt) return null;

        if (targetIndex > 0) {
            const prevWordPos = currentWordPos - 1;
            const prevPrompt = sortedPrompts.find(p => p.wordPos === prevWordPos);



            if (prevPrompt) {

                const prevSources = prevPrompt.promptSource
                    .split(',')
                    .map(s => s.trim().toLowerCase())
                    .filter(Boolean);

                const fieldSource = prevSources.find(src =>
                    src.includes('keyvalue') || src.includes('fieldname')
                );

                if (fieldSource) {

                    const prevTokenRaw = cleanString(tokens[targetIndex - 1]);
                    let foundItem = null;

                    for (const key in axDatasourceObj) {
                        if (key.startsWith(fieldSource)) {
                            const list = axDatasourceObj[key];

                            foundItem = list.find(item =>
                                (item.name && item.name.toLowerCase() === prevTokenRaw.toLowerCase()) ||
                                (item.displaydata && item.displaydata.toLowerCase() === prevTokenRaw.toLowerCase())
                            );

                            if (foundItem) break;
                        }
                    }

                    if (foundItem && foundItem.isfield === 'f') {
                        console.log("Short Circuit: Previous item is not a field. Stopping prompts.");
                        return null;
                    }
                }
            }

        }

        let activeSource = prompt.promptSource || "";

        // Handle Dynamic Source Switching 
        if (activeSource.includes(",")) {
            const prevWordPos = currentWordPos - 1;
            const prevPrompt = sortedPrompts.find(p => p.wordPos === prevWordPos);

            if (prevPrompt && prevPrompt.promptValues) {
                const prevTokenIndex = targetIndex - 1;
                const prevValue = cleanString(tokens[prevTokenIndex]);
                const allowedValues = prevPrompt.promptValues.split(',').map(v => v.trim().toLowerCase());
                let valueIndex = allowedValues.indexOf(prevValue.toLowerCase());

                if (valueIndex === -1 && commandConfig.commandGroup?.toLowerCase() === 'view') {
                    const detectedType = getType(commandConfig?.prompts?.[0]?.promptSource.toLowerCase(), prevValue, prevPrompt.promptValues);

                    if (detectedType) {
                        valueIndex = allowedValues.indexOf(detectedType.toLowerCase());
                    }


                }

                if (valueIndex !== -1) {
                    const sources = activeSource.split(',');
                    activeSource = sources[valueIndex] ? sources[valueIndex].trim() : "";
                }

                console.log("Value Index: " + valueIndex);
                console.log("Active Source: " + activeSource);
            }
        }

        return { config: prompt, realSource: activeSource };
    }





    async function loadList(sourceName, paramValue = "") {
        const key = paramValue ? `${sourceName}_${paramValue}`.toLowerCase() : sourceName.toLowerCase();
        if (activeFetches.has(key)) return;
        activeFetches.add(key);

        console.log(`Fetching list: ${sourceName} params: ${paramValue}`);

        try {
            const data = await getList(sourceName, paramValue);
            axDatasourceObj[key] = data;
            console.log(JSON.stringify(axDatasourceObj));
            handleInput();

        } catch (error) {
            console.error("loadlist failed", error);
        } finally {
            activeFetches.delete(key);


        }

    }


    function redirectToSmartView({ adsName, filters }) {


        // let targetUrl = "../CustomPages/Smartview_table_1769088257557.html";
        // let targetUrl = `${getAppBaseUrl()}/CustomPages/Smartview_table_1769088257557.html`;
        // let targetUrl = `${getAppBaseUrl()}/CustomPages/Smartview_table.html`;
        let targetUrl = `${getAppBaseUrl()}/plugins/Axi/HTMLPages/Smartview_table.html`;

        // let targetUrl = "../axidev/HTMLPages/Smartview_table_1769088257557.html";

        targetUrl += `?ads=${encodeURIComponent(adsName)}`;
        targetUrl += "&load=1769601086182";

        let encodedFilterQuery;

        if (filters && filters.length === 1 && (filters[0].datatype === "c" || filters[0].datatype === "d") && (!filters[0].value || filters[0].value === "")) {
            const columnName = filters[0].field;
            targetUrl += `&groupby=${encodeURIComponent(columnName)}`;
        }
        else {
            const payload = {

                filters: filters
            }

            encodedFilterQuery = btoa(JSON.stringify(payload));

            targetUrl += `&filter=${encodedFilterQuery}`;
        }

        console.log("Target Url for SmartViewTable:  " + targetUrl);
        /**====================================================================================
         * NOTE: This is Debug code remove it before deploying  to the  production environment 
         * ====================================================================================
         */
        if (typeof encodedFilterQuery !== "undefined") {
            try {
                const decodedForDebug = JSON.parse(atob(encodedFilterQuery));
                console.group("AXI SmartView Redirect Debug");
                console.log("Final URL:", targetUrl);
                console.log("Encoded q:", encodedFilterQuery);
                console.log("Decoded payload:", JSON.stringify(decodedForDebug));
                console.groupEnd();
            } catch (e) {
                console.error("AXI SmartView payload decode failed", e);
            }
        }


        /**
         * ===================== End ========================================
         */
        top.window.LoadIframe(targetUrl);


    }


    function redirectToPermissionScreeen(username) {

        const transId = "a__up";
        let targetUrl = "../aspx/tstruct.aspx";


        targetUrl += "?act=load";
        targetUrl += `&transid=${transId}`;


        if (username) {

            targetUrl += `&axusername=${username}`;




        }

        targetUrl += "&fromsource=U";


        targetUrl += "&openerIV=axusers";



        targetUrl += "&isIV=true";

        targetUrl += `&isDupTab=true`;



        targetUrl += "&dummyload=false";

        setEditSessionState(transId);
        console.log(`LoadIframe called with Url: ${targetUrl}`);


        top.window.LoadIframe(targetUrl);

    }



    function redirectToTstruct(transId, isEdit = false, fieldName = "", fieldValue = "") {
        console.log(`Redirecting to Tstruct: ${transId}, Edit: ${isEdit}, Field: ${fieldName}, Val: ${fieldValue}`);



        if (!transId) {
            alert("There is no Tstruct name provided!");
            return;
        }


        let targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (isEdit) {
            if (fieldName && fieldValue) {
                targetUrl += `&${fieldName}=${encodeURIComponent(fieldValue)}`;
            }
            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=${transId}`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;

            targetUrl += `&dummyload=false♠`;

        }
        else {
            if (fieldName && fieldValue) {
                targetUrl += `&${fieldName}=${encodeURIComponent(fieldValue)}`;
            }
            targetUrl += `&hltype=open`;
            targetUrl += `&dummyload=false`;
        }

        top.window.LoadIframe(targetUrl);
    }


    function redirectToResponsibilitiesPage(fieldValue = "") {


        let targetUrl = "../aspx/AddEditResponsibility.aspx";

        if (fieldValue) {
            targetUrl += "?status=true";
            targetUrl += "&action=edit";
            targetUrl += `&name=${encodeURIComponent(fieldValue)}`;
        } else {
            targetUrl += "?status=true";
            targetUrl += "&action=add";
        }


        top.window.LoadIframe(targetUrl);
    }

    function redirectToIView(iViewName) {
        console.log("Redirecting to Iview: " + iViewName + "..............");
        let targetUrl = `../aspx/iview.aspx?ivname=${iViewName}`;

        window.LoadIframe(targetUrl);


    }


    function redirectToProcessFlow(caption) {
        console.log(`Redirecting to Process flow for caption:  ${caption}`);




        let targetUrl;

        if (caption) {
            targetUrl = `../aspx/processflow.aspx`;
            targetUrl += "?loadcaption=AxProcessBuilder"
            targetUrl += `&processname=${encodeURIComponent(caption)}`;
        }
        else {
            targetUrl = `../aspx/tstruct.aspx?transid=ad_pm`;
        }


        top.window.LoadIframe(targetUrl);
    }



    /* ===============================
       2. INPUT HANDLER
    =============================== */
    function handleInput() {
        const text = input.value;

        if (axiClearBtn) {
            if (text.length > 0) {
                axiClearBtn.style.display = "flex";

            } else {
                axiClearBtn.style.display = "none";
            }
        }
        if (!commands) return;

        if (!text.trim()) {

            items = Object.keys(commands);
            hintDiv.textContent = "";
            render();
            return;
        }
        
        ///set command - numeric handling.
        if (SET_COMMAND_STATE.currentFieldType === 'n') {

            let tokens = getTokens(text);
            let lastIndex = tokens.length - 1;
            let lastToken = tokens[lastIndex];

            const numericRegex = /^-?\d*$/;

            if (!numericRegex.test(lastToken)) {
                console.error("Type only numeric value");
                showToast("Type only numeric value");

                tokens[lastIndex] = "";

                input.value = tokens.join(" ");
                return;
            }
        }


        // Clear stale resolutions when input changes
        const currentTokens = getTokens(text);
        currentTokens.forEach((token, idx) => {
            const cleanToken = token.replace(/"/g, "");
            const lastToken = lastTypedTokens[idx] ? lastTypedTokens[idx].replace(/"/g, "") : null;

            if (lastToken && cleanToken !== lastToken && resolvedParams[idx]) {
                console.log(`Token changed at position ${idx}: "${lastToken}" → "${cleanToken}"`);
                delete resolvedParams[idx];
                Object.keys(resolvedParams).forEach(key => {
                    if (parseInt(key) > idx) {
                        delete resolvedParams[key];
                        console.log(`Cleared dependent resolution at index ${key}`);
                    }
                });
            }
        });

        if (currentTokens.length < lastTypedTokens.length) {
            for (let i = currentTokens.length; i < lastTypedTokens.length; i++) {
                if (resolvedParams[i]) {
                    delete resolvedParams[i];
                    console.log(`Cleared deleted token resolution at index ${i}`);
                }
            }
        }

        lastTypedTokens = [...currentTokens];
        items = suggestLocal(text);


        render();
    }

    /* ===============================
       3. TOKENIZER
    =============================== */


    function getTokens(str) {


        const regex = new RegExp(`"[^"]*"?|${OPERATOR_REGEX_PART}|[^\\s=<>!]+`, "g");
        return str.match(regex) || [];
    }

    function cleanString(val) {
        return (val || "").replace(/['"]/g, "").trim();
    }







    function processAdsRepetitiveTokens(tokens, commandConfig) {
        const targetIndex = tokens.length - 1;
        const partialTyped = cleanString(tokens[targetIndex]);
        const adsName = cleanString(tokens[1]);


        if (targetIndex < 2) return [];


        if (targetIndex % 2 === 0) {
            const sourceName = "axi_adscolumnlist";
            const sourceKey = `${sourceName}_${adsName}`.toLowerCase();


            if (!axDatasourceObj[sourceKey]) {
                loadList(sourceName, adsName);
                return ["Loading columns..."];
            }

            const list = axDatasourceObj[sourceKey];
            if (!Array.isArray(list)) return [];


            const usedColumns = new Set();
            for (let i = 2; i < targetIndex; i += 2) {
                const usedToken = cleanString(tokens[i]).toLowerCase();
                usedColumns.add(usedToken);
            }






            const filtered = list.filter(col => {

                const rawDisplay = (col.displaydata || col.name).toLowerCase();


                const cleanDisplay = rawDisplay
                    .replace(/\s*\(.*?\)/g, "")
                    .replace(/\s*\[[^\]]+\]\s*$/, "")
                    .trim();

                const rawName = (col.name || "").toLowerCase();


                const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);


                const matchesInput = cleanDisplay.includes(partialTyped.toLowerCase());

                return !isUsed && matchesInput;
            });




            filteredObjects = [goOption, ...filtered];
            if (tokens.length > 2) {
                return [
                    goOption,
                    ...filtered.map(col => col.displaydata || col.name)
                ];

            }

        }


        else {

            const prevColumnName = cleanString(tokens[targetIndex - 1]);


            const colSourceKey = `axi_adscolumnlist_${adsName}`.toLowerCase();
            const colList = axDatasourceObj[colSourceKey];

            if (!colList) return [];

            const columnMetadata = colList.find(
                c =>
                    c.name?.toLowerCase() === prevColumnName.toLowerCase() ||
                    c.displaydata?.toLowerCase().replace(/\s*\(.*?\)/g, '').trim() === prevColumnName.toLowerCase()
            ) || null;

            if (!columnMetadata) return [];

            const isAccept = !columnMetadata.sourcetable || !columnMetadata.sourcefld;


            const datatype = columnMetadata.fdatatype;


            if (isAccept) {
                const acceptedValue = cleanString(tokens[tokens.length - 1]);
                const columnName = prevColumnName
                adsfieldvalueanddt[columnName] = {
                    datatype: datatype,
                    isAccept: isAccept,
                };
                if (tokens?.length <= 4) {

                    return [
                        goOption,

                    ];


                }
                return [];
            }
            else {

                if (!columnMetadata.sourcetable || !columnMetadata.sourcefld) {
                    console.log("Error in DropDownField check: sourcetable or sourcefld is empty");
                    return [];
                }

                const acceptedValue = cleanString(tokens[tokens.length - 1]);
                const columnName = prevColumnName
                adsfieldvalueanddt[columnName] = {
                    datatype: datatype,
                    isAccept: isAccept,
                };

                const sourcetable = columnMetadata.sourcetable;
                const sourcefld = columnMetadata.sourcefld;

                const sourceName = "axi_adsdropdowntokens";
                const paramValue = `${sourcetable}$#$${sourcefld}`;
                const sourceKey = `${sourceName}_${paramValue}`.toLowerCase();

                if (!axDatasourceObj[sourceKey]) {
                    loadList(sourceName, paramValue);
                    return ["Loading values..."];
                }

                const list = axDatasourceObj[sourceKey];
                if (!Array.isArray(list)) return [];


                const filtered = list.filter(col => {
                    const rawDisplay = String(col.displaydata || col.name)
                        .toLowerCase();

                    const normalizedTypedValue = (acceptedValue ?? "")
                        .toLowerCase();

                    return !normalizedTypedValue || rawDisplay.includes(normalizedTypedValue);
                });





                if (tokens.length <= 4) {
                    filteredObjects = [goOption, ...filtered];
                    return [
                        goOption,
                        ...filtered.map(col => col.displaydata || col.name)
                    ];

                }

                filteredObjects = filtered


                return filtered.map(col => col.displaydata || col.name);

            }

        }
    }

    function processRunCommands(tokens, targetIndex, structType) {
        if (targetIndex !== 1) return [];
        let allButtons


        const partialTyped = cleanString(tokens[targetIndex]);

        switch (structType) {
            case "t":
            case "i":

                const isDesign = isTstructDesignMode();
                if (isDesign) {
                    designModeToolbarButtons = getDesignModeToolbarButtons();

                    allButtons = { ...designModeToolbarButtons }


                } else {
                    bottomToolbarButtons = getBottomToolbarButtons();
                    topToolbarButtons = getTopToolbarButtons();
                    allButtons = { ...bottomToolbarButtons, ...topToolbarButtons };

                }


                break;

            case "e":
            case "ef":
            case "c":
                entityToolbarButtons = getEntityToolbarButtons();
                allButtons = { ...entityToolbarButtons };
                break;

            case "pf":
                pfToolbarButtons = getPFToolbarButtons();
                allButtons = { ...pfToolbarButtons }
                break;


            default:
                console.error("Invalid StructType")
                break;
        }

        buttonsList = Object.values(allButtons).map(btn => ({
            name: btn.id,
            displaydata: `${btn.label} (${btn.id})`
        }));

        const filtered = buttonsList.filter(item =>
            item.displaydata.toLowerCase().includes(partialTyped.toLowerCase())
        );

        filteredObjects = filtered;


        if (structType === "o") {
            return [];
        }

        return filtered.map(item => item.displaydata);

    }

    /* ===============================
        Suggestion Logic
    =============================== */

    function suggestLocal(inputText) {
        let ignoreExtraParams = false;
        let detectedType = "";
        const tokens = getTokens(inputText);
        const endsWithSpace = inputText.endsWith(" ");


        const lastTokenRaw = tokens[tokens.length - 1];
        const isUnclosedString = lastTokenRaw && lastTokenRaw.startsWith('"') && (!lastTokenRaw.endsWith('"') || lastTokenRaw === '"');
        if (endsWithSpace && !isUnclosedString) tokens.push("");

        if (tokens.length === 0) {
            hintDiv.textContent = "";
            return Object.keys(commands);
        }

        const groupKey = cleanString(tokens[0]);
        if (tokens.length === 1 && !endsWithSpace) {
            hintDiv.textContent = "";
            return Object.keys(commands).filter(k => k.startsWith(groupKey));
        }

        const commandConfig = commands[groupKey];
        if (!commandConfig) { hintDiv.textContent = ""; return []; }

        const targetIndex = tokens.length - 1;

        if (commandConfig.commandGroup?.toLowerCase() === "view") {
            const viewSource = commandConfig?.prompts?.[0]?.promptSource?.toLowerCase();
            const viewValues = commandConfig.prompts?.[0]?.promptValues;
            const firstToken = cleanString(tokens[1] || "");
            detectedType = getType(viewSource.toLowerCase(), firstToken, viewValues);

            if (detectedType === "ads") {
                ignoreExtraParams = true;
                if (tokens.length > 2) {
                    updateDynamicHintFromPrompt({ prompt: (targetIndex % 2 === 0) ? "column" : "value" });
                    return processAdsRepetitiveTokens(tokens, commandConfig)

                }

                console.log("ResolutionContext: ignoreExtraParams = true (ADS)")
            }
        }

        ///Need to make a common function for processAdsRepetitivetokens
        if (groupKey === "create" && tokens.length > 3) {
            let viewSource = commandConfig?.prompts?.[2]?.promptSource?.toLowerCase();
            //let viewSource = "Axi_FieldList".toLowerCase();
            let tokenCopy = [...tokens];
            //let dummyValue = commandConfig?.prompts?.[1]?.promptValues.toLowerCase();
            //let orgTokens = [...tokens];
            //if (dummyValue) {
            //    tokenCopy = tokenCopy.filter(t => t?.toLowerCase() !== dummyValue.toLowerCase());
            //}
            updateDynamicHintFromPrompt({ prompt: (targetIndex % 2 !== 0) ? "fieldname": "fieldValue"})
            return createCommandHandling(tokenCopy, commandConfig, viewSource);
        }
            
        if (groupKey === "run") {
            const structType = getStructType();

            if (!structType || structType === "o") {
                showToast("Warning: CommandGroup Invalid: Please open Tstruct or Any other page");
                return [];
            }



            return processRunCommands(tokens, targetIndex, structType);
        }
        const promptInfo = getActivePromptInfo(commandConfig, tokens, targetIndex);


        ///KeyValue based edit handling.

        if ((!promptInfo || tokens[3] === "with" )&& groupKey === "edit" && tokens.length >= 4) {

            let viewSource;
            if (tokens.length == 4) {
                viewSource = commandConfig?.prompts?.[3]?.promptValues?.toLowerCase().split(',').map(v => v.trim());
                //const partialTyped = cleanString(tokens[targetIndex]);
                const result = viewSource.filter(val => val.toLowerCase());
                filteredObjects = result.map(val => ({ name: val, displaydata: val }));

                result.unshift(goOption);
                filteredObjects.unshift(goOption);
                updateDynamicHintFromPrompt({ prompt: commandConfig?.prompts?.[3]?.prompt })
                return result;


            }
            else if (tokens.length >= 5) {
                viewSource = commandConfig?.prompts?.[4]?.promptSource?.toLowerCase()

                let tokenCopy = [...tokens];

                updateDynamicHintFromPrompt({ prompt: (targetIndex % 2 === 0) ? "fieldname" : "fieldValue" })
                return editCommandHandling(tokenCopy, commandConfig, viewSource);


            }

        }

        if (!promptInfo) {
            updateDynamicHintFromPrompt(null);
            return [];
        }






        const { config: activePrompt, realSource } = promptInfo;
        updateDynamicHintFromPrompt(activePrompt);

        const partialTyped = cleanString(tokens[targetIndex]);



        // Scenario A: Static Values

        ///Skipping PromptValue "With" token for edit
        if (!realSource && activePrompt.promptValues && groupKey!== "edit") {
            const staticValues = activePrompt.promptValues.split(',').map(v => v.trim());
            const result = staticValues.filter(val => val.toLowerCase().startsWith(partialTyped.toLowerCase()));
            filteredObjects = result.map(val => ({ name: val, displaydata: val }));

            if (groupKey === "create" && tokens.length === 3) {
                result.unshift(goOption);
                filteredObjects.unshift(goOption);
            }
            return result;
        }

        // Scenario B: Data Source
        if (realSource) {
            let paramValue = "";

            // Resolve Standard Dependencies 
            if (activePrompt.promptParams) {
                const indices = activePrompt.promptParams.toString().split(',');
                const values = indices.map(idx => {
                    const logicalWordPos = parseInt(idx.trim());
                    const depTokenIndex = logicalWordPos - 1;
                    const depToken = cleanString(tokens[depTokenIndex] || "");
                    return tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                });
                paramValue = values.join('$#$');
            }




            if (activePrompt.extraParams) {


                if (!paramValue || paramValue.trim() === "") {
                    console.log("Skipping extraParams – dependency not resolved yet");

                } else {
                    const extraSource = activePrompt.extraParams.toLowerCase();

                    const extraKey = `${extraSource}_${paramValue}`.toLowerCase();


                    if (!axDatasourceObj[extraKey]) {

                        console.log(`Fetching Hidden Param Source: ${extraSource}`);
                        loadList(extraSource, paramValue);
                        return [];
                    }

                    // Extra List is cached, extract Index 0
                    const extraList = axDatasourceObj[extraKey];
                    if (extraList && extraList.length > 0) {
                        const hiddenValue = extraList[0].name || extraList[0].displaydata || extraList[0].fname || extraList[0].keyfield;
                        console.log(`Hidden Param Found (Index 0): ${hiddenValue}`);

                        // Append hidden value to params for the MAIN list
                        if (paramValue) paramValue += "$#$" + hiddenValue;
                        else paramValue = hiddenValue;
                    } else {
                        return [];
                    }

                }

            }


            let apiSourceName = realSource.toLowerCase();
            if (apiSourceName.toLowerCase() === "axi_analyticslist") {
                paramValue = window.mainUserName;
            }
            const sourceKey = (paramValue ? `${apiSourceName}_${paramValue}` : apiSourceName).toLowerCase();

            if (!axDatasourceObj[sourceKey]) {
                const hasValidParams = !activePrompt.promptParams || (paramValue && paramValue.replace(/,/g, '').trim().length > 0);
                if (hasValidParams) {
                    loadList(apiSourceName, paramValue);
                    console.log(axDatasourceObj);
                    return [`Loading ${realSource}...`];
                }
                return ["Waiting for input..."];
            }

            // Filter Cache
            const dataList = axDatasourceObj[sourceKey];
            const filtered = dataList.filter(item => {
                const display = item.displaydata || item.caption || item.name || "";
                return display.toLowerCase().includes(partialTyped.toLowerCase());
            });

            filteredObjects = filtered;

            let resultList = filtered.map(item => item.displaydata || item.caption || item.name || item.fname || item.keyfield);

            if ((groupKey === "view" || groupKey === "configure") && tokens.length > 2 && tokens[1] !== "keyfield") {
                resultList.unshift(goOption);
                filteredObjects.unshift(goOption);
            }

            else if (groupKey === "analyse" && tokens.length <= 3) {
                resultList.unshift(goOption);
                filteredObjects.unshift(goOption);
            }

            //else if (groupKey === "create" && tokens.length == 3) {
            //    resultList.unshift(goOption);
            //    filteredObjects.unshift(goOption);
            //}

            else if (groupKey === "edit" &&  tokens.length > 4) {
                resultList.unshift(goOption);
                filteredObjects.unshift(goOption);
            }

            return resultList;
        }

        return [];
    }





    function tryResolveToken(tokenIndex, tokenText, commandConfig, forceResolve = false) {
        tokenText = cleanString(tokenText);
        if (!tokenText) return "";

        if (resolvedParams[tokenIndex] && !forceResolve) return resolvedParams[tokenIndex];
        // if (!tokenText && !forceResolve) return "";
        if (!commandConfig) return tokenText;


        const currentTokens = getTokens(input.value);


        const promptInfo = getActivePromptInfo(commandConfig, getTokens(input.value), tokenIndex);
        if (!promptInfo) return tokenText;

        const { config: prompt, realSource } = promptInfo;


        if (realSource) {
            let paramValue = "";


            // Resolve Dependencies
            if (prompt.promptParams) {
                const indices = prompt.promptParams.toString().split(',');
                const values = indices.map(idx => {
                    const logicalWordPos = parseInt(idx.trim());
                    const depTokenIndex = logicalWordPos - 1;
                    const depToken = cleanString(getTokens(input.value)[depTokenIndex] || "");
                    return tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                });
                paramValue = values.join('$#$');
            }

            // Append Hidden Param for Resolution Context
            if (prompt.extraParams) {
                const extraSource = prompt.extraParams;
                const extraKey = `${extraSource}_${paramValue}`.toLowerCase();
                const extraList = axDatasourceObj[extraKey];

                if (extraList && extraList.length > 0) {
                    const hiddenValue = extraList[0].name || extraList[0].keyfield || extraList[0].fname || extraList[0].displaydata;
                    
                    if (paramValue) paramValue += "$#$" + hiddenValue;
                    else paramValue = hiddenValue;
                }
            }

            let apiName = realSource;
            if (apiName.toLowerCase() === "axi_analyticslist") {
                paramValue = window.mainUserName;
            }
            let cacheKey = paramValue ? `${apiName}_${paramValue}` : apiName;

            const cachedList = axDatasourceObj[cacheKey.toLowerCase()];
            if (cachedList) {
                const found = cachedList.find(item =>
                    (item.displaydata || "").toLowerCase() === tokenText.toLowerCase() ||
                    (item.caption || "").toLowerCase() === tokenText.toLowerCase() ||
                    (item.name || "").toLowerCase() === tokenText.toLowerCase()
                );
                if (found) {
                    let real = found.name  || found.sqlname || found.displaydata;

                    if (real.includes("(") && real.includes(")")) {
                        const match = real.match(/\(([^)]+)\)/);

                        real = match ? match[1] : real;

                    }

                    resolvedParams[tokenIndex] = real;
                    return real;
                }
            }
        }

        return tokenText;
    }


    /* ===============================
       RENDER & APPLY
    =============================== */
    function render() {

        console.log("Render called");
        list.innerHTML = "";

        if (items.length > 0 && isSystemMessage(items[0])) {
            activeIndex = -1;
        } else {
            activeIndex = 0;
        }


        const validItems = items.filter(item => {
            if (!item) return false;
            if (typeof item === "string") {
                return (item !== "Loading options..." && item.trim() !== "");
            }
            if (typeof item === "object") {
                return (typeof item.displaydata === "string" && item.displaydata.trim() !== "");
            }
            return false;
        });

        console.log(`Valid Items: ${validItems.length}`);

        if (validItems.length === 0) {
            const li = document.createElement("li");
            li.textContent = "No Data";
            li.className = "no-data-axi-suggestion";
            li.style.color = "#FF0000";
            li.style.textAlign = "left";
            li.style.padding = "12px";

            list.appendChild(li);
            list.style.display = "block";
            return;
        }

        validItems.forEach((item, i) => {
            const li = document.createElement("li");
            const text = typeof item === "string" ? item : item.displaydata;
            li.textContent = text;
            li.className = "axi-suggestion";

            if (typeof item === 'object' && item.isExecutable) {
                li.style.fontWeight = "bold";
                li.style.color = "#22c55e";
                li.style.borderBottom = "1px solid #eee";
            }

            if (i === activeIndex) {
                li.classList.add("active");
            }

            li.addEventListener("mousedown", e => {
                e.preventDefault();
                apply(i);
            });

            list.appendChild(li);
        });

        list.style.display = "block";
    }

    function hide() {
        list.style.display = "none";
        items = [];
        activeIndex = -1;
    }

    function GetObjectName(selectedValue) {
        const foundObj = filteredObjects.find(item => item.displaydata === selectedValue);
        if (foundObj) {
            return foundObj.name || foundObj.sqlname || foundObj.displaydata;
        }
        return selectedValue;
    }

    function apply(index) {

        if (!items[index] || isSystemMessage(items[index])) return;

        const selectedItem = items[index];


        const currentInput = input.value;
        const tokens = getTokens(currentInput);

        const saveGroupKeyCheck = cleanString(tokens[0]).toLowerCase();
        const saveCommandConfig = commands[saveGroupKeyCheck];

        if (typeof selectedItem === 'object' && selectedItem.isExecutable && selectedItem.name === "GO_ACTION") {
            console.log("Action item selected. Executing command...");
            hide();
            executeCommandsV2();
            return;
        }
        else if (typeof selectedItem === 'object' && selectedItem.isExecutable && selectedItem.name === "Save_ACTION" && saveGroupKeyCheck === "create") {
            console.log("Save Option Selected...Submitting Data...");
            hide();
            AxisaveDataFn(createfieldnamevaluesList, SET_COMMAND_STATE.transid, "axi_fieldlist", true, tokens, saveCommandConfig);
            resetSetCommandState();
            return;
        }
        else if (typeof selectedItem === 'object' && selectedItem.isExecutable && selectedItem.name === "Save_ACTION" && saveGroupKeyCheck === "edit") {
            console.log("Save Option Selected...Submitting Data...");
            hide();
            AxisaveDataFn(createfieldnamevaluesList, SET_COMMAND_STATE.transid, "axi_fieldlist", false, tokens, saveCommandConfig);
            resetSetCommandState();
            return;
        }

        const endsWithSpace = currentInput.endsWith(" ");
        const lastTokenRaw = tokens[tokens.length - 1];

        const isUnclosedString = lastTokenRaw && lastTokenRaw.startsWith('"') && (!lastTokenRaw.endsWith('"') || lastTokenRaw === '"');


        if (endsWithSpace && !isUnclosedString) {
            tokens.push("");
        }

        let targetIndex = tokens.length - 1;
        if (targetIndex < 0) {
            targetIndex = 0;
            tokens.push("");
        }

        let suggestion = items[index];
        let displayName = suggestion;
        let realValue = "";

        const isViewCommand = tokens[0]?.toLowerCase() === "view";




        let isAdsValue = false;
        /**
         * ==== Robust Type checking for View command ===
         */
        if (isViewCommand && commands) {
            const groupKey = cleanString(tokens[0]);
            const commandConfig = commands[groupKey];

            if (commandConfig && commandConfig.prompts && commandConfig.prompts[0]) {
                const viewSource = commandConfig.prompts[0].promptSource;
                const viewValues = commandConfig.prompts[0].promptValues;
                const firstToken = cleanString(tokens[1] || "");

                const detectedType = getType(viewSource.toLowerCase(), firstToken, viewValues);

                if (detectedType === "ads" && targetIndex >= 3 && targetIndex % 2 !== 0) {
                    isAdsValue = true;
                }
            }
        }



        // Get Real Value logic
        const foundObj = filteredObjects.find(item => item.displaydata === suggestion);


        realValue = foundObj ? (foundObj.name  || foundObj.sqlname || foundObj.displaydata) : suggestion;





        if (suggestion.includes("(") && suggestion.includes(")") && !isAdsValue) {
            const lastBracketIndex = suggestion.lastIndexOf("(");


            const text = suggestion.substring(0, lastBracketIndex)

                .trim();


            displayName = text.replace(/-$/, "");





        }

        if (displayName.includes("[") && displayName.includes("]") && !isAdsValue) {
            const lastBracketIndex = suggestion.lastIndexOf("[");
            const text = suggestion.substring(0, lastBracketIndex).trim();

            displayName = text.replace(/\s*\[[^\]]*\]$/, "").trim();
        }

        resolvedParams[targetIndex] = realValue;

        displayName = displayName.replace(/[\r\n]+/g, " ").trim();

        // Auto-Quote if necessary
        if (displayName.includes(" ")) {
            displayName = `"${displayName}"`;
        }

        tokens[targetIndex] = displayName;

        input.value = tokens.join(" ") + " ";

        lastTypedTokens = [...tokens];
        handleInput();

        input.focus();
    }



    function updateDynamicHintFromPrompt(prompt) {

        if (prompt) {
            let label = prompt.prompt || "value";
            if (prompt.promptValues && !prompt.prompt) {
                // label = prompt.promptValues.split(',').join(' / ');
                label = prompt.promptValues.split(',').slice(0, 3);
            }
            hintDiv.textContent = `Next: <${label}>`;
            hintDiv.style.color = "#f59e0b";
        } else {
            hintDiv.innerHTML = `
    <span style="display: inline-flex; align-items: center; gap: 4px; margin-top: 4px; padding-right: 12px;">
        <svg xmlns="http://www.w3.org/2000/svg" 
             width="16" height="16" 
             viewBox="0 0 24 24" 
             fill="none" 
             stroke="currentColor" 
             stroke-width="2" 
             stroke-linecap="round" 
             stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"></polyline>
        </svg>
        Ready to Run
    </span>
`;

            hintDiv.style.color = "#22c55e";


        }
    }





    document.addEventListener("click", e => {
        if (input && list && e.target !== input && !list.contains(e.target)) {
            hide();
        }
        if (list.style.display === "block" && searchWrapper && !searchWrapper.contains(e.target)) {
            hide();
        }
    });



    async function getAxListAsync(data) {
        return new Promise((resolve, reject) => {
            window.GetDataFromAxList(
                data,
                res => resolve(res),
                err => reject(err)
            );
        });

    }

    /* Generic get List function */
    async function getList(axDatasourceName, paramValuesCsv = "") {
        try {
            //await ensureSignedIn();
            if (!axDatasourceName) {
                throw new Error("axDatasourceName is required");
            }


            // ---- Build sqlparams dynamically ----
            const sqlParams = {};
            const normalizedParams = [];



            if (paramValuesCsv && typeof paramValuesCsv === "string") {
                //const values = paramValuesCsv
                //    .split(",")
                //    .map(v => v.trim())
                //    .filter(Boolean);
                const values = paramValuesCsv
                    .split("$#$")
                    .map(v => v.trim())
                    .filter(Boolean);

                values.forEach((value, index) => {
                    const key = `param${index + 1}`;
                    sqlParams[key] = value;
                    normalizedParams.push(`${key}:${value}`);
                });
            }

            // ---- Stable cache key ----
            const cacheKey = `axi_${axDatasourceName}_${normalizedParams.join("|")}_v1`;

            const cached = localStorage.getItem(cacheKey);
            if (cached) {
                return JSON.parse(cached);
            }

            const requestBody = {

                action: "view",
                adsNames: [axDatasourceName],
                trace: true,
                refreshCache: true,
                sqlParams: sqlParams
            };



            const res = await getAxListAsync(requestBody);

            console.log("Get List data: " + JSON.stringify(res));




            const dataObj = typeof res === "string" ? JSON.parse(res) : res;

            console.log("DATA obj is : " + dataObj);
            console.log("Type of DATA OBJ: " + typeof dataObj);
            const list = dataObj?.result?.data?.[0]?.data ?? [];

            if (dataObj?.result?.data?.[0].error) {
                showToast(`Error: ${dataObj?.result?.data?.[0].error}`);
                console.log(`Error: ${list[0].error}`);
                return;

            }


            if (list.length > 0) {
                localStorage.setItem(cacheKey, JSON.stringify(list));

            } else console.log(`List Data for Ads name ${axDatasourceName} is Empty`);


            return list;

        } catch (err) {
            return [];
        }
    }


    /* ===============================
       TOAST HELPER
    =============================== */
    function showToast(message, duration = 5000, isSuccess = false) {

        const toast = document.createElement("div");

        const textSpan = document.createElement("span");
        textSpan.textContent = message;
        textSpan.style.flexGrow = "1";
        textSpan.style.marginRight = "15px";


        const closeBtn = document.createElement("span");
        closeBtn.innerHTML = "&times;";
        closeBtn.style.cursor = "pointer";
        closeBtn.style.fontWeight = "bold";
        closeBtn.style.fontSize = "20px";
        closeBtn.style.lineHeight = "1";


        closeBtn.onclick = () => {
            toast.style.opacity = "0";
            setTimeout(() => {
                if (document.body.contains(toast)) {
                    document.body.removeChild(toast);
                }
            }, 300);
        };
        const bgColor = isSuccess ? "rgba(34, 197, 94, 0.9)" : "rgba(239, 68, 68, 0.9)";



        Object.assign(toast.style, {
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            padding: "10px",
            position: "fixed",
            bottom: "50px",
            right: "20px",
            minWidth: "300px",
            width: "fit-content",
            backgroundColor: bgColor,
            color: "white",
            padding: "12px 24px",
            borderRadius: "8px",
            boxShadow: "0 4px 6px rgba(0,0,0,0.1)",
            zIndex: "10000",
            fontFamily: "sans-serif",
            fontSize: "14px",
            opacity: "0",
            transition: "opacity 0.3s ease-in-out",
            backdropFilter: "blur(4px)"
        });

        document.body.appendChild(toast);
        toast.appendChild(textSpan);
        toast.appendChild(closeBtn);


        requestAnimationFrame(() => {
            toast.style.opacity = "1";
        });


        setTimeout(() => {
            toast.style.opacity = "0";
            setTimeout(() => {
                if (document.body.contains(toast)) {
                    document.body.removeChild(toast);
                }
            }, 300);
        }, duration);
    }




    function isSuggestionVisible() {
        return list && list.style.display === "block" && items.length > 0;
    }

    function hasActiveSuggestion() {
        return activeIndex >= 0 && activeIndex < items.length;
    }


    /* ===============================
       SETUP LISTENERS
    =============================== */
    function setupEventListeners() {
        if (btnRefresh) {
            btnRefresh.addEventListener("click", async () => {
                console.log("Refresh Logic......");

                try {
                    clearAxiLocalStorage("axi_");
                    commands = null;
                    tstructList = null;
                    adsList = null;
                    axDatasourceObj = {};
                    resolvedParams = {};
                    createfieldnamevaluesList = [];
                    resetSetCommandState();

                    await initCommands(true);

                    showToast("Refreshed Successfully!", 5000, true);
                    input.focus();

                } catch (error) {
                    console.log("Refresh Failed: " + error);
                    alert("Error refreshing: " + error);

                }


            });
        }



        if (axiClearBtn) {
            axiClearBtn.addEventListener("click", () => {
                input.value = "";
                handleInput();
                input.focus();
            })
        }

        if (axiLogo) {
            axiLogo.addEventListener("click", (e) => {
                e.stopPropagation();
                e.preventDefault();
                input.value = "";
                handleInput();
                input.focus();
            })
        }



        if (runBtn) {
            runBtn.addEventListener("click", executeCommandsV2);
        }

        input.addEventListener("focus", () => {
            if (input.value.trim() === "") {
                handleInput();
            }
        });

        input.addEventListener("click", () => {
            if (input.value.trim() === "" && list.style.display === "none") {
                handleInput();
            }
        })

        input.addEventListener("input", handleInput);
        input.addEventListener("blur", () => setTimeout(() => { if (!input.value) hintDiv.textContent = ""; }, 200));
        input.addEventListener("keydown", e => {
            console.log("Keys: " + e.key + "Code: " + e.code + "Alt: " + e.altKey);

            const tokens = getTokens(input.value.trim());

            const grpKey = tokens[0];

            let saveCommandConfig;
            if(grpKey)
              saveCommandConfig = commands[grpKey];

            if (e.key === 'Backspace' && (grpKey === "create" || grpKey === "edit") ){
                if (input.selectionStart !== input.selectionEnd) {
                    resetSetCommandState(); 
                    createfieldnamevaluesList = [];
                    return; 
                }
                e.preventDefault();
                hide();

                const cursorPos = input.selectionStart;

                tokens.pop();

                if (input.value[cursorPos - 1] === " ") { 

                    console.log("Deleted a space using Backspace");
                }
                else {
                    createfieldnamevaluesList.pop();
                 
                }
                input.value = tokens.join(" ");

                resetSetCommandState();
                

            }

            if (e.ctrlKey && e.code === "Space") {
                e.preventDefault();
                handleInput();
                return;
            }



            if (e.key === "Enter") {
                e.preventDefault();

                if (e.ctrlKey) {
                    hide();
                    executeCommandsV2();
                    return;
                }



                if (isSuggestionVisible() && hasActiveSuggestion()) {
                    e.preventDefault();
                    if (!isSystemMessage(items[activeIndex])) {
                        apply(activeIndex);


                    }
                    return;
                }



                executeCommandsV2();
                hide();
                return;




            }

            // AUTO DOUBLE-QUOTE FOR MULTI-WORD SUGGESTIONS
            if (e.key === " " && items.length > 0) {
                const val = input.value;
                if (input.selectionStart === val.length) {
                    const tokens = getTokens(val);
                    const lastTokenRaw = tokens[tokens.length - 1] || "";

                    if (!lastTokenRaw.startsWith('"')) {
                        const hasMultiWordMatch = items.some(item => {
                            const str = (typeof item === 'string' ? item : item.displaydata).toLowerCase();
                            return str.startsWith(lastTokenRaw.toLowerCase()) && str.includes(" ");
                        });

                        if (hasMultiWordMatch) {
                            e.preventDefault();
                            const lastIndex = val.lastIndexOf(lastTokenRaw);
                            if (lastIndex !== -1) {
                                const prefix = val.substring(0, lastIndex);
                                input.value = prefix + '"' + lastTokenRaw + ' ';
                                handleInput();
                                return;
                            }
                        }
                    }
                }
            }

            // ---------------------------------------------------
            if (list.style.display !== "block" || items.length === 0) return;
            if (e.key === "ArrowDown") { e.preventDefault(); activeIndex = (activeIndex + 1) % items.length; highlight(); }
            if (e.key === "ArrowUp") { e.preventDefault(); activeIndex = (activeIndex - 1 + items.length) % items.length; highlight(); }
            if (e.key === "Tab") { e.preventDefault(); if (activeIndex === -1) activeIndex = 0; apply(activeIndex); }
            if (e.key === "Enter" && activeIndex >= 0) {
                e.preventDefault();

                apply(activeIndex);



            }
            if (e.key === "Escape") {
                e.preventDefault();

                hide();
            }

            if (e.ctrlKey && e.key.toLowerCase() === "s") {
                e.preventDefault();
                console.log("Save Option Selected...Submitting Data...");
                hide();
                if (grpKey === "create")
                    AxisaveDataFn(createfieldnamevaluesList, SET_COMMAND_STATE.transid, "axi_fieldlist", true, tokens, saveCommandConfig);
                else
                    AxisaveDataFn(createfieldnamevaluesList, SET_COMMAND_STATE.transid, "axi_fieldlist", false, tokens, saveCommandConfig);
                resetSetCommandState();
                return;
            }
        });

        document.addEventListener("click", e => {
            if (input && list && e.target !== input && !list.contains(e.target)) hide();
        });


        const iframe = document.getElementById("middle1");
        if (iframe) {
            const attachIframeClick = () => {
                try {
                    const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;

                    iframeDoc.removeEventListener("click", hide);
                    iframeDoc.addEventListener("click", () => {
                        hide();
                    });
                } catch (err) {
                    console.warn("Could not attach click listener to iframe (likely CORS restriction):", err);
                }
            };

            // Attach immediately if already loaded
            attachIframeClick();


            iframe.addEventListener("load", attachIframeClick);
        }
    }

    function clearAxiLocalStorage(prefix) {
        const keysToRemove = [];
        for (let i = 0; i < localStorage.length; i++) {
            const key = localStorage.key(i);

            if (key && key.startsWith(prefix)) {
                keysToRemove.push(key);
            }
        }

        keysToRemove.forEach(key => localStorage.removeItem(key));

        console.log(`Cleared ${keysToRemove.length} keys starting with ${prefix}`);

    }

    function highlight() {
        if (list.children.length > 0) {
            [...list.children].forEach((li, i) => li.classList.toggle("active", i === activeIndex));

            const activeItem = list.children[activeIndex];
            if (activeItem) {
                const itemTop = activeItem.offsetTop;
                const itemBottom = itemTop + activeItem.clientHeight;
                const listScrollTop = list.scrollTop;
                const listHeight = list.clientHeight;


                if (itemBottom > listScrollTop + listHeight) {
                    list.scrollTop = itemBottom - listHeight;
                }

                else if (itemTop < listScrollTop) {
                    list.scrollTop = itemTop;
                }
            }
        }
    }



    /**
     * Execute the Commands 
     * @returns 
     */
    function executeCommandsV2() {



        const text = input.value.trim();



        if (!text || !commands) return;



        const tokens = getTokens(text);
        
        if (tokens.length === 0) return;

        const groupKey = cleanString(tokens[0]);





        const groupConfig = commands[groupKey];

        if (!groupConfig) {
            console.warn(`Unknown command group: ${groupKey}`);

        }

        // Build the context object to pass to the dispatcher
        const context = {
            text: text,
            tokens: tokens,
            group: groupKey,
            config: groupConfig,
            resolvedParams: resolvedParams
        };

        dispatchCommand(context);
        hide();

        setTimeout(() => {
            input.focus();
            input.select();
        }, 200)
    }


    function cleanCommandToken(val = "") {
        return val.replace(/['"]/g, "").trim();
    }

    function dispatchCommand(ctx) {
        const { group, config, tokens } = ctx;



        const firstParamPrompt = config?.prompts?.find(p => p.wordPos === 2);
        const firstParamValue = cleanString(tokens[1]);

        let handlerKey = 'default';

        if (firstParamPrompt && firstParamPrompt?.promptValues) {

            if (firstParamValue) {
                handlerKey = firstParamValue?.toLowerCase();
            }
        }

        // Locate the handler function in the mapping
        const groupHandlers = COMMAND_HANDLERS[group];

        if (!groupHandlers) {
            console.error(`System Error: No handlers object defined for command group '${group}'`);
            return;
        }


        const handler = groupHandlers[handlerKey] || groupHandlers['default'];

        if (!handler) {
            console.error(`Dispatch Error: No handler function found for '${group}' -> '${handlerKey}'`);
            return;
        }

        console.log(`Dispatching to: ${group}.${handlerKey}`);


        try {
            handler({
                tokens: tokens,
                commandConfig: config,
                resolvedParams: resolvedParams
            });
        } catch (err) {
            console.error(`Error executing handler for ${group}.${handlerKey}:`, err);
        }
    }

    /**
     * =================== Create Commands ==============================
     *  
     */

    function handleCreateNew({ tokens, commandConfig }) {
        let rawName = cleanCommandToken(tokens[1]);
        let transId = tryResolveToken(1, rawName, commandConfig, false);

        if (transId === rawName) {
            const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
            const found = list?.find(
                x => x.caption.toLowerCase() === rawName.toLowerCase()
            );
            if (found) transId = found.name
            else {
                console.error("Invalid Tstruct name");
                return;
            }
        }

        redirectToTstruct(transId);
    }


    /**
     * ======================== END ==================================
     * 
     */

    /* ============== View Commands Functions =========================
       ----------------- Start ------------------------------------------
    */



    function handleViewDbConsole() {
        window.openDeveloperStudio("AxDBScript.aspx");
        // window.LoadIframe("AxDBScript.aspx");

    }

    function handleViewInbox() {
        // LoadIframe('processflow.aspx?activelist=t')
        window.LoadIframe('../aspx/processflow.aspx?activelist=t');

    }








    /* ============= End =================== */



    /***************************************************
     * Edit Command Function
     * **************************************************
    */


    /**
     * Helper functions 
     * @param {*} param0 
     * @returns 
     */

    function getUniqueId(str) {
        const match = str.match(/\[(.*?)\]/);
        return match ? match[1] : str;
    }






    function handleEditData({ tokens, commandConfig, resolvedParams }) {




        let rawStruct = cleanCommandToken(tokens[1]);
        let transId = tryResolveToken(1, rawStruct, commandConfig, false);
        let rawFieldName = "";
        let fieldName = "";
        let actualFieldName = "";
        let rawValue = "";
        let fieldValue = "";
        let fieldUniqueId = "";
        const extraPromptSource = commandConfig.prompts[1].extraParams.toLowerCase();
        let fieldValuePromptSource;
        let valuePresentInList = false;

        const extraSourceKey = `${extraPromptSource}_${transId}`.toLowerCase();

        const extraList = axDatasourceObj[extraSourceKey];

        const extraInlineValue = commandConfig?.prompts?.[3]?.promptValues;


        if (transId === rawStruct) {
            const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
            const found = list?.find(
                x => x.caption?.toLowerCase() === rawStruct
            );
            if (!found || !found.name) {
                console.error("TStruct not found:", rawStruct);
                return;
            }
            transId = found.name;
        }

        if (tokens.length > 3 && tokens.some(t => t.toLowerCase() !== extraInlineValue.toLowerCase())) {
            fieldValuePromptSource = commandConfig.prompts[2].promptSource.toLowerCase();
            rawFieldName = cleanCommandToken(tokens[2]);
            actualFieldName = tryResolveToken(2, rawFieldName, commandConfig, true);


            if (!Array.isArray(extraList)) {
                console.warn("Hidden field list is missing or invalid", extraList);
                actualFieldName = null;
                return;

            } else if (extraList.length === 0) {
                console.log("hidden field List is Empty!");
                actualFieldName = null;
                return;

            } else {
                const field = extraList[0];

                fieldName = field.fname ?? field.keyfield ?? field.name ?? field.displaydata;

                if (actualFieldName === null) {
                    console.error("Field name resolution failed: ", fieldName)
                }
            }
            if (!fieldName) {
                console.error("Field resolution failed:", rawFieldName);
                return;
            }

            rawValue = cleanCommandToken(tokens[3]);
            fieldValue = tryResolveToken(3, rawValue, commandConfig, false);
            fieldUniqueId = getUniqueId(fieldValue);

            const extraFieldValueList = axDatasourceObj[`${fieldValuePromptSource}_${transId}$#$${actualFieldName}`.toLowerCase()];
            console.log(`Edit Data → TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`);

            setEditSessionState(transId);

            if (Array.isArray(extraFieldValueList) && extraFieldValueList.length > 0) {

                valuePresentInList = extraFieldValueList.some(item =>
                    // item.displaydata?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    // item.name?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    // item.fname?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    // item.keyfield?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    item.caption?.toLowerCase() === fieldUniqueId.toLowerCase()

                );

                if (valuePresentInList)
                    redirectToTstruct(transId, true, fieldName, fieldUniqueId);
                else
                    redirectToTstruct(transId, false, fieldName, fieldUniqueId);
            }
            else {

                redirectToTstruct(transId, false, fieldName, fieldUniqueId);
            }




        } else {
            fieldValuePromptSource = commandConfig.prompts[1].promptSource.toLowerCase()

            if (!Array.isArray(extraList)) {
                console.warn("Hidden field list is missing or invalid", extraList);
                fieldName = null;
                return;

            } else if (extraList.length === 0) {
                console.log("hidden field List is Empty!");
                fieldName = null;
                return;

            } else {
                const field = extraList[0];

                fieldName = field.fname ?? field.keyfield ?? field.name ?? field.displaydata;

                if (fieldName === null) {
                    console.error("Field name resolution failed: ", fieldName)
                }

                rawValue = cleanCommandToken(tokens[2]);
                fieldValue = tryResolveToken(2, rawValue, commandConfig, true);
                fieldUniqueId = getUniqueId(fieldValue);

                if (fieldValue === null) {
                    console.error("Field value resolution failed:", rawValue);
                    return;
                }



            }

            const extraFieldValueList = axDatasourceObj[`${fieldValuePromptSource}_${transId}$#$${fieldName}`.toLowerCase()];
            console.log(`Edit Data → TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`);

            setEditSessionState(transId);

            if (Array.isArray(extraFieldValueList) && extraFieldValueList.length > 0) {

                valuePresentInList = extraFieldValueList.some(item =>
                    // item.displaydata?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    // item.name?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    // item.fname?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    // item.keyfield?.toLowerCase() === fieldUniqueId.toLowerCase() ||
                    item.caption?.toLowerCase() === fieldUniqueId.toLowerCase()

                );

                if (valuePresentInList)
                    redirectToTstruct(transId, true, fieldName, fieldUniqueId);
                else
                    redirectToTstruct(transId, false, fieldName, fieldUniqueId);
            }
            else {

                redirectToTstruct(transId, false, fieldName, fieldUniqueId);
            }

        }

    }


    function handleConfigurePermissions({ tokens, commandConfig }) {



        let rawUserName = cleanCommandToken(tokens[2]);

        let resolvedUserName = tryResolveToken(2, rawUserName, commandConfig, false);






        redirectToPermissionScreeen(rawUserName);






    }

    function handleConfigureAccess({ tokens, commandConfig }) {

        let fieldValue = cleanCommandToken(tokens[2]);




        redirectToResponsibilitiesPage(fieldValue);





    }







    /***************************************************
    * End
    * **************************************************
   */

    /***************************************************
     * Configure Commands Functions
     * *************************************************
     */



    function handleConfigureApi({ tokens, commandConfig }) {
        console.log("commandConfig: " + JSON.stringify(commandConfig));
        let fieldname = "ExecAPIDefName";
        let transId = "apidg";
        let param1Position = commandConfig.prompts[1].wordPos - 1;

        let rawApiName = cleanCommandToken(tokens[param1Position]);



        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, rawApiName);





    }

    function handleConfigureRule({ tokens, commandConfig }) {

        let transId = "ad_re";
        let fieldname = "rulename";

        let rawParamName = cleanCommandToken(tokens[2]);



        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, rawParamName);





    }

    function handleConfigureSchduledNotification({ tokens, commandConfig }) {

        let transId = "a__pn";
        let fieldname = "name";

        let rawParamName = cleanCommandToken(tokens[2]);



        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, rawParamName);





    }

    function handleConfigureServer({ tokens, commandConfig }) {

        let transId = "axpub";
        let fieldname = "servername";

        const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, rawParamName);





    }

    function handleCofigurePegFormNotification({ tokens, commandConfig }) {

        let transId = "ad_pn";
        let fieldname = "name";

        const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, rawParamName);





    }

    function handleConfigurePeg({ tokens, commandConfig }) {

        let rawParamName = cleanCommandToken(tokens[2]);

        redirectToProcessFlow(rawParamName);



    }

    function handleConfigureFormNotification({ tokens, commandConfig }) {

        let transId = "a__fn";
        const fieldname = "stransid";



        let rawParamValue = cleanCommandToken(tokens[2]);
        const fieldValue = tryResolveToken(2, rawParamValue, commandConfig, false);


        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, fieldValue);



    }



    function handleConfigureProperties({ tokens, commandConfig }) {

        window.LoadIframe("../aspx/tstruct.aspx?transid=ad_pr");

    }

    function handleConfigureJob({ tokens, commandConfig }) {

        let transId = "job_s";
        let fieldname = "jname";

        let rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, rawParamName);





    }

    /*********************************************************
      * End 
      * ******************************************************
      */

    function handleUpload({ tokens, commandConfig }) {
        window.LoadIframe("../aspx/ImportAll.aspx");




    }

    function handleDownload({ tokens, commandConfig }) {
        let targetUrl = "../aspx/ExportNew.aspx";
        targetUrl += "?action=export"
        window.LoadIframe(targetUrl);

    }



    function setEditSessionState(transId) {
        if (!transId) return;

        const href = top.window.location.href.toLowerCase();
        const aspxIndex = href.indexOf("/aspx/");

        if (aspxIndex === -1) {
            console.warn("setEditSessionState: '/aspx/' not found in URL", href);
            return;
        }

        const appSessUrl = href.substring(0, aspxIndex);
        const storageKey = `originaltrIds-${appSessUrl}`;

        const transIdArray = JSON.parse(
            localStorage.getItem(storageKey) || "[]"
        );

        if (!Array.isArray(transIdArray)) {
            console.warn("setEditSessionState: invalid stored value", transIdArray);
            return;
        }

        const normalizedTransId = transId.toLowerCase();

        if (transIdArray.some(x => x.toLowerCase() === normalizedTransId)) {
            const updated = transIdArray.filter(
                x => x.toLowerCase() !== normalizedTransId
            );

            localStorage.setItem(storageKey, JSON.stringify(updated));
        }
    }


    function redirectToEntity(transId, fieldName, fieldValue) {
        let targetUrl;
        if (!fieldValue) {

            targetUrl = `../aspx/Entity.aspx?tstid=${transId}`;

        } else {
            targetUrl = `../aspx/EntityForm.aspx?tstid=${transId}`;
            targetUrl += `&${fieldName}=${encodeURIComponent(fieldValue)}`;




        }

        window.LoadIframe(targetUrl);

    }

    function handleViewCommand({ tokens, commandConfig }) {

        let transId = "";
        let type = "";
        let fieldName;
        let fieldValue;
        let rawFieldName;
        let rawFieldValue;
        let fieldUniqueId;
        let fieldValueIndex = 0;


        if (tokens.length < 2) {
            console.warn("View Command required atleast two tokens");
            showToast("view command requires atleast two tokens");
            return;
        }

        console.log(JSON.stringify(commandConfig));


        const promptValues = commandConfig?.prompts?.[0].promptValues;
        const viewDataSource = commandConfig?.prompts?.[0].promptSource;
        const extraDataSource = commandConfig?.prompts?.[1].extraParams;


        const viewDataSourceKey = `${viewDataSource}`.toLowerCase();
        let rawStruct = cleanCommandToken(tokens[1]);
        transId = tryResolveToken(1, rawStruct, commandConfig, false);


        type = getType(viewDataSourceKey, transId, promptValues);

        const handler = VIEW_HANDLERS[type];




        if (!handler) {
            console.log("Error: Unsupported View Type");
            showToast("Error: Unsupported View Type");
            return;
        }


        if (type === "ads") {


            const adsName = cleanCommandToken(tokens[1]);
            const filters = extractAdsFilters(tokens);

            console.log("Ads Filters: ", filters);



            redirectToSmartView({
                adsName: adsName,
                filters: filters,
            });
            return;


        } else if (type === "page") {





            let rawFieldValue = cleanCommandToken(tokens[1]);
            redirectToHtmlPages(rawFieldValue);
            return;

        }


        const extraSourceKey = `${extraDataSource}_${transId}`.toLowerCase();


        if (tokens.length > 3) {
            fieldValueIndex = 3;


        } else {
            fieldValueIndex = 2;


        }




        const extraList = axDatasourceObj[extraSourceKey];

        if (extraList && extraList.length > 0) {
            fieldName = extraList[0].fname ?? extraList[0].keyfield ?? extraList[0].name ?? extraList[0].displaydata ?? null;
        } else {
            console.warn("Hidden field name not found in cache");
        }

        rawFieldValue = cleanCommandToken(tokens[fieldValueIndex]);
        fieldValue = tryResolveToken(fieldValueIndex, rawFieldValue, commandConfig, false);
        fieldUniqueId = getUniqueId(fieldValue);


        console.log(
            `view Data → TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`
        );

        handler({
            transId,
            fieldName,
            fieldValue: fieldUniqueId
        })

    }


    async function handleKeyfield({ tokens, commandConfig }) {

        const tstructName = cleanString(tokens[2]);
        const keyField = cleanString(tokens[3]);
        const actualFieldName = tryResolveToken(3, keyField, commandConfig, false);
        const transId = tryResolveToken(2, tstructName, commandConfig, false);
        if (!tstructName || !keyField) {
            showToast("TStruct and Key Field are required")
            console.log("TStruct and Key Field are required");
            return;
        }


        const requestBody = {
            action: "view",   ///edit
            adsNames: ["axi_tstructprops_insupd"],
            sqlParams: {
                param1: "axp_tstructprops",
                param2: "name,keyfield,userconfigured",
                param3: `'${transId}','${actualFieldName}','t'`,
                param4: `name = '${transId}'`
            }
        };

        const res = await getAxListAsync(requestBody);

        const dataObj = typeof res === "string" ? JSON.parse(res) : res;

        console.log("DATA obj is :", dataObj);
        console.log("Type of DATA OBJ:", typeof dataObj);

        const resultBlock = dataObj?.result?.data?.[0];

        if (dataObj?.result?.success && dataObj?.result?.message?.toLowerCase() === "success") {
            showToast(`Key field-${keyField} has been set for the form ${tstructName}`, 5000, true);
            console.log(`Key field-${keyField} has been set for the form ${tstructName}`);
            return;
        }


        if (resultBlock?.error) {
            showToast(`Error: ${resultBlock.error}`);
            console.log(`Error: ${resultBlock.error}`);
            return;
        }
    }

    function getType(axDatasourceKey, text, paramValuesCsv) {
        const paramList = paramValuesCsv?.split(",").map(v => v.trim().toLowerCase()).filter(Boolean);
        const VALID_TYPES = new Set(paramList);

        const data = axDatasourceObj?.[axDatasourceKey];

        console.log(JSON.stringify(data));


        const normalizedText = text.trim().toLowerCase();

        const item = data?.find(d => {
            if (typeof d.displaydata !== "string") return false;

            if (d.name && d.name.toLowerCase() === normalizedText) {
                return true;
            }


            const pureCaption = d.displaydata
                .replace(/\s*\(.*?\)\s*(?=\[[^\]]+\]$)/, "")
                .replace(/\s*\[[^\]]+\]\s*$/, "")
                .trim()
                .toLowerCase();

            return pureCaption === normalizedText;
        });

        if (!item || typeof item.displaydata !== "string") {
            return null;
        }

        const matches = [...item.displaydata.matchAll(/\[([^\]]+)\]/g)];

        if (matches.length === 0) {
            return null;
        }

        const candidate = matches[matches.length - 1][1].toLowerCase();

        return VALID_TYPES.has(candidate) ? candidate : null;
    }


    function redirectToHtmlPages(text) {
        const viewList = axDatasourceObj["axi_viewlist".toLowerCase()];

        const item = viewList.find(v => v.displaydata.includes(text));

        const requestUrl = item.name;
        console.log(requestUrl);

        window.LoadIframe(requestUrl);


    }

    /***************************************************
* OPEN COMMAND FUNCTION
* **************************************************
*/

    function handleOpenSource({ tokens, commandConfig }) {


        const type = cleanCommandToken(tokens[1]);
        let rawName = cleanCommandToken(tokens[2]);

        let resolvedName = tryResolveToken(2, rawName, commandConfig, false);


        if (resolvedName === rawName) {
            const listKey =
                type === "tstruct"
                    ? "Axi_TStructList".toLowerCase()
                    : type === "iview"
                        ? "Axi_IViewList".toLowerCase()
                        : null;

            if (!listKey) {
                alert("Unknown source type: " + type);
                return;
            }

            const list = axDatasourceObj[listKey];
            const found = list?.find(
                x => x.caption?.toLowerCase() === rawName.toLowerCase()
            );

            if (!found || !found.name) {
                console.error(`Source not found: ${rawName}`);
                return;
            }

            resolvedName = found.name;
        }


        if (type === "tstruct") {
            window.openDeveloperStudio("tstreact", resolvedName, true);
        } else if (type === "iview") {
            window.openDeveloperStudio("ivreact", resolvedName, true);
        } else {
            alert("Unknown source type: " + type);
        }
    }

    function handleOpenAds({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        const iviewName = "csqlist"
        const transId = "b_sql";
        let fieldname = "sqlname";

        let rawName = cleanCommandToken(tokens[2]);



        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        setEditSessionState(transId);



        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {

            redirectToIView(iviewName);


        } else {
            targetUrl += `&${fieldname}=${encodeURIComponent(paramName)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }

    }

    function handleOpenCard({ tokens, commandConfig }) {

        let targetUrl;
        let paramName;
        let transId = "a__cd";
        let fieldname = "cardname";
        let rawName = cleanCommandToken(tokens[2]);


        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }


        setEditSessionState(transId);
        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {

            targetUrl += `&${fieldname}=${encodeURIComponent(paramName)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }
    }

    function handleOpenPage({ tokens, commandConfig }) {

        let targetUrl;
        let paramName;
        let transId = "sect";
        let fieldname = "caption";
        let rawName = cleanCommandToken(tokens[2]);


        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        setEditSessionState(transId);

        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldname}=${encodeURIComponent(paramName)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }



    }

    function handleOpenAppVar({ tokens, commandConfig }) {
        window.LoadIframe("../aspx/tstruct.aspx?transid=axvar");

    }

    function handleOpenDevOptions({ tokens, commandConfig }) {
        window.LoadIframe("../aspx/tstruct.aspx?transid=axstc");

    }

    function handleOpenDbConsole() {
        window.openDeveloperStudio("AxDBScript.aspx");

    }

    /**
     * ======================= End =============================
     */

    /**
     * 
     * Run commands 
     */

    /**
     * Handles the run command execution
     *  @param {object} {tokens, commandConfig}
     * 
     */
    function handleRunCommand({ tokens, commandConfig }) {
        const structType = getStructType();
        let buttonLabel = cleanCommandToken(tokens[1]);
        let allButtons = null;


        if (!buttonLabel) return;



        if (structType === "o") {
            console.error("Invalid Struct type");
            showToast("Invalid Struct type");
            return;
        }


        switch (structType) {
            case "t":
            case "i":

                const isDesign = isTstructDesignMode();
                if (isDesign) {
                    if (!designModeToolbarButtons) {
                        designModeToolbarButtons = getDesignModeToolbarButtons();


                    }

                    allButtons = [...Object.values(designModeToolbarButtons)]


                } else {
                    if (!bottomToolbarButtons) bottomToolbarButtons = getBottomToolbarButtons();
                    if (!topToolbarButtons) topToolbarButtons = getTopToolbarButtons();
                    allButtons = [...Object.values(bottomToolbarButtons),
                    ...Object.values(topToolbarButtons)];


                }


                break;

            case "e":
            case "ef":
            case "c":
                if (!entityToolbarButtons) entityToolbarButtons = getEntityToolbarButtons();
                allButtons = [...Object.values(entityToolbarButtons)];
                break;

            case "pf":
                if (!pfToolbarButtons) pfToolbarButtons = getPFToolbarButtons();
                allButtons = [...Object.values(pfToolbarButtons)]
                break;


            default:
                console.error("Invalid StructType")
                break;
        }

        console.log("All Buttons: " + JSON.stringify(allButtons));

        let resolvedBtnId = tryResolveToken(1, buttonLabel, commandConfig, false);

        console.log(`Run Command Debug: Label="${buttonLabel}", ResolvedID="${resolvedBtnId}"`);

        let targetBtn = null;

        if (resolvedBtnId && resolvedBtnId !== buttonLabel) {
            targetBtn = allButtons.find(btn => btn.id === resolvedBtnId);
        }

        if (!targetBtn && resolvedBtnId) {
            targetBtn = allButtons.find(btn => btn.id === resolvedBtnId);
        }


        if (!targetBtn) {
            targetBtn = allButtons.find(btn => {
                const rawLabel = btn.label || "";

                const normalizedBtnLabel = rawLabel.toLowerCase().replace(/[\r\n\t]+/g, ' ').trim();

                const normalizedInputLabel = buttonLabel.toLowerCase().replace(/[\r\n\t]+/g, ' ').trim();

                return normalizedBtnLabel === normalizedInputLabel;


            });

        }


        if (!targetBtn) {
            console.error(`Button not found for label: ${buttonLabel}`);
            showToast(`Button '${buttonLabel}' not found`, 3000);
            return;
        }

        console.log(`Clicking button: ${targetBtn.label} (${targetBtn.id})`);

        targetBtn.click();



    }

    function normalizeDate(val) {
        if (!val.includes("/")) return val;
        const [d, m, y] = val.split("/");
        return `${y}-${m}-${d}`; // ISO
    }

    function extractAdsFilters(tokens) {
        const filters = [];


        let i = 2;

        while (i < tokens.length) {

            const rawColToken = cleanCommandToken(tokens[i]);
            if (!rawColToken) { i++; continue; }


            let nextTokenRaw = cleanCommandToken(tokens[i + 1] || "");

            let operator = "=";
            let valueTokenIndex = -1;
            let rawValue = "";

            if (OPERATORS_SET.has(nextTokenRaw)) {

                operator = nextTokenRaw;
                rawValue = cleanCommandToken(tokens[i + 2] || "");
                valueTokenIndex = i + 2;


                i += 3;
            } else {

                operator = "=";
                rawValue = nextTokenRaw;
                valueTokenIndex = i + 1;


                i += 2;
            }

            const colMetadata = adsfieldvalueanddt[rawColToken] || {};

            if (colMetadata?.datatype === "d") {
                rawValue = normalizeDate(rawValue);
            }

            filters.push({
                field: rawColToken,
                operator: operator,
                value: rawValue,
                datatype: colMetadata.datatype,
                isAccept: colMetadata.isAccept
            });
        }

        return filters;
    }



    function getBottomToolbarButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const toolbar = doc.querySelector(".BottomToolbarBar");
        if (!toolbar) return {};

        const buttons = toolbar.querySelectorAll("a");

        const result = {};

        buttons.forEach((btn) => {
            if (!hasAction(btn)) return;
            const id = btn.id || btn.getAttribute("data-id");
            if (!id) return;
            const label = extractButtonLabel(btn);
            if (!label) console.log("There is no label for Element: " + btn);



            result[id] = {
                id,
                label,
                element: btn,
                click: () => btn.click()
            };
        });

        return result;
    }

    function getTopToolbarButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const toolbar = doc.querySelector(".toolbarRightMenu");
        if (!toolbar) return {};

        const buttons = toolbar.querySelectorAll("a");

        const result = {};

        buttons.forEach((btn) => {
            if (!hasAction(btn)) return;
            const id = btn.id || btn.getAttribute("data-id");
            if (!id) return;
            // const label = btn.innerText.trim();
            const label = extractButtonLabel(btn);
            if (!label) console.log("There is no label for Element: " + btn);



            result[id] = {
                id,
                label,
                element: btn,
                click: () => btn.click()
            };
        });

        return result;
    }

    function getTStructButtons(attributeName) {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const toolbar = doc.querySelector(`.${attributeName}`);  //|| doc.querySelector(`#${attributeName}`);
        if (!toolbar) return {};

        const buttons = toolbar.querySelectorAll("a");

        const result = {};

        buttons.forEach((btn) => {
            const id = btn.id || btn.getAttribute("data-id");
            if (!id) return;
            const label = btn.innerText.trim();
            if (!label) console.log("There is no label for Element: " + btn);



            result[label] = {
                id,
                label,
                element: btn,
                click: () => btn.click()
            };
        });

        return result;
    }

    function getStructType() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return null;

        const iframeDoc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!iframeDoc) return null;

        const src = iframe.getAttribute("src");
        if (!src) return null;

        const page = src.split("?")[0].toLowerCase();

        if (!page) {
            return null;
        }

        const bodyId = iframeDoc.body?.id || "";

        if ((page.endsWith("/tstruct.aspx") || page.includes("tstruct.aspx")) && bodyId !== "Entitymanagement_Body") {
            return "t" // tstruct page
        }



        if (page.endsWith("/iview.aspx") || page.includes("iview.aspx")) {
            return "i";  // IView page
        }

        if ((page.endsWith("/entity.aspx") || page.includes("entity.aspx")) && bodyId === "Entitymanagement_Body") {
            return "e";  // Entity page
        }



        if ((page.endsWith("/entityform.aspx") || page.includes("entityform.aspx")) || bodyId === "Entitymanagement_Body") {
            return "ef";  // Entity Data page
        }

        if (src.includes("/CustomPages") || src.includes("/axidev")) {
            return "c"; // Custom page

        }

        // ../aspx/processflow.aspx?activelist=t&hdnbElapsTime=0


        if (page.endsWith("/processflow.aspx") || page.includes("processflow.aspx")) {
            return "pf"; // Process flow page

        }


        return "o"; // Others 

    }

    function extractButtonLabel(btn) {


        const dataExtra = btn.getAttribute("data-extra");
        if (dataExtra) return dataExtra.trim();

        const title = btn.getAttribute("title");
        if (title) return title.trim();

        const menuTitle = btn.querySelector(".menu-title");

        if (menuTitle) return menuTitle.textContent.trim();




        const text = Array.from(btn.childNodes)
            .filter(n => n.nodeType === Node.TEXT_NODE && n.textContent.trim())
            .map(n => n.textContent.trim())
            .join(" ")
            .trim();

        if (text) {

            return text;
        }




        return btn.innerText.trim();

    }

    function hasAction(btn) {
        if (btn.getAttribute("onclick")) return true;

        if (btn.tagName === "A") {
            const href = btn.getAttribute("href");

            if (href && href !== "#" && href !== "javascript:void(0)") return true;

        }



        return false;
    }

    function getEntityToolbarButtons() {
        const iframe = document.getElementById("middle1");

        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const toolbar = doc.querySelector(".card-toolbar");
        if (!toolbar) return {};

        const buttons = toolbar.querySelectorAll("a, button");
        const result = {};

        buttons.forEach((btn, index) => {
            // if (!hasAction(btn)) return;

            if (btn.classList.contains("d-none")) return;

            if (btn.getAttribute("data-kt-menu-attach") === "parent") return;

            if (btn.querySelector(".menu-title") && btn.hasAttribute("data-kt-menu-trigger")) return;


            const id = btn.id || btn.getAttribute("data-id") || btn.getAttribute("title") || `toolbar-btn-${index}`;
            if (!id) return;

            const label = extractButtonLabel(btn);
            if (!label) return;

            result[id] = {
                id,
                label: label.toLowerCase(),
                element: btn,
                click: () => btn.click()
            };
        });

        return result;

    }

    function watchDesignModeChange(callback) {
        const iframe = document.getElementById("middle1");
        if (!iframe) return;

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return;

        const target = doc.querySelector("#divDc1");
        if (!target) return;

        const observer = new MutationObserver(() => {
            callback(target.classList.contains("tstructDesignMode"));
        });

        observer.observe(target, {
            attributes: true,
            attributeFilter: ["class"]
        });
    }


    function getDesignModeToolbarButtons() {
        const iframe = document.getElementById("middle1");

        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const toolbar = doc.querySelector("#designModeToolbar");
        if (!toolbar) return {};

        const buttons = toolbar.querySelectorAll("a, button");
        const result = {};

        buttons.forEach((btn, index) => {
            // if (!hasAction(btn)) return;

            const id = btn.id || btn.getAttribute("data-id") || btn.getAttribute("title") || `toolbar-btn-${index}`;
            if (!id) return;

            const label = extractButtonLabel(btn);
            if (!label) return;

            result[id] = {
                id,
                label: label.toLowerCase(),
                element: btn,
                click: () => btn.click()
            };
        });

        return result;

    }

    function isTstructDesignMode() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return false;

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return false;

        const root = doc.querySelector("#divDc1");
        if (!root) return false;

        return root.classList.contains("tstructDesignMode");
    }

    function getPFToolbarButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const result = {};

        // Process Flow toolbar zones
        const containers = [
            ".Page-Title-Bar",
            ".Tkts-toolbar-Left",
            ".Tkts-toolbar-Right"
        ];

        containers.forEach(selector => {
            const root = doc.querySelector(selector);
            if (!root) return;

            const elements = root.querySelectorAll(
                "button, a, div.btn"
            );

            elements.forEach((el, index) => {
                // skip hidden
                if (el.classList.contains("d-none")) return;

                // must be actionable
                // if (!hasAction(el)) return;

                const isActionable =
                    hasAction?.(el) ||
                    el.hasAttribute("data-kt-menu-trigger") ||
                    el.classList.contains("tb-btn") ||
                    el.classList.contains("btn-icon");

                if (!isActionable) return;

                const label = extractButtonLabel(el);
                if (!label) return;

                const id =
                    el.id ||
                    el.getAttribute("data-id") ||
                    el.getAttribute("data-bs-title") ||
                    el.getAttribute("data-bs-original-title") ||
                    el.getAttribute("title") ||
                    `process-btn-${label.toLowerCase().replace(/\s+/g, "_")}-${index}`;

                result[id] = {
                    id,
                    label: label.toLowerCase(),
                    element: el,
                    click: () => el.click()
                };
            });
        });

        return result;
    }

    function isSystemMessage(item) {
        if (!item) return false;

        const text = typeof item === 'string' ? item : (item.displaydata || "");

        return text.startsWith("Loading") ||
            text.startsWith("Waiting") ||
            text.startsWith("Error") ||
            text === "No Data";

    }


    function handleAnalyse({ tokens, commandConfig }) {

        let targetUrl = "../aspx/Analytics.aspx";

        if (tokens.length === 1) {
            targetUrl += "?calendar=t";
            targetUrl += "&isDupTab=true-1770626614111";
            targetUrl += "&hdnbElapsTime=0";
        }


        else {

            const captionSelected = cleanString(tokens[1]);
            const transIdAnalyse = tryResolveToken(1, captionSelected, commandConfig);

            targetUrl += `?entity=${encodeURIComponent(transIdAnalyse)}`;

            if (tokens.length == 3) {
                let groupByFieldCaption = cleanString(tokens[2]);
                const groupByFiedlname = tryResolveToken(2, groupByFieldCaption, commandConfig);
                targetUrl += `&groupby=${encodeURIComponent(groupByFiedlname)}`;
            }
            else if (tokens.length > 3) {
                showToast("Analyse commands requires only 3 tokens");
                return;
            }

            targetUrl += "&calendar=t";
            targetUrl += "&isDupTab=true-1770626614111";
            targetUrl += "&hdnbElapsTime=0";
        }

        console.log("Target URL from analyse command : " + targetUrl);
        window.LoadIframe(targetUrl);
    }


    function resetSetCommandState() {
        SET_COMMAND_STATE.isNextField = false;
        SET_COMMAND_STATE.currentField = null;
        SET_COMMAND_STATE.currentFieldType = null;
        SET_COMMAND_STATE.isFirst = true;
        SET_COMMAND_STATE.transid = null;
        SET_COMMAND_STATE.currentFieldValue = null;
        SET_COMMAND_STATE.isDropDown = false;
        //createfieldnamevaluesList = [];
    }

    function createCommandHandling(tokens, commandConfig, createsourceObj) {
        //const viewSource = commandConfig?.prompts?.[0]?.promptSource?.toLowerCase();
        const viewSource = createsourceObj;

        if (SET_COMMAND_STATE.isDropDown) {
            let acceptedValue = cleanString(tokens[tokens.length - 2]);
            if (acceptedValue)
                SET_COMMAND_STATE.currentFieldValue = acceptedValue;

            SET_COMMAND_STATE.isDropDown = false;
        }
        

        if (SET_COMMAND_STATE.currentFieldType == 'n') {

            return processCreateCommand(tokens, commandConfig, viewSource);
        }
        else if (SET_COMMAND_STATE.currentFieldType == 'd') {
            const prevValueInSet = tokens[tokens.length - 2];
            if (prevValueInSet === "Today" || prevValueInSet === "Yesterday" ||
                prevValueInSet === "Tomorrow" || prevValueInSet === "LastWeek" ||
                prevValueInSet === "NextWeek" || prevValueInSet === "LastYear") {
                const dateResult = getDateByFilter(prevValueInSet);
                let date = dateResult.date;


                if (date !== null || date !== undefined) {
                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];


                    ///We need to use System Date Format
                    date = formatDate(date, dateString);

                    console.log("Final date:", date);

                    settokens[lastIndex] = date;

                    input.value = settokens.join(" ");

                    SET_COMMAND_STATE.currentFieldValue = date;

                    //// set in resolve params.
                    //resolvedParamsCopy.fields.push({
                    //    fieldname: SET_COMMAND_STATE.currentField,
                    //    fieldtype: SET_COMMAND_STATE.currentFieldType,
                    //    fieldvalue: date
                    //});

                }
            }
            else {

                if (prevValueInSet == "Custom") {
                    //let settokens = tokens
                    //let lastIndex = tokens.length - 2;
                    //let lastToken = tokens[lastIndex];
                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];

                    settokens[lastIndex] = "";

                    input.value = settokens.join(" ");

                    ///// set as empty.
                    //resolvedParamsCopy.fields.push({
                    //    fieldname: SET_COMMAND_STATE.currentField,
                    //    fieldtype: SET_COMMAND_STATE.currentFieldType,
                    //    fieldvalue: null
                    //});
                    updateDynamicHintFromPrompt({ prompt: "fieldValue" })
                    showToast("Please Type the date", 5000, true);
                    return ["Please Type the date"];
                }
                else {
                    //const partialDate = tokens[tokens.length - 1]
                    const partialDate = tokens[tokens.length - 1]
                    let isSetValidDate = isValidDate(partialDate)

                    if (isSetValidDate) {

                        ///We need to use System Date Format
                        const formattedDate = formatDate(partialDate, dateString);
                        date = formattedDate;
                        console.log("Final date:", date);
                        SET_COMMAND_STATE.currentFieldValue = date;
                        SET_COMMAND_STATE.currentFieldType = null;
                        SET_COMMAND_STATE.isNextField = true;

                    }
                    else 
                        return ["Please type Valid date using / (ex:DD/MM/YYYY)"];
                }
            }

            return processCreateCommand(tokens, commandConfig, viewSource);

        }
        else
            return processCreateCommand(tokens, commandConfig, viewSource);
    }

    function processCreateCommand(tokens, commandConfig, createCommandSourceObj) {
        const targetIndex = tokens.length - 1;
        const partialTyped = cleanString(tokens[targetIndex]);

        const createTransId = cleanCommandToken(tokens[1]);
        setCommandTransid = tryResolveToken(1, createTransId, commandConfig, false);

        if (SET_COMMAND_STATE.transid === null || setCommandTransid !== SET_COMMAND_STATE.transid) {
            SET_COMMAND_STATE = {
                isNextField: false,
                currentField: null,
                currentFieldType: null,
                isFirst: true,
                transid: setCommandTransid,
                currentFieldValue: null,
                isDropDown: false

            };
        }


        if (targetIndex % 2 !== 0) {
            const sourceName = createCommandSourceObj.toLowerCase();
            const sourceKey = `${sourceName}_${setCommandTransid}`.toLowerCase();

            if (SET_COMMAND_STATE.currentField) {
                let previousColumnName = SET_COMMAND_STATE.currentField;
                previousColumnName = tryResolveToken(targetIndex - 2, previousColumnName, commandConfig, false);
                let previousColumnValue = SET_COMMAND_STATE.currentFieldValue;
                AddFieldstoList(previousColumnName, 1, previousColumnValue);
            }


            if (!axDatasourceObj[sourceKey]) {
                loadList(sourceName, setCommandTransid);
                return ["Loading Field list..."];
            }

            const list = axDatasourceObj[sourceKey];
            if (!Array.isArray(list)) return [];


            const usedColumns = new Set();
            for (let i = 3; i < targetIndex; i += 2) {
                const usedToken = cleanString(tokens[i]).toLowerCase();
                usedColumns.add(usedToken);
            }

            const filtered = list.filter(col => {
                const rawDisplay = (col.displaydata || col.name).toLowerCase();
                const cleanDisplay = rawDisplay
                    .replace(/\s*\(.*?\)/g, "")
                    .replace(/\s*\[[^\]]+\]\s*$/, "")
                    .trim();
                const rawName = (col.name || "").toLowerCase();
                const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);
                const matchesInput = cleanDisplay.includes(partialTyped.toLowerCase());
                return !isUsed && matchesInput;
            });

            filteredObjects = filtered;

            let resultList = filtered.map(item => item.displaydata || item.caption || item.name || item.fname || item.keyfield);

            if (SET_COMMAND_STATE.currentField || (targetIndex % 2 !== 0 && targetIndex >= 4)) {
                resultList.unshift(goOption);
                resultList.unshift(saveOption);
                filteredObjects.unshift(goOption);
                filteredObjects.unshift(saveOption);
            }
            else if (tokens.length >= 3) {
                resultList.unshift(goOption);
                filteredObjects.unshift(goOption);
            }

        
            SET_COMMAND_STATE.currentField = null;
            SET_COMMAND_STATE.currentFieldType = null
            SET_COMMAND_STATE.currentFieldValue = null;
            SET_COMMAND_STATE.isNextField = false;
            SET_COMMAND_STATE.isDropDown = false;




            return resultList;
        }


        else {

            if (!SET_COMMAND_STATE.isNextField) {
                let prevColumnName
                if (!SET_COMMAND_STATE.currentField) {
                    prevColumnName = cleanString(tokens[targetIndex - 1]);
                    SET_COMMAND_STATE.currentField = prevColumnName;
                }
                else
                    prevColumnName = SET_COMMAND_STATE.currentField;


                const colSourceKey = createCommandSourceObj + `_${setCommandTransid}`.toLowerCase();
                const colList = axDatasourceObj[colSourceKey];

                if (!colList) {
                    console.log("In processCreateCommond " + createCommandSourceObj + " is empty");
                    showToast("Please Try Again Later.");
                    return [];
                }

                const columnMetadata = colList.find(
                    c =>
                        c.name?.toLowerCase() === prevColumnName.toLowerCase() ||
                        c.displaydata?.toLowerCase().replace(/\s*\(.*?\)/g, '').trim() === prevColumnName.toLowerCase()
                ) || null;

                if (!columnMetadata) {
                    console.log("Selected Field Name is Not in the List " + prevColumnName);
                    showToast("Please Select Fields from the list");

                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];

                    settokens[lastIndex] = "";

                    input.value = settokens.join(" ");
                    updateDynamicHintFromPrompt({ prompt: "fieldName" })
                    return [];
                }

                let isAccept;

                if (columnMetadata.moe === "a") {
                    isAccept = true;
                } else {
                    isAccept = false;
                }



                let datatype;

                if (SET_COMMAND_STATE.currentFieldType === null) {
                    datatype = columnMetadata.datatype;
                    SET_COMMAND_STATE.currentFieldType = datatype;
                }
                else datatype = SET_COMMAND_STATE.currentFieldType;


                if (datatype === 'c' || datatype === 'n') {
                    if (isAccept) {
                        let acceptedValue = cleanString(tokens[tokens.length - 1]);
                        if (acceptedValue)
                            SET_COMMAND_STATE.currentFieldValue = acceptedValue;
                        else {
                            return ["Please type the value..."];
                        }
                        return [];

                    }
                    else {

                        SET_COMMAND_STATE.isDropDown = true;
                        const acceptedValue = cleanString(tokens[tokens.length - 1]);
                        const sourceName = "axi_firesql";


                        var params1 = columnMetadata.fldsql;
                        var params2 = prepareKeyValueString(allGloblVars);
                        console.log(params2);
                        var params3 = columnMetadata.normalized;
                        var params4 = columnMetadata.fromlist;

                        params2 += ";" + getFieldNameandItsValue(createfieldnamevaluesList, commandConfig);

                        console.log(params2);



                        const paramValue = `${params1}$#$${params2}$#$${params3}$#$${params4}`;
                        const sourceKey = `${sourceName}_${paramValue}`.toLowerCase();

                        if (!axDatasourceObj[sourceKey]) {
                            loadList(sourceName, paramValue);
                            return ["Loading values..."];
                        }

                        const list = axDatasourceObj[sourceKey];
                        if (!Array.isArray(list)) return [];

                        const filtered = list.filter(col => {
                            const rawDisplay = String(col.displaydata || col.name)
                                .toLowerCase();

                            const normalizedTypedValue = (acceptedValue ?? "")
                                .toLowerCase();

                            return !normalizedTypedValue || rawDisplay.includes(normalizedTypedValue);
                        });


                        return filtered.map(col => col.displaydata || col.name);
                    }
                }
                else if (datatype === 'd') {


                    const acceptedValue = cleanString(tokens[tokens.length - 1]);

                    const list = [
                        "Today",
                        "Yesterday",
                        "Tomorrow",
                        "LastWeek",
                        "NextWeek",
                        "LastYear",
                        "Custom"
                    ];

                    const filtered = list.filter(col => {

                        const rawDisplay = col.toLowerCase();

                        const normalizedTypedValue = (acceptedValue ?? "")
                            .toLowerCase();

                        return rawDisplay.includes(normalizedTypedValue);
                    });


                    return filtered;


                }

                //else if (datatype === 'd') {

                //}
            }
            else return [];
        }
    }

    function editCommandHandling(tokens, commandConfig, createsourceObj) {
        //const viewSource = commandConfig?.prompts?.[0]?.promptSource?.toLowerCase();
        const viewSource = createsourceObj;

        if (SET_COMMAND_STATE.isDropDown) {
            let acceptedValue = cleanString(tokens[tokens.length - 2]);
            if (acceptedValue)
                SET_COMMAND_STATE.currentFieldValue = acceptedValue;

            SET_COMMAND_STATE.isDropDown = false;
        }


        if (SET_COMMAND_STATE.currentFieldType == 'n') {

            return processEditCommand(tokens, commandConfig, viewSource);
        }
        else if (SET_COMMAND_STATE.currentFieldType == 'd') {
            const prevValueInSet = tokens[tokens.length - 2];
            if (prevValueInSet === "Today" || prevValueInSet === "Yesterday" ||
                prevValueInSet === "Tomorrow" || prevValueInSet === "LastWeek" ||
                prevValueInSet === "NextWeek" || prevValueInSet === "LastYear") {
                const dateResult = getDateByFilter(prevValueInSet);
                let date = dateResult.date;


                if (date !== null || date !== undefined) {
                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];


                    ///We need to use System Date Format
                    date = formatDate(date, dateString);

                    console.log("Final date:", date);

                    settokens[lastIndex] = date;

                    input.value = settokens.join(" ");

                    SET_COMMAND_STATE.currentFieldValue = date;

                    //// set in resolve params.
                    //resolvedParamsCopy.fields.push({
                    //    fieldname: SET_COMMAND_STATE.currentField,
                    //    fieldtype: SET_COMMAND_STATE.currentFieldType,
                    //    fieldvalue: date
                    //});

                }
            }
            else {

                if (prevValueInSet == "Custom") {
                    //let settokens = tokens
                    //let lastIndex = tokens.length - 2;
                    //let lastToken = tokens[lastIndex];
                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];

                    settokens[lastIndex] = "";

                    input.value = settokens.join(" ");

                    ///// set as empty.
                    //resolvedParamsCopy.fields.push({
                    //    fieldname: SET_COMMAND_STATE.currentField,
                    //    fieldtype: SET_COMMAND_STATE.currentFieldType,
                    //    fieldvalue: null
                    //});
                    showToast("Please Type the date", 5000, true);
                    updateDynamicHintFromPrompt({ prompt: "fieldValue" })
                    return ["Please Type the date"];
                }
                else {
                    //const partialDate = tokens[tokens.length - 1]
                    const partialDate = tokens[tokens.length - 1]

                    let isSetValidDate = isValidDate(partialDate)

                    if (isSetValidDate) {

                        ///We need to use System Date Format
                        const formattedDate = formatDate(partialDate, dateString);
                        date = formattedDate;
                        console.log("Final date:", date);
                        SET_COMMAND_STATE.currentFieldValue = date;
                        SET_COMMAND_STATE.currentFieldType = null;
                        SET_COMMAND_STATE.isNextField = true;

                    }
                    else
                        return ["Please type Valid Date"];
                }
            }

            return processEditCommand(tokens, commandConfig, viewSource);

        }
        else
            return processEditCommand(tokens, commandConfig, viewSource);
    }

    function processEditCommand(tokens, commandConfig, createCommandSourceObj) {
        const targetIndex = tokens.length - 1;
        const partialTyped = cleanString(tokens[targetIndex]);

        const createTransId = cleanCommandToken(tokens[1]);
        setCommandTransid = tryResolveToken(1, createTransId, commandConfig, false);

        if (SET_COMMAND_STATE.transid === null || setCommandTransid !== SET_COMMAND_STATE.transid) {
            SET_COMMAND_STATE = {
                isNextField: false,
                currentField: null,
                currentFieldType: null,
                isFirst: true,
                transid: setCommandTransid,
                currentFieldValue: null,
                isDropDown: false

            };
        }


        if (targetIndex % 2 == 0) {
            const sourceName = createCommandSourceObj.toLowerCase();
            const sourceKey = `${sourceName}_${setCommandTransid}`.toLowerCase();

            if (SET_COMMAND_STATE.currentField) {
                let previousColumnName = SET_COMMAND_STATE.currentField;
                previousColumnName = tryResolveToken(targetIndex - 2, previousColumnName, commandConfig, false);
                let previousColumnValue = SET_COMMAND_STATE.currentFieldValue;
                AddFieldstoList(previousColumnName, 1, previousColumnValue);
            }


            if (!axDatasourceObj[sourceKey]) {
                loadList(sourceName, setCommandTransid);
                return ["Loading Field list..."];
            }

            const list = axDatasourceObj[sourceKey];
            if (!Array.isArray(list)) return [];


            const usedColumns = new Set();
            for (let i = 4; i < targetIndex; i += 2) {
                const usedToken = cleanString(tokens[i]).toLowerCase();
                usedColumns.add(usedToken);
            }

            const filtered = list.filter(col => {
                const rawDisplay = (col.displaydata || col.name).toLowerCase();
                const cleanDisplay = rawDisplay
                    .replace(/\s*\(.*?\)/g, "")
                    .replace(/\s*\[[^\]]+\]\s*$/, "")
                    .trim();
                const rawName = (col.name || "").toLowerCase();
                const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);
                const matchesInput = cleanDisplay.includes(partialTyped.toLowerCase());
                return !isUsed && matchesInput;
            });

            filteredObjects = filtered;

            let resultList = filtered.map(item => item.displaydata || item.caption || item.name || item.fname || item.keyfield);

            if (SET_COMMAND_STATE.currentField || (targetIndex % 2 !== 0 && targetIndex >= 4)) {
                //resultList.unshift(goOption);
                resultList.unshift(saveOption);
                //filteredObjects.unshift(goOption);
                filteredObjects.unshift(saveOption);
            }
            //else if (tokens.length >= 3) {
            //    //resultList.unshift(goOption);
            //    //filteredObjects.unshift(goOption);
            //}


            SET_COMMAND_STATE.currentField = null;
            SET_COMMAND_STATE.currentFieldType = null
            SET_COMMAND_STATE.currentFieldValue = null;
            SET_COMMAND_STATE.isNextField = false;
            SET_COMMAND_STATE.isDropDown = false;




            return resultList;
        }

        //else if (targetIndex <=3) {
        //    const sourceName = createCommandSourceObj.toLowerCase();
        //    const sourceKey = `${sourceName}_${setCommandTransid}`.toLowerCase();

        //    if (!axDatasourceObj[sourceKey]) {
        //        loadList(sourceName, setCommandTransid);
        //        return ["Loading Field list..."];
        //    }

        //    const list = axDatasourceObj[sourceKey];
        //    if (!Array.isArray(list)) return [];


        //    const filtered = list.filter(col => {
        //        const rawDisplay = (col.displaydata || col.name).toLowerCase();
        //        const cleanDisplay = rawDisplay
        //            .replace(/\s*\(.*?\)/g, "")
        //            .replace(/\s*\[[^\]]+\]\s*$/, "")
        //            .trim();
        //        const rawName = (col.name || "").toLowerCase();
        //        const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);
        //        const matchesInput = cleanDisplay.includes(partialTyped.toLowerCase());
        //        return !isUsed && matchesInput;
        //    });

        //    filteredObjects = filtered;

        //    let resultList = filtered.map(item => item.displaydata || item.caption || item.name || item.fname || item.keyfield);

        //    return resultList;
        //}
        else {

            if (!SET_COMMAND_STATE.isNextField) {
                let prevColumnName
                if (!SET_COMMAND_STATE.currentField) {
                    prevColumnName = cleanString(tokens[targetIndex - 1]);
                    SET_COMMAND_STATE.currentField = prevColumnName;
                }
                else
                    prevColumnName = SET_COMMAND_STATE.currentField;


                const colSourceKey = createCommandSourceObj + `_${setCommandTransid}`.toLowerCase();
                const colList = axDatasourceObj[colSourceKey];

                if (!colList) {
                    console.log("In processCreateCommond " + createCommandSourceObj + " is empty");
                    showToast("Please Try Again Later.");
                    return [];
                }                     

                const columnMetadata = colList.find(
                    c =>
                        c.name?.toLowerCase() === prevColumnName.toLowerCase() ||
                        c.displaydata?.toLowerCase().replace(/\s*\(.*?\)/g, '').trim() === prevColumnName.toLowerCase()
                ) || null;

                if (!columnMetadata) {
                    console.log("Selected Field Name is Not in the List " + prevColumnName);
                    showToast("Please Select Fields from the list");

                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];

                    settokens[lastIndex] = "";

                    input.value = settokens.join(" ");
                    updateDynamicHintFromPrompt({ prompt: "fieldName" })
                    return [];
                }

                let isAccept;

                if (columnMetadata.moe === "a") {
                    isAccept = true;
                } else {
                    isAccept = false;
                }



                let datatype;

                if (SET_COMMAND_STATE.currentFieldType === null) {
                    datatype = columnMetadata.datatype;
                    SET_COMMAND_STATE.currentFieldType = datatype;
                }
                else datatype = SET_COMMAND_STATE.currentFieldType;


                if (datatype === 'c' || datatype === 'n') {
                    if (isAccept) {
                        let acceptedValue = cleanString(tokens[tokens.length - 1]);
                        if (acceptedValue)
                            SET_COMMAND_STATE.currentFieldValue = acceptedValue;
                        else {
                            return ["Please type the value..."];;
                        }

                    }
                    else {

                        SET_COMMAND_STATE.isDropDown = true;
                        const acceptedValue = cleanString(tokens[tokens.length - 1]);
                        const sourceName = "axi_firesql";


                        var params1 = columnMetadata.fldsql;
                        var params2 = prepareKeyValueString(allGloblVars);
                        console.log(params2);
                        var params3 = columnMetadata.normalized;
                        var params4 = columnMetadata.fromlist;


                        params2 += ";" + getFieldNameandItsValue(createfieldnamevaluesList, commandConfig);

                        console.log(params2);



                        const paramValue = `${params1}$#$${params2}$#$${params3}$#$${params4}`;
                        const sourceKey = `${sourceName}_${paramValue}`.toLowerCase();

                        if (!axDatasourceObj[sourceKey]) {
                            loadList(sourceName, paramValue);
                            return ["Loading values..."];
                        }

                        const list = axDatasourceObj[sourceKey];
                        if (!Array.isArray(list)) return [];

                        const filtered = list.filter(col => {
                            const rawDisplay = String(col.displaydata || col.name)
                                .toLowerCase();

                            const normalizedTypedValue = (acceptedValue ?? "")
                                .toLowerCase();

                            return !normalizedTypedValue || rawDisplay.includes(normalizedTypedValue);
                        });


                        return filtered.map(col => col.displaydata || col.name);
                    }
                }
                else if (datatype === 'd') {


                    const acceptedValue = cleanString(tokens[tokens.length - 1]);

                    const list = [
                        "Today",
                        "Yesterday",
                        "Tomorrow",
                        "LastWeek",
                        "NextWeek",
                        "LastYear",
                        "Custom"
                    ];

                    const filtered = list.filter(col => {

                        const rawDisplay = col.toLowerCase();

                        const normalizedTypedValue = (acceptedValue ?? "")
                            .toLowerCase();

                        return rawDisplay.includes(normalizedTypedValue);
                    });


                    return filtered;


                }

                //else if (datatype === 'd') {

                //}
            }
            else return [];
        }
    }
    function AddFieldstoList(fieldName, rowNo, value) {

        if (!fieldName || !value) return;

        const keyPrefix = `${fieldName}~${rowNo}=`;
        const existingIndex = createfieldnamevaluesList.findIndex(item =>
            item.startsWith(keyPrefix)
        );

        const formatted = `${fieldName}~${rowNo}=${value}`;

        if (existingIndex !== -1) {
            if (createfieldnamevaluesList[existingIndex] !== formatted) {
                createfieldnamevaluesList[existingIndex] = formatted;
            }
        } else {
            createfieldnamevaluesList.push(formatted);
        }
    }


    function getTransID() {

        // const iframes = document.querySelectorAll("iframe");

        // console.log(iframes);

        const currentIframe = document.getElementById("middle1");

        let transID;

        if (currentIframe) {
            //  const src = currentIframe.src;
            //  const fetchTransidfromUrl = src.match(/[?&](transid|tstid)=([^&]+)/i);

            //const key = fetchTransidfromUrl ? fetchTransidfromUrl[1] : null;
            //const value = fetchTransidfromUrl ? fetchTransidfromUrl[2] : null;
            //console.log(key, value);
            //transID = value;
            transID = currentIframe.contentWindow.transid;

            if (transID) {
                //console.log("iFrame Found");
                return transID;
            }
            else
                return null;
        }
        else {
            return null;
        }



    }


    function AxisetFieldValue(actualFieldName, value, rowNo) {
        const fldid = actualFieldName + "000F" + rowNo;

        const iframe = document.getElementById("middle1");

        if (iframe) {
            const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
            const iframeDocElement = iframeDoc.getElementById(fldid);
            //const iframeDoc = iframe.contentWindow;

            //$("#middle1")[0].contentWindow.CallSetFieldValue(fldid, FldVal);
            //$("#middle1")[0].contentWindow.MainBlur($(fldid))

            //$("#middle1")[0].contentWindow.MainBlur($(fldid))
            //$("#middle1")[0].contentWindow.UpdateFieldArray(fldid, fldDbRowNo, fldValue, "parent", "");
            //$("#middle1")[0].contentWindow.CallSetFieldValue(fldid, fldValue);

            if (iframeDocElement) {

                ///
                /// For dependency we need to verify this logic.
                ////
                
                iframeDocElement.value = value;

                iframeDocElement.dispatchEvent(new Event("input", { bubbles: true }));
                iframeDocElement.dispatchEvent(new Event("change", { bubbles: true }));
                iframeDocElement.dispatchEvent(new Event("blur", { bubbles: true }));


                ///Product Field Set Logic
                //iframeDoc.UpdateFieldArray(fldid, rowNo, value, "parent", "");
                //iframeDoc.CallSetFieldValue(fldid, value);
                //iframeDoc.MainBlur($(fldid));
                return true;
            }
            else {
                return false;
            }
        }
        else return false;
    }

    function handleCreate({ tokens, commandConfig }) {

        if (tokens.length < 2) {
            console.warn("create command requires <tstructname> <fieldname> <fieldvalue>");
            showToast("create command requires <tstructname> <fieldname> <fieldvalue>");
            return;
        }

        resetSetCommandState();

        if (tokens.length <= 3) {
            let rawName = cleanCommandToken(tokens[1]);
            let transId = tryResolveToken(1, rawName, commandConfig, false);

            if (transId === rawName) {
                const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
                const found = list?.find(
                    x => x.caption.toLowerCase() === rawName.toLowerCase()
                );
                if (found) transId = found.name
                else {
                    console.error("Invalid Tstruct name");
                    return;
                }
            }

            redirectToTstruct(transId);
            return;

        }

        let rawName = cleanCommandToken(tokens[1]);
        let transId = tryResolveToken(1, rawName, commandConfig, false);

        if (!transId || createfieldnamevaluesList.length == 0) {
            console.log("Missing transaction ID or field values.");
            showToast("Some required information is missing. Please re-enter the command and try again.");
            return;
        }

        //let CurrentOpentstructName = getTransID();

        //if (CurrentOpentstructName === transId.toLowerCase()) {

        //    //for (let i = 2; i < tokens.length; i += 2) {

        //    for (let i = 0; i < createfieldnamevaluesList.length; i++) {

        //        const item = createfieldnamevaluesList[i];
        //        if (!item) continue;

        //        const parts = item.split("=");
        //        if (parts.length !== 2) continue;

        //        const left = parts[0];
        //        const value = parts[1];
        //        const leftParts = left.split("~");
        //        if (leftParts.length !== 2) continue;
        //        const fieldname = leftParts[0];
        //        const rowNo = leftParts[1];

        //        //const fieldname = cleanCommandToken(tokens[i]);
        //        //const actualFieldName = tryResolveToken(i+3, fieldname, commandConfig, false);
        //        const actualFieldName = fieldname;
        //        //const value = cleanCommandToken(tokens[i + 1]);
        //        //const rowNo = "1";

        //        if (!fieldname || value == null || value === "") {
        //            console.log(`SET FAILED → ${fieldname} = ${value}`);
        //            continue;
        //        }


        //        let resultOfSetFieldValue = AxisetFieldValue(actualFieldName, value, rowNo);

        //        if (resultOfSetFieldValue) {
        //            console.log(`SET SUCCESS → ${fieldname} = ${value}`);
        //            continue;

        //        }
        //        else {
        //            console.log(`SET FAILED → ${fieldname} = ${value}`);
        //        }
        //    }
        //    createfieldnamevaluesList = []
        //}
        //else 
        //{
            console.log("Form not loaded. Attaching onload and redirecting...");

            let iframe = null;

            try {

                iframe = document.getElementById("middle1");

                if (!iframe) {
                    console.log("Iframe not found");
                    return;
                }

              
                iframe.onload = function () {
                    //we can also try: 0 (minimum delay),50,100,200 (if needed)
                    console.log("Iframe loaded. Setting all fields now...");
                    setTimeout(function () {
                    try {

                        console.log(createfieldnamevaluesList);
                        //for (let j = 2; j < tokens.length; j += 2) {
                        for (let j = 0; j < createfieldnamevaluesList.length; j++) {

                            const item = createfieldnamevaluesList[j];
                            if (!item) continue;

                            const parts = item.split("=");
                            if (parts.length !== 2) continue;

                            const left = parts[0];
                            const val = parts[1];
                            const leftParts = left.split("~");
                            if (leftParts.length !== 2) continue;
                            const fieldname = leftParts[0];
                            const rowNo = leftParts[1];

                            //const fname = cleanCommandToken(tokens[j]);
                            //const actualFName = tryResolveToken(j + 3, fieldname, commandConfig, false);
                            const actualFName = fieldname;
                            //const value = cleanCommandToken(tokens[i + 1]);
                            //let rowNo = "1";

                            if (!actualFName || val == null || val === "") {
                                console.log(`SET FAILED → ${fname} = ${val}`);
                                continue;
                            }

                            let resultOfSetFieldValue = AxisetFieldValue(actualFName, val, rowNo);

                            if (resultOfSetFieldValue) {
                                console.log(`SET SUCCESS → ${actualFName} = ${val}`);
                            }
                            else {
                                console.log(`SET FAILED → ${actualFName} = ${val}`);
                            }
                        }

                    }
                    catch (ex) {
                        console.error("Error while setting fields after iframe load:", ex);
                    }
                    finally {
                        iframe.onload = null;
                        createfieldnamevaluesList = [];
                        }
                    }, 100);
                };

                setEditSessionState(transId);

                redirectToTstruct(transId);

            }
            catch (ex) {
                console.log("Error in handleCreate: " + ex);
                createfieldnamevaluesList = [];
            }
            //finally {
            //    if (iframe) {
            //        iframe.onload = iframe.onload;
            //    }
            //}


        //}
    }

    function prepareKeyValueString(data) {
        if (!data || !Array.isArray(data.globalVars)) return "";

        return data.globalVars
            .map(obj => {
                const key = Object.keys(obj)[0];
                const value = obj[key];
                return key + "~" + value;
            })
            .join(";");
    }
    function getFieldNameandItsValue(fieldValueList, commandConfig) {

        let fieldValue = "";

        if (!fieldValueList || fieldValueList.length === 0) {
            return fieldValue;
        }

        for (let i = 0; i < fieldValueList.length; i++) {

            let item = fieldValueList[i];

            if (!item) continue;

            let parts = item.split("=");

            if (parts.length !== 2) continue;

            let leftPart = parts[0];  
            let valuePart = parts[1];  

            let fieldParts = leftPart.split("~");

            let fieldName = fieldParts[0];  

            fieldValue += fieldName + "~" + valuePart + ";";
        }

        return fieldValue;
    }



    function isValidDate(value) {
        if (!value || typeof value !== "string") return false;

        value = value.trim();

        if (value.length === 0) {
            return false;
        }

        let day, month, year;

        // Format: DD/MM/YYYY or D/M/YYYY or DD-MM-YYYY or D-M-YYYY
        let dmyRegex = /^(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})$/;

        // Format: YYYY-MM-DD
        let ymdRegex = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;

        if (dmyRegex.test(value)) {
            const match = value.match(dmyRegex);
            day = parseInt(match[1], 10);
            month = parseInt(match[2], 10);
            year = parseInt(match[3], 10);
        }
        else if (ymdRegex.test(value)) {
            const match = value.match(ymdRegex);
            year = parseInt(match[1], 10);
            month = parseInt(match[2], 10);
            day = parseInt(match[3], 10);
        }
        else {
            return false; // format mismatch
        }

        // Month range
        if (month < 1 || month > 12) return false;

        // Day range
        const daysInMonth = new Date(year, month, 0).getDate();
        if (day < 1 || day > daysInMonth) return false;

        return true;
    }



    function formatDate(date, format) {
        if (!date) return null;

        if (typeof date === "string") {
            let m;
            if ((m = date.match(/^(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})$/)) || (m = date.match(/^(\d{4})-(\d{1,2})-(\d{1,2})$/))) {
                date = new Date(
                    m[1].length === 4 ? m[1] : m[3],
                    (m[1].length === 4 ? m[2] : m[2]) - 1,
                    m[1].length === 4 ? m[3] : m[1]
                );

            } else {
                return null;
            }
        }

        if (!(date instanceof Date) || isNaN(date.getTime())) {
            return null;
        }

        const dd = String(date.getDate()).padStart(2, "0");
        const mm = String(date.getMonth() + 1).padStart(2, "0");
        const yyyy = date.getFullYear();

        switch (format.toLowerCase()) {
            case "YYYYMMDD".toLowerCase() :
                return `${yyyy}${mm}${dd}`;

            case "DDMMYYYY".toLowerCase() :
                return `${dd}${mm}${yyyy}`;

            case "YYYY-MM-DD".toLowerCase() :
                return `${yyyy}-${mm}-${dd}`;

            case "DD-MM-YYYY".toLowerCase() :
                return `${dd}-${mm}-${yyyy}`;

            case "YYYY/MM/DD".toLowerCase() :
                return `${yyyy}/${mm}/${dd}`;

            case "DD/MM/YYYY".toLowerCase() :
                return `${dd}/${mm}/${yyyy}`;

            case "YYYY.MM.DD".toLowerCase() :
                return `${yyyy}.${mm}.${dd}`;

            case "DD.MM.YYYY".toLowerCase() :
                return `${dd}.${mm}.${yyyy}`;

            default:
                return date; // fallback
        }
    }


    function getDateByFilter(type) {
        const baseDate = new Date();
        baseDate.setHours(0, 0, 0, 0);

        let date = null;
        type = type.toLowerCase();

        switch (type) {
            case "today":
                date = formatDate(baseDate, "DD/MM/YYYY");
                break;

            case "yesterday": {
                const d = new Date(baseDate);
                d.setDate(d.getDate() - 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "tomorrow": {
                const d = new Date(baseDate);
                d.setDate(d.getDate() + 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "lastweek": {
                const d = new Date(baseDate);
                d.setDate(d.getDate() - 7);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "nextweek": {
                const d = new Date(baseDate);
                d.setDate(d.getDate() + 7);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "lastyear": {
                const d = new Date(baseDate);
                d.setFullYear(d.getFullYear() - 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "custom":
            default:
                return {
                    date: "",
                    openDatePicker: true
                };
        }

        return {
            date,
            openDatePicker: false
        };
    }


    async function getARMSessionId() {
        if (cachedSessionId) return cachedSessionId;

        const res = await fetch(webUrl + "/WebService.asmx/GetSession", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ key: "ARM_SessionId" })
        });

        const data = await res.json();
        cachedSessionId = data.d;

        return cachedSessionId;
    }



    //function AxisaveDataFn(createfieldnamevaluesList, transid, sourceName, isCreate) {


    //    //const sessionResponse = fetch("GetSession", {
    //    //    method: "POST"
    //    //});
    //    let sessionId;
    //    getARMSessionId().then(sessionIdFetch => {
    //        sessionId = sessionIdFetch;
    //        console.log(sessionIdFetch);
    //    });

    //    if (!sessionId) return ["Session expired"]

    //    //const sessionId = getSession("ARM_SessionId");
    //    //if (!sessionId) return ["Session expired"];

    //    const colSourceKey = (sourceName + "_" + transid).toLowerCase();
    //    const colList = axDatasourceObj[colSourceKey];

    //    const submitdata = {};

    //    for (let i = 0; i < createfieldnamevaluesList.length; i++) {

    //        const item = createfieldnamevaluesList[i];

    //        const parts = item.split("=");
    //        const left = parts[0];
    //        const value = parts[1];

    //        const leftParts = left.split("~");
    //        const fieldName = leftParts[0];
    //        const rowNo = leftParts[1];

    //        const columnMetadata = colList.find(c =>
    //            c.name?.toLowerCase() === fieldName.toLowerCase()
    //        );

    //        if (!columnMetadata) continue;

    //        const dcName = columnMetadata.dcname;
    //        const isGrid = columnMetadata.asgrid;

    //        if (!submitdata[dcName]) {
    //            submitdata[dcName] = {};
    //        }

    //        const rowKey = isGrid ? "row" + rowNo : "row1";

    //        if (!submitdata[dcName][rowKey]) {
    //            submitdata[dcName][rowKey] = {};
    //        }

    //        // If EDIT → add edit-specific fields
    //        if (!isCreate) {
    //            submitdata[dcName][rowKey]["axrow_action"] = "edit";
    //        }

    //        submitdata[dcName][rowKey][fieldName] = value;
    //    }

    //    //const sessionResult = sessionResponse.json();
    //    //const sessionId = sessionResult.d;
    //    //if (!sessionId) return ["Session expired"];

    //    const payload = {
    //        ARMSessionId: sessionId,
    //        trace: true,
    //        data: [
    //            {
    //                transid: transid,
    //                action: isCreate ? "create" : "edit",
    //                submitdata: submitdata
    //            }
    //        ]
    //    };

    //    console.log("AxPut Payload created : " + payload);
    //    //If EDIT → attach keyfield & keyvalue at root level
    //    if (!isCreate) {
    //        payload.data[0]["keyfield"] = "recordid";
    //        payload.data[0]["keyvalue"] = "";
    //    }

    //    var apiUrl = armUrl + "/api/v1/AxPut";

    //    try {

    //        const response = fetch(apiUrl, {
    //            method: "POST",
    //            headers: {
    //                "Content-Type": "application/json"
    //            },
    //            body: JSON.stringify(payload)
    //        });

    //        //const result = await response.json();

    //    } catch (error) {
    //        console.log("Error from Axput : "+ error);
    //    }


    //}

    //function AxisaveDataFn(createfieldnamevaluesList, transid, sourcename, iscreate = true) {

    //    let isSuccessCheck = true;
    //    var result = preparePayload(createfieldnamevaluesList, transid, sourcename, iscreate = true);

    //    if (result.isSuccess) {
    //        console.log("Payload Ready");
    //        console.log(result.payload);
    //        payload = result.payload;
    //    } else {
    //        console.log("Payload is empty");
    //        payload = {};
    //    }

    //    var apiUrl = armUrl + "/api/v1/AxPut";

    //        try {

    //            const response = fetch(apiUrl, {
    //                method: "POST",
    //                headers: {
    //                    "Content-Type": "application/json"
    //                },
    //                body: JSON.stringify(payload)
    //            });

    //            //const result = await response.json();
    //            if (response.isSuccess) {
    //                showToast("Your Data is submitted Successfully!!");
    //                console.log("Data is submitted Successfully!!");
    //            }

    //        } catch (error) {
    //            showToast("Your Data is not Submitted..Please Submit it Manually!!");
    //            console.log("Error from Axput : "+ error);
    //        }


    //}

    function AxisaveDataFn(saveListWithFieldNamendValues, transid, sourcename, isCreate, inputTokens, inputCommandConfig) {

        //getARMSessionId()
        //    .then(sessionId => {

        //        if (!sessionId) {
        //            showToast("Session not found. Please login again....");
        //            return;
        //        }

        //        console.log("Fetched Session ID: " + sessionId);

        // 👉 Call preparePayload AFTER session is ready
        if (!transid || saveListWithFieldNamendValues.length == 0) {
            console.log("Missing transaction ID or field values.");
            showToast("Some required information is missing. Please re-enter the command and try again.");

            return [];
        }

        let keyFieldValue = cleanCommandToken(inputTokens[2]);
        let keyfieldName;
        if (!isCreate) {
            const extraPromptSource = inputCommandConfig.prompts[1].extraParams.toLowerCase();

            const extraSourceKey = `${extraPromptSource}_${transid}`.toLowerCase();

            const extraList = axDatasourceObj[extraSourceKey];

            if (extraList.length == 0) {
                throw new Error("Key Field List is missing");
            }
            const field = extraList[0];

            keyfieldName = field.fname ?? field.keyfield ?? field.name ?? field.displaydata;
        }
        preparePayload(saveListWithFieldNamendValues, transid, sourcename, isCreate, keyFieldValue, keyfieldName).then(result => {

            if (!result || !result.isSuccess) {
                throw new Error("Payload is empty or invalid");
            }

            console.log("Payload is Created successfully " + "\n");
            console.log(result.payload);

            var payload = result.payload;


            var SaveDataapiUrl = mainArmRestDllPath + "ASBTStructrest.dll/datasnap/rest/TASBTstruct/savedata";

            console.log("SaveDataapi URL : " + SaveDataapiUrl);

            showToast("Saving Data is in progress....", 5000, true);

            return fetch(SaveDataapiUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(payload)
            })

                //})
                .then(response => {

                    if (!response) throw new Error("API SaveData return Empty response");;

                    if (!response.ok) {
                        throw new Error("API SaveData Error");
                    }

                    return response.json();
                })
                .then(data => {

                    if (!data) {
                        throw new Error("Invalid API response");
                    }

                    const firstResult = data.result?.[0];

                    if (!firstResult) {
                        throw new Error("Invalid API response");
                    }

                    if (firstResult.error) {
                        showToast(`Save Failed : ${firstResult.error.msg}`);
                        console.log(`Save Failed : ${firstResult.error.msg}`);
                        console.log(data);
                        return [];
                    }

                    if (firstResult.message?.[0]) {
                        const msgObj = firstResult.message[0];
                        const msg = msgObj.msg || "Saved Successfully";
                        //const msg = "Set Success"
                        const recordId = msgObj.recordid || "";
                        const sid = msgObj.SID || "";
                        showToast(`${msg}`, 5000, true);
                        console.log("Data submitted successfully,Record-ID : " + recordId);
                        console.log(data);

                        saveListWithFieldNamendValues = [];
                        createfieldnamevaluesList = [];
                        return [];
                    }

                    console.log(data);
                    throw new Error("Unexpected API response structure");

                })
                .catch(error => {

                    //showToast("Your Data is not Submitted,Please Submit it Manually using(Ctrl + Enter)!");
                    showToast("Save failed.Please retry with Ctrl + Enter.");
                    console.log("Error from AxiSaveDataFn :" + error);
                    return [];

                });

        });
        //var result = preparePayload(saveListWithFieldNamendValues, transid, sourcename, isCreate, keyFieldValue, keyfieldName);

        //if (!result || !result.isSuccess) {
        //    throw new Error("Payload is empty or invalid");
        //}

        
    }


    function getUserPassword(username) {

        const sourceName = "axi_userpwd";
        const paramValue = username;
        const sourceKey = `${sourceName}_${paramValue}`.toLowerCase();

        if (axDatasourceObj[sourceKey]) {
            return Promise.resolve(
                axDatasourceObj[sourceKey]?.[0]?.password || null
            );
        }

        return loadList(sourceName, paramValue)
            .then(() =>
                axDatasourceObj[sourceKey]?.[0]?.password || null
            );
    }
    function preparePayload(saveListWithFieldNamendValues,transid, sourcename, iscreate,inputKeyFieldValue,inputKeyFieldName) {

        let isSuccess = true;
        let payloadUsername = mainUserName;

        if (!payloadUsername || payloadUsername.trim() === "") {
            console.log("Username not found or invalid.");
            return Promise.resolve({
                isSuccess: false,
                payload: {}
            });
        }

        return getUserPassword(payloadUsername).then(password => {

            let payload;
            const formatRowNo = (n) => String(n).padStart(3, "0");
            const dcMap = {};

            const colSourceKey = (sourcename + "_" + transid).toLowerCase();
            const colList = axDatasourceObj[colSourceKey];

            if (!colList) {
                console.warn("Column metadata not found for:", colSourceKey);
                return {
                    isSuccess: false,
                    payload: {}
                };
            }


            // 🔥 Record level variables
            var recordid = iscreate ? "0" : "";
            var keyfield = iscreate ? "" : "";
            var keyvalue = iscreate ? "" : "";

            //👉 In edit mode you will later assign:
            if (!iscreate) {
                recordid = "0";
                keyfield = inputKeyFieldName;
                keyvalue = inputKeyFieldValue;
            }
            try {
                for (let i = 0; i < saveListWithFieldNamendValues.length; i++) {

                    const item = saveListWithFieldNamendValues[i];
                    if (!item) continue;

                    const parts = item.split("=");
                    if (parts.length !== 2) continue;

                    const left = parts[0];
                    const value = parts[1];

                    const leftParts = left.split("~");
                    if (leftParts.length !== 2) continue;

                    const fieldName = leftParts[0];
                    const rowNo = leftParts[1];

                    const columnMetadata = colList.find(c =>
                        c.name?.toLowerCase() === fieldName.toLowerCase()
                    );

                    if (!columnMetadata || !columnMetadata.dcname) continue;

                    const dcName = columnMetadata.dcname;
                    const recKey = `axp_recid${dcName.replace("dc", "")}`;

                    if (!dcMap[recKey]) {
                        dcMap[recKey] = [];
                    }

                    let rowObj = dcMap[recKey].find(
                        r => r.rowno === formatRowNo(rowNo)
                    );

                    if (!rowObj) {
                        rowObj = {
                            rowno: formatRowNo(rowNo),
                            text: recordid,
                            columns: {}
                        };
                        dcMap[recKey].push(rowObj);
                    }

                    rowObj.columns[fieldName] = value;
                }

                const recdata = Object.keys(dcMap).map(recKey => ({
                    [recKey]: dcMap[recKey]
                }));


                if (!password || password.trim() === "") {
                    throw new Error("Password not found or invalid.");
                }
                if (!mainProject || mainProject.trim() === "") {
                    throw new Error("ProjectName not found or invalid.");
                }
                if (!mainSessionId || mainSessionId.trim() === "") {
                    throw new Error("SessionId not found or invalid.");
                }


                payload = {
                    savedata: {
                        cachedsave: "true",
                        axpapp: mainProject,
                        //appsessionkey: "010198670011016401680166017301540168015301640101009800994939364141171051310018",
                        transid: transid,
                        s: mainSessionId,
                        username: payloadUsername,
                        password: password,
                        changedrows: {},
                        trace: "true",
                        recordid: recordid,
                        keyfield: keyfield,
                        keyvalue: keyvalue,
                        recdata: recdata,
                        globalvars: {},
                        uservars: {},
                        axapps: {}
                    }
                };

                console.log("PreparedPayload : Payload is created successfully with isCreate :", iscreate);
                console.log(payload);

            }
            catch (ex) {
                isSuccess = false;
                console.log("Error in preparePayload Payload creation :" + ex);
            }

            if (payload && Object.keys(payload).length > 0 && isSuccess) {
                return {
                    isSuccess: isSuccess,
                    payload: payload
                };
            }
            else {
                return {
                    isSuccess: false,
                    payload: {}
                };
            }
        })

    }





    function redirectToAxibot() {


        // let targetUrl = "../CustomPages/axibot.html";
        // let targetUrl = `${getAppBaseUrl()}/CustomPages/axibot.html`;
        let targetUrl = `${getAppBaseUrl()}/plugins/Axi/HTMLPages/axibot.html`;
        // let targetUrl = "../axidev/HTMLPages/axibot_1770979038509.html";




        console.log("Target Url for AxiBot:  " + targetUrl);



        top.window.LoadIframe(targetUrl);


    }

    function handleAiStart() {
        mode = "ai";
        commands = aiModeCommands;

        redirectToAxibot();

    }

    function getAxiBotActionButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const buttons = doc.querySelectorAll(".actionGroup button");
        const result = {};

        buttons.forEach((btn, index) => {
            if (btn.classList.contains("d-none")) return;

            const id =
                btn.id ||
                btn.getAttribute("title") ||
                `axibot-btn-${index}`;

            const label = btn.innerText?.trim();
            if (!label) return;

            result[id] = {
                id,
                label: label.toLowerCase(),
                element: btn,
                click: () => btn.click()
            };
        });

        const uploadBtn = doc.getElementById("openUpload");

        if (uploadBtn) {
            result["openUpload"] = {
                id: "openUpload",
                label: "upload files",
                element: uploadBtn,
                click: () => uploadBtn.click()
            }
        }

        return result;
    }


    function handleAiButtons(btnId) {
        const axiButtons = getAxiBotActionButtons();
        console.log(axiButtons);


        if (axiButtons) {
            axiButtons[btnId].click();


        } else {
            console.error("Cannot get Axi bot action buttons");
            showToast("Error: Cannot get Axi bot action buttons")
        }

    }

    function handleSendAxiMessageToAxiBot(text) {

        const iframe = document.getElementById("middle1");

        if (!iframe) {
            console.error("Axi bot: Iframe middle1 not found.");
            return;
        }

        const iframeWindow = iframe.contentWindow;

        if (!iframeWindow) {
            console.error("Axi Bot: Cannot access iframe window.");
            return;
        }

        if (typeof iframeWindow.sendAxiMessageToAxibot === "function") {
            console.log(`Sending to AxiBot: "${text}"`);
            iframeWindow.sendAxiMessageToAxibot(text);
        } else {
            console.warn("Axi Bot: Script not fully loaded in iframe yet.");
            if (typeof showToast === 'function') showToast("Chatbot is loading...");
        }

    }

    function getAskText(tokens) {
        const askIndex = tokens.findIndex(t => t.toLowerCase() === "ask");

        return askIndex !== -1
            ? tokens.slice(askIndex + 1).join(" ")
            : "";
    }

    function handleAiAsk({ tokens, commandConfig }) {
        const text = getAskText(tokens);

        handleSendAxiMessageToAxiBot(text);


    }

    function handleAiEnd() {
        if (mode === "") {
            return;
        }
        mode = "";
        cachedCommands = localStorage.getItem("axi_commands_v1");
        commands = JSON.parse(cachedCommands);
        window.LoadIframe("loadhomepage");
        console.log(JSON.stringify(commands));
    }






})();
