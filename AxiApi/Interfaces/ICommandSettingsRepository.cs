using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandSettingsRepository
    {
        /// <summary>Stored rows of one category, of one type when type is given (null = every type).</summary>
        Task<List<CommandSettingRowDTO>> GetCommandSettings(string appname, string category, string? type);

        /// <summary>
        /// Saves the properties in one transaction (insert new rows, update existing ones) and returns the value each
        /// property held before (null when it did not exist). All arguments are already validated and normalised.
        /// </summary>
        Task<Dictionary<string, string?>> SaveCommandSettings(string appname, string username, string category, string type, Dictionary<string, string> properties);
    }
}
