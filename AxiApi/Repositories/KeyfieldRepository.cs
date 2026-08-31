using ARMCommon.Model;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Exceptions;
using AxiApi.Interfaces;
using AxiApi.Lib;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace AxiApi.Repositories
{
    public class KeyfieldRepository : IKeyfieldRepository
    {
        private readonly IAxExtend _axExtend;
        private readonly ILogger<KeyfieldRepository> _logger;

        public KeyfieldRepository(IAxExtend axExtend, ILogger<KeyfieldRepository> logger)
        {
            _axExtend = axExtend;
            _logger = logger;
        }

        public async Task<bool> SetKeyfieldAsync(SetKeyfieldRequestDTO requestDTO)
        {
            string appname = requestDTO.AppName?.Trim() ?? string.Empty;
            string transId = requestDTO.TransId?.Trim() ?? string.Empty;
            string keyField = requestDTO.KeyField?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(requestDTO.Username))
            {
                _logger.LogError("SetKeyfieldAsync called with empty or invalid username");
                throw new ArgumentException("Username is required and cannot be empty.");
            }
            string username = requestDTO.Username.Trim();

            const int maxRetries = 3;
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
                    if (!isDbConnected)
                    {
                        _logger.LogError("Failed to open DB connection for appname: {AppName} on attempt {Attempt}/{MaxRetries}", appname, attempt, maxRetries);
                        if (attempt == maxRetries)
                        {
                            throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
                        }
                        await Task.Delay(200 * attempt);
                        continue;
                    }

                    var db = await _axExtend.GetDB();

                    // 1. Check if record exists
                    string checkSql = SqlQueries.CheckTstructPropsExists;
                    string[] checkParamNames = { ":transid" };
                    DbType[] checkParamTypes = { DbType.String };
                    object[] checkParamValues = { transId.ToLower() };

                    SQLResult checkResult = await db.ExecuteSQLAsync(checkSql, checkParamNames, checkParamTypes, checkParamValues);

                    if (!string.IsNullOrEmpty(checkResult?.error))
                    {
                        _logger.LogError("Database error checking axp_tstructprops: {Error}", checkResult.error);
                        throw new DatabaseException(checkResult.error, "SELECT");
                    }

                    int count = 0;
                    if (checkResult?.data != null && checkResult.data.Rows.Count > 0)
                    {
                        count = Convert.ToInt32(checkResult.data.Rows[0][0]);
                    }

                    NonQueryResult nonQueryResult;
                    if (count > 0)
                    {
                        // 2. Update existing record
                        string updateSql = SqlQueries.UpdateTstructPropsKeyfield;
                        string[] updateParamNames = { ":keyfield", ":updatedby", ":transid" };
                        DbType[] updateParamTypes = { DbType.String, DbType.String, DbType.String };
                        object[] updateParamValues = { keyField, username, transId.ToLower() };

                        nonQueryResult = await db.ExecuteNonQueryAsync(updateSql, updateParamNames, updateParamTypes, updateParamValues);
                    }
                    else
                    {
                        // 3. Insert new record
                        string insertSql = SqlQueries.InsertTstructPropsKeyfield;
                        string[] insertParamNames = { ":name", ":keyfield", ":createdby" };
                        DbType[] insertParamTypes = { DbType.String, DbType.String, DbType.String };
                        object[] insertParamValues = { transId, keyField, username };

                        nonQueryResult = await db.ExecuteNonQueryAsync(insertSql, insertParamNames, insertParamTypes, insertParamValues);
                    }

                    if (!string.IsNullOrEmpty(nonQueryResult?.error))
                    {
                        _logger.LogError("Database error in SetKeyfieldAsync: {Error}", nonQueryResult.error);
                        throw new DatabaseException(nonQueryResult.error, count > 0 ? "UPDATE" : "INSERT");
                    }

                    return true;
                }
                catch (DatabaseException)
                {
                    // Business or query syntax errors should not be retried
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Transient connection/stream error on attempt {Attempt}/{MaxRetries} for appname: {AppName}", attempt, maxRetries, appname);
                    if (attempt == maxRetries)
                    {
                        _logger.LogError(ex, "Exhausted all DB retry attempts for appname: {AppName}", appname);
                        throw;
                    }
                    await Task.Delay(250 * attempt);
                }
            }

            return false;
        }
    }
}
