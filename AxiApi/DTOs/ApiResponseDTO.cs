using System.Net;

namespace AxiApi.DTOs
{
    public class ApiResponseDTO
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = new(); 


    }
}
