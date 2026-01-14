using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json.Linq;


public partial class aspx_AboutUs : System.Web.UI.Page
{
    Util.Util util = new Util.Util();
    public string direction = "ltr";
    public string langType = "en";
    string project = string.Empty;
    string db = string.Empty;
    string applicationServer = string.Empty;

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
        //to display project
        project = Session["project"] != null ? Session["project"].ToString() : string.Empty;//check if project is connected or not
        if (project == string.Empty)
        {
            SessionExpired();
            return;
        }
        else
            divprojcontent.Text = project;

        //to display database name
        db = Session["axdb"] != null ? Session["axdb"].ToString() : string.Empty;
        if (db != string.Empty)
            divdbconent.Text = db;

        //to check Redis connected or not
        FDW fdwObj = new FDW();
        bool isRedisConnected = HttpContext.Current.Session["RedisIsConnected"] != null ? bool.Parse(HttpContext.Current.Session["RedisIsConnected"].ToString()) : false;
        if (isRedisConnected)
            divdatacontent.Text = "Redis";
        else
            divdatacontent.Text = "Not Available";

        string subversion = string.Empty, features = string.Empty, description = string.Empty, enhancements = string.Empty, bugsFixed = string.Empty;
        string jsoncontent = util.ReadFromFile(Server.MapPath("../versioninfo.json"));
        var details = JObject.Parse(jsoncontent);
        var version = ((JValue)(details["version"])).Value;

        if (details["subVersion"] != null)
            subversion = ((JValue)(details["subVersion"])).Value.ToString().ToLower();
        else
            subversion = version.ToString().ToLower();

        //to display version/version_subversion
        lblVersion.Text = (!string.IsNullOrEmpty(subversion) && subversion != version.ToString()) ? version.ToString() + "_" + subversion : version.ToString();

        if (subversion != String.Empty)
        {
            if (details[version] != null && details[version][subversion] != null)
            {
                var detailsNode = details[version][subversion];

                var rdate = detailsNode["releaseDate"];
                if (rdate != null && rdate.ToString() != String.Empty)
                {
                    lblRelDate.Text = rdate.ToString();
                    divRelDate.Visible = true;
                }

                var featureList = detailsNode["features"];
                if (featureList != null && featureList.ToString() != String.Empty)
                {
                    if (((featureList.GetType())).Name == "JArray")
                        foreach (var item in detailsNode["features"])
                        {
                            if (((JValue)item).Value.ToString() != "")//check if features is null or not
                                features += "<li>" + item + "<br>" + "</li>";// display all features in bulleted list
                        }
                    else
                        features = featureList.ToString();

                    divFeaturesContent.InnerHtml = features;
                    if (features != string.Empty)
                        divFeat.Visible = true;
                }

                var descList = detailsNode["description"];
                if (descList != null && descList.ToString() != string.Empty)
                {
                    if (((descList.GetType())).Name == "JArray")
                        foreach (var item in descList)
                        {
                            if (((JValue)item).Value.ToString() != "")//check if description is null or not
                                description += "<li>" + item + "<br>" + "</li>";// display all description in bulleted list
                        }
                    else
                        description = descList.ToString();

                    divDescContent.InnerHtml = description;
                    if (description != string.Empty)
                        divDesc.Visible = true;
                }

                var enhancementList = detailsNode["enhancements"];
                if (enhancementList != null && enhancementList.ToString() != string.Empty)
                {
                    if (((enhancementList.GetType())).Name == "JArray")
                        foreach (var item in enhancementList)
                        {
                            if (((JValue)item).Value.ToString() != "")//check if enhancements is null or not
                                enhancements += "<li>" + item + "<br>" + "</li>";// display all enhancements in bulleted list
                        }
                    else
                        enhancements = enhancementList.ToString();

                    divEnhanContent.InnerHtml = enhancements;
                    if (enhancements != string.Empty)
                        divEnhan.Visible = true;
                }

                //var bugsList = detailsNode["bugsFixed"];
                //if (bugsList != null && bugsList.ToString() != string.Empty)
                //{
                //    if (((bugsList.GetType())).Name == "JArray")
                //        foreach (var item in bugsList)
                //        {
                //            if (((JValue)item).Value.ToString() != "")//check if bugsFixed is null or not
                //                bugsFixed += "<li>" + item + "<br>" + "</li>";// display all bugsFixed in bulleted list
                //        }
                //    else
                //        bugsFixed = bugsList.ToString();

                //    divBugContent.InnerHtml = bugsFixed;
                //    if (bugsFixed != string.Empty)
                //        divBug.Visible = true;
                //}

            }
        }
    }

   
    public void SessionExpired()
    {
        string url = util.SESSEXPIRYPATH;
        Response.Write("<script language='javascript'>");
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write("</script>");
    }
}


