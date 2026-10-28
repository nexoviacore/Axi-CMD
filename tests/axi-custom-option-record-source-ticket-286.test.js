// Ticket #286, static checks: the record source of a Custom Option is stored in axi_command_config.record_source and the
// re-install step restores it. The SQL cannot be executed here (no psql/sqlplus), so these read the scripts and the C#.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const root = path.join(__dirname, '..');
const sqlDir = path.join(root, 'AxpertPlugins', 'AxiCMD', 'Structures');
const read = (...p) => fs.readFileSync(path.join(root, ...p), 'utf8');
const blocks = (sql) => [...sql.matchAll(/<<\r?\n([\s\S]*?)\r?\n>>/g)].map((m) => m[1]);

for (const db of ['Postgre', 'Oracle']) {
    const cfg = fs.readFileSync(path.join(sqlDir, db, 'Scripts', 'axi_command_config.sql'), 'utf8');
    const all = blocks(cfg);
    const last = fs.readFileSync(path.join(root, 'AxpertPlugins', 'AxiCMD', 'docs', 'post-install', (db === 'Postgre' ? 'postgres' : 'oracle') + '_restore_custom_options.sql'), 'utf8');

    test(`${db}: record_source is a nullable column, in the create and in a re-runnable ALTER`, () => {
        const create = all.find((b) => /CREATE TABLE/i.test(b)) || cfg;
        assert.match(create, /record_source"?\s+varchar2?\(\d+\)/i);
        assert.ok(!/record_source[^\n]*NOT NULL/i.test(cfg));
        const alter = all.find((b) => /ALTER TABLE/i.test(b) && /record_source/i.test(b));
        assert.ok(alter, 'ALTER block');
        if (db === 'Postgre') assert.match(alter, /ADD COLUMN IF NOT EXISTS record_source/i);
        else assert.match(alter, /user_tab_columns[\s\S]*RECORD_SOURCE[\s\S]*ALTER TABLE AXI_COMMAND_CONFIG ADD/i);
    });

    test(`${db}: the re-install step restores each option's own record source, Axi_Dummy when null`, () => {
        assert.match(last, /record_source/i);
        assert.match(last, /(coalesce|nvl)\(\s*c\.record_source\s*,\s*'Axi_Dummy'\s*\)/i);
        assert.ok(!/',Axi_Dummy'/.test(last));
        // Postgres repeats both aggregates in HAVING (4000 character guard); Oracle filters in an outer select
        assert.strictEqual((last.match(/ORDER BY c\.config_id/gi) || []).length, db === 'Postgre' ? 4 : 2);
        assert.strictEqual((last.match(/^\s*(UPDATE|MERGE)\b/gim) || []).length, 1);
    });

    test(`${db}: shipped Built-in Options do not set a record source`, () => {
        const inserts = all.filter((b) => /^\s*(INSERT|MERGE INTO)/i.test(b));
        assert.ok(inserts.length > 40);
        assert.ok(inserts.every((b) => !/record_source/i.test(b)));
    });
}

test('the create statement stores the record source and the list query reads it back', () => {
    const q = read('AxiApi', 'Lib', 'SqlQueries.cs');
    const ins = q.slice(q.indexOf('InsertCustomCommandOption'), q.indexOf('InsertCustomCommandOption') + 600);
    assert.match(ins, /extra_params, active, is_system, record_source\)/);
    assert.match(ins, /:recordsource\)/);
    const repo = read('AxiApi', 'Repositories', 'CommandConfigRepository.cs');
    assert.match(repo, /":recordsource"/);
    assert.match(repo, /option\.RecordSource/);
});

test('the list shows the stored record source when there is one', () => {
    const q = read('AxiApi', 'Lib', 'SqlQueries.cs');
    assert.match(q.slice(q.indexOf('SelectAllCommandConfigs')), /is_system, record_source FROM axi_command_config|is_system, record_source "/);
    const repo = read('AxiApi', 'Repositories', 'CommandConfigRepository.cs');
    assert.match(repo, /row\["record_source"\]/);
});

test('C# rules keep the same known record sources as the page logic', () => {
    const logic = require(path.join(root, 'AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfiglogic.js'));
    const cs = read('AxiApi', 'Lib', 'CommandOptionRules.cs');
    for (const s of logic.KNOWN_RECORD_SOURCES) assert.ok(cs.includes('"' + s + '"'), s);
});
