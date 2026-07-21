// Stable Branch: main
(() => {
    // Released On: 11/06/2026
    // /AxPlugins/Axi/HTMLPages/js/axicmdmain.js
    
    let apiMetadataUrl = "";
    let apiMetadataConfigPromise = null;
    let apiMetadataConfigError = "";
    let settingsPageButtons = null;
    let importExportButtons = null;
    let commandHistory = [];
    const MAX_HISTORY = 10;
    let historyIndex = -1;
    let megaDropdown;
    let favouritesCard;
    let commandHeader;
    let hiddenLoader;
    let favouriteBtn;
    let commandFavorites = [];
    let axiFavoritesUrl = "";
    const MAX_FAVORITES = 20;
    let commandRoutes = [];
    let isDeleting = false;
    let isEditing = false;
    let isProgrammaticExecution = false;
    let suppressFocusSuggestions = false;

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

    const popOption = {
        displaydata: "Pop-Up [Ctrl + Shift + Enter]",
        name: "Pop_ACTION",
        isExecutable: true
    };

    const sourceOption = {
        displaydata: "Source [Ctrl + Alt + Enter",
        name: "Source_ACTION",
        isExecutable: true
    }



    const VIEW_HANDLERS = {
        tstruct: ({ transId, rawStruct, fieldName, fieldValue }) =>
            redirectToEntity(transId, rawStruct, fieldName, fieldValue),

        iview: ({ transId, rawStruct }) =>
            redirectToIView(transId, rawStruct),

        page: ({ transId, fieldName, fieldValue }) =>
            redirectToEntity(transId, fieldName, fieldValue),

        ads: ({ transId, fieldName, fieldValue }) => redirectToEntity(transId, "", fieldName, fieldValue),

        inbox: () => handleViewInbox()
    };






    const COMMAND_HANDLERS = {
        show: {
            toast: () => showToast(input.value)

        },
        Edit: {
            default: handleEditData,
            data: handleEditData,
            // user: handleEditUser



        },
        Create: {
            default: handleCreate,




        },
        View: {
            default: handleViewCommand,
            source: handleViewSource,



        },
        Source: {
            default: handleSourceCommand,
        },
        Configure: {
            peg: handleConfigurePeg,

            //api: handleConfigureApi,

            "application properties": handleConfigureProperties,
            //job: handleConfigureJob,
            rule: handleConfigureRule,
            //server: handleConfigureServer,
            "form notification": handleConfigureFormNotification,
            "peg form notification": handleCofigurePegFormNotification,
            //permission: handleConfigurePermissions,
            //access: handleConfigureAccess,
            "scheduled notification": handleConfigureScheduledNotification,
            keyfield: handleKeyfield,
            "news and announcement": handleConfigureNewsAndAnnouncement,
            settings: handleConfigureSettings,

            users: handleConfigureUsers,
            user: handleConfigureUser,
            roles: handleConfigureRoles,
            role: handleConfigureRole,
            "publish axpert api": handleConfigurePublishAxpertApi,
            //"publish api listing": handleApiList,
            "publish config studio": handleConfigurePublishListing,
            card: handleConfigureCards,
            //forms: handleForms,
            responsibility: handleConfigureResponsibility,
            responsibilities: handleConfigureResponsibilities,
            "user group": handleConfigureUserGroup,
            dimension: handleConfigureDimensions,
            actors: handleConfigureActorListing,
            actor: handleConfigureActor,
            // useractivation: handleUserActivation,
            "user activation": handleConfigureUserActivation,
            "user permissions": handleConfigureUserPermissionListing,
            "user permission": handleConfigureUserPermission,
            "role permissions": handleConfigureRolePermissionListing,
            //"role permission": handleRolePermission
            "smart view attributes": handleConfigureSmartViewAttributes
        },
        //Open: {
        SDK: {
            default: handleOpenSource,
            "axpert data sources": handleOpenAds,
            //card: handleOpenCard,
            page: handleOpenPage,
            "app variables": handleOpenAppVar,
            "dev option": handleOpenDevOptions,
            "db explorer": handleOpenDbConsole,
            "arrange menu": handleOpenArrangeMenu,

            "api plugin": handleOpenApi,
            "axpert job": handleOpenJob,
            language: handleOpenLanguage,
            publish: handleOpenPublish,
            "custom data type": handleOpenCustomDataType,
            "email definition": handleOpenEmailDef,
            "table field descriptor": handleOpenTableFieldDescriptor,
            "mem db console": handleOpenMemDBConsole,
            "custom plugin": handleOpenCustomPlugin,
            "queue listing": handleQueueListing,
            //"outbound queue mapping":handleOpenOutBoundQueueM,
            "out bound queue": handleOpenOutBoundQueue,
            "in bound queue": handleOpenInboundBoundQueue


        },
        Upload: {
            default: handleUpload
        },
        Download: {
            default: handleDownload
        },
        Set: {
            default: () => console.log("set command!")
        },
        Run: {
            default: handleRunCommand,
        },
        // Analyse: {
        //     default: handleAnalyse
        // },
        Ai: {
            start: handleAiStart,

        },
        connect: { default: () => handleAiButtons("openConnect"), },
        ask: { default: handleAiAsk, },
        end: { default: handleAiEnd, },
        editprompt: { default: () => handleAiButtons("openSystemPrompt") },
        analyze: { default: () => handleAiButtons("axiLoad"), },
        // upload: { default: () => handleAiButtons("openUpload") }
    };



    let SET_COMMAND_STATE = {
        isNextField: false,
        currentField: null,
        currentFieldType: null,
        isFirst: true,
        transid: null,
        currentFieldValue: null,
        isDropDown: false
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
    let dateControlBoolean = false;
    let popUpOption = false;

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
    let resolvedParamType = {};
    let lastTypedTokens = [];

    // DATA CACHES
    let axDatasourceObj = {};
    let activeFetches = new Set();
    let filteredObjects = [];
    let adsfieldvalueanddt = {};
    let createfieldnamevaluesList = {};
    let AxiArmUrl;
    let isConfigLoaded = false;
    let mode = "";
    let isCommandsLoading = false;
    const aiModeCommands = {
        "connect": { "cmdToken": 11, "command": "", "commandGroup": "connect", "prompts": [] },
        "ask": { "cmdToken": 11, "command": "", "commandGroup": "ask", "prompts": [{ "cmdToken": 11, "wordPos": 2, "prompt": "Chat", "promptSource": "", "promptParams": "", "promptValues": "", "extraParams": "" }] },
        "end": { "cmdToken": 11, "command": "", "commandGroup": "end", "prompts": [] },
        "editprompt": { "cmdToken": 11, "command": "", "commandGroup": "editprompt", "prompts": [] },
        "analyze": { "cmdToken": 11, "command": "", "commandGroup": "analyze", "prompts": [] },
        "upload": { "cmdToken": 11, "command": "", "commandGroup": "upload", "prompts": [] }
    }




    function init() {
        let parentUserName = "";
        let parentProject = "";
        let parentUserRoles = "";
        let parentUserResp = "";
        let parentArmUrl = "";

        // 1. Try callParentNew
        try {
            if (typeof callParentNew === "function") {
                parentUserName = callParentNew("mainUserName") || "";
                parentProject = callParentNew("mainProject") || "";
                parentUserRoles = callParentNew("AxUserRoles") || "";
                parentUserResp = callParentNew("userResp") || "";
                parentArmUrl = callParentNew("armUrl") || "";
            }
        } catch (e) {
            console.warn("callParentNew failed", e);
        }

        // 2. Try parent window
        try {
            if (typeof parent !== "undefined" && parent) {
                if (!parentUserName) parentUserName = parent.mainUserName || "";
                if (!parentProject) parentProject = parent.mainProject || "";
                if (!parentUserRoles) parentUserRoles = parent.AxUserRoles || "";
                if (!parentUserResp) parentUserResp = parent.userResp || "";
                if (!parentArmUrl) parentArmUrl = parent.armUrl || "";
            }
        } catch (e) {
            console.warn("parent access failed", e);
        }

        // 3. Try top window
        try {
            if (typeof top !== "undefined" && top) {
                if (!parentUserName) parentUserName = top.mainUserName || "";
                if (!parentProject) parentProject = top.mainProject || "";
                if (!parentUserRoles) parentUserRoles = top.AxUserRoles || "";
                if (!parentUserResp) parentUserResp = top.userResp || "";
                if (!parentArmUrl) parentArmUrl = top.armUrl || "";
            }
        } catch (e) {
            console.warn("top access failed", e);
        }

        // 4. Try local variable scope fallback
        try {
            if (!parentArmUrl && typeof armUrl !== "undefined") {
                parentArmUrl = armUrl || "";
            }
        } catch (e) {}

        if (typeof window.mainUserName === "undefined" || !window.mainUserName) {
            window.mainUserName = parentUserName || "";
        }
        if (typeof window.mainProject === "undefined" || !window.mainProject) {
            window.mainProject = parentProject || "";
        }
        if (typeof window.AxUserRoles === "undefined" || !window.AxUserRoles) {
            window.AxUserRoles = parentUserRoles || "";
        }
        if (typeof window.userResp === "undefined" || !window.userResp) {
            window.userResp = parentUserResp || "";
        }

        let globalArmUrl = parentArmUrl || "";

        if (globalArmUrl) {
            AxiArmUrl = globalArmUrl;
        }

        if (!AxiArmUrl && !isConfigLoaded) {
            isConfigLoaded = true;
            let configUrl = "";
            try {
                configUrl = `${getAppBaseUrl()}/AxpertPlugins/Axi_Beta/axicmd-config.json`;
            } catch (e) {
                configUrl = `/AxpertPlugins/Axi_Beta/axicmd-config.json`;
            }
            try {
                const res = fetch(configUrl);
                if (res.ok) {
                    const config = res.json();
                    AxiArmUrl = config.axiarmurl || config.armUrl || config.axiArmUrl || "";
                }
            } catch (err) {
                console.error("Failed to load AxiArmUrl from config file:", err);
            }
        }

        console.log("AxiArmUrl = " + AxiArmUrl);
        apiMetadataUrl = `${AxiArmUrl}/AxiApi_Beta/api/v1/Axi/axi_get`;
        console.log("ApiMetadataUrl = " + apiMetadataUrl);

        axiFavoritesUrl = `${AxiArmUrl}/AxiApi_Beta/api/v1/Axi/user-favourites`;
        // axiFavoritesUrl = `http://localhost:5057/api/v1/Axi/user-favourites`; 
        console.log("AxiFavoritesUrl = " + axiFavoritesUrl);


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

        megaDropdown = document.getElementById("axiMegaDropdown");







        console.log("Axi Clear button found!", megaDropdown);

        favouritesCard = document.getElementById("axiFavouritesCard");
        console.log("Axi Clear favouritesCard Found!", favouritesCard);

        commandHeader = document.getElementById("axiCommandsHeader");
        console.log("Axi Command Header found!", commandHeader);

        hiddenLoader = document.getElementById("hiddenLoader");
        console.log("Axi hidden Loader found!", hiddenLoader);


        favouriteBtn = document.getElementById("axiFavouriteBtn");

        console.log("Axi Favourite Button found!", favouriteBtn);






        setupEventListeners();



        initCommands(false);
        loadFavorites();
    }

    let initRetries = 0;
    function startInit() {
        const proj = window.mainProject || (typeof callParentNew === "function" && callParentNew("mainProject"));
        const user = window.mainUserName || (typeof callParentNew === "function" && callParentNew("mainUserName"));
        
        if ((!proj || !user) && initRetries < 20) {
            initRetries++;
            setTimeout(startInit, 100);
            return;
        }
        init();
    }

    startInit();

    function getProjectName() {
        // let appSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        // console.log("Origin: " + appSessUrl);
        // const projInfoKey = `projInfo-${appSessUrl}`;

        // const appname = localStorage.getItem(projInfoKey);

        // console.log(appname);
        // return appname;

        return window.mainProject || 
               (typeof callParentNew === "function" && callParentNew("mainProject")) || 
               (typeof parent !== "undefined" && parent.mainProject) || 
               (typeof top !== "undefined" && top.mainProject) || 
               "";
    }

    /* ===============================
        INITIALIZATION
    =============================== */
    async function initCommands(isForced = false) {
        const structType = getStructType();
        if (mode === "ai") {
            commands = aiModeCommands;
            return;
        }

        const appname = getProjectName();
        console.log(appname);

        if (!apiMetadataUrl) {
            console.error("Metadata API URL is not configured.", apiMetadataConfigError);
            showToast("Metadata API URL is not configured.");
            return;
        }

        try {
            isCommandsLoading = true;
            input.disabled = false;
            input.placeholder = "Axpert AI";

            const cached = localStorage.getItem("axi_commands_raw_v1");
            let commandsFromDb = null;
            if (cached && !isForced) {
                try {
                    commandsFromDb = JSON.parse(cached);
                } catch (e) {
                    console.error("Failed to parse cached raw commands", e);
                }
            }

            if (!commandsFromDb) {
                const res = await fetch(`${apiMetadataUrl}?view=metadata&forceRefresh=${isForced}&appname=${appname}`);

                if (!res.ok) {
                    showToast("Metadata fetch failed. Please contact Administrator");
                    console.error("Metadata fetch failed");
                    return;
                }

                if (res.status === 204) {
                    showToast("No commands found. Please check 'AxiApi' Configuration");
                    return;
                }

                const data = await res.json();
                commandsFromDb = data.commands;
                localStorage.setItem("axi_commands_raw_v1", JSON.stringify(commandsFromDb));
            }

            if (commandsFromDb) {
                const message = isForced ? "Refreshed Successfully!" : "Commands Loaded Successfully!.";
                const accessPermissions = getAccessPermissions();
                // Clone to avoid mutating the cached object stored in memory
                commands = buildCommandsByAccessPermissions(JSON.parse(JSON.stringify(commandsFromDb)), accessPermissions);
                // Append Help command to initial suggestions list
                commands["Help"] = {
                    cmdToken: 11,
                    command: "",
                    commandGroup: "Help",
                    prompts: []
                };
                // Append Version command to initial suggestions list
                commands["Version"] = {
                    cmdToken: 12,
                    command: "",
                    commandGroup: "Version",
                    prompts: []
                };

                console.log(JSON.stringify(commands));
                
                // Load target entities (tstructs, iview, ads and pages) from metadata on startup asynchronously
                const metadataParams = getStructParam();
                loadList("axi_structmetalist", metadataParams);
                // if (isForced) {
                    showToast(message, 3000, true);
                // }
            }
        } catch (err) {
            console.error("Critical: Could not load commands", err);
        } finally {
            isCommandsLoading = false;
            input.disabled = false;
            input.placeholder = "Axpert AI";
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

    // async function loadApiMetadataConfig() {
    //     let configUrl = "";
    //     try {
    //         configUrl = `${getAppBaseUrl()}/AxpertPlugins/Axi/axiConfig.json`;
    //         const res = await fetch(configUrl, { cache: "no-store" });
    //         if (!res.ok) {
    //             throw new Error(`Failed to load ${configUrl}. Status: ${res.status}`);
    //         }

    //         const settings = await res.json();
    //         const configuredApiMetadata = typeof settings?.API_METADATA === "string" ? settings.API_METADATA.trim() : "";
    //         const configuredAxiFavoritesUrl = typeof settings?.AXI_FAVORITES_URL === "string" ? settings.AXI_FAVORITES_URL.trim() : "";

    //         if (!configuredApiMetadata) {
    //             throw new Error(`API_METADATA is missing or empty in ${configUrl}`);
    //         }

    //         if (!configuredAxiFavoritesUrl) {
    //             throw new Error(`API_METADATA is missing or empty in ${configUrl}`);
    //         }

    //         apiMetadataUrl = configuredApiMetadata;
    //         axiFavoritesUrl = configuredAxiFavoritesUrl;
    //         apiMetadataConfigError = "";
    //     } catch (error) {
    //         apiMetadataUrl = "";
    //         apiMetadataConfigError = (error && error.message) ? error.message : String(error);
    //         showToast("Failed to load API metadata configuration.");
    //         console.error(`Failed to resolve API_METADATA from ${configUrl || "app base URL"}`, error);
    //     }
    // }

    // function ensureApiMetadataConfigLoaded() {
    //     if (!apiMetadataConfigPromise) {
    //         apiMetadataConfigPromise = loadApiMetadataConfig();
    //     }

    //     return apiMetadataConfigPromise;
    // }

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


                //We need to optimise this logic as it works only for axi_getstructsdata now.
                let fieldSource = prevSources.find(src =>
                    src.toLowerCase().includes('axi_getstructsdata') || src.includes('keyvalue') || src.includes('fieldname')
                );

                //const fieldSource = prevSources.find(src =>
                //    src.includes('keyvalue') || src.includes('fieldname')
                //);
                if (fieldSource?.toLowerCase() === "axi_getstructsdata" && tokens.length === 5 && commandConfig?.commandGroup?.toLowerCase() === "edit") {
                    fieldSource = "";
                }


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
                // const actualPrevValue = tryResolveToken(prevTokenIndex, prevValue, commandConfig, false) || prevValue;
                const { value: actualPrevValue, type } = tryResolveToken(prevTokenIndex, prevValue, commandConfig, false) || prevValue;
                let valueIndex = allowedValues.indexOf(prevValue.toLowerCase());

                if (valueIndex === -1 && commandConfig.commandGroup?.toLowerCase() === 'view') {
                    if (actualPrevValue.toLowerCase() === "source") {
                        return null;

                    }
                    const detectedType = getType(commandConfig?.prompts?.[0]?.promptSource.toLowerCase(), { value: actualPrevValue, type: type }, prevPrompt.promptValues, tokens, commandConfig);

                    if (detectedType) {
                        valueIndex = allowedValues.indexOf(detectedType.toLowerCase());
                    }


                }

                if (valueIndex === -1 && (commandConfig.commandGroup?.toLowerCase() === "sdk" || commandConfig.commandGroup?.toLowerCase() === "configure")) {

                    return { config: prompt, realSource: null, error: "Not a Valid command" };
                }

                if (valueIndex !== -1) {
                    const sources = activeSource.split(',');
                    activeSource = sources[valueIndex] ? sources[valueIndex].trim() : "";
                }

                console.log("Value Index: " + valueIndex);
                console.log("Active Source: " + activeSource);
            }
        }

        return { config: prompt, realSource: activeSource, error: "" };
    }





    async function loadList(sourceName, paramValue = "") {
        if (sourceName === "axi_dummy") {
            console.error("Axi Dummy source should not trigger loadList");
            return;
        }
        const key = paramValue ? `${sourceName}_${paramValue}`.toLowerCase() : sourceName.toLowerCase();
        if (activeFetches.has(key)) return;
        activeFetches.add(key);

        console.log(`Fetching list: ${sourceName} params: ${paramValue}`);

        try {
            const data = await getList(sourceName, paramValue);
            axDatasourceObj[key] = data;
            console.log(JSON.stringify(axDatasourceObj));
            if (document.activeElement === input) {
                handleInput();
            }

        } catch (error) {
            console.error("loadlist failed", error);
        } finally {
            activeFetches.delete(key);


        }

    }

    //function openPopOption(targetURL) {
    //    console.log("PopOption is clicked");


    //    if (targetURL) {
    //        let popUpContainerUrl = `../AxpertPlugins/Axi/HTMLPages/PopUpContainer.html`;

    //        //if (targetURL && targetURL.toLowerCase().includes("/aspx")) {
    //        //    console.log("Before removing : " + targetURL);
    //        //    targetURL = targetURL.replace("/aspx", "");
    //        //    console.log("After removing : " + targetURL);
    //        //}

    //        //if (targetURL && targetURL.toLowerCase().includes("/aspx/")) {
    //        //    console.log("Before removing : " + targetURL)
    //        //    targetURL = targetURL.split("/aspx/")[1];
    //        //    console.log("After removing : " + targetURL);
    //        //}
    //        if (targetURL && targetURL.toLowerCase().includes("../")) {
    //            console.log("Before removing : " + targetURL)
    //            targetURL = targetURL.replace("../", "");
    //            //targetURL = targetURL.split("/")[1];
    //            console.log("After removing : " + targetURL);
    //        }


    //        //let finalUrl = `${popUpContainerUrl}?contenturl=${encodeURIComponent(btoa(targetURL))}`;
    //        let finalUrl = `${popUpContainerUrl}?contenturl=${targetURL}`;


    //        popUpOption = false;
    //        console.log("PopUp Option is set to :" + popUpOption);


    //        let hiddenVar = document.getElementById("hiddenLoader");
    //        console.log(hiddenVar);
    //        hiddenVar.src = finalUrl;
    //        //hiddenVar.style.display = "block";
    //        hiddenVar.style.display = "flex";
    //        //let popup = document.getElementById("popupContainer");
    //        //popup.style.display = "block";
    //        // window.open(finalUrl);
    //    }
    //    else {
    //        popUpOption = false;
    //        console.log("Error in Popup: TargetURL is empty");
    //        showToast("Something went wrong. Please try again later");
    //    }
    //}

    //******************** Configure Newer introduced commands(31032026)********************//
    //********************Starts here*******************************************************//
    function handleConfigureUsers({ tokens, commandConfig }) {

        let transId = "axusers";
        //let fieldname = "servername";

        const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);

        redirectToIView(transId, cleanCommandToken(tokens[1]));
        //redirectToTstruct(transId, "", true, fieldname, rawParamName);
    }
    function handleConfigureUser({ tokens, commandConfig }) {

        let transId = "axusr";
        //let fieldname = "servername";

        //const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToTstruct(transId, cleanCommandToken(tokens[1]));
    }
    function handleConfigureRoles({ tokens, commandConfig }) {

        let transId = "ad___url";

        //let fieldname = "servername";

        //const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToIView(transId, cleanCommandToken(tokens[1]));
        //redirectToTstruct(transId, "", true, fieldname, rawParamName);
    }
    function handleConfigureRole({ tokens, commandConfig }) {

        let transId = "ad_ur";
        //let fieldname = "servername";

        //const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToTstruct(transId, cleanCommandToken(tokens[1]));
    }
    function handleConfigurePublishAxpertApi({ tokens, commandConfig }) {

        let transId = "ad_pa";
        let fieldname = "publickey";
        let actualParamvalue;
        let rawParamName;

        setEditSessionState(transId);
        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // actualParamvalue = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            actualParamvalue = value;
            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, actualParamvalue);
        }
        else
            redirectToTstruct(transId, cleanCommandToken(tokens[1]));
    }
    function handleConfigureApiList({ tokens, commandConfig }) {

        let transId = "ad__papi";
        //let fieldname = "servername";

        //const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToIView(transId, "");
    }
    function handleConfigurePublishListing({ tokens, commandConfig }) {

        let transId = "ad_pbcs";

        if (tokens.length > 2) {
            let transId = "axpub";
            let fieldname = "servername";

            const rawParamName = cleanCommandToken(tokens[2]);


            setEditSessionState(transId);
            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, rawParamName);


        } else {
            setEditSessionState(transId);
            redirectToIView(transId, cleanCommandToken(tokens[1]));
        }



    }
    function handleConfigureCards({ tokens, commandConfig }) {

        //LISTVIEW
        //let transId = "axpcards";
        //setEditSessionState(transId);
        //redirectToIView(transId, "");


        //NEWMODE
        ///aspx/tstruct.aspx?transid=a__cd&dummyload=false%E2%99%A0
        //editmode
        ///aspx/tstruct.aspx?act=load&transid=a__cd&recordid=1419550000009&recpos=8&curpage=2&pagetype=middle&openeriv=axpcards&isiv=true&isduptab=false&reqProc_logtime=&dummyload=false%e2%99%a0
        let transId = "a__cd";
        let fieldname = "cardname";
        let paramValue;

        const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        if (rawParamName) {
            // paramValue = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            paramValue = value;
            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, paramValue);
        }
        else {
            redirectToTstruct(transId, cleanCommandToken(tokens[1]));
        }

    }
    function handleConfigureForms({ tokens, commandConfig }) {

        let transId = "ad___rel";
        //let fieldname = "servername";

        //const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToIView(transId, "");
    }
    function handleConfigureResponsibility({ tokens, commandConfig }) {

        //new - aspx/AddEditResponsibility.aspx?action=add
        //edit - aspx/AddEditResponsibility.aspx?status=true&action=edit&name=demorole

        //old(31-03)
        //let transId = "axrol";
        ////let fieldname = "servername";

        ////const rawParamName = cleanCommandToken(tokens[2]);


        //setEditSessionState(transId);
        //redirectToTstruct(transId);

        //new(31-03)
        let transId = "axrol";

        let targetUrl = `../aspx/AddEditResponsibility.aspx?`;


        let fieldname = "name";

        if (tokens.length > 2) {
            const rawParamName = cleanCommandToken(tokens[2]);
            // const actualName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value: actualName, type } = tryResolveToken(2, rawParamName, commandConfig, false);


            targetUrl += `status=true`;
            targetUrl += `&action=edit`;
            targetUrl += `&${fieldname}=${actualName}`;
        }
        else {
            targetUrl += `action=add`;
        }

        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(cleanCommandToken(tokens[1]))}`;
            targetUrl += "&AxIsPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            window.LoadIframe(targetUrl);
        }
    }
    function handleConfigureResponsibilities({ tokens, commandConfig }) {

        let transId = "response";
        //let fieldname = "servername";

        // const rawParamName = cleanCommandToken(tokens[1]);


        setEditSessionState(transId);
        redirectToIView(transId, cleanCommandToken(tokens[1]));

        //   console.log("Redirecting to Iview: " + transId + "..............");
        // let targetUrl = `../aspx/iview.aspx?ivname=${transId}`;

        // setCommandRoutes(input.value.trim(), targetUrl);


        //     window.LoadIframe(targetUrl);

    }
    function handleConfigureActor({ tokens, commandConfig }) {

        //newmode
        //javascript: callOpenAction('opentstruct', 'ad_am');
        //editmode
        //aspx/tstruct.aspx?act=load&transid=ad_at&recordid=1432770000001&recpos=2&curpage=1&pagetype=middle&openeriv=ad__act&isiv=true&isduptab=false&reqProc_logtime=&dummyload=false%e2%
        let transId = "ad_am";
        let paramvalue;
        let rawParamname;
        let fieldname = "actorname"
        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawParamname = cleanCommandToken(tokens[2]);

            // paramvalue = tryResolveToken(2, rawParamname, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamname, commandConfig, false);
            paramvalue = value;

            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, paramvalue);

        }
        else {
            redirectToTstruct(transId, cleanCommandToken(tokens[1]));
        }
    }
    function handleConfigureActorListing({ tokens, commandConfig }) {

        //listview
        let transId = "ad__act";
        let fieldname = "servername";

        const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToIView(transId, cleanCommandToken(tokens[1]));

    }
    function handleConfigureDimensions({ tokens, commandConfig }) {
        ////listview
        //let transId = "ad___upg";
        //setEditSessionState(transId);
        //redirectToIView(transId, "");

        //newmode
        //javascript:callOpenAction('opentstruct','a__ag');(dimension and value)
        //editmode
        //aspx/tstruct.aspx?act=load&transid=a_pgm&recordid=1156990000009&recpos=5&curpage=2&pagetype=middle&openeriv=ad___upg&isiv=true&isduptab=false&reqProc_logtime=&dummyload=false%

        let transId = "a_pgm";
        let rawParamName;
        let paramValue;
        //let fieldname = "grpnamedb";
        let fieldname = "grpname";


        setEditSessionState(transId);
        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);

            // paramValue = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            paramValue = value;

            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, paramValue);
        }
        else {
            redirectToTstruct(transId, cleanCommandToken(tokens[1]));
        }
    }
    function handleConfigureUserGroup({ tokens, commandConfig }) {

        //listview
        //let transId = "ad___ugp";
        //setEditSessionState(transId);
        //redirectToIView(transId, "");



        //newmode
        //javascript: callOpenAction('opentstruct', 'a__ug');
        //editmode
        ///aspx/tstruct.aspx?act=load&transid=a__ug&recordid=1436220000012&recpos=1&curpage=1&pagetype=first&openeriv=ad___ugp
        //& isiv=true & isduptab=false & AxPop=true & reqProc_logtime=Request % 20

        let transId = "a__ug";
        let paramValue;
        let rawValue;

        let fieldname = "users_group_name";


        setEditSessionState(transId);
        if (tokens.length > 2) {
            rawValue = cleanCommandToken(tokens[2]);


            if (rawValue) {
                // paramValue = tryResolveToken(2, rawValue, commandConfig, false);
                const { value, type } = tryResolveToken(2, rawValue, commandConfig, false);
                paramValue = value;
            }

            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, paramValue);
        }
        else {
            redirectToTstruct(transId, cleanCommandToken(tokens[1]));
        }
    }
    function handleConfigureUserActivation({ tokens, commandConfig }) {

        let transId = "axurg";
        let paramValue;
        let rawParamName;
        let fieldname = "pusername";


        setEditSessionState(transId);
        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);

            // paramValue = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            paramValue = value;

            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, paramValue);
        }
        else {
            redirectToTstruct(transId, cleanCommandToken(tokens[1]));
        }
    }
    function handleConfigureUserPermissionListing({ tokens, commandConfig }) {

        //ivtoivload.aspx ? ivname = ad___upm && pusername=admin & AxOpenAct=true & isDupTab=false
        let transId = "ad___upm";
        //let fieldname = "pusername";

        const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        // redirectToIView(transId,fieldname);
        //let targetUrl = `../aspx/iview.aspx?ivname=${transId}`;
        let targetUrl = `../aspx/ivtoivload.aspx?ivname=${transId}`;


        if (rawParamName) {
            targetUrl += `&pusername=${encodeURIComponent(rawParamName)}`;
        }


        targetUrl += `&AxOpenAct=true`;
        targetUrl += `&isDupTab=false`;



        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(cleanCommandToken(tokens[1]))}`;
            targetUrl += "&AxIsPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            window.LoadIframe(targetUrl);
        }
        //setCommandRoutes(input.value.trim(), targetUrl);
        //window.LoadIframe(targetUrl);
    }
    function handleConfigureRolePermissionListing({ tokens, commandConfig }) {

        //ivtoivload.aspx ? ivname = ad___ups && prole=managerrole && AxOpenAct=true & isDupTab=false
        let transId = "ad___ups";
        //let fieldname = "prole";

        const rawParamName = cleanCommandToken(tokens[2]);

        let targetUrl = `../aspx/ivtoivload.aspx?ivname=${transId}`;


        if (rawParamName) {
            targetUrl += `&prole=${encodeURIComponent(rawParamName)}`;
        }


        targetUrl += `&AxOpenAct=true`;
        targetUrl += `&isDupTab=false`;



        setEditSessionState(transId);
        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(cleanCommandToken(tokens[1]))}`;
            targetUrl += "&AxIsPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            window.LoadIframe(targetUrl);
        }
    }
    function handleConfigureUserPermission({ tokens, commandConfig }) {

        //tstruct.aspx ? act = open & transid=a__up & axusername=aarav & fromsource=U & openerIV=axusers & isIV=true & isDupTab=false & dummyload=false % E2 % 99 % A0
        let transId = "a__up";
        let fieldname = "axusername";
        let rawParamName;
        let actualParamName;



        let targetUrl = `../aspx/tstruct.aspx?act=open`;

        targetUrl += `&transid=${transId}`;



        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // actualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            paramValue = value;
            targetUrl += `&${fieldname}=${encodeURIComponent(rawParamName)}`;
        }

        targetUrl += `&fromsource=U`;
        targetUrl += `&openerIV=${transId}`;
        targetUrl += `&isIV=true`;
        targetUrl += `&isDupTab=false`;
        targetUrl += `&dummyload=false?`;

        setEditSessionState(transId);

        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(cleanCommandToken(tokens[1]))}`;
            targetUrl += "&AxPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            top.window.LoadIframe(targetUrl);
        }
        //setCommandRoutes(input.value.trim(), targetUrl);

        //top.window.LoadIframe(targetUrl);
    }

    function handleConfigureSmartViewAttributes({ tokens, commandConfig }) {

        
        let transId = "a__sl";
        let fieldname = "adsname";
        let rawParamName;
        let actualParamName;



        let targetUrl = `../aspx/tstruct.aspx`;

        targetUrl += `?transid=${transId}`;



        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // actualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            paramValue = value;
            targetUrl += `&${fieldname}=${encodeURIComponent(paramValue)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false";
        }

        // targetUrl += `&fromsource=U`;
        // targetUrl += `&openerIV=${transId}`;
        
        // targetUrl += `&isDupTab=false`;
        // targetUrl += `&dummyload=false?`;

        setEditSessionState(transId);

        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(cleanCommandToken(tokens[1]))}`;
            targetUrl += "&AxPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            top.window.LoadIframe(targetUrl);
        }
        //setCommandRoutes(input.value.trim(), targetUrl);

        //top.window.LoadIframe(targetUrl);
    }
    //function handleRolePermission({ tokens, commandConfig }) {

    //    //tstruct.aspx ? act = load & transid=ad_ur & recordid=1504010000001 & recpos=6 & curpage=1 & pagetype=middle & openeriv=ad___url & isiv=true & isduptab=false & reqProc_logtime=& dummyload=false % e2 % 99 % a0
    //    let transId = "axurg";
    //    //let fieldname = "recordid";

    //    const rawParamName = cleanCommandToken(tokens[2]);
    //    let recordId = tryResolveToken(2, rawParamName, commandConfig, false);


    //    setEditSessionState(transId);
    //    //redirectToTstruct(transId);

    //    let targetUrl = `../aspx/tstruct.aspx?act=load`;

    //    targetUrl += `&transid=${transId}`;

    //    if (recordId) {
    //        targetUrl += `&recordid=${encodeURIComponent(recordId)}`;
    //    }

    //    // static params
    //    targetUrl += `&recpos=6`;
    //    targetUrl += `&curpage=1`;
    //    targetUrl += `&pagetype=middle`;
    //    targetUrl += `&openeriv=${transId}`; // or exact: ad___url if needed
    //    targetUrl += `&isiv=true`;
    //    targetUrl += `&isduptab=false`;
    //    targetUrl += `&reqProc_logtime=`;
    //    targetUrl += `&dummyload=false?`;

    //    // load iframe
    //    top.window.LoadIframe(targetUrl);
    //}

    //*********************************Configure Newer introduced commands ends here******************************************//


    //******************** Open Newer introduced commands(31032026)********************//
    //********************Starts here**************************************************//
    function handleOpenJob({ tokens, commandConfig }) {

        let transId = "job_s";
        //let fieldname = "jname";
        let fieldname = "jobid";
        let actualParamvalue;

        setEditSessionState(transId);
        if (tokens.length > 2) {
            let rawParamName = cleanCommandToken(tokens[2]);
            // actualParamvalue = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            actualParamvalue = value;
            redirectToTstruct(transId, "", true, fieldname, actualParamvalue);
        }
        else {
            redirectToTstruct(transId);
        }





    }
    function handleOpenLanguage({ tokens, commandConfig }) {
        //tstruct.aspx?act=open&transid=ad_lg&openerIV=axlangs&isIV=true&isDupTab=false&dummyload=false%E2%99%A0
        let transId = "ad_lg";
        let fieldname = "language";
        let rawParamName;
        let ActualParamName;


        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // ActualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            ActualParamName = value;

            redirectToTstruct(transId, "", true, fieldname, ActualParamName);
        }
        else {
            redirectToTstruct(transId);
        }

    }
    function handleOpenPublish({ tokens, commandConfig }) {
        //listview
        //openDeveloperStudio(& quot; ipublist & quot;);
        //iview.aspx ? ivname = axpubls

        let transId = "axpubls";


        setEditSessionState(transId);
        redirectToIView(transId, "");




        ////newmode
        ////tstruct.aspx?act=open&transid=axpub&openerIV=publist&isIV=true&isDupTab=false&dummyload=false?
        //let transId = "axpub";
        ////let fieldname = "jname";

        ////let rawParamName = cleanCommandToken(tokens[2]);


        //setEditSessionState(transId);
        //redirectToTstruct(transId, "", "", "", "","publist");

    }
    function handleOpenCustomDataType({ tokens, commandConfig }) {
        //tstruct.aspx?act=open&transid=ctype&openerIV=cdlist&isIV=true&isDupTab=false&dummyload=false%E2%99%A0
        let transId = "ctype";
        let fieldname = "typename";
        let rawParamName;
        let ActualParamName;


        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // ActualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            ActualParamName = value;

            redirectToTstruct(transId, "", true, fieldname, ActualParamName);
        }
        else {
            redirectToTstruct(transId);
        }

    }
    function handleOpenEmailDef({ tokens, commandConfig }) {
        //tstruct.aspx?act=open&transid=axeml&openerIV=emaildef&isIV=true&isDupTab=false&dummyload=false%E2%99%A0 
        let transId = "axeml";
        let fieldname = "emaildefname";
        let rawParamName;
        let ActualParamName;


        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // ActualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            ActualParamName = value;

            redirectToTstruct(transId, "", true, fieldname, ActualParamName);
        }
        else {
            redirectToTstruct(transId);
        }

    }
    function handleOpenTableFieldDescriptor({ tokens, commandConfig }) {
        //tstruct.aspx?act=open&transid=a__td&openerIV=ad___tbd&isIV=true&isDupTab=false&dummyload=false%E2%99%A0
        let transId = "a__td";
        let fieldname = "dname";
        let rawParamName;
        let ActualParamName;


        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // ActualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            ActualParamName = value;

            redirectToTstruct(transId, "", true, fieldname, ActualParamName);
        }
        else {
            redirectToTstruct(transId);
        }

    }
    function handleOpenMemDBConsole({ tokens, commandConfig }) {
        //iview.aspx?ivname=inmemdb
        let transId = "inmemdb";
        //let fieldname = "jname";

        //let rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        //redirectToTstruct(transId, "", "", "", "","ad___tbd");
        redirectToIView(transId);
    }
    function handleOpenCustomPlugin({ tokens, commandConfig }) {
        //aspx/PluginCustomCode.aspx

        const targetUrl = "../aspx/PluginCustomCode.aspx";
        setCommandRoutes(input.value.trim(), targetUrl);
        window.LoadIframe(targetUrl);

    }
    function handleQueueListing({ tokens, commandConfig }) {
        //iview.aspx?ivname=ad__qls
        let transId = "ad__qls";

        setEditSessionState(transId);
        redirectToIView(transId);
    }

    //function handleOpenOutBoundQueueM({ tokens, commandConfig }) {
    //      //iview.aspx?ivname=inmemdb
    //    let transId = "inmemdb";
    //    //let fieldname = "jname";

    //    //let rawParamName = cleanCommandToken(tokens[2]);


    //    setEditSessionState(transId);
    //    //redirectToTstruct(transId, "", "", "", "","ad___tbd");
    //    redirectToIView(transId);
    //}
    function handleOpenOutBoundQueue({ tokens, commandConfig }) {
        //aspx/tstruct.aspx?act=open&transid=a__qm&AxPop=true&openerIV=ad__qls&isIV=true&isDupTab=false
        let transId = "a__qm";
        let fieldname = "axqueuename";
        let rawParamName;
        let ActualParamName;


        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // ActualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            ActualParamName = value;

            redirectToTstruct(transId, "", true, fieldname, ActualParamName);
        }
        else {
            redirectToTstruct(transId);
        }
    }
    function handleOpenInboundBoundQueue({ tokens, commandConfig }) {
        //aspx/tstruct.aspx?act=open&transid=a__iq&AxPop=true&openerIV=ad__qls&isIV=true&isDupTab=false
        let transId = "a__iq";
        let fieldname = "axqueuename";
        let rawParamName;
        let ActualParamName;


        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawParamName = cleanCommandToken(tokens[2]);
            // ActualParamName = tryResolveToken(2, rawParamName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawParamName, commandConfig, false);
            ActualParamName = value;

            redirectToTstruct(transId, "", true, fieldname, ActualParamName);
        }
        else {
            redirectToTstruct(transId);
        }
    }


    //*********************************Open Newer introduced commands ends here******************************************//


    function openPopOption(targetURL) {
        console.log("PopOption is clicked");

        if (targetURL) {

            // ? FIX: Ensure htmlPages.aspx loads from /aspx/
            if (targetURL.toLowerCase().includes("htmlpages.aspx") && !targetURL.includes("../")) {
                targetURL = "../aspx/" + targetURL;
            }

            let popUpContainerUrl = `../AxpertPlugins/Axi_Beta/HTMLPages/PopupContainer.html`
            //let popUpContainerUrl = `../CustomPages/Axi/HTMLPages/PopUpContainer.html`;

            if (targetURL && targetURL.toLowerCase().includes("../")) {
                console.log("Before removing : " + targetURL);
                targetURL = targetURL.replace("../", "");
                //targetURL = targetURL.split("/")[1];
                console.log("After removing : " + targetURL);
            }

            let hiddenVar = document.getElementById("hiddenLoader");

            // Check if the PopupManager already exists in the iframe
            if (hiddenVar.contentWindow && hiddenVar.contentWindow.PopupManager) {
                // ADD AS NEW TAB: Call the internal manager
                hiddenVar.contentWindow.PopupManager.openForm("Loading...", targetURL);
            } else {
                // INITIAL LOAD: Set src for the first time
                let finalUrl = `${popUpContainerUrl}?contenturl=${targetURL}`;
                hiddenVar.src = finalUrl;
                hiddenVar.style.display = "flex";
            }
            popUpOption = false;
            console.log("Final Url: " + targetURL);
            console.log("PopUp Option is set to :" + popUpOption);

        } else {
            popUpOption = false;
            console.log("Error in Popup: TargetURL is empty");
            showToast("Something went wrong. Please try again later");
        }
    }

    function redirectToSmartView({ adsName, filters }) {


        // let targetUrl = "../CustomPages/Smartview_table_1769088257557.html";
        // let targetUrl = `${getAppBaseUrl()}/CustomPages/Smartview_table_1769088257557.html`;
        // let targetUrl = `${getAppBaseUrl()}/CustomPages/Smartview_table.html`;
        // let targetUrl = `${getAppBaseUrl()}/plugins/Axi/HTMLPages/Smartview_table.html`;
        let targetUrl = `../AxpertPlugins/Axi_Beta/HTMLPages/Smartview.html`;

        // let targetUrl = "../axidev/HTMLPages/Smartview_table_1769088257557.html";

        targetUrl += `?ads=${encodeURIComponent(adsName)}`;
        targetUrl += "&load=1769601086182";

        let encodedFilterQuery;

        let isGroupable = false;
        if (filters && filters.length === 1 && (!filters[0].value || filters[0].value === "")) {
            const datatypes = (filters[0].datatype || "").split(",");
            isGroupable = datatypes.length > 0 && datatypes.every(dt => dt === "c" || dt === "d" || dt === "t" || dt === "NA" || !dt);
        }

        if (isGroupable) {
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
        setCommandRoutes(input.value.trim(), targetUrl);
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

        if (popUpOption) {
            targetUrl += "&AxIsPop=true";

            targetUrl += `&tname=${encodeURIComponent(adsName)}`;
            openPopOption(targetUrl)
        }
        else {
            top.window.LoadIframe(targetUrl);
        }


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
        setCommandRoutes(input.value.trim(), targetUrl);


        top.window.LoadIframe(targetUrl);

    }



    function redirectToTstruct(transId, tstructCaption = "", isEdit = false, fieldName = "", fieldValue = "") {
        console.log(`Redirecting to Tstruct: ${transId}, Edit: ${isEdit}, Field: ${fieldName}, Val: ${fieldValue}`);



        if (!transId) {
            alert("There is no Tstruct name provided!");
            return;
        }

        let targetUrl;

        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (isEdit) {
            if (fieldName && fieldValue) {
                targetUrl += `&${fieldName}=${encodeURIComponent(fieldValue)}`;
            }
            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=${transId}`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;

            targetUrl += `&dummyload=false?`;

        }
        else {
            if (fieldName && fieldValue) {
                targetUrl += `&${fieldName}=${encodeURIComponent(fieldValue)}`;
            }
            targetUrl += `&hltype=open`;

            targetUrl += `&createaxiflag=true`;
            targetUrl += `&dummyload=false?`;
        }



        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(tstructCaption)}`;
            targetUrl += "&AxPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            top.window.LoadIframe(targetUrl);
        }
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


        setCommandRoutes(input.value.trim(), targetUrl);
        top.window.LoadIframe(targetUrl);
    }

    // function redirectToIView(iViewName, iViewCaption = "") {
    //     console.log("Redirecting to Iview: " + iViewName + "..............");


    //     if (popUpOption) {
    //      let targetUrl = `../aspx/ivtoivload.aspx?ivname=${iViewName}`;
    //     // setCommandRoutes(input.value.trim(), targetUrl);


    //         targetUrl += `&tname=${encodeURIComponent(iViewCaption)}`;
    //         targetUrl += "&AxIsPop=true";
    //         openPopOption(targetUrl)
    //     }
    //     else {
    //     let targetUrl = `../aspx/iview.aspx?ivname=${iViewName}`;
    //     setCommandRoutes(input.value.trim(), targetUrl);
    //         window.LoadIframe(targetUrl);
    //     }


    // }

    function redirectToIView(iViewName, iViewCaption = "") {
        console.log("Redirecting to Iview: " + iViewName + "..............");


        if (popUpOption) {
         let targetUrl = `../aspx/ivtoivload.aspx?ivname=${iViewName}`;
        // setCommandRoutes(input.value.trim(), targetUrl);
            targetUrl += `&tname=${encodeURIComponent(iViewCaption)}`;
            targetUrl += "&AxIsPop=true&isDupTab=true-";
            openPopOption(targetUrl)
        }
        else {
        let targetUrl = `../aspx/iview.aspx?ivname=${iViewName}`;
        setCommandRoutes(input.value.trim(), targetUrl);
            window.LoadIframe(targetUrl);
        }


    }


    function redirectToProcessFlow(caption, tstructCaption) {
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



        setEditSessionState("ad_pm");
        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(tstructCaption)}`;
            targetUrl += "&AxIsPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            top.window.LoadIframe(targetUrl);
        }
    }



    /* ===============================
       2. INPUT HANDLER
    =============================== */
    function getInitialCommandsList() {
        if (!commands) return [];
        const structType = getStructType();
        const isRunnable = structType && structType !== "o" && !isPreviewModalOpen() && !isRunDisabledForPage() && !isViewDesignerModalOpen();
        return Object.keys(commands).filter(key => {
            const lowerKey = key.toLowerCase();
            if (lowerKey === "create" || lowerKey === "view" || lowerKey === "edit" || lowerKey === "source") {
                return false;
            }
            if (lowerKey === "run") {
                return isRunnable;
            }
            return true;
        });
    }

    function handleInput() {
        if (favouritesCard) {
            favouritesCard.style.display = "none";
        }
        const suggCard = list.closest(".card");
        if (suggCard) {
            suggCard.style.display = "flex";
        }

        if (input.value && input.value.includes(" ,")) {
            const cursorPos = input.selectionStart;
            input.value = input.value.replace(/ ,/g, ",");
            if (cursorPos !== null) {
                const newPos = Math.max(0, cursorPos - 1);
                input.setSelectionRange(newPos, newPos);
            }
        }
        const initialTokens = getTokens(input.value.trim());
        const isInitialCommandStage = initialTokens.length === 0 || (initialTokens.length === 1 && !input.value.endsWith(" "));

        if (isCommandsLoading && !isInitialCommandStage) {
            items = ["Loading Commands...."];
            hintDiv.textContent = "Please wait...";
            render();
            return;
        }
        const text = input.value;

        if (axiClearBtn) {
            if (text.length > 0) {
                axiClearBtn.style.display = "flex";

            } else {
                axiClearBtn.style.display = "none";
            }
        }
        if (!commands) {
            if (isInitialCommandStage) {
                items = getInitialSuggestions();
                hintDiv.textContent = "";
                render();
                return;
            }
            return;
        }

        if (!text.trim()) {

            items = getInitialSuggestions();
            hintDiv.textContent = "";
            render();
            return;
        }

        ///set command - numeric handling.
        if (SET_COMMAND_STATE.currentFieldType === 'n') {

            let tokens = getTokens(text);

            const grpKey = tokens[0];

            let lastIndex = tokens.length - 1;
            let lastToken = tokens[lastIndex];


            if (lastToken.toLowerCase() == SET_COMMAND_STATE.currentField) {
                return;
            }

            //const numericRegex = /^-?\d*$/;
            let numericRegex;
            if (grpKey.toLowerCase() === "view") {
                numericRegex = /^-?(?!.*\.\.)(?!.*'')(?!.*,,)[\d.,'-<>!=]*$/;
                //numericRegex = /^(>=|<=|!=|>|<|=)?-?(?:\d+(?:,\d+)*(?:\.\d+)?|\.\d+)$/;
            }
            else
                numericRegex = /^-?(?!.*\.\.)(?!.*'')(?!.*,,)[\d.,']*$/;


            //const cursorPos = input.selectionStart
            //if (input.value[cursorPos - 1] === " ") {
            //    return;
            //}


            // If user pressed space ? don't validate previous token again
            //let endsWithSpace = text.endsWith(" ");

            //if (endsWithSpace) {
            //    return; // just wait for next input
            //}



            if (!numericRegex.test(lastToken)) {

                console.error("Type only numeric value");

                if (grpKey.toLowerCase() === "view")
                    showToast("Please enter a valid number. You may use comparison operators (>, <, >=, <=, !=, =).");
                else
                    showToast("Please enter a valid numeric value.");



                tokens[lastIndex] = "";

                input.value = tokens.join(" ");
                return;
            }
        }


        // Clear stale resolutions when input changes
        const currentTokens = getTokens(text, false);
        currentTokens.forEach((token, idx) => {
            const cleanToken = token.replace(/"/g, "");
            const lastToken = lastTypedTokens[idx] ? lastTypedTokens[idx].replace(/"/g, "") : null;

            if (lastToken && cleanToken !== lastToken && resolvedParams[idx]) {
                console.log(`Token changed at position ${idx}: "${lastToken}" ? "${cleanToken}"`);
                delete resolvedParams[idx];
                delete resolvedParamType[idx];
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
                    delete resolvedParamType[i];
                    console.log(`Cleared deleted token resolution at index ${i}`);
                }
            }
        }

        lastTypedTokens = [...currentTokens];
        items = suggestLocal(text);

        const tokens = getTokens(text);
        if (tokens.length >= 2) {
            const first = cleanString(tokens[0]).toLowerCase();
            const second = cleanString(tokens[1]).toLowerCase();
            if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                const temp = tokens[0];
                tokens[0] = tokens[1];
                tokens[1] = temp;
            }
        }
        const grpKey = tokens[0]?.toLowerCase();

        if (grpKey === "edit" && (text.toLowerCase().includes(" with") || tokens.some(t => cleanString(t).toLowerCase() === "with"))) {
            const entityObj = getTargetEntityObj(tokens[1], "edit");
            if (entityObj && (entityObj.keyfieldforedit === "F" || entityObj.keyfieldforedit === false)) {
                showToast("Key field is not configured for this form.", 3000, false);
                items = [];
                render();
                hide();
                return;
            }
        }

        if (
            grpKey === "view" &&
            tokens.length > 1 &&
            !text.endsWith(" ")
        ) {
            items = items.filter(item => {
                const itemText = (
                    typeof item === "string"
                        ? item
                        : (item.displaydata || item.name || "")
                ).toLowerCase();

                return itemText !== "source";
            });
            filteredObjects = filteredObjects.filter(item => {
                const itemText = (
                    typeof item === "string"
                        ? item
                        : (item.displaydata || item.name || "")
                ).toLowerCase();

                return itemText !== "source";
            });
        }


        render();

        /**
         * Auto-Apply Logic
         */
        if (!isDeleting && items.length > 0) {
            const currentTokens = getTokens(text);
            const lastTokenRaw = currentTokens[currentTokens.length - 1] || "";
            const lastTokenClean = cleanString(lastTokenRaw);

            if (!text.endsWith(" ") && lastTokenClean.length > 0) {
                const validItems = items.filter(i => !isSystemMessage(i) && !i.isExecutable);

                let indexToApply = -1;

                if (validItems.length === 1 && lastTokenClean.length >= 2) {
                    indexToApply = items.findIndex(item => {
                        if (isSystemMessage(item) || item.isExecutable) return false;

                        const itemStr = typeof item === "string" ? item : (item.displaydata || item.name);

                        return itemStr.toLowerCase() === lastTokenClean.toLowerCase();
                    });
                } else if (lastTokenClean.toLowerCase() === "help") {
                    input.value = "Help ";
                    handleInput();
                    return;
                }

                // Second-token verb auto-resolve: when the user fully types an
                // action verb (e.g., "edit") after a target entity, resolve to
                // proper case and advance to next-level suggestions.
                if (indexToApply === -1) {
                    const rawCheckTokens = getTokens(text, false);
                    if (rawCheckTokens.length === 2) {
                        const entityToken = rawCheckTokens[0];
                        const verbClean = cleanString(rawCheckTokens[1]).toLowerCase();
                        const verbsToAutoResolve = ["create", "edit", "view", "source"];
                        if (verbClean.length >= 2 && verbsToAutoResolve.includes(verbClean) && isTargetEntity(entityToken)) {
                            const properVerb = verbClean.charAt(0).toUpperCase() + verbClean.slice(1).toLowerCase();
                            input.value = entityToken + " " + properVerb + " ";
                            lastTypedTokens = getTokens(input.value, false);
                            setTimeout(() => { handleInput(); }, 50);
                            return;
                        }
                    }
                }

                if (indexToApply !== -1) {
                    setTimeout(() => {
                        apply(indexToApply);
                    }, 50);
                }
            }
        }
    }

    /* ===============================
       3. TOKENIZER
    =============================== */


    function getTokens(str, shouldNormalize = true) {


        //const regex = new RegExp(`"[^"]*"?|${OPERATOR_REGEX_PART}|[^\\s=<>!]+`, "g");
        // const regex = new RegExp(`"[^"]*"|[^\\s]+`, "g");
        const regex = new RegExp(`"[^"]*"?|[^\\s]+`, "g");
        let tokens = str.match(regex) || [];
        if (shouldNormalize && tokens.length >= 2) {
            const first = cleanString(tokens[0]).toLowerCase();
            const second = cleanString(tokens[1]).toLowerCase();
            if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                const temp = tokens[0];
                tokens[0] = tokens[1];
                tokens[1] = temp;
            }
        }
        return tokens;
    }

    function cleanString(val) {
        return (val || "").replace(/["]/g, "").trim();
    }

    function getStructParam() {
        const userName = window.mainUserName || "";
        const userRoles = window.AxUserRoles || "default";
        const userResp = window.userResp || "default";
        return `${userName}$#$${userRoles}$#$${userResp}$#$all$#$all`;
    }

    function getMatchedField(tokenText, transId) {
        if (!tokenText || !transId) return null;
        const searchStr = transId.toLowerCase();
        let list = [];
        for (const key in axDatasourceObj) {
            const keyLower = key.toLowerCase();
            if (keyLower.startsWith("axi_getstructsdata") && keyLower.includes(searchStr)) {
                list = axDatasourceObj[key] || [];
                break;
            }
        }
        const cleanToken = cleanString(tokenText).toLowerCase();
        return list.find(item => 
            (item.name && item.name.toLowerCase() === cleanToken) ||
            (item.caption && item.caption.toLowerCase() === cleanToken) ||
            (item.displaydata && item.displaydata.toLowerCase() === cleanToken)
        ) || null;
    }

    function getCleanCaption(item) {
        if (!item || typeof item !== "object") return "";
        if (item._cleanCaption !== undefined) return item._cleanCaption;
        const displaydata = item.displaydata || item.DISPLAYDATA || "";
        if (typeof displaydata === "string" && displaydata !== "") {
            item._cleanCaption = displaydata
                .replace(/\s*\(.*?\)\s*(?=\[[^\]]+\]$)/, "")
                .replace(/\s*\[[^\]]+\]\s*$/, "")
                .trim()
                .toLowerCase();
        } else {
            item._cleanCaption = "";
        }
        return item._cleanCaption;
    }

    let targetEntitiesCache = {
        sourceList: null,
        matchesMap: null
    };

    function getTargetEntityMatches(token) {
        if (!token) return [];
        const cleanTok = token.replace(/"/g, "").toLowerCase();
        const key = "axi_structmetalist_" + getStructParam().toLowerCase();
        const list = axDatasourceObj[key] || [];

        if (targetEntitiesCache.sourceList !== list || !targetEntitiesCache.matchesMap) {
            const map = new Map();
            for (let i = 0; i < list.length; i++) {
                const d = list[i];
                const namesSet = new Set();
                
                const name = d.name || d.NAME;
                if (name) namesSet.add(name.toLowerCase());

                const caption = d.caption || d.CAPTION;
                if (caption) namesSet.add(caption.toLowerCase());

                const displaydata = d.displaydata || d.DISPLAYDATA;
                if (displaydata) {
                    namesSet.add(displaydata.toLowerCase());
                    const cleanCaption = getCleanCaption(d);
                    if (cleanCaption) namesSet.add(cleanCaption);
                }

                namesSet.forEach(val => {
                    let itemsArray = map.get(val);
                    if (!itemsArray) {
                        itemsArray = [];
                        map.set(val, itemsArray);
                    }
                    itemsArray.push(d);
                });
            }
            targetEntitiesCache.sourceList = list;
            targetEntitiesCache.matchesMap = map;
        }

        return targetEntitiesCache.matchesMap.get(cleanTok) || [];
    }

    function isTargetEntity(token) {
        return getTargetEntityMatches(token).length > 0;
    }

    function areTypesMatching(type1, type2) {
        if (!type1 || !type2) return false;
        const t1 = String(type1).trim().toLowerCase();
        const t2 = String(type2).trim().toLowerCase();
        if (t1 === t2) return true;
        if ((t1 === "t" || t1 === "tstruct") && (t2 === "t" || t2 === "tstruct")) return true;
        if ((t1 === "i" || t1 === "iview") && (t2 === "i" || t2 === "iview")) return true;
        if ((t1 === "p" || t1 === "page") && (t2 === "p" || t2 === "page")) return true;
        if ((t1 === "a" || t1 === "ads") && (t2 === "a" || t2 === "ads")) return true;
        return false;
    }

    function getTargetEntityObj(token, action) {
        if (!token) return null;
        const matches = getTargetEntityMatches(token);

        if (matches.length === 0) return null;
        if (matches.length === 1) return matches[0];

        let matched = null;
        const lowerToken = token.toLowerCase();
        for (const idx in resolvedParams) {
            if (resolvedParams[idx]) {
                const resolvedVal = resolvedParams[idx].toLowerCase();
                const preferredType = resolvedParamType[idx];

                const found = matches.find(d => {
                    const name = (d.name || d.NAME || d.sqlname || "").toLowerCase();
                    const caption = (d.caption || d.CAPTION || "").toLowerCase();
                    const displaydata = (d.displaydata || d.DISPLAYDATA || "").toLowerCase();
                    const cleanCaption = getCleanCaption(d);

                    const nameMatch = name === resolvedVal || caption === resolvedVal || displaydata === resolvedVal || cleanCaption === resolvedVal;
                    if (!nameMatch) return false;

                    if (preferredType) {
                        return areTypesMatching(d.stype || d.STYPE, preferredType);
                    }
                    return true;
                });

                if (found) {
                    matched = found;
                    break;
                }
            }
        }

        if (matched) return matched;

        if (action) {
            const lowAction = action.toLowerCase();
            if (lowAction === "create" || lowAction === "edit") {
                const tstructMatch = matches.find(d => {
                    const type = (d.stype || d.STYPE || "").toLowerCase();
                    return type === "t" || type === "tstruct";
                });
                if (tstructMatch) return tstructMatch;
            } else if (lowAction === "view") {
                const nonTstructMatch = matches.find(d => {
                    const type = (d.stype || d.STYPE || "").toLowerCase();
                    return type !== "t" && type !== "tstruct";
                });
                if (nonTstructMatch) return nonTstructMatch;
            } else if (lowAction === "source") {
                const sourceMatch = matches.find(d => {
                    const type = (d.stype || d.STYPE || "").toLowerCase();
                    return ["t", "tstruct", "i", "iview", "ads"].includes(type);
                });
                if (sourceMatch) return sourceMatch;
            }
        }

        return matches[0];
    }

    function isInboxStructure(item) {
        if (!item) return false;
        const stype = item.stype !== undefined ? item.stype : item.STYPE;
        const name = (item.name || item.NAME || item.displaydata || item.DISPLAYDATA || "").toLowerCase().trim();
        
        if (stype !== undefined && stype !== null) {
            const stypeStr = String(stype).trim().toLowerCase();
            if (stypeStr === "inbox") {
                return true;
            }
        }
        
        const isStypeEmpty = stype === undefined || stype === null || String(stype).trim() === "";
        if (isStypeEmpty && name === "inbox") {
            return true;
        }
        
        return false;
    }

    function isAdsStructure(item) {
        if (!item) return false;
        const stype = item.stype || item.STYPE;
        if (stype === undefined || stype === null) return false;
        return String(stype).trim().toLowerCase() === "ads";
    }

    function isAdsVisible(item) {
        if (!item) return false;
        const view = item.viewallowed;
        if (view === "F" || view === false || view === "false" || view === "NA" || view === "f" || view === "False") {
            return false;
        }
        return true;
    }

    function isActionAllowed(item, action) {
        if (!item) return true;
        const act = action.toLowerCase();
        const stype = (item.stype || item.STYPE || "").toLowerCase().trim();
        if (stype === "p" || stype === "page" || stype === "i" || stype === "iview" || stype === "") {
            return true;
        }
        const hasView = item.viewallowed !== undefined;
        const hasCreate = item.createallowed !== undefined;
        const hasEditKey = item.keyfieldforedit !== undefined;
        if (!hasView && !hasCreate && !hasEditKey) {
            return true;
        }
        if (act === "view") {
            return hasView ? item.viewallowed === "T" : true;
        }
        if (act === "create" || act === "edit") {
            return hasCreate ? item.createallowed === "T" : true;
        }
        return true;
    }

    function isValidActionForTarget(target, action) {
        if (!target || !action) return false;
        const entityObj = getTargetEntityObj(target, action);
        const lowAction = action.toLowerCase();
        let actions = [];
        if (entityObj) {
            const type = (entityObj.stype || entityObj.STYPE || "").toLowerCase().trim();
            const accessPermissions = getAccessPermissions();
            const canBuild = accessPermissions && accessPermissions.buildAccess;

            if (type === "t" || type === "tstruct") {
                if (isActionAllowed(entityObj, "view")) actions.push("View");
                if (isActionAllowed(entityObj, "create")) {
                    actions.push("Create");
                    actions.push("Edit");
                }
                if (canBuild) actions.push("Source");
            } else if (isInboxStructure(entityObj)) {
                actions.push("Go");
            } else if (type === "ads") {
                if (isAdsVisible(entityObj)) actions.push("View");
                if (canBuild) actions.push("Source");
            } else if (type === "i" || type === "iview") {
                if (isActionAllowed(entityObj, "view")) actions.push("View");
                if (canBuild) actions.push("Source");
            } else {
                if (type === "p" || type === "page" || isActionAllowed(entityObj, "view")) actions.push("View");
                if ((type === "i" || type === "iview" || type === "t" || type === "tstruct" || type === "ads") && canBuild) {
                    actions.push("Source");
                }
            }
        }
        return actions.map(act => act.toLowerCase()).includes(lowAction);
    }

    function isViewAllowed(item) {
        if (!item) return false;
        if (isInboxStructure(item)) {
            return false;
        }
        if (isAdsStructure(item)) {
            return isAdsVisible(item);
        }
        const stype = (item.stype || item.STYPE || "").toLowerCase().trim();
        if (stype === "p" || stype === "page" || stype === "i" || stype === "iview") {
            return true;
        }
        return isActionAllowed(item, "view");
    }

    function isStructureVisible(item) {
        if (!item) return false;
        if (isInboxStructure(item)) {
            return true;
        }
        if (isAdsStructure(item)) {
            return isAdsVisible(item);
        }
        const stype = (item.stype || item.STYPE || "").toLowerCase().trim();
        if (stype === "p" || stype === "page" || stype === "i" || stype === "iview") {
            return true;
        }
        const hasView = item.viewallowed !== undefined;
        const hasCreate = item.createallowed !== undefined;
        if (!hasView && !hasCreate) {
            return true;
        }
        return (hasView && item.viewallowed === "T") || (hasCreate && item.createallowed === "T");
    }

    let initialSuggestionsCache = null;
    let initialSuggestionsSourceList = null;
    let initialSuggestionsRunnable = null;

    function getInitialSuggestions() {
        const key = "axi_structmetalist_" + getStructParam().toLowerCase();
        const currentList = axDatasourceObj[key] || [];
        const structType = getStructType();
        const isRunnable = !!(structType && structType !== "o" && !isPreviewModalOpen() && !isRunDisabledForPage() && !isViewDesignerModalOpen());

        if (initialSuggestionsCache && initialSuggestionsSourceList === currentList && initialSuggestionsRunnable === isRunnable) {
            return initialSuggestionsCache;
        }
        const verbs = getInitialCommandsList().filter(k => !["create", "edit", "view", "source"].includes(k.toLowerCase()));
        const targets = currentList.filter(item => isStructureVisible(item));
        initialSuggestionsCache = [...verbs, ...targets];
        initialSuggestionsSourceList = currentList;
        initialSuggestionsRunnable = isRunnable;
        return initialSuggestionsCache;
    }

    function normalizeGlobalState() {
        if (!input) return;
        const tokens = getTokens(input.value, false);
        if (tokens.length >= 2) {
            const first = cleanString(tokens[0]).toLowerCase();
            const second = cleanString(tokens[1]).toLowerCase();
            if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                const val0 = (resolvedParams[0] || "").toLowerCase();
                const val1 = (resolvedParams[1] || "").toLowerCase();
                if (val0 && !["create", "edit", "view", "source"].includes(val0) && val1 && ["create", "edit", "view", "source"].includes(val1)) {
                    const tempVal = resolvedParams[0];
                    resolvedParams[0] = resolvedParams[1];
                    resolvedParams[1] = tempVal;

                    const tempType = resolvedParamType[0];
                    resolvedParamType[0] = resolvedParamType[1];
                    resolvedParamType[1] = tempType;
                    console.log("Global state: Swapped to normalized order");
                }
            } else {
                const val0 = (resolvedParams[0] || "").toLowerCase();
                const val1 = (resolvedParams[1] || "").toLowerCase();
                if (val0 && ["create", "edit", "view", "source"].includes(val0) && val1 && !["create", "edit", "view", "source"].includes(val1)) {
                    const tempVal = resolvedParams[0];
                    resolvedParams[0] = resolvedParams[1];
                    resolvedParams[1] = tempVal;

                    const tempType = resolvedParamType[0];
                    resolvedParamType[0] = resolvedParamType[1];
                    resolvedParamType[1] = tempType;
                    console.log("Global state: Swapped back to user order");
                }
            }
        } else {
            const val0 = (resolvedParams[0] || "").toLowerCase();
            if (val0 && ["create", "edit", "view", "source"].includes(val0)) {
                const tempVal = resolvedParams[0];
                resolvedParams[0] = resolvedParams[1];
                resolvedParams[1] = tempVal;

                const tempType = resolvedParamType[0];
                resolvedParamType[0] = resolvedParamType[1];
                resolvedParamType[1] = tempType;
                console.log("Global state: Swapped back to user order (length < 2)");
            }
        }
    }




    function viewAdsCommandHandling(tokens, commandConfig) {
        //const viewSource = commandConfig?.prompts?.[0]?.promptSource?.toLowerCase();
        //const viewSource = createsourceObj;

        if (SET_COMMAND_STATE.isDropDown) {
            let acceptedValue = cleanString(tokens[tokens.length - 2]);
            if (acceptedValue)
                SET_COMMAND_STATE.currentFieldValue = acceptedValue;

            SET_COMMAND_STATE.isDropDown = false;
        }


        if (SET_COMMAND_STATE.currentFieldType == 'n') {

            return processAdsRepetitiveTokens(tokens, commandConfig);
        }
        else if (SET_COMMAND_STATE.currentFieldType == 'd') {
            let prevValueInSet = tokens[tokens.length - 2].toLowerCase();

            if (prevValueInSet === "today" || prevValueInSet === "yesterday" || prevValueInSet === "tomorrow" ||
                prevValueInSet === "lastweek" || prevValueInSet === "nextweek" || prevValueInSet === "thisweek" ||
                prevValueInSet === "lastmonth" || prevValueInSet === "thismonth" || prevValueInSet === "nextmonth" ||
                prevValueInSet === "thisquarter" || prevValueInSet === "lastquarter" || prevValueInSet === "nextquarter" ||
                prevValueInSet === "thisyear" || prevValueInSet === "lastyear" || prevValueInSet === "nextyear" || prevValueInSet === "custom") {
                dateControlBoolean = true;
            }

            if (dateControlBoolean) {

                if (prevValueInSet === "today" || prevValueInSet === "yesterday" || prevValueInSet === "tomorrow" ||
                    prevValueInSet === "lastweek" || prevValueInSet === "nextweek" || prevValueInSet === "thisweek" ||
                    prevValueInSet === "lastmonth" || prevValueInSet === "thismonth" || prevValueInSet === "nextmonth" ||
                    prevValueInSet === "thisquarter" || prevValueInSet === "lastquarter" || prevValueInSet === "nextquarter" ||
                    prevValueInSet === "thisyear" || prevValueInSet === "lastyear" || prevValueInSet === "nextyear") {

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
                        dateControlBoolean = false;

                    }
                }
                else {
                    if (prevValueInSet.toLowerCase() == "custom") {
                        //let settokens = tokens
                        //let lastIndex = tokens.length - 2;
                        //let lastToken = tokens[lastIndex];
                        let settokens = [...tokens]
                        let lastIndex = settokens.length - 2;
                        let lastToken = settokens[lastIndex];

                        settokens[lastIndex] = "";

                        input.value = settokens.join(" ");

                        showToast("Please Type the date", 5000, true);
                        updateDynamicHintFromPrompt({ prompt: "fieldValue" })

                        filteredObjects = [goOption, popOption];
                        return [
                            goOption,
                            popOption,
                            "Please Type the date"
                        ];
                        //return ["Please Type the date"];
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
                            dateControlBoolean = false;

                        }
                        else {
                            filteredObjects = [goOption, popOption];
                            return [
                                goOption,
                                popOption,
                                "Please type Valid date using / (ex: DD / MM / YYYY)"
                            ];
                            //return ["Please type Valid date using / (ex: DD / MM / YYYY)"];
                        }
                    }
                }
            }
            else {
                let acceptedValue = cleanString(tokens[tokens.length - 1]).toLowerCase();

                const list = [
                    "Custom",
                    "Today",
                    "Yesterday",
                    "Tomorrow",
                    "LastWeek",
                    "NextWeek",
                    "ThisWeek",
                    "LastMonth",
                    "ThisMonth",
                    "NextMonth",
                    "ThisQuarter",
                    "LastQuarter",
                    "NextQuarter",
                    "ThisYear",
                    "LastYear",
                    "NextYear"
                ];

                let filtered = list.filter(col => {

                    const rawDisplay = col.toLowerCase();

                    const normalizedTypedValue = (acceptedValue ?? "")
                        .toLowerCase();

                    return rawDisplay.includes(normalizedTypedValue);
                });


                if (acceptedValue && filtered.length === 0) {
                    console.log("User given value which is not in the date list");
                    showToast("Please select a valid Option from the list", 5000, true);

                    let lastIndex = tokens.length - 1;
                    let lastToken = tokens[lastIndex];
                    tokens[lastIndex] = "";

                    input.value = tokens.join(" ");

                    filtered = list;
                }

                filteredObjects = [goOption, popOption, ...filtered];
                return [
                    goOption,
                    popOption,
                    ...filtered
                ];

                //return filtered;
            }

            return processAdsRepetitiveTokens(tokens, commandConfig);

        }
        else
            return processAdsRepetitiveTokens(tokens, commandConfig);
    }

    function isValidSuggestion(item) {
        return !!(
            item &&
            (
                (typeof item.displaydata === "string" && item.displaydata.trim()) ||
                (typeof item.name === "string" && item.name.trim())
            )
        );
    }


    function processAdsRepetitiveTokens(tokens, commandConfig) {
        let targetIndex = tokens.length - 1;
        let partialTyped = cleanString(tokens[targetIndex]);

        const adsName = cleanString(tokens[1]);
        setCommandTransid = adsName;

        if (targetIndex < 2) return [];

        if (SET_COMMAND_STATE.transid === null || setCommandTransid !== SET_COMMAND_STATE.transid) {
            SET_COMMAND_STATE = {
                isNextField: false,
                currentField: null,
                currentFieldType: null,
                isFirst: true,
                transid: adsName,
                currentFieldValue: null,
                isDropDown: false

            };
        }


        if (targetIndex % 2 === 0) {
            const sourceName = "axi_adscolumnlist";
            const sourceKey = `${sourceName}_${adsName}`.toLowerCase();


            if (!axDatasourceObj[sourceKey]) {
                loadList(sourceName, adsName);
                return ["Loading columns..."];
            }

            const list = axDatasourceObj[sourceKey];
            if (!Array.isArray(list)) {
                filteredObjects = [goOption, popOption];
                return [goOption, popOption];
            }

            const safeList = list.filter(isValidSuggestion);

            if (safeList.length === 0) {
                filteredObjects = [goOption, popOption];
                return [goOption, popOption];
            }


            const usedColumns = new Set();
            for (let i = 2; i < targetIndex; i += 2) {
                const usedToken = cleanString(tokens[i]).toLowerCase();
                usedColumns.add(usedToken);
            }

            const lastToken = tokens[targetIndex];
            if (lastToken && lastToken.includes(",")) {
                const parts = lastToken.split(",");
                partialTyped = cleanString(parts[parts.length - 1]);
                for (let i = 0; i < parts.length - 1; i++) {
                    const p = cleanString(parts[i]).toLowerCase();
                    if (p) usedColumns.add(p);
                }
            }




            const filtered = safeList.filter(col => {

                const rawDisplay = (col?.displaydata || col?.name)?.toLowerCase();


                const cleanDisplay = rawDisplay?.replace(/\s*\(.*?\)/g, "")?.replace(/\s*\[[^\]]+\]\s*$/, "")?.trim();

                const rawName = (col.name || "").toLowerCase();


                const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);


                const matchesInput = cleanDisplay.includes(partialTyped.toLowerCase());

                return !isUsed && matchesInput;
            });




            SET_COMMAND_STATE.currentField = null;
            SET_COMMAND_STATE.currentFieldType = null
            SET_COMMAND_STATE.currentFieldValue = null;
            SET_COMMAND_STATE.isNextField = false;
            SET_COMMAND_STATE.isDropDown = false;


            filteredObjects = [goOption, popOption, ...filtered];
            if (tokens.length > 2) {
                return [
                    goOption,
                    popOption,
                    ...filtered.map(col => col.displaydata || col.name)
                ];

            }



        } else {
            const prevColToken = tokens[targetIndex - 1];
            if (prevColToken && prevColToken.includes(",")) {
                SET_COMMAND_STATE.isDropDown = false;
                filteredObjects = [goOption, popOption];
                return [goOption, popOption];
            }
            if (!SET_COMMAND_STATE.isNextField) {
                let prevColumnName
                if (!SET_COMMAND_STATE.currentField) {
                    prevColumnName = cleanString(tokens[targetIndex - 1]);
                    SET_COMMAND_STATE.currentField = prevColumnName;
                } else
                    prevColumnName = SET_COMMAND_STATE.currentField;

                //const prevColumnName = cleanString(tokens[targetIndex - 1]);


                const colSourceKey = `axi_adscolumnlist_${adsName}`.toLowerCase();
                const colList = axDatasourceObj[colSourceKey];

                if (!colList) {
                    console.log("In processAds " + "axi_adscolumnlist" + " is empty");
                    showToast("Please Try Again Later.");
                    return [];
                }

                const columnMetadata = colList.find(
                    c =>
                        c.name?.toLowerCase() === prevColumnName.toLowerCase() ||
                        c.displaydata?.toLowerCase().replace(/\s*\(.*?\)/g, '').trim() === prevColumnName.toLowerCase()
                ) || null;

                if (!columnMetadata) {
                    //console.log("Selected Field Name is Not in the List " + prevColumnName);
                    //showToast("Please Select Field from the list", 5000, true);
                    //return [];

                    console.log("Selected Field Name is Not in the List " + prevColumnName);
                    showToast("Please Select Field from the list", 5000, true);

                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];

                    settokens[lastIndex] = "";

                    targetIndex = targetIndex - 1;

                    input.value = settokens.join(" ");
                    updateDynamicHintFromPrompt({ prompt: "fieldname" })


                    const usedColumns = new Set();
                    for (let i = 2; i < targetIndex; i += 2) {
                        const usedToken = cleanString(tokens[i]).toLowerCase();
                        usedColumns.add(usedToken);
                    }


                    const safeColList = colList.filter(isValidSuggestion);


                    // const filtered = colList.filter(col => {

                    //     const rawDisplay = (col.displaydata || col.name).toLowerCase();

                    //     const cleanDisplay = rawDisplay
                    //         .replace(/\s*\(.*?\)/g, "")
                    //         .replace(/\s*\[[^\]]+\]\s*$/, "")
                    //         .trim();

                    //     const rawName = (col.name || "").toLowerCase();

                    //     const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);

                    //     return !isUsed;
                    // });

                    const filtered = safeColList.filter(col => {
                        const rawDisplay = (col.displaydata || col.name || "")
                            .toLowerCase();

                        const cleanDisplay = rawDisplay
                            .replace(/\s*\(.*?\)/g, "")
                            .replace(/\s*\[[^\]]+\]\s*$/, "")
                            .trim();

                        const rawName = (col.name || "").toLowerCase();

                        const isUsed =
                            usedColumns.has(cleanDisplay) ||
                            usedColumns.has(rawName);

                        return !isUsed;
                    });


                    SET_COMMAND_STATE.currentField = null;
                    SET_COMMAND_STATE.currentFieldType = null
                    SET_COMMAND_STATE.currentFieldValue = null;
                    SET_COMMAND_STATE.isNextField = false;
                    SET_COMMAND_STATE.isDropDown = false;


                    filteredObjects = [goOption, popOption, ...filtered];
                    if (tokens.length > 2) {
                        return [
                            goOption,
                            popOption,
                            ...filtered.map(col => col.displaydata || col.name)
                        ];

                    }
                }

                let isAccept = !columnMetadata.sourcetable || !columnMetadata.sourcefld;


                let datatype;

                if (SET_COMMAND_STATE.currentFieldType === null) {
                    datatype = columnMetadata.fdatatype;
                    SET_COMMAND_STATE.currentFieldType = datatype;
                } else datatype = SET_COMMAND_STATE.currentFieldType;


                if (datatype === 'c' || datatype === 'n' || datatype === "t") {
                    if (isAccept) {
                        let acceptedValue = cleanString(tokens[tokens.length - 1]);
                        const columnName = prevColumnName
                        adsfieldvalueanddt[columnName] = {
                            datatype: datatype,
                            isAccept: isAccept,
                        };
                        if (acceptedValue)
                            SET_COMMAND_STATE.currentFieldValue = acceptedValue;
                        else {
                            filteredObjects = [goOption, popOption];
                            return [
                                goOption,
                                popOption,
                                "Please type the value..."
                            ];
                            //return ["Please type the value..."];
                        }
                        //return [];
                        // const columnName = prevColumnName
                        // adsfieldvalueanddt[columnName] = {
                        //     datatype: datatype,
                        //     isAccept: isAccept,
                        // };
                        //if (tokens?.length <= 4) {
                        //    return [goOption,];
                        //}
                        return [];
                    } else {
                        SET_COMMAND_STATE.isDropDown = true;
                        if (!columnMetadata.sourcetable || !columnMetadata.sourcefld) {
                            console.log("Error in DropDownField check: sourcetable or sourcefld is empty");
                            showToast("Please Try Again Later.");
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

                        if (acceptedValue && filtered.length === 0) {
                            console.log("User given value is not in the dropdown");
                            showToast("Please select a valid value from the dropdown", 5000, true);

                            let lastIndex = tokens.length - 1;
                            let lastToken = tokens[lastIndex];
                            tokens[lastIndex] = "";

                            input.value = tokens.join(" ");

                            filtered = list;
                        }




                        //if (tokens.length <= 4) {
                        //    filteredObjects = [goOption, ...filtered];
                        //    return [
                        //        goOption,
                        //        ...filtered.map(col => col.displaydata || col.name)
                        //    ];

                        //}


                        //filteredObjects = filtered

                        //return filtered.map(col => col.displaydata || col.name);

                        filteredObjects = [goOption, popOption, ...filtered];
                        return [
                            goOption,
                            popOption,
                            ...filtered.map(col => col.displaydata || col.name)
                        ];



                    }
                } else if (datatype === 'd') {


                    const acceptedValue = cleanString(tokens[tokens.length - 1]);

                    const columnName = prevColumnName
                    adsfieldvalueanddt[columnName] = {
                        datatype: datatype,
                        isAccept: isAccept,
                    };

                    const list = [
                        "Custom",
                        "Today",
                        "Yesterday",
                        "Tomorrow",
                        "LastWeek",
                        "NextWeek",
                        "ThisWeek",
                        "LastMonth",
                        "ThisMonth",
                        "NextMonth",
                        "ThisQuarter",
                        "LastQuarter",
                        "NextQuarter",
                        "ThisYear",
                        "LastYear",
                        "NextYear"
                    ];

                    const filtered = list.filter(col => {

                        const rawDisplay = col.toLowerCase();

                        const normalizedTypedValue = (acceptedValue ?? "")
                            .toLowerCase();

                        return rawDisplay.includes(normalizedTypedValue);
                    });


                    //return filtered;

                    filteredObjects = [goOption, popOption, ...filtered];
                    return [
                        goOption,
                        popOption,
                        ...filtered
                    ];


                }

                //else if (datatype === 'd') {

                //}
            } else return [];

        }
    }

    //function processAdsRepetitiveTokens(tokens, commandConfig) {
    //    const targetIndex = tokens.length - 1;
    //    const partialTyped = cleanString(tokens[targetIndex]);
    //    const adsName = cleanString(tokens[1]);


    //    if (targetIndex < 2) return [];


    //    if (targetIndex % 2 === 0) {
    //        const sourceName = "axi_adscolumnlist";
    //        const sourceKey = `${sourceName}_${adsName}`.toLowerCase();


    //        if (!axDatasourceObj[sourceKey]) {
    //            loadList(sourceName, adsName);
    //            return ["Loading columns..."];
    //        }

    //        const list = axDatasourceObj[sourceKey];
    //        if (!Array.isArray(list)) return [];


    //        const usedColumns = new Set();
    //        for (let i = 2; i < targetIndex; i += 2) {
    //            const usedToken = cleanString(tokens[i]).toLowerCase();
    //            usedColumns.add(usedToken);
    //        }






    //        const filtered = list.filter(col => {

    //            const rawDisplay = (col.displaydata || col.name).toLowerCase();


    //            const cleanDisplay = rawDisplay
    //                .replace(/\s*\(.*?\)/g, "")
    //                .replace(/\s*\[[^\]]+\]\s*$/, "")
    //                .trim();

    //            const rawName = (col.name || "").toLowerCase();


    //            const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);


    //            const matchesInput = cleanDisplay.includes(partialTyped.toLowerCase());

    //            return !isUsed && matchesInput;
    //        });




    //        filteredObjects = [goOption, ...filtered];
    //        if (tokens.length > 2) {
    //            return [
    //                goOption,
    //                ...filtered.map(col => col.displaydata || col.name)
    //            ];

    //        }

    //    }


    //    else {

    //        const prevColumnName = cleanString(tokens[targetIndex - 1]);


    //        const colSourceKey = `axi_adscolumnlist_${adsName}`.toLowerCase();
    //        const colList = axDatasourceObj[colSourceKey];

    //        if (!colList) return [];

    //        const columnMetadata = colList.find(
    //            c =>
    //                c.name?.toLowerCase() === prevColumnName.toLowerCase() ||
    //                c.displaydata?.toLowerCase().replace(/\s*\(.*?\)/g, '').trim() === prevColumnName.toLowerCase()
    //        ) || null;

    //        if (!columnMetadata) return [];

    //        const isAccept = !columnMetadata.sourcetable || !columnMetadata.sourcefld;


    //        const datatype = columnMetadata.fdatatype;


    //        if (isAccept) {
    //            const acceptedValue = cleanString(tokens[tokens.length - 1]);
    //            const columnName = prevColumnName
    //            adsfieldvalueanddt[columnName] = {
    //                datatype: datatype,
    //                isAccept: isAccept,
    //            };
    //            if (tokens?.length <= 4) {

    //                return [
    //                    goOption,

    //                ];


    //            }
    //            return [];
    //        }
    //        else {

    //            if (!columnMetadata.sourcetable || !columnMetadata.sourcefld) {
    //                console.log("Error in DropDownField check: sourcetable or sourcefld is empty");
    //                return [];
    //            }

    //            const acceptedValue = cleanString(tokens[tokens.length - 1]);
    //            const columnName = prevColumnName
    //            adsfieldvalueanddt[columnName] = {
    //                datatype: datatype,
    //                isAccept: isAccept,
    //            };

    //            const sourcetable = columnMetadata.sourcetable;
    //            const sourcefld = columnMetadata.sourcefld;

    //            const sourceName = "axi_adsdropdowntokens";
    //            const paramValue = `${sourcetable}$#$${sourcefld}`;
    //            const sourceKey = `${sourceName}_${paramValue}`.toLowerCase();

    //            if (!axDatasourceObj[sourceKey]) {
    //                loadList(sourceName, paramValue);
    //                return ["Loading values..."];
    //            }

    //            const list = axDatasourceObj[sourceKey];
    //            if (!Array.isArray(list)) return [];


    //            const filtered = list.filter(col => {
    //                const rawDisplay = String(col.displaydata || col.name)
    //                    .toLowerCase();

    //                const normalizedTypedValue = (acceptedValue ?? "")
    //                    .toLowerCase();

    //                return !normalizedTypedValue || rawDisplay.includes(normalizedTypedValue);
    //            });





    //            if (tokens.length <= 4) {
    //                filteredObjects = [goOption, ...filtered];
    //                return [
    //                    goOption,
    //                    ...filtered.map(col => col.displaydata || col.name)
    //                ];

    //            }

    //            filteredObjects = filtered


    //            return filtered.map(col => col.displaydata || col.name);

    //        }

    //    }
    //}

    function processRunCommands(tokens, targetIndex, structType) {
        if (targetIndex !== 1) return [goOption];
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
                    
                    if (structType === "i") {
                        const utilityButtons = getIViewUtilityButtons();
                        const actionButtons = getIViewActionDropdownButtons();
                        allButtons = { ...allButtons, ...utilityButtons, ...actionButtons };
                    }
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

            case "s":
                settingsPageButtons = getButtons(".Config-cont");
                allButtons = { ...settingsPageButtons };
                break;

            case "im":
            case "ex":
                importExportButtons = getButtons(".card-footer ");
                allButtons = { ...importExportButtons };
                break;



            default:
                console.error("Invalid StructType")
                break;
        }

        buttonsList = Object.values(allButtons).map(btn => {
            const element = btn.element;
            let displayLabel = "";
            if (element) {
                const titleAttr = element.getAttribute("title");
                if (titleAttr && titleAttr.trim()) {
                    displayLabel = titleAttr.trim();
                } else {
                    const cloned = element.cloneNode(true);
                    cloned.querySelectorAll(".material-icons, .material-icons-style").forEach(node => node.remove());
                    displayLabel = (cloned.textContent || "").trim();
                }
            }
            if (!displayLabel) {
                displayLabel = (btn.label || "").trim();
            }
            // Normalize whitespaces and newlines
            displayLabel = displayLabel.replace(/[\r\n\t]+/g, ' ').replace(/\s+/g, ' ').trim();
            // Remove trailing brackets containing icon names
            displayLabel = displayLabel.replace(/\s*\[[^\]]+\]\s*$/, '').trim();
            return {
                name: btn.id,
                onclick: btn.element ? btn.element.getAttribute("onclick") : null,
                displaydata: displayLabel
            };
        })
        .filter(btn => !((structType === "e" || structType === "ef") && btn.name === "deleteSelectedButton"));

        const uniqueButtonsMap = new Map();
        buttonsList.forEach(btn => {
            if (btn.onclick) {
                if (!uniqueButtonsMap.has(btn.onclick)) {
                    uniqueButtonsMap.set(btn.onclick, btn);
                }
            } else {
                uniqueButtonsMap.set(btn.name, btn);
            }
        });

        const filtered = Array.from(uniqueButtonsMap.values()).filter(item =>
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

    const isEmpty = val => typeof val === "string" ? val.trim() === "" : val === null || val === undefined;

    function getMatchedField(tokenText, transId) {
        if (!tokenText || !transId) return null;
        const searchStr = transId.toLowerCase();
        let list = [];
        for (const key in axDatasourceObj) {
            const keyLower = key.toLowerCase();
            if (keyLower.startsWith("axi_getstructsdata") && keyLower.includes(searchStr)) {
                list = axDatasourceObj[key] || [];
                break;
            }
        }
        const cleanToken = cleanString(tokenText).toLowerCase();
        return list.find(item => 
            (item.name && item.name.toLowerCase() === cleanToken) ||
            (item.caption && item.caption.toLowerCase() === cleanToken) ||
            (item.displaydata && item.displaydata.toLowerCase() === cleanToken)
        ) || null;
    }

    function suggestLocal(inputText) {
        normalizeGlobalState();
        let ignoreExtraParams = false;
        let detectedType = "";
        const rawTokens = getTokens(inputText, false);
        if (rawTokens.length > 0) {
            const firstToken = cleanString(rawTokens[0]).toLowerCase();
            if (["create", "view", "edit", "source"].includes(firstToken)) {
                filteredObjects = [];
                hintDiv.textContent = "";
                return [];
            }
        }
        const endsWithSpace = inputText.endsWith(" ");


        const lastTokenRaw = rawTokens[rawTokens.length - 1];
        const isUnclosedString = lastTokenRaw && lastTokenRaw.startsWith('"') && (!lastTokenRaw.endsWith('"') || lastTokenRaw === '"');
        if (endsWithSpace && !isUnclosedString) rawTokens.push("");

        if (rawTokens.length === 0) {
            hintDiv.textContent = "";
            const allSuggestions = getInitialSuggestions();
            filteredObjects = allSuggestions.map(item => {
                if (typeof item === "string") {
                    return { name: item, displaydata: item };
                }
                return item;
            });
            return allSuggestions.map(item => typeof item === "string" ? item : (item.displaydata || item.name));
        }

        const groupKeyRaw = cleanString(rawTokens[0]);

        if (rawTokens.length === 1 && (!endsWithSpace || isUnclosedString)) {
            hintDiv.textContent = "";
            const prefix = groupKeyRaw.toLowerCase();
            const allSuggestions = getInitialSuggestions();
            const filtered = allSuggestions.filter(item => {
                if (typeof item === "string") {
                    return item.toLowerCase().startsWith(prefix);
                } else {
                    const nameMatch = item.name && item.name.toLowerCase().includes(prefix);
                    const displayMatch = item.displaydata && item.displaydata.toLowerCase().includes(prefix);
                    const cleanCaptionMatch = typeof item.displaydata === "string" && 
                        getCleanCaption(item).includes(prefix);
                    const captionMatch = item.caption && item.caption.toLowerCase().includes(prefix);
                    return nameMatch || displayMatch || cleanCaptionMatch || captionMatch;
                }
            });

            // Rank startsWith matches higher than includes matches to keep better matching.
            const startsWithPrefix = [];
            const containsPrefix = [];
            filtered.forEach(item => {
                if (typeof item === "string") {
                    startsWithPrefix.push(item);
                } else {
                    const starts = (item.name && item.name.toLowerCase().startsWith(prefix)) ||
                                   (item.displaydata && item.displaydata.toLowerCase().startsWith(prefix)) ||
                                   (typeof item.displaydata === "string" && getCleanCaption(item).startsWith(prefix)) ||
                                   (item.caption && item.caption.toLowerCase().startsWith(prefix));
                    if (starts) {
                        startsWithPrefix.push(item);
                    } else {
                        containsPrefix.push(item);
                    }
                }
            });
            const rankedFiltered = [...startsWithPrefix, ...containsPrefix];
             filteredObjects = rankedFiltered.map(item => {
                if (typeof item === "string") {
                    return { name: item, displaydata: item };
                }
                return item;
             });
             return rankedFiltered.map(item => typeof item === "string" ? item : (item.displaydata || item.name));
        }

        if (rawTokens.length === 2 && isTargetEntity(rawTokens[0])) {
            const entityObj = getTargetEntityObj(rawTokens[0]);
            const partial = cleanString(rawTokens[1] || "").toLowerCase();

            if (entityObj && isInboxStructure(entityObj)) {
                const filtered = [goOption].filter(opt => {
                    const displayText = opt.displaydata || opt.name;
                    return displayText.toLowerCase().startsWith(partial);
                });
                filteredObjects = filtered;
                return filtered;
            }

            let actions = [];
            let targetType = "";
            if (entityObj) {
                targetType = (entityObj.stype || entityObj.STYPE || "").toLowerCase();
            }

            const accessPermissions = getAccessPermissions();
            const canBuild = accessPermissions && accessPermissions.buildAccess;

            if (targetType === "t" || targetType === "tstruct") {
                const hasPerms = entityObj.viewallowed !== undefined || entityObj.createallowed !== undefined;
                if (!hasPerms) {
                    actions = ["Create", "Edit", "View"];
                } else {
                    if (entityObj.viewallowed === "T") actions.push("View");
                    if (entityObj.createallowed === "T") {
                        actions.push("Create");
                        actions.push("Edit");
                    }
                }
                if (canBuild) {
                    actions.push("Source");
                }
            } else if (targetType === "p" || targetType === "page") {
                actions.push("View");
            } else if (targetType === "ads") {
                if (isAdsVisible(entityObj)) actions.push("View");
                if (canBuild) {
                    actions.push("Source");
                }
            } else if (targetType === "i" || targetType === "iview") {
                const hasPerms = entityObj.viewallowed !== undefined || entityObj.createallowed !== undefined;
                if (!hasPerms || entityObj.viewallowed === "T") actions.push("View");
                if (canBuild) {
                    actions.push("Source");
                }
            } else if (targetType) {
                const hasPerms = entityObj.viewallowed !== undefined || entityObj.createallowed !== undefined;
                if (!hasPerms || entityObj.viewallowed === "T") actions.push("View");
                if ((targetType === "i" || targetType === "iview" || targetType === "t" || targetType === "tstruct" || targetType === "ads") && canBuild) {
                    actions.push("Source");
                }
            } else {
                const matches = getTargetEntityMatches(rawTokens[0]);

                let hasView = false;
                let hasCreate = false;
                let hasGo = false;
                let hasTstruct = false;
                let matchesAnyTstructWithoutPerms = false;
                let matchesAnyIviewWithoutPerms = false;
                let hasSource = false;
                for (let i = 0; i < matches.length; i++) {
                    const m = matches[i];
                    const type = (m.stype || m.STYPE || "").toLowerCase().trim();
                    const hasPerms = m.viewallowed !== undefined || m.createallowed !== undefined;
                    if (isInboxStructure(m)) {
                        hasGo = true;
                    } else if (type === "t" || type === "tstruct") {
                        hasTstruct = true;
                        if (!hasPerms) {
                            matchesAnyTstructWithoutPerms = true;
                        } else {
                            if (m.viewallowed === "T") hasView = true;
                            if (m.createallowed === "T") hasCreate = true;
                        }
                        if (canBuild) hasSource = true;
                    } else if (type === "ads") {
                        if (isAdsVisible(m)) hasView = true;
                        if (canBuild) hasSource = true;
                    } else if (type === "p" || type === "page") {
                        hasView = true;
                    } else if (type === "i" || type === "iview") {
                        if (!hasPerms) {
                            matchesAnyIviewWithoutPerms = true;
                        } else {
                            if (m.viewallowed === "T") hasView = true;
                        }
                        if (canBuild) hasSource = true;
                    } else {
                        if (!hasPerms) {
                            matchesAnyIviewWithoutPerms = true;
                        } else {
                            if (m.viewallowed === "T") hasView = true;
                        }
                        if ((type === "i" || type === "iview" || type === "t" || type === "tstruct" || type === "ads") && canBuild) {
                            hasSource = true;
                        }
                    }
                }
                if (hasView || matchesAnyTstructWithoutPerms || matchesAnyIviewWithoutPerms) actions.push("View");
                if (hasGo) actions.push("Go");
                if (hasTstruct && (hasCreate || matchesAnyTstructWithoutPerms)) {
                    actions.push("Create");
                    actions.push("Edit");
                }
                if (hasSource) actions.push("Source");
            }

            const specialTypes = ["iview", "i", "ads", "p", "page"];
            if (targetType && specialTypes.includes(targetType)) {
                const viewAction = { name: "View", displaydata: "View" };
                const allOptions = [viewAction];
                if (["iview", "i", "ads"].includes(targetType) && canBuild) {
                    allOptions.push({ name: "Source", displaydata: "Source" });
                }
                allOptions.push(goOption, popOption);
                const filtered = allOptions.filter(opt => {
                    const displayText = opt.displaydata || opt.name;
                    return displayText.toLowerCase().startsWith(partial);
                });
                filteredObjects = filtered;
                return filtered.map(item => {
                    if (item.isExecutable) {
                        return item;
                    }
                    return item.displaydata || item.name;
                });
            }

            if (actions.map(act => act.toLowerCase()).includes(partial)) {
                if (partial === "source") {
                    filteredObjects = [goOption];
                    return [goOption];
                }
                filteredObjects = [goOption, popOption];
                return [goOption, popOption];
            }

            const filteredActions = actions.filter(act => act.toLowerCase().startsWith(partial));
            filteredObjects = filteredActions.map(act => ({ name: act, displaydata: act }));
            return filteredActions;
        }

        let tokens = [...rawTokens];
        if (tokens.length >= 2) {
            const first = cleanString(tokens[0]).toLowerCase();
            const second = cleanString(tokens[1]).toLowerCase();
            if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                const temp = tokens[0];
                tokens[0] = tokens[1];
                tokens[1] = temp;
            }
        }

        const groupKey = cleanString(tokens[0]);

        if (groupKey.toLowerCase() === "configure" && tokens[1] && cleanString(tokens[1]).toLowerCase() === "responsibilities") {
            filteredObjects = [goOption];
            return [goOption];
        }

        if (groupKey.toLowerCase() === "help") {
            return [goOption];
        }

        if (groupKey.toLowerCase() === "version") {
            return [goOption];
        }

        if (groupKey.toLowerCase() === "source" && tokens.length >= 2 && isTargetEntity(tokens[1])) {
            filteredObjects = [goOption];
            return [goOption];
        }

        //const commandConfig = commands[groupKey];


        let commandConfig = getCommandConfig(groupKey);


        /** Begin: Note: This must be removed when releasing */
        // if (groupKey.toLowerCase() === "configure") {
        //     //commandConfig.prompts[0].promptValues = "PEG,Form Notification,Scheduled Notification,Peg Form Notification,Job,Rule,Application Properties,"
        //     //    +"KeyField,News And Announcement,Settings,User,Users,Role,Roles,Publish Axpert API,Publish Config Studio,Card,Responsibility,Responsibilities,User Group,"
        //     //    +"Dimension,Actor,Actors,User Activation,User Permission,User Permissions,Role Permissions";
        //     //commandConfig.prompts[1].promptSource = "Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_JobNamesList,Axi_RuleNamesList,Axi_Dummy,"
        //     //    + "axi_structlist,axi_newsandannounce,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_Dummy,axi_publishapi,Axi_ServernameList,axi_cardlist,axi_resposibilitylist,Axi_Dummy,axi_usergrouplist,"
        //     //    +"axi_dimensionlist,axi_actorlist,Axi_Dummy,axi_useractivation,axi_userlist,axi_userlist,axi_rolelist";
        //     commandConfig.prompts[0].promptValues = "PEG,Form Notification,Scheduled Notification,Peg Form Notification,Rule,"
        //         + "KeyField,News And Announcement,User,Users,User Permission,User Permissions,User Activation,User Group,Role,Roles,Role Permissions,"
        //         + "Actor, Actors, Publish Axpert API, Publish Config Studio, Card, Responsibility, Responsibilities,"
        //         + "Dimension,Application Properties,Settings";
        //     commandConfig.prompts[1].promptSource = "Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_RuleNamesList,"
        //         + "axi_structlist,axi_newsandannounce,Axi_Dummy,Axi_Dummy,axi_userlist,axi_userlist,axi_useractivation,axi_usergrouplist,Axi_Dummy,Axi_Dummy,axi_rolelist,"
        //         + "axi_actorlist,Axi_Dummy,axi_publishapi,Axi_ServernameList,axi_cardlist,axi_resposibilitylist,Axi_Dummy,"
        //         + "axi_dimensionlist,Axi_Dummy,Axi_Dummy,";
        // }
        // if (groupKey.toLowerCase() === "open") {
        //     commandConfig.prompts[0].promptValues = "Tstruct,Iview,Axpert Data Sources,Page,Arrange Menu,Dev Option,App Variables,Db Explorer,API Plugin,Axpert Job,Language,Publish,Custom Data Type,Email Definition,Table Field Descriptor,Custom Plugin,Queue Listing,Out Bound Queue,In Bound Queue,Mem DB Console";
        //     commandConfig.prompts[1].promptSource = "axi_structmetalist,axi_structmetalist,axi_structmetalist,axi_structmetalist,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_Dummy,Axi_APINamesList,axi_jobs,axi_language,Axi_Dummy,axi_customtype,axi_emaildef,axi_tabledesc,Axi_Dummy,Axi_Dummy,axi_outbound,axi_inbound,Axi_Dummy";
        // }

        /** End: Note: This must be removed when releasing */



        if (!commandConfig) { hintDiv.textContent = ""; return []; }

        const targetIndex = tokens.length - 1;

        if (commandConfig.commandGroup?.toLowerCase() === "view") {
            const viewSource = commandConfig?.prompts?.[0]?.promptSource?.toLowerCase();
            const viewValues = commandConfig.prompts?.[0]?.promptValues;
            const firstToken = cleanString(tokens[1] || "");
            const { value: actualFirstToken, type } = tryResolveToken(1, firstToken, commandConfig, false);
            detectedType = getType(viewSource.toLowerCase(), { value: actualFirstToken, type: type }, viewValues, tokens, commandConfig);
            // detectedType = getType(viewSource.toLowerCase(), {value: act}, viewValues, tokens, commandConfig);

            if (detectedType === "ads") {
                ignoreExtraParams = true;
                if (tokens.length > 2) {
                    updateDynamicHintFromPrompt({ prompt: (targetIndex % 2 === 0) ? "fieldname" : "fieldvalue" });
                    return viewAdsCommandHandling(tokens, commandConfig)

                }

                console.log("ResolutionContext: ignoreExtraParams = true (ADS)")
            }
        }

        ///Need to make a common function for processAdsRepetitivetokens
        if (groupKey.toLowerCase() === "create" && tokens.length > 3) {
            let viewSource = commandConfig?.prompts?.[2]?.promptSource?.toLowerCase();
            //let viewSource = "Axi_FieldList".toLowerCase();
            let tokenCopy = [...tokens];
            //let dummyValue = commandConfig?.prompts?.[1]?.promptValues.toLowerCase();
            //let orgTokens = [...tokens];
            //if (dummyValue) {
            //    tokenCopy = tokenCopy.filter(t => t?.toLowerCase() !== dummyValue.toLowerCase());
            //}
            updateDynamicHintFromPrompt({ prompt: (targetIndex % 2 !== 0) ? "fieldname" : "fieldValue" })
            return createCommandHandling(tokenCopy, commandConfig, viewSource);
        }

        if (groupKey.toLowerCase() === "run") {
            const structType = getStructType();

            if (!structType || structType === "o" || isPreviewModalOpen() || isRunDisabledForPage() || isViewDesignerModalOpen()) {
                if (isPreviewModalOpen() || isRunDisabledForPage() || isViewDesignerModalOpen()) {
                    return [];
                }
                showToast("Warning: CommandGroup Invalid: Please open Tstruct or Any other page");
                return [];
            }



            return processRunCommands(tokens, targetIndex, structType);
        }
        const promptInfo = getActivePromptInfo(commandConfig, tokens, targetIndex);


        ///KeyValue based edit handling.

        if ((!promptInfo || tokens[3] === "with") && groupKey.toLowerCase() === "edit" && tokens.length >= 4) {

            let viewSource;
            if (tokens.length == 4) {
                viewSource = commandConfig?.prompts?.[3]?.promptValues?.toLowerCase().split(',').map(v => v.trim());
                //const partialTyped = cleanString(tokens[targetIndex]);
                const result = viewSource.filter(val => val.toLowerCase());
                filteredObjects = result.map(val => ({ name: val, displaydata: val }));

                result.unshift(popOption);
                result.unshift(goOption);
                filteredObjects.unshift(popOption);
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

        // if (promptInfo?.error) {
        //     showToast(promptInfo.error);
        //     return [];
        // }

        if (!promptInfo) {
            updateDynamicHintFromPrompt(null);

            ///added as we now dont have option for go and popup in create(When create old logic works we can remove this)(T)
            if (groupKey.toLowerCase() === "create" && tokens.length === 3) {
                filteredObjects = [goOption, popOption];
                return [goOption, popOption];
            }

            ///added as we now dont have option for go and popup in create(When create old logic works we can remove this)(T)
            else if (groupKey.toLowerCase() === "view" && tokens.length >= 3) {
                const secondToken = cleanCommandToken(tokens[1]).toLowerCase();

                if (secondToken === "source") {
                    filteredObjects = [goOption];
                    return [goOption];
                }
                filteredObjects = [goOption, popOption];
                return [goOption, popOption];
            }

            else if (groupKey.toLowerCase() === "source" && tokens.length >= 3) {
                filteredObjects = [goOption];
                return [goOption];
            }

            else if (groupKey.toLowerCase() === "sdk" && tokens.length > 2) {
                filteredObjects = [goOption];
                return [goOption];
            }

            else if (groupKey.toLowerCase() === "configure" && tokens.length > 2) {
                if (cleanCommandToken(tokens[1]).toLowerCase() === "keyfield") {
                    filteredObjects = [goOption];
                    return [goOption];
                }
                else {
                    filteredObjects = [goOption, popOption];
                    return [goOption, popOption];
                }
            }

            else if (groupKey.toLowerCase() === "analyse" && tokens.length > 2) {
                filteredObjects = [goOption];
                return [goOption];
            }

            if (["upload", "download"].includes(groupKey.toLowerCase()) && tokens.length >= 2) {
                filteredObjects = [goOption];
                return [goOption];
            }

            return [];
        }






        const { config: activePrompt, realSource } = promptInfo;
        updateDynamicHintFromPrompt(activePrompt);

        const partialTyped = cleanString(tokens[targetIndex]);

        //We return go/pop up for configure when tokens length is greater than 3(Below we have another check it will work but realsource is finded so here itself we returning it for configure)
        if (groupKey.toLowerCase() === "configure" && tokens.length > 3 && tokens[1].toLowerCase() != "keyfield") {
            filteredObjects = [goOption, popOption];
            return [goOption, popOption]
        }


        // Scenario A: Static Values

        ///Skipping PromptValue "With" token for edit
        if (!realSource && activePrompt.promptValues && groupKey.toLowerCase() !== "edit") {

            const staticValues = activePrompt.promptValues.split(',').map(v => v.trim());
            const result = staticValues.filter(val => val.toLowerCase().startsWith(partialTyped.toLowerCase()));
            filteredObjects = result.map(val => ({ name: val, displaydata: val }));

            if (groupKey.toLowerCase() === "create" && tokens.length === 3) {
                result.unshift(popOption);
                result.unshift(goOption);
                filteredObjects.unshift(popOption);
                filteredObjects.unshift(goOption);
            }

            //  if (staticValues.length > 0 && result.length === 0 && partialTyped.length > 0) {

            //     console.warn(`[Validation] Invalid input detected: "${partialTyped}" not found in allowed list.`);
            //     showToast("Please select a valid option from the list.");

            //     let dummyTokens = [...tokens];
            //     let lastIndex = dummyTokens.length - 1;
            //     let lastTokenValue = dummyTokens[lastIndex];

            //     dummyTokens[lastIndex] = "";

            //     input.value = dummyTokens.join(" ");

            //     result = [...staticValues];
            // }

            if (staticValues.length > 0 && result.length === 0 && partialTyped.length > 0) {
                const currentInput = inputText;


                clearTimeout(window._staticValidationTimer);

                window._staticValidationTimer = setTimeout(() => {
                    if (input.value !== currentInput) return;

                    const latestTokens = getTokens(input.value);
                    const latestPartial = cleanString(latestTokens[latestTokens.length - 1] || "");
                    const isValid = staticValues.some(val => val.toLowerCase() === latestPartial.toLowerCase());

                    if (!isValid) {
                        showToast("Please select a valid option from the list.", 2000, false);
                        let dummyTokens = [...tokens];
                        dummyTokens[dummyTokens.length - 1] = "";
                        input.value = dummyTokens.join(" ");
                        handleInput();
                    }
                }, 400);
            }

            if (groupKey.toLowerCase() === "create" && tokens.length === 3) {
                result.unshift(popOption);
                result.unshift(goOption);
                filteredObjects.unshift(popOption);
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
                    // return tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    const resolved = tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    return resolved.value;
                });
                paramValue = values.join('$#$');
            }




            if (activePrompt.extraParams) {


                if (!paramValue || paramValue.trim() === "") {
                    console.log("Skipping extraParams � dependency not resolved yet");

                } else {
                    const extraSource = activePrompt.extraParams.toLowerCase();

                    const extraKey = `${extraSource}_${paramValue}`.toLowerCase();


                    if (!axDatasourceObj[extraKey]) {

                        console.log(`Fetching Hidden Param Source: ${extraSource}`);
                        if (tokens[1].toLowerCase() === "inbox") {
                            return [goOption];
                        }
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

                        ///addedby(t)
                        if (groupKey.toLowerCase() === "view" && tokens.length >= 3) {
                            filteredObjects = [goOption, popOption];
                            return [goOption, popOption];
                        }
                        return [];
                    }

                }

            }


            let apiSourceName = realSource.toLowerCase();
            if (apiSourceName.toLowerCase() === "axi_analyticslist") {
                paramValue = window.mainUserName;
                // apiSourceName = "axi_analyticslistnew"; 
            }

            else if (realSource.toLowerCase() === "axi_structlist") {
                paramValue = processExtraParams(tokens, commandConfig);
            }

            else if (realSource.toLowerCase() === "axi_structmetalist") {
                paramValue = processExtraParams(tokens, commandConfig);
            }

            else if (realSource.toLowerCase() === "axi_viewlist") {
                paramValue = processExtraParams(tokens, commandConfig);
            }
            else if (realSource.toLowerCase() === "axi_getstructsdata") {

                if (detectedType?.toLowerCase() === "iview" || detectedType?.toLowerCase() === "page") {
                    filteredObjects = [goOption, popOption];
                    return [goOption, popOption];
                }
                let staticParams = "";
                if (tokens.length == 3) {
                    staticParams = "2";
                }
                else {
                    staticParams = "2,3";
                }

                const indices = staticParams.toString().split(',');
                const values = indices.map(idx => {
                    const logicalWordPos = parseInt(idx.trim());
                    const depTokenIndex = logicalWordPos - 1;
                    const depToken = cleanString(tokens[depTokenIndex] || "");
                    // return tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    const resolved = tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    return resolved.value;
                });

                paramValue = values.join('$#$');

                //if (tokens.length == 3) {
                //    const depToken = cleanString(tokens[1] || "");
                //    paramValue = tryResolveToken(1, depToken, commandConfig, false);
                //}
                //else {
                //    const depToken = cleanString(tokens[1] || "");
                //    paramValue = tryResolveToken(1, depToken, commandConfig, false);

                //    const fieldName = cleanString(tokens[2] || "");
                //    paramValue += "$#$"+ tryResolveToken(2, fieldName, commandConfig, false);
                //}



                let struct_name = paramValue;

                if (paramValue && paramValue.includes("$#$")) {
                    const struct_split = paramValue.split("$#$");
                    struct_name = struct_split[0];
                }
                //const extraPromptSource = "axi_keyfieldList".toLowerCase();
                //const extraSourceKey = `${extraPromptSource}_${struct_name}`.toLowerCase();


                //const extraList = axDatasourceObj[extraSourceKey];

                //if (!extraList) {
                //    loadList(extraPromptSource, struct_name);
                //}

                paramValue = processParamforEditndView(tokens, commandConfig, paramValue, tokens.length);
            }


            const sourceKey = (paramValue ? `${apiSourceName}_${paramValue}` : apiSourceName).toLowerCase();

            if (!axDatasourceObj[sourceKey]) {
                const hasValidParams = !activePrompt.promptParams || (paramValue && paramValue.replace(/,/g, '').trim().length > 0);

                if (apiSourceName === "axi_dummy" || apiSourceName === "axi_dummylist") {
                    if (groupKey.toLowerCase() === "sdk" && tokens.length > 2) {
                        filteredObjects = [goOption];
                        return [goOption]
                    }

                    if (groupKey.toLowerCase() === "configure" && tokens.length > 2) {
                        if (tokens[1] && cleanString(tokens[1]).toLowerCase() === "responsibilities") {
                            filteredObjects = [goOption];
                            return [goOption];
                        }
                        filteredObjects = [goOption, popOption];
                        return [goOption, popOption];
                    }

                    if ((groupKey.toLowerCase() === "view" || groupKey.toLowerCase() === "go") && tokens.length == 3) {
                        if (tokens[1] && tokens[1].toLowerCase() === "inbox") {
                            filteredObjects = [goOption];
                            return [goOption];
                        }
                        filteredObjects = [goOption, popOption]
                        return [goOption, popOption];
                    }
                    return [];
                }
                if (hasValidParams) {
                    if (tokens[1] && tokens[1].toLowerCase() === "inbox") {
                        filteredObjects = [goOption];
                        return [goOption];
                    }
                    loadList(apiSourceName, paramValue);
                    console.log(axDatasourceObj);
                    // if (realSource.toLowerCase() === "axi_dummy") {
                    //     if (groupKey.toLowerCase() === "open" && tokens.length > 2) {
                    //         filteredObjects = [goOption];
                    //         return [goOption]
                    //     }
                    //     return [];
                    // }
                    return [`Fetching Data...`];
                }
                return ["Waiting for input..."];
            }

            // Filter Cache
            const dataList = axDatasourceObj[sourceKey];
            let filtered = dataList.filter(item => {
                const display = item.displaydata || item.caption || item.name || "";
                return display.toLowerCase().includes(partialTyped.toLowerCase());
            });

            //filteredObjects = filtered;


            ///added to restrict user by typing only the listed values from the list(if they type other than that we prompt them again with the default list added - 18-3-2026(t))
            if (dataList.length > 0 && filtered.length === 0) {

                console.warn(`[Validation] Invalid input detected: "${partialTyped}" not found in allowed list.`);
                showToast("Please select a valid option from the list.");

                let dummyTokens = [...tokens];
                let lastIndex = dummyTokens.length - 1;


                dummyTokens[lastIndex] = "";

                input.value = dummyTokens.join(" ");

                filtered = [...dataList];
            }


            //let resultList = filtered.map(item => {
            //    if (item.createallowed === 'F') return;
            //    return item.displaydata || item.caption || item.name || item.fname || item.keyfield
            //});

            // let resultList = filtered.map(item => {
            //     let key = groupKey?.toLowerCase();

            //     if (key === 'create') {
            //         if (item.createallowed === 'F') return;
            //     }
            //     else if (key === 'view') {
            //         if (item.viewallowed === 'F') return;
            //     }


            //     return item.displaydata || item.caption || item.name || item.fname || item.keyfield;

            // });

            const key = groupKey?.toLowerCase();
            const validItems = filtered.filter(item => {
                const hasPerms = item?.viewallowed !== undefined || item?.createallowed !== undefined || item?.keyfieldforedit !== undefined;
                if (!hasPerms) return true;
                if (key === "create" || key === "edit") {
                    return isActionAllowed(item, key);
                }
                if (key === "view") {
                    return isViewAllowed(item);
                }
                if (key === "source") {
                    const stype = (item.stype || item.STYPE || "").toLowerCase().trim();
                    return ["t", "tstruct", "i", "iview", "ads"].includes(stype);
                }
                if (isEmpty(item?.displaydata) && isEmpty(item?.caption) && isEmpty(item?.name)) return false;
                return true;
            });

            filteredObjects = validItems;

            let resultList = validItems.map(item => [
                item.displaydata ||
                item.caption ||
                item.name ||
                item.fname ||
                item.keyfield
            ].find(val => val !== null && val !== undefined && (typeof val === "string" ? val.trim() !== "" : true))
            ).filter(val => val !== undefined);

            if ((groupKey.toLowerCase() === "view" || groupKey.toLowerCase() === "go") && tokens.length === 3) {
                if (tokens[1] && tokens[1].toLowerCase() === "inbox") {
                    resultList.unshift(goOption);
                    filteredObjects.unshift(goOption);
                } else {
                    resultList.unshift(goOption, popOption);
                    filteredObjects.unshift(goOption, popOption);
                }
            }

            //otherthan keyfield and userpermissionlisting it will work for all tokens which length is eqaul to 3(ex : peg)
            else if ((groupKey.toLowerCase() === "configure") && tokens.length === 3 && tokens[1].toLowerCase() !== "keyfield" && tokens[1].replace(/"/g, '').toLowerCase().trim() !== "user permissions" && tokens[1].replace(/"/g, '').toLowerCase().trim() !== "role permissions" && tokens[1].replace(/"/g, '').toLowerCase().trim() !== "responsibilities") {
                resultList.unshift(goOption, popOption);
                filteredObjects.unshift(goOption, popOption);
            }

            else if ((groupKey.toLowerCase() === "analyse") && tokens.length === 3) {
                resultList.unshift(goOption);
                filteredObjects.unshift(goOption);
            }
            //else if (groupKey.toLowerCase() === "open" && (tokens[1]?.toLowerCase() === "api"
            else if (groupKey.toLowerCase() === "sdk" && (cleanCommandToken(tokens[1])?.toLowerCase().trim() === "api plugin"
                || cleanCommandToken(tokens[1])?.toLowerCase().trim() == "axpert job" || cleanCommandToken(tokens[1])?.toLowerCase().trim() == "language"
                || cleanCommandToken(tokens[1])?.toLowerCase().trim() == "custom data type" || cleanCommandToken(tokens[1])?.toLowerCase().trim() == "email definition"
                || cleanCommandToken(tokens[1])?.toLowerCase().trim() == "table field descriptor" || cleanCommandToken(tokens[1])?.toLowerCase().trim() == "out bound queue"
                || cleanCommandToken(tokens[1])?.toLowerCase().trim() == "in bound queue" || cleanCommandToken(tokens[1])?.toLowerCase() === "axpert data sources")) {
                resultList.unshift(goOption);
                filteredObjects.unshift(goOption);
            }


            //else if (groupKey.toLowerCase() === "create" && tokens.length == 3) {
            //    resultList.unshift(goOption);
            //    filteredObjects.unshift(goOption);
            //}

            //else if (groupKey.toLowerCase() === "edit" && tokens.length > 4) {
            //    resultList.unshift(popOption);
            //    resultList.unshift(goOption);
            //    filteredObjects.unshift(popOption);
            //    filteredObjects.unshift(goOption);
            //}


            ///added this for sinlge fieldname and fieldvalue(T)
            if (groupKey.toLowerCase() === "edit" && tokens.length > 4) {
                //resultList.unshift(goOption);
                //resultList.unshift(popOption);
                //filteredObjects.unshift(goOption);
                //filteredObjects.unshift(popOption);
                filteredObjects = [goOption, popOption]
                return [goOption, popOption];
            }

            const structName = getCurrentStructName();
            const accessPermissions = getAccessPermissions();

            if ((groupKey.toLowerCase() === "view") && tokens.length <= 2 && structName !== null && accessPermissions?.buildAccess) {
                resultList.unshift("Source");
                // resultList.unshift(goOption);
                filteredObjects.unshift("Source");
                updateDynamicHintFromPrompt({ prompt: "Ready to Run" });
                // filteredObjects.unshift(goOption);
            }

            return resultList;
        }

        ///added this for sinlge fieldname and fieldvalue(T)
        if (groupKey.toLowerCase() === "edit" && tokens.length > 4) {
            //resultList.unshift(goOption);
            //resultList.unshift(popOption);
            //filteredObjects.unshift(goOption);
            //filteredObjects.unshift(popOption);
            updateDynamicHintFromPrompt({ prompt: "Ready to Run" })
            filteredObjects = [goOption, popOption]
            return [goOption, popOption];
        }


        return [];
    }

    function processParamforEditndView(tokens, commandConfig, paramValue, position) {

        const struct_prompt = commandConfig.prompts[0];
        const struct_source = struct_prompt.promptSource;

        // extra params
        const struct_paramValue = processExtraParams(tokens, commandConfig);

        const struct_sourceKey = (struct_paramValue ? `${struct_source}_${struct_paramValue}` : struct_source).toLowerCase();

        // load datasource if not exists
        if (!axDatasourceObj[struct_sourceKey]) {

            const struct_hasParams = !struct_prompt.promptParams ||
                (struct_paramValue && struct_paramValue.replace(/,/g, '').trim().length > 0);

            if (struct_hasParams) {
                loadList(struct_source, struct_paramValue);
                return [`Loading ${struct_source}...`];
            }
        }

        const struct_dataList = axDatasourceObj[struct_sourceKey];
        //if (!struct_dataList) return null;

        // find struct row

        let struct_name = paramValue;

        if (paramValue && paramValue.includes("$#$")) {
            const struct_split = paramValue.split("$#$");
            struct_name = struct_split[0];
        }

        let preferredType = getResolvedParamType(1);
        if (preferredType) {
            preferredType = preferredType.toLowerCase();
            if (preferredType === "tstruct") preferredType = "t";
            if (preferredType === "iview") preferredType = "i";
        }

        const struct_row = struct_dataList.find(r => 
            r.name === struct_name && 
            (!preferredType || (r.stype || "").toLowerCase() === preferredType)
        ) || struct_dataList.find(r => r.name === struct_name);

        if (!struct_row) {
            console.log("The give Form is not in the ads " + struct_source + " list");
            showToast("The Given Transid is not in the list");
            return [];
        }

        // values from structmetalist
        //const struct_dimension = struct_row.dimension;
        //const struct_permission = struct_row.permission;
        //const struct_keyfield = struct_row.keyfield;
        //const struct_primarytable = struct_row.primarytable;
        //const struct_transid = struct_row.name;

        // selected field logic
        let struct_selectedfield = "";
        if (position === 3) {
            struct_selectedfield = "0";
        } else {
            const struct_split = paramValue ? paramValue.split("$#$") : [];
            struct_selectedfield = struct_split.length > 1 ? struct_split[1] : "";
        }

        //// already available globals
        //const struct_username = mainUserName;
        //const struct_userrole = AxUserRoles;

        const struct_globalvars = getGlobalVar(allGloblVars, "axg_applicable_dimensions") || "NA";
        //const struct_globalvars = prepareKeyValueString(allGloblVars);
        //const struct_globalvars = allGloblVars

        const promptIndex = position === 3 ? 1 : 2;
        const extraParamsStr = commandConfig.prompts[promptIndex]?.extraParams || "";

        const paramsArray = extraParamsStr.split(",");

        let finalParamsArray = [];

        //default first param
        //finalParamsArray.push(commandConfig.commandGroup)

        for (let i = 0; i < paramsArray.length; i++) {

            let param = paramsArray[i].trim().toLowerCase();
            let value = "";


            if (param === ":username") {
                value = mainUserName;
            }
            else if (param === ":userrole") {
                value = AxUserRoles;
            }
            else if (param === ":cmd") {
                value = commandConfig.commandGroup;
            }
            else if (param === ":transid") {
                value = struct_row.name;;
            }
            else if (param === ":selectedfield") {
                value = struct_selectedfield;
            }
            else if (param === ":dimension") {
                value = struct_row.dimension;
            }
            else if (param === ":permission") {
                value = struct_row.permission;
            }
            else if (param === ":keyfield") {
                value = struct_row.keyfield;
            }
            else if (param === ":primarytable") {
                value = struct_row.primarytable;
            }
            else if (param === ":globalvars") {
                value = "NA";
            }


            // fallback NA
            if (value === null || value === undefined || value === "") {
                value = "NA";
            }

            finalParamsArray.push(value);
        }

        const struct_finalParams = finalParamsArray.join("$#$");
        //const struct_finalParams = [
        //    commandConfig.commandGroup,
        //    struct_username,
        //    struct_userrole,
        //    struct_transid,
        //    struct_selectedfield,
        //    struct_dimension,
        //    struct_permission,
        //    struct_keyfield,
        //    struct_primarytable,
        //    struct_globalvars
        //]
        //    .map(v => (v === null || v === undefined || v === "") ? "NA" : v)
        //    .join("$#$");

        return struct_finalParams;
    }

    function getGlobalVar(struct_allGlobalVars, struct_keyName) {

        if (!struct_allGlobalVars || !Array.isArray(struct_allGlobalVars.globalVars)) {
            return null;
        }

        for (let struct_var of struct_allGlobalVars.globalVars) {
            if (struct_var.hasOwnProperty(struct_keyName)) {
                return struct_var[struct_keyName];
            }
        }

        return null;
    }

    // function processExtraParams(tokens,commandConfig) {

    //     let paramValue = "";

    //     let iviewBoolCheck = false;

    //     let adsBoolCheck = false;

    //     let pageBoolCheck = false;

    //     if (tokens.length >= 2) {
    //         if (tokens[1].toLowerCase() == "iview" && commandConfig.commandGroup.toLowerCase() == "open") {
    //             iviewBoolCheck = true;
    //         }
    //         else if (tokens[1].toLowerCase() == "ads" && commandConfig.commandGroup.toLowerCase() == "open") {
    //             adsBoolCheck = true;
    //         }
    //         else if (tokens[1].toLowerCase() == "page" && commandConfig.commandGroup.toLowerCase() == "open") {
    //             pageBoolCheck = true;
    //         }
    //     }
    //     try {

    //         let extraParams;
    //         if (commandConfig.commandGroup.toLowerCase() == "configure") {
    //             extraParams = commandConfig?.prompts?.[1]?.extraParams;
    //         }
    //         else if (commandConfig.commandGroup.toLowerCase() == "open") {
    //             extraParams = commandConfig?.prompts?.[1]?.extraParams;
    //         }
    //         else {
    //             extraParams = commandConfig?.prompts?.[0]?.extraParams;
    //         }

    //         if (!extraParams) return paramValue;

    //         let paramsArray = extraParams.split(",");

    //         paramsArray.forEach(param => {

    //             param = param.trim().toLowerCase();
    //             let value = "";

    //             if (param === ":username") {
    //                 value = mainUserName;
    //             }
    //             else if (param === ":userroles") {
    //                 value = AxUserRoles;
    //             }
    //             else if (param === ":userresp") {
    //                 value = userResp;
    //             }
    //             else if (param === ":mode") {
    //                 if (commandConfig.commandGroup.toLowerCase() === "open") {
    //                     value = "dev";
    //                 }
    //                 else if (commandConfig.commandGroup.toLowerCase() === "view") {
    //                     value = "all"
    //                 }
    //                 else value = "run";
    //             }
    //             else if (param === ":structtype") {
    //                 if (commandConfig.commandGroup.toLowerCase() === "view") {
    //                     value = "all"
    //                 }
    //                 else {

    //                     value = iviewBoolCheck ? "i" : adsBoolCheck ? "ads" : pageBoolCheck ? "p" : "t";
    //                 }
    //             }

    //             if (value) {
    //                 if (paramValue === "") {
    //                     paramValue = value;
    //                 } else {
    //                     paramValue += "$#$" + value;
    //                 }
    //             }

    //         });

    //     } catch (err) {
    //         console.log("Error in processExtraParams:", err);
    //     }

    //     return paramValue;
    // }


    function processExtraParams(tokens, commandConfig) {
        let paramValue = "";

        let iviewBoolCheck = false;

        if (tokens.length >= 2) {
            if (tokens[1].toLowerCase() == "iview" && commandConfig.commandGroup.toLowerCase() == "sdk") {
                iviewBoolCheck = true;
            }
        }
        try {
            let extraParams;
            if (commandConfig.commandGroup.toLowerCase() == "configure") {
                extraParams = commandConfig?.prompts?.[1]?.extraParams;
            } else if (commandConfig.commandGroup.toLowerCase() == "sdk") {
                extraParams = commandConfig?.prompts?.[1]?.extraParams;
            } else {
                extraParams = commandConfig?.prompts?.[0]?.extraParams;
            }

            if (!extraParams) return paramValue;

            let paramsArray = extraParams.split(",");

            paramsArray.forEach((param) => {
                param = param.trim().toLowerCase();
                let value = "";

                if (param === ":username") {
                    value = mainUserName;
                } else if (param === ":userroles") {
                    value = AxUserRoles;
                } else if (param === ":userresp") {
                    value = userResp;
                } else if (param === ":mode") {
                    if (commandConfig.commandGroup.toLowerCase() === "sdk") {
                        value = "dev";
                    } else if (commandConfig.commandGroup.toLowerCase() === "view") {
                        value = "all";
                    } else value = "run";
                }

                else if (param === ":structtype") {
                    if (commandConfig.commandGroup.toLowerCase() === "view") {
                        value = "all";
                    } else if (commandConfig.commandGroup.toLowerCase() == "sdk" && tokens.length >= 2) {
                        let token = cleanCommandToken(tokens[1]).toLowerCase();

                        switch (token) {
                            case "iview":
                                value = "i";
                                break;
                            case "tstruct":
                                value = "t";
                                break;
                            case "page":
                                value = "p";
                                break;
                            case "axpert data sources":
                                value = "ads";
                                break;
                            default:
                                value = "t"; //all
                        }
                    }
                    else if (commandConfig.commandGroup.toLowerCase() == "analyse") {
                        value = 'analyse'
                    }
                    else value = iviewBoolCheck ? "i" : "t";
                }
                if (value) {
                    if (paramValue === "") {
                        paramValue = value;
                    } else {
                        paramValue += "$#$" + value;
                    }
                }
            });
        } catch (err) {
            console.log("Error in processExtraParams:", err);
        }

        return paramValue;
    }


    function getUnswappedIndex(tokenIndex) {
        if (!input) return tokenIndex;
        const tokens = getTokens(input.value, false);
        if (tokens.length >= 2) {
            const first = cleanString(tokens[0]).toLowerCase();
            const second = cleanString(tokens[1]).toLowerCase();
            if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                if (tokenIndex === 0) return 1;
                if (tokenIndex === 1) return 0;
            }
        }
        return tokenIndex;
    }

    function getResolvedParamType(tokenIndex) {
        const rawIndex = getUnswappedIndex(tokenIndex);
        return resolvedParamType?.[rawIndex];
    }

    function getCommandConfig(groupKey) {
        if (!groupKey || !commands) return null;
        const lowKey = groupKey.toLowerCase();
        if (commands[groupKey]) return commands[groupKey];
        const matchedKey = Object.keys(commands).find(k => k.toLowerCase() === lowKey);
        return matchedKey ? commands[matchedKey] : null;
    }

    function tryResolveToken(tokenIndex, tokenText, commandConfig, forceResolve = false) {

        tokenText = cleanString(tokenText);
        if (!tokenText) return { value: "", type: "" };

        const rawIndex = getUnswappedIndex(tokenIndex);

        if (resolvedParams[rawIndex] && !forceResolve) {
            const val = resolvedParams[rawIndex];
            const lowerVal = val.toLowerCase();
            const isVerb = ["view", "create", "edit", "run", "refresh", "sdk", "configure", "publish", "help", "save", "go", "pop", "version", "source"].includes(lowerVal);
            if (!(tokenIndex === 1 && isVerb)) {
                return {
                    value: val,
                    type: resolvedParamType?.[rawIndex] || ""
                };
            }
        }

        if (tokenIndex === 1 && !forceResolve) {
            const matches = getTargetEntityMatches(tokenText);

            if (matches.length > 0) {
                let found = matches[0];
                if (matches.length > 1) {
                    const currentTokens = getTokens(input.value);
                    const cmdGroup = currentTokens[0]?.toLowerCase();
                    if (cmdGroup === "create" || cmdGroup === "edit") {
                        const typeMatched = matches.find(item => {
                            const type = (item.stype || "").toLowerCase();
                            return type === "t" || type === "tstruct";
                        });
                        if (typeMatched) found = typeMatched;
                    } else if (cmdGroup === "view") {
                        const typeMatched = matches.find(item => {
                            const type = (item.stype || "").toLowerCase();
                            return type !== "t" && type !== "tstruct";
                        });
                        if (typeMatched) found = typeMatched;
                    }
                }

                let foundStype = found.stype || "";
                if (!foundStype && isInboxStructure(found)) {
                    foundStype = "inbox";
                }
                resolvedParams[rawIndex] = found.name;
                if (foundStype) resolvedParamType[rawIndex] = foundStype;
                return {
                    value: found.name,
                    type: foundStype
                };
            }
        }
        // if (!tokenText && !forceResolve) return "";
        // if (!commandConfig) return tokenText;
        if (!commandConfig) return {
            value: tokenText,
            type: ""
        };


        const currentTokens = getTokens(input.value);


        const promptInfo = getActivePromptInfo(commandConfig, getTokens(input.value), tokenIndex);
        // if (!promptInfo) return tokenText;
        if (!promptInfo) return {
            value: tokenText,
            type: ""
        };

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
                    // return tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    const resolved = tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    return resolved.value;
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
                // apiName = "axi_analyticslistnew"; 
                paramValue = window.mainUserName;
            }

            else if (realSource.toLowerCase() === "axi_structlist") {
                paramValue = processExtraParams(currentTokens, commandConfig);
            }

            else if (realSource.toLowerCase() === "axi_structmetalist") {
                paramValue = processExtraParams(currentTokens, commandConfig);
            }

            else if (realSource.toLowerCase() === "axi_viewlist") {
                paramValue = processExtraParams(currentTokens, commandConfig);
            }
            else if (realSource.toLowerCase() === "axi_getstructsdata") {

                let staticParams = "2";
                //if (currentTokens.length < 3) {
                //    staticParams = "2";
                //}
                //else {
                //    staticParams = "2,3";
                //}

                const indices = staticParams.toString().split(',');
                const values = indices.map(idx => {
                    const logicalWordPos = parseInt(idx.trim());
                    const depTokenIndex = logicalWordPos - 1;
                    const depToken = cleanString(currentTokens[depTokenIndex] || "");
                    // return tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    const resolved = tryResolveToken(depTokenIndex, depToken, commandConfig, true);
                    return resolved?.value;
                });
                paramValue = values.join('$#$');

                //if (currentTokens.length == 2) {
                //    const depToken = cleanString(currentTokens[1] || "");
                //    paramValue = tryResolveToken(1, depToken, commandConfig, false);
                //}
                //else {
                //    const depToken = cleanString(currentTokens[1] || "");
                //    paramValue = tryResolveToken(1, depToken, commandConfig, false);

                //    const fieldName = cleanString(currentTokens[2] || "");
                //    paramValue += "$#$"+tryResolveToken(2, fieldName, commandConfig, false);
                //}



                paramValue = processParamforEditndView(currentTokens, commandConfig, paramValue, 3)
            }

            let cacheKey = paramValue ? `${apiName}_${paramValue}` : apiName;

            const cachedList = axDatasourceObj[cacheKey.toLowerCase()];
            if (cachedList) {
                // Find matching items
                const matches = cachedList.filter(item =>
                    (cleanString(item.displaydata) || "").toLowerCase() === tokenText.toLowerCase() ||
                    (cleanString(item.caption) || "").toLowerCase() === tokenText.toLowerCase() ||
                    (cleanString(item.name) || "").toLowerCase() === tokenText.toLowerCase()
                );

                if (matches.length > 0) {
                    let found = matches[0];
                    let preferredType = resolvedParamType?.[tokenIndex];
                    const cmdGroup = currentTokens[0]?.toLowerCase();
                    if ((cmdGroup === "edit" || cmdGroup === "create") && tokenIndex === 1) {
                        preferredType = "t";
                    }
                    if (cmdGroup === "view" && tokenIndex === 1 && !preferredType) {
                        const typeMatched = matches.find(item => {
                            const type = (item.stype || "").toLowerCase();
                            return type !== "t" && type !== "tstruct";
                        });
                        if (typeMatched) found = typeMatched;
                    }
                    if (preferredType && matches.length > 1) {
                        const typeMatched = matches.find(item => (item.stype || "").toLowerCase() === preferredType.toLowerCase());
                        if (typeMatched) {
                            found = typeMatched;
                        }
                    }

                    let real = found.name || found.sqlname || found.displaydata;

                    if (real.includes("(") && real.includes(")")) {
                        const match = real.match(/\(([^)]+)\)/);

                        real = match ? match[1] : real;

                    }

                    resolvedParams[rawIndex] = real;
                    if (found?.stype) resolvedParamType[rawIndex] = found?.stype;
                    // return real;
                    return {
                        value: real,
                        type: found?.stype ?? ""
                    };
                }
            }
        }

        // return tokenText;
        return {
            value: tokenText,
            type: ""
        };
    }


    /* ===============================
       RENDER & APPLY
    =============================== */
    function render() {

        console.log("Render called");
        list.innerHTML = "";

        const currentInput = input.value;
        const currentInputTokens = getTokens(currentInput.trim());

        // Determine the current suggestion token position/level (0-based)
        let suggestionTokenIndex = currentInputTokens.length;
        if (currentInputTokens.length > 0 && !currentInput.endsWith(" ")) {
            suggestionTokenIndex = currentInputTokens.length - 1;
        }
        const shouldCapitalize = (suggestionTokenIndex <= 1);

        const capitalizeFirstLetter = (str) => {
            if (!str) return str;
            return str.charAt(0).toUpperCase() + str.slice(1);
        };

        const isInitialCommandStage = currentInputTokens.length === 0 || (currentInputTokens.length === 1 && !currentInput.endsWith(" "));

        const commandIcons = {
            "create": "add_circle_outline",
            "edit": "edit_note",
            "view": "visibility",
            "configure": "settings_suggest",
            "sdk": "open_in_new",
            "upload": "upload_file",
            "download": "download",
            "run": "play_arrow",
            "analyse": "bar_chart",
            "ai": "smart_toy",
            "connect": "link",
            "ask": "question_answer",
            "end": "stop",
            "editprompt": "edit",
            "analyze": "analytics",
            "help": "help_outline",
            "version": "info"
        };




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

        if (items.length > 0 && isSystemMessage(items[0])) {
            activeIndex = -1;
        } else {
            const currentTokens = getTokens(input.value.trim());
            const isUserTyping = currentTokens.length > 1 && !input.value.endsWith(" ");
            // const hasGoOption = validItems.some(item => typeof item === 'object' && item.name === "GO_ACTION");
            // const hasSaveOption = validItems.some(item => typeof item === 'object' && item.name === "Save_ACTION");

            // if (hasGoOption && hasSaveOption) {
            //     activeIndex = 2; 
            // } else if (hasGoOption || hasSaveOption) {
            //     activeIndex = 1; 
            // } else {
            //     activeIndex = 0; 
            // }
            // activeIndex = 0;
            //           if (isUserTyping) {
            //     const firstNonActionIndex = validItems.findIndex(

            if (isUserTyping) {
                const lastToken = cleanString(
                    currentTokens[currentTokens.length - 1]
                ).toLowerCase();

                const matchedIndex = validItems.findIndex(item => {
                    const text =
                        typeof item === "string"
                            ? item.toLowerCase()
                            : (item.displaydata || item.name || "").toLowerCase();

                    return text.startsWith(lastToken);
                });

                activeIndex = matchedIndex !== -1 ? matchedIndex : 0;
            } else {
                activeIndex = 0;
            }

        }

        const metadataParams = getStructParam();
        const structKey = "axi_structmetalist_" + metadataParams.toLowerCase();
        const isCmdsLoading = isCommandsLoading || !commands;
        const isStructsLoading = activeFetches.has(structKey) || !axDatasourceObj[structKey];

        const isAnyLoadingItem = items.some(item => {
            const text = typeof item === "string" ? item : (item?.displaydata || "");
            return typeof text === "string" && text.startsWith("Loading");
        });
        const isAnyFetchActive = activeFetches.size > 0 || isCmdsLoading || isStructsLoading || isAnyLoadingItem;

        const showInitialWrapper = isInitialCommandStage && (validItems.length > 0 || isCmdsLoading || isStructsLoading) && !isSystemMessage(validItems[0]);

        if (showInitialWrapper) {
            list.classList.add("My-Command-Wrapper");
            if (favouritesCard) favouritesCard.style.display = "none";
            if (commandHeader) commandHeader.style.display = "flex";
        } else {
            list.classList.remove("My-Command-Wrapper");
            if (favouritesCard) favouritesCard.style.display = "none";
            if (commandHeader) commandHeader.style.display = "none";
        }

        if (validItems.length === 0 && isAnyFetchActive) {
            if (!showInitialWrapper) {
                list.innerHTML = "";
                const li = document.createElement("li");
                li.className = "axi-suggestion-loading d-flex align-items-center justify-content-center py-3 px-4 text-muted gap-2";
                li.style.fontSize = "13px";
                li.style.textAlign = "center";
                li.innerHTML = `
                    <span class="spinner-border h-15px w-15px align-middle text-gray-400" role="status" aria-hidden="true" style="border-width: 2px; margin-right: 6px;"></span>
                    <span>Loading options...</span>
                `;
                list.appendChild(li);
                list.style.display = "block";
                return;
            }
        }

        const showNoData = validItems.length === 0 && !isAnyFetchActive;
        if (showNoData) {
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

        function createSuggestionLi(item, i) {
            const li = document.createElement("li");
            const text = typeof item === "string" ? item : item.displaydata;
            li.className = "axi-suggestion";

            const displayText = (shouldCapitalize && !(typeof item === 'object' && item.isExecutable)) 
                ? capitalizeFirstLetter(text) 
                : text;

            if (typeof item === 'object' && item.isExecutable) {
                li.style.fontWeight = "bold";
                li.style.color = "#22c55e";
                li.style.borderBottom = "1px solid #eee";
                li.textContent = displayText;

            } else if (isInitialCommandStage && getCommandConfig(text)) {
                const iconName = (commandIcons && commandIcons[text.toLowerCase()]) ? commandIcons[text.toLowerCase()] : "info";

                li.innerHTML = `
               
                <div class="d-flex align-items-center">
                        <div class="symbol symbol-25px mainIcon">
                            <div class="symbol-label cardbg-inverse-1">
                                <div class="items-icon">
                                    <span class="material-icons material-icons-style material-icons-2">${iconName}</span>
                                </div>
                            </div>
                        </div>
                        <span class="command-text">${displayText}</span>
                    </div>`;
            } else {
                li.textContent = displayText;
            }

            if (i === activeIndex) {
                li.classList.add("active");
            }

            li.addEventListener("mousedown", e => {
                e.preventDefault();
                let objectIndex = validItems.findIndex(obj => {
                    if (obj === item) return true;
                    const itemText = typeof item === "string" ? item : (item?.displaydata || item?.name || "");
                    const objText = typeof obj === "string" ? obj : (obj?.displaydata || obj?.name || "");
                    return itemText && objText && itemText.toLowerCase() === objText.toLowerCase();
                });
                apply(objectIndex !== -1 ? objectIndex : i);
            });

            return li;
        }

        const fragment = document.createDocumentFragment();

        if (isInitialCommandStage && (validItems.length > 0 || isCmdsLoading || isStructsLoading) && !isSystemMessage(validItems[0])) {
            const verbs = [];
            const structures = [];
            validItems.forEach((item, i) => {
                const text = typeof item === "string" ? item : item.displaydata;
                if (getCommandConfig(text)) {
                    verbs.push({ item, index: i });
                } else {
                    structures.push({ item, index: i });
                }
            });

            const createLoadingElement = (text) => {
                const div = document.createElement("div");
                div.className = "d-flex align-items-center justify-content-center py-2 px-5 text-muted gap-2";
                div.style.fontSize = "13px";
                div.innerHTML = `
                    <span class="spinner-border h-15px w-15px align-middle text-gray-400" role="status" aria-hidden="true" style="border-width: 2px; margin-right: 6px;"></span>
                    <span>${text}</span>
                `;
                return div;
            };

            if (verbs.length > 0 || isCmdsLoading) {
                const gridContainer = document.createElement("div");
                gridContainer.className = "axi-verbs-grid-container";
                
                const header = document.createElement("div");
                header.className = "axi-grid-header";
                header.textContent = "Commands";
                gridContainer.appendChild(header);

                if (isCmdsLoading) {
                    gridContainer.appendChild(createLoadingElement("Loading commands..."));
                } else {
                    const grid = document.createElement("div");
                    grid.className = "axi-verbs-grid";

                    verbs.forEach(({ item, index }) => {
                        const li = createSuggestionLi(item, index);
                        grid.appendChild(li);
                    });

                    gridContainer.appendChild(grid);
                }

                fragment.appendChild(gridContainer);
            }

            if (structures.length > 0 || isStructsLoading) {
                const listContainer = document.createElement("div");
                listContainer.className = "axi-structures-list-container";

                const header = document.createElement("div");
                header.className = "axi-grid-header";
                header.textContent = "Structures";
                listContainer.appendChild(header);

                if (isStructsLoading) {
                    listContainer.appendChild(createLoadingElement("Loading structures..."));
                } else {
                    structures.forEach(({ item, index }) => {
                        const li = createSuggestionLi(item, index);
                        listContainer.appendChild(li);
                    });
                }

                fragment.appendChild(listContainer);
            }
        } else {
            validItems.forEach((item, i) => {
                const li = createSuggestionLi(item, i);
                fragment.appendChild(li);
            });
        }

        list.appendChild(fragment);

        if (list.classList.contains("axi-grid-layout")) {
            list.style.display = "flex";
        } else {
            list.style.display = "block";


        }

        if (megaDropdown) {
            megaDropdown.style.display = "flex";
        } else {
            list.style.display = "block";
        }



    }

    function hide() {
        if (megaDropdown) {
            megaDropdown.style.display = "none";
        }
        list.style.display = "none";
        list.classList.remove("My-Command-Wrapper");

        if (favouritesCard) {
            favouritesCard.style.display = "none";
        }
        const suggCard = list.closest(".card");
        if (suggCard) {
            suggCard.style.display = "flex";
        }

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
        if (document.querySelector(".AXI-Sec")?.classList.contains("axi-tour-active")) {
            return;
        }

        const validItems = items.filter(item => {
            if (!item) return false;
            if (typeof item === "string") {
                return (!item.startsWith("Loading") && item !== "No Data" && item.trim() !== "");
            }
            if (typeof item === "object") {
                const text = item.displaydata || item.name || "";
                return (typeof text === "string" && !text.startsWith("Loading") && text !== "No Data" && text.trim() !== "");
            }
            return false;
        });

        const targetList = (validItems && validItems.length > 0 && index >= 0 && index < validItems.length) ? validItems : items;
        const selectedItem = targetList[index];

        if (!selectedItem || isSystemMessage(selectedItem)) return;


        const currentInput = input.value;
        const tokens = getTokens(currentInput, false);

        let checkTokens = [...tokens];
        if (checkTokens.length >= 2) {
            const first = cleanString(checkTokens[0]).toLowerCase();
            const second = cleanString(checkTokens[1]).toLowerCase();
            if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                const temp = checkTokens[0];
                checkTokens[0] = checkTokens[1];
                checkTokens[1] = temp;
            }
        }

        const saveGroupKeyCheck = cleanString(checkTokens[0]);
        const saveCommandConfig = getCommandConfig(saveGroupKeyCheck);

        if (typeof selectedItem === 'object' && selectedItem.isExecutable && selectedItem.name === "GO_ACTION") {
            console.log("Action item selected. Executing command...");
            const rawInput = input.value.trim();
            const rawTokens = getTokens(rawInput, false);
            const firstToken = rawTokens.length > 0 ? cleanString(rawTokens[0]).toLowerCase() : "";
            let shouldRestoreInput = false;
            if (rawTokens.length === 1 && firstToken && isTargetEntity(firstToken)) {
                const entityObj = getTargetEntityObj(firstToken);
                const stype = entityObj ? (entityObj.stype || entityObj.STYPE || "").toLowerCase() : "";
                if (["i", "iview", "ads", "page", "p"].includes(stype) || isInboxStructure(entityObj)) {
                    shouldRestoreInput = true;
                }
                input.value = "view " + rawInput + " ";
            }
            hide();
            isProgrammaticExecution = true;
            try {
                executeCommandsV2();
            } finally {
                isProgrammaticExecution = false;
            }
            if (shouldRestoreInput) {
                input.value = rawInput + " ";
            }
            return;
        }
        else if (typeof selectedItem === 'object' && selectedItem.isExecutable && selectedItem.name === "Save_ACTION" && saveGroupKeyCheck.toLowerCase() === "create") {
            console.log("Save Option Selected...Submitting Data...");
            hide();
            AxisaveDataFn(createfieldnamevaluesList, setCommandTransid, "axi_nongridfieldlist", true, checkTokens, saveCommandConfig);
            resetSetCommandState();
            return;
        }
        else if (typeof selectedItem === 'object' && selectedItem.isExecutable && selectedItem.name === "Save_ACTION" && saveGroupKeyCheck.toLowerCase() === "edit") {
            console.log("Save Option Selected...Submitting Data...");
            hide();
            AxisaveDataFn(createfieldnamevaluesList, setCommandTransid, "axi_nongridfieldlist", false, checkTokens, saveCommandConfig);
            resetSetCommandState();
            return;
        }
        else if (typeof selectedItem === 'object' && selectedItem.isExecutable && selectedItem.name === "Pop_ACTION") {
            console.log("Pop Option Selected......");
            try {
                let executeTokens = tokens;
                const rawInput = input.value.trim();
                const rawTokens = getTokens(rawInput, false);
                const firstToken = rawTokens.length > 0 ? cleanString(rawTokens[0]).toLowerCase() : "";
                let shouldRestoreInput = false;
                if (rawTokens.length === 1 && firstToken && isTargetEntity(firstToken)) {
                    const entityObj = getTargetEntityObj(firstToken);
                    const stype = entityObj ? (entityObj.stype || entityObj.STYPE || "").toLowerCase() : "";
                    if (["i", "iview", "ads", "page", "p"].includes(stype)) {
                        shouldRestoreInput = true;
                    }
                    input.value = "view " + rawInput + " ";
                    executeTokens = getTokens(input.value, false);
                }
                if (executeTokens.length >= 2) {
                    hide();
                    popUpOption = true;
                    isProgrammaticExecution = true;
                    try {
                        executeCommandsV2();
                    } finally {
                        isProgrammaticExecution = false;
                    }
                    if (shouldRestoreInput) {
                        input.value = rawInput + " ";
                    }
                    return;
                }
                //else return;
            }
            catch (err) {
                popUpOption = false;

                console.log("Error in Popup:", err);

                showToast("Something went wrong. Please try again later");

                return;
            }


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

        let suggestion = typeof selectedItem === "string" ? selectedItem : (selectedItem.displaydata || selectedItem.name || "");
        let displayName = suggestion;
        let realValue = "";
        let realType = "";
        // const currentToken = tokens[targetIndex - 1]; 

        // if (currentToken?.toLowerCase() === "with" && tokens.length === 3) {
        //     isEditing = true; 

        // }

        const isViewCommand = checkTokens[0]?.toLowerCase() === "view";




        let isAdsValue = false;

        const groupKey = cleanString(checkTokens[0]);
        const commandConfig = getCommandConfig(groupKey);
        /**
         * ==== Robust Type checking for View command ===
         */
        if (isViewCommand && commands) {


            if (commandConfig && commandConfig.prompts && commandConfig.prompts[0]) {
                const viewSource = commandConfig.prompts[0].promptSource;
                const viewValues = commandConfig.prompts[0].promptValues;
                const firstToken = cleanString(checkTokens[1] || "");

                // const detectedType = getType(viewSource.toLowerCase(), firstToken, viewValues, tokens, commandConfig);
                const resolved = tryResolveToken(
                    1,
                    firstToken,
                    commandConfig,
                    false
                );

                const detectedType = getType(
                    viewSource.toLowerCase(),
                    resolved,
                    viewValues,
                    checkTokens,
                    commandConfig
                );

                if (detectedType === "ads" && targetIndex >= 3 && targetIndex % 2 !== 0) {
                    isAdsValue = true;
                }
            }
        }

        // Fix: parenthesis bug 
        // If it is a field value we will ignore the parenthesis omission 
        let isValueToken = false;
        const commandGroup = groupKey?.toLowerCase();

        if (commandGroup === "edit") {
            if (targetIndex === 3) {
                isValueToken = true;
            } else {
                const withIndex = checkTokens.findIndex(t => cleanString(t).toLowerCase() === "with");

                if (withIndex !== -1 && targetIndex > withIndex && (targetIndex - withIndex) % 2 === 0) {
                    isValueToken = true;
                }
            }
        } else if (commandGroup === "view") {
            if (targetIndex === 3) isValueToken = true;
        }



        // Get Real Value logic
        let foundObj = null;
        if (filteredObjects && index >= 0 && index < filteredObjects.length) {
            const cand = filteredObjects[index];
            const candText = typeof cand === "string" ? cand : (cand?.displaydata || cand?.name || "");
            if (candText && suggestion && candText.toLowerCase() === suggestion.toLowerCase()) {
                foundObj = cand;
            }
        }
        if (!foundObj && filteredObjects) {
            foundObj = filteredObjects.find(itemObj => {
                if (itemObj === selectedItem) return true;
                const objText = typeof itemObj === "string" ? itemObj : (itemObj?.displaydata || itemObj?.name || "");
                return objText && suggestion && objText.toLowerCase() === suggestion.toLowerCase();
            });
        }

        const checkEditObj = (checkTokens[0]?.toLowerCase() === "edit" && checkTokens[1]) ? getTargetEntityObj(checkTokens[1], "edit") : null;
        if (checkEditObj && (checkEditObj.keyfieldforedit === "F" || checkEditObj.keyfieldforedit === false)) {
            const suggText = (typeof selectedItem === "string" ? selectedItem : (selectedItem?.displaydata || selectedItem?.name || "")).toLowerCase().trim();
            const hasWithInTokens = checkTokens.some(t => cleanString(t).toLowerCase() === "with") || currentInput.toLowerCase().includes(" with");
            if (targetIndex === 1 || suggText === "with" || hasWithInTokens) {
                showToast("Key field is not configured for this form.", 3000, false);
                hide();
                return;
            }
        }

        if (foundObj && isViewCommand) {
            const caption = foundObj?.caption ? foundObj?.caption : foundObj?.displaydata;

            const type = getType(commandConfig?.prompts?.[0]?.promptSource.toLowerCase(), caption, commandConfig?.prompts?.[0]?.promptValues, checkTokens, commandConfig);

            if ((!foundObj?.name || foundObj?.name === null || foundObj?.name === undefined) && type?.toLowerCase() === "page") {
                showToast("Redirection link is not available for this page.");
                console.error("No Redirection link!");
                return;
            }
        }


        realValue = foundObj ? (foundObj.name || foundObj.sqlname || foundObj.displaydata) : suggestion;
        realType = foundObj?.stype ?? "";





        if (suggestion.includes("(") && suggestion.includes(")") && !isAdsValue && !isValueToken) {
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
        resolvedParamType[targetIndex] = realType;

        displayName = displayName.replace(/[\r\n]+/g, " ").trim();

        if (["create", "view", "edit", "source"].includes(displayName.toLowerCase())) {
            displayName = displayName.charAt(0).toUpperCase() + displayName.slice(1).toLowerCase();
        }

        // Auto-Quote if necessary
        if (displayName.includes(" ")) {
            displayName = `"${displayName}"`;
        }

        const originalToken = tokens[targetIndex];
        if (originalToken && originalToken.includes(",")) {
            const parts = originalToken.split(",");
            parts[parts.length - 1] = displayName;
            tokens[targetIndex] = parts.join(",");
        } else {
            tokens[targetIndex] = displayName;
        }

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

            if (res) {
                const resStr = (typeof res === "string" ? res : JSON.stringify(res)).toLowerCase();
                if (resStr.includes("sessionid is not valid") || 
                    resStr.includes("session is not valid") || 
                    resStr.includes("session expired") || 
                    resStr.includes("sessionid is invalid") || 
                    resStr.includes("session is invalid")) {
                    const baseUrl = getAppBaseUrl();
                    top.window.location.href = `${baseUrl}/aspx/sess.aspx`;
                    throw new Error("SessionId is not valid");
                }
            }

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
                console.log("Get List Cache Key: " + cacheKey);
                localStorage.setItem(cacheKey, JSON.stringify(list));

            } else console.log(`List Data for Ads name ${axDatasourceName} is Empty`);


            return list;

        } catch (err) {
            if (err.message === "SessionId is not valid") {
                throw err;
            }
            return [];
        }
    }


    /* ===============================
       TOAST HELPER
    =============================== */
    function showToast(message, duration = 5000, isSuccess = false) {
        let styleTag = document.getElementById("axi-toast-styles");
        if (!styleTag) {
            styleTag = document.createElement("style");
            styleTag.id = "axi-toast-styles";
            styleTag.innerHTML = `
                #axi-toast-container {
                    position: fixed;
                    bottom: 80px;
                    right: 24px;
                    display: flex;
                    flex-direction: column;
                    gap: 10px;
                    z-index: 100000;
                    pointer-events: none;
                }
                .axi-toast-card {
                    display: flex;
                    align-items: center;
                    padding: 12px 16px;
                    border-radius: 10px;
                    min-width: 300px;
                    max-width: 420px;
                    backdrop-filter: blur(12px);
                    -webkit-backdrop-filter: blur(12px);
                    box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.15), 0 4px 6px -2px rgba(0, 0, 0, 0.1), 0 0 0 1px rgba(0, 0, 0, 0.05);
                    opacity: 0;
                    transform: translateY(16px) scale(0.95);
                    transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
                    pointer-events: auto;
                    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
                    font-size: 13.5px;
                    line-height: 1.4;
                    color: #ffffff;
                    white-space: pre-line;
                }
                @media (max-width: 576px) {
                    #axi-toast-container {
                        right: 16px;
                        left: 16px;
                        bottom: 80px;
                    }
                    .axi-toast-card {
                        min-width: 0;
                        max-width: 100%;
                    }
                }
            `;
            document.head.appendChild(styleTag);
        }

        let container = document.getElementById("axi-toast-container");
        if (!container) {
            container = document.createElement("div");
            container.id = "axi-toast-container";
            document.body.appendChild(container);
        }

        const toast = document.createElement("div");
        toast.className = "axi-toast-card";

        // Determine Theme based on type
        let iconSvg = "";
        let borderStyle = "";
        let bgStyle = "";

        if (isSuccess === true || isSuccess === "success") {
            bgStyle = "rgba(16, 28, 21, 0.9)"; // Dark green glass
            borderStyle = "1px solid rgba(34, 197, 94, 0.3)";
            iconSvg = `<svg width="18" height="18" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" style="color: #22c55e; margin-right: 12px; flex-shrink: 0;"><path fill-rule="evenodd" clip-rule="evenodd" d="M10 18C14.4183 18 18 14.4183 18 10C18 5.58172 14.4183 2 10 2C5.58172 2 2 5.58172 2 10C2 14.4183 5.58172 18 10 18ZM13.7071 8.70711C14.0976 8.31658 14.0976 7.68342 13.7071 7.29289C13.3166 6.90237 12.6834 6.90237 12.2929 7.29289L9 10.5858L7.70711 9.29289C7.31658 8.90237 6.68342 8.90237 6.29289 9.29289C5.90237 9.68342 5.90237 10.3166 6.29289 10.7071L8.29289 12.7071C8.68342 13.0976 9.31658 13.0976 9.70711 12.7071L13.7071 8.70711Z" fill="currentColor"/></svg>`;
        } else if (isSuccess === false || isSuccess === "error" || isSuccess === "danger") {
            bgStyle = "rgba(35, 18, 18, 0.9)"; // Dark red glass
            borderStyle = "1px solid rgba(239, 68, 68, 0.3)";
            iconSvg = `<svg width="18" height="18" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" style="color: #ef4444; margin-right: 12px; flex-shrink: 0;"><path fill-rule="evenodd" clip-rule="evenodd" d="M18 10C18 14.4183 14.4183 18 10 18C5.58172 18 2 14.4183 2 10C2 5.58172 5.58172 2 10 2C14.4183 2 18 5.58172 18 10ZM11 14C11 14.5523 10.5523 15 10 15C9.44772 15 9 14.5523 9 14C9 13.4477 9.44772 13 10 13C10.5523 13 11 13.4477 11 14ZM10 5C9.44772 5 9 5.44772 9 6V10C9 10.5523 9.44772 11 10 11C10.5523 11 11 10.5523 11 10V6C11 5.44772 10.5523 5 10 5Z" fill="currentColor"/></svg>`;
        } else {
            bgStyle = "rgba(22, 22, 22, 0.9)"; // Dark charcoal glass
            borderStyle = "1px solid rgba(255, 255, 255, 0.1)";
            iconSvg = `<svg width="18" height="18" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" style="color: #3b82f6; margin-right: 12px; flex-shrink: 0;"><path fill-rule="evenodd" clip-rule="evenodd" d="M18 10C18 14.4183 14.4183 18 10 18C5.58172 18 2 14.4183 2 10C2 5.58172 5.58172 2 10 2C14.4183 2 18 5.58172 18 10ZM11 10C11 10.5523 10.5523 11 10 11C9.44772 11 9 10.5523 9 10V14C9 14.5523 9.44772 15 10 15C10.5523 15 11 14.5523 11 14V10ZM10 5C9.44772 5 9 5.44772 9 6C9 6.55228 9.44772 7 10 7C10.5523 7 11 6.55228 11 6C11 5.44772 10.5523 5 10 5Z" fill="currentColor"/></svg>`;
        }

        toast.style.backgroundColor = bgStyle;
        toast.style.border = borderStyle;

        // Create elements inside toast
        const iconDiv = document.createElement("div");
        iconDiv.innerHTML = iconSvg;
        iconDiv.style.display = "flex";
        iconDiv.style.alignItems = "center";

        const textSpan = document.createElement("span");
        textSpan.textContent = message;
        textSpan.style.flexGrow = "1";
        textSpan.style.marginRight = "12px";

        const closeBtn = document.createElement("button");
        closeBtn.innerHTML = `<svg width="10" height="10" viewBox="0 0 14 14" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M1.4 1.4L12.6 12.6M1.4 12.6L12.6 1.4" stroke="currentColor" stroke-width="2" stroke-linecap="round"/></svg>`;
        Object.assign(closeBtn.style, {
            background: "none",
            border: "none",
            color: "rgba(255, 255, 255, 0.4)",
            cursor: "pointer",
            padding: "6px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            borderRadius: "4px",
            transition: "all 0.2s ease",
            outline: "none",
            flexShrink: "0"
        });

        closeBtn.onmouseenter = () => {
            closeBtn.style.color = "rgba(255, 255, 255, 0.85)";
            closeBtn.style.backgroundColor = "rgba(255, 255, 255, 0.1)";
        };
        closeBtn.onmouseleave = () => {
            closeBtn.style.color = "rgba(255, 255, 255, 0.4)";
            closeBtn.style.backgroundColor = "transparent";
        };

        const removeToast = () => {
            toast.style.opacity = "0";
            toast.style.transform = "translateY(-12px) scale(0.95)";
            setTimeout(() => {
                if (container.contains(toast)) {
                    container.removeChild(toast);
                }
            }, 300);
        };

        closeBtn.onclick = removeToast;

        toast.appendChild(iconDiv);
        toast.appendChild(textSpan);
        toast.appendChild(closeBtn);
        container.appendChild(toast);

        requestAnimationFrame(() => {
            toast.style.opacity = "1";
            toast.style.transform = "translateY(0) scale(1)";
        });

        setTimeout(removeToast, duration);
    }

    async function showVersionInfo() {
        try {
            const axiUrl = `${getAppBaseUrl()}/AxpertPlugins/Axi_Beta/AxiCMDVersioninfo.json`;
            const axpertUrl = `${getAppBaseUrl()}/versionInfo.json`;

            const [axiRes, axpertRes] = await Promise.all([
                fetch(axiUrl, { cache: "no-store" }),
                fetch(axpertUrl, { cache: "no-store" })
            ]);

            let axiVersion = "";
            let axpertVersion = "";

            if (axiRes.ok) {
                const axiData = await axiRes.json();
                axiVersion = axiData?.version || "";
            }

            if (axpertRes.ok) {
                const axpertData = await axpertRes.json();
                const ver = axpertData?.version || "";
                const sub = axpertData?.subVersion || "";
                if (ver) {
                    axpertVersion = ver;
                    if (sub) {
                        axpertVersion += ` (${sub})`;
                    }
                }
            }

            if (axiVersion && axpertVersion) {
                showToast(`Axpert Version: ${axpertVersion}\nAxi CMD Version: ${axiVersion}`, 5000, true);
            } else if (axiVersion) {
                showToast(`Axi CMD Version: ${axiVersion}`, 5000, true);
            } else if (axpertVersion) {
                showToast(`Axpert Version: ${axpertVersion}`, 5000, true);
            } else {
                showToast("Version information is not available.");
            }
        } catch (err) {
            console.error("Failed to load version info", err);
            showToast("Failed to load version information.");
        }
    }




    function isSuggestionVisible() {
        if (megaDropdown) {
            return megaDropdown.style.display !== "none" && items.length > 0;
        }
        return list && list.style.display !== "none" && items.length > 0;
    }

    function hasActiveSuggestion() {
        return activeIndex >= 0 && activeIndex < items.length;
    }


    /* ===============================
       SETUP LISTENERS
    =============================== */
    function setupEventListeners() {

        const favDeleteCancelBtn = document.getElementById("axiFavDeleteCancelBtn");

        if (favDeleteCancelBtn) {
            favDeleteCancelBtn.addEventListener("click", (e) => {
                e.preventDefault();
                hideDeleteFavoriteModal();
            })
        }

        const favDeleteConfirmBtn = document.getElementById("axiFavDeleteConfirmBtn");

        if (favDeleteConfirmBtn) {
            favDeleteConfirmBtn.addEventListener("click", (e) => {
                e.preventDefault();
                confirmDeleteFavorite();
            });
        }



        const favCancelBtn = document.getElementById("axiFavCancelBtn");

        if (favCancelBtn) {
            favCancelBtn.addEventListener("click", (e) => {
                e.preventDefault();
                hideFavoriteModal();
            });
        }

        const favSaveBtn = document.getElementById("axiFavSaveBtn");
        if (favSaveBtn) {
            favSaveBtn.addEventListener("click", (e) => {
                e.preventDefault();
                confirmAddFavorite();
            });
        }

        const favInput = document.getElementById("axiFavNameInput");
        if (favInput) {
            favInput.addEventListener("keydown", (e) => {
                if (e.key === "Enter") {
                    e.preventDefault();
                    confirmAddFavorite();
                } else if (e.key === "Escape") {
                    hideFavoriteModal();
                }
            });
        }

        const favModalOverlay = document.getElementById("axiFavModalOverlay");

        if (favModalOverlay) {
            favModalOverlay.addEventListener("mousedown", (event) => {
                if (event.target === favModalOverlay) {
                    hideFavoriteModal();
                }
            })
        }

        const deleteModalOverlay = document.getElementById("axiFavDeleteModalOverlay");

        if (deleteModalOverlay) {
            deleteModalOverlay.addEventListener("mousedown", (event) => {
                if (event.target === deleteModalOverlay) {
                    hideDeleteFavoriteModal();
                }
            })
        }

        favouriteBtn.addEventListener("click", (e) => {
            e.preventDefault();
            e.stopPropagation();
            toggleFavorite(input.value.trim(), true);

        })

        const newAddFavBtn = document.getElementById("axiAddFavoriteBtn");
        if (newAddFavBtn) {
            newAddFavBtn.addEventListener("click", (e) => {
                e.preventDefault();
                e.stopPropagation();
                toggleFavorite(input.value.trim(), true);
            });
        }

        const newViewFavBtn = document.getElementById("axiViewFavoritesBtn");
        if (newViewFavBtn) {
            newViewFavBtn.addEventListener("click", (e) => {
                e.preventDefault();
                e.stopPropagation();
                loadFavorites();
                showFavoritesPopupOnly();
            });
        }

        const favFilterInput = document.getElementById("axiFavFilterInput");
        if (favFilterInput) {
            favFilterInput.addEventListener("input", (e) => {
                const query = e.target.value.toLowerCase().trim();
                const filtered = commandFavorites.filter(fav => 
                    (fav.commandText || "").toLowerCase().includes(query)
                );
                renderFavoritesUI(filtered);
            });
        }
        const historyBtn = document.getElementById("History_pages");
        if (historyBtn) {
            historyBtn.addEventListener("click", () => {
                showToast("Not Yet Implemented...");
                return;
            });
        }

        const prevBtn = document.getElementById("btnHistoryPrev");
        if (prevBtn) {
            prevBtn.addEventListener("click", () => window.prevbtn_click($(this)));
        }

        const nextBtn = document.getElementById("btnHistoryNext");
        if (nextBtn) {
            nextBtn.addEventListener("click", () => {
                showToast("Not Yet Implemented...");
                return;
            });
        }
        if (btnRefresh) {
            btnRefresh.addEventListener("click", async () => {
                console.log("Refresh Logic......");
                const refreshIcon = btnRefresh.querySelector(".axi-refresh-icon");
                if (refreshIcon) {
                    refreshIcon.classList.add("rotating");
                }

                try {
                    clearAxiLocalStorage("axi_");
                    commands = null;
                    tstructList = null;
                    adsList = null;
                    axDatasourceObj = {};
                    resolvedParams = {};
                    createfieldnamevaluesList = {};
                    resetSetCommandState();
                    cachedAccessPermissions = null;
                    initialSuggestionsCache = null;
                    initialSuggestionsSourceList = null;
                    initialSuggestionsRunnable = null;
                    targetEntitiesCache.sourceList = null;
                    targetEntitiesCache.matchesMap = null;

                    await initCommands(true);

                    input.focus();

                } catch (error) {
                    console.log("Refresh Failed: " + error);
                    alert("Error refreshing: " + error);

                } finally {
                    if (refreshIcon) {
                        refreshIcon.classList.remove("rotating");
                    }
                }
            });
        }



        if (axiClearBtn) {
            axiClearBtn.addEventListener("click", () => {
                input.value = "";
                createfieldnamevaluesList = {};
                setCommandTransid = null;
                dateControlBoolean = false;
                resetSetCommandState();
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
            if (suppressFocusSuggestions) {
                suppressFocusSuggestions = false;
                return;
            }
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
            if (document.querySelector(".AXI-Sec")?.classList.contains("axi-tour-active")) {
                e.preventDefault();
                return;
            }
            isDeleting = (e.key === "Backspace" || e.key === "Delete");
            console.log("Keys: " + e.key + "Code: " + e.code + "Alt: " + e.altKey);

            const tokens = getTokens(input.value.trim(), false);
            let normalizedTokens = [...tokens];
            if (normalizedTokens.length >= 2) {
                const first = cleanString(normalizedTokens[0]).toLowerCase();
                const second = cleanString(normalizedTokens[1]).toLowerCase();
                if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                    const temp = normalizedTokens[0];
                    normalizedTokens[0] = normalizedTokens[1];
                    normalizedTokens[1] = temp;
                }
            }
            const grpKey = normalizedTokens[0];

            let saveCommandConfig;
            if (grpKey)
                saveCommandConfig = getCommandConfig(grpKey);

            if (e.key === 'Backspace' && (grpKey?.toLowerCase() === "create" || grpKey?.toLowerCase() === "edit")) {
                let transIDcheck = setCommandTransid;
                if (input.selectionStart !== input.selectionEnd) {
                    createfieldnamevaluesList[transIDcheck] = [];
                    setCommandTransid = null;
                    dateControlBoolean = false;
                    resetSetCommandState();
                    return;
                }
                e.preventDefault();
                hide();

                const cursorPos = input.selectionStart;

                //tokens.pop();

                if (input.value[cursorPos - 1] === " " && !SET_COMMAND_STATE.currentField?.trim()) {


                    console.log("Deleted a space using Backspace");
                    console.log(SET_COMMAND_STATE);
                }
                else {
                    //if (createfieldnamevaluesList?.[transIDcheck]?.length > 0 && !SET_COMMAND_STATE.currentField) {
                    //    createfieldnamevaluesList[transIDcheck].pop();
                    //}
                    if (createfieldnamevaluesList?.[transIDcheck]?.length > 0) {

                        const list = createfieldnamevaluesList[transIDcheck];
                        const lastListItem = list[list.length - 1];

                        const lastTokenValue = cleanCommandToken(tokens[tokens.length - 1]);
                        // const actualLastTokenValue = tryResolveToken(tokens.length - 1, lastTokenValue, saveCommandConfig, false);
                        const { value: actualLastTokenValue, type } = tryResolveToken(tokens.length - 1, lastTokenValue, saveCommandConfig, false);

                        if (lastListItem) {

                            const parts = lastListItem.split("~");
                            const listValue = parts[0];

                            if (listValue === actualLastTokenValue) {

                                console.log("Removing last matching field:", lastListItem);
                                createfieldnamevaluesList[transIDcheck].pop();

                            } else {

                                console.log("Last token does not match last list value. No pop.");
                            }
                        }
                    }

                }
                //tokens.pop();


                let lastIndex = tokens.length - 1;
                tokens[lastIndex] = "";

                input.value = tokens.join(" ");

                console.log("After backspace our list : ");
                console.log(createfieldnamevaluesList[transIDcheck]);

                dateControlBoolean = false;
                resetSetCommandState();
                handleInput();


            }
            else if (e.key === 'Backspace' && grpKey?.toLowerCase() === "view") {
                if (input.selectionStart !== input.selectionEnd) {
                    setCommandTransid = null;
                    dateControlBoolean = false;
                    resetSetCommandState();
                    return;
                }
                e.preventDefault();
                hide();

                const cursorPos = input.selectionStart;


                if (input.value[cursorPos - 1] === " " && !SET_COMMAND_STATE.currentField?.trim()) {

                    console.log("Deleted a space using Backspace");
                    console.log(SET_COMMAND_STATE);
                }

                //tokens.pop();

                let lastIndex = tokens.length - 1;
                const lastToken = tokens[lastIndex];
                if (lastToken && lastToken.includes(",")) {
                    const parts = lastToken.split(",");
                    parts.pop();
                    tokens[lastIndex] = parts.join(",");
                } else {
                    tokens[lastIndex] = "";
                }

                input.value = tokens.join(" ");

                dateControlBoolean = false;
                resetSetCommandState();
                handleInput();
            }

            if (e.ctrlKey && e.code === "Space") {
                e.preventDefault();
                handleInput();
                return;
            }


            //if (e.ctrlKey && e.shiftKey && e.key.toLowerCase() === "enter") {
            //    popUpOption = true;
            //    e.preventDefault();
            //    hide();
            //    executeCommandsV2();
            //    return;
            //}

            const isSingleTarget = tokens.length === 1 && isTargetEntity(tokens[0]);

            if ((e.ctrlKey && e.shiftKey && e.key?.toLowerCase() === "enter") && (grpKey?.toLowerCase() === "create" || grpKey?.toLowerCase() === "edit" || grpKey?.toLowerCase() === "view" || grpKey?.toLowerCase() === "configure" || isSingleTarget)) {

                e.preventDefault();


                try {
                    if (tokens.length >= 1) {
                        hide();
                        popUpOption = true;
                        executeCommandsV2();
                        return;
                    }
                    //else return;
                }
                catch (err) {
                    popUpOption = false;

                    console.log("Error in Popup:", err);

                    showToast("Something went wrong. Please try again later");

                    return;
                }
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



                const cmdText = input.value.trim().toLowerCase();
                executeCommandsV2();
                if (cmdText !== "help") {
                    hide();
                }
                return;




            }

            // AUTO DOUBLE-QUOTE FOR MULTI-WORD SUGGESTIONS
            if (e.key === " " && items.length > 0) {
                const val = input.value;
                if (input.selectionStart === val.length) {
                    const tokens = getTokens(val);
                    const lastTokenRaw = tokens[tokens.length - 1] || "";

                    const isFirstToken = tokens.length === 1;

                    if (!lastTokenRaw.startsWith('"') && (!isFirstToken || !getCommandConfig(lastTokenRaw))) {
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
            if (list.style.display === "none" || items.length === 0) return;
            const gridEl = list.querySelector(".axi-verbs-grid");
            const isInitialGrid = (gridEl !== null);
            const verbsCount = isInitialGrid ? list.querySelectorAll(".axi-verbs-grid .axi-suggestion").length : 0;
            let cols = 3;
            if (isInitialGrid && gridEl && verbsCount > 0) {
                try {
                    const gridStyle = window.getComputedStyle(gridEl);
                    const computedCols = gridStyle.getPropertyValue("grid-template-columns").split(" ").filter(Boolean).length;
                    if (computedCols > 0) {
                        cols = computedCols;
                    }
                } catch (err) {
                    cols = 3;
                }
            }

            if (e.key === "ArrowDown") {
                e.preventDefault();
                if (isInitialGrid) {
                    if (activeIndex < verbsCount) {
                        const nextRowIndex = activeIndex + cols;
                        if (nextRowIndex < verbsCount) {
                            activeIndex = nextRowIndex;
                        } else {
                            if (items.length > verbsCount) {
                                activeIndex = verbsCount;
                            } else {
                                activeIndex = (activeIndex + 1) % verbsCount;
                            }
                        }
                    } else {
                        activeIndex = (activeIndex + 1) % items.length;
                    }
                } else {
                    activeIndex = (activeIndex + 1) % items.length;
                }
                highlight();
            }
            else if (e.key === "ArrowUp") {
                e.preventDefault();
                if (isInitialGrid) {
                    if (activeIndex >= verbsCount) {
                        if (activeIndex === verbsCount) {
                            const lastRowStart = Math.floor((verbsCount - 1) / cols) * cols;
                            activeIndex = lastRowStart + (cols - 1);
                            if (activeIndex >= verbsCount) {
                                activeIndex = verbsCount - 1;
                            }
                        } else {
                            activeIndex = activeIndex - 1;
                        }
                    } else {
                        const prevRowIndex = activeIndex - cols;
                        if (prevRowIndex >= 0) {
                            activeIndex = prevRowIndex;
                        } else {
                            activeIndex = items.length - 1;
                        }
                    }
                } else {
                    activeIndex = (activeIndex - 1 + items.length) % items.length;
                }
                highlight();
            }
            else if (e.key === "ArrowRight") {
                e.preventDefault();
                if (isInitialGrid && activeIndex < verbsCount) {
                    activeIndex = (activeIndex + 1) % verbsCount;
                } else {
                    activeIndex = (activeIndex + 1) % items.length;
                }
                highlight();
            }
            else if (e.key === "ArrowLeft") {
                e.preventDefault();
                if (isInitialGrid && activeIndex < verbsCount) {
                    activeIndex = (activeIndex - 1 + verbsCount) % verbsCount;
                } else {
                    activeIndex = (activeIndex - 1 + items.length) % items.length;
                }
                highlight();
            }
            if (e.key === "Tab") { e.preventDefault(); if (activeIndex === -1) activeIndex = 0; apply(activeIndex); }
            // if (e.key === "Enter" && activeIndex >= 0) {
            //     e.preventDefault();

            //     apply(activeIndex);



            // }
            if (e.key === "Escape") {
                e.preventDefault();

                hide();
            }

            if (e.ctrlKey && e.key.toLowerCase() === "s") {
                e.preventDefault();
                console.log("Save Option Selected...Submitting Data...");
                hide();
                if (grpKey.toLowerCase() === "create")
                    AxisaveDataFn(createfieldnamevaluesList, setCommandTransid, "axi_nongridfieldlist", true, tokens, saveCommandConfig);
                else if (grpKey.toLowerCase() === "edit")
                    AxisaveDataFn(createfieldnamevaluesList, setCommandTransid, "axi_nongridfieldlist", false, tokens, saveCommandConfig);
                resetSetCommandState();
                return;
            }
        });

        document.addEventListener("keydown", e => {
            if (e.key !== "Escape") return;

            const favModalOverlay = document.getElementById("axiFavModalOverlay");
            const isFavModalOpen = favModalOverlay && (favModalOverlay.style.display === "flex" || (window.getComputedStyle && window.getComputedStyle(favModalOverlay).display !== "none"));
            
            const deleteModalOverlay = document.getElementById("axiFavDeleteModalOverlay");
            const isDeleteModalOpen = deleteModalOverlay && (deleteModalOverlay.style.display === "flex" || (window.getComputedStyle && window.getComputedStyle(deleteModalOverlay).display !== "none"));

            const isFavCardOpen = favouritesCard && (favouritesCard.style.display === "flex" || (megaDropdown && megaDropdown.style.display === "flex" && window.getComputedStyle && window.getComputedStyle(favouritesCard).display !== "none"));

            const isSuggestionsOpen = (list && list.style.display !== "none") || (megaDropdown && megaDropdown.style.display === "flex");

            if (isFavModalOpen) {
                e.preventDefault();
                e.stopPropagation();
                hideFavoriteModal();
                return;
            }

            if (isDeleteModalOpen) {
                e.preventDefault();
                e.stopPropagation();
                hideDeleteFavoriteModal();
                return;
            }

            if (isFavCardOpen || isSuggestionsOpen) {
                e.preventDefault();
                e.stopPropagation();
                suppressFocusSuggestions = true;
                hide();
                if (input) {
                    input.focus();
                }
                return;
            }
        }, true);

        document.addEventListener("click", e => {
            const isDropdownVisible = megaDropdown && megaDropdown.style.display !== "none";
            if (isDropdownVisible || (list && list.style.display !== "none")) {
                const insideSearchWrapper = searchWrapper && searchWrapper.contains(e.target);
                const insideDropdown = megaDropdown && megaDropdown.contains(e.target);
                const insideFavModal = (document.getElementById("axiFavModalOverlay") && document.getElementById("axiFavModalOverlay").contains(e.target));
                const insideDeleteModal = (document.getElementById("axiFavDeleteModalOverlay") && document.getElementById("axiFavDeleteModalOverlay").contains(e.target));
                if (!insideSearchWrapper && !insideDropdown && !insideFavModal && !insideDeleteModal && e.target !== input) {
                    hide();
                }
            }
        });




        const iframe = document.getElementById("middle1");
        if (iframe) {
            const attachClickToDoc = (doc) => {
                if (!doc) return;
                if (doc._axiClickAttached) return;
                doc._axiClickAttached = true;

                doc.addEventListener("click", () => {
                    hide();
                });

                try {
                    const iframes = doc.getElementsByTagName("iframe");
                    for (let i = 0; i < iframes.length; i++) {
                        const subIframe = iframes[i];
                        try {
                            const subDoc = subIframe.contentDocument || subIframe.contentWindow?.document;
                            if (subDoc) attachClickToDoc(subDoc);
                        } catch (e) {}

                        subIframe.addEventListener("load", () => {
                            try {
                                const subDoc = subIframe.contentDocument || subIframe.contentWindow?.document;
                                if (subDoc) attachClickToDoc(subDoc);
                            } catch (e) {}
                        });
                    }
                } catch (e) {}
            };

            const attachIframeClick = () => {
                try {
                    const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                    attachClickToDoc(iframeDoc);
                } catch (err) {
                    console.warn("Could not attach click listener to iframe (likely CORS restriction):", err);
                }
            };

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
        const suggestionElements = Array.from(list.querySelectorAll(".axi-suggestion"));
        if (suggestionElements.length > 0) {
            suggestionElements.forEach((li, i) => li.classList.toggle("active", i === activeIndex));

            const activeItem = suggestionElements[activeIndex];
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
    function executeCommandsV2(isNavigating = false) {
        if (document.querySelector(".AXI-Sec")?.classList.contains("axi-tour-active")) {
            return;
        }
        if (input.value === "") {
            showToast("Invalid Command!");
            return;
        }
        if (isCommandsLoading) {
            showToast("Commands are not loaded!");
            return;
        }

        if (isEditing) {
            showToast("Execution is not Allowed while executing");
            return;
        }



        const text = input.value.trim();



        if (!text || !commands) return;

        const lowerText = text.toLowerCase();
        if (lowerText === "help") {
            startWalkthrough();
            return;
        }

        if (lowerText === "version") {
            showVersionInfo();
            return;
        }

        const rawTokens = getTokens(text, false);
        if (rawTokens.length > 0) {
            const firstToken = cleanString(rawTokens[0]).toLowerCase();
            const secondToken = rawTokens[1] ? cleanString(rawTokens[1].toLowerCase()) : ""; 

            
            if (!isProgrammaticExecution && ["create", "view", "edit", "source"].includes(firstToken) && secondToken !== "inbox") {
                showToast("Invalid Command!");
                return;
            }
        }

        const tokens = getTokens(text);
        if (tokens.length === 1 && isTargetEntity(tokens[0])) {
            const entityObj = getTargetEntityObj(tokens[0]);
            const stype = entityObj ? (entityObj.stype || entityObj.STYPE || "").toLowerCase() : "";
            if (["i", "iview", "ads", "page", "p", "t", "tstruct"].includes(stype) || isInboxStructure(entityObj)) {
                tokens.unshift("view");
            }
        }

        if (tokens.length >= 2) {
            const first = cleanString(tokens[0]).toLowerCase();
            const second = cleanString(tokens[1]).toLowerCase();
            if (isTargetEntity(first) && isValidActionForTarget(first, second)) {
                const temp = tokens[0];
                tokens[0] = tokens[1];
                tokens[1] = temp;
            }
        }

        if (tokens[3]?.toLowerCase() === "with") {
            showToast("Execution is Not Allowed while editing");
            return;
        }

        if (tokens.length === 0) return;

        const groupKey = cleanString(tokens[0]);

        const groupNameNormalized = groupKey.toLowerCase();

        // Enforce Structure Permissions
        if (["view", "create", "edit"].includes(groupNameNormalized)) {
            const targetToken = tokens[1];
            if (targetToken && targetToken.toLowerCase() !== "inbox") {
                const entityObj = getTargetEntityObj(targetToken, groupNameNormalized);
                if (entityObj) {
                    // if (groupNameNormalized === "edit" && (entityObj.keyfieldforedit === "F" || entityObj.keyfieldforedit === false)) {
                    //     showToast("Key field is not configured for this form.", 3000, false);
                    //     return;
                    // }
                    if (!isActionAllowed(entityObj, groupNameNormalized)) {
                        showToast("You do not have permission to execute this command.", 3000, false);
                        return;
                    }
                }
            }
        }

        if (groupNameNormalized === "run" && (isPreviewModalOpen() || isRunDisabledForPage() || isViewDesignerModalOpen())) {
            showToast("Execution of run command is not allowed when the active page, preview modal or view designer is not runnable");
            return;
        }
        const accessPermissions = getAccessPermissions();

        if (groupNameNormalized === "source" && !accessPermissions.buildAccess) {
            showToast(`User '${window.mainUserName}' has no access for command: ${text}`);
            return;
        }

        if (groupNameNormalized === "configure" && !accessPermissions.appMgrAccess) {
            showToast(`User '${window.mainUserName}' has no access for command: ${text}`);
            return;
        }
        if (groupNameNormalized === "upload" && !accessPermissions.importAccess) {
            showToast(`User '${window.mainUserName}' has no access for command: ${text}`);
            return;
        }
        if (groupNameNormalized === "download" && !accessPermissions.exportAccess) {
            showToast(`User '${window.mainUserName}' has no access for command: ${text}`);
            return;
        }
        if (groupNameNormalized === "sdk" && !accessPermissions.buildAccess) {
            showToast(`User '${window.mainUserName}' has no access for command: ${text}`);
            return;
        }

        const groupConfig = getCommandConfig(groupKey);

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
        if (!isNavigating) {
            saveToHistory(text);



        }


        if (groupKey.toLowerCase() === "run") {
            return;
        }



    }


    function cleanCommandToken(val = "") {
        return val.replace(/["]/g, "").trim();
    }

    function getGroupHandlers(group) {
        if (!group || typeof COMMAND_HANDLERS === "undefined") return null;
        const lowGroup = group.toLowerCase();
        if (COMMAND_HANDLERS[group]) return COMMAND_HANDLERS[group];
        const matchedKey = Object.keys(COMMAND_HANDLERS).find(k => k.toLowerCase() === lowGroup);
        return matchedKey ? COMMAND_HANDLERS[matchedKey] : null;
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
        const groupHandlers = getGroupHandlers(group);

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
        // let transId = tryResolveToken(1, rawName, commandConfig, false);
        const { value, type } = tryResolveToken(1, rawName, commandConfig, false);
        let transId = value;

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
        if (typeof top !== "undefined" && top.window && typeof top.window.LoadIframe === "function") {
            top.window.LoadIframe('../aspx/processflow.aspx?activelist=t');
        } else {
            window.LoadIframe('../aspx/processflow.aspx?activelist=t');
        }
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
        // let transId = tryResolveToken(1, rawStruct, commandConfig, false);
        const { value, type } = tryResolveToken(1, rawStruct, commandConfig, false);
        let transId = value;

        let fieldName = "";
        let fieldValue = "";
        let fieldUniqueId = "";


        const struct_prompt = commandConfig.prompts[0];
        const struct_source = struct_prompt.promptSource;

        // extra params
        const struct_paramValue = processExtraParams(tokens, commandConfig);

        const struct_sourceKey = (struct_paramValue ? `${struct_source}_${struct_paramValue}` : struct_source).toLowerCase();

        // load datasource if not exists
        if (!axDatasourceObj[struct_sourceKey]) {

            const struct_hasParams = !struct_prompt.promptParams ||
                (struct_paramValue && struct_paramValue.replace(/,/g, '').trim().length > 0);

            if (struct_hasParams) {
                loadList(struct_source, struct_paramValue);
                return [`Loading ${struct_source}...`];
            }
        }

        const struct_dataList = axDatasourceObj[struct_sourceKey];

        let preferredType = getResolvedParamType(1);
        if (preferredType) {
            preferredType = preferredType.toLowerCase();
            if (preferredType === "tstruct") preferredType = "t";
            if (preferredType === "iview") preferredType = "i";
        }

        const struct_row = struct_dataList.find(r => 
            r.name === transId && 
            (!preferredType || (r.stype || "").toLowerCase() === preferredType)
        ) || struct_dataList.find(r => r.name === transId);

        const primaryField = struct_row?.keyfield;

        if (!primaryField) {
            console.error(`Keyfield is empty in ADS datasource: ${struct_source}`);
            showToast("Please try again later...");
            return [];
        }

        setEditSessionState(transId)

        if (tokens.length === 3) {
            const lastToken = tokens[tokens.length - 1];
            const matchedField = getMatchedField(lastToken, transId);
            if (matchedField) {
                const isFieldVal = (matchedField.isfield || "").toString().toLowerCase();
                if (isFieldVal === "t" || isFieldVal === "true") {
                    showToast(`A value is required after the field '${matchedField.caption || matchedField.name || lastToken}'.`);
                    return;
                }
            }
        }

        ///We need to optimize this(token index)(optimized one is below 17-03-26-T)
        let tokenIndex;
        let tokenBasedBooleanCheck;
        if (tokens.length > 3) {
            tokenIndex = 3;
            tokenBasedBooleanCheck = false;
        }
        else {
            tokenIndex = 2;
            tokenBasedBooleanCheck = true;
        }
        //if (tokens.length > 3) {

        //    let rawFieldName = cleanCommandToken(tokens[2]);
        //    fieldName = tryResolveToken(2, rawFieldName, commandConfig, false);

        //    let rawValue = cleanCommandToken(tokens[3]);
        //    fieldValue = tryResolveToken(3, rawValue, commandConfig, false);

        //    fieldUniqueId = getUniqueId(fieldValue);

        //    redirectToTstruct(transId, rawStruct, true, struct_row.keyfield, fieldUniqueId);
        //}
        //else {


        //    let rawValue = cleanCommandToken(tokens[2]);
        //    fieldValue = tryResolveToken(2, rawValue, commandConfig, true);

        //    fieldUniqueId = getUniqueId(fieldValue);

        //    redirectToTstruct(transId, rawStruct, true, struct_row.keyfield, fieldUniqueId);
        //}
        let rawValue = cleanCommandToken(tokens[tokenIndex]);
        // fieldValue = tryResolveToken(tokenIndex, rawValue, commandConfig, tokenBasedBooleanCheck);
        const { value: resolvedFieldValue, type: resolvedFieldType } = tryResolveToken(tokenIndex, rawValue, commandConfig, tokenBasedBooleanCheck);
        fieldValue = resolvedFieldValue;

        fieldUniqueId = getUniqueId(fieldValue);

        redirectToTstruct(transId, rawStruct, true, struct_row.keyfield, fieldUniqueId);
    }


    // function handleEditData({ tokens, commandConfig, resolvedParams }) {




    //     let rawStruct = cleanCommandToken(tokens[1]);
    //     let transId = tryResolveToken(1, rawStruct, commandConfig, false);
    //     let rawFieldName = "";
    //     let fieldName = "";
    //     let actualFieldName = "";
    //     let rawValue = "";
    //     let fieldValue = "";
    //     let fieldUniqueId = "";
    //     const extraPromptSource = commandConfig.prompts[1].extraParams.toLowerCase();
    //     let fieldValuePromptSource;
    //     let valuePresentInList = false;

    //     const extraSourceKey = `${extraPromptSource}_${transId}`.toLowerCase();

    //     const extraList = axDatasourceObj[extraSourceKey];

    //     const extraInlineValue = commandConfig?.prompts?.[3]?.promptValues;


    //     if (transId === rawStruct) {
    //         const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
    //         const found = list?.find(
    //             x => x.caption?.toLowerCase() === rawStruct
    //         );
    //         if (!found || !found.name) {
    //             console.error("TStruct not found:", rawStruct);
    //             return;
    //         }
    //         transId = found.name;
    //     }

    //     if (tokens.length > 3 && tokens.some(t => t.toLowerCase() !== extraInlineValue.toLowerCase())) {
    //         fieldValuePromptSource = commandConfig.prompts[2].promptSource.toLowerCase();
    //         rawFieldName = cleanCommandToken(tokens[2]);
    //         actualFieldName = tryResolveToken(2, rawFieldName, commandConfig, true);


    //         if (!Array.isArray(extraList)) {
    //             console.warn("Hidden field list is missing or invalid", extraList);
    //             actualFieldName = null;
    //             return;

    //         } else if (extraList.length === 0) {
    //             console.log("hidden field List is Empty!");
    //             actualFieldName = null;
    //             return;

    //         } else {
    //             const field = extraList[0];

    //             fieldName = field.fname ?? field.keyfield ?? field.name ?? field.displaydata;

    //             if (actualFieldName === null) {
    //                 console.error("Field name resolution failed: ", fieldName)
    //             }
    //         }
    //         if (!fieldName) {
    //             console.error("Field resolution failed:", rawFieldName);
    //             return;
    //         }

    //         rawValue = cleanCommandToken(tokens[3]);
    //         fieldValue = tryResolveToken(3, rawValue, commandConfig, false);
    //         fieldUniqueId = getUniqueId(fieldValue);

    //         const extraFieldValueList = axDatasourceObj[`${fieldValuePromptSource}_${transId}$#$${actualFieldName}`.toLowerCase()];
    //         console.log(`Edit Data ? TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`);

    //         setEditSessionState(transId);

    //         if (Array.isArray(extraFieldValueList) && extraFieldValueList.length > 0) {

    //             valuePresentInList = extraFieldValueList.some(item =>
    //                 // item.displaydata?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 // item.name?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 // item.fname?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 // item.keyfield?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 item.caption?.toLowerCase() === fieldUniqueId.toLowerCase()

    //             );

    //             if (valuePresentInList)
    //                 redirectToTstruct(transId, rawStruct, true, fieldName, fieldUniqueId);
    //             else
    //                 redirectToTstruct(transId, rawStruct, false, fieldName, fieldUniqueId);
    //         }
    //         else {

    //             redirectToTstruct(transId, rawStruct, false, fieldName, fieldUniqueId);
    //         }




    //     } else {
    //         fieldValuePromptSource = commandConfig.prompts[1].promptSource.toLowerCase()

    //         if (!Array.isArray(extraList)) {
    //             console.warn("Hidden field list is missing or invalid", extraList);
    //             fieldName = null;
    //             return;

    //         } else if (extraList.length === 0) {
    //             console.log("hidden field List is Empty!");
    //             fieldName = null;
    //             return;

    //         } else {
    //             const field = extraList[0];

    //             fieldName = field.fname ?? field.keyfield ?? field.name ?? field.displaydata;

    //             if (fieldName === null) {
    //                 console.error("Field name resolution failed: ", fieldName)
    //             }

    //             rawValue = cleanCommandToken(tokens[2]);
    //             fieldValue = tryResolveToken(2, rawValue, commandConfig, true);
    //             fieldUniqueId = getUniqueId(fieldValue);

    //             if (fieldValue === null) {
    //                 console.error("Field value resolution failed:", rawValue);
    //                 return;
    //             }



    //         }

    //         const extraFieldValueList = axDatasourceObj[`${fieldValuePromptSource}_${transId}$#$${fieldName}`.toLowerCase()];
    //         console.log(`Edit Data ? TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`);

    //         setEditSessionState(transId);

    //         if (Array.isArray(extraFieldValueList) && extraFieldValueList.length > 0) {

    //             valuePresentInList = extraFieldValueList.some(item =>
    //                 // item.displaydata?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 // item.name?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 // item.fname?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 // item.keyfield?.toLowerCase() === fieldUniqueId.toLowerCase() ||
    //                 item.caption?.toLowerCase() === fieldUniqueId.toLowerCase()

    //             );

    //             if (valuePresentInList)
    //                 redirectToTstruct(transId, rawStruct, true, fieldName, fieldUniqueId);
    //             else
    //                 redirectToTstruct(transId, rawStruct, false, fieldName, fieldUniqueId);
    //         }
    //         else {

    //             redirectToTstruct(transId, rawStruct, false, fieldName, fieldUniqueId);
    //         }

    //     }

    // }


    function handleConfigurePermissions({ tokens, commandConfig }) {



        let rawUserName = cleanCommandToken(tokens[2]);

        let { value: resolvedUserName, type } = tryResolveToken(2, rawUserName, commandConfig, false);






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



    function handleOpenApi({ tokens, commandConfig }) {
        console.log("commandConfig: " + JSON.stringify(commandConfig));
        let fieldname = "ExecAPIDefName";
        let transId = "apidg";
        let param1Position = commandConfig.prompts[1].wordPos - 1;

        let rawApiName = cleanCommandToken(tokens[param1Position]);



        setEditSessionState(transId);
        redirectToTstruct(transId, "", true, fieldname, rawApiName);





    }

    function handleConfigureRule({ tokens, commandConfig }) {

        let transId = "ad_re";
        let fieldname = "rulename";

        let rawParamName = cleanCommandToken(tokens[2]);



        setEditSessionState(transId);
        redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, rawParamName);





    }

    function handleConfigureScheduledNotification({ tokens, commandConfig }) {

        let transId = "a__pn";
        let fieldname = "name";

        let rawParamName = cleanCommandToken(tokens[2]);



        setEditSessionState(transId);
        redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, rawParamName);





    }

    //function handleConfigureServer({ tokens, commandConfig }) {

    //    let transId = "axpub";
    //    let fieldname = "servername";

    //    const rawParamName = cleanCommandToken(tokens[2]);


    //    setEditSessionState(transId);
    //    redirectToTstruct(transId, "", true, fieldname, rawParamName);





    //}

    function handleCofigurePegFormNotification({ tokens, commandConfig }) {

        let transId = "ad_pn";
        let fieldname = "name";

        const rawParamName = cleanCommandToken(tokens[2]);


        setEditSessionState(transId);
        redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, rawParamName);





    }

    function handleConfigurePeg({ tokens, commandConfig }) {

        let rawParamName = cleanCommandToken(tokens[2]);

        redirectToProcessFlow(rawParamName, cleanCommandToken(tokens[1]));



    }

    function handleConfigureFormNotification({ tokens, commandConfig }) {

        let transId = "a__fn";
        const fieldname = "stransid";



        let rawParamValue = cleanCommandToken(tokens[2]);
        const { value: fieldValue, type } = tryResolveToken(2, rawParamValue, commandConfig, false);


        setEditSessionState(transId);
        redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, fieldValue);



    }

    function handleConfigureNewsAndAnnouncement({ tokens, commandConfig }) {
        let transId = "a__na";
        const fieldname = "title";
        let rawTitle;
        let paramValue;


        setEditSessionState(transId);

        if (tokens.length > 2) {
            rawTitle = cleanCommandToken(tokens[2]);
            // paramValue = tryResolveToken(2, rawTitle, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawTitle, commandConfig, false);
            paramValue = value;

            redirectToTstruct(transId, cleanCommandToken(tokens[1]), true, fieldname, paramValue)
        }
        else {
            redirectToTstruct(transId, cleanCommandToken(tokens[1]));
        }
    }

    function handleConfigureSettings({ tokens, commandConfig }) {
        // const targetUrl = "../aspx/Configuration.aspx/LoadUserAppSettings"; 
        let targetUrl = "../aspx/Configuration.aspx?";



        if (popUpOption) {
            targetUrl += `tname=${encodeURIComponent(cleanCommandToken(tokens[1]))}`;
            targetUrl += "&AxPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            window.LoadIframe(targetUrl);
        }
    }



    function handleConfigureProperties({ tokens, commandConfig }) {
        const transId = "ad_pr";
        let targetUrl = `../aspx/tstruct.aspx?act=load&transid=${transId}&axpdef_axpertpropsid=1`;
        //   const targetUrl = "../aspx/tstruct.aspx?act=load&transid=ad_pr&axpdef_axpertpropsid=1";
        setEditSessionState(transId);

        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(cleanCommandToken(tokens[1]))}`;
            targetUrl += "&AxPop=true";

            openPopOption(targetUrl)
        }
        else {
            setCommandRoutes(input.value.trim(), targetUrl);
            top.window.LoadIframe(targetUrl);
        }

    }

    //function handleConfigureJob({ tokens, commandConfig }) {

    //    let transId = "job_s";
    //    let fieldname = "jname";

    //    let rawParamName = cleanCommandToken(tokens[2]);


    //    setEditSessionState(transId);
    //    redirectToTstruct(transId, "", true, fieldname, rawParamName);

    //}

    /*********************************************************
      * End 
      * ******************************************************
      */

    function handleUpload({ tokens, commandConfig }) {

        if (mode === "ai") {
            handleAiButtons("openUpload");

        } else {
            const targetUrl = "../aspx/ImportAll.aspx";
            setCommandRoutes(input.value.trim(), targetUrl);
            window.LoadIframe(targetUrl);


        }




    }

    function handleDownload({ tokens, commandConfig }) {
        let targetUrl = "../aspx/ExportNew.aspx";
        targetUrl += "?action=export";
        setCommandRoutes(input.value.trim(), targetUrl);
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
            console.log("SetEditSessioncachekey: " + storageKey);
            localStorage.setItem(storageKey, JSON.stringify(updated));
        }
    }


    function redirectToEntity(transId, formCaption = "", fieldName, fieldValue) {
        let targetUrl;
        if (!fieldValue) {

            targetUrl = `../aspx/Entity.aspx?tstid=${transId}`;

        } else {
            targetUrl = `../aspx/EntityForm.aspx?tstid=${transId}`;
            targetUrl += `&${fieldName}=${encodeURIComponent(fieldValue)}`;




        }

        setCommandRoutes(input.value.trim(), targetUrl);
        if (popUpOption) {
            targetUrl += `&tname=${encodeURIComponent(formCaption)}`;
            openPopOption(targetUrl)
        }
        else {
            window.LoadIframe(targetUrl);
        }

    }

    function handleViewSourceAds(paramName) {
        let targetUrl;
        // let paramName;
        const iviewName = "csqlist"
        const transId = "b_sql";
        let fieldname = "sqlname";

        // let rawName = cleanCommandToken(tokens[2]);



        // if (rawName) {
        //     paramName = tryResolveToken(2, rawName, commandConfig, false);

        // }



        setEditSessionState(transId);



        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        // if (!paramName) {
        //     setCommandRoutes(input.value.trim(), targetUrl);

        //     redirectToIView(iviewName);


        // } else {
        targetUrl += `&${fieldname}=${encodeURIComponent(paramName)}`;
        targetUrl += "&act=load";
        targetUrl += "&dummyload=false?";
        // setCommandRoutes(input.value.trim(), targetUrl);

        window.LoadIframe(targetUrl);

        // }

    }

    function handleViewSource({ tokens, commandConfig }) {
        const accessPermissions = getAccessPermissions();
        if (!accessPermissions?.buildAccess) {
            showToast(`User '${window.mainUserName}' has no access for command: ${input.value.trim()}`);
            return;
        }

        const structName = getCurrentStructName();

        if (!structName) {
            showToast("Unable to determine current structure");
            console.error("handleViewSource: getCurrentStructName() returned null or undefined");
            return;
        }



        switch (structName.type.toLowerCase()) {
            case "entity":
                window.openDeveloperStudio("tstreact", structName.name, true);
                break;
            case "iview":
                window.openDeveloperStudio("ivreact", structName.name, true);
                break;
            case "ads":
                handleViewSourceAds(structName.name);

                break;
            case "page":
                break;

            default:
                showToast("Unknown source type: " + structName.name);
                break;


        }




    }

    function handleSourceCommand({ tokens, commandConfig }) {
        const accessPermissions = getAccessPermissions();
        if (!accessPermissions?.buildAccess) {
            showToast(`User '${window.mainUserName}' has no access for command: ${input.value.trim()}`);
            return;
        }

        let rawName = cleanCommandToken(tokens[1]);
        const entityObj = getTargetEntityObj(rawName, "source");
        if (!entityObj) {
            showToast("Invalid structure name: " + rawName);
            return;
        }

        const type = (entityObj.stype || entityObj.STYPE || "").toLowerCase().trim();
        const name = entityObj.name || entityObj.NAME || "";

        let targetUrl = "";
        if (type === "t" || type === "tstruct") {
            targetUrl = "developerstudio:tstruct:" + name;
            window.openDeveloperStudio("tstreact", name, true);
        } else if (type === "i" || type === "iview") {
            targetUrl = "developerstudio:iview:" + name;
            window.openDeveloperStudio("ivreact", name, true);
        } else if (type === "ads") {
            targetUrl = `../aspx/tstruct.aspx?transid=b_sql&sqlname=${encodeURIComponent(name)}&act=load&dummyload=false?`;
            handleViewSourceAds(name);
        } else {
            showToast("Unknown source type: " + name);
            return;
        }

        setCommandRoutes(input.value.trim(), targetUrl);
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
        let paramValue;


        if (tokens.length < 2) {
            console.warn("View Command required atleast two tokens");
            showToast("view command requires atleast two tokens");
            return;
        }

        console.log(JSON.stringify(commandConfig));


        const promptValues = commandConfig?.prompts?.[0].promptValues;
        const viewDataSource = commandConfig?.prompts?.[0].promptSource;
        if (viewDataSource.toLowerCase() === "axi_structmetalist") {
            paramValue = processExtraParams(tokens, commandConfig);
        }
        // const extraDataSource = commandConfig?.prompts?.[1].extraParams;
        //const extraDataSource = "axi_keyfieldList";


        const viewDataSourceKey = `${viewDataSource}_${paramValue}`.toLowerCase();
        let rawStruct = cleanCommandToken(tokens[1]);
        // transId = tryResolveToken(1, rawStruct, commandConfig, false);
        const { value: rawStructValue, type: fieldType } = tryResolveToken(1, rawStruct, commandConfig, false);
        transId = rawStructValue;


        type = getType(viewDataSourceKey, { value: rawStructValue, type: fieldType }, promptValues, tokens, commandConfig);

        if (type === "tstruct" && tokens.length === 3) {
            const lastToken = tokens[tokens.length - 1];
            const matchedField = getMatchedField(lastToken, transId);
            if (matchedField) {
                const isFieldVal = (matchedField.isfield || "").toString().toLowerCase();
                if (isFieldVal === "t" || isFieldVal === "true") {
                    showToast(`A value is required after the field '${matchedField.caption || matchedField.name || lastToken}'.`);
                    return;
                }
            }
        }

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
                filters: filters
            });
            return;


        } else if (type === "page") {





            let rawFieldValue = cleanCommandToken(tokens[1]);
            redirectToHtmlPages(rawFieldValue, tokens, commandConfig);
            return;

        } else if (type === "iview") {
            redirectToIView(transId, rawStruct);
            return;

        } else if (type === "inbox") {
            handleViewInbox();
            return;
        }


        //const extraSourceKey = `${extraDataSource}_${transId}`.toLowerCase();


        if (tokens.length > 3) {
            fieldValueIndex = 3;


        } else {
            fieldValueIndex = 2;


        }


        let preferredType = type;
        if (preferredType) {
            preferredType = preferredType.toLowerCase();
            if (preferredType === "tstruct") preferredType = "t";
            if (preferredType === "iview") preferredType = "i";
        }

        const struct_rowList = axDatasourceObj[viewDataSourceKey];
        const struct_row = struct_rowList.find(r => 
            r.name === transId && 
            (!preferredType || (r.stype || "").toLowerCase() === preferredType)
        ) || struct_rowList.find(r => r.name === transId);

        const primaryField = struct_row?.keyfield;

        if (!primaryField) {
            console.error(`Keyfield is empty in ADS datasource: ${struct_source}`);
            showToast("Please try again later...");
            return [];
        }

        setEditSessionState(transId)


        //const extraList = axDatasourceObj[extraSourceKey];

        //if (extraList && extraList.length > 0) {
        //    fieldName = extraList[0].fname ?? extraList[0].keyfield ?? extraList[0].name ?? extraList[0].displaydata ?? null;
        //} else {
        //    console.warn("Hidden field name not found in cache");
        //}

        rawFieldValue = cleanCommandToken(tokens[fieldValueIndex]);
        // rawFieldName = cleanCommandToken(tokens[fieldValueIndex - 1]); 
        // fieldValue = tryResolveToken(fieldValueIndex, rawFieldValue, commandConfig, false);
        const { value } = tryResolveToken(fieldValueIndex, rawFieldValue, commandConfig, false);
        // const { value: fieldname } = tryResolveToken(fieldValueIndex - 1, rawFieldName, commandConfig, false);
        fieldValue = value;
        fieldUniqueId = getUniqueId(fieldValue);


        console.log(
            `view Data ? TStruct=${transId}, Field=${primaryField}, Value=${fieldValue}`
        );

        handler({
            transId,
            fieldName: primaryField,
            fieldValue: fieldUniqueId,
            rawStruct
        })

    }



    // function handleViewCommand({ tokens, commandConfig }) {

    //     let transId = "";
    //     let type = "";
    //     let fieldName;
    //     let fieldValue;
    //     let rawFieldName;
    //     let rawFieldValue;
    //     let fieldUniqueId;
    //     let fieldValueIndex = 0;


    //     if (tokens.length < 2) {
    //         console.warn("View Command required atleast two tokens");
    //         showToast("view command requires atleast two tokens");
    //         return;
    //     }

    //     console.log(JSON.stringify(commandConfig));


    //     const promptValues = commandConfig?.prompts?.[0].promptValues;
    //     const viewDataSource = commandConfig?.prompts?.[0].promptSource;
    //     const extraDataSource = commandConfig?.prompts?.[1].extraParams;


    //     const viewDataSourceKey = `${viewDataSource}`.toLowerCase();
    //     let rawStruct = cleanCommandToken(tokens[1]);
    //     transId = tryResolveToken(1, rawStruct, commandConfig, false);


    //     type = getType(viewDataSourceKey, transId, promptValues, tokens, commandConfig);

    //     const handler = VIEW_HANDLERS[type];




    //     if (!handler) {
    //         console.log("Error: Unsupported View Type");
    //         showToast("Error: Unsupported View Type");
    //         return;
    //     }


    //     if (type === "ads") {


    //         const adsName = cleanCommandToken(tokens[1]);
    //         const filters = extractAdsFilters(tokens);

    //         console.log("Ads Filters: ", filters);



    //         redirectToSmartView({
    //             adsName: adsName,
    //             filters: filters
    //         });
    //         return;


    //     } else if (type === "page") {





    //         let rawFieldValue = cleanCommandToken(tokens[1]);
    //         redirectToHtmlPages(rawFieldValue, tokens, commandConfig);
    //         return;

    //     }


    //     const extraSourceKey = `${extraDataSource}_${transId}`.toLowerCase();


    //     if (tokens.length > 3) {
    //         fieldValueIndex = 3;


    //     } else {
    //         fieldValueIndex = 2;


    //     }




    //     const extraList = axDatasourceObj[extraSourceKey];

    //     if (extraList && extraList.length > 0) {
    //         fieldName = extraList[0].fname ?? extraList[0].keyfield ?? extraList[0].name ?? extraList[0].displaydata ?? null;
    //     } else {
    //         console.warn("Hidden field name not found in cache");
    //     }

    //     rawFieldValue = cleanCommandToken(tokens[fieldValueIndex]);
    //     fieldValue = tryResolveToken(fieldValueIndex, rawFieldValue, commandConfig, false);
    //     fieldUniqueId = getUniqueId(fieldValue);


    //     console.log(
    //         `view Data ? TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`
    //     );

    //     handler({
    //         transId,
    //         fieldName,
    //         fieldValue: fieldUniqueId,
    //         rawStruct
    //     })

    // }


    async function handleKeyfield({ tokens, commandConfig }) {

        const tstructName = cleanString(tokens[2]);
        const keyField = cleanString(tokens[3]);
        const { value: actualFieldName } = tryResolveToken(3, keyField, commandConfig, false);
        const { value: transId } = tryResolveToken(2, tstructName, commandConfig, false);
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

    // function getType(axDatasourceKey, text, paramValuesCsv, tokens, commandConfig) {
    //     const paramList = paramValuesCsv?.split(",").map(v => v.trim().toLowerCase()).filter(Boolean);
    //     const VALID_TYPES = new Set(paramList);

    //     let paramValue;
    //     // if (axDatasourceKey.toLowerCase() === "axi_viewlist") {

    //     if (axDatasourceKey.toLowerCase() === "axi_structmetalist") {
    //         paramValue = processExtraParams(tokens, commandConfig);
    //         axDatasourceKey += "_" + paramValue.toLowerCase();
    //     }



    //     const data = axDatasourceObj?.[axDatasourceKey];

    //     if (!data || !Array.isArray(data)) return null;

    //     console.log(JSON.stringify(data));

    //     const resolvedText = typeof text === 'object' ? (text?.value || "") : (text || "");

    //     const resolvedType =
    //         typeof text === "object"
    //             ? (text?.type || "")
    //             : "";



    //     // const normalizedText = text.trim().toLowerCase();
    //     const normalizedText = resolvedText.trim().toLowerCase();

    //     // const rawTokenText = tokens[1] ? cleanCommandToken(tokens[1]).toLowerCase() : normalizedText;
    //     const rawTokenText =
    //         normalizedText ||
    //         (tokens?.[1]
    //             ? cleanCommandToken(tokens[1]).toLowerCase()
    //             : "");

    //     let bestMatch = null;

    //     // const item = data?.find(d => {
    //     //     if (typeof d.displaydata !== "string") return false;

    //     //     if (d.name && d.name.toLowerCase() === normalizedText) {
    //     //         return true;
    //     //     }


    //     //     const pureCaption = d.displaydata
    //     //         .replace(/\s*\(.*?\)\s*(?=\[[^\]]+\]$)/, "")
    //     //         .replace(/\s*\[[^\]]+\]\s*$/, "")
    //     //         .trim()
    //     //         .toLowerCase();

    //     //     return pureCaption === normalizedText;
    //     // });

    //     bestMatch = data.find(d => typeof d.displaydata === "string" && d.displaydata.toLowerCase() === rawTokenText.toLowerCase());

    //     if (!bestMatch) {
    //         bestMatch = data.find(d => d.name && d.name.toLowerCase() === normalizedText);
    //     }

    //     if (!bestMatch) {
    //         bestMatch = data.find(d => {
    //             if (typeof d.displaydata !== "string") return false;

    //             const pureCaption = d.displaydata
    //                 .replace(/\s*\(.*?\)\s*(?=\[[^\]]+\]$)/, "")
    //                 .replace(/\s*\[[^\]]+\]\s*$/, "")
    //                 .trim()
    //                 .toLowerCase();

    //             return pureCaption === normalizedText || pureCaption === rawTokenText;
    //         });
    //     }

    //     // if (!bestMatch || typeof bestMatch.displaydata !== "string") {
    //     //     return null;
    //     // }

    //     // const matches = [...bestMatch.displaydata.matchAll(/\[([^\]]+)\]/g)];

    //     // if (matches.length === 0) {
    //     //     return null;
    //     // }

    //     // const candidate = matches[matches.length - 1][1].toLowerCase();

    //     // return VALID_TYPES.has(candidate) ? candidate : null;

    //     if (!bestMatch)
    //         return resolvedType || "";

    //     let candidate = "";


    //     if (bestMatch.stype) {

    //         const stypeMap = {
    //             t: "tstruct",
    //             i: "iview",
    //             ads: "ads",
    //             p: "page"
    //         };

    //         candidate =
    //             stypeMap[
    //             bestMatch.stype
    //                 .toLowerCase()
    //             ] || "";
    //     }


    //     if (
    //         !candidate &&
    //         typeof bestMatch.displaydata === "string"
    //     ) {

    //         const matches = [
    //             ...bestMatch.displaydata.matchAll(/\[([^\]]+)\]/g)
    //         ];

    //         if (matches.length > 0) {
    //             candidate =
    //                 matches[
    //                     matches.length - 1
    //                 ][1].toLowerCase();
    //         }
    //     }


    //     if (!candidate) {
    //         candidate = resolvedType || "";
    //     }

    //     return VALID_TYPES.has(candidate)
    //         ? candidate
    //         : "";


    // }

    function getType(axDatasourceKey, text, paramValuesCsv, tokens, commandConfig) {

        // -----------------------------------
        // VALID TYPES FROM promptValues
        // Example:
        // tstruct,iview,ads,page
        // -----------------------------------
        const paramList = paramValuesCsv
            ?.split(",")
            .map(v => v.trim().toLowerCase())
            .filter(Boolean);

        const VALID_TYPES = new Set(paramList);
        VALID_TYPES.add("inbox");
        VALID_TYPES.add("page");

        let paramValue;

        // -----------------------------------
        // HANDLE axi_structmetalist
        // -----------------------------------
        if (
            axDatasourceKey?.toLowerCase() ===
            "axi_structmetalist"
        ) {
            paramValue = processExtraParams(
                tokens,
                commandConfig
            );

            axDatasourceKey +=
                "_" + paramValue.toLowerCase();
        }

        const data =
            axDatasourceObj?.[axDatasourceKey];

        if (!Array.isArray(data))
            return "";

        console.log(JSON.stringify(data));

        // -----------------------------------
        // text may be:
        //
        // "slord"
        //
        // OR
        //
        // {
        //   value:"slord",
        //   type:"i"
        // }
        // -----------------------------------
        const resolvedText =
            typeof text === "object"
                ? (text?.value || "")
                : (text || "");

        const resolvedType =
            typeof text === "object"
                ? (text?.type || "")
                    .trim()
                    .toLowerCase()
                : "";

        const normalizedText =
            resolvedText
                .trim()
                .toLowerCase();

        let bestMatch = null;

        // -----------------------------------
        // stype -> actual type
        // -----------------------------------
        const stypeMap = {
            t: "tstruct",
            tstruct: "tstruct",
            i: "iview",
            iview: "iview",
            ads: "ads",
            p: "page",
            page: "page",
            inbox: "inbox"
        };

        // -----------------------------------
        // normalize resolvedType
        //
        // i -> iview
        // t -> tstruct
        // -----------------------------------
        let normalizedResolvedType =
            stypeMap[
            resolvedType
            ] || resolvedType;

        if (!normalizedResolvedType && normalizedText === "inbox") {
            normalizedResolvedType = "inbox";
        }

        // ===================================
        // STRICT TYPE MODE
        //
        // resolvedType exists
        //
        // 1. find ALL text matches
        // 2. then match TYPE
        //
        // handles:
        //
        // slord[t]
        // slord[i]
        // ===================================
        if (normalizedResolvedType) {

            // -----------------------------
            // TEXT MATCHES FIRST
            // -----------------------------
            const matchingRows =
                data.filter(d => {

                    const displayMatch =
                        typeof d.displaydata ===
                        "string" &&
                        d.displaydata
                            .toLowerCase() ===
                        normalizedText;

                    const nameMatch =
                        d.name &&
                        d.name
                            .toLowerCase() ===
                        normalizedText;

                    const captionMatch =
                        typeof d.displaydata ===
                        "string" &&
                        d.displaydata
                            .replace(
                                /\s*\(.*?\)\s*(?=\[[^\]]+\]$)/,
                                ""
                            )
                            .replace(
                                /\s*\[[^\]]+\]\s*$/,
                                ""
                            )
                            .trim()
                            .toLowerCase() ===
                        normalizedText;

                    return (
                        displayMatch ||
                        nameMatch ||
                        captionMatch
                    );
                });

            // -----------------------------
            // THEN TYPE MATCH
            // -----------------------------
            bestMatch =
                matchingRows.find(d => {

                    let itemType = "";

                    // stype priority
                    if (d.stype) {
                        itemType =
                            stypeMap[
                            d.stype
                                .toLowerCase()
                            ] || "";
                    }

                    // [type] fallback
                    if (
                        !itemType &&
                        typeof d.displaydata ===
                        "string"
                    ) {

                        const matches = [
                            ...d.displaydata.matchAll(
                                /\[([^\]]+)\]/g
                            )
                        ];

                        if (
                            matches.length > 0
                        ) {

                            itemType =
                                matches[
                                    matches.length - 1
                                ][1]
                                    .toLowerCase();
                        }
                    }

                    // compare type
                    return (
                        itemType ===
                        normalizedResolvedType
                    );
                });

            // -----------------------------
            // STRICT MODE
            //
            // no fallback to first slord
            // -----------------------------
            if (bestMatch) {

                let candidate = "";

                if (bestMatch.stype) {
                    candidate =
                        stypeMap[
                        bestMatch.stype
                            .toLowerCase()
                        ] || "";
                }

                if (isInboxStructure(bestMatch)) {
                    candidate = "inbox";
                }

                if (
                    !candidate &&
                    typeof bestMatch.displaydata ===
                    "string"
                ) {

                    const matches = [
                        ...bestMatch.displaydata.matchAll(
                            /\[([^\]]+)\]/g
                        )
                    ];

                    if (
                        matches.length > 0
                    ) {

                        candidate =
                            matches[
                                matches.length - 1
                            ][1]
                                .toLowerCase();
                    }
                }

                return VALID_TYPES.has(candidate)
                    ? candidate
                    : "";
            }

            return VALID_TYPES.has(
                normalizedResolvedType
            )
                ? normalizedResolvedType
                : "";
        }

        // ===================================
        // NORMAL MODE
        //
        // resolvedType empty
        // old behavior
        // ===================================

        // displaydata
        if (!bestMatch) {
            bestMatch = data.find(d =>
                typeof d.displaydata ===
                "string" &&
                d.displaydata
                    .toLowerCase() ===
                normalizedText
            );
        }

        // name
        if (!bestMatch) {
            bestMatch = data.find(d =>
                d.name &&
                d.name
                    .toLowerCase() ===
                normalizedText
            );
        }

        // caption
        if (!bestMatch) {
            bestMatch = data.find(d => {

                if (
                    typeof d.displaydata !==
                    "string"
                )
                    return false;

                const pureCaption =
                    d.displaydata
                        .replace(
                            /\s*\(.*?\)\s*(?=\[[^\]]+\]$)/,
                            ""
                        )
                        .replace(
                            /\s*\[[^\]]+\]\s*$/,
                            ""
                        )
                        .trim()
                        .toLowerCase();

                return (
                    pureCaption ===
                    normalizedText
                );
            });
        }

        if (!bestMatch)
            return "";

        // ===================================
        // FINAL TYPE RESOLUTION
        // ===================================
        let candidate = "";

        // stype priority
        if (bestMatch.stype) {
            candidate =
                stypeMap[
                bestMatch.stype
                    .toLowerCase()
                ] || "";
        }
        if (isInboxStructure(bestMatch)) {
            candidate = "inbox";
        }

        // [type] fallback
        if (
            !candidate &&
            typeof bestMatch.displaydata ===
            "string"
        ) {

            const matches = [
                ...bestMatch.displaydata.matchAll(
                    /\[([^\]]+)\]/g
                )
            ];

            if (matches.length > 0) {

                candidate =
                    matches[
                        matches.length - 1
                    ][1]
                        .toLowerCase();
            }
        }

        return VALID_TYPES.has(candidate)
            ? candidate
            : "";
    }


    function redirectToHtmlPages(text, tokens, commandConfig) {

        let paramValue = processExtraParams(tokens, commandConfig);

        //const viewList = axDatasourceObj["axi_viewlist".toLowerCase() + "_" + paramValue];
        const viewList = axDatasourceObj["axi_structmetalist".toLowerCase() + "_" + paramValue.toLowerCase()];



        // const item = viewList.find(v => v.displaydata.includes(text));
        const normalizedText = text.trim().toLowerCase();

        const rawTokenText = tokens[1] ? cleanCommandToken(tokens[1]).toLowerCase() : normalizedText;

        let bestMatch = null;

        // const item = data?.find(d => {
        //     if (typeof d.displaydata !== "string") return false;

        //     if (d.name && d.name.toLowerCase() === normalizedText) {
        //         return true;
        //     }


        //     const pureCaption = d.displaydata
        //         .replace(/\s*\(.*?\)\s*(?=\[[^\]]+\]$)/, "")
        //         .replace(/\s*\[[^\]]+\]\s*$/, "")
        //         .trim()
        //         .toLowerCase();

        //     return pureCaption === normalizedText;
        // });

        bestMatch = viewList.find(d => {
            const isPageType = ['p', 'page'].includes((d.stype || "").toLowerCase());
            return isPageType && typeof d.displaydata === "string" && d.displaydata.toLowerCase() === rawTokenText.toLowerCase();
        });

        if (!bestMatch) {
            bestMatch = viewList.find(d => {
                const isPageType = ['p', 'page'].includes((d.stype || "").toLowerCase());
                if (!isPageType) return false;

                if (d.name && d.name.toLowerCase() === rawTokenText.toLowerCase()) {
                    return true;
                }
                if (typeof d.displaydata !== "string") return false;

                const pureCaption = d.displaydata
                    .replace(/\s*\(.*?\)\s*(?=\[[^\]]+\]$)/, "")
                    .replace(/\s*\[[^\]]+\]\s*$/, "")
                    .trim()
                    .toLowerCase();

                return (pureCaption === normalizedText || pureCaption === rawTokenText);
            });
        }

        if (!bestMatch || typeof bestMatch.displaydata !== "string") {
            return null;
        }

        const requestUrl = bestMatch.name;
        console.log(requestUrl);

        setCommandRoutes(input.value.trim(), requestUrl);

        if (popUpOption) {

            openPopOption(requestUrl + `&caption=${encodeURIComponent(bestMatch.caption)}`)
        }
        else {
            window.LoadIframe(requestUrl);
        }


    }

    /***************************************************
* OPEN COMMAND FUNCTION
* **************************************************
*/

    function handleOpenSource({ tokens, commandConfig }) {


        const type = cleanCommandToken(tokens[1]);
        let rawName = cleanCommandToken(tokens[2]);

        let { value: resolvedName } = tryResolveToken(2, rawName, commandConfig, false);


        if (resolvedName === rawName) {
            const listKey =
                type.toLowerCase() === "tstruct"
                    ? "Axi_TStructList".toLowerCase()
                    : type.toLowerCase() === "iview"
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
        const targetUrl = `developerstudio:${type.toLowerCase()}:${resolvedName}`;

        setCommandRoutes(input.value.trim(), targetUrl);

        if (type.toLowerCase() === "tstruct") {
            window.openDeveloperStudio("tstreact", resolvedName, true);
        } else if (type.toLowerCase() === "iview") {
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
            // paramName = tryResolveToken(2, rawName, commandConfig, false);
            const { value } = tryResolveToken(2, rawName, commandConfig, false);
            paramName = value;

        }



        setEditSessionState(transId);



        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            setCommandRoutes(input.value.trim(), targetUrl);

            redirectToIView(iviewName);


        } else {
            targetUrl += `&${fieldname}=${encodeURIComponent(paramName)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false?";
            setCommandRoutes(input.value.trim(), targetUrl);

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
            // paramName = tryResolveToken(2, rawName, commandConfig, false);
            const { value, type } = tryResolveToken(2, rawName, commandConfig, false);
            paramName = value;

        }


        setEditSessionState(transId);
        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            setCommandRoutes(input.value.trim(), targetUrl);

            window.LoadIframe(targetUrl);

        } else {

            targetUrl += `&${fieldname}=${encodeURIComponent(paramName)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false?";
            setCommandRoutes(input.value.trim(), targetUrl);

            window.LoadIframe(targetUrl);

        }
    }

    function handleOpenPage({ tokens, commandConfig }) {

        let targetUrl;
        let paramName;
        let transId = "sect";
        let fieldname = "caption";
        let rawName = cleanCommandToken(tokens[2]);


        // if (rawName) {
        //     paramName = tryResolveToken(2, rawName, commandConfig, false);

        // }

        setEditSessionState(transId);

        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!rawName) {
            setCommandRoutes(input.value.trim(), targetUrl);
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldname}=${encodeURIComponent(rawName)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false?";
            setCommandRoutes(input.value.trim(), targetUrl);

            window.LoadIframe(targetUrl);

        }



    }

    function handleOpenAppVar({ tokens, commandConfig }) {
        const targetUrl = "../aspx/tstruct.aspx?transid=axvar";
        setCommandRoutes(input.value.trim(), targetUrl);
        window.LoadIframe(targetUrl);
        //  window.openDeveloperStudio("tstreact", "axvar" , true);

    }

    function handleOpenDevOptions({ tokens, commandConfig }) {
        const targetUrl = "../aspx/tstruct.aspx?transid=axstc";
        setCommandRoutes(input.value.trim(), targetUrl);

        window.LoadIframe(targetUrl);

    }

    function handleOpenDbConsole() {
        // Task Axi-0034 completed
        // window.openDeveloperStudio("AxDBScript.aspx");
        const targetUrl = "../aspx/AxDBScript.aspx";
        setCommandRoutes(input.value.trim(), targetUrl);
        window.LoadIframe(targetUrl);

    }

    function handleOpenArrangeMenu() {
        // ArrangeMenu.aspx
        const targetUrl = "../aspx/ArrangeMenu.aspx";
        setCommandRoutes(input.value.trim(), targetUrl);
        window.LoadIframe(targetUrl);
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
        if (tokens.length < 2) {
            showToast("Invalid Command! Run commands requires atleast 2 tokens!");
            return;
        }
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
                    
                    if (structType === "i") {
                        const utilityButtons = getIViewUtilityButtons();
                        const actionButtons = getIViewActionDropdownButtons();
                        allButtons = [...allButtons, ...Object.values(utilityButtons), ...Object.values(actionButtons)];
                    }
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

            case "s":
                if (!settingsPageButtons) settingsPageButtons = getButtons(".Config-cont");
                allButtons = [...Object.values(settingsPageButtons)]
                break;

            case "im":
            case "ex":
                if (!importExportButtons) importExportButtons = getButtons(".card-footer");
                allButtons = [...Object.values(importExportButtons)];
                break;


            default:
                console.error("Invalid StructType")
                break;
        }

        console.log("All Buttons: " + JSON.stringify(allButtons));

        let { value: resolvedBtnId, type } = tryResolveToken(1, buttonLabel, commandConfig, false);

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
                const normalizedInputLabel = buttonLabel.toLowerCase().replace(/[\r\n\t]+/g, ' ').trim();

                const rawLabel = btn.label || "";
                const normalizedBtnLabel = rawLabel.toLowerCase().replace(/[\r\n\t]+/g, ' ').trim();
                if (normalizedBtnLabel === normalizedInputLabel) return true;

                const element = btn.element;
                if (element) {
                    const titleAttr = (element.getAttribute("title") || "").toLowerCase().replace(/[\r\n\t]+/g, ' ').trim();
                    if (titleAttr === normalizedInputLabel) return true;

                    const textContentVal = (element.textContent || "").toLowerCase().replace(/[\r\n\t]+/g, ' ').trim();
                    if (textContentVal === normalizedInputLabel) return true;
                }

                return false;
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

    //function resolveLikeOperator(rawValue) {

    //    if (!rawValue.includes("%")) {
    //        return { operator: "equal", value: rawValue };
    //    }

    //    const startsWithPercent = rawValue.startsWith("%");
    //    const endsWithPercent = rawValue.endsWith("%");

    //    if (startsWithPercent && endsWithPercent) {
    //        return {
    //            operator: "contains",
    //            value: rawValue.slice(1, -1)
    //        };
    //    }

    //    if (startsWithPercent) {
    //        return {
    //            operator: "endswith",
    //            value: rawValue.slice(1)
    //        };
    //    }

    //    if (endsWithPercent) {
    //        return {
    //            operator: "startswith",
    //            value: rawValue.slice(0, -1)
    //        };
    //    }

    //    return { operator: "equal", value: rawValue };
    //}

    function extractAdsFilters(tokens) {

        const filters = [];
        let i = 2;

        while (i < tokens.length) {

            const rawColToken = cleanCommandToken(tokens[i]);
            if (!rawColToken) {
                i++;
                continue;
            }

            let nextTokenRaw = cleanCommandToken(tokens[i + 1] || "");

            let operator = "";
            let rawValue = "";


            let matchedOperator = null;

            for (let op of OPERATORS_LIST) {
                if (nextTokenRaw.startsWith(op)) {
                    matchedOperator = op;
                    break;
                }
            }


            let colMetadata = adsfieldvalueanddt[rawColToken] || {};

            if (matchedOperator && colMetadata.datatype != "c" && colMetadata.datatype != 't' && colMetadata?.datatype != "d") {

                operator = matchedOperator;
                rawValue = nextTokenRaw.slice(matchedOperator.length);
                i += 2;

            }
            else {

                rawValue = nextTokenRaw;

                if (colMetadata?.datatype === "c" || colMetadata.datatype === 't' || colMetadata.datatype === "d") {
                    if (rawValue.startsWith("%") && rawValue.endsWith("%")) {
                        operator = "contains";
                        rawValue = rawValue.slice(1, -1);
                    }
                    else if (rawValue.endsWith("%")) {
                        operator = "startswith";
                        rawValue = rawValue.slice(0, -1);
                    }
                    else if (rawValue.startsWith("%")) {
                        operator = "endswith";
                        rawValue = rawValue.slice(1);
                    }
                    else {
                        operator = "equal";
                        if (matchedOperator)
                            rawValue = nextTokenRaw.slice(matchedOperator.length);
                    }
                }
                else {
                    operator = "equal";
                }

                i += 2;
            }

            colMetadata = adsfieldvalueanddt[rawColToken] || {};

            if (colMetadata?.datatype === "d") {
                rawValue = normalizeDate(rawValue);
            }

            let filterDatatype = colMetadata.datatype;
            if (rawColToken && rawColToken.includes(",")) {
                filterDatatype = rawColToken.split(",")
                    .map(colName => adsfieldvalueanddt[colName.trim()]?.datatype || "c")
                    .join(",");
            }

            filters.push({
                field: rawColToken,
                operator: operator,
                value: rawValue,
                datatype: filterDatatype,
                isAccept: colMetadata.isAccept
            });
        }

        return filters;
    }
    //function extractAdsFilters(tokens) {
    //    const filters = [];


    //    let i = 2;

    //    while (i < tokens.length) {

    //        const rawColToken = cleanCommandToken(tokens[i]);
    //        if (!rawColToken) { i++; continue; }


    //        let nextTokenRaw = cleanCommandToken(tokens[i + 1] || "");

    //        let operator = "=";
    //        let valueTokenIndex = -1;
    //        let rawValue = "";

    //        if (OPERATORS_SET.has(nextTokenRaw)) {

    //            operator = nextTokenRaw;
    //            rawValue = cleanCommandToken(tokens[i + 2] || "");
    //            valueTokenIndex = i + 2;


    //            i += 3;
    //        } else {

    //            operator = "=";
    //            rawValue = nextTokenRaw;
    //            valueTokenIndex = i + 1;


    //            i += 2;
    //        }

    //        const colMetadata = adsfieldvalueanddt[rawColToken] || {};

    //        if (colMetadata?.datatype === "d") {
    //            rawValue = normalizeDate(rawValue);
    //        }

    //        filters.push({
    //            field: rawColToken,
    //            operator: operator,
    //            value: rawValue,
    //            datatype: colMetadata.datatype,
    //            isAccept: colMetadata.isAccept
    //        });
    //    }

    //    return filters;
    //}



    function getIViewUtilityButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        let doc = null;
        try {
            doc = iframe.contentDocument || iframe.contentWindow?.document;
        } catch (e) {
            console.warn("Axi: Blocked from accessing iframe content due to cross-origin restriction:", e);
        }
        if (!doc) return {};

        const searchElem = doc.getElementById("iconsNewSearch") || doc.querySelector("#iconsNewSearch");
        const refreshElem = doc.getElementById("iconsNewRefresh") || doc.querySelector("#iconsNewRefresh") || doc.getElementById("iviewRefresh") || doc.querySelector(".iviewRefresh");
        const refreshParamElem = doc.getElementById("dvRefreshParam") || doc.querySelector("#dvRefreshParam");
        const refreshParamIconElem = doc.getElementById("dvRefreshParamIcon") || doc.querySelector("#dvRefreshParamIcon");
        const utilityContainer = doc.getElementById("iconsNewUtility") || doc.querySelector("#iconsNewUtility");
        
        const containers = [utilityContainer, searchElem, refreshElem, refreshParamElem, refreshParamIconElem].filter(Boolean);
        if (containers.length === 0) return {};

        const links = [];
        containers.forEach((container) => {
            if (container.matches("a, button, [onclick], [title]")) {
                links.push(container);
            }
            const children = Array.from(container.querySelectorAll("a, button, [onclick]"));
            children.forEach(child => {
                if (!links.includes(child)) links.push(child);
            });
        });

        if (links.length === 0) return {};

        const result = {};
        links.forEach((link) => {
            const title = link.getAttribute("title") || link.parentElement?.getAttribute("title") || "";
            let label = title.trim();
            if (!label) {
                const nameSpan = link.querySelector(".dropdownIconName");
                if (nameSpan) {
                    label = nameSpan.textContent.trim();
                } else {
                    const cloned = link.cloneNode(true);
                    cloned.querySelectorAll(".material-icons, .material-icons-style").forEach(node => node.remove());
                    label = (cloned.textContent || "").trim();
                }
            }
            label = label.replace(/[\r\n\t]+/g, ' ').replace(/\s+/g, ' ').trim();
            if (!label) return;

            const id = link.id || link.getAttribute("id") || `utility_${label.toLowerCase().replace(/\s+/g, '_')}`;
            if (result[id]) return;

            result[id] = {
                id,
                label,
                element: link,
                click: () => {
                    const btnInside = link.querySelector("button, a, [onclick]") || link;
                    if (btnInside.href && btnInside.href.startsWith("javascript:")) {
                        try {
                            const jsCode = decodeURIComponent(btnInside.href.replace(/^javascript:/i, ''));
                            iframe.contentWindow.eval(jsCode);
                        } catch (e) {
                            console.error("Failed to execute javascript: href for utility link", e);
                            btnInside.click();
                        }
                    } else {
                        btnInside.click();
                        if (btnInside !== link) {
                            try { link.click(); } catch(e) {}
                        }
                    }
                }
            };
        });

        return result;
    }

    function getIViewActionDropdownButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        let doc = null;
        try {
            doc = iframe.contentDocument || iframe.contentWindow?.document;
        } catch (e) {
            console.warn("Axi: Blocked from accessing iframe content due to cross-origin restriction:", e);
        }
        if (!doc) return {};

        const items = Array.from(doc.querySelectorAll(".dropDownButton__list li.dropDownButton__item, li.dropDownButton__item"));
        if (items.length === 0) return {};

        const result = {};
        items.forEach((item) => {
            const dropdownVal = item.getAttribute("data-dropdown-value") || "";
            if (dropdownVal !== "chart" && dropdownVal !== "saveAs") return;

            const link = item.querySelector("a") || item;
            
            let title = item.getAttribute("title") || link.getAttribute("title") || "";
            let label = title.trim();
            if (!label) {
                const tempNode = link.cloneNode(true);
                const icons = tempNode.querySelectorAll("span, i, .material-icons");
                icons.forEach(node => node.remove());
                label = tempNode.textContent.trim();
            }
            label = label.replace(/[\r\n\t]+/g, ' ').replace(/\s+/g, ' ').trim();
            if (!label) return;

            const id = item.id || link.id || `action_${dropdownVal}`;

            result[id] = {
                id,
                label,
                element: item,
                click: () => {
                    item.click();
                }
            };
        });

        return result;
    }

    function getBottomToolbarButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const toolbar = doc.querySelector(".BottomToolbarBar");
        if (!toolbar) return {};

        const buttons = Array.from(toolbar.querySelectorAll("a, button, input[type='button'], input[type='submit'], div[data-kt-menu-trigger], div.btn"));
        if (buttons.length === 0) return {};

        const result = {};

        buttons.forEach((btn) => {
            // Check if element or its parent is hidden
            if (btn.offsetParent === null) return;
            if (btn.classList.contains("d-none") || btn.closest(".d-none") || btn.closest("[style*='display: none']")) return;

            const id = btn.id || btn.getAttribute("data-id") || btn.getAttribute("data-extra") || btn.getAttribute("title") || btn.getAttribute("onclick");
            if (!id) return;
            if (id === "ivirActionButton") return;

            const label = extractButtonLabel(btn);
            if (!label) console.log("There is no label for Element: " + btn);
            if (label.toLowerCase() === "plugin custom code") return;
            if (label.toLowerCase() === "data" || btn.classList.contains("js-dropdown") || btn.classList.contains("ivirActionDrpDwn")) return;



            result[id] = {
                id,
                label,
                element: btn,
                click: () => btn.click()
            };
        });

        return result;
    }

    function isPreviewModalOpen() {
        // Check if current window or frameElement indicates it is inside the loadPopUpPage modal or middle1 frame
        try {
            if (window.frameElement) {
                const el = window.frameElement;
                if (el.id === "loadPopUpPage" || el.name === "loadPopUpPage" || el.id === "middle1" || el.name === "middle1" || el.classList.contains("middle") || el.classList.contains("middle1")) {
                    console.log("Axi: running inside loadPopUpPage or middle1 iframe");
                    return true;
                }
            }
            if (window.name === "loadPopUpPage" || window.name === "middle" || window.name === "middle1") {
                console.log("Axi: running inside frame with name loadPopUpPage, middle or middle1");
                return true;
            }
        } catch (e) {
            // Ignore cross-origin errors
        }

        let rootWin = window;
        try {
            if (window.top && window.top.document) {
                rootWin = window.top;
            }
        } catch (e) {
            console.warn("Axi: Failed to access window.top due to cross-origin boundary.");
        }

        function checkWindow(win) {
            try {
                if (!win || !win.document) return false;

                // Check for loadPopUpPage modal
                const loadPopUpPageModal = win.document.getElementById("loadPopUpPage");
                if (loadPopUpPageModal) {
                    if (loadPopUpPageModal.offsetWidth > 0 || loadPopUpPageModal.offsetHeight > 0 || loadPopUpPageModal.style.display === "block" || loadPopUpPageModal.classList.contains("show")) {
                        console.log("Axi: found active preview modal via loadPopUpPage");
                        return true;
                    }
                }

                // Check inside middle1 iframe document
                const middleIframe = win.document.getElementById("middle1");
                if (middleIframe) {
                    const middleDoc = middleIframe.contentDocument || middleIframe.contentWindow?.document;
                    if (middleDoc) {
                        const innerModal = middleDoc.getElementById("loadPopUpPage");
                        if (innerModal) {
                            if (innerModal.offsetWidth > 0 || innerModal.offsetHeight > 0 || innerModal.style.display === "block" || innerModal.classList.contains("show")) {
                                console.log("Axi: found active preview modal inside middle1 iframe");
                                return true;
                            }
                        }
                    }
                }

                // 1. Check for remodal wrapper containing the popupIframeRemodal
                const openedModal = win.document.querySelector(".remodal-wrapper.remodal-is-opened #popupIframeRemodal");
                if (openedModal) {
                    console.log("Axi: found active preview modal via remodal selector");
                    return true;
                }

                // 2. Check for remodal wrapper containing any htmlPages.aspx iframe
                const htmlPagesModal = win.document.querySelector(".remodal-wrapper.remodal-is-opened iframe[src*='htmlPages.aspx']");
                if (htmlPagesModal) {
                    console.log("Axi: found active preview modal via htmlPages src selector");
                    return true;
                }

                // 3. Fallback check for popupIframeRemodal direct element
                const modal = win.document.getElementById("popupIframeRemodal");
                if (modal) {
                    const wrapper = win.document.getElementById("axpertPopupWrapper") || win.document.querySelector("[data-remodal-id=axpertPopupModal]");
                    if (wrapper) {
                        if (wrapper.offsetWidth > 0 || wrapper.offsetHeight > 0 || wrapper.classList.contains("remodal-is-opened")) {
                            console.log("Axi: found active preview modal via wrapper visibility/class");
                            return true;
                        }
                    } else if (modal.offsetWidth > 0 || modal.offsetHeight > 0) {
                        console.log("Axi: found active preview modal via modal direct visibility");
                        return true;
                    }
                }

                // 4. Recursively check iframes
                const frames = win.document.querySelectorAll("iframe");
                for (let i = 0; i < frames.length; i++) {
                    const frameWin = frames[i].contentWindow;
                    if (frameWin && checkWindow(frameWin)) {
                        return true;
                    }
                }
            } catch (err) {
                // Ignore cross-origin error on frame access
            }
            return false;
        }

        const result = checkWindow(rootWin);
        console.log("Axi: isPreviewModalOpen returning:", result);
        return result;
    }

    function isViewDesignerModalOpen() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return false;

        let doc = null;
        try {
            doc = iframe.contentDocument || iframe.contentWindow?.document;
        } catch (e) {
            // Ignore cross-origin error
        }
        if (!doc) return false;

        const modal = doc.getElementById("newViewTabId") || doc.querySelector("#newViewTabId");
        if (modal) {
            return modal.offsetWidth > 0 || modal.offsetHeight > 0 || modal.style.display === "block" || modal.classList.contains("show");
        }
        return false;
    }

    function isRunDisabledForPage(src) {
        if (!src) {
            const iframe = document.getElementById("middle1");
            if (!iframe) return false;
            src = iframe.getAttribute("src") || "";
        }
        const srcLower = src.toLowerCase();

        // 1. Dev Studio check
        if (srcLower.includes("qadev") || srcLower.includes("adinfo=") || srcLower.includes("adinfo%3d") || srcLower.includes("axidev")) {
            return true;
        }

        // 2. Specific non-runnable pages check
        if (srcLower.includes("arrangemenu.aspx")) return true;
        if (srcLower.includes("axdbscript.aspx")) return true;
        if (srcLower.includes("tstruct.aspx") && srcLower.includes("transid=ad_lg")) return true;
        if (srcLower.includes("iview.aspx") && srcLower.includes("ivname=inmemdb")) return true;
        if (srcLower.includes("processflow.aspx") && (srcLower.includes("dashboard=t") || srcLower.includes("calendar=t"))) return true;

        return false;
    }

    function getTopToolbarButtons() {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const toolbar = doc.querySelector(".toolbarRightMenu");
        if (!toolbar) return {};

        const buttons = toolbar.querySelectorAll("a, button, input[type='button'], input[type='submit'], div[data-kt-menu-trigger], div.btn");

        const result = {};

        buttons.forEach((btn) => {
            // Check if element or its parent is hidden
            if (btn.offsetParent === null) return;
            if (btn.classList.contains("d-none") || btn.closest(".d-none") || btn.closest("[style*='display: none']") || btn.closest(".hidden") || btn.closest("[style*='display:none']")) return;

            const id = btn.id || btn.getAttribute("data-id") || btn.getAttribute("data-extra") || btn.getAttribute("title") || btn.getAttribute("onclick");
            if (!id) return;
            if (id === "ivirActionButton") return;

            const label = extractButtonLabel(btn);
            if (!label) console.log("There is no label for Element: " + btn);
            if (label.toLowerCase() === "plugin custom code") return;
            if (label.toLowerCase() === "data" || btn.classList.contains("js-dropdown") || btn.classList.contains("ivirActionDrpDwn")) return;



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
            if (label.toLowerCase() === "plugin custom code") return;



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

        let iframeDoc = null;
        try {
            iframeDoc = iframe.contentDocument || iframe.contentWindow?.document;
        } catch (e) {
            console.warn("Axi: Blocked from accessing iframe content due to cross-origin restriction:", e);
        }

        const src = iframe.getAttribute("src");
        if (!src) return null;

        if (isRunDisabledForPage(src)) {
            return "o";
        }

        const page = src.split("?")[0].toLowerCase();

        if (!page) {
            return null;
        }

        let bodyId = "";
        if (iframeDoc) {
            try {
                bodyId = iframeDoc.body?.id || "";
            } catch (e) {
                // Ignore body ID access failure
            }
        }

        const cardContainer = document.querySelector(".cardsPageWrapper");

        const isCardContainerHidden = cardContainer ? cardContainer.classList.contains("d-none") : true;

        if ((page.endsWith("/tstruct.aspx") || page.includes("tstruct.aspx")) && bodyId !== "Entitymanagement_Body" && isCardContainerHidden) {
            return "t" // tstruct page
        }



        if ((page.endsWith("/iview.aspx") || page.includes("iview.aspx")) && isCardContainerHidden) {
            return "i";  // IView page
        }



        if ((page.endsWith("/entity.aspx") || page.includes("entity.aspx")) && bodyId === "Entitymanagement_Body" && isCardContainerHidden) {
            return "e";  // Entity page
        }



        if ((page.endsWith("/entityform.aspx") || page.includes("entityform.aspx") || bodyId === "Entitymanagement_Body") && isCardContainerHidden) {
            return "ef";  // Entity Data page
        }

        if ((src.includes("/CustomPages") || src.includes("/axidev") || src.includes("/HTMLPages")) && isCardContainerHidden) {
            return "c"; // Custom page

        }

        if (src.includes("../aspx/ImportAll.aspx") && isCardContainerHidden) {
            return "im"; // Import page

        }

        if (src.includes("../aspx/ExportNew.aspx") && isCardContainerHidden) {
            return "ex"; // Export page

        }



        // ../aspx/processflow.aspx?activelist=t&hdnbElapsTime=0


        if ((page.endsWith("/processflow.aspx") || page.includes("processflow.aspx")) && isCardContainerHidden) {
            return "pf"; // Process flow page

        }

        if (src.includes("/aspx/Configuration.aspx/LoadUserAppSettings") && isCardContainerHidden) {
            return "s"; // Settings page
        }

        if (page.endsWith("/axibot.html") || page.includes("/axibot.html")) {
            return "b"; // Axibot page
        }


        return "o"; // Others 

    }

    function extractButtonLabel(btn) {
        if (btn && btn.id === "smartviewFilterToolbarBtn") {
            return "Filter";
        }


        const dataExtra = btn.getAttribute("data-extra");
        if (dataExtra) return dataExtra.trim();

        const text = Array.from(btn.childNodes)
            .filter(n => n.nodeType === Node.TEXT_NODE && n.textContent.trim())
            .map(n => n.textContent.trim())
            .join(" ")
            .trim();

        if (text) {

            return text;
        }

        const title = btn.getAttribute("title") || 
                      btn.getAttribute("data-bs-original-title") || 
                      btn.getAttribute("data-original-title") ||
                      btn.getAttribute("data-bs-title");
        if (title) return title.trim();

        const menuTitle = btn.querySelector(".menu-title");

        if (menuTitle) return menuTitle.textContent.trim();




        const cloned = btn.cloneNode(true);
        cloned.querySelectorAll(".material-icons, .material-icons-style").forEach(node => node.remove());
        return cloned.innerText.trim();

    }

    function hasAction(btn) {
        const isDisabled = btn.classList.contains("disabled");
        if (btn.getAttribute("onclick") && !isDisabled) return true;



        if (btn.tagName === "A" && !isDisabled) {
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
        const buttons = [];
        if (toolbar) {
            buttons.push(...Array.from(toolbar.querySelectorAll("a, button")));
        }

        const filterBtn = doc.querySelector("#smartviewFilterToolbarBtn");
        if (filterBtn && !buttons.includes(filterBtn)) {
            buttons.push(filterBtn);
        }

        if (buttons.length === 0) return {};

        const result = {};

        buttons.forEach((btn, index) => {
            // if (!hasAction(btn)) return;

            if (btn.classList.contains("d-none") || btn.closest(".d-none") || btn.classList.contains("disabled") || btn.closest(".disabled") || btn.closest("[hidden]")) return;


            if (btn.getAttribute("data-kt-menu-attach") === "parent") return;

            if (btn.querySelector(".menu-title") && btn.hasAttribute("data-kt-menu-trigger")) return;


            const id = btn.id || btn.getAttribute("data-id") || btn.getAttribute("title") || `toolbar-btn-${index}`;
            if (!id) return;

            const label = extractButtonLabel(btn);
            if (!label) return;
            const labelLower = label.toLowerCase();
            if (labelLower === "plugin custom code" || labelLower === "export" || labelLower === "theme" || labelLower === "field captions" || labelLower === "view" || labelLower === "pattern") return;

            const dataElement = (btn.getAttribute("data-kt-element") || "").toLowerCase();
            const dataTarget = (btn.getAttribute("data-target") || "").toLowerCase();
            const btnId = (btn.id || btn.getAttribute("data-id") || "").toLowerCase();

            if (getStructType() === "e") {
                const isThemeMode = dataElement === "mode" ||
                                    labelLower === "light" ||
                                    labelLower === "dark" ||
                                    labelLower === "gradient" ||
                                    labelLower === "system" ||
                                    labelLower === "compact" ||
                                    labelLower.includes("light theme") ||
                                    labelLower.includes("dark theme") ||
                                    labelLower.includes("gradient theme") ||
                                    dataTarget.includes("lighttheme") ||
                                    dataTarget.includes("blacktheme") ||
                                    dataTarget.includes("gradtheme") ||
                                    dataTarget.includes("compacttheme") ||
                                    btnId.includes("theme_mode");

                if (isThemeMode) return;
            }

            result[id] = {
                id,
                label: label.toLowerCase(),
                element: btn,
                click: () => btn.click()
            };
        });

        return result;

    }

    function getButtons(querySelector) {
        const iframe = document.getElementById("middle1");
        if (!iframe) return {};

        const doc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!doc) return {};

        const container = doc.querySelector(`${querySelector}`);
        if (!container) return {};

        const buttons = getAllActionButtons(container);

        const result = {};

        buttons.forEach((btn) => {
            // if (!hasAction(btn)) return;
            if (btn.type === "hidden") return;
            if (btn.style.display === "none") return;
            if (btn.offsetParent === null) return;
            if (btn.classList.contains("disabled")) return;

            const id = btn.id || btn.getAttribute("data-id") || btn.getAttribute("title") || btn.name || btn.getAttribute("data-kt-stepper-action");
            if (!id) return;
            // const label = btn.innerText.trim();
            const label = extractButtonLabel(btn);
            if (!label) console.log("There is no label for Element: " + btn);
            if (label.toLowerCase() === "plugin custom code") return;



            result[id] = {
                id,
                label,
                element: btn,
                click: () => btn.click()
            };
        });

        return result;
    }

    function getAllActionButtons(doc) {
        return doc.querySelectorAll(`
        a,
        button,
        input[type="button"],
        input[type="submit"]
    `);
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
            if (label.toLowerCase() === "plugin custom code") return;

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
                if (el.classList.contains("d-none") || el.closest(".d-none") || el.classList.contains("disabled") || el.closest(".disabled") || el.closest("[hidden]")) return;

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
                if (label.toLowerCase() === "plugin custom code") return;

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
            text.startsWith("Fetching") ||
            text === "No Data" ||
            text === "Please type the value..." ||
            text === "Please type Valid date using / (ex: DD / MM / YYYY)" ||
            text === "Please Type the date";

    }


    function handleAnalyse({ tokens, commandConfig }) {

        if (tokens < 1) {
            showToast("Error:Invalid Tokens, Analyze command requires atleast 2 tokens");
            console.error("Error:Invalid Tokens, Analyze command requires atleast 2 tokens");
            return;
        }


        let targetUrl = "../AxpertPlugins/Axi_Beta/HTMLPages/Analytics.html";

        // if (tokens.length === 1) {
        //     targetUrl += "?calendar=t";
        //     targetUrl += `&isDupTab=true-${Date.now()}`;
        //     targetUrl += "&hdnbElapsTime=0";
        // }


        // else {

        const captionSelected = cleanString(tokens[1]);
        const { value: transIdAnalyse, type: resolvedType } = tryResolveToken(1, captionSelected, commandConfig);
        const paramValuesCsv = "tstruct,ads"

        // const type = getType(commandConfig?.prompts?.[0]?.promptSource.toLowerCase(), transIdAnalyse, paramValuesCsv, tokens, commandConfig);
        const type = getType(commandConfig?.prompts?.[0]?.promptSource.toLowerCase(), { value: transIdAnalyse, type: resolvedType }, paramValuesCsv, tokens, commandConfig);

        targetUrl += `?entity=${encodeURIComponent(transIdAnalyse)}`;

        if (tokens.length == 3) {
            let groupByFieldCaption = cleanString(tokens[2]);
            const { value: groupByFiedlname } = tryResolveToken(2, groupByFieldCaption, commandConfig);
            targetUrl += `&groupby=${encodeURIComponent(groupByFiedlname)}`;
        }
        else if (tokens.length > 3) {
            showToast("Analyse commands requires only 3 tokens");
            return;
        }

        targetUrl += "&calendar=t";
        targetUrl += `&type=${type}`
        // targetUrl += "&isDupTab=true-1770626614111";
        targetUrl += `&isDupTab=true-${Date.now()}`;

        targetUrl += "&hdnbElapsTime=0";
        // }

        setCommandRoutes(input.value.trim(), targetUrl);
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
            let prevValueInSet = tokens[tokens.length - 2].toLowerCase();

            if (prevValueInSet === "today" || prevValueInSet === "yesterday" || prevValueInSet === "tomorrow" ||
                prevValueInSet === "lastweek" || prevValueInSet === "nextweek" || prevValueInSet === "thisweek" ||
                prevValueInSet === "lastmonth" || prevValueInSet === "thismonth" || prevValueInSet === "nextmonth" ||
                prevValueInSet === "thisquarter" || prevValueInSet === "lastquarter" || prevValueInSet === "nextquarter" ||
                prevValueInSet === "thisyear" || prevValueInSet === "lastyear" || prevValueInSet === "nextyear" || prevValueInSet === "custom") {
                dateControlBoolean = true;
            }

            if (dateControlBoolean) {

                if (prevValueInSet === "today" || prevValueInSet === "yesterday" || prevValueInSet === "tomorrow" ||
                    prevValueInSet === "lastweek" || prevValueInSet === "nextweek" || prevValueInSet === "thisweek" ||
                    prevValueInSet === "lastmonth" || prevValueInSet === "thismonth" || prevValueInSet === "nextmonth" ||
                    prevValueInSet === "thisquarter" || prevValueInSet === "lastquarter" || prevValueInSet === "nextquarter" ||
                    prevValueInSet === "thisyear" || prevValueInSet === "lastyear" || prevValueInSet === "nextyear") {

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
                        dateControlBoolean = false;

                    }
                }
                else {
                    if (prevValueInSet.toLowerCase() == "custom") {
                        //let settokens = tokens
                        //let lastIndex = tokens.length - 2;
                        //let lastToken = tokens[lastIndex];
                        let settokens = [...tokens]
                        let lastIndex = settokens.length - 2;
                        let lastToken = settokens[lastIndex];

                        settokens[lastIndex] = "";

                        input.value = settokens.join(" ");

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
                            dateControlBoolean = false;

                        }
                        else
                            return ["Please type Valid date using / (ex: DD / MM / YYYY)"];
                    }
                }
            }
            else {
                let acceptedValue = cleanString(tokens[tokens.length - 1]).toLowerCase();

                const list = [
                    "Custom",
                    "Today",
                    "Yesterday",
                    "Tomorrow",
                    "LastWeek",
                    "NextWeek",
                    "ThisWeek",
                    "LastMonth",
                    "ThisMonth",
                    "NextMonth",
                    "ThisQuarter",
                    "LastQuarter",
                    "NextQuarter",
                    "ThisYear",
                    "LastYear",
                    "NextYear"
                ];

                let filtered = list.filter(col => {

                    const rawDisplay = col.toLowerCase();

                    const normalizedTypedValue = (acceptedValue ?? "")
                        .toLowerCase();

                    return rawDisplay.includes(normalizedTypedValue);
                });


                if (acceptedValue && filtered.length === 0) {
                    console.log("User given value which is not in the date list");
                    showToast("Please select a valid Option from the list", 5000, true);

                    let lastIndex = tokens.length - 1;
                    let lastToken = tokens[lastIndex];
                    tokens[lastIndex] = "";

                    input.value = tokens.join(" ");

                    filtered = list;
                }


                return filtered;
            }

            return processCreateCommand(tokens, commandConfig, viewSource);

        }
        else
            return processCreateCommand(tokens, commandConfig, viewSource);
    }

    function processCreateCommand(tokens, commandConfig, createCommandSourceObj) {
        let targetIndex = tokens.length - 1;
        const partialTyped = cleanString(tokens[targetIndex]);

        const createTransId = cleanCommandToken(tokens[1]);
        // setCommandTransid = tryResolveToken(1, createTransId, commandConfig, false);
        const { value, type } = tryResolveToken(1, createTransId, commandConfig, false);
        setCommandTransid = value;

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
                // previousColumnName = tryResolveToken(targetIndex - 2, previousColumnName, commandConfig, false);
                const { value, type } = tryResolveToken(targetIndex - 2, previousColumnName, commandConfig, false);
                previousColumnName = value;
                let previousColumnValue = SET_COMMAND_STATE.currentFieldValue;
                //AddFieldstoList(previousColumnName, 1, previousColumnValue);
                AddFieldstoList(previousColumnName, 1, previousColumnValue, setCommandTransid);
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
                const rawDisplay = (col?.displaydata || col?.name)?.toLowerCase();
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

            if ((SET_COMMAND_STATE.currentField || (targetIndex % 2 !== 0 && targetIndex >= 4)) && filteredObjects.length > 0) {

                resultList.unshift(saveOption);
                resultList.unshift(popOption);
                resultList.unshift(goOption);
                filteredObjects.unshift(saveOption);
                filteredObjects.unshift(popOption);
                filteredObjects.unshift(goOption);
            }
            else if (tokens.length >= 3 && filteredObjects.length > 0) {

                resultList.unshift(popOption);
                resultList.unshift(goOption);
                filteredObjects.unshift(popOption);
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
                    console.log("In processCreateCommand " + createCommandSourceObj + " is empty");
                    showToast("Please Try Again Later.");
                    return [];
                }

                const columnMetadata = colList.find(
                    c =>
                        c.name?.toLowerCase() === prevColumnName.toLowerCase() ||
                        c.displaydata?.toLowerCase().replace(/\s*\(.*?\)/g, '').trim() === prevColumnName.toLowerCase()
                ) || null;

                if (!columnMetadata) {

                    //console.log("In processEditCommond " + createCommandSourceObj + " is empty");
                    //showToast("Please Try Again Later.");

                    console.log("Selected Field Name is Not in the List " + prevColumnName);
                    showToast("Please Select Field from the list", 5000, true);

                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];

                    settokens[lastIndex] = "";

                    targetIndex = targetIndex - 1;

                    input.value = settokens.join(" ");
                    updateDynamicHintFromPrompt({ prompt: "fieldName" })
                    //return [];


                    const usedColumns = new Set();
                    for (let i = 3; i < targetIndex; i += 2) {
                        const usedToken = cleanString(tokens[i]).toLowerCase();
                        usedColumns.add(usedToken);
                    }

                    const filtered = colList.filter(col => {
                        const rawDisplay = (col.displaydata || col.name).toLowerCase();
                        const cleanDisplay = rawDisplay
                            .replace(/\s*\(.*?\)/g, "")
                            .replace(/\s*\[[^\]]+\]\s*$/, "")
                            .trim();
                        const rawName = (col.name || "").toLowerCase();
                        const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);
                        return !isUsed;
                    });

                    filteredObjects = filtered;

                    let resultList = filtered.map(item => item.displaydata || item.caption || item.name || item.fname || item.keyfield);

                    SET_COMMAND_STATE.currentField = null;

                    if (SET_COMMAND_STATE.currentField || (targetIndex % 2 !== 0 && targetIndex >= 4)) {

                        resultList.unshift(saveOption);
                        resultList.unshift(popOption);
                        resultList.unshift(goOption);
                        filteredObjects.unshift(saveOption);
                        filteredObjects.unshift(popOption);
                        filteredObjects.unshift(goOption);

                    }
                    else if (tokens.length >= 3) {
                        resultList.unshift(popOption);
                        resultList.unshift(goOption);
                        filteredObjects.unshift(popOption);
                        filteredObjects.unshift(goOption);
                    }


                    SET_COMMAND_STATE.currentField = null;
                    SET_COMMAND_STATE.currentFieldType = null
                    SET_COMMAND_STATE.currentFieldValue = null;
                    SET_COMMAND_STATE.isNextField = false;
                    SET_COMMAND_STATE.isDropDown = false;




                    return resultList;
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


                if (datatype === 'c' || datatype === 'n' || datatype === "t") {
                    if (isAccept) {
                        let acceptedValue = cleanString(tokens[tokens.length - 1]);
                        if (acceptedValue) {
                            SET_COMMAND_STATE.currentFieldValue = acceptedValue;
                            return [];
                        }
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

                        params2 += ";" + getFieldNameandItsValue(createfieldnamevaluesList[setCommandTransid], commandConfig);

                        console.log(params2);



                        const paramValue = `${params1}$#$${params2}$#$${params3}$#$${params4}`;
                        const sourceKey = `${sourceName}_${paramValue}`.toLowerCase();

                        if (!axDatasourceObj[sourceKey]) {
                            loadList(sourceName, paramValue);
                            return ["Loading values..."];
                        }

                        const list = axDatasourceObj[sourceKey];
                        if (!Array.isArray(list)) return [];

                        let filtered = list.filter(col => {
                            const rawDisplay = String(col.displaydata || col.name)
                                .toLowerCase();

                            const normalizedTypedValue = (acceptedValue ?? "")
                                .toLowerCase();

                            return !normalizedTypedValue || rawDisplay.includes(normalizedTypedValue);
                        });

                        if (acceptedValue && filtered.length === 0) {
                            console.log("User given value is not in the dropdown");
                            showToast("Please select a valid value from the dropdown", 5000, true);

                            let lastIndex = tokens.length - 1;
                            let lastToken = tokens[lastIndex];
                            tokens[lastIndex] = "";

                            input.value = tokens.join(" ");

                            filtered = list;
                        }


                        return filtered.map(col => col.displaydata || col.name);
                    }
                }
                else if (datatype === 'd') {


                    const acceptedValue = cleanString(tokens[tokens.length - 1]);

                    const list = [
                        "Custom",
                        "Today",
                        "Yesterday",
                        "Tomorrow",
                        "LastWeek",
                        "NextWeek",
                        "ThisWeek",
                        "LastMonth",
                        "ThisMonth",
                        "NextMonth",
                        "ThisQuarter",
                        "LastQuarter",
                        "NextQuarter",
                        "ThisYear",
                        "LastYear",
                        "NextYear"
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
            let prevValueInSet = tokens[tokens.length - 2].toLowerCase();

            if (prevValueInSet === "today" || prevValueInSet === "yesterday" || prevValueInSet === "tomorrow" ||
                prevValueInSet === "lastweek" || prevValueInSet === "nextweek" || prevValueInSet === "thisweek" ||
                prevValueInSet === "lastmonth" || prevValueInSet === "thismonth" || prevValueInSet === "nextmonth" ||
                prevValueInSet === "thisquarter" || prevValueInSet === "lastquarter" || prevValueInSet === "nextquarter" ||
                prevValueInSet === "thisyear" || prevValueInSet === "lastyear" || prevValueInSet === "nextyear" || prevValueInSet === "custom") {
                dateControlBoolean = true;
            }

            if (dateControlBoolean) {

                if (prevValueInSet === "today" || prevValueInSet === "yesterday" || prevValueInSet === "tomorrow" ||
                    prevValueInSet === "lastweek" || prevValueInSet === "nextweek" || prevValueInSet === "thisweek" ||
                    prevValueInSet === "lastmonth" || prevValueInSet === "thismonth" || prevValueInSet === "nextmonth" ||
                    prevValueInSet === "thisquarter" || prevValueInSet === "lastquarter" || prevValueInSet === "nextquarter" ||
                    prevValueInSet === "thisyear" || prevValueInSet === "lastyear" || prevValueInSet === "nextyear") {

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
                        dateControlBoolean = false;

                    }
                }
                else {
                    if (prevValueInSet.toLowerCase() == "custom") {
                        //let settokens = tokens
                        //let lastIndex = tokens.length - 2;
                        //let lastToken = tokens[lastIndex];
                        let settokens = [...tokens]
                        let lastIndex = settokens.length - 2;
                        let lastToken = settokens[lastIndex];

                        settokens[lastIndex] = "";

                        input.value = settokens.join(" ");

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
                            dateControlBoolean = false;

                        }
                        else
                            return ["Please type Valid date using / (ex: DD / MM / YYYY)"];
                    }
                }
            }
            else {
                let acceptedValue = cleanString(tokens[tokens.length - 1]).toLowerCase();

                const list = [
                    "Custom",
                    "Today",
                    "Yesterday",
                    "Tomorrow",
                    "LastWeek",
                    "NextWeek",
                    "ThisWeek",
                    "LastMonth",
                    "ThisMonth",
                    "NextMonth",
                    "ThisQuarter",
                    "LastQuarter",
                    "NextQuarter",
                    "ThisYear",
                    "LastYear",
                    "NextYear"
                ];

                let filtered = list.filter(col => {

                    const rawDisplay = col.toLowerCase();

                    const normalizedTypedValue = (acceptedValue ?? "")
                        .toLowerCase();

                    return rawDisplay.includes(normalizedTypedValue);
                });


                if (acceptedValue && filtered.length === 0) {
                    console.log("User given value which is not in the date list");
                    showToast("Please select a valid Option from the list", 5000, true);

                    let lastIndex = tokens.length - 1;
                    let lastToken = tokens[lastIndex];
                    tokens[lastIndex] = "";

                    input.value = tokens.join(" ");

                    filtered = list;
                }


                return filtered;
            }

            return processEditCommand(tokens, commandConfig, viewSource);

        }
        else
            return processEditCommand(tokens, commandConfig, viewSource);
    }

    function processEditCommand(tokens, commandConfig, createCommandSourceObj) {
        let targetIndex = tokens.length - 1;
        const partialTyped = cleanString(tokens[targetIndex]);

        const createTransId = cleanCommandToken(tokens[1]);
        // setCommandTransid = tryResolveToken(1, createTransId, commandConfig, false);
        const { value, type } = tryResolveToken(1, createTransId, commandConfig, false);
        setCommandTransid = value;

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
                // previousColumnName = tryResolveToken(targetIndex - 2, previousColumnName, commandConfig, false);
                const { value, type } = tryResolveToken(targetIndex - 2, previousColumnName, commandConfig, false);
                previousColumnName = value;
                let previousColumnValue = SET_COMMAND_STATE.currentFieldValue;
                AddFieldstoList(previousColumnName, 1, previousColumnValue, setCommandTransid);
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
                const rawDisplay = (col?.displaydata || col?.name)?.toLowerCase();
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

            //if ((SET_COMMAND_STATE.currentField || (targetIndex % 2 == 0 && targetIndex >= 4)) && filteredObjects.length > 0) {
            if ((SET_COMMAND_STATE.currentField || (targetIndex % 2 == 0 && targetIndex >= 4))) {
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
                    console.log("In processEditCommond " + createCommandSourceObj + " is empty");
                    showToast("Please Try Again Later.");
                    return [];
                }

                const columnMetadata = colList.find(
                    c =>
                        c.name?.toLowerCase() === prevColumnName.toLowerCase() ||
                        c.displaydata?.toLowerCase().replace(/\s*\(.*?\)/g, '').trim() === prevColumnName.toLowerCase()
                ) || null;

                if (!columnMetadata) {
                    //console.log("In processEditCommond " + createCommandSourceObj + " is empty");
                    //showToast("Please Try Again Later.");

                    console.log("Selected Field Name is Not in the List " + prevColumnName);
                    showToast("Please Select Field only from the list", 5000, true);

                    let settokens = [...tokens]
                    let lastIndex = settokens.length - 2;
                    let lastToken = settokens[lastIndex];

                    settokens[lastIndex] = "";

                    targetIndex = targetIndex - 1;

                    input.value = settokens.join(" ");
                    updateDynamicHintFromPrompt({ prompt: "fieldName" })

                    //return [];


                    const usedColumns = new Set();
                    for (let i = 4; i < targetIndex; i += 2) {
                        const usedToken = cleanString(tokens[i]).toLowerCase();
                        usedColumns.add(usedToken);
                    }

                    const filtered = colList.filter(col => {
                        const rawDisplay = (col.displaydata || col.name).toLowerCase();
                        const cleanDisplay = rawDisplay
                            .replace(/\s*\(.*?\)/g, "")
                            .replace(/\s*\[[^\]]+\]\s*$/, "")
                            .trim();
                        const rawName = (col.name || "").toLowerCase();
                        const isUsed = usedColumns.has(cleanDisplay) || usedColumns.has(rawName);
                        return !isUsed;
                    });

                    filteredObjects = filtered;

                    let resultList = filtered.map(item => item.displaydata || item.caption || item.name || item.fname || item.keyfield);

                    SET_COMMAND_STATE.currentField = null;

                    if (SET_COMMAND_STATE.currentField || (targetIndex % 2 == 0 && targetIndex >= 4)) {
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


                if (datatype === 'c' || datatype === 'n' || datatype === "t") {
                    if (isAccept) {
                        let acceptedValue = cleanString(tokens[tokens.length - 1]);
                        if (acceptedValue) {
                            SET_COMMAND_STATE.currentFieldValue = acceptedValue;
                            return [];
                        }
                        else {
                            return ["Please type the value..."];
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


                        params2 += ";" + getFieldNameandItsValue(createfieldnamevaluesList[setCommandTransid], commandConfig);

                        console.log(params2);



                        const paramValue = `${params1}$#$${params2}$#$${params3}$#$${params4}`;
                        const sourceKey = `${sourceName}_${paramValue}`.toLowerCase();

                        if (!axDatasourceObj[sourceKey]) {
                            loadList(sourceName, paramValue);
                            return ["Loading values..."];
                        }

                        const list = axDatasourceObj[sourceKey];
                        if (!Array.isArray(list)) return [];

                        let filtered = list.filter(col => {
                            const rawDisplay = String(col.displaydata || col.name)
                                .toLowerCase();

                            const normalizedTypedValue = (acceptedValue ?? "")
                                .toLowerCase();

                            return !normalizedTypedValue || rawDisplay.includes(normalizedTypedValue);
                        });

                        if (acceptedValue && filtered.length === 0) {
                            console.log("User given value which is not in the dropdown");
                            showToast("Please select a valid value from the dropdown", 5000, true);

                            let lastIndex = tokens.length - 1;
                            let lastToken = tokens[lastIndex];
                            tokens[lastIndex] = "";

                            input.value = tokens.join(" ");

                            filtered = list;
                        }



                        return filtered.map(col => col.displaydata || col.name);
                    }
                }
                else if (datatype === 'd') {


                    const acceptedValue = cleanString(tokens[tokens.length - 1]).toLowerCase();

                    const list = [
                        "Custom",
                        "Today",
                        "Yesterday",
                        "Tomorrow",
                        "LastWeek",
                        "NextWeek",
                        "ThisWeek",
                        "LastMonth",
                        "ThisMonth",
                        "NextMonth",
                        "ThisQuarter",
                        "LastQuarter",
                        "NextQuarter",
                        "ThisYear",
                        "LastYear",
                        "NextYear"
                    ];

                    //if (list.some(item => item.toLowerCase() === acceptedValue.toLowerCase())) {
                    //    SET_COMMAND_STATE.currentFieldType = datatype;
                    //    return editCommandHandling(tokens, commandConfig, createCommandSourceObj);

                    //}
                    //else SET_COMMAND_STATE.currentFieldType = null;

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
    function AddFieldstoList(fieldName, rowNo, value, transid) {

        if (!fieldName || !value || !transid) return;

        if (!createfieldnamevaluesList[transid]) {
            createfieldnamevaluesList[transid] = [];
        }

        const currentList = createfieldnamevaluesList[transid];

        const keyPrefix = `${fieldName}~${rowNo}=`;
        const existingIndex = currentList.findIndex(item =>
            item.startsWith(keyPrefix)
        );

        const formatted = `${fieldName}~${rowNo}=${value}`;

        if (existingIndex !== -1) {
            if (currentList[existingIndex] !== formatted) {
                currentList[existingIndex] = formatted;
            }
        } else {
            currentList.push(formatted);
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


    // function AxisetFieldValue(actualFieldName, value, rowNo) {
    //     const fldid = actualFieldName + "000F" + rowNo;

    //     const iframe = document.getElementById("middle1");

    //     if (iframe) {
    //         const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
    //         const iframeDocElement = iframeDoc.getElementById(fldid);
    //         //const iframeDoc = iframe.contentWindow;

    //         //$("#middle1")[0].contentWindow.CallSetFieldValue(fldid, FldVal);
    //         //$("#middle1")[0].contentWindow.MainBlur($(fldid))

    //         //$("#middle1")[0].contentWindow.MainBlur($(fldid))
    //         //$("#middle1")[0].contentWindow.UpdateFieldArray(fldid, fldDbRowNo, fldValue, "parent", "");
    //         //$("#middle1")[0].contentWindow.CallSetFieldValue(fldid, fldValue);

    //         if (iframeDocElement) {

    //             ///
    //             /// For dependency we need to verify this logic.
    //             ////

    //             iframeDocElement.value = value;

    //             iframeDocElement.dispatchEvent(new Event("input", { bubbles: true }));
    //             iframeDocElement.dispatchEvent(new Event("change", { bubbles: true }));
    //             iframeDocElement.dispatchEvent(new Event("blur", { bubbles: true }));


    //             ///Product Field Set Logic
    //             //iframeDoc.UpdateFieldArray(fldid, rowNo, value, "parent", "");
    //             //iframeDoc.CallSetFieldValue(fldid, value);
    //             //iframeDoc.MainBlur($(fldid));
    //             return true;
    //         }
    //         else {
    //             return false;
    //         }
    //     }
    //     else return false;
    // }

    function AxisetFieldValue(actualFieldName, value, rowNo) {

        const fldid = actualFieldName + "000F" + rowNo;
        const iframe = document.getElementById("middle1");
        if (!iframe) return false;

        const iframeWin = iframe.contentWindow;
        const iframeDoc = iframe.contentDocument || iframeWin.document;

        try {

            /* ---------------- MULTI SELECT (SELECT2) ---------------- */

            const multiSelectEl = iframeDoc.getElementById(fldid);

            if (multiSelectEl && multiSelectEl.multiple) {

                const valuesArray = (value || "")
                    .split(",")
                    .map(v => v.trim())
                    .filter(Boolean);

                if (iframeWin.$) {

                    const $el = iframeWin.$(multiSelectEl);

                    // Ensure options exist
                    valuesArray.forEach(val => {
                        if ($el.find("option[value='" + val + "']").length === 0) {
                            const newOption = new iframeWin.Option(val, val, true, true);
                            $el.append(newOption);
                        }
                    });

                    // Set UI
                    $el.val(valuesArray).trigger("change");
                }

                // ?? VERY IMPORTANT � update Axpert internal array
                if (typeof iframeWin.UpdateFieldArray === "function") {
                    iframeWin.UpdateFieldArray(fldid, rowNo, value, "parent", "");
                }

                return true;
            }

            /* ---------------- SINGLE SELECT (SELECT2) ---------------- */

            const singleSelectEl = iframeDoc.getElementById(fldid);

            if (singleSelectEl && !singleSelectEl.multiple && singleSelectEl.classList.contains("select2-hidden-accessible")) {

                if (iframeWin.$) {

                    const $el = iframeWin.$(singleSelectEl);

                    // If option doesn't exist, create it
                    if ($el.find("option[value='" + value + "']").length === 0) {
                        const newOption = new iframeWin.Option(value, value, true, true);
                        $el.append(newOption);
                    }

                    $el.val(value).trigger("change");
                }

                if (typeof iframeWin.UpdateFieldArray === "function") {
                    iframeWin.UpdateFieldArray(fldid, rowNo, value, "parent", "");
                }

                return true;
            }


            /* ---------------- CHECKBOX ---------------- */

            const checkboxEl = iframeDoc.querySelector(
                `input[type="checkbox"]#${fldid}`
            );

            if (checkboxEl) {

                const normalized = (value || "").toString().toLowerCase();

                checkboxEl.checked =
                    normalized === "true" ||
                    normalized === "1" ||
                    normalized === "yes" ||
                    normalized === "t" ||
                    normalized === "y";


                checkboxEl.dispatchEvent(new Event("change", { bubbles: true }));
                checkboxEl.dispatchEvent(new Event("blur", { bubbles: true }));

                return true;
            }


            /* ---------------- RADIO GROUP ---------------- */

            const radioGroup = iframeDoc.querySelectorAll(
                `input[type="radio"][name="${fldid}"]`
            );

            if (radioGroup.length > 0) {

                radioGroup.forEach(r => {
                    r.checked = (r.value === value);
                });

                radioGroup[0].dispatchEvent(
                    new Event("change", { bubbles: true })
                );

                return true;
            }


            /* ---------------- NORMAL INPUT ---------------- */

            const normalInput = iframeDoc.getElementById(fldid);

            if (normalInput) {

                normalInput.value = value;

                normalInput.dispatchEvent(new Event("input", { bubbles: true }));
                normalInput.dispatchEvent(new Event("change", { bubbles: true }));
                normalInput.dispatchEvent(new Event("blur", { bubbles: true }));

                return true;
            }

            return false;

        } catch (ex) {
            console.error("AxisetFieldValue error:", ex);
            return false;
        }
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
            let { value: transId, type } = tryResolveToken(1, rawName, commandConfig, false);

            // if (transId === rawName) {
            //     const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
            //     const found = list?.find(
            //         x => x.caption.toLowerCase() === rawName.toLowerCase()
            //     );
            //     if (found) transId = found.name
            //     else {
            //         console.error("Invalid Tstruct name");
            //         showToast("Invalid Tstruct name please select the valid tstruct");
            //         return;
            //     }
            // }

            const sourceName = commandConfig?.prompts?.[2]?.promptSource?.toLowerCase();
            const sourceKey = `${sourceName}_${transId}`.toLowerCase();
            const fieldsList = axDatasourceObj[sourceKey];

            if (fieldsList) {
                let startIndex = cleanCommandToken(tokens[2]).toLowerCase() === "with" ? 3 : 2;

                for (let i = startIndex; i < tokens.length; i += 2) {
                    let rawField = cleanCommandToken(tokens[i]);

                    if (!rawField) continue;

                    const isValidField = colList.some(c =>
                        (c.name && c.name.toLowerCase() === rawField.toLowerCase()) ||
                        (c.caption && c.caption.toLowerCase() === rawField.toLowerCase()) ||
                        (c.displaydata && c.displaydata.replace(/\s*\(.*?\)/g, '').trim().toLowerCase() === rawField.toLowerCase())
                    );

                    if (!isValidField) {
                        console.error("Execution blocked: Invalid field name - " + rawField);
                        showToast(`'${rawField}' is not a valid field. Please select from the list.`, 5000, false);
                        return;
                    }
                }
            }

            redirectToTstruct(transId, rawName);
            return;

        }

        let rawName = cleanCommandToken(tokens[1]);
        let { value: transId, type } = tryResolveToken(1, rawName, commandConfig, false);

        //if (!transId ) {
        //    console.log("Missing transaction ID or field values.");
        //    showToast("Some required information is missing. Please re-enter the command and try again.");
        //    return;
        //}

        if (!transId || !createfieldnamevaluesList || !createfieldnamevaluesList[transId] || createfieldnamevaluesList[transId].length === 0) {
            console.log("Missing transaction ID or field values. Tstructid : " + transId);
            showToast("Incomplete command detected. Please clear the entire command and try again.");
            return [];
        }


        //commenetd bcz of set issue for a dependency field

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
        //            console.log(`SET FAILED ? ${fieldname} = ${value}`);
        //            continue;
        //        }


        //        let resultOfSetFieldValue = AxisetFieldValue(actualFieldName, value, rowNo);

        //        if (resultOfSetFieldValue) {
        //            console.log(`SET SUCCESS ? ${fieldname} = ${value}`);
        //            continue;

        //        }
        //        else {
        //            console.log(`SET FAILED ? ${fieldname} = ${value}`);
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


                        console.log(createfieldnamevaluesList[transId]);
                        // for (let j = 2; j < tokens.length; j += 2) {
                        for (let j = 0; j < createfieldnamevaluesList[transId].length; j++) {

                            const item = createfieldnamevaluesList[transId][j];
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
                                console.log(`SET FAILED ? ${fname} = ${val}`);
                                continue;
                            }

                            let resultOfSetFieldValue = AxisetFieldValue(actualFName, val, rowNo);

                            if (resultOfSetFieldValue) {
                                console.log(`SET SUCCESS ? ${actualFName} = ${val}`);
                            }
                            else {
                                console.log(`SET FAILED ? ${actualFName} = ${val}`);
                            }
                        }

                    }
                    catch (ex) {
                        console.error("Error while setting fields after iframe load:", ex);
                    }
                    finally {
                        iframe.onload = null;
                        ///comment bcz go option should work based on command not based on list so when we remove it but 
                        //actual command / tokens exits these creates confusion and so many bugs(25-02 - 2026)
                        //createfieldnamevaluesList = [];
                    }
                }, 1000);
            };

            setEditSessionState(transId);

            redirectToTstruct(transId, rawName);

        }
        catch (ex) {
            console.log("Error in handleCreate: " + ex);
            showToast("Incomplete command detected. Please clear the entire command and try again.");
            //createfieldnamevaluesList = [];
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



        const now = new Date();
        const HH = String(now.getHours()).padStart(2, "0");
        const MI = String(now.getMinutes()).padStart(2, "0");
        const SS = String(now.getSeconds()).padStart(2, "0");
        const time = `${HH}:${MI}:${SS}`;


        switch (format.toLowerCase()) {
            case "YYYYMMDD".toLowerCase():
                return `${yyyy}${mm}${dd}`;

            case "DDMMYYYY".toLowerCase():
                return `${dd}${mm}${yyyy}`;

            case "YYYY-MM-DD".toLowerCase():
                return `${yyyy}-${mm}-${dd}`;

            case "DD-MM-YYYY".toLowerCase():
                return `${dd}-${mm}-${yyyy}`;

            case "YYYY/MM/DD".toLowerCase():
                return `${yyyy}/${mm}/${dd}`;

            case "DD/MM/YYYY".toLowerCase():
                return `${dd}/${mm}/${yyyy}`;

            case "YYYY.MM.DD".toLowerCase():
                return `${yyyy}.${mm}.${dd}`;

            case "DD.MM.YYYY".toLowerCase():
                return `${dd}.${mm}.${yyyy}`;

            case "MMDDYYYY".toLowerCase():
                return `${mm}${dd}${yyyy}`;

            case "MM-DD-YYYY".toLowerCase():
                return `${mm}-${dd}-${yyyy}`;

            case "MM/DD/YYYY".toLowerCase():
                return `${mm}/${dd}/${yyyy}`;

            case "MM.DD.YYYY".toLowerCase():
                return `${mm}.${dd}.${yyyy}`;

            case "YYYY-MM-DD HH:MM:SS".toLowerCase():
                return `${yyyy}-${mm}-${dd} ${time}`;

            case "DD-MM-YYYY HH:MM:SS".toLowerCase():
                return `${dd}-${mm}-${yyyy} ${time}`;

            case "YYYY/MM/DD HH:MM:SS".toLowerCase():
                return `${yyyy}/${mm}/${dd} ${time}`;

            case "DD/MM/YYYY HH:MM:SS".toLowerCase():
                return `${dd}/${mm}/${yyyy} ${time}`;

            case "YYYY-MM-DDTHH:MM:SS".toLowerCase():
                return `${yyyy}-${mm}-${dd}T${time}`;

            case "YYYYMMDDHHMMSS".toLowerCase():
                return `${yyyy}${mm}${dd}${HH}${MI}${SS}`;

            case "YYYY-MM-DDTHH:MM:SSZ".toLowerCase():
                return `${yyyy}-${mm}-${dd}T${time}Z`;

            case "DD MMM YYYY".toLowerCase():
                const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                return `${dd}-${months[date.getMonth()]}-${yyyy}`;

            default:
                return `${dd}/${mm}/${yyyy}`;
        }
    }

    function getDateByFilter(type) {
        const baseDate = new Date();
        baseDate.setHours(0, 0, 0, 0);

        let date = null;
        type = type.toLowerCase().replace(/\s/g, "");

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

            case "thisweek": {
                const d = new Date(baseDate);
                const day = d.getDay();
                d.setDate(d.getDate() - day);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "thismonth": {
                const d = new Date(baseDate.getFullYear(), baseDate.getMonth(), 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "lastmonth": {
                const d = new Date(baseDate.getFullYear(), baseDate.getMonth() - 1, 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "nextmonth": {
                const d = new Date(baseDate.getFullYear(), baseDate.getMonth() + 1, 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "thisquarter": {
                const quarter = Math.floor(baseDate.getMonth() / 3);
                const d = new Date(baseDate.getFullYear(), quarter * 3, 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "lastquarter": {
                const quarter = Math.floor(baseDate.getMonth() / 3) - 1;
                const d = new Date(baseDate.getFullYear(), quarter * 3, 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "nextquarter": {
                const quarter = Math.floor(baseDate.getMonth() / 3) + 1;
                const d = new Date(baseDate.getFullYear(), quarter * 3, 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "thisyear": {
                const d = new Date(baseDate.getFullYear(), 0, 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "lastyear": {
                const d = new Date(baseDate.getFullYear() - 1, 0, 1);
                date = formatDate(d, "DD/MM/YYYY");
                break;
            }

            case "nextyear": {
                const d = new Date(baseDate.getFullYear() + 1, 0, 1);
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
    //function getDateByFilter(type) {
    //    const baseDate = new Date();
    //    baseDate.setHours(0, 0, 0, 0);

    //    let date = null;
    //    type = type.toLowerCase();

    //    switch (type) {
    //        case "today":
    //            date = formatDate(baseDate, "DD/MM/YYYY");
    //            break;

    //        case "yesterday": {
    //            const d = new Date(baseDate);
    //            d.setDate(d.getDate() - 1);
    //            date = formatDate(d, "DD/MM/YYYY");
    //            break;
    //        }

    //        case "tomorrow": {
    //            const d = new Date(baseDate);
    //            d.setDate(d.getDate() + 1);
    //            date = formatDate(d, "DD/MM/YYYY");
    //            break;
    //        }

    //        case "lastweek": {
    //            const d = new Date(baseDate);
    //            d.setDate(d.getDate() - 7);
    //            date = formatDate(d, "DD/MM/YYYY");
    //            break;
    //        }

    //        case "nextweek": {
    //            const d = new Date(baseDate);
    //            d.setDate(d.getDate() + 7);
    //            date = formatDate(d, "DD/MM/YYYY");
    //            break;
    //        }

    //        case "lastyear": {
    //            const d = new Date(baseDate);
    //            d.setFullYear(d.getFullYear() - 1);
    //            date = formatDate(d, "DD/MM/YYYY");
    //            break;
    //        }

    //        case "custom":
    //        default:
    //            return {
    //                date: "",
    //                openDatePicker: true
    //            };
    //    }

    //    return {
    //        date,
    //        openDatePicker: false
    //    };
    //}


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

    //        // If EDIT ? add edit-specific fields
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
    //    //If EDIT ? attach keyfield & keyvalue at root level
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

        // ?? Call preparePayload AFTER session is ready

        //if (!transid || saveListWithFieldNamendValues.length == 0) {
        if (!transid || !saveListWithFieldNamendValues || !saveListWithFieldNamendValues[transid] || saveListWithFieldNamendValues[transid].length === 0) {
            console.log("Missing transaction ID or field values.");
            showToast("Incomplete command detected. Please clear the entire command and try again.");

            return [];
        }

        let keyFieldValue = cleanCommandToken(inputTokens[2]);
        let keyfieldName;
        if (!isCreate) {
            //const extraPromptSource = "axi_keyfieldList";
            const struct_source = inputCommandConfig.prompts[0].promptSource;

            // extra params
            let struct_paramValue
            if (struct_source.toLowerCase() === "axi_structmetalist")
                struct_paramValue = processExtraParams(inputTokens, inputCommandConfig);

            const struct_sourceKey = (struct_paramValue ? `${struct_source}_${struct_paramValue}` : struct_source).toLowerCase();

            // load datasource if not exists
            if (!axDatasourceObj[struct_sourceKey]) {

                const struct_hasParams = !struct_prompt.promptParams ||
                    (struct_paramValue && struct_paramValue.replace(/,/g, '').trim().length > 0);

                if (struct_hasParams) {
                    loadList(struct_source, struct_paramValue);
                    return [`Loading ${struct_source}...`];
                }
            }

            const struct_dataList = axDatasourceObj[struct_sourceKey];

            const struct_row = struct_dataList.find(r => r.name === transid);

            const primaryField = struct_row?.keyfield

            //if (!primaryField) {
            //    throw new Error("Key Field List is missing");;
            //}

            //const extraSourceKey = `${extraPromptSource}_${transid}`.toLowerCase();

            //const extraList = axDatasourceObj[extraSourceKey];

            //if (extraList.length == 0) {

            //}
            //const field = extraList[0];

            //keyfieldName = field.fname ?? field.keyfield ?? field.name ?? field.displaydata;

            keyfieldName = primaryField;

        }
        preparePayload(saveListWithFieldNamendValues[transid] || [], transid, sourcename, isCreate, keyFieldValue, keyfieldName).then(result => {

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

                        //saveListWithFieldNamendValues = [];
                        //createfieldnamevaluesList = [];
                        return [];
                    }

                    console.log(data);
                    throw new Error("Unexpected API response structure");

                })
                .catch(error => {

                    //showToast("Your Data is not Submitted,Please Submit it Manually using(Ctrl + Enter)!");
                    // showToast("Save failed.Please retry with Ctrl + Enter.");
                    showToast("Save failed.Please try again later.");
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
    function preparePayload(saveListWithFieldNamendValueswithTransId, transid, sourcename, iscreate, inputKeyFieldValue, inputKeyFieldName) {

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


            // Record level variables
            var recordid = iscreate ? "0" : "";
            var keyfield = iscreate ? "" : "";
            var keyvalue = iscreate ? "" : "";

            // In edit mode you will later assign:
            if (!iscreate) {
                keyfield = inputKeyFieldName;
                keyvalue = inputKeyFieldValue;
            }
            try {
                for (let i = 0; i < saveListWithFieldNamendValueswithTransId.length; i++) {

                    const item = saveListWithFieldNamendValueswithTransId[i];
                    if (!item) continue;

                    const sep = item.includes("=") ? "=" : " ";
                    const parts = item.split(sep);
                    if (parts.length < 2) continue;

                    const left = parts[0];
                    const value = parts.slice(1).join(sep).trim();

                    const leftParts = left.split("~");
                    const fieldName = leftParts[0];
                    const rowNo = leftParts.length > 1 ? leftParts[1] : "1";

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
                    if (fieldName !== keyfield) rowObj.columns[keyfield] = keyvalue;
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
                        cachedsave: "false",
                        axpapp: mainProject,
                        //appsessionkey: "010198670011016401680166017301540168015301640101009800994939364141171051310018",
                        transid: transid,
                        s: mainSessionId,
                        username: payloadUsername,
                        password: password,
                        changedrows: {},
                        trace: "true",
                        forpaybooks: "true",
                        dataupdate: "false",
                        primarykey: keyfield,
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
        let targetUrl = `${getAppBaseUrl()}/AxpertPlugins/Axi_Beta/HTMLPages/axibot.html`;
        // let targetUrl = "../axidev/HTMLPages/axibot_1770979038509.html";




        console.log("Target Url for AxiBot:  " + targetUrl);


        setCommandRoutes(input.value.trim(), targetUrl);
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
            if (btn.classList.contains("d-none") || btn.closest(".d-none") || btn.classList.contains("disabled") || btn.closest(".disabled") || btn.closest("[hidden]")) return;

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
        initCommands();
        window.LoadIframe("loadhomepage");
        console.log(JSON.stringify(commands));
    }

    function saveToHistory(text) {
        if (!text || text.trim() === "") return;

        commandHistory = commandHistory.filter(item => item.toLowerCase() !== text.toLowerCase());

        commandHistory.unshift(text);

        if (commandHistory.length > MAX_HISTORY) {
            commandHistory.pop();
        };

        localStorage.setItem(`axi_command_history_${getAppBaseUrl()}_${window.mainUserName}`, JSON.stringify(commandHistory));

        historyIndex = -1;
    }

    function loadCommandHistory() {
        try {
            commandHistory = JSON.parse(localStorage.getItem(`axi_command_history_${getAppBaseUrl()}_${window.mainUserName}`));
        } catch (ex) {
            commandHistory = [];
        }
    }

    function navigateHistory(direction) {
        if (commandHistory.length === 0) {
            showToast("No command History available");
            return;
        }

        if (direction === "open") {
            historyIndex = 0;
        } else if (direction === "prev") {
            if (historyIndex < commandHistory.length - 1) {
                historyIndex++;
            }
        } else if (direction === "next") {
            if (historyIndex > 0) {
                historyIndex--;
            } else {
                historyIndex = -1;
                input.value = "";
                handleInput();
                return;
            }
        }

        if (historyIndex >= 0 && historyIndex < commandHistory.length) {
            input.value = commandHistory[historyIndex] + " ";
            executeCommandsV2(true);


            // setTimeout(() => {
            input.focus();
            input.setSelectionRange(input.value.length, input.value.length);
            handleInput();
            hide();
            // }, 10); 
        }
    }

    function loadFavorites() {
        const appUrl = getAppBaseUrl();
        const appname = getProjectName();
        const favKey = `axi_favourites_${appUrl}_${window.mainUserName}`;
        try {
            const localData = JSON.parse(localStorage.getItem(favKey)) || [];
            commandFavorites = localData.map(fav =>
                typeof fav === 'string' ? { commandText: fav, targetURL: "" } : fav
            );

        } catch (ex) {
            commandFavorites = [];
        }

        renderFavoritesUI();

        if (axiFavoritesUrl) {
            fetch(`${axiFavoritesUrl}?username=${window.mainUserName}&appname=${appname}`)
                .then(res => {
                    isCommandsLoading = true;
                    input.disabled = false;
                    input.placeholder = "Axpert AI";


                    return res.json()
                })
                .then(data => {
                    console.log("Fetched favorites from backend: ", data);
                    console.log("type : ", typeof data);
                    if (data && Array.isArray(data)) {
                        commandFavorites = data.map(item => ({
                            favouritesId: item?.favouritesId,
                            username: item?.username,


                            commandText: item?.commandText || item?.commandtext,
                            originalCommandText: item.originalCommandText,
                            favOrder: item.favOrder,
                            targetUrl: item.targetUrl || item.targetURL || item.targeturl,
                            createdOn: item.createdOn
                        }));
                        console.log("favKey");
                        localStorage.setItem(favKey, JSON.stringify(commandFavorites));
                        renderFavoritesUI();
                    }
                })
                .catch(err => {
                    console.error("Axi: Failed to sync favorites from backend", err);
                    showToast("Axi: Axi: Failed to sync favorites from backend");

                }).finally(() => {
                    isCommandsLoading = false;
                    input.disabled = false;
                    input.placeholder = "Axpert AI"

                });

        }


    }

    function toggleFavorite(cmdText, isAdding = false) {
        loadFavorites();
        let cmdIndex;
        const tokens = getTokens(cmdText.trim());


        const groupKey = tokens[0];
        const commandVerb = tokens[1];

        if (groupKey?.toLowerCase() === "help" || groupKey?.toLowerCase() === "version") {
            showToast("You cannot add this command to Favorites!");
            return;
        }

        if (groupKey?.toLowerCase() === "run") {
            showToast("You cannot add 'run' commands to favorites");
            return;
        }

        if (commandVerb?.toLowerCase() === "keyfield") {
            showToast("You cannot add this command to Favorites!");
            return;
        }
        const appUrl = getAppBaseUrl();
        const appname = getProjectName();
        const favKey = `axi_favourites_${appUrl}_${window.mainUserName}`;

        if (isAdding) {
            cmdIndex = commandFavorites.findIndex(fav => fav?.originalCommandText?.toLowerCase() === cmdText.toLowerCase());
        } else {
            cmdIndex = commandFavorites.findIndex(fav => fav?.commandText?.toLowerCase() === cmdText.toLowerCase());
        }



        let commandRoute = commandRoutes.find(route => route.commandText.toLowerCase() === cmdText.toLowerCase());

        if (!commandRoute) {
            const cmdTokens = getTokens(cmdText);
            if (cmdTokens.length === 1 && isTargetEntity(cmdTokens[0])) {
                const viewCmd = "view " + cmdText;
                commandRoute = commandRoutes.find(route => route.commandText.toLowerCase() === viewCmd.toLowerCase());
            }
        }

        if (isAdding) {
            if (isDuplicateFavorite(cmdText)) {
                showToast("This command is already in your Favorites.");
                return;
            }
        }

        if (cmdIndex !== -1) {
            if (isAdding) {
                showToast("This command is already in your Favorites.");
                return;
            }
            // const removedFav = commandFavorites.splice(cmdIndex, 1);
            // showToast(`Removed '${cmdText}' from Favorites`);
            // localStorage.setItem(favKey, JSON.stringify(commandFavorites));
            // renderFavoritesUI();
            // render(); 

            // if (axiFavoritesUrl) {
            //     fetch(`${axiFavoritesUrl}?appname=${appname}`, {
            //         method: "POST",
            //         headers: { "Content-Type": "application/json" },
            //         body: JSON.stringify({
            //             username: window.mainUserName,
            //             commandText: removedFav.commandText,
            //             action: "remove",
            //             favOrder: 0,
            //             targetURL: removedFav?.targetUrl || removedFav?.targetURL || removedFav?.targeturl
            //         })
            //     }).catch(err => console.error("Axi: Failed to update on backend", err));
            // }

            showDeleteFavoriteModal(cmdText);
        } else {
            if (!commandRoute) {
                showToast("Please Execute the command at least once before adding to favorites");
                return;
            }
            if (commandFavorites.length >= MAX_FAVORITES) {
                showToast(`Maximum of ${MAX_FAVORITES} favorites allowed. Please remove some favorites before adding new ones.`);
                return;
            }
            // commandFavorites.unshift({ commandText: cmdText, targetUrl: commandRoute.targetUrl });

            // showToast(`Added '${cmdText}' to favorites`, 3000, true);

            showFavoriteModel(cmdText, commandRoute.targetUrl);
        }

        // localStorage.setItem(favKey, JSON.stringify(commandFavorites));

        // renderFavoritesUI();
        // render();

        // if (axiFavoritesUrl) {
        //     fetch(`${axiFavoritesUrl}?appname=${appname}`, {
        //         method: "POST",
        //         headers: { "Content-Type": "application/json" },
        //         body: JSON.stringify({
        //             username: window.mainUserName,
        //             commandText: cmdText,
        //             action: isAdding ? "add" : "remove",
        //             favOrder: 0,
        //             targetURL: commandRoute?.targetUrl

        //         })
        //     }).catch(err => {
        //         showToast("Axi: Failed to update favorite on backend, Please check AxiApi Configuration");
        //         console.error("Axi: Failed to update favorite on backend", err)
        //     });
        // }

    }

    function renderFavoritesUI(itemsToRender = commandFavorites) {
        if (!favouritesCard) return;

        const wrapper = favouritesCard.querySelector(".My-Fav-Items-Wrapper");

        if (!wrapper) return;

        wrapper.innerHTML = "";
        const axiFavoritesCount = document.getElementById("axiFavoriteCount");

        if (axiFavoritesCount) {
            axiFavoritesCount.textContent = commandFavorites.length;


        }

        if (itemsToRender.length === 0) {
            wrapper.innerHTML = `<div style="padding: 15px; color: #999; text-align: center; width: 100%;">No favourites yet. Pin commands to see them here.</div>`;
            return;

        }





        itemsToRender.forEach(fav => {
            const cmdText = fav.commandText || fav.commandtext || "";
            const titleText = cmdText.replace(/"/g, '&quot;');
            // const favHtml = `
            //     <div class="My-Fav-Items">
            //         <div class="symbol symbol-40px symbol-circle me-5">
            //             <span class="symbol-label bg-light-warning">
            //                 <span class="material-icons material-icons-style material-icons-2">grade</span>                            
            //             </span>
            //         </div>
            //         <div class="My-Fav-Items-Content">
            //             <a href="javascript:void(0);" class="My-Fav-Name" data-bs-toggle="tooltip" data-bs-placement="bottom"
            //    data-bs-original-title="${titleText}" title="${titleText}">${cmdText}</a>
            //             <div class="My-Fav-Name-Type">Command</div>
            //         </div>
            //         <div class="edit-fav" style="cursor: pointer;" title="Rename">                        
            //             <span class="material-icons material-icons-style material-icons-2" style="color: brown;">edit</span>                          
            //         </div>
            //         <div class="Delete-Fav" style="cursor: pointer;">                        
            //             <span class="material-icons material-icons-style material-icons-2">clear</span>                          
            //         </div>
            //     </div>
            // `;

            const favHtml = `
                <div class="My-Fav-Items">
                    <div class="fav-icon symbol symbol-40px symbol-circle me-5">
                        <span class="symbol-label bg-light-warning">
                            <span class="material-icons material-icons-style material-icons-2">grade</span>                            
                        </span>
                    </div>
                    
                    <div class="My-Fav-Items-Content" style="display: flex; align-items: center; justify-content: space-between;">
                        <div style="flex-grow: 1; overflow: hidden; margin-right: 10px;">
                            <a href="javascript:void(0);" class="My-Fav-Name" data-bs-toggle="tooltip" data-bs-placement="bottom"
                   data-bs-original-title="${titleText}" title="${titleText}">${cmdText}</a>
                            <div class="My-Fav-Name-Type">Command</div>
                        </div>
                        
                        <div class="edit-fav" style="cursor: pointer; padding: 5px;" title="Rename">                        
                            <span class="material-icons material-icons-style material-icons-2" style="font-size: 16px !important; color: #6b7280;">edit</span>                          
                        </div>
                    </div>

                    <div class="Delete-Fav" style="cursor: pointer; margin-left: auto;" title="Delete">                        
                        <span class="material-icons material-icons-style material-icons-2">clear</span>                          
                    </div>
                </div>
            `;

            const div = document.createElement('div');
            div.innerHTML = favHtml.trim();
            const element = div.firstChild;

            element.querySelector('.Delete-Fav').addEventListener('mousedown', (e) => {
                e.preventDefault();
                e.stopPropagation();
                toggleFavorite(cmdText);
            });

            element.querySelector(".edit-fav").addEventListener("click", (event) => {
                event.preventDefault();
                event.stopPropagation();
                const targetUrl = fav.targetUrl || fav.targetURL || fav.targeturl || "";
                showFavoriteModel(cmdText, targetUrl, true);
            })

            element.querySelector('.My-Fav-Items-Content').addEventListener("click", (e) => {
                e.preventDefault();
                e.stopPropagation();
                executeFavorite(fav);
            })

            element.querySelector('.fav-icon').addEventListener("click", (e) => {
                e.preventDefault();
                e.stopPropagation();
                executeFavorite(fav);
            })

            wrapper.appendChild(element);

        });
    }

    function showFavoritesPopupOnly() {
        const filterInput = document.getElementById("axiFavFilterInput");
        if (filterInput) filterInput.value = "";

        if (megaDropdown) {
            megaDropdown.style.display = "flex";
        }
        if (favouritesCard) {
            favouritesCard.style.display = "flex";
        }
        const suggCard = list.closest(".card");
        if (suggCard) {
            suggCard.style.display = "none";
        }
    }

    // async function executeFavorite(cmdText) {
    //     hide(); 
    //     input.value = ""; 
    //     input.focus(); 

    //     const minTypingSpeed = 30;
    //     const maxTypingSpeed = 80;

    //     const tokens = getTokens(cmdText); 
    //     let simulatedText = ""; 

    //     for (let i=0; i < cmdText.length; i++) {
    //         input.value += cmdText[i]; 
    //         handleInput(); 
    //         while (activeFetches.size > 0) {
    //             await new Promise(resolve => setTimeout(resolve, 50));
    //         }

    //         const delay = Math.floor(Math.random() * (maxTypingSpeed - minTypingSpeed + 1)) + minTypingSpeed;
    //         await new Promise(resolve => setTimeout(resolve, delay));
    //     }

    //     input.value = cmdText + ""; 

    //     while (activeFetches.size > 0) {
    //        await new Promise(resolve => setTimeout(resolve, 50));
    //     }

    //     executeCommandsV2(); 

    //     hide(); 
    // }


    function executeFavorite(favObj) {
        if (document.querySelector(".AXI-Sec")?.classList.contains("axi-tour-active")) {
            return;
        }

        const cmdText = favObj?.originalCommandText || favObj?.originalcommandtext || favObj?.commandText || favObj?.commandtext || "";
        input.value = cmdText + " ";
        const tokens = getTokens(cmdText);

        const accessPermissions = getAccessPermissions();
        // AppMgrAccess(Config)
        // ImportAccess(Upload)
        // ExportAccess(Download)
        // Build(Open)

        for (const [permissionKey, hasAccess] of Object.entries(accessPermissions)) {
            if (!hasAccess || hasAccess === false) {
                switch (permissionKey) {
                    case "appMgrAccess":
                        if (tokens[0].toLowerCase() === "configure") {
                            showToast(`User:${window.mainUserName} has no access for command:${favObj.commandText}`);

                            return;
                        }
                        break;

                    case 'importAccess':
                        if (tokens[0].toLowerCase() === "upload") {
                            showToast(`User '${window.mainUserName}' has no access for command '${favObj.commandText}'`);

                            return;
                        }

                        break;
                    case 'exportAccess':
                        if (tokens[0].toLowerCase() === "download") {
                            showToast(`User '${window.mainUserName}' has no access for command '${favObj.commandText}'`);

                            return;
                        }
                        break;

                    case 'buildAccess':
                        if (tokens[0].toLowerCase() === "sdk" || tokens[0].toLowerCase() === "source") {
                            showToast(`User '${window.mainUserName}' has no access for command '${favObj.commandText}'`);
                            return;

                        }
                        break;

                    default:
                        break;
                }
            }
        }

        axiClearBtn.style.display = "flex";

        if (favObj.targetUrl && favObj.targetUrl.trim() !== "" && !favObj.targetUrl.startsWith("developerstudio:")) {
            console.log("Executing Favorite directly via Target URL:", favObj.targetUrl);
            const params = new URLSearchParams(favObj.targetUrl.split("?")[1]);
            const transId = params.get("transid");
            if (transId) {
                setEditSessionState(transId);
            }
            top.window.LoadIframe(favObj.targetUrl);
        } else if (favObj.targetUrl && favObj.targetUrl.startsWith("developerstudio:")) {
            console.log("Executing Favorite via Developer Studio custom target URL:", favObj.targetUrl);
            const parts = favObj.targetUrl.split(":");
            const type = parts[1]; // tstruct or iview
            const name = parts[2]; // the resolved name
            if (type === "tstruct") {
                window.openDeveloperStudio("tstreact", name, true);
            } else if (type === "iview") {
                window.openDeveloperStudio("ivreact", name, true);
            }
        } else {
            const firstToken = tokens[0];
            const secondToken = tokens[1];
            const nameToken = tokens[2]; // Resolved name / third token

            if (firstToken.toLowerCase() === "sdk" && secondToken.toLowerCase() === "tstruct") {
                window.openDeveloperStudio("tstreact", nameToken, true);
            } else {
                window.openDeveloperStudio("ivreact", nameToken, true);
            }
        }

        hide();





    }

    function setCommandRoutes(cmdText, targetUrl) {
        const existingCommandRoute = commandRoutes.find(route => route.commandText.toLowerCase() === cmdText.toLowerCase());

        if (existingCommandRoute) {
            console.log("Command route for ", cmdText, " already exists");
            return;
        }

        commandRoutes.push({
            commandText: cmdText,
            targetUrl: targetUrl
        })

        console.log("Command Routes: " + JSON.stringify(commandRoutes));
    }

    function generateLocalStorageKey(name, params) {
        const prefixKey = "axi";

        return `${prefixKey}_${name}_${params}`;


    }

    let cachedAccessPermissions = null;

    function getAccessPermissions() {
        // AppMgrAccess(Config)
        // ImportAccess(Upload)
        // ExportAccess(Download)
        // Build(Open/sdk)
        if (cachedAccessPermissions) {
            return cachedAccessPermissions;
        }
        cachedAccessPermissions = {
            appMgrAccess: strToBool(window.getSessionValue("AppMgrAccess")),
            importAccess: strToBool(window.getSessionValue("ImportAccess")),
            exportAccess: strToBool(window.getSessionValue("ExportAccess")),
            buildAccess: strToBool(window.getSessionValue("Build")),
        };
        return cachedAccessPermissions;
    }

    function buildCommandsByAccessPermissions(commandsFromDb, accessPermissions) {
        const currentUserRole = window.AxUserRoles;
        const currentUserName = window.mainUserName;


        for (const [permissionKey, hasAccess] of Object.entries(accessPermissions)) {
            if (!hasAccess || hasAccess === false) {
                switch (permissionKey) {
                    case "appMgrAccess":
                        delete commandsFromDb["Configure"];
                        break;

                    case 'importAccess':
                        delete commandsFromDb['Upload'];
                        break;
                    case 'exportAccess':
                        delete commandsFromDb['Download']
                        break;

                    case 'buildAccess':
                        delete commandsFromDb['SDK'];
                        delete commandsFromDb['Source'];
                        break;

                    default:
                        break;
                }
            }
        }

        if (!commandsFromDb["Configure"]) return commandsFromDb;

        const roles = (currentUserRole || "").split(",").map(r => r.trim().toLowerCase());
        const isAdmin = currentUserName === "admin" && roles.includes("default");
        // const isAdmin = false;

        if (!isAdmin) {
            const configurePrompts = commandsFromDb["Configure"].prompts;

            // configurePrompts.forEach(prompt => {
            //     if (prompt.prompt === "object type" && prompt.promptValues) {
            //         let values = prompt.promptValues.split(",");

            //         values = values.filter(v => v.trim() !== "User Activation");

            //         prompt.promptValues = values.join(",");
            //     }
            // });

            const objectTypePrompt = configurePrompts.find(p => p.prompt === "object type");

            const objectNamePrompt = configurePrompts.find(p => p.prompt === "object name");

            if (objectTypePrompt && objectNamePrompt) {
                const types = objectTypePrompt.promptValues.split(",");
                const sources = objectNamePrompt.promptSource.split(",");

                const index = types.findIndex(
                    t => t.trim() === "User Activation"
                );

                if (index !== -1) {
                    types.splice(index, 1);
                    sources.splice(index, 1);

                    objectTypePrompt.promptValues = types.join(",");
                    objectNamePrompt.promptSource = sources.join(",");
                }

            }
        }

        return commandsFromDb;


    }

    function setButtonLoading(buttonId, spinnerId, isLoading) {
        const button = document.getElementById(buttonId);
        const spinner = document.getElementById(spinnerId);
        if (button) {
            button.disabled = isLoading;
        }
        if (spinner) {
            if (isLoading) {
                spinner.classList.remove("d-none");
            } else {
                spinner.classList.add("d-none");
            }
        }
    }

    function strToBool(str) {
        if (!str) return false;
        const s = String(str).toLowerCase().trim();
        return s === "t" || s === "true";
    }

    function normalizeCommandForCompare(cmd) {
        if (!cmd) return "";
        return cmd.trim().toLowerCase().replace(/\s+/g, " ");
    }

    function isDuplicateFavorite(cmd) {
        if (!cmd) return false;
        const normalizedNew = normalizeCommandForCompare(cmd);
        return commandFavorites.some(fav => {
            const normalizedOrig = normalizeCommandForCompare(fav.originalCommandText);
            const normalizedText = normalizeCommandForCompare(fav.commandText);
            return normalizedOrig === normalizedNew || normalizedText === normalizedNew;
        });
    }

    function showFavoriteModel(cmdText, targetUrl, isEdit = false) {
        const originalCmdTextInput = document.getElementById("axiFavOriginalCmd");
        const favNameInput = document.getElementById("axiFavNameInput");
        const favTargetUrlInput = document.getElementById("axiFavTargetUrl");
        const favEditStateInput = document.getElementById("axiFavIsEdit");
        const titleEl = document.querySelector(".axi-modal-title");

        if (titleEl) titleEl.innerText = isEdit ? "Rename Favourites" : "Save to Favourites"

        favEditStateInput.value = isEdit ? "true" : "false";

        const axiFavModal = document.getElementById("axiFavModalOverlay");

        originalCmdTextInput.value = cmdText;
        favNameInput.value = cmdText;
        favTargetUrlInput.value = targetUrl;

        // Reset loading state on open
        setButtonLoading("axiFavSaveBtn", "axiFavSaveSpinner", false);
        const favCancelBtn = document.getElementById("axiFavCancelBtn");
        if (favCancelBtn) favCancelBtn.disabled = false;

        axiFavModal.style.display = "flex";
        axiFavModal.offsetHeight; // force layout
        axiFavModal.classList.add("open");
        favNameInput.focus();
        favNameInput.select();
    }

    function hideFavoriteModal() {
        const modal = document.getElementById("axiFavModalOverlay");

        if (modal) {
            modal.classList.remove("open");
            setTimeout(() => {
                if (!modal.classList.contains("open")) {
                    modal.style.display = "none";
                }
            }, 200);
        }
    }

    function confirmAddFavorite() {
        const alias = document.getElementById("axiFavNameInput").value.trim();
        const originalCmdText = document.getElementById("axiFavOriginalCmd").value.trim();
        const targetUrl = document.getElementById("axiFavTargetUrl").value.trim();
        const isEdit = document.getElementById("axiFavIsEdit").value === "true";
        const favCancelBtn = document.getElementById("axiFavCancelBtn"); 

        if (!alias) {
            showToast("Favorite name cannot be empty");
            return;
        }

        const aliasTokens = getTokens(alias);
        if (aliasTokens[0]?.toLowerCase() === "help" || aliasTokens[0]?.toLowerCase() === "version") {
            showToast("You cannot add this command to Favorites!");
            return;
        }

        const origTokens = getTokens(originalCmdText);
        if (origTokens[0]?.toLowerCase() === "help" || origTokens[0]?.toLowerCase() === "version") {
            showToast("You cannot add this command to Favorites!");
            return;
        }

        if (aliasTokens[0]?.toLowerCase() === "run") {
            showToast("You cannot save 'run' commands in favorites");
            return;
        }

        if (!isEdit) {
            if (isDuplicateFavorite(alias) || isDuplicateFavorite(originalCmdText)) {
                showToast("This command is already in your Favorites.");
                return;
            }
        } else {
            const isDuplicate = commandFavorites.some(fav => {
                if (fav.commandText.toLowerCase() === originalCmdText.toLowerCase()) return false;
                const normalizedText = normalizeCommandForCompare(fav.commandText);
                const normalizedOrig = normalizeCommandForCompare(fav.originalCommandText);
                const normalizedAlias = normalizeCommandForCompare(alias);
                return normalizedText === normalizedAlias || normalizedOrig === normalizedAlias;
            });
            if (isDuplicate) {
                showToast("This command is already in your Favorites.");
                return;
            }
        }

        // Start loading
        setButtonLoading("axiFavSaveBtn", "axiFavSaveSpinner", true);
        favCancelBtn.disabled = true; 

        const appUrl = getAppBaseUrl();
        const appname = getProjectName();
        const favKey = `axi_favourites_${appUrl}_${window.mainUserName}`;

        if (isEdit) {
            const cmdIndex = commandFavorites.findIndex(fav => fav.commandText.toLowerCase() === originalCmdText.toLowerCase());
            const favObj = commandFavorites.find(fav => fav.commandText.toLowerCase() === originalCmdText.toLowerCase());

            if (cmdIndex !== -1) {
                commandFavorites[cmdIndex].commandText = alias;
            }

            if (axiFavoritesUrl) {
                fetch(`${axiFavoritesUrl}/${favObj?.favouritesId}?username=${window.mainUserName}&appname=${appname}`, {
                    method: "PATCH",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        commandText: alias,
                    })
                }
                ).then(response => {
                    if (response?.ok) {
                        localStorage.setItem(favKey, JSON.stringify(commandFavorites));
                        renderFavoritesUI();
                        render();
                        hideFavoriteModal();
                        showToast(`Renamed '${originalCmdText}' to '${alias}'`, 3000, true);
                    } else {
                        setButtonLoading("axiFavSaveBtn", "axiFavSaveSpinner", false);
                        favCancelBtn.disabled = false; 
                        showToast("Failed to edit favourite");
                    }
                })
                    .catch(error => {
                        console.error("Backend edit failed", error);
                        showToast("An Error occured while editing favourite");
                        setButtonLoading("axiFavSaveBtn", "axiFavSaveSpinner", false);
                        favCancelBtn.disabled = false; 

                    });
            } else {
                setButtonLoading("axiFavSaveBtn", "axiFavSaveSpinner", false);
            }
        } else {
            if (axiFavoritesUrl) {
                fetch(`${axiFavoritesUrl}?appname=${appname}`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        username: window.mainUserName,
                        commandText: alias,
                        originalCommandText: originalCmdText,
                        action: "add",
                        favOrder: 0,
                        targetURL: targetUrl
                    })
                }).then(
                    response => {
                        if (response.ok) {
                            return response.json();
                        } else {
                            throw new Error("Failed response");
                        }
                    }
                ).then(data => {
                    console.log("Response from backend after adding favorite: ", data);
                    const favObj = data[0];

                    commandFavorites.unshift({
                        favouritesId: favObj.favouritesId,
                        username: favObj.username,
                        commandText: favObj.commandText,
                        originalCommandText: originalCmdText,
                        favOrder: favObj.favOrder,
                        targetUrl: targetUrl,
                        createdOn: favObj.createdOn
                    });

                    localStorage.setItem(favKey, JSON.stringify(commandFavorites));
                    renderFavoritesUI();
                    render();
                    hideFavoriteModal();

                    showToast(`'${alias}' added to favorites`, 5000, true);
                })
                    .catch(err => {
                        showToast("Axi: Failed to update favorite on backend");
                        console.error("Backend sync failed", err);
                        setButtonLoading("axiFavSaveBtn", "axiFavSaveSpinner", false);
                        favCancelBtn.disabled = false; 

                    });
            } else {
                setButtonLoading("axiFavSaveBtn", "axiFavSaveSpinner", false);
                        favCancelBtn.disabled = false; 

            }
        }
    }

    function showDeleteFavoriteModal(cmdText) {
        const modal = document.getElementById("axiFavDeleteModalOverlay");

        if (!modal) return;

        const favDeleteCmdText = document.getElementById("axiFavDeleteCmd");

        favDeleteCmdText.value = cmdText;
        const deleteModalParagraph = document.getElementById("axiDeleteModalParagraph");
        deleteModalParagraph.textContent = `Are you sure you want to remove '${cmdText}' from your favourites?`;

        // Reset loading state on open
        setButtonLoading("axiFavDeleteConfirmBtn", "axiFavDeleteSpinner", false);
        const deleteFavCancelBtn = document.getElementById("axiFavDeleteCancelBtn");
        if (deleteFavCancelBtn) deleteFavCancelBtn.disabled = false;

        modal.style.display = "flex";
        modal.offsetHeight; // force layout
        modal.classList.add("open");
    }

    function hideDeleteFavoriteModal() {
        const modal = document.getElementById("axiFavDeleteModalOverlay");
        if (modal) {
            modal.classList.remove("open");
            setTimeout(() => {
                if (!modal.classList.contains("open")) {
                    modal.style.display = "none";
                }
            }, 200);
        }
    }

    function confirmDeleteFavorite() {
        const cmdText = document.getElementById("axiFavDeleteCmd").value;
        if (!cmdText) return;

        executeDeleteFavorite(cmdText);
    }

    function executeDeleteFavorite(cmdText) {
        const appUrl = getAppBaseUrl();
        const appname = getProjectName();
        const favKey = `axi_favourites_${appUrl}_${window.mainUserName}`;
        const deleteFavCancelBtn = document.getElementById("axiFavDeleteCancelBtn");

        const cmdIndex = commandFavorites.findIndex(fav =>
            fav.commandText.toLowerCase() === cmdText.toLowerCase() ||
            (fav.originalCommandText && fav.originalCommandText.toLowerCase() === cmdText.toLowerCase())
        );

        if (cmdIndex !== -1) {
            const removedFav = commandFavorites[cmdIndex];

            // Start loading
            setButtonLoading("axiFavDeleteConfirmBtn", "axiFavDeleteSpinner", true);
            deleteFavCancelBtn.disabled = true; 

            if (axiFavoritesUrl) {
                fetch(`${axiFavoritesUrl}?appname=${appname}`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        username: window.mainUserName,
                        commandText: removedFav.commandText,
                        originalCommandText: removedFav.originalCommandText,
                        action: "remove",
                        favOrder: 0,
                        targetURL: removedFav.targetUrl || removedFav.targetURL || ""
                    })
                }).then(response => {
                    if (response.ok) {
                        commandFavorites.splice(cmdIndex, 1);
                        showToast(`Removed '${removedFav.commandText}' from Favorites`);

                        localStorage.setItem(favKey, JSON.stringify(commandFavorites));
                        renderFavoritesUI();
                        render();

                        hideDeleteFavoriteModal();
                    } else {
                        setButtonLoading("axiFavDeleteConfirmBtn", "axiFavDeleteSpinner", false);
            deleteFavCancelBtn.disabled = false; 

                        showToast("Failed to delete favorite on backend");
                    }
                })
                    .catch(err => {
                        console.error("Axi: Failed to delete on backend", err);
                        setButtonLoading("axiFavDeleteConfirmBtn", "axiFavDeleteSpinner", false);
            deleteFavCancelBtn.disabled = false; 

                        showToast("Error deleting favorite");
                    });
            } else {
                setButtonLoading("axiFavDeleteConfirmBtn", "axiFavDeleteSpinner", false);
            deleteFavCancelBtn.disabled = false; 

                showToast("Backend API URL is not configured. Cannot delete.");
            }
        }
    }

    function getCurrentStructName() {
        const iframe = document.getElementById("middle1");

        if (!iframe) return null;

        const src = iframe.getAttribute("src");
        const searchParams = new URLSearchParams(src.includes("?") ? src.split("?")[1] : "");
        const adInfo = searchParams.get("adInfo");
        if (!src) return null;
        if (adInfo) return null;



        const iframeDoc = iframe.contentDocument || iframe.contentWindow?.document;
        if (!iframeDoc) return null;

        const bodyId = iframeDoc.body?.id || "";

        const isEntityPage = bodyId.toLowerCase() === "entitymanagement_body";


        const cardContainer = document.querySelector(".cardsPageWrapper");

        const isCardContainerHidden = cardContainer.classList.contains("d-none");

        const ivframe = iframeDoc.getElementById("iviewFrame");

        if (isCardContainerHidden && (src.includes("Entity.aspx") || src.includes("EntityForm.aspx"))) {
            try {
                const queryString = src.includes("?") ? src.split("?")[1] : "";

                const params = new URLSearchParams(queryString);

                const structNameRaw = params.get("tstid");

                // return params.get("tstid"); 
                return { name: decodeURIComponent(structNameRaw), type: "entity" };
            } catch (error) {
                console.error("Error parsing struct name from URL: ", error);
                return null;
            }

        } else if (isCardContainerHidden && src.includes("tstruct.aspx")) {
            try {
                const queryString = src.includes("?") ? src.split("?")[1] : "";

                const params = new URLSearchParams(queryString);

                const structNameRaw = params.get("transid");

                // return params.get("tstid"); 
                return { name: decodeURIComponent(structNameRaw), type: "entity" };
            } catch (error) {
                console.error("Error parsing struct name from URL: ", error);
                return null;
            }

        } else if (ivframe !== null && src.includes("iview.aspx")) {
            try {
                const queryString = src.includes("?") ? src.split("?")[1] : "";
                const params = new URLSearchParams(queryString);
                const structNameRaw = params.get("ivname");
                // return params.get("ivname"); 
                return { name: decodeURIComponent(structNameRaw), type: "iview" };
            } catch (error) {
                console.error("Error parsing struct name from URL: ", error);
                return null;
            }
        } else if (src.includes("Smartview_table.html")) {
            try {
                const queryString = src.includes("?") ? src.split("?")[1] : "";
                const params = new URLSearchParams(queryString);
                const structNameRaw = params.get("ads");
                return { name: decodeURIComponent(structNameRaw), type: "ads" };
            } catch (error) {
                console.error("Error parsing struct name from URL: ", error);
                return null;

            }

        }

        // ../AxpertPlugins/Axi/HTMLPages/Smartview_table.html?ads=Sales%20Order&load=1769601086182&filter=eyJmaWx0ZXJzIjpbXX0=&hdnbElapsTime=0




        return null;




    }

    function startWalkthrough() {
        render(); 
        renderFavoritesUI(); 
        if (typeof introJs !== "undefined") {
            injectTourStyles();
            runTour();
            return;
        }

        const appUrl = getAppBaseUrl();

        // Dynamically load CSS
        if (!document.querySelector('link[href*="introjs.min.css"]')) {
            const link = document.createElement("link");
            link.rel = "stylesheet";
            link.type = "text/css";
            link.href = `${appUrl}/AxpertPlugins/Axi_Beta/HTMLPages/css/introjs.min.css`;
            document.head.appendChild(link);
        }

        // Dynamically load JS
        if (!document.querySelector('script[src*="intro.min.js"]')) {
            const script = document.createElement("script");
            script.type = "text/javascript";
            script.src = `${appUrl}/AxpertPlugins/Axi_Beta/HTMLPages/js/intro.min.js`;
            script.onload = () => {
                injectTourStyles();
                runTour();
            };
            script.onerror = () => {
                showToast("Failed to load walkthrough tour. Please try again.");
            };
            document.head.appendChild(script);
        } else {
            const checkInterval = setInterval(() => {
                if (typeof introJs !== "undefined") {
                    clearInterval(checkInterval);
                    injectTourStyles();
                    runTour();
                }
            }, 50);
        }
    }

    function injectTourStyles() {
        if (document.getElementById("axiTourStyles")) return;
        const style = document.createElement("style");
        style.id = "axiTourStyles";
        style.innerHTML = `
            .AXI-Sec.axi-tour-active {
                z-index: 9999999 !important;
            }
            .introjs-overlay {
                background: rgba(10, 10, 18, 0.75) !important;
                backdrop-filter: blur(4px);
            }
            .introjs-helperLayer {
                z-index: 10000000 !important;
                background: rgba(161, 0, 255, 0.1) !important;
                border: 1px solid rgba(161, 0, 255, 0.5) !important;
                box-shadow: 0 0 10px rgba(161, 0, 255, 0.3) !important;
            }
            .introjs-tooltipReferenceLayer {
                z-index: 10000001 !important;
            }
            .introjs-tooltip {
                background: #151521 !important;
                color: #ffffff !important;
                border: 1px solid #a100ff !important;
                box-shadow: 0 0 15px rgba(161, 0, 255, 0.4) !important;
                border-radius: 8px !important;
                font-family: inherit !important;
                padding: 15px !important;
                min-width: 250px !important;
            }
            .introjs-tooltiptext {
                font-size: 13px !important;
                line-height: 1.5 !important;
            }
            .introjs-arrow.bottom {
                border-top-color: #a100ff !important;
            }
            .introjs-arrow.top {
                border-bottom-color: #a100ff !important;
            }
            .introjs-arrow.left {
                border-right-color: #a100ff !important;
            }
            .introjs-arrow.right {
                border-left-color: #a100ff !important;
            }
            .introjs-button {
                background: #a100ff !important;
                color: #ffffff !important;
                border: none !important;
                text-shadow: none !important;
                border-radius: 4px !important;
                font-weight: 600 !important;
                transition: background 0.2s ease;
                padding: 6px 12px !important;
            }
            .introjs-tooltipbuttons {
                display: flex !important;
                gap: 8px !important;
                justify-content: flex-end !important;
            }
            .introjs-button:hover {
                background: #c34dff !important;
                color: #ffffff !important;
            }
            .introjs-disabled {
                background: #2e2e3e !important;
                color: #7e7e8e !important;
                cursor: not-allowed !important;
            }
            .introjs-bullets ul li a.active {
                background: #a100ff !important;
            }
            .AXI-Sec.axi-tour-active .btn,
            .AXI-Sec.axi-tour-active #runBtn,
            .AXI-Sec.axi-tour-active #btnHistoryPrev,
            .AXI-Sec.axi-tour-active #History_pages,
            .AXI-Sec.axi-tour-active #axiFavouriteBtn,
            .AXI-Sec.axi-tour-active #btnRefresh,
            .AXI-Sec.axi-tour-active .clearbtn {
                pointer-events: none !important;
                opacity: 0.5 !important;
                cursor: not-allowed !important;
            }
            .AXI-Sec.axi-tour-active .btn.introjs-showElement,
            .AXI-Sec.axi-tour-active #btnRefresh.introjs-showElement {
                opacity: 1 !important;
            }
            .introjs-bullets ul li a {
                background: #5e5e6e !important;
            }
            @media (max-width: 768px) {
                .introjs-tooltipReferenceLayer {
                    position: fixed !important;
                    top: auto !important;
                    left: 50% !important;
                    bottom: 80px !important;
                    transform: translateX(-50%) !important;
                    width: 90% !important;
                    max-width: 380px !important;
                    min-width: 280px !important;
                    z-index: 10000001 !important;
                }
                .introjs-tooltip {
                    position: absolute !important;
                    left: 0 !important;
                    right: 0 !important;
                    bottom: 0 !important;
                    top: auto !important;
                    width: 100% !important;
                    min-width: 0 !important;
                    max-width: 100% !important;
                    margin: 0 !important;
                    box-sizing: border-box !important;
                    padding: 12px !important;
                    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.6), 0 0 15px rgba(161, 0, 255, 0.4) !important;
                }
                .introjs-arrow {
                    display: none !important;
                }
                .introjs-tooltiptext {
                    font-size: 12px !important;
                }
                .introjs-button {
                    padding: 5px 10px !important;
                    font-size: 11px !important;
                }
            }
        `;
        document.head.appendChild(style);
    }

    function runTour() {
        const oldVal = input.value;
        input.value = "Help";
        input.readOnly = true;
        
        const tour = introJs();
        tour.setOptions({
            steps: [
                {
                    element: '#Axi-Searchinp',
                    intro: '<div class="d-flex align-items-center gap-2 mb-2"><span class="d-flex align-items-center justify-content-center" style="background: #a100ff; border-radius: 50%; padding: 4px; width: 26px; height: 26px;"><span class="material-icons" style="font-size: 16px; color: #ffffff;">search</span></span><strong style="color: #a100ff;">Command Search &amp; Execution</strong></div><div style="font-size: 12px; line-height: 1.6;">command syntax:<br>• <code>&quot;Customer Form&quot; create</code> (Open forms)<br>• <code>&quot;Sales Order&quot; view &quot;SO-101&quot;</code> (View record)<br>• <code>&quot;Items&quot; edit quantity 50</code> (Modify records)<br><br>Or run system administration commands:<br>• <code>configure peg &lt;pegname&gt;</code>, <code>configure settings</code><br>• <code>sdk db console</code> (Access developer tools)<br><br>Press <strong>Enter</strong> to run the command.</div>',
                    position: 'bottom'
                },
                {
                    element: '#axiAddFavoriteBtn',
                    intro: '<div class="d-flex align-items-center gap-2 mb-2"><span class="d-flex align-items-center justify-content-center" style="background: #a100ff; border-radius: 50%; padding: 4px; width: 26px; height: 26px;"><span class="material-icons" style="font-size: 16px; color: #ffffff;">bookmark_add</span></span><strong style="color: #a100ff;">Add to Favorites</strong></div><div style="font-size: 12px; line-height: 1.6;">Click this button to save the current command in the search box to your personal favorites card.<br>Name it as you like for quick reference later (excludes <code>run</code> commands).</div>',
                    position: 'bottom'
                },
                {
                    element: '#axiViewFavoritesBtn',
                    intro: '<div class="d-flex align-items-center gap-2 mb-2"><span class="d-flex align-items-center justify-content-center" style="background: #a100ff; border-radius: 50%; padding: 4px; width: 26px; height: 26px;"><span class="material-icons" style="font-size: 16px; color: #ffffff;">bookmarks</span></span><strong style="color: #a100ff;">View &amp; Search Favorites</strong></div><div style="font-size: 12px; line-height: 1.6;">Click this button to open the sliding Favorites card.<br>Use the new built-in filter field to search and execute your bookmarks instantly.</div>',
                    position: 'bottom'
                },
                {
                    element: '#axiSuggestions',
                    intro: '<div class="d-flex align-items-center gap-2 mb-2"><span class="d-flex align-items-center justify-content-center" style="background: #a100ff; border-radius: 50%; padding: 4px; width: 26px; height: 26px;"><span class="material-icons" style="font-size: 16px; color: #ffffff;">list</span></span><strong style="color: #a100ff;">Smart Autocomplete Suggestions</strong></div><div style="font-size: 12px; line-height: 1.6;">• <strong>Target-First Matching:</strong> Search results list target forms, views, and commands first.<br>• <strong>Space Auto-Quoting:</strong> Pressing space automatically wraps multi-word targets in quotes.<br>• <strong>Capability-Constrained Actions:</strong> Available actions (<code>create</code>, <code>edit</code>, <code>view</code>) are limited based on target type capability (TStructs allow all; IViews, Pages, and ADS allow <code>view</code> only).<br>• <strong>Flexible Navigation:</strong> Select <strong>Go</strong> or <strong>Pop-Up</strong> from suggestion options.</div>',
                    position: 'bottom'
                },
                {
                    element: '#btnRefresh',
                    intro: '<div class="d-flex align-items-center gap-2 mb-2"><span class="d-flex align-items-center justify-content-center" style="background: #a100ff; border-radius: 50%; padding: 4px; width: 26px; height: 26px;"><span class="material-icons" style="font-size: 16px; color: #ffffff;">refresh</span></span><strong style="color: #a100ff;">Force Reload Metadata</strong></div><div style="font-size: 12px; line-height: 1.6;">• <strong>Force Reload:</strong> Click to immediately reload structural metadata, active shell views, and database commands.</div>',
                    position: 'bottom'
                },
                {
                    element: '.searchwrap-AXI',
                    intro: '<div class="d-flex align-items-center gap-2 mb-2"><span class="d-flex align-items-center justify-content-center" style="background: #a100ff; border-radius: 50%; padding: 4px; width: 26px; height: 26px;"><span class="material-icons" style="font-size: 16px; color: #ffffff;">keyboard</span></span><strong style="color: #a100ff;">Shortcuts &amp; Quick Exit</strong></div><div style="font-size: 12px; line-height: 1.6;">• <strong>Ctrl + Enter:</strong> Execute the default <strong>Go</strong> option (opens target in current frame).<br>• <strong>Ctrl + Shift + Enter:</strong> Execute in a new <strong>Pop-Up</strong> window/modal.<br>• <strong>Esc:</strong> Closes the command palette instantly, or click anywhere outside to exit.</div>',
                    position: 'bottom'
                }
            ],
            showBullets: true,
            showStepNumbers: false,
            exitOnOverlayClick: true,
            scrollToElement: false,
            doneLabel: 'Done',
            nextLabel: 'Next &rarr;',
            prevLabel: '&larr; Back'
        });

        tour.onchange((targetElement) => {
            document.querySelector(".AXI-Sec")?.classList.add("axi-tour-active");
            if (targetElement && targetElement.id === "axiSuggestions") {
                input.value = "";
                handleInput();
                if (megaDropdown) {
                    megaDropdown.style.setProperty("display", "flex", "important");
                    megaDropdown.style.setProperty("z-index", "10000000", "important");
                    megaDropdown.style.setProperty("opacity", "1", "important");
                    void megaDropdown.offsetHeight;
                }
                // Force synchronous browser layout reflow to compute correct width/height
                void targetElement.offsetHeight;
            } else if (targetElement && targetElement.id === "axiViewFavoritesBtn") {
                loadFavorites();
                showFavoritesPopupOnly();
            } else {
                hide();
                if (megaDropdown) {
                    megaDropdown.style.zIndex = "";
                }
                if (targetElement && targetElement.id === "Axi-Searchinp") {
                    input.value = "Help";
                }
            }
        });

        let resizeTimeout;
        const handleResize = () => {
            if (resizeTimeout) clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(() => {
                try {
                    if (tour) {
                        tour.refresh();
                    }
                } catch (e) {}
            }, 100);
        };
        window.addEventListener("resize", handleResize);

        tour.onexit(() => {
            window.removeEventListener("resize", handleResize);
            if (resizeTimeout) clearTimeout(resizeTimeout);
            input.value = oldVal;
            input.readOnly = false;
            handleInput();
            document.querySelector(".AXI-Sec")?.classList.remove("axi-tour-active");
            if (megaDropdown) {
                megaDropdown.style.zIndex = "";
            }
        });
        tour.oncomplete(() => {
            window.removeEventListener("resize", handleResize);
            if (resizeTimeout) clearTimeout(resizeTimeout);
            input.value = oldVal;
            input.readOnly = false;
            handleInput();
            document.querySelector(".AXI-Sec")?.classList.remove("axi-tour-active");
            if (megaDropdown) {
                megaDropdown.style.zIndex = "";
            }
        });

        tour.start();
    }

})();


function LoadIframeac(src) {
    try {
        if (typeof callParentNew("addFormRuntimeDcFlag") != "undefined" && callParentNew("addFormRuntimeDcFlag") != "") {
            AddFormDcFieldAdded('fromconfig', src);
            return;
        }
    } catch (ex) {
        callParentNew("addFormRuntimeDcFlag=", "");
    }
    try {
        if (typeof callParentNew("formComponentsFlag") != "undefined" && callParentNew("formComponentsFlag") != "")
            checkIsPageLocked();
    } catch (ex) { }
    isTstructSplited = false;
    try {
        AxOnLoadIframe();
    }
    catch (ex) { }
    if (src.indexOf("iviewInteractive") !== 1)
        src = src.replace("iviewInteractive", "iview");

    if (window.globalChange) {
        if (confirm(appGlobalVarsObject.lcm[31])) {
            SetFormDirty(false);
        } else {
            return;
        }
    } else if ($("#middle1")[0].contentWindow.designChanged != undefined && $("#middle1")[0].contentWindow.designChanged == true) {

        if (!confirm(appGlobalVarsObject.lcm[31]))
            return;
    }
    var el = "";
    let el2 = "";
    try {
        el2 = AxOnLoadMiddleIframe(src);
        if (el2 != undefined || el2 != "") {
            // el2.src = "";
            el2.src = '../../aspx/' + src;
        }
    }
    catch (ex) { }
    if (el2 === undefined || el2 === "") {
        el = document.getElementById('middle1');
        // el.src = "";
        el.src = src;
    }

    isTstructPopup = false;
    return false;
}

