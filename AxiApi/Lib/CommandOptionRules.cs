using AxiApi.DTOs;
using AxiApi.Exceptions;
using System.Text.RegularExpressions;

namespace AxiApi.Lib
{
    /// <summary>
    /// Rules for Custom Options of the Configure and SDK commands. Mirrors AxpertPlugins/AxiCMD/HTMLPages/js/axiconfiglogic.js:
    /// change both together (tests/axi-config-logic-ticket-285.test.js covers the script, the Postman collection covers this).
    /// </summary>
    public static class CommandOptionRules
    {
        public const int MaxNameLength = 200;
        public const int MaxListLength = 4000;
        public const int MaxConfigIdLength = 50;
        public const string NoSource = "Axi_Dummy";

        public static readonly string[] Commands = { "Configure", "SDK" };
        public static readonly string[] CustomTypes = { "tstruct", "iview", "ivtoivload", "url" };

        private static readonly Dictionary<string, string[]> FieldsByType = new()
        {
            ["tstruct"] = new[] { "promptId", "paramField" },
            ["iview"] = new[] { "promptId", "targetUrl" },
            ["ivtoivload"] = new[] { "promptId", "paramField", "extraParams" },
            ["url"] = new[] { "targetUrl", "paramField" }
        };
        private static readonly Dictionary<string, string[]> RequiredByType = new()
        {
            ["tstruct"] = new[] { "promptId" },
            ["iview"] = new[] { "promptId" },
            ["ivtoivload"] = new[] { "promptId" },
            ["url"] = new[] { "targetUrl" }
        };
        private static readonly Dictionary<string, int> FieldMax = new()
        {
            ["promptId"] = 50, ["targetUrl"] = 500, ["paramField"] = 100, ["extraParams"] = 500
        };

        /// <summary>Canonical command spelling (Configure / SDK), or null when the command cannot hold Custom Options.</summary>
        public static string? NormalizeCommand(string? command) =>
            Commands.FirstOrDefault(c => string.Equals(c, (command ?? "").Trim(), StringComparison.OrdinalIgnoreCase));

        /// <summary>cmdtoken of the command in axi_command_prompts: Configure 4, SDK 7.</summary>
        public static int CmdToken(string command) => command == "SDK" ? 7 : 4;

        public static string[] FieldsFor(string type) =>
            FieldsByType.TryGetValue(type, out var f) ? f : Array.Empty<string>();

        public static string ValidateName(string? name, IEnumerable<string> existingNames)
        {
            var n = (name ?? "").Trim();
            if (n.Length == 0) throw new CommandOptionException("Enter a name for the option.");
            if (n.Contains(',')) throw new CommandOptionException("The name cannot contain a comma.");
            if (n.Length > MaxNameLength) throw new CommandOptionException($"The name can have at most {MaxNameLength} characters (now {n.Length}).");
            if (existingNames.Any(e => string.Equals((e ?? "").Trim(), n, StringComparison.OrdinalIgnoreCase)))
                throw new CommandOptionException($"An option named \"{n}\" already exists in this command (names are not case sensitive).", true);
            return n;
        }

        /// <summary>usr_&lt;command&gt;_&lt;slug&gt;, at most 50 characters.</summary>
        public static string MakeConfigId(string command, string name)
        {
            var prefix = "usr_" + command.Trim().ToLowerInvariant() + "_";
            var lower = name.Trim().ToLowerInvariant();
            var slug = Regex.Replace(lower, "[^a-z0-9]+", "_").Trim('_');
            if (slug.Length == 0) slug = "x" + HashName(lower);
            if (slug.Length > MaxConfigIdLength - prefix.Length) slug = slug.Substring(0, MaxConfigIdLength - prefix.Length);
            return (prefix + slug).TrimEnd('_');
        }

        /// <summary>FNV-1a over UTF-16 code units, the same as the page script.</summary>
        private static string HashName(string text)
        {
            uint h = 0x811c9dc5;
            foreach (char c in text) { h ^= c; h *= 0x01000193; }
            return h.ToString("x8");
        }

        private static string Clean(string? v) => (v ?? "").Trim();

        /// <summary>Validates the request and returns it normalised: only the fields of the type are kept.</summary>
        public static CreateCommandOptionRequestDTO Validate(CreateCommandOptionRequestDTO req, IEnumerable<string> existingNames)
        {
            var command = NormalizeCommand(req.Command) ?? throw new CommandOptionException("Choose Configure or SDK.");
            var type = Clean(req.Type).ToLowerInvariant();
            if (!CustomTypes.Contains(type)) throw new CommandOptionException("Choose a type: " + string.Join(", ", CustomTypes) + ".");
            var name = ValidateName(req.Name, existingNames);

            var input = new Dictionary<string, string>
            {
                ["promptId"] = Clean(req.PromptId), ["targetUrl"] = Clean(req.TargetUrl),
                ["paramField"] = Clean(req.ParamField), ["extraParams"] = Clean(req.ExtraParams)
            };
            var value = new Dictionary<string, string> { ["promptId"] = "", ["targetUrl"] = "", ["paramField"] = "", ["extraParams"] = "" };
            foreach (var f in FieldsFor(type))
            {
                var v = input[f];
                value[f] = v;
                if (v.Length == 0 && RequiredByType[type].Contains(f)) throw new CommandOptionException($"{f} is required for a {type} option.");
                if (v.Length == 0) continue;
                if (v.Length > FieldMax[f]) throw new CommandOptionException($"{f} can have at most {FieldMax[f]} characters.");
                if (f == "targetUrl" && !Regex.IsMatch(v, @"^(https?://|/|\.\./)", RegexOptions.IgnoreCase))
                    throw new CommandOptionException("targetUrl must start with http://, https://, / or ../");
                if (f == "paramField" && !Regex.IsMatch(v, "^[A-Za-z0-9_]+$")) throw new CommandOptionException("paramField can only have letters, digits and underscore.");
                if (f == "promptId" && !Regex.IsMatch(v, @"^[A-Za-z0-9_./-]+$")) throw new CommandOptionException("promptId can only have letters, digits, underscore, dot, dash and slash.");
            }

            var source = Clean(req.RecordSource);
            if (source.Length == 0) source = NoSource;
            else if (!Regex.IsMatch(source, "^[A-Za-z0-9_]+$")) throw new CommandOptionException("The record source name is not valid.");

            return new CreateCommandOptionRequestDTO
            {
                Username = req.Username, AppMgrAccess = req.AppMgrAccess,
                Command = command, Name = name, Type = type,
                PromptId = value["promptId"], TargetUrl = value["targetUrl"], ParamField = value["paramField"], ExtraParams = value["extraParams"],
                RecordSource = source
            };
        }

        /// <summary>True when both lists have entries, the same number of them, and no stray trailing commas or spaces, so an append keeps the positions paired.</summary>
        public static bool ListsArePaired(string? names, string? sources)
        {
            var n = SplitList(names);
            var s = SplitList(sources);
            return n.Count > 0 && n.Count == s.Count && string.Join(",", n) == names && string.Join(",", s) == sources;
        }

        private static List<string> SplitList(string? list)
        {
            var text = Regex.Replace((list ?? "").Trim(), @"[,\s]+$", "");
            return text.Length == 0 ? new List<string>() : text.Split(',').ToList();
        }

        /// <summary>Appends the name to the option list and its record source to the paired source list; a short source list is padded first.</summary>
        public static (string Names, string Sources) AppendOption(string? names, string? sources, string name, string? recordSource)
        {
            var src = Clean(recordSource);
            if (src.Length == 0) src = NoSource;
            if (!Regex.IsMatch(src, "^[A-Za-z0-9_]+$")) throw new CommandOptionException("The record source name is not valid.");
            var n = SplitList(names);
            var s = SplitList(sources);
            while (s.Count < n.Count) s.Add(NoSource);
            n.Add(name.Trim());
            s.Add(src);
            var outNames = string.Join(",", n);
            var outSources = string.Join(",", s);
            if (outNames.Length > MaxListLength || outSources.Length > MaxListLength)
                throw new CommandOptionException($"The option list is full ({MaxListLength} characters). Remove an option before adding another.", true);
            return (outNames, outSources);
        }
    }
}
