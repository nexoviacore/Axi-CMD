using System;

namespace AxiApi.DTOs;

public class PromptNodeDTO
{
    public int CmdToken { get; init; }
    public int? WordPos { get; init; }
    public string? Prompt { get; init; }
    public string? PromptSource { get; init; }
    public string? PromptParams { get; init; }
    public string? PromptValues { get; init; }
    public string? ExtraParams { get; init; }
}
