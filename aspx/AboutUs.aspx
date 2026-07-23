<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AboutUs.aspx.cs" Inherits="aspx_AboutUs" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="About Us" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>About Axpert</title>
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/thirdparty/jquery/3.1.1/jquery.min.js" type="text/javascript"></script>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
    <script src="../Js/thirdparty/bootstrap/3.3.6/bootstrap.min.js"></script>
    <script src="../Js/thirdparty/jquery-ui/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Js/noConflict.min.js?v=1" type="text/javascript"></script>
    <link href="../Css/thirdparty/jquery-ui/1.12.1/jquery-ui.min.css" rel="stylesheet" />
    <link href="../Css/globalStyles.min.css?v=36" rel="stylesheet" />
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
    <script src="../Js/common.min.js?v=165"></script>
    <link href="../Css/aboutus.min.css?v=3" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            modalHeader = eval(callParent("divModalHeader", "id") + ".getElementById('divModalHeader')");
            modalHeader.innerText = eval(callParent('lcm[387]'));
            $("#btnClose").prop("title", eval(callParent('lcm[249]')));
            $(this).parent("#btnClose").focus();
        });

        $(document).on("keydown", function (e) {
            if (e.key !== "Escape")
                return;
            e.preventDefault();
            parent.closeModalDialog();
        });
    </script>
    <style>
        .btextDir-rtl .modal-header button#btnModalClose {
            float: left !important;
        }

        html,
        body {
            margin: 0;
            padding: 0;
            overflow: hidden;
            font-family: "Segoe UI", Roboto, Arial, sans-serif;
            font-size: 14px;
            line-height: 1.5;
            color: #333;
        }

        .versionInfo {
            padding: 15px 20px;
        }

            .versionInfo dl {
                margin: 0;
            }

            .versionInfo dt,
            .versionInfo dd {
                display: inline;
                margin: 0;
                padding: 0;
                vertical-align: top;
                font-size: 14px;
                line-height: 1.6;
            }

            .versionInfo dt {
                font-weight: 600;
                margin-right: 6px;
            }

            .versionInfo dd {
                font-weight: 400;
                word-break: break-word;
                overflow-wrap: break-word;
            }

                .versionInfo dd::after {
                    content: "";
                    display: block;
                    margin-bottom: 6px;
                }

        .castLbl {
            font-weight: 600;
        }

        .fontclass {
            font-weight: 400;
        }
    </style>

</head>
<body class="btextDir-<%=direction%>" dir="<%=direction%>" id="dvaboutus">
    <div class="container versionInfo">
        <dl>
            <dt>
                <asp:Label ID="lblVer" runat="server" meta:resourcekey="lblVer" CssClass="castLbl">
                    Version:
                </asp:Label>
            </dt>
            <dd>
                <asp:Label ID="Labelvers" runat="server" />
                <asp:Label ID="lblVersion" runat="server" CssClass="fontclass" />
                <asp:Label ID="lblSubversion" runat="server" CssClass="fontclass" />
            </dd>
            <dt id="divproject" runat="server">
                <asp:Label ID="lblproj" runat="server" meta:resourcekey="lblproj" CssClass="castLbl">
                    Project:
                </asp:Label>
            </dt>
            <dd>
                <asp:Label ID="divprojcontent" runat="server" CssClass="fontclass" />
            </dd>

            <dt>
                <asp:Label ID="lblappsrvr" runat="server" meta:resourcekey="lblappsrvr" CssClass="castLbl">
                    Application Server:
                </asp:Label>
            </dt>
            <dd>
                <asp:Label ID="Label13" runat="server" Text="IIS" CssClass="fontclass" />
            </dd>

            <dt>
                <asp:Label ID="lbldb" runat="server" meta:resourcekey="lbldb" CssClass="castLbl">
                    Database:
                </asp:Label>
            </dt>
            <dd>
                <asp:Label ID="divdbconent" runat="server" CssClass="fontclass" />
            </dd>

            <dt>
                <asp:Label ID="lbldc" runat="server" meta:resourcekey="lbldc" CssClass="castLbl">
                    Data Caching:
                </asp:Label>
            </dt>
            <dd>
                <asp:Label ID="divdatacontent" runat="server" CssClass="fontclass" />
            </dd>

            <dt id="divRelDate" runat="server" visible="false">
                <asp:Label ID="lblrd" runat="server" meta:resourcekey="lblrd" CssClass="castLbl">
                    Release Date:
                </asp:Label>
            </dt>
            <dd runat="server" visible="false">
                <asp:Label ID="lblRelDate" runat="server" CssClass="fontclass" />
            </dd>

            <dt id="divDesc" runat="server" visible="false">
                <asp:Label ID="lbldes" runat="server" meta:resourcekey="lbldes" CssClass="castLbl">
                    Description:
                </asp:Label>
            </dt>
            <dd runat="server" visible="false">
                <div id="divDescContent" runat="server" class="fontclass"></div>
            </dd>

            <dt id="divFeat" runat="server" visible="false">
                <asp:Label ID="lblfeat" runat="server" meta:resourcekey="lblfeat" CssClass="castLbl">
                    Features:
                </asp:Label>
            </dt>
            <dd runat="server" visible="false">
                <div id="divFeaturesContent" runat="server" class="fontclass"></div>
            </dd>

            <dt id="divEnhan" runat="server" visible="false">
                <asp:Label ID="lblenhan" runat="server" meta:resourcekey="lblenhan" CssClass="castLbl">
                    Enhancements:
                </asp:Label>
            </dt>
            <dd runat="server" visible="false">
                <div id="divEnhanContent" runat="server" class="fontclass"></div>
            </dd>
        </dl>
        <div class="text-right">
            <button type="button" id="btnClose" class="btn btn-default" onclick="parent.closeModalDialog()">
                Close
            </button>
        </div>
    </div>
</body>
</html>
