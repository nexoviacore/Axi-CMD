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

// ---- paging (the API pages and searches; see tests/axi-config-paging.test.js for the C# side) -------------------
test('buildPageUrl asks for one page of one command, 25 rows, with the trimmed search encoded', () => {
    assert.strictEqual(page.PAGE_SIZE, 25);
    assert.strictEqual(
        page.buildPageUrl('http://h/AxiCMDApi/api/v1/Axi/command-config/', 'my app', 'sdk', '  a&b ', 3),
        'http://h/AxiCMDApi/api/v1/Axi/command-config/options/page?appname=my%20app&command=sdk&search=a%26b&pageNo=3&pageSize=25');
    assert.match(page.buildPageUrl('http://h/x', 'a', 'configure', '', 0), /&search=&pageNo=1&/);
});

test('normalizePage reads the API page (camelCase or PascalCase) into rows, totals and tab counts', () => {
    const p = page.normalizePage({ items: API_ROWS.slice(0, 2), totalCount: 60, pageNo: 2, pageSize: 25, totalPages: 3, configureCount: 60, sdkCount: 7 });
    assert.deepStrictEqual(p.rows.map(r => r.name), ['user', 'role listing']);
    assert.deepStrictEqual({ total: p.total, pageNo: p.pageNo, pageSize: p.pageSize, totalPages: p.totalPages, counts: p.counts },
        { total: 60, pageNo: 2, pageSize: 25, totalPages: 3, counts: { configure: 60, sdk: 7 } });
    const q = page.normalizePage({ Items: [API_ROWS[2]], TotalCount: 1, PageNo: 1, PageSize: 25, TotalPages: 1, ConfigureCount: 2, SdkCount: 1 });
    assert.strictEqual(q.rows[0].name, 'TStruct');
    assert.deepStrictEqual(q.counts, { configure: 2, sdk: 1 });
});

test('normalizePage of nothing is one empty page', () => {
    const p = page.normalizePage(null);
    assert.deepStrictEqual({ rows: p.rows, total: p.total, pageNo: p.pageNo, totalPages: p.totalPages }, { rows: [], total: 0, pageNo: 1, totalPages: 1 });
});

test('pageNumbers shows the first, the last and the neighbours, with gaps of two or more as "..."', () => {
    assert.deepStrictEqual(page.pageNumbers(1, 1), [1]);
    assert.deepStrictEqual(page.pageNumbers(1, 4), [1, 2, 3, 4]);
    assert.deepStrictEqual(page.pageNumbers(1, 10), [1, 2, '...', 10]);
    assert.deepStrictEqual(page.pageNumbers(5, 10), [1, '...', 4, 5, 6, '...', 10]);
    assert.deepStrictEqual(page.pageNumbers(4, 10), [1, 2, 3, 4, 5, '...', 10]);
    assert.deepStrictEqual(page.pageNumbers(10, 10), [1, '...', 9, 10]);
});

test('rangeText names the rows on the page', () => {
    assert.strictEqual(page.rangeText(1, 25, 60), '1-25 of 60');
    assert.strictEqual(page.rangeText(3, 25, 60), '51-60 of 60');
    assert.strictEqual(page.rangeText(1, 25, 0), '0 of 0');
});

test('directionFor returns rtl for RTL languages only', () => {
    assert.strictEqual(page.directionFor('ar'), 'rtl');
    assert.strictEqual(page.directionFor('AR-SA'), 'rtl');
    assert.strictEqual(page.directionFor('en'), 'ltr');
    assert.strictEqual(page.directionFor(''), 'ltr');
});
