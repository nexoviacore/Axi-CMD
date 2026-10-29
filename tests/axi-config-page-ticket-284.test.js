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

test('buildActiveBody carries both access flags the API checks', () => {
    assert.deepStrictEqual(page.buildActiveBody('admin', false, { appMgrAccess: true, buildAccess: true }),
        { username: 'admin', active: false, appMgrAccess: true, buildAccess: true });
    assert.deepStrictEqual(page.buildActiveBody('admin', true, { appMgrAccess: true }),
        { username: 'admin', active: true, appMgrAccess: true, buildAccess: false });
});

test('the page lets the user configure only with both Configure Access and build access', () => {
    const at = (qs) => page.readAccess(new URLSearchParams(qs));
    assert.strictEqual(at('appmgraccess=1&buildaccess=1').canConfigure, true);
    assert.strictEqual(at('appmgraccess=1&buildaccess=0').canConfigure, false);
    assert.strictEqual(at('appmgraccess=0&buildaccess=1').canConfigure, false);
    assert.strictEqual(at('access=1').canConfigure, false);
    assert.strictEqual(at('').canConfigure, false);
});

test('setActive flips one row only and leaves the original rows untouched', () => {
    const rows = ROWS.map(page.normalizeRow);
    const next = page.setActive(rows, 'cfg_a', false);
    assert.strictEqual(next[0].active, false);
    assert.strictEqual(rows[0].active, true);
    assert.strictEqual(next[1].active, false);
    assert.strictEqual(page.setActive(next, 'cfg_b', true)[1].active, true);
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

test('a switch save keeps focus: it patches its own row and never disables or re-renders', () => {
    const js = fs.readFileSync(path.join(html, 'js', 'axiconfigpage.js'), 'utf8');
    const start = js.indexOf('function save(row, wanted, input)');
    const saveFn = js.slice(start, js.indexOf('function onSwitch', start));
    assert.doesNotMatch(saveFn, /input\.disabled = true/);
    assert.doesNotMatch(saveFn, /render\(\)/);
    assert.match(saveFn, /patchRow\(row\.id, wanted\)/);
    assert.match(js, /if \(input\.dataset\.busy\) \{ input\.checked = !input\.checked; return Promise\.resolve\(\); \}/);
    assert.match(js, /tr\.dataset\.id = r\.id/);
});

test('the page note is a toast outside the page flow, so it never pushes the table', () => {
    const page = fs.readFileSync(path.join(html, 'AxiCMDConfig.html'), 'utf8');
    const note = page.indexOf('id="acNote"');
    assert.ok(note > page.indexOf('</main>'), 'note sits after </main>');
    assert.match(page, /id="acNote" class="ac-note ac-toast"/);
    const css = fs.readFileSync(path.join(html, 'css', 'axiconfig.css'), 'utf8');
    assert.match(css, /\.ac-toast \{[^}]*position: fixed;/);
});

test('switching says the change reaches everyone in the app, and switching off can be undone', () => {
    assert.strictEqual(page.switchMessage('user', false),
        '"user" is hidden from the palette for everyone in this app. Other users see the change after they press Refresh in AxiCMD.');
    assert.strictEqual(page.switchMessage('user', true),
        '"user" is back in the palette for everyone in this app. Other users see the change after they press Refresh in AxiCMD.');
    assert.doesNotMatch(page.OTHER_USERS_NOTE, /^Saved/, 'no "Saved \\"x\\". Saved." double');
    const js = fs.readFileSync(path.join(html, 'js', 'axiconfigpage.js'), 'utf8');
    assert.match(js, /wanted \? null : \{ label: "Undo", run: \(\) => undoSwitchOff\(row\) \}/);
    assert.match(js, /\(e\.ctrlKey \|\| e\.metaKey\) && !e\.shiftKey && \(e\.key === "z" \|\| e\.key === "Z"\)/);
    const markup = fs.readFileSync(path.join(html, 'AxiCMDConfig.html'), 'utf8');
    assert.match(markup, /hides it from the palette for everyone in this app/);
});

test('inactive rows recede by colour and weight, never by opacity (which failed AA contrast at 2.3:1)', () => {
    const css = fs.readFileSync(path.join(html, 'css', 'axiconfig.css'), 'utf8');
    assert.doesNotMatch(css, /\.ac-off[^{]*\{[^}]*opacity/);
    assert.match(css, /\.ac-off \.ac-name, \.ac-off \.ac-optname \{ color: var\(--slate\); font-weight: 400; \}/);
});

test('the tab panel is the table wrapper, so the table keeps its table semantics', () => {
    const markup = fs.readFileSync(path.join(html, 'AxiCMDConfig.html'), 'utf8');
    assert.match(markup, /<div id="acPanel" class="ac-scroll" tabindex="0" role="tabpanel" aria-labelledby="tab-configure">/);
    assert.match(markup, /<table id="acTable" class="ac-table">/);
    assert.strictEqual((markup.match(/aria-controls="acPanel"/g) || []).length, 2);
    const js = fs.readFileSync(path.join(html, 'js', 'axiconfigpage.js'), 'utf8');
    assert.match(js, /\$\("acPanel"\)\.setAttribute\("aria-labelledby", "tab-" \+ name\)/);
});

test('a viewer without both accesses is told why, and a failed load offers Try again', () => {
    const markup = fs.readFileSync(path.join(html, 'AxiCMDConfig.html'), 'utf8');
    assert.match(markup, /id="acAccessNote"[^>]*hidden>You can view these options\. Changing them needs Configure Access and build access\.<\/p>/);
    const js = fs.readFileSync(path.join(html, 'js', 'axiconfigpage.js'), 'utf8');
    assert.match(js, /if \(hasAccess\) addBtn\.hidden = false;\s*\/\/[^\n]*\n\s*else \$\("acAccessNote"\)\.hidden = false;/);
    assert.match(js, /retry\.textContent = "Try again";\s*retry\.addEventListener\("click", \(\) => load\(tab, no, swapFrom, focus\)\);/);
});

test('palette exposes a reload hook that clears the local command-config cache', () => {
    assert.match(palette, /window\.axiReloadCommandConfig\s*=/);
    const start = palette.indexOf('window.axiReloadCommandConfig');
    const snippet = palette.slice(start, start + 600);
    assert.match(snippet, /removeItem\("axi_command_config_v2"\)/);
    assert.match(snippet, /initCommands\(false\)/);
});

test('palette offers Configure only with both accesses and passes both to the page', () => {
    assert.match(palette, /if \(perms\.appMgrAccess && perms\.buildAccess\) entries\.push\(\{ id: "configure"/);
    assert.match(palette, /&appmgraccess=\$\{perms\.appMgrAccess \? 1 : 0\}/);
    assert.match(palette, /&buildaccess=\$\{perms\.buildAccess \? 1 : 0\}/);
    assert.doesNotMatch(palette, /&access=1/);
});

test('API write checks appMgrAccess first and clears the command-config cache key', () => {
    assert.match(controller, /HttpPost\("command-config\/options\/\{configId\}\/active"\)/);
    // one shared guard (appname, username, Configure Access and build access) used by the three write actions
    assert.match(controller, /CheckConfigureAccess\(string appname, string\? username, bool appMgrAccess, bool buildAccess\)[\s\S]{0,600}if \(!appMgrAccess \|\| !buildAccess\)\s*return StatusCode\(StatusCodes\.Status403Forbidden/);
    assert.strictEqual((controller.match(/CheckConfigureAccess\(appname, requestDTO\?\.Username, requestDTO\?\.AppMgrAccess \?\? false, requestDTO\?\.BuildAccess \?\? false\)/g) || []).length, 3);
    for (const d of ['SetCommandOptionActiveRequestDTO', 'CreateCommandOptionRequestDTO', 'UpdateCommandOptionRequestDTO'])
        assert.match(fs.readFileSync(path.join(__dirname, '..', 'AxiApi', 'DTOs', d + '.cs'), 'utf8'), /public bool BuildAccess \{ get; set; \}/, d);
    assert.match(service, /GenerateCacheKey\(appname, "command_config", username\)/);
    assert.match(service, /SetCommandOptionActiveAsync[\s\S]*KeysDeleteAsync/);
});
