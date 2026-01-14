using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using System.Configuration;

public partial class axpertAPI : System.Web.UI.Page
{
    #region variable
    public string direction = "ltr";
    Util.Util util;
    public string langType = "en";
    LogFile.Log logobj = new LogFile.Log();
    ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
    public string sessionID = string.Empty;
    #endregion
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
        if (HttpContext.Current.Session["Project"] == null || Convert.ToString(HttpContext.Current.Session["Project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        if (!IsPostBack)
        {
            sessionID = HttpContext.Current.Session["nsessionid"].ToString();
            loadParamIvewList();
            if (ConfigurationManager.AppSettings["scriptsUrlPath"] != null && ConfigurationManager.AppSettings["scriptsUrlPath"].ToString() != string.Empty)
            {
                hdnScriptPath.Value = ConfigurationManager.AppSettings["scriptsUrlPath"];
            }
        }

    }
    private void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        Response.Write("<script>" + Constants.vbCrLf);
        Response.Write("parent.parent.location.href='" + url + "';");
        Response.Write(Constants.vbCrLf + "</script>");

    }
    public void loadParamIvewList()
    {
        string result = string.Empty;
        JObject jobj = new JObject();
        string sql = @"SELECT DISTINCT iname FROM iviewparams";
        // sql = "select c.iname,c.pname,c.pcaption from iviewparams c group by c.iname,c.pname,c.pcaption";
        try
        {
            result = asbExt.ExecuteSQL("", sql, "JSON");

        }
        catch (Exception ex) { }
        try
        {
            JObject resultJSON = JObject.Parse(result);
            string colName = resultJSON["result"]["fields"][0]["name"].ToString();
            foreach (JObject obj in resultJSON["result"]["row"])
            {
                hdnParamIviewList.Value += obj.GetValue(colName).ToString() + ",";
            }
        }
        catch(Exception ex) { }
       
     
    }
    public void loadDropDownLitItems(string sql, ref DropDownList ddControl)
    {
        string result = string.Empty;
        JObject resultJSON = new JObject();
        try
        {
            result = asbExt.ExecuteSQL("", sql, "JSON");

        }
        catch (Exception ex) { }
        try
        {
            resultJSON = JObject.Parse(result);
            string col1 = resultJSON["result"]["fields"][0]["name"].ToString();
            string col2 = resultJSON["result"]["fields"][1]["name"].ToString();
            foreach (JObject obj in resultJSON["result"]["row"])
            {
                ddControl.Items.Add(new ListItem(obj.GetValue(col2).ToString(), obj.GetValue(col1).ToString()));
            }
        }
        catch (Exception ex) { }
    }

    protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlActions.SelectedIndex == 0)
        {
            ddlIview.Enabled = false;
            ddlTstruct.Enabled = false;
            ddlSQL.Enabled = false;
        }
        string action = ddlActions.SelectedValue.ToLower();
        string sql = string.Empty;

        switch (action)
        {
            case "login":
                ddlIview.Enabled = false;
                ddlTstruct.Enabled = false;
                ddlSQL.Enabled = false;
                break;
            case "submit data":
                ddlIview.Enabled = false;
                ddlTstruct.Enabled = true;
                ddlSQL.Enabled = false;
                sql = "select a.name,a.caption as text from tstructs a order by a.caption";
                ddlTstruct.Items.Clear();
                ddlTstruct.Items.Add("--Select Form--");
                loadDropDownLitItems(sql, ref ddlTstruct);
                break;
            case "get data":
                ddlIview.Enabled = true;
                ddlTstruct.Enabled = false;
                ddlSQL.Enabled = true;
                apiUrl.InnerHtml = "<b>URL : </b>  api/getiview/";
                sql = "select a.name,a.caption as text from iviews a order by a.caption"; ;
                ddlIview.Items.Clear();
                ddlIview.Items.Add("--Select Iview--");
                loadDropDownLitItems(sql, ref ddlIview);
                //sql = "select qname as text,sql_editor_text as name from axp_customesql";
                //ddlSQL.Items.Clear();
                //ddlSQL.Items.Add("--Select SQL results--");
                //loadDropDownLitItems(sql, ref ddlSQL);
                break;

        }
    }
    public void clearFields()
    {

    }

    protected void ddlTstruct_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTstruct.SelectedIndex != 0)
        {
            string sql = "select a.fname,a.dcname from axpflds a where a.tstruct='" + ddlTstruct.SelectedValue + "'";
            string tstFieldjson = string.Empty;
            string tstDCInfo = string.Empty;

            try
            {
                tstFieldjson = asbExt.ExecuteSQL("", sql, "JSON");
                sql = "SELECT a.dname,a.asgrid FROM axpdc a where a.tstruct = '" + ddlTstruct.SelectedValue + "' order by a.dname";
                tstDCInfo = asbExt.ExecuteSQL("", sql, "JSON");
            }
            catch (Exception ex) {
                LogFile.Log logObj = new LogFile.Log();
                string sessID = Constants.GeneralLog;
                if (HttpContext.Current.Session != null)
                    sessID = HttpContext.Current.Session.SessionID;
                logObj.CreateLog("ExecuteSql -" + ex.Message, sessID, "ExecuteSql", "new");
            }

            ScriptManager.RegisterStartupScript(this.Page, GetType(), "createSubmitDataAPIinfo", "createSubmitDataAPIinfo('" + tstFieldjson + "','" + tstDCInfo + "');", true);
        }

    }
    protected void ddlSQL_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSQL.SelectedIndex != 0)
        {
           
        }
    }
    [WebMethod]
    public static string getIviewParams(string ivName)
    {

        ASBExt.WebServiceExt asbExt = new ASBExt.WebServiceExt();
        string sql = "SELECT a.pname,a.pcaption FROM iviewparams a WHERE a.iname = '" + ivName + "'";
        string result = "";
        try
        {
            result = asbExt.ExecuteSQL("", sql, "JSON");

        }
        catch (Exception ex) { }
        return result;
    }
}
