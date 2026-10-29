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
using System.Linq;
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
                    configs.Add(MapConfig<CommandConfigDTO>(row));
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

            SQLResult prompts = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionPrompts);
            var options = ToOptionDtos(rowsResult?.data, prompts);
            _logger.LogInformation("Loaded {Count} command options for appname: {AppName}", options.Count, appname);
            return options;
        }

        public async Task<CommandOptionPageDTO> GetCommandOptionsPageAsync(string appname, string command, string search, int? pageNo, int pageSize)
        {
            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }
            var db = await _axExtend.GetDB();

            var page = new CommandOptionPageDTO { PageSize = pageSize };
            SQLResult perCommand = await db.ExecuteSQLAsync(SqlQueries.CountCommandOptionsByCommand);
            if (!string.IsNullOrEmpty(perCommand?.error)) throw new DatabaseException(perCommand.error, "SELECT");
            if (perCommand?.data != null)
            {
                foreach (DataRow r in perCommand.data.Rows)
                {
                    var n = Convert.ToInt32(r["cnt"]);
                    if (string.Equals(r["command"]?.ToString(), "sdk", StringComparison.OrdinalIgnoreCase)) page.SdkCount = n;
                    else page.ConfigureCount = n;
                }
            }

            // Parameters in the order they appear in the SQL (Oracle binds by position)
            var names = new List<string> { ":command" };
            var values = new List<object> { command.ToLowerInvariant() };
            var filter = string.Empty;
            if (search.Length > 0)
            {
                filter = SqlQueries.CommandOptionsSearch;
                var pattern = CommandOptionPaging.ContainsPattern(search);
                names.AddRange(new[] { ":search1", ":search2", ":search3" });
                values.AddRange(new object[] { pattern, pattern, pattern });
            }
            var types = names.Select(_ => DbType.String).ToArray();

            SQLResult counted = await db.ExecuteSQLAsync(string.Format(SqlQueries.CountCommandOptionsPage, filter), names.ToArray(), types, values.ToArray());
            if (!string.IsNullOrEmpty(counted?.error)) throw new DatabaseException(counted.error, "SELECT");
            page.TotalCount = counted?.data != null && counted.data.Rows.Count > 0 ? Convert.ToInt32(counted.data.Rows[0]["cnt"]) : 0;
            page.TotalPages = CommandOptionPaging.TotalPages(page.TotalCount, pageSize);
            page.PageNo = CommandOptionPaging.ClampPageNo(pageNo, page.TotalPages);

            var sql = string.Format(SqlQueries.SelectCommandOptionsPage, filter, (page.PageNo - 1) * pageSize, pageSize);
            SQLResult rows = await db.ExecuteSQLAsync(sql, names.ToArray(), types, values.ToArray());
            if (!string.IsNullOrEmpty(rows?.error)) throw new DatabaseException(rows.error, "SELECT");
            if (rows?.data != null && rows.data.Rows.Count > 0)
            {
                SQLResult prompts = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionPrompts);
                page.Items = ToOptionDtos(rows.data, prompts);
            }
            return page;
        }

        /// <summary>Option rows as DTOs, each with its record source taken from the prompt lists (axi_command_prompts).</summary>
        private List<CommandConfigOptionDTO> ToOptionDtos(DataTable? rows, SQLResult? promptResult)
        {
            var options = new List<CommandConfigOptionDTO>();
            if (rows == null || rows.Rows.Count == 0) return options;

            // Record source per command (cmdtoken from CommandOptionRules.CmdToken): option names on wordpos 2, sources on wordpos 3
            var sourcesByCommand = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(promptResult?.error) && promptResult?.data != null)
            {
                foreach (var command in CommandOptionRules.Commands)
                {
                    var (names, sources) = ReadPromptLists(promptResult.data, CommandOptionRules.CmdToken(command));
                    sourcesByCommand[command] = CommandOptionSources.Pair(names, sources);
                }
            }
            else if (!string.IsNullOrEmpty(promptResult?.error))
            {
                _logger.LogWarning("Record sources unavailable, listing options without them: {Error}", promptResult.error);
            }

            foreach (DataRow row in rows.Rows)
            {
                var command = row["command"]?.ToString() ?? string.Empty;
                var option = row["prompt_options"]?.ToString() ?? string.Empty;
                string recordSource = string.Empty;
                if (sourcesByCommand.TryGetValue(command, out var map))
                    map.TryGetValue(option.Trim(), out recordSource!);
                // The palette reads the prompt lists, so those win; the stored column fills in when the lists lack the option
                if (string.IsNullOrEmpty(recordSource) && row.Table.Columns.Contains("record_source") && row["record_source"] != DBNull.Value)
                    recordSource = row["record_source"]?.ToString() ?? string.Empty;

                options.Add(ToDto(row, command, option, recordSource));
            }
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

        /// <summary>
        /// Edits or renames a Custom Option. A rename replaces the entry in place in the option list (position and pairing kept) and
        /// the record source at the same position of the source list. A Built-in Option is refused any route change. Null = not found.
        /// </summary>
        public async Task<CommandConfigOptionDTO?> UpdateCommandOptionAsync(string appname, string configId, UpdateCommandOptionRequestDTO request)
        {
            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }
            var db = await _axExtend.GetDB();

            SQLResult found = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionRow,
                new[] { ":configid" }, new[] { DbType.String }, new object[] { configId });
            if (!string.IsNullOrEmpty(found?.error)) throw new DatabaseException(found.error, "SELECT");
            if (found?.data == null || found.data.Rows.Count == 0) return null;

            DataRow row = found.data.Rows[0];
            string Str(string col) => row[col] == DBNull.Value ? string.Empty : (row[col]?.ToString() ?? string.Empty);
            var command = CommandOptionRules.NormalizeCommand(Str("command")) ?? throw new CommandOptionException("Only Configure and SDK options can be changed.");
            int token = CommandOptionRules.CmdToken(command);
            var currentName = Str("prompt_options").Trim();
            bool isSystem = string.Equals(Str("is_system").Trim(), "T", StringComparison.OrdinalIgnoreCase);

            // Option list (wordpos 2) and paired source list (wordpos 3) of the command
            SQLResult prompts = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionPrompts);
            if (!string.IsNullOrEmpty(prompts?.error)) throw new DatabaseException(prompts.error, "SELECT");
            var (listNames, listSources) = ReadPromptLists(prompts?.data, token);
            CommandOptionSources.Pair(listNames, listSources).TryGetValue(currentName, out var currentSource);
            if (string.IsNullOrEmpty(currentSource)) currentSource = Str("record_source");

            if (isSystem)
            {
                // The route of a Built-in Option is fixed; only Active can change (SetCommandOptionActiveAsync)
                if (CommandOptionRules.ChangesRoute(request, currentName, Str("prompt_option_type"), Str("prompt_id"), Str("target_url"), Str("param_field"), Str("extra_params"), currentSource))
                    throw new CommandOptionException("Built-in Options keep their route. Only the Active switch can be changed.", false, true);
                return ToDto(row, command, currentName, currentSource);
            }

            if (!CommandOptionRules.ListsArePaired(listNames, listSources))
                throw new CommandOptionException($"The {command} option and source lists in axi_command_prompts are missing or out of step. Re-run the AxiCMD install scripts and try again.", true);

            // Names used by the command's other options, Built-in Options included; the option's own current name is allowed
            SQLResult existing = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionNames,
                new[] { ":command" }, new[] { DbType.String }, new object[] { command.ToLowerInvariant() });
            if (!string.IsNullOrEmpty(existing?.error)) throw new DatabaseException(existing.error, "SELECT");
            var usedNames = CollectUsedNames(existing?.data, listNames, configId);

            var option = CommandOptionRules.ValidateEdit(request, command, currentName, usedNames);
            var promptId = string.IsNullOrEmpty(option.PromptId) ? configId.Substring(4) : option.PromptId!;
            var replaced = CommandOptionRules.ReplaceOption(listNames, listSources, currentName, option.Name, option.RecordSource);

            object Db(string? v) => string.IsNullOrEmpty(v) ? DBNull.Value : v;
            var commands = new List<SQLCommandBuilder>
            {
                new SQLCommandBuilder(SqlQueries.UpdateCustomCommandOption,
                    new[] { ":promptoptions", ":promptid", ":optiontype", ":paramfield", ":targeturl", ":extraparams", ":recordsource", ":configid" },
                    new[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                    // None is stored as NULL so a re-install restores Axi_Dummy
                    new object[] { option.Name, promptId, option.Type, Db(option.ParamField), Db(option.TargetUrl), Db(option.ExtraParams), option.RecordSource == CommandOptionRules.NoSource ? DBNull.Value : option.RecordSource!, configId },
                    ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE)
            };
            if (replaced.Names != listNames)
                commands.Add(new SQLCommandBuilder(string.Format(SqlQueries.ReplaceCommandOptionNames, token),
                    new[] { ":value" }, new[] { DbType.String }, new object[] { replaced.Names }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE));
            if (replaced.Sources != listSources)
                commands.Add(new SQLCommandBuilder(string.Format(SqlQueries.ReplaceCommandOptionSources, token),
                    new[] { ":value" }, new[] { DbType.String }, new object[] { replaced.Sources }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE));

            // One transaction: the row and both lists are saved together or not at all
            TransactionResult saved = await db.ExecuteSQLsInTransactionAsync(commands, true);
            if (saved == null || !saved.success)
            {
                _logger.LogError("Update command option {ConfigId} failed: {Error}", configId, saved?.error);
                throw new DatabaseException(saved?.error ?? "The option could not be saved.", "UPDATE");
            }

            _logger.LogInformation("Custom Option {ConfigId} updated in {Command} for appname: {AppName}", configId, command, appname);
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
                Active = Str("active").Length == 0 ? "T" : Str("active"),
                IsSystem = "F",
                RecordSource = option.RecordSource ?? CommandOptionRules.NoSource
            };
        }

        private static string? Opt(DataRow row, string col) =>
            row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? row[col]?.ToString() : null;

        /// <summary>The columns every command-config query returns, as a DTO.</summary>
        private static T MapConfig<T>(DataRow row) where T : CommandConfigDTO, new() => new T
        {
            ConfigId = Opt(row, "config_id") ?? string.Empty,
            Command = Opt(row, "command") ?? string.Empty,
            PromptOptions = Opt(row, "prompt_options") ?? string.Empty,
            PromptId = Opt(row, "prompt_id") ?? string.Empty,
            PromptOptionType = Opt(row, "prompt_option_type") ?? string.Empty,
            ParamField = Opt(row, "param_field"),
            TargetUrl = Opt(row, "target_url"),
            ExtraParams = Opt(row, "extra_params"),
            Active = Opt(row, "active") ?? "T"
        };

        private static CommandConfigOptionDTO ToDto(DataRow row, string command, string name, string? recordSource)
        {
            var dto = MapConfig<CommandConfigOptionDTO>(row);
            dto.Command = command;
            dto.PromptOptions = name;
            dto.IsSystem = Opt(row, "is_system") ?? "F";
            dto.RecordSource = recordSource ?? string.Empty;
            return dto;
        }

        /// <summary>Option list (wordpos 2, promptvalues) and paired source list (wordpos 3, promptsource) of a command's cmdtoken.</summary>
        private static (string? Names, string? Sources) ReadPromptLists(DataTable? prompts, int token)
        {
            string? names = null, sources = null;
            if (prompts == null) return (names, sources);
            foreach (DataRow p in prompts.Rows)
            {
                if (Convert.ToInt32(p["cmdtoken"]) != token) continue;
                var wordpos = Convert.ToInt32(p["wordpos"]);
                if (wordpos == 2) names = p["promptvalues"] == DBNull.Value ? null : p["promptvalues"]?.ToString();
                else if (wordpos == 3) sources = p["promptsource"] == DBNull.Value ? null : p["promptsource"]?.ToString();
            }
            return (names, sources);
        }

        /// <summary>Names already used by a command: its config rows (except exceptConfigId) plus the entries of the option list. usedIds, when given, gets every config id.</summary>
        private static List<string> CollectUsedNames(DataTable? existingRows, string? listNames, string? exceptConfigId = null, ISet<string>? usedIds = null)
        {
            var usedNames = new List<string>();
            if (existingRows != null)
            {
                foreach (DataRow r in existingRows.Rows)
                {
                    var id = r["config_id"]?.ToString() ?? string.Empty;
                    usedIds?.Add(id);
                    if (exceptConfigId == null || !string.Equals(id, exceptConfigId, StringComparison.OrdinalIgnoreCase))
                        usedNames.Add(r["prompt_options"]?.ToString() ?? string.Empty);
                }
            }
            usedNames.AddRange(CommandOptionRules.SplitList(listNames));
            return usedNames;
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
            var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // The palette's option list (wordpos 2) and the paired source list (wordpos 3)
            SQLResult prompts = await db.ExecuteSQLAsync(SqlQueries.SelectCommandOptionPrompts);
            if (!string.IsNullOrEmpty(prompts?.error)) throw new DatabaseException(prompts.error, "SELECT");
            var (listNames, listSources) = ReadPromptLists(prompts?.data, token);
            if (!CommandOptionRules.ListsArePaired(listNames, listSources))
                throw new CommandOptionException($"The {command} option and source lists in axi_command_prompts are missing or out of step. Re-run the AxiCMD install scripts and try again.", true);
            var usedNames = CollectUsedNames(existing?.data, listNames, null, usedIds);

            var option = CommandOptionRules.Validate(request, usedNames);
            var configId = CommandOptionRules.MakeConfigId(command, option.Name);
            if (usedIds.Contains(configId))
                throw new CommandOptionException($"An existing option already uses the id '{configId}'. Choose a name that differs by more than punctuation.", true);
            var promptId = string.IsNullOrEmpty(option.PromptId) ? configId.Substring(4) : option.PromptId!;

            // Same limit checks as the page script. A clean list is extended in SQL (two admins saving together do not overwrite
            // each other); a list with stray commas or spaces is saved normalised instead, or the append would leave ",," in it.
            var appended = CommandOptionRules.AppendOption(listNames, listSources, option.Name, option.RecordSource);
            bool clean = CommandOptionRules.IsCanonicalList(listNames) && CommandOptionRules.IsCanonicalList(listSources);

            object Db(string? v) => string.IsNullOrEmpty(v) ? DBNull.Value : v;
            var commands = new List<SQLCommandBuilder>
            {
                new SQLCommandBuilder(SqlQueries.InsertCustomCommandOption,
                    new[] { ":configid", ":command", ":promptoptions", ":promptid", ":optiontype", ":paramfield", ":targeturl", ":extraparams", ":recordsource" },
                    new[] { DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String, DbType.String },
                    // None is stored as NULL so a re-install restores Axi_Dummy
                    new object[] { configId, command, option.Name, promptId, option.Type, Db(option.ParamField), Db(option.TargetUrl), Db(option.ExtraParams), option.RecordSource == CommandOptionRules.NoSource ? DBNull.Value : option.RecordSource! },
                    ARMCommon.Helpers.Constants.DATA_ACTION.INSERT),
                clean
                    ? new SQLCommandBuilder(string.Format(SqlQueries.AppendCommandOptionName, token),
                        new[] { ":suffix" }, new[] { DbType.String }, new object[] { "," + option.Name }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE)
                    : new SQLCommandBuilder(string.Format(SqlQueries.ReplaceCommandOptionNames, token),
                        new[] { ":value" }, new[] { DbType.String }, new object[] { appended.Names }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE),
                clean
                    ? new SQLCommandBuilder(string.Format(SqlQueries.AppendCommandOptionSource, token),
                        new[] { ":suffix" }, new[] { DbType.String }, new object[] { "," + option.RecordSource }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE)
                    : new SQLCommandBuilder(string.Format(SqlQueries.ReplaceCommandOptionSources, token),
                        new[] { ":value" }, new[] { DbType.String }, new object[] { appended.Sources }, ARMCommon.Helpers.Constants.DATA_ACTION.UPDATE)
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
