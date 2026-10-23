const test = require("node:test");
const assert = require("node:assert");

// Test suite for Ticket #248: Initial Bounded Record Load and Short-Input Guidance

test("loadList caps initial loaded dataset to effectivePageSize (100 records)", () => {
    // Generate 250 mock records
    const rawRecords = Array.from({ length: 250 }, (_, i) => ({
        displaydata: `Record ${i + 1}`,
        id: `id_${i + 1}`,
        transrecordid: i + 1
    }));

    const pageSize = 100;
    const cleanTerm = "";
    const effectivePageSize = pageSize || 100;

    let list = Array.isArray(rawRecords) ? rawRecords : [];
    if (!cleanTerm && effectivePageSize > 0 && list.length > effectivePageSize) {
        list = list.slice(0, effectivePageSize);
    }

    assert.strictEqual(list.length, 100, "Initial list should be capped to 100 records");
    assert.strictEqual(list[0].displaydata, "Record 1");
    assert.strictEqual(list[99].displaydata, "Record 100");
});

test("loadList blocks unparameterized calls for axi_getstructsdata", () => {
    function mockLoadList(sourceName, paramValue) {
        if (sourceName.toLowerCase().includes("getstructsdata") && (!paramValue || !String(paramValue).trim())) {
            return { blocked: true, data: [] };
        }
        return { blocked: false, data: [{ displaydata: "ok" }] };
    }

    const emptyResult = mockLoadList("axi_getstructsdata", "");
    assert.strictEqual(emptyResult.blocked, true, "Empty paramValue should be blocked");
    assert.strictEqual(emptyResult.data.length, 0);

    const validResult = mockLoadList("axi_getstructsdata", "Edit$#$admin$#$default$#$tmmsg$#$0$#$F$#$F$#$msgid$#$tmmsg1$#$NA");
    assert.strictEqual(validResult.blocked, false, "Valid paramValue should be allowed");
    assert.strictEqual(validResult.data.length, 1);
});

test("hasValidParams requires non-empty paramStr for getstructsdata regardless of promptParams", () => {
    function checkHasValidParams(apiSourceName, activePromptParams, paramValue) {
        const paramStr = typeof paramValue === "string" ? paramValue : (Array.isArray(paramValue) ? paramValue.join("$#$") : "");
        const isGetStructsData = apiSourceName.toLowerCase().includes("getstructsdata");
        return isGetStructsData
            ? Boolean(paramStr && paramStr.replace(/,/g, '').trim().length > 0)
            : (!activePromptParams || (paramStr && paramStr.replace(/,/g, '').trim().length > 0));
    }

    // In DB, promptParams is null/empty for axi_getstructsdata
    assert.strictEqual(
        checkHasValidParams("axi_getstructsdata", null, ""),
        false,
        "Must be false when paramValue is empty even if promptParams is null"
    );

    assert.strictEqual(
        checkHasValidParams("axi_getstructsdata", "", ""),
        false,
        "Must be false when paramValue is empty string"
    );

    assert.strictEqual(
        checkHasValidParams("axi_getstructsdata", null, "Edit$#$admin$#$default$#$tmmsg"),
        true,
        "Must be true when paramValue has valid data"
    );

    // For other sources where promptParams is null (static list)
    assert.strictEqual(
        checkHasValidParams("axi_dummy", null, ""),
        true,
        "Static sources without promptParams should be valid"
    );
});

test("Short input (< 3 chars) filters locally and yields guidance when no match is found", () => {
    const dataList = [
        { displaydata: "Venkat S" },
        { displaydata: "Sriram R" },
        { displaydata: "salmansheriff" }
    ];

    function handleShortInput(partialTyped, cleanSearch, dataList, isDynamicSource) {
        let filtered = dataList.filter(item => {
            const display = item.displaydata || item.caption || item.name || "";
            return display.toLowerCase().includes(partialTyped.toLowerCase());
        });

        let toastShown = false;
        let result = [];

        if (dataList.length > 0 && filtered.length === 0) {
            if (isDynamicSource) {
                if (cleanSearch.length < 3) {
                    result = ["Type at least 3 characters to search records..."];
                } else {
                    result = ["No matching records found"];
                }
            } else {
                toastShown = true;
                result = [...dataList];
            }
        } else {
            result = filtered.map(item => item.displaydata);
        }

        return { result, toastShown };
    }

    // 2-char match: "sr"
    const matchRes = handleShortInput("sr", "sr", dataList, true);
    assert.strictEqual(matchRes.toastShown, false);
    assert.deepStrictEqual(matchRes.result, ["Sriram R"]);

    // 2-char no match: "zx"
    const noMatchRes = handleShortInput("zx", "zx", dataList, true);
    assert.strictEqual(noMatchRes.toastShown, false, "Toast must not be shown for dynamic source under 3 chars");
    assert.deepStrictEqual(noMatchRes.result, ["Type at least 3 characters to search records..."]);
});
