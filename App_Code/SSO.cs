using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SSO
/// </summary>
public interface ISSO
{

    //<add key = "Facebook_url" value="https://graph.facebook.com/oauth/authorize?client_id={0}&amp;redirect_uri={1}&amp;scope={2}"/>
    //<add key = "FacebookOAuthurl" value="https://graph.facebook.com/oauth/access_token?client_id={0}&amp;redirect_uri={1}&amp;scope={2}&amp;code={3}&amp;client_secret={4}"/>
    //<add key = "FacebookAccessUrl" value="https://graph.facebook.com/me?fields=first_name,last_name,gender,locale,link,email&amp;access_token="/>


    string url { get; set; }
    string redirectUrl { get; set; }
    string clientID { get; set; }
    string clientSecret { get; set; }
    string scope { get; set; }
    string OAuthURL { get; set; }
    string AccessUrl { get; set; }
    string provider { get; set; }

    string GetParameters(string code);

    string GetRequestURL();



}


public class Google
{

    public class Request : ISSO
    {
        //        <!--Google Settings-->
        //<add key = "google_client_id" value="493358977849-so1sc0tcs9gqrh4jiuaqs36jr9cns0cv.apps.googleusercontent.com"/>
        //<add key = "google_client_secret" value="ChzG0QxESuMZkQ--mStkSj9z"/>
        //<add key = "google_redirect_url" value="http://localhost:56647/aspx/signinnew.aspx"/>
        //<add key = "Googleurl" value="https://accounts.google.com/o/oauth2/auth?response_type=code&amp;redirect_uri={0}&amp;scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&amp;client_id={1}"/>
        //<add key = "googleoAuthUrl" value="https://accounts.google.com/o/oauth2/token"/>
        //<add key = "googleoAccessUrl" value="https://www.googleapis.com/oauth2/v1/userinfo?access_token="/>


        public string OAuthURL { get; set; }
        public string AccessUrl { get; set; }

        public string url { get; set; }
        public string redirectUrl { get; set; }
        public string clientID { get; set; }
        public string scope { get; set; }
        public string clientSecret { get; set; }
        public string provider { get; set; }

        public Request()
        {

            url = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri={0}&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id={1}";
            //redirectUrl = "http://localhost:56647/aspx/signinnew.aspx";
            //clientID = "493358977849-so1sc0tcs9gqrh4jiuaqs36jr9cns0cv.apps.googleusercontent.com";
            //clientSecret = "ChzG0QxESuMZkQ--mStkSj9z";
            provider = "google";
        }

        public string GetParameters(string code)
        {
            string parameters = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code",
                       code,
                       clientID,
                     clientSecret,
                      redirectUrl);

            return parameters;

        }


        public string GetRequestURL()
        {
            return String.Format(url, redirectUrl, clientID);
        }
    }
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
    public class Info
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }
        public string gender { get; set; }
    }

}

public class Facebook
{
    public class Request : ISSO
    {
        public string url { get; set; }
        public string redirectUrl { get; set; }
        public string clientID { get; set; }
        public string scope { get; set; }
        public string clientSecret { get; set; }
        public string OAuthURL { get; set; }
        public string AccessUrl { get; set; }
        public string provider { get; set; }


        public Request()
        {

            url = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}";
            OAuthURL = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}";
            //clientID = "252601612604428";
            //clientSecret = "2bdba09e6ba0e2f47225487344153b59";
            scope = "email, public_profile";
            //redirectUrl = "" + HttpContext.Current.Request.Url.AbsoluteUri + "";
            AccessUrl = "https://graph.facebook.com/me?fields=first_name,last_name,gender,locale,link,email&amp;access_token=";

            provider = "facebook";
        }

        public string GetParameters(string code)
        {
            string parameters = string.Format(OAuthURL,
                    clientID,
                  redirectUrl,
                    scope,
                   code,
                    clientSecret);



            return parameters;

        }


        public string GetRequestURL()
        {

            //string Facebookurl = string.Format(ConfigurationManager.AppSettings["Facebook_url"], ConfigurationManager.AppSettings["Facebook_AppId"], ConfigurationManager.AppSettings["Facebook_RedirectUrl"], ConfigurationManager.AppSettings["Facebook_scope"]); 
            return String.Format(url, clientID, redirectUrl, scope, "", clientSecret);
        }
    }
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
    public class Info
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }
        public string gender { get; set; }
    }
}


//public class Office365
//{

//    public class Request : ISSO
//    {
//        public string OAuthURL { get; set; }
//        public string AccessUrl { get; set; }

//        public string url { get; set; }
//        public string redirectUrl { get; set; }
//        public string clientID { get; set; }
//        public string scope { get; set; }
//        public string clientSecret { get; set; }
//        public string provider { get; set; }

//        public Request()
//        {

//            url = "";
//            redirectUrl = "http://localhost:56647/aspx/signinnew.aspx";//"" + HttpContext.Current.Request.Url.AbsoluteUri.ToLower() + "";//
//            clientID = "";
//            clientSecret = "";
//            provider = "office365";

//            //AuthenticationContext authContext = new AuthenticationContext("https://login.microsoftonline.com/common");

//            //// The same url we specified in the auth code request
//            //string redirectUri = "http://localhost:55065/aspx/signinnew.aspx";

//            //// Use client ID and secret to establish app identity
//            //ClientCredential credential = new ClientCredential(ConfigurationManager.AppSettings["ClientID"], ConfigurationManager.AppSettings["ClientSecret"]);

//            //try
//            //{
//            //    // Get the token
//            //    var authResult = authContext.AcquireTokenByAuthorizationCode(
//            //        authCode, new Uri(redirectUri), credential, "https://graph.microsoft.com/");

//            //    // Save the token in the session
//            //    Session["access_token"] = authResult.AccessToken;


//            //    Response.Redirect(redirectUri.ToString());
//            //}
//            //catch (AdalException ex)
//            //{
//            //    //return Content(string.Format("ERROR retrieving token: {0}", ex.Message));
//            //}
//        }

//        public string GetParameters(string code)
//        {
//            string parameters = string.Format(code,
//                       clientID,
//                     clientSecret,
//                      redirectUrl);

//            return parameters;

//        }


//        public string GetRequestURL()
//        {
//            return String.Format(url, redirectUrl, clientID);
//        }
//    }
//    public class Token
//    {
//        public string access_token { get; set; }
//        public string token_type { get; set; }
//        public int expires_in { get; set; }
//        public string id_token { get; set; }
//        public string refresh_token { get; set; }
//    }
//    public class Info
//    {
//        public string id { get; set; }
//        public string email { get; set; }
//        public bool verified_email { get; set; }
//        public string name { get; set; }
//        public string given_name { get; set; }
//        public string family_name { get; set; }
//        public string picture { get; set; }
//        public string locale { get; set; }
//        public string gender { get; set; }
//    }

//}


public class SSO
{
    public static ISSO ObjectHelper(string provider)
    {
        switch (provider)
        {
            case "google":
                return new Google.Request();
            case "facebook":
                return new Facebook.Request();
            case "openid":
                return new OpenId.Request();

            default: return null;
        }
    }
}

public class OpenId
{
    public class Request : ISSO
    {
        public string url { get; set; }
        public string redirectUrl { get; set; }
        public string clientID { get; set; }
        public string scope { get; set; }
        public string clientSecret { get; set; }
        public string OAuthURL { get; set; }
        public string AccessUrl { get; set; }
        public string provider { get; set; }

        public Request()
        {
            string ssoopeniddomain = string.Empty; // ConfigurationManager.AppSettings["ssoopeniddomain"].ToString();
            string ssoredirecturl = string.Empty;// ConfigurationManager.AppSettings["ssoredirecturl"].ToString();
            if (HttpContext.Current.Session["openiddomain"] != null && HttpContext.Current.Session["openiddomain"].ToString() != "")
                ssoopeniddomain = HttpContext.Current.Session["openiddomain"].ToString();
            if (HttpContext.Current.Session["OpenIdssoredirecturl"] != null && HttpContext.Current.Session["OpenIdssoredirecturl"].ToString() != "")
                ssoredirecturl = HttpContext.Current.Session["OpenIdssoredirecturl"].ToString();
            url = "https://" + ssoopeniddomain + "/authorize?client_id={1}&redirect_uri=" + ssoredirecturl + "aspx/signin.aspx&scope=openid profile email phone address&response_type=code&state=";
            provider = "openid";
        }

        public string GetParameters(string code)
        {
            string parameters = string.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}",
                       code,
                       clientID,
                     clientSecret,
                      redirectUrl);

            return parameters;
        }

        public string GetRequestURL()
        {
            return String.Format(url, redirectUrl, clientID);
        }
    }
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
    public class Info
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }
        public string gender { get; set; }
    }
}

