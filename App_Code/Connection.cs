using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data;
//using AgileCloud.App_Code.Helper;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;


/// <summary>
/// Summary description for Connection
/// </summary>
public class Connection
{
    string dType = string.Empty;
    public Connection()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Function To Check Which Database Have To connect
    /// </summary>

    public string GetDetails(string id, string ipaddr)
    {
        LogFile.Log logobj = new LogFile.Log();
        string result = string.Empty;
        OracleConnection conn = new OracleConnection(ConfigurationManager.AppSettings["AppConnection"]);
        try
        {
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM agc_userinfotemp where id='" + id + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                result = dr.GetString(0);
            }
            else
            {
             
                string errorLog1 = logobj.CreateLog("DOnt have records", HttpContext.Current.Session.SessionID, "clouddb-connect-Cloud", "new", "true");
                result = "Error!User_And_App_Details_Not_Found";               
            }
            conn.Close();
        }
        catch (Exception ex)
        {
            string errorLog = logobj.CreateLog(ex.ToString(), HttpContext.Current.Session.SessionID, "clouddb-connect-Cloud", "new", "true");
            result = "Error!Connection error";
        }
        return result;

    }


}