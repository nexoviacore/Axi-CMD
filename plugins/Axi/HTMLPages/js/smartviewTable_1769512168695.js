

window._smartviewFullData = null;
// SmartView table is dynamic (filters add/remove columns, infinite scroll appends rows).
// DataTables tends to break on dynamic column counts and throws disruptive alerts.
// Keep DataTables only for export (#hiddenTable), and disable it for the main table by default.
window._smartviewDisableDataTables = (typeof window._smartviewDisableDataTables === 'undefined')
  ? true
  : window._smartviewDisableDataTables;

window._entity = window._entity || {
  metaData: [

  ],
  listJson: [],
  pageSize: 10,
  keyField: "recordid",
  entityName: "smartlist_users",
  entityTransId: "axusr",
  // helper to check invalid values
  inValid: function (v) {
    return v === null || v === undefined || (typeof v === "string" && v.trim() === "");
  },
  // placeholder editEntity & openEntityForm so click handlers don't break
  editEntity: function (recordId, rno) {
    console.log("editEntity called for", recordId, rno);
    return false;
  },
  openEntityForm: function (entityName, transId, recordId, keyValue, rowNo) {
    console.log("openEntityForm", entityName, transId, recordId, keyValue, rowNo);
    return false;
  }
};

// Ensure minimal Entity-Common helpers exist (some SmartView pages don't load the full product Entity-Common.js).
window._entityCommon = window._entityCommon || {};
window._entityCommon.inValid = window._entityCommon.inValid || function (v) {
  return v === null || v === undefined || (typeof v === "string" && v.trim() === "");
};
window._entityCommon.isValid = window._entityCommon.isValid || function (v) {
  return !window._entityCommon.inValid(v);
};
window._entityCommon.getFirstDayOfWeek = window._entityCommon.getFirstDayOfWeek || function (currentDate) {
  currentDate = new Date(currentDate);
  const day = currentDate.getDay();
  const diff = currentDate.getDate() - day + (day === 0 ? -6 : 1); // Monday as first day (product behavior)
  return new Date(currentDate.setDate(diff));
};
// Date presets used by Entity-Filter.js (Today/Yesterday/This week/…)
window._entityCommon.getDatesBasedonSelection = window._entityCommon.getDatesBasedonSelection || function (selectionvalue) {
  const fromToObj = { from: "", to: "" };
  const culture = (typeof dtCulture !== 'undefined' && dtCulture) ? dtCulture : "en-us";
  const fmt = (culture === "en-us") ? "MM/DD/YYYY" : "DD/MM/YYYY";
  const dateObj = new Date();

  switch (selectionvalue) {
    case "customOption":
      break;
    case "todayOption":
      fromToObj.from = fromToObj.to = moment(dateObj).format(fmt);
      break;
    case "yesterdayOption":
      dateObj.setDate(dateObj.getDate() - 1);
      fromToObj.from = fromToObj.to = moment(dateObj).format(fmt);
      break;
    case "tomorrowOption":
      dateObj.setDate(dateObj.getDate() + 1);
      fromToObj.from = fromToObj.to = moment(dateObj).format(fmt);
      break;
    case "this_weekOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      fromToObj.from = moment(d).format(fmt);
      d.setDate(d.getDate() + 6);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_weekOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(d.getDate() - 7);
      fromToObj.from = moment(d).format(fmt);
      d.setDate(d.getDate() + 6);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_weekOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(d.getDate() + 7);
      fromToObj.from = moment(d).format(fmt);
      d.setDate(d.getDate() + 6);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "this_monthOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(1);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 1);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_monthOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(1);
      d.setMonth(d.getMonth() - 1);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 1);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_monthOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(1);
      d.setMonth(d.getMonth() + 1);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 1);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "this_quarterOption": {
      const d = new Date();
      const q = Math.floor((d.getMonth() + 3) / 3);
      d.setDate(1);
      d.setMonth((q * 3) - 3);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 3);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_quarterOption": {
      const d = new Date();
      let q = Math.floor((d.getMonth() + 3) / 3) - 1;
      if (q === 0) { q = 4; d.setFullYear(d.getFullYear() - 1); }
      d.setDate(1);
      d.setMonth((q * 3) - 3);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 3);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_quarterOption": {
      const d = new Date();
      let q = Math.floor((d.getMonth() + 3) / 3) + 1;
      if (q === 5) { q = 1; d.setFullYear(d.getFullYear() + 1); }
      d.setDate(1);
      d.setMonth((q * 3) - 3);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 3);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "this_yearOption": {
      const d = new Date();
      d.setDate(1);
      d.setMonth(0);
      fromToObj.from = moment(d).format(fmt);
      d.setFullYear(d.getFullYear() + 1);
      d.setMonth(0);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_yearOption": {
      const d = new Date();
      d.setFullYear(d.getFullYear() - 1);
      d.setDate(1);
      d.setMonth(0);
      fromToObj.from = moment(d).format(fmt);
      d.setFullYear(d.getFullYear() + 1);
      d.setMonth(0);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_yearOption": {
      const d = new Date();
      d.setFullYear(d.getFullYear() + 1);
      d.setDate(1);
      d.setMonth(0);
      fromToObj.from = moment(d).format(fmt);
      d.setFullYear(d.getFullYear() + 1);
      d.setMonth(0);
      d.setDate(0);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
  }

  return fromToObj;
};
window._entityCommon.getDatesBasedonSelectionForBetweenFilter = window._entityCommon.getDatesBasedonSelectionForBetweenFilter || function (selectionvalue) {
  const fromToObj = { from: "", to: "" };
  const fmt = "DD-MMM-YYYY";
  const dateObj = new Date();

  switch (selectionvalue) {
    case "customOption":
      break;
    case "todayOption":
      fromToObj.from = moment(dateObj).format(fmt);
      dateObj.setDate(dateObj.getDate() + 1);
      fromToObj.to = moment(dateObj).format(fmt);
      break;
    case "yesterdayOption":
      dateObj.setDate(dateObj.getDate() - 1);
      fromToObj.from = moment(dateObj).format(fmt);
      dateObj.setDate(dateObj.getDate() + 1);
      fromToObj.to = moment(dateObj).format(fmt);
      break;
    case "tomorrowOption":
      dateObj.setDate(dateObj.getDate() + 1);
      fromToObj.from = moment(dateObj).format(fmt);
      dateObj.setDate(dateObj.getDate() + 1);
      fromToObj.to = moment(dateObj).format(fmt);
      break;
    case "this_weekOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      fromToObj.from = moment(d).format(fmt);
      d.setDate(d.getDate() + 7);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_weekOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(d.getDate() - 7);
      fromToObj.from = moment(d).format(fmt);
      d.setDate(d.getDate() + 7);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_weekOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(d.getDate() + 7);
      fromToObj.from = moment(d).format(fmt);
      d.setDate(d.getDate() + 7);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "this_monthOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(1);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 1);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_monthOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(1);
      d.setMonth(d.getMonth() - 1);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 1);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_monthOption": {
      const d = window._entityCommon.getFirstDayOfWeek(new Date());
      d.setDate(1);
      d.setMonth(d.getMonth() + 1);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 1);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "this_quarterOption": {
      const d = new Date();
      const q = Math.floor((d.getMonth() + 3) / 3);
      d.setDate(1);
      d.setMonth((q * 3) - 3);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 3);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_quarterOption": {
      const d = new Date();
      let q = Math.floor((d.getMonth() + 3) / 3) - 1;
      if (q === 0) { q = 4; d.setFullYear(d.getFullYear() - 1); }
      d.setDate(1);
      d.setMonth((q * 3) - 3);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 3);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_quarterOption": {
      const d = new Date();
      let q = Math.floor((d.getMonth() + 3) / 3) + 1;
      if (q === 5) { q = 1; d.setFullYear(d.getFullYear() + 1); }
      d.setDate(1);
      d.setMonth((q * 3) - 3);
      fromToObj.from = moment(d).format(fmt);
      d.setMonth(d.getMonth() + 3);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "this_yearOption": {
      const d = new Date();
      d.setDate(1);
      d.setMonth(0);
      fromToObj.from = moment(d).format(fmt);
      d.setFullYear(d.getFullYear() + 1);
      d.setMonth(0);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "last_yearOption": {
      const d = new Date();
      d.setFullYear(d.getFullYear() - 1);
      d.setDate(1);
      d.setMonth(0);
      fromToObj.from = moment(d).format(fmt);
      d.setFullYear(d.getFullYear() + 1);
      d.setMonth(0);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
    case "next_yearOption": {
      const d = new Date();
      d.setFullYear(d.getFullYear() + 1);
      d.setDate(1);
      d.setMonth(0);
      fromToObj.from = moment(d).format(fmt);
      d.setFullYear(d.getFullYear() + 1);
      d.setMonth(0);
      d.setDate(d.getDate() + 1);
      fromToObj.to = moment(d).format(fmt);
      break;
    }
  }

  return fromToObj;
};

/* --------------------------
   ADS Picker Modal Functions (from SmartList)
   -------------------------- */

function getAdsList() {
  return new Promise((resolve) => {
    const params = {
      adsNames: ["ds_getsmartlists"],
      refreshCache: false,
      sqlParams: {},
      props: { ADS: true, pageno: 1, pagesize: 500 },
    };
    
    // Use parent.GetDataFromAxList if available, otherwise window
    const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent : window;
    
    caller.GetDataFromAxList(
      params,
      function (resp) {
        try {
          let parsed = resp;
          if (typeof resp === "string") parsed = JSON.parse(resp);
          if (parsed && parsed.d && typeof parsed.d === "string") parsed = JSON.parse(parsed.d);
          
          let listRaw = [];
          if (parsed && parsed.result && Array.isArray(parsed.result.data)) {
            parsed.result.data.forEach((it) => {
              if (Array.isArray(it.data)) listRaw = listRaw.concat(it.data);
            });
          }
          
          if (listRaw.length === 0 && parsed && parsed.result && parsed.result.data && 
              parsed.result.data[0] && Array.isArray(parsed.result.data[0].data)) {
            listRaw = parsed.result.data[0].data;
          }
          
          const list = listRaw
            .map((r, idx) => {
              const name = r.sqlname || r.adsname || r.name || r.adscode || r.code || "ads_" + idx;
              const caption = r.caption || r.title || r.sqlname || r.name || r.adsname || name;
              return {
                name,
                caption,
                paramsMeta: r.paramsMeta || [],
                raw: r,
              };
            })
            .filter((i) => i && i.name);
          
          resolve(list);
        } catch (e) {
          console.warn("getAdsList parse failed", e);
          resolve([]);
        }
      },
      function (err) {
        console.warn("getAdsList failed", err);
        resolve([]);
      }
    );
  });
}

function showAdsPickerModal() {
  // Create modal backdrop
  const backdrop = document.createElement('div');
  backdrop.id = 'adsPickerModalBackdrop';
  backdrop.style.cssText = `
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 9999;
  `;
  
  // Create modal content
  const modal = document.createElement('div');
  modal.id = 'adsPickerModal';
  modal.style.cssText = `
    background: white;
    border-radius: 12px;
    padding: 24px;
    width: 80%;
    max-width: 900px;
    max-height: 100%;
    overflow: hidden;
    display: flex;
    flex-direction: column;
    box-shadow: 0 20px 60px rgba(3, 6, 23, 0.28);
  `;
  
  modal.innerHTML = `
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px;">
      <h3 style="margin: 0; font-size: 20px; font-weight: 700;">Select ADS to View</h3>
      <button id="closeAdsPicker" style="background: none; border: none; font-size: 24px; cursor: pointer; color: #666;">×</button>
    </div>
    
    <div style="margin-bottom: 20px;">
      <div style="display: flex; gap: 12px; align-items: center;">
        <input id="adsPickerSearch" placeholder="Search ADS..." 
               style="flex: 1; padding: 10px 14px; border-radius: 8px; border: 1px solid #e0e0e0; font-size: 14px;">
      </div>
    </div>
    
    <div style="flex: 1; overflow: auto; min-height: 300px;">
      <div id="adsPickerList" style="display: grid; grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); gap: 12px;"></div>
    </div>
    
    <div style="display: flex; justify-content: flex-end; gap: 12px; margin-top: 20px; padding-top: 20px; border-top: 1px solid #eee;">
      <button id="adsPickerCancel" style="padding: 10px 20px; border: 1px solid #e0e0e0; border-radius: 8px; background: white; cursor: pointer;">Cancel</button>
      <button id="adsPickerApply" style="padding: 10px 20px; border: none; border-radius: 8px; background: #2563eb; color: white; cursor: pointer; font-weight: 600;">Select ADS</button>
    </div>
  `;
  
  backdrop.appendChild(modal);
  document.body.appendChild(backdrop);
  
  // Load ADS list
  getAdsList().then(list => {
    window._adsList = list;
    renderAdsList(list);
    
    // Setup search
    const searchInput = document.getElementById('adsPickerSearch');
    if (searchInput) {
      searchInput.addEventListener('input', (e) => {
        const query = e.target.value.toLowerCase();
        const filtered = list.filter(ad => 
          (ad.caption || '').toLowerCase().includes(query) || 
          (ad.name || '').toLowerCase().includes(query)
        );
        renderAdsList(filtered);
      });
    }
  });
  
  // Event listeners
  document.getElementById('closeAdsPicker').addEventListener('click', closeAdsPickerModal);
  document.getElementById('adsPickerCancel').addEventListener('click', closeAdsPickerModal);
  document.getElementById('adsPickerApply').addEventListener('click', applySelectedAds);
  
  // Close on backdrop click
  backdrop.addEventListener('click', (e) => {
    if (e.target === backdrop) {
      closeAdsPickerModal();
    }
  });
}

function renderAdsList(list) {
  const container = document.getElementById('adsPickerList');
  if (!container) return;
  
  if (!list || list.length === 0) {
    container.innerHTML = '<div style="grid-column: 1 / -1; text-align: center; padding: 40px; color: #666;">No ADS found</div>';
    return;
  }
  
  let html = '';
  list.forEach((ad, idx) => {
    html += `
      <div class="ads-card" data-index="${idx}" 
           style="border: 1px solid #e0e0e0; border-radius: 8px; padding: 16px; cursor: pointer; transition: all 0.2s;">
        <div style="font-weight: 600; margin-bottom: 4px;">${escapeHtml(ad.caption || ad.name)}</div>
        <div style="font-size: 12px; color: #666; margin-bottom: 8px;">${escapeHtml(ad.name)}</div>
        <div style="font-size: 11px; color: #888;">Click to select</div>
      </div>
    `;
  });
  
  container.innerHTML = html;
  
  // Add click handlers
  document.querySelectorAll('.ads-card').forEach(card => {
    card.addEventListener('click', function() {
      // Remove selection from all cards
      document.querySelectorAll('.ads-card').forEach(c => {
        c.style.background = 'white';
        c.style.borderColor = '#e0e0e0';
      });
      
      // Select this card
      this.style.background = '#f0f7ff';
      this.style.borderColor = '#2563eb';
      
      // Store selection
      const idx = parseInt(this.dataset.index);
      window._selectedAd = window._adsList[idx];
    });
  });
}

function closeAdsPickerModal() {
  const modal = document.getElementById('adsPickerModalBackdrop');
  if (modal) {
    modal.remove();
  }
  window._adsList = null;
  window._selectedAd = null;
}

function applySelectedAds() {
  if (!window._selectedAd) {
    alert('Please select an ADS first');
    return;
  }

  const selectedAdsName = window._selectedAd.name;
  console.log('Selected ADS:', selectedAdsName);

  // Close modal
  closeAdsPickerModal();

  // Ensure global entity state updated and page header set
  window._entity = window._entity || {};
  window._entity.adsName = selectedAdsName;

  const titleEl = document.getElementById('EntityTitle') || document.querySelector('.page-header-title');
  if (titleEl) {
    titleEl.textContent = selectedAdsName;
  }
  document.title = selectedAdsName;

  // Initialize or update controller with selected ADS
  if (window.smartTableController) {
    const ctrl = window.smartTableController;
    const prevAds = (ctrl.adsName || '').toString();
    ctrl.adsName = selectedAdsName;

    // ADS changed -> clear cached metadata so filters/hyperlinks use the right schema
    if (!prevAds || prevAds.toLowerCase() !== selectedAdsName.toLowerCase()) {
      ctrl.lastAdsMeta = null;
      ctrl._adsMetaFor = null;
    }

    ctrl.resetPaging();

    // Prefetch ADS metadata (and persist to localStorage) so Filters open instantly later.
    try { if (typeof ctrl.ensureAdsMetadata === 'function') ctrl.ensureAdsMetadata(); } catch (e) {}

    ctrl.loadNextPage();
  } else {
    window.smartTableController = new SmartViewTableController({
      adsName: selectedAdsName,
      pageSize: 100,
      currentPage: 1,
      sorting: []
    });
  }
}


function escapeHtml(s) {
  if (s == null) return '';
  return String(s).replace(/[&<>"']/g, function(m) {
    return {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#39;'
    }[m];
  });
}

/* --------------------------
   Row count manager (minimal)
   -------------------------- */

window.rowCountManager = window.rowCountManager || {
  total: 0,
  loaded: 0,
  lastPage: false,
  setTotal: function (n) { this.total = Number(n) || 0; },
  setLoadedRecords: function (n) { this.loaded = n || 0; },
  setCurrentView: function (v) { this.view = v; },
  setLastPageReached: function (b) { this.lastPage = !!b; },
  refresh: function () { /* no-op UI placeholder */ },
  attachToView: function () { /* no-op placeholder */ }
};

/* --------------------------
   Utility helpers (stubs)
   -------------------------- */

function formatDateString(v) {
  if (!v) return "";
  try {
    if (typeof v === "string" && /^\d{2}\/\d{2}\/\d{4}$/.test(v)) return v;
    const d = new Date(v);
    if (!isNaN(d)) return d.toISOString().split("T")[0];
    return String(v);
  } catch (e) {
    return String(v);
  }
}

function formatNumberBasedOnMillions(v) { return v; }

function parseFilePath() { return {}; }
function getFileType() { return ""; }
function getIconClass() { return ""; }
function downloadFileFromPath() { console.log("download stub"); }

/* --------------------------
   getFieldDataType helper
   -------------------------- */

function getFieldDataType(fldProps) {
  if (!fldProps) return "TEXT";
  if (fldProps.cdatatype) return fldProps.cdatatype;
  if (fldProps.fdatatype) {
    switch (fldProps.fdatatype) {
      case "d": return "Date";
      case "n": return "Numeric";
      case "b": return "Check box";
      default: return "Text";
    }
  }
  return (fldProps.fldtype ? fldProps.fldtype : "Text");
}

/* --------------------------
   generateHTMLBasedOnDataType
   -------------------------- */

function generateHTMLBasedOnDataType(fldProps, rowData) {
  var fldkey = fldProps.fldname;
  var fldtype = getFieldDataType(fldProps);
  var fldcap = fldProps.fldcap ? fldProps.fldcap.replaceAll("*", "") : '';
  var fProps = fldProps.props;
  var fldValue = rowData[fldkey.toLowerCase()];

  if (fldkey.toLowerCase() === 'modifiedon' || fldkey.toLowerCase() === 'username' || 
      fldkey.toLowerCase() === 'createdby' || fldkey.toLowerCase() === 'createdon') {
    return '';
  }

  if (_entity.inValid(fldValue) && fldtype.toUpperCase() != "BUTTON")
    return '';

  let html = '';
  const tooltipAttr = '';

  if (fldtype.toUpperCase() === 'LARGE TEXT') {
    html = `<div class="Data-fields-items Department-field" ${tooltipAttr} data-name="${fldcap.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
                <p class="task-description moretext" style="margin-bottom:0px !important;">${fldValue}</p>
             </div>`;
  } else if (fldtype.toUpperCase() === 'DATE') {
    var formattedDate = formatDateString(fldValue);
    html = `<div class="Data-fields-items Date-field" ${tooltipAttr} data-name="${fldcap.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
              <span class="txt-bold Data-field-value">${formattedDate}</span>
            </div>`;
  } else if (fldtype.toUpperCase() === 'EMAIL') {
    html = `<div class="Data-fields-items Email-field truncate" ${tooltipAttr} data-name="${fldcap.toLowerCase().replaceAll("*", "").replaceAll(" ", "")}">
              <span class="txt-bold Data-field-value" data-text="${fldValue}" onclick="showPopup(this)">${fldValue}</span>
            </div>`;
  } else {
    if (fldValue === "T" || fldValue === "F") {
      html = `<div class="Data-fields-items Department-field" ${tooltipAttr}>
                <div class="d-flex align-items-center">
                  <div class="form-check ms-1">
                    <input class="form-check-input" type="checkbox" ${fldValue === "T" ? 'checked' : ''} readonly disabled>
                  </div>
                </div>
              </div>`;
    } else {
      html = `<div class="Data-fields-items Department-field" ${tooltipAttr}>
                <span class="txt-bold Data-field-value">${fldValue}</span>
              </div>`;
    }
  }
  return html;
}

function formatFieldName(field) {
  return field
    .replace(/([A-Z])/g, ' $1')
    .replace(/createdby/g, 'Created By')
    .replace(/modifiedby/g, 'Modified By')
    .replace(/createdon/g, 'Created On')
    .replace(/modifiedon/g, 'Modified On')
    .replace(/^ /, '');
}

// Best-effort caption -> data-key guesser (helps when ADS metadata fldname doesn't match response keys)
// Example: "PR Number" -> "prnum"
function smartviewGuessDataKeyFromCaption(caption) {
  if (!caption) return '';
  const words = String(caption)
    .replace(/\*/g, '')
    .replace(/[_\-]/g, ' ')
    .replace(/[^a-zA-Z0-9 ]/g, ' ')
    .trim()
    .split(/\s+/)
    .filter(Boolean);

  if (!words.length) return '';

  const mapped = words.map(w => {
    const lw = w.toLowerCase();
    if (lw === 'number' || lw === 'no' || lw === 'num') return 'num';
    return lw;
  });

  return mapped.join('');
}

function buildSmartviewHeaderCell(caption, fieldName) {
  const title = escapeHtml(caption || '');
  const fld = (fieldName || '').toString().trim();
  if (!fld) return `<th>${title}</th>`;
  const safeFld = escapeHtml(fld);
  return `
    <th class="sv-data-header" data-field="${safeFld}">
      <div class="sv-header-cell">
        <span class="sv-header-title">${title}</span>
        <button type="button" class="sv-header-menu-btn" data-field="${safeFld}" title="Column menu" aria-label="Column menu">
          <span class="sv-header-menu-icon">...</span>
        </button>
      </div>
    </th>
  `;
}

function getKeyField() {
  if (!_entity.metaData || !_entity.metaData.length) return null;
  const preferred = ["recordid", "id", "username"];
  for (let p of preferred) {
    const found = _entity.metaData.find(m => m.fldname.toLowerCase() === p.toLowerCase());
    if (found) return found;
  }
  return _entity.metaData[0];
}

function toggleSelectAll(source) {
  const checkboxes = document.querySelectorAll('.rowCheckbox');
  checkboxes.forEach(checkbox => checkbox.checked = source.checked);
}

function showPopup(element) {
  var text = element.getAttribute('data-text') || '';
  alert(text);
}

function destroySmartviewDataTable() {
  try {
    if (!(window.jQuery && $.fn && $.fn.dataTable)) return;

    // Hide disruptive alerts from DataTables (TN/18 etc). We'll manage the table ourselves.
    try { if ($.fn.dataTable.ext) $.fn.dataTable.ext.errMode = 'none'; } catch (e) {}

    const $tbl = $('#table-body_Container .table');
    if (!$tbl.length) return;

    // Only destroy if this particular table is a DataTable instance.
    if ($.fn.dataTable.isDataTable($tbl)) {
      // NOTE: do NOT pass `true` here (that removes the table element from the DOM).
      try { $tbl.DataTable().destroy(); } catch (e) { /* ignore */ }
    }

    // If a wrapper is still around (partial init / failed destroy), unwrap the table instead of deleting it.
    try {
      const $wrap = $('#table-body_Container .dataTables_wrapper');
      if ($wrap.length) {
        const $t = $wrap.find('table.table').first();
        if ($t.length) {
          $('#table-body_Container').append($t);
        }
        $wrap.remove();
      }
    } catch (e) {}
  } catch (e) {
    console.warn('destroySmartviewDataTable failed', e);
  }
}

function initializeDataTable() {
  try {
    if (window._smartviewDisableDataTables) return;
    if (!(window.jQuery && $.fn && $.fn.dataTable)) return;

    // Keep DataTables quiet (avoid alert popups on dynamic column changes)
    try { if ($.fn.dataTable.ext) $.fn.dataTable.ext.errMode = 'none'; } catch (e) {}

    const $tbl = $('#table-body_Container .table');
    if (!$tbl.length) return;
    if ($.fn.dataTable.isDataTable($tbl)) return;

    $tbl.DataTable({
      paging: false,
      searching: false,
      info: false,
      destroy: true
    });
  } catch (e) {
    // no-op if DataTables not present
  }
}

/* --------------------------
   createTableViewHTML
   -------------------------- */

   function createTableViewHTML(listJson, _pageNo) {
   console.log('=== createTableViewHTML called ===');
   console.log('listJson has', listJson.length, 'rows');
   console.log('First row:', listJson[0]);
   console.log('_entity.metaData:', _entity.metaData);
    
    // Initialize _entity if it doesn't exist
    window._entity = window._entity || {};
    
    // Initialize navigationRecords as empty array
    if (!Array.isArray(_entity.navigationRecords)) {
      _entity.navigationRecords = [];
    }
    
    if (rowCountManager) {
      const totalLoaded = listJson.length;
      rowCountManager.setLoadedRecords(totalLoaded);
      rowCountManager.setCurrentView('table');
  
      if (listJson.length < _entity.pageSize && listJson.length > 0) {
        rowCountManager.setLastPageReached(true);
      }
  
      rowCountManager.refresh();
  
      setTimeout(() => {
        rowCountManager.attachToView();
      }, 100);
    }
  
    let tableBodyContainer = $('#table-body_Container');
    let tableExists = tableBodyContainer.find('table').length > 0;
    let keyCol = _entity.keyField || '';
    let html = '';
    let excludedFields = new Set(['transid', 'ftransid']);
  
   let hideTransid = !listJson.some(rowData => rowData[keyCol]);
  
    // Check if any row has axrowoptions to decide whether to show the column
    const hasRowOptions = listJson.some(rowData => {
      return rowData.axrowoptions || rowData.axRowOptions || rowData.axRowoptions;
    });
  
   const isEmptyDataset = !listJson || listJson.length === 0;

    // If server-side groupby/select_columns is active, render only those fields.
    const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
    const isGroupedView = !!(ctrl && Array.isArray(ctrl.groupby_columns) && ctrl.groupby_columns.length > 0);
    let allowedFields = null;
    if (ctrl && Array.isArray(ctrl.select_columns) && ctrl.select_columns.length) {
      allowedFields = new Set();
      ctrl.select_columns.forEach(sc => {
        const fn = smartviewSelectExprToFieldName(sc);
        if (fn) allowedFields.add(fn.toLowerCase());
      });
    }
    if (ctrl && Array.isArray(ctrl.groupby_columns) && ctrl.groupby_columns.length) {
      if (!allowedFields) allowedFields = new Set();
      ctrl.groupby_columns.forEach(gc => {
        const fn = smartviewSelectExprToFieldName(gc);
        if (fn) allowedFields.add(fn.toLowerCase());
      });
    }

    // Build set of fields that actually contain data across rows
    const fieldsWithData = new Set();
    // Also track fields that are present in the response objects (even if values are empty/null)
    const fieldsPresent = new Set();
    listJson.forEach(rowData => {
      for (let field in rowData) {
        if (rowData.hasOwnProperty(field)) {
          const fieldLower = field.toLowerCase();
          if (allowedFields && !allowedFields.has(fieldLower)) continue;
          if (!excludedFields.has(fieldLower)) fieldsPresent.add(fieldLower);
          if (rowData[field] !== null && rowData[field] !== undefined && String(rowData[field]).trim() !== '' && !excludedFields.has(fieldLower)) {
            fieldsWithData.add(fieldLower);
          }
        }
      }
    });
  
    console.log('fieldsWithData:', Array.from(fieldsWithData));
  
    // If keyField missing from metaData, try to infer a sensible one
    if (!keyCol || !_entity.metaData.some(field => {
      const fieldName = (field.fldname || '').toString().toLowerCase();
      return fieldName === keyCol.toLowerCase();
    })) {
      const keyField = getKeyField();
      keyCol = keyField ? keyField.fldname : _entity.keyField;
    }
  
    const keyColLower = (keyCol || '').toString().toLowerCase();
    console.log('keyCol:', keyCol, 'keyColLower:', keyColLower);
  
    // Determine whether the key column should actually be rendered
    const keyFieldPresentInMeta = (_entity.metaData || []).some(f => (f.fldname || '').toString().toLowerCase() === keyColLower);
    let keyFieldHasData = fieldsWithData.has(keyColLower) || keyFieldPresentInMeta; // MOVE THIS LINE UP
    if (allowedFields && keyColLower && !allowedFields.has(keyColLower)) keyFieldHasData = false;
    console.log('keyFieldPresentInMeta:', keyFieldPresentInMeta, 'keyFieldHasData:', keyFieldHasData);
  
    /*
      New metadata merging logic:
      - Start with existing _entity.metaData (if present)
      - Append any fields found in data (fieldsWithData) that are not present in metaData
      - Respect explicit hide === 'T' where possible
    */
    const metaMap = {};
    const originalMeta = Array.isArray(_entity.metaData) ? _entity.metaData.slice() : [];
    const aliasByOriginal = {};
  
    originalMeta.forEach(item => {
      const metaKey = (item.fldname || '').toString().trim().toLowerCase();
      if (!metaKey) return;

      let effectiveKey = metaKey;
      // If ADS metadata key doesn't exist in the response data, try a caption-based alias (PR Number -> prnum, etc).
      if (!fieldsPresent.has(metaKey)) {
        const cap = (item.fldcap || item.fldcaption || item.caption || '').toString();
        const guess = smartviewGuessDataKeyFromCaption(cap);
        if (guess && fieldsPresent.has(guess)) effectiveKey = guess;
      }

      aliasByOriginal[metaKey] = effectiveKey;
      const cloned = Object.assign({}, item, { fldname: effectiveKey });
      if (effectiveKey !== metaKey) {
        cloned._svOriginalFldname = metaKey;
      }

      // If this is a sum column already present in metadata, derive a friendly caption.
      if (effectiveKey.toLowerCase().startsWith('sum_')) {
        const base = effectiveKey.substring(4);
        const baseMeta = originalMeta.find(m => ((m.fldname || '').toString().toLowerCase() === base.toLowerCase())) || null;
        const baseCap = baseMeta ? (baseMeta.fldcap || formatFieldName(baseMeta.fldname || base)) : formatFieldName(base);
        cloned.fldcap = `Sum ${baseCap}`;
        cloned.fdatatype = cloned.fdatatype || 'n';
        cloned.cdatatype = cloned.cdatatype || 'Numeric';
      }

      // Avoid overwriting an already-mapped effectiveKey (keep the first metadata entry).
      if (!metaMap[effectiveKey]) metaMap[effectiveKey] = cloned;
    });
  
    // Add any data fields not present in metaMap
    Array.from(fieldsWithData).forEach(f => {
      if (!metaMap[f] && !excludedFields.has(f)) {
        let cap = formatFieldName(f);
        let fdt = 't';
        let cdt = 'Text';

        // If this is a groupby sum field (e.g., sum_ordqty), derive caption from base field.
        if (f.toLowerCase().startsWith('sum_')) {
          const base = f.substring(4);
          const baseMeta =
            originalMeta.find(m => ((m.fldname || '').toString().toLowerCase() === base.toLowerCase())) ||
            metaMap[base];
          if (baseMeta) {
            const baseCap = baseMeta.fldcap || formatFieldName(baseMeta.fldname || base);
            cap = `Sum ${baseCap}`;
          } else {
            cap = `Sum ${formatFieldName(base)}`;
          }
          fdt = 'n';
          cdt = 'Numeric';
        }

        metaMap[f] = { fldname: f, fldcap: cap, fdatatype: fdt, cdatatype: cdt, listingfld: 'T' };
      }
    });
  
    // Build ordered metadata array: keep original order, then append fieldsWithData order
    const metaOrdered = [];
    originalMeta.forEach(item => {
      const metaKey = (item.fldname || '').toString().trim().toLowerCase();
      if (!metaKey) return;
      const effectiveKey = aliasByOriginal[metaKey] || metaKey;
      if (metaMap[effectiveKey]) { metaOrdered.push(metaMap[effectiveKey]); delete metaMap[effectiveKey]; }
    });
    Array.from(fieldsWithData).forEach(f => {
      if (metaMap[f]) { metaOrdered.push(metaMap[f]); delete metaMap[f]; }
    });
    // any remaining keys (edge cases) append
    Object.keys(metaMap).forEach(k => { metaOrdered.push(metaMap[k]); });
  
    // filteredMetaData is now the merged metadata we will render from
    const filteredMetaData = metaOrdered;
  
    const addedFields = new Set();
    
    const dynamicFields = ['modifiedby', 'modifiedon', 'createdby', 'createdon'];
  
    // If there are modification fields present in rows, ensure they are present
    if (_entity.modificationFields && typeof _entity.modificationFields === 'string') {
      const modificationFieldsArray = _entity.modificationFields.split(",");
  
      if (modificationFieldsArray.length > 0) {
        dynamicFields.forEach(field => {
          if (modificationFieldsArray.includes(field)) {
            const fl = field.toLowerCase();
            if (!filteredMetaData.some(m => m.fldname === fl)) {
              filteredMetaData.push({ fldname: fl, fldcap: formatFieldName(field), fdatatype: 't', cdatatype: 'Text', listingfld: 'T' });
            }
          }
        });
      }
    } else {
      dynamicFields.forEach(field => {
        const fieldLower = field.toLowerCase();
        if (listJson.some(rowData => rowData[fieldLower]) && !filteredMetaData.some(m => m.fldname === fieldLower)) {
          filteredMetaData.push({ fldname: fieldLower, fldcap: formatFieldName(field), fdatatype: 't', cdatatype: 'Text', listingfld: 'T' });
        }
      });
    }
  
    // Reset navigationRecords for current page
    _entity.navigationRecords = [];

    // If the column set changed (common when toggling filters), rebuild the whole table.
    // Otherwise, header stays from previous render and body cells get misaligned.
    try {
      const headerKeys = [];
      headerKeys.push('__select__');
      if (hasRowOptions) headerKeys.push('__actions__');
      if (keyFieldHasData) headerKeys.push(`key:${keyColLower}`);

      const sigAdded = new Set();
      filteredMetaData.forEach(field => {
        const fieldName = (field.fldname || '').toString().toLowerCase();
        if (fieldName === keyColLower) return;
        if (excludedFields.has(fieldName)) return;
        if (allowedFields && !allowedFields.has(fieldName)) return;
        // If field isn't present in the response payload at all, don't render it (avoids blank columns).
        // For empty datasets, we still render headers from metadata so the user can see the schema.
        if (!isEmptyDataset && !fieldsPresent.has(fieldName)) return;
        const hasData = fieldsWithData.has(fieldName);
        const allowShow = hasData || (field.listingfld && (field.listingfld === 'T' || field.listingfld === 't'));
        if (allowShow && !sigAdded.has(fieldName)) {
          headerKeys.push(fieldName);
          sigAdded.add(fieldName);
        }
      });

      const newSig = headerKeys.join('|');
      const oldSig = tableBodyContainer.attr('data-sv-header-sig') || '';
      tableBodyContainer.attr('data-sv-header-sig', newSig);

      if (tableExists && oldSig && oldSig !== newSig) {
        try { destroySmartviewDataTable(); } catch (e) {}
        tableBodyContainer.empty();
        tableExists = false;
      }
    } catch (e) {
      console.warn('SmartView header signature check failed', e);
    }
  
    if (!tableExists) {
      html += '<div class="table-responsive"><table class="table table-striped">';
  
      html += '<thead class="sticky-header"><tr>';
      html += '<th><input type="checkbox" id="selectAllCheckbox" onclick="toggleSelectAll(this)"></th>';
      
      // ADD ACTION COLUMN HEADER IF NEEDED
      if (hasRowOptions) {
        html += '<th style="width: 40px;">Actions</th>';
      }
      
      // Render key header only if it is present (avoid rendering "--" placeholder header)
      if (keyFieldHasData) {
        const keyField = (_entity.metaData || []).find(field => {
          const fieldName = (field.fldname || '').toString().toLowerCase();
          return fieldName === keyColLower;
        });
        const keyCaption = keyField ? (keyField.fldcap || formatFieldName(keyField.fldname)) : formatFieldName(keyCol);
        html += buildSmartviewHeaderCell(keyCaption, keyColLower);
      } // else: intentionally skip rendering the key column header
  
      // Render headers using the merged metadata but only for fields that actually have data and are not the key
      filteredMetaData.forEach(field => {
        const fieldName = (field.fldname || '').toString().toLowerCase();
        // skip key col
        if (fieldName === keyColLower) return;
        // skip excluded fields
        if (excludedFields.has(fieldName)) return;
        if (allowedFields && !allowedFields.has(fieldName)) return;
        // If field isn't present in the response payload at all, don't render it (avoids blank columns).
        // For empty datasets, we still render headers from metadata so the user can see the schema.
        if (!isEmptyDataset && !fieldsPresent.has(fieldName)) return;
        // show only columns that have data OR explicitly marked listingfld === 'T'
        const hasData = fieldsWithData.has(fieldName);
        const allowShow = hasData || (field.listingfld && (field.listingfld === 'T' || field.listingfld === 't'));
        if (allowShow) {
          if (!addedFields.has(fieldName)) {
            html += buildSmartviewHeaderCell(field.fldcap || formatFieldName(field.fldname), fieldName);
            addedFields.add(fieldName);
            console.log('Added header for:', fieldName);
          }
        }
      });
      html += '</tr></thead><tbody>';
    } else {
      tableBodyContainer.find('tbody').empty();
    }
  
    console.log('Headers added:', Array.from(addedFields));
  
    listJson.forEach((rowData, index) => {
      html += `<tr>`;
      if (isGroupedView) {
        html += `<td><button type="button" class="sv-expand-btn" data-rowindex="${index}" data-expanded="false">+</button><input type="checkbox" class="rowCheckbox" data-index="${index}" data-recordid="${rowData.recordid || ''}"></td>`;
      } else {
        html += `<td><input type="checkbox" class="rowCheckbox" data-index="${index}" data-recordid="${rowData.recordid || ''}"></td>`;
      }
      
      // ADD ACTION CELL
      if (hasRowOptions) {
        const actions = parseAxRowOptionsField(rowData);
        if (actions && actions.length > 0) {
          html += `<td class="action-cell" style="text-align: center; padding: 4px;">
                    <button type="button" class="action-arrow-btn" 
                            data-rowindex="${index}"
                            title="Actions"
                            style="border: none; background: none; cursor: pointer; padding: 2px 4px; border-radius: 4px;">
                      <span style="font-size: 18px;">⋮</span>
                    </button>
                  </td>`;
        } else {
          html += `<td></td>`;
        }
      }
      
      const entityName = _entity.entityName;
      const transId = _entity.entityTransId || rowData.transid || "axusr";
      const recordId = rowData.recordid || '';
      
      // SAFE push to navigationRecords
      if (Array.isArray(_entity.navigationRecords)) {
        _entity.navigationRecords.push(recordId);
      } else {
        _entity.navigationRecords = [recordId];
      }
      
      const rowNo = rowData.rno || index;
  
      // Render key cell only if key column should be shown
      if (keyFieldHasData) {
        if (fieldsWithData.has(keyColLower)) {
          let keyValue = '';
          // Try to get key value from rowData
          if (rowData[keyColLower] !== undefined) {
            keyValue = rowData[keyColLower];
          } else if (rowData[keyCol] !== undefined) {
            keyValue = rowData[keyCol];
          } else {
            // Try case-insensitive match
            const rowKey = Object.keys(rowData).find(k => k.toLowerCase() === keyColLower);
            keyValue = rowKey ? rowData[rowKey] : '';
          }
          
          const keyValueStr = (keyValue || '').toString();
          const keyColProps = filteredMetaData.find(x => x.fldname.toLowerCase() === keyColLower);
          let displayValue = keyValueStr;
          
          if (keyColProps && (keyColProps.fdatatype === 'd' || keyColProps.cdatatype === 'Date')) {
            displayValue = formatDateString(keyValueStr);
          }
  
          // Key column should render as plain text (not a hyperlink).
          const keyText = _entityCommon.inValid(displayValue) ? "--" : String(displayValue);
          html += `<td>${escapeHtml(keyText)}</td>`;
        } else {
          // metadata says key exists but no data in rows: render empty cell
          html += `<td></td>`;
        }
      } // else: skip rendering key cell entirely (keeps column alignment by not outputting a placeholder)
  
      // Render cells using merged metadata order and only for fields that have data or are explicitly listed
      filteredMetaData.forEach(field => {
        const fieldName = (field.fldname || '').toString().toLowerCase();
        if (fieldName === keyColLower) return;
        if (excludedFields.has(fieldName)) return;
        // If field isn't present in the response payload at all, don't render it (avoids blank columns).
        // For empty datasets, we still render columns from metadata so the user can see the schema.
        if (!isEmptyDataset && !fieldsPresent.has(fieldName)) return;
  
        const hasData = fieldsWithData.has(fieldName);
        const allowShow = hasData || (field.listingfld && (field.listingfld === 'T' || field.listingfld === 't'));
        if (!allowShow) return;
  
        // Try to get field value from rowData
        let fieldValue = '';
        if (rowData[fieldName] !== undefined) {
          fieldValue = rowData[fieldName];
        } else {
          // Try case-insensitive match
          const rowKey = Object.keys(rowData).find(k => k.toLowerCase() === fieldName);
          fieldValue = rowKey ? rowData[rowKey] : '';
        }
        
        let cellContent = '';
  
        if (field.cdatatype === 'Check box' || field.fdatatype === 'Check box') {
          const isChecked = (fieldValue === 'T' || fieldValue === true || fieldValue === 'true');
          cellContent = `<div class="form-check"><input class="form-check-input" type="checkbox" ${isChecked ? 'checked' : ''} disabled></div>`;
        } else if ((fieldName === 'modifiedon' || fieldName === 'createdon' || field.fdatatype === 'd' || field.cdatatype === 'Date') && fieldValue) {
          cellContent = formatDateString(fieldValue);
        } else if (field.cdatatype === 'Currency' || field.fdatatype === 'Currency') {
          cellContent = formatNumberBasedOnMillions(fieldValue);
        } else {
          cellContent = `${fieldValue || ''}`;
        }

        // SmartView hyperlinks: fields may include hyp_structtype/hyp_transid/tbl_hyperlink from ADS metadata.
        try {
          const linkDesc = smartviewBuildHyperlinkDescriptor(field, rowData);
          if (linkDesc) {
            const txt = (cellContent === null || cellContent === undefined) ? '' : String(cellContent);
            // Don't wrap HTML widgets (checkboxes, etc.)
            if (txt && txt.indexOf('<') === -1) {
              cellContent = `<a href="#" class="sv-hyperlink" data-link="${escapeHtml(linkDesc)}">${escapeHtml(txt)}</a>`;
            }
          }
        } catch (e) {}
  
        html += `<td>${cellContent}</td>`;
      });
  
      html += `</tr>`;
    });

    // Empty dataset: show a friendly row instead of leaving stale data on screen.
    if (!listJson || listJson.length === 0) {
      const colCount = tableExists
        ? (tableBodyContainer.find('thead th').length || 1)
        : (1 + (hasRowOptions ? 1 : 0) + (keyFieldHasData ? 1 : 0) + (addedFields.size || 0));
      const safeColspan = Math.max(1, colCount);
      const noDataRow = `<tr class="sv-no-data"><td colspan="${safeColspan}" style="text-align:center;padding:24px;color:#666;">No data available</td></tr>`;
      if (tableExists) html = noDataRow;
      else html += noDataRow;
    }
  
    if (!tableExists) {
      html += '</tbody></table></div>';
      tableBodyContainer.empty().append(html);
      console.log('Table HTML generated');
      initializeDataTable();
    } else {
      // On dynamic tables, DataTables destroy() can throw when the table was rebuilt/detached.
      // We manage the DOM updates ourselves; destroy DT if present, then update tbody.
      try { destroySmartviewDataTable(); } catch (e) {}
      tableBodyContainer.find('tbody').empty().append(html);
      initializeDataTable();
    }
    
    // ADD EVENT DELEGATION FOR ACTION BUTTONS
    setTimeout(() => {
      attachRowOptionsHandlers();
      attachSmartviewHyperlinkHandlers();
      attachSmartviewGroupExpandHandlers();
    }, 100);

    // Ensure infinite-scroll sentinel exists after every render
    try { ensureSmartviewScrollSentinel(); } catch (e) {}
    // Re-attach scroll listener after DOM updates (prevents binding to the wrong scroll element)
    try {
      if (window.smartTableController && typeof window.smartTableController.attachScrollListener === 'function') {
        setTimeout(() => { try { window.smartTableController.attachScrollListener(); } catch (e) {} }, 120);
      }
    } catch (e) {}
    
    console.log('=== createTableViewHTML completed ===');
  }
  
  /* ---------- Attach Row Options Handlers ---------- */
/* ---------- Attach Row Options Handlers ---------- */
function attachRowOptionsHandlers() {
  // Use event delegation instead of direct binding
  $(document).off('click', '.action-arrow-btn').on('click', '.action-arrow-btn', function(e) {
    e.preventDefault();
    e.stopPropagation();
    
    const index = parseInt($(this).data('rowindex') || '0');
    const rowData = _entity.listJson && _entity.listJson[index];
    
    if (!rowData) {
      console.warn('No row data found for index:', index);
      return;
    }
    
    const actions = parseAxRowOptionsField(rowData);
    console.log('Row actions for index', index, ':', actions);
    
    if (actions && actions.length) {
      showAxRowOptionsMenu(this, actions);
    } else {
      console.log('No actions found for this row');
    }
  });
}

function attachSmartviewGroupExpandHandlers() {
  try {
    if (!window.jQuery) return;
    $(document).off('click.smartviewExpand', '.sv-expand-btn').on('click.smartviewExpand', '.sv-expand-btn', function (e) {
      e.preventDefault();
      e.stopPropagation();

      const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
      if (!ctrl) return false;

      const idx = parseInt(this.getAttribute('data-rowindex') || '-1', 10);
      if (idx < 0) return false;
      const rowData = (window._entity && Array.isArray(window._entity.listJson)) ? window._entity.listJson[idx] : null;
      if (!rowData) return false;

      const $btn = $(this);
      const $tr = $btn.closest('tr');

      // Toggle off if already expanded
      const $next = $tr.next('.sv-group-detail-row');
      if ($next && $next.length) {
        $next.remove();
        $btn.text('+');
        return false;
      }

      const meta = (Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length)
        ? ctrl.lastAdsMeta
        : (window._entity && Array.isArray(window._entity.metaData) ? window._entity.metaData : []);

      const groupFields = smartviewNormalizeGroupbyFields(ctrl.groupby_columns);
      const groupFilters = smartviewBuildGroupFiltersForRow(meta, groupFields, rowData);

      const colCount = ($tr.children('td').length || 1);
      const detailRow = `<tr class="sv-group-detail-row"><td colspan="${colCount}"><div class="sv-group-detail">Loading...</div></td></tr>`;
      $tr.after(detailRow);
      $btn.text('-');

      smartviewFetchGroupDetailRows(ctrl, groupFilters, function (err, rows) {
        const $detail = $tr.next('.sv-group-detail-row').find('.sv-group-detail');
        if (!$detail.length) return;
        if (err) {
          $detail.html('<div>Failed to load details</div>');
          return;
        }
        $detail.html(smartviewRenderGroupDetailTable(rows, meta));
      });

      return false;
    });
  } catch (e) {
    // no-op
  }
}

/* ---------- SmartView Hyperlink Handlers ---------- */
function attachSmartviewHyperlinkHandlers() {
  try {
    if (!window.jQuery) return;
    $(document).off('click.smartviewHyp', '.sv-hyperlink').on('click.smartviewHyp', '.sv-hyperlink', function (e) {
      e.preventDefault();
      e.stopPropagation();
      const link = this.getAttribute('data-link') || '';
      if (!link) return false;
      if (typeof openLinkInPopup === 'function') openLinkInPopup(link);
      else window.open(link, '_blank');
      return false;
    });
  } catch (e) {
    // no-op
  }
}
// Add this function and call it in the controller init
function addRowOptionsStyles() {
  if (!document.getElementById('row-options-styles')) {
    const style = document.createElement('style');
    style.id = 'row-options-styles';
    style.textContent = `
      .axrow-menu {
        position: absolute;
        background: #fff;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        box-shadow: 0 4px 20px rgba(0,0,0,0.15);
        z-index: 1000;
        padding: 4px 0;
        min-width: 160px;
      }
      
      .axrow-menu-item {
        display: flex;
        align-items: center;
        gap: 8px;
        width: 100%;
        padding: 8px 12px;
        border: none;
        background: transparent;
        text-align: left;
        cursor: pointer;
        font-size: 13px;
        color: #333;
        transition: background 0.2s;
      }
      
      .axrow-menu-item:hover {
        background: #f5f5f5;
      }
      
      .action-arrow-btn {
        border: none;
        background: none;
        cursor: pointer;
        padding: 2px 4px;
        border-radius: 4px;
        transition: background 0.2s;
        font-size: 18px;
        line-height: 1;
      }
      
      .action-arrow-btn:hover {
        background: #f0f0f0;
      }
      
      .action-cell {
        text-align: center !important;
        padding: 4px !important;
        width: 40px;
      }
    `;
    document.head.appendChild(style);
  }
}

function addHeaderMenuStyles() {
  if (document.getElementById('sv-header-menu-styles')) return;
  const style = document.createElement('style');
  style.id = 'sv-header-menu-styles';
  style.textContent = `
    /* SmartView table typography to match product table */
    #table-body_Container .table > thead th,
    #table-body_Container .table > thead th span {
      font-size: 14px;
      font-weight: 700;
      white-space: nowrap;
    }
    #table-body_Container .table > tbody td {
      font-size: 14px;
      font-weight: 600;
      white-space: nowrap;
    }
    .sv-data-header .sv-header-cell {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 8px;
      min-width: 0;
    }
    .sv-data-header .sv-header-title {
      display: inline-block;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      max-width: 100%;
    }
    .sv-data-header .sv-header-menu-btn {
      border: none;
      background: transparent;
      color: #666;
      cursor: pointer;
      border-radius: 4px;
      width: 20px;
      height: 20px;
      line-height: 20px;
      text-align: center;
      padding: 0;
      opacity: 0;
      pointer-events: none;
      transition: opacity 0.15s ease, background 0.15s ease;
      flex-shrink: 0;
    }
    .sv-data-header:hover .sv-header-menu-btn,
    .sv-data-header .sv-header-menu-btn:focus {
      opacity: 1;
      pointer-events: auto;
    }
    .sv-data-header .sv-header-menu-btn:hover {
      background: #f0f0f0;
      color: #333;
    }
    .sv-header-menu {
      position: absolute;
      background: #fff;
      border: 1px solid #ddd;
      border-radius: 8px;
      min-width: 180px;
      box-shadow: 0 6px 24px rgba(0, 0, 0, 0.16);
      padding: 4px 0;
      z-index: 12000;
    }
    .sv-header-menu-item {
      display: block;
      width: 100%;
      border: none;
      background: transparent;
      text-align: left;
      padding: 8px 12px;
      font-size: 13px;
      cursor: pointer;
      color: #333;
    }
    .sv-header-menu-item:hover {
      background: #f5f5f5;
    }
    .sv-expand-btn {
      border: 1px solid #d0d0d0;
      background: #fff;
      color: #333;
      width: 18px;
      height: 18px;
      line-height: 16px;
      text-align: center;
      border-radius: 3px;
      font-size: 12px;
      cursor: pointer;
      margin-right: 6px;
      padding: 0;
    }
    .sv-expand-btn:hover {
      background: #f5f5f5;
    }
    .sv-group-detail {
      padding: 8px 6px;
      background: #fafafa;
      border: 1px solid #eee;
      border-radius: 6px;
    }
    .sv-group-detail table {
      width: 100%;
      border-collapse: collapse;
    }
    .sv-group-detail th,
    .sv-group-detail td {
      padding: 6px 8px;
      border-bottom: 1px solid #eee;
      font-size: 12px;
    }
    .sv-group-detail thead th {
      background: #f6f6f6;
      font-weight: 600;
    }
  `;
  document.head.appendChild(style);
}

function closeSmartviewHeaderMenu() {
  const m = document.getElementById('svHeaderMenu');
  if (m && m.parentElement) m.parentElement.removeChild(m);
}

function showSmartviewHeaderMenu(anchorBtn, fieldName) {
  if (!anchorBtn || !fieldName) return;
  closeSmartviewHeaderMenu();

  const ctrl = window.smartTableController || null;
  const grouped = !!(ctrl && Array.isArray(ctrl.groupby_columns) && ctrl.groupby_columns.some(f => String(f).toLowerCase() === String(fieldName).toLowerCase()));

  const menu = document.createElement('div');
  menu.id = 'svHeaderMenu';
  menu.className = 'sv-header-menu';
  menu.innerHTML = `
    <button type="button" class="sv-header-menu-item" data-action="sort_asc" data-field="${escapeHtml(fieldName)}">Sort Ascending</button>
    <button type="button" class="sv-header-menu-item" data-action="sort_desc" data-field="${escapeHtml(fieldName)}">Sort Descending</button>
    <button type="button" class="sv-header-menu-item" data-action="group_toggle" data-field="${escapeHtml(fieldName)}">${grouped ? 'Remove Group By' : 'Group By'}</button>
    <button type="button" class="sv-header-menu-item" data-action="group_clear" data-field="${escapeHtml(fieldName)}">Clear Grouping</button>
  `;
  document.body.appendChild(menu);

  const r = anchorBtn.getBoundingClientRect();
  const left = Math.max(8, Math.min(window.innerWidth - menu.offsetWidth - 8, r.left));
  const top = Math.max(8, Math.min(window.innerHeight - menu.offsetHeight - 8, r.bottom + 4));
  menu.style.left = `${left}px`;
  menu.style.top = `${top}px`;

  setTimeout(() => {
    document.addEventListener('click', function _close(ev) {
      if (!menu.contains(ev.target) && ev.target !== anchorBtn) {
        closeSmartviewHeaderMenu();
        document.removeEventListener('click', _close);
      }
    });
  }, 0);
}

function attachSmartviewHeaderMenuHandlers() {
  if (!window.jQuery || window._smartviewHeaderMenuHandlersAttached) return;

  $(document).off('click.smartviewHeaderMenuOpen', '.sv-header-menu-btn').on('click.smartviewHeaderMenuOpen', '.sv-header-menu-btn', function (e) {
    e.preventDefault();
    e.stopPropagation();
    const field = this.getAttribute('data-field') || '';
    if (!field) return false;
    showSmartviewHeaderMenu(this, field);
    return false;
  });

  $(document).off('click.smartviewHeaderMenuAction', '.sv-header-menu-item').on('click.smartviewHeaderMenuAction', '.sv-header-menu-item', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const ctrl = window.smartTableController || null;
    if (!ctrl) {
      closeSmartviewHeaderMenu();
      return false;
    }

    const action = (this.getAttribute('data-action') || '').toLowerCase();
    const field = this.getAttribute('data-field') || '';

    if (action === 'sort_asc') ctrl.applyHeaderSort(field, 'asc');
    else if (action === 'sort_desc') ctrl.applyHeaderSort(field, 'desc');
    else if (action === 'group_toggle') ctrl.toggleGroupByField(field);
    else if (action === 'group_clear') ctrl.clearGroupByColumns();

    closeSmartviewHeaderMenu();
    return false;
  });

  window._smartviewHeaderMenuHandlersAttached = true;
}
/* --------------------------
   Normalizer + render function
   -------------------------- */

function normalizeAndRenderFromDsResponse(parsed, pageNo, pageSize) {
  try {
    // --- unwrap response robustly (works with "d" envelope or plain object/string)
    let parsedObj = parsed;
    try {
      if (typeof parsedObj === 'string') parsedObj = JSON.parse(parsedObj);
      // handle { d: "..." } or { d: { result: ... } }
      if (parsedObj && typeof parsedObj.d === 'string') parsedObj = JSON.parse(parsedObj.d);
      if (parsedObj && parsedObj.d && typeof parsedObj.d === 'object' && !parsedObj.result) parsedObj = parsedObj.d;
    } catch (e) {
      // keep parsedObj as-is if parsing fails
    }

    // --- locate dsBlock and rows (cover common shapes)
    let dsBlock = null;
    if (parsedObj?.result && Array.isArray(parsedObj.result.data) && parsedObj.result.data.length > 0) {
      dsBlock = parsedObj.result.data[0];
    } else if (Array.isArray(parsedObj?.data) && parsedObj.data.length > 0) {
      dsBlock = parsedObj.data[0];
    } else if (parsedObj && (parsedObj.adsname || parsedObj.data || parsedObj.columns)) {
      dsBlock = parsedObj;
    } else if (Array.isArray(parsedObj)) {
      dsBlock = parsedObj[0] || {};
    } else {
      dsBlock = parsedObj?.result || parsedObj || {};
    }

    const rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsedObj?.data) ? parsedObj.data : []);
    const totalRecords = Number(dsBlock?.totalrecords ?? dsBlock?.recordcount ?? parsedObj?.result?.totalrecords ?? rows.length) || rows.length;
    const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
    const activeFilters = (ctrl && Array.isArray(ctrl.filters)) ? ctrl.filters : [];
    const filteredRows = (activeFilters.length > 0) ? applySmartviewFiltersToRows(rows, activeFilters) : rows;
    const effectiveTotal = (activeFilters.length > 0) ? filteredRows.length : totalRecords;

    // --- page title (best-effort)
    const pageTitle =
    (dsBlock && (dsBlock.adsname || dsBlock.adsName)) ||
    (parsedObj?.result?.ADSNames && parsedObj.result.ADSNames[0]) ||
    (parsedObj?.ADSNames && parsedObj.ADSNames?.[0]) ||
    (window.smartTableController && window.smartTableController.adsName) ||
    (window._entity && window._entity.adsName) ||
    '';

  const titleEl = document.getElementById('EntityTitle') || document.querySelector('.page-header-title');
  if (titleEl && pageTitle) {
    titleEl.textContent = pageTitle;
  } else if (pageTitle) {
    document.title = pageTitle;
  }

    // --- ensure _entity exists and set basic state
    window._entity = window._entity || {};
    if (pageTitle) window._entity.adsName = pageTitle;
const pageSz = Number(pageSize ?? window._entity.pageSize ?? 0);
if (pageSz > 0) {
  // only render the first page (pageNo may be 1 on initial load)
  const fromIndex = ((Number(pageNo) || 1) - 1) * pageSz;
  window._entity.listJson = Array.isArray(filteredRows) ? filteredRows.slice(fromIndex, fromIndex + pageSz) : [];
} else {
  // pagesize === 0 means "render all"
  window._entity.listJson = Array.isArray(filteredRows) ? filteredRows.slice() : [];
}    window._entity.pageSize = Number(pageSize || window._entity.pageSize || 10);

    // --- Build metadata:
    // 1) Prefer dsBlock.columns if provided by the ADS
    // 2) Otherwise infer metadata from the keys of the first row (this fixes your issue)
    if (dsBlock && Array.isArray(dsBlock.columns) && dsBlock.columns.length) {
      window._entity.metaData = dsBlock.columns.map(col => ({
        fldname: (col.key || col.name || '').toString(),
        fldcap: col.caption || formatFieldName((col.key || col.name || '').toString()),
        fdatatype: 't',
        cdatatype: inferColumnType(col),
        listingfld: "T"
      })).filter(m => m.fldname);
    } else if (rows && rows.length > 0) {
      // infer from first row keys
      const sample = rows[0];
      const keys = Object.keys(sample || {}).map(k => k.toString());
      const preferredOrder = ['transid', 'recordid', 'processname', 'taskname', 'formcaption', 'keyfieldcaption', 'username', 'email', 'nickname'];

      const meta = [];
      // add preferred keys first (if present)
      preferredOrder.forEach(k => {
        if (keys.includes(k) && !meta.some(m => m.fldname.toLowerCase() === k.toLowerCase())) {
          meta.push({
            fldname: k,
            fldcap: formatFieldName(k),
            fdatatype: 't',
            cdatatype: 'Text',
            listingfld: 'T'
          });
        }
      });

      // add remaining keys
      keys.forEach(k => {
        if (!meta.some(m => m.fldname.toLowerCase() === k.toLowerCase())) {
          meta.push({
            fldname: k,
            fldcap: formatFieldName(k),
            fdatatype: 't',
            cdatatype: 'Text',
            listingfld: 'T'
          });
        }
      });

      window._entity.metaData = meta;
    } else {
      // fallback minimal metadata (keeps previous behaviour)
      window._entity.metaData = window._entity.metaData && window._entity.metaData.length
        ? window._entity.metaData
        : [
            { fldname: "transid", fldcap: "Trans ID", fdatatype: "t", cdatatype: "Text", listingfld: "T" },
            { fldname: "recordid", fldcap: "Record ID", fdatatype: "t", cdatatype: "Text", listingfld: "T" },
            { fldname: "username", fldcap: "Username", fdatatype: "t", cdatatype: "Text", listingfld: "T" }
          ];
    }

    // ensure keyField is set sensibly
    if (!window._entity.keyField) {
      const keyCandidate = window._entity.metaData.find(m => ['recordid','id','username'].includes((m.fldname || '').toLowerCase()));
      window._entity.keyField = keyCandidate ? keyCandidate.fldname : (window._entity.metaData[0] ? window._entity.metaData[0].fldname : 'recordid');
    }

    // update row count manager if present
    if (typeof rowCountManager !== "undefined" && rowCountManager && typeof rowCountManager.setTotal === "function") {
      rowCountManager.setTotal(effectiveTotal);
      rowCountManager.setLoadedRecords(window._entity.listJson.length || 0);
    }

    // render table
    if (typeof createTableViewHTML === "function") {
      createTableViewHTML(window._entity.listJson, pageNo || 1);
    } else {
      console.warn("createTableViewHTML not found - cannot render table");
    }
  } catch (err) {
    console.error("normalizeAndRenderFromDsResponse failed:", err, parsed);
  }
}

function inferColumnType(col) {
  const key = (col.key || '').toLowerCase();
  if (key.includes('date') || key.includes('on')) return 'Date';
  if (key.includes('email')) return 'Email';
  if (key.includes('amount') || key.includes('price') || key.includes('total')) return 'Currency';
  return 'Text';
}

// robust parser for Ax list responses (handles response, response.d string, response.d object)
function safeParseAxResponse(resp) {
  try {
    let parsed = resp;
    if (typeof parsed === 'string') parsed = JSON.parse(parsed);

    // Some responses come as { d: " { \"result\":{...} } " } (string inside d)
    if (parsed && typeof parsed.d === 'string') {
      try { parsed = JSON.parse(parsed.d); } catch (e) { /* keep original if parse fails */ }
    }

    // Some responses are { d: { result: {...} } } (object inside d)
    if (parsed && parsed.d && typeof parsed.d === 'object' && !parsed.result) {
      parsed = parsed.d;
    }

    return parsed || {};
  } catch (err) {
    console.warn('safeParseAxResponse failed', err, resp);
    return {};
  }
}

/* --------------------------
   Export helpers (copy/adapted from Entity.js)
   -------------------------- */

function ensureHiddenTableContainer() {
  if (!document.getElementById('hiddenTableContainer')) {
    const div = document.createElement('div');
    div.id = 'hiddenTableContainer';
    div.style.display = 'none';
    document.body.appendChild(div);
  }
}

function createHiddenTableFromMetadata() {
  ensureHiddenTableContainer();
  const container = document.getElementById('hiddenTableContainer');
  container.innerHTML = ''; // reset

  let tableHtml = `<table id="hiddenTable" class="display nowrap" style="width:100%"><thead><tr>`;
  const fieldsWithDataArr = [];

  // choose fields based on merged metadata and whether there's data in _entity.listJson
  const meta = Array.isArray(_entity.metaData) ? _entity.metaData : [];

  meta.forEach(field => {
    // skip if explicitly hidden (Entity uses field.hide === 'T')
    if (field.hide === 'T') return;

    const key = (field.fldname || field.fldname === 0) ? field.fldname.toLowerCase() : '';
    const hasData = (_entity.listJson || []).some(row => {
      // case-insensitive lookup
      const rowKey = Object.keys(row).find(k => k.toLowerCase() === key);
      const v = rowKey ? row[rowKey] : row[key];
      return v !== null && v !== undefined && String(v).trim() !== '';
    });

    // keep only columns that have data (avoids empty columns in export)
    if (hasData) {
      fieldsWithDataArr.push(field);
      tableHtml += `<th>${field.fldcap || formatFieldName(field.fldname)}</th>`;
    }
  });

  tableHtml += `</tr></thead><tbody>`;

  (_entity.listJson || []).forEach(row => {
    tableHtml += '<tr>';
    fieldsWithDataArr.forEach(field => {
      const key = field.fldname.toLowerCase();
      // case-insensitive read
      const rowKey = Object.keys(row).find(k => k.toLowerCase() === key);
      let cell = rowKey ? row[rowKey] : row[key];
      if (cell === null || cell === undefined) cell = '';
      tableHtml += `<td>${String(cell)}</td>`;
    });
    tableHtml += '</tr>';
  });

  tableHtml += `</tbody></table>`;
  container.innerHTML = tableHtml;
}

function handleExport(action, tableSelector) {
  // action: 'pdf'|'excel'|'word'|'print' (data-target value from menu)
  action = (action || '').toString().toUpperCase();

  try {
    createHiddenTableFromMetadata();

    if ($.fn.dataTable && $.fn.dataTable.isDataTable('#hiddenTable')) {
      $('#hiddenTable').DataTable().destroy();
    }

    const fileNameBase = (_entity && _entity.entityName) ? _entity.entityName.replace(/\s+/g, '_') : 'export';
    const hidden = $('#hiddenTable').DataTable({
      dom: 'Bfrtip',
      paging: false,
      searching: false,
      info: false,
      ordering: false,
      buttons: [
        { extend: 'copy', text: 'Copy', filename: fileNameBase },
        { extend: 'csv',  text: 'CSV',  filename: fileNameBase },
        { extend: 'excel', text: 'Excel', filename: fileNameBase },
        { extend: 'pdf',   text: 'PDF',  filename: fileNameBase },
        { extend: 'print', text: 'Print', filename: fileNameBase }
      ]
    });

    switch (action) {
      case 'PRINT': hidden.button('.buttons-print').trigger(); break;
      case 'PDF':   hidden.button('.buttons-pdf').trigger();   break;
      case 'EXCEL': hidden.button('.buttons-excel').trigger(); break;
      case 'CSV':   hidden.button('.buttons-csv').trigger();   break;
      case 'COPY':  hidden.button('.buttons-copy').trigger();  break;
      default:
        if (hidden.button('.buttons-pdf').length) hidden.button('.buttons-pdf').trigger();
        else if (hidden.button('.buttons-excel').length) hidden.button('.buttons-excel').trigger();
    }
  } catch (err) {
    console.error('handleExport error', err);
    alert('Export failed. Check console for details.');
  }
}

// Debounce helper (same behaviour as Entity.js)
function debounce(func, delay) {
  let timer = null;
  return function (...args) {
    const context = this;
    clearTimeout(timer);
    timer = setTimeout(() => {
      try { func.apply(context, args); } catch (e) { console.error('debounced func error', e); }
    }, delay);
  };
}

function ensureSmartviewScrollSentinel() {
  try {
    const container = document.getElementById('table-body_Container');
    if (!container) return null;
    let s = document.getElementById('smartviewScrollSentinel');
    if (s && container.contains(s)) return s;
    // If sentinel exists elsewhere, remove it
    if (s && s.parentElement) s.parentElement.removeChild(s);
    s = document.createElement('div');
    s.id = 'smartviewScrollSentinel';
    s.style.cssText = 'width:100%;height:1px;';
    container.appendChild(s);
    return s;
  } catch (e) {
    return null;
  }
}

function smartviewBuildGroupFiltersForRow(meta, groupFields, rowData) {
  const filters = [];
  if (!Array.isArray(groupFields) || !rowData) return filters;
  groupFields.forEach(f => {
    const fld = (f || '').toString().trim();
    if (!fld) return;
    const valRaw = getRowValueCaseInsensitive(rowData, fld);
    const val = smartviewCleanIncomingValue(valRaw);
    if (val === '') return;

    const resolved = smartviewResolveFilterField(fld, meta || []);
    const dt = smartviewInferFilterDatatype({ datatype: (resolved.meta && resolved.meta.fdatatype) || '' }, resolved.meta);

    if (dt === 'DROPDOWN') {
      filters.push({ fldname: fld, datatype: 'DROPDOWN', value: [val] });
    } else if (dt === 'NUMERIC') {
      filters.push({ fldname: fld, datatype: 'NUMERIC', from: val, to: val });
    } else if (dt === 'DATE') {
      filters.push({ fldname: fld, datatype: 'DATE', from: val, to: val, condition: 'customOption' });
    } else {
      filters.push({ fldname: fld, datatype: 'TEXT', value: val, condition: 'EQUALS' });
    }
  });
  return filters;
}

function smartviewRenderGroupDetailTable(rows, meta) {
  const data = Array.isArray(rows) ? rows : [];
  if (!data.length) return '<div>No details found</div>';

  const excluded = new Set(['transid', 'ftransid']);
  const fieldsPresent = new Set();
  data.forEach(r => {
    Object.keys(r || {}).forEach(k => {
      const kl = k.toLowerCase();
      if (!excluded.has(kl)) fieldsPresent.add(kl);
    });
  });

  const ordered = [];
  const metaArr = Array.isArray(meta) ? meta : [];
  metaArr.forEach(m => {
    const fn = (m.fldname || '').toString().toLowerCase();
    if (fn && fieldsPresent.has(fn)) ordered.push(m);
  });
  if (!ordered.length) {
    Object.keys(data[0] || {}).forEach(k => {
      const kl = k.toLowerCase();
      if (!excluded.has(kl)) ordered.push({ fldname: k, fldcap: formatFieldName(k), fdatatype: 't', cdatatype: 'Text' });
    });
  }

  let html = '<table class="table table-sm">';
  html += '<thead><tr>';
  ordered.forEach(m => {
    html += `<th>${escapeHtml(m.fldcap || formatFieldName(m.fldname))}</th>`;
  });
  html += '</tr></thead><tbody>';

  data.forEach(r => {
    html += '<tr>';
    ordered.forEach(m => {
      const fname = (m.fldname || '').toString();
      let v = getRowValueCaseInsensitive(r, fname);
      if ((m.fdatatype === 'd' || m.cdatatype === 'Date') && v) v = formatDateString(v);
      html += `<td>${escapeHtml(v == null ? '' : String(v))}</td>`;
    });
    html += '</tr>';
  });

  html += '</tbody></table>';
  return html;
}

function smartviewFetchGroupDetailRows(ctrl, groupFilters, cb) {
  try {
    const baseFilters = stripSmartviewFilterTransId(ctrl.filters || []);
    const filters = baseFilters.concat(groupFilters || []);
    const params = {
      adsNames: [ctrl.adsName],
      refreshCache: false,
      sqlParams: Object.assign({}, ctrl._entitySqlParams || {}),
      props: {
        ADS: false,
        CachePermissions: true,
        getallrecordscount: false,
        pageno: 1,
        pagesize: 0,
        keyfield: "",
        keyvalue: "",
        sorting: ctrl.sorting || [],
        filters: filters
      }
    };
    if (ctrl.axClient_dateformat) params.props.axClient_dateformat = ctrl.axClient_dateformat;

    const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
      : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
      : null;
    if (!caller || typeof caller.GetDataFromAxList !== 'function') {
      cb && cb(new Error('GetDataFromAxList not available'), []);
      return;
    }

    caller.GetDataFromAxList(params, function (response) {
      try {
        const parsed = (typeof safeParseAxResponse === 'function')
          ? safeParseAxResponse(response)
          : ((typeof response === 'string') ? JSON.parse(response) : response);
        const dsBlock = parsed?.result?.data?.[0] || parsed?.data?.[0] || parsed?.result || parsed;
        const rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
        cb && cb(null, rows);
      } catch (e) {
        cb && cb(e, []);
      }
    }, function (err) {
      cb && cb(err, []);
    });
  } catch (e) {
    cb && cb(e, []);
  }
}

/* --------------------------
   SmartView client-side filter helpers
   -------------------------- */

function getRowValueCaseInsensitive(row, fieldName) {
  if (!row || !fieldName) return '';
  const direct = row[fieldName];
  if (direct !== undefined) return direct;
  const key = Object.keys(row).find(k => k.toLowerCase() === String(fieldName).toLowerCase());
  return key ? row[key] : '';
}

function parseDateLoose(v) {
  if (v == null || v === '') return null;
  if (v instanceof Date && !isNaN(v)) return v;
  const s = String(v).trim();
  // dd/mm/yyyy
  const m = s.match(/^(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})$/);
  if (m) {
    const d = new Date(Number(m[3]), Number(m[2]) - 1, Number(m[1]));
    return isNaN(d) ? null : d;
  }
  const d = new Date(s);
  return isNaN(d) ? null : d;
}

function normalizeFilterCondition(raw) {
  const c = (raw || '').toString().trim().toUpperCase().replace(/\s+/g, ' ');
  if (!c) return '';
  if (c.includes('START')) return 'STARTS_WITH';
  if (c.includes('END')) return 'ENDS_WITH';
  if (c.includes('CONTAINS') || c.includes('LIKE')) return 'CONTAINS';
  if (c.includes('NOT') && c.includes('EQUAL')) return 'NOT_EQUAL';
  if (c.includes('NOT')) return 'NOT';
  if (c.includes('GREATER') || c.includes('>')) return 'GT';
  if (c.includes('LESS') || c.includes('<')) return 'LT';
  if (c.includes('BETWEEN')) return 'BETWEEN';
  if (c.includes('IN')) return 'IN';
  if (c.includes('EQUAL') || c === '=' || c === '==') return 'EQUAL';
  return c;
}

function stripSmartviewFilterTransId(filters) {
  if (!Array.isArray(filters)) return [];
  return filters.map(f => {
    if (!f || typeof f !== 'object') return f;
    const o = Object.assign({}, f);

    // Some platforms include transid in filter objects; SmartView backend doesn't need it.
    delete o.ftransid;
    delete o.fTransId;
    delete o.transid;
    delete o.transId;

    // Normalize fldname (avoid "filter_" prefix leaks).
    if (o.fldname !== undefined && o.fldname !== null) {
      o.fldname = String(o.fldname).replace(/^filter_/, '').trim();
    }

    return o;
  });
}

/* --------------------------
   SmartView ADS Metadata Cache (localStorage)
   -------------------------- */

function smartviewNormalizeAdsName(adsName) {
  return (adsName || '').toString().trim().toLowerCase();
}

function smartviewAdsMetaStorageKey(adsName) {
  const n = smartviewNormalizeAdsName(adsName);
  return n ? `smartview_adsmeta::${n}` : `smartview_adsmeta::`;
}

function saveSmartviewAdsMetaToStorage(adsName, meta) {
  try {
    if (!adsName || !Array.isArray(meta)) return;
    if (typeof localStorage === 'undefined') return;
    const key = smartviewAdsMetaStorageKey(adsName);
    const payload = { ads: String(adsName), ts: Date.now(), meta: meta };
    localStorage.setItem(key, JSON.stringify(payload));
  } catch (e) {
    // ignore (storage may be disabled/quota exceeded)
  }
}

function loadSmartviewAdsMetaFromStorage(adsName) {
  try {
    if (!adsName) return null;
    if (typeof localStorage === 'undefined') return null;
    const key = smartviewAdsMetaStorageKey(adsName);
    const raw = localStorage.getItem(key);
    if (!raw) return null;
    const parsed = JSON.parse(raw);
    // Allow either { meta:[...] } wrapper or direct array for backwards compatibility
    if (Array.isArray(parsed)) return parsed;
    if (parsed && Array.isArray(parsed.meta)) return parsed.meta;
    return null;
  } catch (e) {
    return null;
  }
}

/* --------------------------
   SmartView Table Hyperlinks (from ADS metadata)
   -------------------------- */

function smartviewParseTblHyperlink(raw) {
  if (!raw) return [];
  const s = String(raw).trim();
  if (!s) return [];
  // Pairs are commonly like: "party_name|suppname^recordid|recordid"
  const parts = s.split(/[\^,;]+/).map(p => p.trim()).filter(Boolean);
  const pairs = [];
  parts.forEach(p => {
    const seg = p.split('|');
    if (seg.length < 2) return;
    const k = (seg[0] || '').toString().trim();
    const v = (seg[1] || '').toString().trim();
    if (!k || !v) return;
    pairs.push([k, v]);
  });
  return pairs;
}

function smartviewGetRowValueForHyperlink(row, fieldKey) {
  if (!row || !fieldKey) return '';
  let v = getRowValueCaseInsensitive(row, fieldKey);
  if ((v === undefined || v === null || String(v).trim() === '') && String(fieldKey).includes('.')) {
    // common case: metadata says "a.itemdesc" but row key is "itemdesc" (or vice-versa)
    const tail = String(fieldKey).split('.').pop();
    v = getRowValueCaseInsensitive(row, tail);
  }
  return v;
}

function smartviewBuildHyperlinkDescriptor(fieldMeta, rowData) {
  if (!fieldMeta || !rowData) return '';

  const hypStructType = (fieldMeta.hyp_structtype ?? fieldMeta.hypStructType ?? fieldMeta.hyp_struct_type ?? '') || '';
  const hypTransId = (fieldMeta.hyp_transid ?? fieldMeta.hypTransId ?? fieldMeta.hyp_transId ?? '') || '';
  const tblHyperlink = (fieldMeta.tbl_hyperlink ?? fieldMeta.tblHyperlink ?? fieldMeta.tbl_hyperLink ?? '') || '';

  const st = String(hypStructType || '').trim().toLowerCase();
  const transRaw = String(hypTransId || '').trim();
  const tbl = String(tblHyperlink || '').trim();
  if (!st || !transRaw) return '';

  let prefix = '';
  if (st.includes('tstruct') || st.includes('t struct')) prefix = 't';
  else if (st.includes('iview') || st.includes('i view')) prefix = 'i';
  else if (st.includes('html') || st.includes('page')) prefix = 'h';
  else if (st.includes('entityform') || st.includes('entity form')) prefix = 'd';
  else if (st.includes('entity')) prefix = 'l';
  if (!prefix) return '';

  // hyp_transid often already includes the type prefix (e.g. "tgsupp").
  // Our descriptor also uses a prefix letter for type, so strip it to avoid "ttgsupp".
  let trans = transRaw;
  while (trans && trans.length > 1 && trans[0].toLowerCase() === prefix.toLowerCase()) {
    trans = trans.slice(1);
  }
  if (!trans) return '';

  const pairs = smartviewParseTblHyperlink(tbl);
  const params = [];
  pairs.forEach(([paramName, fieldKey]) => {
    const rawVal = smartviewGetRowValueForHyperlink(rowData, fieldKey);
    if (rawVal === undefined || rawVal === null) return;
    const valStr = String(rawVal).trim();
    if (!valStr) return;

    // Avoid breaking descriptor parsing; openLinkInPopup uses ^ and () as syntax.
    const safeVal = valStr.replace(/[\^~()]/g, ' ');
    params.push(`${paramName}=${safeVal}`);
  });

  const paramStr = params.join('^');
  return paramStr ? `${prefix}${trans}(${paramStr})` : `${prefix}${trans}`;
}

function applySmartviewFiltersToRows(rows, filters) {
  if (!Array.isArray(rows) || !rows.length) return rows || [];
  if (!Array.isArray(filters) || !filters.length) return rows;

  return rows.filter(row => {
    return filters.every(f => {
      if (!f) return true;
      const field = (f.fldname || f.name || f.field || f.col || f.column || '').toString();
      if (!field) return true;

      const rawVal = getRowValueCaseInsensitive(row, field);
      const type = (f.datatype || f.fdatatype || f.type || '').toString().toUpperCase();
      const cond = normalizeFilterCondition(f.condition || f.cond || f.operator || f.opr || f.opt || f.option || '');

      const value = (f.value !== undefined) ? f.value : (f.val !== undefined ? f.val : '');
      const from = (f.from !== undefined) ? f.from : (f.min !== undefined ? f.min : '');
      const to = (f.to !== undefined) ? f.to : (f.max !== undefined ? f.max : '');

      // Normalize arrays for IN comparisons
      const asArray = Array.isArray(value)
        ? value
        : (typeof value === 'string' && value.includes(',')) ? value.split(',') : null;

      if (type === 'DATE' || type === 'D') {
        const rv = parseDateLoose(rawVal);
        const fFrom = parseDateLoose(from);
        const fTo = parseDateLoose(to);
        const fVal = parseDateLoose(value);
        if (cond === 'BETWEEN' || (fFrom && fTo)) {
          if (!rv) return false;
          return (!fFrom || rv >= fFrom) && (!fTo || rv <= fTo);
        }
        if (cond === 'GT') return rv && fVal && rv > fVal;
        if (cond === 'LT') return rv && fVal && rv < fVal;
        if (cond === 'NOT_EQUAL') return rv && fVal ? rv.getTime() !== fVal.getTime() : true;
        if (cond === 'EQUAL' || cond === 'ON') return rv && fVal ? rv.getTime() === fVal.getTime() : true;
        return true;
      }

      if (type === 'NUMERIC' || type === 'NUMBER' || type === 'N') {
        const rv = parseFloat(rawVal);
        const fFrom = parseFloat(from);
        const fTo = parseFloat(to);
        const fVal = parseFloat(value);
        if (cond === 'BETWEEN' || (!isNaN(fFrom) && !isNaN(fTo))) {
          if (isNaN(rv)) return false;
          return (isNaN(fFrom) || rv >= fFrom) && (isNaN(fTo) || rv <= fTo);
        }
        if (cond === 'GT') return !isNaN(rv) && !isNaN(fVal) && rv > fVal;
        if (cond === 'LT') return !isNaN(rv) && !isNaN(fVal) && rv < fVal;
        if (cond === 'NOT_EQUAL') return rv !== fVal;
        if (cond === 'EQUAL') return rv === fVal;
        return true;
      }

      // Text / default comparison
      const rvStr = (rawVal == null ? '' : String(rawVal)).toLowerCase();
      if (asArray && asArray.length) {
        return asArray.map(v => String(v).toLowerCase()).includes(rvStr);
      }
      const vStr = (value == null ? '' : String(value)).toLowerCase();
      if (!cond || cond === 'CONTAINS') return rvStr.includes(vStr);
      if (cond === 'STARTS_WITH') return rvStr.startsWith(vStr);
      if (cond === 'ENDS_WITH') return rvStr.endsWith(vStr);
      if (cond === 'NOT_EQUAL') return rvStr !== vStr;
      if (cond === 'EQUAL') return rvStr === vStr;
      if (cond === 'IN') return vStr.split(',').map(s => s.trim()).includes(rvStr);
      return rvStr.includes(vStr);
    });
  });
}
const searchInput = document.getElementById("searchBox");
const liveSearchDebounced = debounce(liveSearch, 500);

function handleSearchInput() {
    if (searchInput.value === "") {
        liveSearch();
    } else {
        liveSearchDebounced();
    }
}

if (searchInput) {
  searchInput.addEventListener("keyup", handleSearchInput);
  searchInput.addEventListener("input", handleSearchInput);
}

document.getElementById("searchBox")?.addEventListener("keypress", function (event) {
    if (event.key === "Enter") {
        event.preventDefault();
    }
});
/**
 * liveSearch - hides rows (or cards) that don't match the search term.
 * Uses: #searchBox input, looks for rows under #table-body_Container tbody tr
 * and card items under #body_Container .Project_items (if present).
 */
function liveSearch() {
  try {
    const searchInput = document.getElementById('searchBox');
    if (!searchInput) return;

    const term = (searchInput.value || '').toString().trim().toLowerCase();
    const tableRows = document.querySelectorAll('#table-body_Container tbody tr');
    const cards = document.querySelectorAll('#body_Container .Project_items');

    if (tableRows && tableRows.length) {
      // table view filtering
      tableRows.forEach(row => {
        const txt = (row.innerText || '').toString().toLowerCase();
        if (term === '' || txt.includes(term)) {
          row.classList.remove('d-none');
        } else {
          row.classList.add('d-none');
        }
      });

      if (term !== '' && document.querySelectorAll('#table-body_Container tbody tr:not(.d-none)').length === 0) {
        console.warn('No matching data found for:', term);
      }
    } else if (cards && cards.length) {
      // card view filtering
      cards.forEach(card => {
        const txt = (card.innerText || '').toString().toLowerCase();
        if (term === '' || txt.includes(term)) card.classList.remove('d-none');
        else card.classList.add('d-none');
      });

      if (term !== '' && document.querySelectorAll('#body_Container .Project_items:not(.d-none)').length === 0) {
        console.warn('No matching data found for:', term);
      }
    }

    if (rowCountManager && typeof rowCountManager.refresh === 'function') {
      rowCountManager.refresh();
    }
  } catch (err) {
    console.error('liveSearch error', err);
  }
}

// --- helper: read query params ---
function getQueryParam(name) {
  try {
    const params = new URLSearchParams(window.location.search);
    const keys = [name, name.toLowerCase(), name.toUpperCase()];
    for (const k of keys) {
      if (params.has(k)) {
        const v = params.get(k);
        if (v !== null && String(v).trim() !== '') return v.trim();
      }
    }
    return null;
  } catch (e) {
    try {
      const q = window.location.search.replace(/^\?/, '');
      const pairs = q.split('&');
      for (const pair of pairs) {
        const [k, val] = pair.split('=');
        if (!k) continue;
        if (k.toLowerCase() === name.toLowerCase()) return decodeURIComponent((val || '').replace(/\+/g, ' '));
      }
    } catch (ex) {}
    return null;
  }
}

/* --------------------------
   SmartView Initial Filters From Query Param (?filter=base64json)
   -------------------------- */

function smartviewDecodeBase64JsonParam(raw) {
  if (!raw) return null;
  try {
    let s = String(raw);
    // Some callers may URL-encode the base64 string.
    try { s = decodeURIComponent(s); } catch (e) {}
    // In query strings, '+' can turn into space.
    s = s.replace(/ /g, '+').trim();
    // Support URL-safe base64
    s = s.replace(/-/g, '+').replace(/_/g, '/');
    // Pad to multiple of 4 for atob()
    while (s.length % 4 !== 0) s += '=';

    const jsonStr = atob(s);
    return JSON.parse(jsonStr);
  } catch (e) {
    console.warn('smartviewDecodeBase64JsonParam failed', e);
    return null;
  }
}

function smartviewGetInitialFilterPayloadFromQuery() {
  // Expected: ?filter=<base64(JSON.stringify({ filters: [...] }))>
  const q = getQueryParam('filter') || getQueryParam('filters') || null;
  if (!q) return null;
  const decoded = smartviewDecodeBase64JsonParam(q);
  if (!decoded || typeof decoded !== 'object') return null;
  const filters = Array.isArray(decoded.filters) ? decoded.filters : (Array.isArray(decoded.filter) ? decoded.filter : []);
  const ads = decoded.ads || decoded.adsName || decoded.adsname || null;
  return { ads: ads, filters: filters };
}

function smartviewCleanIncomingValue(v) {
  if (v === null || v === undefined) return '';
  let s = String(v);
  try { s = s.trim(); } catch (e) {}

  // If caller passed a JSON string literal (e.g. "\"FOXCON\""), parse it.
  if (s.length >= 2 && ((s.startsWith('"') && s.endsWith('"')) || (s.startsWith("'") && s.endsWith("'")))) {
    try { s = JSON.parse(s); } catch (e) { s = s.slice(1, -1); }
  }

  // Remove stray leading/trailing quotes and backslashes
  s = s.replace(/^\\+/, '').replace(/\\+$/, '');
  s = s.replace(/^"+/, '').replace(/"+$/, '');
  s = s.replace(/^'+/, '').replace(/'+$/, '');
  return String(s).trim();
}

function smartviewResolveFilterField(rawField, metaData) {
  const rf = (rawField === null || rawField === undefined) ? '' : String(rawField).trim();
  if (!rf) return { fldname: '', meta: null };
  const rfl = rf.toLowerCase();
  const arr = Array.isArray(metaData) ? metaData : [];

  let m = arr.find(x => ((x.fldname || '').toString().trim().toLowerCase() === rfl)) || null;
  if (!m) m = arr.find(x => ((x._svOriginalFldname || '').toString().trim().toLowerCase() === rfl)) || null;
  if (!m) m = arr.find(x => ((x.fldcap || x.fldcaption || x.caption || '').toString().trim().toLowerCase() === rfl)) || null;

  if (m) return { fldname: (m.fldname || '').toString(), meta: m };

  // Guess key from caption-like labels (e.g. "PR Number" -> "prnum")
  const guess = smartviewGuessDataKeyFromCaption(rf);
  if (guess) {
    m = arr.find(x => ((x.fldname || '').toString().trim().toLowerCase() === guess.toLowerCase())) || null;
    if (m) return { fldname: (m.fldname || '').toString(), meta: m };
    // If we have metadata and still can't resolve, do NOT invent a column name.
    // Sending unknown fldname to backend breaks the ADS SQL (e.g. "column does not exist").
    if (arr.length) return { fldname: '', meta: null };
    return { fldname: guess, meta: null };
  }

  // Fallback:
  // - if metadata exists, drop unknown fields
  // - if metadata is missing, best-effort normalize
  if (arr.length) return { fldname: '', meta: null };
  return { fldname: rf.replace(/\s+/g, '').toLowerCase(), meta: null };
}

function smartviewIsNumericMetaField(meta) {
  if (!meta) return false;
  const ft = (meta.fdatatype || '').toString().trim().toLowerCase();
  const ct = (meta.cdatatype || '').toString().trim().toLowerCase();
  if (ft === 'n') return true;
  if (ct === 'numeric' || ct === 'number' || ct === 'currency' || ct === 'decimal' || ct === 'float' || ct === 'double' || ct === 'int' || ct === 'integer') return true;
  return false;
}

function smartviewIsAggregationExpr(val) {
  const s = (val === null || val === undefined) ? '' : String(val).trim();
  if (!s) return false;
  return /^(sum|count|avg|min|max)\s*\(/i.test(s);
}

function smartviewSelectExprToFieldName(expr) {
  const s = (expr === null || expr === undefined) ? '' : String(expr).trim();
  if (!s) return '';
  // If expression includes alias (e.g., "sum(ordqty) sum_ordqty"), use last token as field name.
  const parts = s.split(/\s+/).filter(Boolean);
  if (parts.length >= 2) return parts[parts.length - 1];
  return s;
}

function smartviewNormalizeGroupbyFields(list) {
  const arr = Array.isArray(list) ? list : [];
  return Array.from(new Set(arr
    .map(x => String(x || '').trim())
    .filter(Boolean)
    .filter(x => !smartviewIsAggregationExpr(x))
  ));
}

function smartviewBuildAggregationsForGroupby(metaData, groupbyCols) {
  const meta = Array.isArray(metaData) ? metaData : [];
  const groupSet = new Set(smartviewNormalizeGroupbyFields(groupbyCols).map(x => x.toLowerCase()));
  const aggs = {};

  meta.forEach(m => {
    const fld = (m && m.fldname !== undefined && m.fldname !== null) ? String(m.fldname).trim() : '';
    if (!fld) return;
    if (groupSet.has(fld.toLowerCase())) return;
    if (!smartviewIsNumericMetaField(m)) return;
    const alias = (`sum_${fld}`).replace(/[^a-zA-Z0-9_]/g, '_');
    if (!aggs[alias]) aggs[alias] = `sum(${fld})`;
  });

  return aggs;
}

function smartviewBuildGroupbyWithSums(metaData, groupbyCols) {
  const base = smartviewNormalizeGroupbyFields(groupbyCols);

  const meta = Array.isArray(metaData) ? metaData : [];
  const groupSet = new Set(base.map(x => x.toLowerCase()));
  const sumExprs = [];

  meta.forEach(m => {
    const fld = (m && m.fldname !== undefined && m.fldname !== null) ? String(m.fldname).trim() : '';
    if (!fld) return;
    if (groupSet.has(fld.toLowerCase())) return;
    if (!smartviewIsNumericMetaField(m)) return;
    const alias = (`sum_${fld}`).replace(/[^a-zA-Z0-9_]/g, '_');
    sumExprs.push(`sum(${fld}) ${alias}`);
  });

  return {
    groupby_columns: base.slice(),
    select_columns: base.concat(sumExprs)
  };
}

function smartviewInferFilterDatatype(rawItem, meta) {
  const item = rawItem || {};
  const tRaw = (item.datatype || item.dataType || item.type || item.fdatatype || item.cdatatype || '').toString().trim();
  const t = tRaw.toUpperCase();

  // Prefer metadata if it explicitly says dropdown
  const metaCd = (meta && meta.cdatatype) ? String(meta.cdatatype).trim().toLowerCase() : '';
  if (metaCd === 'dropdown' || metaCd === 'drop down' || metaCd === 'drop_down') return 'DROPDOWN';

  if (t === 'DROPDOWN' || t === 'DROP DOWN' || t === 'SELECT') return 'DROPDOWN';
  if (t === 'DATE' || t === 'D') return 'DATE';
  if (t === 'NUMERIC' || t === 'NUMBER' || t === 'N') return 'NUMERIC';
  if (t === 'TEXT' || t === 'C' || t === 'T' || t === 'STRING') return 'TEXT';

  // Short datatype codes common in ADS metadata
  if (tRaw.toLowerCase() === 'd') return 'DATE';
  if (tRaw.toLowerCase() === 'n') return 'NUMERIC';
  if (tRaw.toLowerCase() === 'c' || tRaw.toLowerCase() === 't') return 'TEXT';

  // Fallback to metadata fdatatype
  const ft = (meta && meta.fdatatype) ? String(meta.fdatatype).trim().toLowerCase() : '';
  if (ft === 'd') return 'DATE';
  if (ft === 'n') return 'NUMERIC';
  if (ft === 'c' || ft === 't') return 'TEXT';

  return 'TEXT';
}

function smartviewOperatorToTextCondition(op) {
  const o = (op === null || op === undefined) ? '' : String(op).trim().toUpperCase();
  if (!o) return 'CONTAINS';
  if (o === '=' || o === '==' || o === 'EQUALS') return 'EQUALS';
  if (o === 'STARTSWITH' || o === 'STARTS WITH' || o === '^') return 'STARTSWITH';
  if (o === 'ENDSWITH' || o === 'ENDS WITH' || o === '$') return 'ENDSWITH';
  if (o === 'CONTAINS' || o === 'LIKE' || o === '*' || o === 'INCLUDES') return 'CONTAINS';
  return 'CONTAINS';
}

function smartviewMapExternalFiltersToEntityFilters(rawFilters, metaData) {
  if (!Array.isArray(rawFilters) || rawFilters.length === 0) return [];
  const arr = Array.isArray(metaData) ? metaData : [];

  return rawFilters.map(item => {
    if (!item) return null;
    const rawField = item.fldname || item.field || item.name || item.column || item.col || item.fld || '';
    const rawOp = item.condition || item.operator || item.op || item.cond || item.opt || item.option || '';
    const rawVal = (item.value !== undefined) ? item.value : (item.val !== undefined ? item.val : '');

    const resolved = smartviewResolveFilterField(rawField, arr);
    const fldname = (resolved.fldname || '').toString().trim();
    if (!fldname) return null;

    const dt = smartviewInferFilterDatatype(item, resolved.meta);

    if (dt === 'DROPDOWN') {
      let values = [];
      if (Array.isArray(rawVal)) values = rawVal;
      else if (typeof rawVal === 'string') values = rawVal.split(',').map(x => smartviewCleanIncomingValue(x)).filter(Boolean);
      else if (rawVal !== null && rawVal !== undefined && String(rawVal).trim() !== '') values = [smartviewCleanIncomingValue(rawVal)];
      if (!values.length) return null;
      return { fldname: fldname, datatype: 'DROPDOWN', value: values };
    }

    if (dt === 'DATE') {
      const fromRaw = (item.from !== undefined) ? item.from : '';
      const toRaw = (item.to !== undefined) ? item.to : '';
      const from = smartviewCleanIncomingValue(fromRaw || rawVal);
      const to = smartviewCleanIncomingValue(toRaw || rawVal);
      if (!from && !to) return null;
      return { fldname: fldname, datatype: 'DATE', from: from || '', to: to || '', condition: item.condition || 'customOption' };
    }

    if (dt === 'NUMERIC') {
      const op = String(rawOp || '').trim();
      const v = smartviewCleanIncomingValue(rawVal);
      if ((item.from !== undefined) || (item.to !== undefined)) {
        const from = smartviewCleanIncomingValue(item.from) || '0';
        const to = smartviewCleanIncomingValue(item.to) || '999999999';
        return { fldname: fldname, datatype: 'NUMERIC', from: from, to: to };
      }
      if (!v) return null;
      if (op === '<' || op === '<=') return { fldname: fldname, datatype: 'NUMERIC', from: '0', to: v };
      if (op === '>' || op === '>=') return { fldname: fldname, datatype: 'NUMERIC', from: v, to: '999999999' };
      if (op === '=' || op === '==') return { fldname: fldname, datatype: 'NUMERIC', from: v, to: v };
      // default: treat as "from"
      return { fldname: fldname, datatype: 'NUMERIC', from: v, to: '999999999' };
    }

    // TEXT
    const value = smartviewCleanIncomingValue(rawVal);
    if (!value) return null;
    const condition = smartviewOperatorToTextCondition(rawOp);
    return { fldname: fldname, datatype: 'TEXT', value: value, condition: condition };
  }).filter(Boolean);
}

function ensureSmartviewFilterPillsContainer() {
  try {
    const existing = document.querySelector('.filterPills');
    if (existing) {
      // Product pill code toggles display via inline styles; don't keep a Bootstrap d-none around.
      try { existing.classList.remove('d-none'); } catch (e) {}
      return;
    }
    const parent = document.querySelector('.page-header') || document.querySelector('.toolbar') || document.body;
    const wrapper = document.createElement('div');
    wrapper.className = 'filterPills flex-row py-2 px-2 gap-3';
    wrapper.style.display = 'none';
    parent.insertBefore(wrapper, parent.firstChild);
  } catch (e) {}
}

function ensureSmartviewEntityFilterPatched() {
  try {
    if (typeof EntityFilter === 'undefined') return false;
    window._entityFilter = window._entityFilter || new EntityFilter();

    if (window._entityFilter._smartviewPatchedForPills) return true;

    // Minimal patch: ensure pills apply filters to SmartView controller (without requiring openFilters() first).
    window._entityFilter.applyFilters = function () {
      try {
        const filters = stripSmartviewFilterTransId(this.activeFilterArray || []);
        const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
        if (!ctrl) return;
        ctrl.filters = filters;
        ctrl.forceClientFiltering = false;
        ctrl._filteredCache = null;
        window._smartviewFullData = null;
        if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
        if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
      } catch (e) {
        console.error('smartview entityFilter.applyFilters patch error', e);
      }
    };

    window._entityFilter._smartviewPatchedForPills = true;
    return true;
  } catch (e) {
    return false;
  }
}

function smartviewCreateInitialFilterPill(mappedFilters, metaData, adsName) {
  try {
    if (!Array.isArray(mappedFilters) || mappedFilters.length === 0) return;
    if (typeof EntityFilter === 'undefined') return;

    ensureSmartviewFilterPillsContainer();
    try { if (!document.getElementById('filterModal')) createSmartviewFilterModal && createSmartviewFilterModal(); } catch (e) {}
    try { ensureSmartviewFilterCompatDom && ensureSmartviewFilterCompatDom(); } catch (e) {}

    ensureSmartviewEntityFilterPatched();

    window._entityFilter = window._entityFilter || new EntityFilter();
    window._entityFilter.metaData = Array.isArray(metaData) ? metaData : (window._entity && window._entity.metaData) || [];
    window._entityFilter.pageName = 'SmartView';
    window._entityFilter.entityTransId = adsName || window.smartTableController?.adsName || (window._entity && window._entity.entityTransId) || '';

    const id = (window._entityFilter.getCurrentTimestamp && window._entityFilter.getCurrentTimestamp()) || ('Filter-' + Date.now());
    let caption = '';
    try {
      caption = (window._entityFilter.constructFilterCaption && window._entityFilter.constructFilterCaption(mappedFilters)) || '';
    } catch (e) {}
    if (!caption) caption = 'Filter';

    window._entityFilter.filterObj = window._entityFilter.filterObj || {};
    // Replace existing pills for initial payload (keeps UX predictable)
    window._entityFilter.filterObj = {};
    window._entityFilter.activeFilterArray = mappedFilters;
    window._entityFilter.activeFilterId = id;
    window._entityFilter.activeFilterName = caption;
    window._entityFilter.filterObj[id] = { caption: caption, filter: mappedFilters, save: false };

    if (typeof window._entityFilter.createFilterPills === 'function') window._entityFilter.createFilterPills();
  } catch (e) {
    console.warn('smartviewCreateInitialFilterPill failed', e);
  }
}
function startSmartTableFromAdsName(adsName) {
  if (!adsName) {
    console.warn('startSmartTableFromAdsName: no adsName supplied');
    return false;
  }

  console.log('Starting SmartViewTableController with ADS from query param:', adsName);

  // Set header right away (so user sees it even while data is loading)
  window._entity = window._entity || {};
  window._entity.adsName = adsName;
  const titleEl = document.getElementById('EntityTitle') || document.querySelector('.page-header-title');
  if (titleEl) titleEl.textContent = adsName;
  document.title = adsName;

  if (window.smartTableController) {
    const ctrl = window.smartTableController;
    const prevAds = (ctrl.adsName || '').toString();
    ctrl.adsName = adsName;

    if (!prevAds || prevAds.toLowerCase() !== adsName.toLowerCase()) {
      ctrl.lastAdsMeta = null;
      ctrl._adsMetaFor = null;
    }

    ctrl.resetPaging();

    // Prefetch ADS metadata (and persist to localStorage) so Filters/Hyperlinks have schema.
    try { if (typeof ctrl.ensureAdsMetadata === 'function') ctrl.ensureAdsMetadata(); } catch (e) {}

    ctrl.loadNextPage();
  } else {
    window.smartTableController = new SmartViewTableController({
      adsName: adsName,
      pageSize: 100,
      currentPage: 1,
      sorting: window.smartTableController?.sorting || []
    });
  }

  return true;
}

function bindSearchToggleFocus() {
  const searchInput =
    document.getElementById('searchBox');

  const searchToggleBtn =
    document.getElementById('searchBoxButton') ||
    document.getElementById('searchBtn');

  if (!searchInput || !searchToggleBtn) return;

  // Remove broken inline onclick if present
  try { searchToggleBtn.onclick = null; } catch (e) {}

  searchToggleBtn.addEventListener('click', (e) => {
    e.preventDefault();

    // Toggle visibility
    searchInput.classList.toggle('show');

    // Focus after render so typing works immediately
    setTimeout(() => {
      searchInput.focus();

      // Move caret to end
      const len = searchInput.value.length;
      if (searchInput.setSelectionRange) {
        searchInput.setSelectionRange(len, len);
      }
    }, 40);
  });

  // ESC → close search
  searchInput.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
      searchInput.value = '';
      searchInput.classList.remove('show');
      searchInput.blur();

      if (typeof liveSearch === 'function') {
        liveSearch(); // refresh results
      }
    }
  });
}

/**
 * Open the Filter modal. Ensure we fetch ads-metadata first (so the filter UI shows correct fields).
 * Called by toolbar button: onclick="openFilters(); return false;"
 */
function openFilters() {
  try {
    // Must have a controller
    if (!window.smartTableController) {
      console.warn('openFilters: controller not initialized. Creating default controller.');
      window.smartTableController = new SmartViewTableController({ adsName: window._entity?.adsName || 'ds_smartlist_users' });
    }

    // If we already have metadata cached for the currently selected ADS, show modal directly
    const controller = window.smartTableController;

    // Warm ADS metadata from localStorage (avoids extra network call on every filter open)
    try {
      const adsKey = controller.adsName || window._entity?.adsName || '';
      const metaFor = (controller._adsMetaFor || '').toString();
      if (adsKey && (!controller.lastAdsMeta || !metaFor || metaFor.toLowerCase() !== String(adsKey).toLowerCase())) {
        const cached = loadSmartviewAdsMetaFromStorage(adsKey);
        if (cached && Array.isArray(cached) && cached.length) {
          controller.lastAdsMeta = cached;
          controller._adsMetaFor = adsKey;
          window._entity = window._entity || {};
          window._entity.metaData = cached;
        }
      }
    } catch (e) {}

    const showModalWithFilter = function() {
      // If you already injected a modal DOM earlier (my prior message gave createSmartviewFilterModal),
      // make sure #dvModalFilter exists. If not present, create a minimal modal container now:
      if (!document.getElementById('filterModal')) {
        // simple fallback modal DOM — minimal markup that EntityFilter expects
        createSmartviewFilterModal && createSmartviewFilterModal();
      }

      // Ensure filter modal has the IDs Entity-Filter.js expects (edit pill, etc.)
      try { ensureSmartviewFilterCompatDom && ensureSmartviewFilterCompatDom(); } catch (e) {}

      // Ensure EntityFilter class is available (Entity-Filter.js)
      if (typeof EntityFilter === 'undefined') {
        console.error('EntityFilter not found. Make sure Entity-Filter.js is loaded on the page.');
        return;
      }

      // reuse existing instance or create new
      window._entityFilter = window._entityFilter || new EntityFilter();
      // supply the metadata fetched for this ADS
      // Prefer ADS metadata (has flags like normalized/filters) over inferred table metadata.
      window._entityFilter.metaData = controller.lastAdsMeta || window._entity.metaData || [];
      window._entityFilter.pageName = 'SmartView';
      window._entityFilter.entityTransId = controller.adsName || (window._entity && window._entity.entityTransId) || '';

      // Patch EntityFilter.initializeDropdowns so SmartView dropdown filters load values from ADS:
      //   ds_smartlist_filters(sqlParams: { psrctxt: "fld~T~table~fld" })
      // instead of product Analytics.aspx/GetEntityDropDownDataWS.
      if (window._entityFilter && typeof window._entityFilter.initializeDropdowns === 'function' && !window._entityFilter._smartviewDropdownAdsPatched) {
        const originalInitDropdowns = window._entityFilter.initializeDropdowns.bind(window._entityFilter);

        window._entityFilter.initializeDropdowns = function () {
          try {
            const _this = this;
            if (!window.jQuery || !$.fn || typeof $.fn.select2 !== 'function') {
              // No select2: fall back to product behavior.
              return originalInitDropdowns();
            }

            window._smartviewDropdownCache = window._smartviewDropdownCache || {};

            const pageSize = Number(window._smartviewDropdownPageSize || 500) || 500;

            function getMetaForField(fldId) {
              const arr = Array.isArray(_this.metaData) ? _this.metaData : [];
              const fid = (fldId || '').toString().toLowerCase();
              return arr.find(m => ((m.fldname || '').toString().toLowerCase() === fid)) || null;
            }

            function buildPsrctxt(meta, fldId) {
              if (!meta) meta = {};
              const direct = (meta.psrctxt || meta.psrcTxt || '').toString().trim();
              if (direct) return direct;

              const fld = (meta.fldname || fldId || '').toString().trim();
              const norm = (String(meta.normalized || '').toUpperCase() === 'T') ? 'T' : 'F';
              const st = (meta.srctable || '').toString().trim();
              const sf = (meta.srcfld || '').toString().trim();
              if (!fld || !st || !sf) return '';
              return `${fld}~${norm}~${st}~${sf}`;
            }

            function parseRowsFromAxList(resp) {
              const parsed = (typeof safeParseAxResponse === 'function') ? safeParseAxResponse(resp) : resp;
              const dsBlock = parsed?.result?.data?.[0] || parsed?.data?.[0] || parsed?.result || parsed;
              let rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
              // some responses embed array under dsBlock.data[0].data
              if (rows && rows.length && rows[0] && Array.isArray(rows[0].data)) rows = rows[0].data;
              return Array.isArray(rows) ? rows : [];
            }

            function rowToOption(row) {
              if (row === null || row === undefined) return null;
              if (typeof row === 'string' || typeof row === 'number') {
                const v = String(row).trim();
                return v ? { id: v, text: v } : null;
              }
              if (typeof row !== 'object') return null;

              const id = row.datavalue ?? row.value ?? row.id ?? row.code ?? row.name ?? row.text;
              const text = row.datacaption ?? row.caption ?? row.text ?? row.value ?? row.name ?? row.code ?? row.id;

              const idStr = (id === null || id === undefined) ? '' : String(id).trim();
              const textStr = (text === null || text === undefined) ? '' : String(text).trim();
              const cleanId = (idStr && idStr.toLowerCase() !== 'null') ? idStr : '';
              const cleanText = (textStr && textStr.toLowerCase() !== 'null') ? textStr : cleanId;

              if (!cleanId) {
                // fallback: pick first usable value
                for (const k in row) {
                  const v = row[k];
                  if (v === null || v === undefined) continue;
                  const s = String(v).trim();
                  if (!s || s.toLowerCase() === 'null') continue;
                  return { id: s, text: s };
                }
                return null;
              }

              return { id: cleanId, text: cleanText || cleanId };
            }

            let needsProductInit = false;

            document.querySelectorAll(`#dvModalFilter .filter-fld[data-type=DropDown]`).forEach(fld => {
              try {
                const fldId = (fld.id || '').replace('filter_', '');
                const meta = getMetaForField(fldId);
                const psrctxt = buildPsrctxt(meta, fldId);

                // Only SmartView ADS dropdowns with psrctxt are handled here; others fall back to product init.
                if (!psrctxt) { needsProductInit = true; return; }

                const $fld = $(fld);

                // Ensure wrapper + buttons exist (idempotent)
                let $wrapper = $fld.closest('.dropdown-wrapper');
                if ($wrapper.length === 0) {
                  $wrapper = $('<div class="dropdown-wrapper" style="position:relative;"></div>');
                  $fld.wrap($wrapper);
                  $wrapper = $fld.closest('.dropdown-wrapper');
                }

                if ($wrapper.find('.dropdown-toggle-btn').length === 0) {
                  $wrapper.append(`<button type="button" class="dropdown-toggle-btn btn btn-sm" aria-label="Open" title="Open"
                    style="position:absolute; right:36px; top:6px; z-index:1100; height:30px; padding:0 6px; line-height:1;">▾</button>`);
                }
                if ($wrapper.find('.clear-all-btn').length === 0) {
                  $wrapper.append(`<button type="button" class="clear-all-btn btn btn-sm" aria-label="Clear" title="Clear"
                    style="position:absolute; right:4px; top:6px; z-index:1100; height:30px; padding:0 6px; line-height:1; display:none;">✕</button>`);
                }

                const $openBtn = $wrapper.find('.dropdown-toggle-btn');
                const $clearBtn = $wrapper.find('.clear-all-btn');

                // Initialize Select2 (if not already initialized)
                if (!$fld.hasClass('select2-hidden-accessible')) {
                  $fld.select2({
                    multiple: true,
                    width: '100%',
                    ajax: {
                      delay: 150,
                      transport: function (params, success, failure) {
                        try {
                          const term = (params && params.data && params.data.term) ? String(params.data.term).toLowerCase() : '';
                          const cacheKey = psrctxt;
                          const cached = window._smartviewDropdownCache[cacheKey];

                          const filterAndReturn = function (all) {
                            const arr = Array.isArray(all) ? all : [];
                            const filtered = term
                              ? arr.filter(o => (o && ((o.text || '').toLowerCase().includes(term) || (o.id || '').toLowerCase().includes(term))))
                              : arr;
                            success({ results: filtered.slice(0, pageSize) });
                          };

                          if (Array.isArray(cached) && cached.length) {
                            filterAndReturn(cached);
                            return;
                          }

                          const callParams = {
                            adsNames: ['ds_smartlist_filters'],
                            refreshCache: false,
                            sqlParams: { psrctxt: psrctxt },
                            props: { ADS: true, CachePermissions: true, getallrecordscount: false, pageno: 1, pagesize: 0 }
                          };

                          const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
                            : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
                            : null;

                          if (!caller || typeof caller.GetDataFromAxList !== 'function') {
                            failure && failure(new Error('GetDataFromAxList not available for ds_smartlist_filters'));
                            return;
                          }

                          caller.GetDataFromAxList(callParams, function (resp) {
                            try {
                              const rows = parseRowsFromAxList(resp);
                              const seen = new Set();
                              const opts = [];
                              rows.forEach(r => {
                                const o = rowToOption(r);
                                if (!o || !o.id) return;
                                const key = String(o.id).toLowerCase();
                                if (seen.has(key)) return;
                                seen.add(key);
                                opts.push(o);
                              });
                              window._smartviewDropdownCache[cacheKey] = opts;
                              filterAndReturn(opts);
                            } catch (e) {
                              console.error('ds_smartlist_filters parse error', e);
                              success({ results: [] });
                            }
                          }, function (err) {
                            console.warn('ds_smartlist_filters call failed', err);
                            success({ results: [] });
                          });
                        } catch (e) {
                          failure && failure(e);
                        }
                      },
                      processResults: function (data) { return data; },
                      cache: true
                    },
                    escapeMarkup: function (m) { return m; }
                  });
                }

                $openBtn.off('click.smartviewDropOpen').on('click.smartviewDropOpen', function (e) {
                  e.preventDefault();
                  try { $fld.select2('open'); } catch (err) {}
                });

                $clearBtn.off('click.smartviewDropClear').on('click.smartviewDropClear', function (e) {
                  e.preventDefault();
                  $fld.val(null).trigger('change');
                  try { $fld.select2('close'); } catch (err) {}
                });

                const updateClearVisibility = function () {
                  const values = $fld.val() || [];
                  if (values.length > 0) $clearBtn.show();
                  else $clearBtn.hide();
                };

                $fld.off('.smartviewDropEvents')
                  .on('change.smartviewDropEvents', updateClearVisibility)
                  .on('select2:select.smartviewDropEvents', updateClearVisibility)
                  .on('select2:unselect.smartviewDropEvents', updateClearVisibility);

                updateClearVisibility();

              } catch (err) {
                console.error('SmartView initializeDropdowns error', err);
              }
            });

            // If there are any dropdowns without psrctxt, let product init handle them.
            // (call once; it will no-op for already initialized selects)
            if (needsProductInit) {
              try { originalInitDropdowns(); } catch (e) {}
            }

          } catch (err) {
            console.error('SmartView patched initializeDropdowns failed, falling back to product method', err);
            return originalInitDropdowns();
          }
        };

        window._entityFilter._smartviewDropdownAdsPatched = true;
      }

      // Patch EntityFilter.editPill so it always rebuilds the filter UI before prefilling values.
      // The product code only rebuilds when #dvModalFilter is empty, which can leave stale values around.
      if (window._entityFilter && typeof window._entityFilter.editPill === 'function' && !window._entityFilter._smartviewEditPillPatched) {
        const originalEditPill = window._entityFilter.editPill.bind(window._entityFilter);
        window._entityFilter.editPill = function (key) {
          try {
            const dv = document.getElementById('dvModalFilter');
            if (dv) dv.innerHTML = '';
          } catch (e) {}
          return originalEditPill(key);
        };
        window._entityFilter._smartviewEditPillPatched = true;
      }

      // Patch EntityFilter.createFilterPills to undo `.d-none` left behind by removeFilter()
      // (Bootstrap's d-none uses `display:none !important`, which otherwise keeps pills hidden forever.)
      if (window._entityFilter && typeof window._entityFilter.createFilterPills === 'function' && !window._entityFilter._smartviewCreatePillsPatched) {
        const originalCreateFilterPills = window._entityFilter.createFilterPills.bind(window._entityFilter);
        window._entityFilter.createFilterPills = function () {
          const res = originalCreateFilterPills();
          try {
            const hasAny = this.filterObj && Object.keys(this.filterObj).length > 0;
            if (window.jQuery) {
              const $pills = $('.filterPills');
              if ($pills && $pills.length) {
                if (hasAny) $pills.removeClass('d-none').css('display', 'flex');
                else $pills.addClass('d-none').css('display', 'none');
              }
            } else {
              const el = document.querySelector('.filterPills');
              if (el) {
                if (hasAny) { el.classList.remove('d-none'); el.style.display = 'flex'; }
                else { el.classList.add('d-none'); el.style.display = 'none'; }
              }
            }
          } catch (e) {}
          return res;
        };
        window._entityFilter._smartviewCreatePillsPatched = true;
      }

      // Patch EntityFilter.readFilterInput/updateFilterLayout for field IDs that contain special characters
      // like "." (CSS selectors break, but getElementById works).
      if (window._entityFilter && !window._entityFilter._smartviewIdSafePatched) {
        const originalRead = (typeof window._entityFilter.readFilterInput === 'function')
          ? window._entityFilter.readFilterInput.bind(window._entityFilter)
          : null;

        window._entityFilter.readFilterInput = function () {
          try {
            const _this = this;
            const filterArray = [];

            document.querySelectorAll(`#dvModalFilter .filter-fld`).forEach(fld => {
              const fldId = (fld.id || '').replace("filter_", "");
              let filterval = fld.value;
              let fldType = (fld.dataset.type || "").toUpperCase();

              let tempObj = {};
              switch (fldType) {
                case "DROPDOWN":
                  if (EntityFilter.inValid(filterval) || filterval == 0) return;
                  filterval = $(fld).val();
                  tempObj = { fldname: fldId, datatype: fldType, value: filterval };
                  filterArray.push(tempObj);
                  break;

                case "DATE": {
                  const dates = fld.querySelectorAll("input");
                  const fromDate = dates[0];
                  const toDate = dates[1];
                  if ((!fromDate || fromDate.value === "") && (!toDate || toDate.value === "")) return;

                  tempObj = { fldname: fldId, datatype: fldType, from: "", to: "" };

                  const toDdMmm = function (v) {
                    if (!v) return "";
                    const m = moment(v, ['YYYY-MM-DD', advFilterDtCulture, 'MM/DD/YYYY', 'DD/MM/YYYY', 'DD-MMM-YYYY'], true);
                    if (!m.isValid()) return "";
                    return m.format("DD-MMM-YYYY");
                  };

                  if (fromDate && fromDate.value !== "") {
                    const dd = toDdMmm(fromDate.value);
                    if (dd) tempObj["from"] = dd;
                  }
                  if (toDate && toDate.value !== "") {
                    const dd = toDdMmm(toDate.value);
                    if (dd) tempObj["to"] = dd;
                  }

                  const optEl = document.getElementById(`${fld.id}_dateoption`);
                  tempObj["condition"] = optEl ? optEl.value : "customOption";
                  filterArray.push(tempObj);
                  break;
                }

                case "NUMERIC": {
                  const nums = fld.querySelectorAll("input");
                  const fromNum = nums[0];
                  const toNum = nums[1];
                  if ((!fromNum || fromNum.value === "") && (!toNum || toNum.value === "")) return;

                  tempObj = { fldname: fldId, datatype: fldType, from: "", to: "" };

                  if (fromNum && fromNum.value !== "") tempObj["from"] = fromNum.value;
                  if (toNum && toNum.value !== "") tempObj["to"] = toNum.value;

                  filterArray.push(tempObj);
                  break;
                }

                case "TEXT": {
                  const fldVal = $(fld).val();
                  if (_entityCommon.inValid(fldVal)) return;

                  tempObj = { fldname: fldId, datatype: fldType };
                  tempObj["value"] = filterval;

                  const optEl = document.getElementById(`${fld.id}_searchoption`);
                  tempObj["condition"] = (optEl && optEl.value) ? optEl.value : "CONTAINS";
                  filterArray.push(tempObj);
                  break;
                }

                default:
                  break;
              }
            });

            _this.activeFilterArray = filterArray;
          } catch (err) {
            console.error('SmartView patched readFilterInput failed, falling back to product method', err);
            if (originalRead) originalRead();
          }
        };

        const originalUpdate = (typeof window._entityFilter.updateFilterLayout === 'function')
          ? window._entityFilter.updateFilterLayout.bind(window._entityFilter)
          : null;

        window._entityFilter.updateFilterLayout = function (activeFilter) {
          try {
            if (!activeFilter || !Array.isArray(activeFilter.filter)) return;

            activeFilter.filter.forEach(fldFilter => {
              const fldType = (fldFilter.datatype || '').toUpperCase();
              const fldId = fldFilter.fldname;
              const condition = fldFilter.condition;
              const value = fldFilter.value;

              switch (fldType) {
                case "TEXT": {
                  const inp = document.getElementById(`filter_${fldId}`);
                  if (inp) inp.value = value;
                  const sel = document.getElementById(`filter_${fldId}_searchoption`);
                  if (sel) sel.value = condition;
                  break;
                }
                case "DROPDOWN": {
                  const el = document.getElementById(`filter_${fldId}`);
                  if (!el) break;
                  const $el = $(el);
                  (value || []).forEach(item => {
                    const option = new Option(item, item);
                    $el.append(option);
                  });
                  $el.val(value).trigger('change');
                  break;
                }
                case "DATE": {
                  const sel = document.getElementById(`filter_${fldId}_dateoption`);
                  if (sel) sel.value = condition;
                  if (sel) $(sel).trigger('change');

                  const fromDate = document.getElementById(`filter_${fldId}_from`);
                  const toDate = document.getElementById(`filter_${fldId}_to`);

                  if (condition === "customOption") {
                    const setDateVal = function (el, raw) {
                      if (!el) return;
                      if (!raw) { el.value = ""; return; }
                      const m = moment(raw, ['DD-MMM-YYYY', 'YYYY-MM-DD', advFilterDtCulture, 'MM/DD/YYYY', 'DD/MM/YYYY'], true);
                      if (!m.isValid()) { el.value = ""; return; }
                      if ((el.type || '').toLowerCase() === 'date') el.value = m.format('YYYY-MM-DD');
                      else el.value = m.format(advFilterDtCulture);
                    };
                    setDateVal(fromDate, fldFilter.from);
                    setDateVal(toDate, fldFilter.to);
                    $(fromDate).prop('disabled', false).addClass('disabledDate');
                    $(toDate).prop('disabled', false).addClass('disabledDate');
                  } else {
                    $(fromDate).prop('disabled', true).removeClass('disabledDate');
                    $(toDate).prop('disabled', true).removeClass('disabledDate');
                  }
                  break;
                }
                case "NUMERIC": {
                  const fromNum = document.getElementById(`filter_${fldId}_from`);
                  const toNum = document.getElementById(`filter_${fldId}_to`);
                  if (fromNum) fromNum.value = fldFilter.from || "";
                  if (toNum) toNum.value = fldFilter.to || "";
                  break;
                }
                default:
                  break;
              }
            });
          } catch (err) {
            console.error('SmartView patched updateFilterLayout failed, falling back to product method', err);
            if (originalUpdate) originalUpdate(activeFilter);
          }
        };

        window._entityFilter._smartviewIdSafePatched = true;
      }

      // IMPORTANT: mimic EntityFilter.init() behaviour so applying from the toolbar creates a NEW pill
      // (init resets activeFilterId/name; without this, apply overwrites the previous pill)
      window._entityFilter.activeFilterArray = [];
      window._entityFilter.activeFilterName = '';
      window._entityFilter.activeFilterId = '';

      window._entityFilter.createFilterLayout && window._entityFilter.createFilterLayout(false);

      // Reset "save filter" UI (SmartView doesn't use it, but EntityFilter methods expect the elements)
      try {
        const cb = document.getElementById('filterGroupCheckbox');
        if (cb) cb.checked = false;
        const name = document.getElementById('filterGroupName');
        if (name) { name.value = ''; name.disabled = true; }
      } catch (e) {}

      // Ensure Apply button invokes the EntityFilter handler (minimal fix)
if (window._entityFilter && typeof window._entityFilter.handleApply === 'function') {
  // remove any existing handlers to avoid duplicate calls
  $('#applyFilterButton').off('click.smartview_apply').on('click.smartview_apply', function (ev) {
    try {
      window._entityFilter.handleApply();
    } catch (err) {
      console.error('error calling entityFilter.handleApply', err);
    }
    return false;
  });
}

(function attachEntityFilterToSmartView() {
  try {
    if (!window._entityFilter) {
      // Nothing to attach yet — keep safe bindings (product code expects these handlers)
      $('#applyFilterButton').off('click.smartview_apply').on('click.smartview_apply', function (ev) {
        ev.preventDefault();
        try {
          if (window._entityFilter && typeof window._entityFilter.handleApply === 'function') {
            window._entityFilter.handleApply();
          } else if (window._pendingEntityFilterPayload) {
            const payload = window._pendingEntityFilterPayload;
            window._pendingEntityFilterPayload = undefined;
            if (window.applyEntityFiltersToSmartview) window.applyEntityFiltersToSmartview(payload);
          } else {
            console.warn('apply clicked: EntityFilter not initialized yet.');
          }
        } catch (err) { console.error('apply button fallback error', err); }
        return false;
      });

      $('#clearFilterBtn').off('click.smartviewClear').on('click.smartviewClear', function () {
        try {
          if (window._entityFilter && typeof window._entityFilter.clearFilters === 'function') window._entityFilter.clearFilters();
          window._pendingEntityFilterPayload = undefined;
          const ctrl = window.smartTableController || window._smartviewController || null;
          if (ctrl) {
            ctrl.filters = [];
            // clear only filter-related sqlParams keys; keep any ADS params that the page set
            try {
              ctrl._entitySqlParams = ctrl._entitySqlParams || {};
              try { delete ctrl._entitySqlParams.FILTERS; } catch (e) {}
              const prevKeys = Array.isArray(ctrl._smartviewFilterSqlKeys) ? ctrl._smartviewFilterSqlKeys : [];
              prevKeys.forEach(k => { try { delete ctrl._entitySqlParams[k]; } catch (e) {} });
              ctrl._smartviewFilterSqlKeys = [];
            } catch (e) {}
            typeof ctrl.resetPaging === 'function' && ctrl.resetPaging();
            typeof ctrl.loadNextPage === 'function' && ctrl.loadNextPage();
          }
        } catch (ex) { console.error('clearFilterBtn fallback error', ex); }
        return false;
      });

      return;
    }

    // Helper: flatten EntityFilter objects -> simple sql params
    function buildSqlParamsFromFilters(filters) {
      const params = {};
      if (!Array.isArray(filters)) return params;

      filters.forEach(f => {
        // many payloads use fldname / ftransid / datatype or fdatatype
        let rawName = (f.fldname || f.name || f.field || '').toString();
        let name = rawName.replace(/^filter_/, '').trim(); // remove any "filter_" prefix and trim spaces
        if (!name) return;

        const t = ((f.datatype || f.fdatatype || '').toString() || '').toUpperCase();

        // normalize values that entity filter may use
        if (['TEXT', 'C', 'STRING', 'T'].includes(t)) {
          if (f.value !== undefined && f.value !== '') params[name] = f.value;
        } else if (['NUMERIC', 'NUMBER', 'N'].includes(t)) {
          if (f.from !== undefined && f.from !== '') params[`${name}_from`] = f.from;
          if (f.to !== undefined && f.to !== '') params[`${name}_to`] = f.to;
          if (f.value !== undefined && f.value !== '') params[name] = f.value;
        } else if (['DATE', 'D'].includes(t)) {
          if (f.from) params[`${name}_from`] = f.from;
          if (f.to) params[`${name}_to`]   = f.to;
        } else if (['DROPDOWN', 'SELECT'].includes(t)) {
          if (Array.isArray(f.value)) params[name] = f.value.join(',');
          else if (f.value !== undefined && f.value !== '') params[name] = f.value;
        } else if (['BOOLEAN','CHECKBOX','B'].includes(t)) {
          params[name] = (f.value === true || f.value === 'T' || f.value === 'true') ? 'T' : 'F';
        } else {
          // fallback: copy whatever is present
          if (f.value !== undefined && f.value !== '') params[name] = f.value;
          if (f.from !== undefined && f.from !== '') params[`${name}_from`] = f.from;
          if (f.to   !== undefined && f.to   !== '') params[`${name}_to`] = f.to;
        }
      });

      return params;
    }

    // Ensure .filterPills exists so UI shows (product code expects it)
    (function ensureFilterPillsContainer() {
      if (document.querySelector('.filterPills')) return;
      const parent = document.querySelector('.page-header') || document.querySelector('.toolbar') || document.body;
      const wrapper = document.createElement('div');
      wrapper.className = 'filterPills flex-row py-2 px-2 gap-3';
      wrapper.style.display = 'none';
      wrapper.innerHTML = '<button class="filterGroupBadge badge rounded-pill bg-primary d-flex align-items-center gap-2 py-2 px-6 border-0" style="max-width: fit-content;" data-toggle="tooltip" data-placement="top" data-html="true">All</button>';
      parent.insertBefore(wrapper, parent.firstChild);
    })();

    // Ensure apply button calls EntityFilter.handleApply (product behavior)
    $('#applyFilterButton').off('click.smartview_apply').on('click.smartview_apply', function (ev) {
      ev.preventDefault();
      try {
        if (window._entityFilter && typeof window._entityFilter.handleApply === 'function') {
          window._entityFilter.handleApply();
        } else if (typeof window._entityFilter.applyFilters === 'function') {
          window._entityFilter.applyFilters();
        } else {
          console.warn('apply clicked: no handleApply/applyFilters on _entityFilter');
        }
      } catch (err) { console.error('apply button binding error', err); }
      return false;
    });

    // MAIN OVERRIDE: set applyFilters on the EntityFilter instance
    window._entityFilter.applyFilters = function () {
      try {
        const filters = stripSmartviewFilterTransId(Array.isArray(this.activeFilterArray) ? this.activeFilterArray : []);
        console.debug('EntityFilter.applyFilters ->', filters);

        // find controller (support several possible names)
        const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || (typeof controller !== 'undefined' ? controller : null);

        if (ctrl) {
          const hasFilters = Array.isArray(filters) && filters.length > 0;
          // attach structured filters
          ctrl.filters = hasFilters ? filters : [];
          // Prefer server-side filtering using `props.filters` (AxList API contract).
          // If needed for troubleshooting, you can force client-side filtering by setting:
          //   window._smartviewForceClientFiltering = true;
          ctrl.forceClientFiltering = !!window._smartviewForceClientFiltering && hasFilters;
          ctrl._filteredCache = null;
          window._smartviewFullData = null;

          // IMPORTANT: Do NOT pass filters via sqlParams (no FILTERS string / no *_from keys).
          // Filters must be sent as JSON array in `props.filters`.
          try {
            ctrl._entitySqlParams = ctrl._entitySqlParams || {};

            // remove any previous FILTERS string
            try { delete ctrl._entitySqlParams.FILTERS; } catch (e) {}

            // remove any previously injected flattened filter keys
            const prevKeys = Array.isArray(ctrl._smartviewFilterSqlKeys) ? ctrl._smartviewFilterSqlKeys : [];
            prevKeys.forEach(k => { try { delete ctrl._entitySqlParams[k]; } catch (e) {} });

            // compute current would-be keys (for cleanup) but DO NOT assign them
            const flat = buildSqlParamsFromFilters(filters);
            const nextKeys = flat ? Object.keys(flat) : [];
            ctrl._smartviewFilterSqlKeys = nextKeys;
            nextKeys.forEach(k => { try { delete ctrl._entitySqlParams[k]; } catch (e) {} });
          } catch (e) {
            console.warn('smartview filter sqlParams cleanup failed', e);
          }

          // ensure we start from page 1
          try {
            if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
            else { ctrl.pageno = 1; }
          } catch (e) { /* ignore */ }

          // trigger reload
          try { typeof ctrl.loadNextPage === 'function' && ctrl.loadNextPage(); } catch (e) { console.warn('loadNextPage failed', e); }

        } else {
          // controller not ready — stash payload for controller init to pick up
          window._pendingEntityFilterPayload = {
            filters: filters,
            timestamp: Date.now()
          };

          // fallback single-shot server call using GetDataFromAxList (buildParams merges ._entitySqlParams internally)
          try {
            const params = (typeof buildParams === 'function') ? buildParams(1) : { adsNames: [window._entity && window._entity.adsName], props: { ADS: true }, sqlParams: {} };
            params.props = params.props || {};
            params.props.filters = filters;
            // do not send FILTERS / flattened sqlParams; backend expects props.filters JSON
            params.sqlParams = Object.assign({}, params.sqlParams || {});

            const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window : null;
            if (caller && typeof caller.GetDataFromAxList === 'function') {
              console.debug('EntityFilter fallback calling GetDataFromAxList with params ->', params);
              caller.GetDataFromAxList(params, function (resp) {
                try {
                  const parsed = (typeof resp === 'string') ? JSON.parse(resp) : resp;
                  const rows = parsed && parsed.result && parsed.result.data && Array.isArray(parsed.result.data[0]?.data) ? parsed.result.data[0].data : (parsed.data || []);
                  if (typeof createTableViewHTML === 'function') createTableViewHTML(rows, 1);
                } catch (e) { console.error('fallback GetDataFromAxList parse error', e); }
              });
            }
          } catch (e) {
            console.error('applyFilters fallback error', e);
          }
        }

        // hide the modal after apply
        try {
          const modalEl = document.getElementById('filterModal');
          if (modalEl && typeof bootstrap !== 'undefined' && bootstrap.Modal) {
            const bs = bootstrap.Modal.getInstance(modalEl);
            if (bs) bs.hide();
          } else {
            $('#filterModal').modal('hide');
          }
        } catch (e) { /* ignore */ }

      } catch (error) {
        console.error('entityFilter.applyFilters error', error);
      }
    };

    // Wire Clear button (clear both EntityFilter saved state and SmartView controller state)
    $('#clearFilterBtn').off('click.smartviewClear').on('click.smartviewClear', function () {
      try {
        if (window._entityFilter && typeof window._entityFilter.clearFilters === 'function') window._entityFilter.clearFilters();
        const ctrl = window.smartTableController || window._smartviewController || null;
        if (ctrl) {
          ctrl.filters = [];
          // clear only filter-related sqlParams keys; keep any ADS params that the page set
          try {
            ctrl._entitySqlParams = ctrl._entitySqlParams || {};
            try { delete ctrl._entitySqlParams.FILTERS; } catch (e) {}
            const prevKeys = Array.isArray(ctrl._smartviewFilterSqlKeys) ? ctrl._smartviewFilterSqlKeys : [];
            prevKeys.forEach(k => { try { delete ctrl._entitySqlParams[k]; } catch (e) {} });
            ctrl._smartviewFilterSqlKeys = [];
          } catch (e) {}
          ctrl.forceClientFiltering = false;
          ctrl._filteredCache = null;
          window._smartviewFullData = null;
          typeof ctrl.resetPaging === 'function' && ctrl.resetPaging();
          typeof ctrl.loadNextPage === 'function' && ctrl.loadNextPage();
        }
        $('#filterModal').modal('hide');
      } catch (ex) { console.error('clearFilterBtn handler error', ex); }
      return false;
    });

    // If there were pending filters applied before controller init, apply them now
    try {
      const pending = window._pendingEntityFilterPayload;
      if (pending && pending.filters && (window.smartTableController || window._smartviewController || window._smartviewTableController)) {
        const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController;
        ctrl.filters = pending.filters;
        // Filters are passed via props.filters; do not touch sqlParams here.
        try { delete window._pendingEntityFilterPayload; } catch (e) {}
      }
    } catch (e) { /* ignore */ }

  } catch (e) {
    console.error('attachEntityFilterToSmartView error', e);
  }
})();



      // show the modal (bootstrap or jQuery)
      const filterModalEl = document.getElementById('filterModal');
      if (filterModalEl) {
        if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
          const bs = new bootstrap.Modal(filterModalEl);
          bs.show();
        } else {
          $('#filterModal').modal('show');
        }
      }
    };

    // If metadata cached for this ADS, show modal immediately
    if (controller.lastAdsMeta && Array.isArray(controller.lastAdsMeta) && controller.lastAdsMeta.length) {
      showModalWithFilter();
      return;
    }

    // otherwise fetch ads metadata then show modal
    controller.fetchAdsMetadata(function(err, meta) {
      if (err) {
        console.warn('openFilters: fetchAdsMetadata returned error, opening filter UI without ADS metadata', err);
        showModalWithFilter();
        return;
      }
      showModalWithFilter();
    });
  } catch (ex) {
    console.error('openFilters unexpected error', ex);
  }
}
/**
 * Creates the Filter Modal exactly in the shape Entity-Filter.js expects.
 * Safe to call multiple times (will create only once).
 */
function createSmartviewFilterModal() {
  // Do not recreate if already exists
  if (document.getElementById('filterModal')) return;

  const modalHtml = `
  <div id="filterModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable" role="document">
      <div class="modal-content">

        <!-- HEADER -->
        <div class="modal-header">
          <h5 class="modal-title">Filters</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>

        <!-- BODY -->
        <div class="modal-body">
          <!-- Entity-Filter.js dynamically injects filter rows here -->
          <div id="dvModalFilter" class="row g-3"></div>
        </div>

        <!-- FOOTER -->
        <div class="modal-footer justify-content-between">
          <div class="d-flex align-items-center gap-3">
            <!-- Save Filter checkbox (Entity product compatibility) -->
            <div class="form-check">
              <input class="form-check-input" type="checkbox" id="filterGroupCheckbox">
              <label class="form-check-label" for="filterGroupCheckbox">
                Save filter
              </label>
            </div>

            <!-- Entity-Filter.js expects this input to exist for edit-pill -->
            <input type="text"
                   id="filterGroupName"
                   class="form-control form-control-sm"
                   placeholder="Filter name"
                   style="max-width: 260px;"
                   disabled>
          </div>

          <div class="d-flex gap-2">
            <!-- Clear button -->
            <button type="button"
                    id="clearFilterBtn"
                    class="btn btn-outline-secondary">
              Clear
            </button>

            <!-- Apply button (Entity-Filter.js binds to this ID) -->
            <button type="button"
                    id="applyFilterButton"
                    class="btn btn-primary">
              Apply
            </button>
          </div>
        </div>

      </div>
    </div>
  </div>
  `;

  // Inject modal into DOM
  const wrapper = document.createElement('div');
  wrapper.innerHTML = modalHtml;
  document.body.appendChild(wrapper);

  // Keep "Save filter" name input UX in sync (Entity-Filter.js toggles disabled state in edit mode too)
  try {
    const cb = document.getElementById('filterGroupCheckbox');
    const name = document.getElementById('filterGroupName');
    if (cb && name) {
      cb.addEventListener('change', function () {
        name.disabled = !cb.checked;
        if (!cb.checked) name.value = '';
      });
    }
  } catch (e) {}
}

// Some pages may already have a #filterModal but without the exact Entity-Filter.js IDs.
// This keeps SmartView compatible without touching product files.
function ensureSmartviewFilterCompatDom() {
  const modal = document.getElementById('filterModal');
  if (!modal) return;

  // #dvModalFilter is where EntityFilter injects rows
  if (!document.getElementById('dvModalFilter')) {
    const body = modal.querySelector('.modal-body') || modal;
    const dv = document.createElement('div');
    dv.id = 'dvModalFilter';
    dv.className = 'row g-3';
    body.appendChild(dv);
  }

  // Ensure save checkbox + name input exist so editPill() doesn't crash
  const footer = modal.querySelector('.modal-footer') || modal;

  if (!document.getElementById('filterGroupCheckbox')) {
    const wrap = document.createElement('div');
    wrap.className = 'form-check';
    wrap.innerHTML = `
      <input class="form-check-input" type="checkbox" id="filterGroupCheckbox">
      <label class="form-check-label" for="filterGroupCheckbox">Save filter</label>
    `;
    footer.insertBefore(wrap, footer.firstChild);
  }

  if (!document.getElementById('filterGroupName')) {
    const inp = document.createElement('input');
    inp.type = 'text';
    inp.id = 'filterGroupName';
    inp.className = 'form-control form-control-sm';
    inp.placeholder = 'Filter name';
    inp.style.maxWidth = '260px';
    inp.disabled = true;

    footer.insertBefore(inp, footer.firstChild ? footer.firstChild.nextSibling : null);

    const cb = document.getElementById('filterGroupCheckbox');
    if (cb) {
      cb.addEventListener('change', function () {
        inp.disabled = !cb.checked;
        if (!cb.checked) inp.value = '';
      });
    }
  }
}

// Add these functions near the top, after the escapeHtml function

/* ---------- Row Options Parsing ---------- */
function parseAxRowOptionsField(row) {
  // Check for axrowoptions field
  const cand = row.axrowoptions || row.axRowOptions || row.axRowoptions || '';
  if(!cand) return [];
  
  console.log('Parsing axrowoptions:', cand);
  
  // Try to parse as JSON first
  if(typeof cand === 'string') {
    try {
      const parsed = JSON.parse(cand);
      if(Array.isArray(parsed)) return parsed;
      if(typeof parsed === 'object') return [parsed];
    } catch(e) {
      // If not JSON, try to parse the format: "smartlts,opn,Add,script,add_circle_outline,"
      console.log('Not JSON, trying comma-separated format');
      const parts = cand.split(',').filter(p => p.trim() !== '');
      
      if (parts.length >= 3) {
        // Format: name, operation, text, type, icon
        return [{
          name: parts[0] || 'Action',
          operation: parts[1] || 'open',
          text: parts[2] || 'Open',
          type: parts[3] || 'script',
          icon: parts[4] || '',
          link: buildLinkFromParts(parts) // Helper function to build link
        }];
      }
    }
  } else if(Array.isArray(cand)) {
    return cand;
  } else if(typeof cand === 'object') {
    return [cand];
  }
  
  return [];
}

// Helper function to build link from parts
function buildLinkFromParts(parts) {
  // Simple example: if first part is a known entity type
  if (parts[0] === 'smartlts' || parts[0] === 'tstruct') {
    // Build a tstruct link
    return `tstruct.aspx?transid=${parts[0]}&act=open&AxPop=true`;
  }
  return '';
}
/* ---------- Open Link in Popup ---------- */
function openLinkInPopup(input) {
  try {
    if (!input) return;

    let linkStr = '';
    
    if (typeof input === 'object') {
      if (Array.isArray(input) && input.length > 0) {
        const first = input[0];
        linkStr = (first && (first.link || first.Link)) || '';
      } else {
        linkStr = (input.link || input.Link || '') || '';
      }
    } else if (typeof input === 'string') {
      const s = input.trim();
      
      if ((s.startsWith('{') && s.endsWith('}')) || (s.startsWith('[') && s.endsWith(']'))) {
        try {
          const parsed = JSON.parse(s);
          if (Array.isArray(parsed) && parsed.length > 0) {
            linkStr = parsed[0].link || parsed[0].Link || '';
          } else if (parsed && typeof parsed === 'object') {
            linkStr = parsed.link || parsed.Link || '';
          }
        } catch (err) {}
      }
      
      if (!linkStr) {
        const m = s.match(/["']?\s*link\s*["']?\s*:\s*["']([^"']+)["']/i);
        if (m && m[1]) linkStr = m[1].trim();
      }
      
      if (!linkStr) linkStr = s;
    }

    if (!linkStr) return;

    linkStr = linkStr.trim();
    const type = linkStr[0] ? linkStr[0].toLowerCase() : '';
    const rest = linkStr.slice(1);
    const parts = rest.split('(');
    const struct = parts[0];
    const params = parts[1] ? parts[1].slice(0, -1) : '';
    const paramString = params ? params.replaceAll('~', '^') : '';
    const paramPairs = paramString ? paramString.split('^').map(p => p.split('=')) : [];
    const qp = new URLSearchParams();
    paramPairs.forEach(([k, v]) => { if (k) qp.append(k.trim(), (v || '').trim()); });

    let url = '';

    if (type === 'i') url = `ivtoivload.aspx?ivname=${struct}&${qp.toString()}`;
    else if (type === 't') {
      // New hyperlink format (tstruct loader)
      // Example:
      //   ivtstload.aspx?tstname=gsupp&party_name=...&hltype=load&torecid=false
      qp.set('hltype', 'load');
      qp.set('torecid', 'false');
      const qs = qp.toString();
      url = qs ? `ivtstload.aspx?tstname=${struct}&${qs}` : `ivtstload.aspx?tstname=${struct}&hltype=load&torecid=false`;
    } else if (type === 'h') {
      if ((linkStr[0] + linkStr[1]).toLowerCase() === "hp" && (/^\d+$/.test(struct.slice(1)))) 
        url = `htmlpages.aspx?load=${struct.slice(1)}&${qp.toString()}`;
      else url = `htmlpages.aspx?loadcaption=${struct}&${qp.toString()}`;
    } else if (type === 'l') url = `entity.aspx?tstid=${struct}&${qp.toString()}`;
    else if (type === 'd') url = `entityform.aspx?tstid=${struct}&${qp.toString()}`;

    // Open the popup
    if (url) {
      if (typeof parent.createPopup === 'function') {
        parent.createPopup(url, true, ()=>{}, ()=>{});
      } else {
        window.open(url, '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=900,height=700');
      }
    }
  } catch (err) {
    console.error('openLinkInPopup error', err);
  }
}

/* ---------- Show Row Options Menu ---------- */
/* ---------- Show Row Options Menu ---------- */
function showAxRowOptionsMenu(anchorBtn, actions){
  // remove existing menu
  const existing = document.querySelector('.axrow-menu');
  if(existing) existing.remove();
  if(!actions || !actions.length) return;
  
  const menu = document.createElement('div');
  menu.className = 'axrow-menu';
  menu.style.cssText = `
    position: absolute;
    background: #fff;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    box-shadow: 0 4px 20px rgba(0,0,0,0.15);
    z-index: 1000;
    padding: 4px 0;
    min-width: 160px;
  `;

  actions.forEach(act => {
    // Handle different formats
    const name = act.name || act.Name || act.title || act.text || 'Open';
    const link = act.link || act.Link || act.l || act.url || act.Url || '';
    const icon = act.icon || act.Icon || '';
    
    const b = document.createElement('button');
    b.type = 'button';
    b.className = 'axrow-menu-item';
    b.style.cssText = `
      display: flex;
      align-items: center;
      gap: 8px;
      width: 100%;
      padding: 8px 12px;
      border: none;
      background: transparent;
      text-align: left;
      cursor: pointer;
      font-size: 13px;
      color: #333;
    `;
    
    if (icon) {
      const iconSpan = document.createElement('span');
      // Handle icon string (could be material icon name or HTML)
      if (icon.includes('material-icons') || icon.includes('<')) {
        iconSpan.innerHTML = icon;
      } else {
        iconSpan.textContent = icon;
      }
      iconSpan.style.fontSize = '16px';
      b.appendChild(iconSpan);
    }
    
    const textSpan = document.createElement('span');
    textSpan.textContent = String(name);
    b.appendChild(textSpan);
    
    b.dataset.link = link;
    
    // Hover effect
    b.addEventListener('mouseenter', function() {
      this.style.background = '#f5f5f5';
    });
    b.addEventListener('mouseleave', function() {
      this.style.background = 'transparent';
    });
    
    b.addEventListener('click', function(e){
      e.stopPropagation();
      menu.remove();
      if(link) {
        openLinkInPopup(link);
      } else {
        console.warn('No link provided for action:', name);
      }
    });
    
    menu.appendChild(b);
  });
  
  document.body.appendChild(menu);
  
  // Position menu
  const r = anchorBtn.getBoundingClientRect();
  menu.style.left = Math.min(window.innerWidth - menu.offsetWidth - 10, r.left) + 'px';
  menu.style.top = (r.bottom + 6) + 'px';

  // Click outside to close
  setTimeout(() => {
    document.addEventListener('click', function _close(e){
      if(!menu.contains(e.target) && e.target !== anchorBtn){
        menu.remove();
        document.removeEventListener('click', _close);
      }
    });
  }, 20);
}
/* --------------------------
   Class controller
   -------------------------- */

class SmartViewTableController {
  constructor(opts = {}) {
    this.adsName = opts.adsName || "ds_smartlist_users";
    this.pageSize = opts.pageSize ?? 100;
    this.pageno = opts.currentPage ?? 1;
    this.sorting = opts.sorting || [];
    this.filters = opts.filters || [];
    this.axClient_dateformat = opts.axClient_dateformat || (typeof window.axClient_dateformat !== 'undefined' ? window.axClient_dateformat : "");
    this.select_columns = Array.isArray(opts.select_columns) ? opts.select_columns.slice() : [];
    this.groupby_columns = Array.isArray(opts.groupby_columns) ? opts.groupby_columns.slice() : [];
    this.aggregations = (opts.aggregations && typeof opts.aggregations === 'object') ? Object.assign({}, opts.aggregations) : {};
    this.deferInitialLoad = !!opts.deferInitialLoad;
    this.refreshCache = false;

    this.isFetching = false;
    this.totalCount = 0;
    this.loadedCount = 0;
    this._pagingFallbackTried = false;
    this._userHasScrolled = false;
    this._lastPageReached = false;

    this.init();
  }

  init() {
    window._entity = window._entity || {};
    window._entity.listJson = [];
    window._entity.pageSize = this.pageSize;
  
    // Ensure global adsName stored
    if (this.adsName) {
      window._entity.adsName = this.adsName;
      const titleEl = document.getElementById('EntityTitle') || document.querySelector('.page-header-title');
      if (titleEl) titleEl.textContent = this.adsName;
      document.title = this.adsName;
    }

    // Prefetch ADS metadata early (and store in localStorage). This makes Filters fast and enables hyperlinks.
    try { if (typeof this.ensureAdsMetadata === 'function') this.ensureAdsMetadata(); } catch (e) {}

    // Ensure hyperlink click handlers are attached (event delegation).
    try { attachSmartviewHyperlinkHandlers(); } catch (e) {}
    addRowOptionsStyles();
    addHeaderMenuStyles();
    attachSmartviewHeaderMenuHandlers();
    this.wireDom();
    this.setupSortingHeaders();

    if (!this.deferInitialLoad) {
      this.loadNextPage();
    } else {
      console.log('SmartViewTableController.init: deferInitialLoad=true, skipping initial data fetch');
    }

  
    setTimeout(() => {
      this.attachScrollListener();
    }, 150);
  }

wireDom() {
  const self = this;

  // Helper to safely attach a single handler (replaces previous assigned handler)
  function singleClickBind(id, handler) {
    const el = document.getElementById(id);
    if (!el) return;
    el.onclick = (ev) => {
      try { handler(ev); } catch (e) { console.error(`Handler ${id} error`, e); }
      return false;
    };
  }

  // Filters / paging / refresh handlers (keeps original behaviour)
  singleClickBind("applyFilterBtn", () => {
    self.collectFilters();
    self.resetPaging();
    self.loadNextPage();
  });

  singleClickBind("clearFilterBtn", () => {
    self.clearFilters();
    self.resetPaging();
    self.loadNextPage();
  });

  singleClickBind("loadAllRecordsBtn", () => {
    self.clearFilters();
    self.pageSize = 0;
    window._entity.pageSize = 0;
    self.resetPaging();
    self.loadAllOnce();
  });

  singleClickBind("refreshBtn", () => {
    self.refreshCache = true;
    self.resetPaging();
    self.loadNextPage();
  });

  // Page select change
  const pageSelect = document.getElementById("pageSelect");
  if (pageSelect) {
    pageSelect.onchange = (ev) => {
      try {
        const v = parseInt(ev.target.value, 10) || 1;
        self.resetPaging();
        self.pageno = v;
        self.loadNextPage();
      } catch (e) { console.error('pageSelect onchange', e); }
    };
  }

  // ADS picker button (if present)
  const adsBtn = document.getElementById("adsPickerBtn");
  if (adsBtn) {
    adsBtn.onclick = (ev) => { try { showAdsPickerModal(); } catch (e) { console.error('adsPickerBtn click error', e); } return false; };
  }



  // Export menu binding (keeps previous implementation)
  (function bindExportMenu(retries = 0) {
    const selectors = [
      '#exportMenuItem .menu-link',
      '.export-menu .menu-link',
      '.btn-export, [data-export-action]'
    ];
    const elems = Array.from(document.querySelectorAll(selectors.join(',')));
    if (elems && elems.length) {
      elems.forEach(el => {
        el.onclick = (ev) => {
          try {
            ev.preventDefault();
            const target = (el.getAttribute('data-target') || el.dataset.target || el.getAttribute('data-export-action') || '').toString();
            if (!target) {
              const text = (el.textContent || '').toLowerCase();
              if (text.includes('pdf')) handleExport('pdf');
              else if (text.includes('excel') || text.includes('xls')) handleExport('excel');
              else if (text.includes('csv')) handleExport('csv');
              else if (text.includes('print')) handleExport('print');
              else handleExport('pdf');
            } else {
              handleExport(target);
            }
          } catch (err) { console.error('export menu click error', err); }
          return false;
        };
      });
      return;
    }
    if (retries < 8) setTimeout(() => bindExportMenu(retries + 1), 250);
  })();

  // Keyboard shortcut Ctrl+E to open export (dev helper)
  try { window.removeEventListener('keydown', this._smartviewKeyHandler); } catch (e) {}
  this._smartviewKeyHandler = function (ev) {
    if ((ev.ctrlKey || ev.metaKey) && ev.key.toLowerCase() === 'e') {
      const menuOpenBtn = document.querySelector('.export-toggle, #exportMenuToggle, .btn-export');
      if (menuOpenBtn) menuOpenBtn.click();
      else handleExport('pdf');
      ev.preventDefault();
    }
  };
  window.addEventListener('keydown', this._smartviewKeyHandler, { passive: false });

  // Re-attach rowCount manager on resize
  try { if (this._resizeHandler) window.removeEventListener('resize', this._resizeHandler); } catch (e) {}
  this._resizeHandler = () => { try { if (rowCountManager && typeof rowCountManager.attachToView === 'function') rowCountManager.attachToView(); } catch (e) {} };
  window.addEventListener('resize', this._resizeHandler, { passive: true });
  bindSearchToggleFocus();


  const searchInput = document.getElementById('searchBox');
  const searchBtn = document.getElementById('searchBtn');
  const clearSearchBtn = document.getElementById('clearSearchBtn'); // optional

  const debouncedLiveSearch = debounce(liveSearch, 300);

  if (searchInput) {
    // remove existing handlers defensively
    try { searchInput.onkeyup = null; searchInput.oninput = null; } catch (e) {}
    searchInput.addEventListener('keyup', function () {
      // If enter pressed, run immediate search
      // (don't submit forms)
      debouncedLiveSearch();
    });
    searchInput.addEventListener('input', function () {
      debouncedLiveSearch();
    });
    // prevent default Enter submit behavior
    searchInput.addEventListener('keypress', function (event) {
      if (event.key === "Enter") event.preventDefault();
    });
  }

  if (searchBtn) {
    searchBtn.onclick = (ev) => {
      try { liveSearch(); } catch (e) { console.error('searchBtn click', e); }
      return false;
    };
  }

  if (clearSearchBtn) {
    clearSearchBtn.onclick = (ev) => {
      try {
        if (searchInput) { searchInput.value = ''; liveSearch(); }
      } catch (e) { console.error('clearSearchBtn click', e); }
      return false;
    };
  }
}




  collectFilters() {
    this.filters = [];
    const empCode = document.getElementById("empCodeFilter")?.value.trim() || "";
    const empCodeCond = document.getElementById("empCodeCondition")?.value || "CONTAINS";
    if (empCode) this.filters.push({ fldname: "employee_code", datatype: "TEXT", value: empCode, condition: empCodeCond });

    const salaryFrom = document.getElementById("salaryFrom")?.value || "";
    const salaryTo = document.getElementById("salaryTo")?.value || "";
    if (salaryFrom || salaryTo) this.filters.push({ fldname: "salary", datatype: "NUMERIC", from: salaryFrom || "0", to: salaryTo || "999999999" });

    const dojFrom = document.getElementById("dojFrom")?.value || "";
    const dojTo = document.getElementById("dojTo")?.value || "";
    if (dojFrom || dojTo) this.filters.push({ fldname: "doj", datatype: "DATE", from: formatDateString(dojFrom) || "01/01/1900", to: formatDateString(dojTo) || "31/12/2999" });

    const deptSelect = document.getElementById("deptFilter");
    const selectedDepts = deptSelect ? Array.from(deptSelect.selectedOptions).map(o => o.value) : [];
    if (selectedDepts.length > 0) this.filters.push({ fldname: "department", datatype: "DROPDOWN", value: selectedDepts });
  }

  clearFilters() {
    this.filters = [];
    this.forceClientFiltering = false;
    this._filteredCache = null;
    window._smartviewFullData = null;
    this.pageSize = 100;
    ["empCodeFilter","salaryFrom","salaryTo","dojFrom","dojTo"].forEach(id => { const el = document.getElementById(id); if (el) el.value = ""; });
    const dept = document.getElementById("deptFilter"); if (dept) dept.selectedIndex = -1;
  }

  resetPaging() {
    this.pageno = 1;
    this.isFetching = false;
    this.totalCount = 0;
    this.loadedCount = 0;
    this._pagingFallbackTried = false;
    this._userHasScrolled = false;
    this._lastPageReached = false;
    window._entity.listJson = [];
    window._entity.pageSize = this.pageSize;
    this._filteredCache = null;
    window._smartviewFullData = null;
  }

  buildParams(pageNo = 1) {
    const sqlParams = Object.assign({}, (this._entitySqlParams || {}), (this.props && this.props.sqlParams) ? this.props.sqlParams : {});
    const safeFilters = stripSmartviewFilterTransId(this.filters || []);
    const props = {
      ADS: false,
      CachePermissions: true,
      // SmartView paging works without requesting total count; keep this false to avoid extra overhead.
      getallrecordscount: false,
      pageno: pageNo,
      pagesize: this.pageSize,
      keyfield: "",
      keyvalue: "",
      sorting: this.sorting,
      filters: safeFilters
    };

    if (this.axClient_dateformat) props.axClient_dateformat = this.axClient_dateformat;
    if (Array.isArray(this.select_columns) && this.select_columns.length) props.select_columns = this.select_columns.slice();
    if (Array.isArray(this.groupby_columns) && this.groupby_columns.length) props.groupby_columns = this.groupby_columns.slice();
    // Do not pass aggregations; use groupby_columns with sum(...) expressions instead.

    return {
      adsNames: [this.adsName],
      refreshCache: this.refreshCache,
      sqlParams: sqlParams,
      props: props
    };
  }

  // Ensure ADS metadata is available for the currently selected ADS.
  // Preference order: in-memory cache -> localStorage -> server call.
  ensureAdsMetadata(callback) {
    try {
      const adsKey = (this.adsName || window._entity?.adsName || '').toString().trim();
      if (!adsKey) {
        if (typeof callback === 'function') callback(new Error('ensureAdsMetadata: no adsName'));
        return;
      }

      const metaFor = (this._adsMetaFor || '').toString();
      if (this.lastAdsMeta && Array.isArray(this.lastAdsMeta) && this.lastAdsMeta.length &&
          metaFor && metaFor.toLowerCase() === adsKey.toLowerCase()) {
        if (typeof callback === 'function') callback(null, this.lastAdsMeta);
        return;
      }

      const cached = loadSmartviewAdsMetaFromStorage(adsKey);
      if (cached && Array.isArray(cached) && cached.length) {
        this.lastAdsMeta = cached;
        this._adsMetaFor = adsKey;
        window._entity = window._entity || {};
        window._entity.metaData = cached;
        // If data is already rendered, re-render rows so hyperlinks/captions take effect.
        try {
          if (typeof createTableViewHTML === 'function' && Array.isArray(window._entity.listJson) && window._entity.listJson.length) {
            const pn = Math.max(1, (Number(this.pageno) || 1) - 1);
            createTableViewHTML(window._entity.listJson, pn);
          }
        } catch (e) {}
        if (typeof callback === 'function') callback(null, cached);
        return;
      }

      this.fetchAdsMetadata(callback);
    } catch (e) {
      if (typeof callback === 'function') callback(e);
    }
  }

  fetchAdsMetadata(callback) {
    const self = this;
    try {
      const adsKey = (this.adsName || window._entity?.adsName || '').toString().trim();
      const metaFor = (this._adsMetaFor || '').toString();
      if (this.lastAdsMeta && Array.isArray(this.lastAdsMeta) && this.lastAdsMeta.length &&
          metaFor && metaFor.toLowerCase() === String(adsKey).toLowerCase()) {
        if (typeof callback === 'function') callback(null, this.lastAdsMeta);
        return;
      }
  
      const params = {
        adsNames: ['ds_smartlist_ads_metadata'],
        refreshCache: false,
        sqlParams: { adsname: this.adsName || window._entity?.adsName },
        props: { ADS: false, CachePermissions: true, getallrecordscount: false, pageno: 1, pagesize: 500, sorting: [], filters: [] }
      };
  
      const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
                   : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
                   : null;
  
      if (!caller || typeof caller.GetDataFromAxList !== 'function') {
        const err = new Error('GetDataFromAxList not available for fetchAdsMetadata');
        console.error(err);
        if (typeof callback === 'function') callback(err);
        return;
      }
  
      caller.GetDataFromAxList(params, function(response) {
        try {
          let parsed = (typeof response === 'string') ? JSON.parse(response) : response;
          if (typeof safeParseAxResponse === 'function') parsed = safeParseAxResponse(parsed);
  
          // locate dsBlock similarly to other parsers
          let dsBlock = null;
          if (parsed?.result && Array.isArray(parsed.result.data) && parsed.result.data.length > 0) {
            dsBlock = parsed.result.data[0];
          } else if (Array.isArray(parsed?.data) && parsed.data.length > 0) {
            dsBlock = parsed.data[0];
          } else if (parsed && (parsed.adsname || parsed.data || parsed.columns)) {
            dsBlock = parsed;
          } else if (Array.isArray(parsed)) {
            dsBlock = parsed[0] || {};
          } else {
            dsBlock = parsed?.result || parsed || {};
          }
  
          let meta = [];
  
          // ---------- Case A: ADS returned metadata rows (common in your sample) ----------
          // detect when dsBlock.data is an array of objects that look like metadata (have fldname)
          const candidateMetaRows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
          if (candidateMetaRows && candidateMetaRows.length > 0 && candidateMetaRows[0] && (candidateMetaRows[0].fldname || candidateMetaRows[0].fieldname)) {
            meta = candidateMetaRows.map(r => {
              const fldnameRaw = (r.fldname || r.fieldname || r.name || '').toString();
              const fldname = fldnameRaw.trim();
              const fldcap = (r.fldcaption || r.fldcap || r.fldcaption || r.fldcaption || r.fldcaptionName || r.fldcap || r.fldcaption || r.caption || r.fldcaption || r.fldcaption || '').toString();
              const fdtRaw = (r.fdatatype || r.datatype || r.cdatatype || '').toString().toLowerCase();

              // SmartView ADS metadata commonly marks "normalized" fields (T) that should be dropdown filters.
              const normalizedRaw = (r.normalized ?? r.isnormalized ?? r.is_normalized ?? r.isNormalized ?? r.normalised ?? r.isnormalised);
              const isNormalized = (normalizedRaw === true) || (String(normalizedRaw || '').toUpperCase() === 'T');
  
              // Accept 'c','n','d','b' or longer words — keep original token so EntityFilter can use it.
              // If normalized, force a text-ish base type so Entity-Filter doesn't override dropdown as Numeric/Date.
              const fdatatype = isNormalized ? 'c' : (fdtRaw || 'c');
  
              // filters may be 'T'/'F' or true/false — normalize to 'T'/'F'
              let filtersFlag = r.filters;
              if (filtersFlag === true || String(filtersFlag).toLowerCase() === 'true' || String(filtersFlag).toUpperCase() === 'T') filtersFlag = 'T';
              else if (filtersFlag === false || String(filtersFlag).toLowerCase() === 'false' || String(filtersFlag).toUpperCase() === 'F') filtersFlag = 'F';
              else filtersFlag = (String(filtersFlag || '').toUpperCase() === 'T') ? 'T' : 'F';
  
              // include any dropdown options if backend provided them
              const options = Array.isArray(r.options) ? r.options : (r.options && typeof r.options === 'string' ? tryParseJsonSafe(r.options) : undefined);

              // Normalize cdatatype spelling/case for Entity-Filter.js switch cases.
              let cdatatype = r.cdatatype;
              if (typeof cdatatype === 'string') {
                const cd = cdatatype.trim().toLowerCase();
                if (cd === 'dropdown' || cd === 'drop down' || cd === 'drop_down' || cd === 'select') cdatatype = 'DropDown';
              }
              // If normalized, force dropdown filter (product metadata doesn't always mark it correctly).
              if (isNormalized) cdatatype = 'DropDown';
              else if (!cdatatype || String(cdatatype).trim() === '') cdatatype = undefined;

              // Build psrctxt for ds_smartlist_filters:
              //   fldname~normalized~source table~source fld
              const srcTable = (r.srctable || r.src_table || r.srctbl || r.sourcetable || r.source_table || r.srcTable || r.sourceTable || r.table || r.tablename || r.tblname || '').toString().trim();
              const srcFld = (r.srcfld || r.src_fld || r.srcfield || r.sourcefld || r.source_fld || r.sourcefield || r.srcField || r.sourceField || r.column || r.colname || r.columnname || '').toString().trim();
              const normalizedToken = isNormalized ? 'T' : 'F';

              let psrctxt = (r.psrctxt || r.psrctext || r.psrcTxt || '').toString().trim();
              if (!psrctxt && fldname && srcTable && srcFld) {
                psrctxt = `${fldname}~${normalizedToken}~${srcTable}~${srcFld}`;
              }

              // Keep a usable transid for compatibility (product code uses it for dropdown lookups).
              const ftransid = (r.ftransid || r.fTransId || r.transid || r.tstid || r.entityTransId || self.adsName || window._entity?.entityTransId || '') || '';

              // Table hyperlink metadata (optional)
              const sqlname = (r.sqlname || r.sqlName || r.adsname || r.adsName || self.adsName || '').toString().trim();
              const hypStructType = (r.hyp_structtype ?? r.hypStructType ?? r.hyp_struct_type ?? '') || '';
              const hypTransId = (r.hyp_transid ?? r.hypTransId ?? r.hyp_transId ?? '') || '';
              const tblHyperlink = (r.tbl_hyperlink ?? r.tblHyperlink ?? r.tbl_hyperLink ?? '') || '';
              const hypStructTypeStr = (hypStructType === null || hypStructType === undefined) ? '' : String(hypStructType).trim();
              const hypTransIdStr = (hypTransId === null || hypTransId === undefined) ? '' : String(hypTransId).trim();
              const tblHyperlinkStr = (tblHyperlink === null || tblHyperlink === undefined) ? '' : String(tblHyperlink).trim();
  
              return {
                fldname: fldname,
                fldcap: fldcap || formatFieldName(fldname),
                fdatatype: fdatatype,   // keep the short code as provided ('c','n','d' etc)
                cdatatype: cdatatype || undefined,
                ftransid: ftransid,
                listingfld: (r.listingfld === 'T' || r.listingfld === true) ? 'T' : (r.listingfld || 'T'),
                filters: filtersFlag,
                options: options,
                normalized: isNormalized ? 'T' : 'F',
                srctable: srcTable,
                srcfld: srcFld,
                psrctxt: psrctxt,
                sqlname: sqlname || null,
                hyp_structtype: hypStructTypeStr || null,
                hyp_transid: hypTransIdStr || null,
                tbl_hyperlink: tblHyperlinkStr || null
              };
            }).filter(m => m.fldname);
          }
          // ---------- Case B: dsBlock.columns present (existing behavior) ----------
          else if (dsBlock && Array.isArray(dsBlock.columns) && dsBlock.columns.length) {
            meta = dsBlock.columns.map(col => {
              const normalizedRaw = (col.normalized ?? col.isnormalized ?? col.is_normalized ?? col.isNormalized ?? col.normalised ?? col.isnormalised);
              const isNormalized = (normalizedRaw === true) || (String(normalizedRaw || '').toUpperCase() === 'T');

              const fldname = (col.key || col.name || '').toString();
              const fldcap = col.caption || formatFieldName(fldname);

              const srcTable = (col.srctable || col.src_table || col.srctbl || col.sourcetable || col.source_table || col.srcTable || col.sourceTable || col.table || col.tablename || col.tblname || '').toString().trim();
              const srcFld = (col.srcfld || col.src_fld || col.srcfield || col.sourcefld || col.source_fld || col.sourcefield || col.srcField || col.sourceField || col.column || col.colname || col.columnname || '').toString().trim();
              const normalizedToken = isNormalized ? 'T' : 'F';
              let psrctxt = (col.psrctxt || col.psrctext || col.psrcTxt || '').toString().trim();
              if (!psrctxt && fldname && srcTable && srcFld) {
                psrctxt = `${fldname}~${normalizedToken}~${srcTable}~${srcFld}`;
              }

              const sqlname = (col.sqlname || col.sqlName || col.adsname || col.adsName || self.adsName || '').toString().trim();
              const hypStructType = (col.hyp_structtype ?? col.hypStructType ?? col.hyp_struct_type ?? '') || '';
              const hypTransId = (col.hyp_transid ?? col.hypTransId ?? col.hyp_transId ?? '') || '';
              const tblHyperlink = (col.tbl_hyperlink ?? col.tblHyperlink ?? col.tbl_hyperLink ?? '') || '';
              const hypStructTypeStr = (hypStructType === null || hypStructType === undefined) ? '' : String(hypStructType).trim();
              const hypTransIdStr = (hypTransId === null || hypTransId === undefined) ? '' : String(hypTransId).trim();
              const tblHyperlinkStr = (tblHyperlink === null || tblHyperlink === undefined) ? '' : String(tblHyperlink).trim();

              return {
                fldname: fldname,
                fldcap: fldcap,
                fdatatype: isNormalized ? 'c' : (col.datatype || 't'),
                cdatatype: isNormalized ? 'DropDown' : inferColumnType(col),
                ftransid: (col.ftransid || col.fTransId || col.transid || self.adsName || window._entity?.entityTransId || '') || '',
                listingfld: "T",
                filters: (col.filters === 'T' || col.filters === true) ? 'T' : 'F',
                normalized: isNormalized ? 'T' : 'F',
                srctable: srcTable,
                srcfld: srcFld,
                psrctxt: psrctxt,
                sqlname: sqlname || null,
                hyp_structtype: hypStructTypeStr || null,
                hyp_transid: hypTransIdStr || null,
                tbl_hyperlink: tblHyperlinkStr || null
              };
            }).filter(m => m.fldname);
          }
          // ---------- Case C: fallback — infer from first data row ----------
          else {
            const rows = Array.isArray(dsBlock.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
            if (rows && rows.length > 0) {
              const sample = rows[0];
              const keys = Object.keys(sample || {}).map(k => k.toString());
              const preferredOrder = ['transid', 'recordid', 'processname', 'taskname', 'formcaption', 'keyfieldcaption', 'username', 'email', 'nickname'];
              const tmp = [];
              preferredOrder.forEach(k => {
                if (keys.includes(k) && !tmp.some(m => m.fldname.toLowerCase() === k.toLowerCase())) {
                  tmp.push({ fldname: k, fldcap: formatFieldName(k), fdatatype: 't', cdatatype: 'Text', listingfld: 'T', filters: 'F' });
                }
              });
              keys.forEach(k => {
                if (!tmp.some(m => m.fldname.toLowerCase() === k.toLowerCase())) {
                  tmp.push({ fldname: k, fldcap: formatFieldName(k), fdatatype: 't', cdatatype: 'Text', listingfld: 'T', filters: 'F' });
                }
              });
              meta = tmp;
            } else {
              meta = [];
            }
          }
  
          // Normalize fldname lowercase, trim spaces and remove duplicates while preserving order
          const seen = new Set();
          const normalized = [];
          meta.forEach(m => {
            if (!m || !m.fldname) return;
            const key = m.fldname.toString().trim();
            if (seen.has(key.toLowerCase())) return;
            seen.add(key.toLowerCase());
            m.fldname = key;
            normalized.push(m);
          });
  
          // If metadata contains no explicit filter flags 'T', provide a safe fallback:
          // mark fields with fdatatype present as filterable so the filter UI isn't empty.
          const hasExplicitFilters = normalized.some(x => (String(x.filters || '').toUpperCase() === 'T'));
          if (!hasExplicitFilters && normalized.length) {
            // pick reasonable subset — all non-hidden text/number/date fields
            normalized.forEach(n => {
              const ft = (n.fdatatype || '').toString().toLowerCase();
              if (ft === 'c' || ft === 't' || ft === 'string' || ft === 'n' || ft === 'd' || !n.filters) {
                n.filters = 'T';
              } else {
                n.filters = n.filters || 'F';
              }
            });
          }
  
          window._entity = window._entity || {};
          window._entity.metaData = normalized;
          self.lastAdsMeta = normalized;
          try {
            const storeKey = (params && params.sqlParams && params.sqlParams.adsname) ? params.sqlParams.adsname : (self.adsName || adsKey);
            self._adsMetaFor = storeKey;
            saveSmartviewAdsMetaToStorage(storeKey, normalized);
          } catch (e) {}

          // If data is already on screen, re-render rows so hyperlinks/captions take effect.
          try {
            if (typeof createTableViewHTML === 'function' && Array.isArray(window._entity.listJson) && window._entity.listJson.length) {
              const pn = Math.max(1, (Number(self.pageno) || 1) - 1);
              createTableViewHTML(window._entity.listJson, pn);
            }
          } catch (e) {}
  
          if (typeof callback === 'function') callback(null, self.lastAdsMeta);
        } catch (e) {
          console.error('fetchAdsMetadata parse/assign error', e);
          if (typeof callback === 'function') callback(e);
        }
      }, function(err) {
        console.error('fetchAdsMetadata GetDataFromAxList error', err);
        if (typeof callback === 'function') callback(err);
      });
    } catch (ex) {
      console.error('fetchAdsMetadata unexpected error', ex);
      if (typeof callback === 'function') callback(ex);
    }
  
    // small helper
    function tryParseJsonSafe(s) {
      try {
        if (!s) return undefined;
        return (typeof s === 'object') ? s : JSON.parse(s);
      } catch (err) { return undefined; }
    }
  }
  

 loadNextPage() {
  // Block until ads selected (if your controller uses that)
  if (this.requiresAdsSelection && !this.adsSelected) return;

  // Do not run in parallel
  if (this.isFetching) {
    console.log('loadNextPage: aborted because already fetching');
    return;
  }

  // Stop when we already detected that there are no more rows (prevents repeated calls on empty results).
  if (this._lastPageReached) {
    console.log('loadNextPage: aborted because last page already reached');
    return;
  }

  // "Load all" mode: fetch all rows once (including filters) and render.
  if (this.pageSize === 0) {
    try { this.loadAllOnce(); } catch (e) { console.error('loadNextPage: loadAllOnce failed', e); }
    return;
  }

  // If known total and already loaded, stop
  if (this.totalCount > 0 && this.loadedCount >= this.totalCount) {
    console.log('loadNextPage: all records loaded (loadedCount, totalCount)=', this.loadedCount, this.totalCount);
    return;
  }

  // Client-side filtering fallback: fetch all once, then page filtered results
  if (this.forceClientFiltering && Array.isArray(this.filters) && this.filters.length) {
    const requestPage = this.pageno;
    const serveFilteredPage = () => {
      const pageSize = this.pageSize;
      const cache = Array.isArray(this._filteredCache) ? this._filteredCache : [];
      if (pageSize === 0) {
        window._entity.listJson = cache.slice();
      } else {
        const start = (requestPage - 1) * pageSize;
        const slice = cache.slice(start, start + pageSize);
        if (!slice.length) {
          console.log('loadNextPage: no more filtered rows to append');
          this._lastPageReached = true;
          try { rowCountManager && rowCountManager.setLastPageReached && rowCountManager.setLastPageReached(true); } catch (e) {}
          return;
        }
        if (requestPage === 1) {
          window._entity.listJson = slice;
        } else {
          window._entity.listJson = (window._entity.listJson || []).concat(slice);
        }
      }

      this.totalCount = cache.length;
      this.loadedCount = window._entity.listJson.length;
      if (rowCountManager && typeof rowCountManager.setTotal === "function") {
        rowCountManager.setTotal(this.totalCount);
        rowCountManager.setLoadedRecords(this.loadedCount);
      }
      if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, requestPage);
      this.pageno = requestPage + 1;
    };

    if (Array.isArray(this._filteredCache)) {
      serveFilteredPage();
      return;
    }

    if (Array.isArray(window._smartviewFullData) && window._smartviewFullData.length > 0) {
      this._filteredCache = applySmartviewFiltersToRows(window._smartviewFullData, this.filters);
      serveFilteredPage();
      return;
    }

    // Fetch all rows once (pagesize=0) then filter client-side
    const params = this.buildParams(1);
    params.props = params.props || {};
    params.props.pagesize = 0;
    params.props.pageno = 1;

    console.log('loadNextPage: client-filter mode -> fetching full dataset', params);

    this.isFetching = true;
    const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
                 : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
                 : null;
    if (!caller || typeof caller.GetDataFromAxList !== 'function') {
      console.error('GetDataFromAxList not available');
      this.isFetching = false;
      return;
    }

    caller.GetDataFromAxList(params, (response) => {
      try {
        const parsed = safeParseAxResponse(response);
        const dsBlock = parsed?.result?.data?.[0] || parsed?.data?.[0] || parsed?.result || parsed;
        const rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
        window._smartviewFullData = rows.slice();
        this._filteredCache = applySmartviewFiltersToRows(rows, this.filters);
        serveFilteredPage();
      } catch (e) {
        console.error('loadNextPage: client-filter fetch error', e);
      } finally {
        this.isFetching = false;
      }
    }, (err) => {
      console.error('loadNextPage: client-filter GetDataFromAxList error', err);
      this.isFetching = false;
    });

    return;
  }

  // If we already cached all rows client-side, serve next slice
  if (Array.isArray(window._smartviewFullData) && window._smartviewFullData.length > 0) {
    const start = (this.pageno - 1) * this.pageSize;
    const slice = window._smartviewFullData.slice(start, start + this.pageSize);
    if (!slice.length) {
      console.log('loadNextPage: no more cached rows to append');
      this._lastPageReached = true;
      return;
    }
    window._entity.listJson = (window._entity.listJson || []).concat(slice);
    this.loadedCount = window._entity.listJson.length;
    console.log('loadNextPage: appended client-cached slice, new loadedCount=', this.loadedCount);
    if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, this.pageno);
    if (this.pageSize > 0 && (start + slice.length) >= window._smartviewFullData.length) this._lastPageReached = true;
    this.pageno++;
    return;
  }

  // Make server request for page this.pageno
  const requestPage = this.pageno;
  const params = this.buildParams(requestPage);

  console.log('loadNextPage: requesting page', requestPage, 'pagesize', this.pageSize, params);

  this.isFetching = true;

  // pick caller safely
  const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
               : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
               : null;

  if (!caller || typeof caller.GetDataFromAxList !== 'function') {
    console.error('GetDataFromAxList not available');
    this.isFetching = false;
    return;
  }

  caller.GetDataFromAxList(params, (response) => {
    try {
      const parsed = (typeof safeParseAxResponse === 'function')
        ? safeParseAxResponse(response)
        : ((typeof response === 'string') ? JSON.parse(response) : response);

      const dsBlock =
        (parsed?.result && Array.isArray(parsed.result.data) && parsed.result.data.length > 0) ? parsed.result.data[0]
        : (Array.isArray(parsed?.data) && parsed.data.length > 0) ? parsed.data[0]
        : (parsed?.result || parsed || {});

      const rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
      const totalFromServer = Number(dsBlock?.totalrecords ?? dsBlock?.recordcount ?? parsed?.result?.totalrecords ?? 0) || 0;

      console.log('loadNextPage: server returned rows=', rows.length, 'totalFromServer=', totalFromServer, 'for requested page', requestPage);

      // If page > 1 returned no rows, fall back to "fetch all once and slice" (some ADS ignore pageno).
      if (requestPage > 1 && (!rows || rows.length === 0) && !this._pagingFallbackTried && this.pageSize > 0) {
        this._pagingFallbackTried = true;
        const rp = requestPage;
        setTimeout(() => {
          try { this._fallbackFetchAllAndAppend(rp); } catch (e) {}
        }, 0);
        return;
      }

      // If server returned *pages* (rows.length <= pagesize) treat it as server-paged
      if (rows.length > 0 && rows.length <= this.pageSize) {
        // server is doing paging: append server page
        if (requestPage === 1) {
          window._entity.listJson = rows.slice();
        } else {
          window._entity.listJson = (window._entity.listJson || []).concat(rows);
        }
        this.loadedCount = window._entity.listJson.length;

        // Treat totalFromServer as authoritative only if it looks like a real total (i.e., > current page size).
        // Some ADS return totalrecords == pagesize, which would incorrectly stop paging at 100.
        if (totalFromServer > rows.length) {
          this.totalCount = Math.max(this.totalCount || 0, totalFromServer);
        } else if (this.pageSize > 0 && rows.length < this.pageSize) {
          // last page detected when fewer rows than requested returned
          this.totalCount = this.loadedCount;
          try { rowCountManager && rowCountManager.setLastPageReached && rowCountManager.setLastPageReached(true); } catch (e) {}
          this._lastPageReached = true;
        }

        try {
          if (rowCountManager && typeof rowCountManager.setLoadedRecords === "function") rowCountManager.setLoadedRecords(this.loadedCount);
          if (rowCountManager && typeof rowCountManager.setTotal === "function") rowCountManager.setTotal(this.totalCount || this.loadedCount);
        } catch (e) {}

        if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, requestPage);
        // increment page for next time (after success)
        this.pageno = requestPage + 1;
        console.log('loadNextPage: server-paged append complete, pageno set to', this.pageno);
        // If we've now loaded all known total, stop further requests
        if (this.totalCount > 0 && this.loadedCount >= this.totalCount) {
          console.log('loadNextPage: reached totalCount, stopping further loads');
          this._lastPageReached = true;
        }
        return;
      }

      // Otherwise server returned many rows (likely full dataset) – cache and slice client-side
      if (rows.length > 0) {
        window._smartviewFullData = rows.slice(); // cache full set
        // Render only first page (if initial request) else append appropriate slice
        if (requestPage === 1) {
          window._entity.listJson = rows.slice(0, this.pageSize);
        } else {
          const start = (requestPage - 1) * this.pageSize;
          window._entity.listJson = (window._entity.listJson || []).concat(rows.slice(start, start + this.pageSize));
        }
        this.totalCount = rows.length;
        this.loadedCount = window._entity.listJson.length;
        if (this.pageSize > 0 && this.loadedCount >= this.totalCount) this._lastPageReached = true;
        if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, requestPage);
        // next page will be served from cached data
        this.pageno = requestPage + 1;
        console.log('loadNextPage: cached-full fallback used, cached rows=', rows.length, 'pageno now', this.pageno);
        return;
      }

      // If no rows returned, probably no data — stop
      if (requestPage === 1) {
        // First page is empty (common when filters return 0 results). Clear UI.
        window._entity.listJson = [];
        this.loadedCount = 0;
        this.totalCount = totalFromServer || 0;
        try {
          if (rowCountManager && typeof rowCountManager.setTotal === 'function') rowCountManager.setTotal(this.totalCount);
          if (rowCountManager && typeof rowCountManager.setLoadedRecords === 'function') rowCountManager.setLoadedRecords(0);
          rowCountManager && rowCountManager.setLastPageReached && rowCountManager.setLastPageReached(true);
        } catch (e) {}
        try { if (typeof createTableViewHTML === 'function') createTableViewHTML([], 1); } catch (e) {}
      } else {
        if (this.totalCount === 0) this.totalCount = this.loadedCount;
        try { rowCountManager && rowCountManager.setLastPageReached && rowCountManager.setLastPageReached(true); } catch (e) {}
      }

      this._lastPageReached = true;
      console.log('loadNextPage: server returned no rows for page', requestPage);
    } catch (e) {
      console.error('loadNextPage: error processing response', e);
    } finally {
      this.isFetching = false;
    }
  }, (err) => {
    console.error('loadNextPage: GetDataFromAxList error', err);
    this.isFetching = false;
  });
}

  _fallbackFetchAllAndAppend(requestPage) {
    try {
      if (this.isFetching) return;
      if (!this.pageSize || this.pageSize === 0) return;

      const params = this.buildParams(1);
      params.props = params.props || {};
      params.props.pageno = 1;
      params.props.pagesize = 0; // fetch all rows then slice locally

      console.warn('loadNextPage: paging fallback -> fetching all rows once', params);

      // pick caller safely
      const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
                   : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
                   : null;

      if (!caller || typeof caller.GetDataFromAxList !== 'function') {
        console.error('GetDataFromAxList not available (paging fallback)');
        return;
      }

      this.isFetching = true;

      caller.GetDataFromAxList(params, (response) => {
        try {
          const parsed = (typeof safeParseAxResponse === 'function')
            ? safeParseAxResponse(response)
            : ((typeof response === 'string') ? JSON.parse(response) : response);

          const dsBlock =
            (parsed?.result && Array.isArray(parsed.result.data) && parsed.result.data.length > 0) ? parsed.result.data[0]
            : (Array.isArray(parsed?.data) && parsed.data.length > 0) ? parsed.data[0]
            : (parsed?.result || parsed || {});

          const allRows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
          window._smartviewFullData = allRows.slice();

          const start = (requestPage - 1) * this.pageSize;
          const slice = allRows.slice(start, start + this.pageSize);

          if (!slice.length) {
            if (this.totalCount === 0) this.totalCount = this.loadedCount;
            try { rowCountManager && rowCountManager.setLastPageReached && rowCountManager.setLastPageReached(true); } catch (e) {}
            this._lastPageReached = true;
            console.log('paging fallback: no more rows to append for page', requestPage);
            return;
          }

          window._entity.listJson = (window._entity.listJson || []).concat(slice);
          this.loadedCount = window._entity.listJson.length;
          this.totalCount = allRows.length;
          if (this.pageSize > 0 && this.loadedCount >= this.totalCount) this._lastPageReached = true;

          try {
            if (rowCountManager && typeof rowCountManager.setTotal === 'function') rowCountManager.setTotal(this.totalCount);
            if (rowCountManager && typeof rowCountManager.setLoadedRecords === 'function') rowCountManager.setLoadedRecords(this.loadedCount);
          } catch (e) {}

          if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, requestPage);
          this.pageno = requestPage + 1;

        } catch (e) {
          console.error('paging fallback: error processing response', e);
        } finally {
          this.isFetching = false;
        }
      }, (err) => {
        console.error('paging fallback: GetDataFromAxList error', err);
        this.isFetching = false;
      });
    } catch (e) {
      console.error('paging fallback: unexpected error', e);
      this.isFetching = false;
    }
  }

  loadAllOnce() {
    const params = this.buildParams(1);
    params.props.pagesize = 0;
    this.isFetching = true;
    try {
      const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent : window;
      caller.GetDataFromAxList(
        params,
(response) => {
  try {
    const parsed = safeParseAxResponse(response);
    // normalize will unwrap, infer metaData and call createTableViewHTML
    normalizeAndRenderFromDsResponse(parsed, 1, 0);

    // ensure controller state is set
    const dsBlock = parsed?.result?.data?.[0] || parsed?.data?.[0] || parsed?.result || parsed;
    const rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
    this.loadedCount = rows.length;
    this.totalCount = Number(dsBlock?.totalrecords ?? dsBlock?.recordcount ?? rows.length) || rows.length;

    if (rowCountManager && typeof rowCountManager.setTotal === "function") rowCountManager.setTotal(this.totalCount);
  } catch (e) {
    console.error("GetDataFromAxList error:", e);
  } finally {
    this.isFetching = false;
  }
},
        (err) => {
          console.error("GetDataFromAxList error:", err);
          this.isFetching = false;
        }
      );
    } catch (ex) {
      console.error("Exception calling GetDataFromAxList:", ex);
      this.isFetching = false;
    }
  }

attachScrollListener() {
  const container = document.getElementById("table-body_Container");
  if (!container) return console.warn('attachScrollListener: no container found');

  const responsive = container.querySelector('.table-responsive');
  const sentinel = ensureSmartviewScrollSentinel();

  // remove any previous handler
  if (this._scrollHandler && this._scrollTarget) {
    try { this._scrollTarget.removeEventListener('scroll', this._scrollHandler); } catch (e) {}
  }
  this._scrollTarget = null;

  // remove any previous intersection observer
  try { if (this._scrollObserver) this._scrollObserver.disconnect(); } catch (e) {}
  this._scrollObserver = null;

  function hasVerticalScroll(el) {
    if (!el || el === window) return false;
    try {
      const style = window.getComputedStyle(el);
      const oy = (style.overflowY || '').toLowerCase();
      return (oy === 'auto' || oy === 'scroll' || oy === 'overlay');
    } catch (e) {
      return false;
    }
  }

  function findScrollableAncestor(el) {
    let cur = el;
    while (cur && cur !== document.body && cur !== document.documentElement) {
      if (hasVerticalScroll(cur)) return cur;
      cur = cur.parentElement;
    }
    return null;
  }

  const scrollEl =
    (responsive && hasVerticalScroll(responsive)) ? responsive
    : (hasVerticalScroll(container) ? container
    : (findScrollableAncestor(container) || window));

  // debounce guard
  let debounceTimer = null;
  const thresholdPx = 150;

  const isNearBottom = () => {
    try {
      if (scrollEl === window) {
        const scrolled = window.innerHeight + window.scrollY;
        const full = document.documentElement.scrollHeight;
        return (full - scrolled) <= thresholdPx;
      }
      // do not auto-load if there's nothing to scroll yet (user asked to load only on scroll-to-bottom)
      if (scrollEl.scrollHeight <= scrollEl.clientHeight + 2) return false;
      return (scrollEl.scrollHeight - (scrollEl.scrollTop + scrollEl.clientHeight)) <= thresholdPx;
    } catch (e) {
      return false;
    }
  };

  this._scrollHandler = () => {
    // IMPORTANT: only load more based on user scroll interaction (no auto prefetch on attach)
    this._userHasScrolled = true;

    if (debounceTimer) clearTimeout(debounceTimer);
    debounceTimer = setTimeout(() => {
      try {
        if (this.isFetching) return;
        if (this.pageSize === 0) return; // loadAll mode

        if (isNearBottom()) {
          console.log('[scroll] nearBottom detected -> loadNextPage (pageno=', this.pageno, ')');
          this.loadNextPage();
        }
      } catch (e) {
        console.error('scroll handler error', e);
      }
    }, 120);
  };

  this._scrollTarget = scrollEl;
  this._scrollTarget.addEventListener('scroll', this._scrollHandler, { passive: true });

  // IntersectionObserver (supports mousewheel/touchpad scroll inside nested containers), but only after user scrolls.
  try {
    if (sentinel && 'IntersectionObserver' in window) {
      this._scrollObserver = new IntersectionObserver((entries) => {
        try {
          if (!this._userHasScrolled) return; // avoid auto-loading on initial render
          if (this.isFetching) return;
          if (this.pageSize === 0) return;
          for (const ent of entries) {
            if (ent && ent.isIntersecting) {
              console.log('[io] sentinel intersect -> loadNextPage (pageno=', this.pageno, ')');
              this.loadNextPage();
              break;
            }
          }
        } catch (e) {}
      }, {
        root: (scrollEl === window ? null : scrollEl),
        rootMargin: `0px 0px ${thresholdPx}px 0px`,
        threshold: 0.01
      });
      this._scrollObserver.observe(sentinel);
    }
  } catch (e) {}

  console.log('attachScrollListener attached to', scrollEl === window ? 'window' : (scrollEl.id || scrollEl.className || scrollEl.tagName));
}


  applyHeaderSort(fieldName, sortOrder) {
    const field = (fieldName || '').toString().trim();
    if (!field) return;
    const order = (String(sortOrder || '').toLowerCase() === 'desc') ? 'desc' : 'asc';
    this.sorting = [{ fldname: field, sort_order: order }];
    this.resetPaging();
    this.loadNextPage();
  }

  toggleGroupByField(fieldName) {
    const field = (fieldName || '').toString().trim();
    if (!field) return;

    const applyGroup = (meta) => {
      const base = smartviewNormalizeGroupbyFields(this.groupby_columns);
      const idx = base.findIndex(f => String(f).toLowerCase() === field.toLowerCase());
      if (idx >= 0) base.splice(idx, 1);
      else base.push(field);

      if (base.length > 0) {
        const gb = smartviewBuildGroupbyWithSums(meta || [], base);
        this.groupby_columns = gb.groupby_columns;
        this.select_columns = gb.select_columns;
        this.aggregations = {};
      } else {
        this.groupby_columns = [];
        this.select_columns = [];
        this.aggregations = {};
      }

      this.resetPaging();
      this.loadNextPage();
    };

    const meta = (Array.isArray(this.lastAdsMeta) && this.lastAdsMeta.length)
      ? this.lastAdsMeta
      : (window._entity && Array.isArray(window._entity.metaData) ? window._entity.metaData : []);

    if (meta && meta.length) {
      applyGroup(meta);
    } else if (typeof this.ensureAdsMetadata === 'function') {
      this.ensureAdsMetadata((err, m) => applyGroup(m || []));
    } else {
      applyGroup([]);
    }
  }

  clearGroupByColumns() {
    this.groupby_columns = [];
    this.select_columns = [];
    this.aggregations = {};
    this.resetPaging();
    this.loadNextPage();
  }


  setupSortingHeaders() {
    document.querySelectorAll("#employeeTable thead th.sortable").forEach(th => {
      th.addEventListener("click", () => {
        const field = th.getAttribute("data-field");
        let current = this.sorting.find(s => s.fldname === field);
        if (current) current.sort_order = current.sort_order === "asc" ? "desc" : "asc";
        else this.sorting = [{ fldname: field, sort_order: "asc" }];

        document.querySelectorAll("#employeeTable thead th.sortable").forEach(h => h.classList.remove("asc", "desc"));
        th.classList.add(this.sorting[0].sort_order === "desc" ? "desc" : "asc");

        this.resetPaging();
        this.loadNextPage();
      });
    });
  }
}





document.addEventListener('DOMContentLoaded', function () {
  const adsFromQuery = getQueryParam('ads') || getQueryParam('adsName') || getQueryParam('adsname');
  const groupByRaw = getQueryParam('groupby') || getQueryParam('groupBy') || getQueryParam('groupby_columns');
  const initialPayload = (typeof smartviewGetInitialFilterPayloadFromQuery === 'function')
    ? smartviewGetInitialFilterPayloadFromQuery()
    : null;

  const initialFiltersRaw = (initialPayload && Array.isArray(initialPayload.filters)) ? initialPayload.filters : [];
  const adsName = adsFromQuery || (initialPayload && (initialPayload.ads || initialPayload.adsName || initialPayload.adsname)) || null;

  function setHeaderTitle(name) {
    try {
      window._entity = window._entity || {};
      window._entity.adsName = name;
      const titleEl = document.getElementById('EntityTitle') || document.querySelector('.page-header-title');
      if (titleEl) titleEl.textContent = name;
      document.title = name || document.title;
    } catch (e) {}
  }

  function parseGroupByList(raw) {
    if (!raw) return [];
    if (Array.isArray(raw)) return raw.map(x => String(x || '').trim()).filter(Boolean);
    return String(raw)
      .split(',')
      .map(s => String(s || '').trim())
      .filter(Boolean);
  }

  function applyGroupByFromQuery(ctrl, meta) {
    if (!groupByRaw || !ctrl) return;
    const rawList = parseGroupByList(groupByRaw);
    if (!rawList.length) return;
    const resolved = [];
    rawList.forEach(rf => {
      const res = (typeof smartviewResolveFilterField === 'function')
        ? smartviewResolveFilterField(rf, meta || [])
        : { fldname: String(rf || '').trim(), meta: null };
      const fld = (res.fldname || '').toString().trim();
      if (fld) resolved.push(fld);
    });
    if (!resolved.length) return;

    const gb = smartviewBuildGroupbyWithSums(meta || [], Array.from(new Set(resolved)));
    ctrl.groupby_columns = gb.groupby_columns;
    ctrl.select_columns = gb.select_columns;
    ctrl.aggregations = {};
  }

  function startWithInitialFilters(name, rawFilters) {
    try {
      if (!name) return false;
      setHeaderTitle(name);

      // Create controller but defer data fetch until we map query filters using ADS metadata.
      if (!window.smartTableController) {
        window.smartTableController = new SmartViewTableController({
          adsName: name,
          pageSize: 100,
          currentPage: 1,
          sorting: [],
          filters: [],
          deferInitialLoad: true
        });
      } else {
        const ctrl = window.smartTableController;
        const prevAds = (ctrl.adsName || '').toString();
        ctrl.adsName = name;
        ctrl.deferInitialLoad = true;
        if (!prevAds || prevAds.toLowerCase() !== name.toLowerCase()) {
          ctrl.lastAdsMeta = null;
          ctrl._adsMetaFor = null;
        }
        try { ctrl.resetPaging(); } catch (e) {}
      }

      const ctrl = window.smartTableController;

      const applyMapped = function (meta) {
        try {
          const mapped = (typeof smartviewMapExternalFiltersToEntityFilters === 'function')
            ? smartviewMapExternalFiltersToEntityFilters(rawFilters || [], meta || [])
            : [];

          ctrl.filters = mapped;
          applyGroupByFromQuery(ctrl, meta || []);
          ctrl.deferInitialLoad = false;
          ctrl.forceClientFiltering = false;
          ctrl._filteredCache = null;
          window._smartviewFullData = null;

          try { if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging(); } catch (e) {}
          try { if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage(); } catch (e) {}

          // Create a pill that represents the decoded query filter(s)
          try { smartviewCreateInitialFilterPill(mapped, meta || [], name); } catch (e) {}
        } catch (e) {
          console.warn('startWithInitialFilters: applyMapped failed', e);
          try { ctrl.deferInitialLoad = false; ctrl.resetPaging(); ctrl.loadNextPage(); } catch (ex) {}
        }
      };

      if (ctrl && typeof ctrl.ensureAdsMetadata === 'function') {
        ctrl.ensureAdsMetadata(function (err, meta) {
          applyMapped(meta || (window._entity && window._entity.metaData) || []);
        });
      } else {
        applyMapped((window._entity && window._entity.metaData) || []);
      }

      return true;
    } catch (e) {
      console.warn('startWithInitialFilters failed', e);
      return false;
    }
  }

  function startWithInitialGroupBy(name) {
    try {
      if (!name) return false;
      setHeaderTitle(name);

      if (!window.smartTableController) {
        window.smartTableController = new SmartViewTableController({
          adsName: name,
          pageSize: 100,
          currentPage: 1,
          sorting: [],
          filters: [],
          deferInitialLoad: true
        });
      } else {
        const ctrl = window.smartTableController;
        const prevAds = (ctrl.adsName || '').toString();
        ctrl.adsName = name;
        ctrl.deferInitialLoad = true;
        if (!prevAds || prevAds.toLowerCase() !== name.toLowerCase()) {
          ctrl.lastAdsMeta = null;
          ctrl._adsMetaFor = null;
        }
        try { ctrl.resetPaging(); } catch (e) {}
      }

      const ctrl = window.smartTableController;
      const applyAndLoad = function (meta) {
        try {
          applyGroupByFromQuery(ctrl, meta || []);
          ctrl.deferInitialLoad = false;
          ctrl.forceClientFiltering = false;
          ctrl._filteredCache = null;
          window._smartviewFullData = null;
          try { if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging(); } catch (e) {}
          try { if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage(); } catch (e) {}
        } catch (e) {
          console.warn('startWithInitialGroupBy apply failed', e);
          try { ctrl.deferInitialLoad = false; ctrl.resetPaging(); ctrl.loadNextPage(); } catch (ex) {}
        }
      };

      if (ctrl && typeof ctrl.ensureAdsMetadata === 'function') {
        ctrl.ensureAdsMetadata(function (err, meta) { applyAndLoad(meta || (window._entity && window._entity.metaData) || []); });
      } else {
        applyAndLoad((window._entity && window._entity.metaData) || []);
      }
      return true;
    } catch (e) {
      console.warn('startWithInitialGroupBy failed', e);
      return false;
    }
  }

  if (adsName) {
    if (initialFiltersRaw && initialFiltersRaw.length) {
      const started = startWithInitialFilters(adsName, initialFiltersRaw);
      if (!started) {
        setTimeout(() => { try { showAdsPickerModal(); } catch (e) {} }, 250);
      }
    } else if (groupByRaw) {
      const started = startWithInitialGroupBy(adsName);
      if (!started) {
        setTimeout(() => { try { showAdsPickerModal(); } catch (e) {} }, 250);
      }
    } else {
      const started = startSmartTableFromAdsName(adsName);
      if (!started) {
        setTimeout(() => { try { showAdsPickerModal(); } catch (e) {} }, 250);
      }
    }
  } else {
    setTimeout(() => {
      try { showAdsPickerModal(); } catch (e) {}
    }, 300);
  }

  console.log('SmartViewTableController boot logic executed (ads=', adsName, ', initialFilters=', initialFiltersRaw.length, ')');
});
