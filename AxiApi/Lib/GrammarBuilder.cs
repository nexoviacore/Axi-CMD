using System;
using AxiApi.DTOs;

namespace AxiApi.Lib;

public static class GrammarBuilder
{
   public static GrammarDTO Build(List<CommandNodeDTO> commands, List<PromptNodeDTO> prompts)
    {
        var grammar = new GrammarDTO { LoadedAtUtc = DateTime.UtcNow };

        // Group prompts by cmdToken
        grammar.PromptsByCmdToken = prompts
            .GroupBy(p => p.CmdToken)
            .ToDictionary(g => g.Key, g => g.OrderBy(p => p.WordPos).ToList());

        foreach (var cmd in commands)
        {
            // Attach prompts to command
            if (grammar.PromptsByCmdToken.TryGetValue(cmd.CmdToken, out var plist))
                cmd.Prompts.AddRange(plist);


            var key = cmd.CommandGroup.Trim();

            grammar.Commands[key] = cmd; 

            //var group = cmd.CommandGroup.Trim().ToLowerInvariant();
            //var verb = cmd.Command.Trim().ToLowerInvariant();

           
           
  
            //grammar.AutocompleteTrie.Insert(key);
        }

        return grammar;
    }
}
