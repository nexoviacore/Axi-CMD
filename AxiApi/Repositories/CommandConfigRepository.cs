using ARMCommon.Model;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Exceptions;
using AxiApi.Interfaces;
using AxiApi.Lib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AxiApi.Repositories
{
    public class CommandConfigRepository : ICommandConfigRepository
    {
        private readonly IAxExtend _axExtend;
        private readonly ILogger<CommandConfigRepository> _logger;

        public CommandConfigRepository(IAxExtend axExtend, ILogger<CommandConfigRepository> logger)
        {
            _axExtend = axExtend;
            _logger = logger;
        }

        public async Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname)
        {
            _logger.LogInformation("GetCommandConfigsAsync called for appname: {AppName}", appname);

            var configs = new List<CommandConfigDTO>();
            string sql = SqlQueries.SelectActiveCommandConfigs;

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();
            SQLResult sqlResult = await db.ExecuteSQLAsync(sql);

            if (!string.IsNullOrEmpty(sqlResult?.error))
            {
                _logger.LogError("Database error in GetCommandConfigsAsync: {Error}", sqlResult.error);
                throw new DatabaseException(sqlResult.error, "SELECT");
            }

            if (sqlResult?.data != null && sqlResult.data.Rows.Count > 0)
            {
                foreach (DataRow row in sqlResult.data.Rows)
                {
                    configs.Add(new CommandConfigDTO
                    {
                        ConfigId = row["config_id"]?.ToString() ?? string.Empty,
                        Command = row["command"]?.ToString() ?? string.Empty,
                        PromptOptions = row["prompt_options"]?.ToString() ?? string.Empty,
                        PromptId = row["prompt_id"]?.ToString() ?? string.Empty,
                        PromptOptionType = row["prompt_option_type"]?.ToString() ?? string.Empty,
                        ParamField = row["param_field"] != DBNull.Value ? row["param_field"]?.ToString() : null,
                        TargetUrl = row["target_url"] != DBNull.Value ? row["target_url"]?.ToString() : null,
                        ExtraParams = row["extra_params"] != DBNull.Value ? row["extra_params"]?.ToString() : null,
                        Active = row["active"]?.ToString() ?? "T"
                    });
                }
            }

            _logger.LogInformation("Loaded {Count} command configs for appname: {AppName}", configs.Count, appname);
            return configs;
        }

        public async Task<List<CommandConfigOptionDTO>> GetAllCommandOptionsAsync(string appname)
        {
            _logger.LogInformation("GetAllCommandOptionsAsync called for appname: {AppName}", appname);

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();
            SQLResult rowsResult = await db.ExecuteSQLAsync(SqlQueries.SelectAllCommandConfigs);
            if (!string.IsNullOrEmpty(rowsResult?.error))
            {
                _logger.LogError("Database error in GetAllCommandOptionsAsync: {Error}", rowsResult.error);
                throw new DatabaseException(rowsResult.error, "SELECT");
            }

            // Record source per command (cmdtoken 4 = Configure, 7 = SDK): option names on wordpos 2, sources on wordpos 3
            var sourcesByCommand = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            SQLResult promptResult = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionPrompts);
            if (string.IsNullOrEmpty(promptResult?.error) && promptResult?.data != null)
            {
                foreach (var (token, command) in new[] { (4, "Configure"), (7, "SDK") })
                {
                    string? names = null, sources = null;
                    foreach (DataRow p in promptResult.data.Rows)
                    {
                        if (Convert.ToInt32(p["cmdtoken"]) != token) continue;
                        var wordpos = Convert.ToInt32(p["wordpos"]);
                        if (wordpos == 2) names = p["promptvalues"]?.ToString();
                        else if (wordpos == 3) sources = p["promptsource"]?.ToString();
                    }
                    sourcesByCommand[command] = CommandOptionSources.Pair(names, sources);
                }
            }
            else if (!string.IsNullOrEmpty(promptResult?.error))
            {
                _logger.LogWarning("Record sources unavailable, listing options without them: {Error}", promptResult.error);
            }

            var options = new List<CommandConfigOptionDTO>();
            if (rowsResult?.data != null)
            {
                foreach (DataRow row in rowsResult.data.Rows)
                {
                    var command = row["command"]?.ToString() ?? string.Empty;
                    var option = row["prompt_options"]?.ToString() ?? string.Empty;
                    string recordSource = string.Empty;
                    if (sourcesByCommand.TryGetValue(command, out var map))
                        map.TryGetValue(option.Trim(), out recordSource!);

                    options.Add(new CommandConfigOptionDTO
                    {
                        ConfigId = row["config_id"]?.ToString() ?? string.Empty,
                        Command = command,
                        PromptOptions = option,
                        PromptId = row["prompt_id"]?.ToString() ?? string.Empty,
                        PromptOptionType = row["prompt_option_type"]?.ToString() ?? string.Empty,
                        ParamField = row["param_field"] != DBNull.Value ? row["param_field"]?.ToString() : null,
                        TargetUrl = row["target_url"] != DBNull.Value ? row["target_url"]?.ToString() : null,
                        ExtraParams = row["extra_params"] != DBNull.Value ? row["extra_params"]?.ToString() : null,
                        Active = row["active"]?.ToString() ?? "T",
                        IsSystem = row["is_system"] != DBNull.Value ? (row["is_system"]?.ToString() ?? "F") : "F",
                        RecordSource = recordSource ?? string.Empty
                    });
                }
            }

            _logger.LogInformation("Loaded {Count} command options for appname: {AppName}", options.Count, appname);
            return options;
        }

        public async Task<bool> SetCommandOptionActiveAsync(string appname, string configId, bool active)
        {
            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();
            string[] idNames = { ":configid" };
            DbType[] idTypes = { DbType.String };
            object[] idValues = { configId };

            SQLResult found = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionById, idNames, idTypes, idValues);
            if (!string.IsNullOrEmpty(found?.error))
                throw new DatabaseException(found.error, "SELECT");
            if (found?.data == null || found.data.Rows.Count == 0)
                return false;

            string[] names = { ":active", ":configid" };
            DbType[] types = { DbType.String, DbType.String };
            object[] values = { active ? "T" : "F", configId };
            var updated = await db.ExecuteNonQueryAsync(SqlQueries.UpdateCommandOptionActive, names, types, values);
            if (!string.IsNullOrEmpty(updated?.error))
                throw new DatabaseException(updated.error, "UPDATE");

            _logger.LogInformation("Command option {ConfigId} set active={Active} for appname: {AppName}", configId, active, appname);
            return true;
        }

        public async Task<CommandConfigOptionDTO> CreateCommandOptionAsync(string appname, CreateCommandOptionRequestDTO request)
        {
            var command = CommandOptionRules.NormalizeCommand(request.Command) ?? throw new CommandOptionException("Choose Configure or SDK.");
            int token = CommandOptionRules.CmdToken(command);

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }
            var db = await _axExtend.GetDB();

            // Names and ids already used by this command, Built-in Options included, active or not
            SQLResult existing = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionNames,
                new[] { ":command" }, new[] { DbType.String }, new object[] { command.ToLowerInvariant() });
            if (!string.IsNullOrEmpty(existing?.error)) throw new DatabaseException(existing.error, "SELECT");
            var usedNames = new List<string>();
            var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (existing?.data != null)
            {
                foreach (DataRow r in existing.data.Rows)
                {
                    usedIds.Add(r["config_id"]?.ToString() ?? string.Empty);
                    usedNames.Add(r["prompt_options"]?.ToString() ?? string.Empty);
                }
            }

            // The palette's option list (wordpos 2) and the paired source list (wordpos 3)
            SQLResult prompts = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionPrompts);
            if (!string.IsNullOrEmpty(prompts?.error)) throw new DatabaseException(prompts.error, "SELECT");
            string? listNames = null, listSources = null;
            if (prompts?.data != null)
            {
                foreach (DataRow p in prompts.data.Rows)
                {
                    if (Convert.ToInt32(p["cmdtoken"]) != token) continue;
                    var wordpos = Convert.ToInt32(p["wordpos"]);
                    if (wordpos == 2) listNames = p["promptvalues"] == DBNull.Value ? null : p["promptvalues"]?.ToString();
                    else if (wordpos == 3) listSources = p["promptsource"] == DBNull.Value ? null : p["promptsource"]?.ToString();
                }
            }
            if (!CommandOptionRules.ListsArePaired(listNames, listSources))
                throw new CommandOptionException($"The {command} option and source lists in axi_command_prompts are missing or out of step. Re-run the AxiCMD install scripts and try again.", true);
            usedNames.AddRange(listNames!.Split(','));

            var option = CommandOptionRules.Validate(request, usedNames);
            var configId = CommandOptionRules.MakeConfigId(command, option.Name);
            if (usedIds.Contains(configId))
                throw new CommandOptionException($"An existing option already uses the id '{configId}'. Choose a name that differs by more than punctuation.", true);
            var promptId = string.IsNullOrEmpty(option.PromptId) ? configId.Substring(4) : option.PromptId!;

            // Same limit checks as the page script; the lists themselves are extended in SQL
            CommandOptionRules.AppendOption(listNames, listSources, option.Name, option.RecordSource);

            object Db(string? v) => string.IsNullOrEmpty(v) ? DBNull.Value : v;
            var commands = new List<SQLCommandBuilder>
            {
                new SQLCommandBuilder(SqlQueries.InsertCustomCommandOption,
                    new[] { ":configid", ":command", ":promptoptions", ":promptid", ":optiontype", ":paramfield", ":targeturl", ":extraparams" },
                    new[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                    new object[] { configId, command, option.Name, promptId, option.Type, Db(option.ParamField), Db(option.TargetUrl), Db(option.ExtraParams) },
                    ARMCommon.Helpers.Constants.DATA_ACTION.INSERT),
                new SQLCommandBuilder(string.Format(SqlQueries.AppendCommandOptionName, token),
                    new[] { ":suffix" }, new[] { DbType.String }, new object[] { "," + option.Name }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE),
                new SQLCommandBuilder(string.Format(SqlQueries.AppendCommandOptionSource, token),
                    new[] { ":suffix" }, new[] { DbType.String }, new object[] { "," + option.RecordSource }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE)
            };

            // One transaction: the row and both lists are saved together or not at all
            TransactionResult saved = await db.ExecuteSQLsInTransactionAsync(commands, true);
            if (saved == null || !saved.success)
            {
                _logger.LogError("Create command option {ConfigId} failed: {Error}", configId, saved?.error);
                throw new DatabaseException(saved?.error ?? "The option could not be saved.", "INSERT");
            }

            _logger.LogInformation("Custom Option {ConfigId} added to {Command} for appname: {AppName}", configId, command, appname);
            return new CommandConfigOptionDTO
            {
                ConfigId = configId,
                Command = command,
                PromptOptions = option.Name,
                PromptId = promptId,
                PromptOptionType = option.Type,
                ParamField = string.IsNullOrEmpty(option.ParamField) ? null : option.ParamField,
                TargetUrl = string.IsNullOrEmpty(option.TargetUrl) ? null : option.TargetUrl,
                ExtraParams = string.IsNullOrEmpty(option.ExtraParams) ? null : option.ExtraParams,
                Active = "T",
                IsSystem = "F",
                RecordSource = option.RecordSource ?? CommandOptionRules.NoSource
            };
        }
    }
}
