using System;

public partial class aspx_Help : System.Web.UI.Page
{
    public string language = string.Empty;
    public string direction = "ltr";
    public string db_type = String.Empty;
    public string db_connection = String.Empty;
    public string file_url = String.Empty;
    public string langType = "en";
    static LogFile.Log logObj = new LogFile.Log();
    static Util.Util util = new Util.Util();
    static ASBExt.WebServiceExt objWebServiceExt = new ASBExt.WebServiceExt();
    static public string transid = string.Empty;
    static public string recid = string.Empty;
    bool registration = false; 

    protected override void InitializeCulture()
    {
        if (Session["language"] != null)
        {
            util = new Util.Util();
            string dirLang = string.Empty;
            dirLang = util.SetCulture(Session["language"].ToString().ToUpper());
            if (!string.IsNullOrEmpty(dirLang))
            {
                direction = dirLang.Split('-')[0];
                langType = dirLang.Split('-')[1];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["link"] != null && !string.IsNullOrEmpty(Request.QueryString["link"]))
            {
                file_url = Request.QueryString["link"];

                if (!string.IsNullOrEmpty(file_url))
                    ltHTMLhelper.Text = "<iframe id=\"docViewer\" frameborder=\"0\" src=\"https://docs.google.com/viewerng/viewer?url=" + file_url + "&embedded=true\" style=\"overflow:hidden; width:100%\" height=\"90vh\"></iframe>"; 
                else
                    ltHTMLhelper.Text = "<h3>Invalid File Path</h3>"; 
            }
        }
    }
}

