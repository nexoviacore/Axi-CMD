<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WizardPage.aspx.cs" Inherits="aspx_WizardPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="Import" />
    <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
    <meta name="author" content="Agile Labs" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />



    <title>Wizard Page</title>
    <link href="../Css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="../Css/animate.min.css" rel="stylesheet" />
    <link href="../Css/import.min.css?v=31" rel="stylesheet" />--%>
    <link rel="stylesheet" href="../Css/wizardComp.min.css?v=19">
    <link href="../ThirdParty/jquery-confirm-master/jquery-confirm.min.css?v=1" rel="stylesheet" />
    <link href="../Css/Icons/icon.css" rel="stylesheet" />
    <link href="../Css/thirdparty/font-awesome/4.6.3/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../ThirdParty/Linearicons/Font/library/linearIcons.css" rel="stylesheet" />
    <%--<link href="../App_Themes/Gray/Stylesheet.min.css?v=23" rel="stylesheet" />--%>
    <%--<link id="themecss" type="text/css" href="" rel="stylesheet" />--%>
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
    <link href="../Css/import.min.css?v=32" rel="stylesheet" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script src="../Js/Jquery-2.2.2.min.js" type="text/javascript"></script>
    <script src="../AssetsNew/js/bootstrap.min.js"></script>
    <script src="../Js/wizard.min.js?v=10"></script>
    <script src="../Js/common.min.js?v=158"></script>
    <script src="../Js/wizardComp.min.js?v=10" type="text/javascript"></script>
    <script src="../Js/helper.min.js?v=172" type="text/javascript"></script>
    <script src="../Js/alerts.min.js?v=32"></script>
    <script src="../ThirdParty/jquery-confirm-master/jquery-confirm.min.js?v=2"></script>
    <link href="../newPopups/Remodal/remodal-default-theme.min.css?v=2" rel="stylesheet" />
    <link href="../newPopups/Remodal/remodal.min.css?v=3" rel="stylesheet" />
    <script src="../newPopups/Remodal/remodal.min.js"></script>
    <script src="../newPopups/axpertPopup.min.js?v=45"></script>
    <script src="../Js/multiselect.min.js" type="text/javascript"></script>
    <script src="../Js/bootstrap-select.min.js" type="text/javascript"></script>
    <script>
        var wizardSetObj = "";
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server">
        </asp:ScriptManager>
        <div id="wizardWrappper">
            <!-- Widget Header menus - begins -->
            <div id="wizardHeader" runat="server">
               <%-- <div class="wizard-progress">
                    <div data-objtype="WizardDataSetting" data-id="0" data-target="Wizardset1" class="step classic active">
                        <a title="Introduction" class="stepName" href="javascript:void(0)">Introduction</a><div title="Introduction" class="node"></div>
                    </div>

                    <div data-objtype="WizardDataSetting" data-id="1" data-target="Wizardset2" class="step classic ">
                        <a title="Responsive" class="stepName" href="javascript:void(0)">Responsive</a><div title="Responsive" class="node"></div>
                    </div>

                    <div data-objtype="WizardDataSetting" data-id="2" data-target="Wizardset3" class="step classic ">
                        <a title="Payment" class="stepName" href="javascript:void(0)">Payment</a><div title="Payment" class="node"></div>
                    </div>

                    <div data-objtype="WizardDataSetting" data-id="3" data-target="Wizardset4" class="step classic">
                        <a title="Not a carousel" class="stepName" href="javascript:void(0)">Not a carousel</a><div title="Not a carousel" class="node"></div>
                    </div>
                </div>--%>
            </div>
            <!-- Widget Header menus - end -->

            <div id="wizardBodyContent" runat="server">
                <%--<div class="wizardContainer animated fadeIn" id="Wizardset1">
                    <section class="form-group col-md-12">
                        <iframe src="tstruct.aspx?transid=mcn" title="Wizards Dashboard" class="card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened" style="padding: 0px;" frameborder="0" scrolling="no" allowtransparency="True" width="100%" height="300px"></iframe>
                    </section>
                </div>
                <div class="wizardContainer animated fadeIn" id="Wizardset2" style="display: none;">
                    <section class="form-group col-md-12">
                        <iframe src="iview.aspx?ivname=axemllog" title="Wizards Dashboard" class="card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened" style="padding: 0px;" frameborder="0" scrolling="no" allowtransparency="True" width="100%" height="300px"></iframe>
                    </section>
                </div>
                <div class="wizardContainer animated fadeIn" id="Wizardset3" style="display: none;">
                    <section class="form-group col-md-12">
                        <iframe src="ccavenue/dataFrom.aspx" title="Wizards Dashboard" class="card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened" style="padding: 0px;" frameborder="0" scrolling="no" allowtransparency="True" width="100%" height="300px"></iframe>
                    </section>
                </div>
                <div class="wizardContainer animated fadeIn" id="Wizardset4" style="display: none;">
                    <section class="form-group col-md-12">
                        <iframe src="../downloads/20201013143017-email-confirmation.html" title="Wizards Dashboard" class="card col-xs-12 col-sm-12 col-md-12 col-lg-12 searchOpened" style="padding: 0px;" frameborder="0" scrolling="no" allowtransparency="True" width="100%" height="300px"></iframe>
                    </section>
                </div>--%>
                <%--<div class="wizardContainer animated fadeIn" id="imWizardEdit">
                </div>
                <div class="wizardContainer animated fadeIn" id="imWizardSummary">
                </div>--%>
            </div>
        </div>
        <div id="wizardFooter" class="wizard-footer">
            <div class="pull-left">
            </div>
            <div class="pull-right">
                <button type="button" title="Previous" id="wizardPrevbtn" class="coldbtn btn btn-info-full prev-step">Prev</button>
               <%-- <button id="wizardCancelbtn" onclick="closeWindow()" type="button" title="Cancel" class="coldbtn btn btn-info-full ">Cancel</button>--%>
                <%--<a id="tet" onclick="debugger;wizardSetObj.checkClick(this,'prev');" class="coldbtn btn btn-info-full ">testing</a>--%>
                <button data-validator="" title="Next" id="wizardNextbtn" type="button" class="hotbtn btn ">Next</button>
                <button type="button" id="wizardCompbtn" class="hotbtn btn  " title="Done">Done</button>
                <%--<button onclick="wizardSetObj.checkClick(this,'prev')" type="button" id="wizardPrevbtn1" class="wizardPrevNxtBtns waves-effect btn themeButton cutsomColorbtn left" title="Previous">&lt; Prev</button>--%>
            </div>
        </div>
        
        <script src="../Js/wizardnew.js"></script>
        <script>
        var proj = '<%=proj%>';
        var sid = '<%=sid%>';
        var lsid = '<%=lsIds%>';
        var lsstp = '<%=lsSteps%>';
        var lsrCnt = '<%=lssCount%>';

        var lsidArr = lsid.trim().split(',');
        var lsstpArr = lsstp.trim().split(',');
        var lsNewstp = "";
        for (i = 0; i <= lsidArr.length - 1; i++) {
            if (i == 0) {
                lsNewid = "'" + lsidArr[i].trim() + "'";
                lsNewstp = "'" + lsstpArr[i].trim() + "'";
            }
            else if (i == (lsidArr.length - 1)) {
                lsNewid = lsNewid + ",'" + lsidArr[i].trim() + "'";
                lsNewstp = lsNewstp + ",'" + lsstpArr[i].trim() + "'";
            }
            else {
                lsNewid = lsNewid + ",'" + lsidArr[i].trim() + "'";
                lsNewstp = lsNewstp + ",'" + lsstpArr[i].trim() + "'";
            }
        }
        var myIds = lsNewid.replace('\"', '');
        var mystps = lsNewstp.replace('\"', '');

        var $j = jQuery.noConflict();
        var $ = jQuery.noConflict();
        //  var AxwizardType = '<%=Session["AxWizardType"]%>';
        var targetElemClicked;

        function wizardSett(options) {
            var wizObj = this;
            wizObj.options = options || {};
            wizObj.WizardDataSetting = {
                steps: lsrCnt,
                ids: lsidArr,
                stepNames: lsstpArr,
                validateKeys: []
            }
            //wizObj.createWizard = function ()
            wizObj.completeCurrentStep = function () {
                var presentStep = $("#wizardHeader .step.in-progress");
                var target = presentStep.next().data('target');
                presentStep.removeClass('in-progress active').addClass('complete');
                presentStep.next().addClass('in-progress active');
                wizObj.showActiveData(target);
            }
            wizObj.assignEvents = function () {
                $("#wizardHeader .node,#wizardHeader a.stepName").on('click', function (event) {
                    event.preventDefault();
                    var elem = $(this);
                    wizObj.checkClick(elem);
                });
                if (wizObj.options.rtl) {
                    $("#wizardHeader .wizard-progress,#wizardBodyContent").addClass('wizardRtl');
                }
            }
            wizObj.checkClick = function (elem, type) {
                debugger;
                if (type == "prev" || type == "next") {
                    var parentElem = $("#wizardHeader .step.active");
                    var validateKey = parentElem.data('id');
                    var validateObj = parentElem.data('objtype');
                    if (type == "next")
                        var validationResult = wizObj.validateTheKey(validateObj, validateKey);
                    else if (type == "prev")
                        var validationResult = true;
                    if (validationResult === true) {
                        if (type == "next") {
                            var nextparentelem = parentElem.next();
                            if (!parentElem.hasClass('complete')) {
                                parentElem.addClass('complete').removeClass('in-progress active');
                                nextparentelem.addClass('in-progress active');
                            } else {
                                parentElem.removeClass('active');
                                nextparentelem.addClass('active');
                            }
                            var targetObj = nextparentelem.data('objtype');
                            var targetId = wizObj[targetObj].ids[nextparentelem.data('id')];
                            wizObj.showActiveData(targetId.trim())
                            wizObj.checkNxtPrevBtns();
                        } else {
                            parentElem.removeClass('active');
                            var prevParentElem = parentElem.prev();
                            prevParentElem.addClass('active');
                            var targetObj = prevParentElem.data('objtype');
                            var targetId = wizObj[targetObj].ids[prevParentElem.data('id')];
                            wizObj.showActiveData(targetId.trim())
                            wizObj.checkNxtPrevBtns();
                        }

                    } else {
                        if (validationResult !== undefined && validationResult != "")
                            showAlertDialog("warning", validationResult);
                    }

                } else {
                    var parentElem = elem.parents('.step');
                    var goingStep = parentElem.data('id');
                    var validateObj = parentElem.data('objtype');
                    var curStep = $("#wizardHeader .step.active").data('id');

                    if (curStep === goingStep || (!parentElem.hasClass('in-progress') && !parentElem.hasClass('complete'))) return;


                    targetElemClicked = elem;
                    var isValid = true;
                    // var isValid = (goingStep > curStep) ? obj.validateTheKey(validateObj, curStep) : true;
                    if (goingStep > curStep) {
                        for (var i = curStep; i < goingStep; i++) {
                            // Things[i]
                            var presValidation = wizObj.validateTheKey(validateObj, i);
                            if (presValidation !== true) {
                                if (presValidation !== undefined) $("#wizardHeader .step[data-id=" + i + "] a").click();
                                isValid = presValidation;
                                break;
                            }
                        }
                    }

                    if (isValid === true && !parentElem.hasClass('active') && (parentElem.hasClass('complete') || parentElem.hasClass('in-progress'))) {
                        targetElemClicked = "";
                        $("#wizardHeader .step.active").removeClass('active')
                        var target = parentElem.addClass('active').data('target');
                        wizObj.showActiveData(target);
                        wizObj.checkNxtPrevBtns();
                    } else if (isValid !== undefined && isValid != "") {
                        showAlertDialog("warning", isValid);
                    }
                }
            }
            wizObj.showActiveData = function (target) {
                $(".wizardContainer").hide();
                $("#" + target).show();
                wizObj.onPageShow(target);
                wizObj.focusTheFirstFld(target);
            }
            wizObj.onPageShow = function (target) {
                //hook to add extra code after page show
            }
            wizObj.onWizardCreate = function () {
                //hook to add extra code after wizard is created
            }
            wizObj.focusTheFirstFld = function (target) {
                $("#" + target).find('a,input:visible,button:visible,textarea:visible,select:visible').not(':disabled').first().focus();
            }
            wizObj.checkNxtPrevBtns = function () {
                var totalSteps = $("#wizardHeader .step").length;
                var curStep = $("#wizardHeader .step.active").data('id');
                if (curStep == 0) {
                    $("#wizardPrevbtn").hide();
                    $("#wizardNextbtn").show().html("Next &gt;");
                } else if (curStep == (totalSteps - 1)) {
                    $("#wizardPrevbtn").show();
                    $("#wizardNextbtn").show().html("Finish");
                } else {
                    $("#wizardPrevbtn").show();
                    $("#wizardNextbtn").show().html("Next &gt;");
                }
            }
            wizObj.validateTheKey = function (objct, key) {
                var presentObj = wizObj[objct];
                var validateKeys = presentObj.validateKeys;
                var validateKeyName = validateKeys[key];
                if (validateKeyName != "") {
                    //if (validateKeyName == "chtType") {
                    //    if ($("#chartSelData").data("isCompleted") != true)
                    //        return "Please select chart type.";
                    //}
                    return true;
                } else {
                    return true;
                }
            }
            wizObj.resetTheWizard = function () {
                $("#wizarTypeSel").show();
                $("#wizardWrappper").hide();
                $("#wizardHeader").html("");
                $(".wizardPrevNxtBtns").hide();
                wizObj.onWizardClose();
            }
            wizObj.onWizardClose = function () {
                // hook when wizard close
            }
        }

        function closeWindow() {
            parent.closeModalDialog();
        }


    </script>
    <script>
       
        $(document).ready(function () {
                //Widget Work flow code - begins
                //var wizardSetObj = new WizardComp("modern");
            wizardSetObj = new wizardSett("modern");
            wizardSetObj.checkNxtPrevBtns();
                //alert(wizardSetObj.);
                //wizardSetObj.checkClick(this,'next')
                //$('#tet').click(function () {
                //    wizardSetObj.checkClick(this, 'next');
                //});
                $('#wizardNextbtn').click(function () {
                    wizardSetObj.checkClick(this, 'next');
                });
                $('#wizardPrevbtn').click(function () {
                    wizardSetObj.checkClick(this, 'prev');
                });
            });
        </script>
    </form>
</body>
</html>
