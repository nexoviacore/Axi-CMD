using System;
using AxiApi.DTOs;
using AxiApi.Enums;

namespace AxiApi.Interfaces;

public interface IGrammarService
{
    GrammarDTO Current { get; set; }
    void Set(GrammarDTO grammer);
    Task<object> Get(GrammarView context, bool forceRefresh, string armSessionId);
    Task RefreshAsync(string appname);
}
