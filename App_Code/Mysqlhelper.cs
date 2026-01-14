using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Web;

public class Mysqlhelper : Ihelper
{
    #region Private Members

    private string mySqlConnectionString = string.Empty;
    private List<MySqlParameter> mySqlParameters;
    private MySqlParameter mySqlParameter;
    private MySqlConnection mySqlConnection;
    private MySqlTransaction mySqlTransaction;
    private StringBuilder mySqlLog;

    #endregion //Private Members

    #region Properties

    /// <summary>
    /// Constructor.
    /// </summary>
    public Mysqlhelper(string connectionString)
    {
        mySqlConnectionString = connectionString;
        mySqlParameters = new List<MySqlParameter>();
        mySqlLog = new StringBuilder();
    }

    /// <summary>
    /// Sets the connection string for the helper instance.
    /// </summary>
    public string ConnectionString
    {
        get { return mySqlConnectionString; }
        set { mySqlConnectionString = value; }
    }

    #endregion //Properties

    #region Set Parameters

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, int size, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Value = value ?? DBNull.Value;
        mySqlParameter.Size = size;
        mySqlParameter.Direction = ParameterDirection.Input;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Direction = ParameterDirection.Input;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds an input parameter to the parameter collection.
    /// </summary>
    public void AddInParameter(string name, object value, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Value = value ?? DBNull.Value;
        mySqlParameter.Direction = ParameterDirection.Input;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, int size, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Size = size;
        mySqlParameter.Direction = ParameterDirection.Output;
        mySqlParameters.Add(mySqlParameter);
    }

    public void AddOutParameter(string name)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.Direction = ParameterDirection.Output;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds an output parameter to the parameter collection
    /// </summary>
    public void AddOutParameter(string name, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Direction = ParameterDirection.Output;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Value = value;
        mySqlParameter.Direction = ParameterDirection.InputOutput;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, object value, int size, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Value = value;
        mySqlParameter.Size = size;
        mySqlParameter.Direction = ParameterDirection.InputOutput;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds a InputOutput parameter to the parameter collection
    /// </summary>
    public void AddInOutParameter(string name, DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = name;
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Direction = ParameterDirection.InputOutput;
        mySqlParameters.Add(mySqlParameter);
    }

    /// <summary>
    /// Adds a return value parameter to the parameter collection.
    /// Use this when executing an Sql function instead of a stored procedure.
    /// </summary>
    public void AddReturnValueParameter(DbType dbType)
    {
        mySqlParameter = new MySqlParameter();
        mySqlParameter.ParameterName = "return_value";
        mySqlParameter.MySqlDbType = GetDbType(dbType);
        mySqlParameter.Direction = ParameterDirection.ReturnValue;
        mySqlParameters.Add(mySqlParameter);
    }

    #endregion Set Parameters

    #region Get Values

    /// <summary>
    /// Gets the value of aparameter from the parameter collection
    /// </summary>
    public object GetParameterValue(string name)
    {
        object result = null;

        foreach (MySqlParameter parameter in mySqlParameters)
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

        foreach (MySqlParameter parameter in mySqlParameters)
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
        using (MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (MySqlParameter sqlParam in mySqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            try
            {
                sqlDataAdapter.Fill(dsResult);
            }
            catch (DbException ex)
            {
                throw ex;
                //ExceptionLogging.SendErrorToText(ex);
                //throw new Exception(Codes.ERR1000);
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
        using (MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            for (int x = 0; x < commandName.Split(';').Length; x++)
            {
                DataSet dsResultnew = new DataSet();
                sqlDataAdapter.SelectCommand.CommandText = commandName.Split(';')[x];
                foreach (MySqlParameter sqlParam in mySqlParameters)
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
        DataSet dsResult;

        using (MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(commandName, ConnectionString))
        {
            dsResult = new DataSet();
            //sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (MySqlParameter sqlParam in mySqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            try
            {

                sqlDataAdapter.Fill(dsResult);

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
            if (mySqlConnection == null)
                CreateConnection();

            using (MySqlCommand sqlCommand = new MySqlCommand(commandName, mySqlConnection))
            {
                sqlCommand.CommandText = commandName;

                foreach (MySqlParameter sqlParam in mySqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParam);
                }

                {
                    result = sqlCommand.ExecuteNonQuery();
                }
            }

        }
        catch (DbException ex)
        {
            //ExceptionLogging.SendErrorToText(ex);
            //throw new Exception(Codes.ERR1020);
        }
        catch (Exception ex)
        {
            //ExceptionLogging.SendErrorToText(ex);
            //throw new Exception(Codes.ERR1021);
            AddExceptionToSess(ex.Message);
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in ExecuteNonQuerySqlinline :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteNonQuerySqlinline", string.Empty, "true");
        }
        finally
        {
            if (mySqlTransaction == null)
                DisposeResources();
        }

        return result;



    }

    public string ExecuteNonQuerySqlinlineAlc(string commandName)
    {
        string result = string.Empty;

        try
        {
            if (mySqlConnection == null)
                CreateConnection();

            using (MySqlCommand sqlCommand = new MySqlCommand(commandName, mySqlConnection))
            {
                sqlCommand.CommandText = commandName;

                foreach (MySqlParameter sqlParam in mySqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParam);
                }

                {
                    int results = sqlCommand.ExecuteNonQuery();
                    result = results.ToString();
                }
            }

        }
        catch (DbException ex)
        {
            //ExceptionLogging.SendErrorToText(ex);
            //throw new Exception(Codes.ERR1020);
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
            if (mySqlTransaction == null)
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
        using (MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(commandName, ConnectionString))
        {
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (MySqlParameter sqlParam in mySqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            try
            {

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
            catch (DbException ex)
            {
                //  throw new Exception(Codes.ERR1030);
            }
            catch (Exception ex)
            {
                //throw new Exception(Codes.ERR1031);
            }
            finally
            {
                if (mySqlTransaction == null)
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

        using (MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(commandName, ConnectionString))
        {
            dtResult = new DataTable();
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            sqlDataAdapter.SelectCommand.CommandText = commandName;

            foreach (MySqlParameter sqlParam in mySqlParameters)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
            }

            //LogInParameters(_SqlParameters, commandName);

            try
            {

                sqlDataAdapter.Fill(dtResult);

            }
            catch (DbException ex)
            {
                //  throw new Exception(Codes.ERR1040);
            }
            catch (Exception ex)
            {
                // throw new Exception(Codes.ERR1041);
            }

            finally
            {
                if (mySqlTransaction == null)
                    DisposeResources();
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
            if (mySqlConnection == null)
                CreateConnection();

            using (MySqlCommand sqlCommand = new MySqlCommand(commandName, mySqlConnection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = commandName;

                foreach (MySqlParameter sqlParam in mySqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlParam);
                }

                {
                    result = sqlCommand.ExecuteNonQuery();
                }
            }
        }
        catch (DbException ex)
        {
            // throw new Exception(Codes.ERR1050);
        }
        catch (Exception ex)
        {
            //throw new Exception(Codes.ERR1051);
            LogFile.Log logObj = new LogFile.Log();
            logObj.CreateLog("Exception in ExecuteNonQuery :- " + ex.Message + "", HttpContext.Current.Session["nsessionid"].ToString(), "ExecuteNonQuery", string.Empty, "true");
        }
        finally
        {
            if (mySqlTransaction == null)
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
        mySqlConnection = new MySqlConnection(mySqlConnectionString);
        mySqlConnection.Open();
    }

    /// <summary>
    /// Begins a new transaction
    /// </summary>
    public void BeginTransaction()
    {
        CreateConnection();
        mySqlTransaction = mySqlConnection.BeginTransaction();
    }

    /// <summary>
    /// Rolls back the transaction
    /// </summary>
    public void RollBackTransaction()
    {
        if (mySqlTransaction != null)
            mySqlTransaction.Rollback();
    }

    /// <summary>
    /// Commits transaction
    /// </summary>
    public void CommitTransaction()
    {
        if (mySqlTransaction != null)
            mySqlTransaction.Commit();
    }

    /// <summary>
    /// Closes the connection
    /// </summary>
    private void CloseConnection()
    {
        if (mySqlConnection != null)
        {
            if (mySqlConnection.State == ConnectionState.Open)
                mySqlConnection.Close();
            mySqlConnection.Dispose();
        }
    }

    /// <summary>
    /// Releases all the used memory
    /// </summary>
    public void DisposeResources()
    {
        if (mySqlTransaction != null)
            mySqlTransaction.Dispose();
        CloseConnection();
    }

    /// <summary>
    /// Parameter collection is renewed
    /// </summary>
    public void RenewParameterSet()
    {
        mySqlParameters = new List<MySqlParameter>();
    }
    /// <summary>
    /// Gets the common db type to Sql db type.
    /// </summary>
    /// <param name="dbType">Db type enum.</param>
    /// <returns>Sql relevent DB type.</returns>
    private MySqlDbType GetDbType(DbType dbType)
    {
        switch (dbType)
        {
            case (DbType.StringFixedLength):
                return MySqlDbType.VarString;
            case (DbType.Int32):
                return MySqlDbType.Int32;
            case (DbType.Int16):
                return MySqlDbType.Int16;
            case (DbType.Int64):
                return MySqlDbType.Int64;
            case (DbType.Date):
                return MySqlDbType.Date;
            case (DbType.Decimal):
                return MySqlDbType.Decimal;
            case (DbType.String):
                return MySqlDbType.VarString;
            case (DbType.Boolean):
                return MySqlDbType.Bit;
            case (DbType.Xml):
                return MySqlDbType.LongText;
            default:
                return MySqlDbType.Text;
        }
    }

    #endregion //Helpers

    #region Logging

    private void LogInParameters(List<MySqlParameter> parameters, string commandName)
    {
        //  if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            mySqlLog.AppendLine(Environment.NewLine + "############################################################");
            mySqlLog.AppendLine("Prodecure : " + commandName);
            mySqlLog.AppendLine("----------------------------------------------");
            mySqlLog.AppendLine("Input Parameters : ");
            mySqlLog.AppendLine("----------------------------------------------");
            mySqlLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (MySqlParameter sqlParam in parameters)
            {
                if (sqlParam.Direction == ParameterDirection.Input)
                    mySqlLog.AppendFormat("{0} : {1}{2}", sqlParam.ParameterName, sqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogOutParameters(List<MySqlParameter> parameters)
    {
        //if (ConfigManager.AppSettings.LogDbExecutionTime)
        {
            mySqlLog.AppendLine("Output Parameters : ");
            mySqlLog.AppendLine("----------------------------------------------");

            foreach (MySqlParameter sqlParam in parameters)
            {
                if (sqlParam.Direction == ParameterDirection.Output)
                    mySqlLog.AppendFormat("{0} : {1}{2}", sqlParam.ParameterName, sqlParam.Value, Environment.NewLine);
            }
        }
    }

    private void LogResults(DataSet dataSet, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataSet.HasData())
        //if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            mySqlLog.AppendLine("Data Id      : " + dataId);

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
            mySqlLog.AppendLine("Time      : " + DateTime.Now.ToString());

            foreach (DataTable table in dataSet.Tables)
            {
                mySqlLog.AppendFormat("Result Count for {0} : {1}{2}", table.TableName, table.Rows.Count, Environment.NewLine);
            }
            mySqlLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    private void LogResults(DataTable dataTable, string commandName)
    {
        //if (ConfigManager.AppSettings.LogDbResults && dataTable.HasData())
        //if (ConfigManager.AppSettings.LogDbQueryParameters)
        {
            string dataId = Guid.NewGuid().ToString();
            mySqlLog.AppendLine("Data Id      : " + dataId);

            if (string.IsNullOrEmpty(dataTable.TableName))
                dataTable.TableName = "Table1";

            string fileName = string.Format("{0}_{1}.dat", commandName, dataId);
            //  dataTable.WriteXml(Path.Combine(ConfigManager.AppSettings.DBLogFolder, fileName));
        }

        //if (ConfigManager.AppSettings.LogDbResults)
        {
            mySqlLog.AppendLine("Time         : " + DateTime.Now.ToString());
            mySqlLog.AppendFormat("Result Count for {0} : {1}{2}", dataTable.TableName, dataTable.Rows.Count, Environment.NewLine);
            mySqlLog.AppendLine(Environment.NewLine + "############################################################");
        }
    }

    #endregion //Logging
}
