/*
 * AxiCMD Config page (AxiCMDConfig.html). Lists the `Configure` and `SDK` options 25 a page; the API pages and searches.
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

    // Rows per page; the API pages and searches (GET command-config/options/page), the page only shows one page at a time
    const PAGE_SIZE = 25;

    /** One page of a command's options, searched on the server (name or target contains the term). */
    // show: "custom" or "inactive" filters the list ("all" or nothing adds no parameter).
    // focus: a config_id; the API then answers with the page that holds it.
    function buildPageUrl(baseUrl, appname, command, search, pageNo, show, focus) {
        return baseUrl.replace(/\/+$/, "") + "/options/page?appname=" + encodeURIComponent(appname) +
            "&command=" + encodeURIComponent(command) + "&search=" + encodeURIComponent(String(search || "").trim()) +
            "&pageNo=" + (pageNo > 0 ? Math.floor(pageNo) : 1) + "&pageSize=" + PAGE_SIZE +
            (show && show !== "all" ? "&show=" + encodeURIComponent(show) : "") +
            (focus ? "&focus=" + encodeURIComponent(focus) : "");
    }

    /** Flattens the API page (camelCase or PascalCase keys) into rows plus the numbers the pager and the tabs show. */
    function normalizePage(raw) {
        const src = raw || {};
        const num = (names, fallback) => {
            const n = parseInt(pick(src, names), 10);
            return isNaN(n) ? fallback : n;
        };
        const items = pick(src, ["items", "Items"]);
        const rows = (Array.isArray(items) ? items : []).map(normalizeRow);
        const total = num(["totalCount", "TotalCount"], rows.length);
        const pageSize = num(["pageSize", "PageSize"], PAGE_SIZE) || PAGE_SIZE;
        return {
            rows: rows,
            total: total,
            pageNo: num(["pageNo", "PageNo"], 1),
            pageSize: pageSize,
            totalPages: Math.max(1, num(["totalPages", "TotalPages"], Math.ceil(total / pageSize))),
            counts: { configure: num(["configureCount", "ConfigureCount"], 0), sdk: num(["sdkCount", "SdkCount"], 0) }
        };
    }

    /** Page buttons to show: the first, the last and the neighbours of the current page; "..." marks a gap of two or more. */
    function pageNumbers(pageNo, totalPages) {
        const out = [];
        let last = 0;
        for (let p = 1; p <= totalPages; p++) {
            if (p !== 1 && p !== totalPages && Math.abs(p - pageNo) > 1) continue;
            if (p - last === 2) out.push(p - 1);
            else if (p - last > 2) out.push("...");
            out.push(p);
            last = p;
        }
        return out;
    }

    /** "26-50 of 120": the rows on this page. */
    function rangeText(pageNo, pageSize, total) {
        if (!total) return "0 of 0";
        const from = (pageNo - 1) * pageSize + 1;
        return from + "-" + Math.min(total, pageNo * pageSize) + " of " + total;
    }

    function directionFor(lang) {
        const code = String(lang || "").toLowerCase().split(/[-_]/)[0];
        return RTL_LANGS.indexOf(code) !== -1 ? "rtl" : "ltr";
    }

    const OTHER_USERS_NOTE = "Other users see the change after they press Refresh in AxiCMD.";

    /** Toast text after an Active switch saves: names the option and says the change reaches everyone in the app. */
    function switchMessage(name, isActive) {
        return "\"" + name + "\" is " + (isActive ? "back in the palette" : "hidden from the palette") + " for everyone in this app. " + OTHER_USERS_NOTE;
    }

    function buildActiveUrl(baseUrl, configId, appname) {
        return baseUrl.replace(/\/+$/, "") + "/options/" + encodeURIComponent(configId) + "/active?appname=" + encodeURIComponent(appname);
    }

    /** access is { appMgrAccess, buildAccess }; the API refuses the write unless both are true. */
    function buildActiveBody(username, active, access) {
        return { username: username, active: !!active, appMgrAccess: !!(access && access.appMgrAccess), buildAccess: !!(access && access.buildAccess) };
    }

    /** Both flags from the page URL; the page lets the user change options only when both are set. */
    function readAccess(query) {
        const access = { appMgrAccess: query.get("appmgraccess") === "1", buildAccess: query.get("buildaccess") === "1" };
        access.canConfigure = access.appMgrAccess && access.buildAccess;
        return access;
    }

    /** Returns new rows with one row's Active flag changed; the input is not modified. */
    function setActive(rows, id, active) {
        return rows.map((r) => (r.id === id ? Object.assign({}, r, { active: !!active }) : r));
    }

    /** Returns new rows with one row replaced by its updated version (same place); the input is not modified. */
    function replaceRow(rows, id, row) {
        return rows.map((r) => (r.id === id ? row : r));
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
        const body = $("acBody"), search = $("acSearch"), status = $("acStatus"), count = $("acPageOf");
        let showing = "all";   // the All / Custom / Inactive filter
        const tabs = Array.prototype.slice.call(document.querySelectorAll("[role=tab]"));
        // page: what the table shows now. pageNoOf: the page each tab was left on. cache: pages already fetched, by tab, search and page.
        let page = normalizePage({});
        let active = "configure";
        const pageNoOf = { configure: 1, sdk: 1 };
        const cache = new Map();
        let loadAsk = 0;
        const access = readAccess(q);
        const hasAccess = access.canConfigure;
        const apiBase = q.get("api"), appName = q.get("appname"), userName = q.get("username") || "";
        const Logic = window.AxiConfigLogic;
        const AVAILABLE_TYPES = Logic.CUSTOM_TYPES;   // tstruct, iview, ivtoivload, url (never action, processflow, shellaction)
        const TYPE_LABELS = { tstruct: "Form (tstruct)", iview: "Report (iview)", ivtoivload: "Report with load (ivtoivload)", url: "URL" };

        // The note is a toast pinned to the bottom of the page, so showing it never moves the table.
        // Errors stay until the next note; other notes clear themselves. action = { label, run } adds a button
        // (Undo), which Ctrl+Z also presses while the toast shows it.
        let noteTimer = 0;
        let noteAction = null;
        function showNote(text, isError, sticky, action) {
            clearTimeout(noteTimer);
            noteAction = text && action ? action : null;
            note.hidden = !text;
            note.textContent = "";
            note.className = "ac-note ac-toast" + (isError ? " ac-error" : "");
            if (!text) return;
            const msg = document.createElement("span");
            msg.textContent = text;
            note.appendChild(msg);
            if (noteAction) {
                const b = document.createElement("button");
                b.type = "button";
                b.className = "ac-btn ac-btn-sm";
                b.textContent = noteAction.label;
                b.title = noteAction.label + " (Ctrl+Z)";
                b.addEventListener("click", runNoteAction);
                note.appendChild(b);
            }
            if (!isError && !sticky) noteTimer = setTimeout(() => showNote(""), 8000);
        }
        function runNoteAction() {
            const act = noteAction;
            if (!act) return;
            showNote("");
            act.run();
        }

        // Updates the one table row of this option in place, so the switch the user pressed keeps focus
        function patchRow(id, isActive) {
            const tr = Array.prototype.find.call(body.rows, (t) => t.dataset.id === id);
            if (!tr) return;
            tr.classList.toggle("ac-off", !isActive);
            const input = tr.querySelector(".ac-switch input");
            if (input) input.checked = isActive;
            const lbl = tr.querySelector(".ac-switch-label");
            if (lbl) {
                lbl.className = "ac-switch-label " + (isActive ? "ac-on" : "ac-inactive");
                lbl.textContent = isActive ? "Active" : "Inactive";
            }
        }

        // A switch stays focusable while it saves: a busy flag ignores further presses instead of disabling it,
        // which would drop keyboard focus
        function save(row, wanted, input) {
            input.dataset.busy = "1";
            input.setAttribute("aria-busy", "true");
            const done = () => { delete input.dataset.busy; input.removeAttribute("aria-busy"); };
            showNote("Saving...", false, true);
            return fetch(buildActiveUrl(apiBase, row.id, appName), {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(buildActiveBody(userName, wanted, access))
            }).then((res) => {
                if (!res.ok) return res.text().then((t) => { throw { status: res.status, text: t }; });
                // Only the Active flag changed: the row stays on its page, so the cached pages are patched instead of fetched again
                page = Object.assign({}, page, { rows: setActive(page.rows, row.id, wanted) });
                cache.forEach((p, key) => {
                    if (p.rows.some((r) => r.id === row.id)) cache.set(key, Object.assign({}, p, { rows: setActive(p.rows, row.id, wanted) }));
                });
                // Under the Inactive filter a switch changes which options match and the counts: the next visit fetches again.
                // The row itself stays on screen until then, so Undo is still in reach.
                if (showing === "inactive") cache.clear();
                // The palette drops this user's local copy and reloads its commands
                const shell = window.top || window.parent;
                try { if (shell && typeof shell.axiReloadCommandConfig === "function") shell.axiReloadCommandConfig(); } catch (e) { }
                done();
                patchRow(row.id, wanted);
                if (editing && editing.id === row.id) $("fActive").checked = wanted;
                // Switching off hides the option for everyone, so it can be undone straight away
                showNote(switchMessage(row.name, wanted), false, false,
                    wanted ? null : { label: "Undo", run: () => undoSwitchOff(row) });
            }).catch((err) => {
                done();
                input.checked = !wanted;
                showNote(describeFailure(err && err.status, err && err.text), true);
            });
        }

        // Switches the option back on through its own table switch, or the drawer's when the row is on another page
        function undoSwitchOff(row) {
            const tr = Array.prototype.find.call(body.rows, (t) => t.dataset.id === row.id);
            const input = tr ? tr.querySelector(".ac-switch input") : (editing && editing.id === row.id ? $("fActive") : document.createElement("input"));
            input.checked = true;
            return save(row, true, input);
        }

        // Change handler for every Active switch: a press while the last save is still running is undone
        function onSwitch(row, input) {
            if (input.dataset.busy) { input.checked = !input.checked; return Promise.resolve(); }
            return save(row, input.checked, input);
        }

        function updateCounts() {
            $("acCountConfigure").textContent = page.counts.configure;
            $("acCountSdk").textContent = page.counts.sdk;
            // The counts change the tab widths, so the highlight has to follow
            movePill(false);
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
            // A field with an error must be visible, so a collapsed Advanced section opens
            const adv = text && inputOf[field].closest("details");
            if (adv) adv.open = true;
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
                    : (Logic.isEditable(editing) ? "You need Configure Access and build access to change this option."
                        : "A Built-in Option ships with the product, so its route is read-only. You can still switch it on or off."));
            $("fCommand").disabled = !!editing;
            $("acAdvanced").open = false;
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
                // Advanced opens by itself when the option already uses one of its fields
                $("acAdvanced").open = !!(editing.paramField || editing.extraParams || Logic.sourceName(editing.source));
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
            // Unhide first, then add .ac-open on the next frame so the slide-in transition runs (see axiconfig.css)
            clearTimeout(hideTimer);
            scrim.hidden = false;
            drawer.hidden = false;
            void drawer.offsetWidth;
            scrim.classList.add("ac-open");
            drawer.classList.add("ac-open");
            saveBtn.disabled = false;
            (editing && !isEditable(editing) ? $("fActive") : inputOf.name).focus({ preventScroll: true });
        }

        let opener = null;
        let hideTimer = 0;
        function closeDrawer() {
            if (!drawer.classList.contains("ac-open")) return;
            drawer.classList.remove("ac-open");
            scrim.classList.remove("ac-open");
            editing = null;
            (opener && document.contains(opener) ? opener : addBtn).focus();
            // Hide once the slide-out has finished; the timer also covers reduced motion, where no transition runs
            hideTimer = setTimeout(() => {
                drawer.hidden = true;
                scrim.hidden = true;
            }, 320);
        }

        // True when this user may change the route of the row in the drawer: Configure Access and a Custom Option
        const isEditable = (row) => hasAccess && Logic.isEditable(row);

        function existingNames(command) {
            // Only the names on this page; the API checks the whole list and answers 409 for a duplicate
            return command.toLowerCase() === active ? page.rows.map((r) => r.name) : [];
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
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(Logic.buildUpdateBody(userName, access, checked.value))
                })
                : fetch(Logic.buildCreateUrl(apiBase, appName), {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(Logic.buildCreateBody(userName, access, checked.value))
                });
            request.then((res) => {
                if (!res.ok) return res.text().then((t) => { throw { status: res.status, text: t }; });
                return res.json();
            }).then((created) => {
                const key = checked.value.command.toLowerCase();
                const shell = window.top || window.parent;
                try { if (shell && typeof shell.axiReloadCommandConfig === "function") shell.axiReloadCommandConfig(); } catch (err) { }
                closeDrawer();
                // A new or renamed option can move to another page and changes the counts: fetch again
                cache.clear();
                // Show the saved option: the filter goes back to All (it may hide the option) and the API answers with its page
                const savedId = target ? target.id : String((created && (created.configId || created.ConfigId)) || "");
                setShowing("all");
                if (key === active) load(active, pageNoOf[active], null, savedId); else selectTab(key, savedId);
                showNote((target ? "Saved \"" : "Added \"") + checked.value.name + "\" for everyone in this app. " + OTHER_USERS_NOTE, false);
            }).catch((err) => {
                saveBtn.disabled = false;
                formError.className = "ac-note ac-error";
                formError.hidden = false;
                formError.textContent = describeFailure(err && err.status, err && err.text);
            });
        }

        if (hasAccess) addBtn.hidden = false;
        // Without both accesses the switches are disabled: say why instead of leaving them silently greyed out
        else $("acAccessNote").hidden = false;
        addBtn.addEventListener("click", () => { opener = addBtn; openDrawer(null); });
        $("fType").addEventListener("change", showFieldsForType);
        inputOf.name.addEventListener("input", updateRenameWarning);
        // The Active switch in the drawer saves on its own, like the one in the table
        $("fActive").addEventListener("change", () => {
            const row = editing;
            if (!row) return;
            onSwitch(row, $("fActive")).then(() => {
                if (editing === row) editing = page.rows.filter((r) => r.id === row.id)[0] || row;
            });
        });
        inputOf.promptId.addEventListener("input", searchStructures);
        $("acClose").addEventListener("click", closeDrawer);
        $("acCancel").addEventListener("click", closeDrawer);
        scrim.addEventListener("click", closeDrawer);
        form.addEventListener("submit", submitDrawer);
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape" && !drawer.hidden) { closeDrawer(); return; }
            // Ctrl+Z presses the toast's Undo, except inside a text field, where it keeps undoing typing
            if (noteAction && (e.ctrlKey || e.metaKey) && !e.shiftKey && (e.key === "z" || e.key === "Z")
                && !(e.target.matches && e.target.matches("input:not([type=checkbox]), textarea, select"))) {
                e.preventDefault();
                runNoteAction();
                return;
            }
            // Keep Tab inside the open drawer
            if (e.key === "Tab" && !drawer.hidden) {
                // Fields inside a collapsed Advanced section cannot take focus; its summary can
                const items = Array.prototype.filter.call(drawer.querySelectorAll("input, select, button, summary"), (el) => !el.disabled && !el.closest("[hidden]")
                    && !(el.tagName !== "SUMMARY" && el.closest("details:not([open])")));
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

        // swapFrom: on a tab switch, "next" or "prev" makes the rows slide in from that side (see axiconfig.css)
        function render(swapFrom) {
            const rows = page.rows;
            body.textContent = "";
            body.classList.remove("ac-swap-next", "ac-swap-prev");
            if (swapFrom) {
                void body.offsetWidth;
                body.classList.add("ac-swap-" + swapFrom);
            }
            rows.forEach((r) => {
                const tr = document.createElement("tr");
                tr.dataset.id = r.id;
                if (!r.active) tr.className = "ac-off";
                // The option as the user types it in the palette: command, option name, then its record source as the third word
                const nameTd = document.createElement("td");
                nameTd.className = "ac-name";
                const words = [["ac-cmd", r.command.toLowerCase()], ["ac-optname", r.name]];
                const src = Logic.sourceName(r.source);
                if (src) words.push(["ac-src", "‹" + src + "›"]);
                // One isolated run, so in a right-to-left page the phrase keeps its own word order and brackets
                const phrase = document.createElement("span");
                phrase.className = "ac-phrase";
                words.forEach(([cls, text], n) => {
                    if (n) phrase.appendChild(document.createTextNode(" "));
                    const w = document.createElement("span");
                    w.className = cls;
                    w.textContent = text;
                    phrase.appendChild(w);
                });
                if (src) phrase.lastChild.title = "Record source: " + src + " (" + r.source + ")";
                nameTd.appendChild(phrase);
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
                input.addEventListener("change", () => onSwitch(r, input));
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
            count.textContent = rangeText(page.pageNo, page.pageSize, page.total);
            status.hidden = rows.length > 0;
            status.textContent = emptyText();
            renderPager();
        }

        function emptyText() {
            const term = search.value.trim();
            if (term) return "Nothing matches \"" + term + "\"" + (showing === "all" ? "" : " in " + showing + " options") + ". Try part of a name or target.";
            if (showing === "custom") return "No Custom Options yet. Add one with Add option.";
            if (showing === "inactive") return "Every option is switched on.";
            return "No options found.";
        }

        // Briefly marks the row of an option that was just saved, and brings it into view
        function flashRow(id) {
            const tr = Array.prototype.find.call(body.rows, (t) => t.dataset.id === id);
            if (!tr) return;
            tr.classList.add("ac-flash");
            tr.scrollIntoView({ block: "nearest" });
            // Fade the tint out, then drop the fade so ordinary hovers stay instant
            setTimeout(() => { tr.classList.add("ac-flash-out"); tr.classList.remove("ac-flash"); }, 2400);
            setTimeout(() => tr.classList.remove("ac-flash-out"), 3300);
        }

        // ---- Pager ----
        const pager = $("acPager"), pageNums = $("acPageNums"), prevBtn = $("acPrev"), nextBtn = $("acNext");

        // "1-25 of 61" sits at the start of the pager under the table; the page buttons only appear when there is more than one page
        function renderPager() {
            pager.hidden = page.total === 0;
            $("acPages").hidden = page.totalPages <= 1;
            prevBtn.disabled = page.pageNo <= 1;
            nextBtn.disabled = page.pageNo >= page.totalPages;
            pageNums.textContent = "";
            pageNumbers(page.pageNo, page.totalPages).forEach((p) => {
                if (p === "...") {
                    const gap = document.createElement("span");
                    gap.className = "ac-gap";
                    gap.textContent = "...";
                    gap.setAttribute("aria-hidden", "true");
                    pageNums.appendChild(gap);
                    return;
                }
                const b = document.createElement("button");
                b.type = "button";
                b.className = "ac-btn ac-btn-sm ac-pnum";
                b.textContent = p;
                b.setAttribute("aria-label", "Page " + p);
                if (p === page.pageNo) b.setAttribute("aria-current", "page");
                b.addEventListener("click", () => goToPage(p));
                pageNums.appendChild(b);
            });
        }

        function goToPage(no) {
            if (no < 1 || no > page.totalPages || no === page.pageNo) return;
            load(active, no).then(() => {
                // Back to the first row when the pager at the bottom was used
                if ($("acTable").getBoundingClientRect().top < 0) $("acTable").scrollIntoView({ block: "start" });
            });
        }

        prevBtn.addEventListener("click", () => goToPage(page.pageNo - 1));
        nextBtn.addEventListener("click", () => goToPage(page.pageNo + 1));

        /** Shows page no of tab for the current search and filter: from the cache when it was fetched before, else from the API.
            focus (a config_id, after a save) asks the API for the page holding that option, then marks its row. */
        function load(tab, no, swapFrom, focus) {
            const term = search.value.trim();
            const ask = ++loadAsk;
            const keyOf = (n) => tab + "|" + showing + "|" + term + "|" + n;
            const show = (p) => {
                // A newer request (another page, tab, search or filter) wins
                if (ask !== loadAsk || tab !== active) return;
                page = p;
                pageNoOf[tab] = p.pageNo;
                body.removeAttribute("aria-busy");
                updateCounts();
                render(swapFrom);
                if (focus) flashRow(focus);
            };
            const cached = focus ? null : cache.get(keyOf(no));
            if (cached) {
                show(cached);
                return Promise.resolve();
            }
            body.setAttribute("aria-busy", "true");
            if (!page.rows.length) { status.hidden = false; status.textContent = "Loading options..."; }
            return fetch(buildPageUrl(apiBase, appName, tab, term, no, showing, focus))
                .then((res) => { if (!res.ok) throw new Error("HTTP " + res.status); return res.json(); })
                .then((raw) => {
                    const p = normalizePage(raw);
                    // Stored under the page the API answered with (a page past the end comes back as the last page)
                    cache.set(keyOf(p.pageNo), p);
                    show(p);
                })
                .catch((err) => {
                    if (ask !== loadAsk) return;
                    body.removeAttribute("aria-busy");
                    status.hidden = false;
                    status.textContent = "Could not load the options (" + err.message + "). Check the connection, then try again. ";
                    const retry = document.createElement("button");
                    retry.type = "button";
                    retry.className = "ac-btn ac-btn-sm";
                    retry.textContent = "Try again";
                    retry.addEventListener("click", () => load(tab, no, swapFrom, focus));
                    status.appendChild(retry);
                });
        }

        // Sliding highlight behind the selected tab: one pill that moves and resizes instead of each tab repainting
        const tabList = tabs.length ? tabs[0].parentNode : null;
        const pill = document.createElement("span");
        pill.className = "ac-tab-pill";
        pill.setAttribute("aria-hidden", "true");
        if (tabList) {
            tabList.insertBefore(pill, tabList.firstChild);
            tabList.classList.add("ac-has-pill");
        }
        function movePill(animate) {
            const sel = tabs.find((t) => t.dataset.tab === active);
            if (!sel) return;
            if (!animate) pill.classList.add("ac-no-anim");
            pill.style.width = sel.offsetWidth + "px";
            pill.style.transform = "translateX(" + sel.offsetLeft + "px)";
            if (!animate) {
                void pill.offsetWidth;
                pill.classList.remove("ac-no-anim");
            }
        }
        window.addEventListener("resize", () => movePill(false));

        function selectTab(name, focus) {
            // Same tab: just reload its page, no slide
            if (name === active) {
                movePill(false);
                load(active, pageNoOf[active], null, focus);
                return;
            }
            const from = tabs.findIndex((t) => t.dataset.tab === active);
            const to = tabs.findIndex((t) => t.dataset.tab === name);
            active = name;
            tabs.forEach((t) => {
                const on = t.dataset.tab === name;
                t.setAttribute("aria-selected", on ? "true" : "false");
                t.tabIndex = on ? 0 : -1;
            });
            $("acPanel").setAttribute("aria-labelledby", "tab-" + name);
            movePill(true);
            load(name, pageNoOf[name], to > from ? "next" : "prev", focus);
        }

        // All / Custom / Inactive: pressing one starts both tabs again from page 1
        const filterBtns = Array.prototype.slice.call(document.querySelectorAll("#acFilter [data-show]"));
        function setShowing(value) {
            showing = value;
            filterBtns.forEach((b) => b.setAttribute("aria-pressed", b.dataset.show === value ? "true" : "false"));
        }
        filterBtns.forEach((b) => b.addEventListener("click", () => {
            if (b.dataset.show === showing) return;
            setShowing(b.dataset.show);
            pageNoOf.configure = 1;
            pageNoOf.sdk = 1;
            load(active, 1);
        }));

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
        // The search runs on the server: wait for a pause in typing, then start both tabs again from page 1
        let searchTimer = null;
        search.addEventListener("input", () => {
            clearTimeout(searchTimer);
            searchTimer = setTimeout(() => {
                pageNoOf.configure = 1;
                pageNoOf.sdk = 1;
                load(active, 1);
            }, 300);
        });

        if (!apiBase || !appName) {
            status.hidden = false;
            status.textContent = "This page needs to be opened from the AxiCMD palette.";
            return;
        }
        load(active, 1);
        search.focus();
    }

    return { replaceRow, OTHER_USERS_NOTE, switchMessage, PAGE_SIZE, buildActiveUrl, buildActiveBody, readAccess, setActive, describeFailure, normalizeRow, buildPageUrl, normalizePage, pageNumbers, rangeText, directionFor, init };
}));
