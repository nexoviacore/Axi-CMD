using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aspx_AxMainAuth : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string encAuth = string.Empty;

            if (Request.QueryString["encAuth"] != null)
                encAuth = Request.QueryString["encAuth"].ToString();

            if (encAuth != string.Empty)
            {
                System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
                string cookieName = sessionStateSection.CookieName;

                HttpCookie mycookie = new HttpCookie(cookieName);
                mycookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(mycookie);
                System.Web.HttpContext.Current.Session.Abandon();

                SessionIDManager manager = new SessionIDManager();
                manager.RemoveSessionID(System.Web.HttpContext.Current);
                var newId = manager.CreateSessionID(System.Web.HttpContext.Current);
                var isRedirected = true;
                var isAdded = true;
                manager.SaveSessionID(System.Web.HttpContext.Current, newId, out isRedirected, out isAdded);

                Response.Redirect("AxMain.aspx?encAuth=" + encAuth);
            }
        }
    }
}