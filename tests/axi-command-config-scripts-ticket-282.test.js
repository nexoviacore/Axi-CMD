// Static checks for ticket #282: the command-config install scripts must be safe to re-run
// and ready for Custom Options. The SQL cannot be executed here, so these read the scripts.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const dir = path.join(__dirname, '..', 'AxpertPlugins', 'AxiCMD', 'Structures');
const read = (db, f) => fs.readFileSync(path.join(dir, db, 'Scripts', f), 'utf8');
const blocks = (sql) => [...sql.matchAll(/<<\r?\n([\s\S]*?)\r?\n>>/g)].map((m) => m[1]);

for (const db of ['Postgre', 'Oracle']) {
    const cfg = read(db, 'axi_command_config.sql');
    const cmd = read(db, 'axi_command_tables.sql');
    const inserts = blocks(cfg).filter((b) => /^\s*(INSERT|MERGE)/i.test(b));

    test(`${db}: config script never drops the table`, () => {
        assert.ok(!/DROP\s+TABLE\s+axi_command_config/i.test(cfg));
    });

    test(`${db}: tables script never drops axi_command_prompts, so Custom Option names survive a re-install`, () => {
        assert.ok(!blocks(cmd).some((b) => /DROP\s+TABLE\s+axi_command_prompts/i.test(b)));
    });

    test(`${db}: config table create is guarded and has is_system defaulting to F`, () => {
        if (db === 'Postgre') {
            assert.match(cfg, /CREATE TABLE IF NOT EXISTS axi_command_config/i);
            assert.match(cfg, /ADD COLUMN IF NOT EXISTS is_system varchar\(1\) DEFAULT 'F'/i);
        } else {
            assert.match(cfg, /user_tables WHERE table_name = 'AXI_COMMAND_CONFIG'/i);
            assert.match(cfg, /user_tab_columns[\s\S]*'IS_SYSTEM'/i);
            assert.match(cfg, /ADD \(?\s*IS_SYSTEM VARCHAR2\(1\) DEFAULT 'F'/i);
        }
        assert.match(cfg, /is_system/i);
    });

    test(`${db}: every shipped row sets is_system T and inserts only when missing`, () => {
        // old code
        // assert.strictEqual(inserts.length, 51);
        // new code: +3 rows for configure product / plugin / changerequest
        assert.strictEqual(inserts.length, 54);
        for (const b of inserts) {
            assert.match(b, /is_system/i);
            assert.match(b, /'T'\)\s*(ON CONFLICT \(config_id\) DO UPDATE SET is_system = 'T')?\s*$/i);
            if (db === 'Postgre') assert.match(b, /ON CONFLICT \(config_id\) DO UPDATE SET is_system = 'T'$/);
            else assert.match(b, /^MERGE[\s\S]*WHEN MATCHED THEN UPDATE SET t\.IS_SYSTEM = 'T'[\s\S]*WHEN NOT MATCHED/i);
        }
    });

    test(`${db}: re-run keeps hand edits and active flag (update touches only is_system)`, () => {
        for (const b of inserts) {
            assert.ok(!/UPDATE SET[^']*\b(active|prompt_options|target_url)\b/i.test(b));
        }
    });

    test(`${db}: index create is safe to re-run`, () => {
        if (db === 'Postgre') assert.match(cfg, /CREATE INDEX IF NOT EXISTS/i);
        else assert.match(cfg, /user_indexes WHERE index_name = 'IDX_AXI_CMD_CONFIG_CMD'/i);
    });

    test(`${db}: prompt list columns are 4000 wide, create and re-runnable alter`, () => {
        if (db === 'Postgre') {
            assert.match(cmd, /promptsource varchar\(4000\)/);
            assert.match(cmd, /promptvalues varchar\(4000\)/);
            assert.match(cmd, /ALTER TABLE axi_command_prompts ALTER COLUMN promptsource TYPE varchar\(4000\)/);
            assert.match(cmd, /ALTER TABLE axi_command_prompts ALTER COLUMN promptvalues TYPE varchar\(4000\)/);
        } else {
            assert.match(cmd, /"PROMPTSOURCE" VARCHAR2\(4000\)/);
            assert.match(cmd, /"PROMPTVALUES" VARCHAR2\(4000\)/);
            assert.match(cmd, /ALTER TABLE AXI_COMMAND_PROMPTS MODIFY \(PROMPTSOURCE VARCHAR2\(4000\)\)/);
            assert.match(cmd, /ALTER TABLE AXI_COMMAND_PROMPTS MODIFY \(PROMPTVALUES VARCHAR2\(4000\)\)/);
        }
    });

    test(`${db}: files keep CRLF only`, () => {
        for (const s of [cfg, cmd]) assert.ok(!/(^|[^\r])\n/.test(s));
    });
}
