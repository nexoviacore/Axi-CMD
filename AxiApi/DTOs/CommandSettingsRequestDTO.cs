namespace AxiApi.DTOs
{
    /// <summary>Body of a request that saves a group of AxiCMD command settings (one category and type, many properties).</summary>
    public class CommandSettingsRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        public string PropertyCategory { get; set; } = string.Empty;
        public string PropertyType { get; set; } = string.Empty;
        public Dictionary<string, string> Properties { get; set; } = new();
    }
}
