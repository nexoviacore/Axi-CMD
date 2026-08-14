using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigService
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname, string username, bool forceRefresh = false);
    }
}
