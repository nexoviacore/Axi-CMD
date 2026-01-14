using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Linq;
using System.Web.Caching;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Web.Security;
using System.Configuration;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;

public partial class aspx_excelimportgrid : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    LogFile.Log logobj = new LogFile.Log();
    public string appTitle = string.Empty;
    public string direction = "ltr";
    public string langType = "en";
    string language = string.Empty;

    protected override void InitializeCulture()
    {
        if (ConfigurationManager.AppSettings["proj"] != null && ConfigurationManager.AppSettings["proj"].ToString() != string.Empty)
        {
            language = LoadLanguages();
            string dirLang = string.Empty;
            dirLang = util.SetCulture(language.ToUpper());
            if (!string.IsNullOrEmpty(dirLang))
            {
                direction = dirLang.Split('-')[0];
                langType = dirLang.Split('-')[1];
            }
            FileInfo filcustom = new FileInfo(HttpContext.Current.Server.MapPath("~/Js/lang/content-" + langType + ".js"));
            if (!(filcustom.Exists))
            {
                langType = "en";
                direction = "ltr";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string LoadLanguages()
    {
        string language = string.Empty;
        if (ConfigurationManager.AppSettings["proj"] != null && ConfigurationManager.AppSettings["proj"] != string.Empty)
        {
            string proj = ConfigurationManager.AppSettings["proj"].ToString();
            language = util.GetConfigAttrValue(proj, "AxLanguages");
        }
        return language;
    }
}