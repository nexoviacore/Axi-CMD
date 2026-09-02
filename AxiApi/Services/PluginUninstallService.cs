using AxiApi.DTOs;
using AxiApi.Interfaces;

namespace AxiApi.Services
{
    public class PluginUninstallService : IPluginUninstallService
    {
        private const string AxiCmdPluginId = "Axi_Beta_2";
        private const string AxiCmdPluginName = "Axi CMD";
        private const string AxiCmdPageRelativePath = "CustomPages/AxiCMDMainPage2.html";
        private const string AxiCmdPluginRelativePath = "AxpertPlugins/Axi_Beta_2";

        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PluginUninstallService> _logger;

        public PluginUninstallService(IWebHostEnvironment environment, ILogger<PluginUninstallService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public IReadOnlyList<PluginDTO> GetInstalledPlugins()
        {
            var webRoot = GetAxpertWebRoot();
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

        public Task<ApiResponseDTO> UninstallAxiCmdAsync()
        {
            var webRoot = GetAxpertWebRoot();
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

        private string GetAxpertWebRoot()
        {
            var candidateRoots = new[]
            {
                _environment.ContentRootPath,
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };

            foreach (var candidateRoot in candidateRoots)
            {
                var directory = new DirectoryInfo(Path.GetFullPath(candidateRoot));
                while (directory != null)
                {
                    if (Directory.Exists(Path.Combine(directory.FullName, "AxpertPlugins")) &&
                        Directory.Exists(Path.Combine(directory.FullName, "CustomPages")))
                    {
                        return directory.FullName;
                    }

                    directory = directory.Parent;
                }
            }

            throw new InvalidOperationException("Unable to locate the Axpert web root for plugin uninstall.");
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
