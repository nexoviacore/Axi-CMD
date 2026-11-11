namespace AxiApi.Interfaces
{
    public interface IUserSettingsService
    {
        Task<Dictionary<string, string>> GetUserSettingsAsync(string username, string appname);

        /// <summary>Validates and saves one setting; returns the value that was stored.</summary>
        Task<string> SaveUserSettingAsync(string username, string settingKey, string settingValue, string appname);
    }
}
