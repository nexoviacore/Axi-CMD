using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigRepository
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname);
        Task<List<CommandConfigDTO>> GetAllCommandConfigsAsync(string appname);
        Task<bool> SaveCommandConfigAsync(CommandConfigDTO config, string appname);
        Task<bool> DeleteCommandConfigAsync(string configId, string appname);
        Task<List<CommandPromptDTO>> GetCommandPromptsAsync(string appname);
        Task<bool> SaveCommandPromptAsync(SavePromptRequestDTO request, string appname);
    }
}
