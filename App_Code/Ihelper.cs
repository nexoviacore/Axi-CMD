using System.Data;


public interface Ihelper
{
    string ConnectionString { get; set; }
    void AddInOutParameter(string name, DbType dbType);
    void AddInOutParameter(string name, object value, DbType dbType);
    void AddInOutParameter(string name, object value, int size, DbType dbType);
    void AddInParameter(string name, DbType dbType);
    void AddInParameter(string name, object value, DbType dbType);
    void AddInParameter(string name, object value, int size, DbType dbType);
    void AddOutParameter(string name);
    void AddOutParameter(string name, DbType dbType);
    void AddOutParameter(string name, int size, DbType dbType);
    void AddReturnValueParameter(DbType dbType);
    void BeginTransaction();
    void CommitTransaction();
    void DisposeResources();
    DataSet ExecuteDataSet(string commandName);
    DataSet ExecuteDataSet(DataSet dsResult, string commandName, params string[] tableName);
    DataSet ExecuteDataSetSqlInline(string commandName);
    DataSet ExecuteDataSetSqlConfig(string commandName);
    DataTable ExecuteDataTable(string commandName);
    int ExecuteNonQuery(string commandName);
    int ExecuteNonQuerySqlinline(string commandName);
    object GetParameterValue(string name);
    object GetReturnParameterValue();
    void RenewParameterSet();
    void RollBackTransaction();
    string ExecuteNonQuerySqlinlineAlc(string commandName);
}
