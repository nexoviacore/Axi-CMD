// Ticket #288: User Activation under Configure is shown to admin always, and to anyone else only when
// axusers.licactivation is on. Runs the real getUserLicActivation / buildCommandsByAccessPermissions from
// axicmdmain.js with stubbed globals. The SQL cannot be executed here, so the ADS rows are checked as text.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const root = path.join(__dirname, '..');
const read = (...p) => fs.readFileSync(path.join(root, ...p), 'utf8');
const js = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axicmdmain.js');

// A top-level function in the IIFE starts at 4 spaces and ends at the first closing brace at 4 spaces
function extract(name) {
    const start = js.indexOf('    function ' + name + '(');
    assert.ok(start > 0, name + ' not found');
    const end = js.slice(start).search(/\r?\n    \}\r?\n/);
    return js.slice(start, start + end) + '\n    }\n';
}

function makeEnv({ userName, roles, rows, fail }) {
    const env = new Function('window', 'axDatasourceObj', 'loadList', 'getAxpertDevOpt', 'axiCommandConfigList',
        'getCommandConfigRow', 'isDevOptionAllowed',
        extract('getUserLicActivation') + extract('buildCommandsByAccessPermissions') +
        'return { getUserLicActivation, buildCommandsByAccessPermissions };');
    const store = {};
    const loadList = async (source, param) => {
        if (fail) throw new Error('network');
        store[(source + '_' + param).toLowerCase()] = rows;
        return rows;
    };
    return env({ AxUserRoles: roles, mainUserName: userName }, store, loadList, () => 'all', [], () => null, () => true);
}

function configureTypes(commands) {
    return commands.Configure.prompts[0].promptValues.split(',');
}

function freshCommands() {
    return {
        Configure: {
            prompts: [
                { prompt: 'object type', wordPos: 2, promptValues: 'TStruct,User Activation,User' },
                { prompt: 'object name', wordPos: 3, promptSource: 'axi_tstructlist,axi_useractivation,axi_userlist' }
            ]
        }
    };
}

const access = { appMgrAccess: true, importAccess: true, exportAccess: true, buildAccess: true };

async function visible(user, roles, rows, fail) {
    const env = makeEnv({ userName: user, roles, rows, fail });
    const flag = await env.getUserLicActivation(user);
    return configureTypes(env.buildCommandsByAccessPermissions(freshCommands(), access, flag)).includes('User Activation');
}

const cases = [
    ['admin, licactivation true', 'admin', 'default', [{ username: 'admin', licactivation: 'true' }], true],
    ['not admin, licactivation true', 'priya', 'sales', [{ username: 'priya', licactivation: 'true' }], true],
    ['not admin, licactivation false', 'priya', 'sales', [{ username: 'priya', licactivation: 'false' }], false],
    ['admin, licactivation false', 'admin', 'default', [{ username: 'admin', licactivation: 'F' }], true],
    ['admin without the default role is not admin', 'admin', 'sales', [{ username: 'admin', licactivation: 'f' }], false]
];

for (const [name, user, roles, rows, expected] of cases) {
    test('#288 table: ' + name, async () => {
        assert.strictEqual(await visible(user, roles, rows), expected);
    });
}

test('#288 licactivation reads T / t / true / TRUE (with spaces) as on, and F / f / false as off', async () => {
    for (const v of ['T', 't', 'true', 'TRUE', ' True ']) {
        assert.strictEqual(await visible('priya', 'sales', [{ licactivation: v }]), true, JSON.stringify(v));
    }
    for (const v of ['F', 'f', 'false', 'FALSE', '', 'yes']) {
        assert.strictEqual(await visible('priya', 'sales', [{ licactivation: v }]), false, JSON.stringify(v));
    }
    assert.strictEqual(await visible('priya', 'sales', [{ LICACTIVATION: 'T' }]), true, 'upper-case column name');
});

test('#288 a missing row, null or a failed call hides User Activation for non-admins only', async () => {
    assert.strictEqual(await visible('priya', 'sales', []), false);
    assert.strictEqual(await visible('priya', 'sales', [{ licactivation: null }]), false);
    assert.strictEqual(await visible('priya', 'sales', null, true), false);
    assert.strictEqual(await visible('admin', 'default', null, true), true);
});

test('#288 the other Configure options are untouched', async () => {
    const env = makeEnv({ userName: 'priya', roles: 'sales', rows: [] });
    const out = env.buildCommandsByAccessPermissions(freshCommands(), access, false);
    assert.deepStrictEqual(configureTypes(out), ['TStruct', 'User']);
    assert.strictEqual(out.Configure.prompts[1].promptSource, 'axi_tstructlist,axi_userlist');
});

test('#288 the loader asks for licactivation before building the commands', () => {
    const at = js.indexOf('buildCommandsByAccessPermissions(JSON.parse');
    const before = js.slice(at - 600, at);
    assert.match(before, /await getUserLicActivation\(window\.mainUserName\)/);
    assert.match(js.slice(at, at + 200), /accessPermissions, canActivateUsers\)/);
});

for (const db of ['Postgre', 'Oracle']) {
    test(`#288 ${db}: axi_userlicactivation is deleted and inserted, reading licactivation as text by username`, () => {
        const sql = read('AxpertPlugins', 'AxiCMD', 'Structures', db, 'Scripts', 'axi_axdirectsql_tables.sql');
        assert.match(sql, /DELETE FROM axdirectsql where sqlname = 'axi_userlicactivation'/);
        const row = sql.split('<<').find((b) => /INSERT/i.test(b) && b.includes("'axi_userlicactivation'"));
        assert.ok(row, 'insert row');
        assert.match(row, /licactivation as varchar|to_char\(licactivation\)/);
        assert.match(row, /from axusers where username = :param1/);
        assert.match(row, /99999999990053/);
        assert.ok(!row.includes('::'), 'no :: casts (the ADS parser reads :name)');
    });
}
