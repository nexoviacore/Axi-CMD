using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Npgsql;
using System.Text;
/// <summary>
/// Summary description for PosrgreSQL
/// </summary>
public class PosrgreSQL : Ihelper
{
    #region Private Members

    private string NpgsqlConnectionString = string.Empty;
    private List<NpgsqlParameter> NpgsqlParameters;
    private NpgsqlParameter NpgsqlParameter;
    private NpgsqlConnection NpgsqlConnection;
    private NpgsqlTransaction NpgsqlTransaction;
    private StringBuilder NpgsqllLog;

    #endregion //Private Members

    #region Properties

    /// <summary>
    /// Constructor.
    /// </summary>
    public PosrgreSQL(string connectionString)
    {

        NpgsqlConnectionString = connectionString;
        NpgsqlParameters = new List<NpgsqlParameter>();
        NpgsqllLog = new StringBuilder();

    }

    public string ConnectionString
    {
        get { return NpgsqlConnectionString; }
        set { NpgsqlConnectionString = value; }
    }

    #endregion //Properties

    #region Set Parameters

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, int size, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType= GetDbType(dbType);
        NpgsqlParameter.Value = value ?? DBNull.Value;
        NpgsqlParameter.Size = size;
        NpgsqlParameter.Direction = ParameterDirection.Input;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Direction = ParameterDirection.Input;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Value = value ?? DBNull.Value;
        NpgsqlParameter.Direction = ParameterDirection.Input;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, int size, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Size = size;
        NpgsqlParameter.Direction = ParameterDirection.Output;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    public void AddOutParameter(string name)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.Direction = ParameterDirection.Output;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Direction = ParameterDirection.Output;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Value = value;
        NpgsqlParameter.Direction = ParameterDirection.InputOutput;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, int size, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Value = value;
        NpgsqlParameter.Size = size;
        NpgsqlParameter.Direction = ParameterDirection.InputOutput;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = name;
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Direction = ParameterDirection.InputOutput;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    /// <summary>
    /// Adds a return value parameter to the parameter collection.
    /// Use this when executing an Sql function instead of a stored procedure.
    /// </summary>
    public void AddReturnValueParameter(DbType dbType)
    {
        NpgsqlParameter = new NpgsqlParameter();
        NpgsqlParameter.ParameterName = "return_value";
        NpgsqlParameter.NpgsqlDbType = GetDbType(dbType);
        NpgsqlParameter.Direction = ParameterDirection.ReturnValue;
        NpgsqlParameters.Add(NpgsqlParameter);
    }

    #endregion Set Parameters 


    #region Get Values

    /// <summary>
    /// Gets the value of aparameter from the parameter collection
    /// </summary>
    public object GetParameterValue(string name)
    {
        object result = null;

        foreach (NpgsqlParameter parameter in NpgsqlParameters)
        {
            if (parameter.ParameterName.Equals(name))
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Char;
            result = parameter.Value;
        }

        return result;
    }

    /// <summary>
    /// Gets the return value of an Sql function from the parameter collection.
    /// </summary>
    public object GetReturnParameterValue()
    {
        object result = null;

        foreach (NpgsqlParameter parameter in NpgsqlParameters)
        {
            if (parameter.ParameterName.Equals("return_value"))
                result = parameter.Value;
        }

        return result;
    }

    #endregion //Get Values

    #region Execute

    /// <summary>
    /// Executes and returns a dataset.
    /// </summary>
    /// <param name="commandName">Command name to execute.</param>
    /// <returns>Resultant dataset.</returns>

    public DataSet ExecuteDataSet(string commandName)
    {
        DataSet dsResult;
        using (NpgsqlDataAdapter NpgsqlDataAdapter = new NpgsqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            NpgsqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            NpgsqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
            {
                NpgsqlDataAdapter.SelectCommand.Parameters.Add(NpgsqlParam);
            }

            try
            {
                NpgsqlDataAdapter.Fill(dsResult);
            }
          
            catch (Exception ex)
            {
                throw ex;
                //ExceptionLogging.SendErrorToText(ex);
                //throw new Exception(Codes.ERR1001);
            }

        }


        return dsResult;
    }

    public DataSet ExecuteDataSetSqlConfig(string commandName)
    {
        DataSet dsResult;
        using (NpgsqlDataAdapter NpgsqlDataAdapter = new NpgsqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            for (int x = 0; x < commandName.Split(';').Length; x++)
            {
                DataSet dsResultnew = new DataSet();
                NpgsqlDataAdapter.SelectCommand.CommandText = commandName.Split(';')[x];
                foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
                {
                    NpgsqlDataAdapter.SelectCommand.Parameters.Add(NpgsqlParam);
                }

                try
                {
                    NpgsqlDataAdapter.Fill(dsResultnew);
                    dsResult.Tables.Add("Table" + x);
                    dsResult.Tables["Table" + x].Merge(dsResultnew.Tables["Table"]);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    LogFile.Log logObj = new LogFile.Log();
                    logObj.CreateLog("Exception in ExecuteDataSetSqlConfig :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteDataSetSqlConfig", string.Empty, "true");
                }
                finally
                {

                }
            }
        }

        return dsResult;
    }

    public DataSet ExecuteDataSetSqlInline(string commandName)
    {
        DataSet dsResult;

        using (NpgsqlDataAdapter NpgsqlDataAdapter = new NpgsqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            //sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            NpgsqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
            {
                NpgsqlDataAdapter.SelectCommand.Parameters.Add(NpgsqlParam);
            }

            try
            {

                NpgsqlDataAdapter.Fill(dsResult);

            }
            catch (Exception ex)
            {
                AddExceptionToSess(ex.Message);
                //ExceptionLogging.SendErrorToText(ex);
                //throw new Exception(Codes.ERR1011);
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Exception in ExecuteDataSetSqlInline :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteDataSetSqlInline", string.Empty, "true");
            }
        }

        return dsResult;
    }

    private void AddExceptionToSess(string exception)
    {
        HttpContext.Current.Session["AxDirectDBException"] = exception;
    }

    public int ExecuteNonQuerySqlinline(string commandName)
    {
        int result = 0;

        try
        {
            if (NpgsqlConnection == null)
                CreateConnection();

            using (NpgsqlCommand NpgsqlCommand = new NpgsqlCommand(commandName, NpgsqlConnection))
            {
                NpgsqlCommand.CommandText = commandName;

                foreach (NpgsqlParameter sqlParam in NpgsqlParameters)
                {
                    NpgsqlCommand.Parameters.Add(sqlParam);
                }

                {
                    result = NpgsqlCommand.ExecuteNonQuery();
                }
            }

        }
       
        catch (Exception ex)
        {
            //ExceptionLogging.SendErrorToText(ex);
            //throw new Exception(Codes.ERR1021);
            string s = ex.Message;
            AddExceptionToSess(s);
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in ExecuteNonQuerySqlinline :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteNonQuerySqlinline", string.Empty, "true");
        }
        finally
        {


            if (NpgsqlTransaction == null)
                DisposeResources();
        }

        return result;



    }

    public string ExecuteNonQuerySqlinlineAlc(string commandName)
    {
        string result = string.Empty;
        try
        {
            if (NpgsqlConnection == null)
                CreateConnection();

            using (NpgsqlCommand NpgsqlCommand = new NpgsqlCommand(commandName, NpgsqlConnection))
            {
                NpgsqlCommand.CommandText = commandName;

                foreach (NpgsqlParameter sqlParam in NpgsqlParameters)
                {
                    NpgsqlCommand.Parameters.Add(sqlParam);
                }

                {
                   int results = NpgsqlCommand.ExecuteNonQuery();
                    result = results.ToString();
                }
            }
        }

        catch (Exception ex)
        {
            //ExceptionLogging.SendErrorToText(ex);
            //throw new Exception(Codes.ERR1021);
            string s = ex.Message;
            result = "Error:" + s;
        }
        finally
        {
            if (NpgsqlTransaction == null)
                DisposeResources();
        }
        return result;
    }
    /// <summary>
    /// Executes and returns a dataset.
    /// </summary>
    /// <param name="dsResult">Dataset reference to populate data.</param>
    /// <param name="tableName">Exact table name to populate data into.</param>
    /// <param name="commandName">Command name to execute.</param>
    /// <returns>Resultant dataset.</returns>
    public DataSet ExecuteDataSet(DataSet dsResult, string commandName, params string[] tableName)
    {
        using (NpgsqlDataAdapter NpgsqlDataAdapter = new NpgsqlDataAdapter(commandName, ConnectionString))
        {
            NpgsqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            NpgsqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
            {
                NpgsqlDataAdapter.SelectCommand.Parameters.Add(NpgsqlParam);
            }

            try
            {

                {
                    if (tableName.Length == 1)	//If table name is null or empty use default fill.
                    {
                        NpgsqlDataAdapter.Fill(dsResult, tableName[0].ToString());
                    }
                    else if (tableName.Length > 1)
                    {
                        int index = 0;
                        foreach (string name in tableName)
                        {
                            NpgsqlDataAdapter.TableMappings.Add(string.Format("Table{0}", index > 0 ? index.ToString() : ""), name);
                            index += 1;
                        }
                        NpgsqlDataAdapter.Fill(dsResult);
                    }
                    else
                    {
                        NpgsqlDataAdapter.Fill(dsResult, dsResult.Tables[0].TableName.ToString());
                    }
                }
            }
            
            finally
            {
                if (NpgsqlTransaction == null)
                    DisposeResources();
            }
        }

        return dsResult;
    }

    /// <summary>
    /// Executes and returns a datatable.
    /// </summary>
    /// <param name="commandName">Command name to execute.</param>
    /// <returns>Resultant datatable.</returns>
    public DataTable ExecuteDataTable(string commandName)
    {
        DataTable dtResult;

        using (NpgsqlDataAdapter NpgsqlDataAdapter = new NpgsqlDataAdapter(commandName, ConnectionString))
        {
            dtResult = new DataTable();
            NpgsqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            NpgsqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
            {
                NpgsqlDataAdapter.SelectCommand.Parameters.Add(NpgsqlParam);
            }

            //LogInParameters(_SqlParameters, commandName);

            try
            {

                NpgsqlDataAdapter.Fill(dtResult);

            }
            finally
            {
                //LogOutParameters(_SqlParameters);
                LogResults(dtResult, commandName);
                //AppLogger.LogInfo(_log.ToString());
            }
        }



        return dtResult;
    }

    /// <summary>
    /// Executes a non query.
    /// </summary>
    /// <param name="commandName">Command name to execute.</param>
    /// <returns>No: of row affected.</returns>
    public int ExecuteNonQuery(string commandName)
    {
        int result = 0;

        try
        {
            if (NpgsqlConnection == null)
                CreateConnection();

            using (NpgsqlCommand NpgsqlCommand = new NpgsqlCommand(commandName, NpgsqlConnection))
            {
                NpgsqlCommand.CommandType = CommandType.StoredProcedure;
                NpgsqlCommand.CommandText = commandName;

                foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
                {
                    NpgsqlCommand.Parameters.Add(NpgsqlParam);
                }

                {
                    result = NpgsqlCommand.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            string s = ex.Message;
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in ExecuteNonQuery :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteNonQuery", string.Empty, "true");
        }
        finally
        {


            if (NpgsqlTransaction == null)
                DisposeResources();
        }

        return result;



    }

    #endregion //Execute

    #region Helpers

    /// <summary>
    /// Creates a connection
    /// </summary>
    private void CreateConnection()
    {
        NpgsqlConnection = new NpgsqlConnection(NpgsqlConnectionString);
        NpgsqlConnection.Open();
    }

    /// <summary>
    /// Begins a new transaction
    /// </summary>
    public void BeginTransaction()
    {
        CreateConnection();
        NpgsqlTransaction = NpgsqlConnection.BeginTransaction();
    }

    /// <summary>
    /// Rolls back the transaction
    /// </summary>
    public void RollBackTransaction()
    {
        if (NpgsqlTransaction != null)
            NpgsqlTransaction.Rollback();
    }

    /// <summary>
    /// Commits transaction
    /// </summary>
    public void CommitTransaction()
    {
        if (NpgsqlTransaction != null)
            NpgsqlTransaction.Commit();
    }

    /// <summary>
    /// Closes the connection
    /// </summary>
    private void CloseConnection()
    {
        if (NpgsqlConnection != null)
        {
            if (NpgsqlConnection.State == ConnectionState.Open)
                NpgsqlConnection.Close();
            NpgsqlConnection.Dispose();
        }
    }

    /// <summary>
    /// Releases all the used memory
    /// </summary>
    public void DisposeResources()
    {
        if (NpgsqlTransaction != null)
            NpgsqlTransaction.Dispose();
        CloseConnection();
    }

    /// <summary>
    /// Parameter collection is renewed
    /// </summary>
    public void RenewParameterSet()
    {
        NpgsqlParameters = new List<NpgsqlParameter>();
    }
    /// <summary>
    /// Gets the common db type to Sql db type.
    /// </summary>
    /// <param name="dbType">Db type enum.</param>
    /// <returns>Sql relevent DB type.</returns>
    private NpgsqlTypes.NpgsqlDbType GetDbType(DbType dbType)
    {
        switch (dbType)
        { 
            case (DbType.StringFixedLength):
                return NpgsqlTypes.NpgsqlDbType.Char;
            case (DbType.Int32):
            case (DbType.Int16):
            case (DbType.Int64):
                return NpgsqlTypes.NpgsqlDbType.Integer;
            case (DbType.Date):
                return NpgsqlTypes.NpgsqlDbType.Date;
            case (DbType.Decimal):
                return NpgsqlTypes.NpgsqlDbType.Double;
            case (DbType.String):
                return NpgsqlTypes.NpgsqlDbType.Varchar;
            case (DbType.Boolean):
                return NpgsqlTypes.NpgsqlDbType.Boolean;
            case (DbType.Xml):
                return NpgsqlTypes.NpgsqlDbType.Xml;
            case (DbType.Object):
                return NpgsqlTypes.NpgsqlDbType.Refcursor;

            default:
                return NpgsqlTypes.NpgsqlDbType.Text;
        }
    }

    #endregion //Helpers

    #region Logging

    private void LogInParameters(List<NpgsqlParameter> parameters, string commandName)
    {
        //  if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            NpgsqllLog.AppendLine(Environment.NewLine + "############################################################");
            NpgsqllLog.AppendLine("Prodecure : " + commandName);
            NpgsqllLog.AppendLine("----------------------------------------------");
            NpgsqllLog.AppendLine("Input Parameters : ");
            NpgsqllLog.AppendLine("----------------------------------------------");
            NpgsqllLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
            {
                if (NpgsqlParam.Direction == ParameterDirection.Input)
                    NpgsqllLog.AppendFormat("{0} : {1}{2}", NpgsqlParam.ParameterName, NpgsqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogOutParameters(List<NpgsqlParameter> parameters)
    {
        //if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            NpgsqllLog.AppendLine("Output Parameters : ");
            NpgsqllLog.AppendLine("----------------------------------------------");

            foreach (NpgsqlParameter NpgsqlParam in NpgsqlParameters)
            {
                if (NpgsqlParam.Direction == ParameterDirection.Output)
                    NpgsqllLog.AppendFormat("{0} : {1}{2}", NpgsqlParam.ParameterName, NpgsqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogResults(DataSet dataSet, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataSet.HasData())
        //if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            NpgsqllLog.AppendLine("Data Id      : " + dataId);

            int counter = 1;
            foreach (DataTable table in dataSet.Tables)
            {
                if (string.IsNullOrEmpty(table.TableName))
                {
                    table.TableName = string.Format("Table{0}", counter);
                    counter++;
                }
            }

            string fileName = string.Format("{0}_{1}.dat", commandName, dataId);
            //  dataSet.WriteXml(Path.Combine(ConfigManager.AppSettings.DBLogFolder, fileName));
        }

        //if (ConfigManager.AppSettings.LogDbResults)
        {
            NpgsqllLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (DataTable table in dataSet.Tables)
            {
                NpgsqllLog.AppendFormat("Result Count for {0} : {1}{2}", table.TableName, table.Rows.Count, Environment.NewLine);
            }
            NpgsqllLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    private void LogResults(DataTable dataTable, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataTable.HasData())
        //if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            NpgsqllLog.AppendLine("Data Id      : " + dataId);

            if (string.IsNullOrEmpty(dataTable.TableName))
                dataTable.TableName = "Table1";

            string fileName = string.Format("{0}_{1}.dat", commandName, dataId);
            //  dataTable.WriteXml(Path.Combine(ConfigManager.AppSettings.DBLogFolder, fileName));
        }

        //if (ConfigManager.AppSettings.LogDbResults)
        {
            NpgsqllLog.AppendLine("Time         : " + DateTime.Now.ToString());
            NpgsqllLog.AppendFormat("Result Count for {0} : {1}{2}", dataTable.TableName, dataTable.Rows.Count, Environment.NewLine);
            NpgsqllLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    #endregion //Logging
}
