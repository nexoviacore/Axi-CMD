<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wizardsetting_new.aspx.cs" Inherits="aspx_wizardsetting_new" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Wizard Setting"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>Wizard Setting</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/WizardCM") %>
    </asp:PlaceHolder>
    <% if (direction == "rtl")
        { %>
    <link rel="stylesheet" href="../ThirdParty/bootstrap_rtl.min.css" type="text/css" />
    <% } %>
    <%--<script>
        if(typeof localStorage != "undefined"){
            var projUrl =  top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
            var lsTimeStamp = localStorage["customGlobalStylesExist-" + projUrl]
            if(lsTimeStamp && lsTimeStamp != "false"){
                var appProjName = localStorage["projInfo-" + projUrl] || "";
                var customGS = "<link id=\"customGlobalStyles\" data-proj=\""+ appProjName +"\" href=\"../"+ appProjName +"/customGlobalStyles.css?v="+ lsTimeStamp +"\" rel=\"stylesheet\" />";
                document.write(customGS);
            }
        }
    </script>
    <script>
        try {
            if (typeof localStorage != "undefined") {
                var projUrl = top.window.location.href.toLowerCase().substring("0", top.window.location.href.indexOf("/aspx/"));
                var lsTimeStamp = localStorage["axGlobalThemeStyle-" + projUrl]
                if (lsTimeStamp && lsTimeStamp != "false") {
                    var axThemeFldr = localStorage["axThemeFldr-" + projUrl] || "";
                    var axCustomStyle = "<link id=\"axGlobalThemeStyle\" data-themfld=\"" + axThemeFldr + "\" href=\"../" + axThemeFldr + "/axGlobalThemeStyle.css?v=" + lsTimeStamp + "\" rel=\"stylesheet\" />";
                    document.write(axCustomStyle);
                }
            }
        } catch (ex) { }
    </script>--%>
    <script>
        (function () {
            if (typeof localStorage !== "undefined") {
                try {
                    let projUrl = top.window.location.href.toLowerCase().split("/aspx/")[0];
                    let lsTimeStamp = sanitizeInput(localStorage["customGlobalStylesExist-" + projUrl] || "");
                    let appProjName = sanitizeInput(localStorage["projInfo-" + projUrl] || "");
                    if (lsTimeStamp && lsTimeStamp !== "false" && appProjName) {
                        let linkElement = document.createElement("link");
                        linkElement.id = "customGlobalStyles";
                        linkElement.setAttribute("data-proj", appProjName);
                        linkElement.rel = "stylesheet";
                        let safeHref = encodeURI(`../${appProjName}/customGlobalStyles.css?v=${lsTimeStamp}`);
                        linkElement.href = safeHref;
                        document.head.appendChild(linkElement);
                    }

                    let themeTimeStamp = sanitizeInput(localStorage["axGlobalThemeStyle-" + projUrl] || "");
                    let axThemeFldr = sanitizeInput(localStorage["axThemeFldr-" + projUrl] || "");
                    if (themeTimeStamp && themeTimeStamp !== "false" && axThemeFldr) {
                        let themeLink = document.createElement("link");
                        themeLink.id = "axGlobalThemeStyle";
                        themeLink.setAttribute("data-themfld", axThemeFldr);
                        themeLink.rel = "stylesheet";
                        let safeHref = encodeURI(`../${axThemeFldr}/axGlobalThemeStyle.css?v=${themeTimeStamp}`);
                        themeLink.href = safeHref;
                        document.head.appendChild(themeLink);
                    }
                } catch (ex) {
                }
            }
            function sanitizeInput(input) {
                return input.replace(/[^a-zA-Z0-9_\-\/]/g, "");
            }
        })();
    </script>
    <style>
        .CodeMirror-hints {
            z-index: 9999999999 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/Js/WizardCMJS") %>
        </asp:PlaceHolder>
        <div class="wizardMain-container">
            <div class="container1">
                <div class="col-sm-12 col-md-12">

                    <div class="developerWorkBenchToolbar">
                        <asp:HiddenField ID="tbxeditPage" runat="server" />
                        <input type="hidden" id="txtdropedId" name="txtdropedId" />
                        <input type="hidden" id="txttsId" name="txttsId" />
                        <input type="hidden" id="txttsName" name="txttsName" />
                        <input type="hidden" id="txtPrName" name="txtPrName" />
                        <ul class="dwbiconsUl" id="Formstb" style="text-align: right;">
                            <li class="dropdown" style="display: none;"><a href="javascript:void(0)" id="customGroup1" title="Custom Actions"
                                class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown"
                                data-close-others="true" aria-expanded="false">New <span
                                    class="icon-arrows-down"></span></a>
                                <ul class="dropdown-menu">
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button"
                                            aria-haspopup="true" aria-expanded="true"><span class="nav-label">Add
                                            Field</span> <span class="icon-arrows-down"></span></a>
                                        <ul class="dropdown-menu">
                                        </ul>
                                    </li>

                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_adddc" title="Add Frames"
                                        class="dwbBtn">Add Frames</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_genmap" title="Add GenMap"
                                        class="dwbBtn">Post to another form</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_fillg"
                                        title="Add Fill Grid" class="dwbBtn">Add Fill Grid</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_mdmaps" title="Add MdMap"
                                        class="dwbBtn">Update details to another form</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_actions" title="Actions"
                                        class="dwbBtn">Actions</a></li>
                                </ul>
                            </li>
                            <li><a href="javascript:void(0);" onclick="window.location.reload();" id=""
                                title="New" class="">New</a></li>
                            <li><a href="updatewizard.aspx" id=""
                                title="List View" class="">List View</a></li>
                            <li><a href="javascript:void(0);" data-toggle="modal"
                                data-target="#myModalPublish" id="btn_callws" title="Save" class="dwbBtn">Save</a></li>
                        </ul>

                        <ul class="dwbiconsUl" id="ArrangeMtb" style="display: none;">

                            <li><a href="javascript:void(0);" id="lnkChngIcon" title="Change Icon" class="dwbBtn">Change
                                Icon</a></li>
                            <li><a href="javascript:void(0);" id="btnAdd" title="Add Folder" class="dwbBtn">Add Folder</a>
                            </li>
                            <li><a href="javascript:void(0);" id="btnDelete" title="Delete" class="dwbBtn">Delete</a></li>
                            <li><a href="javascript:void(0);" id="btnSave" title="Save" class="dwbBtn">Save</a></li>

                        </ul>
                    </div>
                </div>
                <div class="col-sm-4 col-md-4">
                    <div class="wizardLeftMenu">
                        <div class="leftsidetopmenu">
                            <ul>
                                <li><a onclick="showTstruct();" href="javascript:void();">Forms</a></li>
                                <li><a onclick="showIview();" href="javascript:void();">Views</a></li>
                                <li><a onclick="showPageDtl();" href="javascript:void();">Pages</a></li>
                                <li><a onclick="showPaymentGatewy();" href="javascript:void();">Utils</a></li>
                            </ul>
                        </div>
                        <div id="LeftMenu" class="column" style="padding-top: 10px; padding-left: 10px;">
                        </div>
                    </div>
                </div>
                <div class="col-sm-8 col-md-8" style="padding: 0px 10px 10px 10px; background-color: white; border: 1px solid lightgrey">

                    <div id="rightMenu" class="column" style="background-color: lightgray;">
                        <div id="bg1">
                            <div id="background">
                                <p id="bg-text">
                                    Drag and Drop
                                <br />
                                    Here
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal -->
            <div class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Parameter Cofiguration</h4>
                        </div>
                        <div class="modal-body">
                            <div class="tab-pane fade in active">
                                <div class="col-xs-12 col-lg-5 col-md-5" style="">
                                    <div class="form-group">
                                        <label for="message-text" id="lbTrg" class="col-form-label fw-boldest">
                                            Target Field/Param:</label>
                                        <select id="ddlResParam" class="form-control">
                                            <option value="0">--Select Field/Param--</option>
                                        </select>

                                    </div>
                                </div>
                                <div class="col-xs-12 col-lg-5 col-md-5" style="">
                                    <div class="form-group">
                                        <label for="message-text" id="lbSrc" class="col-form-label fw-boldest">
                                            Source Field/Param:</label>
                                        <select id="ddlRParam" class="form-control">
                                            <option value="0">--Select Field/Param--</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-lg-2 col-md-2" style="padding-top: 25px;">
                                    <div class="form-control btn btn-primary wizardpop" onclick="putParamVal();">
                                        Add
                                    </div>
                                </div>
                                <div class="col-xs-12 col-lg-12 col-md-12" style="padding-top: 10px;" id="divTbl">
                                </div>
                                <div class="col-xs-12 col-lg-12 col-md-12" style="padding-top: 10px;">
                                    <input type="checkbox" id="chkAction" name="chkAction" /> <label for="message-text" class="col-form-label fw-boldest" >Open in Edit Mode</label>
                                </div>
                                <div class="col-xs-12 col-lg-12 col-md-12" style="z-index: 1; display: none;">
                                    <div class="form-group">
                                        <label for="message-text" class="col-form-label fw-boldest">
                                        </label>
                                        <textarea id="txtParam" name="txtParam" class="form-control" style="z-index: 100000000; height: 150px;"></textarea>
                                    </div>
                                </div>
                                <div id="QryEditor" class="tab-pane fade in active" runat="server" style="z-index: 100000000; display: none;">
                                    <div id="splitWrapper" style="z-index: 100000000;">
                                        <textarea id="txtEditor" name="txtEditor" style="z-index: 100000000; height: 150px;"></textarea>
                                        <input type="hidden" id="txtParaVal" name="txtParaVal" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <a href="javascript:void();" onclick="getInfoParam();" class="btn btn-primary">Submit</a>
                        </div>
                    </div>
                </div>
            </div>
            <!--Modal 2 for Script-->
            <div class="modal fade" id="myModalScript" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Script Configuration</h4>
                        </div>
                        <div class="modal-body">
                            <div id="Div1" class="tab-pane fade in active" runat="server" style="z-index: 100000000;">
                                <div id="splitWrapper1" style="z-index: 100000000;">
                                    <textarea id="txtEditor1" name="txtEditor1" style="z-index: 100000000; height: 150px;"></textarea>
                                    <input type="hidden" id="txtScriptVal" name="txtScriptVal" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <a href="javascript:void();" onclick="getInfoScript();" class="btn btn-primary">Save changes</a>
                        </div>
                    </div>
                </div>
            </div>
            <!--Modal 2 for Props-->
            <div class="modal fade" id="myModalProps" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Property Configuration</h4>
                        </div>
                        <div class="modal-body">
                            <div id="Div2" class="tab-pane fade in active" runat="server" style="z-index: 100000000;">
                                <div id="splitWrapper2" style="z-index: 100000000;">
                                    <textarea id="txtEditor2" name="txtEditor2" style="z-index: 100000000; height: 150px;"></textarea>
                                    <input type="hidden" id="txtPropsVal" name="txtPropsVal" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <a href="javascript:void();" onclick="getInfoProps();" class="btn btn-primary">Save changes</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal" id="myModalPublish">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Publish Page</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="container"></div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="recipient-name" class="col-form-label fw-boldest">
                                Page  Title:</label>
                            <input type="text" id="tbxPageTitle" name="tbxPageTitle" class="form-control" />
                            <%--<asp:TextBox ID="tbxPageTitle" runat="server" CssClass="form-control"></asp:TextBox>--%>
                        </div>
                        <div class="form-group">
                            <label for="recipient-name" class="col-form-label fw-boldest">
                                Description:</label>
                            <textarea id="tbxDescription" name="tbxDescription" class="form-control" style="z-index: 100000000; height: 150px;"></textarea>
                            <%--<asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="modal-footer">
                       <!-- <asp:Button ID="btnPublish" runat="server" CssClass="btn btn-secondary dropdown-toggle" BackColor="White" Text="Publish" />-->
                        <a href="#" data-dismiss="modal" class="btn btn-secondary dropdown-toggle">Close</a>
                        <a href="#" onclick="publishPage();"  class="btn btn-primary dropdown-toggle">Publish</a>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
