<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Wizardsetting.aspx.cs" Inherits="Wizardsetting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Wizard Setting</title>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
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
    <link href="../Css/Icons/icon.css" rel="stylesheet" />
    <link href="../Css/developerWorkBench.min.css?v=14" rel="stylesheet" />
    <link href="../Css/thirdparty/font-awesome/4.6.3/css/font-awesome.min.css" rel="stylesheet" />
    <script src="../Js/Jquery-2.2.2.min.js" type="text/javascript"></script>
    <script src="../Js/thirdparty/bootstrap/3.3.6/bootstrap.min.js" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js"></script>
    <script src="../Js/script.js"></script>
    <script src="../Js/DND.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="../Js/DND.js" />
            </Scripts>
        </asp:ScriptManager>
        <div class="wizardMain-container">
            <div class="container1">
                <div class="col-sm-12 col-md-12">
                    <div class="developerWorkBenchToolbar">
                        <ul class="dwbiconsUl" id="Formstb">
                            <%--<li class="dropdown"><a href="javascript:void(0)" id="customGroup1" title="Custom Actions"
                                class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown"
                                data-close-others="true" aria-expanded="false">New <span
                                    class="icon-arrows-down"></span></a>
                                <ul class="dropdown-menu">
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button"
                                            aria-haspopup="true" aria-expanded="true"><span class="nav-label">Add
                                            Field</span> <span class="icon-arrows-down"></span></a>
                                        <ul class="dropdown-menu">
                                            <div class="dropdown-menu-inner">
                                                <ul>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addshorttext" title="Short Text Field"
                                                        class="dwbBtn">Short Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addsimpletext" title="Simple Text Field"
                                                        class="dwbBtn">Simple Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addlargetext" title="Large Text Field"
                                                        class="dwbBtn">Large Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addrichtext" title="Rich Text Field" class="dwbBtn">Rich
                                                        Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addhtmltext" title="HTML Text Field" class="dwbBtn">HTML
                                                        Text</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addwholenumber" title="Whole Number Field"
                                                        class="dwbBtn">Whole Number</a></li>
                                                </ul>
                                                <ul>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_adddecimalnumb" title="Decimal Number Field"
                                                        class="dwbBtn">Decimal Number</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addcurrency" title="Currency Field"
                                                        class="dwbBtn">Currency</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_adddate"
                                                        title="Date Field" class="dwbBtn">Date</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addtime"
                                                        title="Time Field" class="dwbBtn">Time</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addautogenerate" title="Auto Generate Field"
                                                        class="dwbBtn">Auto Generate</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addrandomnumber" title="Random Number Field"
                                                        class="dwbBtn">Random Number</a></li>
                                                </ul>
                                                <ul>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addemail"
                                                        title="Email Field" class="dwbBtn">Email</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addurl"
                                                        title="URL Field" class="dwbBtn">URL</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addipaddress" title="IP Address Field" class="dwbBtn">IP
                                                        Address</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addpassword" title="Password Field"
                                                        class="dwbBtn">Password</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addmobilenumber" title="Mobile Number Field"
                                                        class="dwbBtn">Mobile Number</a></li>

                                                    <li class="liTaskItems"><a href="javascript:void(0)"
                                                        id="btn_addphonenumber" title="Phone Number Field"
                                                        class="dwbBtn">Phone Number</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addpincode"
                                                        title="Pincode Field" class="dwbBtn">Pin Code</a></li>
                                                    <li class="liTaskItems"><a href="javascript:void(0)" id="btn_addzipcode"
                                                        title="Zip Code Field" class="dwbBtn">Zip Code</a></li>
                                                </ul>
                                                <div class="clearfix"></div>
                                            </div>
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
                            </li>--%>
                            <li>
                                <a href="javascript:void(0);" id="" data-toggle="modal" data-target="#exampleModal"
                                    title="Form Design" class="btn btn-secondary dropdown-toggle">New</a>
                            </li>
                            <%--<li><a href="javascript:void(0);" onclick="javascript:LoadPopPageDesign();" id=""
                                title="Form Design" class="">Form Design</a></li>--%>
                            <li>
                                <button type="button" class="btn btn-secondary dropdown-toggle" style="background-color: white;" data-toggle="modal"
                                    data-target="#myModalPublish" data-whatever="@mdo">
                                    Publish</button>
                                </li>
                                <li>
                                    <asp:Button ID="btnRemoveall" runat="server" CssClass="btn btn-secondary dropdown-toggle" BackColor="White" Text="Remove All" />
                                </li>
                                <li class="dropdown" style="display: none;">
                                    <button type="button" class="btn btn-secondary dropdown-toggle" style="background-color: white;" data-toggle="modal"
                                        data-target="#modalremove" data-whatever="@mdo">
                                        Remove Wizard</button>
                                </li>
                                <li>
                                    <button type="button" class="btn btn-secondary dropdown-toggle" style="background-color: white;" data-toggle="modal"
                                        data-target="#myModal2" data-whatever="@mdo">
                                        Configure Parameters</button>
                                </li>
                                <%--<li class="createWizard" data-toggle="modal" data-target="#exampleModal">
                                <span class="icon-arrows-plus"></span>
                            </li>--%>
                        </ul>

                        <ul class="dwbiconsUl" id="ArrangeMtb" style="display: none;">

                            <li><a href="javascript:void(0);" id="lnkChngIcon" title="Change Icon" class="dwbBtn">Change
                                Icon</a></li>
                            <li><a href="javascript:void(0);" id="btnAdd" title="Add Folder" class="dwbBtn">Add Folder</a>
                            </li>
                            <li><a href="javascript:void(0);" id="btnDelete" title="Delete" class="dwbBtn">Delete</a></li>
                            <li><a href="javascript:void(0);" id="btnSave1" title="Save" class="dwbBtn">Save</a></li>

                        </ul>
                    </div>
                </div>
                <div class="col-sm-3 col-md-3">
                    <div class="wizardLeftMenu">
                        <%--<ul id="lstWizardLeftMenu">
                            <li>
                                <div itemid="itm-11" class="btn btn-default box-item" style="width: 100%;">User Name</div>
                                <div class="btn-group dropleft wizard-drop">
                                    <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown"
                                        aria-haspopup="true" aria-expanded="false">
                                        <i class="fa fa-ellipsis-h"></i>
                                    </button>
                                    <div class="dropdown-menu">
                                        <a class="dropdown-item" href="#"><i class="fa fa-sticky-note-o"></i>Invoices</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-users"></i>Customers</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-user"></i>Sales Person</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-tree"></i>Branch</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-television"></i>Channel</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-line-chart"></i>Profitability</a>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div itemid="itm-12" class="btn btn-default box-item" style="width: 100%;">User Name-2</div>
                                <div class="btn-group dropleft wizard-drop">
                                    <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown"
                                        aria-haspopup="true" aria-expanded="false">
                                        <i class="fa fa-ellipsis-h"></i>
                                    </button>
                                    <div class="dropdown-menu">
                                        <a class="dropdown-item" href="#"><i class="fa fa-sticky-note-o"></i>Invoices</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-users"></i>Customers</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-user"></i>Sales Person</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-tree"></i>Branch</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-television"></i>Channel</a>
                                        <a class="dropdown-item" href="#"><i class="fa fa-line-chart"></i>Profitability</a>
                                    </div>
                                </div>
                            </li>
                            <div itemid="itm-13" class="btn btn-default box-item" style="width: 100%;">User Name-3</div>
                            <div itemid="itm-14" class="btn btn-default box-item" style="width: 100%;">User Name-4</div>
                            <!-- <li>
                            <a href="">Form 1</a>
                            <div class="btn-group dropleft wizard-drop">
                                <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown"
                                    aria-haspopup="true" aria-expanded="false">
                                    <i class="fa fa-ellipsis-h"></i>
                                </button>
                                <div class="dropdown-menu">
                                    <a class="dropdown-item" href="#"><i class="fa fa-sticky-note-o"></i>Invoices</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-users"></i>Customers</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-user"></i>Sales Person</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-tree"></i>Branch</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-television"></i>Channel</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-line-chart"></i>Profitability</a>
                                </div>
                            </div>
                        </li>
                        <li>
                            <a href="">Iview 1</a>
                            <div class="btn-group dropleft wizard-drop">
                                <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown"
                                    aria-haspopup="true" aria-expanded="false">
                                    <i class="fa fa-ellipsis-h"></i>
                                </button>
                                <div class="dropdown-menu">
                                    <a class="dropdown-item" href="#"><i class="fa fa-sticky-note-o"></i>Invoices</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-users"></i>Customers</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-user"></i>Sales Person</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-tree"></i>Branch</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-television"></i>Channel</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-line-chart"></i>Profitability</a>
                                </div>
                            </div>
                        </li>
                        <li>
                            <a href="">Form 2</a>
                            <div class="btn-group dropleft wizard-drop">
                                <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown"
                                    aria-haspopup="true" aria-expanded="false">
                                    <i class="fa fa-ellipsis-h"></i>
                                </button>
                                <div class="dropdown-menu">
                                    <a class="dropdown-item" href="#"><i class="fa fa-sticky-note-o"></i>Invoices</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-users"></i>Customers</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-user"></i>Sales Person</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-tree"></i>Branch</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-television"></i>Channel</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-line-chart"></i>Profitability</a>
                                </div>
                            </div>
                        </li>
                        <li>
                            <a href="">Payment gateway</a>
                            <div class="btn-group dropleft wizard-drop">
                                <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown"
                                    aria-haspopup="true" aria-expanded="false">
                                    <i class="fa fa-ellipsis-h"></i>
                                </button>
                                <div class="dropdown-menu">
                                    <a class="dropdown-item" href="#"><i class="fa fa-sticky-note-o"></i>Invoices</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-users"></i>Customers</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-user"></i>Sales Person</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-tree"></i>Branch</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-television"></i>Channel</a>
                                    <a class="dropdown-item" href="#"><i class="fa fa-line-chart"></i>Profitability</a>
                                </div>
                            </div>
                        </li> -->
                        </ul>--%>
                        <div id="container5" class="panel-body box-container">
                            <ul id="lstWizardLeftMenu" runat="server">
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="col-sm-9 col-md-9">
                    <div class="wizardMid" id="divmiddle" runat="server">
                        <%-- <iframe id="wizMiddle1" name="wizMiddle1" runat="server"
                            class="card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened" src="" style="padding: 0px;"
                            frameborder="0" scrolling="no" allowtransparency="True" width="100%"></iframe>--%>
                    </div>
                </div>
                <%--<div class="col-sm-3 col-md-3" style="padding-top: 10px;">
                    <div class="wizardRight">
                        <h2 class="property">property
                        </h2>
                        <div class="card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened">
                            <h2 class="property">Property
                            </h2>
                            <iframe id="wizMiddle2" name="wizMiddle2" src="" style="padding: 0px;"
                            frameborder="0" scrolling="no" allowtransparency="True" width="100%"></iframe>
                        </div>
                    </div>
                </div>--%>
            </div>
            <!-- Modal -->
            <div class="modal fade" id="myModal22" data-backdrop="static">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Modal Header</h4>
                        </div>
                        <div class="modal-body">
                            <p>
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal
    fade"
                id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
                aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">New Wizard</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group" style="display: none;">
                                <label for="recipient-name" class="col-form-label fw-boldest">
                                    Edit:</label>
                                <asp:TextBox ID="tbTitle" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="tbType" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="tbst" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:TextBox ID="tbxEdit" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="recipient-name" class="col-form-label fw-boldest">
                                    Title:</label>
                                <asp:TextBox ID="tbxTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="message-text" class="col-form-label fw-boldest">
                                    Type:</label>
                                <asp:DropDownList ID="cboType" runat="server" CssClass="form-control" AutoPostBack="true" ValidationGroup="test1">
                                </asp:DropDownList>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="form-group" id="divcbo" runat="server">
                                        <label for="message-text" class="col-form-label fw-boldest">
                                            Details:</label>
                                        <asp:DropDownList ID="cboTstruct" runat="server" CssClass="form-control" ValidationGroup="test1">
                                        </asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="form-group" id="divupload" style="display: none;">
                                        <label for="message-text" class="col-form-label fw-boldest">
                                            Custom File :</label>
                                        <asp:FileUpload ID="fileUploadCustom" runat="server" />
                                        <br />
                                        <br />
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">
                                                Close</button>
                                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Upload and Finish" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnUpload" EventName="Click" />--%>
                                    <asp:PostBackTrigger ControlID="btnUpload" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>
                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>--%>
                        <div class="modal-footer" id="mainbtn">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">
                                Close</button>
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Add New" />
                        </div>
                        <%--</ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>


            <div class="modal fade"
                id="modalremove" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
                aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="H1">Remove Wizard</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label for="message-text" class="col-form-label fw-boldest">
                                    Tab Name:</label>
                                <asp:DropDownList ID="cboTabName" runat="server" CssClass="form-control" ValidationGroup="test1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">
                                Close</button>
                            <%--<button type="button" id="btnAdd" class="btn btn-primary">
                        Add New</button>--%>
                            <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-primary" Text="Remove Tab" />
                        </div>
                    </div>
                </div>
            </div>



            <div class="modal" id="myModal">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Confirmaation</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        </div>
                        <div class="container"></div>
                        <div class="modal-body">
                            <span id="spCont"></span>
                            <div style="display: none;">
                                <span id="sptitle"></span>
                                <span id="sptype"></span>
                                <span id="spst"></span>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <a class="btn btn-secondary" onclick="showeditModal1();">Ok</a>
                            <a href="#" data-dismiss="modal" class="btn btn-secondary">Close</a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" id="myModalInactive">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Confirmaation</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        </div>
                        <div class="container"></div>
                        <div class="modal-body">
                            <span id="spContInc"></span>
                            <div style="display: none;">
                                <span id="sptitleInc"></span>
                                <asp:TextBox ID="tbxTitleInc" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnInactive" runat="server" CssClass="btn btn-primary" Text="Active/Inactive" />
                            <a href="#" data-dismiss="modal" class="btn btn-secondary">Close</a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal" id="myModalDelete">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Confirmaation</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        </div>
                        <div class="container"></div>
                        <div class="modal-body">
                            <span id="spContDel"></span>
                            <div style="display: none;">
                                <span id="sptitleDel"></span>
                                <asp:TextBox ID="tbxTitleDel" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnDeleteTab" runat="server" CssClass="btn btn-primary" Text="Delete Tab" />
                            <a href="#" data-dismiss="modal" class="btn btn-secondary">Close</a>
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
                                <asp:TextBox ID="tbxPageTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="recipient-name" class="col-form-label fw-boldest">
                                    Description:</label>
                                <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnPublish" runat="server" CssClass="btn btn-secondary dropdown-toggle" BackColor="White" Text="Publish" />
                            <a href="#" data-dismiss="modal" class="btn btn-secondary dropdown-toggle">Close</a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="myModal2" data-backdrop="static">
                <div class="modal-dialog" style="width: 80%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Parameters Configuration</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        </div>
                        <div class="container"></div>
                        <div class="modal-body" style="width: 100%;">
                            <div class="row" style="width: 100%;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-xs-12 col-lg-3 col-md-3" style="z-index: 1;">
                                            <div class="form-group">
                                                <label for="message-text" class="col-form-label fw-boldest">
                                                    Pages:</label>
                                                <asp:DropDownList ID="cboCont1" AutoPostBack="true" runat="server" CssClass="form-control" ValidationGroup="test1">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h1 class="panel-title">Container 1
                                                    </h1>
                                                </div>
                                                <div id="container1" class="panel-body box-container">
                                                    <div id="divCont1" runat="server">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <div class="col-xs-12 col-lg-3 col-md-3" style="z-index: 0; padding-top: 75px;">

                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <h1 class="panel-title">Container 2
                                            </h1>
                                        </div>
                                        <div id="container2" class="panel-body box-container">
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-lg-3 col-md-3" style="padding-top: 75px;">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <h1 class="panel-title">Container 3
                                            </h1>
                                        </div>
                                        <div id="container3" class="panel-body box-container">
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <div class="col-xs-12 col-lg-3 col-md-3">
                                            <div class="form-group">
                                                <label for="message-text" class="col-form-label fw-boldest">
                                                    Pages:</label>
                                                <asp:DropDownList ID="cboCont2" AutoPostBack="true" runat="server" CssClass="form-control" ValidationGroup="test1">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h1 class="panel-title">Container 4
                                                    </h1>
                                                </div>

                                                <div id="container4" class="panel-body box-container ui-droppable">
                                                    <div id="divCont4" runat="server">
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <a href="#" data-dismiss="modal" class="btn">Close</a>
                            <a href="#" class="btn btn-primary">Save changes</a>
                        </div>
                    </div>
                </div>
            </div>


        </div>

        <script>

            $(document).ready(function () {

            });

            function filltypesofvalues(lsType) {
                if (lsType == "TSTRUCT") {
                    <%= fillTstruct1()%>
                }
                else if (lsType == "PAYMENT GATEWAY") {
                    <%= fillPaymentGateways1()%>
                }
                else if (lsType == "IVIEW") {
                    <%= fillIviews1()%>
                }
            }

            function showIncModal(lsTitle) {
                $("#spContInc").text("Do you want active or inactive the tab");
                $("#sptitleInc").text(lsTitle);
                $("#tbxTitleInc").val(lsTitle);
                $("#myModalInactive").modal();
            }

            function showDelModal(lsTitle) {
                $("#spContDel").text("Do you want delete the tab");
                $("#sptitleDel").text(lsTitle);
                $("#tbxTitleDel").val(lsTitle);
                $("#myModalDelete").modal();
            }

            function showeditModal(lsEdit, lsTitle, lsType, lsVal) {
                $("#tbxEdit").val(lsEdit);
                $("#tbxTitle").val(lsTitle);
                $("#cboType").val(lsType);
                $("#cboType").change();
                $("#cboTstruct").val(lsVal);
                $("#tbTitle").val(lsTitle);
                $("#tbType").val(lsType);
                $("#tbst").val(lsVal);
                $("#spCont").text("Do you want edit the tab");
                $("#sptitle").text(lsTitle);
                $("#sptype").text(lsType);
                $("#spst").text(lsVal);
                $("#myModal").modal();
                //$("[data-dismiss=modal]").trigger({ type: "click" });
                //$("#exampleModal").modal();
                //showeditModal1(lsEdit, lsTitle, lsType, lsVal);
            }

            function showeditModal1() {
                $("[data-dismiss=modal]").trigger({ type: "click" });
                //$("#myModal").hide();
                $("#tbxEdit").val('Y');
                $("#tbxTitle").val($("#sptitle").text());
                $("#cboType").val($("#sptype").text());
                $("#cboType").change();
                $("#cboTstruct").val($("#spst").text());
                $("#exampleModal").modal();
            }

            function showdiv(id, cnt) {
                var id1 = document.getElementById(id);
                for (i = 1; i <= cnt; i++) {
                    var idd = 'divmd' + i;
                    var id2 = document.getElementById(idd);
                    if (id == idd) {
                        id2.style.display = 'block';
                    }
                    else {
                        id2.style.display = 'none';
                    }
                }
            }
            function showupload() {
                var id = document.getElementById('divupload');
                var id1 = document.getElementById('mainbtn');
                id.style.display = 'block';
                id1.style.display = 'none';
            }
            function hideupload() {
                var id = document.getElementById('divupload');
                var id1 = document.getElementById('mainbtn');
                id1.style.display = 'block';
                id.style.display = 'none';
            }

        </script>
    </form>
</body>
</html>
