  // new me sid
  window._multiTstructStore = window._multiTstructStore || {};
  window._multiTstructQueueArray = window._multiTstructQueueArray || [];
  window._smartviewFullData = null;
  window._smartviewExcelUploadState = window._smartviewExcelUploadState || null;
  window._smartviewBulkSaveEnabled = (typeof window._smartviewBulkSaveEnabled === "undefined")
    ? true
    : !!window._smartviewBulkSaveEnabled;
  window._smartviewUploadDirty = !!window._smartviewUploadDirty;
  window._smartviewCommittedFullData = window._smartviewCommittedFullData || null;
  window._smartviewCommittedExcelUploadState = window._smartviewCommittedExcelUploadState || null;
  window._smartviewNavigationGuardBypass = !!window._smartviewNavigationGuardBypass;
  window._smartviewNavigationPromptOpen = !!window._smartviewNavigationPromptOpen;
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
    keyFields: [],
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
  // Date presets used by Entity-Filter.js (Today/Yesterday/This week/Ã¢â‚¬Â¦)
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
    const fmt = "DD/MM/YYYY";
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
      // const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent : window;
      
    
      const scopes = [parent, window, window.top];

      const caller = scopes.find(
          w => w && typeof w.GetDataFromAxList === 'function'
      );
      
      
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
        <button id="closeAdsPicker" style="background: none; border: none; font-size: 24px; cursor: pointer; color: #666;">Ãƒâ€”</button>
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
    try { ensureSmartviewTitlebarViewControls(); } catch (e) {}
    try { refreshSmartviewTitlebarViewDropdown(); } catch (e) {}

    // Initialize or update controller with selected ADS
    if (window.smartTableController) {
      const ctrl = window.smartTableController;
      const prevAds = (ctrl.adsName || '').toString();
      ctrl.adsName = selectedAdsName;

      // ADS changed -> clear cached metadata so filters/hyperlinks use the right schema
      if (!prevAds || prevAds.toLowerCase() !== selectedAdsName.toLowerCase()) {
        ctrl.lastAdsMeta = null;
        ctrl._adsMetaFor = null;
        ctrl._smartviewSelectedFieldColumns = [];
        // Reset projection/grouping when ADS changes.
        ctrl.select_columns = [];
        ctrl.groupby_columns = [];
        ctrl.column_order = smartviewLoadColumnOrderFromStorage(selectedAdsName);
        ctrl.aggregations = {};
        try {
          smartviewEnsureColumnOrderLoadedForAds(selectedAdsName, (order) => {
            const parsed = smartviewParseColumnOrder(order);
            if (parsed.length) ctrl.column_order = parsed;
          });
        } catch (e) {}
      }

      ctrl.resetPaging();

      // Prefetch ADS metadata (and persist to localStorage) so Filters open instantly later.
      try { if (typeof ctrl.ensureAdsMetadata === 'function') ctrl.ensureAdsMetadata(); } catch (e) {}
      try { smartviewLoadKpiChartsData(selectedAdsName, { refreshCache: false }); } catch (e) {}

      ctrl.loadNextPage();
    } else {
      window.smartTableController = new SmartViewTableController({
        adsName: selectedAdsName,
        pageSize: 100,
        currentPage: 1,
        sorting: []
      });
      try { smartviewLoadKpiChartsData(selectedAdsName, { refreshCache: false }); } catch (e) {}
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

  function smartviewGetTableLoaderState() {
    window._smartviewTableLoaderState = window._smartviewTableLoaderState || {
      count: 0,
      selector: '#table-body_Container',
      useGlobal: false
    };
    return window._smartviewTableLoaderState;
  }

  function smartviewEnsureTableLoaderStyles() {
    if (document.getElementById('smartviewTableLoaderStyles')) return;
    const style = document.createElement('style');
    style.id = 'smartviewTableLoaderStyles';
    style.textContent = `
      #table-body_Container.sv-loading {
        min-height: 220px;
      }
      .sv-table-loader-mask {
        position: absolute;
        inset: 0;
        z-index: 6;
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(245, 247, 252, 0.88);
        pointer-events: none;
      }
      .sv-table-loader-box {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 10px;
        color: #41506b;
        font-size: 13px;
        font-weight: 600;
      }
      .sv-table-loader-spinner {
        width: 34px;
        height: 34px;
        border-radius: 50%;
        border: 3px solid #d7deec;
        border-top-color: #3b82f6;
        animation: sv-spin 0.9s linear infinite;
      }
      @keyframes sv-spin {
        to { transform: rotate(360deg); }
      }
    `;
    document.head.appendChild(style);
  }

  function smartviewResolveGlobalLoaderFns() {
    const scopes = [];
    try { scopes.push(window); } catch (e) {}
    try { if (window.parent && window.parent !== window) scopes.push(window.parent); } catch (e) {}
    try { if (window.top && window.top !== window) scopes.push(window.top); } catch (e) {}

    for (let i = 0; i < scopes.length; i++) {
      const scope = scopes[i];
      if (!scope) continue;
      if (typeof scope.showLoader === 'function' && typeof scope.hideLoader === 'function') {
        return {
          show: function (selector) { return scope.showLoader(selector); },
          hide: function (selector) { return scope.hideLoader(selector); }
        };
      }
    }
    return null;
  }

  function smartviewShowTableLoader(selector) {
    const state = smartviewGetTableLoaderState();
    const targetSelector = selector || state.selector || '#table-body_Container';
    state.selector = targetSelector;
    state.count = (Number(state.count) || 0) + 1;
    if (state.count > 1) return;

    const loaderFns = smartviewResolveGlobalLoaderFns();
    state.useGlobal = !!loaderFns;
    if (loaderFns) {
      try {
        loaderFns.show(targetSelector);
        return;
      } catch (e) {
        state.useGlobal = false;
      }
    }

    const host = document.querySelector(targetSelector);
    if (!host) return;
    smartviewEnsureTableLoaderStyles();

    if (!host.dataset.svLoaderOriginalPosition) {
      host.dataset.svLoaderOriginalPosition = host.style.position || '';
    }
    const computedPos = window.getComputedStyle(host).position;
    if (!computedPos || computedPos === 'static') host.style.position = 'relative';
    host.classList.add('sv-loading');

    let mask = host.querySelector('.sv-table-loader-mask');
    if (!mask) {
      mask = document.createElement('div');
      mask.className = 'sv-table-loader-mask';
      mask.innerHTML = `
        <div class="sv-table-loader-box">
          <div class="sv-table-loader-spinner"></div>
          <div>Loading data...</div>
        </div>
      `;
      host.appendChild(mask);
    }
  }

  function smartviewHideTableLoader(selector) {
    const state = smartviewGetTableLoaderState();
    state.count = Math.max(0, (Number(state.count) || 0) - 1);
    if (state.count > 0) return;

    const targetSelector = selector || state.selector || '#table-body_Container';
    const loaderFns = smartviewResolveGlobalLoaderFns();
    if (state.useGlobal && loaderFns && typeof loaderFns.hide === 'function') {
      try { loaderFns.hide(targetSelector); } catch (e) {}
      state.useGlobal = false;
      return;
    }

    const host = document.querySelector(targetSelector);
    if (!host) return;

    host.querySelectorAll('.sv-table-loader-mask').forEach(el => el.remove());
    host.classList.remove('sv-loading');
    if (host.dataset.svLoaderOriginalPosition !== undefined) {
      host.style.position = host.dataset.svLoaderOriginalPosition;
      delete host.dataset.svLoaderOriginalPosition;
    }
  }

  const SMARTVIEW_KPI_WRAPPER_SELECTOR = '.Sales-data-content-wrapper';
  const SMARTVIEW_CHARTS_WRAPPER_SELECTOR = '.Sales-data-charts-wrapper';

  function smartviewGetBlockLoaderState() {
    window._smartviewBlockLoaderState = window._smartviewBlockLoaderState || {};
    return window._smartviewBlockLoaderState;
  }

  function smartviewEnsureBlockLoaderStyles() {
    if (document.getElementById('smartviewBlockLoaderStyles')) return;
    const style = document.createElement('style');
    style.id = 'smartviewBlockLoaderStyles';
    style.textContent = `
      .sv-block-loading {
        position: relative;
        overflow: hidden;
      }
      .sv-block-loader-mask {
        position: absolute;
        inset: 0;
        z-index: 8;
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(245, 247, 252, 0.88);
        pointer-events: auto;
      }
      .sv-block-loader-box {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 10px;
        color: #41506b;
        font-size: 13px;
        font-weight: 600;
        text-align: center;
      }
      .sv-block-loader-spinner {
        width: 32px;
        height: 32px;
        border-radius: 50%;
        border: 3px solid #d7deec;
        border-top-color: #3b82f6;
        animation: sv-block-spin 0.9s linear infinite;
      }
      .Sales-data-content-wrapper.sv-block-loading {
        min-height: 118px;
      }
      .Sales-data-charts-wrapper.sv-block-loading {
        min-height: 260px;
      }
      @keyframes sv-block-spin {
        to { transform: rotate(360deg); }
      }
    `;
    document.head.appendChild(style);
  }

  function smartviewShowBlockLoader(selector, options = {}) {
    const key = (selector || '').toString().trim();
    if (!key) return;

    const host = document.querySelector(key);
    if (!host) return;

    const state = smartviewGetBlockLoaderState();
    const entry = state[key] || { count: 0, useGlobal: false };
    entry.count = (Number(entry.count) || 0) + 1;
    state[key] = entry;
    if (entry.count > 1) return;

    smartviewEnsureBlockLoaderStyles();
    try { smartviewSetElementHasData(host, true); } catch (e) {}

    const loaderFns = smartviewResolveGlobalLoaderFns();
    entry.useGlobal = !!loaderFns;
    if (loaderFns) {
      try {
        loaderFns.show(key);
        return;
      } catch (e) {
        entry.useGlobal = false;
      }
    }

    if (host.dataset.svBlockLoaderOriginalPosition === undefined) {
      host.dataset.svBlockLoaderOriginalPosition = host.style.position || '';
    }
    if (host.dataset.svBlockLoaderOriginalMinHeight === undefined) {
      host.dataset.svBlockLoaderOriginalMinHeight = host.style.minHeight || '';
    }

    const computedPos = window.getComputedStyle(host).position;
    if (!computedPos || computedPos === 'static') host.style.position = 'relative';

    const minHeight = Number(options.minHeight) || 0;
    if (minHeight > 0) host.style.minHeight = `${Math.max(80, Math.round(minHeight))}px`;

    host.classList.add('sv-block-loading');

    let mask = host.querySelector('.sv-block-loader-mask');
    if (!mask) {
      mask = document.createElement('div');
      mask.className = 'sv-block-loader-mask';
      host.appendChild(mask);
    }
    const text = (options.text || 'Loading data...').toString();
    mask.innerHTML = `
      <div class="sv-block-loader-box">
        <div class="sv-block-loader-spinner"></div>
        <div>${escapeHtml(text)}</div>
      </div>
    `;
  }

  function smartviewHideBlockLoader(selector) {
    const key = (selector || '').toString().trim();
    if (!key) return;

    const state = smartviewGetBlockLoaderState();
    const entry = state[key];
    if (!entry) return;

    entry.count = Math.max(0, (Number(entry.count) || 0) - 1);
    if (entry.count > 0) {
      state[key] = entry;
      return;
    }
    const usedGlobalLoader = !!entry.useGlobal;
    delete state[key];

    const loaderFns = smartviewResolveGlobalLoaderFns();
    if (usedGlobalLoader && loaderFns && typeof loaderFns.hide === 'function') {
      try { loaderFns.hide(key); } catch (e) {}
      return;
    }

    const host = document.querySelector(key);
    if (!host) return;

    host.querySelectorAll('.sv-block-loader-mask').forEach(el => el.remove());
    host.classList.remove('sv-block-loading');

    if (host.dataset.svBlockLoaderOriginalPosition !== undefined) {
      host.style.position = host.dataset.svBlockLoaderOriginalPosition;
      delete host.dataset.svBlockLoaderOriginalPosition;
    }
    if (host.dataset.svBlockLoaderOriginalMinHeight !== undefined) {
      host.style.minHeight = host.dataset.svBlockLoaderOriginalMinHeight;
      delete host.dataset.svBlockLoaderOriginalMinHeight;
    }
  }

  function smartviewBeginKpiChartsLoader() {
    smartviewShowBlockLoader(SMARTVIEW_KPI_WRAPPER_SELECTOR, { minHeight: 118, text: 'Loading KPI data...' });
    smartviewShowBlockLoader(SMARTVIEW_CHARTS_WRAPPER_SELECTOR, { minHeight: 260, text: 'Loading charts...' });
    let closed = false;
    return function () {
      if (closed) return;
      closed = true;
      smartviewHideBlockLoader(SMARTVIEW_KPI_WRAPPER_SELECTOR);
      smartviewHideBlockLoader(SMARTVIEW_CHARTS_WRAPPER_SELECTOR);
    };
  }

  /* --------------------------
    SmartView Named Views (filters + grouping + columns + ordering)
    -------------------------- */

  function smartviewGetControllerForViews() {
    return window.smartTableController || window._smartviewController || window._smartviewTableController || null;
  }

  function smartviewGetCurrentAdsForViews() {
    const ctrl = smartviewGetControllerForViews();
    return (ctrl && ctrl.adsName) || (window._entity && window._entity.adsName) || '';
  }

  function smartviewViewsStorageKey(adsName) {
    const n = smartviewNormalizeAdsName(adsName || smartviewGetCurrentAdsForViews());
    return n ? `smartview_views::${n}` : `smartview_views::`;
  }

  function smartviewCloneJsonSafe(v) {
    try { return JSON.parse(JSON.stringify(v)); } catch (e) { return v; }
  }

  function smartviewSortObjectDeep(input) {
    if (Array.isArray(input)) {
      return input.map(function (item) { return smartviewSortObjectDeep(item); });
    }
    if (input && typeof input === "object") {
      var out = {};
      Object.keys(input).sort().forEach(function (k) {
        out[k] = smartviewSortObjectDeep(input[k]);
      });
      return out;
    }
    return input;
  }

  function smartviewStableStringify(input) {
    try {
      return JSON.stringify(smartviewSortObjectDeep(input));
    } catch (e) {
      try { return JSON.stringify(input); } catch (_e) { return ""; }
    }
  }

  function smartviewFastHash(str) {
    var s = String(str || "");
    var h = 0;
    for (var i = 0; i < s.length; i++) {
      h = ((h << 5) - h) + s.charCodeAt(i);
      h |= 0;
    }
    var n = h < 0 ? (-h) : h;
    return n.toString(36) + "_" + s.length;
  }

  function smartviewBuildFullDataCacheSignature(params) {
    var p = smartviewCloneJsonSafe(params || {});
    p.props = p.props || {};
    p.props.pageno = 1;
    p.props.pagesize = 0;
    p.props.getallrecordscount = false;
    return smartviewStableStringify(p);
  }

  function smartviewBuildFullDataCacheKey(params) {
    var p = params || {};
    var ads = "";
    var cacheVersion = 2;
    try {
      ads = (Array.isArray(p.adsNames) && p.adsNames[0]) ? String(p.adsNames[0]).trim().toLowerCase() : "";
    } catch (e) {}
    var signature = smartviewBuildFullDataCacheSignature(p);
    var hash = smartviewFastHash(signature);
    return {
      cacheKey: "svfullv" + cacheVersion + "::" + ads + "::" + hash,
      signature: signature,
      adsName: ads
    };
  }

  function smartviewLoadViewsFromStorage(adsName) {
    try {
      if (typeof localStorage === 'undefined') return [];
      const key = smartviewViewsStorageKey(adsName);
      const raw = localStorage.getItem(key);
      if (!raw) return [];
      const parsed = JSON.parse(raw);
      if (Array.isArray(parsed)) return parsed;
      if (parsed && Array.isArray(parsed.views)) return parsed.views;
      return [];
    } catch (e) {
      return [];
    }
  }

  function smartviewSaveViewsToStorage(adsName, views) {
    try {
      if (typeof localStorage === 'undefined') return;
      const key = smartviewViewsStorageKey(adsName);
      localStorage.setItem(key, JSON.stringify(Array.isArray(views) ? views : []));
    } catch (e) {
      // ignore
    }
  }

  function smartviewColumnOrderStorageKey(adsName) {
    const n = smartviewNormalizeAdsName(adsName || smartviewGetCurrentAdsForViews());
    return n ? `smartview_colorder::${n}` : `smartview_colorder::`;
  }

  function smartviewParseColumnOrder(raw) {
    if (!raw) return [];
    const arr = Array.isArray(raw) ? raw : String(raw).split(',');
    const seen = new Set();
    const out = [];
    arr.forEach(v => {
      const key = String(v || '').trim().toLowerCase();
      if (!key || seen.has(key)) return;
      seen.add(key);
      out.push(key);
    });
    return out;
  }

  function smartviewLoadColumnOrderFromStorage(adsName) {
    try {
      if (typeof localStorage === 'undefined') return [];
      const raw = localStorage.getItem(smartviewColumnOrderStorageKey(adsName));
      if (!raw) return [];
      return smartviewParseColumnOrder(JSON.parse(raw));
    } catch (e) {
      return [];
    }
  }

  function smartviewSaveColumnOrderToStorage(adsName, order) {
    try {
      if (typeof localStorage === 'undefined') return;
      localStorage.setItem(smartviewColumnOrderStorageKey(adsName), JSON.stringify(smartviewParseColumnOrder(order)));
    } catch (e) {}
  }

  function smartviewGetCurrentUserName() {
    let userName = '';
    try { userName = window?.top?.mainUserName || ''; } catch (e) {}
    if (!userName && typeof callParentNew === 'function') {
      try { userName = callParentNew('mainUserName') || ''; } catch (e) {}
    }
    return String(userName || '').trim();
  }

  function smartviewIsAdminUser() {
    return smartviewGetCurrentUserName().toLowerCase() === 'admin';
  }

  function smartviewGetAnalyticsPropertyValue(props, name) {
    if (!props || typeof props !== 'object') return '';
    const target = String(name || '').toLowerCase();
    const hit = Object.keys(props).find(k => String(k || '').toLowerCase() === target);
    return hit ? props[hit] : '';
  }

  function smartviewExtractAnalyticsProperties(response) {
    let parsed = response;
    if (parsed && typeof parsed === 'object' && parsed.d !== undefined) {
      parsed = parsed.d;
    }
    if (typeof parsed === 'string') {
      try { parsed = JSON.parse(parsed); } catch (e) { return {}; }
    }
    const candidates = [
      parsed?.result?.data?.Properties,
      parsed?.result?.data?.properties,
      parsed?.result?.data,
      parsed?.result?.Properties,
      parsed?.result?.properties,
      parsed?.data?.Properties,
      parsed?.data?.properties,
      parsed?.properties,
      parsed
    ];
    for (let i = 0; i < candidates.length; i++) {
      const c = candidates[i];
      if (c && typeof c === 'object' && !Array.isArray(c)) return c;
    }
    return {};
  }

  function smartviewFetchColumnOrderFromServer(adsName, callback) {
    const done = (order) => { try { if (typeof callback === 'function') callback(smartviewParseColumnOrder(order)); } catch (e) {} };
    try {
      const ads = String(adsName || '').trim();
      if (!ads) { done([]); return; }
      if (typeof _entityCommon === 'undefined' || !_entityCommon || typeof _entityCommon.getAnalyticsDataWS !== 'function') {
        done([]);
        return;
      }
      const payload = {
        page: 'SmartView',
        transId: ads,
        propertiesList: ['SV_COLUMNORDER']
      };
      _entityCommon.getAnalyticsDataWS(payload, (resp) => {
        try {
          const props = smartviewExtractAnalyticsProperties(resp);
          const raw = smartviewGetAnalyticsPropertyValue(props, 'SV_COLUMNORDER') || smartviewGetAnalyticsPropertyValue(props, 'COLUMNORDER');
          const order = smartviewParseColumnOrder(raw);
          if (order.length) smartviewSaveColumnOrderToStorage(ads, order);
          done(order);
        } catch (e) {
          done([]);
        }
      }, () => done([]));
    } catch (e) {
      done([]);
    }
  }

  function smartviewEnsureColumnOrderLoadedForAds(adsName, callback) {
    const ads = String(adsName || '').trim();
    const done = (order) => { try { if (typeof callback === 'function') callback(smartviewParseColumnOrder(order)); } catch (e) {} };
    if (!ads) { done([]); return; }

    const local = smartviewLoadColumnOrderFromStorage(ads);
    if (local.length) { done(local); return; }
    smartviewFetchColumnOrderFromServer(ads, done);
  }

  function smartviewPersistColumnOrder(order, applyForAllUsers) {
    try {
      const ctrl = smartviewGetControllerForViews();
      const ads = (ctrl && ctrl.adsName) || smartviewGetCurrentAdsForViews();
      const ordered = smartviewParseColumnOrder(order);
      if (!ads || !ordered.length) return;

      smartviewSaveColumnOrderToStorage(ads, ordered);

      if (typeof _entityCommon !== 'undefined' && _entityCommon && typeof _entityCommon.setAnalyticsDataWS === 'function') {
        const data = {
          page: 'SmartView',
          transId: ads,
          properties: { SV_COLUMNORDER: ordered.join(',') },
          confirmNeeded: false,
          allUsers: !!applyForAllUsers
        };
        _entityCommon.setAnalyticsDataWS(data, () => {}, () => {});
      }
    } catch (e) {}
  }

  function smartviewGetSelectedNamedViewId() {
    try {
      const footerHost = smartviewGetFooterViewTabsHost();
      if (footerHost) {
        const hostRaw = String(footerHost.getAttribute('data-sv-active-view-id') || '').trim();
        if (hostRaw) return hostRaw;

        const activeTab = footerHost.querySelector('.sv-footer-view-tab-link.is-active[data-view-id]');
        if (activeTab) {
          const tabRaw = String(activeTab.getAttribute('data-view-id') || '').trim();
          if (tabRaw) return tabRaw;
        }
      }

      const sel = document.getElementById('smartviewTitlebarViewSelect');
      if (!sel) return '';
      const raw = String(sel.value || '').trim();
      if (!raw || raw === SMARTVIEW_VIEW_SAVE_AS_VALUE) return '';
      return raw;
    } catch (e) {
      return '';
    }
  }

  function smartviewCaptureVisibilityState() {
    try {
      const kpiWrap = document.querySelector('.Sales-data-content-wrapper');
      const chartsWrap = document.querySelector('.Sales-data-charts-wrapper');
      return {
        kpi_visible: smartviewGetElementPreferredVisibility(kpiWrap),
        charts_visible: smartviewGetElementPreferredVisibility(chartsWrap)
      };
    } catch (e) {
      return { kpi_visible: true, charts_visible: true };
    }
  }

  function smartviewApplyVisibilityState(state) {
    try {
      const st = state || {};
      const kpiWrap = document.querySelector('.Sales-data-content-wrapper');
      const chartsWrap = document.querySelector('.Sales-data-charts-wrapper');
      const kpiVisible = (st.kpi_visible === undefined || st.kpi_visible === null) ? true : !!st.kpi_visible;
      const chartsVisible = (st.charts_visible === undefined || st.charts_visible === null) ? true : !!st.charts_visible;

      smartviewSetElementVisible(kpiWrap, kpiVisible);
      smartviewSetElementVisible(chartsWrap, chartsVisible);
      smartviewRefreshOptionsMenuLabels();
    } catch (e) {}
  }

  function smartviewHasExpandedEditRows() {
    try {
      return !!document.querySelector('.expand-row.sv-expanded');
    } catch (e) {
      return false;
    }
  }

  function smartviewSyncExpandedTableWrapperClass() {
    try {
      const hasExpanded = smartviewHasExpandedEditRows();
      const kpiWrap = document.querySelector('.Sales-data-content-wrapper');
      const chartsWrap = document.querySelector('.Sales-data-charts-wrapper');
      const hideKpi = !!(kpiWrap && !smartviewGetElementPreferredVisibility(kpiWrap));
      const hideCharts = !!(chartsWrap && !smartviewGetElementPreferredVisibility(chartsWrap));

      document.querySelectorAll('#table-body_Container .table-responsive').forEach(function (el) {
        el.classList.toggle('Expanded', !!hasExpanded);
        el.classList.toggle('Hide-kpi', hideKpi);
        el.classList.toggle('Hide-charts', hideCharts);
      });
    } catch (e) {}
  }

  function smartviewEnterTableFocusMode() {
    try {
      if (!window._smartviewTableFocusState) {
        window._smartviewTableFocusState = smartviewCaptureVisibilityState();
      }

      const kpiWrap = document.querySelector('.Sales-data-content-wrapper');
      const chartsWrap = document.querySelector('.Sales-data-charts-wrapper');
      smartviewSetElementVisible(kpiWrap, false);
      smartviewSetElementVisible(chartsWrap, false);
      document.body.classList.add('smartview-table-focus-mode');
      smartviewSyncExpandedTableWrapperClass();
      smartviewRefreshOptionsMenuLabels();
    } catch (e) {}
  }

  function smartviewExitTableFocusMode(forceRestore) {
    try {
      if (!forceRestore && smartviewHasExpandedEditRows()) return;

      document.body.classList.remove('smartview-table-focus-mode');

      const prevState = window._smartviewTableFocusState;
      if (prevState && typeof prevState === 'object') {
        smartviewApplyVisibilityState(prevState);
      } else {
        smartviewRefreshOptionsMenuLabels();
      }

      window._smartviewTableFocusState = null;
      smartviewSyncExpandedTableWrapperClass();
      smartviewRefreshOptionsMenuLabels();
    } catch (e) {}
  }

  function smartviewSyncTableFocusModeFromDom() {
    try {
      if (smartviewHasExpandedEditRows()) smartviewEnterTableFocusMode();
      else smartviewExitTableFocusMode(true);
      smartviewSyncExpandedTableWrapperClass();
    } catch (e) {}
  }

  function smartviewCaptureCurrentViewState(filtersOverride) {
    const ctrl = smartviewGetControllerForViews();
    const currentFilters = Array.isArray(filtersOverride) ? filtersOverride : ((ctrl && Array.isArray(ctrl.filters)) ? ctrl.filters : []);
    const visibility = smartviewCaptureVisibilityState();
    const groupedFields = (ctrl && typeof smartviewGetEffectiveGroupbyColumns === 'function')
      ? smartviewGetEffectiveGroupbyColumns(ctrl)
      : ((ctrl && Array.isArray(ctrl.groupby_columns)) ? ctrl.groupby_columns : []);
    return {
      filters: stripSmartviewFilterTransId(smartviewCloneJsonSafe(currentFilters || [])),
      sorting: smartviewCloneJsonSafe((ctrl && Array.isArray(ctrl.sorting)) ? ctrl.sorting : []),
      select_columns: smartviewCloneJsonSafe((ctrl && Array.isArray(ctrl.select_columns)) ? ctrl.select_columns : []),
      field_columns: smartviewCloneJsonSafe(smartviewGetVisibleSelectedFieldColumns(ctrl)),
      groupby_columns: smartviewCloneJsonSafe(groupedFields || []),
      column_order: smartviewCloneJsonSafe((ctrl && Array.isArray(ctrl.column_order)) ? ctrl.column_order : smartviewLoadColumnOrderFromStorage(smartviewGetCurrentAdsForViews())),
      keyField: ((window._entity && window._entity.keyField) ? String(window._entity.keyField) : ''),
      kpi_visible: visibility.kpi_visible,
      charts_visible: visibility.charts_visible
    };
  }

  function smartviewUpsertNamedView(caption, opts) {
    try {
      const options = opts || {};
      const ctrl = smartviewGetControllerForViews();
      const adsName = (ctrl && ctrl.adsName) || smartviewGetCurrentAdsForViews();
      if (!adsName) return null;

      const cap = (caption || '').toString().trim();
      if (!cap) return null;

      const views = smartviewLoadViewsFromStorage(adsName);
      const viewId = (options.id || options.viewId || (`view_${Date.now()}`)).toString();
      const idx = views.findIndex(v => v && String(v.id || '') === viewId);
      const prev = (idx >= 0) ? (views[idx] || {}) : null;

      const item = {
        id: viewId,
        caption: cap,
        adsName: adsName,
        state: smartviewCaptureCurrentViewState(options.filtersOverride),
        createdAt: (prev && prev.createdAt) ? prev.createdAt : Date.now(),
        updatedAt: Date.now()
      };

      if (idx >= 0) views[idx] = item;
      else views.push(item);

      smartviewSaveViewsToStorage(adsName, views);
      refreshSmartviewTitlebarViewDropdown(viewId);
      return item;
    } catch (e) {
      console.error('smartviewUpsertNamedView failed', e);
      return null;
    }
  }

  function smartviewDeleteNamedView(viewId, adsName) {
    try {
      const token = (viewId || '').toString().trim();
      if (!token) return;
      const ads = adsName || smartviewGetCurrentAdsForViews();
      const tokenL = token.toLowerCase();
      const views = smartviewLoadViewsFromStorage(ads).filter(v => {
        const id = String(v && v.id || '');
        const cap = String(v && v.caption || '');
        return id !== token && cap.toLowerCase() !== tokenL;
      });
      smartviewSaveViewsToStorage(ads, views);
      refreshSmartviewTitlebarViewDropdown();
    } catch (e) {}
  }

  function smartviewRemoveSelectedView() {
    try {
      const ads = smartviewGetCurrentAdsForViews();
      if (!ads) {
        alert('Select an ADS first.');
        return false;
      }

      const activeId = smartviewGetSelectedNamedViewId() || '';
      if (!activeId) {
        alert('Please select a saved view tab to remove.');
        return false;
      }

      const views = smartviewLoadViewsFromStorage(ads);
      const current = views.find(v => v && String(v.id || '') === activeId) || null;
      if (!current) {
        refreshSmartviewTitlebarViewDropdown('');
        return false;
      }

      const ok = window.confirm(`Remove view "${String(current.caption || current.id || activeId)}"?`);
      if (!ok) return false;

      smartviewDeleteNamedView(activeId, ads);
      smartviewApplyDefaultView();
      return true;
    } catch (e) {
      console.error('smartviewRemoveSelectedView failed', e);
      return false;
    }
  }

  function smartviewApplyNamedView(viewId) {
    try {
      const id = (viewId || '').toString().trim();
      if (!id) return false;

      const ctrl = smartviewGetControllerForViews();
      if (!ctrl) return false;
      const ads = ctrl.adsName || smartviewGetCurrentAdsForViews();
      const views = smartviewLoadViewsFromStorage(ads);
      const view = views.find(v => v && String(v.id || '') === id);
      if (!view || !view.state) return false;

      const st = view.state || {};
      ctrl.filters = stripSmartviewFilterTransId(Array.isArray(st.filters) ? smartviewCloneJsonSafe(st.filters) : []);
      ctrl.sorting = Array.isArray(st.sorting) ? smartviewCloneJsonSafe(st.sorting) : [];
      const requestedGroups = smartviewNormalizeGroupbyFields(Array.isArray(st.groupby_columns) ? smartviewCloneJsonSafe(st.groupby_columns) : []);
      ctrl._smartviewSelectedFieldColumns = smartviewNormalizeSelectedFieldColumns(
        Array.isArray(st.field_columns) && st.field_columns.length
          ? smartviewCloneJsonSafe(st.field_columns)
          : (Array.isArray(st.select_columns) ? smartviewCloneJsonSafe(st.select_columns) : [])
      );
      const metaForGroup = (Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length)
        ? ctrl.lastAdsMeta
        : (window._entity && Array.isArray(window._entity.metaData) ? window._entity.metaData : []);
      if (typeof smartviewApplyGroupbySelection === 'function') {
        smartviewApplyGroupbySelection(ctrl, metaForGroup, requestedGroups);
      } else {
        ctrl.groupby_columns = requestedGroups.slice();
        ctrl.select_columns = Array.isArray(st.select_columns) ? smartviewCloneJsonSafe(st.select_columns) : [];
      }
      if (!requestedGroups.length) {
        ctrl.select_columns = ctrl._smartviewSelectedFieldColumns.slice();
      }
      ctrl.column_order = smartviewParseColumnOrder(st.column_order || smartviewLoadColumnOrderFromStorage(ads));
      ctrl.aggregations = {};
      ctrl.forceClientFiltering = ctrl.filters.length > 0;
      ctrl._filteredCache = null;
      window._smartviewFullData = null;

      if (window._entity) {
        if (st.keyField) window._entity.keyField = st.keyField;
        window._entity.adsName = ads;
      }
      smartviewApplyVisibilityState(st);

      // Keep filter pills consistent for UX (show only active view filters).
      try {
        const EntityFilterCtor = smartviewResolveEntityFilterCtor();
        if (typeof EntityFilterCtor === 'function') {
          window._entityFilter = window._entityFilter || new EntityFilterCtor();
        }
        if (window._entityFilter) {
          window._entityFilter.filterObj = {};
          if (Array.isArray(ctrl.filters) && ctrl.filters.length > 0) {
            window._entityFilter.filterObj[id] = { caption: view.caption || id, filter: ctrl.filters.slice(), save: true };
            window._entityFilter.activeFilterId = id;
            window._entityFilter.activeFilterName = view.caption || id;
            window._entityFilter.activeFilterArray = ctrl.filters.slice();
          } else {
            window._entityFilter.activeFilterId = '';
            window._entityFilter.activeFilterName = '';
            window._entityFilter.activeFilterArray = [];
          }
          if (typeof window._entityFilter.createFilterPills === 'function') window._entityFilter.createFilterPills();
        }
      } catch (e) {}

      if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
      if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();

      refreshSmartviewTitlebarViewDropdown(id);
      return true;
    } catch (e) {
      console.error('smartviewApplyNamedView failed', e);
      return false;
    }
  }

  const SMARTVIEW_VIEW_SAVE_AS_VALUE = '__save_as__';

  function smartviewApplyDefaultView() {
    try {
      const ctrl = smartviewGetControllerForViews();
      if (!ctrl) return false;

      ctrl.filters = [];
      ctrl.sorting = [];
      ctrl._smartviewSelectedFieldColumns = [];
      ctrl.select_columns = [];
      ctrl.groupby_columns = [];
      ctrl.column_order = smartviewLoadColumnOrderFromStorage(ctrl.adsName || smartviewGetCurrentAdsForViews());
      ctrl.aggregations = {};
      ctrl.forceClientFiltering = false;
      ctrl._filteredCache = null;
      window._smartviewFullData = null;

      try {
        if (window._entityFilter) {
          window._entityFilter.filterObj = {};
          window._entityFilter.activeFilterId = '';
          window._entityFilter.activeFilterName = '';
          window._entityFilter.activeFilterArray = [];
          if (typeof window._entityFilter.createFilterPills === 'function') window._entityFilter.createFilterPills();
        }
      } catch (e) {}
      smartviewApplyVisibilityState({ kpi_visible: true, charts_visible: true });

      if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
      if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
      refreshSmartviewTitlebarViewDropdown('');
      return true;
    } catch (e) {
      console.error('smartviewApplyDefaultView failed', e);
      return false;
    }
  }

  function smartviewPromptAndSaveCurrentAsView(options) {
    try {
      const opts = options && typeof options === 'object' ? options : {};
      const ads = smartviewGetCurrentAdsForViews();
      if (!ads) {
        alert('Select an ADS first.');
        return;
      }

      const raw = window.prompt('Enter view name');
      const caption = (raw || '').toString().trim();
      if (!caption) return;

      const views = smartviewLoadViewsFromStorage(ads);
      const existing = views.find(v => String(v?.caption || '').toLowerCase() === caption.toLowerCase()) || null;
      if (existing && opts.forceNew) {
        const ok = window.confirm(`View "${caption}" already exists. Create another saved view with this name?`);
        if (!ok) return;
      } else if (existing) {
        const ok = window.confirm(`View "${caption}" already exists. Overwrite it?`);
        if (!ok) return;
      }

      const saved = smartviewUpsertNamedView(caption, {
        id: (!opts.forceNew && existing && existing.id) ? String(existing.id) : undefined
      });
      if (saved && saved.id) {
        refreshSmartviewTitlebarViewDropdown(String(saved.id));
        smartviewApplyNamedView(String(saved.id));
      } else {
        refreshSmartviewTitlebarViewDropdown();
      }
    } catch (e) {
      console.error('smartviewPromptAndSaveCurrentAsView failed', e);
    }
  }

  function smartviewSaveCurrentViewAsNew() {
    try {
      smartviewPromptAndSaveCurrentAsView({ forceNew: true });
    } catch (e) {
      console.error('smartviewSaveCurrentViewAsNew failed', e);
    }
    return false;
  }

  function smartviewSaveCurrentView() {
    try {
      const ads = smartviewGetCurrentAdsForViews();
      if (!ads) {
        alert('Select an ADS first.');
        return false;
      }

      const activeId = smartviewGetSelectedNamedViewId();
      if (!activeId) {
        // No named view selected (Default state) -> fall back to Save As.
        smartviewPromptAndSaveCurrentAsView();
        return false;
      }

      const views = smartviewLoadViewsFromStorage(ads);
      const current = views.find(v => v && String(v.id || '') === activeId) || null;
      if (!current) {
        smartviewPromptAndSaveCurrentAsView();
        return false;
      }

      const saved = smartviewUpsertNamedView(String(current.caption || current.id || '').trim(), { id: activeId });
      refreshSmartviewTitlebarViewDropdown(saved && saved.id ? String(saved.id) : activeId);
    } catch (e) {
      console.error('smartviewSaveCurrentView failed', e);
    }
    return false;
  }

  function smartviewRefreshViewMenuState() {
    try {
      const activeId = smartviewGetSelectedNamedViewId() || '';
      const hasSavedView = !!activeId;

      const saveLabel = document.getElementById('smartviewOptionSaveViewLabel');
      if (saveLabel) {
        saveLabel.textContent = hasSavedView ? 'Update current view' : 'Save view';
      }

      const saveAsLabel = document.getElementById('smartviewOptionSaveAsViewLabel');
      if (saveAsLabel) {
        saveAsLabel.textContent = 'Save as new view';
      }

      const removeItem = document.getElementById('smartviewOptionRemoveViewItem');
      if (removeItem) {
        removeItem.style.display = hasSavedView ? '' : 'none';
      }
    } catch (e) {}
  }

  function smartviewGetFooterViewTabsHost() {
    return document.querySelector('#Analytics-Footer .Tab-Toolbar-Sec .nav.nav-stretch') || null;
  }

  function smartviewRenderFooterViewTabs(preferredId) {
    try {
      const host = smartviewGetFooterViewTabsHost();
      if (!host) return;
      host.classList.add('sv-footer-view-tabs-host');
      host.setAttribute('role', 'tablist');

      const ads = smartviewGetCurrentAdsForViews();
      const views = smartviewLoadViewsFromStorage(ads);
      const preferred = (preferredId !== undefined && preferredId !== null)
        ? String(preferredId)
        : '';
      const hostKeep = String(host.getAttribute('data-sv-active-view-id') || '');
      const keepRaw = preferred || hostKeep || '';
      const keep = (keepRaw && views.some(v => String(v && v.id || '') === keepRaw)) ? keepRaw : '';

      const renderTab = function (id, caption, isActive) {
        const li = document.createElement('li');
        li.className = 'nav-item sv-footer-view-tab-item';

        const a = document.createElement('a');
        a.href = '#';
        a.className = `nav-link sv-footer-view-tab-link${isActive ? ' is-active' : ''}`;
        a.setAttribute('data-view-id', String(id || ''));
        a.setAttribute('role', 'tab');
        a.setAttribute('aria-selected', isActive ? 'true' : 'false');

        const label = document.createElement('span');
        label.textContent = String(caption || '');
        a.appendChild(label);

        li.appendChild(a);
        return li;
      };

      host.innerHTML = '';
      host.appendChild(renderTab('', 'Default View', !keep));
      views.forEach(v => {
        const id = String(v && v.id || '');
        const caption = String(v && (v.caption || v.id) || '');
        if (!id || !caption) return;
        host.appendChild(renderTab(id, caption, keep === id));
      });

      host.setAttribute('data-sv-active-view-id', keep);

      if (!host._smartviewViewTabsBound) {
        host.addEventListener('click', function (event) {
          const tab = event.target && event.target.closest ? event.target.closest('a[data-view-id]') : null;
          if (!tab || !host.contains(tab)) return;
          event.preventDefault();
          const viewId = String(tab.getAttribute('data-view-id') || '').trim();
          if (!viewId) smartviewApplyDefaultView();
          else smartviewApplyNamedView(viewId);
        });
        host._smartviewViewTabsBound = true;
      }
    } catch (e) {
      console.error('smartviewRenderFooterViewTabs failed', e);
    }
  }

  function refreshSmartviewTitlebarViewDropdown(preferredId) {
    const ads = smartviewGetCurrentAdsForViews();
    const views = smartviewLoadViewsFromStorage(ads);
    const sel = document.getElementById('smartviewTitlebarViewSelect');

    const keepRaw = (preferredId !== undefined && preferredId !== null)
      ? String(preferredId)
      : String((sel && sel.value) || '');
    const keep = (keepRaw && views.some(v => String(v && v.id || '') === keepRaw)) ? keepRaw : '';

    if (sel) {
      while (sel.firstChild) sel.removeChild(sel.firstChild);

      const defaultOpt = document.createElement('option');
      defaultOpt.value = '';
      defaultOpt.textContent = 'Default View';
      sel.appendChild(defaultOpt);

      views.forEach(v => {
        const opt = document.createElement('option');
        opt.value = String(v.id || '');
        opt.textContent = String(v.caption || v.id || '');
        sel.appendChild(opt);
      });

      const saveAsOpt = document.createElement('option');
      saveAsOpt.value = SMARTVIEW_VIEW_SAVE_AS_VALUE;
      saveAsOpt.textContent = 'Save As...';
      sel.appendChild(saveAsOpt);

      sel.value = keep || '';
      sel.disabled = false;
    }

    smartviewRenderFooterViewTabs(keep);
    try { smartviewRefreshViewMenuState(); } catch (e) {}
  }

  function ensureSmartviewTitlebarViewControls() {
    try {
      const titleBar = document.querySelector('.Page-title') || document.querySelector('.page-header') || null;
      const fallbackHost = document.querySelector('.Page-title .card-title') || document.querySelector('.page-header-title') || document.getElementById('EntityTitle')?.parentElement;
      const host = titleBar || fallbackHost;
      if (!host) return;

      const toolbar = titleBar ? (titleBar.querySelector('.card-toolbar') || null) : null;

      let wrap = document.getElementById('smartviewTitlebarViewWrap');
      if (!wrap) {
        wrap = document.createElement('div');
        wrap.id = 'smartviewTitlebarViewWrap';
        wrap.style.cssText = 'display:flex;align-items:center;gap:6px;flex:0 0 auto;margin-left:10px;margin-right:auto;';
        wrap.innerHTML = `
          <select id="smartviewTitlebarViewSelect" class="form-select form-select-sm" style="min-width:140px;max-width:170px;height:30px;padding-top:2px;padding-bottom:2px;">
            <option value="">Default View</option>
          </select>
        `;
        if (toolbar && toolbar.parentElement === host) host.insertBefore(wrap, toolbar);
        else host.appendChild(wrap);
      } else if (toolbar && wrap.parentElement !== host) {
        host.insertBefore(wrap, toolbar);
      }

      // Remove legacy save button if present (views are created from filter custom name only).
      try {
        const oldBtn = document.getElementById('smartviewSaveCurrentViewBtn');
        if (oldBtn) oldBtn.remove();
      } catch (e) {}

      const sel = document.getElementById('smartviewTitlebarViewSelect');
      if (sel && !sel._smartviewViewBound) {
        sel.addEventListener('change', function () {
          const id = String(this.value || '');
          if (!id) {
            smartviewApplyDefaultView();
            return;
          }
          if (id === SMARTVIEW_VIEW_SAVE_AS_VALUE) {
            this.value = '';
            smartviewPromptAndSaveCurrentAsView();
            return;
          }
          smartviewApplyNamedView(id);
        });
        sel._smartviewViewBound = true;
      }

      refreshSmartviewTitlebarViewDropdown();
    } catch (e) {
      console.error('ensureSmartviewTitlebarViewControls failed', e);
    }
  }

  function smartviewPersistActiveSavedFilterAsView(entityFilterInstance, filters) {
    // no-op: views are explicitly saved via the title-bar "Save As..." option.
    return null;
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

  function formatDateString(dateString) {
    if (!dateString) return '';

    // Match Entity.js behaviour: format as DD/MM/YYYY (or MM/DD/YYYY for en-us),
    // keep time only when it isn't 00:00:00.
    let culture = '';
    try {
      culture = (typeof dtCulture !== 'undefined')
        ? dtCulture
        : ((window && window.dtCulture) || (window.top && window.top.dtCulture) || '');
    } catch (e) { culture = ''; }
    culture = String(culture || '').toLowerCase();
    const isUS = (culture === 'en-us');

    try {
      let s = String(dateString || '');
      if (!s) return '';

      // Already formatted
      if (/^\d{2}\/\d{2}\/\d{4}(?:\s+\d{2}:\d{2}:\d{2})?$/.test(s)) return s;

      s = s.replaceAll('T', ' ');

      const dateTimeParts = s.split(' ');
      let dateParts = null;
      if (dateTimeParts[0] && dateTimeParts[0].split('-').length === 3) {
        dateParts = dateTimeParts[0].split('-');
      } else if (dateTimeParts[0] && dateTimeParts[0].split('/').length === 3) {
        dateParts = dateTimeParts[0].split('/');
      }

      if (dateParts && dateParts.length === 3) {
        const timeToken = (dateTimeParts[1] || '00:00:00').toString();
        const timeParts = timeToken.split(':');

        let year, month, day;
        if (dateParts[0].length === 4) {
          year = dateParts[0];
          month = dateParts[1];
          day = dateParts[2];
        } else {
          year = dateParts[2];
          month = dateParts[1];
          day = dateParts[0];
        }

        day = String(day).padStart(2, '0');
        month = String(month).padStart(2, '0');

        const hours = String(timeParts[0] || '00').padStart(2, '0');
        const minutes = String(timeParts[1] || '00').padStart(2, '0');
        let seconds = String(timeParts[2] || '00');
        seconds = seconds.replace(/Z$/i, '').split('.')[0];
        seconds = seconds ? seconds.padStart(2, '0') : '00';

        const formattedDate = isUS
          ? `${month}/${day}/${year}`
          : `${day}/${month}/${year}`;

        const timePart = (hours !== '00' || minutes !== '00' || seconds !== '00')
          ? `${hours}:${minutes}:${seconds}`
          : '';

        return timePart ? `${formattedDate} ${timePart}` : formattedDate;
      }

      // Fallback: Date() parsing (covers uncommon formats)
      const d = new Date(s);
      if (!isNaN(d)) {
        const dd = String(d.getDate()).padStart(2, '0');
        const mm = String(d.getMonth() + 1).padStart(2, '0');
        const yyyy = String(d.getFullYear());
        const hh = String(d.getHours()).padStart(2, '0');
        const mi = String(d.getMinutes()).padStart(2, '0');
        const ss = String(d.getSeconds()).padStart(2, '0');
        const formattedDate = isUS ? `${mm}/${dd}/${yyyy}` : `${dd}/${mm}/${yyyy}`;
        const timePart = (hh !== '00' || mi !== '00' || ss !== '00') ? `${hh}:${mi}:${ss}` : '';
        return timePart ? `${formattedDate} ${timePart}` : formattedDate;
      }

      return s;
    } catch (e) {
      try { return String(dateString); } catch (_e) { return ''; }
    }
  }

  function formatNumberBasedOnMillions(amount) {
    let isMillions = false;
    try {
      const globalVars = (window && window.top && window.top.allGloblVars && window.top.allGloblVars.globalVars)
        ? window.top.allGloblVars.globalVars
        : {};
      const gvArr = Object.values(globalVars || {});
      const millionCfg = gvArr.filter(gv => gv && gv["millions"]);
      isMillions = (millionCfg.length !== 0 && millionCfg[0].millions === "T") || false;
    } catch (e) {
      isMillions = false;
    }

    if (isMillions) return InsertCommas(amount);
    return formatIndianNumber(amount);
  }

  function InsertCommas(amount) {
    var i = parseFloat(amount);
    if (isNaN(i)) i = 0.00;
    var minus = '';
    if (i < 0) minus = '-';
    i = Math.abs(i);
    i = parseInt((i + .005) * 100, 10);
    i = i / 100;
    var s = String(i);
    if (s.indexOf('.') < 0) s += '.00';
    if (s.indexOf('.') === (s.length - 2)) s += '0';
    s = minus + s;
    return CommaFormatted(s);
  }

  function CommaFormatted(amount) {
    var delimiter = ",";
    if (String(amount).indexOf(".") === -1) amount = amount + ".";

    var a = String(amount).split('.');
    var d = "";
    if (a.length > 1) d = a[1].toString();
    var i = parseInt(a[0], 10);
    if (isNaN(i)) return '';
    var minus = '';
    if (Number(amount) < 0) minus = '-';
    i = Math.abs(i);
    var n = String(i);
    var parts = [];
    try {
      if (typeof Parameters !== "undefined" && Parameters.length > 0 && Parameters.indexOf("millions~T") > -1) {
        while (n.length > 3) {
          var nn = n.substr(n.length - 3);
          parts.unshift(nn);
          n = n.substr(0, n.length - 3);
        }
      } else {
        if (n.length > 3) {
          var nn3 = n.substr(n.length - 3);
          parts.unshift(nn3);
          n = n.substr(0, n.length - 3);
        }
        while (n.length > 2) {
          var nn2 = n.substr(n.length - 2);
          parts.unshift(nn2);
          n = n.substr(0, n.length - 2);
        }
      }
    } catch (ex) {
      while (n.length > 3) {
        var nnFallback = n.substr(n.length - 3);
        parts.unshift(nnFallback);
        n = n.substr(0, n.length - 3);
      }
    }
    if (n.length > 0) parts.unshift(n);
    n = parts.join(delimiter);
    amount = d.length < 1 ? n : (n + '.' + d);
    amount = minus + amount;
    return amount;
  }

  function formatIndianNumber(amount) {
    if (isNaN(amount)) return '';
    let num = Math.abs(parseInt(amount, 10)).toString();
    let result = '';

    if (num.length > 3) {
      result = ',' + num.slice(-3);
      num = num.slice(0, -3);
    }

    while (num.length > 2) {
      result = ',' + num.slice(-2) + result;
      num = num.slice(0, -2);
    }

    if (num.length > 0) result = num + result;
    return (amount < 0 ? '-' : '') + result;
  }

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
    const raw = (field === null || field === undefined) ? '' : String(field);
    if (!raw) return '';

    const compactKey = raw.toLowerCase().replace(/[\s_\-]/g, '');
    const special = {
      createdby: 'Created By',
      modifiedby: 'Modified By',
      createdon: 'Created On',
      modifiedon: 'Modified On',
      recordid: 'Record ID',
      transid: 'Trans ID'
    };
    if (special[compactKey]) return special[compactKey];

    // Expand underscores/hyphens and camelCase to words, then Title Case.
    let s = raw
      .replace(/[_\-]+/g, ' ')
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .replace(/\s+/g, ' ')
      .trim();

    if (!s) return '';

    return s.split(' ').map(w => {
      if (!w) return '';
      // Keep short all-caps tokens as-is (GST, UOM, etc.).
      if (w.toUpperCase() === w && w.length <= 4) return w;
      return w.charAt(0).toUpperCase() + w.slice(1).toLowerCase();
    }).join(' ');
  }

  function smartviewNormalizeCaption(caption, fallbackField) {
    const capRaw = (caption === null || caption === undefined) ? '' : String(caption).trim();
    const fb = (fallbackField === null || fallbackField === undefined) ? '' : String(fallbackField).trim();
    if (!capRaw) return formatFieldName(fb || '');

    // If caption looks like a raw field name (all lowercase / underscores), format it.
    const looksLikeFieldName = /[_\-]/.test(capRaw) || (capRaw === capRaw.toLowerCase() && !/\s/.test(capRaw));
    if (looksLikeFieldName) return formatFieldName(capRaw);
    return capRaw;
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

  function smartviewColumnWidthsStorageKey(adsName) {
    const n = smartviewNormalizeAdsName(adsName || smartviewGetCurrentAdsForViews());
    return n ? `smartview_colwidths::${n}` : `smartview_colwidths::`;
  }

  function smartviewLoadColumnWidthsFromStorage(adsName) {
    try {
      if (typeof localStorage === 'undefined') return {};
      const raw = localStorage.getItem(smartviewColumnWidthsStorageKey(adsName));
      if (!raw) return {};
      const parsed = JSON.parse(raw);
      return (parsed && typeof parsed === 'object' && !Array.isArray(parsed)) ? parsed : {};
    } catch (e) {
      return {};
    }
  }

  function smartviewSaveColumnWidthsToStorage(adsName, widths) {
    try {
      if (typeof localStorage === 'undefined') return;
      const payload = (widths && typeof widths === 'object' && !Array.isArray(widths)) ? widths : {};
      localStorage.setItem(smartviewColumnWidthsStorageKey(adsName), JSON.stringify(payload));
    } catch (e) {
      // ignore
    }
  }

  function smartviewSyncTableMinWidth(tableEl) {
    if (!tableEl) return;
    try {
      const headerRow = tableEl.querySelector('thead tr');
      if (!headerRow) return;
      const totalWidth = Array.from(headerRow.children || []).reduce(function (sum, cell) {
        return sum + Math.max(0, Math.round(cell.getBoundingClientRect().width || cell.offsetWidth || 0));
      }, 0);
      if (totalWidth > 0) {
        tableEl.style.minWidth = `${Math.max(totalWidth, tableEl.parentElement ? tableEl.parentElement.clientWidth : 0)}px`;
      }
    } catch (e) {}
  }

  function smartviewApplyColumnWidthToTable(tableEl, colIndex, widthPx) {
    if (!tableEl) return;
    const idx = Number(colIndex);
    if (!Number.isFinite(idx) || idx <= 0) return;
    const w = Math.max(80, Math.round(Number(widthPx) || 0));
    if (!w) return;
    const selector = `thead th:nth-child(${idx}), tbody td:nth-child(${idx})`;
    tableEl.querySelectorAll(selector).forEach(cell => {
      cell.style.width = `${w}px`;
      cell.style.minWidth = `${w}px`;
    });
    smartviewSyncTableMinWidth(tableEl);
  }

  function smartviewApplySavedColumnWidths(tableEl) {
    if (!tableEl) return;
    const ads = smartviewGetCurrentAdsForViews();
    const widths = smartviewLoadColumnWidthsFromStorage(ads);
    if (!widths || typeof widths !== 'object') return;

    const headers = tableEl.querySelectorAll('thead th[data-field]');
    headers.forEach((th) => {
      const key = String(th.getAttribute('data-field') || '').trim().toLowerCase();
      if (!key) return;
      const w = Number(widths[key]);
      if (!Number.isFinite(w) || w <= 0) return;
      const row = th.parentElement;
      if (!row) return;
      const colIndex = Array.prototype.indexOf.call(row.children, th) + 1;
      if (colIndex <= 0) return;
      smartviewApplyColumnWidthToTable(tableEl, colIndex, w);
    });
    smartviewSyncTableMinWidth(tableEl);
  }

  function smartviewBindColumnResizeHandlers(tableEl) {
    if (!tableEl) return;
    if (tableEl.dataset.svColResizeBound === 'T') return;
    tableEl.dataset.svColResizeBound = 'T';

    tableEl.addEventListener('mousedown', function (ev) {
      const handle = ev.target && ev.target.closest ? ev.target.closest('.sv-col-resizer') : null;
      if (!handle) return;

      const th = handle.closest('th');
      if (!th || !tableEl.contains(th)) return;

      const row = th.parentElement;
      if (!row) return;
      const colIndex = Array.prototype.indexOf.call(row.children, th) + 1;
      if (colIndex <= 0) return;

      const field = String(th.getAttribute('data-field') || '').trim().toLowerCase();
      if (!field) return;

      ev.preventDefault();
      ev.stopPropagation();

      const startX = Number(ev.clientX) || 0;
      const startWidth = Math.max(80, Math.round(th.getBoundingClientRect().width || th.offsetWidth || 120));
      document.body.classList.add('sv-col-resizing');

      const onMove = function (moveEv) {
        const x = Number(moveEv.clientX) || startX;
        const nextWidth = Math.max(80, Math.round(startWidth + (x - startX)));
        smartviewApplyColumnWidthToTable(tableEl, colIndex, nextWidth);
      };

      const onUp = function () {
        try { document.removeEventListener('mousemove', onMove); } catch (e) {}
        document.body.classList.remove('sv-col-resizing');
        const finalWidth = Math.max(80, Math.round(th.getBoundingClientRect().width || th.offsetWidth || startWidth));
        const ads = smartviewGetCurrentAdsForViews();
        const widths = smartviewLoadColumnWidthsFromStorage(ads);
        widths[field] = finalWidth;
        smartviewSaveColumnWidthsToStorage(ads, widths);
      };

      document.addEventListener('mousemove', onMove);
      document.addEventListener('mouseup', onUp, { once: true });
    });
  }

  function smartviewReorderMetaDataByColumnOrder(metaData, columnOrder) {
    const arr = Array.isArray(metaData) ? metaData.slice() : [];
    const order = smartviewParseColumnOrder(columnOrder);
    if (!arr.length || !order.length) return arr;

    const rank = {};
    order.forEach((f, idx) => { rank[String(f || '').toLowerCase()] = idx; });
    const fallbackRank = order.length + 1000;

    return arr.sort((a, b) => {
      const fa = String((a && a.fldname) || '').toLowerCase();
      const fb = String((b && b.fldname) || '').toLowerCase();
      const ra = (rank[fa] !== undefined) ? rank[fa] : fallbackRank;
      const rb = (rank[fb] !== undefined) ? rank[fb] : fallbackRank;
      if (ra !== rb) return ra - rb;
      return 0;
    });
  }

  function smartviewGetHeaderFieldOrder(tableEl) {
    if (!tableEl) return [];
    return Array.from(tableEl.querySelectorAll('thead th[data-field]'))
      .map(th => String(th.getAttribute('data-field') || '').trim().toLowerCase())
      .filter(Boolean);
  }

  function smartviewMoveTableColumn(tableEl, fromIndex, toIndex) {
    if (!tableEl) return;
    const from = Number(fromIndex);
    const to = Number(toIndex);
    if (!Number.isFinite(from) || !Number.isFinite(to) || from === to || from < 0 || to < 0) return;

    tableEl.querySelectorAll('tr').forEach(row => {
      const cells = Array.from(row.children || []);
      if (!cells.length || from >= cells.length || to >= cells.length) return;
      const movingCell = cells[from];
      if (!movingCell) return;
      if (from < to) row.insertBefore(movingCell, cells[to].nextSibling);
      else row.insertBefore(movingCell, cells[to]);
    });
  }

  function smartviewBindColumnReorderHandlers(tableEl) {
    if (!tableEl) return;
    if (tableEl.dataset.svColDragBound === 'T') return;
    tableEl.dataset.svColDragBound = 'T';

    let dragSrcField = '';

    tableEl.addEventListener('dragstart', function (ev) {
      const th = ev.target && ev.target.closest ? ev.target.closest('thead th[data-field]') : null;
      if (!th) return;
      if (ev.target && ev.target.closest && (ev.target.closest('.sv-col-resizer') || ev.target.closest('.sv-header-menu-btn'))) {
        ev.preventDefault();
        return;
      }
      dragSrcField = String(th.getAttribute('data-field') || '').trim().toLowerCase();
      if (!dragSrcField) {
        ev.preventDefault();
        return;
      }
      ev.dataTransfer.effectAllowed = 'move';
      try { ev.dataTransfer.setData('text/plain', dragSrcField); } catch (e) {}
      th.classList.add('sv-col-dragging');
    });

    tableEl.addEventListener('dragover', function (ev) {
      const th = ev.target && ev.target.closest ? ev.target.closest('thead th[data-field]') : null;
      if (!th || !dragSrcField) return;
      ev.preventDefault();
      ev.dataTransfer.dropEffect = 'move';
    });

    tableEl.addEventListener('drop', function (ev) {
      const targetTh = ev.target && ev.target.closest ? ev.target.closest('thead th[data-field]') : null;
      if (!targetTh || !dragSrcField) return;
      ev.preventDefault();

      const sourceTh = tableEl.querySelector(`thead th[data-field="${dragSrcField}"]`);
      if (!sourceTh || sourceTh === targetTh) return;

      const row = targetTh.parentElement;
      if (!row) return;
      const headers = Array.from(row.children || []);
      const fromIdx = headers.indexOf(sourceTh);
      const toIdx = headers.indexOf(targetTh);
      if (fromIdx < 0 || toIdx < 0 || fromIdx === toIdx) return;

      smartviewMoveTableColumn(tableEl, fromIdx, toIdx);

      const newOrder = smartviewGetHeaderFieldOrder(tableEl);
      const ctrl = smartviewGetControllerForViews();
      if (ctrl) ctrl.column_order = newOrder.slice();

      let applyAllUsers = false;
      try {
        if (smartviewIsAdminUser()) {
          applyAllUsers = !!window.confirm('Apply this column order for ALL users?\nOK = All users\nCancel = Only myself');
        } else {
          window.alert('Column order will be saved for yourself only.');
        }
      } catch (e) {}
      smartviewPersistColumnOrder(newOrder, applyAllUsers);
    });

    tableEl.addEventListener('dragend', function () {
      tableEl.querySelectorAll('thead th.sv-col-dragging').forEach(th => th.classList.remove('sv-col-dragging'));
      dragSrcField = '';
    });
  }

  function buildSmartviewHeaderCell(caption, fieldName, extraClass) {
    const title = escapeHtml(caption || '');
    const fld = (fieldName || '').toString().trim();
    if (!fld) return `<th>${title}</th>`;
    const safeFld = escapeHtml(fld);
    const cls = ['sv-data-header'];
    cls.push('sv-resizable-header');
    cls.push('sv-draggable-header');
    if (extraClass) cls.push(String(extraClass).trim());
    return `
      <th class="${cls.join(' ')}" data-field="${safeFld}" draggable="true" title="Drag to reorder">
        <div class="sv-header-cell">
          <span class="sv-header-title">${title}</span>
          <button type="button" class="sv-header-menu-btn" data-field="${safeFld}" title="Column menu" aria-label="Column menu">
            <span class="sv-header-menu-icon">...</span>
          </button>
        </div>
        <span class="sv-col-resizer" data-field="${safeFld}" title="Resize column"></span>
      </th>
    `;
  }

  function smartviewIsMarkedKeyField(meta) {
    if (!meta || typeof meta !== "object") return false;
    const raw = (meta.keyfield !== undefined)
      ? meta.keyfield
      : ((meta.keyField !== undefined) ? meta.keyField : meta.key_field);
    return smartviewFlagFromValue(raw, false);
  }

  function smartviewResolveMetadataKeyFields(metaData) {
    const seen = new Set();
    const out = [];
    (Array.isArray(metaData) ? metaData : []).forEach(function (item) {
      if (!smartviewIsMarkedKeyField(item)) return;
      const fieldName = String((item && item.fldname) || "").trim();
      const token = fieldName.toLowerCase();
      if (!fieldName || seen.has(token)) return;
      seen.add(token);
      out.push(fieldName);
    });
    return out;
  }

  function smartviewResolveConfiguredKeyField(metaData) {
    const arr = Array.isArray(metaData) ? metaData : [];
    for (let i = 0; i < arr.length; i++) {
      const item = arr[i];
      if (!item || typeof item !== 'object') continue;
      const raw = smartviewGetObjectValueCI(item, ['sv_keycol', 'svkeycol', 'keycolumn', 'key_column']);
      if (!smartviewHasValue(raw)) continue;
      const fieldName = String(raw || '').trim();
      if (!fieldName) continue;
      return fieldName;
    }
    return '';
  }

  function smartviewGetActiveKeyFields(metaData, ctrl) {
    const explicit = smartviewResolveMetadataKeyFields(metaData);
    if (explicit.length) return explicit.slice();

    const availableFields = new Set((Array.isArray(metaData) ? metaData : []).map(function (item) {
      return String((item && item.fldname) || "").trim().toLowerCase();
    }).filter(Boolean));
    const seen = new Set();
    const out = [];
    const add = function (value) {
      const fieldName = String(value || "").trim();
      const token = fieldName.toLowerCase();
      if (!fieldName || seen.has(token)) return;
      if (availableFields.size && !availableFields.has(token)) return;
      seen.add(token);
      out.push(fieldName);
    };

    add(smartviewResolveConfiguredKeyField(metaData));
    if (ctrl && Array.isArray(ctrl.keyFields)) ctrl.keyFields.forEach(add);
    add(ctrl && ctrl.keyField);
    if (window._entity && Array.isArray(window._entity.keyFields)) window._entity.keyFields.forEach(add);
    add(window._entity && window._entity.keyField);
    return out;
  }

  function smartviewSyncKeyFieldState(metaData, ctrl) {
    window._entity = window._entity || {};
    const hasMetaData = Array.isArray(metaData) && metaData.length > 0;
    const explicit = smartviewResolveMetadataKeyFields(metaData);
    const nextKeyFields = explicit.length
      ? explicit.slice()
      : smartviewGetActiveKeyFields(metaData, ctrl);

    window._entity.keyFields = nextKeyFields.slice();
    if (nextKeyFields.length) window._entity.keyField = nextKeyFields[0];
    else if (hasMetaData) window._entity.keyField = "";

    if (ctrl && typeof ctrl === "object") {
      ctrl.keyFields = nextKeyFields.slice();
      if (nextKeyFields.length) ctrl.keyField = nextKeyFields[0];
      else if (hasMetaData) ctrl.keyField = "";
    }

    return nextKeyFields.slice();
  }

  function getKeyField() {
    if (!_entity.metaData || !_entity.metaData.length) return null;

    const explicitKeyFields = smartviewResolveMetadataKeyFields(_entity.metaData);
    if (explicitKeyFields.length) {
      const explicit = _entity.metaData.find(m => {
        const fieldName = String((m && m.fldname) || "").trim().toLowerCase();
        return fieldName === String(explicitKeyFields[0] || "").trim().toLowerCase();
      });
      if (explicit) return explicit;
    }

    const configuredKey = smartviewResolveConfiguredKeyField(_entity.metaData);
    if (configuredKey) {
      const configured = _entity.metaData.find(m => {
        const fieldName = String((m && m.fldname) || "").trim().toLowerCase();
        return fieldName === String(configuredKey || "").trim().toLowerCase();
      });
      if (configured) return configured;
    }

    const internal = new Set(['transid', 'recordid', 'ftransid']);
    const preferred = ["docid", "docno", "documentno", "code", "name", "username", "id"];

    for (let p of preferred) {
      const found = _entity.metaData.find(m => (m && m.fldname && String(m.fldname).toLowerCase() === String(p).toLowerCase()));
      if (found && !internal.has(String(found.fldname).toLowerCase())) return found;
    }

    const firstNonInternal = _entity.metaData.find(m => m && m.fldname && !internal.has(String(m.fldname).toLowerCase()));
    return firstNonInternal || _entity.metaData[0];
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

      const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
      const isGroupedView = !!(ctrl && Array.isArray(ctrl.groupby_columns) && ctrl.groupby_columns.length > 0);
      const sourceRows = Array.isArray(listJson) ? listJson.slice() : [];
      const rowsForView = isGroupedView ? smartviewGetGroupedViewSourceRows(sourceRows, ctrl) : sourceRows;

      if (rowCountManager) {
        const totalLoaded = rowsForView.length;
        rowCountManager.setLoadedRecords(totalLoaded);
        rowCountManager.setCurrentView('table');
        if (isGroupedView) {
          rowCountManager.setTotal(totalLoaded);
        }

        if (rowsForView.length < _entity.pageSize && rowsForView.length > 0) {
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
      let excludedFields = new Set(['transid', 'ftransid', 'recordid']);
    
    let hideTransid = !rowsForView.some(rowData => rowData[keyCol]);
    
      // Check if any row has axrowoptions to decide whether to show the column
      const hasRowOptions = rowsForView.some(rowData => {
        return rowData.axrowoptions || rowData.axRowOptions || rowData.axRowoptions;
      });
    
    const isEmptyDataset = !rowsForView || rowsForView.length === 0;
      let allowedFields = null;
      const visibleSelectedColumns = smartviewGetVisibleSelectedFieldColumns(ctrl);
      if (visibleSelectedColumns.length) {
        allowedFields = new Set();
        visibleSelectedColumns.forEach(sc => {
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
      rowsForView.forEach(rowData => {
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

      // If the previous metadata snapshot was the helper ADS schema, clean it before
      // key-column detection so fields like "sqlname" are not rendered as data columns.
      if (smartviewLooksLikeAdsMetadataDescriptorMeta(_entity.metaData || [])) {
        const fallbackMeta = smartviewBuildMetaFromDataRows(rowsForView, _entity.metaData || [], (ctrl && ctrl.adsName) || smartviewGetCurrentAdsForViews());
        if (fallbackMeta.length) {
          _entity.metaData = fallbackMeta;
          if (ctrl) {
            ctrl.lastAdsMeta = fallbackMeta;
            ctrl._adsMetaFor = ctrl.adsName || smartviewGetCurrentAdsForViews();
          }
          smartviewSyncKeyFieldState(fallbackMeta, ctrl);
          keyCol = _entity.keyField || '';
        }
      }
    
      // If keyField missing from metaData, try to infer a sensible one
      if (!keyCol || !_entity.metaData.some(field => {
        const fieldName = (field.fldname || '').toString().toLowerCase();
        return fieldName === keyCol.toLowerCase();
      })) {
        const keyField = getKeyField();
        keyCol = keyField ? keyField.fldname : _entity.keyField;
      }

      // recordid is internal; prefer a more meaningful key column if possible.
      if (keyCol && String(keyCol).toLowerCase() === 'recordid') {
        const keyField2 = getKeyField();
        if (keyField2 && keyField2.fldname) keyCol = keyField2.fldname;
      }
    
      const keyColLower = (keyCol || '').toString().toLowerCase();
      console.log('keyCol:', keyCol, 'keyColLower:', keyColLower);
    
      // Determine whether the key column should actually be rendered
      const keyFieldMeta = (_entity.metaData || []).find(f => (f.fldname || '').toString().toLowerCase() === keyColLower) || null;
      const keyFieldPresentInMeta = !!keyFieldMeta;
      const keyFieldHiddenInMeta = keyFieldMeta ? smartviewIsMetaFieldHidden(keyFieldMeta) : false;
      let keyFieldHasData = fieldsWithData.has(keyColLower) || (keyFieldPresentInMeta && !keyFieldHiddenInMeta); // MOVE THIS LINE UP
      if (allowedFields && keyColLower && !allowedFields.has(keyColLower)) keyFieldHasData = false;
      if (excludedFields.has(keyColLower)) keyFieldHasData = false;
      if (keyFieldHiddenInMeta) keyFieldHasData = false;
      console.log('keyFieldPresentInMeta:', keyFieldPresentInMeta, 'keyFieldHiddenInMeta:', keyFieldHiddenInMeta, 'keyFieldHasData:', keyFieldHasData);
    
      /*
        New metadata merging logic:
        - Start with existing _entity.metaData (if present)
        - Append any fields found in data (fieldsWithData) that are not present in metaData
        - Respect explicit hide === 'T' where possible
      */
      const metaMap = {};
      let originalMeta = Array.isArray(_entity.metaData) ? _entity.metaData.slice() : [];
      if (smartviewLooksLikeAdsMetadataDescriptorMeta(originalMeta)) {
        const fallbackMeta = smartviewBuildMetaFromDataRows(rowsForView, originalMeta, (ctrl && ctrl.adsName) || smartviewGetCurrentAdsForViews());
        if (fallbackMeta.length) {
          originalMeta = fallbackMeta;
          _entity.metaData = fallbackMeta;
          if (ctrl && smartviewLooksLikeAdsMetadataDescriptorMeta(ctrl.lastAdsMeta || [])) {
            ctrl.lastAdsMeta = fallbackMeta;
          }
        }
      }
      const aliasByOriginal = {};
      const hiddenMetaKeys = new Set();
    
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

        if (smartviewIsMetaFieldHidden(item)) {
          hiddenMetaKeys.add(metaKey);
          hiddenMetaKeys.add(effectiveKey);
          return;
        }

        aliasByOriginal[metaKey] = effectiveKey;
        const cloned = smartviewCloneMetaRow(item, { fldname: effectiveKey });
        if (effectiveKey !== metaKey) {
          cloned._svOriginalFldname = metaKey;
        }

        // If hyperlink mappings refer to the original field name, rewrite them to the effective key.
        if (cloned.tbl_hyperlink && effectiveKey !== metaKey) {
          try {
            const pairs = smartviewParseTblHyperlink(cloned.tbl_hyperlink);
            if (pairs && pairs.length) {
              const rebuilt = pairs.map(([p, k]) => {
                const kk = (k || '').toString().trim().toLowerCase() === metaKey ? effectiveKey : k;
                return `${p}|${kk}`;
              });
              cloned.tbl_hyperlink = rebuilt.join('^');
            }
          } catch (e) {}
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
        if (hiddenMetaKeys.has(String(f || '').toLowerCase())) return;
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
    
      // Apply persisted/active drag-drop column order (if any).
      const activeColumnOrder = (ctrl && Array.isArray(ctrl.column_order) && ctrl.column_order.length)
        ? ctrl.column_order
        : smartviewLoadColumnOrderFromStorage((ctrl && ctrl.adsName) || smartviewGetCurrentAdsForViews());
      const filteredMetaData = smartviewReorderMetaDataByColumnOrder(metaOrdered, activeColumnOrder);
      if (ctrl && (!Array.isArray(ctrl.column_order) || !ctrl.column_order.length) && Array.isArray(activeColumnOrder) && activeColumnOrder.length) {
        ctrl.column_order = smartviewParseColumnOrder(activeColumnOrder);
      }
    
      const addedFields = new Set();
      
      const dynamicFields = ['modifiedby', 'modifiedon', 'createdby', 'createdon'];
    
      // If there are modification fields present in rows, ensure they are present
      if (_entity.modificationFields && typeof _entity.modificationFields === 'string') {
        const modificationFieldsArray = _entity.modificationFields.split(",");
    
        if (modificationFieldsArray.length > 0) {
          dynamicFields.forEach(field => {
            if (modificationFieldsArray.includes(field)) {
              const fl = field.toLowerCase();
              if (hiddenMetaKeys.has(fl)) return;
              if (!filteredMetaData.some(m => m.fldname === fl)) {
                filteredMetaData.push({ fldname: fl, fldcap: formatFieldName(field), fdatatype: 't', cdatatype: 'Text', listingfld: 'T' });
              }
            }
          });
        }
      } else {
        dynamicFields.forEach(field => {
          const fieldLower = field.toLowerCase();
          if (hiddenMetaKeys.has(fieldLower)) return;
          if (rowsForView.some(rowData => rowData[fieldLower]) && !filteredMetaData.some(m => m.fldname === fieldLower)) {
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
          if (smartviewIsMetaFieldHidden(field)) return;
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

      if (isGroupedView) {
        try { smartviewResetGroupedNodeStore(); } catch (e) {}
        try { destroySmartviewDataTable(); } catch (e) {}

        const groupedHtml = smartviewRenderGroupedViewHTML(rowsForView, filteredMetaData, ctrl);
        tableBodyContainer.empty().append(groupedHtml);
        window._entity.listJson = rowsForView.slice();
        window._entity.filteredListJson = rowsForView.slice();

        if (rowCountManager) {
          try { rowCountManager.setTotal(rowsForView.length); } catch (e) {}
          try { rowCountManager.setLoadedRecords(rowsForView.length); } catch (e) {}
        }

        try {
          const groupedTable = tableBodyContainer.find('table').get(0) || document.querySelector('#table-body_Container table');
          if (groupedTable) {
            smartviewApplySavedColumnWidths(groupedTable);
            smartviewBindGroupedTableInteractions(groupedTable);
          }
        } catch (e) {}

        try { smartviewSyncTableFocusModeFromDom(); } catch (e) {}

        setTimeout(() => {
          attachSmartviewHyperlinkHandlers();
          attachSmartviewGroupExpandHandlers();
        }, 100);

        try { ensureSmartviewScrollSentinel(); } catch (e) {}
        try {
          if (window.smartTableController && typeof window.smartTableController.attachScrollListener === 'function') {
            setTimeout(() => { try { window.smartTableController.attachScrollListener(); } catch (e) {} }, 120);
          }
        } catch (e) {}

        console.log('=== createTableViewHTML completed (grouped view) ===');
        return;
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
          const keyCaption = keyField
            ? smartviewNormalizeCaption((keyField.fldcaption || keyField.caption || keyField.fldcap || ''), keyField.fldname)
            : formatFieldName(keyCol);
          const keyHeaderClass = smartviewIsNumericMetaField(keyField) ? 'sv-num-header' : '';
          html += buildSmartviewHeaderCell(keyCaption, keyColLower, keyHeaderClass);
        } // else: intentionally skip rendering the key column header
    
        // Render headers using the merged metadata but only for fields that actually have data and are not the key
        filteredMetaData.forEach(field => {
          const fieldName = (field.fldname || '').toString().toLowerCase();
          // skip key col
          if (fieldName === keyColLower) return;
          if (smartviewIsMetaFieldHidden(field)) return;
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
              const headerClass = smartviewIsNumericMetaField(field) ? 'sv-num-header' : '';
              html += buildSmartviewHeaderCell(smartviewNormalizeCaption((field.fldcaption || field.caption || field.fldcap || ''), field.fldname), fieldName, headerClass);
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

      const editConfig = smartviewExtractEditConfigFromMeta(filteredMetaData);
      const allowInlineEdit = !!editConfig.allowedit && !!editConfig.inlineEditEnabled;
      const editTransIds = smartviewParseNewTstructIds(editConfig.newforms_transid || editConfig.newforms || '');
      const inlineEditTransId = editTransIds.length
        ? editTransIds[0]
        : (editConfig.newforms_transid || editConfig.newforms || '').toString().trim();

      listJson.forEach((rowData, index) => {
        let rowLinkDesc = "";

        for (let f of _entity.metaData) {
          rowLinkDesc = smartviewBuildHyperlinkDescriptor(f, rowData);
          if (rowLinkDesc) break;
        }
        const fallbackEditLink = smartviewBuildFallbackEditLinkDescriptor(rowData, inlineEditTransId);
        const inlineEditLink = allowInlineEdit ? (rowLinkDesc || fallbackEditLink) : '';
        const showInlineEdit = allowInlineEdit && !!inlineEditLink;

        html += `<tr>`;
        html += `<td>`;
        if (isGroupedView) {
          html += `<button type="button" class="sv-expand-btn" data-rowindex="${index}" data-expanded="false">+</button>`;
        }
        html += `<input type="checkbox" class="rowCheckbox" data-index="${index}" data-recordid="${rowData.recordid || ''}">`;
        if (showInlineEdit) {
          html += ` <span class="row-arrow material-icons sv-hyperlinktemp" data-link="${escapeHtml(inlineEditLink)}">edit_note</span>`;
        }
        html += `</td>`;
        
        // ADD ACTION CELL
        if (hasRowOptions) {
          const actions = parseAxRowOptionsField(rowData);
          if (actions && actions.length > 0) {
            html += `<td class="action-cell" style="text-align: center; padding: 4px;">
                      <button type="button" class="action-arrow-btn" 
                              data-rowindex="${index}"
                              title="Actions"
                              style="border: none; background: none; cursor: pointer; padding: 2px 4px; border-radius: 4px;">
                        <span style="font-size: 18px;">Ã¢â€¹Â®</span>
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
    
            // Check if key field has hyperlink metadata - if so, render as hyperlink instead of plain text
            let keyCell = '';
            if (keyColProps && smartviewHasHyperlinkMeta(keyColProps)) {
              console.log('[smartviewTable] Key field has hyperlink metadata:', keyColProps.fldname);
              const linkDesc = smartviewBuildHyperlinkDescriptor(keyColProps, rowData);
              if (linkDesc) {
                const txt = (displayValue === null || displayValue === undefined) ? '' : String(displayValue);
                if (txt && txt.indexOf('<') === -1) {
                  const isInline = keyColProps && smartviewHasValue(keyColProps.hyp_inline) && smartviewFlagFromValue(keyColProps.hyp_inline, false);
                  const inlineAttr = isInline ? ' data-hyp-inline="1"' : '';
                  keyCell = `<a href="#" class="sv-hyperlink" data-link="${escapeHtml(linkDesc)}"${inlineAttr}>${escapeHtml(txt)}</a>`;
                } else {
                  keyCell = escapeHtml(displayValue);
                }
              } else {
                keyCell = escapeHtml(displayValue);
              }
            } else {
              // Key column should render as plain text (not a hyperlink) if no hyperlink metadata
              const keyText = _entityCommon.inValid(displayValue) ? "--" : String(displayValue);
              keyCell = escapeHtml(keyText);
            }
            
            const keyCellClass = smartviewIsNumericMetaField(keyColProps) ? ' class="sv-num-cell"' : '';
            html += `<td${keyCellClass}>${keyCell}</td>`;
          } else {
            // metadata says key exists but no data in rows: render empty cell
            html += `<td></td>`;
          }
        } // else: skip rendering key cell entirely (keeps column alignment by not outputting a placeholder)
    
        // Render cells using merged metadata order and only for fields that have data or are explicitly listed
        filteredMetaData.forEach(field => {
          const fieldName = (field.fldname || '').toString().toLowerCase();
          if (fieldName === keyColLower) return;
          if (smartviewIsMetaFieldHidden(field)) return;
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

          const isNumericField = smartviewIsNumericMetaField(field);
          const cellClass = isNumericField ? ' class="sv-num-cell"' : '';

          // SmartView hyperlinks: link only the first hyperlink-enabled visible column.
          // Also avoid duplicate links when the row expand icon (sv-hyperlinktemp) already points to the same tstruct.
          try {
            const canLinkCell = smartviewHasHyperlinkMeta(field);
            let linkDesc = canLinkCell ? smartviewBuildHyperlinkDescriptor(field, rowData) : '';
            
            // DEBUG: Log hyperlink creation
            if (!linkDesc && canLinkCell) {
              console.warn('[smartviewTable] Hyperlink metadata present but descriptor empty for field:', field.fldname, 'Field meta:', field);
            }
            
            if (linkDesc) {
              const txt = (cellContent === null || cellContent === undefined) ? '' : String(cellContent);
              // Don't wrap HTML widgets (checkboxes, etc.)
              const isDuplicateRowLink = showInlineEdit && !!inlineEditLink && inlineEditLink === linkDesc;
              if (txt && txt.indexOf('<') === -1 && !isDuplicateRowLink) {
                const isInline = field && smartviewHasValue(field.hyp_inline) && smartviewFlagFromValue(field.hyp_inline, false);
                const inlineAttr = isInline ? ' data-hyp-inline="1"' : '';
                cellContent = `<a href="#" class="sv-hyperlink" data-link="${escapeHtml(linkDesc)}"${inlineAttr}>${escapeHtml(txt)}</a>`;
              }
            }
          } catch (e) {
            console.error('[smartviewTable] Hyperlink rendering error:', e);
          }
    
          html += `<td${cellClass}>${cellContent}</td>`;
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

      try {
        const tableEl = (tableBodyContainer && tableBodyContainer.length)
          ? tableBodyContainer.find('table').get(0)
          : document.querySelector('#table-body_Container table');
        if (tableEl) {
          smartviewApplySavedColumnWidths(tableEl);
          smartviewBindColumnResizeHandlers(tableEl);
          smartviewBindColumnReorderHandlers(tableEl);
        }
      } catch (e) {}

      try { smartviewSyncTableFocusModeFromDom(); } catch (e) {}
      
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

        const $btn = $(this);
        const $tr = $btn.closest('tr');
        const $next = $tr.next('.sv-group-detail-row');
        if (!$next.length) return false;

        const isExpanded = String($btn.attr('data-expanded') || '').toLowerCase() === 'true';
        if (isExpanded) {
          $next.hide();
          $btn.text('+').attr('data-expanded', 'false');
          return false;
        }

        const nodeId = String($btn.attr('data-sv-group-node-id') || $tr.attr('data-sv-group-node-id') || '').trim();
        if (!nodeId || !smartviewExpandGroupedNodeById(nodeId)) {
          return false;
        }

        $next.show();
        $btn.text('-').attr('data-expanded', 'true');

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
        if (!link) {
          console.warn('[smartviewTable] Hyperlink clicked but data-link attribute is empty');
          return false;
        }

        // If hyp_inline is set, trigger inline expand below the row instead of popup
        if (this.getAttribute('data-hyp-inline') === '1') {
          console.log('[smartviewTable] Inline hyperlink clicked:', link);
          smartviewOpenInlineHyperlink(this, link);
          return false;
        }

        console.log('[smartviewTable] Opening hyperlink:', link);
        if (typeof openLinkInPopup === 'function') {
          openLinkInPopup(link);
        } else {
          console.warn('[smartviewTable] openLinkInPopup function not found, using window.open');
          window.open(link, '_blank');
        }
        return false;
      });
    } catch (e) {
      console.error('[smartviewTable] Error attaching hyperlink handlers:', e);
    }
  }

  /**
   * Opens a hyperlink inline below the row (instead of a popup).
   * Triggered when a cell link has data-hyp-inline="1".
   * Reuses the same expand-row / iframe pattern as sv-hyperlinktemp.
   */
  function smartviewOpenInlineHyperlink(anchorEl, link) {
    try {
      if (!window.jQuery) return;
      var $anchor  = $(anchorEl);
      var $tr      = $anchor.closest('tr');
      if (!$tr.length) return;

      var $nextRow   = $tr.next('.expand-row');
      var $tableWrap = $tr.closest('.table-responsive');
      var tableScrollLeft = ($tableWrap && $tableWrap.length) ? $tableWrap.scrollLeft() : 0;

      // Create expand row if it doesn't exist yet
      if ($nextRow.length === 0) {
        var colspan = $tr.children('td').length;
        $nextRow = $([
          '<tr class="expand-row">',
          '  <td colspan="' + colspan + '">',
          '    <div class="iframe-loader-wrapper" style="opacity:0;transition:opacity 0.3s ease-in-out;">',
          '      <iframe class="tstruct-frame"',
          '              style="width:100%;height:450px;border:1px solid #eee;',
          '                     box-shadow:0 2px 10px rgba(0,0,0,.1);"></iframe>',
          '    </div>',
          '  </td>',
          '</tr>'
        ].join('')).hide();
        $tr.after($nextRow);
      }

      var $wrapper   = $nextRow.find('.iframe-loader-wrapper');
      var $iframe    = $nextRow.find('iframe');
      var isExpanded = $nextRow.hasClass('sv-expanded');

      if (isExpanded) {
        // Check if clicking same link or different link
        var iframeEl   = $iframe[0];
        var currentSrc = iframeEl ? iframeEl.src : '';
        var newUrl     = (typeof openLinkInPopup === 'function') ? (openLinkInPopup(link, true, { tstructMode: 'load' }) || '') : '';

        var currentTransid = (typeof getParamFromUrl === 'function') ? getParamFromUrl(currentSrc, 'transid') : '';
        var currentRecid   = (typeof getParamFromUrl === 'function') ? getParamFromUrl(currentSrc, 'docid') : '';
        var newTransid     = (typeof getParamFromUrl === 'function') ? getParamFromUrl(newUrl, 'transid') : '';
        var newRecid       = (typeof getParamFromUrl === 'function') ? getParamFromUrl(newUrl, 'docid') : '';

        if (newUrl && (currentTransid !== newTransid || currentRecid !== newRecid)) {
          // Different link clicked — switch the iframe source
          if (typeof saveTstructBeforeCollapse === 'function') {
            try { saveTstructBeforeCollapse(iframeEl, currentTransid, currentRecid); } catch (e) {}
          }
          $wrapper.css('opacity', '0');
          $iframe.attr('src', newUrl);
          smartviewBindInlineIframeLoad($iframe, $wrapper, $tableWrap, tableScrollLeft);
          return;
        }

        // Same link — collapse
        if (typeof saveTstructBeforeCollapse === 'function') {
          try { saveTstructBeforeCollapse(iframeEl, currentTransid, currentRecid); } catch (e) {}
        }
        $nextRow.removeClass('sv-expanded').hide();
        $wrapper.css('opacity', '0');
        if (typeof smartviewExitTableFocusMode === 'function') {
          try { smartviewExitTableFocusMode(false); } catch (e) {}
        }
      } else {
        // Expand
        var url = (typeof openLinkInPopup === 'function') ? openLinkInPopup(link, true, { tstructMode: 'load' }) : '';
        if (url) {
          $wrapper.css('opacity', '0');
          $iframe.attr('src', url);
          smartviewBindInlineIframeLoad($iframe, $wrapper, $tableWrap, tableScrollLeft);
        }
        $nextRow.addClass('sv-expanded').show();
        if (typeof smartviewEnterTableFocusMode === 'function') {
          try { smartviewEnterTableFocusMode(); } catch (e) {}
        }
      }
    } catch (err) {
      console.error('[smartviewTable] smartviewOpenInlineHyperlink error:', err);
    }
  }

  /**
   * Binds iframe load handler to hide toolbar/footer and inject tstruct helpers.
   */
  function smartviewBindInlineIframeLoad($iframe, $wrapper, $tableWrap, tableScrollLeft) {
    $iframe.off('load.svInline').on('load.svInline', function () {
      var iframeEl = this;
      try {
        var iframeDoc = iframeEl.contentDocument || iframeEl.contentWindow.document;
        if (!iframeDoc) { $wrapper.css('opacity', '1'); return; }

        var iTransid = (typeof getParamFromUrl === 'function') ? getParamFromUrl(iframeEl.src, 'transid') : '';
        var iRecid   = (typeof getParamFromUrl === 'function') ? getParamFromUrl(iframeEl.src, 'docid') : '';

        if (typeof injectTstructJsonLiteIntoIframe === 'function') {
          try { injectTstructJsonLiteIntoIframe(iframeEl); } catch (e) {}
        }
        if (typeof attachIframeChangeListener === 'function') {
          try { attachIframeChangeListener(iframeEl, iTransid, iRecid); } catch (e) {}
        }

        // Hide tstruct toolbar / footer inside iframe
        var style = iframeDoc.createElement('style');
        style.innerHTML = [
          '#dvlayout .footer,',
          '#dvlayout .toolbar,',
          '#dvlayout .toolbarRightMenu,',
          '.breadcrumb-panel { display:none !important; }',
          'html, body { overflow-x:hidden !important; max-width:100% !important; }'
        ].join('\n');
        (iframeDoc.head || iframeDoc.documentElement).appendChild(style);

        var hideEls = function () {
          $(iframeDoc)
            .find('#dvlayout .footer, #dvlayout .toolbar, #dvlayout .toolbarRightMenu')
            .css('display', 'none');
        };
        hideEls();

        var lockIframeXScroll = function () {
          try {
            if (iframeDoc.documentElement) iframeDoc.documentElement.scrollLeft = 0;
            if (iframeDoc.body) iframeDoc.body.scrollLeft = 0;
            if (iframeEl.contentWindow && iframeEl.contentWindow.scrollX) {
              iframeEl.contentWindow.scrollTo(0, iframeEl.contentWindow.scrollY || 0);
            }
            if ($tableWrap && $tableWrap.length) $tableWrap.scrollLeft(tableScrollLeft);
          } catch (e) {}
        };
        lockIframeXScroll();
        try {
          iframeDoc.addEventListener('focusin', function () {
            setTimeout(lockIframeXScroll, 0);
          }, true);
        } catch (e) {}

        var obs = new MutationObserver(hideEls);
        obs.observe(iframeDoc.body, { childList: true, subtree: true });

        setTimeout(function () { $wrapper.css('opacity', '1'); }, 50);
      } catch (err) {
        console.warn('[smartviewTable] inline iframe DOM access failed:', err.message);
        $wrapper.css('opacity', '1');
      }
    });
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
      #table-body_Container .table > thead th.sv-num-header,
      #table-body_Container .table > tbody td.sv-num-cell {
        text-align: right;
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
      width: 28px;
      min-width: 28px;
      height: 28px;
      line-height: 28px;
      text-align: center;
      padding: 0;
      opacity: 1;
      pointer-events: auto;
      transition: opacity 0.15s ease, background 0.15s ease;
      flex-shrink: 0;
    }
    .sv-data-header .sv-header-menu-icon {
      display: inline-block;
      width: 100%;
      pointer-events: none;
    }
      .sv-data-header .sv-header-menu-btn:hover {
        background: #f0f0f0;
        color: #333;
      }
      #table-body_Container .table > thead th.sv-resizable-header {
        position: relative;
        padding-right: 14px;
      }
      #table-body_Container .table > thead th.sv-draggable-header {
        cursor: grab;
      }
      #table-body_Container .table > thead th.sv-draggable-header.sv-col-dragging {
        opacity: 0.7;
        cursor: grabbing;
        outline: 2px dashed #3b82f6;
        outline-offset: -2px;
      }
      .sv-col-resizer {
        position: absolute;
        top: 0;
        right: 0;
        width: 8px;
        height: 100%;
        cursor: col-resize;
        user-select: none;
        touch-action: none;
        opacity: 0;
        z-index: 3;
      }
      #table-body_Container .table > thead th.sv-resizable-header:hover .sv-col-resizer {
        opacity: 1;
      }
      body.sv-col-resizing,
      body.sv-col-resizing * {
        cursor: col-resize !important;
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
      .sv-grouped-table .sv-group-toggle-cell {
        width: 40px;
        white-space: nowrap;
      }
      .sv-grouped-table .sv-group-row:hover {
        background: #f8fbff;
      }
      .sv-grouped-table .sv-group-detail-row > td {
        padding: 0 0 8px 0;
        border-top: none;
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
      .sv-group-detail thead th.sv-num-header,
      .sv-group-detail tbody td.sv-num-cell {
        text-align: right;
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
    const groupedCols = (typeof smartviewGetEffectiveGroupbyColumns === 'function')
      ? smartviewGetEffectiveGroupbyColumns(ctrl)
      : ((ctrl && Array.isArray(ctrl.groupby_columns)) ? ctrl.groupby_columns : []);
    const grouped = !!groupedCols.some(f => String(f).toLowerCase() === String(fieldName).toLowerCase());
    const hasAnyGrouping = groupedCols.length > 0;

    const menu = document.createElement('div');
    menu.id = 'svHeaderMenu';
    menu.className = 'sv-header-menu';
    menu.innerHTML = `
      <button type="button" class="sv-header-menu-item" data-action="sort_asc" data-field="${escapeHtml(fieldName)}">Sort Ascending</button>
      <button type="button" class="sv-header-menu-item" data-action="sort_desc" data-field="${escapeHtml(fieldName)}">Sort Descending</button>
      <button type="button" class="sv-header-menu-item" data-action="group_toggle" data-field="${escapeHtml(fieldName)}">${grouped ? 'Remove This Group By' : 'Group By'}</button>
      ${hasAnyGrouping && groupedCols.length > 1 ? `<button type="button" class="sv-header-menu-item" data-action="group_clear" data-field="${escapeHtml(fieldName)}">Clear All Grouping</button>` : ''}
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
      const shouldApplyClientFilters = !!(ctrl && ctrl.forceClientFiltering && activeFilters.length > 0);
      const filteredRows = shouldApplyClientFilters ? applySmartviewFiltersToRows(rows, activeFilters) : rows;
      const effectiveTotal = shouldApplyClientFilters ? filteredRows.length : totalRecords;

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
      const activeAdsName = String(pageTitle || (ctrl && ctrl.adsName) || (window._entity && window._entity.adsName) || '').trim();
      const liveAdsMeta = (ctrl && Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length &&
        !smartviewLooksLikeAdsMetadataDescriptorMeta(ctrl.lastAdsMeta) &&
        (!ctrl._adsMetaFor || !activeAdsName || String(ctrl._adsMetaFor).toLowerCase() === activeAdsName.toLowerCase()))
        ? smartviewCloneJsonSafe(ctrl.lastAdsMeta)
        : null;

      const pageSz = Number(pageSize ?? window._entity.pageSize ?? 0);
      if (pageSz > 0) {
        // only render the first page (pageNo may be 1 on initial load)
        const fromIndex = ((Number(pageNo) || 1) - 1) * pageSz;
        window._entity.listJson = Array.isArray(filteredRows) ? filteredRows.slice(fromIndex, fromIndex + pageSz) : [];
      } else {
        // pagesize === 0 means "render all"
        window._entity.listJson = Array.isArray(filteredRows) ? filteredRows.slice() : [];
      }
      window._entity.pageSize = Number(pageSize || window._entity.pageSize || 10);

      // --- Build metadata:
      // 1) Prefer live ADS metadata if we already fetched it
      // 2) Then use dsBlock.columns if provided by the ADS
      // 3) Otherwise infer metadata from the keys of the first row
      if (liveAdsMeta && liveAdsMeta.length) {
        window._entity.metaData = liveAdsMeta;
      } else if (dsBlock && Array.isArray(dsBlock.columns) && dsBlock.columns.length) {
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

      smartviewSyncKeyFieldState(window._entity.metaData, ctrl);

      // ensure keyField is set sensibly
      const currentKeyField = String((window._entity && window._entity.keyField) || "").trim().toLowerCase();
      const hasCurrentKeyField = currentKeyField && window._entity.metaData.some(function (m) {
        return String((m && m.fldname) || "").trim().toLowerCase() === currentKeyField;
      });
      if (!hasCurrentKeyField) {
        const internal = new Set(['transid', 'recordid', 'ftransid']);
        const preferred = ['docid', 'docno', 'name', 'username', 'id'];
        let keyCandidate =
          window._entity.metaData.find(m => preferred.includes((m.fldname || '').toLowerCase())) ||
          window._entity.metaData.find(m => (m.fldname && !internal.has(String(m.fldname).toLowerCase()))) ||
          null;
        window._entity.keyField = keyCandidate ? keyCandidate.fldname : (window._entity.metaData[0] ? window._entity.metaData[0].fldname : 'docid');
      }
      smartviewSyncKeyFieldState(window._entity.metaData, ctrl);

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
      for (let i = 0; i < 6; i++) {
        if (typeof parsed === 'string') {
          const raw = parsed.trim();
          if (!raw) break;
          try {
            const next = JSON.parse(raw);
            if (next === parsed) break;
            parsed = next;
            continue;
          } catch (e) {
            break;
          }
        }

        if (parsed && typeof parsed === 'object' && parsed.d !== undefined) {
          if (typeof parsed.d === 'string') {
            try {
              parsed = JSON.parse(parsed.d);
              continue;
            } catch (e) {
              // keep trying with current parsed object
            }
          } else if (parsed.d && typeof parsed.d === 'object' && !parsed.result) {
            parsed = parsed.d;
            continue;
          }
        }
        break;
      }

      return parsed || {};
    } catch (err) {
      console.warn('safeParseAxResponse failed', err, resp);
      return {};
    }
  }

  function smartviewResolveAxListCaller() {
    const scopes = [];
    try { if (typeof parent !== 'undefined') scopes.push(parent); } catch (e) {}
    try { scopes.push(window); } catch (e) {}
    try {
      if (window.top && window.top !== window) scopes.push(window.top);
    } catch (e) {}
    return scopes.find(w => w && typeof w.GetDataFromAxList === 'function') || null;
  }

  function smartviewExtractRowsFromAxListResponse(resp) {
    const parsed = (typeof safeParseAxResponse === 'function') ? safeParseAxResponse(resp) : (resp || {});
    const dsBlock = parsed?.result?.data?.[0] || parsed?.data?.[0] || parsed?.result || parsed || {};
    let rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
    if (rows && rows.length && rows[0] && Array.isArray(rows[0].data)) rows = rows[0].data;
    return {
      parsed,
      dsBlock,
      rows: Array.isArray(rows) ? rows : []
    };
  }

  function smartviewNormalizeMetricName(name) {
    const raw = (name === null || name === undefined) ? '' : String(name).trim();
    if (!raw) return '';
    return formatFieldName(raw);
  }

  function smartviewGetObjectValueCI(obj, keys) {
    if (!obj || typeof obj !== 'object') return '';
    const entries = Object.getOwnPropertyNames(obj || {});
    for (let i = 0; i < keys.length; i++) {
      const target = String(keys[i] || '').toLowerCase();
      const hit = entries.find(k => String(k || '').toLowerCase() === target);
      if (!hit) continue;
      const v = obj[hit];
      if (v === undefined || v === null) continue;
      const s = String(v).trim();
      if (!s) continue;
      return v;
    }
    return '';
  }

  function smartviewHasValue(v) {
    if (v === null || v === undefined) return false;
    const s = String(v).trim();
    if (!s) return false;
    const l = s.toLowerCase();
    return !(l === 'null' || l === 'undefined');
  }

  function smartviewResolveNodeFromMetaRows(rows, keys) {
    const arr = Array.isArray(rows) ? rows : [];
    for (let i = 0; i < arr.length; i++) {
      const v = smartviewGetObjectValueCI(arr[i], keys);
      if (smartviewHasValue(v)) return v;
    }
    return '';
  }

  function smartviewResolveAdsMetaNodeValue(metaRows, dsBlock, parsed, keys) {
    const v1 = smartviewGetObjectValueCI(dsBlock || {}, keys);
    if (smartviewHasValue(v1)) return v1;
    const v2 = smartviewGetObjectValueCI((parsed && parsed.result) ? parsed.result : {}, keys);
    if (smartviewHasValue(v2)) return v2;
    const v3 = smartviewGetObjectValueCI(parsed || {}, keys);
    if (smartviewHasValue(v3)) return v3;
    const v4 = smartviewResolveNodeFromMetaRows(metaRows, keys);
    if (smartviewHasValue(v4)) return v4;
    return '';
  }

  function smartviewAllowEditFromValue(raw) {
    if (raw === true) return true;
    if (raw === false || raw === null || raw === undefined) return false;
    const s = String(raw).trim().toLowerCase();
    if (!s || s === 'null' || s === 'undefined') return false;
    if (['f', 'false', '0', 'n', 'no'].includes(s)) return false;
    return true;
  }

  function smartviewFlagFromValue(raw, defaultValue) {
    if (raw === true) return true;
    if (raw === false) return false;
    if (raw === null || raw === undefined) return !!defaultValue;

    const s = String(raw).trim().toLowerCase();
    if (!s || s === "null" || s === "undefined") return !!defaultValue;
    if (["t", "true", "1", "y", "yes"].includes(s)) return true;
    if (["f", "false", "0", "n", "no"].includes(s)) return false;
    return !!defaultValue;
  }

  function smartviewIsMetaFieldHidden(fieldMeta) {
    if (!fieldMeta || typeof fieldMeta !== 'object') return false;

    const hideRaw = smartviewGetObjectValueCI(fieldMeta, ['hide', 'col_hide', 'colhide', 'hidden']);
    if (smartviewHasValue(hideRaw)) return smartviewFlagFromValue(hideRaw, false);

    const listingRaw = smartviewGetObjectValueCI(fieldMeta, ['listingfld', 'listing_fld']) || fieldMeta.listingfld;
    if (smartviewHasValue(listingRaw)) {
      const token = String(listingRaw).trim().toLowerCase();
      if (token === 'f' || token === 'false' || token === '0' || token === 'n' || token === 'no') return true;
    }

    return false;
  }

  function smartviewIsMetaFieldFilterEnabled(fieldMeta, defaultValue) {
    if (!fieldMeta || typeof fieldMeta !== 'object') return !!defaultValue;
    // Product metadata can contain both `filters` and `col_filter` with conflicting values.
    // Prefer explicit column-filter flags when present.
    const colRaw = smartviewGetObjectValueCI(fieldMeta, ['col_filter', 'colfilter', 'applyfilter', 'apply_filter']);
    if (smartviewHasValue(colRaw)) return smartviewFlagFromValue(colRaw, false);

    const raw = smartviewGetObjectValueCI(fieldMeta, ['filters']);
    if (!smartviewHasValue(raw)) return !!defaultValue;
    return smartviewFlagFromValue(raw, false);
  }

  // Keep this on window so runtime debugging/overrides use the same implementation.
  try { window.smartviewIsMetaFieldFilterEnabled = smartviewIsMetaFieldFilterEnabled; } catch (e) {}

  function smartviewBuildFilterUiMetaField(item, options) {
    if (!item || typeof item !== 'object') return null;
    const opts = options && typeof options === 'object' ? options : {};

    const isHidden = smartviewIsMetaFieldHidden(item);
    const isFilterEnabled = smartviewIsMetaFieldFilterEnabled(item, false);

    if (isHidden) {
      // If column is hidden, it must be explicitly enabled for filtering (filter flag = T / Yes)
      if (!isFilterEnabled) return null;
    } else {
      // If not hidden, normal rules apply (it must not be explicitly disabled for filtering)
      if (!opts.ignoreFilterFlag && !smartviewIsMetaFieldFilterEnabled(item, true)) return null;
    }

    const fldname = String((item.fldname || item.fieldname || item.name || '')).trim();
    if (!fldname) return null;

    const rawCaption = item.fldcaption || item.caption || item.fldcap || item.title || item.displayname || item.fieldcaption || '';
    const normalizedCaption = smartviewNormalizeCaption(rawCaption, fldname) || formatFieldName(fldname) || fldname;
    const hasSourceDropdown = smartviewHasSourceDropdownHints(item) || smartviewFlagFromValue(item.normalized, false);

    // Keep the user-facing fields enumerable, but tuck SmartView helper metadata away
    // so setup/filter screens do not render the internal properties as editable fields.
    const uiItem = {
      fldname: fldname,
      fldcap: normalizedCaption,
      fldcaption: normalizedCaption,
      caption: normalizedCaption
    };

    let cdatatype = smartviewHasValue(item.cdatatype) ? String(item.cdatatype).trim() : '';
    const cd = cdatatype.toLowerCase().replace(/[\s_]+/g, '');
    if (cd === 'dropdown' || cd === 'select') cdatatype = 'DropDown';
    else if (hasSourceDropdown) cdatatype = 'DropDown';

    smartviewDefineMetaProp(uiItem, 'cdatatype', cdatatype, false);
    smartviewDefineMetaProp(uiItem, 'hide', smartviewHasValue(item.hide) ? String(item.hide).trim() : '', false);
    smartviewDefineMetaProp(uiItem, 'keyfield', smartviewHasValue(item.keyfield) ? String(item.keyfield).trim() : '', false);
    smartviewDefineMetaProp(uiItem, 'filters', smartviewHasValue(item.filters) ? String(item.filters).trim() : '', false);
    smartviewDefineMetaProp(uiItem, 'normalized', smartviewHasValue(item.normalized) ? String(item.normalized).trim() : '', false);
    smartviewDefineMetaProp(uiItem, 'srctable', smartviewHasValue(item.srctable) ? String(item.srctable).trim() : (smartviewHasValue(item.sourcetable) ? String(item.sourcetable).trim() : ''), false);
    smartviewDefineMetaProp(uiItem, 'srcfld', smartviewHasValue(item.srcfld) ? String(item.srcfld).trim() : (smartviewHasValue(item.sourcefld) ? String(item.sourcefld).trim() : ''), false);
    smartviewDefineMetaProp(uiItem, 'sourcefld', smartviewHasValue(item.sourcefld) ? String(item.sourcefld).trim() : (smartviewHasValue(item.srcfld) ? String(item.srcfld).trim() : ''), false);
    smartviewDefineMetaProp(uiItem, 'sourcetable', smartviewHasValue(item.sourcetable) ? String(item.sourcetable).trim() : (smartviewHasValue(item.srctable) ? String(item.srctable).trim() : ''), false);
    smartviewDefineMetaProp(uiItem, 'psrctxt', smartviewHasValue(item.psrctxt) ? String(item.psrctxt).trim() : '', false);

    const hiddenProps = {
      fdatatype: smartviewHasValue(item.fdatatype) ? String(item.fdatatype).trim() : '',
      datatype: smartviewHasValue(item.datatype) ? String(item.datatype).trim() : '',
      listingfld: smartviewHasValue(item.listingfld) ? String(item.listingfld).trim() : '',
      col_hide: smartviewHasValue(item.col_hide) ? String(item.col_hide).trim() : '',
      col_filter: smartviewHasValue(item.col_filter) ? String(item.col_filter).trim() : '',
      ftransid: smartviewHasValue(item.ftransid) ? String(item.ftransid).trim() : '',
      allowedit: smartviewHasValue(item.allowedit) ? String(item.allowedit).trim() : '',
      allownew: smartviewHasValue(item.allownew) ? String(item.allownew).trim() : '',
      bulksave: smartviewHasValue(item.bulksave) ? String(item.bulksave).trim() : '',
      dataupload: smartviewHasValue(item.dataupload) ? String(item.dataupload).trim() : '',
      validatedata: smartviewHasValue(item.validatedata) ? String(item.validatedata).trim() : '',
      editmode: smartviewHasValue(item.editmode) ? String(item.editmode).trim() : '',
      savemode: smartviewHasValue(item.savemode) ? String(item.savemode).trim() : '',
      newforms: smartviewHasValue(item.newforms) ? String(item.newforms).trim() : '',
      newforms_transid: smartviewHasValue(item.newforms_transid) ? String(item.newforms_transid).trim() : '',
      sv_name: smartviewHasValue(item.sv_name) ? String(item.sv_name).trim() : '',
      sv_caption: smartviewHasValue(item.sv_caption) ? String(item.sv_caption).trim() : '',
      sv_sourcecnd: smartviewHasValue(item.sv_sourcecnd) ? String(item.sv_sourcecnd).trim() : '',
      sv_keycol: smartviewHasValue(item.sv_keycol) ? String(item.sv_keycol).trim() : '',
      api_config: smartviewHasValue(item.api_config) ? item.api_config : '',
      memdb_key: smartviewHasValue(item.memdb_key) ? String(item.memdb_key).trim() : '',
      options: Array.isArray(item.options)
        ? smartviewCloneJsonSafe(item.options)
        : (smartviewHasValue(item.options) ? item.options : undefined),
      sqlname: smartviewHasValue(item.sqlname) ? String(item.sqlname).trim() : '',
      hyp_structtype: smartviewHasValue(item.hyp_structtype) ? String(item.hyp_structtype).trim() : '',
      hyp_transid: smartviewHasValue(item.hyp_transid) ? String(item.hyp_transid).trim() : '',
      tbl_hyperlink: smartviewHasValue(item.tbl_hyperlink) ? String(item.tbl_hyperlink).trim() : '',
      hyp_inline: smartviewHasValue(item.hyp_inline) ? String(item.hyp_inline).trim() : '',
      dynamiccolumns: smartviewHasValue(item.dynamiccolumns) ? String(item.dynamiccolumns).trim() : '',
      pagination: smartviewHasValue(item.pagination) ? String(item.pagination).trim() : '',
      sorting: smartviewHasValue(item.sorting) ? String(item.sorting).trim() : ''
    };

    Object.keys(hiddenProps).forEach(function (key) {
      const value = hiddenProps[key];
      if (!smartviewHasValue(value) && value !== 0 && value !== false) return;
      Object.defineProperty(uiItem, key, {
        value: value,
        enumerable: false,
        configurable: true,
        writable: true
      });
    });

    return uiItem;
  }

  function smartviewExtractEditConfigFromMeta(metaData) {
    const arr = Array.isArray(metaData) ? metaData : [];
    let alloweditRaw = '';
    let allownewRaw = '';
    let bulksaveRaw = '';
    let datauploadRaw = '';
    let validatedataRaw = '';
    let editmodeRaw = '';
    let savemodeRaw = '';
    let newforms = '';
    let newformsTransId = '';
    let svName = '';
    let svCaption = '';
    let svSourceCndRaw = '';
    let svKeycol = '';
    let sqlname = '';
    let apiConfig = '';
    let memdbKey = '';

    arr.forEach(m => {
      if (!m || typeof m !== 'object') return;
      if (!smartviewHasValue(alloweditRaw)) {
        const v = smartviewGetObjectValueCI(m, ['allowedit', 'allow_edit']);
        if (smartviewHasValue(v)) alloweditRaw = v;
      }
      if (!smartviewHasValue(allownewRaw)) {
        const v = smartviewGetObjectValueCI(m, ['allownew', 'allow_new']);
        if (smartviewHasValue(v)) allownewRaw = v;
      }
      if (!smartviewHasValue(bulksaveRaw)) {
        const v = smartviewGetObjectValueCI(m, ['bulksave', 'bulk_save', 'bulkSave']);
        if (smartviewHasValue(v)) bulksaveRaw = v;
      }
      if (!smartviewHasValue(datauploadRaw)) {
        const v = smartviewGetObjectValueCI(m, ['dataupload', 'allowupload', 'upload']);
        if (smartviewHasValue(v)) datauploadRaw = v;
      }
      if (!smartviewHasValue(validatedataRaw)) {
        const v = smartviewGetObjectValueCI(m, ['validatedata', 'validate_data', 'validateupload', 'validate_upload']);
        if (smartviewHasValue(v)) validatedataRaw = v;
      }
      if (!smartviewHasValue(editmodeRaw)) {
        const v = smartviewGetObjectValueCI(m, ['editmode', 'edit_mode']);
        if (smartviewHasValue(v)) editmodeRaw = v;
      }
      if (!smartviewHasValue(savemodeRaw)) {
        const v = smartviewGetObjectValueCI(m, ['savemode', 'save_mode']);
        if (smartviewHasValue(v)) savemodeRaw = v;
      }
      if (!smartviewHasValue(newforms)) {
        const v = smartviewGetObjectValueCI(m, ['newforms', 'new_forms']);
        if (smartviewHasValue(v)) newforms = v;
      }
      if (!smartviewHasValue(newformsTransId)) {
        const v = smartviewGetObjectValueCI(m, ['newforms_transid', 'newforms_trans_id', 'newformstransid']);
        if (smartviewHasValue(v)) newformsTransId = v;
      }
      if (!smartviewHasValue(svName)) {
        const v = smartviewGetObjectValueCI(m, ['sv_name', 'viewname', 'smartviewname']);
        if (smartviewHasValue(v)) svName = v;
      }
      if (!smartviewHasValue(svCaption)) {
        const v = smartviewGetObjectValueCI(m, ['sv_caption', 'viewcaption', 'smartviewcaption']);
        if (smartviewHasValue(v)) svCaption = v;
      }
      if (!smartviewHasValue(svSourceCndRaw)) {
        const v = smartviewGetObjectValueCI(m, ['sv_sourcecnd', 'sourcecnd', 'source_cnd', 'sourcetype', 'source_type']);
        if (smartviewHasValue(v)) svSourceCndRaw = v;
      }
      if (!smartviewHasValue(svKeycol)) {
        const v = smartviewGetObjectValueCI(m, ['sv_keycol', 'svkeycol', 'keycolumn', 'key_column']);
        if (smartviewHasValue(v)) svKeycol = v;
      }
      if (!smartviewHasValue(sqlname)) {
        const v = smartviewGetObjectValueCI(m, ['sqlname', 'adsname', 'ads_name']);
        if (smartviewHasValue(v)) sqlname = v;
      }
      if (!smartviewHasValue(apiConfig)) {
        const v = smartviewGetObjectValueCI(m, ['api_config', 'apiconfig', 'api']);
        if (smartviewHasValue(v)) apiConfig = v;
      }
      if (!smartviewHasValue(memdbKey)) {
        const v = smartviewGetObjectValueCI(m, ['memdb_key', 'memdbkey', 'inmem_key', 'inmemkey', 'redis_key', 'rediskey']);
        if (smartviewHasValue(v)) memdbKey = v;
      }
    });

    const editMode = smartviewNormalizeEditMode(editmodeRaw);
    const saveMode = smartviewNormalizeSaveMode(savemodeRaw);
    const sourceType = smartviewNormalizeSourceType(svSourceCndRaw);
    const hasNewFormConfig = smartviewHasValue(newforms) || smartviewHasValue(newformsTransId);
    const svNameSafe = smartviewHasValue(svName) ? String(svName).trim() : '';
    const memdbKeySafe = smartviewHasValue(memdbKey) ? String(memdbKey).trim() : (svNameSafe ? `data_${svNameSafe}` : '');

    return {
      alloweditRaw: smartviewHasValue(alloweditRaw) ? String(alloweditRaw).trim() : '',
      allowedit: smartviewAllowEditFromValue(alloweditRaw),
      allownewRaw: smartviewHasValue(allownewRaw) ? String(allownewRaw).trim() : '',
      allownew: smartviewFlagFromValue(allownewRaw, hasNewFormConfig),
      bulksaveRaw: smartviewHasValue(bulksaveRaw) ? String(bulksaveRaw).trim() : '',
      bulksave: smartviewFlagFromValue(bulksaveRaw, true),
      datauploadRaw: smartviewHasValue(datauploadRaw) ? String(datauploadRaw).trim() : '',
      dataupload: smartviewFlagFromValue(datauploadRaw, true),
      validatedataRaw: smartviewHasValue(validatedataRaw) ? String(validatedataRaw).trim() : '',
      validatedata: smartviewFlagFromValue(validatedataRaw, false),
      editmodeRaw: smartviewHasValue(editmodeRaw) ? String(editmodeRaw).trim() : '',
      editmode: editMode,
      inlineEditEnabled: editMode === 'inline',
      savemodeRaw: smartviewHasValue(savemodeRaw) ? String(savemodeRaw).trim() : '',
      savemode: saveMode,
      newforms: smartviewHasValue(newforms) ? String(newforms).trim() : '',
      newforms_transid: smartviewHasValue(newformsTransId) ? String(newformsTransId).trim() : '',
      sv_name: svNameSafe,
      sv_caption: smartviewHasValue(svCaption) ? String(svCaption).trim() : '',
      sv_sourcecnd_raw: smartviewHasValue(svSourceCndRaw) ? String(svSourceCndRaw).trim() : '',
      source_type: sourceType,
      sv_keycol: smartviewHasValue(svKeycol) ? String(svKeycol).trim() : '',
      sqlname: smartviewHasValue(sqlname) ? String(sqlname).trim() : '',
      api_config: smartviewHasValue(apiConfig) ? apiConfig : '',
      memdb_key: memdbKeySafe
    };
  }

  function smartviewMetaHasControlNodes(metaData) {
    const cfg = smartviewExtractEditConfigFromMeta(metaData);
    return smartviewHasValue(cfg.alloweditRaw) ||
      smartviewHasValue(cfg.allownewRaw) ||
      smartviewHasValue(cfg.bulksaveRaw) ||
      smartviewHasValue(cfg.datauploadRaw) ||
      smartviewHasValue(cfg.validatedataRaw) ||
      smartviewHasValue(cfg.editmodeRaw) ||
      smartviewHasValue(cfg.savemodeRaw) ||
      smartviewHasValue(cfg.sv_keycol) ||
      smartviewHasValue(cfg.sv_sourcecnd_raw) ||
      smartviewHasValue(cfg.newforms) ||
      smartviewHasValue(cfg.newforms_transid);
  }

  function smartviewNormalizeEditMode(rawValue) {
    const raw = String(rawValue || '').trim().toLowerCase();
    if (!raw) return 'inline';
    if (raw.includes('inline')) return 'inline';
    return 'default';
  }

  function smartviewNormalizeSaveMode(rawValue) {
    const raw = String(rawValue || '').trim().toLowerCase();
    if (!raw) return 'default';
    if (raw.includes('background') || raw.includes('queue')) return 'background';
    return 'default';
  }

  function smartviewNormalizeSourceType(rawValue) {
    const raw = String(rawValue || '').trim();
    if (raw === '2') return '2';
    if (raw === '3') return '3';
    return '1';
  }

  function smartviewToggleToolbarControlVisibility(controlId, visible) {
    try {
      const el = document.getElementById(controlId);
      if (!el) return;
      el.classList.toggle('d-none', !visible);
      el.style.display = visible ? '' : 'none';
      el.setAttribute('aria-hidden', visible ? 'false' : 'true');
      if (typeof el.disabled === 'boolean') el.disabled = !visible;
    } catch (e) {}
  }

  function smartviewApplyViewRuntimeConfig(metaData) {
    const cfg = smartviewExtractEditConfigFromMeta(metaData);
    const ctrl = smartviewGetControllerInstanceSafe();

    window._smartviewViewConfig = cfg;
    if (ctrl) {
      ctrl.viewConfig = cfg;
      ctrl.sourceType = cfg.source_type;
      ctrl.saveMode = cfg.savemode;
    }

    const sourceType = String(cfg.source_type || '1');
    if (sourceType !== '1' && !window._smartviewSourceTypeInfoShown) {
      window._smartviewSourceTypeInfoShown = true;
      console.warn('[SmartView] Source type', sourceType, 'is configured. API/Redis fetch path is pending FE implementation; ADS flow remains active.');
    }

    // Apply key field configured at view level.
    const configuredKey = String(cfg.sv_keycol || '').trim();
    if (configuredKey) {
      window._entity = window._entity || {};
      window._entity.keyField = configuredKey;
      window._entity.keyFields = [configuredKey];
      if (ctrl) {
        ctrl.keyField = configuredKey;
        ctrl.keyFields = [configuredKey];
      }
    }

    // Keep page title user-friendly for SmartView V2.
    const caption = String(cfg.sv_caption || '').trim();
    if (caption) {
      const titleEl = document.getElementById('EntityTitle') || document.querySelector('.page-header-title');
      if (titleEl) titleEl.textContent = caption;
      document.title = caption;
    }

    const canOpenNew = !!cfg.allownew;
    const canUpload = !!cfg.dataupload;

    smartviewToggleToolbarControlVisibility('smartviewNewTstructBtn', canOpenNew);
    smartviewToggleToolbarControlVisibility('add-entity', canOpenNew);
    smartviewToggleToolbarControlVisibility('smartviewDownloadTemplateBtn', canUpload);
    smartviewToggleToolbarControlVisibility('smartviewUploadExcelBtn', canUpload);

    const uploadInput = document.getElementById('smartviewUploadExcelInput');
    if (uploadInput) {
      uploadInput.disabled = !canUpload;
      if (!canUpload) uploadInput.value = '';
    }

    if (!canUpload) smartviewCloseExcelTemplateMenu();
  }

  function smartviewGetActiveViewConfig(metaDataFallback) {
    const ctrl = smartviewGetControllerInstanceSafe();
    if (ctrl && ctrl.viewConfig && typeof ctrl.viewConfig === 'object') return ctrl.viewConfig;
    if (window._smartviewViewConfig && typeof window._smartviewViewConfig === 'object') return window._smartviewViewConfig;
    const source = Array.isArray(metaDataFallback) ? metaDataFallback
      : (window._entity && Array.isArray(window._entity.metaData) ? window._entity.metaData : []);
    const cfg = smartviewExtractEditConfigFromMeta(source);
    window._smartviewViewConfig = cfg;
    if (ctrl) ctrl.viewConfig = cfg;
    return cfg;
  }

  function smartviewIsNewEntryAllowedForView(metaDataFallback) {
    const cfg = smartviewGetActiveViewConfig(metaDataFallback);
    return !!(cfg && cfg.allownew);
  }

  function smartviewIsDataUploadAllowedForView(metaDataFallback) {
    const cfg = smartviewGetActiveViewConfig(metaDataFallback);
    return !!(cfg && cfg.dataupload);
  }

  function smartviewShouldValidateUploadedData(metaDataFallback) {
    const cfg = smartviewGetActiveViewConfig(metaDataFallback);
    return !!(cfg && cfg.validatedata);
  }

  function smartviewMetaHasDropdownNodes(metaData) {
    const arr = Array.isArray(metaData) ? metaData : [];
    return arr.some(m => {
      if (!m || typeof m !== 'object') return false;

      const cdRaw = smartviewGetObjectValueCI(m, ['cdatatype', 'col_filter', 'colfilter']) || m.cdatatype || '';
      const cd = String(cdRaw || '').trim().toLowerCase().replace(/[\s_]+/g, '');
      if (cd === 'dropdown' || cd === 'select') return true;

      const normalizedRaw = smartviewGetObjectValueCI(m, ['normalized', 'isnormalized', 'is_normalized', 'isnormalised', 'is_normalised'])
        || m.normalized
        || m.isnormalized
        || m.is_normalized
        || m.isnormalised
        || m.is_normalised;
      if (smartviewFlagFromValue(normalizedRaw, false)) return true;

      const psrctxt = String(
        smartviewGetObjectValueCI(m, ['psrctxt', 'psrctext', 'psrctxt'])
        || m.psrctxt
        || m.psrcTxt
        || ''
      ).trim();
      if (psrctxt) return true;

      const srcTable = String(
        smartviewGetObjectValueCI(m, ['srctable', 'sourcetable', 'src_table', 'source_table', 'srctbl', 'table', 'tablename', 'tblname'])
        || m.srctable
        || m.sourcetable
        || ''
      ).trim();
      const srcFld = String(
        smartviewGetObjectValueCI(m, ['srcfld', 'sourcefld', 'src_fld', 'source_fld', 'srcfield', 'sourcefield', 'column', 'colname', 'columnname'])
        || m.srcfld
        || m.sourcefld
        || ''
      ).trim();
      return !!(srcTable && srcFld);
    });
  }

  function smartviewMetaHasFilterTypeHints(metaData) {
    const arr = Array.isArray(metaData) ? metaData : [];
    return arr.some(m => {
      if (!m || typeof m !== 'object') return false;
      const cd = String(m.cdatatype || '').trim().toLowerCase().replace(/[\s_]+/g, '');
      if (cd === 'dropdown' || cd === 'select' || cd === 'date' || cd === 'numeric' || cd === 'number' || cd === 'checkbox' || cd === 'currency') return true;
      const fd = String(m.fdatatype || '').trim().toLowerCase();
      if (fd === 'd' || fd === 'n' || fd === 'b') return true;
      if (String(m.normalized || '').trim().toUpperCase() === 'T') return true;
      if (String(m.psrctxt || m.psrcTxt || '').trim()) return true;
      if (smartviewHasSourceDropdownHints(m)) return true;
      return false;
    });
  }

  function smartviewGetControllerInstanceSafe() {
    return window.smartTableController || window._smartviewController || window._smartviewTableController || null;
  }

  function smartviewIsBulkSaveEnabled() {
    const ctrl = smartviewGetControllerInstanceSafe();
    if (ctrl && typeof ctrl.bulkSaveEnabled === "boolean") return !!ctrl.bulkSaveEnabled;
    return smartviewFlagFromValue(window._smartviewBulkSaveEnabled, true);
  }

  function smartviewApplyBulkSaveSetting(metaData) {
    const cfg = smartviewExtractEditConfigFromMeta(metaData);
    const enabled = smartviewFlagFromValue(cfg.bulksaveRaw, true);
    window._smartviewBulkSaveEnabled = enabled;

    const ctrl = smartviewGetControllerInstanceSafe();
    if (ctrl) ctrl.bulkSaveEnabled = enabled;

    const submitBtn = document.getElementById("submitSmartviewButton");
    if (submitBtn) {
      submitBtn.classList.toggle("d-none", !enabled);
      submitBtn.style.display = enabled ? '' : 'none';
      submitBtn.setAttribute('title', cfg.savemode === 'background' ? 'Submit (Background)' : 'Submit');
    }

    smartviewApplyViewRuntimeConfig(metaData);

    return enabled;
  }

  function smartviewBuildFallbackEditLinkDescriptor(rowData, transIdRaw) {
    const transidInput = (transIdRaw || '').toString().trim();
    if (!transidInput) return '';

    const transid = smartviewNormalizeTypedTransId(transidInput, 't');
    if (!transid) return '';

    const recordid =
      smartviewGetRowFieldValue(rowData, 'recordid') ||
      smartviewGetRowFieldValue(rowData, 'docid') ||
      '';
    const recStr = (recordid === null || recordid === undefined) ? '' : String(recordid).trim();
    const safeRec = recStr ? recStr.replace(/[\^~()]/g, ' ') : '';

    const params = [];
    if (safeRec) {
      params.push(`recordid=${safeRec}`);
      params.push('act=open');
    }
    return params.length ? `t${transid}(${params.join('^')})` : `t${transid}`;
  }

  function smartviewGetAssetBaseUrl() {
    try {
      const topHref = `${window?.top?.location?.href || ''}`;
      const topHrefLower = topHref.toLowerCase();
      const aspxIndex = topHrefLower.indexOf('/aspx/');
      if (aspxIndex > -1) return topHref.substring(0, aspxIndex).replace(/\/+$/, '');
      if (window.location && window.location.origin) return window.location.origin.replace(/\/+$/, '');
    } catch (e) {}
    return '';
  }

  function smartviewToAssetUrl(path, baseUrl) {
    const p = (path || '').toString();
    const b = (baseUrl || '').toString().replace(/\/+$/, '');
    if (!p) return b;
    if (!b) return p;
    if (/^https?:\/\//i.test(p)) return p;
    return `${b}/${p.replace(/^\/+/, '')}`;
  }

  function smartviewEnsureHighchartsLoaded(callback) {
    const done = (ok) => {
      try { if (typeof callback === 'function') callback(!!ok); } catch (e) {}
    };

    if (window.Highcharts && typeof window.Highcharts.chart === 'function') {
      done(true);
      return;
    }

    window._smartviewHcCallbacks = window._smartviewHcCallbacks || [];
    window._smartviewHcCallbacks.push(done);
    if (window._smartviewHcLoading) return;
    window._smartviewHcLoading = true;

    const flush = (ok) => {
      window._smartviewHcLoading = false;
      const q = Array.isArray(window._smartviewHcCallbacks) ? window._smartviewHcCallbacks.slice() : [];
      window._smartviewHcCallbacks = [];
      q.forEach(cb => { try { cb(!!ok); } catch (e) {} });
    };

    if (typeof loadAndCall !== 'function') {
      flush(!!(window.Highcharts && typeof window.Highcharts.chart === 'function'));
      return;
    }

    const baseUrl = smartviewGetAssetBaseUrl();
    const files = {
      css: [],
      js: [
        smartviewToAssetUrl('/ThirdParty/Highcharts/highcharts.js', baseUrl),
        smartviewToAssetUrl('/ThirdParty/Highcharts/highcharts-3d.js', baseUrl),
        smartviewToAssetUrl('/ThirdParty/Highcharts/highcharts-more.js', baseUrl),
        smartviewToAssetUrl('/ThirdParty/Highcharts/highcharts-exporting.js', baseUrl)
      ]
    };

    loadAndCall({
      files: files,
      callBack: () => flush(!!(window.Highcharts && typeof window.Highcharts.chart === 'function'))
    });
  }

  function smartviewNormalizeKpiChartConfigs(rows) {
    const arr = Array.isArray(rows) ? rows : [];
    const normalized = arr.map((r, idx) => {
      const chartCaption = smartviewGetObjectValueCI(r, ['chartcaption', 'chart_caption', 'caption', 'title', 'name']) || '';
      const chartType = smartviewGetObjectValueCI(r, ['charttype', 'chart_type', 'type']) || '';
      const groupCol = smartviewGetObjectValueCI(r, ['grpcol', 'groupcol', 'group_col', 'groupby', 'groupbycol']) || '';
      const aggFunc = smartviewGetObjectValueCI(r, ['agg_func', 'aggfunc', 'aggregate', 'aggregation']) || 'sum';
      const aggCol = smartviewGetObjectValueCI(r, ['agg_col', 'aggcol', 'aggregate_col', 'field', 'yfield']) || '';
      const ord = Number(smartviewGetObjectValueCI(r, ['ord', 'order', 'roword', 'row_order'])) || 999;
      const ord2 = Number(smartviewGetObjectValueCI(r, ['ord2', 'order2', 'colord', 'col_order'])) || 999;
      return {
        chartcaption: String(chartCaption || '').trim(),
        charttype: String(chartType || '').trim(),
        grpcol: String(groupCol || '').trim(),
        agg_func: String(aggFunc || 'sum').trim(),
        agg_col: String(aggCol || '').trim(),
        ord: ord,
        ord2: ord2,
        _idx: idx
      };
    }).filter(c => c.chartcaption || c.charttype || c.grpcol || c.agg_col);

    normalized.sort((a, b) => {
      if (a.ord !== b.ord) return a.ord - b.ord;
      if (a.ord2 !== b.ord2) return a.ord2 - b.ord2;
      return a._idx - b._idx;
    });
    return normalized;
  }

  function smartviewCoerceNumber(value) {
    if (value === null || value === undefined) return NaN;
    if (typeof value === 'number') return Number.isFinite(value) ? value : NaN;
    let s = String(value).trim();
    if (!s) return NaN;
    s = s.replace(/,/g, '').replace(/[^\d.\-]/g, '');
    if (s.startsWith('(') && s.endsWith(')')) s = '-' + s.slice(1, -1);
    const n = parseFloat(s);
    return Number.isFinite(n) ? n : NaN;
  }

  function smartviewGetRowFieldValue(row, fieldName) {
    if (!row || !fieldName) return '';
    let v = '';
    try { v = smartviewGetRowValueForHyperlink(row, fieldName); } catch (e) {}
    if (v === undefined || v === null || String(v).trim() === '') {
      v = getRowValueCaseInsensitive(row, fieldName);
    }
    return v;
  }

  function smartviewAggregateRows(rows, aggFunc, aggCol) {
    const data = Array.isArray(rows) ? rows : [];
    const fn = String(aggFunc || 'sum').trim().toLowerCase();
    const col = String(aggCol || '').trim();

    if (fn === 'count') {
      if (!col) return data.length;
      let count = 0;
      data.forEach(r => {
        const v = smartviewGetRowFieldValue(r, col);
        if (v !== undefined && v !== null && String(v).trim() !== '') count++;
      });
      return count;
    }

    const nums = [];
    data.forEach(r => {
      const raw = col ? smartviewGetRowFieldValue(r, col) : '';
      const n = smartviewCoerceNumber(raw);
      if (!isNaN(n)) nums.push(n);
    });
    if (!nums.length) return 0;

    switch (fn) {
      case 'avg':
      case 'average':
        return nums.reduce((a, b) => a + b, 0) / nums.length;
      case 'min':
        return Math.min.apply(null, nums);
      case 'max':
        return Math.max.apply(null, nums);
      default:
        return nums.reduce((a, b) => a + b, 0);
    }
  }

  function smartviewFormatMetricValue(value) {
    const n = smartviewCoerceNumber(value);
    if (isNaN(n)) return '--';
    try {
      const rounded = Math.round(n);
      return Number(rounded).toLocaleString('en-IN', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
      });
    } catch (e) {
      return String(Math.round(n));
    }
  }

  function smartviewMapMiniChartType(chartType) {
    const t = String(chartType || '').trim().toLowerCase();
    if (t === 'donut' || t === 'doughnut') return 'donut';
    if (t === 'semi-donut' || t === 'semi donut' || t === 'semidonut' || t === 'half-donut' || t === 'half donut') return 'semi-donut';
    if (t === 'stacked-column' || t === 'stacked column' || t === 'stackedcolumn') return 'stacked-column';
    if (t === 'stacked-bar' || t === 'stacked bar' || t === 'stackedbar') return 'stacked-bar';
    if (t === 'stacked-percentage-column' || t === 'stacked percentage column' || t === 'stackedpercentagecolumn' || t === '100%-stacked' || t === '100% stacked') return 'stacked-percentage-column';
    if (t === 'pie' || t === 'bar' || t === 'column' || t === 'line' || t === 'area') return t;
    return 'column';
  }

  function smartviewGetSupportedChartTypes() {
    return [
      { type: 'pie', name: 'Pie' },
      { type: 'donut', name: 'Donut' },
      { type: 'semi-donut', name: 'Semi Donut' },
      { type: 'column', name: 'Column' },
      { type: 'bar', name: 'Bar' },
      { type: 'line', name: 'Line' },
      { type: 'area', name: 'Area' },
      { type: 'stacked-column', name: 'Stacked Column' },
      { type: 'stacked-bar', name: 'Stacked Bar' },
      { type: 'stacked-percentage-column', name: '100% Stacked' }
    ];
  }

  function smartviewGetDefaultChartPaletteColors() {
    return ['#f4bc01', '#3d5996', '#e80502', '#539cfe', '#3ddab4', '#f14f5a'];
  }

  function smartviewNormalizeColorHex(colorValue, fallbackValue) {
    const fallback = `${fallbackValue || '#4f8df8'}`.trim().toLowerCase();
    const rawValue = `${colorValue || ''}`.trim().toLowerCase();
    if (/^#[0-9a-f]{6}$/i.test(rawValue)) return rawValue;
    if (/^#[0-9a-f]{3}$/i.test(rawValue)) {
      return `#${rawValue[1]}${rawValue[1]}${rawValue[2]}${rawValue[2]}${rawValue[3]}${rawValue[3]}`;
    }
    return fallback;
  }

  function smartviewResolvePaletteColorsByKey(paletteKey) {
    const defaultColors = smartviewGetDefaultChartPaletteColors();
    const normalizedKey = `${paletteKey || ''}`.trim();
    if (!normalizedKey) return defaultColors.slice();

    const paletteSource = (typeof customChartColors === 'object' && customChartColors)
      ? customChartColors[normalizedKey]
      : null;
    if (!Array.isArray(paletteSource) || !paletteSource.length) return defaultColors.slice();

    return defaultColors.map((fallbackColor, index) => {
      const sourceColor = paletteSource[index] || paletteSource[index % paletteSource.length];
      return smartviewNormalizeColorHex(sourceColor, fallbackColor);
    });
  }

  function smartviewResolveChartPaletteConfig(paletteValue) {
    let resolvedColors = smartviewGetDefaultChartPaletteColors();
    let paletteKey = 'custom';

    if (paletteValue && typeof paletteValue === 'object') {
      const customColors = Array.isArray(paletteValue.customColors)
        ? paletteValue.customColors
        : (Array.isArray(paletteValue.colors) ? paletteValue.colors : []);
      if (customColors.length) {
        resolvedColors = customColors.slice();
      } else if (`${paletteValue.paletteKey || ''}`.trim() !== '') {
        paletteKey = `${paletteValue.paletteKey}`.trim();
        resolvedColors = smartviewResolvePaletteColorsByKey(paletteKey);
      }
    } else if (typeof paletteValue === 'string' && `${paletteValue}`.trim() !== '') {
      paletteKey = `${paletteValue}`.trim();
      resolvedColors = smartviewResolvePaletteColorsByKey(paletteKey);
    }

    const defaultColors = smartviewGetDefaultChartPaletteColors();
    const customColors = defaultColors.map((fallbackColor, index) => {
      const source = resolvedColors[index] || resolvedColors[index % Math.max(resolvedColors.length, 1)];
      return smartviewNormalizeColorHex(source, fallbackColor);
    });

    return {
      paletteKey: paletteKey || 'custom',
      customColors: customColors,
      cck: 'custom',
      cccv: customColors.join(',')
    };
  }

  function smartviewGetChartCustomizationStore() {
    if (!window._smartviewChartCustomizationStore || typeof window._smartviewChartCustomizationStore !== 'object') {
      window._smartviewChartCustomizationStore = {};
    }
    return window._smartviewChartCustomizationStore;
  }

  function smartviewBuildChartStateKey(source, fallbackIndex) {
    const adsName = String(getQueryParam('ads') || getQueryParam('adsName') || getQueryParam('adsname') || '').trim().toLowerCase();
    const safeSource = source && typeof source === 'object' ? source : {};
    const parts = [
      adsName,
      smartviewGetObjectValueCI(safeSource, ['chartcaption', 'title']) || '',
      smartviewGetObjectValueCI(safeSource, ['grpcol', 'groupfield', 'groupField']) || '',
      smartviewGetObjectValueCI(safeSource, ['agg_col', 'aggcol', 'aggCol']) || '',
      smartviewGetObjectValueCI(safeSource, ['agg_func', 'aggfunc', 'aggFunc']) || '',
      smartviewGetObjectValueCI(safeSource, ['charttype', 'chart_type', 'chartType', 'baseChartType']) || '',
      smartviewGetObjectValueCI(safeSource, ['ord']) || '',
      smartviewGetObjectValueCI(safeSource, ['ord2']) || '',
      Number.isFinite(Number(fallbackIndex)) ? String(fallbackIndex) : ''
    ].map(v => String(v || '').trim().toLowerCase());
    return `smartview-chart::${parts.join('|')}`;
  }

  function smartviewApplyStoredChartCustomization(item, fallbackIndex) {
    if (!item || typeof item !== 'object') return item;
    if (!item.stateKey) item.stateKey = smartviewBuildChartStateKey(item, fallbackIndex);

    const customizationStore = smartviewGetChartCustomizationStore();
    const storedState = customizationStore[item.stateKey];
    const defaultChartType = smartviewMapMiniChartType(item.baseChartType || item.chartType);
    const defaultPalette = smartviewResolveChartPaletteConfig(item.paletteConfig || 'newPalette');

    item.baseChartType = defaultChartType;
    item.chartType = defaultChartType;
    item.paletteConfig = defaultPalette;

    if (storedState && typeof storedState === 'object') {
      if (`${storedState.chartType || ''}`.trim() !== '') {
        item.chartType = smartviewMapMiniChartType(storedState.chartType);
      }
      item.paletteConfig = smartviewResolveChartPaletteConfig({
        paletteKey: storedState.paletteKey || 'custom',
        customColors: Array.isArray(storedState.customColors) ? storedState.customColors : defaultPalette.customColors
      });
    }

    return item;
  }

  function smartviewPersistChartCustomization(item, fallbackIndex) {
    if (!item || typeof item !== 'object') return;
    const stateKey = item.stateKey || smartviewBuildChartStateKey(item, fallbackIndex);
    const paletteConfig = smartviewResolveChartPaletteConfig(item.paletteConfig || 'newPalette');
    smartviewGetChartCustomizationStore()[stateKey] = {
      chartType: smartviewMapMiniChartType(item.chartType || item.baseChartType),
      paletteKey: paletteConfig.paletteKey || 'custom',
      customColors: Array.isArray(paletteConfig.customColors) ? paletteConfig.customColors.slice() : smartviewGetDefaultChartPaletteColors()
    };
    item.stateKey = stateKey;
    item.chartType = smartviewMapMiniChartType(item.chartType || item.baseChartType);
    item.paletteConfig = paletteConfig;
  }

  function smartviewGetPopupChartCardHeight(chartType, pointCount) {
    // Keep popup card heights uniform so mixed chart types align in a clean grid.
    return window.innerWidth <= 991 ? 340 : 390;
  }

  function smartviewBuildKpiItemsFromRows(configRows, sourceRows) {
    const configs = smartviewNormalizeKpiChartConfigs(configRows);
    const rows = Array.isArray(sourceRows) ? sourceRows : [];
    const out = [];

    configs.forEach(cfg => {
      const t = String(cfg.charttype || '').trim().toLowerCase();
      const isKpi = (t === 'kpi') || (!cfg.grpcol && (t === '' || t === 'metric' || t === 'card'));
      if (!isKpi) return;

      const rawValue = smartviewAggregateRows(rows, cfg.agg_func, cfg.agg_col);
      const title = cfg.chartcaption || smartviewNormalizeMetricName(cfg.agg_col || 'Metric');
      out.push({
        title: smartviewNormalizeMetricName(title),
        valueRaw: rawValue,
        value: smartviewFormatMetricValue(rawValue),
        ord: cfg.ord,
        ord2: cfg.ord2
      });
    });

    return out;
  }

  function smartviewBuildChartItemsFromRows(configRows, sourceRows) {
    const configs = smartviewNormalizeKpiChartConfigs(configRows);
    const rows = Array.isArray(sourceRows) ? sourceRows : [];
    const out = [];

    configs.forEach(cfg => {
      const tRaw = String(cfg.charttype || '').trim().toLowerCase();
      const isKpi = (tRaw === 'kpi') || (!cfg.grpcol && (tRaw === '' || tRaw === 'metric' || tRaw === 'card'));
      if (isKpi) return;

      const groupField = String(cfg.grpcol || '').trim();
      const grouped = new Map();
      if (groupField) {
        rows.forEach(r => {
          const raw = smartviewGetRowFieldValue(r, groupField);
          const key = (raw === null || raw === undefined || String(raw).trim() === '') ? '(Blank)' : String(raw).trim();
          if (!grouped.has(key)) grouped.set(key, []);
          grouped.get(key).push(r);
        });
      } else {
        grouped.set('Total', rows.slice());
      }

      const points = [];
      grouped.forEach((gRows, key) => {
        const y = smartviewAggregateRows(gRows, cfg.agg_func, cfg.agg_col);
        const yn = smartviewCoerceNumber(y);
        if (isNaN(yn)) return;
        points.push({ name: key, y: yn });
      });

      points.sort((a, b) => b.y - a.y);
      const limitedPoints = points.slice(0, 12);
      const metricName = smartviewNormalizeMetricName(cfg.agg_col || 'value');
      const groupName = smartviewNormalizeMetricName(cfg.grpcol || '');
      const aggLabel = String(cfg.agg_func || 'sum').toUpperCase();
      const subtitle = `${aggLabel} ${metricName}${groupName ? ` by ${groupName}` : ''}`;

      const defaultChartType = smartviewMapMiniChartType(cfg.charttype);
      const item = {
        title: smartviewNormalizeMetricName(cfg.chartcaption || 'Chart'),
        subtitle: subtitle,
        baseChartType: defaultChartType,
        chartType: defaultChartType,
        points: limitedPoints,
        stateKey: smartviewBuildChartStateKey(cfg, out.length),
        paletteConfig: smartviewResolveChartPaletteConfig('newPalette'),
        ord: cfg.ord,
        ord2: cfg.ord2
      };
      smartviewApplyStoredChartCustomization(item, out.length);
      out.push(item);
    });

    return out;
  }

  function smartviewFetchAllRowsForAds(adsName, callback) {
    const cb = (err, rows) => { try { if (typeof callback === 'function') callback(err || null, Array.isArray(rows) ? rows : []); } catch (e) {} };
    try {
      const caller = smartviewResolveAxListCaller();
      if (!caller || typeof caller.GetDataFromAxList !== 'function') {
        cb(new Error('GetDataFromAxList not available'), []);
        return;
      }

      const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
      const safeFilters = stripSmartviewFilterTransId((ctrl && Array.isArray(ctrl.filters)) ? ctrl.filters : []);
      const sqlParams = Object.assign(
        {},
        (ctrl && ctrl._entitySqlParams) ? ctrl._entitySqlParams : {},
        (ctrl && ctrl.props && ctrl.props.sqlParams) ? ctrl.props.sqlParams : {}
      );
      const props = {
        ADS: false,
        CachePermissions: true,
        getallrecordscount: false,
        pageno: 1,
        pagesize: 0,
        keyfield: '',
        keyvalue: '',
        sorting: [],
        filters: safeFilters
      };
      if (ctrl && ctrl.axClient_dateformat) props.axClient_dateformat = ctrl.axClient_dateformat;

      const params = {
        adsNames: [adsName],
        refreshCache: false,
        sqlParams: sqlParams,
        props: props
      };

      caller.GetDataFromAxList(params, function (resp) {
        try {
          const extracted = smartviewExtractRowsFromAxListResponse(resp);
          const sourceRows = Array.isArray(extracted.rows) ? extracted.rows : [];
          const filteredRows = safeFilters.length
            ? smartviewApplyClientFilterFallback(sourceRows, safeFilters)
            : sourceRows;
          cb(null, filteredRows);
        } catch (e) {
          cb(e, []);
        }
      }, function (err) {
        cb(err, []);
      });
    } catch (e) {
      cb(e, []);
    }
  }

  function smartviewGetCarouselScrollStep(containerEl) {
    if (!containerEl) return 320;
    const width = Math.max(0, Math.round(containerEl.clientWidth || 0));
    if (!width) return 320;
    return Math.max(220, Math.round(width * 0.85));
  }

  function smartviewRequestCarouselRefresh(delayMs) {
    const ms = Math.max(0, Number(delayMs) || 0);
    setTimeout(function () {
      try {
        if (typeof requestAnimationFrame === 'function') {
          requestAnimationFrame(function () {
            smartviewRefreshAllCarouselButtons();
          });
        } else {
          smartviewRefreshAllCarouselButtons();
        }
      } catch (e) {}
    }, ms);
  }

  function smartviewGetCarouselCards(containerEl) {
    if (!containerEl) return [];
    return Array.from(containerEl.querySelectorAll('.Carousel-Items'));
  }

  function smartviewIsCarouselLayoutReady(containerEl, cards) {
    if (!containerEl) return false;
    const hostWidth = Math.max(0, Math.round(containerEl.clientWidth || 0));
    if (hostWidth <= 1) return false;
    const arr = Array.isArray(cards) ? cards : smartviewGetCarouselCards(containerEl);
    if (!arr.length) return true;
    const hasSizedCard = arr.some(function (cardEl) {
      const w = Math.max(0, Math.round(cardEl.offsetWidth || cardEl.getBoundingClientRect().width || 0));
      return w > 1;
    });
    return hasSizedCard;
  }

  function smartviewCanCarouselMoveByOffsets(containerEl, direction) {
    if (!containerEl) return false;
    const cards = smartviewGetCarouselCards(containerEl);
    if (!cards.length) return false;

    const epsilon = 2;
    const current = Math.max(0, Number(containerEl.scrollLeft) || 0);
    const viewportRight = current + Math.max(0, Number(containerEl.clientWidth) || 0);
    const hostRect = containerEl.getBoundingClientRect();
    const dir = Number(direction) < 0 ? -1 : 1;

    if (dir < 0) {
      return cards.some(function (cardEl) {
        const rect = cardEl.getBoundingClientRect();
        const contentLeft = current + (rect.left - hostRect.left);
        return contentLeft < (current - epsilon);
      });
    }

    return cards.some(function (cardEl) {
      const rect = cardEl.getBoundingClientRect();
      const contentLeft = current + (rect.left - hostRect.left);
      const width = Math.max(0, Number(rect.width) || 0);
      return (contentLeft + width) > (viewportRight + epsilon);
    });
  }

  function smartviewGetCarouselOffsetOverflow(containerEl) {
    if (!containerEl) return { left: 0, right: 0 };
    const cards = smartviewGetCarouselCards(containerEl);
    if (!cards.length) return { left: 0, right: 0 };

    const current = Math.max(0, Number(containerEl.scrollLeft) || 0);
    const viewportRight = current + Math.max(0, Number(containerEl.clientWidth) || 0);
    const hostRect = containerEl.getBoundingClientRect();
    let hiddenLeft = 0;
    let hiddenRight = 0;

    cards.forEach(function (cardEl) {
      const rect = cardEl.getBoundingClientRect();
      const left = current + (rect.left - hostRect.left);
      const width = Math.max(0, Number(rect.width) || 0);
      const right = left + width;
      if (left < current) hiddenLeft = Math.max(hiddenLeft, current - left);
      if (right > viewportRight) hiddenRight = Math.max(hiddenRight, right - viewportRight);
    });

    return { left: hiddenLeft, right: hiddenRight };
  }

  function smartviewCanCarouselMoveByMetrics(containerEl, direction) {
    if (!containerEl) return false;
    const maxScrollLeft = Math.max(0, Math.round((containerEl.scrollWidth || 0) - (containerEl.clientWidth || 0)));
    const current = Math.max(0, Math.round(containerEl.scrollLeft || 0));
    if (maxScrollLeft <= 1) return false;
    if (Number(direction) < 0) return current > 1;
    return current < (maxScrollLeft - 1);
  }

  function smartviewCanCarouselMove(containerEl, direction) {
    if (!containerEl) return false;
    const cards = smartviewGetCarouselCards(containerEl);
    if (!cards.length) return false;
    const hostRect = containerEl.getBoundingClientRect();
    const epsilon = 2;
    const dir = Number(direction) < 0 ? -1 : 1;

    if (dir < 0) {
      return cards.some(function (cardEl) {
        const rect = cardEl.getBoundingClientRect();
        return rect.left < (hostRect.left - epsilon);
      });
    }
    return cards.some(function (cardEl) {
      const rect = cardEl.getBoundingClientRect();
      return rect.right > (hostRect.right + epsilon);
    });
  }

  function smartviewFindCarouselTargetScrollLeft(containerEl, direction) {
    if (!containerEl) return 0;

    const dir = Number(direction) < 0 ? -1 : 1;
    const cards = smartviewGetCarouselCards(containerEl);
    const current = Math.max(0, Number(containerEl.scrollLeft) || 0);
    const maxScrollLeft = Math.max(0, Math.round((containerEl.scrollWidth || 0) - (containerEl.clientWidth || 0)));
    const hostRect = containerEl.getBoundingClientRect();
    const epsilon = 2;
    let target = null;

    if (cards.length) {
      if (dir > 0) {
        cards.forEach(function (cardEl) {
          const rect = cardEl.getBoundingClientRect();
          if (rect.right <= (hostRect.right + epsilon)) return;
          const candidate = current + (rect.left - hostRect.left);
          if (target === null || candidate < target) target = candidate;
        });
      } else {
        cards.forEach(function (cardEl) {
          const rect = cardEl.getBoundingClientRect();
          if (rect.left >= (hostRect.left - epsilon)) return;
          const candidate = current + (rect.left - hostRect.left);
          if (target === null || candidate > target) target = candidate;
        });
      }
    }

    if (target === null) {
      const step = smartviewGetCarouselScrollStep(containerEl);
      target = current + (dir * step);
    }

    const bounded = Math.max(0, Math.min(maxScrollLeft, Math.round(target)));
    if (Math.abs(bounded - current) <= 1) return dir > 0 ? maxScrollLeft : 0;
    return bounded;
  }

  function smartviewUpdateCarouselButtons(wrapEl) {
    if (!wrapEl) return;
    const container = wrapEl.querySelector('.Carousel-Items-Container');
    const leftBtn = wrapEl.querySelector('.carousel-btn.btn-left');
    const rightBtn = wrapEl.querySelector('.carousel-btn.btn-right');
    if (!container || !leftBtn || !rightBtn) return;

    const applyArrowState = function (btnEl, canMove) {
      if (!btnEl) return;
      const allowMove = !!canMove;
      btnEl.removeAttribute('disabled');
      btnEl.setAttribute('aria-disabled', allowMove ? 'false' : 'true');
      btnEl.setAttribute('aria-hidden', allowMove ? 'false' : 'true');
      btnEl.setAttribute('data-sv-can-move', allowMove ? 'T' : 'F');
      btnEl.tabIndex = allowMove ? 0 : -1;
      btnEl.classList.toggle('is-disabled', !allowMove);
      if (allowMove) btnEl.style.removeProperty('display');
      else btnEl.style.display = 'none';
    };

    const cards = smartviewGetCarouselCards(container);
    const hasCards = cards.length > 1;
    if (!hasCards) {
      applyArrowState(leftBtn, false);
      applyArrowState(rightBtn, false);
      return;
    }

    // If layout is still settling (hidden->shown or async card sizing), keep arrows hidden and re-check shortly.
    if (!smartviewIsCarouselLayoutReady(container, cards)) {
      applyArrowState(leftBtn, false);
      applyArrowState(rightBtn, false);
      smartviewRequestCarouselRefresh(100);
      smartviewRequestCarouselRefresh(260);
      smartviewRequestCarouselRefresh(520);
      return;
    }

    const epsilon = 6; // ignore tiny sub-pixel overflows that should not show arrows
    const currentScrollLeft = Math.max(0, Number(container.scrollLeft) || 0);
    const overflowPixels = Math.max(0, (Number(container.scrollWidth) || 0) - (Number(container.clientWidth) || 0));
    const offsetOverflow = smartviewGetCarouselOffsetOverflow(container);
    const leftHiddenPx = Math.max(currentScrollLeft, Number(offsetOverflow.left) || 0);
    const rightHiddenPx = Math.max(overflowPixels - currentScrollLeft, Number(offsetOverflow.right) || 0);
    let canMoveLeft = leftHiddenPx > epsilon;
    let canMoveRight = rightHiddenPx > epsilon;

    // Fallback for stale scrollWidth/clientWidth measurements during dynamic chart layout.
    if (!canMoveLeft || !canMoveRight) {
      if (!canMoveLeft) canMoveLeft = smartviewCanCarouselMoveByOffsets(container, -1);
      if (!canMoveRight) canMoveRight = smartviewCanCarouselMoveByOffsets(container, 1);
      if (!canMoveLeft) canMoveLeft = smartviewCanCarouselMove(container, -1);
      if (!canMoveRight) canMoveRight = smartviewCanCarouselMove(container, 1);
      if (!canMoveLeft) canMoveLeft = smartviewCanCarouselMoveByMetrics(container, -1);
      if (!canMoveRight) canMoveRight = smartviewCanCarouselMoveByMetrics(container, 1);
    }

    // Final guard: don't show arrows for tiny rounding-only offsets.
    if (canMoveLeft && leftHiddenPx <= epsilon && currentScrollLeft <= epsilon) canMoveLeft = false;
    if (canMoveRight && rightHiddenPx <= epsilon && overflowPixels <= epsilon) canMoveRight = false;

    // Hide the side that cannot move. Example:
    // start -> left hidden, right visible; end -> right hidden, left visible.
    applyArrowState(leftBtn, canMoveLeft);
    applyArrowState(rightBtn, canMoveRight);
  }

  function smartviewRefreshAllCarouselButtons() {
    try {
      document.querySelectorAll('.Sales-data-content-wrapper, .Sales-data-charts-wrapper').forEach(function (wrapEl) {
        smartviewUpdateCarouselButtons(wrapEl);
      });
    } catch (e) {}
  }

  function smartviewBindCarouselControls(wrapEl) {
    if (!wrapEl) return;
    const container = wrapEl.querySelector('.Carousel-Items-Container');
    const leftBtn = wrapEl.querySelector('.carousel-btn.btn-left');
    const rightBtn = wrapEl.querySelector('.carousel-btn.btn-right');
    if (!container || !leftBtn || !rightBtn) return;
    if (container.dataset.svCarouselBound === 'T') return;
    container.dataset.svCarouselBound = 'T';

    try {
      const cs = window.getComputedStyle(container);
      if (cs.display !== 'flex') container.style.display = 'flex';
      if (cs.flexWrap !== 'nowrap') container.style.flexWrap = 'nowrap';
      if (cs.overflowX !== 'auto' && cs.overflowX !== 'scroll') container.style.overflowX = 'auto';
      if (cs.overflowY !== 'hidden') container.style.overflowY = 'hidden';
      container.style.scrollBehavior = 'smooth';
      container.style.webkitOverflowScrolling = 'touch';
    } catch (e) {}

    const scrollByStep = function (direction) {
      const targetLeft = smartviewFindCarouselTargetScrollLeft(container, direction);
      try {
        container.scrollTo({ left: targetLeft, behavior: 'smooth' });
      } catch (e) {
        container.scrollLeft = targetLeft;
      }
      setTimeout(function () { smartviewUpdateCarouselButtons(wrapEl); }, 140);
    };

    leftBtn.addEventListener('click', function (event) {
      event.preventDefault();
      smartviewUpdateCarouselButtons(wrapEl);
      if (leftBtn.getAttribute('data-sv-can-move') !== 'T') return;
      scrollByStep(-1);
    });

    rightBtn.addEventListener('click', function (event) {
      event.preventDefault();
      smartviewUpdateCarouselButtons(wrapEl);
      if (rightBtn.getAttribute('data-sv-can-move') !== 'T') return;
      scrollByStep(1);
    });

    container.addEventListener('scroll', function () {
      smartviewUpdateCarouselButtons(wrapEl);
    }, { passive: true });

    if (typeof ResizeObserver === 'function' && !container._svCarouselResizeObserver) {
      try {
        const resizeObserver = new ResizeObserver(function () {
          smartviewUpdateCarouselButtons(wrapEl);
        });
        resizeObserver.observe(container);
        if (wrapEl !== container) resizeObserver.observe(wrapEl);
        container._svCarouselResizeObserver = resizeObserver;
      } catch (e) {}
    }

    if (!window._smartviewCarouselResizeBound) {
      window.addEventListener('resize', function () {
        smartviewRefreshAllCarouselButtons();
      });
      window._smartviewCarouselResizeBound = true;
    }

    smartviewRequestCarouselRefresh(0);
    smartviewRequestCarouselRefresh(120);
    smartviewRequestCarouselRefresh(320);
    smartviewRequestCarouselRefresh(800);
    smartviewRequestCarouselRefresh(1400);
    smartviewRequestCarouselRefresh(2200);
  }

  function smartviewRenderKpiCards(items) {
    const wrap = document.querySelector('.Sales-data-content-wrapper');
    if (!wrap) return;
    const arr = Array.isArray(items) ? items : [];
    if (!arr.length) {
      wrap.innerHTML = '';
      smartviewSetElementHasData(wrap, false);
      try { smartviewRefreshOptionsMenuLabels(); } catch (e) {}
      return;
    }

    let cardsHtml = '';
    arr.forEach(it => {
      const title = escapeHtml(it.title || '');
      const value = escapeHtml(it.value || '--');
      cardsHtml += `
        <div class="Carousel-Items Sales-data-content">
          <div class="card-custom">
            <div class="d-flex align-items-center gap-3">
              <div class="icon-box">
                <span class="material-icons material-icons-style material-icons-2">tune</span>
              </div>
              <div>
                <div class="title">${title}</div>
                <div class="value">${value}</div>
              </div>
            </div>
          </div>
        </div>
      `;
    });

    wrap.innerHTML = `
      <div class="carousel-wrapper">
        <button type="button" class="carousel-btn btn-left" aria-label="Scroll KPI cards left">
          <span class="material-icons material-icons-style material-icons-2">keyboard_arrow_left</span>
        </button>
        <div class="Carousel-Items-Container">
          ${cardsHtml}
        </div>
        <button type="button" class="carousel-btn btn-right" aria-label="Scroll KPI cards right">
          <span class="material-icons material-icons-style material-icons-2">keyboard_arrow_right</span>
        </button>
      </div>
    `;
    smartviewBindCarouselControls(wrap);
    smartviewSetElementHasData(wrap, true);
    try { smartviewRefreshOptionsMenuLabels(); } catch (e) {}
  }

  function smartviewDestroyHighchartsInContainer(containerEl) {
    if (!containerEl || !window.Highcharts || !Array.isArray(window.Highcharts.charts)) return;
    window.Highcharts.charts.forEach(ch => {
      if (!ch || !ch.renderTo) return;
      if (ch.renderTo === containerEl) {
        try { ch.destroy(); } catch (e) {}
      }
    });
  }

  function smartviewRenderSingleMiniChart(containerEl, item, renderOptions) {
    if (!containerEl || !item) return;
    const opts = (renderOptions && typeof renderOptions === 'object') ? renderOptions : {};
    const expanded = !!opts.expanded;
    const points = Array.isArray(item.points) ? item.points : [];
    if (!points.length) {
      containerEl.innerHTML = `<div class="sales-chart-empty">No data</div>`;
      return;
    }

    smartviewDestroyHighchartsInContainer(containerEl);
    containerEl.innerHTML = '';

    const chartType = smartviewMapMiniChartType(item.chartType || item.baseChartType);
    const isPie = chartType === 'pie' || chartType === 'donut' || chartType === 'semi-donut';
    const isBar = chartType === 'bar' || chartType === 'stacked-bar';
    const isLine = chartType === 'line';
    const isArea = chartType === 'area';
    const isPercentStack = chartType === 'stacked-percentage-column';
    const isStacked = chartType === 'stacked-column' || chartType === 'stacked-bar' || isPercentStack;
    const highchartsType = isPie ? 'pie' : (isBar ? 'bar' : (isLine ? 'line' : (isArea ? 'area' : 'column')));
    const requestedHeight = Number(opts.height);
    const fallbackHeight = expanded ? 360 : 122;
    const minHeight = expanded ? 220 : 110;
    const chartHeight = Math.round(Math.max(
      minHeight,
      (Number.isFinite(requestedHeight) && requestedHeight > 0) ? requestedHeight : (containerEl.clientHeight || fallbackHeight)
    ));
    const categories = points.map(p => String(p.name || ''));
    const values = points.map(p => Number(p.y) || 0);
    const pieData = points.map(p => ({ name: String(p.name || ''), y: Number(p.y) || 0 }));
    const paletteConfig = smartviewResolveChartPaletteConfig(item.paletteConfig || item.palette || 'newPalette');
    const longestCategoryLength = categories.reduce((maxLen, label) => {
      return Math.max(maxLen, String(label || '').trim().length);
    }, 0);
    const expandedCategoryBottomMargin = (expanded && !isPie && !isBar)
      ? Math.max(78, Math.min(108, 52 + Math.min(longestCategoryLength, 24)))
      : 0;
    const expandedCategoryLeftMargin = (expanded && !isPie && !isBar) ? 76 : 66;
    const expandedBarMarginLeft = (expanded && isBar)
      ? Math.max(122, Math.min(260, 56 + (Math.min(longestCategoryLength, 26) * 7)))
      : (expanded ? 64 : (isBar ? 52 : 30));
    const expandedChartMargin = isPie
      ? (expanded ? [14, 14, 14, 14] : [10, 16, 10, 16])
      : (expanded
        ? (isBar ? [16, 20, 58, expandedBarMarginLeft] : [16, 20, expandedCategoryBottomMargin, expandedCategoryLeftMargin])
        : (isBar ? [12, 14, 32, 52] : [12, 14, 32, 28]));

    const chartOptions = {
      chart: {
        type: highchartsType,
        backgroundColor: 'transparent',
        height: chartHeight,
        margin: expandedChartMargin,
        spacing: expanded ? [10, 10, 10, 10] : [4, 4, 4, 4],
        animation: false
      },
      title: { text: null },
      credits: { enabled: false },
      exporting: { enabled: false },
      legend: { enabled: expanded && isPie },
      tooltip: {
        headerFormat: '<span style="font-size:10px">{point.key}</span><br/>',
        pointFormat: '<b>{point.y:,.2f}</b>'
      },
      colors: Array.isArray(paletteConfig.customColors) ? paletteConfig.customColors : smartviewGetDefaultChartPaletteColors(),
      plotOptions: {
        series: {
          animation: false,
          dataLabels: { enabled: false },
          borderWidth: 0
        },
        pie: {
          dataLabels: { enabled: expanded },
          innerSize: chartType === 'donut' ? '55%' : (chartType === 'semi-donut' ? '60%' : '0%'),
          center: chartType === 'semi-donut' ? ['50%', '72%'] : ['50%', '50%'],
          size: chartType === 'semi-donut' ? '100%' : (expanded ? '90%' : '84%'),
          startAngle: chartType === 'semi-donut' ? -90 : undefined,
          endAngle: chartType === 'semi-donut' ? 90 : undefined
        },
        line: {
          marker: { enabled: !expanded },
          lineWidth: 2
        },
        area: {
          marker: { enabled: false },
          lineWidth: 1,
          fillOpacity: 0.25
        }
      }
    };

    if (isStacked) {
      chartOptions.plotOptions.series.stacking = isPercentStack ? 'percent' : 'normal';
    }

    if (isPie) {
      chartOptions.series = [{ name: item.subtitle || 'Value', data: pieData }];
    } else {
      chartOptions.xAxis = {
        categories: categories,
        tickLength: 0,
        lineWidth: 0,
        minPadding: (expanded && !isBar) ? 0.04 : 0,
        maxPadding: (expanded && !isBar) ? 0.04 : 0,
        labels: {
          enabled: expanded,
          reserveSpace: true,
          overflow: 'allow',
          x: expanded ? (isBar ? -4 : 0) : 0,
          y: expanded ? (isBar ? 0 : 14) : 0,
          rotation: (expanded && !isBar) ? -35 : 0,
          align: (expanded && !isBar) ? 'right' : undefined,
          style: expanded ? { fontSize: '11px' } : {},
          formatter: function () {
            const label = String(this.value || '');
            if (!expanded) return label;
            const maxLen = isBar ? 16 : 14;
            return label.length > maxLen ? (label.slice(0, maxLen) + '...') : label;
          }
        }
      };
      chartOptions.yAxis = {
        title: { text: null },
        labels: {
          enabled: expanded,
          reserveSpace: true,
          x: expanded ? -2 : -2,
          style: expanded ? { fontSize: '11px' } : {}
        },
        gridLineWidth: expanded ? 1 : 0,
        startOnTick: true,
        endOnTick: true,
        minPadding: 0.05,
        maxPadding: 0.12
      };
      chartOptions.series = [{
        name: item.subtitle || 'Value',
        data: values,
        colorByPoint: !isLine && !isArea
      }];
    }

    try {
      window.Highcharts.chart(containerEl, chartOptions);
    } catch (e) {
      console.warn('smartviewRenderSingleMiniChart failed', e);
      containerEl.innerHTML = `<div class="sales-chart-empty">Unable to render</div>`;
    }
  }

  function smartviewReflowChartInContainer(containerEl) {
    if (!containerEl || !window.Highcharts || !Array.isArray(window.Highcharts.charts)) return;
    window.Highcharts.charts.forEach(ch => {
      if (!ch || !ch.renderTo) return;
      if (ch.renderTo === containerEl) {
        try { ch.reflow(); } catch (e) {}
      }
    });
  }

  function smartviewRenderHighchartsForCards(items, onDone) {
    const arr = Array.isArray(items) ? items : [];
    let doneCalled = false;
    const done = function () {
      if (doneCalled) return;
      doneCalled = true;
      try { if (typeof onDone === 'function') onDone(); } catch (e) {}
    };
    if (!arr.length) {
      done();
      return;
    }

    smartviewEnsureHighchartsLoaded(function (ok) {
      if (!ok) {
        console.warn('Highcharts not available for SmartView KPI charts');
        done();
        return;
      }
      arr.forEach(item => {
        const el = document.getElementById(item.containerId || '');
        if (!el) return;
        const card = el.closest('.sales-chart-card');
        if (card) card.classList.add('has-live-chart');
        smartviewRenderSingleMiniChart(el, item);
      });
      // Reflow after layout settles so many cards in horizontal strip don't appear clipped.
      const reflowAll = function () {
        arr.forEach(item => {
          const el = document.getElementById(item.containerId || '');
          if (!el) return;
          smartviewReflowChartInContainer(el);
        });
      };
      if (typeof requestAnimationFrame === 'function') {
        requestAnimationFrame(function () {
          requestAnimationFrame(reflowAll);
        });
      }
      setTimeout(reflowAll, 90);
      setTimeout(function () {
        reflowAll();
        done();
      }, 240);
    });
  }

  function smartviewGetChartPopupState() {
    if (!window._smartviewChartPopupState || typeof window._smartviewChartPopupState !== 'object') {
      window._smartviewChartPopupState = {};
    }
    const s = window._smartviewChartPopupState;
    if (!Array.isArray(s.items)) s.items = [];
    if (!Number.isFinite(s.currentIndex)) s.currentIndex = 0;
    if (!Number.isFinite(s.activeResizeRafId)) s.activeResizeRafId = 0;
    return s;
  }

  function smartviewResolvePopupCreator() {
    try {
      if (typeof createPopup === 'function') return createPopup;
    } catch (e) {}
    try {
      if (window.parent && window.parent !== window && typeof window.parent.createPopup === 'function') {
        return function (url, modal) { return window.parent.createPopup(url, modal); };
      }
    } catch (e) {}
    return null;
  }

  function smartviewOpenFallbackChartModal() {
    let modalEl = document.getElementById('smartviewChartPopupModal');
    if (!modalEl) {
      const wrapper = document.createElement('div');
      wrapper.innerHTML = `
        <div id="smartviewChartPopupModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog modal-fullscreen modal-dialog-scrollable" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">Chart View</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
              </div>
              <div class="modal-body overflow-auto"></div>
            </div>
          </div>
        </div>
      `;
      modalEl = wrapper.firstElementChild;
      document.body.appendChild(modalEl);
    }
    if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
      const instance = bootstrap.Modal.getOrCreateInstance
        ? bootstrap.Modal.getOrCreateInstance(modalEl)
        : new bootstrap.Modal(modalEl);
      instance.show();
    } else {
      modalEl.classList.add('show');
      modalEl.style.display = 'block';
      modalEl.removeAttribute('aria-hidden');
      document.body.classList.add('modal-open');
    }
    return {
      modalElement: modalEl,
      modalBody: modalEl.querySelector('.modal-body') || modalEl
    };
  }

  function smartviewNormalizePopupChartIndex(index, total) {
    const len = Math.max(0, Number(total) || 0);
    if (!len) return 0;
    const raw = Number(index);
    const safe = Number.isFinite(raw) ? Math.trunc(raw) : 0;
    return ((safe % len) + len) % len;
  }

  function smartviewApplyPopupCardColorInputs(cardElement, colors) {
    if (!cardElement) return;
    const colorInputs = Array.from(cardElement.querySelectorAll('.analytics-grid-color-input'));
    if (!colorInputs.length) return;

    const fallbackColors = smartviewGetDefaultChartPaletteColors();
    const sourceColors = Array.isArray(colors) && colors.length ? colors : fallbackColors;
    colorInputs.forEach((input, index) => {
      const fallbackColor = fallbackColors[index % fallbackColors.length];
      const sourceColor = sourceColors[index] || sourceColors[index % sourceColors.length];
      input.value = smartviewNormalizeColorHex(sourceColor, fallbackColor);
    });
  }

  function smartviewGetPopupCardCustomColors(cardElement) {
    const fallbackColors = smartviewGetDefaultChartPaletteColors();
    const colorInputs = cardElement ? Array.from(cardElement.querySelectorAll('.analytics-grid-color-input')) : [];
    if (!colorInputs.length) return fallbackColors.slice();

    return colorInputs.map((input, index) => {
      const fallbackColor = fallbackColors[index % fallbackColors.length];
      return smartviewNormalizeColorHex(input.value, fallbackColor);
    });
  }

  function smartviewClosePopupChartMenus(exceptCardElement) {
    const state = smartviewGetChartPopupState();
    if (!state.gridEl) return;
    state.gridEl.querySelectorAll('.smartview-popup-chart-card.menu-open').forEach(cardElement => {
      if (exceptCardElement && cardElement === exceptCardElement) return;
      cardElement.classList.remove('menu-open');
    });
  }

  function smartviewGetPopupGridTotalTracks() {
    return 12;
  }

  function smartviewGetPopupGridDefaultSpan() {
    return window.innerWidth <= 991 ? smartviewGetPopupGridTotalTracks() : 6;
  }

  function smartviewGetPopupGridMinSpan() {
    return window.innerWidth <= 991 ? smartviewGetPopupGridTotalTracks() : 6;
  }

  function smartviewApplyPopupGridCardSpan(cardElement, spanValue) {
    if (!cardElement) return;
    const totalTracks = smartviewGetPopupGridTotalTracks();
    const safeSpan = Math.max(1, Math.min(totalTracks, Number(spanValue) || smartviewGetPopupGridDefaultSpan()));
    cardElement.style.gridColumn = `span ${safeSpan}`;
    cardElement.setAttribute('data-card-span', `${safeSpan}`);
  }

  function smartviewApplyPopupGridLayout(gridEl, items) {
    if (!gridEl) return;
    const totalTracks = smartviewGetPopupGridTotalTracks();
    const forceFullSpan = window.innerWidth <= 991;
    const defaultSpan = smartviewGetPopupGridDefaultSpan();
    gridEl.style.gridTemplateColumns = `repeat(${totalTracks}, minmax(0, 1fr))`;
    gridEl.setAttribute('data-total-tracks', `${totalTracks}`);

    Array.from(gridEl.querySelectorAll('.smartview-popup-chart-card')).forEach((cardElement, itemIndex) => {
      const item = Array.isArray(items) ? items[itemIndex] : null;
      const savedSpan = Number((item && item.popupSpan) || cardElement.getAttribute('data-card-span'));
      const nextSpan = forceFullSpan
        ? totalTracks
        : ((Number.isFinite(savedSpan) && savedSpan > 0) ? savedSpan : defaultSpan);
      smartviewApplyPopupGridCardSpan(cardElement, nextSpan);
    });
  }

  function smartviewBindPopupChartResize(cardElement, handleElement) {
    if (!cardElement || !handleElement || handleElement.dataset.bound === 'true') return;

    handleElement.addEventListener('pointerdown', function (event) {
      if (window.innerWidth <= 991) return;
      event.preventDefault();
      event.stopPropagation();

      const state = smartviewGetChartPopupState();
      const grid = state.gridEl || cardElement.closest('.smartview-chart-popup-grid');
      if (!grid) return;

      const gridRect = grid.getBoundingClientRect();
      const cardRect = cardElement.getBoundingClientRect();
      const totalTracks = smartviewGetPopupGridTotalTracks();
      const computedStyle = window.getComputedStyle(grid);
      const gap = parseFloat(computedStyle.columnGap || computedStyle.gap || '14') || 14;
      const usableWidth = Math.max(0, gridRect.width - (gap * (totalTracks - 1)));
      const trackWidth = usableWidth > 0 ? (usableWidth / totalTracks) : 0;
      if (trackWidth <= 0) return;

      const minSpan = smartviewGetPopupGridMinSpan();
      const maxSpan = totalTracks;
      const startX = Number(event.clientX) || 0;
      const startWidth = Math.max(cardRect.width, (trackWidth * minSpan) + (gap * (minSpan - 1)));
      const chartIndex = Number(cardElement.getAttribute('data-chart-index'));
      const item = Array.isArray(state.items) ? state.items[chartIndex] : null;

      cardElement.classList.add('resizing');
      try {
        if (handleElement.setPointerCapture && event.pointerId !== undefined) {
          handleElement.setPointerCapture(event.pointerId);
        }
      } catch (e) {}

      const onPointerMove = function (moveEvent) {
        const deltaX = (Number(moveEvent.clientX) || startX) - startX;
        const nextWidth = Math.max(startWidth + deltaX, (trackWidth * minSpan) + (gap * (minSpan - 1)));
        const rawSpan = (nextWidth + gap) / (trackWidth + gap);
        const nextSpan = Math.max(minSpan, Math.min(maxSpan, Math.round(rawSpan)));

        if (item) item.popupSpan = nextSpan;
        cardElement.setAttribute('data-custom-span', 'true');
        smartviewApplyPopupGridCardSpan(cardElement, nextSpan);

        if (state.activeResizeRafId) {
          cancelAnimationFrame(state.activeResizeRafId);
        }
        state.activeResizeRafId = requestAnimationFrame(function () {
          const chartEl = cardElement.querySelector('.smartview-popup-chart-canvas');
          if (chartEl) smartviewReflowChartInContainer(chartEl);
          state.activeResizeRafId = 0;
        });
      };

      const stopResize = function () {
        window.removeEventListener('pointermove', onPointerMove);
        window.removeEventListener('pointerup', stopResize);
        window.removeEventListener('pointercancel', stopResize);
        cardElement.classList.remove('resizing');
        try {
          if (handleElement.releasePointerCapture && event.pointerId !== undefined) {
            handleElement.releasePointerCapture(event.pointerId);
          }
        } catch (e) {}

        if (state.activeResizeRafId) {
          cancelAnimationFrame(state.activeResizeRafId);
          state.activeResizeRafId = 0;
        }

        const finalSpan = Math.max(smartviewGetPopupGridMinSpan(), Number(cardElement.getAttribute('data-card-span')) || smartviewGetPopupGridDefaultSpan());
        if (item) item.popupSpan = finalSpan;
        cardElement.setAttribute('data-custom-span', 'true');
        smartviewApplyPopupGridCardSpan(cardElement, finalSpan);
        if (item) smartviewRenderPopupChartCard(cardElement, item);
      };

      window.addEventListener('pointermove', onPointerMove);
      window.addEventListener('pointerup', stopResize);
      window.addEventListener('pointercancel', stopResize);
    });

    handleElement.dataset.bound = 'true';
  }

  function smartviewRenderPopupChartCard(cardElement, item) {
    if (!cardElement || !item) return;
    const chartEl = cardElement.querySelector('.smartview-popup-chart-canvas');
    if (!chartEl) return;

    const popupHeight = smartviewGetPopupChartCardHeight(item.chartType || item.baseChartType, Array.isArray(item.points) ? item.points.length : 0);
    chartEl.style.height = `${popupHeight}px`;
    chartEl.style.minHeight = `${Math.max(320, Math.min(popupHeight, 420))}px`;
    smartviewRenderSingleMiniChart(chartEl, item, { expanded: true, height: popupHeight });
    setTimeout(function () {
      smartviewReflowChartInContainer(chartEl);
    }, 60);
  }

  function smartviewSyncPageChartCard(item) {
    const chartEl = document.getElementById(item && item.containerId ? item.containerId : '');
    if (!chartEl) return;
    const card = chartEl.closest('.sales-chart-card');
    if (card) card.classList.add('has-live-chart');
    smartviewRenderSingleMiniChart(chartEl, item);
    setTimeout(function () {
      smartviewReflowChartInContainer(chartEl);
    }, 60);
  }

  function smartviewScrollPopupChartIntoView(index) {
    const state = smartviewGetChartPopupState();
    const items = Array.isArray(state.items) ? state.items : [];
    if (!items.length || !state.gridEl) return;

    state.currentIndex = smartviewNormalizePopupChartIndex(index, items.length);
    state.gridEl.querySelectorAll('.smartview-popup-chart-card--focus').forEach(card => {
      card.classList.remove('smartview-popup-chart-card--focus');
    });

    const targetCard = state.gridEl.querySelector(`.smartview-popup-chart-card[data-chart-index="${state.currentIndex}"]`);
    if (!targetCard) return;
    targetCard.classList.add('smartview-popup-chart-card--focus');
    try {
      targetCard.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'nearest' });
    } catch (e) {}
  }

  function smartviewBindChartPopupGridEvents(gridEl) {
    if (!gridEl || gridEl.__smartviewPopupEventsBound) return;
    gridEl.__smartviewPopupEventsBound = true;

    const applyPopupColorChange = function (event) {
      const colorInput = event.target.closest('.analytics-grid-color-input');
      if (!colorInput) return false;

      const cardElement = colorInput.closest('.smartview-popup-chart-card');
      const state = smartviewGetChartPopupState();
      const chartIndex = Number(cardElement && cardElement.getAttribute('data-chart-index'));
      const item = Array.isArray(state.items) ? state.items[chartIndex] : null;
      if (!cardElement || !item) return true;

      item.paletteConfig = smartviewResolveChartPaletteConfig({
        paletteKey: 'custom',
        customColors: smartviewGetPopupCardCustomColors(cardElement)
      });
      smartviewPersistChartCustomization(item, chartIndex);
      smartviewRenderPopupChartCard(cardElement, item);
      smartviewSyncPageChartCard(item);
      return true;
    };

    gridEl.addEventListener('click', function (event) {
      const menuButton = event.target.closest('.smartview-popup-chart-menu-btn');
      const resizeButton = event.target.closest('.smartview-popup-chart-resize-btn');
      const resetButton = event.target.closest('.smartview-popup-color-reset-btn');
      const cardElement = event.target.closest('.smartview-popup-chart-card');

      if (resizeButton) {
        event.preventDefault();
        event.stopPropagation();
        return;
      }

      if (menuButton && cardElement) {
        event.preventDefault();
        event.stopPropagation();
        const shouldOpen = !cardElement.classList.contains('menu-open');
        smartviewClosePopupChartMenus(shouldOpen ? cardElement : null);
        if (shouldOpen) {
          cardElement.classList.add('menu-open');
          smartviewScrollPopupChartIntoView(Number(cardElement.getAttribute('data-chart-index')));
        } else {
          cardElement.classList.remove('menu-open');
        }
        return;
      }

      if (resetButton && cardElement) {
        event.preventDefault();
        event.stopPropagation();
        const state = smartviewGetChartPopupState();
        const chartIndex = Number(cardElement.getAttribute('data-chart-index'));
        const item = Array.isArray(state.items) ? state.items[chartIndex] : null;
        if (!item) return;

        const defaultColors = smartviewGetDefaultChartPaletteColors();
        smartviewApplyPopupCardColorInputs(cardElement, defaultColors);
        item.paletteConfig = smartviewResolveChartPaletteConfig({
          paletteKey: 'custom',
          customColors: defaultColors
        });
        smartviewPersistChartCustomization(item, chartIndex);
        smartviewRenderPopupChartCard(cardElement, item);
        smartviewSyncPageChartCard(item);
        return;
      }

      if (event.target.closest('.analytics-grid-menu-panel')) {
        event.stopPropagation();
        return;
      }

      if (cardElement) {
        smartviewScrollPopupChartIntoView(Number(cardElement.getAttribute('data-chart-index')));
        return;
      }

      smartviewClosePopupChartMenus(null);
    });

    gridEl.addEventListener('change', function (event) {
      if (applyPopupColorChange(event)) return;

      const chartSelect = event.target.closest('.smartview-popup-chart-select');
      if (!chartSelect) return;

      const cardElement = chartSelect.closest('.smartview-popup-chart-card');
      const state = smartviewGetChartPopupState();
      const chartIndex = Number(cardElement && cardElement.getAttribute('data-chart-index'));
      const item = Array.isArray(state.items) ? state.items[chartIndex] : null;
      if (!cardElement || !item) return;

      item.chartType = smartviewMapMiniChartType(chartSelect.value || item.chartType || item.baseChartType);
      item.paletteConfig = smartviewResolveChartPaletteConfig({
        paletteKey: 'custom',
        customColors: smartviewGetPopupCardCustomColors(cardElement)
      });
      smartviewPersistChartCustomization(item, chartIndex);
      smartviewRenderPopupChartCard(cardElement, item);
      smartviewSyncPageChartCard(item);
    });

    gridEl.addEventListener('input', function (event) {
      applyPopupColorChange(event);
    });
  }

  function smartviewRenderChartPopupAt(index) {
    const state = smartviewGetChartPopupState();
    const items = Array.isArray(state.items) ? state.items : [];
    if (!items.length || !state.gridEl) return;

    state.currentIndex = smartviewNormalizePopupChartIndex(index, items.length);
    const chartTypes = smartviewGetSupportedChartTypes();
    const defaultColors = smartviewGetDefaultChartPaletteColors();

    state.gridEl.innerHTML = items.map((item, itemIndex) => {
      smartviewApplyStoredChartCustomization(item, itemIndex);
      const chartType = smartviewMapMiniChartType(item.chartType || item.baseChartType);
      const paletteConfig = smartviewResolveChartPaletteConfig(item.paletteConfig || 'newPalette');
      const chartOptionsHtml = chartTypes.map(option => {
        const selectedAttr = smartviewMapMiniChartType(option.type) === chartType ? ' selected' : '';
        return `<option value="${escapeHtml(option.type)}"${selectedAttr}>${escapeHtml(option.name)}</option>`;
      }).join('');
      const colorInputsHtml = defaultColors.map((fallbackColor, colorIndex) => {
        const colorValue = paletteConfig.customColors[colorIndex] || fallbackColor;
        return `<input type="color" class="analytics-grid-color-input" data-color-index="${colorIndex}" value="${escapeHtml(colorValue)}" title="Color ${colorIndex + 1}" aria-label="Color ${colorIndex + 1}" />`;
      }).join('');
      return `
        <div class="analytics-grid-card smartview-popup-chart-card${itemIndex === state.currentIndex ? ' smartview-popup-chart-card--focus' : ''}" data-chart-index="${itemIndex}">
          <div class="analytics-grid-card-header smartview-popup-chart-card-header">
            <div class="smartview-popup-chart-title-wrap">
              <div class="smartview-popup-chart-title">${escapeHtml(item.title || 'Chart')}</div>
              ${item.subtitle ? `<div class="smartview-popup-chart-subtitle">${escapeHtml(item.subtitle)}</div>` : ''}
            </div>
            <div class="analytics-grid-menu-wrap smartview-popup-chart-menu-wrap">
              <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm smartview-popup-chart-resize-btn" title="Resize card" aria-label="Resize card">
                <span class="material-icons material-icons-style material-icons-2">drag_indicator</span>
              </button>
              <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm smartview-popup-chart-menu-btn" title="Chart options" aria-label="Chart options">
                <span class="material-icons material-icons-style material-icons-2">tune</span>
              </button>
              <div class="analytics-grid-menu-panel smartview-popup-chart-menu-panel">
                <div class="analytics-grid-menu-item">
                  <label class="analytics-grid-menu-label">Chart type</label>
                  <select class="form-select form-select-sm smartview-popup-chart-select" title="Chart type">
                    ${chartOptionsHtml}
                  </select>
                </div>
                <div class="analytics-grid-menu-item">
                  <label class="analytics-grid-menu-label">Custom colors</label>
                  <div class="analytics-grid-color-picker-wrap">
                    <div class="analytics-grid-color-picker-list">
                      ${colorInputsHtml}
                    </div>
                    <button type="button" class="btn btn-link btn-sm analytics-grid-color-reset-btn smartview-popup-color-reset-btn">Reset</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="analytics-grid-card-body smartview-popup-chart-card-body">
            <div class="analytics-grid-chart smartview-popup-chart-canvas"></div>
          </div>
        </div>
      `;
    }).join('');

    smartviewApplyPopupGridLayout(state.gridEl, items);
    Array.from(state.gridEl.querySelectorAll('.smartview-popup-chart-card')).forEach((cardElement, itemIndex) => {
      const resizeButton = cardElement.querySelector('.smartview-popup-chart-resize-btn');
      if (resizeButton) smartviewBindPopupChartResize(cardElement, resizeButton);
      smartviewRenderPopupChartCard(cardElement, items[itemIndex]);
    });

    setTimeout(function () {
      smartviewScrollPopupChartIntoView(state.currentIndex);
    }, 80);
  }

  function smartviewMoveChartPopup(step) {
    const state = smartviewGetChartPopupState();
    const items = Array.isArray(state.items) ? state.items : [];
    if (items.length <= 1) return;
    const delta = Number(step);
    const inc = Number.isFinite(delta) ? Math.trunc(delta) : 0;
    smartviewScrollPopupChartIntoView(state.currentIndex + inc);
  }

  function smartviewCleanupChartPopupState() {
    const state = smartviewGetChartPopupState();
    if (state.modalBody) {
      try {
        state.modalBody.querySelectorAll('.smartview-chart-popup-content').forEach(el => el.remove());
      } catch (e) {}
      try {
        state.modalBody.classList.remove('overflow-auto');
        state.modalBody.classList.add('overflow-hidden');
      } catch (e) {}
    }
    if (state.iframeEl) {
      try {
        state.iframeEl.style.display = '';
        state.iframeEl.removeAttribute('aria-hidden');
      } catch (e) {}
    }
    if (state.resizeHandler) {
      try { window.removeEventListener('resize', state.resizeHandler); } catch (e) {}
    }
    if (state.activeResizeRafId) {
      try { cancelAnimationFrame(state.activeResizeRafId); } catch (e) {}
    }

    state.items = [];
    state.currentIndex = 0;
    state.activeResizeRafId = 0;
    state.popupModal = null;
    state.modalElement = null;
    state.modalBody = null;
    state.iframeEl = null;
    state.gridEl = null;
    state.popupContentEl = null;
    state.chartContainerEl = null;
    state.titleEl = null;
    state.subtitleEl = null;
    state.counterEl = null;
    state.prevBtn = null;
    state.nextBtn = null;
    state.resizeHandler = null;
  }

  function smartviewOpenChartPopup(items, index) {
    const arr = Array.isArray(items) ? items : [];
    if (!arr.length) {
      if (typeof showAlertDialog === 'function') showAlertDialog('warning', 'No chart data available to open.');
      return;
    }

    smartviewEnsureHighchartsLoaded(function (ok) {
      if (!ok) {
        if (typeof showAlertDialog === 'function') showAlertDialog('warning', 'Highcharts is not available.');
        return;
      }

      const state = smartviewGetChartPopupState();
      if (state.resizeHandler) {
        try { window.removeEventListener('resize', state.resizeHandler); } catch (e) {}
        state.resizeHandler = null;
      }
      state.items = arr.map((item, itemIndex) => smartviewApplyStoredChartCustomization(item, itemIndex));
      state.currentIndex = smartviewNormalizePopupChartIndex(index, arr.length);

      let popup = null;
      const popupCreator = smartviewResolvePopupCreator();
      if (popupCreator) {
        try { popup = popupCreator('about:blank', false); } catch (e) { popup = null; }
      }
      if (!popup || !popup.modalBody) popup = smartviewOpenFallbackChartModal();
      if (!popup || !popup.modalBody) {
        if (typeof showAlertDialog === 'function') showAlertDialog('warning', 'Unable to open popup.');
        return;
      }

      state.popupModal = popup;
      state.modalBody = popup.modalBody;
      state.modalElement = popup.modalElement || null;

      const iframe = state.modalBody.querySelector('#loadPopUpPage');
      if (iframe) {
        iframe.style.display = 'none';
        iframe.setAttribute('aria-hidden', 'true');
        iframe.src = 'about:blank';
        state.iframeEl = iframe;
      } else {
        state.iframeEl = null;
      }

      state.modalBody.classList.remove('overflow-hidden');
      state.modalBody.classList.add('overflow-auto');
      state.modalBody.querySelectorAll('.smartview-chart-popup-content').forEach(el => el.remove());

      state.modalBody.insertAdjacentHTML('beforeend', `
        <div class="smartview-chart-popup-content smartview-chart-popup-content--all">
          <div class="smartview-chart-popup-toolbar smartview-chart-popup-toolbar--all">
            <div class="smartview-chart-popup-title-wrap">
              <div class="smartview-chart-popup-title">All Charts</div>
              <div class="smartview-chart-popup-subtitle">${arr.length} chart${arr.length === 1 ? '' : 's'} available</div>
            </div>
          </div>
          <div class="smartview-chart-popup-grid"></div>
        </div>
      `);

      state.popupContentEl = state.modalBody.querySelector('.smartview-chart-popup-content');
      state.gridEl = state.modalBody.querySelector('.smartview-chart-popup-grid');
      smartviewBindChartPopupGridEvents(state.gridEl);

      state.resizeHandler = function () {
        try {
          const resizeState = smartviewGetChartPopupState();
          if (!resizeState.gridEl) return;
          smartviewApplyPopupGridLayout(resizeState.gridEl, resizeState.items);
          Array.from(resizeState.gridEl.querySelectorAll('.smartview-popup-chart-card')).forEach((cardElement, itemIndex) => {
            smartviewRenderPopupChartCard(cardElement, resizeState.items[itemIndex]);
          });
          smartviewScrollPopupChartIntoView(resizeState.currentIndex);
        } catch (e) {}
      };
      window.addEventListener('resize', state.resizeHandler);

      const renderPopup = function () {
        smartviewRenderChartPopupAt(state.currentIndex);
      };
      if (state.modalElement && !state.modalElement.classList.contains('show')) {
        state.modalElement.addEventListener('shown.bs.modal', renderPopup, { once: true });
      } else {
        setTimeout(renderPopup, 100);
      }

      if (state.modalElement) {
        state.modalElement.addEventListener('hide.bs.modal', function () {
          smartviewCleanupChartPopupState();
        }, { once: true });
      }
    });
  }

  function smartviewSyncOpenChartPopupWithRenderedItems(items) {
    const state = smartviewGetChartPopupState();
    const popupDoc = (state && state.modalBody && state.modalBody.ownerDocument) ? state.modalBody.ownerDocument : document;
    const popupIsActive = !!(
      state &&
      state.modalBody &&
      state.gridEl &&
      state.popupContentEl &&
      state.modalBody.isConnected &&
      state.gridEl.isConnected &&
      state.popupContentEl.isConnected &&
      popupDoc.body &&
      popupDoc.body.contains(state.modalBody)
    );
    if (!popupIsActive) return;

    const nextItemsRaw = Array.isArray(items) ? items : [];
    const prevItems = Array.isArray(state.items) ? state.items : [];
    const prevCurrentItem = prevItems[state.currentIndex] || null;
    const prevCurrentKey = prevCurrentItem && prevCurrentItem.stateKey ? String(prevCurrentItem.stateKey) : '';

    const spanByStateKey = {};
    prevItems.forEach(function (item) {
      if (!item || !item.stateKey) return;
      const span = Number(item.popupSpan);
      if (Number.isFinite(span) && span > 0) spanByStateKey[String(item.stateKey)] = span;
    });

    const nextItems = nextItemsRaw.map(function (item, idx) {
      const normalized = smartviewApplyStoredChartCustomization(item, idx);
      const key = normalized && normalized.stateKey ? String(normalized.stateKey) : '';
      if (key && spanByStateKey[key]) normalized.popupSpan = spanByStateKey[key];
      return normalized;
    });

    const subtitleEl = state.modalBody.querySelector('.smartview-chart-popup-subtitle');
    if (subtitleEl) subtitleEl.textContent = `${nextItems.length} chart${nextItems.length === 1 ? '' : 's'} available`;

    if (!nextItems.length) {
      state.items = [];
      state.currentIndex = 0;
      state.gridEl.innerHTML = '<div style="padding:24px;text-align:center;color:#666;">No chart data available for the current filter.</div>';
      return;
    }

    let nextIndex = state.currentIndex;
    if (prevCurrentKey) {
      const matchedIdx = nextItems.findIndex(function (item) {
        return item && String(item.stateKey || '') === prevCurrentKey;
      });
      if (matchedIdx >= 0) nextIndex = matchedIdx;
    }

    nextIndex = Math.max(0, Math.min(nextItems.length - 1, Number.isFinite(Number(nextIndex)) ? Math.trunc(Number(nextIndex)) : 0));
    state.items = nextItems;
    state.currentIndex = smartviewNormalizePopupChartIndex(nextIndex, nextItems.length);
    smartviewRenderChartPopupAt(state.currentIndex);
  }

  function smartviewRenderChartCards(items, options = {}) {
    const done = function () {
      try { if (typeof options.onRendered === 'function') options.onRendered(); } catch (e) {}
    };
    const wrap = document.querySelector('.Sales-data-charts-wrapper');
    if (!wrap) {
      done();
      return;
    }
    const arr = Array.isArray(items) ? items : [];
    if (!arr.length) {
      wrap.innerHTML = '';
      window._smartviewRenderedChartItems = [];
      smartviewSetElementHasData(wrap, false);
      try { smartviewSyncOpenChartPopupWithRenderedItems([]); } catch (e) {}
      try { smartviewRefreshOptionsMenuLabels(); } catch (e) {}
      done();
      return;
    }
    window._smartviewRenderedChartItems = arr;

    const stamp = Date.now();
    let cardsHtml = '';
    arr.forEach((it, idx) => {
      smartviewApplyStoredChartCustomization(it, idx);
      const title = escapeHtml(it.title || 'Chart');
      const subtitle = escapeHtml(it.subtitle || '');
      const containerId = `sv-smart-kpi-chart-${stamp}-${idx}`;
      it.containerId = containerId;
      cardsHtml += `
        <div class="Carousel-Items Sales-data-content">
          <div class="card-custom sales-chart-card">
            <button type="button" class="btn btn-icon btn-white btn-color-gray-600 btn-active-primary shadow-sm tb-btn btn-sm sv-chart-popup-btn" data-chart-index="${idx}" title="Open all charts in popup" aria-label="Open all charts in popup">
              <span class="material-icons material-icons-style material-icons-2">open_in_full</span>
            </button>
            <div class="sales-chart-meta">
              <div class="title">${title}</div>
              ${subtitle ? `<div class="sales-chart-subtitle">${subtitle}</div>` : ''}
            </div>
            <div class="sales-chart-container" id="${containerId}"></div>
          </div>
        </div>
      `;
    });
    wrap.innerHTML = `
      <div class="carousel-wrapper">
        <button type="button" class="carousel-btn btn-left" aria-label="Scroll chart cards left">
          <span class="material-icons material-icons-style material-icons-2">keyboard_arrow_left</span>
        </button>
        <div class="Carousel-Items-Container">
          ${cardsHtml}
        </div>
        <button type="button" class="carousel-btn btn-right" aria-label="Scroll chart cards right">
          <span class="material-icons material-icons-style material-icons-2">keyboard_arrow_right</span>
        </button>
      </div>
    `;
    smartviewBindCarouselControls(wrap);
    smartviewSetElementHasData(wrap, true);
    try { smartviewSyncOpenChartPopupWithRenderedItems(arr); } catch (e) {}
    wrap.querySelectorAll('.sv-chart-popup-btn').forEach(btn => {
      btn.addEventListener('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        const index = Number(this.getAttribute('data-chart-index'));
        smartviewOpenChartPopup(arr, Number.isFinite(index) ? index : 0);
      });
    });
    smartviewRenderHighchartsForCards(arr, function () {
      smartviewUpdateCarouselButtons(wrap);
      done();
    });
    try { smartviewRefreshOptionsMenuLabels(); } catch (e) {}
  }

  function smartviewGetKpiChartCacheStore() {
    if (!window._smartviewKpiChartCacheByAds || typeof window._smartviewKpiChartCacheByAds !== 'object') {
      window._smartviewKpiChartCacheByAds = {};
    }
    return window._smartviewKpiChartCacheByAds;
  }

  function smartviewGetKpiChartCacheKey(adsName, filters) {
    const base = String(adsName || '').trim().toLowerCase();
    const safeFilters = stripSmartviewFilterTransId(Array.isArray(filters) ? filters : []);
    if (!safeFilters.length) return base;
    return `${base}::filters::${smartviewFastHash(smartviewStableStringify(safeFilters))}`;
  }

  function smartviewLoadKpiChartsData(adsName, options = {}) {
    if (window._smartviewDisableKpiCharts) return;
    const finishLoading = smartviewBeginKpiChartsLoader();
    const cacheStore = smartviewGetKpiChartCacheStore();
    const renderAndFinalize = function (kpiItems, chartItems) {
      try {
        smartviewRenderKpiCards(Array.isArray(kpiItems) ? kpiItems : []);
      } catch (e) {
        console.warn('smartviewLoadKpiChartsData kpi render failed', e);
      }
      try {
        smartviewRenderChartCards(Array.isArray(chartItems) ? chartItems : [], { onRendered: finishLoading });
      } catch (e) {
        console.warn('smartviewLoadKpiChartsData chart render failed', e);
        try { finishLoading(); } catch (_e) {}
      }
    };

    try {
      const selectedAds = (adsName || getQueryParam('ads') || getQueryParam('adsName') || getQueryParam('adsname') || '').toString().trim();
      const ctrlForCache = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
      const cacheFilters = (ctrlForCache && Array.isArray(ctrlForCache.filters)) ? ctrlForCache.filters : [];
      const cacheKey = smartviewGetKpiChartCacheKey(selectedAds, cacheFilters);
      const cachedPayload = cacheKey ? cacheStore[cacheKey] : null;
      if (!selectedAds) {
        if (cachedPayload && (Array.isArray(cachedPayload.kpiItems) || Array.isArray(cachedPayload.chartItems))) {
          renderAndFinalize(cachedPayload.kpiItems || [], cachedPayload.chartItems || []);
        } else {
          renderAndFinalize([], []);
        }
        return;
      }

      const caller = smartviewResolveAxListCaller();
      if (!caller || typeof caller.GetDataFromAxList !== 'function') {
        console.warn('smartviewLoadKpiChartsData: GetDataFromAxList not available');
        if (cachedPayload && (Array.isArray(cachedPayload.kpiItems) || Array.isArray(cachedPayload.chartItems))) {
          renderAndFinalize(cachedPayload.kpiItems || [], cachedPayload.chartItems || []);
        } else {
          renderAndFinalize([], []);
        }
        return;
      }

      window._smartviewKpiChartRequestSeq = (window._smartviewKpiChartRequestSeq || 0) + 1;
      const reqSeq = window._smartviewKpiChartRequestSeq;

      const configParams = {
        adsNames: ['ds_smartlist_kpicharts'],
        refreshCache: !!options.refreshCache,
        sqlParams: { adsname: selectedAds },
        props: {
          ADS: false,
          CachePermissions: true,
          getallrecordscount: false,
          pageno: 1,
          pagesize: 0,
          sorting: [],
          filters: []
        }
      };

      caller.GetDataFromAxList(configParams, function (response) {
        try {
          if (reqSeq !== window._smartviewKpiChartRequestSeq) {
            finishLoading();
            return;
          }
          const extracted = smartviewExtractRowsFromAxListResponse(response);
          const configRows = extracted.rows || [];
          if (!configRows.length) {
            console.log('smartviewLoadKpiChartsData: no config rows for', selectedAds);
            if (cachedPayload && (Array.isArray(cachedPayload.kpiItems) || Array.isArray(cachedPayload.chartItems))) {
              renderAndFinalize(cachedPayload.kpiItems || [], cachedPayload.chartItems || []);
            } else {
              renderAndFinalize([], []);
            }
            return;
          }

          smartviewFetchAllRowsForAds(selectedAds, function (_err, sourceRows) {
            try {
              if (reqSeq !== window._smartviewKpiChartRequestSeq) {
                finishLoading();
                return;
              }
              const fallbackRows = Array.isArray(window._smartviewFullData) && window._smartviewFullData.length
                ? window._smartviewFullData
                : (Array.isArray(window._entity?.listJson) ? window._entity.listJson : []);
              const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
              const hasActiveFilters = !!(ctrl && Array.isArray(ctrl.filters) && ctrl.filters.length);
              const effectiveRows = hasActiveFilters
                ? (Array.isArray(sourceRows) ? sourceRows : [])
                : ((Array.isArray(sourceRows) && sourceRows.length) ? sourceRows : fallbackRows);

              const kpiItems = smartviewBuildKpiItemsFromRows(configRows, effectiveRows);
              const chartItems = smartviewBuildChartItemsFromRows(configRows, effectiveRows);
              if ((Array.isArray(kpiItems) && kpiItems.length) || (Array.isArray(chartItems) && chartItems.length)) {
                cacheStore[cacheKey] = {
                  kpiItems: smartviewCloneJsonSafe(kpiItems || []),
                  chartItems: smartviewCloneJsonSafe(chartItems || [])
                };
              }
              renderAndFinalize(kpiItems, chartItems);
            } catch (e) {
              console.warn('smartviewLoadKpiChartsData bind failed', e);
              if (cachedPayload && (Array.isArray(cachedPayload.kpiItems) || Array.isArray(cachedPayload.chartItems))) {
                renderAndFinalize(cachedPayload.kpiItems || [], cachedPayload.chartItems || []);
              } else {
                renderAndFinalize([], []);
              }
            }
          });
        } catch (e) {
          console.warn('smartviewLoadKpiChartsData parse failed', e);
          if (cachedPayload && (Array.isArray(cachedPayload.kpiItems) || Array.isArray(cachedPayload.chartItems))) {
            renderAndFinalize(cachedPayload.kpiItems || [], cachedPayload.chartItems || []);
          } else {
            renderAndFinalize([], []);
          }
        }
      }, function (err) {
        console.warn('smartviewLoadKpiChartsData failed', err);
        if (cachedPayload && (Array.isArray(cachedPayload.kpiItems) || Array.isArray(cachedPayload.chartItems))) {
          renderAndFinalize(cachedPayload.kpiItems || [], cachedPayload.chartItems || []);
        } else {
          renderAndFinalize([], []);
        }
      });
    } catch (e) {
      console.warn('smartviewLoadKpiChartsData exception', e);
      if (cacheStore) {
        const selectedAds = (adsName || getQueryParam('ads') || getQueryParam('adsName') || getQueryParam('adsname') || '').toString().trim();
        const ctrlForCache = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
        const cacheFilters = (ctrlForCache && Array.isArray(ctrlForCache.filters)) ? ctrlForCache.filters : [];
        const cacheKey = smartviewGetKpiChartCacheKey(selectedAds, cacheFilters);
        const cachedPayload = cacheKey ? cacheStore[cacheKey] : null;
        if (cachedPayload && (Array.isArray(cachedPayload.kpiItems) || Array.isArray(cachedPayload.chartItems))) {
          renderAndFinalize(cachedPayload.kpiItems || [], cachedPayload.chartItems || []);
          return;
        }
      }
      renderAndFinalize([], []);
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
      if (smartviewIsMetaFieldHidden(field)) return;

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

  function exportHiddenTableToWord(fileNameBase) {
    try {
      const table = document.getElementById('hiddenTable');
      if (!table) return false;

      const docHtml = `<!DOCTYPE html>
  <html>
  <head>
    <meta charset="utf-8">
    <title>${escapeHtml(fileNameBase || 'export')}</title>
  </head>
  <body>
  ${table.outerHTML}
  </body>
  </html>`;

      const blob = new Blob(['\ufeff', docHtml], { type: 'application/msword;charset=utf-8' });
      const url = URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `${fileNameBase || 'export'}.doc`;
      document.body.appendChild(link);
      link.click();
      link.remove();
      setTimeout(() => {
        try { URL.revokeObjectURL(url); } catch (e) {}
      }, 500);
      return true;
    } catch (e) {
      console.error('exportHiddenTableToWord failed', e);
      return false;
    }
  }

  function handleExport(action, tableSelector) {
    // action: 'pdf'|'excel'|'word'|'print' (data-target value from menu)
    action = (action || '').toString().toUpperCase();

    try {
      createHiddenTableFromMetadata();
      const fileNameBase = (_entity && (_entity.adsName || _entity.entityName))
        ? String(_entity.adsName || _entity.entityName).replace(/\s+/g, '_')
        : 'export';

      // Word export is custom (DataTables Buttons has no native "word" button).
      if (action === 'WORD' || action === 'DOC' || action === 'DOCX') {
        const ok = exportHiddenTableToWord(fileNameBase);
        if (!ok) throw new Error('Word export failed');
        return;
      }

      if ($.fn.dataTable && $.fn.dataTable.isDataTable('#hiddenTable')) {
        $('#hiddenTable').DataTable().destroy();
      }

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
      const metaItem = resolved.meta || {};
      let dt = smartviewInferFilterDatatype({ datatype: (metaItem && metaItem.fdatatype) || '' }, metaItem);
      const cd = (metaItem && metaItem.cdatatype) ? String(metaItem.cdatatype).toLowerCase() : '';
      if (cd.includes('timestamp') || cd.includes('time stamp')) dt = 'TIMESTAMP';

      if (dt === 'DROPDOWN') {
        filters.push({ fldname: fld, datatype: 'DROPDOWN', value: [val] });
      } else if (dt === 'NUMERIC') {
        filters.push({ fldname: fld, datatype: 'NUMERIC', from: val, to: val });
      } else if (dt === 'DATE' || dt === 'TIMESTAMP') {
        // Normalize date/timestamp to DD/MM/YYYY (and add time for TIMESTAMP) to match backend expectations.
        let fromVal = val;
        let toVal = val;
        try {
          const m = moment(val, ['YYYY-MM-DD', 'YYYY-MM-DDTHH:mm:ss', 'DD/MM/YYYY', 'MM/DD/YYYY', 'DD-MMM-YYYY'], true);
          if (m.isValid()) {
            if (dt === 'TIMESTAMP') {
              fromVal = m.format('DD/MM/YYYY 00:00:00');
              toVal = m.format('DD/MM/YYYY 23:59:59');
            } else {
              fromVal = m.format('DD/MM/YYYY');
              toVal = m.format('DD/MM/YYYY');
            }
          }
        } catch (e) {}
        filters.push({ fldname: fld, datatype: dt, from: fromVal, to: toVal, condition: 'customOption' });
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

    let firstHyperlinkFieldName = '';
    ordered.some(m => {
      const fn = (m.fldname || '').toString().toLowerCase();
      if (!fn) return false;
      if (!smartviewHasHyperlinkMeta(m)) return false;
      firstHyperlinkFieldName = fn;
      return true;
    });

    let html = '<table class="table table-sm">';
    html += '<thead><tr>';
    ordered.forEach(m => {
      const thClass = smartviewIsNumericMetaField(m) ? ' class="sv-num-header"' : '';
      html += `<th${thClass}>${escapeHtml(m.fldcap || formatFieldName(m.fldname))}</th>`;
    });
    html += '</tr></thead><tbody>';

    data.forEach(r => {
      html += '<tr>';
      ordered.forEach(m => {
        const fname = (m.fldname || '').toString();
        let v = getRowValueCaseInsensitive(r, fname);
        if ((m.fdatatype === 'd' || m.cdatatype === 'Date') && v) v = formatDateString(v);
        let cell = escapeHtml(v == null ? '' : String(v));
        try {
          const canLinkCell = firstHyperlinkFieldName && String(fname).toLowerCase() === firstHyperlinkFieldName;
          const linkDesc = canLinkCell ? smartviewBuildHyperlinkDescriptor(m, r) : '';
          if (linkDesc && cell) {
            const isInline = m && smartviewHasValue(m.hyp_inline) && smartviewFlagFromValue(m.hyp_inline, false);
            const inlineAttr = isInline ? ' data-hyp-inline="1"' : '';
            cell = `<a href="#" class="sv-hyperlink" data-link="${escapeHtml(linkDesc)}"${inlineAttr}>${cell}</a>`;
          }
        } catch (e) {}
        const tdClass = smartviewIsNumericMetaField(m) ? ' class="sv-num-cell"' : '';
        html += `<td${tdClass}>${cell}</td>`;
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

    
      // const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
      //   : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
      //   : null;
      // if (!caller || typeof caller.GetDataFromAxList !== 'function') {
      //   cb && cb(new Error('GetDataFromAxList not available'), []);
      //   return;
      // }
      const scopes = [parent, window, window.top];

      const caller = scopes.find(
          w => w && typeof w.GetDataFromAxList === 'function'
      );
      
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

  function smartviewGetGroupedViewSourceRows(listJson, ctrl) {
    const searchBox = document.getElementById('searchBox');
    const searchTerm = String(
      (window._entity && window._entity.currentSearchTerm) ||
      (searchBox && searchBox.value) ||
      ''
    ).trim().toLowerCase();

    const filteredSearchRows = (window._entity && Array.isArray(window._entity.filteredListJson))
      ? window._entity.filteredListJson
      : [];
    let rows = [];

    if (searchTerm) {
      rows = filteredSearchRows.length
        ? filteredSearchRows.slice()
        : (Array.isArray(listJson) ? listJson.slice() : []);
    } else if (Array.isArray(window._smartviewFullData) && window._smartviewFullData.length) {
      rows = window._smartviewFullData.slice();
    } else if (filteredSearchRows.length) {
      rows = filteredSearchRows.slice();
    } else if (ctrl && Array.isArray(ctrl._filteredCache) && ctrl._filteredCache.length) {
      rows = ctrl._filteredCache.slice();
    } else {
      rows = Array.isArray(listJson) ? listJson.slice() : [];
    }

    const activeFilters = stripSmartviewFilterTransId((ctrl && Array.isArray(ctrl.filters)) ? ctrl.filters : []);
    if (activeFilters.length) {
      try {
        rows = smartviewApplyClientFilterFallback(rows, activeFilters);
      } catch (e) {}
    }

    return Array.isArray(rows) ? rows : [];
  }

  function smartviewNormalizeSelectedFieldColumns(list) {
    const arr = Array.isArray(list) ? list : [];
    const seen = new Set();
    const out = [];

    arr.forEach(function (item) {
      const raw = String(item || '').trim();
      if (!raw || smartviewIsAggregationExpr(raw)) return;

      const fieldName = smartviewSelectExprToFieldName(raw);
      const token = String(fieldName || '').trim().toLowerCase();
      if (!token || seen.has(token)) return;

      seen.add(token);
      out.push(String(fieldName || '').trim());
    });

    return out;
  }

  function smartviewGetVisibleSelectedFieldColumns(ctrl) {
    if (ctrl && Array.isArray(ctrl._smartviewSelectedFieldColumns) && ctrl._smartviewSelectedFieldColumns.length) {
      return smartviewNormalizeSelectedFieldColumns(ctrl._smartviewSelectedFieldColumns);
    }
    const projected = smartviewNormalizeSelectedFieldColumns((ctrl && Array.isArray(ctrl.select_columns)) ? ctrl.select_columns : []);
    if (projected.length) return projected;
    return smartviewNormalizeSelectedFieldColumns((ctrl && Array.isArray(ctrl.groupby_columns)) ? ctrl.groupby_columns : []);
  }

  function smartviewGetStoredSelectedFieldColumns(ctrl) {
    return smartviewNormalizeSelectedFieldColumns((ctrl && Array.isArray(ctrl._smartviewSelectedFieldColumns)) ? ctrl._smartviewSelectedFieldColumns : []);
  }

  function smartviewGetGroupedSelectedColumns(ctrl) {
    return smartviewGetVisibleSelectedFieldColumns(ctrl);
  }

  function smartviewGetGroupedNodeStore() {
    window._smartviewGroupedNodeStore = window._smartviewGroupedNodeStore || {};
    return window._smartviewGroupedNodeStore;
  }

  function smartviewResetGroupedNodeStore() {
    window._smartviewGroupedNodeStore = {};
    window._smartviewGroupedNodeSeq = 0;
  }

  function smartviewGetGroupedNode(nodeId) {
    const store = smartviewGetGroupedNodeStore();
    return store[String(nodeId || '').trim()] || null;
  }

  function smartviewCreateGroupedNode(node) {
    const store = smartviewGetGroupedNodeStore();
    window._smartviewGroupedNodeSeq = Number(window._smartviewGroupedNodeSeq) || 0;
    window._smartviewGroupedNodeSeq += 1;

    const safeNode = node && typeof node === 'object' ? node : {};
    const out = {
      id: `svgroup_${Date.now()}_${window._smartviewGroupedNodeSeq}`,
      rows: Array.isArray(safeNode.rows) ? safeNode.rows : [],
      meta: Array.isArray(safeNode.meta) ? safeNode.meta : [],
      pathFields: smartviewNormalizeGroupbyFields(Array.isArray(safeNode.pathFields) ? safeNode.pathFields : []),
      pathValues: Array.isArray(safeNode.pathValues) ? safeNode.pathValues.slice() : [],
      groupFields: smartviewNormalizeGroupbyFields(Array.isArray(safeNode.groupFields) ? safeNode.groupFields : []),
      level: Math.max(0, Number(safeNode.level) || 0),
      aggColumns: Array.isArray(safeNode.aggColumns) ? safeNode.aggColumns.slice() : [],
      allGroupFields: smartviewNormalizeGroupbyFields(Array.isArray(safeNode.allGroupFields) ? safeNode.allGroupFields : []),
      selectedColumns: Array.isArray(safeNode.selectedColumns) ? safeNode.selectedColumns.slice() : [],
      rendered: false
    };
    store[out.id] = out;
    return out;
  }

  function smartviewGetMetaForField(metaData, fieldName) {
    const token = String(fieldName || '').trim().toLowerCase();
    if (!token) return null;
    const normalizedToken = smartviewNormalizeLookupKey(token);
    const arr = Array.isArray(metaData) ? metaData : [];
    return arr.find(function (m) {
      if (!m || typeof m !== 'object') return false;
      const direct = String(smartviewGetMetaFieldName(m) || '').trim().toLowerCase();
      if (direct === token) return true;
      const original = String((m._svOriginalFldname || '')).trim().toLowerCase();
      if (original === token) return true;
      const caption = String(m.fldcaption || m.caption || m.fldcap || '').trim().toLowerCase();
      if (caption === token) return true;
      if (normalizedToken) {
        return [
          direct,
          original,
          caption
        ].some(function (candidate) {
          return smartviewNormalizeLookupKey(candidate) === normalizedToken;
        });
      }
      return false;
    }) || null;
  }

  function smartviewBuildGroupedAggregateColumns(metaData, groupFields) {
    const meta = Array.isArray(metaData) ? metaData : [];
    const groupSet = new Set(smartviewNormalizeGroupbyFields(groupFields).map(function (f) {
      return String(f || '').trim().toLowerCase();
    }));
    const internal = new Set(['transid', 'ftransid', 'recordid']);
    const seen = new Set();
    const out = [];

    meta.forEach(function (m) {
      const fld = smartviewGetMetaFieldName(m);
      const key = String(fld || '').trim().toLowerCase();
      if (!key || seen.has(key) || internal.has(key) || groupSet.has(key)) return;
      if (smartviewIsMetaFieldHidden(m)) return;
      if (!smartviewIsNumericMetaField(m)) return;

      seen.add(key);
      const baseCaption = smartviewNormalizeCaption(
        m.fldcaption || m.caption || m.fldcap || '',
        fld
      ) || formatFieldName(fld) || fld;
      const alias = (`sum_${fld}`).replace(/[^a-zA-Z0-9_]/g, '_');
      out.push({
        fieldName: alias,
        fieldLower: alias.toLowerCase(),
        sourceField: fld,
        aggregate: 'sum',
        caption: `Sum ${baseCaption}`,
        meta: {
          fldname: alias,
          fldcap: `Sum ${baseCaption}`,
          fdatatype: 'n',
          cdatatype: 'Numeric'
        },
        isAggregate: true
      });
    });

    return out;
  }

  function smartviewBuildGroupedColumnDescriptors(metaData, groupFields, aggColumns, options) {
    const meta = Array.isArray(metaData) ? metaData : [];
    const groupList = smartviewNormalizeGroupbyFields(groupFields);
    const opts = options && typeof options === 'object' ? options : {};
    const selectedColumns = Array.isArray(opts.selectedColumns)
      ? opts.selectedColumns.map(function (v) { return String(v || '').trim(); }).filter(Boolean)
      : [];
    const currentGroupField = String(opts.currentGroupField || '').trim().toLowerCase();
    const visibleGroupSet = new Set(groupList.map(function (f) {
      return String(f || '').trim().toLowerCase();
    }));
    const out = [];
    const seen = new Set();

    const addGroupDescriptor = function (fieldName) {
      const token = String(fieldName || '').trim().toLowerCase();
      if (!token || seen.has(token)) return;
      const metaItem = smartviewGetMetaForField(meta, fieldName) || {
        fldname: fieldName,
        fldcap: formatFieldName(fieldName),
        fdatatype: 't',
        cdatatype: 'Text'
      };
      const caption = smartviewNormalizeCaption(
        metaItem.fldcaption || metaItem.caption || metaItem.fldcap || '',
        fieldName
      ) || formatFieldName(fieldName) || fieldName;
      out.push({
        kind: 'group',
        fieldName: fieldName,
        fieldLower: token,
        caption: caption,
        meta: metaItem,
        isAggregate: false
      });
      seen.add(token);
    };

    const addAggregateDescriptor = function (fieldName, sourceField, aggregate, caption) {
      const token = String(fieldName || '').trim().toLowerCase();
      if (!token || seen.has(token)) return;
      const baseCaption = smartviewNormalizeCaption(
        caption || '',
        sourceField || fieldName
      ) || formatFieldName(sourceField || fieldName) || fieldName;
      out.push({
        kind: 'aggregate',
        fieldName: String(fieldName || '').trim(),
        fieldLower: token,
        caption: String(caption || (`${String(aggregate || 'sum').trim().toLowerCase() === 'count' ? 'Count' : 'Sum'} ${baseCaption}`)).trim(),
        meta: {
          fldname: fieldName,
          fldcap: String(caption || (`${String(aggregate || 'sum').trim().toLowerCase() === 'count' ? 'Count' : 'Sum'} ${baseCaption}`)).trim(),
          fdatatype: 'n',
          cdatatype: 'Numeric'
        },
        sourceField: String(sourceField || '').trim(),
        aggregate: String(aggregate || 'sum').trim().toLowerCase(),
        isAggregate: true
      });
      seen.add(token);
    };

    groupList.forEach(function (fieldName) {
      addGroupDescriptor(fieldName);
    });

    if (selectedColumns.length) {
      selectedColumns.forEach(function (entry) {
        const raw = String(entry || '').trim();
        if (!raw || !smartviewIsAggregationExpr(raw)) return;
        const aggMatch = raw.match(/^(sum|count|avg|min|max)\s*\(\s*([^)]+)\s*\)\s*(.*)$/i);
        const aggregate = aggMatch && aggMatch[1] ? aggMatch[1].toLowerCase() : 'sum';
        const sourceField = aggMatch && aggMatch[2] ? String(aggMatch[2]).trim() : '';
        const aliasToken = smartviewSelectExprToFieldName(raw);
        const fieldName = aliasToken || (`${aggregate}_${sourceField || raw}`).replace(/[^a-zA-Z0-9_]/g, '_');
        const sourceMeta = smartviewGetMetaForField(meta, sourceField) || smartviewGetMetaForField(meta, fieldName) || null;
        const baseCaption = smartviewNormalizeCaption(
          sourceMeta ? (sourceMeta.fldcaption || sourceMeta.caption || sourceMeta.fldcap || '') : '',
          sourceField || fieldName
        ) || formatFieldName(sourceField || fieldName) || fieldName;
        const aggLabel = aggregate === 'count' ? 'Count' : (aggregate.charAt(0).toUpperCase() + aggregate.slice(1));
        addAggregateDescriptor(fieldName, sourceField || fieldName, aggregate, `${aggLabel} ${baseCaption}`);
      });
    }

    (Array.isArray(aggColumns) ? aggColumns : []).forEach(function (col) {
      if (!col || typeof col !== 'object') return;
      addAggregateDescriptor(
        col.fieldName,
        col.sourceField || col.fieldName,
        col.aggregate || 'sum',
        col.caption || col.fieldName
      );
    });

    return out;
  }

  function smartviewBuildGroupedLeafColumnDescriptors(metaData, rows, options) {
    const meta = Array.isArray(metaData) ? metaData : [];
    const sourceRows = Array.isArray(rows) ? rows : [];
    const opts = options && typeof options === 'object' ? options : {};
    const pathFields = smartviewNormalizeGroupbyFields(Array.isArray(opts.pathFields) ? opts.pathFields : []);
    const allGroupFields = smartviewNormalizeGroupbyFields(
      Array.isArray(opts.allGroupFields) && opts.allGroupFields.length
        ? opts.allGroupFields
        : pathFields.concat(Array.isArray(opts.groupFields) ? opts.groupFields : [])
    );
    const selectedColumns = Array.isArray(opts.selectedColumns)
      ? opts.selectedColumns.map(function (v) { return String(v || '').trim(); }).filter(Boolean)
      : [];
    const aggColumns = Array.isArray(opts.aggColumns) ? opts.aggColumns : [];
    const pathSet = new Set(pathFields.map(function (f) { return String(f || '').trim().toLowerCase(); }));
    const groupSet = new Set(allGroupFields.map(function (f) { return String(f || '').trim().toLowerCase(); }));
    const aggSet = new Set(aggColumns.map(function (c) {
      return String((c && c.fieldName) || '').trim().toLowerCase();
    }).filter(Boolean));
    const internalSet = new Set(['transid', 'ftransid', 'recordid', 'rno', 'axrowoptions', 'axrowoption', 'axrowoptionsjson']);
    const leafMeta = sourceRows.length
      ? smartviewBuildMetaFromDataRows(sourceRows, meta, smartviewGetCurrentAdsForViews())
      : meta;
    const out = [];
    const seen = new Set();

    const addDetailDescriptor = function (fieldName, metaItem) {
      const token = String(fieldName || '').trim().toLowerCase();
      if (!token || seen.has(token)) return;
      if (internalSet.has(token) || pathSet.has(token) || groupSet.has(token) || aggSet.has(token)) return;

      const item = metaItem && typeof metaItem === 'object'
        ? metaItem
        : {
            fldname: fieldName,
            fldcap: formatFieldName(fieldName),
            fdatatype: 't',
            cdatatype: 'Text'
          };
      if (smartviewIsMetaFieldHidden(item)) return;

      const caption = smartviewNormalizeCaption(
        item.fldcaption || item.caption || item.fldcap || '',
        fieldName
      ) || formatFieldName(fieldName) || fieldName;

      out.push({
        kind: 'detail',
        fieldName: fieldName,
        fieldLower: token,
        caption: caption,
        meta: item,
        isAggregate: false
      });
      seen.add(token);
    };

    // If the user explicitly selected detail columns, honor that whitelist.
    selectedColumns.forEach(function (entry) {
      const raw = String(entry || '').trim();
      if (!raw || smartviewIsAggregationExpr(raw)) return;

      const fieldName = smartviewSelectExprToFieldName(raw);
      const token = String(fieldName || raw).trim().toLowerCase();
      if (!token || seen.has(token)) return;

      const metaItem = smartviewGetMetaForField(leafMeta, fieldName) ||
        smartviewGetMetaForField(meta, fieldName) ||
        smartviewGetMetaForField(leafMeta, raw) ||
        smartviewGetMetaForField(meta, raw) ||
        {
          fldname: fieldName || raw,
          fldcap: formatFieldName(fieldName || raw),
          fdatatype: 't',
          cdatatype: 'Text'
        };
      addDetailDescriptor(fieldName || raw, metaItem);
    });

    if (out.length) return out;

    // Fallback: show the actual transaction columns for this leaf level.
    leafMeta.forEach(function (m) {
      addDetailDescriptor(smartviewGetMetaFieldName(m), m);
    });

    return out;
  }

  function smartviewGroupRowsByField(rows, fieldName) {
    const source = Array.isArray(rows) ? rows : [];
    const buckets = [];
    const index = new Map();

    source.forEach(function (row) {
      const rawValue = smartviewGetRowFieldValue(row, fieldName);
      const cleaned = smartviewCleanIncomingValue(rawValue);
      const key = cleaned === '' ? '(Blank)' : cleaned;
      let bucket = index.get(key);
      if (!bucket) {
        bucket = {
          key: key,
          rawValue: rawValue,
          rows: [],
          firstRow: row
        };
        index.set(key, bucket);
        buckets.push(bucket);
      }
      bucket.rows.push(row);
    });

    return buckets;
  }

  function smartviewFormatGroupedTextValue(meta, rawValue, options) {
    const opts = options || {};
    const blankLabel = (opts.blankLabel !== undefined) ? String(opts.blankLabel) : '';
    const clean = smartviewCleanIncomingValue(rawValue);
    if (clean === '') return blankLabel;
    if (smartviewIsDateMetaField(meta)) return formatDateString(clean);
    return clean;
  }

  function smartviewFormatGroupedNumberValue(value) {
    const n = smartviewCoerceNumber(value);
    if (isNaN(n)) return '--';
    try {
      return Number(n).toLocaleString('en-IN', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 2
      });
    } catch (e) {
      return String(n);
    }
  }

  function smartviewRenderGroupedCellHtml(metaData, descriptor, rawValue, rowData, rowsForAgg, options) {
    const col = descriptor || {};
    const meta = col.meta || smartviewGetMetaForField(metaData, col.fieldName) || {};
    const isAggregate = !!col.isAggregate;
    let displayValue = '';

    if (isAggregate) {
      const aggValue = smartviewAggregateRows(Array.isArray(rowsForAgg) ? rowsForAgg : [], col.aggregate || 'sum', col.sourceField || col.fieldName);
      displayValue = smartviewFormatGroupedNumberValue(aggValue);
      return `<span>${escapeHtml(displayValue)}</span>`;
    }

    displayValue = smartviewFormatGroupedTextValue(meta, rawValue, options);
    let html = escapeHtml(displayValue);

    if (options && options.allowHyperlink && rowData && smartviewHasHyperlinkMeta(meta)) {
      const linkDesc = smartviewBuildHyperlinkDescriptor(meta, rowData);
      if (linkDesc) {
        const isInline = meta && smartviewHasValue(meta.hyp_inline) && smartviewFlagFromValue(meta.hyp_inline, false);
        const inlineAttr = isInline ? ' data-hyp-inline="1"' : '';
        html = `<a href="#" class="sv-hyperlink" data-link="${escapeHtml(linkDesc)}"${inlineAttr}>${html}</a>`;
      }
    }

    return html;
  }

  function smartviewBindGroupedTableInteractions(rootEl) {
    if (!rootEl) return;
    const tables = rootEl.matches && rootEl.matches('table')
      ? [rootEl]
      : Array.from(rootEl.querySelectorAll('table'));

    tables.forEach(function (tableEl) {
      try { smartviewApplySavedColumnWidths(tableEl); } catch (e) {}
      try { smartviewBindColumnResizeHandlers(tableEl); } catch (e) {}
      try { smartviewBindColumnReorderHandlers(tableEl); } catch (e) {}
    });
  }

  function smartviewRenderGroupedSectionHtml(node) {
    const safeNode = node && typeof node === 'object' ? node : {};
    const meta = Array.isArray(safeNode.meta) ? safeNode.meta : [];
    const rows = Array.isArray(safeNode.rows) ? safeNode.rows : [];
    const pathFields = smartviewNormalizeGroupbyFields(Array.isArray(safeNode.pathFields) ? safeNode.pathFields : []);
    const pathValues = Array.isArray(safeNode.pathValues) ? safeNode.pathValues.slice() : [];
    const groupFields = smartviewNormalizeGroupbyFields(Array.isArray(safeNode.groupFields) ? safeNode.groupFields : []);
    const selectedColumns = Array.isArray(safeNode.selectedColumns) ? safeNode.selectedColumns.slice() : [];
    const allGroupFields = smartviewNormalizeGroupbyFields(
      Array.isArray(safeNode.allGroupFields) && safeNode.allGroupFields.length
        ? safeNode.allGroupFields
        : pathFields.concat(groupFields)
    );
    const level = Math.max(0, Number(safeNode.level) || 0);
    const currentGroupField = groupFields.length ? groupFields[0] : '';
    const nextPathFields = currentGroupField ? pathFields.concat([currentGroupField]) : pathFields.slice();
    const aggColumns = Array.isArray(safeNode.aggColumns) && safeNode.aggColumns.length
      ? safeNode.aggColumns.slice()
      : smartviewBuildGroupedAggregateColumns(meta, allGroupFields);
    const editConfig = smartviewExtractEditConfigFromMeta(meta);
    const allowInlineEdit = !!editConfig.allowedit && !!editConfig.inlineEditEnabled;
    const editTransIds = smartviewParseNewTstructIds(editConfig.newforms_transid || editConfig.newforms || '');
    const inlineEditTransId = editTransIds.length
      ? editTransIds[0]
      : (editConfig.newforms_transid || editConfig.newforms || '').toString().trim();
    const columnDescriptors = currentGroupField
      ? smartviewBuildGroupedColumnDescriptors(meta, [currentGroupField], aggColumns, {
          selectedColumns: selectedColumns,
          currentGroupField: currentGroupField
        })
      : smartviewBuildGroupedLeafColumnDescriptors(meta, rows, {
          pathFields: pathFields,
          groupFields: groupFields,
          allGroupFields: allGroupFields,
          selectedColumns: selectedColumns,
          aggColumns: aggColumns
        });
    const colspan = Math.max(1, 1 + columnDescriptors.length);

    let html = '<div class="table-responsive"><table class="table table-striped sv-grouped-table">';
    html += '<thead class="sticky-header"><tr>';
    html += '<th class="sv-group-toggle-cell"></th>';
    columnDescriptors.forEach(function (col) {
      const thClass = (col.isAggregate || smartviewIsNumericMetaField(col.meta)) ? 'sv-num-header' : '';
      html += buildSmartviewHeaderCell(col.caption, col.fieldName, thClass);
    });
    html += '</tr></thead><tbody>';

    if (!rows.length) {
      html += `<tr class="sv-no-data"><td colspan="${colspan}" style="text-align:center;padding:24px;color:#666;">No data available</td></tr>`;
      html += '</tbody></table></div>';
      return html;
    }

    if (currentGroupField) {
      const buckets = smartviewGroupRowsByField(rows, currentGroupField);
      if (!buckets.length) {
        html += `<tr class="sv-no-data"><td colspan="${colspan}" style="text-align:center;padding:24px;color:#666;">No data available</td></tr>`;
      }

      buckets.forEach(function (bucket) {
        const childNode = smartviewCreateGroupedNode({
          rows: bucket.rows,
          meta: meta,
          pathFields: nextPathFields,
          pathValues: pathValues.concat([bucket.rawValue]),
          groupFields: groupFields.slice(1),
          level: level + 1,
          aggColumns: aggColumns,
          allGroupFields: allGroupFields,
          selectedColumns: selectedColumns
        });

        html += `<tr class="sv-group-row sv-group-level-${level}" data-sv-group-node-id="${escapeHtml(childNode.id)}">`;
        html += `<td class="sv-group-toggle-cell"><button type="button" class="sv-expand-btn" data-sv-group-node-id="${escapeHtml(childNode.id)}" data-expanded="false">+</button></td>`;

        columnDescriptors.forEach(function (col) {
          const tdClass = (col.isAggregate || smartviewIsNumericMetaField(col.meta)) ? ' class="sv-num-cell"' : '';
          let cellHtml = '';
          if (col.isAggregate) {
            cellHtml = smartviewRenderGroupedCellHtml(meta, col, null, bucket.firstRow, bucket.rows, { allowHyperlink: false, blankLabel: '' });
          } else {
            cellHtml = smartviewRenderGroupedCellHtml(meta, col, bucket.rawValue, bucket.firstRow, bucket.rows, { allowHyperlink: false, blankLabel: '(Blank)' });
          }
          html += `<td${tdClass}>${cellHtml}</td>`;
        });

        html += '</tr>';
        html += `<tr class="sv-group-detail-row" style="display:none;"><td colspan="${colspan}"><div class="sv-group-detail" id="${escapeHtml(childNode.id)}" data-sv-group-node-id="${escapeHtml(childNode.id)}" data-sv-loaded="false"></div></td></tr>`;
      });
    } else {
      rows.forEach(function (row) {
        let rowLinkDesc = '';
        if (allowInlineEdit) {
          for (let i = 0; i < meta.length; i++) {
            rowLinkDesc = smartviewBuildHyperlinkDescriptor(meta[i], row);
            if (rowLinkDesc) break;
          }
        }
        const fallbackEditLink = allowInlineEdit ? smartviewBuildFallbackEditLinkDescriptor(row, inlineEditTransId) : '';
        const inlineEditLink = allowInlineEdit ? (rowLinkDesc || fallbackEditLink) : '';
        html += `<tr class="sv-group-row sv-group-level-${level}">`;
        html += '<td>';
        if (inlineEditLink) {
          html += `<span class="row-arrow material-icons sv-hyperlinktemp" data-link="${escapeHtml(inlineEditLink)}">edit_note</span>`;
        }
        html += '</td>';
        columnDescriptors.forEach(function (col) {
          const tdClass = (col.isAggregate || smartviewIsNumericMetaField(col.meta)) ? ' class="sv-num-cell"' : '';
          let cellHtml = '';
          if (col.isAggregate) {
            cellHtml = smartviewRenderGroupedCellHtml(meta, col, null, row, [row], { allowHyperlink: false, blankLabel: '' });
          } else {
            const rawValue = smartviewGetRowFieldValue(row, col.fieldName);
            cellHtml = smartviewRenderGroupedCellHtml(meta, col, rawValue, row, [row], { allowHyperlink: true, blankLabel: '' });
          }
          html += `<td${tdClass}>${cellHtml}</td>`;
        });
        html += '</tr>';
      });
    }

    html += '</tbody></table></div>';
    return html;
  }

  function smartviewRenderGroupedViewHTML(rows, meta, ctrl) {
    const groupFields = smartviewNormalizeGroupbyFields((ctrl && ctrl.groupby_columns) || []);
    const selectedColumns = smartviewGetGroupedSelectedColumns(ctrl);
    const rootNode = {
      rows: Array.isArray(rows) ? rows : [],
      meta: Array.isArray(meta) ? meta : [],
      pathFields: [],
      pathValues: [],
      groupFields: groupFields.slice(),
      level: 0,
      aggColumns: smartviewBuildGroupedAggregateColumns(meta || [], groupFields),
      allGroupFields: groupFields.slice(),
      selectedColumns: selectedColumns
    };
    return smartviewRenderGroupedSectionHtml(rootNode);
  }

  function smartviewExpandGroupedNodeById(nodeId) {
    const token = String(nodeId || '').trim();
    if (!token) return false;

    const host = document.getElementById(token);
    if (!host) return false;

    const loaded = String(host.getAttribute('data-sv-loaded') || '').toLowerCase() === 'true';
    if (loaded) return true;

    const node = smartviewGetGroupedNode(token);
    if (!node) return false;

    host.innerHTML = smartviewRenderGroupedSectionHtml(node);
    host.setAttribute('data-sv-loaded', 'true');
    smartviewBindGroupedTableInteractions(host);
    return true;
  }

  /* --------------------------
    SmartView client-side filter helpers
    -------------------------- */

  function getRowValueCaseInsensitive(row, fieldName) {
    if (!row || !fieldName) return '';
    const direct = row[fieldName];
    if (direct !== undefined) return direct;
    const token = String(fieldName).toLowerCase();
    const normalizedToken = smartviewNormalizeLookupKey(fieldName);
    const key = Object.keys(row).find(function (k) {
      if (String(k || '').toLowerCase() === token) return true;
      if (!normalizedToken) return false;
      return smartviewNormalizeLookupKey(k) === normalizedToken;
    });
    return key ? row[key] : '';
  }

  function smartviewApplyClientSortingFallback(rows, sorting, metaData) {
    const source = Array.isArray(rows) ? rows.slice() : [];
    const sortList = Array.isArray(sorting) ? sorting : [];
    const sortItem = sortList.find(function (item) {
      return item && String(item.fldname || item.field || item.name || '').trim();
    });
    if (!sortItem) return source;

    const fieldName = String(sortItem.fldname || sortItem.field || sortItem.name || '').trim();
    const direction = String(sortItem.sort_order || sortItem.order || sortItem.direction || 'asc').toLowerCase() === 'desc' ? -1 : 1;
    const meta = (typeof smartviewGetMetaForField === 'function') ? smartviewGetMetaForField(metaData || [], fieldName) : null;
    const isDate = meta ? smartviewIsDateMetaField(meta) : /date|on$/i.test(fieldName);

    return source
      .map(function (row, index) { return { row: row, index: index }; })
      .sort(function (a, b) {
        const avRaw = smartviewGetRowFieldValue(a.row, fieldName);
        const bvRaw = smartviewGetRowFieldValue(b.row, fieldName);
        const avBlank = avRaw === null || avRaw === undefined || String(avRaw).trim() === '';
        const bvBlank = bvRaw === null || bvRaw === undefined || String(bvRaw).trim() === '';
        if (avBlank && bvBlank) return a.index - b.index;
        if (avBlank) return 1;
        if (bvBlank) return -1;

        if (isDate) {
          const ad = parseDateLoose(avRaw);
          const bd = parseDateLoose(bvRaw);
          if (ad && bd) {
            const diff = ad.getTime() - bd.getTime();
            return diff === 0 ? (a.index - b.index) : (diff * direction);
          }
        }

        const an = smartviewCoerceNumber(avRaw);
        const bn = smartviewCoerceNumber(bvRaw);
        if (!isNaN(an) && !isNaN(bn)) {
          const diff = an - bn;
          return diff === 0 ? (a.index - b.index) : (diff * direction);
        }

        const at = String(avRaw).trim().toLowerCase();
        const bt = String(bvRaw).trim().toLowerCase();
        const cmp = at.localeCompare(bt, undefined, { numeric: true, sensitivity: 'base' });
        return cmp === 0 ? (a.index - b.index) : (cmp * direction);
      })
      .map(function (entry) { return entry.row; });
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

      // Ensure outgoing DATE/TIMESTAMP filters are in DD/MM/YYYY (or DD/MM/YYYY HH:mm:ss).
      const dt = (o.datatype || o.fdatatype || '').toString().trim().toUpperCase();
      if (dt === 'DATE' || dt === 'TIMESTAMP') {
        const toDdMmYyyy = function (v, includeTime) {
          if (v === null || v === undefined) return '';
          const raw = (typeof smartviewCleanIncomingValue === 'function')
            ? smartviewCleanIncomingValue(v)
            : String(v).trim();
          if (!raw) return '';
          try {
            const formats = [
              'DD/MM/YYYY',
              'DD/MM/YYYY HH:mm:ss',
              'DD/MM/YYYY HH:mm',
              'YYYY-MM-DD',
              'YYYY-MM-DD HH:mm:ss',
              'YYYY-MM-DDTHH:mm:ss',
              'YYYY-MM-DDTHH:mm:ss.SSS',
              'MM/DD/YYYY',
              'DD-MMM-YYYY'
            ];
            if (typeof advFilterDtCulture !== 'undefined' && advFilterDtCulture) formats.push(advFilterDtCulture);
            let m = moment(raw, formats, true);
            if (!m.isValid()) m = moment(raw);
            if (!m.isValid()) return raw;
            const hasTimePart = /(?:\s|T)\d{1,2}:\d{2}/.test(raw);
            if (includeTime && hasTimePart) return m.format('DD/MM/YYYY HH:mm:ss');
            return m.format('DD/MM/YYYY');
          } catch (e) {
            return raw;
          }
        };

        const includeTime = (dt === 'TIMESTAMP');
        if (o.from !== undefined) o.from = toDdMmYyyy(o.from, includeTime);
        if (o.to !== undefined) o.to = toDdMmYyyy(o.to, includeTime);
        if (o.value !== undefined && !Array.isArray(o.value)) o.value = toDdMmYyyy(o.value, includeTime);
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

  const SMARTVIEW_ADS_META_STORAGE_VERSION = 6;

  function smartviewNormalizeFieldToken(value) {
    return String(value || '').trim().toLowerCase().replace(/[\s_\-]/g, '');
  }

  function smartviewIsDescriptorLikeFieldToken(fieldName) {
    const raw = String(fieldName || '').trim().toLowerCase();
    const compact = smartviewNormalizeFieldToken(raw);
    if (!raw) return false;

    // Heuristic only: system metadata rows tend to use technical prefixes and compact keys,
    // while business columns usually read like docid, party_name, itemdesc_shownas, etc.
    const technicalPrefix = /^(sql|fld|src|hyp|tbl|dyn|pag|cache|enc|norm|sort)/.test(compact);
    const technicalShape = compact.length <= 4 && /^[a-z]+$/.test(compact) && !/^(id|uid|code|name)$/i.test(compact);
    return technicalPrefix || technicalShape;
  }

  function smartviewGetMetaFieldName(item) {
    if (!item || typeof item !== 'object') return '';
    return String(
      smartviewGetObjectValueCI(item, ['fldname', 'fieldname', 'name', 'key', 'column', 'colname']) ||
      item.fldname ||
      item.fieldname ||
      item.name ||
      item.key ||
      ''
    ).trim();
  }

  function smartviewLooksLikeAdsMetadataDescriptorMeta(metaData) {
    const arr = Array.isArray(metaData) ? metaData : [];
    const fieldNames = arr.map(smartviewGetMetaFieldName).filter(Boolean);
    if (!fieldNames.length) return false;

    // Descriptor detection should look at the actual field values, not the object
    // property names. Real ADS metadata rows still use keys like `fldname`/`fldcap`,
    // but their values are business fields such as `docid`, `party_name`, etc.
    const normalized = fieldNames.map(smartviewNormalizeFieldToken).filter(Boolean);
    const hitCount = normalized.filter(smartviewIsDescriptorLikeFieldToken).length;
    const familyMatches = ['sql', 'fld', 'src', 'hyp', 'tbl', 'dyn', 'pag', 'cache', 'enc', 'norm', 'sort']
      .filter(prefix => normalized.some(token => token.startsWith(prefix)))
      .length;
    const score = hitCount / Math.max(normalized.length, 1);

    return (normalized.length >= 4 && score >= 0.45 && familyMatches >= 2) ||
      (normalized.length >= 6 && familyMatches >= 3 && hitCount >= 4);
  }

  function smartviewRowsContainValueForField(rows, fieldName) {
    const arr = Array.isArray(rows) ? rows : [];
    return arr.some(function (row) {
      if (!row || typeof row !== 'object') return false;
      const value = getRowValueCaseInsensitive(row, fieldName);
      return smartviewHasValue(value);
    });
  }

  function smartviewInferMetaTypeFromRows(fieldName, rows, columnDef) {
    const fieldToken = smartviewNormalizeFieldToken(fieldName);
    const def = (columnDef && typeof columnDef === 'object') ? columnDef : {};
    const rawType = smartviewNormalizeFieldToken(def.fdatatype || def.datatype || def.cdatatype || '');
    const dateHint = fieldToken.includes('date') || fieldToken.endsWith('on') || rawType === 'd' || rawType.includes('date');
    const numberHint = rawType === 'n' || rawType.includes('num') || rawType.includes('decimal') ||
      rawType.includes('currency') || fieldToken.includes('amount') || fieldToken.includes('total') ||
      fieldToken.includes('tax') || fieldToken.includes('qty') || fieldToken.includes('rate');

    if (dateHint) return { fdatatype: 'd', cdatatype: 'Date' };
    if (numberHint) return { fdatatype: 'n', cdatatype: 'Numeric' };

    const sourceRows = Array.isArray(rows) ? rows : [];
    for (let i = 0; i < sourceRows.length; i++) {
      const value = getRowValueCaseInsensitive(sourceRows[i], fieldName);
      if (!smartviewHasValue(value)) continue;
      if (typeof value === 'number' && Number.isFinite(value)) return { fdatatype: 'n', cdatatype: 'Numeric' };

      const s = String(value).trim();
      if (/^[+-]?\d[\d,]*(\.\d+)?$/.test(s)) return { fdatatype: 'n', cdatatype: 'Numeric' };
      if (/^\d{4}-\d{1,2}-\d{1,2}(?:[T\s].*)?$/i.test(s) || /^\d{1,2}[\/\-]\d{1,2}[\/\-]\d{4}(?:\s.*)?$/.test(s)) {
        return { fdatatype: 'd', cdatatype: 'Date' };
      }
      break;
    }

    return { fdatatype: 'c', cdatatype: 'Text' };
  }

  function smartviewHasSourceDropdownHints(item) {
    if (!item || typeof item !== 'object') return false;

    const srcTable = String(
      smartviewGetObjectValueCI(item, ['srctable', 'sourcetable', 'src_table', 'source_table', 'srctbl', 'table', 'tablename', 'tblname'])
      || item.srctable
      || item.sourcetable
      || item.src_table
      || ''
    ).trim();
    const srcFld = String(
      smartviewGetObjectValueCI(item, ['srcfld', 'sourcefld', 'src_fld', 'source_fld', 'srcfield', 'sourcefield', 'column', 'colname', 'columnname'])
      || item.srcfld
      || item.sourcefld
      || ''
    ).trim();
    const psrctxt = String(
      smartviewGetObjectValueCI(item, ['psrctxt', 'psrctext', 'psrcTxt'])
      || item.psrctxt
      || ''
    ).trim();

    return !!(psrctxt || (srcTable && srcFld));
  }

  function smartviewBuildMetaFromDataRows(rows, existingMeta, adsName) {
    const sourceRows = Array.isArray(rows) ? rows.filter(r => r && typeof r === 'object') : [];
    if (!sourceRows.length) return [];

    const existing = Array.isArray(existingMeta) && !smartviewLooksLikeAdsMetadataDescriptorMeta(existingMeta)
      ? existingMeta
      : [];
    const existingByField = {};
    existing.forEach(function (m) {
      const fieldName = smartviewGetMetaFieldName(m);
      if (fieldName) existingByField[fieldName.toLowerCase()] = m;
    });

    const skipAlways = new Set(['transid', 'ftransid', 'recordid', 'rno', 'axrowoptions', 'axrowoption', 'axrowoptionsjson']);
    const seen = new Set();
    const out = [];

    sourceRows.forEach(function (row) {
      Object.keys(row || {}).forEach(function (key) {
        const fieldName = String(key || '').trim();
        const lower = fieldName.toLowerCase();
        if (!fieldName || seen.has(lower) || skipAlways.has(lower)) return;

        const hasData = smartviewRowsContainValueForField(sourceRows, fieldName);
        if (!hasData && smartviewIsDescriptorLikeFieldToken(fieldName)) return;
        if (!hasData && !existingByField[lower]) return;

        seen.add(lower);
        const existingRow = existingByField[lower] || {};
        const inferredType = smartviewInferMetaTypeFromRows(fieldName, sourceRows, existingRow);
        const caption = smartviewNormalizeCaption(
          existingRow.fldcaption || existingRow.caption || existingRow.fldcap || '',
          fieldName
        ) || formatFieldName(fieldName) || fieldName;
        const sourceDropdown = smartviewHasSourceDropdownHints(existingRow) || smartviewFlagFromValue(existingRow.normalized, false);
        const resolvedCdatatype = sourceDropdown
          ? 'DropDown'
          : (existingRow.cdatatype || inferredType.cdatatype);

        out.push(smartviewCreatePublicMetaRow(existingRow, {
          fldname: fieldName,
          fldcap: caption,
          fldcaption: caption,
          fdatatype: sourceDropdown ? 'c' : (existingRow.fdatatype || inferredType.fdatatype),
          cdatatype: resolvedCdatatype,
          listingfld: 'T',
          filters: 'T',
          normalized: existingRow.normalized || 'F',
          ftransid: existingRow.ftransid || adsName || (window._entity && window._entity.entityTransId) || '',
          allowedit: smartviewHasValue(existingRow.allowedit) ? existingRow.allowedit : 'F',
          allownew: smartviewHasValue(existingRow.allownew) ? existingRow.allownew : 'T',
          bulksave: smartviewHasValue(existingRow.bulksave) ? existingRow.bulksave : 'T',
          dataupload: smartviewHasValue(existingRow.dataupload) ? existingRow.dataupload : 'T',
          validatedata: smartviewHasValue(existingRow.validatedata) ? existingRow.validatedata : 'F',
          editmode: smartviewHasValue(existingRow.editmode) ? existingRow.editmode : 'Inline',
          savemode: smartviewHasValue(existingRow.savemode) ? existingRow.savemode : 'Default',
          newforms: smartviewHasValue(existingRow.newforms) ? existingRow.newforms : '',
          newforms_transid: smartviewHasValue(existingRow.newforms_transid) ? existingRow.newforms_transid : '',
          sv_name: smartviewHasValue(existingRow.sv_name) ? existingRow.sv_name : '',
          sv_caption: smartviewHasValue(existingRow.sv_caption) ? existingRow.sv_caption : '',
          sv_sourcecnd: smartviewHasValue(existingRow.sv_sourcecnd) ? existingRow.sv_sourcecnd : '1',
          sv_keycol: smartviewHasValue(existingRow.sv_keycol) ? existingRow.sv_keycol : '',
          api_config: smartviewHasValue(existingRow.api_config) ? existingRow.api_config : null,
          memdb_key: smartviewHasValue(existingRow.memdb_key) ? existingRow.memdb_key : ''
        }));
      });
    });

    return out;
  }

  function smartviewBuildMetaFallbackFromCurrentRows(existingMeta, adsName) {
    const rows = (Array.isArray(window._smartviewFullData) && window._smartviewFullData.length)
      ? window._smartviewFullData
      : ((window._entity && Array.isArray(window._entity.listJson)) ? window._entity.listJson : []);
    return smartviewBuildMetaFromDataRows(rows, existingMeta, adsName || smartviewGetCurrentAdsForViews());
  }

  function smartviewIsMetaRowObject(value) {
    return !!(value && typeof value === 'object' && !Array.isArray(value));
  }

  function smartviewDefineMetaProp(target, key, value, enumerable) {
    if (!smartviewIsMetaRowObject(target) || key === undefined || key === null) return target;
    try {
      Object.defineProperty(target, String(key), {
        value: value,
        enumerable: !!enumerable,
        configurable: true,
        writable: true
      });
    } catch (e) {
      target[String(key)] = value;
    }
    return target;
  }

  function smartviewCloneMetaRow(metaRow, overrides) {
    const out = {};
    if (smartviewIsMetaRowObject(metaRow)) {
      Object.getOwnPropertyNames(metaRow).forEach(function (key) {
        if (key === '__proto__') return;
        const desc = Object.getOwnPropertyDescriptor(metaRow, key);
        if (!desc) return;
        try { Object.defineProperty(out, key, desc); } catch (e) {}
      });
    }
    if (smartviewIsMetaRowObject(overrides)) {
      Object.keys(overrides).forEach(function (key) {
        out[key] = overrides[key];
      });
    }
    return out;
  }

  function smartviewCreatePublicMetaRow(rawRow, normalizedRow) {
    const raw = smartviewIsMetaRowObject(rawRow) ? rawRow : {};
    const normalized = smartviewIsMetaRowObject(normalizedRow) ? normalizedRow : {};
    const merged = Object.assign({}, raw, normalized);
    const publicRow = {};

    const fldnameRaw = smartviewGetObjectValueCI(normalized, ['fldname', 'fieldname', 'name', 'key'])
      || smartviewGetObjectValueCI(raw, ['fldname', 'fieldname', 'name', 'key']);
    const fldname = String(fldnameRaw || '').trim();
    if (fldname) smartviewDefineMetaProp(publicRow, 'fldname', fldname, true);

    const fldcapRaw = smartviewGetObjectValueCI(normalized, ['fldcaption', 'caption', 'fldcap', 'title', 'displayname', 'fieldcaption'])
      || smartviewGetObjectValueCI(raw, ['fldcaption', 'caption', 'fldcap', 'title', 'displayname', 'fieldcaption']);
    const fldcap = String(fldcapRaw || '').trim() || (fldname ? formatFieldName(fldname) : '');
    if (fldcap) smartviewDefineMetaProp(publicRow, 'fldcap', fldcap, true);

    if (fldcap) smartviewDefineMetaProp(publicRow, 'fldcaption', fldcap, false);

    Object.keys(merged).forEach(function (key) {
      const token = String(key || '').trim();
      if (!token || token === 'fldname' || token === 'fldcap' || token === 'fldcaption') return;
      smartviewDefineMetaProp(publicRow, token, merged[key], false);
    });

    return publicRow;
  }

  function smartviewAdsMetaStorageKey(adsName) {
    const n = smartviewNormalizeAdsName(adsName);
    return n ? `smartview_adsmeta::${n}` : `smartview_adsmeta::`;
  }

  function smartviewMergeAdsMetaRow(rawRow, normalizedRow) {
    return smartviewCreatePublicMetaRow(rawRow, normalizedRow);
  }

  function saveSmartviewAdsMetaToStorage(adsName, meta, options) {
    try {
      if (!adsName || !Array.isArray(meta)) return;
      if (typeof localStorage === 'undefined') return;
      const key = smartviewAdsMetaStorageKey(adsName);
      const extras = (options && typeof options === 'object' && !Array.isArray(options)) ? options : {};
      const payload = {
        ver: SMARTVIEW_ADS_META_STORAGE_VERSION,
        ads: String(adsName),
        ts: Date.now(),
        meta: smartviewCloneJsonSafe(meta) || [],
        meta_raw: Array.isArray(extras.metaRaw) ? (smartviewCloneJsonSafe(extras.metaRaw) || []) : [],
        source: (extras.source && typeof extras.source === 'object' && !Array.isArray(extras.source))
          ? (smartviewCloneJsonSafe(extras.source) || {})
          : {}
      };
      localStorage.setItem(key, JSON.stringify(payload));
    } catch (e) {
      // ignore (storage may be disabled/quota exceeded)
    }
  }

  function loadSmartviewAdsMetaPayloadFromStorage(adsName) {
    try {
      if (!adsName) return null;
      if (typeof localStorage === 'undefined') return null;
      const key = smartviewAdsMetaStorageKey(adsName);
      const raw = localStorage.getItem(key);
      if (!raw) return null;
      return JSON.parse(raw);
    } catch (e) {
      return null;
    }
  }

  function loadSmartviewAdsMetaFromStorage(adsName) {
    try {
      const parsed = loadSmartviewAdsMetaPayloadFromStorage(adsName);
      if (!parsed) return null;
      // Old cache shapes are intentionally ignored so SmartView refetches and rewrites
      // the metadata entry with the richer payload structure.
      if (Array.isArray(parsed)) return null;
      if (!parsed || !Array.isArray(parsed.meta)) return null;
      if (Number(parsed.ver || 0) < SMARTVIEW_ADS_META_STORAGE_VERSION) return null;
      const rawRows = Array.isArray(parsed.meta_raw) ? parsed.meta_raw : [];
      const meta = parsed.meta.map(function (row, idx) {
        return smartviewCreatePublicMetaRow(rawRows[idx] || {}, row || {});
      });
      if (smartviewLooksLikeAdsMetadataDescriptorMeta(meta)) return null;
      return meta;
    } catch (e) {
      return null;
    }
  }

  function smartviewLoadSmartviewAdsMetaRawFromStorage(adsName) {
    try {
      const parsed = loadSmartviewAdsMetaPayloadFromStorage(adsName);
      if (!parsed || !Array.isArray(parsed.meta_raw) || !parsed.meta_raw.length) return [];
      return smartviewCloneJsonSafe(parsed.meta_raw) || [];
    } catch (e) {
      return [];
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

  function smartviewHasHyperlinkMeta(fieldMeta) {
    if (!fieldMeta) return false;
    const hypStructType = (fieldMeta.hyp_structtype ?? fieldMeta.hypStructType ?? fieldMeta.hyp_struct_type ?? '') || '';
    const hypTransId = (fieldMeta.hyp_transid ?? fieldMeta.hypTransId ?? fieldMeta.hyp_transId ?? '') || '';
    return !!String(hypStructType).trim() && !!String(hypTransId).trim();
  }

  function smartviewNormalizeTypedTransId(rawValue, typePrefix) {
    const value = String(rawValue || '').trim();
    const prefix = String(typePrefix || '').trim().toLowerCase();
    if (!value || !prefix) return value;

    // Backend stores typed ids as `<type><transid>`:
    // example: `ttslpo` => type `t` + transid `tslpo`.
    if (value[0].toLowerCase() === prefix) {
      return value.slice(1);
    }
    return value;
  }

  function smartviewBuildHyperlinkDescriptor(fieldMeta, rowData) {
    if (!fieldMeta || !rowData) return '';

    const hypStructType = (fieldMeta.hyp_structtype ?? fieldMeta.hypStructType ?? fieldMeta.hyp_struct_type ?? '') || '';
    const hypTransId = (fieldMeta.hyp_transid ?? fieldMeta.hypTransId ?? fieldMeta.hyp_transId ?? '') || '';
    const tblHyperlink = (fieldMeta.tbl_hyperlink ?? fieldMeta.tblHyperlink ?? fieldMeta.tbl_hyperLink ?? '') || '';

    const st = String(hypStructType || '').trim().toLowerCase();
    const transRaw = String(hypTransId || '').trim();
    const tbl = String(tblHyperlink || '').trim();
    
    // Check if this should use load mode (when hyp_structtype contains "load")
    const isLoadMode = st.includes('load');
    
    // Debug logging
    if (!st || !transRaw) {
      console.debug('[smartviewTable] smartviewBuildHyperlinkDescriptor - Missing metadata:', {
        fieldName: fieldMeta.fldname,
        hasStructType: !!st,
        hasTransId: !!transRaw,
        hypStructType,
        hypTransId
      });
      return '';
    }

    let prefix = '';
    if (st.includes('tstruct') || st.includes('t struct')) prefix = 't';
    else if (st.includes('iview') || st.includes('i view')) prefix = 'i';
    else if (st.includes('html') || st.includes('page')) prefix = 'h';
    else if (st.includes('entityform') || st.includes('entity form')) prefix = 'd';
    else if (st.includes('entity')) prefix = 'l';
    if (!prefix) {
      console.warn('[smartviewTable] Could not determine hyperlink prefix from structtype:', st);
      return '';
    }

    // Backend sends typed ids as `<type><transid>`, e.g. `ttslpo` => `t` + `tslpo`.
    const trans = smartviewNormalizeTypedTransId(transRaw, prefix);
    if (!trans) {
      console.warn('[smartviewTable] No transid remaining after normalization:', transRaw);
      return '';
    }

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

    // If no explicit tbl_hyperlink mapping is available for tstruct, fall back to row record id.
    // This helps open the actual document instead of a blank/new form.
    if (!params.length && prefix === 't') {
      const rec =
        smartviewGetRowValueForHyperlink(rowData, 'recordid') ||
        smartviewGetRowValueForHyperlink(rowData, 'docid') ||
        smartviewGetRowValueForHyperlink(rowData, 'axp_recid') ||
        '';
      const recStr = String(rec || '').trim();
      if (recStr) {
        const safeRec = recStr.replace(/[\^~()]/g, ' ');
        params.push(`recordid=${safeRec}`);
        params.push(`docid=${safeRec}`);
      }
    }

    // Add load mode indicator if needed (for tstruct)
    if (isLoadMode && prefix === 't') {
      params.push('_mode=load');
    }

    const paramStr = params.join('^');
    const descriptor = paramStr ? `${prefix}${trans}(${paramStr})` : `${prefix}${trans}`;
    console.debug('[smartviewTable] Built hyperlink descriptor:', descriptor, {
      fieldName: fieldMeta.fldname,
      isLoadMode,
      hypStructType
    });
    return descriptor;
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

  function smartviewApplyClientFilterFallback(rows, filters) {
    const sourceRows = Array.isArray(rows) ? rows : [];
    const safeFilters = stripSmartviewFilterTransId(Array.isArray(filters) ? filters : []);
    if (!sourceRows.length || !safeFilters.length) return sourceRows;

    try {
      return applySmartviewFiltersToRows(sourceRows, safeFilters);
    } catch (e) {
      console.warn('smartviewApplyClientFilterFallback failed', e);
      return sourceRows;
    }
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
   * liveSearch - matches Entity.js behavior:
   * filter JSON first, then re-render the table from matching rows.
   */
  function liveSearch() {
    try {
      const searchInput = document.getElementById('searchBox');
      if (!searchInput) return;

      const searchTerm = (searchInput.value || '').toString().trim().toLowerCase();
      const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;

      window._entity = window._entity || {};
      window._entity.currentSearchTerm = searchTerm;

      const sourceJson = (searchTerm !== '' && ctrl && ctrl.streamingAllRecords && Array.isArray(window._smartviewFullData) && window._smartviewFullData.length > 0)
        ? window._smartviewFullData
        : (Array.isArray(window._entity.listJson) ? window._entity.listJson : []);

      window._entity.filteredListJson = searchTerm === ''
        ? sourceJson.slice()
        : sourceJson.filter((rowData) =>
            Object.values(rowData || {}).some(value =>
              String(value ?? '').toLowerCase().includes(searchTerm)
            )
          );

      if (sourceJson.length === 0) {
        if (typeof createTableViewHTML === 'function') createTableViewHTML([], 1);
        if (rowCountManager && typeof rowCountManager.refresh === 'function') rowCountManager.refresh();
        return;
      }

      if (window._entity.filteredListJson.length === 0) {
        if (typeof createTableViewHTML === 'function') createTableViewHTML(sourceJson, 1);

        document.querySelectorAll('#table-body_Container tbody tr').forEach((row) => {
          row.classList.add('d-none');
        });

        if (searchTerm !== '') {
          if (typeof showAlertDialog === 'function') showAlertDialog('warning', 'No matching data found');
          else console.warn('No matching data found');
        }
        if (rowCountManager && typeof rowCountManager.refresh === 'function') rowCountManager.refresh();
        return;
      }

      if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.filteredListJson, 1);
      if (rowCountManager && typeof rowCountManager.refresh === 'function') rowCountManager.refresh();
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

  function smartviewNormalizeLookupKey(v) {
    return String(v === null || v === undefined ? '' : v)
      .trim()
      .toLowerCase()
      .replace(/[\s_\-]+/g, '')
      .replace(/[^a-z0-9]/g, '');
  }

  function smartviewResolveFilterField(rawField, metaData) {
    const rf = (rawField === null || rawField === undefined) ? '' : String(rawField).trim();
    if (!rf) return { fldname: '', meta: null };
    const rfl = rf.toLowerCase();
    const arr = Array.isArray(metaData) ? metaData : [];

    let m = arr.find(x => ((x.fldname || '').toString().trim().toLowerCase() === rfl)) || null;
    if (!m) m = arr.find(x => ((x._svOriginalFldname || '').toString().trim().toLowerCase() === rfl)) || null;
    if (!m) m = arr.find(x => ((x.fldcap || x.fldcaption || x.caption || '').toString().trim().toLowerCase() === rfl)) || null;
    if (!m) {
      const normalizedRf = smartviewNormalizeLookupKey(rf);
      if (normalizedRf) {
        m = arr.find(x => {
          const candidates = [
            x && x.fldname,
            x && x._svOriginalFldname,
            x && x.fldcap,
            x && x.fldcaption,
            x && x.caption
          ];
          return candidates.some(val => smartviewNormalizeLookupKey(val) === normalizedRf);
        }) || null;
      }
    }

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

  function smartviewGetEffectiveGroupbyColumns(ctrl) {
    if (!ctrl || typeof ctrl !== 'object') return [];
    return smartviewNormalizeGroupbyFields(Array.isArray(ctrl.groupby_columns) ? ctrl.groupby_columns : []);
  }

  function smartviewApplyGroupbySelection(ctrl, metaData, selectedGroupbyCols) {
    if (!ctrl || typeof ctrl !== 'object') return { groupby_columns: [], select_columns: [] };
    const normalized = smartviewNormalizeGroupbyFields(selectedGroupbyCols);
    if (!normalized.length) {
      ctrl.groupby_columns = [];
      ctrl.select_columns = smartviewGetStoredSelectedFieldColumns(ctrl);
      ctrl.aggregations = {};
      return { groupby_columns: [], select_columns: [] };
    }

    const gb = smartviewBuildGroupbyWithSums(metaData || [], normalized);
    ctrl.groupby_columns = Array.isArray(gb.groupby_columns) ? gb.groupby_columns : [];
    ctrl.select_columns = Array.isArray(gb.select_columns) ? gb.select_columns : [];
    ctrl.aggregations = {};
    return {
      groupby_columns: ctrl.groupby_columns.slice(),
      select_columns: ctrl.select_columns.slice()
    };
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
    if (metaCd === 'dropdown' || metaCd === 'drop down' || metaCd === 'drop_down' || smartviewHasSourceDropdownHints(meta) || smartviewFlagFromValue(meta && meta.normalized, false)) return 'DROPDOWN';

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

  function smartviewGetEntityFilterArray(entityFilterInstance) {
    const inst = entityFilterInstance || window._entityFilter || null;
    if (!inst) return [];

    if (Array.isArray(inst.activeFilterArray) && inst.activeFilterArray.length) {
      return inst.activeFilterArray;
    }

    const filterObj = (inst.filterObj && typeof inst.filterObj === 'object') ? inst.filterObj : {};
    const activeId = String(inst.activeFilterId || '').trim();
    if (activeId && filterObj[activeId] && Array.isArray(filterObj[activeId].filter)) {
      return filterObj[activeId].filter;
    }

    const keys = Object.keys(filterObj);
    for (let i = keys.length - 1; i >= 0; i--) {
      const item = filterObj[keys[i]];
      if (item && Array.isArray(item.filter) && item.filter.length) return item.filter;
    }

    return [];
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
      const EntityFilterCtor = smartviewResolveEntityFilterCtor();
      if (typeof EntityFilterCtor !== 'function') return false;
      window._entityFilter = window._entityFilter || new EntityFilterCtor();

      if (window._entityFilter._smartviewPatchedForPills) return true;

      // Minimal patch: ensure pills apply filters to SmartView controller (without requiring openFilters() first).
      window._entityFilter.applyFilters = function () {
        try {
          window._smartviewFilterApplySeq = (Number(window._smartviewFilterApplySeq) || 0) + 1;
          const filters = stripSmartviewFilterTransId(smartviewGetEntityFilterArray(this));
          try { smartviewPersistActiveSavedFilterAsView(this, filters); } catch (e) {}
          try { refreshSmartviewTitlebarViewDropdown(this && this.activeFilterId ? this.activeFilterId : ''); } catch (e) {}
          const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;
          if (!ctrl) return;
          ctrl.filters = filters;
          ctrl.forceClientFiltering = Array.isArray(filters) && filters.length > 0;
          ctrl._filteredCache = null;
          window._smartviewFullData = null;
          if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
          if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
          try { closeSmartviewFilterModal(); } catch (e) {}
          try {
            this.activeFilterId = '';
            this.activeFilterName = '';
          } catch (e) {}
        } catch (e) {
          console.error('smartview entityFilter.applyFilters patch error', e);
        }
      };

      // Keep "remove one pill" behavior stable even before openFilters() is used.
      if (typeof window._entityFilter.removeFilter === 'function' && !window._entityFilter._smartviewRemoveFilterPatched) {
        window._entityFilter.removeFilter = function (key) {
          try {
            if (!key) return;
            this.filterObj = this.filterObj || {};
            const removed = this.filterObj[key] || null;
            if (this.filterObj[key]) delete this.filterObj[key];
            try {
              smartviewDeleteNamedView(key);
              if (removed && removed.caption) smartviewDeleteNamedView(removed.caption);
            } catch (e) {}
            try { if (typeof this.createFilterPills === 'function') this.createFilterPills(); } catch (e) {}

            const remainingKeys = Object.keys(this.filterObj || {});
            if (remainingKeys.length > 0) {
              const nextKey = remainingKeys[0];
              const next = this.filterObj[nextKey] || {};
              this.activeFilterId = nextKey;
              this.activeFilterName = next.caption || '';
              this.activeFilterArray = Array.isArray(next.filter) ? next.filter : [];
            } else {
              this.activeFilterId = '';
              this.activeFilterName = '';
              this.activeFilterArray = [];
            }

            if (typeof this.applyFilters === 'function') this.applyFilters();

            // Persist only saved filters (product parity).
            try {
              const savedObj = {};
              remainingKeys.forEach(k => {
                const itm = this.filterObj[k];
                if (itm && itm.save === true) savedObj[k] = itm;
              });
              if (typeof _entityCommon !== 'undefined' && _entityCommon && typeof _entityCommon.setAnalyticsDataWS === 'function') {
                const data = {
                  page: this.pageName,
                  transId: this.entityTransId,
                  properties: { FILTERS: JSON.stringify(savedObj) },
                  allUsers: false
                };
                _entityCommon.setAnalyticsDataWS(data, () => {}, () => {});
              }
            } catch (e) {}
          } catch (e) {
            console.error('smartview entityFilter.removeFilter patch error', e);
          }
        };
        window._entityFilter._smartviewRemoveFilterPatched = true;
      }

      window._entityFilter._smartviewPatchedForPills = true;
      return true;
    } catch (e) {
      return false;
    }
  }

  function smartviewPrepareFilterMetaData(metaData) {
    const adsName = smartviewGetCurrentAdsForViews();
    const ctrl = smartviewGetControllerForViews();
    const candidates = [];
    const baseMeta = Array.isArray(metaData) ? metaData : [];
    const ctrlRaw = (ctrl && Array.isArray(ctrl.lastAdsMetaRaw) && ctrl.lastAdsMetaRaw.length &&
      (!ctrl._adsMetaRawFor || !adsName || String(ctrl._adsMetaRawFor).toLowerCase() === String(adsName).toLowerCase()))
      ? smartviewCloneJsonSafe(ctrl.lastAdsMetaRaw) || []
      : [];
    const rawMeta = ctrlRaw.length ? ctrlRaw : smartviewLoadSmartviewAdsMetaRawFromStorage(adsName);

    // Prefer the raw ADS metadata payload because it preserves the original
    // `filters`, `fdatatype`, `normalized`, and source-table/source-field hints.
    if (Array.isArray(rawMeta) && rawMeta.length) candidates.push(rawMeta);
    if (smartviewLooksLikeAdsMetadataDescriptorMeta(baseMeta)) {
      const fallback = smartviewBuildMetaFallbackFromCurrentRows(baseMeta, adsName);
      if (fallback.length) candidates.push(fallback);
    }

    if (baseMeta.length) candidates.push(baseMeta);

    for (let i = 0; i < candidates.length; i++) {
      const source = Array.isArray(candidates[i]) ? candidates[i] : [];
      const preparedStrict = source
        .map(function (item) {
          return smartviewBuildFilterUiMetaField(item);
        })
        .filter(Boolean);
      if (preparedStrict.length) return preparedStrict;

      const preparedLoose = source
        .map(function (item) {
          return smartviewBuildFilterUiMetaField(item, { ignoreFilterFlag: true });
        })
        .filter(Boolean);
      if (preparedLoose.length) return preparedLoose;
    }

    return [];
  }

  function smartviewCreateInitialFilterPill(mappedFilters, metaData, adsName) {
    try {
      if (!Array.isArray(mappedFilters) || mappedFilters.length === 0) return;
      const EntityFilterCtor = smartviewResolveEntityFilterCtor();
      if (typeof EntityFilterCtor !== 'function') return;

      ensureSmartviewFilterPillsContainer();
      try { ensureSmartviewFilterCompatDom && ensureSmartviewFilterCompatDom(); } catch (e) {}

      ensureSmartviewEntityFilterPatched();

      window._entityFilter = window._entityFilter || new EntityFilterCtor();
      window._entityFilter.metaData = smartviewPrepareFilterMetaData(Array.isArray(metaData) ? metaData : ((window._entity && window._entity.metaData) || []));
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
    try { ensureSmartviewTitlebarViewControls(); } catch (e) {}
    try { refreshSmartviewTitlebarViewDropdown(); } catch (e) {}

    if (window.smartTableController) {
      const ctrl = window.smartTableController;
      const prevAds = (ctrl.adsName || '').toString();
      ctrl.adsName = adsName;

      if (!prevAds || prevAds.toLowerCase() !== adsName.toLowerCase()) {
        ctrl.lastAdsMeta = null;
        ctrl._adsMetaFor = null;
        // Reset projection/grouping when ADS changes.
        ctrl.select_columns = [];
        ctrl.groupby_columns = [];
        ctrl.column_order = smartviewLoadColumnOrderFromStorage(adsName);
        ctrl.aggregations = {};
        try {
          smartviewEnsureColumnOrderLoadedForAds(adsName, (order) => {
            const parsed = smartviewParseColumnOrder(order);
            if (parsed.length) ctrl.column_order = parsed;
          });
        } catch (e) {}
      }

      ctrl.resetPaging();

      // Prefetch ADS metadata (and persist to localStorage) so Filters/Hyperlinks have schema.
      try { if (typeof ctrl.ensureAdsMetadata === 'function') ctrl.ensureAdsMetadata(); } catch (e) {}
      try { smartviewLoadKpiChartsData(adsName, { refreshCache: false }); } catch (e) {}

      ctrl.loadNextPage();
    } else {
      window.smartTableController = new SmartViewTableController({
        adsName: adsName,
        pageSize: 100,
        currentPage: 1,
        sorting: window.smartTableController?.sorting || []
      });
      try { smartviewLoadKpiChartsData(adsName, { refreshCache: false }); } catch (e) {}
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

    // ESC Ã¢â€ â€™ close search
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

  function smartviewGetFilterDropdownPanel() {
    return document.getElementById('smartviewInlineFilterPanel');
  }

  function smartviewGetFilterDropdownAnchor() {
    return document.getElementById('smartviewFilterToolbarBtn');
  }

  function smartviewTeardownFilterDropdownListeners() {
    const state = window._smartviewFilterDropdownState;
    if (!state || typeof state !== 'object') return;

    try { document.removeEventListener('pointerdown', state.pointerHandler, true); } catch (e) {}
    try { document.removeEventListener('keydown', state.keyHandler, true); } catch (e) {}
    try { window.removeEventListener('resize', state.repositionHandler); } catch (e) {}
    try { window.removeEventListener('scroll', state.repositionHandler, true); } catch (e) {}

    window._smartviewFilterDropdownState = null;
  }

  function smartviewIsFilterDropdownInteractiveTarget(target, eventObj) {
    const nodes = [];
    if (eventObj && typeof eventObj.composedPath === 'function') {
      try {
        eventObj.composedPath().forEach(function (n) {
          if (n && n.nodeType === 1) nodes.push(n);
        });
      } catch (e) {}
    }

    const el = (target && target.nodeType === 1) ? target : (target && target.parentElement ? target.parentElement : null);
    if (el) nodes.push(el);
    if (!nodes.length) return false;

    return nodes.some(function (node) {
      if (!node || !node.closest) return false;
      return !!(
        node.closest('#smartviewInlineFilterPanel') ||
        node.closest('#smartviewFilterToolbarBtn') ||
        node.closest('.select2-dropdown') ||
        node.closest('.select2-container') ||
        node.closest('.select2-search') ||
        node.closest('.select2-results') ||
        node.closest('[id^="select2-"]') ||
        node.closest('[role="listbox"]') ||
        node.closest('[role="option"]') ||
        node.closest('#ui-datepicker-div') ||
        node.closest('.ui-datepicker') ||
        node.closest('.flatpickr-calendar') ||
        node.closest('.daterangepicker') ||
        node.closest('.bootstrap-datetimepicker-widget') ||
        node.closest('.datetimepicker') ||
        node.closest('.xdsoft_datetimepicker') ||
        node.closest('.mx-datepicker-popup') ||
        node.closest('.ant-picker-dropdown') ||
        node.closest('.react-datepicker-popper') ||
        node.closest('[class*="datepicker"]') ||
        node.closest('[id*="datepicker"]')
      );
    });
  }

  function smartviewEnsureDatepickerLayerStyles() {
    try {
      if (document.getElementById('smartviewDatepickerLayerStyles')) return;
      const style = document.createElement('style');
      style.id = 'smartviewDatepickerLayerStyles';
      style.textContent = `
        #ui-datepicker-div,
        .ui-datepicker,
        .flatpickr-calendar,
        .daterangepicker,
        .bootstrap-datetimepicker-widget,
        .datetimepicker,
        .xdsoft_datetimepicker,
        .mx-datepicker-popup,
        .ant-picker-dropdown,
        .react-datepicker-popper {
          z-index: 13050 !important;
        }
      `;
      document.head.appendChild(style);
    } catch (e) {}
  }

  function smartviewCoerceDateInputsInFilterPanel() {
    try {
      const dateFields = document.querySelectorAll('#dvModalFilter .filter-fld[data-type=Date]');
      dateFields.forEach(fld => {
        fld.querySelectorAll('input').forEach(inp => {
          if (!inp) return;
          const raw = String(inp.value || '').trim();
          if ((inp.type || '').toLowerCase() !== 'date') inp.type = 'date';
          inp.setAttribute('autocomplete', 'off');
          if (!raw) return;
          try {
            const m = moment(raw, ['YYYY-MM-DD', 'DD/MM/YYYY', 'MM/DD/YYYY', 'DD-MMM-YYYY', advFilterDtCulture], true);
            if (m.isValid()) inp.value = m.format('YYYY-MM-DD');
          } catch (e) {}
        });
      });
    } catch (e) {}
  }

  function smartviewPositionFilterDropdown(anchorEl) {
    const panel = smartviewGetFilterDropdownPanel();
    if (!panel) return;

    const viewportPadding = 12;
    const trigger = anchorEl || smartviewGetFilterDropdownAnchor();
    const fallbackRect = {
      top: viewportPadding,
      bottom: 70,
      right: window.innerWidth - viewportPadding
    };
    const rect = trigger ? trigger.getBoundingClientRect() : fallbackRect;
    const availableWidth = Math.max(220, window.innerWidth - (viewportPadding * 2));
    const desiredWidth = Math.min(560, availableWidth);

    panel.style.width = `${desiredWidth}px`;
    panel.style.maxWidth = `${availableWidth}px`;

    let left = rect.right - desiredWidth;
    left = Math.max(viewportPadding, Math.min(left, window.innerWidth - desiredWidth - viewportPadding));

    const estimatedHeight = Math.max(320, panel.offsetHeight || 0);
    const belowTop = rect.bottom + 10;
    const belowSpace = Math.max(0, window.innerHeight - belowTop - viewportPadding);
    const aboveSpace = Math.max(0, rect.top - viewportPadding - 10);
    const placeAbove = belowSpace < 280 && aboveSpace > belowSpace;
    const top = placeAbove
      ? Math.max(viewportPadding, rect.top - Math.min(estimatedHeight, aboveSpace))
      : Math.max(viewportPadding, belowTop);
    const maxHeight = Math.max(260, placeAbove ? aboveSpace : belowSpace);

    panel.style.left = `${Math.round(left)}px`;
    panel.style.top = `${Math.round(top)}px`;
    panel.style.maxHeight = `${Math.round(maxHeight)}px`;
    panel.setAttribute('data-placement', placeAbove ? 'top' : 'bottom');
  }

  function smartviewShowFilterDropdown(anchorEl) {
    const panel = smartviewGetFilterDropdownPanel();
    if (!panel) return;

    const trigger = anchorEl || smartviewGetFilterDropdownAnchor();
    smartviewTeardownFilterDropdownListeners();

    panel.style.display = 'block';
    panel.style.visibility = 'hidden';
    panel.classList.add('sv-filter-dropdown-open');
    panel.setAttribute('aria-hidden', 'false');
    smartviewPositionFilterDropdown(trigger);
    panel.style.visibility = 'visible';

    if (trigger) trigger.setAttribute('aria-expanded', 'true');

    const pointerHandler = function (event) {
      if (smartviewIsFilterDropdownInteractiveTarget(event.target, event)) return;
      closeSmartviewFilterModal();
    };
    const keyHandler = function (event) {
      if (event.key === 'Escape') closeSmartviewFilterModal();
    };
    const repositionHandler = function () {
      const activePanel = smartviewGetFilterDropdownPanel();
      if (!activePanel) return;
      const isOpen = (activePanel.style.display !== 'none') && (window.getComputedStyle(activePanel).display !== 'none');
      if (!isOpen) return;
      smartviewPositionFilterDropdown(trigger || smartviewGetFilterDropdownAnchor());
    };

    document.addEventListener('pointerdown', pointerHandler, true);
    document.addEventListener('keydown', keyHandler, true);
    window.addEventListener('resize', repositionHandler);
    window.addEventListener('scroll', repositionHandler, true);

    window._smartviewFilterDropdownState = {
      pointerHandler: pointerHandler,
      keyHandler: keyHandler,
      repositionHandler: repositionHandler
    };
  }

  function closeSmartviewFilterModal() {
    try {
      // Inline filter panel (SmartView) - collapse it first.
      try {
        smartviewTeardownFilterDropdownListeners();
        const panel = document.getElementById('smartviewInlineFilterPanel');
        if (panel) {
          panel.style.display = 'none';
          panel.style.visibility = '';
          panel.style.left = '';
          panel.style.top = '';
          panel.style.width = '';
          panel.style.maxWidth = '';
          panel.style.maxHeight = '';
          panel.classList.remove('sv-filter-dropdown-open');
          panel.setAttribute('aria-hidden', 'true');
        }
        const filterBtn = smartviewGetFilterDropdownAnchor();
        if (filterBtn) filterBtn.setAttribute('aria-expanded', 'false');
        try {
          if (window.jQuery) {
            $('#dvModalFilter .filter-fld.select2-hidden-accessible').each(function () {
              try { $(this).select2('close'); } catch (err) {}
            });
          }
        } catch (err) {}
      } catch (e) {}

      const modalEl = document.getElementById('filterModal');
      if (!modalEl) return;

      // Bootstrap 5 modal instance
      if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
        let bs = null;
        try { bs = bootstrap.Modal.getInstance(modalEl); } catch (e) {}
        try { if (!bs) bs = new bootstrap.Modal(modalEl); } catch (e) {}
        try { if (bs && typeof bs.hide === 'function') bs.hide(); } catch (e) {}
      }

      // jQuery/Bootstrap fallback
      try {
        if (window.jQuery && typeof $('#filterModal').modal === 'function') $('#filterModal').modal('hide');
      } catch (e) {}

      // Hard fallback (in case instance APIs fail)
      try {
        modalEl.classList.remove('show');
        modalEl.setAttribute('aria-hidden', 'true');
        modalEl.style.display = 'none';
        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('padding-right');
        document.querySelectorAll('.modal-backdrop').forEach(b => b.remove());
      } catch (e) {}
    } catch (e) {}
  }

  function smartviewResolveEntityFilterCtor() {
    try {
      // Top-level `class EntityFilter {}` may exist as a global lexical binding
      // without being attached to `window.EntityFilter`.
      if (typeof EntityFilter === 'function') {
        try { window.EntityFilter = EntityFilter; } catch (e) {}
        return EntityFilter;
      }
    } catch (e) {}

    try {
      if (typeof window.EntityFilter === 'function') return window.EntityFilter;
    } catch (e) {}

    // Some runtimes keep global lexical bindings inaccessible via window property.
    // Global eval can still resolve the constructor.
    try {
      if (typeof window.eval === 'function') {
        const evalCtor = window.eval('(typeof EntityFilter === "function") ? EntityFilter : null');
        if (typeof evalCtor === 'function') {
          try { window.EntityFilter = evalCtor; } catch (e) {}
          return evalCtor;
        }
      }
    } catch (e) {}

    try {
      const instCtor = smartviewGetEntityFilterCtorFromInstance(window._entityFilter);
      if (typeof instCtor === 'function') {
        try { window.EntityFilter = instCtor; } catch (e) {}
        return instCtor;
      }
    } catch (e) {}

    const scopes = [];
    try { scopes.push(window); } catch (e) {}
    try { if (window.parent && window.parent !== window) scopes.push(window.parent); } catch (e) {}
    try { if (window.top && window.top !== window && window.top !== window.parent) scopes.push(window.top); } catch (e) {}

    for (let i = 0; i < scopes.length; i++) {
      try {
        const s = scopes[i];
        if (s && typeof s.EntityFilter === 'function') {
          window.EntityFilter = s.EntityFilter;
          return window.EntityFilter;
        }
      } catch (e) {}
      try {
        const s = scopes[i];
        const instCtor = smartviewGetEntityFilterCtorFromInstance(s && s._entityFilter);
        if (typeof instCtor === 'function') {
          try { window.EntityFilter = instCtor; } catch (e) {}
          return window.EntityFilter;
        }
      } catch (e) {}
    }
    return null;
  }

  function smartviewGetEntityFilterCtorFromInstance(instance) {
    try {
      if (!instance || typeof instance !== 'object') return null;
      const ctor = instance.constructor;
      if (typeof ctor !== 'function') return null;
      const ctorName = String((ctor && ctor.name) || '').toLowerCase();
      if (ctorName === 'entityfilter' || ctorName.indexOf('entityfilter') > -1) return ctor;
    } catch (e) {}
    return null;
  }

  function smartviewNormalizeScriptSrc(url) {
    try {
      const raw = String(url || '').trim();
      if (!raw) return '';
      const abs = new URL(raw, window.location.href);
      return `${abs.origin}${abs.pathname}`.toLowerCase();
    } catch (e) {
      try {
        return String(url || '').split('#')[0].split('?')[0].trim().toLowerCase();
      } catch (_e) {
        return '';
      }
    }
  }

  function smartviewIsEntityFilterScriptSrc(url) {
    const normalized = smartviewNormalizeScriptSrc(url);
    return normalized.endsWith('/entity-filter.js') || normalized.indexOf('/entity-filter.js') > -1;
  }

  function smartviewFindEntityFilterScriptTag(preferredUrl, exactOnly) {
    try {
      const exactMatchOnly = !!exactOnly;
      const preferred = smartviewNormalizeScriptSrc(preferredUrl);
      const scripts = Array.from(document.querySelectorAll('script[src]'));

      for (let i = 0; i < scripts.length; i++) {
        const scriptEl = scripts[i];
        const src = scriptEl.getAttribute('src') || scriptEl.src || '';
        const normalized = smartviewNormalizeScriptSrc(src);
        if (!normalized) continue;

        if (preferred && normalized === preferred) return scriptEl;
        if (exactMatchOnly) continue;
        if (smartviewIsEntityFilterScriptSrc(src)) return scriptEl;
      }
    } catch (e) {}
    return null;
  }

  function smartviewWaitForEntityFilterCtor(onDone, timeoutMs) {
    const done = function (ok) {
      try { if (typeof onDone === 'function') onDone(!!ok); } catch (e) {}
    };
    const maxWait = Math.max(120, Number(timeoutMs) || 500);
    const deadline = Date.now() + maxWait;
    let settled = false;

    const settle = function (ok) {
      if (settled) return;
      settled = true;
      done(!!ok);
    };

    const check = function () {
      const ctor = smartviewResolveEntityFilterCtor();
      if (typeof ctor === 'function') {
        settle(true);
        return true;
      }
      return false;
    };

    if (check()) return;
    const timer = setInterval(function () {
      if (check()) {
        try { clearInterval(timer); } catch (e) {}
        return;
      }
      if (Date.now() >= deadline) {
        try { clearInterval(timer); } catch (e) {}
        settle(false);
      }
    }, 70);
  }

  function smartviewEnsureEntityFilterPrerequisites() {
    try {
      if (typeof window.callParent !== 'function') {
        window.callParent = function (name) {
          const key = String(name || '').trim();
          if (!key) return '';
          try {
            if (window.parent && window.parent !== window) {
              const v = window.parent[key];
              if (typeof v === 'function') return v();
              if (v !== undefined) return v;
            }
          } catch (e) {}
          try {
            const v = window[key];
            if (typeof v === 'function') return v();
            if (v !== undefined) return v;
          } catch (e) {}
          return '';
        };
      }
    } catch (e) {}

    try {
      if (typeof window.callParentNew !== 'function') {
        window.callParentNew = window.callParent;
      }
    } catch (e) {}

    try {
      if (typeof window.glCulture === 'undefined') {
        window.glCulture = (typeof dtCulture !== 'undefined' && dtCulture) ? dtCulture : 'en-us';
      }
    } catch (e) {}
  }

  function smartviewEnsureEntityFilterLoaded(callback) {
    const done = function (ok) {
      try { if (typeof callback === 'function') callback(!!ok, smartviewResolveEntityFilterCtor()); } catch (e) {}
    };

    try {
      smartviewEnsureEntityFilterPrerequisites();

      const ctor = smartviewResolveEntityFilterCtor();
      if (typeof ctor === 'function') {
        done(true);
        return;
      }

      const state = window._smartviewEntityFilterLoadState = window._smartviewEntityFilterLoadState || {
        loading: false,
        queue: [],
        tried: {}
      };

      state.queue.push(done);
      if (state.loading) return;
      state.loading = true;

      const flush = function (ok) {
        state.loading = false;
        const q = Array.isArray(state.queue) ? state.queue.splice(0) : [];
        q.forEach(fn => {
          try { fn(!!ok); } catch (e) {}
        });
      };

      const baseUrl = (typeof smartviewGetAssetBaseUrl === 'function') ? smartviewGetAssetBaseUrl() : '';
      const candidates = [];
      if (baseUrl) {
        candidates.push(`${baseUrl}/Js/Entity-Filter.js`);
        candidates.push(`${baseUrl}/js/Entity-Filter.js`);
      }
      candidates.push('/Js/Entity-Filter.js');
      candidates.push('/js/Entity-Filter.js');
      candidates.push('../../Js/Entity-Filter.js');
      candidates.push('../../js/Entity-Filter.js');

      const uniqueUrls = candidates.filter(function (u, idx, arr) {
        const val = String(u || '').trim();
        return !!val && arr.indexOf(u) === idx;
      });

      const existingScript = smartviewFindEntityFilterScriptTag('', false);
      if (existingScript) {
        // Entity-Filter.js is already on the page.
        // Do not inject again (class redeclaration error). Just wait briefly for ctor to appear.
        smartviewWaitForEntityFilterCtor(function (ok) {
          flush(!!ok);
        }, 650);
        return;
      }

      const tryLoadAt = function (idx) {
        const ctorNow = smartviewResolveEntityFilterCtor();
        if (typeof ctorNow === 'function') {
          flush(true);
          return;
        }
        if (idx >= uniqueUrls.length) {
          flush(false);
          return;
        }

        const url = uniqueUrls[idx];
        if (state.tried[url]) {
          tryLoadAt(idx + 1);
          return;
        }
        state.tried[url] = true;

        const existingForUrl = smartviewFindEntityFilterScriptTag(url, true);
        if (existingForUrl) {
          smartviewWaitForEntityFilterCtor(function (ok) {
            if (ok) flush(true);
            else tryLoadAt(idx + 1);
          }, 500);
          return;
        }

        const script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = url;
        script.async = true;
        script.onload = function () {
          try { smartviewEnsureEntityFilterPrerequisites(); } catch (e) {}
          smartviewWaitForEntityFilterCtor(function (ok) {
            if (ok) flush(true);
            else tryLoadAt(idx + 1);
          }, 600);
        };
        script.onerror = function () {
          try { console.warn('Entity-Filter loader failed for', url); } catch (e) {}
          tryLoadAt(idx + 1);
        };
        (document.head || document.documentElement || document.body).appendChild(script);
      };

      tryLoadAt(0);
    } catch (e) {
      done(false);
    }
  }

  /**
   * Open the Filter modal. Ensure we fetch ads-metadata first (so the filter UI shows correct fields).
   * Called by toolbar button: onclick="openFilters(); return false;"
   */
  function openFilters() {
    try {
      // Toggle inline panel: if already open, collapse and return.
      try {
        const panel = document.getElementById('smartviewInlineFilterPanel');
        if (panel) {
          const isOpen = (panel.style.display !== 'none') && (window.getComputedStyle(panel).display !== 'none');
          if (isOpen) {
            closeSmartviewFilterModal();
            return;
          }
        }
      } catch (e) {}

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
            smartviewSyncKeyFieldState(cached, controller);
          }
        }
      } catch (e) {}

      const showModalWithFilter = function() {
        // Ensure inline filter panel exists and required compat IDs exist.
        try { smartviewShowFilterDropdown(); } catch (e) {}

        try { ensureSmartviewFilterCompatDom && ensureSmartviewFilterCompatDom(); } catch (e) {}

        // Ensure EntityFilter class is available (Entity-Filter.js)
        let EntityFilterCtor = smartviewResolveEntityFilterCtor();
        if (typeof EntityFilterCtor !== 'function') {
          smartviewEnsureEntityFilterLoaded(function (ok, loadedCtor) {
            if (!ok || typeof loadedCtor !== 'function') {
              console.error('EntityFilter not found. Make sure Entity-Filter.js is loaded on the page.');
              return;
            }
            try { showModalWithFilter(); } catch (err) { console.error('showModalWithFilter retry failed', err); }
          });
          return;
        }

        // reuse existing instance or create new
        window._entityFilter = window._entityFilter || new EntityFilterCtor();
        // supply the metadata fetched for this ADS
        // Prefer ADS metadata (has flags like normalized/filters) over inferred table metadata.
        const filterMetaSource = controller.lastAdsMeta || window._entity.metaData || [];
        const filterMeta = smartviewPrepareFilterMetaData(filterMetaSource);
        window._entityFilter.metaData = filterMeta;
        if (smartviewLooksLikeAdsMetadataDescriptorMeta(filterMetaSource) && filterMeta.length) {
          controller.lastAdsMeta = filterMeta;
          window._entity = window._entity || {};
          window._entity.metaData = filterMeta;
        }
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
                try {
                  if (typeof smartviewExtractRowsFromAxListResponse === 'function') {
                    const extracted = smartviewExtractRowsFromAxListResponse(resp);
                    if (Array.isArray(extracted?.rows) && extracted.rows.length) return extracted.rows;
                  }
                } catch (e) {}

                try {
                  const parsed = (typeof safeParseAxResponse === 'function') ? safeParseAxResponse(resp) : resp;
                  const dsBlock = parsed?.result?.data?.[0] || parsed?.data?.[0] || parsed?.result || parsed;
                  let rows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
                  if (rows && rows.length && rows[0] && Array.isArray(rows[0].data)) rows = rows[0].data;
                  if (Array.isArray(rows) && rows.length) return rows;
                } catch (e) {}

                // Last-resort fallback for stringified responses that still contain datavalue entries.
                try {
                  const raw = String(resp || '');
                  const rx = /"datavalue"\s*:\s*"([^"]*)"/gi;
                  const out = [];
                  let m;
                  while ((m = rx.exec(raw)) !== null) {
                    const val = (m[1] || '').toString();
                    if (val) out.push({ datavalue: val });
                  }
                  return out;
                } catch (e) {}

                return [];
              }

              function rowToOption(row) {
                if (row === null || row === undefined) return null;
                if (typeof row === 'string' || typeof row === 'number') {
                  const v = String(row).trim();
                  return v ? { id: v, text: v } : null;
                }
                if (typeof row !== 'object') return null;

                const id = smartviewGetObjectValueCI(row, ['datavalue', 'value', 'id', 'code', 'name', 'text']);
                const text = smartviewGetObjectValueCI(row, ['datacaption', 'caption', 'text', 'value', 'name', 'code', 'id']) || id;

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

              function loadDropdownOptions(cacheKey, psrctxtValue, done) {
                const finish = function (arr) {
                  try { if (typeof done === 'function') done(Array.isArray(arr) ? arr : []); } catch (e) {}
                };

                try {
                  const cached = window._smartviewDropdownCache[cacheKey];
                  if (Array.isArray(cached) && cached.length) {
                    finish(cached);
                    return;
                  }

                  const callParams = {
                    adsNames: ['ds_smartlist_filters'],
                    refreshCache: false,
                    sqlParams: { psrctxt: psrctxtValue },
                    props: { ADS: true, CachePermissions: true, getallrecordscount: false, pageno: 1, pagesize: 0 }
                  };

                  const scopes = [parent, window, window.top];
                  const caller = scopes.find(
                    w => w && typeof w.GetDataFromAxList === 'function'
                  );

                  if (!caller || typeof caller.GetDataFromAxList !== 'function') {
                    console.warn('GetDataFromAxList not available for ds_smartlist_filters');
                    finish([]);
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
                      finish(opts);
                    } catch (e) {
                      console.error('ds_smartlist_filters parse error', e);
                      finish([]);
                    }
                  }, function (err) {
                    console.warn('ds_smartlist_filters call failed', err);
                    finish([]);
                  });
                } catch (e) {
                  console.error('loadDropdownOptions failed', e);
                  finish([]);
                }
              }

              function ensureOptionsOnSelect($select, options) {
                try {
                  if (!$select || !$select.length) return;
                  const valuesPresent = new Set(
                    Array.from($select[0].options || []).map(opt => String(opt.value || '').trim().toLowerCase()).filter(Boolean)
                  );
                  (Array.isArray(options) ? options : []).forEach(function (opt) {
                    const value = String((opt && opt.id) || '').trim();
                    const text = String((opt && (opt.text || opt.id)) || '').trim();
                    if (!value) return;
                    const token = value.toLowerCase();
                    if (valuesPresent.has(token)) return;
                    const node = new Option(text || value, value, false, false);
                    $select.append(node);
                    valuesPresent.add(token);
                  });
                } catch (e) {
                  console.error('ensureOptionsOnSelect failed', e);
                }
              }

              function initializeLocalSelect2($select, options) {
                try {
                  if (!$select || !$select.length) return;
                  const rawSelectedValues = $select.val();
                  const selectedValues = Array.isArray(rawSelectedValues)
                    ? rawSelectedValues.slice()
                    : ((rawSelectedValues === null || rawSelectedValues === undefined || rawSelectedValues === '')
                      ? []
                      : [rawSelectedValues]);
                  const normalizedOptions = (Array.isArray(options) ? options : []).filter(function (o) {
                    return !!(o && String(o.id || '').trim());
                  });
                  try {
                    if ($select.hasClass('select2-hidden-accessible')) $select.select2('destroy');
                  } catch (e) {}

                  // Keep existing options when backend temporarily returns empty payload.
                  // This avoids a visible "blank dropdown" flicker for end users.
                  if (normalizedOptions.length || !$select[0] || !$select[0].options || !$select[0].options.length) {
                    $select.empty();
                  }
                  ensureOptionsOnSelect($select, normalizedOptions);

                  const dropdownHost = smartviewGetFilterDropdownPanel() || document.body;

                  $select.select2({
                    multiple: true,
                    width: '100%',
                    dropdownParent: $(dropdownHost),
                    dropdownCssClass: 'smartview-filter-select2-dropdown',
                    minimumResultsForSearch: 0,
                    closeOnSelect: false,
                    data: normalizedOptions,
                    templateResult: function (item) {
                      if (!item) return '';
                      return item.text || item.id || '';
                    },
                    templateSelection: function (item) {
                      if (!item) return '';
                      return item.text || item.id || '';
                    },
                    escapeMarkup: function (m) { return m; }
                  });

                  if (selectedValues.length) {
                    $select.val(selectedValues).trigger('change');
                  }
                } catch (e) {
                  console.error('initializeLocalSelect2 failed', e);
                }
              }

              function ensureDropdownFieldAsSelect(fld) {
                try {
                  if (!fld) return null;
                  if (String(fld.tagName || '').toUpperCase() === 'SELECT') {
                    fld.setAttribute('multiple', 'multiple');
                    return fld;
                  }

                  const select = document.createElement('select');
                  select.id = fld.id || '';
                  select.name = fld.name || '';
                  select.className = fld.className || '';
                  select.setAttribute('multiple', 'multiple');
                  select.setAttribute('data-type', 'DropDown');
                  select.style.width = '100%';

                  if (fld.dataset && fld.dataset.field) select.dataset.field = fld.dataset.field;
                  if (fld.getAttribute('aria-label')) select.setAttribute('aria-label', fld.getAttribute('aria-label'));

                  const existingValue = (fld.value === undefined || fld.value === null) ? '' : String(fld.value).trim();
                  if (existingValue) {
                    const opt = new Option(existingValue, existingValue, true, true);
                    select.add(opt);
                  }

                  fld.replaceWith(select);
                  return select;
                } catch (e) {
                  console.error('ensureDropdownFieldAsSelect failed', e);
                  return fld;
                }
              }

              let needsProductInit = false;
              let handledSmartviewDropdowns = 0;

              const dropdownFields = Array.from(document.querySelectorAll('#dvModalFilter .filter-fld')).filter(function (fld) {
                try {
                  if (!fld) return false;
                  const dt = String(fld.getAttribute('data-type') || '').trim().toLowerCase().replace(/[\s_]+/g, '');
                  if (dt === 'dropdown' || dt === 'select') return true;

                  // Metadata fallback: treat normalized/source-backed fields as dropdowns
                  // even when data-type casing/shape from UI is inconsistent.
                  const fldId = (fld.id || '').replace('filter_', '');
                  const meta = getMetaForField(fldId) || {};
                  const cd = String(meta.cdatatype || '').trim().toLowerCase().replace(/[\s_]+/g, '');
                  if (cd === 'dropdown' || cd === 'select') return true;
                  const normalized = String(meta.normalized || '').trim().toUpperCase() === 'T';
                  if (normalized) return true;
                  if ((meta.psrctxt || meta.psrcTxt || '').toString().trim()) return true;
                } catch (e) {}
                return false;
              });

              dropdownFields.forEach(fld => {
                try {
                  const fldId = (fld.id || '').replace('filter_', '');
                  const meta = getMetaForField(fldId);
                  const psrctxt = buildPsrctxt(meta, fldId);

                  // Only SmartView ADS dropdowns with psrctxt are handled here; others fall back to product init.
                  if (!psrctxt) { needsProductInit = true; return; }

                  const selectEl = ensureDropdownFieldAsSelect(fld) || fld;
                  const $fld = $(selectEl);
                  handledSmartviewDropdowns += 1;

                  // Ensure wrapper + buttons exist (idempotent)
                  let $wrapper = $fld.closest('.dropdown-wrapper');
                  if ($wrapper.length === 0) {
                    $wrapper = $('<div class="dropdown-wrapper" style="position:relative;"></div>');
                    $fld.wrap($wrapper);
                    $wrapper = $fld.closest('.dropdown-wrapper');
                  }

                  // Keep a single dropdown arrow from the select/select2 control.
                  // Remove legacy custom open button to avoid duplicate chevrons.
                  $wrapper.find('.dropdown-toggle-btn').remove();
                  if ($wrapper.find('.clear-all-btn').length === 0) {
                    $wrapper.append(`<button type="button" class="clear-all-btn btn btn-sm" aria-label="Clear" title="Clear"
                      style="position:absolute; right:4px; top:6px; z-index:1100; height:30px; width:28px; padding:0; line-height:1; display:none;"><span class="material-icons" style="font-size:16px; line-height:1;">close</span></button>`);
                  }

                  const $clearBtn = $wrapper.find('.clear-all-btn');
                  let reopenAfterRefresh = false;

                  const cacheKey = psrctxt;
                  loadDropdownOptions(cacheKey, psrctxt, function (opts) {
                    initializeLocalSelect2($fld, opts);
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
                    .on('select2:unselect.smartviewDropEvents', updateClearVisibility)
                    .on('select2:opening.smartviewDropEvents', function (e) {
                      if (reopenAfterRefresh) {
                        reopenAfterRefresh = false;
                        return;
                      }
                      const currentOptCount = ($fld[0] && $fld[0].options) ? $fld[0].options.length : 0;
                      if (currentOptCount > 0) return;
                      e.preventDefault();
                      loadDropdownOptions(cacheKey, psrctxt, function (opts) {
                        initializeLocalSelect2($fld, opts);
                        reopenAfterRefresh = true;
                        try { $fld.select2('open'); } catch (err) {}
                      });
                    })
                    .on('select2:open.smartviewDropEvents', function () {
                      try {
                        const openDropdown = document.querySelector('body > .select2-container--open .select2-dropdown');
                        if (!openDropdown) return;
                        openDropdown.style.zIndex = '1605';
                      } catch (err) {}
                    });

                  updateClearVisibility();

                } catch (err) {
                  console.error('SmartView initializeDropdowns error', err);
                }
              });

              // If there are any dropdowns without psrctxt, let product init handle them.
              // (call once; it will no-op for already initialized selects)
              if (needsProductInit && handledSmartviewDropdowns === 0) {
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
            const res = originalEditPill(key);
            // EntityFilter normally opens a modal; SmartView uses an inline panel instead.
            try { smartviewShowFilterDropdown(); } catch (e) {}
            return res;
          };
          window._entityFilter._smartviewEditPillPatched = true;
        }

        // Patch EntityFilter.handleApply to guarantee modal close on successful apply.
        if (window._entityFilter && typeof window._entityFilter.handleApply === 'function' && !window._entityFilter._smartviewHandleApplyPatched) {
          const originalHandleApply = window._entityFilter.handleApply.bind(window._entityFilter);
          window._entityFilter.handleApply = function () {
            const out = originalHandleApply();
            try {
              smartviewPersistActiveSavedFilterAsView(this, this.activeFilterArray || []);
              refreshSmartviewTitlebarViewDropdown(this && this.activeFilterId ? this.activeFilterId : '');
              if (Array.isArray(this.activeFilterArray) && this.activeFilterArray.length > 0) {
                closeSmartviewFilterModal();
              }
            } catch (e) {}
            return out;
          };
          window._entityFilter._smartviewHandleApplyPatched = true;
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
            try {
              if (this && this.filterObj && typeof this.filterObj === 'object') {
                Object.keys(this.filterObj).forEach(k => {
                  const it = this.filterObj[k];
                  if (!it || it.save !== true || !it.caption) return;
                  smartviewUpsertNamedView(String(it.caption), { id: String(k), filtersOverride: Array.isArray(it.filter) ? it.filter : [] });
                });
              }
            } catch (e) {}
            try {
              const activeKey = this && this.activeFilterId ? this.activeFilterId : '';
              refreshSmartviewFilterViewDropdown(activeKey);
              refreshSmartviewTitlebarViewDropdown(activeKey);
            } catch (e) {}
            return res;
          };
          window._entityFilter._smartviewCreatePillsPatched = true;
        }

        // Patch EntityFilter.removeFilter:
        // Product implementation clears all active filters when no "saved" filters exist.
        // In SmartView most pills are unsaved, so deleting one pill should keep remaining pills applied.
        if (window._entityFilter && typeof window._entityFilter.removeFilter === 'function' && !window._entityFilter._smartviewRemoveFilterPatched) {
          window._entityFilter.removeFilter = function (key) {
            try {
              if (!key) return;

              this.filterObj = this.filterObj || {};
              const removed = this.filterObj[key] || null;
              if (this.filterObj[key]) delete this.filterObj[key];
              try {
                smartviewDeleteNamedView(key);
                if (removed && removed.caption) smartviewDeleteNamedView(removed.caption);
              } catch (e) {}

              // Rebuild pills first so UI stays in sync.
              try { if (typeof this.createFilterPills === 'function') this.createFilterPills(); } catch (e) {}

              const remainingKeys = Object.keys(this.filterObj || {});
              if (remainingKeys.length > 0) {
                const nextKey = remainingKeys[0];
                const next = this.filterObj[nextKey] || {};
                this.activeFilterId = nextKey;
                this.activeFilterName = next.caption || '';
                this.activeFilterArray = Array.isArray(next.filter) ? next.filter : [];
              } else {
                this.activeFilterId = '';
                this.activeFilterName = '';
                this.activeFilterArray = [];
              }

              // Re-apply with remaining filters (or clear all when none remain).
              try {
                if (typeof this.applyFilters === 'function') this.applyFilters();
              } catch (e) {
                console.error('SmartView removeFilter apply error', e);
              }

              // Keep server-side saved-filters persistence behavior.
              try {
                const savedObj = {};
                remainingKeys.forEach(k => {
                  const itm = this.filterObj[k];
                  if (itm && itm.save === true) savedObj[k] = itm;
                });
                if (typeof _entityCommon !== 'undefined' && _entityCommon && typeof _entityCommon.setAnalyticsDataWS === 'function') {
                  const data = {
                    page: this.pageName,
                    transId: this.entityTransId,
                    properties: { FILTERS: JSON.stringify(savedObj) },
                    allUsers: false
                  };
                  _entityCommon.setAnalyticsDataWS(data, () => {}, () => {});
                }
              } catch (e) {}
            } catch (err) {
              console.error('SmartView patched removeFilter failed', err);
            }
          };
          window._entityFilter._smartviewRemoveFilterPatched = true;
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
                      return m.format("DD/MM/YYYY");
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

        // Saved filter views dropdown (named filter presets)
        try {
          ensureSmartviewFilterViewDropdown();
          refreshSmartviewFilterViewDropdown();
        } catch (e) {}

        // Ensure Apply button invokes the EntityFilter handler (minimal fix)
  if (window._entityFilter && typeof window._entityFilter.handleApply === 'function') {
    // remove any existing handlers to avoid duplicate calls
    $('#applyFilterButton').off('click').on('click.smartview_apply', function (ev) {
      try {
        const applySeqBefore = Number(window._smartviewFilterApplySeq) || 0;
        window._entityFilter.handleApply();
        const nextFilters = smartviewGetEntityFilterArray(window._entityFilter);
        if ((Number(window._smartviewFilterApplySeq) || 0) === applySeqBefore && nextFilters.length && typeof window._entityFilter.applyFilters === 'function') {
          window._entityFilter.applyFilters();
        }
        if (window._entityFilter && nextFilters.length > 0) {
          closeSmartviewFilterModal();
        }
      } catch (err) {
        console.error('error calling entityFilter.handleApply', err);
      }
      return false;
    });
  }

  (function attachEntityFilterToSmartView() {
    try {
      if (!window._entityFilter) {
        // Nothing to attach yet Ã¢â‚¬â€ keep safe bindings (product code expects these handlers)
        $('#applyFilterButton').off('click').on('click.smartview_apply', function (ev) {
          ev.preventDefault();
          try {
            if (window._entityFilter && typeof window._entityFilter.handleApply === 'function') {
              const applySeqBefore = Number(window._smartviewFilterApplySeq) || 0;
              window._entityFilter.handleApply();
              const nextFilters = smartviewGetEntityFilterArray(window._entityFilter);
              if ((Number(window._smartviewFilterApplySeq) || 0) === applySeqBefore && nextFilters.length && typeof window._entityFilter.applyFilters === 'function') {
                window._entityFilter.applyFilters();
              }
              if (nextFilters.length > 0) {
                closeSmartviewFilterModal();
              }
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

        $('#clearFilterBtn').off('click').on('click.smartviewClear', function () {
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
      $('#applyFilterButton').off('click').on('click.smartview_apply', function (ev) {
        ev.preventDefault();
        try {
          if (window._entityFilter && typeof window._entityFilter.handleApply === 'function') {
            const applySeqBefore = Number(window._smartviewFilterApplySeq) || 0;
            window._entityFilter.handleApply();
            const nextFilters = smartviewGetEntityFilterArray(window._entityFilter);
            if ((Number(window._smartviewFilterApplySeq) || 0) === applySeqBefore && nextFilters.length && typeof window._entityFilter.applyFilters === 'function') {
              window._entityFilter.applyFilters();
            }
            if (nextFilters.length > 0) {
              closeSmartviewFilterModal();
            }
          } else if (window._entityFilter && typeof window._entityFilter.applyFilters === 'function') {
            window._entityFilter.applyFilters();
            closeSmartviewFilterModal();
          } else {
            console.warn('apply clicked: no handleApply/applyFilters on _entityFilter');
          }
        } catch (err) { console.error('apply button binding error', err); }
        return false;
      });

      // MAIN OVERRIDE: set applyFilters on the EntityFilter instance
      window._entityFilter.applyFilters = function () {
        try {
          window._smartviewFilterApplySeq = (Number(window._smartviewFilterApplySeq) || 0) + 1;
          const filters = stripSmartviewFilterTransId(smartviewGetEntityFilterArray(this));
          console.debug('EntityFilter.applyFilters ->', filters);
          try { smartviewPersistActiveSavedFilterAsView(this, filters); } catch (e) {}
          try { refreshSmartviewTitlebarViewDropdown(this && this.activeFilterId ? this.activeFilterId : ''); } catch (e) {}

          // find controller (support several possible names)
          const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || (typeof controller !== 'undefined' ? controller : null);

          if (ctrl) {
            const hasFilters = Array.isArray(filters) && filters.length > 0;
            // attach structured filters
            ctrl.filters = hasFilters ? filters : [];
            // Still send filters to AxList, but also filter the returned rows locally.
            // Some ADS responses can ignore props.filters while still returning success.
            ctrl.forceClientFiltering = hasFilters;
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
            // controller not ready Ã¢â‚¬â€ stash payload for controller init to pick up
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

  // const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
      //   : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
      //   : null;
      
      const scopes = [parent, window, window.top];

      const caller = scopes.find(
          w => w && typeof w.GetDataFromAxList === 'function'
      );            if (caller && typeof caller.GetDataFromAxList === 'function') {
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
          try { closeSmartviewFilterModal(); } catch (e) { /* ignore */ }
          try {
            this.activeFilterId = '';
            this.activeFilterName = '';
          } catch (e) {}

        } catch (error) {
          console.error('entityFilter.applyFilters error', error);
        }
      };

      // Wire Clear button (clear both EntityFilter saved state and SmartView controller state)
      $('#clearFilterBtn').off('click').on('click.smartviewClear', function () {
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
          closeSmartviewFilterModal();
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
        // Position the filter panel as a toolbar dropdown anchored to the Filter button.
        try { smartviewShowFilterDropdown(); } catch (e) {}
      };

      // Restore old logic: if metadata is cached for this ADS, open filters immediately.
      if (controller.lastAdsMeta &&
          Array.isArray(controller.lastAdsMeta) &&
          controller.lastAdsMeta.length) {
        showModalWithFilter();
        return;
      }

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

  /* --------------------------
    SmartView Select Fields (Utilities)
    -------------------------- */

  window._smartviewFieldSelectionState = window._smartviewFieldSelectionState || {
    fields: [],
    selected: new Set(),
    query: ''
  };

  function getSmartviewControllerInstance() {
    return window.smartTableController || window._smartviewController || window._smartviewTableController || null;
  }

  function addSmartviewFieldSelectionStyles() {
    if (document.getElementById('sv-select-fields-styles')) return;
    const style = document.createElement('style');
    style.id = 'sv-select-fields-styles';
    style.textContent = `
      #smartviewFieldsModal .sv-fields-toolbar {
        display: flex;
        gap: 10px;
        align-items: center;
        margin-bottom: 12px;
        flex-wrap: wrap;
      }
      #smartviewFieldsModal .sv-fields-search {
        flex: 1;
        min-width: 220px;
      }
      #smartviewFieldsModal .sv-fields-list {
        max-height: 420px;
        overflow: auto;
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        background: #fff;
      }
      #smartviewFieldsModal .sv-field-row {
        display: flex;
        align-items: flex-start;
        gap: 10px;
        padding: 10px 12px;
        border-bottom: 1px solid #f1f1f1;
      }
      #smartviewFieldsModal .sv-field-row:last-child {
        border-bottom: none;
      }
      #smartviewFieldsModal .sv-field-meta {
        min-width: 0;
      }
      #smartviewFieldsModal .sv-field-cap {
        font-weight: 600;
        color: #1f2937;
        line-height: 1.2;
      }
      #smartviewFieldsModal .sv-field-name {
        color: #6b7280;
        font-size: 12px;
        margin-top: 2px;
        line-height: 1.2;
      }
      #smartviewFieldsModal .sv-fields-empty {
        text-align: center;
        color: #6b7280;
        padding: 24px 12px;
        font-size: 13px;
      }
    `;
    document.head.appendChild(style);
  }

  function ensureSmartviewFieldsModal() {
    if (document.getElementById('smartviewFieldsModal')) return;
    addSmartviewFieldSelectionStyles();

    const html = `
    <div id="smartviewFieldsModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Select Fields</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div class="sv-fields-toolbar">
              <input id="smartviewFieldsSearch" type="text" class="form-control form-control-sm sv-fields-search" placeholder="Search fields...">
              <label class="form-check form-check-sm m-0 d-flex align-items-center gap-2">
                <input id="smartviewFieldsSelectAll" class="form-check-input" type="checkbox">
                <span class="form-check-label">Select all</span>
              </label>
            </div>
            <div id="smartviewFieldsList" class="sv-fields-list"></div>
          </div>
          <div class="modal-footer">
            <button type="button" id="smartviewFieldsReset" class="btn btn-outline-secondary btn-sm">Reset</button>
            <button type="button" id="smartviewFieldsApply" class="btn btn-primary btn-sm">Apply</button>
          </div>
        </div>
      </div>
    </div>
    `;

    const wrap = document.createElement('div');
    wrap.innerHTML = html;
    document.body.appendChild(wrap);

    const search = document.getElementById('smartviewFieldsSearch');
    const selectAll = document.getElementById('smartviewFieldsSelectAll');
    const list = document.getElementById('smartviewFieldsList');
    const applyBtn = document.getElementById('smartviewFieldsApply');
    const resetBtn = document.getElementById('smartviewFieldsReset');

    if (search) {
      search.addEventListener('input', function () {
        window._smartviewFieldSelectionState.query = this.value || '';
        renderSmartviewFieldsList();
      });
    }

    if (selectAll) {
      selectAll.addEventListener('change', function () {
        const state = window._smartviewFieldSelectionState || { fields: [], selected: new Set(), query: '' };
        const q = (state.query || '').toString().trim().toLowerCase();
        const matches = (state.fields || []).filter(f => {
          if (!q) return true;
          return String(f.fldcap || '').toLowerCase().includes(q) || String(f.fldname || '').toLowerCase().includes(q);
        });
        matches.forEach(f => {
          const k = String(f.fldname || '').toLowerCase();
          if (!k) return;
          if (this.checked) state.selected.add(k);
          else state.selected.delete(k);
        });
        renderSmartviewFieldsList();
      });
    }

    if (list) {
      list.addEventListener('change', function (ev) {
        const target = ev.target;
        if (!target || !target.classList || !target.classList.contains('sv-field-checkbox')) return;
        const key = String(target.getAttribute('data-fld') || '').toLowerCase();
        const state = window._smartviewFieldSelectionState || { selected: new Set() };
        if (!key) return;
        if (target.checked) state.selected.add(key);
        else state.selected.delete(key);
        updateSmartviewFieldsSelectAllState();
      });
    }

    if (applyBtn) {
      applyBtn.addEventListener('click', function () {
        applyFields();
      });
    }

    if (resetBtn) {
      resetBtn.addEventListener('click', function () {
        resetFields();
      });
    }
  }

  function smartviewGetSelectableFieldsFromMeta(meta) {
    const arr = Array.isArray(meta) ? meta : [];
    const seen = new Set();
    const out = [];

    arr.forEach(m => {
      const fldname = (m && m.fldname !== undefined && m.fldname !== null) ? String(m.fldname).trim() : '';
      if (!fldname) return;
      const key = fldname.toLowerCase();
      if (!key || seen.has(key)) return;
      if (key === 'transid' || key === 'ftransid') return;
      if (smartviewIsMetaFieldHidden(m)) return;

      seen.add(key);
      out.push({
        fldname: fldname,
        fldcap: (m.fldcaption || m.caption || m.fldcap || formatFieldName(fldname) || fldname).toString()
      });
    });

    return out;
  }

  function smartviewGetCurrentSelectedFieldSet(ctrl, fields) {
    const set = new Set();
    const selectedCols = smartviewGetVisibleSelectedFieldColumns(ctrl);
    selectedCols.forEach(sc => {
      const fld = smartviewSelectExprToFieldName(sc);
      if (!fld) return;
      set.add(String(fld).toLowerCase());
    });

    // If no explicit selection yet, default to "all available fields selected".
    if (!set.size) {
      (fields || []).forEach(f => {
        const k = String(f.fldname || '').toLowerCase();
        if (k) set.add(k);
      });
    }
    return set;
  }

  function updateSmartviewFieldsSelectAllState() {
    const selectAll = document.getElementById('smartviewFieldsSelectAll');
    if (!selectAll) return;
    const state = window._smartviewFieldSelectionState || { fields: [], selected: new Set(), query: '' };
    const q = (state.query || '').toString().trim().toLowerCase();
    const matches = (state.fields || []).filter(f => {
      if (!q) return true;
      return String(f.fldcap || '').toLowerCase().includes(q) || String(f.fldname || '').toLowerCase().includes(q);
    });
    if (!matches.length) {
      selectAll.checked = false;
      selectAll.indeterminate = false;
      return;
    }
    const checkedCount = matches.filter(f => state.selected.has(String(f.fldname || '').toLowerCase())).length;
    selectAll.checked = checkedCount === matches.length;
    selectAll.indeterminate = checkedCount > 0 && checkedCount < matches.length;
  }

  function renderSmartviewFieldsList() {
    const list = document.getElementById('smartviewFieldsList');
    if (!list) return;
    const state = window._smartviewFieldSelectionState || { fields: [], selected: new Set(), query: '' };
    const fields = Array.isArray(state.fields) ? state.fields : [];
    const q = (state.query || '').toString().trim().toLowerCase();

    const filtered = fields.filter(f => {
      if (!q) return true;
      return String(f.fldcap || '').toLowerCase().includes(q) || String(f.fldname || '').toLowerCase().includes(q);
    });

    if (!filtered.length) {
      list.innerHTML = '<div class="sv-fields-empty">No fields found</div>';
      updateSmartviewFieldsSelectAllState();
      return;
    }

    list.innerHTML = filtered.map(f => {
      const fld = String(f.fldname || '');
      const key = fld.toLowerCase();
      const checked = state.selected.has(key) ? 'checked' : '';
      return `
        <label class="sv-field-row" for="sv_fld_${escapeHtml(key)}">
          <input id="sv_fld_${escapeHtml(key)}" type="checkbox" class="form-check-input sv-field-checkbox" data-fld="${escapeHtml(fld)}" ${checked}>
          <span class="sv-field-meta">
            <span class="sv-field-cap">${escapeHtml(f.fldcap || fld)}</span>
            <span class="sv-field-name">${escapeHtml(fld)}</span>
          </span>
        </label>
      `;
    }).join('');

    updateSmartviewFieldsSelectAllState();
  }

  function closeSmartviewFieldsModal() {
    try {
      const el = document.getElementById('smartviewFieldsModal');
      if (!el) return;
      if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
        const instance = bootstrap.Modal.getInstance(el);
        if (instance) instance.hide();
        else el.classList.remove('show');
      } else if (window.jQuery && typeof $('#smartviewFieldsModal').modal === 'function') {
        $('#smartviewFieldsModal').modal('hide');
      } else {
        el.style.display = 'none';
        el.classList.remove('show');
      }
    } catch (e) {}
  }

  function openFieldSelection() {
    try {
      const ctrl = getSmartviewControllerInstance();
      if (!ctrl) {
        alert('Please select an ADS first.');
        return false;
      }

      const openWithMeta = (meta) => {
        const fields = smartviewGetSelectableFieldsFromMeta(meta || []);
        if (!fields.length) {
          alert('No fields available for this ADS.');
          return false;
        }

        ensureSmartviewFieldsModal();
        window._smartviewFieldSelectionState = {
          fields: fields,
          selected: smartviewGetCurrentSelectedFieldSet(ctrl, fields),
          query: ''
        };

        const search = document.getElementById('smartviewFieldsSearch');
        if (search) search.value = '';
        renderSmartviewFieldsList();

        const modalEl = document.getElementById('smartviewFieldsModal');
        if (!modalEl) return false;
        if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
          const instance = bootstrap.Modal.getOrCreateInstance
            ? bootstrap.Modal.getOrCreateInstance(modalEl)
            : new bootstrap.Modal(modalEl);
          instance.show();
        } else if (window.jQuery && typeof $('#smartviewFieldsModal').modal === 'function') {
          $('#smartviewFieldsModal').modal('show');
        } else {
          modalEl.style.display = 'block';
          modalEl.classList.add('show');
        }
        return false;
      };

      if (typeof ctrl.ensureAdsMetadata === 'function') {
        ctrl.ensureAdsMetadata(function (err, meta) {
          openWithMeta((Array.isArray(meta) && meta.length) ? meta : ((window._entity && window._entity.metaData) || []));
        });
      } else {
        openWithMeta((window._entity && window._entity.metaData) || []);
      }
    } catch (e) {
      console.error('openFieldSelection failed', e);
    }
    return false;
  }

  function applyFields() {
    try {
      const ctrl = getSmartviewControllerInstance();
      if (!ctrl) return false;

      const state = window._smartviewFieldSelectionState || { fields: [], selected: new Set() };
      const selected = (state.fields || [])
        .filter(f => state.selected.has(String(f.fldname || '').toLowerCase()))
        .map(f => String(f.fldname || '').trim())
        .filter(Boolean);

      if (!selected.length) {
        alert('Please select at least one field.');
        return false;
      }

      ctrl._smartviewSelectedFieldColumns = selected.slice();
      if (Array.isArray(ctrl.groupby_columns) && ctrl.groupby_columns.length) {
        const meta = (Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length)
          ? ctrl.lastAdsMeta
          : (window._entity && Array.isArray(window._entity.metaData) ? window._entity.metaData : []);
        if (typeof smartviewApplyGroupbySelection === 'function') {
          smartviewApplyGroupbySelection(ctrl, meta, ctrl.groupby_columns);
        } else {
          ctrl.select_columns = selected.slice();
        }
      } else {
        ctrl.select_columns = selected.slice();
        ctrl.aggregations = {};
      }
      ctrl.forceClientFiltering = false;
      ctrl._filteredCache = null;
      window._smartviewFullData = null;
      if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
      if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
      closeSmartviewFieldsModal();
    } catch (e) {
      console.error('applyFields failed', e);
    }
    return false;
  }

  function resetFields() {
    try {
      const ctrl = getSmartviewControllerInstance();
      if (!ctrl) return false;

      ctrl._smartviewSelectedFieldColumns = [];
      if (Array.isArray(ctrl.groupby_columns) && ctrl.groupby_columns.length) {
        const meta = (Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length)
          ? ctrl.lastAdsMeta
          : (window._entity && Array.isArray(window._entity.metaData) ? window._entity.metaData : []);
        if (typeof smartviewApplyGroupbySelection === 'function') {
          smartviewApplyGroupbySelection(ctrl, meta, ctrl.groupby_columns);
        } else {
          ctrl.select_columns = [];
          ctrl.aggregations = {};
        }
      } else {
        ctrl.select_columns = [];
        ctrl.aggregations = {};
      }
      ctrl.forceClientFiltering = false;
      ctrl._filteredCache = null;
      window._smartviewFullData = null;
      if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
      if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
      closeSmartviewFieldsModal();
    } catch (e) {
      console.error('resetFields failed', e);
    }
    return false;
  }

  function smartviewIsElementVisible(el) {
    if (!el) return false;
    if (el.style && el.style.display === 'none') return false;
    try {
      return window.getComputedStyle(el).display !== 'none';
    } catch (e) {
      return true;
    }
  }

  function smartviewGetElementPreferredVisibility(el) {
    if (!el) return false;
    const raw = String(el.getAttribute('data-sv-user-visible') || '').trim().toLowerCase();
    if (raw === 'true') return true;
    if (raw === 'false') return false;
    return smartviewIsElementVisible(el);
  }

  function smartviewElementHasData(el) {
    if (!el) return false;
    const raw = String(el.getAttribute('data-sv-has-data') || '').trim().toLowerCase();
    if (!raw) return true;
    return raw !== 'false';
  }

  function smartviewApplyElementVisibility(el) {
    if (!el) return;
    const shouldShow = smartviewGetElementPreferredVisibility(el) && smartviewElementHasData(el);
    if (shouldShow) el.style.removeProperty('display');
    else el.style.display = 'none';
  }

  function smartviewSetElementHasData(el, hasData) {
    if (!el) return;
    el.setAttribute('data-sv-has-data', hasData ? 'true' : 'false');
    smartviewApplyElementVisibility(el);
    try { smartviewSyncExpandedTableWrapperClass(); } catch (e) {}
    try {
      smartviewRequestCarouselRefresh(0);
      smartviewRequestCarouselRefresh(160);
    } catch (e) {}
  }

  function smartviewSetElementVisible(el, visible) {
    if (!el) return;
    el.setAttribute('data-sv-user-visible', visible ? 'true' : 'false');
    smartviewApplyElementVisibility(el);
    try { smartviewSyncExpandedTableWrapperClass(); } catch (e) {}
    try {
      smartviewRequestCarouselRefresh(0);
      smartviewRequestCarouselRefresh(160);
    } catch (e) {}
  }

  function smartviewRefreshOptionsMenuLabels() {
    try {
      const kpiWrap = document.querySelector('.Sales-data-content-wrapper');
      const chartsWrap = document.querySelector('.Sales-data-charts-wrapper');
      const kpiLabel = document.getElementById('smartviewOptionToggleKpiLabel');
      const chartsLabel = document.getElementById('smartviewOptionToggleChartsLabel');
      const hasExpandedRows = smartviewHasExpandedEditRows();

      // Keep the body flag honest so collapse always restores the Options menu items.
      if (!hasExpandedRows) {
        document.body.classList.remove('smartview-table-focus-mode');
      }

      const toggleMenuItemVisibility = function (labelEl, shouldShow) {
        if (!labelEl) return;
        const item = labelEl.closest('.menu-item') || labelEl.closest('.menu-link') || labelEl.parentElement;
        if (item) item.style.display = shouldShow ? '' : 'none';
      };

      if (kpiLabel) {
        kpiLabel.textContent = smartviewElementHasData(kpiWrap)
          ? (smartviewGetElementPreferredVisibility(kpiWrap) ? 'Hide KPI' : 'Unhide KPI')
          : 'No KPI';
      }
      if (chartsLabel) {
        chartsLabel.textContent = smartviewElementHasData(chartsWrap)
          ? (smartviewGetElementPreferredVisibility(chartsWrap) ? 'Hide charts' : 'Unhide charts')
          : 'No charts';
      }

      toggleMenuItemVisibility(kpiLabel, !hasExpandedRows);
      toggleMenuItemVisibility(chartsLabel, !hasExpandedRows);
      smartviewRefreshViewMenuState();
    } catch (e) {}
  }

  function toggleSmartviewKpiVisibility() {
    try {
      const wrap = document.querySelector('.Sales-data-content-wrapper');
      if (!wrap) return false;
      if (!smartviewElementHasData(wrap)) {
        smartviewRefreshOptionsMenuLabels();
        return false;
      }
      smartviewSetElementVisible(wrap, !smartviewGetElementPreferredVisibility(wrap));
      smartviewRefreshOptionsMenuLabels();
    } catch (e) {
      console.error('toggleSmartviewKpiVisibility failed', e);
    }
    return false;
  }

  function toggleSmartviewChartsVisibility() {
    try {
      const wrap = document.querySelector('.Sales-data-charts-wrapper');
      if (!wrap) return false;
      if (!smartviewElementHasData(wrap)) {
        smartviewRefreshOptionsMenuLabels();
        return false;
      }
      smartviewSetElementVisible(wrap, !smartviewGetElementPreferredVisibility(wrap));
      smartviewRefreshOptionsMenuLabels();
    } catch (e) {
      console.error('toggleSmartviewChartsVisibility failed', e);
    }
    return false;
  }

  function smartviewGetCurrentAdsForOptions() {
    const ctrl = (typeof getSmartviewControllerInstance === 'function')
      ? getSmartviewControllerInstance()
      : (window.smartTableController || window._smartviewController || window._smartviewTableController || null);
    const fromCtrl = (ctrl && ctrl.adsName) ? String(ctrl.adsName).trim() : '';
    const fromEntity = (window._entity && window._entity.adsName) ? String(window._entity.adsName).trim() : '';
    const fromQuery = (typeof getQueryParam === 'function')
      ? (getQueryParam('ads') || getQueryParam('adsName') || getQueryParam('adsname') || '').toString().trim()
      : '';
    return fromCtrl || fromEntity || fromQuery || '';
  }

  function openSmartviewConfigureTstruct() {
    try {
      const ads = smartviewGetCurrentAdsForOptions();
      if (!ads) {
        alert('ADS name is not available.');
        return false;
      }

      const qp = new URLSearchParams();
      qp.set('transid', 'a__sl');
      qp.set('act', 'load');
      qp.set('adsname', ads);
      // Keep URL relative to /aspx to avoid wrong root resolution like /aspx/...
      const url = `tstruct.aspx?${qp.toString()}`;

      try {
        if (window.parent && typeof window.parent.createPopup === 'function') {
          window.parent.createPopup(url, true, () => {}, () => {});
        } else if (window.parent && typeof window.parent.PopupManager.openForm === 'function') {
          let popupurl = '../aspx/' + url + '&dummyload=false?&AxPop=true';               
          window.parent.PopupManager.openForm("", popupurl);
        }  
        else {
          window.open(url, '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=1000,height=760');
        }
      } catch (e) {
        window.open(url, '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=1000,height=760');
      }
    } catch (e) {
      console.error('openSmartviewConfigureTstruct failed', e);
    }
    return false;
  }

  function smartviewGetGroupableFields(meta) {
    const arr = Array.isArray(meta) ? meta : [];
    return arr
      .map(f => ({
        fldname: String((f && f.fldname) || '').trim(),
        fldcap: String((f && (f.fldcap || f.caption)) || (f && f.fldname) || '').trim()
      }))
      .filter(f => !!f.fldname);
  }

  function smartviewApplySingleGroupBy(ctrl, meta, fieldName) {
    if (!ctrl || !fieldName) return false;
    const gb = smartviewBuildGroupbyWithSums(meta || [], [fieldName]);
    ctrl.groupby_columns = Array.isArray(gb.groupby_columns) ? gb.groupby_columns : [];
    ctrl.select_columns = Array.isArray(gb.select_columns) ? gb.select_columns : [];
    ctrl.aggregations = {};
    ctrl.forceClientFiltering = false;
    ctrl._filteredCache = null;
    window._smartviewFullData = null;
    if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
    if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
    return true;
  }

  /* SmartView Group By Modal (Utilities) */

  window._smartviewGroupByState = window._smartviewGroupByState || {
    fields: [],
    selected: new Set(),
    query: ''
  };

  function addSmartviewGroupByStyles() {
    if (document.getElementById('sv-groupby-modal-styles')) return;
    const style = document.createElement('style');
    style.id = 'sv-groupby-modal-styles';
    style.textContent = `
      #smartviewGroupByModal .sv-groupby-toolbar {
        display: flex;
        gap: 10px;
        align-items: center;
        margin-bottom: 12px;
        flex-wrap: wrap;
      }
      #smartviewGroupByModal .sv-groupby-search {
        flex: 1;
        min-width: 220px;
      }
      #smartviewGroupByModal .sv-groupby-list {
        max-height: 420px;
        overflow: auto;
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        background: #fff;
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 0;
        padding: 12px;
      }
      #smartviewGroupByModal .sv-groupby-row {
        display: flex;
        align-items: flex-start;
        gap: 10px;
        padding: 8px;
        border: none;
      }
      #smartviewGroupByModal .sv-groupby-row:last-child {
        border-bottom: none;
      }
      #smartviewGroupByModal .sv-groupby-meta {
        min-width: 0;
        flex: 1;
      }
      #smartviewGroupByModal .sv-groupby-cap {
        font-weight: 500;
        color: #1f2937;
        line-height: 1.3;
        font-size: 13px;
      }
      #smartviewGroupByModal .sv-groupby-name {
        color: #6b7280;
        font-size: 11px;
        margin-top: 2px;
        line-height: 1.2;
      }
      #smartviewGroupByModal .sv-groupby-empty {
        text-align: center;
        color: #6b7280;
        padding: 24px 12px;
        font-size: 13px;
        grid-column: 1 / -1;
      }
      @media (max-width: 1024px) {
        #smartviewGroupByModal .sv-groupby-list {
          grid-template-columns: repeat(2, 1fr);
        }
      }
      @media (max-width: 576px) {
        #smartviewGroupByModal .sv-groupby-list {
          grid-template-columns: 1fr;
        }
      }
    `;
    document.head.appendChild(style);
  }

  function ensureSmartviewGroupByModal() {
    if (document.getElementById('smartviewGroupByModal')) return;
    addSmartviewGroupByStyles();

    const html = `
    <div id="smartviewGroupByModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Group By</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div class="sv-groupby-toolbar">
              <input id="smartviewGroupBySearch" type="text" class="form-control form-control-sm sv-groupby-search" placeholder="Search fields...">
            </div>
            <div id="smartviewGroupByList" class="sv-groupby-list"></div>
          </div>
          <div class="modal-footer">
            <button type="button" id="smartviewGroupByClear" class="btn btn-outline-secondary btn-sm">Clear</button>
            <button type="button" id="smartviewGroupByCancel" class="btn btn-outline-secondary btn-sm" data-bs-dismiss="modal">Cancel</button>
            <button type="button" id="smartviewGroupByApply" class="btn btn-primary btn-sm">Apply</button>
          </div>
        </div>
      </div>
    </div>
    `;

    const wrap = document.createElement('div');
    wrap.innerHTML = html;
    document.body.appendChild(wrap);

    const search = document.getElementById('smartviewGroupBySearch');
    const list = document.getElementById('smartviewGroupByList');
    const applyBtn = document.getElementById('smartviewGroupByApply');
    const clearBtn = document.getElementById('smartviewGroupByClear');

    if (search) {
      search.addEventListener('input', function () {
        window._smartviewGroupByState.query = this.value || '';
        renderSmartviewGroupByList();
      });
    }

    if (list) {
      list.addEventListener('change', function (ev) {
        const target = ev.target;
        if (!target || !target.classList || !target.classList.contains('sv-groupby-checkbox')) return;
        const key = String(target.getAttribute('data-fld') || '').toLowerCase();
        const state = window._smartviewGroupByState || { selected: new Set() };
        if (!key) return;
        if (target.checked) state.selected.add(key);
        else state.selected.delete(key);
      });
    }

    if (applyBtn) {
      applyBtn.addEventListener('click', function () {
        applySmartviewGroupBy();
      });
    }

    if (clearBtn) {
      clearBtn.addEventListener('click', function () {
        clearSmartviewGroupBy();
      });
    }
  }

  function smartviewGetGroupableFieldsFromMeta(meta) {
    const arr = Array.isArray(meta) ? meta : [];
    const seen = new Set();
    const out = [];

    arr.forEach(m => {
      const fldname = (m && m.fldname !== undefined && m.fldname !== null) ? String(m.fldname).trim() : '';
      if (!fldname) return;
      const key = fldname.toLowerCase();
      if (!key || seen.has(key)) return;
      if (key === 'transid' || key === 'ftransid') return;
      if (smartviewIsMetaFieldHidden(m)) return;

      seen.add(key);
      out.push({
        fldname: fldname,
        fldcap: (m.fldcaption || m.caption || m.fldcap || formatFieldName(fldname) || fldname).toString()
      });
    });

    return out;
  }

  function smartviewGetCurrentGroupByFieldSet(ctrl, fields) {
    const set = new Set();
    const currentGroups = smartviewNormalizeGroupbyFields(ctrl.groupby_columns || []);
    currentGroups.forEach(groupField => {
      const k = String(groupField || '').toLowerCase().trim();
      if (k) set.add(k);
    });
    return set;
  }

  function updateSmartviewGroupBySelectAllState() {
    const selectAll = document.getElementById('smartviewGroupBySelectAll');
    if (!selectAll) return;
    const state = window._smartviewGroupByState || { fields: [], selected: new Set(), query: '' };
    const q = (state.query || '').toString().trim().toLowerCase();
    const matches = (state.fields || []).filter(f => {
      if (!q) return true;
      return String(f.fldcap || '').toLowerCase().includes(q) || String(f.fldname || '').toLowerCase().includes(q);
    });
    if (!matches.length) {
      selectAll.checked = false;
      selectAll.indeterminate = false;
      return;
    }
    const checkedCount = matches.filter(f => state.selected.has(String(f.fldname || '').toLowerCase())).length;
    selectAll.checked = checkedCount === matches.length;
    selectAll.indeterminate = checkedCount > 0 && checkedCount < matches.length;
  }

  function renderSmartviewGroupByList() {
    const list = document.getElementById('smartviewGroupByList');
    if (!list) return;
    const state = window._smartviewGroupByState || { fields: [], selected: new Set(), query: '' };
    const fields = Array.isArray(state.fields) ? state.fields : [];
    const q = (state.query || '').toString().trim().toLowerCase();

    const filtered = fields.filter(f => {
      if (!q) return true;
      return String(f.fldcap || '').toLowerCase().includes(q) || String(f.fldname || '').toLowerCase().includes(q);
    });

    if (!filtered.length) {
      list.innerHTML = '<div class="sv-groupby-empty">No fields found</div>';
      return;
    }

    list.innerHTML = filtered.map(f => {
      const fld = String(f.fldname || '');
      const key = fld.toLowerCase();
      const checked = state.selected.has(key) ? 'checked' : '';
      return `
        <label class="sv-groupby-row" for="sv_grp_${escapeHtml(key)}">
          <input id="sv_grp_${escapeHtml(key)}" type="checkbox" class="form-check-input sv-groupby-checkbox" data-fld="${escapeHtml(fld)}" ${checked}>
          <span class="sv-groupby-meta">
            <span class="sv-groupby-cap">${escapeHtml(f.fldcap || fld)}</span>
            <span class="sv-groupby-name">${escapeHtml(fld)}</span>
          </span>
        </label>
      `;
    }).join('');
  }

  function closeSmartviewGroupByModal() {
    try {
      const el = document.getElementById('smartviewGroupByModal');
      if (!el) return;
      if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
        const instance = bootstrap.Modal.getInstance(el);
        if (instance) instance.hide();
      }
    } catch (e) {
      console.error('closeSmartviewGroupByModal error:', e);
    }
  }

  function applySmartviewGroupBy() {
    try {
      const ctrl = getSmartviewControllerInstance();
      if (!ctrl) return;
      
      const state = window._smartviewGroupByState || { selected: new Set() };
      if (!state.selected.size) {
        alert('Please select at least one field to group by.');
        return;
      }

      const cachedMeta = (Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length)
        ? ctrl.lastAdsMeta
        : ((window._entity && Array.isArray(window._entity.metaData)) ? window._entity.metaData : []);

      const allFields = smartviewGetGroupableFieldsFromMeta(cachedMeta || []);
      const selectedFields = Array.from(state.selected)
        .map(key => allFields.find(f => f.fldname.toLowerCase() === key))
        .filter(f => !!f)
        .map(f => f.fldname);

      if (selectedFields.length) {
        // Use the standard groupby builder function
        if (typeof smartviewBuildGroupbyWithSums === 'function') {
          const gb = smartviewBuildGroupbyWithSums(cachedMeta || [], selectedFields);
          ctrl.groupby_columns = Array.isArray(gb.groupby_columns) ? gb.groupby_columns : [];
          ctrl.select_columns = Array.isArray(gb.select_columns) ? gb.select_columns : [];
          ctrl.aggregations = {};
          ctrl.forceClientFiltering = false;
          ctrl._filteredCache = null;
          window._smartviewFullData = null;
        } else {
          ctrl.groupby_columns = selectedFields;
        }
        
        if (typeof ctrl.resetPaging === 'function') ctrl.resetPaging();
        if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
      }

      closeSmartviewGroupByModal();
    } catch (e) {
      console.error('applySmartviewGroupBy failed:', e);
    }
  }

  function clearSmartviewGroupBy() {
    try {
      const ctrl = getSmartviewControllerInstance();
      if (!ctrl) return;

      if (typeof ctrl.clearGroupByColumns === 'function') {
        ctrl.clearGroupByColumns();
      } else {
        ctrl.groupby_columns = [];
        ctrl.aggregations = {};
        if (typeof ctrl.loadNextPage === 'function') ctrl.loadNextPage();
      }

      window._smartviewGroupByState.selected.clear();
      renderSmartviewGroupByList();
    } catch (e) {
      console.error('clearSmartviewGroupBy failed:', e);
    }
  }

  function openSmartviewGroupByModal(meta) {
    try {
      ensureSmartviewGroupByModal();
      
      const fields = smartviewGetGroupableFieldsFromMeta(meta || []);
      if (!fields.length) {
        alert('No fields available for grouping.');
        return false;
      }

      const ctrl = getSmartviewControllerInstance();
      const currentGroups = smartviewGetCurrentGroupByFieldSet(ctrl, fields);

      window._smartviewGroupByState.fields = fields;
      window._smartviewGroupByState.query = '';
      window._smartviewGroupByState.selected = new Set(currentGroups);

      renderSmartviewGroupByList();

      const modal = document.getElementById('smartviewGroupByModal');
      if (modal && typeof bootstrap !== 'undefined' && bootstrap.Modal) {
        new bootstrap.Modal(modal).show();
      }
    } catch (e) {
      console.error('openSmartviewGroupByModal failed:', e);
    }
  }

  function openSmartviewGroupOption() {
    try {
      const ctrl = (typeof getSmartviewControllerInstance === 'function')
        ? getSmartviewControllerInstance()
        : (window.smartTableController || window._smartviewController || window._smartviewTableController || null);
      if (!ctrl) {
        alert('Please select an ADS first.');
        return false;
      }

      const cachedMeta = (Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length)
        ? ctrl.lastAdsMeta
        : ((window._entity && Array.isArray(window._entity.metaData)) ? window._entity.metaData : []);

      if (cachedMeta.length) {
        openSmartviewGroupByModal(cachedMeta);
      } else if (typeof ctrl.ensureAdsMetadata === 'function') {
        ctrl.ensureAdsMetadata(function (err, meta) {
          const effectiveMeta = (Array.isArray(meta) && meta.length) ? meta : ((window._entity && window._entity.metaData) || []);
          openSmartviewGroupByModal(effectiveMeta);
        });
      } else {
        openSmartviewGroupByModal([]);
      }
    } catch (e) {
      console.error('openSmartviewGroupOption failed', e);
    }
    return false;
  }

  /**
   * Creates the Filter Modal exactly in the shape Entity-Filter.js expects.
   * Safe to call multiple times (will create only once).
   */
  function createSmartviewFilterModal() {
    // Do not recreate if already exists
    // SmartView now renders filters inline; avoid creating a modal (duplicate IDs break EntityFilter).
    if (document.getElementById('smartviewInlineFilterPanel') || document.getElementById('dvModalFilter')) return;
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
            <div class="sv-filter-footer-left d-none" aria-hidden="true">
              <!-- Hidden compatibility controls for Entity-Filter internals -->
              <input class="form-check-input" type="checkbox" id="filterGroupCheckbox" style="display:none;">
              <input type="text"
                    id="filterGroupName"
                    class="form-control form-control-sm"
                    placeholder="Filter name"
                    style="display:none;"
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

    // Keep compatibility fields disabled/hidden (SmartView saves views from title-bar dropdown only).
    try {
      const cb = document.getElementById('filterGroupCheckbox');
      const name = document.getElementById('filterGroupName');
      if (cb) cb.checked = false;
      if (name) { name.disabled = true; name.value = ''; }
    } catch (e) {}
  }

  // Some pages may already have a #filterModal but without the exact Entity-Filter.js IDs.
  // This keeps SmartView compatible without touching product files.
  function ensureSmartviewFilterCompatDom() {
    const host =
      document.getElementById('smartviewInlineFilterPanel') ||
      document.getElementById('filterModal') ||
      null;
    if (!host) return;

    // #dvModalFilter is where EntityFilter injects rows
    if (!document.getElementById('dvModalFilter')) {
      const body = host.querySelector('.card-body') || host.querySelector('.modal-body') || host;
      const dv = document.createElement('div');
      dv.id = 'dvModalFilter';
      dv.className = 'row g-3';
      body.appendChild(dv);
    }

    // Ensure hidden compatibility inputs exist so Entity-Filter internals don't crash.
    let hiddenHost =
      (host.id === 'smartviewInlineFilterPanel')
        ? (host.querySelector('.d-none[aria-hidden=\"true\"]') || host)
        : (host.querySelector('.modal-footer') || host);

    if (!document.getElementById('filterGroupCheckbox')) {
      const cb = document.createElement('input');
      cb.className = 'form-check-input';
      cb.type = 'checkbox';
      cb.id = 'filterGroupCheckbox';
      cb.style.display = 'none';
      hiddenHost.appendChild(cb);
    }

    if (!document.getElementById('filterGroupName')) {
      const inp = document.createElement('input');
      inp.type = 'text';
      inp.id = 'filterGroupName';
      inp.className = 'form-control form-control-sm';
      inp.placeholder = 'Filter name';
      inp.style.display = 'none';
      inp.disabled = true;
      hiddenHost.appendChild(inp);
    }

    try {
      const cb = document.getElementById('filterGroupCheckbox');
      const name = document.getElementById('filterGroupName');
      if (cb) cb.checked = false;
      if (name) { name.disabled = true; name.value = ''; }
    } catch (e) {}
  }

  function getSmartviewNamedFilterViews() {
    // SmartView views are handled from the title-bar dropdown using localStorage snapshots.
    return [];
  }

  function refreshSmartviewFilterViewDropdown(preferredKey) {
    // no-op: modal-level saved-views dropdown removed by design.
  }

  function bindSmartviewFilterViewDropdown() {
    // no-op: modal-level saved-views dropdown removed by design.
  }

  function ensureSmartviewFilterViewDropdown() {
    // no-op: modal-level saved-views dropdown removed by design.
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
      return `tstruct.aspx?transid=${parts[0]}&act=open`;
    }
    return '';
  }

  function smartviewGetAspxBaseUrl() {
    const scopes = [];
    try { if (window.top && window.top !== window) scopes.push(window.top); } catch (e) {}
    try { if (window.parent && window.parent !== window) scopes.push(window.parent); } catch (e) {}
    try { scopes.push(window); } catch (e) {}

    for (let i = 0; i < scopes.length; i++) {
      const scope = scopes[i];
      try {
        const href = String(scope && scope.location && scope.location.href ? scope.location.href : '');
        if (!href) continue;
        const idx = href.toLowerCase().indexOf('/aspx/');
        if (idx > -1) return href.substring(0, idx + 6).replace(/\/+$/, '');
      } catch (e) {}
    }

    try {
      if (window.location && window.location.origin) return `${window.location.origin}/aspx`;
    } catch (e) {}

    return '/aspx';
  }

  function smartviewBuildTstructUrl(transId, queryString, mode) {
    const safeTransId = encodeURIComponent(String(transId || '').trim());
    if (!safeTransId) return '';

    const base = smartviewGetAspxBaseUrl();
    const qs = String(queryString || '').replace(/^\?+/, '').trim();
    const isLoad = String(mode || '').trim().toLowerCase() === 'load';
    const act = isLoad ? 'load' : 'open';

    return qs
      ? `${base}/tstruct.aspx?transid=${safeTransId}&${qs}`
      : `${base}/tstruct.aspx?transid=${safeTransId}&act=${act}`;
  }

  /* ---------- Open Link in Popup ---------- */
  function openLinkInPopup(input, returnUrl = false, options = {}) {
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
      
      // Extract _mode parameter if present in descriptor
      let modeFromDescriptor = '';
      paramPairs.forEach(([k, v]) => {
        if (k && k.trim() === '_mode') {
          modeFromDescriptor = (v || '').trim().toLowerCase();
        } else if (k) {
          qp.append(k.trim(), (v || '').trim());
        }
      });

      let url = '';

      // Use mode from descriptor, fallback to options parameter
      let tstructMode = modeFromDescriptor || String((options && options.tstructMode) || '').trim().toLowerCase();
      
      if (type === 'i') url = `ivtoivload.aspx?ivname=${struct}&${qp.toString()}`;
      else if (type === 't') {
        const transId = (struct || '').toString().trim();
        if (!transId) return;

        if (tstructMode === 'load') {
          // Load existing tstruct in load mode.
          qp.delete('AxPop');
          qp.delete('tstname');
          if (!qp.has('docid') && qp.has('recordid')) qp.set('docid', qp.get('recordid') || '');
          qp.delete('recordid');
          qp.set('act', 'load');
          const qsLoad = qp.toString();
          console.log('[smartviewTable] Opening tstruct in LOAD mode:', { transId, qsLoad });
          url = smartviewBuildTstructUrl(transId, qsLoad, 'load');
        } else {
          if (!qp.has('act')) qp.set('act', 'open');
          if (!qp.has('AxPop')) qp.set('AxPop', 'true');
          const qs = qp.toString();
          url = smartviewBuildTstructUrl(transId, qs, 'open');
        }
      } else if (type === 'h') {
        if ((linkStr[0] + linkStr[1]).toLowerCase() === "hp" && (/^\d+$/.test(struct.slice(1)))) 
          url = `htmlpages.aspx?load=${struct.slice(1)}&${qp.toString()}`;
        else url = `htmlpages.aspx?loadcaption=${struct}&${qp.toString()}`;
      } else if (type === 'l') url = `entity.aspx?tstid=${struct}&${qp.toString()}`;
      else if (type === 'd') url = `entityform.aspx?tstid=${struct}&${qp.toString()}`;

      // Open the popup
      if (url) {
        if (returnUrl) {
          return url; // Ã¢Å“â€¦ IMPORTANT
        }
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

  function submitSmartviewToolbar() {
    try {
      return handleSubmitClick();
    } catch (e) {
      console.error('submitSmartviewToolbar failed', e);
      return false;
    }
  }

  function smartviewEnsureXlsxLoaded(callback) {
    const done = function (ok) {
      try { if (typeof callback === "function") callback(!!ok); } catch (e) {}
    };

    if (typeof XLSX !== "undefined" && XLSX && XLSX.utils) {
      done(true);
      return true;
    }

    window._smartviewXlsxCallbacks = window._smartviewXlsxCallbacks || [];
    if (typeof callback === "function") window._smartviewXlsxCallbacks.push(callback);
    if (window._smartviewXlsxLoading) return false;
    window._smartviewXlsxLoading = true;

    const flush = function (ok) {
      window._smartviewXlsxLoading = false;
      const callbacks = Array.isArray(window._smartviewXlsxCallbacks) ? window._smartviewXlsxCallbacks.slice() : [];
      window._smartviewXlsxCallbacks = [];
      callbacks.forEach(function (cb) {
        try { cb(!!ok); } catch (e) {}
      });
      if (!ok && typeof callback !== "function") alert("Excel library is not available on this page.");
    };

    try {
      const script = document.createElement("script");
      script.src = smartviewToAssetUrl("/Js/xlsx.full.min.js", smartviewGetAssetBaseUrl());
      script.onload = function () { flush(typeof XLSX !== "undefined" && XLSX && XLSX.utils); };
      script.onerror = function () {
        const fallback = document.createElement("script");
        fallback.src = smartviewToAssetUrl("/Js/thirdparty/excelImport/xlsx.full.min.js", smartviewGetAssetBaseUrl());
        fallback.onload = function () { flush(typeof XLSX !== "undefined" && XLSX && XLSX.utils); };
        fallback.onerror = function () { flush(false); };
        document.head.appendChild(fallback);
      };
      document.head.appendChild(script);
    } catch (e) {
      flush(false);
    }

    return false;
  }

  function smartviewIsDateMetaField(meta) {
    if (!meta || typeof meta !== "object") return false;
    const ft = String(meta.fdatatype || "").trim().toLowerCase();
    const ct = String(meta.cdatatype || "").trim().toLowerCase();
    const fld = String(meta.fldname || "").trim().toLowerCase();
    return ft === "d" || ct === "date" || fld === "createdon" || fld === "modifiedon";
  }

  function smartviewGetTemplateFileBaseName() {
    const titleEl = document.getElementById("EntityTitle") || document.querySelector(".page-header-title");
    const title = String((titleEl && titleEl.textContent) || "").trim();
    const adsName = String(smartviewGetCurrentAdsForViews() || "").trim();
    const base = title || adsName || "SmartView";
    return base.replace(/[\\/:*?"<>|]+/g, " ").replace(/\s+/g, " ").trim() || "SmartView";
  }

  function smartviewBuildExcelHeaderLabel(baseLabel, fieldName, usedLabels) {
    const fallback = String(baseLabel || fieldName || "Column").trim() || "Column";
    const key = fallback.toLowerCase();
    if (!usedLabels[key]) {
      usedLabels[key] = 1;
      return fallback;
    }
    usedLabels[key] += 1;
    return `${fallback} (${fieldName})`;
  }

  function smartviewBuildExcelTemplateColumns(ctrl) {
    const metaData = Array.isArray(window._entity && window._entity.metaData) ? window._entity.metaData : [];
    const metaMap = {};
    metaData.forEach(function (item) {
      const key = String((item && item.fldname) || "").trim().toLowerCase();
      if (key && !metaMap[key]) metaMap[key] = item;
    });

    const excluded = new Set(["recordid", "transid", "ftransid"]);
    let fields = [];

    try {
      const tableEl = document.querySelector("#table-body_Container table");
      if (tableEl) fields = smartviewGetHeaderFieldOrder(tableEl);
    } catch (e) {}

    if (!fields.length && ctrl) {
      fields = smartviewGetVisibleSelectedFieldColumns(ctrl);
    }

    if (!fields.length) {
      fields = metaData
        .map(function (item) { return String((item && item.fldname) || "").trim(); })
        .filter(Boolean);
    }

    const activeKeyFields = smartviewGetActiveKeyFields(metaData, ctrl);
    activeKeyFields.forEach(function (fieldName) {
      const token = String(fieldName || "").trim().toLowerCase();
      if (!token || excluded.has(token)) return;
      const exists = fields.some(function (candidate) {
        return String(candidate || "").trim().toLowerCase() === token;
      });
      if (!exists) fields.push(fieldName);
    });

    const seen = new Set();
    const usedLabels = {};
    const out = [];

    fields.forEach(function (rawField) {
      const field = String(rawField || "").trim();
      const fieldLower = field.toLowerCase();
      if (!field || excluded.has(fieldLower) || seen.has(fieldLower)) return;
      seen.add(fieldLower);

      const meta = metaMap[fieldLower] || {
        fldname: field,
        fldcap: formatFieldName(field),
        fdatatype: "t",
        cdatatype: "Text"
      };
      const caption = smartviewNormalizeCaption(meta.fldcap, meta.fldname) || formatFieldName(field) || field;
      const label = smartviewBuildExcelHeaderLabel(caption, field, usedLabels);

      out.push({
        field: field,
        fieldLower: fieldLower,
        caption: String(caption || field).trim(),
        label: String(label || field).trim(),
        meta: meta
      });
    });

    return out;
  }

  function smartviewGetExcelTemplateSourceRows(ctrl) {
    if (ctrl && typeof ctrl._getAllRowsData === "function") {
      const rows = ctrl._getAllRowsData();
      if (Array.isArray(rows) && rows.length) return rows;
    }
    if (Array.isArray(window._smartviewFullData) && window._smartviewFullData.length) return window._smartviewFullData;
    if (Array.isArray(window._entity && window._entity.listJson) && window._entity.listJson.length) return window._entity.listJson;
    return [];
  }

  function smartviewPrepareExcelTemplateCellValue(row, column) {
    const meta = (column && column.meta) || {};
    const field = (column && column.field) || "";
    const rawValue = smartviewGetRowFieldValue(row, field);
    if (rawValue === null || rawValue === undefined) return "";
    if (smartviewIsDateMetaField(meta) && rawValue !== "") return formatDateString(rawValue);
    if (typeof rawValue === "string" && smartviewIsNumericMetaField(meta)) return rawValue.replace(/,/g, "").trim();
    return rawValue;
  }

  function smartviewGetUploadCellValue(row, candidates) {
    if (!row || typeof row !== "object") return "";
    const keys = Object.keys(row);
    const list = Array.isArray(candidates) ? candidates : [];
    for (let i = 0; i < list.length; i++) {
      const token = String(list[i] || "").trim();
      if (!token) continue;
      if (Object.prototype.hasOwnProperty.call(row, token)) return row[token];
      const match = keys.find(function (k) {
        return String(k || "").trim().toLowerCase() === token.toLowerCase();
      });
      if (match) return row[match];
    }
    return "";
  }

  function smartviewNormalizeUploadedCellValue(rawValue, meta) {
    if (rawValue === null || rawValue === undefined) return "";

    const checkboxType = String((meta && (meta.cdatatype || meta.fdatatype)) || "").trim().toLowerCase() === "check box";
    if (typeof rawValue === "boolean") {
      if (checkboxType) return rawValue ? "T" : "F";
      return rawValue ? "true" : "false";
    }

    if (rawValue instanceof Date && !isNaN(rawValue)) {
      return smartviewIsDateMetaField(meta) ? formatDateString(rawValue) : rawValue.toISOString();
    }

    let text = String(rawValue).trim();
    if (!text) return "";

    if (checkboxType) {
      return /^(true|t|yes|y|1)$/i.test(text) ? "T" : "F";
    }

    if (smartviewIsDateMetaField(meta)) return formatDateString(text);
    if (smartviewIsNumericMetaField(meta)) return text.replace(/,/g, "");
    return text;
  }

  function smartviewIsUploadedRowEmpty(row) {
    if (!row || typeof row !== "object") return true;
    return Object.keys(row).every(function (key) {
      const value = row[key];
      return value === null || value === undefined || String(value).trim() === "";
    });
  }

  function smartviewResolveExcelDuplicateField(columns, rows) {
    const seen = new Set();
    const out = [];
    const columnSet = new Set((Array.isArray(columns) ? columns : []).map(function (item) {
      return String((item && item.field) || "").trim().toLowerCase();
    }).filter(Boolean));
    const add = function (value) {
      const token = String(value || "").trim().toLowerCase();
      if (!token || seen.has(token) || token === "recordid" || token === "transid" || token === "ftransid" || !columnSet.has(token)) return;
      seen.add(token);
      out.push(token);
    };

    ((window._entity && window._entity.keyFields) || []).forEach(add);
    add(window._entity && window._entity.keyField);
    ["docid", "docno", "documentno", "id", "code", "name", "username"].forEach(add);
    (Array.isArray(columns) ? columns : []).forEach(function (item) { add(item && item.field); });

    const sample = (Array.isArray(rows) && rows.length) ? rows[0] : null;
    for (let i = 0; i < out.length; i++) {
      const key = out[i];
      if (sample ? !!getRowValueCaseInsensitive(sample, key) : true) return key;
    }

    return out.length ? out[0] : "";
  }

  function smartviewBuildFullDataCacheMetaFromController(ctrl) {
    if (!ctrl || typeof ctrl.buildParams !== "function") return null;
    const params = ctrl.buildParams(1);
    params.props = params.props || {};
    params.props.pageno = 1;
    params.props.pagesize = 0;
    return smartviewBuildFullDataCacheKey(params);
  }

  function smartviewNormalizeExcelDuplicateToken(value) {
    return String(value === null || value === undefined ? "" : value).trim().toLowerCase();
  }

  function smartviewBuildExcelRowSignature(row, columns) {
    const parts = (Array.isArray(columns) ? columns : []).map(function (column) {
      const field = (column && column.field) || "";
      const value = field ? getRowValueCaseInsensitive(row, field) : "";
      return smartviewNormalizeExcelDuplicateToken(value);
    });
    return parts.join("\u001f");
  }

  function smartviewBuildExcelCompositeKeyToken(row, keyFields) {
    const fields = Array.isArray(keyFields) ? keyFields : [];
    if (!fields.length) return "";

    const parts = [];
    for (let i = 0; i < fields.length; i++) {
      const fieldName = String(fields[i] || "").trim();
      const token = smartviewNormalizeExcelDuplicateToken(getRowValueCaseInsensitive(row, fieldName));
      if (!fieldName || !token) return "";
      parts.push(fieldName.toLowerCase() + "=" + token);
    }
    return parts.join("\u001f");
  }

  function smartviewFindMissingExcelKeyFields(row, keyFields) {
    return (Array.isArray(keyFields) ? keyFields : []).filter(function (fieldName) {
      return !smartviewNormalizeExcelDuplicateToken(getRowValueCaseInsensitive(row, fieldName));
    });
  }

  function smartviewResolveExcelUploadMatchStrategy(columns, rows, ctrl) {
    const columnSet = new Set((Array.isArray(columns) ? columns : []).map(function (item) {
      return String((item && item.field) || "").trim().toLowerCase();
    }).filter(Boolean));

    const metaData =
      (ctrl && Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length) ? ctrl.lastAdsMeta
      : (Array.isArray(window._entity && window._entity.metaData) ? window._entity.metaData : []);

    const explicitKeyFields = smartviewResolveMetadataKeyFields(metaData);
    if (explicitKeyFields.length) {
      const missingFields = explicitKeyFields.filter(function (fieldName) {
        return !columnSet.has(String(fieldName || "").trim().toLowerCase());
      });
      return {
        mode: missingFields.length ? "metadata-keyfields-missing" : "metadata-keyfields",
        keyFields: missingFields.length ? [] : explicitKeyFields.slice(),
        explicitKeyFields: explicitKeyFields.slice(),
        missingFields: missingFields,
        duplicateField: ""
      };
    }

    const duplicateField = smartviewResolveExcelDuplicateField(columns, rows);
    return {
      mode: duplicateField ? "fallback-key" : "fallback-signature",
      keyFields: duplicateField ? [duplicateField] : [],
      explicitKeyFields: [],
      missingFields: [],
      duplicateField: duplicateField
    };
  }

  function smartviewMergeUploadedExcelRows(existingRows, uploadedRows, columns, matchStrategy) {
    const workingRows = smartviewCloneJsonSafe(Array.isArray(existingRows) ? existingRows : []);
    const incomingRows = Array.isArray(uploadedRows) ? uploadedRows : [];
    const strategy = (matchStrategy && typeof matchStrategy === "object") ? matchStrategy : {};
    const mode = String(strategy.mode || "").trim().toLowerCase();
    const keyFields = Array.isArray(strategy.keyFields) ? strategy.keyFields.slice() : [];
    const duplicateField = String(strategy.duplicateField || "").trim();
    const strictKeyFields = mode === "metadata-keyfields" && keyFields.length > 0;

    const existingKeyTokens = new Set();
    const uploadedKeyTokens = new Set();
    const existingSignatures = new Set();
    const uploadedSignatures = new Set();
    const existingKeyIndexMap = new Map();
    const insertedRows = [];
    const operations = [];
    const stats = {
      inserted: 0,
      updated: 0,
      duplicateSkipped: 0,
      emptySkipped: 0,
      keyMissingSkipped: 0
    };

    workingRows.forEach(function (row, index) {
      const signatureToken = smartviewBuildExcelRowSignature(row, columns);
      if (signatureToken) existingSignatures.add(signatureToken);

      if (strictKeyFields) {
        const keyToken = smartviewBuildExcelCompositeKeyToken(row, keyFields);
        if (keyToken && !existingKeyIndexMap.has(keyToken)) existingKeyIndexMap.set(keyToken, index);
        return;
      }

      if (duplicateField) {
        const keyToken = smartviewNormalizeExcelDuplicateToken(getRowValueCaseInsensitive(row, duplicateField));
        if (keyToken) existingKeyTokens.add(keyToken);
      }
    });

    incomingRows.forEach(function (mappedRow) {
      if (smartviewIsUploadedRowEmpty(mappedRow)) {
        stats.emptySkipped += 1;
        return;
      }

      if (strictKeyFields) {
        const missingFields = smartviewFindMissingExcelKeyFields(mappedRow, keyFields);
        if (missingFields.length) {
          stats.keyMissingSkipped += 1;
          operations.push({
            action: "skipped",
            reason: "missing_key",
            keyFields: keyFields.slice(),
            missingFields: missingFields.slice(),
            row: smartviewCloneJsonSafe(mappedRow)
          });
          return;
        }

        const keyToken = smartviewBuildExcelCompositeKeyToken(mappedRow, keyFields);
        if (!keyToken) {
          stats.keyMissingSkipped += 1;
          return;
        }

        if (uploadedKeyTokens.has(keyToken)) {
          stats.duplicateSkipped += 1;
          operations.push({
            action: "skipped",
            reason: "duplicate_upload_key",
            keyFields: keyFields.slice(),
            keyToken: keyToken,
            row: smartviewCloneJsonSafe(mappedRow)
          });
          return;
        }

        const existingIndex = existingKeyIndexMap.has(keyToken) ? existingKeyIndexMap.get(keyToken) : -1;
        if (existingIndex > -1) {
          const mergedRow = Object.assign({}, workingRows[existingIndex] || {}, mappedRow || {});
          workingRows[existingIndex] = mergedRow;
          stats.updated += 1;
          operations.push({
            action: "update",
            keyFields: keyFields.slice(),
            keyToken: keyToken,
            row: smartviewCloneJsonSafe(mergedRow)
          });
        } else {
          const insertedRow = Object.assign({}, mappedRow || {});
          insertedRows.push(insertedRow);
          stats.inserted += 1;
          operations.push({
            action: "insert",
            keyFields: keyFields.slice(),
            keyToken: keyToken,
            row: smartviewCloneJsonSafe(insertedRow)
          });
        }

        uploadedKeyTokens.add(keyToken);
        return;
      }

      const keyToken = duplicateField
        ? smartviewNormalizeExcelDuplicateToken(getRowValueCaseInsensitive(mappedRow, duplicateField))
        : "";
      const signatureToken = smartviewBuildExcelRowSignature(mappedRow, columns);
      const isDuplicate = (
        (!!keyToken && (existingKeyTokens.has(keyToken) || uploadedKeyTokens.has(keyToken))) ||
        existingSignatures.has(signatureToken) ||
        uploadedSignatures.has(signatureToken)
      );

      if (isDuplicate) {
        stats.duplicateSkipped += 1;
        operations.push({
          action: "skipped",
          reason: "duplicate",
          keyToken: keyToken,
          row: smartviewCloneJsonSafe(mappedRow)
        });
        return;
      }

      if (keyToken) uploadedKeyTokens.add(keyToken);
      uploadedSignatures.add(signatureToken);
      insertedRows.push(Object.assign({}, mappedRow || {}));
      stats.inserted += 1;
      operations.push({
        action: "insert",
        reason: duplicateField ? "fallback_key" : "fallback_signature",
        keyFields: keyFields.slice(),
        keyToken: keyToken,
        row: smartviewCloneJsonSafe(mappedRow)
      });
    });

    return {
      rows: insertedRows.concat(workingRows),
      keyFields: keyFields.slice(),
      mode: mode || (strictKeyFields ? "metadata-keyfields" : "fallback"),
      stats: stats,
      operations: operations
    };
  }

  function smartviewHasPendingStoreEntries() {
    try {
      return Object.keys(window._multiTstructStore || {}).length > 0;
    } catch (e) {
      return false;
    }
  }

  function smartviewGetCurrentFullRowsSnapshot() {
    return smartviewCloneJsonSafe(Array.isArray(window._smartviewFullData) ? window._smartviewFullData : []);
  }

  function smartviewResetOpenFrameDirtyFlags() {
    const frames = document.querySelectorAll("iframe.tstruct-frame");
    Array.prototype.forEach.call(frames || [], function (frame) {
      try {
        if (frame && frame.contentWindow) frame.contentWindow.AxGlobalChange = false;
      } catch (e) {}
    });
  }

  function smartviewRefreshGlobalDirtyFlag() {
    let hasFrameChanges = false;
    const frames = document.querySelectorAll("iframe.tstruct-frame");
    Array.prototype.forEach.call(frames || [], function (frame) {
      try {
        if (frame && frame.contentWindow && frame.contentWindow.AxGlobalChange) hasFrameChanges = true;
      } catch (e) {}
    });

    window.AxGlobalChange = !!(window._smartviewUploadDirty || hasFrameChanges || smartviewHasPendingStoreEntries());
    return window.AxGlobalChange;
  }

  function smartviewMarkCurrentStateCommitted() {
    window._smartviewCommittedFullData = smartviewGetCurrentFullRowsSnapshot();
    window._smartviewCommittedExcelUploadState = smartviewCloneJsonSafe(window._smartviewExcelUploadState);
    window._smartviewUploadDirty = false;
    smartviewRefreshGlobalDirtyFlag();
  }

  function smartviewShouldRefreshCommittedState() {
    return !window._smartviewUploadDirty && !smartviewHasPendingStoreEntries() && !window.AxGlobalChange;
  }

  function smartviewGetCommittedFullRowsSnapshot() {
    if (!Array.isArray(window._smartviewCommittedFullData)) return [];
    return smartviewCloneJsonSafe(window._smartviewCommittedFullData);
  }

  function smartviewSetExcelUploadSnapshot(snapshot) {
    const safe = snapshot ? smartviewCloneJsonSafe(snapshot) : null;
    window._smartviewExcelUploadState = safe;
    try {
      window._entity = window._entity || {};
      window._entity.excelUploadJson = safe ? smartviewCloneJsonSafe(safe) : null;
    } catch (e) {}
    return safe;
  }

  function smartviewPersistRowsToCache(ctrl, rows, options) {
    const safeRows = smartviewCloneJsonSafe(Array.isArray(rows) ? rows : []);
    const opts = options || {};
    const adsName = String((ctrl && ctrl.adsName) || smartviewGetCurrentAdsForViews() || "").trim();
    const cacheInfo = smartviewBuildFullDataCacheMetaFromController(ctrl);
    if (!cacheInfo) return Promise.resolve(true);

    return smartviewDbPutFullDataRecord({
      cacheKey: String(cacheInfo.cacheKey || ""),
      signature: String(cacheInfo.signature || ""),
      adsName: String(cacheInfo.adsName || adsName).toLowerCase(),
      totalCount: safeRows.length,
      rows: safeRows,
      excelTemplateColumns: smartviewCloneJsonSafe(opts.columns || []),
      source: String(opts.source || "smartview"),
      sourceFileName: String(opts.fileName || ""),
      updatedAt: Date.now()
    }).then(function () {
      return smartviewDbPruneFullDataRecords();
    }).then(function () {
      return true;
    }).catch(function (err) {
      console.warn("[SmartView] failed to persist SmartView rows:", err && err.message ? err.message : err);
      return false;
    });
  }

  function smartviewRestoreCommittedViewState() {
    const ctrl = getSmartviewControllerInstance();
    const rows = smartviewGetCommittedFullRowsSnapshot();
    smartviewApplyUploadedRowsToCurrentView(ctrl, rows);

    const committedUpload = window._smartviewCommittedExcelUploadState
      ? smartviewCloneJsonSafe(window._smartviewCommittedExcelUploadState)
      : null;

    smartviewSetExcelUploadSnapshot(committedUpload);

    const cacheColumns = committedUpload && Array.isArray(committedUpload.columns) ? committedUpload.columns : [];
    const cacheSource = committedUpload ? "excel-upload" : "smartview";
    const cacheFileName = committedUpload ? committedUpload.fileName : "";

    return smartviewPersistRowsToCache(ctrl, rows, {
      columns: cacheColumns,
      source: cacheSource,
      fileName: cacheFileName
    }).then(function () {
      window._smartviewUploadDirty = false;
      smartviewRefreshGlobalDirtyFlag();
      return true;
    });
  }

  function smartviewPersistUploadedSmartviewData(ctrl, rows, columns, fileName, uploadMeta) {
    const safeRows = smartviewCloneJsonSafe(Array.isArray(rows) ? rows : []);
    const safeColumns = smartviewCloneJsonSafe((Array.isArray(columns) ? columns : []).map(function (item) {
      return {
        field: item.field,
        caption: item.caption,
        label: item.label
      };
    }));
    const safeUploadMeta = (uploadMeta && typeof uploadMeta === "object") ? uploadMeta : {};
    const adsName = String((ctrl && ctrl.adsName) || smartviewGetCurrentAdsForViews() || "").trim();
    const updatedAt = Date.now();

    const snapshot = {
      adsName: adsName,
      fileName: String(fileName || "").trim(),
      columns: safeColumns,
      rows: safeRows,
      keyFields: smartviewCloneJsonSafe(Array.isArray(safeUploadMeta.keyFields) ? safeUploadMeta.keyFields : []),
      mode: String(safeUploadMeta.mode || "").trim(),
      stats: smartviewCloneJsonSafe(safeUploadMeta.stats || {}),
      operations: smartviewCloneJsonSafe(Array.isArray(safeUploadMeta.operations) ? safeUploadMeta.operations : []),
      updatedAt: updatedAt
    };

    smartviewSetExcelUploadSnapshot(snapshot);

    return smartviewPersistRowsToCache(ctrl, safeRows, {
      columns: safeColumns,
      source: "excel-upload",
      fileName: snapshot.fileName
    }).then(function () {
      return snapshot;
    }).catch(function (err) {
      console.warn("[SmartView] failed to persist uploaded Excel data:", err && err.message ? err.message : err);
      return snapshot;
    });
  }

  function smartviewApplyUploadedRowsToCurrentView(ctrl, rows) {
    const safeRows = Array.isArray(rows) ? rows.slice() : [];
    window._smartviewFullData = safeRows.slice();
    window._entity = window._entity || {};
    window._entity.filteredListJson = safeRows.slice();

    if (ctrl) {
      ctrl._filteredCache = safeRows.slice();
      ctrl.totalCount = safeRows.length;
      ctrl.refreshCache = false;
      ctrl._allDataFetchedOnce = true;
    }

    if (ctrl && typeof ctrl._initializeFrontendWindow === "function") {
      ctrl._initializeFrontendWindow(safeRows);
    } else {
      window._entity.listJson = safeRows.slice(0, Number(window._entity.pageSize || 100) || 100);
      if (typeof createTableViewHTML === "function") createTableViewHTML(window._entity.listJson, 1);
    }

    const searchInput = document.getElementById("searchBox");
    if (searchInput && String(searchInput.value || "").trim() && typeof liveSearch === "function") {
      liveSearch();
    }
  }

  function smartviewCloseExcelTemplateMenu() {
    try {
      const existing = document.getElementById("smartviewExcelTemplateMenu");
      if (existing) existing.remove();
    } catch (e) {}
  }

  function smartviewOpenExcelTemplateMenu(anchorBtn) {
    if (!smartviewIsDataUploadAllowedForView()) {
      alert("Excel template download is disabled for this SmartView.");
      return false;
    }
    smartviewCloseExcelTemplateMenu();
    const button = anchorBtn || document.getElementById("smartviewDownloadTemplateBtn");
    if (!button) return false;

    const menu = document.createElement("div");
    menu.id = "smartviewExcelTemplateMenu";
    menu.className = "sv-header-menu";
    menu.innerHTML = [
      '<button type="button" class="sv-header-menu-item" data-include-data="false">Empty Template</button>',
      '<button type="button" class="sv-header-menu-item" data-include-data="true">With Data</button>'
    ].join("");

    document.body.appendChild(menu);

    const rect = button.getBoundingClientRect();
    menu.style.left = Math.min(window.innerWidth - menu.offsetWidth - 10, rect.left) + "px";
    menu.style.top = (rect.bottom + 6) + "px";

    menu.addEventListener("click", function (ev) {
      const item = ev.target && ev.target.closest ? ev.target.closest("[data-include-data]") : null;
      if (!item) return;
      const includeData = String(item.getAttribute("data-include-data") || "").toLowerCase() === "true";
      smartviewCloseExcelTemplateMenu();
      downloadTemplate(includeData);
    });

    setTimeout(function () {
      document.addEventListener("click", function _close(ev) {
        const target = ev.target;
        if ((button === target || (button.contains && button.contains(target))) || (menu.contains && menu.contains(target))) return;
        smartviewCloseExcelTemplateMenu();
        document.removeEventListener("click", _close);
      });
    }, 20);

    return false;
  }

  function downloadTemplate(includeData) {
    if (typeof XLSX === "undefined" || !XLSX || !XLSX.utils) {
      smartviewEnsureXlsxLoaded(function (ok) {
        if (ok) downloadTemplate(includeData);
        else alert("Excel library is not available on this page.");
      });
      return false;
    }

    const ctrl = getSmartviewControllerInstance();
    const columns = smartviewBuildExcelTemplateColumns(ctrl);
    if (!columns.length) {
      alert("No table columns available for template download.");
      return false;
    }

    const data = [columns.map(function (item) { return item.label; })];
    if (includeData) {
      const rows = smartviewGetExcelTemplateSourceRows(ctrl);
      rows.forEach(function (row) {
        data.push(columns.map(function (column) {
          return smartviewPrepareExcelTemplateCellValue(row, column);
        }));
      });
    }

    const ws = XLSX.utils.aoa_to_sheet(data);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Template");
    XLSX.writeFile(wb, smartviewGetTemplateFileBaseName() + "_Template.xlsx");
    return false;
  }

  function triggerSmartviewUploadExcel() {
    if (!smartviewIsDataUploadAllowedForView()) {
      alert("Excel upload is disabled for this SmartView.");
      return false;
    }
    const input = document.getElementById("smartviewUploadExcelInput");
    if (!input) {
      alert("Upload control is not available on this page.");
      return false;
    }
    input.value = "";
    input.click();
    return false;
  }

  function handleUploadExcel(e) {
    if (!smartviewIsDataUploadAllowedForView()) {
      const inputDisabled = e && e.target ? e.target : null;
      if (inputDisabled) inputDisabled.value = "";
      alert("Excel upload is disabled for this SmartView.");
      return false;
    }

    const input = e && e.target ? e.target : null;
    const file = input && input.files ? input.files[0] : null;
    if (!file) return false;
    if (typeof XLSX === "undefined" || !XLSX || !XLSX.utils) {
      smartviewEnsureXlsxLoaded(function (ok) {
        if (ok) handleUploadExcel(e);
        else {
          if (input) input.value = "";
          alert("Excel library is not available on this page.");
        }
      });
      return false;
    }

    const ctrl = getSmartviewControllerInstance();
    const columns = smartviewBuildExcelTemplateColumns(ctrl);
    if (!columns.length) {
      alert("No table columns available for Excel upload.");
      if (input) input.value = "";
      return false;
    }

    const existingRows = smartviewCloneJsonSafe(smartviewGetExcelTemplateSourceRows(ctrl));
    let matchStrategy = smartviewResolveExcelUploadMatchStrategy(columns, existingRows, ctrl);
    const validateUploadedData = smartviewShouldValidateUploadedData(ctrl && Array.isArray(ctrl.lastAdsMeta) ? ctrl.lastAdsMeta : null);
    if (matchStrategy.mode === "metadata-keyfields-missing") {
      if (validateUploadedData) {
        const missing = (matchStrategy.missingFields || []).join(", ");
        alert(`Excel upload requires ADS key column(s): ${missing}. Add them to the SmartView columns and download the template again.`);
        if (input) input.value = "";
        return false;
      }

      const duplicateField = smartviewResolveExcelDuplicateField(columns, existingRows);
      matchStrategy = {
        mode: duplicateField ? "fallback-key" : "fallback-signature",
        keyFields: duplicateField ? [duplicateField] : [],
        explicitKeyFields: [],
        missingFields: [],
        duplicateField: duplicateField
      };
    }
    const reader = new FileReader();

    reader.onload = function (evt) {
      try {
        const data = new Uint8Array(evt.target.result);
        const workbook = XLSX.read(data, { type: "array", cellDates: true });
        const firstSheet = workbook && workbook.SheetNames && workbook.SheetNames.length ? workbook.SheetNames[0] : "";
        if (!firstSheet) throw new Error("EMPTY_WORKBOOK");

        const sheet = workbook.Sheets[firstSheet];
        const uploadedRows = XLSX.utils.sheet_to_json(sheet, { defval: "" });
        if (!uploadedRows.length) {
          alert("No data rows found in the uploaded Excel file.");
          return;
        }

        const normalizedUploadedRows = [];

        uploadedRows.forEach(function (rawRow) {
          const mappedRow = {};

          columns.forEach(function (column) {
            const rawValue = smartviewGetUploadCellValue(rawRow, [column.label, column.caption, column.field]);
            const normalized = smartviewNormalizeUploadedCellValue(rawValue, column.meta);
            if (normalized !== "" || rawValue === 0 || rawValue === false || rawValue === true) {
              mappedRow[column.field] = normalized;
            }
          });

          if (smartviewIsUploadedRowEmpty(mappedRow)) {
            normalizedUploadedRows.push({});
          } else {
            normalizedUploadedRows.push(mappedRow);
          }
        });

        const mergeResult = smartviewMergeUploadedExcelRows(existingRows, normalizedUploadedRows, columns, matchStrategy);
        const insertedCount = Number(mergeResult && mergeResult.stats && mergeResult.stats.inserted) || 0;
        const updatedCount = Number(mergeResult && mergeResult.stats && mergeResult.stats.updated) || 0;
        const duplicateSkipped = Number(mergeResult && mergeResult.stats && mergeResult.stats.duplicateSkipped) || 0;
        const emptySkipped = Number(mergeResult && mergeResult.stats && mergeResult.stats.emptySkipped) || 0;
        const keyMissingSkipped = Number(mergeResult && mergeResult.stats && mergeResult.stats.keyMissingSkipped) || 0;
        const appliedCount = insertedCount + updatedCount;

        if (!appliedCount) {
          if (duplicateSkipped || emptySkipped || keyMissingSkipped) {
            const skipParts = [];
            if (duplicateSkipped) skipParts.push(`${duplicateSkipped} duplicate${duplicateSkipped !== 1 ? "s" : ""} skipped`);
            if (emptySkipped) skipParts.push(`${emptySkipped} empty row${emptySkipped !== 1 ? "s" : ""} skipped`);
            if (keyMissingSkipped) skipParts.push(`${keyMissingSkipped} row${keyMissingSkipped !== 1 ? "s" : ""} missing key value${keyMissingSkipped !== 1 ? "s" : ""} skipped`);
            alert(`Applied 0 changes. ${skipParts.join(", ")}.`);
            return;
          }
          alert("No valid Excel changes found to apply.");
          return;
        }

        const finalRows = Array.isArray(mergeResult && mergeResult.rows) ? mergeResult.rows : [];
        smartviewApplyUploadedRowsToCurrentView(ctrl, finalRows);

        smartviewPersistUploadedSmartviewData(ctrl, finalRows, columns, file.name, mergeResult).then(function () {
          if (smartviewIsBulkSaveEnabled()) {
            window._smartviewUploadDirty = true;
            window.AxGlobalChange = true;
          } else {
            smartviewMarkCurrentStateCommitted();
          }
          smartviewRefreshGlobalDirtyFlag();

          const appliedParts = [];
          if (insertedCount) appliedParts.push(`${insertedCount} insert${insertedCount !== 1 ? "s" : ""}`);
          if (updatedCount) appliedParts.push(`${updatedCount} update${updatedCount !== 1 ? "s" : ""}`);
          const skipParts = [];
          if (duplicateSkipped) skipParts.push(`${duplicateSkipped} duplicate${duplicateSkipped !== 1 ? "s" : ""} skipped`);
          if (emptySkipped) skipParts.push(`${emptySkipped} empty row${emptySkipped !== 1 ? "s" : ""} skipped`);
          if (keyMissingSkipped) skipParts.push(`${keyMissingSkipped} row${keyMissingSkipped !== 1 ? "s" : ""} missing key value${keyMissingSkipped !== 1 ? "s" : ""} skipped`);
          const msg = skipParts.length
            ? `Applied ${appliedParts.join(", ")}. ${skipParts.join(", ")}.`
            : `Applied ${appliedParts.join(", ")} successfully.`;
          alert(msg);
        });
      } catch (err) {
        console.error("[SmartView] Excel upload failed:", err);
        alert("Upload failed. Please check the Excel file and try again.");
      } finally {
        if (input) input.value = "";
      }
    };

    reader.onerror = function () {
      if (input) input.value = "";
      alert("Unable to read the selected Excel file.");
    };

    reader.readAsArrayBuffer(file);
    return false;
  }

  function initSmartviewExcelUpload() {
    const input = document.getElementById("smartviewUploadExcelInput");
    if (!input || input.dataset.svUploadBound === "T") return;
    input.addEventListener("change", handleUploadExcel);
    input.dataset.svUploadBound = "T";
  }

  function smartviewHasUnsavedChangesFast() {
    if (window._smartviewUploadDirty) return true;
    if (smartviewHasPendingStoreEntries()) return true;

    const frames = document.querySelectorAll("iframe.tstruct-frame");
    for (let i = 0; i < frames.length; i++) {
      if (smartviewFrameHasPendingChanges(frames[i])) return true;
    }

    return false;
  }

  function smartviewRunWithNavigationGuardBypass(callback) {
    window._smartviewNavigationGuardBypass = true;
    try {
      if (typeof callback === "function") callback();
    } finally {
      setTimeout(function () {
        window._smartviewNavigationGuardBypass = false;
      }, 0);
    }
  }

  function smartviewShowBulkNavigationPrompt(continueNavigation) {
    if (window._smartviewNavigationPromptOpen) return Promise.resolve(false);
    window._smartviewNavigationPromptOpen = true;

    var finish = function (shouldContinue) {
      window._smartviewNavigationPromptOpen = false;
      if (shouldContinue) smartviewRunWithNavigationGuardBypass(continueNavigation);
      return shouldContinue;
    };

    if (!window.jQuery || typeof jQuery.confirm !== "function") {
      var nativeChoice = window.confirm("You have unsaved SmartView changes. Click OK to save before leaving, or Cancel to stay on this page.");
      if (!nativeChoice) return Promise.resolve(finish(false));
      return smartviewRunSaveFlow({
        mode: "save",
        successMessage: "",
        silentSuccess: true
      }).then(function () {
        return finish(true);
      }).catch(function () {
        return finish(false);
      });
    }

    return new Promise(function (resolve) {
      jQuery.confirm({
        title: "Unsaved changes",
        content: "You have unsaved SmartView changes. Choose how you want to proceed.",
        closeIcon: false,
        backgroundDismiss: false,
        escapeKey: "cancel",
        buttons: {
          save: {
            text: "Save",
            btnClass: "btn-primary",
            action: function () {
              var dlg = this;
              dlg.showLoading();
              smartviewRunSaveFlow({
                mode: "save",
                successMessage: "",
                silentSuccess: true
              }).then(function (result) {
                if (result && result.pending) {
                  dlg.hideLoading();
                  resolve(finish(false));
                  return;
                }
                dlg.close();
                resolve(finish(true));
              }).catch(function () {
                dlg.hideLoading();
                resolve(finish(false));
              });
              return false;
            }
          },
          draft: {
            text: "Draft Save",
            action: function () {
              var dlg = this;
              dlg.showLoading();
              smartviewRunSaveFlow({
                mode: "draft",
                successMessage: "",
                silentSuccess: true
              }).then(function () {
                dlg.close();
                resolve(finish(true));
              }).catch(function () {
                dlg.hideLoading();
                resolve(finish(false));
              });
              return false;
            }
          },
          discard: {
            text: "Discard",
            btnClass: "btn-danger",
            action: function () {
              var dlg = this;
              dlg.showLoading();
              smartviewRunSaveFlow({
                mode: "discard",
                successMessage: "",
                silentSuccess: true
              }).then(function () {
                dlg.close();
                resolve(finish(true));
              }).catch(function () {
                dlg.hideLoading();
                resolve(finish(false));
              });
              return false;
            }
          },
          cancel: {
            text: "Cancel",
            action: function () {
              resolve(finish(false));
            }
          }
        },
        onDestroy: function () {
          if (window._smartviewNavigationPromptOpen) {
            window._smartviewNavigationPromptOpen = false;
          }
        }
      });
    });
  }

  function smartviewHandleProtectedNavigation(continueNavigation) {
    if (!smartviewHasUnsavedChangesFast()) {
      smartviewRunWithNavigationGuardBypass(continueNavigation);
      return Promise.resolve(true);
    }

    if (!smartviewIsBulkSaveEnabled()) {
      return smartviewRunSaveFlow({
        mode: "save",
        successMessage: "",
        silentSuccess: true,
        silentNoChanges: true
      }).then(function (result) {
        if (result && result.pending) return false;
        smartviewRunWithNavigationGuardBypass(continueNavigation);
        return true;
      }).catch(function () {
        return false;
      });
    }

    return smartviewShowBulkNavigationPrompt(continueNavigation);
  }

  function smartviewResolveNavigationAnchor(target) {
    if (!target || typeof target.closest !== "function") return null;

    const anchor = target.closest("a[href]");
    if (!anchor) return null;
    if (anchor.closest(".jconfirm")) return null;

    const href = String(anchor.getAttribute("href") || "").trim();
    if (!href || href === "#" || /^javascript:/i.test(href)) return null;
    if (anchor.hasAttribute("download")) return null;

    const targetAttr = String(anchor.getAttribute("target") || "").trim().toLowerCase();
    if (targetAttr === "_blank") return null;

    return anchor;
  }

  function smartviewBindNavigationGuard(scopeDoc) {
    if (!scopeDoc || scopeDoc._smartviewNavGuardBound) return;

    scopeDoc.addEventListener("click", function (evt) {
      try {
        if (window._smartviewNavigationGuardBypass || window._smartviewNavigationPromptOpen) return;
        if (evt.defaultPrevented) return;
        if (evt.button !== 0 || evt.metaKey || evt.ctrlKey || evt.shiftKey || evt.altKey) return;

        const anchor = smartviewResolveNavigationAnchor(evt.target);
        if (!anchor || !smartviewHasUnsavedChangesFast()) return;

        evt.preventDefault();
        evt.stopPropagation();
        evt.stopImmediatePropagation();

        smartviewHandleProtectedNavigation(function () {
          if (anchor && typeof anchor.click === "function") {
            anchor.click();
            return;
          }
          const href = anchor ? anchor.getAttribute("href") : "";
          if (href) window.top.location.href = href;
        });
      } catch (e) {}
    }, true);

    scopeDoc._smartviewNavGuardBound = true;
  }

  function smartviewInitNavigationGuard() {
    try { smartviewBindNavigationGuard(document); } catch (e) {}
    try {
      if (window.top && window.top.document && window.top.document !== document) {
        smartviewBindNavigationGuard(window.top.document);
      }
    } catch (e) {}

    if (!window._smartviewBeforeUnloadBound) {
      window.addEventListener("beforeunload", function (evt) {
        if (window._smartviewNavigationGuardBypass) return;
        if (!smartviewIsBulkSaveEnabled()) return;
        if (!smartviewHasUnsavedChangesFast()) return;
        evt.preventDefault();
        evt.returnValue = "You have unsaved SmartView changes.";
        return evt.returnValue;
      });
      window._smartviewBeforeUnloadBound = true;
    }
  }

  function smartviewRestoreDraftStateMarker() {
    var applyDraft = function (draft) {
      if (!draft || typeof draft !== "object") return;

      if (Array.isArray(draft.committedRows)) {
        window._smartviewCommittedFullData = smartviewCloneJsonSafe(draft.committedRows);
      }
      if (draft.committedUploadSnapshot) {
        window._smartviewCommittedExcelUploadState = smartviewCloneJsonSafe(draft.committedUploadSnapshot);
      }
      if (draft.uploadSnapshot) {
        smartviewSetExcelUploadSnapshot(draft.uploadSnapshot);
        window._smartviewUploadDirty = true;
      }
    };

    var key = smartviewGetDraftSaveKey();
    var localKey = "smartview_draft_payload::" + key;
    var restored = false;

    try {
      var raw = localStorage.getItem(localKey);
      if (raw) {
        applyDraft(JSON.parse(raw));
        restored = true;
      }
    } catch (e) {}

    if (!restored) {
      smartviewDbGetDraftRecord(key).then(function (draft) {
        applyDraft(draft);
        smartviewRefreshGlobalDirtyFlag();
      }).catch(function () {});
    }

    smartviewRefreshGlobalDirtyFlag();
  }

  function smartviewReloadFrame() {
    smartviewHandleProtectedNavigation(function () {
      // Clear IndexDB cache before reloading
      smartviewClearIndexDBCache(function () {
        try {
          if (window && window.location && typeof window.location.reload === "function") {
            window.location.reload();
            return;
          }
        } catch (e) {}

        try {
          window.location.href = window.location.href;
        } catch (e) {}
      });
    });
    return false;
  }

  function smartviewClearIndexDBCache(callback) {
    try {
      const indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;
      if (!indexedDB) {
        // IndexDB not available, proceed with reload
        if (typeof callback === 'function') callback();
        return;
      }

      const dbNames = ['AxpertDB', 'smartviewDB', 'smartlistDB', 'axpertDB'];
      let completed = 0;
      let totalDbs = dbNames.length;

      dbNames.forEach(function (dbName) {
        try {
          const deleteRequest = indexedDB.deleteDatabase(dbName);
          
          deleteRequest.onsuccess = function () {
            completed++;
            console.log('✓ Deleted IndexDB: ' + dbName);
            if (completed === totalDbs && typeof callback === 'function') {
              callback();
            }
          };

          deleteRequest.onerror = function () {
            completed++;
            console.log('✗ Failed to delete IndexDB: ' + dbName);
            if (completed === totalDbs && typeof callback === 'function') {
              callback();
            }
          };

          deleteRequest.onblocked = function () {
            console.log('⚠ Delete blocked for: ' + dbName);
          };
        } catch (e) {
          completed++;
          console.error('Error clearing ' + dbName + ':', e);
          if (completed === totalDbs && typeof callback === 'function') {
            callback();
          }
        }
      });

      // Fallback: proceed after 2 seconds if not all DBs cleared
      setTimeout(function () {
        if (completed < totalDbs && typeof callback === 'function') {
          callback();
        }
      }, 2000);

    } catch (e) {
      console.error('smartviewClearIndexDBCache error:', e);
      if (typeof callback === 'function') callback();
    }
  }

  function smartviewParseNewTstructIds(rawValue) {
    const seen = new Set();
    return String(rawValue || '')
      .split(/[,;^~|]+/)
      .map(function (item) { return String(item || '').trim(); })
      .filter(function (item) { return !!item; })
      .map(function (item) {
        return smartviewNormalizeTypedTransId(item, 't');
      })
      .filter(function (item) {
        const key = item.toLowerCase();
        if (!item || seen.has(key)) return false;
        seen.add(key);
        return true;
      });
  }

  function smartviewOpenSingleNewTstruct(transid) {
    const safeTransid = smartviewNormalizeTypedTransId(transid, 't');
    if (!safeTransid) return false;

    if (typeof openLinkInPopup === 'function') {
      openLinkInPopup(`t${safeTransid}()`);
      return true;
    }

    const url = smartviewBuildTstructUrl(safeTransid, '', 'open');
    if (typeof parent !== 'undefined' && parent && typeof parent.createPopup === 'function') {
      parent.createPopup(url, true, ()=>{}, ()=>{});
    } else {
      window.open(url, '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=900,height=700');
    }
    return true;
  }

  function smartviewOpenNewTstruct(anchorBtn) {
    try {
      const ctrl = window.smartTableController || window._smartviewController || window._smartviewTableController || null;

      const openFromMeta = function (meta) {
        const arr = Array.isArray(meta) ? meta : [];
        const cfg = (typeof smartviewExtractEditConfigFromMeta === 'function')
          ? smartviewExtractEditConfigFromMeta(arr)
          : { newforms_transid: '', newforms: '' };

        if (!cfg.allownew) {
          window.alert('New entry is disabled for this SmartView.');
          return false;
        }

        const transids = smartviewParseNewTstructIds(cfg.newforms_transid || cfg.newforms || '');
        if (!transids.length) {
          window.alert('New form is not configured for this view.');
          return false;
        }

        if (transids.length === 1) {
          return smartviewOpenSingleNewTstruct(transids[0]);
        }

        if (anchorBtn && typeof showAxRowOptionsMenu === 'function') {
          showAxRowOptionsMenu(anchorBtn, transids.map(function (transid) {
            return {
              name: transid,
              link: `t${transid}()`
            };
          }));
          return true;
        }

        return smartviewOpenSingleNewTstruct(transids[0]);
      };

      const currentMeta =
        (ctrl && Array.isArray(ctrl.lastAdsMeta) && ctrl.lastAdsMeta.length) ? ctrl.lastAdsMeta
        : (window._entity && Array.isArray(window._entity.metaData)) ? window._entity.metaData
        : [];

      const hasNodes = (typeof smartviewMetaHasControlNodes === 'function')
        ? smartviewMetaHasControlNodes(currentMeta)
        : true;

      // If cached metadata doesn't include control nodes, fetch fresh ADS metadata first.
      if ((!currentMeta || !currentMeta.length || !hasNodes) && ctrl && typeof ctrl.ensureAdsMetadata === 'function') {
        ctrl.ensureAdsMetadata(function (_err, freshMeta) {
          openFromMeta((freshMeta && freshMeta.length) ? freshMeta : currentMeta);
        });
        return true;
      }

      return openFromMeta(currentMeta);
    } catch (e) {
      console.error('smartviewOpenNewTstruct failed', e);
      return false;
    }
  }
  /* --------------------------
    Class controller
    -------------------------- */

  class SmartViewTableController {
    constructor(opts = {}) {
      this.adsName = opts.adsName || "ds_smartlist_users";
      this.pageSize = Math.max(1, Number(opts.pageSize ?? 100) || 100);
      this.frontendChunkSize = this.pageSize;
      // Keep only two chunks (200 rows) in DOM, same as Entity.js streaming behavior.
      this.frontendWindowMaxRows = Math.max(this.frontendChunkSize * 2, this.frontendChunkSize);
      // Backend must return full dataset once; frontend handles chunk rendering.
      this.serverPageSize = 0;
      this.streamingAllRecords = true;
      this.pageno = opts.currentPage ?? 1;
      this.sorting = opts.sorting || [];
      this.filters = opts.filters || [];
      this.axClient_dateformat = opts.axClient_dateformat || (typeof window.axClient_dateformat !== 'undefined' ? window.axClient_dateformat : "");
      this.select_columns = Array.isArray(opts.select_columns) ? opts.select_columns.slice() : [];
      this._smartviewSelectedFieldColumns = Array.isArray(opts.field_columns)
        ? opts.field_columns.slice()
        : (Array.isArray(opts.selectedFieldColumns) ? opts.selectedFieldColumns.slice() : []);
      this.groupby_columns = Array.isArray(opts.groupby_columns) ? opts.groupby_columns.slice() : [];
      this.column_order = smartviewParseColumnOrder(Array.isArray(opts.column_order) ? opts.column_order : smartviewLoadColumnOrderFromStorage(opts.adsName));
      this.aggregations = (opts.aggregations && typeof opts.aggregations === 'object') ? Object.assign({}, opts.aggregations) : {};
      this.bulkSaveEnabled = smartviewFlagFromValue(opts.bulkSaveEnabled, window._smartviewBulkSaveEnabled);
      this.deferInitialLoad = !!opts.deferInitialLoad;
      this.refreshCache = false;

      this.isFetching = false;
      this.totalCount = 0;
      this.loadedCount = 0;
      this._pagingFallbackTried = false;
      this._userHasScrolled = false;
      this._lastPageReached = false;
      this.visibleRowRangeStart = 1;
      this.visibleRowRangeEnd = 0;
      this.frontendRenderedCount = 0;
      this._allDataFetchedOnce = false;
      this._lastScrollTop = 0;
      this._loadingAllRows = false;

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
        try { smartviewLoadKpiChartsData(this.adsName, { refreshCache: false }); } catch (e) {}
      }
      try { ensureSmartviewTitlebarViewControls(); } catch (e) {}
      try { refreshSmartviewTitlebarViewDropdown(); } catch (e) {}
      try {
        smartviewEnsureColumnOrderLoadedForAds(this.adsName, (order) => {
          const parsed = smartviewParseColumnOrder(order);
          if (!parsed.length) return;
          this.column_order = parsed.slice();
          if (Array.isArray(window._entity?.listJson) && window._entity.listJson.length && typeof createTableViewHTML === 'function') {
            const pn = Math.max(1, (Number(this.pageno) || 1) - 1);
            createTableViewHTML(window._entity.listJson, pn);
          }
        });
      } catch (e) {}

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
      self.resetPaging();
      self.loadNextPage();
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
                else if (text.includes('word') || text.includes('doc')) handleExport('word');
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
      this.pageSize = this.frontendChunkSize || 100;
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
      this.visibleRowRangeStart = 1;
      this.visibleRowRangeEnd = 0;
      this.frontendRenderedCount = 0;
      this._allDataFetchedOnce = false;
      this._lastScrollTop = 0;
      this._loadingAllRows = false;
      window._entity.listJson = [];
      window._entity.pageSize = this.frontendChunkSize;
      this._filteredCache = null;
      window._smartviewFullData = null;
      try { smartviewLoadKpiChartsData(this.adsName, { refreshCache: false }); } catch (e) {}
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
        pagesize: this.serverPageSize,
        keyfield: "",
        keyvalue: "",
        sorting: this.sorting,
        filters: safeFilters
      };

      if (this.axClient_dateformat) props.axClient_dateformat = this.axClient_dateformat;
      const hasGroupby = Array.isArray(this.groupby_columns) && this.groupby_columns.length > 0;
      if (!hasGroupby && Array.isArray(this.select_columns) && this.select_columns.length) props.select_columns = this.select_columns.slice();
      if (!hasGroupby && Array.isArray(this.groupby_columns) && this.groupby_columns.length) props.groupby_columns = this.groupby_columns.slice();
      // Do not pass aggregations; use groupby_columns with sum(...) expressions instead.

      return {
        adsNames: [this.adsName],
        refreshCache: this.refreshCache,
        sqlParams: sqlParams,
        props: props
      };
    }

    _getAllRowsData() {
      return Array.isArray(window._smartviewFullData) ? window._smartviewFullData : [];
    }

    _setAllRowsData(rows) {
      const metaForSort = (Array.isArray(this.lastAdsMeta) && this.lastAdsMeta.length)
        ? this.lastAdsMeta
        : (window._entity && Array.isArray(window._entity.metaData) ? window._entity.metaData : []);
      const allRows = smartviewApplyClientSortingFallback(
        Array.isArray(rows) ? rows.slice() : [],
        this.sorting,
        metaForSort
      );
      window._smartviewFullData = allRows;
      this._filteredCache = allRows;
      this._allDataFetchedOnce = true;
      this.totalCount = allRows.length;
      this._lastPageReached = allRows.length <= this.frontendChunkSize;
      return allRows;
    }

    _renderCurrentDataWindow(scrollDirection) {
      const allRows = this._getAllRowsData();
      const startIdx = Math.max(0, (this.visibleRowRangeStart || 1) - 1);
      const endIdx = Math.max(startIdx, this.visibleRowRangeEnd || 0);
      const windowRows = allRows.slice(startIdx, endIdx);

      window._entity.listJson = windowRows;
      this.loadedCount = windowRows.length;
      this.frontendRenderedCount = endIdx;
      this._lastPageReached = endIdx >= allRows.length;

      try {
        if (rowCountManager && typeof rowCountManager.setTotal === "function") {
          rowCountManager.setTotal(this.totalCount);
          rowCountManager.setLoadedRecords(windowRows.length);
          if (typeof rowCountManager.setLastPageReached === "function") {
            rowCountManager.setLastPageReached(this._lastPageReached);
          }
        }
      } catch (e) {}

      if (typeof createTableViewHTML === "function") {
        createTableViewHTML(windowRows, 1);
      }

      // Keep scroll anchor stable when we shift the virtual window.
      if (scrollDirection === "down" || scrollDirection === "up") {
        setTimeout(() => {
          try {
            const tableDiv = document.querySelector('#table-body_Container .table-responsive');
            if (!tableDiv) return;
            if (scrollDirection === "down") {
              tableDiv.scrollTop = Math.floor(tableDiv.scrollHeight * 0.35);
            } else {
              tableDiv.scrollTop = Math.floor(tableDiv.scrollHeight * 0.65);
            }
          } catch (e) {}
        }, 0);
      }
    }

    _initializeFrontendWindow(rows) {
      const allRows = this._setAllRowsData(rows);
      if (!allRows.length) {
        this.visibleRowRangeStart = 0;
        this.visibleRowRangeEnd = 0;
        this.frontendRenderedCount = 0;
        window._entity.listJson = [];
        this.loadedCount = 0;
        this._lastPageReached = true;
        if (typeof createTableViewHTML === "function") createTableViewHTML([], 1);
        try {
          if (rowCountManager && typeof rowCountManager.setTotal === "function") {
            rowCountManager.setTotal(0);
            rowCountManager.setLoadedRecords(0);
            if (typeof rowCountManager.setLastPageReached === "function") rowCountManager.setLastPageReached(true);
          }
        } catch (e) {}
        return;
      }

      this.visibleRowRangeStart = 1;
      this.visibleRowRangeEnd = Math.min(this.frontendChunkSize, allRows.length);
      this.frontendRenderedCount = this.visibleRowRangeEnd;
      this.pageno = 1;
      this._renderCurrentDataWindow();

      try {
        const hasCommittedSnapshot = Array.isArray(window._smartviewCommittedFullData);
        if ((!hasCommittedSnapshot && !smartviewHasUnsavedChangesFast()) || smartviewShouldRefreshCommittedState()) {
          smartviewMarkCurrentStateCommitted();
        }
      } catch (e) {}
    }

    loadNextFrontendChunk() {
      const allRows = this._getAllRowsData();
      if (!this.streamingAllRecords || !allRows.length) return false;

      const currentWindowSize = Math.max(0, this.visibleRowRangeEnd - this.visibleRowRangeStart + 1);
      if (this.visibleRowRangeEnd >= allRows.length && currentWindowSize >= this.frontendChunkSize) {
        this._lastPageReached = true;
        return false;
      }

      let nextStart = this.visibleRowRangeStart > 0 ? this.visibleRowRangeStart - 1 : 0;
      let nextEnd = this.visibleRowRangeEnd;

      if ((nextEnd - nextStart) < this.frontendWindowMaxRows) {
        nextEnd = Math.min(nextEnd + this.frontendChunkSize, allRows.length);
      } else {
        nextStart = Math.min(nextStart + this.frontendChunkSize, allRows.length);
        nextEnd = Math.min(nextEnd + this.frontendChunkSize, allRows.length);
      }

      if (nextStart >= nextEnd) {
        this._lastPageReached = true;
        return false;
      }

      this.visibleRowRangeStart = nextStart + 1;
      this.visibleRowRangeEnd = nextEnd;
      this.pageno = Math.max(1, Math.ceil(this.visibleRowRangeEnd / this.frontendChunkSize));
      this._renderCurrentDataWindow("down");
      return true;
    }

    loadPreviousFrontendChunk() {
      const allRows = this._getAllRowsData();
      if (!this.streamingAllRecords || !allRows.length) return false;
      if (this.visibleRowRangeStart <= 1) return false;

      const nextStart = Math.max((this.visibleRowRangeStart - 1) - this.frontendChunkSize, 0);
      const targetWindow = Math.min(this.frontendWindowMaxRows, allRows.length);
      const nextEnd = Math.min(nextStart + targetWindow, allRows.length);

      if (nextStart >= nextEnd) return false;

      this.visibleRowRangeStart = nextStart + 1;
      this.visibleRowRangeEnd = nextEnd;
      this.pageno = Math.max(1, Math.ceil(this.visibleRowRangeEnd / this.frontendChunkSize));
      this._renderCurrentDataWindow("up");
      return true;
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
          if (smartviewLooksLikeAdsMetadataDescriptorMeta(this.lastAdsMeta)) {
            this.lastAdsMeta = null;
            this._adsMetaFor = null;
            this.fetchAdsMetadata(callback, true);
            return;
          }
          const liveCfg = smartviewExtractEditConfigFromMeta(this.lastAdsMeta);
          if (!smartviewHasValue(liveCfg.bulksaveRaw) || !smartviewMetaHasFilterTypeHints(this.lastAdsMeta)) {
            this.fetchAdsMetadata(callback);
            return;
          }
          smartviewApplyBulkSaveSetting(this.lastAdsMeta);
          if (typeof callback === 'function') callback(null, this.lastAdsMeta);
          return;
        }

        const cached = loadSmartviewAdsMetaFromStorage(adsKey);
        if (cached && Array.isArray(cached) && cached.length) {
          const cachedCfg = smartviewExtractEditConfigFromMeta(cached);
          if (!smartviewMetaHasControlNodes(cached) || !smartviewHasValue(cachedCfg.bulksaveRaw) || !smartviewMetaHasFilterTypeHints(cached)) {
            this.fetchAdsMetadata(callback);
            return;
          }
          this.lastAdsMeta = cached;
          this._adsMetaFor = adsKey;
          this.lastAdsMetaRaw = smartviewLoadSmartviewAdsMetaRawFromStorage(adsKey);
          this._adsMetaRawFor = adsKey;
          window._entity = window._entity || {};
          window._entity.metaData = cached;
          smartviewSyncKeyFieldState(cached, this);
          smartviewApplyBulkSaveSetting(cached);
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

    fetchAdsMetadata(callback, forceRefresh) {
      const self = this;
      try {
        const shouldForceRefresh = !!forceRefresh;
        const adsKey = (this.adsName || window._entity?.adsName || '').toString().trim();
        const metaFor = (this._adsMetaFor || '').toString();
        if (this.lastAdsMeta && Array.isArray(this.lastAdsMeta) && this.lastAdsMeta.length &&
            metaFor && metaFor.toLowerCase() === String(adsKey).toLowerCase()) {
          if (smartviewLooksLikeAdsMetadataDescriptorMeta(this.lastAdsMeta)) {
            this.lastAdsMeta = null;
            this._adsMetaFor = null;
          } else {
            const liveCfg = smartviewExtractEditConfigFromMeta(this.lastAdsMeta);
            if (shouldForceRefresh || !smartviewHasValue(liveCfg.bulksaveRaw) || !smartviewMetaHasFilterTypeHints(this.lastAdsMeta)) {
              // Keep going and fetch a fresh metadata snapshot that includes bulksave.
            } else {
              smartviewApplyBulkSaveSetting(this.lastAdsMeta);
              if (typeof callback === 'function') callback(null, this.lastAdsMeta);
              return;
            }
          }
        }
    
        const params = {
          adsNames: ['ds_smartlist_ads_metadata'],
          refreshCache: !!shouldForceRefresh,
          sqlParams: { adsname: this.adsName || window._entity?.adsName },
          props: { ADS: false, CachePermissions: true, getallrecordscount: false, pageno: 1, pagesize: 500, sorting: [], filters: [] }
        };
    
        // const caller = (typeof parent !== 'undefined' && parent.GetDataFromAxList) ? parent
        //              : (typeof window !== 'undefined' && window.GetDataFromAxList) ? window
        //              : null;
      
      
      const scopes = [parent, window, window.top];

      const caller = scopes.find(
          w => w && typeof w.GetDataFromAxList === 'function'
      );
    
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
            let metaRawForStorage = [];
            const responseSourceForStorage = {
              message: smartviewHasValue(parsed?.result?.message) ? String(parsed.result.message).trim() : (smartviewHasValue(parsed?.message) ? String(parsed.message).trim() : null),
              success: (parsed?.result?.success !== undefined) ? parsed.result.success : ((parsed?.success !== undefined) ? parsed.success : null),
              ADSNames: parsed?.result?.ADSNames ?? parsed?.ADSNames ?? dsBlock?.ADSNames ?? null,
              adsname: (dsBlock?.adsname || dsBlock?.adsName || self.adsName || adsKey || '').toString().trim() || null
            };
    
            // ---------- Case A: ADS returned metadata rows (common in your sample) ----------
            // detect when dsBlock.data is an array of objects that look like metadata (have fldname)
            const candidateMetaRows = Array.isArray(dsBlock?.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
            const metaAllowEditDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['allowedit', 'allow_edit']);
            const metaBulkSaveDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['bulksave', 'bulk_save', 'bulkSave']);
            const metaNewFormsDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['newforms', 'new_forms']);
            const metaNewFormsTransIdDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['newforms_transid', 'newforms_trans_id', 'newformstransid']);
            const metaAllowNewDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['allownew', 'allow_new']);
            const metaDataUploadDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['dataupload', 'allowupload', 'upload']);
            const metaValidateDataDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['validatedata', 'validate_data', 'validateupload', 'validate_upload']);
            const metaEditModeDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['editmode', 'edit_mode']);
            const metaSaveModeDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['savemode', 'save_mode']);
            const metaSvNameDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['sv_name', 'viewname', 'smartviewname']);
            const metaSvCaptionDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['sv_caption', 'viewcaption', 'smartviewcaption']);
            const metaSvSourceDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['sv_sourcecnd', 'sourcecnd', 'source_type', 'sourcetype']);
            const metaSvKeycolDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['sv_keycol', 'svkeycol', 'keycolumn', 'key_column']);
            const metaApiConfigDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['api_config', 'apiconfig', 'api']);
            const metaMemdbKeyDefault = smartviewResolveAdsMetaNodeValue(candidateMetaRows, dsBlock, parsed, ['memdb_key', 'memdbkey', 'inmem_key', 'redis_key']);
            const firstMetaFieldName = (candidateMetaRows && candidateMetaRows.length > 0 && candidateMetaRows[0])
              ? smartviewGetObjectValueCI(candidateMetaRows[0], ['fldname', 'fieldname', 'name', 'key'])
              : '';
            if (candidateMetaRows && candidateMetaRows.length > 0 && smartviewHasValue(firstMetaFieldName)) {
              metaRawForStorage = smartviewCloneJsonSafe(candidateMetaRows) || [];
              meta = candidateMetaRows.map(r => {
                const fldnameRaw = (smartviewGetObjectValueCI(r, ['fldname', 'fieldname', 'name', 'key']) || r.fldname || r.fieldname || r.name || '').toString();
                const fldname = fldnameRaw.trim();
                const fldcap = (smartviewGetObjectValueCI(r, ['fldcaption', 'fldcap', 'caption', 'title', 'displayname', 'fieldcaption']) || r.fldcaption || r.fldcap || r.caption || '').toString();
                const fdtRaw = (r.fdatatype || r.datatype || r.cdatatype || '').toString().toLowerCase();

                // SmartView ADS metadata commonly marks "normalized" fields (T) that should be dropdown filters.
                const normalizedRaw = (r.normalized ?? r.isnormalized ?? r.is_normalized ?? r.isNormalized ?? r.normalised ?? r.isnormalised);
                const isNormalized = (normalizedRaw === true) || (String(normalizedRaw || '').toUpperCase() === 'T');
                const srcTable = (r.srctable || r.src_table || r.srctbl || r.sourcetable || r.source_table || r.srcTable || r.sourceTable || r.table || r.tablename || r.tblname || '').toString().trim();
                const srcFld = (r.srcfld || r.src_fld || r.srcfield || r.sourcefld || r.source_fld || r.sourcefield || r.srcField || r.sourceField || r.column || r.colname || r.columnname || '').toString().trim();
                const hasSourceDropdown = !!(srcTable && srcFld);
    
                // Accept 'c','n','d','b' or longer words Ã¢â‚¬â€ keep original token so EntityFilter can use it.
                // If normalized, force a text-ish base type so Entity-Filter doesn't override dropdown as Numeric/Date.
                const fdatatype = (isNormalized || hasSourceDropdown) ? 'c' : (fdtRaw || 'c');
    
                // Respect explicit filter metadata. If not present, leave unset and let fallback decide.
                const rowFiltersRaw = smartviewGetObjectValueCI(r, ['filters', 'filter', 'applyfilter', 'apply_filter']);
                let filtersFlag = '';
                if (smartviewHasValue(rowFiltersRaw)) {
                  filtersFlag = smartviewFlagFromValue(rowFiltersRaw, false) ? 'T' : 'F';
                }
    
                // include any dropdown options if backend provided them
                const options = Array.isArray(r.options) ? r.options : (r.options && typeof r.options === 'string' ? tryParseJsonSafe(r.options) : undefined);

                // Normalize cdatatype spelling/case for Entity-Filter.js switch cases.
                let cdatatype = r.cdatatype;
                if (typeof cdatatype === 'string') {
                  const cd = cdatatype.trim().toLowerCase();
                  if (cd === 'dropdown' || cd === 'drop down' || cd === 'drop_down' || cd === 'select') cdatatype = 'DropDown';
                }
                // If normalized or source-backed, force dropdown filter (product metadata doesn't always mark it correctly).
                if (isNormalized || hasSourceDropdown) cdatatype = 'DropDown';
                else if (!cdatatype || String(cdatatype).trim() === '') cdatatype = undefined;

                // Build psrctxt for ds_smartlist_filters:
                //   fldname~normalized~source table~source fld
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
                const rowAllowEdit = smartviewGetObjectValueCI(r, ['allowedit', 'allow_edit']) || metaAllowEditDefault;
                const rowAllowNew = smartviewGetObjectValueCI(r, ['allownew', 'allow_new']) || metaAllowNewDefault;
                const rowBulkSave = smartviewGetObjectValueCI(r, ['bulksave', 'bulk_save', 'bulkSave']) || metaBulkSaveDefault;
                const rowDataUpload = smartviewGetObjectValueCI(r, ['dataupload', 'allowupload', 'upload']) || metaDataUploadDefault;
                const rowValidateData = smartviewGetObjectValueCI(r, ['validatedata', 'validate_data', 'validateupload', 'validate_upload']) || metaValidateDataDefault;
                const rowEditMode = smartviewGetObjectValueCI(r, ['editmode', 'edit_mode']) || metaEditModeDefault;
                const rowSaveMode = smartviewGetObjectValueCI(r, ['savemode', 'save_mode']) || metaSaveModeDefault;
                const rowNewForms = smartviewGetObjectValueCI(r, ['newforms', 'new_forms']) || metaNewFormsDefault;
                const rowNewFormsTransId = smartviewGetObjectValueCI(r, ['newforms_transid', 'newforms_trans_id', 'newformstransid']) || metaNewFormsTransIdDefault;
                const rowKeyField = smartviewGetObjectValueCI(r, ['keyfield', 'key_field']);
                const rowSvName = smartviewGetObjectValueCI(r, ['sv_name', 'viewname', 'smartviewname']) || metaSvNameDefault;
                const rowSvCaption = smartviewGetObjectValueCI(r, ['sv_caption', 'viewcaption', 'smartviewcaption']) || metaSvCaptionDefault;
                const rowSvSource = smartviewGetObjectValueCI(r, ['sv_sourcecnd', 'sourcecnd', 'source_type', 'sourcetype']) || metaSvSourceDefault;
                const rowSvKeycol = smartviewGetObjectValueCI(r, ['sv_keycol', 'svkeycol', 'keycolumn', 'key_column']) || metaSvKeycolDefault;
                const rowApiConfig = smartviewGetObjectValueCI(r, ['api_config', 'apiconfig', 'api']) || metaApiConfigDefault;
                const rowMemdbKey = smartviewGetObjectValueCI(r, ['memdb_key', 'memdbkey', 'inmem_key', 'redis_key']) || metaMemdbKeyDefault;
                const rowColHide = smartviewGetObjectValueCI(r, ['col_hide', 'colhide', 'hide']);
                const rowColFilter = smartviewGetObjectValueCI(r, ['col_filter', 'colfilter']);
                if (smartviewHasValue(rowColFilter)) {
                  filtersFlag = smartviewFlagFromValue(rowColFilter, false) ? 'T' : 'F';
                }
                const rowHypInline = smartviewGetObjectValueCI(r, ['hyp_inline', 'hypinline']);
                const rowDynamicColumns = smartviewGetObjectValueCI(r, ['dynamiccolumns', 'dynamic_columns']);
                const rowPagination = smartviewGetObjectValueCI(r, ['pagination']);
                const rowSorting = smartviewGetObjectValueCI(r, ['sorting']);

                return smartviewMergeAdsMetaRow(r, {
                  fldname: fldname,
                  fldcap: fldcap || formatFieldName(fldname),
                  fldcaption: fldcap || formatFieldName(fldname),
                  fdatatype: fdatatype,   // keep the short code as provided ('c','n','d' etc)
                  cdatatype: cdatatype || undefined,
                  ftransid: ftransid,
                  listingfld: smartviewHasValue(r.listingfld) ? String(r.listingfld).trim() : 'T',
                  filters: smartviewHasValue(filtersFlag) ? String(filtersFlag).trim().toUpperCase() : null,
                  options: options,
                  normalized: isNormalized ? 'T' : 'F',
                  srctable: srcTable,
                  sourcetable: srcTable,
                  srcfld: srcFld,
                  sourcefld: srcFld,
                  psrctxt: psrctxt,
                  sqlname: sqlname || null,
                  hyp_structtype: hypStructTypeStr || null,
                  hyp_transid: hypTransIdStr || null,
                  tbl_hyperlink: tblHyperlinkStr || null,
                  hyp_inline: smartviewHasValue(rowHypInline) ? String(rowHypInline).trim() : (smartviewHasValue(r?.hyp_inline) ? String(r.hyp_inline).trim() : null),
                  dynamiccolumns: smartviewHasValue(rowDynamicColumns) ? String(rowDynamicColumns).trim() : null,
                  pagination: smartviewHasValue(rowPagination) ? String(rowPagination).trim() : null,
                  sorting: smartviewHasValue(rowSorting) ? String(rowSorting).trim() : null,
                  col_hide: smartviewHasValue(rowColHide) ? String(rowColHide).trim() : null,
                  col_filter: smartviewHasValue(rowColFilter) ? String(rowColFilter).trim() : null,
                  hide: smartviewHasValue(rowColHide) ? (smartviewFlagFromValue(rowColHide, false) ? 'T' : 'F') : null,
                  keyfield: smartviewHasValue(rowKeyField) ? String(rowKeyField).trim() : null,
                  allowedit: smartviewHasValue(rowAllowEdit) ? String(rowAllowEdit).trim() : null,
                  allownew: smartviewHasValue(rowAllowNew) ? String(rowAllowNew).trim() : null,
                  bulksave: smartviewHasValue(rowBulkSave) ? String(rowBulkSave).trim() : null,
                  dataupload: smartviewHasValue(rowDataUpload) ? String(rowDataUpload).trim() : null,
                  validatedata: smartviewHasValue(rowValidateData) ? String(rowValidateData).trim() : null,
                  editmode: smartviewHasValue(rowEditMode) ? String(rowEditMode).trim() : null,
                  savemode: smartviewHasValue(rowSaveMode) ? String(rowSaveMode).trim() : null,
                  newforms: smartviewHasValue(rowNewForms) ? String(rowNewForms).trim() : null,
                  newforms_transid: smartviewHasValue(rowNewFormsTransId) ? String(rowNewFormsTransId).trim() : null,
                  sv_name: smartviewHasValue(rowSvName) ? String(rowSvName).trim() : null,
                  sv_caption: smartviewHasValue(rowSvCaption) ? String(rowSvCaption).trim() : null,
                  sv_sourcecnd: smartviewHasValue(rowSvSource) ? String(rowSvSource).trim() : null,
                  sv_keycol: smartviewHasValue(rowSvKeycol) ? String(rowSvKeycol).trim() : null,
                  api_config: smartviewHasValue(rowApiConfig) ? rowApiConfig : null,
                  memdb_key: smartviewHasValue(rowMemdbKey) ? String(rowMemdbKey).trim() : null
                });
              }).filter(m => m.fldname);
            }
            // ---------- Case B: dsBlock.columns present (existing behavior) ----------
            else if (dsBlock && Array.isArray(dsBlock.columns) && dsBlock.columns.length &&
                !smartviewLooksLikeAdsMetadataDescriptorMeta(dsBlock.columns)) {
              const colsRows = dsBlock.columns;
              metaRawForStorage = smartviewCloneJsonSafe(colsRows) || [];
              const colAllowEditDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['allowedit', 'allow_edit']) || metaAllowEditDefault;
              const colAllowNewDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['allownew', 'allow_new']) || metaAllowNewDefault;
              const colBulkSaveDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['bulksave', 'bulk_save', 'bulkSave']) || metaBulkSaveDefault;
              const colDataUploadDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['dataupload', 'allowupload', 'upload']) || metaDataUploadDefault;
              const colValidateDataDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['validatedata', 'validate_data', 'validateupload', 'validate_upload']) || metaValidateDataDefault;
              const colEditModeDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['editmode', 'edit_mode']) || metaEditModeDefault;
              const colSaveModeDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['savemode', 'save_mode']) || metaSaveModeDefault;
              const colNewFormsDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['newforms', 'new_forms']) || metaNewFormsDefault;
              const colNewFormsTransIdDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['newforms_transid', 'newforms_trans_id', 'newformstransid']) || metaNewFormsTransIdDefault;
              const colSvNameDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['sv_name', 'viewname', 'smartviewname']) || metaSvNameDefault;
              const colSvCaptionDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['sv_caption', 'viewcaption', 'smartviewcaption']) || metaSvCaptionDefault;
              const colSvSourceDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['sv_sourcecnd', 'sourcecnd', 'source_type', 'sourcetype']) || metaSvSourceDefault;
              const colSvKeycolDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['sv_keycol', 'svkeycol', 'keycolumn', 'key_column']) || metaSvKeycolDefault;
              const colApiConfigDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['api_config', 'apiconfig', 'api']) || metaApiConfigDefault;
              const colMemdbKeyDefault = smartviewResolveAdsMetaNodeValue(colsRows, dsBlock, parsed, ['memdb_key', 'memdbkey', 'inmem_key', 'redis_key']) || metaMemdbKeyDefault;
              meta = dsBlock.columns.map(col => {
                const normalizedRaw = (col.normalized ?? col.isnormalized ?? col.is_normalized ?? col.isNormalized ?? col.normalised ?? col.isnormalised);
                const isNormalized = (normalizedRaw === true) || (String(normalizedRaw || '').toUpperCase() === 'T');

                const fldname = (smartviewGetObjectValueCI(col, ['key', 'name', 'fldname', 'fieldname']) || col.key || col.name || '').toString();
                const fldcap = (smartviewGetObjectValueCI(col, ['caption', 'fldcaption', 'fldcap', 'title']) || col.caption || formatFieldName(fldname));

                const srcTable = (col.srctable || col.src_table || col.srctbl || col.sourcetable || col.source_table || col.srcTable || col.sourceTable || col.table || col.tablename || col.tblname || '').toString().trim();
                const srcFld = (col.srcfld || col.src_fld || col.srcfield || col.sourcefld || col.source_fld || col.sourcefield || col.srcField || col.sourceField || col.column || col.colname || col.columnname || '').toString().trim();
                const hasSourceDropdown = !!(srcTable && srcFld);
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
                const colAllowEdit = smartviewGetObjectValueCI(col, ['allowedit', 'allow_edit']) || colAllowEditDefault;
                const colAllowNew = smartviewGetObjectValueCI(col, ['allownew', 'allow_new']) || colAllowNewDefault;
                const colBulkSave = smartviewGetObjectValueCI(col, ['bulksave', 'bulk_save', 'bulkSave']) || colBulkSaveDefault;
                const colDataUpload = smartviewGetObjectValueCI(col, ['dataupload', 'allowupload', 'upload']) || colDataUploadDefault;
                const colValidateData = smartviewGetObjectValueCI(col, ['validatedata', 'validate_data', 'validateupload', 'validate_upload']) || colValidateDataDefault;
                const colEditMode = smartviewGetObjectValueCI(col, ['editmode', 'edit_mode']) || colEditModeDefault;
                const colSaveMode = smartviewGetObjectValueCI(col, ['savemode', 'save_mode']) || colSaveModeDefault;
                const colNewForms = smartviewGetObjectValueCI(col, ['newforms', 'new_forms']) || colNewFormsDefault;
                const colNewFormsTransId = smartviewGetObjectValueCI(col, ['newforms_transid', 'newforms_trans_id', 'newformstransid']) || colNewFormsTransIdDefault;
                const colKeyField = smartviewGetObjectValueCI(col, ['keyfield', 'key_field']);
                const colSvName = smartviewGetObjectValueCI(col, ['sv_name', 'viewname', 'smartviewname']) || colSvNameDefault;
                const colSvCaption = smartviewGetObjectValueCI(col, ['sv_caption', 'viewcaption', 'smartviewcaption']) || colSvCaptionDefault;
                const colSvSource = smartviewGetObjectValueCI(col, ['sv_sourcecnd', 'sourcecnd', 'source_type', 'sourcetype']) || colSvSourceDefault;
                const colSvKeycol = smartviewGetObjectValueCI(col, ['sv_keycol', 'svkeycol', 'keycolumn', 'key_column']) || colSvKeycolDefault;
                const colApiConfig = smartviewGetObjectValueCI(col, ['api_config', 'apiconfig', 'api']) || colApiConfigDefault;
                const colMemdbKey = smartviewGetObjectValueCI(col, ['memdb_key', 'memdbkey', 'inmem_key', 'redis_key']) || colMemdbKeyDefault;
                const colHide = smartviewGetObjectValueCI(col, ['col_hide', 'colhide', 'hide']);
                const colFilter = smartviewGetObjectValueCI(col, ['col_filter', 'colfilter']);
                const colFiltersRaw = smartviewGetObjectValueCI(col, ['filters', 'filter', 'applyfilter', 'apply_filter']);
                let colFiltersFlag = '';
                if (smartviewHasValue(colFilter)) colFiltersFlag = smartviewFlagFromValue(colFilter, false) ? 'T' : 'F';
                else if (smartviewHasValue(colFiltersRaw)) colFiltersFlag = smartviewFlagFromValue(colFiltersRaw, false) ? 'T' : 'F';
                const colHypInline = smartviewGetObjectValueCI(col, ['hyp_inline', 'hypinline']);
                const colDynamicColumns = smartviewGetObjectValueCI(col, ['dynamiccolumns', 'dynamic_columns']);
                const colPagination = smartviewGetObjectValueCI(col, ['pagination']);
                const colSorting = smartviewGetObjectValueCI(col, ['sorting']);

                return smartviewMergeAdsMetaRow(col, {
                  fldname: fldname,
                  fldcap: fldcap,
                  fldcaption: fldcap,
                  fdatatype: (isNormalized || hasSourceDropdown) ? 'c' : (col.datatype || 't'),
                  cdatatype: (isNormalized || hasSourceDropdown) ? 'DropDown' : inferColumnType(col),
                  ftransid: (col.ftransid || col.fTransId || col.transid || self.adsName || window._entity?.entityTransId || '') || '',
                  listingfld: smartviewHasValue(col.listingfld) ? String(col.listingfld).trim() : 'T',
                  filters: smartviewHasValue(colFiltersFlag) ? String(colFiltersFlag).trim().toUpperCase() : null,
                  normalized: isNormalized ? 'T' : 'F',
                  srctable: srcTable,
                  sourcetable: srcTable,
                  srcfld: srcFld,
                  sourcefld: srcFld,
                  psrctxt: psrctxt,
                  sqlname: sqlname || null,
                  hyp_structtype: hypStructTypeStr || null,
                  hyp_transid: hypTransIdStr || null,
                  tbl_hyperlink: tblHyperlinkStr || null,
                  hyp_inline: smartviewHasValue(colHypInline) ? String(colHypInline).trim() : (smartviewHasValue(col?.hyp_inline) ? String(col.hyp_inline).trim() : null),
                  dynamiccolumns: smartviewHasValue(colDynamicColumns) ? String(colDynamicColumns).trim() : null,
                  pagination: smartviewHasValue(colPagination) ? String(colPagination).trim() : null,
                  sorting: smartviewHasValue(colSorting) ? String(colSorting).trim() : null,
                  col_hide: smartviewHasValue(colHide) ? String(colHide).trim() : null,
                  col_filter: smartviewHasValue(colFilter) ? String(colFilter).trim() : null,
                  hide: smartviewHasValue(colHide) ? (smartviewFlagFromValue(colHide, false) ? 'T' : 'F') : null,
                  keyfield: smartviewHasValue(colKeyField) ? String(colKeyField).trim() : null,
                  allowedit: smartviewHasValue(colAllowEdit) ? String(colAllowEdit).trim() : null,
                  allownew: smartviewHasValue(colAllowNew) ? String(colAllowNew).trim() : null,
                  bulksave: smartviewHasValue(colBulkSave) ? String(colBulkSave).trim() : null,
                  dataupload: smartviewHasValue(colDataUpload) ? String(colDataUpload).trim() : null,
                  validatedata: smartviewHasValue(colValidateData) ? String(colValidateData).trim() : null,
                  editmode: smartviewHasValue(colEditMode) ? String(colEditMode).trim() : null,
                  savemode: smartviewHasValue(colSaveMode) ? String(colSaveMode).trim() : null,
                  newforms: smartviewHasValue(colNewForms) ? String(colNewForms).trim() : null,
                  newforms_transid: smartviewHasValue(colNewFormsTransId) ? String(colNewFormsTransId).trim() : null,
                  sv_name: smartviewHasValue(colSvName) ? String(colSvName).trim() : null,
                  sv_caption: smartviewHasValue(colSvCaption) ? String(colSvCaption).trim() : null,
                  sv_sourcecnd: smartviewHasValue(colSvSource) ? String(colSvSource).trim() : null,
                  sv_keycol: smartviewHasValue(colSvKeycol) ? String(colSvKeycol).trim() : null,
                  api_config: smartviewHasValue(colApiConfig) ? colApiConfig : null,
                  memdb_key: smartviewHasValue(colMemdbKey) ? String(colMemdbKey).trim() : null
                });
              }).filter(m => m.fldname);
            }
            // ---------- Case C: fallback Ã¢â‚¬â€ infer from first data row ----------
            else {
              const rows = Array.isArray(dsBlock.data) ? dsBlock.data : (Array.isArray(parsed?.data) ? parsed.data : []);
              if (rows && rows.length > 0) {
                const sample = rows[0];
                const keys = Object.keys(sample || {}).map(k => k.toString());
                const preferredOrder = ['transid', 'recordid', 'processname', 'taskname', 'formcaption', 'keyfieldcaption', 'username', 'email', 'nickname'];
                const tmp = [];
                preferredOrder.forEach(k => {
                  if (keys.includes(k) && !tmp.some(m => m.fldname.toLowerCase() === k.toLowerCase())) {
                    tmp.push({
                      fldname: k,
                      fldcap: formatFieldName(k),
                      fdatatype: 't',
                      cdatatype: 'Text',
                      listingfld: 'T',
                      filters: null,
                      allowedit: smartviewHasValue(metaAllowEditDefault) ? String(metaAllowEditDefault).trim() : null,
                      allownew: smartviewHasValue(metaAllowNewDefault) ? String(metaAllowNewDefault).trim() : null,
                      bulksave: smartviewHasValue(metaBulkSaveDefault) ? String(metaBulkSaveDefault).trim() : null,
                      dataupload: smartviewHasValue(metaDataUploadDefault) ? String(metaDataUploadDefault).trim() : null,
                      validatedata: smartviewHasValue(metaValidateDataDefault) ? String(metaValidateDataDefault).trim() : null,
                      editmode: smartviewHasValue(metaEditModeDefault) ? String(metaEditModeDefault).trim() : null,
                      savemode: smartviewHasValue(metaSaveModeDefault) ? String(metaSaveModeDefault).trim() : null,
                      newforms: smartviewHasValue(metaNewFormsDefault) ? String(metaNewFormsDefault).trim() : null,
                      newforms_transid: smartviewHasValue(metaNewFormsTransIdDefault) ? String(metaNewFormsTransIdDefault).trim() : null,
                      sv_name: smartviewHasValue(metaSvNameDefault) ? String(metaSvNameDefault).trim() : null,
                      sv_caption: smartviewHasValue(metaSvCaptionDefault) ? String(metaSvCaptionDefault).trim() : null,
                      sv_sourcecnd: smartviewHasValue(metaSvSourceDefault) ? String(metaSvSourceDefault).trim() : null,
                      sv_keycol: smartviewHasValue(metaSvKeycolDefault) ? String(metaSvKeycolDefault).trim() : null,
                      api_config: smartviewHasValue(metaApiConfigDefault) ? metaApiConfigDefault : null,
                      memdb_key: smartviewHasValue(metaMemdbKeyDefault) ? String(metaMemdbKeyDefault).trim() : null
                    });
                  }
                });
                keys.forEach(k => {
                  if (!tmp.some(m => m.fldname.toLowerCase() === k.toLowerCase())) {
                    tmp.push({
                      fldname: k,
                      fldcap: formatFieldName(k),
                      fdatatype: 't',
                      cdatatype: 'Text',
                      listingfld: 'T',
                      filters: null,
                      allowedit: smartviewHasValue(metaAllowEditDefault) ? String(metaAllowEditDefault).trim() : null,
                      allownew: smartviewHasValue(metaAllowNewDefault) ? String(metaAllowNewDefault).trim() : null,
                      bulksave: smartviewHasValue(metaBulkSaveDefault) ? String(metaBulkSaveDefault).trim() : null,
                      dataupload: smartviewHasValue(metaDataUploadDefault) ? String(metaDataUploadDefault).trim() : null,
                      validatedata: smartviewHasValue(metaValidateDataDefault) ? String(metaValidateDataDefault).trim() : null,
                      editmode: smartviewHasValue(metaEditModeDefault) ? String(metaEditModeDefault).trim() : null,
                      savemode: smartviewHasValue(metaSaveModeDefault) ? String(metaSaveModeDefault).trim() : null,
                      newforms: smartviewHasValue(metaNewFormsDefault) ? String(metaNewFormsDefault).trim() : null,
                      newforms_transid: smartviewHasValue(metaNewFormsTransIdDefault) ? String(metaNewFormsTransIdDefault).trim() : null,
                      sv_name: smartviewHasValue(metaSvNameDefault) ? String(metaSvNameDefault).trim() : null,
                      sv_caption: smartviewHasValue(metaSvCaptionDefault) ? String(metaSvCaptionDefault).trim() : null,
                      sv_sourcecnd: smartviewHasValue(metaSvSourceDefault) ? String(metaSvSourceDefault).trim() : null,
                      sv_keycol: smartviewHasValue(metaSvKeycolDefault) ? String(metaSvKeycolDefault).trim() : null,
                      api_config: smartviewHasValue(metaApiConfigDefault) ? metaApiConfigDefault : null,
                      memdb_key: smartviewHasValue(metaMemdbKeyDefault) ? String(metaMemdbKeyDefault).trim() : null
                    });
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

            if (smartviewLooksLikeAdsMetadataDescriptorMeta(normalized) || !normalized.length) {
              const fallbackMeta = smartviewBuildMetaFallbackFromCurrentRows(normalized, self.adsName || adsKey);
              if (fallbackMeta.length) {
                normalized.length = 0;
                fallbackMeta.forEach(function (m) { normalized.push(m); });
                metaRawForStorage = [];
              }
            }
    
            // If metadata contains no filter hints at all, provide a safe fallback:
            // mark basic text/number/date fields as filterable so the filter UI isn't empty.
            const hasFilterHints = normalized.some(x => smartviewHasValue(x.filters) || smartviewHasValue(x.col_filter));
            const hasExplicitFilters = normalized.some(x => (String(x.filters || '').toUpperCase() === 'T'));
            if (!hasExplicitFilters && !hasFilterHints && normalized.length) {
              normalized.forEach(n => {
                const ft = (n.fdatatype || '').toString().toLowerCase();
                if (ft === 'c' || ft === 't' || ft === 'string' || ft === 'n' || ft === 'd' || !n.filters) n.filters = 'T';
                else n.filters = 'F';
              });
            } else {
              normalized.forEach(n => {
                if (smartviewHasValue(n.col_filter)) {
                  n.filters = smartviewFlagFromValue(n.col_filter, false) ? 'T' : 'F';
                  return;
                }
                if (smartviewHasValue(n.filters)) {
                  n.filters = smartviewFlagFromValue(n.filters, false) ? 'T' : 'F';
                  return;
                }
                n.filters = 'F';
              });
            }
    
            window._entity = window._entity || {};
            window._entity.metaData = normalized;
            self.lastAdsMeta = normalized;
            self.lastAdsMetaRaw = smartviewCloneJsonSafe(metaRawForStorage) || [];
            smartviewSyncKeyFieldState(normalized, self);
            smartviewApplyBulkSaveSetting(normalized);
            try {
              const storeKey = (params && params.sqlParams && params.sqlParams.adsname) ? params.sqlParams.adsname : (self.adsName || adsKey);
              self._adsMetaFor = storeKey;
              self._adsMetaRawFor = storeKey;
              saveSmartviewAdsMetaToStorage(storeKey, normalized, {
                metaRaw: metaRawForStorage,
                source: responseSourceForStorage
              });
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
    if (this.requiresAdsSelection && !this.adsSelected) return;
    if (this.isFetching || this._loadingAllRows) return;

    // First call (or refresh) fetches full dataset once with pagesize 0.
    if (this.refreshCache || !this._allDataFetchedOnce) {
      this._loadAllRowsFromCacheOrApi();
      return;
    }

    const allRows = this._getAllRowsData();
    if (!allRows.length) {
      this._lastPageReached = true;
      return;
    }
    if (this._lastPageReached && this.visibleRowRangeEnd >= allRows.length) return;

    // If window not initialized yet, initialize from cached full rows.
    if ((this.visibleRowRangeEnd || 0) <= 0) {
      this._initializeFrontendWindow(allRows);
      return;
    }

    this.loadNextFrontendChunk();
  }

    _loadAllRowsFromCacheOrApi() {
      if (this._loadingAllRows || this.isFetching) return;
      try { smartviewShowTableLoader('#table-body_Container'); } catch (e) {}

      var params = this.buildParams(1);
      params.props = params.props || {};
      params.props.pageno = 1;
      params.props.pagesize = 0;

      var cacheMeta = smartviewBuildFullDataCacheKey(params);
      var cacheKey = cacheMeta.cacheKey;
      var cacheSignature = cacheMeta.signature;
      var adsLower = cacheMeta.adsName;
      var self = this;
      var finish = function () {
        self._loadingAllRows = false;
        try { smartviewHideTableLoader('#table-body_Container'); } catch (e) {}
      };
      var fetchFromApi = function () {
        self._fetchAllRowsAndInitialize(params, {
          cacheKey: cacheKey,
          signature: cacheSignature,
          adsName: adsLower,
          onDone: finish
        });
      };

      this._loadingAllRows = true;

      if (this.refreshCache) {
        smartviewDbDeleteFullDataRecord(cacheKey)
          .catch(function () { return false; })
          .then(function () { fetchFromApi(); });
        return;
      }

      smartviewDbGetFullDataRecord(cacheKey).then(function (rec) {
        var rows = (rec && Array.isArray(rec.rows)) ? rec.rows : null;
        var cachedTotalCount = Number(rec && rec.totalCount);
        var hasTrustedRows = Array.isArray(rows) && rows.length > 0;
        var hasTrustedEmpty = Array.isArray(rows)
          && rows.length === 0
          && rec
          && rec.isEmptyResult === true
          && !(Number.isFinite(cachedTotalCount) && cachedTotalCount > 0);
        if (hasTrustedRows || hasTrustedEmpty) {
          // Empty cached results can easily become stale after metadata/filter changes.
          // Trust non-empty cache, but re-fetch the API for empty cache snapshots so the
          // table does not get stuck showing "No data available" while the server has rows.
          if (hasTrustedEmpty) {
            fetchFromApi();
            return;
          }
          var activeFilters = stripSmartviewFilterTransId(self.filters || []);
          var displayRows = activeFilters.length
            ? smartviewApplyClientFilterFallback(rows, activeFilters)
            : rows;
          self.totalCount = activeFilters.length
            ? displayRows.length
            : (Number(rec && rec.totalCount ? rec.totalCount : rows.length) || rows.length);
          self._initializeFrontendWindow(displayRows);
          self.refreshCache = false;
          finish();
          return;
        }
        fetchFromApi();
      }).catch(function () {
        fetchFromApi();
      });
    }

    _fetchAllRowsAndInitialize(prebuiltParams, cacheInfo) {
      const params = prebuiltParams || this.buildParams(1);
      params.props = params.props || {};
      params.props.pageno = 1;
      params.props.pagesize = 0;
      var self = this;
      var doneOnce = false;
      var done = function () {
        if (doneOnce) return;
        doneOnce = true;
        self.refreshCache = false;
        self.isFetching = false;
        if (cacheInfo && typeof cacheInfo.onDone === "function") {
          try { cacheInfo.onDone(); } catch (e) {}
        }
      };

      const caller = (typeof smartviewResolveAxListCaller === 'function')
        ? smartviewResolveAxListCaller()
        : ([parent, window, window.top].find(w => w && typeof w.GetDataFromAxList === 'function') || null);
      if (!caller || typeof caller.GetDataFromAxList !== 'function') {
        console.error('GetDataFromAxList not available');
        done();
        return;
      }

      this.isFetching = true;
      caller.GetDataFromAxList(params, (response) => {
        try {
          const extracted = (typeof smartviewExtractRowsFromAxListResponse === 'function')
            ? smartviewExtractRowsFromAxListResponse(response)
            : { rows: [], dsBlock: {} };
          const sourceRows = Array.isArray(extracted.rows) ? extracted.rows : [];
          const activeFilters = stripSmartviewFilterTransId(this.filters || []);
          const rows = activeFilters.length
            ? smartviewApplyClientFilterFallback(sourceRows, activeFilters)
            : sourceRows;
          const dsBlock = extracted.dsBlock || {};
          const totalFromServer = Number(dsBlock.totalrecords ?? dsBlock.recordcount ?? sourceRows.length) || sourceRows.length;
          this.totalCount = activeFilters.length ? rows.length : (rows.length || totalFromServer || 0);
          this._initializeFrontendWindow(rows);

          if (cacheInfo && cacheInfo.cacheKey) {
            smartviewDbPutFullDataRecord({
              cacheKey: String(cacheInfo.cacheKey),
              signature: String(cacheInfo.signature || ""),
              adsName: String(cacheInfo.adsName || (this.adsName || "")).toLowerCase(),
              totalCount: this.totalCount,
              rows: smartviewCloneJsonSafe(rows),
              isEmptyResult: rows.length === 0,
              updatedAt: Date.now()
            }).then(function () {
              return smartviewDbPruneFullDataRecords();
            }).catch(function (e) {
              console.warn("[SmartView] full-data cache put failed:", e && e.message ? e.message : e);
            });
          }
        } catch (e) {
          console.error('loadNextPage: full-fetch parse error', e);
        } finally {
          done();
        }
      }, (err) => {
        console.error('loadNextPage: GetDataFromAxList error', err);
        done();
      });
    }

    _fallbackFetchAllAndAppend() {
      this._fetchAllRowsAndInitialize();
    }

    loadAllOnce() {
      this._fetchAllRowsAndInitialize();
    }

  attachScrollListener() {
    const container = document.getElementById("table-body_Container");
    if (!container) return console.warn('attachScrollListener: no container found');

    const responsive = container.querySelector('.table-responsive');

    if (this._scrollHandler && this._scrollTarget) {
      try { this._scrollTarget.removeEventListener('scroll', this._scrollHandler); } catch (e) {}
    }
    this._scrollTarget = null;

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

    let debounceTimer = null;
    const bottomThreshold = 120;
    const topThreshold = 24;

    const currentScrollTop = () => {
      if (scrollEl === window) return window.scrollY || window.pageYOffset || 0;
      return scrollEl.scrollTop || 0;
    };

    const isNearBottom = () => {
      try {
        if (scrollEl === window) {
          const scrolled = window.innerHeight + (window.scrollY || 0);
          const full = document.documentElement.scrollHeight;
          return (full - scrolled) <= bottomThreshold;
        }
        if (scrollEl.scrollHeight <= scrollEl.clientHeight + 2) return false;
        return (scrollEl.scrollHeight - (scrollEl.scrollTop + scrollEl.clientHeight)) <= bottomThreshold;
      } catch (e) {
        return false;
      }
    };

    const isNearTop = () => {
      try {
        if (scrollEl === window) return (window.scrollY || 0) <= topThreshold;
        return (scrollEl.scrollTop || 0) <= topThreshold;
      } catch (e) {
        return false;
      }
    };

    this._scrollHandler = () => {
      this._userHasScrolled = true;
      const nowTop = currentScrollTop();
      const direction = nowTop > this._lastScrollTop ? 'down' : (nowTop < this._lastScrollTop ? 'up' : '');
      this._lastScrollTop = nowTop;

      if (debounceTimer) clearTimeout(debounceTimer);
      debounceTimer = setTimeout(() => {
        try {
          if (this.isFetching) return;
          if (direction === 'down' && isNearBottom()) {
            this.loadNextPage();
          } else if (direction === 'up' && isNearTop()) {
            this.loadPreviousFrontendChunk();
          }
        } catch (e) {
          console.error('scroll handler error', e);
        }
      }, 90);
    };

    this._scrollTarget = scrollEl;
    this._scrollTarget.addEventListener('scroll', this._scrollHandler, { passive: true });
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

        if (typeof smartviewApplyGroupbySelection === 'function') {
          smartviewApplyGroupbySelection(this, meta || [], base);
        } else if (base.length > 0) {
          const gb = smartviewBuildGroupbyWithSums(meta || [], base);
          this.groupby_columns = gb.groupby_columns;
          this.select_columns = gb.select_columns;
          this.aggregations = {};
        } else {
          this.groupby_columns = [];
          this.select_columns = smartviewGetStoredSelectedFieldColumns(this);
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
      this.select_columns = smartviewGetStoredSelectedFieldColumns(this);
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
    if (window._smartviewSkipAutoBoot) {
      try { ensureSmartviewTitlebarViewControls(); } catch (e) {}
      try { smartviewRefreshOptionsMenuLabels(); } catch (e) {}
      return;
    }
    try { ensureSmartviewTitlebarViewControls(); } catch (e) {}
    try { smartviewRefreshOptionsMenuLabels(); } catch (e) {}
    try { initSmartviewExcelUpload(); } catch (e) {}
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
        try { ensureSmartviewTitlebarViewControls(); } catch (e) {}
        try { refreshSmartviewTitlebarViewDropdown(); } catch (e) {}
      } catch (e) {}
    }

    function parseGroupByList(raw) {
      if (!raw) return [];
      if (Array.isArray(raw)) return raw.map(x => String(x || '').trim()).filter(Boolean);
      return String(raw)
        .split(/[,\|;\n]+/)
        .map(s => String(s || '').trim())
        .filter(Boolean);
    }

    function resolveGroupByFieldsFromQuery(rawGroupBy, meta) {
      const rawList = parseGroupByList(rawGroupBy);
      if (!rawList.length) return [];

      const resolved = [];
      const seen = new Set();

      rawList.forEach(rf => {
        const raw = String(rf || '').trim();
        if (!raw) return;

        const res = (typeof smartviewResolveFilterField === 'function')
          ? smartviewResolveFilterField(raw, meta || [])
          : { fldname: raw, meta: null };

        let fld = (res && res.fldname ? String(res.fldname) : '').trim();
        if (!fld) fld = raw;

        const token = fld.toLowerCase();
        if (!token || seen.has(token)) return;
        seen.add(token);
        resolved.push(fld);
      });

      return resolved;
    }

    function applyGroupByFromQuery(ctrl, meta) {
      if (!groupByRaw || !ctrl) return;
      const normalized = resolveGroupByFieldsFromQuery(groupByRaw, meta || []);
      if (!normalized.length) return;

      ctrl._smartviewUrlGroupByFields = normalized.slice();
      window._smartviewInitialUrlGroupByFields = normalized.slice();
      if (typeof smartviewApplyGroupbySelection === 'function') {
        smartviewApplyGroupbySelection(ctrl, meta || [], normalized);
      } else {
        const gb = smartviewBuildGroupbyWithSums(meta || [], normalized);
        ctrl.groupby_columns = gb.groupby_columns;
        ctrl.select_columns = gb.select_columns;
        ctrl.aggregations = {};
      }
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
            ctrl.column_order = smartviewLoadColumnOrderFromStorage(name);
            try {
              smartviewEnsureColumnOrderLoadedForAds(name, (order) => {
                const parsed = smartviewParseColumnOrder(order);
                if (parsed.length) ctrl.column_order = parsed;
              });
            } catch (e) {}
          }
          try { ctrl.resetPaging(); } catch (e) {}
        }

        const ctrl = window.smartTableController;
        try { smartviewLoadKpiChartsData(name, { refreshCache: false }); } catch (e) {}

        const applyMapped = function (meta) {
          try {
            const mapped = (typeof smartviewMapExternalFiltersToEntityFilters === 'function')
              ? smartviewMapExternalFiltersToEntityFilters(rawFilters || [], meta || [])
              : [];

            ctrl.filters = mapped;
            applyGroupByFromQuery(ctrl, meta || []);

            ctrl.deferInitialLoad = false;
            ctrl.forceClientFiltering = Array.isArray(mapped) && mapped.length > 0;
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
            ctrl.column_order = smartviewLoadColumnOrderFromStorage(name);
            try {
              smartviewEnsureColumnOrderLoadedForAds(name, (order) => {
                const parsed = smartviewParseColumnOrder(order);
                if (parsed.length) ctrl.column_order = parsed;
              });
            } catch (e) {}
          }
          try { ctrl.resetPaging(); } catch (e) {}
        }

        const ctrl = window.smartTableController;
        try { smartviewLoadKpiChartsData(name, { refreshCache: false }); } catch (e) {}
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

  // function attachSmartviewTempExpandHandlers() {
  //   try {
  //     if (!window.jQuery) return;

  //     $(document)
  //       .off('click.smartviewTempExpand', '.sv-hyperlinktemp')
  //       .on('click.smartviewTempExpand', '.sv-hyperlinktemp', function (e) {
  //         e.preventDefault();
  //         e.stopPropagation();
  //         e.stopImmediatePropagation();

  //         const $icon = $(this);
  //         const link = $icon.attr('data-link');
  //         if (!link) return;

  //         const $tr = $icon.closest('tr');
  //         let $nextRow = $tr.next('.expand-row');

  //         // Create row if not exists
  //         if ($nextRow.length === 0) {
  //           const colspan = $tr.children('td').length;

  //           $nextRow = $(`
  //             <tr class="expand-row">
  //               <td colspan="${colspan}">
  //                 <div class="iframe-loader-wrapper" style="opacity: 0; transition: opacity 0.3s ease-in-out;">
  //                   <iframe class="tstruct-frame"
  //                           style="width:100%; height:450px; border:1px solid #eee; box-shadow: 0 2px 10px rgba(0,0,0,0.1);"></iframe>
  //                 </div>
  //               </td>
  //             </tr>
  //           `).hide();

  //           $tr.after($nextRow);
  //         }

  //         const $wrapper = $nextRow.find('.iframe-loader-wrapper');
  //         const $iframe = $nextRow.find('iframe');
  //         const isExpanded = $nextRow.hasClass('sv-expanded');

  //         if (isExpanded) {
  //           const iframe = $iframe[0];
          

  //           // ðŸ”¥ SAVE BEFORE COLLAPSE
  //           saveTstructBeforeCollapse(iframe);
          
  //           $nextRow.removeClass('sv-expanded').hide();
  //           $nextRow.removeClass('sv-expanded').hide();
  //           $wrapper.css('opacity', '0'); // Reset opacity for next time
  //           $icon.text('chevron_right');
  //         } else {
  //           const url = openLinkInPopup(link, true);
  //           if (url) {
  //             // 1. Set loading state
  //             $wrapper.css('opacity', '0'); 
  //             $iframe.attr('src', url);

  //             $iframe.off('load').on('load', function () {
  //               const iframeEl = this;

              

  //               try {
                
  //                 const iframeDoc = this.contentDocument || this.contentWindow.document;
  //                 if (!iframeDoc) return;
  //  // ðŸ”¥ ADD THIS LINE (CRITICAL FIX)
  //               attachIframeChangeListener(
  //                 iframeEl,
  //                 getParamFromUrl(iframeEl.src, "transid"),
  //                 getParamFromUrl(iframeEl.src, "docid")
  //               );
  //                 // 2. Inject CSS immediately (First Defense)
  //                 const style = iframeDoc.createElement('style');
  //                 style.innerHTML = `
  //                   #dvlayout .footer, 
  //                   #dvlayout .toolbar,
  //                   #dvlayout .toolbarRightMenu,
  //                   .breadcrumb-panel { 
  //                     display: none !important; 
  //                   }
  //                   body { overflow-x: hidden; }
  //                 `;
  //                 (iframeDoc.head || iframeDoc.documentElement).appendChild(style);

  //                 // 3. Script-based hide (Second Defense)
  //                 const hideElements = () => {
  //                   $(iframeDoc)
  //                     .find('#dvlayout .footer, #dvlayout .toolbar, #dvlayout .toolbarRightMenu')
  //                     .css('display', 'none');
  //                 };
  //                 hideElements();

  //                 // 4. MutationObserver to catch late-rendering elements
  //                 const observer = new MutationObserver(() => {
  //                   hideElements();
  //                 });
  //                 observer.observe(iframeDoc.body, { childList: true, subtree: true });

  //                 // 5. Reveal the iframe only AFTER elements are hidden
  //                 setTimeout(() => {
  //                   $wrapper.css('opacity', '1');
  //                 }, 50);

  //               } catch (err) {
  //                 console.warn('Cross-origin restriction or DOM access failed', err);
  //                 $wrapper.css('opacity', '1'); // Show it anyway if we can't edit it
  //               }
  //             });
  //           }
            
  //           $nextRow.addClass('sv-expanded').show();
  //           $icon.text('expand_more');
  //         }

  //         return false;
  //       });

  //   } catch (e) {
  //     console.error("Smartview Expand Error:", e);
  //   }
  // }

  // function handleSubmitClick() {

  //   if (!window.AxGlobalChange) {
  //     alert("No changes to save");
  //     return;
  //   }

  //   if (!window._multiTstructStore || Object.keys(window._multiTstructStore).length === 0) {
  //     alert("No stored changes found");
  //     return;
  //   }

  //   let transactions = {};
  //   let index = 1;

  //   for (let key in window._multiTstructStore) {
  //     transactions["transaction" + index] = window._multiTstructStore[key];
  //     index++;
  //   }

  //   const finalJson = {
  //     _parameters: [
  //       {
  //         ARMSessionId: "", // TODO: set dynamically
  //         project: "your_project",
  //         username: "admin",
  //         trace: "true",
  //         ...transactions
  //       }
  //     ]
  //   };

  //   console.log("Final JSON:", finalJson);

  //   AxPushtoQueueAPI(
  //     finalJson,
  //     function (res) {
  //       console.log("Success:", res);

  //       // reset store after success
  //       window._multiTstructStore = {};
  //     },
  //     function (err) {
  //       console.error("Error:", err);
  //     }
  //   );
  // }

  // function getTstructJsonLite() {

  //   let recdata = [];
  //   let changedrows = {};
  //   let files = [] // existing Axpert function
  //   let rid = "0";

  //   if ($("#recordid000F0").length > 0) {
  //     rid = $("#recordid000F0").val();
  //   }

  //   DCName.forEach(function (ele) {

  //     let dcNo = ele.substring(2);
  //     let dcFields = GetGridFields(dcNo);

  //     // ðŸ”¹ NON-GRID (dc1)
  //     if (!IsDcGrid(dcNo)) {

  //       let recVal = $("#axp_recid" + dcNo + "000F" + dcNo).val();

  //       let columns = {};

  //       dcFields.forEach(function (fld) {
  //         if (fld.startsWith("axp_recid")) return;

  //         let fldId = fld + "000F" + dcNo;
  //         let val = GetFieldValue(fldId) || "";

  //         columns[fld] = val.replace(/\\/g, '\\\\');
  //       });

  //       recdata.push({
  //         ["axp_recid" + dcNo]: [
  //           {
  //             rowno: "001",
  //             text: recVal || "0",
  //             columns: columns
  //           }
  //         ]
  //       });

  //     }

  //     // ðŸ”¹ GRID (dc2, dc3...)
  //     else {

  //       let rows = [];

  //       $("#gridHd" + dcNo + " tbody tr").each(function () {

  //         let rId = $(this).attr("id");

  //         let rowNo = rId.substring(rId.indexOf('F') - 3, rId.indexOf('F'));
  //         let dbRow = GetDbRowNo(rowNo, dcNo).padStart(3, '0');

  //         let recVal = $("#axp_recid" + dcNo + rowNo + "F" + dcNo).val();

  //         let columns = {};

  //         dcFields.forEach(function (fld) {
  //           if (fld.startsWith("axp_recid")) return;

  //           let fldId = fld + rowNo + "F" + dcNo;
  //           let val = GetFieldValue(fldId) || "";

  //           columns[fld] = val.replace(/\\/g, '\\\\');
  //         });

  //         rows.push({
  //           rowno: dbRow,
  //           text: recVal || "",
  //           columns: columns
  //         });

  //       });

  //       if (rows.length > 0) {
  //         changedrows["dc" + dcNo] = "*";

  //         recdata.push({
  //           ["axp_recid" + dcNo]: rows
  //         });
  //       }

  //     }

  //   });

  //   return {
  //     recordid: rid,
  //     changedrows: changedrows,
  //     recdata: recdata,
  //     files: files
  //   };
  // }

  // function saveTstructBeforeCollapse(iframe) {
  //   try {
  //     const win = iframe.contentWindow;
  //     if (!win) return;

  //     // âœ… only if edited
  //     if (!win.AxGlobalChange) return;

  //     // âœ… call Axpert internal save
  //     const success = win.SaveMultiFrameDataJSON();

  //     if (!success) return;

  //     // âœ… get stored JSON
  //     const multiFormsJsonRaw = win.callParentNew("multiFormsJson");
  //     const multiFormsJson = typeof multiFormsJsonRaw === "string"
  //       ? JSON.parse(multiFormsJsonRaw)
  //       : multiFormsJsonRaw;

  //     const transid = getParamFromUrl(iframe.src, "transid");
  //     const recid = getParamFromUrl(iframe.src, "docid");

  //     const key = `${transid}_${recid}`;

  //     const formJson = multiFormsJson[transid];

  //     // âœ… convert to queue format
  //     const transaction = buildTransactionFromAxpert(formJson, transid);

  //     // ðŸ”¥ store or update
  //     window._multiTstructStore[key] = transaction;

  //     console.log("Stored:", key, transaction);

  //   } catch (e) {
  //     console.warn("save failed", e);
  //   }
  // }

  // function attachIframeChangeListener(iframe, transid, recid) {

  //   try {
  //     const win = iframe.contentWindow;

  //     if (!win || win._listenerAttached) return;

  //     win._listenerAttached = true;

  //     const handler = function () {

  //       try {
  //         if (win.AxGlobalChange) {

  //           console.log("AxGlobalChange TRUE â†’ capturing JSON");

  //           const success = win.SaveMultiFrameDataJSON();
  //           if (!success) return;
            
  //           const multiFormsJsonRaw = win.callParentNew("multiFormsJson");
            
  //           const multiFormsJson = typeof multiFormsJsonRaw === "string"
  //             ? JSON.parse(multiFormsJsonRaw)
  //             : multiFormsJsonRaw;
            
  //           const formJson = multiFormsJson[transid];

  //           const transaction = buildTransactionFromAxpert(
  //             formJson,
  //             transid,
  //             recid
  //           );

  //           const key = recid || transid;

  //           window._multiTstructStore[key] = transaction;

  //           console.log("Stored:", transaction);
  //         }
  //       } catch (e) {
  //         console.error(e);
  //       }
  //     };

  //     // ðŸ”¥ IMPORTANT EVENTS
  //     win.document.addEventListener('input', handler);
  //     win.document.addEventListener('change', handler);
  //     win.document.addEventListener('click', handler);

  //   } catch (e) {
  //     console.error("iframe access error", e);
  //   }
  // }
  // function getParamFromUrl(url, param) {
  //   try {
  //     const u = new URL(url, window.location.origin);
  //     return u.searchParams.get(param);
  //   } catch (e) {
  //     console.warn("Invalid URL:", url);
  //     return null;
  //   }
  // }
  // $(document).on('click', '#submitSmartviewButton', function () {
  //   handleSubmitClick();
  // });
  // $(document).ready(function () {
  //   attachSmartviewTempExpandHandlers();
  // });

  // =============================================================================
  // TASK-5506 | Multi-Tstruct Expand â†’ CachedSave Queue Integration
  // =============================================================================
  // HOW IT WORKS
  // â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
  // 1. User expands a SmartView row  â†’ tstruct loads in an inline iframe.
  // 2. User edits fields / grid rows inside the iframe.
  // 3. On every change (debounced) OR on collapse, getTstructJsonLiteInjected()
  //    is called inside the iframe to read live DOM values â€” no AJAX side-effect.
  // 4. The result is converted to the CachedSave queue format and stored in
  //    window._multiTstructStore  keyed by  `${transid}_${recid}`.
  // 5. On "Submit", handleSubmitClick() assembles all stored transactions into
  //    the _parameters JSON and calls AxPushtoQueueAPI().
  // =============================================================================

  // â”€â”€ Global store (survives collapse/re-expand cycles) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€


  // =============================================================================
  // 1. getTstructJsonLite  â€“  runs INSIDE the tstruct iframe
  // =============================================================================
  // Returns:  { recordid, changedrows, recdata, files }
  // Matches exactly what SaveMultiFrameDataJSON builds, but with zero AJAX calls.
  // We inject this as a string so it runs in the iframe's variable scope where
  // DCName, GetGridFields, IsDcGrid, GetFieldValue, GetDbRowNo are all defined.
  // =============================================================================
  function _buildTstructJsonLiteFn() {
    return function getTstructJsonLiteInjected() {

      var recdata     = [];
      var changedrows = {};
      var rid         = "0";

      // â”€â”€ Record ID (master row) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
      var ridEl = document.getElementById("recordid000F0");
      if (ridEl) rid = ridEl.value || "0";

      // â”€â”€ Walk every DC defined for this tstruct â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
      var dcList = (typeof DCName !== "undefined") ? DCName : [];

      dcList.forEach(function (ele) {
        var dcNo     = ele.substring(2);          // "dc1" â†’ "1"
        var dcFields = GetGridFields(dcNo);        // tstruct.js helper

        // â”€â”€ Non-grid DC (header / master section) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        if (!IsDcGrid(dcNo)) {
          var recEl  = document.getElementById("axp_recid" + dcNo + "000F" + dcNo);
          var recVal = recEl ? (recEl.value || "0") : "0";
          var columns = {};

          dcFields.forEach(function (fld) {
            if (fld.startsWith("axp_recid")) return;
            var val = GetFieldValue(fld + "000F" + dcNo) || "";
            columns[fld] = val.replace(/\\/g, "\\\\");
          });

          var entry = {};
          entry["axp_recid" + dcNo] = [{ rowno: "001", text: recVal, columns: columns }];
          recdata.push(entry);

        // â”€â”€ Grid DC (detail / child section) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        } else {

          // Skip DCs that only have a dummy placeholder row
          var isDummy = false;
          if (typeof gridDummyRowVal !== "undefined" && gridDummyRowVal.length > 0) {
            gridDummyRowVal.forEach(function (v) {
              if (v.split("~")[0] === dcNo) isDummy = true;
            });
          }
          if (isDummy) return;

          changedrows["dc" + dcNo] = "*";   // mark this grid as changed

          var rows   = [];
          var tbody  = document.querySelector("#gridHd" + dcNo + " tbody");

          if (tbody) {
            Array.from(tbody.querySelectorAll("tr")).forEach(function (tr) {
              var rId   = tr.getAttribute("id") || "";
              var fIdx  = rId.indexOf("F");

              // Extract the 3-char row token that sits just before "F"
              var rowNo = (fIdx >= 3) ? rId.substring(fIdx - 3, fIdx) : "000";
              var dbRow = GetDbRowNo(rowNo, dcNo);
              // Pad to 3 digits
              if      (dbRow.length === 1) dbRow = "00" + dbRow;
              else if (dbRow.length === 2) dbRow = "0"  + dbRow;

              var rEl   = document.getElementById("axp_recid" + dcNo + rowNo + "F" + dcNo);
              var recVal = rEl ? (rEl.value || "") : "";
              var columns = {};

              dcFields.forEach(function (fld) {
                if (fld.startsWith("axp_recid")) return;
                var val = GetFieldValue(fld + rowNo + "F" + dcNo) || "";
                columns[fld] = val.replace(/\\/g, "\\\\");
              });

              rows.push({ rowno: dbRow, text: recVal, columns: columns });
            });
          }

          if (rows.length > 0) {
            var gridEntry = {};
            gridEntry["axp_recid" + dcNo] = rows;
            recdata.push(gridEntry);
          }
        }
      });

      return {
        recordid:    rid,
        changedrows: changedrows,
        recdata:     recdata,
        files:       (typeof UploadFiles === "function") ? UploadFiles() : ""
      };
    };
  }

  // =============================================================================
  // 2. Inject getTstructJsonLiteInjected into an iframe's window once it loads
  // =============================================================================
  function injectTstructJsonLiteIntoIframe(iframeEl) {
    try {
      var win = iframeEl.contentWindow;
      if (!win || win._getTstructLiteInjected) return;

      var fnSrc = "(" + _buildTstructJsonLiteFn.toString() + ")()";
      var installed = false;

      try {
        var doc = win.document;
        if (doc && (doc.head || doc.documentElement)) {
          var script = doc.createElement("script");
          script.type = "text/javascript";
          script.text = "window.getTstructJsonLiteInjected=" + fnSrc + ";window._getTstructLiteInjected=true;";
          (doc.head || doc.documentElement).appendChild(script);
          if (script.parentNode) script.parentNode.removeChild(script);
          installed = typeof win.getTstructJsonLiteInjected === "function";
        }
      } catch (_e) {}

      if (!installed) {
        try {
          if (typeof win.eval === "function") {
            win.eval("window.getTstructJsonLiteInjected=" + fnSrc + ";window._getTstructLiteInjected=true;");
            installed = typeof win.getTstructJsonLiteInjected === "function";
          }
        } catch (_e2) {}
      }

      if (!installed) {
        win.getTstructJsonLiteInjected = _buildTstructJsonLiteFn();
        win._getTstructLiteInjected = true;
      }
    } catch (e) {
      // Cross-origin pages will throw â€” that's fine, we fall back below
      console.warn("[MultiTstruct] Could not inject getTstructJsonLite:", e.message);
    }
  }

  function smartviewReadIframeFieldValue(el) {
    if (!el) return "";
    try {
      var tag = (el.tagName || "").toLowerCase();
      var type = (el.type || "").toLowerCase();

      if (type === "checkbox") return el.checked ? "T" : "F";
      if (type === "radio") return el.checked ? String(el.value || "T") : "";

      if (tag === "select") {
        if (el.value !== undefined && el.value !== null) return String(el.value);
        var idx = el.selectedIndex;
        if (idx >= 0 && el.options && el.options[idx]) return String(el.options[idx].value || el.options[idx].text || "");
        return "";
      }

      if (el.value !== undefined && el.value !== null) return String(el.value);
      return String(el.textContent || "");
    } catch (e) {
      return "";
    }
  }

  function smartviewCaptureLiteFromDom(iframeEl, transid, recid) {
    try {
      var win = iframeEl && iframeEl.contentWindow;
      var doc = iframeEl && (iframeEl.contentDocument || (win && win.document));
      if (!doc) return null;

      var rid = "0";
      var ridEl = doc.getElementById("recordid000F0");
      if (ridEl) rid = String(ridEl.value || "0");
      if ((!rid || rid === "0") && recid) rid = String(recid);

      var dcMap = {};
      var fieldRegex = /^([A-Za-z_][A-Za-z0-9_]*)(\d{3})F(\d+)$/;
      var elements = doc.querySelectorAll("input[id],select[id],textarea[id]");

      Array.prototype.forEach.call(elements || [], function (el) {
        var id = String((el && el.id) || "");
        if (!id) return;
        var m = id.match(fieldRegex);
        if (!m) return;

        var fldName = m[1];
        var rowToken = m[2];
        var dcNo = m[3];
        var fldLow = fldName.toLowerCase();

        if (!dcMap[dcNo]) dcMap[dcNo] = {};
        if (!dcMap[dcNo][rowToken]) dcMap[dcNo][rowToken] = { columns: {}, text: "" };

        var rawVal = smartviewReadIframeFieldValue(el);
        var safeVal = String(rawVal || "").replace(/\\/g, "\\\\");

        if (fldLow.indexOf("axp_recid") === 0) {
          dcMap[dcNo][rowToken].text = safeVal;
          return;
        }
        if (fldLow.indexOf("hdn") === 0) return;

        dcMap[dcNo][rowToken].columns[fldName] = safeVal;
      });

      var recdata = [];
      var changedrows = {};
      var dcNos = Object.keys(dcMap).sort(function (a, b) {
        return Number(a) - Number(b);
      });

      dcNos.forEach(function (dcNo) {
        var rowsByToken = dcMap[dcNo] || {};
        var headerRow = rowsByToken["000"] || null;

        if (headerRow && Object.keys(headerRow.columns || {}).length) {
          var headEntry = {};
          headEntry["axp_recid" + dcNo] = [{
            rowno: "001",
            text: (headerRow.text === undefined || headerRow.text === null || headerRow.text === "") ? "0" : String(headerRow.text),
            columns: headerRow.columns
          }];
          recdata.push(headEntry);
        }

        var gridTokens = Object.keys(rowsByToken).filter(function (k) { return k !== "000"; });
        if (!gridTokens.length) return;

        var gridRows = [];
        gridTokens.sort(function (a, b) { return Number(a) - Number(b); }).forEach(function (token) {
          var rowObj = rowsByToken[token] || { columns: {}, text: "" };
          var cols = rowObj.columns || {};
          if (!Object.keys(cols).length) return;
          var rowno = (/^\d+$/.test(token) ? String(token).padStart(3, "0") : String(token));
          gridRows.push({
            rowno: rowno,
            text: (rowObj.text === undefined || rowObj.text === null) ? "" : String(rowObj.text),
            columns: cols
          });
        });

        if (gridRows.length) {
          var gridEntry = {};
          gridEntry["axp_recid" + dcNo] = gridRows;
          recdata.push(gridEntry);
          changedrows["dc" + dcNo] = "*";
        }
      });

      if (!recdata.length) return null;
      return {
        recordid: rid || "0",
        changedrows: changedrows,
        recdata: recdata,
        files: ""
      };
    } catch (e) {
      console.warn("[MultiTstruct] smartviewCaptureLiteFromDom error:", e);
      return null;
    }
  }

  // =============================================================================
  // 3. Capture live tstruct data from an iframe
  // =============================================================================
  // Returns a transaction object ready for window._multiTstructStore, or null.
  // Priority:
  //   a) Injected lite function (no AJAX, no side-effects)  â† preferred
  //   b) Native getTstructJsonLite if the iframe already defines it
  //   c) SaveMultiFrameDataJSON + multiFormsJson fallback   â† last resort
  // =============================================================================
  function captureIframeTstructData(iframeEl, transid, recid) {
    try {
      var win = iframeEl.contentWindow;
      if (!win) return null;
      // ðŸ”¥ IMPORTANT FIX START

      // 1. Force current field to commit value
      if (win.document && win.document.activeElement) {
        win.document.activeElement.blur();
      }

      // 2. Force AX to sync internal values
      if (typeof win.SaveValues === "function") {
        win.SaveValues();
      }

      // 3. Small delay safeguard (optional but useful)
      // (AX sometimes updates async)
      // NOTE: remove if not needed
      // debugger;

      // ðŸ”¥ IMPORTANT FIX END


      var rawData = null;

      // â”€â”€ (a) Injected lite function â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
      if (typeof win.getTstructJsonLiteInjected === "function") {
        rawData = win.getTstructJsonLiteInjected();

      // â”€â”€ (b) Page-native lite function â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
      } else if (typeof win.getTstructJsonLite === "function") {
        rawData = win.getTstructJsonLite();

      // â”€â”€ (c) Fallback: SaveMultiFrameDataJSON â†’ multiFormsJson â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
      } else if (typeof win.SaveMultiFrameDataJSON === "function") {
        win.SaveMultiFrameDataJSON();   // builds & stores to multiFormsJson
        var raw    = win.callParentNew ? win.callParentNew("multiFormsJson") : null;
        var parsed = raw ? (typeof raw === "string" ? JSON.parse(raw) : raw) : {};
        var mf     = parsed[transid];
        if (!mf) return null;

        // multiFormsJson uses "changedDcs" â€” map to "changedrows"
        return {
          transid:     transid        || "",
          afiles:      mf.files       || "",
          trace:       "true",
          recordid:    mf.recordid    || recid || "0",
          changedrows: mf.changedDcs  || {},
          recdata:     mf.recdata     || []
        };

      } else {
        console.warn("[MultiTstruct] No capture method available in iframe.");
        return null;
      }

      // â”€â”€ Wrap rawData (lite format) â†’ queue transaction format â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
      if (!rawData) return null;
      return {
        transid:     transid                  || "",
        afiles:      rawData.files            || "",
        trace:       "true",
        recordid:    rawData.recordid         || recid || "0",
        changedrows: rawData.changedrows      || {},
        recdata:     rawData.recdata          || []
      };

    } catch (e) {
      console.warn("[MultiTstruct] captureIframeTstructData error:", e);
      return null;
    }
  }

  // Override capture to make it resilient and always support DOM-id based extraction
  // (example id: customer000F1 -> field customer, row 000, dc 1)
  function captureIframeTstructData(iframeEl, transid, recid) {
    try {
      var win = iframeEl && iframeEl.contentWindow;
      if (!win) return null;

      if (!transid) transid = getParamFromUrl((iframeEl && iframeEl.src) || "", "transid") || "";
      if (!recid) recid = getParamFromUrl((iframeEl && iframeEl.src) || "", "docid") || getParamFromUrl((iframeEl && iframeEl.src) || "", "recordid") || "";

      try {
        if (win.document && win.document.activeElement) win.document.activeElement.blur();
      } catch (_e) {}
      try {
        if (typeof win.SaveValues === "function") win.SaveValues();
      } catch (_e2) {}

      var rawData = null;

      try {
        if (typeof win.getTstructJsonLiteInjected === "function") {
          rawData = win.getTstructJsonLiteInjected();
        } else if (typeof win.getTstructJsonLite === "function") {
          rawData = win.getTstructJsonLite();
        } else if (typeof win.SaveMultiFrameDataJSON === "function") {
          win.SaveMultiFrameDataJSON();
          var raw = win.callParentNew ? win.callParentNew("multiFormsJson") : null;
          var parsed = raw ? (typeof raw === "string" ? JSON.parse(raw) : raw) : {};
          var mf = parsed[transid];
          if (mf) {
            rawData = {
              recordid: mf.recordid || recid || "0",
              changedrows: mf.changedDcs || {},
              recdata: mf.recdata || [],
              files: mf.files || ""
            };
          }
        }
      } catch (_e3) {}

      if (!rawData || !Array.isArray(rawData.recdata) || !rawData.recdata.length) {
        rawData = smartviewCaptureLiteFromDom(iframeEl, transid, recid) || rawData;
      }
      if (!rawData || !Array.isArray(rawData.recdata) || !rawData.recdata.length) return null;

      return {
        transid: transid || "",
        afiles: rawData.files || "",
        trace: "true",
        recordid: rawData.recordid || recid || "0",
        changedrows: rawData.changedrows || {},
        recdata: rawData.recdata || []
      };
    } catch (e) {
      console.warn("[MultiTstruct] captureIframeTstructData override error:", e);
      return null;
    }
  }

  // =============================================================================
  // 4. Save on collapse  (replaces the old saveTstructBeforeCollapse)
  // =============================================================================
  // Call this BEFORE hiding the expand-row.
  // transid and recid come from the iframe URL params (already parsed outside).
  // =============================================================================
  function saveTstructBeforeCollapse(iframeEl, transid, recid) {
    try {
      var win = iframeEl.contentWindow;
      if (!win) return;

      // Nothing edited â†’ nothing to store
      var changeFlag = !!win.AxGlobalChange;
      if (!changeFlag) return;
      var tId = transid || getParamFromUrl((iframeEl && iframeEl.src) || "", "transid") || "";
      var rId = recid || getParamFromUrl((iframeEl && iframeEl.src) || "", "docid") || getParamFromUrl((iframeEl && iframeEl.src) || "", "recordid") || "";

      var transaction = captureIframeTstructData(iframeEl, tId, rId);
      if (!transaction) {
        if (changeFlag) {
          console.warn("[MultiTstruct] Change flag true but capture returned nothing for", tId, rId);
        } else {
          console.log("[MultiTstruct] No capturable data on collapse for", tId, rId);
        }
        return;
      }

      // Store keyed by `transid_recid` â€” re-expanding same row overwrites (correct)
      var key = (tId && rId)
        ? (tId + "_" + rId)
        : (tId || rId || ("row_" + Date.now()));

      smartviewStorePendingTransaction(key, transaction, tId, rId).then(function () {
        smartviewRefreshGlobalDirtyFlag();
        if (!smartviewIsBulkSaveEnabled() && typeof smartviewRunSaveFlow === "function") {
          smartviewRunSaveFlow({
            mode: "save",
            successMessage: "",
            silentSuccess: true,
            silentNoChanges: true
          }).catch(function (err) {
            console.warn("[MultiTstruct] Auto-save on collapse failed:", err && err.message ? err.message : err);
          });
        }
      });

      // Mark the parent page as having pending changes
      window.AxGlobalChange = true;

      console.log("[MultiTstruct] Stored on collapse â†’", key, transaction);

    } catch (e) {
      console.warn("[MultiTstruct] saveTstructBeforeCollapse error:", e);
    }
  }

  var PENDING_STORE_KEY = "smartview_pending_store";
  var SMARTVIEW_PENDING_DB_NAME = "SmartviewPendingTstructDB";
  var SMARTVIEW_PENDING_DB_VERSION = 3;
  var SMARTVIEW_PENDING_STORE_NAME = "pendingTransactions";
  var SMARTVIEW_FULLDATA_STORE_NAME = "fullDataCache";
  var SMARTVIEW_DRAFT_STORE_NAME = "draftSavePayloads";
  var SMARTVIEW_FULLDATA_MAX_RECORDS = 8;
  var _smartviewPendingDbPromise = null;
  var _smartviewQueueSubmitInProgress = false;

  function smartviewOpenPendingDb() {
    if (typeof indexedDB === "undefined") {
      return Promise.reject(new Error("INDEXED_DB_NOT_AVAILABLE"));
    }
    if (_smartviewPendingDbPromise) return _smartviewPendingDbPromise;

    _smartviewPendingDbPromise = new Promise(function (resolve, reject) {
      try {
        var req = indexedDB.open(SMARTVIEW_PENDING_DB_NAME, SMARTVIEW_PENDING_DB_VERSION);
        req.onupgradeneeded = function (evt) {
          var db = evt.target.result;
          if (!db.objectStoreNames.contains(SMARTVIEW_PENDING_STORE_NAME)) {
            var store = db.createObjectStore(SMARTVIEW_PENDING_STORE_NAME, { keyPath: "key" });
            store.createIndex("transid", "transid", { unique: false });
            store.createIndex("updatedAt", "updatedAt", { unique: false });
          }
          if (!db.objectStoreNames.contains(SMARTVIEW_FULLDATA_STORE_NAME)) {
            var fullStore = db.createObjectStore(SMARTVIEW_FULLDATA_STORE_NAME, { keyPath: "cacheKey" });
            fullStore.createIndex("adsName", "adsName", { unique: false });
            fullStore.createIndex("updatedAt", "updatedAt", { unique: false });
          }
          if (!db.objectStoreNames.contains(SMARTVIEW_DRAFT_STORE_NAME)) {
            var draftStore = db.createObjectStore(SMARTVIEW_DRAFT_STORE_NAME, { keyPath: "key" });
            draftStore.createIndex("adsName", "adsName", { unique: false });
            draftStore.createIndex("updatedAt", "updatedAt", { unique: false });
          }
        };
        req.onsuccess = function () { resolve(req.result); };
        req.onerror = function () { reject(req.error || new Error("INDEXED_DB_OPEN_FAILED")); };
      } catch (e) {
        reject(e);
      }
    });

    return _smartviewPendingDbPromise;
  }

  function smartviewDbPutRecord(record) {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_PENDING_STORE_NAME], "readwrite");
          tx.objectStore(SMARTVIEW_PENDING_STORE_NAME).put(record);
          tx.oncomplete = function () { resolve(record); };
          tx.onerror = function () { reject(tx.error || new Error("INDEXED_DB_PUT_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbGetAllRecords() {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_PENDING_STORE_NAME], "readonly");
          var store = tx.objectStore(SMARTVIEW_PENDING_STORE_NAME);

          if (typeof store.getAll === "function") {
            var req = store.getAll();
            req.onsuccess = function () { resolve(Array.isArray(req.result) ? req.result : []); };
            req.onerror = function () { reject(req.error || new Error("INDEXED_DB_GETALL_FAILED")); };
            return;
          }

          var rows = [];
          var cursorReq = store.openCursor();
          cursorReq.onsuccess = function (evt) {
            var cursor = evt.target.result;
            if (cursor) {
              rows.push(cursor.value);
              cursor.continue();
            } else {
              resolve(rows);
            }
          };
          cursorReq.onerror = function () { reject(cursorReq.error || new Error("INDEXED_DB_CURSOR_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbClearRecords() {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_PENDING_STORE_NAME], "readwrite");
          tx.objectStore(SMARTVIEW_PENDING_STORE_NAME).clear();
          tx.oncomplete = function () { resolve(true); };
          tx.onerror = function () { reject(tx.error || new Error("INDEXED_DB_CLEAR_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbPutDraftRecord(record) {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_DRAFT_STORE_NAME], "readwrite");
          tx.objectStore(SMARTVIEW_DRAFT_STORE_NAME).put(record);
          tx.oncomplete = function () { resolve(record); };
          tx.onerror = function () { reject(tx.error || new Error("INDEXED_DB_DRAFT_PUT_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbGetDraftRecord(key) {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_DRAFT_STORE_NAME], "readonly");
          var req = tx.objectStore(SMARTVIEW_DRAFT_STORE_NAME).get(String(key || ""));
          req.onsuccess = function () { resolve(req.result || null); };
          req.onerror = function () { reject(req.error || new Error("INDEXED_DB_DRAFT_GET_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbDeleteDraftRecord(key) {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_DRAFT_STORE_NAME], "readwrite");
          tx.objectStore(SMARTVIEW_DRAFT_STORE_NAME).delete(String(key || ""));
          tx.oncomplete = function () { resolve(true); };
          tx.onerror = function () { reject(tx.error || new Error("INDEXED_DB_DRAFT_DELETE_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbPutFullDataRecord(record) {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_FULLDATA_STORE_NAME], "readwrite");
          tx.objectStore(SMARTVIEW_FULLDATA_STORE_NAME).put(record);
          tx.oncomplete = function () { resolve(record); };
          tx.onerror = function () { reject(tx.error || new Error("INDEXED_DB_FULLDATA_PUT_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbGetFullDataRecord(cacheKey) {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_FULLDATA_STORE_NAME], "readonly");
          var req = tx.objectStore(SMARTVIEW_FULLDATA_STORE_NAME).get(String(cacheKey || ""));
          req.onsuccess = function () { resolve(req.result || null); };
          req.onerror = function () { reject(req.error || new Error("INDEXED_DB_FULLDATA_GET_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbDeleteFullDataRecord(cacheKey) {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_FULLDATA_STORE_NAME], "readwrite");
          tx.objectStore(SMARTVIEW_FULLDATA_STORE_NAME).delete(String(cacheKey || ""));
          tx.oncomplete = function () { resolve(true); };
          tx.onerror = function () { reject(tx.error || new Error("INDEXED_DB_FULLDATA_DELETE_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbGetAllFullDataRecords() {
    return smartviewOpenPendingDb().then(function (db) {
      return new Promise(function (resolve, reject) {
        try {
          var tx = db.transaction([SMARTVIEW_FULLDATA_STORE_NAME], "readonly");
          var store = tx.objectStore(SMARTVIEW_FULLDATA_STORE_NAME);
          if (typeof store.getAll === "function") {
            var req = store.getAll();
            req.onsuccess = function () { resolve(Array.isArray(req.result) ? req.result : []); };
            req.onerror = function () { reject(req.error || new Error("INDEXED_DB_FULLDATA_GETALL_FAILED")); };
            return;
          }
          var rows = [];
          var cursorReq = store.openCursor();
          cursorReq.onsuccess = function (evt) {
            var cursor = evt.target.result;
            if (cursor) {
              rows.push(cursor.value);
              cursor.continue();
            } else {
              resolve(rows);
            }
          };
          cursorReq.onerror = function () { reject(cursorReq.error || new Error("INDEXED_DB_FULLDATA_CURSOR_FAILED")); };
        } catch (e) {
          reject(e);
        }
      });
    });
  }

  function smartviewDbPruneFullDataRecords(maxCount) {
    var limit = Number(maxCount || SMARTVIEW_FULLDATA_MAX_RECORDS || 8);
    if (!isFinite(limit) || limit < 1) limit = 8;

    return smartviewDbGetAllFullDataRecords().then(function (records) {
      var list = Array.isArray(records) ? records.slice() : [];
      if (list.length <= limit) return true;
      list.sort(function (a, b) {
        return Number(b && b.updatedAt ? b.updatedAt : 0) - Number(a && a.updatedAt ? a.updatedAt : 0);
      });
      var toDelete = list.slice(limit);
      if (!toDelete.length) return true;
      return smartviewOpenPendingDb().then(function (db) {
        return new Promise(function (resolve, reject) {
          try {
            var tx = db.transaction([SMARTVIEW_FULLDATA_STORE_NAME], "readwrite");
            var store = tx.objectStore(SMARTVIEW_FULLDATA_STORE_NAME);
            toDelete.forEach(function (rec) {
              if (rec && rec.cacheKey) store.delete(rec.cacheKey);
            });
            tx.oncomplete = function () { resolve(true); };
            tx.onerror = function () { reject(tx.error || new Error("INDEXED_DB_FULLDATA_PRUNE_FAILED")); };
          } catch (e) {
            reject(e);
          }
        });
      });
    }).catch(function () {
      return false;
    });
  }

  function smartviewGetDcNoFromRecKey(recKey) {
    var raw = (recKey || "").toString().toLowerCase().replace("axp_recid", "").trim();
    return raw || "0";
  }

  function smartviewBuildIndexedShapeFromTransaction(transaction) {
    var tx = transaction || {};
    var tid = (tx.transid || "").toString().trim();
    var shape = {};
    shape[tid || ""] = [];

    var recdata = Array.isArray(tx.recdata) ? tx.recdata : [];
    recdata.forEach(function (dcEntry) {
      if (!dcEntry || typeof dcEntry !== "object") return;
      var recKey = Object.keys(dcEntry)[0] || "";
      if (!recKey) return;

      var rows = Array.isArray(dcEntry[recKey]) ? dcEntry[recKey] : [];
      var dcNo = smartviewGetDcNoFromRecKey(recKey);
      var dcName = "dcname_" + dcNo;
      var isGridDc = rows.length > 1 || !!(tx.changedrows && tx.changedrows["dc" + dcNo]);
      var node = {};

      if (isGridDc) {
        var gridRows = rows.map(function (r) {
          var cols = (r && r.columns && typeof r.columns === "object") ? r.columns : {};
          return {
            rowno: (r && r.rowno) ? String(r.rowno) : "001",
            text: (r && r.text !== undefined && r.text !== null) ? String(r.text) : "",
            columns: smartviewCloneJsonSafe(cols)
          };
        });
        node[dcName] = [{ typeofdc: "grid", rows: gridRows }];
      } else {
        var first = rows[0] || {};
        var firstCols = (first.columns && typeof first.columns === "object") ? first.columns : {};
        node[dcName] = [smartviewCloneJsonSafe(firstCols)];
      }

      shape[tid || ""].push(node);
    });

    return shape;
  }

  function smartviewNormalizeQueueTransaction(transaction, transid, recid) {
    if (!transaction || typeof transaction !== "object") return null;

    var tId = (transaction.transid || transid || "").toString().trim();
    if (!tId) return null;

    var recdata = Array.isArray(transaction.recdata) ? smartviewCloneJsonSafe(transaction.recdata) : [];
    var changedrows = (transaction.changedrows && typeof transaction.changedrows === "object")
      ? smartviewCloneJsonSafe(transaction.changedrows)
      : {};

    return {
      transid: tId,
      afiles: (transaction.afiles === undefined || transaction.afiles === null) ? "" : String(transaction.afiles),
      recordid: (transaction.recordid || recid || "0").toString(),
      changedrows: changedrows,
      recdata: recdata,
      delrows: (transaction.delrows === undefined || transaction.delrows === null) ? "" : String(transaction.delrows),
      trace: "true"
    };
  }

  function smartviewBuildPendingRecord(key, transaction, transid, recid) {
    var tx = smartviewNormalizeQueueTransaction(transaction, transid, recid);
    if (!tx) return null;
    var indexedShape = smartviewBuildIndexedShapeFromTransaction(tx);

    return {
      key: (key || (tx.transid + "_" + (tx.recordid || "0"))).toString(),
      transid: tx.transid,
      recid: (recid || tx.recordid || "0").toString(),
      recordid: (tx.recordid || "0").toString(),
      updatedAt: Date.now(),
      data: indexedShape,
      indexedShape: indexedShape,
      queueTransaction: tx
    };
  }

  function persistMultiStore() {
    try {
      localStorage.setItem(PENDING_STORE_KEY, JSON.stringify(window._multiTstructStore || {}));
    } catch (e) {
      console.warn("Could not persist pending store", e);
    }
  }

  function smartviewSyncPendingArrayFromStore() {
    try {
      var store = window._multiTstructStore || {};
      var arr = [];
      Object.keys(store).forEach(function (key) {
        var tx = smartviewNormalizeQueueTransaction(store[key], store[key] && store[key].transid, store[key] && store[key].recordid);
        if (!tx) return;
        arr.push({
          key: key,
          transid: tx.transid,
          recordid: tx.recordid || "0",
          data: smartviewBuildIndexedShapeFromTransaction(tx),
          queueTransaction: tx
        });
      });
      window._multiTstructQueueArray = arr;
    } catch (e) {
      window._multiTstructQueueArray = [];
    }
  }

  function smartviewStorePendingTransaction(key, transaction, transid, recid) {
    var record = smartviewBuildPendingRecord(key, transaction, transid, recid);
    if (!record) return Promise.resolve(false);

    window._multiTstructStore[record.key] = smartviewCloneJsonSafe(record.queueTransaction);
    persistMultiStore();
    smartviewSyncPendingArrayFromStore();
    smartviewRefreshGlobalDirtyFlag();

    return smartviewDbPutRecord(record).catch(function (e) {
      console.warn("[MultiTstruct] IndexedDB store failed, local fallback kept:", e && e.message ? e.message : e);
      return false;
    });
  }

  function smartviewGetMemoryPendingRecords() {
    var store = window._multiTstructStore || {};
    var records = [];
    Object.keys(store).forEach(function (key) {
      var rec = smartviewBuildPendingRecord(key, store[key], store[key] && store[key].transid, store[key] && store[key].recordid);
      if (rec) records.push(rec);
    });
    records.sort(function (a, b) {
      return Number(a.updatedAt || 0) - Number(b.updatedAt || 0);
    });
    return records;
  }

  function loadMultiStore() {
    try {
      var raw = localStorage.getItem(PENDING_STORE_KEY);
      window._multiTstructStore = raw ? JSON.parse(raw) : {};
    } catch (e) {
      window._multiTstructStore = {};
      try { localStorage.removeItem(PENDING_STORE_KEY); } catch (_e) {}
    }
    smartviewSyncPendingArrayFromStore();

    smartviewDbGetAllRecords().then(function (records) {
      if (!Array.isArray(records) || !records.length) return;
      records.forEach(function (rec) {
        if (!rec || !rec.key || !rec.queueTransaction) return;
        window._multiTstructStore[rec.key] = smartviewCloneJsonSafe(rec.queueTransaction);
      });
      persistMultiStore();
      smartviewSyncPendingArrayFromStore();
      smartviewRefreshGlobalDirtyFlag();
    }).catch(function () {
      // IndexedDB unavailable / blocked â€” localStorage fallback already loaded.
      smartviewSyncPendingArrayFromStore();
      smartviewRefreshGlobalDirtyFlag();
    });
  }

  function smartviewGetAllPendingRecords() {
    return smartviewDbGetAllRecords()
      .then(function (records) {
        var mergedByKey = {};
        var dbRows = Array.isArray(records) ? records : [];
        var memRows = smartviewGetMemoryPendingRecords();

        dbRows.forEach(function (r) {
          if (!r || !r.key) return;
          mergedByKey[r.key] = r;
        });
        memRows.forEach(function (r) {
          if (!r || !r.key) return;
          mergedByKey[r.key] = r;
        });

        var merged = Object.keys(mergedByKey).map(function (k) { return mergedByKey[k]; });
        merged.sort(function (a, b) {
          return Number(a && a.updatedAt ? a.updatedAt : 0) - Number(b && b.updatedAt ? b.updatedAt : 0);
        });
        return merged;
      })
      .catch(function () {
        return smartviewGetMemoryPendingRecords();
      });
  }

  function smartviewClearPendingStore() {
    window._multiTstructStore = {};
    window._multiTstructQueueArray = [];
    try { localStorage.removeItem(PENDING_STORE_KEY); } catch (e) {}
    return smartviewDbClearRecords().catch(function () { return false; }).then(function (res) {
      smartviewRefreshGlobalDirtyFlag();
      return res;
    });
  }

  function smartviewFrameHasPendingChanges(frame) {
    try {
      return !!(frame && frame.contentWindow && frame.contentWindow.AxGlobalChange);
    } catch (e) {
      return false;
    }
  }

  function smartviewCaptureAllOpenFramesToStore() {
    var frames = document.querySelectorAll("iframe.tstruct-frame");
    var promises = [];

    Array.prototype.forEach.call(frames || [], function (frame) {
      try {
        if (!smartviewFrameHasPendingChanges(frame)) return;
        var transid = getParamFromUrl(frame.src, "transid") || "";
        var recid = getParamFromUrl(frame.src, "docid") || getParamFromUrl(frame.src, "recordid") || "";
        var transaction = captureIframeTstructData(frame, transid, recid);
        if (!transaction || !Array.isArray(transaction.recdata) || !transaction.recdata.length) return;

        var key = (transid && recid)
          ? (transid + "_" + recid)
          : (transid || recid || ("row_" + Date.now() + "_" + Math.floor(Math.random() * 10000)));

        promises.push(smartviewStorePendingTransaction(key, transaction, transid, recid));
      } catch (e) {
        console.warn("[MultiTstruct] Failed to capture open iframe transaction:", e);
      }
    });

    return Promise.all(promises).catch(function () { return []; });
  }

  function smartviewBuildTransactionsFromOpenFrames() {
    var out = {};
    var idx = 1;
    var frames = document.querySelectorAll("iframe.tstruct-frame");

    Array.prototype.forEach.call(frames || [], function (frame) {
      try {
        if (!smartviewFrameHasPendingChanges(frame)) return;
        var transid = getParamFromUrl(frame.src, "transid") || "";
        var recid = getParamFromUrl(frame.src, "docid") || getParamFromUrl(frame.src, "recordid") || "";
        var tx = captureIframeTstructData(frame, transid, recid);
        var normalized = smartviewNormalizeQueueTransaction(tx, transid, recid);
        if (!normalized || !normalized.transid || !Array.isArray(normalized.recdata) || !normalized.recdata.length) return;
        out["transaction" + idx] = normalized;
        idx++;
      } catch (e) {
        console.warn("[MultiTstruct] Direct frame capture failed:", e);
      }
    });

    return out;
  }

  function smartviewGetContextValue(candidates, defaultValue) {
    var keys = Array.isArray(candidates) ? candidates : [];
    var scopes = [window, window.parent, window.top];

    for (var s = 0; s < scopes.length; s++) {
      var scope = scopes[s];
      if (!scope) continue;
      for (var i = 0; i < keys.length; i++) {
        var key = keys[i];
        try {
          var val = scope[key];
          if (val !== undefined && val !== null && String(val).trim() !== "") return String(val);
        } catch (e) {}
      }
    }

    if (typeof callParentNew === "function") {
      for (var j = 0; j < keys.length; j++) {
        try {
          var pVal = callParentNew(keys[j]);
          if (pVal !== undefined && pVal !== null && String(pVal).trim() !== "") return String(pVal);
        } catch (e) {}
      }
    }

    return defaultValue || "";
  }

  function smartviewGetProjectFromStorage() {
    try {
      var href = ((window.top && window.top.location && window.top.location.href) || window.location.href || "").toLowerCase();
      var base = href.split("/aspx/")[0];
      if (!base) return "";
      return localStorage.getItem("projInfo-" + base) || "";
    } catch (e) {
      return "";
    }
  }

  function smartviewGetQueueContext() {
    var username = smartviewGetContextValue(["mainUserName", "currentUserName", "username", "userName"], "");
    var project = smartviewGetContextValue(["mainProject", "project", "thmProj", "proj"], "") || smartviewGetProjectFromStorage();
    var sessionid = smartviewGetContextValue(["mainSessionId", "sessionid", "sessionId", "sid", "sId", "nsessionid"], "");
    var appsessionkey = smartviewGetContextValue(["appsessionKey", "appSessionKey", "appsessionkey", "appSKey"], "");
    var ARMSessionId = smartviewGetContextValue(["ARM_SessionId", "ARMSessionId", "armSessionId"], "");

    return {
      ARMSessionId: ARMSessionId,
      project: project,
      username: username,
      appsessionkey: appsessionkey,
      sessionid: sessionid,
      trace: "true"
    };
  }

  function smartviewBuildTransactionsForQueue(records) {
    var out = {};
    var index = 1;
    var list = Array.isArray(records) ? records.slice() : [];

    list.sort(function (a, b) {
      return Number(a && a.updatedAt ? a.updatedAt : 0) - Number(b && b.updatedAt ? b.updatedAt : 0);
    });

    list.forEach(function (rec) {
      var tx = smartviewNormalizeQueueTransaction(
        rec && (rec.queueTransaction || rec.transaction || rec),
        rec && rec.transid,
        rec && (rec.recid || rec.recordid)
      );
      if (!tx || !tx.transid || !Array.isArray(tx.recdata) || !tx.recdata.length) return;
      out["transaction" + index] = tx;
      index++;
    });

    return out;
  }

  function smartviewGetQueuePushApi() {
    var scopes = [window, window.parent, window.top];
    var apiNames = ["AxPushtoQueueAPI", "AxPushToQueueAPI", "AxPushToQueue"];
    for (var s = 0; s < scopes.length; s++) {
      var scope = scopes[s];
      if (!scope) continue;
      for (var i = 0; i < apiNames.length; i++) {
        var apiName = apiNames[i];
        try {
          if (typeof scope[apiName] === "function") return scope[apiName];
        } catch (e) {}
      }
    }
    return null;
  }

  function smartviewPushToQueue(payloadInput) {
    return new Promise(function (resolve, reject) {
      var apiFn = smartviewGetQueuePushApi();
      if (!apiFn) {
        reject(new Error("QUEUE_API_MISSING"));
        return;
      }

      var wrappedPayload = "";
      var rawPayload = "";
      if (payloadInput && typeof payloadInput === "object") {
        wrappedPayload = payloadInput.wrapped || payloadInput.payloadForFunction || "";
        rawPayload = payloadInput.raw || payloadInput.finalJsonString || "";
      } else {
        wrappedPayload = payloadInput || "";
      }
      var firstPayload = wrappedPayload || rawPayload;

      try {
        apiFn(firstPayload, resolve, function (err) {
          if (rawPayload && rawPayload !== firstPayload) {
            try {
              apiFn(rawPayload, resolve, reject);
            } catch (e2) {
              reject(e2);
            }
            return;
          }
          reject(err);
        });
      } catch (e) {
        reject(e);
      }
    });
  }

  // =============================================================================
  // 5. Attach live-capture listener inside iframe  (debounced, input + change)
  // =============================================================================
  function attachIframeChangeListener(iframeEl, transid, recid) {
    try {
      var win = iframeEl.contentWindow;
      if (!win || win._listenerAttached) return;
      win._listenerAttached = true;

      function handler(evt) {
        var target = evt && evt.target;
        if (evt && evt.type === "click" && target) {
          var tg = (target.tagName || "").toUpperCase();
          var isFormControl = (tg === "INPUT" || tg === "SELECT" || tg === "TEXTAREA");
          var isEditable = target.isContentEditable === true;
          if (!isFormControl && !isEditable) return;
        }

        clearTimeout(win._captureDebounce);
        win._captureDebounce = setTimeout(function () {
          var transaction = captureIframeTstructData(iframeEl, transid, recid);
          if (!transaction) {
            if (win.AxGlobalChange) {
              console.warn("[MultiTstruct] Change detected but capture returned empty for", transid, recid);
            }
            return;
          }

          var key = (transid && recid)
            ? (transid + "_" + recid)
            : (transid || recid || "default");

          smartviewStorePendingTransaction(key, transaction, transid, recid);
          window.AxGlobalChange = true;
          smartviewRefreshGlobalDirtyFlag();
          console.log("[MultiTstruct] Live-captured â†’", key);
        }, 400);   // 400 ms debounce â€” adjust if needed
      }

      win.document.addEventListener("input",  handler);
      win.document.addEventListener("change", handler);
      win.document.addEventListener("blur", handler, true);

    } catch (e) {
      console.error("[MultiTstruct] attachIframeChangeListener error:", e);
    }
  }

  // Override: do not depend only on AxGlobalChange; capture from DOM/value APIs on every real edit event.
  function attachIframeChangeListener(iframeEl, transid, recid) {
    try {
      var win = iframeEl && iframeEl.contentWindow;
      if (!win || win._listenerAttached) return;
      win._listenerAttached = true;

      function handler(evt) {
        var target = evt && evt.target;
        if (evt && evt.type === "click" && target) {
          var tg = (target.tagName || "").toUpperCase();
          var isFormControl = (tg === "INPUT" || tg === "SELECT" || tg === "TEXTAREA");
          var isEditable = target.isContentEditable === true;
          if (!isFormControl && !isEditable) return;
        }

        clearTimeout(win._captureDebounce);
        win._captureDebounce = setTimeout(function () {
          var transaction = captureIframeTstructData(iframeEl, transid, recid);
          if (!transaction || !Array.isArray(transaction.recdata) || !transaction.recdata.length) {
            if (win.AxGlobalChange) {
              console.warn("[MultiTstruct] Change detected but capture returned empty for", transid, recid);
            }
            return;
          }

          var tId = transid || getParamFromUrl((iframeEl && iframeEl.src) || "", "transid") || "";
          var rId = recid || getParamFromUrl((iframeEl && iframeEl.src) || "", "docid") || getParamFromUrl((iframeEl && iframeEl.src) || "", "recordid") || "";
          var key = (tId && rId) ? (tId + "_" + rId) : (tId || rId || ("default_" + Date.now()));

          smartviewStorePendingTransaction(key, transaction, tId, rId);
          window.AxGlobalChange = true;
          smartviewRefreshGlobalDirtyFlag();
          console.log("[MultiTstruct] Live-captured ->", key);
        }, 250);
      }

      win.document.addEventListener("input", handler, true);
      win.document.addEventListener("change", handler, true);
      win.document.addEventListener("keyup", handler, true);
      win.document.addEventListener("click", handler, true);
      win.document.addEventListener("blur", handler, true);
    } catch (e) {
      console.error("[MultiTstruct] attachIframeChangeListener override error:", e);
    }
  }

  // =============================================================================
  // 6. Submit all stored transactions â†’ ARMPushToQueue
  // =============================================================================
  // Reads session / project vars from the parent page context.
  // Falls back to empty strings so the JSON is still valid.
  // =============================================================================
  function handleSubmitClickLegacy() {

    // 1. Reset the shared multi-form JSON storage in the parent/global context
    if (typeof callParentNew === "function") {
      callParentNew("multiFormsJson=", "");
    }

    // 2. Iterate through all TStruct iframes and trigger the product serialization logic.
    // This logic (in tstruct.js) constructs recdata and changedrows for each form.
    var frames = document.querySelectorAll("iframe.tstruct-frame");

    frames.forEach(function(frame) {
      try {
        var win = frame.contentWindow;
        if (win && typeof win.SaveMultiFrameDataJSON === "function") {
          win.SaveMultiFrameDataJSON();
        }
      } catch (e) {
        console.warn("Failed to extract data from TStruct iframe:", e);
      }
    });

    // 3. Retrieve the compiled JSON objects from the shared storage
    var multiFormsJsonRaw = typeof callParentNew === "function" ? callParentNew("multiFormsJson") : "";
    var multiFormsJson = {};
    if (multiFormsJsonRaw) {
      multiFormsJson = typeof multiFormsJsonRaw === "string" ? JSON.parse(multiFormsJsonRaw) : multiFormsJsonRaw;
    }

    var transactions = {};
    var index = 1;

    for (var tid in multiFormsJson) {
      var formObj = multiFormsJson[tid];
      // Map the internal form data to the CachedSave queue transaction format.
      // tstruct.js's SaveMultiFrameDataJSON populates formJson with recdata and changedDcs.
      transactions["transaction" + index] = {
        transid: tid,
        recordid: formObj.recordid || "0",
        changedrows: formObj.changedDcs || {}, 
        recdata: formObj.recdata || [],
        afiles: formObj.files || "",
        delrows: formObj.delRows || "",
        trace: "true"
      };
      index++;
    }

    // 4. Validate if any pending changes were captured across all forms
    if (Object.keys(transactions).length === 0) {
      alert("No pending changes found to submit.");
      return;
    }

    // â”€â”€ Read session / project context from the host page â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    // These variables are normally set by Axpert's page initialisation scripts.
    var armSessionId =  "";
    var project      = "emrdev";
                      
    var username     = "admin";
    var sessionid    = "";
    var appSessionKey = "";

    // â”€â”€ Assemble final queue payload â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    var finalJson = {
      _parameters: [
      Object.assign(
        {
          ARMSessionId:  armSessionId,
          project:       project,
          username:      username,
          appsessionkey: appSessionKey,
          sessionid:     sessionid,
          trace:         "true"
        },
        transactions
      )
      ]
    }

    console.log("[MultiTstruct] Submitting â†’", JSON.stringify(finalJson, null, 2));

    // â”€â”€ Push to queue â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    if (typeof AxPushtoQueueAPI !== "function") {
      console.error("[MultiTstruct] AxPushtoQueueAPI is not defined. Check AxInterface.js is loaded.");
      alert("Submit failed: AxPushtoQueueAPI not available.");
      return;
    }
  //   const payload = {
  //     queuename: "CachedSaveQueue",
  //     queuedata: JSON.stringify(finalJson)   // ðŸ”¥ REQUIRED FIX
  // };
  var payloadForFunction = JSON.stringify({
    queuename: "CachedSaveQueue",
    queuedata: JSON.stringify(finalJson)
  });
    AxPushtoQueueAPI(
      payloadForFunction,
      function (res) {
        console.log("[MultiTstruct] Push succeeded:", res);
        window._multiTstructStore = {};   // clear after success
        localStorage.removeItem(PENDING_STORE_KEY);
        window.AxGlobalChange     = false;
        alert("Changes submitted successfully.");
      },
      function (err) {
        console.error("[MultiTstruct] Push failed:", err);
        alert("Submission failed. Please try again.");
      }
    );
  }

  function smartviewGetDraftSaveKey() {
    var adsName = String((window._entity && window._entity.adsName) || (getSmartviewControllerInstance() && getSmartviewControllerInstance().adsName) || "smartview").trim().toLowerCase();
    var pageKey = "";
    try {
      pageKey = String((window.location && window.location.pathname) || "smartview").trim().toLowerCase();
    } catch (e) {
      pageKey = "smartview";
    }
    return pageKey + "::" + adsName;
  }

  function smartviewBuildDraftRecord(bundle) {
    var safeBundle = bundle || {};
    return {
      key: smartviewGetDraftSaveKey(),
      adsName: String((window._entity && window._entity.adsName) || "").trim(),
      bulkSave: smartviewIsBulkSaveEnabled(),
      updatedAt: Date.now(),
      queuePayload: safeBundle.finalJson ? smartviewCloneJsonSafe(safeBundle.finalJson) : null,
      queuePayloadString: safeBundle.finalJsonString || "",
      indexedTransactions: smartviewCloneJsonSafe((safeBundle.records || []).map(function (rec) {
        return {
          key: rec && rec.key ? rec.key : "",
          transid: rec && rec.transid ? rec.transid : "",
          recordid: rec && rec.recordid ? rec.recordid : "",
          indexedShape: rec && rec.indexedShape ? rec.indexedShape : null
        };
      })),
      uploadSnapshot: safeBundle.hasUploads ? smartviewCloneJsonSafe(safeBundle.uploadSnapshot) : null,
      fullRows: safeBundle.hasUploads ? smartviewCloneJsonSafe(safeBundle.fullRows) : [],
      committedRows: safeBundle.hasUploads ? smartviewCloneJsonSafe(window._smartviewCommittedFullData) : [],
      committedUploadSnapshot: safeBundle.hasUploads ? smartviewCloneJsonSafe(window._smartviewCommittedExcelUploadState) : null,
      viewContext: {
        href: String((window.location && window.location.href) || ""),
        title: String(((document.getElementById("EntityTitle") || {}).textContent) || "").trim()
      }
    };
  }

  function smartviewStoreDraftPayload(record) {
    var localKey = "smartview_draft_payload::" + String((record && record.key) || "smartview");
    return smartviewDbPutDraftRecord(record).then(function () {
      try { localStorage.setItem(localKey, JSON.stringify(record)); } catch (e) {}
      return record;
    }).catch(function (err) {
      try {
        localStorage.setItem(localKey, JSON.stringify(record));
        return record;
      } catch (e2) {
        throw (err || e2);
      }
    });
  }

  function smartviewClearDraftPayload() {
    var key = smartviewGetDraftSaveKey();
    var localKey = "smartview_draft_payload::" + key;
    return smartviewDbDeleteDraftRecord(key).catch(function () { return false; }).then(function () {
      try { localStorage.removeItem(localKey); } catch (e) {}
      return true;
    });
  }

  function smartviewCollectSaveBundle(options) {
    var opts = options || {};
    var capturePromise = (opts.captureOpenFrames === false)
      ? Promise.resolve([])
      : smartviewCaptureAllOpenFramesToStore();

    return capturePromise
      .then(function () {
        return smartviewGetAllPendingRecords();
      })
      .then(function (records) {
        window._multiTstructQueueArray = Array.isArray(records) ? records.slice() : [];
        var transactions = smartviewBuildTransactionsForQueue(records);
        if (!Object.keys(transactions).length) {
          var directTransactions = smartviewBuildTransactionsFromOpenFrames();
          if (Object.keys(directTransactions).length) transactions = directTransactions;
        }

        var hasTransactions = Object.keys(transactions).length > 0;
        var hasUploads = !!window._smartviewUploadDirty;
        var context = smartviewGetQueueContext();
        var finalJson = hasTransactions
          ? { _parameters: [Object.assign({}, context, transactions)] }
          : null;

        return {
          records: Array.isArray(records) ? records.slice() : [],
          transactions: transactions,
          hasTransactions: hasTransactions,
          hasUploads: hasUploads,
          context: context,
          finalJson: finalJson,
          finalJsonString: finalJson ? JSON.stringify(finalJson) : "",
          uploadSnapshot: smartviewCloneJsonSafe(window._smartviewExcelUploadState),
          fullRows: smartviewGetCurrentFullRowsSnapshot()
        };
      });
  }

  function smartviewFinalizeSuccessfulSave(clearDraft) {
    return smartviewClearPendingStore()
      .then(function () {
        smartviewResetOpenFrameDirtyFlags();
        window._smartviewUploadDirty = false;
        smartviewMarkCurrentStateCommitted();
        return clearDraft ? smartviewClearDraftPayload() : true;
      })
      .then(function () {
        smartviewRefreshGlobalDirtyFlag();
        return true;
      });
  }

  function smartviewDiscardPendingChanges() {
    return smartviewClearPendingStore()
      .then(function () {
        smartviewResetOpenFrameDirtyFlags();
        return smartviewRestoreCommittedViewState();
      })
      .then(function () {
        window._smartviewUploadDirty = false;
        return smartviewClearDraftPayload();
      })
      .then(function () {
        smartviewRefreshGlobalDirtyFlag();
        return true;
      });
  }

  function smartviewRunSaveFlow(options) {
    var opts = options || {};
    var mode = String(opts.mode || "save").trim().toLowerCase();
    var successMessage = (opts.successMessage === undefined) ? null : opts.successMessage;
    var silentSuccess = !!opts.silentSuccess;
    var silentNoChanges = !!opts.silentNoChanges;

    if (mode === "save" && _smartviewQueueSubmitInProgress) {
      return Promise.reject(new Error("SUBMIT_IN_PROGRESS"));
    }

    if (mode === "save") _smartviewQueueSubmitInProgress = true;

    return smartviewCollectSaveBundle({ captureOpenFrames: opts.captureOpenFrames !== false })
      .then(function (bundle) {
        if (!bundle.hasTransactions && !bundle.hasUploads) {
          throw new Error("NO_PENDING_CHANGES");
        }

        if (mode === "discard") return { bundle: bundle };

        if (mode === "draft") {
          var draftRecord = smartviewBuildDraftRecord(bundle);
          return smartviewStoreDraftPayload(draftRecord).then(function () {
            return { bundle: bundle, draftRecord: draftRecord };
          });
        }

        if (bundle.hasTransactions) {
          console.log("[MultiTstruct] IndexedDB snapshot:", (bundle.records || []).map(function (r) {
            return r && r.indexedShape ? r.indexedShape : null;
          }));
          console.log("[MultiTstruct] Submitting ->", JSON.stringify(bundle.finalJson, null, 2));
          return smartviewPushToQueue({
            wrapped: bundle.finalJsonString,
            raw: bundle.finalJsonString
          }).then(function (res) {
            return { bundle: bundle, queueResponse: res };
          });
        }

        return { bundle: bundle, queueResponse: null };
      })
      .then(function (result) {
        if (mode === "discard") {
          return smartviewDiscardPendingChanges().then(function () { return result; });
        }

        if (mode === "draft") {
          smartviewRefreshGlobalDirtyFlag();
          return result;
        }

        return smartviewFinalizeSuccessfulSave(true).then(function () { return result; });
      })
      .then(function (result) {
        if (!silentSuccess) {
          if (mode === "draft") {
            alert(successMessage === null ? "Changes saved as draft." : successMessage);
          } else if (mode === "discard") {
            alert(successMessage === null ? "Changes discarded." : successMessage);
          } else if (result && result.bundle && result.bundle.hasTransactions) {
            alert(successMessage === null ? "Changes submitted successfully." : successMessage);
          } else {
            alert(successMessage === null ? "Uploaded rows saved successfully." : successMessage);
          }
        }
        return result;
      })
      .catch(function (err) {
        if (err && err.message === "NO_PENDING_CHANGES") {
          if (!silentNoChanges) alert("No pending changes found to submit.");
          return { noChanges: true };
        }
        if (err && err.message === "QUEUE_API_MISSING") {
          if (!silentNoChanges) alert("Submit failed: AxPushtoQueueAPI not available.");
          throw err;
        }
        if (err && err.message === "SUBMIT_IN_PROGRESS") {
          console.warn("[MultiTstruct] Submit already in progress.");
          return { pending: true };
        }
        console.error("[MultiTstruct] Save flow failed:", err);
        if (!silentNoChanges) {
          if (mode === "draft") alert("Draft save failed. Please try again.");
          else if (mode === "discard") alert("Discard failed. Please try again.");
          else alert("Submission failed. Please try again.");
        }
        throw err;
      })
      .then(function (result) {
        if (mode === "save") _smartviewQueueSubmitInProgress = false;
        return result;
      }, function (err) {
        if (mode === "save") _smartviewQueueSubmitInProgress = false;
        throw err;
      });
  }

  function handleSubmitClick() {
    const cfg = smartviewGetActiveViewConfig();
    const isBackgroundSave = !!(cfg && cfg.savemode === 'background');
    smartviewRunSaveFlow({
      mode: "save",
      successMessage: isBackgroundSave ? "Changes queued in background." : "Changes submitted successfully."
    }).catch(function () {});
    return false;
  }

  function attachSmartviewTempExpandHandlers() {
    try {
      if (!window.jQuery) return;

      $(document)
        .off("click.smartviewTempExpand", ".sv-hyperlinktemp")
        .on("click.smartviewTempExpand",  ".sv-hyperlinktemp", function (e) {

          e.preventDefault();
          e.stopPropagation();
          e.stopImmediatePropagation();

          var $icon = $(this);
          var link  = $icon.attr("data-link");
          if (!link) return false;

          var $tr      = $icon.closest("tr");
          var $nextRow = $tr.next(".expand-row");
          var $tableWrap = $tr.closest(".table-responsive");
          var tableScrollLeft = ($tableWrap && $tableWrap.length) ? $tableWrap.scrollLeft() : 0;

          if ($nextRow.length === 0) {
            var colspan = $tr.children("td").length;
            $nextRow = $([
              '<tr class="expand-row">',
              '  <td colspan="' + colspan + '">',
              '    <div class="iframe-loader-wrapper" style="opacity:0;transition:opacity 0.3s ease-in-out;">',
              '      <iframe class="tstruct-frame"',
              '              style="width:100%;height:450px;border:1px solid #eee;',
              '                     box-shadow:0 2px 10px rgba(0,0,0,.1);"></iframe>',
              '    </div>',
              '  </td>',
              '</tr>'
            ].join("")).hide();
            $tr.after($nextRow);
          }

          var $wrapper    = $nextRow.find(".iframe-loader-wrapper");
          var $iframe     = $nextRow.find("iframe");
          var isExpanded  = $nextRow.hasClass("sv-expanded");

          // ── COLLAPSE OR SWITCH LINK
          if (isExpanded) {
            var iframeEl = $iframe[0];
            var currentSrc = iframeEl ? iframeEl.src : '';
            var newUrl = openLinkInPopup(link, true, { tstructMode: "load" }) || '';

            var currentTransid = getParamFromUrl(currentSrc, "transid");
            var currentRecid = getParamFromUrl(currentSrc, "docid");
            var newTransid = getParamFromUrl(newUrl, "transid");
            var newRecid = getParamFromUrl(newUrl, "docid");

            if (newUrl && (currentTransid !== newTransid || currentRecid !== newRecid)) {
              saveTstructBeforeCollapse(iframeEl, currentTransid, currentRecid);
              $wrapper.css("opacity", "0");
              $iframe.attr("src", newUrl);
              
              $(".sv-hyperlinktemp", $tr).removeClass("sv-edit-open");
              if ($icon.hasClass("material-icons")) {
                $icon.addClass("sv-edit-open");
              }
              return false;
            }

            var iTransid = getParamFromUrl(iframeEl.src, "transid");
            var iRecid   = getParamFromUrl(iframeEl.src, "docid");

            saveTstructBeforeCollapse(iframeEl, iTransid, iRecid);

            $nextRow.removeClass("sv-expanded").hide();
            $wrapper.css("opacity", "0");
            if ($icon.hasClass("material-icons")) {
              $icon.text("edit_note");
              $icon.removeClass("sv-edit-open");
            }
            smartviewExitTableFocusMode(false);

          // ── EXPAND 
          } else {
            var url = openLinkInPopup(link, true, { tstructMode: "load" });
            if (url) {
              $wrapper.css("opacity", "0");
              $iframe.attr("src", url);

              $iframe.off("load").on("load", function () {
                var iframeEl = this;
                var iTransid = getParamFromUrl(iframeEl.src, "transid");
                var iRecid   = getParamFromUrl(iframeEl.src, "docid");

                try {
                  var iframeDoc = this.contentDocument || this.contentWindow.document;
                  if (!iframeDoc) return;

                  injectTstructJsonLiteIntoIframe(iframeEl);

                  attachIframeChangeListener(iframeEl, iTransid, iRecid);

                  // â”€â”€ Hide tstruct toolbar / footer inside iframe â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                  var style = iframeDoc.createElement("style");
                  style.innerHTML = [
                    "#dvlayout .footer,",
                    "#dvlayout .toolbar,",
                    "#dvlayout .toolbarRightMenu,",
                    ".breadcrumb-panel { display:none !important; }",
                    "html, body { overflow-x:hidden !important; max-width:100% !important; }"
                  ].join("\n");
                  (iframeDoc.head || iframeDoc.documentElement).appendChild(style);

                  var hideEls = function () {
                    $(iframeDoc)
                      .find("#dvlayout .footer, #dvlayout .toolbar, #dvlayout .toolbarRightMenu")
                      .css("display", "none");
                  };
                  hideEls();

                  var lockIframeXScroll = function () {
                    try {
                      if (iframeDoc.documentElement) iframeDoc.documentElement.scrollLeft = 0;
                      if (iframeDoc.body) iframeDoc.body.scrollLeft = 0;
                      if (iframeEl.contentWindow && iframeEl.contentWindow.scrollX) {
                        iframeEl.contentWindow.scrollTo(0, iframeEl.contentWindow.scrollY || 0);
                      }
                      if ($tableWrap && $tableWrap.length) $tableWrap.scrollLeft(tableScrollLeft);
                    } catch (e) {}
                  };
                  lockIframeXScroll();
                  try {
                    iframeDoc.addEventListener("focusin", function () {
                      setTimeout(lockIframeXScroll, 0);
                    }, true);
                  } catch (e) {}

                  var obs = new MutationObserver(hideEls);
                  obs.observe(iframeDoc.body, { childList: true, subtree: true });

                  setTimeout(function () { $wrapper.css("opacity", "1"); }, 50);

                } catch (err) {
                  // Cross-origin â€” just show it
                  console.warn("[MultiTstruct] iframe DOM access failed:", err.message);
                  $wrapper.css("opacity", "1");
                }
              });
            }

            $nextRow.addClass("sv-expanded").show();
            if ($icon.hasClass("material-icons")) {
              $icon.text("edit_note");
              $icon.addClass("sv-edit-open");
            }
            smartviewEnterTableFocusMode();
          }

          return false;
        });

    } catch (e) {
      console.error("[MultiTstruct] attachSmartviewTempExpandHandlers error:", e);
    }
  }

  // =============================================================================
  // 8. Utility â€” unchanged, kept here for self-containment
  // =============================================================================
  function getParamFromUrl(url, param) {
    try {
      var u = new URL(url, window.location.origin);
      var val = u.searchParams.get(param);
      if (val) return val;
      var p = String(param || "").toLowerCase();
      if (p === "transid") return u.searchParams.get("tstname");
      if (p === "tstname") return u.searchParams.get("transid");
      return val;
    } catch (e) {
      console.warn("[MultiTstruct] Invalid URL:", url);
      return null;
    }
  }

  // =============================================================================
  // 9. Wire up submit button & init
  // =============================================================================
  $(document).on("click", "#submitSmartviewButton", function () {
    return handleSubmitClick();
  });

  $(document).ready(function () {
    loadMultiStore();
    smartviewRestoreDraftStateMarker();
    smartviewRefreshGlobalDirtyFlag();
    attachSmartviewTempExpandHandlers();
    initSmartviewExcelUpload();
    smartviewInitNavigationGuard();
  });

  // =============================================================================
  // EXAMPLE of final JSON pushed to ARMPushToQueue (matches ReadMe option 2):
  // =============================================================================
  // {
  //   "_parameters": [{
  //     "ARMSessionId": "<from Signin API>",
  //     "project": "goldendump114",
  //     "username": "admin",
  //     "appsessionkey": "",
  //     "sessionid": "",
  //     "trace": "true",
  //     "transaction1": {
  //       "transid":     "forma",
  //       "afiles":      "",
  //       "trace":       "true",
  //       "recordid":    "0",
  //       "changedrows": {},
  //       "recdata": [
  //         { "axp_recid1": [{ "rowno":"001","text":"0","columns":{"flda":"test","fldb":"admin"} }] }
  //       ]
  //     },
  //     "transaction2": { ... },
  //     ...
  //   }]
  // }
