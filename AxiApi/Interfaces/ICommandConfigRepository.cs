using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigRepository
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname);
        Task<List<CommandConfigOptionDTO>> GetAllCommandOptionsAsync(string appname);
        /// <summary>Sets the active flag of one Configure / SDK option. False when no such option exists.</summary>
        Task<bool> SetCommandOptionActiveAsync(string appname, string configId, bool active);
        /// <summary>Validates and adds a Custom Option: the config row and both prompt lists in one transaction. Throws CommandOptionException when a rule is broken.</summary>
        Task<CommandConfigOptionDTO> CreateCommandOptionAsync(string appname, CreateCommandOptionRequestDTO request);
    }
}
