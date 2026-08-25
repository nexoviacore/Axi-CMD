// using System;
// using AxiApi.Interfaces;
// using AxiApi.Lib.Utils;

// namespace AxiApi.Services;

// public class ContextAutocompleteService : IContextAutocompleteService
// {
//     private readonly IGrammarService _grammarService;

//     public ContextAutocompleteService(IGrammarService grammarService)
//     {
//         _grammarService = grammarService;
//     }

//     public IReadOnlyList<string> Suggest(string input)
//     {
//         var grammar = _grammarService.Current;
//         var (tokens, endsWithSpace) = InputTokenizer.Tokenize(input);

//         // --------------------------------
//         // 0 tokens → command groups
//         // --------------------------------
//         if (tokens.Count == 0)
//             return grammar.Commands.Keys.ToList();

//         // --------------------------------
//         // Typing command group
//         // --------------------------------
//         if (tokens.Count == 1 && !endsWithSpace)
//             return grammar
//                 .Commands.Keys.Where(k =>
//                     k.StartsWith(tokens[0], StringComparison.OrdinalIgnoreCase)
//                 )
//                 .ToList();

//         var group = tokens[0];

//         if (!grammar.Commands.TryGetValue(group, out var verbs))
//             return Array.Empty<string>();

//         // --------------------------------
//         // After group + space → verbs
//         // --------------------------------
//         if (tokens.Count == 1 && endsWithSpace)
//             return verbs.Keys.ToList();

//         // --------------------------------
//         // Typing verb
//         // --------------------------------
//         if (tokens.Count == 2 && !endsWithSpace)
//             return verbs
//                 .Keys.Where(v => v.StartsWith(tokens[1], StringComparison.OrdinalIgnoreCase))
//                 .ToList();

//         var verb = tokens[1];

//         if (!verbs.TryGetValue(verb, out var command))
//             return Array.Empty<string>();

//         // --------------------------------
//         // Prompt resolution (wordpos based)
//         // --------------------------------
//         int wordPos = tokens.Count + 1;
//         var prompt = command.Prompts.FirstOrDefault(p => p.WordPos == wordPos);

//         if (prompt == null)
//             return Array.Empty<string>();

//         // --------------------------------
//         // Static enum values
//         // --------------------------------
//         if (!string.IsNullOrWhiteSpace(prompt.PromptValues))
//             return prompt
//                 .PromptValues.Split(',')
//                 .Select(v => v.Trim())
//                 .Where(v =>
//                     endsWithSpace || v.StartsWith(tokens.Last(), StringComparison.OrdinalIgnoreCase)
//                 )
//                 .ToList();

//         // --------------------------------
//         // Dynamic sources (placeholder)
//         // --------------------------------
//         if (!string.IsNullOrWhiteSpace(prompt.PromptSource))
//             return new List<string> { $"<{prompt.PromptSource}>" };

//         return Array.Empty<string>();
//     }
// }
