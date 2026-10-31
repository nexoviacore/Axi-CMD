using System.Collections.Generic;

namespace AxiApi.DTOs
{
    /// <summary>One page of a command's options on the AxiCMD Config page, with the totals the tabs and the pager show.</summary>
    public class CommandOptionPageDTO
    {
        public List<CommandConfigOptionDTO> Items { get; set; } = new();
        /// <summary>Options of the command that match the search (all of them without a search).</summary>
        public int TotalCount { get; set; }
        /// <summary>The page returned; a page past the end is answered with the last page.</summary>
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        /// <summary>All Configure options, active or not, whatever the search (the tab counts).</summary>
        public int ConfigureCount { get; set; }
        /// <summary>All SDK options, active or not, whatever the search (the tab counts).</summary>
        public int SdkCount { get; set; }
    }
}
