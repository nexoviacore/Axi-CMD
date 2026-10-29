using System;

namespace AxiApi.Lib
{
    /// <summary>
    /// Page and search rules of the paged option list on the AxiCMD Config page. Mirrors pageNumbers / rangeText in axiconfigpage.js.
    /// </summary>
    public static class CommandOptionPaging
    {
        public const int DefaultPageSize = 25;
        public const int MaxPageSize = 100;
        public const int MaxSearchLength = 200;

        public static int ClampPageSize(int? pageSize) =>
            pageSize is null || pageSize < 1 ? DefaultPageSize : Math.Min(pageSize.Value, MaxPageSize);

        /// <summary>At least one page, so an empty list still shows page 1 of 1.</summary>
        public static int TotalPages(int totalCount, int pageSize) =>
            Math.Max(1, (totalCount + pageSize - 1) / pageSize);

        /// <summary>A page past the end (for example after options were switched off elsewhere) gives the last page.</summary>
        public static int ClampPageNo(int? pageNo, int totalPages) =>
            Math.Min(Math.Max(1, pageNo ?? 1), totalPages);

        /// <summary>The trimmed search term, cut to MaxSearchLength; empty when there is none.</summary>
        public static string CleanSearch(string? search)
        {
            var s = (search ?? string.Empty).Trim();
            return s.Length > MaxSearchLength ? s.Substring(0, MaxSearchLength) : s;
        }

        /// <summary>The page's filter: "custom" (Custom Options only), "inactive" (switched off only) or "all" for anything else.</summary>
        public static string NormalizeShow(string? show)
        {
            var s = (show ?? string.Empty).Trim().ToLowerInvariant();
            return s == "custom" || s == "inactive" ? s : "all";
        }

        /// <summary>SQL condition for a normalized show value: fixed text, no parameters.</summary>
        public static string ShowFilter(string show) => show switch
        {
            "custom" => SqlQueries.CommandOptionsShowCustom,
            "inactive" => SqlQueries.CommandOptionsShowInactive,
            _ => string.Empty
        };

        /// <summary>The option to show after a save (a config_id, at most 50 characters); empty when there is none.</summary>
        public static string CleanFocus(string? focus)
        {
            var f = (focus ?? string.Empty).Trim();
            return f.Length > 50 ? string.Empty : f;
        }

        /// <summary>The page holding the row that has rowsBefore rows ahead of it.</summary>
        public static int PageOf(int rowsBefore, int pageSize) => rowsBefore / pageSize + 1;

        /// <summary>LIKE pattern for "contains", lower case. % and _ typed by the user match themselves (ESCAPE '\' in the SQL).</summary>
        public static string ContainsPattern(string search) =>
            "%" + search.ToLowerInvariant().Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_") + "%";
    }
}
