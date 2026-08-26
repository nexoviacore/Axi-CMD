using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigService
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname, string username, bool forceRefresh = false);
        Task<List<CommandConfigDTO>> GetAllCommandConfigsAsync(string appname);
        Task<PagedResultDTO<CommandConfigDTO>> GetPagedCommandConfigsAsync(string appname, int pageIndex, int pageSize, string? search = null, string? command = null);
        Task<bool> SaveCommandConfigAsync(CommandConfigDTO config, string appname);
        Task<bool> DeleteCommandConfigAsync(string configId, string appname);
        Task<List<CommandPromptDTO>> GetCommandPromptsAsync(string appname);
        Task<bool> SaveCommandPromptAsync(SavePromptRequestDTO request, string appname);
    }
}
