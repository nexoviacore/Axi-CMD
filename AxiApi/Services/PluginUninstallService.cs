using AxiApi.DTOs;
using AxiApi.Interfaces;

namespace AxiApi.Services
{
    public class PluginUninstallService : IPluginUninstallService
    {
        private const string AxiCmdPluginId = "Axi_Beta_2";
        private const string AxiCmdPluginName = "Axi CMD";

        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PluginUninstallService> _logger;
        private readonly IConfiguration _configuration;

        public PluginUninstallService(IWebHostEnvironment environment, ILogger<PluginUninstallService> logger, IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
        }

        public IReadOnlyList<PluginDTO> GetInstalledPlugins()
        {
            var webRoot = GetConfiguredAxpertWebRoot();
            var pluginPath = GetPathWithinWebRoot(webRoot, GetRequiredConfiguration("AxiCmdUninstall:PluginRelativePath"));

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

        public Task<ApiResponseDTO> UninstallAxiCmdAsync()
        {
            var webRoot = GetConfiguredAxpertWebRoot();
            var pluginRelativePath = GetRequiredConfiguration("AxiCmdUninstall:PluginRelativePath");
            var pageRelativePath = GetRequiredConfiguration("AxiCmdUninstall:PageRelativePath");
            var pluginPath = GetPathWithinWebRoot(webRoot, pluginRelativePath);
            var pagePath = GetPathWithinWebRoot(webRoot, pageRelativePath);

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
            var stagedPluginPath = Path.Combine(operationPath, Path.GetFileName(pluginRelativePath));
            var stagedPagePath = Path.Combine(operationPath, Path.GetFileName(pageRelativePath));
            var pageWasMoved = false;

            try
            {
                Directory.CreateDirectory(operationPath);
                Directory.Move(pluginPath, stagedPluginPath);

                File.Move(pagePath, stagedPagePath);
                pageWasMoved = true;

                Directory.Delete(operationPath, true);

                _logger.LogInformation("Axi CMD plugin and AxiCMDMainPage2.html were uninstalled.");

                return Task.FromResult(new ApiResponseDTO
                {
                    Success = true,
                    Message = "Axi CMD was uninstalled.",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch
            {
                RestoreTargets(pluginPath, pagePath, stagedPluginPath, stagedPagePath, pageWasMoved);
                throw;
            }
        }

        private string GetConfiguredAxpertWebRoot()
        {
            var configuredRoot = GetRequiredConfiguration("AxpertWebRoot");

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

        private string GetRequiredConfiguration(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"{key} must be configured in appsettings.json.");
            }

            return value;
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
