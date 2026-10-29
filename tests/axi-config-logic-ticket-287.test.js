// Ticket #287: edit and rename Custom Options, lock Built-in Option routes.
// Loads the REAL logic and page scripts. Expected values are literals, not recomputed.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const root = path.join(__dirname, '..');
const html = path.join(root, 'AxpertPlugins', 'AxiCMD', 'HTMLPages');
const logic = require(path.join(html, 'js', 'axiconfiglogic.js'));
const page = require(path.join(html, 'js', 'axiconfigpage.js'));
const read = (...p) => fs.readFileSync(path.join(root, ...p), 'utf8');

const CUSTOM = { id: 'usr_configure_orders', command: 'Configure', name: 'Orders', type: 'tstruct', promptId: 'sorder', targetUrl: '', paramField: 'orderno', extraParams: '', source: 'axi_userlist', builtIn: false };
const BUILTIN = { id: 'configure_user', command: 'Configure', name: 'User', type: 'tstruct', promptId: 'axusr', targetUrl: '', paramField: '', extraParams: '', source: 'axi_userlist', builtIn: true };

// ---- rename keeps position and pairing -----------------------------------------------------------------------
test('a rename replaces the entry in place in both lists', () => {
    const r = logic.replaceOption('A,Orders,C', 'sa,sb,sc', 'Orders', 'Sales orders', 'sb');
    assert.deepStrictEqual(r, { ok: true, names: 'A,Sales orders,C', sources: 'sa,sb,sc' });
});

test('a record source change updates only the paired source', () => {
    const r = logic.replaceOption('A,Orders,C', 'sa,sb,sc', 'Orders', 'Orders', 'axi_rolelist');
    assert.deepStrictEqual(r, { ok: true, names: 'A,Orders,C', sources: 'sa,axi_rolelist,sc' });
});

test('the old name is found ignoring case and a short source list is padded', () => {
    const r = logic.replaceOption('A,Orders,C', 'sa', 'orders', 'Orders 2', '');
    assert.deepStrictEqual(r, { ok: true, names: 'A,Orders 2,C', sources: 'sa,Axi_Dummy,Axi_Dummy' });
});

test('replace refuses a missing entry, a bad source and an overlong list', () => {
    assert.strictEqual(logic.replaceOption('A,B', 'a,b', 'Zed', 'Y', 'x').code, 'missing');
    assert.strictEqual(logic.replaceOption('A,B', 'a,b', 'B', 'Y', 'bad source').code, 'source');
    assert.strictEqual(logic.replaceOption('A,B', 'a,b', 'B', 'x'.repeat(4000), 'x').code, 'listfull');
});

// ---- name uniqueness ignores the option's own name -----------------------------------------------------------
test('editing keeps the option\'s own name valid but still rejects another option\'s name', () => {
    const names = ['User', 'Orders'];
    const same = logic.validateEdit({ type: 'tstruct', name: 'orders', promptId: 'sorder' }, CUSTOM, names);
    assert.strictEqual(same.ok, true);
    assert.strictEqual(same.value.command, 'Configure');
    const clash = logic.validateEdit({ type: 'tstruct', name: 'user', promptId: 'sorder' }, CUSTOM, names);
    assert.strictEqual(clash.ok, false);
    assert.ok(clash.errors.name);
});

test('an edited route keeps only the fields of its type', () => {
    const r = logic.validateEdit({ type: 'url', name: 'Orders', targetUrl: '../aspx/o.aspx', promptId: 'old', paramField: 'id', recordSource: 'axi_rolelist' }, CUSTOM, ['Orders']);
    assert.deepStrictEqual(r.value, { command: 'Configure', name: 'Orders', type: 'url', promptId: '', targetUrl: '../aspx/o.aspx', paramField: 'id', extraParams: '', recordSource: 'axi_rolelist' });
});

// ---- rename warning ------------------------------------------------------------------------------------------
test('the rename warning appears only when the name changes and names both', () => {
    assert.strictEqual(logic.renameWarning('Orders', 'Orders'), '');
    assert.strictEqual(logic.renameWarning('Orders', '  Orders '), '');
    const w = logic.renameWarning('Orders', 'Sales');
    assert.match(w, /favourites/i);
    assert.match(w, /"Orders"/);
    assert.match(w, /stop working/);
});

// ---- built-in lock -------------------------------------------------------------------------------------------
test('a Built-in Option has no editable fields, a Custom Option has', () => {
    assert.strictEqual(logic.isEditable(BUILTIN), false);
    assert.strictEqual(logic.isEditable(CUSTOM), true);
});

// ---- request building ----------------------------------------------------------------------------------------
test('update url and body', () => {
    assert.strictEqual(logic.buildUpdateUrl('http://h/api/v1/Axi/command-config/', 'usr_configure_orders', 'My App'),
        'http://h/api/v1/Axi/command-config/options/usr_configure_orders?appname=My%20App');
    const value = { command: 'Configure', name: 'N', type: 'url', promptId: '', targetUrl: '/x', paramField: '', extraParams: '', recordSource: 'Axi_Dummy' };
    assert.deepStrictEqual(logic.buildUpdateBody('bob', { appMgrAccess: true, buildAccess: true }, value),
        { username: 'bob', appMgrAccess: true, buildAccess: true, name: 'N', type: 'url', promptId: '', targetUrl: '/x', paramField: '', extraParams: '', recordSource: 'Axi_Dummy' });
});

// ---- page --------------------------------------------------------------------------------------------------
test('normalizeRow keeps the route fields the edit drawer needs', () => {
    const r = page.normalizeRow({ configId: 'a', command: 'SDK', promptOptions: 'x', promptId: 'p', targetUrl: '/u', paramField: 'f', extraParams: 'e', promptOptionType: 'url', active: 'T', isSystem: 'F', recordSource: 's' });
    assert.strictEqual(r.promptId, 'p');
    assert.strictEqual(r.targetUrl, '/u');
    assert.strictEqual(r.paramField, 'f');
    assert.strictEqual(r.extraParams, 'e');
});

test('replaceRow swaps the updated row in place', () => {
    const rows = [{ id: 'a', name: 'A' }, { id: 'b', name: 'B' }];
    const out = page.replaceRow(rows, 'b', { id: 'b', name: 'Z' });
    assert.deepStrictEqual(out.map((r) => r.name), ['A', 'Z']);
    assert.strictEqual(rows[1].name, 'B');
});

test('the page wires edit mode, the rename warning and a read-only built-in view', () => {
    const js = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfigpage.js');
    assert.match(js, /method: "POST"[\s\S]*buildUpdateUrl|buildUpdateUrl[\s\S]*method: "POST"/);
    assert.match(js, /renameWarning/);
    assert.match(js, /isEditable/);
    const h = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'AxiCMDConfig.html');
    assert.match(h, /id="acRenameWarn"/);
});

// ---- server mirror -------------------------------------------------------------------------------------------
test('the API has the update endpoint, refuses Built-in route changes and clears caches', () => {
    const ctl = read('AxiApi', 'Controllers', 'AxiController.cs');
    assert.match(ctl, /HttpPost\("command-config\/options\/\{configId\}"\)/);
    const repo = read('AxiApi', 'Repositories', 'CommandConfigRepository.cs');
    assert.match(repo, /UpdateCommandOptionAsync/);
    assert.match(repo, /Built-in Options/);
    const svc = read('AxiApi', 'Services', 'CommandConfigService.cs');
    assert.match(svc, /UpdateCommandOptionAsync[\s\S]*ClearCachesAsync\(appname, request\.Username, includePrompts: true\)/);
    const rules = read('AxiApi', 'Lib', 'CommandOptionRules.cs');
    assert.match(rules, /ReplaceOption/);
});
