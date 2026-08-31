using System.ComponentModel.DataAnnotations;

namespace AxiApi.DTOs
{
    public class SetKeyfieldRequestDTO
    {
        [Required]
        public string TransId { get; set; } = string.Empty;

        [Required]
        public string KeyField { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;
    }
}
