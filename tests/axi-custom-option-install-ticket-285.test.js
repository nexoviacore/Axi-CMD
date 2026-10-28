// Ticket #285, static checks. The SQL cannot be executed here (no psql/sqlplus), so these read the scripts and the C# rules.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const root = path.join(__dirname, '..');
const sqlDir = path.join(root, 'AxpertPlugins', 'AxiCMD', 'Structures');
const read = (...p) => fs.readFileSync(path.join(root, ...p), 'utf8');
const blocks = (sql) => [...sql.matchAll(/<<\r?\n([\s\S]*?)\r?\n>>/g)].map((m) => m[1]);
const restoreFile = (db) => path.join(root, 'AxpertPlugins', 'AxiCMD', 'docs', 'post-install', (db === 'Postgre' ? 'postgres' : 'oracle') + '_restore_custom_options.sql');
const logic = require(path.join(root, 'AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfiglogic.js'));

for (const db of ['Postgre', 'Oracle']) {
    const cfg = fs.readFileSync(path.join(sqlDir, db, 'Scripts', 'axi_command_config.sql'), 'utf8');
    // The re-append step is a separate post-install file run by hand: AxInstaller may run the scripts in any order
    const last = fs.readFileSync(restoreFile(db), 'utf8');

    test(`${db}: the re-append step is not in the installer scripts, only in the post-install file`, () => {
        assert.ok(!blocks(cfg).some((b) => /axi_command_prompts/i.test(b)));
        assert.match(last, /axi_command_prompts/i);
        assert.ok(!/^(<<|>>)/m.test(last), 'plain SQL, no installer block markers');
    });

    test(`${db}: re-append only touches Custom Options of Configure (4) and SDK (7) and skips names already listed`, () => {
        assert.match(last, /'configure'[\s\S]{0,30}4/i);
        assert.match(last, /'sdk', 7/i);
        assert.match(last, /is_system, 'F'\) = 'F'/i);
        assert.match(last, /(strpos|instr)\(/i);
        assert.match(last, /= 0/);
    });

    test(`${db}: names and sources are appended in one statement, in the same order, Axi_Dummy as the fallback`, () => {
        assert.strictEqual((last.match(/^\s*(UPDATE|MERGE)\b/gim) || []).length, 1);
        assert.match(last, /ORDER BY c\.config_id/i);
        assert.strictEqual((last.match(/ORDER BY c\.config_id/gi) || []).length, db === 'Postgre' ? 4 : 2); // Postgres repeats both aggregates in HAVING
        assert.match(last, /'Axi_Dummy'/); // the fallback for a null record_source (ticket 286)
        assert.match(last, /wordpos = 2/i);
        assert.match(last, /wordpos = 3/i);
    });

    test(`${db}: re-append never drops or deletes`, () => {
        assert.ok(!/\b(DROP|DELETE|TRUNCATE)\b/i.test(last.replace(/ON OVERFLOW TRUNCATE/gi, ''))); // LISTAGG ON OVERFLOW TRUNCATE only shortens an aggregate
    });
}

test('the release notes tell admins to run the post-install file', () => {
    const notes = read('AxpertPlugins', 'AxiCMD', 'RELEASE_NOTES.md');
    assert.match(notes, /postgres_restore_custom_options\.sql/);
    assert.match(notes, /oracle_restore_custom_options\.sql/);
});

test('the C# rules use the same limits and fields as the page logic', () => {
    const cs = read('AxiApi', 'Lib', 'CommandOptionRules.cs');
    assert.match(cs, new RegExp('MaxNameLength = ' + logic.MAX_NAME_LENGTH + ';'));
    assert.match(cs, new RegExp('MaxListLength = ' + logic.MAX_LIST_LENGTH + ';'));
    assert.match(cs, new RegExp('NoSource = "' + logic.NO_SOURCE + '";'));
    assert.match(cs, new RegExp('CustomTypes = \\{ ' + logic.CUSTOM_TYPES.map((t) => '"' + t + '"').join(', ') + ' \\}'));
    for (const t of logic.CUSTOM_TYPES) {
        const fields = logic.fieldsFor(t).map((f) => '"' + f + '"').join(', ');
        assert.ok(cs.includes('["' + t + '"] = new[] { ' + fields + ' }'), t);
    }
});

test('the create endpoint checks access first and the service clears both caches', () => {
    const ctl = read('AxiApi', 'Controllers', 'AxiController.cs');
    const create = ctl.slice(ctl.indexOf('CreateCommandOption('));
    assert.ok(create.indexOf('AppMgrAccess') < create.indexOf('CreateCommandOptionAsync'));
    const svc = read('AxiApi', 'Services', 'CommandConfigService.cs');
    assert.match(svc, /ClearCachesAsync\(appname, request\.Username, includePrompts: true\)/);
    const repo = read('AxiApi', 'Repositories', 'CommandConfigRepository.cs');
    assert.match(repo, /ExecuteSQLsInTransactionAsync\(commands, true\)/);
});

test('the palette drops its prompt cache after a save and the script version is bumped', () => {
    const js = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axicmdmain.js');
    assert.match(js, /window\.axiReloadCommandConfig[\s\S]{0,400}removeItem\("axi_commands_raw_v1"\)/);
    assert.match(read('aspx', 'mainHomeConfigTemplate.html'), /axicmdmain\.js\?v=76/);
});

test('the config page loads the logic script before the page script', () => {
    const html = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'AxiCMDConfig.html');
    assert.ok(html.indexOf('axiconfiglogic.js') > 0 && html.indexOf('axiconfiglogic.js') < html.indexOf('axiconfigpage.js'));
});
