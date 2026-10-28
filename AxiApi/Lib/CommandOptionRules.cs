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

        /// <summary>Record sources already used by the Configure and SDK option lists; the page offers them after None. Mirrors KNOWN_RECORD_SOURCES in axiconfiglogic.js (other valid names are still accepted).</summary>
        public static readonly string[] KnownRecordSources =
        {
            "Axi_PegList", "Axi_FormNotifyList", "Axi_ScheduleNotifyList", "Axi_PEGNotifyList", "Axi_RuleNamesList",
            "axi_structmetalist", "axi_userlist", "axi_useractivation", "axi_usergrouplist", "axi_rolelist", "axi_actorlist",
            "axi_publishapi", "Axi_ServernameList", "axi_cardlist", "axi_resposibilitylist", "axi_dimensionlist",
            "axi_smartviewlist", "Axi_APINamesList", "axi_jobs", "axi_language", "axi_customtype", "axi_emaildef",
            "axi_tabledesc", "axi_outbound", "axi_inbound", "axi_getstructsdata", "axi_viewlist"
        };

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

        /// <summary>Validates an edit like a new option; the option's own current name is not a duplicate. The command comes from the stored row.</summary>
        public static CreateCommandOptionRequestDTO ValidateEdit(UpdateCommandOptionRequestDTO req, string command, string currentName, IEnumerable<string> existingNames)
        {
            var own = (currentName ?? "").Trim();
            var others = existingNames.Where(e => !string.Equals((e ?? "").Trim(), own, StringComparison.OrdinalIgnoreCase));
            return Validate(new CreateCommandOptionRequestDTO
            {
                Username = req.Username, AppMgrAccess = req.AppMgrAccess, Command = command, Name = req.Name, Type = req.Type,
                PromptId = req.PromptId, TargetUrl = req.TargetUrl, ParamField = req.ParamField, ExtraParams = req.ExtraParams, RecordSource = req.RecordSource
            }, others);
        }

        /// <summary>Replaces the entry oldName in the option list by newName and the source at the same position by recordSource; order and pairing are kept. Mirrors replaceOption in axiconfiglogic.js.</summary>
        public static (string Names, string Sources) ReplaceOption(string? names, string? sources, string oldName, string newName, string? recordSource)
        {
            var src = Clean(recordSource);
            if (src.Length == 0) src = NoSource;
            if (!Regex.IsMatch(src, "^[A-Za-z0-9_]+$")) throw new CommandOptionException("The record source name is not valid.");
            var n = SplitList(names);
            var s = SplitList(sources);
            var at = n.FindIndex(x => string.Equals(x.Trim(), oldName.Trim(), StringComparison.OrdinalIgnoreCase));
            if (at < 0) throw new CommandOptionException($"The option \"{oldName.Trim()}\" is not in the command's option list.", true);
            while (s.Count < n.Count) s.Add(NoSource);
            n[at] = newName.Trim();
            s[at] = src;
            var outNames = string.Join(",", n);
            var outSources = string.Join(",", s);
            if (outNames.Length > MaxListLength || outSources.Length > MaxListLength)
                throw new CommandOptionException($"The option list is full ({MaxListLength} characters). Shorten the name or remove an option.", true);
            return (outNames, outSources);
        }

        /// <summary>True when the request would change the route (name, type, target, fields or record source) of the stored option.</summary>
        public static bool ChangesRoute(UpdateCommandOptionRequestDTO req, string name, string type, string? promptId, string? targetUrl, string? paramField, string? extraParams, string? recordSource)
        {
            static string C(string? v) => (v ?? "").Trim();
            static string S(string? v) => C(v).Length == 0 ? NoSource : C(v);
            return C(req.Name) != C(name) || !string.Equals(C(req.Type), C(type), StringComparison.OrdinalIgnoreCase)
                || C(req.PromptId) != C(promptId) || C(req.TargetUrl) != C(targetUrl)
                || C(req.ParamField) != C(paramField) || C(req.ExtraParams) != C(extraParams)
                || !string.Equals(S(req.RecordSource), S(recordSource), StringComparison.OrdinalIgnoreCase);
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
