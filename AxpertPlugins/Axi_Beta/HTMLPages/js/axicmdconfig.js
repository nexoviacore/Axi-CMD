/**
 * ==============================================================================
 * AXI COMMAND CONFIGURATION MANAGER - JAVASCRIPT CONTROLLER (axicmdconfig.js)
 * Handles Dynamic Routes (`axi_command_config`) and Autocomplete Prompts (`axi_command_prompts`)
 * ==============================================================================
 */

(function () {
    "use strict";

    // Application state
    let allRoutes = [];
    let allPrompts = [];
    let activeGroupFilter = "ALL";
    let isEditMode = false;

    function getUrlParam(name) {
        try {
            const urlParams = new URLSearchParams(window.location.search);
            return urlParams.get(name) || "";
        } catch (e) {
            return "";
        }
    }

    function resolveAxpertSession(key) {
        try {
            if (typeof getSessionValue === "function") {
                const v = getSessionValue(key);
                if (v) return v;
            }
        } catch (e) { }
        try {
            if (typeof callParentNew === "function") {
                const v = callParentNew(key);
                if (v) return v;
            }
        } catch (e) { }
        try {
            if (parent && typeof parent.getSessionValue === "function") {
                const v = parent.getSessionValue(key);
                if (v) return v;
            }
        } catch (e) { }
        try {
            if (top && typeof top.getSessionValue === "function") {
                const v = top.getSessionValue(key);
                if (v) return v;
            }
        } catch (e) { }
        return "";
    }

    function getActiveAppName() {
        return getUrlParam("appname") ||
            getUrlParam("proj") ||
            getUrlParam("project") ||
            (typeof window.mainProject !== "undefined" && window.mainProject) ||
            (parent && parent.mainProject) ||
            (top && top.mainProject) ||
            resolveAxpertSession("Project") ||
            resolveAxpertSession("project") ||
            sessionStorage.getItem("project") ||
            sessionStorage.getItem("mainProject") ||
            localStorage.getItem("project") ||
            "";
    }

    function getActiveUserName() {
        return getUrlParam("username") ||
            getUrlParam("user") ||
            (typeof window.mainUserName !== "undefined" && window.mainUserName) ||
            (parent && parent.mainUserName) ||
            (top && top.mainUserName) ||
            resolveAxpertSession("username") ||
            resolveAxpertSession("user") ||
            sessionStorage.getItem("user") ||
            sessionStorage.getItem("mainUserName") ||
            "";
    }

    function getActiveUserRoles() {
        return getUrlParam("roles") ||
            getUrlParam("role") ||
            getUrlParam("userroles") ||
            (typeof window.AxUserRoles !== "undefined" && window.AxUserRoles) ||
            (parent && parent.AxUserRoles) ||
            (top && top.AxUserRoles) ||
            resolveAxpertSession("AxUserRoles") ||
            resolveAxpertSession("Roles") ||
            sessionStorage.getItem("AxUserRoles") ||
            "";
    }

    // Dynamic Context
    const mainUserName = getActiveUserName();
    const mainProject = getActiveAppName();
    const AxUserRoles = getActiveUserRoles();

    // DOM Ready Initialization
    document.addEventListener("DOMContentLoaded", () => {
        initSessionBadges();
        setupFilterChips();
        setupSearch();
        setupLiveSimulation();
        loadAllData();
    });

    function initSessionBadges() {
        const appEl = document.getElementById("appNameText");
        const userEl = document.getElementById("userText");
        if (appEl) appEl.innerText = mainProject || "(Not detected)";
        if (userEl) userEl.innerText = mainUserName || "(Not detected)";
    }

    let AxiArmUrl = "";

    async function resolveAxiArmUrl() {
        if (AxiArmUrl) return AxiArmUrl;

        let parentArmUrl = "";

        // 1. Try callParentNew
        try {
            if (typeof callParentNew === "function") {
                parentArmUrl = callParentNew("armUrl") || "";
            }
        } catch (e) { }

        // 2. Try parent window
        try {
            if (!parentArmUrl && typeof parent !== "undefined" && parent) {
                parentArmUrl = parent.armUrl || "";
            }
        } catch (e) { }

        // 3. Try top window
        try {
            if (!parentArmUrl && typeof top !== "undefined" && top) {
                parentArmUrl = top.armUrl || "";
            }
        } catch (e) { }

        // 4. Try local variable scope fallback
        try {
            if (!parentArmUrl && typeof armUrl !== "undefined") {
                parentArmUrl = armUrl || "";
            }
        } catch (e) { }

        // 5. Try config file fallback
        if (!parentArmUrl) {
            try {
                const configUrl = `../../axicmd-config.json`;
                const res = await fetch(configUrl);
                if (res.ok) {
                    const config = await res.json();
                    parentArmUrl = config.axiarmurl || config.armUrl || config.axiArmUrl || "";
                }
            } catch (err) { }
        }

        AxiArmUrl = (parentArmUrl || window.location.origin).replace(/\/+$/, "");
        return AxiArmUrl;
    }

    async function getAxiApiUrl(endpoint) {
        const armBase = await resolveAxiArmUrl();
        const prefix = armBase.includes("/AxiApi_Beta") ? "" : "/AxiApi_Beta";
        const cleanEndpoint = endpoint.replace(/^\/+/, "");
        return `${armBase}${prefix}/api/v1/Axi/${cleanEndpoint}`;
    }

    // =========================================================================
    // DATA FETCHING (API INTEGRATION)
    // =========================================================================
    async function loadAllData(force = false) {
        const tbody = document.getElementById("routesTableBody");
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="8" class="text-center text-muted py-4"><i class="fa-solid fa-spinner fa-spin me-2"></i> Loading configurations from database...</td></tr>`;
        }

        try {
            const routesUrl = await getAxiApiUrl(`command-config/all?appname=${encodeURIComponent(mainProject)}`);
            const promptsUrl = await getAxiApiUrl(`command-prompts/all?appname=${encodeURIComponent(mainProject)}`);

            // 1. Fetch Dynamic Routes
            const routesRes = await fetch(routesUrl);
            if (routesRes.ok) {
                allRoutes = await routesRes.json();
            } else {
                allRoutes = getMockRoutes();
            }

            // 2. Fetch Autocomplete Prompts
            const promptsRes = await fetch(promptsUrl);
            if (promptsRes.ok) {
                allPrompts = await promptsRes.json();
            } else {
                allPrompts = getMockPrompts();
            }

            renderRoutesTable();
            renderPromptCommandDropdown();
        } catch (err) {
            console.error("Error loading command configurations:", err);
            allRoutes = getMockRoutes();
            allPrompts = getMockPrompts();
            renderRoutesTable();
            renderPromptCommandDropdown();
        }
    }

    // =========================================================================
    // TAB 1: ROUTES TABLE RENDERING & FILTERING
    // =========================================================================
    function renderRoutesTable() {
        const tbody = document.getElementById("routesTableBody");
        if (!tbody) return;

        const searchVal = (document.getElementById("routeSearchInput")?.value || "").toLowerCase().trim();

        let filtered = allRoutes.filter(r => {
            const matchesGroup = (activeGroupFilter === "ALL") || (r.command.toLowerCase() === activeGroupFilter.toLowerCase());
            const matchesSearch = !searchVal ||
                (r.configId || "").toLowerCase().includes(searchVal) ||
                (r.command || "").toLowerCase().includes(searchVal) ||
                (r.promptOptions || "").toLowerCase().includes(searchVal) ||
                (r.promptId || "").toLowerCase().includes(searchVal) ||
                (r.targetUrl || "").toLowerCase().includes(searchVal);
            return matchesGroup && matchesSearch;
        });

        const countBadge = document.getElementById("routeCountBadge");
        if (countBadge) countBadge.innerText = allRoutes.length;

        if (filtered.length === 0) {
            tbody.innerHTML = `<tr><td colspan="8" class="text-center text-muted py-4"><i class="fa-solid fa-inbox me-2"></i> No matching dynamic routes found.</td></tr>`;
            return;
        }

        tbody.innerHTML = filtered.map(r => {
            const isActive = (r.active || "T") === "T";
            const typeBadgeClass = getTypeBadgeClass(r.promptOptionType);
            const displayUrl = r.targetUrl || getResolvedDefaultUrl(r);

            return `
                <tr>
                    <td>
                        <span class="badge ${isActive ? 'bg-success' : 'bg-secondary'} rounded-pill" style="font-size: 11px;">
                            ${isActive ? 'Active' : 'Disabled'}
                        </span>
                    </td>
                    <td><span class="fw-bold">${escapeHtml(r.command)}</span></td>
                    <td><code>${escapeHtml(r.promptOptions)}</code></td>
                    <td><span class="text-primary fw-bold">${escapeHtml(r.promptId || '-')}</span></td>
                    <td><span class="badge-type ${typeBadgeClass}">${escapeHtml(r.promptOptionType || 'url')}</span></td>
                    <td><span class="text-muted small">${escapeHtml(r.paramField || '-')}</span></td>
                    <td>
                        <div class="text-truncate" style="max-width: 320px;" title="${escapeHtml(displayUrl)}">
                            <span class="small font-monospace text-dark">${escapeHtml(displayUrl)}</span>
                        </div>
                        ${r.extraParams ? `<div class="small text-muted font-monospace">+ ${escapeHtml(r.extraParams)}</div>` : ''}
                    </td>
                    <td class="text-center">
                        <div class="d-flex justify-content-center gap-1">
                            <button class="btn-action" title="Edit Route" onclick="window.AxiConfigManager.openEditRouteModal('${escapeHtml(r.configId)}')">
                                <i class="fa-solid fa-pen-to-square"></i>
                            </button>
                            <button class="btn-action btn-test-go" title="Test in Main Shell (Go)" onclick="window.AxiConfigManager.executeTestRoute('${escapeHtml(r.configId)}', false)">
                                <i class="fa-solid fa-play"></i>
                            </button>
                            <button class="btn-action btn-test-pop" title="Test in Popup Tab (Pop)" onclick="window.AxiConfigManager.executeTestRoute('${escapeHtml(r.configId)}', true)">
                                <i class="fa-solid fa-arrow-up-right-from-square"></i>
                            </button>
                            <button class="btn-action btn-delete" title="Delete Route" onclick="window.AxiConfigManager.deleteRoute('${escapeHtml(r.configId)}')">
                                <i class="fa-solid fa-trash"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            `;
        }).join("");
    }

    function getTypeBadgeClass(type) {
        switch ((type || "").toLowerCase()) {
            case "tstruct": return "badge-tstruct";
            case "iview": return "badge-iview";
            case "url": return "badge-url";
            case "processflow": return "badge-processflow";
            case "tstruct/iview":
            case "iview/tstruct": return "badge-dual";
            default: return "badge-url";
        }
    }

    function getResolvedDefaultUrl(r) {
        const type = (r.promptOptionType || "").toLowerCase();
        const id = r.promptId || "";
        if (type === "tstruct") return `../aspx/tstruct.aspx?transid=${id}`;
        if (type === "iview") return `../aspx/iview.aspx?ivname=${id}`;
        if (type === "processflow") return `../aspx/processflow.aspx?processname=${id}`;
        return `../aspx/${id}`;
    }

    function setupFilterChips() {
        document.querySelectorAll("#groupFilterChips .filter-chip").forEach(chip => {
            chip.addEventListener("click", () => {
                document.querySelectorAll("#groupFilterChips .filter-chip").forEach(c => c.classList.remove("active"));
                chip.classList.add("active");
                activeGroupFilter = chip.getAttribute("data-group");
                renderRoutesTable();
            });
        });
    }

    function setupSearch() {
        const search = document.getElementById("routeSearchInput");
        if (search) {
            search.addEventListener("input", () => {
                renderRoutesTable();
            });
        }
    }

    // =========================================================================
    // MODAL FORM & SIMULATION LOGIC
    // =========================================================================
    function openAddRouteModal() {
        isEditMode = false;
        document.getElementById("routeModalTitle").innerHTML = `<i class="fa-solid fa-plus-circle text-primary me-2"></i> Add Dynamic Route`;
        document.getElementById("modalConfigId").value = "";
        document.getElementById("modalConfigId").readOnly = false;
        document.getElementById("modalCommand").value = "Configure";
        document.getElementById("modalPromptOptions").value = "";
        document.getElementById("modalPromptOptionType").value = "url";
        document.getElementById("modalPromptId").value = "";
        document.getElementById("modalParamField").value = "";
        document.getElementById("modalTargetUrl").value = "";
        document.getElementById("modalExtraParams").value = "role=:userroles&user=:username";
        document.getElementById("modalActive").checked = true;
        document.getElementById("modalSmartLink").checked = true;
        updateLiveSimulation();
        new bootstrap.Modal(document.getElementById("routeModal")).show();
    }

    function openEditRouteModal(configId) {
        const route = allRoutes.find(r => r.configId.toLowerCase() === configId.toLowerCase());
        if (!route) return;

        isEditMode = true;
        document.getElementById("routeModalTitle").innerHTML = `<i class="fa-solid fa-pen-to-square text-primary me-2"></i> Edit Dynamic Route`;
        document.getElementById("modalConfigId").value = route.configId;
        document.getElementById("modalConfigId").readOnly = true;
        document.getElementById("modalCommand").value = route.command;
        document.getElementById("modalPromptOptions").value = route.promptOptions;
        document.getElementById("modalPromptOptionType").value = route.promptOptionType || "url";
        document.getElementById("modalPromptId").value = route.promptId || "";
        document.getElementById("modalParamField").value = route.paramField || "";
        document.getElementById("modalTargetUrl").value = route.targetUrl || "";
        document.getElementById("modalExtraParams").value = route.extraParams || "";
        document.getElementById("modalActive").checked = (route.active || "T") === "T";
        document.getElementById("modalSmartLink").checked = false;
        updateLiveSimulation();
        new bootstrap.Modal(document.getElementById("routeModal")).show();
    }

    function setupLiveSimulation() {
        ["modalCommand", "modalPromptOptions", "modalPromptOptionType", "modalPromptId", "modalParamField", "modalTargetUrl", "modalExtraParams"].forEach(id => {
            const el = document.getElementById(id);
            if (el) {
                el.addEventListener("input", updateLiveSimulation);
                el.addEventListener("change", updateLiveSimulation);
            }
        });
    }

    function updateLiveSimulation() {
        const command = document.getElementById("modalCommand")?.value || "";
        const promptOpt = document.getElementById("modalPromptOptions")?.value || "";
        const type = document.getElementById("modalPromptOptionType")?.value || "url";
        const promptId = document.getElementById("modalPromptId")?.value || "";
        const paramField = document.getElementById("modalParamField")?.value || "";
        const targetUrl = document.getElementById("modalTargetUrl")?.value || "";
        const extraParams = document.getElementById("modalExtraParams")?.value || "";

        // Auto-generate config_id if adding
        if (!isEditMode && promptOpt) {
            const cleanOpt = promptOpt.toLowerCase().replace(/[^a-z0-9]/g, "_");
            document.getElementById("modalConfigId").value = `cfg_${command.toLowerCase()}_${cleanOpt}`;
        }

        let computed = targetUrl;
        if (!computed) {
            if (type === "tstruct") computed = `../aspx/tstruct.aspx?transid=${promptId}&hltype=open`;
            else if (type === "iview") computed = `../aspx/iview.aspx?ivname=${promptId}`;
            else if (type === "processflow") computed = `../aspx/processflow.aspx?processname=${promptId}`;
            else computed = `../aspx/${promptId}`;
        }

        let full = computed;
        if (paramField) {
            full += (full.includes("?") ? "&" : "?") + `${paramField}=SampleValue`;
        }
        if (extraParams) {
            let resolvedExtra = extraParams
                .replace(/:username/g, mainUserName)
                .replace(/:userroles/g, AxUserRoles.split(",")[0])
                .replace(/:appname/g, mainProject)
                .replace(/:userresp/g, "DefaultResp")
                .replace(/:param/g, "SampleValue");
            full += (full.includes("?") ? "&" : "?") + resolvedExtra;
        }

        const simEl = document.getElementById("modalSimulatedUrl");
        if (simEl) simEl.innerText = full;
    }

    function insertPlaceholder(token) {
        const field = document.getElementById("modalExtraParams");
        if (!field) return;
        field.value = field.value ? `${field.value}&${token.replace(':', '')}=${token}` : `${token.replace(':', '')}=${token}`;
        updateLiveSimulation();
    }

    async function saveRouteFromModal() {
        const configId = document.getElementById("modalConfigId").value.trim();
        const command = document.getElementById("modalCommand").value.trim();
        const promptOptions = document.getElementById("modalPromptOptions").value.trim();
        const promptOptionType = document.getElementById("modalPromptOptionType").value;
        const promptId = document.getElementById("modalPromptId").value.trim();
        const paramField = document.getElementById("modalParamField").value.trim() || null;
        const targetUrl = document.getElementById("modalTargetUrl").value.trim() || null;
        const extraParams = document.getElementById("modalExtraParams").value.trim() || null;
        const active = document.getElementById("modalActive").checked ? "T" : "F";
        const smartLink = document.getElementById("modalSmartLink").checked;

        if (!configId || !command || !promptOptions || !promptId) {
            alert("Please fill out all required fields marked with *.");
            return;
        }

        const payload = {
            configId,
            command,
            promptOptions,
            promptId,
            promptOptionType,
            paramField,
            targetUrl,
            extraParams,
            active
        };

        try {
            const saveUrl = await getAxiApiUrl(`command-config/save?appname=${encodeURIComponent(mainProject)}`);
            await fetch(saveUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (smartLink && !isEditMode) {
                await autoLinkPromptToken(command, promptOptions);
            }

            invalidateAndRefreshPalette();
            bootstrap.Modal.getInstance(document.getElementById("routeModal")).hide();
            await loadAllData(true);
        } catch (err) {
            console.error("Save error:", err);
            const existingIdx = allRoutes.findIndex(r => r.configId.toLowerCase() === configId.toLowerCase());
            if (existingIdx >= 0) allRoutes[existingIdx] = payload;
            else allRoutes.unshift(payload);
            invalidateAndRefreshPalette();
            bootstrap.Modal.getInstance(document.getElementById("routeModal")).hide();
            renderRoutesTable();
        }
    }

    async function deleteRoute(configId) {
        if (!confirm(`Are you sure you want to delete route '${configId}'?`)) return;

        try {
            const delUrl = await getAxiApiUrl(`command-config/delete?configId=${encodeURIComponent(configId)}&appname=${encodeURIComponent(mainProject)}`);
            await fetch(delUrl, {
                method: "POST"
            });
            invalidateAndRefreshPalette();
            await loadAllData(true);
        } catch (err) {
            allRoutes = allRoutes.filter(r => r.configId.toLowerCase() !== configId.toLowerCase());
            invalidateAndRefreshPalette();
            renderRoutesTable();
        }
    }

    // =========================================================================
    // TAB 2: PROMPTS MANAGEMENT & SMART-LINK
    // =========================================================================
    function renderPromptCommandDropdown() {
        const select = document.getElementById("promptCommandSelect");
        if (!select) return;

        const commands = [...new Set(allPrompts.map(p => p.command || `Token ${p.cmdToken}`))].filter(Boolean);
        const countBadge = document.getElementById("promptCountBadge");
        if (countBadge) countBadge.innerText = allPrompts.length;

        select.innerHTML = commands.map(c => `<option value="${escapeHtml(c)}">${escapeHtml(c)}</option>`).join("");
        renderSelectedPromptDetails();
    }

    function renderSelectedPromptDetails() {
        const select = document.getElementById("promptCommandSelect");
        if (!select) return;

        const selectedCmd = select.value;
        const promptRow = allPrompts.find(p => (p.command || `Token ${p.cmdToken}`) === selectedCmd && (p.wordPos === 2 || p.promptValues));
        const nameRow = allPrompts.find(p => (p.command || `Token ${p.cmdToken}`) === selectedCmd && (p.wordPos === 3 || p.promptSource));

        const container = document.getElementById("promptDetailsCard");
        if (!container) return;

        if (!promptRow) {
            container.innerHTML = `<div class="text-muted small">No multi-level prompt values registered for this command.</div>`;
            return;
        }

        const values = (promptRow.promptValues || "").split(",").map(v => v.trim()).filter(Boolean);
        const sources = (nameRow && nameRow.promptSource) ? nameRow.promptSource.split(",").map(s => s.trim()).filter(Boolean) : [];
        const isAligned = values.length === sources.length;

        container.innerHTML = `
            <div class="row g-3">
                <div class="col-md-12">
                    <div class="alert ${isAligned ? 'alert-success' : 'alert-warning'} py-2 small d-flex align-items-center justify-content-between">
                        <span>
                            <i class="fa-solid ${isAligned ? 'fa-circle-check text-success' : 'fa-triangle-exclamation text-warning'} me-2"></i>
                            <b>Positional Alignment:</b> Position 2 has <b>${values.length}</b> prompt values; Position 3 has <b>${sources.length}</b> data sources.
                            ${!isAligned ? ' (Warning: count mismatch may cause misalignment!)' : ''}
                        </span>
                        <button class="btn btn-sm btn-primary fw-bold" onclick="window.AxiConfigManager.savePromptTokensFromUI(${promptRow.cmdToken})">
                            <i class="fa-solid fa-floppy-disk me-1"></i> Save Changes
                        </button>
                    </div>
                </div>

                <div class="col-md-6">
                    <label class="form-label small fw-bold">Position 2: Prompt Values (Object Types)</label>
                    <div class="p-2 border bg-white rounded" style="min-height: 120px;">
                        ${values.map((v, idx) => `
                            <span class="token-chip">
                                ${escapeHtml(v)}
                                <i class="fa-solid fa-xmark remove-token" onclick="window.AxiConfigManager.removePromptToken(${promptRow.cmdToken}, ${idx})"></i>
                            </span>
                        `).join("")}
                    </div>
                    <div class="input-group input-group-sm mt-2">
                        <input type="text" class="form-control" id="newPromptValInput" placeholder="Add new prompt value (e.g. SLA)">
                        <button class="btn btn-outline-primary" type="button" onclick="window.AxiConfigManager.addPromptToken(${promptRow.cmdToken})"><i class="fa-solid fa-plus"></i> Add</button>
                    </div>
                </div>

                <div class="col-md-6">
                    <label class="form-label small fw-bold">Position 3: Data Sources (1-to-1 Mapped)</label>
                    <textarea class="form-control form-control-sm font-monospace" id="promptSourcesTextarea" rows="6">${escapeHtml(nameRow?.promptSource || '')}</textarea>
                    <div class="small text-muted mt-1">Comma-separated list of API data sources mapped 1-to-1 to each prompt value. Use <code>Axi_Dummy</code> for static options.</div>
                </div>
            </div>
        `;
    }

    async function autoLinkPromptToken(command, promptOption) {
        const promptRow = allPrompts.find(p => (p.command || "").toLowerCase() === command.toLowerCase() && (p.wordPos === 2 || p.promptValues));
        const nameRow = allPrompts.find(p => (p.command || "").toLowerCase() === command.toLowerCase() && (p.wordPos === 3 || p.promptSource));

        if (promptRow) {
            const values = (promptRow.promptValues || "").split(",").map(v => v.trim()).filter(Boolean);
            if (!values.some(v => v.toLowerCase() === promptOption.toLowerCase())) {
                values.push(promptOption);
                const sources = (nameRow?.promptSource || "").split(",").map(s => s.trim()).filter(Boolean);
                sources.push("Axi_Dummy");

                const payload = {
                    cmdToken: promptRow.cmdToken,
                    wordPos: 2,
                    promptValues: values.join(","),
                    promptSource: sources.join(",")
                };

                const savePromptUrl = await getAxiApiUrl(`command-prompts/save?appname=${encodeURIComponent(mainProject)}`);
                await fetch(savePromptUrl, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });
            }
        }
    }

    function addPromptToken(cmdToken) {
        const input = document.getElementById("newPromptValInput");
        const val = (input?.value || "").trim();
        if (!val) return;

        const promptRow = allPrompts.find(p => p.cmdToken === cmdToken && (p.wordPos === 2 || p.promptValues));
        const nameRow = allPrompts.find(p => p.cmdToken === cmdToken && (p.wordPos === 3 || p.promptSource));

        if (promptRow) {
            const values = (promptRow.promptValues || "").split(",").map(v => v.trim()).filter(Boolean);
            values.push(val);
            promptRow.promptValues = values.join(",");

            if (nameRow) {
                const sources = (nameRow.promptSource || "").split(",").map(s => s.trim()).filter(Boolean);
                sources.push("Axi_Dummy");
                nameRow.promptSource = sources.join(",");
            }
            renderSelectedPromptDetails();
        }
        input.value = "";
    }

    function removePromptToken(cmdToken, index) {
        const promptRow = allPrompts.find(p => p.cmdToken === cmdToken && (p.wordPos === 2 || p.promptValues));
        const nameRow = allPrompts.find(p => p.cmdToken === cmdToken && (p.wordPos === 3 || p.promptSource));

        if (promptRow) {
            const values = (promptRow.promptValues || "").split(",").map(v => v.trim()).filter(Boolean);
            values.splice(index, 1);
            promptRow.promptValues = values.join(",");

            if (nameRow) {
                const sources = (nameRow.promptSource || "").split(",").map(s => s.trim()).filter(Boolean);
                if (sources.length > index) sources.splice(index, 1);
                nameRow.promptSource = sources.join(",");
            }
            renderSelectedPromptDetails();
        }
    }

    async function savePromptTokensFromUI(cmdToken) {
        const promptRow = allPrompts.find(p => p.cmdToken === cmdToken && (p.wordPos === 2 || p.promptValues));
        const nameRow = allPrompts.find(p => p.cmdToken === cmdToken && (p.wordPos === 3 || p.promptSource));
        const sourcesText = document.getElementById("promptSourcesTextarea")?.value || "";

        if (nameRow) nameRow.promptSource = sourcesText;

        const payload = {
            cmdToken: cmdToken,
            wordPos: 2,
            promptValues: promptRow?.promptValues || "",
            promptSource: sourcesText
        };

        try {
            const savePromptUrl = await getAxiApiUrl(`command-prompts/save?appname=${encodeURIComponent(mainProject)}`);
            await fetch(savePromptUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });
            invalidateAndRefreshPalette();
            alert("Prompt tokens saved successfully!");
        } catch (err) {
            invalidateAndRefreshPalette();
            alert("Saved to local session state.");
        }
    }

    // =========================================================================
    // EXECUTION SIMULATION & CACHE INVALIDATION
    // =========================================================================
    function executeTestRoute(configId, isPop = false) {
        const route = allRoutes.find(r => r.configId.toLowerCase() === configId.toLowerCase());
        if (!route) return;

        let targetUrl = route.targetUrl || getResolvedDefaultUrl(route);
        if (route.paramField) {
            targetUrl += (targetUrl.includes("?") ? "&" : "?") + `${route.paramField}=SampleRecord`;
        }
        if (route.extraParams) {
            let resolved = route.extraParams
                .replace(/:username/g, mainUserName)
                .replace(/:userroles/g, AxUserRoles.split(",")[0])
                .replace(/:appname/g, mainProject)
                .replace(/:userresp/g, "DefaultResp")
                .replace(/:param/g, "SampleRecord");
            targetUrl += (targetUrl.includes("?") ? "&" : "?") + resolved;
        }

        if (isPop) {
            targetUrl += (targetUrl.includes("?") ? "&" : "?") + "AxIsPop=true";
            if (top && typeof top.openPopOption === "function") {
                top.openPopOption(targetUrl, `${route.command} ${route.promptOptions}`);
            } else if (parent && typeof parent.openPopOption === "function") {
                parent.openPopOption(targetUrl, `${route.command} ${route.promptOptions}`);
            } else {
                window.open(targetUrl, "_blank");
            }
        } else {
            if (top && typeof top.LoadIframe === "function") {
                top.LoadIframe(targetUrl);
            } else if (parent && typeof parent.LoadIframe === "function") {
                parent.LoadIframe(targetUrl);
            } else {
                window.location.href = targetUrl;
            }
        }
    }

    function invalidateAndRefreshPalette() {
        try {
            localStorage.removeItem("axi_command_config_v2");
            if (top && top.postMessage) {
                top.postMessage({ type: "AXI_REFRESH_CONFIG" }, "*");
            }
            if (top && typeof top.loadAxiCommands === "function") {
                top.loadAxiCommands(true);
            }
        } catch (e) { }
    }

    function escapeHtml(str) {
        if (!str) return "";
        return str.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#039;");
    }

    // Mock data fallbacks for standalone preview
    function getMockRoutes() {
        return [
            { configId: "cfg_configure_user", command: "Configure", promptOptions: "user", promptId: "axusr", promptOptionType: "tstruct", paramField: "uname", targetUrl: null, extraParams: null, active: "T" },
            { configId: "cfg_configure_users", command: "Configure", promptOptions: "user listing", promptId: "axusers", promptOptionType: "iview", paramField: null, targetUrl: null, extraParams: null, active: "T" },
            { configId: "cfg_configure_role", command: "Configure", promptOptions: "role", promptId: "ad_ur/ad___url", promptOptionType: "tstruct/iview", paramField: "rname", targetUrl: null, extraParams: null, active: "T" },
            { configId: "cfg_configure_settings", command: "Configure", promptOptions: "settings", promptId: "configuration.aspx", promptOptionType: "url", paramField: null, targetUrl: "../aspx/configuration.aspx", extraParams: null, active: "T" },
            { configId: "cfg_configure_axi_cmd", command: "Configure", promptOptions: "axi_cmd", promptId: "AxiCMDConfig.html", promptOptionType: "url", paramField: null, targetUrl: "../AxpertPlugins/Axi_Beta/HTMLPages/AxiCMDConfig.html", extraParams: null, active: "T" },
            { configId: "cfg_sdk_ads", command: "SDK", promptOptions: "axpert data sources", promptId: "b_sql", promptOptionType: "tstruct", paramField: "sqlname", targetUrl: null, extraParams: "act=load&dummyload=false?", active: "T" }
        ];
    }

    function getMockPrompts() {
        return [
            { cmdToken: 4, command: "Configure", commandGroup: "Configure", wordPos: 2, prompt: "object type", promptValues: "PEG,Form Notification,Scheduled Notification,Peg Form Notification,Rule,KeyField,User,User Listing,User Permission Setup,User Permissions,User Activation,User Group,Role,Role Listing,Role Permissions,Actor,Actor Listing,Publish Axpert API,Publish Config Studio,Card,Responsibility,Responsibility Listing,Dimension,Dimension Listing, Application Properties,Settings,Smart View Attributes,Smart View Listing,Axi_CMD" },
            { cmdToken: 4, command: "Configure", commandGroup: "Configure", wordPos: 3, prompt: "object name", promptSource: "Axi_PegList,Axi_FormNotifyList,Axi_ScheduleNotifyList,Axi_PEGNotifyList,Axi_RuleNamesList,axi_structmetalist,Axi_Dummy,Axi_Dummy,axi_userlist,axi_userlist,axi_useractivation,axi_usergrouplist,Axi_Dummy,Axi_Dummy,axi_rolelist,axi_actorlist,Axi_Dummy,axi_publishapi,Axi_ServernameList,axi_cardlist,axi_resposibilitylist,Axi_Dummy,axi_dimensionlist,Axi_Dummy,Axi_Dummy,Axi_Dummy,axi_smartviewlist,Axi_Dummy,Axi_Dummy" },
            { cmdToken: 7, command: "SDK", commandGroup: "SDK", wordPos: 2, prompt: "type", promptValues: "TStruct,IView,Axpert Data Sources,Page,Arrange Menu,Dev Option,App Variables,Db Explorer,API Plugin,Axpert Job,Language,Publish,Custom Data Type,Email Definition,Table Field Descriptor,Custom Plugin,Queue Listing,Out Bound Queue,In Bound Queue,Mem DB Console" }
        ];
    }

    // Expose public methods to window
    window.AxiConfigManager = {
        loadAllData,
        openAddRouteModal,
        openEditRouteModal,
        saveRouteFromModal,
        deleteRoute,
        renderSelectedPromptDetails,
        addPromptToken,
        removePromptToken,
        savePromptTokensFromUI,
        executeTestRoute,
        insertPlaceholder
    };

})();
