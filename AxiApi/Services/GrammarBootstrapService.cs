using System;
using ARMCommon.Interface;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Interfaces;
using AxiApi.Lib;
using AxiApi.Lib.Utils;
using AxiApi.Repositories;

using Newtonsoft.Json;
using StackExchange.Redis;

namespace AxiApi.Services;

public class GrammarBootstrapService: IGrammarBootstrapService
{
    

     private readonly IGrammarRepository _grammarRepository;
    private readonly IAxExtend _axExtend;
   
    private readonly ILogger<GrammarBootstrapService> _logger;



    //private ISubscriber? _subcriber;

    public GrammarBootstrapService(
         IGrammarRepository grammarRepository,
          IAxExtend axExtend,

        ILogger<GrammarBootstrapService> logger
       

    )
    {
         _grammarRepository = grammarRepository;
        _axExtend = axExtend; 
      
       
        _logger = logger;
        

    }

    public async Task<GrammarDTO> LoadGrammarAsync(string appname, bool forceRefresh)
    {
        _logger.LogInformation("Bootstrap: Starting Grammar Load sequence...");
        try
        {
            GrammarDTO grammar;
            GrammarCacheDTO? cachedGrammar = null;
            IRedisCacheHelper redisCache = null;
            string GrammarCacheKey = Keygenerator.GenerateCacheKey(appname, "", "commands"); 



    var redisConnected = await _axExtend.OpenRedisConnectionAsync(appname);
            RedisKey[] redisKeys = { 
                GrammarCacheKey
            }; 

            
            if (redisConnected)
            {
                redisCache = await _axExtend.GetRedis(); 
                try
                {
                    if (forceRefresh)
                    {
                        await redisCache.KeysDeleteAsync(redisKeys); 
                    }
                    string cacheGrammarString = await redisCache.StringGetAsync(GrammarCacheKey);
                    if (!string.IsNullOrEmpty(cacheGrammarString))
                           cachedGrammar = JsonConvert.DeserializeObject<GrammarCacheDTO>(cacheGrammarString);
                }
                catch (RedisException ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Bootstrap: Redis cache fetch failed. Falling back to Database."
                    );
                }

            }
           

            if (cachedGrammar is not null)
            {
                _logger.LogDebug("Bootstrap: Cache Hit. Building grammar from memory.");

                grammar = GrammarBuilder.Build(cachedGrammar.Commands, cachedGrammar.Prompts);

                return grammar; 
            }

            _logger.LogInformation("Bootstrap: Cache Miss. Fetching from Database...");

            

            var (commands, prompts) = await _grammarRepository.LoadGrammerAsync(appname);

            if (commands == null || !commands.Any())
            {
                _logger.LogError("Database returned 0 commands. Grammar cannot be initialized.");
                throw new InvalidOperationException(
                    "Database returned 0 commands. Grammar cannot be initialized."
                );
            }

            _logger.LogInformation(
                "Bootstrap: Loaded {CommandCount} commands from Database.",
                commands.Count
            );

            var cacheDTO = new GrammarCacheDTO { Commands = commands, Prompts = prompts };
            if (redisConnected)
            {
                try
                {

                    await redisCache?.StringSetAsync(GrammarCacheKey, JsonConvert.SerializeObject(cacheDTO), 7200);
                }
                catch (RedisException ex)
                {
                    _logger.LogWarning(ex, "Bootstrap: Failed to update Redis cache (Write failed).");
                }

            }
            

            grammar = GrammarBuilder.Build(commands, prompts);
            _logger.LogInformation("Bootstrap: Grammar successfully initialized.");

            return grammar; 

            

            
        }
        catch (Exception ex)
        {
            _logger.LogCritical(
                ex,
                "Bootstrap: CRITICAL FAILURE. Grammar could not be initialized."
            );
            return null; 
        } 
       
    }

    //public async Task StartAsync(CancellationToken cancellationToken)
    //{
    //    _logger.LogInformation("Service Starting: GrammarBootstrapService");

    //    try
    //    {
    //        //_subcriber = _redisMux.GetSubscriber();

    //        //await _subcriber.SubscribeAsync(
    //        //    RedisChannel.Literal(RedisChannels.GrammarInvalidation),
    //        //    async (_, message) =>
    //        //    {
    //        //        _logger.LogInformation(
    //        //            "Bootstrap: Invalidation received [{Message}]. Reloading...",
    //        //            message
    //        //        );
    //        //        await LoadGrammarAsync();
    //        //    }
    //        //);

    //        await LoadGrammarAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogCritical(
    //            ex,
    //            "Service Startup Failed: GrammarBootstrapService encountered a fatal error."
    //        );
    //    }
    //}

    //public  Task StopAsync(CancellationToken cancellationToken)
    //{
    //    _logger.LogInformation("Service Stopping: GrammarBootstrapService");
    //    return Task.CompletedTask; 

    //    //try
    //    //{
    //    //    //if (_subcriber is not null)
    //    //    //{
    //    //    //    await _subcriber.UnsubscribeAsync(
    //    //    //        RedisChannel.Literal(RedisChannels.GrammarInvalidation)
    //    //    //    );
    //    //    //}
    //    //    ; 
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    _logger.LogError(ex, "Error while stopping GrammarBootstrapService subscription.");
    //    //}
    //}
}
