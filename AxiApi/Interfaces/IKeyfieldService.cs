using AxiApi.DTOs;
using System.Threading.Tasks;

namespace AxiApi.Interfaces
{
    public interface IKeyfieldService
    {
        Task<ApiResponseDTO> SetKeyfieldAsync(string appname, SetKeyfieldRequestDTO requestDTO);
    }
}
