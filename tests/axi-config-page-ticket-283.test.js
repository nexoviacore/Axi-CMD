// Ticket #283: pure logic of the AxiCMD Config page. Loads the REAL page script (not a re-implementation).
const test = require('node:test');
const assert = require('node:assert');
const path = require('node:path');

const page = require(path.join(__dirname, '..', 'AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfigpage.js'));

// Shape returned by AxiApi GET .../Axi/command-config/options (System.Text.Json camelCase)
const API_ROWS = [
    { configId: 'cfg_configure_user', command: 'Configure', promptOptions: 'user', promptId: 'axusr', promptOptionType: 'tstruct', paramField: null, targetUrl: null, extraParams: null, active: 'T', isSystem: 'T', recordSource: 'axi_userlist' },
    { configId: 'cfg_configure_roles', command: 'Configure', promptOptions: 'role listing', promptId: 'ad___url', promptOptionType: 'iview', targetUrl: '../aspx/iview.aspx?ivname=ad___url', active: 'F', isSystem: 'T', recordSource: 'axi_rolelist' },
    { configId: 'cfg_sdk_tstruct', command: 'SDK', promptOptions: 'TStruct', promptId: 'tstruct', promptOptionType: 'url', targetUrl: '../aspx/x.aspx', active: 'T', isSystem: 'F', recordSource: 'axi_structmetalist' },
    { configId: 'cfg_other', command: 'Create', promptOptions: 'zzz', promptId: 'z', promptOptionType: 'tstruct', active: 'T', isSystem: 'F' }
];

test('normalizeRow flattens an API row for display', () => {
    const r = page.normalizeRow(API_ROWS[1]);
    assert.deepStrictEqual(
        { id: r.id, name: r.name, type: r.type, target: r.target, active: r.active, builtIn: r.builtIn, source: r.source },
        { id: 'cfg_configure_roles', name: 'role listing', type: 'iview', target: '../aspx/iview.aspx?ivname=ad___url', active: false, builtIn: true, source: 'axi_rolelist' });
});

test('target falls back to the object id when there is no url', () => {
    assert.strictEqual(page.normalizeRow(API_ROWS[0]).target, 'axusr');
});

test('normalizeRow accepts PascalCase and snake_case keys', () => {
    const r = page.normalizeRow({ Command: 'SDK', PromptOptions: 'Page', PromptId: 'p', PromptOptionType: 'url', Active: 't', IsSystem: 'f' });
    assert.strictEqual(r.name, 'Page');
    assert.strictEqual(r.active, true);
    assert.strictEqual(r.builtIn, false);
    const s = page.normalizeRow({ command: 'Configure', prompt_options: 'x', prompt_id: 'y', active: 'F', is_system: 'T' });
    assert.strictEqual(s.name, 'x');
    assert.strictEqual(s.active, false);
    assert.strictEqual(s.builtIn, true);
});

test('groupByTab keeps inactive options and only Configure / SDK', () => {
    const g = page.groupByTab(API_ROWS);
    assert.deepStrictEqual(g.configure.map(r => r.name), ['role listing', 'user']);
    assert.deepStrictEqual(g.sdk.map(r => r.name), ['TStruct']);
});

test('filterRows matches name or target, case-insensitively, and empty term returns all', () => {
    const rows = page.groupByTab(API_ROWS).configure;
    assert.strictEqual(page.filterRows(rows, '').length, 2);
    assert.deepStrictEqual(page.filterRows(rows, 'USER').map(r => r.name), ['user']);
    assert.deepStrictEqual(page.filterRows(rows, 'ivname=ad___url').map(r => r.name), ['role listing']);
    assert.strictEqual(page.filterRows(rows, 'nomatch').length, 0);
});

test('buildListUrl encodes appname and username', () => {
    assert.strictEqual(
        page.buildListUrl('http://h/AxiCMDApi/api/v1/Axi/command-config', 'my app', 'a&b'),
        'http://h/AxiCMDApi/api/v1/Axi/command-config/options?appname=my%20app&username=a%26b');
});

test('directionFor returns rtl for RTL languages only', () => {
    assert.strictEqual(page.directionFor('ar'), 'rtl');
    assert.strictEqual(page.directionFor('AR-SA'), 'rtl');
    assert.strictEqual(page.directionFor('en'), 'ltr');
    assert.strictEqual(page.directionFor(''), 'ltr');
});
