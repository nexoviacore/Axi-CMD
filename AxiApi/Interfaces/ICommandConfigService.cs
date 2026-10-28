using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigService
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname, string username, bool forceRefresh = false);
        Task<List<CommandConfigOptionDTO>> GetAllCommandOptionsAsync(string appname);
        /// <summary>Sets the active flag and clears the command-config cache of the saving user. False when the option does not exist.</summary>
        Task<bool> SetCommandOptionActiveAsync(string appname, string username, string configId, bool active);
    }
}
