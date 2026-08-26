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

        private static object? GetValue(DataRow row, string colName)
        {
            if (row.Table.Columns.Contains(colName))
                return row[colName];
            if (row.Table.Columns.Contains(colName.ToUpperInvariant()))
                return row[colName.ToUpperInvariant()];
            if (row.Table.Columns.Contains(colName.ToLowerInvariant()))
                return row[colName.ToLowerInvariant()];
            return null;
        }

        private static string? GetString(DataRow row, string colName)
        {
            var val = GetValue(row, colName);
            return (val != null && val != DBNull.Value) ? val.ToString() : null;
        }

        private static CommandConfigDTO MapCommandConfig(DataRow row)
        {
            return new CommandConfigDTO
            {
                ConfigId = GetString(row, "config_id") ?? string.Empty,
                Command = GetString(row, "command") ?? string.Empty,
                PromptOptions = GetString(row, "prompt_options") ?? string.Empty,
                PromptId = GetString(row, "prompt_id") ?? string.Empty,
                PromptOptionType = GetString(row, "prompt_option_type") ?? string.Empty,
                ParamField = GetString(row, "param_field"),
                TargetUrl = GetString(row, "target_url"),
                ExtraParams = GetString(row, "extra_params"),
                Active = GetString(row, "active") ?? "T"
            };
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
                    configs.Add(MapCommandConfig(row));
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
                    configs.Add(MapCommandConfig(row));
                }
            }

            _logger.LogInformation("Loaded {Count} total command configs for appname: {AppName}", configs.Count, appname);
            return configs;
        }

        public async Task<PagedResultDTO<CommandConfigDTO>> GetPagedCommandConfigsAsync(string appname, int pageIndex, int pageSize, string? search = null, string? command = null)
        {
            _logger.LogInformation("GetPagedCommandConfigsAsync called for appname: {AppName}, pageIndex: {PageIndex}, pageSize: {PageSize}, search: {Search}, command: {Command}", appname, pageIndex, pageSize, search, command);

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            var result = new PagedResultDTO<CommandConfigDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);
            if (!isDbConnected)
            {
                _logger.LogError("Failed to open DB connection for appname: {AppName}", appname);
                throw new DatabaseException($"Failed to connect to database for application: {appname}", "CONNECT");
            }

            var db = await _axExtend.GetDB();

            // Build dynamic filters
            var whereClauses = new List<string>();

            if (!string.IsNullOrWhiteSpace(command) && !command.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                whereClauses.Add($"LOWER(command) = '{command.Replace("'", "''").ToLower()}'");
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                string cleanSearch = search.Replace("'", "''").ToLower();
                whereClauses.Add($"(LOWER(config_id) LIKE '%{cleanSearch}%' OR LOWER(command) LIKE '%{cleanSearch}%' OR LOWER(prompt_options) LIKE '%{cleanSearch}%' OR LOWER(prompt_id) LIKE '%{cleanSearch}%' OR LOWER(target_url) LIKE '%{cleanSearch}%')");
            }

            string whereSql = whereClauses.Count > 0 ? " WHERE " + string.Join(" AND ", whereClauses) : "";

            // 1. Get Total Count
            string countSql = $"SELECT COUNT(1) FROM axi_command_config{whereSql}";
            SQLResult countResult = await db.ExecuteSQLAsync(countSql);
            if (countResult?.data != null && countResult.data.Rows.Count > 0)
            {
                int.TryParse(countResult.data.Rows[0][0]?.ToString(), out int totalCount);
                result.TotalCount = totalCount;
            }

            // 2. Query Page Rows
            int offset = (pageIndex - 1) * pageSize;
            string pageSql = $"SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active " +
                             $"FROM axi_command_config{whereSql} " +
                             $"ORDER BY command, prompt_options " +
                             $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            SQLResult sqlResult = await db.ExecuteSQLAsync(pageSql);

            // Fallback for older database versions without OFFSET FETCH
            if (!string.IsNullOrEmpty(sqlResult?.error) || sqlResult?.data == null)
            {
                string fallbackSql = $"SELECT config_id, command, prompt_options, prompt_id, prompt_option_type, param_field, target_url, extra_params, active FROM axi_command_config{whereSql} ORDER BY command, prompt_options";
                sqlResult = await db.ExecuteSQLAsync(fallbackSql);

                if (!string.IsNullOrEmpty(sqlResult?.error))
                {
                    _logger.LogError("Database error in GetPagedCommandConfigsAsync: {Error}", sqlResult.error);
                    throw new DatabaseException(sqlResult.error, "SELECT");
                }

                if (sqlResult?.data != null && sqlResult.data.Rows.Count > 0)
                {
                    result.TotalCount = sqlResult.data.Rows.Count;
                    var rows = sqlResult.data.Rows.Cast<DataRow>().Skip(offset).Take(pageSize);
                    foreach (DataRow row in rows)
                    {
                        result.Items.Add(MapCommandConfig(row));
                    }
                }
            }
            else
            {
                foreach (DataRow row in sqlResult.data.Rows)
                {
                    result.Items.Add(MapCommandConfig(row));
                }
            }

            return result;
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

            // Check if config exists (case-insensitive)
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
                    var tokenStr = GetString(row, "cmdtoken");
                    if (!string.IsNullOrEmpty(tokenStr))
                    {
                        int.TryParse(tokenStr, out cmdToken);
                    }

                    int? wordPos = null;
                    var wordPosStr = GetString(row, "wordpos");
                    if (!string.IsNullOrEmpty(wordPosStr) && int.TryParse(wordPosStr, out int parsedWordPos))
                    {
                        wordPos = parsedWordPos;
                    }

                    prompts.Add(new CommandPromptDTO
                    {
                        Id = GetString(row, "id"),
                        CmdToken = cmdToken,
                        Command = GetString(row, "command"),
                        CommandGroup = GetString(row, "command_group"),
                        WordPos = wordPos,
                        Prompt = GetString(row, "prompt"),
                        PromptSource = GetString(row, "promptsource"),
                        PromptParams = GetString(row, "promptparams"),
                        PromptValues = GetString(row, "promptvalues"),
                        ExtraParams = GetString(row, "extraparams"),
                        RequestUrl = GetString(row, "requesturl")
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

            string cleanValues = (request.PromptValues ?? "").Replace("'", "''");
            string cleanSource = (request.PromptSource ?? "").Replace("'", "''");

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
