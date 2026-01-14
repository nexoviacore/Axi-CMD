using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for AntiforgeryChecker
/// </summary>
public static class AntiforgeryChecker
{
    public static void Check(Page page, HiddenField antiforgery)
    {
        if (!page.IsPostBack)
        {
            Guid antiforgeryToken = Guid.NewGuid();
            page.Session["AntiforgeryToken"] = antiforgeryToken;
            antiforgery.Value = antiforgeryToken.ToString();
        }
        else
        {
            Guid stored;
            try
            {
                stored = (Guid)page.Session["AntiforgeryToken"];
            }
            catch (Exception ex)
            {
                Guid antiforgeryToken = Guid.NewGuid();
                page.Session["AntiforgeryToken"] = stored = antiforgeryToken;
                antiforgery.Value = antiforgeryToken.ToString();
            }
            Guid sent = new Guid(antiforgery.Value);
            if (sent != stored)
            {
                throw new SecurityException("XSRF Attack Detected!");
            }
        }
    }
}