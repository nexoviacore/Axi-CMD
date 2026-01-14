<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppDownload.aspx.cs" Inherits="aspx_AppDownload" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="App Download" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Application Download</title>
    <asp:PlaceHolder runat="server">
        <%:Styles.Render(direction == "ltr" ? "~/UI/axpertUI/ltrBundleCss" : "~/UI/axpertUI/rtlBundleCss") %>
    </asp:PlaceHolder>
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
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <asp:PlaceHolder runat="server">
        <%:Scripts.Render("~/UI/axpertUI/bundleJs") %>
    </asp:PlaceHolder>
    <script src="../Js/alerts.min.js?v=32" type="text/javascript"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2" type="text/javascript"></script>

    <script src="../Js/AppDownload.min.js?v=2" type="text/javascript"></script>
    <script src="../Js/common.min.js?v=158" type="text/javascript"></script>
</head>
<body dir="<%=direction%>">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnSelExe" Value="" runat="server" />
        <div class="col-sm-12 mainDownloadWrapper" style="overflow: auto">

            <div id="wrapper" class="col-lg-12 col-xs-12 col-sm-12 col-md-12">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="panel1" runat="server">
                            <div id="downloadattch" class="col-lg-9 col-xs-12 col-md-12 col-sm-12" runat="server" style="height: auto; background-color: #eeeeee;">
                            </div>
                        </div>
                        <div id="download" runat="server" style="display: none">
                            <div id="starter" class="col-lg-9 col-xs-12 col-md-12 col-sm-12" style="height: auto; background-color: #eeeeee; text-align: center;">
                                <h2>
                                    <asp:Label ID="lbldwnld" runat="server" meta:resourcekey="lbldwnld">Thank you for downloading..</asp:Label></h2>
                                <h5 id="timercount">
                                    <asp:Label ID="lbldwnd" runat="server" meta:resourcekey="lbldwnd">If your download does not start yet,<a href="javascript:void(0)" onclick="OpenDownload();"><b>Click Here</b></a> to try again. </asp:Label></h5>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" Value="" ID="hiddenfilenames" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="btndwnload" runat="server" CssClass="hide" OnClick="download_click" />
                <br />
                <p class="col-lg-9 col-xs-12 col-md-12 col-sm-12" style="margin-top: 30px; color: #505050; font-size: 18px;">&nbsp;</p>

                <div class="container col-lg-9 col-xs-12 col-md-12 col-sm-12" style="margin-top: 20px; display: none">

                    <ul class="toggleUl">
                        <li>
                            <input type="checkbox" checked>
                            <i></i>
                            <h2>
                                <asp:Label ID="lbldtls" runat="server" meta:resourcekey="lbldtls">Details</asp:Label></h2>
                            <div class="togglePanel">
                                <div runat="server" id="FileDetails"></div>
                            </div>
                        </li>
                        <li>
                            <input type="checkbox" checked>
                            <i></i>
                            <h2>
                                <asp:Label ID="lblsysrqr" runat="server" meta:resourcekey="lblsysrqr">System Requirements</asp:Label></h2>
                            <div class="togglePanel">
                                <h3 class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:Label ID="lblclntreq" runat="server" meta:resourcekey="lblclntreq">Axpert DeskTop Client Requirements </asp:Label></h3>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblproc" runat="server" meta:resourcekey="lblproc"><b>Processor:</b>1.6GHz or faster processor</asp:Label>
                                    </p>
                                </div>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblmemory" runat="server" meta:resourcekey="lblmemory"><b>Memory:</b>2GB RAM</asp:Label>
                                    </p>
                                </div>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblharddisk" runat="server" meta:resourcekey="lblharddisk"><b>Hard Disk Space:</b>100MB</asp:Label>
                                    </p>
                                </div>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="Label3" runat="server" meta:resourcekey="lbldisplay"><b>Display:</b>1366 x 768 or higher</asp:Label>
                                    </p>
                                </div>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblos" runat="server" meta:resourcekey="lblos"><b>Operating System:</b>Microsoft Windows XP with SP2/SP3Microsoft Windows 7 ( Professional or better)</asp:Label>
                                    </p>
                                </div>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblos1" runat="server" meta:resourcekey="lblos1"><b>Other Software:</b>Oracle/MS SQL DB client</asp:Label>
                                    </p>
                                </div>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblntwrk" runat="server" meta:resourcekey="lblntwrk"><b>Other:</b>Network connection to DB</asp:Label>
                                    </p>
                                </div>
                                <br />

                                <h3 class="col-lg-12 col-sm-12 col-xs-12 col-md-12">
                                    <asp:Label ID="lblserverreq" runat="server" meta:resourcekey="lblserverreq">Server Requirements</asp:Label>
                                </h3>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblorcldb" runat="server" meta:resourcekey="lblorcldb"><b>Oracle Database:</b> 10g R2 or greater</asp:Label>
                                    </p>
                                </div>
                                <div class="col-lg-6 col-sm-6 col-md-6 col-xs-12">
                                    <p>
                                        <asp:Label ID="lblorcldb1" runat="server" meta:resourcekey="lblorcldb1"><b>MS SQL Server:</b> 2008 R2 or greater</asp:Label>
                                    </p>
                                </div>
                            </div>
                        </li>
                        <li>
                            <input type="checkbox" checked>
                            <i></i>
                            <h2>
                                <asp:Label ID="lblinstall" runat="server" meta:resourcekey="lblinstall">Installation</asp:Label></h2>
                            <div class="togglePanel">
                                <h3>
                                    <asp:Label ID="lblinstalldwn" runat="server" meta:resourcekey="lblinstalldwn">To Install This Download</asp:Label>
                                </h3>
                                <p>
                                    <asp:Label ID="lblinstalldwnld" runat="server" meta:resourcekey="lblinstalldwnld">1.download the Exe clicking the Download button above and saving the file to your hard disk.</asp:Label>
                                </p>
                                <br />
                                <p>
                                    <asp:Label ID="lbldoubleclick" runat="server" meta:resourcekey="lbldoubleclick">2.Double-click the Filename.exe program file on your hard disk to start the setup program.</asp:Label>
                                </p>
                                <br />
                                <p>
                                    <asp:Label ID="lblinstruction" runat="server" meta:resourcekey="lblinstruction">3.Follow the instructions on the screen to complete the installation.</asp:Label>
                                </p>
                                <br />
                                <br />
                                <h3>
                                    <asp:Label ID="lblremovedwnld" runat="server" meta:resourcekey="lblremovedwnld">To Remove This Download </asp:Label></h3>
                                <p>
                                    <asp:Label ID="lblcontrolpanel" runat="server" meta:resourcekey="lblcontrolpanel">1.On the Windows Start menu, click Control Panel.</asp:Label>
                                </p>
                                <br />
                                <p>
                                    <asp:Label ID="lblremoveprog" runat="server" meta:resourcekey="lblremoveprog">2.Select Add/Remove Programs.</asp:Label>
                                </p>
                                <br />
                                <p>
                                    <asp:Label ID="lblinstalledprog" runat="server" meta:resourcekey="lblinstalledprog">3.In the list of currently installed programs, select FileName and then click Remove or Add/Remove. If a dialog box appears, follow the instructions to remove the program.</asp:Label>
                                </p>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
