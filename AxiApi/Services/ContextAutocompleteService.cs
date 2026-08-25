//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AxiApi.Interfaces;
//using AxiApi.Lib.Utils;

//namespace AxiApi.Services
//{
//    public class ContextAutocompleteService : IContextAutocompleteService
//    {
//        private readonly IGrammarService _grammarService;

//        public ContextAutocompleteService(IGrammarService grammarService)
//        {
//            _grammarService = grammarService;
//        }

//        public IReadOnlyList<string> Suggest(string input)
//        {
//            var grammar = _grammarService.Current;
//            // Tokenize: "create tstruct " -> tokens:["create", "tstruct"], endsWithSpace: true
//            var (tokens, endsWithSpace) = InputTokenizer.Tokenize(input);

//            // --------------------------------
//            // 1. Empty Input -> Return all Groups (create, edit, view...)
//            // --------------------------------
//            if (tokens.Count == 0)
//                return grammar.Commands.Keys.ToList();

//            // --------------------------------
//            // 2. Typing the Group Name (e.g., "cre")
//            // --------------------------------
//            if (tokens.Count == 1 && !endsWithSpace)
//            {
//                return grammar
//                    .Commands.Keys.Where(k =>
//                        k.StartsWith(tokens[0], StringComparison.OrdinalIgnoreCase)
//                    )
//                    .ToList();
//            }

//            // Valid Group Found? (e.g., "create")
//            var groupToken = tokens[0];
//            if (!grammar.Commands.TryGetValue(groupToken, out var commandDefinitions))
//                return Array.Empty<string>();

//            // --------------------------------
//            // 3. Logic: Match Input against Full Command Strings
//            // --------------------------------

//            // Reconstruct what the user typed so far to match against keys
//            // If tokens are ["create", "tstruct"], currentInput = "create tstruct"
//            string currentInputPrefix = string.Join(" ", tokens);

//            // Filter: Find all command keys in this group that start with what user typed
//            var matchingKeys = commandDefinitions
//                .Keys.Where(k =>
//                    k.StartsWith(currentInputPrefix, StringComparison.OrdinalIgnoreCase)
//                )
//                .ToList();

//            // If we have potential matches, we need to suggest the NEXT word
//            if (matchingKeys.Count > 0)
//            {
//                var suggestions = new HashSet<string>();

//                // Determine which "word index" we are trying to autocomplete
//                // If "create " (space) -> we want index 1 (2nd word)
//                // If "create tstr" -> we want index 1 (2nd word) completing it
//                int targetIndex = endsWithSpace ? tokens.Count : tokens.Count - 1;

//                foreach (var key in matchingKeys)
//                {
//                    var keyParts = key.Split(' ');

//                    // distinct check to ensure we don't crash if key is shorter than input
//                    if (keyParts.Length > targetIndex)
//                    {
//                        var nextWord = keyParts[targetIndex];

//                        // Only add if it starts with the partial word user typed (if not space)
//                        if (
//                            endsWithSpace
//                            || nextWord.StartsWith(
//                                tokens.Last(),
//                                StringComparison.OrdinalIgnoreCase
//                            )
//                        )
//                        {
//                            // If the next word is a placeholder like <transid>, don't suggest it here,
//                            // let the prompt logic handle it below.
//                            if (!nextWord.StartsWith("<"))
//                            {
//                                suggestions.Add(nextWord);
//                            }
//                        }
//                    }
//                }

//                // If we found next-word keywords (like "tstruct", "user"), return them
//                if (suggestions.Count > 0)
//                    return suggestions.ToList();
//            }

//            // --------------------------------
//            // 4. Prompt / Parameter Logic
//            // --------------------------------
//            // If we are here, it means the user has typed a valid command sequence
//            // and we might need to show values for a placeholder (e.g., <transid>)

//            // Find the Exact Command Definition that matches the structure
//            // We assume the user is typing sequentially.
//            // We find the command definition where the static parts match.

//            var matchedCommandPair = commandDefinitions.FirstOrDefault(kvp =>
//            {
//                var keyParts = kvp.Key.Split(' ');
//                if (keyParts.Length < tokens.Count)
//                    return false;

//                // Check if all *static* words typed so far match the key
//                for (int i = 0; i < tokens.Count; i++)
//                {
//                    // If the key has a placeholder <...>, we skip comparison (it's a user value)
//                    if (keyParts[i].StartsWith("<"))
//                        continue;

//                    // If static word doesn't match, this isn't the command
//                    if (!keyParts[i].Equals(tokens[i], StringComparison.OrdinalIgnoreCase))
//                        return false;
//                }
//                return true;
//            });

//            if (matchedCommandPair.Value == null)
//                return Array.Empty<string>();

//            var commandConfig = matchedCommandPair.Value;

//            // Calculate current word position for prompt lookup (1-based index)
//            // If tokens ["create", "tstruct"] (count 2) and space -> we are typing word 3
//            int currentWordPos = endsWithSpace ? tokens.Count + 1 : tokens.Count;

//            var prompt = commandConfig.Prompts?.FirstOrDefault(p => p.WordPos == currentWordPos);

//            if (prompt == null)
//                return Array.Empty<string>();

//            // A. Static Values (e.g., "excel,pdf")
//            if (!string.IsNullOrWhiteSpace(prompt.PromptValues))
//            {
//                var validValues = prompt
//                    .PromptValues.Split(',')
//                    .Select(v => v.Trim())
//                    .Where(v =>
//                        endsWithSpace
//                        || v.StartsWith(tokens.Last(), StringComparison.OrdinalIgnoreCase)
//                    )
//                    .ToList();
//                return validValues;
//            }

//            // B. Dynamic Source (e.g., "UserList")
//            if (!string.IsNullOrWhiteSpace(prompt.PromptSource))
//            {
//                // In a real app, you would fetch from DB here.
//                // returning the source name so you see it works.
//                return new List<string> { $"Fetch: {prompt.PromptSource}..." };
//            }

//            if (!string.IsNullOrEmpty(commandConfig.Command))
//            {
//                return new List<string> { commandConfig.Command };
//            }

//            return Array.Empty<string>();
//        }
//    }
//}
