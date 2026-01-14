/*!
 * Print button for Buttons and DataTables.
 * 2016 SpryMedia Ltd - datatables.net/license
 */

/**
 * Datatable Support for Export(Print, PDF, Excel) Additional Functionalities
 * @author Prashik
 * @Date   2020-05-28T12:08:56+0530
 */
(function( factory ){
	if ( typeof define === 'function' && define.amd ) {
		// AMD
		define( ['jquery', 'datatables.net', 'datatables.net-buttons'], function ( $ ) {
			return factory( $, window, document );
		} );
	}
	else if ( typeof exports === 'object' ) {
		// CommonJS
		module.exports = function (root, $) {
			if ( ! root ) {
				root = window;
			}

			if ( ! $ || ! $.fn.dataTable ) {
				$ = require('datatables.net')(root, $).$;
			}

			if ( ! $.fn.dataTable.Buttons ) {
				require('datatables.net-buttons')(root, $);
			}

			return factory( $, root, root.document );
		};
	}
	else {
		// Browser
		factory( jQuery, window, document );
	}
}(function( $, window, document, undefined ) {
'use strict';
var DataTable = $.fn.dataTable;


var _link = document.createElement( 'a' );

/**
 * Clone link and style tags, taking into account the need to change the source
 * path.
 *
 * @param  {node}     el Element to convert
 */
var _styleToAbs = function( el ) {
	var url;
	var clone = $(el).clone()[0];
	var linkHost;

	if ( clone.nodeName.toLowerCase() === 'link' ) {
		clone.href = _relToAbs( clone.href );
	}

	return clone.outerHTML;
};

/**
 * Convert a URL from a relative to an absolute address so it will work
 * correctly in the popup window which has no base URL.
 *
 * @param  {string} href URL
 */
var _relToAbs = function( href ) {
	// Assign to a link on the original page so the browser will do all the
	// hard work of figuring out where the file actually is
	_link.href = href;
	var linkHost = _link.host;

	// IE doesn't have a trailing slash on the host
	// Chrome has it on the pathname
	if ( linkHost.indexOf('/') === -1 && _link.pathname.indexOf('/') !== 0) {
		linkHost += '/';
	}

	return _link.protocol+"//"+linkHost+_link.pathname+_link.search;
};


DataTable.ext.buttons.print = {
	className: 'buttons-print',

	text: function ( dt ) {
		return dt.i18n( 'buttons.print', 'Print' );
	},

	action: function (e, dt, button, config) {
		this.processing(true);
		var data = dt.buttons.exportData(
			$.extend( {decodeEntities: false}, config.exportOptions ) // XSS protection
		);
		var exportInfo = dt.buttons.exportInfo( config );
		var columnClasses = dt
			.columns( config.exportOptions.columns )
			.flatten()
			.map( function (idx) {
				return dt.settings()[0].aoColumns[dt.column(idx).index()].sClass;
			} )
			.toArray();

		var addRow = function ( d, tag, attr, addClass ) {
			if (typeof attr == "undefined") {
				attr = "";
			}

			let tagInfo = tag.split("~");
			let calculatePivotColspan = false;

			if(tagInfo.length == 2){
				tag = tagInfo[0];
				if(tagInfo[1] == "pivotcolspan"){
					calculatePivotColspan = true;
				}
			}

			if (typeof addClass == "undefined") {
				addClass = "";
			}

			var isSpecialRow = (addClass == "fontBlue" || addClass == "fontBlack" || addClass == "fontGreen");

			var str = '<tr '+ (isSpecialRow && 'class="specialRow"') +'>';

			let rowCellsArray = [];
			let lastData = undefined;
			let thisColSpan = 1;
			
			for ( var i=0, ien=d.length ; i<ien ; i++ ) {
				// null and undefined aren't useful in the print output
				var dataOut = d[i] === null || d[i] === undefined ? '' : d[i];
				var tempColumnClass = columnClasses[i] || "";

				if(addClass.split(" ").filter(dt=>dt.indexOf("dt-") == 0).length > 0){
					tempColumnClass = tempColumnClass.split(" ").filter(dt=>dt.indexOf("dt-") != 0).join(" ");
				}

				var classAttr = 'class="'+(tempColumnClass || "")+' '+ addClass +'"';

				if(calculatePivotColspan && lastData !== undefined && lastData !== "" && dataOut === lastData){
					rowCellsArray.pop();
					
					thisColSpan++;
					
					rowCellsArray.push('<'+tag+' '+ `class="dt-center ${addClass}"` +' '+ attr + ` colspan=${thisColSpan} ` +'>'+dataOut+'</'+tag+'>');

				} else {
					thisColSpan = 1;
					
					rowCellsArray.push('<'+tag+' '+classAttr+' '+ attr +'>'+dataOut+'</'+tag+'>');
				}
				
				
				lastData = dataOut;
			}

			// lastData = undefined;
            // thisColSpan = 1;
			// rowCellsArray = [];

			return str + rowCellsArray.join("") + '</tr>';
		};

		var colSpan = 1;

		var html = "";

		if (data && data.header && data.header.length) {
			colSpan = data.header.length;
		}
		
		if (config && config.exportHF) {
			let hideExportAppTitle = typeof config.exportHF.hideprojname !== "undefined" && config.exportHF.hideprojname.substr(1).toLowerCase() === "t" || false; // default value false

			if ((!hideExportAppTitle && config.exportHF.exportAppTitle) || (config.exportHF.header && Object.keys(config.exportHF.header).length > 0)) {
				html += '<table class="'+dt.table().node().className+' exportHF">';
			}

			if (!hideExportAppTitle && config.exportHF.exportAppTitle) {
				html += ''+ addRow( [config.exportHF.exportAppTitle], 'th', 'colspan="'+ colSpan +'"', 'dt-center' ) +'';
			}

			if(config.exportHF.header && Object.keys(config.exportHF.header).length > 0){
				let alignClass = 'dt-center'; // Default is center alignment
				const headerAlign = config.exportHF.header["h1"]?.header_aline;
				if (headerAlign) {
					alignClass = { "left": "dt-left", "center": "dt-center", "right": "dt-right" }[headerAlign.substr(1).toLowerCase()] || "dt-center";
				}
				Object.keys(config.exportHF.header).forEach(function(val){
					if (config.exportHF.header[val].text?.trim()) {
						html += ''+ addRow( [config.exportHF.header[val].text?.trim()], 'th', 'colspan="'+ colSpan +'"', alignClass ) +'';
					}
				});
			}

			// if ((!hideExportAppTitle && config.exportHF.exportAppTitle) || (config.exportHF.header && Object.keys(config.exportHF.header).length > 0)) {
			// 	html += ''+ addRow( [""], 'td', 'colspan="'+ colSpan +'"', 'dt-center' ) +'' + '</table>';
			// }
		}

		var paramString = processParamString().trim().replace(/, /g, "; ");
		if (paramString) {
			html += '<table class="' + dt.table().node().className + '">';
			html += ''+ addRow( [paramString], 'td', 'colspan="'+ colSpan +'"', 'dt-left' ) +'';
			html += '</table>';
		}

		// Construct a table for printing
		html += '<table class="'+dt.table().node().className+'">';

		if ( config.header ) {
			html += '<thead>';
			var headerMatrix = _fnGetHeaders(dt);
			let pivotColSpan = false;
			for ( var rowIdx = 0;  rowIdx < headerMatrix.length;  rowIdx++ ) {
				if(headerMatrix.length > 1 && rowIdx != headerMatrix.length - 1){
					pivotColSpan = true;
				}else{
					pivotColSpan = false;
				}
				html += addRow(headerMatrix[rowIdx], `th${pivotColSpan ? "~pivotcolspan": ""}`);
			}
			html += '</thead>';
		}

		html += '<tbody>';
		for (var i = 0, ien = data.body.length; i < ien; i++) {
			var thisRowData = ivDatas[i];
			var specialRowStyle = "";
			if (typeof thisRowData["axrowtype"] != "undefined") {
				switch (thisRowData["axrowtype"]) {
					case "stot":
						specialRowStyle = 'fontBlue';
						break;
					case "subhead":
						specialRowStyle = 'fontBlack';
						break;
					case "gtot":
						specialRowStyle = 'fontGreen';
						break;
				}
			}
			html += addRow( data.body[i], 'td', '', specialRowStyle );
		}
		html += '</tbody>';

		if ( config.footer && data.footer ) {
			html += '<tfoot>';
			var footerMatrix = _fnGetFooters(dt);
			for ( var rowIdx = 0;  rowIdx < footerMatrix.length;  rowIdx++ ) {
                		html += addRow(footerMatrix[rowIdx], 'th');
            		}
			// html += addRow(data.footer, 'th');
			html += '</tfoot>';
		}

		html += "</table>";

		if (config && config.exportHF) {
			if (config.exportHF.footer && Object.keys(config.exportHF.footer).length > 0) {
				html += '<table class="' + dt.table().node().className + ' exportHF">';
				// html += ''+ addRow( [""], 'td', 'colspan="'+ colSpan +'"', 'dt-center' ) +'';
			}

			if(config.exportHF.footer && Object.keys(config.exportHF.footer).length > 0){
				let alignClass = 'dt-center'; // Default is center alignment
				const footerAlign = config.exportHF.footer["f1"]?.footer_aline;
				if (footerAlign) {
					alignClass = { "left": "dt-left", "center": "dt-center", "right": "dt-right" }[footerAlign.substr(1).toLowerCase()] || "dt-center";
				}
				Object.keys(config.exportHF.footer).forEach(function(val){
					if (config.exportHF.footer[val].text?.trim()) {
						html += ''+ addRow( [config.exportHF.footer[val].text?.trim()], 'td', 'colspan="'+ colSpan +'"', alignClass ) +'';
					}
				});
				html += "</table>";
			}
		}

		// Open a new window for the printable table
		var win = window.open( '', '' );
		win.document.close();

		// Inject the title and also a copy of the style and link tags from this
		// document so the table can retain its base styling. Note that we have
		// to use string manipulation as IE won't allow elements to be created
		// in the host document and then appended to the new window.
		var head = '<title>'+exportInfo.title+'</title>';
		$('style, link').each( function () {
			head += _styleToAbs( this );
		} );

		head += "\
		<style>\
			.dataTable th, .dataTable td{\
				padding: 3px !important;\
				/*border: 1px solid #ccc !important;*/\
				font-size: 14px !important;\
			}\
			.table.dataTable tbody tr.specialRow td.fontGreen {\
				color: #1f911f !important;\
			}\
			.table.dataTable tbody tr.specialRow td.fontBlack {\
				color: #000 !important;\
			}\
			.table.dataTable tbody tr.specialRow td.fontBlue {\
				color: #0000ff !important;\
			}\
			\
			thead { display: table-row-group !important;}\
        	tfoot { display: table-row-group !important; }\
        	tr { page-break-inside: avoid !important; }\
		</style>\
		";

		try {
			win.document.head.innerHTML = head; // Work around for Edge
		}
		catch (e) {
			$(win.document.head).html( head ); // Old IE
		}

		// Inject the table and other surrounding information
		win.document.body.innerHTML =
			'<h1>'+exportInfo.title+'</h1>'+
			'<div>'+(exportInfo.messageTop || '')+'</div>'+
			html+
			'<div>'+(exportInfo.messageBottom || '')+'</div>';

		$(win.document.body).addClass('dt-print-view');

		$('img', win.document.body).each( function ( i, img ) {
			img.setAttribute( 'src', _relToAbs( img.getAttribute('src') ) );
		} );

		if ( config.customize ) {
			config.customize( win, config, dt );
		}

		if(config.htmlExport){
			setTimeout(() => {
				$.fn.dataTable.fileSave(
					new Blob( [ win.document.documentElement.outerHTML ] ),
					`${IVIRCaption}.html`
				);

				win.close(); 
			}, 1000);

			this.processing(false);
			return;
		}

		// Allow stylesheets time to load
		var autoPrint = function () {
			if ( config.autoPrint ) {
				win.print(); // blocking - so close will not
				win.close(); // execute until this is done
			}
		};

		if ( navigator.userAgent.match(/Trident\/\d.\d/) ) { // IE needs to call this without a setTimeout
			autoPrint();
		}
		else {
			win.setTimeout( autoPrint, 1000 );
		}
		this.processing(false);
	},

	title: '*',

	messageTop: '*',

	messageBottom: '*',

	exportHF: {},


	exportOptions: {},

	header: true,

	footer: false,

	autoPrint: true,

	customize: null
};


return DataTable.Buttons;
}));
