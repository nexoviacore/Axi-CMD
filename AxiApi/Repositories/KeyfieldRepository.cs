using ARMCommon.Model;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Exceptions;
using AxiApi.Interfaces;
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

        public async Task<bool> SetKeyfieldAsync(string appname, SetKeyfieldRequestDTO requestDTO)
        {
            _logger.LogInformation("SetKeyfieldAsync called for App: {AppName}, TransId: {TransId}, KeyField: {KeyField}",
                appname, requestDTO.TransId, requestDTO.KeyField);

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();
            string transId = requestDTO.TransId.Trim();
            string keyField = requestDTO.KeyField.Trim();
            if (string.IsNullOrWhiteSpace(requestDTO.Username))
            {
                _logger.LogError("SetKeyfieldAsync called with empty or invalid username");
                throw new ArgumentException("Username is required and cannot be empty.");
            }
            string username = requestDTO.Username.Trim();
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 1. Check if record exists
            string checkSql = "SELECT COUNT(*) FROM axp_tstructprops WHERE LOWER(name) = :transid";
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
                string updateSql = "UPDATE axp_tstructprops SET keyfield = :keyfield, userconfigured = 't', updatedon = :updatedon, updatedby = :updatedby WHERE LOWER(name) = :transid";
                string[] updateParamNames = { ":keyfield", ":updatedon", ":updatedby", ":transid" };
                DbType[] updateParamTypes = { DbType.String, DbType.String, DbType.String, DbType.String };
                object[] updateParamValues = { keyField, currentTime, username, transId.ToLower() };

                nonQueryResult = await db.ExecuteNonQueryAsync(updateSql, updateParamNames, updateParamTypes, updateParamValues);
            }
            else
            {
                // 3. Insert new record
                string insertSql = "INSERT INTO axp_tstructprops (name, keyfield, userconfigured, createdon, createdby) VALUES (:name, :keyfield, 't', :createdon, :createdby)";
                string[] insertParamNames = { ":name", ":keyfield", ":createdon", ":createdby" };
                DbType[] insertParamTypes = { DbType.String, DbType.String, DbType.String, DbType.String };
                object[] insertParamValues = { transId, keyField, currentTime, username };

                nonQueryResult = await db.ExecuteNonQueryAsync(insertSql, insertParamNames, insertParamTypes, insertParamValues);
            }

            if (!string.IsNullOrEmpty(nonQueryResult?.error))
            {
                _logger.LogError("Database error in SetKeyfieldAsync: {Error}", nonQueryResult.error);
                throw new DatabaseException(nonQueryResult.error, count > 0 ? "UPDATE" : "INSERT");
            }

            return true;
        }
    }
}
