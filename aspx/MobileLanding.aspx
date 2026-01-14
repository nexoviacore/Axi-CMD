<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MobileLanding.aspx.cs" Inherits="aspx_MobileLanding" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8"/> 
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>    
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <script>
        if (!('from' in Array)) {
            // IE 11: Load Browser Polyfill
            document.write('<script src="../Js/polyfill.min.js"><\/script>');
        }
    </script>
    <script type="text/javascript" src="../Js/thirdparty/jquery/3.1.1/jquery.min.js"></script>
    <link href="../Css/thirdparty/bootstrap/3.3.6/bootstrap.min.css" rel="stylesheet" />
    <link href="../Css/Icons/icon.css" rel="stylesheet" />
    <link href="../Css/mobilelanding.min.css" rel="stylesheet" />    
    <link id="themecss" type="text/css" href="" rel="stylesheet" />
</head>
<body>
   <div class="main-modelbox">
    <div class="row">
      <div class="main-folderwrapper">
      </div>
      <div id='divModalImportData'></div>
    </div>  
    <script type="text/javascript">
      var pageHead='';
     $(document).ready(function(){        
        var tempLandingWrapper = $(parent.callParentNew("mobileMenuWrapper")).html();
        if(tempLandingWrapper != ''){
            $('.main-folderwrapper').html(tempLandingWrapper);
        }else{
          addMenuPage('root',0);
        }
      // var theme = parent.currentThemeColor;
      // $("#themecss").attr('href', "../App_Themes/" + theme + "/Stylesheet.min.css?v=23");
        
        
      });

    

     function loadChildHtml(that,id,parentId,tasks){
        $('.menuLevels').hide();
        if( $('#menuLevel_'+id).length  === 0 && id != 0)
        {
          var tmpHeadTitle = $(that).find('span').html();
          addMenuPage(id,parentId,tmpHeadTitle);
        }
        if(tasks == 'back'){
          $('#menuLevel_'+parentId).show();
        }else{
          $('#menuLevel_'+id).show();
        }
        
     }

  
    function addMenuPage(id,parentId,HeadTitle){
      HeadTitle = (HeadTitle)?HeadTitle:'';
      var thatObj,htmlOutput='',tmpId='';
        tmpId = (id == 'root')?0:id;
        htmlOutput += '<div class="menuLevels" id="menuLevel_'+tmpId+'" data-parent="">';
        if(id > 0){
            htmlOutput += '<div class="pageHeadMobile col-sm-12"><i class="pageHead-back icon-arrows-left" onClick="loadChildHtml(this,' + tmpId + ',' + parentId + ',\'back\')"></i><span>' + HeadTitle + '</span></div>';
        }
        $.each(parent.menuJson.root.row,function(){
          thatObj = $(this)[0];
          if($.trim(thatObj.parent).toLowerCase() == id){
            console.log(thatObj);
            if($.trim(thatObj.struct_type) == 'F'){
              htmlOutput +=addFolderHtml(thatObj,'icon-basic-folder',tmpId);
            }else{
              htmlOutput +=addFolderHtml(thatObj,'icon-basic-sheet-txt',tmpId);
            }
          }          
        });
        htmlOutput += '<div class="clearfix"></div>';
        htmlOutput += '</div>';
        $('.main-folderwrapper').append(htmlOutput);
    }
    function updateMainPage(){
        $(parent.callParentNew("mobileMenuWrapper")).html($('.main-folderwrapper').html());
    }
    function addFolderHtml(thatObj, icon, parentId) {
        
      var tmpObj = thatObj;
      if(thatObj.folder_name)
      {
        tmpObj.folder_id = (thatObj.folder_id == 'root')?0:thatObj.folder_id;
        tmpObj.onClick = 'onClick="loadChildHtml(this,'+tmpObj.folder_id+','+parentId+');"';
        tmpObj.displayName = thatObj.folder_name;
      } else {
          if (thatObj.struct_type == 'I')
              tmpObj.onClick = 'onclick="updateMainPage();parent.LoadIframe(\'iview.aspx?ivname=' + thatObj.sname + '\');"';
          else if (thatObj.struct_type == 'S')
              tmpObj.onClick = 'onclick="updateMainPage();parent.LoadIframe(\'tstruct.aspx?transid=' + thatObj.sname + '\');"';
        tmpObj.displayName = thatObj.menu_caption;
      }
      var htmlTmp = '';
          htmlTmp += '<div class="col-sm-3 col-xs-6">';
          htmlTmp += '    <div class="table-folder-inner" '+tmpObj.onClick+'>';
          htmlTmp += '      <img src="../assets/menuIcons/'+$.trim(tmpObj.displayName.toLowerCase())+'.jpg" style="height: 135px;width: 135px;margin-top: 12px;">';          
          htmlTmp += '      <span>'+ tmpObj.displayName+'</span>';
          htmlTmp += '    </div>';
          htmlTmp += '</div>';
        return htmlTmp;
    }
    </script>
</body>
</html>
