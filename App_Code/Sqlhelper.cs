using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

public class Sqlhelper : Ihelper
{
    #region Private Members

    private string msSqlConnectionString = string.Empty;
    private List<SqlParameter> msSqlParameters;
    private SqlParameter msSqlParameter;
    private SqlConnection msSqlConnection;
    private SqlTransaction msSqlTransaction;
    private StringBuilder msSqlLog;

    #endregion //Private Members

    #region Properties

    /// <summary>
    /// Constructor.
    /// </summary>
    public Sqlhelper(string connectionString)
    {
        msSqlConnectionString = connectionString;
        msSqlParameters = new List<SqlParameter>();
        msSqlLog = new StringBuilder();
    }

    /// <summary>
    /// Sets the connection string for the helper instance.
    /// </summary>
    public string ConnectionString
    {
        get { return msSqlConnectionString; }
        set { msSqlConnectionString = value; }
    }

    #endregion //Properties

    #region Set Parameters

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, int size, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Value = value ?? DBNull.Value;
        msSqlParameter.Size = size;
        msSqlParameter.Direction = ParameterDirection.Input;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Direction = ParameterDirection.Input;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Value = value ?? DBNull.Value;
        msSqlParameter.Direction = ParameterDirection.Input;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, int size, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Size = size;
        msSqlParameter.Direction = ParameterDirection.Output;
        msSqlParameters.Add(msSqlParameter);
    }

    public void AddOutParameter(string name)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.Direction = ParameterDirection.Output;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Direction = ParameterDirection.Output;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Value = value;
        msSqlParameter.Direction = ParameterDirection.InputOutput;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, int size, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Value = value;
        msSqlParameter.Size = size;
        msSqlParameter.Direction = ParameterDirection.InputOutput;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = name;
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Direction = ParameterDirection.InputOutput;
        msSqlParameters.Add(msSqlParameter);
    }

    /// <summary>
    /// Adds a return value parameter to the parameter collection.
    /// Use this when executing an Sql function instead of a stored procedure.
    /// </summary>
    public void AddReturnValueParameter(DbType dbType)
    {
        msSqlParameter = new SqlParameter();
        msSqlParameter.ParameterName = "return_value";
        msSqlParameter.SqlDbType = GetDbType(dbType);
        msSqlParameter.Direction = ParameterDirection.ReturnValue;
        msSqlParameters.Add(msSqlParameter);
    }

    #endregion Set Parameters

    #region Get Values

    /// <summary>
    /// Gets the value of aparameter from the parameter collection
    /// </summary>
    public object GetParameterValue(string name)
    {
        object result = null;

        foreach (SqlParameter parameter in msSqlParameters)
        {
            if (parameter.ParameterName.Equals(name))
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

        foreach (SqlParameter parameter in msSqlParameters)
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

        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (SqlParameter sqlParam in msSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            LogInParameters(msSqlParameters, commandName);

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
            finally
            {
                //LogOutParameters(msSqlParameters);
                //LogResults(dsResult, commandName);
                //AppLogger.LogInfo(_log.ToString());
            }
        }

        return dsResult;
    }

    public DataSet ExecuteDataSetSqlConfig(string commandName)
    {
        DataSet dsResult;
        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            for (int x = 0; x < commandName.Split(';').Length; x++)
            {
                DataSet dsResultnew = new DataSet();
                sqlDataAdapter.SelectCommand.CommandText = commandName.Split(';')[x];
                foreach (SqlParameter sqlParam in msSqlParameters)
                {
                    sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
                }

                try
                {
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
        DataSet dsResult = null; ;

        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            //sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (SqlParameter sqlParam in msSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            try
            {
                {
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
            if (msSqlConnection == null)
                CreateConnection();

            using (SqlCommand sqlCommand = new SqlCommand(commandName, msSqlConnection))
            {
                sqlCommand.CommandText = commandName;
                sqlCommand.CommandType = CommandType.Text;
                foreach (SqlParameter sqlParam in msSqlParameters)
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
            if (msSqlTransaction == null)
                DisposeResources();
        }

        return result;

    }

    public string ExecuteNonQuerySqlinlineAlc(string commandName)
    {
        string result = string.Empty;
        try
        {
            if (msSqlConnection == null)
                CreateConnection();

            using (SqlCommand sqlCommand = new SqlCommand(commandName, msSqlConnection))
            {
                sqlCommand.CommandText = commandName;
                sqlCommand.CommandType = CommandType.Text;
                foreach (SqlParameter sqlParam in msSqlParameters)
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
            if (msSqlTransaction == null)
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
        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(commandName, ConnectionString))
        {
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (SqlParameter sqlParam in msSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            LogInParameters(msSqlParameters, commandName);

            try
            {
                //  using (new ElapsedTimeTracer(string.Format("Command: {0}", commandName), _log))
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
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                ///LogOutParameters(msSqlParameters);
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

        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(commandName, ConnectionString))
        {
            dtResult = new DataTable();
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (SqlParameter sqlParam in msSqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            //LogInParameters(msSqlParameters, commandName);

            try
            {
                // using (new ElapsedTimeTracer(string.Format("Command: {0}", commandName), _log))
                {
                    sqlDataAdapter.Fill(dtResult);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                //LogOutParameters(msSqlParameters);
                //LogResults(dtResult, commandName);
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
            if (msSqlConnection == null)
                CreateConnection();

            using (SqlCommand sqlCommand = new SqlCommand(commandName, msSqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = commandName;

                foreach (SqlParameter sqlParam in msSqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParam);
                }

                LogInParameters(msSqlParameters, commandName);

                // using (new ElapsedTimeTracer(string.Format("Command: {0}", commandName), _log))
                {
                    result = sqlCommand.ExecuteNonQuery();
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
            //LogOutParameters(msSqlParameters);
            //AppLogger.LogInfo(_log.ToString());

            if (msSqlTransaction == null)
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
        msSqlConnection = new SqlConnection(msSqlConnectionString);
        msSqlConnection.Open();
    }

    /// <summary>
    /// Begins a new transaction
    /// </summary>
    public void BeginTransaction()
    {
        CreateConnection();
        msSqlTransaction = msSqlConnection.BeginTransaction();
    }

    /// <summary>
    /// Rolls back the transaction
    /// </summary>
    public void RollBackTransaction()
    {
        if (msSqlTransaction != null)
            msSqlTransaction.Rollback();
    }

    /// <summary>
    /// Commits transaction
    /// </summary>
    public void CommitTransaction()
    {
        if (msSqlTransaction != null)
            msSqlTransaction.Commit();
    }

    /// <summary>
    /// Closes the connection
    /// </summary>
    private void CloseConnection()
    {
        if (msSqlConnection != null)
        {
            if (msSqlConnection.State == ConnectionState.Open)
                msSqlConnection.Close();
            msSqlConnection.Dispose();
        }
    }

    /// <summary>
    /// Releases all the used memory
    /// </summary>
    public void DisposeResources()
    {
        if (msSqlTransaction != null)
            msSqlTransaction.Dispose();
        CloseConnection();
    }

    /// <summary>
    /// Parameter collection is renewed
    /// </summary>
    public void RenewParameterSet()
    {
        msSqlParameters = new List<SqlParameter>();
    }
    /// <summary>
    /// Gets the common db type to Sql db type.
    /// </summary>
    /// <param name="dbType">Db type enum.</param>
    /// <returns>Sql relevent DB type.</returns>
    private SqlDbType GetDbType(DbType dbType)
    {
        switch (dbType)
        {
            case (DbType.StringFixedLength):
                return SqlDbType.Char;
            case (DbType.Int32):
                return SqlDbType.Int;
            case (DbType.Date):
                return SqlDbType.Date;
            case (DbType.Decimal):
                return SqlDbType.Decimal;
            case (DbType.String):
                return SqlDbType.VarChar;
            case (DbType.Boolean):
                return SqlDbType.Bit;
            default:
                return SqlDbType.VarChar;
        }
    }

    #endregion //Helpers

    #region Logging

    private void LogInParameters(List<SqlParameter> parameters, string commandName)
    {
        //  if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            msSqlLog.AppendLine(Environment.NewLine + "############################################################");
            msSqlLog.AppendLine("Prodecure : " + commandName);
            msSqlLog.AppendLine("----------------------------------------------");
            msSqlLog.AppendLine("Input Parameters : ");
            msSqlLog.AppendLine("----------------------------------------------");
            msSqlLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (SqlParameter sqlParam in parameters)
            {
                if (sqlParam.Direction == ParameterDirection.Input)
                    msSqlLog.AppendFormat("{0} : {1}{2}", sqlParam.ParameterName, sqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogOutParameters(List<SqlParameter> parameters)
    {
        //if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            msSqlLog.AppendLine("Output Parameters : ");
            msSqlLog.AppendLine("----------------------------------------------");

            foreach (SqlParameter sqlParam in parameters)
            {
                if (sqlParam.Direction == ParameterDirection.Output)
                    msSqlLog.AppendFormat("{0} : {1}{2}", sqlParam.ParameterName, sqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogResults(DataSet dataSet, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataSet.HasData())
        //if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            msSqlLog.AppendLine("Data Id      : " + dataId);

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
            msSqlLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (DataTable table in dataSet.Tables)
            {
                msSqlLog.AppendFormat("Result Count for {0} : {1}{2}", table.TableName, table.Rows.Count, Environment.NewLine);
            }
            msSqlLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    private void LogResults(DataTable dataTable, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataTable.HasData())
        //if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            msSqlLog.AppendLine("Data Id      : " + dataId);

            if (string.IsNullOrEmpty(dataTable.TableName))
                dataTable.TableName = "Table1";

            string fileName = string.Format("{0}_{1}.dat", commandName, dataId);
            //  dataTable.WriteXml(Path.Combine(ConfigManager.AppSettings.DBLogFolder, fileName));
        }

        //if (ConfigManager.AppSettings.LogDbResults)
        {
            msSqlLog.AppendLine("Time         : " + DateTime.Now.ToString());
            msSqlLog.AppendFormat("Result Count for {0} : {1}{2}", dataTable.TableName, dataTable.Rows.Count, Environment.NewLine);
            msSqlLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    #endregion //Logging
}

