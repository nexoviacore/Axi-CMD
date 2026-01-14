<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FastDataUtility.aspx.cs" Inherits="FastDataUtility" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="description" content="utility page" />
    <meta name="keywords" content="utility agile labs" />
    <meta name="author" content="agile labs" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>FastData Utility</title>

    <%--    <link href="../Css/thirdparty/bootstrap/bootstrap-multiselect.css" rel="stylesheet">--%>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript" src="../Js/thirdparty/jquery/3.1.1/jquery.min.js"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <%--custom alerts start--%>
    <link href="../Css/animate.min.css" rel="stylesheet" />
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <%--custom alerts end--%>
    <script src="../Js/thirdparty/bootstrap/3.3.6/bootstrap.min.js" type="text/javascript"></script>
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
    <link href="../Css/utility.min.css?v=3" rel="stylesheet" />
    <link href="../App_Themes/Gray/Stylesheet.min.css?v=23" rel="stylesheet" />
    <script src="../ThirdParty/Highcharts/highcharts.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-3d.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-more.js"></script>
    <script src="../ThirdParty/Highcharts/highcharts-exporting.js"></script>
    <script src="../ThirdParty/DataTables-1.10.13/media/js/jquery.dataTables.min.js"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <link href="../ThirdParty/Linearicons/Font/library/linearIcons.css" rel="stylesheet" />

    <link href="../Css/thirdparty/font-awesome/4.6.3/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../ThirdParty/DataTables-1.10.13/media/css/jquery.dataTables.min.css" rel="stylesheet" />

    <script type="text/javascript" src="../Js/common.min.js?v=158"></script>
    <script src="../Js/utility.min.js?v=23"></script>

    <link id="themecss" type="text/css" href="" rel="stylesheet" />


</head>
<body dir='<%=direction%>'>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="overflow: hidden">
            <div class="">
                <asp:Label for="filter depend" class="dset" meta:resourcekey="lblMemory" ID="Label1" runat="server">Memory Utilization 
                </asp:Label>
                <%--<asp:Label ID="Label2" runat="server" CssClass="maxcount"></asp:Label>--%>
            </div>
            <div class="form-group">
                <div class="card" align="center">
                    <div id="containerChartsPie" runat="server" style="height: 210px">
                    </div>
                </div>
            </div>
            <span id="spnUtilityActions" style="display: none"></span>
            <div class="col-lg-12">
                <div class="col-lg-4 col-md-6 col-sm-12">
                    <input type="text" name="name" value="" id="txtGridSearch" class="form-control" style="padding-right: 22px" />
                    <span id="searchclear" class="icon-arrows-remove colorButton" style="display: none" title="Clear"></span>
                    <a href="javascript:void(0);" id="btnDeleteKey" class="fa fa-trash-o colorButton utility-actions-button" aria-hidden="true" onclick="confirmDialog('delete');"></a>
                    <a href="javascript:void(0);" id="btnRefreshKey" class="fa fa-refresh colorButton utility-actions-button" aria-hidden="true" onclick="confirmDialog('refresh')"></a>
                </div>
                <table id="tblFastDataUtility" class="gridData" cellspacing="0" width="100%">
                    <thead>
                        <tr>
                            <th>
                                <input type="checkbox" name="select_all" value="1" id="chkSelectAll" />
                            </th>
                            <th>Sl. No</th>
                            <th>Redis Key's</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>

        <%--    <div id="header">
            <p class="fdata">
                <asp:Label runat="server" meta:resourcekey="lblPageTitile" ID="lblPageTitile">FastData Utility</asp:Label>
            </p>
        </div>
        <div style="display: none">
            <asp:Button ID="btnGetLog" Text="GetFDWLog" runat="server" OnClick="btnGetLog_Click" />
        </div>
        <div style="display: none">
            <div>
                <asp:Label runat="server" meta:resourcekey="lblrefresh" ID="lblrefresh">Add Key To Refresh</asp:Label><asp:TextBox ID="txtNewKey" runat="server"></asp:TextBox><asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="btnAdd_Click" />
            </div>
            <div>
                <asp:Label runat="server" meta:resourcekey="lbldsrefresh" ID="lbldsrefresh">Keys in DsRefresh</asp:Label><i></i>
                <div>
                    <asp:GridView ID="grdRefreshKeys" runat="server">
                    </asp:GridView>
                </div>
            </div>
        </div>
       
        <div id="main">
            <div class="row">
                    <div class="col-sm-12  col-sm-offset-1 col-md-12 col-md-offset-1">
                        <div class="row feeddesgin">

                            <div class="col-sm-12">
                                <div class="row selectrwoone">
                                    <div class="col-sm-12 col-xs-12">
                                       <div class="">
                                                <asp:Label for="filter depend" class="dset" meta:resourcekey="lblMemory" ID="lblMemory" runat="server">Memory Utilization 
                                                </asp:Label>
                                                <asp:Label ID="lblMaxmemory" runat="server" CssClass="maxcount"></asp:Label>
                                            </div>
                                            <div class="pie">
                                                    <div id="containerChartsPie" runat="server">
                                                    </div>
                                                </div>
                                    </div>

                                    <div class="col-sm-6 col-xs-12">
                                        <fieldset class="filedico">
                                            <legend>
                                                <asp:Label ID="lbllegend" runat="server" meta:resourcekey="lbllegend">Fast Data operation</asp:Label>
                                            </legend>
                                            <div class="row icodata">
                                                <div class="col-sm-12 ">
                                                    <asp:Label for="filterkey" class="dset" ID="lblQuickaction" meta:resourcekey="lblQuickaction" runat="server">Quick action </asp:Label>

                                                    <button id="btnRefersh" type="button" class="btn btn-default btn-sm custom" onclick="RefereshDataset();">
                                                        <span class="fa fa-refresh " aria-hidden="true" title="Referesh all following keys - fast data defination, association and dataset keys "></span>
                                                    </button>

                                                    <button id="btnFlush" type="button" class="btn btn-default btn-sm btn-danger" onclick="FlushAll();">
                                                        <span class="fa fa-trash-o" aria-hidden="true" title="Delete all following keys - fast data defination, association and dataset keys "></span>
                                                    </button>
                                                </div>
                                            </div>
                                        </fieldset>

                                    </div>
                                </div>


                                <div class="row selectrwo">

                                    <div class="col-sm-8 col-xs-8">
                                        <div class="btnprime">
                                            <asp:Button ID="btnListAllKeys" class="btn btn-primary  hotbtntiny" Text="List all Keys" ToolTip="All the fast data key's show on below" runat="server" OnClick="btnListAllKeys_Click" OnClientClick=""/>
                                        </div>

                                        
                                        <div class="wafile">
                                            <asp:TextBox ID="txtGridSearch" type="text" class="search form-control" runat="server" Visible="false" placeholder="Filter Grid"></asp:TextBox>
                                            <asp:Label ID="lblTotalKeys" runat="server" CssClass="maxcount"></asp:Label>
                                        </div>

                                    </div>
                                    <div class="col-sm-4 col-xs-4 del">

                                        <asp:TextBox ID="txtSearch" runat="server" Visible="false" type="text" class="search form-control" placeholder="Search Key "></asp:TextBox>
                                        <asp:Button ID="btnGO" runat="server" Visible="false" Text="GO" ToolTip="Based on search key matched key's will show on below grid" CssClass="btn btn-info custombtn" OnClick="btnGO_Click" />
                                        <asp:Button ID="btnClear" class="btn btn-info custombtn" Text="Clear" ToolTip="Clear Search Key" runat="server" Visible="false" OnClick="btnClear_Click" />
                                        <asp:HiddenField ID="hdnSearchText" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <!--Question one -->
                            <div class="btnsecond" style="display: inline-block">
                                            <asp:Button ID="btnClearAll" class="btn btn-primary  coldbtntiny" Text="Clear all" runat="server" Visible="false" OnClick="btnClearAll_Click" />
                                        </div>

                                        <div style="display: inline-block;">
                                            <button id="btnDelete" runat="server" type="button" class="btn btn-default btn-sm btn-danger"  onclick="DeleteAllKeys('delete');">
                                                <span class="fa fa-trash-o" aria-hidden="true" title="Delete selected key"></span>
                                            </button>
                                        </div>

                                        <div style="display: inline-block;">
                                            <button id="btnRefSelectedDtname" runat="server" type="button" class="btn btn-default btn-sm custom" onclick="RefereshDTName('referesh');">
                                                <span class="fa fa-refresh " aria-hidden="true" title="Referesh selected DT Name"></span>
                                            </button>
                                        </div>

                            <div class="row question">
                                
                                <asp:UpdatePanel ID="Updatepanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="col-sm-12 gridbox">
                                          
                                            <section>
                                                <asp:GridView ID="gvRedisKey" runat="server" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#0c7abf" AllowPaging="true" OnPageIndexChanging="OnPageIndexChange" PageSize="15" CssClass="table table-hover table-bordered results" Width="100%" CellSpacing="0" PagerStyle-CssClass="grdPageStyle" PagerSettings-Mode="NextPrevious" PagerSettings-NextPageText="Next" PagerSettings-PreviousPageText="Previous">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-CssClass="no-sort">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkrdsKeyID" class="checkthis" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </section>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <!--END-->
                        </div>
                    </div>
                </div>
        </div>--%>

        <asp:Label ID="lblnodata" runat="server" meta:resourcekey="lblnodata" Visible="false">No data found.</asp:Label>
        <asp:HiddenField ID="hfCount" runat="server" Value="0" />
        <asp:HiddenField ID="hdnKeyCount" runat="server" />
        <button id="btnCharts" type="button" class="hide" onclick="callCharts();"></button>
        <div class="modal fade" id="edit" tabindex="-1" role="dialog" aria-labelledby="edit" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                        </button>
                        <h4 class="modal-title custom_align" id="Heading">
                            <asp:Label ID="lbleditdtl" runat="server" meta:resourcekey="lbleditdtl">Edit Your Detail</asp:Label></h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <input class="form-control " type="text" placeholder="Sl No">
                        </div>
                        <div class="form-group">
                            <input class="form-control " type="text" placeholder="KeyName">
                        </div>
                    </div>
                    <div class="modal-footer ">
                        <button type="button" class="btn coldbtndynamic btn-lg" style="width: 100%;">
                            <span class="glyphicon glyphicon-ok-sign"></span>
                            <asp:Label ID="lblupdate" runat="server" meta:resourcekey="lblupdate">Update</asp:Label>
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <div class="modal fade" id="delete" tabindex="-1" role="dialog" aria-labelledby="edit" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                        </button>
                        <h4 class="modal-title custom_align" id="H1">
                            <asp:Label ID="lbldelete" runat="server" meta:resourcekey="lbldelete">Delete this entry</asp:Label>
                        /h4>
                    </div>
                    <div class="modal-body">
                        <div class="alert alert-danger">
                            <span class="glyphicon glyphicon-warning-sign"></span>
                            <asp:Label ID="lbldltrec" meta:resourcekey="lbldltrec" runat="server">Are you sure you want to delete this Record?</asp:Label>
                        </div>
                    </div>
                    <div class="modal-footer ">
                        <button type="button" class="btn btn-success hotbtntiny ">
                            Yes
                        </button>
                        <button type="button" class="btn btn-default coldbtntiny " data-dismiss="modal">
                            No
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </form>
    <div id='waitDiv' style='display: none;'>
        <div id='backgroundDiv' style='background: url(../Axpimages/loadingBars.gif) center center no-repeat rgba(255, 255, 255, 0.843137); background-size: 135px;'>
        </div>
    </div>
</body>
</html>
