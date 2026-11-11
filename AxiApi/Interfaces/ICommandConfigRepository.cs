using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigRepository
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname);
        Task<List<CommandConfigOptionDTO>> GetAllCommandOptionsAsync(string appname);
        /// <summary>One page of a command's options whose name or target contains search, plus the totals. command is "Configure" or "SDK".</summary>
        Task<CommandOptionPageDTO> GetCommandOptionsPageAsync(string appname, string command, string search, string show, string focus, int? pageNo, int pageSize);
        /// <summary>Sets the active flag of one Configure / SDK option. False when no such option exists.</summary>
        Task<bool> SetCommandOptionActiveAsync(string appname, string configId, bool active);
        /// <summary>Validates and adds a Custom Option: the config row and both prompt lists in one transaction. Throws CommandOptionException when a rule is broken.</summary>
        Task<CommandConfigOptionDTO> CreateCommandOptionAsync(string appname, CreateCommandOptionRequestDTO request);
        Task<CommandConfigOptionDTO?> UpdateCommandOptionAsync(string appname, string configId, UpdateCommandOptionRequestDTO request);
    }
}
