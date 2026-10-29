using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigService
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname, string username, bool forceRefresh = false);
        Task<List<CommandConfigOptionDTO>> GetAllCommandOptionsAsync(string appname);
        /// <summary>One page of the Configure or SDK options for the AxiCMD Config page. Throws CommandOptionException for an unknown command.</summary>
        Task<CommandOptionPageDTO> GetCommandOptionsPageAsync(string appname, string? command, string? search, int? pageNo, int? pageSize);
        /// <summary>Sets the active flag and clears the command-config cache of the saving user. False when the option does not exist.</summary>
        Task<bool> SetCommandOptionActiveAsync(string appname, string username, string configId, bool active);
        /// <summary>Adds a Custom Option and clears the command-config cache of the saving user and the shared prompt (grammar) cache.</summary>
        Task<CommandConfigOptionDTO> CreateCommandOptionAsync(string appname, CreateCommandOptionRequestDTO request);
        Task<CommandConfigOptionDTO?> UpdateCommandOptionAsync(string appname, string configId, UpdateCommandOptionRequestDTO request);
    }
}
