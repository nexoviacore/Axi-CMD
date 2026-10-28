namespace AxiApi.DTOs
{
    /// <summary>Body of a request that adds a Custom Option to the Configure or SDK command.</summary>
    public class CreateCommandOptionRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        /// <summary>Configure Access of the caller; the write is refused unless this is true.</summary>
        public bool AppMgrAccess { get; set; }
        public string Command { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? PromptId { get; set; }
        public string? TargetUrl { get; set; }
        public string? ParamField { get; set; }
        public string? ExtraParams { get; set; }
        /// <summary>Data source that lists the records of the option; empty or Axi_Dummy means None.</summary>
        public string? RecordSource { get; set; }
    }
}
