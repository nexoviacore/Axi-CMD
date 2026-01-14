        function CheckLinksType() 
        {        
            if ($("#rdbtnapplicationlinks").attr('checked')) {
                ShowAppLinks();
            }
            else if ($("#rdbtnotherlinks").attr('checked')) {
                ShowOtherLlinks();
            }
        }

        function ShowAppLinks() {
         var nweFrm = $("#middle1", parent.document);
         nweFrm.height("317");
           $("#div_applicationlinks").show();
            $("#div_otherlinks").hide();
            $("#hdnLinkType").val("Application");
            $("#btnLinkType").trigger('click');
        }

        function ShowOtherLlinks() {
           var nweFrm = $("#middle1", parent.document);
           nweFrm.height("367");
            $("#div_otherlinks").show();
            $("#div_applicationlinks").hide();
            $("#hdnLinkType").val("other");
            $("#btnLinkType").trigger('click');
        }
        function ValidateQuickLinks() 
        {
           if ($("#rdbtnapplicationlinks").attr('checked')) 
            {
                
               if( ($("#hdnTotalRecords").val() >= 8) && ($("#hdnAddUpdate").val() == "0") )
                 {
                   showAlertDialog("warning","Maximum 8 quick links can be added");
                 }
               
                else if (document.getElementById("ddlQuickLinks").value == "0") {
                    showAlertDialog("warning","Please Select Page");
                    return false;
                }
                else if ($("#divicon #imgdefualtadd").is(":visible")) {
                    showAlertDialog("warning","Please Select Icon");
                    return false;
                }

              else {
                    $("#btnSaveQuickLink").trigger('click');

                }
            }
            else {
                var expression = /[-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?/gi;
                var regex = new RegExp(expression);
                var t = document.getElementById("txturl").value;
                if( $("#hdnTotalRecords").val() >= 8 && ("#hdnAddUpdate").val() == "0")
                 {
                    showAlertDialog("warning","Maximum 8 quick links can be added");
                 }
               
                if (document.getElementById("txtname").value == "") {
                    showAlertDialog("warning","Please enter Link Name");
                    return false;
                }
                else if (document.getElementById("txturl").value == "") {
                    showAlertDialog("warning","Please enter Url");
                    return false;
                }
                else if (!t.match(regex)) {
                    showAlertDialog("warning","Please enter url in correct format www.domainname.com");
                }
                else if
               ($("#divicon #imgdefualtadd").is(":visible")) {
                    showAlertDialog("warning","Please Select Icon");
                    return false;
                }
                else {

                    $("#btnSaveQuickLink").trigger('click');
                }
            }
        }
        
        function LoadMainLinks(msg) {
            window.parent.document.getElementById('divQuickLinksMenu').innerHTML = $("#hdnQuickLinkMenus").val();
            showAlertDialog("warning",msg)
            location.reload(true);
          }
        
         function adjustquicklinkwin(iframeWindow, hgt) 
         {
          var nweFrm = $("#middle1", parent.document);
          if (parseInt(hgt) > 330)
                 {
                    nweFrm.height("370");
                 }
                else {
                    nweFrm.height("317"); 
                }
          }

          /*menu scripts*/
          function showIcongallery() {
          $('#divtbliconimages').html('');
            var iconclassarray = ["icon-glass","icon-music","icon-search","icon-envelope","icon-heart","icon-star","icon-star-empty","icon-user","icon-film","icon-th-large","icon-th","icon-th-list","icon-ok","icon-remove","icon-zoom-in","icon-zoom-out","icon-off","icon-signal","icon-cog","icon-trash","icon-home","icon-file","icon-time","icon-road","icon-download-alt","icon-download","icon-upload","icon-inbox","icon-play-circle","icon-repeat","icon-refresh","icon-list-alt","icon-lock","icon-flag","icon-headphones","icon-volume-off","icon-volume-down","icon-volume-up","icon-qrcode","icon-barcode","icon-tag","icon-tags","icon-book","icon-bookmark","icon-print","icon-camera","icon-font","icon-bold","icon-italic","icon-text-height","icon-text-width","icon-align-left","icon-align-center","icon-align-right","icon-align-justify","icon-list","icon-indent-left","icon-indent-right","icon-facetime-video","icon-picture","icon-pencil","icon-map-marker","icon-adjust","icon-tint","icon-edit","icon-share","icon-check","icon-move","icon-step-backward","icon-fast-backward","icon-backward","icon-play","icon-pause","icon-stop","icon-forward","icon-fast-forward","icon-step-forward","icon-eject","icon-chevron-left","icon-chevron-right","icon-plus-sign","icon-minus-sign","icon-remove-sign","icon-ok-sign","icon-question-sign","icon-info-sign","icon-screenshot","icon-remove-circle","icon-ok-circle","icon-ban-circle","icon-arrow-left","icon-arrow-right","icon-arrow-up","icon-arrow-down","icon-share-alt","icon-resize-full","icon-resize-small","icon-plus","icon-minus","icon-asterisk","icon-exclamation-sign","icon-gift","icon-leaf","icon-fire","icon-eye-open","icon-eye-close","icon-warning-sign","icon-plane","icon-calendar","icon-random","icon-comment","icon-magnet","icon-chevron-up","icon-chevron-down","icon-shopping-cart","icon-folder-close","icon-folder-open","icon-resize-vertical","icon-bullhorn","icon-bell","icon-thumbs-up","icon-thumbs-down","ap","ba","cma","dis","dl","lms","pms","rm","slo","ssm","ma","cm","mas","as","cd","am","icon-hand-right","icon-hand-left","icon-hand-up","icon-hand-down","icon-circle-arrow-right","icon-circle-arrow-left","icon-circle-arrow-up","icon-circle-arrow-down","icon-globe","icon-wrench","icon-tasks","icon-filter","icon-briefcase","icon-fullscreen","mis"];
            var arrayLength = iconclassarray.length;
            for (var i = 0; i < arrayLength; i++) {
                $('#divtbliconimages').append("<div id='iconContent'><a onclick=bindicon('" + iconclassarray[i] + "');><i style='font-size:22px; padding:3px;'class='" + iconclassarray[i] + "' ></i> </a><div> ");
            }
            $(".divicongallery").dialog({
                title: "Choose Icon",
                width: 360,
                height: 250,
                top: 0,
                modal: true,
                buttons: [
                        {
                            text: 'Close',
                            class: 'btn blue',
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                           ]
            });
        }

        function bindicon(path) {
          if($("#hdnImageIconPath").length>0)
            $("#hdnImageIconPath").val(path);
            $('.divicongallery').dialog('close');
            $("#divicon i").removeClass().addClass(path);
            $("#divicon i").css("display", "block");
            $("#divicon i").css("height", "70px");
            $("#divicon #imgdefualtadd").css("display", "none");
            document.getElementById('hdnImageIconPath').value = path;
            var nweFrm = $("#middle1", parent.document);
            if ($("#rdbtnapplicationlinks").attr('checked')) {
               nweFrm.height("317");
            }
            else if ($("#rdbtnotherlinks").attr('checked')) {
              nweFrm.height("367");
            }
        }
             
        function Loadmainmenus(curIcon, parentPageName) {
            $("#i_" + parentPageName, window.parent).removeClass();
            $("#i_" + parentPageName, window.parent).addClass(curIcon);
            window.parent.document.getElementById("i_" + parentPageName).className = curIcon;

        }
       
        function ShowIconImage() {
            var item = $('#ddlParentMenu :selected').text();
            $('#hdnSelectedVal').val(item);
            var itemVal = $('#ddlParentMenu :selected').val();
            if (itemVal != "") {
                $("#imgdefualtadd").hide();
                $("#iconimage").removeClass();
                $("#iconimage").addClass(itemVal);
                $("#imgdefualtadd").hide();
            }
            else {
                $("#iconimage").hide();
                $("#imgdefualtadd").show();
            }

        }
        function LoadIframe(src) {
            
            var el = document.getElementById('middle1');
            el.src = src;
            if (!$("#inner-page").is(":visible"))
                $("#inner-page").show();
        }
        function loadHome() {
            $('#inner-page', window.parent.document).hide();

        }

        function adjustwin(iframeWindow, hgt) {
            var framename = iframeWindow.name;

            var nweFrm = $("#middle1", parent.document);

            if (nweFrm.length > 0) {
                var winHgt = $(window.document.body).height();
                if (parseInt(winHgt, 10) < 250) {
                    nweFrm.height("229");
                }
                else {
                    nweFrm.height("267");
                }
            }
        }
