using System;
using AxiApi.DTOs;

namespace AxiApi.Interfaces;

public interface IGrammarRepository
{
    Task<(List<CommandNodeDTO>, List<PromptNodeDTO>)> LoadGrammerAsync(string appname);
}
