namespace AxiApi.DTOs
{
    public class CommandConfigDTO
    {
        public string ConfigId { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string PromptOptions { get; set; } = string.Empty;
        public string PromptId { get; set; } = string.Empty;
        public string PromptOptionType { get; set; } = string.Empty;
        public string? ParamField { get; set; }
        public string? TargetUrl { get; set; }
        public string? ExtraParams { get; set; }
        public string Active { get; set; } = "T";
    }
}
