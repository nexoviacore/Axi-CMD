// Configure Product / Plugin / ChangeRequest (prompt_option_type 'commandsetting').
// Like the other palette tests, this re-implements the pure helpers from axicmdmain.js with mock data,
// so keep the copies below identical to the helpers in the script.
const test = require('node:test');
const assert = require('node:assert');

// ---- mirrors of the helpers in axicmdmain.js ----------------------------------------------------------------
function cleanCommandToken(val = "") {
    return val.replace(/["]/g, "").trim();
}

function validateCommandSettingName(raw) {
    const value = cleanCommandToken(raw == null ? "" : String(raw));
    if (!value) return { ok: false, value: "", message: "Name is required" };
    if (value.length > 100) return { ok: false, value, message: "Name must be 100 characters or fewer" };
    if (!/^[A-Za-z0-9._-]+$/.test(value)) return { ok: false, value, message: "Name can contain only letters, numbers, dot, underscore and hyphen" };
    return { ok: true, value, message: "" };
}

function isCommandSettingRow(row) {
    if (!row) return false;
    return (row.promptOptionType || row.prompt_option_type || row.PromptOptionType || "").toLowerCase() === "commandsetting";
}

function findConfigureRow(list, tokenName) {
    const opt = cleanCommandToken(tokenName || "").toLowerCase();
    if (!opt) return null;
    return list.find(r => (r.command || "").toLowerCase() === "configure"
        && (r.promptOptions || r.prompt_options || "").trim().toLowerCase() === opt) || null;
}

function isCommandSettingOption(list, tokenName) {
    return isCommandSettingRow(findConfigureRow(list, tokenName));
}

function hasCommandSettingExtraWords(tokens) {
    return tokens.slice(4).some(t => cleanCommandToken(t || "").trim() !== "");
}

function shouldOfferCommandSettingGo(tokens) {
    return tokens.length > 3 && !hasCommandSettingExtraWords(tokens)
        && validateCommandSettingName(tokens[2]).ok && validateCommandSettingName(tokens[3]).ok;
}

function getCommandSettingLabel(type) {
    return ({ product: "Product", plugin: "Plugin", changerequest: "Change Request" })[(type || "").toLowerCase()] || type;
}

// ---- tests --------------------------------------------------------------------------------------------------
const rows = [
    { command: "Configure", promptOptions: "Product", promptOptionType: "commandsetting", promptId: "product" },
    { command: "Configure", promptOptions: "ChangeRequest", prompt_option_type: "CommandSetting", promptId: "changerequest" },
    { command: "Configure", promptOptions: "Keyfield", promptOptionType: "action" },
    { command: "Configure", promptOptions: "Tstruct", promptOptionType: "navigation" },
    { command: "SDK", promptOptions: "Plugin", promptOptionType: "navigation" },
];

test('valid names are accepted and trimmed/unquoted', () => {
    assert.deepStrictEqual(validateCommandSettingName('  nexoviacore '), { ok: true, value: 'nexoviacore', message: '' });
    assert.strictEqual(validateCommandSettingName('"axpert.web_v2-x"').ok, true);
});

test('empty, too long, or illegal characters are refused', () => {
    assert.strictEqual(validateCommandSettingName('').ok, false);
    assert.strictEqual(validateCommandSettingName('   ').ok, false);
    assert.strictEqual(validateCommandSettingName(undefined).ok, false);
    assert.strictEqual(validateCommandSettingName('a'.repeat(100)).ok, true);
    assert.strictEqual(validateCommandSettingName('a'.repeat(101)).ok, false);
    assert.strictEqual(validateCommandSettingName('old/repo').ok, false);
    assert.strictEqual(validateCommandSettingName('a b').ok, false);
    assert.strictEqual(validateCommandSettingName('a;b').ok, false);
});

test('isCommandSettingOption looks up the configure row type, case-insensitively', () => {
    assert.strictEqual(isCommandSettingOption(rows, 'product'), true);
    assert.strictEqual(isCommandSettingOption(rows, 'ChangeRequest'), true);
    assert.strictEqual(isCommandSettingOption(rows, 'keyfield'), false);
    assert.strictEqual(isCommandSettingOption(rows, 'tstruct'), false);
    assert.strictEqual(isCommandSettingOption(rows, 'plugin'), false); // only an SDK row exists
    assert.strictEqual(isCommandSettingOption(rows, ''), false);
    assert.strictEqual(isCommandSettingOption([], 'product'), false);
});

test('Go is offered only when account and repo names are both present and valid', () => {
    assert.strictEqual(shouldOfferCommandSettingGo(['configure', 'product']), false);
    assert.strictEqual(shouldOfferCommandSettingGo(['configure', 'product', 'nexoviacore']), false);
    assert.strictEqual(shouldOfferCommandSettingGo(['configure', 'product', 'nexoviacore', '']), false);
    assert.strictEqual(shouldOfferCommandSettingGo(['configure', 'product', 'nexoviacore', 'bad/repo']), false);
    assert.strictEqual(shouldOfferCommandSettingGo(['configure', 'product', 'nexoviacore', 'axpertweb']), true);
    assert.strictEqual(shouldOfferCommandSettingGo(['configure', 'product', 'nexoviacore', 'axpertweb', '']), true);
    assert.strictEqual(shouldOfferCommandSettingGo(['configure', 'product', 'my', 'account', 'repo']), false);
});

test('labels are human readable', () => {
    assert.strictEqual(getCommandSettingLabel('changerequest'), 'Change Request');
    assert.strictEqual(getCommandSettingLabel('product'), 'Product');
    assert.strictEqual(getCommandSettingLabel('plugin'), 'Plugin');
});
