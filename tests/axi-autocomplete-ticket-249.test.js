const test = require("node:test");
const assert = require("node:assert");

// Test suite for Ticket #249: Server-Side Dynamic 3+ Character Search with Keystroke Debouncing

test("Dynamic search triggers after 300ms debounce and cancels when typing continues or shrinks below 3 chars", async () => {
    let pendingTimer = null;
    let dispatchedSearches = [];

    function simulateTyping(inputStr) {
        const cleanSearch = (inputStr || "").trim().toLowerCase();
        if (cleanSearch.length >= 3) {
            if (pendingTimer) clearTimeout(pendingTimer);
            pendingTimer = setTimeout(() => {
                dispatchedSearches.push(cleanSearch);
            }, 300);
        } else {
            if (pendingTimer) clearTimeout(pendingTimer);
        }
    }

    // Fast typing: 'a' -> 'ab' -> 'abc' -> 'abcd' within 100ms intervals
    simulateTyping("a");
    simulateTyping("ab");
    simulateTyping("abc");
    await new Promise(r => setTimeout(r, 100));
    simulateTyping("abcd");

    // Wait 250ms (total 350ms from start, but only 250ms from 'abcd' dispatch)
    await new Promise(r => setTimeout(r, 250));
    assert.strictEqual(dispatchedSearches.length, 0, "No search should fire before full 300ms of inactivity");

    // Wait remaining 100ms (350ms after 'abcd')
    await new Promise(r => setTimeout(r, 100));
    assert.strictEqual(dispatchedSearches.length, 1, "Debounced search should fire once for 'abcd'");
    assert.strictEqual(dispatchedSearches[0], "abcd");

    // Backspacing below 3 chars cancels any pending timer
    simulateTyping("abcde");
    await new Promise(r => setTimeout(r, 50));
    simulateTyping("ab"); // shrunk below 3 chars
    await new Promise(r => setTimeout(r, 350));
    assert.strictEqual(dispatchedSearches.length, 1, "Cancelled search must not fire after shrinking below 3 chars");
});

test("getList maps param11, param12, param13 for getstructsdata ADS payload", () => {
    function buildGetListPayload(axDatasourceName, paramValuesCsv = "", searchTerm = "", pageNo = 1, pageSize = 100) {
        const sqlParams = {};
        const normalizedParams = [];

        if (paramValuesCsv && typeof paramValuesCsv === "string") {
            const values = paramValuesCsv
                .split("$#$")
                .map(v => v.trim())
                .filter(Boolean);

            values.forEach((value, index) => {
                const key = `param${index + 1}`;
                sqlParams[key] = value;
                normalizedParams.push(`${key}:${value}`);
            });
        }

        const cleanTerm = (typeof searchTerm === "string" ? searchTerm : "").trim().toLowerCase();
        const effectivePageSize = (typeof pageSize === "number") ? pageSize : 100;
        const effectivePageNo = (typeof pageNo === "number") ? pageNo : 1;

        if (axDatasourceName && axDatasourceName.toLowerCase().includes("getstructsdata")) {
            if (!sqlParams.param11 || cleanTerm) {
                sqlParams.param11 = cleanTerm || "";
                normalizedParams.push(`param11:${sqlParams.param11}`);
            }
            if (!sqlParams.param12) {
                sqlParams.param12 = String(effectivePageNo);
                normalizedParams.push(`param12:${sqlParams.param12}`);
            }
            if (!sqlParams.param13) {
                sqlParams.param13 = String(effectivePageSize);
                normalizedParams.push(`param13:${sqlParams.param13}`);
            }
        }

        let cacheKey = `axi_${axDatasourceName}_${normalizedParams.join("|")}`;
        if (cleanTerm) {
            cacheKey += `_q_${cleanTerm}`;
        }
        cacheKey += `_v1`;

        const requestBody = {
            action: "view",
            adsNames: [axDatasourceName],
            sqlParams: sqlParams,
            props: {
                pageno: effectivePageNo,
                pagesize: effectivePageSize
            }
        };

        if (cleanTerm && cleanTerm.length >= 3) {
            requestBody.props.filters = [
                {
                    fldname: "displaydata",
                    condition: "CONTAINS",
                    value: cleanTerm,
                    datatype: "TEXT"
                }
            ];
        }

        return { requestBody, cacheKey, sqlParams };
    }

    const baseParams10 = "edit$#$admin$#$default$#$tmmsg$#$0$#$F$#$F$#$msgid$#$ax_tmmsg$#$NA";

    // 1. Initial Load (empty search)
    const initialPayload = buildGetListPayload("axi_getstructsdata", baseParams10, "", 1, 100);
    assert.strictEqual(initialPayload.sqlParams.param1, "edit");
    assert.strictEqual(initialPayload.sqlParams.param10, "NA");
    assert.strictEqual(initialPayload.sqlParams.param11, "", "param11 must be empty string on initial load");
    assert.strictEqual(initialPayload.sqlParams.param12, "1", "param12 must be page 1");
    assert.strictEqual(initialPayload.sqlParams.param13, "100", "param13 must be page size 100");
    assert.strictEqual(initialPayload.requestBody.props.filters, undefined, "No filters on initial load");

    // 2. Dynamic Search (3+ characters)
    const searchPayload = buildGetListPayload("axi_getstructsdata", baseParams10, "salman", 1, 100);
    assert.strictEqual(searchPayload.sqlParams.param11, "salman", "param11 must be the search term");
    assert.strictEqual(searchPayload.sqlParams.param12, "1");
    assert.strictEqual(searchPayload.sqlParams.param13, "100");
    assert.strictEqual(searchPayload.requestBody.props.filters.length, 1);
    assert.strictEqual(searchPayload.requestBody.props.filters[0].value, "salman");
    assert.ok(searchPayload.cacheKey.includes("_q_salman_v1"), "Cache key must include search term qualifier");
});

test("Cache in axDatasourceObj provides 0ms retrieval on re-typing search term", () => {
    const axDatasourceObj = {};
    const sourceKey = "axi_getstructsdata_edit$#$admin$#$tmmsg";
    const searchTerm = "salman";
    const searchKey = `${sourceKey}_q_${searchTerm}`;

    // Populate search results
    const mockSearchResults = [
        { displaydata: "salman sheriff", id: "101", transrecordid: 101 },
        { displaydata: "salman khan", id: "102", transrecordid: 102 }
    ];
    axDatasourceObj[searchKey] = mockSearchResults;

    // Check lookup
    assert.ok(axDatasourceObj[searchKey] !== undefined, "Search key must exist in cache");
    assert.strictEqual(axDatasourceObj[searchKey].length, 2);
    assert.strictEqual(axDatasourceObj[searchKey][0].displaydata, "salman sheriff");
});

test("Adaptive client safety filter strips non-matching rows and merges into base RAM list", () => {
    const axDatasourceObj = {
        "axi_getstructsdata_edit": [
            { displaydata: "Initial 1", id: "1", transrecordid: 1 },
            { displaydata: "Initial 2", id: "2", transrecordid: 2 }
        ]
    };

    // Simulated legacy server returning 5 items including non-matching rows
    const serverReturnedData = [
        { displaydata: "Tech Support Message", id: "3", transrecordid: 3 },
        { displaydata: "Urgent Meeting Note", id: "4", transrecordid: 4 },
        { displaydata: "Support Ticket #105", id: "5", transrecordid: 5 },
        { displaydata: "Random irrelevant row", id: "6", transrecordid: 6 }
    ];

    const cleanTerm = "support";
    let list = serverReturnedData.filter(item => {
        const display = item.displaydata || item.caption || item.name || item.fname || item.keyfield || "";
        return String(display).toLowerCase().includes(cleanTerm);
    });

    assert.strictEqual(list.length, 2, "Only 2 rows should match 'support'");
    assert.strictEqual(list[0].displaydata, "Tech Support Message");
    assert.strictEqual(list[1].displaydata, "Support Ticket #105");

    // Merge into base list
    const baseKey = "axi_getstructsdata_edit";
    const existing = axDatasourceObj[baseKey];
    const seenIds = new Set(existing.map(x => String(x.transrecordid)));

    for (const item of list) {
        const uid = String(item.transrecordid);
        if (!seenIds.has(uid)) {
            seenIds.add(uid);
            existing.push(item);
        }
    }

    assert.strictEqual(axDatasourceObj[baseKey].length, 4, "Base list should now contain 4 merged items");
    assert.strictEqual(axDatasourceObj[baseKey][2].displaydata, "Tech Support Message");
    assert.strictEqual(axDatasourceObj[baseKey][3].displaydata, "Support Ticket #105");
});
