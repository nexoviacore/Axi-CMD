using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for Constants
/// </summary>
public class Constants
{
    public Constants()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public const string vbCrLf = "";

    #region Error messages for culture 
    public const string ERROR = "<error>";
    public const string ERRORCLOSE = "</error>";
    public const string REC_NOT_FOUND = "No data found.";
    public const string SEL_ROWS_FOUND = "All rows are already selected.";
    public const string SESSIONERROR = "invalid sessionid";
    public const string CUSTOMERROR = "Server error. Please try again.If the problem continues, please contact your administrator.";
    public const string SUCCESS = "<success>";
    public const string SUCCESSCLOSE = "</success>";
    public const string SESSIONEXPMSG = "Disconnected because you have logged into another session.";
    public const string EMPTYOPTION = "-- Select --";
    public const string PARAMERR = "err.aspx?errmsg=Invalid parameter";
    public const string LOGINERR = "signin.aspx?msg=Invalid Username or Password.";
    public const string LOGINPAGE = "signin.aspx";
    public const string ERRPATH = "../aspx/err.aspx?errmsg=";
    public const string ERAUTHENTICATION = "Session Authentication failed...";
    public const string DUPLICATESESS = "Duplicate_session";
    public const string INVALIDURL = "Requested Page cannot be loaded as the URL contains invalid character.";
    public const string MALICIOUSNPUTDETECTED = "Invalid input detected, Please try again.";
    public const string SESSIONTIMEOUT = "SESSION_TIMEOUT";
    public const string RELEASINGSOON = "Releasing soon.";
    public const string RELEASINGSOONPAGES = "ad___ugp,ad___ucl,ad___cmp,ad___brn,ad__papi";
    #endregion

    public const string IMGPrefix = "nodb_";
    public const string AXPIMAGEPATH = "axpimagepath";
    public const string TRACESPLITSTR2 = "♦♦";
    public const string TRACESPLITSTR1 = "♦";
    public const string IMGSRCPrefix = "nodbsrc_";
    public const string PROGRESS = "InProgress";
    public const string INIT = "INIT";
    public const string AOWE = "AOWE";
    public const string APPEND = "APPEND";
    public const string BACKIMG = "../AxpImages/icons/24X24/backbutton.png";
    public const string FORWARDIMG = "../AxpImages/icons/24X24/forwardbutton.png";
    public const string TILE = "tile";
    public const string DEFAULT = "default";
    public const string DOUBLE = "double";
    public const string SINGLE = "single";
    public const string ACPICKPAGESIZE = "50";
    public const string TABBED = "tabbed";
    public const string AXPISROWVALID = "axp__isGrdVld";
    public const string GeneralLog = "GeneralLog";
    public const string structHTML = "TSTHTML";
    public const string tstHeaderHTML = "TSTHDRHTML";
    public const string toolbarBtnIcons = "TOOLBTNS";
    public const string modernToolbarBtnOpen = "MODERNTOOLBTNS";
    public const string attachHTML = "ATTHTML";
    public const string taskBtns = "TSKBTNS";
    public const string structScript = "TSTSCRIPT";
    public const string structBtns = "TSTBTNS";
    public const string structCaption = "TSTCAPTION";
    public const string strSubmitCancel = "TSTFOOTER";
    public const string strDesign = "TSTDESIGN";
    public static string[] fileTypes = { ".bmp", ".jpg", ".jpeg", ".pjpeg", ".png", ".gif", ".doc", ".docx", ".xml", ".xlsx", ".ppt", ".pdf", ".txt", ".xls", ".pwd", ".tiff", ".tif", ".dib", ".jfif", ".heic", ".xlxs", ".csv", ".html", ".css", ".js", ".htm", ".eml", ".msg", ".pptx" };
    public static string[] notallowfileTypes = { ".dll.", ".exe.", ".bat.", ".reg.", ".com.", ".php.", ".py.", ".svg." };
    public static string[] disallowedMimeTypes = { "application/x-msdownload", "application/vnd.microsoft.portable-executable", "application/x-sh", "application/x-bat", "application/java-archive", "application/zip", "application/javascript", "text/javascript", "application/x-python", "application/x-ruby", "text/x-php", "application/x-rar-compressed", "application/x-7z-compressed", "application/x-iso9660-image", "application/x-virtualbox-ova" };
    public static string fileExtensionImage = ".gif,.png,.jpg,.jpeg,.bmp,.tiff,.tif,.heic,.jfif";
    public const string NO_DRAFTS = "No Drafts";
    public const string DRAFT_REFRESH = "refresh";
    public const string NO_DRAFT_HTML = "<ul class=\"dropdown-menu draftUL\"><li><a href=\"javascript:void(0)\">No Drafts</a></li></ul>";
    public const string EX_DRAFT_HTML = "<ul class=\"dropdown-menu draftUL\"><li><a href=\"javascript:void(0)\">Couldn't fetch the Drafts</a></li></ul>";


    public const string ORCL_QRY_PRPS_DSGN = "declare cnt int; val varchar2(31000);  begin val := '$VALUE$'; select count(*) into cnt from axpCloudDevSettings where TranID='$TRANID$' and type='$TYPE$'; if (cnt>0) then   UPDATE axpCloudDevSettings SET value = val where  TranID='$TRANID$' and type='$TYPE$'; else   insert into axpCloudDevSettings (USERID,tranid,type,value) Values ('$USERID$','$TRANID$','$TYPE$',val) ; END IF; end;      ";
    public const string ORCL_QRY_PRPS_USER = "declare cnt int; begin select count(*) into cnt from axpCloudUserSettings where  userid = '$USERID$' and TranID='$TRANID$' and type='$TYPE$'; if (cnt>0) then   UPDATE axpCloudUserSettings SET value = '$VALUE$' where  userid = '$USERID$' and TranID='$TRANID$' and type='$TYPE$'; else   insert into axpCloudUserSettings (userid,tranid,type,value) Values ('$USERID$','$TRANID$','$TYPE$','$VALUE$') ; END IF; end;";

    public const string MYSQL_QRY_PRPS_DSGN = "INSERT INTO axpCloudDevSettings (userid,TranID,Type,Value)  values ('$USERID$','$TRANID$','$TYPE$','$VALUE$') ON DUPLICATE KEY UPDATE Value= values(Value);";

    public const string MYSQL_QRY_PRPS_USER = "INSERT INTO axpCloudUserSettings (userid,TranID,Type,Value)  values ('$USERID$','$TRANID$','$TYPE$','$VALUE$') ON DUPLICATE KEY UPDATE Value= values(Value);";

    public const string SQL_QRY_PRPS_USER = "if exists(select * from axpCloudUserSettings where userid='$USERID$' and Type='$TYPE$' and TranID='$TRANID$') update axpCloudUserSettings set Value ='$VALUE$' where userid='$USERID$' and Type='$TYPE$' and TranID='$TRANID$' else insert into axpCloudUserSettings (userid,TranID,Type,Value)  values ('$USERID$','$TRANID$','$TYPE$','$VALUE$')";
    public const string SQL_QRY_PRPS_DSGN = "if exists(select * from axpCloudDevSettings where Type='$TYPE$' and TranID='$TRANID$') update axpCloudDevSettings set Value ='$VALUE$' where Type='$TYPE$' and TranID='$TRANID$' else insert into axpCloudDevSettings (userid,TranID,Type,Value)  values ('$USERID$','$TRANID$','$TYPE$','$VALUE$')";


    public const string QUERY_GET_PRPS_STATUS = "Select ( CASE WHEN (Select Value  from axpCloudDevSettings where userid='$USERID$' and type='$TYPE$' and TranID='$TRANID$') IS NULL THEN to_clob('0') else (Select Value  from axpCloudDevSettings where userid='$USERID$' and type='$TYPE$' and TranID='$TRANID$') end ) AS admn,(CASE WHEN (Select Value  from axpCloudUserSettings where USERID='$USERID$' and type ='$TYPE$' and TranID='$TRANID$') IS NULL THEN to_clob('0') else (Select Value  from axpCloudUserSettings where USERID='$USERID$' and type='$TYPE$' and TranID='$TRANID$')end ) AS usr from dual"; //from dual, to_clob

    public const string MYSQL_QUERY_GET_PRPS_STATUS = "Select ( CASE WHEN (Select Value  from axpCloudDevSettings where userid='$USERID$' and type='$TYPE$' and TranID='$TRANID$') IS NULL THEN '0' else (Select Value  from axpCloudDevSettings where userid='$USERID$' and type='$TYPE$' and TranID='$TRANID$') end ) AS admn,(CASE WHEN (Select Value  from axpCloudUserSettings where USERID='$USERID$' and type ='$TYPE$' and TranID='$TRANID$') IS NULL THEN '0' else (Select Value  from axpCloudUserSettings where USERID='$USERID$' and type='$TYPE$' and TranID='$TRANID$')end ) AS usr ";

    public const string SQL_CHECK_USER = "SELECT USERNAME FROM AX_USER_SETTINGS WHERE USERNAME = '$USERID$'";
    public const string SQL_LOAD_USER_APPSETTINGS = "SELECT APPSETTINGS FROM AX_USER_SETTINGS WHERE USERNAME = '$USERID$'";
    public const string SQL_SAVE_USER_APPSETTINGS = "UPDATE AX_USER_SETTINGS SET APPSETTINGS = '$VALUE$' WHERE USERNAME = '$USERID$'";
    public const string SQL_INSERT_USER_APPSETTINGS = "INSERT INTO AX_USER_SETTINGS (USERNAME, APPSETTINGS) VALUES ('$USERID$', '$VALUE$')";
    public const string DEFAULT_USER_APPSETTINGS = "{\"sabrnig\":false}";

    public const string MYSQL_QUERY_IVIR_SAVE_JSON = "update AX_USER_SETTINGS set IR_CONFIG = '$VALUE$' where USERNAME = '$USERID$'";
    public const string MYSQL_QUERY_IVIR_GET_JSON = "select IR_CONFIG from AX_USER_SETTINGS where USERNAME = '$USERID$'";

    public const string UPDATE_PRIVATE_SSO_TOKEN_IN_DB = "update axusers set singleloginkey = '$VALUE$' where USERNAME = '$USERID$'";

    public const string GET_TST_CUSTOM_ACTIONS = "Select configname,cvalue from axpconfigs_$TRANSID$";
    public const string RAPID_FORMLOAD_TBL = "Axp_formload";
    public const string RAPID_DEPENDENCY_TBL = "Axp_Dependents";
    public const string GET_RAPID_DEF_SP = "SP_RAPIDDEFINITION";

    public const string GET_IV_GROUPED_BUTTONS = "Select cvalue from axpconfigsiv_$IVNAME$ where configname='groupbtns'";

    public const string GET_IV_TEMPLETE = "select cvalue, elements from templates where type='Iview' and iviewid='$IVNAME$'";

    public const string VAR_TRANSID = "$TRANSID$";
    public const string VAR_IVNAME = "$IVNAME$";
    public const string VAR_NAME = "$NAME$";


    public const string IVIEWSTRUCT = "ivewstruct";
    public const string IVIEWPARAM = "ivewsparam";
    public const string REDISXML = "<data perfxml=\"true\"><comps cat =\"comps\" LocalNum=\"11\" btn=\"7\" vupd=\"t\" pnlheight=\"33\"><btn2 caption =\"\" hint=\"Preview\" img=\"preveiw.bmp\" position=\"Left\" flat=\"True\" sname=\"\" container=\"\" action=\"\" task=\"preview\" def=\"right\" cat=\"btn\" parent=\"btnPanel\" tlhw=\"0,0,25,80\" font=\",,,\" /><btn4 caption =\"\" hint=\"View\" img=\"view.bmp\" position=\"Left\" flat=\"True\" sname=\"\" container=\"\" action=\"\" task=\"view\" def=\"right\" cat=\"btn\" parent=\"btnPanel\" tlhw=\"0,80,25,80\" font=\",,,\" /> <btn5 caption =\"\" hint=\"Find\" img=\"find.bmp\" position=\"Left\" flat=\"True\" sname=\"\" container=\"\" action=\"\" task=\"find\" def=\"right\" cat=\"btn\" parent=\"btnPanel\" tlhw=\"0,160,25,80\" font=\",,,\" /><btn10 cat =\"btn\" caption=\"Clear cache\" img=\"\" position=\"\" flat=\"False\" down=\"False\" sname=\"\" container=\"\" action=\"deletekey\" task=\"\" hint=\"\" def=\"\" pop=\"False\" rel=\"\" parent=\"btnPanel\" tlhw=\"0,240,25,80\" font=\",,,\" desc=\"\" apply=\"\" /><btn1 cat =\"btn\" parent=\"btnPanel\" tlhw=\"0,0,25,80\" font=\",,,\" /><btn3 cat =\"btn\" parent=\"btnPanel\" tlhw=\"0,160,25,80\" font=\",,,\" /><btn7 caption =\"\" hint=\"Params\" img=\"Params.bmp\" position=\"Left\" flat=\"True\" sname=\"\" container=\"\" action=\"\" task=\"params\" def=\"right\" cat=\"btn\" parent=\"btnPanel\" tlhw=\"0,320,25,80\" font=\",,,\" auto=\"yes\" rtask=\"yes\" /><header><h1 font =\"clBlack,12,,T\">TESTING</h1></header></comps><headrow reccount =\"895\" pagesize=\"0\" totalrows=\"895\" datarows=\"895\" vtype=\"c\"><pivotghead /><rowno hide =\"false\" /><axrowtype hide =\"true\" width=\"80\" dec=\"0\" type=\"c\">axrowtype</axrowtype><axp__font hide =\"true\" width=\"80\" dec=\"0\" type=\"c\">axp__fontdetails</axp__font><ftype hide =\"false\" width=\"98\" dec=\"0\" align=\"Left\" type=\"c\">Type</ftype><tname hide =\"false\" width=\"106\" dec=\"0\" align=\"Left\" type=\"c\">Name</tname><caption hide =\"false\" width=\"287\" dec=\"0\" align=\"Left\" type=\"c\">Caption</caption><keysize hide =\"true\" width=\"90\" dec=\"0\" align=\"Left\" type=\"c\">keysize</keysize><totalkeys hide =\"false\" width=\"84\" dec=\"0\" align=\"Center\" type=\"n\">Total keys</totalkeys><keys hide =\"true\" width=\"473\" dec=\"0\" align=\"Left\" type=\"c\">keys</keys></headrow></data>";

    public const string CACMENU = "CACMENU";
    public const string REDISTSTRUCT = "Tstruct";
    public const string REDISTSTFLDFROMAPI = "TstFldFromApi";
    public const string REDISTSTRUCTMOB = "TstructMob";
    public const string REDISTSTRUCTTABLE = "TstDcTable";
    public const string REDISTSTRUCTDOFORM = "TstructDoForm";
    public const string REDISTSTRUCTDESIGN = "TstDsgn";
    public const string REDISTSTRUCTAXDESIGN = "axTstDsgn";
    public const string REDISTSTRUCTAXCUSTHTML = "axTstCustomHtml";
    public const string REDISTSTRUCTAXDESIGNTABLE = "axTstDsgnTbl";
    public const string REDISTSTRUCTALL = "axTstAll";
    public const string REDISTSTRUCTDTLS = "TstructDtls";
    public const string REDISTSTRUCTDTLSMOB = "TstructDtlsMob";
    public const string REDISTSTRUCTDESIGNDTLS = "TstDsgnDtls";
    public const string REDISTSTCUSTOMJSCSS = "TstCustomJsCss";
    public const string DEPFLDARRAY = "depFldArray";
    public const string FIELDNAME = "fieldName";
    public const string FIELDPARAMNAMES = "fldParamNames";
    public const string FIELDINDEX = "fieldIndex";
    public const string FORMLOADARRAY = "FormLoadArray";
    public const string FORMLOADINDEX = "FormLoadIndex";
    public const string DBMEMVARSFORMLOAD = "axdbmemvars";
    public const string CONFIGDATAVARSFORMLOAD = "axconfigdatavars";
    public const string FORMLOADRES = "FormLoadRes";
    public const string AXRULESDEF = "AxRulesDef";
    public const string AXRULESDEFUserRole = "AxRulesDefUserRole";
    public const string AXVALERRORCODE = "AxValErrorCode";
    public const string AXRULESDEFBTNCMSG = "AxRulesDefBtnConMsg";
    public const string AXRULESDEFBTNCONMSGUSERROLE = "AxRBtnConMsgUserRole";

    public const string HTMLPAGESQUERY = "HTMLPAGESQUERY";

    public const string CLOUD_HOME_API = "api/Session/IsValidSession/{0}/{1}";
    public const string REDISMENUDATA = "MenuData";
    public const string REDISCARDSDATA = "CardsData";
    public const string REDISCARDKEYS = "CardKeys";
    public const string REDISCARDPARAMS = "CardsParams";
    public const string REDISCARDROLES = "CardsRoles";

    public const string REDISHOMEPAGECARDS = "HomePageCards";

    public const string REDISCARDSDATADASHBOARD = "DashBoardCardsData";
    public const string REDISCARDKEYSDASHBOARD = "DashBoardCardKeys";
    public const string REDISCARDPARAMSDASHBOARD = "DashBoardCardsParams";
    public const string REDISCARDROLESDASHBOARD = "DashBoardCardsRoles";

    public const string REDISAPIKEYS = "ApiKeys";
    public const string REDISGLOBALVARS = "GlobalVars";
    public const string REDISAXUSEROPTIONS = "axUserOptions";
    public const string REDISLVRECORDLISTING = "lvRecordListing";
    public const string REDISHYBRIDINFO = "mobilehybridinfo";
    public const string REDISKEEPWEBINFO = "keepaliveweb";
    public const string REDISLASTLOGGEDIN = "LastLoggedInInfo";

    public const string REDISPROFILEPIC = "ProfilePic";

    public const string ListViewFieldsInfo = "ListFldsInfo";

    public const string RedisIviewObjList = "IviewObjList";

    public const string RedisIviewObj = "IviewObj";

    public const string RedisListviewObjList = "ListviewObjList";

    public const string RedisListviewObj = "ListviewObj";

    public const string RedisIvData = "IvData";

    public const string RedisLvData = "LvData";

    public const string RedisIviewBuilderObj = "IviewBuildObj";

    public const string RedisIviewSettings = "IviewSettings";

    public const string RedisListviewSettings = "ListviewSettings";

    public const string RedisIviewBuilderSettingsObj = "IviewBuildSetingsObj";

    public const string RedisOldIviewSettings = "IviewOldSettings";

    public const string RedisOldListviewSettings = "ListviewOldSettings";

    public const string RedisOldIviewTemplates = "IviewOldTemplates";

    public const string RedisOldListviewTemplates = "ListviewOldTemplates";

    public const string IviewNavigationData = "IviewNavigationData";

    public const string REDISAXAUTOINTELLI = "AxAutoIntelli";

    public const string GET_SEARCH_DATA = "select SEARCHTEXT,HLTYPE,STRUCTNAME,PARAMS,OLDAPPURL from axp_appsearch where username=:username and lower(searchtext) like $KEYWORD$";
    //public const string GET_SEARCH_DATA = "select SEARCHTEXT,HLTYPE,STRUCTNAME,PARAMS from axp_appsearch where username=:username and lower(searchtext) like $KEYWORD$";

    //public const string GET_SEARCH_DATA_LANG = " select case when  b.dispname= :language and b.dispname is not null then b.compcaption  else searchtext end  as caption,HLTYPE,STRUCTNAME,PARAMS , case when b.dispname is null then 'ALL' else b.dispname end  as language, b.compcaption as searchtext from axp_appsearch a left join axlanguage b on a.params is null and b.compname like '%'||a.structname and b.sname= 'axpages' where username =:username and lower(b.compcaption) like $KEYWORD$";
    public const string GET_SEARCH_DATA_LANG = " select case when lower(b.dispname)= :language and b.dispname is not null then b.compcaption  else a.searchtext end  as caption, a.hltype,a.structname,a.params , case when b.dispname is null then 'ALL' else b.dispname end  as language, coalesce(b.compcaption, a.searchtext) as searchtext from axp_appsearch a left join axlanguage b on a.params is null and b.compname like '%'||a.structname and lower(b.sname) = 'axpages' where username =:username  and (lower(b.compcaption) like $KEYWORD$ or lower(a.searchtext) like $KEYWORD$)";
    public const string GET_SEARCH_DATA_LANG_MSSQL = " select case when lower(b.dispname)= :language and b.dispname is not null then b.compcaption  else a.searchtext end  as caption, a.hltype,a.structname,a.params , case when b.dispname is null then 'ALL' else b.dispname end  as language, coalesce(b.compcaption, a.searchtext) as searchtext from axp_appsearch a left join axlanguage b on a.params is null and b.compname like '%'+ a.structname and lower(b.sname) = 'axpages' where username =:username  and (lower(b.compcaption) like $KEYWORD$ or lower(a.searchtext) like $KEYWORD$)";
    public const string COLLAPSEMENU = "Collapse";

    public const string CFG_CONFIG_FILE = "generic_config";
    public const string GET_DASHBOARD_FILTERDATA = "select * from axp_fparamvalues where username=:username";// columns will be adding dynamically

    //Note: While changing 'DEFAULT_CONFIGSTR', verify the 'AppConfiguration.cs' scenarios as well. No spaces should be used before and after in Key Names.

    public const string DEFAULT_CONFIGSTR = "{\"configStr\": [{\"AxLoginTrace\": \"false\"},{\"AxAlertTimeout\": \"3\"},{\"AxSessionExpiryDays\": \"5\"},{\"AxBreadCrumb\": \"false\"},{\"AxEnableCards\": \"true\"},{\"AxExportTallyTid\": \"\"},{\"AxCPWDOnLogin\": \"true\"},{\"AxHelpIview\": \"\"},{\"AxSessionExtend\": \"true\"},{\"AxTimezoneVariation\": \"true\"},{\"AxDisableSplit\":\"false\"},{\"AxGlobalSrchLimit\":\"5000\"},{\"AxErrorMsg\": \"false\"},{\"AxErrorMsgTimeout\": \"0\"},{\"AxShowLoggedinUsersCount\": \"false\"},{\"AxImpExpTemptPath\": \"\"},{\"AxLanguages\": \"english\"},{\"AxUSCulture\": \"false\"},{\"AxUserLevelLang\": \"false\"},{\"AxDevInstance\": \"false\"},{\"AxDisplayAutoGenVal\": \"false\"},{\"AxIsPerfCode\": \"true\"},{\"AxDcGridOnSave\": \"true\"},{\"AxAttachmentSize\": \"1\"},{\"AxImagePath\": \"\"},{\"AxAttachFilePath\": \"\"},{\"AxGridAttachPath\": \"\"},{\"AxShowSubmitCancel\": \"false\"},{\"AxDesignerAccess\": \"designer\"},{\"AxIsPrintExe\": \"true\"},{\"AxPrintExePath\": \"\"},{\"AxHtmlPath\": \"\"},{\"AxPrintMargins\": \"50,50,200,100\"},{\"AxPrintTitleAlign\": \"center\"},{\"AxIviewcelltextwrap\": \"true\"},{\"AxMergeRowIviews\": \"\"},{\"AxPrintRowsMaxLimit\": \"10000\"},{\"AxDbPagination\": \"true\"},{\"AxDefaultPageSize\": \"30\"},{\"AxShowAppTitle\": \"true\"},{\"AxShowStripedReport\": \"false\"},{\"AxGetIviewRowCount\": \"false\"},{\"AxIviewDataWSRows\": \"1000\"},{\"AxInlineGridEdit\": \"true\"},{\"AxHomeBuildAccess\": \"default\"},{\"AxMaxNumOfWidgets\": \"99\"},{\"AxMenuStyle\": \"default\"},{\"AxMenuColumns\": \"3\"},{\"AxSubmenuPerView\": \"6\"},{\"AxMenuWordWrap\": \"false\"},{\"AxSubMenuCount\": \"4\"},{\"AxDirectSubMenuCount\": \"6\"},{\"AxWizardType\": \"classic\"},{\"AxOTPAuth\": \"false\"},{\"AxOTPAuthCahrs\": \"3\"},{\"AxOTPAuthExpiry\": \"1\"},{\"AxDrafts\": \"false\"},{\"AxAutoPurge\": \"true\"},{\"AxMaxDraftsCount\": \"1\"},{\"AutoSavePublish\" : \"false\"}],\"configLangKeys\": [{\"AxAppTitle_ENG\": \"\"},{\"AxCopyRightText_ENG\": \"Powered by Axpert\"},{\"AxPrintTitle_ENG\": \"\"}]}";


    #region Fast Data
    public const string FD_TBL_DEFINITION = "select * from axp_FastDataDefinition";
    public const string FD_TBL_ASSOCIATION = "select * from axp_fastdataassociation";
    public const string FD_DT_DEFINITION = "FdDtDefinition";
    public const string FD_DT_ASSOCIATION = "FdDtAssociation";
    public const string RS_FD_DEFINITION = "RsFdDefinition";
    public const string FD_REFRESH_DT = "FDRefreshDT";
    #endregion

    //public const string IMPEXP_GETTSTUCTS = "select name,caption from tstructs where blobno=1 and name in (SELECT SNAME FROM AXUSERACCESS WHERE STYPE = 't' AND RNAME IN (select userroles from axuserlevelgroups ul join axusergroups ug on ul.usergroup= ug.groupname where ul.username = '{0}')) order by lower(caption)";

    public const string IMPEXP_GETTSTRUCTS = "select distinct t.name, t.caption from tstructs t left outer join AXUSERACCESS a on a.sname=t.name and  a.stype='t' where (('default' in (select userroles from axuserlevelgroups ul join axusergroups ug on ul.usergroup=ug.groupname where ul.username = '{0}')) or a.rname in (select userroles from axuserlevelgroups ul join axusergroups ug on ul.usergroup=ug.groupname where ul.username = '{0}')) order by 2";
    public const string SQL_GET_AXLANGSOURCE = "Select distinct langname from axlangsource order by langname asc";

    public const string AXPATTACHMENTPATH = "axpattachmentpath";
    public const string DESIGN_MODE_BTN_HTML = "<li><a href=\"javascript:void(0)\" id='design' title='Design Mode' class='handCur' onclick='goToDesignMode()'>Design Mode</a></li>";
    public const string DESIGN_MODE_BTN_HTML_NEW = "<a href=\"javascript:void(0)\" id='design' title='Design Mode' onclick='goToDesignMode()' class=\"dwbBtn btn btn-white btn-sm btn-color-gray-600 btn-active-primary shadow-sm me-2\"><i class='material-icons'>domain</i><span class='tbCaption'>Design Mode</span></a>";


    public const string WIZARD_TEMPLATE = "<div id=\"wizardTstructWrapper\" style=\"position:fixed; top:0px; left:0px; width:100%; height:100%; background-color:white; z-index:999;display: block; /*overflow-x: hidden; overflow-y: auto;*/\"></div>";

    public const string TBL_AXRELATIONS = "select mstruct,dstruct,mfield,dfield from axrelations where rtype='sp' or rtype='md'";
    public const string RS_AXRELATIONS = "RS_AxRelations";
    public const int DEFAULT_DRAFTS_RETAIN_COUNT = 3;
    public const string UNIQUE_DRAFT_KEY_PHRASE = "-sad-";

    //Config Page
    public const string CONFIG_LOCK_KEY = "ConfigLock";
    public const string CONFIGAPP_JSON_KEY = "ConfigAppJSON";
    public const string AXAPPS_XML_KEY = "AxAppsXML";
    public const string AXARM_CONN_KEY = "AxARMJSON";
    public const string AXARM_ExtRes_CONN_KEY = "AxARMExtResJSON";
    public const string AXARM_IntRes_CONN_KEY = "AxARMIntResJSON";
    public const string AXFileServer_CONN_KEY = "AxFileServerJSON";
    public const string AXARM_RMQSettings = "AxRMQSettings";
    public const string AXSSO_CONN_KEY = "AxSSOJSON";
    public const string AXEMAILSMTP_CONN_KEY = "AxEmailSMTPJSON";
    public const string AXPASSWORDPOL_CONN_KEY = "AxPwdPolicy";
    public const string AXPMOBSETTING_CONN_KEY = "AxMobSetting";

    //Axpert Web Logs
    public const string SQL_GET_AXPWEBLOGS = "SELECT USERNAME, LOGTIME, SESSIONID, IPADDRESS, LOGDETAILS FROM AXPWEBLOGS WHERE TYPE = '$Type$'";
    public const string SQL_SET_AXPWEBLOGS = "INSERT INTO AXPWEBLOGS(USERNAME, LOGTIME, TYPE, SESSIONID, IPADDRESS, LOGDETAILS) VALUES ('$Username$', '$Logtime$', '$Type$', '$Sessionid$', '$Ipaddress$', '$Logdetails$')";

    // public const string SQL_GET_AXPCONFIGS = "select * from (select A.STYPE, A.STRUCTNAME, A.CONTEXT, A.SFIELD, A.ICOLUMN, A.SBUTTON, A.HLINK, A.PROPS, A.PROPSVAL from axpstructconfig a, axuserlevelgroups b where A.USERROLES = axusergroup and B.AXUSERNAME = '$username$' union all select A.STYPE, A.STRUCTNAME, A.CONTEXT, A.SFIELD, A.ICOLUMN, A.SBUTTON, A.HLINK, A.PROPS, A.PROPSVAL from  axpstructconfig a where nvl( USERROLES,'default')= 'default') where stype = '$stype$' and structname = '$transid$'";
    //public const string SQL_GET_AXPCONFIGS = "select * from (select  A.STYPE,A.STRUCTNAME, A.CONTEXT, A.SFIELD,A.PROPS, A.PROPSVAL from axpstructconfig a, axuserlevelgroups b where A.USERROLES = axusergroup and B.AXUSERNAME = '$username$' union all select  A.STYPE,A.STRUCTNAME, A.CONTEXT, A.SFIELD,A.PROPS, A.PROPSVAL from  axpstructconfig a where upper(nvl( USERROLES,'ALL'))= 'ALL') where stype = '$stype$' and structname = '$transid$'";
    public const string SQL_GET_AXPCONFIGS = "SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$'  AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE NVL (USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE, '$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%' UNION SELECT A.STYPE,'$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE NVL(USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%'";
    public const string SQL_GET_AXPCONFIGS_MYSQL = "SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$'  AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE ifnull(USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE, '$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%' UNION SELECT A.STYPE,'$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE ifnull(USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%'";
    public const string SQL_GET_AXPCONFIGS_MSSQL = "SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$'  AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE isnull(USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE, '$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%' UNION SELECT A.STYPE,'$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE isnull(USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%'";
    public const string SQL_GET_AXPCONFIGS_POSTGRESQL = "SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$'  AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE,A.STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE COALESCE(USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND structname = '$transid$' AND A.STRUCTNAME NOT LIKE 'ALL%' UNION SELECT A.STYPE, '$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a, axuserlevelgroups b WHERE A.USERROLES = axusergroup AND B.AXUSERNAME = '$username$' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%' UNION SELECT A.STYPE,'$transid$' STRUCTNAME,A.CONTEXT,A.SFIELD,A.ICOLUMN,A.SBUTTON,A.HLINK,A.PROPS,A.PROPSVAL FROM axpstructconfig a WHERE COALESCE(USERROLES, 'ALL') = 'ALL' AND stype = '$stype$' AND A.STRUCTNAME LIKE 'ALL%'";
    public const string SQL_GET_TSTRUCTDESIGN = "select * from $tblname$ where lower(transid)=lower('$transid$') and module='$mod$' order by created_on desc";
    public const string SQL_GET_OLDTSTRUCTDESIGN = "select value from axpclouddevsettings where type='axDesign' and lower(TRANID)=lower('$transid$')";
    public const string SQL_GET_AXPGENCONFIGS = "select props,asprops,propsval,structname,stype,userroles,propvalue2 from axpstructconfig where props='General'";

    public const string SQL_GET_SP = "select pageno , caption, accessstring, spsavejson  from axpdef_stdpages where pageno='$NAME$'";

    //Adding for App config
    public const string STRUCTTYPE_TSTRUCT = "Tstruct";
    public const string STRUCTTYPE_IVIEW = "Iview";
    public const string CONFIGTYPE_DESIGN = "design";
    public const string CONFIGTYPE_CONFIGS = "configs";
    public const string AXCONFIGURATIONS = "axpconfigurations";
    public const string AXCONFIGURATIONTABLE = "axpConfigTable";
    public const string AXOLDDESIGN = "axOldDesign";
    public const string AXLOGGEDUSER = "axloggedUser";
    public const string AXPAGETITLE = "PageTitle";
    public const string AXHYBRIDINFO = "GetHybridInfo";
    public const string AXHYBRIDNOTIFIINFO = "GetHybridNotifiInfo";
    public const string AXHYBRIDWEIGHTSCALEINFO = "HybridWeightScaleInfo";
    public const string AXCONFIGTSTRUCT = "Tstruct";
    public const string AXCONFIGIVIEW = "Iview";
    public const string AXCONFIGGENERAL = "General";
    public const string AXNODATACONFIGTSTRUCT = "Tstruct";
    public const string AXNODATACONFIGIVIEW = "Iview";
    public const string AXNODATACONFIGGENERAL = "General";
    public const string NOTIFICATION_PHASE = "-notify-";
    public const string SQL_GET_WFPDCOMMENTS = "select wfcomments from ax_wfcomments where wfid='$wfid$' and wfaction='$actiontype$' and stransid='$tid$'";
    public const string SQL_GET_EMAIL = "select EMAIL,ACTIVE,NICKNAME from axusers where USERNAME ='$USERNAME$'";

    public const string LBLHL_GETTSTRUCTS = "select caption,ntransid as name from axpdef_tstruct order by caption";
    public const string LBLHL_GETIVIEWS = "select name,caption from dwb_iviews order by caption";
    public const string LBLHL_GETPAGES = "select pageno as name,caption from htmlsections";//"select name,caption from axpages";
    public const string LBLHL_GETTSTRUCTFIELDS = "select caption,fname AS name from axpflds where tstruct=$stransid$";// "select caption,name from coretstructhdr where STRANSID=$stransid$";
    public const string LBLHL_GETIVIEWPARAMS = "select pname from iviewparams where iname=$iname$";// "select pname from dwb_iviewparams where iname=$iname$";

    public const string AXHYBRID_GETUSERDEVICEID = "select imei_no as deviceid from ax_mobilenotify where username = '$USERNAME$' and projectname = '$PROJECTNAME$'";

    //public const string SQL_GET_AXUSERLANG = "select axlang,pwdauth,otpauth from axusers where username='$USERNAME$'";
    public const string SQL_GET_AXUSERLANG = "select axlang,pwdauth,otpauth,email,mobile,active from axusers where username='$USERNAME$'";

    public const string IMP_GETTEMPLATES = "SELECT AXPDEF_IMPDATA_TEMPLATESID,TEMPLATENAME, TEMPLATECAP, IMPFIELDS, DATAUPD, FLDPKEY FROM AXPDEF_IMPDATA_TEMPLATES WHERE STRANSID = '$STRANSID$'";
    public const string IMP_DELTEMPLATES = "DELETE FROM AXPDEF_IMPDATA_TEMPLATES WHERE STRANSID = '$STRANSID$' AND TEMPLATENAME = '$TEMPLATENAME$'";
    public const string AXNOTIFYHISTORY = "AxNotifyHistory";

    public const string REDISOTPAUTH = "otpauth";
    public const string REDISPWDOTPAUTHLANG = "pwdotpauthlang";
    public const string AxAPIField_SqlData = "select sqlparams from Axdirectsql where sqlname=$sqlName$;";
    public const string REDISFREEZEGRDCOLS = "FreezeGridCols";
    public const string REDISTSTHTMLLS = "tstHtmlLs";
    public const string REDISTSTHTMLLSMob = "tstHtmlLsMob";
    public const string REDISTSTHTMLLSGLOBALVARS = "tstHtmlLsGlbVars";
    public const string REDISTSTRUCTDOPTIME = "TstflDopTime";

    public static string[] globalVarPathVaribles = { "axp_apipath", "axpdbdirpath", "axscriptsapiurl", "axsignalrapiurl", "axrmqapiurl", "axrapidsaveurl", "axfcmsendmsgurl", "applogo", "axp_apipath", "auth_path", "axpimageserver", "axattachfilepath" };

    #region ARM KEYS
    public const string REDISARMPERMISSION = "ARM-AX-*-PERMISSION";
    public const string REDISARMMETADATA = "ARM-AX-*-MetaData";
    public const string REDISARMCONNECTEDDATAMETADATA = "ARM-AX-DATAPAGE-ConnectedData";
    public const string REDISARMCONNECTEDDATAPERMISSION = "ARM-AX-CONNECTEDDATA-PERMISSION";

    public const string REDISARMAXADSSQL = "AxADSSQL";
    public const string REDISARMAXADSDATA = "AxADSData";

    public const string REDISARMAXCARDSLIST = "AxCardsList";
    #endregion

    public const string SQL_Bulk_User_Activation = "select username, case when portaluser='T' then 'p' else null end usertype from axusers WHERE username != 'admin' AND (authkey IS NULL OR authkey = '') AND (userkey IS NULL OR userkey = '')";
    public const string SQL_InActive_User_Count = "SELECT COUNT(*) AS InactiveCount FROM axusers WHERE username != 'admin' AND (authkey IS NULL OR authkey = '') AND (userkey IS NULL OR userkey = '')";
    public const string REDISBULKUSERACTIVATE = "BulkUserActivate";
}
