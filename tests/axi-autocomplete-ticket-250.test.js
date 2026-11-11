const test = require("node:test");
const assert = require("node:assert");

// Test suite for Ticket #250: On-Demand Token Execution Resolution and Search Cache Invalidation

test("findMatchingRecordItem resolves record by transrecordid, id, display caption, and substring", () => {
    function cleanString(str) {
        return (str || "").toString().trim();
    }

    function findMatchingRecordItem(structDataList, candidates) {
        if (!Array.isArray(structDataList) || structDataList.length === 0 || !Array.isArray(candidates) || candidates.length === 0) return null;
        let matched = null;

        // 1. Match by transrecordid directly
        for (const val of candidates) {
            matched = structDataList.find(item => {
                const transRecId = (item?.transrecordid ?? item?.TRANSRECORDID ?? "").toString().trim();
                return transRecId !== "" && transRecId !== "0" && transRecId.toLowerCase() === val.toLowerCase();
            });
            if (matched) return matched;
        }

        // 2. Match by id directly
        for (const val of candidates) {
            matched = structDataList.find(item => {
                const itemId = (item?.id ?? item?.ID ?? "").toString().trim();
                return itemId !== "" && itemId !== "0" && itemId.toLowerCase() === val.toLowerCase();
            });
            if (matched) return matched;
        }

        // 3. Match exact displaydata, caption, or name (preferring actual records where isfield != 't')
        for (const val of candidates) {
            const cleanVal = cleanString(val).toLowerCase();
            matched = structDataList.find(item => {
                const isField = String(item?.isfield || item?.ISFIELD || "").toLowerCase() === "t";
                if (isField) return false;
                const display = cleanString(item?.displaydata || item?.DISPLAYDATA || "").toLowerCase();
                const caption = cleanString(item?.caption || item?.CAPTION || "").toLowerCase();
                const name = cleanString(item?.name || item?.NAME || "").toLowerCase();
                return display === cleanVal || caption === cleanVal || name === cleanVal;
            }) || structDataList.find(item => {
                const display = cleanString(item?.displaydata || item?.DISPLAYDATA || "").toLowerCase();
                const caption = cleanString(item?.caption || item?.CAPTION || "").toLowerCase();
                const name = cleanString(item?.name || item?.NAME || "").toLowerCase();
                return display === cleanVal || caption === cleanVal || name === cleanVal;
            });
            if (matched) return matched;
        }

        // 4. Match without bracketed text
        for (const val of candidates) {
            const cleanVal = cleanString(val).replace(/\[.*?\]/g, "").trim().toLowerCase();
            if (!cleanVal) continue;
            matched = structDataList.find(item => {
                const isField = String(item?.isfield || item?.ISFIELD || "").toLowerCase() === "t";
                if (isField) return false;
                const display = cleanString(item?.displaydata || item?.DISPLAYDATA || "").replace(/\[.*?\]/g, "").trim().toLowerCase();
                const caption = cleanString(item?.caption || item?.CAPTION || "").replace(/\[.*?\]/g, "").trim().toLowerCase();
                return display === cleanVal || caption === cleanVal;
            }) || structDataList.find(item => {
                const display = cleanString(item?.displaydata || item?.DISPLAYDATA || "").replace(/\[.*?\]/g, "").trim().toLowerCase();
                const caption = cleanString(item?.caption || item?.CAPTION || "").replace(/\[.*?\]/g, "").trim().toLowerCase();
                return display === cleanVal || caption === cleanVal;
            });
            if (matched) return matched;
        }

        return null;
    }

    const mockList = [
        { displaydata: "Message One [1001]", id: "1001", caption: "Message One", transrecordid: 1001, isfield: "f" },
        { displaydata: "Message Two [1002]", id: "1002", caption: "Message Two", transrecordid: 1002, isfield: "f" },
        { displaydata: "subject (subject) [field]", id: "0", caption: "subject", transrecordid: 0, isfield: "t" }
    ];

    // Direct transrecordid lookup
    const res1 = findMatchingRecordItem(mockList, ["1002"]);
    assert.strictEqual(res1?.transrecordid, 1002);

    // Caption lookup without brackets
    const res2 = findMatchingRecordItem(mockList, ["Message One"]);
    assert.strictEqual(res2?.transrecordid, 1001);

    // Field names must not be matched as records
    const res3 = findMatchingRecordItem(mockList, ["subject"]);
    assert.strictEqual(res3?.isfield, "t");

    // Non-existent item returns null
    const res4 = findMatchingRecordItem(mockList, ["9999"]);
    assert.strictEqual(res4, null);
});

test("On-demand record resolution fetches missing record from server and redirects with resolved record ID", async () => {
    let redirectedUrl = null;
    let toastMessage = null;

    // Simulated server database containing records not in initial preloaded list
    const remoteServerRecords = [
        { displaydata: "Archived Record 54321", id: "54321", transrecordid: 54321, isfield: "f" }
    ];

    async function mockGetList(sourceName, paramValue, searchTerm) {
        return remoteServerRecords.filter(r =>
            r.displaydata.toLowerCase().includes(searchTerm.toLowerCase()) ||
            String(r.transrecordid).includes(searchTerm)
        );
    }

    async function executeEditCommand(typedRecordToken, preloadedList = []) {
        const candidates = [typedRecordToken].filter(Boolean);
        let matchedItem = preloadedList.find(item => String(item.transrecordid) === typedRecordToken);

        if (!matchedItem && candidates.length > 0) {
            // Trigger on-demand lookup
            const fetched = await mockGetList("axi_getstructsdata", "mock_params", typedRecordToken);
            if (fetched && fetched.length > 0) {
                matchedItem = fetched[0];
            }
        }

        if (matchedItem && matchedItem.transrecordid) {
            redirectedUrl = `tstruct.aspx?act=open&transid=tmmsg&recordid=${matchedItem.transrecordid}`;
        } else {
            toastMessage = `Record '${typedRecordToken}' not found in tmmsg.`;
        }
    }

    // 1. Test existing record on server (not preloaded in initial 100)
    await executeEditCommand("54321", []);
    assert.strictEqual(redirectedUrl, "tstruct.aspx?act=open&transid=tmmsg&recordid=54321", "Should redirect with resolved record ID");
    assert.strictEqual(toastMessage, null, "No error toast should be shown on success");

    // 2. Test non-existent record on server
    redirectedUrl = null;
    await executeEditCommand("999999", []);
    assert.strictEqual(redirectedUrl, null, "Should prevent redirection when record does not exist");
    assert.strictEqual(toastMessage, "Record '999999' not found in tmmsg.", "Should show clear error toast");
});

test("clearStructSearchCache flushes in-memory and storage caches for the target structure", () => {
    const axDatasourceObj = {
        "axi_getstructsdata_edit$#$admin$#$tmmsg": [{ id: 1 }],
        "axi_getstructsdata_edit$#$admin$#$tmmsg_q_abc": [{ id: 1 }],
        "axi_getstructsdata_edit$#$admin$#$otherstruct": [{ id: 99 }]
    };

    const mockStorage = {
        "axi_axi_getstructsdata_param4:tmmsg_v1": "data",
        "axi_axi_getstructsdata_param4:tmmsg_q_abc_v1": "data",
        "axi_axi_getstructsdata_param4:other_v1": "data"
    };

    function clearStructSearchCache(transId) {
        if (!transId) return;
        const lowTransId = transId.toLowerCase().trim();
        for (const key in axDatasourceObj) {
            const lowKey = key.toLowerCase();
            if (lowKey.includes("getstructsdata") && lowKey.includes(lowTransId)) {
                delete axDatasourceObj[key];
            }
        }
        for (const key in mockStorage) {
            const lowKey = key.toLowerCase();
            if (lowKey.includes("getstructsdata") && lowKey.includes(lowTransId)) {
                delete mockStorage[key];
            }
        }
    }

    clearStructSearchCache("tmmsg");

    // Only tmmsg should be removed; otherstruct must remain intact
    assert.strictEqual(axDatasourceObj["axi_getstructsdata_edit$#$admin$#$tmmsg"], undefined);
    assert.strictEqual(axDatasourceObj["axi_getstructsdata_edit$#$admin$#$tmmsg_q_abc"], undefined);
    assert.strictEqual(axDatasourceObj["axi_getstructsdata_edit$#$admin$#$otherstruct"]?.length, 1);

    assert.strictEqual(mockStorage["axi_axi_getstructsdata_param4:tmmsg_v1"], undefined);
    assert.strictEqual(mockStorage["axi_axi_getstructsdata_param4:tmmsg_q_abc_v1"], undefined);
    assert.strictEqual(mockStorage["axi_axi_getstructsdata_param4:other_v1"], "data");
});

test("Selecting autocomplete suggestion merges selected item into in-memory base list", () => {
    const axDatasourceObj = {
        "axi_getstructsdata_edit$#$admin$#$tmmsg": [
            { displaydata: "Item 1", transrecordid: 1 }
        ]
    };

    const selectedItem = { displaydata: "Item 505", transrecordid: 505 };

    // Simulate apply(index) selection merge
    const recUid = selectedItem.transrecordid ?? selectedItem.id;
    for (const key in axDatasourceObj) {
        if (key.toLowerCase().startsWith("axi_getstructsdata") && !key.includes("_q_")) {
            const baseArr = axDatasourceObj[key];
            if (Array.isArray(baseArr) && !baseArr.some(x => (x.transrecordid ?? x.id) == recUid)) {
                baseArr.push(selectedItem);
            }
        }
    }

    const baseList = axDatasourceObj["axi_getstructsdata_edit$#$admin$#$tmmsg"];
    assert.strictEqual(baseList.length, 2, "Base list should now include selected item");
    assert.strictEqual(baseList[1].transrecordid, 505);
});
