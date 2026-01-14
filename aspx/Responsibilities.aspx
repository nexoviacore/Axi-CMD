<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Responsibilities.aspx.cs"
    Inherits="aspx_Responsibilities" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="Responsibilities"/>
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP"/>
    <meta name="author" content="Agile Labs"/>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>Responsibility</title>
    <!-- ________ CSS __________ -->
    <%--<link href="../Css/generic.min.css?v=10" rel="stylesheet" type="text/css" id="generic" />--%>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
    
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <link href="../Css/Icons/icon.css" rel="stylesheet" />
    <link href="../Css/thirdparty/font-awesome/4.6.3/css/font-awesome.min.css" rel="stylesheet" />
    <%--<link href="" rel="stylesheet" id="themecss" type="text/css" />--%>


    <link href="../Css/Users.min.css?v=5" rel="stylesheet" type="text/css" />
    <link href="../ThirdParty/Linearicons/Font/library/linearIcons.css" rel="stylesheet" />
    <link href="../ThirdParty/DataTables-1.10.13/media/css/jquery.dataTables.min.css" rel="stylesheet" />

    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" />
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
    <link href="../Css/responsibilties.min.css?v=12" rel="stylesheet" />
    

    <!-- ________ JAVASCRIPT __________ -->
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <script src="../Js/Jquery-2.2.2.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <script src="../AssetsNew/js/bootstrap.min.js"></script>
    <script src="../Js/common.min.js?v=158"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script src="../ThirdParty/DataTables-1.10.13/media/js/jquery.dataTables.min.js"></script>
    <script src="../Js/Resposibilities.min.js?v=22" type="text/javascript"></script>
    <script>
        var enableBackButton = '<%=enableBackButton%>';
        var enableForwardButton = '<%=enableForwardButton%>';
    </script>
    <style>
        #tblResponsibilities_filter input[type='search'] {
            margin-left: -10px;
        }
        .btextDir-rtl #searchclear{
              position: absolute;
            left: 18px;
            top: 2px;
            bottom: 0;
            margin: auto;
            font-size: 21px;
            cursor: pointer;
        }

        .btextDir-ltr #searchclear{
              position: absolute;
            right: 18px;
            top: 0px;
            bottom: 0;
            margin: auto;
            font-size: 21px;
            cursor: pointer;
        }

        .dataTable th, .dataTable td {
            max-width: 70px;
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
        }
    .dataTables_scrollHeadInner {
    width: 100% !important;
}
    .btextDir-rtl #divDeleteActions {
    text-align: left;
}

    </style>
    
</head>
<body class="btextDir-<%=direction%>"  dir="<%=direction%>">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="../WebService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="wBdr">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="backforwrdbuttons" class="hide backbutton <%=direction == "ltr"?"left":"right" %>  "><span class="navLeft icon-arrows-left-double-32 handCur" onclick="javascript:BackForwardButtonClicked(&quot;back&quot;);" id="goback" title="Click here to go back"></span></div>
                    <div id="breadcrumb-panel">
                        <div id="breadcrumb" class="<%=direction == "ltr"?"left":"right" %>">
                            <div class="<%=direction == "ltr"?"left":"right" %>  bcrumb">
                                    <span id="lblHeader">Responsibilities</span>
                            </div>
                        </div>
                    </div>
                    <div class="container-fluid">
                        <div class="clear"></div>
                        <div class="row">
                            <div class="form-group">
                                <span id="spnResponsibilityActions" class="col-sm-10">
                                    <div class="col-lg-2 col-xs-4 col-sm-2 col-md-2 " id="divSearchOptions">
                                        <select id="ddlSearchColumns" class="form-control" title="Search by">
                                            <option value="-1" id="optSearchAll">All</option>
                                            <option value="1" id="optSearchName">Name</option>
                                            <option value="2" id="optSearchAccessRights">Access Rights</option>
                                        </select>

                                    </div>
                                    <div class="col-lg-3 col-md-3 col-xs-4 col-sm-3">
                                        <input type="text" name="name" value="" id="txtGridSearch" class="form-control"/>
                                        <span id="searchclear" class="icon-arrows-remove colorButton search-clear" title="Clear"></span>
                                    </div>
                                    <%--<div class="col-lg-4" id="divDeleteStatus">
                                         <select id="ddlRespStatus" class="form-control">
                                        <option value="">All</option>
                                        <option value="Y">Active</option>
                                        <option value="N">Inactive</option>
                                    </select>
                                     </div>--%>
                                    <div class="col-lg-5 col-xs-4 col-md-5 col-sm-5" id="divDeleteActions">
                                        <%--<asp:LinkButton ID="imgCopyResponsibility" runat="server" CssClass="colorButton fa fa-clone action-button"
                                        OnClientClick="javascript:return copyResponsibility();" ToolTip="Copy" OnClick="imgCopyRes_Click" Visible="false"></asp:LinkButton>
                                    <asp:Button Text="text" runat="server" ID="btnhdnCopy" Style="display: none" />

                                    <asp:LinkButton ID="imgAddNewRes" runat="server" CssClass="colorButton icon-plus action-button"
                                        OnClientClick="javascript:ShowDimmer(true)"
                                        OnClick="imgAddNewRes_Click" ToolTip="Add" Visible="false"></asp:LinkButton>

                                    <asp:LinkButton ID="imgDelRes" runat="server" CssClass="colorButton icon-trash action-button"
                                        OnClientClick="javascript:return confirmDelete();" ToolTip="Delete"></asp:LinkButton>
                                    <asp:Button Text="text" runat="server" ID="btnhdnDel" OnClick="imdDelRes_Click" Visible="false" />--%>

                                        <%-- <asp:LinkButton ID="imgListAllRes" ToolTip="List All Responsibilities" runat="server"
                                        CssClass="colorButton fa fa-refresh action-button" 
                                        OnClick="imgListAllRes_Click"></asp:LinkButton>--%>

                                        <a href="javascript:void(0)" id="lnkAddResponsibility" title="Add" class="colorButton icon-plus action-button"></a>
                                        <a href="javascript:void(0)" id="lnkDeleteResponsibility" title="Delete" class="colorButton icon-trash action-button"></a>
                                    </div>
                                </span>
                                <table id="tblResponsibilities" class="gridData" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th>
                                                <input type="checkbox" name="select_all" value="1" id="chkSelectAll" /></th>
                                            <th>Name</th>
                                            <th>Access Rights</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div>
                                <asp:Label ID="lblNoRecordMsg" runat="server" Text="" CssClass="totrec" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div id='waitDiv'>
                        <div id='backgroundDiv'>
                        </div>
                    </div>

                    <%--<div id="dvErr" class="errBdr" runat="server" style="display: none; visibility: hidden; margin-top: 5px;">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left">
                                    <asp:HiddenField ID="errorMessage" runat="server" />
                                    <asp:HiddenField ID="successMessage" runat="server" />
                                </td>
                                <td style="text-align: right">
                                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="../AxpImages/icons/close-button.png" BorderWidth="0"
                                        OnClientClick="javascript:HideErrDiv('dvErr');" OnClick="imgListAllRes_Click" alt="Close" />
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="dvPopErrMsg" runat="server" class="errBdr">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDisErrMsg" class="erred" runat="server"></asp:Label>
                        </td>

                        <td>
                            <input type="button" value="Ok" onclick="javascript: HideErrDiv('dvPopErrMsg'); hiddenFloatingDiv('dvEditResp');" />
                        </td>
                    </tr>
                </table>
            </div>

            <br />
            <div class="container2">
                <div class="tblfoot">
                </div>
            </div>
        </div>
        <asp:Label ID="lblNodata" runat="server" meta:resourcekey="lblNodata" Visible="false">No data found.</asp:Label>
        <asp:HiddenField ID="hdnIsSearched" runat="server" />
        <asp:HiddenField ID="hdnTreeSelVal" runat="server" />
        <asp:HiddenField ID="hdnStatus" runat="server" />
        <%=acScript%>
    </form>
</body>
</html>
