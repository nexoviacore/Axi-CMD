using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Oracle.DataAccess;
using System.IO;
using System.Web;
using System.IO;


public class Oraclehelper : Ihelper
{
    #region Private Members

    private string oracleConnectionString = string.Empty;
    private List<Oracle.DataAccess.Client.OracleParameter> oracleSqlParameters;
    private Oracle.DataAccess.Client.OracleParameter oracleSqlParameter;
    private Oracle.DataAccess.Client.OracleConnection oracleSqlConnection;
    private Oracle.DataAccess.Client.OracleTransaction oracleSqlTransaction;
    private StringBuilder oracleLog;

    #endregion //Private Members

    #region Properties

    /// <summary>
    /// Constructor.
    /// </summary>
    public Oraclehelper(string connectionString)
    {
        oracleConnectionString = connectionString;
        oracleSqlParameters = new List<Oracle.DataAccess.Client.OracleParameter>();
        oracleLog = new StringBuilder();
    }

    /// <summary>
    /// Sets the connection string for the helper instance.
    /// </summary>
    public string ConnectionString
    {
        get { return oracleConnectionString; }
        set { oracleConnectionString = value; }
    }

    #endregion //Properties

    #region Set Parameters

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, int size, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Value = value ?? DBNull.Value;
        oracleSqlParameter.Size = size;
        oracleSqlParameter.Direction = ParameterDirection.Input;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Direction = ParameterDirection.Input;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Value = value ?? DBNull.Value;
        oracleSqlParameter.Direction = ParameterDirection.Input;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, int size, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Size = size;
        oracleSqlParameter.Direction = ParameterDirection.Output;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    public void AddOutParameter(string name)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.Direction = ParameterDirection.Output;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Direction = ParameterDirection.Output;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Value = value;
        oracleSqlParameter.Direction = ParameterDirection.InputOutput;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, int size, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Value = value;
        oracleSqlParameter.Size = size;
        oracleSqlParameter.Direction = ParameterDirection.InputOutput;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = name;
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Direction = ParameterDirection.InputOutput;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    /// <summary>
    /// Adds a return value parameter to the parameter collection.
    /// Use this when executing an Sql function instead of a stored procedure.
    /// </summary>
    public void AddReturnValueParameter(DbType dbType)
    {
        oracleSqlParameter = new Oracle.DataAccess.Client.OracleParameter();
        oracleSqlParameter.ParameterName = "return_value";
        oracleSqlParameter.OracleDbType = GetDbType(dbType);
        oracleSqlParameter.Direction = ParameterDirection.ReturnValue;
        oracleSqlParameters.Add(oracleSqlParameter);
    }

    #endregion Set Parameters

    #region Get Values

    /// <summary>
    /// Gets the value of aparameter from the parameter collection
    /// </summary>
    public object GetParameterValue(string name)
    {
        object result = null;

        foreach (Oracle.DataAccess.Client.OracleParameter parameter in oracleSqlParameters)
        {
            if (parameter.ParameterName.Equals(name))
            {
                parameter.OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
                result = parameter.Value;
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the return value of an Sql function from the parameter collection.
    /// </summary>
    public object GetReturnParameterValue()
    {
        object result = null;

        foreach (Oracle.DataAccess.Client.OracleParameter parameter in oracleSqlParameters)
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

        using (Oracle.DataAccess.Client.OracleDataAdapter sqlDataAdapter = new Oracle.DataAccess.Client.OracleDataAdapter(commandName, ConnectionString))
        {
            DateTime st = DateTime.Now;
            dsResult = new DataSet();
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            //LogInParameters(_SqlParameters, commandName);

            try
            {
                //using (new ElapsedTimeTracer(string.Format("Command: {0}", commandName), _log))
                {
                    sqlDataAdapter.Fill(dsResult);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return dsResult;
    }

    public DataSet ExecuteDataSetSqlConfig(string commandName)
    {
        DataSet dsResult;
        using (Oracle.DataAccess.Client.OracleDataAdapter sqlDataAdapter = new Oracle.DataAccess.Client.OracleDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            for (int x = 0; x < commandName.Split(';').Length; x++)
            {
                DataSet dsResultnew = new DataSet();
                sqlDataAdapter.SelectCommand.CommandText = commandName.Split(';')[x];
                foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
                {
                    sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
                }
                try
                {
                    DateTime st = DateTime.Now;
                    sqlDataAdapter.Fill(dsResultnew);
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

        using (Oracle.DataAccess.Client.OracleDataAdapter sqlDataAdapter = new Oracle.DataAccess.Client.OracleDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            //sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            try
            {
                {
                    DateTime st = DateTime.Now;
                    sqlDataAdapter.Fill(dsResult);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                AddExceptionToSess(s);
                LogFile.Log logObj = new LogFile.Log();
                logObj.CreateLog("Exception in ExecuteDataSetSqlInline :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteDataSetSqlInline", string.Empty, "true");
            }
            finally
            {

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
            if (oracleSqlConnection == null)
                CreateConnection();

            using (Oracle.DataAccess.Client.OracleCommand sqlCommand = new Oracle.DataAccess.Client.OracleCommand(commandName, oracleSqlConnection))
            {
                sqlCommand.CommandText = commandName;

                foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParam);
                }

                {
                    result = sqlCommand.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            string s = ex.Message;
            AddExceptionToSess(s);
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in ExecuteNonQuerySqlinline :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteNonQuerySqlinline", string.Empty, "true");
        }
        finally
        {


            if (oracleSqlTransaction == null)
                DisposeResources();
        }

        return result;



    }

    public string ExecuteNonQuerySqlinlineAlc(string commandName)
    {
        string result = string.Empty;

        try
        {
            if (oracleSqlConnection == null)
                CreateConnection();

            using (Oracle.DataAccess.Client.OracleCommand sqlCommand = new Oracle.DataAccess.Client.OracleCommand(commandName, oracleSqlConnection))
            {
                sqlCommand.CommandText = commandName;

                foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParam);
                }

                {
                    int results = sqlCommand.ExecuteNonQuery();
                    result = results.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            string s = ex.Message;
            result = "Error:" + s;
        }
        finally
        {
            if (oracleSqlTransaction == null)
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
        using (Oracle.DataAccess.Client.OracleDataAdapter sqlDataAdapter = new Oracle.DataAccess.Client.OracleDataAdapter(commandName, ConnectionString))
        {
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            // LogInParameters(_SqlParameters, commandName);

            try
            {
                //using (new ElapsedTimeTracer(string.Format("Command: {0}", commandName), _log))
                {
                    if (tableName.Length == 1)	//If table name is null or empty use default fill.
                    {
                        sqlDataAdapter.Fill(dsResult, tableName[0].ToString());
                    }
                    else if (tableName.Length > 1)
                    {
                        int index = 0;
                        foreach (string name in tableName)
                        {
                            sqlDataAdapter.TableMappings.Add(string.Format("Table{0}", index > 0 ? index.ToString() : ""), name);
                            index += 1;
                        }
                        sqlDataAdapter.Fill(dsResult);
                    }
                    else
                    {
                        sqlDataAdapter.Fill(dsResult, dsResult.Tables[0].TableName.ToString());
                    }
                }
            }
            finally
            {
                //LogOutParameters(_SqlParameters);
                //LogResults(dsResult, commandName);
                //AppLogger.LogInfo(_log.ToString());
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


        using (Oracle.DataAccess.Client.OracleDataAdapter sqlDataAdapter = new Oracle.DataAccess.Client.OracleDataAdapter(commandName, ConnectionString))
        {
            dtResult = new DataTable();
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            //LogInParameters(_SqlParameters, commandName);

            try
            {
                //using (new ElapsedTimeTracer(string.Format("Command: {0}", commandName), _log))
                {
                    sqlDataAdapter.Fill(dtResult);
                }
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
        int result;

        try
        {
            if (oracleSqlConnection == null)
                CreateConnection();

            using (Oracle.DataAccess.Client.OracleCommand sqlCommand = new Oracle.DataAccess.Client.OracleCommand(commandName, oracleSqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = commandName;

                foreach (Oracle.DataAccess.Client.OracleParameter sqlParam in oracleSqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParam);
                }

                //LogInParameters(_SqlParameters, commandName);

                // using (new ElapsedTimeTracer(string.Format("Command: {0}", commandName), _log))
                {
                    result = sqlCommand.ExecuteNonQuery();
                }
            }
        }
        finally
        {
            //LogOutParameters(_SqlParameters);
            //AppLogger.LogInfo(_log.ToString());

            if (oracleSqlTransaction == null)
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
        oracleSqlConnection = new Oracle.DataAccess.Client.OracleConnection(oracleConnectionString);
        oracleSqlConnection.Open();
    }

    /// <summary>
    /// Begins a new transaction
    /// </summary>
    public void BeginTransaction()
    {
        CreateConnection();
        oracleSqlTransaction = oracleSqlConnection.BeginTransaction();
    }

    /// <summary>
    /// Rolls back the transaction
    /// </summary>
    public void RollBackTransaction()
    {
        if (oracleSqlTransaction != null)
            oracleSqlTransaction.Rollback();
    }

    /// <summary>
    /// Commits transaction
    /// </summary>
    public void CommitTransaction()
    {
        if (oracleSqlTransaction != null)
            oracleSqlTransaction.Commit();
    }

    /// <summary>
    /// Closes the connection
    /// </summary>
    private void CloseConnection()
    {
        if (oracleSqlConnection != null)
        {
            if (oracleSqlConnection.State == ConnectionState.Open)
                oracleSqlConnection.Close();
            oracleSqlConnection.Dispose();
        }
    }

    /// <summary>
    /// Releases all the used memory
    /// </summary>
    public void DisposeResources()
    {
        if (oracleSqlTransaction != null)
            oracleSqlTransaction.Dispose();
        CloseConnection();
    }

    /// <summary>
    /// Parameter collection is renewed
    /// </summary>
    public void RenewParameterSet()
    {
        oracleSqlParameters = new List<Oracle.DataAccess.Client.OracleParameter>();
    }
    /// <summary>
    /// Gets the common db type to Sql db type.
    /// </summary>
    /// <param name="dbType">Db type enum.</param>
    /// <returns>Sql relevent DB type.</returns>
    private Oracle.DataAccess.Client.OracleDbType GetDbType(DbType dbType)
    {
        switch (dbType)
        {
            case (DbType.StringFixedLength):

                return Oracle.DataAccess.Client.OracleDbType.Char;
            case (DbType.Int16):
                return Oracle.DataAccess.Client.OracleDbType.Int16;
            case (DbType.Date):
                return Oracle.DataAccess.Client.OracleDbType.Date;
            case (DbType.Decimal):
                return Oracle.DataAccess.Client.OracleDbType.Decimal;
            case (DbType.String):
                return Oracle.DataAccess.Client.OracleDbType.NVarchar2;
            case (DbType.Object):
                return Oracle.DataAccess.Client.OracleDbType.RefCursor;
            default:
                return Oracle.DataAccess.Client.OracleDbType.Varchar2;
        }
    }

    #endregion //Helpers

    #region Logging

    private void LogInParameters(List<SqlParameter> parameters, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            oracleLog.AppendLine(Environment.NewLine + "############################################################");
            oracleLog.AppendLine("Prodecure : " + commandName);
            oracleLog.AppendLine("----------------------------------------------");
            oracleLog.AppendLine("Input Parameters : ");
            oracleLog.AppendLine("----------------------------------------------");
            oracleLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (SqlParameter sqlParam in parameters)
            {
                if (sqlParam.Direction == ParameterDirection.Input)
                    oracleLog.AppendFormat("{0} : {1}{2}", sqlParam.ParameterName, sqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogOutParameters(List<SqlParameter> parameters)
    {
        //if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            oracleLog.AppendLine("Output Parameters : ");
            oracleLog.AppendLine("----------------------------------------------");

            foreach (SqlParameter sqlParam in parameters)
            {
                if (sqlParam.Direction == ParameterDirection.Output)
                    oracleLog.AppendFormat("{0} : {1}{2}", sqlParam.ParameterName, sqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogResults(DataSet dataSet, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataSet.HasData())
        // if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            oracleLog.AppendLine("Data Id      : " + dataId);

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
            //dataSet.WriteXml(Path.Combine(ConfigManager.AppSettings.DBLogFolder, fileName));
        }

        // if (ConfigManager.AppSettings.LogDbResults)
        {
            oracleLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (DataTable table in dataSet.Tables)
            {
                oracleLog.AppendFormat("Result Count for {0} : {1}{2}", table.TableName, table.Rows.Count, Environment.NewLine);
            }
            oracleLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    private void LogResults(DataTable dataTable, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataTable.HasData())
        //  if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            oracleLog.AppendLine("Data Id      : " + dataId);

            if (string.IsNullOrEmpty(dataTable.TableName))
                dataTable.TableName = "Table1";

            string fileName = string.Format("{0}_{1}.dat", commandName, dataId);
            //dataTable.WriteXml(Path.Combine(ConfigManager.AppSettings.DBLogFolder, fileName));
        }

        //if (ConfigManager.AppSettings.LogDbResults)
        {
            oracleLog.AppendLine("Time         : " + DateTime.Now.ToString());
            oracleLog.AppendFormat("Result Count for {0} : {1}{2}", dataTable.TableName, dataTable.Rows.Count, Environment.NewLine);
            oracleLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    #endregion //Logging

}

