// Server side paging of the AxiCMD Config page. The C# cannot run here (no database), so these read the source text;
// the page's own paging logic is tested on the REAL script in axi-config-page-ticket-283.test.js.
const test = require('node:test');
const assert = require('node:assert');
const fs = require('node:fs');
const path = require('node:path');

const root = path.join(__dirname, '..');
const read = (...p) => fs.readFileSync(path.join(root, ...p), 'utf8');
const page = require(path.join(root, 'AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfigpage.js'));
const sql = read('AxiApi', 'Lib', 'SqlQueries.cs');
const paging = read('AxiApi', 'Lib', 'CommandOptionPaging.cs');
const repo = read('AxiApi', 'Repositories', 'CommandConfigRepository.cs');
const controller = read('AxiApi', 'Controllers', 'AxiController.cs');
const service = read('AxiApi', 'Services', 'CommandConfigService.cs');

const constOf = (name) => {
    const m = sql.match(new RegExp('public const string ' + name + ' =([\\s\\S]*?);\\r?\\n'));
    assert.ok(m, name);
    return m[1].match(/"((?:[^"\\]|\\.)*)"/g).map((s) => JSON.parse(s)).join('');
};

test('the page size of the page is the default of the API', () => {
    assert.match(paging, new RegExp('DefaultPageSize = ' + page.PAGE_SIZE + ';'));
    assert.match(paging, /MaxPageSize = 100;/);
});

test('the page query sorts by name, filters by command and pages with OFFSET / FETCH (Postgres and Oracle 12c+)', () => {
    const q = constOf('SelectCommandOptionsPage');
    assert.match(q, /WHERE LOWER\(command\) = :command\{0\} /);
    assert.match(q, /ORDER BY LOWER\(prompt_options\), config_id /);
    assert.match(q, /OFFSET \{1\} ROWS FETCH NEXT \{2\} ROWS ONLY$/);
    assert.ok(!/LIMIT/i.test(q));
    assert.match(constOf('CountCommandOptionsPage'), /COUNT\(\*\) AS cnt FROM axi_command_config WHERE LOWER\(command\) = :command\{0\}$/);
});

test('the search matches name or target, escapes the user\'s % and _, and names each parameter once', () => {
    const s = constOf('CommandOptionsSearch');
    for (const col of ['prompt_options', 'target_url', 'prompt_id']) assert.match(s, new RegExp('LOWER\\(' + col + '\\) LIKE :search\\d ESCAPE \'\\\\\''));
    const names = s.match(/:search\d/g);
    assert.deepStrictEqual(names, [':search1', ':search2', ':search3']);
    assert.match(paging, /Replace\("\\\\", "\\\\\\\\"\)\.Replace\("%", "\\\\%"\)\.Replace\("_", "\\\\_"\)/);
});

test('parameters are passed in the order the SQL uses them (Oracle binds by position)', () => {
    assert.match(repo, /new List<string> \{ ":command" \}[\s\S]{0,400}AddRange\(new\[\] \{ ":search1", ":search2", ":search3" \}\)/);
});

test('offset and page size go into the SQL only as integers', () => {
    assert.match(repo, /string\.Format\(SqlQueries\.SelectCommandOptionsPage, filter, \(page\.PageNo - 1\) \* pageSize, pageSize\)/);
    assert.match(service, /CommandOptionPaging\.ClampPageSize\(pageSize\)/);
    assert.match(repo, /int\? pageNo, int pageSize/);
});

test('a page past the end is answered with the last page, and an empty list is page 1 of 1', () => {
    assert.match(paging, /Math\.Max\(1, \(totalCount \+ pageSize - 1\) \/ pageSize\)/);
    assert.match(paging, /Math\.Min\(Math\.Max\(1, pageNo \?\? 1\), totalPages\)/);
    assert.match(repo, /page\.PageNo = CommandOptionPaging\.ClampPageNo\(pageNo, page\.TotalPages\)/);
});

test('the endpoint is a GET next to the full list, which stays for the Postman checks', () => {
    assert.match(controller, /\[HttpGet\("command-config\/options\/page"\)\]/);
    assert.match(controller, /\[HttpGet\("command-config\/options"\)\]/);
    assert.match(controller, /GetCommandOptionsPage\(\[FromQuery\] string appname, \[FromQuery\] string\? command,/);
    assert.match(service, /NormalizeCommand\(command\) \?\? throw new CommandOptionException\("command must be Configure or SDK\."\)/);
});

test('the full list and the page share one row mapper, so both report the same record sources', () => {
    assert.strictEqual((repo.match(/ToOptionDtos\(/g) || []).length, 3);
    assert.match(repo, /var options = ToOptionDtos\(rowsResult\?\.data, prompts\);/);
    assert.match(repo, /page\.Items = ToOptionDtos\(rows\.data, prompts\);/);
});

test('the page fetches pages instead of the full list, and keeps a request id so a slow answer cannot overwrite a newer one', () => {
    const js = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'js', 'axiconfigpage.js');
    assert.ok(!/buildListUrl|groupByTab|filterRows/.test(js));
    assert.match(js, /fetch\(buildPageUrl\(apiBase, appName, tab, term, no\)\)/);
    assert.match(js, /if \(ask !== loadAsk \|\| tab !== active\) return;/);
    const html = read('AxpertPlugins', 'AxiCMD', 'HTMLPages', 'AxiCMDConfig.html');
    for (const id of ['acPager', 'acPageOf', 'acPrev', 'acPageNums', 'acNext']) assert.match(html, new RegExp('id="' + id + '"'), id);
});
