namespace AxiApi.DTOs
{
    /// <summary>Body of a request that saves one AxiCMD user setting.</summary>
    public class UserSettingRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
