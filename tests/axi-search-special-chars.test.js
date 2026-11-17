// The AxList search (displaydata CONTAINS) returns no rows for a term holding ! @ # $ % ^ & * ( ).
// Checked on the server: "spchar!@#$%^&*()" -> 0 rows, "spchar" -> the form "SPChar!@#$%^&*() (Rel53)".
// So "SPChar!@#$%^&*() Edit" never found the form (beyond page 1 of the structure list) and sent no
// axi_getstructsdata call. getList now sends serverSearchTerm(term); loadList still narrows to the full term.
// These tests load the real function from axicmdmain.js (not a copy).
const test = require("node:test");
const assert = require("node:assert");
const fs = require("fs");
const path = require("path");

const src = fs.readFileSync(path.join(__dirname, "../AxpertPlugins/AxiCMD/HTMLPages/js/axicmdmain.js"), "utf8");

function extract(name) {
    const start = src.indexOf(`    function ${name}(`);
    assert.ok(start > -1, `${name} not found in axicmdmain.js`);
    let depth = 0;
    for (let i = src.indexOf("{", start); i < src.length; i++) {
        if (src[i] === "{") depth++;
        else if (src[i] === "}" && --depth === 0) return src.slice(start, i + 1);
    }
    throw new Error(`could not read ${name}`);
}
const charsLine = src.match(/const SERVER_SEARCH_UNSAFE_CHARS = (\/.*\/);/);
assert.ok(charsLine, "SERVER_SEARCH_UNSAFE_CHARS not found");
const serverSearchTerm = new Function(`const SERVER_SEARCH_UNSAFE_CHARS = ${charsLine[1]};\n${extract("serverSearchTerm")}\nreturn serverSearchTerm;`)();

test("a caption with special characters is searched by its longest plain part", () => {
    assert.strictEqual(serverSearchTerm("spchar!@#$%^&*()"), "spchar");
    assert.strictEqual(serverSearchTerm("spchar!@#$%^&*() edit"), "spchar");
    assert.strictEqual(serverSearchTerm("smशशि)(*&^/#$7"), "smशशि");
});

test("plain terms are sent unchanged (no change for captions that already worked)", () => {
    for (const t of ["smoke55", "test form2", "sm53t", "city form", "dropdown all moe's", "app global search data/periodically", "ab"]) {
        assert.strictEqual(serverSearchTerm(t), t);
    }
});

test("no searchable part left: no server filter", () => {
    assert.strictEqual(serverSearchTerm("@#$"), "");
    assert.strictEqual(serverSearchTerm("ab!cd"), "");
});

test("getList sends serverSearchTerm, loadList still narrows the rows to the full typed term", () => {
    assert.match(src, /const serverTerm = cleanTerm \? serverSearchTerm\(cleanTerm\) : "";/);
    assert.match(src, /value: serverTerm,/);
    assert.match(src, /return String\(display\)\.toLowerCase\(\)\.includes\(cleanTerm\);/);
});
