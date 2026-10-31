/*
 * AxiCMD Config page logic for Custom Options: name rules, fields per option type, config id, and the
 * option/source list update. No DOM, no network. Loaded by AxiCMDConfig.html before axiconfigpage.js and by
 * tests/axi-config-logic-ticket-285.test.js. The API (AxiApi/Lib/CommandOptionRules.cs) enforces the same rules:
 * change both together.
 */
(function (root, factory) {
    const api = factory();
    if (typeof module === "object" && module.exports) module.exports = api;
    else root.AxiConfigLogic = api;
}(typeof self !== "undefined" ? self : this, function () {
    "use strict";

    const MAX_NAME_LENGTH = 200;
    const MAX_LIST_LENGTH = 4000;      // axi_command_prompts.promptvalues / promptsource
    const MAX_RECORD_SOURCE_LENGTH = 100; // longest record source (data source) name accepted
    const MAX_CONFIG_ID_LENGTH = 50;   // axi_command_config.config_id
    const NO_SOURCE = "Axi_Dummy";     // "None" as a record source
    const COMMANDS = ["Configure", "SDK"];
    const CUSTOM_TYPES = ["tstruct", "iview", "ivtoivload", "url"];

    /** Fields each Custom Option type uses; every other field is blanked. promptId = target form / report. */
    const FIELDS_BY_TYPE = {
        tstruct: ["promptId", "paramField"],
        iview: ["promptId", "targetUrl"],
        ivtoivload: ["promptId", "paramField", "extraParams"],
        url: ["targetUrl", "paramField"]
    };
    const REQUIRED_FIELDS = { tstruct: ["promptId"], iview: ["promptId"], ivtoivload: ["promptId"], url: ["targetUrl"] };
    const FIELD_MAX = { promptId: 50, targetUrl: 500, paramField: 100, extraParams: 500 };

    /** Record sources already used by the Configure and SDK option lists (axi_command_tables.sql), after "None". */
    const KNOWN_RECORD_SOURCES = [
        "Axi_PegList", "Axi_FormNotifyList", "Axi_ScheduleNotifyList", "Axi_PEGNotifyList", "Axi_RuleNamesList",
        "axi_structmetalist", "axi_userlist", "axi_useractivation", "axi_usergrouplist", "axi_rolelist", "axi_actorlist",
        "axi_publishapi", "Axi_ServernameList", "axi_cardlist", "axi_resposibilitylist", "axi_dimensionlist",
        "axi_smartviewlist", "Axi_APINamesList", "axi_jobs", "axi_language", "axi_customtype", "axi_emaildef",
        "axi_tabledesc", "axi_outbound", "axi_inbound", "axi_getstructsdata", "axi_viewlist"
    ];
    const STRUCT_TYPE_CODE = { tstruct: "t", iview: "i", ivtoivload: "i" };
    const MAX_STRUCT_CHOICES = 50;

    /** Readable names of the record sources (Axpert Data Sources); the stored value stays the id. */
    const RECORD_SOURCE_NAMES = {
        axi_peglist: "PEG list", axi_formnotifylist: "Form notifications", axi_schedulenotifylist: "Scheduled notifications",
        axi_pegnotifylist: "PEG notifications", axi_rulenameslist: "Rules", axi_structmetalist: "Forms and reports",
        axi_userlist: "Users", axi_useractivation: "User activation", axi_usergrouplist: "User groups", axi_rolelist: "Roles",
        axi_actorlist: "Actors", axi_publishapi: "Published APIs", axi_servernamelist: "Servers", axi_cardlist: "Cards",
        axi_resposibilitylist: "Responsibilities", axi_dimensionlist: "Dimensions", axi_smartviewlist: "Smart views",
        axi_apinameslist: "APIs", axi_jobs: "Jobs", axi_language: "Languages", axi_customtype: "Custom types",
        axi_emaildef: "Email definitions", axi_tabledesc: "Tables", axi_outbound: "Outbound", axi_inbound: "Inbound",
        axi_getstructsdata: "Records of a form", axi_viewlist: "Views"
    };

    /** The readable name of a record source id; an unknown id is shown as it is, None for no source. */
    function sourceName(id) {
        const s = String(id || "").trim();
        if (!s || s.toLowerCase() === NO_SOURCE.toLowerCase()) return "";
        return RECORD_SOURCE_NAMES[s.toLowerCase()] || s;
    }

    function recordSourceOptions() {
        return [{ value: NO_SOURCE, label: "None" }].concat(KNOWN_RECORD_SOURCES.map((s) => ({ value: s, label: sourceName(s) + " (" + s + ")" })));
    }

    /** Structures (axi_structmetalist rows) the picker offers for an option type: tstruct -> forms, iview/ivtoivload -> reports. */
    function structureChoices(rows, type) {
        const code = STRUCT_TYPE_CODE[type];
        const seen = {};
        const out = [];
        (rows || []).forEach((r) => {
            if (!r || out.length >= MAX_STRUCT_CHOICES) return;
            const name = norm(r.name !== undefined ? r.name : r.NAME);
            const caption = norm(r.caption !== undefined ? r.caption : r.CAPTION);
            const stype = norm(r.stype !== undefined ? r.stype : r.STYPE).toLowerCase();
            if (!code || !name || stype !== code || seen[name.toLowerCase()]) return;
            seen[name.toLowerCase()] = true;
            out.push({ value: name, label: (caption && caption.toLowerCase() !== name.toLowerCase() ? caption : name) + (caption && caption.toLowerCase() !== name.toLowerCase() ? " (" + name + ")" : "") });
        });
        return out;
    }

    function canSearchStructures(term) {
        return norm(term).length >= 3;
    }

    function fieldsFor(type) {
        return (FIELDS_BY_TYPE[type] || []).slice();
    }

    const norm = (s) => String(s === undefined || s === null ? "" : s).trim();

    function validateName(name, existingNames) {
        const n = norm(name);
        if (!n) return { ok: false, code: "required", message: "Enter a name for the option." };
        if (n.indexOf(",") !== -1) return { ok: false, code: "comma", message: "The name cannot contain a comma." };
        if (n.length > MAX_NAME_LENGTH) return { ok: false, code: "length", message: "The name can have at most " + MAX_NAME_LENGTH + " characters (now " + n.length + ")." };
        const lower = n.toLowerCase();
        if ((existingNames || []).some((e) => norm(e).toLowerCase() === lower)) {
            return { ok: false, code: "duplicate", message: "An option named \"" + n + "\" already exists in this command (names are not case sensitive)." };
        }
        return { ok: true, name: n };
    }

    /** FNV-1a over UTF-16 code units: a stable id part for names that have no latin letters or digits. */
    function hashName(text) {
        let h = 0x811c9dc5;
        for (let i = 0; i < text.length; i++) {
            h ^= text.charCodeAt(i);
            h = Math.imul(h, 0x01000193) >>> 0;
        }
        return ("00000000" + h.toString(16)).slice(-8);
    }

    /** usr_<command>_<slug>, at most 50 characters. */
    function makeConfigId(command, name) {
        const prefix = "usr_" + norm(command).toLowerCase() + "_";
        const lower = norm(name).toLowerCase();
        let slug = lower.replace(/[^a-z0-9]+/g, "_").replace(/^_+|_+$/g, "");
        if (!slug) slug = "x" + hashName(lower);
        return (prefix + slug.slice(0, MAX_CONFIG_ID_LENGTH - prefix.length)).replace(/_+$/, "");
    }

    /**
     * A target URL is http(s) or a relative path (iview.aspx?ivname=x, aspx/page.aspx, /x, ../x). Any other scheme
     * (javascript:, data:, ftp:, ...) and control characters (which browsers strip inside a scheme) are refused.
     */
    function validateUrl(url) {
        if (/[\u0000-\u001f\u007f]/.test(url)) return false;
        const scheme = /^([a-z][a-z0-9+.\-]*):/i.exec(url);
        return !scheme || /^https?$/i.test(scheme[1]);
    }

    /** Record source: a plain name of at most MAX_RECORD_SOURCE_LENGTH characters. Returns an error text or "". */
    function recordSourceError(src) {
        if (src.length > MAX_RECORD_SOURCE_LENGTH) return "At most " + MAX_RECORD_SOURCE_LENGTH + " characters.";
        if (!/^[A-Za-z0-9_]+$/.test(src)) return "Use a data source name without commas or spaces.";
        return "";
    }

    /**
     * Checks a whole option from the drawer. Returns { ok, errors } or { ok, value } where value only holds the
     * fields of the chosen type (the rest are blank). existingNames = every option name of the command, built-in too.
     */
    function validateOption(input, existingNames) {
        const errors = {};
        const command = COMMANDS.filter((c) => c.toLowerCase() === norm(input.command).toLowerCase())[0];
        if (!command) errors.command = "Choose Configure or SDK.";
        const type = norm(input.type).toLowerCase();
        if (CUSTOM_TYPES.indexOf(type) === -1) errors.type = "Choose a type: " + CUSTOM_TYPES.join(", ") + ".";

        const nameCheck = validateName(input.name, existingNames);
        if (!nameCheck.ok) errors.name = nameCheck.message;

        const value = { command: command || "", name: nameCheck.name || norm(input.name), type: type, promptId: "", targetUrl: "", paramField: "", extraParams: "", recordSource: NO_SOURCE };
        const used = fieldsFor(type);
        used.forEach((f) => {
            const v = norm(input[f]);
            value[f] = v;
            if (!v && (REQUIRED_FIELDS[type] || []).indexOf(f) !== -1) errors[f] = "This field is required.";
            else if (v && v.length > FIELD_MAX[f]) errors[f] = "At most " + FIELD_MAX[f] + " characters.";
            else if (v && f === "targetUrl" && !validateUrl(v)) errors[f] = "Use an http(s) address or a relative path such as aspx/page.aspx.";
            else if (v && f === "paramField" && !/^[A-Za-z0-9_]+$/.test(v)) errors[f] = "Letters, digits and underscore only.";
            else if (v && f === "promptId" && !/^[A-Za-z0-9_.\/-]+$/.test(v)) errors[f] = "Letters, digits, underscore, dot, dash and slash only.";
        });

        if (input.recordSource !== undefined) {
            const src = norm(input.recordSource);
            if (src && recordSourceError(src)) errors.recordSource = recordSourceError(src);
            else if (src) value.recordSource = src;
        }
        return Object.keys(errors).length ? { ok: false, errors: errors } : { ok: true, value: value };
    }

    function splitList(list) {
        // Entries are trimmed. Trailing empty entries (stray commas, spaces) carry no position and are dropped;
        // an empty entry in the middle keeps its place so the two lists stay paired.
        const text = norm(list);
        if (!text) return [];
        const parts = text.split(",").map((x) => x.trim());
        while (parts.length && !parts[parts.length - 1]) parts.pop();
        return parts;
    }

    /**
     * Appends one option to the option list and its record source to the paired source list. The lists are
     * comma separated and matched by position, so a short source list is padded first.
     */
    function appendOption(names, sources, name, recordSource) {
        const src = norm(recordSource) || NO_SOURCE;
        if (recordSourceError(src)) return { ok: false, code: "source", message: "The record source name is not valid." };
        const n = splitList(names), s = splitList(sources);
        while (s.length < n.length) s.push(NO_SOURCE);
        n.push(norm(name));
        s.push(src);
        const outNames = n.join(","), outSources = s.join(",");
        if (outNames.length > MAX_LIST_LENGTH || outSources.length > MAX_LIST_LENGTH) {
            return { ok: false, code: "listfull", message: "The option list is full (" + MAX_LIST_LENGTH + " characters). Remove an option before adding another." };
        }
        return { ok: true, names: outNames, sources: outSources };
    }

    /**
     * Rename / re-source one entry in place: finds oldName (case insensitive) in the option list, puts newName there and
     * recordSource at the same position of the source list. Both lists stay paired and keep their order.
     */
    function replaceOption(names, sources, oldName, newName, recordSource) {
        const src = norm(recordSource) || NO_SOURCE;
        if (recordSourceError(src)) return { ok: false, code: "source", message: "The record source name is not valid." };
        const n = splitList(names), s = splitList(sources);
        const at = n.findIndex((x) => norm(x).toLowerCase() === norm(oldName).toLowerCase());
        if (at === -1) return { ok: false, code: "missing", message: "The option \"" + norm(oldName) + "\" is not in the command's option list." };
        while (s.length < n.length) s.push(NO_SOURCE);
        n[at] = norm(newName);
        s[at] = src;
        const outNames = n.join(","), outSources = s.join(",");
        if (outNames.length > MAX_LIST_LENGTH || outSources.length > MAX_LIST_LENGTH) {
            return { ok: false, code: "listfull", message: "The option list is full (" + MAX_LIST_LENGTH + " characters). Shorten the name or remove an option." };
        }
        return { ok: true, names: outNames, sources: outSources };
    }

    /** Edit mode: the same rules as a new option, but the option's own current name does not count as a duplicate and the command stays. */
    function validateEdit(input, row, existingNames) {
        const own = norm(row.name).toLowerCase();
        const others = (existingNames || []).filter((e) => norm(e).toLowerCase() !== own);
        return validateOption(Object.assign({}, input, { command: row.command }), others);
    }

    /** Only Custom Options can be edited; a Built-in Option keeps its route (only Active can change). */
    function isEditable(row) {
        return !!row && !row.builtIn;
    }

    function renameWarning(oldName, newName) {
        const o = norm(oldName), n = norm(newName);
        if (!o || !n || o === n) return "";
        return "Renaming \"" + o + "\" to \"" + n + "\": saved favourites that use \"" + o + "\" will stop working.";
    }

    function buildUpdateUrl(baseUrl, configId, appname) {
        return baseUrl.replace(/\/+$/, "") + "/options/" + encodeURIComponent(configId) + "?appname=" + encodeURIComponent(appname);
    }

    /** access is { appMgrAccess, buildAccess }; the API refuses the write unless both are true. */
    function buildUpdateBody(username, access, value) {
        return {
            username: username, appMgrAccess: !!(access && access.appMgrAccess), buildAccess: !!(access && access.buildAccess),
            name: value.name, type: value.type,
            promptId: value.promptId, targetUrl: value.targetUrl, paramField: value.paramField, extraParams: value.extraParams,
            recordSource: value.recordSource
        };
    }

    function buildCreateUrl(baseUrl, appname) {
        return baseUrl.replace(/\/+$/, "") + "/options?appname=" + encodeURIComponent(appname);
    }

    function buildCreateBody(username, access, value) {
        return {
            username: username, appMgrAccess: !!(access && access.appMgrAccess), buildAccess: !!(access && access.buildAccess),
            command: value.command, name: value.name, type: value.type,
            promptId: value.promptId, targetUrl: value.targetUrl, paramField: value.paramField, extraParams: value.extraParams,
            recordSource: value.recordSource
        };
    }

    return {
        MAX_NAME_LENGTH, MAX_LIST_LENGTH, MAX_RECORD_SOURCE_LENGTH, NO_SOURCE, COMMANDS, CUSTOM_TYPES, FIELDS_BY_TYPE,
        fieldsFor, recordSourceOptions, sourceName, structureChoices, canSearchStructures, KNOWN_RECORD_SOURCES, validateName, makeConfigId, validateOption, appendOption, replaceOption, validateEdit, isEditable, renameWarning, buildUpdateUrl, buildUpdateBody, buildCreateUrl, buildCreateBody
    };
}));
