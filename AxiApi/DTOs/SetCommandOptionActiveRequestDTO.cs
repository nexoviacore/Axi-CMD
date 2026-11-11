namespace AxiApi.DTOs
{
    /// <summary>Body of a request that switches one command option active or inactive.</summary>
    public class SetCommandOptionActiveRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        public bool Active { get; set; }
        /// <summary>Configure Access of the caller; the write is refused unless this is true.</summary>
        public bool AppMgrAccess { get; set; }
        /// <summary>Build access of the caller; the write is also refused unless this is true.</summary>
        public bool BuildAccess { get; set; }
    }
}
