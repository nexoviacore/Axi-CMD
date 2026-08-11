using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface IGrammarBootstrapService
    {
        Task<GrammarDTO> LoadGrammarAsync(string appname, bool forceRefresh); 
    }
}
