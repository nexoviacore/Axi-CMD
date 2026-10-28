using System;
using System.Collections.Generic;

namespace AxiApi.Lib
{
    /// <summary>
    /// Pairs the comma separated option names of a command prompt (wordpos 2) with the comma separated record source
    /// names of the next prompt (wordpos 3) by position, the same way the palette does.
    /// </summary>
    public static class CommandOptionSources
    {
        public static Dictionary<string, string> Pair(string? promptValues, string? promptSources)
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var names = (promptValues ?? string.Empty).Split(',');
            var sources = (promptSources ?? string.Empty).Split(',');
            for (int i = 0; i < names.Length && i < sources.Length; i++)
            {
                var name = names[i].Trim();
                if (name.Length > 0 && !map.ContainsKey(name))
                    map[name] = sources[i].Trim();
            }
            return map;
        }
    }
}
