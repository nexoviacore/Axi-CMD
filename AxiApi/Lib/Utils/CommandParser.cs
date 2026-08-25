using System;

using AxiApi.DTOs;

namespace AxiApi.Lib.Utils;

public static class CommandParser
{
    public static ParsedCommand Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new ParsedCommand();

        var endsWithSpace = input.EndsWith(" ");

        var tokens = input
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.ToLowerInvariant())
            .ToList();

        return new ParsedCommand { Tokens = tokens, EndsWithSpace = endsWithSpace };
    }
}
