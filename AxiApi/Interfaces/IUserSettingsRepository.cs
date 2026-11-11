namespace AxiApi.Interfaces
{
    public interface IUserSettingsRepository
    {
        /// <summary>All saved settings of one user as key/value pairs.</summary>
        Task<Dictionary<string, string>> GetUserSettings(string username, string appname);

        /// <summary>Saves (replaces) one setting of one user.</summary>
        Task SaveUserSetting(string username, string settingKey, string settingValue, string appname);
    }
}
