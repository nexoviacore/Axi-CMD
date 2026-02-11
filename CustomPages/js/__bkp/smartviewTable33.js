/* smartviewTable.js - Complete integration for ds_smartlist_users -> createTableViewHTML */
/* Now includes ADS picker modal on page load */

/* --------------------------
   Minimal global entity stubs
   -------------------------- */

window._smartviewFullData = null;

window._entity = window._entity || {
  metaData: [
    { fldname: "transid", fldcap: "Trans ID", fdatatype: "t", cdatatype: "Text", listingfld: "T" },
    { fldname: "recordid", fldcap: "Record ID", fdatatype: "t", cdatatype: "Text", listingfld: "T" },
    { fldname: "username", fldcap: "Username", fdatatype: "t", cdatatype: "Text", listingfld: "T" },
    { fldname: "nickname", fldcap: "Nickname", fdatatype: "t", cdatatype: "Text", listingfld: "T" },
    { fldname: "email", fldcap: "Email", fdatatype: "t", cdatatype: "Text", listingfld: "T" }
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

window._entityCommon = window._entityCommon || {
  inValid: function (v) { return v === null || v === undefined || (typeof v === "string" && v.trim() === ""); }
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
  
  // Initialize controller with selected ADS
  if (window.smartTableController) {
    // If controller already exists, update it
    window.smartTableController.adsName = selectedAdsName;
    window.smartTableController.resetPaging();
    window.smartTableController.loadNextPage();
  } else {
    // Create new controller
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

function initializeDataTable() {
  try {
    if (window.jQuery && $.fn && $.fn.DataTable) {
      $('#table-body_Container .table').DataTable({
        paging: false,
        searching: false,
        info: false,
        destroy: true
      });
    }
  } catch (e) {
    // no-op if DataTables not present
  }
}

/* --------------------------
   createTableViewHTML
   -------------------------- */

function createTableViewHTML(listJson, _pageNo) {
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

const filteredMetaData = (_entity.metaData || [])
  .filter(item => item.listingfld === "T")
  .map(item => {
    // return a shallow copy with a lowercase fldname for robust matching
    return Object.assign({}, item, { fldname: (item.fldname || '').toString().toLowerCase() });
  });  const fieldsWithData = new Set();
  const addedFields = new Set();

  const dynamicFields = ['modifiedby', 'modifiedon', 'createdby', 'createdon'];

  if (_entity.modificationFields && typeof _entity.modificationFields === 'string') {
    const modificationFieldsArray = _entity.modificationFields.split(",");

    if (modificationFieldsArray.length > 0) {
      dynamicFields.forEach(field => {
        if (modificationFieldsArray.includes(field)) {
          const fieldMeta = {
            fldname: field,
            fldcap: formatFieldName(field),
            listingfld: "T"
          };
          filteredMetaData.push(fieldMeta);
        }
      });
    }
  } else {
    dynamicFields.forEach(field => {
      if (listJson.some(rowData => rowData[field])) {
        const fieldMeta = {
          fldname: field,
          fldcap: formatFieldName(field),
          listingfld: "T"
        };
        filteredMetaData.push(fieldMeta);
      }
    });
  }

listJson.forEach(rowData => {
  for (let field in rowData) {
    if (rowData[field] !== null && rowData[field] !== undefined && !excludedFields.has(field)) {
      fieldsWithData.add(String(field).toLowerCase());
    }
  }
});


  if (!keyCol || !_entity.metaData.some(field => field.fldname.toLowerCase() === keyCol.toLowerCase())) {
    const keyField = getKeyField();
    keyCol = keyField ? keyField.fldname : _entity.keyField;
  }

  if (!tableExists) {
    html += '<div class="table-responsive"><table class="table table-striped">';

    html += '<thead class="sticky-header"><tr>';
    html += '<th><input type="checkbox" id="selectAllCheckbox" onclick="toggleSelectAll(this)"></th>';
  

    if (fieldsWithData.has(keyCol?.toLowerCase())) {
      const keyField = _entity.metaData.find(field => field.fldname.toLowerCase() === keyCol.toLowerCase());
      if (keyField) {
        html += `<th>${keyField.fldcap}</th>`;
      }
    } else {
      html += `<th>--</th>`;
    }

    filteredMetaData.forEach(field => {
      if (field.fldname.toLowerCase() !== keyCol.toLowerCase() && fieldsWithData.has(field.fldname.toLowerCase()) && !excludedFields.has(field.fldname) && (!hideTransid || field.fldname.toLowerCase() !== 'transid')) {
        if (!addedFields.has(field.fldname)) {
          html += `<th>${field.fldcap}</th>`;
          addedFields.add(field.fldname);
        }
      }
    });
    html += '</tr></thead><tbody>';
  } else {
    tableBodyContainer.find('tbody').empty();
  }

  _entity.navigationRecords = [];
  listJson.forEach((rowData, index) => {
    html += `<tr>`;
    html += `<td><input type="checkbox" class="rowCheckbox" data-index="${index}" data-recordid="${rowData.recordid}"></td>`;

    const entityName = _entity.entityName;
    const transId = _entity.entityTransId || rowData.transid || "axusr";
    const recordId = rowData.recordid;
    _entity.navigationRecords.push(recordId);
    const rowNo = rowData.rno;

    if (fieldsWithData.has(keyCol.toLowerCase())) {
      var keyValue = (rowData[keyCol.toLowerCase()]?.toString()) ?? "";
      var keyColProps = filteredMetaData.find(x => x.fldname.toLowerCase() == keyCol.toLowerCase());
      if (keyColProps && (keyColProps.fdatatype === 'd' || keyColProps.cdatatype === 'Date')) keyValue = formatDateString(keyValue);

      html += `<td><a href="#" onclick="_entity.openEntityForm('${entityName}','${transId}', '${recordId}', '${(keyValue||"").replace(/\\/g, "\\\\")}', ${rowNo})">${_entityCommon.inValid(keyValue) ? "--" : keyValue.replace(/\\/g, "\\\\") }</a></td>`;
    } else {
      html += `<td><a href="#" onclick="_entity.openEntityForm('${entityName}','${transId}', '${recordId}', '', ${rowNo})">--</a></td>`;
    }

    filteredMetaData.forEach(field => {
      if (field.fldname.toLowerCase() !== keyCol.toLowerCase() && fieldsWithData.has(field.fldname.toLowerCase()) && !excludedFields.has(field.fldname) && (!hideTransid || field.fldname.toLowerCase() !== 'transid')) {
        let fieldValue = rowData[field.fldname.toLowerCase()];
        let cellContent = '';

        if (field.cdatatype === 'Check box' || field.fdatatype === 'Check box') {
          cellContent = `<div class="form-check"><input class="form-check-input" type="checkbox" ${fieldValue === 'T' ? 'checked' : ''} disabled></div>`;
        } else if ((field.fldname.toLowerCase() === 'modifiedon' || field.fldname.toLowerCase() === 'createdon' || field.fdatatype === 'd' || field.cdatatype === 'Date') && fieldValue) {
          cellContent = formatDateString(fieldValue);
        } else if (field.cdatatype === 'Currency' || field.fdatatype === 'Currency') {
          cellContent = formatNumberBasedOnMillions(fieldValue);
        } else {
          cellContent = `${fieldValue || ''}`;
        }

        html += `<td>${cellContent}</td>`;
      }
    });

    html += `</tr>`;
  });

  if (!tableExists) {
    html += '</tbody></table></div>';
    tableBodyContainer.empty().append(html);
    initializeDataTable();
  } else {
    const dataTableInstance = $('#table-body_Container .table').DataTable ? $('#table-body_Container .table').DataTable() : null;
    if (dataTableInstance && $.fn.DataTable) {
      dataTableInstance.destroy();
      tableBodyContainer.find('tbody').empty();
    }
    tableBodyContainer.find('tbody').append(html);
    initializeDataTable();
  }
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

    // --- page title (best-effort)
    const pageTitle =
      (dsBlock && (dsBlock.adsname || dsBlock.adsName)) ||
      (parsedObj?.result?.ADSNames && parsedObj.result.ADSNames[0]) ||
      (parsedObj?.ADSNames && parsedObj.ADSNames?.[0]) ||
      (window.smartTableController && window.smartTableController.adsName) ||
      '';

    const titleEl = document.getElementById('EntityTitle') || document.querySelector('.page-header-title');
    if (titleEl && pageTitle) titleEl.textContent = pageTitle;
    else if (pageTitle) document.title = pageTitle;

    // --- ensure _entity exists and set basic state
    window._entity = window._entity || {};
    if (pageTitle) window._entity.adsName = pageTitle;
const pageSz = Number(pageSize ?? window._entity.pageSize ?? 0);
if (pageSz > 0) {
  // only render the first page (pageNo may be 1 on initial load)
  const fromIndex = ((Number(pageNo) || 1) - 1) * pageSz;
  window._entity.listJson = Array.isArray(rows) ? rows.slice(fromIndex, fromIndex + pageSz) : [];
} else {
  // pagesize === 0 means "render all"
  window._entity.listJson = Array.isArray(rows) ? rows.slice() : [];
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
      rowCountManager.setTotal(totalRecords);
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
  const fieldsWithData = [];

  // choose fields based on _entity.metaData and whether there's data in _entity.listJson
  (_entity.metaData || []).forEach(field => {
    // skip if explicitly hidden (Entity uses field.hide === 'T')
    if (field.hide === 'T' || (field.listingfld && field.listingfld !== 'T')) return;

    const key = (field.fldname || field.fldname === 0) ? field.fldname.toLowerCase() : '';
    const hasData = (_entity.listJson || []).some(row => {
      const v = row[key];
      return v !== null && v !== undefined && String(v).trim() !== '';
    });

    // keep only columns that have data (Entity logic) -- avoids empty columns in export
    if (hasData) {
      fieldsWithData.push(field);
      tableHtml += `<th>${field.fldcap || formatFieldName(field.fldname)}</th>`;
    }
  });

  tableHtml += `</tr></thead><tbody>`;

  (_entity.listJson || []).forEach(row => {
    tableHtml += '<tr>';
    fieldsWithData.forEach(field => {
      const key = field.fldname.toLowerCase();
      let cell = row[key];
      if (cell === null || cell === undefined) cell = '';
      // small formatting for dates or booleans can be added here if required
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

  // If Word or very large dataset, you might queue server-side export (Entity uses ASB.WebService.ARMExportPushToQueue)
  // For now, mimic client-side export like Entity: create hidden table and trigger DataTables button.
  try {
    // ensure container and hidden table built from current metadata + listJson
    createHiddenTableFromMetadata();

    // destroy if exists
    if ($.fn.dataTable && $.fn.dataTable.isDataTable('#hiddenTable')) {
      $('#hiddenTable').DataTable().destroy();
    }

    // initialize hidden table with Buttons
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

    // trigger corresponding button
    switch (action) {
      case 'PRINT': hidden.button('.buttons-print').trigger(); break;
      case 'PDF':   hidden.button('.buttons-pdf').trigger();   break;
      case 'EXCEL': hidden.button('.buttons-excel').trigger(); break;
      case 'CSV':   hidden.button('.buttons-csv').trigger();   break;
      case 'COPY':  hidden.button('.buttons-copy').trigger();  break;
      default:
        // fallback: try pdf then excel
        if (hidden.button('.buttons-pdf').length) hidden.button('.buttons-pdf').trigger();
        else if (hidden.button('.buttons-excel').length) hidden.button('.buttons-excel').trigger();
    }

    // cleanup can be left (DataTables will be destroyed when next createHiddenTableFromMetadata runs)
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
const searchInput = document.getElementById("searchBox");
const liveSearchDebounced = debounce(liveSearch, 500);

function handleSearchInput() {
    if (searchInput.value === "") {
        liveSearch();
    } else {
        liveSearchDebounced();
    }
}

searchInput.addEventListener("keyup", handleSearchInput);
searchInput.addEventListener("input", handleSearchInput);
document.getElementById("searchBox").addEventListener("keypress", function (event) {
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

      // notify if none found (use simple fallback)
      if (term !== '' && document.querySelectorAll('#table-body_Container tbody tr:not(.d-none)').length === 0) {
        // fallback to console if you don't want an alert
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

// --- helper: safe instantiate controller & auto-load ---
function startSmartTableFromAdsName(adsName) {
  if (!adsName) {
    console.warn('startSmartTableFromAdsName: no adsName supplied');
    return false;
  }

  console.log('Starting SmartViewTableController with ADS from query param:', adsName);

  if (window.smartTableController) {
    window.smartTableController.adsName = adsName;
    window.smartTableController.resetPaging();
    window.smartTableController.loadNextPage();
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
    this.refreshCache = false;

    this.isFetching = false;
    this.totalCount = 0;
    this.loadedCount = 0;

    this.init();
  }

  init() {
    window._entity = window._entity || {};
    window._entity.listJson = [];
    window._entity.pageSize = this.pageSize;

    this.wireDom();
    this.setupSortingHeaders();
    this.loadNextPage();

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

  /*************** Search bindings (Entity-like) ***************/
  // Debounced live search so typing doesn't spam reflows
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
    this.pageSize = 100;
    ["empCodeFilter","salaryFrom","salaryTo","dojFrom","dojTo"].forEach(id => { const el = document.getElementById(id); if (el) el.value = ""; });
    const dept = document.getElementById("deptFilter"); if (dept) dept.selectedIndex = -1;
  }

  resetPaging() {
    this.pageno = 1;
    this.isFetching = false;
    this.totalCount = 0;
    this.loadedCount = 0;
    window._entity.listJson = [];
    window._entity.pageSize = this.pageSize;
  }

  buildParams(pageNo = 1) {
    const sqlParams = {};
    return {
      adsNames: [this.adsName],
      refreshCache: this.refreshCache,
      sqlParams: sqlParams,
      props: {
        ADS: true,
        getallrecordscount: false,
        pageno: pageNo,
        pagesize: this.pageSize,
        sorting: this.sorting,
        filters: this.filters
      }
    };
  }

 loadNextPage() {
  // Block until ads selected (if your controller uses that)
  if (this.requiresAdsSelection && !this.adsSelected) return;

  // Do not run in parallel
  if (this.isFetching) {
    console.log('loadNextPage: aborted because already fetching');
    return;
  }

  // If known total and already loaded, stop
  if (this.totalCount > 0 && this.loadedCount >= this.totalCount) {
    console.log('loadNextPage: all records loaded (loadedCount, totalCount)=', this.loadedCount, this.totalCount);
    return;
  }

  // If we already cached all rows client-side, serve next slice
  if (Array.isArray(window._smartviewFullData) && window._smartviewFullData.length > 0) {
    const start = (this.pageno - 1) * this.pageSize;
    const slice = window._smartviewFullData.slice(start, start + this.pageSize);
    if (!slice.length) {
      console.log('loadNextPage: no more cached rows to append');
      return;
    }
    window._entity.listJson = (window._entity.listJson || []).concat(slice);
    this.loadedCount = window._entity.listJson.length;
    console.log('loadNextPage: appended client-cached slice, new loadedCount=', this.loadedCount);
    if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, this.pageno);
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
      const parsed = (typeof response === 'string') ? JSON.parse(response) : response;
      const dsBlock = parsed?.result?.data?.[0] || null;
      const rows = Array.isArray(dsBlock?.data) ? dsBlock.data : [];
      const totalFromServer = Number(dsBlock?.totalrecords ?? dsBlock?.recordcount ?? 0) || 0;

      console.log('loadNextPage: server returned rows=', rows.length, 'totalFromServer=', totalFromServer, 'for requested page', requestPage);

      // If server returned *pages* (rows.length <= pagesize) treat it as server-paged
      if (rows.length > 0 && rows.length <= this.pageSize && totalFromServer > 0) {
        // server is doing paging: append server page
        if (requestPage === 1) {
          window._entity.listJson = rows.slice();
        } else {
          window._entity.listJson = (window._entity.listJson || []).concat(rows);
        }
        this.loadedCount = window._entity.listJson.length;
        this.totalCount = totalFromServer;
        if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, requestPage);
        // increment page for next time (after success)
        this.pageno = requestPage + 1;
        console.log('loadNextPage: server-paged append complete, pageno set to', this.pageno);
        // If we've now loaded all known total, stop further requests
        if (this.loadedCount >= this.totalCount) {
          console.log('loadNextPage: reached totalCount, stopping further loads');
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
        if (typeof createTableViewHTML === 'function') createTableViewHTML(window._entity.listJson, requestPage);
        // next page will be served from cached data
        this.pageno = requestPage + 1;
        console.log('loadNextPage: cached-full fallback used, cached rows=', rows.length, 'pageno now', this.pageno);
        return;
      }

      // If no rows returned, probably no data — stop
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
}x
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

  // Prefer the inner scrolling element (.table-responsive) if it exists
  let scrollEl = container.querySelector('.table-responsive') || container;
  // If still not overflowing, fall back to window
  const style = window.getComputedStyle(scrollEl);
  if (!(style.overflowY === 'auto' || style.overflowY === 'scroll') || scrollEl.scrollHeight <= scrollEl.clientHeight) {
    scrollEl = window;
  }

  // remove any previous handler
  if (this._scrollHandler && this._scrollTarget) {
    try { this._scrollTarget.removeEventListener('scroll', this._scrollHandler); } catch (e) {}
  }

  // debounce guard
  let debounceTimer = null;
  const thresholdPx = 150;
  this._scrollHandler = (ev) => {
    if (debounceTimer) clearTimeout(debounceTimer);
    debounceTimer = setTimeout(() => {
      try {
        if (this.isFetching) return;
        if (this.pageSize === 0) return; // loadAll mode

        let nearBottom = false;
        if (scrollEl === window) {
          const scrolled = window.innerHeight + window.scrollY;
          const full = document.documentElement.scrollHeight;
          nearBottom = (full - scrolled) <= thresholdPx;
        } else {
          nearBottom = (scrollEl.scrollHeight - (scrollEl.scrollTop + scrollEl.clientHeight)) <= thresholdPx;
        }

        if (nearBottom) {
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
  console.log('attachScrollListener attached to', scrollEl === window ? 'window' : (scrollEl.id || scrollEl.className || scrollEl.tagName));
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



/* --------------------------
   Initialize on page load with ADS picker
   -------------------------- */

document.addEventListener('DOMContentLoaded', function () {
  const adsFromQuery = getQueryParam('ads') || getQueryParam('adsName') || getQueryParam('adsname');

  if (adsFromQuery) {
    const started = startSmartTableFromAdsName(adsFromQuery);
    if (!started) {
      setTimeout(() => {
        showAdsPickerModal();
      }, 250);
    }
  } else {
    setTimeout(() => {
      showAdsPickerModal();
    }, 300);
  }

  console.log('SmartViewTableController boot logic executed (adsFromQuery=', adsFromQuery, ')');
});