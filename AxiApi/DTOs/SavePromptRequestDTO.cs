namespace AxiApi.DTOs
{
    public class SavePromptRequestDTO
    {
        public int CmdToken { get; set; }
        public int WordPos { get; set; }
        public string PromptValues { get; set; } = string.Empty;
        public string PromptSource { get; set; } = string.Empty;
    }
}
