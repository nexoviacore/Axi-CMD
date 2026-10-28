namespace AxiApi.DTOs
{
    /// <summary>Body of a request that edits or renames a Custom Option. The command and the option are named by the route and cannot change.</summary>
    public class UpdateCommandOptionRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        /// <summary>Configure Access of the caller; the write is refused unless this is true.</summary>
        public bool AppMgrAccess { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? PromptId { get; set; }
        public string? TargetUrl { get; set; }
        public string? ParamField { get; set; }
        public string? ExtraParams { get; set; }
        public string? RecordSource { get; set; }
    }
}
