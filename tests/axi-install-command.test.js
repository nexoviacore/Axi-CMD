// Install Product / Plugin / ChangeRequest: install <type> <directory> [version].
// Like the other palette tests, this re-implements the pure helpers from axicmdmain.js with mock data,
// so keep the copies below identical to the helpers in the script.
const test = require('node:test');
const assert = require('node:assert');

// ---- mirrors of the helpers in axicmdmain.js ----------------------------------------------------------------
const AXI_INSTALL_QUEUE_NAME = "AxiInstallQueue";
const AXI_INSTALL_QUEUE_OK = "QUEUEDATAINSERTEDSUCESSFULLY";
const INSTALL_TYPES = { product: "product", plugin: "plugin", changerequest: "changerequest" };
const INSTALL_CACHE_TTL_MS = 30 * 60 * 1000;

function tokenizeInstallCommand(str) {
    const words = [];
    if (!str || typeof str !== "string") return words;
    const re = /"([^"]*)"?|([^\s"]+)/g;
    let m;
    while ((m = re.exec(str)) !== null) {
        words.push(m[1] !== undefined ? m[1] : m[2]);
    }
    return words;
}

function getInstallWords(str) {
    const words = tokenizeInstallCommand(str);
    const text = typeof str === "string" ? str : "";
    const quoteCount = (text.match(/"/g) || []).length;
    if (/\s$/.test(text) && quoteCount % 2 === 0) words.push("");
    return words;
}

function getInstallManagerType(typeWord) {
    const key = String(typeWord == null ? "" : typeWord).replace(/["\s]/g, "").toLowerCase();
    return INSTALL_TYPES[key] || "";
}

function validateInstallArgs(words, directories) {
    const fail = (message) => ({ ok: false, message, directory: "", version: "" });
    const list = Array.isArray(directories) ? directories : [];
    if (!words || words.length < 2 || !getInstallManagerType(words[1])) return fail("Install expects product, plugin or changerequest");
    const dirWord = words[2] === undefined || words[2] === null ? "" : String(words[2]).trim();
    if (!dirWord) return fail("Directory is required");
    const dir = list.find(d => String(d.name).toLowerCase() === dirWord.toLowerCase());
    if (!dir) return fail(`Directory '${dirWord}' was not found in the repository`);
    const versions = Array.isArray(dir.versions) ? dir.versions : [];
    const extra = words.slice(versions.length > 0 ? 4 : 3).filter(w => String(w).trim() !== "");
    if (extra.length > 0) {
        return fail(versions.length > 0
            ? "Only a directory and a version are expected after the type"
            : `'${dir.name}' has no versions, only the directory is expected after the type`);
    }
    if (versions.length === 0) return { ok: true, message: "", directory: dir.name, version: "" };
    const verWord = words[3] === undefined || words[3] === null ? "" : String(words[3]).trim();
    if (!verWord) return fail(`Version is required for '${dir.name}'`);
    const ver = versions.find(v => String(v).toLowerCase() === verWord.toLowerCase());
    if (!ver) return fail(`Version '${verWord}' was not found for '${dir.name}'`);
    return { ok: true, message: "", directory: dir.name, version: ver };
}

function buildInstallCacheKey(type, account, repo) {
    return `axi_install_dirs_${type}_${account}_${repo}_v1`;
}

function readInstallCacheEntry(raw, now) {
    if (!raw) return null;
    try {
        const entry = JSON.parse(raw);
        if (entry && typeof entry.t === "number" && entry.d && (now - entry.t) < INSTALL_CACHE_TTL_MS) return entry;
    } catch (e) { }
    return null;
}

function getInstallProblemMessage(status, text) {
    let obj = null;
    try { obj = JSON.parse(text); } catch (e) { }
    const msg = obj && typeof obj === "object" ? (obj.detail || obj.title || obj.message || "") : "";
    return String(msg || (obj === null ? text : "") || "").trim() || `Request failed (HTTP ${status})`;
}

function generateInstallRequestId() {
    try {
        if (typeof crypto !== "undefined" && crypto && typeof crypto.randomUUID === "function") return crypto.randomUUID();
    } catch (e) { }
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, c => {
        const r = Math.floor(Math.random() * 16);
        return (c === "x" ? r : ((r % 4) + 8)).toString(16);
    });
}

function buildInstallPayload(p) {
    return {
        action: "install",
        requestId: p.requestId || "",
        type: p.type,
        account: p.account,
        repo: p.repo,
        managerType: p.type,
        directory: p.directory,
        version: p.version || "",
        commitSha: p.commitSha || "",
        appname: p.appname,
        username: p.username,
        requestedOn: p.requestedOn || (p.now instanceof Date ? p.now : new Date()).toISOString()
    };
}

function buildInstallQueueBody(payload) {
    return { queuename: AXI_INSTALL_QUEUE_NAME, queuedata: JSON.stringify(payload), queuejson: payload, trace: false };
}

function parseInstallQueueResult(ok, status, text) {
    const body = typeof text === "string" ? text : "";
    if (ok && body.indexOf(AXI_INSTALL_QUEUE_OK) !== -1) return { success: true, message: "" };
    let obj = null;
    try { obj = JSON.parse(body); } catch (e) { }
    let msg = "";
    if (obj && typeof obj === "object") msg = obj.message || obj.Message || obj.error || obj.detail || obj.title || "";
    else if (typeof obj === "string") msg = obj;
    else msg = body;
    msg = String(msg || "").trim();
    if (!msg) msg = ok ? "The queue did not confirm the request" : `Request failed (HTTP ${status})`;
    return { success: false, message: msg };
}

// ---- tests --------------------------------------------------------------------------------------------------
const dirs = [
    { name: 'AxpertWeb', versions: ['Latest', 'Version2', 'Version1'] },
    { name: 'My Dir', versions: ['Latest', 'Version1'] },
    { name: 'Tools', versions: [] },
];

test('tokenizer: a quoted directory is one word and the quotes are stripped', () => {
    assert.deepStrictEqual(tokenizeInstallCommand('install product "My Dir" Latest'), ['install', 'product', 'My Dir', 'Latest']);
    assert.deepStrictEqual(tokenizeInstallCommand('  install   plugin Tools '), ['install', 'plugin', 'Tools']);
    assert.deepStrictEqual(tokenizeInstallCommand('install product "My Di'), ['install', 'product', 'My Di']);
    assert.deepStrictEqual(tokenizeInstallCommand(''), []);
    assert.deepStrictEqual(tokenizeInstallCommand(null), []);
});

test('getInstallWords: a trailing space opens a new empty word, except inside an open quote', () => {
    assert.deepStrictEqual(getInstallWords('install product '), ['install', 'product', '']);
    assert.deepStrictEqual(getInstallWords('install product "My Dir" '), ['install', 'product', 'My Dir', '']);
    assert.deepStrictEqual(getInstallWords('install product "My '), ['install', 'product', 'My ']);
    assert.deepStrictEqual(getInstallWords('install product Tools'), ['install', 'product', 'Tools']);
});

test('managerType mapping is lower case and tolerant of quotes and case', () => {
    assert.strictEqual(getInstallManagerType('Product'), 'product');
    assert.strictEqual(getInstallManagerType('PLUGIN'), 'plugin');
    assert.strictEqual(getInstallManagerType('ChangeRequest'), 'changerequest');
    assert.strictEqual(getInstallManagerType('"Change Request"'), 'changerequest');
    assert.strictEqual(getInstallManagerType('tstruct'), '');
    assert.strictEqual(getInstallManagerType(undefined), '');
});

test('validation: a directory with versions needs one of them', () => {
    const r = validateInstallArgs(['install', 'product', 'AxpertWeb'], dirs);
    assert.strictEqual(r.ok, false);
    assert.match(r.message, /Version is required/);
    assert.deepStrictEqual(validateInstallArgs(['install', 'product', 'axpertweb', 'latest'], dirs), { ok: true, message: '', directory: 'AxpertWeb', version: 'Latest' });
    assert.strictEqual(validateInstallArgs(['install', 'product', 'AxpertWeb', 'Version9'], dirs).ok, false);
});

test('validation: a directory without versions takes none', () => {
    assert.deepStrictEqual(validateInstallArgs(['install', 'plugin', 'Tools'], dirs), { ok: true, message: '', directory: 'Tools', version: '' });
    const r = validateInstallArgs(['install', 'plugin', 'Tools', 'Latest'], dirs);
    assert.strictEqual(r.ok, false);
    assert.match(r.message, /no versions/);
});

test('validation: quoted directory, unknown directory and extra words', () => {
    const words = tokenizeInstallCommand('install product "My Dir" Version1');
    assert.deepStrictEqual(validateInstallArgs(words, dirs), { ok: true, message: '', directory: 'My Dir', version: 'Version1' });
    assert.strictEqual(validateInstallArgs(['install', 'product', 'Nope'], dirs).ok, false);
    assert.strictEqual(validateInstallArgs(['install', 'product'], dirs).ok, false);
    assert.strictEqual(validateInstallArgs(['install', 'product', 'AxpertWeb', 'Latest', 'extra'], dirs).ok, false);
    assert.strictEqual(validateInstallArgs(['install', 'bogus', 'Tools'], dirs).ok, false);
    assert.strictEqual(validateInstallArgs(['install', 'product', 'Tools'], []).ok, false);
});

test('cache key and TTL', () => {
    assert.strictEqual(buildInstallCacheKey('product', 'nexoviacore', 'axpert.web'), 'axi_install_dirs_product_nexoviacore_axpert.web_v1');
    const now = 1_000_000_000;
    const fresh = JSON.stringify({ t: now - 29 * 60 * 1000, d: { commitSha: 'abc', directories: [] } });
    const stale = JSON.stringify({ t: now - 31 * 60 * 1000, d: { commitSha: 'abc', directories: [] } });
    assert.strictEqual(readInstallCacheEntry(fresh, now).d.commitSha, 'abc');
    assert.strictEqual(readInstallCacheEntry(stale, now), null);
    assert.strictEqual(readInstallCacheEntry('not json', now), null);
    assert.strictEqual(readInstallCacheEntry(null, now), null);
    assert.strictEqual(readInstallCacheEntry(JSON.stringify({ t: now }), now), null);
});

test('AxiGit error message prefers detail, then title', () => {
    assert.strictEqual(getInstallProblemMessage(404, JSON.stringify({ title: 'Not Found', detail: 'Repo x not found' })), 'Repo x not found');
    assert.strictEqual(getInstallProblemMessage(502, JSON.stringify({ title: 'Bad Gateway' })), 'Bad Gateway');
    assert.strictEqual(getInstallProblemMessage(500, 'boom'), 'boom');
    assert.strictEqual(getInstallProblemMessage(500, ''), 'Request failed (HTTP 500)');
});

test('request id: uuid shaped, unique, and a fallback when crypto.randomUUID is missing', () => {
    const uuid = /^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/;
    const a = generateInstallRequestId();
    assert.match(a, uuid);
    assert.notStrictEqual(a, generateInstallRequestId());
    const saved = Object.getOwnPropertyDescriptor(globalThis, 'crypto');
    Object.defineProperty(globalThis, 'crypto', { value: undefined, configurable: true });
    try {
        const b = generateInstallRequestId();
        assert.match(b, uuid);
        assert.notStrictEqual(b, generateInstallRequestId());
    } finally {
        Object.defineProperty(globalThis, 'crypto', saved);
    }
});

test('queue payload and body', () => {
    const payload = buildInstallPayload({ requestId: 'req-1', type: 'plugin', account: 'acc', repo: 'rep', directory: 'My Dir', version: undefined, commitSha: 'abcdef1234', appname: 'app', username: 'admin', requestedOn: '2026-01-02T03:04:05.000Z' });
    assert.deepStrictEqual(payload, {
        action: 'install', requestId: 'req-1', type: 'plugin', account: 'acc', repo: 'rep', managerType: 'plugin', directory: 'My Dir', version: '',
        commitSha: 'abcdef1234', appname: 'app', username: 'admin', requestedOn: '2026-01-02T03:04:05.000Z'
    });
    const body = buildInstallQueueBody(payload);
    assert.strictEqual(body.queuename, 'AxiInstallQueue');
    assert.strictEqual(body.trace, false);
    assert.strictEqual(body.queuejson, payload);
    assert.deepStrictEqual(JSON.parse(body.queuedata), payload);
});

test('retry payload keeps the same requestId and requestedOn', () => {
    const base = { requestId: generateInstallRequestId(), requestedOn: '2026-01-02T03:04:05.000Z', type: 'product', account: 'a', repo: 'r', directory: 'd', version: 'Latest', commitSha: 'c', appname: 'x', username: 'u' };
    const first = buildInstallPayload(base);
    const retry = buildInstallPayload(Object.assign({}, base, { commitSha: 'newer' }));
    assert.strictEqual(retry.requestId, first.requestId);
    assert.strictEqual(retry.requestedOn, first.requestedOn);
});

test('payload without requestedOn falls back to now; missing requestId is empty', () => {
    const p = buildInstallPayload({ type: 'product', now: new Date('2026-05-06T00:00:00.000Z') });
    assert.strictEqual(p.requestedOn, '2026-05-06T00:00:00.000Z');
    assert.strictEqual(p.requestId, '');
});

test('validation keeps brackets and parentheses in directory and version names', () => {
    const list = [{ name: 'App (beta) [x]', versions: ['Latest', 'Version1 (rc)'] }];
    const words = tokenizeInstallCommand('install product "App (beta) [x]" "Version1 (rc)"');
    assert.deepStrictEqual(validateInstallArgs(words, list), { ok: true, message: '', directory: 'App (beta) [x]', version: 'Version1 (rc)' });
});

test('queue answer: plain text, JSON-wrapped, and failures', () => {
    assert.deepStrictEqual(parseInstallQueueResult(true, 200, 'QUEUEDATAINSERTEDSUCESSFULLY'), { success: true, message: '' });
    assert.strictEqual(parseInstallQueueResult(true, 200, JSON.stringify({ success: true, result: { message: 'QUEUEDATAINSERTEDSUCESSFULLY' } })).success, true);
    assert.strictEqual(parseInstallQueueResult(true, 200, JSON.stringify('QUEUEDATAINSERTEDSUCESSFULLY')).success, true);
    assert.strictEqual(parseInstallQueueResult(false, 500, 'QUEUEDATAINSERTEDSUCESSFULLY').success, false);
    assert.deepStrictEqual(parseInstallQueueResult(true, 200, 'queue not found'), { success: false, message: 'queue not found' });
    assert.deepStrictEqual(parseInstallQueueResult(false, 400, JSON.stringify({ message: 'bad request' })), { success: false, message: 'bad request' });
    assert.strictEqual(parseInstallQueueResult(false, 503, '').message, 'Request failed (HTTP 503)');
    assert.strictEqual(parseInstallQueueResult(true, 200, '').message, 'The queue did not confirm the request');
});
