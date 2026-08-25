using ARMCommon.Model;
using AxiApi.DTOs;

namespace AxiApi.Interfaces
{
    public interface IUserFavouriteService
    {
        Task<List<UserFavouritesDTO>> GetUserFavouritesByUsernameAsync(string username, string appname);
        Task<object> ToggleUserFavouritesAsync(UserFavouritesRequestDTO userFavouritesRequestDTO,  string appname);
        Task<NonQueryResult> UpdateCommandText(string favouritesId, UpdateUserFavouritesDTO requestDTO, string appname, string username); 


    }
}
