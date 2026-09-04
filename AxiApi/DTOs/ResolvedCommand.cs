using System;

namespace AxiApi.DTOs;

public class ResolvedCommand
{
    public int CmdToken { get; set; }
    public string CommandGroup { get; set; } = string.Empty;
    public string Command { get; set; } = "";
    public IReadOnlyDictionary<string, string> Paramaters { get; set; } = null!;
}
