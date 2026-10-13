using AxiApi.Interfaces;

namespace AxiApi.Services
{
    /// <summary>Per-user AxiCMD settings (for example how many records the palette loads at a time).</summary>
    public class UserSettingsService : IUserSettingsService
    {
        // Keep in step with MIN_PAGE_SIZE / MAX_PAGE_SIZE / DEFAULT_PAGE_SIZE in axicmdmain.js
        public const string PageSizeKey = "pagesize";
        private const int MinPageSize = 100;
        private const int MaxPageSize = 5000;
        private const int DefaultPageSize = 100;

        private readonly IUserSettingsRepository _repository;

        public UserSettingsService(IUserSettingsRepository repository)
        {
            _repository = repository;
        }

        public Task<Dictionary<string, string>> GetUserSettingsAsync(string username, string appname)
        {
            return _repository.GetUserSettings(username, appname);
        }

        public async Task<string> SaveUserSettingAsync(string username, string settingKey, string settingValue, string appname)
        {
            var key = (settingKey ?? string.Empty).Trim().ToLowerInvariant();
            string value;

            switch (key)
            {
                case PageSizeKey:
                    // Out of range or non numeric values are corrected here, never rejected, so a stale client cannot break the palette
                    value = int.TryParse(settingValue, out var size)
                        ? Math.Clamp(size, MinPageSize, MaxPageSize).ToString()
                        : DefaultPageSize.ToString();
                    break;
                default:
                    throw new ArgumentException($"Unknown setting '{settingKey}'.");
            }

            await _repository.SaveUserSetting(username, key, value, appname);
            return value;
        }
    }
}
