using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface ICommandSettingsService
    {
        /// <summary>Settings of a category (and type); nothing stored gives empty properties.</summary>
        Task<CommandSettingsDTO> GetCommandSettingsAsync(string appname, string category, string? type);

        /// <summary>Validates and saves the properties; throws ArgumentException for anything invalid.</summary>
        Task<CommandSettingsSaveResultDTO> SaveCommandSettingsAsync(string appname, CommandSettingsRequestDTO request);
    }
}
