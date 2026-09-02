using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface IPluginUninstallService
    {
        IReadOnlyList<PluginDTO> GetInstalledPlugins();
        Task<ApiResponseDTO> UninstallAxiCmdAsync(string appname);
    }
}
