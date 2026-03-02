const LS_KEY = "axpert_chats_v2";
const AI_LOGO_SRC = "../../../plugins/Axi/HTMLPages/images/axibot.png";
// OpenRouter API key (DEV ONLY — do not ship keys in frontend)
const OPENROUTERAPIKEY = "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";
const logoUrl = "../../../plugins/Axi/HTMLPages/images/axibot.png";

// ==============================
// AXI Provider/Auth (USER KEY)
// ==============================
const AXI_LS_PROVIDER = "axi_provider";  // set by axi-connect.html
const AXI_LS_KEY = "axi_api_key";        // set by axi-connect.html
const AXI_LS_MODEL = "axi_model";        // optional (if you later store a model id)

function getAxiConfig() {
    const provider = (localStorage.getItem(AXI_LS_PROVIDER) || "openai").trim().toLowerCase();
    const apiKey = (localStorage.getItem(AXI_LS_KEY) || "").trim();
    const modelFromLs = (localStorage.getItem(AXI_LS_MODEL) || "").trim();

    if (!apiKey) {
        throw new Error("No API key found. Type 'AXI CONNECT' and connect your provider key.");
    }

    // Safe defaults (you can change these later)
    let model = modelFromLs;
    if (!model) {
        if (provider === "openai") model = "gpt-4o-mini";
        else if (provider === "openrouter") model = "openai/gpt-4o-mini";
        else if (provider === "gemini") model = "gemini-1.5-flash";
        else if (provider === "anthropic") model = "claude-3-5-sonnet-latest";
        else model = "gpt-4o-mini";
    }

    return { provider, apiKey, model };
}

function handleAuthFailure(provider, resStatus) {
    // If user revoked/rotated key in provider dashboard, clear locally so they must reconnect
    if (resStatus === 401 || resStatus === 403) {
        localStorage.removeItem(AXI_LS_KEY);
        throw new Error(`Authentication failed for ${provider}. Your API key may be invalid/rotated. Please reconnect in AXI CONNECT.`);
    }
}

function messagesToPlainText(messages) {
    // Useful for providers that don't accept OpenAI chat format directly (e.g., Gemini quick integration)
    return (messages || [])
        .map(m => `${(m.role || "user").toUpperCase()}: ${m.content || ""}`)
        .join("\n\n");
}

/**
 * Unified chat completion across providers.
 * Returns: { text } where text is the assistant response string.
 */
async function axiChatCompletion({ messages, temperature = 0.3, max_tokens = 4000, model } = {}) {
    const cfg = getAxiConfig();
    const provider = cfg.provider;
    const apiKey = cfg.apiKey;
    const useModel = model || cfg.model;

    // ---- OpenAI (direct) ----
    if (provider === "openai") {
        const res = await fetch("https://api.openai.com/v1/chat/completions", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${apiKey}`
            },
            body: JSON.stringify({
                model: useModel,
                messages,
                temperature,
                max_tokens
            })
        });

        handleAuthFailure(provider, res.status);

        const data = await res.json().catch(() => ({}));
        if (!res.ok) throw new Error(data?.error?.message || "OpenAI API error");

        return { text: data?.choices?.[0]?.message?.content || "" };
    }

    // ---- OpenRouter ----
    if (provider === "openrouter") {
        const res = await fetch("https://openrouter.ai/api/v1/chat/completions", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${apiKey}`,
                "HTTP-Referer": location.origin,
                "X-Title": "Axpert Insights"
            },
            body: JSON.stringify({
                model: useModel,
                messages,
                temperature,
                max_tokens
            })
        });

        handleAuthFailure(provider, res.status);

        const data = await res.json().catch(() => ({}));
        if (!res.ok) throw new Error(data?.error?.message || "OpenRouter API error");

        return { text: data?.choices?.[0]?.message?.content || "" };
    }

    // ---- Gemini (simple browser-safe integration) ----
    // Note: This converts your chat history into a single prompt for reliability.
    if (provider === "gemini") {
        const promptText = messagesToPlainText(messages);

        const url = `https://generativelanguage.googleapis.com/v1beta/models/${encodeURIComponent(useModel)}:generateContent?key=${encodeURIComponent(apiKey)}`;
        const res = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                contents: [{ role: "user", parts: [{ text: promptText }] }],
                generationConfig: {
                    temperature,
                    maxOutputTokens: Math.min(8192, Number(max_tokens) || 4000)
                }
            })
        });

        handleAuthFailure(provider, res.status);

        const data = await res.json().catch(() => ({}));
        if (!res.ok) throw new Error(data?.error?.message || "Gemini API error");

        const text =
            data?.candidates?.[0]?.content?.parts?.map(p => p?.text).filter(Boolean).join("") || "";
        return { text };
    }

    // ---- Anthropic (likely blocked by CORS in browser) ----
    if (provider === "anthropic") {
        throw new Error("Anthropic calls are not supported directly from the browser (CORS). Use a backend proxy.");
    }

    throw new Error(`Unsupported provider: ${provider}`);
}

const state = {
    busy: false,
    chats: [],
    activeChatId: null,
    pendingAttachments: []
};

const el = {
    // rail + popover
    historyBtn: document.getElementById("historyBtn"),
    historyPopover: document.getElementById("historyPopover"),
    closeHistory: document.getElementById("closeHistory"),
    chatList: document.getElementById("chatList"),
    newChat: document.getElementById("newChat"),
    reset: document.getElementById("reset"),

    // main
    messages: document.getElementById("messages"),
    typing: document.getElementById("typing"),

    // composer
    composer: document.getElementById("composer"),
    fileInput: document.getElementById("fileInput"),
    attachmentTray: document.getElementById("attachmentTray"),
    prompt: document.getElementById("prompt"),
    send: document.getElementById("send")

};

// Fix: Use stopPropagation to prevent UI glitches
document.getElementById("newChatFromHistory")?.addEventListener("click", (e) => {
    e.stopPropagation(); // Stop click from bubbling
    newChat();
    closeHistory(); // Close the popover gracefully
});


function renderAttachmentTray() {
    if (!state.pendingAttachments.length) {
        el.attachmentTray.classList.add("attachmentTray--hidden");
        el.attachmentTray.innerHTML = "";
        return;
    }

    el.attachmentTray.classList.remove("attachmentTray--hidden");
    el.attachmentTray.innerHTML = "";

    state.pendingAttachments.forEach((a, idx) => {
        const chip = document.createElement("div");
        chip.className = "attachmentChip";

        const thumb = document.createElement("div");
        thumb.className = "attachmentChip__thumb";
        if (a.kind === "image" && a.previewUrl) {
            thumb.innerHTML = `<img src="${a.previewUrl}" alt="">`;
        } else {
            thumb.innerHTML = `<div style="width:100%;height:100%;display:grid;place-items:center;color:#999;font-size:10px;">${a.kind}</div>`;
        }

        const name = document.createElement("div");
        name.className = "attachmentChip__name";
        name.textContent = a.name;

        const rm = document.createElement("button");
        rm.className = "attachmentChip__remove";
        rm.type = "button";
        rm.innerHTML = `
      <svg viewBox="0 0 24 24" fill="none" style="width:14px;height:14px;">
        <path d="M18 6L6 18M6 6l12 12" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
      </svg>`;
        rm.addEventListener("click", () => {
            state.pendingAttachments.splice(idx, 1);
            renderAttachmentTray();
            syncComposerButtons();
        });

        chip.appendChild(thumb);
        chip.appendChild(name);
        chip.appendChild(rm);
        el.attachmentTray.appendChild(chip);
    });
}

/* OpenAI Helper */
let OPENAI_API_KEY = localStorage.getItem("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") || "";

async function getApiKey() {
    if (!OPENAI_API_KEY) {
        const key = "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"
        if (key) {
            OPENAI_API_KEY = key;
            localStorage.setItem("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", key);
        } else {
            throw new Error("API Key required.");
        }
    }
    return OPENAI_API_KEY;
}

function enhanceCodeBlocks(rootEl) {
    const pres = rootEl.querySelectorAll("pre");

    pres.forEach((pre) => {
        // Skip if already processed
        if (pre.closest(".codeCard")) return;

        const codeEl = pre.querySelector("code");
        if (!codeEl) return;

        const rawCode = codeEl.textContent || "";
        const langMatch = (codeEl.className || "").match(/language-([\w-]+)/);
        const language = (langMatch && langMatch[1]) ? langMatch[1] : "code";

        // Build wrapper (do NOT move <pre> yet)
        const wrapper = document.createElement("div");
        wrapper.className = "codeCard";

        const header = document.createElement("div");
        header.className = "codeCard__header";

        const langLabel = document.createElement("span");
        langLabel.className = "codeCard__lang";
        langLabel.textContent = language.toUpperCase();

        const copyBtn = document.createElement("button");
        copyBtn.type = "button";
        copyBtn.className = "codeCard__copy";
        copyBtn.innerHTML = `
        <svg viewBox="0 0 24 24" class="codeCard__copyIcon" aria-hidden="true">
          <rect x="9" y="9" width="11" height="11" rx="2" ry="2"></rect>
          <path d="M5 15V5a2 2 0 0 1 2-2h10"></path>
        </svg>
        <span>Copy</span>
      `;

        async function copyToClipboard(text) {
            // Clipboard API works only in secure contexts (https/localhost)
            if (navigator.clipboard?.writeText) {
                await navigator.clipboard.writeText(text);
                return;
            }
            // Fallback
            const ta = document.createElement("textarea");
            ta.value = text;
            ta.setAttribute("readonly", "");
            ta.style.position = "fixed";
            ta.style.left = "-9999px";
            document.body.appendChild(ta);
            ta.select();
            document.execCommand("copy");
            document.body.removeChild(ta);
        }

        copyBtn.addEventListener("click", async () => {
            try {
                // Read current text (in case it changed)
                const text = codeEl.textContent || rawCode;
                await copyToClipboard(text);

                copyBtn.classList.add("codeCard__copy--done");
                copyBtn.querySelector("span").textContent = "Copied";
                setTimeout(() => {
                    copyBtn.classList.remove("codeCard__copy--done");
                    copyBtn.querySelector("span").textContent = "Copy";
                }, 1400);
            } catch (e) {
                console.error("Copy failed:", e);
            }
        });

        const body = document.createElement("div");
        body.className = "codeCard__body";

        header.appendChild(langLabel);
        header.appendChild(copyBtn);
        wrapper.appendChild(header);
        wrapper.appendChild(body);

        // Key sequence:
        // 1) replace <pre> with wrapper
        // 2) move <pre> inside wrapper body
        pre.classList.add("codeCard__pre");
        pre.replaceWith(wrapper);
        body.appendChild(pre);
    });
}

function buildDatasetPayload(fileName, rows, profile, aggregates) {
    // Keep payload small + useful
    const sampleRows = rows.slice(0, 50);

    return {
        fileName,
        schema: {
            rowCount: profile.rowCount,
            columns: profile.columns,
            missingRatio: profile.missingRatio
        },
        aggregates,      // your precomputed counts are perfect for charts
        sampleRows       // gives model a feel of the raw data
    };
}

function renderDatasetOverviewCard(contentWrap, profile, aggregates) {
    if (!profile) return;
    const card = document.createElement("article");
    card.className = "insightsCard";

    const headline = document.createElement("div");
    headline.className = "insightsCard__headline";
    headline.textContent = "Dataset overview";

    const summary = document.createElement("div");
    summary.className = "insightsCard__summary";
    summary.textContent = `This file has ${profile.rowCount.toLocaleString()} rows across ` +
        `${profile.columns.length} columns, with ` +
        `${(profile.missingRatio * 100).toFixed(1)}% missing cells.`;

    const metrics = document.createElement("div");
    metrics.className = "insightsCard__metrics";

    // Example metric 1: Row count
    const m1 = document.createElement("div");
    m1.className = "insightsCard__metric";
    m1.innerHTML = `<strong>${profile.rowCount.toLocaleString()}</strong><span>Rows</span>`;

    // Example metric 2: Column count
    const m2 = document.createElement("div");
    m2.className = "insightsCard__metric";
    m2.innerHTML = `<strong>${profile.columns.length}</strong><span>Columns</span>`;

    metrics.appendChild(m1);
    metrics.appendChild(m2);

    card.appendChild(headline);
    card.appendChild(summary);
    card.appendChild(metrics);
    contentWrap.appendChild(card);
}

// --- 2. Render Highcharts from JSON ---
// (Only ONE copy of this function)
function renderHighchartInMessage(container, chartSpec) {
    const chartDiv = document.createElement("div");
    chartDiv.style.width = "100%";
    chartDiv.style.height = "320px";
    chartDiv.style.marginTop = "20px";
    chartDiv.style.borderRadius = "12px";
    chartDiv.style.overflow = "hidden";
    chartDiv.style.boxShadow = "0 4px 12px rgba(0,0,0,0.05)";
    container.appendChild(chartDiv);

    Highcharts.chart(chartDiv, {
        chart: {
            type: chartSpec.type || 'line',
            style: { fontFamily: 'Inter, sans-serif' },
            backgroundColor: '#ffffff'
        },
        title: { text: chartSpec.title || 'Chart' },
        xAxis: chartSpec.xAxis || {},
        yAxis: { title: { text: 'Values' } },
        series: chartSpec.series || [],
        credits: { enabled: false },
        plotOptions: {
            series: { borderRadius: 4, animation: { duration: 1000 } }
        },
        colors: ['#2563EB', '#10B981', '#F59E0B', '#EF4444', '#8B5CF6']
    });
}


// --- OpenRouter key (for now you can hardcode) ---


async function callOpenRouterForTableAndInsights(
    datasetPayload,
    userGoal = "Generate a smart table view and insights"
) {
    const endpoint = "https://openrouter.ai/api/v1/chat/completions";

    const system = `
  Return ONLY valid JSON (no markdown) in this shape:
  {
    "table": { ...same as before... },
    "insights": {
      "headline": "string",
      "summary": "string",
      "keymetrics": [{ "label": "Rows", "value": "..." }, ...],
      "highlights": ["string","string","string"],
      "qualityFlags": ["string","string"],
      "columnNotes": [{ "key": "exact_column_name", "note": "string" }]
    },
    "report": {
      "title": "string",
      "sections": [
        {
          "heading": "string",
          "body": "2–4 sentence narrative tailored to a civil engineer reviewing pier & girder data"
        }
      ]
    }
  }
  Rules:
  - All text must be grounded in the provided datasetPayload (schema, aggregates, sampleRows).
  - Address the user as a project/bridge engineer: focus on site, girder status, pier status, and obvious risks or next actions.
  - 2–4 sections max in "report.sections.
  - Write in concise, formal, engineering style.
  - Avoid hype words (amazing, critical unless justified).
  - Prefer quantified statements using aggregates (e.g., Pending: 2509).
  `.trim();


    const userMsg = `
  Goal: ${userGoal}
  
  Dataset JSON:
  ${JSON.stringify(datasetPayload)}
  `.trim();


    const { text } = await axiChatCompletion({
        messages: [
            { role: "system", content: system },
            { role: "user", content: userMsg }
        ],
        temperature: 0.2,
        max_tokens: 900,
        // model: optional
    });

    const parsed = tryParseJsonStrict(text);
    return parsed ? parsed : { fallbackText: text };

}


function renderNarrativeReport(contentWrap, report) {
    if (!report || !Array.isArray(report.sections) || !report.sections.length) return;

    const card = document.createElement("article");
    card.className = "answerCard";

    const header = document.createElement("div");
    header.className = "answerCardheader";

    const label = document.createElement("span");
    label.className = "answerCardlabel";
    label.textContent = report.title || "Project report";

    header.appendChild(label);
    card.appendChild(header);

    const body = document.createElement("div");
    body.className = "answerCardbody";

    report.sections.slice(0, 4).forEach(sec => {
        const h = document.createElement("h3");
        h.textContent = sec.heading || "Overview";
        const p = document.createElement("p");
        p.textContent = sec.body || "";
        body.appendChild(h);
        body.appendChild(p);
    });

    card.appendChild(body);
    contentWrap.appendChild(card);
}



function renderTableInMessage(contentWrap, rows, tableSpec, profile) {
    const spec = tableSpec || {};
    const rowCount = Math.max(5, Math.min(25, Number(spec.rowCount || 20) || 20));

    // Columns from AI or fallback
    const fallbackCols = (profile?.columns || [])
        .slice(0, 6)
        .map(k => ({ key: k, label: k, type: "text" }));

    const cols = Array.isArray(spec.columns) && spec.columns.length
        ? spec.columns.slice(0, 8)
        : fallbackCols;

    // ✅ NORMALISE COLUMNS ONCE (OUTSIDE ROW LOOP)
    const actualCols = new Set((profile?.columns || []).map(String));

    let safeCols = cols.filter(c => actualCols.has(String(c.key)));

    if (!safeCols.length) {
        safeCols = (profile?.columns || [])
            .slice(0, 6)
            .map(k => ({ key: k, label: k, type: "text" }));
    }

    const finalCols = safeCols;

    // Debug once
    console.log("Table columns from AI/final:", finalCols.map(c => c.key));
    console.log("Sample row keys:", rows[0] && Object.keys(rows[0]));

    const card = document.createElement("article");
    card.className = "tableCard";

    const header = document.createElement("div");
    header.className = "tableCardheader";
    header.textContent = spec.title || "Data preview";

    const tableWrap = document.createElement("div");
    tableWrap.className = "tableCardwrap";

    const table = document.createElement("table");
    table.className = "tableCardtable";

    const thead = document.createElement("thead");
    const headRow = document.createElement("tr");
    finalCols.forEach(c => {
        const th = document.createElement("th");
        th.textContent = c.label || c.key;
        headRow.appendChild(th);
    });
    thead.appendChild(headRow);

    // Optional sort
    let viewRows = rows.slice(0);
    if (spec.sort?.key && finalCols.some(c => c.key === spec.sort.key)) {
        const dir = (spec.sort.dir || "asc").toLowerCase() === "desc" ? -1 : 1;
        const key = spec.sort.key;
        viewRows.sort((a, b) => {
            const av = a?.[key];
            const bv = b?.[key];
            const an = Number(av);
            const bn = Number(bv);
            const bothNum = Number.isFinite(an) && Number.isFinite(bn);
            if (bothNum) return (an - bn) * dir;
            return String(av ?? "").localeCompare(String(bv ?? "")) * dir;
        });
    }

    const tbody = document.createElement("tbody");
    viewRows.slice(0, rowCount).forEach(r => {
        const tr = document.createElement("tr");
        finalCols.forEach(c => {
            const td = document.createElement("td");
            const v = r && typeof c.key === "string" ? r[c.key] : undefined;
            const text =
                v === null || v === undefined || String(v).trim() === ""
                    ? "—"
                    : String(v);
            td.textContent = text;
            td.title = text;
            tr.appendChild(td);
        });
        tbody.appendChild(tr);
    });

    table.appendChild(thead);
    table.appendChild(tbody);
    tableWrap.appendChild(table);
    card.appendChild(header);
    card.appendChild(tableWrap);
    contentWrap.appendChild(card);
}


function renderTableNotes(contentWrap, payload) {
    const flags = payload?.insights?.qualityFlags || [];
    const notes = payload?.insights?.columnNotes || [];
    if (!flags.length && !notes.length) return;

    const card = document.createElement("article");
    card.className = "tableNotesCard";

    if (flags.length) {
        const h = document.createElement("div");
        h.className = "tableNotesCardtitle";
        h.textContent = "Data quality flags";
        card.appendChild(h);

        const ul = document.createElement("ul");
        ul.className = "tableNotesCardlist";
        flags.slice(0, 6).forEach(t => {
            const li = document.createElement("li");
            li.textContent = String(t);
            ul.appendChild(li);
        });
        card.appendChild(ul);
    }

    if (notes.length) {
        const h2 = document.createElement("div");
        h2.className = "tableNotesCardtitle";
        h2.textContent = "Column notes";
        card.appendChild(h2);

        const ul2 = document.createElement("ul");
        ul2.className = "tableNotesCardlist";
        notes.slice(0, 8).forEach(n => {
            const li = document.createElement("li");
            li.textContent = `${n.key}: ${n.note}`;
            ul2.appendChild(li);
        });
        card.appendChild(ul2);
    }

    contentWrap.appendChild(card);
}


function tryParseJsonStrict(text) {
    // Handle raw JSON or JSON inside ```json ... ```
    const trimmed = (text || "").trim();
    const fenced = trimmed.match(/```json([\s\S]*?)```/i);
    const candidate = fenced ? fenced[1].trim() : trimmed;

    try { return JSON.parse(candidate); }
    catch { return null; }
}

// Helper: Read file as text
function readFileAsText(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = (e) => resolve(e.target.result);
        reader.onerror = (e) => reject(e);
        reader.readAsText(file);
    });
}

function buildLLMPayload({ fileName, rows, profile, aggregates }) {
    const sample = rows.slice(0, 50); // keep small
    return {
        fileName,
        schema: {
            rowCount: profile.rowCount,
            columns: profile.columns,
            missingRatio: profile.missingRatio,
        },
        aggregates: {
            girderStatus: aggregates.girderStatus,
            pierStatus: aggregates.pierStatus,
            context: aggregates.context,
            project: aggregates.project,
        },
        sampleRows: sample
    };
}

// Function to Polish Code Blocks
function enhanceCodeBlocks(rootEl) {
    // 1. Find all raw pre tags
    const pres = rootEl.querySelectorAll("pre");

    pres.forEach((pre) => {
        // Skip if already processed
        if (pre.closest(".codeCard")) return;

        const codeEl = pre.querySelector("code");
        if (!codeEl) return;

        // 2. Identify Language (e.g., class="language-js")
        const langMatch = (codeEl.className || "").match(/language-([\w-]+)/);
        const language = (langMatch && langMatch[1]) ? langMatch[1] : "Code";

        // 3. Create the Wrapper Card
        const wrapper = document.createElement("div");
        wrapper.className = "codeCard";

        // Header (Language + Copy Button)
        const header = document.createElement("div");
        header.className = "codeCard__header";

        header.innerHTML = `
            <span class="codeCard__lang">${language}</span>
            <button class="codeCard__copy" onclick="copyCode(this)">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path></svg>
                Copy
            </button>
        `;

        // 4. Wrap the <pre>
        wrapper.appendChild(header);

        // Insert wrapper before <pre>, then move <pre> inside
        pre.parentNode.insertBefore(wrapper, pre);
        wrapper.appendChild(pre);

        // 5. Apply Syntax Highlighting
        if (window.hljs) {
            hljs.highlightElement(codeEl);
        }
    });
}

// Global Copy Helper
window.copyCode = function (btn) {
    const code = btn.closest('.codeCard').querySelector('code').innerText;
    navigator.clipboard.writeText(code);
    const original = btn.innerHTML;
    btn.innerHTML = `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="#98c379" stroke-width="2"><polyline points="20 6 9 17 4 12"></polyline></svg> Copied!`;
    setTimeout(() => btn.innerHTML = original, 2000);
};

const SYSTEM_PROMPT_CHARTS = `
You are a helpful AI assistant.
WHEN ASKED TO GENERATE A CHART:
1. You must return the data in a strict JSON format.
2. Do NOT include any conversational text before or after the JSON.
3. The JSON must follow this schema:
{
  "chart": {
    "type": "column", // or bar, line, pie, donut, area
    "title": "Chart Title",
    "categories": ["Jan", "Feb", "Mar"],
    "series": [
      { "name": "Series 1", "data": [10, 20, 30] }
    ]
  }
}
If multiple charts are needed, use { "charts": [ { "chart": ... }, ... ] }.
`;


async function callOpenAI(messages, datasetContext) {
    // Check for pending database data
    let enhancedDatasetContext = datasetContext;
    let actualDataRows = null;

    if (window.pendingDatabaseData) {
        const dbInfo = window.pendingDatabaseData;
        enhancedDatasetContext = {
            source: 'database',
            name: dbInfo.name,
            recordCount: dbInfo.data.length,
            data: dbInfo.data
            // Don't put full data here - it will be in the user message
        };
        actualDataRows = dbInfo.data; // Store the actual data rows
        window.pendingDatabaseData = null;
    }

    // Get system prompt
    let basePrompt;
    if (typeof window.getActiveSystemPrompt === 'function') {
        basePrompt = window.getActiveSystemPrompt();
    } else {
        basePrompt = `You are AXI, an expert Data Analyst and Report Generator...`; // Your default prompt
    }

    const systemPrompt = basePrompt + `

CONTEXT:
${enhancedDatasetContext ? `[DATASET]: ${JSON.stringify(enhancedDatasetContext)}` : "[DATASET IN MESSAGE HISTORY]"}
`.trim();

    // Prepare messages array
    let finalMessages = [{ role: "system", content: systemPrompt }];

    // --- CRITICAL FIX: Add the actual data as a user message ---
    if (actualDataRows && actualDataRows.length > 0) {
        // Get the last user message (the question)
        const lastUserMessage = messages && messages.length > 0
            ? messages[messages.length - 1]
            : { role: "user", content: "Analyze this data" };

        // Create a new user message with the data + question
        const dataMessage = {
            role: "user",
            content: `I have a dataset with ${actualDataRows.length} records.

Here are ALL the records:
${JSON.stringify(actualDataRows, null, 2)}

My question is: ${lastUserMessage.content || "Please analyze this data"}`
        };

        finalMessages.push(dataMessage);
    } else {
        // No data rows, just use the original messages
        finalMessages = [...finalMessages, ...(messages || [])];
    }

    const { text } = await axiChatCompletion({
        messages: finalMessages,
        temperature: 0.3,
        max_tokens: 4000,
    });

    return text;
}



function uid() { return Math.random().toString(16).slice(2) + Date.now().toString(16); }
function fmtTime(ts) {
    const d = new Date(ts);
    return d.toLocaleTimeString(undefined, { hour: "2-digit", minute: "2-digit" });
}
function fmtDate(ts) {
    const d = new Date(ts);
    return d.toLocaleDateString(undefined, { month: "short", day: "numeric" });
}

function setBusy(v) {
    state.busy = v;

    if (v) {
        // Show new animation
        el.typing.classList.remove('typing--hidden');

        // OPTION 1: Gradient Pulse Wave
        el.typing.className = 'typing typing--pulse';
        el.typing.innerHTML = `
            <div class="pulse-bar"></div>
            <div class="pulse-bar"></div>
            <div class="pulse-bar"></div>
            <div class="pulse-bar"></div>
        `;

        /* OPTION 2: Rotating Sparkle Icon (Comment out Option 1 and use this instead)
        el.typing.className = 'typing typing--icon';
        el.typing.innerHTML = `
            <svg class="sparkle-icon" viewBox="0 0 24 24" fill="none">
                <path d="M12 2L14.09 8.26L20 10L14.09 11.74L12 18L9.91 11.74L4 10L9.91 8.26L12 2Z" 
                      fill="url(#sparkle-gradient)" stroke="none"/>
                <defs>
                    <linearGradient id="sparkle-gradient" x1="0%" y1="0%" x2="100%" y2="100%">
                        <stop offset="0%" style="stop-color:#3B82F6;stop-opacity:1" />
                        <stop offset="50%" style="stop-color:#8B5CF6;stop-opacity:1" />
                        <stop offset="100%" style="stop-color:#EC4899;stop-opacity:1" />
                    </linearGradient>
                </defs>
            </svg>
        `;
        */

    } else {
        // Hide animation
        el.typing.classList.add('typing--hidden');
        el.typing.innerHTML = '';
    }

    syncComposerButtons();
}

function syncComposerButtons() {
    const hasText = !!el.prompt.value.trim();
    const hasAny = hasText || state.pendingAttachments.length > 0;
    el.send.disabled = state.busy || !hasAny;
}
function scrollToBottom() { el.messages.scrollTop = el.messages.scrollHeight; }
function renderMarkdown(md) { return DOMPurify.sanitize(marked.parse(md || "")); }

function loadChats() {
    try { return JSON.parse(localStorage.getItem(LS_KEY) || "[]"); }
    catch { return []; }
}
function saveChats() { localStorage.setItem(LS_KEY, JSON.stringify(state.chats)); }
function getActiveChat() { return state.chats.find(c => c.id === state.activeChatId) || null; }

function ensureAtLeastOneChat() {
    if (state.chats.length) return;
    const c = { id: uid(), title: "New chat", createdAt: Date.now(), updatedAt: Date.now(), messages: [], dataset: null };
    state.chats.unshift(c);
    state.activeChatId = c.id;
    saveChats();
}


const HAS_HISTORY_UI = !!(el.historyBtn && el.historyPopover && el.closeHistory && el.chatList);
/* Popover open/close */
function openHistory() {
    if (!HAS_HISTORY_UI) return;
    el.historyPopover.classList.add("historyPopover--open");
    el.historyPopover.setAttribute("aria-hidden", "false");
    renderChatList();
}
function closeHistory() {
    if (!HAS_HISTORY_UI) return;
    el.historyPopover.classList.remove("historyPopover--open");
    el.historyPopover.setAttribute("aria-hidden", "true");
}

/* Hover behavior (desktop) + click toggle */
let hoverTimer = null;
el.historyBtn?.addEventListener("mouseenter", () => {

    clearTimeout(hoverTimer);
    openHistory();
});
el.historyPopover?.addEventListener("mouseenter", () => {
    clearTimeout(hoverTimer);
});
el.historyBtn?.addEventListener("mouseleave", () => {
    hoverTimer = setTimeout(closeHistory, 220);
});
el.historyPopover?.addEventListener("mouseleave", () => {
    hoverTimer = setTimeout(closeHistory, 220);
});
el.historyBtn?.addEventListener("click", () => {
    const open = el.historyPopover.classList.contains("historyPopover--open");
    open ? closeHistory() : openHistory();
});
el.closeHistory?.addEventListener("click", closeHistory);

/* Sidebar list */
function renderChatList() {
    el.chatList.innerHTML = "";

    state.chats
        .slice()
        .sort((a, b) => b.updatedAt - a.updatedAt)
        .forEach(chat => {
            const item = document.createElement("div");
            item.className = `chatItem ${chat.id === state.activeChatId ? "chatItem--active" : ""}`;

            const main = document.createElement("div");
            main.className = "chatItem__main";

            const title = document.createElement("div");
            title.className = "chatItem__title";
            title.textContent = chat.title || "New chat";

            const meta = document.createElement("div");
            meta.className = "chatItem__meta";
            meta.textContent = `${fmtDate(chat.updatedAt)} • ${chat.messages.length} msg`;

            main.appendChild(title);
            main.appendChild(meta);

            const del = document.createElement("button");
            del.className = "chatItem__btn";
            del.title = "Delete";
            del.innerHTML = `
            <svg viewBox="0 0 24 24" fill="none" class="i">
                <path d="M3 6h18" stroke="currentColor" stroke-width="2" stroke-linecap="round" />
                <path d="M8 6V4h8v2" stroke="currentColor" stroke-width="2" stroke-linecap="round" />
                <path d="M6 6l1 16h10l1-16" stroke="currentColor" stroke-width="2" stroke-linejoin="round" />
            </svg>
            `;

            item.addEventListener("click", () => {
                setActiveChat(chat.id);
                closeHistory();
            });
            del.addEventListener("click", (e) => {
                e.stopPropagation();
                deleteChat(chat.id);
            });

            item.appendChild(main);
            item.appendChild(del);
            el.chatList.appendChild(item);
        });
}

function setActiveChat(chatId) {
    state.activeChatId = chatId;
    saveChats();
    renderChatList();
    renderThread();
}

function newChat() {
    const c = { id: uid(), title: "New chat", createdAt: Date.now(), updatedAt: Date.now(), messages: [], dataset: null };
    state.chats.unshift(c);
    saveChats();
    setActiveChat(c.id);
}

function deleteChat(chatId) {
    const idx = state.chats.findIndex(c => c.id === chatId);
    if (idx === -1) return;
    const wasActive = chatId === state.activeChatId;
    state.chats.splice(idx, 1);

    if (!state.chats.length) {
        state.activeChatId = null;
        ensureAtLeastOneChat();
    } else if (wasActive) {
        state.activeChatId = state.chats[0].id;
    }
    saveChats();
    setActiveChat(state.activeChatId);
}

/* Thread rendering */
function ensureThread() {
    el.messages.innerHTML = "";
    const t = document.createElement("div");
    t.className = "thread";
    el.messages.appendChild(t);
    return t;
}

function createMessageNode(role, content, isMarkdown, ts) {
    // 1. Determine the modifier class
    var roleClass = (role === 'user') ? 'message--user' : 'message--assistant';

    // 2. Format Timestamp
    var date = new Date(ts || Date.now());
    var timeStr = date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

    // 3. Avatar Logic
    var avatarHTML = '';
    if (role === 'user') {
        // User Avatar
        avatarHTML = '<div class="message__avatar">' +
            '<div style="font-size: 10px; font-weight: bold;">YOU</div>' +
            '</div>';
    } else {
        // Assistant Avatar
        avatarHTML = '<div class="message__avatar">' +
            '<img src="../../../plugins/Axi/HTMLPages/images/axibot.png">' +
            '</div>';
    }

    // 4. Content Processing
    var bodyHTML = content;
    if (isMarkdown && typeof marked !== 'undefined') {
        bodyHTML = marked.parse(content);
        if (typeof DOMPurify !== 'undefined') {
            bodyHTML = DOMPurify.sanitize(bodyHTML);
        }
    } else {
        // Plain text fallback (escape HTML)
        bodyHTML = String(content)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
    }


    // 5. Construct the HTML String (Using concatenation, NO backticks)
    var html = '<div class="message ' + roleClass + '">' +
        avatarHTML +
        '<div class="message__content">' +
        '<div class="message__bubble">' +
        bodyHTML +
        '</div>' +
        '<div class="message__meta">' + timeStr + '</div>' +
        '</div>' +
        '</div>';

    return html;
}

/* ==========================================================================
   UPDATED renderThread Function
   (Replace your existing renderThread with this complete block)
   ========================================================================== */

/* --- UPDATED RENDER THREAD --- */
function renderThread(animateLast = false) {
    const chat = getActiveChat();
    const thread = document.getElementById("messages");
    if (!thread) return;

    thread.innerHTML = "";

    if (!chat || !chat.messages.length) {
        thread.innerHTML = `
        <div style="text-align:center; margin-top:20vh; color:#6B7280;">
             <h1 style="font-size:24px; font-weight:600;">Smart Software. Intelligent Data.</h1>
             <p style="margin-top:8px;">Ask anything about your data</p>
        </div>`;
        return;
    }

    chat.messages.forEach((m, idx) => {
        // Determine if we should animate this specific message
        const isLast = idx === chat.messages.length - 1;
        const shouldAnimate = animateLast && isLast && m.role === 'assistant';

        // 1. Create Node (pass empty content if animating, so we can fill it ourselves)
        const initialContent = shouldAnimate ? "" : m.content;
        const result = createMessageNode(m.role, initialContent, !!m.markdown, m.ts);

        // 2. Safe Node Extraction
        let node;
        if (result instanceof Node) {
            node = result;
        } else if (typeof result === 'string') {
            const wrapper = document.createElement('div');
            wrapper.innerHTML = result.trim(); // Trim to avoid text node issues
            node = wrapper.firstChild;
        } else if (typeof result === 'object' && result !== null) {
            node = result.contentWrap || result.domNode || result.node || result.el;
            if (!node) return;
        } else {
            return;
        }

        // 3. Find target for content (The Bubble)
        // Adjust selectors to match your createMessageNode classnames
        const target = node.querySelector('.message__bubble') || node.querySelector('.bubble') || node.querySelector('.msg-content') || node;

        // Make sure target is relative so absolute buttons work
        target.style.position = 'relative';

        // 4. Feature: Edit User Messages
        if (m.role === 'user') {
            makeMessageEditable(target, m.content);
        }

        thread.appendChild(node);

        // --- ANIMATION VS INSTANT RENDER ---
        if (shouldAnimate) {
            // Run animation async
            streamTextToElement(target, m.content, 3).then(() => {
                // Post-Animation: Polish
                enhanceCodeBlocks(node);
                // Render Charts
                if (m.chartSpecs && Array.isArray(m.chartSpecs)) {
                    m.chartSpecs.forEach(spec => renderHighchartInMessage(target, spec));
                } else if (m.chartSpec) {
                    renderHighchartInMessage(target, m.chartSpec);
                }
                // Render Actions
                if (m.content.length > 100 && typeof renderReportActions === 'function') {
                    renderReportActions(target, m.content);
                }
            });
        } else {
            // Standard Render (History or Non-Animated)
            enhanceCodeBlocks(node);

            // Render Charts
            if (m.chartSpecs && Array.isArray(m.chartSpecs)) {
                m.chartSpecs.forEach(spec => renderHighchartInMessage(target, spec));
            } else if (m.chartSpec) {
                renderHighchartInMessage(target, m.chartSpec);
            }

            // Render Report Actions
            if (m.role === 'assistant' && m.content && m.content.length > 100 && typeof renderReportActions === 'function') {
                renderReportActions(target, m.content);
            }
        }
    });

    scrollToBottom();
}
// --- FAST TYPEWRITER EFFECT ---
async function streamTextToElement(element, fullText, speed = 8) {
    // If it's markdown, we need to render markdown first, 
    // BUT typing HTML tags looks weird. 
    // Best approach for "AI feel": Type the RAW text, then swap to Markdown at the end? 
    // OR: Type plain text fast, then render MD.

    // STRATEGY: We type the rendered HTML content but handle tags instantly
    // so user doesn't see "<div>" being typed.

    // 1. Render final HTML first
    let finalHTML = fullText;
    if (typeof marked !== 'undefined') {
        finalHTML = DOMPurify.sanitize(marked.parse(fullText));
    }

    // If text is huge, skip animation to avoid frustration
    if (fullText.length > 2000) {
        element.innerHTML = finalHTML;
        enhanceCodeBlocks(element); // Colorize code immediately
        return;
    }

    element.innerHTML = ''; // Clear for typing

    // Create a temporary cursor
    const cursor = document.createElement('span');
    cursor.className = 'typing-cursor';
    cursor.textContent = '▋';
    cursor.style.color = '#3B82F6';
    cursor.style.animation = 'blink 1s infinite';
    element.appendChild(cursor);

    // We will parse the HTML into "chunks" (text nodes vs tags)
    // Simple regex to split by HTML tags
    const chunks = finalHTML.split(/(<[^>]+>)/g);

    for (const chunk of chunks) {
        if (chunk.startsWith('<')) {
            // It's a tag (e.g. <b> or </div>). Insert immediately (invisible)
            cursor.insertAdjacentHTML('beforebegin', chunk);
        } else {
            // It's text. Type it character by character (or word by word for speed)
            const words = chunk.split(''); // Split by char for smoothness

            for (const char of words) {
                cursor.insertAdjacentText('beforebegin', char);
                // Scroll to bottom as we type
                const chatContainer = document.getElementById('messages');
                if (chatContainer) chatContainer.scrollTop = chatContainer.scrollHeight;

                // Delay (Fast!)
                await new Promise(r => setTimeout(r, speed));
            }
        }
    }

    // Cleanup
    cursor.remove();

    // Final Polish (Highlight code, render charts if embedded)
    enhanceCodeBlocks(element);
}


function renderReportActions(container, markdownText) {
    // 1. Remove any old actions to prevent duplicates/ghosts
    const old = container.querySelector('.reportActions');
    if (old) old.remove();

    // 2. Make sure container is relative so we can position absolutely inside it
    if (getComputedStyle(container).position === 'static') {
        container.style.position = 'relative';
    }

    // Add padding to container so text doesn't overlap buttons
    if (!container.style.paddingRight || parseInt(container.style.paddingRight) < 40) {
        container.style.paddingRight = '48px';
    }

    const actions = document.createElement("div");
    actions.className = "reportActions";

    // 3. Position: Absolute Top-Right corner of the bubble
    // We use a small background blur so buttons are readable over text/charts
    actions.style.cssText = `
        position: absolute !important;
        top: 8px !important;
        right: 8px !important;
        display: flex !important;
        gap: 6px !important;
        opacity: 0; 
        transition: opacity 0.2s ease;
        z-index: 50;
        background: rgba(255, 255, 255, 0.85) !important; 
        backdrop-filter: blur(4px);
        border-radius: 8px;
        padding: 4px;
        margin: 0 !important;
    `;

    // Show on hover
    container.onmouseenter = () => actions.style.opacity = "1";
    container.onmouseleave = () => actions.style.opacity = "0";

    // Button Styling (Clean, square-ish with rounded corners)
    const btnStyle = `
        padding: 0;
        border-radius: 6px; 
        border: 1px solid #E5E7EB; 
        background: #FFFFFF; 
        cursor: pointer; 
        color: #6B7280;
        display: flex; 
        align-items: center; 
        justify-content: center;
        width: 28px; 
        height: 28px;
        transition: all 0.2s;
        box-shadow: 0 1px 2px rgba(0,0,0,0.05);
    `;

    // --- PDF Button ---
    const pdfBtn = document.createElement("button");
    pdfBtn.style.cssText = btnStyle;
    pdfBtn.title = "Export PDF";
    pdfBtn.innerHTML = `<svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path><polyline points="14 2 14 8 20 8"></polyline><line x1="12" y1="18" x2="12" y2="12"></line><line x1="9" y1="15" x2="15" y2="15"></line></svg>`;

    pdfBtn.onmouseover = () => { pdfBtn.style.color = "#2563EB"; pdfBtn.style.borderColor = "#BFDBFE"; };
    pdfBtn.onmouseout = () => { pdfBtn.style.color = "#6B7280"; pdfBtn.style.borderColor = "#E5E7EB"; };

    pdfBtn.onclick = async (e) => {
        e.stopPropagation();
        const originalIcon = pdfBtn.innerHTML;
        // Simple spinner
        pdfBtn.innerHTML = `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" style="animation: spin 1s linear infinite;"><path d="M12 2v4m0 12v4M4.93 4.93l2.83 2.83m8.48 8.48l2.83 2.83M2 12h4m12 0h4M4.93 19.07l2.83-2.83m8.48-8.48l2.83-2.83"/></svg>`;

        try {
            await generatePDFReport(container, markdownText);
            pdfBtn.innerHTML = `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#16A34A" stroke-width="2.5"><polyline points="20 6 9 17 4 12"></polyline></svg>`;
            setTimeout(() => pdfBtn.innerHTML = originalIcon, 2500);
        } catch (err) {
            console.error(err);
            alert("Failed to generate PDF: " + err.message);
            pdfBtn.innerHTML = originalIcon;
        }
    };

    // --- Copy Button ---
    const copyBtn = document.createElement("button");
    copyBtn.style.cssText = btnStyle;
    copyBtn.title = "Copy Text";
    copyBtn.innerHTML = `<svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path></svg>`;

    copyBtn.onmouseover = () => { copyBtn.style.color = "#111827"; copyBtn.style.borderColor = "#D1D5DB"; };
    copyBtn.onmouseout = () => { copyBtn.style.color = "#6B7280"; copyBtn.style.borderColor = "#E5E7EB"; };

    copyBtn.onclick = (e) => {
        e.stopPropagation();
        navigator.clipboard.writeText(markdownText);
        const original = copyBtn.innerHTML;
        copyBtn.innerHTML = `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#16A34A" stroke-width="2.5"><polyline points="20 6 9 17 4 12"></polyline></svg>`;
        setTimeout(() => copyBtn.innerHTML = original, 2000);
    };

    actions.appendChild(pdfBtn);
    actions.appendChild(copyBtn);

    // Add CSS Animation for spinner if not exists
    if (!document.getElementById('spin-anim')) {
        const style = document.createElement('style');
        style.id = 'spin-anim';
        style.innerHTML = `@keyframes spin { 100% { transform: rotate(360deg); } }`;
        document.head.appendChild(style);
    }

    container.appendChild(actions);
}



// Add CSS animation for spinner
const style = document.createElement('style');
style.textContent = `
@keyframes spin { 
    from { transform: rotate(0deg); } 
    to { transform: rotate(360deg); } 
}
`;
document.head.appendChild(style);

async function generatePDFReport(container, markdownText) {
    const { jsPDF } = window.jspdf;
    const pdf = new jsPDF('p', 'pt', 'a4');

    const pageWidth = pdf.internal.pageSize.getWidth();
    const pageHeight = pdf.internal.pageSize.getHeight();
    const margin = 40;
    const contentWidth = pageWidth - (margin * 2);

    // Add Header
    pdf.setFillColor(102, 126, 234);
    pdf.rect(0, 0, pageWidth, 80, 'F');
    pdf.setTextColor(255, 255, 255);
    pdf.setFontSize(24);
    pdf.setFont(undefined, 'bold');
    pdf.text('AXI Analysis Report', margin, 50);

    // Add Date
    pdf.setFontSize(10);
    pdf.setFont(undefined, 'normal');
    const date = new Date().toLocaleDateString('en-IN', {
        year: 'numeric', month: 'long', day: 'numeric'
    });
    pdf.text(`Generated: ${date}`, margin, 68);

    // Process Markdown Content
    pdf.setTextColor(0, 0, 0);
    let yPos = 110;

    const lines = markdownText.split('\n');

    for (let line of lines) {
        // Check for page break
        if (yPos > pageHeight - 60) {
            pdf.addPage();
            yPos = margin;
        }

        // Headers
        if (line.startsWith('## ')) {
            yPos += 15;
            pdf.setFontSize(16);
            pdf.setFont(undefined, 'bold');
            pdf.setTextColor(102, 126, 234);
            pdf.text(line.replace('## ', ''), margin, yPos);
            yPos += 25;
        }
        else if (line.startsWith('### ')) {
            yPos += 10;
            pdf.setFontSize(14);
            pdf.setFont(undefined, 'bold');
            pdf.setTextColor(60, 60, 60);
            pdf.text(line.replace('### ', ''), margin, yPos);
            yPos += 20;
        }
        // Bold text
        else if (line.includes('**')) {
            pdf.setFontSize(11);
            pdf.setFont(undefined, 'bold');
            pdf.setTextColor(40, 40, 40);
            const cleaned = line.replace(/\*\*/g, '');
            const wrapped = pdf.splitTextToSize(cleaned, contentWidth);
            pdf.text(wrapped, margin, yPos);
            yPos += wrapped.length * 15;
        }
        // Bullet points
        else if (line.trim().startsWith('- ')) {
            pdf.setFontSize(10);
            pdf.setFont(undefined, 'normal');
            pdf.setTextColor(60, 60, 60);
            pdf.text('•', margin, yPos);
            const cleaned = line.replace('- ', '');
            const wrapped = pdf.splitTextToSize(cleaned, contentWidth - 20);
            pdf.text(wrapped, margin + 15, yPos);
            yPos += wrapped.length * 14;
        }
        // Normal text
        else if (line.trim()) {
            pdf.setFontSize(10);
            pdf.setFont(undefined, 'normal');
            pdf.setTextColor(60, 60, 60);
            const wrapped = pdf.splitTextToSize(line, contentWidth);
            pdf.text(wrapped, margin, yPos);
            yPos += wrapped.length * 14 + 5;
        } else {
            yPos += 10; // Empty line spacing
        }
    }

    // Capture Charts
    const charts = container.querySelectorAll('.highcharts-container');
    for (let i = 0; i < charts.length; i++) {
        const chart = charts[i];

        // Add new page for chart
        pdf.addPage();
        pdf.setFontSize(14);
        pdf.setFont(undefined, 'bold');
        pdf.text(`Chart ${i + 1}`, margin, 50);

        // Capture chart as image
        const canvas = await html2canvas(chart, {
            backgroundColor: '#ffffff',
            scale: 2
        });
        const imgData = canvas.toDataURL('image/png');

        // Add to PDF
        const imgWidth = contentWidth;
        const imgHeight = (canvas.height * imgWidth) / canvas.width;
        pdf.addImage(imgData, 'PNG', margin, 80, imgWidth, imgHeight);
    }

    // Add Footer to all pages
    const totalPages = pdf.internal.getNumberOfPages();
    for (let i = 1; i <= totalPages; i++) {
        pdf.setPage(i);
        pdf.setFontSize(8);
        pdf.setTextColor(150, 150, 150);
        pdf.text(`Page ${i} of ${totalPages} | Generated by AXI`, margin, pageHeight - 20);
    }

    // Save
    pdf.save(`AXI_Report_${Date.now()}.pdf`);
}

// --- Feature: Edit Previous Prompts ---
function makeMessageEditable(node, text) {
    node.title = "Double-click to edit";
    node.style.cursor = "pointer";

    node.ondblclick = () => {
        const input = document.getElementById("prompt");
        if (input) {
            input.value = text;
            input.focus();
            // Optional: Scroll to input
            input.scrollIntoView({ behavior: "smooth" });
        }
    };
}


function renderInsightsInMessage(contentWrap, insights) {
    const card = document.createElement("article");
    card.className = "insightsCard";

    const headline = document.createElement("div");
    headline.className = "insightsCard__headline";
    headline.textContent = insights?.headline || "Dataset insights";

    const summary = document.createElement("div");
    summary.className = "insightsCard__summary";
    summary.textContent = insights?.summary || "";

    const metrics = document.createElement("div");
    metrics.className = "insightsCard__metrics";

    (insights?.key_metrics || []).forEach((m) => {
        const item = document.createElement("div");
        item.className = "insightsCard__metric";

        const label = document.createElement("div");
        label.className = "insightsCard__metricLabel";
        label.textContent = m?.label || "";

        const value = document.createElement("div");
        value.className = "insightsCard__metricValue";
        value.textContent = m?.value || "";

        item.appendChild(label);
        item.appendChild(value);
        metrics.appendChild(item);
    });

    const highlights = document.createElement("ul");
    highlights.className = "insightsCard__list";
    (insights?.highlights || []).forEach((t) => {
        const li = document.createElement("li");
        li.textContent = t;
        highlights.appendChild(li);
    });

    card.appendChild(headline);
    if (insights?.summary) card.appendChild(summary);
    if ((insights?.key_metrics || []).length) card.appendChild(metrics);
    if ((insights?.highlights || []).length) card.appendChild(highlights);

    contentWrap.appendChild(card);
}

function pushMessage(role, content, { markdown = false, charts = [] } = {}) {
    const chat = getActiveChat();
    if (!chat) return;

    const m = { id: uid(), role, content, markdown, ts: Date.now(), charts };
    chat.messages.push(m);

    if (chat.title === "New chat" && role === "user") {
        chat.title = (content || "New chat").slice(0, 40) + ((content || "").length > 40 ? "…" : "");
    }

    chat.updatedAt = Date.now();
    saveChats();
    renderChatList();

    const node = createMessageNode(role, content, markdown, m.ts);
    if (charts?.length) charts.forEach(spec => renderChartInMessage(node.contentWrap, spec));
    scrollToBottom();
}

/* Premium chart with distinct colors per category */
function renderChartInMessage(contentWrap, chartSpec) {
    const card = document.createElement("div");
    card.className = "messageChart";

    const title = document.createElement("div");
    title.className = "messageChart__title";
    title.textContent = chartSpec.title || "Chart";

    const canvas = document.createElement("canvas");
    card.appendChild(title);
    card.appendChild(canvas);
    contentWrap.appendChild(card);

    // Premium matte color palette - distinct per category
    const palette = [
        "#3b82f6", // blue
        "#10b981", // green
        "#f59e0b", // amber
        "#8b5cf6", // purple
        "#ef4444", // red
        "#06b6d4", // cyan
        "#ec4899", // pink
        "#64748b", // slate
        "#14b8a6", // teal
        "#f97316", // orange
        "#a855f7", // violet
        "#84cc16"  // lime
    ];

    const type = (chartSpec.type || "bar").toLowerCase();

    // For doughnut/pie, use different color per segment
    const isDoughnutPie = type === "doughnut" || type === "pie";

    new Chart(canvas, {
        type,
        data: {
            labels: chartSpec.labels || [],
            datasets: (chartSpec.datasets || []).map((dataset, dsIdx) => {
                const baseColor = palette[dsIdx % palette.length];
                const perCategoryColors = (dataset.data || []).map((_, i) => palette[i % palette.length]);
                return {
                    label: dataset.label || `Series ${dsIdx + 1}`,
                    data: dataset.data || [],
                    backgroundColor: perCategoryColors,
                    borderColor: isDoughnutPie ? "#ffffff" : baseColor,
                    borderWidth: isDoughnutPie ? 3 : 0,
                    borderRadius: type === "bar" ? 10 : 0,
                    hoverOffset: isDoughnutPie ? 8 : 0
                };
            })
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            layout: { padding: { left: 6, right: 6, top: 6, bottom: 6 } },
            plugins: {
                legend: {
                    position: "bottom",
                    labels: {
                        color: "#666666",
                        font: { size: 12, weight: "500" },
                        padding: 12,
                        boxWidth: 12,
                        boxHeight: 12,
                        usePointStyle: true,
                        pointStyle: "circle"
                    }
                },
                tooltip: {
                    backgroundColor: "rgba(0,0,0,0.85)",
                    titleColor: "#ffffff",
                    bodyColor: "#ffffff",
                    borderColor: "rgba(255,255,255,0.1)",
                    borderWidth: 1,
                    padding: 12,
                    cornerRadius: 10,
                    displayColors: true,
                    intersect: false,
                    mode: "index"
                }
            },
            scales: (isDoughnutPie) ? {} : {
                x: {
                    grid: { display: false },
                    ticks: {
                        color: "#8f8f8f",
                        font: { size: 11 }
                    }
                },
                y: {
                    grid: { color: "#ecece6", drawBorder: false },
                    ticks: {
                        color: "#8f8f8f",
                        font: { size: 11 }
                    }
                }
            }
        }
    });
}


/* Dataset parsing + insights in chat */
function normalizeRow(r) {
    const clean = {};
    for (const [k, v] of Object.entries(r)) {
        const key = (k || "").trim();
        clean[key] = (v ?? "").toString().trim();
    }
    if (clean.noofpile !== undefined) {
        const n = parseInt(clean.noofpile, 10);
        clean.noofpile = Number.isFinite(n) ? n : null;
    }
    return clean;
}

function buildProfile(rows) {
    const columns = rows.length ? Object.keys(rows[0]) : [];
    const blanks = {};
    for (const c of columns) blanks[c] = 0;

    for (const r of rows) {
        for (const c of columns) {
            const v = r[c];
            if (v === null || v === undefined || (typeof v === "string" && v.trim() === "")) blanks[c]++;
        }
    }

    const totalCells = rows.length * columns.length || 1;
    const missingCells = Object.values(blanks).reduce((s, v) => s + v, 0);
    const missingRatio = missingCells / totalCells;

    return { rowCount: rows.length, columns, blanks, missingRatio };
}

function countBy(rows, field) {
    const out = {};
    for (const r of rows) {
        const key = (r[field] ?? "").toString().trim() || "(blank)";
        out[key] = (out[key] || 0) + 1;
    }
    return out;
}

function buildAggregates(rows) {
    return {
        girderStatus: countBy(rows, "girder_status"),
        pierStatus: countBy(rows, "pier_status"),
        context: countBy(rows, "context"),
        project: countBy(rows, "projectname")
    };
}

function topN(obj, n = 8) {
    return Object.entries(obj || {})
        .sort((a, b) => b[1] - a[1])
        .slice(0, n);
}

/* Polished dataset insight message (no bullets) */
function datasetInsightMessage(fileName, profile, aggregates) {
    const missingTop = topN(profile.blanks, 3)
        .map(([k, v]) => `${k}: ${v.toLocaleString()}`)
        .join(" • ") || "None";

    const girderTop = topN(aggregates.girderStatus, 3)
        .map(([k, v]) => `${k}: ${v.toLocaleString()}`)
        .join(" • ") || "—";

    // Polished markdown card (no bullets, cleaner spacing)
    const md = `
            <div class="datasetCard">
                <div class="datasetCard__header">
                    <div class="datasetCard__badge">
                        <svg viewBox="0 0 24 24" fill="none" class="datasetCard__icon">
                            <rect x="3" y="3" width="18" height="18" rx="2" stroke="currentColor" stroke-width="2" />
                            <path d="M3 9h18M9 21V9" stroke="currentColor" stroke-width="2" />
                        </svg>
                        Dataset loaded
                    </div>
                </div>

                <div class="datasetCard__body">
                    <div class="datasetCard__file">${fileName}</div>

                    <div class="datasetCard__grid">
                        <div class="datasetCard__stat">
                            <div class="datasetCard__label">Rows</div>
                            <div class="datasetCard__value">${profile.rowCount.toLocaleString()}</div>
                        </div>
                        <div class="datasetCard__stat">
                            <div class="datasetCard__label">Columns</div>
                            <div class="datasetCard__value">${profile.columns.length}</div>
                        </div>
                        <div class="datasetCard__stat">
                            <div class="datasetCard__label">Missing</div>
                            <div class="datasetCard__value">${(profile.missingRatio * 100).toFixed(1)}%</div>
                        </div>
                    </div>

                    <div class="datasetCard__meta">
                        <strong>Top missing:</strong> ${missingTop}
                    </div>
                    <div class="datasetCard__meta">
                        <strong>Top girder status:</strong> ${girderTop}
                    </div>
                </div>
            </div>
            `;

    // Charts with accurate labels from data
    const girder = topN(aggregates.girderStatus, 10);
    const pier = topN(aggregates.pierStatus, 10);
    const context = topN(aggregates.context, 8);

    const charts = [
        {
            title: "Girder status distribution",
            type: "bar",
            labels: girder.map(x => x[0]), // accurate label from data
            datasets: [{
                label: "Girder Status Count", // accurate legend
                data: girder.map(x => x[1])
            }]
        },
        {
            title: "Context split",
            type: "doughnut",
            labels: context.map(x => x[0]), // accurate label from data
            datasets: [{
                label: "Context Distribution", // accurate legend
                data: context.map(x => x[1])
            }]
        },
        {
            title: "Pier status distribution",
            type: "bar",
            labels: pier.map(x => x[0]), // accurate label from data
            datasets: [{
                label: "Pier Status Count", // accurate legend
                data: pier.map(x => x[1])
            }]
        }
    ];

    return { md, charts };
}
/* CSV/XLSX upload only triggers dataset info + charts */
async function readAsArrayBuffer(file) { return await file.arrayBuffer(); }

async function parseCSV(file) {
    const text = await file.text();
    const parsed = Papa.parse(text, { header: true, skipEmptyLines: "greedy" });
    const rows = (parsed.data || []).map(normalizeRow);
    const profile = buildProfile(rows);
    const aggregates = buildAggregates(rows);

    const chat = getActiveChat();
    if (chat) chat.dataset = { fileName: file.name, profile, aggregates };
    saveChats();

    const payload = buildDatasetPayload(file.name, rows, profile, aggregates);

    // Call both OpenRouter endpoints in sequence (or use Promise.all if you want parallel later)
    const tableAi = await callOpenRouterForTableAndInsights(
        payload,
        "Create a smart table preview and insights."
    );

    const chartAi = await callOpenRouterForInsightsAndCharts(
        payload,
        "Generate charts and insights."
    );

    // Single assistant node for everything
    const node = createMessageNode("assistant", "", false, Date.now());

    // Table + table insights + narrative
    if (tableAi?.table) {
        renderTableInMessage(node.contentWrap, rows, tableAi.table, profile);
    }
    if (tableAi?.insights) {
        renderInsightsInMessage(node.contentWrap, tableAi.insights);
    }
    if (tableAi?.report) {
        renderNarrativeReport(node.contentWrap, tableAi.report);
    }
    renderTableNotes(node.contentWrap, tableAi);

    // Charts + chart insights
    if (chartAi?.insights) {
        renderInsightsInMessage(node.contentWrap, chartAi.insights);
    }
    if (Array.isArray(chartAi?.charts)) {
        chartAi.charts.forEach(ch =>
            renderHighchartInMessage(node.contentWrap, ch)
        );
    }

    // Fallbacks (no extra JSON dumped into chat)
    if (!tableAi?.table && tableAi?.fallbackText) {
        pushMessage("assistant", tableAi.fallbackText, true);
    }
    if ((!chartAi?.charts || !chartAi.charts.length) && chartAi?.fallbackText) {
        pushMessage("assistant", chartAi.fallbackText, true);
    }
}


async function callOpenRouterForInsightsAndCharts(datasetPayload, userGoal = "Generate charts and insights") {
    const endpoint = "https://openrouter.ai/api/v1/chat/completions";

    const system = `
            Return ONLY valid JSON (no markdown) with:
            {
                "insights": {
                "headline": "string",
            "summary": "string",
            "key_metrics": [{"label":"Rows","value":"..."},{"label":"Columns","value":"..."},{"label":"Missing","value":"..."}],
            "highlights": ["string","string"]
    },
            "charts": [
            {
                "title": "string",
            "subtitle": "string",
            "type": "column|bar|line|pie|donut",
            "categories": ["A","B"],
            "series": [{"name":"Count", "data":[1,2] }]
      }
            ]
  }
            Rules: max 3 charts, use only labels from dataset payload.
            `.trim();

    // IMPORTANT: define this variable (don’t call it `user` if you aren’t defining it)
    const userMsg = `
            Goal: ${userGoal}

            Dataset JSON:
            ${JSON.stringify(datasetPayload)}
            `.trim();

    const { text } = await axiChatCompletion({
        messages: [
            { role: "system", content: system },
            { role: "user", content: userMsg }
        ],
        temperature: 0.2,
        max_tokens: 1200,
        // model: optional
    });

    const parsed = tryParseJsonStrict(text);
    return parsed ? parsed : { fallbackText: text };

}

async function parseXLSX(file) {
    const buf = await readAsArrayBuffer(file);
    const wb = XLSX.read(buf, { type: "array" });
    const sheetName = wb.SheetNames[0];
    const ws = wb.Sheets[sheetName];
    const json = XLSX.utils.sheet_to_json(ws, { defval: "" });
    const rows = json.map(normalizeRow);
    const profile = buildProfile(rows);
    const aggregates = buildAggregates(rows);

    const datasetName = `${file.name} / ${sheetName}`;

    const chat = getActiveChat();
    if (chat) chat.dataset = { fileName: datasetName, profile, aggregates };
    saveChats();

    const payload = buildDatasetPayload(file.name, rows, profile, aggregates);

    // Table + smart insights
    const tableAi = await callOpenRouterForTableAndInsights(
        payload,
        "Create a smart table preview and insights."
    );

    // Charts + chart insights
    const chartAi = await callOpenRouterForInsightsAndCharts(
        payload,
        "Generate charts and insights."
    );

    // Single assistant node
    const node = createMessageNode("assistant", "", false, Date.now());

    // Table + table insights + narrative
    if (tableAi?.table) {
        renderTableInMessage(node.contentWrap, rows, tableAi.table, profile);
    }
    if (tableAi?.insights) {
        renderInsightsInMessage(node.contentWrap, tableAi.insights);
    }
    if (tableAi?.report) {
        renderNarrativeReport(node.contentWrap, tableAi.report);
    }
    renderTableNotes(node.contentWrap, tableAi);

    // Charts + chart insights
    if (chartAi?.insights) {
        renderInsightsInMessage(node.contentWrap, chartAi.insights);
    }
    if (Array.isArray(chartAi?.charts)) {
        chartAi.charts.forEach(ch =>
            renderHighchartInMessage(node.contentWrap, ch)
        );
    }

    // Fallbacks
    if (!tableAi?.table && tableAi?.fallbackText) {
        pushMessage("assistant", tableAi.fallbackText, true);
    }
    if ((!chartAi?.charts || !chartAi.charts.length) && chartAi?.fallbackText) {
        pushMessage("assistant", chartAi.fallbackText, true);
    }
}



function extOf(name = "") {
    const i = name.lastIndexOf(".");
    return i >= 0 ? name.slice(i + 1).toLowerCase() : "";
}

el.fileInput?.addEventListener("change", async (e) => {
    const files = Array.from(e.target.files || []);
    e.target.value = "";
    if (!files.length) return;

    setBusy(true);
    try {
        for (const f of files) {
            const ext = extOf(f.name);
            if (ext === "csv") await parseCSV(f);
            else if (ext === "xlsx" || ext === "xls") await parseXLSX(f);
            else {
                // Non-analysable uploads accepted, but no dataset panel / charts
                pushMessage("assistant", `Received \`${f.name}\`. Upload CSV/XLSX for dataset insights.`, { markdown: true });
            }
        }
    } catch (err) {
        pushMessage("assistant", `Upload failed: ${err.message || err}`);
    } finally {
        setBusy(false);
    }
});

/* Chat send (text only here; plug backend if needed) */
// REPLACE your existing el.composer event listener with this:
el.composer.addEventListener("submit", async (e) => {
    e.preventDefault();
    await handleSend();
});


el.prompt.addEventListener("input", () => {
    el.prompt.style.height = "auto";
    el.prompt.style.height = Math.min(el.prompt.scrollHeight, 120) + "px";
    syncComposerButtons();
});

el.newChat?.addEventListener("click", () => { newChat(); });
el.reset?.addEventListener("click", () => {
    const chat = getActiveChat();
    if (!chat) return;
    chat.messages = [];
    chat.dataset = null;
    chat.title = "New chat";
    chat.updatedAt = Date.now();
    saveChats();
    renderThread();
    renderChatList();
});

/* init */
(function init() {
    state.chats = loadChats();
    ensureAtLeastOneChat();

    initHeaderUI();
    initHeaderActions();
    setActiveChat(state.activeChatId);
    syncComposerButtons();
})();

function setActiveChat(chatId) {
    state.activeChatId = chatId;
    saveChats();
    renderChatList();
    renderThread();
}

function newChat() {
    const c = { id: uid(), title: "New chat", createdAt: Date.now(), updatedAt: Date.now(), messages: [], dataset: null };
    state.chats.unshift(c);
    saveChats();
    setActiveChat(c.id);
}

const btn = document.getElementById("newChatFromHistory");

if (btn) {
    // 1. Clone the button to remove ALL existing event listeners (fixes double-add)
    const freshBtn = btn.cloneNode(true);
    btn.parentNode.replaceChild(freshBtn, btn);

    // 2. Add the SINGLE correct listener
    freshBtn.addEventListener("click", (e) => {
        e.stopPropagation(); // Prevents clicks from triggering "close" logic elsewhere
        e.preventDefault();  // Stops default browser behavior
        newChat();           // Creates the chat
        // Notice: We do NOT call closeHistory() here, so it stays open.
    });
}

/* ==========================================================================
   UPDATED handleSend Function
   (Replace your existing handleSend with this complete block)
   ========================================================================== */


async function handleSend() {
    const text = el.prompt.value.trim();

    // --- 1. Navigation Commands ---
    const upperText = text.toUpperCase();
    if (upperText === "AXI CONNECT") { window.location.href = "axi-connect.html"; return; }
    if (upperText === "AXI UPLOAD") { window.location.href = "axi-upload.html"; return; }
    if (upperText === "AXI ASK") { window.location.href = "axi-ask.html"; return; }

    const attachments = state.pendingAttachments.slice();

    // --- 2. Read File Content (Context Injection) ---
    // This block fixes the "500 lines" and "Excel" issues
    let fileContext = "";

    if (attachments.length > 0) {
        const file = attachments[0];
        const ext = file.name.split('.').pop().toLowerCase();
        let fullData = "";

        try {
            // FIX: Handle Excel Files (Binary -> Text)
            if (ext === 'xlsx' || ext === 'xls') {
                if (typeof XLSX !== 'undefined') {
                    const data = await file.arrayBuffer();
                    const workbook = XLSX.read(data, { type: 'array' });
                    const firstSheetName = workbook.SheetNames[0];
                    const worksheet = workbook.Sheets[firstSheetName];
                    // Convert to CSV so AI can read every row
                    fullData = XLSX.utils.sheet_to_csv(worksheet);
                } else {
                    fullData = "Error: XLSX library not loaded. Cannot parse Excel file.";
                    console.error("XLSX library missing");
                }
            }
            // FIX: Handle CSV/Text Files
            else {
                fullData = await readFileAsText(file);
            }

            // CRITICAL: Send EVERYTHING. No slice(), no 500 line limit.
            fileContext = `
\n--- SYSTEM: FILE CONTENT ATTACHED ---
File Name: ${file.name}
Row Count: ${fullData.split('\n').length}
Data Content:
${fullData}
--- END FILE CONTENT ---
`;
        } catch (err) {
            console.error("Failed to read file", err);
            fileContext = `\n[System: Failed to read file ${file.name}: ${err.message}]`;
        }
    }

    if (!text && !attachments.length) return;

    // 3. Ensure Chat Exists
    if (!getActiveChat()) { newChat(); }

    // 4. Clear UI state
    state.pendingAttachments = [];
    renderAttachmentTray();
    el.prompt.value = "";
    el.prompt.style.height = "auto";

    // 5. Push User Message to UI
    const userContent = text || (attachments.length ? `Uploaded ${attachments[0].name}` : "");
    pushMessage("user", userContent, { markdown: false, attachments });
    setBusy(true);

    // 6. Push Assistant Placeholder
    pushMessage("assistant", "Thinking...", { markdown: false });

    // 7. Prepare Payload for API
    const currentChat = getActiveChat();
    const assistantMsg = currentChat.messages[currentChat.messages.length - 1];

    try {
        // Build History
        const history = currentChat.messages
            .filter(m => m.id !== assistantMsg.id)
            .map(m => ({ role: m.role, content: m.content }));

        // [FIX] Inject the FULL file data into the system message
        if (fileContext) {
            history.unshift({ role: "system", content: fileContext });
        }

        history.unshift({ role: "system", content: SYSTEM_PROMPT_CHARTS });

        // [CRITICAL FIX] Pass 'null' as the 2nd argument. 
        // Do NOT pass a dataset object, or the AI will use the truncated summary.
        const answer = await callOpenAI(history, null);

        // --- 8. Handle Charts & Response ---
        let finalContent = answer;
        let chartDataList = [];
        let noFences = answer.replace(/```json/gi, "").replace(/```/g, "").replace(/\/\/.*$/gm, "");

        let jsonCandidate = null;
        const firstBrace = noFences.indexOf("{");
        if (firstBrace !== -1) {
            let depth = 0;
            for (let i = firstBrace; i < noFences.length; i++) {
                if (noFences[i] === "{") depth++;
                if (noFences[i] === "}") depth--;
                if (depth === 0) {
                    jsonCandidate = noFences.slice(firstBrace, i + 1);
                    break;
                }
            }
        }

        if (jsonCandidate) {
            try {
                const parsed = JSON.parse(jsonCandidate);
                if (parsed.chart) chartDataList.push(parsed.chart);
                else if (parsed.charts) {
                    parsed.charts.forEach(item => {
                        if (item.chart) chartDataList.push(item.chart);
                        else chartDataList.push(item);
                    });
                }
                if (chartDataList.length > 0) {
                    finalContent = `**Analysis Complete**\n\nGenerated ${chartDataList.length} chart(s) from the full dataset.`;
                }
            } catch (e) {
                console.warn("Chart parse error:", e);
            }
        }

        assistantMsg.content = finalContent;
        assistantMsg.markdown = true;
        assistantMsg.ts = Date.now();
        if (chartDataList.length > 0) assistantMsg.chartSpecs = chartDataList;

        saveChats();
        renderThread();

        setTimeout(() => {
            const allMsgs = document.querySelectorAll(".msg.assistant .bubble");
            const lastBubble = allMsgs[allMsgs.length - 1];
            if (lastBubble) {
                if (chartDataList.length > 0 && typeof renderHighchartInMessage === 'function') {
                    chartDataList.forEach(spec => renderHighchartInMessage(lastBubble, spec));
                }
                if (finalContent.length > 200 && typeof renderReportActions === 'function') {
                    renderReportActions(lastBubble, finalContent);
                }
            }
        }, 100);

    } catch (err) {
        assistantMsg.content = `Error: ${err.message}`;
        saveChats();
        renderThread();
    } finally {
        setBusy(false);
        syncComposerButtons();
    }
}




function initHeaderActions() {
    const btnLinks = document.querySelector('.headerBtn[title="Links"]');
    const btnImages = document.querySelector('.headerBtn[title="Images"]');
    const btnVideos = document.querySelector('.headerBtn[title="Videos"]');
    const btnMenu = document.querySelector('.iconBtn[title="Menu"]');
    const btnShare = document.querySelector(".shareBtn");
    const modelToggle = document.querySelector(".modelToggle");

    if (!btnLinks && !btnImages && !btnVideos && !btnMenu && !btnShare && !modelToggle) return;

    btnLinks?.addEventListener("click", () => openHeaderDrawer("links"));
    btnImages?.addEventListener("click", () => openHeaderDrawer("images"));
    btnVideos?.addEventListener("click", () => openHeaderDrawer("videos"));
    btnMenu?.addEventListener("click", () => openHeaderDrawer("menu"));
    btnShare?.addEventListener("click", () => handleShareActiveChat());
    modelToggle?.addEventListener("click", () => openHeaderDrawer("models"));
}

function ensureHeaderDrawerEls() {
    const root = document.getElementById("headerDrawer");
    const titleEl = document.getElementById("headerDrawerTitle");
    const bodyEl = document.getElementById("headerDrawerBody");
    if (!root || !titleEl || !bodyEl) throw new Error("headerDrawer elements missing");

    // Close handlers (once)
    if (!root.dataset.wired) {
        root.dataset.wired = "1";
        root.addEventListener("click", (e) => {
            const t = e.target;
            if (t && t.dataset && t.dataset.close) closeHeaderDrawer();
        });
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape") closeHeaderDrawer();
        });
    }

    return { root, titleEl, bodyEl };
}

function openHeaderDrawer(kind) {
    const { root, titleEl, bodyEl } = ensureHeaderDrawerEls();
    const chat = getActiveChat(); // from your existing code [file:126]
    const resources = collectChatResources(chat);

    root.classList.remove("headerDrawer--hidden");
    root.setAttribute("aria-hidden", "false");

    if (kind === "links") {
        titleEl.textContent = `Links (${resources.links.length})`;
        bodyEl.innerHTML = renderLinksList(resources.links);
        return;
    }

    if (kind === "images") {
        titleEl.textContent = `Images (${resources.images.length})`;
        bodyEl.innerHTML = renderImagesGrid(resources.images);
        return;
    }

    if (kind === "videos") {
        titleEl.textContent = `Videos (${resources.videos.length})`;
        bodyEl.innerHTML = renderLinksList(resources.videos);
        return;
    }

    if (kind === "menu") {
        titleEl.textContent = "Menu";
        bodyEl.innerHTML = `
          <div class="drawerActions">
            <button class="drawerBtn" type="button" data-action="export-pdf">Export as PDF</button>
            <button class="drawerBtn" type="button" data-action="export-md">Export as Markdown</button>
            <button class="drawerBtn" type="button" data-action="export-docx">Export as DOCX</button>
      
            <div style="height:10px"></div>
      
            <button class="drawerBtn" type="button" data-action="export-json">Export chat JSON</button>
            <button class="drawerBtn" type="button" data-action="clear">Clear current chat</button>
          </div>
        `;

        bodyEl.querySelector('[data-action="export-pdf"]')?.addEventListener("click", async () => {
            await exportActiveChatPdf();
            closeHeaderDrawer();
        });

        bodyEl.querySelector('[data-action="export-md"]')?.addEventListener("click", async () => {
            await exportActiveChatMarkdown();
            closeHeaderDrawer();
        });

        bodyEl.querySelector('[data-action="export-docx"]')?.addEventListener("click", async () => {
            await exportActiveChatDocx();
            closeHeaderDrawer();
        });

        bodyEl.querySelector('[data-action="export-json"]')?.addEventListener("click", () => {
            exportActiveChatJson();
            // exportActiveChatJson() already calls closeHeaderDrawer() in your code; ok if double-closed. [file:178]
        });

        bodyEl.querySelector('[data-action="clear"]')?.addEventListener("click", () => {
            const c = getActiveChat();
            if (!c) return;
            c.messages = [];
            c.dataset = null;
            c.updatedAt = Date.now();
            saveChats();
            renderThread();
            closeHeaderDrawer();
        });

        return;
    }


    if (kind === "models") {
        titleEl.textContent = "Model (UI)";
        bodyEl.innerHTML = `
        <div class="drawerNote">
          Hook this up to your OpenRouter "model" field if you want true switching.
        </div>
        <div class="drawerActions">
          <button class="drawerBtn" type="button" data-model="Assistant">Assistant</button>
          <button class="drawerBtn" type="button" data-model="Analyst">Analyst</button>
        </div>
      `;

        bodyEl.querySelectorAll("[data-model]").forEach((b) => {
            b.addEventListener("click", () => {
                const name = b.getAttribute("data-model");
                const label = document.querySelector(".modelToggle__name");
                if (label) label.textContent = name || "Assistant";
                closeHeaderDrawer();
            });
        });

        return;
    }
}

function closeHeaderDrawer() {
    const root = document.getElementById("headerDrawer");
    if (!root) return;
    root.classList.add("headerDrawer--hidden");
    root.setAttribute("aria-hidden", "true");
}

/* ---------- resource extraction ---------- */

function collectChatResources(chat) {
    const out = { links: new Set(), images: new Set(), videos: new Set() };
    if (!chat || !Array.isArray(chat.messages)) {
        return { links: [], images: [], videos: [] };
    }

    for (const m of chat.messages) {
        const text = typeof m.content === "string" ? m.content : "";
        for (const u of extractUrls(text)) {
            if (isVideoUrl(u)) out.videos.add(u);
            else if (isImageUrl(u)) out.images.add(u);
            else out.links.add(u);
        }
    }

    return {
        links: Array.from(out.links),
        images: Array.from(out.images),
        videos: Array.from(out.videos),
    };
}

function extractUrls(text) {
    const urls = new Set();

    // Markdown images: ![alt](url)
    const mdImg = /!\[[^\]]*]\((https?:\/\/[^)\s]+)\)/gi;
    for (const m of text.matchAll(mdImg)) urls.add(m[1]);

    // HTML: href/src
    const href = /href=["'](https?:\/\/[^"']+)["']/gi;
    for (const m of text.matchAll(href)) urls.add(m[1]);

    const src = /src=["'](https?:\/\/[^"']+)["']/gi;
    for (const m of text.matchAll(src)) urls.add(m[1]);

    // Plain URLs
    const plain = /(https?:\/\/[^\s)<>"]+)/gi;
    for (const m of text.matchAll(plain)) urls.add(m[1]);

    return Array.from(urls);
}

function isImageUrl(u) {
    return /\.(png|jpg|jpeg|gif|webp|svg)(\?|#|$)/i.test(u);
}

function isVideoUrl(u) {
    return (
        /(youtube\.com\/watch|youtu\.be\/|vimeo\.com\/)/i.test(u) ||
        /\.(mp4|webm|mov)(\?|#|$)/i.test(u)
    );
}

/* ---------- renderers ---------- */

function renderLinksList(list) {
    if (!list.length) return `<div class="drawerEmpty">Nothing found yet.</div>`;
    const items = list
        .map((u) => {
            let host = "";
            try { host = new URL(u).hostname; } catch { }
            return `
          <a class="drawerLink" href="${u}" target="_blank" rel="noopener noreferrer">
            <div class="drawerLink__url">${u}</div>
            ${host ? `<div class="drawerLink__host">${host}</div>` : ""}
          </a>
        `;
        })
        .join("");
    return `<div class="drawerList">${items}</div>`;
}

function renderImagesGrid(list) {
    if (!list.length) return `<div class="drawerEmpty">No images found in this chat.</div>`;
    const items = list
        .map((u) => `<a class="drawerImg" href="${u}" target="_blank" rel="noopener noreferrer"><img src="${u}" alt=""></a>`)
        .join("");
    return `<div class="drawerGrid">${items}</div>`;
}

/* ---------- share / export ---------- */

async function handleShareActiveChat() {
    const chat = getActiveChat();
    if (!chat) return;

    const text = (chat.messages || [])
        .slice(-10)
        .map((m) => `${m.role}: ${String(m.content || "").slice(0, 200)}`)
        .join("\n");

    // Web Share API if available, else copy
    if (navigator.share) {
        await navigator.share({ title: chat.title || "Chat", text, url: location.href });
        return;
    }

    await navigator.clipboard.writeText(text);
    // Optional: show a toast if you have one
}

function safeFileName(name) {
    return String(name || "chat")
        .replace(/[\\/:*?"<>|]+/g, "-")
        .replace(/\s+/g, " ")
        .trim()
        .slice(0, 80);
}

function stripHtmlToText(s) {
    const str = String(s || "");
    if (!str.trim()) return "";
    const doc = new DOMParser().parseFromString(str, "text/html");
    return (doc.body.textContent || "").trim();
}

function downloadBlob(blob, filename) {
    const a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    a.remove();
    setTimeout(() => URL.revokeObjectURL(a.href), 1000);
}

function safeFileName(name) {
    return String(name || "chat")
        .replace(/[\\/:*?"<>|]+/g, "-")
        .replace(/\s+/g, " ")
        .trim()
        .slice(0, 80);
}

function stripHtmlToText(s) {
    const str = String(s || "");
    if (!str.trim()) return "";
    const doc = new DOMParser().parseFromString(str, "text/html");
    return (doc.body.textContent || "").trim();
}

function downloadBlob(blob, filename) {
    const a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    a.remove();
    setTimeout(() => URL.revokeObjectURL(a.href), 1000);
}

function chatToMarkdown(chat) {
    const title = chat?.title || "Chat";
    const exportedAt = new Date().toLocaleString();

    const lines = [];
    lines.push(`# ${title}`);
    lines.push(``);
    lines.push(`_Exported: ${exportedAt}_`);
    lines.push(``);
    lines.push(`---`);
    lines.push(``);

    (chat?.messages || []).forEach((m) => {
        const role = m?.role === "assistant" ? "Assistant" : "User";
        const ts = m?.ts ? new Date(m.ts).toLocaleString() : "";
        const heading = ts ? `## ${role} — ${ts}` : `## ${role}`;

        let content = "";
        if (m?.markdown) {
            content = String(m?.content || "");
            if (content.trim().startsWith("<")) {
                content = stripHtmlToText(content);
            }
        } else {
            const raw = String(m?.content || "");
            content = raw.trim().startsWith("<") ? stripHtmlToText(raw) : raw;
        }

        lines.push(heading);
        lines.push("");
        lines.push(content || "_(No content)_");
        lines.push("");
        lines.push("---");
        lines.push("");
    });

    return lines.join("\n");
}


async function exportActiveChatMarkdown() {
    const data = getExportableThread();
    if (!data.messages.length) return alert("No messages found to export.");

    const lines = [];
    lines.push(`# ${data.title}`);
    lines.push(``);
    lines.push(`_Exported: ${data.exportedAt}_`);
    lines.push(``);
    lines.push(`---`);
    lines.push(``);

    data.messages.forEach((m) => {
        const roleLabel = m.role === "assistant" ? "Assistant" : "User";
        const ts = m.ts ? new Date(m.ts).toLocaleString() : (m.tsText || "");
        lines.push(`## ${roleLabel}${ts ? ` — ${ts}` : ""}`);
        lines.push(``);
        lines.push(String(m.content || "").trim() || "—");
        lines.push(``);
        lines.push(`---`);
        lines.push(``);
    });

    const blob = new Blob([lines.join("\n")], { type: "text/markdown;charset=utf-8" });
    downloadBlob(blob, `${safeFileName(data.title)}.md`);
}


async function exportActiveChatPdf() {
    const data = getExportableThread();
    if (!data.messages.length) {
        alert("No messages found to export");
        return;
    }

    const title = data.title;
    const exportedAt = data.exportedAt;
    const messages = data.messages;

    let messagesHtml = "";
    messages.forEach((m) => {
        const roleLabel = m?.role === "assistant" ? "Assistant" : "User";
        const ts = m?.ts ? new Date(m.ts).toLocaleString() : (m?.tsText || "");
        const timestamp = ts
            ? `<div style="color:#6b7280;font-size:11px;margin-bottom:6px;">${escapeHtml(ts)}</div>`
            : "";

        let raw = String(m?.content || "").trim();

        // If markdown, render it to HTML for the PDF window
        let bodyHtml = "";
        if (m?.markdown) {
            bodyHtml = renderMarkdown(raw || "_No content_");
        } else {
            if (raw.startsWith("<")) raw = stripHtmlToText(raw);
            bodyHtml = `<div style="white-space:pre-wrap;word-break:break-word;line-height:1.6;color:#374151">${escapeHtml(raw || "No content")}</div>`;
        }

        messagesHtml += `
              <div style="margin-bottom:24px;padding-bottom:18px;border-bottom:1px solid #e5e7eb">
                <div style="font-weight:600;margin-bottom:4px;color:#111827">${escapeHtml(roleLabel)}</div>
                ${timestamp || ""}
                <div>${bodyHtml}</div>
              </div>
            `;

    });

    const w = window.open("", "_blank");
    if (!w) {
        alert("Popup blocked. Allow popups to export PDF.");
        return;
    }

    w.document.open();
    w.document.write(`
      <!DOCTYPE html>
      <html>
        <head>
          <meta charset="utf-8" />
          <title>${escapeHtml(title)}</title>
          <style>
            @page { margin: 1.5cm; }
            body {
              font-family: ui-sans-serif, system-ui, -apple-system, "Segoe UI", Roboto, Arial, sans-serif;
              padding: 20px;
              color: #111827;
              line-height: 1.6;
              position: relative;
            }
            /* Axpert logo watermark in top-right corner */
            body::before {
              content: "";
              position: fixed;
              top: 20px;
              right: 20px;
              width: 80px;
              height: 80px;
              background-image: url('${logoUrl}');
              background-size: contain;
              background-repeat: no-repeat;
              opacity: 0.3;
              z-index: 1000;
            }
            h1 { margin: 0 0 8px; font-size: 24px; color: #111827; position: relative; z-index: 1; }
            .meta { color: #6b7280; font-size: 12px; margin-bottom: 24px; position: relative; z-index: 1; }
            @media print { 
              body { padding: 0; }
              body::before { position: absolute; }
            }
          </style>
        </head>
        <body>
          <h1>${escapeHtml(title)}</h1>
          <div class="meta">Exported: ${escapeHtml(exportedAt)}</div>
          ${messagesHtml}
          <script>
            window.onload = () => window.print();
          </script>
        </body>
      </html>
    `);
    w.document.close();
}


async function exportActiveChatDocx() {
    const data = getExportableThread();
    if (!data.messages.length) {
        alert("No messages found to export");
        return;
    }

    const docxLib = window.docx;
    if (!docxLib) {
        alert(
            "DOCX export requires the docx library. Add:\n" +
            "<script src=\"https://unpkg.com/docx@8.5.0/build/index.umd.js\"></script>"
        );
        return;
    }

    const { Document, Packer, Paragraph, TextRun, HeadingLevel } = docxLib;

    const title = data.title;
    const exportedAt = data.exportedAt;
    const messages = data.messages;

    const children = [];
    children.push(new Paragraph({ text: title, heading: HeadingLevel.HEADING_1 }));
    children.push(
        new Paragraph({
            children: [new TextRun({ text: `Exported: ${exportedAt}`, color: "6B7280", size: 20 })],
        })
    );
    children.push(new Paragraph(""));

    messages.forEach((m) => {
        const roleLabel = m?.role === "assistant" ? "Assistant" : "User";
        const ts = m?.ts ? new Date(m.ts).toLocaleString() : (m?.tsText || "");
        const heading = ts ? `${roleLabel} — ${ts}` : roleLabel;

        let raw = String(m?.content || "");
        if (raw.trim().startsWith("<")) raw = stripHtmlToText(raw);
        const text = raw || "(No content)";

        children.push(new Paragraph({ text: heading, heading: HeadingLevel.HEADING_2 }));

        // Preserve line breaks in DOCX by splitting into separate paragraphs
        String(text).split(/\r?\n/).forEach((line) => {
            children.push(new Paragraph(line));
        });

        children.push(new Paragraph(""));
    });

    const doc = new Document({ sections: [{ properties: {}, children }] });
    const blob = await Packer.toBlob(doc);
    downloadBlob(blob, `${safeFileName(title)}.docx`);
}


function escapeMd(s) {
    return String(s || "").replace(/\|/g, "\\|").replace(/\r?\n/g, " ").trim();
}

function tableToMarkdown(tableEl, maxRows = 25) {
    const rows = Array.from(tableEl.querySelectorAll("tr"));
    if (!rows.length) return "";

    const matrix = rows.map((tr) =>
        Array.from(tr.children).map((cell) => escapeMd(cell.innerText || cell.textContent))
    );

    const header = matrix[0];
    const colCount = header.length || Math.max(...matrix.map((r) => r.length));
    const norm = (r) => {
        const out = r.slice(0, colCount);
        while (out.length < colCount) out.push("");
        return out;
    };

    const head = norm(header);
    const sep = new Array(colCount).fill("---");
    const body = matrix.slice(1, 1 + maxRows).map(norm);

    const line = (arr) => `| ${arr.join(" | ")} |`;

    return [line(head), line(sep), ...body.map(line)].join("\n");
}



function exportActiveChatJson() {
    const chat = getActiveChat();
    if (!chat) return;

    const blob = new Blob([JSON.stringify(chat, null, 2)], { type: "application/json" });
    const a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.download = `${(chat.title || "chat").replace(/[^\w\-]+/g, "_")}.json`;
    document.body.appendChild(a);
    a.click();
    a.remove();
    closeHeaderDrawer();
}

function initHeaderUI() {
    initModelDropdown();
    initResourcesDrawer();
}

function initModelDropdown() {
    const toggle = document.getElementById("modelToggle");
    const menu = document.getElementById("modelMenu");
    const label = document.getElementById("activeModelLabel");
    if (!toggle || !menu || !label) return;

    function open() {
        menu.classList.remove("modelMenu--hidden");
        menu.setAttribute("aria-hidden", "false");
    }
    function close() {
        menu.classList.add("modelMenu--hidden");
        menu.setAttribute("aria-hidden", "true");
    }
    function isOpen() {
        return !menu.classList.contains("modelMenu--hidden");
    }

    toggle.addEventListener("click", (e) => {
        // allow clicking items without immediately toggling twice
        if (e.target.closest(".modelMenu__item")) return;
        isOpen() ? close() : open();
    });

    menu.querySelectorAll("[data-model]").forEach((btn) => {
        btn.addEventListener("click", () => {
            const name = btn.getAttribute("data-model") || "Assistant";
            label.textContent = name;
            close();

            // optional: store selection
            localStorage.setItem("axpert_model_name", name);
        });
    });

    document.addEventListener("click", (e) => {
        if (!toggle.contains(e.target)) close();
    });

    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape") close();
    });

    // restore
    const saved = localStorage.getItem("axpert_model_name");
    if (saved) label.textContent = saved;
}

function escapeHtml(str) {
    return String(str)
        .replace(/&/g, String.fromCharCode(38) + 'amp;')
        .replace(/</g, String.fromCharCode(38) + 'lt;')
        .replace(/>/g, String.fromCharCode(38) + 'gt;')
        .replace(/"/g, String.fromCharCode(38) + 'quot;')
        .replace(/'/g, String.fromCharCode(38) + '#39;');
}

function normalizeText(s) {
    return String(s || "")
        .replace(/\u00A0/g, " ")
        .replace(/[ \t]+\n/g, "\n")
        .replace(/\n{3,}/g, "\n\n")
        .trim();
}

function detectRoleFromDom(messageEl) {
    // Your DOM shows assistant avatar is an <img>, user avatar is text. [file:183]
    const avatar = messageEl.querySelector(".message__avatar, .messageavatar");
    const hasImg = !!avatar?.querySelector("img");
    return hasImg ? "assistant" : "user";
}

function escapeMd(s) {
    return String(s || "")
        .replace(/\|/g, "\\|")
        .replace(/\r?\n/g, " ")
        .trim();
}

function tableToMarkdown(tableEl, maxRows = 25) {
    const rows = Array.from(tableEl.querySelectorAll("tr"));
    if (!rows.length) return "";

    const matrix = rows.map((tr) =>
        Array.from(tr.children).map((cell) => escapeMd(cell.innerText || cell.textContent))
    );

    const header = matrix[0] || [];
    const colCount = header.length || Math.max(0, ...matrix.map((r) => r.length));

    const norm = (r) => {
        const out = (r || []).slice(0, colCount);
        while (out.length < colCount) out.push("");
        return out;
    };

    const head = norm(header);
    const sep = new Array(colCount).fill("---");
    const body = matrix.slice(1, 1 + maxRows).map(norm);

    const line = (arr) => `| ${arr.join(" | ")} |`;
    return [line(head), line(sep), ...body.map(line)].join("\n");
}

function escapeMd(s) {
    return String(s || "")
        .replace(/\|/g, "\\|")
        .replace(/\r?\n/g, " ")
        .trim();
}

function tableToMarkdown(tableEl, maxRows = 25) {
    const rows = Array.from(tableEl.querySelectorAll("tr"));
    if (!rows.length) return "";

    const matrix = rows.map((tr) =>
        Array.from(tr.children).map((cell) => escapeMd(cell.innerText || cell.textContent))
    );

    const header = matrix[0] || [];
    const colCount = header.length || Math.max(0, ...matrix.map((r) => r.length));

    const norm = (r) => {
        const out = (r || []).slice(0, colCount);
        while (out.length < colCount) out.push("");
        return out;
    };

    const head = norm(header);
    const sep = new Array(colCount).fill("---");
    const body = matrix.slice(1, 1 + maxRows).map(norm);

    const line = (arr) => `| ${arr.join(" | ")} |`;
    return [line(head), line(sep), ...body.map(line)].join("\n");
}


function answerCardToMarkdown(cardEl) {
    const out = [];
    const title =
        cardEl.querySelector(".answerCard__label, .answerCardlabel")?.innerText || "";
    if (title.trim()) out.push(`### ${normalizeText(title)}`);

    cardEl.querySelectorAll("h3").forEach(h => {
        const t = normalizeText(h.innerText || "");
        if (t) out.push(`#### ${t}`);
    });

    cardEl.querySelectorAll("p").forEach(p => {
        const t = normalizeText(p.innerText || "");
        if (t) out.push(t);
    });

    return out.join("\n\n");
}


function extractMessageTextFromDom(messageEl) {
    const wrap =
        messageEl.querySelector(".message__content") ||
        messageEl.querySelector(".messagecontent");
    if (!wrap) return "";

    const clone = wrap.cloneNode(true);

    // Remove timestamps/meta
    clone.querySelectorAll(".message__meta, .messagemeta").forEach((n) => n.remove());

    const parts = [];

    // Export rendered dataset tables as markdown tables
    const tableCards = Array.from(clone.querySelectorAll("article.tableCard"));
    if (tableCards.length) {
        tableCards.forEach((card) => {
            const title = normalizeText(
                card.querySelector(".tableCardheader")?.innerText || "Data preview"
            );
            parts.push(`### ${title}`);

            const table = card.querySelector("table");
            if (table) parts.push(tableToMarkdown(table, 25));
            else parts.push(normalizeText(card.innerText || ""));
        });

        // Include any additional cards that appear in the same message
        Array.from(
            clone.querySelectorAll("article.insightsCard, article.answerCard, article.tableNotesCard, .messageChart")
        ).forEach((el) => {
            const t = normalizeText(el.innerText || "");
            if (t) parts.push(t);
            if (el.matches("article.answerCard")) {
                const md = answerCardToMarkdown(el);
                if (md) parts.push(md);
                return;
            }
        });

        return normalizeText(parts.join("\n\n"));
    }

    // Non-table message: keep line breaks
    return normalizeText(clone.innerText || clone.textContent || "");
}



function getExportableThread() {
    const chat = getActiveChat();
    const title = chat?.title || "Chat";
    const exportedAt = new Date().toLocaleString();

    // 1) Primary: saved messages (normal chat flow uses pushMessage -> chat.messages) [file:183]
    if (chat?.messages?.length) {
        const msgs = chat.messages
            .map((m) => {
                let raw = String(m?.content || "");
                if (raw.trim().startsWith("<")) raw = stripHtmlToText(raw);
                return {
                    role: m?.role === "assistant" ? "assistant" : "user",
                    ts: m?.ts,
                    tsText: m?.ts ? new Date(m.ts).toLocaleString() : "",
                    content: normalizeText(raw),
                };
            })
            .filter((m) => m.content);

        if (msgs.length) return { title, exportedAt, messages: msgs };
    }

    // 2) Fallback: scrape rendered UI (this is your current case: chat.messages is 0) [file:183]
    const nodes = Array.from(document.querySelectorAll("#messages .thread .message"));
    const messages = nodes
        .map((n) => {
            const role = detectRoleFromDom(n);
            const tsText = normalizeText(
                n.querySelector(".message__meta, .messagemeta")?.textContent || ""
            );
            const content = extractMessageTextFromDom(n);
            return { role, content, markdown: true, tsText };
        })
        .filter((m) => m.content);

    return { title, exportedAt, messages };
}



function initResourcesDrawer() {
    const drawer = document.getElementById("resourcesDrawer");
    if (!drawer) return;

    const btnLinks = document.querySelector('.headerBtn[title="Links"]');
    const btnImages = document.querySelector('.headerBtn[title="Images"]');
    const btnVideos = document.querySelector('.headerBtn[title="Videos"]');

    const tabs = Array.from(drawer.querySelectorAll(".drawerTab[data-tab]"));
    const panels = Array.from(drawer.querySelectorAll(".drawerPanel[data-panel]"));

    function setTab(tabId) {
        tabs.forEach((t) => {
            const active = t.dataset.tab === tabId;
            t.classList.toggle("is-active", active);
            t.setAttribute("aria-selected", active ? "true" : "false");
        });
        panels.forEach((p) => {
            p.hidden = p.dataset.panel !== tabId;
        });
    }

    function open(tabId) {
        hydrateResources();      // refresh content each time you open
        setTab(tabId);

        drawer.classList.remove("drawer--hidden");
        drawer.setAttribute("aria-hidden", "false");
    }

    function close() {
        drawer.classList.add("drawer--hidden");
        drawer.setAttribute("aria-hidden", "true");
    }

    function hydrateResources() {
        const chat = getActiveChat();
        const res = collectChatResources(chat);

        const linksPanel = drawer.querySelector('[data-panel="links"]');
        const imagesPanel = drawer.querySelector('[data-panel="images"]');
        const videosPanel = drawer.querySelector('[data-panel="videos"]');

        linksPanel.innerHTML = renderLinksList(res.links);
        imagesPanel.innerHTML = renderImagesGrid(res.images);
        videosPanel.innerHTML = renderLinksList(res.videos);
    }

    // open drawer at specific tab
    btnLinks?.addEventListener("click", () => open("links"));
    btnImages?.addEventListener("click", () => open("images"));
    btnVideos?.addEventListener("click", () => open("videos"));

    // tab switching inside drawer
    tabs.forEach((t) => t.addEventListener("click", () => setTab(t.dataset.tab)));

    // close actions
    drawer.addEventListener("click", (e) => {
        const t = e.target;
        if (t && t.dataset && t.dataset.close) close();
    });

    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape") close();
    });
}

/* --- helpers reused from earlier approach --- */

function collectChatResources(chat) {
    const out = { links: new Set(), images: new Set(), videos: new Set() };
    if (!chat || !Array.isArray(chat.messages)) return { links: [], images: [], videos: [] };

    for (const m of chat.messages) {
        const text = typeof m.content === "string" ? m.content : "";
        for (const u of extractUrls(text)) {
            if (isVideoUrl(u)) out.videos.add(u);
            else if (isImageUrl(u)) out.images.add(u);
            else out.links.add(u);
        }
    }

    return { links: [...out.links], images: [...out.images], videos: [...out.videos] };
}

function extractUrls(text) {
    const urls = new Set();
    const plain = /(https?:\/\/[^\s)<>"]+)/gi;
    for (const m of text.matchAll(plain)) urls.add(m[1]);
    const mdImg = /!\[[^\]]*]\((https?:\/\/[^)\s]+)\)/gi;
    for (const m of text.matchAll(mdImg)) urls.add(m[1]);
    const href = /href=["'](https?:\/\/[^"']+)["']/gi;
    for (const m of text.matchAll(href)) urls.add(m[1]);
    const src = /src=["'](https?:\/\/[^"']+)["']/gi;
    for (const m of text.matchAll(src)) urls.add(m[1]);
    return [...urls];
}

function isImageUrl(u) { return /\.(png|jpg|jpeg|gif|webp|svg)(\?|#|$)/i.test(u); }
function isVideoUrl(u) { return /(youtube\.com\/watch|youtu\.be\/|vimeo\.com\/)/i.test(u) || /\.(mp4|webm|mov)(\?|#|$)/i.test(u); }

function renderLinksList(list) {
    if (!list.length) return `<div class="drawerEmpty">Nothing found yet.</div>`;
    return `<div class="drawerList">${list.map((u) => `<a class="drawerLink" href="${u}" target="_blank" rel="noopener noreferrer">${u}</a>`).join("")
        }</div>`;
}

function renderImagesGrid(list) {
    if (!list.length) return `<div class="drawerEmpty">No images found in this chat.</div>`;
    return `<div class="drawerGrid">${list.map((u) => `<a class="drawerImg" href="${u}" target="_blank" rel="noopener noreferrer"><img src="${u}" alt=""></a>`).join("")
        }</div>`;
}

/**
 *  Author: Salman Sheriff
 * This function gets the message text from the axi command pallete and executes the existing handlesend function
 * 
 * Note: Don't remove. This is a helper function for Axi Command pallette 
 * @param {*} text 
 * @returns 
 */
function sendAxiMessageToAxibot(text) {
    if (!el.prompt || !el.composer) {
        console.log("Axi Bot: Input elements not found."); 
        return; 
    }

    el.prompt.value = text; 

    el.prompt.dispatchEvent(new Event('input', {bubbles: true})); 

    handleSend(); 
}

window.sendAxiMessageToAxibot = sendAxiMessageToAxibot; 




