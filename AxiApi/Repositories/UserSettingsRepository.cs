using AxExtend.Interface;
using AxiApi.Exceptions;
using AxiApi.Interfaces;
using System.Data;

namespace AxiApi.Repositories
{
    /// <summary>
    /// Reads and writes Axi_UserSettings (one row per user and setting key).
    /// Plain DELETE + INSERT is used instead of a vendor specific upsert so the same code runs on PostgreSQL and Oracle.
    /// </summary>
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly IAxExtend _axExtend;

        public UserSettingsRepository(IAxExtend axExtend)
        {
            _axExtend = axExtend;
        }

        public async Task<Dictionary<string, string>> GetUserSettings(string username, string appname)
        {
            string sqlQuery = "SELECT settingkey, settingvalue FROM Axi_UserSettings WHERE username = :username";
            string[] paramNames = { ":username" };
            DbType[] paramTypes = { DbType.String };
            object[] paramValues = { username };

            var settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (await _axExtend.OpenDBConnectionAsync(appname))
            {
                var db = await _axExtend.GetDB();
                var sqlResult = await db.ExecuteSQLAsync(sqlQuery, paramNames, paramTypes, paramValues);

                if (!string.IsNullOrEmpty(sqlResult?.error))
                    throw new DatabaseException(sqlResult.error, "SELECT");

                if (sqlResult?.data != null)
                {
                    foreach (DataRow row in sqlResult.data.Rows)
                    {
                        var key = row["settingkey"]?.ToString();
                        if (!string.IsNullOrEmpty(key))
                            settings[key] = row["settingvalue"]?.ToString() ?? string.Empty;
                    }
                }
            }

            return settings;
        }

        public async Task SaveUserSetting(string username, string settingKey, string settingValue, string appname)
        {
            string deleteQuery = "DELETE FROM Axi_UserSettings WHERE username = :username AND settingkey = :settingkey";
            string[] deleteNames = { ":username", ":settingkey" };
            DbType[] deleteTypes = { DbType.String, DbType.String };
            object[] deleteValues = { username, settingKey };

            string insertQuery = "INSERT INTO Axi_UserSettings (username, settingkey, settingvalue) VALUES (:username, :settingkey, :settingvalue)";
            string[] insertNames = { ":username", ":settingkey", ":settingvalue" };
            DbType[] insertTypes = { DbType.String, DbType.String, DbType.String };
            object[] insertValues = { username, settingKey, settingValue };

            if (await _axExtend.OpenDBConnectionAsync(appname))
            {
                var db = await _axExtend.GetDB();

                var deleted = await db.ExecuteNonQueryAsync(deleteQuery, deleteNames, deleteTypes, deleteValues);
                if (!string.IsNullOrEmpty(deleted?.error))
                    throw new DatabaseException(deleted.error, "DELETE");

                var inserted = await db.ExecuteNonQueryAsync(insertQuery, insertNames, insertTypes, insertValues);
                if (!string.IsNullOrEmpty(inserted?.error))
                    throw new DatabaseException(inserted.error, "INSERT");
            }
        }
    }
}
