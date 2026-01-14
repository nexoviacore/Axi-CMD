<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AxDBScript.aspx.cs" Inherits="aspx_AxDBScript" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="IView" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Database Scripting</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render("~/Css/DbScript") %>
    </asp:PlaceHolder>
    <% if (direction == "rtl")
        { %>
    <link rel="stylesheet" href="../ThirdParty/bootstrap_rtl.min.css" type="text/css" />
    <% } %>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>

    <script>
        var DbObjData = '<%=DbObjDataXML%>'
    </script>
    <style>
        .btn.btn-close---[data-bs-dismiss="modal"] span {
            color: red !important;
        }
    </style>
</head>
<body class="btextDir-<%=direction%>" dir="<%=direction%>">

    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server">
            <%:Scripts.Render("~/Js/DbScriptJS") %>
        </asp:PlaceHolder>

        <div id="schemaBrwsr" class="">
            <h4 class="card-title mb-4">DB Console</h4>
            <div id="divTreeView">
                <input name="search" id="srchTree" placeholder="Filter..." autocomplete="off" />
                <%--  <a href="javascript:void(0);" id="closeMenu" title="Close Menu" class="closeMenu"><span class="material-icons">navigate_before</span></a>--%>
                <div class="clearfix">
                </div>
                <div id="treeView"></div>
            </div>
            <div id="dataView">
                <div class="dvOpenMenu">
                    <ul>
                        <li>
                            <a href="javascript:void(0);" id="toggleScriptMenuLeft" title="Open menu"><span class="material-icons">navigate_before</span></a>
                        </li>
                    </ul>

                </div>
                <div class="AxDBToolBar">
                    <ul class="">
                        <%--<li><a href="javascript:void(0);" id="toggleScriptMenuLeft"  title="Toggle menu"><span class="material-icons">dehaze</span></a></li>--%>
                        <li><a href="javascript:void(0);" id="exeQuery" title="Execute(Ctrl+E)" class="execute"><i class="fa fa-bolt" style="color: white;" aria-hidden="true"></i></a></li>
                        <li><a href="javascript:void(0);" id="clrEdtr" title="Clear(Ctrl+L)" class="clear"><i class="fa fa-eraser" style="color: white;" aria-hidden="true"></i></a></li>
                        <li id="lstSbmt"><a href="javascript:void(0);" id="sbmtQry" title="Submit" class="clear"><i class="fa fa-arrow-circle-down" style="color: white;" aria-hidden="true"></i></a></li>

                    </ul>

                </div>
                <ul class="nav nav-tabs DbNavTabs">
                    <li><a data-toggle="tab" id="tabColumns" href="#dvColumns">Columns</a></li>
                    <li><a data-toggle="tab" id="tabInfo" href="#dvInfo">Info</a></li>
                    <li class="active"><a data-toggle="tab" id="tabSource" href="#QryEditor">Script/Source</a></li>
                    <li><a data-toggle="tab" id="tabIndexes" href="#dvIndexes">Indexes</a></li>
                    <li><a data-toggle="tab" id="tabTriggers" href="#dvTriggers">Triggers</a></li>
                    <li><a data-toggle="tab" id="tabData" href="#dvData">Data</a></li>
                    <li><a data-toggle="tab" id="tabErrors" href="#dvError">Error</a></li>
                    <li><a data-toggle="tab" id="tabArgs" href="#dvArgument">Argument</a></li>
                </ul>


                <div class="tab-content" runat="server">
                    <div id="noRecord" class="hide">No records found.</div>
                    <div id="dvColumns" class="tab-pane fade">
                        <table id="tblColumns"></table>
                    </div>
                    <div id="dvInfo" class="tab-pane fade">
                        <table id="tblInfo"></table>
                    </div>
                    <div id="QryEditor" class="tab-pane fade in active" runat="server">
                        <div id="ScriptToolBar">
                            <ul>
                                <li id="editScript"><a href="javascript:void(0)"><span class="material-icons" title="Edit">edit</span></a></li>
                                <li id="copyScript"><a href="javascript:void(0)"><span class="material-icons" title="Copy">content_copy</span></a></li>
                                <%--                                <li id="runScript"><a href="javascript:void(0)"><span class="material-icons" title="Run">play_arrow</span></a></li>--%>
                            </ul>
                        </div>
                        <div id="splitWrapper">
                            <textarea id="txtEditor"></textarea>

                            <div class="dvOutput">
                                <div class="rsltHeader">Result<span id="spnRowCnt"></span></div>
                                <div id="txtOutput"></div>
                                <table id="tblOutput">
                                </table>

                            </div>
                        </div>
                    </div>

                    <div id="dvIndexes" class="tab-pane fade">
                        <table id="tblIndexes"></table>
                    </div>
                    <div id="dvTriggers" class="tab-pane fade">
                        <table id="tblTriggers"></table>
                    </div>
                    <div id="dvData" class="tab-pane fade">
                        <table id="tblData"></table>
                    </div>
                    <div id="dvError" class="tab-pane fade">
                        <table id="tblError"></table>
                    </div>
                    <div id="dvArgument" class="tab-pane fade">
                        <table id="tblArgs"></table>
                    </div>

                </div>
                <div id="dvObjList">
                    <table id="tblObjList"></table>
                </div>
            </div>
        </div>
        <div id='waitDiv'>
            <div id='backgroundDiv'>
            </div>
        </div>


    </form>
</body>
</html>
