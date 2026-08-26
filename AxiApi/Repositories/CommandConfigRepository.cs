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
        public async Task<List<CommandConfigDTO>> GetAllCommandConfigsAsync(string appname)
        {
            _logger.LogInformation("GetAllCommandConfigsAsync called for appname: {AppName}", appname);

            var configs = new List<CommandConfigDTO>();
            string sql = SqlQueries.SelectAllCommandConfigs;

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
                _logger.LogError("Database error in GetAllCommandConfigsAsync: {Error}", sqlResult.error);
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

            _logger.LogInformation("Loaded {Count} total command configs for appname: {AppName}", configs.Count, appname);
            return configs;
        }

        public async Task<bool> SaveCommandConfigAsync(CommandConfigDTO config, string appname)
        {
            _logger.LogInformation("SaveCommandConfigAsync called for config_id: {ConfigId}, appname: {AppName}", config.ConfigId, appname);

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();

            // Check if config exists
            string checkSql = $"SELECT COUNT(1) FROM axi_command_config WHERE LOWER(config_id) = '{config.ConfigId.Replace("'", "''").ToLower()}'";
            SQLResult checkResult = await db.ExecuteSQLAsync(checkSql);
            int count = 0;
            if (checkResult?.data != null && checkResult.data.Rows.Count > 0)
            {
                int.TryParse(checkResult.data.Rows[0][0]?.ToString(), out count);
            }

            string cleanField(string? val) => val == null ? "NULL" : $"'{val.Replace("'", "''")}'";

            string executeSql;
            if (count > 0)
            {
                executeSql = $"UPDATE axi_command_config SET " +
                             $"command = {cleanField(config.Command)}, " +
                             $"prompt_options = {cleanField(config.PromptOptions)}, " +
                             $"prompt_id = {cleanField(config.PromptId)}, " +
                             $"prompt_option_type = {cleanField(config.PromptOptionType)}, " +
                             $"param_field = {cleanField(config.ParamField)}, " +
                             $"target_url = {cleanField(config.TargetUrl)}, " +
                             $"extra_params = {cleanField(config.ExtraParams)}, " +
                             $"active = {cleanField(config.Active ?? "T")} " +
                             $"WHERE LOWER(config_id) = '{config.ConfigId.Replace("'", "''").ToLower()}'";
            }
            else
            {
                executeSql = $"INSERT INTO axi_command_config (config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active) VALUES (" +
                             $"{cleanField(config.ConfigId)}, " +
                             $"{cleanField(config.Command)}, " +
                             $"{cleanField(config.PromptOptions)}, " +
                             $"{cleanField(config.PromptId)}, " +
                             $"{cleanField(config.PromptOptionType)}, " +
                             $"{cleanField(config.ParamField)}, " +
                             $"{cleanField(config.TargetUrl)}, " +
                             $"{cleanField(config.ExtraParams)}, " +
                             $"{cleanField(config.Active ?? "T")})";
            }

            var nonQueryResult = await db.ExecuteNonQueryAsync(executeSql);
            if (!string.IsNullOrEmpty(nonQueryResult?.error))
            {
                _logger.LogError("Database error in SaveCommandConfigAsync: {Error}", nonQueryResult.error);
                throw new DatabaseException(nonQueryResult.error, count > 0 ? "UPDATE" : "INSERT");
            }

            return true;
        }

        public async Task<bool> DeleteCommandConfigAsync(string configId, string appname)
        {
            _logger.LogInformation("DeleteCommandConfigAsync called for config_id: {ConfigId}, appname: {AppName}", configId, appname);

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();
            string sql = $"DELETE FROM axi_command_config WHERE LOWER(config_id) = '{configId.Replace("'", "''").ToLower()}'";

            var nonQueryResult = await db.ExecuteNonQueryAsync(sql);
            if (!string.IsNullOrEmpty(nonQueryResult?.error))
            {
                _logger.LogError("Database error in DeleteCommandConfigAsync: {Error}", nonQueryResult.error);
                throw new DatabaseException(nonQueryResult.error, "DELETE");
            }

            return true;
        }

        public async Task<List<CommandPromptDTO>> GetCommandPromptsAsync(string appname)
        {
            _logger.LogInformation("GetCommandPromptsAsync called for appname: {AppName}", appname);

            var prompts = new List<CommandPromptDTO>();
            string sql = SqlQueries.SelectCommandPrompts;

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
                _logger.LogError("Database error in GetCommandPromptsAsync: {Error}", sqlResult.error);
                throw new DatabaseException(sqlResult.error, "SELECT");
            }

            if (sqlResult?.data != null && sqlResult.data.Rows.Count > 0)
            {
                foreach (DataRow row in sqlResult.data.Rows)
                {
                    int cmdToken = 0;
                    int.TryParse(row["cmdtoken"]?.ToString(), out cmdToken);

                    int? wordPos = null;
                    if (row["wordpos"] != DBNull.Value && int.TryParse(row["wordpos"]?.ToString(), out int parsedWordPos))
                    {
                        wordPos = parsedWordPos;
                    }

                    prompts.Add(new CommandPromptDTO
                    {
                        Id = row["id"]?.ToString(),
                        CmdToken = cmdToken,
                        Command = row["command"] != DBNull.Value ? row["command"]?.ToString() : null,
                        CommandGroup = row["command_group"] != DBNull.Value ? row["command_group"]?.ToString() : null,
                        WordPos = wordPos,
                        Prompt = row["prompt"] != DBNull.Value ? row["prompt"]?.ToString() : null,
                        PromptSource = row["promptsource"] != DBNull.Value ? row["promptsource"]?.ToString() : null,
                        PromptParams = row["promptparams"] != DBNull.Value ? row["promptparams"]?.ToString() : null,
                        PromptValues = row["promptvalues"] != DBNull.Value ? row["promptvalues"]?.ToString() : null,
                        ExtraParams = row["extraparams"] != DBNull.Value ? row["extraparams"]?.ToString() : null,
                        RequestUrl = row["requesturl"] != DBNull.Value ? row["requesturl"]?.ToString() : null
                    });
                }
            }

            _logger.LogInformation("Loaded {Count} prompt rows for appname: {AppName}", prompts.Count, appname);
            return prompts;
        }

        public async Task<bool> SaveCommandPromptAsync(SavePromptRequestDTO request, string appname)
        {
            _logger.LogInformation("SaveCommandPromptAsync called for cmdtoken: {CmdToken}, wordpos: {WordPos}, appname: {AppName}", request.CmdToken, request.WordPos, appname);

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();

            string cleanValues = request.PromptValues.Replace("'", "''");
            string cleanSource = request.PromptSource.Replace("'", "''");

            string sql = $"UPDATE axi_command_prompts SET promptvalues = '{cleanValues}', promptsource = '{cleanSource}' " +
                         $"WHERE cmdtoken = {request.CmdToken} AND wordpos = {request.WordPos}";

            var nonQueryResult = await db.ExecuteNonQueryAsync(sql);
            if (!string.IsNullOrEmpty(nonQueryResult?.error))
            {
                _logger.LogError("Database error in SaveCommandPromptAsync: {Error}", nonQueryResult.error);
                throw new DatabaseException(nonQueryResult.error, "UPDATE");
            }

            return true;
        }
    }
}
