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
            promptId: promptId,
            targetUrl: url,
            paramField: String(pick(raw, ["paramField", "ParamField", "param_field"])).trim(),
            extraParams: String(pick(raw, ["extraParams", "ExtraParams", "extra_params"])).trim(),
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

    /** Returns new groups with one row replaced by its updated version (same tab, same place); the input is not modified. */
    function replaceRow(groups, id, row) {
        const out = {};
        TABS.forEach((k) => {
            out[k] = groups[k].map((r) => (r.id === id ? row : r));
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
        const Logic = window.AxiConfigLogic;
        const AVAILABLE_TYPES = Logic.CUSTOM_TYPES;   // tstruct, iview, ivtoivload, url (never action, processflow, shellaction)
        const TYPE_LABELS = { tstruct: "Form (tstruct)", iview: "Report (iview)", ivtoivload: "Report with load (ivtoivload)", url: "URL" };

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

        function updateCounts() {
            $("acCountConfigure").textContent = groups.configure.length;
            $("acCountSdk").textContent = groups.sdk.length;
        }

        // ---- Add option drawer ----
        const drawer = $("acDrawer"), scrim = $("acScrim"), form = $("acForm"), addBtn = $("acAdd");
        const formError = $("acFormError"), saveBtn = $("acSave");
        const fields = ["name", "promptId", "targetUrl", "paramField", "extraParams"];
        const inputOf = { name: $("fName"), promptId: $("fPromptId"), targetUrl: $("fTargetUrl"), paramField: $("fParamField"), extraParams: $("fExtraParams") };
        const errOf = { name: $("eName"), promptId: $("ePromptId"), targetUrl: $("eTargetUrl"), paramField: $("eParamField"), extraParams: $("eExtraParams") };

        function showFieldsForType() {
            const type = $("fType").value;
            const used = Logic.fieldsFor(type);
            Array.prototype.forEach.call(form.querySelectorAll("[data-field]"), (el) => {
                el.hidden = used.indexOf(el.getAttribute("data-field")) === -1;
            });
            $("lPromptId").textContent = type === "tstruct" ? "Form" : "Report";
            $("fPromptIdList").textContent = "";
        }

        // Structure picker: the palette (parent window) searches the paged axi_structmetalist source
        let structTimer = null, structAsk = 0;
        function searchStructures() {
            const term = inputOf.promptId.value;
            clearTimeout(structTimer);
            if (!Logic.canSearchStructures(term)) { $("fPromptIdList").textContent = ""; return; }
            structTimer = setTimeout(() => {
                const shell = window.top || window.parent;
                let search = null;
                try { search = shell && shell.axiSearchStructures; } catch (e) { }
                if (typeof search !== "function") return;   // opened on its own: the ID can still be typed
                const ask = ++structAsk, type = $("fType").value;
                Promise.resolve(search.call(shell, term.trim())).then((rows) => {
                    if (ask !== structAsk) return;
                    const list = $("fPromptIdList");
                    list.textContent = "";
                    Logic.structureChoices(rows, type).forEach((c) => {
                        const o = document.createElement("option");
                        o.value = c.value;
                        o.label = c.label;
                        list.appendChild(o);
                    });
                }).catch(() => { });
            }, 300);
        }

        function setFieldError(field, text) {
            errOf[field].textContent = text || "";
            if (text) inputOf[field].setAttribute("aria-invalid", "true"); else inputOf[field].removeAttribute("aria-invalid");
        }

        function clearErrors() {
            fields.forEach((f) => setFieldError(f, ""));
            formError.hidden = true;
            formError.textContent = "";
        }

        let editing = null;   // the row being edited or viewed; null = adding

        function setReadOnly(on) {
            Array.prototype.forEach.call(form.querySelectorAll("input[type=text], select"), (el) => {
                el.disabled = on;
            });
            saveBtn.hidden = on;
            $("acCancel").textContent = on ? "Close" : "Cancel";
        }

        function updateRenameWarning() {
            const w = editing && isEditable(editing) ? Logic.renameWarning(editing.name, inputOf.name.value) : "";
            $("acRenameWarn").hidden = !w;
            $("acRenameWarn").textContent = w;
        }

        function openDrawer(row) {
            editing = row && row.id ? row : null;
            form.reset();
            clearErrors();
            setReadOnly(false);
            $("acRenameWarn").hidden = true;
            $("acActiveWrap").hidden = !editing;
            const typeSel = $("fType");
            typeSel.textContent = "";
            AVAILABLE_TYPES.forEach((t) => {
                const o = document.createElement("option");
                o.value = t;
                o.textContent = TYPE_LABELS[t] || t;
                typeSel.appendChild(o);
            });
            $("fCommand").value = editing ? editing.command : (active === "sdk" ? "SDK" : "Configure");
            $("acDrawerTitle").textContent = !editing ? "Add option" : (isEditable(editing) ? "Edit option" : (Logic.isEditable(editing) ? "Option" : "Built-in option"));
            $("acHint").textContent = !editing ? "A Custom Option opens a page you choose. It appears at the end of the command's list."
                : (isEditable(editing) ? "Changes apply to the palette after it reloads; other users see them after they press Refresh."
                    : (Logic.isEditable(editing) ? "You need Configure Access to change this option."
                        : "A Built-in Option ships with the product, so its route is read-only. You can still switch it on or off."));
            $("fCommand").disabled = !!editing;
            const srcSel = $("fSource");
            srcSel.textContent = "";
            Logic.recordSourceOptions().forEach((s) => {
                const o = document.createElement("option");
                o.value = s.value;
                o.textContent = s.label;
                srcSel.appendChild(o);
            });
            if (editing) {
                const typeSel2 = $("fType");
                if (AVAILABLE_TYPES.indexOf(editing.type) === -1) {
                    // A Built-in type (action, processflow, ...) is shown as it is, read-only
                    const o = document.createElement("option");
                    o.value = editing.type;
                    o.textContent = editing.type;
                    typeSel2.appendChild(o);
                }
                typeSel2.value = editing.type;
                const src = editing.source || Logic.NO_SOURCE;
                if (!Array.prototype.some.call(srcSel.options, (o) => o.value === src)) {
                    const o = document.createElement("option");
                    o.value = src;
                    o.textContent = src;
                    srcSel.appendChild(o);
                }
                srcSel.value = src;
                fields.forEach((f) => { inputOf[f].value = f === "name" ? editing.name : (editing[f] || ""); });
                $("fActive").checked = editing.active;
                $("fActive").disabled = !hasAccess;
            }
            showFieldsForType();
            if (editing && !isEditable(editing)) {
                // Route fields that have a value stay visible but locked
                if (AVAILABLE_TYPES.indexOf(editing.type) === -1) {
                    Array.prototype.forEach.call(form.querySelectorAll("[data-field]"), (el) => { el.hidden = !editing[el.getAttribute("data-field")]; });
                }
                setReadOnly(true);
            }
            scrim.hidden = false;
            drawer.hidden = false;
            saveBtn.disabled = false;
            (editing && !isEditable(editing) ? $("fActive") : inputOf.name).focus();
        }

        let opener = null;
        function closeDrawer() {
            drawer.hidden = true;
            scrim.hidden = true;
            editing = null;
            (opener && document.contains(opener) ? opener : addBtn).focus();
        }

        // True when this user may change the route of the row in the drawer: Configure Access and a Custom Option
        const isEditable = (row) => hasAccess && Logic.isEditable(row);

        function existingNames(command) {
            return (groups[command.toLowerCase()] || []).map((r) => r.name);
        }

        function submitDrawer(e) {
            e.preventDefault();
            clearErrors();
            const input = { command: $("fCommand").value, type: $("fType").value, recordSource: $("fSource").value };
            fields.forEach((f) => { input[f] = inputOf[f].value; });
            if (editing && !isEditable(editing)) return;
            const checked = editing ? Logic.validateEdit(input, editing, existingNames(editing.command)) : Logic.validateOption(input, existingNames(input.command));
            if (!checked.ok) {
                let first = null;
                Object.keys(checked.errors).forEach((f) => {
                    if (errOf[f]) { setFieldError(f, checked.errors[f]); first = first || inputOf[f]; }
                    else { formError.hidden = false; formError.textContent = checked.errors[f]; }
                });
                if (first) first.focus();
                return;
            }
            saveBtn.disabled = true;
            formError.hidden = false;
            formError.className = "ac-note";
            formError.textContent = "Saving...";
            const target = editing;
            const request = target
                ? fetch(Logic.buildUpdateUrl(apiBase, target.id, appName), {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(Logic.buildUpdateBody(userName, hasAccess, checked.value))
                })
                : fetch(Logic.buildCreateUrl(apiBase, appName), {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(Logic.buildCreateBody(userName, hasAccess, checked.value))
                });
            request.then((res) => {
                if (!res.ok) return res.text().then((t) => { throw { status: res.status, text: t }; });
                return res.json();
            }).then((created) => {
                const key = checked.value.command.toLowerCase();
                if (target) groups = replaceRow(groups, target.id, normalizeRow(created));
                else groups[key] = groups[key].concat([normalizeRow(created)]);
                groups[key].sort((a, b) => a.name.localeCompare(b.name, undefined, { sensitivity: "base" }));
                updateCounts();
                const shell = window.top || window.parent;
                try { if (shell && typeof shell.axiReloadCommandConfig === "function") shell.axiReloadCommandConfig(); } catch (err) { }
                closeDrawer();
                selectTab(key);
                showNote((target ? "Saved \"" : "Added \"") + checked.value.name + "\". " + OTHER_USERS_NOTE, false);
            }).catch((err) => {
                saveBtn.disabled = false;
                formError.className = "ac-note ac-error";
                formError.hidden = false;
                formError.textContent = describeFailure(err && err.status, err && err.text);
            });
        }

        if (hasAccess) addBtn.hidden = false;
        addBtn.addEventListener("click", () => { opener = addBtn; openDrawer(null); });
        $("fType").addEventListener("change", showFieldsForType);
        inputOf.name.addEventListener("input", updateRenameWarning);
        // The Active switch in the drawer saves on its own, like the one in the table
        $("fActive").addEventListener("change", () => {
            const row = editing;
            if (!row) return;
            save(row, $("fActive").checked, $("fActive")).then(() => {
                if (editing === row) editing = groups[row.command.toLowerCase()].filter((r) => r.id === row.id)[0] || row;
            });
        });
        inputOf.promptId.addEventListener("input", searchStructures);
        $("acClose").addEventListener("click", closeDrawer);
        $("acCancel").addEventListener("click", closeDrawer);
        scrim.addEventListener("click", closeDrawer);
        form.addEventListener("submit", submitDrawer);
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape" && !drawer.hidden) { closeDrawer(); return; }
            // Keep Tab inside the open drawer
            if (e.key === "Tab" && !drawer.hidden) {
                const items = Array.prototype.filter.call(drawer.querySelectorAll("input, select, button"), (el) => !el.disabled && !el.closest("[hidden]"));
                if (!items.length) return;
                const first = items[0], last = items[items.length - 1];
                if (e.shiftKey && document.activeElement === first) { last.focus(); e.preventDefault(); }
                else if (!e.shiftKey && document.activeElement === last) { first.focus(); e.preventDefault(); }
            }
        });

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
                const act = document.createElement("td");
                const eb = document.createElement("button");
                eb.type = "button";
                eb.className = "ac-btn ac-btn-sm";
                eb.textContent = isEditable(r) ? "Edit" : "View";
                eb.setAttribute("aria-label", eb.textContent + ": " + r.name);
                eb.addEventListener("click", () => { opener = eb; openDrawer(r); });
                act.appendChild(eb);
                tr.appendChild(act);
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
                updateCounts();
                selectTab(active);
            })
            .catch((err) => {
                status.hidden = false;
                status.textContent = "Could not load the options (" + err.message + "). Check the connection and reopen the page.";
            });
        search.focus();
    }

    return { replaceRow, OTHER_USERS_NOTE, buildActiveUrl, buildActiveBody, setActive, describeFailure, normalizeRow, groupByTab, filterRows, buildListUrl, directionFor, init };
}));
