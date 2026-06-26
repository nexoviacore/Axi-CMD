var _customPlugins;
var _codeMirrorEdit;
var _codeMirrorNewPlugin;
var _entityCommon;
var dtCulture = eval(callParent('glCulture'));

class CustomPlugins {
    constructor() {
        this.appName = window.top.mainProject || "agileerpdemo";
        this.navMenuHTML =
            `<div class="Data-Group_Items group-all" data-nav="{{caption}}" onclick="_customPlugins.loadFileTypes(this, '{{caption}}')">
                <a href="#" class="group-item">
                    <div class="d-flex">
                        <div class="symbol symbol-40px symbol-circle me-5" style="margin-left: 5px !important;">
                            <div class="symbol-label bgs1">
                                <span class="material-icons material-icons-style material-icons-2">{{icon}}</span>
                            </div>
                        </div>
                        <div class="d-flex flex-column y-axis-caption">
                            <span class="Data-Group-name">{{caption}}</span>
                        </div>
                    </div>
                </a>
            </div>`;
        this.fileTypeHTML =
            `<li class="nav-item" data-menuname="{{menuname}}" data-menutype="{{menutype}}" data-filetype="{{filetype}}" onclick="_customPlugins.loadFileContent(this, '{{filepath}}' )">
                <a class="nav-link" href="#">{{filetype}}</a>
            </li>`;
        this.filePropsHTML =
            `<div class="fs-6 fw-bold">
                <span class="text-gray-700 text-hover-primary">Created on: </span>
                <span class="text-muted">{{CreatedOn}}</span>
            </div>
            <div class="fs-6 fw-bold">
                <span class="text-gray-700 text-hover-primary">Modified on: </span>
                <span class="text-muted">{{ModifiedOn}}</span>
            </div>
            <div class="fs-6 fw-bold">
                <span class="text-gray-700 text-hover-primary">File Size: </span>
                <span class="text-muted">{{FileSize}} KBs</span>
            </div>
            <div class="fs-6 fw-bold">
                <span class="text-gray-700">{{FullFilePath}}</span>
                
            </div>`;
        this.newFilePropsHTML =
            `<div class="fs-6 fw-bold">
                <span class="text-success fw-bold">{{CreatedOn}}</span>
            </div>
            <div class="fs-6 fw-bold">
            </div>
            <div class="fs-6 fw-bold">
            </div>
            <div class="fs-6 fw-bold">
                <span class="text-success fw-bold">{{FullFilePath}}</span>
            </div>`

        this.imageGallaryHTML =
            `<div class="container-fluid mt-5">
                <div class="row">
                    <div class="col-md-3 col-lg-2">
                        <div class="card mb-4">
                            <div class="card-body">
                                <div id="dragDropZone" class="drag-drop-zone">
                                    <input type="file" id="fileUpload" accept=".jpg, .jpeg, .png, .gif, .bmp, .svg" style="display:none;">
                                    <p>Drag and drop files here or click to select</p>
                                    <div class="file-preview" id="filePreview"></div>
                                </div>
                                <button id="btnUpload" class="btn btn-primary mt-3" onclick="return _customPlugins.uploadImage('{{filepath}}');">Upload</button>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-9 col-lg-10">
                        <div class="d-flex justify-content-between align-items-center mb-3">
                            <h3>Image Gallery</h3>
                            <span id="imageCount" class="badge bg-secondary"></span>
                        </div>
                        <div id="imageGallery" class="image-gallery">
                            <!-- Images will be dynamically loaded here -->
                        </div>
                    </div>
                </div>
            </div>`;

        this.addFiles = {};
        this.addFiles.main =
            `
            <div class="row" data-type="DropDown" >
                <div class="col-md-3 fldCaption"><p class="form-group required">Main page template</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld" id="structure-selection" >
                        <option value="globalmainpage">Global - Main Page</option>
                        <option value="appmainpage">Application specific - Main Page</option>
                    </select>
                </div>
                    
            </div>
                
            <div class="row" data-type="DropDown">
                <div class="col-md-3 fldCaption"><p class="form-group required">File Type</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld" id="filetype-selection" >
                        <option value="">Select a state</option>
                        <option value=".html">HTML</option>
                        <option value=".js">Javascript</option>
                        <option value=".css">CSS</option>
                    </select>
                </div>
                    
            </div>`;
        this.addFiles.tstruct =
            `
            <div class="row" data-type="DropDown" >
                <div class="col-md-3 fldCaption"><p class="form-group required">Form selection</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="structure-selection" >
                        <option value="custom">All Forms</option>                        
                    </select>
                </div>
                    
            </div>
                
            <div class="row" data-type="DropDown">
                <div class="col-md-3 fldCaption"><p class="form-group required">File Type</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="filetype-selection" >                        
                        <option value=".js">Javascript</option>
                        <option value=".css">CSS</option>
                    </select>
                </div>
                    
            </div>`;
        this.addFiles.report =
            `
            <div class="row" data-type="DropDown" >
                <div class="col-md-3 fldCaption"><p class="form-group required">Report selection</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="structure-selection" >      
                        <option value="custom">All Reports</option>                        
                    </select>
                </div>
                    
            </div>
                
            <div class="row" data-type="DropDown">
                <div class="col-md-3 fldCaption"><p class="form-group required">File Type</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="filetype-selection" >
                        <option value=".js">Javascript</option>
                        <option value=".css">CSS</option>
                    </select>
                </div>
                    
            </div>`
        this.addFiles.datapage =
            `
            <div class="row" data-type="DropDown" >
                <div class="col-md-3 fldCaption"><p class="form-group required">Form selection</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="structure-selection" >
                        <option value="custom">All Forms</option>                        
                    </select>
                </div>
                    
            </div>
                
            <div class="row" data-type="DropDown">
                <div class="col-md-3 fldCaption"><p class="form-group required">File Type</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="filetype-selection" >                        
                        <option value=".js">Javascript</option>
                        <option value=".css">CSS</option>
                    </select>
                </div>
                    
            </div>`;
        this.addFiles.datalist =
            `
            <div class="row" data-type="DropDown" >
                <div class="col-md-3 fldCaption"><p class="form-group required">Report selection</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="structure-selection" >      
                        <option value="custom">All Forms</option>                        
                    </select>
                </div>
                    
            </div>
                
            <div class="row" data-type="DropDown">
                <div class="col-md-3 fldCaption"><p class="form-group required">File Type</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="filetype-selection" >
                        <option value=".js">Javascript</option>
                        <option value=".css">CSS</option>
                    </select>
                </div>
                    
            </div>`;
        
        this.addFiles.image =
            `
            <div class="row" data-type="DropDown" >
                <div class="col-md-3 fldCaption"><p class="form-group required">Upload Image</p></div>
                <div class="col-md-6 fldCaption">
                     <input class="form-control filter-fld required" type="file" id="imageUpload" accept=".jpg, .jpeg, .png, .gif, .bmp, .svg"  required ></input>
                </div>
                    
            </div>`;

        this.addFiles.htmlplugin =
            `
            <div class="row" data-type="DropDown" >
                <div class="col-md-3 fldCaption"><p class="form-group required">HTML Plugin Name</p></div>
                <div class="col-md-6 fldCaption">
                    <input class="form-control filter-fld required" type="text" id="pluginName" required ></input>
                </div>
                    
            </div>
                
            <div class="row" data-type="DropDown">
                <div class="col-md-3 fldCaption"><p class="form-group required">Plugin type</p></div>
                <div class="col-md-6 fldCaption">
                    <select class="form-control filter-fld " id="pluginType" >
                        <option value="cards">Cards</option>
                    </select>
                </div>                   
            </div>
            <div class="row">
                <div class="col-md-3 fldCaption"><p class="form-group required">Plugin HTML Text</p></div>
                <div class="col-md-12 fldCaption" style="min-height:40vh;">
                    <textarea id="pluginText" style="width:100%;min-height:40vh;"></textarea>
                </div>                   
            </div>`;
        this.navList = {};
        this.filesList = [];
        this.foldersList = [];
        this.addFileType = "";
        this.structures = JSON.parse($("#hdnStructures").val().replaceAll(`"CAPTION":`, `"caption":`).replaceAll(`"NAME":`, `"name":`).replaceAll(`"TYPE":`, `"type":`)).result.row;
        this.uploadedImage = "";
        this.tempFiles = [];
        this.fileManagerFiles = {};
        this.htmlPlugins = [];
    }

    init() {
        var _this = this;
        _this.createDefaultMenus();
        _this.fetchFilesList();
        _this.parseFilesList();
        _this.getHTMLPlugins();
        _this.constructFilesListHTML();
    }

    createDefaultMenus() {
        var _this = this;
        _this.navList = {
            "Custom Main - All Apps": {
                "files": {
                    "HTML": {
                        "FolderName": "CustomPages",
                        "FileName": "AgileBizMainPage.html",
                        "FullFilePath": "CustomPages\\AgileBizMainPage.html",
                        "FileType": ".html"
                    },
                    "JS": {
                        "FolderName": "CustomPages",
                        "FileName": "mainpage-custom.js",
                        "FullFilePath": "CustomPages\\js\\mainpage-custom.js",
                        "FileType": ".js"
                    },
                    "CSS": {
                        "FolderName": "CustomPages",
                        "FileName": "mainpage-custom.css",
                        "FullFilePath": "CustomPages\\css\\mainpage-custom.css",
                        "FileType": ".css"
                    }
                },
                "type": "main",
                "groupname" : "Main Page"
            },
            "Custom Main - $APP_NAME$": {
                "files": {
                    "HTML": {
                        "FolderName": "CustomPages",
                        "FileName": "$APP_NAME$-MainPage.html",
                        "FullFilePath": "CustomPages\\$APP_NAME$-MainPage.html",
                        "FileType": ".html"
                    },
                    "JS": {
                        "FolderName": "CustomPages",
                        "FileName": "$APP_NAME$-mainpage-custom.js",
                        "FullFilePath": "CustomPages\\js\\$APP_NAME$-mainpage-custom.js",
                        "FileType": ".js"
                    },
                    "CSS": {
                        "FolderName": "CustomPages",
                        "FileName": "$APP_NAME$-mainpage-custom.css",
                        "FullFilePath": "CustomPages\\css\\$APP_NAME$-mainpage-custom.css",
                        "FileType": ".css"
                    }
                },
                "type": "main",
                "groupname": "Main Page"
            },           
            "Custom Iview Reports - All": {
                "files": {
                    "JS": {
                        "FolderName": "$APP_NAME$\\report\\js",
                        "FileName": "custom.js",
                        "FullFilePath": "$APP_NAME$\\report\\js\\custom.js",
                        "FileType": ".js"
                    },
                    "CSS": {
                        "FolderName": "$APP_NAME$\\report\\css",
                        "FileName": "custom.css",
                        "FullFilePath": "$APP_NAME$\\report\\css\\custom.css",
                        "FileType": ".css"
                    }
                },
                "type": "report",
                "groupname": "Iview Reports"
            },
            "Custom Tstruct Forms - All": {
                "files": {
                    "JS": {
                        "FolderName": "$APP_NAME$\\tstruct\\js",
                        "FileName": "custom.js",
                        "FullFilePath": "$APP_NAME$\\tstruct\\js\\custom.js",
                        "FileType": ".js"
                    },
                    "CSS": {
                        "FolderName": "$APP_NAME$\\tstruct\\css",
                        "FileName": "custom.css",
                        "FullFilePath": "$APP_NAME$\\tstruct\\css\\custom.css",
                        "FileType": ".css"
                    }
                },
                "type": "tstruct",
                "groupname": "Tstruct Forms"
            },       
            "Custom Data List - All": {
                "files": {
                    "JS": {
                        "FolderName": "$APP_NAME$\\datalist\\js",
                        "FileName": "custom.js",
                        "FullFilePath": "$APP_NAME$\\datalist\\js\\custom.js",
                        "FileType": ".js"
                    },
                    "CSS": {
                        "FolderName": "$APP_NAME$\\datalist\\css",
                        "FileName": "custom.css",
                        "FullFilePath": "$APP_NAME$\\datalist\\css\\custom.css",
                        "FileType": ".css"
                    }
                },
                "type": "report",
                "groupname": "Data Lists"
            },
            "Custom Data Page - All": {
                "files": {
                    "JS": {
                        "FolderName": "$APP_NAME$\\datapage\\js",
                        "FileName": "custom.js",
                        "FullFilePath": "$APP_NAME$\\datapage\\js\\custom.js",
                        "FileType": ".js"
                    },
                    "CSS": {
                        "FolderName": "$APP_NAME$\\datapage\\css",
                        "FileName": "custom.css",
                        "FullFilePath": "$APP_NAME$\\datapage\\css\\custom.css",
                        "FileType": ".css"
                    }
                },
                "type": "tstruct",
                "groupname": "Data Pages"
            },
            "Images - All Apps": {
                "files": {
                    "Images": {
                        "FolderName": "CustomPages",
                        "FullFilePath": "CustomPages\\Images\\"
                    }
                },
                "type": "images",
                "groupname": "Images"
            },
            "Images - $APP_NAME$": {
                "files": {
                    "Images": {
                        "FolderName": "$APP_NAME$",
                        "FullFilePath": "$APP_NAME$\\Images\\"
                    }
                },
                "type": "images",
                "groupname": "Images"
            }
        }

        if (document.getElementById("hdnApplicationTemplate").value != "") {
            _this.navList["Custom Main - Application Template"] = {
                "files": {
                    "HTML": {
                        "FolderName": "CustomPages",
                        "FileName": document.getElementById("hdnApplicationTemplate").value,
                        "FullFilePath": "CustomPages\\" + document.getElementById("hdnApplicationTemplate").value,
                        "FileType": ".html"
                    }
                },
                "type": "main",
                "groupname": "Main Page"
            }

        };

        var tempObjStr = JSON.stringify(_this.navList);
        _this.navList = JSON.parse(tempObjStr.replaceAll("$APP_NAME$", _this.appName))
    }

    fetchFilesList() {
        var files = [];
        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/GetFilesAndFolders",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var list = response.d;
                files = JSON.parse(list);

            },
            error: function (xhr, status, error) {
                console.error("Error fetching file details: " + error);
            }
        });

        this.filesList = files.files;
        this.foldersList = files.folders;

        let fileList = this.filesList;
        let folderList = this.foldersList;

        function updateUI() {
            updateFolderDropdowns();
            displayFilesAccordion();
        }

        // Update folder dropdowns (same as before)
        function updateFolderDropdowns() {
            const folders = [...new Set(folderList.map(file => file.FolderName))];
            const $parentFolder = $('#parentFolder');
            const $uploadFolder = $('#uploadFolder');

            $parentFolder.empty();
            $uploadFolder.empty();

            folders.forEach(folder => {
                $parentFolder.append(`<option value="${folder}">${folder}</option>`);
                $uploadFolder.append(`<option value="${folder}">${folder}</option>`);
            });
        }

        // Display files in accordion structure
        function displayFilesAccordion() {
            const $accordion = $('#folderAccordion');
            $accordion.empty();

            // Group files by folder
            const folderGroups = {};
            fileList.forEach(file => {
                if (!folderGroups[file.FolderName]) {
                    folderGroups[file.FolderName] = [];
                }
                folderGroups[file.FolderName].push(file);
            });

            // Create accordion items
            Object.keys(folderGroups).sort().forEach((folderName, index) => {
                const files = folderGroups[folderName];
                const fileTypes = getFileTypeCounts(files);
                const accordionItem = createAccordionItem(folderName, files, fileTypes, index);
                $accordion.append(accordionItem);
            });
        }

        // Create accordion item HTML
        function createAccordionItem(folderName, files, fileTypes, index) {
            const isFirst = index === 0;
            const fileTypesBadges = Object.entries(fileTypes)
                .map(([type, count]) => `<span class="file-type-badge">${type} (${count})</span>`)
                .join('');

            return `
                    <div class="accordion-item">
                        <h2 class="accordion-header" id="heading${index}">
                            <button class="accordion-button ${isFirst ? '' : 'collapsed'}" 
                                    type="button" 
                                    data-bs-toggle="collapse" 
                                    data-bs-target="#collapse${index}">
                                <i class="fas fa-folder folder-icon text-warning"></i>
                                ${folderName}
                                <div class="folder-stats">
                                    ${files.length} files ${fileTypesBadges}
                                </div>
                            </button>
                        </h2>
                        <div id="collapse${index}" 
                             class="accordion-collapse collapse ${isFirst ? 'show' : ''}" 
                             data-bs-parent="#folderAccordion">
                            <div class="accordion-body">
                                ${files.map(file => createFileItemHtml(file)).join('')}
                            </div>
                        </div>
                    </div>
                `;
        }

        // Get file type counts for a folder
        function getFileTypeCounts(files) {
            const counts = {};
            files.forEach(file => {
                const type = file.FileType.toLowerCase();
                counts[type] = (counts[type] || 0) + 1;
            });
            return counts;
        }

        // Create HTML for individual file items
        function createFileItemHtml(file) {
            const fileSize = formatFileSize(file.FileSize);
            _customPlugins.fileManagerFiles[file.FullFilePath] = file;
            return `
                    <div class="file-item" data-filepath="${file.FullFilePath}">
                        <div class="d-flex justify-content-between align-items-center p-3" >
                            <div class="col-4 d-flex align-items-center">
                                <a href="javascript:void(0);" onclick="_customPlugins.loadFileContent(this, '${file.FullFilePath.replaceAll("\\", "\\\\")}', 'FileManager')" class="filename fw-bold">${file.FileName}</a>
                            </div>
    
                            <div class="col-4 text-center">
                                <span><i class="fas fa-hdd me-1"></i>${fileSize}</span>
                            </div>
    
                            <div class="col-4 text-end">
                                <span><i class="fas fa-clock me-1"></i>${_customPlugins.formatDate(file.ModifiedOn)}</span>
                            </div>
                        </div>
                    </div>

                    
                `;
        }

        function formatFileSize(size) {
            if (size === 0) return '0 KB';
            if (size < 1024) return size.toFixed(2) + ' KB';
            return (size / 1024).toFixed(2) + ' MB';
        }        


        updateUI();
    }

    parseFilesList() {
        var _this = this;
        var appName = _this.appName.toLowerCase();
        _this.filesList.forEach((file, idx) => {
            var fileType = file.FileType.substr(1).toUpperCase();
            var fileName = file.FileName.toLowerCase();
            var folderName = file.FolderName.toLowerCase();

            var menuName = "";
            var pageType = "";
            var groupName = "";
            if (folderName.indexOf("custompages") == 0) {

                if (fileName == "agilebizmainpage.html" || fileName == "mainpage-custom.js" || fileName == "mainpage-custom.css") {
                    menuName = `Custom Main - All Apps`;
                }
                else if (fileName == `${appName}-mainpage.html` || fileName == `${appName}-mainpage-custom.js` || fileName == `${appName}-mainpage-custom.css`) {
                    menuName = `Custom Main - ${appName}`;                    
                } else if (fileName == document.getElementById("hdnApplicationTemplate").value) {
                    menuName = `Custom Main - Application Template`;
                }
                pageType = "main";
                groupName = "Main Page";
            }
            else if (folderName.indexOf(appName) == 0) {
                if (folderName.indexOf("tstruct") > -1) {
                    if (fileName == "custom.js" || fileName == "custom.css") {
                        menuName = `Custom Tstruct Forms - All`;
                    }
                    else {
                        var transId = fileName.split('.')[0];
                        var name = _this.structures.find(x => x.type == "tstruct" && x.name == transId)?.caption || "";
                        menuName = `Custom Tstruct Forms - ${name} (${transId})`;                        
                    }
                    pageType = "tstruct";
                    groupName = "Tstruct Forms";
                }
                else if (folderName.indexOf("report") > -1) {
                    if (fileName == "custom.js" || fileName == "custom.css") {
                        menuName = `Custom Iview Reports - All`;
                    }
                    else {
                        var reportName = fileName.split('.')[0];
                        var name = _this.structures.find(x => x.type == "report" && x.name == reportName)?.caption || "";
                        menuName = `Custom Iview Reports - ${name} (${reportName})`;
                    }
                    pageType = "report";
                    groupName = "Iview Reports";
                }
                if (folderName.indexOf("datapage") > -1) {
                    if (fileName == "custom.js" || fileName == "custom.css") {
                        menuName = `Custom Data Page - All`;
                    }
                    else {
                        var transId = fileName.split('.')[0];
                        var name = _this.structures.find(x => x.type == "tstruct" && x.name == transId)?.caption || "";
                        menuName = `Custom Data Page - ${name} (${transId})`;
                    }
                    pageType = "datapage";
                    groupName = "Data Pages";
                }
                else if (folderName.indexOf("datalist") > -1) {
                    if (fileName == "custom.js" || fileName == "custom.css") {
                        menuName = `Custom Data List - All`;
                    }
                    else {
                        var reportName = fileName.split('.')[0];
                        var name = _this.structures.find(x => x.type == "report" && x.name == reportName)?.caption || "";
                        menuName = `Custom Data List - ${name} (${reportName})`;
                    }
                    pageType = "datapage";
                    groupName = "Data Lists";
                }
                else if (folderName.indexOf("htmlpages") > -1) {
                    menuName = fileName;
                    pageType = "html";
                    groupName = `Custom HTML Pages`;
                }
            }

            
            if (menuName) {
                if (_entityCommon.inValid(_this.navList[menuName])) {
                    _this.navList[menuName] = {};
                    _this.navList[menuName]["files"] = {};
                }
                _this.navList[menuName]["files"][fileType] = file;
                _this.navList[menuName]["type"] = pageType;
                _this.navList[menuName]["groupname"] = groupName;
            }
        });
    }

    loadFileTypes(elem, nav) {
        var _this = this;
        $(".Data-Group_Items.selected").removeClass("selected")
        $(elem).addClass("selected");
        _this.constructFileTypesHTML(nav);
    }

    constructFileTypesHTML(menuName) {
        var _this = this;
        var filesList = _this.navList[menuName].files;
        var fileTypeHtml = "";

        if (_this.navList[menuName].type == "htmlplugin") {
            var tempObj = {
                filetype: "HTML",
                filepath: "NA",
                menuname: menuName,
                menutype: "htmlplugin"
            }

            _this.HTMLPluginName = menuName;

            fileTypeHtml += Handlebars.compile(this.fileTypeHTML)(tempObj);
        }
        else {
            Object.keys(filesList).forEach((fileType, idx) => {
                var tempObj = {
                    filetype: fileType,
                    filepath: _this.navList[menuName].files[fileType].FullFilePath.replaceAll("\\", "\\\\"),
                    menuname: menuName,
                    menutype: "file"
                }

                fileTypeHtml += Handlebars.compile(this.fileTypeHTML)(tempObj);

            });
        }

        $("#dv_EntityContainer").html(fileTypeHtml);
        _this.openCodeEditor();

        if ($("#dv_EntityContainer .nav-item").length) {
            $("#dv_EntityContainer .nav-item")[0].click();
        }
    }

    constructFilesListHTML() {
        var _this = this;
        var htmlString = "";
        //Object.keys(_this.navList).forEach((nav, idx) => {
        //    var tempObj = {
        //        caption: nav,
        //        icon: _this.getIcon(_this.navList[nav].type)
        //    }

        //    navHtml += Handlebars.compile(_this.navMenuHTML)(tempObj);
        //})

        let groups = {};
        //let htmlString = '';

        // Group items
        Object.entries(_this.navList).forEach(([key, value]) => {
            if (!value.groupname)
                value.groupname = "Others";

            if (!groups[value.groupname]) {
                groups[value.groupname] = [];
            }
            groups[value.groupname].push({ key, ...value });
        });

        // Build HTML string
        Object.entries(groups).forEach(([groupName, items]) => {
            htmlString += `
                    <div class="accordion accordion-icon-toggle">
                        <div class="accordion-header">
                            ${groupName}
                            <span class="arrow">▼</span>
                        </div>
                        <div class="accordion-content">
                `;

            items.forEach(item => {
                htmlString += `
                        <div class="Data-Group_Items group-all" data-nav="${item.key}" 
                            onclick="_customPlugins.loadFileTypes(this, '${item.key}')">
                            <a href="#" class="group-item">
                                <div class="d-flex">
                                    <div class="symbol symbol-40px symbol-circle me-5" style="margin-left: 5px !important;">
                                        <div class="symbol-label bgs1">
                                            <span class="material-icons material-icons-style material-icons-2">${_customPlugins.getIcon(item.type)}</span>
                                        </div>
                                    </div>
                                    <div class="d-flex flex-column y-axis-caption">
                                        <span class="Data-Group-name">${item.key}</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                    `;
            });

            htmlString += `
                        </div>
                    </div>
                `;
        });

        // Insert the built HTML string
        const container = document.getElementById('Data-Group-container');
        container.insertAdjacentHTML("beforeend", htmlString);

        // Add click handlers
        document.querySelectorAll('.accordion-header').forEach(header => {
            header.addEventListener('click', function () {
                if ($(this).parent().hasClass("filemanager-accordion"))
                    return;
                this.classList.toggle('active');
                const content = this.nextElementSibling;
                content.classList.toggle('active');
            });
        });

        if (htmlString == "")
            htmlString = "<p>No custom files available.</p>"

        //$("#Data-Group-container").html(navHtml);

    }

    getIcon(type) {
        let icon = "assignment";
        switch (type.toLowerCase()) {
            case "tstruct":
            case "datapage":
                icon = "assignment";
                break;
            case "report":
            case "datalist":
                icon = "view_list";
                break;
            case "main":
                icon = "public";
                break;
            case "json":
                icon = "data_object";
                break;
            case "images":
                icon = "photo_library";
                break;
            default:
                break;
        }

        return icon;
    }

    constructFilePropertiesHeader(props) {
        var _this = this;
        if (typeof props.CreatedOn != "undefined" && props.CreatedOn != "")
            props.CreatedOn = _customPlugins.formatDate(props.CreatedOn);
        else
            props.CreatedOn = 'New File';

        if (typeof props.ModifiedOn != "undefined")
            props.ModifiedOn = _customPlugins.formatDate(props.ModifiedOn);

        var propsHtml = "";

        if (props.CreatedOn == 'New File')
            propsHtml = Handlebars.compile(_this.newFilePropsHTML)(props);
        else
            propsHtml = Handlebars.compile(_this.filePropsHTML)(props);

        $("#analytics-chart-title").html(propsHtml);

    }

    loadFileContent(elem, filePath, calledFrom) {

        var skipFiles = [".jpeg", ".jpg", ".png", ".svg"];
        var fileExtension = filePath.toLowerCase().substring(filePath.lastIndexOf('.'));
        var fileName = filePath.toLowerCase().substring(filePath.lastIndexOf('\\'));
        if (skipFiles.indexOf(fileExtension) > -1) {
            showAlertDialog("error", "Edit image type files from 'Images' menu.");
            return;
        }

        $("#dv_EntityContainer .active").removeClass("active")
        $(elem).find(".nav-link").addClass("active");

        var _this = this;
        var menuName = elem.dataset["menuname"];
        var fileType = elem.dataset["filetype"];
        var menuType = elem.dataset["menutype"];
        var props = "";
        if (calledFrom != "FileManager")
            props = _this.navList[menuName]["files"][fileType];
        else if (menuType == "htmlplugin") {
            props = {};
        }
        else {            
            props = _customPlugins.fileManagerFiles[filePath];
        }

        if (fileType == "Images") {
            _this.loadImages(filePath);
            $('[data-toggle="tooltip"]').tooltip();
        }
        else if (menuType == "htmlplugin") {    
            $(".Summary_Charts").html(`<textarea id="codeEditor"></textarea>`);

            var htmlContent = _this.navList[menuName]["files"]["html"];
            _this.initCodeEditor(htmlContent, "HTML");
            $("#Entity_summary_Right .card-header").addClass("d-none");
            $("#Entity_summary_Right .card-footer-bottom.file-footer").addClass("d-none");
            $("#Entity_summary_Right .card-footer-bottom.plugin-footer").removeClass("d-none");

        }
        else {
            _this.constructFilePropertiesHeader(props);

            _this.fileName = filePath;
            $(".Summary_Charts").html(`<textarea id="codeEditor"></textarea>`);
            $.ajax({
                type: "POST",
                url: "../aspx/PluginCustomCode.aspx/GetFileContent",
                async: false,
                data: JSON.stringify({ filePath: filePath }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (calledFrom == "FileManager")
                        _this.openCodeEditor();

                    const fileContent = response.d;
                    _this.initCodeEditor(fileContent);

                },
                error: function (xhr, status, error) {
                    console.error("Error fetching file content: " + error);
                }
            });

            $("#Entity_summary_Right .card-header").removeClass("d-none");
            $("#Entity_summary_Right .card-footer-bottom.file-footer").removeClass("d-none");
            $("#Entity_summary_Right .card-footer-bottom.plugin-footer").addClass("d-none");
        }        
    }

    initCodeEditor(fileContent, extension) {
        var _this = this;
        _codeMirrorEdit = CodeMirror.fromTextArea(document.getElementById('codeEditor'), {
            mode: _this.getFileExt(extension), // or "css", "htmlmixed" based on file type
            smartIndent: true,
            lineNumbers: true,
            matchBrackets: true,
            autoCloseBrackets: true,
            autoRefresh: true,
            foldGutter: true,
            gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
            extraKeys: { "Alt-F": "findPersistent" },
            height: "100%",  // Ensure editor height takes full space
            width: "100%"
        });

        if (fileContent != "File not found.") {
            _codeMirrorEdit.setValue(fileContent);
        }
    }

    getFileExt(ext) {
        var _this = this;
        var result = "";
        var fileExt = ext || _this.fileName.split('.').pop();
        switch (fileExt.toLowerCase()) {
            case "js":
                result = "text/javascript";
                break;
            case "css":
                result = "text/css";
                break;
            case "html":
                result = "htmlmixed";
                break;
            case "json":
                result = "text/javascript";
                break;
            default:
                result = "text/javascript";
                break;
        }

        return result;
    }

    updateFile() {
        var _this = this;
        const modifiedContent = _codeMirrorEdit.getValue();
        if (modifiedContent == "") {
            showAlertDialog("error", "File content is empty. Can't save.");
            return false;
        }

        const filePath = _this.fileName;

        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/SaveFileContent",
            data: JSON.stringify({ filePath: filePath, content: modifiedContent }),
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (response) {
                if (response.d.indexOf("File saved successfully.") > -1) {
                    showAlertDialog("success", "File saved successfully!");                    
                    $("#dv_EntityContainer .active").click();

                    try {
                        var props = JSON.parse(response.d.split('~~')[1]);
                        _customPlugins.constructFilePropertiesHeader(props);
                    }
                    catch { }
                }
                else
                    showAlertDialog("error", "Error: " + response.d);
            },
            error: function (xhr, status, error) {
                showAlertDialog("error", error);
            }
        });

        return false;
    }

    deleteFile() {

        if (!confirm("Do you want to delete this file?"))
            return false;

        var _this = this;

        const filePath = _this.fileName;

        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/DeleteFile",
            data: JSON.stringify({ filePath: filePath }),
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (response) {
                if (response.d == "File is deleted.") {
                    showAlertDialog("success", "File is deleted!");
                    window.location.reload();
                }
                else
                    showAlertDialog("error", "Error: " + response.d);
            },
            error: function (xhr, status, error) {
                showAlertDialog("error", error);
            }
        });

        return false;
    }

    openAddFile() {
        $("#filterModal").modal("show");
    }


    loadAddFilesFields(type) {
        $("#filterModal").modal("show");
        var _this = this;

        var html = _this.addFiles[type];

        _this.addFileType = type;

        if (_entityCommon.inValid(html))
            html = "Error in template.";

        $("#dvAddFiles").html(html);

        _this.initializeDropdowns(type);

  
        if (type == "htmlplugin") {
            _codeMirrorNewPlugin = CodeMirror.fromTextArea(document.getElementById('pluginText'), {
                mode: "htmlmixed",
                smartIndent: true,
                lineNumbers: true,
                matchBrackets: true,
                autoCloseBrackets: true,
                autoRefresh: true,
                foldGutter: true,
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                extraKeys: { "Alt-F": "findPersistent" },
                height: "40vh",  // Ensure editor height takes full space
                width: "100%"
            });
        }

    }

    initializeDropdowns(type) {
        var _this = this;
        if (type == "tstruct" || type == "datapage" || type == "datalist") {
            var tstructSelection = document.querySelector("#structure-selection");
            _this.structures.filter(x => x.type == "tstruct").forEach(item => {
                tstructSelection.insertAdjacentHTML("beforeend", `<option value="${item.name}">${item.caption} (${item.name})</option>`);
            });
        }
        else if (type == "report") {
            var tstructSelection = document.querySelector("#structure-selection");
            _this.structures.filter(x => x.type == "report").forEach(item => {
                tstructSelection.insertAdjacentHTML("beforeend", `<option value="${item.name}">${item.caption} (${item.name})</option>`);
            });
        }


        document.querySelectorAll(`#dvAddFiles select`).forEach(fld => {
            $(fld).select2({
                placeholder: "Select..."
            })

        })
    }

    closeModalDialog() {
        $("#filterModal").modal("hide");
    }

    addFile() {
        var _this = this;
        var structure = "";
        var fileType = "";
        var fileContent = "";
        var filePath = "";
        
        if (_this.addFileType == "tstruct" || _this.addFileType == "report" || _this.addFileType == "datapage" || _this.addFileType == "datalist") {
            fileType = $("#filetype-selection").val();

            filePath = `${_this.appName.toLowerCase()}\\${_this.addFileType}\\${fileType.replace(".", "")}\\`;
            structure = $("#structure-selection").val();


            $.ajax({
                type: "POST",
                url: "../aspx/PluginCustomCode.aspx/CreateFile",
                data: JSON.stringify({ pageType: _this.addFileType, fileName: structure, fileType: fileType, content: fileContent, filePath: filePath }),
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: function (response) {
                    if (response.d == "File created successfully.") {
                        showAlertDialog("success", "File created successfully!");
                        window.location.reload();
                    }
                    else
                        showAlertDialog("error", "Error: " + response.d);
                },
                error: function (xhr, status, error) {
                    showAlertDialog("error", error);
                }
            });
        }
        else if (_this.addFileType == "htmlplugin" ){
            var pluginName = $("#pluginName").val();
            var pluginType = $("#pluginType").val();
            var pluginText = _codeMirrorNewPlugin.getValue();;

            _this.callHTMLPluginWS(pluginName, pluginType, pluginText, "INSERT", "Plugin added successfully.")            
        }
    }

    callHTMLPluginWS(pluginName, pluginType, pluginText, action, successMsg) {
        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/AddorEditHTMLPlugin",
            data: JSON.stringify({ name: pluginName, context: pluginType, htmlText: pluginText, action: action }),
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (response) {

                if (JSON.parse(response.d).result.message == 'success') {
                    showAlertDialog("success", successMsg);
                    window.location.reload();
                }
                else
                    showAlertDialog("error", "Error: " + JSON.parse(response.d).result.message);
            },
            error: function (xhr, status, error) {
                showAlertDialog("error", error);
            }
        });
    }

    readFileAsArrayBuffer(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => resolve(reader.result);
            reader.onerror = reject;
            reader.readAsArrayBuffer(file);
        });
    }

    bindImageDragDropEvent() {
        var _this = this;

        const dropZone = document.getElementById('dragDropZone');
        const fileInput = document.getElementById('fileUpload');
        const filePreview = document.getElementById('filePreview');

        // Prevent default drag behaviors
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            dropZone.addEventListener(eventName, preventDefaults, false);
            document.body.addEventListener(eventName, preventDefaults, false);
        });

        // Highlight drop zone when item is dragged over
        ['dragenter', 'dragover'].forEach(eventName => {
            dropZone.addEventListener(eventName, highlight, false);
        });

        ['dragleave', 'drop'].forEach(eventName => {
            dropZone.addEventListener(eventName, unhighlight, false);
        });

        // Handle dropped files
        dropZone.addEventListener('drop', handleDrop, false);

        // Handle click to select files
        dropZone.addEventListener('click', () => fileInput.click());

        fileInput.addEventListener('change', async (event) => {
            await handleFiles(event);
        }, false);

        function preventDefaults(e) {
            e.preventDefault();
            e.stopPropagation();
        }

        function highlight() {
            dropZone.classList.add('dragover');
        }

        function unhighlight() {
            dropZone.classList.remove('dragover');
        }

        function handleDrop(e) {
            const dt = e.dataTransfer;
            const files = dt.files;
            handleFiles(files);
        }

        async function handleFiles(files) {
            // If called from input change event, get files from event
            files = files.target ? files.target.files : files;

            if (files.length > 0) {
                const file = files[0];
                const allowedTypes = [
                    'image/jpeg',
                    'image/png',
                    'image/gif',
                    'image/bmp',
                    'image/svg+xml'
                ];

                if (allowedTypes.includes(file.type)) {
                    filePreview.textContent = `Selected: ${file.name}`;
                    fileInput.files = files;
                } else {
                    filePreview.textContent = 'Invalid file type. Please select an image.';
                    fileInput.value = ''; // Clear the input
                }


                try {
                    const arrayBuffer = await _this.readFileAsArrayBuffer(file);
                    _customPlugins.uploadedImage = Array.from(new Uint8Array(arrayBuffer));
                } catch (error) {
                    console.error('Error:', error);
                }

            }
        }

        
    }

    loadImages(filePath) {
        let _this = this;
        let galleryHtml = Handlebars.compile(_this.imageGallaryHTML)({ filepath: filePath.replaceAll("\\","\\\\") });
        $(".Summary_Charts").html(galleryHtml);

        _this.bindImageDragDropEvent();

        

        $("#Entity_summary_Right .card-header").addClass("d-none");
        $("#Entity_summary_Right .card-footer-bottom").addClass("d-none");


        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/GetImages",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ filePath: filePath }),
            dataType: "json",
            success: function (response) {
                var gallery = $('#imageGallery');
                gallery.empty();
                try {
                    var images = JSON.parse(response.d);
                    if (images.length > 0) {
                        images.forEach(function (imageInfo) {
                            var imageCard = $(`
                                    <div class="image-card">
                                        <img src="../${imageInfo.FullFilePath.replaceAll("\\", "/")}" alt="${imageInfo.FileName}" />
                                        <div class="delete-overlay" data-filepath="${imageInfo.FullFilePath}">Delete</div>
                                        <div class="filename" title="${imageInfo.FileName}">
                                            <a href="#" onclick="alert('File path: ${imageInfo.FullFilePath.replaceAll("\\", "/")}')">${imageInfo.FileName}</a>
                                        </div>
                                    </div>
                                `);

                            // Add delete event handler
                            imageCard.find('.delete-overlay').click(function () {
                                var filePath = $(this).data('filepath');
                                _this.deleteImage(filePath);
                            });

                            gallery.append(imageCard);
                        });
                    } else {
                        gallery.html('<div class="alert">No images found</div>');
                    }
                }
                catch {
                    showAlertDialog("error", "Error loading images:" + response.d);
                }
                
            },
            error: function (xhr, status, error) {
                showAlertDialog("error","Error loading images:" + error);
            }
        });
    }


    uploadImage(filePath) {  
        let _this = this;
        var fileInput = $('#fileUpload')[0];
        var file = fileInput.files[0];

        if (!file) {
            showAlertDialog("error", "Please select a file");
            return false;
        }

        // Create FormData object
        var formData = new FormData();
        formData.append('file', file);
        var fileName = file.name;

        var fileData = _this.uploadedImage;

        // AJAX call to upload image
        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/UploadImage",
            data: JSON.stringify({ fileName: fileName, filePath: filePath, fileData: fileData }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {                
                if (response.d == "Image uploaded successfully.") {
                    showAlertDialog("success", "Image uploaded successfully.");
                    fileInput.value = '';
                    $("#dv_EntityContainer .active").click();
                } else {
                    showAlertDialog("error", "Error: " + response.d );
                }
            },
            error: function (xhr, status, error) {
                showAlertDialog("error", "Error: " + error);
            }
        });

        return false;
    }

    deleteImage(filePath) {
        if (!confirm("Do you want to delete this file?"))
            return false;

        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/DeleteImage",
            data: JSON.stringify({ filePath: filePath }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d == "Image deleted successfully.") {
                    showAlertDialog("success", "Image is deleted.");
                    $("#dv_EntityContainer .active").click();
                } else {
                    showAlertDialog("error", "Error: " + response.d);
                }
            },
            error: function (xhr, status, error) {
                showAlertDialog("error", "Error: " + error);
            }
        });
    }

    openFileManager() {
        $("#Entity_summary_Right_Empty").addClass("d-none")
        $("#Entity_summary_Right").addClass("d-none");
        $("#dv_EntityContainer").html("")
        $("#FileManager-container").removeClass("d-none");
    }

    openCodeEditor() {
        $("#Entity_summary_Right_Empty").addClass("d-none")
        $("#FileManager-container").addClass("d-none");
        $("#Entity_summary_Right").removeClass("d-none");        
    }

    createFolder() {
        let _this = this;
        const parentFolder = $('#parentFolder').val();
        const newFolderName = $('#newFolderName').val();

        if (!newFolderName) {
            showAlertDialog("error", 'Please enter a folder name');
            return;
        }

        $.ajax({
            url: "../aspx/PluginCustomCode.aspx/CreateFolder",
            type: 'POST',
            data: JSON.stringify({
                parentFolder: parentFolder,
                folderName: newFolderName
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                if (response.d == "Folder created successfully.") {
                    showAlertDialog("success", "Folder created successfully.");
                    window.location.reload();
                } else {
                    showAlertDialog("error", response.d);
                }
            },
            error: function (error) {
                showAlertDialog("error", 'Error creating folder:', error);
            }
        });
    }

    uploadFile() {
        const fileInput = $('#fileUpload')[0].files[0];
        const customFileNameInput = $('#customFileName');
        const targetFolder = $('#uploadFolder').val();

        if (!fileInput) {
            alert('Please select a file');
            return;
        }

        
        const uploadedFileName = customFileNameInput.val() || fileInput.name; // Original filename
        const sanitizedFileName = uploadedFileName.replace(/\s+/g, '_').replace(/[^a-zA-Z0-9._-]/g, '');
        customFileNameInput.val(sanitizedFileName);

        const reader = new FileReader();

        reader.onload = function () {
            const fileData = reader.result.split(',')[1]; // Extract Base64 part of the data URL

            const payload = {
                fileData: fileData,
                customFileName: customFileNameInput.val(), // Use the value from the input field
                targetFolder: targetFolder
            };

            $.ajax({
                url: '../aspx/PluginCustomCode.aspx/UploadFile',
                type: 'POST',
                data: JSON.stringify(payload),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    if (response.d === "File uploaded successfully.") {
                        showAlertDialog("success", "File uploaded successfully.");
                        window.location.reload();
                    } else {
                        showAlertDialog("error", response.d);
                    }
                },
                error: function (error) {
                    console.error('Error uploading file:', error);
                }
            });
        };

        reader.onerror = function () {
            alert('Failed to read file. Please try again.');
        };

        // Read file as Base64
        reader.readAsDataURL(fileInput);
    }

    getHTMLPlugins() {
        let _this = this;
        var htmlPlugins = [];
        $.ajax({
            type: "POST",
            url: "../aspx/PluginCustomCode.aspx/GetHTMLPlugins",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var list = response.d;
                if (JSON.parse(list).result.message == 'success') {
                    htmlPlugins = JSON.parse(list).result.data.data;
                }               
            },
            error: function (xhr, status, error) {
                console.error("Error fetching file details: " + error);
            }
        });

        htmlPlugins.forEach(plugin => {
            if (_entityCommon.inValid(_this.navList[plugin.name])) {
                _this.navList[plugin.name] = {};
                _this.navList[plugin.name]["files"] = {};
            }
            _this.navList[plugin.name]["files"]["html"] = plugin.htmltext;
            _this.navList[plugin.name]["type"] = "htmlplugin";
            _this.navList[plugin.name]["groupname"] = "HTML Plugins";
        });
    }

    updatePlugin() {        
        const modifiedContent = _codeMirrorEdit.getValue();
        if (modifiedContent == "") {
            showAlertDialog("error", "HTML content is empty. Can't save.");
            return false;
        }

        var _this = this;
        _this.callHTMLPluginWS(_this.HTMLPluginName, "cards", modifiedContent, "UPDATE", "Plugin updated successfully.")
        return false;
    }

    deletePlugin() {
        if (!confirm("Do you want to delete this plugin?"))
            return false;

        var _this = this;
        _this.callHTMLPluginWS(_this.HTMLPluginName, "cards", "NA", "DELETE", "Plugin deleted successfully.")
        return false;
    }

    formatDate(dateString) {

        if (!dateString) return '';

        if (dateString == "New File") return '';

        dateString = dateString.replaceAll("T", " ");

        const dateTimeParts = dateString.split(' ');
        var dateParts;
        if (dateTimeParts[0].split('-').length == 3) {
            dateParts = dateTimeParts[0].split('-');
        }
        else if (dateTimeParts[0].split('/').length == 3) {
            dateParts = dateTimeParts[0].split('/');
        }

        if (dateParts.length == 3) {
            const timeParts = (dateTimeParts[1] || "00:00:00").split(':');

            var year, month, day;
            if (dateParts[0].length == 4) {
                year = dateParts[0];
                month = dateParts[1];
                day = dateParts[2];
            }
            else {
                year = dateParts[2];
                month = dateParts[1];
                day = dateParts[0];
            }

            const hours = timeParts[0];
            const minutes = timeParts[1];
            const seconds = timeParts[2];

            const formattedDate = dtCulture === "en-us"
                ? `${month}/${day}/${year}`
                : `${day}/${month}/${year}`;

            const timePart = (hours !== "00" || minutes !== "00" || seconds !== "00")
                ? `${hours}:${minutes}:${seconds}`
                : '';

            return timePart ? `${formattedDate} ${timePart}` : formattedDate;
        }
        else {
            return dateString;
        }
    }

}

$(document).ready(function () {
    _entityCommon = new EntityCommon();
    _customPlugins = new CustomPlugins();
    _customPlugins.init();

});
