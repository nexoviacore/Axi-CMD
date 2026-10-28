// Ticket #284: Active switch on the AxiCMD Config page. Loads the REAL page script and the REAL palette script text.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const html = path.join(__dirname, '..', 'AxpertPlugins', 'AxiCMD', 'HTMLPages');
const page = require(path.join(html, 'js', 'axiconfigpage.js'));
const palette = fs.readFileSync(path.join(html, 'js', 'axicmdmain.js'), 'utf8');
const controller = fs.readFileSync(path.join(__dirname, '..', 'AxiApi', 'Controllers', 'AxiController.cs'), 'utf8');
const service = fs.readFileSync(path.join(__dirname, '..', 'AxiApi', 'Services', 'CommandConfigService.cs'), 'utf8');

const ROWS = [
    { configId: 'cfg_a', command: 'Configure', promptOptions: 'user', promptId: 'axusr', promptOptionType: 'tstruct', active: 'T', isSystem: 'T' },
    { configId: 'cfg_b', command: 'SDK', promptOptions: 'TStruct', promptId: 't', promptOptionType: 'url', active: 'F', isSystem: 'F' }
];

test('buildActiveUrl targets the option by id and carries the appname', () => {
    assert.strictEqual(
        page.buildActiveUrl('https://h/AxiCMDApi/api/v1/Axi/command-config/', 'cfg a', 'My App'),
        'https://h/AxiCMDApi/api/v1/Axi/command-config/options/cfg%20a/active?appname=My%20App');
});

test('buildActiveBody carries the access flag the API checks', () => {
    assert.deepStrictEqual(page.buildActiveBody('admin', false, true), { username: 'admin', active: false, appMgrAccess: true });
});

test('setActive flips one row only and leaves the original groups untouched', () => {
    const groups = page.groupByTab(ROWS);
    const next = page.setActive(groups, 'cfg_a', false);
    assert.strictEqual(next.configure[0].active, false);
    assert.strictEqual(groups.configure[0].active, true);
    assert.strictEqual(next.sdk[0].active, false);
    assert.strictEqual(page.setActive(next, 'cfg_b', true).sdk[0].active, true);
});

test('describeFailure gives a readable message for a refusal and for a network error', () => {
    assert.match(page.describeFailure(403, 'Configure Access is required.'), /Configure Access is required/);
    assert.match(page.describeFailure(0, ''), /connection/i);
    assert.match(page.describeFailure(500, ''), /500/);
});

test('the notice tells the user when other users see the change', () => {
    assert.doesNotMatch(page.OTHER_USERS_NOTE, /30 minutes/);
    assert.match(page.OTHER_USERS_NOTE, /Refresh/);
});

test('palette exposes a reload hook that clears the local command-config cache', () => {
    assert.match(palette, /window\.axiReloadCommandConfig\s*=/);
    const start = palette.indexOf('window.axiReloadCommandConfig');
    const snippet = palette.slice(start, start + 600);
    assert.match(snippet, /removeItem\("axi_command_config_v2"\)/);
    assert.match(snippet, /initCommands\(false\)/);
});

test('palette passes Configure Access to the page', () => {
    assert.match(palette, /&access=1/);
});

test('API write checks appMgrAccess first and clears the command-config cache key', () => {
    assert.match(controller, /HttpPost\("command-config\/options\/\{configId\}\/active"\)/);
    // one shared guard (appname, username, Configure Access) used by the three write actions
    assert.match(controller, /CheckConfigureAccess\(string appname, string\? username, bool appMgrAccess\)[\s\S]{0,600}StatusCode\(StatusCodes\.Status403Forbidden/);
    assert.strictEqual((controller.match(/CheckConfigureAccess\(appname, requestDTO/g) || []).length, 3);
    assert.match(service, /GenerateCacheKey\(appname, "command_config", username\)/);
    assert.match(service, /SetCommandOptionActiveAsync[\s\S]*KeysDeleteAsync/);
});
