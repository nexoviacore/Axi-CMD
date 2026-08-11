using System;

namespace AxiApi.DTOs;

public sealed class ParsedCommand
{
    public IReadOnlyList<string> Tokens { get; init; } = [];

    public bool EndsWithSpace { get; set; }
}
