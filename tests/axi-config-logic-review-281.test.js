// Code review fixes for the AxiCMD Config page (#281): record source length, relative target URLs,
// tolerant prompt lists. Loads the REAL axiconfiglogic.js; C# parity is checked statically on the source text.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const root = path.join(__dirname, '..');
const logic = require(path.join(root, 'AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfiglogic.js'));
const read = (...p) => fs.readFileSync(path.join(root, ...p), 'utf8');
const rules = read('AxiApi', 'Lib', 'CommandOptionRules.cs');

const urlOpt = (u) => logic.validateOption({ command: 'SDK', name: 'A', type: 'url', targetUrl: u }, []);

// ---- record source length -----------------------------------------------------------------------------------
test('a record source of 100 characters is accepted, 101 is refused', () => {
    const ok = logic.validateOption({ command: 'SDK', name: 'A', type: 'url', targetUrl: '/a', recordSource: 'a'.repeat(100) }, []);
    assert.strictEqual(ok.ok, true);
    const bad = logic.validateOption({ command: 'SDK', name: 'A', type: 'url', targetUrl: '/a', recordSource: 'a'.repeat(101) }, []);
    assert.strictEqual(bad.ok, false);
    assert.match(bad.errors.recordSource, /100/);
});

test('append and replace refuse a record source over 100 characters', () => {
    assert.strictEqual(logic.appendOption('a', 'x', 'B', 'a'.repeat(100)).ok, true);
    const r = logic.appendOption('a', 'x', 'B', 'a'.repeat(101));
    assert.strictEqual(r.ok, false);
    assert.strictEqual(r.code, 'source');
    assert.strictEqual(logic.replaceOption('a', 'x', 'a', 'a', 'a'.repeat(101)).ok, false);
});

test('the C# rules carry the same record source limit and message', () => {
    assert.match(rules, new RegExp('MaxRecordSourceLength = ' + logic.MAX_RECORD_SOURCE_LENGTH + ';'));
    assert.strictEqual(logic.MAX_RECORD_SOURCE_LENGTH, 100);
    assert.match(rules, /MaxRecordSourceLength/);
});

// ---- target URL ---------------------------------------------------------------------------------------------
test('relative paths with or without a query string are accepted', () => {
    for (const u of ['iview.aspx?ivname=x', 'aspx/page.aspx', 'page.aspx?a=b:c', './x.aspx', 'https://h/x', '/x', '../x'])
        assert.strictEqual(urlOpt(u).ok, true, u);
});

test('other schemes, control characters and blanks are refused', () => {
    for (const u of ['javascript:alert(1)', 'JaVaScRiPt:alert(1)', 'data:text/html,x', 'ftp://h/x', 'vbscript:x', 'java\tscript:alert(1)', 'java\nscript:x', 'mailto:a@b.c'])
        assert.ok(urlOpt(u).errors && urlOpt(u).errors.targetUrl, JSON.stringify(u));
    assert.ok(urlOpt('   ').errors.targetUrl);
});

test('the C# rules no longer demand a leading http, slash or dot-dot', () => {
    assert.ok(!rules.includes('must start with http://'));
    assert.ok(rules.includes('IsValidTargetUrl') && rules.includes('^([a-z][a-z0-9+.\\-]*):'));
});

// ---- tolerant prompt lists ----------------------------------------------------------------------------------
test('append trims entries and drops stray trailing commas in both lists', () => {
    const r = logic.appendOption(' a , b ,', ' x ,y,, ', 'C');
    assert.deepStrictEqual(r, { ok: true, names: 'a,b,C', sources: 'x,y,Axi_Dummy' });
});

test('replace trims entries and still finds the old name', () => {
    const r = logic.replaceOption('a, b ,', 'x, y,', 'b', 'B', 's');
    assert.deepStrictEqual(r, { ok: true, names: 'a,B', sources: 'x,s' });
});

test('an interior empty entry keeps its position so pairing is not broken', () => {
    const r = logic.appendOption('a,,c', 'x,y,z', 'D');
    assert.deepStrictEqual(r, { ok: true, names: 'a,,c,D', sources: 'x,y,z,Axi_Dummy' });
});

test('the C# list helpers trim entries and drop only trailing empties', () => {
    assert.match(rules, /Select\(x => x\.Trim\(\)\)/);
    assert.match(rules, /RemoveAt\(parts\.Count - 1\)/);
    assert.ok(!/string\.Join\(",", n\) == names/.test(rules), 'pairing check must not demand byte-identical lists');
});

// ---- install: 4000 character guard, same in both databases ----------------------------------------------------
for (const db of ['Postgre', 'Oracle']) {
    test(`${db}: the re-append step skips a command whose lists would pass 4000 characters instead of failing`, () => {
        const last = read('AxpertPlugins', 'AxiCMD', 'docs', 'post-install', (db === 'Postgre' ? 'postgres' : 'oracle') + '_restore_custom_options.sql');
        assert.match(last, /<= 4000/);
        assert.strictEqual((last.match(/<= 4000/g) || []).length >= 2, true);
        if (db === 'Oracle') assert.match(last, /ON OVERFLOW TRUNCATE/);
    });
}

// ---- C# structure ---------------------------------------------------------------------------------------------
test('the options list endpoint declares no unused username parameter', () => {
    const c = read('AxiApi', 'Controllers', 'AxiController.cs');
    assert.match(c, /GetCommandConfigOptions\(\[FromQuery\] string appname\)/);
});

test('the repository has no hard-coded Configure/SDK tokens', () => {
    const repo = read('AxiApi', 'Repositories', 'CommandConfigRepository.cs');
    assert.ok(!/\(4, "Configure"\)|\(7, "SDK"\)/.test(repo));
    assert.strictEqual((repo.match(/Convert\.ToInt32\(p\["cmdtoken"\]\)/g) || []).length, 1, 'one reader of the prompt lists');
});
