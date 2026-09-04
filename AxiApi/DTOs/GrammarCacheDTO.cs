using System;

namespace AxiApi.DTOs;

public class GrammarCacheDTO
{
    public List<CommandNodeDTO> Commands { get; init; } = [];
    public List<PromptNodeDTO> Prompts { get; init; } = [];
}
