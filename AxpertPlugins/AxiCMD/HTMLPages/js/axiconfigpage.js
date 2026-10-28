/*
 * AxiCMD Config page (AxiCMDConfig.html). Read-only list of every `Configure` and `SDK` option.
 * The top half is pure logic (loaded by tests/axi-config-page-ticket-283.test.js); the bottom half wires the DOM
 * and only runs in a browser. Opened by the gear menu in axicmdmain.js with ?api=<command-config url>&appname=&username=&lang=.
 */
(function (root, factory) {
    const api = factory();
    if (typeof module === "object" && module.exports) module.exports = api;
    else { root.AxiConfigPage = api; if (typeof document !== "undefined") api.init(); }
}(typeof self !== "undefined" ? self : this, function () {
    "use strict";

    const RTL_LANGS = ["ar", "he", "fa", "ur"];
    const TABS = ["configure", "sdk"];

    function pick(raw, names) {
        for (let i = 0; i < names.length; i++) {
            const v = raw[names[i]];
            if (v !== undefined && v !== null) return v;
        }
        return "";
    }
    const isTrue = (v) => ["T", "TRUE", "1", "Y"].indexOf(String(v).trim().toUpperCase()) !== -1;

    /** Flattens one API row (camelCase, PascalCase or snake_case keys) into what the table shows. */
    function normalizeRow(raw) {
        const url = String(pick(raw, ["targetUrl", "TargetUrl", "target_url"])).trim();
        const promptId = String(pick(raw, ["promptId", "PromptId", "prompt_id"])).trim();
        return {
            id: String(pick(raw, ["configId", "ConfigId", "config_id"])),
            command: String(pick(raw, ["command", "Command"])).trim(),
            name: String(pick(raw, ["promptOptions", "PromptOptions", "prompt_options"])).trim(),
            type: String(pick(raw, ["promptOptionType", "PromptOptionType", "prompt_option_type"])).trim(),
            target: url || promptId,
            active: isTrue(pick(raw, ["active", "Active"])),
            builtIn: isTrue(pick(raw, ["isSystem", "IsSystem", "is_system"])),
            source: String(pick(raw, ["recordSource", "RecordSource", "record_source"])).trim()
        };
    }

    /** Splits rows into the two tabs, inactive ones included, sorted by name. */
    function groupByTab(rows) {
        const out = { configure: [], sdk: [] };
        (rows || []).forEach((raw) => {
            const r = normalizeRow(raw);
            const key = r.command.toLowerCase();
            if (TABS.indexOf(key) !== -1) out[key].push(r);
        });
        TABS.forEach((k) => out[k].sort((a, b) => a.name.localeCompare(b.name, undefined, { sensitivity: "base" })));
        return out;
    }

    /** Keeps rows whose name or target contains the term. */
    function filterRows(rows, term) {
        const t = String(term || "").trim().toLowerCase();
        if (!t) return rows.slice();
        return rows.filter((r) => r.name.toLowerCase().indexOf(t) !== -1 || r.target.toLowerCase().indexOf(t) !== -1);
    }

    function buildListUrl(baseUrl, appname, username) {
        return baseUrl.replace(/\/+$/, "") + "/options?appname=" + encodeURIComponent(appname) + "&username=" + encodeURIComponent(username);
    }

    function directionFor(lang) {
        const code = String(lang || "").toLowerCase().split(/[-_]/)[0];
        return RTL_LANGS.indexOf(code) !== -1 ? "rtl" : "ltr";
    }

    const OTHER_USERS_NOTE = "Saved. Other users see the change after they press Refresh in AxiCMD.";

    function buildActiveUrl(baseUrl, configId, appname) {
        return baseUrl.replace(/\/+$/, "") + "/options/" + encodeURIComponent(configId) + "/active?appname=" + encodeURIComponent(appname);
    }

    function buildActiveBody(username, active, appMgrAccess) {
        return { username: username, active: !!active, appMgrAccess: !!appMgrAccess };
    }

    /** Returns new groups with one row's Active flag changed; the input is not modified. */
    function setActive(groups, id, active) {
        const out = {};
        TABS.forEach((k) => {
            out[k] = groups[k].map((r) => (r.id === id ? Object.assign({}, r, { active: !!active }) : r));
        });
        return out;
    }

    /** A readable sentence for a failed save. status 0 means the request never got an answer. */
    function describeFailure(status, text) {
        const detail = String(text || "").replace(/^"|"$/g, "").trim();
        if (!status) return "Could not reach the server. Check the connection and try again.";
        if (detail) return detail + " (HTTP " + status + ")";
        return "The change was not saved (HTTP " + status + ").";
    }

    // ---- Browser part ----------------------------------------------------------------------------------------
    function init() {
        const q = new URLSearchParams(location.search);
        const lang = q.get("lang") || document.documentElement.lang || "";
        document.documentElement.setAttribute("dir", directionFor(lang));
        if (lang) document.documentElement.setAttribute("lang", lang);

        const $ = (id) => document.getElementById(id);
        const note = $("acNote");
        const body = $("acBody"), search = $("acSearch"), status = $("acStatus"), count = $("acCount");
        const tabs = Array.prototype.slice.call(document.querySelectorAll("[role=tab]"));
        let groups = { configure: [], sdk: [] };
        let active = "configure";
        const hasAccess = q.get("access") === "1";
        const apiBase = q.get("api"), appName = q.get("appname"), userName = q.get("username") || "";

        function showNote(text, isError) {
            note.hidden = !text;
            note.textContent = text || "";
            note.className = "ac-note" + (isError ? " ac-error" : "");
        }

        function save(row, wanted, input) {
            input.disabled = true;
            showNote("Saving...", false);
            return fetch(buildActiveUrl(apiBase, row.id, appName), {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(buildActiveBody(userName, wanted, hasAccess))
            }).then((res) => {
                if (!res.ok) return res.text().then((t) => { throw { status: res.status, text: t }; });
                groups = setActive(groups, row.id, wanted);
                // The palette drops this user's local copy and reloads its commands
                const shell = window.top || window.parent;
                try { if (shell && typeof shell.axiReloadCommandConfig === "function") shell.axiReloadCommandConfig(); } catch (e) { }
                render();
                showNote(OTHER_USERS_NOTE, false);
            }).catch((err) => {
                input.checked = !wanted;
                input.disabled = false;
                showNote(describeFailure(err && err.status, err && err.text), true);
            });
        }

        function cell(text, cls) {
            const td = document.createElement("td");
            if (cls) td.className = cls;
            td.textContent = text;
            return td;
        }

        function render() {
            const rows = filterRows(groups[active], search.value);
            body.textContent = "";
            rows.forEach((r) => {
                const tr = document.createElement("tr");
                if (!r.active) tr.className = "ac-off";
                const nameTd = cell(r.name, "ac-name");
                if (r.builtIn) {
                    const b = document.createElement("span");
                    b.className = "ac-badge";
                    b.textContent = "Built-in";
                    nameTd.appendChild(b);
                }
                tr.appendChild(nameTd);
                tr.appendChild(cell(r.type, "ac-type"));
                tr.appendChild(cell(r.target, "ac-target"));
                const st = document.createElement("td");
                const sw = document.createElement("label");
                sw.className = "ac-switch";
                const input = document.createElement("input");
                input.type = "checkbox";
                input.checked = r.active;
                input.disabled = !hasAccess;
                input.setAttribute("aria-label", "Active: " + r.name);
                const track = document.createElement("span");
                track.className = "ac-track";
                const lbl = document.createElement("span");
                lbl.className = "ac-switch-label " + (r.active ? "ac-on" : "ac-inactive");
                lbl.textContent = r.active ? "Active" : "Inactive";
                input.addEventListener("change", () => save(r, input.checked, input));
                sw.appendChild(input);
                sw.appendChild(track);
                sw.appendChild(lbl);
                st.appendChild(sw);
                tr.appendChild(st);
                body.appendChild(tr);
            });
            count.textContent = rows.length + " of " + groups[active].length;
            status.hidden = rows.length > 0;
            status.textContent = groups[active].length ? "Nothing matches \"" + search.value.trim() + "\". Try part of a name or target." : "No options found.";
        }

        function selectTab(name) {
            active = name;
            tabs.forEach((t) => {
                const on = t.dataset.tab === name;
                t.setAttribute("aria-selected", on ? "true" : "false");
                t.tabIndex = on ? 0 : -1;
            });
            $("acTable").setAttribute("aria-labelledby", "tab-" + name);
            render();
        }

        tabs.forEach((t, i) => {
            t.addEventListener("click", () => selectTab(t.dataset.tab));
            t.addEventListener("keydown", (e) => {
                const step = e.key === "ArrowRight" ? 1 : e.key === "ArrowLeft" ? -1 : 0;
                if (!step) return;
                // Arrow keys follow the reading direction
                const dir = document.documentElement.getAttribute("dir") === "rtl" ? -step : step;
                const next = tabs[(i + dir + tabs.length) % tabs.length];
                next.focus();
                selectTab(next.dataset.tab);
                e.preventDefault();
            });
        });
        search.addEventListener("input", render);

        const api = apiBase, appname = appName, username = userName;
        if (!api || !appname) {
            status.hidden = false;
            status.textContent = "This page needs to be opened from the AxiCMD palette.";
            return;
        }
        status.hidden = false;
        status.textContent = "Loading options...";
        fetch(buildListUrl(api, appname, username || ""))
            .then((res) => { if (!res.ok) throw new Error("HTTP " + res.status); return res.json(); })
            .then((rows) => {
                groups = groupByTab(rows);
                $("acCountConfigure").textContent = groups.configure.length;
                $("acCountSdk").textContent = groups.sdk.length;
                selectTab(active);
            })
            .catch((err) => {
                status.hidden = false;
                status.textContent = "Could not load the options (" + err.message + "). Check the connection and reopen the page.";
            });
        search.focus();
    }

    return { OTHER_USERS_NOTE, buildActiveUrl, buildActiveBody, setActive, describeFailure, normalizeRow, groupByTab, filterRows, buildListUrl, directionFor, init };
}));
