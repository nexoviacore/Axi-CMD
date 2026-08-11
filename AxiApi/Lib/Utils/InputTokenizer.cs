using System;

namespace AxiApi.Lib.Utils;

public class InputTokenizer
{
    public static (List<string> Tokens, bool EndsWithSpace) Tokenize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (new List<string>(), false);

        bool endsWithSpace = input.EndsWith(" ");

        var tokens = input
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.ToLowerInvariant())
            .ToList();

        return (tokens, endsWithSpace);
    }
}
