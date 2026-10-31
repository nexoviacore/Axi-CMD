using ARMCommon.Model;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Exceptions;
using AxiApi.Interfaces;
using System.Data;

namespace AxiApi.Repositories
{
    /// <summary>
    /// Reads and writes axi_command_settings (app-wide, one row per category, type and property name).
    /// The save reads the current rows, then inserts the new ones and updates the existing ones in one transaction. Plain INSERT / UPDATE is
    /// used instead of ON CONFLICT or MERGE so the same code runs on PostgreSQL and Oracle (as in UserSettingsRepository).
    /// If two saves insert the same new property at the same moment, the unique key makes the second fail with a DatabaseException.
    /// </summary>
    public class CommandSettingsRepository : ICommandSettingsRepository
    {
        private readonly IAxExtend _axExtend;
        private readonly ILogger<CommandSettingsRepository> _logger;

        public CommandSettingsRepository(IAxExtend axExtend, ILogger<CommandSettingsRepository> logger)
        {
            _axExtend = axExtend;
            _logger = logger;
        }

        public async Task<List<CommandSettingRowDTO>> GetCommandSettings(string appname, string category, string? type)
        {
            string sqlQuery = "SELECT propertytype, propertyname, propertyvalue, username, modifiedon FROM axi_command_settings WHERE propertycategory = :propertycategory";
            var names = new List<string> { ":propertycategory" };
            var values = new List<object> { category };
            if (type != null)
            {
                sqlQuery += " AND propertytype = :propertytype";
                names.Add(":propertytype");
                values.Add(type);
            }
            sqlQuery += " ORDER BY propertytype, propertyname";

            var rows = new List<CommandSettingRowDTO>();

            if (await _axExtend.OpenDBConnectionAsync(appname))
            {
                var db = await _axExtend.GetDB();
                var sqlResult = await db.ExecuteSQLAsync(sqlQuery, names.ToArray(), names.Select(_ => DbType.String).ToArray(), values.ToArray());

                if (!string.IsNullOrEmpty(sqlResult?.error))
                    throw new DatabaseException(sqlResult.error, "SELECT");

                if (sqlResult?.data != null)
                {
                    foreach (DataRow row in sqlResult.data.Rows)
                    {
                        rows.Add(new CommandSettingRowDTO
                        {
                            PropertyType = row["propertytype"]?.ToString() ?? string.Empty,
                            PropertyName = row["propertyname"]?.ToString() ?? string.Empty,
                            PropertyValue = row["propertyvalue"] is DBNull ? string.Empty : row["propertyvalue"].ToString() ?? string.Empty,
                            Username = row["username"] is DBNull ? null : row["username"].ToString(),
                            ModifiedOn = row["modifiedon"] is DBNull ? null : Convert.ToDateTime(row["modifiedon"])
                        });
                    }
                }
            }

            return rows;
        }

        public async Task<Dictionary<string, string?>> SaveCommandSettings(string appname, string username, string category, string type, Dictionary<string, string> properties)
        {
            var previous = new Dictionary<string, string?>(StringComparer.Ordinal);

            if (!await _axExtend.OpenDBConnectionAsync(appname))
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");

            // What each property holds now: the old -> new answer, and it tells an update from an insert
            var current = (await GetCommandSettings(appname, category, type))
                .ToDictionary(r => r.PropertyName, r => r.PropertyValue, StringComparer.Ordinal);

            var db = await _axExtend.GetDB();
            var commands = new List<SQLCommandBuilder>();
            foreach (var pair in properties)
            {
                if (current.TryGetValue(pair.Key, out var old))
                {
                    previous[pair.Key] = old;
                    commands.Add(new SQLCommandBuilder(
                        "UPDATE axi_command_settings SET propertyvalue = :propertyvalue, username = :username, modifiedon = CURRENT_TIMESTAMP " +
                        "WHERE propertycategory = :propertycategory AND propertytype = :propertytype AND propertyname = :propertyname",
                        new[] { ":propertyvalue", ":username", ":propertycategory", ":propertytype", ":propertyname" },
                        new[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                        new object[] { pair.Value, username, category, type, pair.Key },
                        ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE));
                }
                else
                {
                    previous[pair.Key] = null;
                    // old code
                    // commands.Add(new SQLCommandBuilder(
                    //     "INSERT INTO axi_command_settings (id, propertycategory, propertytype, propertyname, propertyvalue, username, createdon, modifiedon) " +
                    //     "VALUES (:id, :propertycategory, :propertytype, :propertyname, :propertyvalue, :username, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)",
                    //     new[] { ":id", ":propertycategory", ":propertytype", ":propertyname", ":propertyvalue", ":username" },
                    //     new[] { DbType.Guid, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                    //     new object[] { Guid.NewGuid(), category, type, pair.Key, pair.Value, username },
                    //     ARMCommon.Helpers.Constants.DATA_ACTION.INSERT));
                    // new code: id comes from the column default (uuid in Postgres, VARCHAR2 in Oracle), like Axi_UserSettings
                    commands.Add(new SQLCommandBuilder(
                        "INSERT INTO axi_command_settings (propertycategory, propertytype, propertyname, propertyvalue, username, createdon, modifiedon) " +
                        "VALUES (:propertycategory, :propertytype, :propertyname, :propertyvalue, :username, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)",
                        new[] { ":propertycategory", ":propertytype", ":propertyname", ":propertyvalue", ":username" },
                        new[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                        new object[] { category, type, pair.Key, pair.Value, username },
                        ARMCommon.Helpers.Constants.DATA_ACTION.INSERT));
                }
            }

            // One transaction: all properties are saved together or not at all
            TransactionResult saved = await db.ExecuteSQLsInTransactionAsync(commands, true);
            if (saved == null || !saved.success)
            {
                _logger.LogError("Save command settings {Category}/{Type} failed: {Error}", category, type, saved?.error);
                throw new DatabaseException(saved?.error ?? "The settings could not be saved.", "UPSERT");
            }

            _logger.LogInformation("Command settings {Category}/{Type} saved by {Username} for appname: {AppName}", category, type, username, appname);
            return previous;
        }
    }
}
