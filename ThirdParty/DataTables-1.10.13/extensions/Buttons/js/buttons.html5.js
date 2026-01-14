/*!
 * HTML5 export buttons for Buttons and DataTables.
 * 2016 SpryMedia Ltd - datatables.net/license
 *
 * FileSaver.js (1.3.3) - MIT license
 * Copyright © 2016 Eli Grey - http://eligrey.com
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
		module.exports = function (root, $, jszip, pdfmake) {
			if ( ! root ) {
				root = window;
			}

			if ( ! $ || ! $.fn.dataTable ) {
				$ = require('datatables.net')(root, $).$;
			}

			if ( ! $.fn.dataTable.Buttons ) {
				require('datatables.net-buttons')(root, $);
			}

			return factory( $, root, root.document, jszip, pdfmake );
		};
	}
	else {
		// Browser
		factory( jQuery, window, document );
	}
}(function( $, window, document, jszip, pdfmake, undefined ) {
'use strict';
var DataTable = $.fn.dataTable;

// Allow the constructor to pass in JSZip and PDFMake from external requires.
// Otherwise, use globally defined variables, if they are available.
function _jsZip () {
	return jszip || window.JSZip;
}
function _pdfMake () {
	return pdfmake || window.pdfMake;
}

DataTable.Buttons.pdfMake = function (_) {
	if ( ! _ ) {
		return _pdfMake();
	}
	pdfmake = _;
}

DataTable.Buttons.jszip = function (_) {
	if ( ! _ ) {
		return _jsZip();
	}
	jszip = _;
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * FileSaver.js dependency
 */

/*jslint bitwise: true, indent: 4, laxbreak: true, laxcomma: true, smarttabs: true, plusplus: true */

var _saveAs = (function(view) {
	"use strict";
	// IE <10 is explicitly unsupported
	if (typeof view === "undefined" || typeof navigator !== "undefined" && /MSIE [1-9]\./.test(navigator.userAgent)) {
		return;
	}
	var
		  doc = view.document
		  // only get URL when necessary in case Blob.js hasn't overridden it yet
		, get_URL = function() {
			return view.URL || view.webkitURL || view;
		}
		, save_link = doc.createElementNS("http://www.w3.org/1999/xhtml", "a")
		, can_use_save_link = "download" in save_link
		, click = function(node) {
			var event = new MouseEvent("click");
			node.dispatchEvent(event);
		}
		, is_safari = /constructor/i.test(view.HTMLElement) || view.safari
		, is_chrome_ios =/CriOS\/[\d]+/.test(navigator.userAgent)
		, throw_outside = function(ex) {
			(view.setImmediate || view.setTimeout)(function() {
				throw ex;
			}, 0);
		}
		, force_saveable_type = "application/octet-stream"
		// the Blob API is fundamentally broken as there is no "downloadfinished" event to subscribe to
		, arbitrary_revoke_timeout = 1000 * 40 // in ms
		, revoke = function(file) {
			var revoker = function() {
				if (typeof file === "string") { // file is an object URL
					get_URL().revokeObjectURL(file);
				} else { // file is a File
					file.remove();
				}
			};
			setTimeout(revoker, arbitrary_revoke_timeout);
		}
		, dispatch = function(filesaver, event_types, event) {
			event_types = [].concat(event_types);
			var i = event_types.length;
			while (i--) {
				var listener = filesaver["on" + event_types[i]];
				if (typeof listener === "function") {
					try {
						listener.call(filesaver, event || filesaver);
					} catch (ex) {
						throw_outside(ex);
					}
				}
			}
		}
		, auto_bom = function(blob) {
			// prepend BOM for UTF-8 XML and text/* types (including HTML)
			// note: your browser will automatically convert UTF-16 U+FEFF to EF BB BF
			if (/^\s*(?:text\/\S*|application\/xml|\S*\/\S*\+xml)\s*;.*charset\s*=\s*utf-8/i.test(blob.type)) {
				return new Blob([String.fromCharCode(0xFEFF), blob], {type: blob.type});
			}
			return blob;
		}
		, FileSaver = function(blob, name, no_auto_bom) {
			if (!no_auto_bom) {
				blob = auto_bom(blob);
			}
			// First try a.download, then web filesystem, then object URLs
			var
				  filesaver = this
				, type = blob.type
				, force = type === force_saveable_type
				, object_url
				, dispatch_all = function() {
					dispatch(filesaver, "writestart progress write writeend".split(" "));
				}
				// on any filesys errors revert to saving with object URLs
				, fs_error = function() {
					if ((is_chrome_ios || (force && is_safari)) && view.FileReader) {
						// Safari doesn't allow downloading of blob urls
						var reader = new FileReader();
						reader.onloadend = function() {
							var url = is_chrome_ios ? reader.result : reader.result.replace(/^data:[^;]*;/, 'data:attachment/file;');
							var popup = view.open(url, '_blank');
							if(!popup) view.location.href = url;
							url=undefined; // release reference before dispatching
							filesaver.readyState = filesaver.DONE;
							dispatch_all();
						};
						reader.readAsDataURL(blob);
						filesaver.readyState = filesaver.INIT;
						return;
					}
					// don't create more object URLs than needed
					if (!object_url) {
						object_url = get_URL().createObjectURL(blob);
					}
					if (force) {
						view.location.href = object_url;
					} else {
						var opened = view.open(object_url, "_blank");
						if (!opened) {
							// Apple does not allow window.open, see https://developer.apple.com/library/safari/documentation/Tools/Conceptual/SafariExtensionGuide/WorkingwithWindowsandTabs/WorkingwithWindowsandTabs.html
							view.location.href = object_url;
						}
					}
					filesaver.readyState = filesaver.DONE;
					dispatch_all();
					revoke(object_url);
				}
			;
			filesaver.readyState = filesaver.INIT;

			if (can_use_save_link) {
				object_url = get_URL().createObjectURL(blob);
				setTimeout(function() {
					save_link.href = object_url;
					save_link.download = name;
					click(save_link);
					dispatch_all();
					revoke(object_url);
					filesaver.readyState = filesaver.DONE;
				});
				return;
			}

			fs_error();
		}
		, FS_proto = FileSaver.prototype
		, saveAs = function(blob, name, no_auto_bom) {
			return new FileSaver(blob, name || blob.name || "download", no_auto_bom);
		}
	;
	// IE 10+ (native saveAs)
	if (typeof navigator !== "undefined" && navigator.msSaveOrOpenBlob) {
		return function(blob, name, no_auto_bom) {
			name = name || blob.name || "download";

			if (!no_auto_bom) {
				blob = auto_bom(blob);
			}
			return navigator.msSaveOrOpenBlob(blob, name);
		};
	}

	FS_proto.abort = function(){};
	FS_proto.readyState = FS_proto.INIT = 0;
	FS_proto.WRITING = 1;
	FS_proto.DONE = 2;

	FS_proto.error =
	FS_proto.onwritestart =
	FS_proto.onprogress =
	FS_proto.onwrite =
	FS_proto.onabort =
	FS_proto.onerror =
	FS_proto.onwriteend =
		null;

	return saveAs;
}(
	   typeof self !== "undefined" && self
	|| typeof window !== "undefined" && window
	|| this.content
));


// Expose file saver on the DataTables API. Can't attach to `DataTables.Buttons`
// since this file can be loaded before Button's core!
DataTable.fileSave = _saveAs;


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Local (private) functions
 */

/**
 * Get the sheet name for Excel exports.
 *
 * @param {object}	config Button configuration
 */
var _sheetname = function ( config )
{
	var sheetName = 'Sheet1';

	if ( config.sheetName ) {
		sheetName = config.sheetName.replace(/[\[\]\*\/\\\?\:]/g, '');
	}

	return sheetName;
};

/**
 * Get the newline character(s)
 *
 * @param {object}	config Button configuration
 * @return {string}				Newline character
 */
var _newLine = function ( config )
{
	return config.newline ?
		config.newline :
		navigator.userAgent.match(/Windows/) ?
			'\r\n' :
			'\n';
};

/**
 * Combine the data from the `buttons.exportData` method into a string that
 * will be used in the export file.
 *
 * @param	{DataTable.Api} dt		 DataTables API instance
 * @param	{object}				config Button configuration
 * @return {object}							 The data to export
 */
var _exportData = function ( dt, config )
{
	var newLine = _newLine( config );
	var data = dt.buttons.exportData( config.exportOptions );

	if(data.header){
		data.header = _fnGetHeaders(dt);
	}

	if(data.footer){
		data.footer = [data.footer];
	}
	
	var boundary = config.fieldBoundary;
	var separator = config.fieldSeparator;
	var reBoundary = new RegExp( boundary, 'g' );
	var escapeChar = config.escapeChar !== undefined ?
		config.escapeChar :
		'\\';
	var join = function ( a ) {
		var s = '';

		// If there is a field boundary, then we might need to escape it in
		// the source data
		for ( var i=0, ien=a.length ; i<ien ; i++ ) {
			if ( i > 0 ) {
				s += separator;
			}

			s += boundary ?
				boundary + ('' + a[i]).replace( reBoundary, escapeChar+boundary ) + boundary :
				a[i];
		}

		return s;
	};

	var header = [];//config.header ? join( data.header )+newLine : '';
	for ( var i=0, ien=data.header.length ; i<ien ; i++ ) {
		header.push( join( data.header[i] ) );
	}

	var footer = [];//config.footer && data.footer ? newLine+join( data.footer ) : '';
	for ( var i=0, ien=data.footer.length ; i<ien ; i++ ) {
		footer.push( join( data.footer[i] ) );
	}

	var body = [];
	for ( var i=0, ien=data.body.length ; i<ien ; i++ ) {
		body.push( join( data.body[i] ) );
	}

	return {
		str: (header.length > 0 ? (header.join( newLine ) + newLine) : "") + (body.join( newLine )) + (footer.length > 0 ? (newLine + footer.join( newLine )) : ""),
		rows: body.length
	};
};

/**
 * Older versions of Safari (prior to tech preview 18) don't support the
 * download option required.
 *
 * @return {Boolean} `true` if old Safari
 */
var _isDuffSafari = function ()
{
	var safari = navigator.userAgent.indexOf('Safari') !== -1 &&
		navigator.userAgent.indexOf('Chrome') === -1 &&
		navigator.userAgent.indexOf('Opera') === -1;

	if ( ! safari ) {
		return false;
	}

	var version = navigator.userAgent.match( /AppleWebKit\/(\d+\.\d+)/ );
	if ( version && version.length > 1 && version[1]*1 < 603.1 ) {
		return true;
	}

	return false;
};

/**
 * Convert from numeric position to letter for column names in Excel
 * @param  {int} n Column number
 * @return {string} Column letter(s) name
 */
function createCellPos( n ){
	var ordA = 'A'.charCodeAt(0);
	var ordZ = 'Z'.charCodeAt(0);
	var len = ordZ - ordA + 1;
	var s = "";

	while( n >= 0 ) {
		s = String.fromCharCode(n % len + ordA) + s;
		n = Math.floor(n / len) - 1;
	}

	return s;
}

try {
	var _serialiser = new XMLSerializer();
	var _ieExcel;
}
catch (t) {}

/**
 * Recursively add XML files from an object's structure to a ZIP file. This
 * allows the XSLX file to be easily defined with an object's structure matching
 * the files structure.
 *
 * @param {JSZip} zip ZIP package
 * @param {object} obj Object to add (recursive)
 */
function _addToZip( zip, obj ) {
	if ( _ieExcel === undefined ) {
		// Detect if we are dealing with IE's _awful_ serialiser by seeing if it
		// drop attributes
		_ieExcel = _serialiser
			.serializeToString(
				( new window.DOMParser() ).parseFromString( excelStrings['xl/worksheets/sheet1.xml'], 'text/xml' )
			)
			.indexOf( 'xmlns:r' ) === -1;
	}

	$.each( obj, function ( name, val ) {
		if ( $.isPlainObject( val ) ) {
			var newDir = zip.folder( name );
			_addToZip( newDir, val );
		}
		else {
			if ( _ieExcel ) {
				// IE's XML serialiser will drop some name space attributes from
				// from the root node, so we need to save them. Do this by
				// replacing the namespace nodes with a regular attribute that
				// we convert back when serialised. Edge does not have this
				// issue
				var worksheet = val.childNodes[0];
				var i, ien;
				var attrs = [];

				for ( i=worksheet.attributes.length-1 ; i>=0 ; i-- ) {
					var attrName = worksheet.attributes[i].nodeName;
					var attrValue = worksheet.attributes[i].nodeValue;

					if ( attrName.indexOf( ':' ) !== -1 ) {
						attrs.push( { name: attrName, value: attrValue } );

						worksheet.removeAttribute( attrName );
					}
				}

				for ( i=0, ien=attrs.length ; i<ien ; i++ ) {
					var attr = val.createAttribute( attrs[i].name.replace( ':', '_dt_b_namespace_token_' ) );
					attr.value = attrs[i].value;
					worksheet.setAttributeNode( attr );
				}
			}

			var str = _serialiser.serializeToString(val);

			// Fix IE's XML
			if ( _ieExcel ) {
				// IE doesn't include the XML declaration
				if ( str.indexOf( '<?xml' ) === -1 ) {
					str = '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'+str;
				}

				// Return namespace attributes to being as such
				str = str.replace( /_dt_b_namespace_token_/g, ':' );

				// Remove testing name space that IE puts into the space preserve attr
				str = str.replace( /xmlns:NS[\d]+="" NS[\d]+:/g, '' );
			}

			// Safari, IE and Edge will put empty name space attributes onto
			// various elements making them useless. This strips them out
			str = str.replace( /<([^<>]*?) xmlns=""([^<>]*?)>/g, '<$1 $2>' );

			zip.file( name, str );
		}
	} );
}

/**
 * Create an XML node and add any children, attributes, etc without needing to
 * be verbose in the DOM.
 *
 * @param  {object} doc      XML document
 * @param  {string} nodeName Node name
 * @param  {object} opts     Options - can be `attr` (attributes), `children`
 *   (child nodes) and `text` (text content)
 * @return {node}            Created node
 */
function _createNode( doc, nodeName, opts ) {
	var tempNode = doc.createElement( nodeName );

	if ( opts ) {
		if ( opts.attr ) {
			$(tempNode).attr( opts.attr );
		}

		if ( opts.children ) {
			$.each( opts.children, function ( key, value ) {
				tempNode.appendChild( value );
			} );
		}

		if ( opts.text !== null && opts.text !== undefined ) {
			tempNode.appendChild( doc.createTextNode( opts.text ) );
		}
	}

	return tempNode;
}

/**
 * Get the width for an Excel column based on the contents of that column
 * @param  {object} data Data for export
 * @param  {int}    col  Column index
 * @return {int}         Column width
 */
function _excelColWidth( data, col ) {
	var max = data.header[col].length;
	var len, lineSplit, str;

	if ( data.footer && data.footer[col].length > max ) {
		max = data.footer[col].length;
	}

	for ( var i=0, ien=data.body.length ; i<ien ; i++ ) {
		var point = data.body[i][col];
		str = point !== null && point !== undefined ?
			point.toString() :
			'';

		// If there is a newline character, workout the width of the column
		// based on the longest line in the string
		if ( str.indexOf('\n') !== -1 ) {
			lineSplit = str.split('\n');
			lineSplit.sort( function (a, b) {
				return b.length - a.length;
			} );

			len = lineSplit[0].length;
		}
		else {
			len = str.length;
		}

		if ( len > max ) {
			max = len;
		}

		// Max width rather than having potentially massive column widths
		if ( max > 40 ) {
			return 54; // 40 * 1.35
		}
	}

	max *= 1.35;

	// And a min width
	return max > 6 ? max : 6;
}

var addedDecimalFormats = [];
var addedWordWraps = [];
var addedNumericComma = [];
var addedFormatStyle = [];
// Excel - Pre-defined strings to build a basic XLSX file
var excelStrings = {
	"_rels/.rels":
		'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'+
		'<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">'+
			'<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>'+
		'</Relationships>',

	"xl/_rels/workbook.xml.rels":
		'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'+
		'<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">'+
			'<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>'+
			'<Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>'+
		'</Relationships>',

	"[Content_Types].xml":
		'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'+
		'<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">'+
			'<Default Extension="xml" ContentType="application/xml" />'+
			'<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />'+
			'<Default Extension="jpeg" ContentType="image/jpeg" />'+
			'<Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml" />'+
			'<Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml" />'+
			'<Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml" />'+
		'</Types>',

	"xl/workbook.xml":
		'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'+
		'<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">'+
			'<fileVersion appName="xl" lastEdited="5" lowestEdited="5" rupBuild="24816"/>'+
			'<workbookPr showInkAnnotation="0" autoCompressPictures="0"/>'+
			'<bookViews>'+
				'<workbookView xWindow="0" yWindow="0" windowWidth="25600" windowHeight="19020" tabRatio="500"/>'+
			'</bookViews>'+
			'<sheets>'+
				'<sheet name="Sheet1" sheetId="1" r:id="rId1"/>'+
			'</sheets>'+
			'<definedNames/>'+
		'</workbook>',

	"xl/worksheets/sheet1.xml":
		'<?xml version="1.0" encoding="UTF-8" standalone="yes"?>'+
		'<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" mc:Ignorable="x14ac" xmlns:x14ac="http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac">'+
			'<sheetData/>'+
			'<mergeCells count="0"/>'+
		'</worksheet>',

	"xl/styles.xml":
		'<?xml version="1.0" encoding="UTF-8"?>'+
		'<styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" mc:Ignorable="x14ac" xmlns:x14ac="http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac">'+
			'<numFmts count="9">'+
				'<numFmt numFmtId="164" formatCode="#,##0.00_-\ [$$-45C]"/>'+
				'<numFmt numFmtId="165" formatCode="&quot;£&quot;#,##0.00"/>'+
				'<numFmt numFmtId="166" formatCode="[$€-2]\ #,##0.00"/>'+
				'<numFmt numFmtId="167" formatCode="0.0%"/>'+
				'<numFmt numFmtId="168" formatCode="#,##0;(#,##0)"/>'+
				'<numFmt numFmtId="169" formatCode="#,##0.00;(#,##0.00)"/>'+

				'<numFmt numFmtId="170" formatCode="@"/>'+//170
				'<numFmt numFmtId="171" formatCode="#.########"/>'+//171
				'<numFmt numFmtId="172" formatCode="#,###.000"/>'+//172
			'</numFmts>'+
			'<fonts count="8" x14ac:knownFonts="1">'+
				'<font>'+
					'<sz val="11" />'+
					'<name val="Calibri" />'+
				'</font>'+
				'<font>'+
					'<sz val="11" />'+
					'<name val="Calibri" />'+
					'<color rgb="FFFFFFFF" />'+
				'</font>'+
				'<font>'+
					'<sz val="11" />'+
					'<name val="Calibri" />'+
					'<b />'+
				'</font>'+
				'<font>'+
					'<sz val="11" />'+
					'<name val="Calibri" />'+
					'<i />'+
				'</font>'+
				'<font>'+
					'<sz val="11" />'+
					'<name val="Calibri" />'+
					'<u />'+
				'</font>'+
				
				'<font>'+//5
					'<sz val="11" />'+
					'<name val="Calibri" />'+
					'<color rgb="FF0000ff" />'+
					'<b />'+
				'</font>'+
				'<font>'+//6
					'<sz val="11" />'+
					'<name val="Calibri" />'+
					'<color rgb="FF000000" />'+
					'<b />'+
				'</font>'+
				'<font>'+//7
					'<sz val="11" />'+
					'<name val="Calibri" />'+
					'<color rgb="FF1f911f" />'+
					'<b />'+
				'</font>'+
				
			'</fonts>'+
			'<fills count="6">'+
				'<fill>'+
					'<patternFill patternType="none" />'+
				'</fill>'+
				'<fill>'+ // Excel appears to use this as a dotted background regardless of values but
					'<patternFill patternType="none" />'+ // to be valid to the schema, use a patternFill
				'</fill>'+
				'<fill>'+
					'<patternFill patternType="solid">'+
						'<fgColor rgb="FFD9D9D9" />'+
						'<bgColor indexed="64" />'+
					'</patternFill>'+
				'</fill>'+
				'<fill>'+
					'<patternFill patternType="solid">'+
						'<fgColor rgb="FFD99795" />'+
						'<bgColor indexed="64" />'+
					'</patternFill>'+
				'</fill>'+
				'<fill>'+
					'<patternFill patternType="solid">'+
						'<fgColor rgb="ffc6efce" />'+
						'<bgColor indexed="64" />'+
					'</patternFill>'+
				'</fill>'+
				'<fill>'+
					'<patternFill patternType="solid">'+
						'<fgColor rgb="ffc6cfef" />'+
						'<bgColor indexed="64" />'+
					'</patternFill>'+
				'</fill>'+
			'</fills>'+
			'<borders count="2">'+
				'<border>'+
					'<left />'+
					'<right />'+
					'<top />'+
					'<bottom />'+
					'<diagonal />'+
				'</border>'+
				'<border diagonalUp="false" diagonalDown="false">'+
					'<left style="thin">'+
						'<color auto="1" />'+
					'</left>'+
					'<right style="thin">'+
						'<color auto="1" />'+
					'</right>'+
					'<top style="thin">'+
						'<color auto="1" />'+
					'</top>'+
					'<bottom style="thin">'+
						'<color auto="1" />'+
					'</bottom>'+
					'<diagonal />'+
				'</border>'+
			'</borders>'+
			'<cellStyleXfs count="1">'+
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" />'+
			'</cellStyleXfs>'+
			'<cellXfs count="400">'+
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//0
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//1
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//2
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//3
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//4
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//5
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//6
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//7
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//8
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//9
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//10
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//11
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//12
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//13
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//14
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//15
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//16
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//17
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//18
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//19
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//20
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//21
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//22
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//23
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//24
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//25
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//26
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//27
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//28
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//29
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//30
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//31
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//32
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//33
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//34
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//35
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//36
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//37
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//38
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//39
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//40
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//41
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//42
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//43
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//44
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//45
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//46
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//47
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//48
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//49
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="left"/>'+
				'</xf>'+//50
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="center"/>'+
				'</xf>'+//51
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="right"/>'+
				'</xf>'+//52
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="fill"/>'+
				'</xf>'+//53
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment textRotation="90"/>'+
				'</xf>'+//54
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment wrapText="1"/>'+
				'</xf>'+//55
				'<xf numFmtId="9" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//56
				'<xf numFmtId="164" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//57
				'<xf numFmtId="165" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//58
				'<xf numFmtId="166" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//59
				'<xf numFmtId="167" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//60
				'<xf numFmtId="168" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//61
				'<xf numFmtId="169" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//62
				'<xf numFmtId="3" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//63
				'<xf numFmtId="4" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//64
				'<xf numFmtId="1" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//65
				'<xf numFmtId="2" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//66
				'<xf numFmtId="14" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//67
				
				'<xf numFmtId="0" fontId="5" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//68
				'<xf numFmtId="0" fontId="6" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//69
				'<xf numFmtId="0" fontId="7" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1"/>'+//70

				'<xf numFmtId="170" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//71
				'<xf numFmtId="171" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>'+//72

				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//73
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//74
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//75
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//76
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//77
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//78
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//79
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//80
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//81
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//82
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//83
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//84
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//85
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//86
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//87
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//88
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//89
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//90
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//91
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//92
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//93
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//94
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//95
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//96
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//97
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//98
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1"/>'+//99


				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//100
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//101
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//102
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//103
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//104
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//105
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//106
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//107
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//108
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//109
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//110
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//111
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//112
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//113
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//114
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//115
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//116
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//117
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//118
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//119
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//120
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//121
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//122
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//123
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//124
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//125
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//126
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//127
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//128
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//129
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//130
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//131
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//132
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//133
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//134
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//135
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//136
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//137
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//138
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//139
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//140
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//141
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//142
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//143
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//144
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//145
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//146
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//147
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//148
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//149
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="left"/>'+
				'</xf>'+//150
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="center"/>'+
				'</xf>'+//151
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="right"/>'+
				'</xf>'+//152
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="fill"/>'+
				'</xf>'+//153
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment textRotation="90"/>'+
				'</xf>'+//154
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment wrapText="1"/>'+
				'</xf>'+//155
				'<xf numFmtId="9" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//156
				'<xf numFmtId="164" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//157
				'<xf numFmtId="165" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//158
				'<xf numFmtId="166" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//159
				'<xf numFmtId="167" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//160
				'<xf numFmtId="168" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//161
				'<xf numFmtId="169" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//162
				'<xf numFmtId="3" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//163
				'<xf numFmtId="4" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//164
				'<xf numFmtId="1" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//165
				'<xf numFmtId="2" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//166
				'<xf numFmtId="14" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//167
				
				'<xf numFmtId="0" fontId="5" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//168
				'<xf numFmtId="0" fontId="6" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//169
				'<xf numFmtId="0" fontId="7" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//170

				'<xf numFmtId="170" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//171
				'<xf numFmtId="171" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//172

				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//173
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//174
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//175
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//176
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//177
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//178
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//179
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//180
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//181
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//182
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//183
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//184
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//185
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//186
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//187
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//188
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//189
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//190
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//191
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//192
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//193
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//194
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//195
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//196
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//197
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//198
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="left" vertical="top" />'+
				'</xf>'+//199


				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//200
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//201
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//202
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//203
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//204
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//205
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//206
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//207
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//208
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//209
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//210
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//211
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//212
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//213
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//214
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//215
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//216
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//217
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//218
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//219
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//220
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//221
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//222
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//223
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//224
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//225
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//226
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//227
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//228
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//229
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//230
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//231
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//232
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//233
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//234
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//235
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//236
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//237
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//238
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//239
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//240
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//241
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//242
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//243
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//244
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//245
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//246
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//247
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//248
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//249
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="left"/>'+
				'</xf>'+//250
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="center"/>'+
				'</xf>'+//251
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="right"/>'+
				'</xf>'+//252
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="fill"/>'+
				'</xf>'+//253
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment textRotation="90"/>'+
				'</xf>'+//254
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment wrapText="1"/>'+
				'</xf>'+//255
				'<xf numFmtId="9" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//256
				'<xf numFmtId="164" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//257
				'<xf numFmtId="165" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//58
				'<xf numFmtId="166" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//259
				'<xf numFmtId="167" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//260
				'<xf numFmtId="168" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//261
				'<xf numFmtId="169" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//262
				'<xf numFmtId="3" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//263
				'<xf numFmtId="4" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//264
				'<xf numFmtId="1" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//265
				'<xf numFmtId="2" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//266
				'<xf numFmtId="14" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//267
				
				'<xf numFmtId="0" fontId="5" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//268
				'<xf numFmtId="0" fontId="6" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//269
				'<xf numFmtId="0" fontId="7" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//270

				'<xf numFmtId="170" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//271
				'<xf numFmtId="171" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//272

				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//273
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//274
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//275
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//276
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//277
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//278
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//279
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//280
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//281
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//282
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//283
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//284
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//285
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//286
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//287
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//288
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//289
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//290
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//291
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//292
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//293
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//294
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//295
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//296
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//297
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//298
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="center" vertical="top" />'+
				'</xf>'+//299


				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//300
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//301
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//302
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//303
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//304
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//305
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//306
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//307
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//308
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//309
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//310
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//311
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//312
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//313
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//314
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//315
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//316
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//317
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//318
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//319
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//320
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//321
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//322
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//323
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//324
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//325
				'<xf numFmtId="0" fontId="1" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//326
				'<xf numFmtId="0" fontId="2" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//327
				'<xf numFmtId="0" fontId="3" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//328
				'<xf numFmtId="0" fontId="4" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//329
				'<xf numFmtId="0" fontId="0" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//330
				'<xf numFmtId="0" fontId="1" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//331
				'<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//332
				'<xf numFmtId="0" fontId="3" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//333
				'<xf numFmtId="0" fontId="4" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//334
				'<xf numFmtId="0" fontId="0" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//335
				'<xf numFmtId="0" fontId="1" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//336
				'<xf numFmtId="0" fontId="2" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//337
				'<xf numFmtId="0" fontId="3" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//338
				'<xf numFmtId="0" fontId="4" fillId="3" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//339
				'<xf numFmtId="0" fontId="0" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//340
				'<xf numFmtId="0" fontId="1" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//341
				'<xf numFmtId="0" fontId="2" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//342
				'<xf numFmtId="0" fontId="3" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//343
				'<xf numFmtId="0" fontId="4" fillId="4" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//344
				'<xf numFmtId="0" fontId="0" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//345
				'<xf numFmtId="0" fontId="1" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//346
				'<xf numFmtId="0" fontId="2" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//347
				'<xf numFmtId="0" fontId="3" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//348
				'<xf numFmtId="0" fontId="4" fillId="5" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//349
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="left"/>'+
				'</xf>'+//350
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="center"/>'+
				'</xf>'+//351
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="right"/>'+
				'</xf>'+//352
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment horizontal="fill"/>'+
				'</xf>'+//353
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment textRotation="90"/>'+
				'</xf>'+//354
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">'+
					'<alignment wrapText="1"/>'+
				'</xf>'+//355
				'<xf numFmtId="9" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//356
				'<xf numFmtId="164" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//357
				'<xf numFmtId="165" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//358
				'<xf numFmtId="166" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//359
				'<xf numFmtId="167" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//360
				'<xf numFmtId="168" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//361
				'<xf numFmtId="169" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//362
				'<xf numFmtId="3" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//363
				'<xf numFmtId="4" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//364
				'<xf numFmtId="1" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//365
				'<xf numFmtId="2" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//366
				'<xf numFmtId="14" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//367
				
				'<xf numFmtId="0" fontId="5" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//368
				'<xf numFmtId="0" fontId="6" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//369
				'<xf numFmtId="0" fontId="7" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//370

				'<xf numFmtId="170" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//371
				'<xf numFmtId="171" fontId="0" fillId="0" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//372

				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//373
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//374
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//375
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//376
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//377
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//378
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//379
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//380
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//381
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//382
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//383
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//384
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//385
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//386
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//387
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//388
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//389
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//390
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//391
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//392
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//393
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//394
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//395
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//396
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//397
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//398
				'<xf numFmtId="0" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" applyAlignment="1">'+
					'<alignment horizontal="right" vertical="top" />'+
				'</xf>'+//399


				
			'</cellXfs>'+
			'<cellStyles count="1">'+
				'<cellStyle name="Normal" xfId="0" builtinId="0" />'+
			'</cellStyles>'+
			'<dxfs count="0" />'+
			'<tableStyles count="0" defaultTableStyle="TableStyleMedium9" defaultPivotStyle="PivotStyleMedium4" />'+
		'</styleSheet>'
};
// Note we could use 3 `for` loops for the styles, but when gzipped there is
// virtually no difference in size, since the above can be easily compressed

// Pattern matching for special number formats. Perhaps this should be exposed
// via an API in future?
// Ref: section 3.8.30 - built in formatters in open spreadsheet
//   https://www.ecma-international.org/news/TC45_current_work/Office%20Open%20XML%20Part%204%20-%20Markup%20Language%20Reference.pdf
var _excelSpecials = [
	{ match: /^\-?\d+\.\d%$/,               style: 60, fmt: function (d) { return d/100; } }, // Precent with d.p.
	{ match: /^\-?\d+\.?\d*%$/,             style: 56, fmt: function (d) { return d/100; } }, // Percent
	{ match: /^\-?\$[\d,]+.?\d*$/,          style: 57 }, // Dollars
	{ match: /^\-?£[\d,]+.?\d*$/,           style: 58 }, // Pounds
	{ match: /^\-?€[\d,]+.?\d*$/,           style: 59 }, // Euros
	{ match: /^\-?\d+$/,                    style: 65 }, // Numbers without thousand separators
	{ match: /^\-?\d+\.\d{2}$/,             style: 66 }, // Numbers 2 d.p. without thousands separators
	{ match: /^\([\d,]+\)$/,                style: 61, fmt: function (d) { return -1 * d.replace(/[\(\)]/g, ''); } },  // Negative numbers indicated by brackets
	{ match: /^\([\d,]+\.\d{2}\)$/,         style: 62, fmt: function (d) { return -1 * d.replace(/[\(\)]/g, ''); } },  // Negative numbers indicated by brackets - 2d.p.
	{ match: /^\-?[\d,]+$/,                 style: 63 }, // Numbers with thousand separators
	{ match: /^\-?[\d,]+\.\d{2}$/,          style: 64 },
	{ match: /^[\d]{4}\-[\d]{2}\-[\d]{2}$/, style: 67, fmt: function (d) {return Math.round(25569 + (Date.parse(d) / (86400 * 1000)));}} //Date yyyy-mm-dd
];



/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Buttons
 */

//
// Copy to clipboard
//
DataTable.ext.buttons.copyHtml5 = {
	className: 'buttons-copy buttons-html5',

	text: function ( dt ) {
		return dt.i18n( 'buttons.copy', 'Copy' );
	},

	action: function ( e, dt, button, config ) {
		this.processing( true );

		var that = this;
		var exportData = _exportData( dt, config );
		var info = dt.buttons.exportInfo( config );
		var newline = _newLine(config);
		var output = exportData.str;
		var hiddenDiv = $('<div/>')
			.css( {
				height: 1,
				width: 1,
				overflow: 'hidden',
				position: 'fixed',
				top: 0,
				left: 0
			} );

		if ( info.title ) {
			output = info.title + newline + newline + output;
		}

		if ( info.messageTop ) {
			output = info.messageTop + newline + newline + output;
		}

		if ( info.messageBottom ) {
			output = output + newline + newline + info.messageBottom;
		}

		if ( config.customize ) {
			output = config.customize( output, config, dt );
		}

		var textarea = $('<textarea readonly/>')
			.val( output )
			.appendTo( hiddenDiv );

		// For browsers that support the copy execCommand, try to use it
		if ( document.queryCommandSupported('copy') ) {
			hiddenDiv.appendTo( dt.table().container() );
			textarea[0].focus();
			textarea[0].select();

			try {
				var successful = document.execCommand( 'copy' );
				hiddenDiv.remove();

				if (successful) {
					dt.buttons.info(
						dt.i18n( 'buttons.copyTitle', 'Copy to clipboard' ),
						dt.i18n( 'buttons.copySuccess', {
							1: 'Copied one row to clipboard',
							_: 'Copied %d rows to clipboard'
						}, exportData.rows ),
						2000
					);

					this.processing( false );
					return;
				}
			}
			catch (t) {}
		}

		// Otherwise we show the text box and instruct the user to use it
		var message = $('<span>'+dt.i18n( 'buttons.copyKeys',
				'Press <i>ctrl</i> or <i>\u2318</i> + <i>C</i> to copy the table data<br>to your system clipboard.<br><br>'+
				'To cancel, click this message or press escape.' )+'</span>'
			)
			.append( hiddenDiv );

		dt.buttons.info( dt.i18n( 'buttons.copyTitle', 'Copy to clipboard' ), message, 0 );

		// Select the text so when the user activates their system clipboard
		// it will copy that text
		textarea[0].focus();
		textarea[0].select();

		// Event to hide the message when the user is done
		var container = $(message).closest('.dt-button-info');
		var close = function () {
			container.off( 'click.buttons-copy' );
			$(document).off( '.buttons-copy' );
			dt.buttons.info( false );
		};

		container.on( 'click.buttons-copy', close );
		$(document)
			.on( 'keydown.buttons-copy', function (e) {
				if ( e.keyCode === 27 ) { // esc
					close();
					that.processing( false );
				}
			} )
			.on( 'copy.buttons-copy cut.buttons-copy', function () {
				close();
				that.processing( false );
			} );
	},

	exportHF: {},

	exportOptions: {},

	fieldSeparator: '\t',

	fieldBoundary: '',

	header: true,

	footer: false,

	title: '*',

	messageTop: '*',

	messageBottom: '*'
};

//
// CSV export
//
DataTable.ext.buttons.csvHtml5 = {
	bom: false,

	className: 'buttons-csv buttons-html5',

	available: function () {
		return window.FileReader !== undefined && window.Blob;
	},

	text: function ( dt ) {
		return dt.i18n( 'buttons.csv', 'CSV' );
	},

	action: function ( e, dt, button, config ) {
		this.processing( true );

		// Set the text
		var output = _exportData( dt, config ).str;
		var info = dt.buttons.exportInfo(config);
		var charset = config.charset;

		if ( config.customize ) {
			output = config.customize( output, config, dt );
		}

		if ( charset !== false ) {
			if ( ! charset ) {
				charset = document.characterSet || document.charset;
			}

			if ( charset ) {
				charset = ';charset='+charset;
			}
		}
		else {
			charset = '';
		}

		if ( config.bom ) {
			output = '\ufeff' + output;
		}

		_saveAs(
			new Blob( [output], {type: 'text/csv'+charset} ),
			info.filename,
			true
		);

		this.processing( false );
	},

	filename: '*',

	extension: '.csv',

	exportHF: {},

	exportOptions: {},

	fieldSeparator: ',',

	fieldBoundary: '"',

	escapeChar: '"',

	charset: null,

	header: true,

	footer: false
};

//
// Excel (xlsx) export
//
DataTable.ext.buttons.excelHtml5 = {
	className: 'buttons-excel buttons-html5',

	available: function () {
		return window.FileReader !== undefined && _jsZip() !== undefined && ! _isDuffSafari() && _serialiser;
	},

	text: function ( dt ) {
		return dt.i18n( 'buttons.excel', 'Excel' );
	},

	action: function ( e, dt, button, config ) {
		this.processing( true );

		var that = this;
		var rowPos = 0;
		var dataStartRow, dataEndRow;
		var getXml = function ( type ) {
			var str = excelStrings[ type ];

			//str = str.replace( /xmlns:/g, 'xmlns_' ).replace( /mc:/g, 'mc_' );

			return $.parseXML( str );
		};
		var rels = getXml('xl/worksheets/sheet1.xml');
		var relsGet = rels.getElementsByTagName( "sheetData" )[0];

		var xlsx = {
			_rels: {
				".rels": getXml('_rels/.rels')
			},
			xl: {
				_rels: {
					"workbook.xml.rels": getXml('xl/_rels/workbook.xml.rels')
				},
				"workbook.xml": getXml('xl/workbook.xml'),
				"styles.xml": getXml('xl/styles.xml'),
				"worksheets": {
					"sheet1.xml": rels
				}

			},
			"[Content_Types].xml": getXml('[Content_Types].xml')
		};

		var data = dt.buttons.exportData( config.exportOptions );
		var currentRow, rowNode;
		addedDecimalFormats = [];
		addedWordWraps = [];
		addedNumericComma = [];
		addedFormatStyle = [];
		var bodyDir = $("body").attr("dir");
		var styles = xlsx.xl['styles.xml'];
		var numFmts = $('numFmts', styles);
		var cellXfs = $('cellXfs', styles);
		var addRow = function (row, calculatePivotColspan, specialRowStyle, bodyMeta, colProp, thisRowData) {
			specialRowStyle = typeof specialRowStyle != "undefined" ? specialRowStyle : false;
			bodyMeta = typeof bodyMeta != "undefined" ? bodyMeta : [];
			colProp = typeof colProp != "undefined" ? colProp : [];
			thisRowData = typeof thisRowData != "undefined" ? thisRowData : [];
			calculatePivotColspan = typeof calculatePivotColspan != "undefined" ? calculatePivotColspan : false;
			
			currentRow = rowPos+1;
			rowNode = _createNode( rels, "row", { attr: {r:currentRow} } );

			let pivotMergeValues = [];
			let popIndexArray = [];
			let lastData = undefined;
			let thisColSpan = 1;

			for ( var i=0, ien=row.length ; i<ien ; i++ ) {
				// Concat both the Cell Columns as a letter and the Row of the cell.
				var cellId = createCellPos(i) + '' + currentRow;
				var cell = null;

				var dataOut = row[i] === null || row[i] === undefined ? '' : row[i];
				if(calculatePivotColspan){
					if(calculatePivotColspan && lastData !== undefined && lastData !== "" && dataOut === lastData){
						popIndexArray.push(i - 1);
						
						thisColSpan++;
						
						// rowCellsArray.push('<'+tag+' '+ `class="dt-center ${addClass}"` +' '+ attr + ` colspan=${thisColSpan} ` +'>'+dataOut+'</'+tag+'>');
	
					} else {
						thisColSpan = 1;
						
						// rowCellsArray.push('<'+tag+' '+classAttr+' '+ attr +'>'+dataOut+'</'+tag+'>');
					}

					pivotMergeValues.push(thisColSpan);
				}

				// For null, undefined of blank cell, continue so it doesn't create the _createNode
				if ( row[i] === null || row[i] === undefined || row[i] === '' ) {
					if ( config.createEmptyCells === true ) {
						row[i] = '';
					}
					else {
						continue;
					}
				}

				var isSpecialNumber = false;
				if(row[i].indexOf("\u200C") == 0 && !specialRowStyle){
					isSpecialNumber = true;
					// row[i] = "'" + row[i].substr(1);
					// row[i] = "=TEXT(\"" + row[i].substr(1) + "\",\"0\")";
				}

				var originalContent = row[i];
				row[i] = row[i]?.trim();

				// Special number formatting options
				for ( var j=0, jen=_excelSpecials.length ; j<jen ; j++ ) {
					var special = _excelSpecials[j];

					// TODO Need to provide the ability for the specials to say
					// if they are returning a string, since at the moment it is
					// assumed to be a number
					if ( row[i].match && ! row[i].match(/^0\d+/) && row[i].match( special.match ) ) {
						var val = row[i].replace(/[^\d\.\-]/g, '');

						if ( special.fmt ) {
							val = special.fmt( val );
						}

						if(isNaN(row[i].replace(/,/g, ""))){
							cell = _createNode( rels, 'c', {
								attr: {
									r: cellId,
									s: special.style
								},
								children: [
									_createNode( rels, 'v', { text: val } )
								]
							});
						}

						break;
					}
				}

				let isNumericComma = false;

				let noCommaVal = "";

				if(colProp && typeof colProp == "object" && colProp.length > 0 && colProp[i].type == "n" && row[i].indexOf(",") > -1){
					noCommaVal = row[i].replace(/,/g, '')
					if(typeof noCommaVal === 'number' || (noCommaVal.match && noCommaVal.match(/^-?\d+(\.\d+)?$/) && !noCommaVal.match(/^0\d+/))){
						isNumericComma = true;
					}
				}

				if ( ! cell ) {
					if (isNumericComma || typeof row[i] === 'number' || (colProp && colProp?.[i]?.type == "n") || (
						row[i].match &&
						row[i].match(/^-?\d+(\.\d+)?$/) &&
						! row[i].match(/^0\d+/) )
					) {
						// Detect numbers - don't match numbers with leading zeros
						// or a negative anywhere but the start
						cell = _createNode( rels, 'c', {
							attr: {
								t: 'n',
								r: cellId
							},
							children: [
								_createNode( rels, 'v', { text: (isNumericComma ? noCommaVal : row[i]) } )
							]
						} );
					}
					else {
						// String output - replace non standard characters for text output
						var text = ! originalContent.replace ?
							originalContent :
							originalContent.replace(/[\x00-\x09\x0B\x0C\x0E-\x1F\x7F-\x9F]/g, '');

						cell = _createNode( rels, 'c', {
							attr: {
								t: 'inlineStr',
								r: cellId
							},
							children:{
								row: _createNode( rels, 'is', {
									children: {
										row: _createNode( rels, 't', {
											text: text,
											attr: {
												'xml:space': 'preserve'
											}
										} )
									}
								} )
							}
						} );
					}
				}

				$(cell).attr("s", "25");

				if (cell && isSpecialNumber) {
					$(cell).attr("s", "71");
				}

				if (cell && specialRowStyle && typeof specialRowStyle == "string") {
					$(cell).attr("s", specialRowStyle);
				}

				try {
					var currentStyle = parseInt($(cell).attr("s") || "0");
					const arabic = /[\u0600-\u06FF]/;
					
					if(bodyMeta.length > 0){
						switch(ivHeadRows[bodyMeta[i]]["@align"] || (bodyMeta[i] == "rowno" && !enableCardsUi ? "Center" : "Left")){
							case "Left":
								currentStyle += 100;
								if (bodyDir && bodyDir == "rtl" && arabic.test(row[i])) {
									currentStyle += 200;
								}
								break;
							case "Center":
								currentStyle += 200;
								break;
							case "Right":
								currentStyle += 300;
								if (bodyDir && bodyDir == "rtl" && arabic.test(row[i])) {
									currentStyle -= 200;
								}
								break;
							default:
								break;
						}
					}
					$(cell).attr("s", currentStyle);
				} catch (ex) {
					
				}

				//adding format style
				if(cell && colProp && typeof colProp == "object" && colProp.length > 0){
					var formatCode = undefined;

					if(colProp[i].type == "n" && colProp[i].dec != "0"){
						try {
							if(addedDecimalFormats[currentStyle + "~" + colProp[i].dec]){
								currentStyle = addedDecimalFormats[currentStyle + "~" + colProp[i].dec];

								formatCode = '0.'+"0".repeat(colProp[i].dec);
							}else{
								var el = numFmts;

								//Create a custom number format
								//Get the available id for the custom number format
								var numFmtId = parseInt(el.children().last().attr("numFmtId")) + 1;
								
								formatCode = '0.'+"0".repeat(colProp[i].dec);
								
								//XML Node: Custom number format
								var nFmt = '<numFmt numFmtId="' + numFmtId + '" formatCode="'+ formatCode +'" />';
								
								el.append(nFmt).attr('count', parseInt(el.attr('count'))+1);

								//adding format styles to xml format node
								el = cellXfs;
								var newXF = $('xf:eq('+currentStyle+')', el).clone().attr("numFmtId", numFmtId).attr("applyNumberFormat", "1")[0].outerHTML.replace(/xmlns=\"(.*?)\"/g, '');
								el.append(newXF).attr('count', parseInt(el.attr('count'))+1);

								// Index of our new style
								var styleIdx = el?.[0]?.querySelectorAll('xf').length - 1;

								addedDecimalFormats[currentStyle + "~" + colProp[i].dec] = styleIdx;

								currentStyle = styleIdx;

								// decimalFormatCreated = true;
							}
						} catch (ex) {}
						$(cell).attr("s", currentStyle);
					}
					
					if(isNumericComma){
						try {
							var el = cellXfs;
							var existingNumFmtId = parseInt(el?.[0]?.querySelectorAll('xf')?.[currentStyle]?.getAttribute("numFmtId") || 0);

							if(existingNumFmtId == 0 || typeof formatCode != "undefined"){
								if(addedNumericComma[currentStyle]){
									currentStyle = addedNumericComma[currentStyle];
								}else{
									var el = numFmts;

									//Create a custom number format
									//Get the available id for the custom number format
									var numFmtId = parseInt(numFmts.children().last().attr("numFmtId")) + 1;
	
									if(colProp[i].type == "n" && colProp[i].dec != "0"){
										formatCode = '#,###.'+"0".repeat(colProp[i].dec);
									}else{
										formatCode = '#,###';
									}
				
									//XML Node: Custom number format
									var nFmt = '<numFmt numFmtId="' + numFmtId + '" formatCode="'+ formatCode +'" />';
									
									el.append(nFmt).attr('count', parseInt(el.attr('count'))+1);
	
									//adding format styles to xml format node
									el = cellXfs;
									var newXF = $('xf:eq('+currentStyle+')', el).clone().attr("numFmtId", numFmtId).attr("applyNumberFormat", "1")[0].outerHTML.replace(/xmlns=\"(.*?)\"/g, '');
									el.append(newXF).attr('count', parseInt(el.attr('count'))+1);
	
									// Index of our new style
									var styleIdx = $('xf', el).length - 1;
	
									addedNumericComma[currentStyle] = styleIdx;
	
									currentStyle = styleIdx;
								}
							}
						} catch (ex) {}
						$(cell).attr("s", currentStyle);
					}

					if(!exportPerf){
						var fontString = applyConditionalFormatting({ rowData: thisRowData, colID: colProp[i].name }, false);

						if (fontString) {
							if(addedFormatStyle[colProp[i].name + "=>" + fontString]){
								currentStyle = addedFormatStyle[colProp[i].name + "=>" + fontString];
							}
							else{
								currentStyle = SetExcelFontCondition(currentStyle, fontString, styles);

								addedFormatStyle[colProp[i].name + "=>" + fontString] = currentStyle;
							}

							$(cell).attr("s", currentStyle);
						}
					}
				}

				try {
					if(addedWordWraps.indexOf(currentStyle) == -1){
						//update format styles in xml for wrapText
						el = cellXfs;

						$('xf:eq('+currentStyle+')', el).attr("applyAlignment", "1");
						
						if($('xf:eq('+currentStyle+')', el).find('alignment').length > 0){
							$('xf:eq('+currentStyle+')', el).find('alignment').attr("wrapText", "1");
						}else{
							$('xf:eq('+currentStyle+')', el).append(`<alignment wrapText="1"/>`);
						}

						addedWordWraps.push(currentStyle);
					}
				} catch (ex) {}

				rowNode.appendChild( cell );

				lastData = dataOut;
			}

			if(calculatePivotColspan){
				popIndexArray.reverse().forEach(remArr=>{
					rowNode.removeChild(rowNode.childNodes[remArr]);
					pivotMergeValues.splice(remArr, 1);
					// [pushHeaderArray[remArr], pushHeaderArray[remArr + 1]] = [pushHeaderArray[remArr + 1], {}];
					// rowNode.childNodes[remArr];
				});

				let lastPivotMerge = 0
				if(pivotMergeValues.length == rowNode.childNodes.length){
					pivotMergeValues.forEach((mergeCount, mergeIndex)=>{
						rowNode.childNodes[mergeIndex].setAttribute("r", createCellPos(lastPivotMerge) + '' + currentRow);
						// rowNode.childNodes[mergeIndex].setAttribute("s", "202");
						
						mergeCells( currentRow, mergeCount + lastPivotMerge - 1, lastPivotMerge, false);

						lastPivotMerge = mergeCount + lastPivotMerge;
					})
				}
			}

			// pivotMergeValues = [];
			// popIndexArray = [];
            // lastData = undefined;
            // thisColSpan = 1;

			relsGet.appendChild(rowNode);
			rowPos++;
		};

		if ( config.customizeData ) {
			config.customizeData( data );
		}

		var mergeCells = function ( row, colspan, startColumnIndex, alignCenter) {
			if(typeof startColumnIndex == "undefined"){
				startColumnIndex = 0;
			}
			
			if(typeof alignCenter == "undefined"){
				alignCenter = true;
			}
			
			var mergeCells = $('mergeCells', rels);

			mergeCells[0].appendChild( _createNode( rels, 'mergeCell', {
				attr: {
					ref: createCellPos(startColumnIndex)+row+':'+createCellPos(colspan)+row
				}
			} ) );
			mergeCells.attr( 'count', parseFloat(mergeCells.attr( 'count' ))+1 );
			if(alignCenter){
				$('row:eq('+(row-1)+') c', rels).attr( 's', '51' ); // centre
			}
		};

		// Title and top messages
		var exportInfo = dt.buttons.exportInfo( config );
		// if ( exportInfo.title ) {
		// 	addRow( [exportInfo.title, ...[...new Array(data.header.length-1)].map(arr=>"")], rowPos );
		// 	mergeCells( rowPos, data.header.length-1 );
		// }

		if ( exportInfo.messageTop ) {
			addRow( [exportInfo.messageTop, ...[...new Array(data.header.length-1)].map(arr=>"")], undefined );
			mergeCells( rowPos, data.header.length-1 );
		}

		if (config && config.exportHF) {
			let hideExportAppTitle = typeof config.exportHF.hideprojname !== "undefined" && config.exportHF.hideprojname.substr(1).toLowerCase() === "t" || false; // default false			

			if ( !hideExportAppTitle && config.exportHF.exportAppTitle ) {
				addRow([config.exportHF.exportAppTitle, ...[...new Array(data.header.length-1)].map(arr=>"")], undefined);
				mergeCells( rowPos, data.header.length-1 );
				// $('row c', rels).attr('s', '2'); // bold
				$('row:eq('+(rowPos-1)+') c', rels).attr( 's', '202' );
			}

			if(config.exportHF.header && Object.keys(config.exportHF.header).length > 0){
				let alignStyle = '202'; // Default is center alignment
				const headerAlign = config.exportHF.header["h1"]?.header_aline;
				if (headerAlign) {
					alignStyle = { "left": '102', "center": '202', "right": '302' }[headerAlign.substr(1).toLowerCase()] || alignStyle;
				}

				Object.keys(config.exportHF.header).forEach(function (val) {
					const headerConfig = config.exportHF.header[val];					
					if (headerConfig.text?.trim()) {
						addRow([headerConfig.text?.trim(), ...[...new Array(data.header.length - 1)].map(arr => "")], undefined);
						mergeCells(rowPos, data.header.length - 1);						
			
						$('row:eq(' + (rowPos - 1) + ') c', rels).attr('s', alignStyle);
					}
				});
			}

			// if ((!hideExportAppTitle && config.exportHF.exportAppTitle) || (config.exportHF.header && Object.keys(config.exportHF.header).length > 0)) {
			// 	addRow(["", ...[...new Array(data.header.length-1)].map(arr=>"")], undefined); // add empty row after adding project-title report-header if exists any
			// 	mergeCells( rowPos, data.header.length-1 );
			// 	// $('row c', rels).attr('s', '2'); // bold
			// 	$('row:eq('+(rowPos-1)+') c', rels).attr( 's', '202' );
			// }
		}

		let hideExportDate = typeof config.exportHF.hidereportdate !== "undefined" && config.exportHF.hidereportdate.substr(1).toLowerCase() === "t" || false; // default false

		//add date to excel
		if (!hideExportDate) {
			addRow(["Date: " + (new Date()).format(dtCulture == "en-us" ? "MM/dd/yyyy" : "dd/MM/yyyy"), ...[...new Array(data.header.length-1)].map(arr=>"")], undefined);
			mergeCells( rowPos, data.header.length-1 );
			$('row:eq('+(rowPos-1)+') c', rels).attr( 's', '52' );
		}		

		var paramString = processParamString().replace(/, /g, "; ").trim();
		if (paramString) {
			let paramArr = paramString.split(';').map(val=>val.trim()).filter(val=>val);
			paramArr.forEach(function(val){
				addRow([val, ...[...new Array(data.header.length-1)].map(arr=>"")], undefined);
				mergeCells( rowPos, data.header.length-1 );
				$('row:eq('+(rowPos-1)+') c', rels).attr( 's', '50' );
			});
		}

		// Table itself
		if ( config.header ) {
			var headerMatrix = _fnGetHeaders(dt);
			let pivotColSpan = false;
			for ( var rowIdx = 0;  rowIdx < headerMatrix.length;  rowIdx++ ) {
				if(headerMatrix.length > 1 && rowIdx != headerMatrix.length - 1){
					pivotColSpan = true;
				}else{
					pivotColSpan = false;
				}
				
				addRow(headerMatrix[rowIdx], pivotColSpan,  !pivotColSpan ? '2' : '202');
				$('row:last c', rels).attr( 's', !pivotColSpan ? '2' : '202' ); // bold
			}
			// addRow( data.header, undefined );
			// $('row c', rels).attr( 's', '2' ); // bold
			// $('row:last c', rels).attr( 's', '2' ); // bold
		}
	
		dataStartRow = rowPos;

		var colProp = ivirDataTableApi.columns().visible().toArray().map((vis, ind)=>{
			return {vis, ind}
		}).filter(col=>{
			return col.ind == 0 && isChkBox == "true" ? false : col.vis
		}).map(col=>{
			return {name: FieldName[col.ind], type: ColumnType[col.ind], dec: (ivHeadRows[FieldName[col.ind]]["@dec"])};
		});

		for (var n = 0, ie = data.body.length; n < ie; n++) {
			// var specialRow = ivDatas[n]["axrowtype"] || "";
			var thisRowData = ivDatas[n];
			var specialRowStyle = "";
			if (typeof thisRowData["axrowtype"] != "undefined") {
				switch (thisRowData["axrowtype"]) {
					case "stot":
						// $(data).attr("font-color", "blue");
						// $('c', data).attr( 's', '68' );
						specialRowStyle = '68';
						break;
					case "subhead":
						// $(data).attr("font-color", "black");
						// $('c', data).attr( 's', '69' );
						specialRowStyle = '69';
						break;
					case "gtot":
						// $(data).attr("font-color", "green");
						// $('c', data).attr( 's', '70' );
						specialRowStyle = '70';
						break;
				}
			}
			addRow( data.body[n], undefined, specialRowStyle, data.bodyMeta, colProp, thisRowData );
		}
	
		dataEndRow = rowPos;

		if ( config.footer && data.footer ) {
			var footerMatrix = _fnGetFooters(dt);
            		for ( var rowIdx = 0;  rowIdx < footerMatrix.length;  rowIdx++ ) {
                		addRow( footerMatrix[rowIdx], undefined );
            		}
			//addRow( data.footer, rowPos);
			$('row:last c', rels).attr( 's', '2' ); // bold
		}
		if (config && config.exportHF) {
			// if (config.exportHF.footer && Object.keys(config.exportHF.footer).length > 0) {
			// 	addRow(["", ...[...new Array(data.header.length-1)].map(arr=>"")], undefined); // add empty row before adding report-footer if exists
			// 	mergeCells( rowPos, data.header.length-1 );
			// 	// $('row:last c', rels).attr( 's', '2' );
			// 	$('row:eq('+(rowPos-1)+') c', rels).attr( 's', '51' );
			// }

			if(config.exportHF.footer && Object.keys(config.exportHF.footer).length > 0){
				let alignStyle = '51'; // Default is center alignment
				const footerAlign = config.exportHF.footer["f1"]?.footer_aline;
				if (footerAlign) {
					alignStyle = { "left": '50', "center": '51', "right": '52' }[footerAlign.substr(1).toLowerCase()] || alignStyle;
				}

				Object.keys(config.exportHF.footer).forEach(function (val) {
					const footerConfig = config.exportHF.footer[val];
										
					if (footerConfig.text?.trim()) {
						addRow([footerConfig.text?.trim(), ...[...new Array(data.header.length - 1)].map(arr => "")], undefined);
						mergeCells(rowPos, data.header.length - 1);						
						// $('row:last c', rels).attr( 's', '2' );
						$('row:eq(' + (rowPos - 1) + ') c', rels).attr('s', alignStyle);
					}
				});
			}
		}

		// Below the table
		if ( exportInfo.messageBottom ) {
			addRow( [exportInfo.messageBottom, ...[...new Array(data.header.length-1)].map(arr=>"")], undefined );
			mergeCells( rowPos, data.header.length-1 );
		}

		// Set column widths
		var cols = _createNode( rels, 'cols' );
		$('worksheet', rels).prepend( cols );

		for ( var i=0, ien=data.header.length ; i<ien ; i++ ) {
			cols.appendChild( _createNode( rels, 'col', {
				attr: {
					min: i+1,
					max: i+1,
					width: _excelColWidth( data, i ),
					customWidth: 1
				}
			} ) );
		}

		// Workbook modifications
		var workbook = xlsx.xl['workbook.xml'];

		$( 'sheets sheet', workbook ).attr( 'name', _sheetname( config ) );

		// Auto filter for columns
		if ( config.autoFilter ) {
			$('mergeCells', rels).before( _createNode( rels, 'autoFilter', {
				attr: {
					ref: 'A'+dataStartRow+':'+createCellPos(data.header.length-1)+dataEndRow
				}
			} ) );

			$('definedNames', workbook).append( _createNode( workbook, 'definedName', {
				attr: {
					name: '_xlnm._FilterDatabase',
					localSheetId: '0',
					hidden: 1
				},
				text: _sheetname(config)+'!$A$'+dataStartRow+':'+createCellPos(data.header.length-1)+dataEndRow
			} ) );
		}

		var sheet = xlsx.xl.worksheets['sheet1.xml'];
		var col = $('col', sheet);
		ivirDataTableApi.columns().visible().toArray().map((vis, ind)=>{
			return {vis, ind}
		}).filter(col=>{
			return col.ind == 0 && isChkBox == "true" ? false : col.vis
		}).map(col=>ivHeadRows[FieldName[col.ind]]["@width"]||minCellWidth).forEach((width, index)=>{
			var finalwidth = Math.floor(width/5);
			if(finalwidth > 54){
				finalwidth = 54;
			}
			$(col[index]).attr('width', finalwidth);
		});

		// Let the developer customise the document if they want to
		if ( config.customize ) {
			config.customize( xlsx, config, dt );
		}

		// Excel doesn't like an empty mergeCells tag
		if ( $('mergeCells', rels).children().length === 0 ) {
			$('mergeCells', rels).remove();
		}

		var jszip = _jsZip();
		var zip = new jszip();
		var zipConfig = {
			type: 'blob',
			mimeType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
		};

		_addToZip( zip, xlsx );

		if ( zip.generateAsync ) {
			// JSZip 3+
			zip
				.generateAsync( zipConfig )
				.then( function ( blob ) {
					_saveAs( blob, exportInfo.filename );
					that.processing( false );
				} );
		}
		else {
			// JSZip 2.5
			_saveAs(
				zip.generate( zipConfig ),
				exportInfo.filename
			);
			this.processing( false );
		}
	},

	filename: '*',

	extension: '.xlsx',

	exportHF: {},

	exportOptions: {},

	header: true,

	footer: false,

	title: '*',

	messageTop: '*',

	messageBottom: '*',

	createEmptyCells: false,

	autoFilter: false,

	sheetName: ''
};

//
// PDF export - using pdfMake - http://pdfmake.org
//
DataTable.ext.buttons.pdfHtml5 = {
	className: 'buttons-pdf buttons-html5',

	available: function () {
		return window.FileReader !== undefined && _pdfMake();
	},

	text: function ( dt ) {
		return dt.i18n( 'buttons.pdf', 'PDF' );
	},

	action: function ( e, dt, button, config ) {
		// window.jsPDF = window.jspdf.jsPDF;
		// var bidiEngine = new jsPDF.__bidiEngine__();
		// var bidiEngine = new jsPDF.__bidiEngine__({isInputVisual: true});
		// var bidiEngine = new jsPDF.__bidiEngine__({isInputVisual: true, isOutputVisual: true, isInputRtl: false, isOutputRtl: true, isSymmetricSwapping: true});
		// var bidiEngine = new jsPDF.__bidiEngine__({isInputVisual: true,isSymmetricSwapping: false});
		// var bidiEngine = new jsPDF.__bidiEngine__({isInputVisual: true,isInputRtl: false,isOutputRtl: false});
		// bidiEngine.doBidiReorder(`كرسي بلاستيك - موديل 9091`);//"9091 ﻞﻳﺩﻮﻣ - ﻚﻴﺘﺳﻼﺑ ﻲﺳﺮﻛ"
		//jsPDF.API.processArabic(`كرسي بلاستيك - موديل 9091`)

		// {
		// 	`كرسي بلاستيك - موديل 9091`//string;
		// 	`9091 موديل - بلاستيك كرسي`//expectedStringOld;//25 char
		// 	`9091 موديل - بلاستيك كرسي`//expectedStringOld;//24 char


		// 	'9091 ﻞﻳﺩﻮﻣ - ﻚﻴﺘﺳﻼﺑ ﻲﺳﺮﻛ'//new processing
		// }

		var isArabicText = (text) => {
			var pattern = /[\u0600-\u06FF\u0750-\u077F]/;
			result = pattern.test(text);
			return result;
		}


		var processTextDirection = (text, align = false, ind) => {
			let processArabic = pdfExportAllLanguageSupport && isArabicText(text);

			// return bidiEngine.doBidiReorder(jsPDF.API.processArabic(text));
			let { bodyMeta } = data;
			let alignment = "left";
			if(bodyMeta.length > 0 && align){
				alignment = (ivHeadRows[bodyMeta[ind]]["@align"] || (bodyMeta[ind] == "rowno" && !enableCardsUi ? "Center" : "Left")).toLowerCase();
			}

			if(processArabic){
				return {
					text: text.split(" ").reverse().join(" "),
					...(align ? { alignment } : {}),
				};
			}else{
				return {
					text,
					...(align ? { alignment } : {}),
				};
			}
		};

		this.processing( true );

		var that = this;
		var data = dt.buttons.exportData( config.exportOptions );
		var info = dt.buttons.exportInfo( config );
		var rows = [];

		var headerRows = 1;
		var pivotHeaderExist = false;
		if ( config.header ) {
			var headerMatrix = _fnGetHeaders(dt);
			headerRows = headerMatrix.length;
			if(headerRows > 1){
				pivotHeaderExist = true;
			}
			let pivotColSpan = false;
			for ( var rowIdx = 0;  rowIdx < headerMatrix.length;  rowIdx++ ) {
				if(headerMatrix.length > 1 && rowIdx != headerMatrix.length - 1){
					pivotColSpan = true;
				}else{
					pivotColSpan = false;
				}

				let popIndexArray = [];
				let lastData = undefined;
				let thisColSpan = 1;

				let pushHeaderArray =  $.map( headerMatrix[rowIdx], function ( d, k ) {
					var dataOut = typeof d === 'string' ? d : d+'';

					if(pivotColSpan && lastData !== undefined && lastData !== "" && dataOut === lastData){
						// delete headerMatrix[rowIdx]
						popIndexArray.push(k - 1);
						
						thisColSpan++;
					}else{
						thisColSpan = 1;
					}

					lastData = dataOut;

					return {
						style: 'tableHeader',
						...(pivotColSpan ? {
							colSpan: thisColSpan,
							alignment: 'center'
						} : {}),
						...(processTextDirection(dataOut, !pivotColSpan, k) || {}),
					};
				} );

				if(pivotColSpan){
					// pushHeaderArray = pushHeaderArray.map((obj, ind) => popIndexArray.indexOf(ind) < 0 ? obj : {});
					popIndexArray.reverse().forEach(remArr=>{
						[pushHeaderArray[remArr], pushHeaderArray[remArr + 1]] = [pushHeaderArray[remArr + 1], {}]
					})
				}

				// popIndexArray = [];
				// lastData = undefined;
				// thisColSpan = 1;

				rows.push(pushHeaderArray);
			}
			// rows[0] = JSON.parse(`[{"text":"","style":"tableHeader","colSpan":1,"alignment":"center"},{"text":"","style":"tableHeader","colSpan":1,"alignment":"center"},{"text":"  Jan","style":"tableHeader","colSpan":2,"alignment":"center"},{},{"text":"  Feb","style":"tableHeader","colSpan":2,"alignment":"center"},{},{"text":"  Mar","style":"tableHeader","colSpan":2,"alignment":"center"},{}]`);

			// rows.push( $.map( data.header, function ( d ) {
			// 	return {
			// 		text: typeof d === 'string' ? d : d+'',
			// 		style: 'tableHeader'
			// 	};
			// } ) );
			
			// if(pivotHeaderExist){
			// 	headerRows++;	
			// 	rows.unshift($.map( headerMatrix[0], function ( d, k ) {
			// 		return {
			// 			text: "",
			// 			style: 'tableHeaderDummy',
			// 			margin: [ 0, 0, 0, 0 ]
			// 		};
			// 	}));
			// }
		}

		// {rows.push( $.map( data.body[0], function ( d ) {
		// 		d=`9091 موديل - بلاستيك كرسي`;
		// 		return {
		// 			text: d,
		// 			// style: i % 2 ? 'tableBodyEven' : 'tableBodyOdd'
		// 			style: specialRowStyle || (i % 2 ? 'tableBodyEven' : 'tableBodyEven')
		// 		};
		// 	} ) );
		// }

		for (var i = 0, ien = data.body.length; i < ien; i++) {
			var thisRowData = ivDatas[i];
			var specialRowStyle = "";
			if (typeof thisRowData["axrowtype"] != "undefined") {
				switch (thisRowData["axrowtype"]) {
					case "stot":
						specialRowStyle = 'stot';
						break;
					case "subhead":
						specialRowStyle = 'subhead';
						break;
					case "gtot":
						specialRowStyle = 'gtot';
						break;
				}
			}
			rows.push( $.map( data.body[i], function ( d, k ) {
				if ( d === null || d === undefined ) {
					d = '';
				}
				return {
					...(processTextDirection(typeof d === 'string' ? d : d+'', true, k) || {}),
					// style: i % 2 ? 'tableBodyEven' : 'tableBodyOdd'
					style: specialRowStyle || (i % 2 ? 'tableBodyEven' : 'tableBodyEven'),
				};
			} ) );
		}

		if ( config.footer && data.footer) {
			var footerMatrix = _fnGetFooters(dt);
			for ( var rowIdx = 0;  rowIdx < footerMatrix.length;  rowIdx++ ) {
				rows.push( $.map( footerMatrix[rowIdx], function ( d, k ) {
				return {
					...(processTextDirection(typeof d === 'string' ? d : d+'', true, k) || {}),
					style: 'tableFooter'
				};
			} ) );
		}
			// rows.push( $.map( data.footer, function ( d ) {
			// 	return {
			// 		text: typeof d === 'string' ? d : d+'',
			// 		style: 'tableFooter'
			// 	};
			// } ) );
		}

		pdfMake.fonts = {
			Roboto: {
				normal: 'Roboto-Regular.ttf',
				bold: 'Roboto-Medium.ttf',
				italics: 'Roboto-Italic.ttf',
				bolditalics: 'Roboto-MediumItalic.ttf'
			},
			ArialUnicodeMsFont: {
				normal: 'ArialUnicodeMsFont.ttf',
				bold: 'ArialUnicodeMsFont.ttf',
				italics: 'ArialUnicodeMsFont.ttf',
				bolditalics: 'ArialUnicodeMsFont.ttf'
			}
		}
		let alignExportHF = { header: "center", footer: "center" }; // Default is center alignment
		if (config && config.exportHF) {
			if (config.exportHF.header && Object.keys(config.exportHF.header).length > 0) {				
				const headerAlign = config.exportHF.header["h1"]?.header_aline;
				if (headerAlign) {
					alignExportHF.header = { "left": "left", "center": "center", "right": "right" }[headerAlign.substr(1).toLowerCase()] || "center";
				}
			}
			if (config.exportHF.footer && Object.keys(config.exportHF.footer).length > 0) {				
				const footerAlign = config.exportHF.footer["f1"]?.footer_aline;
				if (footerAlign) {
					alignExportHF.footer = { "left": "left", "center": "center", "right": "right" }[footerAlign.substr(1).toLowerCase()] || "center";
				}
			}
		}
		

		var doc = {
			pageSize: config.pageSize,
			pageOrientation: config.orientation,
			content: [
				{
					table: {
						headerRows,
						body: rows
					},
					// layout: 'noBorders'
					layout: {
						// hLineWidth: function (i, node) { return 0; },  // Set the horizontal line width
						vLineWidth: function (i, node) { return 0.5; },  // Set the vertical line width
						// hLineColor: function (i, node) { return '#000000'; },  // Line color
						// vLineColor: function (i, node) { return '#000000'; },  // Line color
						paddingLeft: function (i, node) { return 2; },  // Padding left for each cell
						paddingRight: function (i, node) { return 2; },  // Padding right for each cell
						paddingTop: function (i, node) { return 0; },  // Remove top padding
						paddingBottom: function (i, node) { return 0; },  // Remove bottom padding
					}
				}
			],
			styles: {
				exportHeader: {
					bold: true,
					fontSize: 11,
					alignment: alignExportHF.header
				},
				exportFooter: {
					bold: true,
					fontSize: 11,
					alignment: alignExportHF.footer
				},
				paramString: {
					bold: false,
					fontSize: 11,
					// color: 'white',
					// fillColor: '#2d4154',
					alignment: 'left'
				},
				stot: {
					bold: true,
					fontSize: 11,
					color: '#0000ff',
				},
				subhead: {
					bold: true,
					fontSize: 11,
					color: '#000000',
				},
				gtot: {
					bold: true,
					fontSize: 11,
					color: '#1f911f',
				},
				tableHeader: {
					bold: true,
					fontSize: 11,
					color: 'white',
					fillColor: '#2d4154',
					alignment: 'center'
				},
				tableHeaderDummy: {
					bold: true,
					fontSize: 1,
					color: 'white',
					fillColor: '#2d4154',
					alignment: 'center'
				},
				tableBodyEven: {},
				tableBodyOdd: {
					fillColor: '#f3f3f3'
				},
				tableFooter: {
					bold: true,
					fontSize: 11,
					color: 'white',
					fillColor: '#2d4154'
				},
				title: {
					alignment: 'center',
					fontSize: 15,
					bold: true
				},
				message: {}
			},
			defaultStyle: {
				fontSize: 10
			}
		};

		if (config && config.exportHF) {
			// if (config.exportHF.footer && Object.keys(config.exportHF.footer).length > 0) {
			// 	doc.content.push( {
			// 		...(processTextDirection(" ") || {}),
			// 		style: 'title',
			// 		// margin: [ 0, 0, 0, 12 ]
			// 	} );
			// }

			if(config.exportHF.footer && Object.keys(config.exportHF.footer).length > 0){
				Object.keys(config.exportHF.footer).forEach(function(val){
					if (config.exportHF.footer[val].text?.trim()) {
						doc.content.push( {
							...(processTextDirection(config.exportHF.footer[val].text?.trim()) || {}),
							style: 'exportFooter',
							// margin: [ 0, 0, 0, 12 ]
						} );
					}
				});
			}
		}


		var paramString = processParamString().replace(/, /g, "; ").trim();
		if (paramString) {
			doc.content.unshift( {
				...(processTextDirection(paramString) || {}),
				style: 'paramString',
				// margin: [ 0, 0, 0, 12 ]
			} );
		}

		if (config && config.exportHF) {
			let hideExportAppTitle = typeof config.exportHF.hideprojname !== "undefined" && config.exportHF.hideprojname.substr(1).toLowerCase() === "t" || false; // default value false
			
			// if ((!hideExportAppTitle && config.exportHF.exportAppTitle) || (config.exportHF.header && Object.keys(config.exportHF.header).length > 0)) {
			// 	doc.content.unshift( {
			// 		...(processTextDirection(" ") || {}),
			// 		style: 'title',
			// 		// margin: [ 0, 0, 0, 12 ]
			// 	} );
			// }

			if(config.exportHF.header && Object.keys(config.exportHF.header).length > 0){
				Object.keys(config.exportHF.header).reverse().forEach(function(val){
					if (config.exportHF.header[val].text?.trim()) {
						doc.content.unshift( {
							...(processTextDirection(config.exportHF.header[val].text?.trim()) || {}),
							style: 'exportHeader',
							// margin: [ 0, 0, 0, 12 ]
						} );
					}
				});
			}

			if (!hideExportAppTitle && config.exportHF.exportAppTitle) {
				// addRow([config.exportHF.exportAppTitle], rowPos);
				// $('row c', rels).attr('s', '2'); // bold

				doc.content.unshift( {
					...(processTextDirection(config.exportHF.exportAppTitle) || {}),
					style: 'title',
					// margin: [ 0, 0, 0, 12 ]
				} );
			}
		}

		if ( info.messageTop ) {
			doc.content.unshift( {
				...(processTextDirection(info.messageTop) || {}),
				style: 'message',
				margin: [ 0, 0, 0, 12 ]
			} );
		}

		if ( info.messageBottom ) {
			doc.content.push( {
				...(processTextDirection(info.messageBottom) || {}),
				style: 'message',
				margin: [ 0, 0, 0, 12 ]
			} );
		}

		// if ( info.title ) {
		// 	doc.content.unshift( {
		// 		text: info.title,
		// 		style: 'title',
		// 		margin: [ 0, 0, 0, 12 ]
		// 	} );
		// }

		if ( config.customize ) {
			config.customize( doc, config, dt );
		}
		
		var _that = this;

		try{
			if (!config.allLanguageSupport){
				throw("English Only Supported.");
			}

			loadAndCall({
				files: {
					css: [],
					js: [
						"/ThirdParty/DataTables-1.10.13/extensions/Buttons/js/fonts/vfs_aioFonts.js"
					]
				},
				callBack() {
					doc.defaultStyle.font = "ArialUnicodeMsFont";

					config.process(_that, doc, config, info);
				}
			});
		}catch(ex){
			config.process(_that, doc, config, info);
		}
	},

	title: '*',

	filename: '*',

	extension: '.pdf',

	exportHF: {},

	allLanguageSupport: false,

	exportOptions: {},

	orientation: 'portrait',

	pageSize: 'A4',

	header: true,

	footer: false,

	messageTop: '*',

	messageBottom: '*',

	customize: null,
	
	process(that, doc, config, info){
		var pdf = _pdfMake().createPdf( doc );

		if ( config.download === 'open' && ! _isDuffSafari() ) {
			pdf.open();
		}
		else {
			pdf.download( info.filename );
		}

		that.processing( false );
	},

	download: 'download'
};


return DataTable.Buttons;
}));

var _fnGetHeaders = function(dt) {
    var thRows = $(dt.header()[0]).children();
    var numRows = thRows.length;
    var matrix = [];

    // Iterate over each row of the header and add information to matrix.
    for ( var rowIdx = 0;  rowIdx < numRows;  rowIdx++ ) {
        var $row = $(thRows[rowIdx]);

        // Iterate over actual columns specified in this row.
        var $ths = $row.children("th");
        for ( var colIdx = 0;  colIdx < $ths.length;  colIdx++ )
		{
			var $th = $($ths.get(colIdx)).clone();
			var colspan = $th.attr("colspan") || 1;
			var rowspan = $th.attr("rowspan") || 1;
			var colCount = 0;

			// ----- add this cell's title to the matrix
			if (matrix[rowIdx] === undefined) {
				matrix[rowIdx] = [];  // create array for this row
			}
			// find 1st empty cell
			for (var j = 0; j < (matrix[rowIdx]).length; j++, colCount++) {
				if (matrix[rowIdx][j] === "PLACEHOLDER") {
					break;
				}
			}
			var myColCount = colCount;
			$th.find(".rightClickMenuIcn").remove();
			matrix[rowIdx][colCount++] = $th.text().trim();

			// ----- If title cell has colspan, add empty titles for extra cell width.
			for (var j = 1; j < colspan; j++) {
				matrix[rowIdx][colCount++] = matrix[rowIdx][colCount - 2];
			}

			// ----- If title cell has rowspan, add empty titles for extra cell height.
			for (var i = 1; i < rowspan; i++) {
				var thisRow = rowIdx + i;
				if (matrix[thisRow] === undefined) {
					matrix[thisRow] = [];
				}
				// First add placeholder text for any previous columns.                 
				for (var j = (matrix[thisRow]).length; j < myColCount; j++) {
					matrix[thisRow][j] = "PLACEHOLDER";
				}
				for (var j = 0; j < colspan; j++) {  // and empty for my columns
					matrix[thisRow][myColCount + j] = "";
				}
			}
        }
    }
	if (matrix.length > 0) {
		matrix = matrix.map(function (val, ind) {
			val = val.filter(function (val2, ind2) { 
				var isVisible = true;
				if (ind2 == 0 && (isChkBox == "true" || rowOptionsExist)) {
					isVisible = false;
				}
				return isVisible;
			});
			return val;
		});
	}
    return matrix;
};


var _fnGetFooters = function(dt) {
    var thRows = $(dt.footer()[0]).children();
    var numRows = thRows.length;
    var matrix = [];

    // Iterate over each row of the header and add information to matrix.
    for ( var rowIdx = 0;  rowIdx < numRows;  rowIdx++ ) {
        var $row = $(thRows[rowIdx]);

        // Iterate over actual columns specified in this row.
        var $ths = $row.children("th");
        for ( var colIdx = 0;  colIdx < $ths.length;  colIdx++ )
        {
            var $th = $($ths.get(colIdx)).clone();
            var colspan = $th.attr("colspan") || 1;
            var rowspan = $th.attr("rowspan") || 1;
            var colCount = 0;

            // ----- add this cell's title to the matrix
            if (matrix[rowIdx] === undefined) {
                matrix[rowIdx] = [];  // create array for this row
            }
            // find 1st empty cell
            for ( var j = 0;  j < (matrix[rowIdx]).length;  j++, colCount++ ) {
                if ( matrix[rowIdx][j] === "PLACEHOLDER" ) {
                    break;
                }
            }
            var myColCount = colCount;
			$th.find(".rightClickMenuIcn").remove();
            matrix[rowIdx][colCount++] = $th.text().trim();

            // ----- If title cell has colspan, add empty titles for extra cell width.
            for ( var j = 1;  j < colspan;  j++ ) {
                matrix[rowIdx][colCount++] = matrix[rowIdx][colCount - 2];
            }

            // ----- If title cell has rowspan, add empty titles for extra cell height.
            for ( var i = 1;  i < rowspan;  i++ ) {
                var thisRow = rowIdx+i;
                if ( matrix[thisRow] === undefined ) {
                    matrix[thisRow] = [];
                }
                // First add placeholder text for any previous columns.                 
                for ( var j = (matrix[thisRow]).length;  j < myColCount;  j++ ) {
                    matrix[thisRow][j] = "PLACEHOLDER";
                }
                for ( var j = 0;  j < colspan;  j++ ) {  // and empty for my columns
                    matrix[thisRow][myColCount+j] = "";
                }
            }
        }
    }
	if (matrix.length > 0) {
		matrix = matrix.map(function (val, ind) {
			val = val.filter(function (val2, ind2) { 
				var isVisible = true;
				if (ind2 == 0 && (isChkBox == "true" || rowOptionsExist)) {
					isVisible = false;
				}
				return isVisible;
			});
			return val;
		});
	}
    return matrix;
};

/**
 * @description: create styles for font fomatting
 * @author Prashik
 * @date 18/11/2021
 * @param {*} currentStyle: style number
 * @param {*} applyStyle: style string
 * @param {*} styles: style object
 * @return {*}  
 */
function SetExcelFontCondition(currentStyle, applyStyle, styles) {
	var stylesChanged = false;

	var cellStyles = $('cellXfs', styles);
	var currentCellStyleNode = $('xf:eq('+currentStyle+')', cellStyles);
	
	var fontId = currentCellStyleNode.attr("fontId");
	var fillId = currentCellStyleNode.attr("fillId");

	var cellFonts = $('fonts', styles);
	var currentFontClone = $('font:eq('+fontId+')', cellFonts).clone();

	var cellFills = $('fills', styles);
	var currentFillClone = $('fill:eq('+fillId+')', cellFills).clone();

    if (applyStyle != "") {
        applyStyle = applyStyle.split(',');
        for (var y = 0; y < applyStyle.length; y++) {
            var currentSplittedStyle = applyStyle[y].split('=');
            switch (currentSplittedStyle[0].toLowerCase()) {
                case "fontname":
					if(currentFontClone.find("name").length == 0){
						currentFontClone.append(`<name val="Calibri" />`);
					}
					currentFontClone.find("name").attr("val", currentSplittedStyle[1]);
					stylesChanged = true;
                    break;
                case "fontsize":
					if(currentFontClone.find("sz").length == 0){
						currentFontClone.append(`<color val="11" />`);
					}
					currentFontClone.find("sz").attr("val", currentSplittedStyle[1]);
					stylesChanged = true;
                    break;
                case "fontstyle":
                    for (var z = 0; z < currentSplittedStyle[1].toLowerCase().length; z++) {
                        switch (currentSplittedStyle[1].toLowerCase()[z]) {
                            case "b":
								if(currentFontClone.find("b").length == 0){
									currentFontClone.append(`<b />`);
								}
								stylesChanged = true;
                                break;
                            case "i":
								if(currentFontClone.find("i").length == 0){
									currentFontClone.append(`<i />`);
								}
								stylesChanged = true;
                                break;
                            case "u":
								if(currentFontClone.find("u").length == 0){
									currentFontClone.append(`<u />`);
								}
								stylesChanged = true;
                                break;
                            case "s":
								if(currentFontClone.find("strike").length == 0){
									currentFontClone.append(`<strike />`);
								}
								stylesChanged = true;
                                break;
                        }
                    }
                    break;
                case "fontcolor":
					if(currentFontClone.find("color").length == 0){
						currentFontClone.append(`<color rgb="" />`);
					}

					var hexColor = AxClColors[currentSplittedStyle[1]]

					if(hexColor && hexColor.startsWith("#")){
						currentFontClone.find("color").attr("rgb", `FF${hexColor.substr(1)}`);
					}else if(currentSplittedStyle[1].startsWith("#")){
						currentFontClone.find("color").attr("rgb", `FF${currentSplittedStyle[1].substr(1)}`);
					}

					stylesChanged = true;
                    break;
                case "backcolor":
					if(currentFillClone.find("patternFill").length == 0){
						currentFillClone.append(`<patternFill patternType="none" />`);
					}

					if(currentFillClone.find("patternFill").attr("patternType") != "solid"){
						currentFillClone.find("patternFill").attr("patternType", "solid");
					}

					currentFillClone.find("patternFill").empty();



					currentFillClone.find("patternFill").append(`<fgColor rgb="00ffffff" /><bgColor indexed="64" />`);

					let fgColor = "";

                    if (AxClColors[currentSplittedStyle[1]]) {
						fgColor = AxClColors[currentSplittedStyle[1]];
						stylesChanged = true;
                    } else if (currentSplittedStyle[1].indexOf("#") == 0) {
						// fgColor = $("<div />").spectrum({color: currentSplittedStyle[1]}).spectrum("get").toHex();
						fgColor = currentSplittedStyle[1];
						stylesChanged = true;
                    } else {
						currentFillClone.find("patternFill").attr("patternType", "none");
						stylesChanged = true;
                    }

					if(fgColor.startsWith("#")){
						currentFillClone.find("patternFill").find("fgColor").attr("rgb", `FF${fgColor.substr(1)}`)
					}

                    break;
            }
        }
    }

	if(stylesChanged){
		cellFonts.append(currentFontClone);
		var cellFontsNewId = cellFonts.children().length - 1;
		cellFonts.attr("count", cellFontsNewId + 1);

		cellFills.append(currentFillClone);
		var cellFillsNewId = cellFills.children().length - 1;
		cellFills.attr("count", cellFillsNewId + 1);


		var newCellStyleNode = $('xf:eq('+currentStyle+')', cellStyles).clone().attr("fontId", cellFontsNewId).attr("fillId", cellFillsNewId)[0].outerHTML.replace(/xmlns=\"(.*?)\"/g, '');

		cellStyles.append(newCellStyleNode).attr('count', parseInt(cellStyles.attr('count'))+1);

		currentStyle = $('xf', cellStyles).length - 1;
	}

	return currentStyle;
}
