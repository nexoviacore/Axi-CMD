using AxiApi.DTOs;
using System.Threading.Tasks;

namespace AxiApi.Interfaces
{
    public interface IKeyfieldRepository
    {
        Task<bool> SetKeyfieldAsync(SetKeyfieldRequestDTO requestDTO);
    }
}
