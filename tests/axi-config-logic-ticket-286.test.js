// Ticket #286: Custom Options of type tstruct, iview and ivtoivload, structure picker and record-source picker.
// Loads the REAL logic script. Expected values are literals from the spec / install scripts, not recomputed.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const root = path.join(__dirname, '..');
const logic = require(path.join(root, 'AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfiglogic.js'));
const read = (...p) => fs.readFileSync(path.join(root, ...p), 'utf8');

// ---- types offered ------------------------------------------------------------------------------------------
test('the drawer offers url, tstruct, iview and ivtoivload, never action, processflow or shellaction', () => {
    assert.deepStrictEqual(logic.CUSTOM_TYPES, ['tstruct', 'iview', 'ivtoivload', 'url']);
    for (const t of ['action', 'processflow', 'shellaction']) {
        assert.strictEqual(logic.validateOption({ command: 'Configure', type: t, name: 'x', promptId: 'a' }, []).ok, false, t);
    }
});

test('fields per type: tstruct structure + key field, iview iview + URL, ivtoivload iview + param + extra', () => {
    assert.deepStrictEqual(logic.fieldsFor('tstruct'), ['promptId', 'paramField']);
    assert.deepStrictEqual(logic.fieldsFor('iview'), ['promptId', 'targetUrl']);
    assert.deepStrictEqual(logic.fieldsFor('ivtoivload'), ['promptId', 'paramField', 'extraParams']);
    assert.deepStrictEqual(logic.fieldsFor('url'), ['targetUrl', 'paramField']);
});

test('a tstruct option keeps only its fields and the chosen record source', () => {
    const r = logic.validateOption({ command: 'SDK', type: 'tstruct', name: 'Orders', promptId: 'sorder', paramField: 'orderno', targetUrl: 'ignored', extraParams: 'ignored', recordSource: 'axi_getstructsdata' }, []);
    assert.strictEqual(r.ok, true);
    assert.deepStrictEqual(r.value, { command: 'SDK', name: 'Orders', type: 'tstruct', promptId: 'sorder', targetUrl: '', paramField: 'orderno', extraParams: '', recordSource: 'axi_getstructsdata' });
});

test('an ivtoivload option needs the iview and keeps extra params', () => {
    const bad = logic.validateOption({ command: 'Configure', type: 'ivtoivload', name: 'Dim', paramField: 'prole' }, []);
    assert.strictEqual(bad.ok, false);
    assert.ok(bad.errors.promptId);
    const ok = logic.validateOption({ command: 'Configure', type: 'ivtoivload', name: 'Dim', promptId: 'ad___upg', paramField: 'prole', extraParams: 'AxOpenAct=true' }, []);
    assert.strictEqual(ok.value.extraParams, 'AxOpenAct=true');
    assert.strictEqual(ok.value.recordSource, 'Axi_Dummy');
});

// ---- record-source picker -----------------------------------------------------------------------------------
test('record sources start with None (Axi_Dummy) and then the known Configure/SDK sources, once each', () => {
    const opts = logic.recordSourceOptions();
    assert.deepStrictEqual(opts[0], { value: 'Axi_Dummy', label: 'None' });
    const values = opts.map((o) => o.value);
    for (const s of ['axi_structmetalist', 'axi_userlist', 'axi_rolelist', 'Axi_PegList', 'axi_jobs', 'axi_inbound']) {
        assert.ok(values.includes(s), s);
    }
    assert.strictEqual(new Set(values.map((v) => v.toLowerCase())).size, values.length);
    assert.strictEqual(values.filter((v) => v === 'Axi_Dummy').length, 1);
});

test('every known record source has a readable name; the picker shows it with the id, the stored value stays the id', () => {
    for (const s of logic.KNOWN_RECORD_SOURCES) assert.notStrictEqual(logic.sourceName(s), s, s + ' has no readable name');
    assert.strictEqual(logic.sourceName('axi_userlist'), 'Users');
    assert.strictEqual(logic.sourceName('AXI_USERLIST'), 'Users');
    assert.strictEqual(logic.sourceName('axi_resposibilitylist'), 'Responsibilities');
    assert.strictEqual(logic.sourceName('cust_special_ads'), 'cust_special_ads', 'an unknown id is shown as it is');
    assert.strictEqual(logic.sourceName('Axi_Dummy'), '');
    assert.strictEqual(logic.sourceName(''), '');
    assert.deepStrictEqual(logic.recordSourceOptions().find((o) => o.value === 'axi_userlist'), { value: 'axi_userlist', label: 'Users (axi_userlist)' });
});

test('rows read as the typed phrase and the rarely used drawer fields sit in a collapsed Advanced section', () => {
    const page = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfigpage.js');
    assert.match(page, /const words = \[\["ac-cmd", r\.command\.toLowerCase\(\)\], \["ac-optname", r\.name\]\];/);
    assert.match(page, /if \(src\) words\.push\(\["ac-src", "‹" \+ src \+ "›"\]\);/);
    assert.match(page, /\$\("acAdvanced"\)\.open = !!\(editing\.paramField \|\| editing\.extraParams \|\| Logic\.sourceName\(editing\.source\)\);/);
    assert.match(page, /const adv = text && inputOf\[field\]\.closest\("details"\);\s*if \(adv\) adv\.open = true;/);
    assert.match(page, /querySelectorAll\("input, select, button, summary"\)/);
    assert.match(page, /phrase\.className = "ac-phrase";/);
    // Right-to-left: the phrase and the English sentences keep their own direction; the open chevron turns the other way
    const css = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'css', 'axiconfig.css');
    assert.match(css, /\.ac-phrase \{ unicode-bidi: plaintext; \}/);
    assert.match(css, /\.ac-src \{ white-space: nowrap; \}/);
    assert.match(css, /\.ac-sub, \.ac-pageof, \.ac-status, \.ac-note, \.ac-hint, \.ac-help, \.ac-err \{ unicode-bidi: plaintext; \}/);
    assert.match(css, /\[dir="rtl"\] \.ac-advanced\[open\] > summary::before \{ transform: rotate\(-45deg\); \}/);
    const html = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'AxiCMDConfig.html');
    assert.match(html, /<th scope="col">You type<\/th>/);
    const adv = html.slice(html.indexOf('<details id="acAdvanced"'), html.indexOf('</details>'));
    for (const id of ['fParamField', 'fExtraParams', 'fSource']) assert.match(adv, new RegExp('id="' + id + '"'), id + ' inside Advanced');
    for (const id of ['fName', 'fTargetUrl', 'fPromptId']) assert.ok(!adv.includes('id="' + id + '"'), id + ' stays outside Advanced');
});

test('the known sources match the ones the install scripts give Configure and SDK', () => {
    const sql = read('AxpertPlugins', 'AxiCMD', 'Structures', 'Postgre', 'Scripts', 'axi_command_tables.sql');
    const used = new Set();
    for (const m of sql.matchAll(/VALUES\('[^']+'::uuid, (4|7), 3, [^\n]*?, '([^']*(?:axi_|Axi_)[^']*)'/g)) m[2].split(',').forEach((s) => used.add(s.trim()));
    used.delete('Axi_Dummy');
    assert.ok(used.size > 20);
    const known = logic.recordSourceOptions().map((o) => o.value.toLowerCase());
    for (const s of used) assert.ok(known.includes(s.toLowerCase()), s);
});

// ---- structure picker ---------------------------------------------------------------------------------------
const rows = [
    { name: 'sorder', caption: 'Sales Order', stype: 't' },
    { name: 'sinv', caption: 'Sales Invoice', stype: 't' },
    { name: 'slist', caption: 'Sales List', stype: 'i' },
    { name: 'pgmain', caption: 'Main page', stype: 'p' },
    { name: 'dsx', caption: 'A data source', stype: 'ads' },
    { name: 'sorder', caption: 'Sales Order', stype: 't' }
];

test('structure picker keeps tstructs for tstruct and iviews for iview and ivtoivload, without duplicates', () => {
    assert.deepStrictEqual(logic.structureChoices(rows, 'tstruct').map((c) => c.value), ['sorder', 'sinv']);
    assert.deepStrictEqual(logic.structureChoices(rows, 'iview').map((c) => c.value), ['slist']);
    assert.deepStrictEqual(logic.structureChoices(rows, 'ivtoivload').map((c) => c.value), ['slist']);
    assert.deepStrictEqual(logic.structureChoices(rows, 'url'), []);
});

test('structure choices show caption and name and are capped', () => {
    assert.strictEqual(logic.structureChoices(rows, 'tstruct')[0].label, 'Sales Order (sorder)');
    const many = Array.from({ length: 80 }, (_, i) => ({ name: 's' + i, caption: 'S' + i, stype: 't' }));
    assert.strictEqual(logic.structureChoices(many, 'tstruct').length, 50);
});

test('uppercase column names from the server are understood', () => {
    assert.deepStrictEqual(logic.structureChoices([{ NAME: 'AX1', CAPTION: 'One', STYPE: 'T' }], 'tstruct'), [{ value: 'AX1', label: 'One (AX1)' }]);
});

test('a search needs 3 characters, as the palette does', () => {
    assert.strictEqual(logic.canSearchStructures('so'), false);
    assert.strictEqual(logic.canSearchStructures(' sal '), true);
});

// ---- wiring -------------------------------------------------------------------------------------------------
test('the page offers all four types and the palette exposes the structure search', () => {
    const page = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfigpage.js');
    assert.ok(!/AVAILABLE_TYPES = \["url"\]/.test(page));
    assert.match(page, /AVAILABLE_TYPES = Logic\.CUSTOM_TYPES/);
    assert.match(page, /recordSourceOptions\(\)/);
    const js = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axicmdmain.js');
    assert.match(js, /window\.axiSearchStructures = /);
    const html = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'AxiCMDConfig.html');
    assert.ok(!/<select id="fSource" disabled/.test(html));
    assert.match(html, /id="fPromptIdList"/);
});
