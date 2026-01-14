using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_Reconnect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
        try
        {
            string inputXml = string.Empty;
            string scriptsPath = HttpContext.Current.Application["ScriptsPath"].ToString();
            LogFile.Log logobj = new LogFile.Log();
            string errorLog = logobj.CreateLog("Continue session - ", HttpContext.Current.Session["nsessionid"].ToString(), "ContinueCurrentSession", "new");
            inputXml = "<root axpapp = '" + HttpContext.Current.Session["project"].ToString() + "' username='" + HttpContext.Current.Session["username"].ToString() + "' sessionid = '" + HttpContext.Current.Session["nsessionid"].ToString() + "' appsessionkey = '" + HttpContext.Current.Session["AppSessionKey"].ToString() + "' trace = '" + errorLog + "'  scriptpath='" + scriptsPath + "'> ";
            inputXml += HttpContext.Current.Session["axApps"].ToString() + HttpContext.Current.Application["axProps"].ToString() + HttpContext.Current.Session["axGlobalVars"].ToString() + HttpContext.Current.Session["axUserVars"].ToString() + "</root>";
            ASBExt.WebServiceExt objwse = new ASBExt.WebServiceExt();
            objwse.CallContinueCurrentSessionWS(inputXml);
        }
        catch (Exception ex)
        { }
    }
}
