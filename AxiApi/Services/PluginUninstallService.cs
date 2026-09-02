using AxiApi.DTOs;
using AxiApi.Interfaces;
using AxExtend.Interface;

namespace AxiApi.Services
{
    public class PluginUninstallService : IPluginUninstallService
    {
        private const string AxiCmdPluginId = "Axi_Beta_2";
        private const string AxiCmdPluginName = "Axi CMD";
        private const string AxiCmdPageRelativePath = "CustomPages/AxiCMDMainPage2.html";
        private const string AxiCmdPluginRelativePath = "AxpertPlugins/Axi_Beta_2";
        private const long AxiCmdAdsFirstId = 99999999990001;
        private const long AxiCmdAdsLastId = 99999999990052;

        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PluginUninstallService> _logger;
        private readonly IAxExtend _axExtend;
        private readonly IConfiguration _configuration;

        public PluginUninstallService(IWebHostEnvironment environment, ILogger<PluginUninstallService> logger, IAxExtend axExtend, IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            _axExtend = axExtend;
            _configuration = configuration;
        }

        public IReadOnlyList<PluginDTO> GetInstalledPlugins()
        {
            var webRoot = GetConfiguredAxpertWebRoot();
            var pluginPath = GetPathWithinWebRoot(webRoot, AxiCmdPluginRelativePath);

            if (!Directory.Exists(pluginPath))
            {
                return Array.Empty<PluginDTO>();
            }

            return new[]
            {
                new PluginDTO
                {
                    Id = AxiCmdPluginId,
                    Name = AxiCmdPluginName,
                    IsAxiCmd = true
                }
            };
        }

        public async Task<ApiResponseDTO> UninstallAxiCmdAsync(string appname)
        {
            var webRoot = GetConfiguredAxpertWebRoot();
            var pluginPath = GetPathWithinWebRoot(webRoot, AxiCmdPluginRelativePath);
            var pagePath = GetPathWithinWebRoot(webRoot, AxiCmdPageRelativePath);

            if (!Directory.Exists(pluginPath))
            {
                throw new KeyNotFoundException("Axi CMD is not installed.");
            }
            if (!File.Exists(pagePath))
            {
                throw new KeyNotFoundException("AxiCMDMainPage2.html is not installed.");
            }

            var stagingRoot = GetPathWithinWebRoot(webRoot, ".axi-uninstall");
            var operationPath = Path.Combine(stagingRoot, Guid.NewGuid().ToString("N"));
            var stagedPluginPath = Path.Combine(operationPath, AxiCmdPluginId);
            var stagedPagePath = Path.Combine(operationPath, "AxiCMDMainPage2.html");
            var pageWasMoved = false;
            var databaseArtifactsRemoved = false;

            try
            {
                Directory.CreateDirectory(operationPath);
                Directory.Move(pluginPath, stagedPluginPath);

                File.Move(pagePath, stagedPagePath);
                pageWasMoved = true;

                await RemoveDatabaseArtifactsAsync(appname);
                databaseArtifactsRemoved = true;

                try
                {
                    Directory.Delete(operationPath, true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Axi CMD database artifacts were removed, but staged plugin files could not be deleted from {OperationPath}.", operationPath);
                    throw;
                }

                _logger.LogInformation("Axi CMD database artifacts, plugin, and AxiCMDMainPage2.html were uninstalled.");

                return new ApiResponseDTO
                {
                    Success = true,
                    Message = "Axi CMD was uninstalled.",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch
            {
                if (!databaseArtifactsRemoved)
                {
                    RestoreTargets(pluginPath, pagePath, stagedPluginPath, stagedPagePath, pageWasMoved);
                }
                throw;
            }
        }

        private async Task RemoveDatabaseArtifactsAsync(string appname)
        {
            if (!await _axExtend.OpenDBConnectionAsync(appname))
            {
                throw new InvalidOperationException($"Unable to connect to the database for application '{appname}'.");
            }

            var db = await _axExtend.GetDB();
            var postgresProbe = await db.ExecuteSQLAsync("SELECT version()");
            var isPostgreSql = string.IsNullOrWhiteSpace(postgresProbe?.error);

            if (!isPostgreSql)
            {
                var oracleProbe = await db.ExecuteSQLAsync("SELECT 1 FROM dual");
                if (!string.IsNullOrWhiteSpace(oracleProbe?.error))
                {
                    throw new InvalidOperationException("Axi CMD uninstall supports only PostgreSQL and Oracle databases.");
                }
            }

            await ExecuteCleanupSqlAsync(db, $"DELETE FROM axdirectsql_metadata WHERE axdirectsqlid BETWEEN {AxiCmdAdsFirstId} AND {AxiCmdAdsLastId}");
            await ExecuteCleanupSqlAsync(db, $"DELETE FROM axdirectsql WHERE axdirectsqlid BETWEEN {AxiCmdAdsFirstId} AND {AxiCmdAdsLastId}");

            foreach (var tableName in new[] { "axi_command_prompts", "axi_commands", "axi_command_config", "axp_tstructprops" })
            {
                var dropSql = isPostgreSql
                    ? $"DROP TABLE IF EXISTS {tableName} CASCADE"
                    : $"BEGIN EXECUTE IMMEDIATE 'DROP TABLE {tableName.ToUpperInvariant()} CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN IF SQLCODE != -942 THEN RAISE; END IF; END;";

                await ExecuteCleanupSqlAsync(db, dropSql);
            }
        }

        private static async Task ExecuteCleanupSqlAsync(dynamic db, string sql)
        {
            var result = await db.ExecuteNonQueryAsync(sql);
            var error = result?.error?.ToString();
            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new InvalidOperationException($"Axi CMD database cleanup failed: {error}");
            }
        }

        private string GetConfiguredAxpertWebRoot()
        {
            var configuredRoot = _configuration["AxpertWebRoot"];
            if (string.IsNullOrWhiteSpace(configuredRoot))
            {
                throw new InvalidOperationException("AxpertWebRoot must be configured in appsettings.json.");
            }

            var webRoot = Path.IsPathRooted(configuredRoot)
                ? configuredRoot
                : Path.Combine(_environment.ContentRootPath, configuredRoot);
            webRoot = Path.GetFullPath(webRoot);

            if (!Directory.Exists(Path.Combine(webRoot, "AxpertPlugins")) ||
                !Directory.Exists(Path.Combine(webRoot, "CustomPages")))
            {
                throw new InvalidOperationException("Configured AxpertWebRoot does not contain AxpertPlugins and CustomPages directories.");
            }

            return webRoot;
        }

        private static string GetPathWithinWebRoot(string webRoot, string relativePath)
        {
            var rootWithSeparator = Path.EndsInDirectorySeparator(webRoot) ? webRoot : webRoot + Path.DirectorySeparatorChar;
            var fullPath = Path.GetFullPath(Path.Combine(webRoot, relativePath));

            if (!fullPath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Plugin uninstall path is outside the Axpert web root.");
            }

            return fullPath;
        }

        private static void RestoreTargets(string pluginPath, string pagePath, string stagedPluginPath, string stagedPagePath, bool pageWasMoved)
        {
            if (Directory.Exists(stagedPluginPath) && !Directory.Exists(pluginPath))
            {
                Directory.Move(stagedPluginPath, pluginPath);
            }

            if (pageWasMoved && File.Exists(stagedPagePath) && !File.Exists(pagePath))
            {
                File.Move(stagedPagePath, pagePath);
            }
        }
    }
}
