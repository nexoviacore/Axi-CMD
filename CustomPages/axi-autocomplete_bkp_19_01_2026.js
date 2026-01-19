(() => {
    // ENDPOINTS
    const API_METADATA = "http://localhost:5000/api/v1/Axi/axi_get";
    const API_AXLIST = "http://localhost:5000/api/v1/AxList";
    const API_ARM_SIGNIN = "http://localhost:5000/api/v1/Signin";

    // DOM ELEMENTS
    //const input = document.getElementById("Axi-Searchinp");
    //console.log("Input Element Found?", input);
    //const hintDiv = document.getElementById("axiHint");
    //const list = document.getElementById("axiSuggestions");
    //const btnRefresh = document.getElementById("btnRefresh");
    //const runBtn = document.getElementById("runBtn); 

    const COMMAND_HANDLERS = {
        edit: {
            source: handleEditSource,
            data: handleEditData,
            user: handleEditUser

        },
        create: {
            new: handleCreateNew,
            ads: handleCreateAds,
            page: handleCreatePage,
            card: handleCreateCard,
            user: handleCreateUser,
            usergroup: handleCreateUserGroup, // Partially Completed
            role: handleCreateRole,
            dimension: handleCreateDimension,  // Partially Completed 
            actor: handleCreateActor

        },
        view: {
            report: handleViewReport,
            dbconsole: handleViewDbConsole,
            data: handleViewData,  // Partially Completed
            inbox: handleViewInbox,
            dimension: handleViewDimension,
            user: handleViewUser,
            actor: handleViewActor,
            role: handleViewRole,


        },
        configure: {
            peg: handleConfigurePeg,
            appvar: handleConfigureAppVar,
            api: handleConfigureApi,
            devoptions: handleConfigureDevOptions,
            properties: handleConfigureProperties,
            job: handleConfigureJob,
            rule: handleConfigureRule,
            server: handleConfigureServer,
            notification: handleConfigureNotification
        }
    };

    let input;
    //console.log("Input Element Found?", input);
    let hintDiv;
    let list;
    let btnRefresh;
    let runBtn;
    let axiClearBtn;
    let isCommandTypingCompleted = false;


    let signingInPromise = null;

    const TOKEN_TTL_MS = 4 * 60 * 60 * 1000;

    // STATE
    let commands = null;
    let items = [];
    let activeIndex = -1;
    let resolvedParams = {};
    let lastTypedTokens = [];

    // DATA CACHES
    let tstructList = null;
    let isFetchingTStructs = false;
    let adsList = null;
    let isFetchingAdsList = false;
    let fieldList = null;
    let isFetchingFieldList = false;
    let fieldValueList = null;
    let isFetchingFieldValueList = false;
    let lastLoadedTStruct = null;

    let axDatasourceObj = {};
    let activeFetches = new Set();
    let filteredObjects = [];

    function init() {

        input = document.getElementById("Axi-Searchinp");


        if (!input) {
            console.log("Axi Input not ready yet... waiting.");
            setTimeout(init, 200);
            return;
        }

        console.log("Axi Input Found!", input);


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
       1. INITIALIZATION
    =============================== */
    async function initCommands(isForced = false) {
        //localStorage.setItem("arm_appname_v1", "pgbase114");
        //const origin = window.location.origin;
        let appSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        console.log("Origin: " + appSessUrl);
        const projInfoKey = `projInfo-${appSessUrl}`;

        const appname = localStorage.getItem(projInfoKey);
        console.log(appname);

        //await ensureSignedIn(appname);
        //logAuthExpiry();

        const cached = localStorage.getItem("axi_commands_v1");
        if (cached && !isForced) {
            commands = JSON.parse(cached);
        } else {
            try {
                //const accessToken = localStorage.getItem("arm_accessToken_v1");
                //const appname = localStorage.getItem("arm_appname_v1");
                const res = await fetch(`${API_METADATA}?view=metadata&forceRefresh=${isForced}&appname=${appname}`);
                if (!res.ok) throw new Error("Metadata fetch failed");
                const data = await res.json();
                commands = data.commands;
                localStorage.setItem("axi_commands_v1", JSON.stringify(commands));
            } catch (err) {
                console.error("Critical: Could not load commands", err);
            }
        }
    }
    //initCommands(false);

    /* ===============================
       ASYNC LOADERS
    =============================== */
    //async function loadTStructs() {
    //    if (isFetchingTStructs) return;
    //    isFetchingTStructs = true;
    //    console.log("Fetching TStruct List...");
    //    tstructList = await getTStructList();
    //    isFetchingTStructs = false;
    //    handleInput();
    //}

    //async function loadAds() {
    //    if (isFetchingAdsList) return;
    //    isFetchingAdsList = true;
    //    console.log("Fetching Ads List...");
    //    adsList = await getAdsList();
    //    isFetchingAdsList = false;
    //    handleInput();
    //}

    //async function loadFieldList(tstructname) {
    //    if (isFetchingFieldList) return;
    //    isFetchingFieldList = true;
    //    console.log("Fetching Field List...");
    //    fieldList = await getFieldList(tstructname);
    //    lastLoadedTStruct = tstructname;
    //    isFetchingFieldList = false;
    //    handleInput();
    //}

    //async function loadFieldValueList() {
    //    if (isFetchingFieldValueList) return;
    //    isFetchingFieldValueList = true;
    //    console.log("Fetching Field value List...");
    //    fieldValueList = await getFieldValueList();
    //    isFetchingFieldValueList = false;
    //    handleInput();
    //}

    async function loadList(sourceName, paramValue = "") {
        const key = paramValue ? `${sourceName}_${paramValue}` : sourceName;
        if (activeFetches.has(key)) return;
        activeFetches.add(key);

        console.log(`Fetching list: ${sourceName} params: ${paramValue}`);
        const data = await getList(sourceName, paramValue);

        axDatasourceObj[key] = data;
        console.log(JSON.stringify(axDatasourceObj));
        activeFetches.delete(key);
        handleInput();
    }

    /* ===============================
       REFRESH LOGIC
    =============================== */
    //if (btnRefresh) {
    //    btnRefresh.addEventListener("click", async () => {
    //        localStorage.clear();
    //        commands = null;
    //        tstructList = null;
    //        adsList = null;
    //        axDatasourceObj = {};
    //        resolvedParams = {};
    //        await initCommands(true);
    //        alert("Refreshed!");
    //        input.focus();
    //    });
    //}

    //if (runBtn) {
    //    runBtn.addEventListener("click", () => {
    //        const text = input.value.trim();
    //        if (!text || !commands) return;

    //        // 1. Tokenize Input
    //        const tokens = getTokens(text);
    //        if (tokens.length < 2) return;

    //        const groupKey = tokens[0].replace(/"/g, "").toLowerCase();
    //        const verbKey = tokens[1].replace(/"/g, "").toLowerCase();

    //        const commandConfig = commands[groupKey]?.[verbKey]; // Access nested config

    //        // Identify Command Logic
    //        if (groupKey === "edit" && verbKey === "source") {

    //            // Get config to help resolution

    //            if (!commandConfig) return;


    //            const type = tokens[2].replace(/"/g, "").toLowerCase();


    //            let rawName = tokens[3] || "";
    //            let structName = tryResolveToken(3, rawName, commandConfig, true); // forceResolve=true

    //            console.log(`Run Command: Type=${type}, Name=${structName}`);

    //            if (type === "tstruct") {
    //                openDeveloperStudio("tstreact", structName);
    //            } else if (type === "iview") {
    //                openDeveloperStudio("ivreact", structName);
    //            } else {
    //                alert("Unknown source type: " + type);
    //            }
    //        }  else if(groupKey === "create" && verbKey==="new") {
    //            console.log(text);

    //            if (!commandConfig) {
    //                console.error("No command Config");
    //            }

    //            //const type = tokens[2].replace(/"/g, "").toLowerCase();

    //            let rawName = tokens[2] || "";


    //            //console.log(`Structname : ${structName}`)

    //            let transId = tryResolveToken(2, rawName, commandConfig, true);


    //            redirectToTstruct(transId);



    //        }
    //    });
    //}

    //function redirectToTstruct(transId, isEdit = false, fieldname = "") {
    //    console.log("Redirecting to Tstruct: " + transId);

    //    let targetUrl;
    //    if (!transId) {
    //        alert("There is no Tstruct name provided!");
    //        return;
    //    }

    //    if (isEdit) targetUrl = `../aspx/tstruct.aspx?transid=${transId}fieldname=${fieldname}&act=open`;
    //    else targetUrl = `../aspx/tstruct.aspx?transid=${transId}&dummyload=false`;
    //    window.LoadIframe(targetUrl);

    //    //if (typeof LoadIframe === "function") {
    //    //    LoadIframe(targetUrl);
    //    //}
    //    //else if (window.parent && typeof window.parent.LoadIframe === "function") {

    //    //    window.parent.LoadIframe(targetUrl);
    //    //}
    //    //else {

    //    //    console.error("LoadIframe function not found");
    //    //    window.location.href = targetUrl;
    //    //}

    //    //if (typeof window.LoadIframe === "function") {
    //    //    window.LoadIframe(targetUrl);
    //    //}

    //    //else if (window.parent && typeof window.parent.LoadIframe === "function") {
    //    //    window.parent.LoadIframe(targetUrl);
    //    //}

    //    //else {
    //    //    console.error("Critical: 'LoadIframe' function not found in window or parent.");
    //    //    alert("Unable to load form: Application context missing.");
    //    //}

    //}

    function redirectToTstruct(transId, isEdit = false, fieldName = "", fieldValue = "") {
        console.log(`Redirecting to Tstruct: ${transId}, Edit: ${isEdit}, Field: ${fieldName}, Val: ${fieldValue}`);

        //LoadIframe("../aspx/tstruct.aspx?transid=tstcn&empid=HSR0019&hltype=load&torecid=false&openerIV=tstcn&isIV=false&isDupTab=false&dummyload=false♠")

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

        } else {

            targetUrl += `&dummyload=false`;
        }

        top.window.LoadIframe(targetUrl);
    }

    function redirectToIView(iViewName) {
        console.log("Redirecting to Iview: " + iViewName + "..............");
        let targetUrl = `../aspx/iview.aspx?ivname=${iViewName}`;

        window.LoadIframe(targetUrl);


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
        if (!text.trim()) {
            hintDiv.textContent = "";
            hide();
            return;
        }
        if (!commands) return;

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
        const regex = /"[^"]+"|[^\s]+/g;
        return str.match(regex) || [];
    }

    /* ===============================
       4. LOGIC ENGINE
    =============================== */
    function suggestLocal(inputText) {
        const endsWithSpace = inputText.endsWith(" ");
        const tokens = getTokens(inputText);
        if (endsWithSpace) tokens.push("");

        if (tokens.length === 0) {
            hintDiv.textContent = "";
            return Object.keys(commands);
        }

        const groupKey = tokens[0].replace(/"/g, "").toLowerCase();

        // 1. Group
        if (tokens.length === 1 && !endsWithSpace) {
            hintDiv.textContent = "";
            return Object.keys(commands).filter(k => k.startsWith(groupKey));
        }

        const groupParams = commands[groupKey];
        if (!groupParams) { hintDiv.textContent = ""; return []; }

        const targetIndex = tokens.length - 1;

        // 2. Verb
        if (targetIndex === 1) {
            const verbInput = tokens[1].replace(/"/g, "").toLowerCase();
            const matchingVerbs = Object.keys(groupParams).filter(k => k.startsWith(verbInput));
            updateDynamicHintForVerb(matchingVerbs);
            return matchingVerbs;
        }

        // 3. Parameters
        const verbKey = tokens[1].replace(/"/g, "").toLowerCase();
        const commandConfig = groupParams[verbKey];
        if (!commandConfig) { hintDiv.textContent = ""; return []; }

        const currentWordPos = targetIndex + 1;
        let activePrompt = commandConfig.prompts.find(p => p.wordPos === currentWordPos);

        updateDynamicHintFromPrompt(activePrompt);

        // Dynamic source switching
        const prevIndex = targetIndex - 1;
        if (prevIndex >= 1) {
            const prevWordPos = currentWordPos - 1;
            const prevPrompt = commandConfig.prompts.find(p => p.wordPos === prevWordPos);
            if (prevPrompt && prevPrompt.promptValues) {
                const prevValue = tokens[prevIndex].replace(/"/g, "").toLowerCase();
                const allowedValues = prevPrompt.promptValues.split(',').map(v => v.trim().toLowerCase());
                const valueIndex = allowedValues.indexOf(prevValue);
                if (valueIndex !== -1 && activePrompt && activePrompt.promptSource) {
                    const sources = activePrompt.promptSource.split(',');
                    if (sources.length > valueIndex) {
                        activePrompt = { ...activePrompt, promptSource: sources[valueIndex].trim() };
                    }
                }
            }
        }

        const partialTyped = tokens[targetIndex].replace(/"/g, "").toLowerCase();

        // Generic list handler
        if (activePrompt && activePrompt.promptSource) {
            const sourceName = activePrompt.promptSource;
            let paramValue = "";

            if (activePrompt.promptParams) {
                const indices = activePrompt.promptParams.toString().split(',');
                const values = indices.map(idx => {
                    const dependencyIndex = parseInt(idx.trim()) - 1;
                    if (resolvedParams[dependencyIndex]) {
                        return resolvedParams[dependencyIndex];
                    }
                    let tokenText = (tokens[dependencyIndex] || "").replace(/"/g, "");
                    // Try to resolve this token NOW
                    const resolved = tryResolveToken(dependencyIndex, tokenText, commandConfig);
                    return resolved;
                });
                paramValue = values.join(',');
            }

            let apiSourceName = sourceName.toLowerCase();
            const sourceKey = paramValue ? `${apiSourceName}_${paramValue}` : apiSourceName;

            if (!axDatasourceObj[sourceKey]) {
                const hasValidParams = !activePrompt.promptParams || (paramValue && paramValue.replace(/,/g, '').trim().length > 0);
                if (hasValidParams) {
                    console.log(`Triggering load for ${apiSourceName} with param: ${paramValue}`);
                    loadList(apiSourceName, paramValue);
                    return [`Loading ${sourceName}...`];
                } else {
                    return ["Waiting for input..."];
                }
            }

            const dataList = axDatasourceObj[sourceKey];
            const filtered = dataList.filter(item => {
                const display = item.displaydata;
                if (display == null) return false;
                return display.toLowerCase().includes(partialTyped);
            });

            filteredObjects = filtered;
            return filtered.map(item => item.displaydata);
        }

        // Static Values
        if (activePrompt && activePrompt.promptValues) {
            const staticValues = activePrompt.promptValues.split(',');
            const result = staticValues.filter(val => val.toLowerCase().startsWith(partialTyped));
            filteredObjects = result.map(val => ({ name: val, displaydata: val }));
            return result;
        }

        return [];
    }

    /* ===============================
       LAZY RESOLUTION HELPER
    =============================== */
    //function tryResolveToken(tokenIndex, tokenText, commandConfig, forceResolve = false) {
    //    tokenText = tokenText.replace(/"/g, "");

    //    if (resolvedParams[tokenIndex] && !forceResolve) {
    //        return resolvedParams[tokenIndex];
    //    }

    //    const wordPos = tokenIndex + 1;
    //    const prompt = commandConfig.prompts.find(p => p.wordPos === wordPos);

    //    if (!prompt || !prompt.promptSource) return tokenText;

    //    if (prompt.promptValues) {
    //        const staticValues = prompt.promptValues.split(',').map(v => v.trim().toLowerCase());
    //        if (staticValues.includes(tokenText.toLowerCase())) {
    //            resolvedParams[tokenIndex] = tokenText.toLowerCase();
    //            return tokenText.toLowerCase();
    //        }
    //        return tokenText;
    //    }

    //    const sourceName = prompt.promptSource;
    //    let cacheKey = sourceName;
    //    if (prompt.promptParams) {
    //        const indices = prompt.promptParams.toString().split(',');
    //        const values = indices.map(idx => {
    //            const depIndex = parseInt(idx.trim()) - 1;
    //            const depToken = (getTokens(input.value)[depIndex] || "").replace(/"/g, "");
    //            return tryResolveToken(depIndex, depToken, commandConfig);
    //        });
    //        const paramValue = values.join(',');
    //        if (paramValue) cacheKey = `${sourceName}_${paramValue}`;
    //    }

    //    const cachedList = axDatasourceObj[cacheKey];
    //    if (cachedList) {
    //        const foundItem = cachedList.find(item =>
    //            (item.displaydata && item.displaydata.toLowerCase() === tokenText.toLowerCase()) ||
    //            (item.caption && item.caption.toLowerCase() === tokenText.toLowerCase()) ||
    //            (item.name && item.name.toLowerCase() === tokenText.toLowerCase())
    //        );

    //        if (foundItem) {
    //            const realValue = foundItem.name || foundItem.sqlname || foundItem.displaydata;
    //            console.log(`✅ Lazy Resolved: "${tokenText}" → "${realValue}"`);
    //            resolvedParams[tokenIndex] = realValue;
    //            return realValue;
    //        }
    //    }

    //    return tokenText;
    //}

    /* ===============================
       LAZY RESOLUTION HELPER 
    =============================== */
    function tryResolveToken(tokenIndex, tokenText, commandConfig, forceResolve = false) {

        tokenText = tokenText.replace(/"/g, "").trim();


        if (resolvedParams[tokenIndex] && !forceResolve) {
            return resolvedParams[tokenIndex];
        }

        // Get Configuration
        if (!commandConfig || !commandConfig.prompts) return tokenText;
        const wordPos = tokenIndex + 1;
        const prompt = commandConfig.prompts.find(p => p.wordPos === wordPos);

        if (!prompt || !prompt.promptSource) return tokenText;

        // Handle Static Values (e.g. "tstruct, iview")
        if (prompt.promptValues) {
            const staticValues = prompt.promptValues.split(',').map(v => v.trim().toLowerCase());
            if (staticValues.includes(tokenText.toLowerCase())) {
                return tokenText.toLowerCase();
            }
            return tokenText;
        }

        // Determine Cache Key (Handle Dependencies)
        const sourceName = prompt.promptSource;
        let cacheKey = sourceName.toLowerCase();

        // Handle dependencies (e.g. FieldList needs TStruct name)
        if (prompt.promptParams) {
            const indices = prompt.promptParams.toString().split(',');
            const values = indices.map(idx => {
                const depIndex = parseInt(idx.trim()) - 1;
                const depToken = (getTokens(input.value)[depIndex] || "").replace(/"/g, "");

                return tryResolveToken(depIndex, depToken, commandConfig, true);
            });
            const paramValue = values.join(',');


            let apiSourceName = sourceName;
            // if (sourceName === "FieldList") apiSourceName = "AxpFieldList";

            if (paramValue) cacheKey = `${apiSourceName}_${paramValue}`.toLowerCase();
        }

        // Look in Cache
        const cachedList = axDatasourceObj[cacheKey];

        if (cachedList) {

            const foundItem = cachedList.find(item =>
                (item.displaydata && item.displaydata.toLowerCase() === tokenText.toLowerCase()) ||
                (item.caption && item.caption.toLowerCase() === tokenText.toLowerCase()) ||
                (item.name && item.name.toLowerCase() === tokenText.toLowerCase()) ||
                (item.sqlname && item.sqlname.toLowerCase() === tokenText.toLowerCase())
            );

            if (foundItem) {

                const realValue = foundItem.name || foundItem.sqlname || foundItem.displaydata;
                console.log(`Auto-Resolved: "${tokenText}" -> "${realValue}"`);


                resolvedParams[tokenIndex] = realValue;
                return realValue;
            } else {
                console.warn(`Cache Hit for ${cacheKey}, but item "${tokenText}" not found in list.`);
            }
        } else {
            console.warn(`Cache Miss for key: ${cacheKey}. List not loaded yet.`);
        }


        return tokenText;
    }

    /* ===============================
       5. RENDER & APPLY
    =============================== */
    function render() {
        console.log("Render called");
        list.innerHTML = "";
        activeIndex = -1;

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
        if (!items[index] || items[index] === "Loading options...") return;

        let suggestion = items[index];
        let displayName = suggestion;
        let realValue = suggestion;

        // Get Real Value using Lookup
        realValue = GetObjectName(suggestion);

        // Clean up Display Name
        if (suggestion.includes("(") && suggestion.includes(")")) {
            const lastBracketIndex = suggestion.lastIndexOf("(");
            displayName = suggestion.substring(0, lastBracketIndex).trim();
        } else {
            displayName = suggestion;
        }

        // Identify Token Position
        const currentInput = input.value;
        const endsWithSpace = currentInput.endsWith(" ");
        const tokens = getTokens(currentInput);
        const targetIndex = endsWithSpace ? tokens.length : tokens.length - 1;

        // Store Real Value
        resolvedParams[targetIndex] = realValue;
        console.log("Updated Params:", resolvedParams);

        // Auto-Quote Display Name if needed
        if (displayName.includes(" ")) {
            displayName = `"${displayName}"`;
        }

        // Update Input
        if (endsWithSpace) {
            tokens.push(displayName);
        } else {
            tokens[targetIndex] = displayName;
        }

        input.value = tokens.join(" ") + " ";
        handleInput();
        hide();
        input.focus();
    }

    /* ===============================
       HELPERS & EVENTS
    =============================== */
    function updateDynamicHintForVerb(matchingVerbs) {
        if (matchingVerbs.length === 1) {
            hintDiv.textContent = `Next: ${matchingVerbs[0]}`;
            hintDiv.style.color = "#f59e0b";
        } else if (matchingVerbs.length > 0) {
            hintDiv.textContent = `Suggestion: ${matchingVerbs[0]}...`;
            hintDiv.style.color = "#9ca3af";
        } else {
            hintDiv.textContent = "";
        }
    }

    function updateDynamicHintFromPrompt(prompt) {
        if (prompt) {
            let label = prompt.prompt || "value";
            if (prompt.promptValues && !prompt.prompt) {
                label = prompt.promptValues.split(',').join(' / ');
            }
            hintDiv.textContent = `Next: <${label}>`;
            hintDiv.style.color = "#f59e0b";
        } else {
            hintDiv.textContent = "✅ Ready to Run";
            hintDiv.style.color = "#22c55e";
            isCommandTypingCompleted = true;

        }
    }

    // Add event listeners
    //if (input) {
    //    input.addEventListener("input", handleInput);
    //    //input.addEventListener("blur", () => {
    //    //    setTimeout(() => {
    //    //        if (!input.value) hintDiv.textContent = "";
    //    //    }, 200);
    //    //});

    //    input.addEventListener("keydown", e => {
    //        if (list.style.display !== "block" || items.length === 0) return;

    //        if (e.key === "ArrowDown") {
    //            e.preventDefault();
    //            activeIndex = (activeIndex + 1) % items.length;
    //            highlight();
    //        }
    //        if (e.key === "ArrowUp") {
    //            e.preventDefault();
    //            activeIndex = (activeIndex - 1 + items.length) % items.length;
    //            highlight();
    //        }
    //        if (e.key === "Tab") {
    //            e.preventDefault();
    //            if (activeIndex === -1) activeIndex = 0;
    //            apply(activeIndex);
    //        }
    //        if (e.key === "Enter" && activeIndex >= 0) {
    //            e.preventDefault();
    //            apply(activeIndex);
    //        }
    //        if (e.key === "Escape") hide();
    //    });
    //}

    function highlight() {
        if (list.children.length > 0) {
            [...list.children].forEach((li, i) => {
                li.classList.toggle("active", i === activeIndex);
            });
        }
    }

    document.addEventListener("click", e => {
        if (input && list && e.target !== input && !list.contains(e.target)) {
            hide();
        }
    });

    function getDataFromAxListSuccess(result) {
        console.log(`AxList data` + JSON.stringify(result));
    }

    function getDataFromAxListFail(error) {
        showToast(error)
        console.error(`AxList data` + error);
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

            //const accessToken = localStorage.getItem("arm_accessToken_v1");
            //const armSessionId = localStorage.getItem("arm_armSessionId_v1");

            //if (!accessToken || !armSessionId) {
            //    console.error("Missing auth/session data");
            //    return [];
            //}

            // ---- Build sqlparams dynamically ----
            const sqlParams = {};
            const normalizedParams = [];



            if (paramValuesCsv && typeof paramValuesCsv === "string") {
                const values = paramValuesCsv
                    .split(",")
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
                sqlParams: sqlParams
            };

            //const res = await fetch(API_AXLIST, {
            //    method: "POST",
            //    headers: {
            //        "Content-Type": "application/json",
            //        Authorization: `Bearer ${accessToken}`
            //    },
            //    body: JSON.stringify(requestBody)


            const res = await getAxListAsync(requestBody);

            console.log("Get List data: " + JSON.stringify(res));

            //if (!res.ok) {
            //    console.log("API error:", res.status, res.statusText);
            //    showToast(`Something went wrong ${res.statusText}`); 
            //    return [];
            //}

            //console.log(`STatus : ${res.status}`)

            const dataObj = typeof res === "string" ? JSON.parse(res) : res;

            console.log("DATA obj is : " + dataObj);
            console.log("Type of DATA OBJ: " + typeof dataObj);
            //if (res.status === 206) {
            //    const errorMsg = dataObj?.result?.data?.[0]?.error;

            //    if (errorMsg) {
            //        console.error("API Partial Error:", errorMsg);
            //        showToast(`Error: ${errorMsg}`);
            //        return [];
            //    }
            //}
            const list = dataObj?.result?.data?.[0]?.data ?? [];

            if (list.length > 0) {
                localStorage.setItem(cacheKey, JSON.stringify(list));

            } else console.log(`List Data for Ads name ${axDatasourceName} is Empty`);


            return list;

        } catch (err) {
            //console.error("getList failed:", err);
            //showToast(`Error: ${err}`); 
            return [];
        }
    }

    //function logAuthExpiry() {
    //    const expiresAt = Number(localStorage.getItem("arm_auth_expiresAt_v1"));

    //    if (!expiresAt) {
    //        console.log("Auth expiry: NOT SET");
    //        return;
    //    }

    //    const remainingMs = expiresAt - Date.now();

    //    if (remainingMs <= 0) {
    //        console.log("Auth expired");
    //        return;
    //    }

    //    const minutes = Math.floor(remainingMs / 60000);
    //    const seconds = Math.floor((remainingMs % 60000) / 1000);

    //    console.log(`Auth expires in ${minutes}m ${seconds}s`);
    //}



    //async function ensureSignedIn(appname) {
    //    if (isAuthValid()) return;

    //    if (!signingInPromise) {
    //        signingInPromise = signIn(appname)
    //            .finally(() => signingInPromise = null);
    //    }

    //    await signingInPromise;
    //}

    /* ===============================
       TOAST HELPER
    =============================== */
    function showToast(message, duration = 5000) {

        const toast = document.createElement("div");
        toast.textContent = message;


        Object.assign(toast.style, {
            position: "fixed",
            bottom: "20px",
            right: "20px",
            backgroundColor: "#ef4444", // Red-500
            color: "white",
            padding: "12px 24px",
            borderRadius: "8px",
            boxShadow: "0 4px 6px rgba(0,0,0,0.1)",
            zIndex: "10000",
            fontFamily: "sans-serif",
            fontSize: "14px",
            opacity: "0",
            transition: "opacity 0.3s ease-in-out"
        });

        document.body.appendChild(toast);


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

    function saveAuth(accessToken, armSessionId) {
        const expiresAt = Date.now() + TOKEN_TTL_MS;

        localStorage.setItem("arm_accessToken_v1", accessToken);
        localStorage.setItem("arm_armSessionId_v1", armSessionId);
        localStorage.setItem("arm_auth_expiresAt_v1", expiresAt.toString());
    }

    //function isAuthValid() {
    //    const token = localStorage.getItem("arm_accessToken_v1");
    //    const session = localStorage.getItem("arm_armSessionId_v1");
    //    const expiresAt = Number(localStorage.getItem("arm_auth_expiresAt_v1"));

    //    if (!token || !session || !expiresAt) return false;

    //    if (Date.now() >= expiresAt) {
    //        clearAuth();
    //        return false;
    //    }

    //    return true;
    //}

    //function clearAuth() {
    //    localStorage.removeItem("arm_accessToken_v1");
    //    console.log("Arm accesstoken removed........");
    //    localStorage.removeItem("arm_armSessionId_v1");
    //    console.log("Arm session id removed..........");
    //    localStorage.removeItem("arm_auth_expiresAt_v1");
    //    console.log("Arm expiry removed.........");
    //}



    //async function signIn(appname) {
    //    console.log("Sign in called.........")

    //    try {
    //        //const appname = localStorage.getItem("arm_appname_v1"); 
    //        const requestBody = {
    //            //appname: "pgbase114", //agileerpbaselocal // orclbase114local //ax114 //ghcmdev
    //            appname: appname,
    //            UserName: "admin",  //mohan // salesexecutive//indiauser //admin //laksh@transper.com
    //            password: "22723bbd4217a0abf6d3e68073c7603d", //827ccb0eea8a706c4c34a16891f84e7b //22723bbd4217a0abf6d3e68073c7603d //cb636c00783cdf430eedd449fcfd10c3// //827ccb0eea8a706c4c34a16891f84e7b
    //            Language: "English",
    //            SessionId: "12345",
    //            Globalvars: true, //true
    //            ClearPreviousSession: true,//true
    //            trace: true
    //        }

    //        //const cachedAccessToken = localStorage.getItem("arm_access_token");

    //        //if (accessToken) {
    //        //    return JSON.parse(cached);
    //        //}



    //        const res = await fetch(`${API_ARM_SIGNIN}`, {
    //            method: "POST",
    //            headers: {
    //                "Content-Type": "application/json",
    //                //"Authorization": `Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkFSTS1JTlRFUk5BTC0xNzBCMzYwQ0VDMTkwNjk5MkEzMjhSUzI1NiJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzaWQiOiJBUk0tcGdiYXNlMTE0LTU3MDc0YjAwLTg4NWUtNGQ0Mi1hYjUyLTY4NWI4OWI0MDc2MiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiQVJNLXBnYmFzZTExNC01NzA3NGIwMC04ODVlLTRkNDItYWI1Mi02ODViODliNDA3NjIiLCJuYmYiOjE3NjYyMjgxMjQsImV4cCI6MTc2NjIzMTcyNCwiaXNzIjoiQXhwZXJ0IC0gQVJNIiwiYXVkIjoiQXhwZXJ0IC0gQVJNIn0.c2Ju3kk5mxnAlCULdxoqgRKqc2SRbh-mBQOvkuvMmBE`
    //            },
    //            body: JSON.stringify(requestBody)

    //        });

    //        if (res.ok) {

    //            const dataObj = await res.json();
    //            //if (dataObj && dataObj.result && dataObj.result.data[0]) {
    //            const accessToken = dataObj.result.token;
    //            const armSessionId = dataObj.result.ARMSessionId;
    //            //saveAuth(accessToken, armSessionId);
    //            console.log("ARM Sign in successfull");


    //        }




    //        //}
    //        //return [];



    //    } catch (err) {
    //        console.log(err);
    //        return [];
    //    }
    //}

    /* =================================== 
        EXECUTE COMMAND 
    ======================================
    */

    function executeCommands() {
        const text = input.value.trim();
        if (!text || !commands) return;
        const tokens = getTokens(text);
        if (tokens.length < 2) return;

        const groupKey = tokens[0].replace(/"/g, "").toLowerCase();
        const verbKey = tokens[1].replace(/"/g, "").toLowerCase();
        const commandConfig = commands[groupKey]?.[verbKey];

        if (groupKey === "edit" && verbKey === "source") {
            if (!commandConfig) return;
            const type = tokens[2].replace(/"/g, "").toLowerCase();
            let rawName = tokens[3] || "";
            rawName = rawName.replace(/['"]/g, "").trim();
            let structName = tryResolveToken(3, rawName, commandConfig, false);

            console.log("StructName = " + structName);

            if (structName === rawName && type === "tstruct") {
                const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
                if (list) {
                    const found = list.find(x => x.caption.toLowerCase() === rawName.toLowerCase());
                    if (found && found.name) {
                        console.log(`fallback Resolved: ${rawName} -> ${found.name}`);
                        structName = found.name;
                    }
                }
            }

            if (structName === rawName && type === "iview") {
                const list = axDatasourceObj["Axi_IViewList".toLowerCase()];
                if (list) {
                    const found = list.find(x => x.caption.toLowerCase() === rawName.toLowerCase());
                    if (found && found.name) {
                        console.log(`fallback Resolved: ${rawName} -> ${found.name}`);
                        structName = found.name;
                    }
                }
            }

            if (type === "tstruct") window.openDeveloperStudio("tstreact", structName);
            else if (type === "iview") window.openDeveloperStudio("ivreact", structName);
            else alert("Unknown source type: " + type);
        } else if (groupKey === "create") {
            if (!commandConfig) {
                console.error("No command Config");
            }

            if (verbKey === "new") {
                //const type = tokens[2].replace(/"/g, "").toLowerCase();

                let rawName = tokens[2] || "";


                //console.log(`Structname : ${structName}`)

                let transId = tryResolveToken(2, rawName, commandConfig, false);

                if (transId === rawName) {
                    const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
                    if (list) {
                        const found = list.find(x => x.caption.toLowerCase() === rawName.toLowerCase());
                        if (found) transId = found.name;
                    }
                }


                redirectToTstruct(transId);

            } else if (verbKey.toLowerCase() === "ads") {
                // return ActButtonClick('btn_newsql','','');
                let adsRawName = tokens[2] || "";
                console.log("Ads Raw Name: " + adsRawName);

                rawName = adsRawName.replace(/['"]/g, "").trim();

                window.openDeveloperStudio("icsqlist");

                //  setTimeout(() => { console.log("Waiting for Ads List IView to Open....")}, 500); 


                // top.window.ActButtonClick('btn_newsql', '', ''); 
            } else if (verbKey.toLowerCase() === "page") {
                let adsRawName = tokens[2] || "";
                console.log("Ads Raw Name: " + adsRawName);

                rawName = adsRawName.replace(/['"]/g, "").trim();


                window.openDeveloperStudio("ihplist");

            }





        } else if (groupKey === "edit" && verbKey === "data") {

            console.log(text);

            if (!commandConfig) {
                console.error("No command Config");
            }

            //const type = tokens[2].replace(/"/g, "").toLowerCase();

            let rawName = tokens[2] || "";


            //console.log(`Structname : ${structName}`)

            let transId = tryResolveToken(2, rawName, commandConfig, false);

            if (transId === rawName) {
                const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
                if (list) {
                    const found = list.find(x => x.caption.toLowerCase() === rawName.toLowerCase());
                    if (found) transId = found.name;
                }
            }

            let rawField = tokens[3] || "";
            rawField = rawField.replace(/['"]/g, "").trim();
            // Note: promptParams in metadata handles the dependency on transId
            let fieldName = tryResolveToken(3, rawField, commandConfig, true);


            let rawValue = tokens[4] || "";
            rawValue = rawValue.replace(/['"]/g, "").trim();
            let fieldValue = tryResolveToken(4, rawValue, commandConfig, true);

            console.log(`Edit Data: TransId=${transId}, Field=${fieldName}, Value=${fieldValue}`);

            redirectToTstruct(transId, true, fieldName, fieldValue);



        } else if (groupKey === "view") {

            if (verbKey === "report") {
                if (!commandConfig) {
                    console.error("No command Config");
                    return;
                }

                //const type = tokens[2].replace(/"/g, "").toLowerCase();

                let rawName = tokens[2] || "";

                if (!rawName) {
                    console.log("No IView Name Provided!");
                    return;
                }

                //console.log(`Structname : ${structName}`)

                let iViewName = tryResolveToken(2, rawName, commandConfig, false);

                //if (rawName) {
                if (iViewName === rawName) {
                    const list = axDatasourceObj["Axi_IViewList".toLowerCase()];
                    console.log(JSON.stringify(list));

                    if (!Array.isArray(list) || list.length === 0) {
                        console.error("Axi_IViewList is Missing  or Invalid!");
                        return;
                    }

                    const found = list.find(x => x.caption.toLowerCase() === rawName.toLowerCase());
                    if (!found) {
                        console.error("Iview not found for Caption!: " + rawName);
                        return;
                    }

                    iViewName = found.name;


                }



                //}


                redirectToIView(iViewName);

            } else if (verbKey.toLowerCase() === "dbconsole") {
                // openDeveloperStudio(&quot;AxDBScript.aspx&quot;);
                window.openDeveloperStudio("AxDBScript.aspx");
            }







        }
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

                    await initCommands(true);
                    alert("Refreshed!");
                    input.focus();

                } catch (error) {
                    console.log("Refresh Failed: " + error);
                    alert("Error refreshing: " + error);

                }


            });
        }

        //function getName(caption, type = "") {

        //}

        if (axiClearBtn) {
            axiClearBtn.addEventListener("click", () => {
                input.value = "";
                handleInput();
                input.focus();
            })
        }



        if (runBtn) {
            runBtn.addEventListener("click", executeCommandsV2);
        }

        input.addEventListener("input", handleInput);
        input.addEventListener("blur", () => setTimeout(() => { if (!input.value) hintDiv.textContent = ""; }, 200));
        input.addEventListener("keydown", e => {
            if (e.key === "Enter" && e.ctrlKey) {
                e.preventDefault();
                console.log("Debug: Enter + shift Detected Executing..........");
                executeCommandsV2();
                hide();
                return;
            }
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

                if (input.value.trim() !== "") {
                    input.value = "";
                    handleInput();
                    input.focus();
                } else {
                    hide();
                }
            }
        });

        document.addEventListener("click", e => {
            if (input && list && e.target !== input && !list.contains(e.target)) hide();
        });
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
        }
    }

    function executeCommandsV2() {
        const text = input.value.trim();
        if (!text || !commands) return;

        const tokens = getTokens(text);
        if (tokens.length < 2) return;

        const commandTokens = buildCommandTokens(tokens);
        if (!commandTokens) return;

        dispatchCommand(commandTokens);
    }

    function buildCommandTokens(tokens) {
        const group = cleanCommandToken(tokens[0]);
        const verb = cleanCommandToken(tokens[1]);

        const groupConfig = commands[group];
        if (!groupConfig) {
            console.warn("Unknown group:", group);
            return null;
        }

        const commandConfig = groupConfig[verb];
        if (!commandConfig) {
            console.warn("Unknown verb:", verb);
            return null;
        }

        return {
            text: tokens.join(" "),
            tokens,
            group,
            verb,
            commandConfig
        };

    }

    function cleanCommandToken(val = "") {
        return val.replace(/['"]/g, "").trim();
    }

    function dispatchCommand(cmdTokens) {
        const handler = COMMAND_HANDLERS[cmdTokens.group]?.[cmdTokens.verb];

        if (!handler) {
            console.error(`No handler for ${cmdTokens.group} ${cmdTokens.verb}`);
            return;
        }

        handler(cmdTokens);
    }

    function handleCreateNew({ tokens, commandConfig }) {
        let rawName = cleanCommandToken(tokens[2]);
        let transId = tryResolveToken(2, rawName, commandConfig, false);

        if (transId === rawName) {
            const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
            const found = list?.find(
                x => x.caption.toLowerCase() === rawName
            );
            if (found) transId = found.name;
        }

        redirectToTstruct(transId);
    }

    function handleCreateAds({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        targetUrl = "../aspx/tstruct.aspx?transid=b_sql";

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&sqlname=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }
        // window.LoadIframe("../aspx/tstruct.aspx?transid=b_sql");
    }

    function handleCreateCard({ tokens, commandConfig }) {
        // LoadIframeac(&quot;ivtoivload.aspx?ivname=axusers
        // window.LoadIframe("ivtoivload.aspx?ivname=axpcards");
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        targetUrl = "../aspx/tstruct.aspx?transid=a__cd";

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&cardname=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }






    }

    function handleCreateUser({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        targetUrl = "../aspx/tstruct.aspx?transid=axusr";

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&nickname=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=axusr"); 
    }

    function handleCreateUserGroup({ tokens, commandConfig }) {

        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        targetUrl = "../aspx/tstruct.aspx?transid=a__ug";

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&users_group_name=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }
        // window.LoadIframe("../aspx/tstruct.aspx?transid=a__ug");

    }

    /* ============== View Commands Functions =========================
       ----------------- Start ------------------------------------------
    */

    function handleViewUser({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        // if (rawName) {
        //     paramName = tryResolveToken(2, rawName, commandConfig, false);

        // }

        //  var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;

       

        if (!rawName) {
             targetUrl = "../aspx/Entity.aspx?tstid=axusr";
            window.LoadIframe(targetUrl);

        } else {
             targetUrl = "../aspx/EntityForm.aspx?tstid=axusr";
            targetUrl += `&pusername=${rawName}`;

            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=axusr"); 
    }

    function handleViewActor({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        // if (rawName) {
        //     paramName = tryResolveToken(2, rawName, commandConfig, false);

        // }

        //  var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;

       

        if (!rawName) {
             targetUrl = "../aspx/Entity.aspx?tstid=ad_am";
            window.LoadIframe(targetUrl);

        } else {
             targetUrl = "../aspx/EntityForm.aspx?tstid=ad_am";
            targetUrl += `&actorname=${rawName}`;

            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=axusr"); 
    }

    function handleViewRole({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        // if (rawName) {
        //     paramName = tryResolveToken(2, rawName, commandConfig, false);

        // }

        //  var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;

        

        if (!rawName) {
            targetUrl = "../aspx/Entity.aspx?tstid=ad_ur";
            window.LoadIframe(targetUrl);

        } else {
            targetUrl = "../aspx/EntityForm.aspx?tstid=ad_ur";
            targetUrl += `&axusergroup=${rawName}`;

            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=axusr"); 
    }



    function handleViewReport({ tokens, commandConfig }) {
        let rawName = cleanCommandToken(tokens[2]);
        if (!rawName) return;

        let ivName = tryResolveToken(2, rawName, commandConfig, false);

        if (ivName === rawName) {
            const list = axDatasourceObj["Axi_IViewList".toLowerCase()];
            const found = list?.find(
                x => x.caption.toLowerCase() === rawName
            );
            if (!found) return;
            ivName = found.name;
        }

        redirectToIView(ivName);
    }

    function handleViewDbConsole() {
        window.openDeveloperStudio("AxDBScript.aspx");

    }

    function handleViewInbox() {
        // LoadIframe('processflow.aspx?activelist=t')
        window.LoadIframe('../aspx/processflow.aspx?activelist=t');

    }

    function handleViewDimension({ tokens, commandConfig }) {
        // LoadIframe('processflow.aspx?activelist=t')



        var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;

        let targetUrl = `../aspx/Entity.aspx?tstid=a__na`;
        window.LoadIframe(targetUrl);

    }

    function handleViewData({ tokens, commandConfig }) {

        let rawStruct = cleanCommandToken(tokens[2]);
        let rawField = cleanCommandToken(tokens[3]);
        let rawValue = cleanCommandToken(tokens[4]);



        let transid = tryResolveToken(2, rawStruct, commandConfig, false);
        let searchField = tryResolveToken(3, rawField, commandConfig, false);
        let searchValue = tryResolveToken(4, rawValue, commandConfig, false);

        if (transid === rawStruct) {
            const list = axDatasourceObj["Axi_TStructList".toLowerCase()];

            const found = list?.find(x => x.caption.toLowerCase() === rawStruct);

            if (!found) {
                console.warn("No Tstruct found for caption: " + rawStruct);
                return;
            }

            transid = found.name;

        }

        let targetUrl = `../aspx/Entity.aspx?tstid=${transid}`;

        if (!searchField && !searchValue) {
            // targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);
        } else {
            targetUrl += `&${searchField}=${searchValue}`;
            // targetUrl += "&act=open";
            // targetUrl += "&dummyload=false♠"

        }




        // LoadIframe('Entity.aspx?tstid=mrplo')






    }



    /* ============= End =================== */
    function handleCreatePage({ tokens, commandConfig }) {

        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        targetUrl = "../aspx/tstruct.aspx?transid=sect";

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&cardname=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }



        // window.openDeveloperStudio("ihplist");
        //  window.LoadIframe("../aspx/tstruct.aspx?transid=sect"); 

    }

    function handleCreateRole({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        targetUrl = "../aspx/tstruct.aspx?transid=ad_ur";

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&axusergroup=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }



        // window.LoadIframe("../aspx/tstruct.aspx?transid=ad_ur");

    }

    function handleCreateActor({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }



        targetUrl = "../aspx/tstruct.aspx?transid=ad_am";

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&actorname=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=ad_am");

    }

    function handleCreateDimension({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let rawFieldName = cleanCommandToken(tokens[2]);
        let rawFieldValue = cleanCommandToken(tokens[3]);

        let fieldName = tryResolveToken(2, rawFieldName, commandConfig, false);
        let fieldValue = tryResolveToken(3, rawFieldValue, commandConfig, false);



        targetUrl = "../aspx/tstruct.aspx?transid=a__na";

        if (rawFieldName && rawFieldValue) {
            targetUrl += `&${rawFieldName}=${rawFieldValue}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);


        } else {

            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=a__ag");

    }



    /***************************************************
     * Edit Command Function
     * **************************************************
    */

    function handleEditSource({ tokens, commandConfig }) {

        if (tokens.length < 4) {
            console.warn("edit source requires <type> <name>");
            return;
        }

        const type = cleanCommandToken(tokens[2]);
        let rawName = cleanCommandToken(tokens[3]);

        let resolvedName = tryResolveToken(3, rawName, commandConfig, false);


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
            window.openDeveloperStudio("tstreact", resolvedName);
        } else if (type === "iview") {
            window.openDeveloperStudio("ivreact", resolvedName);
        } else {
            alert("Unknown source type: " + type);
        }
    }


    function handleEditData({ tokens, commandConfig }) {

        if (tokens.length < 5) {
            console.warn("edit data requires <tstruct> <field> <value>");
            return;
        }


        let rawStruct = cleanCommandToken(tokens[2]);
        let transId = tryResolveToken(2, rawStruct, commandConfig, false);

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


        let rawField = cleanCommandToken(tokens[3]);
        const fieldName = tryResolveToken(3, rawField, commandConfig, true);

        if (!fieldName) {
            console.error("Field resolution failed:", rawField);
            return;
        }


        let rawValue = cleanCommandToken(tokens[4]);
        const fieldValue = tryResolveToken(4, rawValue, commandConfig, true);

        if (fieldValue == null) {
            console.error("Field value resolution failed:", rawValue);
            return;
        }

        console.log(
            `Edit Data → TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`
        );

        redirectToTstruct(transId, true, fieldName, fieldValue);
    }

    function handleEditUser({ tokens, commandConfig }) {




        let rawUserName = cleanCommandToken(tokens[2]);

        let resolvedUserName = tryResolveToken(2, rawUserName, commandConfig, false);


        // if (resolvedName === rawName) {
        //     const listKey =
        //         type === "tstruct"
        //             ? "Axi_TStructList"
        //             : type === "iview"
        //                 ? "Axi_IViewList"
        //                 : null;

        //     if (!listKey) {
        //         alert("Unknown source type: " + type);
        //         return;
        //     }

        //     const list = axDatasourceObj[listKey];
        //     const found = list?.find(
        //         x => x.caption?.toLowerCase() === rawName
        //     );

        //     if (!found || !found.name) {
        //         console.error(`Source not found: ${rawName}`);
        //         return;
        //     }

        //     resolvedName = found.name;
        // }

        targetUrl = "../aspx/tstruct.aspx?transid=axusr";

        targetUrl += `&hltype=load`;
        targetUrl += `&torecid=false`;
        targetUrl += `&openerIV=axusr`;
        targetUrl += `&isIV=false`;
        targetUrl += `&isDupTab=false`;


        // if (!paramName) {
        //     window.LoadIframe(targetUrl);

        // } else {
        targetUrl += `&nickname=${rawUserName}`;

        targetUrl += "&dummyload=false♠"
        window.LoadIframe(targetUrl);

        // }



    }

    /***************************************************
    * End
    * **************************************************
   */

    /***************************************************
     * Configure Commands Functions
     * *************************************************
     */

    function handleConfigureAppVar({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;iaxvars&quot;);
        // window.openDeveloperStudio("iaxvars"); 
        window.LoadIframe("../aspx/tstruct.aspx?transid=axvar");

    }

    function handleConfigureApi({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;iexapidef&quot;);
        // window.openDeveloperStudio("iexapidef"); 
        let rawApiName = cleanCommandToken(tokens[2]);
        let targetUrl = "../aspx/tstruct.aspx?transid=apidg";

        if (!rawApiName) {
            window.LoadIframe(targetUrl);
        } else {

            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=apidg`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;



            targetUrl += `&ExecAPIDefName=${rawApiName}`;

            targetUrl += "&dummyload=false♠"


            window.LoadIframe(targetUrl);

        }



    }

    function handleConfigureRule({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;iexapidef&quot;);
        // window.openDeveloperStudio("iexapidef"); 
        let rawParamName = cleanCommandToken(tokens[2]);
        let targetUrl = "../aspx/tstruct.aspx?transid=ad_re";

        if (!rawParamName) {
            window.LoadIframe(targetUrl);
        } else {

            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=ad_re`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;



            targetUrl += `&rulename=${rawParamName}`;

            targetUrl += "&dummyload=false♠"


            window.LoadIframe(targetUrl);

        }



    }

    function handleConfigureServer({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;iexapidef&quot;);
        // window.openDeveloperStudio("iexapidef"); 
        let rawParamName = cleanCommandToken(tokens[2]);
        let targetUrl = "../aspx/tstruct.aspx?transid=axpub";

        if (!rawParamName) {
            window.LoadIframe(targetUrl);
        } else {

            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=axpub`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;



            targetUrl += `&servername=${rawParamName}`;

            targetUrl += "&dummyload=false♠"


            window.LoadIframe(targetUrl);

        }



    }

    function handleConfigurePeg({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;iexapidef&quot;);
        // window.openDeveloperStudio("iexapidef"); 
        let rawParamName = cleanCommandToken(tokens[2]);
        let targetUrl = "../aspx/tstruct.aspx?transid=ad_pn";

        if (!rawParamName) {
            window.LoadIframe(targetUrl);
        } else {

            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=ad_pn`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;



            targetUrl += `&servername=${rawParamName}`;

            targetUrl += "&dummyload=false♠"


            window.LoadIframe(targetUrl);

        }



    }

    function handleConfigureNotification({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;iexapidef&quot;);
        // window.openDeveloperStudio("iexapidef"); 
        let rawParamName = cleanCommandToken(tokens[2]);
        let targetUrl = "../aspx/tstruct.aspx?transid=a__fn";

        if (!rawParamName) {
            window.LoadIframe(targetUrl);
        } else {

            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=a__fn`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;



            targetUrl += `&servername=${rawParamName}`;

            targetUrl += "&dummyload=false♠"


            window.LoadIframe(targetUrl);

        }



    }

    function handleConfigureDevOptions({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;idop_list&quot;);
        // window.openDeveloperStudio("idop_list"); 
        window.LoadIframe("../aspx/tstruct.aspx?transid=axstc");

    }

    function handleConfigureProperties({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;idop_list&quot;);
        // window.openDeveloperStudio("idop_list"); 
        window.LoadIframe("../aspx/tstruct.aspx?transid=ad_pr");

    }

    function handleConfigureJob({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;idop_list&quot;);
        // window.openDeveloperStudio("idop_list"); 
        let rawParamName = cleanCommandToken(tokens[2]);
        let targetUrl = "../aspx/tstruct.aspx?transid=job_s";

        if (!rawParamName) {
            window.LoadIframe(targetUrl);
        } else {

            targetUrl += `&hltype=load`;
            targetUrl += `&torecid=false`;
            targetUrl += `&openerIV=job_s`;
            targetUrl += `&isIV=false`;
            targetUrl += `&isDupTab=false`;



            targetUrl += `&jname=${rawParamName}`;

            targetUrl += "&dummyload=false♠"


            window.LoadIframe(targetUrl);

        }
        // window.LoadIframe("../aspx/tstruct.aspx?transid=job_s")

    }

    /*********************************************************
      * End 
      * ******************************************************
      */

})();