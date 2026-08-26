namespace AxiApi.DTOs
{
    public class CommandPromptDTO
    {
        public string? Id { get; set; }
        public int CmdToken { get; set; }
        public string? Command { get; set; }
        public string? CommandGroup { get; set; }
        public int? WordPos { get; set; }
        public string? Prompt { get; set; }
        public string? PromptSource { get; set; }
        public string? PromptParams { get; set; }
        public string? PromptValues { get; set; }
        public string? ExtraParams { get; set; }
        public string? RequestUrl { get; set; }
    }
}
