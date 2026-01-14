using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class aspx_dashBoard : System.Web.UI.Page
{
    public string direction = "ltr";
    public string langType = "en";
    public string globalVars = string.Empty;
    public string langs = "";
    public string appsessionKey = string.Empty;
    public static string sid = string.Empty;
    Util.Util util = new Util.Util();

    protected override void InitializeCulture()
    {
        if (Session["language"] != null)
        {
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
        util.IsValidSession();
        //util.IsValidAxpertSession();
        ResetSessionTime();
        if (Session["AppSessionKey"] != null)
        {
            if (Session["lictype"] != null && Session["lictype"].ToString() == "limited")
                appsessionKey = "l~";
            else
                appsessionKey = "u~";
            appsessionKey += Session["AppSessionKey"].ToString();
        }

        globalVars = util.GetGlobalVarString();

        if (Session["language"] == null || Session["nsessionid"] == null || Convert.ToString(Session["language"]) == string.Empty || Session["project"] == null || Convert.ToString(Session["project"]) == string.Empty)
        {
            SessExpires();
            return;
        }
        else
        {
            //if (!util.licencedValidSessionCheck())
            //{
            //    HttpContext.Current.Response.Redirect(util.ERRPATH + Constants.SESSIONEXPMSG, false);
            //    return;
            //}
            if (!IsPostBack)
            {
                if (Session["language"] != null)
                {
                    langs = Session["language"].ToString();
                    if (Session["language"].ToString() == "ARABIC")
                    {
                        direction = "rtl";
                    }
                }

                sid = Session["nsessionid"].ToString();
                sid = util.CheckSpecialChars(sid);
            }
        }

        if (Session["backForwBtnPressed"] != null && !Convert.ToBoolean(Session["backForwBtnPressed"]))
        {
            util.UpdateNavigateUrl(HttpContext.Current.Request.Url.AbsoluteUri);
        }
        Session["backForwBtnPressed"] = false;

    }

    private void ResetSessionTime()
    {
        if (Session["AxSessionExtend"] != null && Session["AxSessionExtend"].ToString() == "true")
        {
            HttpContext.Current.Session["LastUpdatedSess"] = DateTime.Now.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "parent.ResetSession();", true);
        }
    }

    private static void SessExpires()
    {
        string url = Convert.ToString(HttpContext.Current.Application["SessExpiryPath"]);
        HttpContext.Current.Response.Write("<script>" + Constants.vbCrLf);
        HttpContext.Current.Response.Write("parent.parent.location.href='" + url + "';");
        HttpContext.Current.Response.Write(Constants.vbCrLf + "</script>");
    }

    [WebMethod]
    public static string GetDashBoardDetails() // this is main method for dash board, its contains 4 table values 
    {
        string sqlQuery = string.Empty;
        DataSet ds = new DataSet();
        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["user"] != null)
            {
                DBContext obj = new DBContext();
                ds = obj.GetMainPageDataDB(sqlQuery);
                //For Sorting the Table3 
                //DataTable dt = getSortedTable(ds.Tables[3]);
                //ds.Tables.RemoveAt(3);
                //ds.Tables.Add(dt); dt.TableName = "Table3";
                json = JsonConvert.SerializeObject(ds, Formatting.Indented);
            }
            else
            {
                SessExpires();
            }
        }
        catch (SqlException ex) // This will catch all SQL exceptions
        {
            return "getting exception from sql: " + ex.Message;
        }
        catch (Exception ex) // This will catch every Exception
        {
            return "getting exception in code: " + ex.Message;
        }
        return json;
    }

    private static DataTable getSortedTable(DataTable dt)
    {
        dt.DefaultView.Sort = "WIDGETTYPE DESC";
        return dt.DefaultView.ToTable();
    }

    [WebMethod]
    public static string GetDashBoardRightsideTable(string sqlQuery, string widgetId, string flts) // this one call for fetch the right table records 
    {
        DataSet ds = new DataSet();
        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["user"] != null)
            {
                DBContext obj = new DBContext();
                ds = obj.GetMainPageDBTableInline(sqlQuery, flts, widgetId);
                json = JsonConvert.SerializeObject(ds, Formatting.Indented);
            }
            else
            {
                SessExpires();
            }
        }
        catch (SqlException ex) // This will catch all SQL exceptions
        {
            return ex.Message;
        }
        catch (Exception ex) // This will catch every Exception
        {
            return ex.Message;
        }
        return json;
    }

    [WebMethod]
    public static string AddQL(string linkName, string linkUrl) // for insert quick link details, And using inline query 
    {
        string json = string.Empty;
        try
        {
            DBContext obj = new DBContext();
            DataSet ds = obj.InsertMainPageDBTableInline(linkName, linkUrl);
            if (ds.Tables.Count != 0)
            {
                return json = JsonConvert.SerializeObject(ds, Formatting.Indented);
            }
            else
            {
                return "Some error occured please try again later.";
            }
        }
        catch (SqlException ex) // This will catch all SQL exceptions
        {
            return "Some error occured please try again later: " + ex.Message;
        }
        catch (Exception ex) // This will catch every Exception
        {
            return "Some error occured please try again later: " + ex.Message;
        }
    }

    [WebMethod]
    public static string UpdateQL(string linkUrl, string linkname, string linkId) // for insert quick link details, And using inline query
    {
        try
        {
            DBContext obj = new DBContext();
            var sqlQuery = "update axquicklinks set linkurl ='" + linkUrl + "',linkname='" + linkname + "' where axquicklinksid =" + linkId + "";
            bool flag = obj.ExecuteSqlQueryInline(sqlQuery);
            if (flag == true)
            {
                return "Quicklink updated successfully";
            }
            else
            {
                return "Some error occured please try again later.";
            }
        }
        catch (SqlException ex) // This will catch all SQL exceptions
        {
            return "Some error occured please try again later: " + ex.Message;
        }
        catch (Exception ex) // This will catch every Exception
        {
            return "Some error occured please try again later: " + ex.Message;
        }
    }

    [WebMethod]
    public static string DeleteQL(string linkId) // for insert quick link details, And using inline query
    {
        try
        {
            DBContext obj = new DBContext();
            var sqlQuery = "delete from axquicklinks where axquicklinksid =" + linkId + "";
            bool flag = obj.ExecuteSqlQueryInline(sqlQuery);
            if (flag == true)
            {
                return "Quicklink deleted successfully";
            }
            else
            {
                return "Some error occured please try again later.";
            }
        }
        catch (SqlException ex) // This will catch all SQL exceptions
        {
            return "Some error occured please try again later: " + ex.Message;
        }
        catch (Exception ex) // This will catch every Exception
        {
            return "Some error occured please try again later: " + ex.Message;
        }
    }

    [WebMethod]
    public static string GetDashBoardCharts(string sqlQuery, string widgetId, string[] chartTypes, string flts)
    {
        DataSet ds = new DataSet();
        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["user"] != null)
            {
                DBContext obj = new DBContext();
                ds = obj.GetMainPageDBTableInline(sqlQuery, flts, widgetId);
                json = highChart(ds, chartTypes);
            }
            else
            {
                SessExpires();
            }
        }
        catch (SqlException ex) // This will catch all SQL exceptions
        {
            return ex.Message;
        }
        catch (Exception ex) // This will catch every Exception
        {
            return ex.Message;
        }
        return json;
    }


    #region chart data
    private static string highChart(DataSet ds, string[] chartType)
    {
        string resultset = string.Empty;
        for (int tbcount = 0; tbcount < ds.Tables.Count; tbcount++)
        {
            try
            {
                if (chartType[tbcount].ToLower() != "pie" && chartType[tbcount].ToLower() != "donut" && chartType[tbcount].ToLower() != "area" && chartType[tbcount].ToLower() != "gauge")
                {
                    List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
                    List<string> xAxis = new List<string>();
                    int irow = 0;
                    foreach (DataRow dr in ds.Tables[tbcount].Rows)
                    {
                        Dictionary<string, object> aSeries = new Dictionary<string, object>();
                        aSeries["data"] = new List<decimal>();
                        int itcol = 0;
                        foreach (DataColumn dc in ds.Tables[tbcount].Columns)
                        {
                            if (!dc.ColumnName.ToLower().Contains("hide_"))
                            {
                                if (itcol == 0)
                                {
                                    aSeries["name"] = dr[dc].ToString();
                                    itcol++;
                                }
                                else
                                {
                                    ((List<decimal>)aSeries["data"]).Add(decimal.Parse(dr[dc].ToString()));
                                    if (irow == 0)
                                        xAxis.Add(dc.ColumnName);
                                }
                            }
                        }
                        irow++;
                        allSeries.Add(aSeries);
                    }
                    resultset += JsonConvert.SerializeObject(xAxis) + "~" + JsonConvert.SerializeObject(allSeries) + ";";
                }
                else if (chartType[tbcount].ToLower() == "pie")
                {
                    List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
                    foreach (DataRow dr in ds.Tables[tbcount].Rows)
                    {
                        Dictionary<string, object> aSeries = new Dictionary<string, object>();
                        int itcol = 0;
                        foreach (DataColumn dc in ds.Tables[tbcount].Columns)
                        {
                            if (!dc.ColumnName.ToLower().Contains("hide_"))
                            {
                                if (itcol == 0)
                                {
                                    aSeries["name"] = dr[dc].ToString();
                                    itcol++;
                                }
                                else
                                {
                                    aSeries["y"] = new List<decimal>();
                                    ((List<decimal>)aSeries["y"]).Add(decimal.Parse(dr[dc].ToString()));
                                }
                            }
                        }
                        allSeries.Add(aSeries);
                    }
                    string pirrst = JsonConvert.SerializeObject(allSeries) + ";";
                    resultset += pirrst.Replace(":[", ":").Replace("]}", "}");
                }
                else if (chartType[tbcount].ToLower() == "donut")
                {

                    List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
                    foreach (DataRow dr in ds.Tables[tbcount].Rows)
                    {
                        Dictionary<string, object> aSeries = new Dictionary<string, object>();
                        int itcol = 0;
                        foreach (DataColumn dc in ds.Tables[tbcount].Columns)
                        {
                            if (!dc.ColumnName.ToLower().Contains("hide_"))
                            {
                                if (itcol == 0)
                                {
                                    aSeries["name"] = dr[dc].ToString();
                                    itcol++;
                                }
                                else
                                {
                                    aSeries["y"] = new List<decimal>();
                                    ((List<decimal>)aSeries["y"]).Add(decimal.Parse(dr[dc].ToString()));
                                    break;
                                }
                            }
                        }
                        allSeries.Add(aSeries);
                    }
                    string pirrst = JsonConvert.SerializeObject(allSeries) + ";";
                    resultset += pirrst.Replace("{\"name\":\"", "['").Replace("\",\"y\":[", "',").Replace("]}", "]");
                }
                else if (chartType[tbcount].ToLower() == "area")
                {
                    List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
                    List<string> xAxis = new List<string>();

                    List<string> _data = new List<string>();
                    foreach (DataRow row in ds.Tables[tbcount].Rows)
                    {
                        if (!_data.Contains(row[0].ToString()))
                        {
                            _data.Add((string)Convert.ToString(row[0]));
                        }
                    }
                    for (int i = 0; i < _data.Count; i++)
                    {
                        DataTable dt = ds.Tables[tbcount].AsEnumerable().Where(x => x.Field<string>(0) == _data[i]).CopyToDataTable();
                        Dictionary<string, object> aSeries = new Dictionary<string, object>();
                        aSeries["name"] = _data[i];
                        aSeries["data"] = new List<decimal>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            int itcol = 0;
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (!dc.ColumnName.ToLower().Contains("hide_"))
                                {
                                    if (itcol == 1)
                                    {
                                        if (!xAxis.Contains(dr[dc].ToString()))
                                            xAxis.Add(dr[dc].ToString());
                                    }
                                    else if (itcol > 1)
                                        ((List<decimal>)aSeries["data"]).Add(decimal.Parse(dr[dc].ToString()));
                                    itcol++;
                                }
                            }
                        }
                        allSeries.Add(aSeries);
                    }
                    resultset += JsonConvert.SerializeObject(xAxis) + "~" + JsonConvert.SerializeObject(allSeries) + ";";
                }
                else if (chartType[tbcount].ToLower() == "gauge")
                {
                    List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
                    int irow = 0;
                    foreach (DataRow dr in ds.Tables[tbcount].Rows)
                    {
                        if (irow == 0)
                        {
                            Dictionary<string, object> aSeries = new Dictionary<string, object>();
                            aSeries["data"] = new List<decimal>();
                            int itcol = 0;
                            foreach (DataColumn dc in ds.Tables[tbcount].Columns)
                            {
                                if (!dc.ColumnName.ToLower().Contains("hide_"))
                                {
                                    if (itcol == 0)
                                    {
                                        aSeries["name"] = dr[dc].ToString();
                                        itcol++;
                                    }
                                    else
                                        ((List<decimal>)aSeries["data"]).Add(decimal.Parse(dr[dc].ToString()));
                                }
                            }
                            allSeries.Add(aSeries);
                        }
                        irow++;
                    }
                    resultset += JsonConvert.SerializeObject(allSeries) + ";";
                }
            }
            catch (Exception)
            {
                resultset += ";";
            }
        }
        return resultset.Substring(0, resultset.Length - 1);
    }
    #endregion

    [WebMethod]
    public static string GetFilterParamsData(string sqlQuery)
    {
        DataSet ds = new DataSet();
        string json = string.Empty;
        try
        {
            if (HttpContext.Current.Session["user"] != null)
            {
                DBContext obj = new DBContext();
                sqlQuery = Constants.GET_DASHBOARD_FILTERDATA;
                ds = obj.GetMainPageDBTableInline(sqlQuery, "", "Filter");
                json = JsonConvert.SerializeObject(ds, Formatting.Indented);
            }
            else
            {
                SessExpires();
            }
        }
        catch (SqlException ex) // This will catch all SQL exceptions
        {
            return ex.Message;
        }
        catch (Exception ex) // This will catch every Exception
        {
            return ex.Message;
        }
        return json;
    }
}
