/**
 * ==============================================================================
 * AXI COMMAND CONFIGURATION MANAGER - JAVASCRIPT CONTROLLER (axicmdconfig.js)
 * Handles Dynamic Routes (`axi_command_config`) and Autocomplete Prompts (`axi_command_prompts`)
 * 100% Real API Integration - Zero Mocks - Server-Side & Client Pagination
 * ==============================================================================
 */

(function () {
    "use strict";

    // Application state
    let allRoutes = [];
    let allPrompts = [];
    let activeGroupFilter = "ALL";
    let isEditMode = false;

    // Pagination state
    let currentPage = 1;
    let pageSize = 10;
    let totalCount = 0;
    let totalPages = 1;

    let searchDebounceTimer = null;

    function getUrlParam(name) {
        try {
            const urlParams = new URLSearchParams(window.location.search);
            return urlParams.get(name) || "";
        } catch (e) {
            return "";
        }
    }

    function getCookie(name) {
        try {
            const value = `; ${document.cookie}`;
            const parts = value.split(`; ${name}=`);
            if (parts.length === 2) return parts.pop().split(';').shift();
        } catch (e) { }
        return "";
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
            (parent && parent.window && parent.window.mainProject) ||
            (top && top.window && top.window.mainProject) ||
            resolveAxpertSession("Project") ||
            resolveAxpertSession("project") ||
            resolveAxpertSession("mainProject") ||
            sessionStorage.getItem("project") ||
            sessionStorage.getItem("mainProject") ||
            sessionStorage.getItem("proj") ||
            sessionStorage.getItem("axpertProject") ||
            localStorage.getItem("project") ||
            localStorage.getItem("mainProject") ||
            localStorage.getItem("proj") ||
            getCookie("project") ||
            getCookie("proj") ||
            "Agile";
    }

    function getActiveUserName() {
        return getUrlParam("username") ||
            getUrlParam("user") ||
            (typeof window.mainUserName !== "undefined" && window.mainUserName) ||
            (parent && parent.mainUserName) ||
            (top && top.mainUserName) ||
            (parent && parent.window && parent.window.mainUserName) ||
            (top && top.window && top.window.mainUserName) ||
            resolveAxpertSession("username") ||
            resolveAxpertSession("user") ||
            resolveAxpertSession("mainUserName") ||
            sessionStorage.getItem("user") ||
            sessionStorage.getItem("mainUserName") ||
            sessionStorage.getItem("userName") ||
            localStorage.getItem("user") ||
            localStorage.getItem("mainUserName") ||
            getCookie("username") ||
            getCookie("user") ||
            "admin";
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
            "Developer,Administrator";
    }

    // DOM Ready Initialization
    document.addEventListener("DOMContentLoaded", () => {
        initSessionBadges();
        setupTabs();
        setupFilterChips();
        setupSearch();
        setupLiveSimulation();
        loadAllData();
    });

    function initSessionBadges() {
        const appEl = document.getElementById("appNameText");
        const userEl = document.getElementById("userText");
        if (appEl) appEl.innerText = getActiveAppName() || "(Not detected)";
        if (userEl) userEl.innerText = getActiveUserName() || "(Not detected)";
    }

    function setupTabs() {
        document.querySelectorAll("#configTabs [data-tab-target]").forEach(tabBtn => {
            tabBtn.addEventListener("click", () => {
                document.querySelectorAll("#configTabs .nav-link").forEach(btn => btn.classList.remove("active"));
                document.querySelectorAll(".tab-content .tab-pane").forEach(pane => pane.classList.remove("active"));
                tabBtn.classList.add("active");
                const targetSelector = tabBtn.getAttribute("data-tab-target");
                const targetPane = document.querySelector(targetSelector);
                if (targetPane) targetPane.classList.add("active");

                // Lazy load prompts only when Autocomplete Prompts tab is clicked
                if (targetSelector === "#promptsPane" && allPrompts.length === 0) {
                    loadPromptsData();
                }
            });
        });
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
    // DATA FETCHING (REAL API INTEGRATION WITH PAGINATION)
    // =========================================================================
    async function loadAllData(force = false) {
        initSessionBadges();
        await loadPagedRoutes();

        // Refresh prompts only if the user is currently on the Autocomplete Prompts tab
        const isPromptsTabActive = document.querySelector("#promptsPane")?.classList.contains("active");
        if (isPromptsTabActive) {
            await loadPromptsData();
        }
    }

    async function loadPagedRoutes() {
        const tbody = document.getElementById("routesTableBody");
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="7" class="text-center text-muted py-4"><i class="fa fa-spinner fa-spin me-2"></i> Loading command configurations...</td></tr>`;
        }

        const activeApp = getActiveAppName();
        const searchVal = (document.getElementById("routeSearchInput")?.value || "").trim();
        const commandFilter = activeGroupFilter;

        let routesUrl = "";

        try {
            const queryParams = new URLSearchParams({
                appname: activeApp,
                pageIndex: currentPage.toString(),
                pageSize: pageSize.toString()
            });

            if (searchVal) queryParams.append("search", searchVal);
            if (commandFilter && commandFilter !== "ALL") queryParams.append("command", commandFilter);

            routesUrl = await getAxiApiUrl(`command-config/paged?${queryParams.toString()}`);
            const routesRes = await fetch(routesUrl);

            if (routesRes.ok) {
                const data = await routesRes.json();
                allRoutes = data.items || [];
                totalCount = data.totalCount || 0;
                totalPages = data.totalPages || 1;
                currentPage = data.pageIndex || 1;

                renderRoutesTable();
                renderPagination();
            } else {
                const errText = await routesRes.text();
                tbody.innerHTML = `<tr><td colspan="7" class="text-center text-danger py-4"><i class="fa fa-exclamation-triangle me-2"></i> <b>Failed to load routes (${routesRes.status}):</b> ${escapeHtml(errText || routesRes.statusText)}<br><small class="text-muted font-monospace">${escapeHtml(routesUrl)}</small></td></tr>`;
            }
        } catch (err) {
            console.error("Error loading command configurations:", err);
            if (tbody) {
                tbody.innerHTML = `<tr><td colspan="7" class="text-center text-danger py-4"><i class="fa fa-exclamation-triangle me-2"></i> <b>Network / Server Error:</b> ${escapeHtml(err.message)}<br><small class="text-muted font-monospace">${escapeHtml(routesUrl)}</small></td></tr>`;
            }
        }
    }

    async function loadPromptsData() {
        const activeApp = getActiveAppName();
        let promptsUrl = "";

        try {
            promptsUrl = await getAxiApiUrl(`command-prompts/all?appname=${encodeURIComponent(activeApp)}`);
            const promptsRes = await fetch(promptsUrl);

            if (promptsRes.ok) {
                const promptData = await promptsRes.json();
                allPrompts = Array.isArray(promptData) ? promptData : [];
                renderPromptCommandDropdown();
            } else {
                const promptErrText = await promptsRes.text();
                const container = document.getElementById("promptDetailsCard");
                if (container) {
                    container.innerHTML = `<div class="alert alert-danger"><i class="fa fa-exclamation-triangle me-2"></i> <b>Failed to load prompts (${promptsRes.status}):</b> ${escapeHtml(promptErrText || promptsRes.statusText)}<br><small class="font-monospace">${escapeHtml(promptsUrl)}</small></div>`;
                }
            }
        } catch (err) {
            console.error("Error loading prompts:", err);
            const container = document.getElementById("promptDetailsCard");
            if (container) {
                container.innerHTML = `<div class="alert alert-danger"><i class="fa fa-exclamation-triangle me-2"></i> <b>Network / Server Error:</b> ${escapeHtml(err.message)}<br><small class="font-monospace">${escapeHtml(promptsUrl)}</small></div>`;
            }
        }
    }

    // =========================================================================
    // TAB 1: ROUTES TABLE RENDERING & PAGINATION
    // =========================================================================
    function renderRoutesTable() {
        const tbody = document.getElementById("routesTableBody");
        if (!tbody) return;

        const countBadge = document.getElementById("routeCountBadge");
        if (countBadge) countBadge.innerText = totalCount;

        if (allRoutes.length === 0) {
            tbody.innerHTML = `<tr><td colspan="7" class="text-center text-muted py-4"><i class="fa fa-inbox me-2 text-muted"></i> No dynamic routes found.</td></tr>`;
            return;
        }

        tbody.innerHTML = allRoutes.map(r => {
            const isActive = (r.active || "T") === "T";
            const typeBadgeClass = getTypeBadgeClass(r.promptOptionType);
            const displayUrl = r.targetUrl || getResolvedDefaultUrl(r);

            return `
                <tr>
                    <td>
                        <span class="${isActive ? 'status-pill-active' : 'status-pill-disabled'}">
                            <i class="fa ${isActive ? 'fa-circle' : 'fa-circle-o'}" style="font-size: 7px;"></i>
                            ${isActive ? 'Active' : 'Disabled'}
                        </span>
                    </td>
                    <td><span class="cmd-badge">${escapeHtml(r.command)}</span></td>
                    <td><span class="code-chip">${escapeHtml(r.promptOptions)}</span></td>
                    <td><span class="target-id-chip">${escapeHtml(r.promptId || '-')}</span></td>
                    <td><span class="badge-type ${typeBadgeClass}">${escapeHtml(r.promptOptionType || 'url')}</span></td>
                    <td><span class="text-muted small font-monospace">${escapeHtml(r.paramField || '-')}</span></td>
                    <td>
                        <div class="text-truncate" style="max-width: 420px;" title="${escapeHtml(displayUrl)}">
                            <span class="small font-monospace" style="color: var(--text-heading);">${escapeHtml(displayUrl)}</span>
                        </div>
                        ${r.extraParams ? `<div class="small text-muted font-monospace mt-1">+ ${escapeHtml(r.extraParams)}</div>` : ''}
                    </td>
                </tr>
            `;
        }).join("");
    }

    function renderPagination() {
        const infoEl = document.getElementById("routesPaginationInfo");
        const btnsContainer = document.getElementById("routesPaginationButtons");

        if (!infoEl || !btnsContainer) return;

        if (totalCount === 0) {
            infoEl.innerText = "Showing 0-0 of 0 entries";
            btnsContainer.innerHTML = "";
            return;
        }

        const startItem = pageSize === 0 ? 1 : ((currentPage - 1) * pageSize) + 1;
        const endItem = pageSize === 0 ? totalCount : Math.min(currentPage * pageSize, totalCount);
        infoEl.innerText = `Showing ${startItem}-${endItem} of ${totalCount} entries`;

        if (totalPages <= 1) {
            btnsContainer.innerHTML = "";
            return;
        }

        let html = `
            <button class="pagination-btn" ${currentPage === 1 ? 'disabled' : ''} onclick="window.AxiConfigManager.goToPage(${currentPage - 1})" title="Previous Page">
                <i class="fa fa-chevron-left"></i>
            </button>
        `;

        // Numeric page chips with ellipsis
        const maxVisibleButtons = 5;
        let startPage = Math.max(1, currentPage - Math.floor(maxVisibleButtons / 2));
        let endPage = Math.min(totalPages, startPage + maxVisibleButtons - 1);

        if (endPage - startPage + 1 < maxVisibleButtons) {
            startPage = Math.max(1, endPage - maxVisibleButtons + 1);
        }

        if (startPage > 1) {
            html += `<button class="pagination-btn" onclick="window.AxiConfigManager.goToPage(1)">1</button>`;
            if (startPage > 2) {
                html += `<span class="px-1 text-muted">...</span>`;
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            html += `<button class="pagination-btn ${i === currentPage ? 'active' : ''}" onclick="window.AxiConfigManager.goToPage(${i})">${i}</button>`;
        }

        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                html += `<span class="px-1 text-muted">...</span>`;
            }
            html += `<button class="pagination-btn" onclick="window.AxiConfigManager.goToPage(${totalPages})">${totalPages}</button>`;
        }

        html += `
            <button class="pagination-btn" ${currentPage === totalPages ? 'disabled' : ''} onclick="window.AxiConfigManager.goToPage(${currentPage + 1})" title="Next Page">
                <i class="fa fa-chevron-right"></i>
            </button>
        `;

        btnsContainer.innerHTML = html;
    }

    function goToPage(page) {
        if (page < 1 || page > totalPages || page === currentPage) return;
        currentPage = page;
        loadPagedRoutes();
    }

    function changePageSize(newSize) {
        pageSize = parseInt(newSize, 10);
        currentPage = 1;
        loadPagedRoutes();
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
                currentPage = 1;
                loadPagedRoutes();
            });
        });
    }

    function setupSearch() {
        const search = document.getElementById("routeSearchInput");
        if (search) {
            search.addEventListener("input", () => {
                clearTimeout(searchDebounceTimer);
                searchDebounceTimer = setTimeout(() => {
                    currentPage = 1;
                    loadPagedRoutes();
                }, 300);
            });
        }
    }

    // =========================================================================
    // MODAL FORM & SIMULATION LOGIC (Native Self-Contained)
    // =========================================================================
    function openRouteModal() {
        const backdrop = document.getElementById("routeModalBackdrop");
        const modal = document.getElementById("routeModal");
        if (backdrop) backdrop.classList.add("show");
        if (modal) modal.classList.add("show");
        document.body.style.overflow = "hidden";
    }

    function closeRouteModal() {
        const backdrop = document.getElementById("routeModalBackdrop");
        const modal = document.getElementById("routeModal");
        if (backdrop) backdrop.classList.remove("show");
        if (modal) modal.classList.remove("show");
        document.body.style.overflow = "";
    }

    function openAddRouteModal() {
        isEditMode = false;
        document.getElementById("routeModalTitle").innerHTML = `<i class="fa fa-plus-circle text-primary me-2"></i> Add Dynamic Route`;
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
        openRouteModal();
    }

    function openEditRouteModal(configId) {
        const route = allRoutes.find(r => r.configId.toLowerCase() === configId.toLowerCase());
        if (!route) return;

        isEditMode = true;
        document.getElementById("routeModalTitle").innerHTML = `<i class="fa fa-pencil text-primary me-2"></i> Edit Dynamic Route`;
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
        openRouteModal();
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
                .replace(/:username/g, getActiveUserName())
                .replace(/:userroles/g, getActiveUserRoles().split(",")[0])
                .replace(/:appname/g, getActiveAppName())
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
            showToast("Please fill out all required fields marked with *.", 4000, "warning");
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

        const activeApp = getActiveAppName();

        const saveBtn = document.getElementById("btnModalSaveRoute");
        const originalBtnHtml = saveBtn ? saveBtn.innerHTML : "";
        if (saveBtn) {
            saveBtn.disabled = true;
            saveBtn.innerHTML = `<i class="fa fa-spinner fa-spin me-1"></i> Saving...`;
        }

        try {
            const saveUrl = await getAxiApiUrl(`command-config/save?appname=${encodeURIComponent(activeApp)}`);
            const res = await fetch(saveUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                const err = await res.text();
                showToast(`Error saving route (${res.status}): ${err}`, 5000, "error");
                return;
            }

            if (smartLink && !isEditMode) {
                await autoLinkPromptToken(command, promptOptions);
            }

            invalidateAndRefreshPalette();
            closeRouteModal();
            showToast("Dynamic route saved successfully!", 3500, "success");
            await loadPagedRoutes();
        } catch (err) {
            console.error("Save error:", err);
            showToast(`Failed to save configuration: ${err.message}`, 5000, "error");
        } finally {
            if (saveBtn) {
                saveBtn.disabled = false;
                saveBtn.innerHTML = originalBtnHtml;
            }
        }
    }

    async function deleteRoute(configId) {
        if (!confirm(`Are you sure you want to delete route '${configId}'?`)) return;

        const activeApp = getActiveAppName();

        try {
            const delUrl = await getAxiApiUrl(`command-config/delete?configId=${encodeURIComponent(configId)}&appname=${encodeURIComponent(activeApp)}`);
            const res = await fetch(delUrl, {
                method: "POST"
            });

            if (!res.ok) {
                const err = await res.text();
                showToast(`Error deleting route (${res.status}): ${err}`, 5000, "error");
                return;
            }

            invalidateAndRefreshPalette();
            showToast("Route deleted successfully.", 3500, "success");
            await loadPagedRoutes();
        } catch (err) {
            console.error("Delete error:", err);
            showToast(`Failed to delete configuration: ${err.message}`, 5000, "error");
        }
    }

    // =========================================================================
    // TAB 2: PROMPTS MANAGEMENT & SMART-LINK (100% Dynamic from Database)
    // =========================================================================
    function renderPromptCommandDropdown() {
        const select = document.getElementById("promptCommandSelect");
        if (!select) return;

        // Show only 'Configure' and 'SDK' command options
        const allowedCommands = ["configure", "sdk"];
        const tokenMap = new Map();

        allPrompts.forEach(p => {
            if (p.cmdToken) {
                const label = ((p.command && p.command.trim()) || (p.commandGroup && p.commandGroup.trim()) || "").trim();
                if (allowedCommands.includes(label.toLowerCase()) && !tokenMap.has(p.cmdToken)) {
                    tokenMap.set(p.cmdToken, label);
                }
            }
        });

        // Fallback matching for token IDs 4 (Configure) and 7 (SDK) if command label is unpopulated
        if (tokenMap.size === 0) {
            allPrompts.forEach(p => {
                if (p.cmdToken && (p.cmdToken === 4 || p.cmdToken === 7) && !tokenMap.has(p.cmdToken)) {
                    const label = p.cmdToken === 4 ? "Configure" : "SDK";
                    tokenMap.set(p.cmdToken, label);
                }
            });
        }

        const countBadge = document.getElementById("promptCountBadge");
        if (countBadge) countBadge.innerText = allPrompts.length;

        select.innerHTML = Array.from(tokenMap.entries())
            .map(([token, name]) => `<option value="${token}">${escapeHtml(name)} (Token ${token})</option>`)
            .join("");

        if (tokenMap.size > 0) {
            select.value = Array.from(tokenMap.keys())[0].toString();
        }

        renderSelectedPromptDetails();
    }

    function renderSelectedPromptDetails() {
        const select = document.getElementById("promptCommandSelect");
        if (!select) return;

        const selectedToken = parseInt(select.value, 10);
        const commandRows = allPrompts.filter(p => p.cmdToken === selectedToken);

        const container = document.getElementById("promptDetailsCard");
        if (!container) return;

        if (commandRows.length === 0) {
            container.innerHTML = `<div class="text-muted small">No prompt records found in database for Token ${selectedToken}.</div>`;
            return;
        }

        // Strictly find Position 2 (promptValues) and Position 3 (promptSource)
        const promptRow = commandRows.find(p => p.wordPos === 2) ||
                          commandRows.find(p => p.promptValues && p.promptValues.trim()) ||
                          commandRows[0];

        const sourceRow = commandRows.find(p => p.wordPos === 3) ||
                          commandRows.find(p => p.promptSource && p.promptSource.trim()) ||
                          commandRows.find(p => p !== promptRow);

        const rawValues = promptRow?.promptValues || "";
        const rawSources = sourceRow?.promptSource || "";

        const values = rawValues ? rawValues.split(",").map(v => v.trim()).filter(Boolean) : [];
        const sources = rawSources ? rawSources.split(",").map(s => s.trim()).filter(Boolean) : [];
        const isAligned = values.length > 0 && (values.length === sources.length);

        const posValLabel = `Position 2 (Prompt Values)`;
        const posSrcLabel = `Position 3 (Data Sources)`;

        container.innerHTML = `
            <div class="row g-4">
                <div class="col-md-12">
                    <div class="alert ${isAligned ? 'alert-success' : (values.length === 0 ? 'alert-info' : 'alert-warning')} py-2 px-3 small d-flex align-items-center justify-content-between" style="border-radius: var(--radius-md);">
                        <span class="d-flex align-items-center">
                            <i class="fa ${isAligned ? 'fa-check-circle text-success' : (values.length === 0 ? 'fa-info-circle text-info' : 'fa-exclamation-triangle text-warning')} me-2" style="font-size: 15px;"></i>
                            <span>
                                <b>Positional Alignment:</b> ${posValLabel} has <b>${values.length}</b> values; ${posSrcLabel} has <b>${sources.length}</b> sources.
                                ${(!isAligned && values.length > 0) ? ' (Warning: count mismatch may cause prompt misalignment!)' : ''}
                            </span>
                        </span>
                        <button id="btnSavePromptTokens" class="btn-primary-modern btn-sm" onclick="window.AxiConfigManager.savePromptTokensFromUI(${selectedToken})">
                            <i class="fa fa-floppy-o me-1"></i> Save Changes
                        </button>
                    </div>
                </div>

                <div class="col-md-6">
                    <label class="form-label small fw-bold" style="color: var(--text-heading);">${escapeHtml(posValLabel)}</label>
                    <div class="token-cloud-container">
                        ${values.length > 0 ? values.map((v, idx) => `
                            <span class="token-chip">
                                ${escapeHtml(v)}
                                <i class="fa fa-times remove-token" onclick="window.AxiConfigManager.removePromptToken(${selectedToken}, ${idx})" title="Remove token"></i>
                            </span>
                        `).join("") : '<div class="text-muted small p-2">No static prompt tokens configured. Add tokens below.</div>'}
                    </div>
                    <div class="input-group input-group-sm mt-2">
                        <input type="text" class="form-control" id="newPromptValInput" placeholder="Add new prompt token (e.g. SLA)">
                        <button class="btn btn-outline-primary fw-bold d-flex align-items-center gap-1" type="button" onclick="window.AxiConfigManager.addPromptToken(${selectedToken})">
                            <i class="fa fa-plus"></i> Add
                        </button>
                    </div>
                </div>

                <div class="col-md-6">
                    <label class="form-label small fw-bold" style="color: var(--text-heading);">${escapeHtml(posSrcLabel)} (1-to-1 Mapped)</label>
                    <textarea class="form-control textarea-sources" id="promptSourcesTextarea" rows="6" placeholder="Comma-separated API data sources...">${escapeHtml(rawSources)}</textarea>
                    <div class="small text-muted mt-2">Comma-separated list of API data sources mapped 1-to-1 to each prompt value. Use <code>Axi_Dummy</code> for static options.</div>
                </div>
            </div>
        `;
    }

    async function autoLinkPromptToken(command, promptOption) {
        const commandRows = allPrompts.filter(p => ((p.command || '').toLowerCase() === command.toLowerCase()) || ((p.commandGroup || '').toLowerCase() === command.toLowerCase()));
        const promptRow = commandRows.find(p => p.wordPos === 2) || commandRows.find(p => p.promptValues && p.promptValues.trim());
        const sourceRow = commandRows.find(p => p.wordPos === 3) || commandRows.find(p => p.promptSource && p.promptSource.trim());

        if (promptRow) {
            const values = (promptRow.promptValues || "").split(",").map(v => v.trim()).filter(Boolean);
            if (!values.some(v => v.toLowerCase() === promptOption.toLowerCase())) {
                values.push(promptOption);
                const sources = (sourceRow?.promptSource || "").split(",").map(s => s.trim()).filter(Boolean);
                sources.push("Axi_Dummy");

                const activeApp = getActiveAppName();
                const savePromptUrl = await getAxiApiUrl(`command-prompts/save?appname=${encodeURIComponent(activeApp)}`);

                // 1. Update wordPos 2 with updated promptValues
                const payloadPos2 = {
                    cmdToken: promptRow.cmdToken,
                    wordPos: 2,
                    promptValues: values.join(","),
                    promptSource: ""
                };

                // 2. Update wordPos 3 with updated promptSource
                const payloadPos3 = {
                    cmdToken: promptRow.cmdToken,
                    wordPos: 3,
                    promptValues: "",
                    promptSource: sources.join(",")
                };

                await Promise.all([
                    fetch(savePromptUrl, {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(payloadPos2)
                    }),
                    fetch(savePromptUrl, {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(payloadPos3)
                    })
                ]);

                promptRow.promptValues = values.join(",");
                if (sourceRow) sourceRow.promptSource = sources.join(",");
            }
        }
    }

    function addPromptToken(cmdToken) {
        const input = document.getElementById("newPromptValInput");
        const val = (input?.value || "").trim();
        if (!val) return;

        const commandRows = allPrompts.filter(p => p.cmdToken === cmdToken);
        const promptRow = commandRows.find(p => p.wordPos === 2) || commandRows.find(p => p.promptValues && p.promptValues.trim()) || commandRows[0];
        const sourceRow = commandRows.find(p => p.wordPos === 3) || commandRows.find(p => p.promptSource && p.promptSource.trim());

        if (promptRow) {
            const values = (promptRow.promptValues || "").split(",").map(v => v.trim()).filter(Boolean);
            values.push(val);
            promptRow.promptValues = values.join(",");

            if (sourceRow) {
                const sources = (sourceRow.promptSource || "").split(",").map(s => s.trim()).filter(Boolean);
                sources.push("Axi_Dummy");
                sourceRow.promptSource = sources.join(",");
            }
            renderSelectedPromptDetails();
        }
        input.value = "";
    }

    function removePromptToken(cmdToken, index) {
        const commandRows = allPrompts.filter(p => p.cmdToken === cmdToken);
        const promptRow = commandRows.find(p => p.wordPos === 2) || commandRows.find(p => p.promptValues && p.promptValues.trim()) || commandRows[0];
        const sourceRow = commandRows.find(p => p.wordPos === 3) || commandRows.find(p => p.promptSource && p.promptSource.trim());

        if (promptRow) {
            const values = (promptRow.promptValues || "").split(",").map(v => v.trim()).filter(Boolean);
            values.splice(index, 1);
            promptRow.promptValues = values.join(",");

            if (sourceRow) {
                const sources = (sourceRow.promptSource || "").split(",").map(s => s.trim()).filter(Boolean);
                if (sources.length > index) sources.splice(index, 1);
                sourceRow.promptSource = sources.join(",");
            }
            renderSelectedPromptDetails();
        }
    }

    async function savePromptTokensFromUI(cmdToken) {
        const commandRows = allPrompts.filter(p => p.cmdToken === cmdToken);
        const promptRow = commandRows.find(p => p.wordPos === 2) || commandRows.find(p => p.promptValues && p.promptValues.trim()) || commandRows[0];
        const sourceRow = commandRows.find(p => p.wordPos === 3) || commandRows.find(p => p.promptSource && p.promptSource.trim());
        const sourcesText = (document.getElementById("promptSourcesTextarea")?.value || "").trim();

        const updatedValues = (promptRow?.promptValues || "").trim();
        const updatedSources = sourcesText;

        if (promptRow) promptRow.promptValues = updatedValues;
        if (sourceRow) sourceRow.promptSource = updatedSources;

        const saveBtn = document.getElementById("btnSavePromptTokens");
        const originalBtnHtml = saveBtn ? saveBtn.innerHTML : "";
        if (saveBtn) {
            saveBtn.disabled = true;
            saveBtn.innerHTML = `<i class="fa fa-spinner fa-spin me-1"></i> Saving...`;
        }

        const activeApp = getActiveAppName();

        try {
            const savePromptUrl = await getAxiApiUrl(`command-prompts/save?appname=${encodeURIComponent(activeApp)}`);

            // 1. Update wordPos 2 with promptValues
            const payloadPos2 = {
                cmdToken: cmdToken,
                wordPos: 2,
                promptValues: updatedValues,
                promptSource: ""
            };

            // 2. Update wordPos 3 with promptSource
            const payloadPos3 = {
                cmdToken: cmdToken,
                wordPos: 3,
                promptValues: "",
                promptSource: updatedSources
            };

            const [res2, res3] = await Promise.all([
                fetch(savePromptUrl, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payloadPos2)
                }),
                fetch(savePromptUrl, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payloadPos3)
                })
            ]);

            if (!res2.ok) {
                const err = await res2.text();
                showToast(`Error saving prompt values for Position 2 (${res2.status}): ${err}`, 5000, "error");
                return;
            }

            if (!res3.ok) {
                const err = await res3.text();
                showToast(`Error saving data sources for Position 3 (${res3.status}): ${err}`, 5000, "error");
                return;
            }

            invalidateAndRefreshPalette();
            showToast("Prompt tokens (Position 2) and Data sources (Position 3) saved successfully!", 3500, "success");
            await loadPromptsData();
        } catch (err) {
            console.error("Save prompt error:", err);
            showToast(`Failed to save prompt tokens: ${err.message}`, 5000, "error");
        } finally {
            if (saveBtn) {
                saveBtn.disabled = false;
                saveBtn.innerHTML = originalBtnHtml;
            }
        }
    }

    // =========================================================================
    // TOAST NOTIFICATIONS (Matches axicmdmain.js)
    // =========================================================================
    function showToast(message, duration = 4000, isSuccess = false) {
        let alertType = "error";
        if (isSuccess === true || isSuccess === "success") {
            alertType = "success";
        } else if (isSuccess === "warning") {
            alertType = "warning";
        } else if (isSuccess === "info" || isSuccess === "information") {
            alertType = "info";
        } else if (isSuccess === false || isSuccess === "error" || isSuccess === "danger") {
            alertType = "error";
        } else if (typeof message === "string") {
            const lowerMsg = message.toLowerCase();
            if (lowerMsg.includes("warning") || lowerMsg.startsWith("please ")) {
                alertType = "warning";
            } else if (lowerMsg.includes("success") || lowerMsg.includes("saved")) {
                alertType = "success";
            } else {
                alertType = "error";
            }
        }

        const alertFn = (typeof top !== "undefined" && typeof top.showAlertDialog === "function" && top.showAlertDialog) ||
            (typeof parent !== "undefined" && typeof parent.showAlertDialog === "function" && parent.showAlertDialog) ||
            (typeof window !== "undefined" && typeof window.showAlertDialog === "function" && window.showAlertDialog);

        if (alertFn) {
            try {
                alertFn(alertType, message);
                return;
            } catch (e) { }
        }

        let styleTag = document.getElementById("axi-toast-styles");
        if (!styleTag) {
            styleTag = document.createElement("style");
            styleTag.id = "axi-toast-styles";
            styleTag.innerHTML = `
                #axi-toast-container {
                    position: fixed;
                    bottom: 30px;
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
                    min-width: 280px;
                    max-width: 420px;
                    box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.15), 0 4px 6px -2px rgba(0, 0, 0, 0.1);
                    opacity: 0;
                    transform: translateY(16px) scale(0.95);
                    transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
                    pointer-events: auto;
                    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
                    font-size: 13.5px;
                    line-height: 1.4;
                    color: #ffffff;
                    white-space: pre-line;
                }
                @media (max-width: 576px) {
                    #axi-toast-container {
                        right: 16px;
                        left: 16px;
                        bottom: 20px;
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

        let iconSvg = "";
        let borderStyle = "";
        let bgStyle = "";

        if (alertType === "success") {
            bgStyle = "#059669";
            borderStyle = "1px solid #10b981";
            iconSvg = `<i class="fa fa-check-circle" style="color: #ffffff; margin-right: 10px; font-size: 16px;"></i>`;
        } else if (alertType === "warning") {
            bgStyle = "#d97706";
            borderStyle = "1px solid #f59e0b";
            iconSvg = `<i class="fa fa-exclamation-triangle" style="color: #ffffff; margin-right: 10px; font-size: 16px;"></i>`;
        } else if (alertType === "info") {
            bgStyle = "#2563eb";
            borderStyle = "1px solid #3b82f6";
            iconSvg = `<i class="fa fa-info-circle" style="color: #ffffff; margin-right: 10px; font-size: 16px;"></i>`;
        } else {
            bgStyle = "#dc2626";
            borderStyle = "1px solid #ef4444";
            iconSvg = `<i class="fa fa-times-circle" style="color: #ffffff; margin-right: 10px; font-size: 16px;"></i>`;
        }

        toast.style.backgroundColor = bgStyle;
        toast.style.border = borderStyle;

        const iconDiv = document.createElement("div");
        iconDiv.innerHTML = iconSvg;
        iconDiv.style.display = "flex";
        iconDiv.style.alignItems = "center";

        const textSpan = document.createElement("span");
        textSpan.textContent = message;
        textSpan.style.flexGrow = "1";
        textSpan.style.marginRight = "10px";

        const closeBtn = document.createElement("button");
        closeBtn.innerHTML = `<i class="fa fa-times" style="font-size: 12px;"></i>`;
        Object.assign(closeBtn.style, {
            background: "none",
            border: "none",
            color: "rgba(255, 255, 255, 0.7)",
            cursor: "pointer",
            padding: "4px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            outline: "none"
        });

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

    // Expose public methods to window
    window.AxiConfigManager = {
        loadAllData,
        loadPagedRoutes,
        openAddRouteModal,
        openEditRouteModal,
        closeRouteModal,
        saveRouteFromModal,
        deleteRoute,
        renderSelectedPromptDetails,
        addPromptToken,
        removePromptToken,
        savePromptTokensFromUI,
        insertPlaceholder,
        changePageSize,
        goToPage
    };

})();
