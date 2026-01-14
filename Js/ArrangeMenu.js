var menuJSON;
var fancytreeObj;
var isFormDirty = false;
var changedNodeArr = []; // renamed and changed icon menu items
var maxlevel = 2;
var actionSequenceArr = []; // reordered, add and deleted menu items
var prevNodeOrd = "";
var prevNodeLevel = "";
var changedVisibility = []; // changed visibility of menu items. format: Head19=false|,Page245=true|Head19~Head23

$(document).ready(function (event) {
    callParentNew("HideStructLoader()", "function");
    $.ui.fancytree.debugLevel = 0;
    if (menuData) {
        menuData = menuData.replace(/&apos;/g, "'");
        var xml = parseXml(menuData);
        menuJSON = JSON.parse(xml2json(xml, ""));
    }

    createTheTreeView({ menuJSON, treeContainer: "#arrangeMenu" });
    fancytreeObj = $.ui.fancytree.getTree("#arrangeMenu");
    fancytreeObj.selectAll(false);
    $("#arrangeMenu").fancytree("getRootNode").visit(function (node) {
        node.setExpanded(false);
    });

    $('#btnDelete').on('click', function () {
        var selNodes = fancytreeObj.getSelectedNodes();
        if (selNodes.length == 0) {
            showAlertDialog('warning', 'Please select atleast one node.');
            return false;
        }
        deleteCnfrmBx = $.confirm({
            title: "Confirm!",
            onContentReady: function () {
                disableBackDrop('bind');
            },
            onOpen: function () { $('.cnfrmBxHotBtn').focus(); },
            escapeKey: 'buttonB',
            //rtl: isRtl,
            content: "Do you really want to delete ?",
            //columnClass: 'col s6 offset-s3',
            buttons: {
                buttonA: {
                    text: "Yes",
                    btnClass: 'hotbtn',
                    action: function () {
                        selNodes.forEach(function (node) {
                            // if (!node.folder) {           //This condition restrict user to remove folders only.
                            //     return false;
                            // }
                            while (node.hasChildren()) {
                                var child = node.getFirstChild();
                                prevNodeOrd = getNodeOrd(child);
                                prevNodeLevel = child.getLevel() - 1;
                                child.moveTo(node.parent, "child");
                                changeNodeLevel(child);
                            }
                            // if(node.getKeyPath())
                            var actionstring = getActionString("remove", node, node.getParent());
                            actionstring != "" ? actionSequenceArr.push(actionstring) : "";
                            if (Array.indexOf(changedNodeArr, node.key) != -1)
                                Array.remove(changedNodeArr, node.key);

                            if (changedVisibility.findIndex(changed => changed.name == node.key) > -1) {
                                elemIndex = changedVisibility.findIndex(changed => changed.name == node.key);
                                changedVisibility.splice(elemIndex, 1);
                            }

                            node.remove();
                        });
                        showAlertDialog("success", "Node(s) deleted successfully");
                    }
                },
                buttonB: {
                    text: "Cancel",
                    btnClass: 'coldbtn',
                    action: function () {
                        disableBackDrop('destroy');
                    }
                },
            }
        });

        return false;
    });
    $('#btnAdd').on('click', function () {
        // var node = $.ui.fancytree.getTree("#arrangeMenu").getActiveNode();
        // if (node) {
        //     node.editCreateNode("after", {
        //         title: "Node title",
        //         folder: true
        //     });
        // }
        // else {
        fancytreeObj.getRootNode().editCreateNode("child", {
            key: "Head" + randomInteger(100000, 999999),
            title: "New Folder",
            folder: true,
            level: "0",
        });

        // }
        return false;
    });
    $('#btnSave').on('click', function () {
        parent.ShowDimmer(true);
        var ordno = 0;
        fancytreeObj.visit(function (node) {
            node.data.ordno = ++ordno;
            node.data.parent = node.parent.key == "root_1" ? "" : node.parent.key;
            node.data.action ? node.data.action = node.data.action.toString() : "";
            node.getLevel() > maxlevel ? maxlevel = node.getLevel() : "";
            //node.data.childcnt=node.countChildren();
            if (node.getLevel() == 1) {
                if (node.getNextSibling() == null) {
                    node.data.menuPos = "default"
                }
                else {
                    node.data.menuPos = "Insert Before";
                    node.data.menuPath = getMenuPath(node, node.data.menuPos);
                }
            }
            else if (node.getLevel() > 1) {
                if (node.getNextSibling() == null) {
                    node.data.menuPos = "Add Under";
                    node.data.menuPath = getMenuPath(node, node.data.menuPos);
                }
                else {
                    node.data.menuPos = "Insert Before";
                    node.data.menuPath = getMenuPath(node, node.data.menuPos);
                }
            }
        });

        changedNodeArr.sort(function (a, b) {
            return fancytreeObj.getNodeByKey(a).data.ordno - fancytreeObj.getNodeByKey(b).data.ordno
        });
        changedNodeArr = changedNodeArr.map(ele => fancytreeObj.getNodeByKey(ele).getKeyPath());

        changedVisibility = changedVisibility.map(elem => [[elem.name, elem.status].join("="), elem.parentList.join("~")].join("|")).join();
        // changedVisibility.map(elem=> [elem.name,elem.status].join("=")).join();
        var treeJson = fancytreeObj.toDict();
        menuJSON.root.max = maxlevel;
        menuJSON.root.parent = ParseTreeJsonToOriginalFormat({ TreeJSON: treeJson });
        var MenuXML = document.createElementNS(null, "arrangemenu");
        var s = new XMLSerializer();
        MenuXML = s.serializeToString(OBJtoXML(menuJSON.root, MenuXML));
        //ShowDimmer(true);
        //ShowDimmer(false);
        var ldrText = "Arrange Menu..";
        callParentNew("StructLoader(" + ldrText + ")", "function");
        $.ajax({
            url: "ArrangeMenu.aspx/SaveTreeView",
            type: "POST",
            cache: false,
            async: true,
            data: JSON.stringify({
                key: MenuXML, changedNodes: changedNodeArr.toString(), actionSequence: actionSequenceArr.toString(), changedVisibility
            }),
            contentType: "application/json",
            success: function (data) {
                var result = data.d;
                if (result.status == "success") {
                    showAlertDialog("success", "Data saved successfully");
                    window.location.href = window.location.href;
                }
                else {
                    if (result.msg == "SESSION_TIMEOUT")
                        parent.window.location.href = "../aspx/sess.aspx";
                    else if (result.msg == "")
                        showAlertDialog("error", "Error while saving.");
                    else
                        showAlertDialog("error", result.msg);
                }
                parent.ShowDimmer(false);
                setTimeout(() => {
                    callParentNew("HideStructLoader()", "function");
                }, 0);                
            },
            failure: function () {
                showAlertDialog("error", "Error while saving.");
                parent.ShowDimmer(false);
                setTimeout(() => {
                    callParentNew("HideStructLoader()", "function");
                }, 0);
            },
            error: function () {
                showAlertDialog("error", "Error while saving.");
                parent.ShowDimmer(false);
                setTimeout(() => {
                    callParentNew("HideStructLoader()", "function");
                }, 0);
            }
        });
        //ShowDimmer(false);
    });
    $("#lnkChngIcon").on('click', function () {
        var activeNode = fancytreeObj.getSelectedNodes();
        if (!activeNode.length) {
            showAlertDialog('warning', 'Please select atleast one node.');
            return false;
        }
        createIconPopup('uploadIcon', 'hdnUserIconList', $('#hdnIconPath').val())
        return false;
    });


    $(document).off('click').on("click", '.modal-content #iconWrapperData  span', function (event) {
        // toggleIcons("destroy");
        if ($(event.target).is($('.DltIcnBtn')))
            return true;
        var activeNode = fancytreeObj.getSelectedNodes();
        if ($(this).find('img').length) {
            var filename = $(this).find('img').attr('src');
            for (var i = 0; i < activeNode.length; i++) {
                activeNode[i].icon = filename;
                activeNode[i].renderTitle();
                if (Array.indexOf(changedNodeArr, activeNode[i].key) == -1)
                    changedNodeArr.push(activeNode[i].key)
                fancytreeObj.options.renderNode("changeIcon", activeNode[i]);
                activeNode[i].setSelected(true);
            }
        }
        else {
            icon = $(this).text();
            for (var i = 0; i < activeNode.length; i++) {
                activeNode[i].icon = { text: icon, addClass: 'material-icons' };
                activeNode[i].renderTitle();
                if (Array.indexOf(changedNodeArr, activeNode[i].key) == -1)
                    changedNodeArr.push(activeNode[i].key);
                fancytreeObj.options.renderNode("changeIcon", activeNode[i]);
                activeNode[i].setSelected(true);
            }
        }
        if (!isFormDirty) {
            isFormDirty = true;
        }

    });

    $("body").on("keyup", "#srchIcon", function () {
        var value = $(this).val().toLowerCase();
        $("#iconWrapperData span").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }).on("keyup", "input#amSearch", function (e) {
        var filterResult,
            filterString = $(this).val();
        filterResult = fancytreeObj.filterNodes.call(fancytreeObj, filterString);
    });

    $(document).on("click", ".DltIcnBtn", function () {
        var fileName = $(this).data('caption');
        var _this = this;
        deleteCnfrmBx = $.confirm({
            title: "Confirm!",
            onContentReady: function () {
                disableBackDrop('bind');
            },
            onOpen: function () { $('.cnfrmBxHotBtn').focus(); },
            escapeKey: 'buttonB',
            //rtl: isRtl,
            content: "Do you really want to delete ?",
            //columnClass: 'col s6 offset-s3',
            buttons: {
                buttonA: {
                    text: "Yes",
                    btnClass: 'hotbtn',
                    action: function () {
                        $.ajax({
                            url: "ArrangeMenu.aspx/deleteIconImage",
                            type: "POST",
                            data: JSON.stringify({ fileName }),
                            dataType: 'json',
                            contentType: "application/json",
                            success: function (data) {
                                var result = data.d;
                                if (result == "Success") {
                                    $(_this).parent('span').remove();
                                    var fileArray = $('#hdnUserIconList').val().split(',');
                                    var index = fileArray.indexOf(fileName);
                                    fileArray.splice(index, 1);
                                    $('#hdnUserIconList').val(fileArray.join());
                                    showAlertDialog("success", "Icon deleted successfully");
                                }
                                else {
                                    showAlertDialog("error", "Error while Deleting");
                                }
                            },
                            failure: function () {
                                showAlertDialog("error", "Error while Deleting");
                            },
                            error: function () {
                                showAlertDialog("error", "Error while Deleting");
                            }
                        });
                        deleteCnfrmBx = "";
                    }
                },
                buttonB: {
                    text: "Cancel",
                    btnClass: 'coldbtn',
                    action: function () {
                        disableBackDrop('destroy');
                        deleteCnfrmBx = "";
                    }
                },
            }
        });
    }).on("click", "button#amResetSearch", function (e) {
        $("input#amSearch").val("");
        fancytreeObj.clearFilter();
    });
});

function validateFile(sender, args) {
    var filename = args.get_fileName();
    var filext = filename.substring(filename.lastIndexOf(".") + 1);
    var size = args._length;
    if ($('#hdnUserIconList').val().indexOf(filename) != -1) {
        var err = new Error();
        err.name = 'Duplicate File';
        err.message = 'Selected file alredy exists';
        throw (err);
        return false;
    }
    if (filext != "jpg" && filext != "jpeg" && filext != "png" && filext != "JPG" && filext != "PNG") {
        var err = new Error();
        err.name = 'File Format';
        err.message = 'Selected file is not supported, please select .jpg, .jpeg, .png file only';
        throw (err);
        return false;
    }
    if (size > 515785) {
        var err = new Error();
        err.name = 'Max Size';
        err.message = 'Max. icon upload size is 500 KB';
        throw (err);
        return false;
    }


}
function uploadError(sender, args) {
    showAlertDialog('error', args.get_errorMessage());
    $.each($('#uploadIcon input'), function () { //clear the value of hidden file upload control.
        $(this).val("");
    });
}
function SetNodeIcon(sender, e) {
    $('#hdnUserIconList').val($('#hdnUserIconList').val() + e.get_fileName() + ',');
    $('.userIcons').append('<span><img src="' + $('#hdnIconPath').val() + e.get_fileName() + '"><a  href="javascript:void(0)" title="Delete" data-caption="' + e.get_fileName() + '" class="icon-arrows-remove DltIcnBtn"></a></span>');
    $('#lblFileInf').text(e.get_fileName());
    $.each($('#uploadIcon input'), function () { //clear the value of hidden file upload control.
        $(this).val("");
    });
}


/**
 * @description convert JSON Object to XML document.
 * @author Rakesh
 * @date 2021-03-02
 * @param {object}         obj         :JSON object needs to be converted.
 * @param {object}         xmlDocument :root node of the xml document
 * @returns {object}                   :converted XML object.
 */
function OBJtoXML(obj, xmlDocument) {
    var xmldoc;
    for (var prop in obj) {
        // if(!(obj[prop] instanceof Array)){
        //     xmlDocument= document.createElement(prop);
        // }
        if (obj[prop] instanceof Array) {
            for (var array in obj[prop]) {
                if (typeof obj[prop][array].key != 'undefined')
                    xmldoc = document.createElementNS(null, obj[prop][array].key);
                else
                    xmldoc = document.createElementNS(null, prop);
                //xmlDocument.appendChild(xmldoc);
                xmlDocument.appendChild(OBJtoXML(new Object(obj[prop][array]), xmldoc));
            }
        } else if (typeof obj[prop] == "object") {
            if (typeof obj[prop].key != 'undefined')
                xmldoc = document.createElementNS(null, obj[prop].key);
            else
                xmldoc = document.createElementNS(null, prop);
            xmlDocument.appendChild(OBJtoXML(new Object(obj[prop]), xmldoc));
        } else {
            // var xmlDocument =document.createElement(prop);
            xmlDocument.setAttributeNS(null, prop, (typeof obj[prop] == "string" ? ReverseCheckSpecialChars(obj[prop]) : obj[prop]));
        }
    }
    return xmlDocument;
}

(function () {
    var isRootSelected = false;



    function createTheTreeView({ menuJSON, treeContainer }) {
        treeContainer = $(treeContainer);
        //nodeCreatorInput = $(nodeCreatorInput);
        var FinalmenuJson = _parseTheMenuJsonForTreeView(menuJSON.root);
        if (FinalmenuJson.length === undefined) {
            FinalmenuJson = [FinalmenuJson];
        }
        treeContainer.fancytree({
            checkbox: true,
            selectMode: 2,
            icon: true,
            autoCollapse: true,
            quicksearch: true,
            defaultKey: function () {
                return "Head" + randomInteger(100000, 999999);
            },
            // focusOnSelect: true,
            extensions: ["dnd5", "edit", "glyph", "filter"],
            source: FinalmenuJson,//menuJSON,
            dnd5: {
                autoExpandMS: 400,
                focusOnClick: true,
                preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                preventRecursion: true, // Prevent dropping nodes on own descendants
                dragStart: function (node, data) {
                    /** This function MUST be defined to enable dragging for the tree.
                     *  Return false to cancel dragging of node.
                     */
                    prevNodeOrd = getNodeOrd(node);
                    prevNodeLevel = node.getLevel() - 1;
                    return true;
                },
                dragEnter: function (node, data) {
                    /** data.otherNode may be null for non-fancytree droppables.
                     *  Return false to disallow dropping on node. In this case
                     *  dragOver and dragLeave are not called.
                     *  Return 'over', 'before, or 'after' to force a hitMode.
                     *  Return ['before', 'after'] to restrict available hitModes.
                     *  Any other return value will calc the hitMode from the cursor position.
                     */
                    // Prevent dropping a parent below another parent (only sort
                    // nodes under the same parent)
                    /*           if(node.parent !== data.otherNode.parent){
                                return false;
                              }
                              // Don't allow dropping *over* a node (would create a child)
                              return ["before", "after"];
                    */
                    if (!node.isFolder()) {
                        return ["before"];
                    }
                    return true;
                },
                dragDrop: function (node, data) {
                    /** This function MUST be defined to enable dropping of items on
                     *  the tree.
                     */
                    data.otherNode.moveTo(node, data.hitMode);
                },
                dragEnd: function (node, data) {
                    // node.data.level = (node.getLevel() - 1).toString()
                    changeNodeLevel(node);
                }

            },
            edit: {
                adjustWidthOfs: null,
                triggerStart: ["clickActive", "dblclick", "f2", "mac+enter", "shift+click", "active", "focus"],
                beforeEdit: function (event, data) {
                    if (!data.node.folder) {           //This condition restrict user to edit the folder name only.
                        return false;
                    }
                },
                edit: function (event, data) {
                    $(data.input).attr("maxlength", "50");
                },
                close: function (event, data) {
                    fancytreeObj.options.renderNode(event, data);
                }

            },
            // createNode: function(node,data){
            //       node.title= node.title + Date.now().toString();
            // },
            glyph: {
                preset: "material",
                // map:{}
                map: {
                    _addClass: "material-icons",
                    checkbox: { text: "check_box_outline_blank" },
                    checkboxSelected: { text: "check_box" },
                    checkboxUnknown: { text: "indeterminate_check_box" },
                    dragHelper: { text: "play_arrow" },
                    dropMarker: { text: "arrow-forward" },
                    error: { text: "warning" },
                    expanderClosed: { text: "chevron_right" },
                    expanderLazy: { text: "last_page" },
                    expanderOpen: { text: "expand_more" },
                    loading: { text: "autorenew", addClass: "fancytree-helper-spin" },
                    nodata: { text: "info" },
                    noExpander: { text: "" },
                    radio: { text: "radio_button_unchecked" },
                    radioSelected: { text: "radio_button_checked" },
                    // Default node icons.
                    // (Use tree.options.icon callback to define custom icons based on node data)
                    doc: { text: "web_asset" },
                    docOpen: { text: "web_asset" },
                    folder: { text: "folder" },
                    folderOpen: { text: "folder_open" }
                }
            },
            filter: {
                // autoApply: true,   // Re-apply last filter if lazy data is loaded
                autoExpand: true, // Expand all branches that contain matches while filtered
                counter: false,     // Show a badge with number of matching child nodes near parent icons
                // fuzzy: false,      // Match single characters in order, e.g. 'fb' will match 'FooBar'
                // hideExpandedCounter: true,  // Hide counter badge if parent is expanded
                hideExpanders: true,       // Hide expanders if all child nodes are hidden by filter
                // highlight: true,   // Highlight matches by wrapping inside <mark> tags
                // leavesOnly: false, // Match end nodes only
                // nodata: true,      // Display a 'no data' status node if result is empty
                mode: "hide"       // Grayout unmatched nodes (pass "hide" to remove unmatched node instead)
            },

            //     dragEnd: function(node, data) {
            //     node.debug( "T1: dragEnd: " + "data: " + data.dropEffect + "/" + data.effectAllowed +
            //       ", dataTransfer: " + data.dataTransfer.dropEffect + "/" + data.dataTransfer.effectAllowed, data);
            //       alert("T1: dragEnd")
            //   },
            // icon: function(event, data) {
            //     var node = data.node; preventRecursion

            // },
            activate: function (event, data) {
                //        alert("activate " + data.node);
            },
            lazyLoad: function (event, data) {
                data.result = { FinalmenuJson }
            },
            modifyChild: function (event, data) {
                if (!isFormDirty) {
                    isFormDirty = true;
                }
                var node = data.childNode;
                var actionName = data.operation;
                var actionDetails = "";
                //addNodeAction(node,actionName);
                if (node) {
                    if (actionName == 'move' || actionName == 'add') {
                        actionDetails = getActionString(actionName, node, data.node)
                        actionDetails != "" ? actionSequenceArr.push(actionDetails) : "";
                        prevNodeOrd = "";
                        prevNodeLevel = "";
                    }
                    else {
                        if (actionName != "remove" && Array.indexOf(changedNodeArr, node.key) == -1)
                            changedNodeArr.push(node.key);
                    }
                }

                //   if(actionDetails !=""){
                //       actionSequenceArr.push(actionDetails);
                //   }

                return true;
            },
            renderNode: function (event, data) {
                var node = data.node || data,
                    nodeSpan = $(node.span);

                // check if span of node already rendered
                if (!nodeSpan.data("rendered") || event.type == "close" || event == "changeIcon") {

                    var nodeVisibility = "visibility";

                    if (changedVisibility.findIndex(changed => changed.name == node.key) > -1) {
                        nodeVisibility = changedVisibility[changedVisibility.findIndex(changed => changed.name == node.key)].class;
                    }
                    else {
                        nodeVisibility = node.data.visible == "T" ? "visibility" : node.data.visible == undefined ? "visibility" : "visibility_off";
                    }

                    var visibleButton = $(`<span role="visible" class="fancytree-visible material-icons">${nodeVisibility}</span>`),
                        deleteButton = $(`<span role="delnode" class="fancytree-delnode material-icons">highlight_off</span>`);

                    visibleButton.insertBefore(nodeSpan.find(".fancytree-icon, .fancytree-custom-icon"));
                    nodeSpan.append(deleteButton);
                    deleteButton.hide();

                    nodeSpan.hover(function () {
                        // mouse over         
                        if (!node.folder)
                            deleteButton.show();
                    }, function () {
                        // mouse out                        
                        deleteButton.hide();
                    });
                    node.setSelected(false);
                    nodeSpan.data("rendered", true);
                }
            },
            click: function (event, data) {

                if (data.targetType == "checkbox") {
                    return;
                }

                var activeNode = data.node,
                    orgEvent = $(data.originalEvent.target);

                // unselect all selected nodes as click is applied on single node
                $.each(fancytreeObj.getSelectedNodes(), function (ind, selNode) {
                    selNode.setSelected(false);
                });

                if (orgEvent.attr("role") == "visible") {

                    var nodeParentList = activeNode.getParentList(),
                        nodeVisibileObj = {
                            name: activeNode.key,
                            class: "visibility",
                            status: true,
                            parentList: []
                        };

                    if (orgEvent.html() == "visibility") {
                        nodeVisibileObj.class = "visibility_off";
                        nodeVisibileObj.status = false;
                    }
                    else {
                        nodeVisibileObj.class = "visibility";
                        nodeVisibileObj.status = true;
                    }

                    orgEvent.html(nodeVisibileObj.class);

                    if (activeNode.folder && activeNode.hasChildren()) {
                        activeNode.setExpanded(true);
                        activeNode.visit(function (node) {
                            node.setExpanded(true);
                            $(node.span).find(".fancytree-visible").html(nodeVisibileObj.class);
                        });
                    }

                    if (nodeParentList && nodeVisibileObj.status) {
                        $.each(nodeParentList, function (ind, ele) {
                            $(ele.span).find(".fancytree-visible").html(nodeVisibileObj.class);
                            nodeVisibileObj.parentList.push(ele.key);
                        });
                    }

                    if (changedVisibility.findIndex(changed => changed.name == nodeVisibileObj.name) == -1) {
                        changedVisibility.push(nodeVisibileObj);
                    }
                    else if (changedVisibility.findIndex(changed => changed.name == nodeVisibileObj.name) > -1) {
                        elemIndex = changedVisibility.findIndex(changed => changed.name == nodeVisibileObj.name);
                        changedVisibility.splice(elemIndex, 1);
                        changedVisibility.push(nodeVisibileObj);
                    }
                }
                else if (orgEvent.attr("role") == "delnode") {
                    activeNode.setSelected(true);
                    $("#btnDelete").trigger("click");
                }
                else if (data.targetType == "icon") {
                    activeNode.setSelected(true);
                    createIconPopup('uploadIcon', 'hdnUserIconList', $('#hdnIconPath').val());
                }
            }
        });
        $(".drag").draggable();

    }


    /**
     * @description Parse the orignal menu json to  suitable fancytree Input json.
     * @author Rakesh
     * @date 2021-03-02
     * @param {object}         menuJson : JSON needs to be parsed.
     * @returns{object}                 : Parsed JSON.
     */
    function _parseTheMenuJsonForTreeView(menuJson) {

        let finalJSON = [];
        let { parent: mainParent } = menuJson;
        if (mainParent.length === undefined) {
            mainParent = [mainParent];
        }
        if (mainParent) {
            mainParent.forEach((element, parentIndex) => {
                const { child: childrens } = element;
                if (childrens) {
                    let childData = [];

                    // if (Object.prototype.toString.call(childrens) === "[object Object]") {
                    //     childData.push(_parseTheMenuJsonForTreeView({ parent: [childrens] }));
                    // } else {

                    //     childrens.forEach((child, childIindex) => {
                    //         childData.push(_parseTheMenuJsonForTreeView({ parent: [child] }));
                    //     });
                    // }

                    if (typeof childrens.length == "undefined") {
                        childData.push(_parseTheMenuJsonForTreeView({ parent: [childrens] }));
                    } else {

                        childrens.forEach((child, childIindex) => {
                            childData.push(_parseTheMenuJsonForTreeView({ parent: [child] }));
                        });
                    }

                    delete element.child;
                    Object.assign(element, { title: element.name, key: element.oname, folder: true, children: childData });
                    finalJSON.push(element);
                } else {
                    let { name, target, oname, icon } = element;
                    let { targetType, iconn } = _getTheTargetType({ target, icon });
                    targetType === "folder" ? folder = true : folder = false;
                    Object.assign(element, { title: name, key: oname, folder: folder, icon: iconn });
                    finalJSON.push(element);
                }
            });
        }
        // return typeof finalJSON.length == "undefined" ? finalJSON[0] : finalJSON;
        return finalJSON.length === 1 ? finalJSON[0] : finalJSON;//for childNodes it will always reurn a JSON object and for final Json an Array will return if length > 1
    }

    function _getTheTargetType({ target, icon }) {
        let targetType = "folder";
        let iconn = "folder";
        if (target) {
            if (target.indexOf("tstruct.aspx") === 0 || target.indexOf("iview.aspx") === 0 || target.indexOf("iviewInteractive.aspx") === 0 || target.indexOf("page.aspx") == 0 || target.indexOf("htmlPages.aspx") == 0)
                targetType = 'item';
            if (icon == undefined) {
                if (target.indexOf("tstruct.aspx") === 0) {
                    iconn = "assignment";
                } else if (target.indexOf("iview.aspx") === 0 || target.indexOf("iviewInteractive.aspx") === 0) {
                    iconn = "view_list";
                }
                else if (target.indexOf("page.aspx") == 0 || target.indexOf("htmlPages.aspx") == 0) {
                    iconn = "insert_drive_file";
                }
                return { targetType: targetType, iconn: { text: iconn, addClass: 'material-icons' } };
            }
        }

        return { targetType: targetType, iconn: icon };

    }
    /**
     * @description parse the fancytree JSON to orignal menu json format
     * @author Rakesh
     * @date 2021-03-02
     * @param {*} { TreeJSON }
     * @returns 
     */
    function ParseTreeJsonToOriginalFormat({ TreeJSON }) {
        let JsonToSave = [];
        TreeJSON.forEach((element, parentIndex) => {
            const { data: attr, children: childrens, title: title, key: key } = element;
            delete element.children;
            delete element.data;
            if (childrens) {
                let childData = [];
                if (typeof childrens.length == "undefined") {
                    childData.push(ParseTreeJsonToOriginalFormat({ TreeJSON: [childrens] }));
                } else {

                    childrens.forEach((child, childIindex) => {
                        childData.push(ParseTreeJsonToOriginalFormat({ TreeJSON: [child] }));
                    });
                }
                if (attr) {
                    JsonToSave.push(Object.assign(element, attr, { name: title, oname: key }, { child: childData }));
                }
                else
                    JsonToSave.push(Object.assign(element, { name: title, oname: key }, { child: childData }));

            } else {
                if (attr)
                    JsonToSave.push(Object.assign(element, attr, { name: title, oname: key }));
                else
                    JsonToSave.push(Object.assign(element, { name: title, oname: key }));
            }

        });

        return JsonToSave.length === 1 ? JsonToSave[0] : JsonToSave;//for childNodes it will always reurn a JSON object and for final Json an Array will return if length > 1
    }


    window.createTheTreeView = createTheTreeView;
    window.ParseTreeJsonToOriginalFormat = ParseTreeJsonToOriginalFormat;
})();

function changeNodeLevel(node) {
    if (node.hasChildren()) {
        node.getChildren().forEach(function (child) {
            changeNodeLevel(child);
        });
        node.data.level = (node.getLevel() - 1).toString();
    }
    else {
        node.data.level = (node.getLevel() - 1).toString();
    }

}

function ShowDimmer(status) {

    DimmerCalled = true;
    var dv = $("#waitDiv");

    if (dv.length > 0 && dv != undefined) {
        if (status == true) {
            $("body").addClass("page-loading");
            document.onkeydown = function EatKeyPress() {
                return false;
            }
        } else if (!$("body").hasClass("stay-page-loading")) {
            $("body").removeClass("page-loading");
            document.onkeydown = function EatKeyPress() {
                if (DimmerCalled == true) {
                    return true;
                }
            }
        }
    } else {

        //TODO:Needs to be tested
        if (window.opener != undefined) {

            dv = $("#waitDiv", window.opener.document);
            if (dv.length > 0) {
                if (status == true) {
                    $("body", window.opener.document).addClass("page-loading");
                } else if (!$("body", window.opener.document).hasClass("stay-page-loading")) {
                    $("body", window.opener.document).removeClass("page-loading");
                }
            }
        }
    }
    DimmerCalled = false;
}
function randomInteger(min, max) {
    return (Math.floor(Math.random() * (max - min + 1)) + min).toString();
}
function getNodeOrd(node) {
    let ordno = 0
    fancytreeObj.visit(function (n, d) {
        ordno++;
        if (n.key == node.key) {
            return false;
        }
    });
    return ordno;
}

/**
 * @description Generate a string which contains informations about an action(move,delete,add) in the tree,each information is separated by a '|' separator.
 * @author Rakesh
 * @date 2021-03-02
 * @param {string} actionName: Name of the action performed
 * @param {object} node:Changed Node Object
 * @param {object} parentNode: Parent node of the Changed node.
 * @returns {string}
 * Format of the return string for various actions are as follows :-
 * move   : NodeName=actionname|prevOrder-currentOrder|NextNodeName|prevLevel-currentLevel|parentName.  Ex:head240=move|3-10|head239|0-1|head22.
 * Add    : NodeName=actionname|currentOrder|currentLevel|parentName.   Ex:head240=add|48|0|null.
 * delete : NodeName=actionname|currentOrder|currentLevel|parentName.   Ex:head240=remove|48|0|head22.
 */
function getActionString(actionName, node, parentNode) {

    var nextSibling = "";
    var curNode = node;
    nextSibling = curNode.getNextSibling() != null ? curNode.getNextSibling().key : "";

    if (nextSibling == "") {
        curNode = curNode.getParent();
        let curLvlNo = curNode.getLevel() - 1;
        for (i = curLvlNo; i >= 0; i--) {
            let tempNode = curNode.getNextSibling();
            if (tempNode != null) {
                nextSibling = tempNode.key;
                break;
            }
            else {
                curNode = curNode.getParent();
                curLvlNo = curNode.getLevel() - 1;
            }
        }
    }

    switch (actionName) {
        case 'move':
            if (prevNodeOrd != getNodeOrd(node)) {
                return node.key + "=" + actionName + "|" + prevNodeOrd + "-" + getNodeOrd(node) + "|" + nextSibling + "|" + prevNodeLevel + "-" + (node.getLevel() - 1).toString() + "|" + (parentNode.key == "root_1" ? "null" : parentNode.key);
            }
            break;
        case 'add':
            if (prevNodeOrd == "") {
                return node.key + "=" + actionName + "|" + getNodeOrd(node) + "|" + (node.getLevel() - 1).toString() + "|" + "null";
            }
            else {
                return node.key + "=" + "move|" + prevNodeOrd + "-" + getNodeOrd(node) + "|" + nextSibling + "|" + prevNodeLevel + "-" + (node.getLevel() - 1).toString() + "|" + (parentNode.key == "root_1" ? "null" : parentNode.key);
            }
            break;
        case 'remove':
            return node.key + "=" + actionName + "|" + getNodeOrd(node) + "|" + (node.getLevel() - 1).toString() + "|" + (parentNode.key == "root_1" ? "null" : parentNode.key);
            break;
        default:
            return "";
    }
}
/**
 * @description Generates the Menu path of the specified node based on the Menu Position.
 * @author Rakesh
 * @date 2021-03-02
 * @param {object} node:node whose menu path is required.
 * @param {sting} menuPos:Menu Position of the specified node.
 * @returns {string}
 */
function getMenuPath(node, menuPos) {
    if (menuPos == "insert Before")
        return "\\" + node.getNextSibling().getPath().replaceAll("/", "\\");
    else if (menuPos == "Add Under")
        return "\\" + node.getParent().getPath().replaceAll("/", "\\");
}
