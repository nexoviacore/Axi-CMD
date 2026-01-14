using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Util;
using ASBExt;
using LogFile;
using System.Xml;
using System.Web.UI.HtmlControls;

public partial class aspx_Drafts : System.Web.UI.Page
{
    Util.Util utilObj = new Util.Util();
    ArrayList fileNames = new ArrayList();
    ArrayList captions = new ArrayList();
    ArrayList names = new ArrayList();
    ArrayList tstCaptions = new ArrayList();
    LogFile.Log logObj = new LogFile.Log();
    public string direction = "ltr";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlLink Link = FindControl("generic") as HtmlLink;
        Link.Href = utilObj.toggleTheme();
        if (Request.UrlReferrer != null)
        {
           if (!(Request.UrlReferrer.AbsolutePath.ToLower().Contains("main.aspx") || Request.UrlReferrer.AbsolutePath.ToLower().Contains("drafts.aspx")))
            {
                Response.Redirect("../cusError/axcustomerror.aspx");
            }
            
        }
        if (Session["user"] == null || Session["project"] == null)
        {
            utilObj.IFrameSessExpiry();
        }
        GetDrafts();
    }
    public void GetDrafts()
    {
        utilObj.UpdateDraftArrays();
        fileNames = (ArrayList)HttpContext.Current.Session["draftFileNames"];
        captions = (ArrayList)HttpContext.Current.Session["draftCaptions"];
        string draftsHtml = utilObj.CreateDraftsHtml(fileNames, captions);
        dvDrafts.InnerHtml = draftsHtml;
        if (Session["language"].ToString() == "ARABIC")
        {
            direction = "rtl";
        }
    }

    protected void btnGetDrafts_Click(object sender, EventArgs e)
    {
        //GetDrafts();
    }
}
