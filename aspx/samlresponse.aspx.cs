using System;
using System.Collections.Generic;
using System.Web.Security;
using Saml;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Configuration;
using System.Web;
using Newtonsoft.Json.Linq;

public partial class samlresponse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetSSOCerKeys(Request.QueryString["Project"]);
            string SamlCertificate = Session["SamlCertificate"].ToString();// ConfigurationManager.AppSettings["SamlCertificate"].ToString();
            string certificate = File.ReadAllText(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "/Saml_Certificates/" + SamlCertificate);
            Response samlResponse = new Response(certificate, Request.Form["SAMLResponse"]);
            if (samlResponse != null)
            {
                if (samlResponse.IsValid())
                {
                    //Session["LoggedInId"] = samlResponse.GetNameID();
                    //Session["SAML_USER_PROFILE"] = samlResponse.GetNameID();

                    Session["LoginWith"] = "saml";
                    Util.Util util = new Util.Util();
                    string qstr = "Project=" + Request.QueryString["Project"].ToString() + "&";
                    string wfenc = string.Empty;
                    bool isFromWF = false;
                    if (Request.QueryString["fromwf"] != null && Request.QueryString["fromwf"].ToString() == "true")
                    {
                        isFromWF = true;
                        wfenc = Request.QueryString["enc"] != null ? Request.QueryString["enc"].ToString() : "";
                    }
                    else
                    {
                        qstr += "AxLanguages=" + Request.QueryString["AxLanguages"].ToString() + "&";
                        qstr += "isMobDevice=" + Request.QueryString["isMobDevice"].ToString() + "&";
                        qstr += "staySignIn=" + Request.QueryString["staySignIn"].ToString() + "&";
                    }
                    string objectidentifier = samlResponse.GetCustomAttribute("http://schemas.microsoft.com/identity/claims/objectidentifier");
                    qstr += "code=" + objectidentifier + "*$*" + samlResponse.GetNameID();
                    qstr = util.encrtptDecryptAES(qstr);
                    string returnUrl = Session["SamlRedirectUrl"].ToString();// ConfigurationManager.AppSettings["ssoredirecturl"].ToString();
                    string targetUrl = string.Empty;
                    if (isFromWF)
                        targetUrl = returnUrl + "aspx/Workflownotification.aspx?res=" + qstr + "&enc=" + wfenc;
                    else
                        targetUrl = returnUrl + "aspx/signin.aspx?res=" + qstr;
                    Response.Redirect(targetUrl, false);
                }
                else
                {
                    Response.Redirect("~/aspx/Signin.aspx", false);
                }
            }
            else
            {
                Response.Redirect("~/aspx/Signin.aspx", false);
            }
        }
        catch (Exception ex)
        {
            Session.Remove("LoginWith");
            // In production application, we recommend logging the exception and redirecting the user to a generic error page.
            throw ex;
        }
    }

    protected void GetSSOCerKeys(string proj)
    {
        try
        {
            string _strProj = proj;
            FDR fdrObj = new FDR(_strProj);
            string SSOJsoncontent = fdrObj.StringFromRedis(Constants.AXSSO_CONN_KEY, _strProj);
            if (SSOJsoncontent != string.Empty)
            {
                JObject config = JObject.Parse(SSOJsoncontent);
                JObject samlObject = config["saml"] as JObject;
                if (samlObject != null)
                {
                    foreach (var property in samlObject.Properties())
                    {
                        if (property.Name == "SamlCertificate")
                        {
                            Session["SamlCertificate"] = property.Value.ToString();
                        }
                        if (property.Name == "SamlRedirectUrl")
                        {
                            Session["SamlRedirectUrl"] = property.Value.ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { }
    }
}

