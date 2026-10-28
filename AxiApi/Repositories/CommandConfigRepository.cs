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
    }
}
