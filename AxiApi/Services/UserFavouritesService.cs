using ARMCommon.Helpers;
using ARMCommon.Interface;
using ARMCommon.Model;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Enums;
using AxiApi.Exceptions;
using AxiApi.Interfaces;
using AxiApi.Lib.Utils;
using Newtonsoft.Json;
using Npgsql;
using StackExchange.Redis;

namespace AxiApi.Services
{
    public class UserFavouritesService : IUserFavouriteService
    {
        private readonly ILogger<UserFavouritesService> _logger; 
        private readonly IUserFavouritesRepository _userFavouritesRepository;
        private readonly IAxExtend _axExtend;

        public UserFavouritesService(ILogger<UserFavouritesService> logger, IUserFavouritesRepository userFavouritesRepository, IAxExtend axExtend)
        {
            _logger = logger;
            _userFavouritesRepository = userFavouritesRepository;
            _axExtend = axExtend;


        }

        public async Task<List<UserFavouritesDTO>> GetUserFavouritesByUsernameAsync(string username, string appname)
        {
            

            //string axiUserFavouritesCacheKey = $"axi_{appname}_userfavourites_{username}"; 
            string axiUserFavouritesCacheKey = Keygenerator.GenerateCacheKey(appname, "userfavourites", username);

            var redisConnected = await _axExtend.OpenRedisConnectionAsync(appname);

            List<UserFavouritesDTO> userFavouritesDTOs = new(); 


           _logger.LogInformation($"Fetching UserFavourites for Username: {username}"); 
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogError("Username is Invalid"); 
                throw new ArgumentException("Username is Invalid"); 
            }

            if (redisConnected)
            {
                _logger.LogInformation("Redis Connected. Checking UserFavourites in Redis Cache....");
                var redisCache = await _axExtend.GetRedis(); 
                var cached = await redisCache.StringGetAsync(axiUserFavouritesCacheKey);
                
                if (!string.IsNullOrEmpty(cached))
                {
                    _logger.LogInformation("Redis Cache found!. Retriving from Cache");

                    userFavouritesDTOs = JsonConvert.DeserializeObject<List<UserFavouritesDTO>>(cached);
                    if (userFavouritesDTOs?.Count == 0)
                    {
                        _logger.LogInformation($"UserFavourites for Username: {username} was not found!");

                        

                    }
                    _logger.LogDebug($"Retrived UserFavourites for Username: {username} | UserFavourites: {JsonConvert.SerializeObject(userFavouritesDTOs)}");

                   


                } else
                {
                    _logger.LogInformation("Redis Cache not found Fallback to DB"); 
                    userFavouritesDTOs = await _userFavouritesRepository.GetUserFavouritesByUsername(username, appname);
                    try
                    {
                        await redisCache.StringSetAsync(axiUserFavouritesCacheKey, JsonConvert.SerializeObject(userFavouritesDTOs), 7200); 
                    } catch(RedisException ex)
                    {
                        throw new RedisCacheConnectionException(ex.Message); 
                    }
                    if (userFavouritesDTOs?.Count == 0)
                    {
                        _logger.LogInformation($"UserFavourites for Username: {username} was not found!");



                    }
                    _logger.LogDebug($"Retrived UserFavourites for Username: {username} | UserFavourites: {JsonConvert.SerializeObject(userFavouritesDTOs)}");



                }

            } else
            {
                _logger.LogError("Redis did not connected");
                throw new RedisCacheConnectionException(); 
            }

            return userFavouritesDTOs ?? new List<UserFavouritesDTO>();



        }

        public async Task<object> ToggleUserFavouritesAsync(UserFavouritesRequestDTO requestDTO,  string appname)
        {
            if (requestDTO == null)
            {
                _logger.LogError("Invalid request!");

                throw new ArgumentException(nameof(requestDTO)); 
            }

            if (!Enum.TryParse<FavouritesAction>(requestDTO.Action, true, out var action)) {
                _logger.LogError("Invalid action!");

                throw new ArgumentException("Action is Invalid");

            }

          
            if (string.IsNullOrEmpty(appname))
            {
                _logger.LogError("Invalid appname!");

                throw new ArgumentException(nameof(appname));
            }

           

            var redisConnected = await _axExtend.OpenRedisConnectionAsync(appname);
            //string axiUserFavouritesCacheKey = $"axi_{appname}_userfavourites_{requestDTO.Username}";
            string axiUserFavouritesCacheKey = Keygenerator.GenerateCacheKey(appname, "userfavourites", requestDTO.Username);





            //NonQueryResult nonQueryResult = new NonQueryResult();
            object result = new(); 

            UserFavouritesDTO userFavouritesDTO = new UserFavouritesDTO
            {
                Username = requestDTO.Username,
                CommandText = requestDTO.CommandText,
                OriginalCommandText = requestDTO.OriginalCommandText,
                FavOrder = requestDTO.FavOrder,
                TargetURL = requestDTO.TargetURL

            }; 



            if (redisConnected)
            {
                var redisCache = await _axExtend.GetRedis(); 
                switch (action)
                {
                    case FavouritesAction.Add:
                        _logger.LogInformation($"Adding UserFavourites for Username: {requestDTO.Username}");

                       List<UserFavouritesDTO> userFavouritesDTOs  = await _userFavouritesRepository.CreateUserFavourites(userFavouritesDTO, appname);
                        //if (!string.IsNullOrEmpty(nonQueryResult.error))
                        //{
                        //    throw new DatabaseException(nonQueryResult.error,"Insert");

                        //}

                        await redisCache.KeyDeleteAsync(axiUserFavouritesCacheKey);

                        _logger.LogInformation($"UserFavourites added successfully for Username: {requestDTO.Username}");
                        result = userFavouritesDTOs; 
                        break;
                    case FavouritesAction.Remove:
                        _logger.LogInformation($"Deleting UserFavourites for Username: {requestDTO.Username}");

                        NonQueryResult deleteResult = await _userFavouritesRepository.DeleteUserFavouritesByCmd(userFavouritesDTO, appname);
                        if (!string.IsNullOrEmpty(deleteResult?.error))
                        {
                           throw new DatabaseException(deleteResult.error,"Delete");

                        }
                        await redisCache.KeyDeleteAsync(axiUserFavouritesCacheKey); 
                        _logger.LogInformation($"UserFavourites Deleted successfully for Username: {requestDTO.Username} | Deleted command: {requestDTO.CommandText}");
                        result = deleteResult!; 
                        break;

                    default:
                        break;
                }

            }

           



            return result; 

        }

        public async Task<NonQueryResult> UpdateCommandText(string favouritesId,UpdateUserFavouritesDTO requestDTO, string appname, string username)
        {
            //if (favouritesId == null)
            //{
            //    throw new ArgumentException("Favourties id cannot be empty!"); 

            //}

            if (requestDTO.CommandText == null)
            {
                throw new ArgumentException("command text cannot be empty!");

            }

            if (appname == null)
            {
                throw new ArgumentException("appname cannot be empty!");

            }

            // if (Guid.TryParse(favouritesId, out Guid favouritesGUID))
            // {
            //     // use resul
            // }
            // else
            // {
            //     // handle invalid GUID
            // }

            string axiUserFavouritesCacheKey = Keygenerator.GenerateCacheKey(appname, "userfavourites", username);

            var redisConnected = await _axExtend.OpenRedisConnectionAsync(appname); 

            if (redisConnected)
            {
                var redisCache = await _axExtend.GetRedis();

                await redisCache.KeyDeleteAsync(axiUserFavouritesCacheKey);
                _logger.LogInformation("Update: Redis cache has been removed while updating"); 
            } else
            {
                _logger.LogWarning("Redis is Not Connected"); 
            }

            NonQueryResult nonQueryResult = await _userFavouritesRepository.UpdateUserFavourties(favouritesId, appname, requestDTO.CommandText); 

            if (!string.IsNullOrEmpty(nonQueryResult.error))
            {
                throw new DatabaseException(nonQueryResult.error, "Update"); 
            }

            return nonQueryResult; 





        }

        //private UserFavouritesDTO CheckAccessPermissionForUserFavourites(UserFavouritesDTO userFavouritesDTO, AccessPermissionsDTO accessPermissionsDTO)
        //{

        //}
    }
}
