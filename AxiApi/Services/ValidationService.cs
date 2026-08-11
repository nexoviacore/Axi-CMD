//using System;
//using System.Text.RegularExpressions;
//using AxiApi.DTOs;
//using AxiApi.Interfaces;
//using AxiApi.Lib.Utils;

//namespace AxiApi.Services;

//public class ValidationService : IValidationService
//{
//    private readonly IGrammarService _grammarService;

//    public ValidationService(IGrammarService grammarService)
//    {
//        _grammarService = grammarService;
//    }

//    private static List<string> ExtractParameterKeys(string command)
//    {
//        // Matches <transid>, <form caption>
//        var matches = Regex.Matches(command, "<([^>]+)>");

//        return matches
//            .Select(m => m.Groups[1].Value.Trim())
//            .Select(k => k.Replace(" ", ""))
//            .ToList();
//    }

//    public ValidationResult Validate(string input)
//    {
//        var parsed = CommandParser.Parse(input);
//        var tokens = parsed.Tokens;

//        if (tokens.Count < 2)
//            return ValidationResult.Fail("Invalid command format");

//        var grammar = _grammarService.Current;
//        var group = tokens[0];

//        if (!grammar.Commands.TryGetValue(group, out var commandPatterns))
//            return ValidationResult.Fail($"Unknown command group '{group}'");

//        foreach (var kv in commandPatterns)
//        {
//            var cmdNode = kv.Value;

//            if (!MatchLiteralTokens(cmdNode, tokens))
//                continue;

//            var parameters = ExtractParameters(tokens, cmdNode);
//            if (parameters == null)
//                return ValidationResult.Fail("Invalid parameters");

//            return ValidationResult.Ok(
//                cmdNode.CmdToken,
//                cmdNode.CommandGroup!,
//                cmdNode.Command!,
//                parameters
//            );
//        }

//        return ValidationResult.Fail($"No matching command found under '{group}'");
//    }

//    // Match only the literal part of the command (create, tstruct, edit, etc.)
//    private static bool MatchLiteralTokens(
//        CommandNodeDTO cmdNode,
//        IReadOnlyList<string> inputTokens
//    )
//    {
//        var commandLiteral = cmdNode
//            .Command!.Split('<')[0] // remove placeholders
//            .Trim();

//        var literalTokens = commandLiteral
//            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
//            .Select(t => t.ToLowerInvariant())
//            .ToList();

//        if (inputTokens.Count < literalTokens.Count)
//            return false;

//        for (int i = 0; i < literalTokens.Count; i++)
//        {
//            if (
//                !string.Equals(literalTokens[i], inputTokens[i], StringComparison.OrdinalIgnoreCase)
//            )
//                return false;
//        }

//        return true;
//    }

//    // Extract parameters (supports multi-word values)
//    private static IReadOnlyDictionary<string, string>? ExtractParameters(
//        IReadOnlyList<string> tokens,
//        CommandNodeDTO cmdNode
//    )
//    {
//        var prompts = cmdNode.Prompts.OrderBy(p => p.WordPos).ToList();
//        var keys = ExtractParameterKeys(cmdNode.Command!);

//        // if (keys.Count != prompts.Count)
//        //     return null;

//        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

//        for (int i = 0; i < prompts.Count; i++)
//        {
//            var prompt = prompts[i];
//            int startIndex = prompt.WordPos!.Value - 1;

//            if (startIndex >= tokens.Count)
//                return null;

//            bool isLast = i == prompts.Count - 1;

//            string value = isLast ? string.Join(" ", tokens.Skip(startIndex)) : tokens[startIndex];

//            // Enum validation
//            if (!string.IsNullOrWhiteSpace(prompt.PromptValues))
//            {
//                var allowed = prompt
//                    .PromptValues.Split(',', StringSplitOptions.RemoveEmptyEntries)
//                    .Select(v => v.Trim().ToLowerInvariant());

//                if (!allowed.Contains(value.ToLowerInvariant()))
//                    return null;
//            }

//            result[keys[i]] = value;
//        }

//        //return result;
//    }
//}
