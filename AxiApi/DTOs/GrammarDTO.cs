using System;
using AxiApi.Lib.Utils;

namespace AxiApi.DTOs;

public class GrammarDTO
{
    public Dictionary<string, CommandNodeDTO> Commands { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);


    //public Dictionary<string, Dictionary<string, CommandNodeDTO>> Commands { get; set; } =
    //    new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<int, List<PromptNodeDTO>> PromptsByCmdToken { get; set; } = new();

    //public Trie AutocompleteTrie { get; set; } = new();

    public DateTime LoadedAtUtc { get; set; }
}
