using System;

namespace AxiApi.DTOs;

public class CommandNodeDTO
{
    //public bool hasChildToken { get; set; }
    public int CmdToken { get; init; }
    public string Command { get; init; } = null!;
    public string CommandGroup { get; init; } = null!;

    // public string? Action { get; init; }
    public List<PromptNodeDTO> Prompts { get; } = new();
}
