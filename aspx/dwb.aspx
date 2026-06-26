<%@ Page Title="" Language="C#" MasterPageFile="~/aspx/AxMPage.master" AutoEventWireup="true" CodeFile="dwb.aspx.cs" Inherits="dwb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/developerWorkBench.min.js?v=26" type="text/javascript"></script>
    <script>
        var SQLHintObj = '<%=SQLHintObj%>'
        var nodeAccessToken = '<%=nodeAccessToken%>';
        var userResps = '<%=userResps%>';
        var restdllPath = '<%=restdllPath%>';
        var nodeAPI = '<%=nodeAPI%>';
        var userroles = '<%=userroles%>';
        var redisutl = '<%= redisutl%>';
        var sid = '<%=sid%>';
        var utl = '<%=utl%>';//Session["utl"]
        var hasPageBuildAccess = '<%=hasPageBuildAccess%>';
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="right-section">
        <div class="col-lg-12 col-md-12 col-sm-12 col-sm-12" style="padding: 0px;">
            <div class="developerWorkBenchToolbarMain">
                <div class="col-sm-4 col-lg-4 col-md-4">
                    <div class="developerbreadcrumb-panel">
                        <div class="developerbreadcrumb icon-services left">
                            <span class="developerbreadcrumbTitle"></span>
                        </div>
                        <div class="developerSearch">
                        </div>
                    </div>
                </div>
                <div class="col-sm-8 col-lg-8 col-md-8">
                    <div class="developerWorkBenchToolbar">
                        <ul class="dwbiconsUl" id="Formstb">
                            <li><a href="javascript:void(0);" id="btn_opentstr" title="Create Form" class="dwbBtn">New</a></li>
                            <li class="dropdown"><a href="javascript:void(0)" id="customGroup1" title="Custom Actions" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true" aria-expanded="false">New <span class="icon-arrows-down"></span></a>
                                <ul class="dropdown-menu">
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="true"><span class="nav-label"><i class="fa fa-plus-square-o"></i>Add Field</span> <span class="icon-arrows-down"></span></a>
                                        <ul class="dropdown-menu">
                                            <div class="dropdown-menu-inner">
                                                <ul>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addshorttext" title="Short Text Field" class="dwbBtn"><i class="fa fa-text-width"></i>Short Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addsimpletext" title="Simple Text Field" class="dwbBtn"><i class="fa fa-file"></i>Simple Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addlargetext" title="Large Text Field" class="dwbBtn"><i class="fa fa-file-text-o"></i>Large Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addrichtext" title="Rich Text Field" class="dwbBtn"><i class="fa fa-file-text"></i>Rich Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_dropdwnlist" title="Drop Down" class="dwbBtn"><i class="fa fa-caret-square-o-down"></i>Drop Down</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addcheckbox" title="checkbox" class="dwbBtn"><i class="fa fa-check-square-o"></i>Check Box</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addchecklist" title="Check List" class="dwbBtn"><i class="fa fa-list-alt"></i>Check List</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addradiogroup" title="Radio Group" class="dwbBtn"><i class="fa fa-check-circle-o"></i>Radio Group</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addmultiselect" title="Multi Select" class="dwbBtn"><i class="fa fa-list-alt"></i>Multi Select</a></li>
                                                    <%-- <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addhtmltext" title="HTML Text Field" class="dwbBtn"><i class="fa fa-code"></i> HTML Text</a></li>--%>
                                                </ul>
                                                <ul>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addwholenumber" title="Whole Number Field" class="dwbBtn"><i class="fa fa-list-ol"></i>Whole Number</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_adddecimalnumb" title="Decimal Number Field" class="dwbBtn"><i class="fa fa-list-ol"></i>Decimal Number</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addrandomnumber" title="Random Number Field" class="dwbBtn"><i class="fa fa-random"></i>Random Number</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addmobilenumber" title="Mobile Number Field" class="dwbBtn"><i class="fa fa-mobile"></i>Mobile Number</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addphonenumber" title="Phone Number Field" class="dwbBtn"><i class="fa fa-phone"></i>Phone Number</a></li>

                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addpsqleditor" title="SQL Editor" class="dwbBtn"><i class="fa fa-code"></i>SQL Editor</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addpexpression" title="Expression Editor" class="dwbBtn"><i class="fa fa-code"></i>Expression Editor</a></li>
                                                </ul>
                                                <ul>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addcurrency" title="Currency Field" class="dwbBtn"><i class="fa fa-money"></i>Currency</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_adddate" title="Date Field" class="dwbBtn"><i class="fa fa-calendar"></i>Date</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addtime" title="Time Field" class="dwbBtn"><i class="fa fa-clock-o"></i>Time</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addtable" title="Table" class="dwbBtn"><i class="fa fa-table"></i>Table</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addimage" title="Image" class="dwbBtn"><i class="fa fa-image"></i>Image</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addautogenerate" title="Auto Generate Field" class="dwbBtn"><i class="fa fa-sort-numeric-asc"></i>Auto Generate</a></li>

                                                </ul>
                                                <ul>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addemail" title="Email Field" class="dwbBtn"><i class="fa fa-envelope-o"></i>Email</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addurl" title="URL Field" class="dwbBtn"><i class="fa fa-link"></i>URL</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addipaddress" title="IP Address Field" class="dwbBtn"><i class="fa fa-laptop"></i>IP Address</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addpassword" title="Password Field" class="dwbBtn"><i class="fa fa-key"></i>Password</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addpincode" title="Pincode Field" class="dwbBtn"><i class="fa fa-map-pin"></i>Pin Code</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addzipcode" title="Zip Code Field" class="dwbBtn"><i class="fa fa-map-pin"></i>Zip Code</a></li>
                                                </ul>
                                                <ul>
                                                </ul>


                                                <div class="clearfix"></div>
                                            </div>
                                        </ul>
                                    </li>

                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_adddc" title="Add Frames" class="dwbBtn"><i class="fa fa-columns"></i>Add Frames</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_genmap" title="Add GenMap" class="dwbBtn"><i class="fa fa-file-text"></i>Post to another form</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_fillg" title="Add Fill Grid" class="dwbBtn"><i class="fa fa-th-large"></i>Add Fill Grid</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_mdmaps" title="Add MdMap" class="dwbBtn"><i class="fa fa-file-text-o"></i>Update details to another form</a></li>
                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_actions" title="Scripts" class="dwbBtn"><i class="fa fa-tasks"></i>Scripts</a></li>
                                </ul>
                            </li>
                            <li><a href="javascript:void(0);" onclick="javascript:LoadPopPageDesign();" id="" title="Form Design" class="">Form Design</a></li>
                            <li><a href="javascript:void(0);" id="btn_callws" title="Save" class="dwbBtn">Save</a></li>
                            <li><a href="javascript:void(0);" id="PublishDesign" title="Publish" class="dwbBtn">Publish</a></li>
                            <li><a href="javascript:void(0)" id="btn_toolbar" title="ToolBar" data-pgtype="tstruct" class="dwbBtn btntoolbar">ToolBar</a></li>
                            <li><a href="javascript:void(0)" id="btn_deletesc" title="Delete" class="dwbBtn">Delete</a></li>
                            <li><a href="javascript:void(0);" id="btn_openproperty" title="Properties" class="dwbBtn">Properties </a></li>
                            <li><a href="javascript:void(0);" id="" title="changeLog" class="dwbBtn">Change log</a></li>



                        </ul>

                        <ul class="dwbiconsUl" id="ArrangeMtb">
                            <%--// Arrange Menu Page Buttons //--%>
                            <li><a href="javascript:void(0);" id="lnkChngIcon" title="Change Icon" class="dwbBtn">Change Icon</a></li>
                            <li><a href="javascript:void(0);" id="btnAdd" title="Add Folder" class="dwbBtn">Add Folder</a></li>
                            <li><a href="javascript:void(0);" id="btnDelete" title="Delete" class="dwbBtn">Delete</a></li>
                            <li><a href="javascript:void(0);" id="btnSave" title="Save" class="dwbBtn">Save</a></li>

                        </ul>

                        <ul class="dwbiconsUl" id="iviewListToolbar">
                            <li><a href="javascript:void(0);" id="btn_createreport" title="Create Report" class="dwbBtn">Create Report</a></li>
                            <li><a href="javascript:void(0);" id="reportBuilderBtn" title="Report Designer" class="dwbBtnReportBuilder" onclick="LoadIframeDwb('iviewBuilder.aspx?ivname=flist');">Report Builder</a></li>
                        </ul>                        

                        <ul class="dwbiconsUl" id="wbtb">
                            <li><a href="javascript:void(0);" id="btn_dwbwidget" title="Widget builder" class="dwbwidget" onclick=" LoadIframeDwb('widgetbuilder.aspx');"><i class="material-icons" style="font-size: initial;">widgets</i><span>Widget Builder</span></a></li>
                        </ul>

                        <ul class="dwbiconsUl" id="htmlPagesIvToolBar">
                            <li><a href="javascript:void(0);" title="New" onclick="$('#middle1')[0].contentWindow.callOpenAction('opentstruct','sect');">New</a></li>
                            <li><a href="javascript:void(0);" title="Remove" onclick="$('#middle1')[0].contentWindow.callDelete('sect','lview');">Remove</a></li>
                            <li><a href="javascript:void(0);" title="Search" onclick="$('#middle1')[0].contentWindow.$('#idsearch').addClass('fa-remove');$('#middle1')[0].contentWindow.$('#idsearch').removeClass('fa-search');$('#middle1')[0].contentWindow.$('.searchBoxChildContainer').removeClass('hide');$('#middle1')[0].contentWindow.$('#ivInSearchInput').focus();">Search</a></li>
                        </ul>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="right-iviewmain card panel-fisrt-partdwb" id="dvMiddle1">
                <iframe id="middle1" name="middle1" frameborder="0" scrolling="no" class="searchOpened"
                    allowtransparency="True" width="100%" height="100%"></iframe>
            </div>

            <div class="right-iviewmain card panel-second-partdwb" id="divaxpiframe" style="display: none;">
                <iframe id="axpiframe" name="axpiframe" frameborder="0" scrolling="no"
                    class="searchOpened" allowtransparency="True" width="100%" height="100%"
                    style="display: none;"></iframe>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="../WebService.asmx" />
            <asp:ServiceReference Path="../CustomWebService.asmx" />
        </Services>
    </asp:ScriptManager>
</asp:Content>

