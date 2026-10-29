// Ticket #285: page logic for adding a Custom Option. Loads the REAL script (no DOM needed).
// Expected values are literals taken from the spec, not recomputed with the code under test.
const test = require('node:test');
const assert = require('node:assert');
const path = require('node:path');

const logic = require(path.join(__dirname, '..', 'AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfiglogic.js'));

// ---- name rules ---------------------------------------------------------------------------------------------
test('a plain unique name is accepted and trimmed', () => {
    const r = logic.validateName('  Audit Log  ', ['user', 'role']);
    assert.strictEqual(r.ok, true);
    assert.strictEqual(r.name, 'Audit Log');
});

test('an empty or blank name is refused', () => {
    assert.strictEqual(logic.validateName('', []).code, 'required');
    assert.strictEqual(logic.validateName('   ', []).code, 'required');
    assert.strictEqual(logic.validateName(undefined, []).code, 'required');
});

test('a comma is refused because it would break the option list', () => {
    const r = logic.validateName('a,b', []);
    assert.strictEqual(r.ok, false);
    assert.strictEqual(r.code, 'comma');
    assert.match(r.message, /comma/i);
});

test('200 characters pass, 201 are refused', () => {
    assert.strictEqual(logic.validateName('x'.repeat(200), []).ok, true);
    const r = logic.validateName('x'.repeat(201), []);
    assert.strictEqual(r.code, 'length');
    assert.match(r.message, /200/);
});

test('a duplicate is refused in any case, built-in names included', () => {
    assert.strictEqual(logic.validateName('USER', ['user']).code, 'duplicate');
    assert.strictEqual(logic.validateName('user listing', ['User Listing']).code, 'duplicate');
    assert.strictEqual(logic.validateName(' user ', ['user']).code, 'duplicate');
    assert.strictEqual(logic.validateName('users', ['user']).ok, true);
});

// ---- config id ----------------------------------------------------------------------------------------------
test('config id is usr_<command>_<slug>', () => {
    assert.strictEqual(logic.makeConfigId('Configure', 'Audit Log'), 'usr_configure_audit_log');
    assert.strictEqual(logic.makeConfigId('SDK', '  My -- Page! '), 'usr_sdk_my_page');
});

test('config id never exceeds the 50 character column', () => {
    const id = logic.makeConfigId('Configure', 'a'.repeat(200));
    assert.strictEqual(id.length, 50);
    assert.ok(id.startsWith('usr_configure_aaaa'));
    assert.strictEqual(logic.makeConfigId('SDK', 'b'.repeat(200)).length, 50);
});

test('a name with no latin letters or digits still gets a stable, readable id', () => {
    const a = logic.makeConfigId('Configure', 'تقرير');
    assert.match(a, /^usr_configure_x[0-9a-f]{8}$/);
    assert.strictEqual(a, logic.makeConfigId('configure', 'تقرير'));
    assert.notStrictEqual(a, logic.makeConfigId('Configure', 'ملف'));
});

// ---- fields per type ----------------------------------------------------------------------------------------
test('fields per type match the spec', () => {
    assert.deepStrictEqual(logic.CUSTOM_TYPES, ['tstruct', 'iview', 'ivtoivload', 'url']);
    assert.deepStrictEqual(logic.fieldsFor('tstruct'), ['promptId', 'paramField']);
    assert.deepStrictEqual(logic.fieldsFor('iview'), ['promptId', 'targetUrl']);
    assert.deepStrictEqual(logic.fieldsFor('ivtoivload'), ['promptId', 'paramField', 'extraParams']);
    assert.deepStrictEqual(logic.fieldsFor('url'), ['targetUrl', 'paramField']);
    assert.deepStrictEqual(logic.fieldsFor('action'), []);
});

test('a url option needs a URL and nothing else', () => {
    const r = logic.validateOption({ command: 'Configure', name: 'Audit', type: 'url', targetUrl: '../aspx/audit.aspx', paramField: ' id ', promptId: 'ignored', extraParams: 'ignored' }, []);
    assert.strictEqual(r.ok, true);
    assert.deepStrictEqual(r.value, {
        command: 'Configure', name: 'Audit', type: 'url', promptId: '', targetUrl: '../aspx/audit.aspx', paramField: 'id', extraParams: '', recordSource: 'Axi_Dummy'
    });
});

test('missing URL, bad URL scheme and bad command are reported per field', () => {
    assert.strictEqual(logic.validateOption({ command: 'Configure', name: 'A', type: 'url', targetUrl: '' }, []).errors.targetUrl.length > 0, true);
    assert.ok(logic.validateOption({ command: 'Configure', name: 'A', type: 'url', targetUrl: 'javascript:alert(1)' }, []).errors.targetUrl);
    assert.ok(logic.validateOption({ command: 'Configure', name: 'A', type: 'url', targetUrl: 'x'.repeat(501) }, []).errors.targetUrl);
    assert.ok(logic.validateOption({ command: 'Upload', name: 'A', type: 'url', targetUrl: '/a' }, []).errors.command);
    assert.ok(logic.validateOption({ command: 'SDK', name: 'A', type: 'action', targetUrl: '/a' }, []).errors.type);
});

test('http, https, root and relative URLs are accepted', () => {
    for (const u of ['https://example.com/x', 'http://example.com', '/aspx/a.aspx', '../aspx/a.aspx']) {
        assert.strictEqual(logic.validateOption({ command: 'SDK', name: 'A', type: 'url', targetUrl: u }, []).ok, true, u);
    }
});

test('the name rules apply inside validateOption', () => {
    const r = logic.validateOption({ command: 'Configure', name: 'USER', type: 'url', targetUrl: '/a' }, ['user']);
    assert.strictEqual(r.ok, false);
    assert.ok(r.errors.name);
});

// ---- list append --------------------------------------------------------------------------------------------
test('append puts the name and Axi_Dummy at the end of both lists, keeping them paired', () => {
    const r = logic.appendOption('user,role', 'axi_userlist,axi_rolelist', 'Audit');
    assert.strictEqual(r.ok, true);
    assert.strictEqual(r.names, 'user,role,Audit');
    assert.strictEqual(r.sources, 'axi_userlist,axi_rolelist,Axi_Dummy');
});

test('append works on empty lists and ignores trailing spaces and commas', () => {
    assert.deepStrictEqual(logic.appendOption('', '', 'Audit'), { ok: true, names: 'Audit', sources: 'Axi_Dummy' });
    assert.deepStrictEqual(logic.appendOption(null, undefined, 'Audit'), { ok: true, names: 'Audit', sources: 'Axi_Dummy' });
    const r = logic.appendOption('a,b, ', 'x,y,', 'C');
    assert.strictEqual(r.names, 'a,b,C');
    assert.strictEqual(r.sources, 'x,y,Axi_Dummy');
});

test('a source list shorter than the name list is padded so positions stay paired', () => {
    const r = logic.appendOption('a,b,c', 'x', 'D');
    assert.strictEqual(r.names, 'a,b,c,D');
    assert.strictEqual(r.sources, 'x,Axi_Dummy,Axi_Dummy,Axi_Dummy');
});

test('append refuses to pass the 4000 character column limit', () => {
    const names = 'n'.repeat(3995);
    assert.strictEqual(logic.appendOption(names, 'a', 'x').ok, true);          // 3995 + 2 = 3997
    const r = logic.appendOption(names, 'a', 'xxxx');                          // 3995 + 5 = 4000 -> ok
    assert.strictEqual(r.ok, true);
    const tooLong = logic.appendOption(names, 'a', 'xxxxx');                   // 4001
    assert.strictEqual(tooLong.ok, false);
    assert.strictEqual(tooLong.code, 'listfull');
    assert.strictEqual(logic.appendOption('a', 's'.repeat(3991), 'x').ok, false); // Axi_Dummy would pass 4000
});

test('a record source of None means Axi_Dummy, anything else must be a plain name', () => {
    assert.strictEqual(logic.appendOption('a', 'x', 'B', 'axi_userlist').sources, 'x,axi_userlist');
    assert.strictEqual(logic.appendOption('a', 'x', 'B', '').sources, 'x,Axi_Dummy');
    assert.strictEqual(logic.appendOption('a', 'x', 'B', 'bad,source').ok, false);
});

// ---- request ------------------------------------------------------------------------------------------------
test('create body carries the access flag and user, create url carries the appname', () => {
    const value = logic.validateOption({ command: 'SDK', name: 'A', type: 'url', targetUrl: '/a' }, []).value;
    assert.deepStrictEqual(logic.buildCreateBody('admin', { appMgrAccess: true, buildAccess: true }, value), {
        username: 'admin', appMgrAccess: true, buildAccess: true, command: 'SDK', name: 'A', type: 'url', promptId: '', targetUrl: '/a', paramField: '', extraParams: '', recordSource: 'Axi_Dummy'
    });
    assert.strictEqual(logic.buildCreateUrl('https://h/api/v1/Axi/command-config/', 'My App'), 'https://h/api/v1/Axi/command-config/options?appname=My%20App');
});
