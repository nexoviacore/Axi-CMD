using AxiApi.DTOs;
using AxiApi.Interfaces;
using System.Text.RegularExpressions;

namespace AxiApi.Services
{
    /// <summary>App-wide AxiCMD command settings (for example the GitHub account and repository of a product).</summary>
    public class CommandSettingsService : ICommandSettingsService
    {
        public const string AxiCmdCategory = "axicmd";
        public const string AccountNameKey = "accountname";
        public const string RepoNameKey = "reponame";
        private const int MaxValueLength = 100;

        private static readonly string[] AxiCmdTypes = { "product", "plugin", "changerequest" };
        private static readonly string[] AxiCmdKeys = { AccountNameKey, RepoNameKey };

        // GitHub account and repository naming
        private static readonly Regex GitHubName = new Regex("^[A-Za-z0-9._-]+$", RegexOptions.Compiled);

        private readonly ICommandSettingsRepository _repository;

        public CommandSettingsService(ICommandSettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandSettingsDTO> GetCommandSettingsAsync(string appname, string category, string? type)
        {
            var cat = Normalize(category);
            var typ = string.IsNullOrWhiteSpace(type) ? null : Normalize(type);
            var rows = await _repository.GetCommandSettings(appname, cat, typ);

            if (typ != null)
            {
                var group = BuildGroup(rows);
                return new CommandSettingsDTO
                {
                    Category = cat,
                    Type = typ,
                    Properties = group.Properties,
                    ModifiedOn = group.ModifiedOn,
                    Username = group.Username
                };
            }

            var types = rows
                .GroupBy(r => r.PropertyType, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => BuildGroup(g), StringComparer.OrdinalIgnoreCase);
            return new CommandSettingsDTO { Category = cat, Types = types };
        }

        public async Task<CommandSettingsSaveResultDTO> SaveCommandSettingsAsync(string appname, CommandSettingsRequestDTO request)
        {
            var (username, category, type, properties) = Validate(request);
            var previous = await _repository.SaveCommandSettings(appname, username, category, type, properties);
            return new CommandSettingsSaveResultDTO
            {
                Category = category,
                Type = type,
                Properties = properties,
                Previous = previous
            };
        }

        /// <summary>Checks a save request and returns it normalised (names lower-cased and trimmed, values trimmed). Throws ArgumentException when invalid.</summary>
        public static (string Username, string Category, string Type, Dictionary<string, string> Properties) Validate(CommandSettingsRequestDTO? request)
        {
            if (request == null)
                throw new ArgumentException("Request body is required.");
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username is required in payload.");
            if (string.IsNullOrWhiteSpace(request.PropertyCategory))
                throw new ArgumentException("propertyCategory is required.");
            if (string.IsNullOrWhiteSpace(request.PropertyType))
                throw new ArgumentException("propertyType is required.");
            if (request.Properties == null || request.Properties.Count == 0)
                throw new ArgumentException("properties must contain at least one entry.");

            var category = Normalize(request.PropertyCategory);
            var type = Normalize(request.PropertyType);

            var properties = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var pair in request.Properties)
            {
                var name = Normalize(pair.Key);
                if (name.Length == 0)
                    throw new ArgumentException("Property names cannot be empty.");
                if (!properties.TryAdd(name, (pair.Value ?? string.Empty).Trim()))
                    throw new ArgumentException($"Property '{name}' is given more than once.");
            }

            if (category == AxiCmdCategory)
            {
                if (!AxiCmdTypes.Contains(type))
                    throw new ArgumentException($"Unknown propertyType '{type}' for category '{AxiCmdCategory}'. Allowed: {string.Join(", ", AxiCmdTypes)}.");

                var missing = AxiCmdKeys.Where(k => !properties.ContainsKey(k)).ToList();
                var extra = properties.Keys.Where(k => !AxiCmdKeys.Contains(k)).ToList();
                if (missing.Count > 0 || extra.Count > 0)
                    throw new ArgumentException($"Category '{AxiCmdCategory}' needs exactly the properties {string.Join(" and ", AxiCmdKeys)}."
                        + (missing.Count > 0 ? $" Missing: {string.Join(", ", missing)}." : string.Empty)
                        + (extra.Count > 0 ? $" Not allowed: {string.Join(", ", extra)}." : string.Empty));

                foreach (var pair in properties)
                {
                    if (pair.Value.Length == 0 || pair.Value.Length > MaxValueLength)
                        throw new ArgumentException($"{pair.Key} must be 1 to {MaxValueLength} characters.");
                    if (!GitHubName.IsMatch(pair.Value))
                        throw new ArgumentException($"{pair.Key} can only contain letters, digits, '.', '_' and '-'.");
                }
            }

            return (request.Username.Trim(), category, type, properties);
        }

        private static string Normalize(string? value) => (value ?? string.Empty).Trim().ToLowerInvariant();

        private static CommandSettingsGroupDTO BuildGroup(IEnumerable<CommandSettingRowDTO> rows)
        {
            var group = new CommandSettingsGroupDTO();
            foreach (var row in rows)
            {
                group.Properties[row.PropertyName] = row.PropertyValue;
                // The group shows who changed it last
                if (row.ModifiedOn.HasValue && (!group.ModifiedOn.HasValue || row.ModifiedOn > group.ModifiedOn))
                {
                    group.ModifiedOn = row.ModifiedOn;
                    group.Username = row.Username;
                }
            }
            return group;
        }
    }
}
