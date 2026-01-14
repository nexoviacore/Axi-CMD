var menuJSON;
var fancytreeObj;
var isFormDirty = false;
var changedNodeArr = []; // renamed and changed icon menu items
var maxlevel = 2;
var actionSequenceArr = []; // reordered, add and deleted menu items
var prevNodeOrd = "";
var prevNodeLevel = "";
var changedVisibility = []; // changed visibility of menu items. format: Head19=false|,Page245=true|Head19~Head23
var xmlString = "";
var action, resName, responsibilitynameedit;
var menuTstIvAccess = "";
$(document).ready(function (event) {
    getUrlParams();
    if (action == "edit") {
        responsibilityName = resName;
        $("#editResLabel .page-caption").text(`Access control for responsibility: ${resName}`);
    } else if (action == "add") {
        var resfld = $("#txtReEditResp").val();
        responsibilityName = resfld;
    }
    callParentNew("loadFrame();", "function");
    $.ajax({
        type: "POST",
        url: "../WebService.asmx/GetResponsibilityDetails",
        cache: false,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({ "responsibility": `${responsibilityName}` }),
        dataType: "json",
        success: function (data) {
            xmlString = data.d;
            var match = xmlString.match(/<error>(.*?)<\/error>/);
            if (match) {
                showAlertDialog("error", match[1]);
            } else {
                var menuJSON = parseXMLToJSON(xmlString);
                //console.log(menuJSON);
                menuJSON = menuJSON.root.root.child;
                menuTstIvAccess = menuJSON;
                createTheTreeView({ menuJSON, treeContainer: "#contain" });
                fancytreeObj = $.ui.fancytree.getTree("#contain");
                fancytreeObj.selectAll(false);
                fancytreeObj.$container.addClass("h-100 overflow-auto border-0");
                $("#contain").fancytree("getRootNode").visit(function (node) {
                    node.setExpanded(true);
                });
            }
            callParentNew("closeFrame();", "function");
        },
        error: function (error) {
            showAlertDialog("error", "An error occurred while retrieving Responsibility data.");
            callParentNew("closeFrame();", "function");
        }
    });

    function parseXMLToJSON(xmlString) {
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(xmlString, 'text/xml');
        const rootNode = xmlDoc.documentElement;
        const json = {};

        function parseNode(node) {
            const nodeName = node.tagName;
            const nodeAttributes = Array.from(node.attributes);
            const childNodes = Array.from(node.childNodes);

            const nodeData = {};

            if (nodeAttributes.length > 0) {
                for (const attribute of nodeAttributes) {
                    nodeData[attribute.name] = attribute.value;
                }
            }

            if (childNodes.length > 0) {
                const childArray = [];

                for (const childNode of childNodes) {
                    if (childNode.nodeType === Node.ELEMENT_NODE) {
                        childArray.push(parseNode(childNode));
                    }
                }

                nodeData.child = childArray;
            }

            return {
                [nodeName]: nodeData
            };
        }

        json[rootNode.tagName] = parseNode(rootNode);

        return json;
    }



    $("body").on("keyup", "input#amSearch", function (e) {
        var filterString = $(this).val();

        // Filter the tree nodes based on the search string
        filterResult = fancytreeObj.filterNodes.call(fancytreeObj, filterString);

        // Update the visibility of dynamically added nodes
        fancytreeObj.visit(function (node) {
            var isVisible = node.title.toLowerCase().indexOf(filterString.toLowerCase()) > -1;
            $(node.span).toggle(isVisible);

            // Check if the node is hidden and collapse it if necessary
            if (!isVisible && node.isExpanded()) {
                node.setExpanded(true);
            } else if (isVisible && node.isExpanded() && filterString == "") {
                node.setExpanded(false);
            }
        });
    });

    function getUrlParams() {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        this.action = urlParams.get('action');
        this.name = urlParams.get('name');
        action = this.action;
        resName = this.name;
        responsibilitynameedit = this.name;
        if (action == "add") {
            var resblk = document.getElementById("newRes");
            resblk.classList.remove("d-none");
            var editblk = document.getElementById("editResLabel");
            editblk.classList.add("d-none");
        } else {
            var resblk = document.getElementById("newRes");
            resblk.classList.add("d-none");
            var editblk = document.getElementById("editResLabel");
            editblk.classList.remove("d-none");
        }
    }

    $("#save_btn").off("click").on("click", function () {
        var responsibilityName;
        var checkedNodes = [];
        var checkboxparent;
        callParentNew("loadFrame();", "function");

        $(".fancytree-checkbox.actualcheckbox:checked").each(function () {
            var checkbox = $(this);
            var nodeSpan = checkbox.closest(".fancytree-node");
            var key = typeof checkbox.attr("data-tagname") == "undefined" ? "" : checkbox.attr("data-tagname");
            var title = nodeSpan.find(".fancytree-title").text();

            var parent = typeof checkbox.attr("data-parent") == "undefined" ? "" : checkbox.attr("data-parent");
            var sname = nodeSpan.find(".fancytree-checkbox").attr("data-sname");

            // Check if the node has a parent
            if (nodeSpan.parent().hasClass("fancytree-childnode")) {
                var parentNodeSpan = nodeSpan.closest(".fancytree-node").parent().closest(".fancytree-node");
                parent = parentNodeSpan.find(".fancytree-title").text();
            }

            // Construct the node object
            var node = {
                key: key,
                title: title,
                parent: parent,
                sname: sname
            };
            checkedNodes.push(node);
        });

        //console.log(checkedNodes);

        var xml = "";
        for (var i = 0; i < checkedNodes.length; i++) {
            var node = checkedNodes[i];
            xml += '<' + node.key + ' parent="' + node.parent + '">' + node.title.replaceAll("&", "&amp;") + '</' + node.key + '>';
        }
        let _isIview = false;
        let _isTstruct = false;
        $(".fancytree-checkbox.additionalcheckbox:checked").each(function () {
            var checkbox = $(this);
            var nodeSpan = checkbox.closest(".fancytree-node");
            var title = nodeSpan.find(".fancytree-title").text();
            var sname = nodeSpan.find(".fancytree-checkbox").attr("data-sname");

            if (sname.startsWith("i")) {
                _isIview = true;
                checkboxparent = "Iviews";
            } else if (sname.startsWith("t")) {
                _isTstruct = true;
                checkboxparent = "Tstructs";
            }

            sname = sname.substr(1);

            var additionalNode = {
                key: sname,
                title: title,
                parent: checkboxparent
            };
            checkedNodes.push(additionalNode);

            xml += '<' + additionalNode.key + ' parent="' + additionalNode.parent + '">' + additionalNode.title.replaceAll("&", "&amp;") + '</' + additionalNode.key + '>';
        });

        if (action == "edit") {
            responsibilityName = responsibilitynameedit;
            resName = "update";
        } else if (action == "add") {
            resName = "add";
            responsibilityName = $("#txtReEditResp").val();
        }
        if (responsibilityName != "" && xml != "") {
            $.ajax({
                type: "POST",
                url: "../WebService.asmx/EditResponsibilityDetails",
                cache: false,
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({ "action": resName, "responsibility": responsibilityName, "saveXML": xml, "isTstructSel": _isTstruct.toString(), "isIviewSel": _isIview.toString() }),
                dataType: "json",
                success: function (data) {
                    if (data.d == "<success>success</success>")
                        showAlertDialog("success", "Responsibility saved successfully.");
                    else if ((data.d).match(/<error>(.*?)<\/error>/).length > 0)
                        showAlertDialog("error", (data.d).match(/<error>(.*?)<\/error>/)[1] || "An error occurred while saving the responsibility. Please try again later.");
                    else
                        showAlertDialog("error", "Responsibilty is not saved. Please try again later.");
                    callParentNew("closeFrame();", "function");
                },
                error: function (error) {
                    showAlertDialog("error", "Responsibilty is not saved. Please try again later.");
                    callParentNew("closeFrame();", "function");
                }
            });
        } else if (responsibilityName == "") {
            showAlertDialog("warning", "The responsibility name cannot be empty. Please enter a responsibility name.");
            callParentNew("closeFrame();", "function");
        } else if (xml == "") {
            showAlertDialog("warning", "Please select the access for the responsibility.");
            callParentNew("closeFrame();", "function");
        }
    });
});

(function () {
    var isRootSelected = false;

    function createTheTreeView({ menuJSON, treeContainer }) {
        treeContainer = $(treeContainer);
        menuJSON = menuJSON[0];
        //nodeCreatorInput = $(nodeCreatorInput);
        var FinalmenuJson = _parseTheMenuJsonForTreeView(menuJSON);
        if (FinalmenuJson.length === undefined) {
            FinalmenuJson = [FinalmenuJson];
        }
        treeContainer.fancytree({
            create: function (event, data) {
                tree = data.tree;
                // Assign the fancytree instance to the "tree" variable
            },
            checkbox: false,
            selectMode: 2,
            icon: false,
            autoCollapse: true,
            quicksearch: true,
            defaultKey: function () {
                return "Head" + randomInteger(100000, 999999);
            },
            // focusOnSelect: true,
            extensions: ["glyph", "filter"],
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
                    _addClass: "material-icons material-icons-style",
                    checkbox: { text: "check_box_outline_blank" },
                    checkboxSelected: { text: "check_box" },
                    checkboxUnknown: { text: "indeterminate_check_box" },
                    dragHelper: { text: "play_arrow" },
                    dropMarker: { text: "arrow-forward" },
                    error: { text: "warning" },
                    expanderClosed: { text: "chevron_right", addClass: "align-middle"},
                    expanderLazy: { text: "last_page" },
                    expanderOpen: { text: "expand_more", addClass: "align-middle" },
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
                hideExpanders: false,       // Hide expanders if all child nodes are hidden by filter
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

                return true;
            },
            renderNode: function (event, data) {
                var node = data.node || data,
                    nodeSpan = $(node.span);

                if (!nodeSpan.data("rendered") || event.type === "close" || event === "changeIcon") {
                    var tstructimg = $(`<span class="material-icons material-icons-style text-info align-middle">assignment</span>`);
                    var iviewimg = $(`<span class="material-icons material-icons-style text-warning align-middle">view_list</span>`);
                    var folderimg = $(`<span class="material-icons material-icons-style text-success align-middle">folder_open</span>`);
                    var htmlimg = $(`<span class="material-icons material-icons-style text-danger align-middle">description</span>`);


                    if (node.data.sname.startsWith("t")) {
                        tstructimg.insertBefore(nodeSpan.find(".fancytree-title"));
                    } else if (node.data.sname.startsWith("i")) {
                        iviewimg.insertBefore(nodeSpan.find(".fancytree-title"));
                    } else if (node.data.sname == "" && node.data.parent.startsWith("Head")) {
                        folderimg.insertBefore(nodeSpan.find(".fancytree-title"));
                    } else if (node.data.cap !== 'Pages' && node.type === 'parent' && node.data.parent == "") {

                        var shortform = getShortForm(node.data.cap);
                        var lab = document.createElement("label");
                        // lab.style.fontSize = "10px";
                        lab.classList.add("picon");
                        lab.innerHTML = shortform;
                        $(lab).insertBefore(nodeSpan.find(".fancytree-title"));
                    } else if (node.data.sname == "web") {
                        htmlimg.insertBefore(nodeSpan.find(".fancytree-title"));
                    }
                    var tagName = typeof node.data.tagName == "undefined" ? "" : node.data.tagName

                    var checkbox = $(`<input type="checkbox" class="fancytree-checkbox actualcheckbox treecheckbox w-auto position-absolute end-30 w-15px h-15px mt-3" data-tagName=${tagName} data-parent=${node.data.parent}> `);

                    if (node.type === "parent" && node.data.parent === "") {
                        var parentValue;
                        node.children.forEach(function (childNode) {
                            parentValue = childNode.data.tagName;
                            checkbox = $(`<input type="checkbox" class="fancytree-checkbox actualcheckbox treecheckbox w-auto position-absolute end-30 w-15px h-15px mt-3" data-tagName=${parentValue} data-parent=${childNode.data.parent} data-sname=${childNode.data.sname}>`);
                        });
                    }

                    if (node.data.checked === "true") {
                        checkbox.prop("checked", true);
                    }

                    checkbox.insertBefore(nodeSpan.find(".fancytree-title"));

                    if (node.data.sname.startsWith("i") || node.data.sname.startsWith("t")) {
                        var additionalCheckbox = $(`<input type="checkbox" class="fancytree-checkbox additionalcheckbox treecheckbox w-auto position-absolute end-10 w-15px h-15px mt-3" data-tagName=${node.data.sname.substr(1)} data-parent=${node.data.parent} data-sname=${node.data.sname}> `);


                        if (node.data.checked === "true") {
                            additionalCheckbox.prop("checked", true);
                        } else if (node.data.sname.startsWith("t")) {
                            var result = $.grep(menuTstIvAccess[1].Tstructs.child, function (element, index) {
                                return (typeof element[node.data.sname.substr(1)] != "undefined" && element[node.data.sname.substr(1)].checked == 'true');
                            });
                            if (result.length > 0)
                                additionalCheckbox.prop("checked", true);
                        }
                        else if (node.data.sname.startsWith("i")) {
                            var result = $.grep(menuTstIvAccess[2].Iviews.child, function (element, index) {
                                return (typeof element[node.data.sname.substr(1)] != "undefined" && element[node.data.sname.substr(1)].checked == 'true');
                            });
                            if (result.length > 0)
                                additionalCheckbox.prop("checked", true);
                        }

                        additionalCheckbox.insertBefore(nodeSpan.find(".fancytree-title"));

                        checkbox.insertAfter(additionalCheckbox);
                    }

                    node.setSelected(false);
                    nodeSpan.data("rendered", true);
                }
                node.addClass("border-bottom");
                nodeSpan.find(".fancytree-title").addClass("w-50 text-truncate fs-6 align-middle");                
            },
            click: function (event, data) {
                if (data.targetType == "title") {
                    var sname = data.node.data.sname;
                    var cap = data.node.data.cap;
                    if (sname != "")
                        openChildWindow(sname, cap);
                }

                if (data.targetType == "checkbox") {
                    if (data.node.type == "parent") {
                        var checkbox = $(data.node.span).find(".fancytree-checkbox");
                        var isChecked = checkbox.prop("checked");

                        // Check all child nodes, including hidden (collapsed) nodes
                        data.node.visit(function (node) {
                            var childCheckbox = $(node.span).find(".fancytree-checkbox");
                            childCheckbox.prop("checked", isChecked);

                            // Perform additional actions or updates on child checkboxes
                        }, { includeHidden: true });
                        checkUncheckParentnode(data.node.getParent(), isChecked);
                    } else {
                        // Check or uncheck additionalCheckbox based on actualcheckbox state
                        var checkbox = $(data.node.span).find(".fancytree-checkbox.actualcheckbox"); // Menu Access
                        var additionalCheckbox = $(data.node.span).find(".fancytree-checkbox.additionalcheckbox");  // Structure Access

                        checkbox.on("change", function () {
                            var isChecked = $(this).prop("checked");
                            additionalCheckbox.prop("checked", isChecked);

                            var parentNode = data.node.getParent();
                            while (parentNode) {
                                checkUncheckParentnode(parentNode, isChecked);
                                parentNode = parentNode.getParent();
                            }
                        });

                        additionalCheckbox.on("change", function () {
                            additionalCheckbox.prop("checked", $(this).prop("checked"));
                            if (!$(this).prop("checked")) {
                                checkbox.prop("checked", false).trigger("change");
                            }
                        });
                    }
                }

                function checkUncheckParentnode(node, isChildChecked) {
                    var checkbox = $(node.span).find(".fancytree-checkbox.actualcheckbox");
                    if (isChildChecked) {
                        checkbox.prop("checked", true);
                    } else {
                        if ($(node.ul).find(".fancytree-checkbox.actualcheckbox:checked").length) {
                            checkbox.prop("checked", true);
                        } else {
                            checkbox.prop("checked", false);
                        }
                    }
                }


                var activeNode = data.node,
                    orgEvent = $(data.originalEvent.target);
                if (data.targetType == "expander") {
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
                        } else {
                            nodeVisibileObj.class = "visibility";
                            nodeVisibileObj.status = true;
                        }

                        orgEvent.html(nodeVisibileObj.class);

                        if (activeNode.folder && activeNode.hasChildren()) {
                            activeNode.setExpanded(true);
                            activeNode.visit(function (node) {
                                node.setExpanded(true);
                                $(node.span).find(".fancytree-expander").trigger("click");
                                //  $(node.span).find(".fancytree-title").html(nodeVisibleObj.class);
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
                        } else if (changedVisibility.findIndex(changed => changed.name == nodeVisibileObj.name) > -1) {
                            elemIndex = changedVisibility.findIndex(changed => changed.name == nodeVisibileObj.name);
                            changedVisibility.splice(elemIndex, 1);
                            changedVisibility.push(nodeVisibileObj);
                        }
                    } else if (orgEvent.attr("role") == "delnode") {
                        activeNode.setSelected(true);
                        $("#btnDelete").trigger("click");
                    } else if (data.targetType == "icon") {
                        activeNode.setSelected(true);
                        createIconPopup('uploadIcon', 'hdnUserIconList', $('#hdnIconPath').val());
                    }
                }

                var enableaccessbtn = `toggle${data.node.data.cap}`;
                var enableaccesselem = document.getElementById(enableaccessbtn);
                if (enableaccesselem != null) {
                    enableaccesselem.addEventListener("change", function () {
                        var iframe = document.getElementById("iFrameUserAccess");
                        var checkbox = enableaccesselem.querySelector('input[type="checkbox"]');
                        if (checkbox.checked) {
                            iframe.classList.remove(..."pe-none opacity-50".split(" "));
                            var checkboxes = data.node.span.querySelectorAll('.fancytree-checkbox');
                            checkboxes.forEach(function (checkbox) {
                                checkbox.checked = true;
                            });
                            var parentNode = data.node.getParent();
                            if (parentNode) {
                                var parentCheckbox = $(parentNode.span).find(".fancytree-checkbox.actualcheckbox");
                                parentCheckbox.prop("checked", true);
                                var grandparentNode = parentNode.getParent();
                                while (grandparentNode) {
                                    var grandparentCheckbox = $(grandparentNode.span).find(".fancytree-checkbox.actualcheckbox");
                                    grandparentCheckbox.prop("checked", true);
                                    grandparentNode = grandparentNode.getParent();
                                }
                            }
                        } else {
                            iframe.classList.add(..."pe-none opacity-50".split(" "));
                            var checkboxes = data.node.span.querySelectorAll('.fancytree-checkbox');
                            checkboxes.forEach(function (checkbox) {
                                checkbox.checked = false;
                            });
                            var parentNode = data.node.getParent();
                            if (parentNode) {
                                var parentCheckbox = $(parentNode.span).find(".fancytree-checkbox.actualcheckbox");
                                parentCheckbox.prop("checked", false);
                                var grandparentNode = parentNode.getParent();
                                while (grandparentNode) {
                                    var grandparentCheckbox = $(grandparentNode.span).find(".fancytree-checkbox.actualcheckbox");
                                    grandparentCheckbox.prop("checked", false);
                                    grandparentNode = grandparentNode.getParent();
                                }
                            }
                        }
                    });
                }
            }
        });

        $("#toggleButtonSmall").on("change", function () {
            var isChecked = $(this).prop("checked");

            fancytreeObj.getRootNode().visit(function (node) {
                var checkbox = $(node.span)[0]?.querySelector('input[type="checkbox"]');
                var isCheckboxChecked = checkbox ? $(checkbox).prop("checked") : false;

                if (isChecked) {
                    if (node.data.checked && isCheckboxChecked) {
                        $(node.span).show();
                    } else {
                        $(node.span).hide();
                    }
                } else {
                    $(node.span).show();
                }
            });
        });


        $(".actualcheckbox").on("change", function () {
            var isChecked = $(this).prop("checked");
            var additionalCheckbox = $(this).closest(".fancytree-node").find(".additionalcheckbox");
            additionalCheckbox.prop("checked", isChecked);
        });

    }

    function openChildWindow(name, cap) {

        try {
            let _pObj = {
                selRole: $("#txtReEditResp").val(),
                prefix: name.startsWith("i") ? "iview" : name.startsWith("t") ? "tstruct" : "",
                checked: $(".fancytree-node").find(`input[data-sname='${name}']`).is(":checked"),
                icon: () => {
                    return _pObj.prefix == "iview" ? `view_list` : _pObj.prefix == "tstruct" ? `assignment` : "";
                },
                label: () => {
                    return _pObj.prefix ? `${cap}-${_pObj.prefix}` : "";
                },
                frameClass: () => {
                    return _pObj.checked ? "w-100 h-400px border-0 overflow-hidden" : "w-100 h-400px border-0 overflow-hidden pe-none opacity-50";
                },
                frameSrc: () => {
                    return _pObj.prefix == "iview" ? `useracciview.aspx?iname=` : _pObj.prefix == "tstruct" ? `useraccess.aspx?transid=` : "";
                }
            };
            _pObj.template = `<div class="d-flex gap-2">
                <span class="material-icons material-icons-style my-auto">${_pObj.icon()}</span>
                <label class="form-label col-form-label fw-bolder">
                    ${_pObj.label()}
                </label>
                <div class="d-flex gap-2 ms-auto">
                    <label class="form-label col-form-label">Enable Access</label>
                    <label id="toggle${cap}"class="form-check form-switch form-check-solid-- my-auto toggle-switch-small">
                        <input type="checkbox" id="toggleButtonSmallRight" class="form-check-input w-45px h-20px">
                    </label>
                </div>
            </div>
            <iframe id="iFrameUserAccess" class="${_pObj.frameClass()}" src="${_pObj.frameSrc()}${name.slice(1)}&role=${_pObj.selRole}"></iframe>`;

            $("#iviewpage").html(_pObj.template);
            $("#toggleButtonSmallRight").prop("checked", _pObj.checked);

            ShowDimmer(false);
        } catch (error) {
            ShowDimmer(false);
        }
    }

    function ShowDimmer(status) {
        DimmerCalled = true;
        var dv = $("#waitDiv");

        if (dv.length > 0 && dv != undefined) {
            if (status == true) {
                closeParentFrame();
                $("body.ER-Body").addClass("page-loading");
                document.onkeydown = function EatKeyPress() {
                    return false;
                }
            } else if (!$("body.ER-Body").hasClass("stay-page-loading")) {
                $("body.ER-Body").removeClass("page-loading");
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
                        $("body.ER-Body", window.opener.document).addClass("page-loading");
                    } else if (!$("body.ER-Body", window.opener.document).hasClass("stay-page-loading")) {
                        $("body.ER-Body", window.opener.document).removeClass("page-loading");
                    }
                }
            }
        }
        DimmerCalled = false;
    }

    function _parseTheMenuJsonForTreeView(menuJson) {
        let finalJSON = [];

        let { Pages: mainParent } = menuJson;

        if (mainParent) {
            let d = [];
            if (typeof mainParent !== "undefined") {
                d = mainParent;
            } else {
                const dynamicKey = Object.keys(mainParent[0])[0];
                if (mainParent[0][dynamicKey].hasOwnProperty("child")) {
                    // Access the child elements
                    d = mainParent[0][dynamicKey]["child"];
                }
            }
            /*if (d.length > 0) {*/
            if (d?.child.length > 0) {
                finalJSON = _parseTheChildJSON(d.child);
            }
        }
        return finalJSON;
    }

    function _parseTheChildJSON(JSONObj) {
        let finalJSON = [];
        JSONObj.forEach((element) => {
            let childKey = Object.keys(element).find(
                (key) => key.startsWith("H") || key.startsWith("P")
            );
            if (typeof childKey == "undefined")
                return;
            let childrens = element[childKey].child;

            if (childrens) {
                let childData = [];

                childData = _parseTheChildJSON(childrens);

                delete element[childKey].child;
                Object.assign(element[childKey], {
                    title: element[childKey].cap,
                    key: element[childKey].parent,
                    parent: (typeof element[childKey].parent == "undefined" || element[childKey].parent == "") ? "Pages" : element[childKey].parent,
                    folder: true,
                    children: childData,
                    tagName: childKey
                });

                var obj = { ...element[childKey] };
                finalJSON.push(obj);
            }
            else {
                var key = Object.keys(element)[0];
                element = element[Object.keys(element)];
                element.parent = (typeof element.parent == "undefined" || element.parent == "") ? "Pages" : element.parent;
                let { cap, target, parent, icon } = element;
                let { targetType, iconn } = _getTheTargetType({ target, icon });
                let folder = targetType === "folder";
                Object.assign(element, {
                    title: cap,
                    key: parent,
                    folder: folder,
                    icon: iconn,
                    tagName: key//element.tagName
                });
                if (cap != "Active Lists") {
                    finalJSON.push(element);
                }

            }
        });
        return finalJSON;
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
                } else if (target.indexOf("page.aspx") == 0 || target.indexOf("htmlPages.aspx") == 0) {
                    iconn = "insert_drive_file";
                }
                return { targetType: targetType, iconn: { text: iconn, addClass: 'material-icons' } };
            }
        }

        return { targetType: targetType, iconn: icon };

    }

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
                } else
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
    } else {
        node.data.level = (node.getLevel() - 1).toString();
    }

}

function ShowDimmer(status) {

    DimmerCalled = true;
    var dv = $("#waitDiv");

    if (dv.length > 0 && dv != undefined) {
        if (status == true) {
            closeParentFrame();
            $("body.ER-Body").addClass("page-loading");
            document.onkeydown = function EatKeyPress() {
                return false;
            }
        } else if (!$("body.ER-Body").hasClass("stay-page-loading")) {
            $("body.ER-Body").removeClass("page-loading");
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
                    $("body.ER-Body", window.opener.document).addClass("page-loading");
                } else if (!$("body.ER-Body", window.opener.document).hasClass("stay-page-loading")) {
                    $("body.ER-Body", window.opener.document).removeClass("page-loading");
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

function getShortForm(cap) {
    const words = cap.split(' ');
    let shortForm = '';

    if (words.length > 0) {
        const firstWord = words[0];
        let lastWord = words[words.length - 1];

        if (lastWord === undefined) {
            lastWord = '';
        }

        shortForm = firstWord.charAt(0) + lastWord.charAt(0);
    }

    return shortForm;
}
