using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandConfigRepository
    {
        Task<List<CommandConfigDTO>> GetCommandConfigsAsync(string appname);
        Task<List<CommandConfigOptionDTO>> GetAllCommandOptionsAsync(string appname);
    }
}
