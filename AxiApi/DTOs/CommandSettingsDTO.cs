using System.Text.Json.Serialization;

namespace AxiApi.DTOs
{
    /// <summary>One stored command settings row (axi_command_settings).</summary>
    public class CommandSettingRowDTO
    {
        public string PropertyType { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyValue { get; set; } = string.Empty;
        public string? Username { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    /// <summary>The properties of one type, with when and by whom they were last changed.</summary>
    public class CommandSettingsGroupDTO
    {
        public Dictionary<string, string> Properties { get; set; } = new();
        public DateTime? ModifiedOn { get; set; }
        public string? Username { get; set; }
    }

    /// <summary>
    /// Answer of GET CommandSettings. With a type: category, type, properties, modifiedOn, username.
    /// Without a type: category and types (type name to group). Fields that do not apply are left out of the JSON.
    /// </summary>
    public class CommandSettingsDTO
    {
        public string Category { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string>? Properties { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ModifiedOn { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Username { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, CommandSettingsGroupDTO>? Types { get; set; }
    }

    /// <summary>Answer of POST CommandSettings: the stored values and what each property held before (null when it is new).</summary>
    public class CommandSettingsSaveResultDTO
    {
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Dictionary<string, string> Properties { get; set; } = new();
        public Dictionary<string, string?> Previous { get; set; } = new();
    }
}
