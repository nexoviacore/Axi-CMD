(() => {
    // 29/01/2026 - New Axi Command Pallete Behaviour Changes
    // ENDPOINTS
    const API_METADATA = "http://localhost:5000/api/v1/Axi/axi_get";

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
        edit: {
            default: handleEditData,
            data: handleEditData,
            // user: handleEditUser



        },
        create: {
            default: handleCreateNew,
            ads: handleCreateAds,
            card: handleCreateCard,
            page: handleCreatePage

        },
        view: {
            default: handleViewCommand,
            report: handleViewReport,
            dbconsole: handleViewDbConsole,
            data: handleViewData,
            inbox: handleViewInbox,
            dimension: handleViewDimension,
            user: handleViewUser,
            usergroup: handleViewUsergroup,
            actor: handleViewActor,
            role: handleViewRole,


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
            dbconsole: handleViewDbConsole,

            //default: handleOpen
            //default: (ctx) => console.log("Open handler", ctx) 
        },
        upload: {
            default: handleUpload
        },
        download: {
            default: handleDownload
        }
    };

    let input;
    //console.log("Input Element Found?", input);
    let hintDiv;
    let list;
    let btnRefresh;
    let runBtn;
    let axiClearBtn;
    let axiLogo;
    let searchWrapper;
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



        const cached = localStorage.getItem("axi_commands_v1");
        if (cached && !isForced) {
            commands = JSON.parse(cached);
            console.log(JSON.stringify(commands));

        } else {
            try {

                const res = await fetch(`${API_METADATA}?view=metadata&forceRefresh=${isForced}&appname=${appname}`);
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



    /**
     * Determines the active prompt and the specific source string to use.
     * Handles the logic where "Source A, Source B" maps to "Type A, Type B".
     *
     */
    function getActivePromptAndSource(commandConfig, tokens, targetIndex) {

        const currentWordPos = targetIndex + 1;

        const prompt = commandConfig.prompts.find(p => p.wordPos === currentWordPos);
        if (!prompt) return null;

        let activeSource = prompt.promptSource || "";


        if (activeSource.includes(",")) {

            const prevWordPos = currentWordPos - 1;
            const prevPrompt = commandConfig.prompts.find(p => p.wordPos === prevWordPos);

            if (prevPrompt && prevPrompt.promptValues) {

                const prevTokenIndex = targetIndex - 1;
                const prevValue = cleanString(tokens[prevTokenIndex]);


                const allowedValues = prevPrompt.promptValues.split(',').map(v => v.trim().toLowerCase());
                const valueIndex = allowedValues.indexOf(prevValue.toLowerCase());

                if (valueIndex !== -1) {
                    const sources = activeSource.split(',');

                    if (sources[valueIndex]) {
                        activeSource = sources[valueIndex].trim();
                    } else {

                        activeSource = "";
                    }
                }
            }
        }

        return {
            config: prompt,
            realSource: activeSource
        };
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


            if (prevPrompt && prevPrompt.promptSource.toLowerCase() === 'axi_keyvalueswithfieldnameslist') {

                const prevTokenRaw = cleanString(tokens[targetIndex - 1]);


                const sourcePrefix = prevPrompt.promptSource.toLowerCase();
                let foundItem = null;


                for (const key in axDatasourceObj) {
                    if (key.startsWith(sourcePrefix)) {
                        const list = axDatasourceObj[key];

                        foundItem = list.find(item =>
                            (item.name && item.name.toLowerCase() === prevTokenRaw.toLowerCase()) ||
                            (item.displaydata && item.displaydata.toLowerCase() === prevTokenRaw.toLowerCase())
                        );
                        if (foundItem) break;
                    }
                }

                // If found, and isfield is 'f' , STOP THE PROMPT CHAIN.
                if (foundItem && foundItem.isfield === 'f') {
                    console.log("Short Circuit: Previous item is not a field. Stopping prompts.");
                    return null;
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
                const valueIndex = allowedValues.indexOf(prevValue.toLowerCase());

                if (valueIndex !== -1) {
                    const sources = activeSource.split(',');
                    activeSource = sources[valueIndex] ? sources[valueIndex].trim() : "";
                }
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


    function redirectToSmartView(adsname) {
        let targetUrl = "../CustomPages/Smartview_table_1769088257557.html";


        targetUrl += `?ads=${adsname}`;
        targetUrl += "&load=1769601086182";

        top.window.LoadIframe(targetUrl);

        // LoadIframe('../pgbase114/HTMLPages/SmartView_1769601086182.html?ads=axi_userlist&load=1769601086182')
    }

    function redirectToPermissionScreeen(username) {
        // aspx/tstruct.aspx?act=open&transid=a__up&axusername=aarav&fromsource=U&openerIV=axusers&isIV=true&isDupTab=true-1769600154391&dummyload=false

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
        //   targetUrl += `&isDupTab=true-${Date.now()}`;
        targetUrl += `&isDupTab=true`;


        //   targetUrl += "&dummyload=false♠";   
        targetUrl += "&dummyload=false";

        setEditSessionState(transId);
        console.log(`LoadIframe called with Url: ${targetUrl}`);


        top.window.LoadIframe(targetUrl);

    }



    // function redirectToTstruct(transId, isEdit = false, fieldName = "", fieldValue = "") {
    //     console.log(`Redirecting to Tstruct: ${transId}, Edit: ${isEdit}, Field: ${fieldName}, Val: ${fieldValue}`);



    //     if (!transId) {
    //         alert("There is no Tstruct name provided!");
    //         return;
    //     }


    //     let targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

    //     if (isEdit) {






    //         if (fieldName && fieldValue) {
    //             targetUrl += `&${fieldName}=${encodeURIComponent(fieldValue)}`;
    //         }
    //         targetUrl += `&hltype=load`;
    //         targetUrl += `&torecid=false`;
    //         targetUrl += `&openerIV=${transId}`;
    //         targetUrl += `&isIV=false`;
    //         targetUrl += `&isDupTab=false`;

    //         targetUrl += `&dummyload=false♠`;

    //     } else {

    //         targetUrl += `&dummyload=false`;
    //     }

    //     top.window.LoadIframe(targetUrl);
    // }

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



        //    LoadIframe('AddEditResponsibility.aspx?status=true&action=edit&name=demorole')


        let targetUrl = "../aspx/AddEditResponsibility.aspx";

        if (fieldValue) {
            targetUrl += "?status=true";
            targetUrl += "&action=edit";
            targetUrl += `&name=${encodeURIComponent(fieldValue)}`
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

    // function redirectToProcessFlow(caption) {
    //     console.log(`Redirecting to Process flox for caption:  ${caption}`);




    //     let targetUrl = `../aspx/processflow.aspx`;
    //     targetUrl += "?loadcaption=AxProcessBuilder"

    //     if (caption) {
    //         targetUrl += `&processname=${encodeURIComponent(caption)}`;
    //     }


    //     top.window.LoadIframe(targetUrl);
    // }

    function redirectToProcessFlow(caption) {
        console.log(`Redirecting to Process flox for caption:  ${caption}`);




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
        // if (!text.trim()) {
        //     hintDiv.textContent = "";
        //     hide();
        //     return;
        // }
        if (!commands) return;

        if (!text.trim()) {
            items = Object.keys(commands);
            hintDiv.textContent = "";
            render();
            return;
        }

        render();

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
    // function getTokens(str) {
    //     // const regex = /"[^"]+"|[^\s]+/g;
    //     const regex = /"[^"]*"?|[^\s]+/g;
    //     return str.match(regex) || [];
    // }

    function getTokens(str) {
        const regex = /"[^"]*"?|[^\s]+/g;
        return str.match(regex) || [];
    }

    function cleanString(val) {
        return (val || "").replace(/['"]/g, "").trim();
    }





    /* ===============================
       4. Suggestion Logic
    =============================== */
    //    function suggestLocal(inputText, ignoreExtraParams = false) {
    //         const tokens = getTokens(inputText);
    //         const endsWithSpace = inputText.endsWith(" ");

    //         // Handle unclosed quotes logic for space
    //         const lastTokenRaw = tokens[tokens.length - 1];
    //         const isUnclosedString = lastTokenRaw && lastTokenRaw.startsWith('"') && (!lastTokenRaw.endsWith('"') || lastTokenRaw === '"');

    //         if (endsWithSpace && !isUnclosedString) {
    //             tokens.push("");
    //         }

    //         if (tokens.length === 0) {
    //             hintDiv.textContent = "";
    //             return Object.keys(commands);
    //         }

    //         const groupKey = cleanString(tokens[0]);

    //         // 1. Suggest Group (create, edit, etc.)
    //         if (tokens.length === 1 && !endsWithSpace) {
    //             hintDiv.textContent = "";
    //             return Object.keys(commands).filter(k => k.startsWith(groupKey));
    //         }

    //         const commandConfig = commands[groupKey];
    //         if (!commandConfig) {
    //             hintDiv.textContent = "";
    //             return [];
    //         }

    //         // 2. Suggest Parameters based on Prompts
    //         const targetIndex = tokens.length - 1;
    //         const promptInfo = getActivePromptAndSource(commandConfig, tokens, targetIndex);

    //         if (!promptInfo) {
    //              // No prompt definition for this position
    //             hintDiv.textContent = "";
    //             updateDynamicHintFromPrompt(null);
    //             isCommandTypingCompleted = true; // Likely finished
    //             return [];
    //         }

    //         const { config: activePrompt, realSource} = promptInfo;
    //         updateDynamicHintFromPrompt(activePrompt);

    //         const partialTyped = cleanString(tokens[targetIndex]);




    //         if (!realSource && activePrompt.promptValues) {
    //              const staticValues = activePrompt.promptValues.split(',').map(v => v.trim());
    //              const result = staticValues.filter(val => val.toLowerCase().startsWith(partialTyped.toLowerCase()));
    //              filteredObjects = result.map(val => ({ name: val, displaydata: val }));
    //              return result;
    //         }



    //         if (realSource) {
    //             // Resolve dependencies if promptParams exists
    //             let paramValue = "";
    //             if (activePrompt.promptParams) {
    //                 const indices = activePrompt.promptParams.toString().split(',');
    //                 const values = indices.map(idx => {
    //                     const depTokenIndex = parseInt(idx.trim()) - 1; // Convert wordPos to token index
    //                      // Resolve the dependency token
    //                     const depToken = cleanString(tokens[depTokenIndex] || "");
    //                     return tryResolveToken(depTokenIndex, depToken, commandConfig, true);
    //                 });
    //                 paramValue = values.join(',');
    //             }

    //             if (activePrompt.extraParams && !ignoreExtraParams) {

    //                 const extraSource = activePrompt.extraParams.toLowerCase(); 

    //                 const extraKey = `${extraSource}_${paramValue}`.toLowerCase();


    //                 if (!axDatasourceObj[extraKey]) {

    //                     console.log(`Fetching Hidden Param Source: ${extraSource}`);
    //                     loadList(extraSource, paramValue);

    //                     // return [`Loading configuration...`];
    //                 }


    //                 const extraList = axDatasourceObj[extraKey];
    //                 if (extraList && extraList.length > 0) {
    //                     const hiddenValue = extraList[0].name || extraList[0].displaydata;
    //                     console.log(`Hidden Param Found: ${hiddenValue}`);

    //                     if (paramValue) paramValue += "," + hiddenValue;
    //                     else paramValue = hiddenValue;
    //                 } else {
    //                     return ["Error: Configuration not found"];
    //                 }


    //             }



    //             let apiSourceName = realSource.toLowerCase();



    //             // Case-insensitive cache key
    //             const sourceKey = (paramValue ? `${apiSourceName}_${paramValue}` : apiSourceName).toLowerCase();

    //             if (!axDatasourceObj[sourceKey]) {
    //                 const hasValidParams = !activePrompt.promptParams || (paramValue && paramValue.replace(/,/g, '').trim().length > 0);

    //                 if (hasValidParams) {
    //                     // Trigger fetch
    //                     loadList(apiSourceName, paramValue);
    //                     return [`Loading ${realSource}...`];
    //                 } else {
    //                     return ["Waiting for previous input..."];
    //                 }
    //             }

    //             // Filter cached list
    //             const dataList = axDatasourceObj[sourceKey];
    //             const filtered = dataList.filter(item => {
    //                 const display = item.displaydata || item.name || item.caption || "";
    //                 return display.toLowerCase().includes(partialTyped.toLowerCase());
    //             });

    //             filteredObjects = filtered;
    //             return filtered.map(item => item.displaydata || item.caption || item.name);
    //         }

    //         return [];
    //     }


    function suggestLocal(inputText) {
        const tokens = getTokens(inputText);
        const endsWithSpace = inputText.endsWith(" ");

        const lastTokenRaw = tokens[tokens.length - 1];
        const isUnclosedString = lastTokenRaw && lastTokenRaw.startsWith('"') && (!lastTokenRaw.endsWith('"') || lastTokenRaw === '"');
        if (endsWithSpace && !isUnclosedString) tokens.push("");

        if (tokens.length === 0) { hintDiv.textContent = ""; return Object.keys(commands); }

        const groupKey = cleanString(tokens[0]);
        if (tokens.length === 1 && !endsWithSpace) {
            hintDiv.textContent = "";
            return Object.keys(commands).filter(k => k.startsWith(groupKey));
        }

        const commandConfig = commands[groupKey];
        if (!commandConfig) { hintDiv.textContent = ""; return []; }

        const targetIndex = tokens.length - 1;
        const promptInfo = getActivePromptInfo(commandConfig, tokens, targetIndex);

        if (!promptInfo) {
            updateDynamicHintFromPrompt(null);
            return [];
        }

        const { config: activePrompt, realSource } = promptInfo;
        updateDynamicHintFromPrompt(activePrompt);

        const partialTyped = cleanString(tokens[targetIndex]);

        // Scenario A: Static Values
        if (!realSource && activePrompt.promptValues) {
            const staticValues = activePrompt.promptValues.split(',').map(v => v.trim());
            const result = staticValues.filter(val => val.toLowerCase().startsWith(partialTyped.toLowerCase()));
            filteredObjects = result.map(val => ({ name: val, displaydata: val }));
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
                paramValue = values.join(',');
            }


            if (activePrompt.extraParams) {
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
                    if (paramValue) paramValue += "," + hiddenValue;
                    else paramValue = hiddenValue;
                } else {
                    // console.error("Error: Configuration not found (Empty List)"); 
                    // showToast("Error: Configuration not found (Empty List)"); 
                    return [];
                }
            }


            let apiSourceName = realSource.toLowerCase();
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
            return filtered.map(item => item.displaydata || item.caption || item.name || item.fname || item.keyfield);
        }

        return [];
    }


    /* ===============================
       LAZY RESOLUTION HELPER 
    =============================== */
    //   function tryResolveToken(tokenIndex, tokenText, commandConfig, forceResolve = false) {
    //         tokenText = cleanString(tokenText);

    //         if (resolvedParams[tokenIndex] && !forceResolve) return resolvedParams[tokenIndex];
    //         if (!commandConfig) return tokenText;

    //         const promptInfo = getActivePromptAndSource(commandConfig, getTokens(input.value), tokenIndex);
    //         if (!promptInfo) return tokenText;

    //         const { config: prompt, realSource } = promptInfo;

    //         // Static Value Check
    //         // if (!realSource && prompt.promptValues) {
    //         //      const staticValues = prompt.promptValues.split(',').map(v => v.trim().toLowerCase());
    //         //      if (staticValues.includes(tokenText.toLowerCase())) return tokenText.toLowerCase();
    //         //      return tokenText;
    //         // }

    //         // Data Source Check
    //         if (realSource) {
    //             // let cacheKey = realSource;
    //             let paramVal
    //             if (prompt.promptParams) {
    //                 const indices = prompt.promptParams.toString().split(',');
    //                 const values = indices.map(idx => {
    //                     // Recursion for dependencies
    //                     const depIndex = parseInt(idx.trim()) - 1; 
    //                     const depToken = cleanString(getTokens(input.value)[depIndex] || "");
    //                     return tryResolveToken(depIndex, depToken, commandConfig, true);
    //                 });
    //                 paramVal = values.join(',');

    //             }

    //             // if (!isVirtual && prompt.extraParams) {

    //             //      const prevToken = cleanString(getTokens(input.value)[tokenIndex - 1] || "");
    //             //      if(paramValue) paramValue += "," + prevToken;
    //             //      else paramValue = prevToken;
    //             // }

    //             if (prompt.extraParams) {
    //                 const extraSource = prompt.extraParams; 
    //                 const extraKey = `${extraSource}_${paramValue}`.toLowerCase();
    //                 const extraList = axDatasourceObj[extraKey];

    //                 if (extraList && extraList.length > 0) {
    //                     const hiddenValue = extraList[0].name;
    //                     if (paramValue) paramValue += "," + hiddenValue;
    //                     else paramValue = hiddenValue;
    //                 }
    //             }

    //             let apiName = realSource; 
    //             let cacheKey = `${apiName}_${paramVal}`;
    //             const cachedList = axDatasourceObj[cacheKey.toLowerCase()];
    //             if (cachedList) {
    //                 const found = cachedList.find(item => 
    //                     (item.displaydata && item.displaydata.toLowerCase() === tokenText.toLowerCase()) ||
    //                     (item.name && item.name.toLowerCase() === tokenText.toLowerCase()) || 
    //                     (item.caption && item.caption.toLowerCase() === tokenText.toLowerCase() )
    //                 );
    //                 if (found) {
    //                     const realValue = found.name || found.sqlname || found.displaydata;
    //                     resolvedParams[tokenIndex] = realValue; // Cache it
    //                     return realValue;
    //                 }
    //             }
    //         }

    //         return tokenText;
    //     }

    // function tryResolveToken(tokenIndex, tokenText, commandConfig, forceResolve = false) {
    //         tokenText = cleanString(tokenText);


    //         if (resolvedParams[tokenIndex] && !forceResolve) return resolvedParams[tokenIndex];


    //         if (!tokenText && !forceResolve) return ""; 

    //         if (!commandConfig) return tokenText;

    //         const promptInfo = getActivePromptAndSource(commandConfig, getTokens(input.value), tokenIndex);
    //         if (!promptInfo) return tokenText;

    //         const { config: prompt, realSource } = promptInfo;

    //         // Static Value Check
    //         if (!realSource && prompt.promptValues) {
    //              const staticValues = prompt.promptValues.split(',').map(v => v.trim().toLowerCase());
    //              if (staticValues.includes(tokenText.toLowerCase())) return tokenText.toLowerCase();
    //              return tokenText;
    //         }

    //         // Data Source Check
    //         if (realSource) {
    //             let paramValue = "";

    //             // Resolve PromptParams (Dependencies)
    //             if (prompt.promptParams) {
    //                 const indices = prompt.promptParams.toString().split(',');
    //                 const values = indices.map(idx => {
    //                     const logicalWordPos = parseInt(idx.trim());
    //                     // Calculate token index based on WordPos. 
    //                     // WordPos 1 = Group (Token 0). WordPos 2 = Param 1 (Token 1).
    //                     const depTokenIndex = logicalWordPos - 1; 

    //                     const currentTokens = getTokens(input.value);
    //                     const depToken = cleanString(currentTokens[depTokenIndex] || "");

    //                     // Recursive Resolution
    //                     return tryResolveToken(depTokenIndex, depToken, commandConfig, true); 
    //                 });
    //                 paramValue = values.join(',');
    //             }

    //             // Handle Hidden Extra Params for Resolution context
    //             if (prompt.extraParams) {
    //                  const currentTokens = getTokens(input.value);
    //                  // If we are at the value, the previous token *might* be relevant if it wasn't hidden.
    //                  // But since it IS hidden, we have to look it up from cache based on the dependencies.

    //                  const extraSource = prompt.extraParams;
    //                  // Note: We use the paramValue (Tstruct Name) we just resolved above
    //                  const extraKey = `${extraSource}_${paramValue}`.toLowerCase();
    //                  const extraList = axDatasourceObj[extraKey];

    //                  if (extraList && extraList.length > 0) {
    //                      const hiddenValue = extraList[0].name || extraList[0].displaydata;
    //                      if (paramValue) paramValue += "," + hiddenValue;
    //                      else paramValue = hiddenValue;
    //                  }
    //             }

    //             let apiName = realSource;
    //             let cacheKey = paramValue ? `${apiName}_${paramValue}` : apiName;

    //             const cachedList = axDatasourceObj[cacheKey.toLowerCase()];
    //             if (cachedList) {
    //                 const found = cachedList.find(item => 
    //                     (item.displaydata && item.displaydata.toLowerCase() === tokenText.toLowerCase()) ||
    //                     (item.caption && item.caption.toLowerCase() === tokenText.toLowerCase()) ||
    //                     (item.name && item.name.toLowerCase() === tokenText.toLowerCase())
    //                 );
    //                 if (found) {
    //                     const real = found.name || found.sqlname || found.displaydata;
    //                     resolvedParams[tokenIndex] = real;
    //                     return real;
    //                 }
    //             }
    //         } 

    //         return tokenText;
    //     }



    function tryResolveToken(tokenIndex, tokenText, commandConfig, forceResolve = false) {
        tokenText = cleanString(tokenText);
        if (resolvedParams[tokenIndex] && !forceResolve) return resolvedParams[tokenIndex];
        if (!tokenText && !forceResolve) return "";
        if (!commandConfig) return tokenText;

        const currentTokens = getTokens(input.value);


        const promptInfo = getActivePromptInfo(commandConfig, getTokens(input.value), tokenIndex);
        if (!promptInfo) return tokenText;

        const { config: prompt, realSource } = promptInfo;

        // if (prompt.extraParams) {
        //     let extraParamParentValue = "";

        //     // We must resolve the parent dependency to build the cache key (e.g., TStruct Name)
        //     // We look at promptParams to find what this token depends on.
        //     if (prompt.promptParams) {
        //         const indices = prompt.promptParams.toString().split(',');
        //         const values = indices.map(idx => {
        //             const logicalWordPos = parseInt(idx.trim());
        //             const depIndex = logicalWordPos - 1;


        //             const depToken = cleanString(currentTokens[depIndex] || "");
        //             return tryResolveToken(depIndex, depToken, commandConfig, true);
        //         });
        //         extraParamParentValue = values.join(',');
        //     }

        //     // Reconstruct the cache key: "axi_keyfieldlist_mytstruct"
        //     const extraSource = prompt.extraParams.toLowerCase();
        //     const extraKey = `${extraSource}_${extraParamParentValue}`.toLowerCase();
        //     const extraList = axDatasourceObj[extraKey];

        //     // If the hidden list exists, RETURN THE FIELD NAME immediately.
        //     if (extraList && extraList.length > 0) {
        //         // Prioritize fname, fall back to name or displaydata
        //         const fieldName = extraList[0].fname || extraList[0].keyfield || extraList[0].name || extraList[0].displaydata;

        //         console.log(`[tryResolveToken] Intercepted Token ${tokenIndex}: Swapping '${tokenText}' for Hidden Field '${fieldName}'`);

        //         return fieldName;
        //     }
        // }

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
                paramValue = values.join(',');
            }

            // Append Hidden Param for Resolution Context
            if (prompt.extraParams) {
                const extraSource = prompt.extraParams;
                const extraKey = `${extraSource}_${paramValue}`.toLowerCase();
                const extraList = axDatasourceObj[extraKey];

                if (extraList && extraList.length > 0) {
                    const hiddenValue = extraList[0].name || extraList[0].keyfield || extraList[0].fname || extraList[0].displaydata;
                    if (paramValue) paramValue += "," + hiddenValue;
                    else paramValue = hiddenValue;
                }
            }

            let apiName = realSource;
            let cacheKey = paramValue ? `${apiName}_${paramValue}` : apiName;

            const cachedList = axDatasourceObj[cacheKey.toLowerCase()];
            if (cachedList) {
                const found = cachedList.find(item =>
                    (item.displaydata || "").toLowerCase() === tokenText.toLowerCase() ||
                    (item.caption || "").toLowerCase() === tokenText.toLowerCase() ||
                    (item.name || "").toLowerCase() === tokenText.toLowerCase()
                );
                if (found) {
                    let real = found.name || found.sqlname || found.displaydata;

                    if (real.includes("(") && real.includes(")")) {
                        const match = real.match(/\(([^)]+)\)/); 

                        real = match ? match[1]: real; 

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
        // activeIndex = -1;
        if (activeIndex < 0) activeIndex = 0;

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
        if (!items[index] || items[index] === "Loading options...") return;

        let suggestion = items[index];
        let displayName = suggestion;
        let realValue = ""; 

        // Get Real Value logic
        const foundObj = filteredObjects.find(item => item.displaydata === suggestion);

        if (foundObj?.displaydata?.includes("(") && foundObj?.displaydata?.includes(")")) {
            const match = foundObj.displaydata.match(/\(([^)]+)\)/); 

            realValue = match ? match[1]: null; 

        } else {
        realValue = foundObj ? (foundObj.name || foundObj.sqlname || foundObj.displaydata) : suggestion;


        }


        if (suggestion.includes("(") && suggestion.includes(")")) {
            const lastBracketIndex = suggestion.lastIndexOf("(");

            // if (lastBracketIndex > 0 && suggestion[lastBracketIndex - 1] === '-') {
            //     lastBracketIndex = lastBracketIndex - 1; 
            // }
            const text = suggestion.substring(0, lastBracketIndex)
                // .replace(/\-\s*\([^)]*\)\s*$/, "")
                .trim();


            displayName = text.replace(/-$/, "");


            // displayName = suggestion.replace(/\-\s*\([^)]*\)\s*$/, "").trim();


        }

        if (displayName.includes("[") && displayName.includes("]")) {
            const lastBracketIndex = suggestion.lastIndexOf("[");
            const text = suggestion.substring(0, lastBracketIndex).trim();

            displayName = text.replace(/\s*\[[^\]]*\]$/, "").trim();
        }

        const currentInput = input.value;
        const tokens = getTokens(currentInput);


        const endsWithSpace = currentInput.endsWith(" ");
        const lastTokenRaw = tokens[tokens.length - 1];

        // Check if we are inside an unclosed quote
        const isUnclosedString = lastTokenRaw && lastTokenRaw.startsWith('"') && (!lastTokenRaw.endsWith('"') || lastTokenRaw === '"');


        if (endsWithSpace && !isUnclosedString) {
            tokens.push("");
        }


        let targetIndex = tokens.length - 1;
        if (targetIndex < 0) {
            targetIndex = 0;
            tokens.push("");
        }

        resolvedParams[targetIndex] = realValue;

        // Auto-Quote if necessary
        if (displayName.includes(" ")) {
            displayName = `"${displayName}"`;
        }

        tokens[targetIndex] = displayName;

        input.value = tokens.join(" ") + " ";
        handleInput();
        hide();
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
            hintDiv.textContent = "✅ Ready to Run";
            hintDiv.style.color = "#22c55e";
            isCommandTypingCompleted = true;

        }
    }



    // function highlight() {
    //     if (list.children.length > 0) {
    //         [...list.children].forEach((li, i) => {
    //             li.classList.toggle("active", i === activeIndex);
    //         });
    //     }
    // }

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

            if (dataObj?.result?.data?.[0].error) {
                showToast(`Error: ${dataObj?.result?.data?.[0].error}`);
                console.log(`Error: ${list[0].error}`);
                return;

            }

            // if (list[0].error) {
            //     showToast(`Error: ${list[0].error}`); 
            //     console.log(`Error: ${list[0].error}`); 
            //     return; 
            // }

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
                window.openDeveloperStudio("open", "AxDBScript.aspx", true);
            }







        }
    }

    function isSuggestionVisible() {
        return list && list.style.display === "block" && items.length > 0;
    }

    function hasActiveSuggestion() {
        return activeIndex >= 0 && activeIndex < items.length;
    }

    function canRunCommand() {
        const text = input.value.trim();
        if (!text || !commands) return false;

        const tokens = getTokens(text);
        const groupKey = cleanString(tokens[0]);
        const groupConfig = commands[groupKey];
        if (!groupConfig) return false;

        const targetIndex = tokens.length - 1;
        const promptInfo = getActivePromptInfo(groupConfig, tokens, targetIndex);


        return !promptInfo;
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
        })

        input.addEventListener("input", handleInput);
        input.addEventListener("blur", () => setTimeout(() => { if (!input.value) hintDiv.textContent = ""; }, 200));
        input.addEventListener("keydown", e => {
            console.log("Keys: " + e.key + "Code: " + e.code + "Alt: " + e.altKey);



            if (e.ctrlKey && e.code === "Space") {
                e.preventDefault();
                handleInput();
                return;
            }



            if (e.key === "Enter") {
                e.preventDefault();


                // if (list.style.display === "block" && active >= 0) {
                //     e.preventDefault(); 
                //     apply(activeIndex); 
                //     return; 
                // }
                //     if (list.style.display === "none") {
                //         e.preventDefault(); 
                //         handleInput(); 
                //         return; 
                //     }

                // const isListOpen = list.style.display === "block"; 
                // const hasActiveItem = isListOpen && activeIndex >= 0; 

                // if (hasActiveItem) {
                //     e.preventDefault(); 
                //     apply(activeIndex); 
                //     return; 
                // }

                // executeCommandsV2(); 
                // hide(); 
                // return; 

                if (isSuggestionVisible() && hasActiveSuggestion()) {
                    e.preventDefault();
                    apply(activeIndex);
                    return;
                }



                executeCommandsV2();
                hide();
                return;


                // e.preventDefault(); 
                // handleInput();

            }
            // // Auto Double quotes 
            // // --------------------------------------------------------

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

    function executeCommandsV2() {
        const text = input.value.trim();
        if (!text || !commands) return;

        const tokens = getTokens(text);
        if (tokens.length === 0) return; // Need at least the command group

        const groupKey = cleanString(tokens[0]);
        const groupConfig = commands[groupKey];

        if (!groupConfig) {
            console.warn(`Unknown command group: ${groupKey}`);
            return;
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
        hide(); // Close suggestions after running
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

    function dispatchCommand(ctx) {
        const { group, config, tokens } = ctx;



        const firstParamPrompt = config.prompts.find(p => p.wordPos === 2);
        const firstParamValue = cleanString(tokens[1]);

        let handlerKey = 'default';

        if (firstParamPrompt && firstParamPrompt.promptValues) {

            if (firstParamValue) {
                handlerKey = firstParamValue.toLowerCase();
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

    function handleCreateAds({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        const transId = "b_sql";
        let fieldname = "sqlname";

        let rawName = cleanCommandToken(tokens[2]);


        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        setEditSessionState(transId);



        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldname}=${paramName}`;
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
        let transId = "a__cd";
        let fieldname = "cardname";
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }


        setEditSessionState(transId);
        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            // targetUrl += `&cardname=${paramName}`;
            targetUrl += `&${fieldname}=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }






    }

    function handleCreateUser({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let transId = "axusr";
        let fieldname = "pusername";
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        setEditSessionState(transId);



        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldname}=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=axusr"); 
    }

    function handleCreateUserGroup({ tokens, commandConfig }) {

        let targetUrl;
        let paramName;
        let transId = "a__ug";
        let fieldname = "users_group_name";
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }


        setEditSessionState(transId);
        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldname}=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }
        // window.LoadIframe("../aspx/tstruct.aspx?transid=a__ug");

    }

    /**
     * ======================== END ==================================
     * 
     */

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

    function handleViewUsergroup({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let transId = "a__ug";
        let fieldname = "users_group_name";

        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        // if (rawName) {
        //     paramName = tryResolveToken(2, rawName, commandConfig, false);

        // }

        //  var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;



        if (!rawName) {
            targetUrl = `../aspx/Entity.aspx?tstid=${transId}`;
            window.LoadIframe(targetUrl);

        } else {
            targetUrl = `../aspx/EntityForm.aspx?tstid=${transId}`;
            targetUrl += `&${fieldname}=${rawName}`;

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
        // window.LoadIframe("AxDBScript.aspx");

    }

    function handleViewInbox() {
        // LoadIframe('processflow.aspx?activelist=t')
        window.LoadIframe('../aspx/processflow.aspx?activelist=t');

    }

    function handleViewDimension({ tokens, commandConfig }) {
        // LoadIframe('processflow.aspx?activelist=t')
        let targetUrl;
        let paramName;
        let rawName = cleanCommandToken(tokens[2]);
        // let rawFieldName = cleanCommandToken(tokens[3]);
        // let rawFieldValue = cleanCommandToken(tokens[4]);



        //   if (!rawName) return;
        // if (rawName) {
        //     paramName = tryResolveToken(2, rawName, commandConfig, false);

        // }

        //  var url = `../aspx/EntityForm.aspx?tstid=${transId}&recid=${recordId}`;



        if (!rawName) {
            targetUrl = "../aspx/Entity.aspx?tstid=a__ag";
            window.LoadIframe(targetUrl);

        } else {
            targetUrl = "../aspx/EntityForm.aspx?tstid=a__ag";
            targetUrl += `&grpname=${rawName}`;

            window.LoadIframe(targetUrl);

        }








    }



    function handleViewData({ tokens, commandConfig }) {
        let targetUrl;

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





        if (!searchField && !searchValue) {
            // targetUrl += "&dummyload=false♠"
            targetUrl = `../aspx/Entity.aspx?tstid=${transid}`;
            window.LoadIframe(targetUrl);
        } else {
            targetUrl = `../aspx/EntityForm.aspx?tstid=${transid}`;
            targetUrl += `&${searchField}=${searchValue}`;
            // targetUrl += "&act=open";
            // targetUrl += "&dummyload=false♠"

            window.LoadIframe(targetUrl);

        }




        // LoadIframe('Entity.aspx?tstid=mrplo')






    }



    /* ============= End =================== */
    function handleCreatePage({ tokens, commandConfig }) {

        let targetUrl;
        let paramName;
        let transId = "sect";
        let fieldname = "caption";
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        setEditSessionState(transId);

        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldname}=${paramName}`;
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
        let transId = "ad_ur";
        let fieldname = "axusergroup";
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        setEditSessionState(transId);

        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldname}=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }



        // window.LoadIframe("../aspx/tstruct.aspx?transid=ad_ur");

    }

    function handleCreateActor({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let transId = "ad_am";
        let fieldName = "actorname";
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        setEditSessionState(transId);

        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            targetUrl += `&${fieldName}=${paramName}`;
            targetUrl += "&act=open";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }


        // window.LoadIframe("../aspx/tstruct.aspx?transid=ad_am");

    }

    function handleCreateDimension({ tokens, commandConfig }) {
        let targetUrl;
        let paramName;
        let transId = "a__ag";

        let rawField = cleanCommandToken(tokens[2]);
        let rawFieldValue = cleanCommandToken(tokens[3]);
        const fieldname = tryResolveToken(2, rawField, commandConfig, false);

        setEditSessionState(transId)
        redirectToTstruct(transId, true, fieldname, rawFieldValue);


        // LoadIframeac(&quot;ivtoivload.aspx?ivname=ad___upg&quot;)

        // targetUrl = "../aspx/tstruct.aspx?transid=a__ag";

        // if (!rawFieldValue && !rawField) {
        //     window.LoadIframe(targetUrl);




        // } else {


        //     targetUrl += `&${rawField}=${rawFieldValue}`;
        //     targetUrl += "&act=open";
        //     targetUrl += "&dummyload=false♠"
        //     window.LoadIframe(targetUrl);

        // }


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


        // if (resolvedName === rawName) {
        //     const listKey =
        //         type === "tstruct"
        //             ? "Axi_TStructList".toLowerCase()
        //             : type === "iview"
        //                 ? "Axi_IViewList".toLowerCase()
        //                 : null;

        //     if (!listKey) {
        //         alert("Unknown source type: " + type);
        //         return;
        //     }

        //     const list = axDatasourceObj[listKey];
        //     const found = list?.find(
        //         x => x.caption?.toLowerCase() === rawName.toLowerCase()
        //     );

        //     if (!found || !found.name) {
        //         console.error(`Source not found: ${rawName}`);
        //         return;
        //     }

        //     resolvedName = found.name;
        // }


        if (type === "tstruct") {
            window.openDeveloperStudio("tstreact", resolvedName, true);
        } else if (type === "iview") {
            window.openDeveloperStudio("ivreact", resolvedName, true);
        } else {
            alert("Unknown source type: " + type);
        }
    }


    // function handleEditData({ tokens, commandConfig, resolvedParams }) {

    //     if (tokens.length < 3) {
    //         console.warn("edit data requires <tstruct> <field> <value>");
    //         // alert("edit data requires <tstruct> <field> <value>");
    //         showToast("edit data requires <tstruct> <field> <value>");
    //         return;
    //     }


    //     let rawStruct = cleanCommandToken(tokens[1]);
    //     let transId = tryResolveToken(1, rawStruct, commandConfig, false);

    //     const extraSourceKey = `axi_fieldlist_${transId}`.toLowerCase();

    //     const extraList = axDatasourceObj[extraSourceKey];

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


    //     // let rawField = cleanCommandToken(tokens[2]);
    //     // const fieldName = tryResolveToken(2, rawField, commandConfig, true);

    //     // if (!fieldName) {
    //     //     console.error("Field resolution failed:", rawField);
    //     //     return;
    //     // }

    //     let fieldName = "";
    //     if (extraList && extraList.length > 0) {
    //         fieldName = extraList[0].displaydata || extraList[0].name || extraList[0].fname;
    //     } else {
    //         console.warn("Hidden field name not found in cache");
    //     }


    //     let rawValue = cleanCommandToken(tokens[2]);
    //     const fieldValue = tryResolveToken(2, rawValue, commandConfig, true);

    //     if (fieldValue == null) {
    //         console.error("Field value resolution failed:", rawValue);
    //         return;
    //     }

    //     console.log(
    //         `Edit Data → TStruct=${transId}, Field=${fieldName}, Value=${fieldValue}`
    //     );

    //     setEditSessionState(transId);

    //     redirectToTstruct(transId, true, fieldName, fieldValue);
    // }


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

        if (tokens.length > 3) {
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

            const extraFieldValueList = axDatasourceObj[`${fieldValuePromptSource}_${transId},${actualFieldName}`.toLowerCase()];
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

            const extraFieldValueList = axDatasourceObj[`${fieldValuePromptSource}_${transId},${fieldName}`.toLowerCase()];
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





        // let rawField = cleanCommandToken(tokens[2]);
        // const fieldName = tryResolveToken(2, rawField, commandConfig, true);

        // if (!fieldName) {
        //     console.error("Field resolution failed:", rawField);
        //     return;
        // }


















    }


    function handleConfigurePermissions({ tokens, commandConfig }) {

        let transId = "a__up"

        let rawUserName = cleanCommandToken(tokens[2]);

        let resolvedUserName = tryResolveToken(2, rawUserName, commandConfig, false);





        // redirectToTstruct(transId, true, "pusername", rawUserName);
        redirectToPermissionScreeen(rawUserName);






    }

    function handleConfigureAccess({ tokens, commandConfig }) {

        let fieldValue = cleanCommandToken(tokens[2]);




        redirectToResponsibilitiesPage(fieldValue);





    }

    function handleEditDimension({ tokens, commandConfig }) {




        let rawName = cleanCommandToken(tokens[2]);
        let transId = "a__ag";
        let fieldName = "grpname";

        let resolvedUserName = tryResolveToken(2, rawName, commandConfig, false);

        //   let _thisappSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        //     let _thisstoredKey = 'originaltrIds-' + _thisappSessUrl;
        //     let _transidArray = JSON.parse(localStorage.getItem(_thisstoredKey) || '[]');

        //     if (_transidArray.includes(transId)) {
        //         _transidArray = _transidArray.filter(x => x.toLowerCase() !== transId.toLowerCase());
        //         localStorage.setItem(_thisstoredKey, JSON.stringify(_transidArray));
        //     }



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

        setEditSessionState(transId);

        // targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        // targetUrl += `&hltype=load`;
        // targetUrl += `&torecid=false`;
        // targetUrl += `&openerIV=${transId}`;
        // targetUrl += `&isIV=false`;
        // targetUrl += `&isDupTab=false`;


        // // if (!paramName) {
        // //     window.LoadIframe(targetUrl);

        // // } else {
        // targetUrl += `&pusername=${rawUserName}`;

        // targetUrl += "&dummyload=false♠"
        // window.LoadIframe(targetUrl);
        redirectToTstruct(transId, true, fieldName, rawName);

        // }



    }

    function handleEditRole({ tokens, commandConfig }) {




        let rawName = cleanCommandToken(tokens[2]);
        let transId = "ad_ur";
        let fieldName = "axusergroup";

        let resolvedUserName = tryResolveToken(2, rawName, commandConfig, false);

        //   let _thisappSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        //     let _thisstoredKey = 'originaltrIds-' + _thisappSessUrl;
        //     let _transidArray = JSON.parse(localStorage.getItem(_thisstoredKey) || '[]');

        //     if (_transidArray.includes(transId)) {
        //         _transidArray = _transidArray.filter(x => x.toLowerCase() !== transId.toLowerCase());
        //         localStorage.setItem(_thisstoredKey, JSON.stringify(_transidArray));
        //     }



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

        setEditSessionState(transId);

        // targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        // targetUrl += `&hltype=load`;
        // targetUrl += `&torecid=false`;
        // targetUrl += `&openerIV=${transId}`;
        // targetUrl += `&isIV=false`;
        // targetUrl += `&isDupTab=false`;


        // // if (!paramName) {
        // //     window.LoadIframe(targetUrl);

        // // } else {
        // targetUrl += `&pusername=${rawUserName}`;

        // targetUrl += "&dummyload=false♠"
        // window.LoadIframe(targetUrl);
        redirectToTstruct(transId, true, fieldName, rawName);

        // }



    }

    function handleEditUsergroup({ tokens, commandConfig }) {




        let rawName = cleanCommandToken(tokens[2]);
        let transId = "a__ug";
        let fieldName = "users_group_name";

        let resolvedUserName = tryResolveToken(2, rawName, commandConfig, false);

        //   let _thisappSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        //     let _thisstoredKey = 'originaltrIds-' + _thisappSessUrl;
        //     let _transidArray = JSON.parse(localStorage.getItem(_thisstoredKey) || '[]');

        //     if (_transidArray.includes(transId)) {
        //         _transidArray = _transidArray.filter(x => x.toLowerCase() !== transId.toLowerCase());
        //         localStorage.setItem(_thisstoredKey, JSON.stringify(_transidArray));
        //     }



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

        setEditSessionState(transId);

        // targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        // targetUrl += `&hltype=load`;
        // targetUrl += `&torecid=false`;
        // targetUrl += `&openerIV=${transId}`;
        // targetUrl += `&isIV=false`;
        // targetUrl += `&isDupTab=false`;


        // // if (!paramName) {
        // //     window.LoadIframe(targetUrl);

        // // } else {
        // targetUrl += `&pusername=${rawUserName}`;

        // targetUrl += "&dummyload=false♠"
        // window.LoadIframe(targetUrl);
        redirectToTstruct(transId, true, fieldName, rawName);

        // }



    }

    function handleEditActor({ tokens, commandConfig }) {




        let rawName = cleanCommandToken(tokens[2]);
        let transId = "ad_am";
        let fieldName = "actorname";

        let resolvedUserName = tryResolveToken(2, rawName, commandConfig, false);

        //   let _thisappSessUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
        //     let _thisstoredKey = 'originaltrIds-' + _thisappSessUrl;
        //     let _transidArray = JSON.parse(localStorage.getItem(_thisstoredKey) || '[]');

        //     if (_transidArray.includes(transId)) {
        //         _transidArray = _transidArray.filter(x => x.toLowerCase() !== transId.toLowerCase());
        //         localStorage.setItem(_thisstoredKey, JSON.stringify(_transidArray));
        //     }



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

        setEditSessionState(transId);

        // targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        // targetUrl += `&hltype=load`;
        // targetUrl += `&torecid=false`;
        // targetUrl += `&openerIV=${transId}`;
        // targetUrl += `&isIV=false`;
        // targetUrl += `&isDupTab=false`;


        // // if (!paramName) {
        // //     window.LoadIframe(targetUrl);

        // // } else {
        // targetUrl += `&pusername=${rawUserName}`;

        // targetUrl += "&dummyload=false♠"
        // window.LoadIframe(targetUrl);
        redirectToTstruct(transId, true, fieldName, rawName);

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
        console.log("commandConfig: " + JSON.stringify(commandConfig));
        let fieldname = "ExecAPIDefName";
        let transId = "apidg";
        let param1Position = commandConfig.prompts[1].wordPos - 1;
        // let rawApiName = cleanCommandToken(tokens[2]);
        let rawApiName = cleanCommandToken(tokens[param1Position]);

        // let targetUrl = "../aspx/tstruct.aspx?transid=apidg";

        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, rawApiName);

        // if (!rawApiName) {
        //     window.LoadIframe(targetUrl);
        // } else {

        //     targetUrl += `&hltype=load`;
        //     targetUrl += `&torecid=false`;
        //     targetUrl += `&openerIV=apidg`;
        //     targetUrl += `&isIV=false`;
        //     targetUrl += `&isDupTab=false`;



        //     targetUrl += `&ExecAPIDefName=${rawApiName}`;

        //     targetUrl += "&dummyload=false♠"


        //     window.LoadIframe(targetUrl);

        // }



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



        // let rawFieldname = cleanCommandToken(tokens[2]);
        // const fieldname = tryResolveToken(2, rawFieldname, commandConfig, false);
        let rawParamValue = cleanCommandToken(tokens[2]);
        const fieldValue = tryResolveToken(2, rawParamValue, commandConfig, false);


        setEditSessionState(transId);
        redirectToTstruct(transId, true, fieldname, fieldValue);



    }

    function handleConfigureDevOptions({ tokens, commandConfig }) {

        window.LoadIframe("../aspx/tstruct.aspx?transid=axstc");

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
        // window.openDeveloperStudio("ImportAll.aspx");



    }

    function handleDownload({ tokens, commandConfig }) {
        let targetUrl = "../aspx/ExportNew.aspx";
        targetUrl += "?action=export"
        window.LoadIframe(targetUrl);
        // window.openDeveloperStudio("ExportNew.aspx");



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

    function extractParams(tokens, commandConfig) {
        return commandConfig.prompts.map(prompt => {
            const tokenIndex = prompt.wordPos - 1;
            return {
                prompt,
                rawValue: cleanCommandToken(tokens[tokenIndex] || "")
            };
        });
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
            // alert("edit data requires <tstruct> <field> <value>");
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
            transId = "b_sql";
            fieldName = "sqlname";
            fieldValue = cleanCommandToken(tokens[1]);


            // handler({ transId, fieldName, fieldValue });
            redirectToSmartView(fieldValue);
            return;


        } else if (type === "page") {



            // const requestUrl = item.requestUrl;

            let rawFieldValue = cleanCommandToken(tokens[1]);
            // let fieldValue = tryResolveToken(1, rawFieldValue, commandConfig, false); 
            // const item = viewList.find(v => v.displaydata === rawFieldValue);

            // const fieldValue = item.caption; 



            // handler({ transId, fieldName, fieldValue });
            redirectToHtmlPages(rawFieldValue);
            return;

        }

       
        const extraSourceKey = `${extraDataSource}_${transId}`.toLowerCase();

        // if (transId === rawStruct) {
        //     const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
        //     const found = list?.find(
        //         x => x.caption?.toLowerCase() === rawStruct
        //     );
        //     if (!found || !found.name) {
        //         console.error("TStruct not found:", rawStruct);
        //         return;
        //     }
        //     transId = found.name;
        // }


        // let rawField = cleanCommandToken(tokens[2]);
        // const fieldName = tryResolveToken(2, rawField, commandConfig, true);

        // if (!fieldName) {
        //     console.error("Field resolution failed:", rawField);
        //     return;
        // }

        if (tokens.length > 3) {
            fieldValueIndex = 3; 
           

        } else {
            fieldValueIndex = 2; 
           

        }

       
        

         const extraList = axDatasourceObj[extraSourceKey];

            if (extraList && extraList.length > 0) {
                fieldName = extraList[0].fname ?? extraList[0].keyfield ?? extraList[0].name ?? extraList[0].displaydata ?? null;
                // if (extraList[0].fname) {
                //     fieldName = extraList[0].fname; 
                // } else {
                // fieldName = extraList[0].displaydata || extraList[0].name || extraList[0].fname;


                // }
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

        //const list = axDatasourceObj["Axi_TStructList".toLowerCase()];
        //const found = list?.find(
        //    x => x.caption?.trim().toLowerCase() === tstructName.trim().toLowerCase()
        //);

        //if (!found || !found.name) {
        //    console.error("TStruct not found:", rawStruct);
        //    return;
        //}
        //    transId = found.name;

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
        // const inputLower = text.trim().toLowerCase(); 
        console.log(JSON.stringify(data));

        // const item = data.find(d => d.caption === caption);
        // const item = data.find(d => d.displaydata.includes(caption));

        const normalizedText = text.trim().toLowerCase();

        const item = data.find(d => {
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

    function handleViewAds(tokens) {
        let targetUrl;
        let paramName;
        const transId = "b_sql";
        let fieldname = "sqlname";

        let rawName = cleanCommandToken(tokens[2]);


        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        redirectToEntity(tranId, fieldName)





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

        //if (tokens.length < 4) {
        //    console.warn("edit source requires <type> <name>");
        //    return;
        //}

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


        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }

        

        setEditSessionState(transId);



        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            // window.LoadIframe(targetUrl);
        redirectToIView(iviewName); 


        } else {
            targetUrl += `&${fieldname}=${encodeURIComponent(paramName)}`;
            targetUrl += "&act=load";
            targetUrl += "&dummyload=false♠"
            window.LoadIframe(targetUrl);

        }
        // window.LoadIframe("../aspx/tstruct.aspx?transid=b_sql");
    }

    function handleOpenCard({ tokens, commandConfig }) {
        // LoadIframeac(&quot;ivtoivload.aspx?ivname=axusers
        // window.LoadIframe("ivtoivload.aspx?ivname=axpcards");
        let targetUrl;
        let paramName;
        let transId = "a__cd";
        let fieldname = "cardname";
        let rawName = cleanCommandToken(tokens[2]);

        //   if (!rawName) return;
        if (rawName) {
            paramName = tryResolveToken(2, rawName, commandConfig, false);

        }


        setEditSessionState(transId);
        targetUrl = `../aspx/tstruct.aspx?transid=${transId}`;

        if (!paramName) {
            window.LoadIframe(targetUrl);

        } else {
            // targetUrl += `&cardname=${paramName}`;
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

        //   if (!rawName) return;
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

        // window.openDeveloperStudio("ihplist");
        //  window.LoadIframe("../aspx/tstruct.aspx?transid=sect"); 

    }

    function handleOpenAppVar({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;iaxvars&quot;);
        // window.openDeveloperStudio("iaxvars"); 
        window.LoadIframe("../aspx/tstruct.aspx?transid=axvar");

    }

    function handleOpenDevOptions({ tokens, commandConfig }) {
        // openDeveloperStudio(&quot;idop_list&quot;);
        // window.openDeveloperStudio("idop_list"); 
        window.LoadIframe("../aspx/tstruct.aspx?transid=axstc");

    }

    function handleOpenDbConsole() {
        window.openDeveloperStudio("AxDBScript.aspx");

    }

    /**
     * ======================= End =============================
     */




})();